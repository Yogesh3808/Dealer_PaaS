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

namespace MANART.Forms.Spares
{
    public partial class frmPartsPO : System.Web.UI.Page
    {
        int sSupplierId = 0;
        string sFileName = "";
        string PONo = "";
        private int iPOID = 0;
        int DealerID = 0;
        string sDealerCode = "";
        private DataTable dtDetails = new DataTable();
        private DataSet dsCreatedBy = new DataSet();
        private bool bDetailsRecordExist = false;
        clsSparePO objPO = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        private int UsreType;
        int Sup_Type = 0;
        int iUser_ID = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 2;
                ToolbarC.iValidationIdForConfirm = 2;
                //ToolbarC.iFormIdToOpenForm = 101;
                ToolbarC.bUseImgOrButton = true;

                ExportLocation.bUseSpareDealerCode = true;
                ExportLocation.SetControlValue();

                iUser_ID = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);

                if (!IsPostBack)
                {
                    Session["SPOPartDetails"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                    GetPreviousPOTypeDate();
                }

                SearchGrid.sGridPanelTitle = "PO List";

                if (iPOID != 0)
                {
                    GetDataAndDisplay();
                    GetPreviousPOTypeDate();
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
            DealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
            sSupplierId = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["SPOPartDetails"] = null;
                GeneratePONo();
                GetCreatedBy();
                DisplayPreviousRecord();

            }
        }
        private void GetPreviousPOTypeDate()
        {
            DataSet dsDate = new DataSet();
            objPO = new clsSparePO();
            dsDate = objPO.GetPreviousPOTypeDate(iUserId, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), Func.Convert.iConvertToInt(ExportLocation.iSupplierId), Func.Convert.iConvertToInt(drpPoType.SelectedValue), iPOID);
            if (dsDate.Tables[0].Rows.Count == 0)
            {
                txtPreviousPoDate.Text = "N";
                return;
            }
            if (iPOID != 0)
            {
                txtPreviousPoDate.Text = Func.Convert.sConvertToString(dsDate.Tables[0].Rows[0]["PO_Date"]);
                return;
            }
            txtPreviousPoDate.Text = Func.Convert.sConvertToString(dsDate.Tables[0].Rows[0]["PO_Date"]);
        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sSupplierId = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);
                txtListPartNo.Text = "";
                txtListPartNo.Visible = false;
                lblListPartNo.Visible = false;
                Sup_Type = ExportLocation.iSupType;
                DealerID = ExportLocation.iDealerId;
                FillCombo();
                FillSelectionGrid();
                Session["SPOPartDetails"] = null;
                GeneratePONo();
                GetCreatedBy();
                DisplayPreviousRecord();
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
                FillCombo();
                FillSelectionGrid();
                Session["SPOPartDetails"] = null;
                GeneratePONo();
                GetCreatedBy();
                DisplayPreviousRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillCombo()
        {
            //Changed by VIkram on 27.06.16
            Func.Common.BindDataToCombo(drpPoType, clsCommon.ComboQueryType.DOMPOType, 0, (Sup_Type == 0) ? " and Id > (9999)" : (Sup_Type == 18) ? " and Id in (1,2,4,5,6,8)" : " and Id in (11)");
            //DataSet dsPOType = new DataSet();
            //objPO = new clsSparePO();
            //dsPOType = objPO.GetPOType(0, Sup_Type);
            //drpPoType.DataSource = dsPOType.Tables[0];
            //drpPoType.DataTextField = "Name";
            //drpPoType.DataValueField = "ID";
            //drpPoType.DataBind();
            //drpPoType.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iPOID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
            GetPreviousPOTypeDate();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objPO = new clsSparePO();
                if (iPOID != 0)
                {
                    ds = objPO.GetPO(iPOID, "All", iDealerID, ExportLocation.bDistributor, 0);
                    sNew = "N";
                    DisplayData(ds);
                    objPO = null;
                }
                else
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objPO = null;
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

                objPO = new clsSparePO();
                ds = objPO.GetPO(iPOID, "New", iDealerID, ExportLocation.bDistributor, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["PO_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["PO_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                //txtPONo.Text = Func.Common.sGetMaxDocNo(ExportLocation.sDealerCode, "", "PO", Func.Convert.iConvertToInt(ExportLocation.iDealerId));
                txtPODate.Text = Func.Common.sGetCurrentDate(1, false);
                UploafFile.Style.Add("display", "");
                objPO = null;
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
                UploafFile.Style.Add("display", "none");
                //Display Header

                txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Po_No"]);
                txtPODate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Date"]);
                txtCreatedBy.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_CreatedBy"]);

                if (iPOID != 0)
                {
                    drpPoType.Style.Add("display", "none");
                    txtPoType.Style.Add("display", "");
                    txtPoType.Enabled = false;
                    txtPoType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Desc"]);
                    hdnPoTypeID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]);
                }
                else
                {
                    drpPoType.Style.Add("display", "");
                    txtPoType.Style.Add("display", "none");
                    drpPoType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]);
                }

                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChassisNo"]);
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngineNo"]);
                txtJobCardNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobDtl"]);
                txtJobcardHDRID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCard_HDR_ID"]);
                DrpEstimate.SelectedValue = (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8) ? Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCard_HDR_ID"]) : "0";
                Session["SPOPartDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["SPOPartDetails"] = dtDetails;
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                BindDataToGrid();
                ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, sNew == "N" ? true : false);

                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmConfirm);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]) == "7")
                    {
                        btnShortClose.Style.Add("display", "none");
                    }
                    else
                        btnShortClose.Style.Add("display", "");
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, true);
                }
                else
                {
                    btnShortClose.Style.Add("display", "none");
                }

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Cancel"]) == "Y")
                {
                    bEnableControls = false;
                }
                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                    MakeEnableDisableCtrl(false);
                }
                else
                {
                    MakeEnableDisableControls(false);
                    MakeEnableDisableCtrl(false);
                }

                // For Hide VOR Related Fields
                trPCRDetails.Style.Add("display", "none");

                if (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 3 || Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8)
                {
                    trPCRDetails.Style.Add("display", "");
                    txtChassisNo.ReadOnly = true;
                    txtEngineNo.ReadOnly = true;
                    txtJobCardNo.ReadOnly = true;

                    txtJobCardNo.Style.Add("display", (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8 && (txtID.Text == "0" || txtID.Text == "")) ? "none" : "");
                    DrpEstimate.Style.Add("display", (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8 && (txtID.Text == "0" || txtID.Text == "")) ? "" : "none");
                    lblJobCardNo.Text = (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8) ? "Ref. Estimate" : "Ref. Jobcard";
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
            PartGrid.Enabled = bEnable;
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, bEnable);
        }

        //Created By Vikram
        private void MakeEnableDisableCtrl(bool bEnable)
        {
            //drpPoType.Enabled = bEnable;
            txtPODate.Enabled = bEnable;
        }

        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerId = 3;
                //DealerID = Func.Convert.iConvertToInt(ExportLocation.iDealerId);
                DealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());

                objPO = new clsSparePO();
                ds = objPO.GetPO(iPOID, "Max", DealerID, ExportLocation.bDistributor, 0);
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
                    Session["SPOPartDetails"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objPO = null;
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
                    //txtPONo.Text = "";
                    PSelectionGrid.Style.Add("display", "none");
                    txtPoType.Style.Add("display", "none");
                    txtListPartNo.Text = "";
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
                    DisplayPreviousRecord();
                    if (Func.Convert.iConvertToInt(ExportLocation.iSupplierId) != 0 && drpPoType.SelectedValue != "0")
                    {
                        GeneratePONo();
                        GetCreatedBy();
                    }

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
                iPOID = Func.Convert.iConvertToInt(txtID.Text);
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
                SearchGrid.AddToSearchCombo("PO No");
                SearchGrid.AddToSearchCombo("PO Date");
                SearchGrid.AddToSearchCombo("PO Status");
                //SearchGrid.AddToSearchCombo("PO Type");
                SearchGrid.iDealerID = sSupplierId;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUserId) + "," + ExportLocation.bDistributor;
                SearchGrid.sSqlFor = "SparesPO";
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
                dtHdr.Columns.Add(new DataColumn("PO_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PO_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Po_Type_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PO_Desc", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PO_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PO_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PO_Total", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("PO_TotalQty", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PO_TotalItems", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PO_CreatedBy", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Chassis_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Is_Distributor", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("JobCard_HDR_ID", typeof(int)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["PO_No"] = txtPONo.Text;
                dr["PO_Date"] = txtPODate.Text;
                dr["Dealer_ID"] = Session["iDealerID"].ToString();

                if (txtID.Text != "")
                {
                    dr["PO_Desc"] = txtPoType.Text;
                    dr["Po_Type_ID"] = Func.Convert.iConvertToInt(hdnPoTypeID.Value);
                }
                else
                    dr["Po_Type_ID"] = Func.Convert.iConvertToInt(drpPoType.SelectedValue);
                dr["PO_Confirm"] = "N";
                dr["PO_Cancel"] = "N";
                dr["PO_Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                dr["PO_TotalQty"] = Func.Convert.iConvertToInt(txtTotalQty.Text);

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
                dr["PO_TotalItems"] = TotCntQty;
                dr["PO_CreatedBy"] = txtCreatedBy.Text;
                dr["Chassis_No"] = txtChassisNo.Text;
                dr["UserId"] = iUserId;

                dr["Supplier_ID"] = (ExportLocation.bDistributor == "N") ? Func.Convert.iConvertToInt(ExportLocation.iSupplierId) : 0;
                dr["Is_Distributor"] = ExportLocation.bDistributor;
                dr["JobCard_HDR_ID"] = (txtJobcardHDRID.Text.Trim().ToString() == "") ? 0 : Func.Convert.iConvertToInt(txtJobcardHDRID.Text.Trim().ToString());

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

            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (sSupplierId == 0)
            {
                sMessage = sMessage + "\\n *Please Select Supplier.";
                bValidateRecord = false;
            }
            if (txtPODate.Text == "")
            {
                sMessage = sMessage + "\\n *Enter the document date.";
                bValidateRecord = false;
            }

            if (drpPoType.SelectedValue == "0" && hdnPoTypeID.Value == "")
            {
                sMessage = sMessage + "\\n *Please select the PO Type.";
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
            clsSparePO objPO = new clsSparePO();
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
                    dtHdr.Rows[0]["PO_Confirm"] = "Y";


                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["PO_Cancel"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }

                //if (objPO.bSaveRecordWithPart(ExportLocation.iDealerId, dtHdr, dtDetails, ref iPOID, iUserId) == true)
                if (objPO.bSaveRecordWithPart(sDealerCode, iDealerID, dtHdr, dtDetails, ref iPOID, iUserId) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iPOID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        private void GeneratePONo()
        {
            objPO = new clsSparePO();
            txtPONo.Text = Func.Convert.sConvertToString(objPO.GeneratePO(sDealerCode, DealerID, Convert.ToInt16(drpPoType.SelectedValue), ExportLocation.bDistributor));
        }
        private void GetCreatedBy()
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            UsreType = Func.Convert.iConvertToInt(Session["UserType"]);

            // 'Replace Func.DB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                dsCreatedBy = objDB.ExecuteStoredProcedureAndGetDataset("SP_POCreatedByName", iUserId, UsreType);

                txtCreatedBy.Text = dsCreatedBy.Tables[0].Rows[0]["PO_CreatedBy"].ToString();

            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        private DataTable GetListOfPartNo()
        {
            DataTable dtLayout = new DataTable();
            try
            {
                //DataTable dtLayout = new DataTable();
                if ((FileUpload1.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    string query = null;
                    string connString = "";
                    string strFileName = "SparesPO" + Session["iDealerID"].ToString() + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        FileUpload1.SaveAs(Server.MapPath("~/DownloadFiles/SparesPO/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/SparesPO/" + strFileName + ".xlsx");

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    query = "SELECT * FROM [Sheet1$]";

                    //Create the connection object
                    conn = new OleDbConnection(connString);
                    //Open connection
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //Create the command object
                    cmd = new OleDbCommand(query, conn);
                    da = new OleDbDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dtLayout = ds.Tables[0];

                }
                //return dtLayout;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtLayout;

        }
        private void SetListOfNotUploadedPartNo(DataTable dtPartList)
        {
            try
            {
                string sPartList = "";
                if (Func.Common.iRowCntOfTable(dtPartList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                    {
                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["PartNo"]) + ",";
                    }
                    lblListPartNo.Text = "Parts which are not uploaded";
                    txtListPartNo.Text = sPartList;
                    txtListPartNo.Visible = true;
                    lblListPartNo.Visible = true;
                }
                else
                {
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
                bindGrid("S", "x");
                ModalPopUpExtender.Show();
                //FillDetailsFromGrid(false);
                //BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strPosNo = "";
                dtDetails = (DataTable)Session["SPOPartDetails"];

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
                            //PartID                
                            TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                            TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                            LinkButton lblCancel = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lblCancel");
                            if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                            {
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "U")
                                    {
                                        if (strPosNo == "")
                                            strPosNo = iRowCnt.ToString();
                                        else
                                            strPosNo = strPosNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "O")
                                    {
                                        if (strPosNo == "")
                                            strPosNo = iRowCnt.ToString();
                                        else
                                            strPosNo = strPosNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "M")
                                    {
                                        if (strPosNo == "")
                                            strPosNo = iRowCnt.ToString();
                                        else
                                            strPosNo = strPosNo + "," + iRowCnt.ToString();
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

                                // Get Qty
                                TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                dtDetails.Rows[iMRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                // Get MRPRate
                                TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                                //MOQ
                                TextBox txtMOQ = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMOQ");
                                dtDetails.Rows[iMRowCnt]["MOQ"] = Func.Convert.dConvertToDouble(txtMOQ.Text);

                                // Get Total
                                TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                // Get Status                           
                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

                                TextBox txtJobDtlID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtJobDtlID");
                                dtDetails.Rows[iMRowCnt]["JobDtlID"] = Func.Convert.iConvertToInt(txtJobDtlID.Text);


                                if (txtStatus.Text == "C")
                                {
                                    lblCancel.Attributes.Add("disabled", "true");
                                    lblCancel.Text = "Cancelled";
                                }

                                CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                                sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Status"]);

                                Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
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
                                        else if (dtDetails.Rows[iMRowCnt]["Status"] != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "E";
                                            bDetailsRecordExist = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //if (strPosNo != "")
                    //{
                    //    string[] splstrPos = strPosNo.Split(',');
                    //    if (splstrPos.Length > 1)
                    //    {
                    //        for (int i = 0; i < splstrPos.Length; i++)
                    //        {
                    //            dtDetails.Rows.RemoveAt(Func.Convert.iConvertToInt(splstrPos[i]) - i);
                    //        }
                    //    }
                    //    else
                    //        dtDetails.Rows.RemoveAt(Func.Convert.iConvertToInt(strPosNo));
                    //}

                    for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    {
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter PO Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
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
                    dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    //dtDetails.Columns.Add(new DataColumn("IS_PartSupercded", typeof(string)));

                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "")
                    {
                        goto Exit;
                    }
                }


                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 1;
                dr["MOQ"] = 1;
                dr["MRPRate"] = 1;
                dr["Total"] = 1;
                dr["Status"] = "N";
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

        protected void Search(object sender, EventArgs e)
        {
            //this.BindGrid();
        }

        private void BindGrid()
        {
            //string constr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //       // cmd.CommandText = "SELECT Part_ID, Qty, Rate FROM TD_SpPurchaseOrderParts WHERE Part_ID LIKE '%' + @Part_ID + '%'";
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@Part_ID", txtSearch.Text.Trim());
            //        DataTable dt = new DataTable();
            //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //        {
            //            sda.Fill(dt);
            //            PartGrid.DataSource = dt;
            //            PartGrid.DataBind();
            //        }
            //    }
            //}
        }
        private void BindDataToGrid()
        {
            try
            {
                //If No Data in Grid
                // if (Func.Convert.iConvertToInt(drpPoType.SelectedValue.ToString()) != 3 && Func.Convert.iConvertToInt(drpPoType.SelectedValue.ToString()) != 8)
                if (Func.Convert.iConvertToInt(hdnPoTypeID.Value) != 3)
                {
                    if (Session["SPOPartDetails"] == null)
                    {

                        CreateNewRowToDetailsTable();
                        Session["SPOPartDetails"] = dtDetails;

                    }
                    else
                    {
                        dtDetails = (DataTable)Session["SPOPartDetails"];
                        if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                            CreateNewRowToDetailsTable();
                        dtDetails = (DataTable)Session["SPOPartDetails"];

                    }
                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();
                SetGridControlProperty();
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
                string sIS_PartSupercded = "";

                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                hdnSelectedPartID.Value = "";

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");
                // Changed By Vikram on Date 01.07.2016
                string sDealerId = Session["iDealerID"].ToString();
                string sPartID = "", sPartName = "", cPartID = "";
                string sPartId = "0";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    if (hdnPoTypeID.Value == "7") // For Part Claim Po Creation
                    {
                        // Hide  Delete and cancel 
                        PartGrid.HeaderRow.Cells[8].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Hide Cell 
                    }
                   
                    //Status
                 
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                    //
                    sIS_PartSupercded = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["IS_PartSupercded"]);

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        //PartID
                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        sPartId = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                        if (sPartId != "0")
                        {
                            hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + sPartId;
                        }
                        //PartNo
                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        cPartID = txtPartNo1.Text;
                        if (cPartID != "" && sRecordStatus != "C")
                        {
                            dPartQty = 0;
                            dPartRate = 0;

                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);

                            TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                            dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);

                            TextBox txtJobDtlID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtJobDtlID");
                            dtDetails.Rows[iRowCnt]["JobDtlID"] = Func.Convert.sConvertToString(txtJobDtlID.Text.Trim());

                            if (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 3)
                            {
                                txtQuantity.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            }
                        }
                    }

                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowMultiPartSearch(this,'" + iDealerID + "','" + ExportLocation.bDistributor + "','" + ExportLocation.iSupplierId + "');");

                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    if (sPartID == "0")
                        lnkSelectPart.Style.Add("display", "");
                    else
                        lnkSelectPart.Style.Add("display", "none");
                    if (txtGPartName.Text == "")
                    {
                        lnkSelectPart.Style.Add("display", "");
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                    {
                        lnkSelectPart.Style.Add("display", "none");
                        txtPartNo.Style.Add("display", "");

                    }

                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");

                    if (sRecordStatus == "N")
                    {
                        Chk.Style.Add("display", "none");
                        lblDelete.Style.Add("display", "none");

                    }
                    else
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                    }



                    //If Record is uploaded then it is not allowed to change

                    if (sRecordStatus == "M")
                    {
                        if (sPartID == "0" && sRecordStatus == "M")
                        {
                            // lnkSelectPart.Style.Add("display", "none");
                            PartGrid.Rows[iRowCnt].Visible = false;
                        }
                    }
                    //If Part Id  is not allocated           
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        //PartGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                    }
                    else if (sRecordStatus == "P")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                    }
                    else if (sRecordStatus == "O")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                    }
                    else if (sRecordStatus == "C")
                    {
                        //lblCancel.Style.Add("display", "");
                        //lblCancel.Attributes.Add("disabled", "true");
                        //lblCancel.Text = "Cancelled";
                    }
                    //// For Make color to Susperceded Part
                    if (sIS_PartSupercded == "Y")
                    {
                        for (int j = 0; j < PartGrid.Rows[iRowCnt].Cells.Count; j++)
                        {
                            PartGrid.Rows[iRowCnt].Cells[j].ForeColor = Color.Red;
                        }
                    }
                }
                txtTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void lblCancel_Click(object sender, EventArgs e)
        {
            FillDetailsFromGrid(false);
            BindDataToGrid();
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
                    Session["SPOPartDetails"] = dtDetails;
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

                dtDetails = (DataTable)Session["SPOPartDetails"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
                //SetGridControlProperty();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                objPO = new clsSparePO();
                DataTable dt = GetListOfPartNo();

                int iCountNotUpload = 0;

                string sFileNameForSave = "";
                DataSet dsUploadPartDetails;
                System.Threading.Thread.Sleep(2000);
                sFileName = FileUpload1.FileName;
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));
                sSupplierId = Func.Convert.iConvertToInt(ExportLocation.iSupplierId);//iDealerId


                //Changed by Vikram on Date 20.06.2016
                //dsUploadPartDetails = objPO.UploadPartDetailsAndGetPartDetails(FileUpload1.FileName, Func.Convert.iConvertToInt(ExportLocation.iDealerId), dt, ExportLocation.bDistributor, iUserId);
                dsUploadPartDetails = objPO.UploadPartDetailsAndGetPartDetails(FileUpload1.FileName, Convert.ToInt32(Session["iDealerID"].ToString()), sSupplierId, dt, ExportLocation.bDistributor, iUserId);
                if (dsUploadPartDetails == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in upload the the file contact admin.');</script>");
                    return;
                }
                else
                {
                    SetListOfNotUploadedPartNo(dsUploadPartDetails.Tables[0]);
                    dtDetails = dsUploadPartDetails.Tables[1];
                }
                if (iCountNotUpload != 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + " parts not uploaded.');</script>");
                }
                DataRow dr;
                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Status"] = "M";
                dr["IS_PartSupercded"] = "";
                dtDetails.Rows.Add(dr);
                dtDetails.AcceptChanges();
                Session["SPOPartDetails"] = dtDetails;
                BindDataToGrid();
                CPEPartDetails.Collapsed = false;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void drpPoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GeneratePONo();
            GetCreatedBy();
            GetPreviousPOTypeDate();
            trPCRDetails.Style.Add("display", "none");
            hdnPoTypeID.Value = drpPoType.SelectedValue;
            if (Func.Convert.iConvertToInt(hdnPoTypeID.Value) == 8)
            {
                Func.Common.BindDataToCombo(DrpEstimate, clsCommon.ComboQueryType.EstNo, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), " and Id not in(select distinct JobCard_HDR_ID from TM_SparePO where PO_Type_ID =8 and isnull(PO_Cancel,'N')='N')");
                trPCRDetails.Style.Add("display", "");
                txtChassisNo.ReadOnly = true;
                txtEngineNo.ReadOnly = true;
                txtJobCardNo.Style.Add("display", "none");
                DrpEstimate.Style.Add("display", "");
                lblJobCardNo.Text = "Ref. Estimate";


            }
        }
        protected void btnShortClose_Click(object sender, EventArgs e)
        {
            try
            {
                iPOID = Func.Convert.iConvertToInt(txtID.Text);
                objPO = new clsSparePO();
                if (objPO.bSaveShortClose(iPOID) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(16,'" + Server.HtmlEncode("Parts PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
                }
                //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PO Shortclosed Successfully');</script>");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        int i = 1;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);

                int no = e.Row.RowIndex;
                if (txtPartID != "0")
                {
                    //1.NW
                    // (e.Row.FindControl("lblNo") as Label).Text = (no).ToString();
                    //2. NW
                    lblNo.Text = i.ToString();
                    i++;
                    //3.W
                    // lblNo.Text = ((PartGrid.PageIndex * PartGrid.PageSize) + e.Row.RowIndex + 1).ToString();
                    //4.
                    //lblNo.Text = ((PartGrid.PageIndex * PartGrid.PageSize) + i - 1).ToString();
                    //i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    //i = 2;
                }
            }
        }

        protected void DrpEstimate_SelectedIndexChanged(object sender, EventArgs e)
        {
            objPO = new clsSparePO();
            DataSet ds = new DataSet();

            int iPOID = 0;
            ds = objPO.GetPO(Func.Convert.iConvertToInt(DrpEstimate.SelectedValue), "ACCIDENT", DealerID, "N", 0);
            if (ds != null) // if no Data Exist
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ds.Tables[0].Rows[0]["PO_Cancel"] = "N";
                        ds.Tables[0].Rows[0]["PO_Confirm"] = "N";
                        sNew = "Y";
                        DisplayData(ds);
                    }
                }
            }
            txtID.Text = "";
            txtPODate.Text = Func.Common.sGetCurrentDate(1, false);
            UploafFile.Style.Add("display", "");
            objPO = null;
            ds = null;

            if (Func.Convert.iConvertToInt(ExportLocation.iSupplierId) != 0)//ExportLocation.iDealerId
            {
                GeneratePONo();
                GetCreatedBy();
            }
            ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);

        }

        #region PopUpModal Coding

        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        string sSourchFrom = "";
        string sTransFrom = String.Empty;

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                int idataCount = 0;
                string[] asd;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                dtDetails = (DataTable)Session["SPOPartDetails"];
                idataCount = dtDetails.Rows.Count;
                sSourchFrom = "SPOPartDetails";

                dtTemp = dtDetails.Clone();

                if (dtDetails.Rows.Count != 0)
                {
                    dtTemp.ImportRow(dtDetails.Rows[0]);
                    dtDetails.Rows.RemoveAt(0);
                }

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("JobDtlID", typeof(int)));
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
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["Part_ID"] = Func.Convert.iConvertToInt(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[4]);

                        dr["Qty"] = Func.Convert.dConvertToDouble(myArray[2]);
                        dr["MOQ"] = Func.Convert.dConvertToDouble(myArray[2]);
                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);

                        dr["Total"] = Convert.ToDouble(Func.Convert.sConvertToString(myArray[1])) * Convert.ToDouble(Func.Convert.sConvertToString(myArray[2]));
                        dr["Status"] = "U";
                        if (Func.Convert.sConvertToString(Request.QueryString["TransFrom"]) != "SockAdjustment" && Func.Convert.sConvertToString(Request.QueryString["TransFrom"]) != "SockTranChallan")
                        {
                            dr["JobDtlID"] = 0;
                        }
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                //dtDetails.ImportRow(dtTemp.Rows[0]);
                if (dtDetails.Rows.Count == 0 && dtTemp.Rows.Count != 0)
                    dtDetails.ImportRow(dtTemp.Rows[0]);
                Session[sSourchFrom] = dtDetails;
                //FillDetailsGrid
                FillDetailsFromGrid(false);
                BindDataToGrid();
                txtSearch.Text = "";
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
            ModalPopUpExtender.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PagerV2_1.ItemCount = 15;
                PagerV2_1.CurrentIndex = 0;
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
                ModalPopUpExtender.Show();
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

        public void bindGrid(string sSelect, string sSearchPart)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                int iDealerId;
                int iSupplierID;
                string sSelectedPartID = "";
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }

                iDealerId = DealerID;
                iSupplierID = Func.Convert.iConvertToInt(sSupplierId);
                sSelectedPartID = hdnSelectedPartID.Value;
                string sTransFrom="Common";

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_PartDetailsWithMRPRate_GETPaging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, iSupplierID, iUser_ID, sTransFrom, sSelectedPartID);

                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][5]);
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
                    string[] srtArrPartID = strArr[j].Split(new string[] { "<--" }, StringSplitOptions.None);
                    for (int K = 0; K < srtArrPartID.Length; K++)
                        if (sPartID == Func.Convert.sConvertToString(srtArrPartID[K]))
                        {
                            chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                            chk.Checked = true;
                            break;
                        }
                }
            }
        }

        //private string GetIdFromString(string strArr)
        //{
        //    string sID = "";
        //    char[] strToParse = strArr.ToCharArray();
        //    // convert string to array of chars    
        //    char ch; int charpos = 0;
        //    ch = strToParse[charpos];
        //    while (ch != ' ')
        //    {
        //        sID = sID + ch;
        //        charpos++;
        //        ch = strToParse[charpos];

        //    }
        //    return sID;
        //}
        #endregion

    }
}