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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Web.UI.HtmlControls;

namespace MANART.Forms.VehicleSales
{
    public partial class frmM7MTI : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
        private DataTable dtAllocation = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        private DataTable dtClosureDetails = new DataTable();
        private int iDealerID = 0;
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
        private int iDealerId = 0;
        private int HOBrID = 0;
        int iPrimaryApplicationID = 0;
        int iM0PriAppID = 0;
        int sStage = 1;
        string DealerCode = "";
        string sNew = "N";
        Boolean bSaveRecord = false;
        int iMenuId = 0;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                ToolbarC.bUseImgOrButton = true;
               

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }


                txtDealerCode.Text =Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Session["UserType"].ToString();
                PDoc.sFormID = "64";
                Location.bUseSpareDealerCode = false;


                if (!IsPostBack)
                {
                    FillCombo();
                    Session["LeadObjective"] = null;
                    //Session["LeadFleetDtls"] = null;
                    Session["LeadClosure"] = null;
                    Session["LeadQuotDtls"] = null;
                    Session["Allocation"] = null;

                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "M2 Detials";
                FillSelectionGrid();

                if (iID != 0)
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

                //lblTitle.Text = "M3 Details";
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
                    //PSelectionGrid.Style.Add("display", "none");

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;

                    if (bFillDetailsFromGrid(true) == false) return;

                    if (txtCashLoan.Text == "" && txtDocCashLoanType.Text!="")
                    {
                        txtCashLoan.Text = txtDocCashLoanType.Text;
                    }
                    else if (txtDocCashLoanType.Text == "" && txtCashLoan.Text!="")
                    {
                        txtDocCashLoanType.Text = txtCashLoan.Text;
                    }





                    if (txtCashLoan.Text == "L")
                    {
                        if (txtM7Date.Text == "" || txtM7Date.Text == "01/01/1900")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M7 Date');</script>");
                            return;
                        }

                        if (txtMarginAmt.Text == "" || txtMarginAmt.Text == "0")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Margin Amount');</script>");
                            return;
                        }

                        if (txtChRTGSDet.Text == "")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Cheque/RTGS Details');</script>");
                            return;
                        }


                        if (txtChRtgsDate.Text == "" || txtChRtgsDate.Text == "01/01/1900")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Cheque/RTGS Date');</script>");
                            return;
                        }

                    }
                    else if (txtCashLoan.Text == "C")
                    {
                        if (txtM7DateCash.Text == "" || txtM7DateCash.Text == "01/01/1900")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M7 Date');</script>");
                            return;
                        }

                        if (txtPaymentCash.Text == "" || txtPaymentCash.Text == "0")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Payment Amount');</script>");
                            return;
                        }

                        if (txtChRTGSDetCash.Text == "")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Cheque/RTGS Details');</script>");
                            return;
                        }


                        if (txtChRTGSDateCash.Text == "" || txtChRTGSDateCash.Text == "01/01/1900")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Cheque/RTGS Date');</script>");
                            return;
                        }
                    }

                    if (bSaveAttachedDocuments() == false)
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Attached Atleast One Document');</script>");
                        //return;
                    }

                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {
                           //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C")
                            {

                                //  Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M7") + "','" + Server.HtmlEncode(txtM7NoCash.Text) + "');</script>");


                            }
                            else
                            {
                                //  Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M7 ") + "','" + Server.HtmlEncode(txtM7No.Text) + "');</script>");

                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm/Close
                {
                    //string OrderStatus = "";
                    iID = Func.Convert.iConvertToInt(txtID.Text);




                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                          //  Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                            if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C")
                            {

                               
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("M7") + "','" + Server.HtmlEncode(txtM7NoCash.Text) + "');</script>");


                            }
                            else
                            {
                               
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("M7 ") + "','" + Server.HtmlEncode(txtM7No.Text) + "');</script>");

                            }

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
                    GenerateLeadNo("LA");
                    
                    if (bFillDetailsFromGridClosure(true) == false)
                    {

                        ClosureGrid.Enabled = true;
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter Closure details');</script>");
                        return;
                    }


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

        private void ClearDealerHeader()
        {

            txtLeadNo.Text = "";
            txtDocDate.Text = "";

        }



        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);
            Func.Common.BindDataToCombo(drpPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);

            Func.Common.BindDataToCombo(drpCompetitor, clsCommon.ComboQueryType.Competitor, 0);
            Func.Common.BindDataToCombo(drpM4Financier, clsCommon.ComboQueryType.Financier, 0);
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







        //protected void drpM0PriApp_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);
        //    Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);

        //}



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


            if (Type == "M7")
            {
                if (Func.Convert.sConvertToString(txtCashLoan.Text) == "C")
                {

                    txtM7NoCash.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "M7"));
                }
                else
                {
                    txtM7No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "M7"));
                }
            }
            else if (Type == "LA")
            {
                txtLossApp.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "LA"));
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
                if (Type == "M7")
                {
                    sDocName = "M7";
                }
                else if (Type == "LA")
                {
                    sDocName = "LA";
                }
              




                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(5, '0');

                if (sFinYear == "2016")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (VDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                    sDocNo = VDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                    //sDocNo = sDealerCode + "/" + sDocName + "/" + sFinYear + "/" + sMaxDocNo;
                }
                else
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;



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
                string sDocType = Func.Convert.sConvertToString(txtCashLoan.Text);


                ds = GetM7(iID, "New", iDealerId, iDealerId, 0, "");

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


                ds = GetM7(iID, "Max", iDealerId, iDealerId, iID, "");
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


        public DataSet GetM7(int POId, string POType, int DealerID, int HOBrID, int iM1ID, string DocType)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM7", POId, POType, DealerID, HOBrID, iM1ID, DocType);
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






        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "M7 (Margin Money Received)List";
                SearchGrid.AddToSearchCombo("M7 No");
                SearchGrid.AddToSearchCombo("M7 Date");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("M7 Status");
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
                SearchGrid.sSqlFor = "M7DetailsMTI";
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
            GetDataSearch();
            GetDataAndDisplay();
        }

        private void GetDataSearch()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7Searchgrid", iID, iDealerId, iDealerId);
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtDocCashLoanType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CashLoan"]);
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

        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();


                int iID = Func.Convert.iConvertToInt(txtID.Text);
                int iM0ID = Func.Convert.iConvertToInt(txtM1ID.Text);
                string sDoctype = Func.Convert.sConvertToString(txtDocCashLoanType.Text);
                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetM7(iID, "All", iDealerId, iDealerId, iM0ID, sDoctype);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);

                    DisplayData(ds);

                }
                else
                {
                    ds = GetM7(iID, "Max", iDealerId, iDealerId, iM0ID, sDoctype);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);

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
                if (ds == null || ds.Tables[5].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }

                //Display Header    
                if (ds.Tables[3].Rows.Count > 0)
                {



                    //M0
                    txtM0ID.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["ID"]);
                    txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Cust_ID"]);
                    FillLeadType();
                    drpM0CustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["type_flag_id"]);
                    FillTitle();
                    drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Prefix"]);
                    txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["name"]);


                    txtM0.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Tr_Num"]);
                    txtM0Date.Text = Func.Convert.tConvertToDate(ds.Tables[4].Rows[0]["M0_Date"], false);


                    string IsMTICust = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Is_MTICust"]);
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
                    drpM0Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["M0_Financier"]);
                    //M1

                    //Header
                    txtM1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtLeadNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_inq_no"]);
                    txtEnquiryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EnquiryNo"]);
                    txtEnquiryNoCash.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EnquiryNo"]);
                    txtDocDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Lead_Date"], false);
                    drpPOType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]);


                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mod_Cat_ID"]);

                    drpModelGroup.SelectedValue = "1";


                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    //hdnMinFPDADate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MinDate"]);

                    txtM2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                    txtQutNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Qut_No"]);
                    txtqutdate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Qut_Date"], false);
                    //txtDiscAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Disc_ApprovalNo"]);
                    //txtDiscAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Disc_ApprovalDate"], false);
                    //txtProformaNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Proforma_No"]);
                    //txtProformaDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Proforma_Date"], false);
                    txtM2No.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["M2_No"]);
                    txtM2Date.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["M2_Date"], false);
                    drpCompetitor.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Competitor"]);
                    txtCompModel.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Comp_Model"]);
                    txtCompDiscAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Comp_DiscAmt"]);


                    // Appproval Details
                    txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["ID"]);
                    txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Approval_No"]);
                    txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[5].Rows[0]["Approval_Date"], false);
                    txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Discount_ApprovedAmt"]);
                    txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Approved_DealerShare"]);
                    txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["Approved_MTIShare"]);
                    txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["remarks"]);
                    txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[5].Rows[0]["FinalAmt"]);

                    txtM3ID.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["ID"]);
                    txtM3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["M3_No"]);
                    txtM3Date.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["M3_Date"], false);
                    txtCustPONo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["PO_No"]);
                    txtCustPODate.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["PO_Date"], false);
                    txtMTIProforma.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["MTI_Proforma_No"]);
                    txtMTIProformaDate.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["MTI_Proforma_Date"], false);
                    txtRemarks.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["remarks"]);

                    //txtApprovalDate.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["Disc_ApprovalDate"], false);

                    string Isfund = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Fund"]);
                    if (Isfund == "C")
                    {
                        drpFund.SelectedValue = "1";
                    }
                    else if (Isfund == "L")
                    {
                        drpFund.SelectedValue = "2";
                    }
                    else
                    {
                        drpFund.SelectedValue = "0";
                    }


                    txtM4ID.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["ID"]);
                    txtM4No.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["M4_no"]);
                    txtM4Date.Text = Func.Convert.tConvertToDate(ds.Tables[6].Rows[0]["M4_Date"], false);
                    drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["M4Financier"]);
                    txtBranch.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["Branch"]);
                    string IsDocCollected = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["DocPending"]);

                    if (IsDocCollected == "Y")
                    {
                        drpDocCollected.SelectedValue = "1";
                    }
                    else if (IsDocCollected == "N")
                    {
                        drpDocCollected.SelectedValue = "2";
                    }
                    else
                    {
                        drpDocCollected.SelectedValue = "0";
                    }
                    txtPendingDoc.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["PendingDoc"]);
                    txtLoanAmt.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["LoanAmt"]);
                    txtMarginMoney.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["MarginMoney"]);
                    txtTenure.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["Tenure"]);
                    txtInterestRate.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["Interest_Rate"]);


                    txtM5ID.Text = Func.Convert.sConvertToString(ds.Tables[8].Rows[0]["ID"]);
                    txtM5No.Text = Func.Convert.sConvertToString(ds.Tables[8].Rows[0]["M5_no"]);
                    txtM5Date.Text = Func.Convert.tConvertToDate(ds.Tables[8].Rows[0]["M5_Date"], false);


                    string IsAggPDC = Func.Convert.sConvertToString(ds.Tables[8].Rows[0]["AggPDCCollect"]);

                    if (IsAggPDC == "Y")
                    {
                        drpAggPDC.SelectedValue = "1";
                    }
                    else if (IsAggPDC == "N")
                    {
                        drpAggPDC.SelectedValue = "2";
                    }
                    else
                    {
                        drpAggPDC.SelectedValue = "0";
                    }
                    txtPendingRemarks.Text = Func.Convert.sConvertToString(ds.Tables[8].Rows[0]["PendingRemark"]);


                    //M6
                    txtM6ID.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["ID"]);
                    txtM6No.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["M6_no"]);
                    txtM6Date.Text = Func.Convert.tConvertToDate(ds.Tables[9].Rows[0]["M6_Date"], false);
                    txtDoNo.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["DO_No"]);
                    txtDODate.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["DO_Date"]);
                    txtDoAMt.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["DO_Amt"]);
                    txtPaymentAmt.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["Payment_Amt"]);
                    txtPaymentDate.Text = Func.Convert.sConvertToString(ds.Tables[9].Rows[0]["Payment_Date"]);

                    //M7
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["ID"]);
                    txtM7No.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["M7_no"]);
                    txtM7Date.Text = Func.Convert.tConvertToDate(ds.Tables[10].Rows[0]["M7_Date"], false);
                    txtMarginAmt.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["MarginAmt"]);
                    txtChRTGSDet.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["ChRTGSDet"]);
                    txtChRtgsDate.Text = Func.Convert.tConvertToDate(ds.Tables[10].Rows[0]["ChRTGSDate"], false);
                    txtDocCashLoanType.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["CashLoan"]);


                    txtM7NoCash.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["M7_no"]);
                    txtM7DateCash.Text = Func.Convert.tConvertToDate(ds.Tables[10].Rows[0]["M7_Date"], false);
                    txtPaymentCash.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["MarginAmt"]);
                    txtChRTGSDetCash.Text = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["ChRTGSDet"]);
                    txtChRTGSDateCash.Text = Func.Convert.tConvertToDate(ds.Tables[10].Rows[0]["ChRTGSDate"], false);



                }


                else
                {
                    ClearDealerHeader();
                }

                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = "N";
                hdnLost.Value = Func.Convert.sConvertToString(ds.Tables[10].Rows[0]["M7_Lost"]);

                if (txtDocCashLoanType.Text == "C")
                {

                    M4DocDet.Visible = false;
                    M5DocDet.Visible = false;
                    M6DocDet.Visible = false;
                }
                else if (txtDocCashLoanType.Text == "L")
                {

                    M4DocDet.Visible = true;
                    M5DocDet.Visible = true;
                    M6DocDet.Visible = true;
                }

                else
                {

                    M4DocDet.Visible = true;
                    M5DocDet.Visible = true;
                    M6DocDet.Visible = true;
                }

                Session["LeadObjective"] = null;
                dtDetails = ds.Tables[11];
                Session["LeadObjective"] = dtDetails;
                BindDataToGrid(bRecordIsOpen, 0);


                Session["LeadClosure"] = null;
                dtClosureDetails = ds.Tables[7];
                Session["LeadClosure"] = dtClosureDetails;
                BindDataToGridClosure(bRecordIsOpen, 0);


                Session["LeadQuotDtls"] = null;
                dtQuotDetails = ds.Tables[2];
                Session["LeadQuotDtls"] = dtQuotDetails;
                BindDataToGridQuot();

                Session["Allocation"] = null;
                dtAllocation = ds.Tables[12];
                Session["Allocation"] = dtAllocation;
                BindDataToGridAllocation();

                //Display Attachment Details
                dtFileAttach = ds.Tables[13];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();


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


        protected void ClosureGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillDetailsFromGridClosure(false);
                BindDataToGridClosure(true, 1);
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

        private void BindDataToGridAllocation()
        {


            if (Session["Allocation"] == null)
            {
                Session["Allocation"] = dtAllocation;
            }
            else
            {
                dtAllocation = (DataTable)Session["Allocation"];
            }
            Session["Allocation"] = dtAllocation;
            GrdAllocation.DataSource = dtAllocation;
            GrdAllocation.DataBind();



            //QuotationDtls.Rows[3].Cells[3].Enabled = false;
            SetGridControlPropertyAllocation(false);




        }



        private bool bFillDetailsFromGridAllocation(bool bDisplayMsg)
        {

            dtAllocation = (DataTable)Session["Allocation"];
            //int iCntForDelete = 0;

            bDetailsRecordExist = false;

            //int iCntForSelect = 0;


            for (int iRowCnt = 0; iRowCnt < GrdAllocation.Rows.Count; iRowCnt++)
            {
                TextBox txtallotID = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtallotID");
                dtAllocation.Rows[iRowCnt]["ID"] = Func.Convert.iConvertToInt(txtallotID.Text);



                DropDownList drpChassisNo = (DropDownList)GrdAllocation.Rows[iRowCnt].FindControl("drpChassisNo");
                dtAllocation.Rows[iRowCnt]["Chassis_No"] = Func.Convert.iConvertToInt(drpChassisNo.SelectedValue);



                TextBox txtEngineNo = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtEngineNo");
                dtAllocation.Rows[iRowCnt]["Engine_No"] = Func.Convert.sConvertToString(txtEngineNo.Text);

                TextBox txtMTIInvNo = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtMTIInvNo");
                dtAllocation.Rows[iRowCnt]["MTI_InvNo"] = Func.Convert.sConvertToString(txtMTIInvNo.Text);

                TextBox txtMTIInvDate = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtMTIInvDate");
                dtAllocation.Rows[iRowCnt]["MTI_InvDate"] = Func.Convert.sConvertToString(txtMTIInvDate.Text);

                TextBox txtAllocationQty = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtAllocationQty");
                dtAllocation.Rows[iRowCnt]["Qty"] = Func.Convert.sConvertToString(txtAllocationQty.Text);

            }


            return true;


        }


        private void SetGridControlPropertyAllocation(bool bRecordIsOpen)
        {

            string sDealerId = Func.Convert.sConvertToString(iDealerId);




            for (int iRowCnt = 0; iRowCnt < GrdAllocation.Rows.Count; iRowCnt++)
            {

                TextBox txtallotID = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtallotID");
                DropDownList drpChassisNo = (DropDownList)GrdAllocation.Rows[iRowCnt].FindControl("drpChassisNo");

                if (Func.Convert.sConvertToString(dtAllocation.Rows[iRowCnt]["Chassis_No"]) != "")
                {
                    Func.Common.BindDataToCombo(drpChassisNo, clsCommon.ComboQueryType.AllocatedChassis, Func.Convert.iConvertToInt(dtAllocation.Rows[iRowCnt]["ID"]));
                }
                else
                {
                    Func.Common.BindDataToCombo(drpChassisNo, clsCommon.ComboQueryType.AllocationChassis, Func.Convert.iConvertToInt(txtID.Text));
                }


                TextBox txtEngineNo = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtEngineNo");
                TextBox txtMTIInvNo = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtMTIInvNo");
                TextBox txtMTIInvDate = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtMTIInvDate");
                TextBox txtAllocationQty = (TextBox)GrdAllocation.Rows[iRowCnt].FindControl("txtAllocationQty");




                txtallotID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(dtAllocation.Rows[iRowCnt]["ID"]));
                drpChassisNo.SelectedValue = Func.Convert.sConvertToString(dtAllocation.Rows[iRowCnt]["Chassis_No"]);

                txtEngineNo.Text = Func.Convert.sConvertToString(dtAllocation.Rows[iRowCnt]["Engine_No"]);
                txtMTIInvNo.Text = Func.Convert.sConvertToString(dtAllocation.Rows[iRowCnt]["MTI_InvNo"]);
                txtMTIInvDate.Text = Func.Convert.tConvertToDate(dtAllocation.Rows[iRowCnt]["MTI_InvDate"], false);

                txtAllocationQty.Text = Func.Convert.sConvertToString(dtAllocation.Rows[iRowCnt]["Qty"]);




            }



        }




        protected void drpChassisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            GridViewRow Allot = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpChassisNo = (DropDownList)Allot.FindControl("drpChassisNo");

            TextBox txtEngineNo = (TextBox)Allot.FindControl("txtEngineNo");
            TextBox txtMTIInvNo = (TextBox)Allot.FindControl("txtMTIInvNo");
            TextBox txtMTIInvDate = (TextBox)Allot.FindControl("txtMTIInvDate");
            TextBox txtAllocationQty = (TextBox)Allot.FindControl("txtAllocationQty");
            int ichassisid = Func.Convert.iConvertToInt(drpChassisNo.SelectedValue);



            if (AlreadyExistChassis(Func.Convert.iConvertToInt(drpChassisNo.SelectedValue)) == false)
            {
                drpChassisNo.SelectedValue = "0";
                return;
            }



            int dscount = 2;
            DataSet dsdet = new DataSet();
            dsdet = objDB.ExecuteStoredProcedureAndGetDataset("SP_checkM7ForAllocation", Func.Convert.iConvertToInt(txtID.Text), ichassisid);

            if (dsdet != null)
            {

                dscount = Func.Convert.iConvertToInt(dsdet.Tables[0].Rows[0]["count"]);
            }

            if (dscount == 1)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select Correct Chassis');</script>");
                txtEngineNo.Text = "";
                txtMTIInvNo.Text = "";
                txtMTIInvDate.Text = "";
                txtAllocationQty.Text = "";
                drpChassisNo.SelectedValue = "0";
                return;
            }


            DataSet ds = new DataSet();
            ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetAllocationChassisDet", ichassisid, iDealerId, iDealerId);

            if (ds != null) // if no Data Exist
            {
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_No"]);
                txtMTIInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MTI_InvNo"]);
                txtMTIInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["MTI_InvDate"], false);

                txtAllocationQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
            }
        }



        private bool AlreadyExistChassis(int chassisid)
        {


            Boolean bValidate = true;
            int exist = 0;
            for (int iRowCnt = 0; iRowCnt < GrdAllocation.Rows.Count; iRowCnt++)
            {

                DropDownList drpChassisNo = (DropDownList)GrdAllocation.Rows[iRowCnt].FindControl("drpChassisNo");



                if (chassisid == Func.Convert.iConvertToInt(drpChassisNo.SelectedValue))
                {
                    exist = exist + 1;
                }




            }

            if (exist > 1)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('ChasssisNo Already Selected !');</script>");
                bValidate = false;
            }

            return bValidate;

        }

        protected void drpCloseRsn_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList drpClosure = (DropDownList)gvr.FindControl("drpCloseRsn");
            DropDownList drpCompClosure = (DropDownList)gvr.FindControl("drpCloseCompetitor");
            TextBox txtCompetitorClosure = (TextBox)gvr.FindControl("txtCompetitor");
            TextBox txtCompQty = (TextBox)gvr.FindControl("txtCompQty");

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

        private void BindDataToGridQuot()
        {
            //If No Data in Grid
            if (Session["LeadQuotDtls"] == null)
            {
                Session["LeadQuotDtls"] = dtQuotDetails;
            }
            else
            {
                dtQuotDetails = (DataTable)Session["LeadQuotDtls"];
            }
            Session["LeadQuotDtls"] = dtQuotDetails;
            QuotationDtls.DataSource = dtQuotDetails;
            QuotationDtls.DataBind();



            //QuotationDtls.Rows[3].Cells[3].Enabled = false;
            SetGridControlPropertyQuot(false);



        }

        private void SetGridControlPropertyQuot(bool bRecordIsOpen)
        {

        }


        protected void txtQuotValue_TextChanged(object sender, EventArgs e)
        {
            QuotationDtls.Rows[3].Cells[3].Text = QuotationDtls.Rows[0].Cells[3].Text;
        }



        private bool bFillQuotFromGrid()
        {
            dtQuotDetails = (DataTable)Session["LeadQuotDtls"];



            double dQty = 0;

            for (int iRowCnt = 0; iRowCnt < QuotationDtls.Rows.Count; iRowCnt++)
            {
                Label lblQuotID = QuotationDtls.Rows[iRowCnt].FindControl("lblQuotID") as Label;

                int iDtSelPartRow = 0;

                for (int iDtRowCnt = 0; iDtRowCnt < dtQuotDetails.Rows.Count; iDtRowCnt++)
                {
                    if (Func.Convert.iConvertToInt(dtQuotDetails.Rows[iDtRowCnt]["QuotId"]) == Func.Convert.iConvertToInt(lblQuotID.Text))
                    {
                        iDtSelPartRow = iDtRowCnt;
                        break;
                    }
                }

                // Get Qty



                dQty = Func.Convert.dConvertToDouble((QuotationDtls.Rows[iRowCnt].FindControl("txtQuotValue") as TextBox).Text);
                dtQuotDetails.Rows[iDtSelPartRow]["Value"] = dQty;







            }
            bDetailsRecordExist = true;
            return true;
        }

        //protected void ClosureGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
        //    {
        //        bFillDetailsFromGridClosure(false);
        //        BindDataToGridClosure(true, 1);
        //    }
        //}




        //private void BindDataToGridClosure(bool bRecordIsOpen, int iNoRowToAdd)
        //{
        //    if (bRecordIsOpen == true)
        //    {
        //        CreateNewRowToDetailsTableClosure(iNoRowToAdd);
        //    }
        //    else
        //    {
        //        ClosureGrid.DataSource = dtClosureDetails;
        //        ClosureGrid.DataBind();
        //    }
        //    SetGridControlPropertyClosure(bRecordIsOpen);
        //}



        //private void SetGridControlPropertyClosure(bool bRecordIsOpen)
        //{
        //    string sDeleteStatus = "";
        //    string sDealerId = Func.Convert.sConvertToString(iDealerId);

        //    int idtRowCnt = 0;


        //    for (int iRowCnt = 0; iRowCnt < ClosureGrid.Rows.Count; iRowCnt++)
        //    {

        //        TextBox txtClosureID = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtClosureID");

        //        DropDownList drpCloseRsn = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseRsn");
        //        Func.Common.BindDataToCombo(drpCloseRsn, clsCommon.ComboQueryType.InqClose, 0);



        //        DropDownList drpCloseCompetitor = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseCompetitor");
        //        Func.Common.BindDataToCombo(drpCloseCompetitor, clsCommon.ComboQueryType.Competitor, 0);




        //        TextBox txtCompetitor = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompetitor");

        //        sDeleteStatus = "E";
        //        if (idtRowCnt < dtClosureDetails.Rows.Count)
        //        {
        //            txtClosureID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(dtClosureDetails.Rows[iRowCnt]["ID"]));
        //            drpCloseRsn.SelectedValue = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["rsn_Id"]);
        //            drpCloseCompetitor.SelectedValue = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["CompID"]);
        //            txtCompetitor.Text = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["CompetitorMake"]);
        //            sDeleteStatus = Func.Convert.sConvertToString(dtClosureDetails.Rows[iRowCnt]["Status"]);
        //            idtRowCnt = idtRowCnt + 1;
        //        }

        //        //New 
        //        LinkButton lnkNew = (LinkButton)ClosureGrid.Rows[iRowCnt].FindControl("lnkNew");
        //        lnkNew.Style.Add("display", "none");



        //        //Delete 
        //        CheckBox Chk = (CheckBox)ClosureGrid.Rows[iRowCnt].FindControl("ChkForDelete");
        //        Chk.Attributes.Add("onClick", "SelectDeletCheckbox(this)");
        //        Chk.Style.Add("display", "none");

        //        // N :- New , D:- Dellete, E:- Exissting            
        //        if (sDeleteStatus == "D")
        //        {
        //            Chk.Style.Add("display", "");
        //            Chk.Checked = true;
        //            //ClosureGrid.Rows[iRowCnt].BackColor = Color.Orange;
        //        }
        //        else if (sDeleteStatus == "E")
        //        {
        //            lnkNew.Style.Add("display", "none");
        //            Chk.Style.Add("display", "");
        //            Chk.Checked = false;
        //        }

        //        // Allow New To Last Row
        //        if ((iRowCnt + 1) == ClosureGrid.Rows.Count)
        //        {
        //            lnkNew.Style.Add("display", "");
        //        }



        //    }



        //}









        //private void CreateNewRowToDetailsTableClosure(int iNoRowToAdd)
        //{
        //    try
        //    {
        //        //MaxRFPModelRowCount
        //        DataRow dr;
        //        DataTable dtDefaultModel = new DataTable();
        //        int iRowCntStartFrom = 0;
        //        int iMaxLeadObjRowCount = 1;
        //        //iMaxRFPModelRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxRFPModelRowCount"]);

        //        if (Session["LeadClosure"] != null)
        //        {
        //            dtDefaultModel = (DataTable)Session["LeadClosure"];
        //        }
        //        else
        //        {
        //            dtDefaultModel = dtClosureDetails;
        //        }
        //        if (iNoRowToAdd == 0)
        //        {
        //            if (dtDefaultModel.Rows.Count == 0)
        //            {
        //                dtDefaultModel.Columns.Clear();


        //                //dtDefaultModel.Columns.Add(new DataColumn("SRNo", typeof(string)));
        //                dtDefaultModel.Columns.Add(new DataColumn("ID", typeof(int)));
        //                dtDefaultModel.Columns.Add(new DataColumn("rsn_Id", typeof(int)));
        //                dtDefaultModel.Columns.Add(new DataColumn("CompID", typeof(int)));
        //                dtDefaultModel.Columns.Add(new DataColumn("CompetitorMake", typeof(string)));
        //                dtDefaultModel.Columns.Add(new DataColumn("Status", typeof(string)));

        //            }
        //            else
        //            {
        //                if (dtDefaultModel.Rows.Count >= iMaxLeadObjRowCount) goto Bind;
        //            }
        //        }
        //        else
        //        {
        //            iRowCntStartFrom = iMaxLeadObjRowCount;
        //        }

        //        iMaxLeadObjRowCount = iMaxLeadObjRowCount + iNoRowToAdd;

        //        for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLeadObjRowCount; iRowCnt++)
        //        {
        //            dr = dtDefaultModel.NewRow();
        //            //dr["SRNo"] = "1";
        //            dr["ID"] = 0;
        //            dr["rsn_Id"] = 0;
        //            dr["CompID"] = 0;
        //            dr["CompetitorMake"] = "";
        //            dr["Status"] = "";
        //            dtDefaultModel.Rows.Add(dr);
        //            dtDefaultModel.AcceptChanges();

        //        }
        //    Bind: ;
        //        Session["LeadClosure"] = dtDefaultModel;
        //        ClosureGrid.DataSource = dtDefaultModel;
        //        ClosureGrid.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}








        //private bool bFillDetailsFromGridClosure(bool bDisplayMsg)
        //{
        //    string sStatus = "";
        //    dtClosureDetails = (DataTable)Session["LeadClosure"];
        //    int iCntForDelete = 0;
        //    int iModelBodyTypeID = 0;
        //    bDetailsRecordExist = false;
        //    int iModelID = 0;
        //    int iCntForSelect = 0;


        //    for (int iRowCnt = 0; iRowCnt < ClosureGrid.Rows.Count; iRowCnt++)
        //    {
        //        TextBox txtClosureID = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtClosureID");
        //        dtClosureDetails.Rows[iRowCnt]["ID"] = Func.Convert.iConvertToInt(txtClosureID.Text);

        //        DropDownList drpCloseRsn = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseRsn");

        //        dtClosureDetails.Rows[iRowCnt]["rsn_Id"] = Func.Convert.iConvertToInt(drpCloseRsn.SelectedValue);

        //        if (Func.Convert.iConvertToInt(drpCloseRsn.SelectedValue) != 0)
        //        {
        //            iCntForSelect = iCntForSelect + 1;
        //        }

        //        DropDownList drpCloseCompetitor = (DropDownList)ClosureGrid.Rows[iRowCnt].FindControl("drpCloseCompetitor");

        //        dtClosureDetails.Rows[iRowCnt]["CompID"] = Func.Convert.iConvertToInt(drpCloseCompetitor.SelectedValue);



        //        TextBox txtCompetitor = (TextBox)ClosureGrid.Rows[iRowCnt].FindControl("txtCompetitor");
        //        dtClosureDetails.Rows[iRowCnt]["CompetitorMake"] = Func.Convert.sConvertToString(txtCompetitor.Text);




        //        CheckBox Chk = (CheckBox)ClosureGrid.Rows[iRowCnt].FindControl("ChkForDelete");
        //        dtClosureDetails.Rows[iRowCnt]["Status"] = "";
        //        if (Chk.Checked == true)
        //        {
        //            dtClosureDetails.Rows[iRowCnt]["Status"] = "D";
        //            bDetailsRecordExist = true;
        //            iCntForDelete++;
        //        }
        //        else if (drpCloseRsn.SelectedValue != "0")
        //        {
        //            dtClosureDetails.Rows[iRowCnt]["Status"] = "N";
        //            bDetailsRecordExist = true;
        //        }
        //    }

        //    if (iCntForDelete == iCntForSelect)
        //    {
        //        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Entered atleast One Record !');</script>");
        //        return false;
        //    }
        //    return true;


        //}



        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            if (iID == 0)
            {
                GenerateLeadNo("M7");
            }

            int PrvId = 0;
            //if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C" )
            //{
            PrvId = Func.Convert.iConvertToInt(txtM3ID.Text);
            //}
            //else if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "L" )
            //{
            //    PrvId=Func.Convert.iConvertToInt(txtM6ID.Text);
            //}

            if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C")
            {
                if (OrderStatus == "Y")
                {
                    HdrID = objLead.bSaveM7Header(iID, iDealerId, iDealerId, txtM7NoCash.Text, txtM7DateCash.Text, Confirm, OrderStatus,
                       PrvId, Func.Convert.dConvertToDouble(txtPaymentCash.Text), txtChRTGSDetCash.Text, txtChRTGSDateCash.Text
                       , Func.Convert.sConvertToString(txtDocCashLoanType.Text),iMenuId,txtLossApp.Text
                                      );
                }
                else
                {
                    HdrID = objLead.bSaveM7Header(iID, iDealerId, iDealerId, txtM7NoCash.Text, txtM7DateCash.Text, Confirm, OrderStatus,
                       PrvId, Func.Convert.dConvertToDouble(txtPaymentCash.Text), txtChRTGSDetCash.Text, txtChRTGSDateCash.Text
                       , Func.Convert.sConvertToString(txtDocCashLoanType.Text), iMenuId, ""
                                      );
                }
            }
            else if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "L")
            {
                if (OrderStatus == "Y")
                {
                    HdrID = objLead.bSaveM7Header(iID, iDealerId, iDealerId, txtM7No.Text, txtM7Date.Text, Confirm, OrderStatus,
                       PrvId, Func.Convert.dConvertToDouble(txtMarginAmt.Text), txtChRTGSDet.Text, txtChRtgsDate.Text, 
                       Func.Convert.sConvertToString(txtDocCashLoanType.Text),iMenuId,txtLossApp.Text
                                      );
                }
                else
                {
                    HdrID = objLead.bSaveM7Header(iID, iDealerId, iDealerId, txtM7No.Text, txtM7Date.Text, Confirm, OrderStatus,
                     PrvId, Func.Convert.dConvertToDouble(txtMarginAmt.Text), txtChRTGSDet.Text, txtChRtgsDate.Text,
                     Func.Convert.sConvertToString(txtDocCashLoanType.Text), iMenuId, ""
                                    );
                }
            }

            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();
            if (objLead.bSaveM7Objectives(objDB, iDealerID, iDealerId, dtDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }
            //if (bSaveAttachedDocuments() == false) return bSaveRecord;
            if (objLead.bSaveM7FileAttachDetails
                (objDB, dtFileAttach, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }


            if (OrderStatus != "Y")
            {
                bSaveRecord = true;
            }
            else if (OrderStatus == "Y")
            {

                if (objLead.bSaveM7ClosureDetails(objDB, iDealerId, iDealerId, dtClosureDetails, iID) == true)
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
            drpM0Financier.Enabled = false;

            txtM0.Enabled = false;
            txtM0Date.Enabled = false;

            drpIsMTICust.Enabled = false;

            DetailsGrid.Enabled = bEnable;

            //Enable header Controls of Form
            txtDocDate.Enabled = false;
            txtLeadNo.Enabled = false;

            drpPOType.Enabled = false;

            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelCat.Enabled = false;
            drpModelGroup.Enabled = false;
            txtQty.Enabled = false;
            txtQutNo.Enabled = false;
            txtqutdate.Enabled = false;
            txtM2No.Enabled = false;
            txtM2Date.Enabled = false;
            drpCompetitor.Enabled = false;
            txtCompModel.Enabled = false;
            txtCompDiscAmt.Enabled = false;
            QuotationDtls.Enabled = false;

            txtAppNo.Enabled = false;
            txtAppDate.Enabled = false;
            txtAppDisc.Enabled = false;
            txtAppDealershare.Enabled = false;
            txtAppMTIshare.Enabled = false;
            txtAppFinalAmt.Enabled = false;
            txtAppremarks.Enabled = false;




            txtM3No.Enabled = false;
            txtM3Date.Enabled = false;
            txtCustPONo.Enabled = false;
            txtCustPODate.Enabled = false;
            txtMTIProforma.Enabled = false;
            txtMTIProformaDate.Enabled = false;
            txtRemarks.Enabled = false;
            drpFund.Enabled = false;



            txtM4No.Enabled = false;
            txtM4Date.Enabled = false;
            drpM4Financier.Enabled = false;
            txtBranch.Enabled = false;
            drpDocCollected.Enabled = false;
            txtPendingDoc.Enabled = false;
            txtLoanAmt.Enabled = false;
            txtMarginMoney.Enabled = false;
            txtTenure.Enabled = false;
            txtInterestRate.Enabled = false;


            txtM5No.Enabled = false;
            txtM5Date.Enabled = false;
            drpAggPDC.Enabled = false;
            txtPendingRemarks.Enabled = false;


            txtM6No.Enabled = false;
            txtM6Date.Enabled = false;
            txtDoNo.Enabled = false;
            txtDODate.Enabled = false;
            txtDoAMt.Enabled = false;
            txtPaymentDate.Enabled = false;
            txtPaymentAmt.Enabled = false;


            txtM7No.Enabled = false;
            txtM7Date.Enabled = false;
            txtMarginAmt.Enabled = bEnable;
            txtChRTGSDet.Enabled = bEnable;
            txtChRtgsDate.Enabled = bEnable;
            txtM7NoCash.Enabled = false;

            txtM7DateCash.Enabled = false;
            txtPaymentCash.Enabled = bEnable;
            txtChRTGSDetCash.Enabled = bEnable;
            txtChRTGSDateCash.Enabled = bEnable;
            PAllocation.Visible = false;

            if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C")
            {

                M7DocCash.Visible = true;
                M7DocDet.Visible = false;

            }
            else
            {
                M7DocCash.Visible = false;
                M7DocDet.Visible = true;
            }



            if (type == "Cancel" && bEnable == false)
            {
                ClosureGrid.Enabled = false;
                GrdAllocation.Enabled = false;
                bConfirm.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Confirm" && bEnable == false)
            {

                GrdAllocation.Enabled = true;
                ClosureGrid.Enabled = false;
                bConfirm.Enabled = true;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Lost" && bEnable == false)
            {

                GrdAllocation.Enabled = false;
                ClosureGrid.Enabled = true;
                bConfirm.Enabled = false;

            }

            else if (type == "Nothing" && bEnable == true)
            {
                ClosureGrid.Enabled = false;
                GrdAllocation.Enabled = false;
                bConfirm.Enabled = false;
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




        protected void SaveAllocation(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                bDetailsRecordExist = false;
                bDetailsRecordExist = bFillDetailsFromGridAllocation(true);


                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return;
                }

                if (bsaveAllocation(objDB, iDealerId, iDealerId, dtAllocation, iID) == true)
                {
                    bSaveRecord = true;
                }

                if (bSaveRecord == true)
                {
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('ChasssisNo Already Selected !');</script>");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                }

                FillSelectionGrid();
                GetDataAndDisplay();
            }

        }
        public bool bsaveAllocation(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {


                    if (dtDet.Rows[iRowCnt]["Id"].ToString() != "0")
                    {
                        objDB.BeginTranasaction();

                        objDB.ExecuteStoredProcedure("SP_SaveM7Allocation", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["Chassis_No"],

                        dtDet.Rows[iRowCnt]["Engine_No"], dtDet.Rows[iRowCnt]["MTI_InvNo"],
                            //Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false),
                         dtDet.Rows[iRowCnt]["MTI_InvDate"], dtDet.Rows[iRowCnt]["Qty"], Func.Convert.iConvertToInt(txtM3ID.Text)
                            );

                        objDB.CommitTransaction();

                    }




                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
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
            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);

        }


        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {


         
            Location.iDealerId = PDoc.iDealerID;
            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            txtCashLoan.Text = Func.Convert.sConvertToString(PDoc.sDoc);
            Location.FillLocation();


            FillCombo();
            FillSelectionGrid();

            PSelectionGrid.Style.Add("display", "none");
            txtID.Text = "";

            GetDataFromM3M6();



            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }


        private void GetDataFromM3M6()
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
                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                string sDocType = Func.Convert.sConvertToString(txtCashLoan.Text);
                //iProformaID = 15;
                if (iM0ID != 0)
                {
                    ds = GetM7(iID, "New", iDealerId, iDealerId, iM0ID, sDocType);

                    DisplayData(ds);

                }
                else
                {
                    ClearDealerHeader();
                }

                if (Func.Convert.sConvertToString(txtDocCashLoanType.Text) == "C")
                {

                    M7DocCash.Visible = true;
                    M7DocDet.Visible = false;

                }
                else
                {
                    M7DocCash.Visible = false;
                    M7DocDet.Visible = true;
                }
                GenerateLeadNo("M7");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
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


        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
            {
                bFillDetailsFromGrid(false);
                BindDataToGrid(true, 1);
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
            //string sStatus = "";
            dtDetails = (DataTable)Session["LeadObjective"];
            int iCntForDelete = 0;
            //int iModelBodyTypeID = 0;
            bDetailsRecordExist = false;
            //int iModelID = 0;
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
                Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M7'");



                //Get Date
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate")) as ASP.webparts_currentdate_ascx).Enabled = true;
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Enabled =true;
                (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = "";


                //Get discussion
                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");

                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");
                Func.Common.BindDataToCombo(drpNextObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M7'");


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

        #region Attach File

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string fileNames = FileAttchGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            string FileExtension = Path.GetExtension(fileNames);

            if (fileNames.Trim() != "")
            {
                //Clear the content of the responce
                Response.ClearContent();
                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileNames);
                // Add the file size into the response header
                Response.AddHeader("Content-Length", fileNames.Length.ToString());
                // Set the Content Type 
                Response.ContentType = ReturnExtension(FileExtension.ToLower());
                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile((sPath + "Vehicle Sale\\M7" + "\\" + fileNames));
                // End the response 
                Response.End();
            }
            //Response.ContentType = "image/jpg";
            //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileNames + "\"");
            ////Response.TransmitFile(Server.MapPath(filePath));
            //string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            //Response.TransmitFile((sPath + "Parts\\Part Claim" + "\\" + fileNames));
            //Response.End();
        }




        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                //case ".htm":
                //case ".html":
                //case ".log": return "text/HTML";
                case ".txt": return "text/plain";
                case ".doc": return "application/ms-word";
                //case ".tiff":
                //case ".tif": return "image/tiff";
                //case ".asf": return "video/x-ms-asf";
                //case ".avi": return "video/avi";
                case ".zip": return "application/zip";
                case ".xls":
                case ".csv": return "application/vnd.ms-excel";
                case ".gif": return "image/gif";
                case ".jpg":
                case "jpeg": return "image/jpeg";
                case ".bmp": return "image/bmp";
                //case ".wav": return "audio/wav";
                //case ".mp3": return "audio/mpeg3";
                //case ".mpg":
                //case "mpeg": return "video/mpeg";
                case ".rtf": return "application/rtf";
                //case ".asp": return "text/asp";
                case ".pdf": return "application/pdf";
                //case ".fdf": return "application/vnd.fdf";
                case ".ppt": return "application/mspowerpoint";
                //case ".dwg": return "image/vnd.dwg";
                //case ".msg": return "application/msoutlook";
                case ".xml":
                case ".sdxl": return "application/xml";
                //case ".xdp": return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }



        protected void FileAttchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            //string[] paths = Directory.GetFiles(sPath + "Parts\\Part Claim");

            //HtmlAnchor achFileName = (HtmlAnchor)e.Row.Cells[0].FindControl("achFileName");
            //if (achFileName != null)
            //{
            //    for (int i = 0; i < paths.Length; i++)
            //    {
            //        if (sPath + "Parts\\Part Claim" + "\\" + achFileName.InnerHtml == paths[i])
            //        {

            //           achFileName.HRef = paths[i];
            //            break;
            //        }
            //    }
            //    //achFileName.HRef = sPath + "Parts\\Part Claim" + "\\" + achFileName.InnerHtml;
            //    //achFileName.HRef = @"C:\Upload Documents\Transaction\Parts\Part Claim\" + achFileName.InnerHtml;
            //    achFileName.Target = "_blank";
            //}

        }


        // To Save the attach Document 
        // To Save the attach Document 
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bool bSaveRecord = true;
            // Get Details Of The Existing file attach.
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            string sSourceFileType = "";
            string sSourceFileName1 = "";
            string strNewPath = "";
            DataRow dr;
            int iRecorFound = 0;
            string[] acceptedFileTypes = new string[12];
            acceptedFileTypes[0] = ".jpg";
            acceptedFileTypes[1] = ".pdf";
            acceptedFileTypes[2] = ".jpeg";
            acceptedFileTypes[3] = ".gif";
            acceptedFileTypes[4] = ".png";
            acceptedFileTypes[5] = ".doc";
            acceptedFileTypes[6] = ".docx";
            acceptedFileTypes[7] = ".xls";
            acceptedFileTypes[8] = ".xlsx";
            acceptedFileTypes[9] = ".ppt";
            acceptedFileTypes[10] = ".txt";
            acceptedFileTypes[11] = ".zip";

            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);
                    sSourceFileType = Path.GetExtension(uploads[i].FileName);
                    bool acceptFile = false;
                    if (sSourceFileName.Trim() != "")
                    {
                        //should we accept the file?
                        for (int j = 0; j <= 11; j++)
                        {
                            if (sSourceFileType == acceptedFileTypes[j])
                            {
                                //accept the file, yay!
                                acceptFile = true;
                                break;
                            }
                        }
                        if (!acceptFile)
                        {
                            lblMessage.Text = "The file you are trying to upload is not a permitted file type!";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            return false;
                        }
                        //File Size
                        int iFileSize = uploads[i].ContentLength;
                        double filelimit = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetFileSizeLimit, 0, ""));
                        if (iFileSize > Func.Convert.iConvertToInt(filelimit))
                        {
                            // File exceeds the file maximum size
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please make sure your file size is less than 3 MB')", true);
                            return false;
                        }


                        if (sSourceFileName.Trim() != "" && (iFileSize <= Func.Convert.iConvertToInt(filelimit)) && acceptFile == true)
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

                            //string[] splClaimNo = txtClaimNo.Text.Split('/');
                            //if (splClaimNo.Length > 1)
                            //{
                            //    lblFileName.Text = "";
                            //    for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                            //        lblFileName.Text = lblFileName.Text + splClaimNo[iCnt];
                            //}
                            sSourceFileName1 = Func.Convert.sConvertToString(iDealerId) + "_" + txtM3No.Text + "_" + sSourceFileName;
                            sSourceFileName1 = sSourceFileName1.Replace("/", "");
                            dr["File_Names"] = sSourceFileName1;
                            //dr["File_Names"] = Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                            dr["UserId"] = Func.Convert.sConvertToString(Session["UserID"]);
                            dr["Status"] = "S";


                            //Saving it in temperory Directory.                                       
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Vehicle Sale\\M3");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }

                            //uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));
                            uploads[i].SaveAs((sPath + "Vehicle Sale \\M3" + "\\" + sSourceFileName1));

                            strNewPath = sPath + "Vehicle Sale\\M3" + "\\" + sSourceFileName1;
                            //dr["Path"] = strNewPath;

                            dtFileAttach.Rows.Add(dr);
                            dtFileAttach.AcceptChanges();
                        }
                    }//END String is Empty
                }//END Try
                catch (Exception ex)
                {
                    return false;
                    Func.Common.ProcessUnhandledException(ex);
                }

            }


            if (dtFileAttach.Rows.Count > 0)
            {
                bSaveRecord = true;
                return bSaveRecord;
            }
            else
            {
                bSaveRecord = false;
                return bSaveRecord;
            }


            return bSaveRecord;
        }

        private void bFillDetailsFromFileAttachGrid()
        {
            bool bSaveRecord = true;
            DataRow dr;
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
                    //if (txtID.Text != "")//no
                    //    dr["ID"] = 0;
                    //else
                    dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(iDealerId);

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                    }
                    dtFileAttach.Rows.Add(dr);
                    dtFileAttach.AcceptChanges();
                }
            }
            //if (dtFileAttach.Rows.Count > 0)
            //{
            //    bSaveRecord = true;
            //    return bSaveRecord;
            //}
            //else
            //{
            //    bSaveRecord = false;
            //    return bSaveRecord;
            //}

            //return bSaveRecord;
        }

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if (dtFileAttach.Rows.Count != 0)
            {
                if (dtFileAttach != null)
                {
                    FileAttchGrid.DataSource = dtFileAttach;
                    FileAttchGrid.DataBind();

                    for (int iColCnt = 0; iColCnt < FileAttchGrid.Columns.Count; iColCnt++)
                    {
                        if (dtFileAttach.Rows[0]["ID"].ToString() == "0")
                            FileAttchGrid.Columns[iColCnt].Visible = (FileAttchGrid.Columns[iColCnt].HeaderText == "Download") ? false : true;
                        else
                        {
                            FileAttchGrid.Columns[iColCnt].Visible = (FileAttchGrid.Columns[iColCnt].HeaderText == "Download") ? true : true;
                        }
                    }
                }
            }

        }
        #endregion







    }
}