﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Text;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Tickets

        [Authorize]
        public ActionResult Index()

        {

            //var ticketOwnerId = db.Tickets.FirstOrDefault(t => t.OwnerUserId == user.Id);
            var user = User.Identity.GetUserId();
            var currentUser = db.Users.Find(user);
            var cudisplay = currentUser.DisplayName;
            List<Tickets> ticketResults = new List<Tickets>();
            var tickets = db.Tickets; 
            var projects = db.Projects;
            var ownerTickets = tickets.Where(t => t.OwnerUserId == user);
            var assignedTickets = tickets.Where(t => t.AssignedToUserId == user);
            var userProjects = projects.Where(p => p.OwnerName == user).Include(t => t.Tickets);
            var pmProjects = projects.Where(p => p.OwnerName == cudisplay).Include(t => t.Tickets).ToList();
        

            if (roleHelper.IsUserInRole(user, "Admin"))
            {
                foreach (var t in tickets)
                {
                    ticketResults.Add(t);
                }
            }
            else if (roleHelper.IsUserInRole(user, "ProjectManager"))
            {
                // Tickets for Projects Managers
                foreach (var up in pmProjects)
                    //foreach (var up in userProjects)
                    {
                    foreach (var t in up.Tickets)
                    {
                        ticketResults.Add(t);
                    }
                }
            }
            else if (roleHelper.IsUserInRole(user, "Developer"))
            {

                // Tickets for Developers - Assigned Tickets
                foreach (var t in assignedTickets)
                {
                    ticketResults.Add(t);
                }

             
            }
            else  // default to Submitter
            {
                foreach (var t in ownerTickets)
                {
                    ticketResults.Add(t);
                }
            }
            return View(ticketResults);
        }


        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var role = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            var usrs = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            var currentuser = db.Users.Find(User.Identity.GetUserId());

            var project = currentuser.Project.ToList();
            ViewBag.AssignedToUserId = new SelectList(usrs, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(project, "Id", "Name");
            ViewBag.TicketPrioritiesId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AssignedToUserId,OwnerUserId,Title,Description,ProjectId,TicketTypeId,TicketStatusId,TicketPrioritiesId")] Tickets tickets)
        {
            TicketHistories ticketHistory = new TicketHistories();

            if (ModelState.IsValid)
            {
                
                tickets.Created = DateTime.Now;
                //Create the initial ticket history entry
                ticketHistory.TicketId = tickets.Id;
                ticketHistory.Property = "Ticket Created";
                ticketHistory.NewValue = "All";
                ticketHistory.UserId = tickets.OwnerUserId;
                ticketHistory.ChangeDate = tickets.Created;

                db.Tickets.Add(tickets);
                db.TicketHistories.Add(ticketHistory);
                db.SaveChanges();




                //db.Tickets.Add(tickets);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPrioritiesId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPrioritiesId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }




        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            var role = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            var usrs = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.AssignedToUserId = new SelectList(usrs, "Id", "FirstName", tickets.AssignedToUserId);
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPrioritiesId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPrioritiesId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }



        //Added
        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager, Developer,Submitter")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPrioritiesId,TicketStatusId,OwnerUserID, AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                //TicketHistories ticketHistory = new TicketHistories();
                HistoryHelper historyHelper = new HistoryHelper();

                StringBuilder updateMessage = new StringBuilder();

                //Old vs new data        
                var oldTicketInfo = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == tickets.Id);

                if (oldTicketInfo.Title != tickets.Title)
                {
                    historyHelper.AddHistory(tickets.Id, "Title", oldTicketInfo.Title, tickets.Title, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Title: {0}, ", tickets.Title);
                }

                if (oldTicketInfo.Description != tickets.Description)
                {
                    historyHelper.AddHistory(tickets.Id, "Description", oldTicketInfo.Description, tickets.Description, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Description: {0}, ", tickets.Description);
                }

                if (oldTicketInfo.TicketTypeId != tickets.TicketTypeId)
                {
                    var oldTicketType = db.TicketTypes.Find(oldTicketInfo.TicketTypeId).Name;
                    var newTicketType = db.TicketTypes.Find(tickets.TicketTypeId).Name;
                    historyHelper.AddHistory(tickets.Id, "Ticket Type", oldTicketType, newTicketType, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Type: {0}, ", newTicketType);
                }

                if (oldTicketInfo.ProjectId != tickets.ProjectId)
                {
                    var oldProject = db.Projects.Find(oldTicketInfo.ProjectId).Name;
                    var newProject = db.Projects.Find(tickets.ProjectId).Name;
                    historyHelper.AddHistory(tickets.Id, "Project Id", oldProject, newProject, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Project: {0}, ", newProject);
                }

                if (oldTicketInfo.TicketPrioritiesId != tickets.TicketPrioritiesId)
                {
                    var oldPriority = db.TicketPriorities.Find(oldTicketInfo.TicketPrioritiesId).Name;
                    var newPriority = db.TicketPriorities.Find(tickets.TicketPrioritiesId).Name;
                    historyHelper.AddHistory(tickets.Id, "Priority", oldPriority, newPriority, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Priority: {0}, ", newPriority);
                }

                if (oldTicketInfo.TicketStatusId != tickets.TicketStatusId)
                {
                    var oldStatus = db.TicketStatuses.Find(oldTicketInfo.TicketStatusId).Name;
                    var newStatus = db.TicketStatuses.Find(tickets.TicketStatusId).Name;
                    historyHelper.AddHistory(tickets.Id, "Ticket Status", oldStatus, newStatus, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Status: {0}, ", newStatus);
                }

                if (oldTicketInfo.AssignedToUserId != tickets.AssignedToUserId
                    && oldTicketInfo.AssignedToUserId != null)
                {
                    var oldAssignedUser = db.Users.Find(oldTicketInfo.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                    var newAssignedUser = db.Users.Find(tickets.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                    historyHelper.AddHistory(tickets.Id, "Assigned User", oldAssignedUser, newAssignedUser, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Assigned User: {0}, ", newAssignedUser);
                }

                if (oldTicketInfo.AssignedToUserId == null && oldTicketInfo.AssignedToUserId != tickets.AssignedToUserId)
                {
                    var oldAssignedUser = "Unassigned";
                    var newAssignedUser = db.Users.Find(tickets.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                    historyHelper.AddHistory(tickets.Id, "Assigned User", oldAssignedUser, newAssignedUser, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Assigned User: {0}, ", newAssignedUser);
                }

                tickets.Updated = DateTime.Now;
                if (tickets.TicketStatusId == 1)
                {
                    tickets.AssignedToUserId = null;
                }
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();

                //Send Notification
                var developer = db.Users.Find(tickets.AssignedToUserId);
                if (developer != null && developer.Email != null)
                {
                    var svc = new EmailService();
                    var msg = new IdentityMessage();
                    msg.Destination = developer.Email;
                    msg.Subject = "Bug Tracker Update: " + tickets.Title;
                    msg.Body = ("The following changes have been made to Ticket ID: " + tickets.Id + " - " + tickets.Title + ": " + updateMessage);
                    await svc.SendAsync(msg);
                }

                {
                    return RedirectToAction("Index");
                }

            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPrioritiesId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }


        // GET: Tickets/Delete/5
       


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        // POST: Tickets/Delete/5
        // It Does Not Delete record it just markes it as closed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            Tickets ticket = db.Tickets.Find(id);
            var strLoginUser = User.Identity.GetUserId();

            //Sends Ticket staus change to history audit log
            HistoryHelper audit = new HistoryHelper();
            audit.AddHistory(ticket.Id, "Status", ticket.TicketStatus.Name, "Resolved", strLoginUser);

            //Updates Status to Closed in the ticket table
            ticket.TicketStatusId = 4;
            var developer = db.Users.Find(ticket.AssignedToUserId);
            if (developer != null && developer.Email != null)
            {
                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Destination = developer.Email;
                msg.Subject = "Bug Tracker Update: " + ticket.Title;
                msg.Body = ("Ticket ID: " + ticket.Id + " - " + ticket.Title + "has been resolved");
                await svc.SendAsync(msg);
            }

            db.Tickets.Attach(ticket);
            db.Entry(ticket).Property("TicketStatusId").IsModified = true;
            db.SaveChanges();

            return RedirectToAction("Index",  new { id = ticket.Id });
        }



        // GET: Tickets/Assign
        public ActionResult Assign(int? id)
        {
            Tickets tickets = db.Tickets.Find(id);
            TempData["owner"] = tickets.OwnerUserId;
            //checking to see if ticket has previously been assigned
            if (tickets.TicketStatusId != 1)
                { 
            TempData["oldVal"] = db.Users.Find(tickets.AssignedToUserId).FirstName;
                }
            else
                {
                TempData["oldVal"] = "Unassigned";
                }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
 
            if (tickets == null)
            {
                return HttpNotFound();
            }
            var role = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            var usrs = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            
            ViewBag.AssignedToUserId = new SelectList(usrs, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPrioritiesId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPrioritiesId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }
        // POST: Tickets/Assign
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign([Bind(Include = "Id,AssignedToUserId,Created,OwnerUserId,Title,Description,ProjectId,TicketTypeId,TicketStatusId,TicketPrioritiesId")] Tickets tickets)
           
        {
            HistoryHelper historyHelper = new HistoryHelper();

            StringBuilder updateMessage = new StringBuilder();

            var oldTicketInfo = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == tickets.Id);

            if (oldTicketInfo.AssignedToUserId != tickets.AssignedToUserId
                  && oldTicketInfo.AssignedToUserId != null)
            {
                var oldAssignedUser = db.Users.Find(oldTicketInfo.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                var newAssignedUser2 = db.Users.Find(tickets.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                historyHelper.AddHistory(tickets.Id, "Assigned User", oldAssignedUser, newAssignedUser2, User.Identity.GetUserId());
                updateMessage.AppendFormat("Assigned User: {0}, ", newAssignedUser2);
            }

            if (oldTicketInfo.AssignedToUserId == null && oldTicketInfo.AssignedToUserId != tickets.AssignedToUserId)
            {
                var oldAssignedUser = "Unassigned";
                var newAssignedUser3 = db.Users.Find(tickets.AssignedToUserId).FirstName + " " + db.Users.Find(tickets.AssignedToUserId).LastName;
                historyHelper.AddHistory(tickets.Id, "Assigned User", oldAssignedUser, newAssignedUser3, User.Identity.GetUserId());
                updateMessage.AppendFormat("Assigned User: {0}, ", newAssignedUser3);
           }
            //Send Notification
            var developer = db.Users.Find(tickets.AssignedToUserId);
            if (developer != null && developer.Email != null)
            {
                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Destination = developer.Email;
                msg.Subject = "Bug Tracker Update: " + tickets.Title + " Assigned";
                msg.Body = ("The following Ticket ID: " + tickets.Id + " - " + tickets.Title + ": " + updateMessage);
                await svc.SendAsync(msg);
            }




            TicketHistories ticketHistory = new TicketHistories();
            var  old = TempData["oldVal"];
            var owner = TempData["owner"];
            var newAssignedUser = db.Users.Find(tickets.AssignedToUserId).FirstName;
            if (ModelState.IsValid)
            {
                ticketHistory.TicketId = tickets.Id;
                ticketHistory.Property = "Ticket Assigned";
                ticketHistory.OldValue = old.ToString();
                ticketHistory.NewValue = newAssignedUser;
                ticketHistory.UserId = tickets.OwnerUserId;
                ticketHistory.ChangeDate = tickets.Created;
                ticketHistory.Changed = true;
             
                db.TicketHistories.Add(ticketHistory);
                //db.SaveChanges();
                                                      
                tickets.Updated = DateTime.Now;
                tickets.OwnerUserId = owner.ToString();
                tickets.TicketStatusId = 2;
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPrioritiesId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPrioritiesId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
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
