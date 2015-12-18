/* PostalCodeValidationAtt.cs
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
    public class PostalCodeValidationAtt : ValidationAttribute
    {
        // method called to check if postal code is valid canadian
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString() == "")
            {
                return ValidationResult.Success;
            }

            Regex regExp = new Regex(@"[ABCEGHJKLMNPRSTVXY][0-9][A-Z]\s?[0-9][A-Z][0-9]", RegexOptions.IgnoreCase);

            if (!regExp.IsMatch(value.ToString()))
            {
                return new ValidationResult(string.Format(MVTranslations.postalCodeInvalid, validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
