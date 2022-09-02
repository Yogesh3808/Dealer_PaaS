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
    public partial class frmFPDA : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iFPDA_No = 0;
        string sDealerId = "";
        int iUserId = 0;
        int iMenuId = 0;
        string sDealerCode = "";
        string sUserType = "";
        int iUserRollId = 0;
        protected void Page_Init(object sender, EventArgs e)
        {         
            sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
            ToolbarC.iValidationIdForSave = 51;
            ToolbarC.iValidationIdForConfirm = 51;
            ToolbarC.bUseImgOrButton = true;
            Location.bUseSpareDealerCode = true;
            sDealerCode = Location.sDealerCode;
            Location.SetControlValue();          
            ToolbarC.iFormIdToOpenForm = 51;
            iUserRollId = Func.Convert.iConvertToInt(Session["UserRole"]);
            txtUserRoleID.Text = Func.Convert.sConvertToString(iUserRollId);          
            sUserType = (Session["UserType"] == null) ? "" : Session["UserType"].ToString();
            //MDUser Change            
            if (sUserType == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                Session["HOBR_ID"] = Func.Convert.sConvertToString(Session["DealerID"]);
            }
            //MDUser Change
            if (sUserType == "3")
            {
                lblSelectWarrantyClaim.Attributes.Add("onclick", "return ShowFPDAWarrantyClaim('" + Location.iDealerId + "');");
            }
            else
            {
                MakeEnableDisableControls("N", "N");
                lblSelectWarrantyClaim.Enabled = false;
                ToolbarC.Visible = false;
                btnPrintScarpReg.Enabled = false;
                btnPrintFPDAClaim.Enabled = false;
            }
            if (!IsPostBack)
            {
                DisplayPreviousRecord();
            }
            FillSelectionGrid();
            if (iFPDA_No != 0)
            {
                GetDataAndDisplay();
            }
            SetDocumentDetails();
            if (sUserType == "6")
            {
                ToolbarC.Visible = true;

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);

                lblSelectWarrantyClaim.Style.Add("display", "none");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {     
            btnPrintScarpReg.Attributes.Add("onclick", "return ShowReportScarpRegister();");
            btnPrintFPDAClaim.Attributes.Add("onclick", "return ShowReportFPDAClaimReport();");
          
            if (Page.IsPostBack == false)
            {
                
            }



        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
        }
        // set Document text 
        private void SetDocumentDetails()
        {
            lblTitle.Text = " FPDA  ";
            lblDocNo.Text = "FPDA No.:";
            lblDocDate.Text = "FPDA Date:";
            if (txtID.Text == "")
            {
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);            
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                btnPrintScarpReg.Enabled = false;
                btnPrintFPDAClaim.Enabled = false;           
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
                    dtPartClaimDetails.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_No", typeof(string)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_Date", typeof(string)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Part_Qty", typeof(double)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("AccPart_Qty", typeof(double)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("Box_no", typeof(string)));
                    dtPartClaimDetails.Columns.Add(new DataColumn("ChkForAccept", typeof(bool)));
                }

            }

            for (int iRowCnt = 0; iRowCnt < 1; iRowCnt++)
            {
                dr = dtPartClaimDetails.NewRow();              
                dr["Customer_Name"] = "";
                dr["Cliam_No"] = "";
                dr["Cliam_Date"] = "";
                dr["Part_Name"] = "";
                dr["Part_Qty"] = 0.00;
                dr["AccPart_Qty"] = 0.00;
                dr["Box_no"] = "";
                dr["ChkForAccept"] = 1;
                dtPartClaimDetails.Rows.Add(dr);
                dtPartClaimDetails.AcceptChanges();
            }
        Bind: ;            
            DetailsGrid.DataSource = dtPartClaimDetails;
            DetailsGrid.DataBind();
            setGridDetails("N");
            gvScrapPart.DataSource = null;
            gvScrapPart.DataBind();
        }

        //BindData to Grid
        private void BindDataToGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            if (bRecordIsOpen == true)
            {             
                iFPDA_No = 0;
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
                ClsFPDA ObjFPDA = new ClsFPDA();
                DataSet ds = new DataSet();
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    Session["FPDAClaimsParts"] = null;
                    Session["FPDAScrapParts"] = null;
                    Session["FPDAClaims"] = null;
                    hdnMinFPDADate.Value = "";
                    hdnMaxFPDADate.Value = "";
                    //Sujata 
                    ds = ObjFPDA.GetFPDA("NEW", "0", Location.iDealerId);
                    DisplayData(ds);
                    ClearFormControl();                            
                    txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "F", Location.iDealerId);                  
                    //txtDocDate.Text = "";// Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                   
                    btnPrintScarpReg.Enabled = false;
                    btnPrintFPDAClaim.Enabled = false;

                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {

                    if (sUserType == "3")
                    {
                        if (bSaveRecord("P", "N") == false) return;
                    }
                    else
                    {
                        if (bSaveRecord("Y", "P") == false) return;
                    }
                    PSelectionGrid.Style.Add("display", "");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("FPDA") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    ds = ObjFPDA.GetFPDA("All", txtID.Text, Location.iDealerId);
                    DisplayData(ds);
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (sUserType == "3")
                    {
                        if (bSaveRecord("Y", "N") == false) return;
                    }
                    else
                    {
                        if (bSaveRecord("Y", "Y") == false) return;
                    }
                    PSelectionGrid.Style.Add("display", "");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("FPDA") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    ds = ObjFPDA.GetFPDA("All", txtID.Text, Location.iDealerId);
                    DisplayData(ds);

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    btnPrintScarpReg.Enabled = true;
                    btnPrintFPDAClaim.Enabled = true;

                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (ObjFPDA.CancelConfirmFPDA(true, false, txtID.Text) == false) return;
                    //ds = ObjFPDA.GetFPDA(txtID.Text, Location.iDealerId);
                    ds = ObjFPDA.GetFPDA("All", txtID.Text, Location.iDealerId);
                    DisplayData(ds);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    btnPrintScarpReg.Enabled = true;
                    btnPrintFPDAClaim.Enabled = true;
                }

                //iClaimID = Func.Convert.iConvertToInt(txtID.Text);
                //FillSelectionGrid();
                ds = null;
                ObjFPDA = null;

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
            if (txtFromDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Claim Date From.";
                bValidateRecord = false;
            }
            if (txtToDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Claim Date To.";
                bValidateRecord = false;
            }
            if (txtDocDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the FPDA Date.";
                bValidateRecord = false;
            }
            if (txtTransporter.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Transporter.";
                bValidateRecord = false;
            }

            if (txtNoOfCases.Text == "")
            {
                sMessage = sMessage + "\\n Enter the No of Boxes.";
                bValidateRecord = false;
            }
            if (txtLRNo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the LR No.";
                bValidateRecord = false;
            }

            if (txtLRDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the LR Date.";
                bValidateRecord = false;
            }
            int iCntRec = 0;
            int iCntRecScrp = 0;

            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                iCntRec = iCntRec + (((DetailsGrid.Rows[iRowCnt].FindControl("ChkStatus") as CheckBox).Checked == true) ? 0 : 1);
            }

            for (int iRowCnt = 0; iRowCnt < gvScrapPart.Rows.Count; iRowCnt++)
            {
                iCntRecScrp = iCntRecScrp + (((gvScrapPart.Rows[iRowCnt].FindControl("ChkPStatus") as CheckBox).Checked == true) ? 0 : 1);
            }
            if (iCntRec == 0 && iCntRecScrp == 0) //Ask this to vikram sir for scrap && or ||
            {
                sMessage = sMessage + "\\n No Details To Save FPDA.";
                bValidateRecord = false;
            }

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            

            return bValidateRecord;
            //return false;
        }


        //ToSave Record
        private bool bSaveRecord(string sDealerSaveWithConfirm, string sVecvSaveWithConfirm)
        {
            int iFPDA_Hdr_ID = 0;
            ClsFPDA ObjFPDA = new ClsFPDA();
            //DataSet ds = new DataSet();
            //Sujata 03122012_Begin
            //if (bValidateRecord() == false)             
            if (bValidateRecord(sVecvSaveWithConfirm) == false)
                return false;
            //Sujata 03122012_End
            sDealerId = Func.Convert.sConvertToString(Location.iDealerId);

            if (DetailsGrid.Rows.Count == 0 && gvScrapPart.Rows.Count == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                return false;
            }

            if (ObjFPDA.bSaveFPDA(txtID.Text, sDealerId, txtDocNo.Text, txtDocDate.Text, txtLRDate.Text, txtLRNo.Text, txtNoOfCases.Text, txtTransporter.Text, txtRemarks.Text, txtFromDate.Text, txtToDate.Text, sDealerSaveWithConfirm, sVecvSaveWithConfirm, UpdateFPDADetailsSection(), ref  iFPDA_Hdr_ID, Func.Convert.sConvertToString(hdnTrNo.Value)) == false)
            {
                return false;
            }


            if (txtID.Text == "0")
            {
                txtID.Text = Convert.ToString(iFPDA_Hdr_ID);
            }
            ObjFPDA = null;
            return true;
        }
        private void FillSelectionGrid()
        {
            SearchGrid.sGridPanelTitle = "FPDA List";
            SearchGrid.AddToSearchCombo("FPDA No");
            SearchGrid.AddToSearchCombo("FPDA Date");            
            //MDUser Change
            //SearchGrid.iDealerID = Location.iDealerId;// Location.iDealerId;
            if (sUserType == "6")
            {
                SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
            }
            else
            {
                SearchGrid.iDealerID = Location.iDealerId;// Location.iDealerId;
            }
            //MDUser Change
            SearchGrid.sSqlFor = "FPDA";
            if (sUserType == "3" || sUserType == "6")
                SearchGrid.sModelPart = "N";
            else
                SearchGrid.sModelPart = "Y";
        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iFPDA_No = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        // To Display Max Record After Save
        private void DisplayCurrentRecord()
        {
            clsRFP ObjRFP = new clsRFP();
            DataSet ds = new DataSet();
            int iDealerId = 3;
            iDealerId = Location.iDealerId;
            ds = ObjRFP.GetRFP(iFPDA_No, "Max", "M", iDealerId);
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
            ClsFPDA ObjFPDA = new ClsFPDA();
            DataSet ds = new DataSet();
            if (iFPDA_No != 0)
            {
                ds = ObjFPDA.GetFPDA("All", txtID.Text, Location.iDealerId);
                DisplayData(ds);
                ObjFPDA = null;
            }
            else
            {
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                //Megha24082011  
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                btnPrintScarpReg.Enabled = false;
                btnPrintFPDAClaim.Enabled = false;
                //Megha24082011  
            }
            ds = null;
            ObjFPDA = null;
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
                iFPDA_No = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Advice_no"]);
                txtDocDate.Text = Func.Convert.tConvertToDate(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Advice_Date"]), false);
                txtTransporter.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Transporter"]);
                txtNoOfCases.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NoOfCases"]);
                txtLRNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR"]);
                txtLRDate.Text = Func.Convert.tConvertToDate(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_Date"]), false);
                txtRemarks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remarks"]);
                txtFromDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CreditDate_From"]);
                txtToDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CreditDate_To"]);
                hdnMinFPDADate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MinFPDADate"]);
                hdnMaxFPDADate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MaxFPDADate"]);

                hdnMinCreditDate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MinCreditDateFrom"]);
                hdnMaxCreditDate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MaxCreditDateTo"]);

                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                ////Display Failed Part Details
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DetailsGrid.DataSource = ds.Tables[1];
                    DetailsGrid.DataBind();
                    Session["FPDAClaimsParts"] = ds.Tables[1];
                }
                else
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }

                ////Display Failed Part Details
                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvScrapPart.DataSource = ds.Tables[2];
                    gvScrapPart.DataBind();
                    Session["FPDAScrapParts"] = (DataTable)gvScrapPart.DataSource;
                }
                else
                {
                    gvScrapPart.DataSource = null;
                    gvScrapPart.DataBind();
                }

                // If Record is Confirm or cancel then it is not editable
                if (sUserType == "3")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    //Megha21012012
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);

                    btnPrintScarpReg.Enabled = true;
                    btnPrintFPDAClaim.Enabled = true;

                }
                else
                {
                    ToolbarC.Visible = false;
                    btnPrintScarpReg.Enabled = false;
                    btnPrintFPDAClaim.Enabled = true;
                }
                // Megha21012012
                lblSelectWarrantyClaim.Style.Add("display", "");
                string Dealer_FPDA_Confirm = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_FPDA_Confirm"]);
                //Sujata 06122012_Begin
                //MakeEnableDisableControls(Dealer_FPDA_Confirm, Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vecv_FPDA_Confirm"]));
                string Dealer_FPDA_Cancel = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FPDA_Cancel"]);
                MakeEnableDisableControls(((Dealer_FPDA_Cancel == "Y") ? "R" : Dealer_FPDA_Confirm == "Y" ? "Y" : "P"), Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vecv_FPDA_Confirm"]));
                //Sujata 06122012_End
                setGridDetails(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vecv_FPDA_Confirm"]));
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                if (sUserType == "6")
                {
                    ToolbarC.Visible = true;

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);

                    btnPrintScarpReg.Enabled = true;
                    btnPrintFPDAClaim.Enabled = true;
                    lblSelectWarrantyClaim.Style.Add("display", "none");
                }
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);

                //if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FPDA_Cancel"]) == "Y")
                //{
                //    //txtDocNo.ReadOnly = true;
                //    //txtDocDate.ReadOnly = true;
                //    //txtTransporter.ReadOnly = true;
                //    //txtNoOfCases.ReadOnly = true;
                //    //txtLRNo.ReadOnly = true;
                //    //txtLRDate.ReadOnly = true;
                //    //txtRemarks.ReadOnly = true;
                //    MakeEnableDisableControls(false);
                //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                //    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                //    //Megha24082011  
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                //    //Megha24082011  
                //    lblSelectWarrantyClaim.Style.Add("display", "none");

                //}


            }
            catch (Exception ex)
            {


                //throw ex;
                Func.Common.ProcessUnhandledException(ex);
                //string strreportURL;
                //strreportURL = " var FPDADetails = null; ";
                //////strreportURL = strreportURL +" window.opener = self; ";
                //////strreportURL = strreportURL +" windowFeatures = \" top=0,left=0,resizable=yes,width= \" + (screen.width) + \",height=\" + (screen.height);\""; 
                //////// strreportURL = "'/DCS/Forms/Common/frmDocumentView.aspx" + "?RptName=" + sRptName + "'";
                //strreportURL =strreportURL + "  FPDADetails = window.showModalDialog(\"/DCS/frmError.aspx\", \"List\", \"dialogWidth:700px;dialogHeight:500px;status:no;help:no\"); " ;
                //Page.RegisterStartupScript("Close", "<script language='javascript'>" + strreportURL + "</script>");
            }

        }
        // To enable or disable fields
        private void MakeEnableDisableControls(string sDealerConfirm, string sVecvConfirm)
        {
            //bool bEnable = (sDealerConfirm == "Y") ? false : true;
            bool bEnable = (sDealerConfirm == "Y" || sDealerConfirm == "R") ? false : true;
            if (sDealerConfirm == "Y")
            {
                lblSelectWarrantyClaim.Style.Add("display", "none");
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
            }
            else if (sDealerConfirm == "R")
            {
                lblSelectWarrantyClaim.Style.Add("display", "none");
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
            }
            //Enable header Controls of Form        
            txtDocNo.Enabled = bEnable;
            txtDocDate.Enabled = bEnable;
            txtTransporter.Enabled = bEnable;
            txtNoOfCases.Enabled = bEnable;
            txtLRNo.Enabled = bEnable;
            txtLRDate.Enabled = bEnable;
            txtRemarks.Enabled = bEnable;
            txtFromDate.Enabled = bEnable;
            txtToDate.Enabled = bEnable;
            //if (bEnable == false)
            //{
            //    drpShippingLineNominationRequired.Attributes.Add("disabled", "true");
            //    drpNominatedAgency.Attributes.Add("disabled", "true");       
            //}        
            DetailsGrid.Enabled = bEnable;
            gvScrapPart.Enabled = bEnable;
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //Sujata 06122012_Begin
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            //Sujata 06122012_End
            //Megha24082011  
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);
            //Megha24082011  
            if (sUserType == "3")
                if (sDealerConfirm == "N")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //Megha24082011  
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //Megha24082011 
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                }
                else if (sDealerConfirm == "P")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmSave);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    //Megha24082011  
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    //Megha24082011  
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                }
            if (sUserType != "3")
                if (sVecvConfirm == "Y")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }
                else if (sVecvConfirm == "N")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    if (txtID.Text != "")
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    else
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //Megha24082011  
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //Megha24082011  
                }
                else if (sVecvConfirm == "P")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmSave);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //Megha24082011  
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    //Megha24082011  
                }

            if (sUserType == "6")
            {
                ToolbarC.Visible = true;

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);

                btnPrintScarpReg.Enabled = true;
                btnPrintFPDAClaim.Enabled = true;
                lblSelectWarrantyClaim.Style.Add("display", "none");
            }
        }

        // To Display Previous Record
        private void DisplayPreviousRecord()
        {
            try
            {             
                ClsFPDA ObjFPDA = new ClsFPDA();
                DataSet ds = new DataSet();
                int iDealerId = 0;
                iDealerId = Location.iDealerId; //Location.iDealerId;
               
                ClearFormControl();
                if (iDealerId != 0)
                    ds = ObjFPDA.GetFPDA("All", "0", iDealerId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {              
                            DisplayData(ds);
                        }

                        if (Func.Common.iRowCntOfTable(ds.Tables[1]) == 0)
                        {
                            BindDataToGrid(true, 0);
                            txtID.Text = "0";                          
                            txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "F", Location.iDealerId);                            
                            //txtDocDate.Text = "";
                            txtDocDate.Text = Func.Common.sGetCurrentDate(1, false);
                        }
                    }
                }
                else
                {
                    BindDataToGrid(true, 0);                   
                    txtDocNo.Text = Func.Common.sGetMaxDocNo(sDealerCode, "", "F", Location.iDealerId);
                }
                hdnTrNo.Value = Location.sDealerCode + "/F/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
                ds = null;
                ObjFPDA = null;
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
                DataSet dsPartsWarrantyClaimWise = new DataSet();
                DataTable dtPartsWarrantyClaimWise = new DataTable();
                string strwarratyIDs = "";
                if ((String)Session["FPDAClaims"] != null)
                {
                    strwarratyIDs = (String)Session["FPDAClaims"];
                }
                DataTable dtWarrantyClaimsParts = new DataTable();
                DataTable dtWarrantyScrapParts = new DataTable();
                //

                if (strwarratyIDs.Length > 1)
                {
                    strwarratyIDs = ((String)Session["FPDAClaims"]).ToString();
                    strwarratyIDs = strwarratyIDs.Substring(0, strwarratyIDs.Trim().Length - 1);
                    dsPartsWarrantyClaimWise = objDB.ExecuteStoredProcedureAndGetDataset("[SP_Get_FPDAPartsWarrantyClaimWise]", strwarratyIDs);
                    dtPartsWarrantyClaimWise = dsPartsWarrantyClaimWise.Tables[0];
                    dtWarrantyScrapParts = dsPartsWarrantyClaimWise.Tables[1];
                    if (dsPartsWarrantyClaimWise.Tables[0].Rows.Count > 0)
                    {
                        if (Session["FPDAClaimsParts"] == null)
                        {
                            DetailsGrid.DataSource = dtPartsWarrantyClaimWise;
                            DetailsGrid.DataBind();
                            Session["FPDAClaimsParts"] = dsPartsWarrantyClaimWise.Tables[0];
                            setGridDetails("N");
                        }
                        else
                        {
                            dtWarrantyClaimsParts = (DataTable)Session["FPDAClaimsParts"];
                            foreach (DataRow dr in dsPartsWarrantyClaimWise.Tables[0].Rows)
                            {
                                DataView dtView = new DataView(dtWarrantyClaimsParts);
                                dtView.RowFilter = "Cliam_No  like '" + dr["Cliam_No"] + "*' and Part_Name like '" + dr["Part_Name"] + "*'";
                                if (dtView.Count == 0)
                                {
                                    dtView = null;
                                    dtWarrantyClaimsParts.ImportRow(dr);
                                }
                            }
                            DetailsGrid.DataSource = dtWarrantyClaimsParts;
                            DetailsGrid.DataBind();
                            Session["FPDAClaimsParts"] = dsPartsWarrantyClaimWise.Tables[0];
                            setGridDetails("N");

                        }
                    }

                    if (dsPartsWarrantyClaimWise.Tables[1].Rows.Count > 0)
                    {
                        if (Session["FPDAScrapParts"] == null)
                        {
                            gvScrapPart.DataSource = dtWarrantyScrapParts;
                            gvScrapPart.DataBind();
                            Session["FPDAScrapParts"] = (DataTable)gvScrapPart.DataSource;
                        }
                        else
                        {
                            dtWarrantyScrapParts = (DataTable)Session["FPDAScrapParts"];
                            foreach (DataRow dr in dsPartsWarrantyClaimWise.Tables[1].Rows)
                            {
                                DataView dtView = new DataView(dtWarrantyScrapParts);
                                dtView.RowFilter = "Cliam_No  like '" + dr["Cliam_No"] + "*' and Part_Name like '" + dr["Part_Name"] + "*'";
                                if (dtView.Count == 0)
                                {
                                    dtView = null;
                                    dtWarrantyScrapParts.ImportRow(dr);
                                }
                            }
                            gvScrapPart.DataSource = dtWarrantyScrapParts;
                            gvScrapPart.DataBind();
                            Session["FPDAScrapParts"] = gvScrapPart.DataSource;

                        }
                    }
                }
                dsPartsWarrantyClaimWise = null;
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
            txtLRDate.Text = null;
            txtTransporter.Text = "";
            txtLRNo.Text = "";
            txtNoOfCases.Text = "";
            txtRemarks.Text = "";
            MakeEnableDisableControls("N", "N");
            txtFromDate.Text = Func.Common.sGetMonthStartDate(Func.Common.sGetCurrentDate(0, false));
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblSelectWarrantyClaim.Style.Add("display", "");
            //Megha24082011  
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            //Megha24082011        
            DetailsGrid.Enabled = true;
            gvScrapPart.Enabled = true;
            DetailsGrid.DataSource = null;
            DetailsGrid.DataBind();
            gvScrapPart.DataSource = null;
            gvScrapPart.DataBind();
        }
        private void setGridDetails(string sVecvConform)
        {
            if (sUserType == "3" || sUserType == "6")
            {
                foreach (DataControlField col in DetailsGrid.Columns)
                {
                    if (col.HeaderText == "Location")
                    {
                        int colPos = DetailsGrid.Columns.IndexOf(col);
                        DetailsGrid.Columns[colPos].Visible = false;
                    }
                }

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {
                    TextBox txtAccPartQty = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtAccPartQty");
                    txtAccPartQty.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 6 + ")");
                    txtAccPartQty.Attributes.Add("onblur", "return CheckFPDAPartAcceptedQty(event,this)");
                    if ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked != true)
                    {
                        (DetailsGrid.Rows[iRowCnt].FindControl("txtBoxNo") as TextBox).Enabled = false;
                        (DetailsGrid.Rows[iRowCnt].FindControl("txtRejRemark") as TextBox).Enabled = true;
                    }
                    else
                    {
                        (DetailsGrid.Rows[iRowCnt].FindControl("txtBoxNo") as TextBox).Enabled = true;
                        (DetailsGrid.Rows[iRowCnt].FindControl("txtRejRemark") as TextBox).Enabled = false;
                    }
                }
            }
            else
            {
                if (sVecvConform == "N" || sVecvConform == "P")
                {
                    if (DetailsGrid.Rows.Count > 0)
                    {
                        DetailsGrid.Enabled = true;
                        //if (DetailsGrid.HeaderRow.RowType == DataControlRowType.Header)
                        //    if ((DetailsGrid.HeaderRow.FindControl("ChkAcceptAll") as CheckBox) != null)
                        //        (DetailsGrid.HeaderRow.FindControl("ChkAcceptAll") as CheckBox).Enabled = false;
                        foreach (GridViewRow row in DetailsGrid.Rows)
                        {
                            if ((row.Cells[0].FindControl("txtLocation") as TextBox) != null)
                                (row.Cells[0].FindControl("txtLocation") as TextBox).Enabled = true;

                            if ((row.Cells[0].FindControl("ChkForAccept") as CheckBox) != null)
                                (row.Cells[0].FindControl("ChkForAccept") as CheckBox).Enabled = false;

                            if ((row.Cells[0].FindControl("txtBoxNo") as TextBox) != null)
                                (row.Cells[0].FindControl("txtBoxNo") as TextBox).Enabled = false;

                        }
                    }
                }
            }
        }
        //protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if ((CheckBox)e.Row.FindControl("ChkAcceptAll") != null)
        //    {
        //        ((CheckBox)e.Row.FindControl("ChkAcceptAll")).Attributes.Add("onclick", "javascript:SelectAllFPDA('" + ((CheckBox)e.Row.FindControl("ChkAcceptAll")).ClientID + "')");
        //    }


        //}

        private DataTable UpdateFPDADetailsSection()
        {
            try
            {
                DataRow dr;
                dtDetails = new DataTable();
                dtDetails.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Cliam_No", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Cliam_Date", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Part_Qty", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("AccPart_Qty", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Box_no", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("ChkForAccept", typeof(bool)));
                dtDetails.Columns.Add(new DataColumn("Location", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("RejRemark", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));                

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Sujata 05122012_Begin
                    //if ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true)
                    //{
                    //Sujata 05122012_End
                    dr = dtDetails.NewRow();
                    //dr["SRNo"] = "1";            
                    dr["Customer_Name"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblCustomer") as Label).Text;
                    dr["Cliam_No"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblCliam_No") as Label).Text;
                    dr["Cliam_Date"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblCliam_Date") as Label).Text;
                    dr["Part_Name"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblPart_Name") as Label).Text;
                    dr["Part_Qty"] = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iRowCnt].FindControl("lblPart_Qty") as Label).Text);
                    dr["AccPart_Qty"] = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iRowCnt].FindControl("txtAccPartQty") as TextBox).Text);
                    dr["Box_no"] = (DetailsGrid.Rows[iRowCnt].FindControl("txtBoxNo") as TextBox).Text;
                    if ((DetailsGrid.Rows[iRowCnt].FindControl("txtLocation") as TextBox) != null)
                        dr["Location"] = (DetailsGrid.Rows[iRowCnt].FindControl("txtLocation") as TextBox).Text;
                    if ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true)
                    {
                        dr["ChkForAccept"] = 1;
                    }
                    else
                    {
                        dr["ChkForAccept"] = 0;
                    }
                    dr["RejRemark"] = Func.Convert.sConvertToString((DetailsGrid.Rows[iRowCnt].FindControl("txtRejRemark") as TextBox).Text);
                    dr["Status"] = ((DetailsGrid.Rows[iRowCnt].FindControl("ChkStatus") as CheckBox).Checked == true) ? "N" : "Y";                    

                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                    //Sujata 05122012_Begin
                    //}
                    //Sujata 05122012_End
                }

                for (int iRowCnt = 0; iRowCnt < gvScrapPart.Rows.Count; iRowCnt++)
                {

                    //DataView dtView = new DataView(dtDetails);
                    //dtView.RowFilter = "Cliam_No  like '" + (gvScrapPart.Rows[iRowCnt].FindControl("lblCliam_No") as Label).Text + "*'";
                    //if (dtView.Count != 0)
                    //{ 
                    dr = dtDetails.NewRow();
                    //dr["SRNo"] = "1";            
                    dr["Customer_Name"] = (gvScrapPart.Rows[iRowCnt].FindControl("lblCustomer") as Label).Text;
                    dr["Cliam_No"] = (gvScrapPart.Rows[iRowCnt].FindControl("lblCliam_No") as Label).Text;
                    dr["Cliam_Date"] = (gvScrapPart.Rows[iRowCnt].FindControl("lblCliam_Date") as Label).Text;
                    dr["Part_Name"] = (gvScrapPart.Rows[iRowCnt].FindControl("lblPart_Name") as Label).Text;
                    dr["Part_Qty"] = (gvScrapPart.Rows[iRowCnt].FindControl("lblPart_Qty") as Label).Text;
                    //Sujata 03122012_Begin Accepted Qty Concept is not for Scrap temporary it is 0 ask to madam for this value.
                    //dr["AccPart_Qty"] = (DetailsGrid.Rows[iRowCnt].FindControl("txtAccPartQty") as TextBox).Text;
                    dr["AccPart_Qty"] = Func.Convert.dConvertToDouble("0");
                    dr["ChkForAccept"] = 1;
                    dr["RejRemark"] = "";
                    dr["Status"] = ((gvScrapPart.Rows[iRowCnt].FindControl("ChkPStatus") as CheckBox).Checked == true) ? "N" : "Y";
                    //Sujata 03122012_End
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                    //dtView = null;
                    //}
                }

                return dtDetails;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                throw;
            }

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


        protected void btnPrintScarpReg_Click(object sender, EventArgs e)
        {

        }
        protected void btnPrintFPDAClaim_Click(object sender, EventArgs e)
        {

        }

    }
}