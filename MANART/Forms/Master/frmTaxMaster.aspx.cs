using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using AjaxControlToolkit;
using System.Drawing;

namespace MANART.Forms.Master
{
    public partial class frmTaxMaster : System.Web.UI.Page
    {
        int iUserId = 0;
        private int iTaxID = 0;
        string Active = "Y";
        string tax_tag = "";
        string Service_tag = "";
        private int EGpdealerID = 0;
        int iHOBr_id = 0;
        int iMenuId = 0;
        private int iDealerID = 0;
        private int StateId = 0;
        string ISGST = "";
        string ISGST_dis = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                clsTax ObjTax = new clsTax();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                ISGST = Func.Convert.sConvertToString(Session["ISGST"]);

                StateId = Location.iStateId;
                if (ISGST == "Y" && StateId == 0)
                {
                    StateId = 999;
                }
                else
                {
                    StateId = Location.iStateId;
                }
                DataSet ds = new DataSet();
                // ds = ObjTax.GetMaxTax(iHOBr_id, iDealerID);
                if (iMenuId == 700)
                {
                    ds = ObjTax.GetMaxTax(iHOBr_id, StateId,"S");
                    ToolbarC.Visible = true; 
                }
                else
                {
                    ds = ObjTax.GetMaxTax(iHOBr_id, iDealerID,"D");
                    ToolbarC.Visible = false; 
                }

                if (ds.Tables[0].Rows.Count > 0)
                {


                    iTaxID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iTaxID == 0)
                        iTaxID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                }
                if (iTaxID != 0)
                {
                    GetDataAndDisplay();

                }


