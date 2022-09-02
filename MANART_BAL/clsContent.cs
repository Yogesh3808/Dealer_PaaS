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
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsContent
    /// </summary>
    public class clsContent
    {
        public clsContent()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetMenuDataForContent(string ExportDomestic, int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSMenuData = new DataSet();
                DSMenuData = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuDataForContent", ExportDomestic, ID);
                return DSMenuData;
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
        public DataSet GetContent(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSContent = new DataSet();
                DSContent = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetContent", ID);
                return DSContent;
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
        public bool bSaveContent(DataTable dtHdr)
        {

            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                if (dtHdr.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_Content_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["File_Name"], dtHdr.Rows[0]["Document_Name"], dtHdr.Rows[0]["Dept_ID"], dtHdr.Rows[0]["Effective_Date_From"], dtHdr.Rows[0]["Effective_Date_To"], dtHdr.Rows[0]["Active_Status"], dtHdr.Rows[0]["Is_Confirm"]);
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
        public bool bSaveContentDetails(DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_ContentMgmtDtls_Delete", iHdrID);
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.ExecuteStoredProcedure("SP_ContentMgmtDtls_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Region_ID"], dtDet.Rows[iRowCnt]["Country_ID"], dtDet.Rows[iRowCnt]["State_ID"], dtDet.Rows[iRowCnt]["Dealer_ID"], dtDet.Rows[iRowCnt]["Is_Confirm"], dtDet.Rows[iRowCnt]["Is_DomExp"], dtDet.Rows[iRowCnt]["Is_All"]);

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
        public bool bSaveContentMaster(DataTable dtHdr)
        {
            //saveVehicleInMaster
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_ContentMgmtHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["File_Name"], dtHdr.Rows[0]["Document_Name"], dtHdr.Rows[0]["Document_Heading"], dtHdr.Rows[0]["Dept_ID"], dtHdr.Rows[0]["Effective_Date_From"], dtHdr.Rows[0]["Effective_Date_To"], dtHdr.Rows[0]["Active_Status"], dtHdr.Rows[0]["Is_Confirm"]);
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

        public bool bDeleteContentMaster(int ID)
        {
            //saveVehicleInMaster
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_ContentMgmtHdr_Delete", ID);
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

        //new 
        public DataSet GetDocumentBulletin_New(int iDealer_ID, int iUserType_ID, int iDocTypeID, string sSelectType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSContent = new DataSet();
                DSContent = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDocumentBulletin_New", iDealer_ID, iUserType_ID, iDocTypeID, sSelectType);
                return DSContent;
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

        public DataSet GetDocumentBulletin(int iDealer_ID, int iUserType_ID, string Is_DomExp, string sSelectType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSContent = new DataSet();
                DSContent = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDocumentBulletin", iDealer_ID, iUserType_ID, Is_DomExp, sSelectType);
                return DSContent;
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

        public bool bSaveContent(clsDB objDB, DataTable dtHdr)
        {

            bool bSaveRecord = false;
            try
            {
                if (dtHdr.Rows.Count != 0)
                {
                    //objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_Content_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["File_Name"], dtHdr.Rows[0]["Doc_Name"], dtHdr.Rows[0]["Doc_Heading"], dtHdr.Rows[0]["Doc_Type"], dtHdr.Rows[0]["Region_ID"],
                        dtHdr.Rows[0]["State_ID"], dtHdr.Rows[0]["Country_ID"], dtHdr.Rows[0]["User_ID"], dtHdr.Rows[0]["Active_Status"], dtHdr.Rows[0]["Path"]);
                    // objDB.CommitTransaction();
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
        public bool bSaveRecordWithFile(DataTable dtFileAttach)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                if (bSaveContent(objDB, dtFileAttach) == false) goto ExitFunction;
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
    }
}
