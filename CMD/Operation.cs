using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


namespace CMD
{
    class Operation
    {
        Print p;
        DirectoryInfo currentDirectory;

        List<ObjectVO> objects;

        string instruction = "";
        string currentPath = "";

        const int COPY = 1;
        const int MOVE = 2;

        // 명령어가 한번이라도 입력되면 true
        bool instructionCheck = false;
        // cls 명령어가 입력되었을 경우
        bool clsCheck = false;
        // cd 명령어가 한번이라도 입력되었을 경우
        bool cdCheck = false;

        public Operation()
        {
            p = new Print();

            objects = new List<ObjectVO>();
        }

        public void start()
        {
            // CMD 창 처음 켰을 때 문구를 보여준다
            if(!instructionCheck) p.startSentence();
            // cls가 입력되어서 초기화되었을 경우 출력화면을 위해
            if (clsCheck)
            {
                clsCheck = false;
                if (cdCheck) return;
                Console.WriteLine();
            }
            if (cdCheck)
            {
                Console.WriteLine();
            }
            // 유저 기본 경로 받아오기
            if (!cdCheck) currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.Write("{0}>", currentPath);

            currentDirectory = new DirectoryInfo(currentPath);

            instruction = Console.ReadLine();

            instruction = instruction.Trim();
            instruction = instruction.ToLower();

            // 문자열을 공백기준으로 분할
            string[] SpaceDivision = instruction.Split(' ');

            instructionCheck = true;

            switch(SpaceDivision[0])
            {
                case "help":
                    p.basicHelp();
                    break;
                case "cls":
                    Console.Clear();
                    clsCheck = true;
                    break;
                case "dir":
                    excuteDir(instruction);
                    break;
                case "cd":
                    cd(instruction);
                    break;
                case "cd\\":
                    currentPath = rootDirectory(currentPath);
                    cdCheck = true;
                    break;
                case "copy":
                    //copyAndMove(instruction, COPY);
                    copyMove2(instruction, COPY);
                    break;
                case "move":
                    copyMove2(instruction, MOVE);
                    break;
                default:
                    driveCheck(SpaceDivision[0]);
                    break;
            }
        }

        public void excuteDir(string instruction)
        {
            int count = 0;

            string[] SpaceDivision = instruction.Split(' ');

            foreach(var str in SpaceDivision)
            {
                if (str.Length > 0) count++;
            }

            if(count.Equals(1))
            {
                dir(currentPath);
            }
            else
            {
                if (existPath(SpaceDivision[1]))
                {
                    dir(SpaceDivision[1]);
                }
                else
                {
                    Console.WriteLine("\n파일을 찾을 수 없습니다.\n");
                }
            }
        }

