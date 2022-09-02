using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsPreshipment
    /// </summary>
    public class clsPackingList
    {
        public clsPackingList()
        {
        }
        // To Save Preshipment Header record
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, string sIsPartUpload, string sIsBoxUpload)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VPCK";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SPCK";
            }
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {                   
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Packing_List_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_PackingListHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Packing_List_No"], dtHdr.Rows[0]["Packing_List_Date"], dtHdr.Rows[0]["Preshipment_ID"], dtHdr.Rows[0]["Postshipment_ID"], dtHdr.Rows[0]["No_Of_Boxes"], dtHdr.Rows[0]["Net_Weight"], dtHdr.Rows[0]["Gross_Weight"], dtHdr.Rows[0]["Packing_Confirm"], sIsPartUpload, sIsBoxUpload, dtHdr.Rows[0]["Packing_List_RefNo"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_PackingListHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Packing_List_No"], dtHdr.Rows[0]["Packing_List_Date"], dtHdr.Rows[0]["Preshipment_ID"], dtHdr.Rows[0]["Postshipment_ID"], dtHdr.Rows[0]["No_Of_Boxes"], dtHdr.Rows[0]["Net_Weight"], dtHdr.Rows[0]["Gross_Weight"], dtHdr.Rows[0]["Packing_Confirm"], sIsPartUpload, sIsBoxUpload, dtHdr.Rows[0]["Packing_List_RefNo"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //To Save Record
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtBoxDetails, string sIsPartUpload, string sIsBoxUpload)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                // Save Header
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID, sIsPartUpload, sIsBoxUpload) == false) goto ExitFunction;

                // Save Details 
                if (bSaveDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                //save Box Details for Parts
                if (dtBoxDetails != null)
                {
                    if (bSaveBoxDetails(objDB, dtBoxDetails, iHdrID) == false) goto ExitFunction;
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
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        private bool bSaveDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iPackingListDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iPackingListDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                    if (iPackingListDetID == 0)
                    {
                        iPackingListDetID = objDB.ExecuteStoredProcedure("SP_PackingListDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Preshipment_Det_ID"], dtDet.Rows[iRowCnt]["Box_No"], dtDet.Rows[iRowCnt]["Net_Weight"], dtDet.Rows[iRowCnt]["Gross_Weight"]);
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_PackingListDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Preshipment_Det_ID"], dtDet.Rows[iRowCnt]["Box_No"], dtDet.Rows[iRowCnt]["Net_Weight"], dtDet.Rows[iRowCnt]["Gross_Weight"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //To Save Box Details
        private bool bSaveBoxDetails(clsDB objDB, DataTable dtBoxDetails, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iPackingListDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtBoxDetails.Rows.Count; iRowCnt++)
                {
                    iPackingListDetID = Func.Convert.iConvertToInt(dtBoxDetails.Rows[iRowCnt]["ID"]);
                    if (iPackingListDetID == 0)
                    {
                        iPackingListDetID = objDB.ExecuteStoredProcedure("SP_PackingListBoxDet_Save", dtBoxDetails.Rows[iRowCnt]["ID"], iHdrID, dtBoxDetails.Rows[iRowCnt]["SLength"], dtBoxDetails.Rows[iRowCnt]["SWidth"], dtBoxDetails.Rows[iRowCnt]["SHeight"], dtBoxDetails.Rows[iRowCnt]["Volume"], dtBoxDetails.Rows[iRowCnt]["Net_Weight"], dtBoxDetails.Rows[iRowCnt]["Gross_Weight"]);
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_PackingListBoxDet_Save", dtBoxDetails.Rows[iRowCnt]["ID"], iHdrID, dtBoxDetails.Rows[iRowCnt]["SLength"], dtBoxDetails.Rows[iRowCnt]["SWidth"], dtBoxDetails.Rows[iRowCnt]["SHeight"], dtBoxDetails.Rows[iRowCnt]["Volume"], dtBoxDetails.Rows[iRowCnt]["Net_Weight"], dtBoxDetails.Rows[iRowCnt]["Gross_Weight"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        /// <summary>    
        /// PreshipmentId else '0' 
        /// sPreshipmentType 'All' all Preshipment HDR +Det,'Confirm' get Confirm Preshipment ,'Max' Max Record, 'LC' 
        /// Preshipment_Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        public DataSet GetPackingList(int iPackingListID, string sPackingListType, string sModel_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPackingList", iPackingListID, sPackingListType, sModel_Part, iDealerID);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        /// <summary>
        /// To Confirm the Preshipment
        /// </summary>
        /// <param name="iProformaID">Id Of the Preshipment</param>
        public void ConfirmPreshipment(int iPreshipmentID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetDataset("SP_PreshipmentHdr_Confirm", iPreshipmentID);
                objDB.CommitTransaction();
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet, string sPreshipment_HDR_ID, string sDocName)
        {
            DataSet ds = null;

            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportPackingListDetailsFromExcelSheet", sPreshipment_HDR_ID, sDocName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        private bool bSaveUploadedPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        {
            //saveVehicleInDetails
            int iRowCnt;
            string bNewTable = "Y";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertPartDetailsFromExcelSheetForPackingList", dtDet.Rows[iRowCnt]["Part No"], dtDet.Rows[iRowCnt]["Box No"], bNewTable);
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


        public DataSet UploadBoxWtDetailsAndGetBoxWtDetails(string sFileName, int iDealerId, DataTable dtDet, string sPreshipment_HDR_ID, string sDocName)
        {
            DataSet ds = null;

            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedBoxDtlsDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportPackingListBoxDetailsFromExcelSheet", sPreshipment_HDR_ID, sDocName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        private bool bSaveUploadedBoxDtlsDetails(string sFileName, int iDealerId, DataTable dtDet)
        {
            //saveVehicleInDetails
            int iRowCnt;
            string bNewTable = "Y";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertBoxDetailsFromExcelSheetForPackingList", Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Box No"]), Func.Convert.dConvertToDouble(dtDet.Rows[iRowCnt]["Length"]), Func.Convert.dConvertToDouble(dtDet.Rows[iRowCnt]["Width"]), Func.Convert.dConvertToDouble(dtDet.Rows[iRowCnt]["Height"]), Func.Convert.dConvertToDouble(dtDet.Rows[iRowCnt]["Net Wt"]), Func.Convert.dConvertToDouble(dtDet.Rows[iRowCnt]["Gross Wt"]), bNewTable);
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
