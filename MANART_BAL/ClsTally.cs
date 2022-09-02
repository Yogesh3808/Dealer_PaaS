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
    public class ClsTally
    {
        public ClsTally()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetCustSuppList(int DealerID, string Type)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerCustSupplist", DealerID,Type);
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

        public DataSet UploadCustomerDetailsAndGetCustomerDetails(string sFileName, int iDealerId, DataTable dtDet, string sType)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedCustomerDetails(sFileName, iDealerId, dtDet, sType) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportCustomerDetailsFromExcelSheet", sFileName, iDealerId, sType);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        private bool bSaveUploadedCustomerDetails(string sFileName, int iDealerId, DataTable dtDet, string sType)
        {
            bool bSaveRecord = false;
            string bNewTable = "Y";
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertCustomerTallyDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Name"], dtDet.Rows[iRowCnt]["DCANCode"], dtDet.Rows[iRowCnt]["LedgerName"], bNewTable, sType);
                    bNewTable = "N";
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
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
