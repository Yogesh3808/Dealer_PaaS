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
    /// Summary description for clsProforma
    /// </summary>
    public class clsProforma
    {
        public clsProforma()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        // To Save Proforma Header record
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);

            if (sModel_Part == "M")
            {
                sDocName = "VPRI";
            }
            else if (sModel_Part == "P")
            {
                // sDocName = "SPRI";
                sDocName = "SPFI";
            }
            //Save Header
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Proforma_Inv_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    //,dtHdr.Rows[0]["Is_PrintManufacturer"],dtHdr.Rows[0]["Is_PrintInsurance"],dtHdr.Rows[0]["Is_PrintPreShipment"],dtHdr.Rows[0]["Is_PrintPortOfShipment"],dtHdr.Rows[0]["Is_PrintCountryOfOrigin"],dtHdr.Rows[0]["Is_PrintOurBanker"]

                    //Sujata 14012011
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"],dtHdr.Rows[0]["IsIncludeSign"]);
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"], dtHdr.Rows[0]["IsIncludeSign"], dtHdr.Rows[0]["Fumigation_Required"], dtHdr.Rows[0]["VECVBankInfo_ID"], dtHdr.Rows[0]["Is_PriceBreakUp"], dtHdr.Rows[0]["Is_DisCount"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"], dtHdr.Rows[0]["IsIncludeSign"], dtHdr.Rows[0]["Fumigation_Required"], dtHdr.Rows[0]["VECVBankInfo_ID"], dtHdr.Rows[0]["Is_PriceBreakUp"], dtHdr.Rows[0]["Is_DisCount"], dtHdr.Rows[0]["Is_PrintPrice"], dtHdr.Rows[0]["Is_PrintShipment"], dtHdr.Rows[0]["Is_PrintValidity"], dtHdr.Rows[0]["Is_PrintPayment"], dtHdr.Rows[0]["UserIDDisc"], dtHdr.Rows[0]["Is_PrintManufacturer"], dtHdr.Rows[0]["Is_PrintInsurance"], dtHdr.Rows[0]["Is_PrintPreShipment"], dtHdr.Rows[0]["Is_PrintPortOfShipment"], dtHdr.Rows[0]["Is_PrintCountryOfOrigin"], dtHdr.Rows[0]["Is_PrintOurBanker"]);
                    //Sujata 14012011
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    //Sujata 14012011
                    //objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"],dtHdr.Rows[0]["IsIncludeSign"]);
                    //objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE",dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"], dtHdr.Rows[0]["IsIncludeSign"], dtHdr.Rows[0]["Fumigation_Required"], dtHdr.Rows[0]["VECVBankInfo_ID"], dtHdr.Rows[0]["Is_PriceBreakUp"], dtHdr.Rows[0]["Is_DisCount"]);
                    objDB.ExecuteStoredProcedure("SP_ProformaHDR_SAVE", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Proforma_Inv_No"], dtHdr.Rows[0]["Proforma_Inv_Date"], dtHdr.Rows[0]["RFP_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["IncoTerm_ID"], dtHdr.Rows[0]["Payment_Mode_ID"], dtHdr.Rows[0]["Partial_Shipment_YN"], dtHdr.Rows[0]["ThirdPartyInsAgency_ID"], dtHdr.Rows[0]["destinationPort_ID"], dtHdr.Rows[0]["dispatchMode_ID"], dtHdr.Rows[0]["ShippingLineNomination_YN"], dtHdr.Rows[0]["NominatedAgency_ID"], dtHdr.Rows[0]["ValidityDays_ID"], dtHdr.Rows[0]["LastDate_Negotiation"], dtHdr.Rows[0]["ShipmentDate"], dtHdr.Rows[0]["ValidityDate"], dtHdr.Rows[0]["ShipmentDays_Id"], dtHdr.Rows[0]["MultiModal_Shipment_YN"], dtHdr.Rows[0]["MultiModal_Destination_ID"], dtHdr.Rows[0]["RFP_Proforma_Flag"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["Proforma_Confirm"], dtHdr.Rows[0]["IsIncludeSign"], dtHdr.Rows[0]["Fumigation_Required"], dtHdr.Rows[0]["VECVBankInfo_ID"], dtHdr.Rows[0]["Is_PriceBreakUp"], dtHdr.Rows[0]["Is_DisCount"], dtHdr.Rows[0]["Is_PrintPrice"], dtHdr.Rows[0]["Is_PrintShipment"], dtHdr.Rows[0]["Is_PrintValidity"], dtHdr.Rows[0]["Is_PrintPayment"], dtHdr.Rows[0]["UserIDDisc"], dtHdr.Rows[0]["Is_PrintManufacturer"], dtHdr.Rows[0]["Is_PrintInsurance"], dtHdr.Rows[0]["Is_PrintPreShipment"], dtHdr.Rows[0]["Is_PrintPortOfShipment"], dtHdr.Rows[0]["Is_PrintCountryOfOrigin"], dtHdr.Rows[0]["Is_PrintOurBanker"]);
                    //Sujata 14012011
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
        public bool bSaveRecordWithModel(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtLcClause)
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
                // Delete Inco Details
                objDB.ExecuteStoredProcedure("SP_ProformaIncoDetails_Del", iHdrID);
                // Save Details with Inco Details
                if (bSaveModelDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                //Save LCClause Details
                if (bSaveLCClauseDetais(objDB, iHdrID, dtLcClause) == false) goto ExitFunction;

                //Update Proforma Flag To RFP
                objDB.ExecuteStoredProcedure("SP_RFPHdr_UpdateProformaFlag", dtHdr.Rows[0]["RFP_ID"]);


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
            int iProformaDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]) == 0)
                        {
                            iProformaDetID = objDB.ExecuteStoredProcedure("SP_ProformaDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["RFP_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["NewModel_Des"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["FOBRate"], dtDet.Rows[iRowCnt]["Discount"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["ShipmentDays_Id"], dtDet.Rows[iRowCnt]["Remarks"], dtDet.Rows[iRowCnt]["Fert_Description"], dtDet.Rows[iRowCnt]["OrgnRate"], dtDet.Rows[iRowCnt]["HeadUserIDDisc"], dtDet.Rows[iRowCnt]["FOBMax"], dtDet.Rows[iRowCnt]["Freight"], dtDet.Rows[iRowCnt]["Insurance"], dtDet.Rows[iRowCnt]["bSentMail"]);
                            //bSaveRecord = bSaveModelIncoDetails(objDB,dtDet.Rows[iRowCnt], iProformaDetID, iHdrID);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_ProformaDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["RFP_Det_ID"], 0, dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["NewModel_Des"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["BodyType_ID"], dtDet.Rows[iRowCnt]["FOBRate"], dtDet.Rows[iRowCnt]["Discount"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["ShipmentDays_Id"], dtDet.Rows[iRowCnt]["Remarks"], dtDet.Rows[iRowCnt]["Fert_Description"], dtDet.Rows[iRowCnt]["OrgnRate"], dtDet.Rows[iRowCnt]["HeadUserIDDisc"], dtDet.Rows[iRowCnt]["FOBMax"], dtDet.Rows[iRowCnt]["Freight"], dtDet.Rows[iRowCnt]["Insurance"], dtDet.Rows[iRowCnt]["bSentMail"]);
                            iProformaDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                            //bSaveRecord = bSaveModelIncoDetails(objDB,dtDet.Rows[iRowCnt], iProformaDetID, iHdrID);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_ProformaDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        // To Save Proforma Model InCo Details
        private bool bSaveModelIncoDetails(clsDB objDB, DataRow drDet, int iProformaDetID, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;

            try
            {
                for (int iColCnt = 12; iColCnt < 18; iColCnt++)
                {
                    if (drDet.ItemArray[iColCnt].ToString() != "-1")
                    {
                        objDB.ExecuteStoredProcedure("SP_ProformaIncoDetails_Save", iHdrID, iProformaDetID, (iColCnt - 9), drDet.ItemArray[iColCnt]);
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
        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet, DataTable dtIncoDetails, DataTable dtLcClause, DataTable dtPartWeight)
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
                // Delete Inco Details
                objDB.ExecuteStoredProcedure("SP_ProformaIncoDetails_Del", iHdrID);
                // Save Details
                if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                // Save Part Weight in Part Master
                if (bSavepartWeight(objDB, dtPartWeight) == false) goto ExitFunction;
                //Save Inco Details
                if (bSavePartIncoDetails(objDB, dtIncoDetails, iHdrID) == false) goto ExitFunction;
                //Save LCClause Details
                if (bSaveLCClauseDetais(objDB, iHdrID, dtLcClause) == false) goto ExitFunction;

                //Update Proforma Flag To RFP
                objDB.ExecuteStoredProcedure("SP_RFPHdr_UpdateProformaFlag", dtHdr.Rows[0]["RFP_ID"]);
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
                        objDB.ExecuteStoredProcedure("SP_ProformaDET_SAVE", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["RFP_Det_ID"], dtDet.Rows[iRowCnt]["Part_No_ID"], 0, "", dtDet.Rows[iRowCnt]["Qty"], 0, dtDet.Rows[iRowCnt]["FOBRate"], dtDet.Rows[iRowCnt]["Discount"], dtDet.Rows[iRowCnt]["UnitAmount"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["ShipmentDays_Id"], "", "", dtDet.Rows[iRowCnt]["FOBRate"], 0, 0, 0, 0, "");
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_ProformaDET_Del", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }
        private bool bSavePartIncoDetails(clsDB objDB, DataTable dtIncoDetails, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;

            try
            {
                for (int iColCnt = 2; iColCnt < 9; iColCnt++)
                {
                    if (dtIncoDetails.Rows[0][iColCnt].ToString() != "-1")
                    {
                        objDB.ExecuteStoredProcedure("SP_ProformaIncoDetails_Save", iHdrID, 0, iColCnt, dtIncoDetails.Rows[0][iColCnt]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        private bool bSaveLCClauseDetais(clsDB objDB, int iRFPHdrId, DataTable dtLcClause)
        {
            bool bSaveRecord = false;
            try
            {
                objDB.ExecuteStoredProcedure("SP_ProformaDetLC_Del", iRFPHdrId);
                for (int iRowCnt = 0; iRowCnt < dtLcClause.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_ProformaDetLC_Save", iRFPHdrId, dtLcClause.Rows[iRowCnt]["ID"]);
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        private bool bSavepartWeight(clsDB objDB, DataTable dtPartWeight)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtPartWeight.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_PartWeight_Save", dtPartWeight.Rows[iRowCnt]["PartID"], dtPartWeight.Rows[iRowCnt]["PartWeight"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        /// <summary>    
        /// ProformaId else '0' 
        /// sProformaType 'All' all Proforma HDR +Det,'Confirm' get Confirm Proforma ,'Max' Max Record, 'LC' 
        /// Proforma_Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        public DataSet GetProforma(int iProformaID, string sProformaType, string sProforma_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Proforma", iProformaID, sProformaType, sProforma_Model_Part, iDealerID);
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
        public DataTable GetIncoTermsDetails(int iIncoTermIds)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = null;
                if (iIncoTermIds != 0)
                {
                    dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_IncoTermColumns", iIncoTermIds);
                }
                return dt;
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
        /// 
        /// </summary>
        /// <param name="sDocNo"></param>
        /// <param name="sDocDate"></param>
        public bool SendMailForProformaSave(string sDocNo, string sDocDate, string sDealerId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.ExecuteStoredProcedureAndGetDataTable("SP_Send_Mail", "SM", "ProformaSave", "M", "VECV", sDocNo, sDocDate, sDealerId, "");
                //'SM', 'WarrantyClaim','P','VECV',@Claim_no,@Confirm_Date,@Dealer_Id,'' 
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
    }
}
