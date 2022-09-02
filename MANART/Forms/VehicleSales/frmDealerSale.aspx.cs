﻿using System;
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
    public partial class frmDealerSale : System.Web.UI.Page
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
        string sAppNew;
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
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);

                }

                //ToolbarC.iValidationIdForSave = 65;
                ToolbarC.iFormIdToOpenForm = 202;
               

                //PDoc.sFormID = "54";


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
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);


                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
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
                //PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
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
                    PSelectionGrid.Style.Add("display", "");

                    DisplayPreviousRecord();

                   
                       //GetM8(iID, "New", iDealerId, iHOBrId, iM0ID);

                        //DisplayData(ds);

                


                    GenerateLeadNo("DS");
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    //  ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);


                    //return;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    bDetailsRecordExist = false;

                 


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
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                        //if (bSaveDetails("N", "Y", "N") == true)
                        //{
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed');</script>");
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
                //PDoc.BindDataToGrid();


            }

            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            if (txtUserType.Text == "6")
            {
                Func.Common.BindDataToCombo(drpCustName, clsCommon.ComboQueryType.DealerTypeCust, iDealerId, " and DealerHOBrID=" + iDealerId);
            }
            else
            {
                Func.Common.BindDataToCombo(drpCustName, clsCommon.ComboQueryType.DealerTypeCust, iDealerId, " and DealerHOBrID=" + iHOBrId);
            }
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);

            drpModelGroup.SelectedValue = "1";
        }
       


        protected void drpModelGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillModel();

        }
        private void FillTaxDet()
        {
            try
            {
                clsDB objDB = new clsDB();

                DataSet dsCustType = new DataSet();

                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxes", iDealerId, Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));


                if (dsCustType != null)
                {
                    lblTax1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2Desc"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Per"]);
                    txtTax2Per.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2Per"]);
                    if (txtTaxType.Text == "C")
                    {
                        lblCST.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedItem);
                        txtCSTPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                        txtCSTID.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedValue);
                        txtVATID.Text = "0";
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        lblVat.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedItem);
                        txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                        txtCSTID.Text = "0";
                        txtVATID.Text = Func.Convert.sConvertToString(drpTaxPer.SelectedValue);
                    }
                 
                    else if(hidegst.Value=="N")
                    {
                        lblCST.Text = "CST";
                        lblVat.Text = "VAT";
                        txtCSTID.Text = "0";
                        txtVATID.Text = "0";
                        txtVatPer.Text = "0";
                        txtCSTPer.Text = "0";
                    }
                    else if (hidegst.Value == "Y")
                    {
                        lblCST.Text = "IGST";
                        lblVat.Text = "SGST";
                        txtCSTID.Text = "0";
                        txtVATID.Text = "0";
                        txtVatPer.Text = "0";
                        txtCSTPer.Text = "0";
                    }
                    txtTax1ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1ID"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2ID"]);
                    txtTaxApp.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp"]);
                    txtTaxApp1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp1"]);
                    txtTaxApp2.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp2"]);
                }
                else
                {
                    if (hidegst.Value == "N")
                    {
                        lblCST.Text = "CST";
                        lblVat.Text = "VAT";
                        lblTax1.Text = "Tax1";
                        lblTax2.Text = "Tax2";
                    }
                    else if (hidegst.Value == "Y")
                    {
                        lblCST.Text = "IGST";
                        lblVat.Text = "SGST";
                        lblTax1.Text = "CGST";

                    }
                }

                 
                if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
                {
                    DataSet dsTaxCal = new DataSet();
                    dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_TaxCalculationM8", Func.Convert.iConvertToInt(drpTaxPer.SelectedValue)
                    , Func.Convert.iConvertToInt(txtTax1ID.Text), Func.Convert.iConvertToInt(txtTax2ID.Text), Func.Convert.iConvertToInt(txtTaxApp.Text)
                    , Func.Convert.iConvertToInt(txtTaxApp1.Text), Func.Convert.iConvertToInt(txtTaxApp2.Text), Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text),
                    Func.Convert.dConvertToDouble(txtDisc.Text)

                    );

                    if (dsTaxCal != null)
                    {
                        if (txtTaxType.Text == "C")
                        {
                            txtCSTAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                        }
                        else if (txtTaxType.Text == "V")
                        {
                            txtVatAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                        }
                        else
                        {
                            txtCSTAmt.Text = "0";
                            txtVatAmt.Text = "0";
                        }

                        txttax1Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]), 2).ToString();
                        txttax2Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]), 2).ToString();


                    }

                }
            }
            catch (Exception ex) { }
        }

        private void FillTaxdetials(string Taxtype)
        {
            try
            {
                clsDB objDB = new clsDB();
                if (txtUserType.Text == "6")
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8HSNDetailsFill", Func.Convert.iConvertToInt(Session["DealerID"]), Convert.ToString(hidegst.Value), Taxtype,Convert.ToInt32(drpModelCode.SelectedValue));
                }
                else
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8HSNDetailsFill", Func.Convert.iConvertToInt(Session["iDealerID"]), Convert.ToString(hidegst.Value), Taxtype, Convert.ToInt32(drpModelCode.SelectedValue));
                }
                if (dsState != null)
                {
                    drpTaxPer.DataSource = dsState.Tables[0];
                    drpTaxPer.DataTextField = "Name";
                    drpTaxPer.DataValueField = "ID";
                    drpTaxPer.DataBind();
                    drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dsState.Tables[0].Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void drpTaxPer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillTaxDet();
                txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                  Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                  Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                  Func.Convert.dConvertToDouble(txtTCS.Text)
                  );
            }
            catch (Exception ex) { }
        }

        //protected void drpCSTPer_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    drpCSTPer.Style.Add("display", "none");

        //    txtCSTPer.Style.Add("display", "");

        //    txtCSTPer.Text = Func.Convert.sConvertToString(drpCSTPer.SelectedIndex);

        //    lblCST.Text = Func.Convert.sConvertToString(drpCSTPer.SelectedItem);

        //    clsDB objDB = new clsDB();

        //    DataSet dsCustType = new DataSet();
        //    //dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxe1", iDealerId, Func.Convert.iConvertToInt(drpVATPer.SelectedValue));


        //    //if (dsCustType != null)
        //    //{
        //    //    txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
        //    //}


        //}




        protected void drpModelCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                drpModel.SelectedValue = drpModelCode.SelectedValue;
                clsDB objDB = new clsDB();

                DataSet dsCustType = new DataSet();
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    txtModelRate.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["MRP"]);
                    //drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
                }
            }
            catch (Exception ex) { }

        }


        protected void drpModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex) { }

        }
        protected void drpTCSApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) == 1)
                {
                    FillTCSTaxDet();
                }
                else if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) == 2)
                {
                    txtTCSPer.Text = "0";
                    txtTCS.Text = "0";
                    lblTCS.Text = "TCS";
                    txtTCSTaxID.Text = "0";
                }

                txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                    Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                    Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                    Func.Convert.dConvertToDouble(txtTCS.Text)
                    );
            }
            catch (Exception ex) { }
        }
        private void FillTCSTaxDet()
        {
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_TCSTaxCalculationM8", Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text),
                Func.Convert.dConvertToDouble(txtDisc.Text), Func.Convert.dConvertToDouble(txtVatAmt.Text),
                Func.Convert.dConvertToDouble(txtCSTAmt.Text), Func.Convert.dConvertToDouble(txttax1Amt.Text),
                Func.Convert.dConvertToDouble(txttax2Amt.Text), Func.Convert.dConvertToDouble(txtPFCharges.Text),
                Func.Convert.dConvertToDouble(txtOthercharges.Text), iDealerId);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtTCSPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSPer"]);
                txtTCS.Text =Math.Round(Convert.ToDouble (dsCustType.Tables[0].Rows[0]["TCSAmt"]),2).ToString();
                lblTCS.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSDesc"]);
                txtTCSTaxID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSTaxID"]);
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


            if (Type == "DS")
            {

                txtM8No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "DS"));

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
                if (Type == "DS")
                {
                    sDocName = "DS";
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


                ds = GetM8(iID, "New", iDealerId, iHOBrId, 0);

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


                ds = GetM8(iID, "Max", iDealerId, iHOBrId, iID);
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

        public DataSet GetM8(int POId, string POType, int DealerID, int HOBrID, int iM1ID)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerSale", POId, POType, DealerID, HOBrID);
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
                SearchGrid.sGridPanelTitle = "Dealer Sale Invoice List";
                SearchGrid.AddToSearchCombo("Invoice No");
                SearchGrid.AddToSearchCombo("Invoice Date");
                SearchGrid.AddToSearchCombo("Customer Name");
                SearchGrid.AddToSearchCombo("Chassis No");
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

                SearchGrid.sSqlFor = "DealerSaleDet";
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
                    ds = GetM8(iID, "All", iDealerId, iHOBrId, 0);
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
                    ds = GetM8(iID, "Max", iDealerId, iHOBrId, 0);
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

                    if (txtUserType.Text == "6")
                    {
                        FillCombo();
                    }
                    //txtM3ID.Text = "N";
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtM8No.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvNo"]);
                    txtM8Date.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["InvDate"], false);

                    

                    if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Cust_ID"]) != 0)
                    {

                        FillSaveCustomer(Func.Convert.iConvertToInt(txtID.Text));
                    }
                    else
                    {
                        FillCombo();

                        
                        drpCustName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cust_ID"]);
                    }


                    txtSaleDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SaleDealer"]);
                    txtSAlePOID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalePO_Id"]);


                    if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SalePO_Id"]) != 0)
                    {

                        FillPOLIst(Func.Convert.iConvertToInt(txtID.Text));
                    }
                    else
                    {
                        //FillCombo();

                        drpPOList.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpPOList.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalePO_Id"]);
                    }



                    if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_Id"])!= 0)
                    {

                        FillSaveChassis(Func.Convert.iConvertToInt(txtID.Text));
                    }
                    else
                    {

                        drpChassisNo.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpChassisNo.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_Id"]);
                    }

                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngineNo"]);

                    txtStockID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Stock_Id"]);
                    

                    txtTaxType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Taxtype"]);
                   

                   
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                    drpModelGroup.SelectedValue = "1";
                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                   drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                   if (txtTaxType.Text == "C")
                   {
                       FillTaxdetials(txtTaxType.Text);
                       // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                       drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                       txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CST"]);
                       txtVATID.Text = "0";
                       txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTApp"]);
                   }
                   else if (txtTaxType.Text == "V")
                   {
                       FillTaxdetials(txtTaxType.Text);
                       // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                       drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                       txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VAT"]);
                       txtCSTID.Text = "0";
                       txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATApp"]);
                   }
                   else
                   {
                       drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                       drpTaxPer.SelectedValue = "0";
                       txtVATID.Text = "0";
                       txtCSTID.Text = "0";
                       txtTaxApp.Text = "0";
                   }
                                                        
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                    txtTotalAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Total"]);
                    txtTaxTotalAmt.Text =Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["Total"]),2).ToString();
                    txtGrandTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GrandTotal"]);
                    if (ds.Tables[0].Rows[0]["Disc"] != "" || ds.Tables[0].Rows[0]["TCSAmt"] != "")
                    {
                        txtDisc.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["Disc"]), 2).ToString();
                        txtTCS.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["TCSAmt"]), 2).ToString();
                    }
                    else
                    {
                        txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Disc"]);
                        txtTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSAmt"]);
                    }
                    txtPFCharges.Text =Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["PFCharges"]),2).ToString();
                    txtOthercharges.Text = Math.Round(Convert.ToDouble (ds.Tables[0].Rows[0]["Other"]),2).ToString();
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax1"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["tax2"]);
                    txtCSTAmt.Text =  Math.Round(Convert.ToDouble (ds.Tables[0].Rows[0]["CSTAmt"]),2).ToString();
                    txtVatAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATAmt"]);
                    txttax1Amt.Text =Math.Round(Convert.ToDouble (ds.Tables[0].Rows[0]["Tax1Amt"]),2).ToString();
                    txttax2Amt.Text =Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["Tax2Amt"]),2).ToString();
                    drpTCSApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_App"]);
                    txtTCSTaxID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSPer"]);
                 
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCS_Per"]);
                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTDesc"]);
                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TAx1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Desc"]);
                    lblTCS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TCSDesc"]);
                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSTPer"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VATPer"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1Per"]);
                    txtTax2Per.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2Per"]);
                    txtTaxApp1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax1App"]);
                    txtTaxApp2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tax2App"]);
                }
                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Cancel"]);
                hdnLost.Value = "N";
                hidegst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gst"]);
                lbldiscount.Visible = false; tddiscbank.Visible = false; tdtxtdisc.Visible = false;
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gst"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]) != "N")
                {
                    lblCST.Text="IGST";
                    lblVat.Text = "SGST";
                    lblTax1.Text = "CGST";
                    tdamount.Visible = false; tdlbl.Visible = false; tdper.Visible = false; 
                }
                else {  tdamount.Visible = true; tdlbl.Visible = true; tdper.Visible = true; lblTax1.Text = "TAX1"; lblCST.Text = "CST"; lblVat.Text = "VAT"; hidegst.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISGST"]); }

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

                if (hdnConfirm.Value == "Y")
                {

                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }

                if (Func.Convert.iConvertToInt(txtID.Text) != 0 && (bEnableControls == true))
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);

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

            if (iID == 0)
            {
                GenerateLeadNo("DS");
            }

            HdrID = objLead.bSaveDealerSale(iID, iDealerId, HOBrID,
                 txtM8No.Text, txtM8Date.Text,Confirm, Cancel,Func.Convert.iConvertToInt(txtCSTID.Text), Func.Convert.iConvertToInt(txtVATID.Text),
                    Func.Convert.dConvertToDouble(txtPFCharges.Text), Func.Convert.dConvertToDouble(txtOthercharges.Text),
                    Func.Convert.dConvertToDouble(txtTotalAmt.Text), Func.Convert.dConvertToDouble(txtModelRate.Text), Func.Convert.dConvertToDouble(txtGrandTotal.Text),
                    Func.Convert.iConvertToInt(txtTax1ID.Text), Func.Convert.iConvertToInt(txtTax2ID.Text),
                    Func.Convert.dConvertToDouble(txtCSTAmt.Text), Func.Convert.dConvertToDouble(txtVatAmt.Text), Func.Convert.dConvertToDouble(txttax1Amt.Text),
                    Func.Convert.dConvertToDouble(txttax2Amt.Text), Func.Convert.iConvertToInt(txtTCSTaxID.Text), Func.Convert.dConvertToDouble(txtTCS.Text),
                     Func.Convert.dConvertToDouble(txtDisc.Text), Func.Convert.iConvertToInt(txtQty.Text),
                    Func.Convert.iConvertToInt(drpTCSApp.SelectedValue), txtTaxType.Text, Func.Convert.iConvertToInt(txtStockID.Text)
                    , 0, Func.Convert.iConvertToInt(txtSaleDealer.Text), Func.Convert.iConvertToInt(drpModel.SelectedValue)
                    , Func.Convert.iConvertToInt(drpModelGroup.SelectedValue), Func.Convert.iConvertToInt(drpChassisNo.SelectedValue)
                    , Func.Convert.iConvertToInt(drpCustName.SelectedValue), Func.Convert.iConvertToInt(txtSAlePOID.Text), Func.Convert.iConvertToInt(drpModelCat.SelectedValue),hidegst.Value

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


            txtM8No.Enabled = false;
            txtM8Date.Enabled = false;

            //drpM4Financier.Enabled = bEnable;
            //txtLoanAmt.Enabled = bEnable;
            //txtMarginMoney.Enabled = bEnable;
            //txtTenure.Enabled = bEnable;
            //txtInterestRate.Enabled = bEnable;
            //txtModelChange.Enabled = bEnable;
            //txtTrailerChanssis.Enabled = bEnable;
            //txtTrailerAmt.Enabled = bEnable;
            //txtDONo.Enabled = bEnable;
            //txtDODate.Enabled = bEnable;
            //txtDOAmt.Enabled = bEnable;




            //model details
            drpModelGroup.Enabled = false;
            drpModelCat.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;

            drpChassisNo.Enabled = bEnable;
            txtEngineNo.Enabled = false;

            //txtCustName.Enabled = false;
            txtModelRate.Enabled = bEnable;

            txtQty.Enabled = false;


            txtTotalAmt.Enabled = false;
            txtTaxTotalAmt.Enabled = false;
            txtGrandTotal.Enabled = false;



            drpTaxPer.Enabled = bEnable;
            drpPOList.Enabled = bEnable;



            txtPFCharges.Enabled = bEnable;
            txtOthercharges.Enabled = bEnable;

            txtCSTAmt.Enabled = false;
            txtVatAmt.Enabled = false;
            txttax1Amt.Enabled = false;
            txttax2Amt.Enabled = false;


            drpTCSApp.Enabled = bEnable;

            txtTCS.Enabled = false;
            txtTCSPer.Enabled = false;

            lblCST.Enabled = false;
            lblVat.Enabled = false;
            lblTax1.Enabled = false;
            lblTax2.Enabled = false;
            lblTCS.Enabled = false;

            txtCSTPer.Enabled = false;
            txtVatPer.Enabled = false;
            txttax1Per.Enabled = false;
            txtTax2Per.Enabled = false;

            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);



        }

        private void FillEnquiry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquiry", iDealerId, HOBrID);

                //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
                if (dsCustType != null)
                {
                    //drpM7Det.DataSource = dsCustType.Tables[0];
                    //drpM7Det.DataTextField = "Name";
                    //drpM7Det.DataValueField = "ID";
                    //drpM7Det.DataBind();
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

        private void FillSaveChassis(int ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSaveDealerSaleChassis",ID ,iDealerId, HOBrID);

              
                if (dsCustType != null)
                {
                    drpChassisNo.DataSource = dsCustType.Tables[0];
                    drpChassisNo.DataTextField = "Name";
                    drpChassisNo.DataValueField = "ID";
                    drpChassisNo.DataBind();
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

        private void FillSaveCustomer(int ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSaveDealerSaleCustomer", ID, iDealerId, HOBrID);


                if (dsCustType != null)
                {
                    drpCustName.DataSource = dsCustType.Tables[0];
                    drpCustName.DataTextField = "Name";
                    drpCustName.DataValueField = "ID";
                    drpCustName.DataBind();
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

        private void FillPOLIst(int ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSaveDealerSalePO", ID, iDealerId, HOBrID);


                if (dsCustType != null)
                {
                    drpPOList.DataSource = dsCustType.Tables[0];
                    drpPOList.DataTextField = "Name";
                    drpPOList.DataValueField = "ID";
                    drpPOList.DataBind();
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




        //protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        //{


        //    txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
        //    //txtCashLoan.Text = Func.Convert.sConvertToString(PDoc.sDoc);

        //    FillCombo();
        //    FillSelectionGrid();

        //    PSelectionGrid.Style.Add("display", "none");
        //    txtID.Text = "";

        //    GetDataFromAllocation();



        //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
        //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
        //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
        //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
        //    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

        //}
        //private void GetDataFromAllocation()
        //{
        //    try
        //    {

        //        DataSet ds = new DataSet();
        //        int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);

        //        if (iM0ID != 0)
        //        {
        //            ds = GetM8(iID, "New", iDealerId, iHOBrId, iM0ID);

        //            DisplayData(ds);

        //        }



        //        GenerateLeadNo("M8");
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        
        protected void txtDisc_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
            {
                FillTaxDet();
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                {
                    FillTCSTaxDet();
                }
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
            txtGrandTotal.Text = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2).ToString();
        }

        protected void txtModelRate_TextChanged(object sender, EventArgs e)
        {

            txtTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));
            txtTaxTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));

            if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
            {
                FillTaxDet();
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                {
                    FillTCSTaxDet();
                }
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
            txtGrandTotal.Text = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2).ToString();
        }

        protected void txtPFCharges_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
            {
                FillTCSTaxDet();
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
            txtGrandTotal.Text = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2).ToString();
        }

        protected void txtOthercharges_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
            {
                FillTCSTaxDet();
            }
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
              Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
              Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
              Func.Convert.dConvertToDouble(txtTCS.Text)
              );
            txtGrandTotal.Text = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2).ToString();
        }
        
        protected void drpCustName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int custID;
            custID = Func.Convert.iConvertToInt(drpCustName.SelectedValue);

            GetCustData(custID);

        }

        private void GetCustData(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                DataSet dsPO = new DataSet();
                if (ID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCustState", ID, iDealerId, HOBrID);
                    
                    

                    if (ds !=null)
                    {


                        txtTaxType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TaxType"]);
                        txtSaleDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SaleDealer"]);

                        if (txtTaxType.Text == "C")
                        {
                           // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                            FillTaxdetials(txtTaxType.Text);
                        }
                        else if (txtTaxType.Text == "V")
                        {
                           // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                            FillTaxdetials(txtTaxType.Text);
                        }
                        else
                        {
                            drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                          
                        }

                    }


                    dsPO = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerSAlePOList", ID, iDealerId, HOBrID);
                    if (dsPO != null)
                    {
                        drpPOList.DataSource = dsPO.Tables[0];
                        drpPOList.DataTextField = "Name";
                        drpPOList.DataValueField = "ID";
                        drpPOList.DataBind();
                        drpPOList.Items.Insert(0, new ListItem("--Select--", "0"));
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

        protected void drpPOList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int POID;
            POID = Func.Convert.iConvertToInt(drpPOList.SelectedValue);
            txtSAlePOID.Text = Func.Convert.sConvertToString(POID);
            GetPOData(POID);

        }
        private void GetPOData(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                
                if (ID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSalePODet", ID, iDealerId, HOBrID, Func.Convert.iConvertToInt(txtSaleDealer.Text));



                    if (ds != null)
                    {

                        Func.Common.BindDataToCombo(drpChassisNo, clsCommon.ComboQueryType.DealerSaleChassis, iDealerId, " and TM_VehicleStock.HoBr_Id=" + iHOBrId
                            + " and M_Chassismaster.Model_Id=" + Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Model_ID"]));


                    }
                    else
                    {
                        drpChassisNo.Items.Insert(0, new ListItem("--Select--", "0"));
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




        protected void drpChassisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ChassisID;
            ChassisID = Func.Convert.iConvertToInt(drpChassisNo.SelectedValue);

            GetChassisData(ChassisID);
        }


        private void GetChassisData(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                if (ID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerSaleChassisDet", ID, iDealerId, HOBrID);
                    //txtCustID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);


                    if (ds != null)
                    {


                        drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Gp"]);
                        drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCat"]);
                        drpModelGroup.SelectedValue = "1";

                        FillModel();
                        drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
                        txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_No"]);
                        txtStockID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Stock_Id"]);
                        //txtPOID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PO_Id"]);
                        txtQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Qty"]);
                        txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelPrice"]);

                        //txtDisc.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Disc"]);
                        txtTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));
                        txtTaxTotalAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(txtModelRate.Text) * Func.Convert.dConvertToDouble(txtQty.Text));
                        FillTaxdetials(txtTaxType.Text);
                        if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
                        {
                            FillTaxDet();
                            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                            {
                                FillTCSTaxDet();
                            }
                        }
                       
                        txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                          Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                          Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                          Func.Convert.dConvertToDouble(txtTCS.Text)
                          );



                        drpModelGroup.Enabled = false;
                        drpModelCode.Enabled = false;
                        drpModelCat.Enabled = false;
                        drpModel.Enabled = false;
                        txtEngineNo.Enabled = false;

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
        //protected void txtVatPer_TextChanged(object sender, EventArgs e)
        //{
        //    drpVATPer.Style.Add("display", "");
        //    //drpCSTPer.Style.Add("display", "");

        //    txtVatPer.Style.Add("display", "none");
        //    //txtCSTPer.Style.Add("display", "none");





        //}

        //protected void txtCSTPer_TextChanged(object sender, EventArgs e)
        //{
        //    drpCSTPer.Style.Add("display", "");
        //    txtCSTPer.Style.Add("display", "none");

        //}
    }
}