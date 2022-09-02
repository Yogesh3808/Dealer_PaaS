using System;
using System.Data;
using System.Configuration;
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
using AjaxControlToolkit;
using System.Drawing;

namespace MANART.Forms.Admin
{
    public partial class frmMailConfigurationforDomestic : System.Web.UI.Page
    {
        private DataSet DSDealer;
        DataSet dsState;
        private int iID;
        string sMessage = "";
        private string _sDealerId;
        private string sControlClientID = "";
        string sDealerIds = "";
        string UserType = "";
        string ModuleType = "";
        string sign = "";
        string active = "";

        protected void Page_Init(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Fillcombo();
                FillRegion();
                DataSet dsFillDetails = new DataSet();
                // 'Replace Func.DB to objDB by Shyamal on 05042012
                clsDB objDB = new clsDB();
                try
                {
                    if (OptionUserType.SelectedValue == "D")
                    {
                        dsFillDetails = objDB.ExecuteQueryAndGetDataset("select Top 1 ID from M_Sys_AutoMailDomestic");
                    }
                    else
                    {
                        dsFillDetails = objDB.ExecuteQueryAndGetDataset("select Top 1 ID from M_Sys_AutoMail");
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (objDB != null) objDB = null;
                }
                iID = Func.Convert.iConvertToInt(dsFillDetails.Tables[0].Rows[0]["ID"].ToString());
                // iID = Func.Convert.iConvertToInt(txtID.Text);
                if (iID == 0)
                {
                    iID = 1;
                    //ViewState["iID"] = 1; 
                    GetDataAndDisplayInDetails();

                }
                else
                {
                    GetDataAndDisplayInDetails();
                }

            }
            FillSelectionGrid();
        }
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Process Name");
                SearchGrid.AddToSearchCombo("Process Description");
                SearchGrid.AddToSearchCombo("User Type");
                SearchGrid.AddToSearchCombo("Module Type");
                SearchGrid.AddToSearchCombo("Dealer Name");
                SearchGrid.AddToSearchCombo("Region");
                SearchGrid.AddToSearchCombo("State");
                SearchGrid.AddToSearchCombo("To Mail");
                SearchGrid.AddToSearchCombo("CC Mail");
                SearchGrid.AddToSearchCombo("Subject");
                SearchGrid.AddToSearchCombo("Mail Body");
                SearchGrid.AddToSearchCombo("Signature");
                SearchGrid.AddToSearchCombo("Active");
                SearchGrid.sModelPart = "";
                SearchGrid.iDealerID = -1;
                SearchGrid.sSqlFor = "MailConfig";
                SearchGrid.sGridPanelTitle = "Mail Configuration";
                //SearchGrid.bIsCallForServer = true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // txtProcessName.Style.Add("display", "none");
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
            //  txtTomail.Attributes.Add("onkeyperss", "validEmail()");  
            //lnkMain.Attributes.Add("onclick", "javascript:return SHMulSel01('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");

            //txtDealerName.Text = "";
            //for (int k = 0; k < ChkDealer.Items.Count; k++)
            //{
            //    if (ChkDealer.Items[k].Selected == true)
            //    {


            //        txtDealerName.Text = txtDealerName.Text + ChkDealer.Items[k].Text + ",";
            //    }
            //}
            //txtDealerID.Text = "";
            //for (int cnt = 0; cnt < ChkDealer.Items.Count; cnt++)
            //{
            //    if (ChkDealer.Items[cnt].Selected == true)
            //    {


            //        txtDealerID.Text = txtDealerID.Text + ChkDealer.Items[cnt].Value + ",";
            //    }
            //}




        }


        private void Fillcombo()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet DsProcessName = new DataSet();
                DsProcessName = objDB.ExecuteQueryAndGetDataset("Select distinct Module_Name as Name, Module_Name as ID from M_Sys_AutoMailDomestic");
                drpProcessName.DataSource = DsProcessName.Tables[0];
                drpProcessName.DataTextField = "Name";
                drpProcessName.DataValueField = "ID";
                drpProcessName.DataBind();
                drpProcessName.Items.Insert(0, new ListItem("--Select--", "0"));

