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
    public partial class frmLeadMaster : System.Web.UI.Page
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
        int iHOBrId = 0;
        private int EGpdealerID = 0;
        private int HOBrID = 0;

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
                if (iUserId != 0)
                {
                    //dsDealer = ObjCust.GetLeadDealerID(iUserId);

                    //if (dsDealer.Tables[0].Rows.Count > 0)
                    //{
                    //    EGpdealerID = Func.Convert.iConvertToInt(dsDealer.Tables[0].Rows[0]["DealerID"]);

                    //    ds = ObjCust.GetLeadMaster(EGpdealerID);
                    //}

                }

                if (iHOBrId != 0)
                {
                    dsDealer = ObjCust.GetLeadDealerID(iHOBrId);

                    if (dsDealer.Tables[0].Rows.Count > 0)
                    {
                        EGpdealerID = Func.Convert.iConvertToInt(dsDealer.Tables[0].Rows[0]["DealerID"]);

                        ds = ObjCust.GetLeadMaster(EGpdealerID);
                    }

                }

                if (!IsPostBack)
                {
                    // FillRegion();
                    FillStateCountry();
                    FillCustType();
                    //FillOrgType();
                    FillLeadType();

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                    SearchGrid.sGridPanelTitle = "Lead Master";
                    FillSelectionGrid();
                    iDealerID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iDealerID == 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            iDealerID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                        }
                }
                if (iDealerID != 0)
                {
                    GetDataAndDisplay();
                }
                FillSelectionGrid();
                ToolbarC.iValidationIdForSave = 66;
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
                lblTitle.Text = "Lead Master";
                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                }
                //drpCustType.Attributes.Add("onblur", "CheckcustType(event,this)");
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
                clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                if (iDealerID != 0)
                {
                    ds = ObjDealer.GetLeadMaster(iDealerID);
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
                    FillLeadType();
                    drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["type_flag_id"]);

                    FillOrgType();
                    drporgType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["OrgType"]);

                    FillCustType();
                    drpcustSubType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustSub"]);

                    txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add2"]);
                    txtAddress3.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Add3"]);
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
                    txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone"]);
                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["E_mail"]);
                    txtContactPerson.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_Person"]);

                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else
                    {
                        drpActive.SelectedValue = "2";
                    }
                    //if (drpCustType.SelectedValue == "3")
                    //{
                    //    drpCustType.Enabled = false;
                    //}
                    //else
                    //{
                    //    drpCustType.Enabled = true;
                    //}
                }
                //if (ds.Tables[1].Rows.Count > 0)
                //{

                //GrdCategoryDetails.DataSource = ds.Tables[1];
                //GrdCategoryDetails.DataBind();
                //}
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
            drporgType.SelectedValue = "0";
            drpcustSubType.SelectedValue = "0";
            txtCustomerName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtpincode.Text = "";
            // drpRegion.SelectedValue = "0";
            drpState.SelectedValue = "0";
            drpDistrict.SelectedItem.Text = "0";
            drpRegion.SelectedItem.Text = "";
            txtMobile.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtContactPerson.Text = "";

            //drpCustType.Enabled = true;
        }

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("City");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("Region");
                SearchGrid.AddToSearchCombo("Country");
                SearchGrid.iDealerID = EGpdealerID;
                SearchGrid.sSqlFor = "LeadMaster";
                SearchGrid.sGridPanelTitle = "Lead Master List";
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
                    ClearDealerHeader();
                    txtID.Text = "0";
                    iID = 0;
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    // if (bValidateRecord() == false) return;
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    //Chnaged by VIkram
                    //iID= objCustomer.bSaveLeadDetails(
                    //                                   iID, 
                    //                                   EGpdealerID, 
                    //                                   HOBrID, 
                    //                                   Func.Convert.iConvertToInt(drpCustType.SelectedValue), 
                    //                                   Func.Convert.iConvertToInt(drporgType.SelectedValue), 
                    //                                   Func.Convert.iConvertToInt(drpcustSubType.SelectedValue), 
                    //                                   txtCustomerName.Text,
                    //                                   txtAddress1.Text, 
                    //                                   txtAddress2.Text, 
                    //                                   txtAddress3.Text, 
                    //                                   txtCity.Text, 
                    //                                   txtpincode.Text,
                    //                                   Func.Convert.iConvertToInt(drpRegion.SelectedValue),
                    //                                   Func.Convert.iConvertToInt(drpState.SelectedValue), 
                    //                                   Func.Convert.iConvertToInt(drpDistrict.SelectedValue), 
                    //                                   1, 
                    //                                   txtMobile.Text, 
                    //                                   txtPhone.Text, 
                    //                                   txtEmail.Text,
                    //                                   txtContactPerson.Text, 
                    //                                   drpActive.SelectedItem.Text
                    //                                   );

                    if (iID > 0)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                    if (iID != 0)
                    {
                        ds = objCustomer.GetLeadMaster(iID);
                        DisplayData(ds);
                        objCustomer = null;
                        FillGrid();
                    }
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
                SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", SearchGrid.sModelPart, EGpdealerID, "LeadMaster"));
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
        private void FillCustType()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType;

                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetCustSubType", 0);
                if (dsCustType != null)
                {
                    drpcustSubType.DataSource = dsCustType.Tables[0];
                    drpcustSubType.DataTextField = "Name";
                    drpcustSubType.DataValueField = "ID";
                    drpcustSubType.DataBind();
                    drpcustSubType.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillOrgType()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsOrgtype;

                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");

                if (drpCustType.SelectedValue != "1")
                {
                    drporgType.Enabled = true;
                    dsOrgtype = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetOrgType", drpCustType.SelectedValue);
                    if (dsOrgtype != null)
                    {
                        drporgType.DataSource = dsOrgtype.Tables[0];
                        drporgType.DataTextField = "Name";
                        drporgType.DataValueField = "ID";
                        drporgType.DataBind();
                        drporgType.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }
                else
                {
                    drporgType.Enabled = false;
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
            FillRegion();
            Filldistrict();
        }

        protected void drpCustType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillOrgType();
        }

        private void FillDetailsFromGrid()
        {
            //DataRow dr;
            //dtDetails = new DataTable();
            //dtDetails.Columns.Add(new DataColumn("PartCat", typeof(string)));
            //dtDetails.Columns.Add(new DataColumn("Percentage", typeof(decimal)));

            //for (int iRowCnt = 0; iRowCnt < GrdCategoryDetails.Rows.Count; iRowCnt++)
            //{

            //    dr = dtDetails.NewRow();
            //    //dr["SRNo"] = "1";            
            //    dr["PartCat"] = Func.Convert.sConvertToString((GrdCategoryDetails.Rows[iRowCnt].FindControl("lblPartCategory") as Label).Text);
            //    dr["Percentage"] = Func.Convert.dConvertToDouble((GrdCategoryDetails.Rows[iRowCnt].FindControl("lblPercentage") as TextBox).Text);

            //    dtDetails.Rows.Add(dr);
            //    dtDetails.AcceptChanges();

            //}
        }

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FillStateCountry();
        }
    }
}