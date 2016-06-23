using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CMD
{
    class Print
    {

        public Print()
        {

        }


        public void startSentence()
        {
            Console.WriteLine("Microsoft Windows [Version 10.0.10586]");
            Console.WriteLine("(c) 2015 Microsoft Corporation. All rights reserved.");
            Console.WriteLine();
        }

        public void basicHelp()
        {
            Console.WriteLine("특정 명령어에 대한 자세한 내용이 필요하면 HELP 명령어 이름을 입력하십시오.");
            Console.WriteLine("ASSOC    파일 확장명 연결을 보여주거나 수정합니다.");
            Console.WriteLine("ATTRIB   파일 속성을 표시하거나 바꿉니다.");
            Console.WriteLine("BREAK    확장된 CTRL+C 검사를 설정하거나 지웁니다.");
            Console.WriteLine("BCDEDIT        부팅 로딩을 제어하기 위해 부팅 데이터베이스에서 속성을 설정합니다.");
            Console.WriteLine("CACLS    파일의 액세스 컨트롤 목록(ACL)을 표시하거나 수정합니다.");
            Console.WriteLine("CALL     한 일괄 프로그램에서 다른 일괄 프로그램을 호출합니다.");
            Console.WriteLine("CD       현재 디렉터리 이름을 보여주거나 바꿉니다.");
            Console.WriteLine("CHCP     활성화된 코드 페이지의 번호를 표시하거나 설정합니다.");
            Console.WriteLine("CHDIR    현재 디렉터리 이름을 보여주거나 바꿉니다.");
            Console.WriteLine("CHKDSK   디스크를 검사하고 상태 보고서를 표시합니다.");
            Console.WriteLine("CHKNTFS  부팅하는 동안 디스크 확인을 화면에 표시하거나 변경합니다.");
            Console.WriteLine("CLS      화면을 지웁니다.");
            Console.WriteLine("CMD      Windows 명령 인터프리터의 새 인스턴스를 시작합니다.");
            Console.WriteLine("COLOR    콘솔의 기본색과 배경색을 설정합니다.");
            Console.WriteLine("COMP     두 개 또는 여러 개의 파일을 비교합니다.");
            Console.WriteLine("COMPACT  NTFS 분할 영역에 있는 파일의 압축을 표시하거나 변경합니다.");
            Console.WriteLine("CONVERT  FAT 볼륨을 NTFS로 변환합니다. 현재 드라이브는");
            Console.WriteLine("         변환할 수 없습니다.");
            Console.WriteLine("COPY     하나 이상의 파일을 다른 위치로 복사합니다.");
            Console.WriteLine("DATE     날짜를 보여주거나 설정합니다.");
            Console.WriteLine("DEL      하나 이상의 파일을 지웁니다.");
            Console.WriteLine("DIR      디렉터리에 있는 파일과 하위 디렉터리 목록을 보여줍니다.");
            Console.WriteLine("DISKPART       디스크 파티션 속성을 표시하거나 구성합니다.");
            Console.WriteLine("DOSKEY       명령줄을 편집하고, Windows 명령을 다시 호출하고,");
            Console.WriteLine("               매크로를 만듭니다.");
            Console.WriteLine("DRIVERQUERY    현재 장치 드라이버 상태와 속성을 표시합니다.");
            Console.WriteLine("ECHO           메시지를 표시하거나 ECHO를 켜거나 끕니다.");
            Console.WriteLine("ENDLOCAL       배치 파일에서 환경 변경의 지역화를 끝냅니다.");
            Console.WriteLine("ERASE          하나 이상의 파일을 지웁니다.");
            Console.WriteLine("EXIT           CMD.EXE 프로그램(명령 인터프리터)을 종료합니다.");
            Console.WriteLine("FC             두 파일 또는 파일 집합을 비교하여 다른 점을");
            Console.WriteLine("         표시합니다.");
            Console.WriteLine("FIND           파일에서 텍스트 문자열을 검색합니다.");
            Console.WriteLine("FINDSTR        파일에서 문자열을 검색합니다.");
            Console.WriteLine("FOR            파일 집합의 각 파일에 대해 지정된 명령을 실행합니다.");
            Console.WriteLine("FORMAT         Windows에서 사용할 디스크를 포맷합니다.");
            Console.WriteLine("FSUTIL         파일 시스템 속성을 표시하거나 구성합니다.");
            Console.WriteLine("FTYPE          파일 확장명 연결에 사용되는 파일 형식을 표시하거나");
            Console.WriteLine("               수정합니다.");
            Console.WriteLine("GOTO           Windows 명령 인터프리터가 일괄 프로그램에서");
            Console.WriteLine("               이름표가 붙여진 줄로 이동합니다.");
            Console.WriteLine("GPRESULT       컴퓨터 또는 사용자에 대한 그룹 정책 정보를 표시합니다.");
            Console.WriteLine("GRAFTABL       Windows가 그래픽 모드에서 확장 문자 세트를 표시할");
            Console.WriteLine("         수 있게 합니다.");
            Console.WriteLine("HELP           Windows 명령에 대한 도움말 정보를 제공합니다.");
            Console.WriteLine("ICACLS         파일과 디렉터리에 대한 ACL을 표시, 수정, 백업 또는");
            Console.WriteLine("복원합니다.");
            Console.WriteLine("IF             일괄 프로그램에서 조건 처리를 수행합니다.");
            Console.WriteLine("LABEL          디스크의 볼륨 이름을 만들거나, 바꾸거나, 지웁니다.");
            Console.WriteLine("MD             디렉터리를 만듭니다.");
            Console.WriteLine("MKDIR          디렉터리를 만듭니다.");
            Console.WriteLine("MKLINK         바로 가기 링크와 하드 링크를 만듭니다.");
            Console.WriteLine("MODE           시스템 장치를 구성합니다.");
            Console.WriteLine("MORE           출력을 한번에 한 화면씩 표시합니다.");
            Console.WriteLine("MOVE           하나 이상의 파일을 한 디렉터리에서 다른 디렉터리로");
            Console.WriteLine("               이동합니다.");
            Console.WriteLine("OPENFILES      파일 공유에서 원격 사용자에 의해 열린 파일을 표시합니다.");
            Console.WriteLine("PATH           실행 파일의 찾기 경로를 표시하거나 설정합니다.");
            Console.WriteLine("PAUSE          배치 파일의 처리를 일시 중단하고 메시지를 표시합니다.");
            Console.WriteLine("POPD           PUSHD에 의해 저장된 현재 디렉터리의 이전 값을");
            Console.WriteLine("               복원합니다.");
            Console.WriteLine("PRINT          텍스트 파일을 인쇄합니다.");
            Console.WriteLine("PROMPT         Windows 명령 프롬프트를 변경합니다.");
            Console.WriteLine("PUSHD          현재 디렉터리를 저장한 다음 변경합니다.");
            Console.WriteLine("RD             디렉터리를 제거합니다.");
            Console.WriteLine("RECOVER        불량이거나 결함이 있는 디스크에서 읽을 수 있는 정보를 복구합니다.");
            Console.WriteLine("REM            배치 파일 또는 CONFIG.SYS에 주석을 기록합니다.");
            Console.WriteLine("REN            파일 이름을 바꿉니다.");
            Console.WriteLine("RENAME         파일 이름을 바꿉니다.");
            Console.WriteLine("REPLACE        파일을 바꿉니다.");
            Console.WriteLine("RMDIR          디렉터리를 제거합니다.");
            Console.WriteLine("ROBOCOPY       파일과 디렉터리 트리를 복사할 수 있는 고급 유틸리티입니다.");
            Console.WriteLine("SET            Windows 환경 변수를 표시, 설정 또는 제거합니다.");
            Console.WriteLine("SETLOCAL       배치 파일에서 환경 변경의 지역화를 시작합니다.");
            Console.WriteLine("SC             서비스(백그라운드 프로세스)를 표시하거나 구성합니다.");
            Console.WriteLine("SCHTASKS       컴퓨터에서 실행할 명령과 프로그램을 예약합니다.");
            Console.WriteLine("SHIFT          배치 파일에서 바꿀 수 있는 매개 변수의 위치를 바꿉니다.");
            Console.WriteLine("SHUTDOWN       컴퓨터의 로컬 또는 원격 종료를 허용합니다.");
            Console.WriteLine("SORT           입력을 정렬합니다.");
            Console.WriteLine("START          지정한 프로그램이나 명령을 실행할 별도의 창을 시작합니다.");
            Console.WriteLine("SUBST          경로를 드라이브 문자에 연결합니다.");
            Console.WriteLine("SYSTEMINFO     컴퓨터별 속성과 구성을 표시합니다.");
            Console.WriteLine("TASKLIST       서비스를 포함하여 현재 실행 중인 모든 작업을 표시합니다.");
            Console.WriteLine("TASKKILL       실행 중인 프로세스나 응용 프로그램을 중단합니다.");
            Console.WriteLine("TIME           시스템 시간을 표시하거나 설정합니다.");
            Console.WriteLine("TITLE          CMD.EXE 세션에 대한 창 제목을 설정합니다.");
            Console.WriteLine("TREE           드라이브 또는 경로의 디렉터리 구조를 그래픽으로");
            Console.WriteLine("               표시합니다.");
            Console.WriteLine("TYPE           텍스트 파일의 내용을 표시합니다.");
            Console.WriteLine("VER            Windows 버전을 표시합니다.");
            Console.WriteLine("VERIFY         파일이 디스크에 올바로 기록되었는지 검증할지");
            Console.WriteLine("         여부를 지정합니다.");
            Console.WriteLine("VOL            디스크 볼륨 레이블과 일련 번호를 표시합니다.");
            Console.WriteLine("XCOPY          파일과 디렉터리 트리를 복사합니다.");
            Console.WriteLine("WMIC           대화형 명령 셸 내의 WMI 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine("도구에 대한 자세한 내용은 온라인 도움말의 명령줄 참조를 참조하십시오.");
            Console.WriteLine();
        }

        public void dir(ObjectVO newObject)
        {
            string type = "";
            
            if(newObject.ObjectType.Equals("Directory"))
            {
                type = "<DIR>";
                Console.WriteLine("{0}  {1}         {2}", newObject.CreateDate, type, newObject.ObjectName);
            }
            else
            {
                int fileSize = Convert.ToInt32(newObject.Filesize);
                string strFileSize = fileSize.ToString("#,##0");

                Console.WriteLine("{0}  {1,13} {2}", newObject.CreateDate, strFileSize, newObject.ObjectName);
            }            
        }


    }
}
