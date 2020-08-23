using JiraCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JiraCore.Data
{
    public static class IssueProvider
    {
        public static List<DataCallIssue> Load(string src) {
            List<DataCallIssue> issues = new List<DataCallIssue>();
            int cnt = 0;
            foreach (string xml in XmlProvider.GetFolder(src))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xml);
                DataCallIssue issue = new DataCallIssue(); 
                issue.Ticket = xmldoc.SelectSingleNode("//key").InnerText.Trim().ToLower();
                issue.Section = xmldoc.SelectSingleNode("//qgroup").InnerText.Trim();
                foreach (XmlNode xn in xmldoc.SelectNodes("//metric"))
                {
                    DataCallMetric metric = new DataCallMetric();
                    metric.IDText = xn["id-text"].InnerText;
                    metric.MetricText = xn["question-text"].InnerText; 
                    metric.MetricPicklist = xn["picklist"]==null ? "" : xn["picklist"].InnerXml;
                    issue.Metrics.Add(metric);
                }
                cnt++;
                issue.Key = xmldoc.SelectSingleNode("//qgroup").Attributes["id"].Value;
          
                string sectionno = xmldoc.SelectSingleNode("//sectionno").InnerText;
                if (!string.IsNullOrWhiteSpace(sectionno))
                {
                    issue.SectionNo = Convert.ToInt32(sectionno );
                } 
                issues.Add(issue);
                
            }
            return issues;
        }
    }
}
