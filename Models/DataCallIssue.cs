using JiraCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JiraCore.Models
{

    public class DataCallIssue
    {
        public DataCallIssue()
        {
            Metrics = new List<DataCallMetric>();
        }
        public string Key { get; set; }
        public string Ticket { get; set; }
        public string Section { get; set; }
        public string SectionId { get; set; }
        public int SectionNo { get; set; }
        public List<DataCallMetric> Metrics {get;set;}
    }
  
}