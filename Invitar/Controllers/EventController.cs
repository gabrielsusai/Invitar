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
using System.Drawing;

using Google.GData.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.Contacts;
using System.Data;
using System.Data.SqlClient;

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
            Invitar.Models.Event @event = db.Events.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,Title,Location,StartDate,StartTime,Description,HideGuest,InviteOtherGuest")] Invitar.Models.Event @event)
        {
            if (ModelState.IsValid)
            {
                //db.Events.Add(@event);
                //db.SaveChanges();
                Session["Event"] = @event;
                return RedirectToAction("Photo");
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
            Invitar.Models.Event @event = db.Events.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Title,Location,StartDate,StartTime,EndDate,EndTime,Description,HideGuest,InviteOtherGuest")] Invitar.Models.Event @event)
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
            Invitar.Models.Event @event = db.Events.Find(id);
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
            Invitar.Models.Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region "Image Actions"

        public ActionResult Photo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Photo(string hdnimageSource)
        {
            var @event = (Invitar.Models.Event)Session["Event"];
            if (@event == null) return View();

            bool result = Uri.IsWellFormedUriString(hdnimageSource, UriKind.Absolute);

            if (result) // for web url
            {
                var request = WebRequest.Create(hdnimageSource);
                var response = request.GetResponse();
                MemoryStream ms = new MemoryStream();
                response.GetResponseStream().CopyTo(ms);
                @event.Image = ms.ToArray();
            }
            else // for file system
            {
                var image = System.IO.File.ReadAllBytes(Server.MapPath(hdnimageSource));
                @event.Image = image;
            }

            Session["Event"] = @event;

            return RedirectToAction("Preview");
        }

        [HttpGet]
        public PartialViewResult Themes(string id)
        {
            var paths = new List<String>();
            var dir = new DirectoryInfo(Server.MapPath(string.Format(@"~/assets/img/event/{0}", id)));
            foreach (var d in dir.GetFiles())
            {
                paths.Add(string.Format("/assets/img/event/{0}/{1}", id, Path.GetFileName(d.FullName)));
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

        [HttpGet]
        public ActionResult Preview()
        {
            var @event = (Invitar.Models.Event)Session["Event"];
            return View(@event);
        }

        [HttpPost]
        public ActionResult Preview(FormCollection collection)
        {
            var @event = (Invitar.Models.Event)Session["Event"];
            @event.UserId = Session["UserId"].ToString();
            db.Events.Add(@event);
            db.SaveChanges();
            return RedirectToAction("AddInvitee", new { eventid = @event.Id });
        }

        [HttpGet]
        public ActionResult ViewEvent(int eventID)
        {
            var @event = db.Events.Include("Invitees").Single(e => e.Id == eventID);
            return View(@event);
        }

        private void SendEmail(string email, string eventTitle, string body)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(email);
            message.Subject = "Invitar Invitation: " + eventTitle;
            message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["NoReply"]);
            message.IsBodyHtml = true;
            message.Body = body;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);
            smtp.Send(message);
        }

        [HttpGet]
        public ActionResult AddInvitee(int eventID)
        {
            var ID = eventID;
            TempData["EventID"] = eventID.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult AddInvitee(string inviteelist, int eventID, string button, string txtgmailusername, string txtPaswword, string[] lstgooglecontact)
        {
            if (button == "Submit")
            {
                var invitees = inviteelist.Split(',');
                Invitar.Models.Event @event = db.Events.Find(eventID);
                @event.Invitees = new List<Invitee>();

                for (int i = 0; i < invitees.Length; i++)
                {
                    Invitee invitee = new Invitee();
                    invitee.Email = invitees[i].ToString();
                    @event.Invitees.Add(invitee);
                }
                for (int i = 0; i < lstgooglecontact.Length; i++)
                {
                    Invitee invitee = new Invitee();
                    invitee.Email = lstgooglecontact[i].ToString();
                    @event.Invitees.Add(invitee);
                }

                db.SaveChanges();

                // Send email to all the invitees in this event.
                foreach (var item in @event.Invitees)
                {
                    SendEmail(item.Email, @event.Title, ConstructInviteEmail(@event, item.Id));
                }

                return RedirectToAction("User", "Home");
            }
            else if (button == "Sign In")
            {
                try
                {
                    //return RedirectToAction("ImportGmailContact", "Event");
                    ViewData.Add("gmailContacts", new SelectList(getcontacts(txtgmailusername, txtPaswword)));
                    // getcontacts(txtgmailusername, txtPaswword);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid Email or Password.");
                }
                return View();
            }
            return RedirectToAction("User", "Home");
        }

        private string ConstructInviteEmail(Invitar.Models.Event @event, int inviteeID)
        {
            var body = string.Format(System.IO.File.ReadAllText(Server.MapPath("~/Invite.html")),
            (string)Session["UserName"],
            @event.Title,
            @event.StartDate.ToLongDateString(),
            @event.StartTime,
            @event.Location,
            string.Format(ConfigurationManager.AppSettings["URL"] + "eventID={0}&inviteeID={1}", @event.Id, inviteeID));
            return body;
        }

        [HttpGet]
        public ActionResult ImportGmailContact()
        {
            return View();
        }

        public List<string> GetGmailContacts(string p_name, string e_id, string psw)
        {
           
                List<string> lstemail = new List<string>();
                RequestSettings rs = new RequestSettings(p_name, e_id, psw);
                rs.AutoPaging = true;
                ContactsRequest cr = new ContactsRequest(rs);
                Feed<Contact> f = cr.GetContacts();
                foreach (Contact t in f.Entries)
                {
                    foreach (EMail email in t.Emails)
                    {
                        lstemail.Add(email.Address.ToString());

                    }
                }
        
            return lstemail;

        }

        public List<string> getcontacts(string username, string password)
        {
            List<string> lstemail = new List<string>();
            lstemail = GetGmailContacts("invitar", username, password);
            return lstemail;
        }
    }
}
