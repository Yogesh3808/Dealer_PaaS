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
    /// Summary description for clsModeofshipment
    /// </summary>
    public class clsModeofshipment
    {
        public clsModeofshipment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetModeofShipmentRecord(int iID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ModeofShipment_Master", iID);
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

        public int bSaveModeofShipment(int iID, int sDealer_Id, string svsType, string sModeofship, string sshipMode, string sactive)
        {

            bool bSaveRecord = false;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_ModeofShipment_Save", iID, sDealer_Id, svsType, sModeofship, sshipMode, sactive);

                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


    }
}
