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

namespace MANART.Forms.VehiclePurchase
{
    public partial class frmDealerPurchaseReceipt : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
        private DataTable dtClosureDetails = new DataTable();
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

                if (txtUserType.Text == "6")
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iHOBrId = iDealerId;
                    HOBrID = iDealerId;

                    //txtDealerCode.Text = Session["sDealerCodeLoc"].ToString();
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                }


                ToolbarC.iValidationIdForSave = 65;

               


                if (!IsPostBack)
                {
                    FillCombo();

                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Vehicle PO (Dealer) Detials";
                FillSelectionGrid();

                if (iID != 0)
                {
                    GetDataAndDisplay();
                }

                if (txtID.Text == "")
                {


                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
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
                    Session["TmpDlrPurReceipt"] = 0; 
                    PSelectionGrid.Style.Add("display", "");
                    DisplayPreviousRecord();
                    GenerateLeadNo("VI");


                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                    drpInvNo.Style.Add("display", "");
                    txtDMSInvNo.Style.Add("display", "none");
                    FillInv();


                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {

                    string TmpDlrPurReceipt = Session["TmpDlrPurReceipt"].ToString();

                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;


                    if (iID == 0)
                    {


                        GenerateLeadNo("VI");
                    }

                    if (TmpDlrPurReceipt == "0" || iID != 0)
                    {

                        Session["TmpDlrPurReceipt"] = 1;

                        iID = bHeaderSave("N", "N", "N");

                        PSelectionGrid.Style.Add("display", "");
                        if (iID > 0)
                        {
                            txtID.Text = Func.Convert.sConvertToString(iID);

                            //if (bSaveDetails("N", "N", "N") == true)
                            //{
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            //}


                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
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

                        //if (bSaveDetails("N", "Y", "N") == true)
                        //{
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                        //}
                      
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
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
                        //if (bSaveDetails("N", "N", "Y") == true)
                        //{
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");
                        //}
                    
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
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


            if (Type == "VI")
            {

                txtMReceiptNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "VI"));

            }




        }

        public string FindMAxLeadNo(string VDealerCode, int iDealerID, string Type)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";
                if (Type == "VI")
                {
                    sDocName = "VI";
                }




                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(5, '0');

                if (sFinYear == "2016")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (VDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                    //sDocNo = sDealerCode + "/" + sDocName + "/" + sFinYear + "/" + sMaxDocNo;
                }
                else
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;

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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerPurchase", POId, POType, DealerID, HOBrID);
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
                dtInvoice = objDB.ExecuteStoredProcedureAndGetDataTable("GetDealerPurchaseInvList", iDealerId);

                drpInvNo.DataValueField = "ID";
                drpInvNo.DataTextField = "Name";
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
                SearchGrid.sGridPanelTitle = "Dealer Purchase Receipt List";
                SearchGrid.AddToSearchCombo("Invoice No");
                SearchGrid.AddToSearchCombo("Invoice Date");
                SearchGrid.AddToSearchCombo("Receipt Details");
                SearchGrid.AddToSearchCombo("Supplier Name");
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
                SearchGrid.sSqlFor = "DealerPurchaseDet";
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

                Session["TmpDlrPurReceipt"] = 0;

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
                    txtDMSInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvNo"]);
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["InvDate"], false);
                    txtMReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tr_no"]);
                    txtMReceiptDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["ref_date"], false);

                    txtParking.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ParkingLocation"]);

                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);

                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);

                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChassisNo"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngineNo"]);
                    txtSupplier.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SuppName"]);
                    txtSuppID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_ID"]);
                    txtPurchDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PurchDealer"]);
                    txtchassis_id.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_Id"]);
                    txtSAlePOID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalePO_Id"]);
                    txtPODet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PODet"]);




                    HidenGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]);
                    txtTaxType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Taxtype"]);
                    if (txtTaxType.Text == "C")
                    {
                        FillTaxdetials(txtTaxType.Text);
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                        drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                        txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                        txtVATID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTApp"]);
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        FillTaxdetials(txtTaxType.Text);
                       // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                        drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                        txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                        txtCSTID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATApp"]);
                    }
                    else
                    {
                        drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxPer.SelectedValue = "0";
                        txtVATID.Text = "0";
                        txtCSTID.Text = "0";
                        txtTaxApp.Text = "0";
                    }


                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Total"]);
                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Total"]);


                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GrandTotal"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Disc"]);



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PFCharges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Other"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax1"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax2"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTAmt"]);
                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATAmt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1Amt"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Amt"]);


                    drpTCSApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_App"]);
                    txtTCSTaxID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSPer"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSAmt"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_Per"]);

                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTDesc"]);
                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TAx1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Desc"]);
                    lblTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSDesc"]);

                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTPer"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATPer"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1Per"]);
                    txtTax2Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Per"]);

                    txtTaxApp1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1App"]);
                    txtTaxApp2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2App"]);




                }

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]) == "Y")
                {
                    lblCST.Text = "IGST";
                    lblVat.Text = "SGST";
                    lblTax1.Text = "CGST";
                    tdlbl.Visible = false; tdper.Visible = false; tdmount.Visible = false;
                }
                else
                {
                    lblCST.Text = "CST";
                    lblVat.Text = "VAT";
                    lblTax1.Text = "TAX1";
                    //tdvatamt.Visible = true; tdvatlbl.Visible = true; tdvattax.Visible = true; 
                    Table3.Visible = true; tdlbl.Visible = true; tdper.Visible = true; tdmount.Visible = true;
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



                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                }




                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true, "Nothing");
                }
                if (hdnCancle.Value == "Y")
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



                //if (Func.Convert.iConvertToInt(txtID.Text) != 0 && (bEnableControls == true))
                //{

                //    if (Func.Convert.iConvertToInt(txtInqID.Text) != 0)
                //    {
                //        //bConvertToInq.Enabled = false;
                //        //bHold.Enabled = false;
                //        //MakeEnableDisableControls(false, "Nothing");
                //    }
                //    else
                //    {
                //        //bConvertToInq.Enabled = true;
                //        //bHold.Enabled = true;
                //    }

                //}
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

        private void FillTaxdetials(string Taxtype)
        {
            try
            {
                clsDB objDB = new clsDB();
                if (txtUserType.Text == "6")
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8TaxDetailsFill", Func.Convert.iConvertToInt(Session["DealerID"]), Convert.ToString(HidenGST.Value), Taxtype);
                }
                else
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8TaxDetailsFill", Func.Convert.iConvertToInt(Session["iDealerID"]), Convert.ToString(HidenGST.Value), Taxtype);
                }
                if (dsState != null)
                {
                    drpTaxPer.DataSource = dsState.Tables[0];
                    drpTaxPer.DataTextField = "Name";
                    drpTaxPer.DataValueField = "ID";
                    drpTaxPer.DataBind();
                    drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex)
            {
            }
        }

        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            HdrID = objLead.bSaveDealerPurchase(iID, iDealerId, HOBrID,
                 txtDMSInvNo.Text, txtDMSInvDate.Text, Confirm, Cancel, Func.Convert.iConvertToInt(txtCSTID.Text), Func.Convert.iConvertToInt(txtVATID.Text),
                    Func.Convert.dConvertToDouble(txtPFCharges.Text), Func.Convert.dConvertToDouble(txtOthercharges.Text),
                    Func.Convert.dConvertToDouble(txtTotalAmt.Text), Func.Convert.dConvertToDouble(txtModelRate.Text), Func.Convert.dConvertToDouble(txtGrandTotal.Text),
                    Func.Convert.iConvertToInt(txtTax1ID.Text), Func.Convert.iConvertToInt(txtTax2ID.Text),
                    Func.Convert.dConvertToDouble(txtCSTAmt.Text), Func.Convert.dConvertToDouble(txtVatAmt.Text), Func.Convert.dConvertToDouble(txttax1Amt.Text),
                    Func.Convert.dConvertToDouble(txttax2Amt.Text), Func.Convert.iConvertToInt(txtTCSTaxID.Text), Func.Convert.dConvertToDouble(txtTCS.Text),
                     Func.Convert.dConvertToDouble(txtDisc.Text), Func.Convert.iConvertToInt(txtQty.Text),
                    Func.Convert.iConvertToInt(drpTCSApp.SelectedValue), txtTaxType.Text,0
                    , Func.Convert.iConvertToInt(txtPurchDealer.Text), Func.Convert.iConvertToInt(drpModel.SelectedValue)
                    , Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), Func.Convert.iConvertToInt(txtchassis_id.Text)
                    , Func.Convert.iConvertToInt(txtSuppID.Text )
                    , txtMReceiptNo.Text, txtMReceiptDate.Text, txtParking.Text, Func.Convert.iConvertToInt(txtSAlePOID.Text)
                    , Func.Convert.iConvertToInt(drpModelCat.SelectedValue),HidenGST.Value

             );


            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();
            if (objLead.bSaveM7Objectives(objDB, iDealerID, iHOBrId, dtDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }

            if (OrderStatus != "Y")
            {
                bSaveRecord = true;
            }
            else if (OrderStatus == "Y")
            {

                if (objLead.bSaveM7ClosureDetails(objDB, iDealerId, iHOBrId, dtClosureDetails, iID) == true)
                {
                    bSaveRecord = true;
                }
                else
                {
                    bSaveRecord = false;
                }


            }

            return bSaveRecord;

        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable, string type)
        {


          
            drpModel.Enabled = false;
            drpModelCat.Enabled = false;
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
            txtPODet.Enabled = false;
            //txtDeliveryNo.Enabled = false;
            txtParking.Enabled = bEnable;
            //drpVehConditon.Enabled = bEnable;
            txtPFCharges.Enabled = bEnable;
            txtOthercharges.Enabled = bEnable;
            drpTCSApp.Enabled = bEnable;
            txtDisc.Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }



        private void FillTaxDet()
        {
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxes", iDealerId, Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));


            if (dsCustType != null)
            {
                lblTax1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Desc"]);
                lblTax2.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2Desc"]);
                txttax1Per.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Per"]);
                txtTax2Per.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2Per"]);
                if (txtTaxType.Text == "C")
                {
                    lblCST.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedItem);
                    txtCSTPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    txtCSTID.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedValue);
                    txtVATID.Text = "0";
                }
                else if (txtTaxType.Text == "V")
                {
                    lblVat.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedItem);
                    txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    txtCSTID.Text = "0";
                    txtVATID.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedValue);
                }
                else
                {
                    lblCST.Text = "CST";
                    lblVat.Text = "VAT";
                    txtCSTID.Text = "0";
                    txtVATID.Text = "0";
                    txtVatPer.Text = "0";
                    txtCSTPer.Text = "0";
                }

                txtTax1ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1ID"]);
                txtTax2ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2ID"]);
                txtTaxApp.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp"]);
                txtTaxApp1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp1"]);
                txtTaxApp2.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp2"]);
            }
            else
            {
                lblCST.Text = "CST";
                lblVat.Text = "VAT";
                lblTax1.Text = "Tax1";
                lblTax2.Text = "Tax2";
            }

            if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
            {
                DataSet dsTaxCal = new DataSet();
                dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_TaxCalculationM8", Func.Convert.iConvertToInt(drpTaxPer.SelectedValue)
                , Func.Convert.iConvertToInt(txtTax1ID.Text), Func.Convert.iConvertToInt(txtTax2ID.Text), Func.Convert.iConvertToInt(txtTaxApp.Text)
                , Func.Convert.iConvertToInt(txtTaxApp1.Text), Func.Convert.iConvertToInt(txtTaxApp2.Text), Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text),
                Func.Convert.dConvertToDouble(txtDisc.Text)

                );

                if (dsTaxCal != null)
                {
                    if (txtTaxType.Text == "C")
                    {
                        txtCSTAmt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        txtVatAmt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    }
                    else
                    {
                        txtCSTAmt.Text = "0";
                        txtVatAmt.Text = "0";
                    }

                    txttax1Amt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]);


                }

            }
        }
        protected void txtModelRate_TextChanged(object sender, EventArgs e)
        {

            txtTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));
            txtTaxTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));

            if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
            {
                FillTaxDet();
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                {
                    FillTCSTaxDet();
                }
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
        }
        protected void drpTaxPer_SelectedIndexChanged(object sender, EventArgs e)
        {


            FillTaxDet();
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );

        }
        protected void drpInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDMSInvNo.Text = Func.Convert.sConvertToString(drpInvNo.SelectedItem);
            drpInvNo.Style.Add("display", "none");
            txtDMSInvNo.Style.Add("display", "");

            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            try
            {

                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetDealerPurchaseInvDet", iDealerId, Func.Convert.iConvertToInt(drpInvNo.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["InvDate"], false);

                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);

                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);

                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_No"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngineNo"]);
                    txtSupplier.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_name"]);
                    txtSuppID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SuppID"]);
                    txtPurchDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PurchDealer"]);
                    txtchassis_id.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_Id"]);
                    txtSAlePOID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalePO_ID"]);
                    txtPODet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PODet"]);

                    txtTaxType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Taxtype"]);
                    if (txtTaxType.Text == "C")
                    {
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                        drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                        txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                        txtVATID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTApp"]);
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                        drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                        txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                        txtCSTID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATApp"]);
                    }
                    else
                    {
                        drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxPer.SelectedValue = "0";
                        txtVATID.Text = "0";
                        txtCSTID.Text = "0";
                        txtTaxApp.Text = "0";
                    }


                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Total"]);
                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Total"]);


                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GrandTotal"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Disc"]);



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PFCharges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Other"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax1"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax2"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTAmt"]);
                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATAmt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1Amt"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Amt"]);


                    drpTCSApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_App"]);
                    txtTCSTaxID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSPer"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSAmt"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_Per"]);

                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTDesc"]);
                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TAx1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Desc"]);
                    lblTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSDesc"]);

                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTPer"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATPer"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1Per"]);
                    txtTax2Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Per"]);

                    txtTaxApp1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1App"]);
                    txtTaxApp2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2App"]);





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


        protected void txtDeliveryNo_TextChanged(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();

            //objMatReceipt = new clsMaterialReceipt();
            try
            {

                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetVehInvallDetailsDealerPO", iDealerId, Func.Convert.iConvertToInt(drpInvNo.SelectedValue), "DelNo");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //txtDeliveryNo.Enabled = false;
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Invoice_Date"], false);
                    //txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    //txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);
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




                }
                else
                {
                    //txtDeliveryNo.Enabled = true;
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
        protected void drpTCSApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) == 1)
            {
                FillTCSTaxDet();
            }
            else if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) == 2)
            {
                txtTCSPer.Text = "0";
                txtTCS.Text = "0";
                lblTCS.Text = "TCS";
                txtTCSTaxID.Text = "0";
            }

            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                Func.Convert.dConvertToDouble(txtTCS.Text)
                );
        }

        private void FillTCSTaxDet()
        {
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_TCSTaxCalculationM8", Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text),
                Func.Convert.dConvertToDouble(txtDisc.Text), Func.Convert.dConvertToDouble(txtVatAmt.Text),
                Func.Convert.dConvertToDouble(txtCSTAmt.Text), Func.Convert.dConvertToDouble(txttax1Amt.Text),
                Func.Convert.dConvertToDouble(txttax2Amt.Text), Func.Convert.dConvertToDouble(txtPFCharges.Text),
                Func.Convert.dConvertToDouble(txtOthercharges.Text),iDealerId);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtTCSPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSPer"]);
                txtTCS.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSAmt"]);
                lblTCS.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSDesc"]);
                txtTCSTaxID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSTaxID"]);
            }
        }

        protected void txtDisc_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
            {
                FillTaxDet();
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                {
                    FillTCSTaxDet();
                }
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
        }

        protected void txtPFCharges_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
            {
                FillTCSTaxDet();
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
        }

        protected void txtOthercharges_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
            {
                FillTCSTaxDet();
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
        }



    }
}