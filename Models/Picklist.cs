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
               string code = Regex.Replace(s.ToUpper(), @"[^A-Z]", "");
               string PRE = code.Substring(0,2); 
               code = PRE + Regex.Replace(code.Replace(PRE,""), @"[AEIOUY]", "");
               int len = (code.Length > 6) ? 6 : code.Length;
               return code.Substring(0, len); 
           }; 
    }
}
