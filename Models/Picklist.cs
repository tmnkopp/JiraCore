using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JiraCore.Models
{
    public class PickList
    {
        public PickList()
        { 
        }
        public PickList( string DisplayValue)
        { 
            this.DisplayValue = DisplayValue;
        }
        public string PK_PickList { get; set; }
        public string PK_PickListType { get; set; }
        public string DisplayValue { get; set; }
        private string _codevalue; 
        public string CodeValue {
           get { return _codevalue ?? GetCode(this.DisplayValue);}
           set { _codevalue = value; } 
        } 
        public Func<string,string> GetCode = (s) =>
           {
               s = s.ToUpper().Trim();
               if (s == "OTHER") return "OTH";
               string code = Regex.Replace(s, @"[^A-Z]", "");
               string PRE = code.Substring(0,2);
               string POST = code.Substring(code.Length-2, 2);
               code = code.Replace(PRE, ""); 
               code = PRE + Regex.Replace(code, @"[AEIOUY]", "") + POST;
               int len = (code.Length > 8) ? 8 : code.Length;
               return code.Substring(0, len); 
           }; 
    }
}