                ListItem lstitm = new ListItem("NEW", "9999");
                drpProcessName.Items.Add(lstitm);
                //drpProcessName.Attributes.Add("onChange", "OnComboValueChange1(this,'" + txtProcessName.ID + "')");
                // drpProcessName.Attributes.Add("onclick", "OnComboValueChange1(this,'" + txtProcessName.ID + "')");

                txtProcessName.Visible = false;

                drpProcessName.Visible = true;

                // ChkDealer.Attributes.Add("onclick", "SelectAll(this,'" + sControlClientID + "')");
                // ChkDealer.Attributes.Add("onclick", "SelectAll(this)");

                // drpProcessName.DataTextField = "Name";
                //drpProcessName.DataValueField = "ID";
                // drpProcessName.Items.Insert(0,(new ListItem("--Select--"));    

                //clsDealer objDealer = new clsDealer();
                //DSDealer = objDealer.GetAllDomesticDealer();
                //ChkDealer.DataSource = DSDealer.Tables[0] ;
                //ChkDealer.DataTextField = "Name";
                //ChkDealer.DataValueField = "ID";
                //ChkDealer.DataBind();
                //sControlClientID = "ContentPlaceHolder1_" + this.ID;
                //txtControl_ID.Value = sControlClientID;

                //for (int k = 0; k < ChkDealer.Items.Count; k++)
                //{
                //    ChkDealer.Items[k].Attributes.Add("onclick", "SCIT(this,'" + sControlClientID + "')");
                //    ChkDealer.Items[k].Attributes.Add("alt", ChkDealer.Items[k].Value);
                //    ChkDealer.Items[k].Attributes.Add("runat", "server");
                //}
                //sControlClientID = "ContentPlaceHolder1_" + this.ID;
                //txtControl_ID.Value = sControlClientID;

                //DataSet dsRegion;

                //dsRegion = objDB.ExecuteStoredProcedureAndGetDataset("SP_DomMailConfi_StateRegion", Func.Convert.iConvertToInt(drpDealerName.SelectedValue), "R");
                //if (dsRegion != null)
                //{
                //    drpregion.DataSource = dsRegion.Tables[0];
                //    drpregion.DataTextField = "Name";
                //    drpregion.DataValueField = "ID";
                //    drpregion.DataBind();
                //    drpregion.Items.Insert(0, new ListItem("--Select--", "0"));
                //}
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

                dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= '" + OptionUserType.SelectedValue + "' ");
                if (dsRegion != null)
                {
                    drpregion.DataSource = dsRegion.Tables[0];
                    drpregion.DataTextField = "Name";
                    drpregion.DataValueField = "ID";
                    drpregion.DataBind();
                    drpregion.Items.Insert(0, new ListItem("--Select--", "0"));
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
                if (OptionUserType.SelectedValue == "D")
                {
                    dsState = objDB.ExecuteQueryAndGetDataset("select ID as ID,State as Name from M_State where Region_Id= " + drpregion.SelectedValue);
                    drpState.DataSource = dsState.Tables[0];
                    drpState.DataTextField = "Name";
                    drpState.DataValueField = "ID";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new ListItem("--Select--", "0"));

                }
                else
                {
                    dsState = objDB.ExecuteQueryAndGetDataset("select ID as ID,Country_Name as Name from M_Country where Region_Id= " + drpregion.SelectedValue);
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


        private void clearControl()
        {
            OptionUserType.SelectedValue = "D";
            FillRegion();
            txtDealerName.Text = "";
            //drpState.Text = "";
            //ChkDealer.SelectedValue = "";
            txtDesc.Text = "";
            txtProcessName.Text = "";
            drpUserType.SelectedValue = "0";
            drpModuleType.SelectedValue = "0";
            drpActive.SelectedValue = "0";
            txtTomail.Text = "";
            txtCcMail.Text = "";
            txtsubject.Text = "";
            txtMailBody.Text = "";
            drpSign.SelectedValue = "0";

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                foreach (ListItem li in ChkDealer.Items)
                {
                    li.Selected = false;
                }
                SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
                iID = Func.Convert.iConvertToInt(txtID.Text);

                // ViewState["iID"] = iID; 
                GetDataAndDisplayInDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void GetDataAndDisplayInDetails()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                //ds = objModeofShip.GetModeofShipmentRecord();
                //DisplayData(ds);

                if (iID != 0)
                {
                    //ViewState["iID"] = iID;
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMailConfiguration", iID, 'D');
                    DisplayData(ds);

                }
                else
                {
                    ds = null;


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
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    OptionUserType.SelectedValue = ds.Tables[0].Rows[0]["Mail_type"].ToString();
                    // OptionUserType.SelectedValue = "D";
                    txtProcessName.Text = ds.Tables[0].Rows[0]["Process Name"].ToString();
                    txtProcessName.Visible = false;
                    drpProcessName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Process Name"].ToString());
                    drpProcessName.Visible = true;
                    active = ds.Tables[0].Rows[0]["Active"].ToString();
                    if (active == "Y")
                    {
                        drpActive.SelectedValue = "1";

                    }
                    else if (active == "N")
                    {
                        drpActive.SelectedValue = "2";
                    }
                    else
                    {
                        drpActive.SelectedValue = "0";
                    }
                    UserType = ds.Tables[0].Rows[0]["User Type"].ToString();
                    if (UserType == "Dealer")
                    {
                        drpUserType.SelectedValue = "1";

                    }
                    else if (UserType == "VECV")
                    {
                        drpUserType.SelectedValue = "2";
                    }
                    else
                    {
                        drpUserType.SelectedValue = "0";
                    }
                    ModuleType = ds.Tables[0].Rows[0]["Module Type"].ToString();
                    if (ModuleType == "P")
                    {
                        drpModuleType.SelectedValue = "1";

                    }
                    else if (ModuleType == "M")
                    {
                        drpModuleType.SelectedValue = "2";
                    }
                    else
                    {
                        drpModuleType.SelectedValue = "0";
                    }

                    FillRegion();
                    drpregion.SelectedValue = ds.Tables[0].Rows[0]["Region_ID"].ToString();
                    FillStateCountry();
                    drpState.SelectedValue = ds.Tables[0].Rows[0]["State_ID"].ToString();
                    FillDealer();
                    txtDealerName.Text = ds.Tables[0].Rows[0]["Dealer Name"].ToString();
                    ListItem myListItem = ChkDealer.Items.FindByValue((ds.Tables[0].Rows[0]["Dealer_ID"].ToString()));
                    if (myListItem != null)
                    {
                        myListItem.Selected = true;
                    }

                    // ChkDealer.SelectedValue = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Dealer Id"].ToString());
                    //  ChkDealer.Items[49]. = true;  
                    txtTomail.Text = ds.Tables[0].Rows[0]["To Mail"].ToString();
                    txtCcMail.Text = ds.Tables[0].Rows[0]["CC Mail"].ToString();
                    txtsubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
                    txtMailBody.Text = ds.Tables[0].Rows[0]["Mail_Body"].ToString();
                    txtDesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    sign = ds.Tables[0].Rows[0]["Signature"].ToString();
                    if (sign == "Dealer")
                    {
                        drpSign.SelectedValue = "1";
                    }
                    else if (sign == "VECV")
                    {
                        drpSign.SelectedValue = "2";
                    }
                    else
                    {
                        drpSign.SelectedValue = "0";
                    }

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateRecord()
        {

            bool bValidateRecord = true;

            //if (drpDealerName.SelectedItem.Value == "0" || drpUserType.SelectedItem.Value == "0" || drpModuleType.SelectedItem.Value == "0" || drpregion.SelectedItem.Value == "0" || drpState.SelectedItem.Value == "0" || drpSign.SelectedItem.Value == "0")
            //{
            if (drpActive.SelectedItem.Text == "--Select--" || drpUserType.SelectedItem.Text == "--Select--" || drpModuleType.SelectedItem.Text == "--Select--" || drpSign.SelectedItem.Text == "--Select--")
            {
                sMessage = sMessage + "Please Select record.";
                bValidateRecord = false;

            }
            if ((txtProcessName.Text == "" && drpProcessName.SelectedItem.Text == "--Select--") | txtTomail.Text == "" || txtsubject.Text == "" || txtMailBody.Text == "" || txtCcMail.Text == "" || txtDesc.Text == "")
            {
                sMessage = sMessage + "Please Enter Record.";
                bValidateRecord = false;
            }

            if (txtsubject.Text != "")
            {
                int intIndexSubstring = 0;
                int intIndexSubstring11 = 0;
                string strsubject = txtsubject.Text;

                string SearchString = " " + "\"no\"" + " ";
                string SearchString1 = " " + "\"date\"" + " ";
                intIndexSubstring = strsubject.IndexOf(SearchString);
                intIndexSubstring11 = strsubject.IndexOf(SearchString1);
                if (intIndexSubstring == -1 || intIndexSubstring11 == -1)
                {
                    sMessage = sMessage + "Please Select Proper format of Subject.";
                    bValidateRecord = false;

                }

            }

            if (txtMailBody.Text != "")
            {
                int intIndexSubstring1 = 0;
                int intIndexSubstring2 = 0;
                string strMailBody = txtMailBody.Text;

                string SearchString2 = " " + "\"no\"" + " ";
                string SearchString3 = " " + "\"date\"" + " ";
                intIndexSubstring1 = strMailBody.IndexOf(SearchString2);
                intIndexSubstring2 = strMailBody.IndexOf(SearchString3);
                if (intIndexSubstring1 == -1 || intIndexSubstring2 == -1)
                {
                    sMessage = sMessage + "Please Select Proper format of MailBody.";
                    bValidateRecord = false;

                }

            }
            if (drpProcessName.SelectedItem.Text == "NEW")
            {
                // 'Replace Func.DB to objDB by Shyamal on 05042012
                DataSet dsProcessName = new DataSet();
                clsDB objDB = new clsDB();
                try
                {
                    dsProcessName = objDB.ExecuteQueryAndGetDataset("select Module_Name from  M_Sys_AutoMailDomestic where Module_Name ='" + txtProcessName.Text + "'");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (objDB != null) objDB = null;
                }
                if (dsProcessName != null)
                {
                    if (dsProcessName.Tables[0].Rows.Count > 0)
                    {
                        sMessage = sMessage + "Process Name is already Exist";
                        bValidateRecord = false;

                    }
                }
            }

            if (ChkDealer.SelectedValue == "")
            {

                sMessage = sMessage + "Please Select Record.";
                bValidateRecord = false;
            }

            return bValidateRecord;
        }


        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;

                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    FillRegion();
                    Fillcombo();
                    clearControl();
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    int j = 0;

                    if (bValidateRecord() == false)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        return;
                    }

                    for (int k = 0; k < ChkDealer.Items.Count; k++)
                    {
                        if (ChkDealer.Items[k].Selected)
                        {
                            string ProcessName = "";

                            if (drpProcessName.SelectedItem.Text == "NEW")
                            {
                                ProcessName = txtProcessName.Text;
                            }
                            else
                            {
                                ProcessName = drpProcessName.SelectedItem.Text;
                            }

                            iID = bSaveMailConfiguration(iID, Func.Convert.sConvertToString(OptionUserType.SelectedValue), ProcessName, drpUserType.SelectedItem.Text, drpModuleType.SelectedItem.Text, "DealerMail",
                            Func.Convert.iConvertToInt(ChkDealer.Items[k].Value), Func.Convert.iConvertToInt(drpregion.SelectedItem.Value), Func.Convert.iConvertToInt(drpState.SelectedItem.Value),
                            txtTomail.Text, txtCcMail.Text, txtsubject.Text, txtMailBody.Text, drpSign.SelectedItem.Text, txtDesc.Text, drpActive.SelectedItem.Text);

                            j = j + 1;


                        }
                    }


                    if (iID > 0)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                }
                FillGrid();

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
                SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", "", -1, "MailConfig"));
                objclsComman = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        public int bSaveMailConfiguration(int iID, string MailType, string ModuleName, string UserType, string ModuleType, string FromMailID, int DealerID, int RegionID, int StateID, string ToMailID, string CcMailID, string Subject, string MailBody, string Signature, string Description, string Active)
        {


            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_SaveMailConfigurationForDomestic", iID, MailType, ModuleName, UserType, ModuleType, FromMailID, DealerID, RegionID, StateID, ToMailID, CcMailID, Subject, MailBody, Signature, Description, Active);

                objDB.CommitTransaction();

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
        }


        protected void drpProcessName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (drpProcessName.SelectedItem.Text != "--Select--")
            //{

            //    txtProcessName.Text = drpProcessName.SelectedItem.Text;
            //}
            //else { txtProcessName.Text = ""; }
            if (drpProcessName.SelectedItem.Text == "NEW")
            {
                txtProcessName.Text = "";
                txtProcessName.Visible = true;
                txtProcessName.Focus();
                drpProcessName.Visible = false;
                //txtProcessName.Text = drpProcessName.SelectedItem.Text;
            }
            //else { txtProcessName.Text = ""; }

        }
        protected void txtTomail_TextChanged(object sender, EventArgs e)
        {

        }


