using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

//using static BugTracker.Models.TicketComments;
//using static BugTracker.Models.TicketAttachments;


namespace BugTracker.Models

{
    public class Tickets
    {
        public Tickets()
        {
            TicketComment = new HashSet<TicketComments>();

            TicketAttachment = new HashSet<TicketAttachments>();

            TicketHistory = new HashSet<TicketHistories>();

            TicketNotification = new HashSet<TicketNotifications>();
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm tt}")]
        public DateTimeOffset? Updated { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public int TicketTypeId { get; set; }

        public int TicketPrioritiesId { get; set; }

        public int TicketStatusId { get; set; }
        [Required]
        public string OwnerUserId { get; set; }

        public string AssignedToUserId { get; set; }

        public virtual ICollection<TicketComments> TicketComment { get; set; }

        public virtual ICollection<TicketNotifications> TicketNotification { get; set; }

        public virtual ICollection<TicketHistories> TicketHistory { get; set; }

        public virtual ICollection<TicketAttachments> TicketAttachment { get; set; }

        public virtual Projects Project { get; set; }
        public virtual TicketStatuses TicketStatus { get; set; }

        public virtual TicketPriorities TicketPriorities { get; set; }

        public virtual TicketTypes TicketType { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }

        public virtual ApplicationUser AssignedToUser { get; set; }

    }
}
