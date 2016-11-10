namespace BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            //Added to seed demo roles
            //if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            //{
            //    roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            //}

            //if (!context.Roles.Any(r => r.Name == "DemoDeveloper"))
            //{
            //    roleManager.Create(new IdentityRole { Name = "DemoDeveloper" });
            //}

            //if (!context.Roles.Any(r => r.Name == "DemoProjectManager"))
            //{
            //    roleManager.Create(new IdentityRole { Name = "DemoProjectManager" });
            //}

            //if (!context.Roles.Any(r => r.Name == "DemoSubmitter"))
            //{
            //    roleManager.Create(new IdentityRole { Name = "DemoSubmitter" });
            //}

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "wavcoder@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "wavcoder@gmail.com",
                    Email = "wavcoder@gmail.com",
                    FirstName = "Bill",
                    LastName = "Voss",
                    DisplayName = "Bill Voss",
                }, "Wv!234");
            }
            var administratorUserID = userManager.FindByEmail("wavcoder@gmail.com").Id;
            userManager.AddToRole(administratorUserID, "Admin");


            //seeding PM



            if (!context.Users.Any(u => u.Email == "pm1@anymail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "pm1@anymail.com",
                    Email = "pm1@anymail.com",
                    FirstName = "Manager1",
                    LastName = "Bug",
                    DisplayName = "Manager One",
                }, "Wv!234");
            }
            var adminID = userManager.FindByEmail("pm1@anymail.com").Id;
            userManager.AddToRole(administratorUserID, "ProjectManager");


            //seeding dev



            if (!context.Users.Any(u => u.Email == "dev1@anymail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dev1@anymail.com",
                    Email = "dev1@anymail.com",
                    FirstName = "Dev1",
                    LastName = "Bugs",
                    DisplayName = "Dev One",
                }, "Wv!234");
            }
            var administratorUserID2 = userManager.FindByEmail("dev1@anymail.com").Id;
            userManager.AddToRole(administratorUserID2, "Developer");

            //seeding submitter



            if (!context.Users.Any(u => u.Email == "sub1@anymail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sub1@anymail.com",
                    Email = "sub1@anymail.com",
                    FirstName = "Sub1",
                    LastName = "Bugsy",
                    DisplayName = "Sub One",
                }, "Wv!234");
            }
            var administratorUserID3 = userManager.FindByEmail("sub1@anymail.com").Id;
            userManager.AddToRole(administratorUserID3, "Submitter");

            //}




            ////////////////////////////////

            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //seeding DemoAdmin
            //if (!context.Users.Any(u => u.Email == "DemoAdmin@email.com"))
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "DemoAdmin@email.com",
            //        Email = "DemoAdmin@email.com",
            //        FirstName = "DemoAdmin",
            //        LastName = "DemoAdmin",
            //        DisplayName = "DemoAdmin",
            //    }, "Demo!234");
            //}
            //var administratorUserID4 = userManager.FindByEmail("DemoAdmin@email.com").Id;
            //userManager.AddToRole(administratorUserID, "DemoAdmin");


            //seeding DemoPM



            //if (!context.Users.Any(u => u.Email == "DemoPM@email.com"))
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "DemoPM@email.com",
            //        Email = "DemoPM@email.com",
            //        FirstName = "DemoPM",
            //        LastName = "DemoPM",
            //        DisplayName = "DemoPM",
            //    }, "Demo!234");
            //}
            //var adminID2 = userManager.FindByEmail("DemoPM@email.com").Id;
            //userManager.AddToRole(administratorUserID, "DemoProjectManager");


            //seeding DemoDev



            //if (!context.Users.Any(u => u.Email == "DemoDev@email.com"))
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "DemoDev@email.com",
            //        Email = "DemoDev@email.com",
            //        FirstName = "DemoDev",
            //        LastName = "DemoDev",
            //        DisplayName = "DemoDev",
            //    }, "Demo!234");
            //}
            //var administratorUserID5 = userManager.FindByEmail("DemoDev@email.com").Id;
            //userManager.AddToRole(administratorUserID5, "DemoDeveloper");

            //seeding DemoSubmitter



            //if (!context.Users.Any(u => u.Email == "DemoSub@email.com"))
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "DemoSub@email.com",
            //        Email = "DemoSub@email.com",
            //        FirstName = "DemoSub",
            //        LastName = "DemoSub",
            //        DisplayName = "DemoSub",
            //    }, "Demo!234");
            //}
            //var administratorUserID6 = userManager.FindByEmail("DemoSub@email.com").Id;
            //userManager.AddToRole(administratorUserID6, "DemoSubmitter");
        }
    }
}
