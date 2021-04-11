using JiraCore.Data;
using JiraCore.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace JiraCore
{
    public class JiraIssueProvider
    {
        #region CTOR

        public JiraIssueProvider()
        {
        }
        private Expression<Func<JiraIssue, bool>> predicate = (i) => true;
        public JiraIssueProvider(Expression<Func<JiraIssue, bool>> predicate)
        {
            this.predicate = predicate;
        }

        #endregion

        #region PROPS
        public IEnumerable<JiraIssue> Items
        {
            get { return GetItems() ; }
        }
        #endregion

        #region Methods 
        private IEnumerable<JiraIssue> GetItems()
        { 
            List<JiraIssue> items = new List<JiraIssue>();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoCollection<JiraIssue> collection = client.GetDatabase("jira").GetCollection<JiraIssue>("issues");
            items = collection.Find(predicate).ToList();
            return items.AsEnumerable();
        }
        #endregion
    }
}
