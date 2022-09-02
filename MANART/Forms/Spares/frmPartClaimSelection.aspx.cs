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

namespace MANART.Forms.Spares
{
    public partial class frmPartClaimSelection : System.Web.UI.Page
    {
        int iUserType = 0;
        int iUserId = 0;
        int iUserRoleId;
        string sDealerIds = "";
        int iRejectApprovedCnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                Location.bUseSpareDealerCode = true;

                if (drpPartClaimStatus.SelectedValue == "N")
                    trDate.Style.Add("display", "none");
                else
                {
                    trDate.Style.Add("display", "");
                    //txtFromDate.sOnBlurScript = " return CheckDateLess(ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate,ctl00_ContentPlaceHolder1_txtToDate_txtDocDate,'From Date should be Less than To Date');";
                    //txtToDate.sOnBlurScript = " return CheckDateGreater(ctl00_ContentPlaceHolder1_txtToDate_txtDocDate,ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate,'To Date should be Greater than From Date');";
                }
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                txtRoleID.Text = iUserRoleId.ToString();
                //Vikram Dated 15122017
                //if (iUserRoleId == 2)
                //{
                //    //RFQSelection.Style.Add("display", "none");
                //    drpPartClaimStatus.SelectedValue = "R";

                //}
                //else
                //{
                //    RFQSelection.Style.Add("display", "");
                //}
                  
