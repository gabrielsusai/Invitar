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

        public ActionResult Index(int eventID)
        {
            var @event = db.Events.Single(e => e.Id == eventID);
            return View(@event);
        }

        public ActionResult Index(int eventID, int inviteeID, string res)
        {

            return Content("Thanks for your response!");
        }
	}
}