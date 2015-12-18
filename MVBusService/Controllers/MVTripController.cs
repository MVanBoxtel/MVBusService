/* MVTripController.cs
 * Assignment 4
 * Revision History
 *      Matt Van Boxtel, 2015.10.07: Created
 *      Matt Van Boxtel, 2015.10.13: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVBusService.Models;

namespace MVBusService.Controllers
{
    /// <summary>
    /// this controller handles requests and responses for trips
    /// </summary>
    public class MVTripController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();

        // GET: MVTrip
        // handles index requests and returns a view which renders all the trips
        public ActionResult Index(string busRouteCode, string busRouteName)
        {
            // set arguement variables to local variables
            string busRouteCodeValue = busRouteCode;
            string busRouteNameValue = busRouteName;

            // If sessions set, set them to local vars
            if (Session["busRouteCodeValue"] != null && Session["busRouteNameValue"] != null)
            {
                busRouteCodeValue = Session.Contents["busRouteCodeValue"].ToString();
                busRouteNameValue = Session.Contents["busRouteNameValue"].ToString();
            }
            // if there are no local vars return to busRoute index
            else if (busRouteCodeValue == null && busRouteNameValue == "")
            {
                TempData["message"] = "Please Select a route";
                return RedirectToAction("Index", "MVBusRoute");
            }
            // set sessions to local vars
            else
            {
                Session["busRouteCodeValue"] = busRouteCodeValue;
                Session["busRouteNameValue"] = busRouteNameValue;
            }

            // create trip model to display the records
            var tripRecords = from record in db.trips
                              where (record.routeSchedule.busRoute.busRouteCode == busRouteCode)
                              orderby record.tripDate, record.routeSchedule.startTime
                              select new TripModel
                              {
                                  date = record.tripDate,
                                  startTime = record.routeSchedule.startTime,
                                  driver = record.driver.lastName + ", " + record.driver.firstName,
                                  busNumber = record.bus.busNumber,
                                  comments = record.comments
                              };

            // set ViewBag data
            ViewBag.busRouteCode = busRouteCode;
            ViewBag.busRouteName = busRouteName;
            
            return View(tripRecords);
        }

        // handles create requests and returns view to create a trip
        public ActionResult Create()
        {
            // try to get session, null reference sends user to bus route index
            try
            {
                // set local var to session
                string busRouteCodeValue = Session.Contents["busRouteCodeValue"].ToString();

                // get the routeSchedules
                var routeSchedules = from record in db.routeSchedules
                                     where record.busRouteCode == busRouteCodeValue
                                     orderby record.isWeekDay, record.startTime
                                     select record;

                // create selectList
                IEnumerable<SelectListItem> routeScheduleList = new SelectList(routeSchedules, "routeScheduleId", "startTime");
                ViewBag.routeScheduleList = routeScheduleList;

                // get drivers
                var drivers = from record in db.drivers
                              orderby record.lastName, record.firstName
                              select record;

                // create driverList
                IEnumerable<SelectListItem> driverList = new SelectList(drivers, "driverId", "fullName");
                ViewBag.driverList = driverList;

                // get buses
                var buses = from record in db.buses
                            where record.status == "available"
                            orderby record.busNumber
                            select record;

                // create busList
                var busList = new SelectList(buses, "busId", "busNumber");
                ViewData["busList"] = busList;


                return View();
            }
            // catch null reference and send user back to select a route
            catch (Exception)
            {
                TempData["message"] = "You must select a bus route";
                return RedirectToAction("Index", "MVBusRoute");
            }    
        }

        [HttpPost]
        // method called when creating a trip after clicking "create", if valid data, return to index with newly created trip
        public ActionResult Create(trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.trips.Add(trip);
                    db.SaveChanges();
                    TempData["message"] = "Trip added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null) ex = ex.InnerException;
                    ModelState.AddModelError("", "error on insert: " + ex.Message);
                }                
            }

            ViewBag.routeScheduleList = new SelectList(db.routeSchedules, "routeScheduleId", "startTime");
            ViewBag.driverList = new SelectList(db.drivers, "driverId", "fullName");
            ViewData["busList"] = new SelectList(db.buses, "busId", "busNumber");

            return View(trip);
        }
    }
}