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
    public partial class frmDealer : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.Visible = false;
                clsDealer ObjDealer = new clsDealer();
                DataSet ds = new DataSet();
                ds = ObjDealer.GetMaxDealer(0);
                ReadOnlyProperty(true);
                if (!IsPostBack)
                {
                    Session["ModelDetails"] = null;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                }
                SearchGrid.sGridPanelTitle = "Dealer Master";
                FillSelectionGrid();
                //iDealerID = Func.Convert.iConvertToInt(txtID.Text);
                //if (iDealerID == 0)
                //    iDealerID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                //if (iDealerID != 0)
                //{
                //    GetDataAndDisplay();
                //}
                SetDocumentDetails();
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
                //Sujata 15032011
                lblTitle.Text = "Dealer";
                //Sujata 15032011
                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                    //Sujata 24082011
                    Func.Common.BindDataToCombo(DrpDealerType, clsCommon.ComboQueryType.DealerType, 0);                    
                    //Sujata 24082011
                }
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
        private void SetDocumentDetails()
        {
            //lblTitle.Text = " Chassis Header Details ";
            //lblDocNo.Text = "RFP No.:";
            //lblDocDate.Text = "RFP Date:";              
        }
        private void GetDataAndDisplay()
        {
            try
            {
                clsDealer ObjDealer = new clsDealer();
                DataSet ds = new DataSet();
                //int iDealerID = Func.Convert.iConvertToInt(txtID.Text);
                //iDealerID = 1;
                if (iDealerID != 0)
                {

                    //ds = ObjChassis.GetChassis("", "", "", "","","");
                    ds = ObjDealer.GetDealer(iDealerID);
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

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iDealerID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
            ReadOnlyProperty(true);

        }
        private void ReadOnlyProperty(bool Flag)
        {
            if (Flag == true)
            {
                txtDealerName.ReadOnly = true;
                txtDealerName.Enabled = false;
                txtHierarchyCode.ReadOnly = true;
                txtHierarchyCode.Enabled = false;
                txtSparesCode.ReadOnly = true;
                txtSparesCode.Enabled = false;
                txtVehicleCode.ReadOnly = true;
                txtVehicleCode.Enabled = false;
                txtHDCode.ReadOnly = true;
                txtHDCode.Enabled = false;
                txtHOCode.ReadOnly = true;
                txtHOCode.Enabled = false;
                txtDealerOrigin.Enabled = false;
                txtBusCode.ReadOnly = true;
                txtBusCode.Enabled = false;
                txtServiceTaxType.ReadOnly = true;
                txtServiceTaxType.Enabled = false;
                txtRemanCode.ReadOnly = true;
                txtRemanCode.Enabled = false;
                // New Code Vikram
                DrpDealerType.Enabled = false;
                txtTINNo.Enabled = false;
                txtVATNo.Enabled = false;
                txtLVANo.Enabled = false;
                txtPANNo.Enabled = false;
                drpDealerRepRegion.Enabled = false;
                txtIRCNo.Enabled = false;

            }
            else
            {
                txtDealerName.ReadOnly = false;
                txtDealerName.Enabled = true;
                txtHierarchyCode.ReadOnly = false;
                txtHierarchyCode.Enabled = true;
                txtSparesCode.ReadOnly = false;
                txtSparesCode.Enabled = true;
                txtVehicleCode.ReadOnly = false;
                txtVehicleCode.Enabled = true;
                txtHDCode.ReadOnly = false;
                txtHDCode.Enabled = true;
                txtHOCode.ReadOnly = false;
                txtHOCode.Enabled = true;
                txtDealerOrigin.Enabled = true;
                txtBusCode.ReadOnly = false;
                txtBusCode.Enabled = true;
                txtServiceTaxType.ReadOnly = false;
                txtServiceTaxType.Enabled = true;
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
                    txtDealerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Name"]);
                    txtDealerShortName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Short_Name"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_City"]);
                    txtAddress.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Address"]);
                    txtDealerMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Mobile"]);
                    txtLandLinePhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_LandLinePhone"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Email"]);
                    txtMDEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_MD_Email"]);
                    DealerOrigin = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Origin"]);
                    if (DealerOrigin == "E")
                    {
                        txtDealerOrigin.Text = "Export";
                        //Sujata 24082011
                        Func.Common.BindDataToCombo(drpDistributor, clsCommon.ComboQueryType.DealerAll, 0, " and Dealer_Type_ID=2 and Dealer_Origin='E'");
                        //Sujata 24082011
                    }
                    else
                    {
                        txtDealerOrigin.Text = "Domestic";
                        //Sujata 24082011
                        Func.Common.BindDataToCombo(drpDistributor, clsCommon.ComboQueryType.DealerAll, 0, " and Dealer_Type_ID=2 and Dealer_Origin='D'");
                        //Sujata 24082011
                    }
                    //Sujata 03102013
                    Func.Common.BindDataToCombo(drpDealerRepRegion, clsCommon.ComboQueryType.RegionUserWise, 0, (DealerOrigin == "E") ? " and Domestic_Export='E'" : " and Domestic_Export='D'");
                    drpDealerRepRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Report_Region_ID"]);
                    txtPANNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PANNo"]);
                    //Sujata 03102013                

                    txtSalesOffice.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_sales_Office"]);
                    txtDealerTerritory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Territory"]);
                    txtSparesCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Spares_Code"]);
                    txtVehicleCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Vehicle_Code"]);
                    txtHDCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_HD_Code"]);
                    txtExtendedWarr.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Extended_warr"]);
                    txtHOBranch.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_HO_Branch"]);
                    txtHOCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_HO_Code"]);
                    txtActive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Active"]);
                    //txtTINNo.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["TIN_No"]).ToString();
                    //txtVATNo.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["VAT_No"]).ToString();
                    txtTINNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TIN_No"]);
                    txtVATNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT_No"]);
                    txtLVANo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LVA_No"]);
                    txtIRCNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IRC_No"]);
                    txtCountry.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Country_Name"]);
                    txtState.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["State"]);
                    txtDistrict.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Distict_Name"]);
                    //sujata 24082011
                    //txtDealerType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Type_Des"]);
                    DrpDealerType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Type_ID"]);
                    //sujata 24082011
                    txtDealerCategory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Category_Des"]);
                    txtDealerRegion.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_Name"]);
                    txtDealerDepot.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Depo_name"]);
                    txtHierarchyCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Hierarchy_code"]);
                    //Sujata 24082011
                    drpDistributor.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DistributorID"]);
                    drpDistributor.Enabled = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Type_Des"]) == "Dealer Under Distributor") ? true : false;
                    //Sujata 24082011
                    //Megha 15042013 add bus code
                    txtBusCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bus_Code"]);
                    //Megha 15042013
                    //Megha 24072013 add TAXCodeDescr
                    txtServiceTaxType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TAXCodeDescr"]);
                    //Megha 24072013
                    //poonam add reman code 
                    txtRemanCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Reman_Code"]);
                    //Dealer Live 
                    txtDealerLive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DlrLive"]);
                    txtDealerLiveDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DlrLiveDt"]);
                    clsCommon objCommon = null;            
                    objCommon = new clsCommon();
                    txtDlrStateID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_State_ID"]);
                    txtDistrictID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_district_ID"]);
                    Func.Common.BindDataToCombo(DrpDistrict, clsCommon.ComboQueryType.DistrictList, 0, " and (Id ='" + txtDistrictID.Text.Trim() + "' or (Active='Y' and State_ID='" + txtDlrStateID.Text.Trim() + "'))");
                    DrpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_district_ID"]);
                    btnGoLive.Visible = (txtDealerLive.Text.Trim() == "No" && objCommon.sUserRole == "8" && Func.Convert.iConvertToInt(Session["UserID"]) == 1) ? true : false;

                    Func.Common.BindDataToCombo(DrpWarehouse, clsCommon.ComboQueryType.WarehouseList, 0, " and (ID ='" + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warehouse_ID"]).Trim()  + "' or Active='Y')");
                    DrpWarehouse.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warehouse_ID"]);   
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
            txtDealerName.Text = "";
            txtDealerShortName.Text = "";
            txtCity.Text = "";
            txtAddress.Text = "";
            txtDealerMobile.Text = "";
            txtLandLinePhone.Text = "";
            txtEmail.Text = "";
            txtMDEmail.Text = "";
            txtDealerOrigin.Text = "";
            txtSalesOffice.Text = "";
            txtDealerTerritory.Text = "";
            txtSparesCode.Text = "";
            txtVehicleCode.Text = "";
            txtHDCode.Text = "";
            txtExtendedWarr.Text = "";
            txtHOBranch.Text = "";
            txtHOCode.Text = "";
            txtActive.Text = "";
            txtTINNo.Text = "";
            txtVATNo.Text = "";
            txtLVANo.Text = "";
            txtIRCNo.Text = "";
            txtCountry.Text = "";
            txtState.Text = "";
            txtDistrict.Text = "";
            //Sujata 24082011
            //txtDealerType.Text = "";
            DrpDealerType.SelectedValue = Func.Convert.sConvertToString(0);
            //Sujata 24082011
            txtDealerCategory.Text = "";
            txtDealerRegion.Text = "";
            txtDealerDepot.Text = "";
            txtBusCode.Text = "";
            txtServiceTaxType.Text = "";
            //poonam
            txtRemanCode.Text = "";
        }
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Dealer Name");
                SearchGrid.AddToSearchCombo("Dealer Code");
                //SearchGrid.AddToSearchCombo("Vehicle Code");
                //SearchGrid.AddToSearchCombo("Spares Code");
                //SearchGrid.AddToSearchCombo("BUS Code");
                //SearchGrid.AddToSearchCombo("Reg No");
                SearchGrid.AddToSearchCombo("City");
                SearchGrid.AddToSearchCombo("Country");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("District");
                SearchGrid.AddToSearchCombo("Address");
                //SearchGrid.AddToSearchCombo("Reman Code");
                string[] url = HttpContext.Current.Request.Url.AbsoluteUri.Split('=');
                if (url.Length > 1)
                    if (Convert.ToInt32(url[1]) == 155)
                        SearchGrid.sModelPart = "E";
                    else if (Convert.ToInt32(url[1]) == 184)
                        SearchGrid.sModelPart = "D";
                SearchGrid.iDealerID = -1;
                SearchGrid.sSqlFor = "Dealer";
                SearchGrid.sGridPanelTitle = "Dealer List";
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
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;

                clsDealer objdealer = new clsDealer();
                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                    txtID.Text = "0";
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ClearDealerHeader();
                    ReadOnlyProperty(false);

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (txtDealerName.Text == "")
                    {
                        sMessage = "Please Enter Dealer Name";
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        return;
                    }
                    //Sujata 24082011
                    if (Func.Convert.iConvertToInt(DrpDealerType.SelectedValue) == 3 && Func.Convert.iConvertToInt(drpDistributor.SelectedValue) == 0)
                    {
                        sMessage = "Please Select Distributor.";
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        return;
                    }
                    //Sujata 24082011
                    if (txtDealerOrigin.Text == "Export")
                    {
                        DOrigin = "E";

                    }
                    else
                    {
                        DOrigin = "D";
                    }
                    if (DOrigin == "E")
                    {
                        //if (txtCity.Text != "")
                        //{
                        //    sMessage = "City must be Blank"; 
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        //    return;
                        //}
                        if (txtState.Text != "")
                        {
                            sMessage = "State must be Blank";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            return;
                        }

                        if (txtDistrict.Text != "")
                        {
                            sMessage = "District must be Blank";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            return;
                        }
                    }



                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    //Sujata 24082011
                    //iID = objdealer.bSaveDealerDetails(iID, txtDealerName.Text, txtDealerShortName.Text, txtCity.Text, txtAddress.Text,
                    //txtDealerMobile.Text, txtLandLinePhone.Text, txtEmail.Text, txtMDEmail.Text, txtDealerOrigin.Text, txtSalesOffice.Text,
                    //txtDealerTerritory.Text, txtSparesCode.Text, txtVehicleCode.Text, txtHDCode.Text, txtExtendedWarr.Text, txtHOBranch.Text,
                    //txtHOCode.Text, txtActive.Text, txtTINNo.Text, txtVATNo.Text, txtLVANo.Text, txtIRCNo.Text, txtCountry.Text, txtState.Text,
                    //txtDistrict.Text, txtDealerType.Text, txtDealerCategory.Text, txtDealerRegion.Text, txtDealerDepot.Text, txtHierarchyCode.Text);
                    //Sujata 03102013 // Add Report Region ID
                    iID = objdealer.bSaveDealerDetails(iID, txtDealerName.Text, txtDealerShortName.Text, txtCity.Text, txtAddress.Text,
                        txtDealerMobile.Text, txtLandLinePhone.Text, txtEmail.Text, txtMDEmail.Text, txtDealerOrigin.Text, txtSalesOffice.Text,
                        txtDealerTerritory.Text, txtSparesCode.Text, txtVehicleCode.Text, txtHDCode.Text, txtExtendedWarr.Text, txtHOBranch.Text,
                        txtHOCode.Text, txtActive.Text, txtTINNo.Text, txtVATNo.Text, txtLVANo.Text, txtIRCNo.Text, txtCountry.Text, txtState.Text,
                        txtDistrict.Text, Func.Convert.sConvertToString(DrpDealerType.SelectedItem), txtDealerCategory.Text, txtDealerRegion.Text,
                        txtDealerDepot.Text, txtHierarchyCode.Text, Func.Convert.iConvertToInt(drpDistributor.SelectedValue), txtBusCode.Text,
                        txtServiceTaxType.Text, Func.Convert.iConvertToInt(drpDealerRepRegion.SelectedValue), txtPANNo.Text, txtRemanCode.Text);
                    //Sujata 24082011

                    //iID = objdealer.bSaveDealerDetails(iID, txtDealerName.Text,txtVehicleCode.Text,txtSparesCode.Text,txtHDCode.Text,txtHierarchyCode.Text,
                    //    txtAddress.Text,txtCity.Text,txtDistrict.Text,txtState.Text,txtDealerDepot.Text,txtCountry.Text,txtDealerRegion.Text,txtDealerMobile.Text,
                    //    txtLandLinePhone.Text,txtEmail.Text,txtMDEmail.Text,txtDealerType.Text,txtDealerCategory.Text,txtDealerOrigin.Text,txtSalesOffice.Text,
                    //    txtExtendedWarr.Text,txtHOBranch.Text,txtHOCode.Text,txtTINNo.Text,txtVATNo.Text,txtLVANo.Text,txtIRCNo.Text,     


                    if (iID > 0)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                    //  FillSelectionGrid();

                }
                if (iID != 0)
                {
                    ds = objdealer.GetDealer(iID);
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
                string[] url_New = HttpContext.Current.Request.Url.AbsoluteUri.Split('=');
                if (url_New.Length > 1)
                    if (Convert.ToInt32(url_New[1]) == 155)
                        SearchGrid.sModelPart = "E";
                    else if (Convert.ToInt32(url_New[1]) == 184)
                        SearchGrid.sModelPart = "D";
                clsCommon objclsComman = new clsCommon();
                SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", SearchGrid.sModelPart, -1, "Dealer"));
                objclsComman = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void DrpDealerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpDistributor.Enabled = (Func.Convert.iConvertToInt(DrpDealerType.SelectedValue) == Func.Convert.iConvertToInt(3)) ? true : false;
            drpDistributor.SelectedValue = drpDistributor.Enabled == false ? Func.Convert.sConvertToString(0) : drpDistributor.SelectedValue;
        }

        protected void btnGoLive_Click(object sender, EventArgs e)
        {
            //Dealer Live 
            clsDealer objdealer = new clsDealer();
            DataSet ds = new DataSet();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            iID = objdealer.bDealerLive(iID, Func.Convert.iConvertToInt(DrpDistrict.SelectedValue), Func.Convert.iConvertToInt(DrpWarehouse.SelectedValue));           

            if (iID > 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Live. Now You can create dealer user. Upload Stock and customer supplier list for this dealer.');</script>");

                ds = objdealer.GetDealer(iID);
                DisplayData(ds);
                objdealer = null;
                FillGrid();
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer Not Live. Please Contact to Administrator.');</script>");
            }
        }
    }
}