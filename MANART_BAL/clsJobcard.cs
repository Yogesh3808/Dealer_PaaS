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
    /// <summary>
    /// Summary description for clsJobcard
    /// </summary>
    public class clsJobcard
    {
        public clsJobcard()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetJobcard(int iDocID, string sDocType, int iDealerID, int iHOBranchDealerId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Jobcard", iDocID, sDocType, iDealerID, iHOBranchDealerId);
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
        //public bool bSaveJobcard(ref int iHdrID, string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtLabour, DataTable dtComplaintDet, DataTable dtInvestigations, DataTable dtActionTaken, DataTable dtFreeService,DataTable dtJbGrpTaxDetails, DataTable dtJbTaxDetails, DataTable dtJbLbrTiming , int iDealerId, int iDlrBranchID, int iUserID, string sAddCStatus)//DataTable dtJobDet,
        public bool bSaveJobcard(ref int iHdrID, string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtLabour, DataTable dtComplaintDet, DataTable dtInvestigations, DataTable dtActionTaken, DataTable dtFreeService, DataTable dtJbGrpTaxDetails, DataTable dtJbTaxDetails, DataTable dtFileAttach, int iDealerId, int iDlrBranchID, int iUserID, string sAddCStatus)//DataTable dtJobDet,
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sJobConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["job_confirm"]);
                int iJobType = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Job_Ty_ID"]);
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, dtJbTaxDetails, ref iHdrID, iDlrBranchID, iUserID) == false) goto ExitFunction;

                //save Part Details Details
                if (bSavePartDetails(objDB, dtDet, iHdrID, iUserID, iDealerId, sDealerCode) == false) goto ExitFunction;

                // Save Complaint Details 
                if (dtComplaintDet != null) if (bSaveJobcardClaimComplaintDetails(objDB, dtComplaintDet, iHdrID, iDealerId, iDlrBranchID) == false) goto ExitFunction;

                //save Labour Details Details
                if (bSaveLabourDetails(objDB, dtLabour, iHdrID, iUserID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, sDealerCode, dtJbGrpTaxDetails, iHdrID) == false) goto ExitFunction;

                // Save Culprit,Defect,Technical Details                                
                //if (dtJobDet != null) if (bSaveJobcardCulpritDefectDetails(objDB, dtJobDet, iHdrID) == false) goto ExitFunction;

                // Save Investigations Details 
                if (dtInvestigations != null) if (bSaveJobcardClaimInvestigationDetails(objDB, dtInvestigations, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save Action Details 
                if (dtActionTaken != null) if (bSaveJobcardClaimActionTakenDetails(objDB, dtActionTaken, iHdrID, iDealerId) == false) goto ExitFunction;

                // Save free Coupon details  
                if (dtFreeService != null) if (bSaveJobcardCouponDetails(objDB, dtFreeService, iHdrID, iJobType) == false) goto ExitFunction;

                //Add Jobcode Details
                if (sJobConfirm == "Y") if (bAddJobcardCulpritDefectDetails(objDB, iHdrID) == false) goto ExitFunction;

                //if (sAddCStatus == "Y") if (bSaveChassisStatus(objDB, dtHdr, iHdrID) == false) goto ExitFunction;
                if (bSaveChassisStatus(objDB, dtHdr, iHdrID) == false) goto ExitFunction;

                if (bSaveJobcardInvCheck(objDB, iDealerId, iHdrID) == false) goto ExitFunction;

                if (sJobConfirm == "Y") if (bSaveJbServiceReminderDtls(objDB, iHdrID) == false) goto ExitFunction;
                

                //save Labour Timing Details Details
                //if (bSaveLabourTimingDetails(objDB, dtJbLbrTiming, iHdrID, iUserID) == false) goto ExitFunction;

                // save File attach Details
                if (bSaveJobcardFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;

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
                if (dtJobDet != null) if (bSaveJobcardCulpritDefectDetails(objDB, dtJobDet, iHdrID, sFConfirm) == false) goto ExitFunction;

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

        // Save Free Service Coupon Details 
        private bool bSaveJobcardCouponDetails(clsDB objDB, DataTable dtFreeService, int iHdrID, int iJobtype)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFreeService.Rows.Count; iRowCnt++)
                {

                    if (iJobtype == 7 && dtFreeService.Rows[iRowCnt]["Serv_Name"].ToString() == "PDI")
                    {
                        dtFreeService.Rows[iRowCnt]["JobID"] = iHdrID;
                    }

                    if (dtFreeService.Rows[iRowCnt]["JobID"].ToString() == "" || Func.Convert.iConvertToInt(dtFreeService.Rows[iRowCnt]["JobID"].ToString()) == 0 || Func.Convert.iConvertToInt(dtFreeService.Rows[iRowCnt]["JobID"].ToString()) == iHdrID)
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardCouponDetails_Save", dtFreeService.Rows[iRowCnt]["ID"], dtFreeService.Rows[iRowCnt]["JobID"]);
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
        private bool bSaveJobcardFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_Jobcard_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_Jobcard_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_Jobcard_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtJbTaxDetails, ref int iHdrID, int iDlrBranchID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["job_no"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "R", iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_JobcardHeader_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["job_no"],
                        dtHdr.Rows[0]["JobDate"], dtHdr.Rows[0]["Job_Ty_ID"], dtHdr.Rows[0]["kmsTot"], dtHdr.Rows[0]["hrsTot"],
                        dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Dealer_ID"],
                        dtHdr.Rows[0]["BayID"], dtHdr.Rows[0]["bay_allot_date"], dtHdr.Rows[0]["VehIn"],
                         dtHdr.Rows[0]["JobOpTime"], dtHdr.Rows[0]["VehCommitTime"], dtHdr.Rows[0]["VehOut"],
                          dtHdr.Rows[0]["EstmtID"], dtHdr.Rows[0]["VehIn_HDR_Id"], dtHdr.Rows[0]["ApPartAmt"],
                           dtHdr.Rows[0]["ApLabAmt"], dtHdr.Rows[0]["ApLubAmt"], dtHdr.Rows[0]["ApMiscAmt"],
                           iDlrBranchID, dtHdr.Rows[0]["job_confirm"], dtHdr.Rows[0]["job_canc_tag"],
                           dtHdr.Rows[0]["JbCd_Confirm"], iUserID, dtHdr.Rows[0]["SupervisiorID"],
                           dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["EstmtdTm"], dtHdr.Rows[0]["delay_Rsn_Id"],
                           dtHdr.Rows[0]["Aggregate"], dtHdr.Rows[0]["WarrantyTag"], dtHdr.Rows[0]["chassis_sale_date"],
                           dtHdr.Rows[0]["KmsJobcard"], dtHdr.Rows[0]["HrsJobcard"], dtHdr.Rows[0]["OdoMeterChange"],
                           dtHdr.Rows[0]["HrsMeterChange"], dtHdr.Rows[0]["Bfr_Last_SpdMtrChange_KmsJb"], dtHdr.Rows[0]["Bfr_Last_HrsMtrChange_HrsJb"]
                           , dtJbTaxDetails.Rows[0]["net_tr_amt"], dtJbTaxDetails.Rows[0]["discount_amt"], dtJbTaxDetails.Rows[0]["before_tax_amt"], dtJbTaxDetails.Rows[0]["mst_amt"], dtJbTaxDetails.Rows[0]["cst_amt"], dtJbTaxDetails.Rows[0]["surcharge_amt"], 
                           dtJbTaxDetails.Rows[0]["tot_amt"], dtJbTaxDetails.Rows[0]["pf_per"], dtJbTaxDetails.Rows[0]["pf_amt"], dtJbTaxDetails.Rows[0]["other_per"], dtJbTaxDetails.Rows[0]["other_money"], dtJbTaxDetails.Rows[0]["Jobcard_tot"],
                           dtHdr.Rows[0]["CRM_HDR_ID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Model_ID"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["InsCustID"]);

                    dtHdr.Rows[0]["ID"] = iHdrID;

                    Func.Common.UpdateMaxNo(objDB, sFinYear, "R", iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_JobcardHeader_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["job_no"],
                        dtHdr.Rows[0]["JobDate"], dtHdr.Rows[0]["Job_Ty_ID"], dtHdr.Rows[0]["kmsTot"], dtHdr.Rows[0]["hrsTot"],
                        dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Dealer_ID"],
                        dtHdr.Rows[0]["BayID"], dtHdr.Rows[0]["bay_allot_date"], dtHdr.Rows[0]["VehIn"],
                         dtHdr.Rows[0]["JobOpTime"], dtHdr.Rows[0]["VehCommitTime"], dtHdr.Rows[0]["VehOut"],
                          dtHdr.Rows[0]["EstmtID"], dtHdr.Rows[0]["VehIn_HDR_Id"], dtHdr.Rows[0]["ApPartAmt"],
                           dtHdr.Rows[0]["ApLabAmt"], dtHdr.Rows[0]["ApLubAmt"], dtHdr.Rows[0]["ApMiscAmt"],
                           iDlrBranchID, dtHdr.Rows[0]["job_confirm"], dtHdr.Rows[0]["job_canc_tag"],
                           dtHdr.Rows[0]["JbCd_Confirm"], iUserID, dtHdr.Rows[0]["SupervisiorID"],
                           dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["EstmtdTm"], dtHdr.Rows[0]["delay_Rsn_Id"],
                           dtHdr.Rows[0]["Aggregate"], dtHdr.Rows[0]["WarrantyTag"], dtHdr.Rows[0]["chassis_sale_date"],
                           dtHdr.Rows[0]["KmsJobcard"], dtHdr.Rows[0]["HrsJobcard"], dtHdr.Rows[0]["OdoMeterChange"],
                           dtHdr.Rows[0]["HrsMeterChange"], dtHdr.Rows[0]["Bfr_Last_SpdMtrChange_KmsJb"], dtHdr.Rows[0]["Bfr_Last_HrsMtrChange_HrsJb"]
                           , dtJbTaxDetails.Rows[0]["net_tr_amt"], dtJbTaxDetails.Rows[0]["discount_amt"], dtJbTaxDetails.Rows[0]["before_tax_amt"], dtJbTaxDetails.Rows[0]["mst_amt"], dtJbTaxDetails.Rows[0]["cst_amt"], dtJbTaxDetails.Rows[0]["surcharge_amt"], 
                           dtJbTaxDetails.Rows[0]["tot_amt"], dtJbTaxDetails.Rows[0]["pf_per"], dtJbTaxDetails.Rows[0]["pf_amt"], dtJbTaxDetails.Rows[0]["other_per"], dtJbTaxDetails.Rows[0]["other_money"], dtJbTaxDetails.Rows[0]["Jobcard_tot"], dtHdr.Rows[0]["CRM_HDR_ID"],
                           dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Model_ID"], dtHdr.Rows[0]["AggregateNo"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["InsCustID"]);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Save Chassis Status Details
        private bool bSaveChassisStatus(clsDB objDB, DataTable dtHdr, int iHdrID)
        {
            try
            {
                objDB.ExecuteStoredProcedure("SP_JobcardChassisStatus_Save", iHdrID, dtHdr.Rows[0]["Aggregate"],
                        dtHdr.Rows[0]["WarrantyTag"], dtHdr.Rows[0]["AMC_Chk"], dtHdr.Rows[0]["AMC_Type"], dtHdr.Rows[0]["AMC_End_Date"],
                        dtHdr.Rows[0]["In_KAM"], dtHdr.Rows[0]["Float_flag"], dtHdr.Rows[0]["UpgrdCamp"],
                        dtHdr.Rows[0]["UndObserv"], dtHdr.Rows[0]["UndObservEffFrom"], dtHdr.Rows[0]["UndObservEffTo"], dtHdr.Rows[0]["Theft_flag"],
                        dtHdr.Rows[0]["LstKm"], dtHdr.Rows[0]["Bfr_Last_SpdMtrChange_Kms"], dtHdr.Rows[0]["LstJbKm"],
                        dtHdr.Rows[0]["LstHrs"], dtHdr.Rows[0]["Bfr_Last_HrsMtrChange_Hrs"], dtHdr.Rows[0]["LstJbHrs"],
                        dtHdr.Rows[0]["Warranty_Kms"], dtHdr.Rows[0]["Warranty_hrs"], dtHdr.Rows[0]["Extended_Start_Kms"], 
                        dtHdr.Rows[0]["Extended_End_Kms"], dtHdr.Rows[0]["Extended_Start_Hrs"], dtHdr.Rows[0]["Extended_End_Hrs"],
                        dtHdr.Rows[0]["AMC_Start_KM"], dtHdr.Rows[0]["AMC_End_KM"], dtHdr.Rows[0]["AMC_Start_Hrs"], dtHdr.Rows[0]["AMC_End_Hrs"], dtHdr.Rows[0]["MTI_Cat"], 
                        dtHdr.Rows[0]["kmsTot"], dtHdr.Rows[0]["hrsTot"], dtHdr.Rows[0]["Warranty_End_Date"], dtHdr.Rows[0]["Extended_Warranty_start_Date"],
                        dtHdr.Rows[0]["Extended_Waranty_End_Date"], dtHdr.Rows[0]["AMC_St_Date"],
                        dtHdr.Rows[0]["Failure_Date"], dtHdr.Rows[0]["JobDate"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Job_Ty_ID"],
                        dtHdr.Rows[0]["Additional_Warranty_Start_date"], dtHdr.Rows[0]["Additional_Warranty_End_Date"]);                                                                                                                                          
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Save Chassis Status Details
        private bool bSaveJobcardInvCheck(clsDB objDB, int iDealerId, int iHdrID)
        {
            try
            {
                objDB.ExecuteStoredProcedure("SP_Jobcard_InvCheckSet", iDealerId, iHdrID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Save Part  Details
        private bool bSavePartDetails(clsDB objDB, DataTable dtDet, int iHdrID, int iUserID, int iDelearId, string sDealerCode)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                string sFinYear = Func.sGetFinancialYear(iDelearId);
                string sIPONo = Func.Common.sGetMaxSubDocNo(sDealerCode, "", "IPO", iDelearId);
                int iPos = 1;
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "0" && dtDet.Rows[iRowCnt]["IPO_no"].ToString() == "" && dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        dtDet.Rows[iRowCnt]["IPO_no"] = sIPONo;
                        if (iPos == 1) Func.Common.UpdateMaxNoSubDoc(objDB, sFinYear, "IPO", iDelearId);
                        iPos = iPos + 1;
                    }
                }

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "0" && dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardPartOil_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["part_type_tag"], dtDet.Rows[iRowCnt]["IPO_no"], dtDet.Rows[iRowCnt]["Mech_ID"], dtDet.Rows[iRowCnt]["ReqQty"], dtDet.Rows[iRowCnt]["RetQty"], dtDet.Rows[iRowCnt]["IssueQty"], dtDet.Rows[iRowCnt]["UseQty"], dtDet.Rows[iRowCnt]["BillQty"],
                            dtDet.Rows[iRowCnt]["war_tag"], dtDet.Rows[iRowCnt]["foc_tag"], dtDet.Rows[iRowCnt]["FOCQty"], dtDet.Rows[iRowCnt]["foc_reason_ID"], dtDet.Rows[iRowCnt]["FSCQty"], dtDet.Rows[iRowCnt]["PDIQty"], dtDet.Rows[iRowCnt]["AMCQty"], dtDet.Rows[iRowCnt]["CampaignQty"],
                            dtDet.Rows[iRowCnt]["transitQty"], dtDet.Rows[iRowCnt]["EnRouteTechQty"], dtDet.Rows[iRowCnt]["EnrouteNonTechQty"], dtDet.Rows[iRowCnt]["SpWarQty"], dtDet.Rows[iRowCnt]["GoodWlQty"],
                            dtDet.Rows[iRowCnt]["WarrQty"], dtDet.Rows[iRowCnt]["PrePDIQty"], dtDet.Rows[iRowCnt]["AggregateQty"], dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["MakeID"], dtDet.Rows[iRowCnt]["LubLocID"], dtDet.Rows[iRowCnt]["WarrRate"], dtDet.Rows[iRowCnt]["JobcodeID"], iUserID, dtDet.Rows[iRowCnt]["EstDtlID"], dtDet.Rows[iRowCnt]["Tax"], dtDet.Rows[iRowCnt]["AMCRate"], dtDet.Rows[iRowCnt]["AccdFlag"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Labor  Details
        private bool bSaveLabourDetails(clsDB objDB, DataTable dtDet, int iHdrID, int iUserID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        if (dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "0" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardLabour_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["part_type_tag"], dtDet.Rows[iRowCnt]["Lab_Tag"],
                            dtDet.Rows[iRowCnt]["out_lab_desc"], dtDet.Rows[iRowCnt]["ManHrs"], dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Job_Code_ID"], iUserID,
                            dtDet.Rows[iRowCnt]["AddLbrDescription"], dtDet.Rows[iRowCnt]["war_tag"],
                            dtDet.Rows[iRowCnt]["foc_tag"], dtDet.Rows[iRowCnt]["foc_reason_ID"], dtDet.Rows[iRowCnt]["EstDtlID"],
                            dtDet.Rows[iRowCnt]["rep_job"], dtDet.Rows[iRowCnt]["Mech_ID"], dtDet.Rows[iRowCnt]["out_Lab_amt"], dtDet.Rows[iRowCnt]["VenderID"],
                            dtDet.Rows[iRowCnt]["out_Mech_ID"], dtDet.Rows[iRowCnt]["AddLbrDescriptionID"], dtDet.Rows[iRowCnt]["Tax"], dtDet.Rows[iRowCnt]["AccdFlag"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardLabourDetails_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }


                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //Save Labour Timing Details
        private bool bSaveLabourTimingDetails(clsDB objDB, DataTable dtDet, int iHdrID, int iUserID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        if (dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "0" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardLabourTime_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["Lab_Tag"],
                            dtDet.Rows[iRowCnt]["StartTime"], dtDet.Rows[iRowCnt]["PauseReason"], dtDet.Rows[iRowCnt]["PauseTime"], dtDet.Rows[iRowCnt]["EndTime"], dtDet.Rows[iRowCnt]["SRNo"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardLabourTimeDetails_Del", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["Lab_Tag"]);
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
        private bool bSaveJobcardClaimComplaintDetails(clsDB objDB, DataTable dtComplaintDet, int iHdrID, int iDealerId, int iDlrBranchID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iJobcardDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtComplaintDet.Rows.Count; iRowCnt++)
                {
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iJobcardDetID = Func.Convert.iConvertToInt(dtComplaintDet.Rows[iRowCnt]["ID"]);
                        if (iJobcardDetID == 0)
                        {
                            iJobcardDetID = objDB.ExecuteStoredProcedure("SP_JobcardComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iDlrBranchID, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iDlrBranchID, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                    }
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardComplaint_Del", dtComplaintDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //Add Jobcode only when jobcard get confirm.
        private bool bAddJobcardCulpritDefectDetails(clsDB objDB, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_JobcardDefectCulprit_Add", iHdrID);
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        // Save Culprit, Defect And Technical Code 
        private bool bSaveJobcardCulpritDefectDetails(clsDB objDB, DataTable dtJobDet, int iHdrID, string sFConfirm)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iJobcardDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtJobDet.Rows.Count; iRowCnt++)
                {
                    if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iJobcardDetID = Func.Convert.iConvertToInt(dtJobDet.Rows[iRowCnt]["ID"]);
                        objDB.ExecuteStoredProcedure("SP_JobcardDefectCulprit_Save", dtJobDet.Rows[iRowCnt]["ID"], iHdrID, dtJobDet.Rows[iRowCnt]["Part_No_ID"], dtJobDet.Rows[iRowCnt]["Job_Code_ID"], dtJobDet.Rows[iRowCnt]["Culprit_ID"], dtJobDet.Rows[iRowCnt]["Defect_ID"], dtJobDet.Rows[iRowCnt]["Technical_ID"], sFConfirm);

                    }
                    //else if (dtJobDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    //{
                    //    objDB.ExecuteStoredProcedure("SP_JobcardDefectCulprit_Del", dtJobDet.Rows[iRowCnt]["ID"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Jobcard Investigations Details
        private bool bSaveJobcardClaimInvestigationDetails(clsDB objDB, DataTable dtInvestigationDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iJobcardDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtInvestigationDet.Rows.Count; iRowCnt++)
                {
                    if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iJobcardDetID = Func.Convert.iConvertToInt(dtInvestigationDet.Rows[iRowCnt]["ID"]);
                        if (iJobcardDetID == 0)
                        {
                            iJobcardDetID = objDB.ExecuteStoredProcedure("SP_JobcardInvestigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardInvestigation_Save", dtInvestigationDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtInvestigationDet.Rows[iRowCnt]["Investigation_ID"], dtInvestigationDet.Rows[iRowCnt]["Investigation_Desc"]);
                        }
                    }
                    else if (dtInvestigationDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardInvestigation_Del", dtInvestigationDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Jobcard Action Taken Details
        private bool bSaveJobcardClaimActionTakenDetails(clsDB objDB, DataTable dtActionDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iJobcardDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtActionDet.Rows.Count; iRowCnt++)
                {
                    if (dtActionDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iJobcardDetID = Func.Convert.iConvertToInt(dtActionDet.Rows[iRowCnt]["ID"]);
                        if (iJobcardDetID == 0)
                        {
                            iJobcardDetID = objDB.ExecuteStoredProcedure("SP_JobcardAction_Save", dtActionDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtActionDet.Rows[iRowCnt]["Action_ID"], dtActionDet.Rows[iRowCnt]["Dealer_Action"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardAction_Save", dtActionDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtActionDet.Rows[iRowCnt]["Action_ID"], dtActionDet.Rows[iRowCnt]["Dealer_Action"]);
                        }
                    }
                    else if (dtActionDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardAction_Del", dtActionDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        // Save Jobcard Parameter Checked Details
        private bool bSaveJobcardClaimParameterDetails(clsDB objDB, DataTable dtParameterDet, int iHdrID, int iDealerId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iJobcardDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtParameterDet.Rows.Count; iRowCnt++)
                {
                    if (dtParameterDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iJobcardDetID = Func.Convert.iConvertToInt(dtParameterDet.Rows[iRowCnt]["ID"]);
                        if (iJobcardDetID == 0)
                        {
                            iJobcardDetID = objDB.ExecuteStoredProcedure("SP_JobcardParameter_Save", dtParameterDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtParameterDet.Rows[iRowCnt]["ParameterChecked_ID"], dtParameterDet.Rows[iRowCnt]["Dealer_ParameterChecked"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_JobcardParameter_Save", dtParameterDet.Rows[iRowCnt]["ID"], iDealerId, iHdrID, dtParameterDet.Rows[iRowCnt]["ParameterChecked_ID"], dtParameterDet.Rows[iRowCnt]["Dealer_ParameterChecked"]);
                        }
                    }
                    else if (dtParameterDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardParameter_Del", dtParameterDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //
        public bool bSaveJobcodePCRDetails(ref int iHdrID, DataTable dtJobCdPCR, string sSubmit, string sDealerCode, int iUserID, DataTable dtFileAttach, string sApproved, string sReject, string sSubmitASM, string sApprovedASM, string sRejectASM, string sSubmitRSM, string sApprovedRSM, string sRejectRSM, string sSubmitHead, string sApprovedHead, string sRejectHead,string sPCRReject)
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
                if (dtJobCdPCR != null) if (bSavePCRHeader(objDB, sDealerCode, dtJobCdPCR, ref iHdrID, iUserID, sSubmit, sApproved, sReject, sSubmitASM, sApprovedASM, sRejectASM, sSubmitRSM, sApprovedRSM, sRejectRSM, sSubmitHead, sApprovedHead, sRejectHead, sPCRReject) == false) goto ExitFunction;

                //Add Attchment Details                
                if (dtFileAttach != null) if (bSavePCRFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;
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

        private bool bSavePCRHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserID, string sSubmit, string sApproved, string sReject, string sSubmitASM, string sApprovedASM, string sRejectASM, string sSubmitRSM, string sApprovedRSM, string sRejectRSM, string sSubmitHead, string sApprovedHead, string sRejectHead, string sPCRReject)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["PCRHDRID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    int iDlrBranchID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["DlrBranchID"]);
                    dtHdr.Rows[0]["PCRNo"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "PCR", iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_PCRHeader_Save", dtHdr.Rows[0]["PCRHDRID"], dtHdr.Rows[0]["PCRNo"], dtHdr.Rows[0]["PCRDate"],
                    dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["Jobcard_Hdr_ID"], dtHdr.Rows[0]["JobCodeID"], dtHdr.Rows[0]["PrimFailPartSuppID"], dtHdr.Rows[0]["PrimFailPartBatch"],
                    dtHdr.Rows[0]["VehicleAppl"], dtHdr.Rows[0]["NatureLoadCar"], dtHdr.Rows[0]["Payload"], dtHdr.Rows[0]["AvgRunPerDay"], dtHdr.Rows[0]["RoadCondition"], dtHdr.Rows[0]["FuelConsump"],
                    dtHdr.Rows[0]["EngOilConsump"], dtHdr.Rows[0]["SalesRegionID"], dtHdr.Rows[0]["InspectedBy"], dtHdr.Rows[0]["DtlDescription"], dtHdr.Rows[0]["ObservByMSECSM"], dtHdr.Rows[0]["ObservByASM"], dtHdr.Rows[0]["ObservByRSM"], sSubmit, sApproved, sReject, sSubmitASM, sApprovedASM, sRejectASM, sSubmitRSM, sApprovedRSM, sRejectRSM, sSubmitHead, sApprovedHead, sRejectHead, dtHdr.Rows[0]["ObservByHead"], sPCRReject);


                    Func.Common.UpdateMaxNo(objDB, sFinYear, "PCR", iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PCRHeader_Save", dtHdr.Rows[0]["PCRHDRID"], dtHdr.Rows[0]["PCRNo"], dtHdr.Rows[0]["PCRDate"],
                    dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["Jobcard_Hdr_ID"], dtHdr.Rows[0]["JobCodeID"], dtHdr.Rows[0]["PrimFailPartSuppID"], dtHdr.Rows[0]["PrimFailPartBatch"],
                    dtHdr.Rows[0]["VehicleAppl"], dtHdr.Rows[0]["NatureLoadCar"], dtHdr.Rows[0]["Payload"], dtHdr.Rows[0]["AvgRunPerDay"], dtHdr.Rows[0]["RoadCondition"], dtHdr.Rows[0]["FuelConsump"],
                    dtHdr.Rows[0]["EngOilConsump"], dtHdr.Rows[0]["SalesRegionID"], dtHdr.Rows[0]["InspectedBy"], dtHdr.Rows[0]["DtlDescription"], dtHdr.Rows[0]["ObservByMSECSM"], dtHdr.Rows[0]["ObservByASM"], dtHdr.Rows[0]["ObservByRSM"], sSubmit, sApproved, sReject, sSubmitASM, sApprovedASM, sRejectASM, sSubmitRSM, sApprovedRSM, sRejectRSM, sSubmitHead, sApprovedHead, sRejectHead, dtHdr.Rows[0]["ObservByHead"], sPCRReject);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["PCRHDRID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveGatePassHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["GPHDRID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    int iDlrBranchID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["DlrBranchID"]);
                    string sGPType = Func.Convert.sConvertToString(dtHdr.Rows[0]["GPtype"]);
                    //string sDocType = (sGPType.Trim() == "J") ? "GPJ" : "GPI";
                    string sDocType = "";
                    if (sGPType.Trim() == "J")
                        sDocType = "GPJ";
                    else if (sGPType.Trim() == "I")
                        sDocType = "GPI";
                    else
                        sDocType = "GPC";
                    dtHdr.Rows[0]["Gp_No"] = Func.Common.sGetMaxDocNo(sDealerCode, "", sDocType, iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_GatePassHeader_Save", dtHdr.Rows[0]["GPHDRID"], dtHdr.Rows[0]["Gp_No"], dtHdr.Rows[0]["Gp_date"],
                    dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["RefJobcardID"], dtHdr.Rows[0]["GPtype"], dtHdr.Rows[0]["RefSlInvID"], dtHdr.Rows[0]["RefJbInvID"],
                    dtHdr.Rows[0]["Narr"]);


                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocType, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_GatePassHeader_Save", dtHdr.Rows[0]["GPHDRID"], dtHdr.Rows[0]["Gp_No"], dtHdr.Rows[0]["Gp_date"],
                    dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["RefJobcardID"], dtHdr.Rows[0]["GPtype"], dtHdr.Rows[0]["RefJobcardID"], dtHdr.Rows[0]["RefJbInvID"],
                    dtHdr.Rows[0]["Narr"]);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["GPHDRID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool bSaveGatepass(ref int iHdrID, DataTable dtJbGatePass, string sDealerCode, int iUserID)
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
                if (dtJbGatePass != null) if (bSaveGatePassHeader(objDB, sDealerCode, dtJbGatePass, ref iHdrID, iUserID) == false) goto ExitFunction;

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

        public bool bSaveJobcardServiceVAN(ref int iHdrID, DataTable dtJbSrvVAN, string sDealerCode, int iUserID)
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
                if (dtJbSrvVAN != null) if (bSaveJbServVANDtls(objDB, sDealerCode, dtJbSrvVAN, ref iHdrID, iUserID) == false) goto ExitFunction;

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

        private bool bSaveJbServiceReminderDtls(clsDB objDB, int iHdrID)
        {
            try
            {
                objDB.ExecuteStoredProcedure("SP_JobcardChassisService", iHdrID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveJbServVANDtls(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["SrvVANHDRID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_JobcardServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
                    dtHdr.Rows[0]["Complaint_Tm"], dtHdr.Rows[0]["Mech_VAN_Out_Tm"], dtHdr.Rows[0]["Serv_Rate"], dtHdr.Rows[0]["Tot_Amt"], dtHdr.Rows[0]["LabType"], dtHdr.Rows[0]["SrvMechID"], dtHdr.Rows[0]["StKms"], dtHdr.Rows[0]["EndKms"]);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_JobcardServiceVAN_Save", dtHdr.Rows[0]["SrvVANHDRID"], dtHdr.Rows[0]["Jobcard_HDR_ID"], dtHdr.Rows[0]["From_Location"], dtHdr.Rows[0]["To_Location"], dtHdr.Rows[0]["One_Way_Dist"], dtHdr.Rows[0]["NO_Trips"], dtHdr.Rows[0]["TravelMode"],
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
        // Save Attached File Attached Details
        private bool bSavePCRFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_PCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_PCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_PCR_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public DataSet GetPCRForApprovalUserWise(string sRegionID, string sContryID, string sDealerID, string sFromDate, string sToDate, int iClaimStatus, int iUserRoleId, string sDomestic_Export, string sSearchText, int StartIndexRow, int MaxRowCount, string sSelRole, string sClaimTypeID, int iUserId, int ModelCategory_ID)
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPCRForApprovalDomestic", sRegionID, sContryID, sDealerID, sFromDate, sToDate, iClaimStatus, iUserRoleId, sDomestic_Export, sSearchText, ModelCategory_ID, iUserId, StartIndexRow, MaxRowCount, sSelRole, sClaimTypeID);
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

        private bool bSaveGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_JobcardDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_JobcardDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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
    }
}
