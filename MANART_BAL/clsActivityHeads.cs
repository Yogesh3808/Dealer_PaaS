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
    /// Summary description for clsActivityHeads
    /// </summary>
    public class clsActivityHeads
    {
        public clsActivityHeads()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string GenerateActiviytNo(string sDealerCode, int iDealerID, int Dept, string DOCType)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                // sFinYearChar = sFinYear.Substring(3);

                if (sFinYear == "2016")
                {
                    sFinYearChar = sFinYear.Substring(3);
                }
                else
                {
                    sFinYearChar = sFinYear;
                }


                string sDocName = "";

                if (Dept == 1 && DOCType=="R")  //Spare Request
                {
                    sDocName = "ARSP";
                }
                else if (Dept == 2 && DOCType == "R")  //Service Request
                {
                    sDocName = "ARSE";
                }
                else if (Dept == 1 && DOCType == "C")  //Spares Claim
                {
                    sDocName = "ACSP";
                }
                else if (Dept == 2 && DOCType == "C")  //Service Claim
                {
                    sDocName = "ACSE";
                }
              

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(4, '0');


                if (sDealerCode != "" && sFinYear == "2016")
                {
                    sDocNo = sDealerCode.Substring(1, 6) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (sDealerCode != "" && sFinYear != "2016")
                {
                    sDocNo = sDealerCode + sDocName + sFinYearChar + sMaxDocNo;
                }
                else
                {
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                }





                return sDocNo;
            }
            catch
            {

                return "0";
            }
        }

        public DataSet GetMaxActivity(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxActivity", ID);
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
        public DataSet GetMaxActivityExpensesHeadMaster(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxActivityExpensesHeadMaster", ID);
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
        public DataSet GetMaxActivityMerchandizeReqMaster(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxActivityMerchandizeReqMaster", ID);
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

        public DataSet GetActivityMaster( int ActivityID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityMaster",  ActivityID );
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
        public DataSet GetActivityExpensesHeadMaster(int ActivityID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityExpensesHeadMaster", ActivityID);
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
        public DataSet GetActivityMerchandizeReqMaster(int ActivityID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityMerchandizeReqMaster", ActivityID);
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
        public int bSaveActivityMaster(int iID, int TypeoftheActivity, string NameoftheActivity, string FromDate, string ToDate, string GLCode, string CostCenter,string active)
        {

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_ActivityMaster",  iID,  TypeoftheActivity,  NameoftheActivity,  FromDate, ToDate,  GLCode,  CostCenter, active);
                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public int bSaveActivityExpensesHead(int iID, string ExpensesHead, string active)
        {

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_ActivityExpensesHeadMaster", iID, ExpensesHead, active);
                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public int bSaveActivityMerchandizeReq(int iID, string MerchandizeReq, string active)
        {

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_ActivityMerchandizeReqMaster", iID, MerchandizeReq, active);
                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetActivityClaimDetailsByActivityID(int ActivityID, int iDealerID, int DeptId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                // dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityForProcessing", sDealerID, sSelectionType, sStatus, FromDate, ToDate,Flag);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityClaimDetailsByActivityID", ActivityID, iDealerID, DeptId);
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
        public bool bSaveActivityRequest(DataTable dtHdr, DataTable dtDtls, DataTable dtDtls1, DataTable dtDocDtls, ref int iHdrID, int Dept, int userID, string DOCType, string DealerCode)
        {


            string ParameterList = "";
            string sDocName = "";
            string IMAPFlag = "";
            int RefClaimID = 0;
            if (Dept == 1 && DOCType == "R")  //Spare Request
            {
                sDocName = "ARSP";
            }
            else if (Dept == 2 && DOCType == "R")  //Service Request
            {
                sDocName = "ARSE";
            }
            else if (Dept == 1 && DOCType == "C")  //Spares Claim
            {
                sDocName = "ACSP";
            }
            else if (Dept == 2 && DOCType == "C")  //Service Claim
            {
                sDocName = "ACSE";
            }
            
            iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
            //IMAPFlag = Func.Convert.sConvertToString(dtHdr.Rows[0]["IMAP_Claim"]);
            //RefClaimID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["RefClaimID"]);
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                if (dtHdr.Rows.Count != 0)
                {

                    if (iHdrID == 0)
                    {
                        objDB.BeginTranasaction();
                       
                        int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                        string sFinYear = Func.sGetFinancialYear(iDealerId);

                        dtHdr.Rows[0]["Activity_Req_No"] = Func.Convert.sConvertToString(GenerateActiviytNo(DealerCode, iDealerId, Dept, DOCType));

                        iHdrID = objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"],
                            dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"],
                            dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"],
                           dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["Comments"],
                           dtHdr.Rows[0]["Total_Budget_Available"], dtHdr.Rows[0]["Budget_Utilized"], dtHdr.Rows[0]["Pending_Budget"],
                            dtHdr.Rows[0]["ExpectedNoCustomers"], dtHdr.Rows[0]["ExpectedNoofVehicles"], dtHdr.Rows[0]["ExpectedPartsBusiness"],
                           dtHdr.Rows[0]["ExpectedServiceRevenue"], dtHdr.Rows[0]["ExpectedLube"], dtHdr.Rows[0]["ActivityClaim_Confirm"],
                           dtHdr.Rows[0]["ActivityClaim_Cancel"],
                           dtHdr.Rows[0]["Apprv_VECV_Amt"], dtHdr.Rows[0]["IGST_SGST_Amt"], dtHdr.Rows[0]["CGST_Amt"], 
                           dtHdr.Rows[0]["Total_Apprv_VECV_Amt"],  dtHdr.Rows[0]["IGST_SGST_Tax_ID"], dtHdr.Rows[0]["CGST_Tax_ID"], 
                           dtHdr.Rows[0]["Deduction_Amount_GST"], dtHdr.Rows[0]["Apprv_VECV_Amt_with_Deduction"]);
                   
                            
                            Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                       
                    }
                    else
                    {
                        objDB.BeginTranasaction();
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"],
                            dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"],
                            dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"],
                           dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["Comments"],
                           dtHdr.Rows[0]["Total_Budget_Available"], dtHdr.Rows[0]["Budget_Utilized"], dtHdr.Rows[0]["Pending_Budget"],
                            dtHdr.Rows[0]["ExpectedNoCustomers"], dtHdr.Rows[0]["ExpectedNoofVehicles"], dtHdr.Rows[0]["ExpectedPartsBusiness"],
                           dtHdr.Rows[0]["ExpectedServiceRevenue"], dtHdr.Rows[0]["ExpectedLube"], dtHdr.Rows[0]["ActivityClaim_Confirm"],
                           dtHdr.Rows[0]["ActivityClaim_Cancel"],
                           dtHdr.Rows[0]["Apprv_VECV_Amt"], dtHdr.Rows[0]["IGST_SGST_Amt"], dtHdr.Rows[0]["CGST_Amt"],
                           dtHdr.Rows[0]["Total_Apprv_VECV_Amt"], dtHdr.Rows[0]["IGST_SGST_Tax_ID"], dtHdr.Rows[0]["CGST_Tax_ID"],
                           dtHdr.Rows[0]["Deduction_Amount_GST"], dtHdr.Rows[0]["Apprv_VECV_Amt_with_Deduction"]);
                    }
                }

                if (bSaveActivityRequestDetails(objDB, dtDtls, iHdrID) == false ) 
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }
               if (bSaveActivityMerchandizeDetails(objDB, dtDtls1, iHdrID) == false)
                    {

                        objDB.RollbackTransaction();
                        bSaveRecord = false;

                 }
                if (bSaveActivityDocumentDetails(objDB, dtDocDtls, iHdrID, userID) == false)
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }

                 else
                 {
                    objDB.CommitTransaction();
                 }

                  
                
               

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


        public bool bSaveActivityClaim(DataTable dtHdr, DataTable dtDtls, DataTable dtDtls1, DataTable dtDocDtls, ref int iHdrID, int Dept, int userID, string DOCType,string DealerCode)
        {


            string ParameterList = "";
            string sDocName = "";
            string IMAPFlag = "";
            int RefClaimID = 0;
            if (Dept == 1 && DOCType == "R")  //Spare Request
            {
                sDocName = "ARSP";
            }
            else if (Dept == 2 && DOCType == "R")  //Service Request
            {
                sDocName = "ARSE";
            }
            else if (Dept == 1 && DOCType == "C")  //Spares Claim
            {
                sDocName = "ACSP";
            }
            else if (Dept == 2 && DOCType == "C")  //Service Claim
            {
                sDocName = "ACSE";
            }

            iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
            //IMAPFlag = Func.Convert.sConvertToString(dtHdr.Rows[0]["IMAP_Claim"]);
            //RefClaimID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["RefClaimID"]);
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                if (dtHdr.Rows.Count != 0)
                {

                    if (iHdrID == 0)
                    {
                        objDB.BeginTranasaction();

                        int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                        string sFinYear = Func.sGetFinancialYear(iDealerId);
                      //  dtHdr.Rows[0]["Ticket_No"] = Func.Convert.sConvertToString(GenerateTicketNo(DealerCode, iDealerId, CallType));

                       dtHdr.Rows[0]["Activity_Req_No"]= Func.Convert.sConvertToString(GenerateActiviytNo(DealerCode, iDealerId, Dept, DOCType));

                        iHdrID = objDB.ExecuteStoredProcedure("SP_ActivityClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"],
                            dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"],
                            dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"],
                           dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["Comments"],
                           dtHdr.Rows[0]["Total_Budget_Available"], dtHdr.Rows[0]["Budget_Utilized"], dtHdr.Rows[0]["Pending_Budget"],
                            dtHdr.Rows[0]["ExpectedNoCustomers"], dtHdr.Rows[0]["ExpectedNoofVehicles"], dtHdr.Rows[0]["ExpectedPartsBusiness"],
                           dtHdr.Rows[0]["ExpectedServiceRevenue"], dtHdr.Rows[0]["ExpectedLube"], dtHdr.Rows[0]["ActivityClaim_Confirm"],
                           dtHdr.Rows[0]["ActivityClaim_Cancel"], dtHdr.Rows[0]["Activity_Request_ID"],dtHdr.Rows[0]["Apprv_VECV_Amt"], 
                           dtHdr.Rows[0]["IGST_SGST_Amt"], dtHdr.Rows[0]["CGST_Amt"], 
                           dtHdr.Rows[0]["Total_Apprv_VECV_Amt"],  dtHdr.Rows[0]["IGST_SGST_Tax_ID"], dtHdr.Rows[0]["CGST_Tax_ID"], 
                           dtHdr.Rows[0]["Deduction_Amount_GST"], dtHdr.Rows[0]["Apprv_VECV_Amt_with_Deduction"]);


                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);

                    }
                    else
                    {
                        objDB.BeginTranasaction();
                        objDB.ExecuteStoredProcedure("SP_ActivityClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"],
                            dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"],
                            dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"],
                           dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["Comments"],
                           dtHdr.Rows[0]["Total_Budget_Available"], dtHdr.Rows[0]["Budget_Utilized"], dtHdr.Rows[0]["Pending_Budget"],
                            dtHdr.Rows[0]["ExpectedNoCustomers"], dtHdr.Rows[0]["ExpectedNoofVehicles"], dtHdr.Rows[0]["ExpectedPartsBusiness"],
                           dtHdr.Rows[0]["ExpectedServiceRevenue"], dtHdr.Rows[0]["ExpectedLube"], dtHdr.Rows[0]["ActivityClaim_Confirm"],
                           dtHdr.Rows[0]["ActivityClaim_Cancel"], dtHdr.Rows[0]["Activity_Request_ID"],dtHdr.Rows[0]["Apprv_VECV_Amt"], dtHdr.Rows[0]["IGST_SGST_Amt"], dtHdr.Rows[0]["CGST_Amt"], 
                           dtHdr.Rows[0]["Total_Apprv_VECV_Amt"],  dtHdr.Rows[0]["IGST_SGST_Tax_ID"], dtHdr.Rows[0]["CGST_Tax_ID"], 
                           dtHdr.Rows[0]["Deduction_Amount_GST"], dtHdr.Rows[0]["Apprv_VECV_Amt_with_Deduction"]);
                    }
                }

                if (bSaveActivityClaimDetails(objDB, dtDtls, iHdrID) == false)
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }
                if (bSaveActivityClaimMerchandizeDetails(objDB, dtDtls1, iHdrID) == false)
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }
                if (bSaveActivityClaimDocumentDetails(objDB, dtDocDtls, iHdrID, userID) == false)
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }

                else
                {
                    objDB.CommitTransaction();
                }





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
        public bool bSaveActivityClaimDocumentDetails(clsDB objDB, DataTable dtDet, int iHdrID, int userid)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        objDB.ExecuteStoredProcedure("SP_Activity_Claim_AttchFiles_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["File_Names"], dtDet.Rows[iRowCnt]["Description"], userid);
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_Activity_Claim_AttchFiles_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        public bool bSaveActivityDocumentDetails(clsDB objDB, DataTable dtDet, int iHdrID, int userid)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        objDB.ExecuteStoredProcedure("SP_Activity_AttchFiles_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["File_Names"], dtDet.Rows[iRowCnt]["Description"], userid);
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_Activity_AttchFiles_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        private bool bSaveActivityRequestDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                //if (dtDet.Rows.Count == 0 || (dtDet.Rows.Count == 1 && Func.Convert.sConvertToString(dtDet.Rows[0]["Expense_Head"]).Trim() == ""))
                if (dtDet.Rows.Count == 0)
                    bSaveRecord = false;
                else
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {

                        if (dtDet.Rows[iRowCnt]["Status"] != "D")
                        {
                            if (dtDet.Rows[iRowCnt]["Actual_Head"] != "")
                                objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimDtls_Save", dtDet.Rows[iRowCnt]["ID"],
                                    iHdrID, dtDet.Rows[iRowCnt]["Actual_Head"], dtDet.Rows[iRowCnt]["Tentative_Amount"], dtDet.Rows[iRowCnt]["Actual_Amount"], dtDet.Rows[iRowCnt]["VECV_Shr_Per"], dtDet.Rows[iRowCnt]["VECV_Shr_Amt"], dtDet.Rows[iRowCnt]["Dealer_Shr_Per"], dtDet.Rows[iRowCnt]["Dealer_Shr_Amt"], dtDet.Rows[iRowCnt]["Apprv_VECV_Per"],
                                    dtDet.Rows[iRowCnt]["Apprv_VECV_Amt"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Per"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Amt"]);
                                  //dtDet.Rows[iRowCnt]["Expense_Head"],  
                        }
                        else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                        {
                            objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimDtls_Delete", dtDet.Rows[iRowCnt]["ID"]);
                        }
                    }
                    bSaveRecord = true;
                }
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveActivityClaimDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                //if (dtDet.Rows.Count == 0 || (dtDet.Rows.Count == 1 && Func.Convert.sConvertToString(dtDet.Rows[0]["Expense_Head"]).Trim() == ""))
                if (dtDet.Rows.Count == 0)
                    bSaveRecord = false;
                else
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {

                        if (dtDet.Rows[iRowCnt]["Status"] != "D")
                        {
                            if (dtDet.Rows[iRowCnt]["Actual_Head"] != "")
                                objDB.ExecuteStoredProcedure("SP_ActivityClaimDtls_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Actual_Head"], dtDet.Rows[iRowCnt]["Tentative_Amount"], dtDet.Rows[iRowCnt]["Actual_Amount"], dtDet.Rows[iRowCnt]["VECV_Shr_Per"], dtDet.Rows[iRowCnt]["VECV_Shr_Amt"], dtDet.Rows[iRowCnt]["Dealer_Shr_Per"], dtDet.Rows[iRowCnt]["Dealer_Shr_Amt"], dtDet.Rows[iRowCnt]["Apprv_VECV_Per"],
                                    dtDet.Rows[iRowCnt]["Apprv_VECV_Amt"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Per"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Amt"]);
                            //dtDet.Rows[iRowCnt]["Expense_Head"], 
                        }
                        else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                        {
                            objDB.ExecuteStoredProcedure("SP_ActivityClaimDtls_Delete", dtDet.Rows[iRowCnt]["ID"]);
                        }
                    }
                    bSaveRecord = true;
                }
            }
            catch
            {

            }
            return bSaveRecord;
        }
        public bool bSaveActivityClaimMerchandizeDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityClaimMerchandizeReq_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["MerchandizeReq"], dtDet.Rows[iRowCnt]["Qty"]);
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D" && Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]) != 0)
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityClaimMerchandizeReq_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveActivityMerchandizeDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestMerchandizeReq_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["MerchandizeReq"], dtDet.Rows[iRowCnt]["Qty"]);
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D" && Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]) != 0)
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestMerchandizeReq_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        public DataSet GetActivity(int iActivityID, string sSelectionType)
        {

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivity", iActivityID, sSelectionType);
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
        public DataSet GetActivityForProcessing(int UserID, string sDealerID, string sSelectionType, string sStatus, string FromDate, string ToDate, int sDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                // dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityForProcessing", sDealerID, sSelectionType, sStatus, FromDate, ToDate,Flag);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityForProcessing", UserID, sDealerID, sSelectionType, sStatus, FromDate, ToDate, sDeptID);
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

        public bool bReturnRequestforModification(int iClaimId, int iUserRoleID, string Type, string ASMRemark,string RSMRemark, string HeadRemark, string AfterSalesHeadRemark, int UserDeptID)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_Activity_ReturnRequestforModification", iClaimId, iUserRoleID, Type, ASMRemark, RSMRemark, HeadRemark, AfterSalesHeadRemark, UserDeptID);
                         
                objDB.CommitTransaction();
               




                bSubmit = true;
                return bSubmit;
            }
            catch (Exception ex)
            {

                objDB.RollbackTransaction();

                return bSubmit;
            }

            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bSubmitActivityRequestClaim(int iClaimId, int iUserRoleID, string Type, string ASMRemark ,string RSMRemark, string HeadRemark, string  AfterSalesHeadRemark, 
             double ApprVeCVShareTotAmt_GST,double IGST_SGST_GST, double CGST_GST, double ApprVeCVShareFinalTotAmt_GST,double DeductionAmount_GST,double ApprVeCVShareTotAmt_GSTWithDeduction,
            DataTable dtDtls, DataTable dtDocDtls, int Userid, int UserDeptID,int DeptID)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_Activity_SubmitActivityRequestClaim", iClaimId, iUserRoleID, Type, ASMRemark,RSMRemark, HeadRemark, AfterSalesHeadRemark,
                    ApprVeCVShareTotAmt_GST, IGST_SGST_GST, CGST_GST, ApprVeCVShareFinalTotAmt_GST, DeductionAmount_GST, ApprVeCVShareTotAmt_GSTWithDeduction, UserDeptID, DeptID);

                if (Type == "A")
                {
                    if (bSaveActivityRequestDetails(objDB, dtDtls, iClaimId) == false)
                    {

                        objDB.RollbackTransaction();
                        bSubmit = false;

                    }

                    if (bSaveActivityDocumentDetails(objDB, dtDocDtls, iClaimId, Userid) == false)
                    {

                        objDB.RollbackTransaction();
                        bSubmit = false;

                    }
                    else
                    {
                        objDB.CommitTransaction();
                    }
                }
                else if (Type == "P")
                {
                    if (bSaveActivityClaimDetails(objDB, dtDtls, iClaimId) == false)
                    {

                        objDB.RollbackTransaction();
                        bSubmit = false;

                    }

                    if (bSaveActivityClaimDocumentDetails(objDB, dtDocDtls, iClaimId, Userid) == false)
                    {

                        objDB.RollbackTransaction();
                        bSubmit = false;

                    }
                    else
                    {
                        objDB.CommitTransaction();
                    }
                }




                bSubmit = true;
                return bSubmit;
            }
            catch (Exception ex)
            {

                objDB.RollbackTransaction();

                return bSubmit;
            }

            finally
            {
                if (objDB != null) objDB = null;
            }
        }
     
        public DataSet GetActivity_FillTotalBudget( string sType,int DealerID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivity_BudgetCalculation", sType, DealerID);
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

    }
}
