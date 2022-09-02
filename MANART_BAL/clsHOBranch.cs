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
    /// Summary description for clsHOBranch
    /// </summary>
    public class clsHOBranch
    {
        public clsHOBranch()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetMaxHOBranch_Record(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxHOBranch_Record", ID);
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
        public DataTable GetHODealer(int iHO)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtHODealer = new DataTable();
                dtHODealer = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetHODealer", iHO);
                return dtHODealer;
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
        public DataTable GetHOBranchDealer(int iHOBranch, string Type)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtHOBranchDealer = new DataTable();
                dtHOBranchDealer = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetHOBranchDealer", iHOBranch, Type);
                return dtHOBranchDealer;
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
        public bool bSaveDealerHOBranchEntry(int iHODealerID, string sHOCode, int iDealerID, string sBranchCode, string sHOBranchCode, int DealerLocationType)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveHOBranchEntry", iHODealerID, sHOCode, iDealerID, sBranchCode, sHOBranchCode, DealerLocationType);
                objDB.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetBranchCode(int iHODealer)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsBranchCode = new DataSet();
                dsBranchCode = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetBranchCode", iHODealer);
                return dsBranchCode;
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

        public DataTable GetDealerLocationType(int iHOBranch)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtDealerLocationType = new DataTable();
                dtDealerLocationType = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealerLocationType", iHOBranch);
                return dtDealerLocationType;
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

        public DataSet GetHOBranchData(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsHOBranchData = new DataSet();
                dsHOBranchData = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHOBranchData", iID);
                return dsHOBranchData;
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
