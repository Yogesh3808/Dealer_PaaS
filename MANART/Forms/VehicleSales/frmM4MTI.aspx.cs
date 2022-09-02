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
    public partial class frmM4MTI : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
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
        
int iMenuId = 0;

  
        Boolean bSaveRecord = false;
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



                PDoc.sFormID = "61";
                Location.bUseSpareDealerCode = false;
                if (!IsPostBack)
                {
                    FillCombo();
                    Session["LeadObjective"] = null;
                    //Session["LeadFleetDtls"] = null;
                    Session["LeadClosure"] = null;
                    Session["LeadQuotDtls"] = null;

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



                    if (txtM4Date.Text == "" || txtM4Date.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M4 Date');</script>");
                        return;
                    }
                    if (drpM4Financier.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Financier');</script>");
                        return;
                    }


                    if (txtBranch.Text == "" )
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Branch');</script>");
                        return;
                    }

                    if (drpDocCollected.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Document Collected');</script>");
                        return;
                    }

                    if (drpFullFund.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select 100 % Funding');</script>");
                        return;
                    }

                    if (txtLoanAmt.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Loan Amount');</script>");
                        return;
                    }

                    if (drpFullFund.SelectedValue == "2")
                    {
                        if (txtMarginMoney.Text == "")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Margin Money');</script>");
                            return;
                        }
                    }


                    if (txtTenure.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Tenure');</script>");
                        return;
                    }

                    if (txtInterestRate.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Interest Rate');</script>");
                        return;
                    }


                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {
                           // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M4 ") + "','" + Server.HtmlEncode(txtM4No.Text) + "');</script>");
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
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M4 ") + "','" + Server.HtmlEncode(txtM4No.Text) + "');</script>");
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
                drpCompClosure.SelectedIndex = 0;
                txtCompetitorClosure.Text = "";

                txtCompQty.Text = txtQty.Text;
                txtCompQty.Enabled = false;


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


            if (Type == "M4")
            {
                txtM4No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "M4"));

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
                if (Type == "M4")
                {
                    sDocName = "M4";
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

                ds = GetM4(iID, "New", iDealerId, iDealerId, 0);

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


                ds = GetM4(iID, "Max", iDealerId, iDealerId, iID);
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


        public DataSet GetM4(int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM4", POId, POType, DealerID, HOBrID, iM1ID);
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
                SearchGrid.sGridPanelTitle = "M4 (Finnacing Process) List";
                SearchGrid.AddToSearchCombo("M4 No");
                SearchGrid.AddToSearchCombo("M4 Date");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("M4 Status");
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
                SearchGrid.sSqlFor = "M4DetailsMTI";
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
                int iM0ID = Func.Convert.iConvertToInt(txtM3ID.Text);
                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetM4(iID, "All", iDealerId, iDealerId, iM0ID);
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
                    ds = GetM4(iID, "Max", iDealerId, iDealerId, iM0ID);
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
                if (ds.Tables[6].Rows.Count > 0)
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


                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["ID"]);
                    txtM4No.Text = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["M4_no"]);
                    txtM4Date.Text = Func.Convert.tConvertToDate(ds.Tables[6].Rows[0]["M4_Date"], false);

                    txtLikelydate.Text = Func.Convert.tConvertToDate(ds.Tables[6].Rows[0]["Likely_BuyDateM4"], false);

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

                    string IsFullFund = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["FullFund"]);

                    if (IsFullFund == "Y")
                    {
                        drpFullFund.SelectedValue = "1";
                    }
                    else if (IsFullFund == "N")
                    {
                        drpFullFund.SelectedValue = "2";
                    }
                    else
                    {
                        drpFullFund.SelectedValue = "0";
                    }


                }


                else
                {
                    ClearDealerHeader();
                }



                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = "N";
                hdnLost.Value = Func.Convert.sConvertToString(ds.Tables[6].Rows[0]["M4_Lost"]);

                Session["LeadObjective"] = null;
                dtDetails = ds.Tables[8];
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
                GenerateLeadNo("M4");
            }


            if (OrderStatus == "Y")
            {
                HdrID = objLead.bSaveM4Header(iID, iDealerId, iDealerId, txtM4No.Text, txtM4Date.Text, Confirm, OrderStatus, Func.Convert.iConvertToInt(txtM3ID.Text),
                    Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtBranch.Text, Func.Convert.sConvertToString(drpDocCollected.SelectedItem.Text)
                    , txtPendingDoc.Text, Func.Convert.dConvertToDouble(txtLoanAmt.Text), Func.Convert.dConvertToDouble(txtMarginMoney.Text)
                    , Func.Convert.iConvertToInt(txtTenure.Text), Func.Convert.dConvertToDouble(txtInterestRate.Text), txtLikelydate.Text
                     , Func.Convert.sConvertToString(drpFullFund.SelectedItem.Text),iMenuId,txtLossApp.Text
                                    );
            }
            else
            {
                HdrID = objLead.bSaveM4Header(iID, iDealerId, iDealerId, txtM4No.Text, txtM4Date.Text, Confirm, OrderStatus, Func.Convert.iConvertToInt(txtM3ID.Text),
                    Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtBranch.Text, Func.Convert.sConvertToString(drpDocCollected.SelectedItem.Text)
                    , txtPendingDoc.Text, Func.Convert.dConvertToDouble(txtLoanAmt.Text), Func.Convert.dConvertToDouble(txtMarginMoney.Text)
                    , Func.Convert.iConvertToInt(txtTenure.Text), Func.Convert.dConvertToDouble(txtInterestRate.Text), txtLikelydate.Text
                     , Func.Convert.sConvertToString(drpFullFund.SelectedItem.Text), iMenuId, ""
                                    );
            }

            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();


            if (objLead.bSaveM4Objectives(objDB, iDealerID, iDealerId, dtDetails, iID) == true)
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

                if (objLead.bSaveM4ClosureDetails(objDB, iDealerId, iDealerId, dtClosureDetails, iID) == true)
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
            drpFullFund.Enabled = bEnable;
            drpIsMTICust.Enabled = false;



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
            DetailsGrid.Enabled = bEnable;
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
            drpM4Financier.Enabled = bEnable;
            txtBranch.Enabled = bEnable;
            drpDocCollected.Enabled = bEnable;
            txtPendingDoc.Enabled = bEnable;
            txtLoanAmt.Enabled = bEnable;
            txtMarginMoney.Enabled = bEnable;
            txtTenure.Enabled = bEnable;
            txtInterestRate.Enabled = bEnable;
            txtLikelydate.Enabled = bEnable;



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
            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);

            //}


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

            GetDataFromM2();
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }


        private void GetDataFromM2()
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


                DataSet ds = new DataSet();
                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iM0ID != 0)
                {
                    ds = GetM4(iID, "New", iDealerId, iDealerId, iM0ID);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //ObjDealer = null;
                }
                else
                {
                    ClearDealerHeader();
                }


                GenerateLeadNo("M4");
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
                Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M4'");



                //Get Date
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate")) as ASP.webparts_currentdate_ascx).Enabled = true;
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Enabled =true;
                (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = "";


                //Get discussion
                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");

                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");
                Func.Common.BindDataToCombo(drpNextObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M4'");


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




    }
}