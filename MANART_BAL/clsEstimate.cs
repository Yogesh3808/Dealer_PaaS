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
    public class clsEstimate
    {
        public clsEstimate()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetEstimate(int iDocID, string sDocType, int iDealerID, int iHOBranchDealerId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Estimate", iDocID, sDocType, iDealerID, iHOBranchDealerId);
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
        public bool bSaveEstimate(ref int iHdrID, string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtLabour, DataTable dtComplaintDet, DataTable dtGrpTaxDetails, DataTable dtTaxDetails, int iDealerId, int iDlrBranchID, int iUserID, string sAddCStatus, DataTable dtInvJobDescDet)//DataTable dtEstDet,
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sEstConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Est_confirm"]);
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr,dtTaxDetails, ref iHdrID, iDlrBranchID, iUserID) == false) goto ExitFunction;

                //save Part Details Details
                if (dtDet != null) if (bSavePartDetails(objDB, dtDet, iHdrID, iUserID, iDealerId, sDealerCode) == false) goto ExitFunction;

                // Save Complaint Details 
                if (dtComplaintDet != null) if (bSaveEstimateClaimComplaintDetails(objDB, dtComplaintDet, iHdrID, iDealerId, iDlrBranchID) == false) goto ExitFunction;

                //save Labour Details Details
                if (dtLabour != null) if (bSaveLabourDetails(objDB, dtLabour, iHdrID, iUserID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, sDealerCode, dtGrpTaxDetails, iHdrID) == false) goto ExitFunction;

                //Save Inv Job Desc
                if (bSaveInvJobDescDetails(objDB, sDealerCode, dtInvJobDescDet, iHdrID) == false) goto ExitFunction;

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
        
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr,DataTable dtTaxDetails, ref int iHdrID, int iDlrBranchID, int iUserID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {                    
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Est_no"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "Est", iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_EstimateHeader_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Est_no"],
                        dtHdr.Rows[0]["EstDate"],dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Dealer_ID"],                        
                           iDlrBranchID, dtHdr.Rows[0]["Est_confirm"], dtHdr.Rows[0]["Est_canc_tag"],iUserID, dtHdr.Rows[0]["SupervisiorID"],
                           dtHdr.Rows[0]["InsurnceComp"],dtHdr.Rows[0]["PolicyNo"],dtHdr.Rows[0]["DriverName"],dtHdr.Rows[0]["DriverLicNo"],
                           dtHdr.Rows[0]["Surveyor"], dtHdr.Rows[0]["Phone"], dtHdr.Rows[0]["InspectDt"], dtHdr.Rows[0]["VisitDt"], 
                           dtHdr.Rows[0]["InsuranceDiv"], dtHdr.Rows[0]["InsurnceValDt"]
                           , dtTaxDetails.Rows[0]["net_tr_amt"], dtTaxDetails.Rows[0]["discount_amt"], dtTaxDetails.Rows[0]["before_tax_amt"], 
                           dtTaxDetails.Rows[0]["mst_amt"], dtTaxDetails.Rows[0]["cst_amt"], dtTaxDetails.Rows[0]["surcharge_amt"], dtTaxDetails.Rows[0]["tot_amt"], 
                           dtTaxDetails.Rows[0]["pf_per"], dtTaxDetails.Rows[0]["pf_amt"], dtTaxDetails.Rows[0]["other_per"], dtTaxDetails.Rows[0]["other_money"],
                           dtTaxDetails.Rows[0]["Estimate_tot"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["InsCustID"]);

                    dtHdr.Rows[0]["ID"] = iHdrID;

                    Func.Common.UpdateMaxNo(objDB, sFinYear, "Est", iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_EstimateHeader_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Est_no"],
                        dtHdr.Rows[0]["EstDate"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Dealer_ID"],                      
                        iDlrBranchID, dtHdr.Rows[0]["Est_confirm"], dtHdr.Rows[0]["Est_canc_tag"],iUserID, dtHdr.Rows[0]["SupervisiorID"],
                        dtHdr.Rows[0]["InsurnceComp"], dtHdr.Rows[0]["PolicyNo"], dtHdr.Rows[0]["DriverName"], dtHdr.Rows[0]["DriverLicNo"],
                        dtHdr.Rows[0]["Surveyor"], dtHdr.Rows[0]["Phone"], dtHdr.Rows[0]["InspectDt"], dtHdr.Rows[0]["VisitDt"],
                        dtHdr.Rows[0]["InsuranceDiv"], dtHdr.Rows[0]["InsurnceValDt"]
                        , dtTaxDetails.Rows[0]["net_tr_amt"], dtTaxDetails.Rows[0]["discount_amt"], dtTaxDetails.Rows[0]["before_tax_amt"], dtTaxDetails.Rows[0]["mst_amt"], dtTaxDetails.Rows[0]["cst_amt"], dtTaxDetails.Rows[0]["surcharge_amt"], dtTaxDetails.Rows[0]["tot_amt"], dtTaxDetails.Rows[0]["pf_per"], dtTaxDetails.Rows[0]["pf_amt"], dtTaxDetails.Rows[0]["other_per"], dtTaxDetails.Rows[0]["other_money"], dtTaxDetails.Rows[0]["Estimate_tot"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["InsCustID"]);

                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
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
                
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        if (dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_EstimatePartOil_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["part_type_tag"], dtDet.Rows[iRowCnt]["ReqQty"],
                              "D", dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], iUserID, dtDet.Rows[iRowCnt]["Tax"]);
                        }
                    }                     
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_EstimatePartLabourDetails_Del", dtDet.Rows[iRowCnt]["ID"]);
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
                            objDB.ExecuteStoredProcedure("SP_EstimateLabour_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PartLabourID"], dtDet.Rows[iRowCnt]["part_type_tag"], dtDet.Rows[iRowCnt]["Lab_Tag"],
                            dtDet.Rows[iRowCnt]["out_lab_desc"], dtDet.Rows[iRowCnt]["ManHrs"], dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"],  iUserID,
                            dtDet.Rows[iRowCnt]["AddLbrDescription"], dtDet.Rows[iRowCnt]["out_Lab_amt"], dtDet.Rows[iRowCnt]["AddLbrDescriptionID"], dtDet.Rows[iRowCnt]["Tax"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D" || dtDet.Rows[iRowCnt]["PartLabourID"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_EstimatePartLabourDetails_Del", dtDet.Rows[iRowCnt]["ID"]);
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
        private bool bSaveEstimateClaimComplaintDetails(clsDB objDB, DataTable dtComplaintDet, int iHdrID, int iDealerId, int iDlrBranchID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iEstimateDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtComplaintDet.Rows.Count; iRowCnt++)
                {
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iEstimateDetID = Func.Convert.iConvertToInt(dtComplaintDet.Rows[iRowCnt]["ID"]);
                        if (iEstimateDetID == 0)
                        {
                            iEstimateDetID = objDB.ExecuteStoredProcedure("SP_EstimateComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iDlrBranchID, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_EstimateComplaint_Save", dtComplaintDet.Rows[iRowCnt]["ID"], iDealerId, iDlrBranchID, iHdrID, dtComplaintDet.Rows[iRowCnt]["Complaint_ID"], dtComplaintDet.Rows[iRowCnt]["Complaint_Desc"]);
                        }
                    }
                    if (dtComplaintDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_EstimateComplaint_Del", dtComplaintDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveInvJobDescDetails(clsDB objDB, string sDealerCode, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["InvDescID"].ToString() != "0" || dtDet.Rows[iRowCnt]["ID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_EstimateDescDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["InvDescID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        private bool bSaveGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_EstimateDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_EstimateDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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
