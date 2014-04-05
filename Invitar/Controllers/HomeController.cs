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
        public ActionResult Index()
        {
            var events = (new ApplicationDbContext()).Events.ToList();
            return View(events);
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
              
    }
}