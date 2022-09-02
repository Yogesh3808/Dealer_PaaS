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
    /// Summary description for Activity
    /// </summary>
    public class Activity
    {
        public Activity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetActivity(int iActivityID, string sSelectionType, string sStatus, int Dept_ID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivity", iActivityID, sSelectionType, sStatus, Dept_ID);
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

        public DataSet GetAcountGLAndCostCenter(int iTypeOfActivity)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetAcountGLAndCostCenter", iTypeOfActivity);
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

        //public DataSet GetActivity(string sDealerID, string sSelectionType, string sStatus,string FromDate,string ToDate,string Flag)
        public DataSet GetActivity(int UserID, string sDealerID, string sSelectionType, string sStatus, string FromDate, string ToDate, string sDeptID)
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




        public DataSet GetActivityClaimDetailsByActivityID(int ActivityID, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                // dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityForProcessing", sDealerID, sSelectionType, sStatus, FromDate, ToDate,Flag);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetActivityClaimDetailsByActivityID", ActivityID, iDealerID);
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



        public bool bSaveActivityMaster(ref int iActivityHdr_ID, DataTable dtDet, DataTable dtActivityDetails)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //int iActivityHdr_ID = 0;
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    iActivityHdr_ID = Func.Convert.iConvertToInt(dtDet.Rows[0]["ID"]);
                    if (iActivityHdr_ID == 0)
                        iActivityHdr_ID = objDB.ExecuteStoredProcedure("SP_ActivityMaster_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Activity_Name"], dtDet.Rows[0]["Dealer_Type"], dtDet.Rows[0]["From_Date"], dtDet.Rows[0]["To_Date"], dtDet.Rows[0]["VECV_Remark"], dtDet.Rows[0]["Dept_ID"], dtDet.Rows[0]["Type_ID"], dtDet.Rows[0]["ClaimAllow"], dtDet.Rows[0]["CredtitiorPostKey_Id"], dtDet.Rows[0]["SpecialGL_Id"], dtDet.Rows[0]["ETBPostKey_Id"], dtDet.Rows[0]["CostCenter_Id"], dtDet.Rows[0]["Account_Id"], dtDet.Rows[0]["BasicCat_Id"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_ActivityMaster_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Activity_Name"], dtDet.Rows[0]["Dealer_Type"], dtDet.Rows[0]["From_Date"], dtDet.Rows[0]["To_Date"], dtDet.Rows[0]["VECV_Remark"], dtDet.Rows[0]["Dept_ID"], dtDet.Rows[0]["Type_ID"], dtDet.Rows[0]["ClaimAllow"], dtDet.Rows[0]["CredtitiorPostKey_Id"], dtDet.Rows[0]["SpecialGL_Id"], dtDet.Rows[0]["ETBPostKey_Id"], dtDet.Rows[0]["CostCenter_Id"], dtDet.Rows[0]["Account_Id"], dtDet.Rows[0]["BasicCat_Id"]);
                    if (ActivityDetails(objDB, iActivityHdr_ID, dtActivityDetails) == true)
                    {
                        objDB.CommitTransaction();
                        bSaveRecord = true;
                    }
                    else
                    {
                        objDB.RollbackTransaction();
                    }
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

        private bool ActivityDetails(clsDB objDB, int iActivityHdr_ID, DataTable dtActivityDetails)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtActivityDetails.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_ActivityDetailsSave", iActivityHdr_ID, dtActivityDetails.Rows[iRowCnt]["Dealer_ID"], dtActivityDetails.Rows[iRowCnt]["StateIDORCountryID"], dtActivityDetails.Rows[iRowCnt]["RegionID"]);

                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
                bSaveRecord = false;
            }
            return bSaveRecord;
        }

        public void CancelActivity(int iActivityID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                string sSQlUpdate = "";
                sSQlUpdate = "Update M_ActivityMaster Set Active=0 where ID =" + iActivityID;
                objDB.ExecuteQuery(sSQlUpdate);
                objDB.CommitTransaction();
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }


        public bool bSaveActivityRequestClaim(DataTable dtHdr, DataTable dtDtls, DataTable dtDocDtls, ref int iHdrID)
        {


            string ParameterList = "";
            string sDocName = "A";
            iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
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
                        int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                        string sFinYear = Func.sGetFinancialYear(iDelearId);
                        iHdrID = objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Actl_No_Participants"], dtHdr.Rows[0]["Activity_Approved_Date"], dtHdr.Rows[0]["Activity_Approved_Status"], dtHdr.Rows[0]["Claim_Date"], dtHdr.Rows[0]["Claim_Approved_Date"], dtHdr.Rows[0]["Claim_Approved_Status"], dtHdr.Rows[0]["Detail_Of_Activity"], dtHdr.Rows[0]["Actual_Of_Activity"], dtHdr.Rows[0]["Activity_Location"], dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["ExcpOutCome"], dtHdr.Rows[0]["ActualOutCome"], dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"], dtHdr.Rows[0]["Claim_Request"], dtHdr.Rows[0]["Activity_Confirm"], dtHdr.Rows[0]["ActivityClaim_Confirm"], dtHdr.Rows[0]["ActivityClaim_Cancel"]);
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                    }
                    else
                    {
                        objDB.BeginTranasaction();
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Activity_ID"], dtHdr.Rows[0]["Activity_Req_No"], dtHdr.Rows[0]["Activity_Req_Date"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Actl_No_Participants"], dtHdr.Rows[0]["Activity_Approved_Date"], dtHdr.Rows[0]["Activity_Approved_Status"], dtHdr.Rows[0]["Claim_Date"], dtHdr.Rows[0]["Claim_Approved_Date"], dtHdr.Rows[0]["Claim_Approved_Status"], dtHdr.Rows[0]["Detail_Of_Activity"], dtHdr.Rows[0]["Actual_Of_Activity"], dtHdr.Rows[0]["Activity_Location"], dtHdr.Rows[0]["Objective"], dtHdr.Rows[0]["ExcpOutCome"], dtHdr.Rows[0]["ActualOutCome"], dtHdr.Rows[0]["Dealer_Activity_DateFrom"], dtHdr.Rows[0]["Dealer_Activity_DateTo"], dtHdr.Rows[0]["Claim_Request"], dtHdr.Rows[0]["Activity_Confirm"], dtHdr.Rows[0]["ActivityClaim_Confirm"], dtHdr.Rows[0]["ActivityClaim_Cancel"]);
                    }
                }

                if (bSaveActivityRequestDetails(objDB, dtDtls, iHdrID) == false || bSaveActivityDocumentDetails(objDB, dtDocDtls, iHdrID) == false)
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

        private bool bSaveActivityRequestDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Expense_Head"] != "")
                            objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimDtls_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Expense_Head"], dtDet.Rows[iRowCnt]["Actual_Head"], dtDet.Rows[iRowCnt]["Tentative_Amount"], dtDet.Rows[iRowCnt]["Actual_Amount"], dtDet.Rows[iRowCnt]["VECV_Shr_Per"], dtDet.Rows[iRowCnt]["VECV_Shr_Amt"], dtDet.Rows[iRowCnt]["Dealer_Shr_Per"], dtDet.Rows[iRowCnt]["Dealer_Shr_Amt"], dtDet.Rows[iRowCnt]["Apprv_VECV_Per"], dtDet.Rows[iRowCnt]["Apprv_VECV_Amt"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Per"], dtDet.Rows[iRowCnt]["Apprv_Dealer_Amt"], dtDet.Rows[iRowCnt]["Approved"], dtDet.Rows[iRowCnt]["Request_Type"]);
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimDtls_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveActivityDocumentDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_Activity_AttchFiles_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["File_Names"], dtDet.Rows[iRowCnt]["Description"]);

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveActivityProcessDetails(DataTable dtDet)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_ActivityProcessing_Save", dtDet.Rows[iRowCnt]["HdrId"], dtDet.Rows[iRowCnt]["Activity_Approved_Date"], dtDet.Rows[iRowCnt]["Activity_Approved_Status"], dtDet.Rows[iRowCnt]["DtlID"], dtDet.Rows[iRowCnt]["Apprv_VECV_Per"], dtDet.Rows[iRowCnt]["Apprv_VECV_Amt"], dtDet.Rows[iRowCnt]["Approved"]);

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

        public static int GetActivityDepartmentID(int iMenuId)
        {
            int iDeptId = 0;
            switch (iMenuId)
            {
                case 65:
                case 206:
                case 207:
                case 67: iDeptId = 1; break;//SAles
                case 78:
                case 220:
                case 221:
                case 79: iDeptId = 2; break;//Service
                case 74:
                case 214:
                case 215:
                case 75: iDeptId = 3; break;//Spares

                case 40: iDeptId = 4; break;//Warranty
                case 376:


                case 417: iDeptId = 5; break;//Export Sales

                case 37: iDeptId = 6; break;//Export Service
                //   case 418: iDeptId = 99;
            }
            return iDeptId;
        }

        public static string GetActivitylblName(int iMenuId, string lblFor)
        {
            string lblName = "";
            switch (iMenuId)
            {
                case 78:
                case 74:
                case 65:

                case 114:
                case 376:
                    if (lblFor == "Title")
                        lblName = "Activity Claim Approval";
                    else if (lblFor == "Status")
                        lblName = "Activity Approval Status";
                    else lblName = "Approval";
                    break;//Service
                case 79:
                case 75:
                case 67:
                case 378:
                case 410: lblName = "Activity Claim Processing";
                    if (lblFor == "Title")
                        lblName = "Activity Claim Processing";
                    else if (lblFor == "Status")
                        lblName = "Activity Claim Status";
                    else lblName = "Processing";
                    break;
            }
            return lblName;

        }
        public static int MaxID(int iMenuID, int DealerID, string sDeptIDs)
        {
            int iMaxID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                if (iMenuID == 206 || iMenuID == 214 || iMenuID == 220 || iMenuID == 417)
                {
                    iMaxID = Func.Convert.iConvertToInt(objDB.ExecuteQueryAndGetObject("select max(TM_ActivityRequestClaim.ID) from TM_ActivityRequestClaim inner join M_ActivityMaster on TM_ActivityRequestClaim.Activity_ID =M_ActivityMaster.ID and M_ActivityMaster.Dept_ID in (" + sDeptIDs + ") where Activity_Confirm!='' AND Dealer_ID=" + DealerID));
                }
                else
                    if (iMenuID == 207 || iMenuID == 215 || iMenuID == 221 || iMenuID == 418)
                    {

                        iMaxID = Func.Convert.iConvertToInt(objDB.ExecuteQueryAndGetObject("select max(TM_ActivityRequestClaim.ID) from TM_ActivityRequestClaim inner join M_ActivityMaster on TM_ActivityRequestClaim.Activity_ID =M_ActivityMaster.ID  and M_ActivityMaster.Dept_ID in (" + sDeptIDs + ") where Claim_Request='Y' AND Dealer_ID=" + DealerID));

                    }
                return iMaxID;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        public bool bSaveClaimApprdDtls(DataTable dtDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_ActivityClaimApprd_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Claim_Approved_Date"], dtDet.Rows[0]["Claim_Approved_Status"], dtDet.Rows[0]["Activity_Claim_Approved_By"], dtDet.Rows[0]["Activity_Claim_Remark"]);
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

        public bool bSaveActivityApprovalDtls(DataTable dtDet, DataTable dtClp)
        {


            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();
                if (dtDet != null)
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_ActivityRequestClaimDtls_Save", dtDet.Rows[iRowCnt]["ID"], 0, dtDet.Rows[iRowCnt]["Expense_Head"], dtDet.Rows[iRowCnt]["Tentative_Amount"], dtDet.Rows[iRowCnt]["VECV_Shr_Per"], dtDet.Rows[iRowCnt]["VECV_Shr_Amt"], dtDet.Rows[iRowCnt]["Dealer_Shr_Per"], dtDet.Rows[iRowCnt]["Dealer_Shr_Amt"], dtDet.Rows[iRowCnt]["Apprv_VECV_Per"], dtDet.Rows[iRowCnt]["Apprv_VECV_Amt"], dtDet.Rows[iRowCnt]["Approved"]);
                        objDB.ExecuteStoredProcedure("SP_ActivityClaimApprd_Save", dtClp.Rows[0]["ID"], dtClp.Rows[0]["Claim_Approved_Date"], dtClp.Rows[0]["Claim_Approved_Status"], dtClp.Rows[0]["ActApprEmpID"], dtClp.Rows[0]["Activity_Claim_Remark"], "Approved");

                    }

                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_ActivityClaimApprd_Save", dtClp.Rows[0]["ID"], dtClp.Rows[0]["Claim_Approved_Date"], dtClp.Rows[0]["Claim_Approved_Status"], dtClp.Rows[0]["ActApprEmpID"], dtClp.Rows[0]["Activity_Claim_Remark"], "Process");
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
