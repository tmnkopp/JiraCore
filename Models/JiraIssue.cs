﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiraCore.Models
{
    public class JiraIssue
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string issuekey { get; set; }
        public string epiclink { get; set; }
        public string section { get; set; }
        public string epic  { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string labels { get; set; }
        public string version { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string deschash { get; set; }
        public string desc  { get; set; }
        public string content { get; set; } 
        public List<JiraIssueItem> issueitems { get; set; }
    }
    public class JiraIssueItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string metricid { get; set; } 
        public string metrictext { get; set; }
        public string pk { get; set; }
        public string pkpicklist { get; set; }
        public string control { get; set; }
        public string controltype { get; set; }
        public string hash { get; set; } 
    }
}
