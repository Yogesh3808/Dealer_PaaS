using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSparePO
    /// </summary>
    public class clsSparePO
    {
        String sPONo = "";
        public clsSparePO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private bool bSaveHeader(clsDB objDB, DataTable dtHdr, string sDealerCode, ref int iHdrID)
        {
            string sDocName = "";

            int POType = Func.Convert.iConvertToInt(dtHdr.Rows[0]["PO_Type_ID"]);
            string Is_Distributor = Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Distributor"]);

            if (POType == 2)
                sDocName = "REGPO";
            else if (POType == 3)
                sDocName = "VORPO";
            else if (POType == 8)
                sDocName = "ACCPO";
            else if (POType == 11)
                sDocName = "LOCPO";
            else if (POType == 14)
                sDocName = "LSRE";
            else if (POType == 1)
                sDocName = "STKPO";
            else if (POType == 4)
                sDocName = "SPEPO";
            else if (POType == 5)
                sDocName = "FLTPO";
            else if (POType == 6)
                sDocName = "WRTPO";
            else if (POType == 7)
                sDocName = "ZREPO";

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                   
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    dtHdr.Rows[0]["PO_No"] = Func.Convert.sConvertToString(GeneratePO(sDealerCode, iDealerId, POType, ""));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"], dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"], dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["JobCard_HDR_ID"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"], dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"], dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["JobCard_HDR_ID"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool bSavePartDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_SparePODtl_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Status"], dtDet.Rows[iRowCnt]["JobDtlID"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SparePODtl_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        public bool bSaveRecordWithPart(string sDealerCode, int iDealerID, DataTable dtHdr, DataTable dtDet, ref int iHdrID, int iUserID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, dtHdr, sDealerCode, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                if (Func.Convert.sConvertToString(dtHdr.Rows[0]["PO_Confirm"].ToString()) == "Y")
                {
                    objDB.ExecuteStoredProcedureAndGetObject("SP_POSendTODMS", iHdrID, iDealerID, dtHdr.Rows[0]["PO_No"], iUserID);
                }
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
        public DataSet GetPO(int POId, string POType, int DealerID, string DistSupplier, int POTypeID) //change for REMAN PO
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_SparePO_Get", POId, POType, DealerID, DistSupplier, POTypeID);
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

        public DataSet GetPOType(int iID, int iSupType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPOType", iID, iSupType);
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
        public DataSet GetPreviousPOTypeDate(int iUserID, int iDealerID, int iSupplierID, int iPoTypeID, int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetEnablePreviousDateForSparePO", iUserID, iDealerID, iSupplierID, iPoTypeID, iID);
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
        public string GeneratePO(string sDealerCode, int iDealerID, int POType, string Is_Distributor)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear=="2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "";

                if (POType == 2)
                    sDocName = "REGPO";
                else if (POType == 3)
                    sDocName = "VORPO";
                else if (POType == 8)
                    sDocName = "ACCPO";
                else if (POType == 11)
                    sDocName = "LOCPO";
                else if (POType == 14)
                    sDocName = "LSRE";
                else if (POType == 1)
                    sDocName = "STKPO";
                else if (POType == 4)
                    sDocName = "SPEPO";
                else if (POType == 5)
                    sDocName = "FLTPO";
                else if (POType == 6)
                    sDocName = "WRTPO";
                else if (POType == 7)
                    sDocName = "ZREPO";

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                if (sFinYear == "2016" ){
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

                //if (sDealerCode != "")
                //    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
               //else
               //     sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                return sDocNo;
            }
            catch
            {
                return "0";
            }
        }
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, int iSupplierId, DataTable dtDet, string Is_Distributor, int UserId)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesPOPartDetailsFromExcelSheet", sFileName, iDealerId, iSupplierId, Is_Distributor, UserId);
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
                    objDB.ExecuteStoredProcedure("SP_InsertSparesPOPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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
        public bool bSaveShortClose(int iHdrID)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetObject("SP_SAVESparePOShortClose", iHdrID);
                bSaveRecord = true;
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

        public DataTable GetStockByPartNo(int iUserId, string PartNo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtPart;
                dtPart = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetStockByPartNo", iUserId, PartNo);
                return dtPart;
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
