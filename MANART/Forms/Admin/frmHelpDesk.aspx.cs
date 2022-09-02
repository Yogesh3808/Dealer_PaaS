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

namespace MANART.Forms.Admin
{
    public partial class frmHelpDesk : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            txtSearch.Text = "";
            lblConfirm.Text = "";
            dv = bindGrid();
            SelectionGrid.DataSource = dv;
            SelectionGrid.DataBind();
        }

        private DataView bindGrid()
        {
            try
            {
                clsUser objUser = new clsUser();
                //DataSet ds = new DataSet();
                ds = objUser.GetUserDetails("Helpdesk", Func.Convert.sConvertToString(Session["LoginName"]));
                if (ViewState["sortExpr"] != null)
                {
                    dv = new DataView(ds.Tables[0]);
                    dv.Sort = (string)ViewState["sortExpr"];
                }
                else
                    dv = ds.Tables[0].DefaultView;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dv;


        }
        protected void SelectionGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SetStatus();
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
        protected void SelectionGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }
        protected void UpdateStatus(object s, System.EventArgs e)
        {
            clsUser objUser = new clsUser();
            try
            {
                //Binds the grid so that change can be seen 
                string desc = "Activate/Deactivate User";
                HtmlAnchor UpdateBLockLnk = (HtmlAnchor)s;

                //Retrive the ProductID from the HRef attribute
                int UserID = Func.Convert.iConvertToInt(UpdateBLockLnk.HRef);
                //Response.Write(UpdateID);
                Boolean isBLocked = objUser.GetStatusForUser("Active", UserID);
                if (isBLocked == true)
                {
                    objUser.UpdateStatusForUser("Active", UserID, false);

                }
                else
                {
                    objUser.UpdateStatusForUser("Active", UserID, true);


                }

                if (txtSearch.Text != "")
                    SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString());
                else
                    BindGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void UpdateBLockedStatus(object s, System.EventArgs e)
        {
            clsUser objUser = new clsUser();
            try
            {
                //Binds the grid so that change can be seen 
                string desc = "BLock/UnBLock User";
                HtmlAnchor UpdateBLockLnk = (HtmlAnchor)s;

                //Retrive the ProductID from the HRef attribute
                int UserID = Func.Convert.iConvertToInt(UpdateBLockLnk.HRef);
                //Response.Write(UpdateID);
                Boolean isBLocked = objUser.GetStatusForUser("Block", UserID);
                if (isBLocked == true)
                {
                    objUser.UpdateStatusForUser("Block", UserID, false);

                }
                else
                {
                    objUser.UpdateStatusForUser("Block", UserID, true);


                }

                if (txtSearch.Text != "")
                    SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString());
                else
                    BindGrid();
            }
            catch (Exception ex)
            {
                //string errer = ex.Message;
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void UpdateLockStatus(object s, System.EventArgs e)
        {
            clsUser objUser = new clsUser();
            try
            {
                //Binds the grid so that change can be seen 
                string desc = "Lock/UnLock User";
                HtmlAnchor UpdateBLockLnk = (HtmlAnchor)s;

                //Retrive the ProductID from the HRef attribute
                int UserID = Func.Convert.iConvertToInt(UpdateBLockLnk.HRef);
                //Response.Write(UpdateID);
                Boolean isBLocked = objUser.GetStatusForUser("Lock", UserID);
                if (isBLocked == true)
                {
                    objUser.UpdateStatusForUser("Lock", UserID, false);

                }
                else
                {
                    objUser.UpdateStatusForUser("Lock", UserID, true);


                }


                if (txtSearch.Text != "")
                    SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString());
                else
                    BindGrid();
            }
            catch (Exception ex)
            {
                //string errer = ex.Message;
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void SetStatus()
        {
            try
            {
                GridViewRowCollection rowCollection = SelectionGrid.Rows;
                foreach (GridViewRow gridRow in rowCollection)
                {
                    Label lblActs = (Label)gridRow.FindControl("lblActs");
                    Label lblStatus = (Label)gridRow.FindControl("lblStatus");
                    Label lblPermissions = (Label)gridRow.FindControl("lblPermissions");
                    Label lblUserType = (Label)gridRow.FindControl("lblUserType");                    

                    HtmlAnchor lnkLockUpdate = (HtmlAnchor)gridRow.FindControl("lnkLockUpdate");
                    HtmlAnchor lnkBLockUpdate = (HtmlAnchor)gridRow.FindControl("lnkBLockUpdate");
                    HtmlAnchor lnkUpdate = (HtmlAnchor)gridRow.FindControl("lnkUpdate");

                    //Added by virendra for password policy
                    Label lblActs2 = (Label)gridRow.FindControl("lblActs2");
                    Label lblActs3 = (Label)gridRow.FindControl("lblActs3");
                    Label lblBLockStatus = (Label)gridRow.FindControl("lblBLockStatus");
                    Label lblLockStatus = (Label)gridRow.FindControl("lblLockStatus");
                    if (lblBLockStatus.Text == "True")
                    {
                        lblActs3.Text = "Block";
                    }
                    else
                    {

                        lblActs3.Text = "UnBLock";
                    }
                    if (lblLockStatus.Text == "True")
                    {
                        //lblActs2.Text = "Unlock";
                        lblActs2.Text = "Lock";
                    }
                    else
                    {
                        //lblActs2.Text = "Lock";
                        lblActs2.Text = "Unlock";
                    }

                    //password policy ends here



                    if (lblStatus.Text == "True")
                    {
                        //lblActs.Text = "Deactivate";
                        lblActs.Text = "Activate";
                        lblPermissions.Visible = (lblUserType.Text.ToString() != "9") ? true : false;
                    }
                    else
                    {
                        //lblActs.Text = "Activate";
                        lblActs.Text = "Deactivate";
                        //lblPermissions.Text = "";
                        lblPermissions.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void SelectionGrid_OnSorting(Object sender, GridViewSortEventArgs e)
        {
            ViewState["sortExpr"] = e.SortExpression;
            SelectionGrid.DataSource = bindGrid();
            SelectionGrid.DataBind();
        }

        protected void SelectionGrid_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {

            SelectionGrid.PageIndex = e.NewPageIndex;
            if (txtSearch.Text != "")
                SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString());
            else
            {
                SelectionGrid.DataSource = bindGrid();
                SelectionGrid.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SelectionGrid.PageIndex = 0;
            SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString());
        }

        private void SearchText(string strSearchText, string strSearchOnField)
        {
            try
            {
                lblConfirm.Text = "";
                DataView dv = bindGrid();
                DataTable dt = new DataTable();
                string SearchExpression = null;

                if (!String.IsNullOrEmpty(strSearchText))
                {
                    SearchExpression = string.Format("{0} '{1}*'", SelectionGrid.SortExpression, strSearchText);

                }
                strSearchOnField = "[" + strSearchOnField + "]";
                dv.RowFilter = strSearchOnField + " like " + SearchExpression;
                dt = dv.ToTable();
                System.Threading.Thread.Sleep(2000);
                if (dv.Count > 0)
                {

                    SelectionGrid.DataSource = dv;
                    SelectionGrid.DataBind();

                    btnClearSearch.Visible = true;
                }
                else
                {
                    lblConfirm.Text = "Record Does Not Exist !";
                    SelectionGrid.DataSource = null;
                    SelectionGrid.DataBind();

                    btnClearSearch.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblConfirm.Text = "Record Does Not Exist !";
                SelectionGrid.DataSource = null;
                SelectionGrid.DataBind();

                btnClearSearch.Visible = true;
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            SelectionGrid.PageIndex = 0;
            BindGrid();
        }
    }
}