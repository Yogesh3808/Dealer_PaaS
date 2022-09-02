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

namespace MANART.Forms.Spares
{
    public partial class frmSalesRetunNote : System.Web.UI.Page
    {
        private int iSalesRetNoteID = 0;
        private DataTable dtDetails = new DataTable();
        private DataTable dtSRNTaxDetails = new DataTable();
        private DataTable dtSRNGrpTaxDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsSpareSalesRetNote objSalesRetNote = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        int iHOBr_id = 0;
        private int iDealerID = 0;
        private bool sDiscChange = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 61;
                ToolbarC.iValidationIdForConfirm = 61;
                ToolbarC.bUseImgOrButton = true;

                ToolbarC.iFormIdToOpenForm = 105;
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                //For MD User
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    FillSelectionGrid();
                }

                if (!IsPostBack)
                {
                    Session["SalesRetPart"] = null;
                    Session["SRNGrpTaxDetails"] = null;
                    Session["SRNTaxDetails"] = null;
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Sales Return Note List";

                if (iSalesRetNoteID != 0)
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
        protected void Page_Load(object sender, EventArgs e)
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            // For MD user
            if (txtUserType.Text != "6")
            {
                FillSelectionGrid();
            }
            if (!IsPostBack)
            {
                Session["SalesRetPart"] = null;
                Session["SRNGrpTaxDetails"] = null;
                Session["SRNTaxDetails"] = null;
                FillCombo();
                GenerateSalesRetNoteNo();
                DisplayPreviousRecord();
                if (Func.Convert.iConvertToInt(Location.iDealerId) != 0)
                {
                    GenerateSalesRetNoteNo();
                }
            }

        }
        private void FillCombo()
        {
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "')");
            Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and ISNULL(CM.Active,'N')='Y'");
            DrpCustomer.SelectedValue = "0";

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iSalesRetNoteID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();
                objSalesRetNote = new clsSpareSalesRetNote();
                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);

                }


                if (iSalesRetNoteID != 0)
                {
                    ds = objSalesRetNote.GetSalesRetNote(iSalesRetNoteID, "All", iDealerID, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue), iUserId);
                    sNew = "N";
                    DisplayData(ds);
                    objSalesRetNote = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objSalesRetNote = null;
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
                objSalesRetNote = new clsSpareSalesRetNote();
                ds = objSalesRetNote.GetSalesRetNote(iSalesRetNoteID, "New", iDealerID, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue), iUserId);

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
                GenerateSalesRetNoteNo();
                txtSalesRetNoteDate.Text = Func.Common.sGetCurrentDate(1, false);

                objSalesRetNote = null;
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

                hdnIsOpenSRN.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SRN_Open"]);
                txtPrevOpenSrnNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrevOpenSrnNo"]);
                //if (hdnIsOpenSRN.Value == "Y")
                //{
                //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Confirm Previous SRN.');</script>");
                //    //return;
                //}

                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    FillCombo();
                }
                if (sNew == "N")
                {
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 ");
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and ISNULL(CM.Active,'N')='Y'");
                }
                DrpCustomer.SelectedValue = "0";

                txtSalesRetNoteNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SRN_No"]);
                txtSalesRetNoteDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SRN_Date"]);
                hdnDealerID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerID"]);
                DrpCustomer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                DrpCustomer.Enabled = (sNew == "N") ? false : true;
                txtSalesRetNoteDate.Enabled = false;
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PerAmt"]) == "Per")
                    rbtLstDiscount.SelectedValue = "Per";
                else
                    rbtLstDiscount.SelectedValue = "Amt";
                rbtLstDiscount.Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");

                Session["SalesRetPart"] = null;
                Session["SRNGrpTaxDetails"] = null;
                Session["SRNTaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["SalesRetPart"] = dtDetails;
                dtSRNGrpTaxDetails = ds.Tables[2];
                Session["SRNGrpTaxDetails"] = dtSRNGrpTaxDetails;
                dtSRNTaxDetails = ds.Tables[3];
                Session["SRNTaxDetails"] = dtSRNTaxDetails;
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
                    DrpCustomer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            PartGrid.Enabled = bEnable;
            GrdPartGroup.Enabled = bEnable;
            GrdDocTaxDet.Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }

        private void DisplayCurrentRecord()
        {
            try
            {
                DataSet ds = new DataSet();
                objSalesRetNote = new clsSpareSalesRetNote();
                ds = objSalesRetNote.GetSalesRetNote(iSalesRetNoteID, "Max", iDealerID, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue), iUserId);
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
                    Session["SalesRetPart"] = null;
                    Session["SRNGrpTaxDetails"] = null;
                    Session["SRNTaxDetails"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objSalesRetNote = null;
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

                    DisplayPreviousRecord();
                    if (Func.Convert.iConvertToInt(Location.iDealerId) != 0)
                    {
                        GenerateSalesRetNoteNo();
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
                iSalesRetNoteID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected override void OnPreRender(EventArgs e)
        {
            //For MD User
            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                FillSelectionGrid();
            }


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
                //For MD User
                if (txtUserType.Text == "6")
                {
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("SRN No");
                SearchGrid.AddToSearchCombo("SRN Date");
                SearchGrid.AddToSearchCombo("SRN Status");
                SearchGrid.iDealerID = Location.iDealerId;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUserId);
                SearchGrid.sSqlFor = "SparesSRN";
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
                dtHdr.Columns.Add(new DataColumn("SRN_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("SRN_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DealerID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("IS_PerAmt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PrevOpenSrnNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CustTaxTag", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["SRN_No"] = txtSalesRetNoteNo.Text;
                dr["SRN_Date"] = txtSalesRetNoteDate.Text;
                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["UserId"] = iUserId;
                dr["DealerID"] = iDealerID;
                dr["CustID"] = Func.Convert.iConvertToInt(DrpCustomer.SelectedValue);
                dr["IS_PerAmt"] = Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue);
                dr["PrevOpenSrnNo"] = txtPrevOpenSrnNo.Text;
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["CustTaxTag"] = Func.Convert.sConvertToString(hdnCustTaxTag.Value);
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
            if (txtSalesRetNoteDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (DrpCustomer.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select Customer.";
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
            clsSpareSalesRetNote objSalesRetNote = new clsSpareSalesRetNote();
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

                if (objSalesRetNote.bSaveRecord(Location.sDealerCode, dtHdr, dtDetails, dtSRNGrpTaxDetails, dtSRNTaxDetails, ref iSalesRetNoteID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iSalesRetNoteID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts SRN ") + "','" + Server.HtmlEncode(txtSalesRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts SRN ") + "','" + Server.HtmlEncode(txtSalesRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts SRN ") + "','" + Server.HtmlEncode(txtSalesRetNoteNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts SRN ") + "','" + Server.HtmlEncode(txtSalesRetNoteNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        private void GenerateSalesRetNoteNo()
        {
            objSalesRetNote = new clsSpareSalesRetNote();
            txtSalesRetNoteNo.Text = Func.Convert.sConvertToString(objSalesRetNote.GenerateSalesRetNote(Location.sDealerCode, Location.iDealerId));
        }


        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            DataTable dtExistRecord = null;
            DataTable dtSysStock = null;
            StringBuilder sPartIDs = null;
            try
            {
                //objSalesRetNote = new clsSpareSalesRetNote();
                //dtSysStock = new DataTable();
                //dtExistRecord = new DataTable();
                //dtExistRecord = (DataTable)Session["SalesRetPart"];
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
                //    dtSysStock = objSalesRetNote.GetSysStock(Func.Convert.sConvertToString(sPartIDs), iUserId, "SRNAll", Location.iDealerId, Func.Convert.iConvertToInt(txtID.Text));
                //    if (dtSysStock != null && dtSysStock.Rows.Count > 0)
                //        Session["SalesRetPart"] = dtSysStock;
                //}

                FillDetailsFromGrid(false);
                BindDataToGrid();
                //new code
                FillDetailsFromGrid(false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["SalesRetPart"] = dtDetails;
                Session["SRNGrpTaxDetails"] = dtSRNGrpTaxDetails;
                Session["SRNTaxDetails"] = dtSRNTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objSalesRetNote != null) objSalesRetNote = null;
                if (dtSysStock != null) dtSysStock = null;
                if (dtExistRecord != null) dtExistRecord = null;
                if (sPartIDs != null) sPartIDs = null;
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                dtDetails = (DataTable)Session["SalesRetPart"];

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

                        if (txtPartID.Text != "" && txtPartID.Text != "0")
                        {
                            for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                            {
                                if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text)
                                 && Func.Convert.sConvertToString(dtDetails.Rows[iMRowCnt]["Invoice_No"]) == txtInvoiceNo.Text)
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
                                        TextBox txtStockQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStockQty");
                                        dtDetails.Rows[iMRowCnt]["Stock_Qty"] = Func.Convert.dConvertToDouble(txtStockQty.Text);
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
                                        TextBox txtDiscAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscAmt");
                                        dtDetails.Rows[iMRowCnt]["Disc_Amt"] = Func.Convert.dConvertToDouble(txtDiscAmt.Text);
                                        // Get Disc_Rate
                                        TextBox txtDiscRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscRate");
                                        dtDetails.Rows[iMRowCnt]["Disc_Rate"] = Func.Convert.dConvertToDouble(txtDiscRate.Text);
                                        // Get Disc_Rate
                                        TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                        dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

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

                    for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    {
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["PartTaxID"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please select Part Tax at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                            break;
                        }
                        else if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Ret_Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please Enter Return Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
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
                dtSRNGrpTaxDetails = (DataTable)(Session["SRNGrpTaxDetails"]);
                dtSRNTaxDetails = (DataTable)(Session["SRNTaxDetails"]);

                if (dtDetails.Rows.Count > 1 && dtSRNGrpTaxDetails.Rows.Count == 0)
                    CreateNewRowToTaxGroupDetailsTable();

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Net Reverse Amount
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //dtSRNGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(txtTax2Per.Text);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtSRNGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }
                //CreateNewRowToTaxGroupDetailsTable();
                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtSRNTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtSRNTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtSRNTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtSRNTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtSRNTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtSRNTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtSRNTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtSRNTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtSRNTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtSRNTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtSRNTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtSRNTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtSRNTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtSRNTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
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
                    dtDetails.Columns.Add(new DataColumn("Invoice_No", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Invoice_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Ret_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Stock_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Unit", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_amt", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("lab_tag", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Rev_Inv_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(string)));

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
                dr["Invoice_No"] = "";
                dr["Part_ID"] = 0;
                dr["part_no"] = "";
                dr["Part_Name"] = "";
                dr["Rate"] = 0.00;
                dr["Invoice_Qty"] = 0.00;
                dr["Ret_Qty"] = 0.00;
                dr["Rev_Inv_Qty"] = 0.00;
                dr["Stock_Qty"] = 0.00;
                dr["group_code"] = "";
                dr["Unit"] = "";
                dr["Price"] = 0.00;
                dr["disc_per"] = 0.00;
                dr["disc_rate"] = 0.00;
                dr["disc_amt"] = 0.00;
                dr["Total"] = 0.00;

                dr["Status"] = "U";
                if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";

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
                if (Session["SalesRetPart"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["SalesRetPart"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["SalesRetPart"];
                    if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                        CreateNewRowToDetailsTable();
                    dtDetails = (DataTable)Session["SalesRetPart"];

                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();

                //if (Session["SRNGrpTaxDetails"] == null)
                //    Session["SRNGrpTaxDetails"] = dtSRNGrpTaxDetails;
                //}
                //else
                //    dtSRNGrpTaxDetails = (DataTable)Session["SRNGrpTaxDetails"];

                GrdPartGroup.DataSource = dtSRNGrpTaxDetails;
                GrdPartGroup.DataBind();

                //if (Session["SRNTaxDetails"] == null)
                //    Session["SRNTaxDetails"] = dtSRNTaxDetails;
                //else
                //    dtSRNTaxDetails = (DataTable)Session["SRNTaxDetails"];

                GrdDocTaxDet.DataSource = dtSRNTaxDetails;
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
                double dInvQty = 0;
                double dRetQty = 0;
                double dPartRate = 0;
                double dDiscPer = 0;
                double dDiscRate = 0;
                double dPartMrpPrice = 0;
                double dRevMainTaxRate = 0;
                double dDiscRevRate = 0;

                //Sujata 05092014
                double dExclPartDiscRate = 0;
                double dExclPartTotal = 0;
                double dExclTotal = 0;

                double dPartTax = 0;
                double dPartTax1 = 0;
                double dPartTax2 = 0;
                //Sujata 05092014

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
                string sPartID = "", sPartName = "", cPartID = "";
                string sGroupCode = "";
                int iPartID = 0;
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        TextBox txtInvoiceNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInvoiceNo");
                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        cPartID = txtPartNo1.Text;
                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");

                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");// Inv Qty
                        TextBox txtOrgInvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOrgInvQty");//REv Qty

                        TextBox txtReturnQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtReturnQty");// Ret Qty

                        TextBox txtStockQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStockQty");
                        TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                        TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                        TextBox txtRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRate");
                        TextBox txtDiscPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscPer");
                        TextBox txtDiscAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscAmt");
                        TextBox txtDiscRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscRate");
                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");

                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();
                        //GST Relates Work Part Tax
                        PartGrid.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                        //Main Tax
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");

                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);


                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        //Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                        TextBox txtExclDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExclDiscountRate");
                        TextBox TxtExclTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("TxtExclTotal");

                        TextBox txtlabtag = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtlabtag");

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

                        //Status
                        sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);


                        if (sGroupCode == "02")
                            drpPartTax.Enabled = false;
                        else
                            drpPartTax.Enabled = true;

                        //if ((sGroupCode != "L" && hdnMenuID.Value == "546") || (sGroupCode == "L" && hdnMenuID.Value == "687"))
                        //{
                        //    txtDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                        //    txtDiscountPer.Attributes.Add("onblur", "return CalculateSRNPartTotal(event,this);");
                        //}
                        //else
                        //{
                        //    txtDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        //}
                        if (sGroupCode == "99")
                        {
                            txtRate.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtRate.Attributes.Add("onblur", "return CalculateSRNPartTotal(event,this);");
                        }
                        else
                            txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");



                        if (cPartID != "")
                        {
                            dInvQty = 0;
                            dRetQty = 0;
                            dPartRate = 0;
                            dDiscRate = 0;
                            dDiscPer = 0;
                            dPartMrpPrice = 0;
                            dDiscRevRate = 0;

                            dInvQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Invoice_Qty"]);
                            dRetQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Ret_Qty"]);
                            //Reverse Rate Calulation for System Part
                            dPartMrpPrice = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);

                            //dPartTax = Math.Round(dPartMrpPrice * Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / (100 + Func.Convert.dConvertToDouble(txtPartTaxPer.Text)), 2);
                            //if (sGroupCode == "01")
                            // dPartRate = dPartMrpPrice - dPartTax;
                            clsDB objDB = new clsDB();
                            if (sGroupCode == "01" || sGroupCode == "02")
                                dPartRate = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", dPartMrpPrice, Func.Convert.iConvertToInt(drpPartTax.SelectedValue));
                            else
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);
                            // Set Reverse Calculated Rate to Rate Column
                            txtRate.Text = Func.Convert.sConvertToString(dPartRate);

                            dDiscPer = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["disc_per"]);
                            dDiscRate = Func.Convert.dConvertToDouble(dPartRate * (dDiscPer / 100));
                            dPartRate = Func.Convert.dConvertToDouble(dPartRate - dDiscRate);

                            if (sRecordStatus != "C")
                                dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Ret_Qty"]);

                            txtlabtag.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["lab_tag"]).Trim();
                            txtDiscAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dDiscRate));
                            txtDiscRate.Text = Func.Convert.sConvertToString(dPartRate);
                            dPartTotal = Func.Convert.dConvertToDouble(dRetQty * dPartRate);
                            txtPrtTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dPartTotal, 2)));
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dPartTotal);

                            //New Code for Reverse rate Calculation on 02052017_Vikram Begin
                            //if (sGroupCode == "01" || sGroupCode == "02")
                            //{
                            //    dRevMainTaxRate = dPartMrpPrice / (1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));
                            //    dDiscRevRate = Func.Convert.dConvertToDouble(dRevMainTaxRate * dDiscPer / 100);
                            //    txtExclDiscountRate.Text = Func.Convert.sConvertToString(Math.Round(dDiscRevRate, 2));
                            //    dRevMainTaxRate = dRevMainTaxRate - dDiscRevRate;
                            //    dExclPartTotal = Func.Convert.dConvertToDouble(dRetQty * dRevMainTaxRate);
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}
                            //else
                            //{
                            //    dExclPartTotal = dPartTotal;
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}
                            dExclPartTotal = dPartTotal;
                            TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            dExclTotal = dExclTotal + dExclPartTotal;

                        }

                    }


                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowINVMultiPartSearch(this,'" + sDealerId + "','" + DrpCustomer.ID + "'," + iHOBr_id + ",'" + hdnIsOpenSRN.Value + "');");

                    iPartID = Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    lblDelete.Style.Add("display", "none");

                    if (sPartID == "0")//&& hdnIsOpenSRN.Value == "N"
                    {
                        lnkSelectPart.Style.Add("display", "");
                        Chk.Style.Add("display", "none");
                        lblDelete.Style.Add("display", "none");
                    }
                    else
                        lnkSelectPart.Style.Add("display", "none");

                    if (txtGPartName.Text == "")//&& hdnIsOpenSRN.Value == "N"
                    {
                        lnkSelectPart.Style.Add("display", "");
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                    {
                        lnkSelectPart.Style.Add("display", "none");
                        txtPartNo.Style.Add("display", "");
                    }

                    //If Part Id  is not allocated           
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        lblDelete.Style.Add("display", "");
                    }
                    else if (sRecordStatus == "C")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");
                    }
                    else if (sRecordStatus == "N" || (sPartID != "0" && sRecordStatus == "U"))
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");
                    }
                    if (txtUserType.Text.Trim() == "6")
                    {
                        lnkSelectPart.Style.Add("display", "none");
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
                    if (sDiscChange == true)
                    {
                        txtGrDiscountPer.Text = Func.Convert.sConvertToString(0.00);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString(0.00);
                    }
                    //if ((srowGRPID != "L" && hdnMenuID.Value == "546" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per") ||
                    //    (srowGRPID == "L" && hdnMenuID.Value == "687" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per"))
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if ((srowGRPID != "L" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per"))
                        {
                            txtGrDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountPer.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");
                            txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        else
                        {
                            txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountAmt.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");
                        }
                    }

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");


                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' ");

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");


                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    //if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                    //    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    //else
                    //    Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    // Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    //Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    //Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
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
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //txtGrnetrevamt.Text = "0";
                }

                for (int i = 0; i < PartGrid.Rows.Count; i++)
                {
                    TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                    TextBox TxtExclTotal = (TextBox)PartGrid.Rows[i].FindControl("TxtExclTotal");
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");

                    if (txtGrNo.Text.Trim() != "")
                    {
                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                        //TotalRev = Math.Round(TotalRev + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0)
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
                        dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                        dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));
                    }
                    else
                    {
                        dGrpDiscPer = 0.00;
                        dGrpDiscAmt = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);
                    }

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
                    //Vikram Begin_09052017
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    //dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    //End
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
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFPer.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");

                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                        }
                        else
                        {
                            txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFAmt.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");

                            //dDocPFPer = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_per"]), 2);
                            //dDocPFAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_amt"]), 2);
                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(txtPFAmt.Text);
                        }
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherPer.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100)), 2);
                        }
                        else
                        {
                            txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherAmt.Attributes.Add("onblur", "return CalulateSRNPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            //dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["other_money"]), 2);
                            dDocOtherAmt = Func.Convert.dConvertToDouble(txtOtherAmt.Text);
                        }
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

        protected void PartGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dtDetails = (DataTable)Session["SalesRetPart"];
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
            Session["SalesRetPart"] = dtDetails;
            Session["SRNGrpTaxDetails"] = dtSRNGrpTaxDetails;
            Session["SRNTaxDetails"] = dtSRNTaxDetails;
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
                dtSRNGrpTaxDetails.Clear();
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
                    for (int iRCnt = 0; iRCnt < dtSRNGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtSRNGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtSRNGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0 && txtStatus.Text != "D" && txtStatus.Text != "C")
                    {
                        dr = dtSRNGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        if (sGrCode.Trim() == "01")
                            dr["Gr_Name"] = "Parts";
                        else if (sGrCode.Trim() == "02")
                            dr["Gr_Name"] = "Lubricant";
                        else
                            dr["Gr_Name"] = "Local Part";

                        dr["net_inv_amt"] = 0;
                        //dr["net_rev_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

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


                        dtSRNGrpTaxDetails.Rows.Add(dr);
                        dtSRNTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
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
                    i = 1;
                }
            }
        }
        protected void rbtLstDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnConfirm.Value == "N")
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

        protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayPreviousRecord();
        }

    }
}