                if (Location.sDealerId == "")
                    sDealerIds = Func.Common.GetDealersByUserID(iUserId);
                else
                    sDealerIds = Location.sDealerId;
                if (!IsPostBack)
                {
                    Func.Common.BindDataToCombo(drpClaimType, clsCommon.ComboQueryType.PartClaimType, 0);
                    drpClaimType.Items.Insert(1, new ListItem("All", "-1"));
                    drpClaimType.SelectedIndex = 1;
                    trDate.Style.Add("display", "none");
                    FillSelectiongGrid(drpPartClaimStatus.SelectedValue.ToString(), Func.Convert.iConvertToInt(drpClaimType.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }


        private void FillSelectiongGrid(string sSelect, int iClaimType)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtDetails = null;
                clsPartClaim objPartClaim = new clsPartClaim();


                string sCurrDate = Func.Common.sGetCurrentDate();
                if (drpPartClaimStatus.SelectedValue != "N")
                {
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = "01/01/" + DateTime.Now.Year; ;//Func.Common.sGetMonthStartDate(sCurrDate);
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = Func.Common.sGetMonthEndDate(sCurrDate);
                    }
                }
                else
                {
                    txtFromDate.Text = "01/01/" + DateTime.Now.Year; ;
                    txtToDate.Text = Func.Common.sGetMonthEndDate(sCurrDate);
                }

                ds = objPartClaim.GetPartClaim(sDealerIds, "SelectionGrid", sSelect, iClaimType, iUserRoleId, txtFromDate.Text, txtToDate.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //string sCurrDate = Func.Common.sGetCurrentDate();
                        //if (drpPartClaimStatus.SelectedValue != "N")
                        //{
                        //    if (txtFromDate.Text == "")
                        //    {
                        //        txtFromDate.Text = "01/01/" + DateTime .Now.Year;;//Func.Common.sGetMonthStartDate(sCurrDate);
                        //    }
                        //    if (txtToDate.Text == "")
                        //    {
                        //        txtToDate.Text = "31/12/9999";//Func.Common.sGetMonthEndDate(sCurrDate);
                        //    }
                        //}
                        //else
                        //{
                        //    txtFromDate.Text = "01/01/" + DateTime .Now.Year;;
                        //    txtToDate.Text = "31/12/9999";//Func.Common.sGetMonthEndDate(sCurrDate);
                        //}

                        ViewState["sortExpr"] = ds.Tables[0];
                        

                        //string[] sStartDate = txtFromDate.Text.Split('/');
                        //string[] sEndDate = txtToDate.Text.Split('/');

                        //DateTime dtStart = new DateTime(Func.Convert.iConvertToInt(sStartDate[2]), Func.Convert.iConvertToInt(sStartDate[1]), Func.Convert.iConvertToInt(sStartDate[0]));
                        //DateTime dtEnd = new DateTime(Func.Convert.iConvertToInt(sEndDate[2]), Func.Convert.iConvertToInt(sEndDate[1]), Func.Convert.iConvertToInt(sEndDate[0]));

                        //Vikram 
                        dtDetails = ds.Tables[0];
                        DataView dvDetails = new DataView();
                        //dvDetails = dtDetails.DefaultView;
                        dvDetails = ds.Tables[0].DefaultView;



                        if (txtSearchText.Text.Trim() != "")
                            dvDetails.RowFilter = "part_claim_no Like '" + txtSearchText.Text.Trim() + "' or Inv_no like '" + txtSearchText.Text.Trim() + "'";
                        //dvDetails.RowFilter = "claim_date>" + txtFromDate.Text + "AND claim_date<" + txtToDate.Text;

                        GridParClaim.DataSource = dvDetails;
                        GridParClaim.DataBind();
                        txtFlage.Text = sSelect;

                        //if ((sSelect == "Y" || sSelect == "R" || iRejectApprovedCnt == dvDetails.Table.Rows.Count) || iUserRoleId == 12)
                        //    btnSendPartClaimTo3PL.Style.Add("display", "none");
                        //else
                        //    btnSendPartClaimTo3PL.Style.Add("display", "");
                        lblMessage.Text = "";
                    }
                    else
                    {
                        //btnSendPartClaimTo3PL.Style.Add("display", "none");
                        GridParClaim.DataSource = null;
                        GridParClaim.DataBind();
                        lblMessage.Text = "Part Claim Does Not Exist!";
                    }
                }
                else
                {
                    GridParClaim.DataSource = null;
                    GridParClaim.DataBind();
                    lblMessage.Text = "Part Claim Does Not Exist!";
                }
                objPartClaim = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        protected void btnClaimRequest_Click(object sender, EventArgs e)
        {
            try
            {
                string sSelect = "";
                int iClaimType = 0;
                iClaimType = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                sSelect = drpPartClaimStatus.SelectedValue.ToString();
                FillSelectiongGrid(sSelect, iClaimType);
                ViewState["CHECKED_ITEMS"] = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
      
        protected void GridParClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridParClaim.DataSource = (DataTable)ViewState["sortExpr"];
                GridParClaim.PageIndex = e.NewPageIndex;
                //Vikram 1512
                //RememberOldValues();
                GridParClaim.DataBind();
                // DisplayFirstREcord();
               // RePopulateValues();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sSelect = "";
                int iClaimType = 0;
                iClaimType = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                sSelect = drpPartClaimStatus.SelectedValue.ToString();
                FillSelectiongGrid(sSelect, iClaimType);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sSelect = "";
                int iClaimType = 0;
                iClaimType = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                sSelect = drpPartClaimStatus.SelectedValue.ToString();
                FillSelectiongGrid(sSelect, iClaimType);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void RememberOldValues()
        {
            ArrayList categoryIDList = new ArrayList();
            int index = -1;
            foreach (GridViewRow row in GridParClaim.Rows)
            {
                index = int.Parse(((Label)row.FindControl("lblNo")).Text);
                bool result = ((CheckBox)row.FindControl("ChkSelect")).Checked;
                // Check in the Session
                if (ViewState["CHECKED_ITEMS"] != null)
                    categoryIDList = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (result)
                {
                    if (!categoryIDList.Contains(index))
                        categoryIDList.Add(index);
                }
                else
                    categoryIDList.Remove(index);
            }
            if (categoryIDList != null && categoryIDList.Count > 0)
                ViewState["CHECKED_ITEMS"] = categoryIDList;
        }

        private void RePopulateValues()
        {
            ArrayList categoryIDList = (ArrayList)ViewState["CHECKED_ITEMS"];
            if (categoryIDList != null && categoryIDList.Count > 0)
            {
                foreach (GridViewRow row in GridParClaim.Rows)
                {
                    int index = int.Parse(((Label)row.FindControl("lblNo")).Text);
                    CheckBox myCheckBox = (CheckBox)row.FindControl("ChkSelect");

                    if (categoryIDList.Contains(index))
                    {
                        myCheckBox.Checked = true;
                    }
                    else
                        myCheckBox.Checked = false;
                }
            }
        }

        protected void btnSendPartClaimTo3PL_Click(object sender, EventArgs e)
        {
            try
            {
                clsPartClaim objPartClaim = new clsPartClaim();
                bool bSaveRecord = false;
                clsDA ObjDA = new clsDA();
                bSaveRecord = objPartClaim.bSavePartclaimT03PL(hdnRememberValue.Value);
                if (bSaveRecord == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    string sSelect = "";
                    int iClaimType = 0;
                    iClaimType = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                    sSelect = drpPartClaimStatus.SelectedValue.ToString();
                    FillSelectiongGrid(sSelect, iClaimType);
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void GridParClaim_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Vikram Dated 15122017
            //if (iUserRoleId == 11 || iUserRoleId == 4)
            //{
            //    if (((Label)e.Row.FindControl("lblIs3PL")) != null)
            //        if (((Label)e.Row.FindControl("lblIs3PL")).Text == "P")
            //            ((CheckBox)e.Row.FindControl("chkSelect")).Enabled = false;

            //    if (((Label)e.Row.FindControl("lblStatus")) != null)
            //        if (((Label)e.Row.FindControl("lblStatus")).Text != "N")
            //        {
            //            ((CheckBox)e.Row.FindControl("chkSelect")).Enabled = false;
            //            iRejectApprovedCnt = iRejectApprovedCnt + 1;
            //        }

            //}
            //else if (iUserRoleId == 12)
            //{
            //    for (int iCnt = 0; iCnt < GridParClaim.Columns.Count; iCnt++)
            //        if (GridParClaim.Columns[iCnt].HeaderText == "Select Claim")
            //            GridParClaim.Columns[iCnt].Visible = false;
            //}

        }
    }
}