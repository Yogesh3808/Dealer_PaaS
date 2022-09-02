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
    /// Summary description for clsRFQ
    /// </summary>
    public class clsRFQ
    {
        public clsRFQ()
        {

        }

        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["RFQ_Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VRFQ";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SRFQ";
            }
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);                    
                    dtHdr.Rows[0]["RFQ_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_RFQHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFQ_No"], dtHdr.Rows[0]["RFQ_Date"], dtHdr.Rows[0]["RFQ_Model_Part"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["RFQ_Confirm"], dtHdr.Rows[0]["RFQ_Cancel"], dtHdr.Rows[0]["Remarks"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["MPG_ID"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_RFQHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFQ_No"], dtHdr.Rows[0]["RFQ_Date"], dtHdr.Rows[0]["RFQ_Model_Part"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["RFQ_Confirm"], dtHdr.Rows[0]["RFQ_Cancel"], dtHdr.Rows[0]["Remarks"], dtHdr.Rows[0]["Chassis_No"], dtHdr.Rows[0]["MPG_ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //To Save Model Record
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtDetFile)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (bSaveDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                // Save RFQ File Details
                if (bSaveRFQFileAttachDetails(objDB, dtDetFile, iHdrID) == false) goto ExitFunction;

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
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["New_Description"].ToString() != "")
                        {
                            objDB.ExecuteStoredProcedure("SP_RFQDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["New_Description"], dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["RequestType"], dtDet.Rows[iRowCnt]["Details"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["Process_Accept"], dtDet.Rows[iRowCnt]["Process_Confirm"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_RFQDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        // Save Attached File Attached Details
        private bool bSaveRFQFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_RFQ_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["User_ID"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_RFQ_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["User_ID"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_RFQ_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        /// <summary>    
        /// To Get RFQ of all types    
        /// </summary>    
        /// <param name="sRFQType"> sRFQType 'All','Confirm','Max', 'New' </param>
        /// <param name="RFQ_Model_Part"> "M' Model, "P" Part else ""</param>

        public DataSet GetRFQ(int iRFQID, string sRFQType, string sRFP_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_RFQ", iRFQID, sRFQType, sRFP_Model_Part, iDealerID);
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
        ///  To Update RFQ Process Status
        /// </summary>
        /// <param name="iRFQID"></param>
        /// <param name="sProcessFlag"></param>
        /// <param name="dtDet"></param>
        /// <param name="sRFQ_Model_Part"></param>
        /// <returns></returns>

        public bool bUpdateRFQProcessFlag(int iRFQID, string sProcessFlag, string sRemarks, string sFertCode, DataTable dtDet)
        {
            bool bUpdateRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_RFQHdr_UpdateProcessFlag", iRFQID, sProcessFlag, sRemarks, sFertCode);

                bUpdateRecord = bSaveDetails(objDB, dtDet, iRFQID);
                if (bUpdateRecord == true)
                {
                    objDB.CommitTransaction();
                    bUpdateRecord = true;
                }
                else
                {
                    objDB.RollbackTransaction();
                    bUpdateRecord = false;
                }
                return bUpdateRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bUpdateRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

    }
}
