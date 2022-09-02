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

namespace MANART.Forms.CRM
{
    public partial class frmCRMCreation : System.Web.UI.Page
    {
        private int iCRMID = 0;
        private int iID;
        int iUserId = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        int iMenuId = 0;
        string DealerCode;
        string ISDealer = "";
        clsCRM objCRM = null;
        private DataTable dtDealerFeedback = new DataTable();
        private DataTable dtMTIFeedback = new DataTable();
        private bool bDetailsRecordExist = false;
        private string sSelText, sSelType;
        private int CustID;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                ToolbarC.iValidationIdForSave = 74;
                ToolbarC.iValidationIdForConfirm=74;
                ToolbarC.bUseImgOrButton = true;
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();
                clsCustomer ObjCust = new clsCustomer();


                clsEGPSupplier ObjSup = new clsEGPSupplier();
                DataSet ds = new DataSet();
                DataSet dsDealer = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                txtMenuid.Text = Func.Convert.sConvertToString(Request.QueryString["MenuID"]);
                txtUserid.Text = Func.Convert.sConvertToString(Session["UserID"]);
                // MD User Related Changes 
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                // MD User Related Changes 
                if (iMenuId == 670) //MTI User/call center user 
                {
                    Location.Visible = false;
                    callcenterRemark.Visible = true;


                }
                if (iMenuId == 665) //dealer User 
                {
                    Panel7.Visible = false;
                    PMTIFeedback.Visible = false;
                    callcenterRemark.Visible = false;
                }

              

                if (!IsPostBack)
                {
                    if (iMenuId == 665)
                    {
                        if (Func.Convert.sConvertToString(Session["sDealerCode"]).Substring(0, 1) == "R")
                        {
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                        }
                        else
                        {

                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                        }
                    }
                    else
                    {

                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    }

                }




