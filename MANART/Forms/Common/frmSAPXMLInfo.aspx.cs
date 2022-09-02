using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Common
{
    public partial class frmSAPXMLInfo : System.Web.UI.Page
    {
        clsDocument objclsDoc = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            try
            {

                objclsDoc = new clsDocument();
                if (0 == objclsDoc.PingXml(drpXML.SelectedValue.ToString()))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Xml is generated.');</script>");

                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Xml is not  generated.');</script>");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
    }
}