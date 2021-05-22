using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Validator
    {
        public static string ValidateString(string text)
        {
            if (text != null)
            {
                text = text.ToLower();
                text = text.Trim();
                if (text.Contains("   "))
                {
                    text = text.Replace("   ", " ");
                    text = ValidateString(text);
                }
                if (text.Contains("  "))
                {
                    text = text.Replace("  ", " ");
                    text = ValidateString(text);
                }
            }
            return text;
        }
        public static string ValidateStringWithoutLower(string text)
        {
            if (text != null)
            {
                text = text.Trim();
                if (text.Contains("   "))
                {
                    text = text.Replace("   ", " ");
                    text = ValidateString(text);
                }
                if (text.Contains("  "))
                {
                    text = text.Replace("  ", " ");
                    text = ValidateString(text);
                }
            }
            return text;
        }
        public static decimal ValidateDecimal(string price)
        {
            if (price != null)
            {
                price = price.Trim();
                try
                {
                    return decimal.Parse(price);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }
        public static int ValidateInt(string id)
        {
            try
            {
                if (id != "") 
                { 
                    return int.Parse(id.Trim(' ')); 
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
