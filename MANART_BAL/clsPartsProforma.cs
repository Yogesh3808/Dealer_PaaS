using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MANART_DAL;
using System.Data;

namespace MANART_BAL
{
    public class clsPartsProforma
    {
        string sDocName = "";
        public clsPartsProforma() { }

        #region Proforma Saving
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, int iDealerID)
        {
            sDocName = "PIA";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0 && Func.Convert.sConvertToString(dtHdr.Rows[0]["TrNo"]) != "")
                {
                    string sFinYear = Func.sGetFinancialYear(iDealerID);
                    clsCommon ObjCom = new clsCommon();
                    dtHdr.Rows[0]["Prof_No"] = ObjCom.GenerateDocNo(sDealerCode, iDealerID, sDocName, "");

                    iHdrID = objDB.ExecuteStoredProcedure("SP_PartsProfHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Prof_No"], dtHdr.Rows[0]["Prof_Date"],
                         dtHdr.Rows[0]["UserId"], iDealerID, dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Prof_Inv_no"], dtHdr.Rows[0]["Prof_Inv_Date"],
                         dtHdr.Rows[0]["Total"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["Remark"],
                         dtHdr.Rows[0]["Total_Qty"], dtHdr.Rows[0]["Total_Line_Items"]);
                    if (iHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerID);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PartsProfHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Prof_No"], dtHdr.Rows[0]["Prof_Date"],
                         dtHdr.Rows[0]["UserId"], iDealerID, dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Prof_Inv_no"], dtHdr.Rows[0]["Prof_Inv_Date"],
                         dtHdr.Rows[0]["Total"], dtHdr.Rows[0]["Is_Confirm"], dtHdr.Rows[0]["Is_Cancel"], dtHdr.Rows[0]["TrNo"], dtHdr.Rows[0]["Remark"],
                         dtHdr.Rows[0]["Total_Qty"], dtHdr.Rows[0]["Total_Line_Items"]);
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
                        objDB.ExecuteStoredProcedure("SP_PartsProfDtl_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["RFP_Det_ID"],
                            dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Prof_Qty"], dtDet.Rows[iRowCnt]["Acc_Qty"],
                                 dtDet.Rows[iRowCnt]["Rate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["RFP_No"], dtDet.Rows[iRowCnt]["SAP_Order_No"],
                                 dtDet.Rows[iRowCnt]["ProfInv_Det_ID"], dtDet.Rows[iRowCnt]["DocStatus"]);
                    }
                    else if (dtDet.Rows[iRowCnt]["DocStatus"].ToString() == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_PartsProfDtl_Delete", dtDet.Rows[iRowCnt]["ID"]);
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
                //if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"].ToString()) == "Y")
                //{
                //    objDB.ExecuteStoredProcedureAndGetObject("SP_POSendTODMS", iHdrID, iDealerID, dtHdr.Rows[0]["RFP_No"], iUserID);
                //}
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

        public DataSet GetPartsProformaInvoice(int RFPID, string DocType, int DealerID, string sSupplierID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartsProformaInvoice", RFPID, DocType, DealerID, sSupplierID);
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
        #endregion

        #region PO Creattion After Proforma Confirmation
        public DataSet GeneratePO(int iProfID, string sDocType, int iDealerID, string DistSupplier, int POTypeID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_SparePO_Get", iProfID, sDocType, iDealerID, DistSupplier, POTypeID);
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

        private bool bSaveHeaderForPO(clsDB objDB, DataTable dtHdr, string sDealerCode, ref int iPOHdrID, int iDealerId)
        {
            string sDocName = "RFPPO";
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    clsCommon ObjComm = new clsCommon();
                    dtHdr.Rows[0]["PO_No"] = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, iDealerId, sDocName, ""));

                    iPOHdrID = objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"],
                        dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"],
                        dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"],
                        dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["JobCard_HDR_ID"]);
                    if (iPOHdrID != 0)
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"], dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"], dtHdr.Rows[0]["Supplier_ID"], dtHdr.Rows[0]["Is_Distributor"], dtHdr.Rows[0]["JobCard_HDR_ID"]);
                    iPOHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool bSavePartDetailsForPO(clsDB objDB, DataTable dtDet, int iPOHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"].ToString() != "D" && dtDet.Rows[iRowCnt]["Status"].ToString() != "C" && dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_SparePODtl_Save", dtDet.Rows[iRowCnt]["ID"], iPOHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["Qty"],
                            dtDet.Rows[iRowCnt]["MRPRate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Status"], dtDet.Rows[iRowCnt]["JobDtlID"]);

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

        public bool bSaveRecordWithPartForRFPPO(string sDealerCode, int iDealerID, DataTable dtHdr, DataTable dtDet, ref int iPOHdrID, int iUserID)
        {
            iPOHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                if (bSaveHeaderForPO(objDB, dtHdr, sDealerCode, ref iPOHdrID, iDealerID) == false) goto ExitFunction;

                if (bSavePartDetailsForPO(objDB, dtDet, iPOHdrID) == false) goto ExitFunction;
                if (Func.Convert.sConvertToString(dtHdr.Rows[0]["PO_Confirm"].ToString()) == "Y")
                {
                    objDB.ExecuteStoredProcedureAndGetObject("SP_POSendTODMS", iPOHdrID, iDealerID, dtHdr.Rows[0]["PO_No"], 0000);
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
        #endregion

    }
}
