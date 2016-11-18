using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Controllers
{
    public class ProjectHelper
    {

        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper userHelper = new UserRolesHelper();

        //Assign users to a project
        public void AssignProjectUser(int projectId, string userId)
        {

            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            project.OwnerUser.Add(user);
            db.SaveChanges();

         }
        //Remove users from a project
        public void RemoveProjectUser(int projectId, string userId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            project.OwnerUser.Remove(user);
            db.SaveChanges();
        }

        // Is this user assigned to this project?
        public bool UserInProject(int projectId, string userId)
        {
            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            if (project.OwnerUser.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // List of project managers
        public ICollection<ApplicationUser> ProjectManagers()
        {
            var projectManagerList = new List<ApplicationUser>();
            var List = manager.Users.ToList();
            foreach (var user in List)
            {
                if (userHelper.IsUserInRole(user.Id, "ProjectManager"))
                    projectManagerList.Add(user);
            }

            return projectManagerList;
        }

        //// List of projects by role
        public List<Projects> ListProjects(string userId)
     
        {
            var user = db.Users.Find(userId);
           
            var resultList = new List<Projects>();
            var userProjects = new List<Projects>();
         
            // Find user projects
            if (userHelper.IsUserInRole(userId, "Admin"))
            {
                userProjects = db.Projects.ToList();
               
            }
            else if (userHelper.IsUserInRole(userId, "ProjectManager"))//project manager list
            {
                var projOwnerId = db.Users.Find(userId);

                var projOwnerName = db.Users.FirstOrDefault(x => x.Id == projOwnerId.Id).DisplayName;
            
                userProjects = db.Projects.Where(p => p.OwnerName.Contains(projOwnerName)).ToList();
           }

            else 
            {
                var UserId = db.Users.Find(userId);
                userProjects = UserId.Project.ToList();

            }

            // Only return projects not marked as complete
            foreach (var project in userProjects)
            {
                Projects projects = new Projects();
                if (!project.ProjectComplete)
                {
                    resultList.Add(project);
                }
            }

            return resultList;

        }


        // List of users assigned to a project

        public List<string> ListUsersInProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var users = project.OwnerUser.Select(u => u.Id).ToList();
            return users;
        }
    }
}