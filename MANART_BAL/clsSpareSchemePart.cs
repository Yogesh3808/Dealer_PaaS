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
    /// Summary description for clsSpareSchemePart
    /// </summary>
    public class clsSpareSchemePart
    {
        public clsSpareSchemePart()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //Get SpareSchemePart Details

        /// <summary>    
        /// To Get SpareSchemePart of all types    
        /// </summary>    
        /// <param name="sRFPType"> sSpareSchemePartType 'All','Max', 'New' </param>
        public DataSet GetSpareSchemePart(int iSpareSchemePartID, string sSpareSchemePartType)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSparePartScheme", iSpareSchemePartID, sSpareSchemePartType);
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
        public bool bSaveSpareSchemePart(DataTable dtHdr, DataTable dtDet, ref int iPartSchemeHdrID)
        {
            int iHdrID = 0;

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveSpareSchemePartHeader(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (bSaveSpareSchemePartDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                iPartSchemeHdrID = iHdrID;
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
        private bool bSaveSpareSchemePartHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_SpareSchemePartHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SchemeName"], dtHdr.Rows[0]["FromDate"], dtHdr.Rows[0]["ToDate"], dtHdr.Rows[0]["Scheme_Confirm"]);

                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SpareSchemePartHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SchemeName"], dtHdr.Rows[0]["FromDate"], dtHdr.Rows[0]["ToDate"], dtHdr.Rows[0]["Scheme_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool bSaveSpareSchemePartDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "Y")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_SpareSchemePartDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_ID"], dtDet.Rows[iRowCnt]["MinQty"], dtDet.Rows[iRowCnt]["MaxQty"], dtDet.Rows[iRowCnt]["Discount"]);

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "Y")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_SpareSchemePartDet_Delete", dtDet.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        public int SpareSchemeNameAvailablity(DataTable dtHdr)
        {
            int ret = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_SpareSchemeName_Availablity", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["SchemeName"], dtHdr.Rows[0]["FromDate"], dtHdr.Rows[0]["ToDate"]);
                return ret;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Sujata 28112011
        //public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, DataTable dtDet)
        {

            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                // Commented by Shyamal on 05042012 'BeginTranasaction' not needed here
                //objDB.BeginTranasaction();
                if (bSaveUploadedPartDetails(sFileName, dtDet) == true)
                {

                    //ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesPOPartDetailsFromExcelSheet", sFileName, iDealerId);
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesCampaignPartDetailsFromExcelSheet", sFileName);

                }
                // Commented by Shyamal on 05042012 'RollbackTransaction' not needed here            
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

        //private bool bSaveUploadedPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        private bool bSaveUploadedPartDetails(string sFileName, DataTable dtDet)
        {
            //saveVehicleInDetails       
            string bNewTable = "Y";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //objDB.ExecuteStoredProcedure("SP_InsertSparesPOPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
                    objDB.ExecuteStoredProcedure("SP_InsertSparesCampaignPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["MinQty"], dtDet.Rows[iRowCnt]["MaxQty"], dtDet.Rows[iRowCnt]["Discount"], bNewTable);
                    bNewTable = "N";
                }
                objDB.CommitTransaction();
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
        //Sujata 28112011

        //Sujata 30112011
        public DataSet GetDealerSparesScheme(string sDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_DealerLinkSparesScheme", sDealerID);
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

        public bool bSaveDealerSparesScheme(DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if ((dtDet.Rows[iRowCnt]["DealerID"].ToString() != "0"))
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDealerLinkSparesScheme", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["DealerID"], dtDet.Rows[iRowCnt]["SchemeNo"], dtDet.Rows[iRowCnt]["Active"]);
                    }
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
        //Sujata 30112011
    }
}
