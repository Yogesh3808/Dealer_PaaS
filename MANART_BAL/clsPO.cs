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
    /// Summary description for clsPO
    /// </summary>
    public class clsPO
    {
        public clsPO()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        // To Get Indent Data For the Dealer
        public DataSet GetDealerPO(int iID, string sIndentType)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPO", iID, sIndentType);
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
    }
}
