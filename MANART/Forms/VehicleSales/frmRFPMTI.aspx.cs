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
    public partial class frmRFPMTI : System.Web.UI.Page
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
        int iUserRole = 0;
        int iUserPCRHeadApprID = 0;
        int iUserSQHResourceId = 0;
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
                iUserRole = Func.Convert.iConvertToInt(Session["UserRole"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                iUserSQHResourceId = Func.Convert.iConvertToInt(Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetUserVehPOApprv, 0, "")));


                iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Common.sGetMultiUserAccess(Func.Convert.iConvertToInt(Session["UserID"]), iMenuId));

                iUserId = (iUserId == iUserPCRHeadApprID) ? iUserSQHResourceId : iUserId;
                iUserRole = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 9 : iUserRole;


                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                //iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                

                iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);

                if (iHOBrId == 0)
                {
                    FindHOBr(iDealerId);
                }

                PDoc.sFormID = "67";
                Location.bUseSpareDealerCode = false;

                if (!IsPostBack)
                {
                    FillCombo();

                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "Request For Price (RFP) Details";
                FillSelectionGrid();

                if (iID != 0)
                {
                    GetDataAndDisplay();
                }

                if (txtID.Text == "")
                {


                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);



                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
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

                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
                Location.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                if (!IsPostBack)
                {
                    SearchGrid.bIsCollapsable = false;
                }
                if (iUserRole == 9)
                {
                    ToolbarC.Visible = true;
                }
                else
                {
                    ToolbarC.Visible = false;
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
            //ClearDealerHeader();
            DisplayPreviousRecord();





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
                    //PSelectionGrid.Style.Add("display", "");

                    //DisplayPreviousRecord();


                    ////GenerateLeadNo("RF");
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

                    //MakeEnableDisableControls(true, "Nothing");

                    //return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;





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



                    if (iID == 0)
                    {


                        GenerateLeadNo("RF");
                    }

                    iID = bHeaderSave("N", "N", "N");

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        //if (bSaveDetails("N", "N", "N") == true)
                        //{
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        //}
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("RFP") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");

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
                            // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("RFP") + "','" + Server.HtmlEncode(txtPONo.Text) + "');</script>");
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




                    iID = bHeaderSave("Y", "N", "N");

                     if (Func.Convert.iConvertToInt(drpM7Det.SelectedValue)!= 0)
                    {
                         FunCloseM1 (Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                    }
                     iID = Func.Convert.iConvertToInt(txtID.Text);

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        //if (bSaveDetails("N", "N", "Y") == true)
                        //{
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        //}
                        
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('RFP " + txtPONo.Text + " Rejected');</script>");
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
        private void FunCloseM1(int M1ID)
        {
            clsDB objDB = new clsDB();
            try
            {

               
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_CloseM1fromRFP", M1ID, iDealerId);

                objDB.CommitTransaction();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            //Func.Common.BindDataToCombo(drpM4Financier, clsCommon.ComboQueryType.Financier, 0);
            //Func.Common.BindDataToCombo(drpVehPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
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

        }


        protected void drpModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpModelCode.SelectedValue = drpModel.SelectedValue;



        }
        private void FillModel()
        {

            int modelgrp = 0;

            modelgrp = Func.Convert.iConvertToInt(drpModelCat.SelectedValue);



            //Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //    + iDealerId + "and " + "'" + txtPODate.Text + "'"
            //    + " not between convert(varchar(10),Effective_From,103) and convert(varchar(10),Effective_To,103) )   "
            //    );



            //Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //  + iDealerId + "and " + "'" + txtPODate.Text + "'"
            //  + " not between convert(varchar(10),Effective_From,103) and convert(varchar(10),Effective_To,103))    "
            //  );



            //drpModelGroup.SelectedValue = "1";


            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);



            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);


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


            if (Type == "RFA")
            {

                txtPOAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "RFA"));

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
                if (Type == "RFA")
                {
                    sDocName = "RFA";
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


                ds = GetPO(iID, "Max", iDealerId, iHOBrId, 0);
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetRFP", POId, POType, DealerID, HOBrID, iM1ID);
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

                DataSet ds = new DataSet();
                int iID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                if (iID != 0)
                {
                    ds = GetPO(iID, "All", iDealerId, iHOBrId, 0);
                    DisplayData(ds);
                }

                if (iUserRole == 9)
                {
                    txtM3ID.Text = "Y";
                    GenerateLeadNo("RFA");
                }
                else
                {
                    txtM3ID.Text = "N";
                }
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
                SearchGrid.sGridPanelTitle = "Requset For Price (RFP) List";
                SearchGrid.AddToSearchCombo("RFP No");
                SearchGrid.AddToSearchCombo("RFP Date");
                //SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("Status");

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


                SearchGrid.iDealerID = iDealerId;
                SearchGrid.iBrHODealerID = iHOBrId;
                SearchGrid.sSqlFor = "RFPDetMTI";
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
                    drpModelGroup.SelectedValue = "1";
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Cat"]);
                    drpModelGroup.SelectedValue = "1";

                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);

                    txtPONo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RFP_No"]);
                    txtPODate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["RFP_Date"], false);


                    int M7Id = 0;

                    if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["M0_Id"]) != 0)
                    {
                        M7Id = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["M0_Id"]);
                        FillEnquirySave(M7Id);
                        drpM7Det.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_Id"]);

                    }
                    else
                    {
                        FillEnquiry();
                        drpM7Det.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M0_Id"]);

                    }

                    txtPaymentDetailsPO.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remarks"]);

                    txtPOAppNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppNo"]);
                    txtPOApppDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["AppDate"], false);
                    txtAppRemarks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AppRemarks"]);
                    txtMRP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MRP"]);
                    txtList.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["List"]);


                }
                hideentaxID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax_ID"]);
                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["App_Confirm"]);
                hdnCancle.Value = "N";
                hdnLost.Value = "N";








                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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

                txttaxdetails.Text = "";
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txttaxdetails.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Taxdetails"]);
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

            //RFP Rej
            //HdrID = objLead.bSaveRFP(iID, iDealerId, HOBrID, txtPONo.Text, txtPODate.Text, "Y", "N",
            //     Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
            //    Func.Convert.iConvertToInt(drpModel.SelectedValue), txtPOAppNo.Text, txtPOApppDate.Text, iMenuId, "Y", Confirm, iUserRole,
            // Func.Convert.iConvertToInt(drpModelCat.SelectedValue), txtPaymentDetailsPO.Text, txtM3ID.Text, txtAppRemarks.Text,
            // Func.Convert.dConvertToDouble(txtList.Text), Func.Convert.dConvertToDouble(txtMRP.Text), 1,"N"
            // );

            if (Cancel == "Y")
            {
                HdrID = objLead.bSaveRFP(iID, iDealerId, HOBrID, txtPONo.Text, txtPODate.Text, "Y", "N",
                   Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                  Func.Convert.iConvertToInt(drpModel.SelectedValue), txtPOAppNo.Text, txtPOApppDate.Text, iMenuId, "J", Confirm, iUserRole,
               Func.Convert.iConvertToInt(drpModelCat.SelectedValue), txtPaymentDetailsPO.Text, txtM3ID.Text, txtAppRemarks.Text,
               Func.Convert.dConvertToDouble(txtList.Text), Func.Convert.dConvertToDouble(txtMRP.Text), 1, "N"
               );
            }

            else
            {
                HdrID = objLead.bSaveRFP(iID, iDealerId, HOBrID, txtPONo.Text, txtPODate.Text, "Y", "N",
                     Func.Convert.iConvertToInt(drpM7Det.SelectedValue),
                    Func.Convert.iConvertToInt(drpModel.SelectedValue), txtPOAppNo.Text, txtPOApppDate.Text, iMenuId, "Y", Confirm, iUserRole,
                 Func.Convert.iConvertToInt(drpModelCat.SelectedValue), txtPaymentDetailsPO.Text, txtM3ID.Text, txtAppRemarks.Text,
                 Func.Convert.dConvertToDouble(txtList.Text), Func.Convert.dConvertToDouble(txtMRP.Text), 1, "N"
                 );
            }


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

            drpM7Det.Enabled = false;

            drpModelCat.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            txtPaymentDetailsPO.Enabled = false;

            txtPOAppNo.Enabled = false;
            txtPOApppDate.Enabled = false;
            txtMRP.Enabled = bEnable;
            txtList.Enabled = bEnable;
            txtAppRemarks.Enabled = bEnable;

            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarTyphttp://localhost:52916/Forms/VehicleSales/frmRFPMTI.aspx?MenuID=720e.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }

        private void FillEnquiry()
        {
            drpM7Det.ClearSelection();
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM0forRFP", iDealerId);

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
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSaveM0forRFP", iDealerId, M7ID);

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



        protected void drpM7Det_SelectedIndexChanged(object sender, EventArgs e)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();


                drpModel.SelectedValue = "0";
                drpModelCode.SelectedValue = "0";
                drpModelGroup.SelectedValue = "0";
                drpModelCat.SelectedValue = "0";


                txtPaymentDetailsPO.Text = "";



                //FillEnquiry();
                drpM7Det.Enabled = true;

                drpModel.Enabled = true;
                drpModelCode.Enabled = true;
                drpModelGroup.Enabled = true;

                txtPaymentDetailsPO.Enabled = true;

                if (Func.Convert.iConvertToInt(drpM7Det.SelectedValue) != 0)
                {


                    ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPODealer", iDealerId, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                    if (ds != null)
                    {

                        drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                        drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                        drpModelGroup.SelectedValue = "1";
                        FillModel();
                        drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);

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

        protected void txtList_TextChanged(object sender, EventArgs e)
                {
            try
            {
                FillTaxDet();
                clsDB objDB = new clsDB();
                DataSet dsTaxCal = new DataSet();
                dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_RFPTaxDetails", hideentaxID.Value, Func.Convert.iConvertToInt(txtList.Text));
                if (dsTaxCal != null)
                {
                 //   txtMRP.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["Tax1Amt"]), 0).ToString();

                }
            }
            catch (Exception)
            {


            }
        }

        private void FillTaxDet()
        {
            try
            {
                clsDB objDB = new clsDB();

                DataSet dsCustType = new DataSet();
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxes", iDealerId, Func.Convert.iConvertToInt(hideentaxID.Value));


                if (dsCustType != null)
                {
                    DataSet dsTaxCal = new DataSet();
                    dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_TaxCalculationRFP", Func.Convert.iConvertToInt(hideentaxID.Value)
                    , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax1ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax2ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp"])
                    , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp1"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp2"]), Func.Convert.dConvertToDouble(txtList.Text),
                Func.Convert.dConvertToDouble(0));
                    txtMRP.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]) + Func.Convert.dConvertToDouble(txtList.Text) + Func.Convert.dConvertToDouble(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]) + Func.Convert.dConvertToDouble(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]));
                    double grandtotal = Math.Round(Convert.ToDouble(txtMRP.Text), 0);
                    txtMRP.Text = grandtotal.ToString();
                }

            }
            catch (Exception ex) { }
        }
    }
}