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

namespace MANART.Forms.Warranty
{
   public partial class frmFPDAFORReceipt : System.Web.UI.Page   
    {
        int iUserId = 0;
        int iUserRoleId = 0;
        int iUserPCRHeadApprID = 0;
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
                objCustomPager.PageSize = WarrantyClaimGrid.PageSize;

                ClientScript.RegisterClientScriptInclude(this.GetType(), "myScript", "../../CSS/jquery.datepick.css");
                txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);

                if (Request.Form[hidSourceID.UniqueID] != null &&
                    Request.Form[hidSourceID.UniqueID] != string.Empty)
                {
                    controlName = Request.Form[hidSourceID.UniqueID];
                }

                //Response .Redirect ("TestSample3.aspx");
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetUserPCRHeadApprv, 0, "")));               
                iUserId = (iUserId == iUserPCRHeadApprID) ? 62 : iUserId;
                
                int iMenuId = 0;               
                Location.bUseSpareDealerCode = true;               
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                iUserRoleId = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 1 : iUserRoleId;
                                
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
                    //Func.Common.BindDataToCombo(drpClaimStatus, clsCommon.ComboQueryType.WarrantyClaims, 0, "");                    
                    drpClaimStatus.SelectedValue = "1";                    
                    DisplayData(sDealerIds, Func.Convert.iConvertToInt(0));                   
                    lblClr2.Text = "";

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
            lblTitle.Text = "FPDA Receipt";            
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            //Session["Pending"] = "N";
            objCustomPager.CurrentIndex = 1;
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
        }
        // To Display Data
        private void DisplayData(string sDealerId, int iClaimStatus)
        {
            ClsFPDA objFPDA = new ClsFPDA();
            lblMessage.Visible = false;
            DataSet dsClaim = new DataSet();
            string sCurrDate = Func.Common.sGetCurrentDate();
            if (drpClaimStatus.SelectedValue != "1")
            {
                if (txtFromDate.Text == "")
                {
                    //txtFromDate.Text = Func.Common.sGetMonthStartDate(sCurrDate);
                    txtFromDate.Text = "01/01/2016";//As per discuss with DeeptiMam as on 01/01/2012,Only for 'Pending'.
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = Func.Common.sGetMonthEndDate(sCurrDate);
                }
            }
            else
            {
                //txtFromDate.Text = "01/01/" + DateTime .Now.Year;
                txtFromDate.Text = "01/11/2016";//As per discuss with DeeptiMam as on 01/01/2012,Only for 'Pending'.
                txtToDate.Text = Func.Common.sGetMonthEndDate(sCurrDate);
            }
            iClaimStatus = Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue);
            dsClaim = objFPDA.GetFPDAForReceiptUserWise(Location.iRegionId.ToString(), Location.iCountryId.ToString(), (sDealerId == "") ? "0" : sDealerId, txtFromDate.Text, txtToDate.Text, iClaimStatus, (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 1 : iUserRoleId, sDomestic_Export, txtSearchText.Text.Trim() , objCustomPager.CurrentIndex, objCustomPager.PageSize, "0", "0", iUserId, 0);
            
            DataView dvDetails = new DataView();
            if (dsClaim != null)
            {
               

                dvDetails = dsClaim.Tables[0].DefaultView;
                


                WarrantyClaimGrid.DataSource = dvDetails.ToTable();
                WarrantyClaimGrid.DataBind();
               

                if (dvDetails.ToTable() == null || dsClaim.Tables.Count == 0 || dvDetails.ToTable().Rows.Count == 0)
                {
                    lblMessage.Text = " Records Does Not Exists !";
                    lblMessage.Visible = true;
                
                    WarrantyClaimGrid.DataSource = null;
                    WarrantyClaimGrid.DataBind();
                    return;
                }
                SetControlPropertyOfGrid();
            }
            else if (dsClaim == null)
            {
                WarrantyClaimGrid.DataSource = dsClaim;
                WarrantyClaimGrid.DataBind();
            }
            objFPDA = null;

        }
        private void SetControlPropertyOfGrid()
        {
            for (int iRowcnt = 0; iRowcnt < WarrantyClaimGrid.Rows.Count; iRowcnt++)
            {
                Label lblDealerID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblDealerID");                
                Label lblID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblID");                

                ImageButton ImgSelect = (ImageButton)WarrantyClaimGrid.Rows[iRowcnt].FindControl("ImgSelect");
                ImgSelect.Attributes.Add("onclick", " return GetFPDADtls(this,'" + lblDealerID.Text.ToString() + "','" + lblID.Text.ToString() + "','" + txtDealerCode.Text + "')");
            }
        }

        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {          
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));           
        }

        protected void drpClaimType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchText.Text = "";
            objCustomPager.CurrentIndex = 1;
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            objCustomPager.CurrentIndex = 1;
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));

        }
        protected void WarrantyClaimGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            blbtnClick = true;
            WarrantyClaimGrid.PageIndex = e.NewPageIndex;
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
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
                e.InputParameters["iUserRoleId"] = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 1 : iUserRoleId;
                e.InputParameters["sDomestic_Export"] = sDomestic_Export;
                e.InputParameters["sSearchText"] = txtSearchText.Text;
                e.InputParameters["StartIndexRow"] = 0;
                e.InputParameters["MaxRowCount"] = 0;

            }
            // here controlName is a variable set in Page_Load event
            if (controlName != null)
            {
                // check if your search button was clicked
                if (controlName.Equals("ContentPlaceHolder1_btnSearch"))
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
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));

        }
        protected void WarrantyClaimGrid_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (objCustomPager.ItemCount == 0 || (objCustomPager.ItemCount <= WarrantyClaimGrid.PageSize))
                objCustomPager.Visible = false;
            else
                objCustomPager.Visible = true;
        }
 
    }
}