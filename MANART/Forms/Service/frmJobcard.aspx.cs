using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Globalization;
using MANART_BAL;
using MANART_DAL;
using System.IO;


namespace MANART.Forms.Service
{
    public partial class frmJobcard : System.Web.UI.Page
    {
        //private DataTable dtDetails = new DataTable();
        private DataTable dtIncoDetails = new DataTable();
        private DataTable dtComplaint = new DataTable();
        private DataTable dtInvestigations = new DataTable();
        private DataTable dtPart = new DataTable();
        private DataTable dtLabour = new DataTable();
        private DataTable dtJob = new DataTable();
        private DataTable dtActionTaken = new DataTable();
        private DataTable dtParameter = new DataTable();
        private DataTable dtFreeService = new DataTable();
        private DataTable dtJbGrpTaxDetails = new DataTable();
        private DataTable dtJbTaxDetails = new DataTable();
        private DataTable dtFileAttach = new DataTable();

        //private DataTable dtLabourTime = new DataTable(); 
        clsSparePO objPO = null;

        private int iDocID = 0;
        int iDealerId;
        string sDealerCode;
        int iHOBranchDealerId;
        private bool bDetailsRecordExist = false;
        string sNew = "N";
        string sCreateGP = "";
        private int ichassisID = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            ToolbarC.bUseImgOrButton = true;
            //ToolbarC.iFormIdToOpenForm = 1;
            ToolbarC.iValidationIdForConfirm = 67;
            ToolbarC.iValidationIdForSave = 67;
            ToolbarC.iFormIdToOpenForm = 67;
            Location.bUseSpareDealerCode = true;
            Location.SetControlValue();
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);

            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                Session["DepartmentID"] = "7";
            }


            if (!IsPostBack)
            {
                Session["PartDetails"] = null;
                DisplayPreviousRecord();
            }
            SearchGrid.sGridPanelTitle = "Jobcard List";
            FillSelectionGrid();

            if (iDocID != 0)
            {
                GetDataAndDisplay();
            }
            if (txtID.Text == "")
            {
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            }
            SetDocumentDetails();

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillCombo();
                }
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
                if (!IsPostBack)
                {
                    Session["PartDetails"] = null;

                    DisplayPreviousRecord();

                }
               // Register JavaScript Function Vikram 02022018_Begin
                //Register in Page Load and After Grid Bind
                //if (PartDetailsGrid.Rows.Count > 0){
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + PartDetailsGrid.ClientID + "',DivHeaderRow_part,DivMainContent_part, 400, 1250 , 40 ,false); </script>", false);
                //}
                //if (LabourDetailsGrid.Rows.Count > 0) {
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key1", "<script>MakeStaticHeader('" + LabourDetailsGrid.ClientID + "',DivHeaderRow_labour,DivMainContent_labour, 400, 1250 , 40 ,false); </script>", false);
                //}
                //END

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {

            FillCombo();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            ClearFormControl();
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (txtUserType.Text == "6")
            {
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
                if (objCommon.sUserRole == "15")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }
                else if (objCommon.sUserRole == "19")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                    btnJobConfirm.Enabled = false;
                    btnJobSave.Enabled = false;
                    BtnOpen.Enabled = false;
                    btnPO.Enabled = false;
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
        // set Document text 
        private void SetDocumentDetails()
        {
            lblTitle.Text = "Jobcard";
            lblDocNo.Text = "Job No.:";
            lblDocDate.Text = "Job Date:";
            if (txtUserType.Text == "6")
            {
                lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Location.iDealerId.ToString() + "','" + Session["DepartmentID"].ToString() + "')");
            }
            else
            {
                lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Session["HOBR_ID"].ToString() + "','" + Session["DepartmentID"].ToString() + "')");
            }
        }

        // FillCombo
        private void FillCombo()
        {
            if (Location.iDealerId != 0)
            {
                if (txtUserType.Text == "6")
                {
                    Func.Common.BindDataToCombo(DrpBay, clsCommon.ComboQueryType.BayAllocation, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString());
                    Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString() + " and Empl_Type=1");
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpBay, clsCommon.ComboQueryType.BayAllocation, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type=1");
                }
            }
            Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "')");
            Func.Common.BindDataToCombo(DrpInsurnceComp, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "') and CM.Cust_Type in (Select Id from M_CustType where DMS_type_flag='IC' and Cust_Sup='C')");            
            //Func.Common.BindDataToCombo(drpJobType, clsCommon.ComboQueryType.JobcardType, 0);
            Func.Common.BindDataToCombo(drpJobType, clsCommon.ComboQueryType.JobcardType, 0, (txtDealerCode.Text.Trim().StartsWith("R") ? " and ID in (3,10,14)" : ""));
            Func.Common.BindDataToCombo(DrpDelayReason, clsCommon.ComboQueryType.DelayReason, 0);
            Func.Common.BindDataToCombo(DrpVehSaleDealer, clsCommon.ComboQueryType.VehSaleDealer, 0, " and Dealer_Origin='D'");
            Func.Common.BindDataToCombo(DrpModelCode, clsCommon.ComboQueryType.ModelCodeForJobcard, 0);
            Func.Common.BindDataToCombo(DrpModelName, clsCommon.ComboQueryType.ModelNameForJobcard, 0);
        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    DisplayPreviousRecord();
                    lnkRequestHV.Visible = false;
                    lnkRequestGD.Visible = false;
                    lnkSrvVAN.Visible = false;
                    lblServiceHistroy.Style.Add("display", "none");
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    txtChkfun.Text = "false";
                    if (bSaveRecord(false, false) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    txtChkfun.Text = "true";
                    if (bSaveRecord(true, false) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (bSaveRecord(false, true) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // To Display Previous Record
        private void DisplayPreviousRecord()
        {
            try
            {
                clsJobcard ObjJobcard = new clsJobcard();
                DataSet ds = new DataSet();
                //Please remove this Temporary comment

                iDealerId = Location.iDealerId;
                sDealerCode = Location.sDealerCode;
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);

                if (txtUserType.Text == "6")
                {

                    iHOBranchDealerId = Location.iDealerId;
                }

                //iDealerId = 158;// Entry taken from TM_DealerHOBRData for 158
                //sDealerCode = "1S3045";
                if (txtUserType.Text == "6")
                {
                    Func.Common.BindDataToCombo(DrpBay, clsCommon.ComboQueryType.BayAllocation, Location.iDealerId, " and HOBrID=" + iHOBranchDealerId.ToString());
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpBay, clsCommon.ComboQueryType.BayAllocation, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                }
                ds = ObjJobcard.GetJobcard(iDocID, "New", iDealerId, iHOBranchDealerId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["job_canc_tag"] = "N";
                            ds.Tables[0].Rows[0]["job_confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";                        
                            sNew = "Y";
                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "R", iDealerId);
                hdnTrNo.Value = Location.sDealerCode + "/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false); 
                txtDocDate.Text = Func.Common.sGetCurrentDateTime(Location.iCountryId, true);
                dtpJobCommited.Text = "";

                dtpVehInTime.Text = Func.Convert.sConvertToString(txtDocDate.Text);
                dtpJobOpeningTm.Text = Func.Convert.sConvertToString(txtDocDate.Text);
                DtpVehicleOut.Text = Func.Convert.sConvertToString(txtDocDate.Text);
                dtpAllocateTime.Text = Func.Convert.sConvertToString(txtDocDate.Text);

                //ichassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_ID"]);

                //txtReportChassisID.Text = Func.Convert.sConvertToString(ichassisID);

                if (Session["DepartmentID"].ToString() == "7") lblSelectModel.Visible = true;
                txtApPartAmt.Text = "";
                txtApLabAmt.Text = "";
                txtApLubAmt.Text = "";
                txtApMiscAmt.Text = "";

                BindDataToPartGrid(true, 0);
                BindDataToLaborGrid(true, 0);

                ObjJobcard = null;
                ds = null;
                ObjJobcard = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int iNewRowToAdd = Func.Convert.iConvertToInt(txtNewRecountCount.Text);
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    if ((sender as GridView).ID == "ComplaintsGrid")//Complaints Grid
                    {
                        bFillDetailsFromComplaintGrid();
                        //BindDataToComplaintGrid(true, iNewRowToAdd);
                        CreateNewRowToComplaintGrid();
                        Session["ComplaintsDetails"] = dtComplaint;
                        BindDataToComplaintGrid();

                    }
                    else if ((sender as GridView).ID == "InvestigationsGrid")//InvestigationsGrid
                    {
                        //bFillDetailsFromInvestigationsGrid();
                        //BindDataToInvestigationsGrid(true, iNewRowToAdd);
                        bFillDetailsFromInvestigationsGrid();
                        CreateNewRowToInvestigationsGrid();
                        Session["InvestigationDetails"] = dtInvestigations;
                        BindDataToInvestigationsGrid();
                    }
                    else if ((sender as GridView).ID == "PartDetailsGrid")//PartDetails Grid
                    {
                        bFillDetailsFromPartGrid();
                        BindDataToPartGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "LabourDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromLabourGrid();
                        BindDataToLaborGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "ActionsGrid")//Action Grid
                    {
                        //bFillDetailsFromActionGrid();
                        //BindDataToActionTakenGrid();

                        bFillDetailsFromActionGrid();
                        CreateNewRowToActionGrid();
                        Session["ActionDetails"] = dtActionTaken;
                        BindDataToActionTakenGrid();
                    }
                    //else if ((sender as GridView).ID == "SubletDetailsGrid")//Labour Grid
                    //{
                    //    bFillDetailsFromSubletGrid();
                    //    BindDataToSubletGrid(true, iNewRowToAdd);
                    //}
                    else if ((sender as GridView).ID == "JobDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromJobGrid("N");
                        BindDataToJobGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        #region  Complaints Function
        // Create Row To Complaint Grid    
        private void CreateNewRowToComplaintGrid()
        {
            try
            {
                DataRow dr;

                dr = dtComplaint.NewRow();
                dr["ID"] = 0;
                dr["Complaint_ID"] = 0;
                dr["Complaint_Desc"] = "";
                dr["Status"] = "N";
                dtComplaint.Rows.Add(dr);
                dtComplaint.AcceptChanges();


            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Complaint Grid    
        private void BindDataToComplaintGrid()
        {
            try
            {
                if (Session["ComplaintsDetails"] == null)
                {
                    CreateNewRowToComplaintGrid();
                    Session["ComplaintsDetails"] = dtComplaint;

                }
                else
                {
                    dtComplaint = (DataTable)Session["ComplaintsDetails"];
                    if (dtComplaint.Rows.Count == 0)
                    {
                        CreateNewRowToComplaintGrid();

                    }
                }

                ComplaintsGrid.DataSource = dtComplaint;
                ComplaintsGrid.DataBind();

                SetControlPropertyToComplaintGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // Set Control property To Complaint Grid    
        private void SetControlPropertyToComplaintGrid()
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sComplaintID = "";
                for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
                {
                    //Complaint Description            
                    TextBox txtNewComplaintDesc = (TextBox)ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc");

                    //Complaint 
                    DropDownList drpComplaint = (DropDownList)ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint");
                    Func.Common.BindDataToCombo(drpComplaint, clsCommon.ComboQueryType.CustomerComplaints, Location.iDealerId);

                    sRecordStatus = "N";

                    if (idtRowCnt < dtComplaint.Rows.Count)
                    {
                        txtNewComplaintDesc.Text = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_Desc"]);
                        sComplaintID = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_ID"]);
                        // Add New Complaint
                        if (sComplaintID == "9999" || sComplaintID == "0")
                        {
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpComplaint.Items.Add(lstitm);
                            drpComplaint.Attributes.Add("onChange", "OnComplaintValueChange(event, this,'" + txtNewComplaintDesc.ID + "')");
                        }
                        else
                        {
                            drpComplaint.Attributes.Add("onChange", "return CheckComplaintSelected(event,this);");
                        }
                        drpComplaint.SelectedValue = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_ID"]);
                        sRecordStatus = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;

                    }
                    if (sComplaintID == "9999")
                    {
                        txtNewComplaintDesc.Style.Add("display", "");
                        drpComplaint.Style.Add("display", "none");
                    }
                    else
                    {
                        drpComplaint.Style.Add("display", "");
                        txtNewComplaintDesc.Style.Add("display", "none");
                    }

                    //New 
                    LinkButton lnkNew = (LinkButton)ComplaintsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)ComplaintsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)ComplaintsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Attributes.Add("onClick", "SelectDeleteCheckboxCommon(this)");
                    //Chk.Text = "Delete";
                    Chk.Attributes["align"] = "center";
                    Chk.Style.Add("display", "none");
                    lnkNew.Style.Add("display", "none");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        ComplaintsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        drpComplaint.Enabled = false;
                    }
                    else
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                        // Allow New To Last Row
                        if ((iRowCnt + 1) == ComplaintsGrid.Rows.Count)
                        {
                            lnkNew.Style.Add("display", "");
                        }
                    }
                }
                //lblComplaintsRecCnt.Text = idtRowCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Complaint Grid
        private bool bFillDetailsFromComplaintGrid()
        {
            string sStatus = "";
            dtComplaint = (DataTable)Session["ComplaintsDetails"];
            int iCntForDelete = 0;
            int iComplaintID = 0;
            string sComplainDesc = "";
            bool bValidate = false;
            for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
            {
                //ComplaintID                
                iComplaintID = Func.Convert.iConvertToInt((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedValue);
                //sComplainDesc = (ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc") as TextBox).Text.ToString();

                if (Func.Convert.sConvertToString((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedItem) == "NEW")
                    sComplainDesc = (ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc") as TextBox).Text.ToString();
                else
                    sComplainDesc = Func.Convert.sConvertToString((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedItem);

                if (iComplaintID != 0 && sComplainDesc != "")
                {
                    bValidate = true;

                    dtComplaint.Rows[iRowCnt]["Complaint_ID"] = iComplaintID;

                    //Complaint Description
                    if (iComplaintID == Func.Convert.iConvertToInt("9999") && sComplainDesc != "")
                        dtComplaint.Rows[iRowCnt]["Complaint_Desc"] = sComplainDesc;

                    dtComplaint.Rows[iRowCnt]["Complaint_Desc"] = sComplainDesc;
                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Status"]);

                    dtComplaint.Rows[iRowCnt]["Status"] = "S";// for Save                

                    CheckBox Chk = (CheckBox)ComplaintsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtComplaint.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete == ComplaintsGrid.Rows.Count)
            {
                bValidate = false;
            }

            // Sujata 19012011
            if (txtChkfun.Text == "false") bValidate = true;
            // Sujata 19012011

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the complaints.');</script>");
            }
        Last:
            return bValidate;
        }

        //To Get Total Complaint Count
        private void CalculateComplaintGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sComplaintID = "";
                for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
                {
                    //Complaint Description            
                    TextBox txtNewComplaintDesc = (TextBox)ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc");

                    //Complaint 
                    DropDownList drpComplaint = (DropDownList)ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint");
                    sComplaintID = drpComplaint.SelectedValue;
                    if (sComplaintID != "0")
                    {
                        if (sComplaintID != "9999")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                        else if (txtNewComplaintDesc.Text != "")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                    }
                }
                lblComplaintsRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        #endregion

        #region Investigations Function
        // Create Row To Part Grid
        private void CreateNewRowToInvestigationsGrid()
        {
            try
            {
                //    DataRow dr;
                //    DataTable dtDefaultInvestigations = new DataTable();
                //    int iRowCntStartFrom = 0;
                //    int iMaxInvestigationsGridRowCount = 0;
                //    iMaxInvestigationsGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxInvestigationsGridRowCount"]);
                //    if (Session["InvestigationDetails"] != null)
                //    {
                //        dtDefaultInvestigations = (DataTable)Session["InvestigationDetails"];
                //    }
                //    else
                //    {
                //        dtDefaultInvestigations = dtInvestigations;
                //    }
                //    if (iNoRowToAdd == 0)
                //    {
                //        if (dtDefaultInvestigations.Rows.Count == 0)
                //        {
                //            dtDefaultInvestigations.Columns.Clear();
                //            dtDefaultInvestigations.Columns.Add(new DataColumn("ID", typeof(int)));
                //            dtDefaultInvestigations.Columns.Add(new DataColumn("Investigation_ID", typeof(int)));
                //            dtDefaultInvestigations.Columns.Add(new DataColumn("Investigation_Desc", typeof(string)));
                //            dtDefaultInvestigations.Columns.Add(new DataColumn("Status", typeof(string)));
                //        }
                //        else
                //        {
                //            if (dtDefaultInvestigations.Rows.Count >= iMaxInvestigationsGridRowCount) goto Bind;
                //        }
                //    }
                //    else
                //    {
                //        iRowCntStartFrom = iMaxInvestigationsGridRowCount;
                //    }

                //    iMaxInvestigationsGridRowCount = iMaxInvestigationsGridRowCount + iNoRowToAdd;

                //    for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxInvestigationsGridRowCount; iRowCnt++)
                //    {
                //        dr = dtDefaultInvestigations.NewRow();
                //        dr["ID"] = 0;
                //        dr["Investigation_ID"] = 0;
                //        dr["Investigation_Desc"] = "";
                //        dr["Status"] = "N";
                //        dtDefaultInvestigations.Rows.Add(dr);
                //        dtDefaultInvestigations.AcceptChanges();
                //    }

                //Bind:
                //    Session["InvestigationDetails"] = dtDefaultInvestigations;
                //    InvestigationsGrid.DataSource = dtDefaultInvestigations;
                //    InvestigationsGrid.DataBind();

                DataRow dr;

                dr = dtInvestigations.NewRow();
                dr["ID"] = 0;
                dr["Investigation_ID"] = 0;
                dr["Investigation_Desc"] = "";
                dr["Status"] = "N";
                dtInvestigations.Rows.Add(dr);
                dtInvestigations.AcceptChanges();


            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Investigations Grid
        private void BindDataToInvestigationsGrid()
        {
            try
            {
                //if (bRecordIsOpen == true)
                //{
                //    CreateNewRowToInvestigationsGrid(iNoRowToAdd);
                //    SetControlPropertyToInvestigationsGrid(bRecordIsOpen);
                //}
                //else
                //{
                //    InvestigationsGrid.DataSource = dtInvestigations;
                //    InvestigationsGrid.DataBind();
                //    SetControlPropertyToInvestigationsGrid(bRecordIsOpen);
                //}

                if (Session["InvestigationDetails"] == null)
                {
                    CreateNewRowToInvestigationsGrid();
                    Session["InvestigationDetails"] = dtInvestigations;

                }
                else
                {
                    dtInvestigations = (DataTable)Session["InvestigationDetails"];
                    if (dtInvestigations.Rows.Count == 0)
                    {
                        CreateNewRowToInvestigationsGrid();

                    }
                }

                InvestigationsGrid.DataSource = dtInvestigations;
                InvestigationsGrid.DataBind();

                SetControlPropertyToInvestigationsGrid(false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToInvestigationsGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                string sInvestigationID = "";
                for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
                {
                    //Investigations Description            
                    TextBox txtNewInvestigationDesc = (TextBox)InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc");


                    //Investigations 
                    DropDownList drpInvestigations = (DropDownList)InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation");
                    Func.Common.BindDataToCombo(drpInvestigations, clsCommon.ComboQueryType.DealerInvistigation, Location.iDealerId);

                    if (idtRowCnt < dtInvestigations.Rows.Count)
                    {
                        txtNewInvestigationDesc.Text = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_Desc"]);
                        //drpInvestigations.SelectedValue = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_ID"]);
                        sInvestigationID = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_ID"]);

                        // Add New Investigations  
                        if (sInvestigationID == "0" || sInvestigationID == "9999")
                        {
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpInvestigations.Items.Add(lstitm);
                            drpInvestigations.Attributes.Add("onChange", "return OnInvestigationValueChange(event, this,'" + txtNewInvestigationDesc.ID + "')");
                        }
                        else
                        {
                            drpInvestigations.Attributes.Add("onChange", "return CheckInvestigationSelected(event,this);)");
                        }
                        drpInvestigations.SelectedValue = sInvestigationID;
                        sRecordStatus = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;
                    }
                    if (sInvestigationID == "9999")
                    {
                        txtNewInvestigationDesc.Style.Add("display", "");
                        drpInvestigations.Style.Add("display", "none");
                    }
                    else
                    {
                        drpInvestigations.Style.Add("display", "");
                        txtNewInvestigationDesc.Style.Add("display", "none");
                    }

                    //New 
                    LinkButton lnkNew = (LinkButton)InvestigationsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)InvestigationsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)InvestigationsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Attributes.Add("onClick", "SelectDeleteCheckboxCommon(this)");
                    Chk.Text = "Delete";
                    Chk.Attributes["align"] = "center";
                    Chk.Style.Add("display", "none");
                    lnkNew.Style.Add("display", "none");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        ComplaintsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        drpInvestigations.Enabled = false;
                    }
                    else
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                        // Allow New To Last Row
                        if ((iRowCnt + 1) == InvestigationsGrid.Rows.Count)
                        {
                            lnkNew.Style.Add("display", "");
                        }
                    }

                    //if (sRecordStatus == "U")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}

                    //if (sRecordStatus == "N")
                    //{
                    //    //lnkNew.Style.Add("display", "");                
                    //}
                    //else if (sRecordStatus == "D")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    InvestigationsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    //}
                    //else if (sRecordStatus == "E")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                    //// Allow New To Last Row
                    //if ((iRowCnt + 1) == InvestigationsGrid.Rows.Count)
                    //{

                    //    lnkNew.Style.Add("display", "");

                    //}
                    //if (bRecordIsOpen == false)
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            //lblInvestigationsRecCnt.Text = Func.Common.sRowCntOfTable(dtInvestigations);
        }

        //Fill Details From Investigations Grid
        private bool bFillDetailsFromInvestigationsGrid()
        {
            string sStatus = "";
            dtInvestigations = (DataTable)Session["InvestigationDetails"];
            int iCntForDelete = 0;
            int iInvestigationID = 0;
            string sInvestigationDesc = "";
            bool bValidate = false;
            for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
            {
                //InvestigationID                
                iInvestigationID = Func.Convert.iConvertToInt((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedValue);
                //sInvestigationDesc = (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;
                if (Func.Convert.sConvertToString((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedItem) == "NEW")
                    sInvestigationDesc = (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;
                else
                    sInvestigationDesc = Func.Convert.sConvertToString((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedItem);

                if (iInvestigationID != 0 && sInvestigationDesc != "")
                {
                    bValidate = true;

                    //if (txtRefClaimID.Text != "")
                    //dtInvestigations.Rows[iRowCnt]["ID"] = 0;

                    // Investigations ID
                    //if (iInvestigationID != Func.Convert.iConvertToInt("9999"))
                    dtInvestigations.Rows[iRowCnt]["Investigation_ID"] = iInvestigationID;

                    //Investigations Description
                    if (iInvestigationID == Func.Convert.iConvertToInt("9999"))
                        dtInvestigations.Rows[iRowCnt]["Investigation_Desc"] = sInvestigationDesc;// (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;

                    dtInvestigations.Rows[iRowCnt]["Investigation_Desc"] = sInvestigationDesc;

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Status"]);

                    dtInvestigations.Rows[iRowCnt]["Status"] = "S";// for Save                

                    CheckBox Chk = (CheckBox)InvestigationsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtInvestigations.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete == InvestigationsGrid.Rows.Count)
            {
                bValidate = false;
            }

            // Sujata 19012011
            if (txtChkfun.Text == "false") bValidate = true;
            // Sujata 19012011

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the dealar investigation.');</script>");
            }
            return bValidate;
        }
        //To Get Total Invetigation Count
        private void CalculateInvestigationGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sInvestigationID = "";
                for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
                {
                    //Investigation Description            
                    TextBox txtNewInvestigationDesc = (TextBox)InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc");

                    //Investigation 
                    DropDownList drpInvestigation = (DropDownList)InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation");
                    sInvestigationID = drpInvestigation.SelectedValue;
                    if (sInvestigationID != "0")
                    {
                        if (sInvestigationID != "9999")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                        else if (txtNewInvestigationDesc.Text != "")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                    }
                }
                lblInvestigationsRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region ActionTakenFunction
        // Create Row To Action Grid
        private void CreateNewRowToActionGrid()
        {
            try
            {
                DataRow dr;
                //DataTable dtDefaultAction = new DataTable();
                //int iRowCntStartFrom = 0;
                //int iMaxActionGridRowCount = 0;
                //iMaxActionGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxActionGridRowCount"]);
                //if (Session["ActionDetails"] != null)
                //{
                //    dtDefaultAction = (DataTable)Session["ActionDetails"];
                //}
                //else
                //{
                //    dtDefaultAction = dtActionTaken;
                //}
                //if (iNoRowToAdd == 0)
                //{
                //    if (dtDefaultAction.Rows.Count == 0)
                //    {
                //        dtDefaultAction.Columns.Clear();
                //        dtDefaultAction.Columns.Add(new DataColumn("ID", typeof(int)));
                //        dtDefaultAction.Columns.Add(new DataColumn("Action_ID", typeof(int)));
                //        dtDefaultAction.Columns.Add(new DataColumn("Dealer_Action", typeof(string)));
                //        dtDefaultAction.Columns.Add(new DataColumn("Status", typeof(string)));
                //    }
                //    else
                //    {
                //        if (dtDefaultAction.Rows.Count >= iMaxActionGridRowCount) goto Bind;
                //    }
                //}
                //else
                //{
                //    iRowCntStartFrom = iMaxActionGridRowCount;
                //}

                //iMaxActionGridRowCount = iMaxActionGridRowCount + iNoRowToAdd;

                //for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxActionGridRowCount; iRowCnt++)
                //{
                dr = dtActionTaken.NewRow();
                dr["ID"] = 0;
                dr["Action_ID"] = 0;
                dr["Dealer_Action"] = "";
                dr["Status"] = "N";
                dtActionTaken.Rows.Add(dr);
                dtActionTaken.AcceptChanges();
                //}

                //Bind:
                //    Session["ActionDetails"] = dtDefaultAction;
                //    ActionsGrid.DataSource = dtDefaultAction;
                //    ActionsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Action Grid
        private void BindDataToActionTakenGrid()
        {
            try
            {
                //if (bRecordIsOpen == true)
                //{
                //    CreateNewRowToActionGrid(iNoRowToAdd);
                //    SetControlPropertyToActionGrid(bRecordIsOpen);
                //}
                //else
                //{
                //    ActionsGrid.DataSource = dtActionTaken;
                //    ActionsGrid.DataBind();
                //    SetControlPropertyToActionGrid(bRecordIsOpen);
                //}

                if (Session["ActionDetails"] == null)
                {
                    CreateNewRowToActionGrid();
                    Session["ActionDetails"] = dtActionTaken;

                }
                else
                {
                    dtActionTaken = (DataTable)Session["ActionDetails"];
                    if (dtActionTaken.Rows.Count == 0)
                    {
                        CreateNewRowToActionGrid();

                    }
                }

                ActionsGrid.DataSource = dtActionTaken;
                ActionsGrid.DataBind();

                SetControlPropertyToActionGrid(false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToActionGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "";
                string sActionID = "";
                int idtRowCnt = 0;
                for (int iRowCnt = 0; iRowCnt < ActionsGrid.Rows.Count; iRowCnt++)
                {
                    //Action Description            
                    TextBox txtNewActionDesc = (TextBox)ActionsGrid.Rows[iRowCnt].FindControl("txtNewActionDesc");


                    //Action
                    DropDownList drpAction = (DropDownList)ActionsGrid.Rows[iRowCnt].FindControl("drpAction");
                    Func.Common.BindDataToCombo(drpAction, clsCommon.ComboQueryType.Dealer_ActionTaken, Location.iDealerId);

                    if (idtRowCnt < dtActionTaken.Rows.Count)
                    {
                        txtNewActionDesc.Text = Func.Convert.sConvertToString(dtActionTaken.Rows[iRowCnt]["Dealer_Action"]);
                        sActionID = Func.Convert.sConvertToString(dtActionTaken.Rows[iRowCnt]["Action_ID"]);
                        // Add New Complaint
                        if (sActionID == "9999" || sActionID == "0")
                        {
                            // Add New Action
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpAction.Items.Add(lstitm);
                            drpAction.Attributes.Add("onChange", "OnActionValueChange(event, this,'" + txtNewActionDesc.ID + "')");
                        }
                        else
                        {
                            drpAction.Attributes.Add("onChange", "return CheckActionSelected(event,this);");
                        }
                        drpAction.SelectedValue = Func.Convert.sConvertToString(dtActionTaken.Rows[iRowCnt]["Action_ID"]);
                        sRecordStatus = Func.Convert.sConvertToString(dtActionTaken.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;
                    }

                    if (sActionID == "9999")
                    {
                        txtNewActionDesc.Style.Add("display", "");
                        drpAction.Style.Add("display", "none");
                    }
                    else
                    {
                        drpAction.Style.Add("display", "");
                        txtNewActionDesc.Style.Add("display", "none");
                    }
                    //txtNewActionDesc.Style.Add("display", "none");
                    //if (txtNewInvestigationDesc.Text != "")
                    //{
                    //    txtNewInvestigationDesc.Style.Add("display", "");
                    //    drpInvestigations.Style.Add("display", "none");
                    //}

                    //New 
                    LinkButton lnkNew = (LinkButton)ActionsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)ActionsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)ActionsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Attributes.Add("onClick", "SelectDeleteCheckboxCommon(this)");
                    Chk.Text = "Delete";
                    Chk.Attributes["align"] = "center";
                    Chk.Style.Add("display", "none");
                    lnkNew.Style.Add("display", "none");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        ActionsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        drpAction.Enabled = false;
                    }
                    else
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                        // Allow New To Last Row
                        if ((iRowCnt + 1) == ActionsGrid.Rows.Count)
                        {
                            lnkNew.Style.Add("display", "");
                        }
                    }
                    //if (sRecordStatus == "U")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}

                    //if (sRecordStatus == "N")
                    //{
                    //    //lnkNew.Style.Add("display", "");                
                    //}
                    //else if (sRecordStatus == "D")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    ActionsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    //}
                    //else if (sRecordStatus == "E")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                    //// Allow New To Last Row
                    //if ((iRowCnt + 1) == ActionsGrid.Rows.Count)
                    //{

                    //    lnkNew.Style.Add("display", "");

                    //}
                    //if (bRecordIsOpen == false)
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            //lblInvestigationsRecCnt.Text = Func.Common.sRowCntOfTable(dtInvestigations);
        }

        //Fill Details From Action Grid
        private bool bFillDetailsFromActionGrid()
        {
            string sStatus = "";
            dtActionTaken = (DataTable)Session["ActionDetails"];
            int iCntForDelete = 0;
            int iActionID = 0;
            string sActionDesc = "";
            bool bValidate = false;
            for (int iRowCnt = 0; iRowCnt < ActionsGrid.Rows.Count; iRowCnt++)
            {
                //ActionID                
                iActionID = Func.Convert.iConvertToInt((ActionsGrid.Rows[iRowCnt].FindControl("drpAction") as DropDownList).SelectedValue);
                //sActionDesc = (ActionsGrid.Rows[iRowCnt].FindControl("txtNewActionDesc") as TextBox).Text;
                if (Func.Convert.sConvertToString((ActionsGrid.Rows[iRowCnt].FindControl("drpAction") as DropDownList).SelectedItem) == "NEW")
                    sActionDesc = (ActionsGrid.Rows[iRowCnt].FindControl("txtNewActionDesc") as TextBox).Text;
                else
                    sActionDesc = Func.Convert.sConvertToString((ActionsGrid.Rows[iRowCnt].FindControl("drpAction") as DropDownList).SelectedItem);

                if (iActionID != 0 && sActionDesc != "")
                {
                    bValidate = true;

                    //if (txtRefClaimID.Text != "")
                    //dtActionTaken.Rows[iRowCnt]["ID"] = 0;                

                    // Action ID
                    //if (iActionID != Func.Convert.iConvertToInt("9999"))
                    dtActionTaken.Rows[iRowCnt]["Action_ID"] = iActionID;

                    //Action Description
                    if (iActionID == Func.Convert.iConvertToInt("9999"))
                        dtActionTaken.Rows[iRowCnt]["Dealer_Action"] = sActionDesc;// (ActionsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;

                    dtActionTaken.Rows[iRowCnt]["Dealer_Action"] = sActionDesc;
                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtActionTaken.Rows[iRowCnt]["Status"]);

                    dtActionTaken.Rows[iRowCnt]["Status"] = "S";// for Save                

                    CheckBox Chk = (CheckBox)ActionsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtActionTaken.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete == ActionsGrid.Rows.Count)
            {
                bValidate = false;
            }

            // Sujata 19012011
            if (txtChkfun.Text == "false") bValidate = true;
            // Sujata 19012011

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the dealar Action Taken.');</script>");
            }
            return bValidate;
        }

        //To Get Total Action Count
        private void CalculateActionGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sActionID = "";
                for (int iRowCnt = 0; iRowCnt < ActionsGrid.Rows.Count; iRowCnt++)
                {
                    //Action Description            
                    TextBox txtNewActionDesc = (TextBox)ActionsGrid.Rows[iRowCnt].FindControl("txtNewActionDesc");

                    //Action 
                    DropDownList drpAction = (DropDownList)ActionsGrid.Rows[iRowCnt].FindControl("drpAction");
                    sActionID = drpAction.SelectedValue;
                    if (sActionID != "0")
                    {
                        if (sActionID != "9999")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                        else if (txtNewActionDesc.Text != "")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                    }
                }
                lblActionsRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        #endregion

        #region Parts Function
        // Create Row To Part Grid
        private void CreateNewRowToPartGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultPart = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxPartGridRowCount = 0;
                iMaxPartGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxPartGridRowCount"]);

                if (Session["PartDetails"] != null)
                {
                    dtDefaultPart = (DataTable)Session["PartDetails"];
                }
                else
                {
                    dtDefaultPart = dtPart;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultPart.Rows.Count == 0)
                    {
                        dtDefaultPart.Columns.Clear();
                        dtDefaultPart.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDefaultPart.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("part_type_tag", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("parts_no", typeof(string)));

                        dtDefaultPart.Columns.Add(new DataColumn("IPO_no", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Mech_ID", typeof(int)));

                        dtDefaultPart.Columns.Add(new DataColumn("ReqQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("RetQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("IssueQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("UseQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("BillQty", typeof(double)));

                        dtDefaultPart.Columns.Add(new DataColumn("war_tag", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("foc_tag", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("FOCQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("foc_reason_ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("FSCQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("PDIQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("AMCQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("CampaignQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("transitQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("EnRouteTechQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("EnrouteNonTechQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("SpWarQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("GoodWlQty", typeof(double)));

                        dtDefaultPart.Columns.Add(new DataColumn("WarrQty", typeof(double)));

                        dtDefaultPart.Columns.Add(new DataColumn("PrePDIQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("AggregateQty", typeof(double)));

                        dtDefaultPart.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Total", typeof(double)));

                        dtDefaultPart.Columns.Add(new DataColumn("JobcodeID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("MakeID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("LubLocID", typeof(int)));


                        //dtDefaultPart.Columns.Add(new DataColumn("Failed_Make", typeof(string)));                    
                        dtDefaultPart.Columns.Add(new DataColumn("Status", typeof(string)));


                    }
                    else
                    {
                        if (dtDefaultPart.Rows.Count >= iMaxPartGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxPartGridRowCount;
                }

                iMaxPartGridRowCount = iMaxPartGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxPartGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultPart.NewRow();
                    dr["SRNo"] = "1";
                    dr["ID"] = 0;
                    dr["part_type_tag"] = "P";
                    dr["PartLabourID"] = 0;
                    dr["parts_no"] = "";

                    dr["IPO_no"] = 0;
                    dr["Mech_ID"] = 0;

                    dr["ReqQty"] = 0;
                    dr["RetQty"] = 0;
                    dr["IssueQty"] = 0;
                    dr["UseQty"] = 0;
                    dr["BillQty"] = 0;

                    dr["war_tag"] = "N";
                    dr["foc_tag"] = "N";
                    dr["FOCQty"] = 0;
                    dr["foc_reason_ID"] = 0;
                    dr["FSCQty"] = 0;
                    dr["PDIQty"] = 0;
                    dr["AMCQty"] = 0;
                    dr["CampaignQty"] = 0;
                    dr["transitQty"] = 0;
                    dr["EnRouteTechQty"] = 0;
                    dr["EnrouteNonTechQty"] = 0;
                    dr["SpWarQty"] = 0;
                    dr["GoodWlQty"] = 0;

                    dr["WarrQty"] = 0;
                    dr["PrePDIQty"] = 0;
                    dr["AggregateQty"] = 0;

                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["JobcodeID"] = 0;
                    dr["MakeID"] = 0;
                    dr["LubLocID"] = 0;

                    dr["Status"] = "S";
                    dtDefaultPart.Rows.Add(dr);
                    dtDefaultPart.AcceptChanges();
                }
            Bind:
                Session["PartDetails"] = dtDefaultPart;
                PartDetailsGrid.DataSource = dtDefaultPart;
                PartDetailsGrid.DataBind();
                //Vikram 30012018
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + PartDetailsGrid.ClientID + "',DivHeaderRow_part,DivMainContent_part, 400, 1250 , 40 ,false); </script>", false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Part Grid
        private void BindDataToPartGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToPartGrid(iNoRowToAdd);
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
                else
                {
                    PartDetailsGrid.DataSource = dtPart;
                    PartDetailsGrid.DataBind();
                    // Vikram 30012018 
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + PartDetailsGrid.ClientID + "',DivHeaderRow_part,DivMainContent_part, 400, 1250 , 40 ,false); </script>", false);
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToPartGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "N";
                string sPartId = "", sReqNo = "";
                int idtRowCnt = 0;
                string sPartName = "";
                bool bShowNewPart = true;
                double dPartAmount = 0;
                double dLubAmount = 0;
                double dTmpValue = 0;
                hdnSelectedPartID.Value = "";

                if (PartDetailsGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {

                    //hide Return Qty
                    //PartDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none"); // Hide Header        
                    //PartDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Hide Cell

                    //Req Qty
                    PartDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "none");//Hide Cell

                    //Issue Qty
                    PartDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//Hide Cell

                    //Return Qty
                    PartDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Hide Cell

                    //Use Qty
                    PartDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//Hide Cell

                    //Paid Qty
                    PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//Hide Cell

                    //Warranty tag
                    PartDetailsGrid.HeaderRow.Cells[11].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[11].Style.Add("display", "none");//Hide Cell

                    //FOC Qty
                    PartDetailsGrid.HeaderRow.Cells[13].Style.Add("display", ""); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "");//Hide Cell

                    //FOC Reason
                    PartDetailsGrid.HeaderRow.Cells[14].Style.Add("display", ""); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");//Hide Cell

                    //FSC Qty
                    PartDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");//Hide Cell

                    //PDI Qty
                    PartDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "none");//Hide Cell

                    //AMC Qty
                    PartDetailsGrid.HeaderRow.Cells[17].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", "none");//Hide Cell

                    //Campaign Qty
                    PartDetailsGrid.HeaderRow.Cells[18].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[18].Style.Add("display", "none");//Hide Cell

                    //hide transit Qty
                    PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[19].Style.Add("display", "none");//Hide Cell

                    //hide enroute technical Qty
                    PartDetailsGrid.HeaderRow.Cells[20].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[20].Style.Add("display", "none");//Hide Cell

                    //hide enroute non technical Qty
                    PartDetailsGrid.HeaderRow.Cells[21].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[21].Style.Add("display", "none");//Hide Cell 

                    //Hide Spare Warranty Qty
                    PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "none");//Hide Cell

                    //Show Goodwill Qty
                    PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", "none"); // show Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", "none");//show Cell

                    //Show Warranty Qty
                    PartDetailsGrid.HeaderRow.Cells[24].Style.Add("display", "none"); // Show Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[24].Style.Add("display", "none");//Show Cell  

                    //Hide PrePDI Qty
                    PartDetailsGrid.HeaderRow.Cells[25].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "none");//Hide Cell

                    //Hide Aggregate Qty
                    PartDetailsGrid.HeaderRow.Cells[26].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "none");//Hide Cell

                    //Extra qty 1
                    PartDetailsGrid.HeaderRow.Cells[27].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[27].Style.Add("display", "none");//Hide Cell

                    //Extra qty 2
                    PartDetailsGrid.HeaderRow.Cells[28].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[28].Style.Add("display", "none");//Hide Cell

                    //Req Qty
                    PartDetailsGrid.HeaderRow.Cells[6].Style.Add("display", ""); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "");//Hide Cell

                    //Use Qty
                    PartDetailsGrid.HeaderRow.Cells[9].Style.Add("display", ""); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");//Hide Cell

                    //Accidental Details 
                    PartDetailsGrid.HeaderRow.Cells[41].Style.Add("display", "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[41].Style.Add("display", "none");//Hide Cell

                    if (Session["DepartmentID"].ToString() == "6")
                    {
                        //Issue Qty
                        PartDetailsGrid.HeaderRow.Cells[7].Style.Add("display", ""); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");//Hide Cell

                        //Return Qty
                        PartDetailsGrid.HeaderRow.Cells[8].Style.Add("display", ""); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");//Hide Cell
                    }

                    if (Session["DepartmentID"].ToString() == "7")
                    {
                        //show /hide FSC Qty
                        if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 1 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2
                            || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 3 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 10
                            || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 11 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 14
                            || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 15 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 17)
                        {
                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Hide Cell

                            //Hide Spare Warranty Qty
                            PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "");//Hide Cell

                            //Show Goodwill Qty
                            PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", ""); // show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", "");//show Cell

                            //Show Warranty Qty
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.HeaderRow.Cells[24].Style.Add("display", ""); // Show Header        
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.Rows[iRowCnt].Cells[24].Style.Add("display", "");//Show Cell  

                            PartDetailsGrid.HeaderRow.Cells[41].Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2) ? "" : "none"); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[41].Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2) ? "" : "none");//Hide Cell

                            //Hide Aggregate Qty
                            //if (txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.HeaderRow.Cells[26].Style.Add("display", ""); // Hide Header        
                            //if (txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "");//Hide Cell                          
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 5)
                        {//5	Service Aggrement
                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Hide Cell

                            //Show Spare Warranty Qty
                            PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "");//Hide Cell

                            //Show AMC Qty
                            if (txtAMCChk.Text == "Y") PartDetailsGrid.HeaderRow.Cells[17].Style.Add("display", ""); // Hide Header        
                            if (txtAMCChk.Text == "Y") PartDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", "");//Hide Cell

                            //Show Warranty Qty
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.HeaderRow.Cells[24].Style.Add("display", ""); // Show Header        
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.Rows[iRowCnt].Cells[24].Style.Add("display", "");//Show Cell  
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 7)
                        {//7	PDI
                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Show Cell

                            //PDI Qty
                            PartDetailsGrid.HeaderRow.Cells[16].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "");//Show Cell
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 12)
                        {//12	PreSale
                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Show Cell

                            //Show transit Qty
                            PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[19].Style.Add("display", "");//Show Cell

                            //Show enroute technical Qty
                            PartDetailsGrid.HeaderRow.Cells[20].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[20].Style.Add("display", "");//Show Cell

                            //Show enroute non technical Qty
                            PartDetailsGrid.HeaderRow.Cells[21].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[21].Style.Add("display", "");//Show Cell       
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 13)
                        {//13	Campaign
                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Show Cell

                            //Show Spare Warranty Qty
                            PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "");//Hide Cell

                            //Show Warranty Qty
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.HeaderRow.Cells[24].Style.Add("display", ""); // Show Header        
                            if (txtWarrantyTag.Text.ToString().Trim() != "N" || txtAggregate.Text.ToString().Trim() == "G") PartDetailsGrid.Rows[iRowCnt].Cells[24].Style.Add("display", "");//Show Cell  

                            //Show Campaign Qty
                            PartDetailsGrid.HeaderRow.Cells[18].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[18].Style.Add("display", "");//Show Cell                             
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 16)
                        {//16	Pre PDI
                            //PRe-PDI Qty
                            PartDetailsGrid.HeaderRow.Cells[25].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "");//Show Cell

                            //Paid Qty
                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Show Cell
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 18)
                        { //18	Aggregate
                            // hide as per discussion with Mouli and vikram sir.
                            //if (txtAggregate.Text.ToString().Trim() == "G")
                            //{  
                            //    //Aggregate Qty
                            //    PartDetailsGrid.HeaderRow.Cells[26].Style.Add("display", ""); // Show Header        
                            //    PartDetailsGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "");//Show Cell; //Aggregate Qty //if chassis aggregate flag ='G'
                            //}

                            PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", ""); // Hide Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "");//Hide Cell

                            PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", ""); // Show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Show Cell

                            PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", ""); // show Header        
                            PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", "");//show Cell
                        }


                    }

                    // Add New Part                  
                    LinkButton lnkSelectPart = (LinkButton)PartDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowSpWPFPart(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");

                    //Part Detail ID                    
                    TextBox txtDtlPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtDtlPartID");

                    //PartID
                    TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    //Part No
                    TextBox txtPartNo = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox);

                    //Part Type Tag 
                    TextBox txtPartTypeTag = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartTypeTag") as TextBox);

                    //JobCode
                    DropDownList drpJobCode = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    //FOC Reason
                    DropDownList drpReasonID = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpReasonID");
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpReasonID, clsCommon.ComboQueryType.Dealer_FOCReason, Location.iDealerId, " and DealerHOBrID=" + Location.iDealerId.ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpReasonID, clsCommon.ComboQueryType.Dealer_FOCReason, Location.iDealerId, " and DealerHOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    //FOC Tag
                    DropDownList drpfoctag = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpfoctag");

                    // Mechanic 
                    DropDownList drpPMechanic = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpPMechanic");
                    
                    //Make
                    DropDownList drpMake = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpMake");

                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    //Part Stock
                    TextBox txtPartStock = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartStock") as TextBox);

                    TextBox txtPEstDtlID = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPEstDtlID") as TextBox);
                    TextBox txtPartGroupCode = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox);
                    
                    DropDownList drpAccdFlag = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpAccdFlag");                                        
                    sPartId = "0";
                    sReqNo = "";
                    sRecordStatus = "N";
                    int iDtlID = 0;
                    if (idtRowCnt < dtPart.Rows.Count)
                    {
                        iDtlID = Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ID"]);

                        //PartID
                        sPartId = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["PartLabourID"]);
                        txtPartID.Text = sPartId;

                        // Get IPO_no,
                        sReqNo = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["IPO_no"]);

                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIPO_no") as TextBox).Text = sReqNo;

                        if (sPartId != "0" && (iDtlID == 0 || Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["ReqQty"])) > Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["IssueQty"]))))
                        {
                            hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + sPartId;
                        }


                        //Part Type Tag
                        txtPartTypeTag.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["part_type_tag"]);

                        //PartNo 
                        txtPartNo.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["parts_no"]);

                        txtDtlPartID.Text = Func.Convert.sConvertToString(iDtlID);

                        //Status
                        sRecordStatus = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Status"]);

                        //Part Name
                        sPartName = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["PartLabourName"]);
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text = sPartName;
                        //(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).ToolTip = sPartName;

                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Stock"]);
                        txtPartStock.Text = dTmpValue.ToString("0.00");

                        //Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["ReqQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Return Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["RetQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Text = dTmpValue.ToString("0.00");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousRetQty") as TextBox).Text = dTmpValue.ToString("0.00");

                        }
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousRetQty") as TextBox).Text = dTmpValue.ToString("0.00");


                        //IssueQty Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["IssueQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Text = dTmpValue.ToString("0.00");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousIssueQty") as TextBox).Text = dTmpValue.ToString("0.00");

                        }
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousIssueQty") as TextBox).Text = dTmpValue.ToString("0.00");

                        //UseQty Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["UseQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //BillQty Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["BillQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //WarrQty Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["WarrQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Warranty tag
                        string swar_tag = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["war_tag"]);
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtwar_tag") as TextBox).Text = swar_tag;

                        //FOC Flag                    
                        drpfoctag.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["foc_tag"]);

                        //IPO No: Requisition
                        string sIPONo = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["IPO_no"]);
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIPO_no") as TextBox).Text = sIPONo;
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIPO_no") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        //Mechanic ID
                        if (txtUserType.Text == "6")
                        {
                            Func.Common.BindDataToCombo(drpPMechanic, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString() + " and Empl_Type in (7,8,2) " + ((Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["Mech_ID"]) > 0) ? " or ID=" + Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Mech_ID"]) : ""));
                        }
                        else
                        {
                            Func.Common.BindDataToCombo(drpPMechanic, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type in (7,8,2)" + ((Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["Mech_ID"]) > 0) ? " or ID=" + Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Mech_ID"]) : ""));

                        }
                        drpPMechanic.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Mech_ID"]);

                        if (iDtlID != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            //drpPMechanic.Enabled =false; 
                        }

                        //FOC Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["FOCQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //FOC Reason ID Quantity                    
                        drpReasonID.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["foc_reason_ID"]);

                        //FSC Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["FSCQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //PDI Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["PDIQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //AMC Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["AMCQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Campaign Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["CampaignQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Transit Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["transitQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Enroute Tecnhical Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["EnRouteTechQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Enroute Non Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["EnrouteNonTechQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Spares Warranty Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["SpWarQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Goodwill Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["GoodWlQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //PrePDI Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["PrePDIQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Aggregate Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["AggregateQty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Rate Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Rate"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Warranty Rate(NDP)
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["WarrRate"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtNDPRate") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //AMC/RMC Rate(SA)
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["AMCRate"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCRate") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Total Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Total"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        if (txtPartTypeTag.Text == "P" && sRecordStatus != "D") dPartAmount = dPartAmount + dTmpValue;
                        if (txtPartTypeTag.Text == "O" && sRecordStatus != "D") dLubAmount = dLubAmount + dTmpValue;
                        //MakeID                    
                        drpMake.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["MakeID"]);
                        drpAccdFlag.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["AccdFlag"]);                        

                        //Lubricant Capacity
                        if (Func.Convert.iConvertToInt(txtModelGroupID.Value) != 0 && txtPartTypeTag.Text == "O")
                        {
                            //LubLocID
                            DropDownList drpLubLoc = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc");
                            //Func.Common.BindDataToCombo(drpLubLoc, clsCommon.ComboQueryType.LubricantType, Func.Convert.iConvertToInt(txtModelGroupID.Value));
                            Func.Common.BindDataToCombo(drpLubLoc, clsCommon.ComboQueryType.LubricantTypeJobcard, Func.Convert.iConvertToInt(txtModelID.Value));
                            drpLubLoc.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["LubLocID"]);

                            DropDownList drpLubCap = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubCap");
                            //Func.Common.BindDataToCombo(drpLubCap, clsCommon.ComboQueryType.LubricantCapacity, Func.Convert.iConvertToInt(txtModelGroupID.Value));
                            Func.Common.BindDataToCombo(drpLubCap, clsCommon.ComboQueryType.LubricantCapacity, Func.Convert.iConvertToInt(txtModelID.Value));
                            drpLubCap.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["LubLocID"]);

                            drpLubLoc.Attributes.Add("onBlur", "SetLubCapasityOnLubLocationChange(this,'" + drpLubCap.ID + "')");
                            drpLubCap.Attributes.Add("disabled", "disabled");
                            if (Session["DepartmentID"].ToString() == "6") drpLubLoc.Enabled = false;
                        }
                        txtPartGroupCode.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["group_code"]);

                        if (txtPartTypeTag.Text == "P" && txtPartGroupCode.Text != "99")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                        }
                        else if (txtPartTypeTag.Text == "P" && txtPartGroupCode.Text == "99")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        else
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                        }

                        //JobcodeID                    
                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["JobcodeID"]);

                        //Make
                        drpMake.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["MakeID"]);

                        txtPEstDtlID.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["EstDtlID"]);


                        txtPTax.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax"]).Trim();
                        txtPTax1.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax1"]).Trim();
                        txtPTax2.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax2"]).Trim();

                        idtRowCnt = idtRowCnt + 1;
                    }

                    Label lnkCancel = (Label)PartDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    lnkSelectPart.Style.Add("display", "none");
                    Chk.Checked = false;
                    //If Part Id  is not allocated
                    if (sPartId == "0")
                    {
                        txtPartNo.Style.Add("display", "none");
                        if (bShowNewPart == true)
                        {
                            //lnkSelectPart.Style.Add("display", "");
                            if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 7 && (txtApPartAmt.Text.Trim().Length > 0 || txtApLabAmt.Text.Trim().Length > 0 || txtApLubAmt.Text.Trim().Length > 0 || txtApMiscAmt.Text.Trim().Length > 0))
                            {
                                lnkSelectPart.Style.Add("display", "");
                            }
                            bShowNewPart = false;
                        }
                    }
                    else
                    {
                        //lblChngPart.Style.Add("display", "");
                    }
                    if (sRecordStatus == "D")
                    {
                        lnkCancel.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        PartDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E" || sRecordStatus == "S")
                    {
                        lnkCancel.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                    }
                    if (bRecordIsOpen == false || Session["DepartmentID"].ToString() == "6")
                    {
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "");
                        //lblAddPart.Style.Add("display", "none");
                        lnkSelectPart.Style.Add("display", "none");
                        //lblChngPart.Style.Add("display", "none");
                        drpfoctag.Enabled = false;
                        drpJobCode.Enabled = false;
                        drpMake.Enabled = false;
                        //drpPMechanic.Enabled = false;
                        drpReasonID.Enabled = false;

                    }
                    //if (iDtlID != 0) Chk.Style.Add("display", "none");
                    if (txtUserType.Text == "6")
                    {

                        lnkSelectPart.Style.Add("display", "none");
                    }

                }

                txtPartAmount.Text = dPartAmount.ToString("0.00");
                txtLubricantAmount.Text = dLubAmount.ToString("0.00");

                txtJbTotAmt.Text = Func.Convert.dConvertToDouble(txtPartAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLubricantAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLabourAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtSubletAmount.Text.ToString()).ToString("0.00");

                //lblPartRecCnt.Text = Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtPart) ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void lnkSelectJobDtl_Click(object sender, EventArgs e)
        {
            try
            {
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
                //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + hdnSelectedPartID.Value + ".');</script>");
                bFillDetailsFromPartGrid();
                BindDataToPartGrid(true, 0);
                dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                CreateNewRowToTaxGroupDetailsTable();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbTaxDetails"] = dtJbTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Part Grid
        private bool bFillDetailsFromPartGrid()
        {
            string sStatus = "";
            dtPart = (DataTable)Session["PartDetails"];
            int iCntForDelete = 0;
            int iPartID = 0;

            double iPartReqQty = 0;
            double iPartIsuueQty = 0;
            double iPartReturnQty = 0;
            double iPartBillQty = 0, iaggtQty = 0, iPrePDIQty = 0;
            double iPartWarrQty = 0;
            double iPartUseQty = 0;

            //bool bValidate = false;
            bool bValidate = true;
            double dRate = 0;
            //hdnSelectedPartID.Value = "";

            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                iPartID = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                if (iPartID != 0)
                {
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == false)
                    {
                        double PrvIssueQty, PrvReturnQty, IssueQty, lubCapasityQty, StockQty, ReturnQty, UseQty;
                        string PartType = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartTypeTag") as TextBox).Text);
                        lubCapasityQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubCap") as DropDownList).SelectedItem);
                        lubCapasityQty = lubCapasityQty + (lubCapasityQty * 0.05);

                        clsDB objDB = new clsDB();
                        //(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartStock") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", Location.iDealerId, iPartID, "N"));

                        iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
                        if (iPartReqQty == 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Required Qty should not be blank for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }


                        //if (PartType == "O" && iPartReqQty > lubCapasityQty)
                        //{
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Required Qty should not be greater than pouring location capasity For Part line no " + (iRowCnt) + ".');</script>");
                        //    bValidate = false;
                        //    break;
                        //}
                        // Get Issued Qty
                        iPartIsuueQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Text);
                        IssueQty = iPartIsuueQty;

                        PrvIssueQty = 0;

                        PrvIssueQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousIssueQty") as TextBox).Text);
                        if (iPartReqQty < iPartIsuueQty)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Issue Qty is greater than Required Qty for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }

                        if (PrvIssueQty > IssueQty && PrvIssueQty > 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Previous Issue Qty is greater than editable Issue Qty for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }

                        iPartReturnQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Text);
                        ReturnQty = iPartReturnQty;

                        PrvReturnQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPreviousRetQty") as TextBox).Text);

                        if (PrvReturnQty > ReturnQty && PrvReturnQty > 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Return Qty cannot be reduced for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }

                        if (PrvIssueQty < IssueQty && Func.Convert.iConvertToInt(Session["DepartmentID"]) == 6) (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartStock") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", Location.iDealerId, iPartID, "N"));

                        StockQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartStock") as TextBox).Text);
                        if (IssueQty > StockQty + PrvIssueQty)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Issue Qty is greater than Stock Qty for Part line no " + (iRowCnt) + ".');</script>");
                            //if (PrvIssueQty > 0) ObjQtyControl.value = PrvIssueQty;
                            bValidate = false;
                            break;
                        }

                        if (IssueQty < ReturnQty)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Return Qty can not be reduce for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }
                        iPartUseQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Text);
                        UseQty = iPartUseQty;
                        double BillQty, FOCQty, FSCQty, PDIQty, AMCQty, CampaignQty, TransitQty, EnrouteTQty, EnrouteNTQty, SpWarrQty, GoodWillQty, WarrQty, PrePDIQty, aggtQty;

                        BillQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Text);
                        if (IssueQty < BillQty)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Bill Qty is greater than Issue Qty for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }
                        FOCQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Text);
                        FSCQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Text);
                        PDIQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Text);

                        AMCQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Text);
                        CampaignQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Text);
                        TransitQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Text);
                        EnrouteTQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Text);
                        EnrouteNTQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Text);
                        SpWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Text);
                        GoodWillQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Text);
                        WarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Text);
                        PrePDIQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Text);
                        aggtQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Text);

                        if (UseQty != (BillQty + FOCQty + FSCQty + PDIQty + AMCQty + CampaignQty + TransitQty + EnrouteTQty + EnrouteNTQty + SpWarrQty + GoodWillQty + WarrQty + PrePDIQty + aggtQty))
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Enter Correct Qty. Total Qty not match with UseQty for Part line no " + (iRowCnt) + ".');</script>");
                            bValidate = false;
                            break;
                        }

                        //Code change for speed related
                        dtPart.Rows[iRowCnt]["PartLabourID"] = iPartID;

                        //iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

                        // Get Issued Qty
                        //iPartIsuueQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Text);
                        iPartIsuueQty = IssueQty;
                        dtPart.Rows[iRowCnt]["IssueQty"] = iPartIsuueQty;
                        if (iPartIsuueQty > 0) sCreateGP = "N";

                        // Get Return Qty
                        //iPartReturnQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Text);
                        iPartReturnQty = ReturnQty;
                        dtPart.Rows[iRowCnt]["RetQty"] = iPartReturnQty;

                        //PartNo Or NewPart
                        dtPart.Rows[iRowCnt]["parts_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

                        // Get Required Qty
                        //iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);                        
                        dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

                        // Get Return Qty
                        //iPartUseQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Text);
                        iPartUseQty = UseQty;
                        dtPart.Rows[iRowCnt]["UseQty"] = iPartUseQty;

                        // Get IPO_no,
                        dtPart.Rows[iRowCnt]["IPO_no"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIPO_no") as TextBox).Text);

                        // Get Mech_ID                                 
                        dtPart.Rows[iRowCnt]["Mech_ID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpPMechanic") as DropDownList).SelectedValue);

                        // Get MakeID     
                        dtPart.Rows[iRowCnt]["MakeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpMake") as DropDownList).SelectedValue);

                        // Get LubLocID     
                        dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);

                        // Get war_tag
                        dtPart.Rows[iRowCnt]["war_tag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtwar_tag") as TextBox).Text);

                        // Get foc_tag                 
                        dtPart.Rows[iRowCnt]["foc_tag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("drpfoctag") as DropDownList).SelectedValue);

                        // Get FOCQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Text);
                        iPartWarrQty = FOCQty;
                        dtPart.Rows[iRowCnt]["FOCQty"] = iPartWarrQty;

                        // Get foc_reason_ID                
                        dtPart.Rows[iRowCnt]["foc_reason_ID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpReasonID") as DropDownList).SelectedValue);

                        // Get FSCQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Text);
                        iPartWarrQty = FSCQty;
                        dtPart.Rows[iRowCnt]["FSCQty"] = iPartWarrQty;

                        // Get PDIQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Text);
                        iPartWarrQty = PDIQty;
                        dtPart.Rows[iRowCnt]["PDIQty"] = iPartWarrQty;

                        // Get AMCQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Text);
                        iPartWarrQty = AMCQty;
                        dtPart.Rows[iRowCnt]["AMCQty"] = iPartWarrQty;

                        // Get CampaignQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Text);
                        iPartWarrQty = CampaignQty;
                        dtPart.Rows[iRowCnt]["CampaignQty"] = iPartWarrQty;

                        // Get transitQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Text);
                        iPartWarrQty = TransitQty;
                        dtPart.Rows[iRowCnt]["transitQty"] = iPartWarrQty;

                        // Get EnRouteTechQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Text);
                        iPartWarrQty = EnrouteTQty;
                        dtPart.Rows[iRowCnt]["EnRouteTechQty"] = iPartWarrQty;

                        // Get EnrouteNonTechQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Text);
                        iPartWarrQty = EnrouteNTQty;
                        dtPart.Rows[iRowCnt]["EnrouteNonTechQty"] = iPartWarrQty;

                        // Get SpWarQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Text);
                        iPartWarrQty = SpWarrQty;
                        dtPart.Rows[iRowCnt]["SpWarQty"] = iPartWarrQty;

                        // Get GoodWlQty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Text);
                        iPartWarrQty = GoodWillQty;
                        dtPart.Rows[iRowCnt]["GoodWlQty"] = iPartWarrQty;

                        // Get Warranty Qty
                        //iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Text);
                        iPartWarrQty = WarrQty;
                        dtPart.Rows[iRowCnt]["WarrQty"] = iPartWarrQty;

                        // Get Billed Qty
                        //iPartBillQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Text);
                        iPartBillQty = BillQty;
                        dtPart.Rows[iRowCnt]["BillQty"] = iPartBillQty;

                        //iPrePDIQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Text);
                        iPrePDIQty = PrePDIQty;
                        dtPart.Rows[iRowCnt]["PrePDIQty"] = iPrePDIQty;

                        //iaggtQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Text);
                        iaggtQty = aggtQty;
                        dtPart.Rows[iRowCnt]["AggregateQty"] = iaggtQty;

                        // Get Rate
                        dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["Rate"] = dRate;

                        //// Get Total
                        dtPart.Rows[iRowCnt]["Total"] = iPartBillQty * dRate;// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);


                        //Get WarrRate
                        dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtNDPRate") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["WarrRate"] = dRate;

                        //Get WarrRate
                        dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCRate") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["AMCRate"] = dRate;

                        //MakeID
                        dtPart.Rows[iRowCnt]["MakeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpMake") as DropDownList).SelectedValue);

                        //LubLocID
                        dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);

                        //JobCode  
                        dtPart.Rows[iRowCnt]["JobcodeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                        dtPart.Rows[iRowCnt]["EstDtlID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPEstDtlID") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["group_code"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode") as TextBox).Text);

                        dtPart.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox).Text);
                        dtPart.Rows[iRowCnt]["AccdFlag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("drpAccdFlag") as DropDownList).SelectedValue);
                        
                        // Record Status
                        sStatus = "S";

                        if (iPartReqQty != 0)
                        {
                            bValidate = true;
                            dtPart.Rows[iRowCnt]["Status"] = "S";// for Save 
                        }
                    }
                    if (Chk.Checked == true)
                    {
                        dtPart.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }

            //for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            //{
            //    //PartID                
            //    iPartID = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
            //    if (iPartID != 0)
            //    {
            //        //ID
            //        //if (txtRefClaimID.Text != "")                    //    dtPart.Rows[iRowCnt]["ID"] = 0;

            //        dtPart.Rows[iRowCnt]["PartLabourID"] = iPartID;

            //        iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

            //        // Get Issued Qty
            //        iPartIsuueQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["IssueQty"] = iPartIsuueQty;

            //        // Get Return Qty
            //        iPartReturnQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["RetQty"] = iPartReturnQty;

            //        //if (Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ReqQty"]) > Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["IssueQty"]) ||(iPartID != 0 && Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ID"]) == 0 )) 
            //        //hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + iPartID;

            //        //PartNo Or NewPart
            //        dtPart.Rows[iRowCnt]["parts_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

            //        ////Part Name
            //        //dtPart.Rows[iRowCnt]["Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text;

            //        //// Failed Part Make
            //        //dtPart.Rows[iRowCnt]["Failed_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartMake") as TextBox).Text;

            //        //// Replaced PartID
            //        //dtPart.Rows[iRowCnt]["Replaced_Part_No_ID"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartID") as TextBox).Text;

            //        ////Replaced PartNo 
            //        //dtPart.Rows[iRowCnt]["Replaced_Part_No"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartNo") as TextBox).Text;

            //        ////Replaced Part Name
            //        //dtPart.Rows[iRowCnt]["Replaced_Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartName") as TextBox).Text;

            //        //// Replaced Part Make
            //        //dtPart.Rows[iRowCnt]["Replaced_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartMake") as TextBox).Text;


            //        // Get Required Qty
            //        iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

            //        // Get Issued Qty
            //        iPartIsuueQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIssueQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["IssueQty"] = iPartIsuueQty;
            //        if (iPartIsuueQty > 0) sCreateGP = "N";
            //        // Get Return Qty
            //        iPartReturnQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRetQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["RetQty"] = iPartReturnQty;

            //        // Get Return Qty
            //        iPartUseQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtUseQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["UseQty"] = iPartUseQty;

            //        // Get IPO_no,
            //        dtPart.Rows[iRowCnt]["IPO_no"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtIPO_no") as TextBox).Text);

            //        // Get Mech_ID                                 
            //        dtPart.Rows[iRowCnt]["Mech_ID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpPMechanic") as DropDownList).SelectedValue);

            //        // Get MakeID     
            //        dtPart.Rows[iRowCnt]["MakeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpMake") as DropDownList).SelectedValue);

            //        // Get LubLocID     
            //        dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);

            //        // Get war_tag
            //        dtPart.Rows[iRowCnt]["war_tag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtwar_tag") as TextBox).Text);

            //        // Get foc_tag                 
            //        dtPart.Rows[iRowCnt]["foc_tag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("drpfoctag") as DropDownList).SelectedValue);

            //        // Get FOCQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFOCQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["FOCQty"] = iPartWarrQty;

            //        // Get foc_reason_ID                
            //        dtPart.Rows[iRowCnt]["foc_reason_ID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpReasonID") as DropDownList).SelectedValue);

            //        // Get FSCQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtFSCQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["FSCQty"] = iPartWarrQty;

            //        // Get PDIQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["PDIQty"] = iPartWarrQty;

            //        // Get AMCQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["AMCQty"] = iPartWarrQty;

            //        // Get CampaignQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["CampaignQty"] = iPartWarrQty;

            //        // Get transitQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["transitQty"] = iPartWarrQty;

            //        // Get EnRouteTechQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["EnRouteTechQty"] = iPartWarrQty;

            //        // Get EnrouteNonTechQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["EnrouteNonTechQty"] = iPartWarrQty;

            //        // Get SpWarQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["SpWarQty"] = iPartWarrQty;

            //        // Get GoodWlQty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["GoodWlQty"] = iPartWarrQty;

            //        // Get Warranty Qty
            //        iPartWarrQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["WarrQty"] = iPartWarrQty;

            //        // Get Billed Qty
            //        iPartBillQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtBillQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["BillQty"] = iPartBillQty;

            //        iPrePDIQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["PrePDIQty"] = iPrePDIQty;

            //        iaggtQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["AggregateQty"] = iaggtQty;                                        

            //        // Get Rate
            //        dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["Rate"] = dRate;

            //        //// Get Total
            //        dtPart.Rows[iRowCnt]["Total"] = iPartBillQty * dRate;// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);


            //        //Get WarrRate
            //        dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtNDPRate") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["WarrRate"] = dRate;


            //        //MakeID
            //        dtPart.Rows[iRowCnt]["MakeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpMake") as DropDownList).SelectedValue);

            //        //LubLocID
            //        dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);

            //        //JobCode  
            //        dtPart.Rows[iRowCnt]["JobcodeID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

            //        dtPart.Rows[iRowCnt]["EstDtlID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPEstDtlID") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["group_code"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode") as TextBox).Text);

            //        dtPart.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox).Text);
            //        dtPart.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox).Text);

            //        // Record Status
            //        sStatus = "S";

            //        if (iPartReqQty != 0)
            //        {
            //            bValidate = true;
            //            dtPart.Rows[iRowCnt]["Status"] = "S";// for Save 
            //        }
            //        CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
            //        if (Chk.Checked == true)
            //        {
            //            dtPart.Rows[iRowCnt]["Status"] = "D";
            //            iCntForDelete++;
            //        }

            //    }
            //}
            //bValidate = true;
            if (iCntForDelete != 0)
            {
                if (iCntForDelete == PartDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                }
            }
            return bValidate;
        }

        //To Get Total Part Count
        private void CalculatePartGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Part Description            
                    TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    sPartID = txtPartID.Text;
                    if (sPartID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblPartRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Labour Function

        // Create Row To Labour Grid
        private void CreateNewRowToLabourGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                int iMaxLabourGridRowCount = 0;
                DataTable dtDefaultLabour = new DataTable();
                int iRowCntStartFrom = 0;
                iMaxLabourGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxLabourGridRowCount"]);
                if (Session["LabourDetails"] != null)
                {
                    dtDefaultLabour = (DataTable)Session["LabourDetails"];
                }
                else
                {
                    dtDefaultLabour = dtLabour;
                }

                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultLabour.Rows.Count == 0)
                    {
                        dtDefaultLabour.Columns.Clear();
                        dtDefaultLabour.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_Desc", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Lab_Tag", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("AddLbrDescription", typeof(String)));

                        dtDefaultLabour.Columns.Add(new DataColumn("war_tag", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("foc_tag", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("foc_reason_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("EstDtlID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("rep_job", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Mech_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("out_Lab_amt", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("VenderID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("out_Mech_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("AddLbrDescriptionID", typeof(int)));
                    }
                    else
                    {
                        if (dtDefaultLabour.Rows.Count >= iMaxLabourGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxLabourGridRowCount;
                }

                iMaxLabourGridRowCount = iMaxLabourGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLabourGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultLabour.NewRow();
                    dr["SRNo"] = "1";
                    dr["ID"] = 0;
                    dr["PartLabourID"] = 0;
                    dr["part_type_tag"] = "L";
                    dr["Lab_Tag"] = "";
                    dr["labour_code"] = "";
                    dr["PartLabourName"] = "";
                    dr["out_lab_desc"] = "";
                    dr["ManHrs"] = 0;
                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["Job_Code_ID"] = 0;
                    dr["AddLbrDescription"] = "";
                    dr["war_tag"] = "N";

                    dr["foc_tag"] = "N";
                    dr["foc_reason_ID"] = 0;
                    dr["EstDtlID"] = 0;
                    dr["rep_job"] = "N";
                    dr["Mech_ID"] = 0;
                    dr["out_Lab_amt"] = 0;
                    dr["VenderID"] = 0;
                    dr["out_Mech_ID"] = 0;
                    dr["AddLbrDescriptionID"] = 0;

                    dtDefaultLabour.Rows.Add(dr);
                    dtDefaultLabour.AcceptChanges();
                }

            Bind:
                Session["LabourDetails"] = dtDefaultLabour;
                LabourDetailsGrid.DataSource = dtDefaultLabour;
                LabourDetailsGrid.DataBind();
                //Vikram K 02022018
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key1", "<script>MakeStaticHeader('" + LabourDetailsGrid.ClientID + "',DivHeaderRow_labour,DivMainContent_labour, 400, 1250 , 40 ,false); </script>", false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Bind Data to labour Grid
        private void BindDataToLaborGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToLabourGrid(iNoRowToAdd);
                    SetControlPropertyToLabourGrid(bRecordIsOpen);
                }
                else
                {
                    LabourDetailsGrid.DataSource = dtLabour;
                    LabourDetailsGrid.DataBind();
                    // Vikram K 02022018 
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key1", "<script>MakeStaticHeader('" + LabourDetailsGrid.ClientID + "',DivHeaderRow_labour,DivMainContent_labour, 400, 1250 , 40 ,false); </script>", false);
                    SetControlPropertyToLabourGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Labour Grid
        private void SetControlPropertyToLabourGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "N";
                // Hide Labour Id  Column
                if (LabourDetailsGrid.Rows.Count == 0) return;
                LabourDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none");
                string sLabourId = "0", sLabTag = "", sLabCode = "";
                int idtRowCnt = 0;
                double dLabourAmount = 0;
                double dSubletAmount = 0;
                bool bShowLabour = true;
                double dTmpValue = 0;
                hdnSelectedLabourID.Value = "";
                string sAddLbrID = "";
                string sLstTwoDigit = "";
                string sFirstFiveDigit = "";
                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //LabourID                                
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[1].Style.Add("display", "none");


                    LabourDetailsGrid.HeaderRow.Cells[22].Style.Add("display", "none"); // Hide Header        
                    LabourDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "none");//Hide Cell

                    //LabourNo Or NewLabour
                    TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");

                    // New  Labour Control
                    //Label lblNewLabour = (Label)LabourDetailsGrid.Rows[iRowCnt].FindControl("lblNewLabour");
                    //lblNewLabour.Attributes.Add("onclick", " return ShowMultiLabourMaster(this," + Location.iDealerId  + ")");
                    //lblNewLabour.Style.Add("display", "none");
                    LinkButton lnkSelectLabour = (LinkButton)LabourDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectLabour");
                    lnkSelectLabour.Attributes.Add("onclick", "return ShowMultiLabourMaster(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");
                    //lnkSelectLabour.Attributes.Add("onclick", "return ShowCheck();");

                    lnkSelectLabour.Style.Add("display", "none");
                    //Labour Name
                    TextBox txtLabourDesc = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc");
                    //txtLabourDesc.Attributes.Add("disabled", "disabled");

                    //Other Description
                    DropDownList drpLbrDescription = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription");
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    TextBox txtLbrDescription = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription");

                    //Lab Tag
                    TextBox txtLabCD = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD");

                    //Warr tag 
                    //TextBox txtLabWarr = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabWarr");
                    DropDownList drpLabWarr = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLabWarr");

                    // Man Hrs
                    TextBox txtManHrs = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs");

                    //Rate
                    TextBox txtRate = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate");
                    TextBox txtWRate = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtWRate");
                    TextBox txtPaidRate = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtPaidRate");
                    
                    //Total
                    TextBox txtTotal = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");


                    //FOC Tag       foc_tag
                    DropDownList drpFOC = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpFOC");

                    //FOC Reaseon       foc_reason_ID
                    DropDownList drpFOCReason = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpFOCReason");
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpFOCReason, clsCommon.ComboQueryType.Dealer_FOCReason, Location.iDealerId, " and DealerHOBrID=" + Location.iDealerId.ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpFOCReason, clsCommon.ComboQueryType.Dealer_FOCReason, Location.iDealerId, " and DealerHOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    //Mech Name     Mech_ID
                    DropDownList drpMechName = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpMechName");

                    //Sublet Supplier     VenderID
                    DropDownList drpSubletSupp = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpSubletSupp");
                    Func.Common.BindDataToCombo(drpSubletSupp, clsCommon.ComboQueryType.Dealer_Supplier, Location.iDealerId, " and Sup_Type not in(18)"); //No HO Branch filter because supplier create at ho only.

                    //Out Mech Name     out_Mech_ID
                    DropDownList drpOutMechName = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpOutMechName");
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpOutMechName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString() + " and Empl_Type in (7,8,2)");
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpOutMechName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type in (7,8,2)");
                    }
                    //Labour Main group LbrMnGrp
                    TextBox txtLabMnGr = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabMnGr");

                    //Labour EstDtlID
                    TextBox txtEstDtlID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtEstDtlID");

                    //Labour group for tax
                    TextBox txtLGroupCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");

                    TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox);
                    TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox);
                    TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox);

                    //Repjob rep_job
                    TextBox txtRepjob = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRepjob");

                    //Sublet Amt    out_Lab_amt
                    TextBox txtSubletAmt = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletAmt");

                    //Sublet Labour Description     out_lab_desc
                    TextBox txtSubletDescription = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletDescription");

                    //Sublet WO No      WONO
                    TextBox txtWONo = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtWONo");

                    //Material Receipt No ReceiptNo
                    TextBox txtMaterialReceipt = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtMaterialReceipt");

                    //JobCode
                    DropDownList drpJobCode = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    DropDownList drpAccdFlag = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpAccdFlag");
                    
                    sLabourId = "0";
                    sRecordStatus = "N";

                    if (idtRowCnt < dtLabour.Rows.Count)
                    {
                        // labour ID 
                        sLabourId = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["PartLabourID"]);
                        txtLabourID.Text = sLabourId;

                        //Labour Code 
                        txtLabourCode.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["labour_code"]);
                        txtLabourCode.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        if (sLabourId != "0")
                        {
                            sLstTwoDigit = txtLabourCode.Text.ToString().Substring(Func.Convert.iConvertToInt(txtLabourCode.Text.ToString().Length) - 2, 2);
                            sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                        }

                        //Labour Description
                        txtLabourDesc.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["PartLabourName"]);

                        txtLbrDescription.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescription"]);

                        sAddLbrID = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"]);
                        // Add New Lbr Description
                        if (sAddLbrID == "9999" || sAddLbrID == "0" || sAddLbrID == "")
                        {
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpLbrDescription.Items.Add(lstitm);
                            drpLbrDescription.Attributes.Add("onChange", "OnLbrDescValueChange(this,'" + txtLbrDescription.ID + "')");
                        }
                        drpLbrDescription.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"]);
                        if (sAddLbrID == "9999")
                        {
                            txtLbrDescription.Style.Add("display", "");
                            drpLbrDescription.Style.Add("display", "none");
                        }
                        else
                        {
                            drpLbrDescription.Style.Add("display", "");
                            txtLbrDescription.Style.Add("display", "none");
                        }


                        //drpLbrDescription.Enabled = (sLstTwoDigit == "99" || sFirstFiveDigit == "33333" || sFirstFiveDigit == "44444" || sFirstFiveDigit == "55555" || sFirstFiveDigit == "MTIMI") ? true : false;
                        drpLbrDescription.Enabled = (sFirstFiveDigit == "MTIMI") ? true : false;

                        txtLabCD.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Lab_Tag"]);


                        //1	Paid Service || //2	Accident || //3	Free Service || //10	BreakDown || //11	Fitness Certificate || 
                        //14	Unscheduled Repairs ||//15	On Site || //17	Extended Warranty
                        if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 1 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 3 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 10 ||
                           Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 11 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 14 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 15 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 17)
                        {

                            if (txtLabCD.Text.Trim() == "C") if (txtWarrantyTag.Text.Trim() != "N" || txtAggregate.Text.Trim() == "G") drpLabWarr.Items.Insert(0, new ListItem("Warranty", "W"));
                            if (txtLabCD.Text.Trim() == "D")
                            {
                                drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                                drpLabWarr.Items.Add(new ListItem("Goodwill", "G", true));
                            }

                            LabourDetailsGrid.HeaderRow.Cells[22].Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2 && Session["DepartmentID"].ToString() == "7") ? "" : "none"); // Hide Header        
                            LabourDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 2 && Session["DepartmentID"].ToString() == "7") ? "" : "none");//Hide Cell
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 5)
                        {//5	Service Aggrement
                            if (txtLabCD.Text.Trim() == "C")
                            {
                                //drpLabWarr.Items.Insert(0, new ListItem("AMC", "A", true));
                                drpLabWarr.Items.Insert(0, new ListItem("RMC", "A", true));
                                if (txtWarrantyTag.Text.Trim() != "N" || txtAggregate.Text.Trim() == "G") drpLabWarr.Items.Insert(0, new ListItem("Warranty", "W", true));
                            }
                            if (txtLabCD.Text.Trim() == "D") drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 7)
                        {//7	PDI              
                            if (txtLabCD.Text.Trim() == "C") drpLabWarr.Items.Add(new ListItem("PDI", "P", true));
                            if (txtLabCD.Text.Trim() == "D") drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 12)
                        {//12	PreSale            
                            if (txtLabCD.Text.Trim() == "C")
                            {
                                drpLabWarr.Items.Add(new ListItem("Transit", "T", true));
                                drpLabWarr.Items.Add(new ListItem("Enroute Technical", "E", true));
                                drpLabWarr.Items.Add(new ListItem("Enroute Non Technical", "R", true));
                            }
                            if (txtLabCD.Text.Trim() == "D")
                            {
                                drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                            }
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 13)
                        {//13	Campaign             
                            if (txtLabCD.Text.Trim() == "C")
                            {
                                drpLabWarr.Items.Add(new ListItem("Campaign", "C", true));
                                if (txtWarrantyTag.Text != "N" || txtAggregate.Text.Trim() == "G") drpLabWarr.Items.Add(new ListItem("Warranty", "W", true));
                            }
                            if (txtLabCD.Text.Trim() == "D") drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 16)
                        {//16	Pre PDI                
                            if (txtLabCD.Text.Trim() == "C") drpLabWarr.Items.Add(new ListItem("Pre-PDI", "I", true));
                            if (txtLabCD.Text.Trim() == "D") drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                        }
                        else if (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 18)
                        { //18	Aggregate            
                            if (txtLabCD.Text.Trim() == "C" && txtAggregate.Text == "G") drpLabWarr.Items.Add(new ListItem("Warranty", "W", true));
                            if (txtLabCD.Text.Trim() == "D")
                            {
                                drpLabWarr.Items.Add(new ListItem("Non-Warranty", "N", true));
                                drpLabWarr.Items.Add(new ListItem("Goodwill", "G", true));
                            }
                        }

                        //txtLabourDesc.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        //txtLabWarr.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["war_tag"]);

                        drpLabWarr.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["war_tag"]).Trim();


                        sLabCode = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["labour_code"]);
                        sLabTag = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Lab_Tag"]).Trim();
                        string sEstDtlID = "";
                        sEstDtlID = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["EstDtlID"]);

                        txtLGroupCode.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["group_code"]);

                        txtLTax.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax"]).Trim();
                        txtLTax1.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax1"]).Trim();
                        txtLTax2.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax2"]).Trim();

                        //if (sLabourId != "0" && sLabCode != "999999") hdnSelectedLabourID.Value = hdnSelectedLabourID.Value + ((hdnSelectedLabourID.Value.Length > 0) ? "," : "") + sLabourId.Trim()  + "<--" + sLabTag.Trim()  + "<--" + sEstDtlID.Trim() ;
                        if (sLabourId != "0" && sFirstFiveDigit != "MTIMI") hdnSelectedLabourID.Value = hdnSelectedLabourID.Value + ((hdnSelectedLabourID.Value.Length > 0) ? "," : "") + sLabourId.Trim() + "<--" + sLabTag.Trim() + "<--" + sEstDtlID.Trim();

                        //Man Hrs
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["ManHrs"]);
                        if (dTmpValue != 0)
                        {
                            txtManHrs.Text = dTmpValue.ToString("0.00");
                        }
                        //txtManHrs.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");


                        //Rate 
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Rate"]);
                        if (dTmpValue != 0)
                        {
                            txtRate.Text = dTmpValue.ToString("0.00");
                        }
                        //txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");                        

                        txtPaidRate.Text = (sFirstFiveDigit == "MTIDC") ? txtRate.Text : hdnLPaidRate.Value;
                        txtWRate.Text = (sFirstFiveDigit == "MTIDC") ? txtRate.Text : hdnLWarrRate.Value;
                        
                        //Total
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Total"]);
                        if (dTmpValue != 0)
                        {
                            txtTotal.Text = dTmpValue.ToString("0.00");
                        }
                        //txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        // job Code
                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Job_Code_ID"]);

                        //FOC Tag       foc_tag
                        drpFOC.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["foc_tag"].ToString().Trim());

                        //FOC Reaseon       foc_reason_ID
                        drpFOCReason.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["foc_reason_ID"]);

                        //Mech Name     Mech_ID
                        if (txtUserType.Text == "6")
                        {
                            Func.Common.BindDataToCombo(drpMechName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString() + " and Empl_Type in (7,8,2)" + ((Func.Convert.iConvertToInt(dtLabour.Rows[iRowCnt]["Mech_ID"]) > 0) ? " or ID=" + Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Mech_ID"]) : ""));
                        }
                        else
                        {
                            Func.Common.BindDataToCombo(drpMechName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type in (7,8,2)" + ((Func.Convert.iConvertToInt(dtLabour.Rows[iRowCnt]["Mech_ID"]) > 0) ? " or ID=" + Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Mech_ID"]) : ""));
                        }
                        drpMechName.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Mech_ID"]);

                        //Sublet Supplier     VenderID                        
                        drpSubletSupp.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["VenderID"]);

                        //Out Mech Name     out_Mech_ID                        
                        drpOutMechName.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["out_Mech_ID"]);

                        //Labour Main group LbrMnGrp                        
                        txtLabMnGr.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["LbrMnGrp"]);

                        //Labour EstDtlID
                        txtEstDtlID.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["EstDtlID"]);

                        //Repjob rep_job
                        txtRepjob.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["rep_job"]);
                        drpAccdFlag.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AccdFlag"]);

                        //if (txtLabourCode.Text != "999999")     44444 is a sublet amount so it is block              
                        //if (sLstTwoDigit == "99" || sFirstFiveDigit == "33333" || sFirstFiveDigit == "55555" || sFirstFiveDigit == "MTIMI")

                        //if (sFirstFiveDigit == "MTIMI")
                        //{
                        //    txtManHrs.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                        //    txtRate.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");

                        //    txtManHrs.Attributes.Add("onblur", "return calculateLabourTotal(event,this);");
                        //    txtRate.Attributes.Add("onblur", "return calculateLabourTotal(event,this);");
                        //}
                        //else
                        //{
                        //    txtManHrs.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        //    txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        //}
                        //txtTotal.Attributes.Add("readonly", "readonly");
                        txtManHrs.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        if (sFirstFiveDigit == "MTIMI" || sFirstFiveDigit == "MTICC")
                        {
                            txtTotal.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtTotal.Attributes.Add("onblur", "return calculateLabourTotal(event,this);");
                        }
                        else
                        {
                            txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }

                        if (txtLabCD.Text.Trim() == "D" && drpFOC.SelectedValue == "N" && drpLabWarr.SelectedValue.Trim() == "N" && sFirstFiveDigit != "MTIOU")
                        {
                            dLabourAmount = dLabourAmount + dTmpValue;
                        }
                        //Sublet Amt    out_Lab_amt
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["out_Lab_amt"]);
                        if (txtLabCD.Text.Trim() == "D" && drpFOC.SelectedValue == "N" && drpLabWarr.SelectedValue.Trim() == "N" && sFirstFiveDigit == "MTIOU")
                        {
                            dSubletAmount = dSubletAmount + dTmpValue;
                        }
                        if (dTmpValue != 0)
                        {
                            txtSubletAmt.Text = dTmpValue.ToString("0.00");
                        }
                        if (sFirstFiveDigit != "MTIOU") txtSubletAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        //Sublet Labour Description     out_lab_desc
                        txtSubletDescription.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["out_lab_desc"]);
                        if (sFirstFiveDigit != "MTIOU") txtSubletDescription.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        //Sublet WO No      WONO
                        txtWONo.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["WONO"]);
                        txtWONo.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        //Material Receipt No ReceiptNo                        
                        txtMaterialReceipt.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["ReceiptNo"]);
                        txtMaterialReceipt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        //txtMaterialReceipt.Visible = false;
                        txtMaterialReceipt.Style.Add("display", "none");
                        sRecordStatus = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;

                        //if (Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["ChangeDetails_YN"]) == "Y")
                        //{
                        //    LabourDetailsGrid.Rows[iRowCnt].Enabled = true;
                        //}
                    }

                    Label lnkCancel = (Label)LabourDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //Delete 
                    CheckBox Chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Chk.Checked = false;
                    //If Labour Id  is not allocated
                    if (sLabourId == "0")
                    {
                        if (bShowLabour == true)
                        {
                            bShowLabour = false;
                            //lblNewLabour.Style.Add("display", "");
                            if (txtApPartAmt.Text.Trim().Length > 0 || txtApLabAmt.Text.Trim().Length > 0 || txtApLubAmt.Text.Trim().Length > 0 || txtApMiscAmt.Text.Trim().Length > 0)
                            {
                                lnkSelectLabour.Style.Add("display", "");
                            }

                        }
                        txtLabourCode.Style.Add("display", "none");
                    }
                    else
                    {
                        //lblNewLabour.Style.Add("display", "none");
                    }
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        LabourDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E" || sRecordStatus == "S")
                    {
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                    }
                    if (bRecordIsOpen == false || Session["DepartmentID"].ToString() == "6")
                    {
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        lnkSelectLabour.Style.Add("display", "none");

                        drpFOC.Enabled = false;
                        drpJobCode.Enabled = false;
                        //drpMechName.Enabled = false;
                        drpFOCReason.Enabled = false;
                        drpSubletSupp.Enabled = false;
                        txtManHrs.Enabled = false;
                        drpOutMechName.Enabled = false;
                    }
                }
                txtLabourAmount.Text = dLabourAmount.ToString("0.00");
                txtSubletAmount.Text = dSubletAmount.ToString("0.00");
                txtJbTotAmt.Text = (Func.Convert.dConvertToDouble(txtPartAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLubricantAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLabourAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtSubletAmount.Text.ToString())).ToString("0.00");
                //lblLabourRecCnt.Text = Func.Common.sRowCntOfTable(dtLabour);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Click on Labour Select
        protected void lnkSelectLabour_Click(object sender, EventArgs e)
        {
            try
            {
                //bFillDetailsFromLabourTimeGrid();

                bFillDetailsFromLabourGrid();
                BindDataToLaborGrid(true, 0);

                //BindDataToLaborTimeGrid(true, 0);

                dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                CreateNewRowToTaxGroupDetailsTable();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbTaxDetails"] = dtJbTaxDetails;
                BindDataToGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Labour  Grid
        private bool bFillDetailsFromLabourGrid()
        {
            string sStatus = "";
            dtLabour = (DataTable)Session["LabourDetails"];
            int iCntForDelete = 0;
            int iLabourID = 0;
            bool bValidate = true;
            double dManHrs = 0;
            double dRate = 0;
            int iLbrDescriptionID = 0;
            string sLbrDescription = "";

            for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
            {
                //LabourID                
                iLabourID = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID") as TextBox).Text);

                if (iLabourID != 0)
                {
                    bValidate = true;
                    sCreateGP = "N";
                    //ID
                    //if (txtID.Text != "")
                    //    dtLabour.Rows[iRowCnt]["ID"] = 0;

                    // Labour ID
                    dtLabour.Rows[iRowCnt]["PartLabourID"] = iLabourID;

                    //Labour Code 
                    dtLabour.Rows[iRowCnt]["labour_code"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;

                    //Labour Description
                    dtLabour.Rows[iRowCnt]["PartLabourName"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc") as TextBox).Text;

                    //Labour Other Description
                    //dtLabour.Rows[iRowCnt]["AddLbrDescription"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription") as TextBox).Text;                    

                    iLbrDescriptionID = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription") as DropDownList).SelectedValue);
                    sLbrDescription = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription") as TextBox).Text.ToString();
                    dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"] = iLbrDescriptionID;

                    //Labor Additional Description
                    if (iLbrDescriptionID == Func.Convert.iConvertToInt("9999") && sLbrDescription != "")
                        dtLabour.Rows[iRowCnt]["AddLbrDescription"] = sLbrDescription;

                    //Lab Tag
                    dtLabour.Rows[iRowCnt]["Lab_Tag"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD") as TextBox).Text;

                    // Warr tag 
                    //dtLabour.Rows[iRowCnt]["war_tag"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabWarr") as TextBox).Text;

                    dtLabour.Rows[iRowCnt]["war_tag"] = Func.Convert.sConvertToString((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLabWarr") as DropDownList).SelectedValue.ToString());


                    // Get ManHrs
                    dManHrs = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["ManHrs"] = dManHrs;

                    // Get Rate
                    dRate = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Rate"] = dRate;

                    // Get Total
                    //dtLabour.Rows[iRowCnt]["Total"] = dManHrs * dRate;// Commit By Vikram dated 08092017
                    //Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);

                    //JobCode                
                    dtLabour.Rows[iRowCnt]["Job_Code_ID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                    //FOC Tag       foc_tag
                    dtLabour.Rows[iRowCnt]["foc_tag"] = Func.Convert.sConvertToString((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpFOC") as DropDownList).SelectedValue.Trim());

                    //FOC Reaseon       foc_reason_ID
                    dtLabour.Rows[iRowCnt]["foc_reason_ID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpFOCReason") as DropDownList).SelectedValue);

                    //Mech Name     Mech_ID
                    dtLabour.Rows[iRowCnt]["Mech_ID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpMechName") as DropDownList).SelectedValue);

                    //Sublet Supplier     VenderID                        
                    dtLabour.Rows[iRowCnt]["VenderID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpSubletSupp") as DropDownList).SelectedValue);

                    //Out Mech Name     out_Mech_ID                        
                    dtLabour.Rows[iRowCnt]["out_Mech_ID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpOutMechName") as DropDownList).SelectedValue);

                    //Labour Main group LbrMnGrp     
                    dtLabour.Rows[iRowCnt]["LbrMnGrp"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabMnGr") as TextBox).Text;

                    //Labour EstDtlID
                    dtLabour.Rows[iRowCnt]["EstDtlID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtEstDtlID") as TextBox).Text);

                    //Labour group code
                    dtLabour.Rows[iRowCnt]["group_code"] = Func.Convert.sConvertToString((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode") as TextBox).Text);

                    dtLabour.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox).Text);

                    //Repjob rep_job
                    dtLabour.Rows[iRowCnt]["rep_job"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRepjob") as TextBox).Text;

                    //Sublet Amt    out_Lab_amt
                    dtLabour.Rows[iRowCnt]["out_Lab_amt"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletAmt") as TextBox).Text);

                    //Sublet Labour Description     out_lab_desc
                    dtLabour.Rows[iRowCnt]["out_lab_desc"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletDescription") as TextBox).Text;

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Status"]);

                    dtLabour.Rows[iRowCnt]["Status"] = "S";// for Save      
                    dtLabour.Rows[iRowCnt]["AccdFlag"] = Func.Convert.sConvertToString((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpAccdFlag") as DropDownList).SelectedValue); ;// for Save      
                    
                    CheckBox Chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtLabour.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete != 0)
            {
                if (iCntForDelete == LabourDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                }
            }
            return bValidate;
        }

        //To Get Total Labour Count
        private void CalculateLabourGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sLabourID = "";
                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Labour Description            
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                    sLabourID = txtLabourID.Text;
                    if (sLabourID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblLabourRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Job Function
        // Create Row To Part Grid
        private void CreateNewRowToJobGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultJob = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxJobGridRowCount = 0;
                iMaxJobGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxJobGridRowCount"]);
                if (Session["JobDetails"] != null)
                {
                    dtDefaultJob = (DataTable)Session["JobDetails"];
                }
                else
                {
                    dtDefaultJob = dtJob;
                }

                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultJob.Rows.Count == 0)
                    {
                        dtDefaultJob.Columns.Clear();
                        dtDefaultJob.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_No", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("job_code", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Culprit_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Defect_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Technical_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Warratable_YN", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultJob.Rows.Count >= iMaxJobGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxJobGridRowCount;
                }

                iMaxJobGridRowCount = iMaxJobGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxJobGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultJob.NewRow();
                    dr["ID"] = 0;
                    dr["Part_No_ID"] = 0;
                    dr["Part_No"] = "";
                    dr["Part_Name"] = "";
                    dr["Job_Code_ID"] = 0;
                    dr["job_code"] = "";
                    dr["Culprit_ID"] = 0;
                    dr["Defect_ID"] = 0;
                    dr["Technical_ID"] = 0;
                    dr["Status"] = "N";
                    dr["Warratable_YN"] = "N";
                    dtDefaultJob.Rows.Add(dr);
                    dtDefaultJob.AcceptChanges();
                }

            Bind: ;
                Session["JobDetails"] = dtDefaultJob;
                JobDetailsGrid.DataSource = dtDefaultJob;
                JobDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Job Grid
        private bool bFillDetailsFromJobGrid(string sJbCdConfirm)
        {
            string sStatus = "";
            dtJob = (DataTable)Session["JobDetails"];
            int iCntForDelete = 0;
            //int iJobCodeID = 0;
            int iPartID = 0;
            bool bValidate = false;
            string sWarrantablePart = "";
            for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
            {
                //iJobCodeID = Func.Convert.iConvertToInt((JobDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);
                iPartID = Func.Convert.iConvertToInt((JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                ////ID
                //if (txtRefClaimID.Text != "")
                //dtJob.Rows[iRowCnt]["ID"] = 0;

                //JobCode                
                dtJob.Rows[iRowCnt]["Job_Code_ID"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID") as TextBox).Text;

                //JobCode                
                dtJob.Rows[iRowCnt]["job_code"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode") as Label).Text;

                //PartID                               
                bValidate = true;
                dtJob.Rows[iRowCnt]["Part_No_ID"] = iPartID;

                //Part No
                dtJob.Rows[iRowCnt]["Part_No"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

                //Part Name
                dtJob.Rows[iRowCnt]["Part_Name"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text;

                //culprit Code  

                DropDownList drpCulpritCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpCulpritCode");
                dtJob.Rows[iRowCnt]["Culprit_ID"] = Func.Convert.iConvertToInt(drpCulpritCode.SelectedValue);

                //Defect Code
                DropDownList drpDefectCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpDefectCode");
                dtJob.Rows[iRowCnt]["Defect_ID"] = Func.Convert.iConvertToInt(drpDefectCode.SelectedValue);

                //Technical Code
                DropDownList drpTechnicalCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpTechnicalCode");
                dtJob.Rows[iRowCnt]["Technical_ID"] = Func.Convert.iConvertToInt(drpTechnicalCode.SelectedValue);
                sWarrantablePart = Func.Convert.sConvertToString((JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart") as TextBox).Text);

                //Warrantable Part 
                dtJob.Rows[iRowCnt]["Warratable_YN"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart") as TextBox).Text;

                TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");
                TextBox txtWarrJobCode = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrJobCode");

                // Record Status
                dtJob.Rows[iRowCnt]["Status"] = "S";

                if (txtWarrJobCode.Text == "Y" && sJbCdConfirm == "Y" && (iPartID == 0 || Func.Convert.iConvertToInt(drpCulpritCode.SelectedValue) == 0 || Func.Convert.iConvertToInt(drpDefectCode.SelectedValue) == 0))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
                    return false;
                }
                TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");
                if (txtWarrJobCode.Text == "Y" && sJbCdConfirm == "Y" && (txtPCRHDRID.Text.Trim() == "" || txtPCRHDRID.Text.Trim() == "0"))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Create PCR For All JobCode.');</script>");
                    return false;
                }

                //if (sWarrantablePart.ToUpper() == "N")
                //{
                //    if (dtJob.Rows[iRowCnt]["Technical_ID"].ToString() == "0")
                //    {
                //        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select The Technical Code.');</script>");
                //        return false;
                //    }
                //}

            }

            if (iCntForDelete == JobDetailsGrid.Rows.Count)
            {
                bValidate = false;
            }

            Session["JobDetails"] = dtJob;

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
            }
            return bValidate;
        }

        protected void lnkJbSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + hdnSelectedPartID.Value + ".');</script>");
                //bFillDetailsFromPartGrid();
                //BindDataToPartGrid(true, 0);
                bFillDetailsFromJobGrid("N");
                BindDataToJobGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Set Control property To Job Grid
        private void SetControlPropertyToJobGrid()
        {
            try
            {
                string sRecordStatus = "N";
                int idtRowCnt = 0;
                string sPartName = "";
                string sPartId = "";
                //JobDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Job ID
                    TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID");

                    //txtJobCodeDtlID
                    TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");

                    //txtWarrJobCode
                    TextBox txtWarrJobCode = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrJobCode");

                    //Job Code 
                    Label lblJobCode = (Label)JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode");

                    // Culprit  Part Control                
                    LinkButton lnkJbSelectPart = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkJbSelectPart");
                    lnkJbSelectPart.Attributes.Add("onclick", "return ShowPartMaster(this,'" + Func.Convert.sConvertToString(Location.iDealerId.ToString()) + "','" + lblJobCode.Text + "','N');");

                    //Part No
                    TextBox txtPartNo = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    //culprit Code
                    DropDownList drpCulpritCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpCulpritCode");
                    //Func.Common.BindDataToCombo(drpCulpritCode, clsCommon.ComboQueryType.CulpritCode, 0);
                    Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0);

                    //Defect Code
                    DropDownList drpDefectCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpDefectCode");
                    //Func.Common.BindDataToCombo(drpDefectCode, clsCommon.ComboQueryType.DefectCode, 0);                    
                    Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0);

                    //Technical Code
                    DropDownList drpTechnicalCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpTechnicalCode");
                    Func.Common.BindDataToCombo(drpTechnicalCode, clsCommon.ComboQueryType.TechnicalCode, 0);
                    drpTechnicalCode.Attributes.Add("disabled", "disabled");

                    //txt warrantable Part 
                    TextBox txtWarrantablePart = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart");

                    //link To PCR
                    LinkButton lnkSelectJobDtl = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectJobDtl");

                    //PCR HDR ID
                    TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");

                    sRecordStatus = "N";
                    sPartId = "0";

                    // Get Value from Culprit details table
                    if (idtRowCnt < dtJob.Rows.Count)
                    {
                        //Jobcode defined for Warranty or not
                        txtWarrJobCode.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["WarrJobcode"]);

                        //JobCode Dtls saved or not
                        txtJobCodeDtlSaved.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["JobCodeDtlSaved"]);

                        //Job Code ID
                        txtJobID.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Job_Code_ID"]);

                        //Jobcode
                        lblJobCode.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["job_code"]);

                        //Part ID       
                        sPartId = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_No_ID"]);
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text = sPartId;

                        txtPCRHDRID.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["PCRHDRID"]);

                        lnkSelectJobDtl.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + Location.iDealerId.ToString() + "','" + txtJobID.Text + "','" + txtPCRHDRID.Text.ToString() + "','" + txtJobCodeDtlSaved.Text.Trim() + "')");

                        lnkSelectJobDtl.Style.Add("display", (txtJobCodeDtlSaved.Text.Trim() == "N" || txtJobCodeDtlSaved.Text.Trim() == "") ? "none" : "");

                        //Part No
                        txtPartNo.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_No"]);

                        //Part Name
                        sPartName = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_Name"]);
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text = sPartName;
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).ToolTip = sPartName;

                        sRecordStatus = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Status"]);

                        Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Culprit_ID"]) + ")");
                        Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Defect_ID"]) + ")");

                        drpCulpritCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Culprit_ID"]);
                        drpDefectCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Defect_ID"]);
                        drpTechnicalCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Technical_ID"]);
                        txtWarrantablePart.Text = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Warratable_YN"]);

                        if (txtWarrantablePart.Text == "N")
                        {
                            drpTechnicalCode.Attributes.Remove("disabled");
                        }

                        idtRowCnt = idtRowCnt + 1;
                    }

                    //lnkCancel.Style.Add("display", "none");
                    if (sRecordStatus == "U")
                    {
                        txtPartNo.ReadOnly = true;

                    }

                    //If Part Id  is not allocated
                    if (sPartId == "0")
                    {
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                        //lblNewPart.Style.Add("display", "");
                        lnkJbSelectPart.Style.Add("display", "");

                    if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkJbSelectPart.Style.Add("display", "none");
                        drpDefectCode.Enabled = false;
                        drpCulpritCode.Enabled = false;
                        drpTechnicalCode.Enabled = false;
                    }
                }
                //lblJobRecCnt.Text = Func.Common.sRowCntOfTable(dtJob);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        //To Get Total Job Count
        private void CalculateJobGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sJobID = "";
                for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Job Description            
                    TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    sJobID = txtJobID.Text;
                    if (sJobID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblJobRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void btnJobSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsJobcard ObjJobcard = new clsJobcard();
                string sJbCdConfirm = "N";
                if (bFillDetailsFromJobGrid(sJbCdConfirm) == true)
                {
                    if (ObjJobcard.bSaveJobcodeDetails(Func.Convert.iConvertToInt(txtID.Text), dtJob, sJbCdConfirm) == true)
                    {
                        for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                        {
                            TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID");

                            TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");

                            TextBox txtPartNo = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo");

                            Label lblJobCode = (Label)JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode");

                            TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");

                            LinkButton lnkSelectJobDtl = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectJobDtl");

                            if (txtPartNo.Text.Trim() != "" && txtPartNo.Text.Trim() != "0") txtJobCodeDtlSaved.Text = "Y";

                            lnkSelectJobDtl.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + Location.iDealerId.ToString() + "','" + txtJobID.Text + "','" + txtPCRHDRID.Text.ToString() + "','" + txtJobCodeDtlSaved.Text.Trim() + "')");

                            lnkSelectJobDtl.Style.Add("display", (txtJobCodeDtlSaved.Text.Trim() == "N" || txtJobCodeDtlSaved.Text.Trim() == "") ? "none" : "");
                        }
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Saved JobCode Details.');</script>");
                    }
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        //Bind Data to Job Grid
        private void BindDataToJobGrid()
        {
            try
            {
                JobDetailsGrid.DataSource = dtJob;
                JobDetailsGrid.DataBind();
                SetControlPropertyToJobGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region FreeService Function

        // Bind Data to labour Grid
        private void BindDataToFreeServiceGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                FreeServicesGrid.DataSource = dtFreeService;
                FreeServicesGrid.DataBind();
                ChkSelectedFreeService();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        //Fill Details From Free Service  Grid
        private bool bFillDetailsFromFreeServiceGrid()
        {

            dtFreeService = (DataTable)Session["FreeServiceDtls"];
            int iCouponID = 0;
            int iSrvjobID = 0;
            bool bValidate = false;
            string sSelect = "";
            if (Func.Convert.iConvertToInt(txtID.Text.ToString()) == 0 || (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) != 3 && Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) != 7)) bValidate = true;
            //if (Func.Convert.iConvertToInt(txtID.Text.ToString()) == 0 ) bValidate = true;

            for (int iRowCnt = 0; iRowCnt < FreeServicesGrid.Rows.Count; iRowCnt++)
            {
                //CouponID   
                sSelect = "";
                iCouponID = Func.Convert.iConvertToInt((FreeServicesGrid.Rows[iRowCnt].FindControl("lblSrvo") as Label).Text);
                iSrvjobID = Func.Convert.iConvertToInt((FreeServicesGrid.Rows[iRowCnt].FindControl("txtSrvJobID") as TextBox).Text);
                dtFreeService.Rows[iRowCnt]["JobID"] = iSrvjobID;
                if (iSrvjobID == Func.Convert.iConvertToInt(txtID.Text.ToString()) && Func.Convert.iConvertToInt(txtID.Text.ToString()) > 0 && (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 3 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 7))
                //if (iSrvjobID == Func.Convert.iConvertToInt(txtID.Text.ToString()) && Func.Convert.iConvertToInt(txtID.Text.ToString()) > 0)
                //if (lblChk.Text.Trim() == "Y" && (Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 3 || Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 7))
                {
                    bValidate = true;
                }
            }
            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the Free Service Coupon.');</script>");
            }
            return bValidate;
        }

        //To Get Total Free Service Count
        private void CalculateFreeServiceGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sLabourID = "";
                for (int iRowCnt = 0; iRowCnt < FreeServicesGrid.Rows.Count; iRowCnt++)
                {
                    //Labour Description            
                    TextBox txtLabourID = (TextBox)FreeServicesGrid.Rows[iRowCnt].FindControl("txtLabourID");

                    sLabourID = txtLabourID.Text;
                    if (sLabourID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblFreeServicesRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //To cheked Free Service 
        private void ChkSelectedFreeService()
        {
            int iCouponID = 0;
            int iSrvjobID = 0;
            int iLstUserSrvID = 0;
            string sServName = "";
            string sValidDt = "";
            CheckBox chk = new CheckBox();
            int iJobTypeID = Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString());
            for (int iRowCnt = 0; iRowCnt < FreeServicesGrid.Rows.Count; iRowCnt++)
            {
                //CouponID                
                iCouponID = Func.Convert.iConvertToInt((FreeServicesGrid.Rows[iRowCnt].FindControl("lblSrvo") as Label).Text);
                iSrvjobID = Func.Convert.iConvertToInt((FreeServicesGrid.Rows[iRowCnt].FindControl("txtSrvJobID") as TextBox).Text);
                iLstUserSrvID = Func.Convert.iConvertToInt((FreeServicesGrid.Rows[iRowCnt].FindControl("lblLstSelSrvID") as Label).Text);
                sServName = Func.Convert.sConvertToString((FreeServicesGrid.Rows[iRowCnt].FindControl("lblSrvName") as Label).Text.Trim());
                Label lblSrvJobDtl = (Label)(FreeServicesGrid.Rows[iRowCnt].FindControl("lblSrvJobDtl"));
                sValidDt = Func.Convert.sConvertToString((FreeServicesGrid.Rows[iRowCnt].FindControl("lblValidDt") as Label).Text.Trim());
                chk = (CheckBox)FreeServicesGrid.Rows[iRowCnt].FindControl("ChkService");
                chk.Checked = (iSrvjobID != 0) ? true : false;
                if (sServName.Trim() == "PDI" && iJobTypeID == 7)
                {
                    chk.Checked = true;
                    lblSrvJobDtl.Text = txtDocNo.Text + " (" + txtDocDate.Text + ")";
                    (FreeServicesGrid.Rows[iRowCnt].FindControl("txtSrvJobID") as TextBox).Text = txtID.Text;
                }

                //chk.Enabled = (iSrvjobID == 0 || iSrvjobID == Func.Convert.iConvertToInt(txtID.Text)) ? true : false;
                //chk.Enabled = (iSrvjobID == 0 && iJobTypeID == 3 && iLstUserSrvID > iCouponID) ? true : false;
                chk.Enabled = ((iSrvjobID == 0 || iSrvjobID == Func.Convert.iConvertToInt(txtID.Text)) && iJobTypeID == 3 && iLstUserSrvID < iCouponID && sValidDt == "Y") ? true : false;

            }
        }

        #endregion

        #region File Attachment Functions
        // To Save the attach Document 
        private bool bSaveAttachedDocuments(bool bSaveWithConfirm)
        {
            int iFileCount = 0;
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    CheckBox ChkForDelete;
                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {

                    }
                    else
                    {
                        iFileCount = iFileCount + 1;
                    }

                }
            }

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            string sSourceFileName1 = "";
            DataRow dr;
            int iRecorFound = 0;
            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);

                    if (sSourceFileName.Trim() != "")
                    {
                        //if (upload.ContentLength == 0)                continue;
                        dr = dtFileAttach.NewRow();

                        dr["ID"] = 0;

                        // Retriveing the Description of the File

                        for (int iCnt = iRecorFound; iCnt < 20; iCnt++)
                        {
                            if (Request.Form["Text" + (iCnt + 1)] != null)
                            {
                                iRecorFound = iCnt + 1;
                                dr["Description"] = Request.Form["Text" + (iCnt + 1).ToString()];
                                break;
                            }
                        }


                        //dr["File_Names"] = sSourceFileName;
                        //if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmNormal)                       
                        sSourceFileName1 = Func.Convert.sConvertToString(Location.iDealerId.ToString()) + "_" + Func.Convert.sConvertToString(txtDocNo.Text) + "_" + sSourceFileName;
                        sSourceFileName1 = sSourceFileName1.Replace("/", "");
                        dr["File_Names"] = sSourceFileName1;
                        dr["UserId"] = Func.Convert.sConvertToString(Location.iDealerId.ToString());
                        dr["Status"] = "S";
                        iFileCount = iFileCount + 1;
                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        //Saving it in temperory Directory.               

                        if (!System.IO.Directory.Exists(sPath + "Jobcard"))
                            System.IO.Directory.CreateDirectory(sPath + "Jobcard");
                        uploads[i].SaveAs((sPath + "Jobcard" + "\\" + sSourceFileName1));
                    }

                }

                catch (Exception ex)
                {
                    return false;
                    Func.Common.ProcessUnhandledException(ex);
                }

            }
            if (iFileCount == 0 && Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString()) == 7 && bSaveWithConfirm == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Attach checklist.');</script>");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void bFillDetailsFromFileAttachGrid()
        {
            bool bSaveRecord = false;
            DataRow dr;
            int iFileCnt = 0;
            dtFileAttach = new DataTable();
            //Get Header InFormation        
            dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
                    dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(Location.iDealerId.ToString());

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                        iFileCnt = iFileCnt + 1;
                    }
                    dtFileAttach.Rows.Add(dr);
                    dtFileAttach.AcceptChanges();
                }
            }
        }

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }

        protected void FileAttchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblFile = (Label)e.Row.FindControl("lblFile");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (lblFile != null)
                lblFile.Text = DataBinder.Eval(e.Row.DataItem, "File_Names").ToString().Replace(" ", "&nbsp;");

        }
        #endregion

        //#region LabourTiming Function

        //// Create Row To Labour Grid
        //private void CreateNewRowToLabourTimeGrid(int iNoRowToAdd)
        //{
        //    try
        //    {
        //        DataRow dr;
        //        int iMaxLabourGridRowCount = 0;
        //        DataTable dtDefaultLabourTm = new DataTable();
        //        int iRowCntStartFrom = 0;
        //        iMaxLabourGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxLabourGridRowCount"]);
        //        if (Session["LabourTimeDetails"] != null)
        //        {
        //            dtDefaultLabourTm = (DataTable)Session["LabourTimeDetails"];
        //        }
        //        else
        //        {
        //            dtDefaultLabourTm = dtLabourTime;
        //        }

        //        if (iNoRowToAdd == 0)
        //        {
        //            if (dtDefaultLabourTm.Rows.Count == 0)
        //            {
        //                dtDefaultLabourTm.Columns.Clear();
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("ID", typeof(int)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("Labour_ID", typeof(int)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("Lab_Tag", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("StartTime", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("PauseReason", typeof(int)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("PauseTime", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("EndTime", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("SRNo", typeof(string)));
        //                dtDefaultLabourTm.Columns.Add(new DataColumn("Status", typeof(string)));
        //                //dtDefaultLabourTm.Columns.Add(new DataColumn("DtlID", typeof(int)));                        
        //            }
        //            else
        //            {
        //                if (dtDefaultLabourTm.Rows.Count >= iMaxLabourGridRowCount) goto Bind;
        //            }
        //        }
        //        else
        //        {
        //            iRowCntStartFrom = iMaxLabourGridRowCount;
        //        }

        //        iMaxLabourGridRowCount = iMaxLabourGridRowCount + iNoRowToAdd;

        //        for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLabourGridRowCount; iRowCnt++)
        //        {
        //            dr = dtDefaultLabourTm.NewRow();
        //            dr["SRNo"] = "1";
        //            dr["ID"] = 0;
        //            dr["Labour_ID"] = 0;
        //            dr["labour_code"] = "";
        //            dr["PartLabourName"] = "";
        //            dr["Lab_Tag"] = "";
        //            dr["StartTime"] = "";
        //            dr["PauseReason"] = 0;
        //            dr["PauseTime"] = "";
        //            dr["EndTime"] = "";
        //            dr["Status"] = "S";                    
        //            //dr["DtlID"] = 0;

        //            dtDefaultLabourTm.Rows.Add(dr);
        //            dtDefaultLabourTm.AcceptChanges();
        //        }

        //    Bind:
        //        Session["LabourTimeDetails"] = dtDefaultLabourTm;
        //        LabourTimeDetailsGrid.DataSource = dtDefaultLabourTm;
        //        LabourTimeDetailsGrid.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        //// Bind Data to labour Grid
        //private void BindDataToLaborTimeGrid(bool bRecordIsOpen, int iNoRowToAdd)
        //{
        //    try
        //    {
        //        if (bRecordIsOpen == true)
        //        {
        //            CreateNewRowToLabourTimeGrid(iNoRowToAdd);
        //            SetControlPropertyToLabourTimeGrid(bRecordIsOpen);
        //        }
        //        else
        //        {
        //            LabourTimeDetailsGrid.DataSource = dtLabourTime;
        //            LabourTimeDetailsGrid.DataBind();
        //            SetControlPropertyToLabourTimeGrid(bRecordIsOpen);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        //// Set Control property To Labour Grid
        //private void SetControlPropertyToLabourTimeGrid(bool bRecordIsOpen)
        //{
        //    try
        //    {
        //        string sRecordStatus = "N";
        //        // Hide Labour Id  Column
        //        if (LabourTimeDetailsGrid.Rows.Count == 0) return;
        //        LabourTimeDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none");
        //        string sLabourId = "0", sLabTag = "", sLabCode = "";
        //        int idtRowCnt = 0;

        //        for (int iRowCnt = 0; iRowCnt < LabourTimeDetailsGrid.Rows.Count; iRowCnt++)
        //        {
        //            //LabourID                                
        //            TextBox txtLabourID = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
        //            LabourTimeDetailsGrid.Rows[iRowCnt].Cells[1].Style.Add("display", "none");

        //            //LabourNo Or NewLabour
        //            TextBox txtLabourCode = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");

        //            //Labour Name
        //            TextBox txtLabourDesc = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc");
        //            //txtLabourDesc.Attributes.Add("disabled", "disabled");

        //            ////Other Description
        //            //TextBox txtLabourDesc = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc");

        //            //Lab Tag
        //            TextBox txtLabCD = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD");
        //            TextBox txtStartTime = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtStartTime");
        //            TextBox txtPauseTime = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtPauseTime");
        //            TextBox txtEndTime = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtEndTime");
        //            TextBox txtSrNo = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtSrNo");
        //            //Mech Name     Mech_ID
        //            TextBox DrpPauseReason = (TextBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("DrpPauseReason");
        //            //DropDownList DrpPauseReason = (DropDownList)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("DrpPauseReason");
        //            //Func.Common.BindDataToCombo(DrpPauseReason, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type = 2");

        //            sLabourId = "0";
        //            sRecordStatus = "N";

        //            if (idtRowCnt < dtLabourTime.Rows.Count)
        //            {
        //                // labour ID 
        //                sLabourId = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["PartLabourID"]);
        //                txtLabourID.Text = sLabourId;

        //                //Labour Code 
        //                txtLabourCode.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["labour_code"]);
        //                txtLabourCode.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

        //                //Labour Description
        //                txtLabourDesc.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["PartLabourName"]);

        //                //txtLbrDescription.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["AddLbrDescription"]);

        //                //sAddLbrID = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["AddLbrDescriptionID"]);
        //                // Add New Lbr Description

        //                txtLabCD.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["Lab_Tag"]);

        //                sLabCode = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["labour_code"]);
        //                sLabTag = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["Lab_Tag"]).Trim();

        //                txtStartTime.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["StartTime"]);
        //                txtPauseTime.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["PauseTime"]).Trim();
        //                txtEndTime.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["EndTime"]).Trim();
        //               // DrpPauseReason.SelectedValue = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["PauseReason"]).Trim();
        //                DrpPauseReason.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["PauseReason"]).Trim();
        //                txtSrNo.Text = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["SRNo"]).Trim();
        //                sRecordStatus = Func.Convert.sConvertToString(dtLabourTime.Rows[iRowCnt]["Status"]);
        //                idtRowCnt = idtRowCnt + 1;
        //            }

        //            Label lnkCancel = (Label)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

        //            //Delete 
        //            CheckBox Chk = (CheckBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
        //            Chk.Style.Add("display", "none");

        //            Chk.Checked = false;
        //            if (sRecordStatus == "D")
        //            {
        //                Chk.Style.Add("display", "");
        //                LabourTimeDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
        //                Chk.Checked = true;
        //            }
        //            else if (sRecordStatus == "E" || sRecordStatus == "S")
        //            {
        //                Chk.Style.Add("display", "");
        //                lnkCancel.Style.Add("display", "none");
        //            }
        //            if (bRecordIsOpen == false || Session["DepartmentID"].ToString() == "6")
        //            {
        //                Chk.Style.Add("display", "none");
        //                lnkCancel.Style.Add("display", "none");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        ////Fill Details From Labour  Grid
        //private bool bFillDetailsFromLabourTimeGrid()
        //{
        //    dtLabourTime = (DataTable)Session["LabourTimeDetails"];
        //    int iLabourID = 0;
        //    bool bValidate = true;

        //    for (int iRowCnt = 0; iRowCnt < LabourTimeDetailsGrid.Rows.Count; iRowCnt++)
        //    {
        //        //LabourID                
        //        iLabourID = Func.Convert.iConvertToInt((LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID") as TextBox).Text);

        //        if (iLabourID != 0)
        //        {
        //            bValidate = true;
        //            //ID
        //            //if (txtID.Text != "")
        //            //    dtLabourTime.Rows[iRowCnt]["ID"] = 0;

        //            // Labour ID
        //            dtLabourTime.Rows[iRowCnt]["PartLabourID"] = iLabourID;

        //            //Labour Code 
        //            dtLabourTime.Rows[iRowCnt]["labour_code"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;

        //            //Labour Code 
        //            dtLabourTime.Rows[iRowCnt]["labour_code"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;

        //            //Labour Description
        //            dtLabourTime.Rows[iRowCnt]["PartLabourName"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc") as TextBox).Text;

        //            //Lab Tag
        //            dtLabourTime.Rows[iRowCnt]["Lab_Tag"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD") as TextBox).Text;

        //            //PauseReasonID                
        //           // dtLabourTime.Rows[iRowCnt]["PauseReason"] = Func.Convert.iConvertToInt((LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("DrpPauseReason") as DropDownList).SelectedValue);
        //            dtLabourTime.Rows[iRowCnt]["PauseReason"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("DrpPauseReason") as TextBox).Text;

        //            //Labour Start/Resume Time     
        //            dtLabourTime.Rows[iRowCnt]["StartTime"] = (LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtStartTime") as TextBox).Text;

        //            //Labour Pause Time
        //            dtLabourTime.Rows[iRowCnt]["PauseTime"] = Func.Convert.sConvertToString((LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtPauseTime") as TextBox).Text);

        //            //Labour End Time
        //            dtLabourTime.Rows[iRowCnt]["EndTime"] = Func.Convert.sConvertToString((LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtEndTime") as TextBox).Text);

        //            dtLabourTime.Rows[iRowCnt]["SRNo"] = Func.Convert.sConvertToString((LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("txtSrNo") as TextBox).Text);

        //            dtLabourTime.Rows[iRowCnt]["Status"] = "S";// for Save 

        //            CheckBox Chk = (CheckBox)LabourTimeDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
        //            if (Chk.Checked == true)
        //            {
        //                dtLabourTime.Rows[iRowCnt]["Status"] = "D";

        //            }
        //        }
        //    }
        //    return bValidate;
        //}
        //#endregion

        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select records.";
            bool bValidateRecord = true;

            if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 6 && Func.Convert.iConvertToInt(txtID.Text) == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Store User Cannot create Jobcard.');</script>");
                bValidateRecord = false;
                return bValidateRecord;
            }

            if (txtDocDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            if (Func.Convert.iConvertToInt(txtID.Text) == 0 && Func.Convert.sConvertToString(hdnPDIDone.Value.Trim()) == "Y" && (Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 7 || Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 12 || Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 16))
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PDI already done for this chassis. you can not create " + ((Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 7) ? "PDI" : ((Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 12) ? "Presale" : "PrePDI")) + " jobcard.');</script>");
                bValidateRecord = false;
                return bValidateRecord;
            }

            //Validation For Model Ramarks
            return bValidateRecord;
        }

        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;


                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("job_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("JobDate", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Job_Ty_ID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("KmsJobcard", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("HrsJobcard", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("OdoMeterChange", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("HrsMeterChange", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("kmsTot", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("hrsTot", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Chassis_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("BayID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("bay_allot_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("VehIn", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("JobOpTime", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("VehCommitTime", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("VehOut", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("EstmtID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("VehIn_HDR_Id", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("ApPartAmt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("ApLabAmt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("ApLubAmt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("ApMiscAmt", typeof(double)));


                dtHdr.Columns.Add(new DataColumn("job_confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("job_canc_tag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("JbCd_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("SupervisiorID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Failure_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("EstmtdTm", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("delay_Rsn_Id", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Aggregate", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("WarrantyTag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("chassis_sale_date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("In_KAM", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UpgrdCamp", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AMC_Chk", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AMC_End_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AMC_Type", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("AMC_Start_KM", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("AMC_End_KM", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("AMC_Start_Hrs", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("AMC_End_Hrs", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Warranty_Kms", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Warranty_hrs", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Extended_Start_Kms", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Extended_End_Kms", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Extended_Start_Hrs", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Extended_End_Hrs", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("MTI_Cat", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UndObserv", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Theft_flag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Float_flag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UndObservEffFrom", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UndObservEffTo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LstKm", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("LstJbKm", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Bfr_Last_SpdMtrChange_KmsJb", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Bfr_Last_SpdMtrChange_Kms", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("LstHrs", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("LstJbHrs", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Bfr_Last_HrsMtrChange_HrsJb", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Bfr_Last_HrsMtrChange_Hrs", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("CRM_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Model_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("AggregateNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Warranty_End_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Extended_Warranty_start_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Extended_Waranty_End_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AMC_St_Date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Additional_Warranty_Start_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Additional_Warranty_End_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsCustID", typeof(int)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["job_no"] = txtDocNo.Text;
                dr["JobDate"] = txtDocDate.Text;

                dr["Dealer_ID"] = Location.iDealerId;

                if (txtUserType.Text == "6")
                {
                    dr["DlrBranchID"] = Location.iDealerId;
                }
                else
                {
                    dr["DlrBranchID"] = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                }
                dr["Job_Ty_ID"] = Func.Convert.iConvertToInt(drpJobType.SelectedValue.ToString());

                dr["kmsTot"] = Func.Convert.iConvertToInt(txtTotKm.Text.ToString());
                dr["hrsTot"] = Func.Convert.iConvertToInt(txtTotHrs.Text.ToString());

                dr["KmsJobcard"] = Func.Convert.iConvertToInt(txtKms.Text.ToString());
                dr["HrsJobcard"] = Func.Convert.iConvertToInt(txtHrs.Text.ToString());

                dr["OdoMeterChange"] = (Func.Convert.sConvertToString(txtSpdMtrChg.Text.ToString().Trim()) == "Yes") ? "Y" : "N";
                dr["HrsMeterChange"] = (Func.Convert.sConvertToString(txtHrsMtrChg.Text.ToString().Trim()) == "Yes") ? "Y" : "N";

                //dr["CustID"] = Func.Convert.iConvertToInt(txtCustID.Text.ToString());
                dr["CustID"] = (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? Func.Convert.iConvertToInt(DrpCustomer.SelectedValue) : Func.Convert.iConvertToInt(txtCustID.Text.ToString());

                dr["Chassis_ID"] = Func.Convert.iConvertToInt(txtChassisID.Text.ToString());
                dr["BayID"] = Func.Convert.iConvertToInt(DrpBay.SelectedValue.ToString());

                dr["chassis_sale_date"] = Func.Convert.sConvertToString(dtpVehSaleDt.Text);

                dr["bay_allot_date"] = dtpAllocateTime.Text;
                dr["CRM_HDR_ID"] = Func.Convert.iConvertToInt(txtCRMID.Text);

                dr["VehIn"] = dtpVehInTime.Text;
                dr["JobOpTime"] = dtpJobOpeningTm.Text;
                dr["VehCommitTime"] = dtpJobCommited.Text;
                dr["VehOut"] = DtpVehicleOut.Text;

                dr["EstmtID"] = Func.Convert.iConvertToInt(DrpEstimate.SelectedValue.ToString());
                dr["VehIn_HDR_Id"] = Func.Convert.iConvertToInt(txtPreviousDocId.Text.ToString());

                dr["ApPartAmt"] = Func.Convert.dConvertToDouble(txtApPartAmt.Text);
                dr["ApLabAmt"] = Func.Convert.dConvertToDouble(txtApLabAmt.Text);
                dr["ApLubAmt"] = Func.Convert.dConvertToDouble(txtApLubAmt.Text);
                dr["ApMiscAmt"] = Func.Convert.dConvertToDouble(txtApMiscAmt.Text);

                dr["job_confirm"] = "N";
                dr["job_canc_tag"] = "N";
                dr["JbCd_Confirm"] = "N";

                dr["UserID"] = Func.Convert.iConvertToInt(Session["UserID"].ToString());

                dr["SupervisiorID"] = Func.Convert.iConvertToInt(DrpSupervisorName.SelectedValue.ToString());
                dr["Failure_Date"] = txtFailureDt.Text;
                dr["EstmtdTm"] = Func.Convert.dConvertToDouble(lblEstmtdTm.Text);

                dr["delay_Rsn_Id"] = Func.Convert.iConvertToInt(DrpDelayReason.SelectedValue.ToString());

                dr["Aggregate"] = txtAggregate.Text.ToString();
                dr["WarrantyTag"] = txtWarrantyTag.Text.ToString();

                //Chassis Status Details Begin                                                
                dr["In_KAM"] = Func.Convert.sConvertToString(hdnKAM.Value.Trim());
                dr["UpgrdCamp"] = Func.Convert.sConvertToString(hdnUpgCamp.Value.Trim());
                dr["AMC_Chk"] = Func.Convert.sConvertToString(txtAMCChk.Text.Trim());
                dr["AMC_End_Date"] = Func.Convert.sConvertToString(txtAMCDate.Text);
                dr["AMC_Type"] = Func.Convert.sConvertToString(hdnAMCType.Value.Trim());

                dr["AMC_Start_KM"] = Func.Convert.sConvertToString(hdnAMCStKms.Value.Trim());
                dr["AMC_End_KM"] = Func.Convert.sConvertToString(hdnAMCEndKms.Value.Trim());

                dr["AMC_Start_Hrs"] = Func.Convert.sConvertToString(hdnAMCStHrs.Value.Trim());
                dr["AMC_End_Hrs"] = Func.Convert.sConvertToString(hdnAMCEndHrs.Value.Trim());

                dr["Warranty_Kms"] = Func.Convert.sConvertToString(hdnWarrEndKms.Value.Trim());
                dr["Warranty_hrs"] = Func.Convert.sConvertToString(hdnWarrEndHrs.Value.Trim());

                dr["Extended_Start_Kms"] = Func.Convert.sConvertToString(hdnExtWarrStartKms.Value.Trim());
                dr["Extended_End_Kms"] = Func.Convert.sConvertToString(hdnExtWarrEndKms.Value.Trim());

                dr["Extended_Start_Hrs"] = Func.Convert.sConvertToString(hdnExtWarrStartHrs.Value.Trim());
                dr["Extended_End_Hrs"] = Func.Convert.sConvertToString(hdnExtWarrEndHrs.Value.Trim());

                dr["MTI_Cat"] = Func.Convert.sConvertToString(hdnWarrChkType.Value.Trim());
                dr["Warranty_End_Date"] = Func.Convert.sConvertToString(hdnWarrEndDt.Value.Trim());
                dr["Extended_Warranty_start_Date"] = Func.Convert.sConvertToString(hdnExtWarrStDt.Value.Trim());
                dr["Extended_Waranty_End_Date"] = Func.Convert.sConvertToString(hdnExtWarrEndDt.Value.Trim());

                dr["Additional_Warranty_Start_date"] = Func.Convert.sConvertToString(hdnAddWarrStDt.Value.Trim());
                dr["Additional_Warranty_End_Date"] = Func.Convert.sConvertToString(hdnAddWarrEndDt.Value.Trim());

                dr["AMC_St_Date"] = Func.Convert.sConvertToString(hdnAMCStDt.Value.Trim());
                dr["UndObserv"] = Func.Convert.sConvertToString(hdnUndObserv.Value.Trim());
                dr["Theft_flag"] = Func.Convert.sConvertToString(IsTheft.Value.Trim());
                dr["Float_flag"] = Func.Convert.sConvertToString(hdnFloatPart.Value.Trim());
                dr["UndObservEffFrom"] = Func.Convert.sConvertToString(txtObservEffFrom.Text);
                dr["UndObservEffTo"] = Func.Convert.sConvertToString(txtObservEffTo.Text);

                dr["LstKm"] = Func.Convert.iConvertToInt(txtLastKms.Text);
                dr["LstJbKm"] = Func.Convert.iConvertToInt(txtLstJbKms.Text);
                dr["Bfr_Last_SpdMtrChange_Kms"] = Func.Convert.iConvertToInt(txtBfr_Last_SpdMtrChange_Kms.Text);
                dr["Bfr_Last_SpdMtrChange_KmsJb"] = Func.Convert.iConvertToInt(txthdnBfr_Last_SpdMtrChange_Kms.Text);

                dr["LstHrs"] = Func.Convert.iConvertToInt(txtLastHrs.Text);
                dr["LstJbHrs"] = Func.Convert.iConvertToInt(txtLstJbHrs.Text);
                dr["Bfr_Last_HrsMtrChange_Hrs"] = Func.Convert.iConvertToInt(txtBfr_Last_HrsMtrChange_Hrs.Text);
                dr["Bfr_Last_HrsMtrChange_HrsJb"] = Func.Convert.iConvertToInt(txthdnBfr_Last_HrsMtrChange_Hrs.Text);
                //Chassis Status Details end                
                dr["DocGST"] = Func.Convert.sConvertToString(hdnISDocGST.Value);
                //change For Aggregate
                dr["Model_ID"] = (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? Func.Convert.iConvertToInt(DrpModelCode.SelectedValue) : 0;
                dr["AggregateNo"] = Func.Convert.sConvertToString(txtAggreagateNo.Text);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);
                dr["InsCustID"] = Func.Convert.iConvertToInt(DrpInsurnceComp.SelectedValue);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        //ToSave Record
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            try
            {
                bool bEnblSave = false;
                if (bSaveWithConfirm == false && bSaveWithCancel == false)
                {
                    bEnblSave = true;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                if (bValidateRecord() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (bSaveWithConfirm == true && NeedGoodwillApproval() == true && (txtGDReqID.Text == "" || txtGDReqID.Text == "0"))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Goodwill Request Pending.');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (bSaveWithConfirm == true && NeedHighValueApproval() == true && (txtHVReqID.Text == "" || txtHVReqID.Text == "0"))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('High Value Request Pending.');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (bSaveWithConfirm == true && Func.Convert.iConvertToInt(txtGDReqID.Text) > 0 && hdnGWReqApprNo.Value.Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Goodwill Request Pending for Approval.');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (bSaveWithConfirm == true && Func.Convert.iConvertToInt(txtHVReqID.Text) > 0 && hdnHVReqApprNo.Value.Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('High Value Request Pending for Approval.');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }


                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();

                UpdateHdrValueFromControl(dtHdr);
                //Get Model Details     
                bDetailsRecordExist = false;
                sCreateGP = "Y";
                //Get Part Details
                if (bFillDetailsFromPartGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                //Get Labor Details
                if (bFillDetailsFromLabourGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                //Get Complaint Details     
                if (bFillDetailsFromComplaintGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                //Get Job Details     
                //if (bFillDetailsFromJobGrid() == false) return false;
                //Get Investigation Details     
                if (bFillDetailsFromInvestigationsGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                //Get Action Details
                if (bFillDetailsFromActionGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                //Get Free Service Details
                if (bFillDetailsFromFreeServiceGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

                SetGridControlPropertyTaxCalculation();
                bFillDetailsFromTaxGrid();
                //bFillDetailsFromLabourTimeGrid();

                if (bSaveAttachedDocuments(bSaveWithConfirm) == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["job_confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["job_canc_tag"] = "Y";
                }
                string sAdd = (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0) ? "Y" : "N";
                //if (ObjJobcard.bSaveJobcard(ref iDocID, Location.sDealerCode, dtHdr, dtPart, dtLabour, dtComplaint, dtJob, dtInvestigations, dtActionTaken, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString())) == true)
                //if (ObjJobcard.bSaveJobcard(ref iDocID, Location.sDealerCode, dtHdr, dtPart, dtLabour, dtComplaint, dtInvestigations, dtActionTaken, dtFreeService,dtJbGrpTaxDetails,dtJbTaxDetails,dtLabourTime, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString()), sAdd) == true)
                if (ObjJobcard.bSaveJobcard(ref iDocID, Location.sDealerCode, dtHdr, dtPart, dtLabour, dtComplaint, dtInvestigations, dtActionTaken, dtFreeService, dtJbGrpTaxDetails, dtJbTaxDetails, dtFileAttach, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString()), sAdd) == true)
                {
                    if (bSaveWithConfirm == true)
                    {
                        //Get File Attach
                        if (sCreateGP == "N")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Jobcard") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                            if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                            return true;
                        }
                        else
                        {
                            DataTable dtGPHdr = new DataTable();
                            int iGPHDRID = 0;
                            CreateGatePass(dtGPHdr);

                            if (ObjJobcard.bSaveGatepass(ref iGPHDRID, dtGPHdr, Location.sDealerCode, 0) == true)
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Jobcard") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                                if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                                return true;
                            }
                        }
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Jobcard") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(8)", true);
                        return true;
                    }
                    else
                    {

                        txtID.Text = Func.Convert.sConvertToString(iDocID);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Jobcard") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Jobcard") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    return false;
                }

                ObjJobcard = null;
                if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                return true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "Jobcard List";
                SearchGrid.AddToSearchCombo("JobCard No");
                SearchGrid.AddToSearchCombo("Job Date");
                SearchGrid.AddToSearchCombo("Job Type");
                SearchGrid.AddToSearchCombo("Chassis No");
                SearchGrid.AddToSearchCombo("Ticket No");
                SearchGrid.AddToSearchCombo("Job Status");
                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                }
                SearchGrid.sSqlFor = "JobcardList";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            GetDataAndDisplay();
        }

        private void GetDataAndDisplay()
        {
            try
            {
                clsJobcard ObjJobcard = new clsJobcard();
                DataSet ds = new DataSet();
                string strReportpath;
                int iDocID = Func.Convert.iConvertToInt(txtID.Text);
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);//set it to actual ho branch id
                if (txtUserType.Text == "6")
                {

                    iHOBranchDealerId = Location.iDealerId;
                }

                //iProformaID = 1;
                if (iDocID != 0)
                {
                    ds = ObjJobcard.GetJobcard(iDocID, "All", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    ichassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_ID"]);

                    txtReportChassisID.Text = Func.Convert.sConvertToString(ichassisID);
                    DisplayData(ds);

                }
                else
                {
                    ds = ObjJobcard.GetJobcard(iDocID, "Max", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    ichassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_ID"]);
                    txtReportChassisID.Text = Func.Convert.sConvertToString(ichassisID);

                    DisplayData(ds);
                }
                ds = null;
                ObjJobcard = null;

                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                lblServiceHistroy.Attributes.Add("onClick", "return ShowClaimHistoryDtls('" + txtReportChassisID.Text + "','" + strReportpath + "');");
                lblServiceHistroy.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 18) ? "none" : "");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void ClearFormControl()
        {
            //Display Header                                
            txtDocNo.Text = "";
            txtDocDate.Text = "";
            txtChassisNo.Text = "";
            txtVehicleNo.Text = "";

            txtPreviousDocId.Text = "";

            txtChassisID.Text = "";
            txtEngineNo.Text = "";

            drpJobType.SelectedValue = "0";
            txtKms.Text = "0";
            txtHrs.Text = "0";

            txtCustomer.Text = "";
            txtCustID.Text = "0";

            TxtModelCode.Text = "";
            txtModelName.Text = "";
            txtModelGroupID.Value = "0";

            dtpVehSaleDt.Text = "";

            DrpBay.SelectedValue = "0";

            dtpAllocateTime.Text = "";
            DrpEstimate.SelectedValue = "0";
            txtVehicleInNo.Text = "";

            dtpVehInTime.Text = "";
            dtpJobOpeningTm.Text = "";
            dtpJobCommited.Text = "";
            DtpVehicleOut.Text = "";
            DrpDelayReason.SelectedValue = "0";

            txtApPartAmt.Text = "";
            txtApLabAmt.Text = "";
            txtApLubAmt.Text = "";
            txtApMiscAmt.Text = "";


            DrpSupervisorName.SelectedValue = "0";
            //'N' as JbCd_Confirm
            txtFailureDt.Text = "";

            //Display Details
            hdnConfirm.Value = "N";
            hdnCancle.Value = "N";
            hdnJbCdConfirm.Value = "N";
            txtAggregate.Text = "N";
            txtPrevAggregate.Text = "N";
            txtWarrantyTag.Text = "N";

            //Display Part Details  
            Session["PartDetails"] = null;
            lblPartRecCnt.Text = "0";
            PartDetailsGrid.DataSource = null;
            PartDetailsGrid.DataBind();
            //Vikram K 02022018
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + PartDetailsGrid.ClientID + "',DivHeaderRow_part,DivMainContent_part, 400, 1250 , 40 ,false); </script>", false);

            // Display Labour Details        
            Session["LabourDetails"] = null;
            lblLabourRecCnt.Text = "0";
            LabourDetailsGrid.DataSource = null;
            LabourDetailsGrid.DataBind();
            //Vikram K 02022018
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key1", "<script>MakeStaticHeader('" + LabourDetailsGrid.ClientID + "',DivHeaderRow_labour,DivMainContent_labour, 400, 1250 , 40 ,false); </script>", false);

            // Display Job Details  
            Session["JobDetails"] = null;
            lblJobRecCnt.Text = "0";
            JobDetailsGrid.DataSource = null;
            JobDetailsGrid.DataBind();

            // Display Complaints Details             
            Session["ComplaintsDetails"] = null;
            lblComplaintsRecCnt.Text = "0";
            ComplaintsGrid.DataSource = null;
            ComplaintsGrid.DataBind();

            // Display Investigation Details
            Session["InvestigationDetails"] = null;
            lblInvestigationsRecCnt.Text = "0";
            InvestigationsGrid.DataSource = null;
            InvestigationsGrid.DataBind();

            // Display Action Details             
            Session["ActionDetails"] = null;
            lblActionsRecCnt.Text = "0";
            ActionsGrid.DataSource = null;
            ActionsGrid.DataBind();

            // Display Labour Timing Details        
            //Session["LabourTimeDetails"] = null;            
            //LabourTimeDetailsGrid.DataSource = null;
            //LabourTimeDetailsGrid.DataBind();

            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();
        }

        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_no"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobDate"]);

                txtPreviousDocId.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehIn_HDR_Id"]);

                txtChassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_ID"]);
                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["chassis_no"]);
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["engine_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["vehicle_no"]);
                txtAggreagateNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AggregateNo"]);

                txtCRMID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_HDR_ID"]);
                txtCRMTicketNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_No"]);
                txtCRMTicketDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_Date"]);

                drpJobType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Job_Ty_ID"]);
                txtKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["KmsJobcard"]);
                txtHrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HrsJobcard"]);

                txtTotKm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["kmsTot"]);
                txtTotHrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["hrsTot"]);

                txtSpdMtrChg.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["OdoMeterChange"]) == "Y") ? "Yes" : "No";
                txtHrsMtrChg.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HrsMeterChange"]) == "Y") ? "Yes" : "No";

                txtBfr_Last_SpdMtrChange_Kms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bfr_Last_SpdMtrChange_Kms"]);
                txthdnBfr_Last_SpdMtrChange_Kms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bfr_Last_SpdMtrChange_KmsJb"]);

                txtBfr_Last_HrsMtrChange_Hrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bfr_Last_HrsMtrChange_Hrs"]);
                txthdnBfr_Last_HrsMtrChange_Hrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Bfr_Last_HrsMtrChange_HrsJb"]);

                hdnAMCStKms.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Start_KM"]);
                hdnAMCEndKms.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_End_KM"]);

                hdnAMCStHrs.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Start_Hrs"]);
                hdnAMCEndHrs.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_End_Hrs"]);

                hdnWarrEndKms.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warranty_Kms"]);
                hdnWarrEndHrs.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warranty_hrs"]);

                hdnExtWarrStartKms.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_Start_Kms"]);
                hdnExtWarrEndKms.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_End_Kms"]);

                hdnExtWarrStartHrs.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_Start_Hrs"]);
                hdnExtWarrEndHrs.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_End_Hrs"]);

                hdnWarrChkType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MTI_Cat"]);

                hdnWarrEndDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warranty_End_Date"]);

                hdnExtWarrStDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_Warranty_start_Date"]);
                hdnExtWarrEndDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Extended_Waranty_End_Date"]);

                hdnAddWarrStDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Additional_Warranty_Start_date"]);
                hdnAddWarrEndDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Additional_Warranty_End_Date"]);

                hdnAMCStDt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_St_Date"]);

                txtLastKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LstKm"]);
                txtLastHrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LstHrs"]);

                txtCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["cust_NAME"]);
                txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);

                TxtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_code"]);
                txtModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_name"]);
                txtModelGroupID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gr_ID"]);
                txtModelID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);

                dtpVehSaleDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["chassis_sale_date"]);
                FillCombo();
                DrpBay.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["BayID"]);

                DrpBay.Attributes.Add("onChange", "SetJobCardBay(this)");

                if (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) DrpCustomer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                if (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) DrpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                if (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) DrpModelName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);

                DrpModelCode.Attributes.Add("onChange", "SetModelCombo(this,'" + DrpModelName.ID + "')");
                DrpModelName.Attributes.Add("onChange", "SetModelCombo(this,'" + DrpModelCode.ID + "')");

                dtpAllocateTime.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["bay_allot_date"]);
                if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["EstmtID"]) == 0)
                {
                    Func.Common.BindDataToCombo(DrpEstimate, clsCommon.ComboQueryType.EstNoWithDate, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), " and Chassis_ID=" + txtChassisID.Text + " and Id not in(select distinct EstmtID from TM_Jobcard_Header where Job_Ty_ID =2)");
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpEstimate, clsCommon.ComboQueryType.EstNoWithDate, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), " and Chassis_ID=" + txtChassisID.Text + " and Id=" + Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["EstmtID"]));
                }
                DrpEstimate.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EstmtID"]);
                txtVehicleInNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehicleIn"]);                

                dtpVehInTime.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehIn"]);
                dtpJobOpeningTm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobOpTime"]);
                dtpJobCommited.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehCommitTime"]);
                DtpVehicleOut.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehOut"]);
                DrpDelayReason.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["delay_Rsn_Id"]);
                hdnPDIDone.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PDIDone"]);
                
                if (txtID.Text == "" || txtID.Text == "0")
                {
                    txtApPartAmt.Text = "";
                    txtApLabAmt.Text = "";
                    txtApLubAmt.Text = "";
                    txtApMiscAmt.Text = "";
                }
                else
                {
                    txtApPartAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["ApPartAmt"]));
                    txtApLabAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["ApLabAmt"]));
                    txtApLubAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["ApLubAmt"]));
                    txtApMiscAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["ApMiscAmt"]));
                }
                drpJobType.Attributes.Add("onChange", " return OnJobTypeChange('" + Session["DepartmentID"].ToString() + "')");

                if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0)
                {
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString() + " and Empl_Type=1" + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]) : ""));
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type=1" + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]) : ""));
                    }
                }

                

                DrpSupervisorName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]);
                //'N' as JbCd_Confirm
                txtFailureDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Failure_Date"]);

                // Commented By Shyamal as on 16/02/2011
                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_canc_tag"]);
                hdnJbCdConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JbCd_Confirm"]);
                hdnRounOff.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);
                //Chassis Status Details Begin
                BtnOpen.Visible = (hdnConfirm.Value == "Y") ? true : false;

                txtCAggregate.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Aggregate"]) == "G") ? "Yes" : "No";

                txtNormalWarrantyTag.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["WarrantyTag"]) == "W") ? "Yes" : "No";
                txtExtndWarrTag.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["WarrantyTag"]) == "E") ? "Yes" : "No";
                txtAddnWarrTag.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["WarrantyTag"]) == "A") ? "Yes" : "No";
                txtKAM.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_KAM"]) == "Y") ? "Yes" : "No";
                txtUpgCamp.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UpgrdCamp"]) == "Y") ? "Yes" : "No";
                txtCAMCChk.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Chk"]) == "Y") ? "Yes" : "No";
                txtAMCDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_End_Date"]);
                //txtAMCType.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Chk"]) == "N") ? "" : (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Type"]) == "L") ? "Labor" : "Labor and Part";
                txtAMCType.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Chk"]) == "N") ? "" : (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Type"]) == "C") ? "Comfort" : (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Type"]) == "S") ? "Comfort Super" : "Comfort Premium";
                txtUndObserv.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UndObserv"]) == "Y") ? "Yes" : "No";
                txtObservEffFrom.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UndObservEffFrom"]);
                txtObservEffTo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UndObservEffTo"]);
                txtFloatPart.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Float_flag"]) == "Y") ? "Yes" : "No";
                txtIsTheft.Text = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Theft_flag"]) == "Y") ? "Yes" : "No";
                txtLstJbKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LstJbKm"]);
                txtLstJbHrs.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LstJbHrs"]);

                txtWarrantyTag.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["WarrantyTag"]);
                txtAggregate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Aggregate"]);
                txtPrevAggregate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Aggregate"]);
                hdnUpgCamp.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UpgrdCamp"]);
                hdnKAM.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_KAM"]);
                txtAMCChk.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Chk"]);
                hdnAMCType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AMC_Type"]);
                hdnUndObserv.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UndObserv"]);
                IsTheft.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Theft_flag"]);
                hdnFloatPart.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Float_flag"]);
                txtModCatIDBasic.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Mod_Cat_ID_Basic"]);
                hdnPendingVORPORec.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RecPendingVOR"]);

                hdnISDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                hdnShwReqLst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ShwReqLst"]);
                //Chassis Status Details end

                lnkRequestGD.Visible = false;
                lnkRequestHV.Visible = false;
                lnkSrvVAN.Visible = false;
                lnkRequestHV.Attributes.Add("onClick", " return GetClaimRequestDetails(this,'" + Location.iDealerId.ToString() + "','" + txtHVReqID.Text + "','H','" + Location.sDealerCode + "','" + Location.iCountryId.ToString() + "')");
                lnkRequestGD.Attributes.Add("onclick", " return GetClaimRequestDetails(this,'" + Location.iDealerId.ToString() + "','" + txtGDReqID.Text + "','G','" + Location.sDealerCode + "','" + Location.iCountryId.ToString() + "')");
                lnkSrvVAN.Attributes.Add("onclick", " return GetSrvVANDtls(this,'" + Location.iDealerId.ToString() + "')");

                txtHVReqID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HVReqID"]);
                txtGDReqID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GDReqID"]);
                hdnGPID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GPHDRID"]);
                hdnSrvVANID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SrvVANHDRID"]);
                hdnHVReqApprNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HVReqApprNo"]);
                hdnGWReqApprNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GDReqApprNo"]);                

                hdnHVReqStatus.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HVReqStatus"]);
                hdnGDReqStatus.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GDReqStatus"]);

                if (NeedHighValueApproval() == false && hdnHVReqStatus.Value.ToString() == "3") hdnHVReqApprNo.Value = "Rejected";
                if (NeedGoodwillApproval() == false && hdnGDReqStatus.Value.ToString() == "3") hdnGWReqApprNo.Value = "Rejected";

                hdnLPaidRate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LPaidRate"]);
                hdnLWarrRate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LWarrRate"]);
                
                DrpInsurnceComp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsCustID"]);
                //DrpInsurnceComp.Enabled = (Func.Convert.sConvertToString(drpJobType.SelectedValue) == "2") ? true : false;
                
                //Display Part Details  
                Session["PartDetails"] = null;
                dtPart = ds.Tables[2];
                Session["PartDetails"] = dtPart;
                lblPartRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtPart)) == 0) ? Func.Common.sRowCntOfTable(dtPart) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtPart)) - 1);
                BindDataToPartGrid(true, 0);

                // Display Labour Details        
                Session["LabourDetails"] = null;
                dtLabour = ds.Tables[3];
                Session["LabourDetails"] = dtLabour;
                lblLabourRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) == 0) ? Func.Common.sRowCntOfTable(dtLabour) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) - 1);
                BindDataToLaborGrid(true, 0);

                // Display Free Service Details        
                Session["FreeServiceDtls"] = null;
                dtFreeService = ds.Tables[8];
                Session["FreeServiceDtls"] = dtFreeService;
                lblFreeServicesRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtFreeService)) == 0) ? Func.Common.sRowCntOfTable(dtFreeService) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtFreeService)) - 1);
                BindDataToFreeServiceGrid(true, 0);


                // Display Job details Details  
                Session["JobDetails"] = null;
                dtJob = ds.Tables[4];
                Session["JobDetails"] = dtJob;
                lblJobRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtJob)) == 0) ? Func.Common.sRowCntOfTable(dtJob) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtJob))); ;
                BindDataToJobGrid();

                // Display Complaints Details     
                dtComplaint = ds.Tables[1];
                Session["ComplaintsDetails"] = dtComplaint;
                lblComplaintsRecCnt.Text = Func.Common.sRowCntOfTable(dtComplaint);
                //BindDataToComplaintGrid(true, 0);
                BindDataToComplaintGrid();

                // Display Investigation Details     
                dtInvestigations = ds.Tables[5];
                Session["InvestigationDetails"] = dtInvestigations;
                lblInvestigationsRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtInvestigations)) == 0) ? Func.Common.sRowCntOfTable(dtInvestigations) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtInvestigations)));
                BindDataToInvestigationsGrid();

                // Display Action Details     
                dtActionTaken = ds.Tables[6];
                Session["ActionDetails"] = dtActionTaken;
                lblActionsRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtActionTaken)) == 0) ? Func.Common.sRowCntOfTable(dtActionTaken) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtActionTaken)));
                BindDataToActionTakenGrid();

                dtJbGrpTaxDetails = ds.Tables[9];
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;

                dtJbTaxDetails = ds.Tables[10];
                Session["JbTaxDetails"] = dtJbTaxDetails;

                BindDataToGrid();

                //dtLabourTime = ds.Tables[11];
                //Session["LabourTimeDetails"] = dtLabourTime;
                //BindDataToLaborTimeGrid(true, 0);
                if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 7 && Func.Convert.iConvertToInt(drpJobType.SelectedValue) == 7)
                {
                    PFileAttchDetails.Visible = true;
                    dtFileAttach = ds.Tables[12];
                    lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                    ShowAttachedFiles();
                }
                else
                    PFileAttchDetails.Visible = false;

                trNewAttachment.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
                trNewAttachment1.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 7) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true); //print option enable for save & confirm
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);

                lblSelectModel.Visible = (Session["DepartmentID"].ToString() == "7" && (txtID.Text == "" || txtID.Text == "0")) ? true : false;

                btnPO.Text = (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 2) ? "Accidental PO" : "VOR PO";

                if (txtDealerCode.Text.Trim().StartsWith("R"))
                {
                    btnPO.Style.Add("display", ((Session["DepartmentID"].ToString() == "6" && hdnConfirm.Value == "N" && Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) != 2 && Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) != 18) && (txtDealerCode.Text.Trim() == "R503157" || txtDealerCode.Text.Trim() == "R503159")) ? "" : "none");
                }
                else
                {
                    btnPO.Style.Add("display", (Session["DepartmentID"].ToString() == "6" && hdnConfirm.Value == "N" && Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) != 2 && Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) != 18) ? "" : "none");
                }                

                if (NeedGoodwillApproval() == true && Session["DepartmentID"].ToString() == "7") lnkRequestGD.Visible = true;
                if (NeedHighValueApproval() == true && Session["DepartmentID"].ToString() == "7") lnkRequestHV.Visible = true;

                if (Session["DepartmentID"].ToString() == "7" && Func.Convert.iConvertToInt(txtID.Text.ToString()) != 0//)  lnkSrvVAN.Visible = true;
                    && (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 5 ||
                    Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 10 ||
                    Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 12 ||
                    Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 15)) lnkSrvVAN.Visible = true;

                //Fill Jobcard type based on previous jobcard type during jobcard edit mode.
                //For ADD mode it should not work.
                // 
                if ((txtID.Text != "" || txtID.Text != "0") && (hdnConfirm.Value == "N"))
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                // If Record is Confirm or cancel then it is not editable   
                btnJobSave.Visible = false;
                btnJobConfirm.Visible = false;
                lnkGatePass.Visible = false;
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    if (hdnJbCdConfirm.Value == "N" && Session["DepartmentID"].ToString() == "7") btnJobSave.Visible = true;
                    if (hdnJbCdConfirm.Value == "N" && Session["DepartmentID"].ToString() == "7") btnJobConfirm.Visible = true;
                    if (Func.Convert.iConvertToInt(hdnGPID.Value.Trim()) > 0)
                    {
                        lnkGatePass.Visible = true; // Add condition after create Gatepass
                        lnkGatePass.Attributes.Add("onclick", " return GetGatePassDtls(this,'" + Location.iDealerId.ToString() + "','" + hdnJobInvID.Value.ToString().Trim() + "','" + hdnSaleInvID.Value.Trim() + "')");
                    }
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);

                }
                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);

                }
                if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 6) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                txtCustomer.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                TxtModelCode.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                txtModelName.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                txtVehicleNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                lblVehicleNo.Text = (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "Aggregate No:" : "Veh Reg No.:";

                DrpCustomer.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
                DrpModelCode.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
                DrpModelName.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
                txtAggreagateNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
                lblAggrMndt.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");

                txtChassisNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                txtEngineNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                lblChassisNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                lblEngineNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
                lblchassisMandt.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");

                if (hdnShwReqLst.Value == "Y")
                {
                    Func.Common.BindDataToCombo(DrpRequisitionLst, clsCommon.ComboQueryType.JobcardRequisition, Func.Convert.iConvertToInt(txtID.Text));
                }
                string sReportpath = "";
                sReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                lblReqlabel.Style.Add("display", (hdnShwReqLst.Value == "N") ? "none" : "");
                lblReqPrint.Style.Add("display", (hdnShwReqLst.Value == "N") ? "none" : "");
                DrpRequisitionLst.Style.Add("display", (hdnShwReqLst.Value == "N") ? "none" : "");
                lblReqPrint.Attributes.Add("onClick", "return ShowRequisitionDtls('" + txtID.Text + "','" + sReportpath + "');");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {
            dtpAllocateTime.Enabled = bEnable;
            txtFailureDt.Enabled = bEnable;
            ComplaintsGrid.Enabled = bEnable;
        }

        protected void btnJobConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                clsJobcard ObjJobcard = new clsJobcard();
                string sJbCdConfirm = "Y";
                if (bFillDetailsFromJobGrid(sJbCdConfirm) == true)
                {
                    if (ObjJobcard.bSaveJobcodeDetails(Func.Convert.iConvertToInt(txtID.Text), dtJob, sJbCdConfirm) == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Confirmed 2nd Stage Jobcard.');</script>");
                    }
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnPO_Click(object sender, EventArgs e)
        {
            objPO = new clsSparePO();
            DataSet ds = new DataSet();

            int iPOID = 0;
            ds = objPO.GetPO(Func.Convert.iConvertToInt(txtID.Text.Trim()), (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 2) ? "ACCIDENT" : "VOR", Location.iDealerId, "N", 0);

            //if (ds.Tables[0].Rows.Count == 0)
            //{                
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('No Details For PO Creation.');</script>");    
            //}
            //else 
            if (ds.Tables[1].Rows.Count == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('No Details For PO Creation.');</script>");
            }
            else
            {
                DataTable dtPOHdr = new DataTable();
                DataTable dtPODtl = new DataTable();
                POUpdateHdrValueFromControl(dtPOHdr);
                dtPODtl = ds.Tables[1];

                if (objPO.bSaveRecordWithPart(Location.sDealerCode, iDealerId, dtPOHdr, dtPODtl, ref iPOID, 0) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PO Created Successfully.');</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PO Not Created, Please Contact to Administrator.');</script>");
                }

            }



        }

        private void POUpdateHdrValueFromControl(DataTable dtPOHdr)
        {
            try
            {
                objPO = new clsSparePO();

                string cntPartID = "";
                int TotCntQty = 0;
                DataRow dr;
                //Get Header InFormation        
                dtPOHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_No", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Date", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Po_Type_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Confirm", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Cancel", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("PO_Total", typeof(double)));
                dtPOHdr.Columns.Add(new DataColumn("PO_TotalQty", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_TotalItems", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("PO_CreatedBy", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("Chassis_No", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Supplier_ID", typeof(int)));
                dtPOHdr.Columns.Add(new DataColumn("Is_Distributor", typeof(string)));
                dtPOHdr.Columns.Add(new DataColumn("JobCard_HDR_ID", typeof(int)));

                dr = dtPOHdr.NewRow();
                dr["ID"] = "0";
                dr["PO_No"] = Func.Convert.sConvertToString(objPO.GeneratePO(Location.sDealerCode, iDealerId, (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 2) ? 8 : 3, "N"));//VOR 3
                dr["PO_Date"] = Func.Common.sGetCurrentDate(1, false); ;

                //Changed By Vikram Date 17.06.2016
                //dr["Dealer_ID"] = (ExportLocation.bDistributor == "Y") ? Func.Convert.iConvertToInt(ExportLocation.iDealerId) : 0;
                dr["Dealer_ID"] = Location.iDealerId;

                dr["Po_Type_ID"] = (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 2) ? 8 : 3;
                dr["PO_Confirm"] = "N";
                dr["PO_Cancel"] = "N";
                dr["PO_Total"] = Func.Convert.dConvertToDouble(0);
                dr["PO_TotalQty"] = Func.Convert.iConvertToInt(0);

                for (int iRCnt = 0; iRCnt < PartDetailsGrid.Rows.Count; iRCnt++)
                {
                    if (iRCnt != Func.Convert.iConvertToInt(PartDetailsGrid.Rows.Count))
                    {
                        TextBox txtPartNo11 = (TextBox)PartDetailsGrid.Rows[iRCnt].FindControl("txtPartNo");
                        cntPartID = txtPartNo11.Text;
                        if (cntPartID != "")
                        {
                            TotCntQty = TotCntQty + 1;

                        }
                    }
                }
                dr["PO_TotalItems"] = TotCntQty;
                dr["PO_CreatedBy"] = "Jobcard";
                dr["Chassis_No"] = txtChassisNo.Text;
                dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);
                // dr["Supplier_ID"] = (ExportLocation.bDistributor == "N") ? Func.Convert.iConvertToInt(ExportLocation.iDealerId) : 0;
                dr["Supplier_ID"] = Func.Convert.iConvertToInt(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetManufSuppID, Location.iDealerId, " and HOBr_ID=" + Session["HOBR_ID"].ToString())); //MTI 18 --
                dr["Is_Distributor"] = "N";
                dr["JobCard_HDR_ID"] = Func.Convert.iConvertToInt(txtID.Text.Trim().ToString());

                dtPOHdr.Rows.Add(dr);
                dtPOHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private bool NeedGoodwillApproval()
        {
            try
            {
                bool bNeedGoodwillApproval = false;
                if (PartDetailsGrid.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                    {
                        if (Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtGoodWlQty") as TextBox).Text) > 0)
                        {
                            bNeedGoodwillApproval = true;
                        }
                    }

                }
                if (bNeedGoodwillApproval == false)
                {
                    if (LabourDetailsGrid.Rows.Count > 0)
                    {
                        for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                        {
                            DropDownList drpLabWarr = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLabWarr");
                            TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                            if (drpLabWarr.SelectedValue.Trim() == "G" && txtLabourID.Text.Trim() != "")
                            {
                                bNeedGoodwillApproval = true;
                            }
                        }
                    }
                }
                return bNeedGoodwillApproval;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }


        }

        private bool NeedHighValueApproval()
        {
            try
            {
                bool bNeedHighValueApproval = false;
                double dWarrQty = 0, dNDPRate = 0, dPTotal = 0, dTotal = 0, dAMCQty = 0, dAMCRate = 0;

                dTotal = 0;

                if (PartDetailsGrid.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                    {
                        dWarrQty = 0; dNDPRate = 0; dPTotal = 0; dAMCQty = 0; dAMCRate = 0;

                        TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");
                        if (txtPartID.Text != "")
                        {
                            TextBox txtWarrQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtWarrQty");
                            TextBox txtPDIQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPDIQty");
                            TextBox txtAMCQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCQty");
                            TextBox txtCampaignQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtCampaignQty");
                            TextBox txttransitQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txttransitQty");
                            TextBox txtEnRouteTechQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnRouteTechQty");
                            TextBox txtEnrouteNonTechQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtEnrouteNonTechQty");
                            TextBox txtSpWarQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtSpWarQty");
                            TextBox txtPrePDIQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPrePDIQty");
                            TextBox txtAggregateQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtAggregateQty");

                            dWarrQty = Func.Convert.dConvertToDouble(txtWarrQty.Text) + Func.Convert.dConvertToDouble(txtPDIQty.Text) +
                                       Func.Convert.dConvertToDouble(txtCampaignQty.Text) + Func.Convert.dConvertToDouble(txttransitQty.Text) + Func.Convert.dConvertToDouble(txtEnRouteTechQty.Text) +
                                       Func.Convert.dConvertToDouble(txtEnrouteNonTechQty.Text) + Func.Convert.dConvertToDouble(txtSpWarQty.Text) + Func.Convert.dConvertToDouble(txtPrePDIQty.Text) +
                                       Func.Convert.dConvertToDouble(txtAggregateQty.Text); //Func.Convert.dConvertToDouble(txtAMCQty.Text) +

                            TextBox txtNDPRate = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtNDPRate");
                            dNDPRate = Func.Convert.dConvertToDouble(txtNDPRate.Text);

                            //dPTotal = dNDPRate * dWarrQty;

                            dAMCQty = Func.Convert.dConvertToDouble(txtAMCQty.Text);

                            TextBox txtAMCRate = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtAMCRate");
                            dAMCRate = Func.Convert.dConvertToDouble(txtAMCRate.Text);

                            dPTotal = (dNDPRate * dWarrQty) + (dAMCQty * dAMCRate);

                            dTotal = dTotal + dPTotal;
                        }
                    }
                }

                if (LabourDetailsGrid.Rows.Count > 0)
                {
                    for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                    {
                        DropDownList drpLabWarr = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLabWarr");
                        TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                        TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");
                        if (txtLabourCode.Text != "")
                        {
                            string sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                                                      
                            double dLnTotal = 0.00;
                            
                            //Lab Tag
                            TextBox txtLabCD = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD");


                            TextBox txtTotal = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");

                            //Sublet Amt    out_Lab_amt
                            TextBox txtSubletAmt = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletAmt");


                            //dTotal = dTotal + ((txtLabCD.Text.Trim() == "D") ? 0 : ((sFirstFiveDigit == "MTIOU") ? Func.Convert.dConvertToDouble(txtSubletAmt.Text) : Func.Convert.dConvertToDouble(txtTotal.Text)));
                            //--------------------- instead of total add total + tax value

                            dLnTotal = ((txtLabCD.Text.Trim() == "D") ? 0 : ((sFirstFiveDigit == "MTIOU") ? Func.Convert.dConvertToDouble(txtSubletAmt.Text) : Func.Convert.dConvertToDouble(txtTotal.Text)));
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dLnTotal);

                            //string sTax1ApplOn = "";
                            //string sTax2ApplOn = "";
                            //double dLnDiscAmt = 0.00;
                            //double dLnTaxAppAmt = 0.00;
                            //double dLnMTaxPer = 0.00;
                            //double dLnMTaxAmt = 0.00;
                            //double dLnTax1Per = 0.00;
                            //double dLnTax1Amt = 0.00;
                            //double dLnTax2Per = 0.00;
                            //double dLnTax2Amt = 0.00;
                            //dLnDiscAmt = 0;

                            ////Amount whiich is applicable for tax
                            //dLnTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dLnTotal) - Func.Convert.dConvertToDouble(dLnDiscAmt), 2);

                            ////Main tax calculation
                            //dLnMTaxPer = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetTaxPercentage, 0, " and ID=" + txtLTax.Text.Trim()));
                            //dLnMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTaxAppAmt) * Func.Convert.dConvertToDouble(dLnMTaxPer / 100)), 2);

                            //dLnTax1Per = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetTaxPercentage, 0, " and ID=" + txtLTax1.Text.Trim()));
                            //sTax1ApplOn = Func.Convert.sConvertToString(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetTaxApplicableOn, 0, " and ID=" + txtLTax1.Text.Trim()));
                            ////Sujata 23092014 Begin
                            ////dLnTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax1Per / 100));
                            //if (sTax1ApplOn == "1")
                            //{
                            //    dLnTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTotal - dLnDiscAmt) * Func.Convert.dConvertToDouble(dLnTax1Per / 100)), 2);
                            //}
                            //else if (sTax1ApplOn == "3")
                            //{
                            //    dLnTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTotal - dLnDiscAmt + dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax1Per / 100)), 2);
                            //}
                            //else
                            //{
                            //    dLnTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax1Per / 100)), 2);
                            //}
                            ////Sujata 23092014 End

                            //dLnTax2Per = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetTaxPercentage, 0, " and ID=" + txtLTax2.Text.Trim()));
                            //sTax2ApplOn = Func.Convert.sConvertToString(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetTaxApplicableOn, 0, " and ID=" + txtLTax2.Text.Trim()));

                            ////Sujata 23092014 Begin
                            ////dLnTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax2Per / 100));
                            //if (sTax2ApplOn == "1")
                            //{
                            //    dLnTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTotal - dLnDiscAmt) * Func.Convert.dConvertToDouble(dLnTax2Per / 100)), 2);
                            //}
                            //else if (sTax2ApplOn == "3")
                            //{
                            //    dLnTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTotal - dLnDiscAmt + dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax2Per / 100)), 2);
                            //}
                            //else
                            //{
                            //    dLnTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnMTaxAmt) * Func.Convert.dConvertToDouble(dLnTax2Per / 100)), 2);
                            //}
                            ////Sujata 23092014 End
                            //dLnTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dLnTaxAppAmt) + Func.Convert.dConvertToDouble(dLnMTaxAmt) + Func.Convert.dConvertToDouble(dLnTax1Amt) + Func.Convert.dConvertToDouble(dLnTax2Amt)), 2);
                            //dTotal = dTotal + Func.Convert.dConvertToDouble(dLnTotal);
                            //---------------------                            
                        }
                    }
                }

                if (Func.Convert.dConvertToDouble(dTotal) >= Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, "")))
                {
                    bNeedHighValueApproval = true;
                }

                return bNeedHighValueApproval;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }


        }

        protected void lblSelectModel_Click(object sender, EventArgs e)
        {
            DataRow[] dtSrchgrid;
            DataTable dtSearch;
            dtSearch = (DataTable)Session["ChassisDetails"];
            //dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString() + " and VehInID=" + txtPreviousDocId.Text.ToString());
            if (dtSearch != null)
            {
                dtSrchgrid = dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString() + " and VehInID=" + txtPreviousDocId.Text.ToString() + " and CRM_HDR_ID= " + txtCRMID.Text.ToString(), "", DataViewRowState.CurrentRows);

                if (dtSrchgrid.Length != 0)
                {
                    for (int i = 0; i < dtSrchgrid.Length; i++)
                    {
                        //txtChassisID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_ID"]);
                        //txtChassisNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_no"]);

                        txtChassisID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_ID"]);
                        txtChassisNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_no"]);
                        txtVehicleNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Vehicle_No"]);
                        txtCustomer.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Customer_name"]);

                        txtVehicleInNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["VehInNo"]);
                        dtpVehInTime.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["VehInTime"]);

                        txtCRMID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["CRM_HDR_ID"]);
                        txtCRMTicketNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Ticket_No"]);
                        txtCRMTicketDate.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Ticket_Date"]);

                        txtEngineNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Engine_no"]);
                        txtCustEdit.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["CustEdit"]);
                        txtCustID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["CustID"]);
                        hdnCustTaxTag.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["CustTaxTag"]);

                        TxtModelCode.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_code"]);
                        txtModelName.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_name"]);

                        txtAggregate.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Aggregate"]);
                        txtPrevAggregate.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Aggregate"]);
                        txtCAggregate.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["Aggregate"]) == "G") ? "Yes" : "No";
                        txtWarrantyTag.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["WarrantyTag"]);

                        txtNormalWarrantyTag.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["WarrantyTag"]) == "W") ? "Yes" : "No";
                        txtExtndWarrTag.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["WarrantyTag"]) == "E") ? "Yes" : "No";
                        txtAddnWarrTag.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["WarrantyTag"]) == "A") ? "Yes" : "No";

                        txtAMCChk.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Chk"]);
                        txtCAMCChk.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Chk"]) == "Y") ? "Yes" : "No";
                        hdnAMCType.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Type"]);

                        txtAMCType.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Chk"]) == "N") ? "" : (Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Type"]) == "C") ? "Comfort" : (Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Type"]) == "S") ? "Comfort Super" : "Comfort Premium";

                        txtModelGroupID.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_Gr_ID"]);

                        txtPreviousDocId.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["VehInID"]);

                        txtLastKms.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["LstKm"]);
                        txtLastHrs.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["LstHrs"]);

                        hdnAMCStKms.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Start_KM"]);
                        hdnAMCEndKms.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_End_KM"]);

                        hdnAMCStHrs.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_Start_Hrs"]);
                        hdnAMCEndHrs.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_End_Hrs"]);

                        hdnWarrEndKms.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Warranty_Kms"]);
                        hdnWarrEndHrs.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Warranty_hrs"]);

                        hdnExtWarrStartKms.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_Start_Kms"]);
                        hdnExtWarrEndKms.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_End_Kms"]);

                        hdnExtWarrStartHrs.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_Start_Hrs"]);
                        hdnExtWarrEndHrs.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_End_Hrs"]);

                        hdnWarrChkType.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["MTI_Cat"]);

                        hdnWarrEndDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Warranty_End_Date"]);
                        hdnExtWarrStDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_Warranty_start_Date"]);
                        hdnExtWarrEndDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Extended_Waranty_End_Date"]);
                        hdnAMCStDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_St_Date"]);

                        hdnAddWarrStDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Additional_Warranty_Start_date"]);
                        hdnAddWarrEndDt.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Additional_Warranty_End_Date"]);

                        txtBfr_Last_SpdMtrChange_Kms.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Bfr_Last_SpdMtrChange_Kms"]);
                        txtBfr_Last_HrsMtrChange_Hrs.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Bfr_Last_HrsMtrChange_Hrs"]);

                        txtKAM.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["In_KAM"]) == "Y") ? "Yes" : "No";
                        txtUpgCamp.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["UpgrdCamp"]) == "Y") ? "Yes" : "No";
                        txtAMCDate.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["AMC_End_Date"]);
                        txtUndObserv.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["UndObserv"]) == "Y") ? "Yes" : "No";
                        txtObservEffFrom.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["UndObservEffFrom"]);
                        txtObservEffTo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["UndObservEffTo"]);

                        txtFloatPart.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["Float_flag"]) == "Y") ? "Yes" : "No";
                        txtIsTheft.Text = (Func.Convert.sConvertToString(dtSrchgrid[i]["Theft_flag"]) == "Y") ? "Yes" : "No";
                        hdnFloatPart.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Float_flag"]);
                        IsTheft.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Theft_flag"]);

                        txtLstJbKms.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["LstJbKm"]);
                        txtLstJbHrs.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["LstJbHrs"]);

                        txtSpdMtrChg.Text = "No";
                        txtHrsMtrChg.Text = "No";

                        txtTotKm.Text = txtLastKms.Text;
                        txtTotHrs.Text = txtLastHrs.Text;

                        txtKms.Text = "0";
                        txtHrs.Text = "0";

                        txtModCatIDBasic.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Mod_Cat_ID_Basic"]);
                        hdnPendingVORPORec.Value = "N";
                        hdnPDIDone.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["PDIDone"]);

                        BindDataToPartGrid(true, 0);
                        BindDataToLaborGrid(true, 0);

                        Func.Common.BindDataToCombo(DrpEstimate, clsCommon.ComboQueryType.EstNoWithDate, Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), " and Chassis_ID=" + txtChassisID.Text + " and Id not in(select distinct EstmtID from TM_Jobcard_Header where Job_Ty_ID =2)");

                        txtReportChassisID.Text = txtChassisID.Text.Trim();
                        string strReportpath = "";
                        strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowClaimHistoryDtls('" + txtReportChassisID.Text + "','" + strReportpath + "');");
                        lblServiceHistroy.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.SelectedValue.Trim().ToString()) == 18) ? "none" : "");
                    }
                }
            }
        }

        private void CreateGatePass(DataTable dtGPHdr)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                DataRow dr;

                dtGPHdr.Columns.Add(new DataColumn("GPHDRID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("Gp_No", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Gp_date", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefJobcardID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefJbInvID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefSlInvID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("GPtype", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Narr", typeof(string)));


                dr = dtGPHdr.NewRow();
                dr["GPHDRID"] = 0;
                dr["Gp_No"] = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GPJ", iDealerId);
                dr["Gp_date"] = Func.Common.sGetCurrentDateTime(1, true);
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(Location.iDealerId);
                dr["DlrBranchID"] = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                dr["RefJobcardID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["RefJbInvID"] = 0;
                dr["RefSlInvID"] = 0;
                dr["GPtype"] = "J";
                dr["Narr"] = "";

                dtGPHdr.Rows.Add(dr);
                dtGPHdr.AcceptChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }


        protected void lnkSrvVAN_Click(object sender, EventArgs e)
        {
            try
            {
                dtLabour = (DataTable)Session["LabourDetails"];
                lblLabourRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) == 0) ? Func.Common.sRowCntOfTable(dtLabour) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) - 1);
                BindDataToLaborGrid(true, 0);

                dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                CreateNewRowToTaxGroupDetailsTable();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbTaxDetails"] = dtJbTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void GrdPartGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private bool bFillDetailsFromTaxGrid()
        {

            dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];
            int iCouponID = 0;
            int iSrvjobID = 0;
            bool bValidate = true;

            //For Tax Details
            dtJbGrpTaxDetails = (DataTable)(Session["JbGrpTaxDetails"]);
            dtJbTaxDetails = (DataTable)(Session["JbTaxDetails"]);

            //if (bSaveTmTxDtls == true)
            //{                
            for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
            {
                //Group Code
                TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                dtJbGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                //Group Name
                TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                //Get Net Amount
                TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                //Get Discount Perc
                TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                //Get Discount Amount
                TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                // Get Tax
                DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                //Get Tax Percentage                
                DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                //Get Tax Amount
                TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                // Get Tax1
                DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                //Get Tax1 Percentage                
                DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                //Get Tax1 Amount
                TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                // Get Tax2
                DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                //Get Tax2 Percentage                
                DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                //Get Tax2 Amount
                TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                // Get Total
                TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
            }

            for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
            {
                //Doc ID
                TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                dtJbTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                //Get Net Amount
                TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                dtJbTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                //Get Discount amt
                TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                dtJbTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                //Get Amt Before Tax (with Discount)
                TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                dtJbTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                // Get Tax 
                TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                dtJbTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                //Get Tax         
                TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                dtJbTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                //Get Tax1 Amount
                TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                dtJbTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                // Get Tax2 Amount
                TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                dtJbTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                //Get PF Per                 
                TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                dtJbTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                //Get PF Amount
                TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                dtJbTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                // Get Other Per
                TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                dtJbTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                //Get Other Amount
                TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                dtJbTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                //Get grand Total Amount
                TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                dtJbTaxDetails.Rows[iRowCnt]["Jobcard_tot"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
            }


            return bValidate;
        }

        private void CreateNewRowToTaxGroupDetailsTable()
        {
            try
            {
                string sGrCode = "";
                int iPartTaxID = 0;
                int iPartTaxID1 = 0;
                int iPartTaxID2 = 0;
                int iLTaxID = 0;
                int iLTaxID1 = 0;
                int iLTaxID2 = 0;

                dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];

                Boolean bDtSelPartRow = false;
                dtJbGrpTaxDetails.Clear();
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    TextBox txtPartGroupCode = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode");
                    sGrCode = txtPartGroupCode.Text.Trim();

                    CheckBox ChkForDelete = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0)
                    {
                        TextBox txtPTax = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax");
                        iPartTaxID = Func.Convert.iConvertToInt(txtPTax.Text);

                        TextBox txtPTax1 = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(txtPTax1.Text);

                        TextBox txtPTax2 = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(txtPTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Spares" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iPartTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iPartTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iPartTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }

                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iLTaxID = 0;
                    iLTaxID1 = 0;
                    iLTaxID2 = 0;
                    bDtSelPartRow = false;

                    //TextBox txtLGroupCode = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");
                    //sGrCode = txtLGroupCode.Text.Trim();                    
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                    sGrCode = "L";

                    CheckBox ChkForDelete = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0 && (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != ""))
                    {
                        TextBox txtLTax = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax");
                        iLTaxID = Func.Convert.iConvertToInt(txtLTax.Text);

                        TextBox txtLTax1 = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1");
                        iLTaxID1 = Func.Convert.iConvertToInt(txtLTax1.Text);

                        TextBox txtLTax2 = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2");
                        iLTaxID2 = Func.Convert.iConvertToInt(txtLTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iLTaxID) &&
                            iLTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iLTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Spares" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iLTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iLTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iLTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void BindDataToGrid()
        {
            try
            {
                GrdPartGroup.DataSource = dtJbGrpTaxDetails;
                GrdPartGroup.DataBind();

                GrdDocTaxDet.DataSource = dtJbTaxDetails;
                GrdDocTaxDet.DataBind();

                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Tax
                    //Dislay hide and label change code related to GST Appl and non appl.
                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    // fill tax change  GST Appl and non appl.
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N")) //For service we need to add hdnISDocGST.Value == "N"
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    // ISGST related 
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    // Service tax related code add into O case and ISGST related 

                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));

                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));


                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "   and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
                    txtTax2Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer2.SelectedValue) == 0 ? "0" : drpTaxPer2.SelectedItem.Text);

                    drpTax.Enabled = false;
                    drpTax1.Enabled = false;
                    drpTax2.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlPropertyTaxCalculation()
        {
            try
            {
                double dGrpTotal = 0;
                double dGrpDiscPer = 0;
                double dGrpDiscAmt = 0;
                double dGrpTaxAppAmt = 0;

                double dGrpMTaxPer = 0;
                double dGrpMTaxAmt = 0;

                double dGrpTax1Per = 0;
                double dGrpTax1Amt = 0;

                double dGrpTax2Per = 0;
                double dGrpTax2Amt = 0;

                double dDocTotalAmtFrPFOther = 0;
                double dDocDiscAmt = 0;
                double dDocLSTAmt = 0;
                double dDocCSTAmt = 0;
                double dDocTax1Amt = 0;
                double dDocTax2Amt = 0;
                string sGrpMTaxTag = "";

                double TotalOA = 0;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0.00";
                }

                for (int i = 0; i < PartDetailsGrid.Rows.Count; i++)
                {
                    TextBox TxtExclTotal = (TextBox)PartDetailsGrid.Rows[i].FindControl("txtTotal");
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[i].FindControl("ChkForDelete");
                    TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax2") as TextBox);


                    if (txtGrNo.Text.Trim() != "" && Chk.Checked == false) TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtPTax.Text && Chk.Checked == false)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                        }
                    }
                }

                for (int i = 0; i < LabourDetailsGrid.Rows.Count; i++)
                {
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourID");

                    if (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != "")
                    {
                        string sFirstFiveDigit = "";
                        TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourCode");
                        sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);

                        TextBox txtLabCD = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabCD");
                        TextBox txtLabWarr = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabWarr");
                        DropDownList drpFOC = (DropDownList)LabourDetailsGrid.Rows[i].FindControl("drpFOC");
                        DropDownList drpLabWarr = (DropDownList)LabourDetailsGrid.Rows[i].FindControl("drpLabWarr");

                        TextBox txtSubletAmt = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtSubletAmt");
                        TextBox TxtExclTotal = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtTotal");

                        TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax") as TextBox);
                        TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax1") as TextBox);
                        TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax2") as TextBox);

                        double LTotal = 0;
                        if (txtLabCD.Text.Trim() == "D" && drpFOC.SelectedValue == "N" && drpLabWarr.SelectedValue == "N" && sFirstFiveDigit != "MTIOU")
                        {
                            LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());
                        }

                        if (txtLabCD.Text.Trim() == "D" && drpFOC.SelectedValue == "N" && drpLabWarr.SelectedValue == "N" && sFirstFiveDigit == "MTIOU")
                        {
                            LTotal = Func.Convert.dConvertToDouble(txtSubletAmt.Text.Trim());
                        }

                        if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                            DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                            TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)
                            {
                                txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal)).ToString("0.00"));
                            }
                        }
                    }
                }


                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    //group Percentage
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    //group Discount Amount
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    //Doc Discount Amount
                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Sujata 23092014 Begin
                    //dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    if (sTax1ApplOn == "1")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    //Sujata 23092014 End

                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

                    //Sujata 23092014 Begin
                    //dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    //Sujata 23092014 End

                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 2);
                    //dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 0);
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {

                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnISDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnISDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnISDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   

                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    double dDocPFPer = 0;
                    double dDocPFAmt = 0;
                    double dDocOtherPer = 0;
                    double dDocOtherAmt = 0;
                    double dDocGrandAmt = 0;

                    dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                    dDocPFAmt = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100), 2);
                    txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);

                    dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                    dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100)), 2);
                    txtOtherAmt.Text = Func.Convert.sConvertToString(dDocOtherAmt.ToString("0.00"));
                    if (hdnRounOff.Value == "Y")
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), 0);
                    }
                    else
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), 2);
                    }
                    //dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), 0);

                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void DrpEstimate_SelectedIndexChanged(object sender, EventArgs e)
        {

            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                int EstmId = 0;
                EstmId = Func.Convert.iConvertToInt(DrpEstimate.SelectedValue);
                if (EstmId > 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_JbEstAppAmt", EstmId);
                    if (ds != null) // if no Data Exist
                    {
                        txtApPartAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppPartAmt"]);
                        txtApLubAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppLubAmt"]);
                        txtApLabAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppLabAmt"]);
                        txtApMiscAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppMiscAmt"]);
                        if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["InsCustID"]) > 0 && Func.Convert.iConvertToInt(DrpInsurnceComp.SelectedValue) == 0)
                            DrpInsurnceComp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsCustID"]);
                    }
                }
                objDB = null;
                ds = null;
                objDB = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private int i = 1;
        protected void PartDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < PartDetailsGrid.Columns.Count; i++)
                {
                    e.Row.Cells[i].ToolTip = PartDetailsGrid.Columns[i].HeaderText;
                }

                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtPartID != "0")
                {
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }
        private int j = 1;
        protected void LabourDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < LabourDetailsGrid.Columns.Count; i++)
                {
                    e.Row.Cells[i].ToolTip = LabourDetailsGrid.Columns[i].HeaderText;
                }
                string txtLabourID = (e.Row.FindControl("txtLabourID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtLabourID != "0")
                {
                    lblNo.Text = j.ToString();
                    j++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    j = 1;
                }
            }
        }

        protected void BtnOpen_Click(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                if (iDocID == 0) iDocID = Func.Convert.iConvertToInt(txtID.Text);
                if (iDocID > 0)
                {
                    string status;
                    status = "";
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_JbOpen", iDocID);
                    if (ds != null) // if no Data Exist
                    {
                        status = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JStatus"]);
                        if (status.Trim() != "") Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + status + "');</script>");
                        PSelectionGrid.Style.Add("display", "");
                    }
                }
                objDB = null;
                ds = null;
                objDB = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        //protected void drpJobType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //     if (iDocID == 0) iDocID = Func.Convert.iConvertToInt(txtID.Text);
        //     txtAggregate.Text = (iDocID == 0 && Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "G" : txtPrevAggregate.Text.ToString();
        //     txtCAggregate.Text = (txtAggregate.Text == "G") ? "Yes" : "No";

        //     txtCustomer.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
        //     TxtModelCode.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
        //     txtModelName.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");
        //     txtVehicleNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "none" : "");


        //     DrpCustomer.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
        //     DrpModelCode.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
        //     DrpModelName.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");
        //     txtAggreagateNo.Style.Add("display", (Func.Convert.iConvertToInt(drpJobType.Text.ToString()) == 18) ? "" : "none");  

        //}      
    }
}