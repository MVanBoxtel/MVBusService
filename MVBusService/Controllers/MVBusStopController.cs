/* MVBusStopController.cs
 * Assignment 1
 * Revision History
 *      Matt Van Boxtel, 2015.09.11: Created
 *      Matt Van Boxtel, 2015.09.13: Completed
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
    /// this controller handles requests and responses for bus stops
    /// </summary>
    public class MVBusStopController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: /MVBusStop/
        // handles index requests and returns a view which renders all the bus stops
        public ActionResult Index()
        {
            try
            {
                if (Request.QueryString["orderBy"].ToString() == "location")
                {
                    var busStops = db.busStops.OrderBy(r => r.location);
                    return View(busStops.ToList());
                }
                else
                {
                    var busStops = db.busStops.OrderBy(r => r.busStopNumber);
                    return View(busStops.ToList());
                }
            }
            catch (Exception)
            {
                return View(db.busStops.ToList());
            }
        }

        // handles requests for bus stops to see its routes, takes a bus stop number as an arguement
        public ActionResult RouteSelector(int? busStopNumber) 
        {
            // get all routestops that have the bus stop number we are looking for
            var routeStops = db.routeStops.Include("busRoute").Where(r => r.busStopNumber == busStopNumber && r.busRoute.routeName != null).Distinct();
            if (busStopNumber == null || db.busStops.Find(busStopNumber) == null)
            {
                TempData["message"] = "No bus stop selected";
                return RedirectToAction("Index");
            }
            else if (!routeStops.Any())
            {
                // no routes
                busStop busStop = db.busStops.Find(busStopNumber);
                TempData["message"] = "No routes for " + busStop.busStopNumber.ToString() + " " + busStop.location.ToString();
                return RedirectToAction("Index");
            }
            else if (routeStops.Count() == 1)
            {
                // go directly to routeschedule if there is only 1 routestop
                // set session var
                var routeStop = routeStops.Single();
                Session["routeStopIdValue"] = routeStop.routeStopId;
                return RedirectToAction("RouteStopSchedule", "MVRouteSchedule");
            }
            else
            {
                // order the route stops by route name
                var orderedStops = routeStops.OrderBy(r => r.busRoute.routeName);
 
                // get relevant data for display and set to viewbag
                busStop busStop = db.busStops.Find(busStopNumber);
                ViewBag.busStopNumber = busStopNumber.ToString();
                ViewBag.busStopName = busStop.location.ToString();

                // creat select list from the oredered stops, pass routestopID as the value and route name as the name
                ViewBag.routeStopId = new SelectList(orderedStops, "routeStopId", "busRoute.routeName");
                return View(routeStops);
            }
        } 

        // GET: /MVBusStop/Details/5
        // handles detail requests specific to the id of the bus stop requested, view only renders if id and busstop exist
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busstop = db.busStops.Find(id);
            if (busstop == null)
            {
                return HttpNotFound();
            }
            return View(busstop);
        }

        // GET: /MVBusStop/Create
        // handles create requests and returns view to create a bus stop
        public ActionResult Create()
        {
            return View();
        }

        // method called to create a location hash for bus stops
        public int CreateHash(string location) 
        {
            // create hash sum by converting each char to a byte in the stop location and summing it
            int hashSum = 0;
            foreach (char ch in location)
            {
                hashSum += Convert.ToByte(ch);
            }
            return hashSum;
        }

        // POST: /MVBusStop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a bus stop after clicking "create", if valid data, return to index with newly created bus stop
        public ActionResult Create([Bind(Include="busStopNumber,location,locationHash,goingDowntown")] busStop busstop)
        {
            if (ModelState.IsValid)
            {
                // save hashSum in locationHash after calling method on busstop location
                busstop.locationHash = CreateHash(busstop.location);
                db.busStops.Add(busstop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(busstop);
        }

        // GET: /MVBusStop/Edit/5
        // handles edit requests specific to the id of the bus stop requested, view only renders if bus stop and id exist
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busstop = db.busStops.Find(id);
            if (busstop == null)
            {
                return HttpNotFound();
            }
            return View(busstop);
        }

        // POST: /MVBusStop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to bus stop
        public ActionResult Edit([Bind(Include="busStopNumber,location,locationHash,goingDowntown")] busStop busstop)
        {
            if (ModelState.IsValid)
            {
                // save hashSum in locationHash after calling method on busstop location
                busstop.locationHash = CreateHash(busstop.location);
                db.Entry(busstop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(busstop);
        }

        // GET: /MVBusStop/Delete/5
        // handles delete requests specific to the id of the bus stop requested, view only renders if the bus stop and id exist
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busStop busstop = db.busStops.Find(id);
            if (busstop == null)
            {
                return HttpNotFound();
            }
            return View(busstop);
        }

        // POST: /MVBusStop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of bus stop at delete view, once bus stop is deleted return to index
        public ActionResult DeleteConfirmed(int id)
        {
            busStop busstop = db.busStops.Find(id);
            db.busStops.Remove(busstop);
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
