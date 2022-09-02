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
     public partial class frmCRMCreation_Sales : System.Web.UI.Page
    {
         private int iPrimaryApplicationID;
        private int iCRMID = 0;
        private int iID;
        int iUserId = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        int iMenuId = 0;
        string DealerCode;
        string ISDealer = "";
        private DataSet dsState;
        private DataSet dsSecondaryApplication;
        clsCRM objCRM = null;
        private DataTable dtDealerFeedback = new DataTable();
        private DataTable dtMTIFeedback = new DataTable();
        private bool bDetailsRecordExist = false;
        private string sSelText, sSelType;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                Location.bUseSpareDealerCode = false;
                Location.SetControlValue();
                clsCustomer ObjCust = new clsCustomer();


            
                DataSet ds = new DataSet();
                DataSet dsDealer = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                //ToolbarC.iValidationIdForSave = 66;
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                //For MD User
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                //For MD User
                if (iMenuId == 673) //For call center user
                {
                    Location.Visible =false;
                    PDealerFeedback.Visible = false;
                    PMTIFeedback.Visible = false;
                    Panel7.Visible = false;
                    callcenterRemark.Visible = true;
                }
                if (iMenuId == 672) // For Dealer
                {
                    Panel7.Visible = false;
                    PMTIFeedback.Visible = false;
                    callcenterRemark.Visible = false;
                }
                //if (iUserId == 5046 && iMenuId == 673)  // dealer assign to geeta id
               if (iUserId == 21 && iMenuId == 673)  // dealer assign to geeta id
                {
                    Panel7.Visible = true; //dealer assign tab visible for geeta

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }

                

                if (!IsPostBack)
                {
                    FillStateCountry();
                    FillCountry();
                    FillCustType();
                    FillSubCallType();
                    FillDealer(); 
                    FillCombo();
                    FillTitle();
                    FillCombo_SecondaryApplication();
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                  
                }




                //if (iCRMID != 0)
                //{
                //    GetDataAndDisplay();
                //}
                SearchGrid.sGridPanelTitle = "Call List";
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
        protected void drpCustType_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillTitle();

        }
        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCountry();

        }

        protected void drpPrimaryApplication_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillCombo_SecondaryApplication();
        }
        private void FillStateCountry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                drpState.Items.Clear();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                if (dsState != null)
                {
                    drpState.DataSource = dsState.Tables[0];
                    drpState.DataTextField = "Name";
                    drpState.DataValueField = "ID";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new ListItem("--Select--", "0"));
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

        private void FillCombo_SecondaryApplication()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
               
                drpSeconadryApplication.Items.Clear();
                dsSecondaryApplication = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetSecondaryAppln", Convert.ToInt16(drpPrimaryApplication.SelectedValue));
                if (dsSecondaryApplication != null)
                {
                    drpSeconadryApplication.DataSource = dsSecondaryApplication.Tables[0];
                    drpSeconadryApplication.DataTextField = "Name";
                    drpSeconadryApplication.DataValueField = "ID";
                    drpSeconadryApplication.DataBind();
                    drpSeconadryApplication.Items.Insert(0, new ListItem("--Select--", "0"));
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
        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filldistrict();
            FillRegion();
           
            FillCountry();
        }
        private void FillCustType()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType;

                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_CRM_GetCustType", 0);
                if (dsCustType != null)
                {
                    drpCustType.DataSource = dsCustType.Tables[0];
                    drpCustType.DataTextField = "Name";
                    drpCustType.DataValueField = "ID";
                    drpCustType.DataBind();
                    drpCustType.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillRegion()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsRegion;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsRegion = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpState.SelectedValue, "RegionCRM");
                if (dsRegion != null)
                {
                    drpRegion.DataSource = dsRegion.Tables[0];
                    drpRegion.DataTextField = "Name";
                    drpRegion.DataValueField = "ID";
                    drpRegion.DataBind();
                    if (Func.Convert.iConvertToInt(drpState.SelectedValue) == 0)
                    {
                        drpRegion.Items.Insert(0, new ListItem("--Select--", "0"));
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

        private void FillCountry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCountry;


                dsCountry = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpRegion.SelectedValue, "CountryCRM");
                if (dsCountry != null)
                {
                    drpCountry.DataSource = dsCountry.Tables[0];
                    drpCountry.DataTextField = "Name";
                    drpCountry.DataValueField = "ID";
                    drpCountry.DataBind();
                    if (Func.Convert.iConvertToInt(drpRegion.SelectedValue) == 0)
                    {
                        drpCountry.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void Filldistrict()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDistrict;

               
                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsDistrict = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpState.SelectedValue, "DistrictCRM");
                if (dsDistrict != null)
                {
                    drpDistrict.DataSource = dsDistrict.Tables[0];
                    drpDistrict.DataTextField = "Name";
                    drpDistrict.DataValueField = "ID";
                    drpDistrict.DataBind();
                   // if (Func.Convert.iConvertToInt(drpState.SelectedValue) == 0)
                    //{
                        drpDistrict.Items.Insert(0, new ListItem("--Select--", "0"));
                   // }
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

        private void SetDocumentDetails()
        {

        // lblSelectCustomer.Attributes.Add("onclick", " return ShowCustomerMaster_CRM(this)");
        }
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Ticket No");
                //SearchGrid.AddToSearchCombo("Ticket Date");
                SearchGrid.AddToSearchCombo("Record Status");
                //SearchGrid.AddToSearchCombo("PO Type");
                 
                 if (iMenuId == 673)
                 {
                     //SearchGrid.iDealerID = 9999;
                     //SearchGrid.sModelPart = "N";
                     SearchGrid.iDealerID = iUserId;
                     SearchGrid.sModelPart = "N";
                 }
                 if (iMenuId == 672)
                 {
                     SearchGrid.iDealerID = iDealerID;
                     SearchGrid.sModelPart = "Y";
                 }
                // if (iUserId == 7043)  // Amrit call center login
                 //if (iUserId == 24)  // Amrit call center login
                 if (iUserId == 24 || iUserId == 308 || iUserId == 318 || iUserId == 319 || iUserId == 320 || iUserId == 321 || iUserId == 728)
                 {
                     SearchGrid.sSqlFor = "CRMSales_ALL";
                 }
                 else
                 {
                     SearchGrid.sSqlFor = "CRMSales";
                 }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FillSelectionGrid();
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
                if (iMenuId == 673)
                {
                    ISDealer = "N";
                }
                if (iMenuId == 672)
                {
                    ISDealer = "Y";

                }

                ds = objCRM.GetCRM_Sales(iCRMID, Type, ISDealer);

                    //sNew = "N";
                    DisplayData(ds);
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
               // if (iUserId == 5046 && iMenuId == 673)
                    if (iUserId == 21 && iMenuId == 673) //geeta login id
                {
                    ISDealer = "N";
                }
                else if (iMenuId == 673)
                {
                    ISDealer = "N";
                }
                else if (iMenuId == 672)
                {
                    ISDealer = "Y";

                }
               
                if (iCRMID != 0)
                {
                    ds = objCRM.GetCRM_Sales(iCRMID, "Max", ISDealer);
                    
                    //sNew = "N";
                    DisplayData(ds);
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
        private void FillTitle()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsTitle;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsTitle = objDB.ExecuteStoredProcedureAndGetDataset("sp_CRM_GetNameTitle", drpCustType.SelectedValue);
                if (dsTitle != null)
                {
                    drpTitle.DataSource = dsTitle.Tables[0];
                    drpTitle.DataTextField = "Name";
                    drpTitle.DataValueField = "ID";
                    drpTitle.DataBind();
                    drpTitle.Items.Insert(0, new ListItem("--Select--", "0"));

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
        private void DisplayData(DataSet ds)
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
                FillCallType();
                FillCombo();
                drpCallType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]);
                FillSubCallType();
                drpCallSubType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Sub_Type_ID"]);
                FillCustType();
                drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_Type"]);
                FillTitle();
                drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prefix"]);
                txtCRMCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_Cust_ID"]);
                txtGlobalCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Global_Cust_ID"]);
               txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);
               string IsMTICust = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Existing_MTI_Cust"]);
               if (IsMTICust == "Y")
               {
                   drpIsMTICust.SelectedValue = "1";
               }
               else if (IsMTICust == "N")
               {
                   drpIsMTICust.SelectedValue = "2";
               }
               else
               {
                   drpIsMTICust.SelectedValue = "0";
               }
               txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add1"]);
               txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add2"]);
               txtPinCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
               FillStateCountry();
               drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["State_id"]);
               FillRegion();
               drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_ID"]);

               Filldistrict();
               drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["District_ID"]);

               
              
               txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
               FillCountry();
               drpCountry.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Country_Id"]);
               txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone"]);
               txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
               drpPrimaryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PriApp"]);
               iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
              // Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
               FillCombo_SecondaryApplication();
               drpSeconadryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SecApp"]);

               drpStateDealer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DstateID"]);
               FilldistrictDealer();
               drpDistrictDealer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DDistrictID"]); 
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
                   lblSelectCustomer.Style.Add("display", "none");
                   ToolbarC.sSetMessage(Toolbar.enmToolbarType.enmNothing);
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, false);
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
               }
               else
               {
                   btnFeedBackSave.Style.Add("display", "none");
                   lblSelectCustomer.Style.Add("display", "");
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, true);
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, true);
                   //ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, true);
                   ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
               }
               // if (iUserId == 5046) //geeta login id
                 if (iUserId == 21) //geeta login id
                {
                    btnFeedBackSave.Style.Add("display", "none");
                    lblSelectCustomer.Style.Add("display", "none");
                    drpCustType.Enabled = false;
                    drpTitle.Enabled = false;
                    txtCustName.Enabled = false;
                    drpIsMTICust.Enabled = false;
                    txtAddress1.Enabled = false;
                    txtAddress2.Enabled = false;
                    txtPinCode.Enabled = false;
                    drpState.Enabled = false;
                    drpDistrict.Enabled = false;
                    drpRegion.Enabled = false;
                    txtCity.Enabled = false;
                    drpCountry.Enabled = false;
                    txtMobile.Enabled = false;
                    txtEmail.Enabled = false;
                    drpPrimaryApplication.Enabled = false;
                    drpSeconadryApplication.Enabled = false;

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                }
                 // After confimation  call ticket callcelled 
                 if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_confirm"]) == "" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Cancel"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]) != "4")
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
               if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && ISDealer == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"])=="4") && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "N")
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
             //  if (iUserId == 24)  // Amrit call center login
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
                   if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Dealer"]) == "N")
                   {
                       Panel7.Visible = true;
                       
                   }
                   //if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "N")
                   //{
                   if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Confirm"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Acknowledge"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Close"]) == "N" && (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Call_Type_ID"]) == "4") && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_DealerCallClosure"]) == "Y")
                   {
                       Panel7.Visible = true;
                       PDealerFeedback.Visible = true;
                       PMTIFeedback.Visible = true;
                       Calclose.Style.Add("display", "");
                       btnFeedBackSave.Style.Add("display", "");
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
                       btnFeedBackSave.Style.Add("display", "none");
                       ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                       ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                       ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                       ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                       ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                   }
                   if (txtUserType.Text == "6")
                   {
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


         



            //SetGridControlPropertyQuot(false);



        }
        private void SetGridControlPropertyQuot(bool bRecordIsOpen)
        {
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





            //SetGridControlPropertyQuot(false);



        }
        private void FillDealer()
        {
            clsDB objDB = new clsDB();
            try
            {

                DataSet dsDealerName;

                dsDealerName = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "3", Func.Convert.iConvertToInt(drpDistrictDealer.SelectedItem.Value));
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
        private void FillSubCallType()
        {
            clsDB objDB = new clsDB();
            try
            {
                
                DataSet dsSubCallType;
                // Created By Vikram on 23.06.16
                dsSubCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "5", Func.Convert.iConvertToInt(drpCallType.SelectedItem.Value));
                if (dsSubCallType != null)
                {
                    drpCallSubType.DataSource = dsSubCallType.Tables[0];
                    drpCallSubType.DataTextField = "Name";
                    drpCallSubType.DataValueField = "ID";
                    drpCallSubType.DataBind();
                    drpCallSubType.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillCallType()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCallType;
                // Created By Vikram on 23.06.16
                dsCallType = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "4",0);
                if (dsCallType != null)
                {
                    drpCallType.DataSource = dsCallType.Tables[0];
                    drpCallType.DataTextField = "Name";
                    drpCallType.DataValueField = "ID";
                    drpCallType.DataBind();
                    drpCallType.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DataSet dsStateDealer;
                //dsStateDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", 0);
                //dsStateDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", iUserId);
                //if (dsStateDealer != null)
                //{
                //    drpStateDealer.DataSource = dsStateDealer.Tables[0];
                //    drpStateDealer.DataTextField = "Name";
                //    drpStateDealer.DataValueField = "ID";
                //    drpStateDealer.DataBind();
                //    drpStateDealer.Items.Insert(0, new ListItem("--Select--", "0"));
                //}


                if (iMenuId == 673) //MTI login Menu id 
                {

                    dsStateDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "6", iUserId);
                    if (dsStateDealer != null)
                    {
                        drpStateDealer.DataSource = dsStateDealer.Tables[0];
                        drpStateDealer.DataTextField = "Name";
                        drpStateDealer.DataValueField = "ID";
                        drpStateDealer.DataBind();
                        drpStateDealer.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }
                if (iMenuId == 672) //Dealer login Menu id 
                {
                    dsStateDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "8", iDealerID);
                    if (dsStateDealer != null)
                    {
                        drpStateDealer.DataSource = dsStateDealer.Tables[0];
                        drpStateDealer.DataTextField = "Name";
                        drpStateDealer.DataValueField = "ID";
                        drpStateDealer.DataBind();
                        drpStateDealer.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }






                FilldistrictDealer();
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
        private void FillCombo()
        {
           
            Func.Common.BindDataToCombo(drpPrimaryApplication, clsCommon.ComboQueryType.PrimaryApplication, 0);
            iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);

            Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
           
         
        }
        private void GenerateTicketNo()
        {
            clsDB objDB = new clsDB();
            try
            {
                if (iMenuId == 673)
                {
                    DealerCode = "D009999";
                  
                    iDealerID = 9999;
                }
                if (iMenuId == 672)
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

            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (txtTicketDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Ticket date.";
                bValidateRecord = false;
            }
            else if (drpCustType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Customer Type.";
                bValidateRecord = false;
            }
            else if (txtCustName.Text.Trim()=="")
            {
                sMessage = sMessage + "\\n Please Enter the Customer Name.";
                bValidateRecord = false;
            }
            else if (drpIsMTICust.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Existing MTI Customer.";
                bValidateRecord = false;
            }
            else if (drpTitle.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Title.";
                bValidateRecord = false;
            }
            else if (txtAddress1.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter the Address1.";
                bValidateRecord = false;
            }
            //else if (txtAddress2.Text.Trim() == "") Email and address2 is not mandatory. 
            //{
            //    sMessage = sMessage + "\\n Please Enter the Address2.";
            //    bValidateRecord = false;
            //}
            else if (txtPinCode.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter the Pin Code.";
                bValidateRecord = false;
            }
            else if (drpState.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the State.";
                bValidateRecord = false;
            }
            else if (drpDistrict.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the District.";
                bValidateRecord = false;
            }
            else if (drpRegion.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Region.";
                bValidateRecord = false;
            }
            else if (txtCity.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter the City.";
                bValidateRecord = false;
            }
            else if (drpCountry.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Country.";
                bValidateRecord = false;
            }
                
          else if (drpCallType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Call Type.";
                bValidateRecord = false;
            }
            else if (drpCallSubType.SelectedValue == "0" && drpCallType.SelectedValue == "1")
            {
                sMessage = sMessage + "\\n Please select the Sub Call Type.";
                bValidateRecord = false;
            }
            else if (txtMobile.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter the Mobile.";
                bValidateRecord = false;
            }
            //else if (txtEmail.Text.Trim() == "") Email and address2 is not mandatory. 
            //{
            //    sMessage = sMessage + "\\n Please Enter the Email.";
            //    bValidateRecord = false;
            //}
            else if (drpPrimaryApplication.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Primary Application.";
                bValidateRecord = false;
            }
            else if (drpSeconadryApplication.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Seconadry Application.";
                bValidateRecord = false;
            }
           
                if (bConfirm == true)
                {
                    if (txtAlternateContactNo.Text.Trim() == "")
                    {
                        sMessage = sMessage + "\\n Please Enter the Alternate Contact No.";
                        bValidateRecord = false;
                    }

                }
            
           // if (iUserId == 21 || iUserId == 7043)  // dealer assign to geeta id

                if ((iUserId == 21 || iUserId == 24 || iUserId == 308 || iUserId == 318 || iUserId == 319 || iUserId == 320 || iUserId == 321 || iUserId == 728) && bConfirm == true)  // dealer assign to geeta id
                {
                    if (drpDealerName.SelectedValue == "0")
                    {
                        sMessage = sMessage + "\\n Please select the Dealer.";
                        bValidateRecord = false;
                    }
                }

            //if (iMenuId == 672 && bConfirm==true)
            //{
            //     int CNT_D = 0;
            //    string DealerRemarks = "";
            //    for (int iRowCnt = 0; iRowCnt < DealerFeedback.Rows.Count; iRowCnt++)
            //    {

            //        DealerRemarks = Func.Convert.sConvertToString((DealerFeedback.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text);
            //     //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
            //        if (DealerRemarks !="" )
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
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
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
                if (iMenuId == 673)
                {
                    dr["Dealer_ID"] = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                }
                if (iMenuId == 672)
                {
                    dr["Dealer_ID"] = Session["iDealerID"].ToString();
                }
                

                dr["Call_Type_ID"] = Func.Convert.iConvertToInt(drpCallType.SelectedValue);
                dr["Call_Sub_Type_ID"] = Func.Convert.iConvertToInt(drpCallSubType.SelectedValue);
                dr["Call_Confirm"] = "N";
               
                dr["Chassis_ID"] = 0;
                dr["CRM_Cust_ID"] = txtCRMCustID.Text;
                dr["Global_Cust_ID"] = txtGlobalCustID.Text;
                dr["IS_Acknowledge"] = "N";
                if (iMenuId == 673)
                {
                    dr["IS_Dealer"] = "N";
                }
                if (iMenuId == 672)
                {
                    dr["IS_Dealer"] = "Y";
                }
                dr["UserId"] = iUserId;
                dr["DealerRemark"] = txtDetailsRemark.Text;
                dr["AlternateContact_No"] = txtAlternateContactNo.Text;
                dr["DDM_District_ID"] = Func.Convert.iConvertToInt(drpDistrictDealer.SelectedValue);
                dr["Vehicle_No"] = "";
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
            int CRM_newCustID = 0;
            try
            {
                //if (bValidateRecord(bSaveWithConfirm) == false)
                //{
                //    return false;
                //}

               

                CRM_newCustID = objCRM.bSaveCRMCustomerMaster(Func.Convert.iConvertToInt(txtCRMCustID.Text), Func.Convert.iConvertToInt(txtGlobalCustID.Text),
                    Func.Convert.iConvertToInt(drpCustType.SelectedValue), Func.Convert.iConvertToInt(drpTitle.SelectedValue),drpIsMTICust.SelectedItem.Text, txtCustName.Text,
                       txtAddress1.Text, txtAddress2.Text, txtPinCode.Text, Func.Convert.iConvertToInt(drpState.SelectedValue),
                       Func.Convert.iConvertToInt(drpDistrict.SelectedValue), Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                       txtCity.Text, Func.Convert.iConvertToInt(drpCountry.SelectedValue),txtMobile.Text,txtEmail.Text,
                       Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue)
            , Func.Convert.iConvertToInt(drpSeconadryApplication.SelectedValue));


                if (CRM_newCustID > 0)
                {
                    txtCRMCustID.Text = Func.Convert.sConvertToString(CRM_newCustID);
                    UpdateHdrValueFromControl(dtHdr);
                    if (bSaveWithConfirm == true)
                    {
                        dtHdr.Rows[0]["Call_Confirm"] = "Y";

                        if (iMenuId == 672)
                        {
                            dtHdr.Rows[0]["IS_Acknowledge"] = "Y";
                        }


                    }
                    if (bSaveWithCancel == true)
                    {
                        dtHdr.Rows[0]["Call_Cancel"] = "Y";
                    }
                    if (iMenuId == 673)
                    {
                        DealerCode = "D009999";

                        iDealerID = 9999;
                    }
                    if (iMenuId == 672)
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
                                    //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                       // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Ticket No") + "','" + Server.HtmlEncode(txtTicketNo.Text) + "');</script>");
                        return false;
                    }
                    return true;
                }
                else
                {
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
                    FillCallType();
                    FillSubCallType();
                    FillDealer(); 
                    FillTitle(); 
                    GenerateTicketNo();
                    DisplayCurrentRecord("New");

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord(false) == false)
                    {
                        goto Last;
                    }
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
                Last: ;
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

            if (drpCallType.SelectedValue == "4")
            {
                drpCallSubType.Enabled = false;

            }
            else
            {
                drpCallSubType.Enabled = true;
            }
        }

        protected void lblSelectChassis_Click(object sender, EventArgs e)
        {

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
                // //   dtMTIFeedback.Rows[iRowCnt]["MTIRemark"] = MTIRemarks;
                //    if (DealerRemarks !="" )
                //    {
                //        CNT_D = CNT_D + 1;
                //    }

                //}
                //if (CNT_D > 0)
                //{
                    if (objCRM.bSaveAcknowledge(iCRMID,txtDetailsRemark.Text) == true)
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
                    if (MTIRemarks !="" )
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
                            if (objCRM.bSAVE_IsCalClose(iCRMID, Func.Convert.sConvertToString(txtcallcenterRemark.Text)) == true)
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

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        private void  bFillMTIFeedbackFromGrid()
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

         /***** NEW CODE ****/

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
        protected void drpStateDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldistrictDealer();
        }
        private void FilldistrictDealer()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDistrict;

                dsDistrict = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCallTypes", "7", Func.Convert.iConvertToInt(drpStateDealer.SelectedItem.Value));
                if (dsDistrict != null)
                {
                    drpDistrictDealer.DataSource = dsDistrict.Tables[0];
                    drpDistrictDealer.DataTextField = "Name";
                    drpDistrictDealer.DataValueField = "ID";
                    drpDistrictDealer.DataBind();
                    drpDistrictDealer.Items.Insert(0, new ListItem("--Select--", "0"));
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
   
        protected void drpDistrictDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealer();
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

       
    }
}