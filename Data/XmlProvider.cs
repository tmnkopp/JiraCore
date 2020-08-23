using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JiraCore.Data
{
    public static class XmlProvider
    {
        public static IEnumerable<string> GetFolder(string _path)
        { 
            DirectoryInfo DI = new DirectoryInfo($"{_path}");
            foreach (FileInfo file in DI.GetFiles("*.xml"))
            {
                using (TextReader tr = File.OpenText(file.FullName))
                {
                    yield return (tr.ReadToEnd());
                }

            }
        }
    }
}
