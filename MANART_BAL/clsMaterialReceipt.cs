using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsMaterialReceipt
    /// </summary>
    public class clsMaterialReceipt
    {
        public clsMaterialReceipt()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, DataTable dtReceiptTaxDetails, ref int iHdrID)
        {
            string sDocName = "MRT";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {

                    int iDistributorId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Distributor_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDistributorId);
                    dtHdr.Rows[0]["MatReceipt_No"] = Func.Convert.sConvertToString(GenerateReceipt(sDealerCode, iDistributorId));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesMatReceiptHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["MatReceipt_No"],
                        dtHdr.Rows[0]["MatReceipt_Date"], dtHdr.Rows[0]["User_ID"], dtHdr.Rows[0]["Distributor_ID"], dtHdr.Rows[0]["DMSInv_No"],
                        dtHdr.Rows[0]["DMSInv_Date"], dtHdr.Rows[0]["MRN_Date"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtReceiptTaxDetails.Rows[0]["net_tr_amt"], dtReceiptTaxDetails.Rows[0]["discount_amt"],
                        dtReceiptTaxDetails.Rows[0]["before_tax_amt"], dtReceiptTaxDetails.Rows[0]["mst_amt"],
                        dtReceiptTaxDetails.Rows[0]["cst_amt"], dtReceiptTaxDetails.Rows[0]["surcharge_amt"],
                        dtReceiptTaxDetails.Rows[0]["tot_amt"], dtReceiptTaxDetails.Rows[0]["pf_per"], dtReceiptTaxDetails.Rows[0]["pf_amt"],
                        dtReceiptTaxDetails.Rows[0]["other_per"], dtReceiptTaxDetails.Rows[0]["other_money"], dtReceiptTaxDetails.Rows[0]["Total"],
                        dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["Is_AutoReceipt"]
                        , dtHdr.Rows[0]["LR_No"], dtHdr.Rows[0]["LR_Date"], dtHdr.Rows[0]["Delivery_No"], dtReceiptTaxDetails.Rows[0]["discount_amt"],
                        dtReceiptTaxDetails.Rows[0]["Excise_Per"], dtReceiptTaxDetails.Rows[0]["Excise_Amt"], dtReceiptTaxDetails.Rows[0]["Insu_Per"], dtReceiptTaxDetails.Rows[0]["Insu_Amt"]
                        , dtHdr.Rows[0]["IS_PerAmt"], 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Is_Ch_Rpt"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDistributorId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesMatReceiptHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["MatReceipt_No"],
                        dtHdr.Rows[0]["MatReceipt_Date"], dtHdr.Rows[0]["User_ID"], dtHdr.Rows[0]["Distributor_ID"], dtHdr.Rows[0]["DMSInv_No"],
                        dtHdr.Rows[0]["DMSInv_Date"], dtHdr.Rows[0]["MRN_Date"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtReceiptTaxDetails.Rows[0]["net_tr_amt"], dtReceiptTaxDetails.Rows[0]["discount_amt"],
                        dtReceiptTaxDetails.Rows[0]["before_tax_amt"], dtReceiptTaxDetails.Rows[0]["mst_amt"],
                        dtReceiptTaxDetails.Rows[0]["cst_amt"], dtReceiptTaxDetails.Rows[0]["surcharge_amt"],
                        dtReceiptTaxDetails.Rows[0]["tot_amt"], dtReceiptTaxDetails.Rows[0]["pf_per"], dtReceiptTaxDetails.Rows[0]["pf_amt"],
                        dtReceiptTaxDetails.Rows[0]["other_per"], dtReceiptTaxDetails.Rows[0]["other_money"], dtReceiptTaxDetails.Rows[0]["Total"],
                        dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["Is_AutoReceipt"]
                        , dtHdr.Rows[0]["LR_No"], dtHdr.Rows[0]["LR_Date"], dtHdr.Rows[0]["Delivery_No"], dtReceiptTaxDetails.Rows[0]["discount_amt"],
                        dtReceiptTaxDetails.Rows[0]["Excise_Per"], dtReceiptTaxDetails.Rows[0]["Excise_Amt"], dtReceiptTaxDetails.Rows[0]["Insu_Per"],
                        dtReceiptTaxDetails.Rows[0]["Insu_Amt"], dtHdr.Rows[0]["IS_PerAmt"], 0.00, dtHdr.Rows[0]["DocGST"], dtHdr.Rows[0]["Is_Ch_Rpt"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSavePartDetails(clsDB objDB, DataTable dtHdr, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            string sIsConfirm = "N";
            try
            {
                //@ID, @OA_HDR_ID, @Part_ID, @Qty, @Rate, @bal_qty, @disount_per, discount_amt, @disc_rate, @Total 
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_SparesMatReceiptDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["PO_Det_ID"],
                                dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Bal_PO_Qty"], dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["Bill_Qty"],
                                dtDet.Rows[iRowCnt]["Recv_Qty"], dtDet.Rows[iRowCnt]["Disc_Per"], dtDet.Rows[iRowCnt]["Accept_Rate"], dtDet.Rows[iRowCnt]["Total"],
                                dtDet.Rows[iRowCnt]["MRP_Rate"], dtDet.Rows[iRowCnt]["Ass_Value"], dtDet.Rows[iRowCnt]["Tax_Per"], dtDet.Rows[iRowCnt]["PO_No"],
                                dtDet.Rows[iRowCnt]["PartTaxID"], dtDet.Rows[iRowCnt]["Descripancy_YN"], dtDet.Rows[iRowCnt]["Shortage_Qty"],
                                dtDet.Rows[iRowCnt]["Excess_Qty"], dtDet.Rows[iRowCnt]["Damage_Qty"], dtDet.Rows[iRowCnt]["Man_Defect_Qty"],
                                dtDet.Rows[iRowCnt]["Wrong_Supply_Qty"], dtDet.Rows[iRowCnt]["Retain_YN"], dtDet.Rows[iRowCnt]["Wrg_Part_ID"],
                                dtDet.Rows[iRowCnt]["Price"], dtDet.Rows[iRowCnt]["Inv_Det_ID"], dtDet.Rows[iRowCnt]["SAP_Order_No"]
                                , dtDet.Rows[iRowCnt]["Tax_Amt"], dtDet.Rows[iRowCnt]["Tax1_Code"], dtDet.Rows[iRowCnt]["Tax1_Per"]
                                , dtDet.Rows[iRowCnt]["Tax1_Amt"], dtDet.Rows[iRowCnt]["BFRGST"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SparesMatReceiptDet_Delete", iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["PO_Det_ID"]);
                    }
                }
                // Changed by Vikram on 16.07.2016
                // After Save Part details Insert Part qty in Stock
                if (iHdrID != 0)
                {
                    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]);
                    if (sIsConfirm == "Y")
                        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "MReceipt", dtHdr.Rows[0]["ID"]);
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
                objDB.ExecuteStoredProcedure("SP_SparesMatReceiptDetTax", iHdrID);
                for (int iRowCnt = 0; iRowCnt < dtGrTaxDet.Rows.Count; iRowCnt++)
                {
                    if (dtGrTaxDet.Rows[iRowCnt]["group_code"].ToString() != "0" && Func.Convert.dConvertToDouble(dtGrTaxDet.Rows[iRowCnt]["net_inv_amt"].ToString()) > 0)
                    {
                        //@ID int,@OA_HDR_ID int, @group_code varchar(2), @net_inv_amt money, @discount_per money,@discount_amt money,@Tax_Code int,
                        //@tax_amt money,@tax1_code int,@tax1_amt money,@tax2_code int,@tax2_amt money,@Total money

                        objDB.ExecuteStoredProcedure("SP_SparesMatReceiptDetTax_Save", dtGrTaxDet.Rows[iRowCnt]["ID"], iHdrID, dtGrTaxDet.Rows[iRowCnt]["group_code"],
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

        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtGrTaxDet, DataTable dtReceiptTaxDetails, ref int iHdrID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, dtReceiptTaxDetails, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, dtHdr, dtDet, iHdrID) == false) goto ExitFunction;

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


        public DataSet GetMatReceipt(int MatReceiptID, string MatReceiptType, int UserID, int iDealerID, string sInvNo, int SupplierID, string Is_AutoReceipt, string sDeliveryNo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartsMatReceipt", MatReceiptID, MatReceiptType, UserID, iDealerID, sInvNo, SupplierID, Is_AutoReceipt, sDeliveryNo);
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
        public DataSet GetMatReceipt1(int MatReceiptID, string MatReceiptType, int iDealerID, int iDistributorID, string sInvNo, int SupplierID, string Is_Distributor, string Is_AutoReceipt)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_SparesInvoiceDts_Get", MatReceiptID, MatReceiptType, iDealerID, iDistributorID, sInvNo, SupplierID, Is_Distributor, Is_AutoReceipt);
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
        public DataTable GetInvoice(int iDealerId,string sSupplierType)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_DealerInvoice_Get", iDealerId,sSupplierType);
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
        public DataTable GetPartForReceiptDetails(string sPartIds, int iUserID, int iDistSupplierID, int iMatReceiptHdrID, string Is_distributor, string Is_AutoReceipt)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_PartForReceiptDetails_Get", sPartIds, iUserID, iDistSupplierID, iMatReceiptHdrID, Is_distributor, Is_AutoReceipt);
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
        public string GenerateReceipt(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "MRT";
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
                {
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                }
                return sDocNo;
            }
            catch
            {
                return "0";
            }
        }

        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet, string Is_Distributor, int UserId)
        {


            DataSet ds = null;

            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportMReceiptPartDetailsFromExcelSheet", sFileName, iDealerId, Is_Distributor, UserId);
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
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertMReceiptPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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
    }
}
