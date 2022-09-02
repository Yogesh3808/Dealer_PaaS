using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace MANART.Forms.Spares
{
    public partial class frmPurRetunNote : System.Web.UI.Page
    {
        private int iPurRetNoteID = 0;
        private DataTable dtDetails = new DataTable();
        private DataTable dtPRNTaxDetails = new DataTable();
        private DataTable dtPRNGrpTaxDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsSparePurRetNote objPurRetNote = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        int iHOBr_id = 0;
        int iDealerID = 0;
        string sDealerCode = "";
        string sSupplierType = "";
        int iSupplierID = 0;
        private bool sDiscChange = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 75;
                ToolbarC.iValidationIdForConfirm = 75;
                ToolbarC.bUseImgOrButton = true;
                ExportLocation.bUseSpareDealerCode = true;
                ToolbarC.iFormIdToOpenForm = 106;
                ExportLocation.SetControlValue();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                if (!IsPostBack)
                {
                    Session["PurRetPart"] = null;
                    Session["PRNGrpTaxDetails"] = null;
                    Session["PRNTaxDetails"] = null;
                    FillInvoiceCombo();
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Purchase Return Note List";

                if (iPurRetNoteID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                //SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillInvoiceCombo()
        {
            try
            {
                Func.Common.BindDataToCombo(ddlInvoice, clsCommon.ComboQueryType.InvoiceNoForPRN, 0, ((hdnIsDocGST.Value == "Y" && ExportLocation.sSupplierType == "M" && hdnIsAutoPRN.Value == "Y") ? " And ISNULL(TD.PO_Det_ID,0) !=0 " : "And ISNULL(IsPRN,'N')='N' ") + "And ( Supplier_ID=" + ExportLocation.iSupplierId + ") And ISNULL(IsPRN,'N')='N'");
                //Func.Common.BindDataToCombo(ddlInvoice, clsCommon.ComboQueryType.InvoiceNoForPRN, 0, "And ( Supplier_ID=" + ExportLocation.iSupplierId + ") And ISNULL(IsPRN,'N')='N'");
                ddlInvoice.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ExportLocation.DealerSelectedIndexChanged += new EventHandler(ExportLocation_DealerSelectedIndexChanged);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["PurRetPart"] = null;
                Session["PRNGrpTaxDetails"] = null;
                Session["PRNTaxDetails"] = null;
                GeneratePurRetNoteNo();
                DisplayPreviousRecord();
            }

        }

        protected void ExportLocation_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iSupplierID = ExportLocation.iSupplierId;
                FillSelectionGrid();
                Session["PurRetPart"] = null;

                if (ExportLocation.sSupplierType == "M")//  For Manufacturer Supplier
                {
                    ddlPurReturnType.SelectedValue = "Y";
                    hdnIsAutoPRN.Value = ddlPurReturnType.SelectedValue;

                    lblDeliveryNo.Style.Add("display", "");
                    txtDeliveryNo.Style.Add("display", "");

                    rbtLstDiscount.Style.Add("display", "none");
                    trAppClaimRet.Style.Add("display", "");
                }
                else if (ExportLocation.sSupplierType == "D")//  For Dealer Type Supplier
                {
                    ddlPurReturnType.SelectedValue = "Y";
                    hdnIsAutoPRN.Value = ddlPurReturnType.SelectedValue;

                    lblDeliveryNo.Style.Add("display", "");
                    txtDeliveryNo.Style.Add("display", "");

                    rbtLstDiscount.Style.Add("display", "none");
                    trAppClaimRet.Style.Add("display", "none");
                }
                else  // For Local Supplier
                {
                    ddlPurReturnType.SelectedValue = "N";
                    hdnIsAutoPRN.Value = ddlPurReturnType.SelectedValue;

                    trGRNDetails.Style.Add("display", "none");
                    //lblDeliveryNo.Style.Add("display", "none");
                    //txtDeliveryNo.Style.Add("display", "none");

                    rbtLstDiscount.Style.Add("display", "");
                    trAppClaimRet.Style.Add("display", "none");
                }

                if (ddlInvoice.Items.Count > 1)
                {
                    ddlInvoice.Style.Add("display", "");
                    txtInvNo.Style.Add("display", "none");
                    lblMInvoiceNo.Style.Add("display", "");
                }
                else
                {
                    ddlInvoice.Style.Add("display", "none");
                    txtInvNo.Style.Add("display", "");
                    lblMInvoiceNo.Style.Add("display", "");
                }
                hdnSupplierType.Value = Func.Convert.sConvertToString(ExportLocation.sSupplierType);

                FillInvoiceCombo();
                DisplayPreviousRecord();
                GeneratePurRetNoteNo();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iPurRetNoteID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objPurRetNote = new clsSparePurRetNote();
                if (iPurRetNoteID != 0)
                {
                    ds = objPurRetNote.GetPurRetNote(iPurRetNoteID, "All", Func.Convert.iConvertToInt(ExportLocation.iDealerId), iUserId, Func.Convert.iConvertToInt(ExportLocation.iSupplierId), hdnIsAutoPRN.Value, "");
                    sNew = "N";
                    DisplayData(ds);
                    objPurRetNote = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objPurRetNote = null;
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

                objPurRetNote = new clsSparePurRetNote();
                ds = objPurRetNote.GetPurRetNote(iPurRetNoteID, "New", iDealerID, iUserId, ExportLocation.iSupplierId, hdnIsAutoPRN.Value, ddlInvoice.SelectedItem.Text.Trim());

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
                hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + iSupplierID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "PRN" + "/" + hdnIsAutoPRN.Value;
                txtID.Text = "";
                GeneratePurRetNoteNo();
                txtPurRetNoteDate.Text = Func.Common.sGetCurrentDate(1, false);

                objPurRetNote = null;
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
                hdnIsOpenPRN.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PRN_Open"]);
                txtPrevOpenPrnNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrevOpenPrnNo"]);
                hdnIsAutoPRN.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoPRN"]);

                txtPurRetNoteDate.Enabled = false;
                txtPurRetNoteNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PRN_No"]);
                txtPurRetNoteDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PRN_Date"]);
                hdnDealerID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerID"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["DealerID"]);
                }

                txtReference.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Reference"]);
                txtDeliveryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delivery_No"]);
                txtGrnDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_Date"]);
                txtGrnNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GRN_No"]);
                if (hdnIsAutoPRN.Value == "Y")
                {
                    //FillInvoiceCombo();
                    if (sNew == "N")
                        Func.Common.BindDataToCombo(ddlInvoice, clsCommon.ComboQueryType.InvoiceNoForPRN, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IsReceiptGST,'N')='Y' " : " And isnull(IsReceiptGST,'N')='N' ") + " And ( Supplier_ID='" + ExportLocation.iSupplierId + "') ");
                    //ddlInvoice.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_No"]);
                    ddlInvoice.SelectedItem.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_No"]);
                    ddlInvoice.Style.Add("display", "");
                    txtInvNo.Style.Add("display", "none");
                    txtInvDate.Enabled = false;
                    trGRNDetails.Style.Add("display", "");

                }
                else
                {
                    txtInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_No"]);
                    ddlInvoice.Style.Add("display", "none");
                    txtInvNo.Style.Add("display", "");
                    txtInvDate.Enabled = true;
                    trGRNDetails.Style.Add("display", "none");
                }
                txtInvDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Invoice_date"]);
                ddlInvoice.Enabled = (sNew == "N") ? false : true;
                ddlPurReturnType.Enabled = (sNew == "N") ? false : true;
                ddlAppClaimRet.Enabled = (sNew == "N") ? false : true;
                ds.Tables[0].Rows[0]["Is_AppClaimRet"] = hdnAppClaimRet.Value;
                ddlAppClaimRet.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AppClaimRet"]);
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);

                if (hdnIsAutoPRN.Value == "Y" && ExportLocation.sSupplierType == "M")
                    trAppClaimRet.Style.Add("display", "");
                else
                    trAppClaimRet.Style.Add("display", "none");


                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoPRN"]) == "Y")
                    ddlPurReturnType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoPRN"]);
                else
                    ddlPurReturnType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoPRN"]);

                hdnSupTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PerAmt"]) == "Per")
                    rbtLstDiscount.SelectedValue = "Per";
                else
                    rbtLstDiscount.SelectedValue = "Amt";
                rbtLstDiscount.Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");

                Session["PurRetPart"] = null;
                Session["PRNGrpTaxDetails"] = null;
                Session["PRNTaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["PurRetPart"] = dtDetails;
                dtPRNGrpTaxDetails = ds.Tables[2];
                Session["PRNGrpTaxDetails"] = dtPRNGrpTaxDetails;
                dtPRNTaxDetails = ds.Tables[3];
                Session["PRNTaxDetails"] = dtPRNTaxDetails;
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                BindDataToGrid();
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
                if (txtUserType.Text.Trim() == "6")
                {
                    ddlPurReturnType.Enabled = false;
                    ddlInvoice.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form        
            //drpPurRetNoteType.Enabled = bEnable;

            PartGrid.Enabled = bEnable;
            GrdPartGroup.Enabled = bEnable;
            GrdDocTaxDet.Enabled = bEnable;
            txtGrnNo.Enabled = bEnable;
            txtGrnDate.Enabled = bEnable;

            if (Func.Convert.sConvertToString(ExportLocation.sSupplierType) == "M" && hdnIsAutoPRN.Value == "Y" && hdnConfirm.Value != "Y")
            {
                GrdPartGroup.Enabled = false;
                GrdDocTaxDet.Enabled = false;
            }
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }

        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerId = 3;
                iDealerID = Func.Convert.iConvertToInt(ExportLocation.iDealerId);
                objPurRetNote = new clsSparePurRetNote();
                ds = objPurRetNote.GetPurRetNote(iPurRetNoteID, "Max", iDealerID, iUserId, Func.Convert.iConvertToInt(ExportLocation.iSupplierId), hdnIsAutoPRN.Value, "");
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
                    Session["PurRetPart"] = null;
                    Session["PRNGrpTaxDetails"] = null;
                    Session["PRNTaxDetails"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objPurRetNote = null;
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
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    ddlInvoice.Enabled = true;
                    ddlInvoice.SelectedValue = "0";
                    ddlInvoice.SelectedItem.Text = "0";
                    ClearConrol();
                    FillInvoiceCombo();
                    DisplayPreviousRecord();
                    if (Func.Convert.iConvertToInt(ExportLocation.iDealerId) != 0)
                    {
                        GeneratePurRetNoteNo();
                    }

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

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
                iPurRetNoteID = Func.Convert.iConvertToInt(txtID.Text);
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
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }
                else if (objCommon.sUserRole == "19")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
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
                SearchGrid.AddToSearchCombo("PRN No");
                SearchGrid.AddToSearchCombo("PRN Date");
                SearchGrid.AddToSearchCombo("Invoice No");
                SearchGrid.AddToSearchCombo("PRN Status");
                SearchGrid.iDealerID = ExportLocation.iSupplierId;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUserId);
                SearchGrid.sSqlFor = "PartsPurRetNote";
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
                dtHdr.Columns.Add(new DataColumn("PRN_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PRN_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Reference", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Invoice_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Invoice_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Invoice_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GRN_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GRN_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Delivery_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("IS_PerAmt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_PRN_Mode_Auto", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PrevOpenPrnNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_AppClaimRet", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["PRN_No"] = txtPurRetNoteNo.Text;
                dr["PRN_Date"] = txtPurRetNoteDate.Text;
                dr["Dealer_ID"] = iDealerID;
                dr["Is_PRN_Mode_Auto"] = Func.Convert.sConvertToString(ddlPurReturnType.SelectedValue);
                dr["UserId"] = iUserId;
                dr["Reference"] = txtReference.Text;
                if (hdnIsAutoPRN.Value == "N")
                    dr["Invoice_No"] = txtInvNo.Text;
                else
                    dr["Invoice_No"] = Func.Convert.sConvertToString(ddlInvoice.SelectedItem.Text);
                dr["Invoice_Date"] = txtInvDate.Text;
                dr["GRN_No"] = txtGrnNo.Text;
                dr["GRN_Date"] = txtGrnDate.Text;
                dr["Supplier_ID"] = ExportLocation.iSupplierId;
                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["Delivery_No"] = txtDeliveryNo.Text;
                dr["IS_PerAmt"] = Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue);
                dr["PrevOpenPrnNo"] = txtPrevOpenPrnNo.Text;
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["Is_AppClaimRet"] = Func.Convert.sConvertToString(ddlAppClaimRet.SelectedValue);
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

            string sMessage = " ";
            bool bValidateRecord = true;
            if (txtPurRetNoteDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (txtPurRetNoteNo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document No.";
                bValidateRecord = false;
            }
            if (ddlPurReturnType.SelectedValue == "Y" && Func.Convert.sConvertToString(ddlInvoice.SelectedItem.Text) == "--Select--")
            {
                sMessage = sMessage + "\\n Please Select Invoice No.";
                bValidateRecord = false;
            }
            if (ddlPurReturnType.SelectedValue == "N" && txtInvNo.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Invoice No.";
                bValidateRecord = false;
            }
            if (ddlPurReturnType.SelectedValue == "N" && txtInvDate.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Invoice Date.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsSparePurRetNote objPurRetNote = new clsSparePurRetNote();
            try
            {
                if (bValidateRecord() == false)
                {
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
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
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }

                if (objPurRetNote.bSaveRecord(sDealerCode, dtHdr, dtDetails, dtPRNGrpTaxDetails, dtPRNTaxDetails, ref iPurRetNoteID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iPurRetNoteID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts PRN ") + "','" + Server.HtmlEncode(txtPurRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts PRN ") + "','" + Server.HtmlEncode(txtPurRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts PRN ") + "','" + Server.HtmlEncode(txtPurRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts PRN ") + "','" + Server.HtmlEncode(txtPurRetNoteNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        private void GeneratePurRetNoteNo()
        {
            objPurRetNote = new clsSparePurRetNote();
            txtPurRetNoteNo.Text = Func.Convert.sConvertToString(objPurRetNote.GeneratePurRetNote(sDealerCode, iDealerID));
        }


        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            //DataTable dtExistRecord = null;
            //DataTable dtSysStock = null;
            //StringBuilder sPartIDs = null;
            try
            {
                //objPurRetNote = new clsSparePurRetNote();
                //dtSysStock = new DataTable();
                //dtExistRecord = new DataTable();
                //dtExistRecord = (DataTable)Session["PurRetPart"];
                //if (dtExistRecord != null)
                //{
                //    sPartIDs = new StringBuilder();
                //    for (int icnt = 0; icnt < dtExistRecord.Rows.Count; icnt++)
                //    {
                //        if (sPartIDs.Length == 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //            sPartIDs.Append(Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //        else if (sPartIDs.Length > 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //            sPartIDs.Append("," + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //    }
                //    dtSysStock = objPurRetNote.GetSysStock(Func.Convert.sConvertToString(sPartIDs), iUserId, (ddlMode.SelectedValue == "Y") ? "PRNAuto" : "PRNManual", ExportLocation.iDealerId, Func.Convert.iConvertToInt(txtID.Text));

                //    if (dtSysStock != null && dtSysStock.Rows.Count > 0)
                //    {
                //        Session["PurRetPart"] = dtSysStock;
                //    }
                //}
                FillDetailsFromGrid(false);
                BindDataToGrid();
                //new code
                FillDetailsFromGrid(false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["SalesRetPart"] = dtDetails;
                Session["SRNGrpTaxDetails"] = dtPRNGrpTaxDetails;
                Session["SRNTaxDetails"] = dtPRNTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objPurRetNote != null) objPurRetNote = null;
                //if (dtSysStock != null) dtSysStock = null;
                //if (dtExistRecord != null) dtExistRecord = null;
                //if (sPartIDs != null) sPartIDs = null;
            }

        }

        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                dtDetails = (DataTable)Session["PurRetPart"];

                string sQtyMsg = "";
                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                bDetailsRecordExist = true;
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                        TextBox txtInvoiceNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvoiceNo");
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                        TextBox MR_Dts_ID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMR_Dts_ID");

                        if (txtPartID.Text != "" && txtPartID.Text != "0")
                        {
                            for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                            {
                                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                                 && Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Invoice_No"]) == txtInvoiceNo.Text
                                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["MR_Dts_ID"]) == Func.Convert.iConvertToInt(MR_Dts_ID.Text))
                                {
                                    iCntForSelect = iCntForSelect + 1;

                                    if (Chk.Checked == true)
                                    {
                                        dtDetails.Rows[iMRowCnt]["Status"] = "D";
                                        iCntForDelete++;
                                    }
                                    else
                                    {
                                        dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
                                        dtDetails.Rows[iMRowCnt]["MR_Dts_ID"] = Func.Convert.iConvertToInt(MR_Dts_ID.Text);
                                        dtDetails.Rows[iMRowCnt]["Invoice_No"] = txtInvoiceNo.Text;

                                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                                        dtDetails.Rows[iMRowCnt]["Part_No"] = txtPartNo.Text;

                                        //Part Name
                                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                                        dtDetails.Rows[iMRowCnt]["Part_Name"] = txtPartName.Text;
                                        //Invoice No
                                        //TextBox txtInvoiceNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvoiceNo");
                                        dtDetails.Rows[iMRowCnt]["Invoice_No"] = txtInvoiceNo.Text;
                                        //Invoice Qty
                                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                        dtDetails.Rows[iMRowCnt]["Invoice_Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                        // Get Ret Qty
                                        TextBox txtReturnQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtReturnQty");
                                        dtDetails.Rows[iMRowCnt]["Ret_Qty"] = Func.Convert.dConvertToDouble(txtReturnQty.Text);
                                        // Get Sys Stock
                                        TextBox txtOrgStkQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOrgStkQty");
                                        dtDetails.Rows[iMRowCnt]["Stock_Qty"] = Func.Convert.dConvertToDouble(txtOrgStkQty.Text);
                                        // Get Unit
                                        TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                                        dtDetails.Rows[iMRowCnt]["Unit"] = Func.Convert.sConvertToString(txtUnit.Text);
                                        // Get Price
                                        TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                                        dtDetails.Rows[iMRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);
                                        // Get Rate
                                        TextBox txtRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRate");
                                        dtDetails.Rows[iMRowCnt]["Rate"] = Func.Convert.dConvertToDouble(txtRate.Text);
                                        // Get Disc_Per
                                        TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                                        dtDetails.Rows[iMRowCnt]["Disc_Per"] = Func.Convert.dConvertToDouble(txtDiscPer.Text);
                                        // Get Disc_Amt
                                        //TextBox txtDiscAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscAmt");
                                        //dtDetails.Rows[iMRowCnt]["Disc_Amt"] = Func.Convert.dConvertToDouble(txtDiscAmt.Text);
                                        // Get Accept_Rate
                                        TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                                        dtDetails.Rows[iMRowCnt]["Accept_Rate"] = Func.Convert.dConvertToDouble(txtAccRate.Text);
                                        // Get Total
                                        TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                        dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                                        //PO NO
                                        // Get Unit
                                        TextBox txtPONo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPONo");
                                        dtDetails.Rows[iMRowCnt]["PO_No"] = Func.Convert.sConvertToString(txtPONo.Text);

                                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                                        dtDetails.Rows[iRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                                        Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                                        if (DrpPartTax1.Items.Count == 2)
                                        {
                                            DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                                        }

                                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                                        Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                                        if (DrpPartTax2.Items.Count == 2)
                                        {
                                            DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                                        }

                                        dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

                                    }
                                }
                            }
                        }
                    }
                    double dCurrSTK = 0.00;
                    double dRetQty = 0.00;
                    double dInv_Qty = 0.00;
                    // New Validation Here Start
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        string sSrNo = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("lblNo") as Label).Text.Trim());
                        string sPartID = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                        string siRowCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        string sInvoiceNo = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtInvoiceNo") as TextBox).Text.Trim());
                        double dDisper = 0.00;
                        dDisper = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtDiscPer") as TextBox).Text);

                        if (sPartID != "" && sPartID != "0" && siRowCntStatus != "D")
                        {
                            double totPrevQty = 0.00;
                            double totRetQty = 0.00;
                            clsDB objDB = new clsDB();

                            (PartGrid.Rows[iRowCnt].FindControl("txtOrgStkQty") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), "N"));

                            dCurrSTK = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtOrgStkQty") as TextBox).Text);
                            dRetQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtReturnQty") as TextBox).Text);
                            dInv_Qty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);

                            for (int iCnt = 0; iCnt < PartGrid.Rows.Count; iCnt++)
                            {
                                string sCntPartID = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                                string sCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                                if (sPartID != "0" && sPartID != "" && sPartID == sCntPartID && sCntStatus != "D")
                                {
                                    double dPreInvQty = 0.00;
                                    double dinvQty = 0.00;
                                    dinvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("txtReturnQty") as TextBox).Text);
                                    dPreInvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("TxtPrevRetQty") as TextBox).Text);
                                    totPrevQty = totPrevQty + dPreInvQty;
                                    totRetQty = totRetQty + dinvQty;
                                }
                            }

                            if (dRetQty == 0 && siRowCntStatus != "D")// Check Return Quantity
                            { 
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please Enter Return Quantity at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                            if (dDisper >= 100)// Check Discount Per
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "100% and above discount not allowed at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                            if (Func.Convert.iConvertToInt(drpPartTax.SelectedValue) == 0 && siRowCntStatus != "D")// Check Part tax
                            { 
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please select Part Tax at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                            if (dRetQty != 0 && siRowCntStatus != "D" && dRetQty > dInv_Qty && sInvoiceNo != "")
                            { // Check Return Quantity
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please Enter Return Quantity should not greater Than Invoice Quantity at Row No " + sSrNo;
                                bDetailsRecordExist = false;
                                break;
                            }
                            // New Code Dated 05042018 VIkram Start
                            if ((totRetQty > (dCurrSTK + totPrevQty)) && siRowCntStatus != "D")
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please enter less  return Quantity from Part Stock at Row No " + sSrNo;
                                bDetailsRecordExist = false;
                                break;
                            }

                            //END
                            //OLD COde
                            
                            //if (totPrevQty == dCurrSTK && totRetQty > dCurrSTK && siRowCntStatus != "D" && txtID.Text == "")
                            //{ // Check Curr Stock
                            //    iCntError = iCntError + 1;
                            //    sQtyMsg = "Please enter less  Return Quantity from Part Stock at Row No " + sSrNo;
                            //    bDetailsRecordExist = false;
                            //    break;
                            //}

                            //if (totPrevQty != dCurrSTK && (totRetQty > (dCurrSTK + totPrevQty)) && siRowCntStatus != "D")// Check Curr Stock
                            //{
                            //    iCntError = iCntError + 1;
                            //    sQtyMsg = "Please enter less  Return Quantity from Part Stock at Row No " + sSrNo;
                            //    bDetailsRecordExist = false;
                            //    break;
                            //}


                        }//END IF
                    }//END For
                    //Validation Ends Here

                    //New Validation END

                    //for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    //{
                    //    if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["PartTaxID"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                    //    {
                    //        iCntError = iCntError + 1;
                    //        sQtyMsg = "Please select Part Tax at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //        break;
                    //    }
                    //    else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Ret_Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                    //    {
                    //        iCntError = iCntError + 1;
                    //        sQtyMsg = "Please Enter Return Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //        break;
                    //    }
                    //    else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Ret_Qty"]) > Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Invoice_Qty"]) &&
                    //        Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Invoice_No"]) != "" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                    //    {
                    //        iCntError = iCntError + 1;
                    //        sQtyMsg = "Please Enter Return Quantity should not greater Than Invoice Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //        break;
                    //    }
                    //    else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Ret_Qty"]) > Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Stock_Qty"]) &&
                    //         Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                    //    {
                    //        iCntError = iCntError + 1;
                    //        sQtyMsg = "Please Enter Return Quantity should not greater Then Stock Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //        break;
                    //    }
                    //}

                    if (iCntForDelete == iCntForSelect)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
                        bDetailsRecordExist = false;
                    }
                    else if (iCntError > 0)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
                        bDetailsRecordExist = false;
                    }
                }
                //For Tax Details
                dtPRNGrpTaxDetails = (DataTable)(Session["PRNGrpTaxDetails"]);
                dtPRNTaxDetails = (DataTable)(Session["PRNTaxDetails"]);

                if (dtDetails.Rows.Count > 1 && dtPRNGrpTaxDetails.Rows.Count == 0)
                    CreateNewRowToTaxGroupDetailsTable();

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Net Reverse Amount
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //dtPRNGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(txtTax2Per.Text);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }
                //CreateNewRowToTaxGroupDetailsTable();
                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtPRNTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtPRNTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtPRNTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtPRNTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtPRNTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtPRNTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtPRNTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtPRNTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtPRNTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtPRNTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillDetailsFromGrid_OLD(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strPurRetNotesNo = "";
                dtDetails = (DataTable)Session["PurRetPart"];
                string sQtyMsg = "";
                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                    {
                        for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                        {

                            //PartID                
                            TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                            TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                            if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                            {
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "U")
                                    {
                                        if (strPurRetNotesNo == "")
                                            strPurRetNotesNo = iRowCnt.ToString();
                                        else
                                            strPurRetNotesNo = strPurRetNotesNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "O")
                                    {
                                        if (strPurRetNotesNo == "")
                                            strPurRetNotesNo = iRowCnt.ToString();
                                        else
                                            strPurRetNotesNo = strPurRetNotesNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "M")
                                    {
                                        if (strPurRetNotesNo == "")
                                            strPurRetNotesNo = iRowCnt.ToString();
                                        else
                                            strPurRetNotesNo = strPurRetNotesNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);

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

                                // Get Sys Qty
                                TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtReturnQty");
                                dtDetails.Rows[iMRowCnt]["Ret_Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                //Sujata 26052014_Begin
                                DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                                dtDetails.Rows[iMRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                                DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                                Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                                if (DrpPartTax1.Items.Count == 2)
                                {
                                    DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
                                }

                                DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                                Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                                if (DrpPartTax2.Items.Count == 2)
                                {
                                    DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
                                }

                                // Get Disc Per
                                TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                                dtDetails.Rows[iMRowCnt]["Disc_Per"] = Func.Convert.dConvertToDouble(txtDiscPer.Text);

                                // Get Accept_Rate
                                TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                                dtDetails.Rows[iMRowCnt]["Accept_Rate"] = Func.Convert.dConvertToDouble(txtAccRate.Text);

                                // Get Total
                                TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                // Get Status                           
                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;


                                CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                                sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Status"]);
                                //dtDetails.Rows[iRowCnt]["Status"] = "";            
                                if (Chk.Checked == true)
                                {
                                    dtDetails.Rows[iMRowCnt]["Status"] = "D";
                                    iCntForDelete++;
                                }
                                else
                                {
                                    if (txtPartID.Text != "" && txtQuantity.Text != "0")
                                    {
                                        if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "M")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "M";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "U")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "U";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "E";
                                            bDetailsRecordExist = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    {
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Ret_Qty"]) > Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Stock_Qty"]))
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Return Qty " + Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Ret_Qty"]) + " should be less or equal to Stock Qty " + Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Stock_Qty"]) + " at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                            break;
                        }
                        else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["PartTaxID"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please select Part Tax at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                            break;
                        }
                        else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Ret_Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter Return Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                            break;
                        }

                    }

                    if (iCntForDelete == iCntForSelect)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
                        bDetailsRecordExist = false;
                    }
                    else if (iCntError > 0)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
                        bDetailsRecordExist = false;
                    }
                }

                //For Tax Details
                dtPRNGrpTaxDetails = (DataTable)(Session["PRNGrpTaxDetails"]);
                dtPRNTaxDetails = (DataTable)(Session["PRNTaxDetails"]);

                if (dtDetails.Rows.Count > 1 && dtPRNGrpTaxDetails.Rows.Count == 0)
                    CreateNewRowToTaxGroupDetailsTable();
                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(txtTax2Per.Text);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtPRNGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }
                CreateNewRowToTaxGroupDetailsTable();
                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtPRNTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtPRNTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtPRNTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtPRNTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtPRNTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtPRNTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtPRNTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtPRNTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtPRNTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtPRNTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
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
                    if (dtDetails.Columns.Count == 0)
                    {
                        dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("MR_Dts_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Invoice_No", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Invoice_Qty", typeof(double)));

                        dtDetails.Columns.Add(new DataColumn("Ret_Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Stock_Qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("disc_per", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Accept_Rate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("PO_No", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Tax_Per", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
                    }
                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "0")
                        goto Exit;
                }

                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["MR_Dts_ID"] = 0;
                dr["Invoice_No"] = "";
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Invoice_Qty"] = 0;
                dr["Ret_Qty"] = 0;
                dr["group_code"] = "";
                dr["Unit"] = "";
                dr["Price"] = 0.00;
                dr["Stock_Qty"] = 0;
                dr["Rate"] = 1;
                dr["disc_per"] = 0.00;
                dr["Accept_Rate"] = 0.00;
                dr["PO_No"] = "";
                dr["Total"] = 0.00;
                dr["Tax_Per"] = 0.00;
                dr["PartTaxID"] = 0;
                dr["Status"] = "N";
                if (hdnSupTaxTag.Value == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";

                //dtDetails.Rows.Add(dr);
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
                if (Session["PurRetPart"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["PurRetPart"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["PurRetPart"];
                    if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                        CreateNewRowToDetailsTable();
                    dtDetails = (DataTable)Session["PurRetPart"];
                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();

                GrdPartGroup.DataSource = dtPRNGrpTaxDetails;
                GrdPartGroup.DataBind();

                GrdDocTaxDet.DataSource = dtPRNTaxDetails;
                GrdDocTaxDet.DataBind();

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
                double dPartMrpPrice = 0;
                double dRevPartRate = 0;
                double dDiscPer = 0;
                double dDiscRate = 0;

                double dExclPartDiscRate = 0;
                double dExclPartTotal = 0;
                double dExclTotal = 0;
                string sPartId = "0";
                double dPartTax = 0;
                double dPartTax1 = 0;
                double dPartTax2 = 0;
                double dRevMainTaxRate = 0;
                double dDiscRevRate = 0;
                hdnSelectedPartID.Value = "";

                double dPartBillQty = 0;
                double dInvoiceQty = 0;
                string sGroupCode = "";

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Func.Convert.sConvertToString(iDealerID);
                string sPartID = "", sPartName = "", cPartID = "";
                string sPartDetID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        //After GST Hide Part Tax 
                        PartGrid.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        TextBox txtInvoiceNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvoiceNo");
                        txtInvoiceNo.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Invoice_No"]);

                        LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                        lnkSelectPart.Attributes.Add("onclick", "return ShowPartSearchForPRN(this,'" + iDealerID + "','" + ExportLocation.iSupplierId + "','" + ddlPurReturnType.SelectedValue + "','" + ddlInvoice.SelectedItem.Text.Trim() + "'," + iHOBr_id + ",'" + hdnIsOpenPRN.Value + "');");

                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        sPartId = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        if (sPartId != "0")
                        {
                            //   hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + sPartId;
                            hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + txtInvoiceNo.Text + '-' + sPartId;
                        }

                        sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        sPartDetID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["ID"]);
                        TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                        TextBox txtReturnQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtReturnQty");
                        TextBox TxtPrevRetQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("TxtPrevRetQty");
                        TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                        TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                        TextBox txtRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRate");
                        TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                        TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                        TextBox txtGTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                        TextBox txtOrgStkQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOrgStkQty");

                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();

                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        if (ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "M")
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");
                        else
                        {
                            if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                                Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                            else
                                Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 ");
                        }
                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                        TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                        TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");

                        TextBox txtExclDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExclDiscountRate");
                        TextBox TxtExclTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("TxtExclTotal");


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


                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                        Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");

                        if ((ExportLocation.sSupplierType == "M" && ddlPurReturnType.SelectedValue == "Y") || sGroupCode == "02")
                            drpPartTax.Enabled = false;
                        else
                            drpPartTax.Enabled = true;

                        if (sPartID == "0" && (ddlPurReturnType.SelectedValue == "N" || ddlPurReturnType.SelectedValue == "0" || ddlPurReturnType.SelectedValue == "Y"))
                        {
                            txtPartNo.Style.Add("display", "none");
                            lnkSelectPart.Style.Add("display", "");
                            Chk.Style.Add("display", "none");
                            lblDelete.Style.Add("display", "none");
                            txtGPartName.Style.Add("display", "none");
                            txtInvoiceNo.Style.Add("display", "none");
                            txtQuantity.Style.Add("display", "none");
                            txtReturnQty.Style.Add("display", "none");
                            txtUnit.Style.Add("display", "none");
                            txtPrice.Style.Add("display", "none");
                            txtRate.Style.Add("display", "none");
                            txtDiscPer.Style.Add("display", "none");
                            txtAccRate.Style.Add("display", "none");
                            txtGTotal.Style.Add("display", "none");
                            txtOrgStkQty.Style.Add("display", "none");
                            drpPartTax.Style.Add("display", "none");
                        }
                        else
                            lnkSelectPart.Style.Add("display", "none");


                        if (txtGPartName.Text == "" && (ddlPurReturnType.SelectedValue == "N" || ddlPurReturnType.SelectedValue == "0" || ddlPurReturnType.SelectedValue == "Y"))
                        {
                            if (ddlPurReturnType.SelectedValue == "Y")
                            {
                                lnkSelectPart.Style.Add("display", "none");
                            }
                            else
                            {
                                lnkSelectPart.Style.Add("display", "");
                                txtPartNo.Style.Add("display", "none");
                            }
                            lnkSelectPart.Style.Add("display", "");
                        }
                        else
                        {
                            lnkSelectPart.Style.Add("display", "none");
                            txtPartNo.Style.Add("display", "");
                        }
                        if (txtUserType.Text.Trim() == "6")
                        {
                            lnkSelectPart.Style.Add("display", "none");
                        }


                        cPartID = txtPartNo1.Text;
                        if (cPartID != "")
                        {
                            dPartQty = 0;
                            dPartRate = 0;
                            dPartMrpPrice = 0;
                            dRevPartRate = 0;
                            dDiscPer = 0;
                            dDiscRate = 0;
                            dRevMainTaxRate = 0;
                            dDiscRevRate = 0;
                            dInvoiceQty = 0;
                            if (txtID.Text == "" || txtID.Text == "0" || sPartDetID == "" || sPartDetID == "0")
                                TxtPrevRetQty.Text = "0.00";
                            else
                                TxtPrevRetQty.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Ret_Qty"]);

                            //TxtPrevRetQty.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Ret_Qty"]));
                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Ret_Qty"]);
                            //dPartBillQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Bill_Qty"]);
                            //Reverse Rate Calulation for Local Part and Lubricant
                            if ((ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "M") || sGroupCode == "99")
                            {
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Accept_Rate"]);
                            }
                            else
                            {
                                clsDB objDB = new clsDB();
                                dPartMrpPrice = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);
                                
                                //06072018 Sujata For remove reverse calculation
                                if (ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "D")
                                {
                                    dRevPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Accept_Rate"]);                                
                                }
                                else
                                {
                                    dRevPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);
                                }
                                //dRevPartRate = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", dPartMrpPrice, Func.Convert.iConvertToInt(drpPartTax.SelectedValue));
                                //06072018 Sujata For remove reverse calculation

                                // Set Reverse Calculated Rate to Rate Column
                                txtRate.Text = Func.Convert.sConvertToString(dRevPartRate);
                                dPartRate = dRevPartRate;
                                dDiscPer = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["disc_per"]);
                                dDiscRate = Func.Convert.dConvertToDouble(dPartRate * (dDiscPer / 100));
                                dPartRate = Func.Convert.dConvertToDouble(dPartRate - dDiscRate);
                                txtAccRate.Text = Func.Convert.sConvertToString(Math.Round(dPartRate, 4));
                            }
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);
                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);
                            // Supplier Type M aasel Tar Only Qty Edit comment on 06102017
                            //if ((ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "M"))
                            //{
                            //    dInvoiceQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Invoice_Qty"]);
                            //    dPartTotal = Func.Convert.dConvertToDouble(dInvoiceQty * dPartRate);
                            //    dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);
                            //}

                            //Status
                            sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                            if (sRecordStatus != "C")
                            {
                                dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Ret_Qty"]);

                                dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);

                                //New Code for Reverse rate Calculation on 02052017_Vikram Begin
                                //if ((ddlPurReturnType.SelectedValue != "Y" && ExportLocation.sSupplierType != "M") || (ExportLocation.sSupplierType != "M" && sGroupCode != "99"))
                                //{
                                //    dRevMainTaxRate = dPartMrpPrice / (1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));
                                //    dDiscRevRate = Func.Convert.dConvertToDouble(dRevMainTaxRate * dDiscPer / 100);
                                //    txtExclDiscountRate.Text = Func.Convert.sConvertToString(Math.Round(dDiscRevRate, 2));
                                //    dRevMainTaxRate = dRevMainTaxRate - dDiscRevRate;
                                //    dExclPartTotal = Func.Convert.dConvertToDouble(dPartQty * dRevMainTaxRate);
                                //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                                //    dExclTotal = dExclTotal + dExclPartTotal;
                                //}
                                //else
                                //{
                                //    dExclPartTotal = dPartTotal;
                                //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                                //    dExclTotal = dExclTotal + dExclPartTotal;
                                //}
                                //End

                                //dPartTax = 1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100) + Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax1Per.Text) / 100) + Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax2Per.Text) / 100));
                                //dExclPartDiscRate = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) / Func.Convert.dConvertToDouble(dPartTax));

                                ////txtExclDiscountRate.Text = Func.Convert.sConvertToString(Math.Round(dExclPartDiscRate, 2));
                                ////dExclPartTotal = Func.Convert.dConvertToDouble(dPartQty * dExclPartDiscRate);
                                dExclPartTotal = dPartTotal;
                                TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dExclPartTotal));
                                dExclTotal = dExclTotal + dExclPartTotal;
                            }

                            if (ddlPurReturnType.SelectedValue == "N" && sGroupCode == "99")
                                txtRate.Enabled = true;
                            else
                                txtRate.Enabled = false;
                            //if (ddlPurReturnType.SelectedValue == "N" && sGroupCode == "02")
                            //    drpPartTax.Enabled = false;
                            //else
                            //    drpPartTax.Enabled = true;
                            if ((ddlPurReturnType.SelectedValue != "Y" && ExportLocation.sSupplierType != "M") || (ddlPurReturnType.SelectedValue == "N" && ExportLocation.sSupplierType == "M"))
                            {
                                //txtMRPRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this);");
                                txtDiscPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                                txtDiscPer.Attributes.Add("onblur", "return CalculatePRNPartTotal(event,this);");
                                txtRate.Attributes.Add("onblur", "return CalculatePRNPartTotal(event,this);");
                            }
                            else
                            {
                                txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                                txtDiscPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            }

                            if (ddlPurReturnType.SelectedValue == "Y")
                            {
                                TextBox txtPartTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                txtPartTotal.Text = Func.Convert.sConvertToString(dPartTotal);
                            }

                        }
                        //For MD User
                        if (txtUserType.Text == "6")
                        {
                            lnkSelectPart.Style.Add("display", "none");

                        }
                    }
                }

                //if (hdnIsAutoPRN.Value == "Y")
                //    txtTotal.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtPRNTaxDetails.Rows[0]["Net_Tr_Amt"]), 2));
                //else
                txtTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");

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
                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    // END
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    if (sDiscChange == true)
                    {
                        txtGrDiscountPer.Text = Func.Convert.sConvertToString(0.00);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString(0.00);
                    }
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (ddlPurReturnType.SelectedValue == "N" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Amt")
                        {
                            txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountAmt.Attributes.Add("onblur", "return CalulatePRNPartGranTotal();");
                        }
                        else
                        {
                            txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountPer.Attributes.Add("onblur", "return CalulatePRNPartGranTotal();");
                        }
                    }

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    //if ((ddlPurReturnType.SelectedValue != "Y" && ExportLocation.sSupplierType != "M") || (ExportLocation.sSupplierType != "M" && srowGRPID != "99"))
                    if (ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "M")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + "And ( DealerID='" + iDealerID + "') and Tax_Type= 1 and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 ");
                    }

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    //if ((ddlPurReturnType.SelectedValue != "Y" && ExportLocation.sSupplierType != "M") || (ExportLocation.sSupplierType != "M" && srowGRPID != "99"))
                    if (ddlPurReturnType.SelectedValue == "Y" && ExportLocation.sSupplierType == "M")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");
                    }
                    //DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + hdnDealerID.Value + "')");

                    //DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);

                    drpTax.Attributes.Add("onBlur", "SetMainTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");

                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();
                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
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
                double dGrpTotal = 0.00;
                double dGrpDiscPer = 0.00;
                double dGrpDiscAmt = 0.00;
                double dGrpTaxAppAmt = 0.00;

                double dGrpMTaxPer = 0.00;
                double dGrpMTaxAmt = 0.00;

                double dGrpTax1Per = 0.00;
                double dGrpTax1Amt = 0.00;

                double dGrpTax2Per = 0.00;
                double dGrpTax2Amt = 0.00;

                double dDocTotalAmtFrPFOther = 0.00;
                double dDocDiscAmt = 0.00;
                double dDocLSTAmt = 0.00;
                double dDocCSTAmt = 0.00;
                double dDocTax1Amt = 0.00;
                double dDocTax2Amt = 0.00;
                string sGrpMTaxTag = "";

                double TotalOA = 0.00;
                double TotalRev = 0.00;

                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //txtGrnetrevamt.Text = "0";
                }

                for (int i = 0; i < PartGrid.Rows.Count; i++)
                {
                    TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                    TextBox TxtExclTotal = (TextBox)PartGrid.Rows[i].FindControl("TxtExclTotal");
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGroupCode");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");
                    TextBox txtStatus = (TextBox)PartGrid.Rows[i].FindControl("txtStatus");
                    CheckBox Chk = (CheckBox)PartGrid.Rows[i].FindControl("ChkForDelete");

                    if (txtGrNo.Text.Trim() != "" && Chk.Checked == false)
                    {
                        TotalOA = TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text);
                        //TotalRev = TotalRev + Func.Convert.dConvertToDouble(txtTotal.Text);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && Chk.Checked == false && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == drpPartTax.SelectedValue && drpTax.SelectedIndex != 0)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                            //txtGrnetrevamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetrevamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
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

                    if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                    {
                        //group Percentage
                        dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                        //group Discount Amount
                        dGrpDiscAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100));
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));
                    }
                    else
                    {
                        dGrpDiscPer = 0.00;
                        dGrpDiscAmt = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);
                    }
                    //Doc Discount Amount
                    dDocDiscAmt = Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt);

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
                    dGrpTaxAppAmt = Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100));
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);
                    //double sum = 0;
                    if (sTax1ApplOn == "1")
                    {
                        //double DiscNetAmt = 0;
                        //DiscNetAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt)), 2);
                        //DiscNetAmt = DiscNetAmt + dGrpMTaxAmt;
                        //sum = DiscNetAmt / (1 + (Func.Convert.dConvertToDouble(dGrpTax1Per / 100)));
                        //dGrpTax1Amt = Math.Round(sum * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                        //OLD Logic
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else if (sTax1ApplOn == "3")
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    else
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));

                    dDocTax1Amt = Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Func.Convert.dConvertToDouble(txtTax2Per.Text);
                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    else
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    dDocTax2Amt = Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    //Vikram Begin_03052017
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    //END
                    //dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));

                    dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END

                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocRevTotal");
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
                    //txtDocRevTotal.Text = Func.Convert.sConvertToString(TotalRev.ToString("0.00"));
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
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                    dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                    txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));

                    dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt));

                    dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                    dDocOtherAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100));
                    txtOtherAmt.Text = Func.Convert.sConvertToString(dDocOtherAmt.ToString("0.00"));

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void PartGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Label RFPtatus = (Label)PartGrid.Rows[0].FindControl("lblRFPStatus");
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    FillDetailsFromGrid(false);
                    CreateNewRowToDetailsTable();
                    Session["PurRetPart"] = dtDetails;
                    BindDataToGrid();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void PartGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                dtDetails = (DataTable)Session["PurRetPart"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
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
            Session["PurRetPart"] = dtDetails;
            Session["PRNGrpTaxDetails"] = dtPRNGrpTaxDetails;
            Session["PRNTaxDetails"] = dtPRNTaxDetails;
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
                dtPRNGrpTaxDetails.Clear();
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGroupCode");
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
                    for (int iRCnt = 0; iRCnt < dtPRNGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtPRNGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtPRNGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0)
                    {
                        dr = dtPRNGrpTaxDetails.NewRow();

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
                        //dr["net_rev_amt"] = 0.00;

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


                        dtPRNGrpTaxDetails.Rows.Add(dr);
                        dtPRNTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearConrol();
            DisplayPreviousRecord();
            GeneratePurRetNoteNo();
        }
        protected void rbtLstDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnConfirm.Value == "N" && hdnIsAutoPRN.Value == "N")
                {
                    sDiscChange = true;
                    FillDetailsFromGrid(false);
                    BindDataToGrid();
                    sDiscChange = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void ddlPurReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnIsAutoPRN.Value = ddlPurReturnType.SelectedValue;
            if (ddlPurReturnType.SelectedValue == "Y")
            {
                ddlInvoice.Style.Add("display", "");
                txtInvNo.Style.Add("display", "none");
                trGRNDetails.Style.Add("display", "");
                FillInvoiceCombo();

            }
            else
            {
                ddlInvoice.Style.Add("display", "none");
                ddlInvoice.SelectedItem.Text = "--Select--";
                txtInvNo.Style.Add("display", "");
                trGRNDetails.Style.Add("display", "none");
            }
            if (ExportLocation.sSupplierType == "M" && ddlPurReturnType.SelectedValue == "Y")
                trAppClaimRet.Style.Add("display", "");
            ddlAppClaimRet.SelectedValue = "N";
            ClearConrol();

            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

            DisplayPreviousRecord();

        }
        private void ClearConrol()
        {
            Session["PurRetPart"] = null;
            Session["PRNGrpTaxDetails"] = null;
            Session["PRNTaxDetails"] = null;
            dtDetails = null;
            PartGrid.DataSource = dtDetails;
            PartGrid.DataBind();
            dtPRNGrpTaxDetails = null;
            Session["PRNGrpTaxDetails"] = dtPRNGrpTaxDetails;
            dtPRNTaxDetails = null;
            Session["PRNTaxDetails"] = dtPRNTaxDetails;

            txtInvNo.Text = "";
            //ddlInvoice.SelectedItem.Text = "0";
            txtInvDate.Text = "";
            txtGrnNo.Text = "";
            txtGrnNo.Text = "";
            txtTotal.Text = "0.00";
            txtTotalQty.Text = "0";
            txtID.Text = "";
            txtDeliveryNo.Text = "";
            txtReference.Text = "";
        }
        private int i = 1;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtPartID != "0")
                {
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }
        protected void txtInvNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CheckInvoiceno();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void CheckInvoiceno()
        {
            try
            {
                var conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                };

                string query = "select * from TM_PurchaseRetNote  where TM_PurchaseRetNote.Invoice_No='" + txtInvNo.Text + "'and Supplier_ID='" + ExportLocation.iSupplierId + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                if (dt.Rows.Count != 0)
                {
                    txtInvNo.Text = "";
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Invoice Number Already Exist !');</script>");
                }


            }
            catch (Exception ex) { }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetInvoice(string InvoiceNo)
        {
            try
            {
                //Convert.ToInt32(HttpContext.Current.)***YOGESH***;
                frmPurRetunNote obj = new frmPurRetunNote();
                var conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                };

                string query = "select * from TM_PurchaseRetNote  where TM_PurchaseRetNote.Invoice_No='" + InvoiceNo + "' and Supplier_ID=" + Func.Convert.iConvertToInt(HttpContext.Current.Session["iSupplierID"]) + "";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                return dt.Rows.Count.ToString();
            }
            catch (Exception ex) { return null; }
        }

        protected void ddlAppClaimRet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAppClaimRet.SelectedValue == "Y")
                hdnAppClaimRet.Value = "Y";
            else
                hdnAppClaimRet.Value = "N";

            DisplayPreviousRecord();
        }
    }
}