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
                string desc = xmldoc.SelectSingleNode("//item//description").InnerXml;
                issue.Description = xmldoc.SelectSingleNode("//item//description").InnerText.Trim(); 

                if (xmldoc.SelectSingleNode("//qgroup") != null)
                    issue.Section = xmldoc.SelectSingleNode("//qgroup").InnerText.Trim();

                if (xmldoc.SelectSingleNode("//item//title") != null)
                    issue.Title = xmldoc.SelectSingleNode("//item//title").InnerText.Trim();
                
                if (xmldoc.SelectSingleNode("//item//link") != null)
                    issue.Link = xmldoc.SelectSingleNode("//item//link").InnerText.Trim();
                
                if (xmldoc.SelectSingleNode("//metric") != null)
                {
                    foreach (XmlNode xn in xmldoc.SelectNodes("//metric"))
                    {
                        DataCallMetric metric = new DataCallMetric();
                        metric.IDText = xn["id-text"].InnerText;
                        metric.MetricText = xn["question-text"].InnerText;
                        metric.MetricPicklist = xn["picklist"] == null ? null : $"<picklist>{xn["picklist"].InnerXml}</picklist>";
                        if (metric.MetricPicklist != null)
                        {
                            metric.PickLists = PicklistShred(metric.MetricPicklist);
                        }
                        issue.Metrics.Add(metric);
                    }
                }

                cnt++;
                if (xmldoc.SelectSingleNode("//qgroup") != null) 
                    issue.Key = xmldoc.SelectSingleNode("//qgroup").Attributes["id"].Value;
                
                if (xmldoc.SelectSingleNode("//sectionno") != null) {
                    string sectionno = xmldoc.SelectSingleNode("//sectionno").InnerText;
                    if (!string.IsNullOrWhiteSpace(sectionno))
                    {
                        issue.SectionNo = Convert.ToInt32(sectionno);
                    }
                }

                issues.Add(issue);
                
            }
            return issues;
        }
        private static List<PickList> PicklistShred(string xml) {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            List<PickList> items = new List<PickList>();
            foreach (XmlNode xn in xmldoc.SelectNodes("//li"))
            {
                PickList item = new PickList();
                item.DisplayValue = xn.InnerText;
            }
            return items; 
        }
    }
}
