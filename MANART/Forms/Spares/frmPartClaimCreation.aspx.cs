using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using MANART.WebParts;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Web.UI.HtmlControls;
using System.Net;


namespace MANART.Forms.Spares
{
    public partial class frmPartClaimCreation : System.Web.UI.Page
    {
        int sSupplierId = 0;
        private int iPCID = 0;
        int DealerID = 0;
        string sDealerCode = "";
        private DataTable dtDetails = new DataTable();
        private DataTable dtTaxDetails = new DataTable();
        private DataTable dtGrpTaxDetails = new DataTable();
        private DataTable Acc_dtTaxDetails = new DataTable();
        private DataTable Acc_dtGrpTaxDetails = new DataTable();
        private DataSet dsCreatedBy = new DataSet();
        private bool bDetailsRecordExist = false;
        clsPartClaim objPartClaim = null;
        clsSparePO objPO = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        private int UsreType;
        int iUser_ID = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;

        private DataTable dtFileAttach = new DataTable();
        private DataTable dtInsuDoc = new DataTable();

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                //ToolbarC.iValidationIdForSave = 2;
                //ToolbarC.iValidationIdForConfirm = 2;
                //ToolbarC.iFormIdToOpenForm = 101;
                ToolbarC.bUseImgOrButton = true;

                ExportLocation.bUseSpareDealerCode = true;
                ExportLocation.SetControlValue();
                ExportLocation.Visible = false;

                iUser_ID = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtDealerCode.Text = sDealerCode;
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (!IsPostBack)
                {
                    Session["PartDetails"] = null;
                    Session["GrpTaxDetails"] = null;
                    Session["TaxDetails"] = null;
                    FillCombo();
                    FillGRNNo();
                }

                SearchGrid.sGridPanelTitle = "Part Claim List";

