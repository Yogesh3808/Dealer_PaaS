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
    public partial class frmSupplierMaster : System.Web.UI.Page
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
        int iHOBr_id = 0;
        private int EGpdealerID = 0;
        string Flag = "";
        string suppliercode = "";
        int iSup_type=0;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                clsSupplier ObjSup = new clsSupplier();
                DataSet ds = new DataSet();
                //  ds = ObjSup.GetMaxSupplier();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iID = Func.Convert.iConvertToInt(txtID.Text);

                ds = ObjSup.GetMaxSupplier(iHOBr_id, iDealerID);

                if (!IsPostBack)
                {
                    //  FillRegion();
                    FillStateCountry();

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                }


                iID = Func.Convert.iConvertToInt(txtID.Text);
                if (iID == 0)
                    iID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                if (iID != 0)
                {
                    GetDataAndDisplay();

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
                lblTitle.Text = "Supplier";
                if (!IsPostBack)
                {
                    FillCustType();
                    //  SearchGrid.bIsCollapsable = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCustType()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType;
                // Created By Vikram on 23.06.16
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetCustTypeForSupplier", "S");
                if (dsCustType != null)
                {
                    drpSupType.DataSource = dsCustType.Tables[0];
                    drpSupType.DataTextField = "Name";
                    drpSupType.DataValueField = "ID";
                    drpSupType.DataBind();
                    drpSupType.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillSelectionGrid()
        {
            try
            {
                // SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("City");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("Region");
                SearchGrid.AddToSearchCombo("Country");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "SupplierMaster";
                SearchGrid.iBrHODealerID = iHOBr_id;
                SearchGrid.sGridPanelTitle = "Supplier Master List";
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
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // iDealerID = Func.Convert.iConvertToInt(SearchGrid.iID);
                GetDataAndDisplay();
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

        private void GetDataAndDisplay()
        {
            try
            {
                clsSupplier ObjDealer = new clsSupplier();
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = ObjDealer.GetSupplier(iID);
                    DisplayData(ds);
                    ObjDealer = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    ObjDealer = null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FilldealerAll()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsdealer;


                dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier_All", drpSupType.SelectedValue, "S", iDealerID);
                if (dsdealer != null)
                {
                    drpSupplierName.DataSource = dsdealer.Tables[0];
                    drpSupplierName.DataTextField = "Name";
                    drpSupplierName.DataValueField = "ID";
                    drpSupplierName.DataBind();
                    drpSupplierName.Items.Insert(0, new ListItem("--Select--", "0"));
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


                dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier", drpSupType.SelectedValue, "S", iDealerID);
                if (dsdealer != null)
                {
                    drpSupplierName.DataSource = dsdealer.Tables[0];
                    drpSupplierName.DataTextField = "Name";
                    drpSupplierName.DataValueField = "ID";
                    drpSupplierName.DataBind();
                    drpSupplierName.Items.Insert(0, new ListItem("--Select--", "0"));
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
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }
                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    drpSupType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sup_Type"]);
                    txtSupplierName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Supplier_Name"]);
                    if (drpSupType.SelectedValue == "16" || drpSupType.SelectedValue == "17")
                    {
                        FieldEnableDisable(false);
                       // Filldealer();
                        FilldealerAll();
                        drpSupplierName.Style.Add("display", "");
                        drpSupplierName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Type_Dealer_ID"]);
                        txtSupplierName.Style.Add("display", "none");
                        txtGSTIn.Enabled = false;
                    }
                    else
                    {
                        FieldEnableDisable(true);
                        drpSupplierName.Style.Add("display", "none");
                        txtSupplierName.Style.Add("display", "");
                        txtGSTIn.Enabled = true;
                    }

                    SupType_Enabledisable(false);

                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add2"]);
                    // txtAddress3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add3"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["city"]);
                    //  txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);

                    FillStateCountry();
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_ID"]);
                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone2"]);
                    txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone1"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    // txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                    // txtContactPersonPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person_Phone"]);
                    txtPANNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PANNo"]);
                    txtTINNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TINNo"]);
                    txtcst.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                    txtST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                    txtGSTIn.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST_IN"]);
                    txtLedgerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SuppLedgerName"]);
                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else
                    {
                        drpActive.SelectedValue = "2";
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
        private void ClearDealerHeader()
        {
            drpSupType.SelectedValue = "0";
            drpSupplierName.SelectedValue = "0";
            drpSupplierName.Style.Add("display", "none");
            txtSupplierName.Style.Add("display", "");  
            txtSupplierName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCity.Text = "";
            drpState.SelectedValue = "0";
            drpRegion.SelectedItem.Text = "";
            txtMobile.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtPANNo.Text = "";
            txtTINNo.Text = "";
            txtcst.Text = "";
            txtST.Text = "";
            txtGSTIn.Text = "";
            drpActive.SelectedValue = "1";
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
            if (drpSupType.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Supplier Type.";
                bValidateRecord = false;
            }
            else if (txtMobile.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Mobile.";
                bValidateRecord = false;
            }
            else if (txtAddress1.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Address.";
                bValidateRecord = false;
            }
            else if (drpState.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the State.";
                bValidateRecord = false;
            }
            if (drpSupType.SelectedValue == "16" || drpSupType.SelectedValue == "17")
            {
                if (drpSupplierName.SelectedValue == "0")
                {
                    sMessage = sMessage + "\\n Please select the Supplier.";
                    bValidateRecord = false;
                }
            }
            else
            {
                if (txtSupplierName.Text == "")
                  {
                 sMessage = sMessage + "\\n Please Enter the Supplier.";
                 bValidateRecord = false;
                  }
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
                clsSupplier objSupplier = new clsSupplier();
                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    ClearDealerHeader();
                    SupType_Enabledisable(true);
                    txtID.Text = "0";
                    iID = 0;
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                     if (bValidateRecord() == false) return;
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iID == 0)
                    {
                        DataSet dsDealerCode;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                  dsDealerCode = objSupplier.GetDealerCode(iDealerID);
                  if (dsDealerCode != null)
                  {
                      txtDealerCode.Text = Func.Convert.sConvertToString(dsDealerCode.Tables[0].Rows[0]["DealerCode"]);
                  }

                        txtRecordUsed.Text = objSupplier.sGetMaxDocNo("SPL", iDealerID,Func.Convert.sConvertToString(txtDealerCode.Text));
                }
                    if (drpSupType.SelectedValue == "16" || drpSupType.SelectedValue == "17")
                    {
                        txtSupplierName.Text = Func.Convert.sConvertToString(drpSupplierName.SelectedItem.Text);
                    }
                    iID = objSupplier.bSaveSupplierDetails(iID, iDealerID, iHOBr_id, txtSupplierName.Text, 
                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                         Func.Convert.iConvertToInt(drpState.SelectedValue), 1, txtMobile.Text, txtPhone.Text, txtEmail.Text, Func.Convert.iConvertToInt(drpSupType.SelectedValue),
                         txtPANNo.Text, txtTINNo.Text, txtcst.Text, txtST.Text, drpActive.SelectedItem.Text, txtRecordUsed.Text, Func.Convert.iConvertToInt(drpSupplierName.SelectedValue),
                         txtGSTIn.Text.Trim(), txtLedgerName.Text.Trim());
                    if (iID > 0)
                    {
                        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);

                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }
                    txtID.Text = Func.Convert.sConvertToString(iID);
                    FillSelectionGrid();

                }
                objSupplier = null;
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

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillStateCountry();
        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillRegion();
        }
        protected void drpSupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            iSup_type = Func.Convert.iConvertToInt(drpSupType.SelectedValue);
            if (drpSupType.SelectedValue == "16" || drpSupType.SelectedValue == "17") // 16-Branch and 17-Dealer
            {

                drpSupplierName.Style.Add("display", "");
                txtSupplierName.Style.Add("display", "none");
                Filldealer();
                FieldEnableDisable(false);
                txtGSTIn.Enabled = false;
            }
            else
            {
                drpSupplierName.Style.Add("display", "none");
                txtSupplierName.Style.Add("display", "");
                FieldEnableDisable(true);
                txtGSTIn.Enabled = true;
            }
        }

        private void FieldEnableDisable(bool bEnable)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                txtSupplierName.Enabled = bEnable;
                txtAddress1.Enabled = bEnable;
                txtAddress2.Enabled = bEnable;

                txtCity.Enabled = bEnable;



                drpState.Enabled = bEnable;

                drpRegion.Enabled = bEnable;
                txtCountry.Enabled = bEnable;
                txtMobile.Enabled = bEnable;
                txtPhone.Enabled = bEnable;
                txtEmail.Enabled = bEnable;

                txtPANNo.Enabled = bEnable;
                txtTINNo.Enabled = bEnable;
                txtcst.Enabled = bEnable;
                txtST.Enabled = bEnable;
                
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
        private void SupType_Enabledisable(bool bEnable)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                drpSupType.Enabled = true;
                drpActive.Enabled = true;
                if (drpSupType.SelectedValue == "18")
                {
                    drpSupType.Enabled = bEnable; 
                    txtSupplierName.Enabled = bEnable;
                    txtAddress1.Enabled = bEnable;
                    txtAddress2.Enabled = bEnable;
                    txtCity.Enabled = bEnable;
                    drpState.Enabled = bEnable;
                    drpRegion.Enabled = bEnable;
                    txtCountry.Enabled = bEnable;
                    txtMobile.Enabled = bEnable;
                    txtPhone.Enabled = bEnable;
                    txtEmail.Enabled = bEnable;
                    txtPANNo.Enabled = bEnable;
                    txtTINNo.Enabled = bEnable;
                    txtcst.Enabled = bEnable;
                    txtST.Enabled = bEnable;
                    drpActive.Enabled = bEnable;
                }
                else
                {
                    drpSupType.Enabled = bEnable; 
                    txtSupplierName.Enabled = bEnable;
                    drpSupplierName.Enabled = bEnable;
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
        protected void drpSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (drpSupplierName.SelectedValue != "0")
                {
                    DataSet dsdealer;


                    dsdealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealer_CustomerSupplier_Details", drpSupplierName.SelectedValue);
                    if (dsdealer != null)
                    {

                        //   txtCustomerName.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Customer_Name"]);
                        txtAddress1.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Address_1"]);
                        txtAddress2.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Address_2"]);
                        // txtAddress3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add3"]);
                        txtCity.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_City"]);
                       

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
                        txtGSTIn.Text = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["GST_NO"]);
                        string Active = Func.Convert.sConvertToString(dsdealer.Tables[0].Rows[0]["Dealer_Active"]);
                        if (Active == "Y")
                        {
                            drpActive.SelectedValue = "1";
                        }
                        else
                        {
                            drpActive.SelectedValue = "2";
                        }

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
    }
}