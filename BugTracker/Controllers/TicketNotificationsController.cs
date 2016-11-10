using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotifications
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,UserId")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.TicketNotifications.Add(ticketNotifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotifications);
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
