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
using System.IO;

namespace MANART.Forms.Coupon
{
    public partial class frmCouponClaimSelection : System.Web.UI.Page
    {
        int iUserId = 0;
        int iUserRoleId = 0;
        bool blbtnClick = false;
        clsWarranty.enmClaimType ValenmFormUsedFor;
        string sDomestic_Export = "";
        double dDomesticFirstSlabAmount = 0;
        string sDealerIds = "";
        string controlName = string.Empty;
        int StartIndexRow = 0;
        int MaximumRowCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                objCustomPager.PageSize = CouponClaimGrid.PageSize;

                ClientScript.RegisterClientScriptInclude(this.GetType(), "myScript", "../../Content/jquery.datepick.css");


                if (Request.Form[hidSourceID.UniqueID] != null &&
                    Request.Form[hidSourceID.UniqueID] != string.Empty)
                {
                    controlName = Request.Form[hidSourceID.UniqueID];
                }

                //Response .Redirect ("TestSample3.aspx");
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                int iMenuId = 0;
                // Megha 08072011
                Location.bUseSpareDealerCode = true;
                // Megha 08072011
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);




                PageItemCount(Func.Convert.iConvertToInt(hdnPagecount.Value));
                if (Location.sDealerId == "" && hdnDealers.Value == "")
                {
                    sDealerIds = Func.Common.GetDealersByUserID(iUserId);
                    hdnDealers.Value = sDealerIds;
                }
                else if (Location.sDealerId != "")
                {
                    sDealerIds = Location.sDealerId;
                    hdnDealers.Value = "";
                }
                else
                {
                    sDealerIds = hdnDealers.Value;
                }

                if (!IsPostBack)
                {
                    DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));

                }
                if (txtSearchText.Text != "")
                    btnClearSearch.Style.Add("display", "");
                else
                    btnClearSearch.Style.Add("display", "none");

                if (drpClaimStatus.SelectedValue == "1")
                    trDate.Style.Add("display", "none");
                else
                    trDate.Style.Add("display", "");
                SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void SetDocumentDetails()
        {

            lblTitle.Text = "Coupon Claim Processing";

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            //Session["Pending"] = "N";
            txtSearchText.Text = "";
            objCustomPager.CurrentIndex = 1;            
            DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));
        }
        // To Display Data
        private void DisplayData(string sDealerId, string iClaimStatus)
        {
            clsCouponClaim ObjCouponClaim = new clsCouponClaim();
            lblMessage.Visible = false;
            DataSet dsClaim = new DataSet();
            string sCurrDate = Func.Common.sGetCurrentDate();

            if (txtFromDate.Text == "")
            {

                txtFromDate.Text = "01/09/2016";
            }

            if (txtToDate.Text == "")
            {
                txtToDate.Text = Func.Common.sGetMonthEndDate(sCurrDate);
            }

            dsClaim = ObjCouponClaim.GetCouponClaimUserWise(Location.iRegionId.ToString(), Location.iCountryId.ToString(),
                (sDealerId == "") ? "0" : sDealerId, txtFromDate.Text, txtToDate.Text, iClaimStatus,
                iUserRoleId, txtSearchText.Text, iUserId);

            DataView dvDetails = new DataView();
            if (dsClaim != null)
            {
                //if (dsClaim.Tables[0].Rows.Count > 0)
                //    hdnPagecount.Value = Func.Convert.sConvertToString(dsClaim.Tables[0].Rows[0]["TotalRowCount"]);
                //else
                //    hdnPagecount.Value = "0";

                //PageItemCount(Func.Convert.iConvertToInt(hdnPagecount.Value));

                dvDetails = dsClaim.Tables[0].DefaultView;
                if (txtSearchText.Text != "")
                    dvDetails.RowFilter = "Coupon_Claim_no Like '" + txtSearchText.Text + "' or ClaimType= '" + txtSearchText.Text + "'";

                CouponClaimGrid.DataSource = dvDetails.ToTable();
                CouponClaimGrid.DataBind();


                if (dvDetails.ToTable() == null || dsClaim.Tables.Count == 0 || dvDetails.ToTable().Rows.Count == 0)
                {
                    lblMessage.Text = " Records Does Not Exists !";
                    lblMessage.Visible = true;

                    CouponClaimGrid.DataSource = null;
                    CouponClaimGrid.DataBind();
                    return;
                }

            }
            else if (dsClaim == null)
            {
                CouponClaimGrid.DataSource = dsClaim;
                CouponClaimGrid.DataBind();
            }
            ObjCouponClaim = null;

        }


        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            //if (Func.Convert.sConvertToString(Session["Pending"]) == "N")
            //{
            DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));
            //}
            //DisplayData();
        }

        protected void drpClaimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DisplayData(Location.sDealerId, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchText.Text = "";
            DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DisplayData(sDealerIds, "0");

        }
        protected void CouponClaimGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            CouponClaimGrid.PageIndex = e.NewPageIndex;
            DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));
        }

        protected void ObjDSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            blbtnClick = false;
            if (e.ExecutingSelectCount != true)
            {
                e.InputParameters["sRegionID"] = Location.iRegionId.ToString();
                e.InputParameters["sContryID"] = Location.iCountryId.ToString();
                e.InputParameters["sDealerID"] = ((sDealerIds == "") ? "0" : sDealerIds);
                e.InputParameters["sFromDate"] = txtFromDate.Text;
                e.InputParameters["sToDate"] = txtToDate.Text;
                e.InputParameters["sRequestOrClaim"] = "R";
                e.InputParameters["iClaimStatus"] = drpClaimStatus.SelectedValue;
                e.InputParameters["iUserRoleId"] = iUserRoleId;
                e.InputParameters["sDomestic_Export"] = sDomestic_Export;
                e.InputParameters["sSearchText"] = txtSearchText.Text;
                e.InputParameters["StartIndexRow"] = 0;
                e.InputParameters["MaxRowCount"] = 0;

            }
            // here controlName is a variable set in Page_Load event
            if (controlName != null)
            {
                // check if your search button was clicked
                if (controlName.Equals("ctl00_ContentPlaceHolder1_btnSearch"))
                {
                    // reset the startRowIndex to zero
                    // note that e.Arguments will work
                    // e.InputParameters will not work
                    e.Arguments.StartRowIndex = 0;
                    e.Arguments.MaximumRows = 10;
                }
            }
        }
        protected void objCustomPager_Command(object sender, CommandEventArgs e)
        {
            objCustomPager.CurrentIndex = Func.Convert.iConvertToInt(e.CommandArgument);
            StartIndexRow = objCustomPager.CurrentIndex;
            DisplayData(sDealerIds, Func.Convert.sConvertToString(drpClaimStatus.SelectedValue));

        }
        protected void CouponClaimGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drview = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNo = (Label)e.Row.FindControl("lblNo");
                lblNo.Text = Func.Convert.sConvertToString(e.Row.RowIndex + (objCustomPager.PageSize * (objCustomPager.CurrentIndex - 1) + 1));
            }
        }
        private void PageItemCount(int iValue)
        {
            objCustomPager.ItemCount = iValue;
            if (objCustomPager.ItemCount == 0 || (objCustomPager.ItemCount <= CouponClaimGrid.PageSize))
                objCustomPager.Visible = false;
            else
                objCustomPager.Visible = true;
        }


        protected void drpClaimStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}