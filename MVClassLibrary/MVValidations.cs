/* MVValidations.cs
 * Assignment 6
 * Revision History
 *      Matt Van Boxtel, 2015.11.14: Created
 *      Matt Van Boxtel, 2015.11.16: Completed
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVClassLibrary
{
    public class MVValidations
    {
        //  method called to capitalize the first letter
        public string Capitalize(string value)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                value = value.ToLower().Trim();
                value = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
                return value;
            }
        }

        // method called to format phone numbers
        public string FormatPhoneNumber(string value)
        {
            // First, remove everything except of numbers
            Regex regexObj = new Regex(@"[^\d]");
            value = regexObj.Replace(value, "");
            string fmt = "000-000-0000";

            // Second, format numbers to phone string 
            if (value.Length == 10)
            {
                value = Convert.ToInt64(value).ToString(fmt);
            }

            return value;
        }
    }
}
