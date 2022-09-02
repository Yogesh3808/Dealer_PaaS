using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSparesInvoice
    /// </summary>
    public class clsSparesInvoice
    {
        String sPONo = "";
        public clsSparesInvoice()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtInvTaxDetails, ref int iHdrID)
        {
            string sDocName = "INV";

            //  sDocName = "DO";

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sPONo = Func.Convert.sConvertToString(dtHdr.Rows[0]["Inv_no"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesInvHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inv_no"], dtHdr.Rows[0]["Inv_Date"], dtHdr.Rows[0]["Inv_type"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Inv_close"], dtHdr.Rows[0]["Inv_canc"], dtHdr.Rows[0]["Inv_com"], dtHdr.Rows[0]["Inv_lock"], dtInvTaxDetails.Rows[0]["net_tr_amt"], dtHdr.Rows[0]["reference"], dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["Inv_validity_date"], dtInvTaxDetails.Rows[0]["discount_amt"], dtInvTaxDetails.Rows[0]["before_tax_amt"], dtInvTaxDetails.Rows[0]["mst_amt"], dtInvTaxDetails.Rows[0]["cst_amt"], dtInvTaxDetails.Rows[0]["surcharge_amt"], dtInvTaxDetails.Rows[0]["tot_amt"], dtInvTaxDetails.Rows[0]["pf_per"], dtInvTaxDetails.Rows[0]["pf_amt"], dtInvTaxDetails.Rows[0]["other_per"], dtInvTaxDetails.Rows[0]["other_money"], dtInvTaxDetails.Rows[0]["Inv_tot"], dtHdr.Rows[0]["Scheme"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesInvHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inv_no"], dtHdr.Rows[0]["Inv_Date"], dtHdr.Rows[0]["Inv_type"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Inv_close"], dtHdr.Rows[0]["Inv_canc"], dtHdr.Rows[0]["Inv_com"], dtHdr.Rows[0]["Inv_lock"], dtInvTaxDetails.Rows[0]["net_tr_amt"], dtHdr.Rows[0]["reference"], dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["Inv_validity_date"], dtInvTaxDetails.Rows[0]["discount_amt"], dtInvTaxDetails.Rows[0]["before_tax_amt"], dtInvTaxDetails.Rows[0]["mst_amt"], dtInvTaxDetails.Rows[0]["cst_amt"], dtInvTaxDetails.Rows[0]["surcharge_amt"], dtInvTaxDetails.Rows[0]["tot_amt"], dtInvTaxDetails.Rows[0]["pf_per"], dtInvTaxDetails.Rows[0]["pf_amt"], dtInvTaxDetails.Rows[0]["other_per"], dtInvTaxDetails.Rows[0]["other_money"], dtInvTaxDetails.Rows[0]["Inv_tot"], dtHdr.Rows[0]["Scheme"]);
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
                //@ID, @Inv_HDR_ID, @Part_ID, @Qty, @Rate, @bal_qty, @disount_per, discount_amt, @disc_rate, @Total 
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_InvDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["OA_Det_ID"], dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["bal_qty"], dtDet.Rows[iRowCnt]["discount_per"], dtDet.Rows[iRowCnt]["discount_amt"], dtDet.Rows[iRowCnt]["disc_rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["PartTaxID"]);
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

        private bool bSaveGroupTaxDetails(clsDB objDB, string sDealerCode, DataTable dtGrTaxDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_InvDet_Tax", iHdrID);

                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0" && Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0)
                    {
                        //@ID int,@Inv_HDR_ID int, @group_code varchar(2), @net_inv_amt money, @discount_per money,@discount_amt money,@Tax_Code int,
                        //@tax_amt money,@tax1_code int,@tax1_amt money,@tax2_code int,@tax2_amt money,@Total money

                        objDB.ExecuteStoredProcedure("SP_InvDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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

        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtInvTaxDetails)
        {
            int iHdrID = 0;
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

        public DataSet GetInv(int InvId, string InvType, int DealerID, int InvTypeID) //change for REMAN PO
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Inv", InvId, InvType, DealerID);
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
    }
}
