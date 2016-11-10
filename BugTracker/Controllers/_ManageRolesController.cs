using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class _ManageRolesController : Controller
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
                var temp = new UsersInRoleModel();
                temp.user = user;
                temp.roles = roleHelper.ListUserRoles(user.Id).ToList();
                users.Add(temp);
            }

            return PartialView("~/Views/Admin/_ManageRolesPartial.cshtml");
        }     


        // GET: Admin/ManageRoles
        [Authorize(Roles = "Admin")]
        //public ActionResult ManageRoles(string Id)
        public PartialViewResult _ManageRolesPartial(string Id)
        {
            var x = db.Users.Find(Id);
            var role = new AdminRoleModel();
            role.Id = Id;
            role.roleList = new MultiSelectList(db.Roles, "Name", "Name", role.selectedRoles);
            role.firstName = x.FirstName;
            role.lastName = x.LastName;
            role.selectedRoles = roleHelper.ListUserRoles(x.Id).ToArray();
            return PartialView("~/Views/Admin/_ManageRolesPartial.cshtml");
        }


        // POST: Admin/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(AdminRoleModel model)
        //public PartialViewResult _ManageRolesPartial(AdminRoleModel model)
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
            return View("Index");
        }

    }
}
