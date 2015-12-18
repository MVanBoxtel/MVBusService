/* MVDriverController.cs
 * Assignment 8
 * Revision History
 *      Matt Van Boxtel, 2015.12.05: Created
 *      Matt Van Boxtel, 2015.12.08: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVBusService.Controllers
{
    public class MVLanguageController : Controller
    {
        // GET: MVLanguage
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext); // get Request variables,  QueryStrings, cookies, etc.
            // if the language cookie exists … set the UI language and culture
            if (Request.Cookies["language"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(Request.Cookies["language"].Value);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(Request.Cookies["language"].Value);
            }
        }
        // GET: Languages
        public ActionResult ChangeLanguage()
        {
            SelectListItem en = new SelectListItem() { Text = "English", Value = "en", Selected = true };
            SelectListItem fr = new SelectListItem() { Text = "Nederlands", Value = "nl" };
            SelectListItem[] languages = new SelectListItem[] { en, fr };
            ViewBag.language = languages;
            // Store page you came from, so user can return there after selecting language
            Response.Cookies.Add(new HttpCookie("returnURL", Request.UrlReferrer.ToString()));
            return View();
        }

        // save selected language to a cookie
        [HttpPost]
        public void ChangeLanguage(string language)
        {
            Response.Cookies.Add(new HttpCookie("language", language));
            if (Request.Cookies["returnURL"] != null)
                Response.Redirect(Request.Cookies["returnURL"].Value);
            else
                Response.Redirect("/");
        }
    }
}