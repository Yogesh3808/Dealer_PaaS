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
using System.Web.Services;


namespace MANART.Forms.Service
{
    public partial class frmEstimate : System.Web.UI.Page
    {
        //private DataTable dtDetails = new DataTable();        
        private DataTable dtComplaint = new DataTable();
        private DataTable dtPart = new DataTable();
        private DataTable dtLabour = new DataTable();
        private DataTable dtEst = new DataTable();
        private DataTable dtJbGrpTaxDetails = new DataTable();
        private DataTable dtJbTaxDetails = new DataTable();
        private DataTable dtInvJobDescDet = new DataTable();
        clsSparePO objPO = null;

        private int iDocID = 0;
        int iDealerId;
        string sDealerCode;
        int iHOBranchDealerId;
        private bool bDetailsRecordExist = false;
        string sNew = "N";
        protected void Page_Init(object sender, EventArgs e)
        {
            ToolbarC.bUseImgOrButton = true;
            ToolbarC.iFormIdToOpenForm = 11;
            ToolbarC.iValidationIdForConfirm = 68;
            ToolbarC.iValidationIdForSave = 68;

            Location.bUseSpareDealerCode = true;
            Location.SetControlValue();

            //MDUser Change
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                Session["HOBR_ID"] = Func.Convert.sConvertToString(Session["DealerID"]);
            }
            //MDUser Change

            if (!IsPostBack)
            {
                Session["PartDetails"] = null;
                DisplayPreviousRecord();
            }
            SearchGrid.sGridPanelTitle = "Estimate List";
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
            base.OnPreRender(e);
            //MDUser Change
            if (txtUserType.Text == "6")
            {
                FillSelectionGrid();
            }//MDUser Change
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
                if (objCommon.sUserRole == "15" || objCommon.sUserRole == "19")
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
        // set Document text 
        private void SetDocumentDetails()
        {
            lblTitle.Text = "Estimate";
            lblDocNo.Text = "Est No.:";
            lblDocDate.Text = "Est Date:";
            lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Session["HOBR_ID"].ToString() + "','" + Session["DepartmentID"].ToString() + "')");
        }

        // FillCombo
        private void FillCombo()
        {
            if (Location.iDealerId != 0)
            {
                //Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type=1");
                if (txtUserType.Text == "6")
                {
                    Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId + " and Empl_Type=1");
                }
                else
                {
                    Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type=1");
                }
                Func.Common.BindDataToCombo(DrpInsurnceComp, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "') and CM.Cust_Type in (Select Id from M_CustType where DMS_type_flag='IC' and Cust_Sup='C')");
            }
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
                clsEstimate ObjEstimate = new clsEstimate();
                DataSet ds = new DataSet();
                //Please remove this Temporary comment

                iDealerId = Location.iDealerId;
                sDealerCode = Location.sDealerCode;
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                //iDealerId = 158;// Entry taken from TM_DealerHOBRData for 158
                //sDealerCode = "1S3045";

                ds = ObjEstimate.GetEstimate(iDocID, "New", iDealerId, iHOBranchDealerId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Est_canc_tag"] = "N";
                            ds.Tables[0].Rows[0]["Est_confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";                        
                            sNew = "Y";
                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "Est", iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false); 
                txtDocDate.Text = Func.Common.sGetCurrentDateTime(Location.iCountryId, true);
                hdnTrNo.Value = Location.sDealerCode + "/Est/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
                if (Session["DepartmentID"].ToString() == "7") lblSelectModel.Visible = true;

                ObjEstimate = null;
                ds = null;
                ObjEstimate = null;
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
                        dtDefaultPart.Columns.Add(new DataColumn("ReqQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("BillQty", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Total", typeof(double)));
                        //dtDefaultPart.Columns.Add(new DataColumn("Failed_Make", typeof(string)));                    
                        dtDefaultPart.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("WarrRate", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Stock", typeof(double)));
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
                    dr["ReqQty"] = 0;
                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["Status"] = "S";
                    dr["WarrRate"] = 0;
                    dr["Stock"] = 0;
                    dtDefaultPart.Rows.Add(dr);
                    dtDefaultPart.AcceptChanges();
                }
            Bind:
                Session["PartDetails"] = dtDefaultPart;
                PartDetailsGrid.DataSource = dtDefaultPart;
                PartDetailsGrid.DataBind();
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


                    //Req Qty
                    //PartDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "none"); // Hide Header        
                    //PartDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "none");//Hide Cell

                    // Add New Part                  
                    LinkButton lnkSelectPart = (LinkButton)PartDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    //lnkSelectPart.Attributes.Add("onclick", "return ShowSpWPFPart(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");

                    //Part Detail ID                    
                    TextBox txtDtlPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtDtlPartID");

                    //PartID
                    TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    //Part No
                    TextBox txtPartNo = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox);

                    //Part Type Tag 
                    TextBox txtPartTypeTag = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartTypeTag") as TextBox);

