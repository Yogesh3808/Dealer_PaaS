using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using System.IO;
using System.Data.OleDb;

namespace MANART.Forms.Spares
{
    public partial class frmPartsSaleInvoice : System.Web.UI.Page
    {
        string sFileName = "";
        string InvNo = "";
        private int iInvID = 0;
        int DealerID = 0;
        private DataTable dtDetails = new DataTable();
        private DataTable dtInvTaxDetails = new DataTable();
        private DataTable dtInvGrpTaxDetails = new DataTable();
        private DataTable dtInvJobDescDet = new DataTable();
        private DataSet dsCreatedBy = new DataSet();
        private bool bDetailsRecordExist = false;
        clsEGPSparesInvoice objInv = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        string sCreateGP = "";
        private int UsreType;
        int iUser_ID = 0;
        int iHOBrId = 0;
        private int iDealerID = 0;
        int iMenuId = 0;
        private string sParaValue = "";
        private bool sDiscChange = false;
        string sDealerCode = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 64;
                ToolbarC.iValidationIdForConfirm = 64;
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iFormIdToOpenForm = 102; //print option

                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();
                iUser_ID = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBrId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                hdnMenuID.Value = Func.Convert.sConvertToString(Request.QueryString["MenuID"]);
                sParaValue = Func.Common.GetDealerWiseSystemParameters(iDealerID, iHOBrId, 4);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                //For MD User
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }

                //For MD User
                if (!IsPostBack)
                {
                    Session["PartDetails"] = null;
                    Session["InvTaxDetails"] = null;
                    Session["InvGrpTaxDetails"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                    lblSelectModel.Style.Add("display", (iMenuId == 687) ? "" : "none");
                }

                SearchGrid.sGridPanelTitle = "Invoice List";

                FillSelectionGrid();
                if (iInvID != 0)
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
                //SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PartDetails"] = null;
                Session["InvTaxDetails"] = null;
                Session["InvGrpTaxDetails"] = null;
                //GenerateInvNo();
                //GetCreatedBy();
                DisplayPreviousRecord();
            }
            Location.drpCountryIndexChanged += new EventHandler(Location_drpCountryIndexChanged);
            Location.drpRegionIndexChanged += new EventHandler(Location_drpRegionChanged);
            //hdnIsWithOA.Value = drpReferType.SelectedValue;
            hdnIsWithOA.Value = "N";

