using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSpareSalesRetNote
    /// </summary>
    public class clsSpareSalesRetNote
    {
        String sSalesRetNote = "";
        public clsSpareSalesRetNote()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveSRNHeader(clsDB objDB, DataTable dtHdr, DataTable dtSRNTaxDetails, string sDealerCode, ref int iHdrID)
        {
            string sDocName = "SRN";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["DealerID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sSalesRetNote = Func.Convert.sConvertToString(dtHdr.Rows[0]["SRN_No"]);
                    dtHdr.Rows[0]["SRN_No"] = Func.Convert.sConvertToString(GenerateSalesRetNote(sDealerCode, iDealerId));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SRNHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SRN_No"], dtHdr.Rows[0]["SRN_Date"],
                        dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtSRNTaxDetails.Rows[0]["net_tr_amt"], dtSRNTaxDetails.Rows[0]["discount_amt"], dtSRNTaxDetails.Rows[0]["before_tax_amt"],
                        dtSRNTaxDetails.Rows[0]["mst_amt"], dtSRNTaxDetails.Rows[0]["cst_amt"], dtSRNTaxDetails.Rows[0]["surcharge_amt"],
                        dtSRNTaxDetails.Rows[0]["tot_amt"], dtSRNTaxDetails.Rows[0]["pf_per"], dtSRNTaxDetails.Rows[0]["pf_amt"],
                        dtSRNTaxDetails.Rows[0]["other_per"], dtSRNTaxDetails.Rows[0]["other_money"], dtSRNTaxDetails.Rows[0]["Total"]
                        , dtHdr.Rows[0]["IS_PerAmt"], 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["DealerID"], dtHdr.Rows[0]["CustTaxTag"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SRNHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SRN_No"], dtHdr.Rows[0]["SRN_Date"], dtHdr.Rows[0]["UserID"],
                        dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"], dtSRNTaxDetails.Rows[0]["net_tr_amt"],
                        dtSRNTaxDetails.Rows[0]["discount_amt"], dtSRNTaxDetails.Rows[0]["before_tax_amt"], dtSRNTaxDetails.Rows[0]["mst_amt"],
                        dtSRNTaxDetails.Rows[0]["cst_amt"], dtSRNTaxDetails.Rows[0]["surcharge_amt"], dtSRNTaxDetails.Rows[0]["tot_amt"],
                        dtSRNTaxDetails.Rows[0]["pf_per"], dtSRNTaxDetails.Rows[0]["pf_amt"], dtSRNTaxDetails.Rows[0]["other_per"],
                        dtSRNTaxDetails.Rows[0]["other_money"], dtSRNTaxDetails.Rows[0]["Total"]
                        , dtHdr.Rows[0]["IS_PerAmt"], 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["DealerID"], dtHdr.Rows[0]["CustTaxTag"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool bSavePartSRNDetails(clsDB objDB, DataTable dtHdr, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            string sIsConfirm = "N";
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_SRNDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Rate"],
                                dtDet.Rows[iRowCnt]["Invoice_No"], dtDet.Rows[iRowCnt]["Invoice_Qty"], dtDet.Rows[iRowCnt]["Ret_Qty"], dtDet.Rows[iRowCnt]["Rev_Inv_Qty"],
                                dtDet.Rows[iRowCnt]["Disc_Per"], dtDet.Rows[iRowCnt]["disc_amt"], dtDet.Rows[iRowCnt]["disc_rate"],
                                dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["PartTaxID"], dtDet.Rows[iRowCnt]["Price"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SRNDet_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                if (iHdrID != 0)
                {
                    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]);
                    if (sIsConfirm == "Y")
                        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "SRN", dtHdr.Rows[0]["ID"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
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
                        objDB.ExecuteStoredProcedure("SP_SRNDetTax_Save", 0, iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                              dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Total"], 0, dtGrTaxDet.Rows[iRowCnt]["TAX_Percentage"], dtGrTaxDet.Rows[iRowCnt]["Tax1_Per"],
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
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtSRNTaxDetails, ref int iHdrID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveSRNHeader(objDB, dtHdr, dtSRNTaxDetails, sDealerCode, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartSRNDetails(objDB, dtHdr, dtDet, iHdrID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, dtGrTaxDet, iHdrID) == false) goto ExitFunction;

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
        public DataSet GetSalesRetNote(int iID, string sSelType, int iDealerID, int iCustID, int iUserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_SalesRetNote_Get", iID, sSelType, iDealerID, iCustID, iUserID);
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
        public string GenerateSalesRetNote(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "SRN";
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
        public DataTable GetSysStock(string sPartIDs, int iUserID, string sSelType, int DistributorID, int SRNID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetSysStock", sPartIDs, iUserID, sSelType, DistributorID, SRNID);
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
    }
}
