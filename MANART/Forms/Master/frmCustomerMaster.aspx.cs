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

namespace MANART.Forms.Master
{
    public partial class frmCustomerMaster : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        private DataSet DSDealer;
        private DataSet dsState;
        private string sControlClientID = "";
        int iUserId = 0;
        private int EGpdealerID = 0;
        int iHOBr_id = 0;
        string status = "";
        int RowCount = 0;
        DataSet ds = new DataSet();
        DataSet dsGlobal = new DataSet();
        //DataTable ds = new DataTable();
        //DataTable dsGlobal = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtGlobal = new DataTable();
        DataSet dsGroupCat = new DataSet();
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iID = Func.Convert.iConvertToInt(txtID.Text);
                clsCustomer ObjCust = new clsCustomer();

                DataSet dsMax = new DataSet();
                if (!IsPostBack)
                {
                    // FillRegion();
                    FillStateCountry();
                    FillCustType();
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                }
                // SearchGrid.sGridPanelTitle = "Customer Master";
                // FillSelectionGrid();
                //  iID = Func.Convert.iConvertToInt(txtID.Text);
                if (iID == 0)
                    // dsMax = ObjCust.GetMaxCustomer(iHOBr_id);
                    dsMax = ObjCust.GetMaxCustomer(iDealerID);
                if (dsMax.Tables[0].Rows.Count > 0)
                {
                    iID = Func.Convert.iConvertToInt(dsMax.Tables[0].Rows[0]["ID"]);
                }
                if (iID != 0)
                {
                    GetDataAndDisplay(iID);

                }
                //if (iDealerID != 0)
                //{
                //    GetDataAndDisplay();
                //}
                FillSelectionGrid();
                //DisplayData();
                //DisplayData_Search();

