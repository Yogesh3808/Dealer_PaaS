using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.IO;

namespace MANART.Forms.VehicleSales
{
    public partial class frmM8 : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtFleetDetails = new DataTable();
        private DataTable dtQuotDetails = new DataTable();
        private DataTable dtClosureDetails = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        private int iDealerID = 0;
        private int iID;
        double MRP = 0.0;
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
        double Price = 0.0;
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

                txtUserType.Text = Session["UserType"].ToString();

                if (txtUserType.Text == "6")
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                    iHOBrId = iDealerId;
                    HOBrID = iDealerId;

                    txtDealerCode.Text = Session["sDealerCodeLoc"].ToString();
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                    txtDealerCode.Text = Session["sDealerCode"].ToString();
                }

                //PDoc.sFormID = "64";

                //ToolbarC.iValidationIdForSave = 65;
                ToolbarC.iFormIdToOpenForm = 68; 

              

                PDoc.sFormID = "54";


                if (!IsPostBack)
                {
                    FillCombo();
                    FillPlant();

                    DisplayPreviousRecord();
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
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
                    ////ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);

                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
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

                PDoc.DocumentGridRowCommand += new EventHandler(PDoc_DocumentGridRowCommand);
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

            FillCombo();
            FillPlant();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            DisplayPreviousRecord();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (txtUserType.Text == "6")
            {
                FillSelectionGrid();
                SearchGrid.sGridPanelTitle = "M8 (Vehicle Invoiced & Delivered to Customer) List";
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
                if (objCommon.sUserRole == "15"|| objCommon.sUserRole == "19")
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

                    //DisplayPreviousRecord();


                    //GenerateLeadNo("PO");
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
                    if (bValidateRecord() == false) return;
                    if (txtGSTNo.Text == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter GSTIN No');</script>");
                        return;
                    }
                    
                    string Temp = Session["M8"].ToString();


            
                    if (Temp == "0" || iID != 0)
                    {
                        Session["M8"] = 1;
                        iID = bHeaderSave("N", "N", "N");
                    }

                    PSelectionGrid.Style.Add("display", "");
                    if (iID > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);

                        if (bSaveDetails("N", "N", "N") == true)
                        {
                          // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M8 ") + "','" + Server.HtmlEncode(txtM8No.Text) + "');</script>");
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
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("M8 ") + "','" + Server.HtmlEncode(txtM8No.Text) + "');</script>");
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
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("M8 ") + "','" + Server.HtmlEncode(txtM8No.Text) + "');</script>");
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
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Cancelled!');</script>");
                        //}
                        
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    }

                }

                FillSelectionGrid();
                GetDataAndDisplay();
                PDoc.BindDataToGrid();
                

            }

            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private bool bValidateRecord()
        {
            bool bValidateRecord = true;
          
                if (drpTaxPer.SelectedValue == "0")
                {
                    sMessage = sMessage + "\\n Please select the Tax.";
                    bValidateRecord = false;
                }
                if (bValidateRecord == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                }
                return bValidateRecord;                               
        }
        private void FillCombo()
        {


            Func.Common.BindDataToCombo(drpModelGroup, clsCommon.ComboQueryType.MPGDetails, 0);
            Func.Common.BindDataToCombo(drpModelCat, clsCommon.ComboQueryType.ModelCategory, 0);
            //Func.Common.BindDataToCombo(drpVehPOType, clsCommon.ComboQueryType.VehPOTypeM1, 0);
            Func.Common.BindDataToCombo(drpM4Financier, clsCommon.ComboQueryType.Financier, 0);

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
                    //drpPlant.DataSource = dsCustType.Tables[0];
                    //drpPlant.DataTextField = "Name";
                    //drpPlant.DataValueField = "ID";
                    //drpPlant.DataBind();
                    //drpPlant.Items.Insert(0, new ListItem("--Select--", "0"));
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
        private void FillTaxDet()
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
                else
                {
                    lblCST.Text = "CST";
                    lblVat.Text = "VAT";
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
                lblCST.Text = "CST";
                lblVat.Text = "VAT";
                lblTax1.Text = "Tax1";
                lblTax2.Text = "Tax2";
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
                        txtCSTAmt.Text =Math.Round(Convert.ToDouble (dsTaxCal.Tables[0].Rows[0]["TaxAmt"]),2).ToString();
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        txtVatAmt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    }
                    else
                    {
                        txtCSTAmt.Text = "0";
                        txtVatAmt.Text = "0";
                    }

                    txttax1Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]),2).ToString();
                    txttax2Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]),2).ToString();


                }

            }
        }

        protected void drpTaxPer_SelectedIndexChanged(object sender, EventArgs e)
           {
            try
            {
               FillSelectedTaxDetials();
               //FillTaxDet();
               // txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
               //   Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
               //   Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
               //   Func.Convert.dConvertToDouble(txtTCS.Text)
               //   );
               // double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 0);
               // txtGrandTotal.Text = grandtotal.ToString();

              //   Math. round function add 
            }
            catch (Exception ex) { }

        }

        private void FillSelectedTaxDetials()
        {
            try
            {
                clsDB objDB = new clsDB();
              //double Revercal =0.0
                double Discount = 0.0;
                DataSet Custdiscout = new DataSet();
                Custdiscout = objDB.ExecuteStoredProcedureAndGetDataset("SP_DiscountDetails", Func.Convert.iConvertToInt(txtAlotID.Text));
                if (Custdiscout.Tables[0].Rows.Count != 0)
                {
                    Discount = Func.Convert.dConvertToDouble(Custdiscout.Tables[0].Rows[0]["Approved_MTIShare"]);
                }
                 DataSet dsCustType = new DataSet();               
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxes", iDealerId, Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                double Total = Func.Convert.dConvertToDouble(txtMRPPrice.Value) - Func.Convert.dConvertToDouble(Discount);
                Price = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", Total, Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
               // double Price = Math.Round(Total / (1 + (Func.Convert.dConvertToDouble(dsCustType.Tables[0].Rows[0]["Tax_Percentage"]) / 100)), 2);
            if (dsCustType != null)
            {

                DataSet dsTaxCal = new DataSet();
                dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_TaxCalculationRFP", Func.Convert.iConvertToInt(drpTaxPer.SelectedValue)
                , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax1ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax2ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp"])
                , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp1"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp2"]), Func.Convert.dConvertToDouble(Price),
                 Func.Convert.dConvertToDouble(0));
                if (dsTaxCal != null)
                {
                    if (txtTaxType.Text == "C")
                    {
                        txtCSTAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                        txtCSTPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                        txtCSTID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                        lblCST.Text = drpTaxPer.SelectedItem.Text;
                        txtVATID.Text = "0";
                    }
                    else if (txtTaxType.Text == "V")
                    {
                        txtVatAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                        txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                        txtVATID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                        txtCSTID.Text = "0";
                        lblVat.Text = drpTaxPer.SelectedItem.Text;

                    }
                    //else if (txtTaxType.Text == "S")
                    //{
                    //    txtSGCTamt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    //    txtSGST.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    //    txtSGSTID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                    //    txtVatAmt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    //    txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    //    txtVATID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                    //    txtIgstiD.Text = "0";
                    //    lblSGST.Text = drpTaxPer.SelectedItem.Text;

                    //}
                    //else if (txtTaxType.Text == "I")
                    //{
                    //    txtIGSTamt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    //    txtIGST.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    //    txtIgstiD.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));
                    //    txtSGSTID.Text = "0";
                    //    lblIGST.Text = drpTaxPer.SelectedItem.Text;
                    //    txtVatAmt.Text = Func.Convert.sConvertToString(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]);
                    //    txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                    //    txtVATID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue));

                    //}
                    else
                    {
                        txtCSTAmt.Text = "0";
                        txtVatAmt.Text = "0";
                   
                    }

                    txttax1Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]), 2).ToString();
                    txttax2Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]), 2).ToString();
                    lblTax1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Desc"]);
                }
                txtTax1ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1ID"]);
                txtTax2ID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax2ID"]);
                txtTaxApp.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp"]);
                txtTaxApp1.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp1"]);
                txtTaxApp2.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TaxApp2"]);
                txttax1Per.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["Tax1Per"]);
                double grandtotal = Math.Round(Convert.ToDouble(Price), 2);
                txtModelRate.Text = Math.Round(Convert.ToDouble(Price), 2).ToString();
                txtTaxTotalAmt.Text = Math.Round(Convert.ToDouble(Price), 2).ToString();
                txtTotalAmt.Text = Math.Round(Convert.ToDouble(Price), 2).ToString();
                double amount = (grandtotal + Func.Convert.dConvertToDouble(txtCSTAmt.Text)) + +Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) + Func.Convert.dConvertToDouble(txtTCS.Text) + Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +Func.Convert.dConvertToDouble(txttax2Amt.Text);
                txtGrandTotal.Text = Math.Round(Convert.ToDouble(amount), 2).ToString();

              }
            }
            catch (Exception)
            {
            }
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
            drpModel.SelectedValue = drpModelCode.SelectedValue;
            clsDB objDB = new clsDB();

            DataSet dsCustType = new DataSet();
            dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetModdelMRP", iDealerId, drpModelCode.SelectedValue);

            //ds = objDB.ExecuteStoredProcedureAndGetDataset("GetM7detailsInVehPO", iDealerID, HOBrID, Func.Convert.iConvertToInt(drpM7Det.SelectedValue));
            if (dsCustType != null)
            {
                txtModelRate.Text = Math.Round(Convert.ToDouble(dsCustType.Tables[0].Rows[0]["ListPrice"]), 2).ToString();
                //drpPlant.SelectedValue = Func.Convert.sConvertToString(dsCustType.Tables[1].Rows[0]["ID"]);
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
                txtModelRate.Text = Math.Round(Convert.ToDouble(dsCustType.Tables[0].Rows[0]["ListPrice"]), 2).ToString();
              
            }

        }
        protected void drpTCSApp_SelectedIndexChanged(object sender, EventArgs e)
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
            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) +
                Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                Func.Convert.dConvertToDouble(txtTCS.Text)
                ));

            //txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
            //    Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
            //    Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
            //    Func.Convert.dConvertToDouble(txtTCS.Text)
            //    );
            double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
            txtGrandTotal.Text = grandtotal.ToString();
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
                txtTCS.Text = Math.Round(Convert.ToDouble(dsCustType.Tables[0].Rows[0]["TCSAmt"]),2).ToString();
                lblTCS.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSDesc"]);
                txtTCSTaxID.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TCSTaxID"]);
            }
        }
        private void FillModel()
        {

            int modelgrp = 0;

            modelgrp = Func.Convert.iConvertToInt(drpModelCat.SelectedValue);

            //if (txtRFPID.Text != "0")
            //{
            //    Func.Common.BindDataToCombo(drpModel, clsCommon.ComboQueryType.ModelNoRate, 0, " and Model_cat_ID=" +
            //        modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //     + iDealerId + "and " + "'" + txtM8Date.Text + "'"
            //     + " not between convert(varchar(10),Effective_From,103) and convert(varchar(10),Effective_To,103) )   "
            //     );



            //    Func.Common.BindDataToCombo(drpModelCode, clsCommon.ComboQueryType.ModelCodeNoRate, 0, " and Model_cat_ID=" + modelgrp + " and Id not in (select model_id from M_ModelRate where Dealer_ID="
            //      + iDealerId + "and " + "'" + txtM8Date.Text + "'"
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


            if (Type == "M8")
            {

                txtM8No.Text = Func.Convert.sConvertToString(FindMAxLeadNo(DealerCode, iDealerId, "M8"));

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
                if (Type == "M8")
                {
                    sDocName = "M8";
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM8", POId, POType, DealerID, HOBrID, iM1ID);
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
                SearchGrid.sGridPanelTitle = "M8 (Vehicle Invoiced & Delivered to Customer) List";
                SearchGrid.AddToSearchCombo("M8 No");
                SearchGrid.AddToSearchCombo("M8 Date");
                SearchGrid.AddToSearchCombo("Customer Name");
                SearchGrid.AddToSearchCombo("Chassis No");
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
                SearchGrid.sSqlFor = "M8Details";
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
                    Session["M8"] = "1";
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


                    //txtM3ID.Text = "N";
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtM8No.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["M8_No"]);
                    txtM8Date.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["M8_Date"], false); 
                    drpM4Financier.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ChangeFinc"]);
                    txtBranch.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Branch"]);
                    txtLoanAmt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Loan_Amt"]),2).ToString();
                    txtMarginMoney.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["MarginMoney"]);
                    txtTenure.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tenure"]);
                    txtInterestRate.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Interest_rate"]);
                    txtModelChange.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Modelchange"]);
                    txtTrailerChanssis.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrailerChassis"]);
                    txtTrailerAmt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrailerAmt"]);
                    txtCashLoan.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Fund"]);
                    txtDONo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["DO_No"]);
                    txtDODate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["DO_Date"], false);
                    txtDOAmt.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["DO_Amt"]);


                    txtCName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Consignee_Name"]);
                    txtCAdd.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Consignee_Add"]);
                    txtCCST.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Consignee_CST"]);
                    txtCVAT.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Consignee_VAT"]);
                    txtCTIN.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Consignee_TIN"]);


                    txtAlotID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AllotId"]);


                    txtRFPID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["RFPID"]);
                    //model details
                    drpModelGroup.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Model_Gp"]);
                    drpModelCat.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Model_Cat"]);
                    drpModelGroup.SelectedValue = "1";
                    FillModel();
                    drpModel.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Model_ID"]);
                    drpModelCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Model_ID"]);

                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Chassis_no"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Engine_No"]);

                    txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["name"]);
                    txtModelRate.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Rate"]);
                    MRP = Func.Convert.dConvertToDouble(ds.Tables[1].Rows[0]["Rate"]);
                    txtMRPPrice.Value = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["FinalMRP"]);
                    txtTaxType.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Taxtype"]);
                    txtM7ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["M7_Hdr"]);
                    txtQty.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Qty"]);
                  
                   
                    txtTotalAmt.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Total"]),2).ToString();
                    txtTaxTotalAmt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Total"]),2).ToString();
                    txtGrandTotal.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]),2).ToString();

                    //Display Details
                    hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Confirm"]);
                    hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IS_Cancel"]);
                    txtRoundoff.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Roundoff"]);


                    txtDisc.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Disc"]),2).ToString();
                    hdntaxtype.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTType"]);
                    txtGSTNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["GSTNo"]);
                    if (txtTaxType.Text == "C")
                    {

                        FillTaxdetials(txtTaxType.Text,Convert.ToInt32(ds.Tables[1].Rows[0]["Model_ID"]));
                       
                      // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.CSTTax, iDealerId, " and HOBr_ID=" + iHOBrId);
                        if (Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]) != "0")
                        {
                            drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);
                        }
                        else
                        {
                            drpTaxPer.SelectedValue = hiddentaxID.Value;
                        }
                        txtCSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CST"]);
                        txtVATID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTApp"]);
                    }
                    else if (txtTaxType.Text == "V")                    {
                       
                        FillTaxdetials(txtTaxType.Text, Convert.ToInt32(ds.Tables[1].Rows[0]["Model_ID"]));
                        if (Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]) != "0")
                        {
                            drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                        }
                        else
                        {
                            drpTaxPer.SelectedValue = hiddentaxID.Value;
                        }                        
                       // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                      
                        txtVATID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VAT"]);
                        txtCSTID.Text = "0";
                        txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATApp"]);
                    }
                    //else if (txtTaxType.Text == "S")
                    //{
                    //    FillTaxdetials(txtTaxType.Text);
                    //    // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                    //    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGST"]);
                    //    //txtSGSTID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGST"]);
                    //    //  txtIgstiD.Text = "0";
                    //    txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATApp"]);
                    //}
                    //else if (txtTaxType.Text == "I")
                    //{
                    //    FillTaxdetials(txtTaxType.Text);
                    //    // Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.VATTax, iDealerId, " and  HOBr_ID=" + iHOBrId);
                    //    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGST"]);
                    //    //txtIgstiD.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGST"]);
                    //    //txtSGSTID.Text = "0";
                    //    txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATApp"]);
                    //}
                    else
                    {
                        drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                        drpTaxPer.SelectedValue = "0";
                        txtVATID.Text = "0";
                        txtCSTID.Text = "0";
                        //txtIgstiD.Text = "0";
                        //txtSGSTID.Text = "0";
                        txtTaxApp.Text = "0";
                    }


                    txtPFCharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["PFCharges"]);
                    txtOthercharges.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Other"]);
                    txtTax1ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["tax1"]);
                    txtTax2ID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["tax2"]);
                    txtCSTAmt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["CSTAmt"]),2).ToString();
                    
                    txtVatAmt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["VATAmt"]),2).ToString();
                    //txtIGSTamt.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["IGSTamt"]), 2).ToString();
                    //txtSGCTamt.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["SGSTamt"]), 2).ToString();
                    txttax1Amt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Tax1Amt"]),2).ToString();
                    txttax2Amt.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["Tax2Amt"]),2).ToString();


                    drpTCSApp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_App"]);
                    txtTCSTaxID.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCSPer"]);
                    txtTCS.Text =Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["TCSAmt"]), 2).ToString();
                    txtTCSPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCS_Per"]);

                    lblCST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTDesc"]);
                    lblVat.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATDesc"]);
                    lblTax1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TAx1Desc"]);
                    lblTax2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2Desc"]);
                    lblTCS.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["TCSDesc"]);

                    txtCSTPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTPer"]);
                    txtVatPer.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATPer"]);
                   // txtSGST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["SGSTper"]);
                   //txtIGST.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["IGSTper"]);
                    txttax1Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1Per"]);
                    txtTax2Per.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2Per"]);

                    txtTaxApp1.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax1App"]);
                    txtTaxApp2.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Tax2App"]);

                    if (drpTaxPer.SelectedValue == "0")
                    {
                        FillTaxDet1();
                    }
                    else
                    {
                        FillSelectedTaxDetials();
                    }             

                    if (txtRoundoff.Text=="Y")
                    {
                        txtGrandTotal.Text = Math.Round(Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]), 2).ToString();
                    }

                }
              
                hdnLost.Value = "N";

                if (txtUserType.Text == "6")
                {
                    PDoc.Visible = false;
                }
                else
                {
                    PDoc.Visible = true;
                }

                if (txtID.Text == "0")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                else
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }

                //if (txtTaxType.Text == "C")
                //{
                //    drpVATPer.Enabled = false;
                //    drpCSTPer.Enabled = true;
                //}
                //else if (txtTaxType.Text == "V")
                //{
                //    drpCSTPer.Enabled = false;
                //    drpVATPer.Enabled = true;
                //}
                //else
                //{
                //    drpCSTPer.Enabled = false;
                //    drpVATPer.Enabled = false;
                //}



                //Display Attachment Details
                dtFileAttach = ds.Tables[2];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();
                lbldiscount.Visible = false; tddiscbank.Visible = false; tdtxtdisc.Visible = false;
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerGStType"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTType"]) != "N" )
                {
                    lblCST.Text = "IGST";
                    lblVat.Text = "SGST";
                   lblTax1.Text = "CGST";
                   tdlbl.Visible = false; tdper.Visible = false; tdmount.Visible = false;
                }
                else
                {
                    lblCST.Text = "CST";
                    lblVat.Text = "VAT";
                    lblTax1.Text = "TAX1";
                    tdvatamt.Visible = true; tdvatlbl.Visible = true; tdvattax.Visible = true; trtcs.Visible = true; tdlbl.Visible = true; tdper.Visible = true; tdmount.Visible = true; }

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
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    //bConvertToInq.Enabled = false;
                    //bHold.Enabled = false;
                }




            }
           catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            
        }

    

        private void FillTaxdetials(string Taxtype,int Model_ID)
        {
            try
            {
                hiddentaxID.Value = "0";
                clsDB objDB = new clsDB();
                if (txtUserType.Text == "6")
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8HSNDetailsFill", Func.Convert.iConvertToInt(Session["DealerID"]), Convert.ToString(hdntaxtype.Value), Taxtype, Model_ID);
                }
                else
                {
                    dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8HSNDetailsFill", Func.Convert.iConvertToInt(Session["iDealerID"]), Convert.ToString(hdntaxtype.Value), Taxtype, Model_ID);
                }
                if (dsState != null)
                {
                    drpTaxPer.DataSource = dsState.Tables[0];
                    drpTaxPer.DataTextField = "Name";
                    drpTaxPer.DataValueField = "ID";
                    drpTaxPer.DataBind();
                    drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
                    hiddentaxID.Value = Func.Convert.sConvertToString(dsState.Tables[0].Rows[0]["ID"]);
                }
                else
                {
                    drpTaxPer.Items.Clear();
                }

            }
            catch (Exception ex)
            {
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
                GenerateLeadNo("M8");
            }

            HdrID = objLead.bSaveM8Header(iID, iDealerId, HOBrID,
                 txtM8No.Text, txtM8Date.Text, Func.Convert.iConvertToInt(txtM7ID.Text), Confirm, Cancel, Func.Convert.iConvertToInt(drpM4Financier.SelectedValue)
                 , Func.Convert.dConvertToDouble(txtLoanAmt.Text), Func.Convert.dConvertToDouble(txtMarginMoney.Text),
                 Func.Convert.iConvertToInt(txtTenure.Text), Func.Convert.dConvertToDouble(txtInterestRate.Text),
                    txtModelChange.Text, txtTrailerChanssis.Text, Func.Convert.dConvertToDouble(txtTrailerAmt.Text),
                    Func.Convert.iConvertToInt(txtCSTID.Text), Func.Convert.iConvertToInt(txtVATID.Text),
                    Func.Convert.dConvertToDouble(txtPFCharges.Text), Func.Convert.dConvertToDouble(txtOthercharges.Text),
                    Func.Convert.dConvertToDouble(txtTotalAmt.Text), Func.Convert.dConvertToDouble(txtModelRate.Text), Func.Convert.dConvertToDouble(txtGrandTotal.Text), 
                    Func.Convert.iConvertToInt(txtTax1ID.Text), Func.Convert.iConvertToInt(txtTax2ID.Text),
                    Func.Convert.dConvertToDouble(txtCSTAmt.Text), Func.Convert.dConvertToDouble(txtVatAmt.Text),  Func.Convert.dConvertToDouble(txttax1Amt.Text),
                    Func.Convert.dConvertToDouble(txttax2Amt.Text), Func.Convert.iConvertToInt(txtTCSTaxID.Text), Func.Convert.dConvertToDouble(txtTCS.Text),
                    Func.Convert.iConvertToInt(txtAlotID.Text), Func.Convert.dConvertToDouble(txtDisc.Text), Func.Convert.iConvertToInt(txtQty.Text),
                    Func.Convert.iConvertToInt(drpTCSApp.SelectedValue),txtTaxType.Text,txtDONo.Text,txtDODate.Text,Func.Convert.dConvertToDouble(txtDOAmt.Text)
                    , txtBranch.Text, txtChassisNo.Text, txtCName.Text, txtCAdd.Text, txtCCST.Text, txtCVAT.Text, txtCTIN.Text, txtGSTNo.Text 

             );


            return HdrID;

        }

        public bool bSaveDetails(string Cancel, string Confirm, string OrderStatus)
        {
            clsVehicle objLead = new clsVehicle();
            clsDB objDB = new clsDB();

            DataSet ds = new DataSet();

            if (bSaveAttachedDocuments() == false) return bSaveRecord;
            if (objLead.bSaveM8FileAttachDetails
                (objDB, dtFileAttach, iID) == true)
            {
                bSaveRecord = true;
            }
            else
            {
                bSaveRecord = false;
            }

            return bSaveRecord;

        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable, string type)
        {


            txtM8No.Enabled = false;
            txtM8Date.Enabled = false;
           
            txtModelChange.Enabled = bEnable;
            txtTrailerChanssis.Enabled = bEnable;
            txtTrailerAmt.Enabled = bEnable;

            txtCName.Enabled = bEnable;
            txtCAdd.Enabled = bEnable;
            txtCCST.Enabled = bEnable;
            txtCVAT.Enabled = bEnable;
            txtCTIN.Enabled = bEnable;


            if (txtCashLoan.Text == "C")
            {
                drpM4Financier.Enabled = false;
                txtLoanAmt.Enabled = false;
                txtMarginMoney.Enabled = false;
                txtTenure.Enabled = false;
                txtInterestRate.Enabled = false;
                txtBranch.Enabled = false;
                txtDONo.Enabled = false;
                txtDODate.Enabled = false;
                txtDOAmt.Enabled = false;
            }
            else
            {
                drpM4Financier.Enabled = bEnable;
                txtBranch.Enabled = bEnable;
                txtLoanAmt.Enabled = bEnable;
                txtMarginMoney.Enabled = bEnable;
                txtTenure.Enabled = bEnable;
                txtInterestRate.Enabled = bEnable;

                txtDONo.Enabled = bEnable;
                txtDODate.Enabled = bEnable;
                txtDOAmt.Enabled = bEnable;
            }

            //model details
            drpModelGroup.Enabled = false;
            drpModelCat.Enabled = false;
            drpModel.Enabled = false;
            drpModelCode.Enabled = false;

            txtChassisNo.Enabled = false;
            txtEngineNo.Enabled = false;

            txtCustName.Enabled = false;
            txtModelRate.Enabled = bEnable;

            txtQty.Enabled = false;


            txtTotalAmt.Enabled = false;
            txtTaxTotalAmt.Enabled = false;
            txtGrandTotal.Enabled = false;



            drpTaxPer.Enabled = bEnable;




            txtPFCharges.Enabled = bEnable;
            txtOthercharges.Enabled = bEnable;

            txtCSTAmt.Enabled = false;
            txtVatAmt.Enabled = false;
            //txtSGCTamt.Enabled = false;
            //txtIGST.Enabled = false;
            txttax1Amt.Enabled = false;
            txttax2Amt.Enabled = false;


            drpTCSApp.Enabled = bEnable;
            txtGSTNo.Enabled = bEnable;


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
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, bEnable);

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);
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

        private void FillEnquirySave()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCustType = new DataSet();


                //dsCustType = objDB.ExecuteQueryAndGetDataset("SELECT   ID, Custtype_name as Name FROM M_CustType where Cust_Sup='C'");
                dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEnquirySave", iDealerId, HOBrID);

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




        protected void PDoc_DocumentGridRowCommand(object sender, EventArgs e)
        {
            try
            {
                
                 txtPreviousDocId.Text = Func.Convert.sConvertToString(PDoc.PDocID);
                //txtCashLoan.Text = Func.Convert.sConvertToString(PDoc.sDoc);

                FillCombo();
                FillSelectionGrid();

                PSelectionGrid.Style.Add("display", "none");
                txtID.Text = "";

                GetDataFromAllocation();
                Session["M8"] = 0;


                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);

            }
            catch (Exception ex) { }
        }
        private void GetDataFromAllocation()
        {
            try
            {
              
                DataSet ds = new DataSet();
                int iM0ID = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
              
                if (iM0ID != 0)
                {
                   ds = GetM8(iID, "New", iDealerId, iHOBrId, iM0ID);

                    DisplayData(ds);

                }
                
               
              
                GenerateLeadNo("M8");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void GetDataFromPO()
        {
            try
            {
                //clsCustomer ObjDealer = new clsCustomer();
                DataSet ds = new DataSet();
                int iID = Func.Convert.iConvertToInt(txtPreviousDocId.Text); ;
                //iProformaID = 15;
                if (iID != 0)
                {
                    ds = GetM8(iID, "All", iDealerId, iHOBrId, 0);

                    //txtInqNo.Text = "";


                    DisplayData(ds);
                    //ObjDealer = null;
                }

                txtM3ID.Text = "Y";

                //txtID.Text = "";
                ////txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "VORF", Location.iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, (txtID.Text == "" || txtID.Text == "0") ? false : true);
                //ds = null;
                //ObjProforma = null;

                GenerateLeadNo("POA");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void txtDisc_TextChanged(object sender, EventArgs e)
        {
            try
            {               
                if (Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) != 0)
                {
                    FillTaxDet();
                    if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                    {
                        FillTCSTaxDet();
                    }
                }
                txtGrandTotal.Text =Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                  Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                  Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                  Func.Convert.dConvertToDouble(txtTCS.Text)
                  );
                double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
                txtGrandTotal.Text = grandtotal.ToString();
            }
            catch (Exception ex) { }
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
            double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
            txtGrandTotal.Text = grandtotal.ToString();
        }

        protected void txtPFCharges_TextChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
            {
                FillTCSTaxDet();
            }

            txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) +
             Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
             Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
             Func.Convert.dConvertToDouble(txtTCS.Text)
             ));

            //txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
            //  Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
            //  Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
            //  Func.Convert.dConvertToDouble(txtTCS.Text)
            //  );
            double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
            txtGrandTotal.Text = grandtotal.ToString();
        }

        protected void txtOthercharges_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Func.Convert.iConvertToInt(drpTCSApp.SelectedValue) != 0)
                {
                    FillTCSTaxDet();
                }

                txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) +
                 Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                 Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                 Func.Convert.dConvertToDouble(txtTCS.Text)
                 ));

                //txtGrandTotal.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtTaxTotalAmt.Text) - Func.Convert.dConvertToDouble(txtDisc.Text)) +
                //  Func.Convert.dConvertToDouble(txtVatAmt.Text) + Func.Convert.dConvertToDouble(txtCSTAmt.Text) + Func.Convert.dConvertToDouble(txttax1Amt.Text) +
                //  Func.Convert.dConvertToDouble(txttax2Amt.Text) + Func.Convert.dConvertToDouble(txtPFCharges.Text) + Func.Convert.dConvertToDouble(txtOthercharges.Text) +
                //  Func.Convert.dConvertToDouble(txtTCS.Text)
                //  );
                double grandtotal = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
                txtGrandTotal.Text = grandtotal.ToString();
            }
            catch (Exception ex) { }
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
                Response.TransmitFile((sPath + "Vehicle Sale\\M8" + "\\" + fileNames));
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
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
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

                            
                                sSourceFileName1 = Func.Convert.sConvertToString(iDealerId) + "_" + txtM8No.Text + "_" + sSourceFileName;

                            
                            

                            sSourceFileName1 = sSourceFileName1.Replace("/", "");
                            dr["File_Names"] = sSourceFileName1;
                            //dr["File_Names"] = Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                            dr["UserId"] = Func.Convert.sConvertToString(Session["UserID"]);
                            dr["Status"] = "S";


                            //Saving it in temperory Directoryt.                                       
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Vehicle Sale\\M8");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }

                            //uploads[i].SaveAs((sPath + "Parts\\Part Claim" + "\\" + Func.Convert.sConvertToString(Session["UserID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));
                            uploads[i].SaveAs((sPath + "Vehicle Sale \\M8" + "\\" + sSourceFileName1));

                            strNewPath = sPath + "Vehicle Sale\\M8" + "\\" + sSourceFileName1;
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
            return true;
        }
        //private void bFillDetailsFromFileAttachGrid()
        //{
        //    DataRow dr;
        //    dtFileAttach = new DataTable();
        //    //Get Header InFormation        
        //    dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
        //    dtFileAttach.Columns.Add(new DataColumn("Description", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("File_Names", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("UserId", typeof(int)));
        //    dtFileAttach.Columns.Add(new DataColumn("Status", typeof(string)));
        //    dtFileAttach.Columns.Add(new DataColumn("Path", typeof(string)));
        //    CheckBox ChkForDelete;
        //    for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
        //    {
        //        if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
        //        {
        //            dr = dtFileAttach.NewRow();
        //            dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
        //            dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
        //            dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
        //            dr["UserId"] = Func.Convert.iConvertToInt(Session["UserID"]);

        //            ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));
        //            Label lblDelete = (Label)(FileAttchGrid.Rows[iGridRowCnt].FindControl("lblDelete"));

        //            if (ChkForDelete.Checked == true)
        //            {
        //                dr["Status"] = "D";
        //            }
        //            else
        //            {
        //                dr["Status"] = "S";
        //            }
        //            dtFileAttach.Rows.Add(dr);
        //            dtFileAttach.AcceptChanges();
        //        }
        //    }
        //}

        private void bFillDetailsFromFileAttachGrid()
        {
            bool bSaveRecord = false;
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
        }

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if ( dtFileAttach.Rows.Count != 0)
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

        private void FillTaxDet1()
          {
              try
              {
                  clsDB objDB = new clsDB();
                  DataSet Custdiscout = new DataSet();
                  double Discount = 0.0;
                  Custdiscout = objDB.ExecuteStoredProcedureAndGetDataset("SP_DiscountDetails", Func.Convert.iConvertToInt(txtAlotID.Text));
                  if (Custdiscout.Tables[0].Rows.Count != 0)
                  {
                      Discount = Func.Convert.dConvertToDouble(Custdiscout.Tables[0].Rows[0]["Approved_MTIShare"]);
                  }
                  DataSet taxdetails = new DataSet();
                  taxdetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_M8TaxDetails", iDealerId, Func.Convert.iConvertToInt(txtAlotID.Text));
                  int TaxID = 0;
                  if (taxdetails != null)
                  {
                      TaxID = Func.Convert.iConvertToInt(taxdetails.Tables[0].Rows[0]["ID1"].ToString());
                      drpTaxPer.SelectedValue = TaxID.ToString();
                  }
                  DataSet dsCustType = new DataSet();
                  dsCustType = objDB.ExecuteStoredProcedureAndGetDataset("GetAdditionalTaxes", iDealerId, TaxID);
                  double Total =Func.Convert.dConvertToDouble(txtMRPPrice.Value) - Func.Convert.dConvertToDouble(Discount);
                  if (dsCustType.Tables[0].Rows.Count > 0)
                  {
                      Price = Math.Round(Total / (1 + (Func.Convert.dConvertToDouble(dsCustType.Tables[0].Rows[0]["Tax_Percentage"]) / 100)), 2);
                        DataSet dsTaxCal = new DataSet();
                      dsTaxCal = objDB.ExecuteStoredProcedureAndGetDataset("SP_TaxCalculationRFP", TaxID
                      , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax1ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["Tax2ID"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp"])
                      , Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp1"]), Func.Convert.iConvertToInt(dsCustType.Tables[0].Rows[0]["TaxApp2"]), Func.Convert.dConvertToDouble(Price),
                       Func.Convert.dConvertToDouble(0));
                      if (dsTaxCal != null)
                      {

                         
                          if (txtTaxType.Text == "C")
                          {
                              txtCSTAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                              txtCSTID.Text = Func.Convert.sConvertToString(TaxID);
                              txtVATID.Text = "0";
                              txtCSTPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                             // txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["CSTApp"]);
                          }
                          else if (txtTaxType.Text == "V")
                          {
                              txtVatAmt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[0].Rows[0]["TaxAmt"]), 2).ToString();
                              txtVATID.Text = Func.Convert.sConvertToString(TaxID);
                              txtCSTID.Text = "0";
                              txtVatPer.Text = Func.Convert.sConvertToString(dsCustType.Tables[0].Rows[0]["TAX_Percentage"]);
                              //txtTaxApp.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["VATApp"]);
                          }
                          else
                          {
                              txtCSTAmt.Text = "0";
                              txtVatAmt.Text = "0";
                          }
                          txttax1Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[1].Rows[0]["Tax1Amt"]), 2).ToString();
                          txttax2Amt.Text = Math.Round(Convert.ToDouble(dsTaxCal.Tables[2].Rows[0]["Tax2Amt"]), 2).ToString();
                      }

                      // double Total =  Func.Convert.dConvertToDouble(txtModelRate.Text) - Func.Convert.dConvertToDouble(txtDisc.Text);




                      double grandtotal = Math.Round(Convert.ToDouble(Price), 2);
                      txtModelRate.Text = Math.Round(Convert.ToDouble(Price), 2).ToString();
                      txtTaxTotalAmt.Text = Price.ToString();
                      txtTotalAmt.Text = Price.ToString();
                      //     double amount =grandtotal / 1 + (14.5 / 100) - Func.Convert.dConvertToDouble(txtCSTAmt.Text);
                      double amount = (grandtotal + Func.Convert.dConvertToDouble(txtCSTAmt.Text))+Func.Convert.dConvertToDouble(txtVatAmt.Text);
                      txtGrandTotal.Text = Math.Round(Convert.ToDouble(amount), 2).ToString();

                  }

              }
              catch (Exception ex) { }
        }

        //private void FillTaxdetials()
        //{
        //    try
        //    {
        //        clsDB objDB = new clsDB();
        //        dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", Func.Convert.iConvertToInt(Session["iDealerID"]), "Taxdetails");
        //        if (dsState != null)
        //        {
        //            drpTaxPer.DataSource = dsState.Tables[0];
        //            drpTaxPer.DataTextField = "Name";
        //            drpTaxPer.DataValueField = "ID";
        //            drpTaxPer.DataBind();
        //            drpTaxPer.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}
    }
}