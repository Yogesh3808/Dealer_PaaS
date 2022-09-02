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
    /// Summary description for clsPartClaim
    /// </summary>
    public class clsPartClaim
    {
        String sClaimNo = "";
        public clsPartClaim()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtTaxDetails, ref int iHdrID)
        {
            string sDocName = "";
            int ClaimType = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Claim_Type_ID"]);
            if (ClaimType == 1) sDocName = "PDS";
            else if (ClaimType == 2) sDocName = "PDE";
            else if (ClaimType == 3) sDocName = "PDW";
            else if (ClaimType == 4) sDocName = "PDM";
            else if (ClaimType == 5) sDocName = "PDD";

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    dtHdr.Rows[0]["Claim_No"] = Func.Convert.sConvertToString(GenerateClaimNO(sDealerCode, iDealerId, ClaimType));
                    //dtHdr.Rows[0]["Claim_No"] = Func.Common.sGetMaxDocNo(sDealerCode, "", sDocName, iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_PartClaimCreationHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Claim_Type_ID"], dtHdr.Rows[0]["Claim_No"],
                        dtHdr.Rows[0]["Claim_Date"], dtHdr.Rows[0]["GRN_No"], dtHdr.Rows[0]["GRN_Date"], dtHdr.Rows[0]["Invoice_No"], dtHdr.Rows[0]["Invoice_Date"],
                        dtHdr.Rows[0]["LR_No"], dtHdr.Rows[0]["LR_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["Approval_Status"], dtHdr.Rows[0]["Is_Uploaded"], dtHdr.Rows[0]["Claim_TotalQty"], dtHdr.Rows[0]["Claim_TotalItems"],//dtHdr.Rows[0]["Claim_Amt"], 
                        dtHdr.Rows[0]["Claim_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["InsuCmpyName"], dtHdr.Rows[0]["InsuCoverNoteNo"], dtHdr.Rows[0]["InsuValidityDate"],
                        dtTaxDetails.Rows[0]["net_tr_amt"], dtTaxDetails.Rows[0]["discount_amt"], dtTaxDetails.Rows[0]["before_tax_amt"], dtTaxDetails.Rows[0]["mst_amt"],
                        dtTaxDetails.Rows[0]["cst_amt"], dtTaxDetails.Rows[0]["surcharge_amt"], dtTaxDetails.Rows[0]["tot_amt"], dtTaxDetails.Rows[0]["pf_per"],
                        dtTaxDetails.Rows[0]["pf_amt"], dtTaxDetails.Rows[0]["other_per"], dtTaxDetails.Rows[0]["other_money"], dtTaxDetails.Rows[0]["Claim_Amt"],
                        dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PartClaimCreationHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Claim_Type_ID"], dtHdr.Rows[0]["Claim_No"],
                        dtHdr.Rows[0]["Claim_Date"], dtHdr.Rows[0]["GRN_No"], dtHdr.Rows[0]["GRN_Date"], dtHdr.Rows[0]["Invoice_No"], dtHdr.Rows[0]["Invoice_Date"],
                        dtHdr.Rows[0]["LR_No"], dtHdr.Rows[0]["LR_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["Approval_Status"], dtHdr.Rows[0]["Is_Uploaded"], dtHdr.Rows[0]["Claim_TotalQty"], dtHdr.Rows[0]["Claim_TotalItems"],//dtHdr.Rows[0]["Claim_Amt"], 
                        dtHdr.Rows[0]["Claim_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["InsuCmpyName"], dtHdr.Rows[0]["InsuCoverNoteNo"], dtHdr.Rows[0]["InsuValidityDate"],
                        dtTaxDetails.Rows[0]["net_tr_amt"], dtTaxDetails.Rows[0]["discount_amt"], dtTaxDetails.Rows[0]["before_tax_amt"], dtTaxDetails.Rows[0]["mst_amt"],
                        dtTaxDetails.Rows[0]["cst_amt"], dtTaxDetails.Rows[0]["surcharge_amt"], dtTaxDetails.Rows[0]["tot_amt"], dtTaxDetails.Rows[0]["pf_per"],
                        dtTaxDetails.Rows[0]["pf_amt"], dtTaxDetails.Rows[0]["other_per"], dtTaxDetails.Rows[0]["other_money"], dtTaxDetails.Rows[0]["Claim_Amt"],
                        dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool bSavePartDetails(clsDB objDB, DataTable dtHdr, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_PartClaimCreationDts_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["MR_Dts_ID"],
                                dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Bill_Qty"], dtDet.Rows[iRowCnt]["Recv_Qty"], dtDet.Rows[iRowCnt]["Descripancy_Qty"],
                                dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Retain_YN"], dtDet.Rows[iRowCnt]["Wrg_Part_ID"],
                                dtDet.Rows[iRowCnt]["PartTaxID"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_PartClaimCreationDts_Delete", dtDet.Rows[iRowCnt]["ID"]);
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
        private bool bSaveFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iFileAttachID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iFileAttachID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iFileAttachID == 0)
                        {
                            iFileAttachID = objDB.ExecuteStoredProcedure("SP_PartClaim_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["Path"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_PartClaim_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["Path"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_PartClaim_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveInsuranceDocument(clsDB objDB, DataTable dtInsuDoc, int iHdrID, int iUserID)
        {
            bool bSaveRecord = false;
            int iInsuDocID = 0;
            try
            {
                objDB.ExecuteStoredProcedure("SP_PC_InsuranceDoc", iHdrID);
                for (int iRowCnt = 0; iRowCnt < dtInsuDoc.Rows.Count; iRowCnt++)
                {
                    iInsuDocID = Func.Convert.iConvertToInt(dtInsuDoc.Rows[0]["ID"]);
                    if (iInsuDocID == 0)
                    {
                        iInsuDocID = objDB.ExecuteStoredProcedure("SP_PC_InsuranceDoc_Save", dtInsuDoc.Rows[iRowCnt]["ID"], iHdrID, dtInsuDoc.Rows[iRowCnt]["InsuranceDocID"], dtInsuDoc.Rows[iRowCnt]["IsSelected"], iUserID);
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_PC_InsuranceDoc_Save", dtInsuDoc.Rows[iRowCnt]["ID"], iHdrID, dtInsuDoc.Rows[iRowCnt]["InsuranceDocID"], dtInsuDoc.Rows[iRowCnt]["IsSelected"], iUserID);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;

        }
        private bool bSaveGroupTaxDetails(clsDB objDB, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_PartClaimDetTax_Save", 0, iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                              dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Total"], 0.00, dtGrTaxDet.Rows[iRowCnt]["TAX_Percentage"], dtGrTaxDet.Rows[iRowCnt]["Tax1_Per"],
                              dtGrTaxDet.Rows[iRowCnt]["Tax2_Per"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        public bool bSaveRecordWithPart(string sDealerCode, int iDealerID, DataTable dtHdr, DataTable dtDet, ref int iHdrID, int iUserID, DataTable dtFileAttach,
            DataTable dtInsuDoc, DataTable dtGrTaxDet, DataTable dtTaxDetails)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, dtTaxDetails, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, dtHdr, dtDet, iHdrID) == false) goto ExitFunction;
                //Add Attchment Details                
                if (dtFileAttach != null) if (bSaveFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;

                if (dtHdr.Rows[0]["Claim_Type_ID"].ToString() == "5")
                //&& Func.Convert.dConvertToDouble(dtHdr.Rows[0]["Claim_Amt"]) > 5000.00)
                {
                    if (dtInsuDoc != null) if (bSaveInsuranceDocument(objDB, dtInsuDoc, iHdrID, iUserID) == false) goto ExitFunction;
                }
                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, dtGrTaxDet, iHdrID) == false) goto ExitFunction;

                if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"].ToString()) == "Y")
                {
                    //objDB.ExecuteStoredProcedureAndGetObject("SP_POSendTODMS", iHdrID, iDealerID, dtHdr.Rows[0]["PO_No"], iUserID);
                }
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
        }

        public DataSet GetPartClaim(string sDlrID, string sSelectionType, string sStatus, int iClaimTypeID, int Role_ID, string FromDate, string ToDate)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartClaim", sDlrID, sSelectionType, sStatus, iClaimTypeID, Role_ID, FromDate, ToDate);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartClaimForProcessing", sDlrID, sSelectionType, sStatus, iClaimTypeID, Role_ID, FromDate, ToDate);

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

        //public bool bSavePartClaim(DataTable dtHdr, DataTable dtDet)
        public bool bSavePartClaim(DataTable dtHdr, DataTable dtDet, DataTable dtPartClaimErrorCode, string cHdrStatus, string ReqStatus, int Role_ID,
            DataTable dtGrTaxDet, DataTable TaxDetails)
        {
            int iHdrID = 0;
            int iDealerID = 0;
            string ClaimType = "";
            string DealerCode = "";
            string Fin_No = "";
            bool bSaveRecord = false;
            string sApproveDocName = "";

            ClaimType = Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_Type"]);
            DealerCode = Func.Convert.sConvertToString(dtHdr.Rows[0]["Dealer_Code"]);
            Fin_No = Func.Convert.sConvertToString(dtHdr.Rows[0]["Fin_no"]);
            iDealerID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                if (dtHdr.Rows.Count != 0)
                {
                    dtHdr.Rows[0]["Approval_No"] = ReqStatus.Trim() == "N" ? "" : Func.Convert.sConvertToString(GenerateApprovalNo(DealerCode, iDealerID, ClaimType, ""));
                    objDB.ExecuteStoredProcedure("SP_PartClaim_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Approved_Date"], dtHdr.Rows[0]["Approved_By"],
                        dtHdr.Rows[0]["Remarks"], dtHdr.Rows[0]["Head_Remark"], dtHdr.Rows[0]["RSM_Remark"], dtHdr.Rows[0]["CSM_Remark"], dtHdr.Rows[0]["ASM_Remark"],
                        cHdrStatus.Trim(), dtHdr.Rows[0]["Approval_No"], dtHdr.Rows[0]["Is_PartClaimMgr"], dtHdr.Rows[0]["Is_3PL"],
                        dtHdr.Rows[0]["Is_ProcessContinue"], dtHdr.Rows[0]["Is_Show3PLRemark"],
                        TaxDetails.Rows[0]["Acc_net_tr_amt"], TaxDetails.Rows[0]["Acc_mst_amt"], TaxDetails.Rows[0]["Acc_cst_amt"],
                        TaxDetails.Rows[0]["Acc_surcharge_amt"], TaxDetails.Rows[0]["Acc_Claim_Amt"]
                        );
                }
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID > 0 && ReqStatus.Trim() != "N")
                {
                    if (ClaimType == "Shortage")
                        sApproveDocName = "ASM";
                    else if (ClaimType == "Excess")
                        sApproveDocName = "AEX";
                    else if (ClaimType == "Wrong Supply")
                        sApproveDocName = "AWP";
                    else if (ClaimType == "Manufacturing Defect")
                        sApproveDocName = "ASM";
                    else if (ClaimType == "Damage")
                        sApproveDocName = "ADG";
                    Func.Common.UpdateMaxNo(objDB, Fin_No, sApproveDocName, iDealerID);
                }

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_PartClaim_Details", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Approved_qty"], dtDet.Rows[iRowCnt]["Rejection_Res"],
                        dtDet.Rows[iRowCnt]["Approved_Status"], dtDet.Rows[iRowCnt]["Shortage_Claim_Type_Id"], dtDet.Rows[iRowCnt]["ShortageClaimTypeCredit_Id"],
                        ReqStatus, 11, dtDet.Rows[iRowCnt]["AccTotal"]);
                    //Role_ID not make any difference thats why it specified hard code 11
                }

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_PartClaimProcDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                            dtGrTaxDet.Rows[iRowCnt]["Acc_net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["Acc_tax_amt"], dtGrTaxDet.Rows[iRowCnt]["Acc_tax1_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["Acc_tax2_amt"], dtGrTaxDet.Rows[iRowCnt]["Acc_Total"]);

                    //Role_ID not make any difference thats why it specified hard code 11
                }

                //for (int iRowCnt = 0; iRowCnt < dtPartClaimErrorCode.Rows.Count; iRowCnt++)
                //{
                //    objDB.ExecuteStoredProcedure("SP_PartClaimErrorCode_Save", dtHdr.Rows[0]["ID"], dtPartClaimErrorCode.Rows[iRowCnt]["PartClaimErrHdr_ID"]);
                //}
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
        public bool bSavePartclaimT03PL(string @sIDs)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_PartclaimT03PL_Save", @sIDs);
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
        public static string sGetMaxDocNo(string sDocName, int iDealerID)
        {
            string sFinYear = "";
            string sINSNo = "";
            int iMaxDocNo = 0;
            string sMaxINSNo = "";
            if (sFinYear == "")
            {
                sFinYear = Func.sGetFinancialYear(iDealerID);
            }
            iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, 0);
            sMaxINSNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
            sMaxINSNo = sMaxINSNo.PadLeft(4, '0');
            sINSNo = sFinYear + sDocName + sMaxINSNo;
            return sINSNo;
        }
        public DataSet GetGRNNoByPartClaimType(int iID, int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_FillGRNNoByPartClaimType", iID, iDealerID);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public string GenerateClaimNO(string sDealerCode, int iDealerID, int ClaimType)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "";
                if (ClaimType == 1) sDocName = "PDS";
                //sDocName = "SH";
                else if (ClaimType == 2) sDocName = "PDE";
                //sDocName = "EX";
                else if (ClaimType == 3) sDocName = "PDW";
                //sDocName = "WP";
                else if (ClaimType == 4) sDocName = "PDM";
                //sDocName = "MD";
                else if (ClaimType == 5) sDocName = "PDD";
                //sDocName = "DG";

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYearChar, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);

                if (sFinYear == "2016" && sDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (sDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                return sDocNo;
            }
            catch
            {
                return "0";
            }
        }
        // Get Descripancy Data from Material Receipt
        public DataSet GetDescripancy(string GRN_No, int ClaimTypeID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartClaimDetailsByClaimType", GRN_No, ClaimTypeID);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet Get_PartClaim(int PCId, string SelectType, int DealerID, int ClaimTypeID, string GRNNo)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_PartClaim_Get", PCId, SelectType, DealerID, ClaimTypeID, GRNNo);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public string GenerateApprovalNo(string sDealerCode, int iDealerID, string ClaimType, string Is_Distributor)
        {
            //DataSet dsDealer = new DataSet();
            clsDB objDB = new clsDB();
            try
            {
                //if (iDealerID == 0)
                //{
                //    dsDealer = objDB.ExecuteQueryAndGetDataset("Select  Id from M_Dealer where Dealer_Spares_Code= '" + sDealerCode + "'");
                //    if (dsDealer.Tables[0].Rows.Count > 0)
                //    {
                //        iDealerID = Func.Convert.iConvertToInt(dsDealer.Tables[0].Rows[0]["Id"]);
                //    }
                //}

                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "";
                if (ClaimType == "Shortage")
                    sDocName = "ASM";
                else if (ClaimType == "Excess")
                    sDocName = "AEX";
                else if (ClaimType == "Wrong Supply")
                    sDocName = "AWP";
                else if (ClaimType == "Manufacturing Defect")
                    sDocName = "ASM";
                else if (ClaimType == "Damage")
                    sDocName = "ADG";

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                if (sFinYear == "2016" && sDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (sDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                    //sDocNo = sDealerCode + "/" + sDocName + "/" + sFinYear + "/" + sMaxDocNo;
                }
                else
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;

                return sDocNo;
            }
            catch
            {
                return "0";
            }
            finally
            {
                if (objDB != null) objDB = null;
                //if (dsDealer != null) dsDealer = null;
            }
        }
    }
}
