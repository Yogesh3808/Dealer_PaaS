using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MANART_DAL
{
    /// <summary>
    /// Use for all types of conversion
    /// </summary>
    public class clsConvert
    {
        public clsConvert()
        {

        }
        public string sConvertToString(object objValue)
        {
            string sConvertToString = "";
            try
            {
                sConvertToString = Convert.ToString(objValue);
            }
            catch
            {
            }
            return sConvertToString;
        }
        public int iConvertToInt(object objValue)
        {
            int iConvertToInt = 0;
            try
            {
                iConvertToInt = Convert.ToInt32(objValue);
            }
            catch
            {

            }
            return iConvertToInt;

        }
        public double dConvertToDouble(object objValue)
        {
            double dConvertTodouble = 0;
            try
            {
                dConvertTodouble = Convert.ToDouble(objValue);
            }
            catch
            {

            }
            return dConvertTodouble;

        }
        public bool bConvertToBoolean(object objValue)
        {
            bool bConvertToBoolean = false;
            try
            {
                bConvertToBoolean = Convert.ToBoolean(objValue);
            }
            catch
            {
            }
            return bConvertToBoolean;
        }
        public string tConvertToDate(object objValue, bool bWithTime)
        {
            string tConvertToDate = "";
            string sValue = sConvertToString(objValue);
            if (sValue == "") return "";
            try
            {
                if (bWithTime == true)
                {
                    tConvertToDate = Convert.ToDateTime(sValue).ToString("dd-MMM-yyyy HH:mm");
                }
                else
                {
                    //DateTime.Parse(sValue, new CultureInfo("en-GB"));
                    tConvertToDate = Convert.ToDateTime(sValue).ToString("dd/MM/yyyy");
                }
            }
            catch
            {
            }
            return tConvertToDate;
        }
        public string tConvertToDate1(string sValue, bool bWithTime)
        {
            string tConvertToDate1 = "";             
            if (sValue == "") return "";
            try
            {
                if (bWithTime == true)
                {
                    tConvertToDate1 = Convert.ToDateTime(sValue).ToString("dd-MMM-yyyy HH:mm");
                }
                else
                {
                    //DateTime.Parse(sValue, new CultureInfo("en-GB"));
                    tConvertToDate1 = Convert.ToDateTime(sValue).ToString("dd/MM/yyyy");
                }
            }
            catch
            {
            }
            return tConvertToDate1;
        }
    }
}
