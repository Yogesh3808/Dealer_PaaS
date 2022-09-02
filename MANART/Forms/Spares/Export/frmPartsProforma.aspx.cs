using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using MANART.WebParts;

namespace MANART.Forms.Spares.Export
{
    public partial class frmPartsProforma : System.Web.UI.Page
    {
        private int iProfID = 0;
        string sDealerCode = "";
        private DataTable dtDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsPartsProforma objPartsProf = null;
        string sNew = "Y";
        int iUserId = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        int iTotalLineItem = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 10;
                ToolbarC.iValidationIdForConfirm = 10;
                ToolbarC.bUseImgOrButton = true;
                //ToolbarC.iFormIdToOpenForm = 222;
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);// MD User
                if (!IsPostBack)
                {
                    Session["ProfPartDetail"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Parts Proforma List";

                if (iProfID != 0)
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
                Session["ProfPartDetail"] = null;
                DisplayPreviousRecord();
            }
        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
            //DrpSupplier.SelectedValue = "0";

            Func.Common.BindDataToCombo(DrpProfomaInvoice, clsCommon.ComboQueryType.ProformaInvExport, Location.iDealerId, " and ISNULL(Is_Uploaded,'N')='N' ");
            DrpProfomaInvoice.SelectedValue = "0";
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iProfID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objPartsProf = new clsPartsProforma();
                if (iProfID != 0)
                {
                    ds = objPartsProf.GetPartsProformaInvoice(iProfID, "All", iDealerID, Func.Convert.sConvertToString(DrpProfomaInvoice.SelectedItem.Text));
                    sNew = "N";
                    DisplayData(ds);
                    objPartsProf = null;
                }
                else
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objPartsProf = null;
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
                objPartsProf = new clsPartsProforma();
                ds = objPartsProf.GetPartsProformaInvoice(iProfID, "New", iDealerID, "--Select--");
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
                txtProfNo.Text = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, Location.iDealerId, "PIA", ""));
                txtProfDate.Text = Func.Common.sGetCurrentDate(1, false);
                ObjComm = null;
                objPartsProf = null;
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

                txtProfNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prof_No"]);
                txtProfDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prof_Date"]);
                txtRemaks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remark"]);
                txtProfInvDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prof_Inv_Date"]);

                if (sNew == "N")
                {
                    Func.Common.BindDataToCombo(DrpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " or ISNULL(Active,'N')='N' ");
                    Func.Common.BindDataToCombo(DrpProfomaInvoice, clsCommon.ComboQueryType.ProformaInvExport, Location.iDealerId, "");
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " and ISNULL(Active,'N')='Y' ");
                    Func.Common.BindDataToCombo(DrpProfomaInvoice, clsCommon.ComboQueryType.ProformaInvExport, Location.iDealerId, " and ISNULL(Is_Uploaded,'N')='N' ");
                }

                //DrpSupplier.SelectedValue = "0";
                DrpProfomaInvoice.SelectedValue = "0";
                DrpSupplier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_ID"]);
                DrpProfomaInvoice.SelectedItem.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prof_Inv_No"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                DrpSupplier.Enabled = false;
                DrpProfomaInvoice.Enabled = (sNew == "N") ? false : true;

                Session["ProfPartDetail"] = null;

                dtDetails = ds.Tables[1];
                Session["ProfPartDetail"] = dtDetails;
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
                    DrpProfomaInvoice.Enabled = false;
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
            txtProfDate.Enabled = false;
            txtProfInvDate.Enabled = false;
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

                objPartsProf = new clsPartsProforma();
                ds = objPartsProf.GetPartsProformaInvoice(iProfID, "Max", iDealerID, "--Select--");
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
                    Session["ProfPartDetail"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objPartsProf = null;
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
                    FillCombo();
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
                iProfID = Func.Convert.iConvertToInt(txtID.Text);
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
                SearchGrid.AddToSearchCombo("Prof No");
                SearchGrid.AddToSearchCombo("Prof Date");
                SearchGrid.AddToSearchCombo("Prof Status");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sModelPart = "";
                SearchGrid.sSqlFor = "PartsProforma";
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
                dtHdr.Columns.Add(new DataColumn("Prof_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Prof_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Prof_Inv_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Prof_Inv_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Total", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Remark", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Total_Qty", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Total_Line_Items", typeof(int)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Prof_No"] = txtProfNo.Text;
                dr["Prof_Date"] = txtProfDate.Text;
                dr["Dealer_ID"] = iDealerID;
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(DrpSupplier.SelectedValue);
                dr["Prof_Inv_No"] = Func.Convert.sConvertToString(DrpProfomaInvoice.SelectedItem.Text.Trim());
                dr["Prof_Inv_Date"] = txtProfInvDate.Text;
                dr["Total"] = Func.Convert.dConvertToDouble(txtAllTotal.Text);
                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["UserId"] = iUserId;
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);
                dr["Remark"] = txtRemaks.Text.Trim();
                dr["Total_Qty"] = Func.Convert.iConvertToInt(txtTotalQty.Text);
                dr["Total_Line_Items"] = Func.Convert.iConvertToInt(txtTotalLineItem.Text);
                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {

            string sMessage = "";
            bool bValidateRecord = true;
            if (txtProfDate.Text == "")
            {
                sMessage = sMessage + "\\n *Enter the document date.";
                bValidateRecord = false;
            }
            if (DrpProfomaInvoice.SelectedItem.Text == "--Select--")
            {
                sMessage = sMessage + "\\n *Please select the Proforma Invoice.";
                bValidateRecord = false;
            }
            //if (txtRemaks.Text.Trim() == "" && (bSaveWithConfirm == true || bSaveWithCancel == true))
            //{
            //    sMessage = sMessage + "\\n *Please enter Remark.";
            //    bValidateRecord = false;
            //}
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsPartsProforma objPartsProf = new clsPartsProforma();
            try
            {
                bool bEnblSave = false;
                if (bSaveWithConfirm == false && bSaveWithCancel == false)
                {
                    bEnblSave = true;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                if (bValidateRecord(bSaveWithConfirm, bSaveWithCancel) == false)
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

                if (objPartsProf.bSaveRecordWithPart(sDealerCode, iDealerID, dtHdr, dtDetails, ref iProfID, iUserId) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iProfID);
                    string _result = "", _newResult = "";
                    if (bSaveWithConfirm == true)
                    {
                        _result = POCreation();
                        _newResult = txtProfNo.Text + " ." + " and " + _result;
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Proforma ") + "','" + Server.HtmlEncode(_newResult) + "');</script>");
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Proforma ") + "','" + Server.HtmlEncode(txtProfNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Proforma ") + "','" + Server.HtmlEncode(txtProfNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Proforma ") + "','" + Server.HtmlEncode(txtProfNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Proforma ") + "','" + Server.HtmlEncode(txtProfNo.Text) + "');</script>");
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

        #region PO Creation After Profirma Invoice Confirm
        private string POCreation()
        {
            clsSparePO objPO = new clsSparePO();
            clsPartsProforma objPartsProf = new clsPartsProforma();
            DataSet ds = new DataSet();
            int iPOID = 0;
            try
            {
                ds = objPartsProf.GeneratePO(Func.Convert.iConvertToInt(txtID.Text.Trim()), "P_RFPPO", iDealerID, "N", 6);

                if (ds.Tables[1].Rows.Count == 0)
                {
                    return "No Details For RFP PO Creation.";
                }
                else
                {
                    DataTable dtPOHdr = new DataTable();
                    DataTable dtPODtl = new DataTable();
                    POUpdateHdrValueFromControl(dtPOHdr);
                    dtPODtl = ds.Tables[1];

                    if (objPartsProf.bSaveRecordWithPartForRFPPO(sDealerCode, iDealerID, dtPOHdr, dtPODtl, ref iPOID, 0) == true)
                    {
                        return "RFP PO " + Func.Convert.sConvertToString(dtPOHdr.Rows[0]["PO_No"]) + " Created";
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(14,'" + Server.HtmlEncode("Parts PO") + "','" + Server.HtmlEncode(dtPOHdr.Rows[0]["PO_No"].ToString()) + "');</script>");
                        //return true;
                    }
                    else
                    {
                        return "RFP PO Not Created, Please Contact to Administrator";
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(15,'" + Server.HtmlEncode("Parts PO") + "','" + Server.HtmlEncode(dtPOHdr.Rows[0]["PO_No"].ToString()) + "');</script>");
                        //return false;
                    }

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return "PO Not Created, Please Contact to Administrator";
            }



        }

        private void POUpdateHdrValueFromControl(DataTable dtPOHdr)
        {
            try
            {
                clsSparePO objPO = new clsSparePO();
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
                clsCommon ObjComm = new clsCommon();
                dr["PO_No"] = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, Location.iDealerId, "RFPPO", ""));
                dr["PO_Date"] = Func.Common.sGetCurrentDate(1, false); ;
                dr["Dealer_ID"] = iDealerID;
                dr["Po_Type_ID"] = 6;
                dr["PO_Confirm"] = "Y";
                dr["PO_Cancel"] = "N";
                dr["PO_Total"] = Func.Convert.dConvertToDouble(txtAllTotal.Text);
                dr["PO_TotalQty"] = Func.Convert.iConvertToInt(txtTotalQty.Text);
                dr["PO_TotalItems"] = Func.Convert.iConvertToInt(txtTotalLineItem.Text);
                dr["PO_CreatedBy"] = "Part RFP PO Created By " + sDealerCode;
                dr["Chassis_No"] = "";
                dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetManufSuppID, iDealerID, " and HOBr_ID=" + Session["HOBR_ID"].ToString())); //MTI 18 --
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

        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {

                dtDetails = (DataTable)Session["ProfPartDetail"];

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
                        TextBox txtRFPDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRFPDetID");
                        TextBox txtProfInvDetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtProfInvDetID");
                        TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                        if (txtPartID.Text != "" && txtPartID.Text != "0")
                        {
                            for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                            {
                                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["RFP_det_ID"]) == Func.Convert.iConvertToInt(txtRFPDetID.Text)
                                    && Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["ProfInv_Det_ID"]) == Func.Convert.iConvertToInt(txtProfInvDetID.Text))
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
                                        dtDetails.Rows[iMRowCnt]["RFP_det_ID"] = Func.Convert.iConvertToInt(txtRFPDetID.Text);
                                        dtDetails.Rows[iMRowCnt]["ProfInv_Det_ID"] = Func.Convert.iConvertToInt(txtProfInvDetID.Text);

                                        //PartNo 
                                        TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                                        dtDetails.Rows[iMRowCnt]["Part_No"] = txtPartNo.Text.Trim();

                                        //Part Name
                                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                                        dtDetails.Rows[iMRowCnt]["Part_Name"] = txtPartName.Text.Trim();

                                        // Get Prof Qty
                                        TextBox txtProfQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtProfQty");
                                        dtDetails.Rows[iMRowCnt]["Prof_Qty"] = Func.Convert.dConvertToDouble(txtProfQty.Text);

                                        //Get Acc Qty
                                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                        dtDetails.Rows[iMRowCnt]["Acc_Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                        // Get Rate
                                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                        dtDetails.Rows[iMRowCnt]["Rate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                                        // Get Total
                                        TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                        dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                        // Get RFP No
                                        TextBox txtRFP_No = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRFP_No");
                                        dtDetails.Rows[iMRowCnt]["RFP_No"] = txtRFP_No.Text;

                                        // Get SAP Order No
                                        TextBox txtSAPOrderNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtSAPOrderNo");
                                        dtDetails.Rows[iMRowCnt]["SAP_Order_No"] = txtSAPOrderNo.Text;

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
                        double dAcc_qty = 0.00;
                        double dProf_qty = 0.00;
                        string sSrNo = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("lblNo") as Label).Text.Trim());
                        string sPartID = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                        string siRowCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                        CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                        if (sPartID != "" && sPartID != "0" && siRowCntStatus != "D")
                        {
                            dProf_qty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtProfQty") as TextBox).Text);
                            dAcc_qty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);
                            if (dAcc_qty == 0 && siRowCntStatus != "D")
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please Enter Proforma Accept Quantity at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                            if (dAcc_qty > dProf_qty && siRowCntStatus != "D")
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Accept Qunatity should not greater than Proforma Quantity at Row No " + sSrNo;
                                bDetailsRecordExist = false; break;
                            }
                        }
                    }
                    //END Validation


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

        private void BindDataToGrid()
        {
            try
            {

                if (Session["ProfPartDetail"] == null)
                {
                    Session["ProfPartDetail"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["ProfPartDetail"];
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
                string sGroup_Code = "";
                double dTotalQty = 0.00;
                double dTotal = 0.00;
                double dPartTotal = 0.00;
                double dPartQty = 0.00;
                double dPartRate = 0.00;
                string sPartId = "0";
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
                    TextBox txtProfQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtProfQty");
                    TextBox txtRFP_No = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRFP_No");

                    sPartNo = txtPartNo.Text;
                    sPartId = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);//Part_ID
                    sGroup_Code = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Group_Code"]);
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["DocStatus"]);

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {

                        if (sPartNo != "" && sPartId != "0")
                        {
                            dPartQty = 0.00;
                            dPartRate = 0.00;

                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Acc_Qty"]);
                            dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Rate"]);
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);

                            dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                            dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Acc_Qty"]);
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                        }
                        else
                        {
                            txtPartNo.Style.Add("display", "none");
                            txtPartName.Style.Add("display", "none");
                            txtProfQty.Style.Add("display", "none");
                            txtMRPRate.Style.Add("display", "none");
                            txtQuantity.Style.Add("display", "none");
                            txtLTotal.Style.Add("display", "none");
                            txtRFP_No.Style.Add("display", "none");
                        }
                    }

                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");

                    if (!Chk.Checked && sRecordStatus != "N")
                    {
                        iTotalLineItem = iTotalLineItem + 1;
                    }

                    // if Status is N then Delete not show
                    Chk.Style.Add("display", (sRecordStatus == "N") ? "none" : "");
                    lblDelete.Style.Add("display", (sRecordStatus == "N") ? "none" : "");


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

        #region Supplier Selection
        protected void DrpProfomaInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dschg = new DataSet();

            objPartsProf = new clsPartsProforma();
            dschg = objPartsProf.GetPartsProformaInvoice(iProfID, "New", iDealerID, Func.Convert.sConvertToString(DrpProfomaInvoice.SelectedItem.Text));

            if (dschg != null) // if no Data Exist
            {
                if (dschg.Tables.Count > 0)
                {

                    dschg.Tables[0].Rows[0]["Is_Cancel"] = "N";
                    dschg.Tables[0].Rows[0]["Is_Confirm"] = "N";
                    dschg.Tables[1].Rows[0]["DocStatus"] = "E";
                    DrpSupplier.SelectedValue = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["Supplier_ID"]);
                    sNew = "Y";

                    DisplayData(dschg);
                }
            }
            txtID.Text = "";
            clsCommon ObjComm = new clsCommon();
            txtProfNo.Text = Func.Convert.sConvertToString(ObjComm.GenerateDocNo(sDealerCode, Location.iDealerId, "PIA", ""));
            txtProfDate.Text = Func.Common.sGetCurrentDate(1, false);
            hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "PIA" + "/" + Func.Convert.iConvertToInt(DrpSupplier.SelectedValue);
            ObjComm = null;
            objPartsProf = null;
            dschg = null;

        }
        #endregion
        #region Ganearate Serial Number for GridView
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
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                }
            }
        }
        #endregion

    }
}