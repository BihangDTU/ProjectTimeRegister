using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTimeManagement.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ProjectName { get; set; }
        public string CreatorName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}