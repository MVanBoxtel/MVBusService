/* MVDriverMetaData.cs
 * Assignment 6
 * Revision History
 *      Matt Van Boxtel, 2015.10.26: Created
 *      Matt Van Boxtel, 2015.11.16: Completed
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVClassLibrary;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace MVBusService.Models
{
    // partial class with metadata type annotation which links the metadata to the driver table
    [MetadataType(typeof(MVDriverMetadata))]
    public partial class driver : IValidatableObject
    {
        // method called to validate all data before save
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            MVValidations validations = new MVValidations();
            firstName = validations.Capitalize(firstName.Trim());
            lastName = validations.Capitalize(lastName.Trim());
            fullName = lastName + ", " + firstName;
            homePhone = validations.FormatPhoneNumber(homePhone);

            if (workPhone != null)
	        {
		        workPhone = validations.FormatPhoneNumber(workPhone);
	        }
            
            if (workPhone != null)
            {
                provinceCode = provinceCode.ToUpper();
            }

            if (workPhone != null)
            {
                if (!postalCode.Contains(" "))
                {
                    postalCode = postalCode.ToUpper();
                    postalCode = postalCode.Insert(3, " ");
                }
                else
                {
                    postalCode = postalCode.ToUpper();
                }                
            }

            yield return ValidationResult.Success;
        }
    }

    // metadata class for the driver model
    // - contains the model properties as targets for annotations
    // displays and maintins the driver table
    public class MVDriverMetadata
    {
        public int driverId { get; set; }
        [Display(Name = "firstName", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string firstName { get; set; }
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(App_GlobalResources.MVTranslations))]
        [Display(Name = "lastName", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string lastName { get; set; }
        [Display(Name = "fullName", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string fullName { get; set; }
        [Display(Name = "homePhone", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string homePhone { get; set; }
        [Display(Name = "workPhone", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string workPhone { get; set; }
        [Display(Name = "street", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string street { get; set; }
        [Display(Name = "city", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string city { get; set; }
        [PostalCodeValidationAtt]
        [Display(Name = "postalCode", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string postalCode { get; set; }
        [Remote("ProvinceCodeClean", "MVValidations")]
        [Display(Name = "provinceCode", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        public string provinceCode { get; set; }
        [DateNotInFutureAtt]
        [DisplayFormat(DataFormatString = "{0: dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "dateHired", ResourceType = typeof(App_GlobalResources.MVTranslations))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(App_GlobalResources.MVTranslations))]
        public Nullable<System.DateTime> dateHired { get; set; }
    }
}