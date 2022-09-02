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
    public partial class frmVehOrderFormMTI : System.Web.UI.Page
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
        int iUserRole = 0;
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
                Location.bUseSpareDealerCode = false;
                Location.SetControlValue();


                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iUserRole = Func.Convert.iConvertToInt(Session["UserRole"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
              
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Session["UserType"].ToString();
         
         
                    FindHOBr(iDealerId);

                    ToolbarC.iFormIdToOpenForm = 65; 


          
            


                if (!IsPostBack)
                {
                    FillCombo();
                    FillStateCountry();
                    FillPlant();

                    DisplayPreviousRecord();
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                }

                SearchGrid.sGridPanelTitle = "Direct Billing Order Form Detials";
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
                    FillStateCountry();
                    FillPlant();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {


            iDealerId = Location.iDealerId;
          
                FindHOBr(iDealerId);
         
            FillCombo();
            FillStateCountry();
            FillPlant();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            DisplayPreviousRecord();
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
                        if (iMenuId == 657)
                        {
                            GenerateLeadNo("DD");
                        }
                        else
                        {
                            GenerateLeadNo("DM");
                        }

                        drpServiceDealer.SelectedValue = Func.Convert.sConvertToString(iDealerId);

                        drpSourceDealer.SelectedValue = Func.Convert.sConvertToString(iDealerId);
                        drpPDIDealer.SelectedValue = Func.Convert.sConvertToString(iDealerId);


                        ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                    }
                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;


                    if (txtPODate.Text == "" || txtPODate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter PO Date');</script>");
                        return;
                    }

                    if (drpM7Det.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select M7 Details');</script>");
                        return;
                    }
                    if (drpTaxtypeother.SelectedValue == "2" || drpTaxtypeother.SelectedValue == "3")
                    {
                        if (txtCSTCertificate.Text == "")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter CST Certificate No');</script>");
                            return;
                        }
                    }

                    if (drpBillingType.SelectedValue == "2")
                    {
                        if (txtEPCGNo.Text == "")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter EPCG No');</script>");
                          
                            return;
                        }
                    }
                    if (iMenuId == 657)
                    {
                        if (iID == 0)
                        {
                            GenerateLeadNo("DD");
                        }
                    }
                    else
                    {
                        if (iID == 0)
                        {
                            GenerateLeadNo("DM");
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


                    GenerateLeadNo("DMA");

                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

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
            Func.Common.BindDataToCombo(drpServiceDealer, clsCommon.ComboQueryType.DealerForCountry, 0);
            Func.Common.BindDataToCombo(drpPDIDealer, clsCommon.ComboQueryType.DealerForCountry, 0);
            Func.Common.BindDataToCombo(drpSourceDealer, clsCommon.ComboQueryType.SourceDlrVehOrderFormMTI, 0);
            Func.Common.BindDataToCombo(drpM4Financier, clsCommon.ComboQueryType.Financier, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);
            //Func.Common.BindDataToCombo(drpShipToparty, clsCommon.ComboQueryType.DealerForCountry, 0);
            //Func.Common.BindDataToCombo(drpSoldToParty, clsCommon.ComboQueryType.DealerForCountry, 0);

            //Func.Common.BindDataToCombo(drpVehPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            //FillEnquiry();
            drpModelGroup.SelectedValue = "1";


        }

        private void FillStateCountry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
             
                ddldeliverystate.Items.Clear();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                if (dsState != null)
                
                {
                    ddldeliverystate.DataSource = dsState.Tables[0];
                    ddldeliverystate.DataTextField = "Name";
                    ddldeliverystate.DataValueField = "ID";
                    ddldeliverystate.DataBind();
                    ddldeliverystate.Items.Insert(0, new ListItem("--Select--", "0"));
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


        private void FillPlant()
        {


            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantCode");

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpPlant.DataSource = dsCustType.Tables[0];
                    drpPlant.DataTextField = "Name";
                    drpPlant.DataValueField = "ID";
                    drpPlant.DataBind();
                    drpPlant.Items.Insert(0, new ListItem("--Select--", "0"));
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
                drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
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
                drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
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
            //Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID=" + modelgrp);
            //Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);
            //}



        }

        private void GenerateLeadNo(string Type)
        {
            // 'Replace Func.DB to objDB by Shyamal on 26032012

            MANART_DAL.clsDB objDB = new MANART_DAL.clsDB();
            try
            {
                DataSet dsDCode = new DataSet();

                if (iDealerId == 99999)
                {
                    DealerCode = "D099999";
                }
                else
                {
                    dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerId);

                    if (dsDCode.Tables[0].Rows.Count > 0)
                    {
                        DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                    }
                }

            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }


            if (iMenuId == 657)
            {
                if (Type == "DD")
                {

                    txtPONo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "DD"));

                }
            }
            else
            {
                if (Type == "DM")
                {

                    txtPONo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "DM"));

                }

                else  if (Type == "DMA")
                {

                    txtPOAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "DMA"));

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
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = (Convert.ToInt32(sFinYearChar) - 1).ToString();
                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";


                if (iMenuId == 657)
                {
                    if (Type == "DD")
                    {
                        sDocName = "DD";
                    }
                }
                else
                {
                    if (Type == "DM")
                    {
                        sDocName = "DM";
                    }
                    else if (Type == "DMA")
                    {
                        sDocName = "DMA";
                    }
                        
                }



                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);


                if (Type == "DD" || Type == "DM")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                }
                else
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                }
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


                ds = GetPO(iID, "New", iDealerId, iDealerId, 0);

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


                ds = GetPO(iID, "Max", iDealerId, iDealerId, iID);
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerVehOrderForm", POId, POType, DealerID, HOBrID, iM1ID);
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






        private void FillSelectionGrid()
        {
            try
            {

                if (iMenuId == 657)
                {
                    SearchGrid.sGridPanelTitle = "Direct Billing Order Form List";
                    SearchGrid.AddToSearchCombo("PO No");
                    SearchGrid.AddToSearchCombo("PO Date");
                    SearchGrid.AddToSearchCombo("M7 Details");
                    SearchGrid.AddToSearchCombo("PO Status");
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iDealerId;
                    SearchGrid.sSqlFor = "OrderFormDD";
                }
                else if (iMenuId == 679)
                {
                    SearchGrid.sGridPanelTitle = "Direct Billing Order Form List";
                    SearchGrid.AddToSearchCombo("PO No");
                    SearchGrid.AddToSearchCombo("PO Date");
                    SearchGrid.AddToSearchCombo("M7 Details");
                    SearchGrid.AddToSearchCombo("PO Status");
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
                    //SearchGrid.iDealerID = Func.Convert.iConvertToInt(txtDealerLoc.Text);
                    //SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iDealerId;
                    SearchGrid.sSqlFor = "OrderFormDM";
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
                    ds = GetPO(iID, "All", iDealerId, iDealerId, 0);
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
                    ds = GetPO(iID, "Max", iDealerId, iDealerId, 0);
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
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_GP"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Cat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);


                    drpPlant.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Plant"]);
                    txtDepot.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Depot"]);
                    txtRoadPermitNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RoadPermitNo"]);
                    txtRoadPermitDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["RoadPermitDate"], false);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);
                    txtChangeQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["changeqty"]);
                    txtPayerAdd.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PayerAddress"]);
                    txtDeliveryAdd.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryAddress"]);



                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                    txtContactPersonPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person_Phone"]);
                    txtDelContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_Person"]);
                    txtDelContactPhNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_phone_no"]);
                    txtDelvPin.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_pincode"]);
                    txtDelEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_E_mail"]);
                    txtDelPan.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_PANNo"]);
                    txtDelvGST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_GST_No"]);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryState"]) == "")
                    {
                        clsDB objDB = new clsDB();
                        ddldeliverystate.Items.Clear();
                        dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                        if (dsState != null)
                        {
                            ddldeliverystate.DataSource = dsState.Tables[0];
                            ddldeliverystate.DataTextField = "Name";
                            ddldeliverystate.DataValueField = "ID";
                            ddldeliverystate.DataBind();
                            ddldeliverystate.Items.Insert(0, new ListItem("--Select--", "0"));
                        }
                    }
                    else 
                    { 
                        ddldeliverystate.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryState"]); 
                    }
                    txtDelvName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_name"]);
                    txtDelvMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Mobile"]);





                    txtShipToParty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ship_ToParty"]);
                    txtSoldToParty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sold_ToParty"]);
                    drpServiceDealer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Service_Dealer"]);
                    drpSourceDealer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sorce_Dealer"]);
                    drpPDIDealer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PDIDealer"]);
                    

                    txtCSTNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTNO"]);
                    txtVATNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATNO"]);
                    txtPAN.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PANNO"]);
                    txtLBT.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LBTNO"]);
                    txtCSTExpiry.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["CSTExpDate"], false);
                    txtVATExpiry.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["VATExpDate"], false);


                    txtFormNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FormNo"]);
                    txtEntryTax.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EntryTax"]);
                    txtOlForm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["OnlineForm"]);
                    txtCheckpost.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CheckPost"]);
                    drpBillingLoc.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BillingLocation"]);
                    drpBillingType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BillingType"]);
                    hidengst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSt"]);
                    txtCustState.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                   
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSt"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]) != "N")
                    {
                        hidengst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSt"]);
          
                        lblConsignee.Text = "Place of Supplier:";
                        trvatno.Visible = false;
                        lblcstno.Text = "GSTIN NO:";
                        tdbilllocation1.Visible = false;
                        tdbilllocation.Visible = false;

                        if (iDealerId == 99999)
                        {

                            if (txtCustState.Text != "21")
                            {
                                FillTaxdetials1("C", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                            }
                            else
                            {
                                FillTaxdetials1("V", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                            }
                        }
                        else
                        {
                            FillTaxdetials1("C", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                        }
                    }
                    else if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]) == "N")
                    {
                        hidengst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]);
                        tdbilllocation1.Visible = true;
                        tdbilllocation.Visible = true;
                        trvatno.Visible = true;
                        lblcstno.Text = "CST NO:";
                        lblConsignee.Text = "Consignee (Ship To) Address:";
                        if (drpBillingLoc.SelectedValue == "1")
                        {
                            //FillTaxdetials("C", hidengst.Value);
                            FillTaxdetials("C", hidengst.Value);
                            // Func.Common.BindDataToCombo(drpTaxtypeother, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                            drpTaxtypeother.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TaxType"]);
                        }
                        else if (drpBillingLoc.SelectedValue == "2")
                        {
                            FillTaxdetials("V", hidengst.Value);
                            // Func.Common.BindDataToCombo(drpTaxtypeother, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                            drpTaxtypeother.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TaxType"]);

                        }
                        else
                        {
                            drpTaxtypeother.SelectedValue = "0";
                        }
                    }   

                    txtInsurance.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsuranceComp"]);
                    txtCSTCertificate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST_CertNo"]);
                    txtEPCGNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EPCG_No"]);

                    txtCovernoteno.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CoverNoteNo"]);
                    txtCovernoteExpiry.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["CoverNoteExp"], false);
                    txtremarks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remarks"]);
                    txtBankName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bank_Name"]);
                    txtchqno.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chq_No"]);
                    txtChqDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Chq_Date"], false);
                    txtPayment.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PaymentAmt"]);
                    drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Finc"]);
                    txtLocFinc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Loc"]);

                    //txtPaymentDetailsPO.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PaymentDetails"]);

                    // Appproval Details
                    txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                    txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approval_No"]);
                    txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Approval_Date"], false);
                    txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount_ApprovedAmt"]);
                    txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_DealerShare"]);
                    txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_MTIShare"]);
                    txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["remarks"]);
                    txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalAmt"]);

                    int M7ID = 0;
                    M7ID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["M7_Hdr"]);

                    if (M7ID == 0)
                    {
                        FillEnquiry();
                    }
                    else
                    {
                        FillEnquirySave(M7ID);
                        drpM7Det.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M7_Hdr"]);
                        txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                        txtMobileNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mobile"]);
                    }
                }




                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Cancel"]);
                hdnLost.Value = "N";



                if (txtID.Text == "0")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                else
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }


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
                    //TrFileDesc.Attributes.AddAttributes.
                    //TrFileDesc.Style.Add("display", "none");
                    //TrBtnUpload.Style.Add("display", "none");
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    //TrFileDesc.Style.Add("display", "none");
                    //TrBtnUpload.Style.Add("display", "none");
                }
                if (hdnLost.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    //TrFileDesc.Style.Add("display", "none");
                    //TrBtnUpload.Style.Add("display", "none");
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
                else if (hdnLost.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Lost");
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
                else if (hdnCancle.Value == "Y")
                {
                    //bConvertToInq.Enabled = false;
                    //bHold.Enabled = false;
                }
                else if (hdnConfirm.Value == "Y")
                {
                    //bConvertToInq.Enabled = false;
                    //bHold.Enabled = false;
                }




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

            if (iMenuId == 657)
            {
                HdrID = objLead.bSaveVehOrderForm(iID, iDealerId, iDealerId, txtPONo.Text, txtPODate.Text, Confirm, Cancel,
                         Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                        Func.Convert.iConvertToInt(drpModel.SelectedValue), Func.Convert.iConvertToInt(drpPlant.SelectedValue), txtDepot.Text, txtQty.Text,
                     txtModelRate.Text, txtRoadPermitNo.Text, txtRoadPermitDate.Text, Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtChangeQty.Text, ""
                     , "", iMenuId, "N", "N", "N", Func.Convert.iConvertToInt("2"), txtShipToParty.Text,
                      txtSoldToParty.Text,
                      txtPayerAdd.Text, txtDeliveryAdd.Text,
                     Func.Convert.iConvertToInt(drpServiceDealer.SelectedValue),
                     Func.Convert.iConvertToInt(drpSourceDealer.SelectedValue),
                     txtCSTNo.Text, txtVATNo.Text, txtPAN.Text, txtLBT.Text, txtCSTExpiry.Text,
                     txtVATExpiry.Text, txtFormNo.Text, txtEntryTax.Text, txtOlForm.Text,
                     txtCheckpost.Text, Func.Convert.iConvertToInt(drpBillingLoc.SelectedValue), Func.Convert.iConvertToInt(drpBillingType.SelectedValue),
                     Func.Convert.iConvertToInt(drpTaxtypeother.SelectedValue), txtInsurance.Text, txtCovernoteno.Text, txtCovernoteExpiry.Text, txtremarks.Text
                     , txtBankName.Text, txtchqno.Text, txtChqDate.Text
                     , Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtLocFinc.Text
                     , txtCSTCertificate.Text, txtEPCGNo.Text,iUserRole,Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
                     , Func.Convert.iConvertToInt(drpPDIDealer.SelectedValue),hidengst.Value,"","",""
                     ,"","","","","","","",txtPayment.Text,"","",0);
            }
            else
            {

                if (Confirm == "N")
                {
                    HdrID = objLead.bSaveVehOrderForm(iID, iDealerId, iDealerId, txtPONo.Text, txtPODate.Text, Confirm, Cancel,
                             Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                            Func.Convert.iConvertToInt(drpModel.SelectedValue), Func.Convert.iConvertToInt(drpPlant.SelectedValue), txtDepot.Text, txtQty.Text,
                         txtModelRate.Text, txtRoadPermitNo.Text, txtRoadPermitDate.Text, Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtChangeQty.Text, ""
                         , "", iMenuId, "N", "N", "N", Func.Convert.iConvertToInt("3"), txtShipToParty.Text,
                          txtSoldToParty.Text,
                          txtPayerAdd.Text, txtDeliveryAdd.Text,
                         Func.Convert.iConvertToInt(drpServiceDealer.SelectedValue),
                         Func.Convert.iConvertToInt(drpSourceDealer.SelectedValue)
                         , txtCSTNo.Text, txtVATNo.Text, txtPAN.Text, txtLBT.Text, txtCSTExpiry.Text,
                         txtVATExpiry.Text, txtFormNo.Text, txtEntryTax.Text, txtOlForm.Text,
                         txtCheckpost.Text, Func.Convert.iConvertToInt(drpBillingLoc.SelectedValue), Func.Convert.iConvertToInt(drpBillingType.SelectedValue),
                         Func.Convert.iConvertToInt(drpTaxtypeother.SelectedValue), txtInsurance.Text, txtCovernoteno.Text, txtCovernoteExpiry.Text, txtremarks.Text
                         , txtBankName.Text, txtchqno.Text, txtChqDate.Text
                         , Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtLocFinc.Text
                         , txtCSTCertificate.Text, txtEPCGNo.Text, iUserRole, Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
                         , Func.Convert.iConvertToInt(drpPDIDealer.SelectedValue), hidengst.Value, txtEmail.Text, txtContactPerson.Text, txtContactPersonPhone.Text
                     , txtpincode.Text, txtDelContactPerson.Text, txtDelContactPhNo.Text, txtDelvPin.Text, txtDelEmail.Text, txtDelPan.Text, txtDelvGST.Text
                     , txtPayment.Text, txtDelvName.Text, txtDelvMobile.Text, Func.Convert.iConvertToInt(ddldeliverystate.SelectedValue)
                     
                     );
                        
                }
                  else if (Confirm=="Y")
                {
                    HdrID = objLead.bSaveVehOrderForm(iID, iDealerId, iDealerId, txtPONo.Text, txtPODate.Text, Confirm, Cancel,
                          Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                         Func.Convert.iConvertToInt(drpModel.SelectedValue), Func.Convert.iConvertToInt(drpPlant.SelectedValue), txtDepot.Text, txtQty.Text,
                      txtModelRate.Text, txtRoadPermitNo.Text, txtRoadPermitDate.Text, Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtChangeQty.Text,
                      txtPOAppNo.Text,txtPODate.Text,  iMenuId, "Y", "Y", "Y", Func.Convert.iConvertToInt("3"), txtShipToParty.Text,
                       txtSoldToParty.Text,
                       txtPayerAdd.Text, txtDeliveryAdd.Text,
                      Func.Convert.iConvertToInt(drpServiceDealer.SelectedValue),
                      Func.Convert.iConvertToInt(drpSourceDealer.SelectedValue)
                      , txtCSTNo.Text, txtVATNo.Text, txtPAN.Text, txtLBT.Text, txtCSTExpiry.Text,
                      txtVATExpiry.Text, txtFormNo.Text, txtEntryTax.Text, txtOlForm.Text,
                      txtCheckpost.Text, Func.Convert.iConvertToInt(drpBillingLoc.SelectedValue), Func.Convert.iConvertToInt(drpBillingType.SelectedValue),
                      Func.Convert.iConvertToInt(drpTaxtypeother.SelectedValue), txtInsurance.Text, txtCovernoteno.Text, txtCovernoteExpiry.Text, txtremarks.Text
                      , txtBankName.Text, txtchqno.Text, txtChqDate.Text
                      , Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtLocFinc.Text
                      , txtCSTCertificate.Text, txtEPCGNo.Text, iUserRole, Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
                      , Func.Convert.iConvertToInt(drpPDIDealer.SelectedValue), hidengst.Value,txtEmail.Text,txtContactPerson.Text,txtContactPersonPhone.Text
                     ,txtpincode.Text,txtDelContactPerson.Text,txtDelContactPhNo.Text,txtDelvPin.Text,txtDelEmail.Text,txtDelPan.Text,txtDelvGST.Text
                     , txtPayment.Text, txtDelvName.Text, txtDelvMobile.Text, Func.Convert.iConvertToInt(ddldeliverystate.SelectedValue)
                     );
                }

            }


            return HdrID;

        }

        private void FillTaxdetials(string Taxtype, string GST)
        {
            try
            {
                clsDB objDB = new clsDB();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8TaxDetailsFill", iDealerId, GST, Taxtype);
                if (dsState != null)
                {
                    drpTaxtypeother.DataSource = dsState.Tables[0];
                    drpTaxtypeother.DataTextField = "Name";
                    drpTaxtypeother.DataValueField = "ID";
                    drpTaxtypeother.DataBind();
                    drpTaxtypeother.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();
            //if (objLead.bSaveM7Objectives(objDB, iDealerID, iHOBrId, dtDetails, iID) == true)
            //{
            //    bSaveRecord = true;
            //}
            //else
            //{
            //    bSaveRecord = false;
            //}

            if (bSaveAttachedDocuments() == false) return bSaveRecord;
            if (objLead.bSaveWarrantyClaimFileAttachDetails(objDB, dtFileAttach, iID) == true)
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
            txtRoadPermitDate.Enabled = bEnable;
            txtRoadPermitNo.Enabled = bEnable;
            txtDepot.Enabled = bEnable;
            drpPlant.Enabled = bEnable;
            drpM7Det.Enabled = bEnable;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelGroup.Enabled = false;
            txtQty.Enabled = false;
            txtCustName.Enabled = false;
            txtMobileNo.Enabled = false;
            drpModelCat.Enabled = false;
            txtShipToParty.Enabled = bEnable;
            txtSoldToParty.Enabled = bEnable;
            drpServiceDealer.Enabled = bEnable;
            drpSourceDealer.Enabled = bEnable;
            drpPDIDealer.Enabled = bEnable;
            txtPayerAdd.Enabled = bEnable;
            txtDeliveryAdd.Enabled = bEnable;
            txtCSTNo.Enabled = bEnable;
            txtVATNo.Enabled = bEnable;
            txtPAN.Enabled = bEnable;
            txtLBT.Enabled = bEnable;
            txtCSTExpiry.Enabled = bEnable;
            txtVATExpiry.Enabled = bEnable;


            txtpincode.Enabled = bEnable;
            txtEmail.Enabled = bEnable;
            txtContactPerson.Enabled = bEnable;
            txtContactPersonPhone.Enabled = bEnable;
            txtDelContactPerson.Enabled = bEnable;
            txtDelContactPhNo.Enabled = bEnable;
            txtDelvPin.Enabled = bEnable;
            txtDelEmail.Enabled = bEnable;
            txtDelPan.Enabled = bEnable;
            txtDelvGST.Enabled = bEnable;


            txtFormNo.Enabled = bEnable;
            txtEntryTax.Enabled = bEnable;
            txtOlForm.Enabled = bEnable;
            txtCheckpost.Enabled = bEnable;
            drpBillingLoc.Enabled = bEnable;
            drpBillingType.Enabled = bEnable;
            drpTaxtypeother.Enabled = bEnable;

            txtInsurance.Enabled = bEnable;
            txtCSTCertificate.Enabled = false;
         

            //if (drpBillingType.SelectedValue == "2")
            //{
                txtEPCGNo.Enabled = bEnable;
            //}
            //else
            //{
            //    txtEPCGNo.Enabled = false;
            //}

            txtCovernoteno.Enabled = bEnable;
            txtCovernoteExpiry.Enabled = bEnable;
            txtremarks.Enabled = bEnable;
            txtBankName.Enabled = bEnable;
            txtchqno.Enabled = bEnable;
            txtChqDate.Enabled = bEnable;
            txtPayment.Enabled = bEnable;
            drpM4Financier.Enabled = bEnable;
            txtLocFinc.Enabled = bEnable;




            //if (drpVehPOType.SelectedValue == "2")
            //{
            //drpModel.Enabled = false;
            //drpModelCode.Enabled = false;
            //drpModelGroup.Enabled = false;
            //txtQty.Enabled = false;
            //txtPaymentDetailsPO.Enabled = false;
            //}
            //else
            //{
            //    drpModel.Enabled = bEnable;
            //    drpModelCode.Enabled = bEnable;
            //    drpModelGroup.Enabled = bEnable;
            //    txtQty.Enabled = bEnable;
            //    txtPaymentDetailsPO.Enabled = bEnable;
            //}

            txtChangeQty.Enabled = false;
            txtModelRate.Enabled = false;


            txtAppNo.Enabled = false;
            txtAppDate.Enabled = false;
            txtAppDisc.Enabled = false;
            txtAppDealershare.Enabled = false;
            txtAppMTIshare.Enabled = false;
            txtAppFinalAmt.Enabled = false;
            txtAppremarks.Enabled = false;

            //if (Func.Convert.iConvertToInt(drpVehPOType.SelectedValue) == 2)
            //{
            //    AppDet.Visible = true;
            //}
            //else
            //{
            //    AppDet.Visible = false;
            //}

            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }

        private void FillEnquiry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_getEnquiryForOrderForm", iDealerId, iDealerId, iMenuId);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpM7Det.DataSource = dsCustType.Tables[0];
                    drpM7Det.DataTextField = "Name";
                    drpM7Det.DataValueField = "ID";

                    drpM7Det.DataBind();
                    drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
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

        private void FillEnquirySave(int M7ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquirySave", iDealerId, iDealerId, M7ID);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpM7Det.DataSource = dsCustType.Tables[0];
                    drpM7Det.DataTextField = "Name";
                    drpM7Det.DataValueField = "ID";
                    drpM7Det.DataBind();
                    //drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
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

       
        protected void drpM7Det_SelectedIndexChanged(object sender, EventArgs e)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();

                if (Func.Convert.iConvertToInt(drpM7Det.SelectedValue) != 0)
                {


                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerId, iDealerId, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                    if (ds != null)
                    {
                        txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                        txtMobileNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mobile"]);
                        drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                        drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                        drpModelGroup.SelectedValue = "1";
                        FillModel();
                        drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                        txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]); ;
                        txtChangeQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                        drpPlant.SelectedValue = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);
                        drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Finc"]);
                        
                        txtLocFinc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Branch"]);

                        //txtCSTNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                        txtVATNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                        txtPAN.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PAN"]);
                        txtLBT.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LBT"]);
                        // Appproval Details
                        txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                        txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approval_No"]);
                        txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Approval_Date"], false);
                        txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount_ApprovedAmt"]);
                        txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_DealerShare"]);
                        txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_MTIShare"]);
                        txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["remarks"]);
                        txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalAmt"]);
                        txtCustState.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                        if (hidengst.Value == "Y")
                        {
                            drpTaxtypeother.Items.Clear();
                            if (iDealerId == 99999)
                            {
                                if (txtCustState.Text != "21")
                                {
                                    FillTaxdetials1("C", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                                }
                                else
                                {
                                    FillTaxdetials1("V", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                                }
                            }
                            else
                            {
                                FillTaxdetials1("C", Convert.ToInt32(ds.Tables[0].Rows[0]["Model_ID"]));
                            }
                        }

                        txtDeliveryAdd.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["deliveryAdd1"]);
                        txtCSTNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST_NO"]);
                        txtSoldToParty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BillingAdd"]);
                        txtchqno.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChRTGSDet"]);
                        txtChqDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["ChRTGSDate"], false);
                        txtPayment.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PaymentAmt"]);

                        txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
                        txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                        txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                        txtContactPersonPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person_Phone"]);
                        txtDelContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_Person"]);
                        txtDelContactPhNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_phone_no"]);
                        txtDelvPin.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_pincode"]);
                        txtDelEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_E_mail"]);
                        txtDelPan.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_PANNo"]);
                        txtDelvGST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_GST_No"]);

                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryState"]) == "")
                        {
                            
                            ddldeliverystate.Items.Clear();
                            dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                            if (dsState != null)
                            {
                                ddldeliverystate.DataSource = dsState.Tables[0];
                                ddldeliverystate.DataTextField = "Name";
                                ddldeliverystate.DataValueField = "ID";
                                ddldeliverystate.DataBind();
                                ddldeliverystate.Items.Insert(0, new ListItem("--Select--", "0"));
                            }
                        }
                        else
                        {
                            ddldeliverystate.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryState"]);
                        }
                        txtDelvName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_name"]);
                        txtDelvMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Mobile"]);



                    }


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
                Response.TransmitFile((sPath + "Vehicle Purchase\\VehOrderFormMTI" + "\\" + fileNames));
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
                            sSourceFileName1 = Func.Convert.sConvertToString(iDealerId) + "_" + txtPONo.Text + "_" + sSourceFileName;
                            sSourceFileName1 = sSourceFileName1.Replace("/", "");
                            dr["File_Names"] = sSourceFileName1;
                            //dr["File_Names"] = Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                            dr["UserId"] = Func.Convert.sConvertToString(Session["UserID"]);
                            dr["Status"] = "S";


                            //Saving it in temperory Directory.                                       
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Vehicle Purchase\\VehOrderFormMTI");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }

                            //uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));
                            uploads[i].SaveAs((sPath + "Vehicle Purchase\\VehOrderFormMTI" + "\\" + sSourceFileName1));

                            strNewPath = sPath + "Vehicle Purchase\\VehOrderFormMTI" + "\\" + sSourceFileName1;
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
            if (dtFileAttach.Rows.Count != 0)
            {
                if (dtFileAttach != null)
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




        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            txtChangeQty.Text = txtQty.Text;
        }
        protected void txtSoldToParty_TextChanged(object sender, EventArgs e)
        {
            txtShipToParty.Text = txtSoldToParty.Text;
            txtDeliveryAdd.Text = txtSoldToParty.Text;
        }
        protected void drpBillingLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBillingLoc.SelectedValue == "1")
            {
                FillTaxdetials("C", hidengst.Value);
                //Func.Common.BindDataToCombo(drpTaxtypeother, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);


            }
            else if (drpBillingLoc.SelectedValue == "2")
            {
                FillTaxdetials("V", hidengst.Value);
               // Func.Common.BindDataToCombo(drpTaxtypeother, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);


            }
            else if (drpBillingLoc.SelectedValue == "0")
            {
                drpTaxtypeother.ClearSelection();
                Func.Common.BindDataToCombo(drpTaxtypeother, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + 0);


            }

        }

        private void FillTaxdetials1(string Taxtype, int Model_ID)
        {
            try
            {

                clsDB objDB = new clsDB();


                if (iDealerId == 99999)
                {

                    if (Taxtype == "C")
                    {
                        drpTaxtypeother.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxtypeother.Items.Insert(0, new ListItem("IGST", "1"));
                    }
                    else if (Taxtype == "V")
                    {
                        drpTaxtypeother.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxtypeother.Items.Insert(0, new ListItem("SGST/CSGT", "1"));
                    }
                    else
                    {
                        drpTaxtypeother.Items.Clear();
                    }

                }
                else
                {



                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8HSNDetailsFill", iDealerId, Convert.ToString(hidengst.Value), Taxtype, Model_ID);
                    if (dsState != null)
                    {
                        drpTaxtypeother.DataSource = dsState.Tables[0];
                        drpTaxtypeother.DataTextField = "Name";
                        drpTaxtypeother.DataValueField = "ID";
                        drpTaxtypeother.DataBind();
                        drpTaxtypeother.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxtypeother.SelectedValue = Func.Convert.sConvertToString(dsState.Tables[0].Rows[0]["ID"]);
                    }
                    else
                    {

                        drpTaxtypeother.Items.Clear();

                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

    }
}