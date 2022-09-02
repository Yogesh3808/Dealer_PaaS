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
    /// Summary description for clsRFP
    /// </summary>
    public class clsRFP
    {
        public clsRFP()
        {

        }

        private bool bSaveHeader(clsDB objDB, string sDealerCode, string sIsPartUpload, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["RFP_Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VRFP";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SRFP";
            }
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["RFP_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    //Sujata 14012011
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_RFPHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFP_No"], dtHdr.Rows[0]["RFP_Date"], dtHdr.Rows[0]["RFP_Model_Part"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["RFP_Confirm"], dtHdr.Rows[0]["RFP_Cancel"], dtHdr.Rows[0]["Remarks"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_RFPHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFP_No"], dtHdr.Rows[0]["RFP_Date"], dtHdr.Rows[0]["RFP_Model_Part"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["RFP_Confirm"], dtHdr.Rows[0]["RFP_Cancel"], dtHdr.Rows[0]["Remarks"], dtHdr.Rows[0]["Fumigation_Required"], sIsPartUpload);
                    //Sujata 14012011
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_RFPHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["RFP_No"], dtHdr.Rows[0]["RFP_Date"], dtHdr.Rows[0]["RFP_Model_Part"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["RFP_Confirm"], dtHdr.Rows[0]["RFP_Cancel"], dtHdr.Rows[0]["Remarks"], dtHdr.Rows[0]["Fumigation_Required"], sIsPartUpload);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //To Save Model Record
        public bool bSaveRecordWithModel(ref int iHdrID, string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, "", dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (bSaveModelDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
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
        private bool bSaveModelDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Model_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_RFPDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["NewModel_Des"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Process_Accept"], dtDet.Rows[iRowCnt]["Process_Confirm"], 0);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        //To Save Part Record
        public bool bSaveRecordWithPart(ref int iHdrID, string sDealerCode, string sIsPartUpload, DataTable dtHdr, DataTable dtDet)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, sIsPartUpload, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                // objDB.CommitTransaction();
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
        private bool bSavePartDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_No_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_RFPDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_No_ID"], 0, "", dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["FOBRate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Process_Accept"], dtDet.Rows[iRowCnt]["Process_Confirm"], dtDet.Rows[iRowCnt]["Excel_Qty"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
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
        ///  To Upload the file and GetPartDetails
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="iDealerId"></param>
        /// <param name="iCountNotUpload"></param>
        /// <returns></returns>
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        {

            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                // Commented by Shyamal on 05042012 BeginTranasaction not needed here
                //objDB.BeginTranasaction();
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {

                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportPartDetailsFromExcelSheet", sFileName, iDealerId);

                }
                // Commented by Shyamal on 05042012 RollbackTransaction not needed here
                //else
                //{
                //    objDB.RollbackTransaction();
                //}
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
                    objDB.ExecuteStoredProcedure("SP_InsertPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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


        //Get RFP Details

        /// <summary>    
        /// To Get RFP of all types    
        /// </summary>    
        /// <param name="sRFPType"> sRFPType 'All','Confirm','Max', 'New' </param>
        /// <param name="RFP_Model_Part"> "M' Model, "P" Part else ""</param>

        public DataSet GetRFP(int iRFPID, string sRFPType, string sRFP_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_RFP", iRFPID, sRFPType, sRFP_Model_Part, iDealerID);
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
        ///  To Update RFP Process Status
        /// </summary>
        /// <param name="iRFPID"></param>
        /// <param name="sProcessFlag"></param>
        /// <param name="dtDet"></param>
        /// <param name="sRFP_Model_Part"></param>
        /// <returns></returns>

        public bool bUpdateRFPProcessFlag(int iRFPID, string sProcessFlag, DataTable dtDet, string sRFP_Model_Part)
        {
            bool bUpdateRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_RFPHdr_UpdateProcessFlag", iRFPID, sProcessFlag);
                if (sRFP_Model_Part == "M") // Save Model Details
                {
                    bUpdateRecord = bSaveModelDetails(objDB, dtDet, iRFPID);
                }
                else if (sRFP_Model_Part == "P") // Save Part Details
                {
                    bUpdateRecord = bSavePartDetails(objDB, dtDet, iRFPID);
                }
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
        // Megha 22082013
        public bool bSaveNotUploadPartDetails(DataTable dtDet, int iRFPID, string hdnIsPartNotUpload)
        {
            //saveVehicleInDetails
            int iRowCnt;
            string bNewTable = "Y";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //To Delete
                objDB.ExecuteStoredProcedure("SP_RFP_NotUploadedPartsDetail_Del", iRFPID);

                objDB.BeginTranasaction();

                for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_NotInsertPartDetailsFromExcelSheet", dtDet.Rows[iRowCnt]["PartNo"], iRFPID, hdnIsPartNotUpload);
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
        public DataSet GetNotUploadPartDetails(int iRFPID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("Get_RFP_NotUplaodPartDetails", iRFPID);
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
        // Megha 22082013

    }
}
