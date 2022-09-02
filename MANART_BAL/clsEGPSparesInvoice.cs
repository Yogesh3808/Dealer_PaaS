using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{

    /// <summary>
    /// Summary description for clsEGPSparesInvoice
    /// </summary>
    public class clsEGPSparesInvoice
    {
        String sPONo = "";
        public clsEGPSparesInvoice()
        {

        }
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtInvTaxDetails, ref int iHdrID)
        {
            //string sDocName = "INV";
            string sDocName = "";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0 && Func.Convert.sConvertToString(dtHdr.Rows[0]["TrNo"]) != "")
                {

                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sPONo = Func.Convert.sConvertToString(dtHdr.Rows[0]["Inv_no"]);
                    int iInvType = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Inv_type"]);
                    //string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CC" : "CS";
                    string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CPC" : (iInvType == 1) ? "CPS" : "STI";
                    //sDocName = "INV" + sINVSer;
                    sDocName = sINVSer;
                    //dtHdr.Rows[0]["Inv_no"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "INV" + sINVSer, iDealerId);
                    //dtHdr.Rows[0]["Inv_no"] = Func.Convert.sConvertToString(GenerateInvNo(sDealerCode, iDealerId, "INV" + sINVSer));
                    dtHdr.Rows[0]["Inv_no"] = Func.Convert.sConvertToString(GenerateInvNo(sDealerCode, iDealerId, sINVSer));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesInvHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inv_no"], dtHdr.Rows[0]["Inv_Date"],
                        dtHdr.Rows[0]["Inv_type"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Inv_close"], dtHdr.Rows[0]["Inv_canc"],
                        dtHdr.Rows[0]["Inv_com"], dtHdr.Rows[0]["Inv_lock"], dtInvTaxDetails.Rows[0]["net_tr_amt"], dtHdr.Rows[0]["reference"], dtHdr.Rows[0]["narration"],
                        dtHdr.Rows[0]["Inv_validity_date"], dtInvTaxDetails.Rows[0]["discount_amt"], dtInvTaxDetails.Rows[0]["before_tax_amt"], dtInvTaxDetails.Rows[0]["mst_amt"],
                        dtInvTaxDetails.Rows[0]["cst_amt"], dtInvTaxDetails.Rows[0]["surcharge_amt"], dtInvTaxDetails.Rows[0]["tot_amt"], dtInvTaxDetails.Rows[0]["pf_per"],
                        dtInvTaxDetails.Rows[0]["pf_amt"], dtInvTaxDetails.Rows[0]["other_per"], dtInvTaxDetails.Rows[0]["other_money"], dtInvTaxDetails.Rows[0]["Inv_tot"],
                        dtHdr.Rows[0]["Scheme"], dtHdr.Rows[0]["Job_HDR_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["IS_PerAmt"], 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]
                        , dtInvTaxDetails.Rows[0]["PF_Tax_Per"], dtInvTaxDetails.Rows[0]["PF_IGSTorSGST_Amt"], dtInvTaxDetails.Rows[0]["PF_Tax_Per1"], dtInvTaxDetails.Rows[0]["PF_CGST_Amt"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesInvHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inv_no"], dtHdr.Rows[0]["Inv_Date"], dtHdr.Rows[0]["Inv_type"],
                        dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Inv_close"], dtHdr.Rows[0]["Inv_canc"], dtHdr.Rows[0]["Inv_com"],
                        dtHdr.Rows[0]["Inv_lock"], dtInvTaxDetails.Rows[0]["net_tr_amt"], dtHdr.Rows[0]["reference"], dtHdr.Rows[0]["narration"],
                        dtHdr.Rows[0]["Inv_validity_date"], dtInvTaxDetails.Rows[0]["discount_amt"], dtInvTaxDetails.Rows[0]["before_tax_amt"],
                        dtInvTaxDetails.Rows[0]["mst_amt"], dtInvTaxDetails.Rows[0]["cst_amt"], dtInvTaxDetails.Rows[0]["surcharge_amt"], dtInvTaxDetails.Rows[0]["tot_amt"],
                        dtInvTaxDetails.Rows[0]["pf_per"], dtInvTaxDetails.Rows[0]["pf_amt"], dtInvTaxDetails.Rows[0]["other_per"], dtInvTaxDetails.Rows[0]["other_money"],
                        dtInvTaxDetails.Rows[0]["Inv_tot"], dtHdr.Rows[0]["Scheme"], dtHdr.Rows[0]["Job_HDR_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["IS_PerAmt"]
                        , 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["TrNo"]
                        , dtInvTaxDetails.Rows[0]["PF_Tax_Per"], dtInvTaxDetails.Rows[0]["PF_IGSTorSGST_Amt"], dtInvTaxDetails.Rows[0]["PF_Tax_Per1"], dtInvTaxDetails.Rows[0]["PF_CGST_Amt"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool bSavePartDetails(clsDB objDB, string sDealerCode, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_InvDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID,
                                dtDet.Rows[iRowCnt]["OA_Det_ID"], dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["MRPRate"],
                                dtDet.Rows[iRowCnt]["bal_qty"], dtDet.Rows[iRowCnt]["discount_per"], dtDet.Rows[iRowCnt]["discount_amt"], dtDet.Rows[iRowCnt]["disc_rate"],
                                dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["PartTaxID"], dtDet.Rows[iRowCnt]["Price"], dtDet.Rows[iRowCnt]["BFRGST"]
                                , dtDet.Rows[iRowCnt]["group_code"], dtDet.Rows[iRowCnt]["Labour_Total"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_InvDet_Del", iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["OA_Det_ID"]);
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
                        objDB.ExecuteStoredProcedure("SP_InvDescDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["InvDescID"]);
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
                objDB.ExecuteStoredProcedure("SP_InvDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    //For this condition we have written SP_InvDet_Tax_NonExitDelete proc seperately for delete non selected tax
                    //if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0" && Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0)
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0")
                    {
                        //@ID int,@Inv_HDR_ID int, @group_code varchar(2), @net_inv_amt money, @discount_per money,@discount_amt money,@Tax_Code int,
                        //@tax_amt money,@tax1_code int,@tax1_amt money,@tax2_code int,@tax2_amt money,@Total money

                        objDB.ExecuteStoredProcedure("SP_InvDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                            dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                            dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"], dtGrTaxDet.Rows[iRowCnt]["Total"],
                            0);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtInvTaxDetails, ref int iHdrID, DataTable dtInvJobDescDet)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, dtInvTaxDetails, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, sDealerCode, dtDet, iHdrID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, sDealerCode, dtGrTaxDet, iHdrID) == false) goto ExitFunction;

                //Save Inv Job Desc
                if (bSaveInvJobDescDetails(objDB, sDealerCode, dtInvJobDescDet, iHdrID) == false) goto ExitFunction;

                //After saving All tax details it removes only non selected Part tax details from tax table
                objDB.ExecuteStoredProcedure("SP_InvDet_Tax_NonExitDelete", iHdrID);

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

        public DataSet GetInv(int InvId, string InvType, int DealerID, int iCustID, int iMenuID) //change for REMAN PO
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Inv", InvId, InvType, DealerID, iCustID, iMenuID);
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

        public string GenerateInvNo(string sDealerCode, int iDealerID, string sInvDocName)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = sInvDocName;

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
        }

        //New Code for Get Latest Stock for all parts Dated on 01/08/2017 Vikram K
        public DataTable GetSysStock(string sPartIDs, int iUserID, string sSelType, int iDealerID, int iInvHdrID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_SysStock_Get", sPartIDs, iUserID, sSelType, iDealerID, iInvHdrID);
                return dt;
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
        //END
        #region GetAllPending OA List having Stock > zero Dated 13/10/2013
        public DataTable GetAllPendingOAList(int iDealerId, int iCustID, string SelectedOANo)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetPendingOA_List", iDealerId, iCustID, SelectedOANo);
                return dt;
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
        #endregion

    }
}
