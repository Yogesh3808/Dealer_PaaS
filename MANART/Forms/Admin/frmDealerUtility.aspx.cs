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
using System.IO;

namespace MANART.Forms.Admin
{
    public partial class frmDealerUtility : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iDealerLocationTypeID = 0;
        string sDealerId = "";
        int iUserId = 0;
        int iMenuId = 0;
        string sDealerCode = "";
        clsDealerUtility objdealUti;
        string strPatchName = "";
        protected void Page_Init(object sender, EventArgs e)
        {

            ////Thread.CurrentThread.CurrentCulture =  CultureInfo.CreateSpecificCulture("en-IN");
            ////Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-IN");  
            //sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
            //ToolbarC.iValidationIdForSave = 51;
            //ToolbarC.iValidationIdForConfirm = 51;
            //ToolbarC.bUseImgOrButton = true;
            //Location.bUseSpareDealerCode = true;
            //sDealerCode = Location.sDealerCode;
            //Location.SetControlValue();
            ////Megha24082011          --Report Print Option
            // ToolbarC.iFormIdToOpenForm = 51;
            // //Megha24082011

            //if (!IsPostBack)
            //{
            //    DisplayPreviousRecord();
            //}
            //FillSelectionGrid();
            //if (iFPDA_No != 0)
            //{
            //    GetDataAndDisplay();
            //}
            //SetDocumentDetails();
            Fillcombo();
            ////txtPatchName.Visible = false;
            txtPatchName.Visible = false;
            txtPatchName.Visible = false;
            lblNofiles.Visible = false;
            txtNoSplitFiles.Visible = false;
            btnSelectDealer.Visible = false;
            btnUpdate.Visible = false;
            lblPatchremarks.Visible = false;
            txtPatchRemarks.Visible = false;
            // DisplayControl(true);
            FillSelectionGrid();
            Session["IsDealerView"] = "Yes";

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtLRDate.sOnBlurScript = " return SetCurrentAndPastDate(ContentPlaceHolder1_txtLRDate_txtDocDate,'LR Date Should be Less or equal to Current Date');";
            ////lblSelectWarrantyClaim.Attributes.Add("onclick", "return ShowFPDAWarrantyClaim('" + sDealerId + "');");
            //lblSelectWarrantyClaim.Attributes.Add("onclick", "return ShowFPDAWarrantyClaim('" + Location.iDealerId + "');");
            ////lblSelectWarrantyClaim.Style.Add("display", "none");
            //FillSelectionGrid();
            lblWindiwsservice.Text = "For Date :-" + DateTime.Now.ToString() + " Windows Service Status-No Of File In Process From SAP,DMS ";
            if (!IsPostBack)
            {

                //txtQuantity.Attributes.Add("onblur", "CheckForTextBoxLostFocus(event,this," + 5 + ")");
                //txtAmount.Attributes.Add("onblur", "CheckTextBoxNullValue(event,this)");
                txtDLHODealerID.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 5 + ")");
                txtDLHODealerID.Style["text-align"] = "right";
                txtDLBRDealerID.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 5 + ")");
                txtDLBRDealerID.Style["text-align"] = "right";
                txtHOBranchCode.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 5 + ")");
                txtHOBranchCode.Style["text-align"] = "right";

                drpDLDealerType.Items.Insert(0, new ListItem("--Select--", "0"));
                ListItem lstitm1 = new ListItem("Vehicle", "V");
                drpDLDealerType.Items.Add(lstitm1);
                ListItem lstitm2 = new ListItem("Spare", "S");
                drpDLDealerType.Items.Add(lstitm2);
                ListItem lstitm3 = new ListItem("Vehicle And Spare", "B");
                drpDLDealerType.Items.Add(lstitm3);

                FillJobGrid();
                int UsreType = (Session["UserType"] != null) ? Func.Convert.iConvertToInt(Session["UserType"]) : 0;
                int UserRole = (Session["UserRole"] != null) ? Func.Convert.iConvertToInt(Session["UserRole"]) : 0;
                int UserID = (Session["UserID"] != null) ? Func.Convert.iConvertToInt(Session["UserID"]) : 0;
                // Condition Added for Eicher Admin Login
                if (UsreType == 5 && UserRole == 8 && UserID != 8)
                {
                    Menu1.Items[2].Selected = true;
                    //Menu1.Items.Remove(new MenuItem("Patch Management", "1"));
                    //Menu1.Items.Remove(new MenuItem("Dealer Live Entry", "2"));
                    Menu1.Items.RemoveAt(0);
                    Menu1.Items.RemoveAt(2);
                    Menu1.Items.RemoveAt(0);
                    //Menu1.Items.RemoveAt(2);
                    Label2.Text = "XML Generate For SAP";
                    tblWinService.Style.Add("display", "none");
                    Panel3.Style.Add("display", "none");
                    GridView2.Style.Add("display", "none");
                    MultiView1.ActiveViewIndex = 2;

                }
            }


            //if (ViewState["PatchName"] != null)
            //{
            //    DataSet ds = new DataSet();
            //    objdealUti = new clsDealerUtility();
            //    ds = objdealUti.FillPatchDeatils((string)ViewState["PatchName"], "Update");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        DetailsGrid.DataSource = ds.Tables[0];
            //        DetailsGrid.DataBind();
            //    }
            //    ds = null;
            //}
        }
        private void Fillcombo()
        {
            clsDB objDB = new clsDB();
            DataSet DsProcessName = new DataSet();
            try
            {

                DsProcessName = objDB.ExecuteQueryAndGetDataset("Select distinct PatchName as Name, ID as ID from TM_PatchHistory order by ID");
                drpPatchName.DataSource = DsProcessName.Tables[0];
                drpPatchName.DataTextField = "Name";
                drpPatchName.DataValueField = "ID";
                drpPatchName.DataBind();
                drpPatchName.Items.Insert(0, new ListItem("--Select--", "0"));
                ListItem lstitm = new ListItem("NEW", "9999");
                drpPatchName.Items.Add(lstitm);

                drpDLRolloutPatchName.DataSource = DsProcessName.Tables[0];
                drpDLRolloutPatchName.DataTextField = "Name";
                drpDLRolloutPatchName.DataValueField = "ID";
                drpDLRolloutPatchName.DataBind();

                drpDLRolloutPatchName.Items.Insert(0, new ListItem("--Select--", "0"));
                drpDLRolloutPatchName.Items.Add(lstitm);

                //drpProcessName.Attributes.Add("onChange", "OnComboValueChange1(this,'" + txtProcessName.ID + "')");
                // drpProcessName.Attributes.Add("onclick", "OnComboValueChange1(this,'" + txtProcessName.ID + "')");

                txtPatchName.Visible = false;

                drpPatchName.Visible = true;
                DsProcessName = new DataSet();
                DsProcessName = objDB.ExecuteQueryAndGetDataset("select distinct  COUNT(*) as   DlLiveLocation  from TM_DealerLocationType  inner join M_Dealer  " +
                "on TM_DealerLocationType.DealerID=M_Dealer.ID where M_Dealer.Dealer_Live='Y' ");

                if (DsProcessName.Tables[0].Rows.Count > 0)
                {
                    lblNoDlLiveLocationCount.Text = DsProcessName.Tables[0].Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (objDB != null) objDB = null;
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
            //lblTitle.Text = " FPDA  ";
            //lblDocNo.Text = "Advice No.:";
            //lblDocDate.Text = "Advice Date:";

            //if (drpIncoTerms.SelectedItem != null)
            //{
            //    if (drpIncoTerms.SelectedItem.Text == "FOB")
            //    {
            //        drpShippingLineNominationRequired.Attributes.Remove("disabled");
            //        drpNominatedAgency.Attributes.Remove("disabled");
            //    }
            //    else
            //    {
            //        drpShippingLineNominationRequired.Attributes.Add("disabled", "true");
            //        drpNominatedAgency.Attributes.Add("disabled", "true"); 
            //    }
            //}
            //if (txtID.Text == "")
            //{
            //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            //    //Megha24082011  
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            //    //Megha24082011  
            //}
        }
        // FillCombo
        private void FillCombo()
        {
            // Func.Common.BindDataToCombo(drpPaymentTerms, clsCommon.ComboQueryType.PaymentTerms, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpIncoTerms, clsCommon.ComboQueryType.INCOTerms, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpPortofDischarge, clsCommon.ComboQueryType.PortofDischargeForCountry, Location.iCountryId);
            //Func.Common.BindDataToCombo(drpModeofShipment, clsCommon.ComboQueryType.ModeofDispatch, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpNominatedAgency, clsCommon.ComboQueryType.NominatedAgency, 0);
            //Func.Common.BindDataToCombo(drpThirdPartyInspectionAgency, clsCommon.ComboQueryType.ThirdPartyInspectionAgency, 0);
        }

        // to create Emty Row To Grid
        private void CreateNewRowToDetailsTable(int iNoRowToAdd)
        {
            //    //MaxRFPModelRowCount
            //    DataRow dr;
            //    DataTable dtPartClaimDetails = new DataTable();

            //    if (iNoRowToAdd == 0)
            //    {
            //        if (dtPartClaimDetails.Rows.Count == 0)
            //        {
            //            dtPartClaimDetails.Columns.Clear();
            //            //dtPartClaimDetails.Columns.Add(new DataColumn("SRNo", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_No", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Cliam_Date", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Part_Name", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Part_Qty", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("Box_no", typeof(string)));
            //            dtPartClaimDetails.Columns.Add(new DataColumn("ChkForAccept", typeof(bool)));
            //        }

            //    }

            //    for (int iRowCnt = 0; iRowCnt < 1; iRowCnt++)
            //    {
            //        dr = dtPartClaimDetails.NewRow();
            //        //dr["SRNo"] = "1";
            //        dr["Customer_Name"] = "";
            //        dr["Cliam_No"] = "";
            //        dr["Cliam_Date"] = "";
            //        dr["Part_Name"] = "";
            //        dr["Part_Qty"] = "";
            //        dr["Box_no"] = "";
            //        dr["ChkForAccept"] = 1;
            //        dtPartClaimDetails.Rows.Add(dr);
            //        dtPartClaimDetails.AcceptChanges();
            //    }
            //Bind: ;
            //    //Session["ModelDetails"] = dtPartClaimDetails;
            //    DetailsGrid.DataSource = dtPartClaimDetails;
            //    DetailsGrid.DataBind();
        }

        //BindData to Grid
        private void BindDataToGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            //if (bRecordIsOpen == true)
            //{
            //    //CreateNewRowToDetailsTable(iNoRowToAdd);    
            //    iFPDA_No = 0;
            //}
            //else
            //{
            //    DetailsGrid.DataSource = dtDetails;
            //    DetailsGrid.DataBind();
            //}
            // SetGridControlProperty(bRecordIsOpen);
        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        private bool bValidateRecord()
        {
            //string sMessage = " Please enter the select records.";
            //bool bValidateRecord = true;
            //if (txtDocDate.Text == "")
            //{
            //    sMessage = sMessage + "\\n Enter the document date.";
            //    bValidateRecord = false;
            //}
            //if (drpIncoTerms.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Inco Terms.";
            //    bValidateRecord = false;
            //}
            //if (drpPaymentTerms.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Payment Terms.";
            //    bValidateRecord = false;
            //}


            //if (drpThirdPartyInspectionAgency.SelectedValue == "0")
            //{
            //        sMessage = sMessage + "\\n Select the Third Party Inspection Agency.";
            //        bValidateRecord = false;
            //}
            //if (drpIncoTerms.SelectedItem.Text == "FOB")
            //{
            //    if (drpNominatedAgency.SelectedValue == "0")
            //    {
            //        sMessage = sMessage + "\\n Select the Nominated Agency.";
            //        bValidateRecord = false;
            //    }
            //}

            //if (drpPortofDischarge.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Port of discharge.";
            //    bValidateRecord = false;
            //}
            //if (drpModeofShipment.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Mode of shipment";
            //    bValidateRecord = false;
            //}        
            //if (bValidateRecord == false)
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            //}
            //return bValidateRecord;
            return false;
        }
        //ToSave Record
        protected void lblSelectWarrantyClaim_Click(object sender, EventArgs e)
        {
            //DataSet dsPartsWarrantyClaimWise = new DataSet();
            //DataTable dtPartsWarrantyClaimWise = new DataTable();
            //string strwarratyClaim_no = "";
            //if ((String)Session["FPDAClaims"] != null)
            //{
            //    strwarratyClaim_no = (String)Session["FPDAClaims"];
            //}
            //DataTable dtWarrantyClaimsParts = new DataTable();
            ////

            //if (strwarratyClaim_no.Length > 1)
            //{
            //    strwarratyClaim_no = ((String)Session["FPDAClaims"]).ToString();
            //    strwarratyClaim_no = strwarratyClaim_no.Substring(0, strwarratyClaim_no.Trim().Length - 1);
            //    dsPartsWarrantyClaimWise = BLL.Func.DB.ExecuteStoredProcedureAndGetDataset("[SP_Get_FPDAPartsWarrantyClaimWise]", strwarratyClaim_no);
            //    dtPartsWarrantyClaimWise = dsPartsWarrantyClaimWise.Tables[0];
            //    if (dsPartsWarrantyClaimWise.Tables[0].Rows.Count > 0)
            //    {
            //        if (Session["FPDAClaimsParts"] == null)
            //        {
            //            DetailsGrid.DataSource = dtPartsWarrantyClaimWise;
            //            DetailsGrid.DataBind();
            //            Session["FPDAClaimsParts"] = (DataTable)DetailsGrid.DataSource;
            //        }
            //        else
            //        {
            //            dtWarrantyClaimsParts = (DataTable)Session["FPDAClaimsParts"];
            //            foreach (DataRow dr in dtPartsWarrantyClaimWise.Rows)
            //            {
            //                DataView dtView = new DataView(dtWarrantyClaimsParts);
            //                dtView.RowFilter = "Cliam_No  like '" + dr["Cliam_No"] + "*' and Part_Name like '" + dr["Part_Name"] + "*'";
            //                if (dtView.Count == 0)
            //                {
            //                    dtView = null;
            //                    dtWarrantyClaimsParts.ImportRow(dr);
            //                }
            //            }
            //            DetailsGrid.DataSource = dtWarrantyClaimsParts;
            //            DetailsGrid.DataBind();
            //            Session["FPDAClaimsParts"] = DetailsGrid.DataSource;

            //        }
            //    }
            //}
            //dsPartsWarrantyClaimWise = null;


        }
        // To Clear The form data
        private void ClearFormControl()
        {
            //    txtID.Text = "0";
            //    txtDocNo.Text = "";
            //    txtDocDate.Text = null;
            //    txtLRDate.Text = null;
            //    txtTransporter.Text = "";
            //    txtLRNo.Text = "";
            //    txtNoOfCases.Text = "";
            //    txtRemarks.Text = "";
            //    lblSelectWarrantyClaim.Style.Add("display", "");
            //    MakeEnableDisableControls(true);
            //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            //    //Megha24082011  
            //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            //Megha24082011  
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {

            string[] strIDDeatils = txtID.Text.ToString().Split('_');

            iDealerID = Func.Convert.iConvertToInt(strIDDeatils[0]);
            iDealerLocationTypeID = Func.Convert.iConvertToInt(strIDDeatils[1]);
            GetDataAndDisplay();
        }
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
        protected void drpPatchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            objdealUti = new clsDealerUtility();
            DataSet ds = new DataSet();
            string strPatchName = "";
            btnUpdate.Visible = false;
            txtPatchName.Visible = false;
            txtPatchName.Visible = false;
            lblNofiles.Visible = false;
            txtNoSplitFiles.Visible = false;
            btnSelectDealer.Visible = false;
            lblPatchremarks.Visible = false;
            txtPatchRemarks.Visible = false;
            try
            {
                strPatchName = drpPatchName.SelectedItem.Text.Trim();


                if (drpPatchName.SelectedItem.Text == "NEW")
                {
                    txtPatchName.Text = "";
                    txtPatchName.Visible = true;
                    lblNofiles.Visible = true;
                    txtNoSplitFiles.Visible = true;
                    lblPatchremarks.Visible = true;
                    txtPatchRemarks.Visible = true;
                    btnSelectDealer.Visible = true;
                    lblNoDlLivePatchesCount.Text = "";
                    lblNoDlLivePatchesDownloadCount.Text = "";
                    txtPatchName.Focus();
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();

                }
                else if (drpPatchName.SelectedItem.Text == "--Select--")
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
                else if (drpPatchName.SelectedItem.Text != "--Select--")
                {
                    txtPatchName.Visible = false;
                    txtPatchName.Visible = false;
                    lblNofiles.Visible = false;
                    txtNoSplitFiles.Visible = false;
                    btnSelectDealer.Visible = false;
                    lblPatchremarks.Visible = false;
                    txtPatchRemarks.Visible = false;

                    ds = objdealUti.FillPatchDeatils(strPatchName, "Update");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DetailsGrid.DataSource = ds.Tables[0];
                        DetailsGrid.DataBind();
                        btnUpdate.Visible = true;
                        Cache["dt"] = ds.Tables[0];
                        ViewState["Column_Name"] = "Dealer_Name";
                        ViewState["Sort_Order"] = "ASC";

                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lblNoDlLiveLocationCount.Text = ds.Tables[1].Rows[0][0].ToString();
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        lblNoDlLivePatchesCount.Text = ds.Tables[2].Rows[0][0].ToString();
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        lblNoDlLivePatchesDownloadCount.Text = ds.Tables[3].Rows[0][0].ToString();
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            objdealUti = new clsDealerUtility();
            DataSet ds = new DataSet();

            strPatchName = drpPatchName.SelectedItem.Text.Trim();
            if (drpPatchName.SelectedItem.Text == "NEW")
            {
                if (txtPatchName.Text.Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz Enter Patch Name');</script>");
                }
                else if (txtNoSplitFiles.Text.Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz Enter No of Patchs Split Files ');</script>");
                }
                else
                {
                    strPatchName = txtPatchName.Text.Trim();
                }
            }

            if (objdealUti.bSavePatchDeatils(txtPatchRemarks.Text.Trim(), UpdatePatchSection()) == false)
            {
                return;
            }
            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Patch Status Updated for All Live Dealer');</script>");
            txtPatchName.Visible = false;
            txtPatchName.Visible = false;
            txtPatchName.Visible = false;
            lblNofiles.Visible = false;
            txtNoSplitFiles.Visible = false;
            btnSelectDealer.Visible = false;
            btnUpdate.Visible = false;
            lblPatchremarks.Visible = false;
            txtPatchRemarks.Visible = false;
            Fillcombo();

            drpPatchName.SelectedItem.Text = strPatchName;
            ds = objdealUti.FillPatchDeatils(strPatchName, "Update");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DetailsGrid.DataSource = ds.Tables[0];
                DetailsGrid.DataBind();
                btnUpdate.Visible = true;
                //Session["FPDAClaimsParts"] = (DataTable)DetailsGrid.DataSource;
            }


            if (ds.Tables[1].Rows.Count > 0)
            {
                lblNoDlLiveLocationCount.Text = ds.Tables[1].Rows[0][0].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                lblNoDlLivePatchesCount.Text = ds.Tables[2].Rows[0][0].ToString();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                lblNoDlLivePatchesDownloadCount.Text = ds.Tables[3].Rows[0][0].ToString();
            }

            ViewState["PatchName"] = strPatchName;
        }

        protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((CheckBox)e.Row.FindControl("ChkPatchEntryStatusAll") != null && (CheckBox)e.Row.FindControl("ChkDownloadedStatusAll") != null)
            {
                ((CheckBox)e.Row.FindControl("ChkPatchEntryStatusAll")).Attributes.Add("onclick", "javascript:PatchEntryStatusAll('" + ((CheckBox)e.Row.FindControl("ChkPatchEntryStatusAll")).ClientID + "')");
                ((CheckBox)e.Row.FindControl("ChkDownloadedStatusAll")).Attributes.Add("onclick", "javascript:PatchDownloadedStatusAll('" + ((CheckBox)e.Row.FindControl("ChkDownloadedStatusAll")).ClientID + "')");
                if ((CheckBox)e.Row.FindControl("chkPatchEntry") != null && (CheckBox)e.Row.FindControl("chkDownloadStatus") != null)
                {
                    ((CheckBox)e.Row.FindControl("chkPatchEntry")).Attributes.Add("onclick", "javascript:PatchEntryStatusSingleOne('" + ((CheckBox)e.Row.FindControl("chkPatchEntry")).ClientID + "')");
                }
            }

        }
        protected void DetailsGrid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Increase")
            {
            }

        }

        protected void PatchEntryStatusSingleOne(object sender, EventArgs e)
        {
            if (sender != null)
            {

                CheckBox chkBx1 = sender as CheckBox;
                GridViewRow row = chkBx1.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblpatchstatus");
                Label lblpatchDownloadstatus = (Label)row.FindControl("lblpatchDownloadstatus");
                //foreach (GridViewRow di in DetailsGrid.Rows)
                //{
                //    CheckBox chkBx = (CheckBox)di.FindControl("chkDownloadStatus");
                //    Label lblpatchstatus = (Label)di.FindControl("lblpatchstatus");
                //    Label lblpatchDownloadstatus1 = (Label)di.FindControl("lblpatchDownloadstatus");
                if (lblpatchDownloadstatus.Text.Trim() == "Yes")
                {
                    chkBx1.Checked = true;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Can not Update Patch Added Status Flag Bacause This patch has been Downloaded @ Dealer END!');</script>");

                }
                //}
            }
        }
        protected void PatchDownloadStatusSingleOne(object sender, EventArgs e)
        {
            if (sender != null)
            {

                CheckBox chkBx1 = sender as CheckBox;
                GridViewRow row = chkBx1.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblpatchstatus");
                Label lblpatchDownloadstatus = (Label)row.FindControl("lblpatchDownloadstatus");
                //foreach (GridViewRow di in DetailsGrid.Rows)
                //{
                //    CheckBox chkBx = (CheckBox)di.FindControl("chkDownloadStatus");
                //    Label lblpatchstatus = (Label)di.FindControl("lblpatchstatus");
                //    Label lblpatchDownloadstatus1 = (Label)di.FindControl("lblpatchDownloadstatus");
                if (lblpatchstatus.Text.Trim() == "Yes")
                {
                    if (lblpatchDownloadstatus.Text.Trim() == "Yes")
                    {
                        chkBx1.Checked = true;
                        Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Can not Update Patch Added Status Flag Bacause This patch has been Downloaded @ Dealer END!');</script>");

                    }
                }
                else
                {
                    chkBx1.Checked = false;
                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne2", "<script language='javascript'>alert('Can not Update patch Download Flag  Bacause This patch has been Added Dealer!');</script>");
                }
                //}
            }
        }
        protected void btnSelectDealer_Click(object sender, EventArgs e)
        {
            objdealUti = new clsDealerUtility();
            DataSet ds = new DataSet();
            string strPatchName = "";
            strPatchName = drpPatchName.SelectedItem.Text.Trim();
            if (drpPatchName.SelectedItem.Text == "NEW")
            {
                if (txtPatchName.Text == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz Enter Patch Name');</script>");
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
                else if (txtNoSplitFiles.Text == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz Enter No of Patchs Split Files ');</script>");

                }
                else
                {
                    if (bValidatePatch(txtPatchName.Text.Trim()) == false) return;
                    strPatchName = txtPatchName.Text.Trim();
                    ds = objdealUti.FillPatchDeatils(strPatchName, "New");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DetailsGrid.DataSource = ds.Tables[0];
                        DetailsGrid.DataBind();
                        btnUpdate.Visible = true;
                        //Session["FPDAClaimsParts"] = (DataTable)DetailsGrid.DataSource;
                    }


                }

            }

        }
        private bool bValidatePatch(String strPatchName1)
        {
            objdealUti = new clsDealerUtility();
            DataSet ds = new DataSet();
            bool bPatchValidation;
            string strFileDestPath;
            System.IO.DirectoryInfo dir;

            strFileDestPath = ConfigurationSettings.AppSettings["PatchsPath"] + strPatchName1;

            if (System.IO.Directory.Exists(strFileDestPath))
            {
                System.IO.FileInfo[] files;
                files = new System.IO.DirectoryInfo(strFileDestPath).GetFiles("*.rar");
                if (files.Length == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Add Patch Split file in Patch Folder @ Dataexchnage Location.?');</script>");
                    return false;
                }

            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Create First Patch Name Folder In Dataexchnage Folder @ Dataexchnage Location.?');</script>");
                return false;
            }

            bPatchValidation = objdealUti.PatchValidation(strPatchName1);
            if (bPatchValidation == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This Patch Name Already Exist in Databse');</script>");
                DetailsGrid.DataSource = null;
                DetailsGrid.DataBind();
            }

            return bPatchValidation;
        }
        private void FillSelectionGrid()
        {
            //SearchGrid.bGridFillUsingSql = true;
            SearchGridView1.sGridPanelTitle = "Patch List";
            SearchGridView1.AddToSearchCombo("Patch Name");
            SearchGridView1.AddToSearchCombo("Dealer Name");
            SearchGridView1.AddToSearchCombo("Dealer Type");
            SearchGridView1.AddToSearchCombo("Dealer ID");
            SearchGridView1.sSqlFor = "PatchDetails";
            //SearchGrid.sGridPanelTitle = "Patch Details";
            SearchGridView1.sModelPart = "";
            SearchGridView1.iDealerID = -1;

            SearchGrid.sGridPanelTitle = "Dealer Live";
            SearchGrid.AddToSearchCombo("Dealer Name");
            SearchGrid.AddToSearchCombo("Dealer LIVE");
            SearchGrid.AddToSearchCombo("Partialy live");
            SearchGrid.AddToSearchCombo("Dealer ID");
            SearchGrid.AddToSearchCombo("Vehicle Code");
            SearchGrid.AddToSearchCombo("Spare Code");
            SearchGrid.AddToSearchCombo("Hierarchy Code");
            SearchGrid.sSqlFor = "LiveDealerDetails";
            //SearchGrid.sGridPanelTitle = "Patch Details";
            SearchGrid.sModelPart = "";
            SearchGrid.iDealerID = -1;

            SearchGridView2.sGridPanelTitle = "Dealer HO Branch Deatils";
            SearchGridView2.AddToSearchCombo("HO Dealer ID");
            SearchGridView2.AddToSearchCombo("HO Dealer Name");
            SearchGridView2.AddToSearchCombo("Dealer Name");
            SearchGridView2.AddToSearchCombo("HO/Branch Code");
            SearchGridView2.sSqlFor = "LiveDealerHOBRANCH";
            //SearchGrid.sGridPanelTitle = "Patch Details";
            SearchGridView2.sModelPart = "";
            SearchGridView2.iDealerID = -1;

        }
        private void DisplayControl(bool bDisplay)
        {

            if (bDisplay == true)
            {
                txtPatchName.Style.Add("display", "none");
                txtPatchName.Style.Add("display", "none");
                txtPatchName.Style.Add("display", "none");
                lblNofiles.Style.Add("display", "none");
                txtNoSplitFiles.Style.Add("display", "none");
                btnSelectDealer.Style.Add("display", "none");
                btnUpdate.Style.Add("display", "none");
                lblPatchremarks.Style.Add("display", "none");
                txtPatchRemarks.Style.Add("display", "none");
            }
            else
            {
            }
        }
        private DataTable UpdatePatchSection()
        {
            try
            {
                DataRow dr;
                dtDetails = new DataTable();
                dtDetails.Columns.Add(new DataColumn("strPatchName", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("iNoFiles", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("DealerID", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("EnteryAdd", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("DownloadStatus", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("DealerType", typeof(string)));

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {

                    dr = dtDetails.NewRow();
                    //dr["SRNo"] = "1";            
                    dr["strPatchName"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblpatchname") as Label).Text;
                    dr["iNoFiles"] = txtNoSplitFiles.Text.Trim();
                    dr["DealerID"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblDealerID") as Label).Text;

                    dr["DealerType"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblDealerType") as Label).Text;
                    if ((DetailsGrid.Rows[iRowCnt].FindControl("chkPatchEntry") as CheckBox).Checked == true)
                    {
                        dr["EnteryAdd"] = "Yes";
                    }
                    else
                    {
                        dr["EnteryAdd"] = "No";
                    }


                    if ((DetailsGrid.Rows[iRowCnt].FindControl("chkDownloadStatus") as CheckBox).Checked == true)
                    {
                        dr["DownloadStatus"] = "Yes";
                    }
                    else
                    {
                        dr["DownloadStatus"] = "No";
                    }
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();

                }
                return dtDetails;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                throw ex;
            }

        }
        protected void Menu1_MenuItemClick1(object sender, MenuEventArgs e)
        {
            MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
            //Int16 i;
            //for (i = 0; i < Menu1.Items.Count - 1; i++)
            //{ 
            //    if
            //}

            //Make the selected menu item reflect the correct imageurl
            //For i = 0 To Menu1.Items.Count - 1
            //    If i = e.Item.Value Then
            //        Menu1.Items(i).ImageUrl = "selectedtab.gif"
            //    Else
            //        Menu1.Items(i).ImageUrl = "unselectedtab.gif"
            //    End If
            //Next
        }

        protected void btnDLDealerLiveTypeUpdate_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Select Dealer from Dealer live serch List');</script>");
                return;
            }

            //if ((string)ViewState["ISPartialylive"] == "Y" || ((string)ViewState["ISlive"] == "Y"))
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer status should be required either Live or Partilay Live...!');</script>");
            //    return;
            //}

            if (drpDLDealerType.SelectedItem.Text == "--Select--")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz select Dealer location type....!');</script>");
                return;
            }

            if (ObjDealerutilitty.bDealerLocationTypeValidation(Func.Convert.iConvertToInt(txtDLDealerID.Text), drpDLDealerType.SelectedValue) == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('For this Dealer Location Type is already assigned…!' );</script>");
                return;
            }
            if (txtDLBranchCode.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter Valid Dealer Branch Code');</script>");
                return;

            }
            if (drpDLRolloutPatchName.SelectedItem.Text == "--Select--")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Plz select Dealer Rollout PatchName....!');</script>");
                return;
            }

            iDealerLocationTypeID = ObjDealerutilitty.bSaveDealerUtilityDealerLocationType(Func.Convert.iConvertToInt(txtDLDealerID.Text), drpDLDealerType.SelectedValue, drpDLDealerType.SelectedItem.Text, txtDLBranchCode.Text, Func.Convert.iConvertToInt(drpDLRolloutPatchName.SelectedValue));
            if (iDealerLocationTypeID != 0)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Location Type updated successfully....!');</script>");
            iDealerID = Func.Convert.iConvertToInt(txtDLDealerID.Text);
            txtDealerLOcatioID.Text = Func.Convert.sConvertToString(iDealerLocationTypeID);
            GetDataAndDisplay();

        }
        private void GetDataAndDisplay()
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            DataSet ds = new DataSet();
            if (iDealerID != 0)
            {
                txtDLDealerType.Text = "";
                txtDLPartilayStatus.Text = "";
                txtDLStatus.Text = "";
                txtDLBranchCode.Text = "";
                drpDLDealerType.SelectedValue = "0";
                btnDLDealerPartialyLive.Enabled = true;
                btnDLDealerLiveUpdate.Enabled = true;
                btnDLDealerRolloutUpdate.Enabled = true;
                btnDLDealerLiveTypeUpdate.Enabled = true;
                drpDLRolloutPatchName.SelectedValue = "0";
                ds = ObjDealerutilitty.GetDeatlerDeatilsforLive(iDealerID, iDealerLocationTypeID);
                if (ds != null)
                {
                    ViewState["ISlive"] = null;
                    ViewState["ISPartialylive"] = null;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //iDealerID = ds.Tables[0].Rows[0]["ID"].ToString();

                        txtDLDealerID.Text = iDealerID.ToString();
                        txtDealerLOcatioID.Text = iDealerLocationTypeID.ToString();
                        txtDLDealerName.Text = ds.Tables[0].Rows[0]["Dealer_Name"].ToString();
                        txtDLDealercity.Text = ds.Tables[0].Rows[0]["Dealer_City"].ToString();
                        txtDLDEalerCOde.Text = ds.Tables[0].Rows[0]["Dealer_Code"].ToString();
                        txtDLHierarchyCode.Text = ds.Tables[0].Rows[0]["Hierarchy Code"].ToString();


                        btnDLDealerLiveUpdate.Enabled = true;
                        btnDLDealerPartialyLive.Enabled = true;


                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        txtDLDealerType.Text = ds.Tables[1].Rows[0]["DealerLocationType"].ToString();
                        drpDLDealerType.SelectedValue = ds.Tables[1].Rows[0]["DealerType"].ToString();
                        txtDLPartilayStatus.Text = ds.Tables[1].Rows[0]["partialy_live"].ToString();
                        ViewState["ISPartialylive"] = ds.Tables[1].Rows[0]["partialy_live"].ToString();
                        txtDLStatus.Text = ds.Tables[1].Rows[0]["Dealer_Live"].ToString();
                        ViewState["ISlive"] = ds.Tables[1].Rows[0]["Dealer_Live"].ToString();
                        txtDLBranchCode.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Branch Code"]);
                        if (Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["RollOutPatch_ID"].ToString()) != "")
                        {
                            drpDLRolloutPatchName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["RollOutPatch_ID"].ToString());
                        }
                        else
                        {
                            drpDLRolloutPatchName.SelectedValue = "0";
                        }
                        if (ds.Tables[1].Rows[0]["partialy_live"].ToString() == "Y")
                        {
                            btnDLDealerPartialyLive.Enabled = false;
                        }
                        if (ds.Tables[1].Rows[0]["Dealer_Live"].ToString() == "Y")
                        {
                            btnDLDealerLiveUpdate.Enabled = false;
                            btnDLDealerPartialyLive.Enabled = false;
                            btnDLDealerRolloutUpdate.Enabled = false;
                        }
                        if (ds.Tables[1].Rows[0]["DealerType"].ToString() == "B")
                        {
                            btnDLDealerLiveTypeUpdate.Enabled = false;
                        }
                    }
                }
                ObjDealerutilitty = null;
            }

            ds = null;
            ObjDealerutilitty = null;
            btnDLDealerPartialyLive.Enabled = false;
            btnDLDealerPartialyLive.Visible = false;
        }

        protected void btnDLDealerLiveUpdate_Click1(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Select Dealer from Dealer live serch List');</script>");
                return;
            }


            if (txtDLPartilayStatus.Text == "N" || txtDLPartilayStatus.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Make Dealer First Partilay Live and Set Location Type...!');</script>");
                return;
            }
            if (txtDLStatus.Text == "Y")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This Dealer is already Lived...!');</script>");
                return;
            }

            //if ((string)ViewState["ISlive"] == "Y")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This Dealer Already has been Lived...!');</script>");
            //    return;
            //}

            if (ObjDealerutilitty.bSaveDealerUtilityLiveStaus(Func.Convert.iConvertToInt(txtDLDealerID.Text), Func.Convert.iConvertToInt(txtDealerLOcatioID.Text)) == true)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer live status updated successfully....!');</script>");
            iDealerID = Func.Convert.iConvertToInt(txtDLDealerID.Text);
            iDealerLocationTypeID = Func.Convert.iConvertToInt(txtDealerLOcatioID.Text);
            GetDataAndDisplay();
        }
        protected void btnHOBranchUpdate_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtDLHODealerID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter HO Dealer ID?');</script>");
                return;
            }
            if (txtDLBRDealerID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter Dealer ID?');</script>");
                return;
            }
            if (txtHOBranchCode.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter HO/Branch Code?');</script>");
                return;
            }
            //check HO Dealer ID Present 
            if (ObjDealerutilitty.bDealeridPresent(Func.Convert.iConvertToInt(txtDLHODealerID.Text)) == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('HO Dealer ID has not defined in Dealer Master ( As Domestic Dealer)!' );</script>");
                return;
            }

            //check Dealer ID Present 
            if (ObjDealerutilitty.bDealeridPresent(Func.Convert.iConvertToInt(txtDLBRDealerID.Text)) == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer ID has not defined in Dealer Master ( As Domestic Dealer)!' );</script>");
                return;
            }

            //check  Allready entey exists in database
            if (ObjDealerutilitty.bDealerHOBranchPresent(Func.Convert.iConvertToInt(txtDLHODealerID.Text), Func.Convert.iConvertToInt(txtDLBRDealerID.Text), txtHOBranchCode.Text.Trim()) == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This HO-Branch Entry already exists...!' );</script>");
                return;
            }


            //check  Entry for HO Dealer ID
            if ((txtDLHODealerID.Text != txtDLBRDealerID.Text) || (txtHOBranchCode.Text != "00"))
            {
                if (ObjDealerutilitty.bDealerHOPresent(Func.Convert.iConvertToInt(txtDLHODealerID.Text)) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('First Add HO Dealer Entry with Code as - 00...!' );</script>");
                    return;
                }

            }
            if (ObjDealerutilitty.bSaveDealerUtilityDealerHOBranchEntry(Func.Convert.iConvertToInt(txtDLHODealerID.Text), Func.Convert.iConvertToInt(txtDLBRDealerID.Text), txtHOBranchCode.Text.Trim()) == true)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO-Branch Entry updated successfully....!');</script>");



            //if (ObjDealerutilitty.bDealerHOEntryValidation(Func.Convert.iConvertToInt(txtDLDealerID.Text), drpDLDealerType.SelectedValue) == false)
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('For this Dealer Location Type is already assigned…!' );</script>");
            //    return;
            //}

            //if (ObjDealerutilitty.bSaveDealerUtilityLiveStaus(Func.Convert.iConvertToInt(txtDLDealerID.Text)) == true)
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer live status updated successfully....!');</script>");

        }
        private void FillJobGrid()
        {
            DataSet ds = new DataSet();
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            ds = ObjDealerutilitty.FillSQLJobDeatils();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView2.DataSource = ds.Tables[0];
                GridView2.DataBind();
            }

        }
        protected void btnbtnSQLJOBDisable(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {

                Button lblJoStatus = sender as Button;
                GridViewRow row = lblJoStatus.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblEnabled");
                Label lblJobName = (Label)row.FindControl("lblname");
                if (lblpatchstatus.Text.Trim() == "Disabled")
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Can not update disable status. This JOb Already Disable Mode.!');</script>");

                }
                else
                {
                    if (ObjDealerutilitty.bUpdateSqlJobStatus(lblJobName.Text.Trim(), 0) == true)
                    {
                        Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                        FillJobGrid();
                    }
                }

            }
            ObjDealerutilitty = null;
        }
        protected void btnbtnSQLJOBEnable(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {

                Button lblJoStatus = sender as Button;
                GridViewRow row = lblJoStatus.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblEnabled");
                Label lblJobName = (Label)row.FindControl("lblname");
                if (lblpatchstatus.Text.Trim() == "Enabled")
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Can not update Enable status. This Job Already Enable Mode.!');</script>");

                }
                else
                {
                    if (ObjDealerutilitty.bUpdateSqlJobStatus(lblJobName.Text.Trim(), 1) == true)
                    {
                        Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                        FillJobGrid();
                    }
                }

            }
            ObjDealerutilitty = null;
        }
        protected void btnDealerXMLGen_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (ObjDealerutilitty.bDealerUtilityLiveDealerSpecificXMLGen() == true)
            {
                Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
            }
            ObjDealerutilitty = null;
        }
        protected void btnPingSingleSTN_Click(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB and add try_catch_finally by Shyamal on 28032012
            clsDB objDB = new clsDB();
            try
            {
                if (txtChassisNo.Text.Trim().Length == 0)
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('STN Chassis No is Mandatory ');</script>");
                    return;
                }
                if (txtCrDate.Text.Trim().Length == 0)
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('STN Chassis Date is Mandatory ');</script>");
                    return;
                }

                objDB.ExecuteStoredProcedure("SP_DCSUploadSAPMasterTransRequestSingle", txtChassisNo.Text.Trim(), txtCrDate.Text.Trim());
                Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Successfully pinged for STN No');</script>");

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
        protected void btnDLDealerPartialyLive_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Select Dealer from Dealer live serch List');</script>");
                return;
            }

            if ((string)ViewState["ISlive"] == "Y")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This Dealer Already has been Lived...!');</script>");
                return;
            }

            if (ObjDealerutilitty.bSaveDealerUtilityPartialyLiveStaus(Func.Convert.iConvertToInt(txtDLDealerID.Text)) == true)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Partialy live status updated successfully....!');</script>");
            iDealerID = Func.Convert.iConvertToInt(txtDLDealerID.Text);
            iDealerLocationTypeID = Func.Convert.iConvertToInt(txtDealerLOcatioID.Text);
            GetDataAndDisplay();
        }
        protected void btnWindowsServiceStatus_Click(object sender, EventArgs e)
        {
            String strFilter = "*.xml";
            int nFiles = 0;
            String strFileDestPath;
            String strDriveInfo = "";
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPathDCS"] + @"DCS\DCSFileInArchive\Processed\" + DateTime.Now.ToString("dd-MMM-yyyy").ToString();
            nFiles = MagicFindFileCount(strFileDestPath, strFilter);
            lblNoOfDMSFileInProcessed.Text = "No OF DMS Files Processed :- " + nFiles.ToString();
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPathDCS"] + @"DCS\DCSFileInArchive\Exceptions\" + DateTime.Now.ToString("dd-MMM-yyyy").ToString();
            nFiles = MagicFindFileCount(strFileDestPath, strFilter);
            lblNoOfDMSFileInException.Text = "No OF DMS Files Exception :- " + nFiles.ToString();
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPath"] + @"SAP\SAPFileInArchive\Processed\" + DateTime.Now.ToString("dd-MMM-yyyy").ToString();
            nFiles = MagicFindFileCount(strFileDestPath, strFilter);

            lblNoOfSAPFileInProcessed.Text = "No OF SAP Files Processed :- " + nFiles.ToString();
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPath"] + @"SAP\SAPFileInArchive\Exceptions\" + DateTime.Now.ToString("dd-MMM-yyyy").ToString();
            nFiles = MagicFindFileCount(strFileDestPath, strFilter);
            lblNoOfSAPFileInException.Text = "No OF SAP Files Exception :- " + nFiles.ToString();
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPath"] + @"SAP\FileIn\";
            nFiles = MagicFindFileCountForSubDirectory(strFileDestPath, strFilter);
            lblNoOFSAPFileInProcessing.Text = "No SAP Files in Pipeline for processes  :- " + nFiles.ToString();
            strFileDestPath = ConfigurationSettings.AppSettings["DataExchnageRootPathDCS"] + @"DCS\FileIn\";
            nFiles = MagicFindFileCountForSubDirectory(strFileDestPath, strFilter);
            lblNoOFDMSFileInProcessing.Text = "No DMS Files in Pipeline for processes  :- " + nFiles.ToString();
            //string[] fileNames = Directory.GetFiles(strFileDestPath, "*.xml", SearchOption.AllDirectories);
            //int fileCount = fileNames.Count(); 
            //long fileSize = fileNames.Select(file => new FileInfo(file).Length).Sum(); // 
            //to Get Available drive spaces on 114 Server: 
            DriveInfo[] oDrvs = System.IO.DriveInfo.GetDrives();

            foreach (var Drv in oDrvs)
            {
                if (Drv.IsReady)
                {
                    strDriveInfo = strDriveInfo + Drv.Name + " - " + ((Double)(Drv.AvailableFreeSpace) / 1073741824).ToString("0.0000") + " , ";
                }
            }
            lblSpacesOn114.Text = lblSpacesOn114.Text + strDriveInfo;

        }
        private int MagicFindFileCount(string strDirectory, string strFilter)
        {
            int nFiles = 0;
            if (Directory.Exists(strDirectory))
            {
                System.IO.DirectoryInfo dire;
                nFiles = Directory.GetFiles(strDirectory, strFilter).Length;
                dire = new System.IO.DirectoryInfo(strDirectory);
                nFiles = dire.GetFiles(strFilter).Length;
                //foreach (String dir in Directory.GetDirectories(strDirectory))
                //{
                //    //nFiles += Directory.GetFiles(dir, strFilter).Length;
                //    dire = new System.IO.DirectoryInfo(dir + "\\");
                //    nFiles += dire.GetFiles(strFilter).Length;   
                //}
            }
            return nFiles;
        }
        private int MagicFindFileCountForSubDirectory(string strDirectory, string strFilter)
        {
            int nFiles = 0;
            if (Directory.Exists(strDirectory))
            {
                string[] files = System.IO.Directory.GetFiles(strDirectory, "*.xml", System.IO.SearchOption.AllDirectories);


                ////nFiles = Directory.GetFiles(strDirectory, strFilter).Length;
                ////dire = new System.IO.DirectoryInfo(strDirectory);
                ////nFiles = dire.GetFiles(strFilter).Length;
                //foreach (String dir in Directory.GetDirectories(strDirectory))
                //{
                //    System.IO.DirectoryInfo dire;
                //    dire = new System.IO.DirectoryInfo(@"C:\DataExchange\DCS\FileIn\48");
                //    nFiles =nFiles+ dire.GetFiles(strFilter).Length;
                //    dire = null;
                //}
                nFiles = files.Length;
            }
            return nFiles;
        }
        protected void btnDLDealerRolloutUpdate_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtDLPartilayStatus.Text == "N" || txtDLPartilayStatus.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Make Dealer First Partilay Live and Set Location Type...!');</script>");
                return;
            }
            if (ObjDealerutilitty.bSaveDealerUtilityRollloutDatUploadedONFTP(Func.Convert.iConvertToInt(txtDLDealerID.Text), Func.Convert.iConvertToInt(txtDealerLOcatioID.Text)) == true)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Rollout Data Uploaded on FTP update status  successfully....!');</script>");
            iDealerID = Func.Convert.iConvertToInt(txtDLDealerID.Text);
            iDealerLocationTypeID = Func.Convert.iConvertToInt(txtDealerLOcatioID.Text);
            GetDataAndDisplay();
            ObjDealerutilitty = null;
        }
        protected void btnDLDealerBranchCodeUpdate_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (txtID.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Select Dealer from Dealer live serch List');</script>");
                return;
            }
            if (txtDLBranchCode.Text == "")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter Valid Dealer Branch Code');</script>");
                return;
            }
            if (!(txtDLPartilayStatus.Text == "Y" || txtDLStatus.Text == "Y"))
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Make Dealer Either partilay or live....!');</script>");
                return;
            }
            if (ObjDealerutilitty.bSaveDealerUtilityBranchCodeUpdate(Func.Convert.iConvertToInt(txtDLDealerID.Text), Func.Convert.iConvertToInt(txtDealerLOcatioID.Text), txtDLBranchCode.Text.ToString()) == true)
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Branch Code updated  successfully....!');</script>");
            iDealerID = Func.Convert.iConvertToInt(txtDLDealerID.Text);
            iDealerLocationTypeID = Func.Convert.iConvertToInt(txtDealerLOcatioID.Text);
            GetDataAndDisplay();
            ObjDealerutilitty = null;
            ObjDealerutilitty = null;

        }

        protected void DetailsGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == ViewState["Column_Name"].ToString())
            {
                if (ViewState["Sort_Order"].ToString() == "ASC")
                    RebindData(e.SortExpression, "DESC");
                else
                    RebindData(e.SortExpression, "ASC");
            }
            else
                RebindData(e.SortExpression, "ASC");
        }
        private void RebindData(string sColimnName, string sSortOrder)
        {
            DataTable dt = (DataTable)Cache["dt"];
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder;
            DetailsGrid.DataSource = dt;
            DetailsGrid.DataBind();
            ViewState["Column_Name"] = sColimnName;
            ViewState["Sort_Order"] = sSortOrder;

        } 
    }
}