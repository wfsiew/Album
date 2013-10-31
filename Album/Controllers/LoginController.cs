using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Album.Models;

namespace Album.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                Session.Clear();
                FormsAuthentication.SignOut();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new Auth());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Auth auth, string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;

                if (ModelState.IsValid)
                {
                    if (Authenticate(auth))
                    {
                        FormsAuthentication.SetAuthCookie(auth.Username, true);
                        return RedirectToLocal(returnUrl);
                    }

                    else
                    {
                        ViewBag.Error = "Incorrect username or password";
                        return View(auth);
                    }
                }

                else
                {
                    return View(auth);
                }
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View(auth);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private bool Authenticate(Auth auth)
        {
            if (auth.Username == "wfsiew" && auth.Password == "wfsiew")
                return true;

            else if (auth.Username == "serene" && auth.Password == "serene")
                return true;

            return false;
        }
	}
}