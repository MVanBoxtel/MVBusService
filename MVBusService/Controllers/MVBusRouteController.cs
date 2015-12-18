/* MVBusRouteController.cs
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
    /// this controller handles requests and responses for bus routes
    /// </summary>
    public class MVBusRouteController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: /MVBusRoute/
        // handles index requests and returns a view which renders all the bus routes
        public ActionResult Index()
        {
            return View(db.busRoutes.ToList());
        }

        // GET: /MVBusRoute/Details/5
        // handles detail requests specific to the id of the bus route requested, view only renders if id and busroute exist
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busroute = db.busRoutes.Find(id);
            if (busroute == null)
            {
                return HttpNotFound();
            }
            return View(busroute);
        }

        // GET: /MVBusRoute/Create
        // handles create requests and returns view to create a bus route
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MVBusRoute/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a bus route after clicking "create", if valid data, return to index with newly created bus route
        public ActionResult Create([Bind(Include="busRouteCode,routeName")] busRoute busroute)
        {
            if (ModelState.IsValid)
            {
                db.busRoutes.Add(busroute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(busroute);
        }

        // GET: /MVBusRoute/Edit/5
        // handles edit requests specific to the id of the bus route requested, view only renders if bus route and id exist
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busroute = db.busRoutes.Find(id);
            if (busroute == null)
            {
                return HttpNotFound();
            }
            return View(busroute);
        }

        // POST: /MVBusRoute/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to bus route
        public ActionResult Edit([Bind(Include="busRouteCode,routeName")] busRoute busroute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(busroute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(busroute);
        }

        // GET: /MVBusRoute/Delete/5
        // handles delete requests specific to the id of the bus route requested, view only renders if the bus route and id exist
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            busRoute busroute = db.busRoutes.Find(id);
            if (busroute == null)
            {
                return HttpNotFound();
            }
            return View(busroute);
        }

        // POST: /MVBusRoute/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of bus route at delete view, once bus route is deleted return to index
        public ActionResult DeleteConfirmed(string id)
        {
            busRoute busroute = db.busRoutes.Find(id);
            db.busRoutes.Remove(busroute);
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
