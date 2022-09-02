using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.VehicleSales
{
    public partial class frmAllocation : System.Web.UI.Page
    {
        private int iID;
        private int iPrimaryApplicationID;
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtFleetDetailsCust = new DataTable();

        private bool bDetailsRecordExist = false;
        private int iCustID;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        private DataSet DSDealer;
        private DataSet dsState;
        private string sControlClientID = "";
        int iUserId = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        private int EGpdealerID = 0;
        private int HOBrID = 0;
        string DealerCode = "";
        string sCustID = "";
        Boolean bSaveRecord = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                clsCustomer ObjCust = new clsCustomer();


                clsEGPSupplier ObjSup = new clsEGPSupplier();
                DataSet ds = new DataSet();
                DataSet dsDealer = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                //ToolbarC.iValidationIdForSave = 66;


                Location.bUseSpareDealerCode = false;
                Location.SetControlValue();
                PDoc.sFormID = "58";


                if (!IsPostBack)
                {
                    FillStateCountry();
                    FillLeadType();
                    FillCombo();

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                    Session["LeadFleetDtls"] = null;
                    Session["LeadObjective"] = null;
                    SearchGrid.sGridPanelTitle = "M0";
                    DisplayPreviousRecord();
                }

                //FillGrid();


                if (iID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                FillSelectionGrid();

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
                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);

                //lblTitle.Text = "M0";

                //if (!IsPostBack)


                //{
                //    FillStateCountry();
                //    FillLeadType();

                //    SearchGrid.bIsCollapsable = false;
                //    DisplayPreviousRecord();

                //}
                //drpCustType.Attributes.Add("onblur", "CheckcustType(event,this)");

                //if (!IsPostBack)
                //{
                //    SearchGrid.bIsCollapsable = false;
                //    //DisplayPreviousRecord();

                //}



            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //FillStateCountry();
                //FillLeadType();
                //iDealerID = Func.Convert.iConvertToInt(SearchGrid.iID);
                //GetDataAndDisplay();
                //FillCombo();
            }

            // FillSelectionGrid();

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

        private void GetDataAndDisplay()
        {
            try
            {

                clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                int iID = Func.Convert.iConvertToInt(txtID.Text);

                int sDoctype = Func.Convert.iConvertToInt(txtCrmCustID.Text);
                if (iID != 0)
                {


                    ds = ObjDealer.GetM0(iID, "All", iDealerID, iHOBrId, sDoctype);
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    DisplayData(ds);
                    ObjDealer = null;
                }


                else
                {
                    ds = ObjDealer.GetM0(iID, "Max", iDealerID, iHOBrId, sDoctype);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
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
            GetDataSearch();
            GetDataAndDisplay();


        }

        private void GetDataSearch()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetCRMCustID", iID, iDealerID, HOBrID);
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtCrmCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_Cust_ID"]);
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



        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                bool bRecordIsOpen = true;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_ID"]);
                    FillLeadType();
                    drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["type_flag_id"]);
                    FillTitle();
                    drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prefix"]);
                    //txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                    //txtFirstName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["First_Name"]);
                    //txtLastName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Last_Name"]);
                    txtNewCust.Text = "";
                    txtNewCust.Style.Add("display", "none");
                    drpCustName.Style.Add("display", "");
                    FillCombo();
                    drpCustName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_ID"]);

                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add2"]);

                    txtTcktID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TcktID"]);
                    txtCrmCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_Cust_ID"]);
                    txtTcktNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_No"]);
                    txtTcktDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_Date"]);


                    string IsMTICust = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_MTICust"]);
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
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pin"]);

                    FillStateCountry();

                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_ID"]);

                    Filldistrict();
                    drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["District_ID"]);

                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mobile"]);
                    //txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    txtLeadNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tr_Num"]);
                    txtDocDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["M0_Date"], false);
                    //drpVisitObj.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Obj_Id"]);

                    drpPrimaryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PriApp"]);
                    iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
                    Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
                    drpSeconadryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SecApp"]);
                    drpM0Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_Financier"]);
                    txtBodyBuilder.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BodyBuilder"]);
                    //txtNextDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Next_date"], false);

                    hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]);
                    hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]);


                    Session["LeadFleetDtls"] = null;
                    dtFleetDetails = ds.Tables[1];
                    Session["LeadFleetDtls"] = dtFleetDetails;
                    BindDataToGridFleet();

                    Session["LeadObjective"] = null;
                    dtDetails = ds.Tables[2];
                    Session["LeadObjective"] = dtDetails;
                    BindDataToGrid(bRecordIsOpen, 0);


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
                        MakeEnableDisableControls(true);
                    }
                    else if (hdnCancle.Value == "Y")
                    {
                        MakeEnableDisableControls(false);
                    }
                    else if (hdnConfirm.Value == "Y")
                    {
                        MakeEnableDisableControls(false);
                    }

                }


                else
                {
                    ClearDealerHeader();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        private void MakeEnableDisableControls(bool bEnable)
        {


            txtDocDate.Enabled = bEnable;
            txtLeadNo.Enabled = bEnable;
            drpCustType.Enabled = false;
            drpTitle.Enabled = false;
            //drpVisitObj.Enabled = bEnable;
            drpPrimaryApplication.Enabled = bEnable;
            drpSeconadryApplication.Enabled = bEnable;
            drpCustName.Enabled = false;
            txtNewCust.Enabled = false;
            //txtCustomerName.Enabled = bEnable;
            txtAddress1.Enabled = false;
            txtAddress2.Enabled = false;
            txtCity.Enabled = false;
            txtpincode.Enabled = false;
            drpState.Enabled = false;
            drpDistrict.Enabled = false;
            drpRegion.Enabled = false;
            txtMobile.Enabled = false;
            txtEmail.Enabled = false;
            //txtFirstName.Enabled = bEnable;
            //txtLastName.Enabled = bEnable;
            drpIsMTICust.Enabled = false;
            txtCountry.Enabled = false;
            bConfirm.Enabled = false;
            txtBodyBuilder.Enabled = bEnable;
            drpM0Financier.Enabled = bEnable;
            //txtNextDate.Enabled = bEnable;
            FleetDtls.Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }





        private void ClearDealerHeader()
        {
            drpCustType.SelectedValue = "0";
            drpTitle.SelectedValue = "0";
            //txtCustomerName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCity.Text = "";
            txtpincode.Text = "";
            drpState.SelectedValue = "0";
            //drpDistrict.SelectedItem.Text = "";
            //drpRegion.SelectedItem.Text = "";
            //drpRegion.SelectedValue = "0";
            //drpDistrict.SelectedValue = "0";
            //drpVisitObj.SelectedValue = "0";
            drpPrimaryApplication.SelectedValue = "0";
            drpSeconadryApplication.SelectedValue = "0";
            txtMobile.Text = "";
            txtEmail.Text = "";
        }

        private void FillSelectionGrid()
        {
            try
            {

                SearchGrid.bGridFillUsingSql = false;
                SearchGrid.AddToSearchCombo("M0Number");
                SearchGrid.AddToSearchCombo("M0 Date");
                SearchGrid.AddToSearchCombo("Type");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("Status");
                //SearchGrid.iDealerID = EGpdealerID;
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "M0Master";
                SearchGrid.sGridPanelTitle = "M0 (General Discussion) List";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
        private bool bValidateRecord()
        {

            string sMessage = " ";
            bool bValidateRecord = true;


            if (txtCity.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter City.";
                bValidateRecord = false;
            }
            if (txtCity.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter City.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;

                clsCustomer objCustomer = new clsCustomer();

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    //PSelectionGrid.Style.Add("display", "");
                    ////ClearDealerHeader();
                    //DisplayPreviousRecord();
                    //txtID.Text = "0";
                    //iID = 0;
                    //iCustID = 0;
                    //txtCustID.Text = "0";



                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //return;


                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);

                    //drpCustType.Enabled = true;
                    //drpTitle.Enabled = true;
                    ////txtCustomerName.Text = "";
                    //txtAddress1.Enabled = true;
                    //txtAddress2.Enabled = true;
                    //txtCity.Enabled = true;
                    //txtpincode.Enabled = true;
                    //drpState.Enabled = true;
                    //drpDistrict.Enabled = true;
                    //drpRegion.Enabled = true;
                    //txtMobile.Enabled = true;
                    //txtEmail.Enabled = true;



                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    // if (bValidateRecord() == false) return;
                    bDetailsRecordExist = false;

                    if (bFillDetailsFromGrid(true) == false) return;
                    bFillFleetFromGrid();
                    if (bDetailsRecordExist == false)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                        return;
                    }



                    if (txtDocDate.Text == "" || txtDocDate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M0 Date');</script>");
                        return;
                    }

                    if (drpPrimaryApplication.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Primary Application');</script>");
                        return;
                    }
                    if (drpSeconadryApplication.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Secondary Application');</script>");
                        return;
                    }

                    sCustID = Func.Convert.sConvertToString(drpCustName.SelectedValue);
                    iCustID = Func.Convert.iConvertToInt(txtCustID.Text);

                    iCustID = Func.Convert.iConvertToInt(drpCustName.SelectedValue);


                    if (Func.Convert.sConvertToString(drpCustName.SelectedValue) == "X")
                    {
                        iCustID = objCustomer.bSaveM0Master(iCustID, sCustID, iDealerID, HOBrID, Func.Convert.iConvertToInt(drpCustType.SelectedValue),
                            Func.Convert.iConvertToInt(drpTitle.SelectedValue),
                            // txtFirstName.Text, txtLastName.Text
                            //,
                            txtNewCust.Text,
                            txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtpincode.Text,
                            Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                             Func.Convert.iConvertToInt(drpState.SelectedValue)
                             , Func.Convert.iConvertToInt(drpDistrict.SelectedValue), 1, txtMobile.Text, txtEmail.Text
                             , Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue)
                , Func.Convert.iConvertToInt(drpSeconadryApplication.SelectedValue), drpIsMTICust.SelectedItem.Text
                , Func.Convert.iConvertToInt(drpM0Financier.SelectedValue), txtBodyBuilder.Text, "D", "", 0
                            );

                    }
                    else
                    {
                        iCustID = objCustomer.bSaveM0Master(iCustID, sCustID, iDealerID, HOBrID, Func.Convert.iConvertToInt(drpCustType.SelectedValue),
                            Func.Convert.iConvertToInt(drpTitle.SelectedValue),
                            // txtFirstName.Text, txtLastName.Text
                            //,
                            drpCustName.SelectedItem.Text,
                            txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtpincode.Text,
                            Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                             Func.Convert.iConvertToInt(drpState.SelectedValue)
                             , Func.Convert.iConvertToInt(drpDistrict.SelectedValue), 1, txtMobile.Text, txtEmail.Text
                                , Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue)
                , Func.Convert.iConvertToInt(drpSeconadryApplication.SelectedValue), drpIsMTICust.SelectedItem.Text
                , Func.Convert.iConvertToInt(drpM0Financier.SelectedValue), txtBodyBuilder.Text, "", "", 0
                            );

                    }

                    if (iCustID > 0)
                    {

                        iID = Func.Convert.iConvertToInt(txtID.Text);

                        iID = bSaveM0Details(iCustID, "N", "N");


                        if (iID > 0)
                        {
                            if (bSaveDetails("N", "N") == true)
                            {

                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            }
                            else
                            {
                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                            }
                        }

                    }




                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    iCustID = Func.Convert.iConvertToInt(txtCustID.Text);
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    if (iID > 0)
                    {

                        iID = bSaveM0Details(iCustID, "Y", "N");

                        if (iID > 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                        }
                        //GetDataAndDisplay();

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }

                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {


                    iCustID = Func.Convert.iConvertToInt(txtCustID.Text);
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    if (iID > 0)
                    {

                        iID = bSaveM0Details(iCustID, "N", "Y");

                        if (iID > 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                    }


                }
                PSelectionGrid.Style.Add("display", "");
                FillSelectionGrid();
                GetDataAndDisplay();
                PDoc.BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {

            txtTcktID.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            txtCrmCustID.Text = Func.Convert.sConvertToString(PDoc.sDoc);

            FillCombo();
            FillSelectionGrid();

            PSelectionGrid.Style.Add("display", "none");
            txtID.Text = "";

            GetDataFromM0();
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }
        private void GetDataFromM0()
        {
            try
            {
                //clsCustomer ObjDealer = new clsCustomer();
                clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();

                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                int sDocType = Func.Convert.iConvertToInt(txtCrmCustID.Text);
                //iProformaID = 15;
                if (iM0ID != 0)
                {
                    ds = ObjDealer.GetM0(iID, "New", iDealerID, iHOBrId, sDocType);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //ObjDealer = null;
                }
                else
                {
                    ClearDealerHeader();
                }


                txtTcktID.Text = Func.Convert.sConvertToString(txtPreviousDocId.Text);


                GetTcktDet(Func.Convert.iConvertToInt(txtTcktID.Text));




                GenerateLeadNo("L");

                //ListItem lstitm = new ListItem("NEW", "X");
                //drpCustName.Items.Add(lstitm);

                txtNewCust.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void GetTcktDet(int iTckt)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                if (iTckt != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetTcktDet", iTckt, iDealerID, HOBrID);
                    txtTcktNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_No"]);
                    txtTcktDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_Date"]);
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

        public bool bSaveDetails(string Cancel, string Confirm)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();



            if (bSaveM0Objectives(objDB, iDealerID, iHOBrId, dtDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }


            if (bSaveM0FleetDtls(objDB, iDealerID, iHOBrId, dtFleetDetails, iCustID) == true)
            {
                bSaveRecord = true;
            }

            return bSaveRecord;
        }

        public bool bSaveM0Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M0Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveM0FleetDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                    //,,,,,,

                    objDB.ExecuteStoredProcedure("SP_Save_M0FleetDetails", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["CompID"],
                        dtDet.Rows[iRowCnt]["model1"], dtDet.Rows[iRowCnt]["qty1"], dtDet.Rows[iRowCnt]["model2"],
                        dtDet.Rows[iRowCnt]["qty2"], dtDet.Rows[iRowCnt]["model3"], dtDet.Rows[iRowCnt]["qty3"]
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


        public int bSaveM0Details(int iCustID, string Confirm, string Cancel)
        {
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_M0Details", iID, iDealerID, iHOBrId, iCustID, txtLeadNo.Text, txtDocDate.Text
                    , Confirm, Cancel, Func.Convert.iConvertToInt(txtTcktID.Text)

                    );

                objDB.CommitTransaction();


                string sFinYear = Func.sGetFinancialYear();

                string sDocName = "";
                sDocName = "M0";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerID);
                }
                return iID;




            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

            return iID;
        }


        private void FillGrid()
        {
            try
            {
                string[] url_New = HttpContext.Current.Request.Url.AbsoluteUri.Split('=');
                if (url_New.Length > 1)
                    if (Convert.ToInt32(url_New[1]) == 155)
                        SearchGrid.sModelPart = "E";
                    else if (Convert.ToInt32(url_New[1]) == 184)
                        SearchGrid.sModelPart = "D";
                clsCommon objclsComman = new clsCommon();
                SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", SearchGrid.sModelPart, iDealerID, "M0Master"));
                objclsComman = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
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
                dsRegion = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpState.SelectedValue, "Region");
                if (dsRegion != null)
                {
                    drpRegion.DataSource = dsRegion.Tables[0];
                    drpRegion.DataTextField = "Name";
                    drpRegion.DataValueField = "ID";
                    drpRegion.DataBind();
                    // drpRegion.Items.Insert(0, new ListItem("--Select--", "0"));
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
                dsDistrict = objDB.ExecuteStoredProcedureAndGetDataset("SP_Filldistrict", drpState.SelectedValue);
                if (dsDistrict != null)
                {
                    drpDistrict.DataSource = dsDistrict.Tables[0];
                    drpDistrict.DataTextField = "Name";
                    drpDistrict.DataValueField = "ID";
                    drpDistrict.DataBind();
                    // drpRegion.Items.Insert(0, new ListItem("--Select--", "0"));
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
        //private void FillCustType()
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet dsCustType;

        //        //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
        //        dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetCustSubType", 0);
        //        if (dsCustType != null)
        //        {
        //            drpcustSubType.DataSource = dsCustType.Tables[0];
        //            drpcustSubType.DataTextField = "Name";
        //            drpcustSubType.DataValueField = "ID";
        //            drpcustSubType.DataBind();
        //            drpcustSubType.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }


        //}

        protected void bConvertToM1(object sender, EventArgs e)
        {
            iCustID = Func.Convert.iConvertToInt(txtCustID.Text);
            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                iID = bSaveM0Details(iCustID, "Y", "N");


                FillSelectionGrid();
                GetDataAndDisplay();
            }

        }



        private void FillLeadType()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType;

                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetLeadType", 0);
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
        protected void drpPrimaryApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
            Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);

        }
        //private void FillOrgType()
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet dsOrgtype;

        //        //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");

        //        if (drpCustType.SelectedValue != "1")
        //        {
        //            drporgType.Enabled = true;
        //            dsOrgtype = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetOrgType", drpCustType.SelectedValue);
        //            if (dsOrgtype != null)
        //            {
        //                drporgType.DataSource = dsOrgtype.Tables[0];
        //                drporgType.DataTextField = "Name";
        //                drporgType.DataValueField = "ID";
        //                drporgType.DataBind();
        //                drporgType.Items.Insert(0, new ListItem("--Select--", "0"));
        //            }

        //        }
        //        else
        //        {
        //            drporgType.Enabled = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }


        //}
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

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillRegion();
            Filldistrict();
        }



        protected void drpCustType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTitle();

            //if (Func.Convert.iConvertToInt(drpCustType.SelectedValue) == 1)
            //{
            //    txtFirstName.Enabled = true;
            //    txtLastName.Enabled = true;
            //    txtCustomerName.Enabled = false;
            //}
            //else if (Func.Convert.iConvertToInt(drpCustType.SelectedValue) == 2)
            //{
            //    txtFirstName.Enabled = false;
            //    txtLastName.Enabled = false;
            //    txtCustomerName.Enabled = true;
            //}


        }
        //protected void txtFirstName_TextChanged(object sender, EventArgs e)
        //{
        //    txtCustomerName.Text = txtFirstName.Text + " " + txtLastName.Text;
        //}

        //protected void txtLastName_TextChanged(object sender, EventArgs e)
        //{
        //    txtCustomerName.Text = txtFirstName.Text + " " + txtLastName.Text;
        //}

        private void FillDetailsFromGrid()
        {
            //


        }
        private void FillTitle()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsTitle;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsTitle = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetNameTitle", drpCustType.SelectedValue);
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



        //private void GenerateLeadNo(string Type)
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 26032012
        //    DLL.clsDB objDB = new DLL.clsDB();
        //    try
        //    {
        //        DataSet dsDCode = new DataSet();


        //        dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerID);

        //        if (dsDCode.Tables[0].Rows.Count > 0)
        //        {
        //            DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }


        //    if (Type == "L")
        //    {
        //        txtLeadNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerID, "L"));

        //    }




        //}

        //public string FindMAxLeadNo(string VDealerCode, int iDealerID, string Type)
        //{
        //    try
        //    {
        //        string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear();
        //        int iMaxDocNo;
        //        sFinYearChar = sFinYear.Substring(3);

        //        // 'Commented by Shyamal as on 26032012
        //        //objDB.BeginTranasaction();
        //        string sDocName = "";
        //        if (Type == "L")
        //        {
        //            sDocName = "L";
        //        }
        //        else if (Type == "I")
        //        {
        //            sDocName = "IQ";
        //        }



        //        iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
        //        sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
        //        sMaxDocNo = sMaxDocNo.PadLeft(3, '0');
        //        // sDocNo = sDealerCode.Substring(2, 4) + "PO" + sFinYearChar + sMaxDocNo;
        //        // sDocNo = sDealerCode + "PO" + sFinYearChar + sMaxDocNo;
        //        // sDocNo = sDealerCode.Substring(2, 4) + "RO" + sMaxDocNo;
        //        if (VDealerCode != "")
        //        {
        //            sDocNo = VDealerCode.Substring(2, 4) + sDocName + sFinYearChar + sMaxDocNo;
        //        }
        //        else
        //        {
        //            sDocNo = sDocName + sFinYearChar + sMaxDocNo;
        //        }


        //        return sDocNo;
        //    }
        //    catch
        //    {

        //        return "0";
        //    }
        //}


        private void DisplayPreviousRecord()
        {
            try
            {

                clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();



                ds = ObjDealer.GetM0(iID, "New", iDealerID, iHOBrId, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            //ds.Tables[0].Rows[0]["PO_Cancel"] = "N";
                            //ds.Tables[0].Rows[0]["PO_Confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";
                            //sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                //else
                //{
                //    BindDataToGrid(true, 0);
                //}
                txtID.Text = "";

                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void GenerateLeadNo(string Type)
        {
            // 'Replace Func.DB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDCode = new DataSet();


                dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerID);

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


            if (Type == "L")
            {
                txtLeadNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerID, "L"));

            }
            //else if (Type == "I")
            //{
            //    txtInqNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "I"));

            //}




        }

        public string FindMAxLeadNo(string VDealerCode, int iDealerID, string Type)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear();
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";
                if (Type == "L")
                {
                    sDocName = "M0";
                }
                else if (Type == "I")
                {
                    sDocName = "IQ";
                }



                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(3, '0');

                if (VDealerCode != "")
                {
                    sDocNo = VDealerCode.Substring(2, 4) + sDocName + sFinYearChar + sMaxDocNo;
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
        private void FillCombo()
        {
            //Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0);
            Func.Common.BindDataToCombo(drpPrimaryApplication, clsCommon.ComboQueryType.PrimaryApplication, 0);
            iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);

            Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
            Func.Common.BindDataToCombo(drpM0Financier, clsCommon.ComboQueryType.Financier, 0);
            Func.Common.BindDataToCombo(drpCustName, clsCommon.ComboQueryType.LeadName, iDealerID, " and HOBr_ID=" + iHOBrId);
            //Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0);
        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillDetailsFromGrid(false);
                BindDataToGrid(true, 1);
            }
        }

        protected void FleetGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            // {
            //   bFillFleetFromGrid();
            //  BindDataToGridFleet();
            // }
        }
        private bool bFillFleetFromGrid()
        {
            dtFleetDetails = (DataTable)Session["LeadFleetDtls"];

            //double dQty = 0;

            for (int iRowCnt = 0; iRowCnt < FleetDtls.Rows.Count; iRowCnt++)
            {
                Label lblCompNo = FleetDtls.Rows[iRowCnt].FindControl("lblCompNo") as Label;


                DropDownList drpFleetModel1 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel1");
                dtFleetDetails.Rows[iRowCnt]["model1"] = Func.Convert.iConvertToInt(drpFleetModel1.SelectedValue);

                TextBox txtQty1 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty1");
                dtFleetDetails.Rows[iRowCnt]["Qty1"] = Func.Convert.sConvertToString(txtQty1.Text);

                DropDownList drpFleetModel2 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel2");
                dtFleetDetails.Rows[iRowCnt]["model2"] = Func.Convert.iConvertToInt(drpFleetModel2.SelectedValue);

                TextBox txtQty2 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty2");
                dtFleetDetails.Rows[iRowCnt]["Qty2"] = Func.Convert.sConvertToString(txtQty2.Text);

                DropDownList drpFleetModel3 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel3");
                dtFleetDetails.Rows[iRowCnt]["model3"] = Func.Convert.iConvertToInt(drpFleetModel3.SelectedValue);

                TextBox txtQty3 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty3");
                dtFleetDetails.Rows[iRowCnt]["Qty3"] = Func.Convert.sConvertToString(txtQty3.Text);


                //int iDtSelPartRow = 0;

                //for (int iDtRowCnt = 0; iDtRowCnt < dtFleetDetails.Rows.Count; iDtRowCnt++)
                //{
                //    if (Func.Convert.iConvertToInt(dtFleetDetails.Rows[iDtRowCnt]["CompID"]) == Func.Convert.iConvertToInt(lblCompNo.Text))
                //    {
                //        iDtSelPartRow = iDtRowCnt;
                //        break;
                //    }
                //}

                // Get Qty

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtHDTQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["HDTQty"] = dQty;



                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtLDTQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["LDTQty"] = dQty;

                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtLDBusQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["LDBusQty"] = dQty;

                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtMDTQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["MDTQty"] = dQty;

                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtMDBusQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["MDBusQty"] = dQty;

                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtHDBusQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["HDBusQty"] = dQty;

                //dQty = 0;

                //dQty = Func.Convert.dConvertToDouble((FleetDtls.Rows[iRowCnt].FindControl("txtEngQty") as TextBox).Text);
                //dtFleetDetails.Rows[iDtSelPartRow]["EngQty"] = dQty;







            }
            bDetailsRecordExist = true;
            return true;
        }
        private void BindDataToGridFleet()
        {
            //If No Data in Grid
            if (Session["LeadFleetDtls"] == null)
            {
                Session["LeadFleetDtls"] = dtFleetDetails;
            }
            else
            {
                dtFleetDetails = (DataTable)Session["LeadFleetDtls"];
            }
            Session["LeadFleetDtls"] = dtFleetDetails;
            FleetDtls.DataSource = dtFleetDetails;
            FleetDtls.DataBind();
            SetGridControlPropertyFleet(false);



        }

        private void SetGridControlPropertyFleet(bool bRecordIsOpen)
        {

            string sDealerId = Func.Convert.sConvertToString(iDealerID);
            int idtRowCnt = 0;
            dtFleetDetails = (DataTable)Session["LeadFleetDtls"];
            for (int iRowCnt = 0; iRowCnt < FleetDtls.Rows.Count; iRowCnt++)
            {

                TextBox txtFleetID = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtFleetID");
                Label lblCompNo = (Label)FleetDtls.Rows[iRowCnt].FindControl("lblCompNo");
                Label lblCompName = (Label)FleetDtls.Rows[iRowCnt].FindControl("lblCompName");


                DropDownList drpFleetModel1 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel1");
                Func.Common.BindDataToCombo(drpFleetModel1, clsCommon.ComboQueryType.MTIFleet, 0);

                TextBox txtQty1 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty1");


                DropDownList drpFleetModel2 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel2");
                Func.Common.BindDataToCombo(drpFleetModel2, clsCommon.ComboQueryType.MTIFleet, 0);

                TextBox txtQty2 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty2");


                DropDownList drpFleetModel3 = (DropDownList)FleetDtls.Rows[iRowCnt].FindControl("drpFleetModel3");
                Func.Common.BindDataToCombo(drpFleetModel3, clsCommon.ComboQueryType.MTIFleet, 0);

                TextBox txtQty3 = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtQty3");
                ////Label lblHDTQty = (Label)FleetDtls.Rows[iRowCnt].FindControl("lblHDTQty");

                //TextBox txtLDTQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtLDTQty");

                //TextBox txtLDBusQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtLDBusQty");
                //TextBox txtMDTQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtMDTQty");

                //TextBox txtMDBusQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtMDBusQty");
                //TextBox txtHDBusQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtHDBusQty");
                //TextBox txtEngQty = (TextBox)FleetDtls.Rows[iRowCnt].FindControl("txtEngQty");

                if (idtRowCnt < dtFleetDetails.Rows.Count)
                {
                    txtFleetID.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["ID"]);
                    lblCompNo.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["CompID"]);
                    lblCompName.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["Competitor_Name"]);

                    drpFleetModel1.SelectedValue = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["model1"]);
                    txtQty1.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["qty1"]);

                    drpFleetModel2.SelectedValue = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["model2"]);
                    txtQty2.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["qty2"]);

                    drpFleetModel3.SelectedValue = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["model3"]);
                    txtQty3.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["qty3"]);

                    //txtHDTQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["HDTQty"]);
                    ////lblHDTQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["HDTQty"]);
                    //txtLDTQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["LDTQty"]);
                    //txtLDBusQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["LDBusQty"]);
                    //txtMDTQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["MDTQty"]);
                    //txtMDBusQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["MDBusQty"]);
                    //txtHDBusQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["HDBusQty"]);
                    //txtEngQty.Text = Func.Convert.sConvertToString(dtFleetDetails.Rows[iRowCnt]["EngQty"]);

                    idtRowCnt = idtRowCnt + 1;
                }

            }
        }

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FillStateCountry();

        }

        protected void drpCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int custID;

            if (drpCustName.SelectedItem.Text == "NEW")
            {
                txtNewCust.Text = "";

                txtNewCust.Style.Add("display", "");
                txtNewCust.Focus();
                drpCustName.Style.Add("display", "none");

                drpCustType.SelectedValue = "0";
                drpTitle.SelectedValue = "0";
                //txtCustomerName.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtCity.Text = "";
                txtpincode.Text = "";
                drpState.SelectedValue = "0";
                //drpDistrict.SelectedItem.Text = "";
                //drpRegion.SelectedItem.Text = "";
                FillRegion();
                Filldistrict();
                //drpRegion.SelectedValue = "0";
                //drpDistrict.SelectedValue = "0";

                drpPrimaryApplication.SelectedValue = "0";
                drpSeconadryApplication.SelectedValue = "0";
                txtMobile.Text = "";
                txtEmail.Text = "";


                drpCustType.Enabled = true;
                drpTitle.Enabled = true;
                //txtCustomerName.Text = "";
                txtAddress1.Enabled = true;
                txtAddress2.Enabled = true;
                txtCity.Enabled = true;
                txtpincode.Enabled = true;
                drpState.Enabled = true;
                drpDistrict.Enabled = true;
                drpRegion.Enabled = true;
                txtMobile.Enabled = true;
                txtEmail.Enabled = true;

            }
            else
            {
                txtNewCust.Text = "";
                txtNewCust.Style.Add("display", "none");

                custID = Func.Convert.iConvertToInt(drpCustName.SelectedValue);

                GetCustData(custID);

                drpCustType.Enabled = false;
                drpTitle.Enabled = false;
                //txtCustomerName.Text = "";
                txtAddress1.Enabled = false;
                txtAddress2.Enabled = false;
                txtCity.Enabled = false;
                txtpincode.Enabled = false;
                drpState.Enabled = false;
                drpDistrict.Enabled = false;
                drpRegion.Enabled = false;
                txtMobile.Enabled = false;
                txtEmail.Enabled = false;




            }
            //else { txtProcessName.Text = ""; }

        }


        private void GetCustData(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                if (ID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetCustData", ID, iDealerID, HOBrID);
                    txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    FillLeadType();
                    drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["type_flag_id"]);
                    FillTitle();
                    drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Prefix"]);
                    //txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                    //txtFirstName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["First_Name"]);
                    //txtLastName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Last_Name"]);



                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add2"]);

                    string IsMTICust = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_MTICust"]);
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
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pin"]);

                    FillStateCountry();

                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_ID"]);

                    Filldistrict();
                    drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["District_ID"]);

                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mobile"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    drpPrimaryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PriApp"]);
                    iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
                    Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
                    drpSeconadryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SecApp"]);
                    drpM0Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_Financier"]);
                    txtBodyBuilder.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BodyBuilder"]);



                    // Session["LeadFleetDtls"] = null;
                    dtFleetDetails = ds.Tables[1];
                    Session["LeadFleetDtls"] = dtFleetDetails;
                    BindDataToGridFleet();


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



        private void SetGridControlProperty(bool bRecordIsOpen)
        {
            string sDeleteStatus = "";
            string sDealerId = Func.Convert.sConvertToString(iDealerID);
            string sModelID = "0";
            int idtRowCnt = 0;



            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {


                TextBox txtObjID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtObjID");

                DropDownList drpVisitObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpVisitObj");
                Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0);

                //Get Date
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate")) as ASP.webparts_currentdate_ascx).Enabled = true;
                (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Enabled = true;


                //Get discussion
                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");

                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");
                Func.Common.BindDataToCombo(drpNextObj, clsCommon.ComboQueryType.LeadObjective, 0);


                //TextBox txtNextObjDate = 
                (DetailsGrid.Rows[iRowCnt].FindControl("txtNextObjDate") as MANART.WebParts.CurrentDate).Text = "";



                TextBox txtCommitment = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtCommitment");

                sDeleteStatus = "E";
                if (idtRowCnt < dtDetails.Rows.Count)
                {


                    txtObjID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["ID"]));
                    drpVisitObj.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["obj_Id"]);

                    //txtObjDate.Text = Func.Convert.tConvertToDate(Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["obj_date"]),false);
                    (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["obj_date"]);
                    //Func.Convert.sConvertToString(dtDetails.Rows[(ModelGrid.PageIndex * ModelGrid.PageSize) + iRowCnt]["EffectiveFromDate"]); ;

                    txtDiscussion.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["discussion"]);

                    txtTimeSpent.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["time_spent"]);

                    drpNextObj.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["next_obj_Id"]);

                    //txtNextObjDate.Text = Func.Convert.tConvertToDate(Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["next_date"]), false);
                    (DetailsGrid.Rows[iRowCnt].FindControl("txtNextObjDate") as MANART.WebParts.CurrentDate).Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["next_date"]);
                    //drpPlatform.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["plt_Id"]);
                    txtCommitment.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["commit_det"]);

                    sDeleteStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

                    idtRowCnt = idtRowCnt + 1;



                }

                //New 
                LinkButton lnkNew = (LinkButton)DetailsGrid.Rows[iRowCnt].FindControl("lnkNew");
                lnkNew.Style.Add("display", "none");



                //Delete 
                CheckBox Chk = (CheckBox)DetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                Chk.Attributes.Add("onClick", "SelectDeletCheckbox(this)");
                Chk.Style.Add("display", "none");

                // N :- New , D:- Dellete, E:- Exissting            
                if (sDeleteStatus == "D")
                {
                    Chk.Style.Add("display", "");
                    Chk.Checked = true;
                    //DetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                }
                else if (sDeleteStatus == "E")
                {
                    lnkNew.Style.Add("display", "none");
                    Chk.Style.Add("display", "");
                    Chk.Checked = false;
                }

                // Allow New To Last Row
                if ((iRowCnt + 1) == DetailsGrid.Rows.Count)
                {
                    lnkNew.Style.Add("display", "");
                }



            }



        }





        // to create Emty Row To Grid
        private void CreateNewRowToDetailsTable(int iNoRowToAdd)
        {
            try
            {
                //MaxRFPModelRowCount
                DataRow dr;
                DataTable dtDefaultModel = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxLeadObjRowCount = 1;
                //iMaxRFPModelRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxRFPModelRowCount"]);

                if (Session["LeadObjective"] != null)
                {
                    dtDefaultModel = (DataTable)Session["LeadObjective"];
                }
                else
                {
                    dtDefaultModel = dtDetails;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultModel.Rows.Count == 0)
                    {
                        dtDefaultModel.Columns.Clear();


                        //dtDefaultModel.Columns.Add(new DataColumn("SRNo", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("obj_Id", typeof(int)));

                        dtDefaultModel.Columns.Add(new DataColumn("obj_date", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("discussion", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("time_spent", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("next_obj_Id", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("next_date", typeof(string)));
                        //dtDefaultModel.Columns.Add(new DataColumn("plt_Id", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("commit_det", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("Status", typeof(string)));

                    }
                    else
                    {
                        if (dtDefaultModel.Rows.Count >= iMaxLeadObjRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxLeadObjRowCount;
                }

                iMaxLeadObjRowCount = iMaxLeadObjRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLeadObjRowCount; iRowCnt++)
                {
                    dr = dtDefaultModel.NewRow();
                    //dr["SRNo"] = "1";
                    dr["ID"] = 0;
                    dr["obj_Id"] = 0;
                    dr["obj_date"] = "31/12/9999";
                    dr["discussion"] = "";
                    dr["time_spent"] = "";
                    dr["next_obj_Id"] = 0;
                    dr["next_date"] = "31/12/9999";
                    //dr["plt_Id"] = 0;
                    dr["commit_det"] = "";
                    dr["Status"] = "";
                    dtDefaultModel.Rows.Add(dr);
                    dtDefaultModel.AcceptChanges();

                }
            Bind: ;
                Session["LeadObjective"] = dtDefaultModel;
                DetailsGrid.DataSource = dtDefaultModel;
                DetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Grid
        private bool bFillDetailsFromGrid(bool bDisplayMsg)
        {
            string sStatus = "";
            dtDetails = (DataTable)Session["LeadObjective"];
            int iCntForDelete = 0;
            int iModelBodyTypeID = 0;
            bDetailsRecordExist = false;
            int iModelID = 0;
            int iCntForSelect = 0;


            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                TextBox txtObjID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtObjID");
                dtDetails.Rows[iRowCnt]["ID"] = Func.Convert.iConvertToInt(txtObjID.Text);

                DropDownList drpVisitObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpVisitObj");

                dtDetails.Rows[iRowCnt]["obj_Id"] = Func.Convert.iConvertToInt(drpVisitObj.SelectedValue);

                if (Func.Convert.iConvertToInt(drpVisitObj.SelectedValue) != 0)
                {
                    iCntForSelect = iCntForSelect + 1;
                }

                //TextBox txtObjDate = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate");
                //dtDetails.Rows[iRowCnt]["obj_date"] = Func.Convert.tConvertToDate(txtObjDate.Text, false);
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["obj_date"]));
                dtDetails.Rows[iRowCnt]["obj_date"] = Func.Convert.sConvertToString((DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text);


                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");
                dtDetails.Rows[iRowCnt]["discussion"] = Func.Convert.sConvertToString(txtDiscussion.Text);

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");
                dtDetails.Rows[iRowCnt]["time_spent"] = Func.Convert.sConvertToString(txtTimeSpent.Text);


                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");

                dtDetails.Rows[iRowCnt]["next_obj_Id"] = Func.Convert.iConvertToInt(drpNextObj.SelectedValue);


                //TextBox txtNextObjDate = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtNextObjDate");
                //dtDetails.Rows[iRowCnt]["next_date"] = Func.Convert.tConvertToDate(txtNextObjDate.Text, false);
                dtDetails.Rows[iRowCnt]["next_date"] = Func.Convert.sConvertToString((DetailsGrid.Rows[iRowCnt].FindControl("txtNextObjDate") as MANART.WebParts.CurrentDate).Text);



                //DropDownList drpPlatform = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpPlatform");

                //dtDetails.Rows[iRowCnt]["plt_Id"] = Func.Convert.iConvertToInt(drpPlatform.SelectedValue);


                TextBox txtCommitment = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtCommitment");
                dtDetails.Rows[iRowCnt]["commit_det"] = Func.Convert.sConvertToString(txtCommitment.Text);




                CheckBox Chk = (CheckBox)DetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                dtDetails.Rows[iRowCnt]["Status"] = "";
                if (Chk.Checked == true)
                {
                    dtDetails.Rows[iRowCnt]["Status"] = "D";
                    bDetailsRecordExist = true;
                    iCntForDelete++;
                }
                else if (drpVisitObj.SelectedValue != "0")
                {
                    dtDetails.Rows[iRowCnt]["Status"] = "N";
                    bDetailsRecordExist = true;
                }
            }

            if (iCntForDelete == iCntForSelect)
            {
                if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Entered atleast One Record !');</script>");
                return false;
            }
            return true;


        }
        private void BindDataToGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            if (bRecordIsOpen == true)
            {
                CreateNewRowToDetailsTable(iNoRowToAdd);
            }
            else
            {
                DetailsGrid.DataSource = dtDetails;
                DetailsGrid.DataBind();
            }
            SetGridControlProperty(bRecordIsOpen);
        }


    }
}