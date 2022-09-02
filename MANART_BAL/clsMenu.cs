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
using MANART_BAL;
using MANART_DAL;

namespace MANART_BAL
{
   public class clsMenu
    {
        public clsMenu()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet GetMenuData(int UserType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet datasetMenu = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuData", UserType);
                //datasetMenu.Relations.Add("Child", datasetMenu.Tables[0].Columns["ID"], datasetMenu.Tables[1].Columns["Parent"]);
                return datasetMenu;
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
