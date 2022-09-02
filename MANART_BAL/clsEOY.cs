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
using MANART_BAL;
using MANART_DAL;

namespace MANART_BAL
{
    public class clsEOY
    {
        public clsEOY()
        {
        
        }
        public DataSet GetEOYPendingDoc(int iDealerID, int iHOBranchDealerId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_DealerEOYDocValidation", iDealerID, iHOBranchDealerId);
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

        private bool bSaveEOY(clsDB objDB,int iDealerId, int iDlrBranchID, int iUserID)
        {
            try
            {
                int iHdrID = objDB.ExecuteStoredProcedure("SP_EOYProcessDealer", iDealerId, iDlrBranchID, iUserID);
    
               return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        

        public bool bEOYAction(int iDealerId, int iDlrBranchID, int iUserID)//DataTable dtEstDet,
        {         
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {                
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveEOY(objDB, iDealerId, iDlrBranchID, iUserID) == false) goto ExitFunction;

                bSaveRecord = true;

            ExitFunction:
                if (bSaveRecord == false)
                {
                    objDB.RollbackTransaction();
                    bSaveRecord = false;
                }
                else
                {
                    objDB.CommitTransaction();
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
