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
    public partial class frmM2 : System.Web.UI.Page
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
        int iMenuId = 0;
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
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                HOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                

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


                //ToolbarC.iValidationIdForSave = 65;
                ToolbarC.iFormIdToOpenForm = 27;
                ToolbarC.iValidationIdForConfirm = 76;

                PDoc.sFormID = "46";

                if (!IsPostBack)
                {
                    FillCombo();
                    
                    //Session["LeadFleetDtls"] = null;
                    Session["LeadObjective"] = null;
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
                    //ToolbarC.sSetMessage(MANART.WebParts_Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts_Toolbar.enmToolbarType.enmPrint, false);

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                //txtqutdate.Text = Func.Common.sGetCurrentDate(HOBrID, false);
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

                //lblTitle.Text = "M2 Details";
                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
                if (txtUserType.Text == "6")
                {
                    Location.DDLSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
                }

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

            FillCombo();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            ClearDealerHeader();
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
                  
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;
                    ClosureGrid.Enabled = false;
                    string Temp = Session["M2"].ToString();
                    if (bFillDetailsFromGrid(true) == false) return;

                   bFillQuotFromGrid();


                    if (bDetailsRecordExist == false)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Details for Discount');</script>");
                        return;
                    }

                    if (drpStandardDisc.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Standard Discount');</script>");
                        return;
                    }

                        TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[2].FindControl("txtQuotValue");
                        TextBox txtQuotValueDealer = (TextBox)QuotationDtls.Rows[3].FindControl("txtQuotValue");
                        TextBox txtQuotValueMTI = (TextBox)QuotationDtls.Rows[4].FindControl("txtQuotValue");

                       

                            if (Func.Convert.dConvertToDouble(txtQuotValue.Text) != Func.Convert.dConvertToDouble(txtQuotValueDealer.Text) + Func.Convert.dConvertToDouble(txtQuotValueMTI.Text))
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Discount Offered Amount should be Sum of Dealer Share and MTI Share');</script>");
                                return;
                            }

                            if (Func.Convert.dConvertToDouble(txtQuotValue.Text) > 0)
                            {
                                if (txtQuotValueDealer.Text == "0" && txtQuotValueMTI.Text == "0")
                                {
                                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Discount Offered Amount is greater than 0,Dealer Share and MTI Share both can not be 0 ');</script>");
                                    return;
                                }
                            }
                        

                    if (txtM2Date.Text == "" || txtM2Date.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter M2 Date');</script>");
                        return;
                    }

                    if (txtQutNo.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Quotation No');</script>");
                        return;
                    }
                    if (txtqutdate.Text == "" || txtqutdate.Text == "01/01/1900")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Quotation Date');</script>");
                        return;
                    }
                    if (drpCompetitor.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select Competitor');</script>");
                        return;
                    }

                    if (txtCompModel.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Competitor Model');</script>");
                        return;
                    }


                    if (txtCompDiscAmt.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Proposed Discount');</script>");
                        return;
                    }
                    if (drpTCSApp.SelectedValue == "0")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select TCS Applicable');</script>");
                        return;
                    }
               


                    if (Temp == "0" || iID != 0)
                    {
                        Session["M2"] = 1;
                        iID = bHeaderSave("N", "N", "N");
                    }
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {
                       //    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                           Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M2") + "','" + Server.HtmlEncode(txtM2No.Text) + "');</script>");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }

                    }

                    else
                    {
                        if (Temp == "0" && iID == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        }
                        else
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M2") + "','" + Server.HtmlEncode(txtM2No.Text) + "');</script>");
                        }

                    }


                }


                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                 

                    if (drpStandardDisc.SelectedValue == "1")
                    {
                        GenerateLeadNo("D");
                    }
                    ClosureGrid.Enabled = false;
                    if (bFillDetailsFromGrid(true) == false) return;

                    bFillQuotFromGrid();


                        TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[2].FindControl("txtQuotValue");
                        TextBox txtQuotValueDealer = (TextBox)QuotationDtls.Rows[3].FindControl("txtQuotValue");
                        TextBox txtQuotValueMTI = (TextBox)QuotationDtls.Rows[4].FindControl("txtQuotValue");


                        if (Func.Convert.dConvertToDouble(txtQuotValue.Text) > 0)
                        {
                            if (txtQuotValueDealer.Text == "0" && txtQuotValueMTI.Text == "0")
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Discount Offered Amount is greater than 0,Dealer Share and MTI Share both can not be 0 ');</script>");
                                return;
                            }
                        }
                        
                        if (Func.Convert.dConvertToDouble(txtQuotValue.Text) != Func.Convert.dConvertToDouble(txtQuotValueDealer.Text) + Func.Convert.dConvertToDouble(txtQuotValueMTI.Text))
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Discount Offered Amount should be Sum of Dealer Share and MTI Share');</script>");
                            return;
                        }

                    //}

                    iID = bHeaderSave("N", "Y", "N");
                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);



                        if (bSaveDetails("N", "Y", "N") == true)
                        {
                         // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");

                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("M2 ") + "','" + Server.HtmlEncode(txtM2No.Text) + "');</script>");
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


                    //iID = Func.Convert.iConvertToInt(txtID.Text);


                    //iID = bHeaderSave("Y", "N", "N");
                    //PSelectionGrid.Style.Add("display", "");
                    //if (iID > 0)
                    //{
                    //    txtID.Text = Func.Convert.sConvertToString(iID);

                    //    if (bSaveDetails("Y", "N", "N") == true)
                    //    {
                    //        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    //    }
                    //    else
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    //    }

                    //}


                }


                //}
                //else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                //{
                //    if (bSaveRecord(false, false) == false) return;
                //    PSelectionGrid.Style.Add("display", "");
                //}
                FillSelectionGrid();
                GetDataAndDisplay();
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
            if (txtUserType.Text == "6")
            {
                Func.Common.BindDataToCombo(drpArea, clsCommon.ComboQueryType.LeadArea, iDealerId, " and HOBrID=" + iDealerId);
                Func.Common.BindDataToCombo(drpAttendedby, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId);
                Func.Common.BindDataToCombo(drpAlloatedTo, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iDealerId + " and Empl_Type=11");
            }
            else
            {
                Func.Common.BindDataToCombo(drpArea, clsCommon.ComboQueryType.LeadArea, iDealerId, " and HOBrID=" + iHOBrId);
                Func.Common.BindDataToCombo(drpAttendedby, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iHOBrId);
                Func.Common.BindDataToCombo(drpAlloatedTo, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + iHOBrId + " and Empl_Type=11");
            }
            Func.Common.BindDataToCombo(drpInqSource, clsCommon.ComboQueryType.InqSource, 0);
            
            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);
            
            //Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0);

            Func.Common.BindDataToCombo(drpM0PriApp, clsCommon.ComboQueryType.PrimaryApplication, 0);
            iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);

            Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);

            Func.Common.BindDataToCombo(drpPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            Func.Common.BindDataToCombo(drpCompetitor, clsCommon.ComboQueryType.Competitor, 0);
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

            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {

                TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[0].FindControl("txtQuotValue");
                txtQuotValue.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                txtQuotValue.Enabled = false;
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

                TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[0].FindControl("txtQuotValue");
                txtQuotValue.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                txtQuotValue.Enabled = false;
            }

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

       

        protected void drpM0PriApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);
            Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);

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


            if (Type == "D")
            {
                txtAppNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "D"));
            }
            else
            {

                txtM2No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "T"));

                if (Func.Convert.iConvertToInt(drpPOType.SelectedValue) == 1)
                {
                    txtQutNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "Q"));
                }
               
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
                if (Type == "Q")
                {
                    sDocName = "Q";
                }
                else if (Type == "T")
                {
                    sDocName = "M2";
                }
                else if (Type == "D")
                {
                    sDocName = "D";
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

                ds = GetM2(iID, "New", iDealerId, iHOBrId, iM0ID);

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


                ds = GetM2(iID, "Max", iDealerId, iHOBrId, iID);
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
                //txtqutdate.Text = Func.Common.sGetCurrentDate(HOBrID, false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //public DataSet GetM1(int POId, string POType, int DealerID, int HOBrID, int iM0ID) //change for REMAN PO
        //{
        //    // 'Replace objDB to objDB by Shyamal on 26032012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds;
        //        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM1", POId, POType, DealerID, HOBrID, iM0ID);
        //        return ds;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}



        public DataSet GetM2 (int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM2", POId, POType, DealerID, HOBrID, iM1ID);
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
                SearchGrid.sGridPanelTitle = " M2 (Quoation Submitted) List";
                SearchGrid.AddToSearchCombo("M2 No");
                SearchGrid.AddToSearchCombo("M2 Date");
                SearchGrid.AddToSearchCombo("Name");
                SearchGrid.AddToSearchCombo("M2 Status");
               
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

                SearchGrid.sSqlFor = "M2Details";
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
                int iM1ID = Func.Convert.iConvertToInt(txtM1ID.Text);
                //iProformaID = 1;
                if (iID != 0)
                {
                    Session["M2"] = "1";
                    ds = GetM2(iID, "All", iDealerId, iHOBrId, iM1ID);
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
                    ds = GetM2(iID, "Max", iDealerId, iHOBrId, iM1ID);
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


                    if (txtUserType.Text == "6")
                    {
                        FillCombo();
                    }
                    
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
                    txtAddress1.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Add1"]);
                    txtAddress2.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Add2"]);
                    txtCity.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["city"]);
                    txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["pin"]);
                    FillStateCountry();
                    drpState.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["state_id"]);
                    FillRegion();
                    drpRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Region_ID"]);
                    Filldistrict();
                    drpDistrict.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["District_ID"]);
                    txtCountry.Text = "India";
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["mobile"]);

                    txtEmail.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["E_mail"]);
                    txtM0.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Tr_Num"]);
                    txtM0Date.Text = Func.Convert.tConvertToDate(ds.Tables[4].Rows[0]["M0_Date"], false);

                    //drpVisitObj.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["Obj_Id"]);

                    drpM0PriApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["M0_PriApp"]);

                    iM0PriAppID = Func.Convert.iConvertToInt(drpM0PriApp.SelectedValue);
                    Func.Common.BindDataToCombo(drpM0SecApp, clsCommon.ComboQueryType.SecondaryApplication, 0, "And pri_app_ID=" + iM0PriAppID);


                    drpM0SecApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["M0_SecApp"]);

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
                    txtBodyBuilder.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["BodyBuilder"]);
                    //txtNextDate.Text = Func.Convert.tConvertToDate(ds.Tables[4].Rows[0]["Next_date"], false);

                    //M1

                    //Header
                    txtM1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtLeadNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_inq_no"]);
                    txtEnquiryNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EnquiryNo"]);

                    txtDocDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Lead_Date"], false);
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

                    txtRFPID.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["RFPID"]);
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_gp"]);
                    drpModelGroup.SelectedValue = "1";
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["mod_Cat_ID"]);

                    FillModel();

                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                   

                     //0 as  Competitor,'' as Comp_Model,'' as Comp_DiscAmt,
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["ID"]);
                    txtM2No.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["M2_No"]);
                    txtM2Date.Text = Func.Convert.tConvertToDate(ds.Tables[2].Rows[0]["M2_Date"], false);
                    txtQutNo.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Qut_No"]);
                    txtqutdate.Text = Func.Convert.tConvertToDate(ds.Tables[2].Rows[0]["Qut_Date"], false);
                    drpCompetitor.SelectedValue = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Competitor"]);
                    txtCompModel.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Comp_Model"]);
                    txtCompDiscAmt.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Comp_DiscAmt"]);
                    txtLikelydate.Text = Func.Convert.tConvertToDate(ds.Tables[2].Rows[0]["Likely_BuyDateM2"], false);
                    txtDelWeeks.Text = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["DelWeeks"]);
                    string TCSApp = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["TCS_App"]);
                    if (TCSApp == "Y")
                    {
                        drpTCSApp.SelectedValue = "1";
                    }
                    else if (TCSApp == "N")
                    {
                        drpTCSApp.SelectedValue = "2";
                    }
                    else if (TCSApp == "")
                    {
                        drpTCSApp.SelectedValue = "0";
                    }

                    string StandardDisc = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Is_StandardDisc"]);
                    if (StandardDisc == "Y")
                    {
                        drpStandardDisc.SelectedValue = "1";
                    }
                    else if (StandardDisc == "N")
                    {
                        drpStandardDisc.SelectedValue = "2";
                    }
                    else if (StandardDisc == "")
                    {
                        drpStandardDisc.SelectedValue = "0";
                    }

                    if (txtRFPID.Text != "0")
                    {
                        drpStandardDisc.SelectedValue = "2";
                        drpStandardDisc.Enabled = false;
                    }

                    if (drpStandardDisc.SelectedValue == "1")
                    {
                        TextBox txtQuotValueMTIShare = (TextBox)QuotationDtls.Rows[4].FindControl("txtQuotValue");
                        txtQuotValueMTIShare.Text = "Standard Discount";
                        txtQuotValueMTIShare.Enabled = false;
                    }



                    

                }


                else
                {
                    ClearDealerHeader();
                }

              

                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Is_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["Is_Cancel"]);
                hdnLost.Value = Func.Convert.sConvertToString(ds.Tables[2].Rows[0]["M2Lost"]);

                //hdnHold.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lead_Hold"]);

              
                Session["LeadObjective"] = null;
                dtDetails = ds.Tables[5];
                Session["LeadObjective"] = dtDetails;
                BindDataToGrid(bRecordIsOpen, 0);


                Session["LeadClosure"] = null;
                dtClosureDetails = ds.Tables[1];
                Session["LeadClosure"] = dtClosureDetails;
                BindDataToGridClosure(bRecordIsOpen, 0);

                //Session["LeadFleetDtls"] = null;
                //dtFleetDetails = ds.Tables[3];
                //Session["LeadFleetDtls"] = dtFleetDetails;
                //BindDataToGridFleet();


                Session["LeadQuotDtls"] = null;
                dtQuotDetails = ds.Tables[3];
                Session["LeadQuotDtls"] = dtQuotDetails;
                BindDataToGridQuot();

                TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[0].FindControl("txtQuotValue");
                txtQuotValue.Enabled = false;




                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint);
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
            
            bDetailsRecordExist = false;

            dtQuotDetails = (DataTable)Session["LeadQuotDtls"];

            int iCntForDelete = 0;
            int iCntForSelect = 0;

            double dQty = 0;
             double dQtymrp = 0;
             double dQtydisc = 0;
             double dQtycust = 0;

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

                if (iRowCnt==0)
                {
                    dQtymrp=dQty;
                }
                if (iRowCnt==2)
                {
                    dQtydisc = dQty;

                }

                dQtycust = dQtymrp - dQtydisc;

                dtQuotDetails.Rows[iDtSelPartRow]["Value"] = dQty;
                
                dtQuotDetails.Rows[1]["Value"] = dQtycust;

                if (iRowCnt == 0)
                {
                    if (Func.Convert.dConvertToDouble((QuotationDtls.Rows[0].FindControl("txtQuotValue") as TextBox).Text) == 0)
                    {
                        iCntForSelect = iCntForSelect + 1;
                    }
                }






            }


            if (iCntForDelete != iCntForSelect)
            {
                bDetailsRecordExist = false;
                return bDetailsRecordExist;
            }
            else
            {

                bDetailsRecordExist = true;
                return true;
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


            if (iID == 0)
            {

                GenerateLeadNo("");

                txtM2No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "T"));

                if (Func.Convert.iConvertToInt(drpPOType.SelectedValue) == 1)
                {
                    txtQutNo.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "Q"));
                }
               
            }

           
           


            HdrID = objLead.bSaveM2Header(iID, iDealerId, HOBrID, txtQutNo.Text, txtqutdate.Text, txtM2No.Text, txtM2Date.Text, 
                Func.Convert.iConvertToInt(drpCompetitor.SelectedValue),txtCompModel.Text,Func.Convert.dConvertToDouble(txtCompDiscAmt.Text),
                txtAppNo.Text, "", "N", Cancel, Confirm, OrderStatus, Func.Convert.iConvertToInt(txtM1ID.Text),Func.Convert.iConvertToInt(drpPOType.SelectedValue)
                , txtLikelydate.Text, iMenuId, "", Func.Convert.iConvertToInt(drpModelCat.SelectedValue), Func.Convert.iConvertToInt(drpModelCode.SelectedValue)
                , Func.Convert.iConvertToInt(txtQty.Text), Func.Convert.iConvertToInt(txtDelWeeks.Text), drpTCSApp.SelectedItem.Text, drpStandardDisc.SelectedItem.Text
                                );



            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();


            if (objLead.bSaveM2Objectives(objDB, iDealerID, iHOBrId, dtDetails, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }

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
            //drpVisitObj.Enabled = false;
            drpM0PriApp.Enabled = false;
            drpM0SecApp.Enabled = false;
            drpIsMTICust.Enabled = false;
            //txtNextDate.Enabled = false;
            txtBodyBuilder.Enabled = false;
            drpM0Financier.Enabled = false;
            txtDelWeeks.Enabled = bEnable;

            //Enable header Controls of Form
            txtDocDate.Enabled = false;
            txtLeadNo.Enabled = false;
            drpInqSource.Enabled = false;
            //drpLeadName.Enabled = bEnable;
            drpArea.Enabled = false;
            drpAttendedby.Enabled = false;
            drpAlloatedTo.Enabled = false;
            drpPOType.Enabled = false;
            txtSourceAdd.Enabled = false;
            txtSourceMob.Enabled = false;
            txtSourceName.Enabled = false;
            txtAMCDet.Enabled = false;
            txtSpWarrDet.Enabled = false;
            txtExWarrDet.Enabled = false;
            drpSpecialPackage.Enabled = false;
            txtOthersDet.Enabled = false;

            drpModel.Enabled = false;
            drpModelCode.Enabled = false;
            drpModelCat.Enabled = false;
           

            drpModelGroup.Enabled = false;
            txtQty.Enabled = false;
            txtM2No.Enabled = false;
            txtM2Date.Enabled = false;
            txtLikelydate.Enabled = bEnable;
            if (Func.Convert.iConvertToInt(drpPOType.SelectedValue) == 1)
            {
                txtQutNo.Enabled = false;
            }
            else
            {
                txtQutNo.Enabled = bEnable;
            }
            txtqutdate.Enabled = bEnable;
            txtCompModel.Enabled = bEnable;
            txtCompDiscAmt.Enabled = bEnable;
            drpCompetitor.Enabled = bEnable;
            drpTCSApp.Enabled = bEnable;
            QuotationDtls.Enabled = bEnable;

            TextBox txtQuotValue = (TextBox)QuotationDtls.Rows[1].FindControl("txtQuotValue");
            txtQuotValue.Enabled = false;

            TextBox txtQuotValueMTIShare = (TextBox)QuotationDtls.Rows[4].FindControl("txtQuotValue");
            if (drpStandardDisc.SelectedValue == "1")
            {
                txtQuotValueMTIShare.Text = "Standard Discount";
                txtQuotValueMTIShare.Enabled = false;
            }
            else
            {

                txtQuotValueMTIShare.Enabled = bEnable;
            }


            if (txtRFPID.Text != "0")
            {
                drpStandardDisc.SelectedValue = "2";
                drpStandardDisc.Enabled = false;
            }
            else
            {
                drpStandardDisc.Enabled = bEnable;
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



        protected void bConvertToM3(object sender, EventArgs e)
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


            //if (txtRFPID.Text != "0")
            //{
            //    Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //     + iDealerId + "and " + "'" + txtDocDate.Text + "'"
            //     + " not between convert(varchar(10),Effective_From,103) and convert(varchar(10),Effective_To,103) )   "
            //     );



            //    Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //      + iDealerId + "and " + "'" + txtDocDate.Text + "'"
            //      + " not between convert(varchar(10),Effective_From,103) and convert(varchar(10),Effective_To,103))    "
            //      );

            //}
            //else
            //{
            //    Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.AllModels, 0, " and Model_cat_ID=" + modelgrp);
            //    Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.AllModelCode, 0, " and Model_cat_ID=" + modelgrp);
            //}

            Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" + modelgrp);



            Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp);

        }


        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {


            txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);

            FillCombo();
            FillSelectionGrid();

            PSelectionGrid.Style.Add("display", "none");
            txtID.Text = "";

            GetDataFromM1();
            Session["M2"] = 0;
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        }


        private void GetDataFromM1()
        {
            try
            {
                //clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                int iM1ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iM1ID != 0)
                {
                    ds = GetM2(iID, "New", iDealerId, iHOBrId, iM1ID);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //txtqutdate.Text = Func.Common.sGetCurrentDate(HOBrID, false);
                    //ObjDealer = null;
                }
                else
                {
                    ClearDealerHeader();
                }
               

                GenerateLeadNo("");
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
                Func.Common.BindDataToCombo(drpVisitObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M2'");



                //Get Date
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate")) as ASP.webparts_currentdate_ascx).Enabled = true;
                //(DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Enabled =true;
                (DetailsGrid.Rows[iRowCnt].FindControl("txtObjDate") as MANART.WebParts.CurrentDate).Text = "";


                //Get discussion
                TextBox txtDiscussion = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDiscussion");

                TextBox txtTimeSpent = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtTimeSpent");

                DropDownList drpNextObj = (DropDownList)DetailsGrid.Rows[iRowCnt].FindControl("drpNextObj");
                Func.Common.BindDataToCombo(drpNextObj, clsCommon.ComboQueryType.LeadObjective, 0, "and stage='M2'");


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

        protected void drpStandardDisc_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox txtQuotValuRate = (TextBox)QuotationDtls.Rows[0].FindControl("txtQuotValue");
            TextBox txtQuotValuCustDisc = (TextBox)QuotationDtls.Rows[1].FindControl("txtQuotValue");
            
            TextBox txtQuotValueDisc = (TextBox)QuotationDtls.Rows[2].FindControl("txtQuotValue");
            TextBox txtQuotValueMTIShare = (TextBox)QuotationDtls.Rows[4].FindControl("txtQuotValue");
            TextBox txtQuotValueDealer = (TextBox)QuotationDtls.Rows[3].FindControl("txtQuotValue");


            txtQuotValuCustDisc.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtQuotValuRate.Text) - Func.Convert.dConvertToDouble(txtQuotValueDisc.Text));

            if (drpStandardDisc.SelectedValue == "1")
            {
                txtQuotValueMTIShare.Text = "Standard Discount";
                txtQuotValueMTIShare.Enabled = false;
                txtQuotValueDealer.Text = txtQuotValueDisc.Text;
            }
            else
            {
                txtQuotValueMTIShare.Text = "0";
                txtQuotValueMTIShare.Enabled = true;
            }
        }


    }
}