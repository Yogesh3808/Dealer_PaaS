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
    public partial class frmBranchPurchasePO : System.Web.UI.Page
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

                //ToolbarC.iValidationIdForSave = 65;
                ToolbarC.iFormIdToOpenForm = 26;



                if (!IsPostBack)
                {
                    FillCombo();
                    //FillPlant();
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


                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
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
                    //FillPlant();
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
            //FillPlant();
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
                    Session["TmpBranchPO"] = 0;

                    PSelectionGrid.Style.Add("display", "");

                    DisplayPreviousRecord();


                    GenerateLeadNo("STP");
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //  ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                    MakeEnableDisableControls(true, "Nothing");

                    return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {

                    string TmpBranchPO = Session["TmpBranchPO"].ToString();

                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;


                    if(iID==0)
                    {

                        GenerateLeadNo("STP");
                    }

                    if (txtPODate.Text == "" || txtPODate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter PO Date');</script>");
                        return;
                    }




                    if (drpModelCat.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Model Category');</script>");
                        return;
                    }


                    if (drpModelCode.SelectedValue == "0" || drpModel.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Model');</script>");
                        return;
                    }


                    if (txtQty.Text == "0" || txtQty.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Quantity');</script>");
                        return;
                    }

                    if (TmpBranchPO == "0" || iID != 0)
                    {

                        Session["TmpBranchPO"] = 1;
                        iID = bHeaderSave("N", "N", "N");
                  

                        PSelectionGrid.Style.Add("display", "");
                        if (iID > 0)
                        {
                            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                            txtID.Text = Func.Convert.sConvertToString(iID);

                            //if (bSaveDetails("N", "N", "N") == true)
                            //{
                            //   Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            //}
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");

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

                        //if (bSaveDetails("N", "Y", "N") == true)
                        //{
                      //  Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
                        //}
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("PO ") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
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
                           Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled');</script>");
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
            Func.Common.BindDataToCombo(drpSupplier, clsCommon.ComboQueryType.Dealer_Supplier, iDealerId, " and Sup_Type=16 ");
            //Func.Common.BindDataToCombo(drpVehPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);

            drpModelGroup.SelectedValue = "1";


        }
        //private void FillPlant()
        //{


        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet dsCustType = new DataSet();


        //        //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
        //        dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPlantCode");

        //        //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
        //        if (dsCustType != null)
        //        {
        //            drpPlant.DataSource = dsCustType.Tables[0];
        //            drpPlant.DataTextField = "Name";
        //            drpPlant.DataValueField = "ID";
        //            drpPlant.DataBind();
        //            drpPlant.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }




        //}


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
                if (dsCustType.Tables[0].Rows.Count > 0)
                {
                    txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                    //drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
                }
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
                if (dsCustType.Tables[0].Rows.Count > 0)
                {
                    txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                    //drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
                }
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
        private void FillSaveSupplier(int ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSaveStockTransPOSupplier", ID, iDealerId, HOBrID);


                if (dsCustType != null)
                {
                    drpSupplier.DataSource = dsCustType.Tables[0];
                    drpSupplier.DataTextField = "Name";
                    drpSupplier.DataValueField = "ID";
                    drpSupplier.DataBind();
                    //drpChassisNo.Items.Insert(0, new ListItem("--Select--", "0"));
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


            if (Type == "STP")
            {

                txtPONo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "STP"));

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
                if (Type == "STP")
                {
                    sDocName = "STP";
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
                MakeEnableDisableControls(false, "Nothing");
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetStockTransferPO", POId, POType, DealerID, HOBrID);
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
                SearchGrid.sGridPanelTitle = "Branch Sale PO List";
                SearchGrid.AddToSearchCombo("PO No");
                SearchGrid.AddToSearchCombo("PO Date");
                SearchGrid.AddToSearchCombo("Supplier");
                SearchGrid.AddToSearchCombo("PO Status");
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
                SearchGrid.sSqlFor = "StkTransSalePO";
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
                Session["TmpBranchPO"] = 0;
                
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



                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_GP"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["qty"]);
                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["PO_Date"], false);

                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);


                    if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SuppID"]) != 0)
                    {

                        FillSaveSupplier(Func.Convert.iConvertToInt(txtID.Text));
                    }
                    else
                    {
                        FillCombo();


                        drpSupplier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SuppID"]);
                    }


                }




                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]); ;
                hdnLost.Value = "N";








                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
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

        public int bHeaderSave(string Cancel, string Confirm, string OrderStatus)
        {

            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            int HdrID = 0;

            HdrID = objLead.bSaveStockTransferPO(iID, iDealerId, HOBrID, Func.Convert.iConvertToInt(drpSupplier.SelectedValue), txtPONo.Text, txtPODate.Text,
               Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), Func.Convert.iConvertToInt(drpModel.SelectedValue), 1,
              Func.Convert.dConvertToDouble(txtModelRate.Text), Confirm, Cancel, Func.Convert.iConvertToInt(drpModelCat.SelectedValue)
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

            drpModelCat.Enabled = bEnable;
            drpModel.Enabled = bEnable;
            drpModelCode.Enabled = bEnable;
            drpModelGroup.Enabled = bEnable;
            txtQty.Enabled = false;

            drpSupplier.Enabled = bEnable;

            txtModelRate.Enabled = bEnable;





        }

        //private void FillEnquiry()
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet dsCustType = new DataSet();


        //        //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
        //        dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquiryNew", iDealerId, HOBrID);

        //        //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
        //        if (dsCustType != null)
        //        {
        //            drpM7Det.DataSource = dsCustType.Tables[0];
        //            drpM7Det.DataTextField = "Name";
        //            drpM7Det.DataValueField = "ID";
        //            drpM7Det.DataBind();
        //            drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }


        //}

        //private void FillEnquirySave(int M7ID)
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet dsCustType = new DataSet();


        //        //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
        //        dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquirySavenew", iDealerId, HOBrID, M7ID);

        //        //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
        //        if (dsCustType != null)
        //        {
        //            drpM7Det.DataSource = dsCustType.Tables[0];
        //            drpM7Det.DataTextField = "Name";
        //            drpM7Det.DataValueField = "ID";
        //            drpM7Det.DataBind();
        //            //drpM7Det.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }


        //}

        //protected void drpVehPOType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Func.Convert.iConvertToInt(drpVehPOType.SelectedValue) == 1)
        //    {
        //        //drpM7Det.SelectedValue = "0";
        //        //txtCustName.Text = "";
        //        drpModel.SelectedValue = "0";
        //        drpModelCode.SelectedValue = "0";
        //        drpModelGroup.SelectedValue = "0";
        //        txtQty.Text = "";
        //        txtModelRate.Text = "";
        //        txtChangeQty.Text = "";
        //        txtPaymentDetailsPO.Text = "";

        //        txtAppID.Text = "";
        //        txtAppNo.Text = "";
        //        txtAppDate.Text = "";
        //        txtAppDisc.Text = "";
        //        txtAppDealershare.Text = "";
        //        txtAppMTIshare.Text = "";
        //        txtAppremarks.Text = "";
        //        txtAppFinalAmt.Text = "";
        //        drpM4Financier.SelectedValue = "0";
        //        txtLocFinc.Text = "";

        //        FillEnquiry();
        //        drpM7Det.Enabled = true;
        //        txtCustName.Enabled = true;
        //        //drpModel.Enabled = false;
        //        //drpModelCode.Enabled = false;
        //        //drpModelGroup.Enabled = false;
        //        //txtQty.Enabled = false;
        //        //txtModelRate.Enabled = false;
        //        txtModelRate.Enabled = true;
        //        drpModel.Enabled = true;
        //        drpModelCode.Enabled = true;
        //        drpModelGroup.Enabled = true;
        //        txtQty.Enabled = true;
        //        txtPaymentDetailsPO.Enabled = true;

        //        //txtChangeQty.Enabled = false;

        //        txtAppID.Enabled = false;
        //        txtAppNo.Enabled = false;
        //        txtAppDate.Enabled = false;
        //        txtAppDisc.Enabled = false;
        //        txtAppDealershare.Enabled = false;
        //        txtAppMTIshare.Enabled = false;
        //        txtAppremarks.Enabled = false;
        //        txtAppFinalAmt.Enabled = false;
        //        //txtPaymentDetailsPO.Enabled = false;

        //        AppDet.Visible = true;
        //    }
        //    else
        //    {
        //        drpM7Det.SelectedValue = "0";
        //        txtCustName.Text = "";
        //        drpM7Det.Enabled = false;
        //        txtCustName.Enabled = false;

        //        drpModel.Enabled = true;
        //        drpModelCode.Enabled = true;
        //        drpModelGroup.Enabled = true;
        //        txtQty.Enabled = true;
        //        txtPaymentDetailsPO.Enabled = true;

        //        drpModel.SelectedValue = "0";
        //        drpModelCode.SelectedValue = "0";
        //        drpModelGroup.SelectedValue = "0";
        //        txtQty.Text = "";
        //        txtModelRate.Text = "";
        //        txtChangeQty.Text = "";
        //        txtPaymentDetailsPO.Text = "";
        //        txtAppID.Text = "";
        //        txtAppNo.Text = "";
        //        txtAppDate.Text = "";
        //        txtAppDisc.Text = "";
        //        txtAppDealershare.Text = "";
        //        txtAppMTIshare.Text = "";
        //        txtAppremarks.Text = "";
        //        txtAppFinalAmt.Text = "";
        //        drpM4Financier.SelectedValue = "0";
        //        txtLocFinc.Text = "";

        //        txtAppID.Enabled = false;
        //        txtAppNo.Enabled = false;
        //        txtAppDate.Enabled = false;
        //        txtAppDisc.Enabled = false;
        //        txtAppDealershare.Enabled = false;
        //        txtAppMTIshare.Enabled = false;
        //        txtAppremarks.Enabled = false;
        //        txtAppFinalAmt.Enabled = false;
        //        txtModelRate.Enabled = false;
        //        txtChangeQty.Enabled = false;
        //        AppDet.Visible = false;


        //    }
        //}

        //protected void drpM7Det_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds = new DataSet();


        //        drpModel.SelectedValue = "0";
        //        drpModelCode.SelectedValue = "0";
        //        drpModelGroup.SelectedValue = "0";
        //        txtQty.Text = "";
        //        txtModelRate.Text = "";
        //        txtChangeQty.Text = "";
        //        //txtPaymentDetailsPO.Text = "";

        //        //txtAppID.Text = "";
        //        //txtAppNo.Text = "";
        //        //txtAppDate.Text = "";
        //        //txtAppDisc.Text = "";
        //        //txtAppDealershare.Text = "";
        //        //txtAppMTIshare.Text = "";
        //        //txtAppremarks.Text = "";
        //        //txtAppFinalAmt.Text = "";
        //        //drpM4Financier.SelectedValue = "0";
        //        //txtLocFinc.Text = "";

        //        ////FillEnquiry();
        //        //drpM7Det.Enabled = true;
        //        //txtCustName.Enabled = true;
        //        //drpModel.Enabled = false;
        //        //drpModelCode.Enabled = false;
        //        //drpModelGroup.Enabled = false;
        //        //txtQty.Enabled = false;
        //        //txtModelRate.Enabled = false;
        //        txtModelRate.Enabled = true;
        //        drpModel.Enabled = true;
        //        drpModelCode.Enabled = true;
        //        drpModelGroup.Enabled = true;
        //        txtQty.Enabled = true;
        //        //txtPaymentDetailsPO.Enabled = true;

        //        //txtChangeQty.Enabled = false;

        //        txtAppID.Enabled = false;
        //        txtAppNo.Enabled = false;
        //        txtAppDate.Enabled = false;
        //        txtAppDisc.Enabled = false;
        //        txtAppDealershare.Enabled = false;
        //        txtAppMTIshare.Enabled = false;
        //        txtAppremarks.Enabled = false;
        //        txtAppFinalAmt.Enabled = false;
        //        //txtPaymentDetailsPO.Enabled = false;

        //        AppDet.Visible = true;




        //        if (Func.Convert.iConvertToInt(drpM7Det.SelectedValue) != 0)
        //        {


        //            ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPODealer", iDealerId, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
        //            if (ds != null)
        //            {
        //                txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["name"]);
        //                drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
        //                FillModel();
        //                drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
        //                drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
        //                txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
        //                txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]); ;
        //                txtChangeQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
        //                drpPlant.SelectedValue = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);
        //                drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Finc"]);
        //                // Appproval Details
        //                txtAppID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
        //                txtAppNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approval_No"]);
        //                txtAppDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Approval_Date"], false);
        //                txtAppDisc.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Discount_ApprovedAmt"]);
        //                txtAppDealershare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_DealerShare"]);
        //                txtAppMTIshare.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Approved_MTIShare"]);
        //                txtAppremarks.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["remarks"]);
        //                txtAppFinalAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalAmt"]);
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}


    }
}