using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToolshedApp.Models
{
    public class Tool : IComparable
    {
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public virtual ToolshedUser Owner { get; set; }
        public virtual ToolshedUser Borrower { get; set; }

        public string Image { get; set; }

        [Key]
        public int ToolId { get; set; }

        public bool Available { get; set; }
        

        public int CompareTo(object obj)
        {
            Tool other_tools = obj as Tool;
            int answer = this.Name.CompareTo(other_tools.Name);
            return answer;
        }

    }
}