using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class Projects
    {
        public Projects()
        {
            OwnerUser = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Tickets>();
        }
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm tt}")]
        public DateTimeOffset LastUpdate { get; set; }

        public bool ProjectComplete { get; set; }

        public virtual ICollection<ApplicationUser> OwnerUser { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
