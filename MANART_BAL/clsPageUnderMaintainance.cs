using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Data.SqlClient;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsPageUnderMaintainance
    /// </summary>
    public class clsPageUnderMaintainance
    {
        public clsPageUnderMaintainance()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetMaintainance(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSMaintainance = new DataSet();
                DSMaintainance = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaintainance", ID);
                return DSMaintainance;
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
        public DataSet GetSiteMaintainance()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSSiteMaintainance = new DataSet();
                DSSiteMaintainance = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSiteMaintainance", 0);
                return DSSiteMaintainance;
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
        public bool bSaveMaintainance(DataTable dtHdr)
        {


            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtHdr.Rows.Count != 0)
                {
                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_SaveMaintainance", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["StartTime"], dtHdr.Rows[0]["EndTime"], dtHdr.Rows[0]["DMsgTime"], dtHdr.Rows[0]["Region"]);
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
    }
}
