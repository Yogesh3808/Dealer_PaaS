using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using MANART_DAL;

namespace MANART_BAL
{
   public class clsStkTransferReceipt
    {
        String sSTReceiptNo = "";
        public clsStkTransferReceipt()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveStkReceiptHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "STRPT";

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                   
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sSTReceiptNo = Func.Convert.sConvertToString(dtHdr.Rows[0]["Receipt_No"]);
                    dtHdr.Rows[0]["Receipt_No"] = Func.Convert.sConvertToString(GenerateStkReceiptNo(sDealerCode, iDealerId));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SPReceiptHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SupplierID"], dtHdr.Rows[0]["Receipt_No"], dtHdr.Rows[0]["Receipt_Date"], dtHdr.Rows[0]["Challan_No"], dtHdr.Rows[0]["Receipt_Amt"], dtHdr.Rows[0]["Receipt_canc"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["Receipt_Confirm"], dtHdr.Rows[0]["ISDocGST"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SPReceiptHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SupplierID"], dtHdr.Rows[0]["Receipt_No"], dtHdr.Rows[0]["Receipt_Date"], dtHdr.Rows[0]["Challan_No"], dtHdr.Rows[0]["Receipt_Amt"], dtHdr.Rows[0]["Receipt_canc"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["Receipt_Confirm"], dtHdr.Rows[0]["ISDocGST"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool bSavePartStkReciptDetails(clsDB objDB, DataTable dtDet, DataTable dtHdr, int iHdrID)
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
                            objDB.ExecuteStoredProcedure("SP_SPReceiptDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["Recv_Qty"], dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["BFRGST"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_SPReceiptDetails_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                if (iHdrID != 0)
                {
                    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Receipt_Confirm"]);
                    if (sIsConfirm == "Y")
                        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "StkTransferReceipt", iHdrID);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        public bool bSaveRecord(string sDealerCode, int iDealerID, DataTable dtHdr, DataTable dtDet, ref int iHdrID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveStkReceiptHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartStkReciptDetails(objDB, dtDet, dtHdr, iHdrID) == false) goto ExitFunction;

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
        public string GenerateStkReceiptNo(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "STRPT";

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
        public DataSet GetStockTrnReceipt(int iID, string sSelType, int iDealerID, int iChallanID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_SPReceipt", iID, sSelType, iDealerID, iChallanID);
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
