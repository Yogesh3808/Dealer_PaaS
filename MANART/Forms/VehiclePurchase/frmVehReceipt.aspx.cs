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
    public partial class frmVehReceipt : System.Web.UI.Page
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
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
             
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iHOBrId = iDealerId;
                    HOBrID = iDealerId;

                    //txtDealerCode.Text = Session["sDealerCodeLoc"].ToString();
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                }



                ToolbarC.iValidationIdForSave = 65;
               
                if (!IsPostBack)
                {
                    FillCombo();
                    DisplayPreviousRecord();
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                }
                SearchGrid.sGridPanelTitle = "Vehicle PO (Dealer) Detials";
                FillSelectionGrid();
                if (iID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
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

                if (txtUserType.Text == "6")
                {
                    Location.DDLSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                }
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
            DisplayPreviousRecord();
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

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;



                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "");
                    DisplayPreviousRecord();
                    GenerateLeadNo("VR");
                    Session["VR"] = 0;

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                    drpInvNo.Style.Add("display", "");
                    txtDMSInvNo.Style.Add("display", "none");
                    FillInv();


                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;

                    if (drpVehConditon.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Vehicle Condition ');</script>");
                        return;
                    }
                  else if (txtDeliveryNo.Text=="")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Delivery NO');</script>");
                        return;
                    }
                    else if (txtPONo.Text== "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Po NO');</script>");
                        return;
                    }
                  
                     string Temp = Session["VR"].ToString();
                     if (Temp == "0" || iID != 0)
                     {
                         Session["VR"] = 1;
                         iID = bHeaderSave("N", "N", "N");
                     }

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        //if (bSaveDetails("N", "N", "N") == true)
                        //{
                     //   Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Receipt NO ") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        //}


                    }
                    else
                    {
                        if (Temp == "0" && iID == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                        else
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Receipt NO ") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        }

                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm/Close
                {
                    //string OrderStatus = "";
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    if (drpVehConditon.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Vehicle Condition ');</script>");
                        return;
                    }


                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        //if (bSaveDetails("N", "Y", "N") == true)
                        //{
                        ////    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Receipt NO ") + "','" + Server.HtmlEncode(txtMReceiptNo.Text) + "');</script>");
                        //}
                      
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }



                }

                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;




                    iID = bHeaderSave("Y", "N", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        //if (bSaveDetails("N", "N", "Y") == true)
                        //{
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        //}
                       
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }

                }

                FillSelectionGrid();
                GetDataAndDisplay();

            }

            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);

            Func.Common.BindDataToCombo(drpVehConditon, clsCommon.ComboQueryType.VehicleCondition, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);

            drpModelGroup.SelectedValue = "1";



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


            if (Type == "VR")
            {

                txtMReceiptNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "VR"));

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
                if (Type == "VR")
                {
                    sDocName = "VR";
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerVehReceipt", POId, POType, DealerID, HOBrID);
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




        private void FillInv()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            DataTable dtInvoice = null;
            //objMatReceipt = new clsMaterialReceipt();
            try
            {
                dtInvoice = new DataTable();
                dtInvoice = objDB.ExecuteStoredProcedureAndGetDataTable("GetVehInvoiceforDealerPO", iDealerId);

                drpInvNo.DataValueField = "ID";
                drpInvNo.DataTextField = "Invoice_No";
                drpInvNo.DataSource = dtInvoice;
                drpInvNo.DataBind();
                drpInvNo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (dtInvoice != null) dtInvoice = null;
                if (objDB != null) objDB = null;
            }

        }


        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "Dealer Vehicle Receipt List";
                SearchGrid.AddToSearchCombo("Receipt No");
                SearchGrid.AddToSearchCombo("Receipt Date");
                SearchGrid.AddToSearchCombo("PO Details");
                SearchGrid.AddToSearchCombo("Chassis No");
                SearchGrid.AddToSearchCombo("Vehicle Condition");
                SearchGrid.AddToSearchCombo("Status");
                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    SearchGrid.iDealerID = iDealerId;
                    SearchGrid.iBrHODealerID = iHOBrId;
                }

                SearchGrid.sSqlFor = "DealerVehReceipt";
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
                    Session["VR"] = 1;
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


                drpInvNo.Style.Add("display", "none");
                txtDMSInvNo.Style.Add("display", "");

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



                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtMReceiptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tr_no"]);
                    txtMReceiptDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["ref_date"], false);
                    txtDMSInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["inv_no"]);
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["inv_date"], false);
                    txtDeliveryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["delivery_No"]);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_date"], false);
                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["chassis_no"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["engine_no"]);



                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_GP"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["rate"]);
                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TotalAmt"]);
                    txtParking.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ParkingLocation"]);
                    drpVehConditon.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehConditionID"]);


                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TotalAmt"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);

                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VatDesc"]);
                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2Desc"]);

                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_Amt"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_Amt"]);
                    txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                    txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PF_Charges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other_Charges"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Amt"]);
                   // txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["inv_amt"]);

                    txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_TaxID"]);
                    txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_TaxID"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1_TaxID"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2_TaxID"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCH"]);
                    Addtax.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1_TaxID"]);
                    HiddenGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehGST"]);
                    //txtGrandTotal.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["inv_amt"]), 2).ToString();
                     txtGrandTotal.Text=  Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["inv_amt"]);
                    

                }




                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]);

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GST"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehGST"]) != "N")
                {
                    tdtaxper.Visible = false; tdlbl.Visible = false; tdamount.Visible = false; tdlbl.Visible = false;
                }
                else
                {   tdtaxper.Visible = true; tdlbl.Visible = true; tdamount.Visible = true; lblCST.Visible = true; txtCSTPer.Visible = true; txtCSTAmt.Visible = true;  }

                if (hdnConfirm.Value == "Y")
                {
                    if (drpVehConditon.SelectedValue != "1" && drpVehConditon.SelectedValue != "0")
                    {
                        bChangeToSaleable.Visible = true;
                    }
                    else
                    {
                        bChangeToSaleable.Visible = false;
                    }
                }
                else
                {
                    bChangeToSaleable.Visible = false;
                }



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
                //else if (hdnLost.Value == "Y")
                //{
                //    MakeEnableDisableControls(false, "Lost");
                //}



                //if (Func.Convert.iConvertToInt(txtID.Text) != 0 && (bEnableControls == true))
                //{

                //    if (Func.Convert.iConvertToInt(txtInqID.Text) != 0)
                //    {
                //        //bConvertToInq.Enabled = false;
                //        //bHold.Enabled = false;
                //        //MakeEnableDisableControls(false, "Nothing");
                //    }
                //    else
                //    {
                //        //bConvertToInq.Enabled = true;
                //        //bHold.Enabled = true;
                //    }

                //}
                //else if (hdnCancle.Value == "Y")
                //{
                //    //bConvertToInq.Enabled = false;
                //    //bHold.Enabled = false;
                //}
                //else if (hdnConfirm.Value == "Y")
                //{
                //    //bConvertToInq.Enabled = false;
                //    //bHold.Enabled = false;
                //}




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

            if (iID == 0)
            {
                GenerateLeadNo("VR");
            }



            int HdrID = 0;

            HdrID = objLead.bSaveDealerVehReceipt(iID, iDealerId, HOBrID,
                txtMReceiptNo.Text, txtMReceiptDate.Text, txtDMSInvNo.Text, txtDMSInvDate.Text, txtDeliveryNo.Text, txtPONo.Text, txtPODate.Text,
                txtGrandTotal.Text, txtChassisNo.Text, txtEngineNo.Text, Func.Convert.iConvertToInt(drpModel.SelectedValue),
                Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), txtModelRate.Text, txtQty.Text, txtTotalAmt.Text, txtDisc.Text,
                Func.Convert.iConvertToInt(txtCSTID.Text), txtCSTAmt.Text, Func.Convert.iConvertToInt(txtVATID.Text), txtVatAmt.Text,
                Func.Convert.iConvertToInt(Addtax.Text), txttax1Amt.Text, Func.Convert.iConvertToInt(txtTax2ID.Text), txttax2Amt.Text, 
                txtTCSPer.Text, txtTCS.Text, txtParking.Text, Func.Convert.iConvertToInt(drpVehConditon.SelectedValue), Confirm
                , Cancel, txtPFCharges.Text, txtOthercharges.Text, Func.Convert.iConvertToInt(drpModelCat.SelectedValue),HiddenGST.Value
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


            txtPONo.Enabled = false;
            txtPODate.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelGroup.Enabled = false;
            drpModelCat.Enabled = false;
            txtQty.Enabled = false;
            txtModelRate.Enabled = false;
            txtTotalAmt.Enabled = false;
            txtChassisNo.Enabled = false;
            txtEngineNo.Enabled = false;
            txtMReceiptNo.Enabled = false;
            txtMReceiptDate.Enabled = false;
            txtDMSInvNo.Enabled = false;
            txtDMSInvDate.Enabled = false;
            txtDeliveryNo.Enabled = false;
            txtParking.Enabled = bEnable;
            drpVehConditon.Enabled = bEnable;



            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
          //  ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);
             ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);



        }





        protected void drpInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDMSInvNo.Text = Func.Convert.sConvertToString(drpInvNo.SelectedItem);
            drpInvNo.Style.Add("display", "none");
            txtDMSInvNo.Style.Add("display", "");
            txtDeliveryNo.Enabled = true;



        }

        protected void ChangeToSaleable(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            if (iID > 0)
            {

                if (bChangeVehCondition(objDB, iDealerId, iHOBrId, iID) == true)
                {
                    bSaveRecord = true;
                }


                if (bSaveRecord==true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Vehicle Condition Changed To Saleable');</script>");
                }
                else
                {

                }

                FillSelectionGrid();
                GetDataAndDisplay();
            }

        }
        public bool bChangeVehCondition(clsDB objDB, int dealerid, int hobrId, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {




                if (iHdrID > 0)
                    {
                        objDB.BeginTranasaction();

                        objDB.ExecuteStoredProcedure("SP_ChangeToSaleable", dealerid, hobrId, iHdrID
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


        protected void txtDeliveryNo_TextChanged(object sender, EventArgs e)
        {
           
            
            //objMatReceipt = new clsMaterialReceipt();
            FillDeliveryDetails();

        }

        private void FillDeliveryDetails()
        {
            try
            {
                clsDB objDB = new clsDB();
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetVehInvallDetailsDealerPO", iDealerId, Func.Convert.iConvertToInt(drpInvNo.SelectedValue), txtDeliveryNo.Text);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtDeliveryNo.Enabled = false;
                    txtDMSInvDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Invoice_Date"], false);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Rate"]);
                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Rate"]);
                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_No"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_No"]);

                    txtTaxTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Total"]);
                    txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);

                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VatDesc"]);
                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTDesc"]);
                    //lblSGST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGSTDesc"]);
                    //lblIGST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGSTDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Add2Desc"]);

                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATAmt"]);
                    txtCSTAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTAmt"]);
                    HiddenGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsInvGST"]);
                    //txtSGCTamt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGSTAmt"]);
                    //txtIGST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGSTAmt"]);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsInvGST"]) == "Y")
                    {
                        tdtaxper.Visible = false; tdlbl.Visible = false; tdamount.Visible = false; tdlbl.Visible = false;
                    }
                    else
                    { tdtaxper.Visible = true; tdlbl.Visible = true; tdamount.Visible = true; lblCST.Visible = true; txtCSTPer.Visible = true; txtCSTAmt.Visible = true; }
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsInvGST"]) == "N")
                    {
                        txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                        Addtax.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2ID"]);
                        txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATSURCHAMT"]);
                        txttax2Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ADDVATAMT"]);
                        txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                    }
                    else
                    {
                        Addtax.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1ID"]);
                        txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax_CGST"]);
                        txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax_CGST"]);
                        txttax1Amt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Taxamt_CGST"]);

                    }



                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PF_Charges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other_Charges"]);
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);
                    txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Amt"]);
                    txtGrandTotal.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]), 2).ToString();
                    txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST_TaxID"]);
                    txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT_TaxID"]);
                    //txtSGSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGST_TaxID"]);
                    // txtIGSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGST_TaxID"]);







                }
                else
                {
                    txtDeliveryNo.Enabled = true;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Correct Delivery No !');</script>");
                }



            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                //if (ds != null) ds = null;
                //if (objDB != null) objDB = null;
            }
        }

       
     
    }
}