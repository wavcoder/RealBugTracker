using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index(int? ticketid)
        {
            if (User.IsInRole("Admin"))
            {
                var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
                return View(ticketAttachments.ToList());
            }
            else
            {
                int? aid = ticketid;
                //var userId = User.Identity.GetUserId();
                //var projects = projHelper.ListProjects(userId);
                //var tickets = db.Tickets.Where(t => t.ProjectId = projects.Id)
                var ticketAttachments = db.TicketAttachments.Where(a => a.TicketId == aid).ToList();
                return View(ticketAttachments);
            }
        }


        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                ViewBag.ticketId = db.Tickets.Find(id);
                ViewBag.NoAtts = "No Attachments";
                return View();
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create(int ticketid)
        {
            ViewBag.TicketId = ticketid;
            ViewBag.TicketTitle = db.Tickets.Find(ticketid).Title;
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, TicketId,FilePath,Description,UserId,FileUrl")] TicketAttachments ticketAttachments, HttpPostedFileBase attachment)
        {
            var ticketHistory = new TicketHistories();
            var tickets = new Tickets();
            var user = User.Identity.GetUserId();
            var currentUser = db.Users.Find(user);
            //var ticketId = db.Tickets.FirstOrDefault(t => t.OwnerUserId == user).Id;
            var ticketOwnerId = db.Tickets.FirstOrDefault(t => t.OwnerUserId == user);
            
            ticketAttachments.UserId = currentUser.Id;
   
            if (FileUploadValidator.IsWebFriendlyFile(attachment))
            {
                var absPath = Path.GetFileName(attachment.FileName);
                attachment.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), absPath));
                ticketAttachments.FileUrl = "~/Uploads/" + absPath;
              
                if (ModelState.IsValid)
                {
                    //Ticket History
                    ticketHistory.TicketId = ticketAttachments.TicketId;
                    ticketHistory.Property = "Added Attachment";
                    ticketHistory.NewValue = "Added";
                    ticketHistory.UserId = ticketAttachments.UserId;
                    ticketHistory.ChangeDate = DateTime.Now;

                    ticketAttachments.Created = DateTime.Now;
                   
                    db.TicketHistories.Add(ticketHistory);
                    db.TicketAttachments.Add(ticketAttachments);

                    db.SaveChanges();
                    return RedirectToAction("Index", "Tickets", new { id = ticketAttachments.TicketId });
                }

            }
            else
            {
                ModelState.AddModelError("attachment", "Invalid Format.");
            }

            //    }
            //}



            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.Id);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachments);
            db.SaveChanges();
            return RedirectToAction("Index");
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
