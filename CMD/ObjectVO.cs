using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD
{
    class ObjectVO
    {
        private string createDate;
        private string objectType; // 폴더인지 파일인지 나타내는것
        private string filesize;
        private string objectName;

        public ObjectVO(string createDate, string objectType, string filesize, string objectName)
        {
            this.createDate = createDate;
            this.objectType = objectType;
            this.filesize = filesize;
            this.objectName = objectName;
        }

        public string CreateDate
        {
            get
            {
                return createDate;
            }

            set
            {
                createDate = value;
            }
        }

        public string ObjectType
        {
            get
            {
                return objectType;
            }

            set
            {
                objectType = value;
            }
        }

        public string Filesize
        {
            get
            {
                return filesize;
            }

            set
            {
                filesize = value;
            }
        }

        public string ObjectName
        {
            get
            {
                return objectName;
            }

            set
            {
                objectName = value;
            }
        }








    }
}
