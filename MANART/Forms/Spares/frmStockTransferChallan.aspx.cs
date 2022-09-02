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
    public partial class frmStockTransferChallan : System.Web.UI.Page
    {
        private int iStockChallanID = 0;
        string sFileName = "";
        int iDealerID = 0;
        private DataTable dtDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsStkTransferChallan objStkChallan = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        int iMenuId = 0;
        int iHOBr_id = 0;
        string sDealerCode = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                hdnMenuID.Value = Func.Convert.sConvertToString(Request.QueryString["MenuID"]);
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iFormIdToOpenForm = 301;

                if (iMenuId == 667)
                {

                    drpStkTrnChallanType.Items.Add(new ListItem("Stock Transfer", "ST", true));
                    drpStkTrnChallanType.SelectedValue = "ST";
                }
                if (iMenuId == 676)
                {
                    drpStkTrnChallanType.Items.Add(new ListItem("Work Order", "WO", true));
                    drpStkTrnChallanType.SelectedValue = "WO";
                }

                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();
                clsSupplier ObjSup = new clsSupplier();
                DataSet dsDealer = new DataSet();
                DataSet ds = new DataSet();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    FillSelectionGrid();
                }

                if (!IsPostBack)
                {
                    Session["StkTrnChaPart"] = null;
                    DisplayPreviousRecord();
                    if (iStockChallanID == 0)
                    {
                        drpStkTrnChallanType_SelectedIndexChanged(sender, e);
                    }
                }

                SearchGrid.sGridPanelTitle = (iMenuId == 667) ? "Stock Transfer List" : "WorkOrder List";
                lblTitle.Text = (iMenuId == 667) ? "Stock Transfer Challan" : "WorkOrder Challan";

                if (iStockChallanID != 0)
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

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                //SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Location.DDLSelectedIndexChanged += new EventHandler(Location_DDLSelectedIndexChanged);
            iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["StkTrnChaPart"] = null;
                DisplayPreviousRecord();
                //GenerateStockChallanNo();
                if (iStockChallanID == 0)
                {
                    drpStkTrnChallanType_SelectedIndexChanged(sender, e);
                }
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
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                FillSelectionGrid();
                Session["StkTrnChaPart"] = null;
                DisplayPreviousRecord();
                GenerateStockChallanNo();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iStockChallanID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objStkChallan = new clsStkTransferChallan();
                if (iStockChallanID != 0)
                {
                    ds = objStkChallan.GetStockTrnChallan(iStockChallanID, "All", Func.Convert.iConvertToInt(Session["iDealerID"]), 0, iMenuId);
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
                objStkChallan = null;
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
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);

                objStkChallan = new clsStkTransferChallan();
                ds = objStkChallan.GetStockTrnChallan(iStockChallanID, "New", iDealerID, 0, iMenuId);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Chal_canc"] = "N";
                            ds.Tables[0].Rows[0]["Chal_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                //txtStkTrnChNo.Text = Func.Common.sGetMaxDocNo(Location1.sDealerCode, "", "StockAdj", Func.Convert.iConvertToInt(Location1.iDealerId));
                txtStkTrnChDate.Text = Func.Common.sGetCurrentDate(1, false);
                //UploafFile.Style.Add("display", "");
                objStkChallan = null;
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


                txtStkTrnChNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Challan_no"]);
                txtStkTrnChDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Challan_date"]);
                txtReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReceiptNo"]);
                txtReceiptDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReciptDate"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISCHGST"]);

                if (iStockChallanID != 0)
                {
                    DrpCustomer.Enabled = false;
                    drpStkTrnChallanType.Enabled = false;
                    DrpCustomer.CssClass = "NonEditableFields";
                    drpStkTrnChallanType.CssClass = "NonEditableFields";
                    DrpReference.Enabled = false;
                    DrpReference.CssClass = "NonEditableFields";
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And DealerID=" + Location.iDealerId + " And Cust_Type = 6 And CM.Cust_Type <> 18 ");
                }
                else
                {
                    DrpCustomer.Enabled = true;
                    drpStkTrnChallanType.Enabled = true;
                    DrpCustomer.CssClass = "ComboBoxFixedSize";
                    drpStkTrnChallanType.CssClass = "ComboBoxFixedSize";
                    DrpReference.Enabled = true;
                    DrpReference.CssClass = "ComboBoxFixedSize";

                }
                txtStkTrnChDate.Enabled = false;

                drpStkTrnChallanType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChType"]);
                hdnChallanType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChType"]);
                lblCustTitle.Text = (hdnChallanType.Value != "ST") ? "Supplier:" : "Customer:";

                if (hdnChallanType.Value != "ST" && iStockChallanID != 0)
                {
                    Func.Common.BindDataToCombo(DrpReference, clsCommon.ComboQueryType.OpenJobcard, Location.iDealerId, " And DlrBranchID=" + Func.Convert.iConvertToInt(Session["HOBR_ID"]) + " and TD_Jobcard_PartDet.ID in (select RefID from TM_SPChallan where ID=" + iStockChallanID + ")");
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId);
                }
                else
                {
                    DrpReference.Items.Add(new ListItem("--Select--", "0", true));
                }
                DrpCustomer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);

                DrpReference.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RefID"]);

                txtnarration.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["narration"]);

                Session["StkTrnChaPart"] = null;

                dtDetails = ds.Tables[1];
                Session["StkTrnChaPart"] = dtDetails;
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chal_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                hdnCancel.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chal_canc"]);
                trLast.Style.Add("display", (hdnChallanType.Value == "WO" && hdnConfirm.Value == "Y") ? "" : "none");
                BindDataToGrid();
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                // If Record is Confirm or cancel then it is not editable            

                lblstkTrnRcpNo.Style.Add("display", "none");
                lblstkTrnRcpDt.Style.Add("display", "none");
                txtReceiptNo.Style.Add("display", "none");
                txtReceiptDate.Style.Add("display", "none");
                btnReceive.Style.Add("display", "none");

                lblReference.Style.Add("display", (hdnChallanType.Value != "ST") ? "" : "none");
                DrpReference.Style.Add("display", (hdnChallanType.Value != "ST") ? "" : "none");

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chal_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    if (hdnChallanType.Value != "ST")
                    {
                        lblstkTrnRcpNo.Style.Add("display", "");
                        lblstkTrnRcpDt.Style.Add("display", "");
                        txtReceiptNo.Style.Add("display", "");
                        txtReceiptDate.Style.Add("display", "");
                        if (txtReceiptNo.Text.Trim() == "") btnReceive.Style.Add("display", "");
                    }
                }

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chal_canc"]) == "Y")
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
                if (iMenuId == 676)
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                if (txtUserType.Text.Trim() == "6")
                {
                    DrpCustomer.Enabled = false;
                    UploafFile.Style.Add("display", "none");
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
                objStkChallan = new clsStkTransferChallan();
                ds = objStkChallan.GetStockTrnChallan(iStockChallanID, "Max", iDealerID, 0, iMenuId);
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
                    Session["StkTrnChaPart"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objStkChallan = null;
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
                    Session["StkTrnChaPart"] = null;
                    DisplayPreviousRecord();
                    if (hdnChallanType.Value == "ST")
                    {
                        FillCombo();
                        UploafFile.Style.Add("display", "");
                        FillCombo();
                    }
                    //if (DrpCustomer.SelectedIndex != 0 )
                    //{
                    GenerateStockChallanNo();
                    //}

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
                iStockChallanID = Func.Convert.iConvertToInt(txtID.Text);
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
                //For MD User
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);

                }

                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Challan No");
                SearchGrid.AddToSearchCombo("Challan Date");
                // SearchGrid.iDealerID = iDealerID;
                if (iMenuId == 676) SearchGrid.AddToSearchCombo("Receipt No");
                if (iMenuId == 676) SearchGrid.AddToSearchCombo("Receipt Date");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iMenuId);
                SearchGrid.sSqlFor = "StockTrnChallan";
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
                dtHdr.Columns.Add(new DataColumn("Challan_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Challan_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ChType", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("RefID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Ch_Amt", typeof(double)));

                dtHdr.Columns.Add(new DataColumn("Chal_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Chal_canc", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("narration", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("HOBR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("ISCHGST", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Challan_no"] = txtStkTrnChNo.Text;
                dr["Challan_date"] = txtStkTrnChDate.Text;
                dr["ChType"] = drpStkTrnChallanType.SelectedValue.ToString();
                dr["CustID"] = Func.Convert.sConvertToString(DrpCustomer.SelectedValue);
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
                dr["RefID"] = (DrpReference.SelectedValue.Trim().ToString() == "") ? 0 : Func.Convert.iConvertToInt(DrpReference.SelectedValue.Trim().ToString());
                //dr["UserId"] = iUserId;
                dr["Chal_Confirm"] = "N";
                dr["Chal_canc"] = "N";

                //for (int iRCnt = 0; iRCnt < PartGrid.Rows.Count; iRCnt++)
                //{
                //    if (iRCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                //    {
                //        TextBox txtPartNo11 = (TextBox)PartGrid.Rows[iRCnt].FindControl("txtPartNo");
                //        cntPartID = txtPartNo11.Text;
                //        if (cntPartID != "")
                //        {
                //            TotCntQty = TotCntQty + 1;

                //        }
                //    }
                //}
                dr["Ch_Amt"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                //dr["Ch_AmtItem"] = TotCntQty;
                dr["narration"] = (txtnarration.Text.Trim().ToString());
                dr["HOBR_ID"] = Func.Convert.sConvertToString(Session["HOBR_ID"]);
                dr["ISCHGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);

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
            if (txtStkTrnChNo.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Challan date.";
                bValidateRecord = false;
            }
            if (txtStkTrnChDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Challan date.";
                bValidateRecord = false;
            }
            if (drpStkTrnChallanType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Challan Type.";
                bValidateRecord = false;
            }
            if (DrpCustomer.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Customer.";
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
            clsStkTransferChallan objStkChallan = new clsStkTransferChallan();
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
                    dtHdr.Rows[0]["Chal_Confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Chal_canc"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }
                if (objStkChallan.bSaveRecord(Location.sDealerCode, iDealerID, dtHdr, dtDetails, ref iStockChallanID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iStockChallanID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Challan") + "','" + Server.HtmlEncode(txtStkTrnChNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Challan") + "','" + Server.HtmlEncode(txtStkTrnChNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Challan") + "','" + Server.HtmlEncode(txtStkTrnChNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Challan") + "','" + Server.HtmlEncode(txtStkTrnChNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        private void GenerateStockChallanNo()
        {
            objStkChallan = new clsStkTransferChallan();
            // txtStkTrnChNo.Text = Func.Convert.sConvertToString(objStockAdj.GenerateStockAdj(Location.sDealerCode, iDealerID));
            txtStkTrnChNo.Text = Func.Convert.sConvertToString(objStkChallan.GenerateStkChallan(sDealerCode, iDealerID, hdnChallanType.Value));
        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            DataTable dtExistRecord = null;
            DataTable dtSysStock = null;
            StringBuilder sPartIDs = null;
            try
            {
                objStkChallan = new clsStkTransferChallan();
                dtSysStock = new DataTable();
                dtExistRecord = new DataTable();
                dtExistRecord = (DataTable)Session["StkTrnChaPart"];
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
                //    dtSysStock = objStkChallan.GetSysStock(Func.Convert.sConvertToString(sPartIDs), iUserId, "StockTranChallan", Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), Func.Convert.iConvertToInt(txtID.Text));
                //    if (dtSysStock != null && dtSysStock.Rows.Count > 0)
                //        Session["StkTrnChaPart"] = dtSysStock;
                // }
                FillDetailsFromGrid(false);
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objStkChallan != null) objStkChallan = null;
                if (dtSysStock != null) dtSysStock = null;
                if (dtExistRecord != null) dtExistRecord = null;
                if (sPartIDs != null) sPartIDs = null;
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strStockAdjsNo = "";
                dtDetails = (DataTable)Session["StkTrnChaPart"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                int iCntError1 = 0;
                string sQtyMsg = "";
                double dBFRGSTSTK = 0.00;
                double dCurrSTK = 0.00;
                double dChQty = 0.00;

                // New Code For Validation
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    string sPartID = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                    string sBFRGST = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtBFRGST") as TextBox).Text.Trim());
                    string siRowCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    string sChallanType = drpStkTrnChallanType.SelectedValue.Trim();

                    if (sPartID != "" && sPartID != "0" && siRowCntStatus != "D" && sChallanType == "ST")
                    {
                        double totPrevQty = 0.00;
                        double totInvQty = 0.00;
                        dCurrSTK = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtBalQty") as TextBox).Text);
                        dBFRGSTSTK = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock") as TextBox).Text);
                        dChQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);

                        for (int iCnt = 0; iCnt < PartGrid.Rows.Count; iCnt++)
                        {
                            string sCntPartID = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                            string sCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                            if (sPartID != "0" && sPartID != "" && sPartID == sCntPartID && sCntStatus != "D" && sChallanType == "ST")
                            {
                                double dPreInvQty = 0.00;
                                double dinvQty = 0.00;
                                dinvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("txtQuantity") as TextBox).Text);
                                dPreInvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("txtPreviousInvQty") as TextBox).Text);
                                totPrevQty = totPrevQty + dPreInvQty;
                                totInvQty = totInvQty + dinvQty;
                            }
                        }

                        if (dChQty == 0 && siRowCntStatus != "D")
                        { // Check Invoice Quantity
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter Challan Quantity at Row No " + (iRowCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iRowCnt / 10) + 1);
                            break;
                        }
                        if (sBFRGST == "N" && totPrevQty == dCurrSTK && totInvQty > dCurrSTK && siRowCntStatus != "D" && txtID.Text == "")
                        { // Check Curr Stock
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Challan Quantity from Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (sBFRGST == "Y" && totPrevQty == dBFRGSTSTK && totInvQty > dBFRGSTSTK && siRowCntStatus != "D" && txtID.Text == "")
                        { // check OLD Stock
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Challan Quantity from Pre GST Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (sBFRGST == "N" && totPrevQty != dCurrSTK && (totInvQty > (dCurrSTK + totPrevQty)) && siRowCntStatus != "D")// Check Curr Stock
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Challan Quantity from Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (sBFRGST == "Y" && totPrevQty != dBFRGSTSTK && (totInvQty > (dBFRGSTSTK + totPrevQty)) && siRowCntStatus != "D")
                        {// check OLD Stock
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Challan Quantity from Pre GST Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                            bDetailsRecordExist = false;
                            break;
                        }

                    }//END IF
                }//END For
                //END

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
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "O")
                                    {
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "M")
                                    {
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
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
                                TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                                dtDetails.Rows[iMRowCnt]["bal_qty"] = Func.Convert.dConvertToDouble(txtBalQty.Text);

                                // Get Physical Qty
                                TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                dtDetails.Rows[iMRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                // Get Inward Qty
                                TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                                dtDetails.Rows[iMRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                                // Get Outward Qty
                                TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                                dtDetails.Rows[iMRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                                //Get BFR GST Rate Flag
                                TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                                dtDetails.Rows[iRowCnt]["BFRGST"] = txtBFRGST.Text.Trim();

                                // Get BFR GST Stock
                                TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");
                                dtDetails.Rows[iRowCnt]["BFRGST_Stock"] = Func.Convert.dConvertToDouble(txtBFRGST_Stock.Text);

                                // Get Status                           
                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text.Trim();

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
                    //for (int iCnt = 1; iCnt < dtDetails.Rows.Count; iCnt++)
                    //{
                    //    string sBFRGST = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["BFRGST"]);
                    //    int iQty = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Qty"]);
                    //    string sCntStatus = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]);
                    //    string sChallanType = drpStkTrnChallanType.SelectedValue.Trim();
                    //    int iPartID = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]);
                    //    double dCurrStock = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["bal_qty"]);
                    //    double dBFRGSTStock = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["BFRGST_Stock"]);
                    //    if (sCntStatus != "C" && sCntStatus != "D" && iPartID != 0)
                    //    {
                    //        if (iQty == 0 )
                    //        {
                    //            iCntError = iCntError + 1;
                    //            sQtyMsg = "Please Enter the Challan Qty at Row No " + (iCnt) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //            break;
                    //        }
                    //        if (sBFRGST == "N" && sChallanType == "ST" && iPartID != 0 && iQty > dCurrStock )
                    //        {
                    //            iCntError = iCntError + 1;
                    //            sQtyMsg = "Please Cannot Enter Greater Challan Qty from Stock Qty at Row No " + (iCnt) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //            break;
                    //        }
                    //        if (sBFRGST == "Y" && sChallanType == "ST" && iPartID != 0 && iQty > dBFRGSTStock )
                    //        {
                    //            iCntError = iCntError + 1;
                    //            sQtyMsg = "Please Cannot Enter Greater Challan Qty from Pre GST Stock Qty at Row No " + (iCnt) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                    //            break;
                    //        }
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
                    if (dtDetails.Columns.Count == 0)
                    {
                        dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("RefDtlID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("bal_qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));
                    }

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
                dr["Part_ID"] = 0;
                dr["RefDtlID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 0.00;
                dr["bal_qty"] = 0.00;
                dr["MRPRate"] = 0.00;
                dr["Total"] = 0.00;
                dr["Status"] = "N";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;
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
                if (Session["StkTrnChaPart"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["StkTrnChaPart"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["StkTrnChaPart"];
                    if (dtDetails.Rows.Count == 0 && iMenuId != 676)
                        CreateNewRowToDetailsTable();
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N" && hdnCancel.Value == "N")
                            CreateNewRowToDetailsTable();
                    }
                    dtDetails = (DataTable)Session["StkTrnChaPart"];

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
                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                double dPartStk = 0;

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Func.Convert.sConvertToString(Session["iDealerID"]);
                string sPartID = "", sPartName = "", cPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    //Hide Part Stock for WO
                    PartGrid.HeaderRow.Cells[4].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt].Cells[4].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none");//Hide Cell
                    //Hide BFRPart Stock for WO
                    PartGrid.HeaderRow.Cells[11].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt].Cells[11].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none");//Hide Cell

                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowMultiPartSearch(this," + sDealerId + ",'" + DrpCustomer.ID + "');");

                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    TextBox txtPreviousInvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPreviousInvQty");
                    if (sPartID != "")
                    {
                        dPartQty = 0;
                        dPartRate = 0;
                        dPartStk = 0;
                        txtPreviousInvQty.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Qty"]);
                        dPartStk = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["bal_qty"]);
                        dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                        dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);

                        dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);

                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                        dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.sConvertToString(dPartTotal);

                        dTotalQty = dTotalQty + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                        dTotal = dTotal + Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                    }
                    //Status
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

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

                    //New 
                    LinkButton lnkNew = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkNew");
                    lnkNew.Style.Add("display", "none");
                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    lblDelete.Style.Add("display", "none");

                    LinkButton lblCancel = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lblCancel");
                    lblCancel.Style.Add("display", "none");
                    //If Record is uploaded then it is not allowed to change
                    if (sRecordStatus.Trim() == "U")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                    }
                    if (sRecordStatus.Trim() == "M")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                        if (sPartID == "0" && sRecordStatus == "M")
                        {
                            PartGrid.Rows[iRowCnt].Visible = false;
                        }

                    }


                    //If Part Id  is not allocated           
                    if (sRecordStatus.Trim() == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        lblDelete.Style.Add("display", "");
                        //PartGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus.Trim() == "E")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                        Chk.Checked = false;
                    }
                    else if (sRecordStatus.Trim() == "P")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");

                    }
                    else if (sRecordStatus.Trim() == "O")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");

                        if ((iRowCnt + 1) == PartGrid.Rows.Count)
                        {
                            //lnkNew.Style.Add("display", "");
                        }

                    }
                    else if (sRecordStatus.Trim() == "C")
                    {
                        lblDelete.Style.Add("display", "");
                        Chk.Checked = true;
                        //lblCancel.Style.Add("display", "");
                        //lblCancel.Attributes.Add("disabled", "true");
                        //lblCancel.Text = "Cancelled";

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
                    Session["StkTrnChaPart"] = dtDetails;
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

                dtDetails = (DataTable)Session["StkTrnChaPart"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        #region Xlsx Upload For Stock Transfer
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                objStkChallan = new clsStkTransferChallan();
                DataTable dt = GetListOfPartNo();

                int iCountNotUpload = 0;

                string sFileNameForSave = "";
                DataSet dsUploadPartDetails;
                System.Threading.Thread.Sleep(2000);
                sFileName = FileUpload1.FileName;
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));

                dsUploadPartDetails = objStkChallan.UploadPartDetailsAndGetPartDetails(FileUpload1.FileName, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), 0, dt, "N", iUserId);
                if (dsUploadPartDetails == null)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in upload the the file contact admin.');</script>");
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
                dr["RefDtlID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 0;
                dr["bal_qty"] = 0;
                dr["MRPRate"] = 1;
                dr["Total"] = 1;
                dr["Status"] = "N";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();
                Session["StkTrnChaPart"] = dtDetails;
                BindDataToGrid();
                CPEPartDetails.Collapsed = false;


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
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
                    string strFileName = "StkTransfer" + Session["iDealerID"].ToString() + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        FileUpload1.SaveAs(Server.MapPath("~/DownloadFiles/StkTransfer/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/StkTransfer/" + strFileName + ".xlsx");

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
        #endregion

        #region For Row Number
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
                    //i = 2;
                }
            }
        }
        #endregion

        protected void drpStkTrnChallanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdnChallanType.Value = Func.Convert.sConvertToString(drpStkTrnChallanType.SelectedValue);
                DrpReference.Items.Add(new ListItem("Select", "N", true));
                if (hdnChallanType.Value == "ST")
                {
                    FillCombo();
                    UploafFile.Style.Add("display", "");
                    lblReference.Style.Add("display", "none");
                    DrpReference.Style.Add("display", "none");
                    trLast.Style.Add("display", "none");
                    lblTtlDocDetails.Text = "Stock Transfer Details";
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId);
                    UploafFile.Style.Add("display", "none");
                    lblReference.Style.Add("display", "");
                    DrpReference.Style.Add("display", "");
                    trLast.Style.Add("display", "none");
                    lblTtlDocDetails.Text = "WorkOrder Details";
                }
                GenerateStockChallanNo();
                lblCustTitle.Text = (hdnChallanType.Value != "ST") ? "Supplier:" : "Customer:";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    PartGrid.HeaderRow.Cells[4].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt].Cells[4].Style.Add("display", (hdnChallanType.Value == "ST") ? "" : "none");//Hide Cell
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void FillCombo()
        {
            if (hdnIsDocGST.Value == "N")
                Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And Cust_Type = 6 And CM.Cust_Type <> 18 and ISNULL(CM.Active,'N')='Y' ");
            else
                Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.CustomerBranchGSTNO, 0, " And DCL.DealerID=" + Location.iDealerId + " And Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') = ISNULL(CM.GST_No,'') And CM.Cust_Type <> 18 and ISNULL(CM.Active,'N')='Y' ");// When ST Use in Counter Sale Change in ComboQuery Also
            //OLD 29092017
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.CustomerBranchGSTNO, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And Cust_Type = 6");



            DrpCustomer.SelectedValue = "0";
        }

        protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (hdnChallanType.Value == "WO") Func.Common.BindDataToCombo(DrpReference, clsCommon.ComboQueryType.OpenJobcard, Location.iDealerId, " and VenderID=" + DrpCustomer.SelectedValue.Trim() + " And DlrBranchID=" + Func.Convert.iConvertToInt(Session["HOBR_ID"]) + " and TM_Jobcard_Header.ID not in (select RefID from TM_SPChallan where isnull(Chal_canc,'')='N')");                     // and TM_Jobcard_Header.ID not in (select RefID from TM_SPChallan where  isnull(Chal_canc,''N'')=''N'' )'
            if (hdnChallanType.Value == "WO") Func.Common.BindDataToCombo(DrpReference, clsCommon.ComboQueryType.OpenJobcard, Location.iDealerId, " and isnull(job_confirm,'N')='N'  and VenderID=" + DrpCustomer.SelectedValue.Trim() + " And DlrBranchID=" + Func.Convert.iConvertToInt(Session["HOBR_ID"]) + " and TM_Jobcard_Header.ID not in (select RefID from TM_SPChallan where isnull(Chal_canc,'')='N')");                     // and TM_Jobcard_Header.ID not in (select RefID from TM_SPChallan where  isnull(Chal_canc,''N'')=''N'' )'            
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());

            string sFinYear = Func.sGetFinancialYear(iDealerID);
            int iDelearId = iDealerID;
            string sDocNo = Func.Common.sGetMaxDocNo(Func.Convert.sConvertToString(Session["sDealerCode"]), sFinYear, "MCR", iDelearId);
            string sDocDt = Func.Common.sGetCurrentDateTime(Func.Convert.iConvertToInt(Location.iCountryId), true);


            if (bSavePartStkTrnChReceipt(Func.Convert.iConvertToInt(txtID.Text), iDealerID, sDocNo, sDocDt) == true)
            {
                Func.Common.UpdateMaxNo(objDB, sFinYear, "MCR", iDelearId);
                txtReceiptNo.Text = sDocNo;
                txtReceiptDate.Text = sDocDt;
                btnReceive.Style.Add("display", "none");
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Material Receipt Created Successfully.');</script>"); ;
            }
            else
            {
                txtReceiptNo.Text = "";
                txtReceiptDate.Text = "";
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Material Receipt Not Created, Please Contact to Administrator.');</script>");
            }
        }

        public bool bSavePartStkTrnChReceipt(int iHdrID, int iDealerID, string sDocNo, string sDocDt)
        {
            bool bSaveRecord = false;
            try
            {
                clsDB objDB = new clsDB();
                objDB.ExecuteStoredProcedure("SP_SPChallanHDR_Update_Receipt", iHdrID, sDocNo, sDocDt);
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }
    }


}