                if (iPCID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                //SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ExportLocation.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
            ExportLocation.TypeSelectedIndexChanged += new EventHandler(Location_TypeSelectedIndexChanged);
            DealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            sSupplierId = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["PartDetails"] = null;
                Session["GrpTaxDetails"] = null;
                Session["TaxDetails"] = null;
                GetCreatedBy();
                //DisplayPreviousRecord();
            }
            //FillCombo();
        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //sSupplierId = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);
                //Sup_Type = ExportLocation.iSupType;
                //DealerID = ExportiDealerID;
                //FillCombo();
                //FillSelectionGrid();
                //Session["PartDetails"] = null;
                //GenerateClaimNo();
                //GetCreatedBy();
                //DisplayPreviousRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void Location_TypeSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //FillCombo();
                //FillSelectionGrid();
                //Session["PartDetails"] = null;
                //GenerateClaimNo();
                //GetCreatedBy();
                //DisplayPreviousRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpClaimType, clsCommon.ComboQueryType.PartClaimType, 0, "");
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iPCID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();
                objPartClaim = new clsPartClaim();
                if (iPCID != 0)
                {
                    ds = objPartClaim.Get_PartClaim(iPCID, "All", iDealerID, 0, "");
                    sNew = "N";
                    DisplayData(ds);
                    objPartClaim = null;
                }
                else
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objPartClaim = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Get Data from Material receipt
        private void DisplayPartClaimRecord()
        {
            try
            {
                DataSet ds = new DataSet();
                objPartClaim = new clsPartClaim();
                ds = objPartClaim.Get_PartClaim(iPCID, "New", iDealerID, Func.Convert.iConvertToInt(drpClaimType.SelectedValue), Func.Convert.sConvertToString(drpGRNNo.SelectedValue));

                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                //txtClaimNo.Text = objPartClaim.GenerateClaimNO(sDealerCode, iDealerID, Func.Convert.iConvertToInt(drpClaimType.SelectedValue), ExportLocation.bDistributor);
                GenerateClaimNo();
                txtClaimDate.Text = Func.Common.sGetCurrentDate(1, false);
                txtValidityDate.Text = Func.Common.sGetCurrentDate(1, false);
                string sDocName = "";
                int ClaimType = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                if (ClaimType == 1) sDocName = "PDS";
                else if (ClaimType == 2) sDocName = "PDE";
                else if (ClaimType == 3) sDocName = "PDW";
                else if (ClaimType == 4) sDocName = "PDM";
                else if (ClaimType == 5) sDocName = "PDD";
                hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + sDocName;
                objPartClaim = null;
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayPreviousRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                DealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);


                objPartClaim = new clsPartClaim();
                ds = objPartClaim.Get_PartClaim(iPCID, "New", iDealerID, Func.Convert.iConvertToInt(drpClaimType.SelectedValue), Func.Convert.sConvertToString(drpGRNNo.SelectedValue));

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                //txtPONo.Text = Func.Common.sGetMaxDocNo(sDealerCode, "", "PO", Func.Convert.iConvertToInt(ExportiDealerID));
                txtClaimDate.Text = Func.Common.sGetCurrentDate(1, false);
                objPartClaim = null;
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                //Display Header
                if (iPCID != 0)
                {
                    txtGrnNo.Visible = true;
                    drpClaimType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Type_ID"]);
                    txtClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_No"]);
                    txtClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Date"]);
                    txtGrnNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]);
                    hdnGrnNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]);
                    drpGRNNo.Visible = false;
                    drpClaimType.Enabled = false;
                }
                else
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]) == "")
                    {
                        drpGRNNo.SelectedValue = "0";
                    }
                    else
                        drpGRNNo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]);
                }
                //drpGRNNo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]);

                txtGRN_Date.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_Date"]);
                txtInvoiceNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_No"]);
                txtInvoice_Date.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_Date"]);
                txtLR_No.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_No"]);
                txtLR_Date.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_Date"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                if (drpClaimType.SelectedValue == "5")
                {
                    chkInsuranceDoc.Items.Clear();
                    dtInsuDoc = ds.Tables[3];
                    txtInsuCmpyName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsuCmpyName"]);
                    txtInsuCoverNoteNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsuCoverNoteNo"]);
                    txtValidityDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsuValidityDate"]);
                }
                else
                {
                    trInsuranceDts.Visible = false;
                    trInsuranceDoc.Visible = false;
                }
                trApproveDetails.Visible = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Status"]) == "Y") ? true : false;
                Acc_PPartGroupDetails.Visible = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Status"]) == "Y" ? true : false;
                Acc_PCntTaxDetails.Visible = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Status"]) == "Y" ? true : false;
                txtApproveClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_No"]);
                txtApprovalDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Date"]);
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Status"]) == "Y")
                {
                    Acc_dtGrpTaxDetails = ds.Tables[6];
                    ViewState["AccGrpTaxDetails"] = Acc_dtGrpTaxDetails;

                    Acc_dtTaxDetails = ds.Tables[7];
                    ViewState["AccTaxDetails"] = Acc_dtTaxDetails;
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, true);
                }

                //txtCreatedBy.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_CreatedBy"]);

                txtClaimDate.Enabled = false;
                Session["PartDetails"] = null;
                Session["GrpTaxDetails"] = null;
                Session["TaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["PartDetails"] = dtDetails;
                dtGrpTaxDetails = ds.Tables[4];
                Session["GrpTaxDetails"] = dtGrpTaxDetails;
                dtTaxDetails = ds.Tables[5];
                Session["TaxDetails"] = dtTaxDetails;
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                BindDataToGrid();

                //Display Attachment Details
                dtFileAttach = ds.Tables[2];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();

                //New Code
                CreateNewRowToTaxGroupDetailsTable();
                ReBindDataToTax();
                Session["GrpTaxDetails"] = dtGrpTaxDetails;

                ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmConfirm);
                    //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, true);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]) == "Y")
                {
                    bEnableControls = false;
                }

                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ReBindDataToTax()
        {
            GrdPartGroup.DataSource = dtGrpTaxDetails;
            GrdPartGroup.DataBind();

            GrdDocTaxDet.DataSource = dtTaxDetails;
            GrdDocTaxDet.DataBind();

            SetGridControlPropertyTax();
            SetGridControlPropertyTaxCalculation();
        }

        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form  
            txtClaimNo.Enabled = bEnable;
            //txtClaimDate.Enabled = bEnable;
            txtGRN_Date.Enabled = bEnable;
            txtInvoiceNo.Enabled = bEnable;
            txtInvoice_Date.Enabled = bEnable;
            // Insu datails
            txtInsuCmpyName.Enabled = bEnable;
            txtInsuCoverNoteNo.Enabled = bEnable;
            txtValidityDate.Enabled = bEnable;
            chkInsuranceDoc.Enabled = bEnable;

            PartGrid.Enabled = bEnable;
            FileAttchGrid.Enabled = bEnable;
            Acc_GrdPartGroup.Enabled = false;
            Acc_GrdDocTaxDet.Enabled = false;
            txtApproveClaimNo.Enabled = false;
            txtApprovalDate.Enabled = false;

            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, bEnable);
        }
        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerId = 3;
                //DealerID = Func.Convert.iConvertToInt(ExportiDealerID);
                DealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);

                objPartClaim = new clsPartClaim();
                //ds = objPartClaim.GetDescripancy(iPCID, "Max", DealerID, ExportLocation.bDistributor, 0);
                ds = null;
                if (ds == null) // if no Data Exist
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    sNew = "N";
                    DisplayData(ds);
                }
                else
                {
                    DisplayData(ds);
                    Session["PartDetails"] = null;
                    Session["GrpTaxDetails"] = null;
                    Session["TaxDetails"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objPartClaim = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool bEnableCtrl = true;
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    drpClaimType.Enabled = true;
                    drpClaimType.SelectedIndex = -1;
                    drpGRNNo.Visible = true;
                    txtGrnNo.Visible = false;
                    ClearControl();
                    chkInsuranceDoc.Items.Clear();
                    DisplayPreviousRecord();

                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false, false) == false) return;
                    if (txtID.Text == "")
                    {

                        FillSelectionGrid();
                        DisplayCurrentRecord();
                        PSelectionGrid.Style.Add("display", "");
                        return;
                    }
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bSaveRecord(true, false) == false) return;
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (bSaveRecord(false, true) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                iPCID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
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
            CPEPartDetails.Collapsed = false;
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
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmNew, false);
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
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Claim No");
                SearchGrid.AddToSearchCombo("Claim Date");
                SearchGrid.AddToSearchCombo("Claim Status");
                SearchGrid.AddToSearchCombo("Claim Type");
                SearchGrid.iDealerID = DealerID;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUserId) + "," + ExportLocation.bDistributor;
                SearchGrid.sSqlFor = "PartClaimCreation";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                string cntPartID = "";
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Claim_Type_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Claim_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_Date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("GRN_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GRN_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Invoice_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Invoice_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LR_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LR_Date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Approval_Status", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Uploaded", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_TotalQty", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Claim_TotalItems", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Claim_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Claim_CreatedBy", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsuCmpyName", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsuCoverNoteNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsuValidityDate", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Claim_Type_ID"] = Func.Convert.iConvertToInt(drpClaimType.SelectedValue);
                dr["Claim_No"] = txtClaimNo.Text;
                dr["Claim_Date"] = txtClaimDate.Text;
                if (txtID.Text != "")
                {
                    dr["GRN_No"] = Func.Convert.sConvertToString(txtGrnNo.Text);
                }
                else
                {
                    dr["GRN_No"] = Func.Convert.sConvertToString(drpGRNNo.SelectedValue);
                }

                dr["GRN_Date"] = txtGRN_Date.Text;
                dr["Invoice_No"] = txtInvoiceNo.Text;
                dr["Invoice_Date"] = txtInvoice_Date.Text;
                dr["LR_No"] = txtLR_No.Text;
                dr["LR_Date"] = txtLR_Date.Text;
                dr["InsuCmpyName"] = txtInsuCmpyName.Text;
                dr["InsuCoverNoteNo"] = txtInsuCoverNoteNo.Text;
                dr["InsuValidityDate"] = txtValidityDate.Text;

                dr["Dealer_ID"] = Func.Convert.sConvertToString(Session["iDealerID"]);

                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["Approval_Status"] = "N";
                dr["Is_Uploaded"] = "N";

                dr["Claim_Amt"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                dr["Claim_TotalQty"] = Func.Convert.iConvertToInt(txtTotalQty.Text);

                for (int iRCnt = 0; iRCnt < PartGrid.Rows.Count; iRCnt++)
                {
                    if (iRCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        TextBox txtPartNo11 = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtPartNo");
                        cntPartID = txtPartNo11.Text;
                        if (cntPartID != "")
                        {
                            TotCntQty = TotCntQty + 1;
                        }
                    }
                }
                dr["Claim_TotalItems"] = TotCntQty;
                dr["Claim_CreatedBy"] = txtCreatedBy.Text;

                dr["UserId"] = iUserId;
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateRecord()
        {

            string sMessage = "";
            //string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (drpClaimType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Claim Type.";
                bValidateRecord = false;
            }
            if (drpGRNNo.SelectedValue == "0" && hdnGrnNo.Value == "")
            {
                sMessage = sMessage + "\\n Please select the GRN No.";
                bValidateRecord = false;
            }
            if (drpClaimType.SelectedValue == "5" && Func.Convert.dConvertToDouble(txtTotal.Text) >= Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetDamageClaimAmt, 0, "")))
            {
                if (txtInsuCmpyName.Text == "")
                {
                    sMessage = sMessage + "\\n Please Enter Insurance Company Name.";
                    bValidateRecord = false;
                }
                if (txtInsuCoverNoteNo.Text == "")
                {
                    sMessage = sMessage + "\\n Please Enter Insurance Cover Note No.";
                    bValidateRecord = false;
                }
                if (txtValidityDate.Text == "")
                {
                    sMessage = sMessage + "\\n Please select Validity Date.";
                    bValidateRecord = false;
                }
            }
            if (bValidateRecord == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('" + sMessage + "');</script>", false);
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsPartClaim objPartClaim = new clsPartClaim();
            try
            {
                if (bValidateRecord() == false)
                {
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please enter details record');</script>", false);
                    return false;
                }
                UpdateHdrValueFromControl(dtHdr);

                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["Is_Confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Is_Cancel"] = "Y";
                }
                if (bSaveWithConfirm == true && drpClaimType.SelectedValue == "5" && Func.Convert.dConvertToDouble(txtTotal.Text) >= Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetDamageClaimAmt, 0, "")))
                {
                    bool bValidate = true;
                    string sMessage1 = "";
                    string errMessage = "";
                    int iCount = 0;
                    for (int i = 0; i < chkInsuranceDoc.Items.Count; i++)
                    {// Repair Bill/ Invoice Copy is Not Madetory for Confirmation
                        if (chkInsuranceDoc.Items[i].Selected == true && i != 4)
                        {
                            iCount++;
                        }
                        //else
                        //{
                        //    errMessage += "'"+chkInsuranceDoc.Items[i].Text+"'\n";
                        //}
                    }
                    if (chkInsuranceDoc.Items.Count - 1 != iCount)
                    {
                        sMessage1 = sMessage1 + "\\n Please Select All Insurance Document CheckList.";
                        bValidate = false;
                    }
                    else
                    {
                        if (FileAttchGrid.Rows.Count > 0 && (FileAttchGrid.Rows[0].FindControl("lblFile") as Label).Text.Trim() != "")
                            bValidate = true;
                        else
                        {
                            sMessage1 = sMessage1 + "\\n Please Upload Atleast One Insurance Document.";
                            bValidate = false;
                        }
                    }

                    if (bValidate == false)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('" + sMessage1 + "');</script>", false);
                        return false;
                    }
                }

                //Save Insurance Claim Document For Damage Claim
                if (bSaveInsuranceDocument() == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please Select Insurance Documents');</script>", false);
                    return false;
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);

                // Get Attachment
                if (bSaveAttachedDocuments() == false)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please File Attach Error');</script>", false);
                    return false;
                }
                if (bDetailsRecordExist == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please enter details record');</script>", false);
                    return false;
                }
                // Save Header and Details
                if (objPartClaim.bSaveRecordWithPart(sDealerCode, iDealerID, dtHdr, dtDetails, ref iPCID, iUserId, dtFileAttach, dtInsuDoc, dtGrpTaxDetails, dtTaxDetails) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iPCID);
                    if (bSaveWithConfirm == true)
                    {
                        //if (drpClaimType.SelectedValue.ToString() == "2" || drpClaimType.SelectedValue.ToString() == "3")
                        //{
                        //    POCreation();
                        //}
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Claim ") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Claim ") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Claim ") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        private void GenerateClaimNo()
        {
            //txtClaimNo.Text = Func.Common.sGetMaxDocNo(sDealerCode, "", sDocName, iDealerID);
            txtClaimNo.Text = Func.Convert.sConvertToString(objPartClaim.GenerateClaimNO(sDealerCode, iDealerID, Convert.ToInt32(drpClaimType.SelectedValue)));
        }
        private void GetCreatedBy()
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            UsreType = Func.Convert.iConvertToInt(Session["UserType"]);

            clsDB objDB = new clsDB();
            try
            {
                dsCreatedBy = objDB.ExecuteStoredProcedureAndGetDataset("SP_POCreatedByName", iUserId, UsreType);
                txtCreatedBy.Text = dsCreatedBy.Tables[0].Rows[0]["PO_CreatedBy"].ToString();
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
        private void FillGRNNo()
        {
            DataSet dsGRNNo = new DataSet();
            objPartClaim = new clsPartClaim();
            dsGRNNo = objPartClaim.GetGRNNoByPartClaimType(Func.Convert.iConvertToInt(drpClaimType.SelectedValue), iDealerID);
            drpGRNNo.DataSource = dsGRNNo.Tables[0];
            drpGRNNo.DataTextField = "GRN_NO";
            drpGRNNo.DataValueField = "GRN_NO";
            drpGRNNo.DataBind();
            drpGRNNo.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void drpClaimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillGRNNo();
                // Generate Claim No
                GenerateClaimNo();
                txtClaimDate.Text = Func.Common.sGetCurrentDate(1, false);
                ClearControl();
                if (Func.Convert.sConvertToString(drpClaimType.SelectedValue) != "5")
                {
                    trInsuranceDts.Visible = false;
                    trInsuranceDoc.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void drpGRNNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayPartClaimRecord();
        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strPosNo = "";
                dtDetails = (DataTable)Session["PartDetails"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                string sQtyMsg = "";
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                    {
                        for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                        {
                            TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                            TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                            TextBox txtMRDtsID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRDtsID");

                            if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text) &&
                                Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["MR_Dts_ID"]) == Func.Convert.iConvertToInt(txtMRDtsID.Text))
                            {
                                dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
                                dtDetails.Rows[iMRowCnt]["MR_Dts_ID"] = Func.Convert.iConvertToInt(txtMRDtsID.Text);


                                if (txtPartID.Text != "" && txtPartID.Text != "0")
                                {
                                    iCntForSelect = iCntForSelect + 1;
                                }
                                //PartNo Or NewPart
                                TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                                dtDetails.Rows[iMRowCnt]["Part_No"] = txtPartNo.Text;

                                //Part Name
                                TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                                dtDetails.Rows[iMRowCnt]["Part_Name"] = txtPartName.Text;

                                //Get Bill_Qty
                                TextBox txtBill_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBill_Qty");
                                dtDetails.Rows[iMRowCnt]["Bill_Qty"] = Func.Convert.dConvertToDouble(txtBill_Qty.Text);

                                //Get Recv_Qty
                                TextBox txtRecv_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecv_Qty");
                                dtDetails.Rows[iMRowCnt]["Recv_Qty"] = Func.Convert.dConvertToDouble(txtRecv_Qty.Text);

                                //Get Descripancy_Qty
                                TextBox txtDescripancy_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDescripancy_Qty");
                                dtDetails.Rows[iMRowCnt]["Descripancy_Qty"] = Func.Convert.dConvertToDouble(txtDescripancy_Qty.Text);

                                // Get Rate
                                TextBox txtRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRate");
                                dtDetails.Rows[iMRowCnt]["Rate"] = Func.Convert.dConvertToDouble(txtRate.Text);

                                // Get Total
                                TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                // Get Status                           
                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

                                // Get Wrong Part ID
                                TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
                                dtDetails.Rows[iMRowCnt]["Wrg_Part_ID"] = Func.Convert.iConvertToInt(txtWrgPartID.Text);

                                // Get Wrong Part No
                                TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
                                dtDetails.Rows[iMRowCnt]["Wrg_Part_No"] = txtWrgPartNo.Text;

                                // Get Wrong Part Name
                                TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
                                dtDetails.Rows[iMRowCnt]["Wrg_Part_Name"] = txtWrgPartName.Text;

                                // Get Retain Status
                                DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
                                dtDetails.Rows[iMRowCnt]["Retain_YN"] = Func.Convert.sConvertToString(drpRetain.SelectedValue);

                                sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Status"]);


                                if (txtPartID.Text != "" && txtDescripancy_Qty.Text != "0")
                                {
                                    if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "N")
                                    {
                                        dtDetails.Rows[iMRowCnt]["Status"] = "N";
                                        bDetailsRecordExist = true;
                                    }
                                }



                            }
                        }
                    }


                }
                //For Tax Details
                dtGrpTaxDetails = (DataTable)(Session["GrpTaxDetails"]);
                dtTaxDetails = (DataTable)(Session["TaxDetails"]);

                if (dtDetails.Rows.Count > 1 && dtGrpTaxDetails.Rows.Count == 0)
                    CreateNewRowToTaxGroupDetailsTable();

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    dtGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    dtGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    dtGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(txtTax2Per.Text);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }
                //CreateNewRowToTaxGroupDetailsTable();
                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtTaxDetails.Rows[iRowCnt]["Claim_Amt"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToDetailsTable()
        {
            try
            {
                DataRow dr;
                if (dtDetails.Rows.Count == 0)
                {
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MR_Dts_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Bill_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Recv_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Descripancy_Qty", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Retain_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_ID", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_Name", typeof(String)));
                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "0")
                    {
                        goto Exit;
                    }
                }

                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["MR_Dts_ID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Bill_Qty"] = 0.00;
                dr["Recv_Qty"] = 0.00;
                dr["Rate"] = 0.00;
                dr["Total"] = 0.00;
                dr["Status"] = "N";
                dr["Descripancy_Qty"] = 0.00;
                dr["Retain_YN"] = "N";
                dr["Wrg_Part_ID"] = 0;
                dr["Wrg_Part_No"] = "";
                dr["Wrg_Part_Name"] = "";
                //dr["Part_Claim_Status"] = "N";
                //dr["Part_Claim_No"] = 0;

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGrid()
        {
            try
            {
                //If No Data in Grid
                if (Session["PartDetails"] == null)
                {

                    CreateNewRowToDetailsTable();
                    Session["PartDetails"] = dtDetails;

                }
                else
                {
                    //dtDetails = (DataTable)Session["PartDetails"];
                    //if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                    //    CreateNewRowToDetailsTable();
                    dtDetails = (DataTable)Session["PartDetails"];
                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();
                GrdPartGroup.DataSource = dtGrpTaxDetails;
                GrdPartGroup.DataBind();
                GrdDocTaxDet.DataSource = dtTaxDetails;
                GrdDocTaxDet.DataBind();
                if (txtApproveClaimNo.Text.Trim() != "")
                {
                    Acc_GrdPartGroup.DataSource = Acc_dtGrpTaxDetails;
                    Acc_GrdPartGroup.DataBind();

                    Acc_GrdDocTaxDet.DataSource = Acc_dtTaxDetails;
                    Acc_GrdDocTaxDet.DataBind();

                    Acc_SetGridControlPropertyTax();
                    Acc_SetGridControlPropertyTaxCalculation();
                }

                SetGridControlProperty();
                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlProperty()
        {
            try
            {
                string sRecordStatus = "";

                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                double dBillQty = 0;
                double dDescripancyQty = 0;
                string sGroupCode = "";

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Func.Convert.sConvertToString(Session["iDealerID"]);

                string sPartID = "", sPartName = "", cPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        // Hide  Received Wrong part No
                        PartGrid.HeaderRow.Cells[12].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//Hide Cell

                        // Hide  Received Wrong part Name
                        PartGrid.HeaderRow.Cells[13].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "none");//Hide Cell

                        // Hide Retain Status
                        PartGrid.HeaderRow.Cells[14].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "none");//Hide Cell

                        // Hide Acc Rate
                        PartGrid.HeaderRow.Cells[16].Style.Add("display", (txtApproveClaimNo.Text.Trim() == "") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (txtApproveClaimNo.Text.Trim() == "") ? "none" : "");//Hide Cell

                        // Hide Acc Total
                        PartGrid.HeaderRow.Cells[17].Style.Add("display", (txtApproveClaimNo.Text.Trim() == "") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[17].Style.Add("display", (txtApproveClaimNo.Text.Trim() == "") ? "none" : "");//Hide Cell

                        if (Func.Convert.iConvertToInt(drpClaimType.SelectedValue) == 2) // Excess Descripancy
                        {
                            // Show Retain Status 
                            //PartGrid.HeaderRow.Cells[14].Style.Add("display", ""); // Show Header     on Dated 13122017   
                            //PartGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");//Show Cell on Dated 13122017   
                        }
                        else if (Func.Convert.iConvertToInt(drpClaimType.SelectedValue) == 3) // Wrong Supply Descripancy
                        {
                            // Show  Received Wrong part No
                            PartGrid.HeaderRow.Cells[12].Style.Add("display", ""); // Show Header        
                            PartGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "");//Show Cell

                            // Show  Received Wrong part Name
                            PartGrid.HeaderRow.Cells[13].Style.Add("display", ""); // Show Header        
                            PartGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "");//Show Cell

                            // Show Retain Status
                            //PartGrid.HeaderRow.Cells[14].Style.Add("display", ""); // Show Header   on Dated 13122017        
                            //PartGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");//Show Cell     on Dated 13122017   
                        }

                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                        sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        TextBox txtMRDtsID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRDtsID");
                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");

                        TextBox txtBill_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBill_Qty");
                        TextBox txtRecv_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecv_Qty");
                        TextBox txtDescripancy_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDescripancy_Qty");
                        TextBox txtRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRate");

                        TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
                        TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
                        DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
                        TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
                        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");

                        drpRetain.Enabled = false;
                        //Main Tax
                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);


                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                        //Additional Tax 1
                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                        DropDownList DrpPartTax1Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1Per");
                        TextBox txtPartTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax1Per");
                        if (DrpPartTax1.Items.Count == 2)
                        {
                            DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax1Per, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " and ID=" + DrpPartTax1.SelectedItem);
                            if (DrpPartTax1Per.Items.Count == 2)
                            {
                                DrpPartTax1Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax1.SelectedItem);
                            }
                            txtPartTax1Per.Text = Func.Convert.sConvertToString(DrpPartTax1Per.SelectedItem);
                        }

                        //Additional Tax 2
                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                        DropDownList DrpPartTax2Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2Per");
                        TextBox txtPartTax2Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax2Per");
                        if (DrpPartTax2.Items.Count == 2)
                        {
                            DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax2Per, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " and ID=" + DrpPartTax2.SelectedItem);
                            if (DrpPartTax2Per.Items.Count == 2)
                            {
                                DrpPartTax2Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax2.SelectedItem);
                            }
                            txtPartTax2Per.Text = Func.Convert.sConvertToString(DrpPartTax2Per.SelectedItem);
                        }

                        cPartID = txtPartNo1.Text;
                        if (cPartID != "")
                        {
                            dPartQty = 0;
                            dPartRate = 0;
                            dBillQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Bill_Qty"]);
                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Recv_Qty"]);
                            dDescripancyQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Descripancy_Qty"]);

                            dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Rate"]);

                            dPartTotal = Func.Convert.dConvertToDouble(dDescripancyQty * dPartRate);
                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                            //Status
                            sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                            if (sRecordStatus != "C")
                            {
                                dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Descripancy_Qty"]);
                                dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                            }

                            drpRetain.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Retain_YN"]);
                            txtWrgPartID.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_ID"]);
                            txtWrgPartNo.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_No"]);
                            txtWrgPartName.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_Name"]);

                            if (ExportLocation.bDistributor == "N")
                            {
                                TextBox txtPartTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                txtPartTotal.Text = Func.Convert.sConvertToString(dPartTotal);
                            }
                        }
                    }


                }

                txtTotal.Text = dTotal.ToString();
                txtTotalQty.Text = dTotalQty.ToString();
                if (Func.Convert.dConvertToDouble(dTotal) >= Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetDamageClaimAmt, 0, ""))
                    && Func.Convert.iConvertToInt(drpClaimType.SelectedValue) == 5)
                {
                    trInsuranceDts.Visible = true;
                    trInsuranceDoc.Visible = true;
                    for (int i = 0; i < dtInsuDoc.Rows.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = dtInsuDoc.Rows[i]["Doc_Name"].ToString();
                        item.Value = dtInsuDoc.Rows[i]["InsuranceDocID"].ToString();
                        item.Selected = Convert.ToBoolean(dtInsuDoc.Rows[i]["IsSelected"]);
                        chkInsuranceDoc.Items.Add(item);
                    }

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Vikram Begin 05062017_BEgin Dislay hide and label change code related to GST Appl and non appl.
                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    // END
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }


                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
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
                double TotalRev = 0.00;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                for (int i = 0; i < PartGrid.Rows.Count; i++)
                {
                    TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");

                    if (txtGrNo.Text.Trim() != "")
                    {
                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
                        }
                    }
                }
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

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

                    if (sTax1ApplOn == "1")
                    {
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }


                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

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


                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    double dDocPFPer = 0;
                    double dDocPFAmt = 0;
                    double dDocOtherPer = 0;
                    double dDocOtherAmt = 0;
                    double dDocGrandAmt = 0;

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void Acc_SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Vikram Begin 05062017_BEgin Dislay hide and label change code related to GST Appl and non appl.
                    Acc_GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    Acc_GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    Acc_GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    // END
                    TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }


                    //Tax
                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxTag = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList DrpTax1ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList drpTaxPer2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
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
        private void Acc_SetGridControlPropertyTaxCalculation()
        {
            try
            {
                for (int i = 0; i < Acc_GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    Acc_GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    Acc_GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    Acc_GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        protected void drpPartTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDetailsFromGrid(false);
            CreateNewRowToTaxGroupDetailsTable();
            Session["PartDetails"] = dtDetails;
            Session["GrpTaxDetails"] = dtGrpTaxDetails;
            Session["TaxDetails"] = dtTaxDetails;
            BindDataToGrid();
        }

        private void CreateNewRowToTaxGroupDetailsTable()
        {
            try
            {
                string sGrCode = "";
                int iPartTaxID = 0;
                int iPartTaxID1 = 0;
                int iPartTaxID2 = 0;
                Boolean bDtSelPartRow = false;
                dtGrpTaxDetails.Clear();
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                    TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                    sGrCode = txtGrNo.Text;
                    if (sGrCode.Length > 0 && txtStatus.Text != "D" && txtStatus.Text != "C")
                    {
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        iPartTaxID = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(DrpPartTax1.SelectedItem.Text);

                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(DrpPartTax2.SelectedItem.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0 && txtStatus.Text != "D" && txtStatus.Text != "C")
                    {
                        dr = dtGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        if (sGrCode.Trim() == "01")
                            dr["Gr_Name"] = "Parts";
                        else if (sGrCode.Trim() == "02")
                            dr["Gr_Name"] = "Lubricant";
                        else
                            dr["Gr_Name"] = "Local Part";

                        dr["net_inv_amt"] = 0.00;

                        dr["discount_per"] = 0.00;
                        dr["discount_amt"] = 0.00;

                        dr["Tax_Code"] = iPartTaxID;
                        dr["TAX_Percentage"] = 0.00;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0.00;

                        dr["tax1_code"] = iPartTaxID1;
                        dr["Tax1_Per"] = 0.00;
                        dr["tax1_amt"] = 0.00;

                        dr["tax2_code"] = iPartTaxID2;
                        dr["Tax2_Per"] = 0.00;
                        dr["tax2_amt"] = 0.00;

                        dr["Total"] = 0.00;


                        dtGrpTaxDetails.Rows.Add(dr);
                        dtTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ClearControl()
        {
            //drpClaimType.SelectedIndex = -1;
            //txtClaimNo.Text = "";

            drpGRNNo.SelectedIndex = -1;
            txtGRN_Date.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoice_Date.Text = "";
            txtLR_No.Text = "";
            txtLR_Date.Text = "";
            txtInsuCmpyName.Text = "";
            txtInsuCoverNoteNo.Text = "";
            txtTotal.Text = "";
            txtTotalQty.Text = "";
            PartGrid.DataSource = null;
            PartGrid.DataBind();
            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();
            GrdPartGroup.DataSource = null;
            GrdPartGroup.DataBind();
            GrdDocTaxDet.DataSource = null;
            GrdDocTaxDet.DataBind();
            txtApproveClaimNo.Text = "";
            txtApprovalDate.Text = "";

        }

        #region Attach File
        // To Save the attach Document 
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            string sSourceFileType = "";
            string sSourceFileName1 = "";
            string strNewPath = "";
            string sFilenamewithoutExt = "";
            DataRow dr;
            int iRecorFound = 0;
            string[] acceptedFileTypes = new string[12];
            acceptedFileTypes[0] = ".jpg";
            acceptedFileTypes[1] = ".pdf";
            acceptedFileTypes[2] = ".jpeg";
            acceptedFileTypes[3] = ".gif";
            acceptedFileTypes[4] = ".png";
            acceptedFileTypes[5] = ".doc";
            acceptedFileTypes[6] = ".docx";
            acceptedFileTypes[7] = ".xls";
            acceptedFileTypes[8] = ".xlsx";
            acceptedFileTypes[9] = ".ppt";
            acceptedFileTypes[10] = ".txt";
            acceptedFileTypes[11] = ".zip";

            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    string newStrstr = "";
                    //Retrieving the fullpath of the File.
                    sFilenamewithoutExt = Path.GetFileNameWithoutExtension(uploads[i].FileName);
                    newStrstr = Regex.Replace(sFilenamewithoutExt, " {2,}", " ");
                    sFilenamewithoutExt = newStrstr;
                    //sSourceFileName = Path.GetFileName(uploads[i].FileName);
                    sSourceFileType = Path.GetExtension(uploads[i].FileName).ToLower();
                    sSourceFileName = sFilenamewithoutExt + sSourceFileType;

                    string str = sFilenamewithoutExt;
                    //Regex r = new Regex(@"[~`!@#$%^&*()-+=|\{}':;.,<>/?]");
                    Regex r = new Regex(@"[~`!@#$%^&*+=|\{}':;,<>/?]");
                    if (r.IsMatch(sFilenamewithoutExt))
                    {
                        //MessageBox.Show("special characters are not allowed !!!");
                        //TextBox1.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your File Name Contains Special Characters')", true);
                        return false;

                    }

                    bool acceptFile = false;
                    if (sSourceFileName.Trim() != "")
                    {
                        //should we accept the file?
                        for (int j = 0; j <= 11; j++)
                        {
                            if (sSourceFileType == acceptedFileTypes[j])
                            {
                                //accept the file, yay!
                                acceptFile = true;
                                break;
                            }
                        }
                        if (!acceptFile)
                        {
                            lblMessage.Text = "The file you are trying to upload is not a permitted file type!";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            return false;
                        }
                        //File Size
                        int iFileSize = uploads[i].ContentLength;
                        double filelimit = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetFileSizeLimit, 0, ""));
                        if (iFileSize > Func.Convert.iConvertToInt(filelimit))
                        {
                            // File exceeds the file maximum size
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please make sure your file size is less than 3 MB')", true);
                            return false;
                        }


                        if (sSourceFileName.Trim() != "" && (iFileSize <= Func.Convert.iConvertToInt(filelimit)) && acceptFile == true)
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

                            //string[] splClaimNo = txtClaimNo.Text.Split('/');
                            //if (splClaimNo.Length > 1)
                            //{
                            //    lblFileName.Text = "";
                            //    for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                            //        lblFileName.Text = lblFileName.Text + splClaimNo[iCnt];
                            //}
                            sSourceFileName1 = Func.Convert.sConvertToString(iDealerID) + "_" + txtClaimNo.Text.Trim() + "_" + sSourceFileName;
                            sSourceFileName1 = sSourceFileName1.Replace("/", "");
                            dr["File_Names"] = sSourceFileName1;
                            //dr["File_Names"] = Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                            dr["UserId"] = Func.Convert.sConvertToString(Session["UserID"]);
                            dr["Status"] = "S";


                            //Saving it in temperory Directory.                                       
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Parts\\Part Claim");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }

                            //uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));
                            uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + sSourceFileName1));

                            strNewPath = sPath + "Parts\\Part Claim" + "\\" + sSourceFileName1;
                            dr["Path"] = strNewPath;

                            dtFileAttach.Rows.Add(dr);
                            dtFileAttach.AcceptChanges();
                        }
                    }//END String is Empty

                }//END Try
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
            DataRow dr;
            dtFileAttach = new DataTable();
            //Get Header InFormation        
            dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("Path", typeof(string)));
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
                    dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));
                    Label lblDelete = (Label)(FileAttchGrid.Rows[iGridRowCnt].FindControl("lblDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                    }
                    dr["Path"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblPath") as Label).Text;
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

                for (int iColCnt = 0; iColCnt < FileAttchGrid.Columns.Count; iColCnt++)
                {
                    if (dtFileAttach.Rows[0]["ID"].ToString() == "0")
                        FileAttchGrid.Columns[iColCnt].Visible = (FileAttchGrid.Columns[iColCnt].HeaderText == "Download") ? false : true;
                    else
                    {
                        FileAttchGrid.Columns[iColCnt].Visible = (FileAttchGrid.Columns[iColCnt].HeaderText == "Download") ? true : true;
                    }
                }
            }

        }
        #endregion

        #region PO Creation
        private bool POCreation()
        {
            objPO = new clsSparePO();
            DataSet ds = new DataSet();

            int iPOID = 0;
            ds = objPO.GetPO(Func.Convert.iConvertToInt(txtID.Text.Trim()), (Func.Convert.iConvertToInt(drpClaimType.SelectedValue.Trim().ToString()) == 2) ? "Excess" : "WrngSupp", Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), "N", 0);

            if (ds.Tables[1].Rows.Count == 0)
            {
                // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('No Details For PO Creation.');</script>");
                return false;
            }
            else
            {
                DataTable dtPOHdr = new DataTable();
                DataTable dtPODtl = new DataTable();
                POUpdateHdrValueFromControl(dtPOHdr);
                dtPODtl = ds.Tables[1];

                if (objPO.bSaveRecordWithPart(sDealerCode, iDealerID, dtPOHdr, dtPODtl, ref iPOID, 0) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(14,'" + Server.HtmlEncode("Parts PO") + "','" + Server.HtmlEncode(dtPOHdr.Rows[0]["PO_No"].ToString()) + "');</script>");
                    return true;
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(15,'" + Server.HtmlEncode("Parts PO") + "','" + Server.HtmlEncode(dtPOHdr.Rows[0]["PO_No"].ToString()) + "');</script>");
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PO Not Created, Please Contact to Administrator.');</script>");
                    return false;
                }

            }

        }

        private void POUpdateHdrValueFromControl(DataTable dtPOHdr)
        {
            try
            {
                objPO = new clsSparePO();

                string cntRetainStatus = "";
                string cntPartID = "";
                int TotCntQty = 0;
                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                DataRow dr;
                //Get Header InFormation        
                dtPOHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_No", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Date", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Po_Type_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Confirm", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Cancel", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Total", typeof(double)));
                dtPOHdr.Columns.Add(new DataColumn("PO_TotalQty", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_TotalItems", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_CreatedBy", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("Chassis_No", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Is_Distributor", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("JobCard_HDR_ID", typeof(int)));

                dr = dtPOHdr.NewRow();
                dr["ID"] = "0";
                dr["PO_No"] = Func.Convert.sConvertToString(objPO.GeneratePO(sDealerCode, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), (Func.Convert.iConvertToInt(drpClaimType.SelectedValue.Trim().ToString()) == 2) ? 7 : 7, "N"));
                dr["PO_Date"] = Func.Common.sGetCurrentDate(1, false); ;

                dr["Dealer_ID"] = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());

                dr["Po_Type_ID"] = (Func.Convert.iConvertToInt(drpClaimType.SelectedValue.Trim().ToString()) == 2) ? 7 : 7;
                dr["PO_Confirm"] = "Y";
                dr["PO_Cancel"] = "N";
                //dr["PO_Total"] = Func.Convert.dConvertToDouble(0);
                //dr["PO_TotalQty"] = Func.Convert.iConvertToInt(0);

                for (int iRCnt = 0; iRCnt < PartGrid.Rows.Count; iRCnt++)
                {
                    if (iRCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        TextBox txtPartNo11 = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtPartNo");
                        cntPartID = txtPartNo11.Text;
                        DropDownList drpRetain11 = (DropDownList)PartGrid.Rows[iRCnt].FindControl("drpRetain");
                        cntRetainStatus = drpRetain11.SelectedValue.ToString();

                        //Get Descripancy_Qty
                        TextBox txtDescripancy_Qty = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtDescripancy_Qty");
                        dtDetails.Rows[iRCnt]["Descripancy_Qty"] = Func.Convert.dConvertToDouble(txtDescripancy_Qty.Text);

                        // Get Rate
                        TextBox txtRate = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtRate");
                        dtDetails.Rows[iRCnt]["Rate"] = Func.Convert.dConvertToDouble(txtRate.Text);

                        if (cntPartID != "" && cntRetainStatus == "Y")
                        {
                            TotCntQty = TotCntQty + 1;
                            dPartQty = 0;
                            dPartRate = 0;

                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRCnt]["Descripancy_Qty"]);
                            dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRCnt]["Rate"]);
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);

                            TextBox txtQuantity = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtQuantity");
                            dtDetails.Rows[iRCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                            dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRCnt]["Descripancy_Qty"]);
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRCnt]["Total"]);

                        }
                    }
                }
                dr["PO_Total"] = Func.Convert.dConvertToDouble(dTotal);
                dr["PO_TotalQty"] = Func.Convert.iConvertToInt(dTotalQty);
                dr["PO_TotalItems"] = TotCntQty;
                dr["PO_CreatedBy"] = "Part Claim PO Created By " + txtCreatedBy.Text;
                dr["Chassis_No"] = "";
                dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetManufSuppID, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), " and HOBr_ID=" + Session["HOBR_ID"].ToString())); //MTI 18 --
                dr["Is_Distributor"] = "N";
                dr["JobCard_HDR_ID"] = Func.Convert.iConvertToInt(txtID.Text.Trim().ToString());

                dtPOHdr.Rows.Add(dr);
                dtPOHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion
        #region Insurance Claim Documnets
        private bool bSaveInsuranceDocument()
        {
            try
            {
                dtInsuDoc = new DataTable();
                DataRow dr;
                dtInsuDoc.Columns.Add(new DataColumn("ID", typeof(int)));
                dtInsuDoc.Columns.Add(new DataColumn("ClaimID", typeof(int)));
                dtInsuDoc.Columns.Add(new DataColumn("InsuranceDocID", typeof(int)));
                dtInsuDoc.Columns.Add(new DataColumn("Doc_Name", typeof(string)));
                dtInsuDoc.Columns.Add(new DataColumn("IsSelected", typeof(bool)));
                dtInsuDoc.Columns.Add(new DataColumn("User_ID", typeof(int)));

                foreach (ListItem item in chkInsuranceDoc.Items)
                {
                    dr = dtInsuDoc.NewRow();
                    dr["ID"] = 0;
                    dr["ClaimID"] = Func.Convert.iConvertToInt(txtID.Text);
                    dr["InsuranceDocID"] = Func.Convert.iConvertToInt(item.Value);
                    dr["Doc_Name"] = item.Text;
                    dr["IsSelected"] = Func.Convert.bConvertToBoolean(item.Selected);
                    dr["User_ID"] = iUserId;
                    dtInsuDoc.Rows.Add(dr);
                    dtInsuDoc.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
                Func.Common.ProcessUnhandledException(ex);
            }
            return true;
        }
        #endregion

        protected void FileAttchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            //string[] paths = Directory.GetFiles(sPath + "Parts\\Part Claim");

            //HtmlAnchor achFileName = (HtmlAnchor)e.Row.Cells[0].FindControl("achFileName");
            //if (achFileName != null)
            //{
            //    for (int i = 0; i < paths.Length; i++)
            //    {
            //        if (sPath + "Parts\\Part Claim" + "\\" + achFileName.InnerHtml == paths[i])
            //        {

            //           achFileName.HRef = paths[i];
            //            break;
            //        }
            //    }
            //    //achFileName.HRef = sPath + "Parts\\Part Claim" + "\\" + achFileName.InnerHtml;
            //    //achFileName.HRef = @"C:\Upload Documents\Transaction\Parts\Part Claim\" + achFileName.InnerHtml;
            //    achFileName.Target = "_blank";
            //}

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            //string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string fileNames = "";
            string fileNamewithoutExt = "";
            string FileExtension = "";
            fileNamewithoutExt = Path.GetFileNameWithoutExtension(FileAttchGrid.DataKeys[gvrow.RowIndex].Value.ToString());
            FileExtension = Path.GetExtension(FileAttchGrid.DataKeys[gvrow.RowIndex].Value.ToString()).ToLower();
            fileNames = fileNamewithoutExt + FileExtension;

            // New Function Added on 16/01/2018 for download Files
            DownloadDocument(fileNames, txtUserType.Text.Trim(), sDealerCode);

            if (fileNames.Trim() != "")
            {
                //WebClient req = new WebClient();
                ////Clear the content of the responce
                //Response.ClearContent();
                //// LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + fileNames);
                //// Add the file size into the response header
                //Response.AddHeader("Content-Length", fileNames.Length.ToString());
                //// Set the Content Type 
                ////Response.ContentType = ReturnExtension(FileExtension.ToLower());
                //Response.ContentType = "application/octet-stream";
                //// Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                //Response.TransmitFile((sPath + "Parts\\Part Claim" + "\\" + fileNames));

                //// End the response 
                //Response.End();
            }
            //Response.ContentType = "image/jpg";
            //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileNames + "\"");
            ////Response.TransmitFile(Server.MapPath(filePath));
            //string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            //Response.TransmitFile((sPath + "Parts\\Part Claim" + "\\" + fileNames));
            //Response.End();
        }
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                //case ".htm":
                //case ".html":
                //case ".log": return "text/HTML";
                case ".txt": return "text/plain";
                case ".docx": return "application/ms-word";
                case ".doc": return "application/ms-word";
                //case ".tiff":
                //case ".tif": return "image/tiff";
                //case ".asf": return "video/x-ms-asf";
                //case ".avi": return "video/avi";
                case ".zip": return "application/zip";
                case ".xls":
                case ".csv": return "application/vnd.ms-excel";
                case ".gif": return "image/gif";
                case ".jpg": return "image/jpg";
                case ".jpeg": return "image/jpeg";
                case ".bmp": return "image/bmp";
                //case ".wav": return "audio/wav";
                //case ".mp3": return "audio/mpeg3";
                //case ".mpg":
                //case "mpeg": return "video/mpeg";
                case ".rtf": return "application/rtf";
                //case ".asp": return "text/asp";
                case ".pdf": return "application/pdf";
                //case ".fdf": return "application/vnd.fdf";
                case ".ppt": return "application/mspowerpoint";
                //case ".dwg": return "image/vnd.dwg";
                //case ".msg": return "application/msoutlook";
                case ".xml":
                case ".sdxl": return "application/xml";
                //case ".xdp": return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        private void DownloadDocument(string sFileName, string sUserType, string sDealerCode)
        {
            FileInfo fileToDownload = null;
            string sBasePath = "";
            string sOldBasePath = "";
            string sFullPath = "";

            //if (Func.Convert.sConvertToString(sUserType) == "2" || Func.Convert.sConvertToString(sUserType) == "3" || Func.Convert.sConvertToString(sUserType) == "8")
            //{
            sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            sOldBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadOldDomesticFiles"]);

            if (!Directory.Exists(@sBasePath))
            {// Check Directory present or Not
                Directory.CreateDirectory(@sBasePath);
            }
            else
            {// list all sub Directory in Directory (sub Folder)
                string[] dirs = Directory.GetDirectories(@sBasePath, "*", SearchOption.AllDirectories);
                foreach (string dir in dirs)
                {
                    if (System.IO.File.Exists(dir + @"\" + sFileName))// check file exists 
                    {
                        sFullPath = dir + @"\" + sFileName;
                        break;
                    }
                }// END Foreach
            } //END Else

            if (sFullPath == "")
            {// if C Drive File Doesnot Exists then check on E Drive
                if (!Directory.Exists(@sOldBasePath)) // Check Directoty Present or Not
                {
                    Directory.CreateDirectory(@sOldBasePath);
                }
                bool isEmpty = !Directory.EnumerateFiles(@sOldBasePath).Any();// check Directory contains any file or subFolder
                if (isEmpty == true)
                {
                    Response.Write(@sFullPath);
                    Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in particular Dealer folder path!');</script>");
                    return;
                }
                else
                {
                    string[] dirsOld = Directory.GetDirectories(@sOldBasePath, "*", SearchOption.AllDirectories);//List of all Sub Folder in E Drive
                    foreach (string dirOld in dirsOld)
                    {
                        if (System.IO.File.Exists(dirOld + @"\" + sFileName))
                        {
                            sFullPath = dirOld + @"\" + sFileName;
                            break;
                        }
                        else
                        {
                            Response.Write(@sFullPath);
                            Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in particular Dealer folder path!');</script>");
                            return;
                        }
                    }// END Foreach
                }// END Else
            }// END IF

            fileToDownload = new System.IO.FileInfo(@sFullPath);
            Response.ContentType = ReturnExtension(fileToDownload.Extension.ToLower());
            System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(sFileName) + "\"";
            Response.AppendHeader("Content-Disposition", disHeader);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            //}
        }

    }
}