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
namespace MANART.Forms.Common
{
    public partial class frmJobcardSelection : System.Web.UI.Page
    {
        private string sSelText, sSelType, sDocFormat, sClmInvType;
        private int iClaimtype;
        private int iHOBrID, iDealerID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ExpirePageCache();
            DisplayData();

        }
        private void DisplayData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;

                sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                sSelText = txtSearch.Text.ToString();
                iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                iClaimtype = Func.Convert.iConvertToInt(Request.QueryString["sDocType"].ToString());
                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBrID"].ToString());
                sDocFormat = Func.Convert.sConvertToString(Request.QueryString["sDocFormat"].ToString());                
                if (!IsPostBack)
                {
                    lblTitle.Text = (sDocFormat == "JbInv" || sDocFormat == "JbInvP" || sDocFormat == "JbInvL") ? "Jobcard Selection to create Jobcard Invoice" : (sDocFormat == "Claim") ? "Jobcard Selection to create Warranty Claim" : "Jobcard Selection to ";
                }
                if (sDocFormat == "Claim")
                {
                    sDocFormat = sDocFormat + Func.Convert.sConvertToString(Request.QueryString["ClmInvType"].ToString());
                }

                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_JobcardSelection", sSelText, iClaimtype, iDealerID, iHOBrID, sDocFormat);
                ViewState["Chassis"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                //// if (DdlSelctionCriteria.SelectedValue == "Chassis_no" && txtSearch.Text != "")
                //dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                //ChassisGrid.DataSource = dvDetails;
                ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" && btnSearch.Text == "Search")
                btnSearch.Text = "ClearSearch";
            else if (txtSearch.Text != "" && btnSearch.Text == "ClearSearch")
            {
                txtSearch.Text = "";
                btnSearch.Text = "Search";
            }
            DisplayData();
        }

        protected void ChassisGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ChassisGrid.PageIndex = e.NewPageIndex;
            DisplayData();
        }    
    }
}