using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
    {
    public class UsersInRoleModel
    {
        public ApplicationUser user { get; set; }
        public List<string> roles { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string Id { get; set; }
        public MultiSelectList roleList { get; set; }
        public string[] selectedRoles { get; set; }
    }
    //public class AdministrationRoleModel
    //{
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public string Id { get; set; }
    //    public MultiSelectList roleList { get; set; }
    //    public string[] selectedRoles { get; set; }


    //}
}
 

