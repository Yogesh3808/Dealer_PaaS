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
    /// Summary description for clsPickList
    /// </summary>
    public class clsPickList
    {
        public clsPickList()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get PickList Details
        /// </summary>
        /// <param name="iORFID"></param>
        /// <param name="sPickListType"></param>
        /// <returns></returns>

        public DataSet GetPickList(int iORFID, string sPickListType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPickList", iORFID, sPickListType);
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
        /// <summary>
        /// Intimation TO SAP For Create Invoice
        /// </summary>
        /// <param name="sORFNo"></param>
        public bool IntimationToSAPCreateInvoice(string sORFNo, string ORFDate)
        {
            return true;
        }
    }
}
