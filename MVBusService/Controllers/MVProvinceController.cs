/* MVProvinceController.cs
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

namespace MVBusService.Controllers
{
    public class MVProvinceController : Controller
    {
        /// <summary>
        /// This controller handles requests and responses for countries
        /// </summary>
        private busServiceContext db = new busServiceContext();

        // GET: MVProvince
        // handles index requests and returns a view which renders all the Provinces
        public ActionResult Index()
        {
            var provinces = db.provinces.Include(p => p.country);
            return View(provinces.ToList());
        }

        // GET: MVProvince/Details/5
        // handles detail requests specific to the id of the province requested, view only renders if id and province exist
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // GET: MVProvince/Create
        // handles create requests and returns view to create a Province
        public ActionResult Create()
        {
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name");
            return View();
        }

        // POST: MVProvince/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a province after clicking "create", if valid data, return to index with newly created province
        public ActionResult Create([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.provinces.Add(province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // GET: MVProvince/Edit/5
        // handles edit requests specific to the id of the province requested, view only renders if province and id exist
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // POST: MVProvince/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to province
        public ActionResult Edit([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // GET: MVProvince/Delete/5
        // handles delete requests specific to the id of the province requested, view only renders if the province and id exist
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: MVProvince/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of Province at delete view, once Province is deleted return to index
        public ActionResult DeleteConfirmed(string id)
        {
            province province = db.provinces.Find(id);
            db.provinces.Remove(province);
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
