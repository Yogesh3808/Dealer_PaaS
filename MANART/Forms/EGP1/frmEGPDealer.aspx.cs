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

namespace MANART.Forms.EGP1
{
    public partial class frmEGPDealer : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        private int icustID = 0;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        private DataSet DSDealer;
        private DataSet dsState;
        private string sControlClientID = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                clsEGPDealer ObjDealer = new clsEGPDealer();
                DataSet ds = new DataSet();
                ds = ObjDealer.GetMaxEGPDealer(0);
                if (!IsPostBack)
                {
                    //FillRegion();
                    FillStateCountry();
                    //FillDealer();
                    FillYear();
                    Session["ModelDetails"] = null;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                    SearchGrid.sGridPanelTitle = "EGP Dealer Master";

                    iDealerID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iDealerID == 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            iDealerID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                        }


                }
                //if (iDealerID != 0)
                //{
                //    GetDataAndDisplay();
                //}
                FillSelectionGrid();

                ToolbarC.iValidationIdForSave = 61;
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

                lblTitle.Text = "EGP Dealer";

                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;

                }
                txtDealerName.Text = "";
                for (int k = 0; k < ChkDealer.Items.Count; k++)
                {
                    if (ChkDealer.Items[k].Selected == true)
                    {


                        txtDealerName.Text = txtDealerName.Text + ChkDealer.Items[k].Text + ",";
                    }
                }
                sControlClientID = "ContentPlaceHolder1_" + this.ID;
                txtControl_ID.Value = sControlClientID;
                txtDealerName.Attributes.Add("onclick", "SHMulSelmail123(event);");
                lnkMain.Attributes.Add("onclick", "javascript:return SHMulSel11('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");
                txtPartcatA.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                txtPartcatA.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                txtPartcatB.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                txtPartcatB.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                txtPartcatC.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                txtPartcatC.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                txtPartcatD.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                txtPartcatD.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                txtPartcatD.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
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
                iDealerID = Func.Convert.iConvertToInt(SearchGrid.iID);
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
                clsEGPDealer ObjDealer = new clsEGPDealer();
                DataSet ds = new DataSet();
                //int iDealerID = Func.Convert.iConvertToInt(txtID.Text);
                //iDealerID = 1;
                if (iDealerID != 0)
                {

                    //ds = ObjChassis.GetChassis("", "", "", "","","");
                    ds = ObjDealer.GetEGPDealer(iDealerID);
                    DisplayData(ds);
                    ObjDealer = null;
                }
                else
                {
                    ds = null;
                    //DisplayData(ds);
                    ObjDealer = null;

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
            iDealerID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();


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
                    txtEGPDealerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Name"]);
                    txtEGPDealerShortName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Short_Name"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_City"]);
                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Address1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Address2"]);
                    txtDealerMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Mobile"]);
                    txtLandLinePhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_LandLinePhone"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Email"]);
                    txtMDEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_MD_Email"]);
                    DealerOrigin = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Origin"]);
                    txtPANNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PANNo"]);
                    txtTINNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TIN_No"]);
                    txtcst.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                    txtST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT_No"]);
                    txtServiceTax.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ServiceTax"]);
                    txtfax.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Fax"]);
                    txtCountry.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Country_Name"]);
                    FillStateCountry();
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["StateID"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RegionID"]);

                    FillDealer();
                    txtDealerName.Text = "";
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        ListItem myListItem = ChkDealer.Items.FindByValue((ds.Tables[0].Rows[k]["DistributorID"].ToString()));
                        if (myListItem != null)
                        {
                            myListItem.Selected = true;
                            txtDealerName.Text = txtDealerName.Text + (ds.Tables[0].Rows[k]["DistributorName"].ToString()) + ",";
                        }
                    }

                    drpStartYear.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["fin_no"]);
                    txtstartYear.Text = "01/04/" + ds.Tables[0].Rows[0]["Year1"];
                    txtEndYear.Text = "31/03/" + ds.Tables[0].Rows[0]["Year2"];
                    string HOBranch = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HOBranch"]);
                    if (HOBranch == "00")
                    {
                        drpHOBranch.SelectedValue = "1";
                        lblHO.Visible = false;
                        drpHO.Visible = false;
                    }
                    else
                    {
                        drpHOBranch.SelectedValue = "2";
                        FillHO();
                        drpHO.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HO"]);
                        lblHO.Visible = true;
                        drpHO.Visible = true;
                    }
                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Active"]);
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else
                    {
                        drpActive.SelectedValue = "2";
                    }
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
                    txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);

                    txtPartcatA.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartACatDisc"]);
                    txtPartcatB.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartBCatDisc"]);
                    txtPartcatC.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartCCatDisc"]);
                    txtPartcatD.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartDCatDisc"]);
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
            txtEGPDealerName.Text = "";
            txtEGPDealerShortName.Text = "";
            txtCity.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtDealerMobile.Text = "";
            txtLandLinePhone.Text = "";
            txtEmail.Text = "";
            txtMDEmail.Text = "";
            // drpActive.SelectedValue = "0";
            drpState.SelectedValue = "0";
            // drpRegion.SelectedValue = "0";
            drpRegion.SelectedItem.Text = "";
            txtPANNo.Text = "";
            txtTINNo.Text = "";
            txtcst.Text = "";
            txtST.Text = "";
            txtServiceTax.Text = "";
            drpStartYear.SelectedValue = "0";
            txtstartYear.Text = "";
            txtEndYear.Text = "";
            drpHOBranch.Text = "1";
            lblHO.Visible = false;
            drpHO.Visible = false;
            txtDealerName.Text = "";
            txtfax.Text = "";
            txtpincode.Text = "";
            txtContactPerson.Text = "";
            txtPartcatA.Text = "0.00";
            txtPartcatB.Text = "0.00";
            txtPartcatC.Text = "0.00";
            txtPartcatD.Text = "0.00";

            for (var i = 0; i < ChkDealer.Items.Count; i++)
            {
                ChkDealer.Items[i].Selected = false;

            }
            ChkAll.Text = "Select";

            FillDealer();
            FillCurrentYear();
            FillStartYearDate();


        }
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("EGP Dealer Name");
                SearchGrid.AddToSearchCombo("EGP Dealer Code");
                SearchGrid.AddToSearchCombo("City");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("Region");
                //SearchGrid.AddToSearchCombo("Country");
                SearchGrid.iDealerID = -1;
                SearchGrid.sSqlFor = "EGPDealer";
                SearchGrid.sGridPanelTitle = "EGP Dealer List";
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
            int l = 0;
            for (int k = 0; k < ChkDealer.Items.Count; k++)
            {
                if (ChkDealer.Items[k].Selected)
                {


                    l = l + 1;
                }

            }
            if (txtPartcatA.Text == "")
            {
                txtPartcatA.Text = "0.00";
            }
            if (txtPartcatB.Text == "")
            {
                txtPartcatB.Text = "0.00";
            }
            if (txtPartcatC.Text == "")
            {
                txtPartcatC.Text = "0.00";
            }
            if (txtPartcatD.Text == "")
            {
                txtPartcatD.Text = "0.00";
            }

            if (l == 0)
            {

                sMessage = sMessage + "Please Select Distributor.";
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

                clsEGPDealer objdealer = new clsEGPDealer();
                clsEGPCustomer objCustomer = new clsEGPCustomer();

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    ClearDealerHeader();
                    txtID.Text = "0";
                    iID = 0;


                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);



                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord() == false) return;

                    int j = 0;
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    iID = objdealer.bSaveEGPDealerDetails(iID, txtEGPDealerName.Text.Trim(), txtEGPDealerShortName.Text, txtCity.Text, txtAddress1.Text, txtAddress2.Text, txtDealerMobile.Text, txtLandLinePhone.Text, txtEmail.Text
                        , txtMDEmail.Text, "D", drpActive.SelectedItem.Text, txtTINNo.Text, txtcst.Text, txtST.Text, txtServiceTax.Text, txtPANNo.Text, txtfax.Text, 1, Func.Convert.iConvertToInt(drpRegion.SelectedValue), Func.Convert.iConvertToInt(drpState.SelectedValue), "Y", txtpincode.Text, txtContactPerson.Text);

                    if (iID > 0)
                    {
                        icustID = objCustomer.bSaveEGPCustomerDetails(icustID, iID, 3, "Cash Sale",
                            txtAddress1.Text, txtAddress2.Text, "", txtCity.Text, txtpincode.Text, Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                      Func.Convert.iConvertToInt(drpState.SelectedValue), 1, txtDealerMobile.Text, txtLandLinePhone.Text, txtEmail.Text,
                     txtContactPerson.Text, "", txtPANNo.Text, txtTINNo.Text, txtcst.Text, txtST.Text, drpActive.SelectedItem.Text);

                        if (objdealer.bSaveEGPHOBranch(iID, Func.Convert.iConvertToInt(drpHOBranch.SelectedValue), Func.Convert.iConvertToInt(drpHO.SelectedValue), drpStartYear.SelectedValue) == false) return;

                        for (int k = 0; k < ChkDealer.Items.Count; k++)
                        {
                            if (ChkDealer.Items[k].Selected)
                            {

                                if (objdealer.bSaveEGPDistributor(iID, Func.Convert.iConvertToInt(ChkDealer.Items[k].Value)) == false) return;
                                j = j + 1;
                            }

                        }
                    }
                    if (j >= 1 && (iID > 0))
                    {
                        ds = objdealer.GetEGPLoginname(iID);
                        string LoginName = "";
                        LoginName = "EGP Dealer Created with LoginName : " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_EGP_Code"]);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + LoginName + ".');</script>");

                        // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                    }

                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                    FillSelectionGrid();

                }
                if (iID != 0)
                {
                    ds = objdealer.GetEGPDealer(iID);
                    DisplayData(ds);
                    objdealer = null;
                    FillGrid();
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillGrid()
        {
            try
            {
                clsCommon objclsComman = new clsCommon();
                SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", "", -1, "EGPDealer"));
                objclsComman = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillDealer()
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                // DSDealer = objDB.ExecuteQueryAndGetDataset("select Id as ID,Dealer_Name  as Name  from M_dealer where Dealer_Type_Id=2 and Dealer_State_Id= " + drpState.SelectedValue + "" );
                ChkDealer.Items.Clear();
                DSDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpState.SelectedValue, "Distributor");
                if (DSDealer != null)
                {
                    ChkDealer.DataSource = DSDealer.Tables[0];
                    ChkDealer.DataTextField = "Name";
                    ChkDealer.DataValueField = "ID";
                    ChkDealer.DataBind();

                }



                for (int k = 0; k < ChkDealer.Items.Count; k++)
                {
                    // ChkDealer.Items[k].Attributes.Add("onclick", "SCIT(this,'" + sControlClientID + "')");
                    // ChkDealer.Items[k].Attributes.Add("onclick", "SCIT(this,'" + txtControl_ID.Value + "')");
                    //ChkDealer.Items[k].Attributes.Add("onclick", "SCIT1123(this)");
                    ChkDealer.Items[k].Attributes.Add("onclick", "SCIT1123(this)");
                    //ChkDealer.Items[k].Attributes.Add("onclick", "SCIT1123()");
                    ChkDealer.Items[k].Attributes.Add("alt", ChkDealer.Items[k].Value);
                    ChkDealer.Items[k].Attributes.Add("runat", "server");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }


        protected void drpHOBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpHOBranch.SelectedValue == "2")
            {
                lblHO.Visible = true;
                drpHO.Visible = true;

                FillHO();
            }
            else
            {
                lblHO.Visible = false;
                drpHO.Visible = false;
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
        private void FillHO()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsHO;

                dsHO = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", drpState.SelectedValue, "HO");
                if (dsHO != null)
                {
                    drpHO.DataSource = dsHO.Tables[0];
                    drpHO.DataTextField = "Name";
                    drpHO.DataValueField = "ID";
                    drpHO.DataBind();
                    drpHO.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillYear()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsYear;

                //dsYear = objDB.ExecuteQueryAndGetDataset("select fin_no as ID,MISyear as Name from M_MISyear");
                dsYear = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "Year");
                if (dsYear != null)
                {
                    drpStartYear.DataSource = dsYear.Tables[0];
                    drpStartYear.DataTextField = "Name";
                    drpStartYear.DataValueField = "ID";
                    drpStartYear.DataBind();
                    drpStartYear.Items.Insert(0, new ListItem("--Select--", "0"));
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

        private void FillCurrentYear()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCurrentYear;

                //dsYear = objDB.ExecuteQueryAndGetDataset("select fin_no as ID,MISyear as Name from M_MISyear");
                dsCurrentYear = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "CurrentYear");
                if (dsCurrentYear != null)
                {
                    drpStartYear.SelectedValue = Func.Convert.sConvertToString(dsCurrentYear.Tables[0].Rows[0]["fin_no"]);
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
        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealer();
            FillRegion();
        }
        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FillStateCountry();
        }
        protected void drpStartYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStartYearDate();

        }
        private void FillStartYearDate()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsYeardetails;

                dsYeardetails = objDB.ExecuteQueryAndGetDataset("select Year1,Year2 from M_MISyear where fin_no= " + drpStartYear.SelectedValue + "");
                if (dsYeardetails != null)
                {
                    txtstartYear.Text = "01/04/" + dsYeardetails.Tables[0].Rows[0]["Year1"];
                    txtEndYear.Text = "31/03/" + dsYeardetails.Tables[0].Rows[0]["Year2"];
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