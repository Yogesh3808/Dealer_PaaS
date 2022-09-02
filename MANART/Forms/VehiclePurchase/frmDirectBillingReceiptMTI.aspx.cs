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
using System.Globalization;
using MANART_BAL;
using MANART_DAL;
using System.IO;


namespace MANART.Forms.VehiclePurchase
{
    public partial class frmDirectBillingReceiptMTI : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
        private DataTable dtClosureDetails = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        private int iDealerID = 0;
        private int iID;
        private bool bDetailsRecordExist = false;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        private DataSet DSDealer;
        private DataSet dsState;
        private string sControlClientID = "";
        int iUserId = 0;
        int iHOBrId = 0;
        int iInqID = 0;
        int iMenuId = 0;
        private int iDealerId = 0;
        private int HOBrID = 0;
        int iPrimaryApplicationID = 0;
        int iM0PriAppID = 0;
        int sStage = 1;
        string DealerCode = "";
        string sNew = "N";
        Boolean bSaveRecord = false;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                ToolbarC.bUseImgOrButton = true;
                //Location.bUseSpareDealerCode = true;
                //Location.SetControlValue();


                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
               
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                FindHOBr(iDealerId);
                Location.bUseSpareDealerCode = false;
            
                //ToolbarC.iValidationIdForSave = 65;

                //Location.bUseSpareDealerCode = false;
                //Location.SetControlValue();



                if (!IsPostBack)
                {
                    FillCombo();

                    DisplayPreviousRecord();
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                }

                SearchGrid.sGridPanelTitle = "Vehicle PO (Dealer) Detials";
                FillSelectionGrid();

                if (iID != 0)
                {
                    GetDataAndDisplay();
                }

                if (txtID.Text == "")
                {


                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }



