
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraCore  
{  
    public static class StringExtentions
    { 
        public static string Hash(this string Target)
        { 
            return Regex.Replace(Target, "[^A-Za-z0-9]", "").Trim() ;
        }  
        public static string StripHTML(this string Target)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Target);
            string result = htmlDoc.DocumentNode.InnerText;
            result = Regex.Replace(result, "<.*>", "") ;
            result = Regex.Replace(result, "&#\\d{1,3};", ""); 
            result = Regex.Replace(result, "&amp;", "&"); 
            return result;
        }


        public static string ReverseString(this string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }     
    }
}
