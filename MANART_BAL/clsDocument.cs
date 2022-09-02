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
    /// Summary description for clsDocument
    /// </summary>
    public class clsDocument
    {
        public clsDocument()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Boolean UpdateChassisAndINS(string strInCorrectchassiNo, string strCorrectchassiNo, string strINSRePost)
        {
            clsDB objDB = new clsDB();
            bool bReturnStatus = false;
            try
            {
                objDB.ExecuteStoredProcedure("sp_UpdateChassisNoAndINSpostFlag", strInCorrectchassiNo, strCorrectchassiNo, strINSRePost);
                bReturnStatus = true;
            }
            catch (Exception ex)
            {
                bReturnStatus = false;
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                objDB = null;
            }
            return bReturnStatus;
        }

        public DataSet GetDoccumentstatus(string strDocType, string strDlrCode, string strDoccNo)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet dtDocDeatils = new DataSet();
                dtDocDeatils = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetDCSDoccumentStatus", strDocType, strDlrCode, strDoccNo);
                return dtDocDeatils;
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
        public DataSet GetDMSDoccumentstatus(string strDocType, string strDoccNo, string strDlrCode)
        {


            clsDB objDB = new clsDB();
            try
            {
                DataSet dtDocDeatils = new DataSet();
                dtDocDeatils = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetDMSDoccumentStatus", strDocType, strDoccNo, strDlrCode);
                return dtDocDeatils;
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
        public int UpdateDoccXMLGenFlag(string strDocType, string strDlrCode, string strDoccNo)
        {
            int iReturn = 0;
            clsDB objDB = new clsDB();
            try
            {
                iReturn = objDB.ExecuteStoredProcedure("sp_UpdateDCSDoccumentXMLGenFlag", strDocType, strDlrCode, strDoccNo);
                return iReturn;
            }
            catch (Exception ex)
            {
                return iReturn;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public int UpdateDMSDoccXMLGenFlag(string strDocType, string strDoccNo, string strDlrCode)
        {
            int iReturn = 0;
            clsDB objDB = new clsDB();
            try
            {
                iReturn = objDB.ExecuteStoredProcedure("sp_UpdateDMSDoccumentXMLGenFlag", strDocType, strDoccNo, strDlrCode);
                return iReturn;
            }
            catch (Exception ex)
            {
                return iReturn;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet GetInputXMLData(string strDocType, string strDoccNo, string strDlrCode)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet dtDocDeatils = new DataSet();
                dtDocDeatils = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetInputXMLData", strDocType, strDoccNo, strDlrCode);
                return dtDocDeatils;
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
        public int UpdateJobcardData(string strDocType, string strDoccNo, string strDlrCode, int Kms, int Hrs)
        {
            int iReturn = 0;
            clsDB objDB = new clsDB();
            try
            {
                iReturn = objDB.ExecuteStoredProcedure("sp_UpdateJobcardData", strDocType, strDoccNo, strDlrCode, Kms, Hrs);
                return iReturn;
            }
            catch (Exception ex)
            {
                return iReturn;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public int InsertChassisData(string chassisNo, string EngineNo, string ModelCode, string RegNo)
        {
            int iReturn = 0;
            clsDB objDB = new clsDB();
            try
            {
                iReturn = objDB.ExecuteStoredProcedure("Sp_InsertChassis", chassisNo, EngineNo, ModelCode, RegNo);
                return iReturn;
            }
            catch (Exception ex)
            {
                return iReturn;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public int PingXml(string strDocType)
        {
            int iReturn = 0;
            clsDB objDB = new clsDB();
            try
            {
                iReturn = objDB.ExecuteStoredProcedure("Sp_PingXml", strDocType);
                return iReturn;
            }
            catch (Exception ex)
            {
                return iReturn;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

    }
}
