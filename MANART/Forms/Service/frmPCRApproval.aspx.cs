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

namespace MANART.Forms.Service
{
    public partial class frmPCRApproval : System.Web.UI.Page
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
                //iUserId = (iUserId == 1001) ? 62 : iUserId;
                iUserId = (iUserId == iUserPCRHeadApprID) ? 62 : iUserId;
                
                int iMenuId = 0;
                // Megha 08072011
                Location.bUseSpareDealerCode = true;
                // Megha 08072011
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                iUserRoleId = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 1 : iUserRoleId;

                if (iMenuId == 408)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;
                    sDomestic_Export = "E";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END
                }
                else if (iMenuId == 412)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;
                    sDomestic_Export = "D";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END
                }
                else if (iMenuId == 409)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmHighValueClaim;//High Value Claim
                    sDomestic_Export = "E";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END
                }
                else if (iMenuId == 413)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmHighValueClaim;//High Value Claim
                    sDomestic_Export = "D";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END

                }
                else if (iMenuId == 113)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmNormal;
                    sDomestic_Export = "E";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END
                }
                else if (iMenuId == 341 || iMenuId == 498)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmNormal;
                    sDomestic_Export = "D";
                    //VHP_Warranty Start
                    Session["ClaimTypes"] = ValenmFormUsedFor;
                    //VHP_Warranty END

                }
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

                    //Session["Pending"] = "Y";
                    //Func.Common.BindDataToCombo(drpClaimStatus, clsCommon.ComboQueryType.WarrantyClaims, 0, " and ID <> 3");
                    Func.Common.BindDataToCombo(drpClaimStatus, clsCommon.ComboQueryType.WarrantyClaims, 0, "");
                    drpClaimStatus.SelectedValue = "1";

                    //Func.Common.BindDataToCombo(drpClaimType, clsCommon.ComboQueryType.ClaimType, 0, " AND Claim_Type='" + sDomestic_Export + "'");
                    //drpClaimType.Items.RemoveAt(0);
                    //drpClaimType.Items.Insert(0, new ListItem("All", "-1"));
                    //drpClaimType.SelectedValue = "-1";

                    //Func.Common.BindDataToCombo(drpWarrantyRole, clsCommon.ComboQueryType.WarrantyRole, 0);
                    //drpWarrantyRole.Items.RemoveAt(0);
                    //drpWarrantyRole.Items.Insert(0, new ListItem("All", "-1"));
                    //drpWarrantyRole.SelectedValue = "-1";

                    //Func.Common.BindDataToCombo(drpModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
                    //drpModelCategory.Items.RemoveAt(0);
                    //drpModelCategory.Items.Insert(0, new ListItem("All", "0"));
                    //drpModelCategory.SelectedValue = "0";

                    //drpWarrantyRole.SelectedValue = iUserRoleId != '0' ? Func.Convert.sConvertToString(iUserRoleId) : "-1";

                    //if (iUserRoleId != 18)
                    //{
                    //    drpWarrantyRole.SelectedValue = iUserRoleId != '0' ? Func.Convert.sConvertToString(iUserRoleId) : "-1";
                    //}
                    //else
                    //{
                    //    drpClaimType.SelectedValue = "10";
                    //    drpClaimType.Enabled = false;
                    //    drpWarrantyRole.Enabled = false;
                    //}

                    //DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
                    DisplayData(sDealerIds, Func.Convert.iConvertToInt(0));
                    if (sDomestic_Export == "D")
                        lblClr2.Text = "Claim Below 5000 INR";
                    else
                        lblClr2.Text = "Claim Below 5000 INR OR 100 USD";
                    lblClr2.Text = "";

                }

                //if (Location.sDealerId == "")
                //    sDealerIds = Func.Common.GetDealerList("WarrantyProcessing", iUserId, sDomestic_Export);
                //else
                //    sDealerIds = Location.sDealerId;



                //if (Func.Convert.sConvertToString(Session["Pending"]) == "Y")
                //{

                //    DisplayData(sDealerIds, 1);
                //}

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
            lblTitle.Text = "PCR Approval";

            //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            //{
            //    if (iUserRoleId != 18)
            //        lblTitle.Text = "Warranty Claim Processing";
            //    else
            //        lblTitle.Text = "Service Agreement Processing";
            //    txtRequestOrClaim.Text = "C";
            //}
            //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            //{
            //    lblTitle.Text = "Goodwill Request Approval";
            //    txtRequestOrClaim.Text = "R";
            //}
            //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            //{
            //    lblTitle.Text = "High Value Request Approval";
            //    txtRequestOrClaim.Text = "HR";
            //}
            //txtRequestOrClaim.Style.Add("display", "none");

            //if (iUserRoleId == 10 || Func.Convert.sConvertToString(Session["ClaimTypes"]) != "enmNormal")
            //    lblCommonMsg.Style.Add("display", "none");
            //else
            //    lblCommonMsg.Style.Add("display", "");
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
            clsJobcard objJobcard = new clsJobcard();
            lblMessage.Visible = false;
            DataSet dsClaim = new DataSet();
            string sCurrDate = Func.Common.sGetCurrentDate();
            if (drpClaimStatus.SelectedValue != "1")
            {
                if (txtFromDate.Text == "")
                {
                    //txtFromDate.Text = Func.Common.sGetMonthStartDate(sCurrDate);
                    txtFromDate.Text = "01/01/2011";//As per discuss with DeeptiMam as on 01/01/2012,Only for 'Pending'.
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
            //dsClaim = objJobcard.GetPCRForApprovalUserWise(Location.iRegionId.ToString(), Location.iCountryId.ToString(), (sDealerId == "") ? "0" : sDealerId, txtFromDate.Text, txtToDate.Text, iClaimStatus, iUserRoleId, sDomestic_Export, txtSearchText.Text, objCustomPager.CurrentIndex, objCustomPager.PageSize, drpWarrantyRole.SelectedValue, drpClaimType.SelectedValue, iUserId, Func.Convert.iConvertToInt(drpModelCategory.SelectedValue));
            dsClaim = objJobcard.GetPCRForApprovalUserWise(Location.iRegionId.ToString(), Location.iCountryId.ToString(), (sDealerId == "") ? "0" : sDealerId, txtFromDate.Text, txtToDate.Text, iClaimStatus, (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 1 : iUserRoleId, sDomestic_Export, txtSearchText.Text, objCustomPager.CurrentIndex, objCustomPager.PageSize, "0", "0", iUserId, 0);
            
            DataView dvDetails = new DataView();
            if (dsClaim != null)
            {
                //if (dsClaim.Tables[0].Rows.Count > 0)
                //    hdnPagecount.Value = Func.Convert.sConvertToString(dsClaim.Tables[0].Rows[0]["TotalRowCount"]);
                //else
                //    hdnPagecount.Value = "0";

                //PageItemCount(Func.Convert.iConvertToInt(hdnPagecount.Value));

                dvDetails = dsClaim.Tables[0].DefaultView;
                //if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text != "" && drpWarrantyRole.SelectedValue == "-1")
                //    dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "' AND Claim_no Like '*" + txtSearchText.Text + "*'" ;
                //else if (drpClaimType.SelectedValue == "-1" && txtSearchText.Text != "")
                //    dvDetails.RowFilter = "Claim_no Like '" + txtSearchText.Text + "'";
                //else if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text == "" && drpWarrantyRole.SelectedValue != "-1")
                //    dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "' AND Claim_Status LIKE '*" + drpWarrantyRole.SelectedItem.Text + "*'";
                //else if (drpClaimType.SelectedValue == "-1" && txtSearchText.Text == "" && drpWarrantyRole.SelectedValue != "-1")
                //    dvDetails.RowFilter = "Claim_Status LIKE '*" + drpWarrantyRole.SelectedItem.Text + "*'";

                ////if (txtSearchText.Text == "")
                ////{
                ////    if (drpClaimType.SelectedValue == "-1" && txtSearchText.Text == "" && drpWarrantyRole.SelectedValue != "-1")
                ////        dvDetails.RowFilter = "Claim_Status like '%" + drpWarrantyRole.SelectedItem.Text + "'";
                ////    //else if (drpClaimType.SelectedValue == "-1" && txtSearchText.Text != "" && drpWarrantyRole.SelectedValue == "-1")
                ////    //    dvDetails.RowFilter = "Claim_no = '" + txtSearchText.Text + "'";
                ////    //else if (drpClaimType.SelectedValue == "-1" && txtSearchText.Text != "" && drpWarrantyRole.SelectedValue != "-1")
                ////    //    dvDetails.RowFilter = "Claim_Status like '%" + drpWarrantyRole.SelectedItem.Text + "' AND Claim_no = '" + txtSearchText.Text + "'";
                ////    else if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text == "" && drpWarrantyRole.SelectedValue == "-1")
                ////        dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "'";
                ////    else if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text == "" && drpWarrantyRole.SelectedValue != "-1")
                ////        dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "' AND Claim_Status like '%" + drpWarrantyRole.SelectedItem.Text + "'";
                ////    //else if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text != "" && drpWarrantyRole.SelectedValue == "-1")
                ////    //    dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "' AND Claim_no = '" + txtSearchText.Text + "'";
                ////    //else if (drpClaimType.SelectedValue != "-1" && txtSearchText.Text != "" && drpWarrantyRole.SelectedValue != "-1")
                ////    //    dvDetails.RowFilter = "Claim_Type='" + drpClaimType.SelectedItem.Text + "' AND Claim_no = '" + txtSearchText.Text + "' AND Claim_Status like '%" + drpWarrantyRole.SelectedItem.Text + "'";

                ////}



                WarrantyClaimGrid.DataSource = dvDetails.ToTable();
                WarrantyClaimGrid.DataBind();
                //if (hdnFirstSlabamt.Value == "0")
                //{
                //    dDomesticFirstSlabAmount = ObjWarranty.GetDomesticFirstSlabAmount();
                //    hdnFirstSlabamt.Value = dDomesticFirstSlabAmount.ToString();
                //}
                //else
                //{
                //    hdnFirstSlabamt.Value = dDomesticFirstSlabAmount.ToString();
                //    dDomesticFirstSlabAmount = Func.Convert.dConvertToDouble(hdnFirstSlabamt.Value);
                //}
                //VHP END

                if (dvDetails.ToTable() == null || dsClaim.Tables.Count == 0 || dvDetails.ToTable().Rows.Count == 0)
                {
                    lblMessage.Text = " Records Does Not Exists !";
                    lblMessage.Visible = true;
                    //lblCommonMsg.Style.Add("display", "none");
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
            objJobcard = null;

        }
        private void SetControlPropertyOfGrid()
        {
            for (int iRowcnt = 0; iRowcnt < WarrantyClaimGrid.Rows.Count; iRowcnt++)
            {
                Label lblDealerID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblDealerID");
                Label lblJobcardID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblJobcardID");
                Label lblID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblID");
                Label lblJobcodeID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblJobcodeID");

                ImageButton ImgSelect = (ImageButton)WarrantyClaimGrid.Rows[iRowcnt].FindControl("ImgSelect");                
                ImgSelect.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + lblDealerID.Text.ToString() + "','" + lblJobcodeID.Text + "','" + lblID.Text.ToString() + "','" + lblJobcardID.Text.Trim() + "','"+ txtDealerCode.Text +"')");
            }
            //if (WarrantyClaimGrid.Rows.Count == 0) return;
            //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            //{

            //    if (drpClaimStatus.SelectedValue == "99")
            //        WarrantyClaimGrid.HeaderRow.Cells[11].Text = "Pen/Appr/Rej/Ret days";
            //    else
            //        WarrantyClaimGrid.HeaderRow.Cells[11].Text = drpClaimStatus.SelectedItem.Text + " days";

            //    trRejRetStatus.Style.Add("display", "");

            //}
            //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            //{
            //    WarrantyClaimGrid.HeaderRow.Cells[5].Text = "Request No.";
            //    WarrantyClaimGrid.HeaderRow.Cells[6].Text = "Request Date";
            //    WarrantyClaimGrid.HeaderRow.Cells[8].Text = "Request Type";
            //    WarrantyClaimGrid.HeaderRow.Cells[9].Text = "Request Amt";
            //    WarrantyClaimGrid.HeaderRow.Cells[10].Text = "Request Status";

            //    if (drpClaimStatus.SelectedValue == "99")
            //        WarrantyClaimGrid.HeaderRow.Cells[11].Text = "Pen/Appr/Rej/Ret days";
            //    else
            //        WarrantyClaimGrid.HeaderRow.Cells[11].Text = drpClaimStatus.SelectedItem.Text + " days";
            //    trRejRetStatus.Style.Add("display", "none");
            //}
            //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            //{
            //    trRejRetStatus.Style.Add("display", "none");
            //}
            //if (drpClaimStatus.SelectedValue == "99")//when all the nShow With Color
            //{
            //    for (int iRowcnt = 0; iRowcnt < WarrantyClaimGrid.Rows.Count; iRowcnt++)
            //    {
            //        Label lblClaimStatus = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblClaimStatus");

            //        if (lblClaimStatus.Text == "Pending")//Pending
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.LightPink;
            //        }
            //        else if (lblClaimStatus.Text == "Approved")//Approved
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.LightGreen;
            //        }
            //        else if (lblClaimStatus.Text == "Rejected")//Rejected
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.LightGray;
            //        }
            //        else if (lblClaimStatus.Text == "Returned")//Returned
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.LightSkyBlue;
            //        }
            //    }
            //}

            ////Domestic claim display in Yellow color if usere role type SHQ Resource or Section Head

            ////txtClr2.Style.Add("display", "none");
            ////lblClr2.Style.Add("display", "none");

            ////if (drpClaimStatus.SelectedValue == "1")
            ////{
            ////    WarrantyClaimGrid.HeaderRow.Cells[11].Style.Add("display", "");

            ////}
            ////else
            ////{
            ////    WarrantyClaimGrid.HeaderRow.Cells[11].Style.Add("display", "none");           

            ////}

            //if ((sDomestic_Export == "D"))//&& (iUserRoleId == 9 || iUserRoleId == 10 || iUserRoleId == 18)
            //{
            //    lblClr2.Text = "Claim Below " + dDomesticFirstSlabAmount + "Amt";
            //    for (int iRowcnt = 0; iRowcnt < WarrantyClaimGrid.Rows.Count; iRowcnt++)
            //    {
            //        Label lblClaimStatus = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblClaimAmt");

            //        Label lblRejRetStatus = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblRejRetStatus");

            //        if (Convert.ToDouble(lblClaimStatus.Text) < dDomesticFirstSlabAmount)
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromArgb(255, 255, 102);//Yellow
            //        }

            //        if (lblRejRetStatus != null && lblRejRetStatus.Text == "3")
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromName("#ff9797");
            //        }
            //        else if (lblRejRetStatus != null && lblRejRetStatus.Text == "4")
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromName("#ffe6e6");
            //        }



            //        //if (drpClaimStatus.SelectedValue == "1")
            //        //    WarrantyClaimGrid.Rows[iRowcnt].Cells[11].Style.Add("display", "");
            //        //else
            //        //{
            //        //    WarrantyClaimGrid.Rows[iRowcnt].Cells[11].Style.Add("display", "none");

            //        //}
            //    }
            //}
            //else if ((sDomestic_Export == "E"))//&& (iUserRoleId == 9 || iUserRoleId == 10 || iUserRoleId == 18)
            //{
            //    lblClr2.Text = "Claim Below 5000 INR OR 100 USD";
            //    for (int iRowcnt = 0; iRowcnt < WarrantyClaimGrid.Rows.Count; iRowcnt++)
            //    {
            //        Label lblClaimStatus = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblClaimAmt");

            //        Label lblRejRetStatus = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblRejRetStatus");
            //        Label lblStateOrCountryID = (Label)WarrantyClaimGrid.Rows[iRowcnt].FindControl("lblStateOrCountryID");

            //        if ((Convert.ToDouble(lblClaimStatus.Text) < 4999.99 && lblStateOrCountryID.Text == "13") || (Convert.ToDouble(lblClaimStatus.Text) < 99.99 && lblStateOrCountryID.Text != "13"))
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromArgb(255, 255, 102);//Yellow
            //        }

            //        if (lblRejRetStatus != null && lblRejRetStatus.Text == "3")
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromName("#ff9797");
            //        }
            //        else if (lblRejRetStatus != null && lblRejRetStatus.Text == "4")
            //        {
            //            WarrantyClaimGrid.Rows[iRowcnt].BackColor = Color.FromName("#ffe6e6");
            //        }



            //        //if (drpClaimStatus.SelectedValue == "1")
            //        //    WarrantyClaimGrid.Rows[iRowcnt].Cells[11].Style.Add("display", "");
            //        //else
            //        //{
            //        //    WarrantyClaimGrid.Rows[iRowcnt].Cells[11].Style.Add("display", "none");

            //        //}
            //    }
            //}


        }

        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            //if (Func.Convert.sConvertToString(Session["Pending"]) == "N")
            //{
            DisplayData(sDealerIds, Func.Convert.iConvertToInt(drpClaimStatus.SelectedValue));
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