using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Invitar.Models;
using System.Configuration;
using System.IO;

namespace Invitar.Controllers
{
    public class EventController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Event/
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Title,Location,StartDate,StartTime,Description,HideGuest,InviteOtherGuest")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Title,Location,StartDate,StartTime,EndDate,EndTime,Description,HideGuest,InviteOtherGuest")] Event @event)
        {
          if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region "Image Actions"

        public ActionResult Photo()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult Themes(string id)
        {
            var paths = new List<String>();
            var dir = new DirectoryInfo(Server.MapPath(string.Format(@"~/assets/img/event/{0}", id)));
            foreach (var d in dir.GetFiles())
            {
                paths.Add(string.Format("../../assets/img/event/{0}/{1}", id, Path.GetFileName(d.FullName)));
            }

            return PartialView("_Themes", paths);
        }

        [HttpPost]
        public ActionResult SelectedImage(string src)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult WebSearch(string query)
        {
            var paths = MakeRequest(query);
            return PartialView("WebSearch", paths);
        }

        static List<String> MakeRequest(string query)
        {
            var srcURL = new List<String>();
            string AccountKey = ConfigurationManager.AppSettings["AccountKey"];

            // Create a Bing container.
            var bingContainer = new Bing.BingSearchContainer(new Uri(ConfigurationManager.AppSettings["BingSearchURL"]));

            // The market to use.
            string market = "en-us";

            // Configure bingContainer to use your credentials.
            bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);


            string imageFilters = "Size:Large";

            // Build the query, limiting to 10 results.
            var imageQuery = bingContainer.Image(query, null, market, null, null, null, imageFilters);

            imageQuery = imageQuery.AddQueryOption("$top", 10);


            // Run the query and display the results.
            var imageResults = imageQuery.Execute();

            foreach (var result in imageResults)
            {
                srcURL.Add(result.MediaUrl);
            }
            return srcURL;
        }

        #endregion

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