        /* DIR */
        public void dir(string drivePath)
        {
            DriveInfo driveInfo = new DriveInfo(drivePath);
            DirectoryInfo directoryInfo = new DirectoryInfo(drivePath);

            // 드라이브 볼륨의 이름 유/무
            if (driveInfo.VolumeLabel.Equals(""))
            {
                Console.WriteLine(" C 드라이브의 볼륨에는 이름이 없습니다.");
            }
            else
            {
                Console.WriteLine(" C 드라이브의 볼륨 : {0}", driveInfo.VolumeLabel);
            }
            // 해당부분은 고정시켜버림
            Console.WriteLine(" 볼륨 일련 번호: 1EFC-B5DE\n");
            Console.WriteLine(" {0} 디렉터리\n", drivePath);

            // 현재 폴더에 속한 모든 파일명을 가져온다
            FileSystemInfo[] currentFolderObject = directoryInfo.GetFileSystemInfos();

            string objectAccessDate = "";
            string objectType = "";
            string fileSize = "";
            string objectName = "";
            string strDriveTotalSize = "";
            string strDriveFreeSize = "";

            int fileCount = 0;
            int directoryCount = 0;

            long fileTotalSize = 0;

            // 각각의 객체를 처리
            foreach(var item in currentFolderObject)
            {
                // 숨김파일의 경우 나타내지 않음
                if (item.Attributes.ToString().Contains("Hidden")) continue;

                objectAccessDate = item.LastWriteTime.ToString(); // 객체 액세스 날짜/시간정보
                objectType = item.Attributes.ToString(); // 객체 타입정보 (폴더인지,파일인지)

                // 파일
                if (objectType.Contains("Archive"))
                {
                    fileCount++; // 파일의 총 갯수
                    objectType = "Archive";
                    var fullPath = item.FullName;
                    FileInfo file = new FileInfo(fullPath);
                    fileTotalSize += file.Length; // 나중에 파일의 총 크기를 나타내기 위함
                    fileSize = file.Length.ToString();
                }
                // 디렉토리
                else
                {
                    directoryCount++; // 디렉토리 총 갯수
                    objectType = "Directory";
                    fileSize = "";
                }

                objectName = item.Name.ToString(); // 파일이름

                // 객체에 정보를 담음
                ObjectVO newObject = new ObjectVO(objectAccessDate, objectType, fileSize, objectName);
                p.dir(newObject); // 출력
            }

            // 간격에 맞춰서 콤마형식으로 변환
            strDriveTotalSize = fileTotalSize.ToString("#,##0");
            strDriveFreeSize = driveInfo.TotalFreeSpace.ToString("#,##0");

            // 마지막 사이즈 결과 출력
            Console.WriteLine("{0,13}개 파일       {1,13} 바이트", fileCount, strDriveTotalSize);
            Console.WriteLine("{0,13}개 디렉터리  {1,13} 바이트 남음\n", directoryCount, strDriveFreeSize);
        }

        public string rootDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.Root.ToString();
        }

        public void cd(string instruction)
        {
            bool onlyComma = false;
            bool directoryExist = false;

            string setPath = "";

            cdCheck = true;

            string[] SpaceDivision = instruction.Split(' ');

            foreach(string str in SpaceDivision)
            {
                if(str.Equals("..\\.."))
                {
                    currentPath = rootDirectory(currentPath);
                    return;
                }
                else if (str.Equals(".."))
                {
                    onlyComma = true;
                }
                else if (str.Length >= 1)
                {
                    if (onlyComma)
                    {
                        onlyComma = notExistPath(str);
                        if (!onlyComma) return;
                    }
                    else
                    {
                        if (existPath(str))
                        {
                            directoryExist = true;
                            DirectoryInfo forPath = new DirectoryInfo(str);
                            setPath = forPath.FullName;
                        } 
                        else
                        {
                            directoryExist = false;
                        }
                    }
                }
            }



            if (onlyComma)
            {
                currentPath = currentDirectory.Parent.FullName.ToString();
            }
            else if (directoryExist)
            {
                currentPath = setPath;
            }
            else if (!directoryExist)
            {
                notExistPath(instruction);
            }
        }

