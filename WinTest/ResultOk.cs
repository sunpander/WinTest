using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace WinTest
{
    public class ResultOk
    {

        public string fileName;

        public string code;
        public string name;
        public string date = "";

        public string doCode = "";

        private string _doWhat = "";
        public string doWhat
        {
            get
            {
                return _doWhat;
            }
            set
            {
                _doWhat = value;
                if (!string.IsNullOrEmpty(_doWhat))
                {
                    doCode = _doWhat.Substring(0, 1);
                }
                if (_doWhat.IndexOf("_") > 0)
                {
                    remark = "独立董事";
                }
            }
        }


        public string agree;
        public string notagree;
        public string forget;

        public string remark = "";
    
       

        public ResultOk(string fileName, string code, string name, string date)
        {
            // TODO: Complete member initialization
            this.fileName = fileName;
            this.code = code;
            this.name = name;
            this.date = date;
        }

        public ResultOk(string fileName, string code, string name, string date, string remark)
        {
            // TODO: Complete member initialization
            this.fileName = fileName;
            this.code = code;
            this.name = name;
            this.date = date;

            this.remark = remark;
        }

    }

}
