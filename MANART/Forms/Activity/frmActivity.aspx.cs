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

namespace MANART.Forms.Activity
{
    public partial class frmActivity : System.Web.UI.Page
    {
        string sUserType = "";
        int iActivityID = 0;
        int iUsreTypeId = 0;
        int iUserId = 0;
        private string sControlClientID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //txtFromDate.sOnBlurScript = " return SetCurrAndFutureDate(ContentPlaceHolder1_txtFromDate_txtDocDate,'From Date Should be Greater or equal to Current Date');";
                //txtToDate.sOnBlurScript = " return CheckAcDateGreter(ContentPlaceHolder1_txtToDate_txtDocDate,ContentPlaceHolder1_txtFromDate_txtDocDate,'To Date should be Greater than sFrom Date');";

                sControlClientID = "ContentPlaceHolder1";
                txtDealerName.Attributes.Add("onclick", "SHMulSelDlr('" + sControlClientID + "', event)");
                lnkMainDlr.Attributes.Add("onclick", "javascript:return SHMulSel01Dlr('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");

                txtDepoName.Attributes.Add("onclick", "SHMulSelDpo('" + sControlClientID + "', event)");
                lnkMainDpo.Attributes.Add("onclick", "javascript:return SHMulSel01Dpo('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");

                txtRegion.Attributes.Add("onclick", "SHMulSelReg('" + sControlClientID + "', event)");
                lnkMainReg.Attributes.Add("onclick", "javascript:return SHMulSel01Reg('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");


                iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);

                SearchGrid.bIsCollapsable = false;
                SearchGrid.bGridFillUsingSql = false;
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iValidationIdForSave = 23;
                ToolbarC.iValidationIdForConfirm = 23;

