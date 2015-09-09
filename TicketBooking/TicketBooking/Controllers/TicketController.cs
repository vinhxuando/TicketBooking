using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    public class TicketController : Controller
    {
        public ActionResult Create()
        {
            TicketBookingEntities db = new TicketBookingEntities();
            List<string> departures = new List<string>();

            foreach (var item in db.Flights)
            {
                if (departures.FindIndex(x => x == item.Departure) == -1)
                    departures.Add(item.Departure);
            }

            ViewBag.Departures = departures;
            return View();
        }

        [HttpGet]
        public ActionResult BookTicket(string departure, string destination, string dateDepart, string dateDest)
        {
            //decode querystring
            string deDeparture = Server.UrlDecode(departure);
            string deDestination = Server.UrlDecode(destination);
            string deDateDepart = Server.UrlDecode(dateDepart);
            string deDateDest = Server.UrlDecode(dateDest);
            DateTime dateDeparture = Convert.ToDateTime(deDateDepart);
            DateTime dateDestination = Convert.ToDateTime(deDateDest);

            TicketBookingEntities db = new TicketBookingEntities();
            ViewBag.Flights = db.Flights.Where(x => x.Departure == deDeparture && x.Destination == deDestination && x.DepartTime == dateDeparture && x.DestTime == dateDestination);
            int userid = int.MinValue;

            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            }

            Ticket ticket = new Ticket() { RegisteredUser_ID = userid };
            ViewBag.Grades = db.GradeTables;
            return View(ticket);
        }

        public int GetRemainingSeat(string flightID, string grade)
        {
            grade = Server.UrlDecode(grade);
            TicketBookingEntities db = new TicketBookingEntities();
            Flight flight = db.Flights.Single(x => x.ID == flightID);
            Plane plane = flight.Plane;
            SeatDetail seat = plane.SeatDetails.Single(x => x.GradeTable.Name == grade);

            return seat.NumOfSeat - seat.CurReg;
        }
        
        [HttpPost]
        public ActionResult BookTicket(Ticket ticket, int numOfSeat)
        {
            if(Request.Cookies[FormsAuthentication.FormsCookieName]== null)
            {
                Session["Ticket"] = ticket;
                Session["numOfSeat"] = numOfSeat;
                return RedirectToAction("Login", "Account", new { ReturnUrl = Server.UrlEncode("/Ticket/ResubmitAfterLogin") });
            }

            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            TicketBookingEntities db = new TicketBookingEntities();
            int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            ticket.RegisteredUser_ID = userid;

            for (int i = 0; i < numOfSeat; i++)
            {
                db.Tickets.Add(ticket);
            }

            db.SaveChanges();
            Session["Ticket"] = null;
            return RedirectToAction("Details", "Account");
        }
        
        [Authorize]
        public ActionResult ResubmitAfterLogin()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            TicketBookingEntities db = new TicketBookingEntities();
            int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            int numOfSeat = (int)Session["numOfSeat"];
            Ticket ticket = new Ticket() { /*FlightID = (string)Session["FlightID"],*/ RegisteredUser_ID = userid };

            for (int i = 0; i < numOfSeat; i++)
            {
                db.Tickets.Add(ticket);
            }

            db.SaveChanges();
            Session["Ticket"] = null;
            return RedirectToAction("Details", "Account");
        }

        public PartialViewResult GetDateDestination(string departure, string destination, string dateDepart)
        {
            //decode querystring
            string deDeparture = Server.UrlDecode(departure);
            string deDestination = Server.UrlDecode(destination);
            string deDateDepart = Server.UrlDecode(dateDepart);

            DateTime dateDeparture = Convert.ToDateTime(deDateDepart);
            TicketBookingEntities db = new TicketBookingEntities();
            var flights = db.Flights.Where(x => x.Departure == deDeparture && x.Destination == deDestination && x.DepartTime == dateDeparture);
            List<string> result = new List<string>();
            foreach (var flight in flights)
            {
                result.Add(flight.DestTime.ToString());
            }

            return PartialView(result);
        }
        
        public PartialViewResult GetDateDeparture(string departure, string destination)
        {
            //decode querystring
            string deDeparture = Server.UrlDecode(departure);
            string deDestination = Server.UrlDecode(destination);

            TicketBookingEntities db = new TicketBookingEntities();
            var flights = db.Flights.Where(x => x.Departure == deDeparture && x.Destination == deDestination);
            List<string> result = new List<string>();
            foreach (var flight in flights)
            {
                    result.Add(flight.DepartTime.ToString());
            }

            return PartialView(result);
        }
        
        public PartialViewResult GetDestination(string departure)
        {
            //decode querystring
            string deDeparture = Server.UrlDecode(departure);

            TicketBookingEntities db = new TicketBookingEntities();
            var flights = db.Flights.Where(x => x.Departure == deDeparture);
            List<string> result = new List<string>();
            foreach (var flight in flights)
            {
                if (result.FindIndex(x => x == flight.Destination) == -1)
                    result.Add(flight.Destination);
            }

            return PartialView(result);
        }
    }
}