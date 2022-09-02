using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;


namespace MANART.Forms.Spares
{
    public partial class frmMaterialReceipt : System.Web.UI.Page
    {
        int iSupplierId = 0;
        string sFileName = "";
        private int iMatReceiptID = 0;
        private DataTable dtHeader = new DataTable();
        private DataTable dtDetails = new DataTable();
        private DataTable dtMRTaxDetails = new DataTable();
        private DataTable dtMRGrpTaxDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsMaterialReceipt objMatReceipt = null;
        string sNew = "Y";
        int iSupType = 0;
        int iUser_ID = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        string sDealerCode = "";
        string sSupplierType = "";
        int iTotalCnt = 0;
        DataSet dsSrchgrid;
        private string sSelectedPartID;
        private string sSuperceded;
        private bool sDiscChange = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 72;
                ToolbarC.iValidationIdForConfirm = 72;
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iFormIdToOpenForm = 104; //print option

                ExportLocation.bUseSpareDealerCode = true;
                ExportLocation.SetControlValue();

                iUser_ID = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                // For MD User 
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (!IsPostBack)
                {
                    Session["MRHeader"] = null;
                    Session["MRPartDetails"] = null;
                    Session["MRGrpTaxDetails"] = null;
                    Session["MRTaxDetails"] = null;
                    ddlInvoiceType.Items.Insert(0, new ListItem("--Select--", "0"));
                    FillCombo();
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Material Receipt List";
                FillSelectionGrid();
                if (iMatReceiptID != 0)
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


        private void FillCombo()
        {
            DataTable dtInvoice = null;
            objMatReceipt = new clsMaterialReceipt();
            try
            {
                dtInvoice = new DataTable();
                dtInvoice = objMatReceipt.GetInvoice(ExportLocation.iDealerId, hdnSupplierType.Value);
                ddlInvoice.DataValueField = "ID";
                ddlInvoice.DataTextField = "Inv_No";
                ddlInvoice.DataSource = dtInvoice;
                ddlInvoice.DataBind();
                ddlInvoice.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (dtInvoice != null) dtInvoice = null;
                if (objMatReceipt != null) objMatReceipt = null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ExportLocation.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
            iSupplierId = ExportLocation.iSupplierId;
            if (!IsPostBack)
            {
                Session["MRPartDetails"] = null;
                Session["MRGrpTaxDetails"] = null;
                Session["MRTaxDetails"] = null;
                Session["iSupType"] = null;
                Session["iSupplierID"] = null;
                //DisplayPreviousRecord();
                //hdnIsWithPO.Value = ddlReceiptType.SelectedValue;
                //hdnIsWithPO.Value = "N";
                //hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
            }
            FillSelectionGrid();
        }

        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iSupplierId = ExportLocation.iSupplierId;
                Session["iSupplierID"] = iSupplierId;
                sSupplierType = ExportLocation.sSupplierType;

                iSupType = ExportLocation.iSupType;
                Session["iSupType"] = iSupType;
                Session["MRPartDetails"] = null;
                Session["MRGrpTaxDetails"] = null;
                Session["MRTaxDetails"] = null;
                ClearConrol();

                if (ExportLocation.sSupplierType == "M")
                {
                    hdnSupplierType.Value = ExportLocation.sSupplierType;
                    ddlInvoiceType.Items.Add(new ListItem("Auto", "Y", true));
                    ddlInvoiceType.SelectedValue = "Y";
                    hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
                    ddlInvoiceType.Enabled = false;
                    ddlInvoice.Style.Add("display", "");
                    txtDMSInvNo.Style.Add("display", "none");
                    lblMInvoiceNo.Style.Add("display", "");
                    lblDeliveryNo.Style.Add("display", "");
                    txtDeliveryNo.Style.Add("display", "");
                    lblMDeliveryNo.Style.Add("display", "");
                    lblMDeliveryNo.Style.Add("display", "");

                    rbtLstDiscount.Style.Add("display", "none");

                    FillCombo();
                    txtDMSInvDate.Enabled = false;
                }
                else if (ExportLocation.sSupplierType == "B")
                {
                    hdnSupplierType.Value = ExportLocation.sSupplierType;
                    ddlInvoiceType.Items.Add(new ListItem("Auto", "Y", true));
                    ddlInvoiceType.SelectedValue = "Y";
                    hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
                    ddlInvoiceType.Enabled = false;
                    ddlInvoice.Style.Add("display", "");
                    txtDMSInvNo.Style.Add("display", "none");
                    lblMInvoiceNo.Style.Add("display", "");
                    lblDeliveryNo.Style.Add("display", "none");
                    txtDeliveryNo.Style.Add("display", "none");
                    lblMDeliveryNo.Style.Add("display", "none");

                    rbtLstDiscount.Style.Add("display", "none");

                    FillCombo();
                    txtDMSInvDate.Enabled = false;
                }
                else if (ExportLocation.sSupplierType != "M" && (ExportLocation.sSupplierType != "B"))// For Local Supplier
                {
                    hdnSupplierType.Value = ExportLocation.sSupplierType;
                    ddlInvoiceType.Items.Add(new ListItem("Manual", "N", true));
                    ddlInvoiceType.SelectedValue = "N";
                    hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
                    ddlInvoiceType.Enabled = false;

                    ddlInvoice.Style.Add("display", "none");
                    txtDMSInvNo.Style.Add("display", "");
                    lblMInvoiceNo.Style.Add("display", "");

                    lblDeliveryNo.Style.Add("display", "none");
                    txtDeliveryNo.Style.Add("display", "none");
                    lblMDeliveryNo.Style.Add("display", "none");

                    rbtLstDiscount.Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");

                }

                DisplayPreviousRecord();
                FillSelectionGrid();
                hdnSupplierType.Value = ExportLocation.sSupplierType;
                hdnIsDistributor.Value = ExportLocation.bDistributor;
                //hdnIsWithPO.Value = ddlReceiptType.SelectedValue;
                //hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;

                //// For Tax Tag
                DataSet dschg = new DataSet();

                //string iTMInvType = Func.Convert.sConvertToString(ddlInvoice.SelectedValue);
                objMatReceipt = new clsMaterialReceipt();
                dschg = objMatReceipt.GetMatReceipt(iMatReceiptID, "New", iUser_ID, iDealerID, "", Func.Convert.iConvertToInt(ExportLocation.iSupplierId), "", "");

                if (dschg != null) // if no Data Exist
                {
                    if (dschg.Tables.Count > 0)
                    {
                        Session["MRPartDetails"] = null;
                        Session["MRGrpTaxDetails"] = null;
                        Session["MRTaxDetails"] = null;

                        dtDetails = dschg.Tables[1];
                        Session["MRPartDetails"] = dtDetails;

                        dtMRGrpTaxDetails = dschg.Tables[2];
                        Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;

                        dtMRTaxDetails = dschg.Tables[3];
                        Session["MRTaxDetails"] = dtMRTaxDetails;

                        BindDataToGrid();
                    }

                }

                //if (dschg != null) // if no Data Exist
                //{

                //    if (dschg.Tables.Count > 0)
                //    {
                //        if (dschg.Tables[0].Rows.Count == 1)
                //        {
                //            dschg.Tables[0].Rows[0]["Is_Cancel"] = "N";
                //            dschg.Tables[0].Rows[0]["Is_Confirm"] = "N";
                //            //ds.Tables[1].Rows[0]["Status"] = "N";
                //            sNew = "Y";

                //            DisplayData(dschg);
                //        }
                //    }
                //}

                //txtID.Text = "";
                //txtMReceiptNo.Text = objMatReceipt.GenerateReceipt(ExportLocation.sDealerCode, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()));
                //if (hdnIsAutoReceipt.Value == "N")
                //{
                //    txtDMSInvDate.Text = Func.Common.sGetCurrentDate(1, false);
                //    txtLRDate.Text = Func.Common.sGetCurrentDate(1, false);
                //}

                //txtMReceiptDate.Text = Func.Common.sGetCurrentDate(1, false);
                //objMatReceipt = null;
                //dschg = null;
                //ddlInvoice.SelectedValue = Func.Convert.sConvertToString(iTMInvType);
                //if (Location.iDealerID != 0)
                //{
                //    GenerateInvNo(Func.Convert.iConvertToInt(DrpInvType.SelectedValue));
                //}

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iMatReceiptID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objMatReceipt = new clsMaterialReceipt();

                if (iMatReceiptID != 0)
                {
                    ds = objMatReceipt.GetMatReceipt(iMatReceiptID, "All", iUser_ID, iDealerID, ddlInvoice.SelectedValue.ToString(), Func.Convert.iConvertToInt(ExportLocation.iSupplierId), ddlInvoiceType.SelectedValue, "");
                    sNew = "N";
                    DisplayData(ds);
                    objMatReceipt = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objMatReceipt = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void DisplayInvoiceRecord()
        {
            try
            {
                DataSet ds = new DataSet();
                objMatReceipt = new clsMaterialReceipt();
                ds = objMatReceipt.GetMatReceipt(iMatReceiptID, "New", iUser_ID, iDealerID, ddlInvoice.SelectedItem.Text.Trim(), ExportLocation.iSupplierId, ddlInvoiceType.SelectedValue, txtDeliveryNo.Text.Trim());

                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                        if (ds.Tables[0].Rows.Count == 0 && hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Correct Delivery No !');</script>");
                            txtDeliveryNo.Text = "";
                            txtDeliveryNo.Focus();
                            ClearConrol();
                            //txtDeliveryNo.Style.Add("border", "2px solid red");
                        }
                    }
                }
                txtID.Text = "";
                txtMReceiptNo.Text = objMatReceipt.GenerateReceipt(sDealerCode, iDealerID);
                txtMReceiptDate.Text = Func.Common.sGetCurrentDate(1, false);
                objMatReceipt = null;
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

                //if (ExportLocation.bDistributor == "Y" && (ddlReceiptType.SelectedValue == "Y" || ddlReceiptType.SelectedValue == "N") && ddlInvoiceType.SelectedValue == "Y")
                //{
                //    ddlInvoice.Style.Add("display", "");
                //    txtDMSInvNo.Style.Add("display", "none");
                //    FillCombo();
                //}
                //else
                //{
                //    ddlInvoice.Style.Add("display", "none");
                //    txtDMSInvNo.Style.Add("display", "");
                //}

                DataSet ds = new DataSet();

                objMatReceipt = new clsMaterialReceipt();
                ds = objMatReceipt.GetMatReceipt(iMatReceiptID, "New", iUser_ID, Func.Convert.iConvertToInt(Session["iDealerID"]), ddlInvoice.SelectedValue, iSupplierId, ddlInvoiceType.SelectedValue, "");

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtMReceiptNo.Text = objMatReceipt.GenerateReceipt(sDealerCode, iDealerID);
                if (hdnIsAutoReceipt.Value == "N")
                {
                    // txtDMSInvDate.Text = Func.Common.sGetCurrentDate(1, false);
                    //txtLRDate.Text = Func.Common.sGetCurrentDate(1, false);
                    txtDMSInvDate.Text = "";
                    txtLRDate.Text = "";
                }

                //New Code for Data Testing
                //DateTime dt = DateTime.ParseExact(Func.Common.sGetCurrentDate(1, false), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //txtMReceiptDate.Text = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);



                txtMReceiptDate.Text = Func.Common.sGetCurrentDate(1, false);
                objMatReceipt = null;
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
                txtMReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MatReceipt_No"]);
                txtMReceiptDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MatReceipt_Date"]);
                hdnDealerID.Value = Func.Convert.sConvertToString(Session["iDealerID"]);

                //DropDownList drpDealerName = (DropDownList)ExportLocation.FindControl("drpDealerName");
                //if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_ID"]) == "0")
                //{
                //    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                //    ExportLocation.SetControlValue();
                //}
                //else
                //{
                //    drpDealerName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_ID"]);
                //    ExportLocation.FillLocation();
                //}

                if (txtID.Text != "")
                {
                    ddlInvoice.Style.Add("display", "none");
                    txtDMSInvNo.Style.Add("display", "");
                    txtDMSInvNo.Enabled = false;
                    hdnInvoiceID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvoiceID"]);
                    txtDMSInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DMSInv_No"]);
                    //txtDeliveryNo.CssClass = "NonEditableFields";
                }
                else if (hdnIsAutoReceipt.Value == "Y")
                {
                    //ddlInvoice.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DMSInv_No"]);
                    ddlInvoice.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvoiceID"]);
                    //txtLR_No.Enabled = false;
                    //txtDMSInvNo.CssClass = "NonEditableFields";
                    //txtDeliveryNo.Attributes.Add("disabled", "disabled");
                    //txtDMSInvNo.Attributes.Add("disabled", "disabled");
                    txtDMSInvNo.Enabled = true;
                }
                else if (hdnIsAutoReceipt.Value == "N")
                {
                    txtDMSInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DMSInv_No"]);
                    txtDMSInvNo.Enabled = true;
                    txtDMSInvNo.CssClass = "TextBoxForString";
                    txtLR_No.Enabled = true;
                    txtLRDate.Enabled = true;
                    txtDMSInvDate.Enabled = true;
                    txtLR_No.Enabled = true;
                }

                txtMReceiptDate.Enabled = false;
                txtDMSInvDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DMSInv_date"]);
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoReceipt"]) == "Y")
                {
                    ddlInvoiceType.Items.Add(new ListItem("Auto", "Y", true));
                    ddlInvoiceType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoReceipt"]);
                }
                else
                {
                    ddlInvoiceType.Items.Add(new ListItem("Manual", "N", true));
                    ddlInvoiceType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_AutoReceipt"]);
                }


                txtLR_No.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_No"]);
                txtLRDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_Date"]);
                txtDeliveryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delivery_No"]);
                hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
                hdnSupTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PerAmt"]) == "Per")
                {
                    rbtLstDiscount.SelectedValue = "Per";
                }
                else
                {
                    rbtLstDiscount.SelectedValue = "Amt";
                }

                Session["MRPartDetails"] = null;
                Session["MRGrpTaxDetails"] = null;
                Session["MRTaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["MRPartDetails"] = dtDetails;
                dtMRGrpTaxDetails = ds.Tables[2];
                Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;


                dtMRTaxDetails = ds.Tables[3];
                Session["MRTaxDetails"] = dtMRTaxDetails;
                //if (hdnIsAutoReceipt.Value == "Y")
                //    GrdDocTaxDet.Enabled = false;
                //else
                //    GrdDocTaxDet.Enabled = true;

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                BindDataToGrid();
                if (hdnIsAutoReceipt.Value == "Y")
                {
                    MakeEnableDisableControlForAuto(false);
                    if (ExportLocation.sSupplierType == "B")
                        CreateNewRowToTaxGroupDetailsTable();
                    ReBindDataToTax();
                    Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
                }
                else
                {
                    MakeEnableDisableControlForAuto(true);
                }

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
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
                    MakeEnableDisableControlForAuto(false);
                }
                if (txtUserType.Text.Trim() == "6")
                {
                    ddlInvoice.Enabled = false;
                    rbtLstDiscount.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void MakeEnableDisableControlForAuto(bool bEnable)
        {
            txtDMSInvNo.Enabled = bEnable;
            txtDMSInvDate.Enabled = bEnable;
            txtLR_No.Enabled = bEnable;
            txtLRDate.Enabled = bEnable;
            GrdPartGroup.Enabled = bEnable;
            GrdDocTaxDet.Enabled = bEnable;
        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            ddlInvoice.Enabled = bEnable;
            txtDeliveryNo.Enabled = bEnable;
            //txtDMSInvNo.Enabled = bEnable;
            PartGrid.Enabled = bEnable;
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }

        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerID = 3;
                objMatReceipt = new clsMaterialReceipt();
                ds = objMatReceipt.GetMatReceipt(iMatReceiptID, "Max", iUser_ID, iDealerID, "", Func.Convert.iConvertToInt(ExportLocation.iSupplierId), ddlInvoiceType.SelectedValue, "0");
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
                    Session["MRPartDetails"] = null;
                    Session["MRGrpTaxDetails"] = null;
                    Session["MRTaxDetails"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objMatReceipt = null;
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
                    //if ((ExportLocation.bDistributor == "Y" || ExportLocation.bDistributor == "N") && ddlReceiptType.SelectedValue == "Y")
                    //{
                    //    ddlInvoiceType.SelectedValue = "Y";
                    //    ddlInvoiceType.Enabled = false;
                    //}
                    //else if (ExportLocation.bDistributor == "N" && ddlReceiptType.SelectedValue == "N")
                    //{
                    //    ddlInvoiceType.SelectedValue = "N";
                    //    ddlInvoiceType.Enabled = false;
                    //}
                    //else
                    //{
                    //    ddlInvoiceType.Enabled = true;
                    //}

                    //if (Location .bDistributor =="Y" && (ddlReceiptType.SelectedValue == "Y" || ddlReceiptType.SelectedValue == "N") && ddlInvoiceType.SelectedValue == "Y")
                    if (ExportLocation.bDistributor == "N" && ddlInvoiceType.SelectedValue == "Y" || ExportLocation.sSupplierType == "M" || ExportLocation.iSupType == 18)
                    {
                        ddlInvoice.Style.Add("display", "");
                        txtDMSInvNo.Style.Add("display", "none");
                        FillCombo();
                    }
                    else
                    {
                        ddlInvoice.Style.Add("display", "none");
                        txtDMSInvNo.Style.Add("display", "");
                    }
                    //if (ddlInvoiceType.SelectedValue == "N" || (ddlInvoiceType.SelectedValue == "Y" && ExportLocation.bDistributor == "N"))
                    //{
                    //    txtDMSInvNo.ReadOnly = false;
                    //    txtDMSInvDate.ReadOnly = false;
                    //}
                    //else
                    //{
                    //    txtDMSInvNo.ReadOnly = true;
                    //    txtDMSInvDate.ReadOnly = true;
                    //}
                    hdnIsDistributor.Value = ExportLocation.bDistributor;
                    ClearConrol();

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    Session["MRPartDetails"] = null;
                    Session["MRGrpTaxDetails"] = null;
                    Session["MRTaxDetails"] = null;
                    //if ((ddlReceiptType.SelectedValue == "N" && ddlInvoiceType.SelectedValue == "N") || (Location .bDistributor =="N" && ddlReceiptType.SelectedValue == "Y" && ddlInvoiceType.SelectedValue == "Y"))
                    if (ddlInvoiceType.SelectedValue == "N" || (ExportLocation.bDistributor == "N" && ddlInvoiceType.SelectedValue == "Y"))
                        DisplayPreviousRecord();
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
                iMatReceiptID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
                //ddlInvoice.Style.Add("display", "none");
                txtDMSInvNo.Style.Add("display", "");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ClearConrol()
        {

            Session["MRPartDetails"] = null;
            Session["MRGrpTaxDetails"] = null;
            Session["MRTaxDetails"] = null;
            dtDetails = null;
            PartGrid.DataSource = dtDetails;
            PartGrid.DataBind();
            dtMRGrpTaxDetails = null;
            GrdPartGroup.DataSource = dtMRGrpTaxDetails;
            GrdPartGroup.DataBind();
            dtMRTaxDetails = null;
            GrdDocTaxDet.DataSource = dtMRTaxDetails;
            GrdDocTaxDet.DataBind();
            txtDMSInvNo.Text = "";
            txtDMSInvDate.Text = "";
            txtMReceiptNo.Text = "";
            txtMReceiptDate.Text = "";
            txtTotal.Text = "0.00";
            txtTotalQty.Text = "0";
            txtID.Text = "";
            txtLR_No.Text = "";
            txtLRDate.Text = "";

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
                SearchGrid.AddToSearchCombo("Receipt No");
                SearchGrid.AddToSearchCombo("Receipt Date");
                SearchGrid.AddToSearchCombo("Receipt Status");
                SearchGrid.AddToSearchCombo("Invoice No");
                SearchGrid.AddToSearchCombo("Invoice Date");
                SearchGrid.iDealerID = ExportLocation.iSupplierId;
                //SearchGrid.iDealerID = iDealerID;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUser_ID) + "," + ExportLocation.bDistributor;
                SearchGrid.sSqlFor = "SparesMatReceipt";
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
                dtHdr.Columns.Add(new DataColumn("MatReceipt_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("MatReceipt_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("User_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Distributor_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DMSInv_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DMSInv_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("MRN_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("IS_Cancel", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Is_Distributor", typeof(string)));
                //dtHdr.Columns.Add(new DataColumn("Is_WithPO", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_AutoReceipt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LR_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LR_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Delivery_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("IS_PerAmt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Ch_Rpt", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["MatReceipt_No"] = txtMReceiptNo.Text;
                dr["MatReceipt_Date"] = txtMReceiptDate.Text;
                dr["Distributor_ID"] = Session["iDealerID"].ToString();
                dr["User_ID"] = iUser_ID;
                if (txtID.Text != "" || hdnIsAutoReceipt.Value == "N")
                {
                    dr["DMSInv_No"] = Func.Convert.sConvertToString(txtDMSInvNo.Text);
                }
                else
                    dr["DMSInv_No"] = Func.Convert.sConvertToString(ddlInvoice.SelectedItem.Text);
                dr["DMSInv_Date"] = txtDMSInvDate.Text;
                dr["MRN_Date"] = txtMReceiptDate.Text;
                dr["LR_No"] = txtLR_No.Text;
                dr["LR_Date"] = txtLRDate.Text;
                dr["Is_Confirm"] = "N";
                dr["IS_Cancel"] = "N";
                //Changed by VIkram 08.07.2016
                dr["Supplier_ID"] = (ExportLocation.bDistributor == "N") ? Func.Convert.iConvertToInt(ExportLocation.iSupplierId) : 0;
                // dr["Supplier_ID"] = (ExportLocation.bDistributor == "N") ? ExportLocation.iDealerID : 0;
                dr["Is_Distributor"] = ExportLocation.bDistributor;
                //dr["Is_WithPO"] = ddlReceiptType .SelectedValue ;
                dr["Is_AutoReceipt"] = ddlInvoiceType.SelectedValue;
                dr["Delivery_No"] = txtDeliveryNo.Text;
                dr["IS_PerAmt"] = Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue);
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["Is_Ch_Rpt"] = (ExportLocation.sSupplierType == "B") ? "Y" : "N";

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
            bool bValidateRecord = true;
            if (txtMReceiptDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (hdnIsAutoReceipt.Value == "N" && txtDMSInvNo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Invoice No.";
                bValidateRecord = false;
            }
            if (hdnIsAutoReceipt.Value == "N" && txtDMSInvDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Invoice date.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            if (bValidateRecord == false)
            {
                CheckInvoiceno();
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsMaterialReceipt objMatReceipt = new clsMaterialReceipt();
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

                if (objMatReceipt.bSaveRecordWithPart(sDealerCode, dtHdr, dtDetails, dtMRGrpTaxDetails, dtMRTaxDetails, ref iMatReceiptID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iMatReceiptID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Material Receipt") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Material Receipt") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Material Receipt") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Material Receipt") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            //DataTable dtExistRecord = null;
            //DataTable dtParts = null;
            //StringBuilder sPartIDs = null;
            try
            {

                //objMatReceipt = new clsMaterialReceipt();
                //dtParts = new DataTable();
                //dtExistRecord = new DataTable();
                //dtExistRecord = (DataTable)Session["MRPartDetails"];
                //if (dtExistRecord != null)
                //{
                //    sPartIDs = new StringBuilder();
                //    for (int icnt = 0; icnt < dtExistRecord.Rows.Count; icnt++)
                //    {
                //        //if (ExportLocation.bDistributor == "N" && ddlReceiptType.SelectedValue == "Y" && ddlInvoiceType.SelectedValue == "Y")
                //        if (ExportLocation.bDistributor == "N" && ddlInvoiceType.SelectedValue == "N")
                //        {
                //            if (sPartIDs.Length == 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["ID"]) != 0) // ID is PO_HDR_ID
                //                sPartIDs.Append(Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["ID"]) + "_" + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));// ID is PO_HDR_ID
                //            else if (sPartIDs.Length > 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["ID"]) != 0)// ID is PO_HDR_ID
                //                sPartIDs.Append("," + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["ID"]) + "_" + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));// ID is PO_HDR_ID
                //        }
                //        else
                //        {

                //            if (sPartIDs.Length == 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //                sPartIDs.Append(Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //            else if (sPartIDs.Length > 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //                sPartIDs.Append("," + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //        }
                //    }
                //    //dtParts = objMatReceipt.GetPartForReceiptDetails(Func.Convert.sConvertToString(sPartIDs), iUser_ID, ExportLocation.iDealerID, Func.Convert.iConvertToInt(txtID.Text), ExportLocation.bDistributor,ddlReceiptType .SelectedValue ,ddlInvoiceType .SelectedValue );
                //    dtParts = objMatReceipt.GetPartForReceiptDetails(Func.Convert.sConvertToString(sPartIDs), iUser_ID, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), Func.Convert.iConvertToInt(txtID.Text), ExportLocation.bDistributor, ddlInvoiceType.SelectedValue);
                //    if (dtParts != null && dtParts.Rows.Count > 0)
                //        Session["MRPartDetails"] = dtParts;
                //}

                //FillDetailsFromGrid(false);
                //BindDataToGrid();
                //FillDetailsFromGrid(false);
                ////CreateNewRowToTaxGroupDetailsTable();
                //Session["MRPartDetails"] = dtDetails;
                //Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
                //Session["MRTaxDetails"] = dtMRTaxDetails;
                //BindDataToGrid();



                /////////Vikram Kite
                //FillDetailsFromGrid(false);
                //CreateNewRowToTaxGroupDetailsTable();
                //Session["MRPartDetails"] = dtDetails;
                //Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
                //Session["MRTaxDetails"] = dtMRTaxDetails;
                //BindDataToGrid();


                /****   MODELPOPUP NEW CODE ****/
                bindGrid("S", "x");
                mpeSelectPart.Show();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                //if (dtParts != null) dtParts = null;
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string strPosNo = "";
                dtDetails = (DataTable)Session["MRPartDetails"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                int iDtSelPartRow = 0;
                string sQtyMsg = "";
                bDetailsRecordExist = true;
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                        TextBox txtPODetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPODetID");
                        TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
                        TextBox txtInvDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvDetID");
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                        if (txtPartID.Text != "" && txtPartID.Text != "0")
                        {
                            for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                            {
                                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["PO_Det_ID"]) == Func.Convert.iConvertToInt(txtPODetID.Text)
                                    && Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["SAP_Order_No"]) == txtSAPOrderNo.Text.Trim()
                                    && Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Inv_Det_ID"]) == Func.Convert.sConvertToString(txtInvDetID.Text))
                                {
                                    iCntForSelect = iCntForSelect + 1;

                                    if (Chk.Checked == true)
                                    {
                                        dtDetails.Rows[iMRowCnt]["Status"] = "D";
                                        iCntForDelete++;
                                    }
                                    else
                                    {
                                        //if (ddlInvoiceType.SelectedValue == 'N'.ToString()) //Rate Editable for only Local Part Group
                                        //    MrpRate.Enabled = true;
                                        //else 
                                        //MrpRate.Enabled = false; 

                                        dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
                                        dtDetails.Rows[iMRowCnt]["PO_Det_ID"] = Func.Convert.iConvertToInt(txtPODetID.Text);
                                        dtDetails.Rows[iMRowCnt]["Inv_Det_ID"] = Func.Convert.iConvertToInt(txtInvDetID.Text);
                                        dtDetails.Rows[iMRowCnt]["SAP_Order_No"] = txtSAPOrderNo.Text.Trim();
                                        // Get Bal_PO_Qty
                                        TextBox txtbalPOQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtbalPOQty");
                                        dtDetails.Rows[iMRowCnt]["Bal_PO_Qty"] = Func.Convert.dConvertToDouble(txtbalPOQty.Text);
                                        //new  Get txtBillqty 
                                        TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
                                        dtDetails.Rows[iMRowCnt]["Bill_Qty"] = Func.Convert.dConvertToDouble(txtBillQty.Text);
                                        // Get Recv_Qty
                                        TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                                        dtDetails.Rows[iMRowCnt]["Recv_Qty"] = Func.Convert.dConvertToDouble(txtRecvQty.Text);
                                        // Get Unit
                                        TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                                        dtDetails.Rows[iMRowCnt]["Unit"] = Func.Convert.sConvertToString(txtUnit.Text.Trim());
                                        // Get Price
                                        TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                                        dtDetails.Rows[iMRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);
                                        // Get MRPRate
                                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                        dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);
                                        //For Manual Receipt only ---------------------
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

                                        // Get DiscPer
                                        TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                                        dtDetails.Rows[iMRowCnt]["Disc_Per"] = Func.Convert.dConvertToDouble(txtDiscPer.Text);
                                        // Get AccRate
                                        TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                                        dtDetails.Rows[iMRowCnt]["Accept_Rate"] = Func.Convert.dConvertToDouble(txtAccRate.Text);
                                        // Get Total
                                        TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                        dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                                        // Get txtMRP_Rate
                                        TextBox txtMRP_Rate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRP_Rate");
                                        dtDetails.Rows[iMRowCnt]["MRP_Rate"] = Func.Convert.dConvertToDouble(txtMRP_Rate.Text);
                                        // Get txtAssValue
                                        TextBox txtAssValue = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAssValue");
                                        dtDetails.Rows[iMRowCnt]["Ass_Value"] = Func.Convert.dConvertToDouble(txtAssValue.Text);
                                        // Get Descripancy
                                        DropDownList drpDescripancy = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpDescripancy");
                                        dtDetails.Rows[iMRowCnt]["Descripancy_YN"] = Func.Convert.sConvertToString(drpDescripancy.SelectedValue);
                                        // Get Shortage Qty
                                        TextBox txtShortageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtShortageQty");
                                        dtDetails.Rows[iMRowCnt]["Shortage_Qty"] = Func.Convert.dConvertToDouble(txtShortageQty.Text);
                                        //Get Excess Qty
                                        TextBox txtExcessQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExcessQty");
                                        dtDetails.Rows[iMRowCnt]["Excess_Qty"] = Func.Convert.dConvertToDouble(txtExcessQty.Text);
                                        //Get Damage_Qty
                                        TextBox txtDamageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDamageQty");
                                        dtDetails.Rows[iMRowCnt]["Damage_Qty"] = Func.Convert.dConvertToDouble(txtDamageQty.Text);
                                        //Get Man_Defect_Qty
                                        TextBox txtManDefectQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtManDefectQty");
                                        dtDetails.Rows[iMRowCnt]["Man_Defect_Qty"] = Func.Convert.dConvertToDouble(txtManDefectQty.Text);
                                        //Get Wrong_Supply_Qty
                                        TextBox txtWrongSupplyQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrongSupplyQty");
                                        dtDetails.Rows[iMRowCnt]["Wrong_Supply_Qty"] = Func.Convert.dConvertToDouble(txtWrongSupplyQty.Text);
                                        // Get Descripancy
                                        DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
                                        dtDetails.Rows[iMRowCnt]["Retain_YN"] = Func.Convert.sConvertToString(drpRetain.SelectedValue);
                                        // Get Wrong Part ID
                                        TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
                                        dtDetails.Rows[iMRowCnt]["Wrg_Part_ID"] = Func.Convert.iConvertToInt(txtWrgPartID.Text);
                                        // Get Wrong Part No
                                        TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
                                        dtDetails.Rows[iMRowCnt]["Wrg_Part_No"] = txtWrgPartNo.Text.Trim();
                                        //Get Wrong Part Name
                                        TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
                                        dtDetails.Rows[iMRowCnt]["Wrg_Part_Name"] = txtWrgPartName.Text.Trim();
                                        // Tax Per
                                        TextBox txtTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxPer");
                                        dtDetails.Rows[iMRowCnt]["TAX_Per"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);
                                        // Tax AMount
                                        TextBox txtTaxAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxAmt");
                                        dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTaxAmt.Text);

                                        DropDownList drpTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpTax1");
                                        dtDetails.Rows[iMRowCnt]["Tax1_Code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);
                                        // Tax1 Per
                                        TextBox txtTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Per");
                                        dtDetails.Rows[iMRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);
                                        // Tax1 Amt
                                        TextBox txtTax1Amt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Amt");
                                        dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTax1Amt.Text);
                                        //Status
                                        dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;
                                        //Get BFR GST Rate Flag
                                        TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                                        dtDetails.Rows[iMRowCnt]["BFRGST"] = txtBFRGST.Text.Trim();
                                    }
                                }//END if
                            }//END Inner For
                        }//END Inner IF

                    }//End Outer For

                }//END Outer IF

                //for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                //{
                //    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                //    {
                //        //PartID                
                //        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                //        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                //        LinkButton lblCancel = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lblCancel");
                //        TextBox txtPODetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPODetID");
                //        TextBox MrpRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                //        TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
                //        TextBox txtInvDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvDetID");

                //        if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                //            && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["PO_Det_ID"]) == Func.Convert.iConvertToInt(txtPODetID.Text)
                //            && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["SAP_Order_No"]) == Func.Convert.iConvertToInt(txtSAPOrderNo.Text)
                //            && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Inv_Det_ID"]) == Func.Convert.iConvertToInt(txtInvDetID.Text)
                //            //&& (hdnIsAutoReceipt.Value=="N" || (hdnIsAutoReceipt.Value=="Y" &&  same part in Invoice 
                //            //Func.Convert.dConvertToDouble(dtDetails.Rows[iMRowCnt]["Bill_Qty"]) == Func.Convert.dConvertToDouble(txtBillQty.Text))
                //            //)

                //              )
                //        {
                //            if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
                //            {
                //                if (txtStatus.Text == "U")
                //                {
                //                    if (strPosNo == "")
                //                        strPosNo = iRowCnt.ToString();
                //                    else
                //                        strPosNo = strPosNo + "," + iRowCnt.ToString();
                //                }
                //                continue;
                //            }
                //            if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                //            {
                //                if (txtStatus.Text == "O")
                //                {
                //                    if (strPosNo == "")
                //                        strPosNo = iRowCnt.ToString();
                //                    else
                //                        strPosNo = strPosNo + "," + iRowCnt.ToString();
                //                }
                //                continue;
                //            }
                //            if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                //            {
                //                if (txtStatus.Text == "M")
                //                {
                //                    if (strPosNo == "")
                //                        strPosNo = iRowCnt.ToString();
                //                    else
                //                        strPosNo = strPosNo + "," + iRowCnt.ToString();
                //                }
                //                continue;
                //            }

                //            if (txtPartID.Text != "" && txtPartID.Text != "0")
                //            {
                //                iCntForSelect = iCntForSelect + 1;
                //            }

                //            iDtSelPartRow = 0;
                //            for (int iDtRowCnt = 0; iDtRowCnt < dtDetails.Rows.Count; iDtRowCnt++)
                //            {
                //                if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                //                    && Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["PO_Det_ID"]) == Func.Convert.iConvertToInt(txtPODetID.Text))
                //                {
                //                    iDtSelPartRow = iDtRowCnt;
                //                    break;
                //                }
                //            }
                //            //Rate Editable for only Local Part Group
                //            //if (ddlInvoiceType.SelectedValue == 'N'.ToString())
                //            //{
                //            //    MrpRate.Enabled = true;
                //            //}
                //            //else { MrpRate.Enabled = false; }

                //            dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
                //            dtDetails.Rows[iMRowCnt]["PO_Det_ID"] = Func.Convert.iConvertToInt(txtPODetID.Text);
                //            dtDetails.Rows[iMRowCnt]["Inv_Det_ID"] = Func.Convert.iConvertToInt(txtInvDetID.Text);
                //            // Get Bal_PO_Qty
                //            TextBox txtbalPOQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtbalPOQty");
                //            dtDetails.Rows[iMRowCnt]["Bal_PO_Qty"] = Func.Convert.dConvertToDouble(txtbalPOQty.Text);
                //            //new  Get txtBillqty 
                //            TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
                //            dtDetails.Rows[iMRowCnt]["Bill_Qty"] = Func.Convert.dConvertToDouble(txtBillQty.Text);
                //            // Get Recv_Qty
                //            TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                //            dtDetails.Rows[iMRowCnt]["Recv_Qty"] = Func.Convert.dConvertToDouble(txtRecvQty.Text);
                //            // Get Unit
                //            TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                //            dtDetails.Rows[iMRowCnt]["Unit"] = Func.Convert.sConvertToString(txtUnit.Text);
                //            // Get Price
                //            TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                //            dtDetails.Rows[iMRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);
                //            // Get MRPRate
                //            TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                //            dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);
                //            //For Manual Receipt only ---------------------
                //            DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                //            dtDetails.Rows[iMRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                //            DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                //            Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                //            if (DrpPartTax1.Items.Count == 2)
                //            {
                //                DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
                //            }

                //            DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                //            Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                //            if (DrpPartTax2.Items.Count == 2)
                //            {
                //                DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
                //            }

                //            // Get DiscPer
                //            TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                //            dtDetails.Rows[iMRowCnt]["Disc_Per"] = Func.Convert.dConvertToDouble(txtDiscPer.Text);
                //            // Get AccRate
                //            TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                //            dtDetails.Rows[iMRowCnt]["Accept_Rate"] = Func.Convert.dConvertToDouble(txtAccRate.Text);
                //            // Get Total
                //            TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                //            dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                //            // Get txtMRP_Rate
                //            TextBox txtMRP_Rate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRP_Rate");
                //            dtDetails.Rows[iMRowCnt]["MRP_Rate"] = Func.Convert.dConvertToDouble(txtMRP_Rate.Text);
                //            // Get txtAssValue
                //            TextBox txtAssValue = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAssValue");
                //            dtDetails.Rows[iMRowCnt]["Ass_Value"] = Func.Convert.dConvertToDouble(txtAssValue.Text);
                //            // Get Descripancy
                //            DropDownList drpDescripancy = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpDescripancy");
                //            dtDetails.Rows[iMRowCnt]["Descripancy_YN"] = Func.Convert.sConvertToString(drpDescripancy.SelectedValue);
                //            // Get Shortage Qty
                //            TextBox txtShortageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtShortageQty");
                //            dtDetails.Rows[iMRowCnt]["Shortage_Qty"] = Func.Convert.dConvertToDouble(txtShortageQty.Text);
                //            //Get Excess Qty
                //            TextBox txtExcessQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExcessQty");
                //            dtDetails.Rows[iMRowCnt]["Excess_Qty"] = Func.Convert.dConvertToDouble(txtExcessQty.Text);
                //            //Get Damage_Qty
                //            TextBox txtDamageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDamageQty");
                //            dtDetails.Rows[iMRowCnt]["Damage_Qty"] = Func.Convert.dConvertToDouble(txtDamageQty.Text);
                //            //Get Man_Defect_Qty
                //            TextBox txtManDefectQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtManDefectQty");
                //            dtDetails.Rows[iMRowCnt]["Man_Defect_Qty"] = Func.Convert.dConvertToDouble(txtManDefectQty.Text);

                //            //Get Wrong_Supply_Qty
                //            TextBox txtWrongSupplyQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrongSupplyQty");
                //            dtDetails.Rows[iMRowCnt]["Wrong_Supply_Qty"] = Func.Convert.dConvertToDouble(txtWrongSupplyQty.Text);

                //            // Get Descripancy
                //            DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
                //            dtDetails.Rows[iMRowCnt]["Retain_YN"] = Func.Convert.sConvertToString(drpRetain.SelectedValue);

                //            // Get Wrong Part ID
                //            TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
                //            dtDetails.Rows[iMRowCnt]["Wrg_Part_ID"] = Func.Convert.iConvertToInt(txtWrgPartID.Text);

                //            // Get Wrong Part No
                //            TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
                //            dtDetails.Rows[iMRowCnt]["Wrg_Part_No"] = Func.Convert.sConvertToString(txtWrgPartNo.Text);

                //            //Get Wrong Part Name
                //            TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
                //            dtDetails.Rows[iMRowCnt]["Wrg_Part_Name"] = Func.Convert.sConvertToString(txtWrgPartName.Text);

                //            //Get SAP Order No for with dCAN PO
                //            //TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
                //            dtDetails.Rows[iMRowCnt]["SAP_Order_No"] = Func.Convert.sConvertToString(txtSAPOrderNo.Text);
                //            // Tax Per
                //            TextBox txtTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxPer");
                //            dtDetails.Rows[iMRowCnt]["TAX_Per"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);
                //            // Tax AMount
                //            TextBox txtTaxAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxAmt");
                //            dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTaxAmt.Text);

                //            DropDownList drpTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpTax1");
                //            dtDetails.Rows[iMRowCnt]["Tax1_Code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);
                //            // Tax1 Per
                //            TextBox txtTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Per");
                //            dtDetails.Rows[iMRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);
                //            // Tax1 Amt
                //            TextBox txtTax1Amt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Amt");
                //            dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTax1Amt.Text);

                //            CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                //            // Get Status                           
                //            dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

                //            if (txtStatus.Text == "C")
                //            {
                //                lblCancel.Attributes.Add("disabled", "true");
                //                lblCancel.Text = "Cancelled";
                //            }
                //            //dtDetails.Rows[iRowCnt]["Status"] = "";            
                //            if (Chk.Checked == true)
                //            {
                //                dtDetails.Rows[iRowCnt]["Status"] = "D";
                //                iCntForDelete++;
                //            }
                //            else
                //            {
                //                if (txtPartID.Text != "")// && txtRecvQty.Text != "0"
                //                {
                //                    if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "M")
                //                    {
                //                        dtDetails.Rows[iMRowCnt]["Status"] = "M";
                //                        bDetailsRecordExist = true;
                //                    }
                //                    else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "U")
                //                    {
                //                        dtDetails.Rows[iMRowCnt]["Status"] = "U";
                //                        bDetailsRecordExist = true;
                //                    }
                //                    else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
                //                    {
                //                        dtDetails.Rows[iMRowCnt]["Status"] = "E";
                //                        bDetailsRecordExist = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                {
                    //if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D"
                    //    && hdnIsAutoReceipt.Value == "Y" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Recv_Qty"]) == 0)
                    //{
                    //    iCntError = iCntError + 1;
                    //    sQtyMsg = "Please enter Receive Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //    break;
                    //}
                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && hdnIsAutoReceipt.Value == "N" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Bill_Qty"]) == 0)
                    {
                        iCntError = iCntError + 1;
                        sQtyMsg = "Please enter Bill Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                        break;
                    }
                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["PartTaxID"]) == 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && hdnIsAutoReceipt.Value == "N")
                    {
                        iCntError = iCntError + 1;
                        sQtyMsg = "Please Select Part Tax at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                        break;
                    }
                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Wrong_Supply_Qty"]) > 0 && hdnIsAutoReceipt.Value == "Y" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Wrg_Part_ID"]) == 0)
                    {
                        iCntError = iCntError + 1;
                        sQtyMsg = "Please Select Wrong Part at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
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
                //For Tax Details
                dtMRGrpTaxDetails = (DataTable)(Session["MRGrpTaxDetails"]);
                dtMRTaxDetails = (DataTable)(Session["MRTaxDetails"]);

                //if (dtDetails.Rows.Count > 1 && dtMRGrpTaxDetails.Rows.Count == 0)
                //    CreateNewRowToTaxGroupDetailsTable();

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Net Reverse Amount
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //dtMRGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtMRGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }
                //CreateNewRowToTaxGroupDetailsTable();
                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtMRTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtMRTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtMRTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtMRTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtMRTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);
                    //Get txtGrExciseAmt
                    TextBox txtGrExciseAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrExciseAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["Excise_Amt"] = Func.Convert.dConvertToDouble(txtGrExciseAmt.Text);
                    //Get txtGrInsuAmt
                    TextBox txtGrInsuAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrInsuAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["Insu_Amt"] = Func.Convert.dConvertToDouble(txtGrInsuAmt.Text);
                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtMRTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtMRTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtMRTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtMRTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtMRTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtMRTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //private void FillDetailsFromGrid(bool bDisplayMsg)
        //{
        //    try
        //    {
        //        string strPosNo = "";
        //        dtDetails = (DataTable)Session["MRPartDetails"];

        //        int iCntForDelete = 0;
        //        int iCntForSelect = 0;
        //        int iCntError = 0;
        //        int iDtSelPartRow = 0;
        //        string sQtyMsg = "";
        //        for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
        //        {
        //            for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
        //            {
        //                //PartID                
        //                TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
        //                TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
        //                LinkButton lblCancel = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lblCancel");
        //                TextBox txtPODetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPODetID");
        //                TextBox MrpRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
        //                TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
        //                TextBox txtInvDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvDetID");
        //                //TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
        //                //double s = Func.Convert.dConvertToDouble(txtBillQty.Text);
        //                //double dt = Func.Convert.dConvertToDouble(dtDetails.Rows[iMRowCnt]["Bill_Qty"]);
        //                //&& Func.Convert.iConvertToInt(txtPartID.Text)!=0
        //                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
        //                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["PO_Det_ID"]) == Func.Convert.iConvertToInt(txtPODetID.Text)
        //                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["SAP_Order_No"]) == Func.Convert.iConvertToInt(txtSAPOrderNo.Text)
        //                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Inv_Det_ID"]) == Func.Convert.iConvertToInt(txtInvDetID.Text)
        //                    //&& (hdnIsAutoReceipt.Value=="N" || (hdnIsAutoReceipt.Value=="Y" &&  same part in Invoice 
        //                    //Func.Convert.dConvertToDouble(dtDetails.Rows[iMRowCnt]["Bill_Qty"]) == Func.Convert.dConvertToDouble(txtBillQty.Text))
        //                    //)

        //                      )
        //                {
        //                    if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
        //                    {
        //                        if (txtStatus.Text == "U")
        //                        {
        //                            if (strPosNo == "")
        //                                strPosNo = iRowCnt.ToString();
        //                            else
        //                                strPosNo = strPosNo + "," + iRowCnt.ToString();
        //                        }
        //                        continue;
        //                    }
        //                    if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
        //                    {
        //                        if (txtStatus.Text == "O")
        //                        {
        //                            if (strPosNo == "")
        //                                strPosNo = iRowCnt.ToString();
        //                            else
        //                                strPosNo = strPosNo + "," + iRowCnt.ToString();
        //                        }
        //                        continue;
        //                    }
        //                    if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
        //                    {
        //                        if (txtStatus.Text == "M")
        //                        {
        //                            if (strPosNo == "")
        //                                strPosNo = iRowCnt.ToString();
        //                            else
        //                                strPosNo = strPosNo + "," + iRowCnt.ToString();
        //                        }
        //                        continue;
        //                    }

        //                    if (txtPartID.Text != "" && txtPartID.Text != "0")
        //                    {
        //                        iCntForSelect = iCntForSelect + 1;
        //                    }

        //                    iDtSelPartRow = 0;
        //                    for (int iDtRowCnt = 0; iDtRowCnt < dtDetails.Rows.Count; iDtRowCnt++)
        //                    {
        //                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
        //                            && Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["PO_Det_ID"]) == Func.Convert.iConvertToInt(txtPODetID.Text))
        //                        {
        //                            iDtSelPartRow = iDtRowCnt;
        //                            break;
        //                        }
        //                    }
        //                    //Rate Editable for only Local Part Group
        //                    //if (ddlInvoiceType.SelectedValue == 'N'.ToString())
        //                    //{
        //                    //    MrpRate.Enabled = true;
        //                    //}
        //                    //else { MrpRate.Enabled = false; }

        //                    dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
        //                    dtDetails.Rows[iMRowCnt]["PO_Det_ID"] = Func.Convert.iConvertToInt(txtPODetID.Text);
        //                    dtDetails.Rows[iMRowCnt]["Inv_Det_ID"] = Func.Convert.iConvertToInt(txtInvDetID.Text);
        //                    // Get Bal_PO_Qty
        //                    TextBox txtbalPOQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtbalPOQty");
        //                    dtDetails.Rows[iMRowCnt]["Bal_PO_Qty"] = Func.Convert.dConvertToDouble(txtbalPOQty.Text);
        //                    //new  Get txtBillqty 
        //                    TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
        //                    dtDetails.Rows[iMRowCnt]["Bill_Qty"] = Func.Convert.dConvertToDouble(txtBillQty.Text);
        //                    // Get Recv_Qty
        //                    TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
        //                    dtDetails.Rows[iMRowCnt]["Recv_Qty"] = Func.Convert.dConvertToDouble(txtRecvQty.Text);
        //                    // Get Unit
        //                    TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
        //                    dtDetails.Rows[iMRowCnt]["Unit"] = Func.Convert.sConvertToString(txtUnit.Text);
        //                    // Get Price
        //                    TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
        //                    dtDetails.Rows[iMRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);
        //                    // Get MRPRate
        //                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
        //                    dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);
        //                    //For Manual Receipt only ---------------------
        //                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
        //                    dtDetails.Rows[iMRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

        //                    DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
        //                    Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
        //                    if (DrpPartTax1.Items.Count == 2)
        //                    {
        //                        DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
        //                    }

        //                    DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
        //                    Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
        //                    if (DrpPartTax2.Items.Count == 2)
        //                    {
        //                        DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["PartTaxID"]);
        //                    }

        //                    // Get DiscPer
        //                    TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
        //                    dtDetails.Rows[iMRowCnt]["Disc_Per"] = Func.Convert.dConvertToDouble(txtDiscPer.Text);
        //                    // Get AccRate
        //                    TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
        //                    dtDetails.Rows[iMRowCnt]["Accept_Rate"] = Func.Convert.dConvertToDouble(txtAccRate.Text);
        //                    // Get Total
        //                    TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
        //                    dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);
        //                    // Get txtMRP_Rate
        //                    TextBox txtMRP_Rate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRP_Rate");
        //                    dtDetails.Rows[iMRowCnt]["MRP_Rate"] = Func.Convert.dConvertToDouble(txtMRP_Rate.Text);
        //                    // Get txtAssValue
        //                    TextBox txtAssValue = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAssValue");
        //                    dtDetails.Rows[iMRowCnt]["Ass_Value"] = Func.Convert.dConvertToDouble(txtAssValue.Text);
        //                    // Get Descripancy
        //                    DropDownList drpDescripancy = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpDescripancy");
        //                    dtDetails.Rows[iMRowCnt]["Descripancy_YN"] = Func.Convert.sConvertToString(drpDescripancy.SelectedValue);
        //                    // Get Shortage Qty
        //                    TextBox txtShortageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtShortageQty");
        //                    dtDetails.Rows[iMRowCnt]["Shortage_Qty"] = Func.Convert.dConvertToDouble(txtShortageQty.Text);
        //                    //Get Excess Qty
        //                    TextBox txtExcessQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExcessQty");
        //                    dtDetails.Rows[iMRowCnt]["Excess_Qty"] = Func.Convert.dConvertToDouble(txtExcessQty.Text);
        //                    //Get Damage_Qty
        //                    TextBox txtDamageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDamageQty");
        //                    dtDetails.Rows[iMRowCnt]["Damage_Qty"] = Func.Convert.dConvertToDouble(txtDamageQty.Text);
        //                    //Get Man_Defect_Qty
        //                    TextBox txtManDefectQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtManDefectQty");
        //                    dtDetails.Rows[iMRowCnt]["Man_Defect_Qty"] = Func.Convert.dConvertToDouble(txtManDefectQty.Text);

        //                    //Get Wrong_Supply_Qty
        //                    TextBox txtWrongSupplyQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrongSupplyQty");
        //                    dtDetails.Rows[iMRowCnt]["Wrong_Supply_Qty"] = Func.Convert.dConvertToDouble(txtWrongSupplyQty.Text);

        //                    // Get Descripancy
        //                    DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
        //                    dtDetails.Rows[iMRowCnt]["Retain_YN"] = Func.Convert.sConvertToString(drpRetain.SelectedValue);

        //                    // Get Wrong Part ID
        //                    TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
        //                    dtDetails.Rows[iMRowCnt]["Wrg_Part_ID"] = Func.Convert.iConvertToInt(txtWrgPartID.Text);

        //                    // Get Wrong Part No
        //                    TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
        //                    dtDetails.Rows[iMRowCnt]["Wrg_Part_No"] = Func.Convert.sConvertToString(txtWrgPartNo.Text);

        //                    //Get Wrong Part Name
        //                    TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
        //                    dtDetails.Rows[iMRowCnt]["Wrg_Part_Name"] = Func.Convert.sConvertToString(txtWrgPartName.Text);

        //                    //Get SAP Order No for with dCAN PO
        //                    //TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
        //                    dtDetails.Rows[iMRowCnt]["SAP_Order_No"] = Func.Convert.sConvertToString(txtSAPOrderNo.Text);
        //                    // Tax Per
        //                    TextBox txtTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxPer");
        //                    dtDetails.Rows[iMRowCnt]["TAX_Per"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);
        //                    // Tax AMount
        //                    TextBox txtTaxAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxAmt");
        //                    dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTaxAmt.Text);

        //                    DropDownList drpTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpTax1");
        //                    dtDetails.Rows[iMRowCnt]["Tax1_Code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);
        //                    // Tax1 Per
        //                    TextBox txtTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Per");
        //                    dtDetails.Rows[iMRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);
        //                    // Tax1 Amt
        //                    TextBox txtTax1Amt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Amt");
        //                    dtDetails.Rows[iMRowCnt]["Tax1_Amt"] = Func.Convert.dConvertToDouble(txtTax1Amt.Text);

        //                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

        //                    // Get Status                           
        //                    dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

        //                    if (txtStatus.Text == "C")
        //                    {
        //                        lblCancel.Attributes.Add("disabled", "true");
        //                        lblCancel.Text = "Cancelled";
        //                    }
        //                    //dtDetails.Rows[iRowCnt]["Status"] = "";            
        //                    if (Chk.Checked == true)
        //                    {
        //                        dtDetails.Rows[iRowCnt]["Status"] = "D";
        //                        iCntForDelete++;
        //                    }
        //                    else
        //                    {
        //                        if (txtPartID.Text != "")// && txtRecvQty.Text != "0"
        //                        {
        //                            if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "M")
        //                            {
        //                                dtDetails.Rows[iMRowCnt]["Status"] = "M";
        //                                bDetailsRecordExist = true;
        //                            }
        //                            else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "U")
        //                            {
        //                                dtDetails.Rows[iMRowCnt]["Status"] = "U";
        //                                bDetailsRecordExist = true;
        //                            }
        //                            else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
        //                            {
        //                                dtDetails.Rows[iMRowCnt]["Status"] = "E";
        //                                bDetailsRecordExist = true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
        //        {
        //            //if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D"
        //            //    && hdnIsAutoReceipt.Value == "Y" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Recv_Qty"]) == 0)
        //            //{
        //            //    iCntError = iCntError + 1;
        //            //    sQtyMsg = "Please enter Receive Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
        //            //    break;
        //            //}
        //            if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && hdnIsAutoReceipt.Value == "N" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Bill_Qty"]) == 0)
        //            {
        //                iCntError = iCntError + 1;
        //                sQtyMsg = "Please enter Bill Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
        //                break;
        //            }
        //            if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["PartTaxID"]) == 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && hdnIsAutoReceipt.Value == "N")
        //            {
        //                iCntError = iCntError + 1;
        //                sQtyMsg = "Please Select Part Tax at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
        //                break;
        //            }
        //            if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Wrong_Supply_Qty"]) > 0 && hdnIsAutoReceipt.Value == "Y" && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Wrg_Part_ID"]) == 0)
        //            {
        //                iCntError = iCntError + 1;
        //                sQtyMsg = "Please Select Wrong Part at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
        //                break;
        //            }

        //        }

        //        if (iCntForDelete == iCntForSelect)
        //        {
        //            if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
        //            bDetailsRecordExist = false;
        //        }
        //        else if (iCntError > 0)
        //        {
        //            if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
        //            bDetailsRecordExist = false;
        //        }
        //        //For Tax Details
        //        dtMRGrpTaxDetails = (DataTable)(Session["MRGrpTaxDetails"]);
        //        dtMRTaxDetails = (DataTable)(Session["MRTaxDetails"]);

        //        //if (dtDetails.Rows.Count > 1 && dtMRGrpTaxDetails.Rows.Count == 0)
        //        //    CreateNewRowToTaxGroupDetailsTable();

        //        //if (bSaveTmTxDtls == true)
        //        //{                
        //        for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
        //        {
        //            //Group Code
        //            TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

        //            //Group Name
        //            TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

        //            //Get Net Amount
        //            TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

        //            //Get Net Reverse Amount
        //            //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
        //            //dtMRGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

        //            //Get Discount Perc
        //            TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

        //            //Get Discount Amount
        //            TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

        //            // Get Tax
        //            DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

        //            //Get Tax Percentage                
        //            DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

        //            //Get Tax Amount
        //            TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

        //            // Get Tax1
        //            DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

        //            //Get Tax1 Percentage                
        //            DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

        //            //Get Tax1 Amount
        //            TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

        //            // Get Tax2
        //            DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

        //            //Get Tax2 Percentage                
        //            DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

        //            //Get Tax2 Amount
        //            TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

        //            // Get Total
        //            TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
        //            dtMRGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
        //        }
        //        //CreateNewRowToTaxGroupDetailsTable();
        //        for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
        //        {
        //            //Doc ID
        //            TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
        //            dtMRTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

        //            //Get Net Amount
        //            TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
        //            dtMRTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

        //            //Get Net Amount
        //            //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
        //            //dtMRTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

        //            //Get Discount amt
        //            TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
        //            dtMRTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

        //            //Get Amt Before Tax (with Discount)
        //            TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
        //            dtMRTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);
        //            //Get txtGrExciseAmt
        //            TextBox txtGrExciseAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrExciseAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["Excise_Amt"] = Func.Convert.dConvertToDouble(txtGrExciseAmt.Text);
        //            //Get txtGrInsuAmt
        //            TextBox txtGrInsuAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrInsuAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["Insu_Amt"] = Func.Convert.dConvertToDouble(txtGrInsuAmt.Text);
        //            // Get Tax 
        //            TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

        //            //Get Tax         
        //            TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

        //            //Get Tax1 Amount
        //            TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
        //            dtMRTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

        //            // Get Tax2 Amount
        //            TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
        //            dtMRTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

        //            //Get PF Per                 
        //            TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
        //            dtMRTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

        //            //Get PF Amount
        //            TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

        //            // Get Other Per
        //            TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
        //            dtMRTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

        //            //Get Other Amount
        //            TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
        //            dtMRTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

        //            //Get grand Total Amount
        //            TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
        //            dtMRTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}
        private void CreateNewRowToDetailsTable()
        {
            try
            {
                DataRow dr;
                if (dtDetails.Rows.Count == 0)
                {
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("PO_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Inv_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Bal_PO_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Group_Code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Bill_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Recv_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Disc_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Accept_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRP_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Ass_Value", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Tax_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PO_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Descripancy_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Shortage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Excess_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Damage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Man_Defect_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Wrong_Supply_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Retain_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_Name", typeof(String)));

                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("SAP_Order_No", typeof(string)));

                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "0")
                    {
                        goto Exit;
                    }
                }

                //if (Func.Convert.iConvertToInt(Session["iSupType"].ToString()) != 18)
                //{

                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["PO_Det_ID"] = 0;
                dr["Inv_Det_ID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Bal_PO_Qty"] = 0;
                dr["Bill_Qty"] = 0;
                dr["Recv_Qty"] = 0;
                dr["MOQ"] = 0;
                dr["MRPRate"] = 0;
                dr["Disc_Per"] = 0;
                dr["Accept_Rate"] = 0;
                dr["Total"] = 0;
                dr["MRP_Rate"] = 0;
                dr["Ass_Value"] = 0;
                dr["Status"] = "N";
                dr["Tax_Per"] = 0;
                dr["PO_No"] = "";
                dr["PartTaxID"] = 0;
                dr["MOQ"] = 0;
                dr["Descripancy_YN"] = "N";
                dr["Shortage_Qty"] = 0;
                dr["Excess_Qty"] = 0;
                dr["Damage_Qty"] = 0;
                dr["Man_Defect_Qty"] = 0;
                dr["Wrong_Supply_Qty"] = 0;
                dr["Retain_YN"] = "N";
                dr["Wrg_Part_ID"] = 0;
                dr["Wrg_Part_No"] = "";
                dr["Wrg_Part_Name"] = "";
                dr["SAP_Order_No"] = "";

                //if (Func.Convert.sConvertToString(dtDetails.Rows[0]["TaxTag"]) == "I")
                if (hdnSupTaxTag.Value == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";

                //dtDetails.Rows.Add(dr);
                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();

                //}
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
                if (Session["MRPartDetails"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["MRPartDetails"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["MRPartDetails"];
                    if (dtDetails.Rows.Count == 0)
                        CreateNewRowToDetailsTable();
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N" && hdnIsAutoReceipt.Value == "N")
                            CreateNewRowToDetailsTable();
                    }
                }
                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();
                //if (Session["MRGrpTaxDetails"] == null)
                //{

                //    Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;

                //}
                //else
                //{
                //    dtMRGrpTaxDetails = (DataTable)Session["MRGrpTaxDetails"];

                //}

                GrdPartGroup.DataSource = dtMRGrpTaxDetails;
                GrdPartGroup.DataBind();

                //if (Session["MRGrpTaxDetails"] == null)
                //{

                //    Session["MRTaxDetails"] = dtMRTaxDetails;

                //}
                //else
                //{
                //    dtMRTaxDetails = (DataTable)Session["MRTaxDetails"];

                //}

                GrdDocTaxDet.DataSource = dtMRTaxDetails;
                GrdDocTaxDet.DataBind();

                SetGridControlProperty();
                if (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                    SetGridControlPropertyTax_Auto();
                else
                    SetGridControlPropertyTax();
                if (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                    SetGridControlPropertyTaxCalculation_Auto();
                else
                    SetGridControlPropertyTaxCalculation();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ReBindDataToTax()
        {
            GrdPartGroup.DataSource = dtMRGrpTaxDetails;
            GrdPartGroup.DataBind();

            GrdDocTaxDet.DataSource = dtMRTaxDetails;
            GrdDocTaxDet.DataBind();

            if (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                SetGridControlPropertyTax_Auto();
            else
                SetGridControlPropertyTax();
            if (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                SetGridControlPropertyTaxCalculation_Auto();
            else
                SetGridControlPropertyTaxCalculation();

            //SetGridControlPropertyTax_Auto();
            //SetGridControlPropertyTaxCalculation_Auto();

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
                //Sujata 05092014

                double dPartBillQty = 0;
                string sGroupCode = "";
                double dTaxAmt = 0;
                double dTax1Amt = 0;

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = iDealerID.ToString();
                string sPartID = "", sPartName = "", cPartID = "";
                string sWrgPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {

                        //GST Relates Work Part Tax
                        PartGrid.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                        //If Invoice is OLD the Hide Tax per
                        PartGrid.HeaderRow.Cells[31].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[31].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : "");//Hide Cell
                        //If Invoice is OLD the Hide Tax Amt
                        PartGrid.HeaderRow.Cells[32].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[32].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : "");//Hide Cell
                        //If Invoice is OLD the Hide Tax1 per
                        PartGrid.HeaderRow.Cells[34].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O")) ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[34].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O")) ? "none" : "");//Hide Cell
                        //If Invoice is OLD the Hide Tax1 Amt
                        PartGrid.HeaderRow.Cells[35].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O")) ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[35].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O")) ? "none" : "");//Hide Cell

                        // Name change  Tax Per,Tax Amt,tax1_per and Tax1_amt
                        PartGrid.HeaderRow.Cells[31].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "SGST %" : "IGST %"; // Hide Header   
                        PartGrid.HeaderRow.Cells[32].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt"; // Hide Header   
                        PartGrid.HeaderRow.Cells[33].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                        PartGrid.HeaderRow.Cells[34].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                        PartGrid.HeaderRow.Cells[35].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                        // Hide delete
                        PartGrid.HeaderRow.Cells[29].Style.Add("display", (hdnIsAutoReceipt.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[29].Style.Add("display", (hdnIsAutoReceipt.Value == "Y") ? "none" : "");//Hide Cell 
                        // Show  Received Wrong part No 23
                        PartGrid.HeaderRow.Cells[27].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[27].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // Show  Received Wrong part Name 24
                        PartGrid.HeaderRow.Cells[28].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[28].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 

                        //PartGrid.Style.Add("width", (hdnIsAutoReceipt.Value == "Y") ? "180%" : "110%");
                        //PartGrid.Style.Add("width", (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "B") ? "100%" : "110%");
                        PartGrid.Style.Add("width", (hdnIsAutoReceipt.Value == "Y") ? (ExportLocation.sSupplierType == "M" ? "180%" : "110%") : "100%");

                        // MRP _ Rate
                        PartGrid.HeaderRow.Cells[13].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        //Ass Value 
                        PartGrid.HeaderRow.Cells[14].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        //PO Value 
                        PartGrid.HeaderRow.Cells[15].Style.Add("display", (ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[15].Style.Add("display", (ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // Descripancy Status 
                        PartGrid.HeaderRow.Cells[19].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[19].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        //ShortageQty
                        PartGrid.HeaderRow.Cells[20].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[20].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // ExcessQty
                        PartGrid.HeaderRow.Cells[21].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[21].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        //DamageQty 
                        PartGrid.HeaderRow.Cells[22].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[22].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // manfDefectQty
                        PartGrid.HeaderRow.Cells[23].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[23].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        //WrgSupplyQty 
                        PartGrid.HeaderRow.Cells[24].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[24].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // Retain_YV
                        PartGrid.HeaderRow.Cells[25].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[25].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell 
                        // Hide Retain YN Dated 11122017
                        PartGrid.HeaderRow.Cells[25].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "none");//Hide Cell 


                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        TextBox txtPODetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPODetID");
                        txtPODetID.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PO_Det_ID"]);
                        TextBox txtInvDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvDetID");
                        txtInvDetID.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Inv_Det_ID"]);

                        // This for Select Part or PO Items
                        LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                        lnkSelectPart.Attributes.Add("onclick", "return ShowMultiPartSearch(this,'" + sDealerId + "','" + ExportLocation.iSupplierId + "','" + ddlInvoiceType.SelectedValue + "'," + iHOBrId + ");");

                        // for Selection Wrong Part Supply
                        LinkButton lnkSelectPart1 = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart1");
                        lnkSelectPart1.Attributes.Add("onclick", "return ShowPartMaster(this,'" + ExportLocation.iSupplierId + "','" + 'Y' + "');");

                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        sPartId = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        if (sPartId != "0")
                        {
                            //   hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + sPartId;
                            hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + txtPODetID.Text + '-' + sPartId;
                        }

                        sWrgPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_ID"]);
                        sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                        TextBox txtWPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");
                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        TextBox txtbalPOQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtbalPOQty");
                        TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
                        TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                        TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                        TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                        TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                        TextBox txtAccRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAccRate");
                        TextBox txtGTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                        TextBox txtPONo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPONo");
                        TextBox txtTaxPer1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxPer1");
                        TextBox txtTaxAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTaxAmt");
                        TextBox txtTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Per");
                        TextBox txtTax1Amt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTax1Amt");

                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();

                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        if (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B")//--------------- For Manual Receipt Only
                        {
                            if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                                Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID=" + iDealerID + ")" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                            //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + iDealerID + "')" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                            else
                                Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID=" + iDealerID + ")  " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                            //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 ");
                        }
                        else
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + hdnDealerID.Value + "')");
                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID=" + iDealerID + ") " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + iDealerID + "')");
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);
                        if (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B")
                        {
                            txtTaxPer1.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Tax_per"]), 2));
                        }

                        TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                        TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                        // MRP rate and Assessble Value
                        TextBox txtMRP_Rate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRP_Rate");
                        TextBox txtAssValue = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtAssValue");

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
                            // Tax1_Code
                            DropDownList drpTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpTax1");
                            Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " and ID=" + DrpPartTax1.SelectedItem);

                            if (DrpPartTax1Per.Items.Count == 2)
                            {
                                DrpPartTax1Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax1.SelectedItem);
                                drpTax1.SelectedValue = Func.Convert.sConvertToString(DrpPartTax1.SelectedItem);
                            }
                            txtPartTax1Per.Text = Func.Convert.sConvertToString(DrpPartTax1Per.SelectedItem);
                            if (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "N")
                                txtTax1Per.Text = Func.Convert.sConvertToString(DrpPartTax1Per.SelectedItem);
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

                        DropDownList drpDescripancy = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpDescripancy");
                        TextBox txtShortageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtShortageQty");
                        TextBox txtExcessQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExcessQty");
                        TextBox txtDamageQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDamageQty");
                        TextBox txtManDefectQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtManDefectQty");
                        TextBox txtWrongSupplyQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrongSupplyQty");
                        DropDownList drpRetain = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpRetain");
                        TextBox txtWrgPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartNo");
                        TextBox txtWrgPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartID");
                        TextBox txtWrgPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtWrgPartName");


                        drpRetain.Enabled = false;
                        txtWrgPartNo.Style.Add("display", "none");
                        txtWrgPartName.Style.Add("display", "none");

                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                        Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                        if (sWrgPartID != "0")
                        {
                            drpRetain.Enabled = true;
                            txtWrgPartNo.Style.Add("display", "");
                            txtWrgPartName.Style.Add("display", "");
                        }

                        if (ExportLocation.bDistributor == "N" && ddlInvoiceType.SelectedValue == "Y")
                            drpPartTax.Enabled = false;

                        if (sPartID == "0" && (ddlInvoiceType.SelectedValue == "N" || ddlInvoiceType.SelectedValue == "0" || ddlInvoiceType.SelectedValue == "Y"))
                        {
                            txtPartNo.Style.Add("display", "none");
                            lnkSelectPart.Style.Add("display", "");
                            Chk.Style.Add("display", "none");
                            lblDelete.Style.Add("display", "none");
                            txtGPartName.Style.Add("display", "none");
                            txtbalPOQty.Style.Add("display", "none");
                            txtBillQty.Style.Add("display", "none");
                            txtRecvQty.Style.Add("display", "none");
                            txtUnit.Style.Add("display", "none");
                            txtPrice.Style.Add("display", "none");
                            txtMRPRate.Style.Add("display", "none");
                            txtDiscPer.Style.Add("display", "none");
                            txtAccRate.Style.Add("display", "none");
                            txtGTotal.Style.Add("display", "none");
                            txtPONo.Style.Add("display", "none");
                            drpPartTax.Style.Add("display", "none");
                            //
                            txtMRP_Rate.Style.Add("display", "none");
                            txtAssValue.Style.Add("display", "none");
                            drpDescripancy.Style.Add("display", "none");
                            txtShortageQty.Style.Add("display", "none");
                            txtExcessQty.Style.Add("display", "none");
                            txtDamageQty.Style.Add("display", "none");
                            txtManDefectQty.Style.Add("display", "none");
                            txtWrongSupplyQty.Style.Add("display", "none");
                            drpRetain.Style.Add("display", "none");
                            txtTaxPer1.Style.Add("display", "none");
                            txtTaxAmt.Style.Add("display", "none");
                            txtTax1Per.Style.Add("display", "none");
                            txtTax1Amt.Style.Add("display", "none");

                        }
                        else
                            lnkSelectPart.Style.Add("display", "none");
                        if (sWrgPartID == "0")
                        {
                            lnkSelectPart1.Style.Add("display", "none");
                            //lnkSelectPart1.Style.Add("display", "");
                            txtWrgPartID.Style.Add("display", "none");
                        }
                        else
                        {
                            lnkSelectPart1.Style.Add("display", "none");
                            txtWrgPartID.Style.Add("display", "");
                        }

                        if (txtGPartName.Text == "" && (ddlInvoiceType.SelectedValue == "N" || ddlInvoiceType.SelectedValue == "0" || ddlInvoiceType.SelectedValue == "Y"))
                        {
                            if (ddlInvoiceType.SelectedValue == "Y")
                                lnkSelectPart.Style.Add("display", "none");
                            else
                            {
                                lnkSelectPart.Style.Add("display", "");
                                txtPartNo.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            lnkSelectPart.Style.Add("display", "none");
                            txtPartNo.Style.Add("display", "");
                        }
                        if (txtWPartName.Text == "" && ddlInvoiceType.SelectedValue == "Y")
                        {
                            lnkSelectPart1.Style.Add("display", "none");
                            //lnkSelectPart1.Style.Add("display", "");
                            txtWrgPartNo.Style.Add("display", "none");
                        }
                        else
                        {
                            txtWrgPartNo.Style.Add("display", "");
                            txtWrgPartName.Style.Add("display", "");
                            lnkSelectPart1.Style.Add("display", "none");
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
                            dTaxAmt = 0;
                            dTax1Amt = 0;

                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Recv_Qty"]);
                            dPartBillQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Bill_Qty"]);
                            //Reverse Rate Calulation for Local Part and Lubricant
                            //if (ddlInvoiceType.SelectedValue == "N" && sGroupCode != "02")
                            if (ddlInvoiceType.SelectedValue == "N" && sGroupCode == "02")
                            {
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Accept_Rate"]);
                                //clsDB objDB = new clsDB();
                                //dPartMrpPrice = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);
                                //dRevPartRate = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", dPartMrpPrice, Func.Convert.iConvertToInt(drpPartTax.SelectedValue));

                                //// Set Reverse Calculated Rate to Rate Column
                                //txtMRPRate.Text = Func.Convert.sConvertToString(dRevPartRate);
                                //dPartRate = dRevPartRate;
                                //dDiscPer = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["disc_per"]);
                                //dDiscRate = Func.Convert.dConvertToDouble(dPartRate * (dDiscPer / 100));
                                //dPartRate = Func.Convert.dConvertToDouble(dPartRate - dDiscRate);
                                //txtAccRate.Text = Func.Convert.sConvertToString(dPartRate);
                            }
                            else
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Accept_Rate"]);


                            //if (ddlInvoiceType.SelectedValue == "Y")
                            //dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);
                            //else
                            //if (ddlInvoiceType.SelectedValue == "N")
                            dPartTotal = Func.Convert.dConvertToDouble(dPartBillQty * dPartRate);

                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);
                            if (hdnIsAutoReceipt.Value == "N" && hdnIsDocGST.Value == "Y" || ExportLocation.sSupplierType == "B")
                            {

                                dTaxAmt = dPartTotal * (Func.Convert.dConvertToDouble(txtTaxPer1.Text) / 100);
                                txtTaxAmt.Text = Func.Convert.sConvertToString(Math.Round(dTaxAmt, 2));
                                dtDetails.Rows[iRowCnt]["Tax_Amt"] = txtTaxAmt.Text;

                                dTax1Amt = dPartTotal * (Func.Convert.dConvertToDouble(txtTax1Per.Text) / 100);
                                txtTax1Amt.Text = Func.Convert.sConvertToString(Math.Round(dTax1Amt, 2));
                                dtDetails.Rows[iRowCnt]["Tax1_Amt"] = txtTax1Amt.Text;
                            }


                            //Status
                            sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                            if (sRecordStatus != "C")
                            {
                                if (ddlInvoiceType.SelectedValue == "Y")
                                    dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Recv_Qty"]);
                                else
                                    dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Bill_Qty"]);

                                dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                                //
                                txtExclDiscountRate.Text = Func.Convert.sConvertToString(dExclPartDiscRate);
                                dExclPartTotal = dPartTotal;
                                TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                                dExclTotal = dExclTotal + dExclPartTotal;

                                //Vikran Begin _03052017
                                //if (ddlInvoiceType.SelectedValue == "N" && sGroupCode == "02")
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
                                //END
                            }

                            //txtExcessQty.Text =Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Excess_Qty"]),2));
                            //txtWrongSupplyQty.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrong_Supply_Qty"]);
                            drpDescripancy.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Descripancy_YN"]);
                            drpRetain.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Retain_YN"]);
                            txtWrgPartID.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_ID"]);
                            txtWrgPartNo.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_No"]);
                            txtWrgPartName.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Wrg_Part_Name"]);

                            if (Func.Convert.iConvertToInt(txtExcessQty.Text) > 0 || Func.Convert.iConvertToInt(txtWrongSupplyQty.Text) > 0)
                            {
                                txtExcessQty.Attributes.Add("onblur", "return CalculateReceiptPartTotal(event,this);");
                                txtWrongSupplyQty.Attributes.Add("onblur", "return CalculateReceiptPartTotal(event,this);");
                                drpRetain.Enabled = true;
                            }
                            //if (Func.Convert.iConvertToInt(txtWrongSupplyQty.Text) == 0)
                            //{
                            //    dtDetails.Rows[iRowCnt]["Wrg_Part_ID"] = "0";
                            //    dtDetails.Rows[iRowCnt]["Wrg_Part_Name"] = "";
                            //}

                            //TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                            //TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");

                            //TextBox txtBillQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBillQty");
                            //TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                            if (ddlInvoiceType.SelectedValue == "N" && sGroupCode == "99")
                                txtMRPRate.Enabled = true;
                            else
                                txtMRPRate.Enabled = false;
                            if ((ddlInvoiceType.SelectedValue == "N" && sGroupCode == "02") || (ddlInvoiceType.SelectedValue == "Y"))
                                drpPartTax.Enabled = false;
                            else
                                drpPartTax.Enabled = true;
                            if (ddlInvoiceType.SelectedValue == "N")
                            {
                                //txtMRPRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this);");
                                txtDiscPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                                txtDiscPer.Attributes.Add("onblur", "return CalculateReceiptPartTotal(event,this);");
                                txtMRPRate.Attributes.Add("onblur", "return CalculateReceiptPartTotal(event,this);");

                                txtBillQty.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                                txtBillQty.Attributes.Add("onblur", "return CalculateReceiptPartTotal(event,this);");
                                txtRecvQty.Attributes.Remove("onblur");
                            }
                            else
                            {
                                txtMRPRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                                txtDiscPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");

                                txtBillQty.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            }

                            if (ExportLocation.bDistributor == "N" && ddlInvoiceType.SelectedValue == "Y")
                            {
                                TextBox txtPartTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                txtPartTotal.Text = Func.Convert.sConvertToString(dPartTotal);
                            }

                        }
                    }
                }
                // For Hiding Columns
                //for (int iRowCnt1 = 0; iRowCnt1 < PartGrid.Rows.Count; iRowCnt1++)
                //{
                //    if (iRowCnt1 != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                //    {
                //        if (hdnIsAutoReceipt.Value == "Y")
                //        {
                //            PartGrid.Style.Add("width", "180%");
                //            // Hide  Delete and cancel 25
                //            PartGrid.HeaderRow.Cells[29].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[29].Style.Add("display", "none");//Hide Cell 

                //            // Show  Received Wrong part No 23
                //            PartGrid.HeaderRow.Cells[27].Style.Add("display", ""); // Show Header        
                //            PartGrid.Rows[iRowCnt1].Cells[27].Style.Add("display", "");//Show Cell 

                //            // Show  Received Wrong part Name 24
                //            PartGrid.HeaderRow.Cells[28].Style.Add("display", ""); // Show Header        
                //            PartGrid.Rows[iRowCnt1].Cells[28].Style.Add("display", "");//Show Cell 
                //        }
                //        else
                //        {
                //            PartGrid.Style.Add("width", "100%");
                //            // Hide MRP _ Rate 11
                //            PartGrid.HeaderRow.Cells[13].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[13].Style.Add("display", "none");//Hide Cell 
                //            // Hide Ass Value 12
                //            PartGrid.HeaderRow.Cells[14].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[14].Style.Add("display", "none");//Hide Cell 
                //            // Hide Descripancy Status 15
                //            PartGrid.HeaderRow.Cells[19].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[19].Style.Add("display", "none");//Hide Cell 
                //            // Hide ShortageQty 16
                //            PartGrid.HeaderRow.Cells[20].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[20].Style.Add("display", "none");//Hide Cell 
                //            // Hide ExcessQty 17
                //            PartGrid.HeaderRow.Cells[21].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[21].Style.Add("display", "none");//Hide Cell 
                //            // Hide DamageQty 18
                //            PartGrid.HeaderRow.Cells[22].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[22].Style.Add("display", "none");//Hide Cell 
                //            // Hide manfDefectQty 19
                //            PartGrid.HeaderRow.Cells[23].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[23].Style.Add("display", "none");//Hide Cell 
                //            // Hide WrgSupplyQty 20
                //            PartGrid.HeaderRow.Cells[24].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[24].Style.Add("display", "none");//Hide Cell 
                //            // Hide Retain_YV 21
                //            PartGrid.HeaderRow.Cells[25].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[25].Style.Add("display", "none");//Hide Cell 
                //            // Hide  Received Wrong part No 23
                //            PartGrid.HeaderRow.Cells[27].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[27].Style.Add("display", "none");//Hide Cell 
                //            // Hide  Received Wrong part Name 24
                //            PartGrid.HeaderRow.Cells[28].Style.Add("display", "none"); // Hide Header        
                //            PartGrid.Rows[iRowCnt1].Cells[28].Style.Add("display", "none");//Hide Cell 

                //            // Show  Delete and cancel 25
                //            PartGrid.HeaderRow.Cells[29].Style.Add("display", ""); // Show Header        
                //            PartGrid.Rows[iRowCnt1].Cells[29].Style.Add("display", "");//Show Cell 
                //        }
                //    }
                //}
                if (hdnIsAutoReceipt.Value == "Y" && ExportLocation.sSupplierType == "M")
                    txtTotal.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Net_Tr_Amt"]), 2));
                else
                    txtTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void SetGridControlPropertyTax_Auto()
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

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (hdnIsAutoReceipt.Value == "Y")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + "And ( DealerID='" + iDealerID + "') and Tax_Type= 1 and isnull(Is_Service_Tax,'N') ='N' ");
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, "And ( DealerID='" + iDealerID + "') and Tax_Type= 1 and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 ");
                    }

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (hdnIsAutoReceipt.Value == "Y")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");
                    }


                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, "");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);

                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_amt"]);


                    //drpTax.Attributes.Add("onBlur", "SetMainTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");

                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");


                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();
                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
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
                        if (hdnIsAutoReceipt.Value == "N" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Amt")
                        {
                            txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountAmt.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");
                        }
                        else
                        {
                            txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountPer.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");
                        }
                    }

                    //txtGrDiscountPer.Text = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["discount_per"]);
                    //txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["discount_amt"]);

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (hdnIsAutoReceipt.Value == "Y")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + "And ( DealerID='" + iDealerID + "') and Tax_Type= 1 and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and Tax_Type= 1 ");
                    }

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (hdnIsAutoReceipt.Value == "Y")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "') and isnull(Is_Service_Tax,'N') ='N' ");
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");
                    }


                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, " And ( DealerID='" + iDealerID + "')");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

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


                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();
                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + iDealerID + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
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
        private void SetGridControlPropertyTaxCalculation_Auto()
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

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                //for (int i = 0; i < PartGrid.Rows.Count; i++)
                //{
                //    //Sujata
                //    //TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                //    TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("TxtExclTotal");
                //    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGroupCode");
                //    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");
                //    TextBox txtStatus = (TextBox)PartGrid.Rows[i].FindControl("txtStatus");

                //    if (txtGrNo.Text.Trim() != "" && txtStatus.Text != "D" && txtStatus.Text != "C") TotalOA = TotalOA + Func.Convert.dConvertToDouble(txtTotal.Text);

                //    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                //    {
                //        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                //        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                //        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                //        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtStatus.Text != "D" && txtStatus.Text != "C" && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == drpPartTax.SelectedValue && drpTax.SelectedIndex != 0)
                //        {
                //            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
                //        }
                //    }
                //}

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRGrpTaxDetails.Rows[iRowCnt]["Net_Inv_Amt"]));

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

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");

                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    if (hdnIsAutoReceipt.Value == "N")
                        dGrpMTaxAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100));
                    else
                        dGrpMTaxAmt = Func.Convert.dConvertToDouble(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_amt"]);

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
                    if (hdnIsAutoReceipt.Value == "N")
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    else
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(dtMRGrpTaxDetails.Rows[iRowCnt]["tax1_amt"]);

                    dDocTax1Amt = Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Func.Convert.dConvertToDouble(txtTax2Per.Text);
                    dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    dDocTax2Amt = Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    GrdDocTaxDet.HeaderRow.Cells[3].Text = (hdnIsAutoReceipt.Value == "Y") ? "Ass Amt" : "Discount";
                    // Hide  Discount Amt
                    GrdDocTaxDet.HeaderRow.Cells[3].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[3].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y") ? "none" : "");//Hide Cell
                    // Hide  Exice Amt
                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "Y") ? "none" : "");//Hide Cell
                    // Hide  Insurance Amt
                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && hdnIsAutoReceipt.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");

                    TextBox txtGrExciseAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrExciseAmt");
                    TextBox txtGrInsuAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrInsuAmt");

                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Net_Tr_Amt"]));
                    txtDocDisc.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["discount_amt"])));
                    txtGrExciseAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Excise_Amt"]));
                    txtGrInsuAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Insu_Amt"]));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["mst_amt"]));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["cst_amt"]));

                    txtTax1.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["surcharge_amt"]));
                    txtTax2.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["tot_amt"]));

                    txtPFPer.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["pf_per"]));
                    txtPFAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["pf_amt"]));
                    txtOtherPer.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["other_per"]));
                    txtOtherAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["other_money"]));
                    txtGrandTot.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Total"]));

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

                    if (txtGrNo.Text.Trim() != "" && txtStatus.Text != "D" && txtStatus.Text != "C")
                    {
                        TotalOA = TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text);
                        TotalRev = Math.Round(TotalRev + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtStatus.Text != "D" && txtStatus.Text != "C" && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == drpPartTax.SelectedValue && drpTax.SelectedIndex != 0)
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
                    if (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B")
                        dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    else
                        dGrpMTaxAmt = Func.Convert.dConvertToDouble(dtMRGrpTaxDetails.Rows[iRowCnt]["tax_amt"]);

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
                    //dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    //double sum = 0;
                    if (sTax1ApplOn == "1")
                    {
                        //Vikram Begin 23052017
                        //double DiscNetAmt = 0;
                        //DiscNetAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt)), 2);
                        //DiscNetAmt = DiscNetAmt + dGrpMTaxAmt;
                        //sum = DiscNetAmt / (1 + (Func.Convert.dConvertToDouble(dGrpTax1Per / 100)));
                        //dGrpTax1Amt = Math.Round(sum * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                        //END
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

                    dDocTax1Amt = Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Func.Convert.dConvertToDouble(txtTax2Per.Text);
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
                    GrdDocTaxDet.HeaderRow.Cells[3].Text = (hdnIsAutoReceipt.Value == "Y") ? "Ass Amt" : "Discount";
                    // Hide  Discount Amt
                    GrdDocTaxDet.HeaderRow.Cells[3].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[3].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || hdnIsAutoReceipt.Value == "Y" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell
                    // Hide  Exice Amt
                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell
                    // Hide  Insurance Amt
                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsAutoReceipt.Value == "N" || ExportLocation.sSupplierType == "B") ? "none" : "");//Hide Cell


                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnSupTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END

                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocRevTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");

                    TextBox txtGrExciseAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrExciseAmt");
                    TextBox txtGrInsuAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrInsuAmt");

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

                    if (hdnIsAutoReceipt.Value == "N")
                        txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));
                    else
                    {
                        txtDocDisc.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["discount_amt"]), 2));
                        txtGrExciseAmt.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Excise_Amt"]), 2));
                        txtGrInsuAmt.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["Insu_Amt"]), 2));
                    }

                    //if (i != Func.Convert.iConvertToInt(GrdDocTaxDet.Rows.Count))
                    //{
                    //    if (hdnIsAutoReceipt.Value == "Y")
                    //    {
                    //        // Hide  Exice Amt
                    //        GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", ""); // Hide Header        
                    //        GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", "");//Hide Cell 
                    //        // Show  Insu Amt
                    //        GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", ""); // Show Header        
                    //        GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", "");//Show Cell 
                    //    }
                    //    else
                    //    {
                    //        // Hide  Exice Amt
                    //        GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", "none"); // Hide Header        
                    //        GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", "none");//Hide Cell 
                    //        // Show  Insu Amt
                    //        GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", "none"); // Show Header        
                    //        GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", "none");//Show Cell 
                    //    }
                    //}
                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    double dDocPFPer = 0;
                    double dDocPFAmt = 0;
                    double dDocOtherPer = 0;
                    double dDocOtherAmt = 0;
                    double dDocGrandAmt = 0;
                    double dDocExciseAmt = 0;
                    double dDocInsuAmt = 0;
                    // When Discount changes then First set per and amt to zero
                    if (sDiscChange == true)
                    {
                        txtPFPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtPFAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                        txtOtherPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtOtherAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                    }
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per" && (hdnIsAutoReceipt.Value == "Y" || hdnIsAutoReceipt.Value == "N"))
                        {
                            txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFPer.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");
                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);//PF
                            dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                            //txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));
                        }
                        else
                        {
                            txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFAmt.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");
                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(txtPFAmt.Text);
                            //txtPFPer.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["pf_per"]), 2));
                            //txtPFAmt.Text = Func.Convert.sConvertToString(Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["pf_amt"]), 2));
                            //dDocPFAmt = Func.Convert.dConvertToDouble(txtPFAmt.Text);
                        }
                    }


                    dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt));
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per" && (hdnIsAutoReceipt.Value == "Y" || hdnIsAutoReceipt.Value == "N"))
                        {
                            txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherPer.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            dDocOtherAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100));
                        }
                        else
                        {
                            txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherAmt.Attributes.Add("onblur", "return CalulateReceivePartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            //dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(dtMRTaxDetails.Rows[0]["other_money"]), 2);
                            dDocOtherAmt = Func.Convert.dConvertToDouble(txtOtherAmt.Text);
                        }
                    }
                    //txtOtherAmt.Text = Func.Convert.sConvertToString(dDocOtherAmt.ToString("0.00"));

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), (hdnIsAutoReceipt.Value == "N" && hdnIsRoundOFF.Value == "Y") ? 0 : 2);

                    if (hdnIsAutoReceipt.Value == "N")
                        txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));
                    else
                    {
                        dDocExciseAmt = Func.Convert.dConvertToDouble(txtGrExciseAmt.Text);
                        dDocInsuAmt = Func.Convert.dConvertToDouble(txtGrInsuAmt.Text);
                        dDocGrandAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocExciseAmt) + Func.Convert.dConvertToDouble(dDocInsuAmt)), 2);
                        txtGrandTot.Text = Func.Convert.sConvertToString(dDocGrandAmt.ToString("0.00"));
                    }

                }
                //for (int iColCnt = 0; iColCnt < GrdDocTaxDet.Columns.Count; iColCnt++)
                //{
                //    if (hdnIsAutoReceipt.Value == "Y")
                //    {
                //        GrdDocTaxDet.Columns[3].HeaderText = " Ass Amt";
                //        break;
                //    }
                //    else
                //    {
                //        GrdDocTaxDet.Columns[3].HeaderText = "Discount";
                //        break;
                //    }
                //}
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
                    Session["MRPartDetails"] = dtDetails;
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
                dtDetails = (DataTable)Session["MRPartDetails"];
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

            //Temporary
            CreateNewRowToTaxGroupDetailsTable();
            Session["MRPartDetails"] = dtDetails;
            Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
            Session["MRTaxDetails"] = dtMRTaxDetails;
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
                dtMRGrpTaxDetails = (DataTable)Session["MRGrpTaxDetails"];

                Boolean bDtSelPartRow = false;
                dtMRGrpTaxDetails.Clear();
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
                    for (int iRCnt = 0; iRCnt < dtMRGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtMRGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtMRGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0)
                    {
                        dr = dtMRGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        if (sGrCode.Trim() == "01")
                            dr["Gr_Name"] = "Parts";
                        else if (sGrCode.Trim() == "02")
                            dr["Gr_Name"] = "Lubricant";
                        else
                            dr["Gr_Name"] = "Local Part";
                        //dr["Gr_Name"] = (sGrCode.Trim() == "02" || sGrCode.Trim() == "99") ? "Lubricant" : "Local Part";

                        dr["net_inv_amt"] = 0.00;
                        //dr["net_rev_amt"] = 0.00;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0.00;

                        dr["Tax_Code"] = iPartTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iPartTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iPartTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtMRGrpTaxDetails.Rows.Add(dr);
                        dtMRTaxDetails.AcceptChanges();
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
            if (ExportLocation.sSupplierType == "B")
                DisplayInvoiceRecord();
            else
                ClearConrol();
            txtDeliveryNo.Text = "";
        }


        protected void ddlReceiptType_SelectedIndexChanged(object sender, EventArgs e)
        {

            //    ClearConrol();
            //    if ((ExportLocation.bDistributor == "Y" || ExportLocation.bDistributor == "N") && ddlReceiptType.SelectedValue == "Y")
            //    {
            //        ddlInvoiceType.SelectedValue = "Y";
            //        ddlInvoiceType.Enabled = false;            
            //    }
            //    else if (ExportLocation.bDistributor == "N" && ddlReceiptType.SelectedValue == "N")
            //    {
            //        ddlInvoiceType.SelectedValue = "N";
            //        ddlInvoiceType.Enabled = false;
            //        UploafFile.Style.Add("display", "");
            //        DisplayPreviousRecord();
            //    }
            //    else
            //    {
            //        ddlInvoiceType.Enabled = true;            
            //    }

            //    if (ExportLocation.bDistributor == "Y" && (ddlReceiptType.SelectedValue == "Y" || ddlReceiptType.SelectedValue == "N") && ddlInvoiceType.SelectedValue == "Y")
            //    {
            //        ddlInvoice.Style.Add("display", "");
            //        txtDMSInvNo.Style.Add("display", "none");
            //        FillCombo();
            //    }
            //    else
            //    {
            //        ddlInvoice.Style.Add("display", "none");
            //        txtDMSInvNo.Style.Add("display", "");
            //    }
            //    //hdnIsWithPO.Value = ddlReceiptType.SelectedValue;
            //    hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;


        }
        protected void ddlInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnIsAutoReceipt.Value = ddlInvoiceType.SelectedValue;
            //if (ExportLocation.bDistributor == "Y" && (ddlReceiptType.SelectedValue == "Y" || ddlReceiptType.SelectedValue == "N") && ddlInvoiceType.SelectedValue == "Y")
            if (ExportLocation.bDistributor == "Y" && ddlInvoiceType.SelectedValue == "Y")
            {
                ddlInvoice.Style.Add("display", "");
                txtDMSInvNo.Style.Add("display", "none");
                FillCombo();
            }
            else
            {
                ddlInvoice.Style.Add("display", "none");
                txtDMSInvNo.Style.Add("display", "");
            }
            if (ddlInvoiceType.SelectedValue == "N" || (ddlInvoiceType.SelectedValue == "Y" && ExportLocation.bDistributor == "N"))
            {
                txtDMSInvNo.ReadOnly = false;
                txtDMSInvDate.ReadOnly = false;
            }
            else
            {
                txtDMSInvNo.ReadOnly = true;
                txtDMSInvDate.ReadOnly = true;
            }
            ClearConrol();

            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

            //if (ddlReceiptType.SelectedValue == "N" && ddlInvoiceType.SelectedValue == "N")
            if (ddlInvoiceType.SelectedValue == "N")
            {
                DisplayPreviousRecord();
            }
            else
            {
            }
        }

        protected void lnkSelectPart1_Click(object sender, EventArgs e)
        {
            try
            {
                //DisplayData();
                //ModalPopupExtender1.Show();
                FillDetailsFromGrid(false);
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private int i = 1;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                string txtInvDetID = (e.Row.FindControl("txtInvDetID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtPartID != "0")
                {
                    //if (hdnIsAutoReceipt.Value == "Y" && hdnIsDocGST.Value == "Y")
                    //{
                    //    lblNo.Text = txtInvDetID.ToString();
                    //}
                    //else
                    //{
                    lblNo.Text = i.ToString();
                    i++;
                    //}

                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }

        protected void txtDeliveryNo_TextChanged(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            try
            {
                if (hdnIsAutoReceipt.Value == "Y")
                    DisplayInvoiceRecord();

                foreach (GridViewRow gr in PartGrid.Rows)
                {
                    TextBox MrpRate = (TextBox)gr.FindControl("txtMRPRate");
                    if (ddlInvoiceType.SelectedValue == 'N'.ToString())
                    { MrpRate.Enabled = true; }
                    else { MrpRate.Enabled = false; }
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (ds != null) ds = null;
                if (objDB != null) objDB = null;
            }

        }

        #region Code For Selection of Local Part PopUp related

        private void BindDataAndDisplay()
        {
            try
            {
                int idataCount = 0;
                string[] asd;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                dtDetails = (DataTable)Session["MRPartDetails"];
                idataCount = dtDetails.Rows.Count;

                dtTemp = dtDetails.Clone();
                dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                //dtDetails.Rows.RemoveAt(idataCount - 1);

                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    //SRNo,ID,Part_ID,PO_Det_ID,part_no,group_code,Part_Name,Bill_Qty,MRPRate,Recv_Qty,Bal_PO_Qty,
                    //Accept_Rate,Disc_Per,Total,Status,Tax_Per,PO_No,Qty,PartTaxID,MOQ,
                    //Descripancy_YN,Shortage_Qty,Excess_Qty,Damage_Qty,Man_Defect_Qty,Wrong_Supply_Qty,Retain_YN,Wrg_Part_ID,Wrg_Part_No,Wrg_Part_Name

                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));

                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("PO_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Bill_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Recv_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Bal_PO_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Accept_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Disc_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRP_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Ass_Value", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Tax_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PO_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));

                    dtDetails.Columns.Add(new DataColumn("Descripancy_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Shortage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Excess_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Damage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Man_Defect_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Wrong_Supply_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Retain_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("SAP_Order_No", typeof(String)));
                }
                if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds.Text);

                    // string[] asd = myString.ToString().Split('#');
                    asd = myString.ToString().Split('#');
                    int iRowCnt = 0;
                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        dr = dtDetails.NewRow();
                        //SRNo,ID,Part_ID,part_no,Part_Name,Qty,group_code,bal_qty,MRPRate,discount_per,disc_rate,discount_amt,Total,Status
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["PO_Det_ID"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["PO_No"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[5]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[6]);
                        dr["group_code"] = Func.Convert.sConvertToString(myArray[7]);

                        dr["Bill_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                        dr["Recv_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                        dr["Bal_PO_Qty"] = 0.00;
                        dr["Unit"] = Func.Convert.sConvertToString(myArray[25].Trim());
                        dr["Price"] = Func.Convert.dConvertToDouble(myArray[26]);
                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["Accept_Rate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["Descripancy_YN"] = "N";
                        dr["Shortage_Qty"] = 0.00;
                        dr["Excess_Qty"] = 0.00;
                        dr["Damage_Qty"] = 0.00;
                        dr["Man_Defect_Qty"] = 0.00;
                        dr["Wrong_Supply_Qty"] = 0.00;
                        dr["Retain_YN"] = "N";
                        dr["Wrg_Part_ID"] = 0;
                        dr["Wrg_Part_No"] = "";
                        dr["Wrg_Part_Name"] = "";
                        dr["disc_per"] = Func.Convert.dConvertToDouble(myArray[12]);
                        //dr["discount_amt"] = Convert.ToDouble(myArray[13]);
                        dr["Total"] = Func.Convert.dConvertToDouble(myArray[15]);
                        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[10]);
                        dr["Status"] = "U";
                        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[24].Trim());
                        dr["MRP_Rate"] = 0.00;
                        dr["Ass_Value"] = 0.00;
                        dr["SAP_Order_No"] = "";
                        dr["Tax_Per"] = Func.Convert.dConvertToDouble(myArray[27]);

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                if (dtDetails.Rows.Count == 0 && dtTemp.Rows.Count != 0)
                    dtDetails.ImportRow(dtTemp.Rows[0]);

                Session["MRPartDetails"] = dtDetails;

                // New Code

                FillDetailsFromGrid(false);
                BindDataToGrid();
                FillDetailsFromGrid(false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["MRPartDetails"] = dtDetails;
                Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
                Session["MRTaxDetails"] = dtMRTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                //int idataCount = 0;
                //string[] asd;
                //DataTable dtDetails = new DataTable();
                //DataTable dtTemp = new DataTable();
                //dtDetails = (DataTable)Session["MRPartDetails"];
                //idataCount = dtDetails.Rows.Count;


                //dtTemp = dtDetails.Clone();
                //dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                ////dtDetails.Rows.RemoveAt(idataCount - 1);



                ////DataTable dt = new DataTable();
                ////dt = dsSrchgrid.Tables[0].Clone();

                //DataRow dr;
                //if (dtDetails.Columns.Count == 0)
                //{
                //    //SRNo,ID,Part_ID,PO_Det_ID,part_no,group_code,Part_Name,Bill_Qty,MRPRate,Recv_Qty,Bal_PO_Qty,
                //    //Accept_Rate,Disc_Per,Total,Status,Tax_Per,PO_No,Qty,PartTaxID,MOQ,
                //    //Descripancy_YN,Shortage_Qty,Excess_Qty,Damage_Qty,Man_Defect_Qty,Wrong_Supply_Qty,Retain_YN,Wrg_Part_ID,Wrg_Part_No,Wrg_Part_Name

                //    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));

                //    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                //    dtDetails.Columns.Add(new DataColumn("PO_Det_ID", typeof(int)));
                //    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Bill_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Recv_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Bal_PO_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Accept_Rate", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Disc_Per", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("MRP_Rate", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Ass_Value", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Tax_Per", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("PO_No", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                //    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));

                //    dtDetails.Columns.Add(new DataColumn("Descripancy_YN", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Shortage_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Excess_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Damage_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Man_Defect_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Wrong_Supply_Qty", typeof(double)));
                //    dtDetails.Columns.Add(new DataColumn("Retain_YN", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Wrg_Part_ID", typeof(int)));
                //    dtDetails.Columns.Add(new DataColumn("Wrg_Part_No", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("Wrg_Part_Name", typeof(String)));
                //    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
                //}
                //if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
                //{
                //    string myString = Func.Convert.sConvertToString(txtPartIds.Text);

                //    // string[] asd = myString.ToString().Split('#');
                //    asd = myString.ToString().Split('#');
                //    int iRowCnt = 0;
                //    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                //    {
                //        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                //        string[] delim = { "<--" };
                //        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                //        dr = dtDetails.NewRow();
                //        //SRNo,ID,Part_ID,part_no,Part_Name,Qty,group_code,bal_qty,MRPRate,discount_per,disc_rate,discount_amt,Total,Status
                //        dr["SRNo"] = iRowCnt;
                //        dr["ID"] = 0;
                //        dr["PO_Det_ID"] = Func.Convert.sConvertToString(myArray[2]);
                //        dr["PO_No"] = Func.Convert.sConvertToString(myArray[3]);
                //        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                //        dr["part_no"] = Func.Convert.sConvertToString(myArray[5]);
                //        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[6]);
                //        dr["group_code"] = Func.Convert.sConvertToString(myArray[7]);

                //        dr["Bill_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                //        dr["Recv_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                //        dr["Bal_PO_Qty"] = 0.00;
                //        dr["Unit"] = Func.Convert.sConvertToString(myArray[25]);
                //        dr["Price"] = Func.Convert.dConvertToDouble(myArray[26]);
                //        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);
                //        dr["Accept_Rate"] = Func.Convert.dConvertToDouble(myArray[1]);
                //        dr["Descripancy_YN"] = "N";
                //        dr["Shortage_Qty"] = 0.00;
                //        dr["Excess_Qty"] = 0.00;
                //        dr["Damage_Qty"] = 0.00;
                //        dr["Man_Defect_Qty"] = 0.00;
                //        dr["Wrong_Supply_Qty"] = 0.00;
                //        dr["Retain_YN"] = "N";
                //        dr["Wrg_Part_ID"] = 0;
                //        dr["Wrg_Part_No"] = "";
                //        dr["Wrg_Part_Name"] = "";
                //        dr["disc_per"] = Func.Convert.dConvertToDouble(myArray[12]);
                //        //dr["discount_amt"] = Convert.ToDouble(myArray[13]);
                //        dr["Total"] = Convert.ToDouble(myArray[15]);
                //        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[10]);
                //        dr["Status"] = "U";
                //        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[24]);
                //        dr["MRP_Rate"] = 0.00;
                //        dr["Ass_Value"] = 0.00;

                //        dtDetails.Rows.Add(dr);
                //        dtDetails.AcceptChanges();
                //        iRowCnt = iRowCnt + 1;
                //    }
                //}

                ////dtDetails.ImportRow(dtTemp.Rows[0]);
                //if (dtDetails.Rows.Count == 0 && dtTemp.Rows.Count != 0)
                //    dtDetails.ImportRow(dtTemp.Rows[0]);
                ////FillDetailsGrid
                ////Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");
                //Session["MRPartDetails"] = dtDetails;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);

                //// New Code

                //FillDetailsFromGrid(false);
                //BindDataToGrid();
                //FillDetailsFromGrid(false);
                //CreateNewRowToTaxGroupDetailsTable();
                //Session["MRPartDetails"] = dtDetails;
                //Session["MRGrpTaxDetails"] = dtMRGrpTaxDetails;
                //Session["MRTaxDetails"] = dtMRTaxDetails;
                //BindDataToGrid();

                BindDataAndDisplay();
                if (txtSearch.Text != "" && btnSave.Text == "Search")
                    btnSave.Text = "ClearSearch";
                else if (txtSearch.Text != "" && btnSave.Text == "ClearSearch")
                {
                    txtSearch.Text = "";
                    btnSave.Text = "Search";
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            PagerV2_1.CurrentIndex = currnetPageIndx;
            bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
            mpeSelectPart.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text != "" && btnSave.Text == "Search")
                    btnSave.Text = "ClearSearch";
                else if (txtSearch.Text != "" && btnSave.Text == "ClearSearch")
                {
                    txtSearch.Text = "";
                    btnSave.Text = "Search";
                }
                BindDataAndDisplay();

                PagerV2_1.ItemCount = 10;
                PagerV2_1.CurrentIndex = 0;
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
                mpeSelectPart.Show();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        public void bindGrid(string sSelect, string sSearchPart)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                int iDealerId;
                int iSupplierID;
                string sSelectedPartID = "";
                string sIs_AutoReceipt = "";
                int iHOBrID = 0;
                string sDocGST = "", sCustTaxTag = "";

                if (Func.Convert.sConvertToString(txtSearch.Text.Trim()) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }


                iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iHOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iSupplierID = Func.Convert.iConvertToInt(iSupplierId);
                sSelectedPartID = hdnSelectedPartID.Value;
                sDocGST = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                sCustTaxTag = Func.Convert.sConvertToString(hdnSupTaxTag.Value);

                //sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                //iSupplierID = Func.Convert.iConvertToInt(Request.QueryString["SupplierID"].ToString());
                sIs_AutoReceipt = Func.Convert.sConvertToString(ddlInvoiceType.SelectedValue);
                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }
                object[] ParaValues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iSupplierID, sIs_AutoReceipt, iHOBrID, sDocGST, sCustTaxTag };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPOPartDetails_With_MRPRate_Paging", ParaValues);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPOPartDetails_With_MRPRate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iSupplierID, sIs_AutoReceipt, iHOBrID);


                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    //PartDetailsGrid.DataSource = dsSrchgrid;
                    //PartDetailsGrid.DataBind();

                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][7]);
                    PagerV2_1.ItemCount = iTotalCnt;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts();
                    lblNMsg.Visible = false;
                }
                else
                {
                    lblNMsg.Visible = true;
                    PagerV2_1.ItemCount = 0;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    return;
                }
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

        private void ChkSelectedParts()
        {
            CheckBox chk = new CheckBox();
            string sPartID = "";
            string str = null;
            string[] strArr = null;
            txtPartIds.Text = "";
            str = txtPartIds.Text;
            if (str == "") return;
            strArr = str.Split('#');
            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;

                for (int j = 0; j < strArr.Length; j++)
                {
                    if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                    {
                        chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                        chk.Checked = true;
                        break;
                    }
                }
            }
        }

        private string GetIdFromString(string strArr)
        {
            string sID = "";
            char[] strToParse = strArr.ToCharArray();
            // convert string to array of chars    
            char ch; int charpos = 0;
            ch = strToParse[charpos];
            while (ch != ' ')
            {
                sID = sID + ch;
                charpos++;
                ch = strToParse[charpos];

            }
            return sID;
        }

        #endregion

        protected void txtDMSInvNo_TextChanged(object sender, EventArgs e)
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

                string query = "select * from TM_MaterialReceipt  where TM_MaterialReceipt.DMSInv_No='" + txtDMSInvNo.Text + "'and Supplier_ID='" + iSupplierId + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                if (dt.Rows.Count != 0)
                {
                    txtDMSInvNo.Text = "";
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
                frmMaterialReceipt obj = new frmMaterialReceipt();
                var conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                };

                string query = "select * from TM_MaterialReceipt  where TM_MaterialReceipt.DMSInv_No='" + InvoiceNo + "' and Supplier_ID=" + Func.Convert.iConvertToInt(HttpContext.Current.Session["iSupplierID"]) + "";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                return dt.Rows.Count.ToString();
            }
            catch (Exception ex) { return null; }
        }

        protected void rbtLstDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnConfirm.Value == "N" && hdnIsAutoReceipt.Value == "N")
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

    }
}
