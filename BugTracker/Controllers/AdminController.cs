
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class AdminController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper roleHelper = new UserRolesHelper();

        // GET:  Admin/Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<UsersInRoleModel> users = new List<UsersInRoleModel>();
            foreach (var user in db.Users.ToList())
            {
                //var x = db.Users.Find(Id);
                var temp = new UsersInRoleModel();
                var role = new UsersInRoleModel();
                temp.user = user;
                temp.roles = roleHelper.ListUserRoles(user.Id).ToList();
                role.roleList = new MultiSelectList(db.Roles, "Name", "Name", role.selectedRoles);
                //role.firstName = x.FirstName;
                //role.lastName = x.LastName;
                //role.selectedRoles = roleHelper.ListUserRoles(x.Id).ToArray();
                users.Add(temp);
            }

            return View(users);
        }


        // GET: Admin/ManageRoles
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(string Id)
        {
            var x = db.Users.Find(Id);
            var role = new UsersInRoleModel();
            role.Id = Id;
            role.roleList = new MultiSelectList(db.Roles, "Name", "Name", role.selectedRoles);
            role.firstName = x.FirstName;
            role.lastName = x.LastName;
            role.selectedRoles = roleHelper.ListUserRoles(x.Id).ToArray();
            return View(role);
        }


        // POST: Admin/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(AdminRoleModel model)
        {
            var y = db.Users.Find(model.Id);
            foreach (var item in db.Roles.Select(r => r.Name).ToList())
            {
                roleHelper.RemoveUserFromRole(y.Id, item);
            }
            foreach (var item in model.selectedRoles)
            {
                roleHelper.AddUserToRole(y.Id, item);
            }
            return RedirectToAction("Index");
        }

    }
}
