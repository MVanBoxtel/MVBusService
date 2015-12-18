/* MVValidationsController.cs
 * Assignment 6
 * Revision History
 *      Matt Van Boxtel, 2015.11.14: Created
 *      Matt Van Boxtel, 2015.11.16: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using MVBusService.Models;
using System.ComponentModel.DataAnnotations;
using MVBusService.App_GlobalResources;

namespace MVBusService.Controllers
{
    public class MVValidationsController : Controller
    {
        // create new context to allow us to communicate with db via keywords
        private busServiceContext db = new busServiceContext();
        // GET: MVValidations
        // method used to check JSON object data for the province code, if the data is invalid return error message
        public JsonResult ProvinceCodeClean(string provinceCode)
        {
            Regex regExp = new Regex("^[A-Za-z]{2}$");
            var provinces = db.provinces;

            try
            {
                province checkProvinceCode = db.provinces.Find(provinceCode);
                if (!regExp.IsMatch(provinceCode))
                {
                    return Json(MVTranslations.invalidProvinceCode, JsonRequestBehavior.AllowGet);
                }
                else if (checkProvinceCode == null)
                {
                     return Json(MVTranslations.noProvinceCode, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(MVTranslations.errorValidateProvinceCode + ex.GetBaseException().Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}