            //DrpCustomerType.SelectedValue = DrpCustomer.SelectedValue;
            //if (Location.iDealerId != 0)
            //{
            //    FillSelectionGrid();
            //    DisplayCurrentRecord();
            //    PSelectionGrid.Style.Add("display", "");
            //}
        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //GenerateInvNo();
                //GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            try
            {
                //GenerateInvNo();
                //GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "') And Cust_Type != 6");
            Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and ISULL(CM.Active,'N')='Y'");
            DrpCustomer.SelectedValue = "0";

            //Func.Common.BindDataToCombo(DrpCustomerType, clsCommon.ComboQueryType.Customer_Type, 0, " And ( DealerID='" + Location.iDealerId + "')");
            //DrpCustomerType.SelectedValue = DrpCustomer.SelectedValue;

            Func.Common.BindDataToCombo(DrpInvType, clsCommon.ComboQueryType.InvType, 0, (iMenuId == 687) ? ((sParaValue == "N") ? " and Id in (3,4)" : " and Id in (5,6,7,8)") : (sDealerCode.Trim().StartsWith("R")) ? "and Id in (9)" : " and Id in (1,2,9)");
            DrpInvType.SelectedValue = "0";
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iInvID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objInv = new clsEGPSparesInvoice();
                if (iInvID != 0)
                {
                    ds = objInv.GetInv(iInvID, "All", Location.iDealerId, 0, iMenuId);
                    sNew = "N";
                    DisplayData(ds);
                    objInv = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objInv = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void DisplayPreviousRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                // int iDealerId = 3;
                DealerID = Location.iDealerId;

                objInv = new clsEGPSparesInvoice();
                ds = objInv.GetInv(iInvID, "New", DealerID, 0, iMenuId);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Inv_canc"] = "N";
                            ds.Tables[0].Rows[0]["Inv_lock"] = "N";
                            ds.Tables[0].Rows[0]["Inv_com"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtInvDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                objInv = null;
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
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                if (sNew == "N")
                {
                    txtInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_no"]);
                    txtInvDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_Date"]);
                    lblSelectModel.Style.Add("display", "none");
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);

                    Func.Common.BindDataToCombo(DrpInvType, clsCommon.ComboQueryType.InvType, 0, (iMenuId == 687) ? ((sParaValue == "N") ? " and Id in (3,4,5,6,7,8)" : " and Id in (3,4,5,6,7,8)") : " and Id in (1,2,3,4,5,6,7,8,9)");
                    DrpInvType.SelectedValue = "0";
                    hdnInvTypeID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_type"]);
                    if (Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 9)//And CM.Cust_Type <> 18
                        Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.CustomerBranchGSTNO), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') != ISNULL(CM.GST_No,'') ");
                    else
                        Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, ((iMenuId == 687) ? "" : "") + " and DealerID=" + Location.iDealerId + " and Cust_Type != 6 ");
                }
                else
                {
                    if (Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 9)
                        Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.CustomerBranchGSTNO), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') != ISNULL(CM.GST_No,'') and ISNULL(CM.Active,'N')='Y'");
                    else
                        Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and CM.Active='Y'");
                }
                DrpCustomer.SelectedValue = "0";
                // For MD User
                //if (txtUserType.Text == "6")
                //{
                //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "')");
                //Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and CM.Active='Y'");
                //if (Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 9)
                //    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.CustomerBranchGSTNO, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') != ISNULL(CM.GST_No,'')");
                //else
                //    Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687 || hdnInvTypeID.Value == "3" || hdnInvTypeID.Value == "4" || hdnInvTypeID.Value == "5" || hdnInvTypeID.Value == "6" || hdnInvTypeID.Value == "7" || hdnInvTypeID.Value == "8") ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, " And ( DealerID='" + Location.iDealerId + "') And Cust_Type != 6");
                //DrpCustomer.SelectedValue = "0";
                //}                  

                // For MD User
                txtRefernce.Enabled = true;
                DrpInvType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_type"]);
                DrpCustomer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                //DrpCustomerType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                DrpInvType.Enabled = (sNew == "N") ? false : true;
                DrpCustomer.Enabled = (sNew == "N") ? false : true;

                txtTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["net_tr_amt"]);
                txtRefernce.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["reference"]);
                hdnJobHDRID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Job_HDR_ID"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);

                hdnGPID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GPHDRID"]);
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PerAmt"]) == "Per")
                    rbtLstDiscount.SelectedValue = "Per";
                else
                    rbtLstDiscount.SelectedValue = "Amt";
                //Commented on 16092017 VikramK Begin
                //rbtLstDiscount.Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");
                //END
                //txtNarration.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["narration"]);
                //txtValidDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_validity_date"]);
                // ChkScheme.Checked = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Scheme"]) == "Y") ? true : false;
                txtInvDate.Enabled = false;

                Session["PartDetails"] = null;
                Session["InvTaxDetails"] = null;
                Session["InvGrpTaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["PartDetails"] = dtDetails;

                dtInvGrpTaxDetails = ds.Tables[2];
                Session["InvGrpTaxDetails"] = dtInvGrpTaxDetails;

                dtInvTaxDetails = ds.Tables[3];
                Session["InvTaxDetails"] = dtInvTaxDetails;

                dtInvJobDescDet = ds.Tables[4];
                Session["InvJobDescDet"] = dtInvJobDescDet;


                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_lock"]);
                hdnCancel.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_canc"]);
                lnkGatePass.Visible = false; // Add condition after create Gatepass
                BindDataToGrid();
                if (sNew == "Y" && hdnMenuID.Value == "687")
                {
                    FillDetailsFromGrid(false, false);
                    CreateNewRowToTaxGroupDetailsTable();
                    Session["PartDetails"] = dtDetails;
                    Session["InvTaxDetails"] = dtInvTaxDetails;
                    Session["InvGrpTaxDetails"] = dtInvGrpTaxDetails;
                    BindDataToGrid();
                }
                BtnOpen.Visible = (hdnConfirm.Value == "Y" && (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8)) ? true : false;

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                // If Record is Confirm or cancel then it is not editable      
                lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + Func.Convert.sConvertToString(Session["HOBR_ID"]) + "')");

                if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8)
                {
                    txtRefernce.Enabled = false;
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_lock"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                    if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8)
                    {
                        lnkGatePass.Visible = true; // Add condition after create Gatepass
                        lnkGatePass.Attributes.Add("onclick", " return GetGatePassDtls(this,'" + Location.iDealerId.ToString() + "','" + txtID.Text.ToString().Trim() + "','0')");
                    }
                    else
                    {// condtion for Counter Sale
                        if (hdnGPID.Value == "0" || hdnGPID.Value == "")
                            lnkGatePass.Visible = false;
                        else
                            lnkGatePass.Visible = true; // Add condition after create Gatepass
                        lnkGatePass.Attributes.Add("onclick", " return GetGatePassDtls(this,'" + Location.iDealerId.ToString() + "','" + txtID.Text.ToString().Trim() + "','129')");
                    }
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inv_canc"]) == "Y")
                {
                    bEnableControls = false;
                }
                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);
                }
                if (txtUserType.Text.Trim() == "6")
                {
                    DrpCustomer.Enabled = false;
                    DrpInvType.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form        
            //txtInvDate.Enabled = bEnable;
            //drpPoType.Enabled = bEnable;


            PartGrid.Enabled = bEnable;
            GrdPartGroup.Enabled = bEnable;
            GrdDocTaxDet.Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }
        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //GenerateInvNo();
                //GetCreatedBy();
                FillSelectionGrid();
                if (txtUserType.Text != "6")
                {
                    DisplayCurrentRecord();
                }
                if (txtUserType.Text == "6")
                {
                    DisplayPreviousRecord();
                }
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
                //int iDealerId = 3;
                DealerID = Location.iDealerId;
                objInv = new clsEGPSparesInvoice();
                ds = objInv.GetInv(iInvID, "Max", DealerID, 0, iMenuId);
                if (ds == null) // if no Data Exist
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    sNew = "N";
                    DisplayData(ds);
                }
                else
                {
                    DisplayData(ds);
                    Session["PartDetails"] = null;
                    Session["InvTaxDetails"] = null;
                    Session["InvGrpTaxDetails"] = null;

                    BindDataToGrid();

                }
                ds = null;
                objInv = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    Func.Common.BindDataToCombo(DrpInvType, clsCommon.ComboQueryType.InvType, 0, (iMenuId == 687) ? ((sParaValue == "N") ? " and Id in (3,4)" : " and Id in (5,6,7,8)") : (sDealerCode.Trim().StartsWith("R")) ? "and Id in (9)" : " and Id in (1,2,9)");
                    DrpInvType.Enabled = true;
                    DrpInvType.SelectedValue = "0";
                    DisplayPreviousRecord();
                    //if (Location.iDealerId != 0)
                    //{
                    //    GenerateInvNo();
                    //    //GetCreatedBy();
                    //}
                    txtInvNo.Text = "";
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false, false) == false) return;
                    if (txtID.Text == "")
                    {
                        FillSelectionGrid();
                        DisplayCurrentRecord();
                        PSelectionGrid.Style.Add("display", "");
                        return;
                    }
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bSaveRecord(true, false) == false) return;
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (bSaveRecord(false, true) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                iInvID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
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
            CPEPartDetails.Collapsed = false;
            btnReadonly();
        }
        // Function Use for Readonly User
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
                else if (objCommon.sUserRole == "19")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Inv No");
                SearchGrid.AddToSearchCombo("Inv Date");
                SearchGrid.AddToSearchCombo("Inv Status");
                if (Func.Convert.sConvertToString(Request.QueryString["MenuID"]) == "687")
                {
                    SearchGrid.AddToSearchCombo("Job No");
                    SearchGrid.AddToSearchCombo("Chassis No");
                }
                SearchGrid.iDealerID = Location.iDealerId;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iMenuId);
                SearchGrid.sSqlFor = "SparesInv";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                string cntPartID = "";
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Inv_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_type", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("Inv_close", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_canc", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_com", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_lock", typeof(string)));

                ////net_tr_amt,reference,narration,Inv_validity_date
                dtHdr.Columns.Add(new DataColumn("net_tr_amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("reference", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("narration", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inv_validity_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Scheme", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Job_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("HOBR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("IS_PerAmt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Inv_no"] = txtInvNo.Text;
                dr["Inv_Date"] = txtInvDate.Text;
                dr["Inv_type"] = Func.Convert.iConvertToInt(DrpInvType.SelectedValue);

                dr["CustID"] = Func.Convert.iConvertToInt(DrpCustomer.SelectedValue);
                dr["Dealer_ID"] = Location.iDealerId;
                dr["Inv_close"] = "N";
                dr["Inv_canc"] = "N";
                dr["Inv_com"] = "N";
                dr["Inv_lock"] = "N";

                dr["net_tr_amt"] = 0;
                dr["reference"] = txtRefernce.Text;
                dr["narration"] = "";
                dr["HOBR_ID"] = iHOBrId;
                dr["Inv_validity_date"] = txtInvDate.Text; //txtValidDate.Text;
                dr["Job_HDR_ID"] = Func.Convert.iConvertToInt(hdnJobHDRID.Value); //txtValidDate.Text;
                //dr["Scheme"] = (ChkScheme.Checked == true) ? "Y" : "N"; //txtValidDate.Text;
                dr["Scheme"] = "";
                dr["IS_PerAmt"] = Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue);
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);


                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateRecord()
        {

            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (txtInvNo.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Select Invoice type.";
                bValidateRecord = false;
            }
            if (txtInvDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsEGPSparesInvoice objInv = new clsEGPSparesInvoice();
            try
            {
                bool bEnblSave = false;
                if (bSaveWithConfirm == false && bSaveWithCancel == false)
                {
                    bEnblSave = true;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                if (bValidateRecord() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                sCreateGP = "N";

                if ((Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 ||
                    Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 ||
                    Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) && hdnGPID.Value.Trim() == "0") sCreateGP = "Y";

                if ((Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 1 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 2 ||
                    Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 9) && hdnGPID.Value.Trim() == "0") sCreateGP = "Y";

                UpdateHdrValueFromControl(dtHdr);
                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["Inv_lock"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Inv_canc"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true, true);
                if (bFillDetailsFromInvDescGrid() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

                if (objInv.bSaveRecordWithPart(Location.sDealerCode, dtHdr, dtDetails, dtInvGrpTaxDetails, dtInvTaxDetails, ref iInvID, dtInvJobDescDet) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iInvID);
                    if (bSaveWithConfirm == true)
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                        //return true;
                        if (sCreateGP == "N")
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Invoice") + "','" + Server.HtmlEncode(txtInvNo.Text) + "');</script>");
                            if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                            return true;
                        }
                        else
                        {
                            DataTable dtGPHdr = new DataTable();
                            int iGPHDRID = 0;
                            CreateGatePass(dtGPHdr);
                            clsJobcard ObjJobcard = new clsJobcard();
                            if (ObjJobcard.bSaveGatepass(ref iGPHDRID, dtGPHdr, Location.sDealerCode, 0) == true)
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts Invoice") + "','" + Server.HtmlEncode(txtInvNo.Text) + "');</script>");
                                if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                                return true;
                            }
                            else
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Confirmed Gatepass Not Generated.');</script>");
                                if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                                return true;
                            }
                        }

                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts Invoice") + "','" + Server.HtmlEncode(txtInvNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts Invoice") + "','" + Server.HtmlEncode(txtInvNo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts Invoice") + "','" + Server.HtmlEncode(txtInvNo.Text) + "');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        private void GenerateInvNo(int iInvType)
        {
            //clsDB objDB = new clsDB();
            //try
            //{
            //    DataSet dsDCode = new DataSet();
            //    DealerID = Location.iDealerId;
            //    dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_Spares_Code from M_Dealer where Id=" + DealerID);

            //    if (dsDCode.Tables[0].Rows.Count > 0)
            //    {
            //        DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Spares_Code"].ToString();
            //    }
            //}
            //catch
            //{

            //}
            //finally
            //{
            //    if (objDB != null) objDB = null;
            //}
            objInv = new clsEGPSparesInvoice();
            string sINVSer = "";
            //sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CC" : "CS";
            sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CPC" : (iInvType == 1) ? "CPS" : "STI";

            //txtInvNo.Text = Func.Convert.sConvertToString(objInv.GenerateInv(DealerCode, DealerID, Convert.ToInt16(drpPoType.SelectedValue)));
            //txtInvNo.Text = Func.Common.sGetMaxDocNo(Convert.ToString(Location.iDealerId), "", "INV" + sINVSer, Location.iDealerId);
            //txtInvNo.Text = objInv.GenerateInvNo(DealerCode, iDealerID, "INV" + sINVSer);
            txtInvNo.Text = objInv.GenerateInvNo(sDealerCode, iDealerID, sINVSer);


        }
        private void GetCreatedBy()
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            UsreType = Func.Convert.iConvertToInt(Session["UserType"]);
            clsDB objDB = new clsDB();
            try
            {
                dsCreatedBy = objDB.ExecuteStoredProcedureAndGetDataset("SP_POCreatedByName", iUserId, UsreType);
                //txtCreatedBy.Text = dsCreatedBy.Tables[0].Rows[0]["PO_CreatedBy"].ToString();
            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {

                FillDetailsFromGrid(false, false);
                BindDataToGrid();
                FillDetailsFromGrid(false, false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["PartDetails"] = dtDetails;
                Session["InvTaxDetails"] = dtInvTaxDetails;
                Session["InvGrpTaxDetails"] = dtInvGrpTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg, bool bSaveTmTxDtls)
        {
            try
            {
                string sStatus = "";
                string strPosNo = "";
                dtDetails = (DataTable)Session["PartDetails"];
                bDetailsRecordExist = true;
                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iDtSelPartRow = 0;
                int iCntError = 0;
                string sQtyMsg = "";
                double dBFRGSTSTK = 0;
                double dCurrSTK = 0;
                double dInvQty = 0;
                int iInvType = 0;



                //Fill Details From Grid
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                    TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                    TextBox txtOADetID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOADetID");

                    if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U"))
                    {
                        if (txtStatus.Text == "U")
                        {
                            if (strPosNo == "")
                                strPosNo = iRowCnt.ToString();
                            else
                                strPosNo = strPosNo + "," + iRowCnt.ToString();
                        }
                        iCntForSelect = iCntForSelect + 1;
                        continue;
                    }
                    iDtSelPartRow = 0;
                    for (int iDtRowCnt = 0; iDtRowCnt < dtDetails.Rows.Count; iDtRowCnt++)
                    {
                        //if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text) && Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["OA_Det_ID"]) == Func.Convert.iConvertToInt(txtOADetID.Text))
                        {
                            iDtSelPartRow = iDtRowCnt;
                            break;
                        }
                    }
                    hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + Func.Convert.iConvertToInt(txtPartID.Text);

                    dtDetails.Rows[iRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);
                    dtDetails.Rows[iRowCnt]["OA_Det_ID"] = Func.Convert.iConvertToInt(txtOADetID.Text);



                    //PartNo Or NewPart
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                    dtDetails.Rows[iRowCnt]["part_no"] = txtPartNo.Text;

                    //Part Name
                    TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    dtDetails.Rows[iRowCnt]["Part_Name"] = txtPartName.Text;

                    // Get OA Bal Qty
                    TextBox txtOABal_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOABal_Qty");
                    dtDetails.Rows[iRowCnt]["OABal_Qty"] = Func.Convert.dConvertToDouble(txtOABal_Qty.Text);

                    // Get Qty
                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    dtDetails.Rows[iRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                    //Group code
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                    dtDetails.Rows[iRowCnt]["group_code"] = txtGrNo.Text;

                    // Get Bal Qty
                    TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                    dtDetails.Rows[iRowCnt]["bal_qty"] = Func.Convert.dConvertToDouble(txtBalQty.Text);

                    // Get Price
                    TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                    dtDetails.Rows[iRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);

                    // Get MRPRate
                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    dtDetails.Rows[iRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                    //Discount Per
                    TextBox txtDiscountPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountPer");
                    dtDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtDiscountPer.Text);

                    //Discount rate
                    TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                    dtDetails.Rows[iRowCnt]["disc_rate"] = Func.Convert.dConvertToDouble(txtDiscountRate.Text);

                    //Discount Amt
                    TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                    dtDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDiscountAmt.Text);

                    // Get Total
                    TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                    dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                    //Part Tax main
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                    dtDetails.Rows[iRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                    DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                    Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                    if (DrpPartTax1.Items.Count == 2)
                    {
                        DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                    }

                    DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                    Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                    if (DrpPartTax2.Items.Count == 2)
                    {
                        DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                    }

                    //Get BFR GST Rate Flag
                    TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                    dtDetails.Rows[iRowCnt]["BFRGST"] = txtBFRGST.Text.Trim();

                    // Get BFR GST Stock
                    TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");
                    dtDetails.Rows[iRowCnt]["BFRGST_Stock"] = Func.Convert.dConvertToDouble(txtBFRGST_Stock.Text);

                    //Sujata 26052014_End

                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

                    if (txtPartID.Text != "" && txtPartID.Text != "0" && Chk.Checked == false)
                    {
                        iCntForSelect = iCntForSelect + 1;
                    }
                    if (Chk.Checked == true)
                    {
                        dtDetails.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                    else
                    {
                        dtDetails.Rows[iRowCnt]["Status"] = "N";
                    }
                }
                // } // END Details ta
                //Validation Check Here
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    string sSrNo = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("lblNo") as Label).Text.Trim());
                    string sPartID = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                    string sBFRGST = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtBFRGST") as TextBox).Text.Trim());
                    string siRowCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                    iInvType = Func.Convert.iConvertToInt(DrpInvType.SelectedValue);
                    double dDisper = 0.00;
                    dDisper = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtDiscountPer") as TextBox).Text);


                    if (sPartID != "" && sPartID != "0" && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")
                    {
                        double totPrevQty = 0.00;
                        double totInvQty = 0.00;
                        clsDB objDB = new clsDB();
                        double dGetCurrStk = 0.00;
                        double dGetBFRGSTStk = 0.00;
                        int icount = 0;
                        //dGetCurrStk = objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), sBFRGST);
                        //dGetBFRGSTStk = objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), sBFRGST);

                        //(PartGrid.Rows[iRowCnt].FindControl("txtBalQty") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), sBFRGST));
                        //(PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), sBFRGST));

                        (PartGrid.Rows[iRowCnt].FindControl("txtBalQty") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), "N"));
                        (PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock") as TextBox).Text = Func.Convert.sConvertToString(objDB.ExecuteStoredProcedure_double("SP_GetCurrentStock", iDealerID, Func.Convert.iConvertToInt(sPartID), "Y"));

                        dCurrSTK = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtBalQty") as TextBox).Text);
                        dBFRGSTSTK = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock") as TextBox).Text);
                        dInvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iRowCnt].FindControl("txtQuantity") as TextBox).Text);

                        for (int iCnt = 0; iCnt < PartGrid.Rows.Count; iCnt++)
                        {
                            string sCntPartID = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtPartID") as TextBox).Text.Trim());
                            string sCntStatus = Func.Convert.sConvertToString((PartGrid.Rows[iCnt].FindControl("txtStatus") as TextBox).Text.Trim());
                            if (sPartID != "0" && sPartID != "" && sPartID == sCntPartID && sCntStatus != "D")
                            {
                                double dPreInvQty = 0.00;
                                double dinvQty = 0.00;
                                dinvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("txtQuantity") as TextBox).Text);
                                dPreInvQty = Func.Convert.dConvertToDouble((PartGrid.Rows[iCnt].FindControl("txtPreviousInvQty") as TextBox).Text);
                                totPrevQty = totPrevQty + dPreInvQty;
                                totInvQty = totInvQty + dinvQty;
                                icount = icount + 1;
                            }
                        }

                        if (dInvQty == 0 && siRowCntStatus != "D")
                        { // Check Invoice Quantity
                            iCntError = iCntError + 1;
                            //sQtyMsg = "Please enter Invoice Quantity at Row No " + (iRowCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iRowCnt / 10) + 1);
                            sQtyMsg = "Please enter Invoice Quantity at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (dDisper >= 100 && (iInvType == 1 || iInvType == 2 || iInvType == 9))
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "100% and above discount not allowed at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (Func.Convert.iConvertToInt(drpPartTax.SelectedValue) == 0 && siRowCntStatus != "D")
                        { // Check Part tax
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please select Part Tax at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }
                        if ((totInvQty > (dCurrSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }
                        // New COde on 09/03/2018
                        if (sBFRGST == "N" && (totInvQty > (dCurrSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")// Check Curr Stock
                        {
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }
                        if (sBFRGST == "Y" && (totInvQty > (dBFRGSTSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")
                        {// check OLD Stock
                            iCntError = iCntError + 1;
                            sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + sSrNo;
                            bDetailsRecordExist = false;
                            break;
                        }


                        //Commit on 09032018

                        //if (sBFRGST == "N" && totPrevQty == dCurrSTK && totInvQty > dCurrSTK && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && txtID.Text == "")
                        //{ // Check Curr Stock
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                        //if (sBFRGST == "Y" && totPrevQty == dBFRGSTSTK && totInvQty > dBFRGSTSTK && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && txtID.Text == "")
                        //{ // check OLD Stock
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}

                        //if (sBFRGST == "N" && totPrevQty != dCurrSTK && ((totInvQty) > (dCurrSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")// Check Curr Stock 
                        //{
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                        //if (sBFRGST == "Y" && totPrevQty != dBFRGSTSTK && (totInvQty > (dBFRGSTSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D")
                        //{// check OLD Stock
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                        //Changed Dated 01112017 for both OA SAME Qty and Bal Qty
                        //if (sBFRGST == "N"  && totPrevQty != dCurrSTK && ((totInvQty + totPrevQty) > (dCurrSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && icount > 1)// Check Curr Stock
                        //if (sBFRGST == "N" &&(totInvQty > (dCurrSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && icount > 1)// Check Curr Stock
                        //{
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                        //// new filter added dated 07/03/2018 totPrevQty != totInvQty
                        ////if (sBFRGST == "Y" && totPrevQty != dBFRGSTSTK && ((totInvQty + totPrevQty) > (dBFRGSTSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && icount > 1)
                        //if (sBFRGST == "Y"  && (totInvQty > (dBFRGSTSTK + totPrevQty)) && (iInvType == 1 || iInvType == 2 || iInvType == 9) && siRowCntStatus != "D" && icount > 1)
                        //{// check OLD Stock
                        //    iCntError = iCntError + 1;
                        //    //sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + (i + 1) + " in Page No " + (Func.Convert.iConvertToInt(i / 10) + 1);
                        //    sQtyMsg = "Please enter less  Invoice Quantity from Pre GST Part Stock at Row No " + sSrNo;
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                        //END


                    }//END IF
                }//END For
                //Validation Ends Here


                if (iCntForDelete == iCntForSelect)
                {
                    if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
                    bDetailsRecordExist = false;
                }
                else if (iCntError > 0)
                {
                    if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
                    bDetailsRecordExist = false;
                }

                //For Tax Details
                dtInvGrpTaxDetails = (DataTable)(Session["InvGrpTaxDetails"]);
                dtInvTaxDetails = (DataTable)(Session["InvTaxDetails"]);

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);
                    //Get Net Reverse Amount
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //dtInvGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtInvGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }

                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtInvTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtInvTaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);
                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtInvTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtInvTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtInvTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtInvTaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtInvTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtInvTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    //Get PF IGST or SGST Per  
                    TextBox txtPFTaxPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFTaxPer");
                    dtInvTaxDetails.Rows[iRowCnt]["PF_Tax_Per"] = Func.Convert.dConvertToDouble(txtPFTaxPer.Text);

                    //Get PF IGST or SGST Amount  
                    TextBox txtPFIGSTorSGSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFIGSTorSGSTAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["PF_IGSTorSGST_Amt"] = Func.Convert.dConvertToDouble(txtPFIGSTorSGSTAmt.Text);

                    //Get PF CGST Per
                    TextBox txtPFTaxPer1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFTaxPer1");
                    dtInvTaxDetails.Rows[iRowCnt]["PF_Tax_Per1"] = Func.Convert.dConvertToDouble(txtPFTaxPer1.Text);

                    //Get PF CGST Amount
                    TextBox txtPfCGSTrAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPfCGSTrAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["PF_CGST_Amt"] = Func.Convert.dConvertToDouble(txtPfCGSTrAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtInvTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtInvTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtInvTaxDetails.Rows[iRowCnt]["Inv_tot"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToDetailsTable()
        {
            try
            {
                DataRow dr;
                if (dtDetails.Rows.Count == 0)
                {
                    if (dtDetails.Columns.Count == 0)
                    {
                        dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("OA_Det_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("OANo", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("OABal_Qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("bal_qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));
                    }
                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0")
                    {
                        goto Exit;
                    }
                }
                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["OA_Det_ID"] = 0;
                dr["OANo"] = "";
                dr["Part_ID"] = 0;
                dr["part_no"] = "";
                dr["Part_Name"] = "";
                dr["OABal_Qty"] = 0;
                dr["Qty"] = 0;
                dr["group_code"] = "";
                dr["bal_qty"] = 0;
                dr["MRPRate"] = 0;
                dr["discount_per"] = 0;
                dr["disc_rate"] = 0;
                dr["discount_amt"] = 0;
                dr["Total"] = 0;
                dr["Status"] = "U";
                if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0;

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToTaxGroupDetailsTable()
        {
            try
            {
                string sGrCode = "";
                int iPartTaxID = 0;
                int iPartTaxID1 = 0;
                int iPartTaxID2 = 0;
                Boolean bDtSelPartRow = false;
                dtInvGrpTaxDetails.Clear();
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                    sGrCode = txtGrNo.Text.Trim();

                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    CheckBox ChkForDelete = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0)
                    {
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        iPartTaxID = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(DrpPartTax1.SelectedItem.Text);

                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(DrpPartTax2.SelectedItem.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtInvGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtInvGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;
                    //if (dtOAGrpTaxDetails.Rows.Count == 0)
                    //{
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Gr_Name", typeof(String)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("net_inv_amt", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax_Code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("TAX_Percentage", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax_Tag", typeof(string)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax1_code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax1_Per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax1_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax2_code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax2_Per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax2_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Total", typeof(double)));

                    //}
                    //else if (dtOAGrpTaxDetails.Rows.Count == 1)
                    //{
                    //    if (dtOAGrpTaxDetails.Rows[0]["ID"].ToString() == "0")
                    //    {
                    //        goto Exit;
                    //    }
                    //}
                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtInvGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Parts" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;
                        //dr["net_rev_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iPartTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iPartTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iPartTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtInvGrpTaxDetails.Rows.Add(dr);
                        dtInvGrpTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGrid()
        {
            try
            {
                //If No Data in Grid
                if (Session["PartDetails"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["PartDetails"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["PartDetails"];
                    if (dtDetails.Rows.Count == 0)
                    {
                        CreateNewRowToDetailsTable();
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N" && hdnCancel.Value == "N" && hdnMenuID.Value.ToString() != "687" && (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 1 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 2 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 9))
                            CreateNewRowToDetailsTable();
                    }
                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();

                if (iMenuId == 687)
                {
                    int iRow = dtInvJobDescDet.Rows.Count;
                    for (int iRowCnt = iRow + 1; iRowCnt <= 5; iRowCnt++)
                    {
                        DataRow dr;

                        dr = dtInvJobDescDet.NewRow();
                        dr["ID"] = 0;
                        dr["InvDescID"] = 0;
                        dtInvJobDescDet.Rows.Add(dr);
                        dtInvJobDescDet.AcceptChanges();
                    }
                }

                InvJobDescGrid.DataSource = dtInvJobDescDet;
                InvJobDescGrid.DataBind();
                SetControlPropertyToInvDescGrid();

                GrdPartGroup.DataSource = dtInvGrpTaxDetails;
                GrdPartGroup.DataBind();

                GrdDocTaxDet.DataSource = dtInvTaxDetails;
                GrdDocTaxDet.DataBind();

                SetGridControlProperty();
                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();

                PInvDesc.Style.Add("display", "none");
                //PInvDesc.Style.Add("display", (iMenuId == 687) ? "" : "none");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        #region  InvDescs Function

        // Set Control property To InvDesc Grid    
        private void SetControlPropertyToInvDescGrid()
        {
            try
            {

                string sRecordStatus = "";
                int idtRowCnt = 0;
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sInvDescID = "";
                for (int iRowCnt = 0; iRowCnt < InvJobDescGrid.Rows.Count; iRowCnt++)
                {
                    //Description
                    DropDownList DrpInvJobDesc = (DropDownList)InvJobDescGrid.Rows[iRowCnt].FindControl("DrpInvJobDesc");
                    Func.Common.BindDataToCombo(DrpInvJobDesc, clsCommon.ComboQueryType.InvJobDescription, 0);

                    sRecordStatus = "N";

                    if (idtRowCnt < dtInvJobDescDet.Rows.Count)
                    {

                        //DrpInvJobDesc.Attributes.Add("onChange", "return CheckInvDescSelected(event,this);");

                        DrpInvJobDesc.SelectedValue = Func.Convert.sConvertToString(dtInvJobDescDet.Rows[iRowCnt]["InvDescID"]);

                        DrpInvJobDesc.Attributes.Add("onChange", "return CheckInvDescValueAlreadyUsedInGrid(this);");

                        idtRowCnt = idtRowCnt + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From InvDesc Grid
        private bool bFillDetailsFromInvDescGrid()
        {
            dtInvJobDescDet = (DataTable)Session["InvJobDescDet"];

            int iCntForDelete = 0;
            int iInvDescID = 0;

            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < InvJobDescGrid.Rows.Count; iRowCnt++)
            {
                //InvDescID                
                iInvDescID = Func.Convert.iConvertToInt((InvJobDescGrid.Rows[iRowCnt].FindControl("DrpInvJobDesc") as DropDownList).SelectedValue);

                if (iInvDescID != 0)
                {
                    bValidate = true;

                    dtInvJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                    iCntForDelete++;
                }
                else
                {
                    dtInvJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                }
            }
            bValidate = true;
        //if (iCntForDelete == 0 && iMenuId == 687)
        //{
        //    bValidate = false;
        //}

            //if (bValidate == false)
        //{
        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select At least one Invoice Job Description.');</script>");
        //}
        Last:
            return bValidate;
        }


        #endregion

        private void SetGridControlProperty()
        {
            try
            {
                string sRecordStatus = "";
                string sPartNo = "";
                string sLstTwoDigit = "";
                string sFirstFiveDigit = "";
                double dLabourTotal = 0;

                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                double dDiscPer = 0;
                double dDiscRate = 0;
                double dOABalQty = 0;
                double dPartMrpPrice = 0;

                //double dExclPartDiscRate = 0;
                double dExclPartTotal = 0;
                double dExclTotal = 0;

                //double dPartTax = 0;
                //double dPartTax1 = 0;
                //double dPartTax2 = 0;
                //double dRevMainTaxRate = 0;
                //double dDiscRevRate = 0;

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Location.iDealerId.ToString();
                string sPartID = "", sPartName = "", cPartID = "";
                string sGroupCode = "";
                int iPartID = 0;
                string sPartDetID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        TextBox txtOANo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOANo");
                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        cPartID = txtPartNo1.Text;
                        TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                        TextBox txtOABal_Qty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOABal_Qty");
                        TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                        TextBox txtPreviousInvQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPreviousInvQty");
                        TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();
                        sPartDetID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["ID"]);

                        //GST Relates Work Part Tax
                        PartGrid.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                        // BFR GST Stock 
                        PartGrid.HeaderRow.Cells[28].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[28].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : "");//Hide Cell

                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        //string strTaxTag="";
                        //strTaxTag = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]).Trim();

                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I" || (sGroupCode.Trim() == "L" && hdnIsDocGST.Value == "N"))
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                        //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //else
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");

                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        //Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I" || (sGroupCode.Trim() == "L" && hdnIsDocGST.Value == "N"))
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                        //Additional Tax 1
                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                        DropDownList DrpPartTax1Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1Per");
                        TextBox txtPartTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax1Per");

                        if (DrpPartTax1.Items.Count == 2)
                        {
                            DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax1Per, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " and ID=" + DrpPartTax1.SelectedItem);
                            if (DrpPartTax1Per.Items.Count == 2)
                            {
                                DrpPartTax1Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax1.SelectedItem);
                            }
                            txtPartTax1Per.Text = Func.Convert.sConvertToString(DrpPartTax1Per.SelectedItem);
                        }

                        //Additional Tax 2
                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                        DropDownList DrpPartTax2Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2Per");
                        TextBox txtPartTax2Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax2Per");
                        if (DrpPartTax2.Items.Count == 2)
                        {
                            DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax2Per, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " and ID=" + DrpPartTax2.SelectedItem);
                            if (DrpPartTax2Per.Items.Count == 2)
                            {
                                DrpPartTax2Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax2.SelectedItem);
                            }
                            txtPartTax2Per.Text = Func.Convert.sConvertToString(DrpPartTax2Per.SelectedItem);
                        }

                        TextBox txtDiscountPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountPer");
                        TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                        TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");

                        TextBox txtExclDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExclDiscountRate");
                        TextBox TxtExclTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("TxtExclTotal");

                        TextBox txtlabtag = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtlabtag");
                        TextBox txtfoctag = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtfoctag");
                        TextBox txtwartag = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtwartag");
                        TextBox txtParttypetag = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtParttypetag");
                        TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");

                        TextBox txtFOC = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtFOC");
                        DropDownList DrpWarrType = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpWarrType");

                        DrpWarrType.Items.Add(new ListItem("Paid", "N", true));
                        DrpWarrType.Items.Add(new ListItem("Goodwill", "G", true));
                        DrpWarrType.Items.Add(new ListItem("AMC", "A", true));
                        DrpWarrType.Items.Add(new ListItem("Warranty", "W", true));
                        DrpWarrType.Items.Add(new ListItem("PDI", "P", true));
                        DrpWarrType.Items.Add(new ListItem("Transit", "T", true));
                        DrpWarrType.Items.Add(new ListItem("Enroute Technical", "E", true));
                        DrpWarrType.Items.Add(new ListItem("Enroute Non Technical", "R", true));
                        DrpWarrType.Items.Add(new ListItem("Campaign", "C", true));
                        DrpWarrType.Items.Add(new ListItem("Pre-PDI", "I", true));

                        if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4
                             || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6
                             || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8)
                        {
                            txtQuantity.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtFOC.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        else
                        {
                            txtQuantity.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this);");
                            txtQuantity.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtQuantity.Attributes.Add("onblur", "return CalculateInvPartTotal(event,this);");
                        }
                        //Vikram.. Only Service User Give Discount Per to Labour items on 19102016_Begin
                        //if (sGroupCode != "L" && hdnMenuID.Value == "687")
                        //    txtDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        //19102016_End
                        if (sGroupCode == "02")
                        {
                            drpPartTax.Enabled = false;
                        }
                        else
                        {
                            drpPartTax.Enabled = true;
                        }

                        if ((sGroupCode != "L" && hdnMenuID.Value == "546" && Func.Convert.iConvertToInt(DrpInvType.SelectedValue) != 9) || (sGroupCode == "L" && hdnMenuID.Value == "687"))
                        {
                            txtDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtDiscountPer.Attributes.Add("onblur", "return CalculateInvPartTotal(event,this);");
                        }
                        else
                        {
                            txtDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        if (sGroupCode == "99" && hdnMenuID.Value == "546")
                        {
                            txtMRPRate.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtMRPRate.Attributes.Add("onblur", "return CalculateInvPartTotal(event,this);");
                        }
                        else
                        {
                            txtMRPRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }

                        if (cPartID != "")
                        {
                            dPartQty = 0;
                            dPartRate = 0;
                            dDiscRate = 0;
                            dDiscPer = 0;
                            dOABalQty = 0;
                            dPartMrpPrice = 0;
                            //dDiscRevRate = 0;
                            // For MTIIN or MTIOU Labour take total from Table not Calculate
                            sLstTwoDigit = cPartID.ToString().Substring(Func.Convert.iConvertToInt(cPartID.ToString().Length) - 2, 2);
                            sFirstFiveDigit = cPartID.ToString().Substring(0, 5);

                            dOABalQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["OABal_Qty"]);
                            if (txtID.Text == "" || txtID.Text == "0" || sPartDetID == "" || sPartDetID == "0")
                            {
                                txtPreviousInvQty.Text = "0.00";
                            }
                            else
                            {
                                txtPreviousInvQty.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Qty"]);
                            }


                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            //Reverse Rate Calulation for System Part
                            dPartMrpPrice = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);

                            //dPartTax = Math.Round(dPartMrpPrice * Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / (100 + Func.Convert.dConvertToDouble(txtPartTaxPer.Text)), 2);
                            //if (sGroupCode == "01")
                            // dPartRate = dPartMrpPrice - dPartTax;
                            clsDB objDB = new clsDB();
                            if ((sGroupCode == "01" || sGroupCode == "02") && Func.Convert.iConvertToInt(DrpInvType.SelectedValue) != 9)
                                dPartRate = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", dPartMrpPrice, Func.Convert.iConvertToInt(drpPartTax.SelectedValue));
                            else
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);
                            // Set Reverse Calculated Rate to Rate Column
                            txtMRPRate.Text = Func.Convert.sConvertToString(dPartRate);

                            dDiscPer = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["discount_per"]);
                            dDiscRate = Func.Convert.dConvertToDouble(dPartRate * (dDiscPer / 100));
                            dPartRate = Func.Convert.dConvertToDouble(dPartRate - dDiscRate);

                            txtlabtag.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["lab_tag"]).Trim();
                            txtfoctag.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["foc_tag"]).Trim();
                            txtwartag.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["war_tag"]).Trim();
                            txtParttypetag.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["part_type_tag"]).Trim();
                            DrpWarrType.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["war_tag"]).Trim();
                            txtFOC.Text = (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["foc_tag"]).Trim() == "Y") ? "Yes" : "No";
                            txtBFRGST.Text = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["BFRGST"]).Trim();
                            DrpWarrType.Enabled = false;
                            //Sujata 01092014                        
                            //dPartRate = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) - Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));                            
                            txtDiscountAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dDiscRate, 2)));
                            txtDiscountRate.Text = Func.Convert.sConvertToString(dPartRate);
                            //dLabourTotal = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Total"]);
                            dLabourTotal = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Labour_Total"]);

                            dPartTotal = (txtwartag.Text.ToString().Trim() == "N" && txtfoctag.Text.ToString().Trim() == "N") ? (((sFirstFiveDigit == "MTIMI" || sFirstFiveDigit == "MTIOU" || sFirstFiveDigit == "MTICC") && dDiscPer == 0) ? dLabourTotal : Func.Convert.dConvertToDouble(dPartQty * dPartRate)) : 0;
                            //dPartTotal = (txtwartag.Text.ToString().Trim() == "N" && txtfoctag.Text.ToString().Trim() == "N") ? Func.Convert.dConvertToDouble(dPartQty * dPartRate) : 0;
                            txtPrtTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dPartTotal, 2)));
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dPartTotal);

                            //dPartTax = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100));
                            //dPartTax1 = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax1Per.Text) / 100));
                            //dPartTax2 = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax2Per.Text) / 100));
                            //dPartTax = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartTax) + Func.Convert.dConvertToDouble(dPartTax1) + Func.Convert.dConvertToDouble(dPartTax2));

                            ////dPartRate = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) -Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));

                            //dPartTax = 1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100) + Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax1Per.Text) / 100) + Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTax2Per.Text) / 100));
                            //dExclPartDiscRate = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dPartRate) / Func.Convert.dConvertToDouble(dPartTax));

                            //txtExclDiscountRate.Text = Func.Convert.sConvertToString(Math.Round(dExclPartDiscRate, 2));
                            // Chnaged by VIkram on 05Aug2016 for Cancel reverse calculation tax
                            //dExclPartTotal = Func.Convert.dConvertToDouble(dPartQty * dExclPartDiscRate);
                            // New comment on 19042017
                            //dExclPartTotal = dPartTotal;
                            //TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //dExclTotal = dExclTotal + dExclPartTotal;

                            //New Code Vikram 19042017
                            //if (sGroupCode == "01" || sGroupCode == "02")
                            //{
                            //    dRevMainTaxRate = dPartMrpPrice / (1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));
                            //    dDiscRevRate = Func.Convert.dConvertToDouble(dRevMainTaxRate * dDiscPer / 100);
                            //    txtExclDiscountRate.Text = Func.Convert.sConvertToString(dDiscRevRate);
                            //    dRevMainTaxRate = dRevMainTaxRate - dDiscRevRate;
                            //    dExclPartTotal = Func.Convert.dConvertToDouble(dPartQty * dRevMainTaxRate);
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}
                            //else
                            //{
                            //    dExclPartTotal = dPartTotal;
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}

                            //END
                            dExclPartTotal = dPartTotal;
                            TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            dExclTotal = dExclTotal + dExclPartTotal;
                        }
                    }

                    iPartID = Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Part_ID"]);

                    // Vikram Edit rate Link
                    LinkButton lnkEditRate = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkEditRate");
                    lnkEditRate.Attributes.Add("onclick", "return ShowPartRate(this,'" + sGroupCode + "','" + sDealerId + "'," + iPartID + ");");
                    lnkEditRate.Style.Add("display", "");
                    if (sGroupCode.Trim() == "L") lnkEditRate.Style.Add("display", "none");
                    //Show Editrate when BFRGST=N Dated on 05082017
                    string sBFRGST = Func.Convert.sConvertToString((PartGrid.Rows[iRowCnt].FindControl("txtBFRGST") as TextBox).Text.Trim());
                    if (hdnIsDocGST.Value == "Y" && sBFRGST == "Y") lnkEditRate.Style.Add("display", "none");

                    // New  Part Control
                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowSpWPFPart_New(this,'" + sDealerId + "','" + DrpCustomer.ID + "'," + iHOBrId + ");");
                    //lblSelectPart.Style.Add("display", "none");


                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    //Status
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    lblDelete.Style.Add("display", "none");

                    if (sPartID == "0")
                    {
                        lnkSelectPart.Style.Add("display", "");
                        lnkEditRate.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lblDelete.Style.Add("display", "none");
                    }
                    else
                        lnkSelectPart.Style.Add("display", "none");
                    if (txtGPartName.Text == "")
                    {
                        lnkSelectPart.Style.Add("display", "");
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                    {
                        lnkSelectPart.Style.Add("display", "none");
                        txtPartNo.Style.Add("display", "");

                    }
                    if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4
                             || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6
                             || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8 || hdnMenuID.Value.ToString() == "687")
                    {
                        lnkSelectPart.Style.Add("display", "none");
                    }
                    //If Part Id  is not allocated           
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        lblDelete.Style.Add("display", "");
                    }
                    else if (sRecordStatus == "C")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");
                    }
                    else if (sRecordStatus == "N")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");
                    }
                    //warrenty
                    PartGrid.HeaderRow.Cells[25].Style.Add("display", "none"); // Hide Header 23       
                    PartGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "none");//Hide Cell  
                    //foc
                    PartGrid.HeaderRow.Cells[26].Style.Add("display", "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "none");//Hide Cell  

                    if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4
                        || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6
                        || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8 || hdnMenuID.Value == "687")
                    {//warrenty
                        PartGrid.HeaderRow.Cells[25].Style.Add("display", ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "");//Hide Cell  
                        //foc
                        PartGrid.HeaderRow.Cells[26].Style.Add("display", ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "");//Hide Cell  

                        PartGrid.HeaderRow.Cells[19].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[19].Style.Add("display", "none");//Hide Cell       

                        PartGrid.HeaderRow.Cells[7].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//Hide Cell

                        PartGrid.HeaderRow.Cells[3].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[3].Style.Add("display", "none");//Hide Cell
                    }
                    if (hdnConfirm.Value == "Y" || hdnMenuID.Value == "687" || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 9)//|| hdnIsDocGST.Value == "Y"
                    {// If Record Is COnfirm Then Hide Edit Rate COlumn and Service User Not Show Edit Rate Column
                        PartGrid.HeaderRow.Cells[18].Style.Add("display", "none"); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[18].Style.Add("display", "none");//Hide Cell 
                    }
                    //For MD User
                    if (txtUserType.Text == "6")
                    {
                        lnkSelectPart.Style.Add("display", "none");
                        lnkEditRate.Style.Add("display", "none");
                    }
                }
                txtTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Vikram Begin 05062017_BEgin Dislay hide and label change code related to GST Appl and non appl.
                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    //Hide Group Disc per
                    GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[4].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //hide Group Disc Amt
                    GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    // END

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    if (sDiscChange == true)
                    {
                        txtGrDiscountPer.Text = Func.Convert.sConvertToString(0.00);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString(0.00);
                    }
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if ((srowGRPID != "L" && hdnMenuID.Value == "546" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per") ||
                            (srowGRPID == "L" && hdnMenuID.Value == "687" && Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per"))
                        {
                            txtGrDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountPer.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");
                            txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        else
                        {
                            txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountAmt.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");
                        }
                    }

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' ");

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");


                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnIsDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' ");

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtInvGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
                    txtTax2Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer2.SelectedValue) == 0 ? "0" : drpTaxPer2.SelectedItem.Text);

                    drpTax.Enabled = false;
                    drpTax1.Enabled = false;
                    drpTax2.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlPropertyTaxCalculation()
        {
            try
            {
                double dGrpTotal = 0;
                double dGrpDiscPer = 0;
                double dGrpDiscAmt = 0;
                double dGrpTaxAppAmt = 0;

                double dGrpMTaxPer = 0;
                double dGrpMTaxAmt = 0;

                double dGrpTax1Per = 0;
                double dGrpTax1Amt = 0;

                double dGrpTax2Per = 0;
                double dGrpTax2Amt = 0;

                double dDocTotalAmtFrPFOther = 0;
                double dDocDiscAmt = 0;
                double dDocLSTAmt = 0;
                double dDocCSTAmt = 0;
                double dDocTax1Amt = 0;
                double dDocTax2Amt = 0;
                string sGrpMTaxTag = "";

                double TotalOA = 0;
                double TotalRev = 0;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";
                double dDocPFTaxPer = 0;
                double dDocPFTaxPer1 = 0;


                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //txtGrnetrevamt.Text = "0";
                }

                for (int i = 0; i < PartGrid.Rows.Count; i++)
                {
                    TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                    TextBox TxtExclTotal = (TextBox)PartGrid.Rows[i].FindControl("TxtExclTotal");
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");

                    TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[i].FindControl("txtPartTaxPer");
                    TextBox txtPartTax1Per = (TextBox)PartGrid.Rows[i].FindControl("txtPartTax1Per");
                    CheckBox Chk = (CheckBox)PartGrid.Rows[i].FindControl("ChkForDelete");


                    if (txtGrNo.Text.Trim() != "" && Chk.Checked == false)
                    {
                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                        //TotalRev = Math.Round(TotalRev + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                        if (Func.Convert.dConvertToDouble(txtPartTaxPer.Text) > dDocPFTaxPer)
                            dDocPFTaxPer = Func.Convert.dConvertToDouble(txtPartTaxPer.Text);
                        if (Func.Convert.dConvertToDouble(txtPartTax1Per.Text) > dDocPFTaxPer1)
                            dDocPFTaxPer1 = Func.Convert.dConvertToDouble(txtPartTax1Per.Text);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0 && Chk.Checked == false)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                            //txtGrnetrevamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetrevamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
                        }
                    }
                }
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                    {
                        dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                        dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));
                    }
                    else
                    {
                        dGrpDiscPer = 0.00;
                        dGrpDiscAmt = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);
                    }

                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Sujata 23092014 Begin
                    //dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    //double sum = 0;
                    if (sTax1ApplOn == "1")
                    {
                        //Vikram Begin_02052017
                        //double DiscNetAmt = 0;
                        //DiscNetAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt)), 2);
                        //DiscNetAmt = DiscNetAmt + dGrpMTaxAmt;
                        //sum = DiscNetAmt / (1 + (Func.Convert.dConvertToDouble(dGrpTax1Per / 100)));
                        //dGrpTax1Amt = Math.Round(sum * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                        //END

                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }

                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

                    //Sujata 23092014 Begin
                    //dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    //Sujata 23092014 End

                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 0);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    //New Code
                    //dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    //Hide Group Disc Amt
                    GrdDocTaxDet.HeaderRow.Cells[3].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[3].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //PF Per and PF Amount Display dated 15092017 Vikram
                    GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", "");//Hide Cell
                    GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", "");//Hide Cell
                    //PF Tax per 
                    GrdDocTaxDet.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Freight IGST Tax% " : "Freight SGST Tax%"; //IGST or SGST per
                    GrdDocTaxDet.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : "");//Hide Cell
                    //New PF IGST or SGST Amount
                    GrdDocTaxDet.HeaderRow.Cells[14].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Freight IGST Amt " : "Freight SGST Amt ";
                    // PF Tax per 1
                    GrdDocTaxDet.HeaderRow.Cells[15].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[15].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : "");//Hide Cell
                    //New PF CGST Amount
                    GrdDocTaxDet.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : "");//Hide Cell

                    //GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    //GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    //GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[11].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[11].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    GrdDocTaxDet.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell


                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END

                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocRevTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtPFTaxPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFTaxPer");
                    TextBox txtPFIGSTorSGSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFIGSTorSGSTAmt");
                    TextBox txtPFTaxPer1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFTaxPer1");
                    TextBox txtPfCGSTrAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPfCGSTrAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    //txtDocRevTotal.Text = Func.Convert.sConvertToString(TotalRev.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    txtPFTaxPer.Text = Func.Convert.sConvertToString(dDocPFTaxPer.ToString("0.00"));
                    txtPFTaxPer1.Text = Func.Convert.sConvertToString(dDocPFTaxPer1.ToString("0.00"));

                    double dDocPFPer = 0;
                    double dDocPFAmt = 0;
                    double dDocOtherPer = 0;
                    double dDocOtherAmt = 0;
                    double dDocGrandAmt = 0;

                    double dDocPFTaxAmt1 = 0;
                    double dDocPFTaxAmt2 = 0;

                    if (sDiscChange == true)
                    {
                        txtPFPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtPFAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                        txtOtherPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtOtherAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                    }
                    if (hdnIsDocGST.Value == "N")// if (hdnIsDocGST.Value == "Y") // comment on 19092017 Dated for PF Charge
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFPer.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");

                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                        }
                        else
                        {
                            txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFAmt.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");

                            //dDocPFPer = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_per"]), 2);
                            //dDocPFAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_amt"]), 2);
                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(txtPFAmt.Text);
                        }
                    }
                    txtPFPer.Text = Func.Convert.sConvertToString(dDocPFPer.ToString("0.00"));
                    txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));

                    if (hdnCustTaxTag.Value == "I" && hdnIsDocGST.Value == "Y")
                    {
                        dDocPFTaxAmt1 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                        dDocPFTaxAmt2 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                    }
                    else
                    {
                        dDocPFTaxAmt1 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                    }
                    txtPFIGSTorSGSTAmt.Text = Func.Convert.sConvertToString(dDocPFTaxAmt1.ToString("0.00"));//Set IGST or SGST Amount
                    txtPfCGSTrAmt.Text = Func.Convert.sConvertToString(dDocPFTaxAmt2.ToString("0.00"));//Set CGST Amount

                    if (hdnIsDocGST.Value == "Y")
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt) + Func.Convert.dConvertToDouble(dDocPFTaxAmt1) + Func.Convert.dConvertToDouble(dDocPFTaxAmt2)), 2);
                    }
                    else
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    }

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherPer.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100)), 2);
                        }
                        else
                        {
                            txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherAmt.Attributes.Add("onblur", "return CalulateInvPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            //dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["other_money"]), 2);
                            dDocOtherAmt = Func.Convert.dConvertToDouble(txtOtherAmt.Text);
                        }
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void lblCancel_Click(object sender, EventArgs e)
        {
            FillDetailsFromGrid(false, false);
            BindDataToGrid();
        }
        protected void PartGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Label RFPtatus = (Label)PartGrid.Rows[0].FindControl("lblRFPStatus");
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    FillDetailsFromGrid(false, false);
                    CreateNewRowToDetailsTable();
                    Session["PartDetails"] = dtDetails;
                    Session["InvTaxDetails"] = dtInvTaxDetails;
                    Session["InvGrpTaxDetails"] = dtInvGrpTaxDetails;

                    BindDataToGrid();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void PartGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                dtDetails = (DataTable)Session["PartDetails"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void GrdPartGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void drpPartTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDetailsFromGrid(false, false);
            CreateNewRowToTaxGroupDetailsTable();
            Session["PartDetails"] = dtDetails;
            Session["InvTaxDetails"] = dtInvTaxDetails;
            Session["InvGrpTaxDetails"] = dtInvGrpTaxDetails;
            BindDataToGrid();
        }
        protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dschg = new DataSet();

            DealerID = Location.iDealerId;
            int iTMInvType = Func.Convert.iConvertToInt(DrpInvType.SelectedValue);
            objInv = new clsEGPSparesInvoice();
            dschg = objInv.GetInv(iInvID, "New", DealerID, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue), iMenuId);

            if (dschg != null) // if no Data Exist
            {

                if (dschg.Tables.Count > 0)
                {
                    if (dschg.Tables[0].Rows.Count == 1)
                    {
                        dschg.Tables[0].Rows[0]["Inv_canc"] = "N";
                        dschg.Tables[0].Rows[0]["Inv_lock"] = "N";
                        dschg.Tables[0].Rows[0]["Inv_com"] = "N";
                        sNew = "Y";
                        //hdnCustTaxTag.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["CustTaxTag"]);
                        //hdnIsDocGST.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["DocGST"]);

                        DisplayData(dschg);
                    }
                }
            }
            txtID.Text = "";
            txtInvDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            objInv = null;
            dschg = null;
            DrpInvType.SelectedValue = Func.Convert.sConvertToString(iTMInvType);
            if (Location.iDealerId != 0)
            {
                GenerateInvNo(Func.Convert.iConvertToInt(DrpInvType.SelectedValue));
            }
            lblTtlPartDetails.Text = (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? "Jobcard Details" : "Part Details";
            lblSelectModel.Style.Add("display", (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? "" : "none");

            int iInvType = 0;
            iInvType = Func.Convert.iConvertToInt(iTMInvType);
            //string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CC" : "CS";
            string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CPC" : (iInvType == 1) ? "CPS" : "STI";

            hdnTrNo.Value = iDealerID + "/" + iUser_ID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + sINVSer;
        }
        protected void lblSelectModel_Click(object sender, EventArgs e)
        {
            try
            {
                FillJobcardInvDetails(Func.Convert.iConvertToInt(hdnJobHDRID.Value));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillJobcardInvDetails(int iJob_HDR_ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                PSelectionGrid.Style.Add("display", "none");
                int iTMInvType = Func.Convert.iConvertToInt(DrpInvType.SelectedValue);
                hdnInvTypeID.Value = Func.Convert.sConvertToString(DrpInvType.SelectedValue);
                DataSet ds = new DataSet();
                // int iDealerId = 3;
                DealerID = Location.iDealerId;
                
                //DrpCustomer.SelectedValue = "0";
                DrpCustomer.SelectedValue = Func.Convert.sConvertToString(hdnJbInvCustID.Value);
                txtRefernce.Text = "";

                objInv = new clsEGPSparesInvoice();
                string sType = "";
                sType = (iTMInvType == 3 || iTMInvType == 4) ? "Jobcard" : (((iTMInvType == 5 || iTMInvType == 6)) ? "JobcardP" : "JobcardL");
                //ds = objInv.GetInv(iJob_HDR_ID, "Jobcard", DealerID, 0, iMenuId);
                //ds = objInv.GetInv(iJob_HDR_ID, sType, DealerID, 0, iMenuId);
                ds = objInv.GetInv(iJob_HDR_ID, sType, DealerID, Func.Convert.iConvertToInt(hdnJbInvCustID.Value), iMenuId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Inv_canc"] = "N";
                            ds.Tables[0].Rows[0]["Inv_lock"] = "N";
                            ds.Tables[0].Rows[0]["Inv_com"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtInvDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                objInv = null;
                ds = null;
                DrpInvType.SelectedValue = Func.Convert.sConvertToString(iTMInvType);
                if (Location.iDealerId != 0)
                {
                    GenerateInvNo(Func.Convert.iConvertToInt(DrpInvType.SelectedValue));
                }

                int iInvType = 0;
                iInvType = Func.Convert.iConvertToInt(iTMInvType);
                string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CPC" : (iInvType == 1) ? "CPS" : "STI";
                hdnTrNo.Value = iDealerID + "/" + iUser_ID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + sINVSer;
                //string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CC" : "CS";
                //hdnTrNo.Value = iDealerID + "/" + iUser_ID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "INV" + sINVSer;

                DrpCustomer.Enabled = false;
                DrpInvType.Enabled = false;
                txtRefernce.Enabled = false;
                for (int iRowCnt1 = 0; iRowCnt1 < PartGrid.Rows.Count; iRowCnt1++)
                {
                    PartGrid.HeaderRow.Cells[23].Style.Add("display", ""); // Hide Header        
                    PartGrid.Rows[iRowCnt1].Cells[23].Style.Add("display", "");//Hide Cell  

                    PartGrid.HeaderRow.Cells[24].Style.Add("display", ""); // Hide Header        
                    PartGrid.Rows[iRowCnt1].Cells[24].Style.Add("display", "");//Hide Cell  

                    PartGrid.HeaderRow.Cells[19].Style.Add("display", "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt1].Cells[19].Style.Add("display", "none");//Hide Cell       

                    PartGrid.HeaderRow.Cells[7].Style.Add("display", "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt1].Cells[7].Style.Add("display", "none");//Hide Cell

                    PartGrid.HeaderRow.Cells[3].Style.Add("display", "none"); // Hide Header        
                    PartGrid.Rows[iRowCnt1].Cells[3].Style.Add("display", "none");//Hide Cell

                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt1].FindControl("txtQuantity");
                    txtQuantity.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                }
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                return;
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
        protected void DrpInvType_SelectedIndexChanged(object sender, EventArgs e)
        {

            //For Stock Transfer Challan Type
            if (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 9)
                Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.CustomerBranchGSTNO), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') != ISNULL(CM.GST_No,'') and CM.Active='Y'");
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.CustomerBranchGSTNO, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And Cust_Type = 6 and ISNULL(M_Dealer.GST_No,'') != ISNULL(CM.GST_No,'')");
            else
                Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, ((iMenuId == 687) ? "" : " And CM.Cust_Type <> 18") + " and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and CM.Active='Y'");
            //Func.Common.BindDataToCombo(DrpCustomer, ((iMenuId == 687) ? clsCommon.ComboQueryType.CustomerIncMTI : clsCommon.ComboQueryType.Customer), 0, " And ( DealerID='" + Location.iDealerId + "') And Cust_Type != 6");
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "') And Cust_Type != 6");                                
            DrpCustomer.SelectedValue = "0";
            if (Location.iDealerId != 0)
            {
                GenerateInvNo(Func.Convert.iConvertToInt(DrpInvType.SelectedValue));
            }
            hdnInvTypeID.Value = Func.Convert.sConvertToString((DrpInvType.SelectedValue));
            lblSelectModel.Style.Add("display", (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? "" : "none");
            lblTtlPartDetails.Text = (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? "Jobcard Details" : "Part Details";
            DrpCustomer.Enabled = (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? false : true;
            txtRefernce.Enabled = (Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 3 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 4 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 5 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 6 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 7 || Func.Convert.iConvertToInt(DrpInvType.SelectedValue) == 8) ? false : true;
            int iInvType = 0;
            iInvType = Func.Convert.iConvertToInt(hdnInvTypeID.Value);
            //string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CC" : "CS";
            string sINVSer = (iInvType == 8) ? "JLC" : (iInvType == 7) ? "JLS" : (iInvType == 6) ? "JPC" : (iInvType == 5) ? "JPS" : (iInvType == 4) ? "JC" : (iInvType == 3) ? "JS" : (iInvType == 2) ? "CPC" : (iInvType == 1) ? "CPS" : "STI";

            //hdnTrNo.Value = iDealerID + "/" + iUser_ID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "INV" + sINVSer;
            hdnTrNo.Value = iDealerID + "/" + iUser_ID + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + sINVSer;
        }

        private void CreateGatePass(DataTable dtGPHdr)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                DataRow dr;

                dtGPHdr.Columns.Add(new DataColumn("GPHDRID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("Gp_No", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Gp_date", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefJobcardID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefJbInvID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("RefSlInvID", typeof(int)));
                dtGPHdr.Columns.Add(new DataColumn("GPtype", typeof(string)));
                dtGPHdr.Columns.Add(new DataColumn("Narr", typeof(string)));

                int iInvType = 0;
                iInvType = Func.Convert.iConvertToInt(DrpInvType.SelectedValue);
                string sGPSeries = (iInvType == 8 || iInvType == 7 || iInvType == 6 || iInvType == 5 || iInvType == 4 || iInvType == 3) ? "GPI" : "GPC";

                dr = dtGPHdr.NewRow();
                dr["GPHDRID"] = 0;
                //dr["Gp_No"] = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GPI", Location.iDealerId);
                dr["Gp_No"] = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", sGPSeries, Location.iDealerId);
                dr["Gp_date"] = Func.Common.sGetCurrentDateTime(1, true);
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(Location.iDealerId);
                dr["DlrBranchID"] = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                dr["RefJobcardID"] = Func.Convert.iConvertToInt(hdnJobHDRID.Value);
                dr["RefJbInvID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["RefSlInvID"] = 0;
                dr["GPtype"] = (iInvType == 8 || iInvType == 7 || iInvType == 6 || iInvType == 5 || iInvType == 4 || iInvType == 3) ? "I" : "C";
                dr["Narr"] = "";

                dtGPHdr.Rows.Add(dr);
                dtGPHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {

            }

        }

        private int i = 0;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                int no = e.Row.RowIndex;
                if (txtPartID != "0")
                {
                    if ((Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 1 || Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 2 || Func.Convert.iConvertToInt(hdnInvTypeID.Value) == 9)
                        && hdnConfirm.Value != "Y" && hdnCancel.Value != "Y")
                    {
                        lblNo.Text = i.ToString();
                        i++;
                    }
                    else
                        lblNo.Text = (no + 1).ToString();
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }

        protected void rbtLstDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnConfirm.Value == "N")
                {
                    sDiscChange = true;
                    FillDetailsFromGrid(false, false);
                    bFillDetailsFromInvDescGrid();
                    BindDataToGrid();
                    sDiscChange = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void BtnOpen_Click(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                if (iInvID == 0) iInvID = Func.Convert.iConvertToInt(txtID.Text);
                if (iInvID > 0)
                {
                    string status;
                    status = "";
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_InvOpen", iInvID);
                    if (ds != null) // if no Data Exist
                    {
                        status = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JStatus"]);
                        if (status.Trim() != "") Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + status + "');</script>");
                    }
                }
                objDB = null;
                ds = null;
                objDB = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
    }

}