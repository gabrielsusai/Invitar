using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invitar.Models;

namespace Invitar.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult User()
        {
            string userID = Session["UserId"].ToString();
            var events = (new ApplicationDbContext()).Events.Where(e => e.IsSample == true || e.UserId == userID).ToList();
            return View(events);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}