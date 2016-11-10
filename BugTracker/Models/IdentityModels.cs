using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        
        public ApplicationUser()
        { 
            this.Project = new HashSet<Projects>();
            TicketNotifications = new HashSet<TicketNotifications>();
            TicketHistories = new HashSet<TicketHistories>();
            TicketComments = new HashSet<TicketComments>();
            TicketAttachments = new HashSet<TicketAttachments>();

        }


        public virtual ICollection<Projects> Project { get; set; }
        public virtual ICollection<TicketNotifications> TicketNotifications { get; set; }
        public virtual ICollection<TicketHistories> TicketHistories { get; set; }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static implicit operator ApplicationUser(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketComments> Comments { get; set; }
        public DbSet<TicketHistories> TicketHistories { get; set; }
        public DbSet<TicketAttachments> TicketAttachments { get; set; }
        public DbSet<TicketNotifications> TicketNotifications { get; set; }
        public DbSet<TicketStatuses> TicketStatuses { get; set; }
        public DbSet<TicketPriorities> TicketPriorities { get; set; }
        public DbSet<TicketTypes> TicketTypes { get; set; }
        public DbSet<Projects> Projects { get; set; }

    }
}
