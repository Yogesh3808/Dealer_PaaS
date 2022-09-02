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
    /// Summary description for clsSpareSchemeValue
    /// </summary>
    public class clsSpareSchemeValue
    {
        public clsSpareSchemeValue()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetSpareSchemeValue(string Heads, int SpareSchemeID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSpareSchemeValue", Heads, SpareSchemeID);
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
        public DataSet GetSpareSchemeName()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("select scheme_no as ID,scheme_desc as Name from M_SparesScheme");
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
        public int bSpareSchemeValue(int iID, DataTable dtSpareSchemeValueSave)
        {

            bool bSaveRecord = false;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_SpareSchemeValue_Save", dtSpareSchemeValueSave.Rows[0]["ID"], dtSpareSchemeValueSave.Rows[0]["Scheme_ID"], dtSpareSchemeValueSave.Rows[0]["From_Level1"], dtSpareSchemeValueSave.Rows[0]["To_Level1"], dtSpareSchemeValueSave.Rows[0]["Discount_Level1"], dtSpareSchemeValueSave.Rows[0]["From_Level2"], dtSpareSchemeValueSave.Rows[0]["To_Level2"], dtSpareSchemeValueSave.Rows[0]["Discount_Level2"], dtSpareSchemeValueSave.Rows[0]["From_Level3"], dtSpareSchemeValueSave.Rows[0]["To_Level3"], dtSpareSchemeValueSave.Rows[0]["Discount_Level3"]);
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
