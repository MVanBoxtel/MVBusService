/* PostalCodeValidationAttribute.cs
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
    public class PostalCodeValidationAttribute : ValidationAttribute
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
                return new ValidationResult(string.Format("{0} Not a valid Canadian Postal code Pattern: A3A 3A3", validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
