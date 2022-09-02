using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MANART_DAL;
using System.Data;

namespace MANART_BAL
{
    public class clsPartsRFP
    {
        string sDocName = "";
        public clsPartsRFP() { }
        //Save Header Details
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iDealerID)
        {
            sDocName = "RFP";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0 && Func.Convert.sConvertToString(dtHdr.Rows[0]["TrNo"]) != "")
                {
                    string sFinYear = Func.sGetFinancialYear(iDealerID);
                    clsCommon ObjCom = new clsCommon();
                    dtHdr.Rows[0]["RFP_No"] = ObjCom.GenerateDocNo(sDealerCode, iDealerID, sDocName, "");

                    iHdrID = objDB.ExecuteStoredProcedure("SP_PartsRFPHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFP_No"], dtHdr.Rows[0]["RFP_Date"],
                        dtHdr.Rows[0]["CreatedBy"], dtHdr.Rows[0]["UserId"], iDealerID, dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["total"], dtHdr.Rows[0]["Total_Qty"], dtHdr.Rows[0]["Total_Line_Items"], dtHdr.Rows[0]["Supplier_ID"],
                        dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["Remark"] 
                       );
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerID);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PartsRFPHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFP_No"], dtHdr.Rows[0]["RFP_Date"],
                        dtHdr.Rows[0]["CreatedBy"], dtHdr.Rows[0]["UserId"], iDealerID, dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"],
                        dtHdr.Rows[0]["total"], dtHdr.Rows[0]["Total_Qty"], dtHdr.Rows[0]["Total_Line_Items"], dtHdr.Rows[0]["Supplier_ID"],
                        dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["Remark"]);
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
                    if (dtDet.Rows[iRowCnt]["DocStatus"].ToString() != "D" && dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                    {
                            // Insert Record
                            objDB.ExecuteStoredProcedure("SP_PartsRFPDtl_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"],
                                dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["DocStatus"]);
                    }
                    else if (dtDet.Rows[iRowCnt]["DocStatus"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_PartsRFPDtl_Delete", dtDet.Rows[iRowCnt]["ID"]);
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
                objDB.BeginTranasaction();
                //SaveHeader
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID, iDealerID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, sDealerCode, dtDet, iHdrID) == false) goto ExitFunction;
                if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"].ToString()) == "Y")
                {
                    objDB.ExecuteStoredProcedureAndGetObject("SP_POSendTODMS", iHdrID, iDealerID, dtHdr.Rows[0]["RFP_No"], iUserID);
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

        public DataSet GetPartsRFP(int RFPID, string DocType, int DealerID, int iMenuID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartsRFP", RFPID, DocType, DealerID, iMenuID);
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

        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, int iSupplierId, DataTable dtDet, int UserId)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportPartsRFPPartDetailsFromExcelSheet", sFileName, iDealerId, iSupplierId, UserId);
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
                    objDB.ExecuteStoredProcedure("SP_InsertPartsRFPPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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
