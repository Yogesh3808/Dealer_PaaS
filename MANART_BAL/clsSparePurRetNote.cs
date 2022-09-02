using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSparePurRetNote
    /// </summary>
    public class clsSparePurRetNote
    {
        String sPurRetNote = "";
        public clsSparePurRetNote()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSavePRNHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtPRNTaxDetails, ref int iHdrID)
        {
            string sDocName = "PRN";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0 && Func.Convert.sConvertToString(dtHdr.Rows[0]["TrNo"]) != "")
                {
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    dtHdr.Rows[0]["PRN_No"] = Func.Convert.sConvertToString(GeneratePurRetNote(sDealerCode, iDealerId));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_PRNHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PRN_No"], dtHdr.Rows[0]["PRN_Date"],
                        dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["Is_PRN_Mode_Auto"], dtHdr.Rows[0]["Invoice_No"], dtHdr.Rows[0]["Invoice_Date"], dtHdr.Rows[0]["GRN_No"],
                        dtHdr.Rows[0]["GRN_Date"], dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Delivery_No"], dtHdr.Rows[0]["IS_PerAmt"],
                        dtPRNTaxDetails.Rows[0]["net_tr_amt"], dtPRNTaxDetails.Rows[0]["discount_amt"], dtPRNTaxDetails.Rows[0]["before_tax_amt"],
                        dtPRNTaxDetails.Rows[0]["mst_amt"], dtPRNTaxDetails.Rows[0]["cst_amt"], dtPRNTaxDetails.Rows[0]["surcharge_amt"],
                        dtPRNTaxDetails.Rows[0]["tot_amt"], dtPRNTaxDetails.Rows[0]["pf_per"], dtPRNTaxDetails.Rows[0]["pf_amt"],
                        dtPRNTaxDetails.Rows[0]["other_per"], dtPRNTaxDetails.Rows[0]["other_money"], dtPRNTaxDetails.Rows[0]["Total"],
                        0, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Is_AppClaimRet"], dtHdr.Rows[0]["TrNo"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PRNHdr_Save",
                        dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PRN_No"], dtHdr.Rows[0]["PRN_Date"],
                        dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["Is_PRN_Mode_Auto"], dtHdr.Rows[0]["Invoice_No"], dtHdr.Rows[0]["Invoice_Date"], dtHdr.Rows[0]["GRN_No"],
                        dtHdr.Rows[0]["GRN_Date"], dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Delivery_No"], dtHdr.Rows[0]["IS_PerAmt"],
                        dtPRNTaxDetails.Rows[0]["net_tr_amt"], dtPRNTaxDetails.Rows[0]["discount_amt"], dtPRNTaxDetails.Rows[0]["before_tax_amt"],
                        dtPRNTaxDetails.Rows[0]["mst_amt"], dtPRNTaxDetails.Rows[0]["cst_amt"], dtPRNTaxDetails.Rows[0]["surcharge_amt"],
                        dtPRNTaxDetails.Rows[0]["tot_amt"], dtPRNTaxDetails.Rows[0]["pf_per"], dtPRNTaxDetails.Rows[0]["pf_amt"],
                        dtPRNTaxDetails.Rows[0]["other_per"], dtPRNTaxDetails.Rows[0]["other_money"],
                        dtPRNTaxDetails.Rows[0]["Total"], 0, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Is_AppClaimRet"], dtHdr.Rows[0]["TrNo"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool bSavePartPRNDetails(clsDB objDB, DataTable dtHdr, DataTable dtDet, int iHdrID)
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
                            objDB.ExecuteStoredProcedure("SP_PRNDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"],
                                dtDet.Rows[iRowCnt]["Invoice_No"], dtDet.Rows[iRowCnt]["Invoice_Qty"], dtDet.Rows[iRowCnt]["Ret_Qty"],
                                dtDet.Rows[iRowCnt]["Price"], dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Disc_Per"],
                                dtDet.Rows[iRowCnt]["Accept_Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["MR_Dts_ID"],
                                dtDet.Rows[iRowCnt]["PartTaxID"], dtDet.Rows[iRowCnt]["Tax_Per"], dtDet.Rows[iRowCnt]["PO_No"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_PRNDet_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                //if (iHdrID != 0)
                //{
                //    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]);
                //    if (sIsConfirm == "Y")
                //        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "PRN", iHdrID);
                //}
                bSaveRecord = true;
            }
            catch
            {
                bSaveRecord = false;
            }
            return bSaveRecord;
        }
        private bool bSaveGroupTaxDetails(clsDB objDB, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_PRNDetTax_Delete", iHdrID);
                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0" && Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0)
                    {
                        objDB.ExecuteStoredProcedure("SP_PRNDetTax_Save", 0, iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
                              dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"], dtGrTaxDet.Rows[iRowCnt]["discount_per"], dtGrTaxDet.Rows[iRowCnt]["discount_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Tax_Code"], dtGrTaxDet.Rows[iRowCnt]["tax_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax1_code"], dtGrTaxDet.Rows[iRowCnt]["tax1_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["tax2_code"], dtGrTaxDet.Rows[iRowCnt]["tax2_amt"],
                              dtGrTaxDet.Rows[iRowCnt]["Total"], 0);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtPRNTaxDetails, ref int iHdrID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSavePRNHeader(objDB, sDealerCode, dtHdr, dtPRNTaxDetails, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartPRNDetails(objDB, dtHdr, dtDet, iHdrID) == false) goto ExitFunction;

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
        public DataSet GetPurRetNote(int iID, string sSelType, int iDealerID, int iUserID, int iSupplierID, string sIS_AutoPRN, string sInvoiceNo)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_PurRetNote_Get", iID, sSelType, iDealerID, iUserID, iSupplierID, sIS_AutoPRN, sInvoiceNo);
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
        public string GeneratePurRetNote(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "PRN";
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
        public DataTable GetSysStock(string sPartIDs, int iUserID, string sSelType, int DistributorID, int PRNID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetSysStock", sPartIDs, iUserID, sSelType, DistributorID, PRNID);
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
