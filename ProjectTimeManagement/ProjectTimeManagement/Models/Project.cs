using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTimeManagement.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public DateTime ProjectCreatedTime { get; set; }
        public string ProjectName { get; set; }
        public string CreatorName { get; set; }
        public DateTime RegisterTime { get; set; }
        public float Hours { get; set; }
    }
}