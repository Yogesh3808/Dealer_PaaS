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
    public partial class frmActivityRequestClaimProcessing : System.Web.UI.Page
    {
        DataTable dtHeader = new DataTable();
        DataTable dtDetails = new DataTable();
        DataTable dtDetails1 = new DataTable();
        DataTable dtDetails_forFileAttach = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        string sUserID = "";
        int iActivityId = 0;
        string sActivityApprovedStatus = "";
        string strMessage = "";
        int iMenuId = 0;
        string sApprProcc = "";
        string sUserType = "";
        int iUserRoleId;
        int iRegionID;
        int dealerid;
        string ActivityReqNo = "";
        int RejectCount = 0;
        int iActClaimId = 0;
        int DeptId = 0;
        int iId = 0;
        string Head_SubmitYN = "";
        clsActivityHeads ObjActivity = new clsActivityHeads();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sUserType = Session["UserType"].ToString();
                if (sUserType == "3")
                {
                    sUserType = "D";
                }
                else
                    if (sUserType == "4")
                    {
                        sUserType = "E";
                    }
                sUserID = Func.Convert.sConvertToString(Session["UserID"]);
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
               
                hdnRoleID.Value = Func.Convert.sConvertToString(iUserRoleId);
                iRegionID = Func.Convert.iConvertToInt(Request.QueryString["RegionID"]);
                if (!IsPostBack)
                {
                   
                    iActivityId = Func.Convert.iConvertToInt(Request.QueryString["ActivityID"]);
                    iActClaimId = iActivityId;
                    dealerid = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                    txtdealerid.Text = Func.Convert.sConvertToString(dealerid);
                    iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                    sApprProcc = Func.Convert.sConvertToString(Request.QueryString["ApprProcc"]);
                    txtclaimFlage.Text = sApprProcc;
                    txtDeptID.Text = Func.Convert.sConvertToString(Request.QueryString["DeptId"]);
                    txtUserDeptID.Text = Func.Convert.sConvertToString(Request.QueryString["UserDeptId"]);
                    sActivityApprovedStatus = Func.Convert.sConvertToString(Request.QueryString["Flage"]);
                    FillActivityCalimDetails();
                    //CheckBoxCheck();
                   
                   // FillCombo();
                }
                
                Location.bUseSpareDealerCode = true;
               
                ExpirePageCache();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }



        }
        
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        private void SetControlPropertyToActivityClaimDetails()
        {
            try
            {
                if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
                {
                    GridActivityClaimDetails.HeaderRow.Cells[3].Style.Add("display", "none");
                    GridActivityClaimDetails.FooterRow.Cells[3].Style.Add("display", "none");
                }

                for (int iRowCnt = 0; iRowCnt < GridActivityClaimDetails.Rows.Count; iRowCnt++)
                {


                    //DropDownList drpExpectedExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead");
                    //Func.Common.BindDataToCombo(drpExpectedExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);

                

                    DropDownList drpActualExpHead = (DropDownList)GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpActualExpHead");
                    Func.Common.BindDataToCombo(drpActualExpHead, clsCommon.ComboQueryType.ActivityExpensesHead, 0);

                   // drpExpectedExpHead.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Expense_Head"]);
                    drpActualExpHead.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Actual_Head"]);

                    TextBox txtTentativeAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtTentativeAmt");
                   // txtTentativeAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Tentative_Amount"]);
                    txtTentativeAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Tentative_Amount"]).ToString("0.00");
                 

                    TextBox txtActualAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtActualAmt");
                    //txtActualAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Actual_Amount"]);
                    txtActualAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Actual_Amount"]).ToString("0.00");

                    TextBox txtVECVShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVECVShare");
                    txtVECVShare.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["VECV_Shr_Per"]);

                    TextBox txtVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtVeCVShareAmt");
                   // txtVeCVShareAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["VECV_Shr_Amt"]);
                    txtVeCVShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["VECV_Shr_Amt"]).ToString("0.00");

                    TextBox txtDealShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShare");
                    txtDealShare.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Dealer_Shr_Per"]);

                    TextBox txtDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtDealShareAmt");
                   // txtDealShareAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Dealer_Shr_Amt"]);
                    txtDealShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Dealer_Shr_Amt"]).ToString("0.00");

                    TextBox txtApprVECVShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVECVShare");
                    txtApprVECVShare.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Apprv_VECV_Per"]);

                    TextBox txtApprVeCVShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprVeCVShareAmt");
                    //txtApprVeCVShareAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Apprv_VECV_Amt"]);
                    txtApprVeCVShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_VECV_Amt"]).ToString("0.00");

                    TextBox txtApprDealShare = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShare");
                    txtApprDealShare.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Apprv_Dealer_Per"]);

                    TextBox txtApprDealShareAmt = (TextBox)GridActivityClaimDetails.Rows[iRowCnt].FindControl("txtApprDealShareAmt");
                   // txtApprDealShareAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Apprv_Dealer_Amt"]);
                    txtApprDealShareAmt.Text = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Apprv_Dealer_Amt"]).ToString("0.00");
                    if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
                    {
                        GridActivityClaimDetails.Rows[iRowCnt].Cells[3].Style.Add("display", "none");
                    }

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
        private void FillActivityCalimDetails()
        {
           // iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
            DataSet ds = new DataSet();

            if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
            {
                ds = ObjActivity.GetActivity(iActClaimId, "FillActivityRequestApproval");
            }
            else if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Processing")
            {
                ds = ObjActivity.GetActivity(iActClaimId, "FillActivityClaimProcessing");
            }
            //  dtDetails = ds.Tables[0];
            dtHeader = ds.Tables[0];
            FillActivityHeader(dtHeader);

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
        private void FillActivityHeader(DataTable dtHeader)
        {
            string Claim_Request = "";
            string sIsConfirm = "";
            if (dtHeader.Rows.Count == 0)
            {
                return;
            }
            txtID.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ID"]);
            txtActivityName.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Name"]);
            txtActivityName.Attributes.Add("display", "None");
            txtFromDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["From_Date"]);
           
            txtToDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["To_Date"]);
           
            txtTypeOfActivity.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Type"]);

            txtActivityReqNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Req_No"]);
            txtActivityReqDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Req_Date"]);

            txtActivityClaimNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Claim_No"]);
            txtActivityClaimDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Claim_Date"]);

            txtCostCenter.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Cost_Centre"]);
            txtGLAccount.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Account_GL"]);

            txtDlrActDtFrm.Text = Func.Convert.tConvertToDate(dtHeader.Rows[0]["Dealer_Activity_DateFrom"], false);
           
            txtDlrActDtTo.Text = Func.Convert.tConvertToDate(dtHeader.Rows[0]["Dealer_Activity_DateTo"], false);
           

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

            txtASMRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ASM_Remark"]);
            txtRSMRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["RSM_Remark"]);
            txtHeadRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Head_Remark"]);
            txtclaimApprovedStatus.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Activity_Approved_Status"]);

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
            Head_SubmitYN = Func.Convert.sConvertToString(dtHeader.Rows[0]["Head_SubmitYN"]);
            txtAfterSalesHeadRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["SHQResource_Remark"]);


            txtInvoiceNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_No"]);
            txtInvoicedate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_Date"]);
            

            trInvdetails.Style.Add("display", "none");

            if ((iUserRoleId == 2 && txtclaimApprovedStatus.Text == "P" && Func.Convert.iConvertToInt(txtDeptID.Text) == 1  ) 
                || (iUserRoleId == 1 && Func.Convert.iConvertToInt(txtUserDeptID.Text) == 6 && txtclaimApprovedStatus.Text == "P" && Head_SubmitYN == "Y")
                || (txtclaimApprovedStatus.Text == "Y") || (txtclaimApprovedStatus.Text == "R"))
            {
                DivBtn.Style.Add("display", "none");
                btnReturnclaimforModification.Style.Add("display", "none");
            }

            if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
            {
                ASMRemark.Style.Add("display", "none");
                trclaim.Style.Add("display", "none");
                AftersaleRemark.Style.Add("display", "none");
            }
            else if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Processing")
            {
                trclaim.Style.Add("display", "");
                AftersaleRemark.Style.Add("display", "");
                btnReturnclaimforModification.Text = "Return Claim for Modification";
                trInvdetails.Style.Add("display", "");
            }

            if (Func.Convert.iConvertToInt(txtDeptID.Text) == 1)
            {
                ASMRemark.Style.Add("display", "none");
            }
            else if (Func.Convert.iConvertToInt(txtDeptID.Text) == 2)
            {
                ASMRemark.Style.Add("display", "");
            }



         }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;

            if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
            {

                bSubmit = bSaveRecord("A");

                if (bSubmit == true)
                {
                    lblMessage.Text = "Request Approved Successfully.!";
                }
                else
                {
                    lblMessage.Text = "Request Approved Failed.";
                }
            }
            else if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Processing")
            {
                bSubmit = bSaveRecord("P");

                if (bSubmit == true)
                {
                    lblMessage.Text = "Claim Approved Successfully.!";
                }
                else
                {
                    lblMessage.Text = "Claim Approved Failed.";
                }

            }



            iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
          
            lblMessage.Visible = true;
            DivBtn.Visible = false;
            btnReturnclaimforModification.Visible = false;
            FillActivityCalimDetails();
        }

        private bool bReturnRequestforModification(string Type)
        {
            clsActivityHeads ObjActivity = new clsActivityHeads();
            string sRemark = "";
            bool bSubmit = false;

            iActClaimId = Func.Convert.iConvertToInt(txtID.Text);

            bSubmit = ObjActivity.bReturnRequestforModification(iActClaimId, iUserRoleId, Type, txtASMRemark.Text, txtRSMRemark.Text, txtHeadRemark.Text, txtAfterSalesHeadRemark.Text, Func.Convert.iConvertToInt(txtUserDeptID.Text));

            ObjActivity = null;

            return bSubmit;

        }

       

       private bool bSaveRecord(string Type)
        {
        clsActivityHeads ObjActivity = new clsActivityHeads();
        string sRemark = "";
        bool bSubmit = false;

        iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
        bSaveAttachedDocuments();
        DataTable dt = new DataTable();
        dt = ActivityRequestClaimDtlsSave();

        bSubmit = ObjActivity.bSubmitActivityRequestClaim(iActClaimId, iUserRoleId, Type,txtASMRemark.Text, txtRSMRemark.Text, txtHeadRemark.Text,txtAfterSalesHeadRemark.Text,
            Func.Convert.dConvertToDouble(txtApprVeCVShareTotAmt_GST.Text), Func.Convert.dConvertToDouble(txtIGST_SGST_GST.Text),
            Func.Convert.dConvertToDouble(txtCGST_GST.Text), Func.Convert.dConvertToDouble(txtApprVeCVShareFinalTotAmt_GST.Text),
            Func.Convert.dConvertToDouble(txtDeductionAmount_GST.Text), Func.Convert.dConvertToDouble(txtApprVeCVShareTotAmt_GSTWithDeduction.Text),
            dt, dtFileAttach, Func.Convert.iConvertToInt(sUserID), Func.Convert.iConvertToInt(txtUserDeptID.Text), Func.Convert.iConvertToInt(txtDeptID.Text));

           ObjActivity = null;

            return bSubmit;
        
    }

       
       private DataTable ActivityRequestClaimDtlsSave()
       {
           CheckBox ChkBox = new CheckBox();
           DataRow dr;
           dtDetails = new DataTable();
           dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
           dtDetails.Columns.Add(new DataColumn("ActivityHDrID", typeof(int)));
          // dtDetails.Columns.Add(new DataColumn("Expense_Head", typeof(int)));
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
           dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));

           for (int iRowCnt = 0; iRowCnt < GridActivityClaimDetails.Rows.Count; iRowCnt++)
           {

               dr = dtDetails.NewRow();
               dr["ID"] = Func.Convert.iConvertToInt((GridActivityClaimDetails.Rows[iRowCnt].FindControl("lblID") as Label).Text);
               dr["ActivityHDrID"] = Func.Convert.iConvertToInt(txtID.Text);
              // dr["Expense_Head"] = (GridActivityClaimDetails.Rows[iRowCnt].FindControl("drpExpectedExpHead") as DropDownList).SelectedValue;
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

               dr["Status"] = "S";

              
               dtDetails.Rows.Add(dr);
               dtDetails.AcceptChanges();


           }
           return dtDetails;
       }
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



                       dr["File_Names"] = Func.Convert.sConvertToString(dealerid) + Func.Convert.sConvertToString(txtActivityReqNo.Text) + "_" + sSourceFileName;

                       dr["UserId"] = Func.Convert.sConvertToString(dealerid);
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
                       uploads[i].SaveAs((sPath + "Activity" + "\\" + Func.Convert.sConvertToString(dealerid) + Func.Convert.sConvertToString(txtActivityReqNo.Text) + "_" + sSourceFileName + ""));


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
                   //if (txtRefClaimID.Text != "")
                   //    dr["ID"] = 0;
                   //else
                       dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                   dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                   dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                   dr["UserId"] = Func.Convert.iConvertToInt(dealerid);

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

        protected void btnReject_Click(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                sApprProcc = Func.Convert.sConvertToString(Request.QueryString["ApprProcc"]);
                clsActivityHeads objActivity = new clsActivityHeads();
                DataTable dt = new DataTable();
                //dt = ActivityRequestClaimDtlsSave();
               // HideControl();
                int iId;
                iId = Func.Convert.iConvertToInt(txtID.Text);
                if (sApprProcc == "Approval")
                {

                    try
                    {

                        if (iId > 0)
                        {
                            objDB.BeginTranasaction();
                            string ssql = "";
                            //string ssql = "Update TM_ActivityRequestClaim Set Activity_Approved_Status ='N',Activity_Approved_Date=convert(datetime," + "'" + txtActApprdDate.Text + "'" + ",103),Activity_Approved_By='" + txtActApprovedBy.Text + "',ActApprEmpID=" + Func.Convert.iConvertToInt(sUserID) + ",CostCenter_ID=" + Func.Convert.dConvertToDouble(drpCostCenter.SelectedValue) + " where ID=" + iId;
                            objDB.ExecuteQuery(ssql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string sdtlsql = "Update TD_ActivityRequestClaim Set Apprv_VECV_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Per"]) + "',Approved='" + dt.Rows[i]["Approved"] + "',Apprv_VECV_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Amt"]) + "',Apprv_Dealer_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Per"]) + "',Apprv_Dealer_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Amt"]) + "' where ActivityHDrID=" + iId + "AND ID=" + Func.Convert.iConvertToInt(dt.Rows[i]["ID"]);
                                objDB.ExecuteQuery(sdtlsql);

                            }
                            objDB.CommitTransaction();
                            strMessage = "Activity Request Rejected Successfully";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        objDB.RollbackTransaction();
                        strMessage = "Activity Request Rejection Failed";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                    }

                }
                else
                    if (sApprProcc == "Processing")
                    {

                        try
                        {

                            if (iId > 0)
                            {
                                objDB.BeginTranasaction();
                                string ssql = "";//"Update TM_ActivityRequestClaim Set Claim_Approved_Status ='N',Claim_Approved_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103),Activity_Claim_Approved_By='" + txtClaimApprovedBy.Text + "',Activity_Claim_Remark='" + txtRemark.Text + "',ActCmailEmpID=" + Func.Convert.iConvertToInt(sUserID) + " where ID=" + iId;

                                if (iUserRoleId == 4)
                                {
                                   // ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='N',Claim_Approved_Status ='J',First_SubmitYN='J',First_Remark=" + "'" + txt1stRemark.Text + "'" + ",First_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 3 && txtDeptID.Text == "3")
                                {
                                    //ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='N',Claim_Approved_Status ='J',First_SubmitYN='J',First_Remark=" + "'" + txt1stRemark.Text + "'" + ",First_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 2)
                                {
                                    //ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='N',Claim_Approved_Status ='J',Second_SubmitYN='J',Second_Remark=" + "'" + txt2ndRemark.Text + "'" + ",Second_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 9)
                                {
                                   // ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='N',Claim_Approved_Status ='J',Claim_Approved_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103),Activity_Claim_Approved_By='" + txtClaimApprovedBy.Text + "',Activity_Claim_Remark='" + txtRemark.Text + "',ActCmailEmpID=" + Func.Convert.iConvertToInt(sUserID) + ",Third_SubmitYN='J',Third_Remark=" + "'" + txt3rdRemark.Text + "'" + ",Third_SubmitDate=GETDATE(),CostCenter_ID=" + Func.Convert.dConvertToDouble(drpCostCenter.SelectedValue) + " where ID=" + iId;
                                }

                                objDB.ExecuteQuery(ssql);
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["Request_Type"].ToString() == "CR")
                                    {
                                        string sdtlsql = "Update TD_ActivityRequestClaim Set Apprv_VECV_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Per"]) + "',Approved='" + dt.Rows[i]["Approved"] + "',Apprv_VECV_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Amt"]) + "',Apprv_Dealer_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Per"]) + "',Apprv_Dealer_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Amt"]) + "' where ActivityHDrID=" + iId + "AND ID=" + Func.Convert.iConvertToInt(dt.Rows[i]["ID"]); ;
                                        objDB.ExecuteQuery(sdtlsql);
                                    }
                                }
                                objDB.CommitTransaction();
                                strMessage = "Activity  Claim Rejection Successfully";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);

                            }
                        }
                        catch (Exception ex)
                        {
                            objDB.RollbackTransaction();
                            strMessage = "Activity Claim Rejection Failed";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                        }
                    }

                DivBtn.Visible = false;
                btnReturnclaimforModification.Visible = false;
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
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    Activity objActivity = new Activity();
        //    if (objActivity.bSaveActivityProcessDetails(ActivityProcessDtlsSave("P", "P")) == true)
        //    {

        //        strMessage = "Successfully Save Claim Request";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " confirm('" + strMessage + "')", true);
        //        objActivity = null;
        //        return;
        //    }
        //    else
        //    {
        //        strMessage = "Error In Save Claim Request";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " confirm('" + strMessage + "')", true);
        //        objActivity = null;
        //        return;
        //    }
        //}
        protected void btnClaim_Click(object sender, EventArgs e)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int iId;
                iId = Func.Convert.iConvertToInt(txtID.Text);
                if (iId > 0)
                {
                    objDB.BeginTranasaction();
                    string ssql = "";
                   // string ssql = "Update TM_ActivityRequestClaim Set Claim_Request ='Y',Claim_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103) where ID=" + iId;
                    objDB.ExecuteQuery(ssql);
                    objDB.CommitTransaction();
                    strMessage = "Successfully Generate Activity Claim Request";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
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

        protected void btnApprovedCliam_Click(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int iId;
                iId = Func.Convert.iConvertToInt(txtID.Text);
                if (iId > 0)
                {
                    string ssql = "";
                    //string ssql = "Update TM_ActivityRequestClaim Set Claim_Approved_Status ='Y',Claim_Approved_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103) where ID=" + iId;
                    objDB.BeginTranasaction();
                    objDB.ExecuteQuery(ssql);
                    objDB.CommitTransaction();
                    strMessage = "Successfully Activity Claim Approved";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
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
   
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {

            }
        }
        //Megha 27052015
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            btnReadonly();
        }
        // Function Use for Readonly User
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15")
                {
                    btnApprove.Style.Add("display", "none");
              
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objCommon != null) objCommon = null;
            }
        }
        protected void btnReturnclaimforModification_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;

            if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Approval")
            {

                bSubmit = bReturnRequestforModification("A");

                if (bSubmit == true)
                {
                    lblMessage.Text = "Request return for Modification to Dealer.!";
                }
                else
                {
                    lblMessage.Text = "Request return for Modification Failed.";
                }
            }
            else if (Func.Convert.sConvertToString(txtclaimFlage.Text) == "Processing")
            {
                bSubmit = bReturnRequestforModification("P");

                if (bSubmit == true)
                {
                    lblMessage.Text = "Claim return for Modification to Dealer.!";
                }
                else
                {
                    lblMessage.Text = "Claim return for Modification Failed.";
                }
            }
            iActClaimId = Func.Convert.iConvertToInt(txtID.Text);
            // GetDataAndDisplay();
            lblMessage.Visible = true;
            DivBtn.Visible = false;
            btnReturnclaimforModification.Visible = false;
        }
       
        protected void btnReturnclaimforModification1_Click(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                sApprProcc = Func.Convert.sConvertToString(Request.QueryString["ApprProcc"]);
                clsActivityHeads objActivity = new clsActivityHeads();
                DataTable dt = new DataTable();
               // dt = ActivityRequestClaimDtlsSave();
               // HideControl();
                int iId;
                iId = Func.Convert.iConvertToInt(txtID.Text);
                if (sApprProcc == "Approval")
                {

                    try
                    {

                        if (iId > 0)
                        {

                            objDB.BeginTranasaction();
                            string ssql = "";
                            //string ssql = "Update TM_ActivityRequestClaim Set Activity_Approved_Status ='N',Activity_Approved_Date=convert(datetime," + "'" + txtActApprdDate.Text + "'" + ",103),Activity_Approved_By='" + txtActApprovedBy.Text + "',ActApprEmpID=" + Func.Convert.iConvertToInt(sUserID) + ",CostCenter_ID=" + Func.Convert.dConvertToDouble(drpCostCenter.SelectedValue) + " where ID=" + iId;
                            objDB.ExecuteQuery(ssql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string sdtlsql = "Update TD_ActivityRequestClaim Set Apprv_VECV_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Per"]) + "',Approved='" + dt.Rows[i]["Approved"] + "',Apprv_VECV_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Amt"]) + "',Apprv_Dealer_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Per"]) + "',Apprv_Dealer_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Amt"]) + "' where ActivityHDrID=" + iId + "AND ID=" + Func.Convert.iConvertToInt(dt.Rows[i]["ID"]);
                                objDB.ExecuteQuery(sdtlsql);

                            }
                            objDB.CommitTransaction();
                            strMessage = "Activity Request Rejected Successfully";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        objDB.RollbackTransaction();
                        strMessage = "Activity Request Rejection Failed";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                    }

                }
                else
                    if (sApprProcc == "Processing")
                    {

                        try
                        {

                            if (iId > 0)
                            {
                                objDB.BeginTranasaction();
                                string ssql = "";//"Update TM_ActivityRequestClaim Set Claim_Approved_Status ='N',Claim_Approved_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103),Activity_Claim_Approved_By='" + txtClaimApprovedBy.Text + "',Activity_Claim_Remark='" + txtRemark.Text + "',ActCmailEmpID=" + Func.Convert.iConvertToInt(sUserID) + " where ID=" + iId;

                                if (iUserRoleId == 4)
                                {
                                   // ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='Y', Claim_Approved_Status ='J',First_SubmitYN='J',First_Remark=" + "'" + txt1stRemark.Text + "'" + ",First_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 3 && txtDeptID.Text == "3")
                                {
                                    //ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='Y',Claim_Approved_Status ='J',First_SubmitYN='J',First_Remark=" + "'" + txt1stRemark.Text + "'" + ",First_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 2)
                                {
                                   // ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='Y',Claim_Approved_Status ='J',Second_SubmitYN='J',Second_Remark=" + "'" + txt2ndRemark.Text + "'" + ",Second_SubmitDate=GETDATE() where ID=" + iId;
                                }
                                else if (iUserRoleId == 9)
                                {
                                    //ssql = "Update TM_ActivityRequestClaim Set ReturnclaimforModification='Y',Claim_Approved_Status ='J',Claim_Approved_Date=convert(datetime," + "'" + txtCalimProcDate.Text + "'" + ",103),Activity_Claim_Approved_By='" + txtClaimApprovedBy.Text + "',Activity_Claim_Remark='" + txtRemark.Text + "',ActCmailEmpID=" + Func.Convert.iConvertToInt(sUserID) + ",Third_SubmitYN='J',Third_Remark=" + "'" + txt3rdRemark.Text + "'" + ",Third_SubmitDate=GETDATE(),CostCenter_ID=" + Func.Convert.dConvertToDouble(drpCostCenter.SelectedValue) + " where ID=" + iId;
                                }

                                objDB.ExecuteQuery(ssql);
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["Request_Type"].ToString() == "CR")
                                    {
                                        string sdtlsql = "Update TD_ActivityRequestClaim Set Apprv_VECV_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Per"]) + "',Approved='" + dt.Rows[i]["Approved"] + "',Apprv_VECV_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_VECV_Amt"]) + "',Apprv_Dealer_Per ='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Per"]) + "',Apprv_Dealer_Amt='" + Func.Convert.dConvertToDouble(dt.Rows[i]["Apprv_Dealer_Amt"]) + "' where ActivityHDrID=" + iId + "AND ID=" + Func.Convert.iConvertToInt(dt.Rows[i]["ID"]); ;
                                        objDB.ExecuteQuery(sdtlsql);
                                    }
                                }
                                objDB.CommitTransaction();
                                strMessage = "Activity  Claim Rejection Successfully";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);

                            }
                        }
                        catch (Exception ex)
                        {
                            objDB.RollbackTransaction();
                            strMessage = "Activity Claim Rejection Failed";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "confirm('" + strMessage + "')", true);
                        }
                    }

                btnReturnclaimforModification.Visible = false;
                DivBtn.Visible = false;
                
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


       
    }
}