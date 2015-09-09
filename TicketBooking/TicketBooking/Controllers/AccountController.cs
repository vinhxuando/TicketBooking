using Facebook;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.returnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (IsUserValid(model.Username, model.Password)) 
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    if (Url.IsLocalUrl(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("error", "Something's wrong here!!! Please check your username and password!!!");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            FormsAuthentication.SignOut();
            RegisteredUser user = new RegisteredUser();
            return View(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisteredUser user)
        {
            if (ModelState.IsValid)
            {
                using (TicketBookingEntities db = new TicketBookingEntities())
                {
                    if (!db.RegisteredUsers.Any(x => x.Username == user.Username))
                    {
                        db.RegisteredUsers.Add(user);
                        db.Memberships.Add(new Models.Membership() { UserID = user.ID, Role = db.Roles.Single(x => x.ID == 1).Name });
                        db.SaveChanges();

                        return RedirectToAction("Login", "Account");
                    }
                }
            }

            ModelState.AddModelError("error", "Something's wrong!!!");
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult FacebookLogin()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookAppID"],
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookAppID"],
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=email");

            if (!IsUserRegistered(me.mail))
            {

                using (TicketBookingEntities db = new TicketBookingEntities())
                {
                    string password = DateTime.Now.ToString();
                    RegisteredUser user = new Models.RegisteredUser() { Username = me.email, Password = password, Email = me.email };
                    db.RegisteredUsers.Add(user);
                    db.Memberships.Add(new Models.Membership() { UserID = user.ID, Role = db.Roles.Single(x => x.ID == 1).Name });
                    db.SaveChanges();
                }
            }

            FormsAuthentication.SetAuthCookie(me.email, false);
            Session["login"] = true;
            Session["username"] = me.email;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Tickets()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            TicketBookingEntities db = new TicketBookingEntities();
            int userid = db.RegisteredUsers.Single(x => x.Username == username).ID;
            var model = db.Tickets.Where(x => x.RegisteredUser_ID == userid);

            return View(model);
        }

        #region private_method
        private bool IsUserRegistered(string username)
        {
            using (TicketBookingEntities db = new TicketBookingEntities())
            {
                return db.RegisteredUsers.Any(x => x.Username == username);
            }
        }

        private bool IsUserValid(string username, string password)
        {
            TicketBookingEntities db = new TicketBookingEntities();
            return db.RegisteredUsers.Any(x => x.Username == username && x.Password == password);
        }
        #endregion
    }
}