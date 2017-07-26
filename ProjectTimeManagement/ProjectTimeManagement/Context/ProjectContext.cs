using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ProjectTimeManagement.Models;

namespace ProjectTimeManagement.Context
{
    public class ProjectContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
    }
}