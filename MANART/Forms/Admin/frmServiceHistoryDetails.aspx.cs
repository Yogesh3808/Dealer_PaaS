using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
using System.Drawing;

namespace MANART.Forms.Admin
{
    public partial class frmServiceHistoryDetails : System.Web.UI.Page
    {

        int iDealerID = 0;
        string sJobCardNo = "";
        string DealerName = "";
        string ServiceType = "";
        string JobcardNo = "";
        string JobcardDate = "";
        string CouponNo = "";
        string Kms = "";
        string EngineHr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["DealerID"] != null && Request.QueryString["JobCardNo"] != null)
            {
                iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sJobCardNo = Func.Convert.sConvertToString(Request.QueryString["JobCardNo"]);
            }
            DisplayRecords(iDealerID, sJobCardNo);
        }
        private void DisplayRecords(int DealerID, string JobCardNo)
        {
            try
            {
                clsChassis objChassis = new clsChassis();
                DataSet ds = objChassis.GETServiceHistoryForService(DealerID, JobCardNo);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DetailsGrid.DataSource = ds.Tables[0];
                    DetailsGrid.DataBind();
                }
                else
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblDealerName = (Label)e.Row.FindControl("lblDealerName");
            Label lblServiceType = (Label)e.Row.FindControl("lblServiceType");
            Label lblJobcardNo = (Label)e.Row.FindControl("lblJobcardNo");
            Label lblJobcardDate = (Label)e.Row.FindControl("lblJobcardDate");
            Label lblCouponNo = (Label)e.Row.FindControl("lblCouponNo");
            Label lblKms = (Label)e.Row.FindControl("lblKms");
            Label lblEngineHr = (Label)e.Row.FindControl("lblEngineHr");


            if (lblDealerName != null)
                if (DealerName == lblDealerName.Text)
                {
                    DealerName = lblDealerName.Text;
                    lblDealerName.Text = "";
                }
                else
                    DealerName = lblDealerName.Text;

            if (lblServiceType != null)
                if (ServiceType == lblServiceType.Text)
                {
                    ServiceType = lblServiceType.Text;
                    lblServiceType.Text = "";
                }
                else
                    ServiceType = lblServiceType.Text;

            if (lblJobcardNo != null)
                if (JobcardNo == lblJobcardNo.Text)
                {
                    JobcardNo = lblJobcardNo.Text;
                    lblJobcardNo.Text = "";
                }
                else
                    JobcardNo = lblJobcardNo.Text;

            if (lblJobcardDate != null)
                if (JobcardDate == lblJobcardDate.Text)
                {
                    JobcardDate = lblJobcardDate.Text;
                    lblJobcardDate.Text = "";
                }
                else
                    JobcardDate = lblJobcardDate.Text;

            if (lblCouponNo != null)
                if (CouponNo == lblCouponNo.Text)
                {
                    CouponNo = lblCouponNo.Text;
                    lblCouponNo.Text = "";
                }
                else
                    CouponNo = lblCouponNo.Text;

            if (lblKms != null)
                if (Kms == lblKms.Text)
                {
                    Kms = lblKms.Text;
                    lblKms.Text = "";
                }
                else
                    Kms = lblKms.Text;

            if (lblEngineHr != null)
                if (EngineHr == lblEngineHr.Text)
                {
                    EngineHr = lblEngineHr.Text;
                    lblEngineHr.Text = "";
                }
                else
                    EngineHr = lblEngineHr.Text;

        }
    }
}