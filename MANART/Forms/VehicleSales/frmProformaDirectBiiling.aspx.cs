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
    public partial class frmProformaDirectBiiling : System.Web.UI.Page
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
        Boolean bSaveRecord = false;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                ToolbarC.bUseImgOrButton = true;
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();


                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);


                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }



                //ToolbarC.iValidationIdForSave = 69;


                PDoc.sFormID = "66";
                Location.bUseSpareDealerCode = false;
                if (!IsPostBack)
                {
                    FillCombo();
                    //Session["LeadObjective"] = null;
                    //Session["LeadFleetDtls"] = null;
                    //Session["LeadClosure"] = null;
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

                lblTitle.Text = "Proforma Invoice";
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
                    ////ClearDealerHeader();
                    //DisplayPreviousRecord();
                    //txtID.Text = "0";
                    //iID = 0;
                    ////GenerateLeadNo("L");
                    //ToolbarC.sSetMessage(WebParts_Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, false);
                    //return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    //bDetailsRecordExist = false;
                    ////ClosureGrid.Enabled = false;


                    //bFillQuotFromGrid();


                    //if (bDetailsRecordExist == false)
                    //{
                    //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    //    return;
                    //}
                    if (txtAppDate.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Approval Date');</script>");
                        return;
                    }

                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm/Close
                {
                    //string OrderStatus = "";
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;



                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        //txtID.Text = Func.Convert.sConvertToString(iID);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }


                }

                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    iID = bHeaderSave("Y", "N", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
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


                dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_vehicle_Code from M_Dealer where Id=" + iDealerId);

                if (dsDCode.Tables[0].Rows.Count > 0)
                {
                    DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                }
            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }


            if (Type == "MPI")
            {
                txtAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "MPI"));

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
                if (Type == "MPI")
                {
                    sDocName = "MPI";
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

                ds = GetApproval(iID, "New", iDealerId, iHOBrId, 0);

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


                ds = GetApproval(iID, "Max", iDealerId, iHOBrId, iID);
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


        public DataSet GetApproval(int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetProformaInv", POId, POType, DealerID, HOBrID, iM1ID);
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
                SearchGrid.sGridPanelTitle = "Proforma Invoice Details";
                SearchGrid.AddToSearchCombo("Proforma No");
                SearchGrid.AddToSearchCombo("Proforma Date");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("Status");
                //SearchGrid.iDealerID = iDealerId;
                //SearchGrid.iBrHODealerID = iHOBrId;
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
                SearchGrid.iBrHODealerID = iHOBrId;
                SearchGrid.sSqlFor = "MTIPIDetails";
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
                int iM0ID = Func.Convert.iConvertToInt(txtM2ID.Text);
                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetApproval(iID, "All", iDealerId, iHOBrId, iM0ID);
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
                    ds = GetApproval(iID, "Max", iDealerId, iHOBrId, iM0ID);
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
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }

                //Display Header    
                if (ds.Tables[2].Rows.Count > 0)
                {



                    //M0
                    txtM0ID.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["ID"]);
                    txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Cust_ID"]);
                    FillLeadType();
                    drpM0CustType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["type_flag_id"]);
                    FillTitle();
                    drpTitle.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Prefix"]);
                    txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["name"]);
                    //txtFirstName.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["First_Name"]);
                    //txtLastName.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Last_Name"]);

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

                    //M1

                    //Header
                    txtM1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtLeadNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_inq_no"]);
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
                    txtM2No.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["M2_No"]);
                    txtM2Date.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["M2_Date"], false);
                    txtQutNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Qut_No"]);
                    txtqutdate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Qut_Date"], false);
                    drpCompetitor.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Competitor"]);
                    txtCompModel.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Comp_Model"]);
                    txtCompDiscAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Comp_DiscAmt"]);

                    // Appproval Details
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["ID"]);
                    txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Approval_No"]);
                    txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["Approval_Date"], false);
                    txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Discount_ApprovedAmt"]);
                    txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Approved_DealerShare"]);
                    txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Approved_MTIShare"]);
                    txtFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["FinalAmt"]);
                    txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["remarks"]);
                    txtDiscAppNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["DiscApp"]);
                    txtDiscAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[3].Rows[0]["DiscAppDate"], false);

                }


                else
                {
                    ClearDealerHeader();
                }



                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Is_Cancel"]);
                //hdnLost.Value = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["M3_Lost"]);

                //hdnCancle.Value = "N";
                hdnLost.Value = "N";

                //Session["LeadClosure"] = null;
                //dtClosureDetails = ds.Tables[1];
                //Session["LeadClosure"] = dtClosureDetails;
                //BindDataToGridClosure(bRecordIsOpen, 0);


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
                GenerateLeadNo("MPI");
            }

            HdrID = objLead.bSaveProformaInvDirectBilling(iID, iDealerId, HOBrID, txtAppNo.Text, txtAppDate.Text, Func.Convert.dConvertToDouble(txtAppDisc.Text),
                Func.Convert.dConvertToDouble(txtAppDealershare.Text), Func.Convert.dConvertToDouble(txtAppMTIshare.Text),
                     txtAppremarks.Text, Cancel, Confirm, OrderStatus, Func.Convert.iConvertToInt(txtM2ID.Text)
                                );



            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();


            if (objLead.bSaveM2QuotationDtls(objDB, iDealerId, iHOBrId, dtQuotDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }


            if (OrderStatus == "Y")
            {

                if (objLead.bSaveM2ClosureDetails(objDB, iDealerId, iHOBrId, dtClosureDetails, iID) == true)
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
            //txtFirstName.Enabled = false;
            //txtLastName.Enabled = false;
            txtM0.Enabled = false;
            txtM0Date.Enabled = false;
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


            txtM2No.Enabled = false;
            txtM2Date.Enabled = false;
            txtQutNo.Enabled = false;
            txtqutdate.Enabled = false;
            txtCompModel.Enabled = false;
            txtCompDiscAmt.Enabled = false;
            drpCompetitor.Enabled = false;
            QuotationDtls.Enabled = false;

            
            txtAppNo.Enabled = false;
            txtAppDate.Enabled = false;
            txtAppDisc.Enabled = false;
            txtAppDealershare.Enabled = false;
            txtAppMTIshare.Enabled = false;
            txtAppremarks.Enabled = bEnable;
            txtFinalAmt.Enabled = false;
            
            txtDiscAppNo.Enabled = false;
            txtDiscAppDate.Enabled = false;
         

            if (type == "Cancel" && bEnable == false)
            {
                //ClosureGrid.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Confirm" && bEnable == false)
            {


                //ClosureGrid.Enabled = false;
                //drpOrderStatus.Enabled = false;
                //txtInqConfmDate.Enabled = false;
                //txtCustName.Enabled = false;

            }

            else if (type == "Lost" && bEnable == false)
            {


                //ClosureGrid.Enabled = true;


            }

            else if (type == "Nothing" && bEnable == true)
            {
                //ClosureGrid.Enabled = false;
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
            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID=" + modelgrp);
            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID=" + modelgrp);
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
                //clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                //iDealerID = Location.iDealerId;
                string sDealerID = Func.Convert.sConvertToString(PDoc.iDealerID);
                PDoc.sDealerID = sDealerID;
                PDoc.BindDataToGrid();

                iDealerId = PDoc.iDealerID;
                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }


                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iM0ID != 0)
                {
                    ds = GetApproval(iID, "New", iDealerId, iHOBrId, iM0ID);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //ObjDealer = null;
                }
                else
                {
                    ClearDealerHeader();
                }


                GenerateLeadNo("MPI");
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