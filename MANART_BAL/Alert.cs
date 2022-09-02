using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for Alert
    /// </summary>
    public static class Alert
    {
        /// <summary> 
        /// Shows a client-side JavaScript alert in the browser. 
        /// </summary> 
        /// <param name="message">The message to appear in the alert.</param> 
        public static void Show(string message)
        {
            // Cleans the message to allow single quotation marks 
            string cleanMessage = message.Replace("'", "\'");
            string script = "<script type='text/javascript'>alert('" + cleanMessage + "');</script>";

            // Gets the executing web page 
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }
        }

        [Flags]
        public enum DateRangeOptions : byte
        {
            Week = 1, Month = 2, Quarter = 4, Year = 5
        }

        public struct DateRangeStruct
        {
            public DateTime startDate;
            public DateTime endDate;
        }

        public static DateRangeStruct DateRange(DateRangeOptions DRO, DateTime relativeDate)
        {
            DateTime[] retValue = { DateTime.Today, DateTime.Today };
            DateTime myDate = relativeDate;

            switch (DRO)
            {
                case DateRangeOptions.Week:

                    if (myDate.DayOfWeek > 0)
                    {
                        myDate = myDate.AddDays(-1 * Convert.ToInt32(myDate.DayOfWeek));
                    }

                    retValue[0] = myDate;
                    retValue[1] = myDate.AddDays(6);

                    break;

                case DateRangeOptions.Month:

                    if (myDate.Day > 1) myDate = myDate.AddDays(-1 * (myDate.Day - 1));

                    retValue[0] = myDate;
                    retValue[1] = myDate.AddMonths(1);
                    retValue[1] = retValue[1].AddDays(-1);

                    break;

                case DateRangeOptions.Quarter:

                    if (myDate.Month < 4) retValue[0] = Convert.ToDateTime("1/1/" +
                                myDate.Year.ToString());
                    if (myDate.Month > 3 && myDate.Month < 7) retValue[0] =
                                Convert.ToDateTime("4/1/" + myDate.Year.ToString());
                    if (myDate.Month > 6 && myDate.Month < 10) retValue[0] =
                                Convert.ToDateTime("7/1/" + myDate.Year.ToString());
                    if (myDate.Month > 9) retValue[0] = Convert.ToDateTime("10/1/" +
                                myDate.Year.ToString());

                    retValue[1] = retValue[0].AddMonths(3);
                    retValue[1] = retValue[1].AddDays(-1);

                    break;

                case DateRangeOptions.Year:

                    retValue[0] = Convert.ToDateTime("1/1/" + myDate.Year.ToString());
                    retValue[1] = Convert.ToDateTime("12/31/" + myDate.Year.ToString());

                    break;
            }

            DateRangeStruct retVal;
            retVal.startDate = retValue[0];
            retVal.endDate = retValue[1];

            return retVal;
        }

        public static DateRangeStruct DateRange(DateRangeOptions DRO, string relativeDate)
        {
            return DateRange(DRO, Convert.ToDateTime(relativeDate));
        }



    }
}
