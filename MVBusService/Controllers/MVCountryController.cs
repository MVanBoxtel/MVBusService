/* MVCountryController.cs
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
    /// <summary>
    /// This controller handles requests and responses for countries
    /// </summary>
    public class MVCountryController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: MVCountry
        // handles index requests and returns a view which renders all the countries
        public ActionResult Index()
        {
            return View(db.countries.ToList());
        }

        // GET: MVCountry/Details/5
        // handles detail requests specific to the id of the Country requested, view only renders if id and Country exist
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: MVCountry/Create
        // handles create requests and returns view to create a Country
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVCountry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when creating a Country after clicking "create", if valid data, return to index with newly created Country
        public ActionResult Create([Bind(Include = "countryCode,name,postalPattern,phonePattern")] country country)
        {
            if (ModelState.IsValid)
            {
                db.countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: MVCountry/Edit/5
        // handles edit requests specific to the id of the Country requested, view only renders if Country and id exist
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: MVCountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // method called when confirming changes at the edit view, if valid data, return to index view with changes to Country
        public ActionResult Edit([Bind(Include = "countryCode,name,postalPattern,phonePattern")] country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: MVCountry/Delete/5
        // handles delete requests specific to the id of the country requested, view only renders if the country and id exist
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country country = db.countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: MVCountry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // method called when confirming deletion of Country at delete view, once Country is deleted return to index
        public ActionResult DeleteConfirmed(string id)
        {
            country country = db.countries.Find(id);
            db.countries.Remove(country);
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
