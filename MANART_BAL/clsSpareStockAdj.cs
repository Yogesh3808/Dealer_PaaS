using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsSpareStockAdj
    /// </summary>
    public class clsSpareStockAdj
    {
        String sStockAdjNo = "";
        public clsSpareStockAdj()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveStkAdjHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "STKADJ";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {

                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sStockAdjNo = Func.Convert.sConvertToString(dtHdr.Rows[0]["StockAdj_No"]);
                    dtHdr.Rows[0]["StockAdj_No"] = Func.Convert.sConvertToString(GenerateStockAdj(sDealerCode, iDealerId));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesStockAdjHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["StockAdj_No"], dtHdr.Rows[0]["StockAdj_Date"],
                        dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"]
                        , dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["GSTType"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesStockAdjHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["StockAdj_No"], dtHdr.Rows[0]["StockAdj_Date"],
                        dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Reference"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"]
                        , dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["GSTType"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                objDB.RollbackTransaction();
                return false;
            }
        }
        private bool bSavePartStkAdjDetails(clsDB objDB, DataTable dtDet, DataTable dtHdr, int iHdrID)
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
                            objDB.ExecuteStoredProcedure("SP_SpareStockAdjDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"],
                                dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["Physical_Qty"], dtDet.Rows[iRowCnt]["Inward_Qty"], dtDet.Rows[iRowCnt]["Outward_Qty"],
                                dtDet.Rows[iRowCnt]["Reason"], dtDet.Rows[iRowCnt]["BFRGST"], dtDet.Rows[iRowCnt]["BFRGST_Stock"], dtDet.Rows[iRowCnt]["group_code"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SpareStockAdjDet_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                if (iHdrID != 0)
                {
                    sIsConfirm = Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]);
                    if (sIsConfirm == "Y")
                        objDB.ExecuteStoredProcedure("SP_SysStock_Save", "StockAdjustment", dtHdr.Rows[0]["ID"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bSaveRecord;
            }
            return bSaveRecord;
        }
        public bool bSaveRecord(string sDealerCode, int iDealerId, DataTable dtHdr, DataTable dtDet, ref int iHdrID)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveStkAdjHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartStkAdjDetails(objDB, dtDet, dtHdr, iHdrID) == false) goto ExitFunction;

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
        public DataSet GetStockAdj(int iID, string sSelType, int iDistributorID, int iUserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_SpareStockAdjDet_Get", iID, sSelType, iDistributorID);
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
        public string GenerateStockAdj(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                string sDocName = "STKADJ";
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
        public DataTable GetSysStock(string sPartIDs, int iUserID, string sSelType, int DistributorID, int StkAdjID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_SysStock_Get", sPartIDs, iUserID, sSelType, DistributorID, StkAdjID);
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
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, int iSupplierId, DataTable dtDet, string Is_GSTType, int UserId)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                //objDB.BeginTranasaction();
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportStockAdjPartDetailsFromExcelSheet", sFileName, iDealerId, UserId, Is_GSTType);
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
                    if (Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["PartNo"]) != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertStockAdjPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Physical_Qty"], dtDet.Rows[iRowCnt]["Reason"], bNewTable);
                        bNewTable = "N";
                    }
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