                ToolbarC.iValidationIdForSave = 62;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillSelectionGrid()
        {
            try
            {
                // SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Customer Name");
                //SearchGrid.AddToSearchCombo("City");
                //SearchGrid.AddToSearchCombo("State");
                //SearchGrid.AddToSearchCombo("Region");
                //SearchGrid.AddToSearchCombo("Country");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "CustomerMasterList";
                SearchGrid.iBrHODealerID = iHOBr_id;
                SearchGrid.sGridPanelTitle = "Customer Master List";
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
            GetDataAndDisplay(iID);
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //  //  DisplayData_Search();
        //}
        //protected void btnClearSearch_Click(object sender, EventArgs e)
        //{
        //    btnClearSearch.Visible = false;
        //    txtSearch.Text = "";
        //    lblMessage1.Visible = false;
        //    DisplayData();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblTitle.Text = "Customer";

                if (!IsPostBack)
                {
                    // SearchGrid.bIsCollapsable = false;
                }
                drpCustType.Attributes.Add("onblur", "CheckcustType(event,this)");
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
                iID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay(iID);
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

        private void GetDataAndDisplay(int iID)
        {
            try
            {
                clsCustomer Objcust = new clsCustomer();
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = Objcust.GetCustomerdetails(iID, iDealerID);
                    DisplayDataCurrentRecord(ds);
                    Objcust = null;
                }
                else
                {
                    ds = null;
                    DisplayDataCurrentRecord(ds);
                    Objcust = null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //private void DisplayData()
        //{
        //    try
        //    {
        //        clsCustomer ObjCust = new clsCustomer();
        //        // Changed By VIkram For Apply Branch And Dealer Filter Applying
        //        ds = ObjCust.GetCustomer(iDealerID, iHOBr_id);

        //        if (ds == null)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            Session["CustomerData"] = ds.Tables[0];
        //            Session["GlobalCustomerData"] = ds.Tables[1];
        //            DetailsGrid.DataSource = ds.Tables[0];
        //            DetailsGrid.DataBind();
        //            hdnRowCount.Value = "0";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }


        //}
        //private void DisplayData_Search()
        //{
        //    try
        //    {

        //        DataView dvDetails = new DataView();
        //        dt = (DataTable)Session["CustomerData"];
        //        dvDetails = dt.DefaultView;

        //        DataView dvDetailsGlobal = new DataView();

        //        dtGlobal = (DataTable)Session["GlobalCustomerData"];
        //        // dsGlobal= (DataSet)Session["GlobalCustomerData"];
        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                dvDetails = dt.DefaultView;
        //                if (status == "New")
        //                {
        //                    dvDetails.RowFilter = "Customer_name LIKE '" + txtCustomerName.Text + "*' and add1  LIKE '" + txtAddress1.Text + "*' and pincode  LIKE '" + txtpincode.Text + "*' and Mobile LIKE '" + txtMobile.Text + "*'";
        //                }
        //                else if (drpSearch.SelectedValue == "1" && txtSearch.Text != "" && status == "")
        //                {
        //                    dvDetails.RowFilter = "Customer_name LIKE '" + txtSearch.Text + "*'";
        //                }


        //                DetailsGrid.DataSource = dvDetails.ToTable();
        //                DetailsGrid.DataBind();
        //                hdnRowCount.Value = "0";
        //                hdnRowCount.Value = Func.Convert.sConvertToString(DetailsGrid.Rows.Count);



        //            }
        //        }
        //        if (dvDetails.ToTable().Rows.Count == 0 && status == "New")
        //        {

        //            dvDetailsGlobal = dtGlobal.DefaultView;
        //            if (status == "New")
        //            {
        //                dvDetailsGlobal.RowFilter = "Customer_name LIKE '" + txtCustomerName.Text + "*' and add1  LIKE '" + txtAddress1.Text + "*' and pincode  LIKE '" + txtpincode.Text + "*' and Mobile LIKE '" + txtMobile.Text + "*'";
        //            }



        //            DetailsGrid.DataSource = dvDetailsGlobal.ToTable();
        //            DetailsGrid.DataBind();
        //            //hdnRowCount.Value = "0";
        //            //hdnRowCount.Value = Func.Convert.sConvertToString(DetailsGrid.Rows.Count);
        //            hdnRowCountGlobal.Value = "0";
        //            hdnRowCountGlobal.Value = Func.Convert.sConvertToString(DetailsGrid.Rows.Count);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //}

        // Display Data 
        private void DisplayDataCurrentRecord(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }
                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_Type"]);
                    if (drpCustType.SelectedValue == "6" || drpCustType.SelectedValue == "4")
                    {
                        FieldEnableDisable(false);
                        FilldealerAll();
                        drpDealerName.Style.Add("display", "");
                        drpDealerName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Id"]);
                        txtCustomerName.Style.Add("display", "none");
                    }
                    else
                    {
                        drpDealerName.Style.Add("display", "none");
                        txtCustomerName.Style.Add("display", "");
                    }

                    txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Name"]);
                    txtCustCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_Code"]);
                    txtLedgerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustLedgerName"]);
                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add2"]);
                    txtAddress3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add3"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);

                    FillStateCountry();
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_ID"]);
                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Mobile"]);
                    txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone1"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                    txtContactPersonPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person_Phone"]);
                    txtPANNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PANNo"]);
                    txtTINNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TINNo"]);
                    txtcst.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                    txtST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    txtdeliveryadd1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryAdd1"]);
                    txtdeliveryadd2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryAdd2"]);
                    txtdeliveryadd3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryAdd3"]);
                    txtdeliverycity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryCity"]);
                    txtgstno.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST_No"]);
                    txtDelContactPerson.Text=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_Person"]);
                    txtDelContactPhNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Contact_phone_no"]);
                    txtDelvPin.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_pincode"]);
                    txtDelEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_E_mail"]);
                    txtDelPan.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_PANNo"]);
                    txtDelvGST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_GST_No"]);
                    
                    txtDelvName.Text=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_name"]);
                    txtDelvMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Delv_Mobile"]);

                    
                        

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
                    else { ddldeliverystate.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DeliveryState"]); }
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else
                    {
                        drpActive.SelectedValue = "2";
                    }
                    if (drpCustType.SelectedValue == "3")
                    {
                        drpCustType.Enabled = false;
                        txtCustomerName.Enabled = false;
                    }
                    else
                    {
                        drpCustType.Enabled = true;
                        txtCustomerName.Enabled = true;
                    }

                    //drpCustType.Enabled = false;
                   // txtCustomerName.Enabled = false;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    GrdCategoryDetails.DataSource = ds.Tables[1];
                    GrdCategoryDetails.DataBind();
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
        private void ClearDealerHeader()
        {
            drpCustType.SelectedValue = "0";
            txtCustomerName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtpincode.Text = "";
            txtCustCode.Text = "";
            // drpRegion.SelectedValue = "0";
            drpState.SelectedValue = "0";
            drpRegion.SelectedItem.Text = "";
            txtMobile.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtContactPerson.Text = "";
            txtContactPersonPhone.Text = "";
            txtPANNo.Text = "";
            txtTINNo.Text = "";
            txtcst.Text = "";
            txtST.Text = "";
            drpCustType.Enabled = true;
            txtdeliveryadd1.Text = "";
            txtdeliveryadd2.Text = "";
            txtdeliveryadd3.Text = "";
            txtdeliverycity.Text = "";


            txtDelContactPerson.Text = "";
            txtDelContactPhNo.Text = "";
            txtDelvPin.Text = "";
            txtDelEmail.Text = "";
            txtDelPan.Text = "";
            txtDelvGST.Text = "";

            txtDelvName.Text = "";
            txtDelvMobile.Text = "";


            ddldeliverystate.SelectedValue = "0";
            txtgstno.Text = "";
            //GrdCategoryDetails.DataSource = null;
            //GrdCategoryDetails.DataBind();
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
            clsCustomer objCustomer = new clsCustomer();
            DataSet dsRecord = new DataSet();
            //  dsRecord =objCustomer.GetCustomeRecordCount(txtCustomerName.Text,txtAddress1.Text, txtCity.Text,txtMobile.Text);

            //if (dsRecord.Tables[0].Rows.Count > 0 && txtID.Text == "")
            RowCount = (int)ViewState["GridRowCount"];

            if (RowCount > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;

                string strconfirm = "<script>if(!window.confirm('Customer is already Present?')){window.location.href='Default.aspx'}</script>";
                //  string strconfirm = "<script>confirm('Customer is already Present,you want to Create new Customer?') == true){}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);


                //   sMessage = "Customer is already Present";
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", " alert('" + sMessage + ".');", true);

                //MessageBox.Show("Customer is already Present");
                //   sMessage = "Customer is already Present,";
                //  MSG
                bValidateRecord = false;
            }

            if (bValidateRecord == false)
            {
                //  Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", " alert('" + sMessage + ".');", true);
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
                    Session["Customer"] = "New";
                    FieldEnableDisable(true);
                    ClearDealerHeader();
                    txtID.Text = "0";
                    iID = 0;

                    dsGroupCat = objCustomer.GetPartGroupMasterDts(iID);
                    if (dsGroupCat.Tables[0].Rows.Count > 0)
                    {
                        GrdCategoryDetails.DataSource = dsGroupCat.Tables[0];
                        GrdCategoryDetails.DataBind();
                    }

                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    //// if (bValidateRecord() == false) return;
                    //string flag = hdnFlag.Value;
                    //iID = Func.Convert.iConvertToInt(txtID.Text);
                    //if (flag == "Y")
                    //{
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    if (drpCustType.SelectedValue == "6" || drpCustType.SelectedValue == "4")
                    {
                        txtCustomerName.Text = Func.Convert.sConvertToString(drpDealerName.SelectedItem.Text);
                    }

                    //iID = objCustomer.bSaveCustomerDetails(iID, Func.Convert.iConvertToInt(drpCustType.SelectedValue), txtCustomerName.Text,
                    //    txtAddress1.Text, txtAddress2.Text, txtAddress3.Text, txtCity.Text, txtpincode.Text, Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                    //     Func.Convert.iConvertToInt(drpState.SelectedValue), 1, txtMobile.Text, txtPhone.Text, txtEmail.Text,
                    //    txtContactPerson.Text, txtContactPersonPhone.Text, txtPANNo.Text, txtTINNo.Text, txtcst.Text, txtST.Text, txtLBT.Text, drpActive.SelectedItem.Text,
                    //    Func.Convert.iConvertToInt(drpDealerName.SelectedValue), txtdeliveryadd1.Text, txtdeliveryadd2.Text, txtdeliveryadd3.Text, txtdeliverycity.Text, Func.Convert.iConvertToInt(ddldeliverystate.SelectedValue),txtgstno.Text.Trim());

                    iID = objCustomer.bSaveCustomerDetails(iID, Func.Convert.iConvertToInt(drpCustType.SelectedValue), txtCustomerName.Text,
                        txtAddress1.Text, txtAddress2.Text, txtAddress3.Text, txtCity.Text, txtpincode.Text, Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                         Func.Convert.iConvertToInt(drpState.SelectedValue), 1, txtMobile.Text, txtPhone.Text, txtEmail.Text,
                        txtContactPerson.Text, txtContactPersonPhone.Text, txtPANNo.Text, txtTINNo.Text, txtcst.Text, txtST.Text, txtLBT.Text, drpActive.SelectedItem.Text,
                        Func.Convert.iConvertToInt(drpDealerName.SelectedValue), txtdeliveryadd1.Text, txtdeliveryadd2.Text, txtdeliveryadd3.Text, txtdeliverycity.Text, Func.Convert.iConvertToInt(ddldeliverystate.SelectedValue), txtgstno.Text.Trim()
                        , txtDelContactPerson.Text, txtDelContactPhNo.Text, txtDelvPin.Text, txtDelEmail.Text, txtDelPan.Text, txtDelvGST.Text
                        , txtDelvName.Text, txtDelvMobile.Text
                        );

                    FillDetailsFromGrid();
                  

                    if (iID > 0)
                    {
                        if (objCustomer.bPartCaregory(iID, dtDetails) == true)
                        {
                        }
                        //FillDetailsFromGrid();
                        //if (objCustomer.bPartCaregory(iID, dtDetails) == true)
                        //{
                        if (objCustomer.bDealerWiseCustomerSave(iDealerID, iHOBr_id, iID, txtLedgerName.Text.Trim()) == true)
                        {
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        }
                        else
                        {
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                        }
                        // }
                          FillSelectionGrid();
                          GetDataAndDisplay(iID);
                    }
                    //}
                    //else
                    //{
                    //    status = (string)Session["Customer"];
                    //    if (status == "New")
                    //    {
                    //        //for (int cnt = 0; cnt < DetailsGrid.Rows.Count; cnt++)
                    //        for (int cnt = 0; cnt < 1; cnt++)
                    //        {
                    //            iID = Func.Convert.iConvertToInt((DetailsGrid.Rows[cnt].FindControl("lblID") as Label).Text);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        iID = Func.Convert.iConvertToInt(txtID.Text);
                    //    }

                    //    if (objCustomer.bDealerWiseCustomerSave(iDealerID, iHOBr_id, iID) == true)
                    //    {
                    //        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                    //        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    //    }
                    //    else
                    //    {
                    //        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    //        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    //    }
                    //}
                    //if (iID != 0)
                    //{
                    //    ds = objCustomer.GetCustomer(iID);
                    //    DisplayData();
                    //    objCustomer = null;
                    //   // FillGrid();
                    //}
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        ////private void FillGrid()
        ////{
        ////    try
        ////    {
        ////        string[] url_New = HttpContext.Current.Request.Url.AbsoluteUri.Split('=');
        ////        if (url_New.Length > 1)
        ////            if (Convert.ToInt32(url_New[1]) == 155)
        ////                SearchGrid.sModelPart = "E";
        ////            else if (Convert.ToInt32(url_New[1]) == 184)
        ////                SearchGrid.sModelPart = "D";
        ////        clsCommon objclsComman = new clsCommon();
        ////        SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", SearchGrid.sModelPart, EGpdealerID, "EGPCustomerMaster"));
        ////        objclsComman = null;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Func.Common.ProcessUnhandledException(ex);
        ////    }

        ////}
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
        private void FillCustType()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType;

                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetCustType", "C");
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
        private void FillStateCountry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //if (drpRegion.SelectedValue == "0")
                //{
                //    dsState = objDB.ExecuteQueryAndGetDataset("select ID as ID,State as Name from M_State");
                //}
                //else
                //{
                //dsState = objDB.ExecuteQueryAndGetDataset("select ID as ID,State as Name from M_State where Region_Id= " + drpRegion.SelectedValue);
                drpState.Items.Clear();
                ddldeliverystate.Items.Clear();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                if (dsState != null)
                {
                    drpState.DataSource = dsState.Tables[0];
                    drpState.DataTextField = "Name";
                    drpState.DataValueField = "ID";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new ListItem("--Select--", "0"));
                }                          
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

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FillStateCountry();
        }
        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillRegion();
        }
        private void FillDetailsFromGrid()
        {
            DataRow dr;
            dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("PartCat", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("Percentage", typeof(decimal)));
            for (int iRowCnt = 0; iRowCnt < GrdCategoryDetails.Rows.Count; iRowCnt++)
            {
                dr = dtDetails.NewRow();
                //dr["SRNo"] = "1"; 
                dr["ID"] = Func.Convert.iConvertToInt((GrdCategoryDetails.Rows[iRowCnt].FindControl("lblGroupID") as Label).Text);
                dr["PartCat"] = Func.Convert.sConvertToString((GrdCategoryDetails.Rows[iRowCnt].FindControl("lblPartCategory") as Label).Text);
                dr["Percentage"] = Func.Convert.dConvertToDouble((GrdCategoryDetails.Rows[iRowCnt].FindControl("lblPercentage") as TextBox).Text);

                dtDetails.Rows.Add(dr);
                dtDetails.AcceptChanges();
            }
           
        }
        //protected void drpCustType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (drpCustType.SelectedValue == "3")
        //    {
        //        drpCustType.SelectedValue = "0"; 
        //    }
        //}
        //protected void DetailsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    status = (string)Session["Customer"];
        //    DetailsGrid.PageIndex = e.NewPageIndex;
        //    DisplayData();
        //}
        //protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "")
        //    {
        //        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //        Label lblID = (Label)row.FindControl("lblID");

        //        iID = Func.Convert.iConvertToInt(lblID.Text);
        //        txtID.Text = Func.Convert.sConvertToString(iID);
        //        GetDataAndDisplay(iID);
        //    }
        //}
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            status = (string)Session["Customer"];
            if (status == "New")
            {
                hdnRowCount.Value = "0";
                hdnRowCountGlobal.Value = "0";
               // DisplayData_Search();
            }
        }

        protected void txtAddress1_TextChanged(object sender, EventArgs e)
        {
            status = (string)Session["Customer"];
            if (status == "New")
            {
                hdnRowCount.Value = "0";
                hdnRowCountGlobal.Value = "0";
                //DisplayData_Search();
            }

        }
        protected void txtpincode_TextChanged(object sender, EventArgs e)
        {
            status = (string)Session["Customer"];
            if (status == "New")
            {
                hdnRowCount.Value = "0";
                hdnRowCountGlobal.Value = "0";
               // DisplayData_Search();
            }

        }
        protected void txtMobile_TextChanged(object sender, EventArgs e)
        {
            status = (string)Session["Customer"];
            if (status == "New")
            {
                hdnRowCount.Value = "0";
                hdnRowCountGlobal.Value = "0";
               // DisplayData_Search();
            }
        }

        protected void drpCustType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCustType.SelectedValue == "6" || drpCustType.SelectedValue == "4")  //4 dealer and 6 Branch 
            {

                drpDealerName.Style.Add("display", "");
                txtCustomerName.Style.Add("display", "none");
                Filldealer();
            }
            else
            {
                drpDealerName.Style.Add("display", "none");
                txtCustomerName.Style.Add("display", "");
            }

        }
        private void FieldEnableDisable(bool bEnable)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                drpDealerName.Enabled = bEnable;
                txtCustomerName.Enabled = bEnable;
                txtAddress1.Enabled = bEnable;
                txtAddress2.Enabled = bEnable;
                txtAddress3.Enabled = bEnable;
                txtCity.Enabled = bEnable;
                txtpincode.Enabled = bEnable;
                txtCustCode.Enabled = false;

                drpState.Enabled = bEnable;

                drpRegion.Enabled = bEnable;
                txtCountry.Enabled = bEnable;
                txtMobile.Enabled = bEnable;
                txtPhone.Enabled = bEnable;
                txtEmail.Enabled = bEnable;
                txtContactPerson.Enabled = bEnable;
                txtContactPersonPhone.Enabled = bEnable;
                txtPANNo.Enabled = bEnable;
                txtTINNo.Enabled = bEnable;
                txtcst.Enabled = bEnable;
                txtST.Enabled = bEnable;
                txtLBT.Enabled = bEnable;
                txtdeliveryadd1.Enabled = bEnable;
                txtdeliveryadd2.Enabled = bEnable;
                txtdeliveryadd3.Enabled = bEnable;
                txtdeliverycity.Enabled = bEnable;
                ddldeliverystate.Enabled = bEnable;
                txtgstno.Enabled = bEnable;

                txtDelContactPerson.Enabled = bEnable;
                txtDelContactPhNo.Enabled = bEnable;
                txtDelvPin.Enabled = bEnable;
                txtDelEmail.Enabled = bEnable;
                txtDelPan.Enabled = bEnable;
                txtDelvGST.Enabled = bEnable;

                txtDelvName.Enabled = bEnable;
                txtDelvMobile.Enabled = bEnable;

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
        private void FilldealerAll()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsdealer;


                dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier_All", drpCustType.SelectedValue, "C", iDealerID);
                if (dsdealer != null)
                {
                    drpDealerName.DataSource = dsdealer.Tables[0];
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
        private void Filldealer()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsdealer;


                dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier", drpCustType.SelectedValue, "C", iDealerID);
                if (dsdealer != null)
                {
                    drpDealerName.DataSource = dsdealer.Tables[0];
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
        protected void drpDealerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (drpDealerName.SelectedValue != "0")
                {
                    DataSet dsdealer;


                    dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier_Details", drpDealerName.SelectedValue);
                    if (dsdealer != null)
                    {

                        //   txtCustomerName.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Customer_Name"]);
                        txtAddress1.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Address_1"]);
                        txtAddress2.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Address_2"]);
                        // txtAddress3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add3"]);
                        txtCity.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_City"]);
                        txtpincode.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Pincode"]);

                        FillStateCountry();
                        drpState.SelectedValue = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_State_ID"]);
                        FillRegion();
                        drpRegion.SelectedValue = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Region_ID"]);
                        txtCountry.Text = "India";
                        txtMobile.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Mobile"]);
                        txtPhone.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_LandLinePhone"]);
                        txtEmail.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Email"]);
                        // txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                        // txtContactPersonPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person_Phone"]);
                        txtPANNo.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["PANNo"]);
                        txtTINNo.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["TIN_No"]);
                        txtcst.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["CST"]);
                        txtST.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["VAT_No"]);
                        txtLBT.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["LBT"]);
                        txtgstno.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["GST_No"]);
                     
                        //string Active = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Active"]);
                        //if (Active == "Y")
                        //{
                        //    drpActive.SelectedValue = "1";
                        //}
                        //else
                        //{
                        //    drpActive.SelectedValue = "2";
                        //}
                        //if (drpCustType.SelectedValue == "3")
                        //{
                        //    drpCustType.Enabled = false;
                        //}
                        //else
                        //{
                        //    drpCustType.Enabled = true;
                        //}
                        FieldEnableDisable(false);
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

        //protected void drpActive_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataSet ds = new DataSet();
        //    clsDB objDB = new clsDB();
        //    string status;
        //            status = "";
        //    if (drpActive.SelectedValue=="2")
        //    {
        //        if (txtID.Text != "0")
        //        {
        //            ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCutomerPendingInfo", Func.Convert.iConvertToInt(txtID.Text), iDealerID);
        //            if (ds != null)
        //            {
        //                status = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JStatus"]);
        //                if (status.Trim() != "") Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + status + "');</script>");
        //                //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
        //                //PSelectionGrid.Style.Add("display", "");
        //                drpActive.SelectedValue = "1";
        //            }
        //        }
        //    }
        //     objDB = null;
        //        ds = null;
        //        objDB = null;
        //}


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetInvoice(int ID, int active)
         {


             DataSet ds = new DataSet();
             clsDB objDB = new clsDB();
             string status;
             status = "";

             if (active == 2)
             {

                 if (ID != 0)
                 {
                     ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCutomerPendingInfo", ID, HttpContext.Current.Session["iDealerID"]);
                     if (ds != null)
                     {
                         status = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JStatus"]);
                         if (status.Trim() != "")
                         {
                             //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                         }
                         //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                         //PSelectionGrid.Style.Add("display", "");
                         // drpActive.SelectedValue = "1";

                     }

                 }
             }
                 return status;
        }
    

    }
}