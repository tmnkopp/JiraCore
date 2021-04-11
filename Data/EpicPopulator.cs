using System;
using System.Collections.Generic;
using System.Text; 
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using BOM.CORE;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenQA.Selenium;
 

namespace JiraCore
{
    public class JiraEpic {
        public JiraEpic(string Url)
        {
            this.Url = Url;
        }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class EpicPopulator
    {
        public readonly IConfiguration configuration;
        public readonly ILogger logger;
        public EpicPopulator(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger; 
        }
        #region Methods 
        public void Populate(JiraEpic jiraEpic)
        {
            var conn = configuration.GetSection("contexts").Get<List<BomConfigContext>>()
            .Where(c => c.conn.Contains("jira")).FirstOrDefault().conn;

            SessionContext ctx = new SessionContext();
            ctx.SessionDriver = new SessionDriver(configuration, logger);
            ctx.SessionDriver.Connect(conn); 
            ctx.SessionDriver.GetUrl(jiraEpic.Url);
            var epicTitle = ctx.SessionDriver.Driver.Title;

            MongoClient client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("jira");
            var collection = database.GetCollection<BsonDocument>("issues");
             
            IList<IWebElement> inputs = ctx.SessionDriver.Driver.FindElements(By.CssSelector("table[id='ghx-issues-in-epic-table'] .ghx-minimal a"));
            List<string> items = new List<string>();
            foreach (var item in inputs) items.Add(item.Text);
            foreach (var item in items)
            {
                ctx.SessionDriver.Pause(50).GetUrl($"https://dayman.cyber-balance.com/jira/si/jira.issueviews:issue-xml/{item}/{item}.xml");
                var src = ctx.SessionDriver.Driver.PageSource.ToString();
                src = src.Substring(src.IndexOf("<item>"), src.IndexOf("</item>") - src.IndexOf("<item>")) + "</item>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(src);
                string title = doc.SelectSingleNode("//title")?.InnerText.Trim() ?? "";
                string section = Regex.Replace(title, "\\[.*\\] ", ""); 
                string description = doc.SelectSingleNode("//description")?.InnerText.Trim() ?? "";
                
                BsonArray issueitems = new BsonArray();
                foreach (var para in description.Split("<p>"))
                {
                    var m = Regex.Match(para, "(\\d{1,2}\\.\\d{0,2})(.*)</p>");
                    if (m.Groups.Count > 1)
                    {
                        var metric = m.Groups[1].Value.Trim().TrimEnd('.');
                        var mettext = m.Groups[2].Value.Trim();
                        issueitems.Add(
                            new BsonDocument { 
                                  { "metricid", metric }
                                , { "metrictext", mettext.StripHTML() } 
                                , { "hash", mettext.StripHTML().Hash()
                                } 
                            }
                        );
                    }
                }
                var post = new BsonDocument  {
                        {"issuekey" , item}, 
                        {"epic" , epicTitle},
                        {"title" , title},
                        {"section" , section ?? ""},
                        {"link" , doc.SelectSingleNode("//link")?.InnerText.Trim() ?? ""},
                        {"labels" , doc.SelectSingleNode("//labels")?.InnerText.Trim()  ?? ""},
                        {"version" , doc.SelectSingleNode("//version")?.InnerText.Trim() ?? ""},
                        {"summary" , doc.SelectSingleNode("//summary")?.InnerText.Trim() ?? ""},
                        {"content" , src  ?? ""},
                        {"description" , description.StripHTML() ?? ""}, 
                        {"deschash" , description.StripHTML().Hash()} ,
                        {"issueitems" , issueitems }
                    };

                collection.ReplaceOneAsync(
                    filter: new BsonDocument("issuekey", item),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: post);
            }
            ctx.SessionDriver.Dispose();
        }
        #endregion
    }
}
