using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Controllers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        TicketHistories ticketHistory = new TicketHistories();
        Tickets ticket = new Tickets();

        public void AddHistory(int ticketId, string updateProperty, string oldValue, string newValue, string userId)
        {
            ticketHistory.TicketId = ticketId;
            ticketHistory.Property = updateProperty;
            ticketHistory.OldValue = oldValue;
            ticketHistory.NewValue = newValue;
            ticketHistory.Changed = true;
            ticketHistory.UserId = userId;
            ticketHistory.ChangeDate = DateTime.Now;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
            return;
        }
    }
}