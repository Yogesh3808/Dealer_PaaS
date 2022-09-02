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
    /// Summary description for clsBankStateMent
    /// </summary>
    public class clsBankStateMent
    {
        public clsBankStateMent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private bool bSaveHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID, string sDocName)
        {
            try
            {

                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {                    
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    //Sujata 17012011
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_BankStateMentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["BankSTMTNo"], dtHdr.Rows[0]["BankSTMTDate"], dtHdr.Rows[0]["Bank_ID"], dtHdr.Rows[0]["CurrancyFactor"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["DocHdrText"], dtHdr.Rows[0]["Assignment"], dtHdr.Rows[0]["BankSTMT_Confirm"], dtHdr.Rows[0]["TotalAmount"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_BankStateMentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["BankSTMTNo"], dtHdr.Rows[0]["BankSTMTDate"], dtHdr.Rows[0]["Bank_ID"], dtHdr.Rows[0]["CurrancyFactor"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["TTRefDate"], dtHdr.Rows[0]["DocHdrText"], dtHdr.Rows[0]["Assignment"], dtHdr.Rows[0]["BankSTMT_Confirm"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["BankCharges"]);
                    //Sujata 17012011
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    //Sujata 17012011
                    //objDB.ExecuteStoredProcedure("SP_BankStateMentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["BankSTMTNo"], dtHdr.Rows[0]["BankSTMTDate"], dtHdr.Rows[0]["Bank_ID"], dtHdr.Rows[0]["CurrancyFactor"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["DocHdrText"], dtHdr.Rows[0]["Assignment"], dtHdr.Rows[0]["BankSTMT_Confirm"], dtHdr.Rows[0]["TotalAmount"]);
                    objDB.ExecuteStoredProcedure("SP_BankStateMentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["BankSTMTNo"], dtHdr.Rows[0]["BankSTMTDate"], dtHdr.Rows[0]["Bank_ID"], dtHdr.Rows[0]["CurrancyFactor"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["TTRefDate"], dtHdr.Rows[0]["DocHdrText"], dtHdr.Rows[0]["Assignment"], dtHdr.Rows[0]["BankSTMT_Confirm"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["BankCharges"]);
                    //Sujata 17012011
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool bSaveRecordWithBankSTMT(DataTable dtHdr, DataTable dtDet, string DocName)
        {
            int iHdrID = 0;

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, dtHdr, ref iHdrID, DocName) == false) goto ExitFunction;

                //saveDetails
                if (bSaveBankSTMTDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
            ExitFunction:
                objDB.RollbackTransaction();
                bSaveRecord = false;
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
        private bool bSaveBankSTMTDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    //{
                    if (dtDet.Rows[iRowCnt]["WarrantyClaimHID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_BankStateMentDtls_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["WarrantyClaimHID"], dtDet.Rows[iRowCnt]["BankSTMTApproved"], dtDet.Rows[iRowCnt]["BankRef_No"]);
                    }
                    //}
                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
                bSaveRecord = false;
            }
            return bSaveRecord;
        }
        public DataSet GetBankStatement(int iBankSTMTID, string sBankStatement, int iDealerID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetBankStatement", iBankSTMTID, sBankStatement, iDealerID);
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
        public DataSet GetBankSTMTWarrantyClaimWise(string sWarrantyID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetBankSTMTWarrantyClaimWise", sWarrantyID);
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
    }
}
