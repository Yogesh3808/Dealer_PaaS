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
    /// Summary description for clsPlantExportDetails
    /// </summary>
    public class clsPlantExportDetails
    {
        public clsPlantExportDetails()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveModelDetails(clsDB objDB, DataTable dtDet, int iHdrID, string sConfirm)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iPlantDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iPlantDetID = objDB.ExecuteStoredProcedure("SP_VPlantDetailsDet_Save", iHdrID, dtDet.Rows[iRowCnt]["ORF_HDR_ID"], dtDet.Rows[iRowCnt]["Model_ID"], dtDet.Rows[iRowCnt]["Plant_Qty"], dtDet.Rows[iRowCnt]["EDD_Date"], dtDet.Rows[iRowCnt]["Remark"], sConfirm, dtDet.Rows[iRowCnt]["VECVEDDDate"], dtDet.Rows[iRowCnt]["VECVRemark"]);
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        private bool bSavePartDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            int iPlantDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iPlantDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);

                    iPlantDetID = objDB.ExecuteStoredProcedure("SP_PlantDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Plant_Qty"]);
                    //if (iPlantDetID == 0)
                    //{
                    //    iPlantDetID = objDB.ExecuteStoredProcedure("SP_PlantDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Plant_Qty"]);
                    //}
                    //else
                    //{
                    //    objDB.ExecuteStoredProcedure("SP_PlantDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Plant_Qty"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }

        private bool bSaveExportModelDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iExportDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iExportDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);

                    iExportDetID = objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Model_ID"], 0, dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //if (iExportDetID == 0)
                    //{
                    //    iExportDetID = objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Model_ID"], 0, dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //}
                    //else
                    //{
                    //    objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Model_ID"], 0, dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveExportPartDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            int iExportDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    iExportDetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);

                    iExportDetID = objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //if (iExportDetID == 0)
                    //{
                    //    iExportDetID = objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //}
                    //else
                    //{
                    //    objDB.ExecuteStoredProcedure("SP_ExportDetailsDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, 0, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Bill_Qty"]);
                    //}
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
        public DataSet GetPlantDetails(int iDocID, string sDocType, string sDoc_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                //ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantDoc", iDocID, sDocType, sDoc_Model_Part, iDealerID);

                if (sDoc_Model_Part == "P")
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantDoc", iDocID, sDocType, sDoc_Model_Part, iDealerID);
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantDocV", sDocType, iDocID, iDealerID);
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
        public DataSet GetExportDetails(int iDocID, string sDocType, string sDoc_Model_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetExportDoc", iDocID, sDocType, sDoc_Model_Part, iDealerID);
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
        public bool bUpdatePlantDetails(int iDealerID, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VPLNT";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SPLNT";
            }
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();                
                int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                string sFinYear = Func.sGetFinancialYear(iDelearId);
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);

                if (iHdrID == 0)
                {
                    //Insert Header 
                    iHdrID = objDB.ExecuteStoredProcedure("SP_PlantDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Plant_No"], dtHdr.Rows[0]["Plant_Date"], dtHdr.Rows[0]["ORF_Hdr_ID"], dtHdr.Rows[0]["Plant_Confirm"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    //Update Header 
                    objDB.ExecuteStoredProcedure("SP_PlantDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Plant_No"], dtHdr.Rows[0]["Plant_Date"], dtHdr.Rows[0]["ORF_Hdr_ID"], dtHdr.Rows[0]["Plant_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                // Save Details 
                if (sModel_Part == "M")
                {
                    if (bSaveModelDetails(objDB, dtDet, iHdrID, Func.Convert.sConvertToString(dtHdr.Rows[0]["Plant_Confirm"])) == false) goto ExitFunction;
                }
                else if (sModel_Part == "P")
                {
                    if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                }

                //Update Header 
                if (dtHdr.Rows[0]["Plant_Confirm"] == "Y")
                {
                    objDB.ExecuteStoredProcedure("SP_PlantDetailBalance_Save", iHdrID, dtHdr.Rows[0]["ORF_Hdr_ID"], sModel_Part);
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

        public bool bUpdateVPlantDetails(DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            string sDocName = "";
            string sModel_Part = "M";// Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VPLNT";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SPLNT";
            }
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                string sFinYear = Func.sGetFinancialYear(9999);
                //int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);

                if (iHdrID == 0)
                {
                    //Insert Header 
                    iHdrID = objDB.ExecuteStoredProcedure("SP_VPlantDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Plant_No"], dtHdr.Rows[0]["Plant_Date"], dtHdr.Rows[0]["Plant_Confirm"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, -1);
                }
                else
                {
                    //Update Header 
                    objDB.ExecuteStoredProcedure("SP_VPlantDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Plant_No"], dtHdr.Rows[0]["Plant_Date"], dtHdr.Rows[0]["Plant_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                // Save Details 
                if (sModel_Part == "M")
                {
                    if (bSaveModelDetails(objDB, dtDet, iHdrID, Func.Convert.sConvertToString(dtHdr.Rows[0]["Plant_Confirm"])) == false) goto ExitFunction;
                }
                else if (sModel_Part == "P")
                {
                    if (bSavePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                }

                //Update Header 
                //if (dtHdr.Rows[0]["Plant_Confirm"] == "Y")
                //{
                //    objDB.ExecuteStoredProcedure("SP_PlantDetailBalance_Save", iHdrID, dtHdr.Rows[0]["ORF_Hdr_ID"], sModel_Part);
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
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bReturnPlantDetail(int iORF_HDR_ID, string sRetRemark)
        {

            bool bSaveRecord = false;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_PlantDetail_Return", iORF_HDR_ID, sRetRemark);
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
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bCloseIndentDetail(int iORF_HDR_ID)
        {

            bool bSaveRecord = false;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_PlantDetail_Close", iORF_HDR_ID);
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
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bUpdateExportDetails(int iDealerID, int iCountryId, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VORDE";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SORDE";
            }
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();                
                int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                string sFinYear = Func.sGetFinancialYear(iDelearId);
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);

                if (iHdrID == 0)
                {
                    //Insert Header 
                    iHdrID = objDB.ExecuteStoredProcedure("SP_ExportDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Export_No"], dtHdr.Rows[0]["Export_Date"], 0, dtHdr.Rows[0]["ORF_Hdr_ID"], dtHdr.Rows[0]["Export_Confirm"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);
                }
                else
                {
                    //Update Header 
                    objDB.ExecuteStoredProcedure("SP_ExportDetailHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Export_No"], dtHdr.Rows[0]["Export_Date"], 0, dtHdr.Rows[0]["ORF_Hdr_ID"], dtHdr.Rows[0]["Export_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                // Save Details 
                if (sModel_Part == "M")
                {
                    if (bSaveExportModelDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                }
                else if (sModel_Part == "P")
                {
                    if (bSaveExportPartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                }

                //Update Header 
                if (dtHdr.Rows[0]["Export_Confirm"] == "Y")
                {
                    int iORFID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ORF_Hdr_ID"]);
                    string sDate = Func.Common.sGetCurrentDate(iCountryId, false);
                    if (sModel_Part == "M")
                    {
                        if (bUpdateHdrValueForPreshipmentVInvoice(iORFID, iDealerID, sDate, 0, 0) == 0) goto ExitFunction;
                    }
                    else
                    {
                        if (bUpdateHdrValueForPreshipmentSInvoice(iORFID, iDealerID, sDate) == false) goto ExitFunction;
                    }

                    objDB.ExecuteStoredProcedure("SP_PlantDetailBalance_Save", iHdrID, dtHdr.Rows[0]["ORF_Hdr_ID"], sModel_Part);
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

        public int bUpdateHdrValueForPreshipmentVInvoice(int iORF_HDR_ID, int iDealerID, String sDate, int iModelID, int iQty)
        {
            try
            {
                int bUpdateHdrValueForPreshipmentInvoice = 0;
                clsORF ObjORF = new clsORF();
                clsPreshipment ObjPreshipment = new clsPreshipment();
                clsDB objDB = new clsDB();
                DataSet dsPres = new DataSet();
                int iORFID = Func.Convert.iConvertToInt(iORF_HDR_ID); ;
                //iORFID = 7;
                if (iORFID != 0)
                {
                    dsPres = ObjORF.GetORF(iORFID, "PreShipment", "M", iDealerID, iModelID, iQty);
                    if (dsPres.Tables[0].Rows.Count == 0)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                        return bUpdateHdrValueForPreshipmentInvoice;
                    }
                    else if (dsPres.Tables[1].Rows.Count == 0)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                        return bUpdateHdrValueForPreshipmentInvoice;
                    }
                    DataTable dtHdrPreshipment = new DataTable();
                    DataTable dtDtlPreshipment = new DataTable();
                    DataRow dr;
                    //Get Header InFormation   
                    dtHdrPreshipment.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Inv_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Inv_Date", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("ORF_ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Model_Part", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Confirm", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Narration", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("TotalAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("GrandAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Status", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Mail_SendYN", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Shipment_Date", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("OrgPreshipmentRefNo", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("PresProcessStatus", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Origin_LCA_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Certification", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("LCAF_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Notify", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("EPCG_Lic_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Extra_PriceBrk", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("TotalIncoAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Import_Decl_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Nominated_Ref_details", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Ins_Ref_No", typeof(string)));

                    dr = dtHdrPreshipment.NewRow();
                    dr["ID"] = Func.Convert.iConvertToInt("0");
                    string sPreshipment_Inv_No = "";

                    int iMaxNo = 0;
                    string sMaxNo = "";
                    iMaxNo = Func.Common.iGetMaxDocNo(DateTime.Now.Year.ToString().Substring(2, 2), "ETBV", 999);
                    sMaxNo = Func.Convert.sConvertToString(iMaxNo + 1);
                    sMaxNo = sMaxNo.PadLeft(4, '0');
                    sPreshipment_Inv_No = "ETBV/" + DateTime.Now.Year.ToString().Substring(2, 2) + "/" + sMaxNo;

                    dr["Preshipment_Inv_No"] = sPreshipment_Inv_No;
                    dr["Preshipment_Inv_Date"] = sDate;
                    dr["ORF_ID"] = Func.Convert.iConvertToInt(iORF_HDR_ID);
                    dr["Dealer_ID"] = iDealerID;
                    dr["Model_Part"] = "M";
                    dr["Preshipment_Confirm"] = "N";
                    dr["Narration"] = "";
                    dr["TotalAmount"] = 0; //Func.Convert.dConvertToDouble(txtTotalAmt.Text);
                    dr["GrandAmount"] = 0; //Func.Convert.dConvertToDouble(txtGrandAmt.Text);
                    dr["Preshipment_Status"] = "C"; //C : Complete initially 
                    dr["Mail_SendYN"] = "N";
                    dr["Shipment_Date"] = sDate;
                    dr["OrgPreshipmentRefNo"] = "";
                    dr["PresProcessStatus"] = "C"; //C: Complete initially                
                    dr["Origin_LCA_No"] = "";
                    dr["Certification"] = "";
                    dr["LCAF_No"] = "";
                    dr["Notify"] = "";
                    dr["EPCG_Lic_No"] = "";
                    dr["Extra_PriceBrk"] = "";
                    dr["TotalIncoAmount"] = 0;
                    dr["Import_Decl_No"] = "";
                    dr["Nominated_Ref_details"] = "";
                    dr["Ins_Ref_No"] = "";

                    dtHdrPreshipment.Rows.Add(dr);
                    dtHdrPreshipment.AcceptChanges();

                    dtDtlPreshipment = dsPres.Tables[1];
                    for (int iRowCnt = 0; iRowCnt < dtDtlPreshipment.Rows.Count; iRowCnt++)
                    {
                        dtDtlPreshipment.Rows[iRowCnt]["Shipment_Date"] = sDate;
                        dtDtlPreshipment.Rows[iRowCnt]["PDIDone_YN"] = "N";
                        dtDtlPreshipment.Rows[iRowCnt]["Accept_YN"] = "Y";
                        dtDtlPreshipment.Rows[iRowCnt]["Reason_ID"] = 0;
                        dtDtlPreshipment.Rows[iRowCnt]["ForSelectCh"] = "Y";
                    }

                    if (ObjPreshipment.bSaveRecord(Func.Convert.sConvertToString(iDealerID), dtHdrPreshipment, dtDtlPreshipment, "") == true)
                    {
                        objDB.ExecuteStoredProcedure("SP_PlantDetailBalance_Save", 0, Func.Convert.iConvertToInt(iORF_HDR_ID), 'M');

                        bUpdateHdrValueForPreshipmentInvoice = dtDtlPreshipment.Rows.Count;
                    }
                    else
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                    }
                    ObjPreshipment = null;
                    ObjORF = null;
                }
                return bUpdateHdrValueForPreshipmentInvoice;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return 0;
            }
        }

        public int bUpdateHdrValueForPreshipmentVInvoiceEdit(int iORF_HDR_ID, int iPreshipmentID, int iDealerID, String sDate, int iModelID, int iQty)
        {
            try
            {
                int bUpdateHdrValueForPreshipmentInvoice = 0;
                clsORF ObjORF = new clsORF();
                clsPreshipment ObjPreshipment = new clsPreshipment();
                clsDB objDB = new clsDB();
                DataSet dsPres = new DataSet();
                int iORFID = Func.Convert.iConvertToInt(iORF_HDR_ID); ;
                //iORFID = 7;
                if (iORFID != 0)
                {
                    dsPres = ObjORF.GetIndentPreshipment(iORFID, iPreshipmentID, "PreshipmentEdit", "M", iDealerID, iModelID, iQty);
                    if (dsPres.Tables[0].Rows.Count == 0)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                        return bUpdateHdrValueForPreshipmentInvoice;
                    }
                    else if (dsPres.Tables[1].Rows.Count == 0)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                        return bUpdateHdrValueForPreshipmentInvoice;
                    }
                    DataTable dtHdrPreshipment = new DataTable();
                    DataTable dtDtlPreshipment = new DataTable();

                    dtHdrPreshipment = dsPres.Tables[0];
                    dtDtlPreshipment = dsPres.Tables[1];

                    for (int iRowCnt = 0; iRowCnt < dtDtlPreshipment.Rows.Count; iRowCnt++)
                    {
                        dtDtlPreshipment.Rows[iRowCnt]["Shipment_Date"] = sDate;
                        dtDtlPreshipment.Rows[iRowCnt]["PDIDone_YN"] = "N";
                        dtDtlPreshipment.Rows[iRowCnt]["Accept_YN"] = "Y";
                        dtDtlPreshipment.Rows[iRowCnt]["Reason_ID"] = 0;
                        dtDtlPreshipment.Rows[iRowCnt]["ForSelectCh"] = "Y";
                    }

                    if (ObjPreshipment.bSaveRecord(Func.Convert.sConvertToString(iDealerID), dtHdrPreshipment, dtDtlPreshipment, "") == true)
                    {
                        objDB.ExecuteStoredProcedure("SP_PlantDetailBalance_Save", 0, Func.Convert.iConvertToInt(iORF_HDR_ID), 'M');

                        bUpdateHdrValueForPreshipmentInvoice = dtDtlPreshipment.Rows.Count;
                    }
                    else
                    {
                        bUpdateHdrValueForPreshipmentInvoice = 0;
                    }
                    ObjPreshipment = null;
                    ObjORF = null;
                }
                return bUpdateHdrValueForPreshipmentInvoice;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return 0;
            }
        }

        private bool bUpdateHdrValueForPreshipmentSInvoice(int iORF_HDR_ID, int iDealerID, String sDate)
        {
            try
            {
                bool bUpdateHdrValueForPreshipmentInvoice = true;
                clsORF ObjORF = new clsORF();
                clsPreshipment ObjPreshipment = new clsPreshipment();

                DataSet dsPres = new DataSet();
                int iORFID = Func.Convert.iConvertToInt(iORF_HDR_ID); ;
                //iORFID = 7;
                if (iORFID != 0)
                {
                    dsPres = ObjORF.GetORF(iORFID, "PreShipment", "P", iDealerID, 0, 0);
                    if (dsPres.Tables[0].Rows.Count == 0)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = false;
                    }
                    DataTable dtHdrPreshipment = new DataTable();
                    DataTable dtDtlPreshipment = new DataTable();
                    DataRow dr;
                    //Get Header InFormation   
                    dtHdrPreshipment.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Inv_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Inv_Date", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("ORF_ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Model_Part", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Confirm", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Narration", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("TotalAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("GrandAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Preshipment_Status", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Mail_SendYN", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Shipment_Date", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("OrgPreshipmentRefNo", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("PresProcessStatus", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Origin_LCA_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Certification", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("LCAF_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Notify", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("EPCG_Lic_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Extra_PriceBrk", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("TotalIncoAmount", typeof(double)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Import_Decl_No", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Nominated_Ref_details", typeof(string)));
                    dtHdrPreshipment.Columns.Add(new DataColumn("Ins_Ref_No", typeof(string)));

                    dr = dtHdrPreshipment.NewRow();
                    dr["ID"] = Func.Convert.iConvertToInt("0");
                    string sPreshipment_Inv_No = "";

                    int iMaxNo = 0;
                    string sMaxNo = "";
                    iMaxNo = Func.Common.iGetMaxDocNo(DateTime.Now.Year.ToString().Substring(2, 2), "ETBS", 999);
                    sMaxNo = Func.Convert.sConvertToString(iMaxNo + 1);
                    sMaxNo = sMaxNo.PadLeft(4, '0');
                    sPreshipment_Inv_No = "ETBS/" + DateTime.Now.Year.ToString().Substring(2, 2) + "/" + sMaxNo;

                    dr["Preshipment_Inv_No"] = sPreshipment_Inv_No;
                    dr["Preshipment_Inv_Date"] = sDate;
                    dr["ORF_ID"] = Func.Convert.iConvertToInt(iORF_HDR_ID);
                    dr["Dealer_ID"] = iDealerID;
                    dr["Model_Part"] = "P";
                    dr["Preshipment_Confirm"] = "N";
                    dr["Narration"] = "";
                    dr["TotalAmount"] = 0; //Func.Convert.dConvertToDouble(txtTotalAmt.Text);
                    dr["GrandAmount"] = 0; //Func.Convert.dConvertToDouble(txtGrandAmt.Text);
                    dr["Preshipment_Status"] = "C"; //C : Complete initially 
                    dr["Mail_SendYN"] = "N";
                    dr["Shipment_Date"] = sDate;
                    dr["OrgPreshipmentRefNo"] = "";
                    dr["PresProcessStatus"] = "C"; //C: Complete initially                
                    dr["Origin_LCA_No"] = "";
                    dr["Certification"] = "";
                    dr["LCAF_No"] = "";
                    dr["Notify"] = "";
                    dr["EPCG_Lic_No"] = "";
                    dr["Extra_PriceBrk"] = "";
                    dr["TotalIncoAmount"] = 0;
                    dr["Import_Decl_No"] = "";
                    dr["Nominated_Ref_details"] = "";
                    dr["Ins_Ref_No"] = "";

                    dtHdrPreshipment.Rows.Add(dr);
                    dtHdrPreshipment.AcceptChanges();

                    //dtDtlPreshipment = dsPres.Tables[1];
                    //for (int iRowCnt = 0; iRowCnt < dtDtlPreshipment.Rows.Count; iRowCnt++)
                    //{
                    //    dtDtlPreshipment.Rows[iRowCnt]["Shipment_Date"] = sDate;
                    //    dtDtlPreshipment.Rows[iRowCnt]["PDIDone_YN"] = "N";
                    //    dtDtlPreshipment.Rows[iRowCnt]["Accept_YN"] = "Y";
                    //    dtDtlPreshipment.Rows[iRowCnt]["Reason_ID"] = 0;
                    //    dtDtlPreshipment.Rows[iRowCnt]["ForSelectCh"] = "Y";
                    //}
                    //if (ObjPreshipment.bSaveRecord(Func.Convert.sConvertToString(iDealerID), dtHdrPreshipment, dtDtlPreshipment, "") == true)
                    if (ObjPreshipment.bSaveSpExportHeader(Func.Convert.sConvertToString(iDealerID), dtHdrPreshipment) == true)
                    {
                        bUpdateHdrValueForPreshipmentInvoice = true;
                    }
                    else
                    {
                        bUpdateHdrValueForPreshipmentInvoice = false;
                    }
                    ObjPreshipment = null;
                    ObjORF = null;
                }
                return bUpdateHdrValueForPreshipmentInvoice;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }
        }

        public bool DeletePreshipmentDtlsForEditQty(int iPreshipmentID)
        {

            bool bSaveRecord = false;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_PreshipmentDtl_Delete", iPreshipmentID);
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

        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet, string sORF_HDR_ID, string sDocName)
        {
            DataSet ds = null;

            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportORFPlantPartDetailsFromExcelSheet", sORF_HDR_ID, sDocName);
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
                    //objDB.ExecuteStoredProcedure("SP_InsertPartDetailsFromExcelSheetForORFBal", sFileName, dtDet.Rows[iRowCnt]["Part No"],Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Indent Qty"]),Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Bal Indent Qty"]),Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Plant Qty"]),Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Bal Plant Qty"]), Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Bill Qty"]), bNewTable);
                    objDB.ExecuteStoredProcedure("SP_InsertPartDetailsFromExcelSheetForORFBal", sFileName, dtDet.Rows[iRowCnt]["Part No"], Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Indent Qty"]), Func.Convert.iConvertToInt("0"), Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Plant Qty"]), Func.Convert.iConvertToInt("0"), Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Bill Qty"]), bNewTable);
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


        public DataSet UploadVEDDDetailsAndGetEDDDetails(string sFileName, DataTable dtDet, string sDocName)
        {
            DataSet ds = null;

            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedVEDDDetails(sFileName, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportVPlantEDDDetailsFromExcelSheet");
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


        private bool bSaveUploadedVEDDDetails(string sFileName, DataTable dtDet)
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
                    objDB.ExecuteStoredProcedure("SP_ImportVehPlantEDDDetailsFromExcelSheet", sFileName, Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Fert Code"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Indent No"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Indent Qty"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Expected Date of Despatch (EDD)"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Remarks"]), bNewTable);
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
