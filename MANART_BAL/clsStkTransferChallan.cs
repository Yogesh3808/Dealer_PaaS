using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    public class clsStkTransferChallan
    {
        String sStkChallanNo = "";
        public clsStkTransferChallan()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveStkTrnChHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string ChType = Func.Convert.sConvertToString(dtHdr.Rows[0]["ChType"]);
            string sDocName = "";
            if (ChType == "ST")
                sDocName = "ST";
            else if (ChType == "WO")
                sDocName = "WO";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sStkChallanNo = Func.Convert.sConvertToString(dtHdr.Rows[0]["Challan_no"]);
                    dtHdr.Rows[0]["Challan_no"] = Func.Convert.sConvertToString(GenerateStkChallan(sDealerCode, iDealerId, ChType));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SPChallanHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["ChType"],
                        dtHdr.Rows[0]["Challan_no"], dtHdr.Rows[0]["Challan_date"], dtHdr.Rows[0]["RefID"], dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["Ch_Amt"],
                        dtHdr.Rows[0]["Chal_canc"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["Chal_Confirm"]
                        , dtHdr.Rows[0]["ISCHGST"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SPChallanHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["CustID"], dtHdr.Rows[0]["ChType"], dtHdr.Rows[0]["Challan_no"],
                        dtHdr.Rows[0]["Challan_date"], dtHdr.Rows[0]["RefID"], dtHdr.Rows[0]["narration"], dtHdr.Rows[0]["Ch_Amt"], dtHdr.Rows[0]["Chal_canc"],
                        dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["HOBR_ID"], dtHdr.Rows[0]["Chal_Confirm"], dtHdr.Rows[0]["ISCHGST"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                    
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool bSavePartStkTrnChDetails(clsDB objDB, DataTable dtDet, DataTable dtHdr, int iHdrID)
        {
            bool bSaveRecord = false;
            //string sIsConfirm = "N";
            //int iRefDtlID;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_SPChallanDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], 
                                dtDet.Rows[iRowCnt]["RefDtlID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["Total"]
                                , dtDet.Rows[iRowCnt]["BFRGST"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SPChallanDetails_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                //if (iHdrID != 0)
                //{
                //    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Chal_Confirm"]);
                //    iRefDtlID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["RefID"]);
                //    if (sIsConfirm == "Y" && iRefDtlID == 0)
                //        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "StkTransferChallan", iHdrID);
                //}
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
                if (bSaveStkTrnChHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartStkTrnChDetails(objDB, dtDet, dtHdr, iHdrID) == false) goto ExitFunction;

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
        public string GenerateStkChallan(string sDealerCode, int iDealerID, string ChType)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "";
                if (ChType == "ST")
                    sDocName = "ST";
                else if (ChType == "WO")
                    sDocName = "WO";

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
        public DataSet GetStockTrnChallan(int iID, string sSelType, int iDealerID, int iCustID, int iMenuId)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_SPChallan", iID, sSelType, iDealerID, iCustID, iMenuId);
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
        public DataTable GetSysStock(string sPartIDs, int iUserID, string sSelType, int DealerID, int StkTrnID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_SysStock_Get", sPartIDs, iUserID, sSelType, DealerID, StkTrnID);
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
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, int iSupplierId, DataTable dtDet, string Is_Distributor, int UserId)
        {
            DataSet ds = null;
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                // Commented by Shyamal on 27032012
                //objDB.BeginTranasaction();
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportStockTransferPartDetailsFromExcelSheet", sFileName, iDealerId, UserId);
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
                    objDB.ExecuteStoredProcedure("SP_InsertStockTransferPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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
