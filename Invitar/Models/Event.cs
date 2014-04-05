using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Invitar.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public bool HideGuest { get; set; }
        public bool InviteOtherGuest { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
        public bool IsSample { get; set; }
        public String UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}