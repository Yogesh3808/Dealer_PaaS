using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsDealerTarget
    /// </summary>
    public class clsDealerTarget
    {
        DataSet dsCheck;
        public clsDealerTarget()
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


        public DataSet GetCSMDealerExport(int UserID, string output)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_FillDealerForExport", UserID, output);
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
        public DataSet GetCSMDealerDomestic(int UserID, string output)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_FillDealerForDomestic", UserID, output);
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
        public DataSet GetDealerTarget(int DealerID, int iYear)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_ServPromo_Dlrwise_Target_Display", DealerID, iYear);
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
                return objDB.ExecuteQueryAndGetDataset("Select * from TM_ServPromo_Dlrwise_Target where Dealer_ID=" + DealerID + " and Year_Id=" + YearId + "");
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
        public int bSaveDlrwiseTarget(int iID, int sDealer_Id, int Parameter_Id, string Target, string CrDate, string Flag, int _Year, string TrDate, string _Type)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_ServPromo_Dlrwise_Target_Save", iID, sDealer_Id, Parameter_Id, Target, CrDate, Flag, _Year, TrDate, _Type);

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

        //public int bSaveDlrwiseTarget(int iID, DataTable SaveDt)
        //{
        //    int j = 0;
        //    bool bSaveRecord = false;
        //    try
        //    {
        //        if (SaveDt.Rows.Count != 0)
        //        {

        //            // objDB.BeginTranasaction();
        //            foreach (DataRow dr in SaveDt.Rows)
        //            {
        //                objDB.BeginTranasaction();
        //                iID = objDB.ExecuteStoredProcedure("sp_ServPromo_Dlrwise_Target_Save", iID, dr["Dealer_ID"], dr["Parameter_ID"], dr["Target"], dr["Cr_Date"], dr["Flag"], dr["Year_ID"], dr["Target_Date"]);
        //                objDB.CommitTransaction();
        //                if (iID > 0)
        //                {
        //                    j = j + 1;
        //                }
        //            }
        //            // objDB.CommitTransaction();
        //            bSaveRecord = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objDB.RollbackTransaction();

        //    }
        //    //if (iID > 0)
        //    //{
        //    //    j = j + 1;
        //    //}
        //    return j;
        //}




    }
}