        protected void OptionUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OptionUserType.SelectedValue == "E")
            {
                lblState.Text = "Country";
            }
            else
            {
                lblState.Text = "State";
            }
            FillRegion();
            if (iID == 0)
            {
                iID = 1;
                //ViewState["iID"] = 1; 
                GetDataAndDisplayInDetails();

            }
            else
            {
                GetDataAndDisplayInDetails();
            }

        }
        protected void drpregion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateCountry();
        }
        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealer();
            txtDealerName.Text = "--Select--";

        }
        private void FillDealer()
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (OptionUserType.SelectedValue == "D")
                {
                    DSDealer = objDB.ExecuteQueryAndGetDataset("select Id as ID,Dealer_Name + '(' + ISNULL(Dealer_vehicle_code,'') + ',' +  ISNULL(Dealer_Spares_code,'') + ')' as Name  from M_dealer where Dealer_State_Id= " + drpState.SelectedValue + " and Dealer_Region_Id= " + drpregion.SelectedValue + " and Dealer_origin= '" + OptionUserType.SelectedValue + "'");
                    ChkDealer.DataSource = DSDealer.Tables[0];
                    ChkDealer.DataTextField = "Name";
                    ChkDealer.DataValueField = "ID";
                    ChkDealer.DataBind();

                }
                else
                {
                    DSDealer = objDB.ExecuteQueryAndGetDataset("select Id as ID,Dealer_Name + '(' + ISNULL(Dealer_vehicle_code,'') + ',' +  ISNULL(Dealer_Spares_code,'') + ')' as Name  from M_dealer where Dealer_Country_Id= " + drpState.SelectedValue + " and Dealer_Region_Id= " + drpregion.SelectedValue + " and Dealer_origin= '" + OptionUserType.SelectedValue + "'");
                    // DSDealer = objDB.ExecuteQueryAndGetDataset("select ID as ID ,Dealer_Name + '(' + Dealer_vehicle_code + ',' +  Dealer_Spares_code + ')' as Name  from M_dealer where Dealer_Country_Id=" + drpState.SelectedValue);
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
        //protected void ChkAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkAll.Checked == true)
        //    {
        //        for (int j = 0; j < ChkDealer.Items.Count; j++)
        //        {
        //            ChkDealer.Items[j].Selected = true;
        //        }
        //        txtDealerName.Text = "All Selected";
        //    }
        //    else
        //    {
        //        for (int j = 0; j < ChkDealer.Items.Count; j++)
        //        {
        //            ChkDealer.Items[j].Selected = false;
        //        }
        //        txtDealerName.Text = "--Select--";
        //    }

        //}
    }
}