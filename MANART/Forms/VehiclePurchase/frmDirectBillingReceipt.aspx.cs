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


using System.Collections.Generic;

using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Web.UI.HtmlControls;


namespace MANART.Forms.VehiclePurchase
{
    public partial class frmDirectBillingReceipt : System.Web.UI.Page
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
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();


                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
               
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
              
                txtUserType.Text = Session["UserType"].ToString();
                if (txtUserType.Text == "6")
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iHOBrId = iDealerId;
                    HOBrID = iDealerId;
                    txtDealerCode.Text = Session["sDealerCodeLoc"].ToString();
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                    txtDealerCode.Text = Session["sDealerCode"].ToString();
                }

                ToolbarC.iValidationIdForSave = 65;

               



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


                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                }



                //ToolbarC.iValidationIdForSave = 62;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (txtUserType.Text == "6")
                {
                    Location.DDLSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                }

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

            FillCombo();

            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            DisplayPreviousRecord();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (txtUserType.Text == "6")
            {
                FillSelectionGrid();
            }
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
                if (objCommon.sUserRole == "15" || objCommon.sUserRole == "19")
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
                    if (iMenuId==662)
                    {
                        GenerateLeadNo("VD");
                    }
                    else if (iMenuId==663)
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


                    drpInvNo.Style.Add("display", "");
                    txtDMSInvNo.Style.Add("display", "none");
                    FillInv();
                    
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);


                    MakeEnableDisableControls(true, "Nothing");
             
                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;


                    if (iMenuId == 662)
                    {
                        if (iID == 0)
                        {
                            GenerateLeadNo("VD");
                        }
                    }
                    else if (iMenuId == 663)
                    {
                        if (iID == 0)
                        {
                            GenerateLeadNo("VM");
                        }
                    }

                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
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

                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
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
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
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
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);

            drpModelGroup.SelectedValue = "1";



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

            modelgrp = Func.Convert.iConvertToInt(drpModelCat.SelectedValue);


            //if (txtHDCode.Text == "")
            //{
            //    Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID not in (1,8) and model_gr_id=" + modelgrp);
            //    Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID not in (1,8) and model_gr_id=" + modelgrp);
            //}
            //else
            //{
            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);
            //}

            drpModelGroup.SelectedValue = "1";

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
            else if (iMenuId == 663)
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
                else if (iMenuId == 663)
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


                MakeEnableDisableControls(false, "Nothing");

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
                dtInvoice = objDB.ExecuteStoredProcedureAndGetDataTable("GetVehInvoiceforDirectBilling", iDealerId,iMenuId);

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
                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iHOBrId;
                }

                if (iMenuId == 662)
                {
                    SearchGrid.sSqlFor = "VehReceiptDD";
                }
                else if (iMenuId == 663)
                {
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
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

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
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCH"]);
                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_Amt"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_Amt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);
                    HiddenGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]);
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
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehGST"]) != "N")
                {
                 tdlbl.Visible = false; tdamt.Visible = false; tdper.Visible = false;
                }
                else
                { tdlbl.Visible = true; tdamt.Visible = true; tdper.Visible = true; }



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

                //TrFileDesc.Style.Add("display", "");
                //TrBtnUpload.Style.Add("display", "");


                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
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
                    , Cancel, txtPFCharges.Text, txtOthercharges.Text, txtCustomer.Text, 2,iMenuId
                    , Func.Convert.iConvertToInt(drpModelCat.SelectedValue),HiddenGST.Value);
            }
            else if (iMenuId==663)
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
                  , Cancel, txtPFCharges.Text, txtOthercharges.Text, txtCustomer.Text, 3,iMenuId
                  , Func.Convert.iConvertToInt(drpModelCat.SelectedValue), HiddenGST.Value);
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
        private void  MakeEnableDisableControls(bool bEnable, string type)
        {


            txtPONo.Enabled = false;
            txtPODate.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelGroup.Enabled = false;
            drpModelCat.Enabled = false;
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
        #region Attach File

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string fileNames = FileAttchGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            string FileExtension = Path.GetExtension(fileNames);

            if (fileNames.Trim() != "")
            {
                //Clear the content of the responce
                Response.ClearContent();
                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileNames);
                // Add the file size into the response header
                Response.AddHeader("Content-Length", fileNames.Length.ToString());
                // Set the Content Type 
                Response.ContentType = ReturnExtension(FileExtension.ToLower());
                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile((sPath + "Vehicle Purchase\\Direct Billing Receipt" + "\\" + fileNames));
                // End the response 
                Response.End();
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
                case ".doc": return "application/ms-word";
                //case ".tiff":
                //case ".tif": return "image/tiff";
                //case ".asf": return "video/x-ms-asf";
                //case ".avi": return "video/avi";
                case ".zip": return "application/zip";
                case ".xls":
                case ".csv": return "application/vnd.ms-excel";
                case ".gif": return "image/gif";
                case ".jpg":
                case "jpeg": return "image/jpeg";
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
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);
                    sSourceFileType = Path.GetExtension(uploads[i].FileName);
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
                            sSourceFileName1 = Func.Convert.sConvertToString(iDealerId) + "_" + txtMReceiptNo.Text + "_" + sSourceFileName;
                            sSourceFileName1 = sSourceFileName1.Replace("/", "");
                            dr["File_Names"] = sSourceFileName1;
                            //dr["File_Names"] = Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                            dr["UserId"] = Func.Convert.sConvertToString(Session["UserID"]);
                            dr["Status"] = "S";


                            //Saving it in temperory Directory.                                       
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Vehicle Purchase\\Direct Billing Receipt");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }

                            //uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));
                            uploads[i].SaveAs((sPath + "Vehicle Purchase\\Direct Billing Receipt" + "\\" + sSourceFileName1));

                            strNewPath = sPath + "Vehicle Purchase\\Direct Billing Receipt" + "\\" + sSourceFileName1;
                            //dr["Path"] = strNewPath;

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
        //private void bFillDetailsFromFileAttachGrid()
        //{
        //    DataRow dr;
        //    dtFileAttach = new DataTable();
        //    //Get Header InFormation        
        //    dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
        //    dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
        //    dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("Path", typeof(string)));
        //    CheckBox ChkForDelete;
        //    for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
        //    {
        //        if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
        //        {
        //            dr = dtFileAttach.NewRow();
        //            dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
        //            dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
        //            dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
        //            dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);

        //            ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));
        //            Label lblDelete = (Label)(FileAttchGrid.Rows[iGridRowCnt].FindControl("lblDelete"));

        //            if (ChkForDelete.Checked == true)
        //            {
        //                dr["Status"] = "D";
        //            }
        //            else
        //            {
        //                dr["Status"] = "S";
        //            }
        //            dtFileAttach.Rows.Add(dr);
        //            dtFileAttach.AcceptChanges();
        //        }
        //    }
        //}

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

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if ( dtFileAttach.Rows.Count != 0)
            {
                if(dtFileAttach != null )
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

        }
        #endregion
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
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

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
                    HiddenGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]);
                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PF_Charges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other_Charges"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Amt"]);
                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["GrandTotal"]);
                    txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_TaxID"]);
                    txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_TaxID"]);
                    txtCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer"]);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]) == "N")
                    {
                        txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                        txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2ID"]);
                        txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                        txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);
                        txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1_TaxID"]);
                    }
                    else
                    {
                        txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                        txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2ID"]);
                        txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Taxamt_CGST"]);
                        txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax_CGST"]);

                    }


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