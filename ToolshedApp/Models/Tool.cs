using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolshedApp.Models
{
    public class Tool 
    {
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public virtual ToolshedUser Owner { get; set; }
        public string Image { get; set; }
        [Key]
        public int ToolId { get; set; }
        public bool Available { get; set; }

    }
}