using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using JiraCore.Data;
using JiraCore.Models;
namespace JiraCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string src = "C:\\temp\\jira\\!SAOP2019$DataCallProcessor\\_dest";
            string dest = "C:\\temp\\jira\\!SAOP2020$DataCallProcessor\\_dest";

            List<DataCallIssue> I19 = IssueProvider.Load(src).OrderBy( x => x.SectionNo).ToList();
            List<DataCallIssue> I20 = IssueProvider.Load(dest).OrderBy(x => x.SectionNo).ToList();

            for (int i = 0; i < I19.Count; i++)
            {
                Console.Write("\n{0}: {1}\n", I19[i].SectionNo, I20[i].Section);
               
                for (int j = 0; j < I19[i].Metrics.Count; j++)
                {
                    string I19MetricText = I19[i].Metrics[j].MetricText;
                    string I20MetricText = I20[i].Metrics[j].MetricText;
                    string I19MetricPicklist = I19[i].Metrics[j].MetricPicklist;
                    string I20MetricPicklist = I20[i].Metrics[j].MetricPicklist;
                    if (I20MetricPicklist != "")
                    {
                        if (I19MetricPicklist != I20MetricPicklist)
                        {
                            Console.Write("\n{0}", I20[i].Metrics[j].IDText);
                            Console.Write("\n{0}\n{1}", I19MetricPicklist, I20MetricPicklist); 
                        } 
                    }

                    if (I19MetricText != I20MetricText)
                    {
                        //Console.Write("\nI19 - {0}: {1}", I19[i].Metrics[j].IDText, I19[i].Metrics[j].MetricText);
                        //Console.Write("\nI20 - {0}: {1}", I20[i].Metrics[j].IDText, I20[i].Metrics[j].MetricText);

                    }
                }

            } 
        }
    }
}
