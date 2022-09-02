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
    /// Summary description for clsDealerUtility
    /// </summary>
    public class clsDealerUtility
    {
        public clsDealerUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet FillPatchDeatils(string sPatchName, string sPatchStatus)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityFillPatchDeatils", sPatchName, sPatchStatus);
                return dtFillPatchDeatils;
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
        public DataSet GetDeatlerDeatilsforLive(int iDealerID, int iDealerLocationTypeID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtDeatilsforLive = new DataSet();
                dtDeatilsforLive = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityAllDealers", iDealerID, iDealerLocationTypeID);
                return dtDeatilsforLive;
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
        public bool PatchValidation(string sPatchName)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityPatchValidation", sPatchName);
                if (dtFillPatchDeatils.Tables[0].Rows[0][0].ToString() == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool bSavePatchDeatils(string strPatchDetails, DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    string sFinYear = Func.sGetFinancialYear(Func.Convert.iConvertToInt(dtDet.Rows[0]["DealerID"]));                    
                    objDB.BeginTranasaction();
                    foreach (DataRow dr in dtDet.Rows)
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityPatchDeatils", dr["strPatchName"], dr["iNoFiles"], (dr["DealerID"]), dr["DealerType"], dr["EnteryAdd"], dr["DownloadStatus"], strPatchDetails);
                    }
                    objDB.CommitTransaction();
                }
                return true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public bool bSaveDealerUtilityLiveStaus(int iDealerID, int iDealerLocationTypeID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityLiveStaus", iDealerID, iDealerLocationTypeID);
                objDB.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public bool bSaveDealerUtilityRollloutDatUploadedONFTP(int iDealerID, int iDealerLocationTypeID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityRolloutDataStaus", iDealerID, iDealerLocationTypeID);
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
        public bool bSaveDealerUtilityBranchCodeUpdate(int iDealerID, int iDealerLocationTypeID, string sBrnachCode)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityBranchCodeUpdate", iDealerID, iDealerLocationTypeID, sBrnachCode);
                objDB.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                throw ex;
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public bool bSaveDealerUtilityPartialyLiveStaus(int iDealerID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityPartialyLiveStaus", iDealerID);
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
        public bool bDealerLocationTypeValidation(int iDealerID, string slocationType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityLocationTypeValidation", iDealerID, slocationType);
                if (dtFillPatchDeatils.Tables[0].Rows[0][0].ToString() == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public int bSaveDealerUtilityDealerLocationType(int iDealerID, string sLocType, string sLocTypeDesc, string sDealerBranchCode, int iRolloutPatchID)
        {

            int iID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityLocationType", iDealerID, sLocType, sLocTypeDesc, sDealerBranchCode, iRolloutPatchID);
                objDB.CommitTransaction();
                return iID;
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
        public bool bDealeridPresent(int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityDealerIDExists", iDealerID);
                if (dtFillPatchDeatils.Tables[0].Rows[0][0].ToString() == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool bDealerHOBranchPresent(int iHODealerID, int iDealerID, string sHOBranchCode)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityDealerHOBranchPresent", iHODealerID, iDealerID, sHOBranchCode);
                if (dtFillPatchDeatils.Tables[0].Rows[0][0].ToString() == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool bDealerHOPresent(int iHODealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUtilityDealerHOPresent", iHODealerID);
                if (dtFillPatchDeatils.Tables[0].Rows[0][0].ToString() == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public bool bSaveDealerUtilityDealerHOBranchEntry(int iHODealerID, int iDealerID, string sHOBranchCode)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDealerUtilityHOBranchEntry", iHODealerID, iDealerID, sHOBranchCode);
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

        public DataSet FillSQLJobDeatils()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtFillPatchDeatils = new DataSet();
                dtFillPatchDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSQLJobDeatils");
                return dtFillPatchDeatils;
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
        public bool bUpdateSqlJobStatus(string strJob_Name, int iEnabled)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_UpdateSQLJobStatus", strJob_Name, iEnabled);
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
        public bool bDealerUtilityLiveDealerSpecificXMLGen()
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.ExecuteStoredProcedure("SP_DealerLiveSpecificXMLGen");
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
    }
}
