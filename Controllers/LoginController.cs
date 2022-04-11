using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SystemADM.Models;

namespace SystemADM.Controllers
{

    public class LoginController : Controller
    {
        private systemADMEntities1 db = new systemADMEntities1();
        //public string role;
        // GET: Login
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User logUser)
        {
            if (ModelState.IsValid)
            {
                using (systemADMEntities1 db = new systemADMEntities1())
                {
                    logUser = db.users.Where(a => a.email.Equals(logUser.email) && a.haslo.Equals(logUser.haslo)).FirstOrDefault();
                    if(logUser != null)
                    {
                        Session["id_uzytkownik"] = logUser.id_uzytkownik.ToString();
                        Session["count"] = 1;
                        Session["typ_uzytkownika"] = logUser.role.typ_uzytkownika.ToString();
                        /*FormsAuthentication.SetAuthCookie(logUser.email, false);       
                                          Session["email"] = logUser.email.ToString();
                                          Session["typ_uzytkownika"] = logUser.role.typ_uzytkownika.ToString();*/


                        var authTicket = new FormsAuthenticationTicket(
                                    1,                                                  // version
                                    logUser.id_uzytkownik.ToString(),                   // user id
                                    DateTime.Now,                                       // created
                                    DateTime.Now.AddMinutes(20),                        // expires
                                    false,
                                    logUser.role.typ_uzytkownika                      // can be used to store roles
                                    );

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);


                        return RedirectToAction("Index", "Tickets");
                    }
                }
            }
            @ViewBag.Message = "Dane logowania są niepoprawne";
            return View(logUser);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("email");

            ViewBag.Message = "Zostałeś pomyślnie wylogowany";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult userProfile()
        {

            int id = int.Parse(Session["id_uzytkownik"].ToString());
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}