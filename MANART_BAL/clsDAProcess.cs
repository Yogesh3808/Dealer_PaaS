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
    /// Summary description for clsDAProcess
    /// </summary>
    public class clsDAProcess
    {
        public clsDAProcess()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //sujata 14062011
        //public DataSet GetDAProcess(int iID, string sDealerID, string sSelectionType,string FromDate,string ToDate)
        public DataSet GetDAProcess(int iID, string sDealerID, string sSelectionType, string FromDate, string ToDate, int iUserRoleID)
        //sujata 14062011
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                //sujata 14062011
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDAForProcessing", iID, sDealerID, sSelectionType, FromDate,ToDate);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDAForProcessing", iID, sDealerID, sSelectionType, FromDate, ToDate, iUserRoleID);
                //sujata 14062011
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
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

        public bool bsaveDAProcess(DataTable dtDet, string sIsProcessed)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    //sujata 28022011
                    //objDB.ExecuteStoredProcedure("SP_DAProcess_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Cust_Type"], dtDet.Rows[0]["Drive_Type"], dtDet.Rows[0]["Primary_Application"], dtDet.Rows[0]["Secondary_Application"], dtDet.Rows[0]["Road_Type"], dtDet.Rows[0]["Financir_Type"], dtDet.Rows[0]["Load_Type"], dtDet.Rows[0]["Route_Type"], dtDet.Rows[0]["Industry_Type"], dtDet.Rows[0]["ProcessedBy"], sIsProcessed);
                    objDB.ExecuteStoredProcedure("SP_DAProcess_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Cust_Type"], dtDet.Rows[0]["Drive_Type"], dtDet.Rows[0]["Primary_Application"], dtDet.Rows[0]["Secondary_Application"], dtDet.Rows[0]["Road_Type"], dtDet.Rows[0]["Financir_Type"], dtDet.Rows[0]["Load_Type"], dtDet.Rows[0]["Route_Type"], dtDet.Rows[0]["Industry_Type"], dtDet.Rows[0]["ProcessedBy"], sIsProcessed, dtDet.Rows[0]["BillingType"], dtDet.Rows[0]["PlantID"], dtDet.Rows[0]["DepotID"], dtDet.Rows[0]["CustomerCode"]);
                    //sujata 28022011
                    objDB.CommitTransaction();
                    bSaveRecord = true;
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
