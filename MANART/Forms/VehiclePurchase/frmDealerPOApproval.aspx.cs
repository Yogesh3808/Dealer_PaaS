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

namespace MANART.Forms.VehiclePurchase
{
    public partial class frmDealerPOApproval : System.Web.UI.Page
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
        int iUserPCRHeadApprID = 0;
        int iUserSQHResourceId = 0;
        
        int iUserRole = 0;
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
        string sAppNew;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                ToolbarC.bUseImgOrButton = true;
           


                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iUserRole = Func.Convert.iConvertToInt(Session["UserRole"]);

                iUserSQHResourceId = Func.Convert.iConvertToInt(Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetUserVehPOApprv, 0, "")));
                
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                if (iUserRole != 2)
                {

                    iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Common.sGetMultiUserAccess(Func.Convert.iConvertToInt(Session["UserID"]), iMenuId));

                    iUserId = (iUserId == iUserPCRHeadApprID) ? iUserSQHResourceId : iUserId;
                    iUserRole = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 9 : iUserRole;
                }

             
               





                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                
                
                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
              
                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }

                ToolbarC.iFormIdToOpenForm = 26;
                //ToolbarC.iValidationIdForSave = 71;

                //Location.bUseSpareDealerCode = false;
                //Location.SetControlValue();


                PDoc.sFormID = "53";
                Location.bUseSpareDealerCode = false;

                if (!IsPostBack)
                {
                    FillCombo();
                    FillPlant();

                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Vehicle PO (Dealer) Detials";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
                Location.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                }

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
                    FillPlant();
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
            //FillPlant();
            //FillSelectionGrid();
            //PSelectionGrid.Style.Add("display", "");
            iDealerId = Location.iDealerId;
            if (iHOBrId == 0)
            {
                FindHOBr(iDealerId);
            }
            FillCombo();
            FillPlant();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            PDoc.sDealerID = Func.Convert.sConvertToString(Location.iDealerId);
            string sDealerID = PDoc.sDealerID;
            PDoc.BindDataToGrid();
            
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
                if (iUserRole == 15)
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
                    PSelectionGrid.Style.Add("display", "");

                  

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;



                    if (txtPOApppDate.Text == "" || txtPOApppDate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Approval Date');</script>");
                        return;
                    }

                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        //if (bSaveDetails("N", "N", "N") == true)
                        //{
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        //}


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




                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
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




                    iID = bHeaderSave("N", "N", "Y");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        if (bSaveDetails("N", "N", "Y") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");
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
        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            Func.Common.BindDataToCombo(drpM4Financier, clsCommon.ComboQueryType.Financier, 0);

            //Func.Common.BindDataToCombo(drpVehPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);

            drpModelGroup.SelectedValue = "1";


        }
        private void FillPlant()
        {


            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantCode");

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpPlant.DataSource = dsCustType.Tables[0];
                    drpPlant.DataTextField = "Name";
                    drpPlant.DataValueField = "ID";
                    drpPlant.DataBind();
                    drpPlant.Items.Insert(0, new ListItem("--Select--", "0"));
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


        protected void drpModelGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillModel();

        }



        protected void drpModelCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModel.SelectedValue = drpModelCode.SelectedValue;
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
            }

        }


        protected void drpModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModelCode.SelectedValue = drpModel.SelectedValue;
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
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

            drpModelGroup.SelectedValue = "1";


        }

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


            if (Type == "POA")
            {

                txtPOAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "POA"));

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
                if (Type == "POA")
                {
                    sDocName = "POA";
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

                //int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                //string sDocType = Func.Convert.sConvertToString(txtCashLoan.Text);


                ds = GetPO(iID, "New", iDealerId, iHOBrId, 0);

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


                ds = GetPO(iID, "Max", iDealerId, iHOBrId, iID);
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

        public DataSet GetPO(int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerVehPO", POId, POType, DealerID, HOBrID, iM1ID);
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
                SearchGrid.sGridPanelTitle = "Dealer PO List";
                SearchGrid.AddToSearchCombo("PO No");
                SearchGrid.AddToSearchCombo("PO Date");
                SearchGrid.AddToSearchCombo("Approval No/Date");
                SearchGrid.AddToSearchCombo("Approval Status");
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
                if (iUserRole == 2)
                {
                    SearchGrid.sSqlFor = "DealerVehPOApproval";
                }
                else if(iUserRole==9)
                {
                    SearchGrid.sSqlFor = "DealerVehPOAPPHO";
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
            iID = Func.Convert.iConvertToInt(txtID.Text);

            GetDataAndDisplay();
        }

        private void GetDataAndDisplay()
        {
            try
            {
                DataSet ds = new DataSet();


                int iID = Func.Convert.iConvertToInt(txtID.Text);
                //int iM0ID = Func.Convert.iConvertToInt(txtM1ID.Text);

                //iProformaID = 1;
                if (iID != 0)
                {
                    ds = GetPO(iID, "All", iDealerId, iHOBrId, 0);
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
                    ds = GetPO(iID, "Max", iDealerId, iHOBrId, 0);
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

                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {


                    txtM3ID.Text = "N";
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_GP"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Cat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);
                    drpVehPOType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Type"]);
                    txtPOAppNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["APPNo"]);
                    txtPOApppDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["AppDate"], false);
                    int M7Id = 0;
                    if (drpVehPOType.SelectedValue == "1")
                    {
                        M7Id = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["M7_Hdr"]);
                        FillEnquirySave(M7Id);
                        drpM7Det.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M7_Hdr"]);
                        txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                    }
                    else
                    {
                        FillEnquiry();
                        drpM7Det.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M7_Hdr"]); ;
                        txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                    }

                    drpPlant.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Plant"]);
                    txtDepot.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Depot"]);
                    txtRoadPermitNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RoadPermitNo"]);
                    txtRoadPermitDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["RoadPermitDate"], false);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);
                    txtChangeQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["changeqty"]);
                    txtPaymentDetailsPO.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PaymentDetails"]);
                    drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Finc"]);
                    txtLocFinc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Loc"]);
                    // Appproval Details
                    txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                    txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approval_No"]);
                    txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Approval_Date"], false);
                    txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount_ApprovedAmt"]);
                    txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_DealerShare"]);
                    txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_MTIShare"]);
                    txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["remarks"]);
                    txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalAmt"]);

                    txtHoldRemarks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HoldRemarks"]);

                }


                if (iUserRole == 2)
                {
                    bModify.Visible = false;
                    txtHoldRemarks.Visible = false;
                    lblModify.Visible = false;
                }
                else if (iUserRole == 9 )
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["App_Confirm"])=="Y")
                    {

                        bModify.Visible = false;
                        txtHoldRemarks.Visible = false;
                        lblModify.Visible = false;
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]) != "0" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppFlag"]) == "Y")
                        {
                            bModify.Visible = true;
                            lblModify.Visible = true;
                            txtHoldRemarks.Visible = true;
                        }
                        else
                        {
                            bModify.Visible = false;
                            txtHoldRemarks.Visible = false;
                            lblModify.Visible = false;
                        }
                    }
                  
                }


                //Display Details
                if (iUserRole == 2)
                {
                    hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_AppConfirm"]);
                  
                }
                else if (iUserRole == 9)
                {
                    hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["App_Confirm"]);
                   
                }
                if (iUserRole == 2)
                {
                    HdnModify.Value = "N";

                }
                else if (iUserRole == 9)
                {
                    HdnModify.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Hold"]);

                }
               

                hdnCancle.Value = "N";
                hdnLost.Value = "N";



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
                if (HdnModify.Value == "Y")
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
                else if (HdnModify.Value == "Y")
                {
                    MakeEnableDisableControls(false, "Modify");
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

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            HdrID = objLead.bSaveDealerVehPO(iID, iDealerId, HOBrID, txtPONo.Text, txtPODate.Text, "Y", "N",
                Func.Convert.iConvertToInt(drpVehPOType.SelectedValue), Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                Func.Convert.iConvertToInt(drpModel.SelectedValue), Func.Convert.iConvertToInt(drpPlant.SelectedValue), txtDepot.Text, txtQty.Text,
             txtModelRate.Text, txtRoadPermitNo.Text, txtRoadPermitDate.Text, Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtChangeQty.Text
             , txtPaymentDetailsPO.Text, txtPOAppNo.Text, txtPOApppDate.Text, iMenuId, "Y", Confirm, txtM3ID.Text
             , Func.Convert.iConvertToInt(drpM4Financier.SelectedValue), txtLocFinc.Text,iUserRole
             , Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
             );


            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();
            if (objLead.bSaveM7Objectives(objDB, iDealerID, iHOBrId, dtDetails, iID) == true)
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

                if (objLead.bSaveM7ClosureDetails(objDB, iDealerId, iHOBrId, dtClosureDetails, iID) == true)
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

            drpVehPOType.Enabled = false;
            txtPONo.Enabled = false;
            txtPODate.Enabled = false;
            txtPOAppNo.Enabled = false;
            txtPOApppDate.Enabled = false;
            txtRoadPermitDate.Enabled = false;
            txtRoadPermitNo.Enabled = false;
            txtDepot.Enabled = bEnable;
            drpPlant.Enabled = bEnable;
            drpM7Det.Enabled = false;

            drpModel.Enabled = false;
            drpModelCat.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelGroup.Enabled = false;
            txtQty.Enabled = false;
            txtPaymentDetailsPO.Enabled = false;
            drpM4Financier.Enabled = false;
            txtLocFinc.Enabled = false;
            //if (drpVehPOType.SelectedValue == "2")
            //{
            //    drpModel.Enabled = false;
            //    drpModelCode.Enabled = false;
            //    drpModelGroup.Enabled = false;
            //    txtQty.Enabled = false;
            //    txtPaymentDetailsPO.Enabled = false;
            //}
            //else
            //{
            //    drpModel.Enabled = bEnable;
            //    drpModelCode.Enabled = bEnable;
            //    drpModelGroup.Enabled = bEnable;
            //    txtQty.Enabled = bEnable;
            //    txtPaymentDetailsPO.Enabled = bEnable;
            //}

            txtChangeQty.Enabled = false;
            txtModelRate.Enabled = false;


            txtAppNo.Enabled = false;
            txtAppDate.Enabled = false;
            txtAppDisc.Enabled = false;
            txtAppDealershare.Enabled = false;
            txtAppMTIshare.Enabled = false;
            txtAppFinalAmt.Enabled = false;
            txtAppremarks.Enabled = false;

            txtHoldRemarks.Enabled = bEnable;
            bModify.Enabled = bEnable;

            if (Func.Convert.iConvertToInt(drpVehPOType.SelectedValue) == 1)
            {
                AppDet.Visible = true;
            }
            else
            {
                AppDet.Visible = false;
            }


            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }

        private void FillEnquiry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquiryNew", iDealerId, HOBrID);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpM7Det.DataSource = dsCustType.Tables[0];
                    drpM7Det.DataTextField = "Name";
                    drpM7Det.DataValueField = "ID";
                    drpM7Det.DataBind();
                    drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
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

        private void FillEnquirySave(int M7ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquirySavenew", iDealerId, HOBrID, M7ID);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    drpM7Det.DataSource = dsCustType.Tables[0];
                    drpM7Det.DataTextField = "Name";
                    drpM7Det.DataValueField = "ID";
                    drpM7Det.DataBind();
                    //drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
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

        protected void drpVehPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpVehPOType.SelectedValue) == 1)
            {
                //drpM7Det.SelectedValue = "0";
                //txtCustName.Text = "";
                drpModel.SelectedValue = "0";
                drpModelCode.SelectedValue = "0";
                drpModelCat.SelectedValue = "0";
                drpModelGroup.SelectedValue = "0";
                txtQty.Text = "";
                txtModelRate.Text = "";
                txtChangeQty.Text = "";
                txtPaymentDetailsPO.Text = "";


                FillEnquiry();
                drpM7Det.Enabled = true;
                txtCustName.Enabled = true;
                drpModel.Enabled = false;
                drpModelCode.Enabled = false;
                drpModelGroup.Enabled = false;
                txtQty.Enabled = false;
                txtModelRate.Enabled = false;
                txtChangeQty.Enabled = false;

                txtAppID.Enabled = false;
                txtAppNo.Enabled = false;
                txtAppDate.Enabled = false;
                txtAppDisc.Enabled = false;
                txtAppDealershare.Enabled = false;
                txtAppMTIshare.Enabled = false;
                txtAppremarks.Enabled = false;
                txtAppFinalAmt.Enabled = false;
                txtPaymentDetailsPO.Enabled = false;

                AppDet.Visible = true;
            }
            else
            {
                drpM7Det.SelectedValue = "0";
                txtCustName.Text = "";
                drpM7Det.Enabled = false;
                txtCustName.Enabled = false;
                drpModelCat.Enabled = true;
                drpModel.Enabled = true;
                drpModelCode.Enabled = true;
                drpModelGroup.Enabled = true;
                txtQty.Enabled = true;
                txtPaymentDetailsPO.Enabled = true;

                drpModel.SelectedValue = "0";
                drpModelCat.SelectedValue = "0";
                drpModelCode.SelectedValue = "0";
                drpModelGroup.SelectedValue = "0";
                txtQty.Text = "";
                txtModelRate.Text = "";
                txtChangeQty.Text = "";
                txtPaymentDetailsPO.Text = "";


                txtAppID.Enabled = false;
                txtAppNo.Enabled = false;
                txtAppDate.Enabled = false;
                txtAppDisc.Enabled = false;
                txtAppDealershare.Enabled = false;
                txtAppMTIshare.Enabled = false;
                txtAppremarks.Enabled = false;
                txtAppFinalAmt.Enabled = false;
                txtModelRate.Enabled = false;
                txtChangeQty.Enabled = false;
                AppDet.Visible = false;


            }
        }

        protected void drpM7Det_SelectedIndexChanged(object sender, EventArgs e)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();

                if (Func.Convert.iConvertToInt(drpM7Det.SelectedValue) != 0)
                {


                    //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerId, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPODealer", iDealerId, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                    if (ds != null)
                    {
                        txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
                        drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                        drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                        drpModelGroup.SelectedValue = "1";
                        FillModel();
                        drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                        txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]); ;
                        txtChangeQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                        drpPlant.SelectedValue = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);
                        drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Finc"]);
                        // Appproval Details
                        txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                        txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approval_No"]);
                        txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Approval_Date"], false);
                        txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount_ApprovedAmt"]);
                        txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_DealerShare"]);
                        txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_MTIShare"]);
                        txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["remarks"]);
                        txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalAmt"]);
                    }


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

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            txtChangeQty.Text = txtQty.Text;
        }
        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {

            Location.iDealerId = PDoc.iDealerID;
            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
            Location.FillLocation();
            FillCombo();
            FillPlant();
            FillSelectionGrid();
            
            PSelectionGrid.Style.Add("display", "none");
            txtID.Text = "";

            GetDataFromPO();
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }

        private void GetDataFromPO()
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

                DataSet ds = new DataSet();
                int iID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iID != 0)
                {
                    ds = GetPO(iID, "All", iDealerId, iHOBrId, 0);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //ObjDealer = null;
                }

                //if (iUserRole == 2)
                //{
                //    txtM3ID.Text = "Y";
                //    GenerateLeadNo("POA");
                //}
                if (iUserRole == 9)
                {
                    txtM3ID.Text = "Y";
                    GenerateLeadNo("POA");
                    //txtM3ID.Text = "N";
                }
                //txtID.Text = "";
                ////txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "VORF", Location.iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, (txtID.Text == "" || txtID.Text == "0") ? false : true);
                //ds = null;
                //ObjProforma = null;

              
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void bModifyPO(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                if (txtHoldRemarks.Text=="")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Remarks');</script>");
                    return;
                }
                txtHoldRemarks.Enabled = true;

                if (bChangeVehCondition(objDB, iDealerId, iID, iUserRole, txtHoldRemarks.Text) == true)
                {
                    bSaveRecord = true;
                }


                if (bSaveRecord == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PO Submittedto Dealer for Modification!!');</script>");
                }
               

                FillSelectionGrid();
                GetDataAndDisplay();
            }
        }

        public bool bChangeVehCondition(clsDB objDB, int dealerid, int iHdrID,int Userrole,string Remarks)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {


               

                if (iHdrID > 0)
                {
                    objDB.BeginTranasaction();

                    objDB.ExecuteStoredProcedure("SP_ModifyDealerPO", dealerid, iHdrID,Userrole,Remarks
                        );

                    objDB.CommitTransaction();

                }





                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

       
    }
}