                sUserType = Session["UserType"].ToString();
                txtDealerType.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                if (sUserType == "2")
                {
                    sUserType = "D";
                    txtDealerType.Text = "Domestic";
                    drpDealerType.SelectedValue = "D";
                    lblStateCountry.Text = "State";
                    lblDealer.Text = "Dealer Name";
                }
                else
                    if (sUserType == "1")
                    {
                        sUserType = "E";
                        txtDealerType.Text = "Export";
                        drpDealerType.SelectedValue = "E";
                        lblStateCountry.Text = "Country";
                        lblDealer.Text = "Distributor Name";
                    }
                drpDirectClaim.SelectedValue = "Y";
                if (!IsPostBack)
                {
                    FillCombo();
                    FillRegion();
                    FillDepo();
                    FillDealer();
                    DisplayCurrentRecord();
                    //drpActivityName.Style["display"] = "none"; 



                }
                FillSelectionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void FillCombo()
        {

            Func.Common.BindDataToCombo(drpDepartmentalActivity, clsCommon.ComboQueryType.DepartmentForActivity, 0, " And Dept_ExpOrDome=" + "'" + sUserType + "'");
            Func.Common.BindDataToCombo(drpCreditorPositionKey, clsCommon.ComboQueryType.CredPostKey, 0);
            Func.Common.BindDataToCombo(drpSpecialGL, clsCommon.ComboQueryType.SpecialGL, 0);
            Func.Common.BindDataToCombo(drpEtbPostingKey, clsCommon.ComboQueryType.ETBPostingKey, 0);
            Func.Common.BindDataToCombo(ddlCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
            //Func.Common.BindDataToCombo(drpAccount, clsCommon.ComboQueryType.Account, 0);


        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;

                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PnlSearchGrid.Style.Add("display", "none");
                    PActivityDetails.Style.Add("display", "none");
                    //drpActivityName.Style["display"] = "";             

                    //Func.Common.BindDataToCombo(drpActivityName, clsCommon.ComboQueryType.ActivityName, 0, " And Dealer_Type=" + "'" + sUserType + "'");
                    //ListItem lstitm = new ListItem("NEW", "9999");
                    //drpActivityName.Items.Add(lstitm);
                    NewRecord();
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNew);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    
                    //MANART_BAL.Activity objActivity = new MANART_BAL.Activity();;
                    MANART_BAL.Activity objActivity = new MANART_BAL.Activity();
                    if (bValidateData() == false) return;
                    if (objActivity.bSaveActivityMaster(ref iActivityID, ActivitySave(), ActivityDetails()) == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                        txtID.Text = Func.Convert.sConvertToString(iActivityID);
                        if (txtID.Text == "0")
                        {
                            FillSelectionGrid();
                            DisplayCurrentRecord();
                            PnlSearchGrid.Style.Add("display", "");
                            PActivityDetails.Style.Add("display", "");
                            return;
                        }
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }
                    PnlSearchGrid.Style.Add("display", "");
                    PActivityDetails.Style.Add("display", "");
                    objActivity = null;
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    // Changed by Vikram K
                   MANART_BAL.Activity  objActivity = new MANART_BAL.Activity();
                    if (objActivity.bSaveActivityMaster(ref iActivityID, ActivitySave(), ActivityDetails()) == true)
                    {
                        ConfirmRecord(iActivityID);
                    }
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {

                    MANART_BAL.Activity objActivity = new MANART_BAL.Activity();
                    iActivityID = Func.Convert.iConvertToInt(txtID.Text);
                    objActivity.CancelActivity(iActivityID);
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8);</script>");
                    DisplayCurrentRecord();
                    PnlSearchGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateData()
        {
            string sMessage = " Please enter/select the records.";
            bool bValidateRecord = true;
            if (txtRegion.Text == "--Select--")
            {
                sMessage = sMessage + "\\n Please Select Region";
                bValidateRecord = false;
            }
            if (txtDepoName.Text == "--Select--")
            {
                sMessage = sMessage + "\\n Please Select " + lblStateCountry.Text + " Name";
                bValidateRecord = false;
            }
            if (txtDealerName.Text == "--Select--")
            {
                sMessage = sMessage + "\\n Please Select " + lblDealer.Text + " Name.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            return bValidateRecord;
        }
        protected void drpDepartmentalActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldrpTypeOfActivity();
            FillDealer();
        }

        private void FilldrpTypeOfActivity()
        {
            int iDptActID;
            iDptActID = Func.Convert.iConvertToInt(drpDepartmentalActivity.SelectedValue);
            if (drpDepartmentalActivity.SelectedValue != "1")
            {
                if (ddlCategory.Items.FindByValue("0") == null)
                    ddlCategory.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlCategory.SelectedValue = "0";
                ddlCategory.Enabled = false;
            }
            else
            {
                ddlCategory.Enabled = true;
                if (ddlCategory.Items.Count > 0)
                    ddlCategory.Items.Remove(new ListItem("--Select--", "0"));
            }
            string sCat = ddlCategory.SelectedValue;
            if (sCat == "1")
                sCat = "And Is_HD='Y'";
            else if (sCat == "2")
                sCat = "And Is_LMD='Y'";
            else if (sCat == "3")
                sCat = "And Is_BUS='Y'";
            else
                sCat = "And Is_Default='Y'";
            Func.Common.BindDataToCombo(drpTypeOfActivity, clsCommon.ComboQueryType.TypeOfActivity, iDptActID, sCat);

            SetAcountGLAndCostCenter(0);


        }
        protected void drpTypeOfActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillRegion();
            //FillDepo();
            //FillDealer();        
            SetAcountGLAndCostCenter(Func.Convert.iConvertToInt(drpTypeOfActivity.SelectedValue));
        }
        private void SetAcountGLAndCostCenter(int iTypeOfActivity)
        {
            DataSet ds = new DataSet();
            MANART_BAL.Activity objActivity = new MANART_BAL.Activity();
            ds = objActivity.GetAcountGLAndCostCenter(iTypeOfActivity);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //txtCostCenter.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cost_Centre"]);
                txtGLAccount.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Account_GL"]);
            }
            else
            {
                //txtCostCenter.Text = "";
                txtGLAccount.Text = "";
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iActivityID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();

        }

