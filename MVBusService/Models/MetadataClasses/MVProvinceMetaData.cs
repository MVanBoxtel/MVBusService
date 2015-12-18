/* MVDriverController.cs
 * Assignment 5
 * Revision History
 *      Matt Van Boxtel, 2015.10.26: Created
 *      Matt Van Boxtel, 2015.10.29: Completed
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVBusService.Models
{
    // partial class with metadata type annotation which links the metadata to the province table
    [MetadataType(typeof(MVProvinceMetadata))]
    public partial class province
    {

    }

    // metadata class for the province model
    // - contains the model properties as targets for annotations
    // displays and maintins the province table
    public class MVProvinceMetadata
    {
        [Display(Name = "Province")]
        public string provinceCode { get; set; }
        [Display(Name = "Province")]
        public string name { get; set; }
        public string countryCode { get; set; }
        public string taxCode { get; set; }
        public double taxRate { get; set; }
        public string capital { get; set; }
    }
}