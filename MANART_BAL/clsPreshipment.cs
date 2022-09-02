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
    public class clsPreshipment
    {
        public clsPreshipment()
        {
        }
        // To Save Preshipment Header record
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID, string PrevPreshipmentNo)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "ETBV";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "ETBS";
            }
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {

                    //string sFinYear = Func.sGetFinancialYear();
                    //int iDelearId =Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);

                    //Modified By Shyamal 18/01/2011
                    string sFinYear = DateTime.Now.Year.ToString().Substring(2, 2);
                    int iDelearId = 999;

                    //dtHdr.Rows[0]["Preshipment_Inv_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                    //  iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);

                    //iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo, dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"]);
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo, dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo, dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"], dtHdr.Rows[0]["DescriptionofGood"], dtHdr.Rows[0]["FreeText2"], dtHdr.Rows[0]["FreeText3"]);
                    //Megha 28062014
                    if (PrevPreshipmentNo == "")
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    //Megha 28062014
                    //sujata 03022011
                    //objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                    // objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                    // objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo, dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"]);
                    objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo, dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"], dtHdr.Rows[0]["DescriptionofGood"], dtHdr.Rows[0]["FreeText2"], dtHdr.Rows[0]["FreeText3"]);
                    //Megha 28062014
                    if (dtHdr.Rows[0]["Mail_SendYN"] == "Y")
                    {
                        objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Confirm", dtHdr.Rows[0]["ID"]);
                    }
                    //sujata 03022011
                    //objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool bSaveSpExportHeader(string sDealerCode, DataTable dtHdr)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            clsDB objDB = new clsDB();
            int iHdrID = 0;
            if (sModel_Part == "M")
            {
                sDocName = "ETBV";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "ETBS";
            }
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {
                    //Modified By Megha 30/06/2014
                    //string sFinYear = Func.sGetFinancialYear();
                    //int iDelearId =Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);

                    //Modified By Shyamal 18/01/2011
                    string sFinYear = DateTime.Now.Year.ToString().Substring(2, 2);
                    int iDelearId = 999;

                    //dtHdr.Rows[0]["Preshipment_Inv_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], "", dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"], dtHdr.Rows[0]["DescriptionofGood"], dtHdr.Rows[0]["FreeText2"], dtHdr.Rows[0]["FreeText3"]);
                    //                if (PrevPreshipmentNo == "")
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    //sujata 03022011
                    //objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                    objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Preshipment_Status"], dtHdr.Rows[0]["Mail_SendYN"], dtHdr.Rows[0]["Shipment_Date"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], "", dtHdr.Rows[0]["Origin_LCA_No"], dtHdr.Rows[0]["Certification"], dtHdr.Rows[0]["LCAF_No"], dtHdr.Rows[0]["Notify"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["Import_Decl_No"], dtHdr.Rows[0]["Nominated_Ref_details"], dtHdr.Rows[0]["Ins_Ref_No"], dtHdr.Rows[0]["EPCG_Lic_No"], dtHdr.Rows[0]["Extra_PriceBrk"], dtHdr.Rows[0]["DescriptionofGood"], dtHdr.Rows[0]["FreeText2"], dtHdr.Rows[0]["FreeText3"]);

                    //Modified By Megha 30/06/2014
                    if (dtHdr.Rows[0]["Mail_SendYN"] == "Y")
                    {
                        objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Confirm", dtHdr.Rows[0]["ID"]);
                    }
                    //sujata 03022011
                    //objDB.ExecuteStoredProcedure("SP_PreshipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Preshipment_Inv_No"], dtHdr.Rows[0]["Preshipment_Inv_Date"], dtHdr.Rows[0]["ORF_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Preshipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["OrgPreshipmentRefNo"], dtHdr.Rows[0]["PresProcessStatus"], PrevPreshipmentNo);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //To Save Model Record
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet, string PrevPreshipmentNo)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID, PrevPreshipmentNo) == false) goto ExitFunction;

                // Save Details 
                if (bSaveDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
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
            int iPreshipmentDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iPreshipmentDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                    if (iPreshipmentDetID == 0)
                    {
                        //iPreshipmentDetID = objDB.ExecuteStoredProcedure("SP_PreshipmentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["ORF_Det_ID"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["PDIDone_YN"]);
                        iPreshipmentDetID = objDB.ExecuteStoredProcedure("SP_PreshipmentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["ORF_Det_ID"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["PDIDone_YN"], dtDet.Rows[iRowCnt]["Accept_YN"], dtDet.Rows[iRowCnt]["Reason_ID"], dtDet.Rows[iRowCnt]["ForSelectCh"], dtDet.Rows[iRowCnt]["NtAccQty"]);
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_PreshipmentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["ORF_Det_ID"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["PDIDone_YN"], dtDet.Rows[iRowCnt]["Accept_YN"], dtDet.Rows[iRowCnt]["Reason_ID"], dtDet.Rows[iRowCnt]["ForSelectCh"], dtDet.Rows[iRowCnt]["NtAccQty"]);
                        //objDB.ExecuteStoredProcedure("SP_PreshipmentDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["ORF_Det_ID"], dtDet.Rows[iRowCnt]["Shipment_Date"], dtDet.Rows[iRowCnt]["PDIDone_YN"]);                    
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
        /// PreshipmentId else '0' 
        /// sPreshipmentType 'All' all Preshipment HDR +Det,'Confirm' get Confirm Preshipment ,'Max' Max Record, 'LC' 
        /// Preshipment_Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        public DataSet GetPreshipment(int iPreshipmentID, string sPreshipmentType, string sPreshipment_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPreshipment", iPreshipmentID, sPreshipmentType, sPreshipment_Model_Part, iDealerID);
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

        public DataSet GetMultiPreshipment(String sPreshipmentInv)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetMultiPreShipment", sPreshipmentInv);
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

        public DataSet GetMultiPreshipmentFORLC(String sPreshipmentInv)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMultipleLC", sPreshipmentInv);
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
        /// ChallanId else '0' 
        /// ChallanType 'All' all Preshipment HDR +Det,'Confirm' get Confirm Challan ,'Max' Max Record, 
        /// Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        public DataSet GetChallan(int iChallanID, string sChallanType, string sModel_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChallan", iChallanID, sChallanType, sModel_Part, iDealerID);
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
        /// Update Challan Details
        /// </summary>
        /// <param name="iPreshipmentID">Id Of the Preshipment</param>
        /// <param name="sPackingListNo">No Of the Packing List </param>
        /// <param name="sPackingListDate">Date Of the Packing List  </param>
        /// <returns></returns>
        public bool bSaveChallan(string sDealerCode, int iDelearId, string sModel_Part, DataTable dtHdr)
        {
            bool bUpdateRecord = false;
            string sDocName = "";
            string sConfirmYN = "N";
            int iHdrID = 0;
            if (sModel_Part == "M")
            {
                sDocName = "VCHA";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SCHA";
            }

            //Save Header

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Challan_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    objDB.ExecuteStoredProcedure("SP_Challan_Save", iHdrID, dtHdr.Rows[0]["Preshipment_ID"], dtHdr.Rows[0]["Postshipment_ID"], dtHdr.Rows[0]["Challan_No"], dtHdr.Rows[0]["Challan_Date"], dtHdr.Rows[0]["Challan_Confirm"], dtHdr.Rows[0]["Net_Weight"], dtHdr.Rows[0]["Gross_Weight"], dtHdr.Rows[0]["Challan_RefNo"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Challan_Save", iHdrID, dtHdr.Rows[0]["Preshipment_ID"], dtHdr.Rows[0]["Postshipment_ID"], dtHdr.Rows[0]["Challan_No"], dtHdr.Rows[0]["Challan_Date"], dtHdr.Rows[0]["Challan_Confirm"], dtHdr.Rows[0]["Net_Weight"], dtHdr.Rows[0]["Gross_Weight"], dtHdr.Rows[0]["Challan_RefNo"]);
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
