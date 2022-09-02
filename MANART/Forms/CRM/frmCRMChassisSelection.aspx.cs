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

namespace MANART.Forms.CRM
{
    public partial class frmCRMChassisSelection : System.Web.UI.Page
    {
        private string sSelText, sSelType;
        private int CustID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ExpirePageCache();
            if (!IsPostBack)
            {
                lblTitle.Text = "Chassis List";
                DisplayData();
            }
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
                CustID = Func.Convert.iConvertToInt(Request.QueryString["CustID"].ToString());
              //  iJobtype = Func.Convert.iConvertToInt(Request.QueryString["JobTypeID"].ToString());
               // iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
              //  iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());

              

                //dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_CRM_ChassisSelection", CustID);
               // ViewState["Chassis"] = dtSrchgrid;
               // Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                 if (DdlSelctionCriteria.SelectedValue == "Chassis_no" && txtSearch.Text != "")
                dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                 if (DdlSelctionCriteria.SelectedValue == "Reg_no" && txtSearch.Text != "")
                     dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                 if (DdlSelctionCriteria.SelectedValue == "Customer_name" && txtSearch.Text != "")
                     dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                ChassisGrid.DataSource = dvDetails;
              //  ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

                if (ChassisGrid.Rows.Count == 0) return;

              

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