                //ToolbarC.iValidationIdForSave = 62;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FindHOBr(int iDealerId)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetHoBrIDforMTIUsers", iDealerId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                iHOBrId = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                HOBrID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Location.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillCombo();

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {

            //FillCombo();

            //FillSelectionGrid();
            //PSelectionGrid.Style.Add("display", "");
            iDealerId = Location.iDealerId;

            FindHOBr(iDealerId);

            FillCombo();
            
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            DisplayPreviousRecord();

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
            btnReadonly();
        }
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

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;



                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "");
                    DisplayPreviousRecord();


                    if (iDealerId != 0)
                    {
                        if (iMenuId == 662)
                        {
                            GenerateLeadNo("VD");
                        }
                        else if (iMenuId == 680)
                        {
                            GenerateLeadNo("VM");
                        }


                        ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        //  ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);


                        //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                        drpInvNo.Style.Add("display", "");
                        txtDMSInvNo.Style.Add("display", "none");
                        FillInv();

                    }
                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;



                    if (txtMReceiptDate.Text == "" || txtMReceiptDate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter PO Date');</script>");
                        return;
                    }


                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }


                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm/Close
                {
                    //string OrderStatus = "";
                    iID = Func.Convert.iConvertToInt(txtID.Text);




                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                    }



                }

                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;




                    iID = bHeaderSave("Y", "N", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        if (bSaveDetails("Y", "N", "N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                    }

                }

                FillSelectionGrid();
                GetDataAndDisplay();

            }

            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);

            //Func.Common.BindDataToCombo(drpVehConditon, clsCommon.ComboQueryType.VehicleCondition, 0);



        }


        protected void drpModelGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillModel();

        }



        protected void drpModelCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModel.SelectedValue = drpModelCode.SelectedValue;
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);

            }

        }


        protected void drpModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModelCode.SelectedValue = drpModel.SelectedValue;
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
            }

        }
        private void FillModel()
        {

            int modelgrp = 0;

            modelgrp = Func.Convert.iConvertToInt(drpModelGroup.SelectedValue);



            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);


        }

        private void GenerateLeadNo(string Type)
        {
            // 'Replace Func.DB to objDB by Shyamal on 26032012

            MANART_DAL.clsDB objDB = new MANART_DAL.clsDB();
            try
            {
                DataSet dsDCode = new DataSet();


                dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerId);

                if (dsDCode.Tables[0].Rows.Count > 0)
                {
                    DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                }
            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }

            if (iMenuId == 662)
            {

                if (Type == "VD")
                {

                    txtMReceiptNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "VD"));

                }
            }
            else if (iMenuId == 680)
            {

                if (Type == "VM")
                {

                    txtMReceiptNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "VM"));

                }
            }




        }

        public string FindMAxLeadNo(string VDealerCode, int iDealerID, string Type)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";

                if (iMenuId == 662)
                {
                    if (Type == "VD")
                    {
                        sDocName = "VD";
                    }
                }
                else if (iMenuId == 680)
                {
                    if (Type == "VM")
                    {
                        sDocName = "VM";
                    }
                }



                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(5, '0');

                if (VDealerCode != "")
                {
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else
                {
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                }


                return sDocNo;
            }
            catch
            {

                return "0";
            }
        }

        private void DisplayPreviousRecord()
        {
            try
            {

                DataSet ds = new DataSet();

                //int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                //string sDocType = Func.Convert.sConvertToString(txtCashLoan.Text);


                ds = GetPO(iID, "New", iDealerId, iHOBrId, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            //ds.Tables[0].Rows[0]["PO_Cancel"] = "N";
                            //ds.Tables[0].Rows[0]["PO_Confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }

                //txtID.Text = "";
                //txtDocDate.Text = Func.Common.sGetCurrentDate(HOBrID, false);
                //txtLikelydate.Text = Func.Common.sGetCurrentDate(HOBrID, false);

                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();


                ds = GetPO(iID, "Max", iDealerId, iHOBrId, iID);
                if (ds == null) // if no Data Exist
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
                }
                else
                {
                    DisplayData(ds);


                }
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        public DataSet GetPO(int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDirectBillingReceipt", POId, POType, DealerID, HOBrID);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }




        private void FillInv()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            DataTable dtInvoice = null;
            //objMatReceipt = new clsMaterialReceipt();
            try
            {
                dtInvoice = new DataTable();
                dtInvoice = objDB.ExecuteStoredProcedureAndGetDataTable("GetVehInvoiceforDirectBilling", iDealerId, iMenuId);

                drpInvNo.DataValueField = "ID";
                drpInvNo.DataTextField = "Invoice_No";
                drpInvNo.DataSource = dtInvoice;
                drpInvNo.DataBind();
                drpInvNo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (dtInvoice != null) dtInvoice = null;
                if (objDB != null) objDB = null;
            }

        }


        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "Vehicle Receipt (Direct Customer) List";
                SearchGrid.AddToSearchCombo("Receipt No");
                SearchGrid.AddToSearchCombo("Receipt Date");
                SearchGrid.AddToSearchCombo("PO Details");
                SearchGrid.AddToSearchCombo("Chassis No");
                SearchGrid.AddToSearchCombo("Status");
               

                if (iMenuId == 662)
                {
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iHOBrId;
                    SearchGrid.sSqlFor = "VehReceiptDD";
                }
                else if (iMenuId == 680)
                {
                    if (iDealerId == 0)
                    {
                        if (Location.iDealerId != 0)
                        {
                            SearchGrid.iDealerID = Location.iDealerId;
                            iDealerId = Location.iDealerId;
                        }
                        if (iHOBrId == 0)
                        {
                            FindHOBr(iDealerId);
                        }
                    }
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iHOBrId;
                    SearchGrid.sSqlFor = "VehReceiptDM";
                }
                
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iID = Func.Convert.iConvertToInt(txtID.Text);

            GetDataAndDisplay();
        }

        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();


                int iID = Func.Convert.iConvertToInt(txtID.Text);
                //int iM0ID = Func.Convert.iConvertToInt(txtM1ID.Text);

                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetPO(iID, "All", iDealerId, iHOBrId, 0);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);

                }
                else
                {
                    ds = GetPO(iID, "Max", iDealerId, iHOBrId, 0);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);

                    DisplayData(ds);
                }
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
                bool bRecordIsOpen = true;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {

                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {



                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtMReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tr_no"]);
                    txtMReceiptDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["ref_date"], false);
                    txtDMSInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["inv_no"]);
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["inv_date"], false);
                    txtDeliveryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["delivery_No"]);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_date"], false);
                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["chassis_no"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["engine_no"]);



                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_GP"]);
                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["rate"]);
                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TotalAmt"]);
                    txtParking.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ParkingLocation"]);
                    //drpVehConditon.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehConditionID"]);
                    txtCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Name"]);

                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TotalAmt"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);

                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VatDesc"]);
                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2Desc"]);

                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_Amt"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_Amt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PF_Charges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other_Charges"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Amt"]);
                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["inv_amt"]);

                    txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_TaxID"]);
                    txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_TaxID"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1_TaxID"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2_TaxID"]);



                }




                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]);



                //if (hdnConfirm.Value == "Y")
                //{
                //    if (drpVehConditon.SelectedValue != "1" && drpVehConditon.SelectedValue != "0")
                //    {
                //        bChangeToSaleable.Visible = true;
                //    }
                //    else
                //    {
                //        bChangeToSaleable.Visible = false;
                //    }
                //}
                //else
                //{
                //    bChangeToSaleable.Visible = false;
                //}

                TrFileDesc.Style.Add("display", "");
                TrBtnUpload.Style.Add("display", "");


                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    TrFileDesc.Style.Add("display", "none");
                    TrBtnUpload.Style.Add("display", "none");
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }

                // Display Attach file  Details    
                if (PFileAttchDetails.Visible == true)
                {
                    //TrFileDesc.Style.Add("display", "none");
                    //TrBtnUpload.Style.Add("display", "none");

                    dtFileAttach = ds.Tables[2];
                    lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                    ShowAttachedFiles();
                }


                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true, "Nothing");
                }
                else if (hdnCancle.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Cancel");
                }
                else if (hdnConfirm.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Confirm");
                }
                //else if (hdnLost.Value == "Y")
                //{
                //    MakeEnableDisableControls(false, "Lost");
                //}

                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }

                if (Func.Convert.iConvertToInt(txtID.Text) != 0 && (bEnableControls == true))
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    if (Func.Convert.iConvertToInt(txtInqID.Text) != 0)
                    {
                        //bConvertToInq.Enabled = false;
                        //bHold.Enabled = false;
                        //MakeEnableDisableControls(false, "Nothing");
                    }
                    else
                    {
                        //bConvertToInq.Enabled = true;
                        //bHold.Enabled = true;
                    }

                }
                else
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                }
                //else if (hdnCancle.Value == "Y")
                //{
                //    //bConvertToInq.Enabled = false;
                //    //bHold.Enabled = false;
                //}
                //else if (hdnConfirm.Value == "Y")
                //{
                //    //bConvertToInq.Enabled = false;
                //    //bHold.Enabled = false;
                //}




            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            if (iMenuId == 662)
            {
                HdrID = objLead.bSaveDirectBillingReceipt(iID, iDealerId, HOBrID,
                    txtMReceiptNo.Text, txtMReceiptDate.Text, txtDMSInvNo.Text, txtDMSInvDate.Text, txtDeliveryNo.Text, txtPONo.Text, txtPODate.Text,
                    txtGrandTotal.Text, txtChassisNo.Text, txtEngineNo.Text, Func.Convert.iConvertToInt(drpModel.SelectedValue),
                    Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtModelRate.Text, txtQty.Text, txtTotalAmt.Text, txtDisc.Text,
                    Func.Convert.iConvertToInt(txtCSTID.Text), txtCSTAmt.Text, Func.Convert.iConvertToInt(txtVATID.Text), txtVatAmt.Text,
                    Func.Convert.iConvertToInt(txtTax1ID.Text), txttax1Amt.Text, Func.Convert.iConvertToInt(txtTax2ID.Text), txttax2Amt.Text,
                    txtTCSPer.Text, txtTCS.Text, txtParking.Text,
                    //Func.Convert.iConvertToInt(drpVehConditon.SelectedValue), 
                    Confirm
                    , Cancel, txtPFCharges.Text, txtOthercharges.Text, txtCustomer.Text, 2, iMenuId,0,"N");
            }
            else if (iMenuId == 680)
            {
                HdrID = objLead.bSaveDirectBillingReceipt(iID, iDealerId, HOBrID,
                  txtMReceiptNo.Text, txtMReceiptDate.Text, txtDMSInvNo.Text, txtDMSInvDate.Text, txtDeliveryNo.Text, txtPONo.Text, txtPODate.Text,
                  txtGrandTotal.Text, txtChassisNo.Text, txtEngineNo.Text, Func.Convert.iConvertToInt(drpModel.SelectedValue),
                  Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtModelRate.Text, txtQty.Text, txtTotalAmt.Text, txtDisc.Text,
                  Func.Convert.iConvertToInt(txtCSTID.Text), txtCSTAmt.Text, Func.Convert.iConvertToInt(txtVATID.Text), txtVatAmt.Text,
                  Func.Convert.iConvertToInt(txtTax1ID.Text), txttax1Amt.Text, Func.Convert.iConvertToInt(txtTax2ID.Text), txttax2Amt.Text,
                  txtTCSPer.Text, txtTCS.Text, txtParking.Text,
                    //Func.Convert.iConvertToInt(drpVehConditon.SelectedValue), 
                  Confirm
                  , Cancel, txtPFCharges.Text, txtOthercharges.Text, txtCustomer.Text, 3, iMenuId,0,"N");
            }
            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();
            if (bSaveAttachedDocuments() == false) return bSaveRecord;

            if (objLead.bSaveGRNChecklistAttachment(objDB, dtFileAttach, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }
            return bSaveRecord;
        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable, string type)
        {


            txtPONo.Enabled = false;
            txtPODate.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelGroup.Enabled = false;
            txtQty.Enabled = false;
            txtModelRate.Enabled = false;
            txtTotalAmt.Enabled = false;
            txtChassisNo.Enabled = false;
            txtEngineNo.Enabled = false;
            txtMReceiptNo.Enabled = false;
            txtMReceiptDate.Enabled = false;
            txtDMSInvNo.Enabled = false;
            txtDMSInvDate.Enabled = false;
            txtDeliveryNo.Enabled = false;
            txtParking.Enabled = bEnable;
            txtCustomer.Enabled = false;



            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }





        protected void drpInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDMSInvNo.Text = Func.Convert.sConvertToString(drpInvNo.SelectedItem);
            drpInvNo.Style.Add("display", "none");
            txtDMSInvNo.Style.Add("display", "");
            txtDeliveryNo.Enabled = true;



        }

        protected void ChangeToSaleable(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                if (bChangeVehCondition(objDB, iDealerId, iHOBrId, iID) == true)
                {
                    bSaveRecord = true;
                }


                if (bSaveRecord == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Vehicle Condition Changed To Saleable');</script>");
                }
                else
                {

                }

                FillSelectionGrid();
                GetDataAndDisplay();
            }

        }
        public bool bChangeVehCondition(clsDB objDB, int dealerid, int hobrId, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {




                if (iHdrID > 0)
                {
                    objDB.BeginTranasaction();

                    objDB.ExecuteStoredProcedure("SP_ChangeToSaleable", dealerid, hobrId, iHdrID
                        );

                    objDB.CommitTransaction();

                }





                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            DataRow dr;
            int iRecorFound = 0;
            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);

                    if (sSourceFileName.Trim() != "")
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

                        string[] splClaimNo = txtPONo.Text.Split('/');
                        if (splClaimNo.Length > 1)
                        {
                            txtPONo.Text = "";
                            for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                                txtPONo.Text = txtPONo.Text + splClaimNo[iCnt];
                        }


                        //dr["File_Names"] = sSourceFileName;
                        //if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmNormal)

                        if (iMenuId == 662)
                        {
                            dr["File_Names"] = Func.Convert.sConvertToString(iDealerId) + "_VD_" + Func.Convert.sConvertToString(txtPONo.Text) + "_" + sSourceFileName;
                        }
                        else if (iMenuId == 680)
                        {
                            dr["File_Names"] = Func.Convert.sConvertToString(iDealerId) + "_VM_" + Func.Convert.sConvertToString(txtPONo.Text) + "_" + sSourceFileName;
                        }

                        dr["UserId"] = Func.Convert.sConvertToString(iDealerId);
                        dr["Status"] = "S";

                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        //Saving it in temperory Directory.               

                        if (!System.IO.Directory.Exists(sPath + "Claim Request"))
                            System.IO.Directory.CreateDirectory(sPath + "Claim Request");

                        DirectoryInfo destination = new DirectoryInfo(sPath + "Claim Request");
                        if (!destination.Exists)
                        {
                            destination.Create();
                        }
                        uploads[i].SaveAs((sPath + "Claim Request" + "\\" + Func.Convert.sConvertToString(iDealerId) + "_DD_"
                            + Func.Convert.sConvertToString(txtPONo.Text) + "_" + sSourceFileName + ""));


                    }

                }

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
            bool bSaveRecord = false;
            DataRow dr;
            dtFileAttach = new DataTable();
            //Get Header InFormation        
            dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
                    //if (txtID.Text != "")//no
                    //    dr["ID"] = 0;
                    //else
                    dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(iDealerId);

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                    }
                    dtFileAttach.Rows.Add(dr);
                    dtFileAttach.AcceptChanges();
                }
            }
        }

        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }
        protected void txtDeliveryNo_TextChanged(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();

            //objMatReceipt = new clsMaterialReceipt();
            try
            {

                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetVehInvallDetailsDealerPO", iDealerId, Func.Convert.iConvertToInt(drpInvNo.SelectedValue), txtDeliveryNo.Text);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtDeliveryNo.Enabled = false;
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Invoice_Date"], false);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Rate"]);
                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Rate"]);
                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_No"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_No"]);

                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Total"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);

                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VatDesc"]);
                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2Desc"]);

                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATAmt"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTAmt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PF_Charges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other_Charges"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Amt"]);
                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["GrandTotal"]);

                    txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_TaxID"]);
                    txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_TaxID"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2ID"]);
                    txtCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer"]);



                }
                else
                {
                    txtDeliveryNo.Enabled = true;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Correct Delivery No !');</script>");
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



    }
}