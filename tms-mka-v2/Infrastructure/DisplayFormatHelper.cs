using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace tms_mka_v2.Infrastructure
{
    public class DisplayFormatHelper
    {
        public NumberFormatInfo NumberFormat()
        {
            var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            format.NumberGroupSeparator = ".";
            format.NumberDecimalSeparator = ",";
            return format;
        }

        public string FullDateShortMonthFormatWithTime = "d MMM yyyy hh:mm"; // 1 Jan 2014
        public string FullDateShortMonthFormat = "d MMM yyyy"; // 1 Jan 2014
        public string FullDateFormat = "d MMMM yyyy"; //1 Januari 2014
        public string MonthYearDateFormat = "MMMM yyyy"; //Januari 2014
        public string CompactMonthYearDateFormat = "MMM\\'yy"; //Jan'14
        public string SqlDateFormat = "yyyy-MM-dd"; //2014-01-31
        public string CompactDateFormat = "M/d/yy"; //5/18/14
        public string JavascriptDateFormat = "o"; //Jun 5, 2014 format yang bs diparse oleh javascript Date()
        public string MonthDateFormat = "MMMM"; //Januari
        //public string JavascriptDateFormat = "MMM d, yyyy"; //Jun 5, 2014 format yang bs diparse oleh javascript Date()

        public string DateTimeFormat = "d MMM hh:mm"; //1 Jan 12:34
        public string TimeFormat = "hh:mm"; //12:34

        public string JsShortMonthYearFormat = "MMM yy"; //Dec 15

        /// <summary>
        /// Produces optional, URL-friendly version of a title, "like-this-one". 
        /// hand-tuned for speed, reflects performance refactoring contributed
        /// by John Gietzen (user otac0n) 
        /// </summary>
        public string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        public string GetMonthName(int month)
        {
            string name = "";
            if (month > 0 && month <= 12)
            {
                DateTime date = new DateTime(DateTime.Now.Year, month, 1);
                name = date.ToString(this.MonthDateFormat);
            }

            return name;
        }
    }
}