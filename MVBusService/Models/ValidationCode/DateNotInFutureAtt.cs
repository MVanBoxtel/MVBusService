/* DateNotInFutureAtt.cs
 * Assignment 8
 * Revision History
 *      Matt Van Boxtel, 2015.12.05: Created
 *      Matt Van Boxtel, 2015.12.08: Completed
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MVBusService.App_GlobalResources;

namespace MVBusService.Models
{
    public class DateNotInFutureAtt : ValidationAttribute
    {
        // method called to check if date is a valid format and not in the future
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            try
            {
                DateTime date = (DateTime)value;
                if (date > DateTime.Now)
	            {
		            return new ValidationResult(string.Format(MVTranslations.dateInFuture, validationContext.DisplayName));
	            }
                else
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception)
            {
                return new ValidationResult(string.Format(MVTranslations.dateWrongFormat, validationContext.DisplayName));
            }
        }
    }
}
