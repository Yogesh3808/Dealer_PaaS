using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MANART_DAL
{
    public static class Func
    {
        public static clsDB DB = new clsDB();
        public static clsConvert Convert = new clsConvert();
        public static clsCommon Common = new clsCommon();
        public static clsException Exception = new clsException();
        //To Get CurrentFinancialYear        
        public static string sGetFinancialYear(int iDealerID)
        {
            string sFinancialYear = "";
            int iCurrentMonth = DateTime.Today.Month;
            int iCurrentYear = 0;
            int iTmpYear = 0;

            //iTmpYear = DateTime.Today.Year;
            //iTmpYear = 2016;
            iTmpYear = Func.Convert.iConvertToInt(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetDealerFinYear, iDealerID, ""));

            iCurrentYear = Func.Convert.iConvertToInt(iTmpYear.ToString().Substring(2));
            sFinancialYear = iTmpYear.ToString();
            //Sujata 21042011 On portal financial year should be considered from Jan to Dec
            //if (iCurrentMonth >= 4 && iCurrentMonth <= 12)// month is between Apr to Dec
            //{
            //    sFinancialYear = iCurrentYear.ToString() + (iCurrentYear + 1).ToString();
            //}
            //else if (iCurrentMonth >= 1 && iCurrentMonth <= 3)// month is between Jan to March
            //{
            //    sFinancialYear = (iCurrentYear - 1).ToString("00") + iCurrentYear.ToString();
            //}
            //Sujata 21042011
            return sFinancialYear;
        }
    }
}
