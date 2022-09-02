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
    /// Summary description for clsORF
    /// </summary>

    public class clsORF
    {
        public clsORF()
        {
        }
        // To Save ORF Header record
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VORF";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SORF";
            }
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {                    
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["ORF_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_ORFIndentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["ORF_No"], dtHdr.Rows[0]["ORF_Date"], dtHdr.Rows[0]["Proforma_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["PricingApprovalReq_YN"], dtHdr.Rows[0]["PricingApprovalAtt_YN"], dtHdr.Rows[0]["Special_Insturtions"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["LC_No"], dtHdr.Rows[0]["LC_Date"], dtHdr.Rows[0]["ORF_Confirm"], dtHdr.Rows[0]["LC_Expt_Date"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["IsManualLC_Exist"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_ORFIndentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["ORF_No"], dtHdr.Rows[0]["ORF_Date"], dtHdr.Rows[0]["Proforma_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["PricingApprovalReq_YN"], dtHdr.Rows[0]["PricingApprovalAtt_YN"], dtHdr.Rows[0]["Special_Insturtions"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["LC_No"], dtHdr.Rows[0]["LC_Date"], dtHdr.Rows[0]["ORF_Confirm"], dtHdr.Rows[0]["LC_Expt_Date"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["IsManualLC_Exist"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //To Save Model Record
        public bool bSaveRecordWithModel(string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

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
            int iORFDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iORFDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                    if (iORFDetID == 0)
                    {
                        //Sujata 01072013_Begin
                        //iORFDetID = objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"]);
                        iORFDetID = objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["Colour"], dtDet.Rows[iRowCnt]["Plant"], dtDet.Rows[iRowCnt]["Remark"]);
                        //Sujata 01072013_End
                    }
                    else
                    {
                        //Sujata 01072013_Begin
                        //objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"]);
                        objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["Colour"], dtDet.Rows[iRowCnt]["Plant"], dtDet.Rows[iRowCnt]["Remark"]);
                        //Sujata 01072013_End
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
        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;
                // Save Details
                if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

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
            int iORFDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iORFDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                    if (iORFDetID == 0)
                    {
                        //Sujata 01072013_Begin
                        //iORFDetID = objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], dtDet.Rows[iRowCnt]["Part_No_ID"], 0, dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"]);
                        iORFDetID = objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], dtDet.Rows[iRowCnt]["Part_No_ID"], 0, dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"], "", "", "");
                        //Sujata 01072013_End
                    }
                    else
                    {
                        //Sujata 01072013_Begin
                        //objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], dtDet.Rows[iRowCnt]["Part_No_ID"], 0, dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"]);
                        objDB.ExecuteStoredProcedure("SP_ORFIndentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Proforma_Det_ID"], dtDet.Rows[iRowCnt]["Part_No_ID"], 0, dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Shipment_Date"], "", "", "");
                        //Sujata 01072013_End
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
        /// ORFId else '0' 
        /// sORFType 'All' all ORF HDR +Det,'Confirm' get Confirm ORF ,'Max' Max Record, 'LC' 
        /// ORF_Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        /// Sujata 15072013
        /// public DataSet GetORF(int iORFID, string sORFType, string sORF_Model_Part, int iDealerID)
        public DataSet GetORF(int iORFID, string sORFType, string sORF_Model_Part, int iDealerID, int ModelID, int iQty)
        /// Sujata 15072013
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetORF", iORFID, sORFType, sORF_Model_Part, iDealerID, ModelID, iQty);
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
        /// To Update Create and Indent No
        /// </summary>
        /// <param name="iDealerID"></param>
        /// <param name="dtHdr"></param>
        /// <returns></returns>
        public bool bUpdateIndentDetails(int iDealerID, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
                if (iHdrID == 0) return false;
                //Update Header 
                objDB.ExecuteStoredProcedureAndGetObject("SP_ORFIndent_Update", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Indent_No"], dtHdr.Rows[0]["Indent_Date"], dtHdr.Rows[0]["Indent_Confirm"], dtHdr.Rows[0]["Fumigation_Required"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["ShipingUnder_ID"], dtHdr.Rows[0]["Port_Loading_ID"], dtHdr.Rows[0]["Contenarisation_ID"], dtHdr.Rows[0]["Clearing_ID"]);

                // Save Details 
                if (sModel_Part == "M")
                {
                    if (bSaveModelDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                }
                else if (sModel_Part == "P")
                {
                    if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
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

        public DataTable GetSpareShortClosePO(string sDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtDetails = new DataTable();
                dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetSpareShortClosePO", sDealerID);
                if (dtDetails != null)
                {
                    return dtDetails;
                }
                else
                    return null;
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

        public bool bSaveSpareShortClose(string sID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (sID != "")
                {

                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_SpareShortClosePOSave", sID);
                    objDB.CommitTransaction();
                    bSaveRecord = true;
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

        //Sujata 01042012_Begin
        public DataSet GetORFPlantExportBalDetails(int iDocID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetORFPlantEXport", iDocID);
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
        //Sujata 01042012_End

        public DataSet GetIndentPreshipment(int iORFID, int iPreshipmentID, string sORFType, string sORF_Model_Part, int iDealerID, int ModelID, int iQty)
        /// Sujata 15072013
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPreshipmentEdit", iORFID, iPreshipmentID, sORFType, sORF_Model_Part, iDealerID, ModelID, iQty);
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
    }
}
