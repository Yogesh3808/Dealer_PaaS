using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public class clsCouponClaim
    {
        public clsCouponClaim()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        int TotalRowCount = 0;
        public DataSet GetCouponClaimUserWise(string sRegionID, string sContryID, string sDealerID, string sFromDate, string sToDate, string iClaimStatus, int iUserRoleId, string sSearchText, int iUserId)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = null; ;

                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCouponClaim_ForProcessing", sRegionID, sContryID, sDealerID,
                    sFromDate, sToDate, iClaimStatus, iUserRoleId, sSearchText, iUserId);

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

        public DataSet GetCouponClaim(string sSelect, int iCouponClaimID, int iDealerID, int iHOBrID, string sClaimType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCouponClaim", sSelect, iCouponClaimID, iDealerID, sClaimType);
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

        public bool bSubmitCouponClaimForSaveConfirm(DataTable dtDet, int iID, string sSaveOrConfirm)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_CouponClaim_Save", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["Coupon_Status"], iID, sSaveOrConfirm, dtDet.Rows[iRowCnt]["Reason"]);
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

        public bool bSaveCoupon(DataTable dtHdr, string sDealerCode, DataTable dtDet, DataTable dtGrpTaxDetails, ref int iHdrID)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                string sEstConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Claim_confirm"]);
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID, Func.Convert.iConvertToInt(dtHdr.Rows[0]["UserID"])) == false) goto ExitFunction;

                //save Part Details Details
                if (dtDet != null) if (bSaveCouponDetails(objDB, dtDet, iHdrID, Func.Convert.iConvertToInt(dtHdr.Rows[0]["UserID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_Id"]), sDealerCode) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveCouponGroupTaxDetails(objDB, sDealerCode, dtGrpTaxDetails, iHdrID) == false) goto ExitFunction;

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

        private bool bSaveCouponGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_CouponClaimDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0") //&& Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0
                    {
                        objDB.ExecuteStoredProcedure("SP_CouponClaimDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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

        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iDlrBranchID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    //dtHdr.Rows[0]["Coupon_Claim_No"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "CPN" + dtHdr.Rows[0]["ClaimType"], iDelearId);
                    //dtHdr.Rows[0]["Coupon_Claim_No"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "C" + dtHdr.Rows[0]["ClaimType"], iDelearId);
                    dtHdr.Rows[0]["Coupon_Claim_No"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "CO" + ((Func.Convert.sConvertToString(dtHdr.Rows[0]["ClaimType"]) == "P") ? "D" : "F"), iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_CouponClaimHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Coupon_Claim_No"],
                        dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Dealer_ID"], iDlrBranchID, dtHdr.Rows[0]["Coupon_Claim_Amt"],
                        dtHdr.Rows[0]["Claim_confirm"], dtHdr.Rows[0]["ClaimType"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]);

                    //Func.Common.UpdateMaxNo(objDB, sFinYear, "CPN" + dtHdr.Rows[0]["ClaimType"], iDelearId);
                    //Func.Common.UpdateMaxNo(objDB, sFinYear, "C" + dtHdr.Rows[0]["ClaimType"], iDelearId);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "CO" + ((Func.Convert.sConvertToString(dtHdr.Rows[0]["ClaimType"]) == "P") ? "D" : "F"), iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_CouponClaimHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Coupon_Claim_No"],
                        dtHdr.Rows[0]["Claim_date"], dtHdr.Rows[0]["Dealer_ID"], iDlrBranchID, dtHdr.Rows[0]["Coupon_Claim_Amt"],
                        dtHdr.Rows[0]["Claim_confirm"], dtHdr.Rows[0]["ClaimType"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]);

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
        private bool bSaveCouponDetails(clsDB objDB, DataTable dtDet, int iHdrID, int iUserID, int iDelearId, string sDealerCode)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                string sFinYear = Func.sGetFinancialYear(iDelearId);

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() == "N")
                    {
                        if (dtDet.Rows[iRowCnt]["Jobcard_HDR_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_CouponClaimDtls_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Jobcard_HDR_ID"], dtDet.Rows[iRowCnt]["Tax"], dtDet.Rows[iRowCnt]["Tax1"], dtDet.Rows[iRowCnt]["Tax2"], dtDet.Rows[iRowCnt]["TaxAmt"], dtDet.Rows[iRowCnt]["TotAmtWtTax"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D" || dtDet.Rows[iRowCnt]["ID"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_CouponClaimDtls_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    } 
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool CancelConfirmCouponClaim(bool bCancelConfirm, string iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                string sSQlUpdate = "";
                if (bCancelConfirm == true)
                {
                    sSQlUpdate = "Update TM_CouponClaimHeader Set Cancel='Y' where ID =" + iHdrID;
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
    }
}
