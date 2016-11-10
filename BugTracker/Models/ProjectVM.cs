using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectVM
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string projectName { get; set; }
        public string displayName { get; set; }
        public string Description { get; set; }
       
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm tt}")]
        public DateTimeOffset? Updated { get; set; }

 
        public SelectList projectManagers { get; set; }
        public MultiSelectList developers { get; set; }
        public MultiSelectList userList { get; set; }
        public string[] selectedUsers { get; set; }

    }
}