                //if (iCRMID != 0)
                //{
                //    GetDataAndDisplay();
                //}
                SearchGrid.sGridPanelTitle = "Call Ticket List";
                FillSelectionGrid();
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                SetDocumentDetails();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // MD User Related Changes 
        protected override void OnPreRender(EventArgs e)
        {
            if (txtUserType.Text == "6")
            {
                  iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                  Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
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
        // Function Use for Readonly User
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();

                if (objCommon.sUserRole == "19")
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
        // MD User Related Changes 
        private void SetDocumentDetails()
        {


            //lblSelectChassis.Attributes.Add("onclick", " return ShowChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Session["HOBR_ID"].ToString() + "')");
            lblSelectChassis.Attributes.Add("onclick", " return ShowChassisMaster_CRM(this)");
         //   lblSelectCustomer.Attributes.Add("onclick", " return ShowCustomerMaster_CRM(this)");
        }
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Ticket No");
                SearchGrid.AddToSearchCombo("Chassis No");
               //SearchGrid.AddToSearchCombo("Ticket Date");
                SearchGrid.AddToSearchCombo("Record Status");
              

                if (iMenuId == 670)
                {
                    //SearchGrid.iDealerID = 9999;
                    //SearchGrid.sModelPart = "N";
                    SearchGrid.iDealerID = iUserId;
                    SearchGrid.sModelPart = "N";
                }
                if (iMenuId == 665)
                {
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sModelPart = "Y";
                }


               //// if (iUserId == 7043)  // Amrit call center login
               //     if (iUserId == 24)  // Amrit call center login
                if (iUserId == 24 || iUserId == 308 || iUserId == 318 || iUserId == 319 || iUserId == 320 || iUserId == 321 || iUserId == 728)
                {
                    SearchGrid.sSqlFor = "CRMService_ALL";
                }
                else
                {
                    SearchGrid.sSqlFor = "CRMService";
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iCRMID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();


        }
        private void DisplayCurrentRecord(string Type)
        {
            try
            {
                objCRM = new clsCRM();
                DataSet ds = new DataSet();
                if (iMenuId == 670)
                {
                    ISDealer = "N";
                }
                if (iMenuId == 665)
                {
                    ISDealer = "Y";

                }

                ds = objCRM.GetCRM(iCRMID, Type, ISDealer);

                //sNew = "N";
                DisplayData(ds, Type);
                objCRM = null;


                ds = null;
                objCRM = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void GetDataAndDisplay()
        {
            try
            {
                objCRM = new clsCRM();
                DataSet ds = new DataSet();
                Session["Temp_call"] = 1;

                if (iMenuId == 670) //MTI login Menu id 
                {
                    ISDealer = "N";

                }
                if (iMenuId == 665) //Dealer login Menu id 
                {
                    ISDealer = "Y";

                }
                if (iCRMID != 0)
                {
                    ds = objCRM.GetCRM(iCRMID, "Max", ISDealer);

                    //sNew = "N";
                    DisplayData(ds,"Max");
                    objCRM = null;
                }
                else
                {
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objCRM = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void DisplayData(DataSet ds, string Type)
        {
            try
            {
                bool bEnableControls = true;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                txtcallcenterRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CallCenterRemark"]);
                txtDetailsRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerRemark"]);
                txtTicketNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_no"]);
                txtTicketDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_Date"]);
                txtAlternateContactNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AlternateContact_No"]);
                if (Type == "New")
                {
                    FillCallType_New();
                    drpCallType.Enabled = true;
                }
                else
                {
                    FillCallType();
                    drpCallType.Enabled = false;
                }

                drpCallType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]);

                txtChassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_ID"]);
                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Reg_No"]);
                txtModelID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                txtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_code"]);
                txtModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_name"]);
                txtCRMCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_Cust_ID"]);
                txtGlobalCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Global_Cust_ID"]);
                txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);
                txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone"]);
                txtPinCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
                txtState.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["State"]);
                txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
                //string DstateID = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DstateID"]);
                //if( DstateID=="0")
                //{
                //    drpState.SelectedValue = "0";
               
                //}
                //else
                //{
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DstateID"]);
                //}
                Filldistrict();
                drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DDistrictID"]);
                FillDealer();
                drpDealerName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerID"]);
                txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer Code"]);
                txtDCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_City"]);
               
                txtDRegion.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DRegion"]);

                Session["DealerFeedback"] = null;
                dtDealerFeedback = ds.Tables[1];
                Session["DealerFeedback"] = dtDealerFeedback;
                BindDataToGridDealerFeedback();

                Session["MTIFeedback"] = null;
                dtMTIFeedback = ds.Tables[2];
                Session["MTIFeedback"] = dtMTIFeedback;
                BindDataToGridDealerFeedback();

                BindDataToGridMTIFeedback();
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Cancel"]) == "Y")
                {
                    btnFeedBackSave.Style.Add("display", "none");
                    lblSelectChassis.Style.Add("display", "none");
                    lblSelectCustomer.Style.Add("display", "none");
                    ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                }
               
                else
                {
                    btnFeedBackSave.Style.Add("display", "none");
                    lblSelectChassis.Style.Add("display", "");
                    lblSelectCustomer.Style.Add("display", "");
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, true);
                   // ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                }
                if (ISDealer == "Y")
                {
                    if (Func.Convert.sConvertToString(Session["sDealerCode"]).Substring(0, 1) == "R")
                    {
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                    }
                }
                    
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y"
                    && ((Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]) == "" 
                    && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_confirm"]) == "") 
                    || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_cancel"]) == "Y")
                    && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Dealer"]) == "N" && ISDealer == "N"
                    && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "N")
                {
                    CallOpen.Style.Add("display", "");
                }
                else
                {
                    CallOpen.Style.Add("display", "none");
                }
                // After confimation  call ticket callcelled 
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" 
                    && ((Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]) == "" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_confirm"]) == "") || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_cancel"]) == "Y")
                    && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Cancel"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]) != "5")
                {
                    //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Dealer"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "N" && ISDealer == "Y")
                {
                    Ack1.Style.Add("display", "");
                    btnFeedBackSave.Style.Add("display", "none");
                    txtDetailsRemark.ReadOnly = true;
                    txtDetailsRemark.Enabled = false;
                }
                else
                {
                    Ack1.Style.Add("display", "none");
                    btnFeedBackSave.Style.Add("display", "none");
                    txtDetailsRemark.ReadOnly = false;
                    txtDetailsRemark.Enabled = true;
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && ISDealer == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]) == "5") && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "N")
                {
                    DealerCallClosure.Style.Add("display", "");
                    btnFeedBackSave.Style.Add("display", "none");
                }
                else
                {
                    DealerCallClosure.Style.Add("display", "none");
                    btnFeedBackSave.Style.Add("display", "none");
                }