                if (!IsPostBack)
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    txtID.Text = Func.Convert.sConvertToString(iTaxID);
                    FillCombo();
                    
                }
               
               
                FillSelectionGrid();


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                iTaxID = Func.Convert.iConvertToInt(SearchGrid.iID);
                txtID.Text = Func.Convert.sConvertToString(iTaxID);
                GetDataAndDisplay();
            }

        }
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.AddToSearchCombo("Tax Description");
                //SearchGrid.sModelPart = "";
                //SearchGrid.iDealerID = iDealerID;
                //SearchGrid.iBrHODealerID = iHOBr_id;
                //SearchGrid.sSqlFor = "TaxMaster";
                //SearchGrid.sGridPanelTitle = "Tax Master List";
                if (iMenuId == 700)
                {
                    if (ISGST == "Y" && Location.iStateId==0)
                    {
                        StateId = 999;
                    }
                    SearchGrid.AddToSearchCombo("Tax Description");
                    SearchGrid.sModelPart = "S";
                    SearchGrid.iDealerID = StateId;
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sSqlFor = "TaxMaster";
                    SearchGrid.sGridPanelTitle = "Tax Master List";
                }
                else
                {
                    SearchGrid.AddToSearchCombo("Tax Description");
                    SearchGrid.sModelPart = "D";
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sSqlFor = "TaxMaster";
                    SearchGrid.sGridPanelTitle = "Tax Master List";
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            try
            {

                if (ISGST_dis == "Y")
                {
                    Func.Common.BindDataToCombo(drpTaxType, clsCommon.ComboQueryType.MANTaxType_GST, 0);
                    Func.Common.BindDataToCombo(drpTaxCategory, clsCommon.ComboQueryType.MANTaxcategory_GST, 0);
                    Func.Common.BindDataToCombo(drpTaxApplicable, clsCommon.ComboQueryType.MANTaxApplicable_GST, 0);

                    if (iMenuId == 700)
                    {
                        StateId = 999;
                        Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1_GST, StateId, "AND (Active='Y' or Active='N')");
                        Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2_GST, StateId, "AND (Active='Y' or Active='N')");

                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1_GST_Dealer, iDealerID, " AND IS_GST='" + ISGST_dis + "' AND (Active='Y' or Active='N')");
                        Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2_GST_Dealer, iDealerID, " AND IS_GST='" + ISGST_dis + "' AND (Active='Y' or Active='N')");

                    }
                    Func.Common.BindDataToCombo(drpServiceTax, clsCommon.ComboQueryType.MANServiceType_GST, 0);
                }
               else
                {
                    Func.Common.BindDataToCombo(drpTaxType, clsCommon.ComboQueryType.MANTaxType, 0);
                    Func.Common.BindDataToCombo(drpTaxCategory, clsCommon.ComboQueryType.MANTaxcategory, 0);
                    Func.Common.BindDataToCombo(drpTaxApplicable, clsCommon.ComboQueryType.MANTaxApplicable, 0);
                    if (iMenuId == 700)
                    {
                        Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1, StateId, "AND (Active='Y' or Active='N')");
                        Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2, StateId, "AND (Active='Y' or Active='N')");
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1_GST_Dealer, iDealerID, " AND IS_GST='" + ISGST_dis + "' AND (Active='Y' or Active='N')");
                        Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2_GST_Dealer, iDealerID, " AND IS_GST='" + ISGST_dis + "' AND (Active='Y' or Active='N')");

                    }
                }
                


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo_New()
        {
            try
            {

                if (ISGST == "Y")
                {

                    Func.Common.BindDataToCombo(drpTaxType, clsCommon.ComboQueryType.MANTaxType_GST, 0);
                    Func.Common.BindDataToCombo(drpTaxCategory, clsCommon.ComboQueryType.MANTaxcategory_GST, 0);
                    Func.Common.BindDataToCombo(drpTaxApplicable, clsCommon.ComboQueryType.MANTaxApplicable_GST, 0);
                    Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1_GST, StateId, "AND (Active='Y' or Active='N')");
                    Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2_GST, StateId, "AND (Active='Y' or Active='N')");
                    Func.Common.BindDataToCombo(drpServiceTax, clsCommon.ComboQueryType.MANServiceType_GST, 0);
                }
                else
                {

                    Func.Common.BindDataToCombo(drpTaxType, clsCommon.ComboQueryType.MANTaxType, 0);
                    Func.Common.BindDataToCombo(drpTaxCategory, clsCommon.ComboQueryType.MANTaxcategory, 0);
                    Func.Common.BindDataToCombo(drpTaxApplicable, clsCommon.ComboQueryType.MANTaxApplicable, 0);
                    Func.Common.BindDataToCombo(drpAdd1, clsCommon.ComboQueryType.MANADDSalesTax1, StateId, "AND (Active='Y' or Active='N')");
                    Func.Common.BindDataToCombo(drpAdd2, clsCommon.ComboQueryType.MANADDSalesTax2, StateId, "AND (Active='Y' or Active='N')");
                    Func.Common.BindDataToCombo(drpServiceTax, clsCommon.ComboQueryType.MANServiceType, 0);


                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            StateId = Location.iStateId;
            FillSelectionGrid();
            if (!IsPostBack)
            {
                //SearchGrid.bIsCollapsable = false;
                if (iMenuId == 700)
                {
                    Location.bHideDealerDetails = true;
                }
                else
                {
                    Location.bHideDealerDetails = false;
                }
              
                
            }
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iTaxID = Func.Convert.iConvertToInt(txtID.Text);
            ViewState["iID"] = iTaxID;
            GetDataAndDisplay();


        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (Convert.ToInt32(url1[1]) == 306)
                //{

                //    StateId = Location.iStateId;
                //}
                //else if (Convert.ToInt32(url1[1]) == 496)
                //{

                    StateId = Location.iStateId;
                //}
                //else if (Convert.ToInt32(url1[1]) == 518)
                //{
                   // StateId = Location.iCountryId;
                //}
                FillDetails();
                FillSelectionGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillDetails()
        {
            StateId = Location.iStateId;
            clsTax ObjTax = new clsTax();
            DataSet ds = new DataSet();
            // ds = ObjTax.GetMaxTax(iHOBr_id, iDealerID);
            if (iMenuId == 700)
            {
                ds = ObjTax.GetMaxTax(iHOBr_id, StateId, "S");
            }

            else
            {
                ds = ObjTax.GetMaxTax(iHOBr_id, iDealerID, "D");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {


               // iTaxID = Func.Convert.iConvertToInt(txtID.Text);
              //  if (iTaxID == 0)
                    iTaxID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
            }
            if (iTaxID != 0)
            {
                GetDataAndDisplay();

            }



        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            try
            {
                StateId = Location.iStateId;
                FillDetails();
                FillSelectionGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void GetDataAndDisplay()
        {



            try
            {

                clsTax ObjTax = new clsTax();
                DataSet ds = new DataSet();
                if (iTaxID != 0)
                {
                    // ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetEGPTaxMaster", iTaxID);
                    if (iMenuId == 700)
                    {
                        ds = ObjTax.GetTax(iTaxID, "S");
                    }
                    else
                    {
                        ds = ObjTax.GetTax(iTaxID, "D");
                    }

                    DisplayData(ds);
                    ObjTax = null;
                }
                else
                {
                    ds = null;
                    ObjTax = null;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {

            }

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

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ISGST_dis = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_GST"]);





                    FillCombo();
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtTaxDesc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Description"]);
                    txtTaxPercentage.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Percentage"]);
                    //drpTaxType.SelectedItem.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Type"]);
                    //drpTaxApplicable.SelectedItem.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Applicable on"]);   
                    Service_tag = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Service_Tax"]);

                    if (Service_tag == "Y")
                    {
                        drpServiceTax.SelectedValue = "1";
                    }
                    else if (Service_tag == "T")
                    {
                        drpServiceTax.SelectedValue = "3";
                    }
                    else
                    {
                        drpServiceTax.SelectedValue = "2";
                    }







                    drpTaxCategory.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Type ID"]);
                    tax_tag = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax_tag"]);

                    if (tax_tag == "I")
                    {
                        drpTaxType.SelectedValue = "1";
                    }
                    else if (tax_tag == "O")
                    {
                        drpTaxType.SelectedValue = "2";
                    }
                    else
                    {
                        drpTaxType.SelectedValue = "0";
                    }

                    drpTaxApplicable.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax Applicable on ID"]);

                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else if (Active == "N")
                    {
                        drpActive.SelectedValue = "2";
                    }
                    else
                    {
                        drpActive.SelectedValue = "0";
                    }
                    if (drpTaxCategory.SelectedValue == "1")
                    {
                        trAddTaxId.Visible = true;
                        drpAdd1.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1"]);
                        drpAdd2.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2"]);


                    }
                    else
                    {
                        trAddTaxId.Visible = false;
                    }

                    if (ISGST_dis == "Y")
                    {
                        if (drpTaxType.SelectedValue == "1" && drpTaxCategory.SelectedValue=="1")
                        {
                            // drpTaxCategory.SelectedValue = "1";
                            // drpTaxCategory.SelectedItem.Text = "SGST";

                            drpTaxCategory.Items[1].Text = "SGST";
                            drpTaxCategory.Enabled = true;
                            drpTaxApplicable.SelectedValue = "1";
                            drpTaxApplicable.Enabled = false;

                            trAddTaxId.Visible = true;
                            lblAdd2.Visible = false;
                            drpAdd2.Visible = false;
                        }

                        else if (drpTaxType.SelectedValue == "2" && drpTaxCategory.SelectedValue == "1")
                        {
                            drpTaxCategory.SelectedValue = "1";
                            //drpTaxCategory.SelectedItem.Text = "IGST";
                            drpTaxCategory.Items[1].Text = "IGST";
                            drpTaxCategory.Enabled = false;
                            drpTaxApplicable.SelectedValue = "1";
                            drpTaxApplicable.Enabled = false;
                            trAddTaxId.Visible = false;


                        }
                    }


                }
                else
                {
                    ClearControlState();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void drpTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // To Clear Control
        private void ClearControlState()
        {
            FillCombo_New();
            txtTaxDesc.Text = "";
            txtTaxPercentage.Text = "";

            drpTaxType.SelectedValue = "0";
            drpTaxCategory.SelectedValue = "0";
            drpTaxApplicable.SelectedValue = "0";
            drpAdd1.SelectedValue = "0";
            drpAdd2.SelectedValue = "0";
            drpActive.SelectedValue = "1";
            if (drpTaxCategory.SelectedValue == "1")
            {
                trAddTaxId.Visible = true;
            }
            else
            {
                trAddTaxId.Visible = false;
            }


        }
        private void ReadonlyControl()
        {

            txtTaxDesc.ReadOnly = false;
            txtTaxPercentage.ReadOnly = false;

            drpTaxType.Enabled = true;
            drpTaxApplicable.Enabled = true;



        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    txtID.Text = "";
                    ClearControlState();
                    ReadonlyControl();
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord() == false) return;
                    if (bSaveRecord() == false) return;
                }

                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (txtID.Text == "") return;
                    //CancelRecord();
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8);</script>");
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //ToValidate Record
        private bool bValidateRecord()
        {
            string sMessage = "";
            bool bValidateRecord = true;
            DataSet dsvalidate = new DataSet();
            clsDB objDB = new clsDB();
            int IsExist;
            iTaxID = Func.Convert.iConvertToInt(txtID.Text);
            if (drpTaxType.SelectedValue == "1")
            {
                tax_tag = "I";
            }
            else if (drpTaxType.SelectedValue == "2")
            {
                tax_tag = "O";
            }

            if (ISGST == "Y" && Location.iStateId == 0)
            {
                StateId = 999;
            }
            IsExist = objDB.ExecuteStoredProcedure("SP_GetTaxMaster_Active", txtTaxPercentage.Text, tax_tag, drpTaxCategory.SelectedItem.Value, drpTaxApplicable.SelectedValue, drpAdd1.SelectedValue, drpAdd2.SelectedValue, drpActive.SelectedItem.Text, StateId, iHOBr_id);
            if (IsExist > 0 && iTaxID == 0)
            {
                sMessage = sMessage + "\\n Tax Already Exist.";
                bValidateRecord = false;
            }

            if (txtTaxDesc.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Tax Description.";
                bValidateRecord = false;
            }
            if (txtTaxPercentage.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Tax Percentage.";
                bValidateRecord = false;
            }
            if (drpTaxType.SelectedItem.Value == "0")
            {
                sMessage = sMessage + "\\n Please Select Tax Type.";
                bValidateRecord = false;
            }
            if (drpTaxCategory.SelectedItem.Value == "0")
            {
                sMessage = sMessage + "\\n Please Select Tax Category.";
                bValidateRecord = false;
            }

            if (drpTaxApplicable.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please Select Tax Applicable on.";
                bValidateRecord = false;
            }
            if (drpActive.SelectedItem.Value == "0")
            {
                sMessage = sMessage + "\\n Please Select Active.";
                bValidateRecord = false;
            }


            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                return bValidateRecord;
            }

            return bValidateRecord;
        }

        private bool bSaveRecord()
        {

            clsTax ObjTax = new clsTax();

            bool bSave = false;

            iTaxID = Func.Convert.iConvertToInt(txtID.Text);

            if (drpTaxType.SelectedValue == "1")
            {
                tax_tag = "I";
            }
            else if (drpTaxType.SelectedValue == "2")
            {
                tax_tag = "O";
            }



            //  Active = (string)ViewState["Active"]; 
            //  dtDetails = (DataTable)Session["ModelRateDetails"];
            //objDB.BeginTranasaction();
            //objDB.ExecuteStoredProcedure("SP_EGPTaxMaster_Save", iTaxID, txtTaxDesc.Text, txtTaxPercentage.Text, drpTaxCategory.SelectedItem.Value, tax_tag, drpTaxApplicable.SelectedValue, drpAdd1.SelectedValue, drpAdd2.SelectedValue, drpActive.SelectedItem.Text, EGpdealerID);
            //objDB.CommitTransaction();
            //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
            //return true;
            StateId = Location.iStateId;
            if (ISGST=="Y" && Location.iStateId==0)
            {
                StateId = 999;
                bSave = ObjTax.bSaveTaxMaster(iTaxID, txtTaxDesc.Text.Trim(), Convert.ToDouble(txtTaxPercentage.Text.Trim()),
              Convert.ToInt32(drpTaxCategory.SelectedValue), tax_tag.Trim(),
              Convert.ToInt32(drpTaxApplicable.SelectedValue), Convert.ToInt32(drpAdd1.SelectedValue),
              Convert.ToInt32(drpAdd2.SelectedValue), drpActive.SelectedItem.Text, StateId, Func.Convert.sConvertToString(drpServiceTax.SelectedItem.Text), ISGST);
            }
            else
            {
                ISGST = "N";
                if (drpServiceTax.SelectedValue == "1")
                {
                    Service_tag = "Y";
                }
                else if (drpServiceTax.SelectedValue == "3")
                {
                    Service_tag = "T";
                }
                else
                {
                    Service_tag = "N";
                }

                bSave = ObjTax.bSaveTaxMaster(iTaxID, txtTaxDesc.Text.Trim(), Convert.ToDouble(txtTaxPercentage.Text.Trim()),
              Convert.ToInt32(drpTaxCategory.SelectedValue), tax_tag.Trim(),
              Convert.ToInt32(drpTaxApplicable.SelectedValue), Convert.ToInt32(drpAdd1.SelectedValue),
              Convert.ToInt32(drpAdd2.SelectedValue), drpActive.SelectedItem.Text, StateId, Service_tag, ISGST);
            }
            //bSave = ObjTax.bSaveTaxMaster(iTaxID, txtTaxDesc.Text.Trim(),Convert.ToDouble(txtTaxPercentage.Text.Trim()),
            //    Convert.ToInt32(drpTaxCategory.SelectedValue), tax_tag.Trim(),
            //    Convert.ToInt32(drpTaxApplicable.SelectedValue), Convert.ToInt32(drpAdd1.SelectedValue),
            //    Convert.ToInt32(drpAdd2.SelectedValue), drpActive.SelectedItem.Text, iDealerID, iHOBr_id);
           



            //return bSave;

            if (bSave == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                return false;
            }
            ObjTax = null;
            return true;



        }


        protected void drpTaxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpTaxCategory.SelectedValue == "1")
            {
                trAddTaxId.Visible = true;
            }
            else
            {
                trAddTaxId.Visible = false;
            }
        }

        protected void drpTaxType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ISGST == "Y")
            {
                if (drpTaxType.SelectedValue == "1")
                {
                    // drpTaxCategory.SelectedValue = "1";
                    // drpTaxCategory.SelectedItem.Text = "SGST";

                    drpTaxCategory.Items[1].Text = "SGST";
                    drpTaxCategory.Enabled = true;
                    drpTaxApplicable.SelectedValue = "1";
                    drpTaxApplicable.Enabled = false;
                }

                else if (drpTaxType.SelectedValue == "2")
                {
                    drpTaxCategory.SelectedValue = "1";
                    //drpTaxCategory.SelectedItem.Text = "IGST";
                    drpTaxCategory.Items[1].Text = "IGST";
                    drpTaxCategory.Enabled = false;
                    drpTaxApplicable.SelectedValue = "1";
                    drpTaxApplicable.Enabled = false;
                }
            }
        }
    }
}