using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers

{
    //[RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();//instantiated db context
       
    
        public ActionResult Index()
        {
            var ticket = db.Tickets;
            ViewBag.projectcount = db.Projects.Count();
            ViewBag.ticketcount = db.Tickets.Count();
            ViewBag.resolvedcount = db.Tickets.Where(t => t.TicketStatusId == 4).Count();
            ViewBag.opencount = db.Tickets.Where(t => t.TicketStatusId != 4).Count();
            ViewBag.usercount = db.Users.Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Users()
        {
            var model = db.Users.ToList();
            return View(model);
        }
    }
}