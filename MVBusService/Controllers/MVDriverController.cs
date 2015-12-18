/* MVDriverController.cs
 * Assignment 5
 * Revision History
 *      Matt Van Boxtel, 2015.10.26: Created
 *      Matt Van Boxtel, 2015.10.29: Completed
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
using MVBusService.App_GlobalResources;

namespace MVBusService.Controllers
{
    /// <summary>
    /// This controller handles requests and responses for drivers
    /// </summary>
    public class MVDriverController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: MVDriver
        // handles index requests and returns a view which renders all the trips
        public ActionResult Index()
        {
            var drivers = db.drivers.Include(d => d.province).OrderBy(m => m.fullName);
            return View(drivers.ToList());
        }

        // GET: MVDriver/Details/5
        // handles detail requests specific to the id of the driver requested, view only renders if id and driver exist
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: MVDriver/Create
        // handles create requests and returns view to create a driver
        public ActionResult Create()
        {
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(m => m.name), "provinceCode", "name");
            return View();
        }

        // POST: MVDriver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a driver after clicking "create", if valid data, return to index with newly created driver
        public ActionResult Create([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.drivers.Add(driver);
                    db.SaveChanges();
                    TempData["message"] = MVTranslations.driverAdded + " " + driver.fullName;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", MVTranslations.createDriverException + " " + ex.GetBaseException().Message);
            }


            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(m => m.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // GET: MVDriver/Edit/5
        // handles edit requests specific to the id of the driver requested, view only renders if driver and id exist
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.driverName = driver.fullName;
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(m => m.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // POST: MVDriver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to driver
        public ActionResult Edit([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                driver.fullName = driver.lastName + ", " + driver.firstName;

                if (ModelState.IsValid)
                {
                    db.Entry(driver).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = MVTranslations.updateSuccessful;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", MVTranslations.editDriverException + " " + ex.GetBaseException().Message);
            }

            Create();
            return View(driver);
        }

        // GET: MVDriver/Delete/5
        // handles delete requests specific to the id of the driver requested, view only renders if the bus route and id exist
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: MVDriver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of driver at delete view, once driver is deleted return to index
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                driver driver = db.drivers.Find(id);
                db.drivers.Remove(driver);
                db.SaveChanges();
                TempData["message"] = MVTranslations.deleteSuccessful;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["message"] = MVTranslations.deleteError + " " + ex.Message.ToString();
                return Delete(id);
            }
            
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
