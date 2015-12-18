/* DateNotInFutureAttribute.cs
 * Assignment 6
 * Revision History
 *      Matt Van Boxtel, 2015.11.14: Created
 *      Matt Van Boxtel, 2015.11.16: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MVClassLibrary
{
    public class DateNotInFutureAttribute : ValidationAttribute
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
		            return new ValidationResult(string.Format("{0} is in the future", validationContext.DisplayName));
	            }
                else
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception)
            {
                return new ValidationResult(string.Format("{0} is in the wrong format", validationContext.DisplayName));
            }
        }
    }
}
