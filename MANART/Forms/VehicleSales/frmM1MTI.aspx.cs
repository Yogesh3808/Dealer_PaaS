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

namespace MANART.Forms.VehicleSales
{
    public partial class frmM1MTI : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
        private DataTable dtClosureDetails = new DataTable();
        private int iDealerID = 0;
        private DataSet modelcat;
        private int iID;
        private bool bDetailsRecordExist = false;
        string sMessage = "";
        string DealerOrigin = "";
        string DOrigin = "";
        private DataSet DSDealer;
        private DataSet dsState;
        private string sControlClientID = "";
        int iUserId = 0;
        int iHOBrId = 0;
        int iInqID = 0;
        int iMenuId = 0;
        private int iDealerId = 0;
        private int HOBrID = 0;
        int iPrimaryApplicationID = 0;
        int iM0PriAppID = 0;
        int sStage = 1;
        string DealerCode = "";
        string sNew = "N";
        Boolean bSaveRecord = false;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                ToolbarC.bUseImgOrButton = true;



                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);

                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }

                //ToolbarC.iValidationIdForSave = 65;

              
                PDoc.sFormID = "65";
                Location.bUseSpareDealerCode = false;

                if (!IsPostBack)
                {
                    FillCombo();
                    Session["LeadObjective"] = null;
                    Session["LeadFleetDtls"] = null;
                    Session["LeadClosure"] = null;
                    //Session["LeadQuotDtls"] = null;

                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "M1 Detials";
                FillSelectionGrid();

                if (iID != 0)
                {
                    GetDataAndDisplay();
                }

                if (txtID.Text == "")
                {
                    //ToolbarC.sSetMessage(MANART.WebParts_Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmPrint, false);

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }



                //ToolbarC.iValidationIdForSave = 62;
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

                //lblTitle.Text = "M1 Details";
                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
                Location.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);

                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                    //DisplayPreviousRecord();

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

        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {

            //FillCombo();
            //FillSelectionGrid();
            //PSelectionGrid.Style.Add("display", "");
            //ClearDealerHeader();

            iHOBrId = 0;
            iDealerId = Location.iDealerId;
            if (iHOBrId == 0)
            {
                FindHOBr(iDealerId);
            }
            FillCombo();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            PDoc.sDealerID = Func.Convert.sConvertToString(Location.iDealerId);
            string sDealerID = PDoc.sDealerID;
            PDoc.BindDataToGrid();
            ClearDealerHeader();
            DisplayPreviousRecord();

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

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;



                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;
                    ClosureGrid.Enabled = false;
                    if (bFillDetailsFromGrid(true) == false) return;
                    bFillFleetFromGrid();
                    //if (bDetailsRecordExist == false)
                    //{
                    //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    //    return;
                    //}

                    //bFillQuotFromGrid();


                    if (FunValidation() == false)
                    {
                        return;
                    }


                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {

                             //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                          Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M1 ") + "','" + Server.HtmlEncode(txtLeadNo.Text) + "');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }

                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm/Close
                {

                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;
                    ClosureGrid.Enabled = false;
                    if (bFillDetailsFromGrid(true) == false) return;
                    bFillFleetFromGrid();
                    if (bDetailsRecordExist == false)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                        return;
                    }

                    //bFillQuotFromGrid();

                    if (FunValidation() == false)
                    {
                        return;
                    }


                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);



                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                           //  Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                          Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("M1 ") + "','" + Server.HtmlEncode(txtLeadNo.Text) + "');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }

                    }


                }

                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;
                    ClosureGrid.Enabled = true;
                    //SetGridControlPropertyClosure(true);
                    if (bFillDetailsFromGrid(true) == false) return;
                    bFillFleetFromGrid();

                    if (bFillDetailsFromGridClosure(true) == false)
                    {

                        ClosureGrid.Enabled = true;
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter Closure details');</script>");
                        return;
                    }

                    GenerateLeadNo("LA");

                    iID = bHeaderSave("N", "N", "Y");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);



                        if (bSaveDetails("N", "N", "Y") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Lost');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }

                    }



                }



                FillSelectionGrid();
                GetDataAndDisplay();
                string sdealerid = Func.Convert.sConvertToString(iDealerId);
                PDoc.sDealerID = sdealerid;
                PDoc.BindDataToGrid();
            }



            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }





        private void ClearDealerHeader()
        {
            drpInqSource.SelectedValue = "0";
            drpArea.SelectedValue = "0";
            drpAttendedby.SelectedValue = "0";
            drpAlloatedTo.SelectedValue = "0";
            //drpLeadName.SelectedValue = "0";
            //

            txtLeadNo.Text = "";
            txtDocDate.Text = "";
            txtSourceName.Text = "";
            txtSourceMob.Text = "";
            txtSourceAdd.Text = "";
            //drpDistrict.SelectedItem.Text = "0";

        }



        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpArea, clsCommon.ComboQueryType.LeadArea, iDealerId, " and HOBrID=" + iDealerId);
            Func.Common.BindDataToCombo(drpAttendedby, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId);
            Func.Common.BindDataToCombo(drpAlloatedTo, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId + " and Empl_Type=11");
            Func.Common.BindDataToCombo(drpInqSource, clsCommon.ComboQueryType.InqSource, 0);
            //Func.Common.BindDataToCombo(drpLeadName, clsCommon.ComboQueryType.ProsName, iDealerId, " and HOBr_ID=" + iHOBrId);
            Func.Common.BindDataToCombo(drpLikelyBuyBrand, clsCommon.ComboQueryType.Competitor, 0);

            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);
            //Func.Common.BindDataToCombo(drpFinance, clsCommon.ComboQueryType.Financier, 0);

            Func.Common.BindDataToCombo(drpCustType, clsCommon.ComboQueryType.CustomerType, 0);
            Func.Common.BindDataToCombo(drpDriveType, clsCommon.ComboQueryType.DriveType, 0);
            Func.Common.BindDataToCombo(drpFinancierType, clsCommon.ComboQueryType.Financier, 0);
            Func.Common.BindDataToCombo(drpIndustryType, clsCommon.ComboQueryType.IndustryType, 0);
            Func.Common.BindDataToCombo(drpLoadType, clsCommon.ComboQueryType.LoadType, 0);
            Func.Common.BindDataToCombo(drpPrimaryApplication, clsCommon.ComboQueryType.PrimaryApplication, 0);
            iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);

            Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
            Func.Common.BindDataToCombo(drpRoadType, clsCommon.ComboQueryType.RoadType, 0);
            Func.Common.BindDataToCombo(drpRouteType, clsCommon.ComboQueryType.RouteType, 0);
            //Func.Common.BindDataToCombo(drpInqstage, clsCommon.ComboQueryType.Platform, 0);
            //Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0);

            Func.Common.BindDataToCombo(drpM0PriApp, clsCommon.ComboQueryType.PrimaryApplication, 0);
            iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);

            Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);

            Func.Common.BindDataToCombo(drpPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            Func.Common.BindDataToCombo(drpM0Financier, clsCommon.ComboQueryType.Financier, 0);


            drpModelGroup.SelectedValue = "1";
        }


        protected void drpModelGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillModel();

        }



        protected void drpModelCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModel.SelectedValue = drpModelCode.SelectedValue;

            //PQuotation.Visible = true;
        }


        protected void drpModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModelCode.SelectedValue = drpModel.SelectedValue;
            //PQuotation.Visible = true;

        }





        protected void drpPrimaryApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
            Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);

        }

        protected void drpM0PriApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);
            Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);

        }

        protected void drpCloseRsn_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpClosure = (DropDownList)gvr.FindControl("drpCloseRsn");
            DropDownList drpCompClosure = (DropDownList)gvr.FindControl("drpCloseCompetitor");
            TextBox txtCompetitorClosure = (TextBox)gvr.FindControl("txtCompetitor");
            TextBox txtCompQty = (TextBox)gvr.FindControl("txtCompQty");
            TextBox txtClosureID = (TextBox)gvr.FindControl("txtClosureID");
            int iClosureValue = Func.Convert.iConvertToInt(drpClosure.SelectedValue);
            if (iClosureValue == 1)
            {
                drpCompClosure.Enabled = true;
                txtCompetitorClosure.Enabled = true;
                txtCompQty.Text = txtQty.Text;
                txtCompQty.Enabled = false;
            }
            else
            {
                drpCompClosure.Enabled = false;
                txtCompetitorClosure.Enabled = false;
                txtCompQty.Text = txtQty.Text;
                txtCompQty.Enabled = false;
                
                drpCompClosure.SelectedIndex = 0;
                txtCompetitorClosure.Text = "";
               
                

            }

        }



        private void GenerateLeadNo(string Type)
        {
            // 'Replace Func.DB to objDB by Shyamal on 26032012

            MANART_DAL.clsDB objDB = new MANART_DAL.clsDB();
            try
            {
                DataSet dsDCode = new DataSet();

                if (iDealerId == 99999)
                {
                    DealerCode = "D099999";
                }
                else
                {
                    dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerId);

                    if (dsDCode.Tables[0].Rows.Count > 0)
                    {
                        DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }


            if (Type == "I")
            {
                txtLeadNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "I"));

            }
            else if (Type == "EN")
            {
                txtEnquiryNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "EN"));

            }
            else if (Type == "LA")
            {
                txtAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "LA"));

            }




        }

        public string FindMAxLeadNo(string VDealerCode, int iDealerID, string Type)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;
                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";
                if (Type == "EN")
                {
                    sDocName = "EN";
                }
                else if (Type == "I")
                {
                    sDocName = "M1";
                }
                else if (Type == "LA")
                {
                    sDocName = "LA";
                }



                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(5, '0');

                if (VDealerCode != "")
                {
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
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


        private void DisplayPreviousRecord()
        {
            try
            {

                DataSet ds = new DataSet();

                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);

                ds = GetM1(iID, "New", iDealerId, iDealerId, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            //ds.Tables[0].Rows[0]["PO_Cancel"] = "N";
                            //ds.Tables[0].Rows[0]["PO_Confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                else
                {
                    BindDataToGrid(true, 0);
                }
                //txtID.Text = "";
                //txtDocDate.Text = Func.Common.sGetCurrentDate(HOBrID, false);
                //txtLikelydate.Text = Func.Common.sGetCurrentDate(HOBrID, false);

                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();


                ds = GetM1(iID, "Max", iDealerId, iDealerId, iID);
                if (ds == null) // if no Data Exist
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
                }
                else
                {
                    DisplayData(ds);


                }
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        public DataSet GetM1(int POId, string POType, int DealerID, int HOBrID, int iM0ID) //change for REMAN PO
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM1", POId, POType, DealerID, HOBrID, iM0ID);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        protected void drpLocalMech_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpLocalMech.SelectedValue != "1")
            {
                txtMechName.Enabled = false;
                txtMechAdd.Enabled = false;
                txtMechPhone.Enabled = false;
                txtMechName.Text = "";
                txtMechAdd.Text = "";
                txtMechPhone.Text = "";
            }

            else
            {
                txtMechName.Enabled = true;
                txtMechAdd.Enabled = true;
                txtMechPhone.Enabled = true;
            }




        }


        protected void drpSpecialPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpSpecialPackage.SelectedValue != "1")
            {
                txtAMCDet.Enabled = false;
                txtSpWarrDet.Enabled = false;
                txtExWarrDet.Enabled = false;
                txtOthersDet.Enabled = false;
                txtSpWarrDet.Text = "";
                txtExWarrDet.Text = "";
                txtAMCDet.Text = "";
                txtOthersDet.Text = "";
            }

            else
            {
                txtAMCDet.Enabled = true;
                txtSpWarrDet.Enabled = true;
                txtExWarrDet.Enabled = true;
                txtOthersDet.Enabled = true;
            }




        }
        private bool FunValidation()
        {
            bool bFunValidation = true;

            if (txtDocDate.Text == "" || txtDocDate.Text == "01/01/1900")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M1 Date');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            if (drpInqSource.SelectedValue == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Enquiry Source');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            if (iDealerId != 99999)
            {
                if (drpArea.SelectedValue == "0")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Customer Area');</script>");
                    bFunValidation = false;
                    return bFunValidation;
                }
                if (drpAttendedby.SelectedValue == "0")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Attended By');</script>");
                    bFunValidation = false;
                    return bFunValidation;
                }
                if (drpAlloatedTo.SelectedValue == "0")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Allocated To');</script>");
                    bFunValidation = false;
                    return bFunValidation;
                }
            }

            if (drpPOType.SelectedValue == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select PO Type');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            if (drpModelCat.SelectedValue == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Interested Purchase Category');</script>");
                bFunValidation = false;
                return bFunValidation;
            }
            //if (drpModelGroup.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Model Group');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}

            if (drpModelCode.SelectedValue == "0" || drpModel.SelectedValue == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Model');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            if (txtQty.Text == "" || txtQty.Text == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Quantity');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            //if (drpCustType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Customer Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}
            //if (drpIndustryType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Industry Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}

            //if (drpDriveType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Drive Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}
            //if (drpLoadType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Load Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}

            //if (drpPrimaryApplication.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Primary Application');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}
            //if (drpSeconadryApplication.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Secondary application');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}
            //if (drpRoadType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Road Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}

            //if (drpRouteType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Route Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}
            //if (drpFinancierType.SelectedValue == "0")
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Financier Type');</script>");
            //    bFunValidation = false;
            //    return bFunValidation;
            //}

            if (txtLikelydate.Text == "" || txtLikelydate.Text == "01/01/1900")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Likely Buy Date');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            if (drpLikelyBuyBrand.SelectedValue == "0")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Likely Buy Brand');</script>");
                bFunValidation = false;
                return bFunValidation;
            }

            return bFunValidation;

        }
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "M1 (Enquiry Generated) List";
                SearchGrid.AddToSearchCombo("M1 No");
                SearchGrid.AddToSearchCombo("M1 Date");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("M1 Status");
                if (iDealerId == 0)
                {
                    if (PDoc.iDealerID != 0)
                    {
                        SearchGrid.iDealerID = PDoc.iDealerID;
                        iDealerId = PDoc.iDealerID;


                    }
                    else if (Location.iDealerId != 0)
                    {
                        SearchGrid.iDealerID = Location.iDealerId;
                        iDealerId = Location.iDealerId;
                    }
                    if (iHOBrId == 0)
                    {
                        FindHOBr(iDealerId);
                    }
                }
                //SearchGrid.iDealerID = Func.Convert.iConvertToInt(txtDealerLoc.Text);
                //SearchGrid.iDealerID = Location.iDealerId;
                SearchGrid.iDealerID = iDealerId;
                SearchGrid.iBrHODealerID = iDealerId;


                SearchGrid.sSqlFor = "M1DetailsMTI";
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


        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();


                int iID = Func.Convert.iConvertToInt(txtID.Text);
                int iM0ID = Func.Convert.iConvertToInt(txtM0ID.Text);
                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetM1(iID, "All", iDealerId, iDealerId, iM0ID);
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
                    ds = GetM1(iID, "Max", iDealerId, iDealerId, iM0ID);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
                }
                ds = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }




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



                    //M0
                    txtM0ID.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["ID"]);
                    txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Cust_ID"]);
                    FillLeadType();
                    drpM0CustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["type_flag_id"]);
                    FillTitle();
                    drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Prefix"]);
                    txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["name"]);
                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Add2"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["city"]);
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["pin"]);
                    FillStateCountry();
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Region_ID"]);
                    Filldistrict();
                    drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["District_ID"]);
                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["mobile"]);

                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["E_mail"]);
                    txtM0.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Tr_Num"]);
                    txtM0Date.Text = Func.Convert.tConvertToDate(ds.Tables[5].Rows[0]["M0_Date"], false);

                    //drpVisitObj.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Obj_Id"]);

                    drpM0PriApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["M0_PriApp"]);

                    iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);
                    Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);


                    drpM0SecApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["M0_SecApp"]);

                    string IsMTICust = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Is_MTICust"]);
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
                    drpM0Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["M0_Financier"]);
                    txtBodyBuilder.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["BodyBuilder"]);
                    //txtNextDate.Text = Func.Convert.tConvertToDate(ds.Tables[5].Rows[0]["Next_date"], false);

                    //M1

                    //Header
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtLeadNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_inq_no"]);
                    txtEnquiryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EnquiryNo"]);
                    txtDocDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Lead_Date"], false);
                    txtDoneBy.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DoneBy"]);
                    drpInqSource.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["inq_source_Id"]);
                    txtSourceName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Source_name"]);
                    txtSourceMob.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Source_Ph_no"]);
                    txtSourceAdd.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Source_Address"]);
                    drpArea.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["area"]);

                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AttendedBy"]) != "0")
                    {

                        Func.Common.BindDataToCombo(drpAttendedby, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId +
                            ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["AttendedBy"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AttendedBy"]) : ""));
                    }

                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AllocatedTo"]) != "0")
                    {
                        Func.Common.BindDataToCombo(drpAlloatedTo, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId + " and Empl_Type=11 "
                            + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["AllocatedTo"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AllocatedTo"]) : ""));
                    }


                    
                    drpAttendedby.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AttendedBy"]);
                    drpAlloatedTo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AllocatedTo"]);


                    drpPOType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]);
                    txtAMCDet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AmcDet"]);
                    txtSpWarrDet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SpeWarrDet"]);
                    txtExWarrDet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ExWarrDet"]);

                    string SpPackage = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SP_Packages"]);
                    if (SpPackage == "Y")
                    {
                        drpSpecialPackage.SelectedValue = "1";
                    }
                    else
                    {
                        drpSpecialPackage.SelectedValue = "2";
                    }

                    txtOthersDet.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["OtherDet"]);


                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_gp"]);
                    drpModelGroup.SelectedValue = "1";
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mod_Cat_ID"]);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]) != "N")
                    {
                        hdnGSTDoc.Value = "Y";
                    }
                    else
                    {
                        hdnGSTDoc.Value = "N";
                    }
                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);


                    //profile details

                    //from master

                    drpCustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Cust_Type_ID"]);
                    drpDriveType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Drive_ID"]);
                    drpFinancierType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Finac_Type_ID"]);
                    drpIndustryType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Industry_type_ID"]);
                    drpLoadType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Load_type_ID"]);
                    drpRoadType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Road_ID"]);
                    drpRouteType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Route_ID"]);

                    //from m1 table
                    drpPrimaryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pri_app_code"]);

                    iPrimaryApplicationID = Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue);
                    Func.Common.BindDataToCombo(drpSeconadryApplication, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iPrimaryApplicationID);
                    drpSeconadryApplication.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["sec_app_code"]);

                    txtLikelydate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Likely_buy_dt"], false);

                    //Additional Info
                    drpLikelyBuyBrand.SelectedValue = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Likey_buy_brand_ID"]);


                    string LocalPermit = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Load_Permit_Available"]);
                    if (LocalPermit == "Y")
                    {
                        drpLoadPermit.SelectedValue = "1";
                    }
                    else
                    {
                        drpLoadPermit.SelectedValue = "2";
                    }

                    string TieUpBodyBuilder = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Tie_Body_Builder"]);
                    if (TieUpBodyBuilder == "Y")
                    {
                        drpBodyBuilder.SelectedValue = "1";
                    }
                    else
                    {
                        drpBodyBuilder.SelectedValue = "2";
                    }

                    string LocalMech = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Tie_Local_MEch"]);
                    if (LocalMech == "Y")
                    {
                        drpLocalMech.SelectedValue = "1";
                    }
                    else
                    {
                        drpLocalMech.SelectedValue = "2";
                    }

                    txtMechName.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Mech_Name"]);
                    txtMechAdd.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Mech_Address"]);
                    txtMechPhone.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Mech_PH_No"]);


                    hdnMinFPDADate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MinDate"]);



                }


                else
                {
                    ClearDealerHeader();
                }

                //if (ds == null || ds.Tables[1].Rows.Count == 0)
                //{
                //    ClearDealerHeader();
                //    return;
                //}

                //if (ds.Tables[1].Rows.Count > 0)
                //{ 

                //}

                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["inq_confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["inq_cancel"]);
                hdnLost.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M1Lost"]);

                //hdnHold.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_Hold"]);

                Session["LeadObjective"] = null;
                dtDetails = ds.Tables[1];
                Session["LeadObjective"] = dtDetails;
                BindDataToGrid(bRecordIsOpen, 0);

                Session["LeadClosure"] = null;
                dtClosureDetails = ds.Tables[2];
                Session["LeadClosure"] = dtClosureDetails;
                BindDataToGridClosure(bRecordIsOpen, 0);

                Session["LeadFleetDtls"] = null;
                dtFleetDetails = ds.Tables[3];
                Session["LeadFleetDtls"] = dtFleetDetails;
                BindDataToGridFleet();


                //Session["LeadQuotDtls"] = null;
                //dtQuotDetails = ds.Tables[4];
                //Session["LeadQuotDtls"] = dtQuotDetails;
                //BindDataToGridQuot();




                // If Record is Confirm or cancel then it is not editable            
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
                if (hdnLost.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                }




                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true, "Nothing");
                }
                else if (hdnCancle.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Cancel");
                }
                else if (hdnConfirm.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Confirm");
                }
                else if (hdnLost.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Lost");
                }

                //bConvertToInq.Enabled = false;
                //bHold.Enabled = false;

                if (Func.Convert.iConvertToInt(txtID.Text) != 0 && (bEnableControls == true))
                {

                    if (Func.Convert.iConvertToInt(txtInqID.Text) != 0)
                    {
                        //bConvertToInq.Enabled = false;
                        //bHold.Enabled = false;
                        //MakeEnableDisableControls(false, "Nothing");
                    }
                    else
                    {
                        //bConvertToInq.Enabled = true;
                        //bHold.Enabled = true;
                    }

                }
                else if (hdnCancle.Value == "Y")
                {
                    //bConvertToInq.Enabled = false;
                    //bHold.Enabled = false;
                }
                else if (hdnConfirm.Value == "Y")
                {
                    //bConvertToInq.Enabled = false;
                    //bHold.Enabled = false;
                }




            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }


        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillDetailsFromGrid(false);
                BindDataToGrid(true, 1);
            }
        }



        protected void ClosureGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillDetailsFromGridClosure(false);
                BindDataToGridClosure(true, 1);
            }
        }

        protected void FleetGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillFleetFromGrid();
                BindDataToGridFleet();
            }
        }


        private void BindDataToGridClosure(bool bRecordIsOpen, int iNoRowToAdd)
        {
            if (bRecordIsOpen == true)
            {
                CreateNewRowToDetailsTableClosure(iNoRowToAdd);
            }
            else
            {
                ClosureGrid.DataSource = dtClosureDetails;
                ClosureGrid.DataBind();
            }
            SetGridControlPropertyClosure(bRecordIsOpen);
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

        private bool bFillFleetFromGrid()
        {
            dtFleetDetails = (DataTable)Session["LeadFleetDtls"];

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

            }
            bDetailsRecordExist = true;
            return true;
        }







        private void SetGridControlPropertyClosure(bool bRecordIsOpen)
        {
            string sDeleteStatus = "";
            string sDealerId = Func.Convert.sConvertToString(iDealerId);

            int idtRowCnt = 0;


            for (int iRowCnt = 0; iRowCnt < ClosureGrid.Rows.Count; iRowCnt++)
            {

                TextBox txtClosureID = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtClosureID");

                DropDownList drpCloseRsn = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseRsn");
                Func.Common.BindDataToCombo(drpCloseRsn, clsCommon.ComboQueryType.InqClose, 0);



                DropDownList drpCloseCompetitor = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseCompetitor");
                Func.Common.BindDataToCombo(drpCloseCompetitor, clsCommon.ComboQueryType.Competitor, 0);




                TextBox txtCompetitor = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompetitor");

                TextBox txtCompQty = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompQty");

                txtCompQty.Text = txtQty.Text;

                sDeleteStatus = "E";
                if (idtRowCnt < dtClosureDetails.Rows.Count)
                {
                    txtClosureID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(dtClosureDetails.Rows[iRowCnt]["ID"]));
                    drpCloseRsn.SelectedValue = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["rsn_Id"]);
                    drpCloseCompetitor.SelectedValue = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["CompID"]);
                    txtCompetitor.Text = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["CompetitorMake"]);
                    txtCompQty.Text = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["Qty"]);


                    sDeleteStatus = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["Status"]);
                    idtRowCnt = idtRowCnt + 1;
                }

                //New 
                LinkButton lnkNew = (LinkButton)ClosureGrid.Rows[iRowCnt].FindControl("lnkNew");
                lnkNew.Style.Add("display", "none");



                //Delete 
                CheckBox Chk = (CheckBox)ClosureGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                Chk.Attributes.Add("onClick", "SelectDeletCheckbox(this)");
                Chk.Style.Add("display", "none");

                // N :- New , D:- Dellete, E:- Exissting            
                if (sDeleteStatus == "D")
                {
                    Chk.Style.Add("display", "");
                    Chk.Checked = true;
                    //ClosureGrid.Rows[iRowCnt].BackColor = Color.Orange;
                }
                else if (sDeleteStatus == "E")
                {
                    lnkNew.Style.Add("display", "none");
                    Chk.Style.Add("display", "");
                    Chk.Checked = false;
                }

                // Allow New To Last Row
                if ((iRowCnt + 1) == ClosureGrid.Rows.Count)
                {
                    lnkNew.Style.Add("display", "");
                }

            }



        }

        private void SetGridControlProperty(bool bRecordIsOpen)
        {
            string sDeleteStatus = "";
            string sDealerId = Func.Convert.sConvertToString(iDealerId);
            //string sModelID = "0";
            int idtRowCnt = 0;



            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                TextBox txtObjID = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtObjID");

                DropDownList drpVisitObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpVisitObj");
                Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M1'");



                //Get Date
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate")) as ASP.webparts_currentdate_ascx).Enabled = true;
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Enabled =true;
                (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = "";


                //Get discussion
                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");

                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");
                Func.Common.BindDataToCombo(drpNextObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M1'");


                //TextBox txtNextObjDate = 
                (DetailsGrid.Rows[iRowCnt].FindControl("txtNextObjDate") as MANART.WebParts.CurrentDate).Enabled = true;//Text = "";
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

                DataRow dr;
                DataTable dtDefaultModel = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxLeadObjRowCount = 1;
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



                        dtDefaultModel.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("obj_Id", typeof(int)));

                        dtDefaultModel.Columns.Add(new DataColumn("obj_date", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("discussion", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("time_spent", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("next_obj_Id", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("next_date", typeof(string)));

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


        private void CreateNewRowToDetailsTableClosure(int iNoRowToAdd)
        {
            try
            {
                //MaxRFPModelRowCount
                DataRow dr;
                DataTable dtDefaultModel = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxLeadObjRowCount = 1;
                //iMaxRFPModelRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxRFPModelRowCount"]);

                if (Session["LeadClosure"] != null)
                {
                    dtDefaultModel = (DataTable)Session["LeadClosure"];
                }
                else
                {
                    dtDefaultModel = dtClosureDetails;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultModel.Rows.Count == 0)
                    {
                        dtDefaultModel.Columns.Clear();


                        //dtDefaultModel.Columns.Add(new DataColumn("SRNo", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("rsn_Id", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("CompID", typeof(int)));
                        dtDefaultModel.Columns.Add(new DataColumn("CompetitorMake", typeof(string)));
                        dtDefaultModel.Columns.Add(new DataColumn("Qty", typeof(string)));





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
                    dr["rsn_Id"] = 0;
                    dr["CompID"] = 0;
                    dr["CompetitorMake"] = "";
                    dr["Qty"] = 0;

                    dr["Status"] = "";
                    dtDefaultModel.Rows.Add(dr);
                    dtDefaultModel.AcceptChanges();

                }
            Bind: ;
                Session["LeadClosure"] = dtDefaultModel;
                ClosureGrid.DataSource = dtDefaultModel;
                ClosureGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

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



        private bool bFillDetailsFromGridClosure(bool bDisplayMsg)
        {
            string sStatus = "";
            dtClosureDetails = (DataTable)Session["LeadClosure"];
            int iCntForDelete = 0;
            int iModelBodyTypeID = 0;
            bDetailsRecordExist = false;
            int iModelID = 0;
            int iCntForSelect = 0;


            for (int iRowCnt = 0; iRowCnt < ClosureGrid.Rows.Count; iRowCnt++)
            {
                TextBox txtClosureID = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtClosureID");
                dtClosureDetails.Rows[iRowCnt]["ID"] = Func.Convert.iConvertToInt(txtClosureID.Text);

                DropDownList drpCloseRsn = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseRsn");

                dtClosureDetails.Rows[iRowCnt]["rsn_Id"] = Func.Convert.iConvertToInt(drpCloseRsn.SelectedValue);

                if (Func.Convert.iConvertToInt(drpCloseRsn.SelectedValue) != 0)
                {
                    iCntForSelect = iCntForSelect + 1;
                }

                DropDownList drpCloseCompetitor = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseCompetitor");

                dtClosureDetails.Rows[iRowCnt]["CompID"] = Func.Convert.iConvertToInt(drpCloseCompetitor.SelectedValue);



                TextBox txtCompetitor = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompetitor");
                dtClosureDetails.Rows[iRowCnt]["CompetitorMake"] = Func.Convert.sConvertToString(txtCompetitor.Text);

                TextBox txtCompQty = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompQty");
                dtClosureDetails.Rows[iRowCnt]["Qty"] = Func.Convert.sConvertToString(txtCompQty.Text);



                CheckBox Chk = (CheckBox)ClosureGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                dtClosureDetails.Rows[iRowCnt]["Status"] = "";
                if (Chk.Checked == true)
                {
                    dtClosureDetails.Rows[iRowCnt]["Status"] = "D";
                    bDetailsRecordExist = true;
                    iCntForDelete++;
                }
                else if (drpCloseRsn.SelectedValue != "0")
                {
                    dtClosureDetails.Rows[iRowCnt]["Status"] = "N";
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



        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            //if (Confirm == "Y" && drpOrderStatus.SelectedValue == "1")
            //{
            //    if (objLead.bSavecustomer(objDB, iDealerId, iHOBrId, iID, txtCustName.Text) == true)
            //    {
            //        bSaveRecord = true;
            //    }
            //    else
            //    {
            //        bSaveRecord = false;
            //    }
            //}


            if (iID == 0)
            {
                GenerateLeadNo("I");
                GenerateLeadNo("EN");
            }

            HdrID = objLead.bSaveM1Header(iID, iDealerId, iDealerId, txtLeadNo.Text, txtDocDate.Text,
                                 Func.Convert.iConvertToInt(drpInqSource.SelectedValue)

                                , Func.Convert.iConvertToInt(drpArea.SelectedValue),
                                Func.Convert.iConvertToInt(drpAttendedby.SelectedValue),
                                 Func.Convert.iConvertToInt(drpAlloatedTo.SelectedValue)
                                 , Func.Convert.iConvertToInt(drpRouteType.SelectedValue)
                                 , Func.Convert.iConvertToInt(drpCustType.SelectedValue)
                                 , Func.Convert.iConvertToInt(drpDriveType.SelectedValue)
                                 , Func.Convert.iConvertToInt(drpLoadType.SelectedValue)
                                 , Func.Convert.iConvertToInt(drpFinancierType.SelectedValue)
                                , Func.Convert.iConvertToInt(drpIndustryType.SelectedValue)
                                , Func.Convert.iConvertToInt(drpRoadType.SelectedValue)
                                , Func.Convert.iConvertToInt(drpPrimaryApplication.SelectedValue)
                                , Func.Convert.iConvertToInt(drpSeconadryApplication.SelectedValue)
                                , txtLikelydate.Text,
                                Func.Convert.iConvertToInt(drpModelGroup.SelectedValue),
                                Func.Convert.iConvertToInt(drpModel.SelectedValue), Cancel, Confirm,
                                 Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
                                 , Func.Convert.dConvertToDouble(txtQty.Text), Func.Convert.iConvertToInt(txtM0ID.Text)
                                 , Func.Convert.iConvertToInt(drpLikelyBuyBrand.SelectedValue)
                                 , txtSourceName.Text, txtSourceAdd.Text, txtSourceMob.Text
                                  , drpLoadPermit.SelectedItem.Text, drpBodyBuilder.SelectedItem.Text
                                , txtMechName.Text, txtMechAdd.Text, txtMechPhone.Text, drpLocalMech.SelectedItem.Text
                                , OrderStatus, Func.Convert.iConvertToInt(drpPOType.SelectedValue),
                                txtAMCDet.Text, txtSpWarrDet.Text, txtExWarrDet.Text, drpSpecialPackage.SelectedItem.Text, txtOthersDet.Text
                                ,iMenuId,txtEnquiryNo.Text,txtAppNo.Text,hdnGSTDoc.Value
                                );



            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();




            if (objLead.bSaveM1Objectives(objDB, iDealerId, iDealerId, dtDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }

            if (objLead.bSaveM1FleetDtls(objDB, iDealerId, iDealerId, dtFleetDetails, Func.Convert.iConvertToInt(txtCustID.Text)) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }

            //if (objLead.bSaveInqQuotationDtls(objDB, iDealerId, iHOBrId, dtQuotDetails, iID) == true)
            //{
            //    bSaveRecord = true;
            //}
            //else
            //{
            //    bSaveRecord = false;
            //}



            if (OrderStatus == "Y")
            {

                if (objLead.bSaveM1ClosureDetails(objDB, iDealerId, iDealerId, dtClosureDetails, iID) == true)
                {
                    bSaveRecord = true;
                }
                else
                {
                    bSaveRecord = false;
                }


            }

            return bSaveRecord;

        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable, string type)
        {


            //M0
            txtM0ID.Enabled = false;
            txtCustID.Enabled = false;

            drpM0CustType.Enabled = false;
            drpTitle.Enabled = false;
            txtCustomerName.Enabled = false;

            txtAddress1.Enabled = false;
            txtAddress2.Enabled = false;
            txtCity.Enabled = false;
            txtpincode.Enabled = false;
            drpState.Enabled = false;
            drpRegion.Enabled = false;
            drpDistrict.Enabled = false;
            txtCountry.Enabled = false;
            txtMobile.Enabled = false;
            txtEmail.Enabled = false;
            txtM0.Enabled = false;
            txtM0Date.Enabled = false;

            drpM0PriApp.Enabled = false;
            drpM0SecApp.Enabled = false;
            drpIsMTICust.Enabled = false;
            drpM0Financier.Enabled = false;

            txtBodyBuilder.Enabled = false;


            //Enable header Controls of Form
            txtDocDate.Enabled = false;
            txtLeadNo.Enabled = bEnable;
            drpInqSource.Enabled = bEnable;
            //drpLeadName.Enabled = bEnable;
            drpArea.Enabled = bEnable;
            drpAttendedby.Enabled = bEnable;
            drpAlloatedTo.Enabled = bEnable;
            drpPOType.Enabled = false;
            txtSourceAdd.Enabled = bEnable;
            txtSourceMob.Enabled = bEnable;
            txtSourceName.Enabled = bEnable;
            txtAMCDet.Enabled = bEnable;
            txtSpWarrDet.Enabled = bEnable;
            txtExWarrDet.Enabled = bEnable;


            DetailsGrid.Enabled = bEnable;

            drpModel.Enabled = bEnable;
            drpModelCode.Enabled = bEnable;
            drpModelCat.Enabled = bEnable;
            drpModelGroup.Enabled = bEnable;
            txtQty.Enabled = bEnable;
            drpSpecialPackage.Enabled = bEnable;

            drpCustType.Enabled = bEnable;
            drpDriveType.Enabled = bEnable;
            drpFinancierType.Enabled = bEnable;
            drpIndustryType.Enabled = bEnable;
            drpLoadType.Enabled = bEnable;
            drpPrimaryApplication.Enabled = bEnable;
            drpRoadType.Enabled = bEnable;
            drpRouteType.Enabled = bEnable;
            drpSeconadryApplication.Enabled = bEnable;

            txtLikelydate.Enabled = bEnable;
            drpLikelyBuyBrand.Enabled = bEnable;
            drpBodyBuilder.Enabled = bEnable;
            drpLocalMech.Enabled = bEnable;

            FleetDtls.Enabled = bEnable;

            drpLoadPermit.Enabled = bEnable;
            if (drpLocalMech.SelectedValue == "1" && bEnable == true)
            {
                txtMechName.Enabled = true;
                txtMechPhone.Enabled = true;
                txtMechAdd.Enabled = true;
            }
            else if (drpLocalMech.SelectedValue != "1" && bEnable == true)
            {

                txtMechName.Enabled = false;
                txtMechPhone.Enabled = false;
                txtMechAdd.Enabled = false;
            }

            else if (drpLocalMech.SelectedValue == "1" && bEnable == false)
            {

                txtMechName.Enabled = false;
                txtMechPhone.Enabled = false;
                txtMechAdd.Enabled = false;
            }



            if (drpSpecialPackage.SelectedValue == "1" && bEnable == true)
            {
                txtAMCDet.Enabled = true;
                txtSpWarrDet.Enabled = true;
                txtExWarrDet.Enabled = true;
                txtOthersDet.Enabled = true;
            }
            else if (drpSpecialPackage.SelectedValue != "1" && bEnable == true)
            {

                txtAMCDet.Enabled = false;
                txtSpWarrDet.Enabled = false;
                txtExWarrDet.Enabled = false;
                txtOthersDet.Enabled = false;
            }

            else if (drpSpecialPackage.SelectedValue == "1" && bEnable == false)
            {

                txtAMCDet.Enabled = false;
                txtSpWarrDet.Enabled = false;
                txtExWarrDet.Enabled = false;
                txtOthersDet.Enabled = false;
            }



            if (type == "Cancel" && bEnable == false)
            {
                ClosureGrid.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Confirm" && bEnable == false)
            {


                ClosureGrid.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Lost" && bEnable == false)
            {


                ClosureGrid.Enabled = true;


            }

            else if (type == "Nothing" && bEnable == true)
            {
                ClosureGrid.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }


            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }



        protected void bConvertToM2(object sender, EventArgs e)
        {

            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                iID = bHeaderSave("N", "Y", "N");


                FillSelectionGrid();
                GetDataAndDisplay();
            }

        }
        private void FillModel()
        {

            int modelgrp = 0;

            modelgrp = Func.Convert.iConvertToInt(drpModelCat.SelectedValue);


            //if (txtHDCode.Text == "")
            //{
            //    Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID not in (1,8) and model_gr_id=" + modelgrp);
            //    Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID not in (1,8) and model_gr_id=" + modelgrp);
            //}
            //else
            //{
            //Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID=" + modelgrp);
            //Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID=" + modelgrp);
            //}

            if (hdnGSTDoc.Value == "Y")
            {
                try
                {
                    clsDB objDB = new clsDB();
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_FillM1ModelHSHCode", modelgrp, "Modelcode");
                    if (dsState != null)
                    {
                        drpModelCode.DataSource = dsState.Tables[0];
                        drpModelCode.DataTextField = "Name";
                        drpModelCode.DataValueField = "ID";
                        drpModelCode.DataBind();
                        drpModelCode.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                    modelcat = objDB.ExecuteStoredProcedureAndGetDataset("SP_FillM1ModelHSHCode", modelgrp, "Modelcat");
                    if (modelcat != null)
                    {
                        drpModel.DataSource = modelcat.Tables[0];
                        drpModel.DataTextField = "Name";
                        drpModel.DataValueField = "ID";
                        drpModel.DataBind();
                        drpModel.Items.Insert(0, new ListItem("--Select--", "0"));
                    }

                }
                catch (Exception)
                {


                }
            }
            else
            {
                Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);
                Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            }
        }


        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {


            Location.iDealerId = PDoc.iDealerID;
            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            Location.FillLocation();

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


                string sDealerID = Func.Convert.sConvertToString(PDoc.iDealerID);
                PDoc.sDealerID = sDealerID;
                PDoc.BindDataToGrid();

                iDealerId = PDoc.iDealerID;
                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }
                FillCombo();


                //clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iM0ID != 0)
                {
                    ds = GetM1(iID, "New", iDealerId, iDealerId, iM0ID);

                    //txtInqNo.Text = "";
                    drpPOType.SelectedValue = "3";

                    DisplayData(ds);
                    //ObjDealer = null;
                }
                else
                {
                    ClearDealerHeader();
                }
                //txtID.Text = "";
                ////txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "VORF", Location.iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, (txtID.Text == "" || txtID.Text == "0") ? false : true);
                //ds = null;
                //ObjProforma = null;

                GenerateLeadNo("I");
                GenerateLeadNo("EN");
                drpPOType.SelectedValue = "3";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FindHOBr(int iDealerId)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetHoBrIDforMTIUsers", iDealerId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                iHOBrId = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                HOBrID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
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
                    drpM0CustType.DataSource = dsCustType.Tables[0];
                    drpM0CustType.DataTextField = "Name";
                    drpM0CustType.DataValueField = "ID";
                    drpM0CustType.DataBind();
                    drpM0CustType.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillTitle()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsTitle;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsTitle = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetNameTitle", drpM0CustType.SelectedValue);
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


        protected void bConvertToHold(object sender, EventArgs e)
        {
            //if (bHold.Text == "Archive")
            //{
            //    MakeEnableDisableControls(false, "Nothing");
            //    bHold.Text = "Retrieve";
            //    bConvertToInq.Enabled = false;

            //}

            //else if (bHold.Text == "Retrieve")
            //{
            //    MakeEnableDisableControls(true, "Nothing");
            //    bHold.Text = "Archive";
            //    bConvertToInq.Enabled = true;


            //}


        }




    }
}