        private void GetDataAndDisplay()
        {
            try
            {
                //DataTable dtDetails = null;

                if (iActivityID != 0)
                {
                    DataSet ds = new DataSet();
                    MANART_BAL.Activity objActivity = new MANART_BAL.Activity();;
                    ds = objActivity.GetActivity(iActivityID, "Activity", "N", (sUserType == "D") ? 2 : 1);
                    //dtDetails = ds.Tables[0];
                    DisplayData(ds);
                    objActivity = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }

                //dtDetails = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void DisplayData(DataSet dtActivityDtls)
        {
            try
            {
                string sConfirmStatus = "";
                if (dtActivityDtls.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header   
                txtID.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["ID"]);
                txtNameOfActivity.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Activity_Name"]);
                // txtObjective.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["Objective"]);
                txtToDate.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["To_Date"]);
                txtFromDate.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["From_Date"]);
                txtRemark.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["VECV_Remark"]);
                drpDealerType.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Dealer_Type"]);
                if (drpDealerType.SelectedValue == "D")
                    txtDealerType.Text = "Domestic";
                else
                    txtDealerType.Text = "Export";
                drpDepartmentalActivity.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Dept_ID"]);
                ddlCategory.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["BasicCat_Id"]);
                FilldrpTypeOfActivity();
                drpTypeOfActivity.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Type_ID"]);
                sConfirmStatus = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["ActivityConfirm"]);
                drpDirectClaim.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["ClaimAllow"]);

                drpCreditorPositionKey.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["CredtitiorPostKey_Id"]);
                drpSpecialGL.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["SpecialGL_Id"]);
                drpEtbPostingKey.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["ETBPostKey_Id"]);
                //drpCosrCenter.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["CostCenter_Id"]);

                //Commented by Shyamal as on 08082014 not needed here shifted to processing screen
                //txtCostCenter.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Cost_Centre"]);

                //drpAccount.SelectedValue = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Account_Id"]);
                txtGLAccount.Text = Func.Convert.sConvertToString(dtActivityDtls.Tables[0].Rows[0]["Account_GL"]);


                DataTable dtDetails = new DataTable();
                dtDetails = dtActivityDtls.Tables[1];
                Session["ActivityDtls"] = dtDetails;
                if (dtDetails.Rows.Count == 0)
                    dtDetails = null;
                GridSelectionDetails.DataSource = dtDetails;
                GridSelectionDetails.DataBind();
                GetSelectedDepoAndDealer(dtDetails);
                EnableDisable(sConfirmStatus);
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
                SearchGrid.AddToSearchCombo("Department");
                SearchGrid.AddToSearchCombo("Type of Activity");
                SearchGrid.AddToSearchCombo("Name of Activity");
                SearchGrid.AddToSearchCombo("Activity Date");
                SearchGrid.sModelPart = sUserType;
                SearchGrid.iDealerID = -1;
                SearchGrid.sSqlFor = "ActivityMaster";
                SearchGrid.sGridPanelTitle = " Activity List";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private DataTable ActivitySave()
        {
            DataTable dtHdr = new DataTable();
            DataRow dr;
            //Get Header InFormation        
            dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Activity_Name", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("Dealer_Type", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("From_Date", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("To_Date", typeof(string)));
            //dtHdr.Columns.Add(new DataColumn("Target_Customer", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("VECV_Remark", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Dept_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Type_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("ClaimAllow", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("CredtitiorPostKey_Id", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("SpecialGL_Id", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("ETBPostKey_Id", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("CostCenter_Id", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Account_Id", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("BasicCat_Id", typeof(int)));

            dr = dtHdr.NewRow();
            dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
            dr["Activity_Name"] = txtNameOfActivity.Text;
            dr["Dealer_Type"] = drpDealerType.SelectedValue.ToString();
            dr["From_Date"] = txtFromDate.Text;
            dr["To_Date"] = txtToDate.Text;
            dr["VECV_Remark"] = txtRemark.Text.Trim();
            dr["Dept_ID"] = Func.Convert.iConvertToInt(drpDepartmentalActivity.SelectedValue);
            dr["Type_ID"] = Func.Convert.iConvertToInt(drpTypeOfActivity.SelectedValue);
            dr["ClaimAllow"] = drpDirectClaim.SelectedValue;
            dr["CredtitiorPostKey_Id"] = Func.Convert.iConvertToInt(drpCreditorPositionKey.SelectedValue);
            dr["SpecialGL_Id"] = Func.Convert.iConvertToInt(drpSpecialGL.SelectedValue);
            dr["ETBPostKey_Id"] = Func.Convert.iConvertToInt(drpEtbPostingKey.SelectedValue);
            dr["CostCenter_Id"] = 0;
            dr["Account_Id"] = 0;
            dr["BasicCat_Id"] = Func.Convert.iConvertToInt(ddlCategory.SelectedValue);
            dtHdr.Rows.Add(dr);
            dtHdr.AcceptChanges();
            return dtHdr;

        }
        private DataTable ActivityDetails()
        {
            DataTable dtDls = new DataTable();
            DataRow dr;
            dtDls.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
            dtDls.Columns.Add(new DataColumn("StateIDORCountryID", typeof(int)));
            dtDls.Columns.Add(new DataColumn("RegionID", typeof(int)));
            for (int iRegion = 0; iRegion < ChkRegion.Items.Count; iRegion++)
            {
                int iRegionID = 0;
                if (ChkRegion.Items[iRegion].Selected == true)
                    iRegionID = Func.Convert.iConvertToInt(ChkRegion.Items[iRegion].Value);
                for (int iStateORCountry = 0; iStateORCountry < ChkDepo.Items.Count; iStateORCountry++)
                {
                    int iStateIDORCountryID = 0;
                    if (ChkDepo.Items[iStateORCountry].Selected == true)
                        iStateIDORCountryID = Func.Convert.iConvertToInt(ChkDepo.Items[iStateORCountry].Value);
                    for (int iDealer = 0; iDealer < ChkDealer.Items.Count; iDealer++)
                    {

                        if (ChkDealer.Items[iDealer].Selected == true)
                        {
                            dr = dtDls.NewRow();
                            dr["Dealer_ID"] = ChkDealer.Items[iDealer].Value;
                            dr["StateIDORCountryID"] = iStateIDORCountryID;
                            dr["RegionID"] = iRegionID;
                            dtDls.Rows.Add(dr);
                        }
                    }
                }
            }
            dtDls.AcceptChanges();
            return dtDls;
        }
        private void DisplayCurrentRecord()
        {
            try
            {
                //DataTable dtDetails = null;
                DataSet ds = new DataSet();
                MANART_BAL.Activity objActivity = new MANART_BAL.Activity();;
                ds = objActivity.GetActivity(0, "CurrentActivity", "N", (sUserType == "D") ? 2 : 1);
                //dtDetails = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DisplayData(ds);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void NewRecord()
        {
            try
            {
                txtID.Text = "0";
                txtNameOfActivity.Text = "";
                txtToDate.Text = "";
                txtFromDate.Text = "";
                txtRemark.Text = "";
                //txtCostCenter.Text = "";
                txtGLAccount.Text = "";
                // txtObjective.Text = "";
                //drpDealerType.SelectedValue = "D";
                if (drpDepartmentalActivity.SelectedValue == "1")
                    ddlCategory.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlCategory.SelectedValue = "0";
                drpDepartmentalActivity.SelectedValue = "0";
                drpDirectClaim.SelectedValue = "0";
                drpTypeOfActivity.SelectedValue = "0";
                drpCreditorPositionKey.SelectedValue = "0";
                drpSpecialGL.SelectedValue = "0";
                drpEtbPostingKey.SelectedValue = "0";

                //drpCosrCenter.SelectedValue = "0";
                //drpAccount.SelectedValue = "0";
                EnableDisable("n");
                txtNameOfActivity.Focus();
                FillRegion();
                FillDepo();
                FillDealer();
                LocationDetails.Enabled = true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void ConfirmRecord(int iId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                if (iId > 0)
                {
                    objDB.BeginTranasaction();
                    string ssql = "Update M_ActivityMaster Set ActivityConfirm ='Y' where ID=" + iId;
                    objDB.ExecuteQuery(ssql);
                    objDB.CommitTransaction();
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    EnableDisable("Y");
                    return;

                }
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
        private void EnableDisable(string sConfirmStatus)
        {

            if (sConfirmStatus == "Y")
            {
                //drpActivityName.Style["display"] = "none";

                txtNameOfActivity.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                // txtObjective.Enabled = false;
                txtRemark.Enabled = false;
                drpDealerType.Enabled = false;
                drpDepartmentalActivity.Enabled = false;
                drpDirectClaim.Enabled = false;
                drpTypeOfActivity.Enabled = false;

                drpCreditorPositionKey.Enabled = false;
                drpSpecialGL.Enabled = false;
                drpEtbPostingKey.Enabled = false;
                //drpCosrCenter.Enabled = false;
                //drpAccount.Enabled = false;

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                LocationDetails.Enabled = false;


            }
            else
                if (sConfirmStatus == "N")
                {

                    //drpActivityName.Style["display"] = "none";
                    //txtNameOfActivity.Style["display"] = "";
                    txtNameOfActivity.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    txtRemark.Enabled = true;
                    drpDealerType.Enabled = true;
                    // txtObjective.Enabled = true;
                    drpDepartmentalActivity.Enabled = true;
                    drpDirectClaim.Enabled = true;
                    drpTypeOfActivity.Enabled = true;
                    //drpAccount.Enabled = true;

                    drpCreditorPositionKey.Enabled = true;
                    drpSpecialGL.Enabled = true;
                    drpEtbPostingKey.Enabled = true;
                    //drpCosrCenter.Enabled = true;
                    LocationDetails.Enabled = true;

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmSave);

                }
                else
                    if (sConfirmStatus == "n")
                    {

                        //drpActivityName.Style["display"] = "";
                        //txtNameOfActivity.Style["display"] = "none";
                        txtNameOfActivity.Enabled = true;
                        txtFromDate.Enabled = true;
                        txtToDate.Enabled = true;
                        txtRemark.Enabled = true;
                        drpDealerType.Enabled = true;
                        // txtObjective.Enabled = true;
                        drpDepartmentalActivity.Enabled = true;
                        drpDirectClaim.Enabled = true;
                        drpTypeOfActivity.Enabled = true;
                        //drpAccount.Enabled = true;

                        drpCreditorPositionKey.Enabled = true;
                        drpSpecialGL.Enabled = true;
                        drpEtbPostingKey.Enabled = true;
                        //drpCosrCenter.Enabled = true;

                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                        ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmSave);

                    }

        }

        protected void drpCreditorPositionKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCreditorPositionKey.SelectedValue == "2")
            {
                drpSpecialGL.Visible = true;
                drpSpecialGL.SelectedValue = "1";
                lblSpGL.Visible = true;
            }
            else
            {
                drpSpecialGL.SelectedValue = "0";
                drpSpecialGL.Visible = false;
                lblSpGL.Visible = false;
            }
        }

        #region Location
        protected void ChkAllReg_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAllReg.Checked == true)
            {
                for (int j = 0; j < ChkRegion.Items.Count; j++)
                {
                    ChkRegion.Items[j].Selected = true;
                }
            }
            else
            {
                for (int j = 0; j < ChkRegion.Items.Count; j++)
                {
                    ChkRegion.Items[j].Selected = false;
                }
            }
            FillDepo();
            FillDealer();

        }
        protected void ChkRegion_CheckedChanged(object sender, EventArgs e)
        {
            FillDepo();
            FillDealer();
        }

        protected void ChkAllDpo_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAllDpo.Checked == true)
            {
                for (int j = 0; j < ChkDepo.Items.Count; j++)
                {
                    ChkDepo.Items[j].Selected = true;
                }
                txtDealerName.Text = "All Selected";
            }
            else
            {
                for (int j = 0; j < ChkDepo.Items.Count; j++)
                {
                    ChkDepo.Items[j].Selected = false;
                }
                txtDealerName.Text = "--Select--";
            }
            FillDealer();

        }
        protected void ChkDepo_CheckedChanged(object sender, EventArgs e)
        {
            FillDealer();

        }
        protected void ChkDealer_CheckedChanged(object sender, EventArgs e)
        {
            txtDealerName.Text = "--Select--";
            int iCount = 0;
            for (int j = 0; j < ChkDealer.Items.Count; j++)
            {
                if (ChkDealer.Items[j].Selected == true)
                {
                    if (txtDealerName.Text == "--Select--")
                    {
                        txtDealerName.Text = ChkDealer.Items[j].Text;
                    }
                    else
                    {
                        txtDealerName.Text = txtDealerName.Text + "," + ChkDealer.Items[j].Text;
                    }
                }
                else
                    iCount = iCount + 1;
            }
            if (ChkDealer.Items.Count > 0)
                if (iCount > 0)
                {
                    ChkAllDlr.Checked = false;
                }
                else
                {
                    ChkAllDlr.Checked = true;
                    txtDealerName.Text = "All Selected";
                }

        }
        public void FillRegion()
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtRegion = new DataTable();
                dtRegion = objDB.ExecuteQueryAndGetDataTable("select M_Region.ID,M_Region.Region_Name as Name from  M_Region where M_Region.ID  in(select distinct(RegionId) from M_Sys_UserPermissions where userid =" + iUserId + " And IsActive=1)");
                ChkRegion.DataSource = dtRegion;
                ChkRegion.DataTextField = "Name";
                ChkRegion.DataValueField = "ID";
                ChkRegion.DataBind();
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

        private void FillDepo()
        {
            int iCount = 0;
            string sRegion = "";
            txtRegion.Text = "--Select--";
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                for (int i = 0; i < ChkRegion.Items.Count; i++)
                {
                    if (ChkRegion.Items[i].Selected == true)
                    {
                        if (sRegion == "")
                        {
                            sRegion = ChkRegion.Items[i].Value;
                            txtRegion.Text = ChkRegion.Items[i].Text;
                        }
                        else
                        {
                            sRegion = sRegion + "," + ChkRegion.Items[i].Value;
                            txtRegion.Text = txtRegion.Text + "," + ChkRegion.Items[i].Text;
                        }
                    }
                    else
                    {
                        iCount = iCount + 1;
                    }
                }
                ChkAllReg.Checked = false;
                if (ChkRegion.Items.Count > 0)
                    if (iCount > 0)
                        ChkAllReg.Checked = false;
                    else
                    {
                        ChkAllReg.Checked = true;
                        txtRegion.Text = "All Selected";
                    }

                DataTable dtState = new DataTable();
                if (sRegion != "")
                {
                    ChkDepo.Items.Clear();
                    if (iUsreTypeId == 2)//For Domestic VECV Users
                    {
                        dtState = objDB.ExecuteQueryAndGetDataTable("select M_State.ID,M_State.State AS Name from M_Sys_UserPermissions inner join M_State on M_Sys_UserPermissions.StateID=M_State.ID where RegionID in (" + sRegion + ") and UserID =" + iUserId + " and IsActive =1");

                    }
                    else if (iUsreTypeId == 1)//For Domestic  Dealer
                    {
                        dtState = objDB.ExecuteQueryAndGetDataTable("select M_Country.ID,M_Country.Country_Name AS Name from M_Sys_UserPermissions inner join M_Country  on M_Sys_UserPermissions.CountryId=M_Country.ID where RegionID in (" + sRegion + ") and UserID =" + iUserId + " and IsActive =1");
                    }
                }
                ChkDepo.DataSource = dtState;
                ChkDepo.DataTextField = "Name";
                ChkDepo.DataValueField = "ID";
                ChkDepo.DataBind();
                for (int j = 0; j < ChkDepo.Items.Count; j++)
                {
                    ChkDepo.Items[j].Selected = true;
                    txtDepoName.Text = "All Selected";
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
            txtDepoName.Text = "--Select--";
            txtDealerName.Text = "--Select--";
            string sDepo = "";
            int iCount = 0;
            try
            {
                for (int i = 0; i < ChkDepo.Items.Count; i++)
                {
                    if (ChkDepo.Items[i].Selected == true)
                    {
                        if (sDepo == "")
                        {
                            sDepo = ChkDepo.Items[i].Value;
                            txtDepoName.Text = ChkDepo.Items[i].Text;
                        }
                        else
                        {
                            sDepo = sDepo + "," + ChkDepo.Items[i].Value;
                            txtDepoName.Text = txtDepoName.Text + "," + ChkDepo.Items[i].Text;
                        }
                    }
                    else
                    {
                        iCount = iCount + 1;
                    }
                }
                if (ChkDepo.Items.Count > 0)
                {
                    if (iCount > 0)
                        ChkAllDpo.Checked = false;
                    else
                    {
                        ChkAllDpo.Checked = true;
                        txtDepoName.Text = "All Selected";
                    }
                }
                else
                    ChkAllDpo.Checked = false;
                DataTable dtDealer = new DataTable();
                clsUser objUser = new clsUser();

                if (sDepo != "")
                {
                    ChkDealer.Items.Clear();
                    dtDealer = objUser.GetDealerByStateORCountry(iUserId, sDepo, Func.Convert.iConvertToInt(ddlCategory.SelectedValue));//objDB.ExecuteQueryAndGetDataTable("select M_State.ID,M_State.State as Name from  M_State where M_State.ID in(Select distinct StateID from M_Sys_UserPermissions where IsActive=1 And (RegionID in (" + sRegion + ")) And UserID=" + iUserId + ")");

                }
                DataView dvDealer = dtDealer.DefaultView;
                if (dtDealer.Rows.Count > 0)
                    if (drpDepartmentalActivity.SelectedValue == "1")
                    {

                        DataColumn dcDealer = new DataColumn("NameAndCode");
                        dcDealer.Expression = string.Format("{0}+'('+{1} + ')'", "Name", "VCode");
                        dtDealer.Columns.Add(dcDealer);
                        dvDealer.RowFilter = "VCode<>''";
                    }
                    else
                    {
                        DataColumn dcDealer = new DataColumn("NameAndCode");
                        dcDealer.Expression = string.Format("{0}+'('+{1} + ')'", "Name", "SCode");
                        dtDealer.Columns.Add(dcDealer);
                        dvDealer.RowFilter = "SCode<>''";
                    }

                ChkDealer.DataSource = dvDealer.ToTable();
                ChkDealer.DataTextField = "NameAndCode";
                ChkDealer.DataValueField = "ID";
                ChkDealer.DataBind();
                for (int j = 0; j < ChkDealer.Items.Count; j++)
                {
                    ChkDealer.Items[j].Selected = true;
                    if (txtDealerName.Text == "--Select--")
                    {
                        txtDealerName.Text = ChkDealer.Items[j].Text;
                    }
                    else
                    {
                        txtDealerName.Text = txtDealerName.Text + "," + ChkDealer.Items[j].Text;
                    }
                }
                ChkAllDlr.Checked = false;
                if (ChkDealer.Items.Count > 0)
                {
                    ChkAllDlr.Checked = true;
                    txtDealerName.Text = "All Selected";
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void GetSelectedDepoAndDealer(DataTable dtDetails)
        {
            try
            {
                txtRegion.Text = "--Select--";
                FillRegion();
                if (dtDetails != null)
                    if (dtDetails.Rows.Count > 0)
                        for (int iData = 0; iData < dtDetails.Rows.Count; iData++)
                        {
                            for (int iRegion = 0; iRegion < ChkRegion.Items.Count; iRegion++)
                            {
                                if (Func.Convert.iConvertToInt(ChkRegion.Items[iRegion].Value) == Func.Convert.iConvertToInt(dtDetails.Rows[iData]["RegionID"]))
                                {
                                    ChkRegion.Items[iRegion].Selected = true;
                                }
                            }
                        }
                FillDepo();
                if (dtDetails != null)
                    if (dtDetails.Rows.Count > 0)
                        for (int iStateORCountry = 0; iStateORCountry < ChkDepo.Items.Count; iStateORCountry++)
                        {
                            int iCnt = 0;
                            for (int iData = 0; iData < dtDetails.Rows.Count; iData++)
                            {

                                if (Func.Convert.iConvertToInt(ChkDepo.Items[iStateORCountry].Value) == Func.Convert.iConvertToInt(dtDetails.Rows[iData]["StateIDORCountryID"]))
                                {
                                    ChkDepo.Items[iStateORCountry].Selected = true;
                                    iCnt = iCnt + 1;
                                }

                            }
                            if (iCnt == 0)
                            {
                                ChkDepo.Items[iStateORCountry].Selected = false;
                            }
                        }
                FillDealer();
                if (dtDetails != null)
                    if (dtDetails.Rows.Count > 0)
                        for (int iDealer = 0; iDealer < ChkDealer.Items.Count; iDealer++)
                        {
                            int iCnt = 0;
                            for (int iData = 0; iData < dtDetails.Rows.Count; iData++)
                            {
                                if (Func.Convert.iConvertToInt(ChkDealer.Items[iDealer].Value) == Func.Convert.iConvertToInt(dtDetails.Rows[iData]["Dealer_ID"]))
                                {
                                    ChkDealer.Items[iDealer].Selected = true;
                                    iCnt = iCnt + 1;
                                }
                            }
                            if (iCnt == 0)
                            {
                                ChkDealer.Items[iDealer].Selected = false;
                            }

                        }

                txtRegion.Text = "--Select--";
                txtDepoName.Text = "--Select--";
                txtDealerName.Text = "--Select--";
                //For Region
                int iCount = 0;
                for (int i = 0; i < ChkRegion.Items.Count; i++)
                {
                    if (ChkRegion.Items[i].Selected == false)
                        iCount = iCount + 1;
                    else
                    {
                        if (txtRegion.Text == "--Select--")
                            txtRegion.Text = ChkRegion.Items[i].Text;
                        else
                            txtRegion.Text = txtRegion.Text + "," + ChkRegion.Items[i].Text;
                    }


                }
                if (ChkRegion.Items.Count > 0)
                {
                    if (iCount > 0)
                        ChkAllReg.Checked = false;
                    else
                    {
                        ChkAllReg.Checked = true;
                        txtRegion.Text = "All Selected";
                    }
                }
                else
                    ChkAllReg.Checked = false;



                //For State Or Country
                iCount = 0;
                for (int i = 0; i < ChkDepo.Items.Count; i++)
                {
                    if (ChkDepo.Items[i].Selected == false)
                    {
                        iCount = iCount + 1;
                    }
                    else
                    {
                        if (txtDepoName.Text == "--Select--")
                            txtDepoName.Text = ChkDepo.Items[i].Text;
                        else
                            txtDepoName.Text = txtDepoName.Text + "," + ChkDepo.Items[i].Text;
                    }
                }
                if (ChkDepo.Items.Count > 0)
                {
                    if (iCount > 0)
                        ChkAllDpo.Checked = false;
                    else
                    {
                        ChkAllDpo.Checked = true;
                        txtDepoName.Text = "All Selected";
                    }
                }
                else
                    ChkAllDpo.Checked = false;


                //For Dealer            
                iCount = 0;
                for (int i = 0; i < ChkDealer.Items.Count; i++)
                {
                    if (ChkDealer.Items[i].Selected == false)
                    {
                        iCount = iCount + 1;
                    }
                    else
                    {
                        if (txtDealerName.Text == "--Select--")
                            txtDealerName.Text = ChkDealer.Items[i].Text;
                        else
                            txtDealerName.Text = txtDealerName.Text + "," + ChkDealer.Items[i].Text;
                    }
                }
                if (ChkDealer.Items.Count > 0)
                {
                    if (iCount > 0)
                        ChkAllDlr.Checked = false;
                    else
                    {
                        ChkAllDlr.Checked = true;
                        txtDealerName.Text = "All Selected";
                    }
                }
                else
                    ChkAllDlr.Checked = false;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion Location
        protected void GridSelectionDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtDetais = new DataTable();
            GridSelectionDetails.PageIndex = e.NewPageIndex;
            if (Session["ActivityDtls"] != null)
                dtDetais = (DataTable)Session["ActivityDtls"];
            GridSelectionDetails.DataSource = dtDetais;
            GridSelectionDetails.DataBind();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
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

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldrpTypeOfActivity();
            FillDealer();
        }
    }
}