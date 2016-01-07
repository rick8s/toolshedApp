using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ToolshedApp.Models
{
    public class ToolshedUser : IComparable
    {
        public virtual ApplicationUser RealUser { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MaxLength(15)]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z\d]+[-_a-zA-Z\d]{0,2}[a-zA-Z\d]+")]
        public string UserName { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Street { get; set; }
        [Key]
        public int UserId { get; set; }

        public List<Tool> Tools { get; set; }

        public int CompareTo(object obj)
        {
            ToolshedUser other_user = obj as ToolshedUser;
            int answer = this.UserName.CompareTo(other_user.UserName);
            return answer;
        }
    }
}