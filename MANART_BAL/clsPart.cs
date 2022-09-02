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
    /// Summary description for clsPart
    /// </summary>
    public class clsPart
    {
        public clsPart()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetPart(int PartID, string groupcode,int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_PartMaster", PartID, groupcode, DealerID);
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
        public DataSet GetShemeParts(int PartID, int PageSize, int pageIndex)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_ShemeParts", PartID, PageSize, pageIndex);
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
        public DataSet GetMaxPart(string groupcode)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxPart", groupcode);
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
        public DataSet PartRateMasterByState(int StateID, int PartID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_M_PartRateMasterByState", StateID, PartID);
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
        public DataSet PartRateMasterByCountry(int CountryID, int PartID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_M_PartRateMasterByCountry", CountryID, PartID);
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

        public int bSaveLocalPartMaster(int iID, string Location,int DealerID, string groupCode,string LocalPartName )
                        
        {

            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_LocalPartMaster",  iID, Location, DealerID,  groupCode, LocalPartName);


                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public int bSaveLocalPartMaster_ForMTI(int iID, string PartNo, string PartName,
                          string Unit, string Active, string MinOrderQty, string PartCategory)
        {

            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_LocalPartMaster_ForMTI", iID, PartNo, PartName,
                       Unit, Active, MinOrderQty, PartCategory);


                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


    }
}
