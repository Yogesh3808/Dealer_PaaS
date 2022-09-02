using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsDealerTargetValues
    /// </summary>
    public class clsDealerTargetValues
    {
        DataSet dsCheck;
        public clsDealerTargetValues()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetYear()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("select Id,Year from M_Year");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public int bSaveDlrwiseActualValue(int iID, int sDealer_Id, int Parameter_Id, string ActualValue, string CrDate, string Flag, int _Year, string ActualValueDate, int _YearSelection, string _Type)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_ServPromo_Dlrwise_ActualValue_Save", iID, sDealer_Id, Parameter_Id, ActualValue, CrDate, Flag, _Year, ActualValueDate, _YearSelection, _Type);
                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet GetDealerActualValues(int DealerID, int iYear, int iYearValue)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_ServPromo_Dlrwise_ActualValues_Display", DealerID, iYear, iYearValue);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        public DataSet checkRecord(int DealerID, int YearId)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                return objDB.ExecuteQueryAndGetDataset("Select * from TM_ServPromo_Dlrwise_Values where Dealer_ID=" + DealerID + " and Year_Id=" + YearId + "");

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
