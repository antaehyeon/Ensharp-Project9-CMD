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
            bool containSemi = false;

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



    }
}
