
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
using System.Globalization;
using MANART_BAL;
using MANART_DAL;
using System.IO;

namespace MANART.Forms.Activity
{

    public partial class frmActivityClaimGenerate : System.Web.UI.Page
    {
        DataTable dtCurrentTable = new DataTable();

        DataTable dtHeader = new DataTable();
        DataTable dtDetails = new DataTable();
        DataTable dtDetails1 = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        DataTable dtDetails_NonIMAP = new DataTable();
        string sUserType = "";
        int DeptId = 0;
        int iMenuId = 0;
        int iActivityId = 0;
        int iActClaimId = 0;
        string sActivityClaim = "";
        int iloopcnt = 0;
        string sUserID = "";
        string ActivityReqNo = "";
        string strMessage = "";
        string sDealerId = "";
        int iActRequestId = 0;
        int UserDeptId = 0;

        clsActivityHeads ObjActivity = new clsActivityHeads();
        protected void Page_Init(object sender, EventArgs e)
        {


            sUserID = Func.Convert.sConvertToString(Session["UserID"]);
            iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);

            sDealerId = Location.iDealerId.ToString();
            Location.bUseSpareDealerCode = true;

            Location.SetControlValue();
            PDoc.sFormID = "69";
            //txtDlrActDtFrm.sOnBlurScript = " return SetCurrentAndFutureDate(ctl00_ContentPlaceHolder1_txtDlrActDtFrm_txtDocDate,ctl00_ContentPlaceHolder1_hdnFromDate,ctl00_ContentPlaceHolder1_hdnToDate);";
            //txtDlrActDtTo.sOnBlurScript = " return CheckDateGreter(ctl00_ContentPlaceHolder1_txtDlrActDtTo_txtDocDate,ctl00_ContentPlaceHolder1_txtDlrActDtFrm_txtDocDate,'Dealer Activity To Date should be Greater than sFrom Date',ctl00_ContentPlaceHolder1_hdnFromDate,ctl00_ContentPlaceHolder1_hdnToDate);";

            DeptId = 1;
            ToolbarC.bUseImgOrButton = true;
            ToolbarC.iFormIdToOpenForm = 240;
            //ToolbarC.iValidationIdForSave = 240;
           // ToolbarC.iValidationIdForConfirm = 240;


            UserDeptId = Func.Convert.iConvertToInt(Session["DepartmentID"]);
            txtUserDeptID.Text = UserDeptId.ToString();
            if (UserDeptId == 6)
            {
                DeptId = 1;

            }
            else
            {
                DeptId = 2;
            }