        public bool existPath(string path)
        {
            int drivePathExist = path.ToLower().IndexOf("c:\\");
            
            if (drivePathExist.Equals(0))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                return directory.Exists;
            }
            else
            {
                return false;
            }
        }

        public bool notExistPath(string str)
        {
            if (str.Contains(":"))
            {
                if (str.Contains("c:\\"))
                {
                    Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
                    return false;
                }
                Console.WriteLine("파일 이름, 디렉터리 이름 또는 볼륨 레이블 구문이 잘못되었습니다.");
            }
            else Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
            return false;
        }

        public void driveCheck(string path)
        {
            var driveList = DriveInfo.GetDrives();

            if (Regex.IsMatch(path, "^[a-z]+:$"))
            {
                foreach(var str in driveList)
                {
                    string driveName = str.Name.ToString().ToLower();
                    driveName = driveName.Replace("\\", "");

                    if(driveName.Equals(path))
                    {
                        if (currentPath.Contains(path)) return;
                        currentPath = path;
                        Console.WriteLine();
                        return;
                    }
                }
                Console.WriteLine("시스템이 지정된 드라이브를 찾을 수 없습니다.\n");
            }
            else
            {
                Console.WriteLine("'{0}'은(는) 내부 또는 외부 명령, 실행할 수 있는 프로그램, 또는", path);
                Console.WriteLine("배치 파일이 아닙니다.\n");
            }
        }

        // 현재경로와 다른곳에서 copy를 진행할 경우
        // 경로를 하나만 쓸경우, 쓴곳의 파일을 현재 폴더로 붙여넣는다
        // 경로에서 이미 해당파일의 경로를 하나만 쓸경우 예외처리
        // 경로에서 하나만 썼는데, 없는 파일일 경우 예외처리

        public void copyAndMove(string instruction, int mode)
        {
            int count = 0;

            string firstFile = "";
            string secondFile = "";

            string choice = "";

            string[] combineSpace = instruction.Split(' ');

            foreach(string str in combineSpace)
            {
                if (str.Length > 0) count++;
            }

            if(count.Equals(3))
            {
                firstFile = existFile(combineSpace[1]);
                secondFile = existFile(combineSpace[2]);

                if (string.IsNullOrWhiteSpace(firstFile))
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    return;
                }
                // 공백이면 파일이 존재하지 않는다
                if (string.IsNullOrWhiteSpace(secondFile))
                {
                    string targetPath = "";
                    FileInfo originFile = new FileInfo(firstFile);
                    // 해당경로에 파일이 존재하지 않는다면
                    if (!existPath(combineSpace[2]))
                    {
                        targetPath = existFile(combineSpace[2]);
                        //targetPath = combineSpace[2];
                        //originFile.CopyTo(targetPath, true);
                        //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                        copyAndPasteFunction(originFile, targetPath, mode, 1);
                    }
                    else
                    {
                        targetPath = currentPath + "\\" + combineSpace[2];
                    }

                    if (targetPath.Equals(""))
                    {
                        //originFile.CopyTo(targetPath, true);
                        //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                        copyAndPasteFunction(originFile, targetPath, mode, 1);
                    }
                }
                else
                {
                    FileInfo originFile = new FileInfo(firstFile);

                    Console.Write("{0}을(를) 덮어쓰시겠습니까? (Yes/No/All): ", combineSpace[2]);
                    choice = Console.ReadLine();

                    if (mode.Equals(MOVE))
                    {
                        FileInfo destination = new FileInfo(secondFile);
                        destination.Delete();
                    }

                    switch (choice.ToLower())
                    {
                        case "yes":
                        case "ye":
                        case "y":
                            //originFile.CopyTo(secondFile, true);
                            //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                            copyAndPasteFunction(originFile, secondFile, mode, 1);
                            break;
                        case "no":
                        case "n":
                            if (mode.Equals(COPY)) Console.WriteLine("\t\t0개 파일이 복사되었습니다.\n");
                            else Console.WriteLine("\t\t0개 파일을 이동했습니다.\n");
                            break;
                        case "all":
                            //originFile.CopyTo(secondFile, true);
                            //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                            copyAndPasteFunction(originFile, secondFile, mode, 1);
                            break;
                    }
                }
            }
            else if (count.Equals(2))
            {
                string destinationPath = "";

                firstFile = existFile(combineSpace[1]);
                destinationPath = currentDirectory.FullName;

                FileInfo file = new FileInfo(firstFile);
                destinationPath = destinationPath + "\\" + file.Name;

                secondFile = existFile(destinationPath);

                // 만약 존재하지 않는 파일을 복사하려고 한다면
                if (firstFile.Equals(""))
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    return;
                }
                else if (secondFile.Equals(""))
                {
                    //file.CopyTo(destinationPath, true);
                    //Console.WriteLine("\t\t1개 파일이 복사되었습니다.");
                    copyAndPasteFunction(file, secondFile, mode, 1);
                }
                else
                {
                    Console.Write("{0}을(를) 덮어쓰시겠습니까? (Yes/No/All): ", secondFile);
                    choice = Console.ReadLine();

                    if (mode.Equals(MOVE))
                    {
                        FileInfo destination = new FileInfo(secondFile);
                        destination.Delete();
                    }

                    switch (choice.ToLower())
                    {
                        case "yes":
                        case "ye":
                        case "y":
                            //file.CopyTo(secondFile, true);
                            //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                            copyAndPasteFunction(file, secondFile, mode, 1);
                            break;
                        case "no":
                        case "n":
                            if (mode.Equals(COPY)) Console.WriteLine("\t\t0개 파일이 복사되었습니다.\n");
                            else Console.WriteLine("\t\t0개 파일을 이동했습니다.\n");
                            break;
                        case "all":
                            //file.CopyTo(secondFile, true);
                            //Console.WriteLine("\t\t1개 파일이 복사되었습니다.\n");
                            copyAndPasteFunction(file, secondFile, mode, 1);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("명령 구문이 올바르지 않습니다.\n");
                return;
            }
        }

        public void copyMove2(string instruction, int mode)
        {
            int count = 0;

            string startFileInfo = "";
            string destinationFileInfo = "";
            // 해당 명령문이 경로인지, 파일명인지 체크하기 위함
            bool firstInstruction = false;
            bool secondInstruction = false;

            const int NODUPLICATION = -1;

            string choice = "";

            string[] combineSpace = instruction.Split(' ');

            foreach (string str in combineSpace)
            {
                if (str.Length > 0) count++;
            }

            if(count.Equals(3))
            {
                // 해당 명령문이 파일명인지 경로인지 체크한다음에
                firstInstruction = pathCheck(combineSpace[1]);
                secondInstruction = pathCheck(combineSpace[2]);

                // 둘다 파일명일 경우
                if (!firstInstruction && !secondInstruction)
                {
                    startFileInfo = currentPath + "\\" + combineSpace[1];
                    destinationFileInfo = currentPath + "\\" + combineSpace[2];
                }
                // 앞에만 경로일 경우
                else if (firstInstruction && !secondInstruction)
                {
                    startFileInfo = combineSpace[1];
                    destinationFileInfo = currentPath + "\\" + combineSpace[2];
                }
                // 앞에가 파일명 뒤에가 경로일 경우
                else if (!firstInstruction && secondInstruction)
                {
                    startFileInfo = currentPath + "\\" + combineSpace[1];
                    destinationFileInfo = combineSpace[2];
                }
                // 둘다 경로일 경우
                else if (firstInstruction && secondInstruction)
                {
                    startFileInfo = combineSpace[1];
                    destinationFileInfo = combineSpace[2];
                }

                FileInfo startFile = new FileInfo(startFileInfo);
                FileInfo destinationFile = new FileInfo(destinationFileInfo);

                // DirectoryInfo
                DirectoryInfo startDirectory = new DirectoryInfo(startFile.Directory.ToString());
                DirectoryInfo destinationDirectory = new DirectoryInfo(destinationFile.Directory.ToString());

                // 각각 폴더에 있는 파일들을 전부 끌어옴
                FileInfo[] startDirectoryItems = startDirectory.GetFiles();
                FileInfo[] destinationDirectoryItems = destinationDirectory.GetFiles();

                // 해당 폴더에 파일이 없다면
                if(!existFile2(startDirectoryItems, startFile.Name))
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    return;
                }
                // 해당 폴더에 같은이름의 파일이 이미 있다면
                if(existFile2(destinationDirectoryItems, destinationFile.Name))
                {
                    Console.Write("{0}을(를) 덮어쓰시겠습니까? (Yes/No/All): ", destinationFile.FullName);
                    string answer = Console.ReadLine();
                    answer = answer.ToLower();
                    switch (answer)
                    {
                        case "yes":
                        case "ye":
                        case "y":
                        case "all":
                            destinationFile.Delete();
                            copyAndPasteFunction(startFile, destinationFileInfo, mode, 1);
                            break;
                        case "no":
                        case "n":
                            copyAndPasteFunction(null, null, mode, NODUPLICATION);
                            break;
                    }
                    return;
                }
                copyAndPasteFunction(startFile, destinationFileInfo, mode, 1);
            }
            else if(count.Equals(2))
            {
                startFileInfo = combineSpace[1];

                FileInfo startFile = new FileInfo(startFileInfo);

                destinationFileInfo = currentPath + "\\" + startFile.Name;

                FileInfo destinationFile = new FileInfo(destinationFileInfo);

                DirectoryInfo destinationDirectory = new DirectoryInfo(destinationFile.Directory.ToString());

                FileInfo[] destinationDirectoryItems = destinationDirectory.GetFiles();

                if (existFile2(destinationDirectoryItems, destinationFile.Name))
                {
                    Console.Write("{0}을(를) 덮어쓰시겠습니까? (Yes/No/All): ", destinationFile.FullName);
                    string answer = Console.ReadLine();
                    answer = answer.ToLower();
                    switch (answer)
                    {
                        case "yes":
                        case "ye":
                        case "y":
                        case "all":
                            destinationFile.Delete();
                            copyAndPasteFunction(startFile, destinationFileInfo, mode, 1);
                            break;
                        case "no":
                        case "n":
                            copyAndPasteFunction(null, null, mode, NODUPLICATION);
                            break;
                    }
                    return;
                }
                copyAndPasteFunction(startFile, destinationFileInfo, mode, 1);
            }
            else
            {
                Console.WriteLine("명령 구문이 올바르지 않습니다.");
            }
        }


        public string checkPath(string str)
        {
            string path = "";

            if (str.Contains("c:\\"))
            {
                DirectoryInfo directory = new DirectoryInfo(str);
                path = directory.Parent.FullName.ToString();
                return path;
            }
            else
            {
                //currentDirectoryFileList = currentDirectory.GetFiles();
                return currentDirectory.FullName.ToString();
            }
        }

        public bool pathCheck(string str)
        {
            str = str.ToLower();
            // 해당 명령문이 경로일경우
            if (str.Contains("c:\\")) { return true; }
            else { return false; }
        }

        public string existFile(string str)
        {
            FileInfo[] currentDirectoryFileList;

            DirectoryInfo directory = new DirectoryInfo(checkPath(str));
            currentDirectoryFileList = directory.GetFiles();

            foreach (var fileName in currentDirectoryFileList)
            {
                if (fileName.Name.ToString().Equals(str))
                {
                    return fileName.FullName;
                }
                if (fileName.FullName.ToString().Equals(str))
                {
                    return fileName.FullName;
                }
            }
            return "";
        }

        // 해당 폴더에 파일이 존재하는지 체크
        public bool existFile2(FileInfo[] items, string fileName)
        {
            foreach(var str in items)
            {
                if (fileName.Equals(str.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public void copyAndPasteFunction(FileInfo file, string path, int mode, int num)
        {
            if (mode.Equals(COPY))
            {
                if (num.Equals(-1))
                {
                    Console.WriteLine("\t\t0개 파일이 복사되었습니다/");
                    return;
                }
                file.CopyTo(path, true);
                Console.WriteLine("\t\t{0}개 파일이 복사되었습니다.", num);
            }
            else if(mode.Equals(MOVE))
            {
                if (num.Equals(-1))
                {
                    Console.WriteLine("\t\t0개 파일이 이동했습니다.");
                    return;
                }
                file.MoveTo(path);
                Console.WriteLine("\t\t{0}개 파일이 이동했습니다.", num);
            }
        }


    }
}
