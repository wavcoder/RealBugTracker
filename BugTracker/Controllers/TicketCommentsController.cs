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
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        // GET: TicketComments
        public ActionResult Index(int? ticketid)
        {
            //if (User.IsInRole("Admin"))
            {
                var ticketComments = db.Comments.Include(t => t.Ticket).Include(t => t.User);
                return View(ticketComments.ToList());
            }
            //else
            //{
            //   int? tid = ticketid;

            //    var ticketComments = db.Comments.Where(c => c.TicketId == tid).ToList();
            //    //var ticketComments = db.Comments.Include(t => t.Ticket).Include(t => t.User);
            //    return View(ticketComments);
            //}
        }

        // GET: TicketComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
          
            if (ticketComments == null)
            {
                var tickets = db.Tickets.Find(id);
                int ticketInt = tickets.Id;
                ViewData["ticketId"] = ticketInt;
                ViewBag.NoComms = "No Comments";
                return View();
            }
            return View(ticketComments);
        }

        // GET: TicketComments/Create
        public ActionResult Create(int ticketid)
        {
            TempData["ticketId"] = ticketid;
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //  more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,Comment")] TicketComments ticketComments)


        {
            var ticketHistory = new TicketHistories();
            var user = User.Identity.GetUserId();
            var currentUser = db.Users.Find(user);
            var ticketOwnerId = db.Tickets.FirstOrDefault(t => t.OwnerUserId == user);
            ticketHistory.TicketId = (int)TempData["ticketId"];
            ticketHistory.Property = "Added Comment";
            ticketHistory.NewValue = "Added";
            ticketHistory.UserId = user;
            ticketHistory.ChangeDate = DateTime.Now;          
            ticketComments.Created = DateTime.Now;
            ticketComments.UserId = User.Identity.GetUserId();

            db.TicketHistories.Add(ticketHistory);
            db.Comments.Add(ticketComments);

            db.SaveChanges();

            return RedirectToAction("Index", "Tickets");
        }


        // GET: TicketComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
            //TicketComments ticketComments = db.TicketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComments.UserId);
            return RedirectToAction("Index","Tickets");
            //return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.Comments.Find(id);
            //TicketComments ticketComments = db.TicketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComments ticketComments = db.Comments.Find(id);
            //TicketComments ticketComments = db.TicketComments.Find(id);
            db.Comments.Remove(ticketComments);
            //db.TicketComments.Remove(ticketComments);
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
