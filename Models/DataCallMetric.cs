using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiraCore.Models
{
    public class DataCallMetric
    {
        [Key]
        public string IDText { get; set; }
        public string MetricText { get; set; }
        public string MetricPicklist { get; set;  }
        public List<PickList> PickLists { get; set; }
    }
}
