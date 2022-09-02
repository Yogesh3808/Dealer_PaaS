using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSparesOA
    /// </summary>
    public class clsSparesOA
    {
        string sDocName = "";
        public clsSparesOA()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtOATaxDetails, ref int iHdrID)
        {
            sDocName = "OA";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {

                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    
                    //dtHdr.Rows[0]["oa_no"]=Func.Common.sGetMaxDocNo(sDealerCode, "", "OA", iDealerId); // Example - D016514/OA/2016/0005
                    dtHdr.Rows[0]["oa_no"] = Func.Convert.sConvertToString(GenerateOANo(sDealerCode, iDealerId)); // Example - 16514OA600006

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesOAHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["oa_no"],
                        dtHdr.Rows[0]["oa_Date"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["oa_close"],
                        dtHdr.Rows[0]["oa_canc"], dtHdr.Rows[0]["oa_com"], dtHdr.Rows[0]["oa_lock"], dtOATaxDetails.Rows[0]["net_tr_amt"],
                        dtHdr.Rows[0]["reference"], dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["oa_validity_date"],
                        dtOATaxDetails.Rows[0]["discount_amt"], dtOATaxDetails.Rows[0]["before_tax_amt"], dtOATaxDetails.Rows[0]["mst_amt"],
                        dtOATaxDetails.Rows[0]["cst_amt"], dtOATaxDetails.Rows[0]["surcharge_amt"], dtOATaxDetails.Rows[0]["tot_amt"],
                        dtOATaxDetails.Rows[0]["pf_per"], dtOATaxDetails.Rows[0]["pf_amt"], dtOATaxDetails.Rows[0]["other_per"],
                        dtOATaxDetails.Rows[0]["other_money"], dtOATaxDetails.Rows[0]["oa_tot"], dtHdr.Rows[0]["IS_PerAmt"],0, 
                        dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["CustTaxTag"],dtOATaxDetails.Rows[0]["PF_Tax_Per"], 
                        dtOATaxDetails.Rows[0]["PF_IGSTorSGST_Amt"], dtOATaxDetails.Rows[0]["PF_Tax_Per1"], dtOATaxDetails.Rows[0]["PF_CGST_Amt"]
                        , dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["GSTOAType"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesOAHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["oa_no"], dtHdr.Rows[0]["oa_Date"],
                        dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["oa_close"], dtHdr.Rows[0]["oa_canc"],
                        dtHdr.Rows[0]["oa_com"], dtHdr.Rows[0]["oa_lock"], dtOATaxDetails.Rows[0]["net_tr_amt"], dtHdr.Rows[0]["reference"],
                        dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["oa_validity_date"], dtOATaxDetails.Rows[0]["discount_amt"],
                        dtOATaxDetails.Rows[0]["before_tax_amt"], dtOATaxDetails.Rows[0]["mst_amt"], dtOATaxDetails.Rows[0]["cst_amt"],
                        dtOATaxDetails.Rows[0]["surcharge_amt"], dtOATaxDetails.Rows[0]["tot_amt"], dtOATaxDetails.Rows[0]["pf_per"],
                        dtOATaxDetails.Rows[0]["pf_amt"], dtOATaxDetails.Rows[0]["other_per"], dtOATaxDetails.Rows[0]["other_money"],
                        dtOATaxDetails.Rows[0]["oa_tot"], dtHdr.Rows[0]["IS_PerAmt"], 0, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["CustTaxTag"],
                        dtOATaxDetails.Rows[0]["PF_Tax_Per"], dtOATaxDetails.Rows[0]["PF_IGSTorSGST_Amt"], dtOATaxDetails.Rows[0]["PF_Tax_Per1"],
                        dtOATaxDetails.Rows[0]["PF_CGST_Amt"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["GSTOAType"]);
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
                //@ID, @OA_HDR_ID, @Part_ID, @Qty, @Rate, @bal_qty, @disount_per, discount_amt, @disc_rate, @Total 
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_OADet_Save",
                                dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["MRPRate"],
                                dtDet.Rows[iRowCnt]["bal_qty"], dtDet.Rows[iRowCnt]["discount_per"], dtDet.Rows[iRowCnt]["discount_amt"], dtDet.Rows[iRowCnt]["disc_rate"],
                                dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["PartTaxID"], dtDet.Rows[iRowCnt]["Price"], dtDet.Rows[iRowCnt]["group_code"]
                                , dtDet.Rows[iRowCnt]["BFRGST"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_OADet_Del", iHdrID, dtDet.Rows[iRowCnt]["Part_ID"]);
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
                objDB.ExecuteStoredProcedure("SP_OADet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0" && Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0)
                    {
                        objDB.ExecuteStoredProcedure("SP_OADetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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

        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtOATaxDetails)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, dtOATaxDetails, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, sDealerCode, dtDet, iHdrID) == false) goto ExitFunction;

                //save Tax Details
                if (bSaveGroupTaxDetails(objDB, sDealerCode, dtGrTaxDet, iHdrID) == false) goto ExitFunction;

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

        public DataSet GetOA(int OAId, string OAType, int DealerID, int iCustID) //change for REMAN PO
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_OA", OAId, OAType, DealerID, iCustID);
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

        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet, int iCustID, int iHoBrID, string sCustTaxTag, string sIS_GST, string sGSTOAType)
        {
            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesOAPartDetailsFromExcelSheet", sFileName, iDealerId, iCustID, iHoBrID, sCustTaxTag, sIS_GST, sGSTOAType);
                }

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        private bool bSaveUploadedPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        {
            bool bSaveRecord = false;
            string bNewTable = "Y";

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertSparesOAPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
                    bNewTable = "N";
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

        public string GenerateOANo(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "OA";

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

    }
}