                    TextBox txtPartGroupCode = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox);


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

                        if (sPartId != "0")
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
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).ToolTip = sPartName;

                        //Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["ReqQty"]);
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Rate Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Rate"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Total Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Total"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        if (txtPartTypeTag.Text == "P") dPartAmount = dPartAmount + dTmpValue;
                        if (txtPartTypeTag.Text == "O") dLubAmount = dLubAmount + dTmpValue;

                        if (txtPartTypeTag.Text == "P")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5');");
                        }
                        else
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                        }

                        txtPartGroupCode.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["group_code"]);

                        txtPTax.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax"]).Trim();
                        txtPTax1.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax1"]).Trim();
                        txtPTax2.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax2"]).Trim();

                        idtRowCnt = idtRowCnt + 1;
                    }

                    //Label lnkCancel = (Label)PartDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "");
                    lnkSelectPart.Style.Add("display", "none");
                    Chk.Checked = false;
                    //If Part Id  is not allocated
                    if (sPartId == "0")
                    {
                        txtPartNo.Style.Add("display", "none");
                        if (bShowNewPart == true)
                        {
                            lnkSelectPart.Style.Add("display", "");
                            if (txtUserType.Text == "6") lnkSelectPart.Style.Add("display", "none");
                            bShowNewPart = false;
                        }
                    }
                    if (sRecordStatus == "D")
                    {
                        //Chk.Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                        Chk.Checked = true;
                    }
                    //else
                    //{                     
                    //    Chk.Style.Add("display", "");
                    //}                   
                }

                txtPartAmount.Text = dPartAmount.ToString("0.00");
                txtLubricantAmount.Text = dLubAmount.ToString("0.00");

                txtEstTotAmt.Text = Func.Convert.dConvertToDouble(txtPartAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLubricantAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLabourAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtSubletAmount.Text.ToString()).ToString("0.00");

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void lnkSelectEstDtl_Click(object sender, EventArgs e)
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
                txtPartIds.Text = "";
                bindGrid_SelectPart("A", "x");
                mpeSelectPart.Show();

                //bFillDetailsFromPartGrid();
                //BindDataToPartGrid(true, 0);
                //dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                //CreateNewRowToTaxGroupDetailsTable();
                //Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                //Session["JbTaxDetails"] = dtJbTaxDetails;
                //BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Part Grid
        private bool bFillDetailsFromPartGrid()
        {
            string sStatus;
            dtPart = (DataTable)Session["PartDetails"];
            int iCntForDelete = 0;
            int iPartID = 0;
            double iPartReqQty = 0;
            double iPartBillQty = 0;

            bool bValidate = false;
            double dRate = 0;
            //hdnSelectedPartID.Value = "";

            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                //PartID                
                iPartID = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                if (iPartID != 0)
                {
                    //ID
                    //if (txtRefClaimID.Text != "")
                    //    dtPart.Rows[iRowCnt]["ID"] = 0;

                    dtPart.Rows[iRowCnt]["PartLabourID"] = iPartID;

                    iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

                    //if (Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ReqQty"]) > Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["IssueQty"]) ||(iPartID != 0 && Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ID"]) == 0 )) 
                    //hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + iPartID;

                    //PartNo Or NewPart
                    dtPart.Rows[iRowCnt]["parts_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

                    // Get Required Qty
                    iPartReqQty = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtReqQty") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["ReqQty"] = iPartReqQty;

                    // Get LubLocID     
                    //dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);

                    // Get war_tag
                    //dtPart.Rows[iRowCnt]["war_tag"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtwar_tag") as TextBox).Text);
                    //dtPart.Rows[iRowCnt]["war_tag"] = "D";

                    // Get Rate
                    dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Rate"] = dRate;

                    iPartBillQty = iPartReqQty;
                    //// Get Total
                    dtPart.Rows[iRowCnt]["Total"] = iPartBillQty * dRate;// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);

                    //LubLocID
                    //dtPart.Rows[iRowCnt]["LubLocID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);                   
                    //EstDtlID =0 taken becuase select labour page same as jobcard
                    dtPart.Rows[iRowCnt]["EstDtlID"] = 0;
                    dtPart.Rows[iRowCnt]["group_code"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartGroupCode") as TextBox).Text);

                    dtPart.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox).Text);
                    // Record Status
                    sStatus = "S";

                    if (iPartReqQty != 0)
                    {
                        bValidate = true;
                        dtPart.Rows[iRowCnt]["Status"] = "S";// for Save 
                    }
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtPart.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }

                }
            }
            bValidate = true;
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
                        dtDefaultLabour.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("AddLbrDescription", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("out_lab_desc", typeof(String)));
                        dtDefaultLabour.Columns.Add(new DataColumn("out_Lab_amt", typeof(double)));
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
                    dr["labour_code"] = "";
                    dr["PartLabourName"] = "";
                    dr["out_lab_desc"] = "";
                    dr["ManHrs"] = 0;
                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["AddLbrDescription"] = "";
                    dr["out_Lab_amt"] = 0;
                    dr["AddLbrDescriptionID"] = 0;

                    dtDefaultLabour.Rows.Add(dr);
                    dtDefaultLabour.AcceptChanges();
                }

            Bind:
                Session["LabourDetails"] = dtDefaultLabour;
                LabourDetailsGrid.DataSource = dtDefaultLabour;
                LabourDetailsGrid.DataBind();
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

                    //LabourNo Or NewLabour
                    TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");

                    // New  Labour Control                   
                    LinkButton lnkSelectLabour = (LinkButton)LabourDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectLabour");
                    //lnkSelectLabour.Attributes.Add("onclick", "return ShowMultiLabourMaster(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");

                    lnkSelectLabour.Style.Add("display", "none");
                    //Labour Name
                    TextBox txtLabourDesc = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc");
                    //txtLabourDesc.Attributes.Add("disabled", "disabled");

                    //Other Description
                    DropDownList drpLbrDescription = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription");                    
                    //MDUser Change
                    //Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Location.iDealerId);
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    //MDUser Change

                    TextBox txtLbrDescription = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription");

                    //Lab Tag
                    TextBox txtLabCD = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabCD");

                    // Man Hrs
                    TextBox txtManHrs = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs");

                    //Rate
                    TextBox txtRate = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate");


                    //Total
                    TextBox txtTotal = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");

                    //Sublet Amt    out_Lab_amt
                    TextBox txtSubletAmt = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletAmt");

                    //Sublet Labour Description     out_lab_desc
                    TextBox txtSubletDescription = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletDescription");

                    //Labour group for tax
                    TextBox txtLGroupCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");

                    TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox);
                    TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox);
                    TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox);

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
                        drpLbrDescription.Enabled = (sFirstFiveDigit == "MTIMI") ? true : false;

                        //txtLabCD.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Lab_Tag"]);
                        txtLabCD.Text = "D";

                        sLabCode = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["labour_code"]);
                        sLabTag = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Lab_Tag"]).Trim();
                        string sEstDtlID = "0";
                        //sEstDtlID = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["EstDtlID"]);

                        txtLGroupCode.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["group_code"]);

                        txtLTax.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax"]).Trim();
                        txtLTax1.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax1"]).Trim();
                        txtLTax2.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax2"]).Trim();

                        if (sLabourId != "0" && sFirstFiveDigit != "MTIMI") hdnSelectedLabourID.Value = hdnSelectedLabourID.Value + ((hdnSelectedLabourID.Value.Length > 0) ? "," : "") + sLabourId.Trim() + "<--" + sLabTag.Trim() + "<--" + sEstDtlID;

                        //Man Hrs
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["ManHrs"]);
                        if (dTmpValue != 0)
                        {
                            txtManHrs.Text = dTmpValue.ToString("0.00");
                        }

                        //Rate 
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Rate"]);
                        if (dTmpValue != 0)
                        {
                            txtRate.Text = dTmpValue.ToString("0.00");
                        }

                        //Total
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Total"]);
                        if (dTmpValue != 0)
                        {
                            txtTotal.Text = dTmpValue.ToString("0.00");
                        }
                        //txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

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
                        if (sFirstFiveDigit != "MTIOU")
                        {
                            dLabourAmount = dLabourAmount + dTmpValue;
                        }
                        //Sublet Amt    out_Lab_amt
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["out_Lab_amt"]);
                        if (sFirstFiveDigit == "MTIOU")
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

                        sRecordStatus = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;
                    }

                    //Label lnkCancel = (Label)LabourDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //Delete 
                    CheckBox Chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");

                    //If Labour Id  is not allocated
                    if (sLabourId == "0")
                    {
                        if (bShowLabour == true)
                        {
                            bShowLabour = false;
                            lnkSelectLabour.Style.Add("display", "");
                            //MDUser Change
                            if (txtUserType.Text == "6") lnkSelectLabour.Style.Add("display", "none");
                            //MDUser Change
                        }
                        txtLabourCode.Style.Add("display", "none");
                    }
                    Chk.Checked = false;
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        LabourDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                        Chk.Checked = true;
                    }
                    else if (sRecordStatus == "E" || sRecordStatus == "S")
                    {
                        Chk.Style.Add("display", "");
                        //lnkCancel.Style.Add("display", "none");
                    }
                    if (bRecordIsOpen == false || Session["DepartmentID"].ToString() == "6")
                    {
                        Chk.Style.Add("display", "none");
                        //lnkCancel.Style.Add("display", "none");
                        lnkSelectLabour.Style.Add("display", "none");
                        txtManHrs.Enabled = false;
                    }
                }
                txtLabourAmount.Text = dLabourAmount.ToString("0.00");
                txtSubletAmount.Text = dSubletAmount.ToString("0.00");
                txtEstTotAmt.Text = (Func.Convert.dConvertToDouble(txtPartAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLubricantAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtLabourAmount.Text.ToString()) + Func.Convert.dConvertToDouble(txtSubletAmount.Text.ToString())).ToString("0.00");
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
                //bindGrid("A", "x");
                
                    DrpSelFrom_Labour.Visible =  false ;
                    Func.Common.BindDataToCombo(DrpLabGrp, clsCommon.ComboQueryType.LaborGroup, Func.Convert.iConvertToInt(txtModCatIDBasic.Text));
                    txtPartIds_Labour.Text = "";
                    DrpLabourSelect.Items.Add(new ListItem("Paid Labour", "D", true));                    
                    //bindGrid_SelectLabour("A","x");
                    LabourDetailsGrid_Labour.DataSource = null;
                    LabourDetailsGrid_Labour.DataBind();                
                    mpeSelectLabour.Show();

                //bFillDetailsFromLabourGrid();
                //BindDataToLaborGrid(true, 0);

                //dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                //CreateNewRowToTaxGroupDetailsTable();
                //Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                //Session["JbTaxDetails"] = dtJbTaxDetails;
                //BindDataToGrid();
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

                    // Get ManHrs
                    dManHrs = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["ManHrs"] = dManHrs;

                    // Get Rate
                    dRate = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Rate"] = dRate;

                    // Get Total
                    //dtLabour.Rows[iRowCnt]["Total"] = dManHrs * dRate;//Commited by VIkram Dated 08092017// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);                                       

                    //Labour Main group LbrMnGrp     
                    dtLabour.Rows[iRowCnt]["LbrMnGrp"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabMnGr") as TextBox).Text;

                    //Sublet Amt    out_Lab_amt
                    dtLabour.Rows[iRowCnt]["out_Lab_amt"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletAmt") as TextBox).Text);

                    //Sublet Labour Description     out_lab_desc
                    dtLabour.Rows[iRowCnt]["out_lab_desc"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtSubletDescription") as TextBox).Text;

                    //Labour EstDtlID //taken becuase select labour page same as jobcard
                    dtLabour.Rows[iRowCnt]["EstDtlID"] = 0;

                    //Labour group code
                    dtLabour.Rows[iRowCnt]["group_code"] = Func.Convert.sConvertToString((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode") as TextBox).Text);

                    dtLabour.Rows[iRowCnt]["Tax"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Tax1"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Tax2"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox).Text);

                    dtLabour.Rows[iRowCnt]["Status"] = "S";// for Save      

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

        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select records.";
            bool bValidateRecord = true;

            if (Func.Convert.iConvertToInt(Session["DepartmentID"]) == 6 && Func.Convert.iConvertToInt(txtID.Text) == 0)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Store User Cannot create Estimate.');</script>");
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

            //Validation For Model Ramarks
            return bValidateRecord;
        }

        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;

                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Est_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("EstDate", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Chassis_ID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Est_confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Est_canc_tag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("SupervisiorID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("InspectDt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("VisitDt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsurnceValDt", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("InsuranceDiv", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Phone", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Surveyor", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("DriverLicNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DriverName", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PolicyNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsurnceComp", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InsCustID", typeof(int)));
                
                
                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Est_no"] = txtDocNo.Text;
                dr["EstDate"] = txtDocDate.Text;

                dr["Dealer_ID"] = Location.iDealerId;

                dr["DlrBranchID"] = Func.Convert.iConvertToInt(Session["HOBR_ID"]);

                dr["CustID"] = Func.Convert.iConvertToInt(txtCustID.Text.ToString());
                dr["Chassis_ID"] = Func.Convert.iConvertToInt(txtChassisID.Text.ToString());

                dr["VisitDt"] = dtpVisitTime.Text;

                dr["Est_confirm"] = "N";
                dr["Est_canc_tag"] = "N";

                dr["UserID"] = Func.Convert.iConvertToInt(Session["UserID"].ToString());

                dr["SupervisiorID"] = Func.Convert.iConvertToInt(DrpSupervisorName.SelectedValue.ToString());
                dr["InsurnceValDt"] = txtInsValidDate.Text;

                dr["InsuranceDiv"] = txtInsuranceDiv.Text;
                dr["Phone"] = txtPhone.Text;
                dr["Surveyor"] = txtSurveyor.Text;
                dr["InspectDt"] = dtpInspectDt.Text;
                dr["DriverLicNo"] = txtDriverLicNo.Text;
                dr["DriverName"] = txtDriverName.Text;
                dr["PolicyNo"] = txtPolicyNo.Text;
                dr["InsurnceComp"] = txtInsurnceComp.Text;
                dr["DocGST"] = hdnISDocGST.Value;
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

                if (bValidateRecord() == false)
                {
                    return false;
                }
                DataTable dtHdr = new DataTable();
                clsEstimate ObjEstimate = new clsEstimate();

                UpdateHdrValueFromControl(dtHdr);
                //Get Model Details     
                bDetailsRecordExist = false;
                //Get Part Details
                if (bFillDetailsFromPartGrid() == false) return false;
                //Get Labor Details
                if (bFillDetailsFromLabourGrid() == false) return false;
                //Get Complaint Details     
                if (bFillDetailsFromComplaintGrid() == false) return false;

                if (bFillDetailsFromInvDescGrid() == false) return false;                

                bFillDetailsFromTaxGrid();

                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["Est_confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Est_canc_tag"] = "Y";
                }
                string sAdd = (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0) ? "Y" : "N";
                //if (ObjEstimate.bSaveEstimate(ref iDocID, Location.sDealerCode, dtHdr, dtPart, dtLabour, dtComplaint, dtEst, dtInvestigations, dtActionTaken, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString())) == true)
                if (ObjEstimate.bSaveEstimate(ref iDocID, Location.sDealerCode, dtHdr, dtPart, dtLabour, dtComplaint, dtJbGrpTaxDetails, dtJbTaxDetails, Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString()), sAdd, dtInvJobDescDet) == true)
                {
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Estimate") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(7)", true);
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Estimate") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(8)", true);
                        return true;
                    }
                    else
                    {

                        txtID.Text = Func.Convert.sConvertToString(iDocID);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Estimate") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Estimate") + "','" + Server.HtmlEncode(txtDocNo.Text) + "');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    return false;
                }

                ObjEstimate = null;
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
                SearchGrid.sGridPanelTitle = "Estimate List";
                SearchGrid.AddToSearchCombo("Estimate No");
                SearchGrid.AddToSearchCombo("Est Date");
                SearchGrid.AddToSearchCombo("Est Status");                
                //MDUser Change
                //SearchGrid.iDealerID = Location.iDealerId;
                //SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
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
                //MDUser Change
                SearchGrid.sSqlFor = "EstimateList";
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
                clsEstimate ObjEstimate = new clsEstimate();
                DataSet ds = new DataSet();
                int iDocID = Func.Convert.iConvertToInt(txtID.Text);                
                //MDUser Change
                //iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);//set it to actual ho branch id
                if (txtUserType.Text == "6")
                {
                    iHOBranchDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                }
                //MDUser Change
                //iProformaID = 1;
                if (iDocID != 0)
                {
                    ds = ObjEstimate.GetEstimate(iDocID, "All", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);

                }
                else
                {
                    ds = ObjEstimate.GetEstimate(iDocID, "Max", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
                }
                ds = null;
                ObjEstimate = null;
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

            txtChassisID.Text = "";
            txtEngineNo.Text = "";


            txtCustomer.Text = "";
            txtCustID.Text = "0";

            TxtModelCode.Text = "";
            txtModelName.Text = "";
            txtModelGroupID.Value = "0";

            DrpSupervisorName.SelectedValue = "0";
            //'N' as JbCd_Confirm
            txtInsValidDate.Text = "";

            //Display Details
            hdnConfirm.Value = "N";
            hdnCancle.Value = "N";

            //Display Part Details  
            Session["PartDetails"] = null;
            lblPartRecCnt.Text = "0";
            PartDetailsGrid.DataSource = null;
            PartDetailsGrid.DataBind();

            // Display Labour Details        
            Session["LabourDetails"] = null;
            lblLabourRecCnt.Text = "0";
            LabourDetailsGrid.DataSource = null;
            LabourDetailsGrid.DataBind();

            // Display Complaints Details             
            Session["ComplaintsDetails"] = null;
            lblComplaintsRecCnt.Text = "0";
            ComplaintsGrid.DataSource = null;
            ComplaintsGrid.DataBind();

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
                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Est_no"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EstDate"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                txtChassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_ID"]);
                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["chassis_no"]);
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["engine_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["vehicle_no"]);

                txtCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["cust_NAME"]);
                txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                TxtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_code"]);
                txtModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_name"]);
                txtModelGroupID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gr_ID"]);

                dtpInspectDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InspectDt"]);

                dtpVisitTime.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VisitDt"]);

                if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0)
                {
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Location.iDealerId + " and Empl_Type=1" + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]) : ""));
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type=1" + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SupervisiorID"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]) : ""));
                    }
                }
                else
                {
                    FillCombo();
                }

                DrpSupervisorName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SupervisiorID"]);

                txtInsValidDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsurnceValDt"]);

                txtInsurnceComp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsurnceComp"]);

                txtPolicyNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PolicyNo"]);
                txtDriverName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DriverName"]);
                txtDriverLicNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DriverLicNo"]);
                txtSurveyor.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Surveyor"]);
                txtPhone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Phone"]);
                txtInsuranceDiv.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsuranceDiv"]);
                txtModCatIDBasic.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Mod_Cat_ID_Basic"]);
                // Commented By Shyamal as on 16/02/2011
                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Est_confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Est_canc_tag"]);
                hdnISDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnRounOff.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);
                
                DrpInsurnceComp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InsCustID"]);
                txtInsurnceComp.Style.Add("display", (hdnConfirm.Value == "Y" && Func.Convert.iConvertToInt(DrpInsurnceComp.SelectedValue) == 0) ? "" : "none");
                DrpInsurnceComp.Style.Add("display", (hdnConfirm.Value == "N" || Func.Convert.iConvertToInt(DrpInsurnceComp.SelectedValue) > 0) ? "" : "none");

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

                // Display Complaints Details     
                dtComplaint = ds.Tables[1];
                Session["ComplaintsDetails"] = dtComplaint;
                lblComplaintsRecCnt.Text = Func.Common.sRowCntOfTable(dtComplaint);
                //BindDataToComplaintGrid(true, 0);
                BindDataToComplaintGrid();


                dtJbGrpTaxDetails = ds.Tables[4];
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;

                dtJbTaxDetails = ds.Tables[5];
                Session["JbTaxDetails"] = dtJbTaxDetails;

                dtInvJobDescDet = ds.Tables[6];
                Session["InvJobDescDet"] = dtInvJobDescDet;

                BindDataToGrid();

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true); //print option enable for save & confirm
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                lblSelectModel.Visible = (Session["DepartmentID"].ToString() == "7" && (txtID.Text == "" || txtID.Text == "0")) ? true : false;
                //MDUser Change
                if (txtUserType.Text == "6") lblSelectModel.Visible = false;
                //MDUser Change

                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
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
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {
            dtpInspectDt.Enabled = bEnable;
            txtInsValidDate.Enabled = bEnable;
            ComplaintsGrid.Enabled = bEnable;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            clsExcel objExcel = null;

            System.Data.DataTable dtDownload = null;
            try
            {
                objExcel = new clsExcel();
                //dtDownload = (DataTable)Session["PartDetails"];
                DataSet dts = new DataSet();
                clsEstimate ObjEstimate = new clsEstimate();
                dts = ObjEstimate.GetEstimate(Func.Convert.iConvertToInt(txtID.Text), "All", Location.iDealerId, iHOBranchDealerId);
                dtDownload = dts.Tables[2];
                dtDownload.Rows.RemoveAt(0);
                if (dtDownload != null)
                    if (dtDownload.Rows.Count > 0)
                    {
                        dtDownload.Columns.Remove("SRNo");
                        dtDownload.Columns.Remove("ID");
                        dtDownload.Columns.Remove("part_type_tag");
                        dtDownload.Columns.Remove("PartLabourID");
                        dtDownload.Columns.Remove("PartLabourName");
                        dtDownload.Columns.Remove("Rate");
                        dtDownload.Columns.Remove("Total");
                        dtDownload.Columns.Remove("Status");
                        dtDownload.Columns.Remove("WarrRate");
                        dtDownload.Columns.Remove("Stock");

                        dtDownload.Columns["parts_no"].ColumnName = "PartNo";
                        dtDownload.Columns["ReqQty"].ColumnName = "Quantity";
                    }
                objExcel.DownloadInExcelFile(dtDownload, txtDocNo.Text + ".xls", "");

            }
            catch (Exception ex)
            {
                //Func.Common.ProcessUnhandledException(ex);
            }
        }

        #region  InvDescs Function

        // Set Control property To InvDesc Grid    
        private void SetControlPropertyToInvDescGrid()
        {
            try
            {   
                int idtRowCnt = 0;                
                
                for (int iRowCnt = 0; iRowCnt < InvJobDescGrid.Rows.Count; iRowCnt++)
                {
                    //Description
                    DropDownList DrpInvJobDesc = (DropDownList)InvJobDescGrid.Rows[iRowCnt].FindControl("DrpInvJobDesc");
                    Func.Common.BindDataToCombo(DrpInvJobDesc, clsCommon.ComboQueryType.InvJobDescription, 0);

                    if (idtRowCnt < dtInvJobDescDet.Rows.Count)
                    {

                        //DrpInvJobDesc.Attributes.Add("onChange", "return CheckInvDescSelected(event,this);");

                        DrpInvJobDesc.SelectedValue = Func.Convert.sConvertToString(dtInvJobDescDet.Rows[iRowCnt]["InvDescID"]);

                        DrpInvJobDesc.Attributes.Add("onChange", "return CheckInvDescValueAlreadyUsedInGrid(this);");

                        idtRowCnt = idtRowCnt + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From InvDesc Grid
        private bool bFillDetailsFromInvDescGrid()
        {
            dtInvJobDescDet = (DataTable)Session["InvJobDescDet"];

            int iCntForDelete = 0;
            int iInvDescID = 0;

            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < InvJobDescGrid.Rows.Count; iRowCnt++)
            {
                //InvDescID                
                iInvDescID = Func.Convert.iConvertToInt((InvJobDescGrid.Rows[iRowCnt].FindControl("DrpInvJobDesc") as DropDownList).SelectedValue);

                if (iInvDescID != 0)
                {
                    bValidate = true;

                    dtInvJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                    iCntForDelete++;
                }
                else
                {
                    dtInvJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                }
            }
            bValidate = true;
            //if (iCntForDelete == 0)
            //{
            //    bValidate = false;
            //}

            //if (bValidate == false)
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select At least one Estimate Job Description.');</script>");
            //}
        Last:
            return bValidate;
        }
                
        private bool bSaveInvJobDescDetails(clsDB objDB, string sDealerCode, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["InvDescID"].ToString() != "0" || dtDet.Rows[iRowCnt]["ID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_InvDescDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["InvDescID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        #endregion

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
                dtJbTaxDetails.Rows[iRowCnt]["Estimate_tot"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
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
                
                int iRow = dtInvJobDescDet.Rows.Count;
                for (int iRowCnt = iRow + 1; iRowCnt <= 5; iRowCnt++)
                {
                    DataRow dr;

                    dr = dtInvJobDescDet.NewRow();
                    dr["ID"] = 0;
                    dr["InvDescID"] = 0;
                    dtInvJobDescDet.Rows.Add(dr);
                    dtInvJobDescDet.AcceptChanges();
                }
                InvJobDescGrid.DataSource = dtInvJobDescDet;
                InvJobDescGrid.DataBind();
                SetControlPropertyToInvDescGrid();

                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();

                PInvDesc.Style.Add("display", "none");
                //PInvDesc.Style.Add("display", (hdnISDocGST.Value.ToString() == "Y") ? "" : "none");
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
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnISDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %"): "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnISDocGST.Value == "Y") ?  ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt"): "Tax Amt"; // Hide Header   

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
                   
                    //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

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
                        //Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));


                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

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

                    TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax2") as TextBox);


                    if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtPTax.Text)
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

                        TextBox txtSubletAmt = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtSubletAmt");
                        TextBox TxtExclTotal = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtTotal");

                        TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax") as TextBox);
                        TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax1") as TextBox);
                        TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax2") as TextBox);

                        double LTotal = 0;
                        if (txtLabCD.Text.Trim() == "D" && sFirstFiveDigit != "MTIOU")
                        {
                            LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());
                        }

                        if (txtLabCD.Text.Trim() == "D" && sFirstFiveDigit == "MTIOU")
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

                    if (hdnConfirm.Value == "Y")
                    {
                        dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 0);
                    }
                    else
                    {
                        dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 2);
                    }
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

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnISDocGST.Value == "Y" ) ? "none" : ""); // Hide Header        
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


        private int i = 1;
        protected void PartDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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

        #region Code for Select Chasssis Link

        protected void lblSelectModel_Click(object sender, EventArgs e)
        {
            BindData();
            mpeSelectChassis.Show();


            // Vikram on 14122016
            //DataRow[] dtSrchgrid;
            //DataTable dtSearch;
            //dtSearch = (DataTable)Session["ChassisDetails"];
            ////dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString() + " and VehInID=" + txtPreviousDocId.Text.ToString());
            //if (dtSearch != null)
            //{
            //    dtSrchgrid = dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString(), "", DataViewRowState.CurrentRows);

            //    if (dtSrchgrid.Length != 0)
            //    {
            //        for (int i = 0; i < dtSrchgrid.Length; i++)
            //        {
            //            txtChassisID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_ID"]);
            //            txtChassisNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_no"]);
            //            txtVehicleNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Vehicle_No"]);
            //            txtCustomer.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Customer_name"]);
            //            txtEngineNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Engine_no"]);                        
            //            txtCustID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["CustID"]);
            //            hdnCustTaxTag.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["CustTaxTag"]);
            //            TxtModelCode.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_code"]);
            //            txtModelName.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_name"]);                        
            //            txtModelGroupID.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_Gr_ID"]);
            //            txtModCatIDBasic.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Mod_Cat_ID_Basic"]);

            //            BindDataToPartGrid(true, 0);
            //            BindDataToLaborGrid(true, 0);                        
            //        }
            //    }
            //}
        }

        private string sSelText, sSelType;
        private int iJobtype, iDealerID, iHOBrID;

        private void BindData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;

                sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                sSelText = txtSearch.Text.ToString();
                //iJobtype = Func.Convert.iConvertToInt(Request.QueryString["JobTypeID"].ToString());
                //iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                //iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                iJobtype = 999;
                iDealerID = Func.Convert.iConvertToInt(Location.iDealerId);
                iHOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"].ToString());

                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                ViewState["Chassis"] = dtSrchgrid;
                Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                //// if (DdlSelctionCriteria.SelectedValue == "Chassis_no" && txtSearch.Text != "")
                //dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                //ChassisGrid.DataSource = dvDetails;
                ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

                if (ChassisGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < ChassisGrid.Rows.Count; iRowCnt++)
                {

                    //Show Vehicle In No 
                    ChassisGrid.HeaderRow.Cells[5].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[5].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Vehicle In Date
                    ChassisGrid.HeaderRow.Cells[6].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[6].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Kms
                    ChassisGrid.HeaderRow.Cells[18].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[18].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Hrs
                    ChassisGrid.HeaderRow.Cells[19].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[19].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell
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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
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
            BindData();
            mpeSelectChassis.Show();
        }

        protected void ChassisGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ChassisGrid.PageIndex = e.NewPageIndex;
            BindData();
            mpeSelectChassis.Show();
        }

        protected void btnOkChassis_Click(object sender, EventArgs e)
        {
            DataRow[] dtSrchgrid;
            DataTable dtSearch;
            dtSearch = (DataTable)Session["ChassisDetails"];
            //dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString() + " and VehInID=" + txtPreviousDocId.Text.ToString());
            if (dtSearch != null)
            {
                dtSrchgrid = dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString(), "", DataViewRowState.CurrentRows);

                if (dtSrchgrid.Length != 0)
                {
                    for (int i = 0; i < dtSrchgrid.Length; i++)
                    {
                        txtChassisID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_ID"]);
                        txtChassisNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_no"]);
                        txtVehicleNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Vehicle_No"]);
                        txtCustomer.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Customer_name"]);
                        txtEngineNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Engine_no"]);
                        txtCustID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["CustID"]);
                        hdnCustTaxTag.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["CustTaxTag"]);
                        TxtModelCode.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_code"]);
                        txtModelName.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_name"]);
                        txtModelGroupID.Value = Func.Convert.sConvertToString(dtSrchgrid[i]["Model_Gr_ID"]);
                        txtModCatIDBasic.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Mod_Cat_ID_Basic"]);

                        BindDataToPartGrid(true, 0);
                        BindDataToLaborGrid(true, 0);
                        //BindDataToGrid();
                    }
                }
            }
        }
        #endregion

        #region Code For Select Part
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt_Part = 0;
        string sSelectedPartID = "";

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                int idataCount = 0;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();

                dtDetails = (DataTable)Session["PartDetails"];
                idataCount = dtDetails.Rows.Count;

                dtTemp = dtDetails.Clone();

                // Get first row from table dtDetails,Previously it was last row,Changed by Shyamal on 02062012
                //dtTemp.ImportRow(dtDetails.Rows[0]);
                //dtDetails.Rows.RemoveAt(0);


                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    //dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    //dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    //dtDetails.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
                    //dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    //dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    //dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                    //dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));
                    //dtDetails.Columns.Add(new DataColumn("FOBRate", typeof(double)));
                    //dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    //dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    //dtDetails.Columns.Add(new DataColumn("Process_Accept", typeof(Boolean)));
                    //dtDetails.Columns.Add(new DataColumn("Process_Confirm", typeof(Boolean)));

                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_type_tag", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("parts_no", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("ReqQty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Stock", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("EstDtlID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Tax", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax1", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax2", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                }



                //Sujata 08012011
                string sPartID = sSelectedPartID;
                if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds.Text);
                    string[] asd = myString.ToString().Split('#');
                    int iRowCnt = 0;

                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        //if (dtDetails.Rows.Count == 301) break;
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        dr = dtDetails.NewRow();
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                        sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();
                        dr["parts_no"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["part_type_tag"] = Func.Convert.sConvertToString(myArray[5]);
                        dr["ReqQty"] = Func.Convert.dConvertToDouble(0);
                        //dr["ReqQty"] = Func.Convert.dConvertToDouble(myArray[12]);                        
                        dr["Rate"] = Func.Convert.dConvertToDouble(myArray[3]);
                        dr["WarrRate"] = Func.Convert.dConvertToDouble(myArray[4]);
                        dr["Stock"] = Func.Convert.dConvertToDouble(myArray[6]);
                        dr["EstDtlID"] = Func.Convert.iConvertToInt(myArray[7]);
                        dr["group_code"] = Func.Convert.sConvertToString(myArray[8]);
                        dr["Tax"] = Func.Convert.iConvertToInt(myArray[9]);
                        dr["Tax1"] = Func.Convert.iConvertToInt(myArray[10]);
                        dr["Tax2"] = Func.Convert.iConvertToInt(myArray[11]);
                        dr["Status"] = "S";

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }
                //Sujata 08012011

                //for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                //{
                //    if ((PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart") as CheckBox).Checked == true)
                //    {
                //        dr = dtDetails.NewRow();
                //        dr["SRNo"] = iRowCnt;
                //        dr["ID"] = 0;
                //        dr["Part_No_ID"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                //        dr["part_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblPartNo") as Label).Text; ;
                //        dr["Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblPartName") as Label).Text; ;

                //        //Sujata 25122010
                //        //dr["Qty"] = 1;//(PartDetailsGrid.Rows[iRowCnt].FindControl("lblQty") as TextBox).Text; ;
                //        dr["Qty"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text; ;
                //        //Sujata 25122010
                //        dr["MOQ"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text; ;
                //        dr["FOBRate"] = Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);

                //        //dr["Total"] = 1 * Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);
                //        dr["Total"] = Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text) * Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);
                //        dr["Status"] = "U";
                //        dr["Process_Accept"] = 0;
                //        dr["Process_Confirm"] = 0;
                //        dtDetails.Rows.Add(dr);
                //        dtDetails.AcceptChanges();
                //    }
                //}

                //No need to Add row again,Modified by Shyamal 02062012
                //dtDetails.ImportRow(dtTemp.Rows[0]);

                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");            
                //DLL.clsDB objDB = new DLL.clsDB();
                //int iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartSuperceeded", sPartID, iDealerId);
                //dtDetails = dsSrchgrid.Tables[0]; 
                Session["PartDetails"] = dtDetails;

                // For Binding data to Parent Page

                bFillDetailsFromPartGrid();
                BindDataToPartGrid(true, 0);
                dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                dtInvJobDescDet = (DataTable)Session["InvJobDescDet"];
                CreateNewRowToTaxGroupDetailsTable();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbTaxDetails"] = dtJbTaxDetails;
                BindDataToGrid();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            PagerV2_1.CurrentIndex = currnetPageIndx;
            bindGrid_SelectPart(Func.Convert.sConvertToString(DdlSelctionCriteria_part.SelectedValue), Func.Convert.sConvertToString(txtSearch_Part.Text));
            mpeSelectPart.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PagerV2_1.ItemCount = 10;
                PagerV2_1.CurrentIndex = 0;
                bindGrid_SelectPart(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
                mpeSelectPart.Show();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        public void bindGrid_SelectPart(string sSelect, string sSearchPart)
        {
            DataSet dsSrchgrid;
            clsDB objDB = new clsDB();
            try
            {                
                DrpSelFrom.Visible = false;
                sSelect="A";
                sSearchPart = "x";
                int iDealerId;
                int EstID;
                string RepairOrderDate = "";
                string sCustTaxTag = "";
                string sDocGST="";
                if (Func.Convert.sConvertToString(txtSearch_Part.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch_Part.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria_part.SelectedValue);
                }
                if (DrpSelFrom.SelectedValue == "E") sSelect = "E" + sSelect;
                //iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                //sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                //RepairOrderDate = Func.Convert.sConvertToString(Request.QueryString["RepairOrderDate"]);
                //EstID = Func.Convert.iConvertToInt(Request.QueryString["EstID"]);
                //sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);

                iDealerId = Func.Convert.iConvertToInt(Location.iDealerId);
                sSelectedPartID = hdnSelectedPartID.Value;
                RepairOrderDate = txtDocDate.Text;
                EstID = 0;
                sCustTaxTag = Func.Convert.sConvertToString(hdnCustTaxTag.Value);
                sDocGST= Func.Convert.sConvertToString(hdnISDocGST.Value);

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }


                //dsSrchgrid = BLL.Func.DB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDPRate", iDealerId, sSelectedPartID);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDPRate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID);
                //object paravalues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, RepairOrderDate, sSelectedPartID, EstID, sCustTaxTag, sDocGST };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDP_FOB_Rate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, RepairOrderDate, sSelectedPartID, EstID, sCustTaxTag, sDocGST);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDP_FOB_Rate_Paging", paravalues);

                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    //iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][6]);
                    iTotalCnt_Part = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][7]);
                    PagerV2_1.ItemCount = iTotalCnt_Part;
                    PartDetailsGrid_Part.DataSource = dsSrchgrid;
                    PartDetailsGrid_Part.DataBind();
                    ChkSelectedParts_Part();
                    lblNMsg.Visible = false;
                }
                else
                {
                    lblNMsg.Visible = true;
                    PagerV2_1.ItemCount = 0;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts_Part();
                    return;
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

        private void ChkSelectedParts_Part()
        {
            CheckBox chk = new CheckBox();
            string sPartID = "";
            string str = null;
            string[] strArr = null;
            string strAllLabor = null;
            string[] strAllLaborArr = null;

            //txtPartIds.Text = "";
            //str = txtPartIds.Text;
            //if (str == "") return;
            //strArr = str.Split('#');
            strAllLabor = txtPartIds.Text;
            if (strAllLabor == "") return;
            strAllLaborArr = strAllLabor.Split('#');

            for (int i = 0; i < strAllLaborArr.Length; i++)
            {
                //str = txtPartIds.Text;
                str = Func.Convert.sConvertToString(strAllLaborArr[i]);
                if (str == "") return;

                string[] stringSeparators = new string[] { "<--" };

                strArr = str.Split(stringSeparators, StringSplitOptions.None);

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid_Part.Rows.Count; iRowCnt++)
                {
                    sPartID = (PartDetailsGrid_Part.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                    if (sPartID == Func.Convert.sConvertToString(strArr[0]))
                    {
                        chk = (CheckBox)PartDetailsGrid_Part.Rows[iRowCnt].FindControl("ChkPart");
                        chk.Checked = true;
                        break;
                    }
                    //for (int j = 0; j < strArr.Length; j++)
                    //{
                    //    if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                    //    {
                    //        chk = (CheckBox)PartDetailsGrid_Part.Rows[iRowCnt].FindControl("ChkPart");
                    //        chk.Checked = true;
                    //        break;
                    //    }
                    //}
                }
            }
        }

        private string GetIdFromString(string strArr)
        {
            string sID = "";
            char[] strToParse = strArr.ToCharArray();
            // convert string to array of chars    
            char ch; int charpos = 0;
            ch = strToParse[charpos];
            while (ch != ' ')
            {
                sID = sID + ch;
                charpos++;
                ch = strToParse[charpos];

            }
            return sID;
        }
        #endregion

        #region Select Labour PopUP
        DataSet dsSrchgrid_Labour;
        private string sDealerId;
        private string sSelectedLabourID = "";
        private string sModelGroupID = "41";
        private int iModBasCatID = 0;
        private int iEstmtId = 0;
        private string sWarr = "N";
        private string sJobtype = "";
        //sujata 25012011
        int iTotalCnt_Labor = 0;
        //Sujata 25012011
        protected void btnSave_Labour_Click(object sender, EventArgs e)
        {
            try
            {
                PagerV2_2.ItemCount = 10;
                PagerV2_2.CurrentIndex = 0;
                bindGrid_SelectLabour(Func.Convert.sConvertToString(DdlSelctionCriteria_Labour.SelectedValue), Func.Convert.sConvertToString(txtSearch_Labour.Text));
                LabourDetailsGrid_Labour.DataBind();
                mpeSelectLabour.Show();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void btnBack_Labour_Click(object sender, EventArgs e)
        {
            try
            {
                int idataCount = 0;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();

                dtDetails = (DataTable)Session["LabourDetails"];
                idataCount = dtDetails.Rows.Count;

                dtTemp = dtDetails.Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    dtDetails.Columns.Clear();
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("labour_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Lab_Tag", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Tax", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax1", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax2", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("EstDtlID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("out_lab_desc", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("AddLbrDescriptionID", typeof(int)));
                }

                string sPartID = sSelectedLabourID;
                if (Func.Convert.sConvertToString(txtPartIds_Labour.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds_Labour.Text);
                    string[] asd = myString.ToString().Split('#');
                    int iRowCnt = 0;

                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        //if (dtDetails.Rows.Count == 301) break;
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        dr = dtDetails.NewRow();
                        //dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                        sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();
                        dr["labour_code"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["part_type_tag"] = "L";

                        dr["ManHrs"] = Func.Convert.dConvertToDouble(myArray[3]);
                        dr["Rate"] = Func.Convert.dConvertToDouble(myArray[4]);
                        dr["Total"] = Func.Convert.dConvertToDouble(myArray[5]);
                        dr["Lab_Tag"] = Func.Convert.sConvertToString(myArray[6]);

                        dr["Job_Code_ID"] = Func.Convert.iConvertToInt(0);
                        dr["Status"] = "S";
                        dr["group_code"] = "L";

                        dr["Tax"] = Func.Convert.iConvertToInt(myArray[8]);
                        dr["Tax1"] = Func.Convert.iConvertToInt(myArray[9]);
                        dr["Tax2"] = Func.Convert.iConvertToInt(myArray[10]);
                        dr["EstDtlID"] = Func.Convert.iConvertToInt(myArray[11]);
                        dr["AddLbrDescriptionID"] = Func.Convert.iConvertToInt(myArray[12]);
                        dr["out_lab_desc"] = Func.Convert.sConvertToString(myArray[13]);

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }
                Session["LabourDetails"] = dtDetails;

                //Add Labour Timing
                int itimedataCount = 0;
                DataTable dttimeDetails = new DataTable();
                DataTable dttimeTemp = new DataTable();

                dttimeDetails = (DataTable)Session["LabourTimeDetails"];
                if (dttimeDetails != null)
                {
                    itimedataCount = dttimeDetails.Rows.Count;

                    dttimeTemp = dttimeDetails.Clone();
                }
                DataRow drTime;

                if (itimedataCount == 0)
                {
                    if (dttimeDetails != null) dttimeDetails.Columns.Clear();
                    if (dttimeDetails == null) dttimeDetails = new DataTable();

                    dttimeDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dttimeDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                    dttimeDetails.Columns.Add(new DataColumn("labour_code", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("Lab_Tag", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("StartTime", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("PauseReason", typeof(int)));
                    dttimeDetails.Columns.Add(new DataColumn("PauseTime", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("EndTime", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("SRNo", typeof(string)));
                    dttimeDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                }

                sPartID = sSelectedLabourID;
                if (Func.Convert.sConvertToString(txtPartIds_Labour.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds_Labour.Text);
                    string[] asd = myString.ToString().Split('#');
                    int iRowCnt = 0;

                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        if (dttimeDetails.Rows.Count == 301) break;
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        drTime = dttimeDetails.NewRow();
                        //dr["SRNo"] = iRowCnt;
                        drTime["ID"] = 0;
                        drTime["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                        sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();
                        drTime["labour_code"] = Func.Convert.sConvertToString(myArray[1]);
                        drTime["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                        drTime["Lab_Tag"] = Func.Convert.sConvertToString(myArray[6]);

                        drTime["PauseReason"] = Func.Convert.iConvertToInt(0);
                        drTime["StartTime"] = "";
                        drTime["PauseTime"] = "";
                        drTime["EndTime"] = "";
                        drTime["SRNo"] = "1";
                        drTime["Status"] = "S";

                        dttimeDetails.Rows.Add(drTime);
                        dttimeDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }
                Session["LabourTimeDetails"] = dttimeDetails;
                // New Code Vikram
                bFillDetailsFromLabourGrid();
                BindDataToLaborGrid(true, 0);

                dtJbTaxDetails = (DataTable)Session["JbTaxDetails"];
                CreateNewRowToTaxGroupDetailsTable();
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbTaxDetails"] = dtJbTaxDetails;
                dtInvJobDescDet = (DataTable)Session["InvJobDescDet"];
                BindDataToGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void bindGrid_SelectLabour(string sSelect, string sSearchPart)
        {

            clsDB objDB = new clsDB();
            try
            {
                DrpSelFrom.Visible = false;
                DataSet dsSrchgrid;
                int iDealerId;
                string sSelectedLabourID, sSelectedEstLabourID, sSelectedWarrLabourID, sSelectedPaidLabourID, sSelectedLabourIDSent;
                string sModelGroupID, sLab_Tag;
                int iLabGrpID;
                string sCustTaxTag = "";
                string sDocGST = "";
                string RepairOrderDate = "";
                //sDealerId = Request.QueryString["DealerID"].ToString();
                //sModelGroupID = Request.QueryString["ModelGroupID"].ToString();
                //sLab_Tag = DrpLabourSelect.SelectedValue;
                //sSelectedLabourID = Request.QueryString["SelectedLabourID"].ToString();
                //sCustTaxTag = Request.QueryString["CustTaxTag"].ToString();
                //sJobtype = Request.QueryString["Jobtype"].ToString();

                sDealerId = Func.Convert.sConvertToString(Location.iDealerId);
                sModelGroupID = "1";
                sLab_Tag = DrpLabourSelect.SelectedValue;
                sSelectedLabourID = hdnSelectedLabourID.Value;
                sCustTaxTag = hdnCustTaxTag.Value;
                RepairOrderDate = txtDocDate.Text;
                sJobtype = "0";
                sDocGST = Func.Convert.sConvertToString(hdnISDocGST.Value);
                sSelectedEstLabourID = ""; sSelectedWarrLabourID = ""; sSelectedPaidLabourID = ""; sSelectedLabourIDSent = "";

                iLabGrpID = 0;
                iLabGrpID = Func.Convert.iConvertToInt(DrpLabGrp.SelectedValue);
                if (Func.Convert.sConvertToString(sSelectedLabourID) != "")
                {
                    string myString = Func.Convert.sConvertToString(sSelectedLabourID);
                    string[] asd = myString.ToString().Split(',');

                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {

                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        //if (Func.Convert.sConvertToString(myArray[2]) != "" && Func.Convert.sConvertToString(myArray[2]) != "0" && sLab_Tag =="E")
                        if (Func.Convert.sConvertToString(myArray[2]) != "" && Func.Convert.sConvertToString(myArray[2]) != "0" && DrpSelFrom.SelectedValue == "E")
                        {
                            sSelectedEstLabourID = sSelectedEstLabourID + ((sSelectedEstLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[2]);
                        }
                        else if (Func.Convert.sConvertToString(myArray[1]) == "C" && sLab_Tag == "C")
                        {
                            sSelectedWarrLabourID = sSelectedWarrLabourID + ((sSelectedWarrLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[0]);
                        }
                        else if (Func.Convert.sConvertToString(myArray[1]) == "D" && sLab_Tag == "D")
                        {
                            sSelectedPaidLabourID = sSelectedPaidLabourID + ((sSelectedPaidLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[0]);
                        }
                    }
                }

                if (PagerV2_2.CurrentIndex == 0)
                {
                    PagerV2_2.CurrentIndex = 1;
                }

                //sSelectedLabourIDSent = (sLab_Tag == "E") ? sSelectedEstLabourID : (sLab_Tag == "C") ? sSelectedWarrLabourID : sSelectedPaidLabourID;
                sSelectedLabourIDSent = (DrpSelFrom_Labour.SelectedValue == "E") ? sSelectedEstLabourID : (sLab_Tag == "C") ? sSelectedWarrLabourID : sSelectedPaidLabourID;
                //We never got sLab_Tag ='E' because we remove "Estimate" list item from DrpLabourSelect. and Add new DrpSelFrom for "from where selection(Master or Estimate)" criteria
                if (DrpSelFrom_Labour.SelectedValue == "E") sSelect = "E" + sSelect;

                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourID, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "");        
                //dsSrchgrid_Labour = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourIDSent, sSearchPart, PagerV2_2.PageSize, PagerV2_2.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "", iLabGrpID, sCustTaxTag, sJobtype,txtModCatIDBasic.Text);
                dsSrchgrid_Labour = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourIDSent, sSearchPart, PagerV2_2.PageSize, PagerV2_2.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "", iLabGrpID, sCustTaxTag, sJobtype, Func.Convert.iConvertToInt(txtModCatIDBasic.Text), sDocGST, RepairOrderDate);

                if (dsSrchgrid_Labour == null)
                {
                    return;
                }
                if (dsSrchgrid_Labour.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt_Labor = Func.Convert.iConvertToInt(dsSrchgrid_Labour.Tables[0].Rows[0][6]);
                    PagerV2_2.ItemCount = iTotalCnt_Labor;
                    LabourDetailsGrid_Labour.DataSource = dsSrchgrid_Labour;
                    LabourDetailsGrid_Labour.DataBind();
                    ChkSelectedParts_Labour();
                }
                else
                {
                    return;
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
        protected void PagerV2_2_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            PagerV2_2.CurrentIndex = currnetPageIndx;
            bindGrid_SelectLabour(Func.Convert.sConvertToString(DdlSelctionCriteria_Labour.SelectedValue), Func.Convert.sConvertToString(txtSearch_Labour.Text));
            mpeSelectLabour.Show();
        }
        private void ChkSelectedParts_Labour()
        {
            CheckBox chk = new CheckBox();
            string sLabID = "";
            string str = null;
            string[] strArr = null;
            string strAllLabor = null;
            string[] strAllLaborArr = null;

            //txtPartIds_Labour.Text = "";
            strAllLabor = txtPartIds_Labour.Text;
            if (strAllLabor == "") return;
            strAllLaborArr = strAllLabor.Split('#');
            for (int i = 0; i < strAllLaborArr.Length; i++)
            {
                //str = txtPartIds_Labour.Text;
                str = Func.Convert.sConvertToString(strAllLaborArr[i]);

                if (str == "") return;
                //strArr = str.Split('#');
                string[] stringSeparators = new string[] { "<--" };

                strArr = str.Split(stringSeparators, StringSplitOptions.None);

                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid_Labour.Rows.Count; iRowCnt++)
                {
                    sLabID = (LabourDetailsGrid_Labour.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                    if (sLabID == Func.Convert.sConvertToString(strArr[0]))
                    {
                        chk = (CheckBox)LabourDetailsGrid_Labour.Rows[iRowCnt].FindControl("ChkLabour");
                        chk.Checked = true;
                        break;
                    }
                    //for (int j = 0; j < strArr.Length; j++)
                    //{
                    //    if (sLabID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                    //    {
                    //        chk = (CheckBox)LabourDetailsGrid_Labour.Rows[iRowCnt].FindControl("ChkLabour");
                    //        chk.Checked = true;
                    //        break;
                    //    }
                    //}
                }
            }
        }
        #endregion

        

        

    }
}