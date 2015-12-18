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
    public class MVRouteScheduleController : Controller
    {
        private busServiceContext db = new busServiceContext();

        // method called to create data for a route stop schedule, then returns the view
        public ActionResult RouteStopSchedule(FormCollection collection)
        {
            var routeStopId_Value = collection["routeStopId"];
            string routeStopId = "";
            if (routeStopId_Value == null && Session["routeStopIdValue"] == null) 
            {
                TempData["message"] = "Select a stop to see its schedule";
                return RedirectToAction("Index", "MVBusStop");
            }
            else
            {
                if (Session["routeStopIdValue"] != null)
                {
                    routeStopId = Session.Contents["routeStopIdValue"].ToString();
                    Session["routeStopIdValue"] = null;
                }
                else
                {
                    routeStopId = routeStopId_Value;
                }

                // grab the data we need to display
                routeStop routeStop = db.routeStops.Find(int.Parse(routeStopId));
                busStop busStop = db.busStops.Find(routeStop.busStopNumber);
                busRoute busRoute = db.busRoutes.Find(routeStop.busRouteCode);

                int offsetMinutes = (int)routeStop.offsetMinutes;

                // create routeschedule
                var routeSchedules = from record in db.routeSchedules
                                     where (record.busRouteCode == routeStop.busRoute.busRouteCode)
                                     orderby record.startTime
                                     select record;

                foreach (var item in routeSchedules)
                {
                    item.startTime = item.startTime.Add(TimeSpan.FromMinutes(offsetMinutes));
                }

                // add some data to viewbag so we can show in view
                ViewBag.stop = "Stop: " + routeStop.busStopNumber.ToString() + " " + routeStopId.ToString() + " " + busStop.location;
                ViewBag.route = busRoute.busRouteCode.ToString() + " - " + busRoute.routeName;
                return View(routeSchedules);
            }

            ;
        }

        // GET: /MVRouteSchedule/
        // handles index requests and returns a view which renders all the route schedules
        public ActionResult Index()
        {
            var routeschedules = db.routeSchedules.Include(r => r.busRoute);
            return View(routeschedules.ToList());
        }

        // GET: /MVRouteSchedule/Details/5
        // handles detail requests specific to the id of the route schedule requested, view only renders if id and route schedule exist
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeschedule = db.routeSchedules.Find(id);
            if (routeschedule == null)
            {
                return HttpNotFound();
            }
            return View(routeschedule);
        }

        // GET: /MVRouteSchedule/Create
        // handles create requests and returns view to create a route schedule
        public ActionResult Create()
        {
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode");
            return View();
        }

        // POST: /MVRouteSchedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a route schedule after clicking "create", if valid data, return to index with newly created route schedule
        public ActionResult Create([Bind(Include="routeScheduleId,busRouteCode,startTime,isWeekDay,comments")] routeSchedule routeschedule)
        {
            if (ModelState.IsValid)
            {
                db.routeSchedules.Add(routeschedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeschedule.busRouteCode);
            return View(routeschedule);
        }

        // GET: /MVRouteSchedule/Edit/5
        // handles edit requests specific to the id of the route schedule requested, view only renders if route schedule and id exist
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeschedule = db.routeSchedules.Find(id);
            if (routeschedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeschedule.busRouteCode);
            return View(routeschedule);
        }

        // POST: /MVRouteSchedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to route schedule
        public ActionResult Edit([Bind(Include="routeScheduleId,busRouteCode,startTime,isWeekDay,comments")] routeSchedule routeschedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeschedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.busRouteCode = new SelectList(db.busRoutes, "busRouteCode", "routeName", routeschedule.busRouteCode);
            return View(routeschedule);
        }

        // GET: /MVRouteSchedule/Delete/5
        // handles delete requests specific to the id of the route schedule requested, view only renders if the route schedule and id exist
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            routeSchedule routeschedule = db.routeSchedules.Find(id);
            if (routeschedule == null)
            {
                return HttpNotFound();
            }
            return View(routeschedule);
        }

        // POST: /MVRouteSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of route schedule at delete view, once route schedule is deleted return to index
        public ActionResult DeleteConfirmed(int id)
        {
            routeSchedule routeschedule = db.routeSchedules.Find(id);
            db.routeSchedules.Remove(routeschedule);
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
