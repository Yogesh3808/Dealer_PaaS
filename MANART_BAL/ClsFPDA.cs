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
    /// Summary description for ClsFPDA
    /// </summary>
    public class ClsFPDA
    {
        public ClsFPDA()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public bool bSaveFPDA(string iD, string sDealer_ID, string sAdvice_no, string sAdvice_Date, string sLR_Date, string sLR, string sNoOfCases, string sTransporter, string sRemarks, string CreditDate_From, string CreditDate_To, string Dealer_FPDA_Confirm, string Vecv_FPDA_Confirm, DataTable dtDet, ref int iFPDA_Hdr_ID, string sTrNo)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                if (dtDet.Rows.Count != 0)
                {
                    if (sLR_Date.Length == 0)
                    {
                        sLR_Date = sLR_Date;
                    }
                    string sFinYear = Func.sGetFinancialYear(Convert.ToInt16(sDealer_ID));
                    objDB.BeginTranasaction();

                    iFPDA_Hdr_ID = objDB.ExecuteStoredProcedure("SP_Save_FPDAHeaders", Convert.ToInt16(iD), Convert.ToInt16(sDealer_ID), sAdvice_no, sAdvice_Date, sLR_Date, sLR, sNoOfCases, sTransporter, sRemarks, CreditDate_From, CreditDate_To, Dealer_FPDA_Confirm, Vecv_FPDA_Confirm, sTrNo);
                    foreach (DataRow dr in dtDet.Rows)
                    {
                        //dr["Customer_Name"] ,    dr["Cliam_No"],  dr["Cliam_Date"], dr["Part_Name"],dr["Part_Qty"]
                        //Sujata 05122012_Begin
                        //objDB.ExecuteStoredProcedure("SP_Save_FPDAetails", Convert.ToInt16(iFPDA_Hdr_ID), dr["Customer_Name"], dr["Cliam_No"], (dr["Cliam_Date"]), dr["Part_Name"], dr["Part_Qty"], dr["AccPart_Qty"], dr["Box_no"], dr["Location"]);
                        if (dr["Status"]=="Y")
                            objDB.ExecuteStoredProcedure("SP_Save_FPDAetails", Convert.ToInt16(iFPDA_Hdr_ID), dr["Customer_Name"], dr["Cliam_No"], (dr["Cliam_Date"]), dr["Part_Name"], dr["Part_Qty"], dr["AccPart_Qty"], dr["Box_no"], dr["Location"], dr["ChkForAccept"], dr["RejRemark"]);
                        else
                            objDB.ExecuteStoredProcedure("SP_Save_FPDAetails_PartDeletion", Convert.ToInt16(iFPDA_Hdr_ID), dr["Cliam_No"], dr["Part_Name"]);
                        //Sujata 05122012_End
                    }
                    //Sujata 05122012_Begin
                    //Sujata 07122012_Begin
                    //objDB.ExecuteStoredProcedure("SP_Save_FPDAetails_PartDeletion", Convert.ToInt16(iFPDA_Hdr_ID));
                    //Sujata 07122012_End
                    //Sujata 05122012_End
                    if (Convert.ToInt16(iD) == 0)
                    {
                        //Changes Done by sujata on 12102012-As per Vitthal told
                        //Func.Common.UpdateMaxNo(objDB,sFinYear, "FPDA", Convert.ToInt16(sDealer_ID));
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "F", Convert.ToInt16(sDealer_ID));
                        //Changes Done by sujata on 12102012-As per Vitthal told
                    }
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

        public bool bSaveFPDAReceipt(string iD, DataTable dtDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                if (dtDet.Rows.Count != 0)
                {                   
                    //string sFinYear = Func.sGetFinancialYear(Convert.ToInt16(sDealer_ID));
                    objDB.BeginTranasaction();

                    foreach (DataRow dr in dtDet.Rows)
                    {
                        objDB.ExecuteStoredProcedure("SP_Save_FPDAetailsReceipt", Convert.ToInt16(iD), dr["Cliam_No"], dr["Part_Name"], dr["RecvdYN"], dr["RecvdRemark"], dr["RecvdDate"]);                        
                    }
                    
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

        public bool bSaveFPDAReceiptConfirm(string iD, string sConfirm, string sReceiptRemark, string ReceiptNo, string ReceiptDate, string sDealerCode,int iDealerID,string sReceiptStatus)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {   
             //string sFinYear = Func.sGetFinancialYear(Convert.ToInt16(sDealer_ID));
             objDB.BeginTranasaction();
                
             //if (sReceiptStatus == "N") ReceiptNo = Func.Common.sGetMaxDocNo(sDealerCode, "", "FR", iDealerID);
             if (sReceiptStatus == "N") ReceiptNo = Func.Common.sGetMaxDocNo("D009999", "", "FR", 9999);
             string sFinYear = Func.sGetFinancialYear(9999);
             objDB.ExecuteStoredProcedure("SP_Save_FPDAReceiptConfirm", Convert.ToInt16(iD), sConfirm, sReceiptRemark, ReceiptNo, ReceiptDate);
             //if (sReceiptStatus == "N") Func.Common.UpdateMaxNo(objDB, sFinYear, "FR", iDealerID);
             if (sReceiptStatus == "N") Func.Common.UpdateMaxNo(objDB, sFinYear, "FR", 9999);

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

        /// <summary>
        /// To Get FPDA
        /// </summary>
        /// <param name="iDealerID"></param>
        /// <returns></returns>
        public DataSet GetFPDA(string sSelect, string sRCNumber, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_FPDADetails", sSelect, sRCNumber, iDealerID);
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


        public bool CancelConfirmFPDA(bool bCancelConfirm, bool bVecvConfirm, string iFPDAID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                string sSQlUpdate = "";
                if (bCancelConfirm == true)
                {
                    sSQlUpdate = "Update TM_FPDAWarrantyClaims Set FPDA_Cancel='Y' where ID =" + iFPDAID;
                }
                else if (bCancelConfirm == false)
                {
                    sSQlUpdate = "Update TM_FPDAWarrantyClaims Set Dealer_FPDA_Confirm='Y' where ID =" + iFPDAID;
                }
                else if (bVecvConfirm == false)
                {
                    sSQlUpdate = "Update TM_FPDAWarrantyClaims Set Vecv_FPDA_Confirm='Y' where ID =" + iFPDAID;
                }
                objDB.BeginTranasaction();
                objDB.ExecuteQuery(sSQlUpdate);
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

        public DataSet GetFPDAForReceiptUserWise(string sRegionID, string sContryID, string sDealerID, string sFromDate, string sToDate, int iClaimStatus, int iUserRoleId, string sDomestic_Export, string sSearchText, int StartIndexRow, int MaxRowCount, string sSelRole, string sClaimTypeID, int iUserId, int ModelCategory_ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = null; ;
                //if (sDomestic_Export == "E")
                //{
                //    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetWarrantyClaim_ForProcessingExport", sRegionID, sContryID, sDealerID, sFromDate, sToDate,  iClaimStatus, iUserRoleId, sDomestic_Export, sSearchText, StartIndexRow, MaxRowCount, sSelRole, sClaimTypeID);
                //}
                //else
                //{
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetFPDAForReceipt", sRegionID, sContryID, sDealerID, sFromDate, sToDate, iClaimStatus, iUserRoleId, sDomestic_Export, sSearchText, iUserId, StartIndexRow, MaxRowCount, sSelRole);
                //}
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //    TotalRowCount = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["TotalRowCount"]);
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
