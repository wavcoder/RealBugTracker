using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
    {
    //[RequireHttps]
    public class ProjectsController : Controller
        {
            private ApplicationDbContext db = new ApplicationDbContext();
            private ProjectHelper projectHelper = new ProjectHelper();

            // GET: Projects
            [Authorize(Roles = "Admin ,ProjectManager, Developer, Submitter")]
            public ActionResult Index()
            {
            //Add resultlist from project helper
                var user = projectHelper.ListProjects(User.Identity.GetUserId());
                return View(user);


            }



            // GET: Projects/Details/5
            [Authorize(Roles = "Admin,ProjectManager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var role = db.Roles.FirstOrDefault(r => r.Name == "ProjectManager");
            var usrs = db.Users.Where( u => u.Roles.Any(r => r.RoleId == role.Id )).ToList();
            ViewBag.OwnerName = new SelectList(usrs, "Id", "FirstName");
            return View();
        }
        //ViewBag.AssignedToUserId = new SelectList(usrs, "Id", "FirstName","UserName");

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OwnerName")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.OwnerName = db.Users.FirstOrDefault(x => x.Id == projects.OwnerName).DisplayName;
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            
                var role = db.Roles.FirstOrDefault(r => r.Name == "ProjectManager");
                var usrs = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
                ViewBag.OwnerName = new SelectList(usrs, "Id", "FirstName");
                return View(projects);            
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,OwnerName")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.OwnerName = db.Users.FirstOrDefault(x => x.Id == projects.OwnerName).DisplayName;
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Project/Assignment
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignUsers(int Id)
        {
            var project = db.Projects.Find(Id);
            var user = new AssignUsersToProjVM();
            user.Id = Id;
            user.userList = new MultiSelectList(db.Users, "Id", "FirstName", user.selectedUsers);
            user.projectName = project.Name;
            user.selectedUsers = projectHelper.ListUsersInProject(project.Id).ToList();
            return View(user);
        }

        //POST:  Project/Assignment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignUsers(AssignUsersToProjVM model)
        {
            var x = db.Projects.Find(model.Id);
            foreach (var item in db.Users.Select(u => u.Id).ToList())
            {
                projectHelper.RemoveProjectUser(x.Id, item);
            }
            foreach (var item in model.selectedUsers)
            {
                projectHelper.AssignProjectUser(x.Id, item);
            }
            
            return RedirectToAction("Details", "Projects", new { id = model.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
