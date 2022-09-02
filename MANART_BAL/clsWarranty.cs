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
    /// Summary description for clsWarranty
    /// </summary>
    public class clsWarranty
    {
        public clsWarranty()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        int TotalRowCount = 0;
        public enum enmClaimType
        {
            enmNormal = 1,
            enmTechnicalGoodwill = 2,
            enmPDI = 3,
            enmTransit = 4,
            //enmReclaim = 5,
            enmExtended = 6,
            enmAdditional = 7,
            enmCommercialGoodwill = 8,
            enmEnroute = 9,
            enmAMC = 10,
            enmCampaign = 11,
            enmResubmit = 12,
            enmEnrouteNonTechnical = 13,
            enmSparesPartsWarranty = 14,
            enmE_TechnicalGoodwill = 22,
            enmE_CommercialGoodwill = 23,
            enmHighValueClaim = 98,
            enmGoodwillRequest = 99
        }
        #region Warranty Claim Functions
        //To Save Model Record
        public bool bSaveWarrantyClaimRecord(string sDealerCode, DataTable dtHdr, DataTable dtPartDet, DataTable dtLabourDet, DataTable dtLubricantDet, DataTable dtSubletDet, DataTable dtComplaintDet, DataTable dtInvestigationDet, DataTable dtJobDet, DataTable dtFileAttach, DataTable dtGrpTaxDetails, int iUserId, string ClaimType, ref int iHdrID, DataTable dtJobDescDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sClaimConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_Confirm"]);
                int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveWarrantyClaimHeader(objDB, sDealerCode, dtHdr, ref iHdrID, iUserId, ClaimType) == false) goto ExitFunction;

                // Save Part Details 
                if (bSaveWarrantyClaimPartDetails(objDB, dtPartDet, iHdrID) == false) goto ExitFunction;

                // Save Labour Details 
                if (bSaveWarrantyClaimLabourDetails(objDB, dtLabourDet, iHdrID) == false) goto ExitFunction;

                // Save Lubricant Details 
                if (bSaveWarrantyClaimLubricantDetails(objDB, dtLubricantDet, iHdrID) == false) goto ExitFunction;

                // Save Sublet Details 
                if (bSaveWarrantyClaimSubletDetails(objDB, dtSubletDet, iHdrID) == false) goto ExitFunction;

                // Save Complaint Details 
                if (bSaveWarrantyClaimComplaintDetails(objDB, dtComplaintDet, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save Investigations Details 
                if (bSaveWarrantyClaimInvestigationDetails(objDB, dtInvestigationDet, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save Culprit,Defect,Technical Details
                if (bSaveWarrantyCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;

                // save File attach Details
                if (bSaveWarrantyClaimFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveWarrantyGroupTaxDetails(objDB, sDealerCode, dtGrpTaxDetails, iHdrID) == false) goto ExitFunction;

                //Save Job Desc
                if (bSaveWarrantyJobDescDetails(objDB, sDealerCode, dtJobDescDet, iHdrID) == false) goto ExitFunction;

                //Add Jobcode Details
                if (sClaimConfirm == "Y") if (bAddClaimCulpritDefectDetails(objDB, iHdrID) == false) goto ExitFunction;

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

        //Add Jobcode only when jobcard get confirm.
        private bool bAddClaimCulpritDefectDetails(clsDB objDB, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_WarrantyDefectCulprit_Add", iHdrID);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //Add Jobcode only when jobcard get confirm.
        private bool bAddGCRCulpritDefectDetails(clsDB objDB, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_GCRDefectCulprit_Add", iHdrID);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        // To Save Warranty Header record
        private bool bSaveWarrantyClaimHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserId, string ClaimType)
        {
            string sDocName = "";
            if (ClaimType == "C")
                sDocName = "C";
            else if (ClaimType == "G")
                sDocName = "GR";
            else if (ClaimType == "HR")
                sDocName = "HR";

            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {                    
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    string sClaimType = Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_type_Id"]);
                    string sInvType = Func.Convert.sConvertToString(dtHdr.Rows[0]["InvType"]);
                    //sDocName = sDocName + sInvType;
                    sDocName = ((sClaimType == "14") ? "WP" : (sClaimType == "2") ? "GT" : (sClaimType == "8" || sClaimType == "16") ? "GC" : (sClaimType == "6") ? "CP" : (sClaimType == "10") ? "RM" : "WC") + sInvType;                    

                    if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ReturnedClaim"]) != "P" && Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ResubmitClaim"]) != "P")
                        dtHdr.Rows[0]["Claim_no"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDealerId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_WarrantyClaim_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Claim_no"], dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Claim_type_Id"], dtHdr.Rows[0]["Claim_Rev_No"], dtHdr.Rows[0]["Model_Id"], dtHdr.Rows[0]["Chassis_no"], dtHdr.Rows[0]["Engine_no"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Customer_name"], dtHdr.Rows[0]["Customer_Address"], dtHdr.Rows[0]["Customer_MobNo"], dtHdr.Rows[0]["Customer_Email"], dtHdr.Rows[0]["Jobcard_no"], dtHdr.Rows[0]["Jobcard_date"], dtHdr.Rows[0]["INS_date"], dtHdr.Rows[0]["Odometer"], dtHdr.Rows[0]["Hrs_reading"], dtHdr.Rows[0]["VECV_Inv_no"], dtHdr.Rows[0]["VECV_Inv_date"], dtHdr.Rows[0]["Approval_No"], dtHdr.Rows[0]["Approval_Date"], dtHdr.Rows[0]["Repair_Order_No"], dtHdr.Rows[0]["Repair_Order_Date"], dtHdr.Rows[0]["Repair_Complete_Date"], dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["Root_Type_ID"], dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["GCR_ID"], dtHdr.Rows[0]["GCR_No"], dtHdr.Rows[0]["GCR_Date"], dtHdr.Rows[0]["Share_Type"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"], dtHdr.Rows[0]["Ref_Claim_No"], dtHdr.Rows[0]["Ref_Claim_date"], iUserId, dtHdr.Rows[0]["GVW"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Sub_Dealer_Name"], dtHdr.Rows[0]["ClaimDomesticOrExport"], 0, dtHdr.Rows[0]["Claim_Confirm"], dtHdr.Rows[0]["Confirm_Date"], dtHdr.Rows[0]["Claim_Cancel"], dtHdr.Rows[0]["Is_ResubmitClaim"], dtHdr.Rows[0]["Is_ReturnedClaim"], dtHdr.Rows[0]["RejectedCnt"], dtHdr.Rows[0]["ReturnedCnt"], ClaimType, dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["InvType"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["TrNo"]);
                    if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ReturnedClaim"]) != "P" && Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ResubmitClaim"]) != "P")
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                    //objDB.ExecuteStoredProcedure("SP_Warranty_ClaimAcceptedValue_Save", iHdrID, dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"]);
                    //objDB.ExecuteStoredProcedure("SP_Process_WarrantyHeaderDeduction", iHdrID, "", "", "", "", "L", 6);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_WarrantyClaim_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Claim_no"], dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Claim_type_Id"], dtHdr.Rows[0]["Claim_Rev_No"], dtHdr.Rows[0]["Model_Id"], dtHdr.Rows[0]["Chassis_no"], dtHdr.Rows[0]["Engine_no"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Customer_name"], dtHdr.Rows[0]["Customer_Address"], dtHdr.Rows[0]["Customer_MobNo"], dtHdr.Rows[0]["Customer_Email"], dtHdr.Rows[0]["Jobcard_no"], dtHdr.Rows[0]["Jobcard_date"], dtHdr.Rows[0]["INS_date"], dtHdr.Rows[0]["Odometer"], dtHdr.Rows[0]["Hrs_reading"], dtHdr.Rows[0]["VECV_Inv_no"], dtHdr.Rows[0]["VECV_Inv_date"], dtHdr.Rows[0]["Approval_No"], dtHdr.Rows[0]["Approval_Date"], dtHdr.Rows[0]["Repair_Order_No"], dtHdr.Rows[0]["Repair_Order_Date"], dtHdr.Rows[0]["Repair_Complete_Date"], dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["Root_Type_ID"], dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["GCR_ID"], dtHdr.Rows[0]["GCR_No"], dtHdr.Rows[0]["GCR_Date"], dtHdr.Rows[0]["Share_Type"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"], dtHdr.Rows[0]["Ref_Claim_No"], dtHdr.Rows[0]["Ref_Claim_date"], iUserId, dtHdr.Rows[0]["GVW"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Sub_Dealer_Name"], dtHdr.Rows[0]["ClaimDomesticOrExport"], 0, dtHdr.Rows[0]["Claim_Confirm"], dtHdr.Rows[0]["Confirm_Date"], dtHdr.Rows[0]["Claim_Cancel"], dtHdr.Rows[0]["Is_ResubmitClaim"], dtHdr.Rows[0]["Is_ReturnedClaim"], dtHdr.Rows[0]["RejectedCnt"], dtHdr.Rows[0]["ReturnedCnt"], ClaimType, dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["InvType"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["TrNo"]);
                    //objDB.ExecuteStoredProcedure("SP_Warranty_ClaimAcceptedValue_Save", iHdrID, dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"]);
                    //if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_Confirm"]) == "Y")
                    //    objDB.ExecuteStoredProcedure("SP_Save_ChassisMasterByWarranty", iHdrID, dtHdr.Rows[0]["VECV_Inv_no"], dtHdr.Rows[0]["VECV_Inv_date"], dtHdr.Rows[0]["INS_date"], dtHdr.Rows[0]["Chassis_no"], "W");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Save Warranty Part Details
        private bool bSaveWarrantyClaimPartDetails(clsDB objDB, DataTable dtPartDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtPartDet.Rows.Count; iRowCnt++)
                {
                    if (dtPartDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtPartDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyPartDetails_Save", dtPartDet.Rows[iRowCnt]["ID"], iHdrID, dtPartDet.Rows[iRowCnt]["GCR_Det_ID"], dtPartDet.Rows[iRowCnt]["Part_No_ID"], dtPartDet.Rows[iRowCnt]["Qty"], dtPartDet.Rows[iRowCnt]["Rate"], dtPartDet.Rows[iRowCnt]["Total"], dtPartDet.Rows[iRowCnt]["Job_Code_ID"], dtPartDet.Rows[iRowCnt]["VECV_Share"], dtPartDet.Rows[iRowCnt]["Dealer_Share"], dtPartDet.Rows[iRowCnt]["Cust_Share"], dtPartDet.Rows[iRowCnt]["Accepted_Qty"], dtPartDet.Rows[iRowCnt]["Deduction_Percentage"], dtPartDet.Rows[iRowCnt]["Deducted_Amount"], dtPartDet.Rows[iRowCnt]["Accepted_Amount"], "", dtPartDet.Rows[iRowCnt]["Replaced_Part_No_ID"], dtPartDet.Rows[iRowCnt]["Failed_Make"], dtPartDet.Rows[iRowCnt]["Replaced_Make"], "", "", "", "", dtPartDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtPartDet.Rows[iRowCnt]["TaxID"], dtPartDet.Rows[iRowCnt]["BFRGST"], dtPartDet.Rows[iRowCnt]["DealerOrMTI_Flag"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyPartDetails_Save", dtPartDet.Rows[iRowCnt]["ID"], iHdrID, dtPartDet.Rows[iRowCnt]["GCR_Det_ID"], dtPartDet.Rows[iRowCnt]["Part_No_ID"], dtPartDet.Rows[iRowCnt]["Qty"], dtPartDet.Rows[iRowCnt]["Rate"], dtPartDet.Rows[iRowCnt]["Total"], dtPartDet.Rows[iRowCnt]["Job_Code_ID"], dtPartDet.Rows[iRowCnt]["VECV_Share"], dtPartDet.Rows[iRowCnt]["Dealer_Share"], dtPartDet.Rows[iRowCnt]["Cust_Share"], dtPartDet.Rows[iRowCnt]["Accepted_Qty"], dtPartDet.Rows[iRowCnt]["Deduction_Percentage"], dtPartDet.Rows[iRowCnt]["Deducted_Amount"], dtPartDet.Rows[iRowCnt]["Accepted_Amount"], "", dtPartDet.Rows[iRowCnt]["Replaced_Part_No_ID"], dtPartDet.Rows[iRowCnt]["Failed_Make"], dtPartDet.Rows[iRowCnt]["Replaced_Make"], "", "", "", "", dtPartDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtPartDet.Rows[iRowCnt]["TaxID"], dtPartDet.Rows[iRowCnt]["BFRGST"], dtPartDet.Rows[iRowCnt]["DealerOrMTI_Flag"]);
                        }

                    }
                    else if (dtPartDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyPartDetails_Del", dtPartDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Culprit, Defect And Technical Code 
        private bool bSaveWarrantyCulpritDefectDetails(clsDB objDB, DataTable dtJobDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtJobDet.Rows.Count; iRowCnt++)
                {
                    if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtJobDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyDefectCulprit_Save", dtJobDet.Rows[iRowCnt]["ID"], iHdrID, dtJobDet.Rows[iRowCnt]["Part_No_ID"], dtJobDet.Rows[iRowCnt]["Job_Code_ID"], dtJobDet.Rows[iRowCnt]["Culprit_ID"], dtJobDet.Rows[iRowCnt]["Defect_ID"], dtJobDet.Rows[iRowCnt]["Technical_ID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyDefectCulprit_Save", dtJobDet.Rows[iRowCnt]["ID"], iHdrID, dtJobDet.Rows[iRowCnt]["Part_No_ID"], dtJobDet.Rows[iRowCnt]["Job_Code_ID"], dtJobDet.Rows[iRowCnt]["Culprit_ID"], dtJobDet.Rows[iRowCnt]["Defect_ID"], dtJobDet.Rows[iRowCnt]["Technical_ID"]);
                        }
                    }
                    else if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyDefectCulprit_Del", dtJobDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Warranty Labour Details
        private bool bSaveWarrantyClaimLabourDetails(clsDB objDB, DataTable dtLabourDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtLabourDet.Rows.Count; iRowCnt++)
                {
                    if (dtLabourDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtLabourDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyLabourDetails_Save", dtLabourDet.Rows[iRowCnt]["ID"], iHdrID, dtLabourDet.Rows[iRowCnt]["GCR_Det_ID"], dtLabourDet.Rows[iRowCnt]["Labour_ID"], dtLabourDet.Rows[iRowCnt]["Labour_Desc"], dtLabourDet.Rows[iRowCnt]["ManHrs"], dtLabourDet.Rows[iRowCnt]["Rate"], dtLabourDet.Rows[iRowCnt]["Total"], dtLabourDet.Rows[iRowCnt]["Job_Code_ID"], dtLabourDet.Rows[iRowCnt]["VECV_Share"], dtLabourDet.Rows[iRowCnt]["Dealer_Share"], dtLabourDet.Rows[iRowCnt]["Cust_Share"], dtLabourDet.Rows[iRowCnt]["Accepted_ManHrs"], dtLabourDet.Rows[iRowCnt]["Deduction_Percentage"], dtLabourDet.Rows[iRowCnt]["Deducted_Amount"], dtLabourDet.Rows[iRowCnt]["Accepted_Amount"], "", dtLabourDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLabourDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyLabourDetails_Save", dtLabourDet.Rows[iRowCnt]["ID"], iHdrID, dtLabourDet.Rows[iRowCnt]["GCR_Det_ID"], dtLabourDet.Rows[iRowCnt]["Labour_ID"], dtLabourDet.Rows[iRowCnt]["Labour_Desc"], dtLabourDet.Rows[iRowCnt]["ManHrs"], dtLabourDet.Rows[iRowCnt]["Rate"], dtLabourDet.Rows[iRowCnt]["Total"], dtLabourDet.Rows[iRowCnt]["Job_Code_ID"], dtLabourDet.Rows[iRowCnt]["VECV_Share"], dtLabourDet.Rows[iRowCnt]["Dealer_Share"], dtLabourDet.Rows[iRowCnt]["Cust_Share"], dtLabourDet.Rows[iRowCnt]["Accepted_ManHrs"], dtLabourDet.Rows[iRowCnt]["Deduction_Percentage"], dtLabourDet.Rows[iRowCnt]["Deducted_Amount"], dtLabourDet.Rows[iRowCnt]["Accepted_Amount"], "", dtLabourDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLabourDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtLabourDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyLabourDetails_Del", dtLabourDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Warranty Lubricant Details
        private bool bSaveWarrantyClaimLubricantDetails(clsDB objDB, DataTable dtLubricantDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtLubricantDet.Rows.Count; iRowCnt++)
                {
                    if (dtLubricantDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtLubricantDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyLubricantDetails_Save", dtLubricantDet.Rows[iRowCnt]["ID"], iHdrID, dtLubricantDet.Rows[iRowCnt]["GCR_Det_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_Description"], dtLubricantDet.Rows[iRowCnt]["Qty"], dtLubricantDet.Rows[iRowCnt]["UOM"], dtLubricantDet.Rows[iRowCnt]["Rate"], dtLubricantDet.Rows[iRowCnt]["Total"], dtLubricantDet.Rows[iRowCnt]["Job_Code_ID"], dtLubricantDet.Rows[iRowCnt]["VECV_Share"], dtLubricantDet.Rows[iRowCnt]["Dealer_Share"], dtLubricantDet.Rows[iRowCnt]["Cust_Share"], dtLubricantDet.Rows[iRowCnt]["Accepted_Qty"], dtLubricantDet.Rows[iRowCnt]["Deduction_Percentage"], dtLubricantDet.Rows[iRowCnt]["Deducted_Amount"], dtLubricantDet.Rows[iRowCnt]["Accepted_Amount"], "", dtLubricantDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLubricantDet.Rows[iRowCnt]["TaxID"], dtLubricantDet.Rows[iRowCnt]["BFRGST"], dtLubricantDet.Rows[iRowCnt]["DealerOrMTI_Flag"]);

                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyLubricantDetails_Save", dtLubricantDet.Rows[iRowCnt]["ID"], iHdrID, dtLubricantDet.Rows[iRowCnt]["GCR_Det_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_Description"], dtLubricantDet.Rows[iRowCnt]["Qty"], dtLubricantDet.Rows[iRowCnt]["UOM"], dtLubricantDet.Rows[iRowCnt]["Rate"], dtLubricantDet.Rows[iRowCnt]["Total"], dtLubricantDet.Rows[iRowCnt]["Job_Code_ID"], dtLubricantDet.Rows[iRowCnt]["VECV_Share"], dtLubricantDet.Rows[iRowCnt]["Dealer_Share"], dtLubricantDet.Rows[iRowCnt]["Cust_Share"], dtLubricantDet.Rows[iRowCnt]["Accepted_Qty"], dtLubricantDet.Rows[iRowCnt]["Deduction_Percentage"], dtLubricantDet.Rows[iRowCnt]["Deducted_Amount"], dtLubricantDet.Rows[iRowCnt]["Accepted_Amount"], "", dtLubricantDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLubricantDet.Rows[iRowCnt]["TaxID"], dtLubricantDet.Rows[iRowCnt]["BFRGST"], dtLubricantDet.Rows[iRowCnt]["DealerOrMTI_Flag"]);
                        }
                    }
                    else if (dtLubricantDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyLubricantDetails_Del", dtLubricantDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Warranty Sublet Details
        private bool bSaveWarrantyClaimSubletDetails(clsDB objDB, DataTable dtSubletDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtSubletDet.Rows.Count; iRowCnt++)
                {
                    if (dtSubletDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtSubletDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            //iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantySubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["GCR_Det_ID"], dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Sublet_ID"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Deduction_Percentage"], dtSubletDet.Rows[iRowCnt]["Deducted_Amount"], dtSubletDet.Rows[iRowCnt]["Accepted_Amount"], "", dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"]);
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantySubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["GCR_Det_ID"], dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Total"], 0, dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Deduction_Percentage"], dtSubletDet.Rows[iRowCnt]["Deducted_Amount"], dtSubletDet.Rows[iRowCnt]["Accepted_Amount"], "", dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtSubletDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            //objDB.ExecuteStoredProcedure("SP_WarrantySubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["GCR_Det_ID"], dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Sublet_ID"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Deduction_Percentage"], dtSubletDet.Rows[iRowCnt]["Deducted_Amount"], dtSubletDet.Rows[iRowCnt]["Accepted_Amount"], "", dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"]);
                            objDB.ExecuteStoredProcedure("SP_WarrantySubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["GCR_Det_ID"], dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Total"], 0, dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Deduction_Percentage"], dtSubletDet.Rows[iRowCnt]["Deducted_Amount"], dtSubletDet.Rows[iRowCnt]["Accepted_Amount"], "", dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtSubletDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtSubletDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantySubletDetails_Del", dtSubletDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Warranty Complaint Details
        private bool bSaveWarrantyClaimComplaintDetails(clsDB objDB, DataTable dtComplaintDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtComplaintDet.Rows.Count; iRowCnt++)
                {
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtComplaintDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                    }
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyComplaint_Del", dtComplaintDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Warranty Investigations Details
        private bool bSaveWarrantyClaimInvestigationDetails(clsDB objDB, DataTable dtInvestigationDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtInvestigationDet.Rows.Count; iRowCnt++)
                {
                    if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtInvestigationDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_WarrantyInvestigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_WarrantyInvestigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                    }
                    else if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyInvestigation_Del", dtInvestigationDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Attached File Attached Details
        private bool bSaveWarrantyClaimFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveWarrantyJobDescDetails(clsDB objDB, string sDealerCode, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["InvDescID"].ToString() != "0" || dtDet.Rows[iRowCnt]["ID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyDescDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["InvDescID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        #endregion


        #region Request Save Functions

        //To Save Model Record
        public bool bSaveGCRRecord(string sDealerCode, DataTable dtHdr, DataTable dtPartDet, DataTable dtLabourDet, DataTable dtLubricantDet, DataTable dtSubletDet, DataTable dtComplaintDet, DataTable dtInvestigationDet, DataTable dtJobDet, DataTable dtFileAttach, DataTable dtGrpTaxDetails, int iUserId, string ClaimType, ref int iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sClaimConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_Confirm"]);
                int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveGCRHeader(objDB, sDealerCode, dtHdr, ref iHdrID, iUserId, ClaimType) == false) goto ExitFunction;

                // Save Complaint Details 
                if (bSaveGCRComplaintDetails(objDB, dtComplaintDet, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save Investigations Details 
                if (bSaveGCRInvestigationDetails(objDB, dtInvestigationDet, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save Part Details 
                if (bSaveGCRPartDetails(objDB, dtPartDet, iHdrID) == false) goto ExitFunction;

                // Save Labour Details 
                if (bSaveGCRLabourDetails(objDB, dtLabourDet, iHdrID) == false) goto ExitFunction;

                // Save Lubricant Details 
                if (bSaveGCRLubricantDetails(objDB, dtLubricantDet, iHdrID) == false) goto ExitFunction;

                // Save Sublet Details 
                if (bSaveGCRSubletDetails(objDB, dtSubletDet, iHdrID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGCRGroupTaxDetails(objDB, sDealerCode, dtGrpTaxDetails, iHdrID) == false) goto ExitFunction;

                // Save Culprit,Defect,Technical Details
                if (bSaveGCRCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;
                // save File attach Details
                if (bSaveGCRFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;

                //Add Jobcode Details
                if (sClaimConfirm == "Y") if (bAddGCRCulpritDefectDetails(objDB, iHdrID) == false) goto ExitFunction;

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

        // To Save GCR  record
        private bool bSaveGCRHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserId, string ClaimType)
        {
            string sDocName = "";
            int iClaimType_ID = 0;
            iClaimType_ID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Claim_type_Id"]);
            //VHP Warranty Start
            //if (iClaimType_ID == 22 || iClaimType_ID == 23)// Goodwill Claim Request
            if (iClaimType_ID == 2 || iClaimType_ID == 16)// Goodwill Claim Request                
            {
                sDocName = "GR";
            }
            else //if (iClaimType_ID == 15)// HighValue Claim Request
            {
                sDocName = "HR";
            }
            //VHP Warranty END
            //Save Header
            try
            {                
                int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                string sFinYear = Func.sGetFinancialYear(iDealerId);
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {

                    dtHdr.Rows[0]["Claim_no"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDealerId);
                    //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GCR", Func.Convert.iConvertToInt(ddlEGPDealer.SelectedValue));
                    iHdrID = objDB.ExecuteStoredProcedure("SP_GCR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Claim_no"], dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Claim_type_Id"], dtHdr.Rows[0]["Model_Id"], dtHdr.Rows[0]["Chassis_no"], dtHdr.Rows[0]["Engine_no"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Customer_name"], dtHdr.Rows[0]["Customer_Address"], dtHdr.Rows[0]["Customer_MobNo"], dtHdr.Rows[0]["Customer_Email"], dtHdr.Rows[0]["Jobcard_no"], dtHdr.Rows[0]["Jobcard_date"], dtHdr.Rows[0]["INS_date"], dtHdr.Rows[0]["Odometer"], dtHdr.Rows[0]["Hrs_reading"], dtHdr.Rows[0]["VECV_Inv_no"], dtHdr.Rows[0]["VECV_Inv_date"], dtHdr.Rows[0]["Approval_No"], dtHdr.Rows[0]["Approval_Date"], dtHdr.Rows[0]["Repair_Order_No"], dtHdr.Rows[0]["Repair_Order_Date"], dtHdr.Rows[0]["Repair_Complete_Date"], dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["Root_Type_ID"], dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["Share_Type"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"], iUserId, dtHdr.Rows[0]["GVW"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Sub_Dealer_Name"], dtHdr.Rows[0]["ClaimDomesticOrExport"], 0, dtHdr.Rows[0]["Claim_Confirm"], dtHdr.Rows[0]["Confirm_Date"], dtHdr.Rows[0]["Claim_Cancel"], ClaimType, dtHdr.Rows[0]["Is_ReSubmitReq"], dtHdr.Rows[0]["Ref_Claim_no"], dtHdr.Rows[0]["RejectedCnt"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["TrNo"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                    //objDB.ExecuteStoredProcedure("SP_GCR_RequestAcceptedValue_Save", iHdrID, dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"]);
                    //objDB.ExecuteStoredProcedure("SP_Process_GCRHeaderDeduction", iHdrID, "", "", "", "","L", 6);
                    //objDB.ExecuteStoredProcedure("SP_Save_ChassisMasterByWarranty", iHdrID, "R");
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_GCR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Claim_no"], dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Claim_type_Id"], dtHdr.Rows[0]["Model_Id"], dtHdr.Rows[0]["Chassis_no"], dtHdr.Rows[0]["Engine_no"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Customer_name"], dtHdr.Rows[0]["Customer_Address"], dtHdr.Rows[0]["Customer_MobNo"], dtHdr.Rows[0]["Customer_Email"], dtHdr.Rows[0]["Jobcard_no"], dtHdr.Rows[0]["Jobcard_date"], dtHdr.Rows[0]["INS_date"], dtHdr.Rows[0]["Odometer"], dtHdr.Rows[0]["Hrs_reading"], dtHdr.Rows[0]["VECV_Inv_no"], dtHdr.Rows[0]["VECV_Inv_date"], dtHdr.Rows[0]["Approval_No"], dtHdr.Rows[0]["Approval_Date"], dtHdr.Rows[0]["Repair_Order_No"], dtHdr.Rows[0]["Repair_Order_Date"], dtHdr.Rows[0]["Repair_Complete_Date"], dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["Root_Type_ID"], dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["Share_Type"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"], iUserId, dtHdr.Rows[0]["GVW"], dtHdr.Rows[0]["Dealer_Remark"], dtHdr.Rows[0]["Sub_Dealer_Name"], dtHdr.Rows[0]["ClaimDomesticOrExport"], 0, dtHdr.Rows[0]["Claim_Confirm"], dtHdr.Rows[0]["Confirm_Date"], dtHdr.Rows[0]["Claim_Cancel"], ClaimType, dtHdr.Rows[0]["Is_ReSubmitReq"], dtHdr.Rows[0]["Ref_Claim_no"], dtHdr.Rows[0]["RejectedCnt"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["TrNo"]);
                    //objDB.ExecuteStoredProcedure("SP_GCR_RequestAcceptedValue_Save", iHdrID, dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], dtHdr.Rows[0]["Claim_Amt"], dtHdr.Rows[0]["VECV_Share"], dtHdr.Rows[0]["Dealer_Share"], dtHdr.Rows[0]["Cust_Share"]);                    
                    //objDB.ExecuteStoredProcedure("SP_Save_ChassisMasterByWarranty", iHdrID, "R");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Save GCR Part Details
        private bool bSaveGCRPartDetails(clsDB objDB, DataTable dtPartDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtPartDet.Rows.Count; iRowCnt++)
                {
                    if (dtPartDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtPartDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_PartDetails_Save", dtPartDet.Rows[iRowCnt]["ID"], iHdrID, dtPartDet.Rows[iRowCnt]["Part_No_ID"], dtPartDet.Rows[iRowCnt]["Qty"], dtPartDet.Rows[iRowCnt]["Rate"], dtPartDet.Rows[iRowCnt]["Total"], dtPartDet.Rows[iRowCnt]["Job_Code_ID"], dtPartDet.Rows[iRowCnt]["VECV_Share"], dtPartDet.Rows[iRowCnt]["Dealer_Share"], dtPartDet.Rows[iRowCnt]["Cust_Share"], dtPartDet.Rows[iRowCnt]["Replaced_Part_No_ID"], dtPartDet.Rows[iRowCnt]["Failed_Make"], dtPartDet.Rows[iRowCnt]["Replaced_Make"], "", "", "", "", dtPartDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtPartDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_PartDetails_Save", dtPartDet.Rows[iRowCnt]["ID"], iHdrID, dtPartDet.Rows[iRowCnt]["Part_No_ID"], dtPartDet.Rows[iRowCnt]["Qty"], dtPartDet.Rows[iRowCnt]["Rate"], dtPartDet.Rows[iRowCnt]["Total"], dtPartDet.Rows[iRowCnt]["Job_Code_ID"], dtPartDet.Rows[iRowCnt]["VECV_Share"], dtPartDet.Rows[iRowCnt]["Dealer_Share"], dtPartDet.Rows[iRowCnt]["Cust_Share"], dtPartDet.Rows[iRowCnt]["Replaced_Part_No_ID"], dtPartDet.Rows[iRowCnt]["Failed_Make"], dtPartDet.Rows[iRowCnt]["Replaced_Make"], "", "", "", "", dtPartDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtPartDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtPartDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_PartDetails_Del", dtPartDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Culprit, Defect And Technical Code 
        private bool bSaveGCRCulpritDefectDetails(clsDB objDB, DataTable dtJobDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtJobDet.Rows.Count; iRowCnt++)
                {
                    if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtJobDet.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_GCR_DefectCulprit_Save", dtJobDet.Rows[iRowCnt]["ID"], iHdrID, dtJobDet.Rows[iRowCnt]["Part_No_ID"], dtJobDet.Rows[iRowCnt]["Job_Code_ID"], dtJobDet.Rows[iRowCnt]["Culprit_ID"], dtJobDet.Rows[iRowCnt]["Defect_ID"], dtJobDet.Rows[iRowCnt]["Technical_ID"]);                            
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_DefectCulprit_Save", dtJobDet.Rows[iRowCnt]["ID"], iHdrID, dtJobDet.Rows[iRowCnt]["Part_No_ID"], dtJobDet.Rows[iRowCnt]["Job_Code_ID"], dtJobDet.Rows[iRowCnt]["Culprit_ID"], dtJobDet.Rows[iRowCnt]["Defect_ID"], dtJobDet.Rows[iRowCnt]["Technical_ID"]);
                        }
                    }
                    else if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_DefectCulprit_Del", dtJobDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        // Save GCR Labour Details
        private bool bSaveGCRLabourDetails(clsDB objDB, DataTable dtLabourDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtLabourDet.Rows.Count; iRowCnt++)
                {
                    if (dtLabourDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtLabourDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_LabourDetails_Save", dtLabourDet.Rows[iRowCnt]["ID"], iHdrID, dtLabourDet.Rows[iRowCnt]["Labour_ID"], dtLabourDet.Rows[iRowCnt]["Labour_Desc"], dtLabourDet.Rows[iRowCnt]["ManHrs"], dtLabourDet.Rows[iRowCnt]["Rate"], dtLabourDet.Rows[iRowCnt]["Total"], dtLabourDet.Rows[iRowCnt]["Job_Code_ID"], dtLabourDet.Rows[iRowCnt]["VECV_Share"], dtLabourDet.Rows[iRowCnt]["Dealer_Share"], dtLabourDet.Rows[iRowCnt]["Cust_Share"], dtLabourDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLabourDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_LabourDetails_Save", dtLabourDet.Rows[iRowCnt]["ID"], iHdrID, dtLabourDet.Rows[iRowCnt]["Labour_ID"], dtLabourDet.Rows[iRowCnt]["Labour_Desc"], dtLabourDet.Rows[iRowCnt]["ManHrs"], dtLabourDet.Rows[iRowCnt]["Rate"], dtLabourDet.Rows[iRowCnt]["Total"], dtLabourDet.Rows[iRowCnt]["Job_Code_ID"], dtLabourDet.Rows[iRowCnt]["VECV_Share"], dtLabourDet.Rows[iRowCnt]["Dealer_Share"], dtLabourDet.Rows[iRowCnt]["Cust_Share"], dtLabourDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLabourDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtLabourDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_LabourDetails_Del", dtLabourDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save GCR Lubricant Details
        private bool bSaveGCRLubricantDetails(clsDB objDB, DataTable dtLubricantDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtLubricantDet.Rows.Count; iRowCnt++)
                {
                    if (dtLubricantDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtLubricantDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_LubricantDetails_Save", dtLubricantDet.Rows[iRowCnt]["ID"], iHdrID, dtLubricantDet.Rows[iRowCnt]["Lubricant_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_Description"], dtLubricantDet.Rows[iRowCnt]["Qty"], dtLubricantDet.Rows[iRowCnt]["UOM"], dtLubricantDet.Rows[iRowCnt]["Rate"], dtLubricantDet.Rows[iRowCnt]["Total"], dtLubricantDet.Rows[iRowCnt]["Job_Code_ID"], dtLubricantDet.Rows[iRowCnt]["VECV_Share"], dtLubricantDet.Rows[iRowCnt]["Dealer_Share"], dtLubricantDet.Rows[iRowCnt]["Cust_Share"], dtLubricantDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLubricantDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_LubricantDetails_Save", dtLubricantDet.Rows[iRowCnt]["ID"], iHdrID, dtLubricantDet.Rows[iRowCnt]["Lubricant_ID"], dtLubricantDet.Rows[iRowCnt]["Lubricant_Description"], dtLubricantDet.Rows[iRowCnt]["Qty"], dtLubricantDet.Rows[iRowCnt]["UOM"], dtLubricantDet.Rows[iRowCnt]["Rate"], dtLubricantDet.Rows[iRowCnt]["Total"], dtLubricantDet.Rows[iRowCnt]["Job_Code_ID"], dtLubricantDet.Rows[iRowCnt]["VECV_Share"], dtLubricantDet.Rows[iRowCnt]["Dealer_Share"], dtLubricantDet.Rows[iRowCnt]["Cust_Share"], dtLubricantDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtLubricantDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtLubricantDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCRLubricantDetails_Del", dtLubricantDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save GCR Sublet Details
        private bool bSaveGCRSubletDetails(clsDB objDB, DataTable dtSubletDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtSubletDet.Rows.Count; iRowCnt++)
                {
                    if (dtSubletDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtSubletDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            //iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_SubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Sublet_ID"], dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"]);
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_SubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], 0, dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtSubletDet.Rows[iRowCnt]["TaxID"]);
                        }
                        else
                        {
                            //objDB.ExecuteStoredProcedure("SP_GCR_SubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], dtSubletDet.Rows[iRowCnt]["Sublet_ID"], dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"]);
                            objDB.ExecuteStoredProcedure("SP_GCR_SubletDetails_Save", dtSubletDet.Rows[iRowCnt]["ID"], iHdrID, dtSubletDet.Rows[iRowCnt]["Labour_ID"], dtSubletDet.Rows[iRowCnt]["Sublet_Desc"], dtSubletDet.Rows[iRowCnt]["ManHrs"], dtSubletDet.Rows[iRowCnt]["Rate"], 0, dtSubletDet.Rows[iRowCnt]["Total"], dtSubletDet.Rows[iRowCnt]["Job_Code_ID"], dtSubletDet.Rows[iRowCnt]["VECV_Share"], dtSubletDet.Rows[iRowCnt]["Dealer_Share"], dtSubletDet.Rows[iRowCnt]["Cust_Share"], dtSubletDet.Rows[iRowCnt]["Jobcard_Det_ID"], dtSubletDet.Rows[iRowCnt]["TaxID"]);
                        }
                    }
                    else if (dtSubletDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCRSubletDetails_Del", dtSubletDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save GCR Complaint Details
        private bool bSaveGCRComplaintDetails(clsDB objDB, DataTable dtComplaintDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtComplaintDet.Rows.Count; iRowCnt++)
                {
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtComplaintDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_Complaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_Complaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                    }
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_Complaint_Del", dtComplaintDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save GCR Investigations Details
        private bool bSaveGCRInvestigationDetails(clsDB objDB, DataTable dtInvestigationDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iGCRDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtInvestigationDet.Rows.Count; iRowCnt++)
                {
                    if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iGCRDetID = Func.Convert.iConvertToInt(dtInvestigationDet.Rows[iRowCnt]["ID"]);
                        if (iGCRDetID == 0)
                        {
                            iGCRDetID = objDB.ExecuteStoredProcedure("SP_GCR_Investigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_Investigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                    }
                    else if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_Investigation_Del", dtInvestigationDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Attached File Attached Details
        private bool bSaveGCRFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        #endregion

        /// <summary>    
        /// WarrantyId else '0' 
        /// sWarrantyType 'All' all Warranty HDR +Det,'Confirm' get Confirm Warranty ,'Max' Max Record,     
        /// </summary>    
        public DataSet GetWarrantyClaim(int iWarrantyClaimID, string sWarrantyClaimType, int iDealerID, int Role_ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetWarrantyClaim", iWarrantyClaimID, sWarrantyClaimType, iDealerID, Role_ID);
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

        public DataSet GetWarrantyClaimUserWise(string sRegionID, string sContryID, string sDealerID, string sFromDate, string sToDate, string sRequestOrClaim, int iClaimStatus, int iUserRoleId, string sDomestic_Export, string sSearchText, int StartIndexRow, int MaxRowCount, string sSelRole, string sClaimTypeID, int iUserId, int ModelCategory_ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = null; ;
                
                if (sDomestic_Export == "E")
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetWarrantyClaim_ForProcessingExport", sRegionID, sContryID, sDealerID, sFromDate, sToDate, sRequestOrClaim, iClaimStatus, iUserRoleId, sDomestic_Export, sSearchText, ModelCategory_ID, iUserId, StartIndexRow, MaxRowCount, sSelRole, sClaimTypeID);
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetWarrantyClaim_ForProcessingDomestic", sRegionID, sContryID, sDealerID, sFromDate, sToDate, sRequestOrClaim, iClaimStatus, iUserRoleId, sDomestic_Export, sSearchText, ModelCategory_ID, iUserId, StartIndexRow, MaxRowCount, sSelRole, sClaimTypeID);
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    TotalRowCount = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["TotalRowCount"]);
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
        public int DataCount(string sRegionID, string sContryID, string sDealerID, string sFromDate, string sToDate, string sRequestOrClaim, int iClaimStatus, int iUserRoleId, string sDomestic_Export, string sSearchText, int StartIndexRow, int MaxRowCount)
        {
            return TotalRowCount;
        }

        public double GetDomesticFirstSlabAmount()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtDet;
                double dSlabAmt = 0;
                string strQry;
                strQry = "select top 1 ClaimSlabAmtTo from M_Sys_ClaimSlab where SlabFor='D' and  IsSelf_Certified='Y' order by ClaimSlabAmtFrom";
                dtDet = objDB.ExecuteQueryAndGetDataTable(strQry);
                if (dtDet.Rows.Count > 0)
                    dSlabAmt = Func.Convert.dConvertToDouble(dtDet.Rows[0]["ClaimSlabAmtTo"]);
                return dSlabAmt;
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

        #region Warranty Request/Claim Processing Functions
        // To Get Job Details
        public DataSet GetJobDetails(int iJobID, int iClaimId, string sRequestOrClaim, string sClaimType, int Role_ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetJobDetails", iJobID, iClaimId, sRequestOrClaim, sClaimType, Role_ID);
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

        // To Upate Job Details at processing
        public bool bProcessJobDetails(DataTable dtHdr, DataTable dtCulpritDetails, DataTable dtPartDet, DataTable dtLabourDet, DataTable dtLubricantDet, DataTable dtSubletDet, int iUserId, string sRequestOrClaim, int iUserRoleId)
        {
            int iHdrID = 0;
            string sDomestic_Export = "";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                sDomestic_Export = Func.Convert.sConvertToString(dtHdr.Rows[0]["Domestic_Export"]);
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                objDB.BeginTranasaction();
                if (bProcessClaim(objDB, dtHdr, iUserId, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                if (bProcessCulpritDefectDetails(objDB, dtCulpritDetails, iUserId, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                if (bProcessPartDetails(objDB, iHdrID, sDomestic_Export, dtPartDet, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                if (bProcessLabourDetails(objDB, iHdrID, sDomestic_Export, dtLabourDet, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                if (bProcessLubricantDetails(objDB, iHdrID, sDomestic_Export, dtLubricantDet, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                if (bProcessSubLetDetails(objDB, iHdrID, sDomestic_Export, dtSubletDet, sRequestOrClaim, iUserRoleId) == false) goto ExitFunction;
                bSaveRecord = true;
            ExitFunction:
                if (bSaveRecord == false)
                {
                    objDB.RollbackTransaction();

                }
                else
                {
                    objDB.CommitTransaction();
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        // Update Part Details of Claim while processing
        private bool bProcessClaim(clsDB objDB, DataTable dtHdr, int iUserId, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_Process_Claim", dtHdr.Rows[0]["ID"], sRequestOrClaim, dtHdr.Rows[0]["Part_Amt"], dtHdr.Rows[0]["Labor_Amt"], dtHdr.Rows[0]["Lubricant_Amt"], dtHdr.Rows[0]["Sublet_Amt"], iUserId, iUserRoleId, dtHdr.Rows[0]["PartDeduction"], dtHdr.Rows[0]["LabourDeduction"], dtHdr.Rows[0]["LubricantDeduction"], dtHdr.Rows[0]["SubletDeduction"], dtHdr.Rows[0]["Is_Global"], dtHdr.Rows[0]["Job_Code_ID"]);
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        // update Culprit, Defect And Technical Code 
        private bool bProcessCulpritDefectDetails(clsDB objDB, DataTable dtJobDet, int iUserId, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_Process_DefectCulprit", dtJobDet.Rows[0]["ID"], sRequestOrClaim, dtJobDet.Rows[0]["Claim_Hdr_ID"], dtJobDet.Rows[0]["Part_No_ID"], dtJobDet.Rows[0]["Culprit_ID"], dtJobDet.Rows[0]["Defect_ID"], dtJobDet.Rows[0]["Technical_ID"], iUserId, iUserRoleId);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Update Part Details of Claim while processing
        private bool bProcessPartDetails(clsDB objDB, int iClaimId, string sDomestic_Export, DataTable dtPartDet, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                if (dtPartDet.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtPartDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_Process_PartDetails", iClaimId, dtPartDet.Rows[iRowCnt]["ID"], sRequestOrClaim, dtPartDet.Rows[iRowCnt]["Accepted_Qty"], dtPartDet.Rows[iRowCnt]["VECV_Share"], dtPartDet.Rows[iRowCnt]["Dealer_Share"], dtPartDet.Rows[iRowCnt]["Deduction_Percentage"], dtPartDet.Rows[iRowCnt]["Accepted_Amount"], dtPartDet.Rows[iRowCnt]["Deduction_Remark"], dtPartDet.Rows[iRowCnt]["ChangeDetails_YN"], sDomestic_Export, iUserRoleId);
                    }
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Process_PartDetails", iClaimId, 0, sRequestOrClaim, 0, 0, 0, 0, 0, "", 'N', sDomestic_Export, iUserRoleId);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        // Update Labour Details of Claim while processing
        private bool bProcessLabourDetails(clsDB objDB, int iClaimId, string sDomestic_Export, DataTable dtLabourDet, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                if (dtLabourDet.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtLabourDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_Process_LabourDetails", iClaimId, dtLabourDet.Rows[iRowCnt]["ID"], sRequestOrClaim, dtLabourDet.Rows[iRowCnt]["ManHrs"], dtLabourDet.Rows[iRowCnt]["VECV_Share"], dtLabourDet.Rows[iRowCnt]["Dealer_Share"], dtLabourDet.Rows[iRowCnt]["Cust_Share"], dtLabourDet.Rows[iRowCnt]["Deduction_Percentage"], dtLabourDet.Rows[iRowCnt]["Accepted_Amount"], dtLabourDet.Rows[iRowCnt]["Deduction_Remark"], dtLabourDet.Rows[iRowCnt]["ChangeDetails_YN"], sDomestic_Export, iUserRoleId);
                    }
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Process_LabourDetails", iClaimId, 0, sRequestOrClaim, 0, 0, 0, 0, 0, 0.00, "", "N", sDomestic_Export, iUserRoleId);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        // Update Lubricant Details of Claim while processing
        private bool bProcessLubricantDetails(clsDB objDB, int iClaimId, string sDomestic_Export, DataTable dtLubricantDet, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                if (dtLubricantDet.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtLubricantDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_Process_LubricantDetails", iClaimId, dtLubricantDet.Rows[iRowCnt]["ID"], sRequestOrClaim, dtLubricantDet.Rows[iRowCnt]["Qty"], dtLubricantDet.Rows[iRowCnt]["VECV_Share"], dtLubricantDet.Rows[iRowCnt]["Dealer_Share"], dtLubricantDet.Rows[iRowCnt]["Deduction_Percentage"], dtLubricantDet.Rows[iRowCnt]["Accepted_Amount"], dtLubricantDet.Rows[iRowCnt]["Deduction_Remark"], dtLubricantDet.Rows[iRowCnt]["ChangeDetails_YN"], sDomestic_Export, iUserRoleId);
                    }
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Process_LubricantDetails", iClaimId, 0, sRequestOrClaim, 0, 0, 0, 0, 0, "", "N", sDomestic_Export, iUserRoleId);
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Update SubLet Details of Claim while processing
        private bool bProcessSubLetDetails(clsDB objDB, int iClaimId, string sDomestic_Export, DataTable dtSubLetDet, string sRequestOrClaim, int iUserRoleId)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                if (dtSubLetDet.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtSubLetDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_Process_SubLetDetails", iClaimId, dtSubLetDet.Rows[iRowCnt]["ID"], sRequestOrClaim, dtSubLetDet.Rows[iRowCnt]["VECV_Share"], dtSubLetDet.Rows[iRowCnt]["Dealer_Share"], dtSubLetDet.Rows[iRowCnt]["Deduction_Percentage"], dtSubLetDet.Rows[iRowCnt]["Accepted_Amount"], dtSubLetDet.Rows[iRowCnt]["Deduction_Remark"], dtSubLetDet.Rows[iRowCnt]["ChangeDetails_YN"], sDomestic_Export, iUserRoleId);
                    }
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Process_SubLetDetails", iClaimId, 0, sRequestOrClaim, 0, 0, 0, 0.00, "", "N", sDomestic_Export, iUserRoleId);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }


        // To Save Reamrk of the Claim/request 
        public bool bSubmitClaimForSave(string sRequestOrClaim, int iClaimId, int iUserRoleId, string sRemark, string sSaveOrSubmit, int iClaimStatus, int iUserID, int iReasonID, int iRequestTypeID, string sDomestic_Export, int iDealerID)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                if (iClaimStatus == 2)
                {
                    string sDocName = "";
                    string sFinYear = Func.sGetFinancialYear(iDealerID);
                    string sApprovalNo = "";
                    if (sRequestOrClaim == "R" || sRequestOrClaim == "HR")
                    {
                        sDocName = "RAPR";
                    }
                    else if (sRequestOrClaim == "C")
                    {
                        sDocName = "CAPR";
                    }

                    objDB.BeginTranasaction();
                    int iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                    sApprovalNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                    sApprovalNo = sApprovalNo.PadLeft(5, '0');
                    sApprovalNo = sDocName + "/" + sFinYear + "/" + sApprovalNo;

                    objDB.ExecuteStoredProcedure("SP_Submit_Claim", sRequestOrClaim, iClaimId, iUserRoleId, sRemark, sSaveOrSubmit, iClaimStatus, sApprovalNo, iUserID, iReasonID, iRequestTypeID, sDomestic_Export);

                    //Func.Common.UpdateMaxNo(sFinYear, sDocName, 0);

                    objDB.CommitTransaction();
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Submit_Claim", sRequestOrClaim, iClaimId, iUserRoleId, sRemark, sSaveOrSubmit, iClaimStatus, "", iUserID, iReasonID, iRequestTypeID, sDomestic_Export);
                }
                bSubmit = true;
                return bSubmit;
            }
            catch (Exception ex)
            {
                //if (iClaimStatus == 2)
                //{
                objDB.RollbackTransaction();
                //}
                return bSubmit;
            }

            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public enmClaimType GetEnmClaimType(string sClaimTypeID)
        {
            switch (sClaimTypeID)
            {
                case "1": return enmClaimType.enmNormal;
                case "2": return enmClaimType.enmTechnicalGoodwill;
                case "3": return enmClaimType.enmPDI;
                case "4": return enmClaimType.enmTransit;
                case "6": return enmClaimType.enmExtended;
                case "7": return enmClaimType.enmAdditional;
                case "8": return enmClaimType.enmCommercialGoodwill;
                case "9": return enmClaimType.enmEnroute;
                case "10": return enmClaimType.enmAMC;
                case "11": return enmClaimType.enmCampaign;
                case "12": return enmClaimType.enmResubmit;
                case "13": return enmClaimType.enmEnrouteNonTechnical;
                case "14": return enmClaimType.enmSparesPartsWarranty;
                case "21": return enmClaimType.enmNormal;
                case "22": return enmClaimType.enmE_TechnicalGoodwill;
                case "23": return enmClaimType.enmE_CommercialGoodwill;
                case "99": return enmClaimType.enmGoodwillRequest;
            }

            return enmClaimType.enmNormal;
        }
        #endregion

        public bool bCreateWarrantyClaimFromRequest(string sDealerCode, int iDealerID, int iRequestID)
        {
            string sDocName = "";
            sDocName = "EC";
            //Save Header           
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int NewID;
                string sFinYear = Func.sGetFinancialYear(iDealerID);
                objDB.BeginTranasaction();
                //string sClaimNo = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDealerID);
                NewID = objDB.ExecuteStoredProcedure("SP_WarrantyClaim_From_Request1", iRequestID);
                //Func.Common.UpdateMaxNo(sFinYear, sDocName, iDealerID);
                //objDB.CommitTransaction();

                ////For Auto Approver Both High Value and Goodwill 

                //-------------------- Commented By Shyamal on 26/01/2011--------------------

                // Claim generated from request also go for process like as normal claim. 
                // Auto Approve for Both High Value and Goodwill not applicable as per disscussion with Pramod sir

                //string sApprovalNo = "";                    
                //sDocName = "RAPR";   
                //int iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, 0);
                //sApprovalNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                //sApprovalNo = sApprovalNo.PadLeft(5, '0');
                //sApprovalNo = sDocName + "/" + sFinYear + "/" + sApprovalNo;

                //objDB.ExecuteStoredProcedure("SP_Submit_ClaimWithAutoApproveed", NewID, iRequestID, sApprovalNo);
                //Func.Common.UpdateMaxNo(sFinYear, sDocName, 0);

                //-------------------- Commented By Shyamal on 26/01/2011--------------------

                objDB.CommitTransaction();


                return true;
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

        public DataTable GetWarrantyServiceHistory(int iID, string SelectionType, string sClaimOrRequest)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = new DataTable();
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetWarrantyServiceHistory", iID, SelectionType, sClaimOrRequest);
                return dt;
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

        public bool bSaveWarrantyServiceHistory(DataTable dtDet, ref int iID)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {
                    iID = Func.Convert.iConvertToInt(dtDet.Rows[0]["ID"]);
                    objDB.BeginTranasaction();
                    if (iID == 0)
                        iID = objDB.ExecuteStoredProcedure("SP_WarrantyServiceHistory_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Claim_Hdr_ID"], dtDet.Rows[0]["Service_No"], dtDet.Rows[0]["Service_Type"], dtDet.Rows[0]["Repair_Order_No"], dtDet.Rows[0]["Repair_Order_Date"], dtDet.Rows[0]["Kms"], dtDet.Rows[0]["Failure_Date"], dtDet.Rows[0]["Repair_Details"], dtDet.Rows[0]["ClaimOrRequest"], dtDet.Rows[0]["Is_Confirm"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_WarrantyServiceHistory_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Claim_Hdr_ID"], dtDet.Rows[0]["Service_No"], dtDet.Rows[0]["Service_Type"], dtDet.Rows[0]["Repair_Order_No"], dtDet.Rows[0]["Repair_Order_Date"], dtDet.Rows[0]["Kms"], dtDet.Rows[0]["Failure_Date"], dtDet.Rows[0]["Repair_Details"], dtDet.Rows[0]["ClaimOrRequest"], dtDet.Rows[0]["Is_Confirm"]);
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

        public DataSet GetRequestDetailsForClaim(int iWarrantyClaimID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetRequestDetailsForClaim", iWarrantyClaimID);
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
        public static DataTable GetCulpritDefect(string sSelectType, string sCodeFirst,int CodeID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetCulpritDefect", sSelectType, sCodeFirst, CodeID);
                return dt;
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

        public int ChkChassisWarrantyByInsDate(string sChassis_No, string sInsdate, int ret)
        {
            clsDB objDB = new clsDB();
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_ChkChassisWarrantyByInsDate", sChassis_No, sInsdate, ret);
            }
            catch (Exception ex)
            {
                return ret;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return ret;
        }

        private bool bSaveWarrantyGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_WarrantyDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                            dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"], dtGrTaxDet.Rows[iRowCnt]["Total"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        public bool bSaveWarrantyClaimAccTaxRecord(DataTable dtAccGrTaxDet, int iHdrID)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {   
                objDB.BeginTranasaction();

                if (bSaveWarrantyGroupAccTaxDetails(dtAccGrTaxDet, iHdrID) == false) goto ExitFunction;

                if (bSaveWarrantyAccDetUpdate(iHdrID) == false) goto ExitFunction;

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

        private bool bSaveWarrantyGroupAccTaxDetails(DataTable dtAccGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtAccGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtAccGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_WarrantyDetAccTax_Save", dtAccGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtAccGrTaxDet.Rows[iRowCnt]["group_code"],
                            dtAccGrTaxDet.Rows[iRowCnt]["Acc_net_inv_amt"], dtAccGrTaxDet.Rows[iRowCnt]["discount_per"], dtAccGrTaxDet.Rows[iRowCnt]["discount_amt"],
                            dtAccGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtAccGrTaxDet.Rows[iRowCnt]["Acc_tax_amt"],
                            dtAccGrTaxDet.Rows[iRowCnt]["tax1_code"], dtAccGrTaxDet.Rows[iRowCnt]["Acc_tax1_amt"],
                            dtAccGrTaxDet.Rows[iRowCnt]["tax2_code"], dtAccGrTaxDet.Rows[iRowCnt]["Acc_tax2_amt"], dtAccGrTaxDet.Rows[iRowCnt]["Acc_total"]);
                    }
                }                
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        private bool bSaveWarrantyAccDetUpdate(int iHdrID)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.ExecuteStoredProcedure("SP_WarrantyAccDetUpdate", iHdrID);                
                
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        private bool bSaveGCRGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_GCRDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_GCRDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                            dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"], dtGrTaxDet.Rows[iRowCnt]["Total"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        public bool bSaveJobcodeDetails(int iHdrID, DataTable dtJobDet, string sFConfirm)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Culprit,Defect,Technical Details
                //if (dtJobDet != null) if (bSaveJobcardCulpritDefectDetails(objDB, dtJobDet, iHdrID, sFConfirm) == false) goto ExitFunction;                
                if (dtJobDet != null) if (bSaveWarrantyCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;
                if (sFConfirm == "Y") if (bConfirmExportClaim(objDB, iHdrID) == false) goto ExitFunction;

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

        public bool bSaveGCRJobcodeDetails(int iHdrID, DataTable dtJobDet, string sFConfirm)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Culprit,Defect,Technical Details
                //if (dtJobDet != null) if (bSaveJobcardCulpritDefectDetails(objDB, dtJobDet, iHdrID, sFConfirm) == false) goto ExitFunction;                
                //if (dtJobDet != null) if (bSaveWarrantyCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;
                if (dtJobDet != null) if (bSaveGCRCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;
                if (sFConfirm == "Y") if (bConfirmExportGCR(objDB, iHdrID) == false) goto ExitFunction;
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
        
        private bool bConfirmExportGCR(clsDB objDB, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_GCR_ExportConfirm", iHdrID);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bConfirmExportClaim(clsDB objDB, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_Claim_ExportConfirm", iHdrID);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveWarrantyServiceVAN(ref int iHdrID, DataTable dtJbSrvVAN, string sDealerCode, int iUserID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Culprit,Defect,Technical Details
                if (dtJbSrvVAN != null) if (bSaveWarrServVANDtls(objDB, sDealerCode, dtJbSrvVAN, ref iHdrID, iUserID) == false) goto ExitFunction;

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

        private bool bSaveWarrServVANDtls(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["SrvVANHDRID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_WarrantyServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
                    dtHdr.Rows[0]["Complaint_Tm"], dtHdr.Rows[0]["Mech_VAN_Out_Tm"], dtHdr.Rows[0]["Serv_Rate"], dtHdr.Rows[0]["Tot_Amt"], dtHdr.Rows[0]["LabType"], dtHdr.Rows[0]["SrvMechID"], dtHdr.Rows[0]["StKms"], dtHdr.Rows[0]["EndKms"]);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_WarrantyServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
                    dtHdr.Rows[0]["Complaint_Tm"], dtHdr.Rows[0]["Mech_VAN_Out_Tm"], dtHdr.Rows[0]["Serv_Rate"], dtHdr.Rows[0]["Tot_Amt"], dtHdr.Rows[0]["LabType"], dtHdr.Rows[0]["SrvMechID"], dtHdr.Rows[0]["StKms"], dtHdr.Rows[0]["EndKms"]);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["SrvVANHDRID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool bSaveGCRServiceVAN(ref int iHdrID, DataTable dtJbSrvVAN, string sDealerCode, int iUserID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Culprit,Defect,Technical Details
                if (dtJbSrvVAN != null) if (bSaveGCRServVANDtls(objDB, sDealerCode, dtJbSrvVAN, ref iHdrID, iUserID) == false) goto ExitFunction;

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

        private bool bSaveGCRServVANDtls(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["SrvVANHDRID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_GCRServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
                    dtHdr.Rows[0]["Complaint_Tm"], dtHdr.Rows[0]["Mech_VAN_Out_Tm"], dtHdr.Rows[0]["Serv_Rate"], dtHdr.Rows[0]["Tot_Amt"], dtHdr.Rows[0]["LabType"], dtHdr.Rows[0]["SrvMechID"], dtHdr.Rows[0]["StKms"], dtHdr.Rows[0]["EndKms"]);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_GCRServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
                    dtHdr.Rows[0]["Complaint_Tm"], dtHdr.Rows[0]["Mech_VAN_Out_Tm"], dtHdr.Rows[0]["Serv_Rate"], dtHdr.Rows[0]["Tot_Amt"], dtHdr.Rows[0]["LabType"], dtHdr.Rows[0]["SrvMechID"], dtHdr.Rows[0]["StKms"], dtHdr.Rows[0]["EndKms"]);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["SrvVANHDRID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
