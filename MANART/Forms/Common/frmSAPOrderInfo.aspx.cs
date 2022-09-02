using System;
using System.Web.UI;
using MANART_BAL;
using MANART_DAL;
using System.Configuration;

namespace MANART.Forms.Common
{
    public partial class frmSAPOrderInfo : System.Web.UI.Page
    {

        int DealerID = 0;
        clsDocument objclsDoc = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["iDealerID"] != null)
                {
                    DealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["iDealerID"] != null)
                {
                    DealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }



        protected void btnRetrive_Click(object sender, EventArgs e)
        {
            try
            {

                objclsDoc = new clsDocument();
                if (0 == Func.Convert.iConvertToInt(objclsDoc.PingXml(drpXML.SelectedValue.ToString())))
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
            finally
            {
                if (objclsDoc != null) objclsDoc = null;
            }
        }





    }
}