using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ToolshedApp.Models
{
    public class ToolshedContext : ApplicationDbContext 
    {
        public virtual DbSet<ToolshedUser> ToolshedUsers { get; set; }

        public virtual DbSet<Tool> Tools { get; set; }
      
    }
}