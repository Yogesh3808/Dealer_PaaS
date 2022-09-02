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
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MANART.Forms.Spares
{
    public partial class frmStockTransferReceipt : System.Web.UI.Page
    {
        private int iStkReceiptID = 0;
        string sFileName = "";
        int iDealerID = 0;
        private DataTable dtDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsStkTransferReceipt objStkReceipt = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        int iMenuId = 0;
        int iHOBr_id = 0;
        string sDealerCode = "";
        string ChallanNo = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 7;
                ToolbarC.iValidationIdForConfirm = 7;
                ToolbarC.bUseImgOrButton = true;
                ExportLocation.bUseSpareDealerCode = true;
                ExportLocation.SetControlValue();

                clsSupplier ObjSup = new clsSupplier();
                DataSet dsDealer = new DataSet();
                DataSet ds = new DataSet();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (!IsPostBack)
                {
                    Session["StkTrnRPart"] = null;
                    DisplayPreviousRecord();
                    FillCombo();
                }
                SearchGrid.sGridPanelTitle = "Stock Transfer Receipt" ;

                if (iStkReceiptID != 0)
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
            ExportLocation.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
            ExportLocation.TypeSelectedIndexChanged += new EventHandler(Location_TypeSelectedIndexChanged);

           // iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());

            // Dealer MD User related Change 
            if (Session["iDealerID"] != null)
            {
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
            }
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["StkTrnRPart"] = null;
                DisplayPreviousRecord();
            }
        }
        protected void Location_TypeSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillCombo();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
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
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillSelectionGrid();
                Session["StkTrnRPart"] = null;
                DisplayPreviousRecord();
                //GenerateStockReceiptNo();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iStkReceiptID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objStkReceipt = new clsStkTransferReceipt();
                if (iStkReceiptID != 0)
                {
                    // For MD User
                    if (txtUserType.Text == "6")
                    {
                        ds = objStkReceipt.GetStockTrnReceipt(iStkReceiptID, "ALLMD", iDealerID, 0);
                    }
                    else
                    {
                        ds = objStkReceipt.GetStockTrnReceipt(iStkReceiptID, "All", iDealerID, 0);
                    }
                    sNew = "N";
                    DisplayData(ds);
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objStkReceipt = null;
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

                objStkReceipt = new clsStkTransferReceipt();
                ds = objStkReceipt.GetStockTrnReceipt(iStkReceiptID, "New", iDealerID, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Receipt_canc"] = "N";
                            ds.Tables[0].Rows[0]["Receipt_Confirm"] = "N";
                           // ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtReceiptNo.Text = Func.Convert.sConvertToString(objStkReceipt.GenerateStkReceiptNo(sDealerCode, iDealerID));
                txtReceiptDate.Text = Func.Common.sGetCurrentDate(1, false);
                objStkReceipt = null;
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
                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                    Session["iDealerID"] = iDealerID;

                }
                txtReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Receipt_No"]);
                txtReceiptDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Receipt_Date"]);
                hdnISDocGST.Value= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISDocGST"]);

                DropDownList drpDealerName = (DropDownList)ExportLocation.FindControl("drpDealerName");

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupplierID"]) == "0")
                {
                    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                    ExportLocation.SetControlValue();
                }
                else
                {
                    drpDealerName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupplierID"]);
                    ExportLocation.FillLocation();
                }
                
                
                if (iStkReceiptID != 0)
                {
                    drpChallanNo.Enabled = false;
                    drpChallanNo.CssClass = "NonEditableFields";
                    //Func.Common.BindDataToCombo(drpChallanNo, clsCommon.ComboQueryType.StkChallanNo, 0, "And ( CM.Dealer_Id=" + iDealerID + ") ");
                    Func.Common.BindDataToCombo(drpChallanNo, clsCommon.ComboQueryType.StkChallanNo, 0, "And CH.Challan_no in (select distinct Challan_No from TM_SPReceipt where TM_SPReceipt.Dealer_ID=" + iDealerID + ") And ( CM.Dealer_Id=" + iDealerID + ") ");
                    drpChallanNo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Challan_No"]);
                }
                else
                {
                    drpChallanNo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChallanID"]);
                    hdnChallanNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Challan_No"]);
                    drpChallanNo.Enabled = true;
                    drpChallanNo.CssClass = "ComboBoxFixedSize";
                }
                drpDealerName.Enabled = false;
                drpDealerName.CssClass = "NonEditableFields";
                
                Label lblMDealername = (Label)ExportLocation.FindControl("lblMDealername");
                lblMDealername.Visible = false;

                txtReceiptDate.Enabled = false;

                //txtnarration.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["narration"]);

                Session["StkTrnRPart"] = null;

                dtDetails = ds.Tables[1];
                Session["StkTrnRPart"] = dtDetails;
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Receipt_Confirm"]) == "Y")
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
                // If Record is Confirm or cancel then it is not editable            


                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Receipt_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Receipt_canc"]) == "Y")
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
                    Func.Common.BindDataToCombo(drpChallanNo, clsCommon.ComboQueryType.StkChallanNo, 0, "And CH.Challan_no in (select distinct Challan_No from TM_SPReceipt where TM_SPReceipt.Dealer_ID=" + iDealerID + ") And ( CM.Dealer_Id=" + iDealerID + ") ");
                    drpChallanNo.Enabled = false;
                    drpDealerName.Enabled = true;
                    drpDealerName.CssClass = "";
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
            //txtStkTrnChDate.Enabled = bEnable;

            PartGrid.Enabled = bEnable;
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
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
                objStkReceipt = new clsStkTransferReceipt();
                ds = objStkReceipt.GetStockTrnReceipt(iStkReceiptID, "Max", iDealerID,0);
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
                    Session["StkTrnRPart"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objStkReceipt = null;
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
                    FillCombo();
                    //GenerateStockReceiptNo();

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
                iStkReceiptID = Func.Convert.iConvertToInt(txtID.Text);
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
                SearchGrid.AddToSearchCombo("Receipt No");
                SearchGrid.AddToSearchCombo("Receipt Date");
                // SearchGrid.iDealerID = iDealerID;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iMenuId);
                // For MD user 

                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = iUserId;
                    SearchGrid.sSqlFor = "StockTrnReceiptMD";
                }
                else
                {
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sSqlFor = "StockTrnReceipt";
                }
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
                dtHdr.Columns.Add(new DataColumn("Receipt_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Receipt_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Challan_No", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("SupplierID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Receipt_Amt", typeof(double)));

                dtHdr.Columns.Add(new DataColumn("Receipt_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Receipt_canc", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("HOBR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("ISDocGST", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Receipt_No"] = txtReceiptNo.Text;
                dr["Receipt_Date"] = txtReceiptDate.Text;
                dr["Challan_No"] = drpChallanNo.SelectedItem.Text.Trim();
                dr["SupplierID"] = ExportLocation.iSupplierId.ToString();
                dr["Dealer_ID"] = iDealerID;
                //dr["UserId"] = iUserId;
                dr["Receipt_Confirm"] = "N";
                dr["Receipt_canc"] = "N";

                dr["Receipt_Amt"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                dr["HOBR_ID"] = Func.Convert.sConvertToString(Session["HOBR_ID"]);
                dr["ISDocGST"] = Func.Convert.sConvertToString(hdnISDocGST.Value);
                 

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
            if (txtReceiptNo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Receipt No.";
                bValidateRecord = false;
            }
            if (txtReceiptDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Receipt date.";
                bValidateRecord = false;
            }
            if (drpChallanNo.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Challan No.";
                bValidateRecord = false;
            }
            //if (DrpCustomer.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Please select the Customer.";
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
            clsStkTransferReceipt objStkReceipt = new clsStkTransferReceipt();
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
                    dtHdr.Rows[0]["Receipt_Confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Receipt_canc"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }
                if (objStkReceipt.bSaveRecord(sDealerCode, iDealerID, dtHdr, dtDetails, ref iStkReceiptID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iStkReceiptID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Receipt") + "','" + Server.HtmlEncode(txtReceiptNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Receipt") + "','" + Server.HtmlEncode(txtReceiptNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Receipt") + "','" + Server.HtmlEncode(txtReceiptNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Receipt") + "','" + Server.HtmlEncode(txtReceiptNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        private void GenerateStockReceiptNo()
        {
            objStkReceipt = new clsStkTransferReceipt();
            txtReceiptNo.Text = Func.Convert.sConvertToString(objStkReceipt.GenerateStkReceiptNo(Func.Convert.sConvertToString(Session["sDealerCode"]), iDealerID));
        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strStockReptNo = "";
                dtDetails = (DataTable)Session["StkTrnRPart"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                int iCntError1 = 0;
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
                            if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                            {
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "U")
                                    {
                                        if (strStockReptNo == "")
                                            strStockReptNo = iRowCnt.ToString();
                                        else
                                            strStockReptNo = strStockReptNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "O")
                                    {
                                        if (strStockReptNo == "")
                                            strStockReptNo = iRowCnt.ToString();
                                        else
                                            strStockReptNo = strStockReptNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "M")
                                    {
                                        if (strStockReptNo == "")
                                            strStockReptNo = iRowCnt.ToString();
                                        else
                                            strStockReptNo = strStockReptNo + "," + iRowCnt.ToString();
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

                                //// Get Sys Qty
                                //TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                                //dtDetails.Rows[iMRowCnt]["bal_qty"] = Func.Convert.dConvertToDouble(txtBalQty.Text);

                                // Get Challan Qty
                                TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                dtDetails.Rows[iMRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                // Get Received Qty
                                TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                                dtDetails.Rows[iMRowCnt]["Recv_Qty"] = Func.Convert.dConvertToDouble(txtRecvQty.Text);

                                TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                                TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text.Trim();

                                TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                                dtDetails.Rows[iMRowCnt]["BFRGST"] = txtBFRGST.Text.Trim();                                

                                if (txtStatus.Text == "C")
                                {
                                    //lblCancel.Attributes.Add("disabled", "true");

                                    //lblCancel.Text = "Cancelled";
                                }

                                CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                                sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                                //dtDetails.Rows[iRowCnt]["Status"] = "";            
                                if (Chk.Checked == true)
                                {
                                    dtDetails.Rows[iRowCnt]["Status"] = "D";
                                    iCntForDelete++;
                                }
                                else
                                {
                                    if (txtPartID.Text != "")
                                    {
                                        if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "M")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "M";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "U")
                                        {
                                            dtDetails.Rows[iRowCnt]["Status"] = "U";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
                                        {
                                            dtDetails.Rows[iRowCnt]["Status"] = "E";
                                            bDetailsRecordExist = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    {
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Recv_Qty"]) == 0 && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please Enter the Receive Qty at Row No " + (iCnt) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                            break;
                        }
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]) != 0 && Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Recv_Qty"]) > Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Qty"]) && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "C" && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]) != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please Cannot Enter Greater Receive Qty from Stock Qty at Row No " + (iCnt) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
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
        
        private void BindDataToGrid()
        {
            try
            {
                //If No Data in Grid
                if (Session["StkTrnRPart"] == null)
                    Session["StkTrnRPart"] = dtDetails;
                else
                    dtDetails = (DataTable)Session["StkTrnRPart"];

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
                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                double dRecv_Qty = 0;

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    TextBox txtRecvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtRecvQty");
                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    if (sPartID != "" || sPartID != "0")
                    {
                        dPartQty = 0;
                        dPartRate = 0;
                        dRecv_Qty = 0;

                        dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                        dRecv_Qty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Recv_Qty"]);
                        dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);

                        dPartTotal = Func.Convert.dConvertToDouble(dRecv_Qty * dPartRate);

                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                        dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                        dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Recv_Qty"]);
                        dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                    }
                    //Status
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

                   
                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    lblDelete.Style.Add("display", "none");

                    //If Record is uploaded then it is not allowed to change
                    if (sRecordStatus.Trim() == "U")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                    }

                    //If Part Id  is not allocated           
                    if (sRecordStatus.Trim() == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        lblDelete.Style.Add("display", "");
                        //PartGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                   
                   
                    else if (sRecordStatus.Trim() == "C")
                    {
                        lblDelete.Style.Add("display", "");
                        Chk.Checked = true;
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
        protected void PartGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Label RFPtatus = (Label)PartGrid.Rows[0].FindControl("lblRFPStatus");
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    FillDetailsFromGrid(false);
                    Session["StkTrnRPart"] = dtDetails;
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

                dtDetails = (DataTable)Session["StkTrnRPart"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            
            //Func.Common.BindDataToCombo(drpChallanNo, clsCommon.ComboQueryType.StkChallanNo, 0, "And Challan_no Not in (select distinct Challan_No from TM_SPReceipt) And ( Dealer_ID=" + iDealerID + ") ");
            Func.Common.BindDataToCombo(drpChallanNo, clsCommon.ComboQueryType.StkChallanNo, 0, "And CH.Challan_no Not in (select distinct Challan_No from TM_SPReceipt where TM_SPReceipt.Dealer_ID=" + iDealerID + ") And ( CM.Dealer_Id=" + iDealerID + ") ");
            drpChallanNo.SelectedValue = "0";
        }
        protected void drpChallanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                 DataSet ds = new DataSet();
                ChallanNo = drpChallanNo.SelectedItem.Text.Trim();
                hdnChallanNo.Value = drpChallanNo.SelectedValue;
                objStkReceipt = new clsStkTransferReceipt();
                ds = objStkReceipt.GetStockTrnReceipt(iStkReceiptID, "New", iDealerID, Func.Convert.iConvertToInt(hdnChallanNo.Value));

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Receipt_canc"] = "N";
                            ds.Tables[0].Rows[0]["Receipt_Confirm"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtReceiptNo.Text = Func.Convert.sConvertToString(objStkReceipt.GenerateStkReceiptNo(Func.Convert.sConvertToString(Session["sDealerCode"]), iDealerID));
                txtReceiptDate.Text = Func.Common.sGetCurrentDate(1, false);
                objStkReceipt = null;
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

    }
}