               // if (iUserId == 7043)  // Amrit call center login
                   // if (iUserId == 24)  // Amrit call center login
                if (iUserId == 24 || iUserId == 308 || iUserId == 318 || iUserId == 319 || iUserId == 320 || iUserId == 321 || iUserId == 728)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Dealer"]) == "N")
                    {
                        Panel7.Visible = true;
                        PDealerFeedback.Visible = false;
                        PMTIFeedback.Visible = false;
                        Calclose.Style.Add("display", "none");
                        btnFeedBackSave.Style.Add("display", "none");
                    }
                   // if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "N")
                   // {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "N" && (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]) == "5") && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "Y")
                        {
                        Panel7.Visible = true;
                        PDealerFeedback.Visible = true;
                        PMTIFeedback.Visible = true;
                        Calclose.Style.Add("display", "");
                        btnFeedBackSave.Style.Add("display", "");
                        Calclose.Style.Add("display", ""); 
                    }
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "Y")
                    {
                        Panel7.Visible = true;
                        PDealerFeedback.Visible = true;
                        PMTIFeedback.Visible = true;
                        Calclose.Style.Add("display", "none");
                        btnFeedBackSave.Style.Add("display", "none");
                    }
                    
                }


                    if (iUserId == 1076) // call center id for call creation
                    {
                        Panel7.Visible = false;
                        PDealerFeedback.Visible = false;
                        PMTIFeedback.Visible = false;
                        Calclose.Style.Add("display", "none");
                    }
                    if (txtUserType.Text == "6")
                    {
                        lblSelectChassis.Style.Add("display", "none");
                        lblSelectCustomer.Style.Add("display", "none");
                        btnAcknowledge.Style.Add("display", "none");
                        btnDealerCallClosure.Style.Add("display", "none");
                        btnFeedBackSave.Style.Add("display", "none");
                        btnCalClose.Style.Add("display", "none");
                    }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGridDealerFeedback()
        {
            //If No Data in Grid
            if (Session["DealerFeedback"] == null)
            {
                Session["DealerFeedback"] = dtDealerFeedback;
            }
            else
            {
                dtDealerFeedback = (DataTable)Session["DealerFeedback"];
            }
            Session["DealerFeedback"] = dtDealerFeedback;
            DealerFeedback.DataSource = dtDealerFeedback;
            DealerFeedback.DataBind();


                 }
        private void BindDataToGridMTIFeedback()
        {
            //If No Data in Grid
            if (Session["MTIFeedback"] == null)
            {
                Session["MTIFeedback"] = dtMTIFeedback;
            }
            else
            {
                dtMTIFeedback = (DataTable)Session["MTIFeedback"];
            }
            Session["MTIFeedback"] = dtMTIFeedback;
            MTIFeedback.DataSource = dtMTIFeedback;
            MTIFeedback.DataBind();
            SetGridControlPropertyMTIFeedback(false);
         
        }

        private void SetGridControlPropertyMTIFeedback(bool bRecordIsOpen)
        {

           
            int idtRowCnt = 0;
            dtMTIFeedback = (DataTable)Session["MTIFeedback"];
            for (int iRowCnt = 0; iRowCnt < MTIFeedback.Rows.Count; iRowCnt++)
            {

                DropDownList drpFeedBackStatus1 = (DropDownList)MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus1");
                Func.Common.BindDataToCombo(drpFeedBackStatus1, clsCommon.ComboQueryType.MTIFeedback, 0);
                drpFeedBackStatus1.SelectedValue = Func.Convert.sConvertToString(dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus1"]);


                DropDownList drpFeedBackStatus2 = (DropDownList)MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus2");
                Func.Common.BindDataToCombo(drpFeedBackStatus2, clsCommon.ComboQueryType.MTIFeedback, 0);
                drpFeedBackStatus2.SelectedValue = Func.Convert.sConvertToString(dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus2"]);

                DropDownList drpFeedBackStatus3 = (DropDownList)MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus3");
                Func.Common.BindDataToCombo(drpFeedBackStatus3, clsCommon.ComboQueryType.MTIFeedback, 0);
                drpFeedBackStatus3.SelectedValue = Func.Convert.sConvertToString(dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus3"]);

                DropDownList drpScale = (DropDownList)MTIFeedback.Rows[iRowCnt].FindControl("drpScale");
                Func.Common.BindDataToCombo(drpScale, clsCommon.ComboQueryType.MTIFeedbackScale, 0);
                drpScale.SelectedValue = Func.Convert.sConvertToString(dtMTIFeedback.Rows[iRowCnt]["Scale"]);

               if (drpFeedBackStatus1.SelectedValue == "5" || drpFeedBackStatus1.SelectedValue == "6")
                {
                    drpFeedBackStatus2.Enabled = false;
                    drpFeedBackStatus3.Enabled = false;
                    drpScale.Enabled = true;
                }
               
                else if (drpFeedBackStatus2.SelectedValue == "5" || drpFeedBackStatus2.SelectedValue == "6")
                {
                    drpFeedBackStatus1.Enabled = false;
                    drpFeedBackStatus3.Enabled = false;
                    drpScale.Enabled = true;
                }
               
                else if (drpFeedBackStatus3.SelectedValue == "5" || drpFeedBackStatus3.SelectedValue == "6")
                {
                    drpFeedBackStatus1.Enabled = false;
                    drpFeedBackStatus2.Enabled = false;
                    drpScale.Enabled = true;
                }

               drpScale.Enabled = true;
              
                    idtRowCnt = idtRowCnt + 1;
                }

            }

        private void FillCallType()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCallType;
                // Created By Vikram on 23.06.16
               
                    dsCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "1", 0);
               

                if (dsCallType != null)
                {
                    drpCallType.DataSource = dsCallType.Tables[0];
                    drpCallType.DataTextField = "Name";
                    drpCallType.DataValueField = "ID";
                    drpCallType.DataBind();
                    drpCallType.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DataSet dsSubCallType;
                // Created By Vikram on 23.06.16
                dsSubCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "2", Func.Convert.iConvertToInt(drpCallType.SelectedItem.Value));
                if (dsSubCallType != null)
                {
                    drpCallSubType.DataSource = dsSubCallType.Tables[0];
                    drpCallSubType.DataTextField = "Name";
                    drpCallSubType.DataValueField = "ID";
                    drpCallSubType.DataBind();
                    drpCallSubType.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DataSet dsState;

                //   dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", 0);
                // iUserId

                if (iMenuId == 670) //MTI login Menu id 
                {

                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", iUserId);
                    if (dsState != null)
                    {
                        drpState.DataSource = dsState.Tables[0];
                        drpState.DataTextField = "Name";
                        drpState.DataValueField = "ID";
                        drpState.DataBind();
                        drpState.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }
                if (iMenuId == 665) //Dealer login Menu id 
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "8", iDealerID);
                    if (dsState != null)
                    {
                        drpState.DataSource = dsState.Tables[0];
                        drpState.DataTextField = "Name";
                        drpState.DataValueField = "ID";
                        drpState.DataBind();
                        drpState.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }


                Filldistrict();
                FillDealer();

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

        private void FillCallType_New()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCallType;
                // Created By Vikram on 23.06.16
                if (txtUserType.Text == "3")
                {
                    dsCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "9", 0);
                }
                else
                {
                    dsCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "1", 0);
                }

                if (dsCallType != null)
                {
                    drpCallType.DataSource = dsCallType.Tables[0];
                    drpCallType.DataTextField = "Name";
                    drpCallType.DataValueField = "ID";
                    drpCallType.DataBind();
                    drpCallType.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DataSet dsSubCallType;
                // Created By Vikram on 23.06.16
                dsSubCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "2", Func.Convert.iConvertToInt(drpCallType.SelectedItem.Value));
                if (dsSubCallType != null)
                {
                    drpCallSubType.DataSource = dsSubCallType.Tables[0];
                    drpCallSubType.DataTextField = "Name";
                    drpCallSubType.DataValueField = "ID";
                    drpCallSubType.DataBind();
                    drpCallSubType.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DataSet dsState;
    
             //   dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", 0);
               // iUserId

                if (iMenuId == 670) //MTI login Menu id 
                {
                  
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", iUserId);
                    if (dsState != null)
                    {
                        drpState.DataSource = dsState.Tables[0];
                        drpState.DataTextField = "Name";
                        drpState.DataValueField = "ID";
                        drpState.DataBind();
                        drpState.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }
                if (iMenuId == 665) //Dealer login Menu id 
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "8", iDealerID);
                    if (dsState != null)
                    {
                        drpState.DataSource = dsState.Tables[0];
                        drpState.DataTextField = "Name";
                        drpState.DataValueField = "ID";
                        drpState.DataBind();
                        drpState.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }


              

                Filldistrict();
                FillDealer();
                
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
        private void GenerateTicketNo()
        {
            clsDB objDB = new clsDB();
            try
            {
                if (iMenuId == 670)
                {
                    DealerCode = "D009999";

                    iDealerID = 9999;
                }
                if (iMenuId == 665)
                {
                    DealerCode = Location.sDealerCode;
                }



                objCRM = new clsCRM();
                txtTicketNo.Text = Func.Convert.sConvertToString(objCRM.GenerateTicketNo(DealerCode, iDealerID, Convert.ToInt16(drpCallType.SelectedValue)));
                txtTicketDate.Text = Func.Common.sGetCurrentDate(1, false);
                


            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
        private bool bValidateRecord(bool bConfirm)
        {

            string sMessage = " ";
            bool bValidateRecord = true;
            if (drpCallType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Call Type.";
                bValidateRecord = false;
            }
            else if (txtTicketDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Ticket date.";
                //txtID.Text = "";
                bValidateRecord = false;
            }
            else if (txtChassisID.Text == "0")
            {
                sMessage = sMessage + "\\n Select the Chassis No.";
                // txtID.Text = "";
                bValidateRecord = false;
            }
            if (bConfirm == true)
            {
                if (txtAlternateContactNo.Text.Trim() == "")
                {
                    sMessage = sMessage + "\\n Please Enter the Alternate Contact No.";
                    bValidateRecord = false;
                }

                if ((txtAlternateContactNo.Text.Trim()).Length < 11)
                {
                    sMessage = sMessage + "\\n Alternate Contact No length should be minimum 11.";
                    bValidateRecord = false;
                }
            }


            //if (iMenuId == 670)
            //{
            //    if (drpDealerName.SelectedValue == "0")
            //    {
            //        sMessage = sMessage + "\\n Please select the Dealer.";
            //        bValidateRecord = false;
            //    }
            //}
            //if (iMenuId == 665 && bConfirm == true)
            //{
            //    int CNT_D = 0;
            //    string DealerRemarks = "";
            //    for (int iRowCnt = 0; iRowCnt < DealerFeedback.Rows.Count; iRowCnt++)
            //    {

            //        DealerRemarks = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text);
            //        //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
            //        if (DealerRemarks != "")
            //        {
            //            CNT_D = CNT_D + 1;
            //        }

            //    }
            //    if (CNT_D <= 0)
            //    {
            //        sMessage = sMessage + "\\n Dealer Feedback is mandatory.";
            //        bValidateRecord = false;
            //    }
            //}

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + "');</script>");
            }
            return bValidateRecord;
        }
        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                string cntPartID = "";
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Ticket_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Ticket_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Call_Type_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Call_Sub_Type_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Call_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Chassis_ID", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CRM_Cust_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Global_Cust_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("IS_Acknowledge", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("IS_Dealer", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DealerRemark", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AlternateContact_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DDM_District_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Vehicle_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Call_Cancel", typeof(string)));
                

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Ticket_No"] = txtTicketNo.Text;
                dr["Ticket_Date"] = txtTicketDate.Text;
                if (iMenuId == 670)
                {
                    dr["Dealer_ID"] = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                }
                if (iMenuId == 665)
                {
                    dr["Dealer_ID"] = Session["iDealerID"].ToString();
                }


                dr["Call_Type_ID"] = Func.Convert.iConvertToInt(drpCallType.SelectedValue);
                dr["Call_Sub_Type_ID"] = Func.Convert.iConvertToInt(drpCallSubType.SelectedValue);
                dr["Call_Confirm"] = "N";

                dr["Chassis_ID"] = txtChassisID.Text;
                dr["CRM_Cust_ID"] = Func.Convert.iConvertToInt(txtCRMCustID.Text);
                dr["Global_Cust_ID"] = Func.Convert.iConvertToInt(txtGlobalCustID.Text);
                dr["IS_Acknowledge"] = "N";
                if (iMenuId == 670)
                {
                    dr["IS_Dealer"] = "N";
                }
                if (iMenuId == 665)
                {
                    dr["IS_Dealer"] = "Y";
                }
                dr["UserId"] = iUserId;
                dr["DealerRemark"] = txtDetailsRemark.Text;
                dr["AlternateContact_No"] = txtAlternateContactNo.Text;
                dr["DDM_District_ID"] = Func.Convert.iConvertToInt(drpDistrict.SelectedValue);
                dr["Vehicle_No"] = txtVehicleNo.Text;
                dr["Call_Cancel"] = "N";
               

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
             clsCRM objCRM = new clsCRM();
            try
            {
                if (bValidateRecord(bSaveWithConfirm) == false)
                {
                    return false;
                }

                UpdateHdrValueFromControl(dtHdr);
                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["Call_Confirm"] = "Y";

                    if (iMenuId == 665)
                    {
                        dtHdr.Rows[0]["IS_Acknowledge"] = "Y";
                    }


                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Call_Cancel"] = "Y";
                }

                if (iMenuId == 670)
                {
                    DealerCode = "D009999";

                    iDealerID = 9999;
                }
                if (iMenuId == 665)
                {
                    DealerCode = Location.sDealerCode;
                }

                bFillDealerFeedbackFromGrid();
                bFillMTIFeedbackFromGrid();
                if (objCRM.bSaveRecordCRM(dtHdr, ref iCRMID, DealerCode) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iCRMID);

                    if (objCRM.bSaveDealerFeedBack(dtDealerFeedback, Func.Convert.iConvertToInt(txtID.Text)) == true)
                    {
                        if (objCRM.bSaveMTIFeedBack(dtMTIFeedback, Func.Convert.iConvertToInt(txtID.Text)) == true)
                        {
                            if (bSaveWithConfirm == true)
                            {
                               // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                                return true;
                            }
                            else if (bSaveWithCancel == true)
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                                return true;
                            }


                            else
                            {
                               // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                                return true;
                            }
                        }
                    }
                    return true;
                }

                else
                {
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
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
                    Session["Temp_call"] = 0;
                    txtID.Text = "";
                    FillCallType_New();
                    GenerateTicketNo();
                    DisplayCurrentRecord("New");
                    PSelectionGrid.Style.Add("display", "none");
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    string Temp_call = Session["Temp_call"].ToString();
                    iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                    if (Temp_call == "0" || iCRMID != 0)
                    {
                        Session["Temp_call"] = 1;
                        if (bSaveRecord(false, false) == false) return;
                        PSelectionGrid.Style.Add("display", "");
                        FillSelectionGrid();
                        iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                        GetDataAndDisplay();

                   }

                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bSaveRecord(true, false) == false) return;
                    //if (txtID.Text == "")
                    //{
                    //    FillSelectionGrid();
                    //    DisplayCurrentRecord("Max");
                    //    PSelectionGrid.Style.Add("display", "");
                    //    return;
                    //}
                    PSelectionGrid.Style.Add("display", "");
                    FillSelectionGrid();
                    iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                    GetDataAndDisplay();
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    //if (bSaveRecord(false, true) == false) return;
                    //PSelectionGrid.Style.Add("display", "");

                }
               

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void drpCallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTicketNo();
         
        }

        protected void lblSelectChassis_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayData1();
                ModalPopupExtender2.Show();

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void lblSelectCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                 DisplayData();
                ModalPopUpExtender1.Show();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void bFillDealerFeedbackFromGrid()
        {

            //bDetailsRecordExist = false;

            dtDealerFeedback = (DataTable)Session["DealerFeedback"];

            //int iCntForDelete = 0;
            //int iCntForSelect = 0;

            int FeedBackID = 0;
            string DealerFeedBack = "";
            string Remarks = "";
            for (int iRowCnt = 0; iRowCnt < DealerFeedback.Rows.Count; iRowCnt++)
            {

                FeedBackID = Func.Convert.iConvertToInt((DealerFeedback.Rows[iRowCnt].FindControl("txtDealerID") as TextBox).Text);
                dtDealerFeedback.Rows[iRowCnt]["ID"] = FeedBackID;

                DealerFeedBack = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("lblDealerFeedBack") as Label).Text);
                dtDealerFeedback.Rows[iRowCnt]["DealerFeedBack"] = DealerFeedBack;

                Remarks = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text);
                dtDealerFeedback.Rows[iRowCnt]["Remark"] = Remarks;




            }



        }
        private void bFillMTIFeedbackFromGrid()
        {

            //bDetailsRecordExist = false;

            dtMTIFeedback = (DataTable)Session["MTIFeedback"];

            //int iCntForDelete = 0;
            //int iCntForSelect = 0;

            int MTIFeedBackID = 0;
            string MTIFeedBack = "";
            string MTIFeedBackStatus1 = "0";
            string MTIFeedBackStatus2 = "0";
            string MTIFeedBackStatus3 = "0";
            string MTIFeedBackScale = "0";
            string MTIRemarks = "";

            for (int iRowCnt = 0; iRowCnt < MTIFeedback.Rows.Count; iRowCnt++)
            {

                MTIFeedBackID = Func.Convert.iConvertToInt((MTIFeedback.Rows[iRowCnt].FindControl("txtMTIID") as TextBox).Text);
                dtMTIFeedback.Rows[iRowCnt]["ID"] = MTIFeedBackID;

                MTIFeedBack = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("lblMTIFeedBack") as Label).Text);
                dtMTIFeedback.Rows[iRowCnt]["MTIFeedBack"] = MTIFeedBack;

                MTIFeedBackStatus1 = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus1") as DropDownList).SelectedItem.Value);
                dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus1"] = MTIFeedBackStatus1;

                MTIFeedBackStatus2 = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus2") as DropDownList).SelectedItem.Value);
                dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus2"] = MTIFeedBackStatus2;

                MTIFeedBackStatus3 = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("drpFeedBackStatus3") as DropDownList).SelectedItem.Value);
                dtMTIFeedback.Rows[iRowCnt]["FeedBackStatus3"] = MTIFeedBackStatus3;

                MTIFeedBackScale = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("drpScale") as DropDownList).SelectedItem.Value);
                dtMTIFeedback.Rows[iRowCnt]["Scale"] = MTIFeedBackScale;

                MTIRemarks = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("txtMTIRemarks") as TextBox).Text);
                MTIRemarks = txtcallcenterRemark.Text; 
                dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;


            }



        }

        protected void drpDealerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet dsDealerNameD;
            // Created By Vikram on 23.06.16
            dsDealerNameD = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetDealerDetails", Convert.ToInt16(drpDealerName.SelectedValue));
            if (dsDealerNameD != null)
            {


                txtDealerCode.Text = Func.Convert.sConvertToString(dsDealerNameD.Tables[0].Rows[0]["Dealer Code"]);
                txtDCity.Text = Func.Convert.sConvertToString(dsDealerNameD.Tables[0].Rows[0]["Dealer_City"]);
                txtDRegion.Text = Func.Convert.sConvertToString(dsDealerNameD.Tables[0].Rows[0]["Region_Name"]);

            }
        }
        protected void btnCalClose_Click(object sender, EventArgs e)
        {
            try
            {
                iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                objCRM = new clsCRM();
                int CNT = 0;
                int CNT1 = 0;
                string MTIRemarks = "";
                string MTIScale = "";
                for (int iRowCnt = 0; iRowCnt < MTIFeedback.Rows.Count; iRowCnt++)
                {

                    MTIRemarks = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("txtMTIRemarks") as TextBox).Text);

                    MTIRemarks = txtcallcenterRemark.Text.Trim(); 
                    //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
                    
                    if (MTIRemarks != "")
                    {
                        CNT = CNT + 1;
                    }
                    MTIScale = Func.Convert.sConvertToString((MTIFeedback.Rows[iRowCnt].FindControl("drpScale") as DropDownList).SelectedItem.Value);
                    //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
                    if (MTIScale != "0")
                    {
                        CNT1 = CNT1 + 1;
                    }

                }
                if (CNT > 0 && CNT1 > 0)
                {
                    if (CNT == Func.Convert.iConvertToInt(MTIFeedback.Rows.Count) && CNT1 == Func.Convert.iConvertToInt(MTIFeedback.Rows.Count))
                    {
                        bFillMTIFeedbackFromGrid();
                        if (objCRM.bSaveMTIFeedBack(dtMTIFeedback, Func.Convert.iConvertToInt(txtID.Text)) == true)
                        {
                            if (objCRM.bSAVE_IsCalClose(iCRMID,Func.Convert.sConvertToString(txtcallcenterRemark.Text)) == true)
                            {

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(1)", true);
                                btnCalClose.Style.Add("display", "none");
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(5)", true);
                        btnCalClose.Style.Add("display", "");
                    }
                 }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(2)", true);
                    btnCalClose.Style.Add("display", "");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
            }

        }
        protected void btnAcknowledge_Click(object sender, EventArgs e)
        {
            try
            {
                iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                objCRM = new clsCRM();
                int CNT_D = 0;
                string DealerRemarks = "";
                //for (int iRowCnt = 0; iRowCnt < DealerFeedback.Rows.Count; iRowCnt++)
                //{

                //    DealerRemarks = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text);
                //    //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
                //    if (DealerRemarks != "")
                //    {
                //        CNT_D = CNT_D + 1;
                //    }

                //}
                //if (CNT_D > 0)
                //{
                if (objCRM.bSaveAcknowledge(iCRMID, txtDetailsRemark.Text) == true)
                    {


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " Acknowledge()", true);
                        btnAcknowledge.Style.Add("display", "none");
                    }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(3)", true);
                //    btnAcknowledge.Style.Add("display", "");
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
            }
        }

        protected void btnDealerCallClosure_Click(object sender, EventArgs e)
        {
            try
            {
                iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                objCRM = new clsCRM();
                int CNT_D = 0;
                string DealerRemarks = "";
                for (int iRowCnt = 0; iRowCnt < DealerFeedback.Rows.Count; iRowCnt++)
                {

                    DealerRemarks = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text);
                    //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
                    if (DealerRemarks != "")
                    {
                        CNT_D = CNT_D + 1;
                    }

                }
                if (CNT_D > 0)
                {
                    if (CNT_D == Func.Convert.iConvertToInt(DealerFeedback.Rows.Count))
                    {
                        bFillDealerFeedbackFromGrid();
                        if (objCRM.bSaveDealerFeedBack(dtDealerFeedback, Func.Convert.iConvertToInt(txtID.Text)) == true)
                        {
                            if (objCRM.bSaveDealerCallClosure(iCRMID) == true)
                            {


                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(4)", true);
                                btnDealerCallClosure.Style.Add("display", "none");
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(5)", true);
                        btnDealerCallClosure.Style.Add("display", "");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(3)", true);
                    btnDealerCallClosure.Style.Add("display", "");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
            }
        }



        private void DisplayData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;

                sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                sSelText = txtSearch.Text.ToString();
                //  iJobtype = Func.Convert.iConvertToInt(Request.QueryString["JobTypeID"].ToString());
                // iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                //  iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());



                //dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_CRM_CustomerSelection", 0);
                // ViewState["Chassis"] = dtSrchgrid;
                //Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                if (DdlSelctionCriteria.SelectedValue == "Customer_name" && txtSearch.Text != "")
                    dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                CustomerGrid.DataSource = dvDetails;
                //  ChassisGrid.DataSource = dtSrchgrid;
                CustomerGrid.DataBind();

                if (CustomerGrid.Rows.Count == 0) return;



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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" && btnSearch.Text == "Search")
                btnSearch.Text = "ClearSearch";
            else if (txtSearch.Text != "" && btnSearch.Text == "ClearSearch")
            {
                txtSearch.Text = "";
                btnSearch.Text = "Search";
            }
            DisplayData();
            ModalPopUpExtender1.Show();
        }

        protected void CustomerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CustomerGrid.PageIndex = e.NewPageIndex;
            DisplayData();
            ModalPopUpExtender1.Show();
        }

        /***ChassisGrid Modelpop code ***/


        private void DisplayData1()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;

                sSelType = DropDownList1.SelectedValue.ToString();
                sSelText = TextBox1.Text.ToString();
          //      CustID = Func.Convert.iConvertToInt(Request.QueryString["CustID"].ToString());

                //  iJobtype = Func.Convert.iConvertToInt(Request.QueryString["JobTypeID"].ToString());
                // iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                //  iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                CustID =Convert.ToInt32(txtGlobalCustID.Text);


                //dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_CRM_ChassisSelection", CustID);
                // ViewState["Chassis"] = dtSrchgrid;
                // Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                if (DropDownList1.SelectedValue == "Chassis_no" && TextBox1.Text != "")
                    dvDetails.RowFilter = (DropDownList1.SelectedValue + " LIKE '*" + TextBox1.Text + "*'");
                if (DropDownList1.SelectedValue == "Reg_no" && txtSearch.Text != "")
                    dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + TextBox1.Text + "*'");
                if (DropDownList1.SelectedValue == "Customer_name" && TextBox1.Text != "")
                    dvDetails.RowFilter = (DropDownList1.SelectedValue + " LIKE '*" + TextBox1.Text + "*'");
                ChassisGrid.DataSource = dvDetails;
                //  ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

                if (ChassisGrid.Rows.Count == 0) return;



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
        private void ExpirePageCache1()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            if (TextBox1.Text != "" && Button1.Text == "Search")
                Button1.Text = "ClearSearch";
            else if (TextBox1.Text != "" && Button1.Text == "ClearSearch")
            {
                TextBox1.Text = "";
                Button1.Text = "Search";
            }
            DisplayData1();
            ModalPopupExtender2.Show();
        }

        protected void ChassisGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ChassisGrid.PageIndex = e.NewPageIndex;
            DisplayData1();
            ModalPopupExtender2.Show();
        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filldistrict();
        }
        private void Filldistrict()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDistrict;

                dsDistrict = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "7", Func.Convert.iConvertToInt(drpState.SelectedItem.Value));
                if (dsDistrict != null)
                {
                    drpDistrict.DataSource = dsDistrict.Tables[0];
                    drpDistrict.DataTextField = "Name";
                    drpDistrict.DataValueField = "ID";
                    drpDistrict.DataBind();
                    drpDistrict.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillDealer()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDealerName;

                dsDealerName = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "3", Func.Convert.iConvertToInt(drpDistrict.SelectedItem.Value));
                if (dsDealerName != null)
                {
                    drpDealerName.DataSource = dsDealerName.Tables[0];
                    drpDealerName.DataTextField = "Name";
                    drpDealerName.DataValueField = "ID";
                    drpDealerName.DataBind();
                    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
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

        protected void drpDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealer();
        }

        protected void drpFeedBackStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpFeedBackStatus1 = (DropDownList)gvr.FindControl("drpFeedBackStatus1");
            DropDownList drpFeedBackStatus2 = (DropDownList)gvr.FindControl("drpFeedBackStatus2");
            DropDownList drpFeedBackStatus3 = (DropDownList)gvr.FindControl("drpFeedBackStatus3");
            DropDownList drpScale = (DropDownList)gvr.FindControl("drpScale");
            if (drpFeedBackStatus1.SelectedValue == "5" || drpFeedBackStatus1.SelectedValue == "6")
            {
                drpFeedBackStatus2.Enabled = false;
                drpFeedBackStatus3.Enabled = false;
                drpScale.Enabled = true;
            }
            else
            {
                drpFeedBackStatus2.Enabled = true;
                drpFeedBackStatus3.Enabled = false;
                drpScale.Enabled = true;
            }

        }
        protected void drpFeedBackStatus2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpFeedBackStatus1 = (DropDownList)gvr.FindControl("drpFeedBackStatus1");
            DropDownList drpFeedBackStatus2 = (DropDownList)gvr.FindControl("drpFeedBackStatus2");
            DropDownList drpFeedBackStatus3 = (DropDownList)gvr.FindControl("drpFeedBackStatus3");
            DropDownList drpScale = (DropDownList)gvr.FindControl("drpScale");
            if (drpFeedBackStatus2.SelectedValue == "5" || drpFeedBackStatus2.SelectedValue == "6")
            {
                drpFeedBackStatus1.Enabled = false;
                drpFeedBackStatus3.Enabled = false;
                drpScale.Enabled = true;
            }
            else
            {
                drpFeedBackStatus3.Enabled = true;
                drpFeedBackStatus1.Enabled = false;
                drpScale.Enabled = true;
            }

        }
        protected void drpFeedBackStatus3_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpFeedBackStatus1 = (DropDownList)gvr.FindControl("drpFeedBackStatus1");
            DropDownList drpFeedBackStatus2 = (DropDownList)gvr.FindControl("drpFeedBackStatus2");
            DropDownList drpFeedBackStatus3 = (DropDownList)gvr.FindControl("drpFeedBackStatus3");
            DropDownList drpScale = (DropDownList)gvr.FindControl("drpScale");
            if (drpFeedBackStatus3.SelectedValue == "5" || drpFeedBackStatus3.SelectedValue == "6")
            {
                drpFeedBackStatus1.Enabled = false;
                drpFeedBackStatus2.Enabled = false;
                drpScale.Enabled = true;
            }
            else
            {
                drpFeedBackStatus1.Enabled = false;
                drpFeedBackStatus1.Enabled = false;
                drpScale.Enabled = true;
            }

        }

        protected void btnFeedBackSave_Click(object sender, EventArgs e)
        {
            objCRM = new clsCRM();
             bFillMTIFeedbackFromGrid();
             if (objCRM.bSaveMTIFeedBack(dtMTIFeedback, Func.Convert.iConvertToInt(txtID.Text)) == true)
             {
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(6)", true);
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(7)", true);
             }
        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                iCRMID = Func.Convert.iConvertToInt(txtID.Text);
                objCRM = new clsCRM();

                if (objCRM.bSaveOpen(iCRMID) == true)
                {


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(8)", true);
                    btnAcknowledge.Style.Add("display", "none");
                }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " CalClose(3)", true);
                //    btnAcknowledge.Style.Add("display", "");
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
            }
        }

    

    }
}