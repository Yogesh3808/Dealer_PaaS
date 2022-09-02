using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Admin
{
    public partial class frmWarrantyLeaveMgmt : System.Web.UI.Page
    {
        int iUserId = 0;
        private int UsreType;
        DataSet ds = new DataSet();
        DataSet dsCurrentUser = new DataSet();
        DataSet dsAllUser = new DataSet();
        DataSet dsValidate = new DataSet();
        string sMessage = "";
        private int iID;
        string IsConfirm;
        clsWarrantyLeaveMgmt objWarrantyLeaveMgmt = new clsWarrantyLeaveMgmt();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                UsreType = Func.Convert.iConvertToInt(Session["UserType"]);
                // FillGridDetails();
                FillCombo();

            }
            FillSelectionGrid();
        }
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("CurrentUser");
                SearchGrid.AddToSearchCombo("NewUser");
                SearchGrid.AddToSearchCombo("FromDate");
                SearchGrid.AddToSearchCombo("ToDate");
                SearchGrid.AddToSearchCombo("Remark");
                SearchGrid.sModelPart = "";
                SearchGrid.iDealerID = -1;
                SearchGrid.sSqlFor = "LeaveMgmt";
                SearchGrid.sGridPanelTitle = "Leave Details";
                //SearchGrid.bIsCallForServer = true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
                iID = Func.Convert.iConvertToInt(txtID.Text);

                // ViewState["iID"] = iID; 
                GetDataAndDisplayInDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void GetDataAndDisplayInDetails()
        {
            try
            {
                DataSet ds = new DataSet();


                if (iID != 0)
                {
                    //ViewState["iID"] = iID;
                    ds = objWarrantyLeaveMgmt.GetDataForGridLeaveDetails(iID);
                    DisplayData(ds);
                    objWarrantyLeaveMgmt = null;
                }
                else
                {
                    ds = null;
                    objWarrantyLeaveMgmt = null;

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void DisplayData(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpCurrentUser.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CurrentUserID"]);
                drpAssignUser.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NewUserID"]);
                txtFromDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FromDate"]);
                txtToDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ToDate"]);
                txtRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remark"]);
                IsConfirm = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsConfirm"]);
                if (IsConfirm == "Y")
                {
                    EnableControl(IsConfirm);
                }
                else
                {
                    EnableControl(IsConfirm);
                }

            }

        }
        private void FillGridDetails()
        {
            //DataSet DsGridFill = new DataSet();
            //DsGridFill = objWarrantyLeaveMgmt.GetDataForGridLeaveDetails();
            //if (DsGridFill != null)
            //{
            //    gvLeaveGrid.DataSource = DsGridFill.Tables[0];
            //    gvLeaveGrid.DataBind();  
            //}


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtFromDate.sOnBlurScript = " return SetFutureDate(ContentPlaceHolder1_txtFromDate_txtDocDate,'From Date Should be Greater than Current Date');";
            //txtToDate.sOnBlurScript = " return CheckDateLess(ContentPlaceHolder1_txtToDate_txtDocDate,ContentPlaceHolder1_txtFromDate_txtDocDate,'To Date should be Greater than From Date');";
            txtFromDate.Attributes.Add("onchange", " bCheckFromDateIsGreaterThanToCurrentDate(this)");
            Location.bHideDealerDetails = true;
            btnSave.Attributes.Add("onClick", "Validation()");
            btnConfirm.Attributes.Add("onClick", "Validation()");

        }
        private void FillCombo()
        {

            dsCurrentUser = objWarrantyLeaveMgmt.GetDataForCurrentUser(iUserId, UsreType);
            if (dsCurrentUser != null)
            {
                drpCurrentUser.DataSource = dsCurrentUser.Tables[0];
                drpCurrentUser.DataTextField = "Name";
                drpCurrentUser.DataValueField = "ID";
                drpCurrentUser.DataBind();
                drpCurrentUser.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            dsAllUser = objWarrantyLeaveMgmt.GetDataForAllUser(UsreType);
            if (dsAllUser != null)
            {
                drpAssignUser.DataSource = dsAllUser.Tables[0];
                drpAssignUser.DataTextField = "Name";
                drpAssignUser.DataValueField = "ID";
                drpAssignUser.DataBind();
                drpAssignUser.Items.Insert(0, new ListItem("--Select--", "0"));
            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int CurrentUserId = Func.Convert.iConvertToInt(drpCurrentUser.SelectedItem.Value);
            int AllAssignUserId = Func.Convert.iConvertToInt(drpAssignUser.SelectedItem.Value);
            //dsValidate = objWarrantyLeaveMgmt.GetDataForValidateLeaveDetails(iUserId,txtFromDate.Text,txtToDate.Text,drpCurrentUser.SelectedItem.Value,drpAssignUser.SelectedItem.Value);
            int flage = objWarrantyLeaveMgmt.GetDataForValidateLeaveDetails(iUserId, txtFromDate.Text, txtToDate.Text, CurrentUserId, AllAssignUserId);
            if (flage == 1)
            {
                sMessage = "This Record Is already Exist";
                Page.RegisterClientScriptBlock("close", "<script languange='javaScript'>alert('" + sMessage + ".');</script>");
                return;
            }
            else
            {

                IsConfirm = "N";
                iID = objWarrantyLeaveMgmt.bSaveLeaveDetails(iID, CurrentUserId, AllAssignUserId, txtFromDate.Text, txtToDate.Text, txtRemark.Text, iUserId, IsConfirm);

                if (iID > 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    FillSelectionGrid();
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }

            }

        }
        private void clearControl()
        {
            drpCurrentUser.SelectedItem.Value = "0";
            drpAssignUser.SelectedItem.Value = "0";
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = "31/12/9999";
            txtRemark.Text = "";

        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            clearControl();

        }
        private void EnableControl(string IsConfirm)
        {
            if (IsConfirm == "Y")
            {
                drpCurrentUser.Enabled = false;
                drpAssignUser.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtRemark.Enabled = false;
                btnSave.Enabled = false;
                btnConfirm.Enabled = false;

            }
            else
            {

                drpCurrentUser.Enabled = true;
                drpAssignUser.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtRemark.Enabled = true;
                btnSave.Enabled = true;
                btnConfirm.Enabled = true;

            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int CurrentUserId = Func.Convert.iConvertToInt(drpCurrentUser.SelectedItem.Value);
            int AllAssignUserId = Func.Convert.iConvertToInt(drpAssignUser.SelectedItem.Value);
            IsConfirm = "Y";
            iID = objWarrantyLeaveMgmt.bSaveLeaveDetails(iID, CurrentUserId, AllAssignUserId, txtFromDate.Text, txtToDate.Text, txtRemark.Text, iUserId, IsConfirm);
            if (iID > 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                EnableControl("Y");
                FillSelectionGrid();

            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
            }
        }
    }
}