/* MVBusRouteController.cs
 * Assignment 2
 * Revision History
 *      Matt Van Boxtel, 2015.09.21: Created
 *      Matt Van Boxtel, 2015.09.23: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVBusService.Models;

namespace MVBusService.Controllers
{
    /// <summary>
    /// this controller handles requests and responses for Route Stops
    /// </summary>
    public class MVRouteStopController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: /MVRouteStop/
        // this method handles route stop index requests and returns a list of route stops
        // relevant to the bus route requested
        public ActionResult Index()
        {
            try
            {
                // if exception occurs here, there is no string in QueryString with busRouteID key
                string routeString = Request.QueryString["busRouteValue"].ToString();
                string routeName = Request.QueryString["busRouteName"].ToString();
                // if no exception, save the strings to a cookie
                HttpCookie busRouteCookie = new HttpCookie("busRouteCk");
                busRouteCookie.Values.Add("busRouteValue", routeString);
                busRouteCookie.Values.Add("busRouteName", routeName);
                // send the cookie to the browser with a HTTP response
                Response.Cookies.Add(busRouteCookie);
                // use string to generate the list of route stops that have a busRouteCode identical to the string
                var routestops = db.routeStops.Where(r => r.busRouteCode == routeString).OrderBy(r => r.offsetMinutes)
                   .Include(r => r.busRoute).Include(r => r.busStop);
                // store route name and route number in viewbag
                ViewBag.routeName = routeName;
                ViewBag.routeNumber = routeString;
                // return the view with the generated list of routestops
                return View(routestops.ToList());
            }
            catch (Exception)
            {
                // we are in exception if page was requested without a QueryString, cookie may exist
                // if user was previously at a generated routeStop index above and navigated away.
                if (Request.Cookies["busRouteCk"] != null)
                {
                    // assign values of cookie to local strings
                    string routeString = Request.Cookies["busRouteCk"]["busRouteValue"];
                    string routeName = Request.Cookies["busRouteCk"]["busRouteName"];
                    // use string to generate the list of route stops that have a busRouteCode identical to the string
                    var routestops = db.routeStops.Where(r => r.busRouteCode == routeString).OrderBy(r => r.offsetMinutes)
                        .Include(r => r.busRoute).Include(r => r.busStop);
                    // set viewbag data
                    ViewBag.routeName = routeName;
                    ViewBag.routeNumber = routeString;
                    // return the view with the generated list of routestops
                    return View(routestops.ToList());
                }
                else
                {
                    // no cookie assigned and user attempted to request routestops, store error message in TempData
                    // to be used in Index view of bus route controller
                    TempData["message"] = "Select a bus route to see stops.";
                    // redirect user to MVBusRoute controller to select a route code
                    return RedirectToAction("Index", "MVBusRoute");
                }
            }
        }

        // GET: /MVRouteStop/Details/5
        // handles detail requests specific to the id of the route stop requested, view only renders if id and busroute exist
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routestop = db.routeStops.Find(id);
            if (routestop == null)
            {
                return HttpNotFound();
            }
            // get cookie data and set it to the viewbag so we can access in view
            string routeString = Request.Cookies["busRouteCk"]["busRouteValue"];
            string routeName = Request.Cookies["busRouteCk"]["busRouteName"];
            ViewBag.routeName = routeName;
            ViewBag.routeNumber = routeString;
            return View(routestop);
        }

        // GET: /MVRouteStop/Create
        // handles create requests and returns view to create a route stop
        public ActionResult Create()
        {
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName");
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location");
            // get cookie data and set it to the viewbag so we can access in view
            string routeString = Request.Cookies["busRouteCk"]["busRouteValue"];
            string routeName = Request.Cookies["busRouteCk"]["busRouteName"];
            ViewBag.routeName = routeName;
            ViewBag.routeNumber = routeString;
            return View();
        }

        // POST: /MVRouteStop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a route stop after clicking "create", if valid data, return to index with newly created route stop
        public ActionResult Create([Bind(Include="routeStopId,busRouteCode,busStopNumber,offsetMinutes")] routeStop routestop)
        {
            if (ModelState.IsValid)
            {
                db.routeStops.Add(routestop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routestop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routestop.busStopNumber);
            return View(routestop);
        }

        // GET: /MVRouteStop/Edit/5
        // handles edit requests specific to the id of the route stop requested, view only renders if route stop and id exist
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routestop = db.routeStops.Find(id);
            if (routestop == null)
            {
                return HttpNotFound();
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routestop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routestop.busStopNumber);
            // get cookie data and set it to the viewbag so we can access in view
            string routeString = Request.Cookies["busRouteCk"]["busRouteValue"];
            string routeName = Request.Cookies["busRouteCk"]["busRouteName"];
            ViewBag.routeName = routeName;
            ViewBag.routeNumber = routeString;
            return View(routestop);
        }

        // POST: /MVRouteStop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to route stop
        public ActionResult Edit([Bind(Include="routeStopId,busRouteCode,busStopNumber,offsetMinutes")] routeStop routestop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routestop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routestop.busRouteCode);
            ViewBag.busStopNumber = new SelectList(db.busStops, "busStopNumber", "location", routestop.busStopNumber);
            return View(routestop);
        }

        // GET: /MVRouteStop/Delete/5
        // handles delete requests specific to the id of the route stop requested, view only renders if the route stop and id exist
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeStop routestop = db.routeStops.Find(id);
            if (routestop == null)
            {
                return HttpNotFound();
            }
            // get cookie data and set it to the viewbag so we can access in view
            string routeString = Request.Cookies["busRouteCk"]["busRouteValue"];
            string routeName = Request.Cookies["busRouteCk"]["busRouteName"];
            ViewBag.routeName = routeName;
            ViewBag.routeNumber = routeString;
            return View(routestop);
        }

        // POST: /MVRouteStop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of route stop at delete view, once route stop is deleted return to index
        public ActionResult DeleteConfirmed(int id)
        {
            routeStop routestop = db.routeStops.Find(id);
            db.routeStops.Remove(routestop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // method used for releasing unmanaged resources, performing a final clean up before it is released from memory
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
