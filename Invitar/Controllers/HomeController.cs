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
        public ActionResult User()
        {
            var events = (new ApplicationDbContext()).Events.Where(e=>e.IsSample==true).ToList();
            return View(events);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}