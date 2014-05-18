using Invitar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Invitar.Controllers
{
    public class ResponseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index(int eventID, int inviteeID, int? res)
        {
            var @event = db.Events.Single(e => e.Id == eventID);
            return View(@event);
        }

        [HttpPost]
        public ActionResult Index(int eventID, int inviteeID, int res, FormCollection collection)
        {
            string total, comment;
            total = comment = string.Empty;
            if(res == 1)
            {
                total = collection.GetValue("totalyes").AttemptedValue;
                comment = collection.GetValue("commentyes").AttemptedValue;
            }
            else if (res == 2)
            {
                total = collection.GetValue("totalno").AttemptedValue;
                comment = collection.GetValue("commentno").AttemptedValue;
            }
            else if (res == 3)
            {
                total = collection.GetValue("totalmaybe").AttemptedValue;
                comment = collection.GetValue("commentmaybe").AttemptedValue;
            }
            var @event = db.Events.Single(e => e.Id == eventID);
            if (inviteeID > 0)
            {
                var invitee = @event.Invitees.Single(i => i.Id == inviteeID);
                invitee.Response = (InviteResponse)Enum.Parse(typeof(InviteResponse), res.ToString());
                if (!string.IsNullOrEmpty(total))
                {
                    invitee.Count = int.Parse(total);
                }
                invitee.Comment = comment;
                db.SaveChanges();
            }
            return Content("Thanks for your response!");
        }
	}
}