            if (!IsPostBack)
            {
                sUserType = Session["UserType"].ToString();

                // txtID.Text = Func.Convert.sConvertToString(Activity.MaxID(iMenuId, Location.iDealerId, sDeptId));
                //FillActivityCalimDetails();
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            }
            lblTitle.Text = "Activity Claim Generation";
            FillSelectionGrid();
            txtDlrActDtFrm.Enabled = true;
            txtDlrActDtTo.Enabled = true;
            Location.bUseSpareDealerCode = true;
            FillTotalBudget("New");
        }
        private void FillTotalBudget(string Flag)
        {
            DataSet ds = new DataSet();
            ds = ObjActivity.GetActivity_FillTotalBudget(Flag, Location.iDealerId);
             DataTable dt2 = new DataTable();
            dt2 = ds.Tables[2];
            txtMTIShare.Text = Func.Convert.dConvertToDouble(dt2.Rows[0]["MTISharePercentage"]).ToString("0.00");
            txtDealerShare.Text = Func.Convert.dConvertToDouble(dt2.Rows[0]["DealerSharePercentage"]).ToString("0.00");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
               


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {

            txtTcktID.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            

            FillCombo();
            FillSelectionGrid();

            //PSelectionGrid.Style.Add("display", "none");
            txtID.Text = "";

            FillActivityCalimDetails("R");
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }

        private void FillCombo()
        {
           Func.Common.BindDataToCombo(drpNameOfActivity, clsCommon.ComboQueryType.Activity, DeptId);
        }

        private void FillSelectionGrid()
        {
            iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
          //  SearchGrid.bGridFillUsingSql = false;
            SearchGrid.AddToSearchCombo("ActivityClaimNO");
            SearchGrid.AddToSearchCombo("ActivityClaimDate");
            SearchGrid.sModelPart = Func.Convert.sConvertToString(DeptId);
            SearchGrid.iDealerID = Location.iDealerId;
            SearchGrid.sSqlFor = "ActivityClaim";
            SearchGrid.sGridPanelTitle = " Activity Claim List";
        }

        private void FillActivityCalimDetails(string Type)
        {
            DataSet ds = new DataSet();

            if (Type=="R")
            {

                 iActRequestId = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                ds = ObjActivity.GetActivity(iActRequestId, "FillActivityRequestDetailsForClaim");
                // New changes add below fields to dealer //26022016
               // EnableDisable("N", "N");
                //txtActivityName.Visible = false;
              //  txtActivityName.Attributes.Add("display", "None");
                txtActivityName.Style.Add("display", "None");
                
                if (txtActivityClaimNo.Text == "" && txtActivityClaimDate.Text == "")
                {


                    txtActivityClaimNo.Text = Func.Convert.sConvertToString(ObjActivity.GenerateActiviytNo(Location.sDealerCode, Location.iDealerId, DeptId, "C"));


                    txtActivityClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);

                }

            }
            else 
            {
                iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
                ds = ObjActivity.GetActivity(iActClaimId, "FillActivityClaimDetails");
                //EnableDisable("N", "N");
            }
              

           //  dtDetails = ds.Tables[0];
            dtHeader = ds.Tables[0];
            FillActivityHeader(dtHeader,Type);

            GridActivityClaimDetails.DataSource = ds.Tables[1];
            GridActivityClaimDetails.DataBind();
            ViewState["CurrentTable"] = ds.Tables[1];
            dtDetails = ds.Tables[1];
            SetControlPropertyToActivityClaimDetails();

            if (dtHeader != null)
            {
                if (dtHeader.Rows.Count > 0)
                {
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalTentative_Amount"]).ToString("0.00") != "0.00")
                    {

                        (GridActivityClaimDetails.FooterRow.Cells[2].FindControl("txtTotalTentativeAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalTentative_Amount"]).ToString("0.00");
                    }
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalActual_Amount"]).ToString("0.00") != "0.00")
                    {
                        (GridActivityClaimDetails.FooterRow.Cells[3].FindControl("txtTotalActualAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalActual_Amount"]).ToString("0.00");
                    }
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalVECV_Shr_Amt"]).ToString("0.00") != "0.00")
                    {
                        (GridActivityClaimDetails.FooterRow.Cells[5].FindControl("txtVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalVECV_Shr_Amt"]).ToString("0.00");
                    }
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalDealer_Shr_Amt"]).ToString("0.00") != "0.00")
                    {
                        (GridActivityClaimDetails.FooterRow.Cells[7].FindControl("txtDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalDealer_Shr_Amt"]).ToString("0.00");
                    }
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalApprv_VECV_Amt"]).ToString("0.00") != "0.00")
                    {
                        (GridActivityClaimDetails.FooterRow.Cells[9].FindControl("txtApprVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalApprv_VECV_Amt"]).ToString("0.00");
                    }
                    if (Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalAppr_Dealer_Amt"]).ToString("0.00") != "0.00")
                    {
                        (GridActivityClaimDetails.FooterRow.Cells[11].FindControl("txtApprDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["TotalAppr_Dealer_Amt"]).ToString("0.00");
                    }
                }
            }

            GridActivityMerchantDetails.DataSource = ds.Tables[2];
            GridActivityMerchantDetails.DataBind();
            ViewState["CurrentTable1"] = ds.Tables[2];
            dtDetails1 = ds.Tables[2];
            SetControlPropertyToActivityClaimDetails_Merch();
            FilldocumentDtls(ds.Tables[3]);
            ObjActivity = null;
        }
        private void FilldocumentDtls(DataTable dtDcoDtls)
        {
            FileAttchGrid.DataSource = dtDcoDtls;
            FileAttchGrid.DataBind();

        }
        private void FillActivityHeader(DataTable dtHeader, string Type)
        {
            string Claim_Request = "";
            string sIsConfirm = "";
            if (dtHeader.Rows.Count == 0)
            {
                return;
            }
            txtID.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ID"]);
            iActRequestId = Func.Convert.iConvertToInt(dtHeader.Rows[0]["ActivityReqID"]);
            txtTcktID.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ActivityReqID"]);
            txtActivityName.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Name"]);
            txtActivityName.Attributes.Add("display", "None");
            txtFromDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["From_Date"]);
            hdnFromDate.Value = txtFromDate.Text;
            txtToDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["To_Date"]);
            hdnToDate.Value = txtToDate.Text;
            txtTypeOfActivity.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Type"]);

            txtActivityReqNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Req_No"]);
            txtActivityReqDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Req_Date"]);

            

            txtCostCenter.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Cost_Centre"]);
            txtGLAccount.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Account_GL"]);

            txtDlrActDtFrm.Text = Func.Convert.tConvertToDate(dtHeader.Rows[0]["Dealer_Activity_DateFrom"], false);
            hdnDlrClaimFromDate.Value = Func.Convert.tConvertToDate(dtHeader.Rows[0]["ActivityClaimMinDate"], false);
            txtDlrActDtTo.Text = Func.Convert.tConvertToDate(dtHeader.Rows[0]["Dealer_Activity_DateTo"], false);
            hdnDlrClaimToDate.Value = Func.Convert.tConvertToDate(dtHeader.Rows[0]["ActivityClaimMaxDate"], false);

            txtObjective.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Objective"]);
            txtComments.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Comments"]);

            txtTotalBudgetAvailable.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Total_Budget_Available"]).ToString("0.00");
            txtBudgetUtilized.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Budget_Utilized"]).ToString("0.00");
            txtPendingBudget.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Pending_Budget"]).ToString("0.00");

            txtExpectedNoCustomers.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ExpectedNoCustomers"]);
            txtExpectedNoofVehicles.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ExpectedNoofVehicles"]);

            txtExpectedPartsBusiness.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["ExpectedPartsBusiness"]).ToString("0.00");
            txtExpectedServiceRevenue.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["ExpectedServiceRevenue"]).ToString("0.00");
            txtExpectedLube.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["ExpectedLube"]).ToString("0.00");

            hdnActivityClaimConfirm.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["ActivityClaim_Confirm"]);

            ////sIsConfirm = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Confirm"]);       
            //hdnActivityClaim.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Confirm"]);
            //txtclaimRequest.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Claim_Request"]);
            //hdnApprovalStatus.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Approved_Status"]);
            //string sActivityClaimConfirm = Func.Convert.sConvertToString(dtHeader.Rows[0]["ActivityClaim_Confirm"]);
            hdnNewClaimCreateStatus.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["NewClaimCreateStatus"]);

            hdnActivityClaimCancel.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["ActivityClaim_Cancel"]);

            EnableDisable(hdnActivityClaimConfirm.Value, hdnActivityClaimCancel.Value);

            txtApprVeCVShareTotAmt_GST.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Apprv_VECV_Amt"]).ToString("0.00");
            txtIGST_SGST_GST.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["IGST_SGST_Amt"]).ToString("0.00");
            txtCGST_GST.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["CGST_Amt"]).ToString("0.00");
            txtApprVeCVShareFinalTotAmt_GST.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Total_Apprv_VECV_Amt"]).ToString("0.00");
            //txtHSNCode.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["HSN_Code"]);

            lblIGST_SGST.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["IGST_SGST_Desc"]);
            lblCGST.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["CGST_Desc"]);
            txtIGST_SGST_id.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["IGST_SGST_Tax_ID"]);
            txtCGST_id.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["CGST_Tax_ID"]);
            txtIGST_SGST_Per.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["IGST_SGST_Per"]);
            txtCGST_Per.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["CGST_Per"]);

            txtDeductionAmount_GST.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Deduction_Amount_GST"]).ToString("0.00");
            txtApprVeCVShareTotAmt_GSTWithDeduction.Text = Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Apprv_VECV_Amt_with_Deduction"]).ToString("0.00");

            if (Type == "C")
            {
                txtActivityName.Attributes.Add("display", "None");
                EnableDisable(hdnActivityClaimConfirm.Value, hdnActivityClaimCancel.Value);
                txtActivityClaimNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Claim_No"]);
                txtActivityClaimDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Claim_Date"]);
                txtInvoiceNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_No"]);
                txtInvoicedate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_Date"]);
                if (txtInvoiceNo.Text !="")
                {
                  ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                else
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
            
            }


           
            FillCombo();
            drpNameOfActivity.SelectedValue = Func.Convert.sConvertToString(dtHeader.Rows[0]["ActivityID"]);


        }

        private void FillDetailsGrid()
        {



            //ViewState["CurrentTable"] = dtDtls;
            //GridActivityClaimDetails.DataSource = dtDtls;
            //GridActivityClaimDetails.DataBind();
            CheckBox ChkBox = new CheckBox();
            for (int iRowCnt = 0; iRowCnt < GridActivityClaimDetails.Rows.Count; iRowCnt++)
            {


                Label lblNo = GridActivityClaimDetails.Rows[iRowCnt].FindControl("lblID") as Label;
                // TextBox txtActualExpHead = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtActualExpHead") as TextBox);
                //DropDownList drpExpectedExpHead = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead") as DropDownList);
                //DropDownList drpActualExpHead = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpActualExpHead") as DropDownList);

                //DropDownList drpExpectedExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead");
                //Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);



                DropDownList drpActualExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpActualExpHead");
                Func.Common.BindDataToCombo(drpActualExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);

                TextBox txtActualAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtActualAmt") as TextBox;

                // TextBox txtExpectedExpHead = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtExpectedExpHead") as TextBox;
                TextBox txtTentativeAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtTentativeAmt") as TextBox;
                TextBox txtVECVShare = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox;
                TextBox txtVeCVShareAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVeCVShareAmt") as TextBox;
                TextBox txtDealShare = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShare") as TextBox;
                TextBox txtDealShareAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShareAmt") as TextBox;

                TextBox txtApprVECVShare = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVECVShare") as TextBox;
                TextBox txtApprVeCVShareAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVeCVShareAmt") as TextBox;
                TextBox txtApprDealShare = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShare") as TextBox;
                TextBox txtApprDealShareAmt = GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShareAmt") as TextBox;







            }

            for (int iRowCnt11 = 0; iRowCnt11 < GridActivityMerchantDetails.Rows.Count; iRowCnt11++)
            {


                Label lblNo = GridActivityMerchantDetails.Rows[iRowCnt11].FindControl("lblID") as Label;

                DropDownList drpMerchandizeReq = (DropDownList)GridActivityMerchantDetails.Rows[iRowCnt11].FindControl("drpMerchandizeReq");
                Func.Common.BindDataToCombo(drpMerchandizeReq, clsCommon.ComboQueryType.MerchandizeRequirement, 0);

                TextBox txtQuantity = GridActivityMerchantDetails.Rows[iRowCnt11].FindControl("txtQuantity") as TextBox;


            }
        }




        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
            FillActivityCalimDetails("C");
        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
            clsActivityHeads objActivity = new clsActivityHeads();
            try
            {
                PnlSearchGrid.Style.Add("display", "none");
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    //FillCombo();
                    //SetInitialRow();
                    //EmptyTextBOx();
                    //EnableDisable("N", "N");
                    //txtActivityName.Visible = false;
                    //drpNameOfActivity.Visible = true;


                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {


                    if (bValidateRecord("N") == false)
                    {
                        goto Last;
                    }
                    dtDetails = new DataTable();
                    if (ActivityRequestClaimDtlsSave("N", ref dtDetails) == false)
                        goto Last;

                    if (ActivityRequestClaimMerchandizeSave("N", ref dtDetails1) == false)
                        goto Last;

                    bSaveAttachedDocuments();
                    PnlSearchGrid.Style.Add("display", "");

                    if (objActivity.bSaveActivityClaim(ActivityRequestHdrSave("N"), dtDetails, dtDetails1, dtFileAttach, ref iActClaimId, DeptId,
                        Func.Convert.iConvertToInt(sUserID), "C", Location.sDealerCode) == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        txtID.Text = Func.Convert.sConvertToString(iActClaimId);
                        PnlSearchGrid.Style.Add("display", "");
                        FillActivityCalimDetails("C");
                       
                        
                        //return;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                       // return;
                    }

                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bValidateRecord("Y") == false)
                    {
                        goto Last;
                    }
                    dtDetails = new DataTable();
                    if (ActivityRequestClaimDtlsSave("Y", ref dtDetails) == false)
                        goto Last;

                    if (ActivityRequestClaimMerchandizeSave("Y", ref dtDetails1) == false)
                        goto Last;

                    bSaveAttachedDocuments();


                    if (objActivity.bSaveActivityClaim(ActivityRequestHdrSave("Y"), dtDetails, dtDetails1, dtFileAttach, ref iActClaimId, DeptId,
                        Func.Convert.iConvertToInt(sUserID), "C", Location.sDealerCode) == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                        txtID.Text = Func.Convert.sConvertToString(iActClaimId);
                        FillActivityCalimDetails("C");
                        PnlSearchGrid.Style.Add("display", "");
                        
                       // return;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                       // return;
                    }
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    int iId;
                    iId = Func.Convert.iConvertToInt(txtID.Text);
                    dtDetails = new DataTable();
                    if (ActivityRequestClaimDtlsSave("C", ref dtDetails) == false)
                        goto Last;

                    PnlSearchGrid.Style.Add("display", "");
                }
                PDoc.BindDataToGrid();
                FillSelectionGrid();
                

            Last: ;
            }
            catch (Exception ex)
            {
                objActivity = null;
            }
            objActivity = null;
        }

        private bool bValidateRecord(string SaveConfirm)
        {
            int Chassis_ID = 0;
            string sMessage = " Please enter/select the records.";
            bool bValidateRecord = true;
            clsDB objDB = new clsDB();
            if (txtDlrActDtFrm.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Dealer Activity Date From.";
                bValidateRecord = false;
            }
            if (txtDlrActDtTo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Dealer Activity Date To.";
                bValidateRecord = false;
            }





            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            return bValidateRecord;
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;

            //if (ViewState["CurrentTable"] != null)
            //{
            DataTable dtCurrentTable = new DataTable();//(DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;

            if (GridActivityClaimDetails.Rows.Count > 0)
            {
                string TotalTentative_Amount = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[2].FindControl("txtTotalTentativeAmt")).Text;
                string TotalActual_Amount = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[4].FindControl("txtTotalActualAmt")).Text;
                string TotalVECV_Shr_Amt = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[6].FindControl("txtVeCVShareTotAmt")).Text;
                string TotalDealer_Shr_Amt = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[8].FindControl("txtDealShareTotAmt")).Text;
                string TotalApprVECV_Shr_Amt = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[10].FindControl("txtApprVeCVShareTotAmt")).Text;
                string TotalApprDealer_Shr_Amt = ((TextBox)GridActivityClaimDetails.FooterRow.Cells[12].FindControl("txtApprDealShareTotAmt")).Text;

                dtCurrentTable = ((DataTable)ViewState["CurrentTable"]).Clone();
                for (int i = 0; i < GridActivityClaimDetails.Rows.Count; i++)
                {

                    //extract the TextBox values
                    Label lblID = (Label)GridActivityClaimDetails.Rows[rowIndex].FindControl("lblID");

                 //   DropDownList drpExpectedExpHead = (DropDownList)GridActivityClaimDetails.Rows[rowIndex].FindControl("drpExpectedExpHead");
                    //Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.CulpritCode, 0);



                    DropDownList drpActualExpHead = (DropDownList)GridActivityClaimDetails.Rows[rowIndex].FindControl("drpActualExpHead");
                    // Func.Common.BindDataToCombo(drpActualExpHead, clsCommon.ComboQueryType.DefectCode, 0);







                    //TextBox txtExpectedExpHead = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtExpectedExpHead");

                    //TextBox txtActualExpHead = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtActualExpHead");

                    TextBox txtTentativeAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtTentativeAmt");

                    TextBox txtActualAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtActualAmt");

                    TextBox txtVECVShare = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtVECVShare");
                    TextBox txtVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtVeCVShareAmt");
                    TextBox txtDealShare = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtDealShare");
                    TextBox txtDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtDealShareAmt");

                    TextBox txtApprVECVShare = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtApprVECVShare");
                    TextBox txtApprVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtApprVeCVShareAmt");
                    TextBox txtApprDealShare = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtApprDealShare");
                    TextBox txtApprDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[rowIndex].FindControl("txtApprDealShareAmt");

                    //Label lblApproved = GridActivityClaimDetails.Rows[rowIndex].FindControl("lblApproved") as Label;
                    ////Label lblRequestType = GridActivityClaimDetails.Rows[rowIndex].Cells[2].FindControl("lblApproved") as Label;

                    ////if (lblApproved.Text != "")
                    ////    lblApproved.Text = "Yes";

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["ID"] = lblID.Text;
                    //drCurrentRow["Expense_Head"] = txtExpectedExpHead.Text;

                    //drCurrentRow["Actual_Head"] = txtActualExpHead.Text;

                  //  drCurrentRow["Expense_Head"] = Func.Convert.iConvertToInt(drpExpectedExpHead.SelectedValue);
                    // Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.CulpritCode, 0);

                    drCurrentRow["Actual_Head"] = Func.Convert.iConvertToInt(drpActualExpHead.SelectedValue);
                    //Func.Common.BindDataToCombo(drpActualExpHead, clsCommon.ComboQueryType.DefectCode, 0);


                    drCurrentRow["Tentative_Amount"] = Func.Convert.dConvertToDouble(txtTentativeAmt.Text);

                    drCurrentRow["Actual_Amount"] = Func.Convert.dConvertToDouble(txtActualAmt.Text);

                    drCurrentRow["VECV_Shr_Per"] =Func.Convert.dConvertToDouble(txtVECVShare.Text);
                    drCurrentRow["VECV_Shr_Amt"] = Func.Convert.dConvertToDouble(txtVeCVShareAmt.Text);
                    drCurrentRow["Dealer_Shr_Per"] = Func.Convert.dConvertToDouble(txtDealShare.Text);
                    drCurrentRow["Dealer_Shr_Amt"] = Func.Convert.dConvertToDouble(txtDealShareAmt.Text);

                    drCurrentRow["Apprv_VECV_Per"] = Func.Convert.dConvertToDouble(txtApprVECVShare.Text);
                    drCurrentRow["Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble(txtApprVeCVShareAmt.Text);
                    drCurrentRow["Apprv_Dealer_Per"] = Func.Convert.dConvertToDouble(txtApprDealShare.Text);
                    drCurrentRow["Apprv_Dealer_Amt"] = Func.Convert.dConvertToDouble(txtApprDealShareAmt.Text);
                    //  drCurrentRow["Approved"] = lblApproved.Text;
                    //dtCurrentTable.Rows[i - 1]["Request_Type"] = lblRequestType.Text;
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    dtCurrentTable.AcceptChanges();
                    rowIndex++;
                }

                drCurrentRow = dtCurrentTable.NewRow();
               // drCurrentRow["Expense_Head"] = 0;
                drCurrentRow["Actual_Head"] = 0;
                drCurrentRow["Tentative_Amount"] = 0.00;
                drCurrentRow["Actual_Amount"] = 0.00;
                drCurrentRow["VECV_Shr_Per"] = Func.Convert.dConvertToDouble(txtMTIShare.Text);
                drCurrentRow["VECV_Shr_Amt"] = 0.00;
                drCurrentRow["Dealer_Shr_Per"] = Func.Convert.dConvertToDouble(txtDealerShare.Text);
                drCurrentRow["Dealer_Shr_Amt"] = 0.00;
                drCurrentRow["Apprv_VECV_Per"] = Func.Convert.dConvertToDouble(txtMTIShare.Text);
                drCurrentRow["Apprv_VECV_Amt"] = 0.00;
                drCurrentRow["Apprv_Dealer_Per"] = Func.Convert.dConvertToDouble(txtDealerShare.Text);
                drCurrentRow["Apprv_Dealer_Amt"] = 0.00;
                //  drCurrentRow["Approved"] = "";
                // drCurrentRow["Request_Type"] = "";

                dtCurrentTable.Rows.Add(drCurrentRow);
                dtCurrentTable.AcceptChanges();
                ViewState["CurrentTable"] = dtCurrentTable;
                dtDetails = dtCurrentTable;
                GridActivityClaimDetails.DataSource = dtCurrentTable;
                GridActivityClaimDetails.DataBind();
                // FillDetailsGrid();
                SetControlPropertyToActivityClaimDetails();
                (GridActivityClaimDetails.FooterRow.Cells[2].FindControl("txtTotalTentativeAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalTentative_Amount).ToString("0.00");
                (GridActivityClaimDetails.FooterRow.Cells[4].FindControl("txtTotalActualAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalActual_Amount).ToString("0.00");
                (GridActivityClaimDetails.FooterRow.Cells[6].FindControl("txtVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalVECV_Shr_Amt).ToString("0.00");
                (GridActivityClaimDetails.FooterRow.Cells[8].FindControl("txtDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalDealer_Shr_Amt).ToString("0.00");
                (GridActivityClaimDetails.FooterRow.Cells[10].FindControl("txtApprVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalApprVECV_Shr_Amt).ToString("0.00");
                (GridActivityClaimDetails.FooterRow.Cells[12].FindControl("txtApprDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(TotalApprDealer_Shr_Amt).ToString("0.00");

            }


        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
            // HideColumn();
            //FillDetailsGrid();

        }
        protected void btnAddNew1_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid1();
            // HideColumn();
            //FillDetailsGrid();

        }
        private void AddNewRowToGrid1()
        {
            int rowIndex = 0;

            //if (ViewState["CurrentTable"] != null)
            //{
            DataTable dtCurrentTable1 = new DataTable();//(DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow1 = null;

            if (GridActivityMerchantDetails.Rows.Count > 0)
            {


                dtCurrentTable1 = ((DataTable)ViewState["CurrentTable1"]).Clone();
                for (int i = 0; i < GridActivityMerchantDetails.Rows.Count; i++)
                {

                    //extract the TextBox values
                    Label lblID = (Label)GridActivityMerchantDetails.Rows[rowIndex].FindControl("lblID");

                    DropDownList drpMerchandizeReq = (DropDownList)GridActivityMerchantDetails.Rows[rowIndex].FindControl("drpMerchandizeReq");
                    //Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.CulpritCode, 0);

                    TextBox txtQuantity = (TextBox)GridActivityMerchantDetails.Rows[rowIndex].FindControl("txtQuantity");

                    drCurrentRow1 = dtCurrentTable1.NewRow();
                    drCurrentRow1["ID"] = lblID.Text;

                    drCurrentRow1["MerchandizeReq"] = Func.Convert.iConvertToInt(drpMerchandizeReq.SelectedValue);


                    drCurrentRow1["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                    dtCurrentTable1.Rows.Add(drCurrentRow1);
                    dtCurrentTable1.AcceptChanges();
                    rowIndex++;
                }

                drCurrentRow1 = dtCurrentTable1.NewRow();
                drCurrentRow1["ID"] = 0;
                drCurrentRow1["MerchandizeReq"] = 0;
                drCurrentRow1["Qty"] = 0.00;
                dtCurrentTable1.Rows.Add(drCurrentRow1);
                dtCurrentTable1.AcceptChanges();
                ViewState["CurrentTable1"] = dtCurrentTable1;
                dtDetails1 = dtCurrentTable1;
                GridActivityMerchantDetails.DataSource = dtCurrentTable1;
                GridActivityMerchantDetails.DataBind();
                // FillDetailsGrid();
                SetControlPropertyToActivityClaimDetails_Merch();


            }


        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {

            }
        }

        private void SetInitialRow()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ID", typeof(string)));
           // dt.Columns.Add(new DataColumn("Expense_Head", typeof(int)));
            dt.Columns.Add(new DataColumn("Actual_Head", typeof(int)));
            dt.Columns.Add(new DataColumn("Tentative_Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Actual_Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("VECV_Shr_Per", typeof(string)));
            dt.Columns.Add(new DataColumn("VECV_Shr_Amt", typeof(string)));
            dt.Columns.Add(new DataColumn("Dealer_Shr_Per", typeof(string)));
            dt.Columns.Add(new DataColumn("Dealer_Shr_Amt", typeof(string)));
            dt.Columns.Add(new DataColumn("Approved", typeof(string)));
            dt.Columns.Add(new DataColumn("Apprv_VECV_Per", typeof(string)));
            dt.Columns.Add(new DataColumn("Apprv_VECV_Amt", typeof(string)));
            dt.Columns.Add(new DataColumn("Apprv_Dealer_Per", typeof(string)));
            dt.Columns.Add(new DataColumn("Apprv_Dealer_Amt", typeof(string)));
            dt.Columns.Add(new DataColumn("Request_Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));

            for (int i = 0; i < 1; i++)
            {
                dr = dt.NewRow();
                dr["ID"] = 0;
               // dr["Expense_Head"] = 0;
                dr["Actual_Head"] = 0;
                dr["Tentative_Amount"] = string.Empty;
                dr["Actual_Amount"] = string.Empty;
                dr["VECV_Shr_Per"] = string.Empty;
                dr["VECV_Shr_Amt"] = string.Empty;
                dr["Dealer_Shr_Per"] = string.Empty;
                dr["Dealer_Shr_Amt"] = string.Empty;
                dr["Approved"] = string.Empty;
                dr["Apprv_VECV_Per"] = string.Empty;
                dr["Apprv_VECV_Amt"] = string.Empty;
                dr["Apprv_Dealer_Per"] = string.Empty;
                dr["Apprv_Dealer_Amt"] = string.Empty;
                dr["Request_Type"] = string.Empty;
                dr["Status"] = "N";
                dt.Rows.Add(dr);
            }

            ViewState["CurrentTable"] = dt;

            GridActivityClaimDetails.DataSource = dt;
            GridActivityClaimDetails.DataBind();
            SetControlPropertyToActivityClaimDetails();

            DataTable dt1 = new DataTable();

            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("ID", typeof(string)));
            dt1.Columns.Add(new DataColumn("MerchandizeReq", typeof(int)));
            dt1.Columns.Add(new DataColumn("Qty", typeof(int)));


            for (int j = 0; j < 1; j++)
            {
                dr1 = dt1.NewRow();
                dr1["ID"] = 0;
                dr1["MerchandizeReq"] = 0;
                dr1["Qty"] = 0;

                dt1.Rows.Add(dr1);
            }

            ViewState["CurrentTable1"] = dt1;

            GridActivityMerchantDetails.DataSource = dt1;
            GridActivityMerchantDetails.DataBind();
            SetControlPropertyToActivityClaimDetails_Merch();

            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();



        }
        #region File Attachment Functions
        // To Save the attach Document 
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            DataRow dr;
            int iRecorFound = 0;
            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);

                    if (sSourceFileName.Trim() != "")
                    {
                        //if (upload.ContentLength == 0)                continue;
                        dr = dtFileAttach.NewRow();

                        dr["ID"] = 0;

                        // Retriveing the Description of the File

                        for (int iCnt = iRecorFound; iCnt < 20; iCnt++)
                        {
                            if (Request.Form["Text" + (iCnt + 1)] != null)
                            {
                                iRecorFound = iCnt + 1;
                                dr["Description"] = Request.Form["Text" + (iCnt + 1).ToString()];
                                break;
                            }
                        }

                        string[] splClaimNo = txtActivityReqNo.Text.Split('/');
                        if (splClaimNo.Length > 1)
                        {
                            txtActivityReqNo.Text = "";
                            for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                                txtActivityReqNo.Text = txtActivityReqNo.Text + splClaimNo[iCnt];
                        }



                        dr["File_Names"] = Func.Convert.sConvertToString(sDealerId) + Func.Convert.sConvertToString(txtActivityReqNo.Text) + "_" + sSourceFileName;

                        dr["UserId"] = Func.Convert.sConvertToString(sDealerId);
                        dr["Status"] = "S";

                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        //Saving it in temperory Directory.               

                        if (!System.IO.Directory.Exists(sPath + "Activity"))
                            System.IO.Directory.CreateDirectory(sPath + "Activity");

                        DirectoryInfo destination = new DirectoryInfo(sPath + "Activity");
                        if (!destination.Exists)
                        {
                            destination.Create();
                        }
                        uploads[i].SaveAs((sPath + "Activity" + "\\" + Func.Convert.sConvertToString(sDealerId) + Func.Convert.sConvertToString(txtActivityReqNo.Text) + "_" + sSourceFileName + ""));


                    }

                }

                catch (Exception ex)
                {
                    return false;
                    Func.Common.ProcessUnhandledException(ex);
                }

            }
            return true;
        }

        private void bFillDetailsFromFileAttachGrid()
        {
            bool bSaveRecord = false;
            DataRow dr;
            dtFileAttach = new DataTable();
            //Get Header InFormation        
            dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
                    if (txtRefClaimID.Text != "")
                        dr["ID"] = 0;
                    else
                        dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(sDealerId);

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                    }
                    dtFileAttach.Rows.Add(dr);
                    dtFileAttach.AcceptChanges();
                }
            }

        }

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }
        #endregion



        private void SetControlPropertyToActivityClaimDetails()
        {
            try
            {



                for (int iRowCnt = 0; iRowCnt < GridActivityClaimDetails.Rows.Count; iRowCnt++)
                {


                    //DropDownList drpExpectedExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead");
                    //Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);



                    DropDownList drpActualExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpActualExpHead");
                    Func.Common.BindDataToCombo(drpActualExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);

                    //drpExpectedExpHead.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Expense_Head"]);
                    drpActualExpHead.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Actual_Head"]);

                    TextBox txtTentativeAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtTentativeAmt");
                    txtTentativeAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Tentative_Amount"]).ToString("0.00");

                    TextBox txtActualAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtActualAmt");
                    txtActualAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Actual_Amount"]).ToString("0.00");

                    TextBox txtVECVShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVECVShare");
                    txtVECVShare.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["VECV_Shr_Per"]).ToString("0.00");

                    TextBox txtVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVeCVShareAmt");
                    txtVeCVShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["VECV_Shr_Amt"]).ToString("0.00");

                    TextBox txtDealShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShare");
                    txtDealShare.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Dealer_Shr_Per"]).ToString("0.00");

                    TextBox txtDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShareAmt");
                    txtDealShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Dealer_Shr_Amt"]).ToString("0.00");

                    TextBox txtApprVECVShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVECVShare");
                    txtApprVECVShare.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_VECV_Per"]).ToString("0.00");

                    TextBox txtApprVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVeCVShareAmt");
                    txtApprVeCVShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_VECV_Amt"]).ToString("0.00");

                    TextBox txtApprDealShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShare");
                    txtApprDealShare.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_Dealer_Per"]).ToString("0.00");

                    TextBox txtApprDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShareAmt");
                    txtApprDealShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_Dealer_Amt"]).ToString("0.00");


                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void SetControlPropertyToActivityClaimDetails_Merch()
        {
            try
            {



                for (int iRowCnt1 = 0; iRowCnt1 < GridActivityMerchantDetails.Rows.Count; iRowCnt1++)
                {


                    DropDownList drpMerchandizeReq = (DropDownList)GridActivityMerchantDetails.Rows[iRowCnt1].FindControl("drpMerchandizeReq");
                    Func.Common.BindDataToCombo(drpMerchandizeReq, clsCommon.ComboQueryType.MerchandizeRequirement, 0);
                    drpMerchandizeReq.SelectedValue = Func.Convert.sConvertToString(dtDetails1.Rows[iRowCnt1]["MerchandizeReq"]);


                    TextBox txtQuantity = GridActivityMerchantDetails.Rows[iRowCnt1].FindControl("txtQuantity") as TextBox;
                    txtQuantity.Text = Func.Convert.sConvertToString(dtDetails1.Rows[iRowCnt1]["Qty"]);

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void drpNameOfActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iActivityId;
            hdnDlrClaimFromDate.Value = "";
            hdnDlrClaimToDate.Value = "";
            iActivityId = Func.Convert.iConvertToInt(drpNameOfActivity.SelectedValue);
            GetAndDisplayActivity(Func.Convert.iConvertToInt(iActivityId));
            txtDlrActDtFrm.Text = "";
            txtDlrActDtTo.Text = "";
            //txtID.Text = iActivityId.ToString ();
            //FillActivityCalimDetails();
        }

        private void GetAndDisplayActivity(int iActivityId)
        {
            DataSet ds = new DataSet();
            clsActivityHeads objActivity = new clsActivityHeads();
            ds = objActivity.GetActivityClaimDetailsByActivityID(iActivityId, Location.iDealerId, 1);
            dtDetails = ds.Tables[0];
            if (ds.Tables[0].Rows.Count == 0)
            {
                EmptyTextBOx();
                EnableDisable("N", "N");
            }
            else
            {
                FillActivityHeader(ds.Tables[0],"C");
            }
            //ViewState["CurrentTable"] = dtDtls;
            if (ds.Tables[1].Rows.Count == 0)
                GridActivityClaimDetails.DataSource = (DataTable)ViewState["CurrentTable"];
            else
                GridActivityClaimDetails.DataSource = ds.Tables[1];
            GridActivityClaimDetails.DataBind();
            FillDetailsGrid();


            if (dtDetails != null)
            {
                if (dtDetails.Rows.Count > 0)
                {
                    (GridActivityClaimDetails.FooterRow.Cells[2].FindControl("txtTotalTentativeAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalTentative_Amount"]).ToString("0.00");
                    (GridActivityClaimDetails.FooterRow.Cells[3].FindControl("txtTotalActualAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalActual_Amount"]).ToString("0.00");
                    (GridActivityClaimDetails.FooterRow.Cells[5].FindControl("txtVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalVECV_Shr_Amt"]).ToString("0.00");
                    (GridActivityClaimDetails.FooterRow.Cells[7].FindControl("txtDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalDealer_Shr_Amt"]).ToString("0.00");
                    (GridActivityClaimDetails.FooterRow.Cells[8].FindControl("txtApprVeCVShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalApprv_VECV_Amt"]).ToString("0.00");
                    (GridActivityClaimDetails.FooterRow.Cells[10].FindControl("txtApprDealShareTotAmt") as TextBox).Text = Func.Convert.dConvertToDouble(dtDetails.Rows[0]["TotalAppr_Dealer_Amt"]).ToString("0.00");

                }
            }
            objActivity = null;

            //DisplayData(dtDetails, ds.Tables[1]);
            //objActivity = null;
        }

        private void DisplayData(DataTable dtActivityDtls, DataTable dtDtls)
        {

            if (dtActivityDtls.Rows.Count == 0)
            {
                return;
            }

            txtToDate.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["To_Date"]);
            txtFromDate.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["From_Date"]);

            txtTypeOfActivity.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["Activity_Type"]);


            txtCostCenter.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["Cost_Centre"]);
            txtGLAccount.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["Account_GL"]);

            GridActivityClaimDetails.DataSource = dtDtls;
            GridActivityClaimDetails.DataBind();


        }



        private void HideColumn()
        {
            //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmConfirm, false);
            //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmCancel, false);
            //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, false);
        }

        private DataTable ActivityRequestHdrSave(string sActivityClaimConfirm)
        {
            DataTable dtHdr = new DataTable();
            DataRow dr;
            //Get Header InFormation        
            dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Activity_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Activity_Req_No", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Activity_Req_Date", typeof(string)));

            dtHdr.Columns.Add(new DataColumn("Dealer_Activity_DateFrom", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Dealer_Activity_DateTo", typeof(string)));

            dtHdr.Columns.Add(new DataColumn("Objective", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Comments", typeof(string)));

            dtHdr.Columns.Add(new DataColumn("Total_Budget_Available", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("Budget_Utilized", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("Pending_Budget", typeof(double)));

            dtHdr.Columns.Add(new DataColumn("ExpectedNoCustomers", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("ExpectedNoofVehicles", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("ExpectedPartsBusiness", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("ExpectedServiceRevenue", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("ExpectedLube", typeof(double)));

            dtHdr.Columns.Add(new DataColumn("ActivityClaim_Confirm", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("ActivityClaim_Cancel", typeof(string)));

            dtHdr.Columns.Add(new DataColumn("Activity_Request_ID", typeof(int)));

            dtHdr.Columns.Add(new DataColumn("Apprv_VECV_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("IGST_SGST_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("CGST_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("Total_Apprv_VECV_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("VECV_Apprv_VECV_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("VECV_IGST_SGST_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("VECV_CGST_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("VECV_Total_Apprv_VECV_Amt", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("IGST_SGST_Tax_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("CGST_Tax_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Deduction_Amount_GST", typeof(double)));
            dtHdr.Columns.Add(new DataColumn("Apprv_VECV_Amt_with_Deduction", typeof(double)));



            dr = dtHdr.NewRow();
            dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
            dr["Dealer_ID"] = Location.iDealerId;
            dr["Activity_ID"] = Func.Convert.iConvertToInt(drpNameOfActivity.SelectedValue);
            dr["Activity_Req_No"] = txtActivityClaimNo.Text;
            dr["Activity_Req_Date"] = txtActivityClaimDate.Text;

            dr["Dealer_Activity_DateFrom"] = txtDlrActDtFrm.Text;
            dr["Dealer_Activity_DateTo"] = txtDlrActDtTo.Text;

            dr["Objective"] = txtObjective.Text;
            dr["Comments"] = txtComments.Text;

            dr["Total_Budget_Available"] = txtTotalBudgetAvailable.Text;
            dr["Budget_Utilized"] = txtBudgetUtilized.Text;
            dr["Pending_Budget"] = txtPendingBudget.Text;

            dr["ExpectedNoCustomers"] = Func.Convert.iConvertToInt(txtExpectedNoCustomers.Text);
            dr["ExpectedNoofVehicles"] = Func.Convert.iConvertToInt(txtExpectedNoofVehicles.Text);

            dr["ExpectedPartsBusiness"] = txtExpectedPartsBusiness.Text;
            dr["ExpectedServiceRevenue"] = txtExpectedServiceRevenue.Text;
            dr["ExpectedLube"] = txtExpectedLube.Text;


            dr["ActivityClaim_Confirm"] = (sActivityClaimConfirm == "Y") ? "Y" : "N";
            dr["ActivityClaim_Cancel"] = (sActivityClaimConfirm == "C") ? "Y" : "N"; ;

            dr["Activity_Request_ID"] = Func.Convert.iConvertToInt(txtTcktID.Text);

            dr["Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble(txtApprVeCVShareTotAmt_GST.Text);
            dr["IGST_SGST_Amt"] = Func.Convert.dConvertToDouble(txtIGST_SGST_GST.Text);
            dr["CGST_Amt"] = Func.Convert.dConvertToDouble(txtCGST_GST.Text);
            dr["Total_Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble(txtApprVeCVShareFinalTotAmt_GST.Text);
            dr["VECV_Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble(txtApprVeCVShareTotAmt_GST.Text);
            dr["VECV_IGST_SGST_Amt"] = Func.Convert.dConvertToDouble(txtIGST_SGST_GST.Text);
            dr["VECV_CGST_Amt"] = Func.Convert.dConvertToDouble(txtCGST_GST.Text);
            dr["VECV_Total_Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble(txtApprVeCVShareFinalTotAmt_GST.Text);
            dr["IGST_SGST_Tax_ID"] = Func.Convert.iConvertToInt(txtIGST_SGST_id.Text);
            dr["CGST_Tax_ID"] = Func.Convert.iConvertToInt(txtCGST_id.Text);
            dr["Deduction_Amount_GST"] = Func.Convert.dConvertToDouble(txtDeductionAmount_GST.Text);
            dr["Apprv_VECV_Amt_with_Deduction"] = Func.Convert.dConvertToDouble(txtApprVeCVShareTotAmt_GSTWithDeduction.Text);



            dtHdr.Rows.Add(dr);
            dtHdr.AcceptChanges();
            return dtHdr;

        }

        private Boolean ActivityRequestClaimDtlsSave(string obj, ref DataTable dtDetails)
        {
            CheckBox ChkBox = new CheckBox();
            DataRow dr;
            dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("ActivityHDrID", typeof(int)));
            //dtDetails.Columns.Add(new DataColumn("Expense_Head", typeof(string)));
            //dtDetails.Columns.Add(new DataColumn("Actual_Head", typeof(string)));

         //   dtDetails.Columns.Add(new DataColumn("Expense_Head", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Actual_Head", typeof(int)));

            dtDetails.Columns.Add(new DataColumn("Tentative_Amount", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Actual_Amount", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("VECV_Shr_Per", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("VECV_Shr_Amt", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Dealer_Shr_Per", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Dealer_Shr_Amt", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Apprv_VECV_Per", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Apprv_VECV_Amt", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Apprv_Dealer_Per", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Apprv_Dealer_Amt", typeof(double)));
            dtDetails.Columns.Add(new DataColumn("Approved", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("Request_Type", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
            iloopcnt = 0;
            for (int iRowCnt = 0; iRowCnt < GridActivityClaimDetails.Rows.Count; iRowCnt++)
            {
                ChkBox = (CheckBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("ChkForDelete");
                int iID = Func.Convert.iConvertToInt((GridActivityClaimDetails.Rows[iRowCnt].FindControl("lblID") as Label).Text);

                dr = dtDetails.NewRow();
                if (ChkBox.Checked == false)
                {
                    dr["ID"] = iID;


                    dr["ActivityHDrID"] = Func.Convert.iConvertToInt(txtID.Text);


                //    dr["Expense_Head"] = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead") as DropDownList).SelectedValue;

                    dr["Actual_Head"] = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpActualExpHead") as DropDownList).SelectedValue;

                    dr["Tentative_Amount"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtTentativeAmt") as TextBox).Text);
                    dr["Actual_Amount"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtActualAmt") as TextBox).Text);
                    dr["VECV_Shr_Per"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                    dr["VECV_Shr_Amt"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVeCVShareAmt") as TextBox).Text);
                    dr["Dealer_Shr_Per"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShare") as TextBox).Text);
                    dr["Dealer_Shr_Amt"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShareAmt") as TextBox).Text);
                    dr["Apprv_VECV_Per"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVECVShare") as TextBox).Text);
                    dr["Apprv_VECV_Amt"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVeCVShareAmt") as TextBox).Text);
                    dr["Apprv_Dealer_Per"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShare") as TextBox).Text);
                    dr["Apprv_Dealer_Amt"] = Func.Convert.dConvertToDouble((GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShareAmt") as TextBox).Text);

                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                    iloopcnt = iloopcnt + 1;
                }


            }


            return true;
        }






        private Boolean ActivityRequestClaimMerchandizeSave(string obj, ref DataTable dtDetails1)
        {
            CheckBox ChkBox = new CheckBox();
            DataRow dr;
            dtDetails1 = new DataTable();
            dtDetails1.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails1.Columns.Add(new DataColumn("ActivityHDrID", typeof(int)));
            dtDetails1.Columns.Add(new DataColumn("MerchandizeReq", typeof(int)));
            dtDetails1.Columns.Add(new DataColumn("Qty", typeof(int)));
            dtDetails1.Columns.Add(new DataColumn("Status", typeof(string)));

            iloopcnt = 0;
            for (int iRowCnt = 0; iRowCnt < GridActivityMerchantDetails.Rows.Count; iRowCnt++)
            {
                ChkBox = (CheckBox)GridActivityMerchantDetails.Rows[iRowCnt].FindControl("ChkForDelete1");
                int iID = Func.Convert.iConvertToInt((GridActivityMerchantDetails.Rows[iRowCnt].FindControl("lblID") as Label).Text);

                dr = dtDetails1.NewRow();
                if (ChkBox.Checked == false)
                {
                    dr["ID"] = iID;


                    dr["ActivityHDrID"] = Func.Convert.iConvertToInt(txtID.Text);

                    //DropDownList drpMerchandizeReq = (DropDownList)GridActivityMerchantDetails.Rows[iRowCnt].FindControl("drpMerchandizeReq");
                    //Func.Common.BindDataToCombo(drpMerchandizeReq, clsCommon.ComboQueryType.MerchandizeRequirement, 0);

                    dr["MerchandizeReq"] = (GridActivityMerchantDetails.Rows[iRowCnt].FindControl("drpMerchandizeReq") as DropDownList).SelectedValue;


                    dr["Qty"] = Func.Convert.dConvertToDouble((GridActivityMerchantDetails.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);

                    dr["Status"] = "S";

                    dtDetails1.Rows.Add(dr);
                    dtDetails1.AcceptChanges();
                    iloopcnt = iloopcnt + 1;
                }
                else
                {

                    dr["ID"] = iID;
                    dr["Status"] = "D";
                    dtDetails1.Rows.Add(dr);
                    dtDetails1.AcceptChanges();

                }

            }


            return true;
        }




        private void EmptyTextBOx()
        {
            txtID.Text = "";
            txtActivityReqNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "AC", Location.iDealerId);
            txtActivityReqDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            // txtDeparmentalActivity.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtFormType.Text = "";

            txtCostCenter.Text = "";
            txtObjective.Text = "";

            txtGLAccount.Text = "";
            txtTypeOfActivity.Text = "";

            txtDlrActDtFrm.Text = "";
            txtDlrActDtTo.Text = "";



        }

        private void EnableDisable(string sConfirmStatus, string sCancelStatus)
        {
            if (sConfirmStatus == "Y" || sCancelStatus == "Y")
            {
                txtActivityName.Enabled = false;
                txtObjective.Enabled = false;

                //txtFromDate.Enabled = false;
                //txtToDate.Enabled = false;

                txtTypeOfActivity.Enabled = false;
                txtDlrActDtFrm.Enabled = false;
                txtDlrActDtTo.Enabled = false;
                txtActivityReqNo.Enabled = false;
                //txtActivityReqDate.Enabled = false;

                GridActivityClaimDetails.Enabled = false;
                GridActivityMerchantDetails.Enabled = false;
                trUpload.Visible = false;
                trUpload1.Visible = false;

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave,  false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);

            }
            else if (sConfirmStatus == "N" || sCancelStatus == "N")
            {
                txtActivityName.Enabled = true;
                txtObjective.Enabled = true;

                //txtFromDate.Enabled = false;
                //txtToDate.Enabled = false;

                txtTypeOfActivity.Enabled = true;
                txtDlrActDtFrm.Enabled = true;
                txtDlrActDtTo.Enabled = true;
                txtActivityReqNo.Enabled = true;
                //txtActivityReqDate.Enabled = false;

                GridActivityClaimDetails.Enabled = true;
                GridActivityMerchantDetails.Enabled = true;
                trUpload.Visible = true;
                trUpload1.Visible = true;

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);


            }
           

        }

        private void ConfirmRecord(int iId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (iId > 0)
                {
                    string ssql = "Update TM_ActivityRequestClaim Set Activity_Confirm ='Y' where ID=" + iId;
                    objDB.BeginTranasaction();
                    objDB.ExecuteQuery(ssql);
                    objDB.CommitTransaction();
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                    // ToolbarC.sSetMessage(WebParts_Toolbar.enmToolbarType.enmConfirm);
                    if (ViewState["ActivityId"] != null)
                        GetAndDisplayActivity(Func.Convert.iConvertToInt(ViewState["ActivityId"]));
                    //FillActivityCalimDetails();
                    txtDlrActDtFrm.Enabled = false;
                    txtDlrActDtTo.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        protected void btnClaimRequest_Click(object sender, EventArgs e)
        {

            int iID = Func.Convert.iConvertToInt(txtID.Text);
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //string ssql = "Update TM_ActivityRequestClaim Set  Claim_Request='Y', Claim_Date=convert(datetime," + txtCalimDate.Text + ",103) where ID=" + iID;
                objDB.BeginTranasaction();
                //objDB.ExecuteQuery(ssql);
                objDB.CommitTransaction();
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                //  ToolbarC.sSetMessage(WebParts_Toolbar.enmToolbarType.enmConfirm);
                FillActivityCalimDetails("C");
                return;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }



        private bool validation_Invoice()
        {
            string sMessage1 = "";
            bool bValidateRecord = true;




            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage1 + ".');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " sMessage(1)", true);
            }
            return bValidateRecord;
        }


    }

}