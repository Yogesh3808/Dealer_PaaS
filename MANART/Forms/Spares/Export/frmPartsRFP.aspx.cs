using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using System.Data.OleDb;
using MANART.WebParts;
using System.Drawing;
using System.IO;

namespace MANART.Forms.Spares.Export
{
    public partial class frmPartsRFP : System.Web.UI.Page
    {

        private int iRFPID = 0;
        string sDealerCode = "";
        private DataTable dtDetails = new DataTable();
        private DataSet dsCreatedBy = new DataSet();
        private bool bDetailsRecordExist = false;
        clsPartsRFP ObjPartsRFP = null;
        string sNew = "Y";
        int iUserId = 0;
        private int UsreType;
        int iHOBrId = 0;
        private int iDealerID = 0;
        int iTotalCnt = 0;
        string sSourchFrom = "";
        string sTransFrom = String.Empty;
        int iTotalLineItem = 0;
        int iMenuId = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 9;
                ToolbarC.iValidationIdForConfirm = 9;
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iFormIdToOpenForm = 222;
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);// MD User
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                if (!IsPostBack)
                {
                    Session["RFPPartDetail"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                }
                // RFP MenuID=748 and PO MenuID =863
                SearchGrid.sGridPanelTitle = (iMenuId == 748) ? "RFP List" : "PO List";
                lblTitle.Text = (iMenuId == 748) ? "Parts RFP" : "Parts Purchase Orders";
                CPEDocDetails.CollapsedText = (iMenuId == 748) ? "RFP Details" : "PO Details";
                CPEDocDetails.ExpandedText = (iMenuId == 748) ? "RFP Details" : "PO Details";
                lblRFPNo.Text = (iMenuId == 748) ? "RFP No:" : "PO No:";
                lblRFPDate.Text = (iMenuId == 748) ? "RFP Date:" : "PO Date:";
                UploafFile.Style.Add("display", (txtUserType.Text.Trim() == "6" || iMenuId == 863) ? "none" : "");

                if (iRFPID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmNew, true);
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

            Location.drpCountryIndexChanged += new EventHandler(Location_drpCountryIndexChanged);
            Location.drpRegionIndexChanged += new EventHandler(Location_drpRegionChanged);

            // Dealer MD User related Change 
            if (Session["iDealerID"] != null)
            {
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
            }
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["RFPPartDetail"] = null;
                GetCreatedBy();
                DisplayPreviousRecord();
            }
        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            try
            {
                GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(DrpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " and ISNULL(Active,'N')='Y' ");
            DrpSupplier.SelectedValue = "0";

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iRFPID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                ObjPartsRFP = new clsPartsRFP();
                if (iRFPID != 0)
                {
                    ds = ObjPartsRFP.GetPartsRFP(iRFPID, "All", iDealerID, iMenuId);
                    sNew = "N";
                    DisplayData(ds);
                    ObjPartsRFP = null;
                }
                else
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                ObjPartsRFP = null;
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
                ObjPartsRFP = new clsPartsRFP();
                ds = ObjPartsRFP.GetPartsRFP(iRFPID, "New", iDealerID, iMenuId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["DocStatus"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                clsCommon ObjComm = new clsCommon();
                txtRFPNo.Text = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, Location.iDealerId, (iMenuId == 748) ? "RFP" : "RFPPO", ""));
                txtRFPDate.Text = Func.Common.sGetCurrentDate(1, false);
                UploafFile.Style.Add("display", (txtUserType.Text.Trim() == "6" || iMenuId == 863) ? "none" : "");
                hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + ((iMenuId == 748) ? "RFP" : "RFPPO") + "/" + iMenuId;
                GetCreatedBy();
                ObjPartsRFP = null;
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

                txtRFPNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RFP_No"]);
                txtRFPDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RFP_Date"]);
                txtCreatedBy.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CreatedBy"]);
                txtRemaks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remark"]);

                if (sNew == "N")
                    Func.Common.BindDataToCombo(DrpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " or ISNULL(Active,'N')='N' ");
                else
                    Func.Common.BindDataToCombo(DrpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " and ISNULL(Active,'N')='Y' ");
                DrpSupplier.SelectedValue = "0";
                DrpSupplier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_ID"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                DrpSupplier.Enabled = false;

                Session["RFPPartDetail"] = null;
                dtDetails = ds.Tables[1];
                Session["RFPPartDetail"] = dtDetails;
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                    hdnConfirm.Value = "Y";
                else
                    hdnConfirm.Value = "N";
                BindDataToGrid();
                ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, sNew == "N" ? true : false);

                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, true);
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
                    DrpSupplier.Enabled = false;
                    UploafFile.Style.Add("display", "none");
                }
                if (iMenuId == 863)
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmNew, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
            txtRFPDate.Enabled = false;
            txtRemaks.Enabled = bEnable;
            PartGrid.Enabled = bEnable;
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
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());

                ObjPartsRFP = new clsPartsRFP();
                ds = ObjPartsRFP.GetPartsRFP(iRFPID, "Max", iDealerID, iMenuId);
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
                    Session["RFPPartDetail"] = null;
                    BindDataToGrid();

                }
                ds = null;
                ObjPartsRFP = null;
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
                    txtListPartNo.Text = "";
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
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
                iRFPID = Func.Convert.iConvertToInt(txtID.Text);
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
                else if (objCommon.sUserRole == "19")
                {
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, true);
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
                SearchGrid.AddToSearchCombo((iMenuId == 748) ? "RFP No" : "PO No");
                SearchGrid.AddToSearchCombo((iMenuId == 748) ? "RFP Date" : "PO Date");
                SearchGrid.AddToSearchCombo((iMenuId == 748) ? "RFP Status" : "PO Status");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sModelPart = (iMenuId == 748) ? "RFP" : "RFPPO";
                SearchGrid.sSqlFor = "PartsRFP";
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
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("RFP_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("RFP_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("total", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Total_Qty", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Total_Line_Items", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Remark", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["RFP_No"] = txtRFPNo.Text;
                dr["RFP_Date"] = txtRFPDate.Text;
                dr["Dealer_ID"] = Session["iDealerID"].ToString();
                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["UserId"] = iUserId;
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(DrpSupplier.SelectedValue);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);
                dr["total"] = Func.Convert.dConvertToDouble(txtAllTotal.Text);
                dr["Total_Qty"] = Func.Convert.iConvertToInt(txtTotalQty.Text);
                dr["Total_Line_Items"] = Func.Convert.iConvertToInt(txtTotalLineItem.Text);
                dr["CreatedBy"] = txtCreatedBy.Text.Trim();
                dr["Remark"] = txtRemaks.Text.Trim();
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
            if (txtRFPDate.Text == "")
            {
                sMessage = sMessage + "\\n *Enter the document date.";
                bValidateRecord = false;
            }

            if (DrpSupplier.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n *Please select the Supplier Name.";
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
            clsPartsRFP ObjPartsRFP = new clsPartsRFP();
            try
            {
                bool bEnblSave = false;
                if (bSaveWithConfirm == false && bSaveWithCancel == false)
                {
                    bEnblSave = true;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                if (bValidateRecord() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
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
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

                if (ObjPartsRFP.bSaveRecordWithPart(sDealerCode, iDealerID, dtHdr, dtDetails, ref iRFPID, iUserId) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iRFPID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts RFP ") + "','" + Server.HtmlEncode(txtRFPNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts RFP ") + "','" + Server.HtmlEncode(txtRFPNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts RFP ") + "','" + Server.HtmlEncode(txtRFPNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts RFP ") + "','" + Server.HtmlEncode(txtRFPNo.Text) + "');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

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
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
                if (iMenuId == 748)
                {
                    bindGrid("S", "x");
                    ModalPopUpExtender.Show();
                }

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
                dtDetails = (DataTable)Session["RFPPartDetail"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                string sQtyMsg = "";
                bDetailsRecordExist = true;
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                        if (txtPartID.Text != "" && txtPartID.Text != "0")
                        {
                            for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                            {
                                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                                {
                                    iCntForSelect = iCntForSelect + 1;
                                    if (Chk.Checked == true)
                                    {
                                        dtDetails.Rows[iMRowCnt]["DocStatus"] = "D";
                                        iCntForDelete++;
                                    }
                                    else
                                    {
                                        dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);

                                        //PartNo 
                                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                                        dtDetails.Rows[iMRowCnt]["Part_No"] = txtPartNo.Text;

                                        //Part Name
                                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                                        dtDetails.Rows[iMRowCnt]["Part_Name"] = txtPartName.Text;

                                        // Get Qty
                                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                        dtDetails.Rows[iMRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                        // Get Rate
                                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                        dtDetails.Rows[iMRowCnt]["Rate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                                        //MOQ
                                        TextBox txtMOQ = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMOQ");
                                        dtDetails.Rows[iMRowCnt]["MOQ"] = Func.Convert.dConvertToDouble(txtMOQ.Text);

                                        // Get Total
                                        TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                        dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                        // Get Status                           
                                        dtDetails.Rows[iMRowCnt]["DocStatus"] = txtStatus.Text;
                                    }// END Else
                                }
                            }// END FOR
                        }// END IF

                    }//END For

                    //Validdation
                    // New Validation Here Start
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        double dRFP_Qty = 0.00;
                        string sSrNo = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("lblNo") as Label).Text.Trim());
                        string sPartID = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                        string siRowCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                        if (sPartID != "" && sPartID != "0" && siRowCntStatus != "D")
                        {
                            dRFP_Qty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);
                            if (dRFP_Qty == 0 && siRowCntStatus != "D")// Check Return Quantity
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please Enter RFP Quantity at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                        }
                    }
                    //END Validation

                    //for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    //{
                    //    if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["DocStatus"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["DocStatus"]) != "D")
                    //    {
                    //        iCntError = iCntError + 1;
                    //        sQtyMsg = "Please enter RFP Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
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
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("DocStatus", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Is_Superceded", typeof(String)));

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
                dr["Qty"] = 0.00;
                dr["MOQ"] = 1;
                dr["Rate"] = 0.00;
                dr["Total"] = 0.00;
                dr["DocStatus"] = "N";
                dr["Is_Superceded"] = "N";
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
                if (Session["RFPPartDetail"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["RFPPartDetail"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["RFPPartDetail"];
                    if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                        CreateNewRowToDetailsTable();
                    dtDetails = (DataTable)Session["RFPPartDetail"];
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
                string sGroup_Code = "";
                double dTotalQty = 0.00;
                double dTotal = 0.00;
                double dPartTotal = 0.00;
                double dPartQty = 0.00;
                double dPartRate = 0.00;
                string sPartId = "0";

                hdnSelectedPartID.Value = "";
                string sPartNo = "";

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    TextBox txtLTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                    TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtMOQ = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMOQ");

                    sPartNo = txtPartNo.Text;
                    sPartId = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);//Part_ID
                    sGroup_Code = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Group_Code"]);
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["DocStatus"]);
                    sIS_PartSupercded = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Is_Superceded"]);

                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowMultiPartSearchForRFP(this,'" + iDealerID + "','" + Func.Convert.iConvertToInt(DrpSupplier.SelectedValue) + "');");


                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {

                        if (sPartNo != "" && sPartId != "0")
                        {
                            dPartQty = 0.00;
                            dPartRate = 0.00;

                            lnkSelectPart.Style.Add("display", "none");
                            hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + sPartId;
                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Rate"]);
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);

                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                            dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                        }
                        else
                        {
                            lnkSelectPart.Style.Add("display", (iMenuId == 748) ? "" : "none");
                            txtPartNo.Style.Add("display", "none");
                            txtPartName.Style.Add("display", "none");
                            txtMOQ.Style.Add("display", "none");
                            txtMRPRate.Style.Add("display", "none");
                            txtQuantity.Style.Add("display", "none");
                            txtLTotal.Style.Add("display", "none");
                        }
                    }

                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");

                    if (!Chk.Checked && sRecordStatus != "N")
                    {
                        iTotalLineItem += 1;
                    }

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
                    if (txtUserType.Text == "6")
                    {
                        lnkSelectPart.Style.Add("display", "none");
                    }
                }
                txtAllTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");
                txtTotalLineItem.Text = iTotalLineItem.ToString("0");

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        #region Excel Uploading Block
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ObjPartsRFP = new clsPartsRFP();
            DataSet dsUploadPartDetails;
            try
            {
                if (FileUpload1.HasFile)
                {
                  
                    string Ext = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    if (Ext == ".xls" || Ext == ".xlsx")
                    {
                        lblMessage.Visible = false;
                        string Name = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        string NewName = "PartsRFP" + "_" + iDealerID + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                        string FolderPath = "~/DownloadFiles/PartsRFP/";
                        string FilePath = Server.MapPath(FolderPath + NewName);
                        FileUpload1.SaveAs(FilePath);
                        System.Threading.Thread.Sleep(5000);
                        clsCommon objcomm = new clsCommon();
                        DataTable dtParts = objcomm.FillDatatableFromExcelSheet(FilePath, Ext, "");
                        dsUploadPartDetails = ObjPartsRFP.UploadPartDetailsAndGetPartDetails(Name, iDealerID, Func.Convert.iConvertToInt(DrpSupplier.SelectedValue), dtParts, iUserId);
                        if (dsUploadPartDetails == null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);
                            return;
                        }
                        else
                        {
                            SetListOfNotUploadedPartNo(dsUploadPartDetails.Tables[0]);
                            dtDetails = dsUploadPartDetails.Tables[1];
                        }
                        DataRow dr;
                        dr = dtDetails.NewRow();
                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["Part_ID"] = 0;
                        dr["Part_No"] = "";
                        dr["Part_Name"] = "";
                        dr["Qty"] = 0.00;
                        dr["MOQ"] = 1;
                        dr["Rate"] = 0.00;
                        dr["Total"] = 0.00;
                        dr["DocStatus"] = "N";
                        dr["Is_Superceded"] = "N";
                        dtDetails.Rows.InsertAt(dr, 0);
                        dtDetails.AcceptChanges();
                        Session["RFPPartDetail"] = dtDetails;
                        BindDataToGrid();
                        CPEPartDetails.Collapsed = false;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Please upload valid Excel File";
                        PartGrid.DataSource = null;
                        PartGrid.DataBind();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
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
        #endregion
        #region Supplier Selection
        protected void DrpSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCreatedBy();
            DataSet dschg = new DataSet();

            ObjPartsRFP = new clsPartsRFP();
            dschg = ObjPartsRFP.GetPartsRFP(iRFPID, "New", iDealerID, iMenuId);

            if (dschg != null) // if no Data Exist
            {
                if (dschg.Tables.Count > 0)
                {
                    Session["RFPPartDetail"] = null;

                    dtDetails = dschg.Tables[1];
                    Session["RFPPartDetail"] = dtDetails;

                    BindDataToGrid();
                }
            }
            txtID.Text = "";
            clsCommon ObjComm = new clsCommon();
            txtRFPNo.Text = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, Location.iDealerId, "RFP", ""));
            txtRFPDate.Text = Func.Common.sGetCurrentDate(1, false);
            UploafFile.Style.Add("display", txtUserType.Text.Trim() == "6" ? "none" : "");
            ObjPartsRFP = null;
            dschg = null;
            hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "RFP" + "/" + iMenuId;


        }
        #endregion
        int i = 1;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < PartGrid.Columns.Count; i++)
                {
                    e.Row.Cells[i].ToolTip = PartGrid.Columns[i].HeaderText;
                }

                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                string txtPartSupercded = (e.Row.FindControl("txtPartSupercded") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);

                int no = e.Row.RowIndex;
                if (txtPartID != "0")
                {
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                }
                if (txtPartSupercded == "Y")
                {
                    e.Row.BackColor = System.Drawing.Color.Aqua;
                }
            }
        }
        #region Code For Select Part PopUp Window
        private void BindDataAndDisplay()
        {
            try
            {
                int idataCount = 0;
                string[] asd;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                dtDetails = (DataTable)Session["RFPPartDetail"];
                idataCount = dtDetails.Rows.Count;
                sSourchFrom = "RFPPartDetail";

                dtTemp = dtDetails.Clone();


                // Get first row from table dtDetails,Previously it was last row which is above commented by Shyamal,Changed by Shyamal on 11062012
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
                    dtDetails.Columns.Add(new DataColumn("Group_Code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("DocStatus", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Is_Superceded", typeof(String)));
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
                        dr["Group_Code"] = Func.Convert.iConvertToInt(myArray[6]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[4]);
                        dr["Qty"] = Func.Convert.dConvertToDouble(myArray[2]);
                        dr["MOQ"] = Func.Convert.dConvertToDouble(myArray[2]);
                        dr["Rate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["Total"] = Convert.ToDouble(Func.Convert.sConvertToString(myArray[1])) * Convert.ToDouble(Func.Convert.sConvertToString(myArray[2]));
                        dr["DocStatus"] = "U";
                        dr["Is_Superceded"] = "N";
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }
                //No need to Add row again
                //dtDetails.ImportRow(dtTemp.Rows[0]);
                if (dtDetails.Rows.Count == 0 && dtTemp.Rows.Count != 0)
                    dtDetails.ImportRow(dtTemp.Rows[0]);
                Session[sSourchFrom] = dtDetails;

                FillDetailsFromGrid(false);
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
                BindDataAndDisplay();
                if (txtPopSearch.Text != "" && btnSave.Text == "Search")
                    btnSave.Text = "ClearSearch";
                else if (txtPopSearch.Text != "" && btnSave.Text == "ClearSearch")
                {
                    txtPopSearch.Text = "";
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
            bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtPopSearch.Text.Trim()));
            ModalPopUpExtender.Show();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPopSearch.Text != "" && btnSave.Text == "Search")
                    btnSave.Text = "ClearSearch";
                else if (txtPopSearch.Text != "" && btnSave.Text == "ClearSearch")
                {
                    txtPopSearch.Text = "";
                    btnSave.Text = "Search";
                }
                // BindDataAndDisplay
                BindDataAndDisplay();

                PagerV2_1.ItemCount = 10;
                PagerV2_1.CurrentIndex = 0;
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtPopSearch.Text.Trim()));
                //Show Model Popup
                ModalPopUpExtender.Show();
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

                string sSelectedPartID = "";
                if (Func.Convert.sConvertToString(txtPopSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtPopSearch.Text.Trim());
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }

                sSelectedPartID = hdnSelectedPartID.Value;
                string sTransFrom = "PartsRFP";
                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }
                object[] ParaValues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerID, Func.Convert.iConvertToInt(DrpSupplier.SelectedValue), iUserId, sTransFrom, sSelectedPartID };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_PartDetailsWithMRPRate_GETPaging", ParaValues);

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
    }
}