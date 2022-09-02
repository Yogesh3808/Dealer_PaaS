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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Threading;
using System.Globalization;

namespace MANART.Forms.Warranty
{
    public partial class frmCouponClaim : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtHdr = new DataTable();
        private DataTable dtJbGrpTaxDetails = new DataTable();
        private int iCouponClaimID = 0;
        string sDealerId = "";
        int iUserId = 0;
        int iMenuId = 0;
        string sDealerCode = "";
        string sUserType = "";
        int iUserRollId = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
            //txtDealerCode.Text = Session["sDealerCode"].ToString();
            txtDealerCode.Text = (Session["sDealerCode"] == null) ? "" : Session["sDealerCode"].ToString();
            txtUserType.Text = Session["UserType"].ToString();
            //ToolbarC.iValidationIdForSave = 51;
            //ToolbarC.iValidationIdForConfirm = 51;
            ToolbarC.iFormIdToOpenForm =25;
            ToolbarC.bUseImgOrButton = true;
            Location.bUseSpareDealerCode = true;
            sDealerCode = Location.sDealerCode;
            Location.SetControlValue();
            //ToolbarC.iFormIdToOpenForm = 51;
            iUserRollId = Func.Convert.iConvertToInt(Session["UserRole"]);
            txtUserRoleID.Text = Func.Convert.sConvertToString(iUserRollId);
            sUserType = (Session["UserType"] == null) ? "" : Session["UserType"].ToString();
            //MDUser Change            
            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                Session["HOBR_ID"] = Func.Convert.sConvertToString(Session["DealerID"]);
            }
            //MDUser Change
            lblSelectWarrantyClaim.Attributes.Add("onclick", "return ShowFPDAWarrantyClaim('" + Location.iDealerId + "');");

            if (!IsPostBack)
            {
                DisplayPreviousRecord();
            }
            FillSelectionGrid();
            if (iCouponClaimID != 0)
            {
                GetDataAndDisplay();
            }
            SetDocumentDetails();
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (Page.IsPostBack == false)
            {

            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //MDUser Change
            if (txtUserType.Text == "6")
            {
                FillSelectionGrid();
            }//MDUser Change
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
        }
        // set Document text 
        private void SetDocumentDetails()
        {
            lblTitle.Text = " Coupon Claim  ";
            lblDocNo.Text = "Coupon Claim No.:";
            lblDocDate.Text = "Coupon Claim Date:";
            if (txtID.Text == "")
            {
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);            
            }
        }
        // FillCombo
        private void FillCombo()
        {

        }

        // to create Emty Row To Grid
        private void CreateNewRowToDetailsTable(int iNoRowToAdd)
        {
            //MaxRFPModelRowCount
            DataRow dr;
            DataTable dtPartClaimDetails = new DataTable();

            if (iNoRowToAdd == 0)
            {
                if (dtPartClaimDetails.Rows.Count == 0)
                {
                    dtPartClaimDetails.Columns.Clear();
                    dtPartClaimDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Jobcard_HDR_ID", typeof(int)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_No", typeof(string)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_Date", typeof(string)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Part_Qty", typeof(double)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("AccPart_Qty", typeof(double)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("Box_no", typeof(string)));
                    //dtPartClaimDetails.Columns.Add(new DataColumn("ChkForAccept", typeof(bool)));
                }

            }

            for (int iRowCnt = 0; iRowCnt < 1; iRowCnt++)
            {
                dr = dtPartClaimDetails.NewRow();
                dr["ID"] = 0;
                dr["Jobcard_HDR_ID"] = 0;

                //dr["Customer_Name"] = "";
                //dr["Cliam_No"] = "";
                //dr["Cliam_Date"] = "";
                //dr["Part_Name"] = "";
                //dr["Part_Qty"] = 0.00;
                //dr["AccPart_Qty"] = 0.00;
                //dr["Box_no"] = "";
                //dr["ChkForAccept"] = 1;
                dtPartClaimDetails.Rows.Add(dr);
                dtPartClaimDetails.AcceptChanges();
            }
        Bind: ;
            DetailsGrid.DataSource = dtPartClaimDetails;
            DetailsGrid.DataBind();
            setGridDetails("N");       
        }

        //BindData to Grid
        private void BindDataToGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            if (bRecordIsOpen == true)
            {
                iCouponClaimID = 0;
            }
            else
            {
                DetailsGrid.DataSource = dtDetails;
                DetailsGrid.DataBind();
                setGridDetails("N");
            }
        }



        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
        }


        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                clsCouponClaim ObjCouponClaim = new clsCouponClaim();
                DataSet ds = new DataSet();
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    Session["CouponDtls"] = null;                    
                    Session["CouponClaims"] = null;

                    //Sujata  
                    ClearFormControl();
                    ds = ObjCouponClaim.GetCouponClaim("NEW", 0, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]),drpClaimType.SelectedValue);
                    DisplayData(ds);                   
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CPN" + drpClaimType.SelectedValue.ToString() , Location.iDealerId);
                    txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CO" + ((drpClaimType.SelectedValue.ToString() == "P") ? "D" : "F"), Location.iDealerId);
                    //txtDocDate.Text = "";// Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                { 
                   if (bSaveRecord( "N") == false) return;
                  
                    PSelectionGrid.Style.Add("display", "");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Coupon Claim ") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    ds = ObjCouponClaim.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                    DisplayData(ds);
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {                    
                    if (bSaveRecord("Y") == false) return;                 
                    PSelectionGrid.Style.Add("display", "");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Coupon Claim ") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    ds = ObjCouponClaim.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                    DisplayData(ds);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (ObjCouponClaim.CancelConfirmCouponClaim(true, txtID.Text) == false) return;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Coupon Claim ") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    ds = ObjCouponClaim.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                    DisplayData(ds);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    
                }

                //iClaimID = Func.Convert.iConvertToInt(txtID.Text);
                //FillSelectionGrid();
                ds = null;
                ObjCouponClaim = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        //Sujata 03122012_Begin
        //private bool bValidateRecord()
        private bool bValidateRecord(string sVecvSaveWithConfirm)
        //Sujata 03122012_End
        {
            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            
            if (txtDocDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Coupon Claim Date.";
                bValidateRecord = false;
            }
            //if (txtTransporter.Text == "")
            //{
            //    sMessage = sMessage + "\\n Enter the Transporter.";
            //    bValidateRecord = false;
            //}            
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
            //return false;
        }


        //ToSave Record
        private bool bSaveRecord(string sDealerSaveWithConfirm)
        {
            int iFPDA_Hdr_ID = 0;
            clsCouponClaim ObjCouponClm = new clsCouponClaim();

            if (bValidateRecord(sDealerSaveWithConfirm) == false)
            return false;
           
            sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
            DataTable dtHdr = new DataTable();            
            
            UpdateHdrValueFromControl(dtHdr);

            dtHdr.Rows[0]["Claim_confirm"] = sDealerSaveWithConfirm;
            //Get Coupon Details
            if (bFillDetailsFromCouponGrid() == false) return false;

            bFillDetailsFromTaxGrid();

            if (DetailsGrid.Rows.Count == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter details record');</script>");
                return false;
            }

            if (ObjCouponClm.bSaveCoupon(dtHdr,Location.sDealerCode, dtDetails,dtJbGrpTaxDetails, ref  iFPDA_Hdr_ID) == false)
            {
                return false;
            }


            if (txtID.Text == "0")
            {
                txtID.Text = Convert.ToString(iFPDA_Hdr_ID);
            }
            ObjCouponClm = null;
            return true;
        }
        private void FillSelectionGrid()
        {
            SearchGrid.sGridPanelTitle = "Coupon List";
            SearchGrid.AddToSearchCombo("Coupon Claim No");
            SearchGrid.AddToSearchCombo("Coupon Claim Date");
            SearchGrid.AddToSearchCombo("Status");            
            //MDUser Change
            //SearchGrid.iDealerID = Location.iDealerId;// Location.iDealerId;
            if (txtUserType.Text == "6")
            {
                SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
            }
            else
            {
                SearchGrid.iDealerID = Location.iDealerId;
            }
            //MDUser Change
            SearchGrid.sSqlFor = "CouponClaim";         
        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iCouponClaimID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        // To Display Max Record After Save
        private void DisplayCurrentRecord()
        {
            clsRFP ObjRFP = new clsRFP();
            DataSet ds = new DataSet();
            int iDealerId = 3;
            iDealerId = Location.iDealerId;
            ds = ObjRFP.GetRFP(iCouponClaimID, "Max", "M", iDealerId);
            if (ds == null) // if no Data Exist
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                return;
            }
            txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
            DisplayData(ds);
            ObjRFP = null;
            ds = null;
            ObjRFP = null;
        }
        private void GetDataAndDisplay()
        {
            clsCouponClaim ObjCouponClm = new clsCouponClaim();
            DataSet ds = new DataSet();
            if (iCouponClaimID != 0)
            {                
                //MDUser Change
                //ds = ObjCouponClm.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                if (txtUserType.Text == "6")
                {
                    ds = ObjCouponClm.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Location.iDealerId), drpClaimType.SelectedValue);
                }
                else
                {
                    ds = ObjCouponClm.GetCouponClaim("All", Func.Convert.iConvertToInt(txtID.Text), Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                }
                //MDUser Change
                DisplayData(ds);
                ObjCouponClm = null;
            }
            else
            {
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                //Megha24082011  
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);      
                //Megha24082011  
            }
            ds = null;
            ObjCouponClm = null;
        }

        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                bool bRecordIsOpen = true;


                txtID.Text = "0";
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                ////Display Header 
                iCouponClaimID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Coupon_Claim_No"]);
                txtDocDate.Text = Func.Convert.tConvertToDate(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]), false);
                hdnCustID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                txtClmAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Coupon_Claim_Amt"]);
                drpClaimType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ClaimType"]);
                txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_name"]);
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_confirm"]);
                //hdnCustTaxTag.Value = "I";
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                hdnISDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);

                drpClaimType.Enabled = (Func.Convert.iConvertToInt(txtID.Text) == 0) ? true : false;

                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cancel"]);   
       
                ////Display Failed Part Details
                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    DetailsGrid.DataSource = ds.Tables[1];
                //    DetailsGrid.DataBind();
                dtDetails = ds.Tables[1];
                Session["CouponDtls"] = ds.Tables[1];
                //}
                //else
                //{
                //    DetailsGrid.DataSource = null;
                //    DetailsGrid.DataBind();
                //}
                BindDataToGrid(false, 0);

                dtJbGrpTaxDetails = ds.Tables[2];
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                CreateNewRowToTaxGroupDetailsTable();
                BindDataToGrid();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;

                // If Record is Confirm or cancel then it is not editable

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true); //print option enable for save & confirm
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);                

                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                }
                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);

                }
                lblSelectWarrantyClaim.Style.Add("display", "");

                //if (hdnConfirm.Value == "Y" || hdnCancle.Value == "Y") lblSelectWarrantyClaim.Style.Add("display", "none");
                if (hdnConfirm.Value == "Y" || hdnCancle.Value == "Y" || txtUserType.Text == "6") lblSelectWarrantyClaim.Style.Add("display", "none");

                setGridDetails(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_confirm"]));
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["bShowBtn"]).Trim() == "Y") ? true : false);

                txtApprNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CouponInvoiceNo"]);
                txtApprDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CouponInvoiceDate"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
            }
            catch (Exception ex)
            {
                //throw ex;
                Func.Common.ProcessUnhandledException(ex);           
            }
        }
        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {           
            //Enable header Controls of Form        
            txtDocNo.Enabled = bEnable;
            txtDocDate.Enabled = bEnable;         
            
            DetailsGrid.Enabled = bEnable;            
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);            
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);            
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);
            //MDUser Change
            clsCommon objCommon = null;
            objCommon = new clsCommon();
            if (objCommon.sUserRole == "15" || objCommon.sUserRole == "19")//MDUser Change                
            {
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                lblSelectWarrantyClaim.Visible = false;
            }
            //MDUser Change
            
        }

        // To Display Previous Record
        private void DisplayPreviousRecord()
        {
            try
            {
                clsCouponClaim ObjCouponClaim = new clsCouponClaim();
                DataSet ds = new DataSet();
                int iDealerId = 0;
                iDealerId = Location.iDealerId; //Location.iDealerId;

                ClearFormControl();                
                Session["CouponDtls"] = null;
                Session["CouponClaims"] = null;

                //Sujata  
                ClearFormControl();
                //ds = ObjCouponClaim.GetCouponClaim("NEW", 0, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                if (txtUserType.Text == "6")
                {
                    ds = ObjCouponClaim.GetCouponClaim("NEW", 0, Location.iDealerId, Location.iDealerId, drpClaimType.SelectedValue);
                }
                else
                {
                    ds = ObjCouponClaim.GetCouponClaim("NEW", 0, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), drpClaimType.SelectedValue);
                }
                DisplayData(ds);
                //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CPN" + drpClaimType.SelectedValue.ToString(), Location.iDealerId);
                //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "C" + drpClaimType.SelectedValue.ToString(), Location.iDealerId);
                txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CO" + ((drpClaimType.SelectedValue.ToString() == "P") ? "D" : "F"), Location.iDealerId);
                //txtDocDate.Text = "";// Func.Common.sGetCurrentDate(Location.iCountryId, false);
                txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                hdnTrNo.Value = Location.sDealerCode + "/CO/" + ((drpClaimType.SelectedValue.ToString() == "P") ? "D" : "F") + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);                
                if (txtUserType.Text =="6") ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
              
                ds = null;
                ObjCouponClaim = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }



        protected void lblSelectWarrantyClaim_Click(object sender, EventArgs e)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet dsCouponJobardwise = new DataSet();             
                string strJobcardID = "";
                DataTable dtCouponDtls = new DataTable();
                strJobcardID = hdnJobcardID.Value;
                dsCouponJobardwise = objDB.ExecuteStoredProcedureAndGetDataset("[SP_Get_CouponDtlsJobcardWise]", strJobcardID, (txtID.Text == "") ? 0 : Func.Convert.iConvertToInt(txtID.Text), Func.Convert.iConvertToInt(Location.iDealerId));
                dtCouponDtls = dsCouponJobardwise.Tables[0];
                if (dsCouponJobardwise.Tables[0].Rows.Count > 0)
                {
                    DetailsGrid.DataSource = dtCouponDtls;
                    DetailsGrid.DataBind();
                    dtDetails = dsCouponJobardwise.Tables[0];
                    Session["CouponDtls"] = dsCouponJobardwise.Tables[0];
                    setGridDetails("N");

                    BindDataToGrid(false, 0);

                    dtJbGrpTaxDetails = dsCouponJobardwise.Tables[1];
                    Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                    CreateNewRowToTaxGroupDetailsTable();
                    BindDataToGrid();
                    Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                }
                drpClaimType.Enabled = false;

                //if ((String)Session["CouponClaims"] != null)
                //{
                //    strJobcardID = (String)Session["CouponClaims"];
                //}
                //DataTable dtCouponDtls = new DataTable();
                //if (strJobcardID.Length > 1)
                //{
                //    strJobcardID = ((String)Session["CouponClaims"]).ToString();
                //    strJobcardID = strJobcardID.Substring(0, strJobcardID.Trim().Length - 1);

                //    strJobcardID =hdnJobcardID.Value;
                //    dsCouponJobardwise = objDB.ExecuteStoredProcedureAndGetDataset("[SP_Get_CouponDtlsJobcardWise]", strJobcardID, (txtID.Text == "") ? 0 : Func.Convert.iConvertToInt(txtID.Text), Func.Convert.iConvertToInt(Location.iDealerId));
                //    dtCouponDtls = dsCouponJobardwise.Tables[0];
                //    if (dsCouponJobardwise.Tables[0].Rows.Count > 0)
                //    {
                //        DetailsGrid.DataSource = dtCouponDtls;
                //        DetailsGrid.DataBind();
                //        dtDetails = dsCouponJobardwise.Tables[0];
                //        Session["CouponDtls"] = dsCouponJobardwise.Tables[0];
                //        setGridDetails("N");                        
                        
                //        BindDataToGrid(false, 0);

                //        dtJbGrpTaxDetails = dsCouponJobardwise.Tables[1];
                //        Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                //        CreateNewRowToTaxGroupDetailsTable();
                //        BindDataToGrid();
                //        Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                //    }
                //}
                dsCouponJobardwise = null;
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        // To Clear The form data
        private void ClearFormControl()
        {
            txtID.Text = "0";
            txtDocNo.Text = "";
            txtDocDate.Text = null;          
            txtCustomerName.Text = "";            
            txtRemarks.Text = "";
            //MakeEnableDisableControls(false);        
            lblSelectWarrantyClaim.Style.Add("display", "");
            //Megha24082011  
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            //Megha24082011        
            DetailsGrid.Enabled = true;            
            DetailsGrid.DataSource = null;
            DetailsGrid.DataBind();            
        }

        private void setGridDetails(string sVecvConform)
        {
            try
            {
                int idtRowCnt = 0;
                hdnJobcardID.Value = "";
                double dAmt = 0;
                double dTotAmtWtTax = 0;
                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {

                    TextBox txtJobcardHDRID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtJobcardHDRID");
                    Label lblJobcardNo = (Label)DetailsGrid.Rows[iRowCnt].FindControl("lblJobcardNo");

                    TextBox txtLTax = (TextBox)(DetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox);
                    TextBox txtLTax1 = (TextBox)(DetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox);
                    TextBox txtLTax2 = (TextBox)(DetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox);
                    TextBox txtLTotTaxAmt = (TextBox)(DetailsGrid.Rows[iRowCnt].FindControl("txtLTotTaxAmt") as TextBox);
                    Label lblCouponAmtWtTax = (Label)(DetailsGrid.Rows[iRowCnt].FindControl("lblCouponAmtWtTax") as Label); 

                    txtJobcardHDRID.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Jobcard_HDR_ID"]);
                    lblJobcardNo.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["JobcardNo"]);

                    txtLTax.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Tax"]).Trim();
                    txtLTax1.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Tax1"]).Trim();
                    txtLTax2.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Tax2"]).Trim();

                    txtLTotTaxAmt.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxAmt"]).Trim();

                    dTotAmtWtTax = Math.Round(Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["TotAmtWtTax"]), 2);

                    lblCouponAmtWtTax.Text = Func.Convert.sConvertToString(dTotAmtWtTax);                    

                    int iDtlID = 0;
                    if (idtRowCnt < dtDetails.Rows.Count)
                    {
                        iDtlID = Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["ID"]);
                    }
                    hdnJobcardID.Value = hdnJobcardID.Value + ((hdnJobcardID.Value != "") ? "," : "") + Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Jobcard_HDR_ID"]);
                    //dAmt = dAmt + Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Total_Amt"]);
                    dAmt = Math.Round(dAmt + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["TotAmtWtTax"]), 2);                    
                }
                Session["CouponClaims"] = hdnJobcardID.Value;
                txtClmAmt.Text = dAmt.ToString("0.00"); 
            }
            catch (Exception ex)
            { 

            }
        }
        //protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if ((CheckBox)e.Row.FindControl("ChkAcceptAll") != null)
        //    {
        //        ((CheckBox)e.Row.FindControl("ChkAcceptAll")).Attributes.Add("onclick", "javascript:SelectAllFPDA('" + ((CheckBox)e.Row.FindControl("ChkAcceptAll")).ClientID + "')");
        //    }


        //}

        private DataTable UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;
                //ID		Dlr_Code			Completed	Rejected		Total_Credit_amt	XMLRequest		

                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Coupon_Claim_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_Id", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Coupon_Claim_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Claim_confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ClaimType", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Coupon_Claim_No"] = txtDocNo.Text;
                dr["Claim_date"] = txtDocDate.Text;
                dr["Dealer_Id"] = Location.iDealerId;
                dr["DlrBranchID"] = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                dr["CustID"] = Func.Convert.iConvertToInt(hdnCustID.Value.ToString());
                dr["Coupon_Claim_Amt"] = Func.Convert.dConvertToDouble(txtClmAmt.Text);
                dr["Claim_confirm"] = "N";
                dr["ClaimType"] =  drpClaimType.SelectedValue.ToString();
                dr["UserID"] = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                dr["DocGST"] = Func.Convert.sConvertToString(hdnISDocGST.Value);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();

                return dtHdr;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                throw;
            }

        }

        private bool bFillDetailsFromCouponGrid()
        {
            int iCntForDelete = 0;
            int iCntForSelect = 0;
            dtDetails =(DataTable) Session["CouponDtls"];            
            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {                
                dtDetails.Rows[iRowCnt]["Jobcard_HDR_ID"] = (DetailsGrid.Rows[iRowCnt].FindControl("txtJobcardHDRID") as TextBox).Text;
                dtDetails.Rows[iRowCnt]["JobcardNo"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblJobcardNo") as Label).Text;
                dtDetails.Rows[iRowCnt]["Status"] = "N";
                dtDetails.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((DetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox).Text);
                dtDetails.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((DetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox).Text);
                dtDetails.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((DetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox).Text);

                dtDetails.Rows[iRowCnt]["TaxAmt"] = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iRowCnt].FindControl("txtLTotTaxAmt") as TextBox).Text);
                dtDetails.Rows[iRowCnt]["TotAmtWtTax"] = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iRowCnt].FindControl("lblCouponAmtWtTax") as Label).Text);    
                
                TextBox txtJobcardHDRID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtJobcardHDRID");

                if(txtJobcardHDRID.Text.ToString() !="" && txtJobcardHDRID.Text.ToString() != "0")
                {
                    iCntForSelect = iCntForSelect + 1;
                }

                CheckBox Chk = (CheckBox)DetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                if (Chk.Checked == true)
                {
                    dtDetails.Rows[iRowCnt]["Status"] = "D";
                    iCntForDelete++;
                }                         
            }
            if (iCntForDelete == dtDetails.Rows.Count || iCntForSelect == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the Coupon Details.');</script>");
                return false;
            }            
            return true;
        }

        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFormControl();
                FillSelectionGrid();
                //// SearchGrid.bIsCollapsable = false; 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {

            ClearFormControl();
            FillSelectionGrid();
        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            ClearFormControl();
            FillSelectionGrid();
        }

        private void BindDataToGrid()
        {
            try
            {
                GrdPartGroup.DataSource = dtJbGrpTaxDetails;
                GrdPartGroup.DataBind();


                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private bool bFillDetailsFromTaxGrid()
        {

            dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];
            int iCouponID = 0;
            int iSrvjobID = 0;
            bool bValidate = true;

            //For Tax Details
            dtJbGrpTaxDetails = (DataTable)(Session["JbGrpTaxDetails"]);

            //if (bSaveTmTxDtls == true)
            //{                
            for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
            {
                //Group Code
                TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                dtJbGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                //Group Name
                TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                //Get Net Amount
                TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                //Get Discount Perc
                TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                //Get Discount Amount
                TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                // Get Tax
                DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                //Get Tax Percentage                
                DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                //Get Tax Amount
                TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                // Get Tax1
                DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                //Get Tax1 Percentage                
                DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                //Get Tax1 Amount
                TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                // Get Tax2
                DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                //Get Tax2 Percentage                
                DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                //Get Tax2 Amount
                TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                // Get Total
                TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
            }
            return bValidate;
        }

        private void SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString("L");
                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    ////if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");

                    ////Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    ////Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");
                    
                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtCustTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("TaxTag");
                    

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    //DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' ");

                    //DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");


                    //DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");

                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    
                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    ////Additional Tax 2
                    //DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    //Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    //Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    //Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
                    txtTax2Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer2.SelectedValue) == 0 ? "0" : drpTaxPer2.SelectedItem.Text);

                    drpTax.Enabled = false;
                    drpTax1.Enabled = false;
                    drpTax2.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlPropertyTaxCalculation()
        {
            try
            {
                double dGrpTotal = 0;
                double dGrpDiscPer = 0;
                double dGrpDiscAmt = 0;
                double dGrpTaxAppAmt = 0;

                double dGrpMTaxPer = 0;
                double dGrpMTaxAmt = 0;

                double dGrpTax1Per = 0;
                double dGrpTax1Amt = 0;

                double dGrpTax2Per = 0;
                double dGrpTax2Amt = 0;

                double dDocTotalAmtFrPFOther = 0;
                double dDocDiscAmt = 0;
                double dDocLSTAmt = 0;
                double dDocCSTAmt = 0;
                double dDocTax1Amt = 0;
                double dDocTax2Amt = 0;
                string sGrpMTaxTag = "";

                double TotalOA = 0;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0.00";
                }

                for (int i = 0; i < DetailsGrid.Rows.Count; i++)
                {
                    TextBox txtJobcardHDRID = (TextBox)DetailsGrid.Rows[i].FindControl("txtJobcardHDRID");
                    CheckBox ChkForDelete = (CheckBox)DetailsGrid.Rows[i].FindControl("ChkForDelete");

                    if (txtJobcardHDRID.Text.Trim() != "0" && txtJobcardHDRID.Text.Trim() != "" && ChkForDelete.Checked == false)
                    {
                        //string sFirstFiveDigit = "";
                        ////TextBox txtLabourCode = (TextBox)DetailsGrid.Rows[i].FindControl("txtLabourCode");
                        //sFirstFiveDigit = "";// txtLabourCode.Text.ToString().Substring(0, 5);

                        Label lblCouponAmt = (Label)DetailsGrid.Rows[i].FindControl("lblCouponAmt");

                        //TextBox txtGrNo = (TextBox)(DetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox txtLTax = (TextBox)(DetailsGrid.Rows[i].FindControl("txtLTax") as TextBox);
                        TextBox txtLTax1 = (TextBox)(DetailsGrid.Rows[i].FindControl("txtLTax1") as TextBox);
                        TextBox txtLTax2 = (TextBox)(DetailsGrid.Rows[i].FindControl("txtLTax2") as TextBox);

                        double LTotal = 0;

                        LTotal = Func.Convert.dConvertToDouble(lblCouponAmt.Text.Trim());


                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            //TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                            DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                            TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            //if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)
                            if (drpTax.SelectedValue == txtLTax.Text)
                            {
                                double dGrnetinvamt= Math.Round(Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal), 2);

                                txtGrnetinvamt.Text = dGrnetinvamt.ToString("0.00");
                            }
                        }
                    }
                }


                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", "none");
                    GrdPartGroup.Rows[iRowCnt].Cells[4].Style.Add("display", "none");

                    GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", "none");
                    GrdPartGroup.Rows[iRowCnt].Cells[5].Style.Add("display", "none");

                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    //group Percentage
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    //group Discount Amount
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    //Doc Discount Amount
                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Sujata 23092014 Begin
                    //dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    if (sTax1ApplOn == "1")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    //Sujata 23092014 End

                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

                    //Sujata 23092014 Begin
                    //dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    //Sujata 23092014 End

                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 2);
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));

                    //txtDTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));
                    //txtDTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));
                    //txtDTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));
                }

                //double dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                //     Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                //txtClaimAmt.Text = dClaimAmount.ToString("0.00");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToTaxGroupDetailsTable()
        {
            try
            {
                string sGrCode = "";
                int iLTaxID = 0;
                int iLTaxID1 = 0;
                int iLTaxID2 = 0;

                dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];

                Boolean bDtSelPartRow = false;
                dtJbGrpTaxDetails.Clear();

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iLTaxID = 0;
                    iLTaxID1 = 0;
                    iLTaxID2 = 0;
                    bDtSelPartRow = false;

                    //TextBox txtLGroupCode = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");
                    //sGrCode = txtLGroupCode.Text.Trim();                    
                    TextBox txtJobcardHDRID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtJobcardHDRID");

                    sGrCode = "L";

                    CheckBox ChkForDelete = (CheckBox)DetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0 && (txtJobcardHDRID.Text.Trim() != "0" && txtJobcardHDRID.Text.Trim() != ""))
                    {
                        TextBox txtLTax = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtLTax");
                        iLTaxID = Func.Convert.iConvertToInt(txtLTax.Text);

                        TextBox txtLTax1 = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtLTax1");
                        iLTaxID1 = Func.Convert.iConvertToInt(txtLTax1.Text);

                        TextBox txtLTax2 = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtLTax2");
                        iLTaxID2 = Func.Convert.iConvertToInt(txtLTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iLTaxID) &&
                            iLTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iLTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Spares" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iLTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iLTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iLTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }               

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void drpClaimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CPN" + drpClaimType.SelectedValue.ToString(), Location.iDealerId);
            //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "C" + drpClaimType.SelectedValue.ToString(), Location.iDealerId);
            txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "CO" + ((drpClaimType.SelectedValue.ToString() == "P") ? "D" : "F"), Location.iDealerId);
            hdnTrNo.Value = (txtID.Text == "0" || txtID.Text == "") ? Location.sDealerCode + "/CO/" + ((drpClaimType.SelectedValue.ToString() == "P") ? "D" : "F") + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim()) : hdnTrNo.Value;
        }
                
    }
}