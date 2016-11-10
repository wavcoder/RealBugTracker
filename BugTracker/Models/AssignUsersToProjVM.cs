using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class AssignUsersToProjVM
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string projectName { get; set; }
        public MultiSelectList userList { get; set; }
        public List<string> selectedUsers { get; set; }
    }

    public class ProjectDetails
    {
        public Projects Project { get; set; }
        public AssignUsersToProjVM AssignUsers { get; set; }
    }
}
