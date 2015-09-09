using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        public ActionResult Index()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            TicketBookingEntities db = new TicketBookingEntities();
            int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            var model = db.CardInfoes.Where(x => x.UserID == userid);

            return View(model);
        }

        [ChildActionOnly]
        [HttpGet]
        public ActionResult Create()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            TicketBookingEntities db = new TicketBookingEntities();
            int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            CardInfo model = new CardInfo() { UserID = userid };
            ViewBag.Providers = db.CardProviders;
            return View(model);
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult Create(CardInfo card)
        {
            if (ModelState.IsValid)
            {
                string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                TicketBookingEntities db = new TicketBookingEntities();
                int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
                card.UserID = userid;
                db.CardInfoes.Add(card);
                db.SaveChanges();
                return RedirectToAction("Details", "Account");
            }

            ModelState.AddModelError("error", "Check your data input!!!");
            return View(card);
        }
    }
}