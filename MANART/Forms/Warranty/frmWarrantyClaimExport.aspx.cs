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
using MANART_BAL;
using MANART_DAL;
using System.IO;

namespace MANART.Forms.Warranty
{
    public partial class frmWarrantyClaimExport : System.Web.UI.Page   
    {
        #region Variables

        private DataTable dtComplaint = new DataTable();
        private DataTable dtInvestigations = new DataTable();
        private DataTable dtPart = new DataTable();
        private DataTable dtLabour = new DataTable();
        private DataTable dtLubricant = new DataTable();
        private DataTable dtSublet = new DataTable();
        private DataTable dtJob = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        private DataTable dtJbGrpTaxDetails = new DataTable();
        private DataTable dtWarrJobDescDet = new DataTable();

        private DataTable Acc_dtGrpTaxDetails = new DataTable();        
        
        private int iClaimID = 0;
        private clsWarranty.enmClaimType ValenmFormUsedFor;
        string sDealerId = "";
        int iUserId = 0;
        int iMenuId = 0;
        string sClaimType = "";
        //Sujata 03022011
        string sClaimTypeIdChassis = "";
        //Sujata 03022011
        #endregion

        #region Form Function

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                // Set Basic form Value 
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                Location.bUseSpareDealerCode = true;
                if (txtUserType.Text == "6")
                {
                    Location.SetControlValue();
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                ToolbarC.iValidationIdForSave = 44;
                ToolbarC.iValidationIdForConfirm = 44;
                ToolbarC.iFormIdToOpenForm = 44;
                // Sujata 28122010
                //ToolbarC.iFormIdToOpenForm = 40;
                // Sujata 28122010
                ToolbarC.bUseImgOrButton = true;
                txtClaimDate.Mandatory = true;
                FillCombo();
                if (!IsPostBack)
                {
                    ClearAllSession();
                    Location.SetControlValue();
                    txtClaimDate.Enabled = false;
                }

                sDealerId = Location.iDealerId.ToString();
                SetDocumentDetails();

                FillSelectionGrid();

                //iClaimID = 4;
                if (iClaimID != 0)
                {
                    GetDataAndDisplay(iClaimID);

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        // FillCombo
        private void FillCombo()
        {
            string sUserType = Func.Convert.sConvertToString(Session["UserType"]);
            txtUserType.Text = sUserType;
            //Func.Common.BindDataToCombo(drpClaimType, clsCommon.ComboQueryType.ExportClaimType, 0);
            Func.Common.BindDataToCombo(drpRouteType, clsCommon.ComboQueryType.RouteType, 0);
            Func.Common.BindDataToCombo(DropClaimTypes, clsCommon.ComboQueryType.ExportClaimType, 0, " and Claim_Type='E' ");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (OptShareType.SelectedValue == "2")
                {
                    txtVECVShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtDealerShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtCustomerShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                }
                else
                {
                    txtVECVShare.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    txtDealerShare.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    txtCustomerShare.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    txtVECVShare.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                    txtDealerShare.Attributes.Add("onblur", "return CheckTotalOfPercentageForClaimParts(event,this)");
                    txtCustomerShare.Attributes.Add("onblur", "return CheckTotalOfPercentageForClaim(this)");
                }
                // To Hide the control which used but not display
                txtDealerCode.Style.Add("display", "none");
                PFileAttchDetails.Visible = true;
                sDealerId = Location.iDealerId.ToString();
                txtDealerCode.Text = Location.sDealerCode;
                //drpClaimType.Enabled = true;
                CalculateComplaintGridCnt();
                CalculateInvestigationGridCnt();
                CalculatePartGridCnt();
                CalculateLabourGridCnt();
                CalculateLubricantGridCnt();
                CalculateSubletGridCnt();
                CalculateJobGridCnt();
                //string strReportpath;
                //strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                //lblVehicleHistory.Attributes.Add("onClick", "return ShowChassisWDtls('" + txtchassisID.Text + "','" + strReportpath + "');"); 
                //Sujata 20012011
                //lblVehicleHistory.Style.Add("display", "none");  
                string strReportpath;
                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                lblVehicleHistory.Attributes.Add("onClick", "return ShowChassisWDtls('" + txtchassisID.Text + "','" + strReportpath + "');");
                //Sujata 20012011
                if (txtUserType.Text == "6")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                    btnReturnedClaim.Visible = false;
                    btnReSubmitRequest.Visible = false;
                    btnCreateWarrantyClaim.Visible = false;
                    lblSelectModel.Visible = false;
                    
                }
                trNewAttachment.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
                trNewAttachment1.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false); 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                FillSelectionGrid();
            }
            
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
            if (txtChassisNo.Text != "")
            {
                CPEVehicleDetails.Collapsed = false;
                CPEVehicleDetails.ExpandDirection = CollapsiblePanelExpandDirection.Vertical;
            }
        }

        // set Document text 
        private void SetDocumentDetails()
        {
            try
            {
                //iClaimID =Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                //iClaimID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                txtRequestNo.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtRequestDate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                txtPartAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtLabourAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtLubricantAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtSubletAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtClaimAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                txtAccPartAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtAccLabourAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtAccLubricantAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtAccSubletAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtAccClaimAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                //OptShareType.Items[1].Selected = true;


                ///lblHighValueMsg.Visible = false;


                btnCreateWarrantyClaim.Visible = false;
                btnReSubmitClaim.Visible = false;
                btnReSubmitRequest.Visible = false;
                btnReturnedClaim.Visible = false;

                if (txtUserType.Text == "1")
                    lblDealerRemark.Text = "ASM Remark:";
                else
                    lblDealerRemark.Text = "Dealer Remark:";

                if (iMenuId == 796 || iMenuId == 612)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmNormal;
                    drpClaimType.Style.Add("display", "");
                    txtClaimType.Style.Add("display", "none");

                    ///lblHighValueMsg.Visible = true;

                    lblHighValueMsg.Text = "(  Normal Claim Amount: <  ";
                    //if (Location.iCountryId != 13)
                    //    lblHighValueMsg.Text = lblHighValueMsg.Text + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, "")).ToString("#0.00") + ")";
                    //else
                    //    lblHighValueMsg.Text = lblHighValueMsg.Text + "> " + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForNepal, 0, "")).ToString("#0.00") + ")";
                    if (Location.iCountryId != 13 && Func.Convert.iConvertToInt(Session["UserType"]) == 4) //USD used in case of export warranty and for domestic login icountryId set for stateid
                        lblHighValueMsg.Text = lblHighValueMsg.Text + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForAllExceptNepal, 0, "")).ToString("#0.00") + " in USD)";
                    else
                        lblHighValueMsg.Text = lblHighValueMsg.Text + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, "")).ToString("#0.00") + " in INR)";


                }
                else if (iMenuId == 242)
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;

                    //drpClaimType.Style.Add("display", "none");
                    DropClaimTypes.Visible = false;
                    txtClaimType.Style.Add("display", "");


                }
                else if (iMenuId == 415)
                {
                    //DropClaimTypes.Visible = false;
                    //txtClaimType.Style.Add("display", "");
                    drpClaimType.Style.Add("display", "");
                    txtClaimType.Style.Add("display", "none");
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmHighValueClaim;

                    ///lblHighValueMsg.Visible = true;

                    //if (Location.iCountryId != 13)
                    //    lblHighValueMsg.Text = lblHighValueMsg.Text +  Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, "")).ToString("#0.00") + ")";
                    //else
                    //    lblHighValueMsg.Text = lblHighValueMsg.Text + lblHighValueMsg.Text + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForNepal, 0, "")).ToString("#0.00") + ")";


                    if (Location.iCountryId != 13 && Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                        lblHighValueMsg.Text = lblHighValueMsg.Text + ">  $" + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForAllExceptNepal, 0, "")).ToString("#0.00") + ")";
                    else
                        lblHighValueMsg.Text = lblHighValueMsg.Text + "> " + lblHighValueMsg.Text + Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, "")).ToString("#0.00") + ")";


                }
                //System.Threading.Thread.Sleep(2000);
                //Common
                //drpClaimType.Attributes.Add("onChange", "OnClaimTypeChange(this)");
                txtClaimRevNo.Style.Add("display", "none");
                if (txtClaimRevNo.Text == "")
                {
                    txtClaimRevNo.Text = "1";
                }
                // fields for Claim
                //Sujata 03022011
                //lblSelectModel.Attributes.Add("onclick", " return ShowModelMaster(this," + Location.iDealerId + ")");
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    sClaimTypeIdChassis = "16";
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                    sClaimTypeIdChassis = "15";
                else
                    sClaimTypeIdChassis = DropClaimTypes.SelectedValue;

                //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + sClaimTypeIdChassis + "')");
                //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + Func.Convert.sConvertToString(Session["HOBR_ID"]) + "','" + sClaimTypeIdChassis + "')");
                //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + Func.Convert.sConvertToString(Session["HOBR_ID"]) + "','" + sClaimTypeIdChassis + "','" + ((iMenuId == 415) ? "Request" :"Claim") + "')");
                //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + Func.Convert.sConvertToString(Session["HOBR_ID"]) + "','" + ((iMenuId == 415) ? "Request" : "Claim") + "')");
                lblSelectModel.Attributes.Add("onclick", " return ShowExportChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Location.iDealerId.ToString() + "','" + Session["DepartmentID"].ToString() + "')");

                //Sujata 03022011
                lblSelectRequest.Attributes.Add("onclick", " return ShowGoodwillRequest(this," + Location.iDealerId + ")"); //add SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                lblSelectRefClaim.Attributes.Add("onclick", " return ShowGoodwillRequest(this," + Location.iDealerId + ")");// add SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                txtModelID.Style.Add("display", "none");
                txtModelGroupID.Style.Add("display", "none");

                // fields for reclaim
                lblSelectRefClaim.Style.Add("display", "none");

                lblRefClaimNo.Style.Add("display", "none");
                lblRefClaimDate.Style.Add("display", "none");

                txtRefClaimNo.Style.Add("display", "none");
                txtRefClaimDate.Style.Add("display", "none");

                txtRefClaimID.Style.Add("display", "none");

                // Fields for Request
                lblSelectRequest.Style.Add("display", "none");

                lblRequestNo.Style.Add("display", "none");
                txtRequestNo.Style.Add("display", "none");
                lblRequestDate.Style.Add("display", "none");
                txtRequestDate.Style.Add("display", "none");
                txtRequestID.Style.Add("display", "none");

                ////txtRepairOrderDate.sOnBlurScript = " return CheckRepairOrderDateWithClaimDate();";
                ////txtRepairCompleteDate.sOnBlurScript = " return CheckRepairCompleteDate();";
                ////txtFailureDate.sOnBlurScript = " return CheckFailureDateWithRepairOrderDate();";

                //BindAllGridData();
                SetFormControlAsPerClaimType();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void SetFormControlAsPerClaimType()
        {
            try
            {
                //clsWarranty ObjWarranty = new clsWarranty();
                //ValenmFormUsedFor = ObjWarranty.GetEnmClaimType(drpClaimType.SelectedValue);
                //ObjWarranty = null;
                //onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtModelCode.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtModelDescription.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtInstallationDate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtChassisNo.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtEngineNo.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                //txtInvoiceNo.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtInvoiceDate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                // txtCustomerName.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                //txtCustomerAddress.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                txtGVW.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                //CPEVehicleDetails.Collapsed = true;

                RequestDetails.Style.Add("display", "none");
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                {
                    lblTitle.Text = " Warranty Claim";
                    txtClaimType.Text = "Normal Claim";
                    //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "EC" + DrpInvType.SelectedValue.ToString(), Location.iDealerId);
                    //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", DrpInvType.SelectedValue.ToString() + "C", Location.iDealerId);
                    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", ((sClaimTypeIdChassis == "14") ? "WP" : (sClaimTypeIdChassis == "2") ? "GT" : (sClaimTypeIdChassis == "8" || sClaimTypeIdChassis == "16") ? "GC" : (sClaimTypeIdChassis == "6") ? "CP" : (sClaimTypeIdChassis == "10") ? "RM" : "WC") + DrpInvType.SelectedValue.ToString(), Location.iDealerId);                    
                    txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);

                    // To Set Working to get  Model details            
                    txtModelID.Style.Add("display", "none");
                    txtModelGroupID.Style.Add("display", "none");
                    //lblVehicleNo.Style.Add("display","");


                    //lblVECVShare.Style.Add("display", "none");
                    //lblDealerShare.Style.Add("display", "none");
                    //lblCustomerShare.Style.Add("display", "none");

                    //txtVECVShare.Style.Add("display", "none");
                    //txtDealerShare.Style.Add("display", "none");
                    //txtCustomerShare.Style.Add("display", "none");
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    lblTitle.Text = " Goodwill Claim";
                    txtClaimType.Text = "Goodwill Claim";
                    lblClaimNo.Text = "Claim No.";
                    lblClaimDate.Text = "Claim Date";

                    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GR", Location.iDealerId);
                    txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    // To Set Working to get  Model details           
                    //drpClaimType.Style.Add("display", "none");  
                    //Temporary commentted by shyamal as on 12112013
                    if (txtModelCode.Text != "")
                    {
                        RequestDetails.Style.Add("display", "none");
                    }
                    //lblVehicleNo.Style.Add("display", "none");
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                {
                    lblTitle.Text = " High Value Request";
                    txtClaimType.Text = "HighValue Request";
                    lblClaimNo.Text = "Request No.";
                    lblClaimDate.Text = "Request Date";

                    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "HR", Location.iDealerId);
                    txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    // To Set Working to get  Model details           
                    //drpClaimType.Style.Add("display", "none");  
                    //lblVehicleNo.Style.Add("display", "none");
                }
                btnJobSave.Visible = false;
                btnJobConfirm.Visible = false;
                lblSelectModel.Style.Add("display", "");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // bin data to all grid
        private void BindAllGridData()
        {
            // fields For Return         
            BindDataToComplaintGrid();
            BindDataToInvestigationsGrid();
            BindDataToPartGrid(true, 0);
            BindDataToLaborGrid(true, 0);
            BindDataToLubricantGrid(true, 0);
            BindDataToSubletGrid(true, 0);
            BindDataToJobGrid();
        }



        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton1")//for News
                {
                    PSelectionGrid.Style.Add("display", "none");
                    DrpInvType.Enabled = true;
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                    {
                        double dHighvalue = 0.00;
                        if ((Location.iCountryId == 13 && Func.Convert.iConvertToInt(Session["UserType"]) == 4) || Func.Convert.iConvertToInt(Session["UserType"]) == 3) //For Export 
                            dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, ""));
                        else
                            dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForAllExceptNepal, 0, ""));

                        //if (Location.iCountryId != 13  && Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Normal claim value is upto 1 to " + dHighvalue.ToString() + " USD,if claim value exceeds, request to be made prior to claim');</script>");
                        //else
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Normal claim value is upto 1 to " + dHighvalue.ToString() + " INR,if claim value exceeds, request to be made prior to claim');</script>");
                    }
                    ClearFormControl(); 
                    lnkSrvVAN.Visible = false;
                    lblSelectModel.Visible = true;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    //Sujata 19012011
                    txtChkfun.Text = "false";
                    //Sujata 19012011
                    if (bSaveRecord(false) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                    txtRefClaimID.Text = "";
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    //Sujata 19012011
                    txtChkfun.Text = "true";
                    //Sujata 19012011
                    if (bSaveRecord(true) == false) return;
                    hdnReSubmitRequest.Value = "N";
                    hdnReturnedClaim.Value = "N";
                    hdnReSubmitClaim.Value = "N";
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (txtID.Text == "")
                    {
                        PSelectionGrid.Style.Add("display", "");
                        return;
                    }
                    CancelRecord();
                    PSelectionGrid.Style.Add("display", "");
                }

                txtID.Text = Func.Convert.sConvertToString(iClaimID);
                FillSelectionGrid();
                GetDataAndDisplay(iClaimID);

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //To Cancel The Record
        private void CancelRecord()
        {

        }

        private bool bValidateRecord()
        {
            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (txtClaimDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the Document date.";
                bValidateRecord = false;
            }
            //if (drpRouteType.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Route Type.";
            //    bValidateRecord = false;
            //}


            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            {
                if (OptShareType.Items[0].Selected == false && OptShareType.Items[1].Selected == false)
                {
                    return false;
                }

            }
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            {
                double dHighvalue = 0.00;
                if ((Location.iCountryId == 13 && Func.Convert.iConvertToInt(Session["UserType"]) == 4) || Func.Convert.iConvertToInt(Session["UserType"]) == 3)
                    dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, ""));
                else
                    dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForAllExceptNepal, 0, ""));
                if (Func.Convert.dConvertToDouble(txtClaimAmt.Text) < dHighvalue)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Total Amount Can Not Be Less Than High Value Amount " + dHighvalue.ToString() + ".');</script>");
                    return false;
                }
            }
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            {
                double dHighvalue = 0.00;
                if ((Location.iCountryId == 13 && Func.Convert.iConvertToInt(Session["UserType"]) == 4) || Func.Convert.iConvertToInt(Session["UserType"]) == 3)
                    dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, ""));
                else
                    dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmtForAllExceptNepal, 0, ""));

                //if (hdnReturnedCnt.Value == "0" && hdnRejectCnt.Value == "0" && DropClaimTypes.SelectedValue != "18" && DropClaimTypes.SelectedValue != "2" && DropClaimTypes.SelectedValue != "8")
                //    if (Func.Convert.dConvertToDouble(txtClaimAmt.Text) > dHighvalue)
                //    {
                //        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Amount Exceeds to " + dHighvalue.ToString() + "  will come under High value Request.');</script>");
                //        return false;
                //    }
            }
            //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            //{
            //double dHighvalue = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetHighValueAmt, 0, ""));
            //if (DateTime.Parse(txtFailureDate.Text) < DateTime.Parse(ViewState["ClaimDate"].ToString()))
            //    {
            //        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Failure Date Should be Less Than Claim Date.');</script>");
            //        return false;
            //    }
            //}
            string[] strFailureDate = txtFailureDate.Text.Split('/');
            string[] strInstallationDate = txtInstallationDate.Text.Split('/');
            if ((DropClaimTypes.SelectedValue != "21" && DropClaimTypes.SelectedValue != "3" && DropClaimTypes.SelectedValue != "18" && DropClaimTypes.SelectedValue != "4" && DropClaimTypes.SelectedValue != "13" && DropClaimTypes.SelectedValue != "9" ) && (DropClaimTypes.SelectedValue == "1" &&  txtAggreagateNo.Text.Trim() =="" ))
            {
                DateTime dtFailureDate = new DateTime(Func.Convert.iConvertToInt(strFailureDate[2]), Func.Convert.iConvertToInt(strFailureDate[1]), Func.Convert.iConvertToInt(strFailureDate[0]));
                DateTime dtInstallationDate = new DateTime(Func.Convert.iConvertToInt(strInstallationDate[2]), Func.Convert.iConvertToInt(strInstallationDate[1]), Func.Convert.iConvertToInt(strInstallationDate[0]));
                if (dtFailureDate < dtInstallationDate)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Failure Date Should be Greater or Equal to Installation Date.');</script>");
                    return false;
                }
            }
            //if (txtInstallationDate.Text != "" && txtInstallationDate.Enabled == true)
            //{
            //    clsWarranty ObjWarranty = null;
            //    try
            //    {
            //        ObjWarranty = new clsWarranty();
            //        int flage = ObjWarranty.ChkChassisWarrantyByInsDate(txtChassisNo.Text.Trim(), txtInstallationDate.Text, 0);
            //        if (flage == 0)
            //        {
            //            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Selected Installation Date leads to claim out of warranty, Please select valid Installation Date.');</script>");
            //            txtInstallationDate.Focus();
            //            return false;
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
            //    finally
            //    {
            //        if (ObjWarranty != null) ObjWarranty = null;
            //    }
            //}

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }

        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Dealer_Id", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("InvType", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_type_Id", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Claim_Rev_No", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Model_Id", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Chassis_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Engine_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Vehicle_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Customer_name", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Customer_Address", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Customer_MobNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Customer_Email", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Jobcard_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Jobcard_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("INS_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Odometer", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Hrs_reading", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("VECV_Inv_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("VECV_Inv_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Approval_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Approval_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Repair_Order_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Repair_Order_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Repair_Complete_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Failure_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Root_Type_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Part_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Labor_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Lubricant_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Sublet_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Claim_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Share_Type", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GCR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("GCR_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GCR_Date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("VECV_Share", typeof(float)));
                dtHdr.Columns.Add(new DataColumn("Dealer_Share", typeof(float)));
                dtHdr.Columns.Add(new DataColumn("Cust_Share", typeof(float)));

                dtHdr.Columns.Add(new DataColumn("Ref_Claim_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Ref_Claim_date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("GVW", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_Remark", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Sub_Dealer_Name", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ClaimDomesticOrExport", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Confirm_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Claim_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_ReSubmitReq", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_ResubmitClaim", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_ReturnedClaim", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("RejectedCnt", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("ReturnedCnt", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Jobcard_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AggregateNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();

                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Dealer_ID"] = Location.iDealerId;
                dr["Claim_no"] = txtClaimNo.Text;
                dr["Claim_date"] = txtClaimDate.Text;
                dr["InvType"] = DrpInvType.SelectedValue.ToString();
                
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    dr["Claim_type_Id"] = 16;
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                {
                    dr["Claim_type_Id"] = 15;
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                {
                    dr["Claim_type_Id"] = DropClaimTypes.SelectedValue;
                }
                dr["Claim_type_Id"] = DropClaimTypes.SelectedValue;
                dr["Claim_Rev_No"] = Func.Convert.iConvertToInt(txtClaimRevNo.Text);
                dr["Model_Id"] = Func.Convert.iConvertToInt(txtModelID.Text);
                dr["Chassis_no"] = txtChassisNo.Text;
                dr["Engine_no"] = txtEngineNo.Text;
                dr["Vehicle_No"] = txtVehicleNo.Text;
                dr["Customer_name"] = txtCustomerName.Text;
                dr["Customer_Address"] = txtCustomerAddress.Text;

                dr["Customer_MobNo"] = txtCustMobNo.Text;
                dr["Customer_Email"] = txtCustEmail.Text;

                dr["Jobcard_no"] = "";
                dr["Jobcard_date"] = "";
                dr["INS_date"] = txtInstallationDate.Text;
                dr["Odometer"] = Func.Convert.iConvertToInt(txtOdometer.Text);
                dr["Hrs_reading"] = Func.Convert.iConvertToInt(txtHrsReading.Text);
                dr["VECV_Inv_no"] = txtInvoiceNo.Text;
                if (txtInvoiceDate.Text == "")
                {
                    dr["VECV_Inv_date"] = null;
                }
                else
                {
                    dr["VECV_Inv_date"] = txtInvoiceDate.Text;
                }
                dr["Root_Type_ID"] = Func.Convert.iConvertToInt(drpRouteType.SelectedValue);
                dr["Approval_No"] = "";
                dr["Approval_Date"] = null;
                dr["Repair_Order_No"] = txtRepairOrderNo.Text;
                dr["Repair_Order_Date"] = txtRepairOrderDate.Text;
                dr["Repair_Complete_Date"] = txtRepairCompleteDate.Text;
                dr["Failure_Date"] = txtFailureDate.Text;
                dr["Part_Amt"] = Func.Convert.dConvertToDouble(txtPartAmount.Text);
                dr["Labor_Amt"] = Func.Convert.dConvertToDouble(txtLabourAmount.Text);
                dr["Lubricant_Amt"] = Func.Convert.dConvertToDouble(txtLubricantAmount.Text);
                dr["Sublet_Amt"] = Func.Convert.dConvertToDouble(txtSubletAmount.Text);
                dr["Claim_Amt"] = Func.Convert.dConvertToDouble(txtClaimAmt.Text);
                dr["GCR_ID"] = Func.Convert.iConvertToInt(txtRequestID.Text);
                dr["GCR_No"] = txtRequestNo.Text;
                dr["GCR_Date"] = txtRequestDate.Text;

                if (OptShareType.Items[0].Selected == true)
                {
                    dr["Share_Type"] = "C";
                }
                else if (OptShareType.Items[1].Selected == true)
                {
                    dr["Share_Type"] = "I";
                }                
                dr["VECV_Share"] = Func.Convert.dConvertToDouble(txtVECVShare.Text);
                dr["Dealer_Share"] = Func.Convert.dConvertToDouble(txtDealerShare.Text);
                dr["Cust_Share"] = Func.Convert.dConvertToDouble(txtCustomerShare.Text);

                dr["Ref_Claim_no"] = txtRefClaimNo.Text;
                if (txtRefClaimDate.Text == "")
                {
                    dr["Ref_Claim_date"] = null;
                }
                else
                {
                    dr["Ref_Claim_date"] = txtRefClaimDate.Text;
                }

                dr["GVW"] = txtGVW.Text;
                dr["Dealer_Remark"] = txtDealerRemark.Text;
                dr["Sub_Dealer_Name"] = txtSubDealerName.Text;
                dr["ClaimDomesticOrExport"] = (Func.Convert.iConvertToInt(Session["UserType"]) == 4) ? "E" : "D";
                dr["Claim_Confirm"] = "";
                dr["Confirm_Date"] = "";
                dr["Claim_Cancel"] = "";

                dr["Is_ReSubmitReq"] = hdnReSubmitRequest.Value;
                dr["Is_ResubmitClaim"] = hdnReSubmitClaim.Value;
                dr["Is_ReturnedClaim"] = hdnReturnedClaim.Value;
                dr["RejectedCnt"] = Func.Convert.iConvertToInt(hdnRejectCnt.Value);
                dr["ReturnedCnt"] = Func.Convert.iConvertToInt(hdnReturnedCnt.Value);
                dr["Jobcard_HDR_ID"] = Func.Convert.iConvertToInt(hdnJobcardID.Value);
                dr["DocGST"] = Func.Convert.sConvertToString(hdnISDocGST.Value);
                dr["AggregateNo"] = Func.Convert.sConvertToString(txtAggreagateNo.Text);
                dr["CustID"] = Func.Convert.iConvertToInt(hdnCustID.Value);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        //ToSave Record
        private bool bSaveRecord(bool bWithConfirm)
        {
            bool bSaveRecord = false;

            if (bValidateRecord() == false)
            {
                return bSaveRecord;
            }

            DataTable dtHdr = new DataTable();
            clsWarranty ObjWarranty = new clsWarranty();

            UpdateHdrValueFromControl(dtHdr);

            if (bFillDetailsFromInvDescGrid() == false) return bSaveRecord;           

            //Get Complaint Details     
            if (bFillDetailsFromComplaintGrid() == false) return bSaveRecord;

            //Get Investigations Details     
            if (bFillDetailsFromInvestigationsGrid() == false) return bSaveRecord;

            //User Should enter foloowing Condition way
            //1) Either Part Details Or Labour Details
            //2) Both Part Details and Labour Details
            //3) Either Sublet Allowed
            //4) Only Lubricant is Not Allwoed

            //Get Lubricant Details     
            if (bFillDetailsFromLubricantGrid() == false) return bSaveRecord;

            //sujata 19012011
            // check Wheather Warranty claim have at lease on part or oil or labour or sublet
            if (txtChkfun.Text == "true")
            {
                if (bChkDetailsFromPartLabourSubletGrid() == false) return bSaveRecord;
                //sujata 04022011
                //if (bChkDetailsFromServiceHistory() == false) return bSaveRecord; 

                //if (DropClaimTypes.SelectedValue != "18" && bChkDetailsFromServiceHistory() == false) return bSaveRecord;
                //sujata 04022011            
            }
            //sujata 19012011

            //Get Part Details     
            if (bFillDetailsFromPartGrid() == false)
            {
                //Get Labour Details     
                if (bFillDetailsFromLabourGrid() == false)
                {
                    //Get Sublet Details     
                    if (bFillDetailsFromSubletGrid() == false)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter The Details Of Part/Labour/Sublet.');</script>");
                        return bSaveRecord;
                    }
                }
                else
                {
                    //Get Sublet Details     
                    if (bFillDetailsFromSubletGrid() == false)
                    {

                    }
                }
            }
            else
            {
                //Get Labour Details     
                if (bFillDetailsFromLabourGrid() == false)
                {

                }
                //Get Sublet Details     
                if (bFillDetailsFromSubletGrid() == false)
                {

                }
            }

            //Get Job Details     
            //if (bFillDetailsFromJobGrid() == false) return bSaveRecord;

            //Get File Attach
            if (bSaveAttachedDocuments() == false) return bSaveRecord;
            bFillDetailsFromTaxGrid();

            //If record is confirm
            if (bWithConfirm == true)
            {
                dtHdr.Rows[0]["Claim_Confirm"] = "Y";
                dtHdr.Rows[0]["Confirm_Date"] = Func.Common.sGetCurrentDate();
            }
            iClaimID = 0;
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            {
                bSaveRecord = ObjWarranty.bSaveWarrantyClaimRecord(Location.sDealerCode, dtHdr, dtPart, dtLabour, dtLubricant, dtSublet, dtComplaint, dtInvestigations, dtJob, dtFileAttach, dtJbGrpTaxDetails, iUserId, "C", ref iClaimID, dtWarrJobDescDet);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            {
                bSaveRecord = ObjWarranty.bSaveWarrantyClaimRecord(Location.sDealerCode, dtHdr, dtPart, dtLabour, dtLubricant, dtSublet, dtComplaint, dtInvestigations, dtJob, dtFileAttach, dtJbGrpTaxDetails, iUserId, "G", ref iClaimID, dtWarrJobDescDet);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
            {
                bSaveRecord = ObjWarranty.bSaveWarrantyClaimRecord(Location.sDealerCode, dtHdr, dtPart, dtLabour, dtLubricant, dtSublet, dtComplaint, dtInvestigations, dtJob, dtFileAttach, dtJbGrpTaxDetails, iUserId, "G", ref iClaimID, dtWarrJobDescDet);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            {
                bSaveRecord = ObjWarranty.bSaveGCRRecord(Location.sDealerCode, dtHdr, dtPart, dtLabour, dtLubricant, dtSublet, dtComplaint, dtInvestigations, dtJob, dtFileAttach, dtJbGrpTaxDetails, iUserId, "W", ref iClaimID);
            }
            //Sujata 10042015_Begin
            //bSaveRecord = ObjWarranty.bSaveWarrantyClaimRecord(Location.sDealerCode, dtHdr, dtPart, dtLabour, dtLubricant, dtSublet, dtComplaint, dtInvestigations, dtJob, dtFileAttach, iUserId, "C", ref iClaimID);
            //Sujata 10042015_End
            if (bSaveRecord == true)
            {
                if (bWithConfirm == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Warranty Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Warranty Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                }
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Warranty Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
            }
            return bSaveRecord;
        }

        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.sModelPart = "M";
                SearchGrid.sModelPart = (Func.Convert.iConvertToInt(Session["UserType"]) == 4) ? "E" : "D"; 
                //SearchGrid.iDealerID = Location.iDealerId;               
                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                }

                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)//as per discussion with deepti madam here added two column Claim Type,Claim Amt
                {
                    SearchGrid.AddToSearchCombo("Claim No");
                    SearchGrid.AddToSearchCombo("Claim Date");
                    SearchGrid.AddToSearchCombo("Claim Type");
                    SearchGrid.AddToSearchCombo("Claim Amt");
                    SearchGrid.AddToSearchCombo("Claim Status");
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                    SearchGrid.sSqlFor = "WarrantyClaim";
                    SearchGrid.sGridPanelTitle = "Claim List";

                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    SearchGrid.AddToSearchCombo("Request No");
                    SearchGrid.AddToSearchCombo("Request Date");
                    // SearchGrid.AddToSearchCombo("Request Type");
                    // SearchGrid.AddToSearchCombo("Request Amt");
                    SearchGrid.AddToSearchCombo("Request Status");
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.sSqlFor = "GoodwillRequest";
                    SearchGrid.sGridPanelTitle = "Goodwill Claim List";
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                {
                    SearchGrid.AddToSearchCombo("Request No");
                    SearchGrid.AddToSearchCombo("Request Date");
                    // SearchGrid.AddToSearchCombo("Request Type");
                    //SearchGrid.AddToSearchCombo("Request Amt");
                    SearchGrid.AddToSearchCombo("Request Status");
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.sSqlFor = "HighValueRequest";
                    SearchGrid.sGridPanelTitle = "Request List";
                }//Sujata 10042015
                else
                {
                    SearchGrid.AddToSearchCombo("Claim No");
                    SearchGrid.AddToSearchCombo("Claim Date");
                    SearchGrid.AddToSearchCombo("Claim Type");
                    SearchGrid.AddToSearchCombo("Claim Amt");
                    SearchGrid.AddToSearchCombo("Claim Status");
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.sSqlFor = "WarrantyClaim";
                    SearchGrid.sGridPanelTitle = "Claim List";
                }//Sujata 10042015
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            hdnReSubmitClaim.Value = "N";
            hdnReSubmitRequest.Value = "N";
            hdnReturnedClaim.Value = "N";

            iClaimID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay(iClaimID);
        }

        private void GetDataAndDisplay(int iID)
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                DataSet ds = new DataSet();
                ClearAllSession();
                if (iID != 0)
                {
                    DrpInvType.Enabled = false;


                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                    {
                        if (hdnReturnedClaim.Value == "P")
                        {
                            ds = ObjWarranty.GetWarrantyClaim(iID, "Returned", Location.iDealerId, 6);
                            DisplayData(ds);
                        }
                        else if (hdnReSubmitClaim.Value == "P")
                        {
                            ds = ObjWarranty.GetWarrantyClaim(iID, "Rejected", Location.iDealerId, 6);
                            DisplayData(ds);
                        }
                        else
                        {
                            ds = ObjWarranty.GetWarrantyClaim(iID, "Regular", Location.iDealerId, 6);
                            DisplayData(ds);
                        }
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "Regular", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "ProcessGoodwill", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "Goodwill", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                    else
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "Regular", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                }
                else
                {
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "MaxClaim", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "MaxRequest", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                    {
                        ds = ObjWarranty.GetWarrantyClaim(iID, "MaxHighValue", Location.iDealerId, 6);
                        DisplayData(ds);
                    }
                }
                ObjWarranty = null;
                ds = null;
                ObjWarranty = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                bool bRecordIsOpen = true;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header        
                txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                iClaimID = Func.Convert.iConvertToInt(txtID.Text);
                Location.iDealerId = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Dealer_ID"]);
                txtClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_no"]);
                txtClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]);
                hdnMinClaimDate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]);

                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);

                //ViewState["ClaimDate"] = txtClaimDate.Text;
                sClaimType = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                txtRequestID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_ID"]);

                //drpClaimType.Enabled = false;  
                //lblServiceHistroy.Style.Add("display", "");                
                DropClaimTypes.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                DrpInvType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvType"]);
                if (sClaimType == "15" || sClaimType == "1" || sClaimType == "16")
                {
                    //drpClaimType.SelectedValue = "2";
                    if (iMenuId == 796 || iMenuId == 612)
                    {
                        ValenmFormUsedFor = clsWarranty.enmClaimType.enmNormal;
                        // To Set Working to get  Model details                                    
                        txtClaimType.Text = "Normal Claim";
                        DropClaimTypes.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                        //sClaimType = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                        RequestDetails.Style.Add("display", "none");
                        lblClaimType.Text = "Claim Type";
                        //lblVehicleHistory.Visible = true;
                        lblVehicleHistory.Visible = false;
                    }
                    else if (iMenuId == 415)
                    {
                        ValenmFormUsedFor = clsWarranty.enmClaimType.enmHighValueClaim;
                        // To Set Working to get  Model details                                    
                        txtClaimType.Text = "High Value Request";
                        RequestDetails.Style.Add("display", "none");
                        txtRequestID.Text = txtID.Text;
                        DropClaimTypes.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                        lblClaimType.Text = "Request Type";
                        lblVehicleHistory.Visible = false;
                    }
                    txtInstallationDate.Enabled = true;
                    txtInvoiceDate.Enabled = true;
                }
                else if (sClaimType == "16")
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;
                    //drpClaimType.SelectedValue = "1";

                    //Temporary commentted by shyamal as on 12112013
                    RequestDetails.Style.Add("display", "none");
                    txtRequestID.Text = txtID.Text;
                    if (sClaimType == "2")
                    {
                        txtClaimType.Text = "Technical Goodwill Request";
                    }
                    else if (sClaimType == "16")
                    {
                        txtClaimType.Text = "Goodwill Claim";
                    }
                    else
                    {
                        txtClaimType.Text = "Commercial Goodwill Request";
                    }
                    txtInstallationDate.Enabled = true;
                    txtInvoiceDate.Enabled = true;
                    lblClaimType.Text = "Request Type";
                    lblVehicleHistory.Visible = false;
                }
                else if (sClaimType == "18")
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmNormal;
                    // To Set Working to get  Model details                                    
                    txtClaimType.Text = "Normal Claim";
                    DropClaimTypes.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                    //sClaimType = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                    RequestDetails.Style.Add("display", "none");
                    txtRequestID.Text = txtID.Text;
                    txtInstallationDate.Enabled = false;
                    lblServiceHistroy.Style.Add("display", "none");
                    lblClaimType.Text = "Claim Type";
                    lblVehicleHistory.Visible = false;
                }

                txtClaimRevNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Rev_No"]);
                txtModelID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Id"]);
                txtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_code"]);
                txtModelDescription.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Name"]);
                txtModelGroupID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["model_gr_id"]);

                hdnCustTaxTag.Value = (hdnReturnedClaim.Value == "P" || hdnReSubmitClaim.Value == "P") ? Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RCustTaxTag"]) : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);

                //lblSelectModel.Style.Add("display", "none");
                lblSelectModel.Visible = (txtID.Text == "" || txtID.Text == "0") ? true : false;

                txtAggreagateNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AggregateNo"]);
                lblChassisNo.Text = (txtAggreagateNo.Text == "") ? "Chassis No.:" : "Aggregate No.:";

                lblEngineNo.Visible = (txtAggreagateNo.Text == "") ? true : false;
                txtEngineNo.Visible = (txtAggreagateNo.Text == "") ? true : false;
                Label4.Visible = (txtAggreagateNo.Text == "") ? true : false;
                txtChassisNo.Visible = (txtAggreagateNo.Text == "") ? true : false;
                txtAggreagateNo.Visible = (txtAggreagateNo.Text == "") ? false : true;

                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
                txtchassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChassisHdrID"]);
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vehicle_No"]);
                hdnCustID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);

                txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);
                txtCustomerAddress.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Address"]);

                txtCustMobNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_MobNo"]);
                txtCustEmail.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Email"]);

                //=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_no"];
                //=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_date"];
                txtInstallationDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["INS_date"]);
                txtOdometer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Odometer"]);
                txtHrsReading.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Hrs_reading"]);
                txtHrsApplicable.Text = "N";
                //Sujata 22012011
                //txtOdometer.ReadOnly = false;
                //txtOdometer.ReadOnly = false;
                Label14.Visible = false;
                Label15.Visible = false;
                //Sujata 22012011
                if (txtOdometer.Text == "0")
                {
                    txtHrsApplicable.Text = "Y";
                }
                if (txtHrsApplicable.Text == "Y")
                {
                    txtHrsReading.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this,'5');");
                    //txtOdometer.ReadOnly = true;
                    //Sujata 22012011
                    Label15.Visible = true;
                    //Sujata 22012011
                }
                else if (txtHrsApplicable.Text == "N")
                {
                    //txtHrsReading.ReadOnly = true;
                    //Sujata 22012011
                    Label14.Visible = true;
                    //Sujata 22012011
                }

                txtInvoiceNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VECV_Inv_no"]);
                txtInvoiceDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VECV_Inv_date"]);
                txtApprovalNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_No"]);

                txtApprovalDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Date"]);

                hdnJobcardID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_HDR_ID"]);

                txtRepairOrderNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Order_No"]);
                txtRepairOrderDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Order_Date"]);
                txtRepairCompleteDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Complete_Date"]);
                txtFailureDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Failure_Date"]);
                drpRouteType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Root_Type_ID"]);
                txtPartAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Part_Amt"]).ToString("0.00");
                txtLabourAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Labor_Amt"]).ToString("0.00");
                txtLubricantAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Lubricant_Amt"]).ToString("0.00");
                txtSubletAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Sublet_Amt"]).ToString("0.00");
                txtClaimAmt.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Claim_Amt"]).ToString("0.00");

                txtAccPartAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Accepted_Part_Amt"]).ToString("0.00");
                txtAccLabourAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Accepted_Labour_Amt"]).ToString("0.00");
                txtAccLubricantAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Accepted_Lubricant_Amt"]).ToString("0.00");
                txtAccSubletAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Accepted_Sublet_Amt"]).ToString("0.00");
                txtAccClaimAmt.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Accepted_Claim_Amt"]).ToString("0.00");

                txtRequestNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]);// goodwill Request No 
                txtRequestDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_Date"]);// goodwill Request Date

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Share_Type"]) == "C")
                {
                    OptShareType.Items[0].Selected = true;
                }
                else if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Share_Type"]) == "I")
                {
                    OptShareType.Items[1].Selected = true;
                }

                txtVECVShare.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["VECV_Share"]).ToString("0.00");
                txtDealerShare.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Dealer_Share"]).ToString("0.00");
                txtCustomerShare.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Cust_Share"]).ToString("0.00");


                txtRefClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_no"]);
                txtRefClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_date"]);

                txtGVW.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GVW"]);
                txtDealerRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Remark"]);
                txtSubDealerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sub_Dealer_Name"]);

                hdnReturnedClaim.Value = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ReturnedClaim"]) == "N") ? hdnReturnedClaim.Value : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ReturnedClaim"]);
                hdnReSubmitClaim.Value = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ResubmitClaim"]) == "N") ? hdnReSubmitClaim.Value : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ResubmitClaim"]);
                hdnReSubmitRequest.Value = (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ReSubmitReq"]) == "N") ? hdnReSubmitRequest.Value : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_ReSubmitReq"]);
                hdnRejectCnt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RejectedCnt"]);
                hdnReturnedCnt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReturnedCnt"]);
                hdnIsSHQResource.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]);
                hdnIsSHQ.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]);
                hdnISDocGST.Value = (hdnReturnedClaim.Value == "P" || hdnReSubmitClaim.Value == "P") ? Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RDocGST"]) : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);

                HdnSubletLabID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletLabID"]);
                HdnSubletLabCode.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletLabCode"]);
                HdnSubletLabRate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletLabRate"]);

                //Sujata 21012011
                string strReportpath;
                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                lblVehicleHistory.Attributes.Add("onClick", "return ShowChassisWDtls('" + txtchassisID.Text + "','" + strReportpath + "');");
                //Sujata 21012011
                if (iMenuId == 796)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) == "Y")
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CC','WC');");
                    else
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CP','WC');");
                }
                else if (iMenuId == 242)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) == "Y")
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CC','GR');");
                    else
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CP','GR');");

                }
                else if (iMenuId == 415)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]) == "Y" || Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) == "Y")
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CC','HR');");
                    else
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','CP','HR');");

                }


                //Sujata 20012011
                //if (txtchassisID.Text!="") lblVehicleHistory.Style.Add("display", "");
                //lblVehicleHistory.Style.Add("display", "");
                //Sujata 20012011

                // If Record is Confirm or cancel then it is not editable            
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                hdnAccDetUpdate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AccDetUpdate"]).Trim();
                BtnSaveAccGrpDtl.Visible = (hdnAccDetUpdate.Value.Trim() == "N") ? false : true;
                btnJobSave.Visible = true;
                btnJobConfirm.Visible = true;
                hdnClaimConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]);
                hdnCSMSubmit.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]);

                if (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") btnJobSave.Visible = false;
                if (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") btnJobConfirm.Visible = false;

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    bRecordIsOpen = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint);

                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_No"]).Trim() == "") ? false : true);                   
                    BtnSaveAccGrpDtl.Enabled = (hdnAccDetUpdate.Value.Trim() == "P") ? true : false;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, (hdnAccDetUpdate.Value.Trim() == "Y") ? true : false);
                }
                if (hdnReSubmitClaim.Value == "P" || hdnReSubmitRequest.Value == "P" || hdnReturnedClaim.Value == "P")
                {
                    bEnableControls = false;
                    bRecordIsOpen = true;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) == "Y")
                {
                    bEnableControls = false;
                    bRecordIsOpen = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                }
                string sClaimStatus = "";
                sClaimStatus = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Status"]);
                btnCreateWarrantyClaim.Visible = false;
                btnReSubmitClaim.Visible = false;
                btnReSubmitRequest.Visible = false;
                btnReturnedClaim.Visible = false;
                if (sClaimStatus == "1")//Pending
                {
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                    {

                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill)
                    {

                    }
                }
                else if (sClaimStatus == "2" && ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)//Approved 
                {
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
                    {
                        // No Change
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                    {

                        //User can Create Claim

                        btnCreateWarrantyClaim.Visible = true;
                    }
                }
                else if (sClaimStatus == "3" || hdnIsSHQResource.Value == "J")//Rejected No Change 
                {
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmHighValueClaim && Func.Convert.iConvertToInt(hdnRejectCnt.Value) <= 1 && hdnReSubmitClaim.Value != "Y")
                    {
                        btnReSubmitClaim.Visible = true;
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim && Func.Convert.iConvertToInt(hdnRejectCnt.Value) <= 1 && hdnReSubmitRequest.Value != "Y")
                    {
                        //User can Create Claim
                        btnReSubmitRequest.Visible = true;
                    }
                }
                else if (sClaimStatus == "4" || hdnIsSHQResource.Value == "R" || hdnIsSHQ.Value == "R")//Returned Allow To Edit
                {
                    //3 Items Alloed to change

                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmHighValueClaim && Func.Convert.iConvertToInt(hdnReturnedCnt.Value) <= 3 && hdnReturnedClaim.Value != "Y")
                    {
                        //User can Create Claim
                        btnReturnedClaim.Visible = true;
                    }
                    //if (txtClaimRevNo.Text == "3")
                    //{
                    //    //No Change
                    //}
                    //else
                    //{
                    //    if (txtClaimRevNo.Text == "2")
                    //    {
                    //        //mess For Last Attemt to Return
                    //        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('This is the Last Attemt To Change Claim.');</script>");
                    //    }
                    //    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    //    bEnableControls = true;
                    //    bRecordIsOpen = true;
                    //}
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_CreatedYN"]) == "Y")
                {
                    btnCreateWarrantyClaim.Visible = false;
                    //Sujata 12012011
                    //PFileAttchDetails.Visible = false;
                    PFileAttchDetails.Visible = true;
                    //Sujata 12012011
                }
                else
                {
                    PFileAttchDetails.Visible = true;
                }
                // Display Complaints Details     
                dtComplaint = ds.Tables[1];
                Session["ComplaintsDetails"] = dtComplaint;
                lblComplaintsRecCnt.Text = Func.Common.sRowCntOfTable(dtComplaint);
                BindDataToComplaintGrid();

                // Display Investigations Details
                dtInvestigations = ds.Tables[2];
                Session["InvestigationDetails"] = dtInvestigations;
                lblInvestigationsRecCnt.Text = Func.Common.sRowCntOfTable(dtInvestigations);
                BindDataToInvestigationsGrid();

                //Display Part Details   
                Session["PartDetails"] = null;
                dtPart = ds.Tables[3];
                Session["PartDetails"] = dtPart;
                lblPartRecCnt.Text = Func.Common.sRowCntOfTable(dtPart);
                BindDataToPartGrid(bRecordIsOpen, 0);


                // Display Labour Details                        
                Session["LabourDetails"] = null;
                dtLabour = ds.Tables[4];
                Session["LabourDetails"] = dtLabour;
                lblLabourRecCnt.Text = Func.Common.sRowCntOfTable(dtLabour);
                BindDataToLaborGrid(bRecordIsOpen, 0);

                // Display Lubricant Details      
                Session["LubricantDetails"] = null;
                dtLubricant = ds.Tables[5];
                Session["LubricantDetails"] = dtLubricant;
                lblLubricantRecCnt.Text = Func.Common.sRowCntOfTable(dtLubricant);
                BindDataToLubricantGrid(bRecordIsOpen, 0);

                // Display Sublet  Details                        
                Session["SubletDetails"] = null;
                dtSublet = ds.Tables[6];
                Session["SubletDetails"] = dtSublet;
                lblSubletRecCnt.Text = Func.Common.sRowCntOfTable(dtSublet);
                BindDataToSubletGrid(bRecordIsOpen, 0);

                //Display Job Details   
                //Session["PartDetails"] = ds.Tables[3]; 
                Session["JobDetails"] = null;
                dtJob = ds.Tables[7];
                Session["JobDetails"] = dtJob;
                lblJobRecCnt.Text = Func.Common.sRowCntOfTable(dtJob);
                BindDataToJobGrid();

                dtWarrJobDescDet = ds.Tables[10];
                Session["InvJobDescDet"] = dtWarrJobDescDet;

                dtJbGrpTaxDetails = ds.Tables[9];
                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;

                Acc_dtGrpTaxDetails = ds.Tables[11];
                Session["JbAccGrpTaxDetails"] = Acc_dtGrpTaxDetails;

                CreateNewRowToTaxGroupDetailsTable();

                BindDataToGrid();

                Session["JbGrpTaxDetails"] = dtJbGrpTaxDetails;
                Session["JbAccGrpTaxDetails"] = Acc_dtGrpTaxDetails;

                if (Func.Convert.dConvertToDouble(txtClaimAmt.Text) == 0 || Func.Convert.iConvertToInt(txtClaimAmt.Text) == 0 || Func.Convert.sConvertToString(txtClaimAmt.Text) == "")
                {
                    double dClaimAmount = 0;

                    dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                         Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                    txtClaimAmt.Text = dClaimAmount.ToString("0.00");
                }
                if (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) == 0 || Func.Convert.iConvertToInt(txtAccClaimAmt.Text) == 0 || Func.Convert.sConvertToString(txtAccClaimAmt.Text) == "")
                {
                    double dClaimAmount = 0;

                    dClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                         Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                    txtAccClaimAmt.Text = dClaimAmount.ToString("0.00");
                }

                // Display Attach file  Details    
                if (PFileAttchDetails.Visible == true)
                {
                    dtFileAttach = ds.Tables[8];
                    lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                    ShowAttachedFiles();
                }

                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);
                }
                lnkSrvVAN.Visible = false;
                lnkSrvVAN.Attributes.Add("onclick", " return GetSrvVANDtls(this,'" + Location.iDealerId.ToString() + "')");
                if (Func.Convert.iConvertToInt(txtID.Text.ToString()) != 0 && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) != "Y" && DrpInvType.SelectedValue == "L") lnkSrvVAN.Visible = true;
                hdnSrvVANID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SrvVANHDRID"]);
                hdnbSrvVAN.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["bSrvVANShw"]);
                lnkSrvVAN.Visible = (hdnbSrvVAN.Value == "Y") ? true : false;
                if ((hdnReSubmitClaim.Value == "P" || hdnReSubmitRequest.Value == "P" || hdnReturnedClaim.Value == "P") &&
                    Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Confirm"]) != "Y" &&
                    Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Cancel"]) != "Y")
                {
                    txtDealerRemark.Enabled = true;
                    lnkSrvVAN.Visible = false;
                }

                Acc_PPartGroupDetails.Visible = (hdnIsSHQResource.Value.Trim() == "Y") ? true : false;                                
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {
            txtClaimDate.Enabled = bEnable;
            txtApprovalNo.Enabled = bEnable;
            txtApprovalDate.Enabled = bEnable;

            txtModelCode.Enabled = bEnable;
            txtModelDescription.Enabled = bEnable;
            txtGVW.Enabled = bEnable;

            txtChassisNo.Enabled = bEnable;
            txtEngineNo.Enabled = bEnable;
            txtVehicleNo.Enabled = bEnable;
            txtCustomerName.Enabled = bEnable;
            txtCustomerAddress.Enabled = bEnable;

            txtCustMobNo.Enabled = bEnable;
            txtCustEmail.Enabled = bEnable;

            txtInstallationDate.Enabled = bEnable;

            txtOdometer.Enabled = bEnable;
            txtHrsReading.Enabled = bEnable;
            drpRouteType.Enabled = bEnable;

            txtRepairOrderNo.Enabled = bEnable;
            txtRepairOrderDate.Enabled = bEnable;
            txtRepairCompleteDate.Enabled = bEnable;
            txtFailureDate.Enabled = bEnable;

            txtInvoiceNo.Enabled = bEnable;
            txtInvoiceDate.Enabled = bEnable;

            txtVECVShare.Enabled = bEnable;
            txtDealerShare.Enabled = bEnable;
            txtCustomerShare.Enabled = bEnable;

            txtSubDealerName.Enabled = bEnable;
            txtDealerRemark.Enabled = bEnable;

            //Set Grid Enable/disable 
            ComplaintsGrid.Enabled = bEnable;
            InvestigationsGrid.Enabled = bEnable;
            PartDetailsGrid.Enabled = bEnable;
            LabourDetailsGrid.Enabled = bEnable;
            LubricantDetailsGrid.Enabled = bEnable;
            SubletDetailsGrid.Enabled = bEnable;
            //JobDetailsGrid.Enabled = bEnable;
            //JobDetailsGrid.Enabled = (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") ? false : true;  

            //Megha 22/12/2012 
            // As per dicussion with deepti Madam, she told me after confirmation not able Attachment grid
            // FileAttchGrid.Enabled = bEnable;
            CntFileAttchDetails.Enabled = bEnable;
            //Megha 22/12/2012 

            OptShareType.Items[0].Enabled = bEnable;
            OptShareType.Items[1].Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

            if (hdnReSubmitClaim.Value == "P")
            {
                txtClaimDate.Enabled = true;
                CntFileAttchDetails.Enabled = true;
                //-------- Editing allowed for Rejected claim -----------------------------------
                ComplaintsGrid.Enabled = true;
                InvestigationsGrid.Enabled = true;
                PartDetailsGrid.Enabled = true;
                LabourDetailsGrid.Enabled = true;
                LubricantDetailsGrid.Enabled = true;
                SubletDetailsGrid.Enabled = true;
                JobDetailsGrid.Enabled = true;
                //-----------------------------------------------------------------------------
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
            }
            else if (hdnReSubmitRequest.Value == "P")
            {
                txtClaimDate.Enabled = true;
                txtOdometer.Enabled = true;
                txtHrsReading.Enabled = true;
                txtFailureDate.Enabled = true;
                //Set Grid Enable/disable 
                ComplaintsGrid.Enabled = true;
                InvestigationsGrid.Enabled = true;
                PartDetailsGrid.Enabled = true;
                LabourDetailsGrid.Enabled = true;
                LubricantDetailsGrid.Enabled = true;
                SubletDetailsGrid.Enabled = true;
                JobDetailsGrid.Enabled = true;
                CntFileAttchDetails.Enabled = true;
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);

            }
            else if (hdnReturnedClaim.Value == "P")
            {
                txtClaimDate.Enabled = true;
                JobDetailsGrid.Enabled = true;
                CntFileAttchDetails.Enabled = true;
                //-------- Editing allowed for Returned claim -----------------------------------
                ComplaintsGrid.Enabled = true;
                InvestigationsGrid.Enabled = true;
                PartDetailsGrid.Enabled = true;
                LabourDetailsGrid.Enabled = true;
                LubricantDetailsGrid.Enabled = true;
                SubletDetailsGrid.Enabled = true;
                //-----------------------------------------------------------------------------
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
            }
            if (txtUserType.Text == "6")
            {
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                btnReturnedClaim.Visible = false;
                btnReSubmitRequest.Visible = false;
                btnCreateWarrantyClaim.Visible = false;
                lblSelectModel.Visible = false;
            }
            trNewAttachment.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
            trNewAttachment1.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
            //txtFailureDate.Enabled = true;
            //if (txtLastRepairDate.Text == "")
            //{
            //    txtFailureDate.Enabled = false;
            //}       
        }

        // To Clear The form data
        private void ClearFormControl()
        {
            txtVECVShare.Attributes.Add("onkeypress", "null");
            txtCustomerShare.Attributes.Add("onkeypress", "null");
            txtDealerShare.Attributes.Add("onkeypress", "null");


            txtID.Text = "";
            txtClaimNo.Text = "";

            txtClaimDate.Text = "";
            //drpClaimType.SelectedValue = "0";
            //drpClaimType.Enabled = true;
            txtClaimRevNo.Text = "";
            txtModelID.Text = "";
            txtModelCode.Text = "";
            txtModelDescription.Text = "";
            lblSelectModel.Style.Add("display", "");
            lblServiceHistroy.Style.Add("display", "none");
            
            btnJobSave.Visible = false;
            btnJobConfirm.Visible = false;

            txtChassisNo.Text = "";
            txtEngineNo.Text = "";
            txtGVW.Text = "";
            //Sujata 21012011
            txtchassisID.Text = "";
            //Sujata 21012011

            txtVehicleNo.Text = "";
            txtCustomerName.Text = "";
            txtCustomerAddress.Text = "";

            txtCustMobNo.Text = "";
            txtCustEmail.Text = "";

            txtInstallationDate.Text = "";
            txtOdometer.Text = "";
            txtHrsReading.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceDate.Text = "";
            txtRepairOrderNo.Text = "";

            txtFailureDate.Text = "";
            txtRepairOrderDate.Text = "";
            txtRepairCompleteDate.Text = "";

            drpRouteType.SelectedValue = "0";

            txtPartAmount.Text = "";
            txtLabourAmount.Text = "";
            txtLubricantAmount.Text = "";
            txtSubletAmount.Text = "";
            txtClaimAmt.Text = "";

            txtAccPartAmount.Text = "";
            txtAccLabourAmount.Text = "";
            txtAccLubricantAmount.Text = "";
            txtAccSubletAmount.Text = "";
            txtAccClaimAmt.Text = "";

            txtRequestID.Text = "";
            txtRefClaimID.Text = "";

            txtRequestNo.Text = "";
            txtRequestDate.Text = "";

            txtVECVShare.Text = "";
            txtDealerShare.Text = "";
            txtCustomerShare.Text = "";

            txtRefClaimNo.Text = "";
            txtRefClaimDate.Text = "";
            txtSubDealerName.Text = "";
            txtDealerRemark.Text = "";
            hdnReturnedCnt.Value = "0";
            hdnRejectCnt.Value = "0";
            hdnReSubmitClaim.Value = "N";
            hdnReSubmitRequest.Value = "N";
            hdnPostShipmentDate.Value = "";
            hdnMinClaimDate.Value = "";
            //if (DropClaimTypes.SelectedValue == "18")
            //    lblMInsDate.Style.Add("display", "none");
            //else
            //    lblMInsDate.Style.Add("display", "");

            if (DropClaimTypes.SelectedValue == "18")
                txtInstallationDate.Mandatory = false;
            else
                txtInstallationDate.Mandatory = true;
            txtFailureDate.Enabled = true;
            ClearAllSession();
            SetDocumentDetails();
            MakeEnableDisableControls(true);
            txtClaimDate.Enabled = false;
            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
            //Set default value for all Grid 
            ComplaintsGrid.DataSource = null;
            ComplaintsGrid.DataBind();

            InvestigationsGrid.DataSource = null;
            InvestigationsGrid.DataBind();

            PartDetailsGrid.DataSource = null;
            PartDetailsGrid.DataBind();

            LabourDetailsGrid.DataSource = null;
            LabourDetailsGrid.DataBind();

            LubricantDetailsGrid.DataSource = null;
            LubricantDetailsGrid.DataBind();

            SubletDetailsGrid.DataSource = null;
            SubletDetailsGrid.DataBind();

            JobDetailsGrid.DataSource = null;
            JobDetailsGrid.DataBind();

            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();

            //Re Set the all record count 
            lblComplaintsRecCnt.Text = "";
            lblInvestigationsRecCnt.Text = "";
            lblPartRecCnt.Text = "";
            lblLabourRecCnt.Text = "";
            lblLubricantRecCnt.Text = "";
            lblSubletRecCnt.Text = "";
            lblJobRecCnt.Text = "";
            lblFileAttachRecCnt.Text = "";

            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            {
                //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "EC"+ DrpInvType.SelectedValue.ToString(), Location.iDealerId);
                //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", DrpInvType.SelectedValue.ToString() + "C", Location.iDealerId);
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", ((sClaimTypeIdChassis == "14") ? "WP" : (sClaimTypeIdChassis == "2") ? "GT" : (sClaimTypeIdChassis == "8" || sClaimTypeIdChassis == "16") ? "GC" : (sClaimTypeIdChassis == "6") ? "CP" : (sClaimTypeIdChassis == "10") ? "RM" : "WC") + DrpInvType.SelectedValue.ToString(), Location.iDealerId);                
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            {
                //VHP Waaranty Start
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GR", Location.iDealerId);
                //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GCR", Location.iDealerId);
                //VHP Waaranty Start
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
            {
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "HR", Location.iDealerId);
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            }
        }

        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int iNewRowToAdd = Func.Convert.iConvertToInt(txtNewRecountCount.Text);
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    if ((sender as GridView).ID == "ComplaintsGrid")//Complaints Grid
                    {
                        bFillDetailsFromComplaintGrid();
                        //BindDataToComplaintGrid(true, iNewRowToAdd);
                        CreateNewRowToComplaintGrid();
                        Session["ComplaintsDetails"] = dtComplaint;
                        BindDataToComplaintGrid();

                    }
                    else if ((sender as GridView).ID == "InvestigationsGrid")//InvestigationsGrid
                    {
                        bFillDetailsFromInvestigationsGrid();
                        CreateNewRowToInvestigationsGrid();
                        Session["InvestigationDetails"] = dtInvestigations;
                        BindDataToInvestigationsGrid();
                    }
                    else if ((sender as GridView).ID == "PartDetailsGrid")//PartDetails Grid
                    {
                        bFillDetailsFromPartGrid();
                        BindDataToPartGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "LabourDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromLabourGrid();
                        BindDataToLaborGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "LubricantDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromLubricantGrid();
                        BindDataToLubricantGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "SubletDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromSubletGrid();
                        BindDataToSubletGrid(true, iNewRowToAdd);
                    }
                    else if ((sender as GridView).ID == "JobDetailsGrid")//Labour Grid
                    {
                        bFillDetailsFromJobGrid("N");
                        BindDataToJobGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        #endregion

        #region  Complaints Function
        // Create Row To Complaint Grid
        private void CreateNewRowToComplaintGrid()
        {
            try
            {
                DataRow dr;

                dr = dtComplaint.NewRow();
                dr["ID"] = 0;
                dr["Complaint_ID"] = 0;
                dr["Complaint_Desc"] = "";
                dr["Status"] = "N";
                dtComplaint.Rows.Add(dr);
                dtComplaint.AcceptChanges();


            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void BindDataToComplaintGrid()
        {
            try
            {
                if (Session["ComplaintsDetails"] == null)
                {
                    CreateNewRowToComplaintGrid();
                    Session["ComplaintsDetails"] = dtComplaint;

                }
                else
                {
                    dtComplaint = (DataTable)Session["ComplaintsDetails"];
                    if (dtComplaint.Rows.Count == 0)
                    {
                        CreateNewRowToComplaintGrid();

                    }
                }

                ComplaintsGrid.DataSource = dtComplaint;
                ComplaintsGrid.DataBind();

                SetControlPropertyToComplaintGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // Set Control property To Complaint Grid    
        private void SetControlPropertyToComplaintGrid()
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sComplaintID = "";
                for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
                {
                    //Complaint Description            
                    TextBox txtNewComplaintDesc = (TextBox)ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc");

                    //Complaint 
                    DropDownList drpComplaint = (DropDownList)ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint");
                    Func.Common.BindDataToCombo(drpComplaint, clsCommon.ComboQueryType.CustomerComplaints, Location.iDealerId);

                    sRecordStatus = "N";

                    if (idtRowCnt < dtComplaint.Rows.Count)
                    {
                        txtNewComplaintDesc.Text = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_Desc"]);
                        sComplaintID = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_ID"]);
                        // Add New Complaint
                        if (sComplaintID == "9999" || sComplaintID == "0")
                        {
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpComplaint.Items.Add(lstitm);
                            drpComplaint.Attributes.Add("onChange", "OnComplaintValueChange(event, this,'" + txtNewComplaintDesc.ID + "')");
                        }
                        else
                        {
                            drpComplaint.Attributes.Add("onChange", "return CheckComplaintSelected(event,this);");
                        }
                        drpComplaint.SelectedValue = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Complaint_ID"]);
                        sRecordStatus = Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;

                    }
                    if (sComplaintID == "9999")
                    {
                        txtNewComplaintDesc.Style.Add("display", "");
                        drpComplaint.Style.Add("display", "none");
                    }
                    else
                    {
                        drpComplaint.Style.Add("display", "");
                        txtNewComplaintDesc.Style.Add("display", "none");
                    }

                    //New 
                    LinkButton lnkNew = (LinkButton)ComplaintsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)ComplaintsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)ComplaintsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Attributes.Add("onClick", "SelectDeleteCheckboxCommon(this)");
                    //Chk.Text = "Delete";
                    Chk.Attributes["align"] = "center";
                    Chk.Style.Add("display", "none");
                    lnkNew.Style.Add("display", "none");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        ComplaintsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        drpComplaint.Enabled = false;
                    }
                    else
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                        // Allow New To Last Row
                        if ((iRowCnt + 1) == ComplaintsGrid.Rows.Count)
                        {
                            lnkNew.Style.Add("display", "");
                        }
                    }
                }
                //lblComplaintsRecCnt.Text = idtRowCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Complaint Grid
        private bool bFillDetailsFromComplaintGrid()
        {
            string sStatus = "";
            dtComplaint = (DataTable)Session["ComplaintsDetails"];
            int iCntForDelete = 0;
            int iComplaintID = 0;
            string sComplainDesc = "";
            bool bValidate = false;
            for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
            {
                //ComplaintID                
                iComplaintID = Func.Convert.iConvertToInt((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedValue);
                //sComplainDesc = (ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc") as TextBox).Text.ToString();

                if (Func.Convert.sConvertToString((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedItem) == "NEW")
                    sComplainDesc = (ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc") as TextBox).Text.ToString();
                else
                    sComplainDesc = Func.Convert.sConvertToString((ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint") as DropDownList).SelectedItem);

                if (iComplaintID != 0 && sComplainDesc != "")
                {
                    bValidate = true;

                    dtComplaint.Rows[iRowCnt]["Complaint_ID"] = iComplaintID;

                    //Complaint Description
                    if (iComplaintID == Func.Convert.iConvertToInt("9999") && sComplainDesc != "")
                        dtComplaint.Rows[iRowCnt]["Complaint_Desc"] = sComplainDesc;

                    dtComplaint.Rows[iRowCnt]["Complaint_Desc"] = sComplainDesc;
                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtComplaint.Rows[iRowCnt]["Status"]);

                    dtComplaint.Rows[iRowCnt]["Status"] = "S";// for Save                

                    CheckBox Chk = (CheckBox)ComplaintsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtComplaint.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete == ComplaintsGrid.Rows.Count)
            {
                bValidate = false;
            }

            // Sujata 19012011
            if (txtChkfun.Text == "false") bValidate = true;
            // Sujata 19012011

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the complaints.');</script>");
            }
        Last:
            return bValidate;
        }

        //To Get Total Complaint Count
        private void CalculateComplaintGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sComplaintID = "";
                for (int iRowCnt = 0; iRowCnt < ComplaintsGrid.Rows.Count; iRowCnt++)
                {
                    //Complaint Description            
                    TextBox txtNewComplaintDesc = (TextBox)ComplaintsGrid.Rows[iRowCnt].FindControl("txtNewComplaintDesc");

                    //Complaint 
                    DropDownList drpComplaint = (DropDownList)ComplaintsGrid.Rows[iRowCnt].FindControl("drpComplaint");
                    sComplaintID = drpComplaint.SelectedValue;
                    if (sComplaintID != "0")
                    {
                        if (sComplaintID != "9999")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                        else if (txtNewComplaintDesc.Text != "")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                    }
                }
                lblComplaintsRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        
        #endregion

        #region Investigations Function
        // Create Row To Part Grid
        private void CreateNewRowToInvestigationsGrid()
        {
            try
            {
                DataRow dr;

                dr = dtInvestigations.NewRow();
                dr["ID"] = 0;
                dr["Investigation_ID"] = 0;
                dr["Investigation_Desc"] = "";
                dr["Status"] = "N";
                dtInvestigations.Rows.Add(dr);
                dtInvestigations.AcceptChanges();


            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Investigations Grid
        private void BindDataToInvestigationsGrid()
        {
            try
            {   

                if (Session["InvestigationDetails"] == null)
                {
                    CreateNewRowToInvestigationsGrid();
                    Session["InvestigationDetails"] = dtInvestigations;

                }
                else
                {
                    dtInvestigations = (DataTable)Session["InvestigationDetails"];
                    if (dtInvestigations.Rows.Count == 0)
                    {
                        CreateNewRowToInvestigationsGrid();

                    }
                }

                InvestigationsGrid.DataSource = dtInvestigations;
                InvestigationsGrid.DataBind();

                SetControlPropertyToInvestigationsGrid(false);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToInvestigationsGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                string sInvestigationID = "";
                for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
                {
                    //Investigations Description            
                    TextBox txtNewInvestigationDesc = (TextBox)InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc");


                    //Investigations 
                    DropDownList drpInvestigations = (DropDownList)InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation");
                    Func.Common.BindDataToCombo(drpInvestigations, clsCommon.ComboQueryType.DealerInvistigation, Location.iDealerId);

                    if (idtRowCnt < dtInvestigations.Rows.Count)
                    {
                        txtNewInvestigationDesc.Text = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_Desc"]);
                        //drpInvestigations.SelectedValue = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_ID"]);
                        sInvestigationID = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Investigation_ID"]);

                        // Add New Investigations  
                        if (sInvestigationID == "0" || sInvestigationID == "9999")
                        {
                            ListItem lstitm = new ListItem("NEW", "9999");
                            drpInvestigations.Items.Add(lstitm);
                            drpInvestigations.Attributes.Add("onChange", "return OnInvestigationValueChange(event, this,'" + txtNewInvestigationDesc.ID + "')");
                        }
                        else
                        {
                            drpInvestigations.Attributes.Add("onChange", "return CheckInvestigationSelected(event,this);)");
                        }
                        drpInvestigations.SelectedValue = sInvestigationID;
                        sRecordStatus = Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;
                    }
                    if (sInvestigationID == "9999")
                    {
                        txtNewInvestigationDesc.Style.Add("display", "");
                        drpInvestigations.Style.Add("display", "none");
                    }
                    else
                    {
                        drpInvestigations.Style.Add("display", "");
                        txtNewInvestigationDesc.Style.Add("display", "none");
                    }

                    //New 
                    LinkButton lnkNew = (LinkButton)InvestigationsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)InvestigationsGrid.Rows[iRowCnt].FindControl("lblCancel");
                    //Delete 
                    CheckBox Chk = (CheckBox)InvestigationsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Attributes.Add("onClick", "SelectDeleteCheckboxCommon(this)");
                    Chk.Text = "Delete";
                    Chk.Attributes["align"] = "center";
                    Chk.Style.Add("display", "none");
                    lnkNew.Style.Add("display", "none");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        ComplaintsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        drpInvestigations.Enabled = false;
                    }
                    else
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                        // Allow New To Last Row
                        if ((iRowCnt + 1) == InvestigationsGrid.Rows.Count)
                        {
                            lnkNew.Style.Add("display", "");
                        }
                    }

                    //if (sRecordStatus == "U")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}

                    //if (sRecordStatus == "N")
                    //{
                    //    //lnkNew.Style.Add("display", "");                
                    //}
                    //else if (sRecordStatus == "D")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    InvestigationsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    //}
                    //else if (sRecordStatus == "E")
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                    //// Allow New To Last Row
                    //if ((iRowCnt + 1) == InvestigationsGrid.Rows.Count)
                    //{

                    //    lnkNew.Style.Add("display", "");

                    //}
                    //if (bRecordIsOpen == false)
                    //{
                    //    lnkNew.Style.Add("display", "none");
                    //    Chk.Style.Add("display", "none");
                    //    lnkCancel.Style.Add("display", "none");
                    //}
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            //lblInvestigationsRecCnt.Text = Func.Common.sRowCntOfTable(dtInvestigations);
        }

        //Fill Details From Investigations Grid
        private bool bFillDetailsFromInvestigationsGrid()
        {
            string sStatus = "";
            dtInvestigations = (DataTable)Session["InvestigationDetails"];
            int iCntForDelete = 0;
            int iInvestigationID = 0;
            string sInvestigationDesc = "";
            bool bValidate = false;
            for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
            {
                //InvestigationID                
                iInvestigationID = Func.Convert.iConvertToInt((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedValue);
                //sInvestigationDesc = (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;
                if (Func.Convert.sConvertToString((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedItem) == "NEW")
                    sInvestigationDesc = (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;
                else
                    sInvestigationDesc = Func.Convert.sConvertToString((InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation") as DropDownList).SelectedItem);

                if (iInvestigationID != 0 && sInvestigationDesc != "")
                {
                    bValidate = true;

                    //if (txtRefClaimID.Text != "")
                    //dtInvestigations.Rows[iRowCnt]["ID"] = 0;

                    // Investigations ID
                    //if (iInvestigationID != Func.Convert.iConvertToInt("9999"))
                    dtInvestigations.Rows[iRowCnt]["Investigation_ID"] = iInvestigationID;

                    //Investigations Description
                    if (iInvestigationID == Func.Convert.iConvertToInt("9999"))
                        dtInvestigations.Rows[iRowCnt]["Investigation_Desc"] = sInvestigationDesc;// (InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc") as TextBox).Text;

                    dtInvestigations.Rows[iRowCnt]["Investigation_Desc"] = sInvestigationDesc;

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtInvestigations.Rows[iRowCnt]["Status"]);

                    dtInvestigations.Rows[iRowCnt]["Status"] = "S";// for Save                

                    CheckBox Chk = (CheckBox)InvestigationsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtInvestigations.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete == InvestigationsGrid.Rows.Count)
            {
                bValidate = false;
            }

            // Sujata 19012011
            if (txtChkfun.Text == "false") bValidate = true;
            // Sujata 19012011

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select the dealar investigation.');</script>");
            }
            return bValidate;
        }
        //To Get Total Invetigation Count
        private void CalculateInvestigationGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sInvestigationID = "";
                for (int iRowCnt = 0; iRowCnt < InvestigationsGrid.Rows.Count; iRowCnt++)
                {
                    //Investigation Description            
                    TextBox txtNewInvestigationDesc = (TextBox)InvestigationsGrid.Rows[iRowCnt].FindControl("txtNewInvestigationDesc");

                    //Investigation 
                    DropDownList drpInvestigation = (DropDownList)InvestigationsGrid.Rows[iRowCnt].FindControl("drpInvestigation");
                    sInvestigationID = drpInvestigation.SelectedValue;
                    if (sInvestigationID != "0")
                    {
                        if (sInvestigationID != "9999")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                        else if (txtNewInvestigationDesc.Text != "")
                        {
                            iRecCnt = iRecCnt + 1;
                        }
                    }
                }
                lblInvestigationsRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Parts Function
        // Create Row To Part Grid
        private void CreateNewRowToPartGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultPart = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxPartGridRowCount = 0;
                iMaxPartGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxPartGridRowCount"]);

                if (Session["PartDetails"] != null)
                {
                    dtDefaultPart = (DataTable)Session["PartDetails"];
                }
                else
                {
                    dtDefaultPart = dtPart;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultPart.Rows.Count == 0)
                    {
                        dtDefaultPart.Columns.Clear();
                        dtDefaultPart.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("GCR_Det_ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Part_No", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Qty", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDefaultPart.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("VECV_Share", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Dealer_Share", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Cust_Share", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Deduction_Percentage", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Deducted_Amount", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Accepted_Amount", typeof(float)));
                        dtDefaultPart.Columns.Add(new DataColumn("Replaced_Part_No_ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Replaced_Part_No", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Replaced_Part_Name", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Failed_Make", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Replaced_Make", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Status", typeof(string)));
                        //dtDefaultPart.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));              

                    }
                    else
                    {
                        if (dtDefaultPart.Rows.Count >= iMaxPartGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxPartGridRowCount;
                }

                iMaxPartGridRowCount = iMaxPartGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxPartGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultPart.NewRow();
                    dr["ID"] = 0;
                    dr["GCR_Det_ID"] = 0;
                    dr["Part_No_ID"] = 0;
                    dr["Part_No"] = "";
                    dr["Part_Name"] = "";
                    dr["Qty"] = 0;
                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["Job_Code_ID"] = 0;
                    dr["VECV_Share"] = 999;
                    dr["Dealer_Share"] = 999;
                    dr["Cust_Share"] = 999;
                    dr["Accepted_Qty"] = 0;
                    dr["Deduction_Percentage"] = 0;
                    dr["Deducted_Amount"] = 0;
                    dr["Accepted_Amount"] = 0;
                    dr["Replaced_Part_No_ID"] = 0;
                    dr["Replaced_Part_No"] = "";
                    dr["Replaced_Part_Name"] = "";
                    dr["Failed_Make"] = "";
                    dr["Replaced_Make"] = "";
                    dr["Status"] = "N";
                    //dr["ChangeDetails_YN"] = "N";                   
                    dtDefaultPart.Rows.Add(dr);
                    dtDefaultPart.AcceptChanges();
                }
            Bind:
                Session["PartDetails"] = dtDefaultPart;
                PartDetailsGrid.DataSource = dtDefaultPart;
                PartDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Part Grid
        private void BindDataToPartGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToPartGrid(iNoRowToAdd);
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
                else
                {
                    PartDetailsGrid.DataSource = dtPart;
                    PartDetailsGrid.DataBind();
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {                
                bFillDetailsFromPartGrid();
                BindDataToPartGrid(true, 0);               
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToPartGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "N";
                string sPartId = "";
                int idtRowCnt = 0;
                string sPartName = "";
                bool bShowNewPart = true;
                double dPartAmount = 0;
                double dAccPartAmount = 0;
                double dTmpValue = 0;
                if (PartDetailsGrid.Rows.Count == 0) return;
                

                PartDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none");
                PartDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "none");
                PartDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");


                PartDetailsGrid.HeaderRow.Cells[20].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                PartDetailsGrid.HeaderRow.Cells[21].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");


                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                {
                    PartDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "");
                }
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    // Add New Part 
                    LinkButton lnkSelectPart = (LinkButton)PartDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowSpWPFPart(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");

                    //PartID
                    TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    //Part No
                    TextBox txtPartNo = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox);

                    DropDownList drpPartMake = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpPartMake");
                    //Func.Common.BindDataToCombo(drpPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["DealerID"].ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }

                    DropDownList drpRPartMake = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpRPartMake");
                    //Func.Common.BindDataToCombo(drpRPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpRPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["DealerID"].ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpRPartMake, clsCommon.ComboQueryType.Make, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }

                    //JobCode
                    DropDownList drpJobCode = (DropDownList)PartDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    //Jobcard Det ID
                    TextBox txtJobcardDetID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID");

                    // New  Replaced Part
                    Label lblChngPart = (Label)PartDetailsGrid.Rows[iRowCnt].FindControl("lblChngPart");
                    lblChngPart.Attributes.Add("onclick", " return ShowPartMasterForReplaced(this," + sDealerId + ")");
                    
                    TextBox txtQty = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtQty") as TextBox);
                    
                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2") as TextBox);
                    TextBox txtBFRGST = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtBFRGST") as TextBox);
                    TextBox txtVECVShare = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox);                    

                    sPartId = "0";
                    sRecordStatus = "N";
                    if (idtRowCnt < dtPart.Rows.Count)
                    {
                        //PartID
                        sPartId = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Part_No_ID"]);
                        txtPartID.Text = sPartId;

                        //PartNo 
                        txtPartNo.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Part_No"]);
                        //Status
                        sRecordStatus = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Status"]);

                        //Part Name
                        sPartName = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Part_Name"]);
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text = sPartName;
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).ToolTip = sPartName;

                        //Quantity
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["Qty"]));
                        if (dTmpValue != 0)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtQty") as TextBox).Text = dTmpValue.ToString("0");
                        }
                        //Rate                
                        dTmpValue = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Rate"]));
                        if (dTmpValue != 0 || sRecordStatus != "N")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Total                
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Total"]);
                        dPartAmount = dPartAmount + dTmpValue;
                        if (dTmpValue != 0 || sRecordStatus != "N")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        txtPTax.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["TaxID"]).Trim();
                        txtPTax1.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax1"]).Trim();
                        txtPTax2.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Tax2"]).Trim();
                        txtBFRGST.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["BFRGST"]).Trim();
                        //Failed Make
                        //(PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartMake") as TextBox).Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Failed_Make"]);
                        drpPartMake.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Failed_Make"]);                        

                        //Replaced Part ID 
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartID") as TextBox).Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Replaced_Part_No_ID"]);

                        //Replaced Part No.
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartNo") as TextBox).Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Replaced_Part_No"]);

                        //Replaced Part Name
                        sPartName = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Replaced_Part_Name"]);
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartName") as TextBox).Text = sPartName;
                        (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartName") as TextBox).ToolTip = sPartName;

                        //Replaced Part Make
                        //(PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartMake") as TextBox).Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Replaced_Make"]);
                        drpRPartMake.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Replaced_Make"]);

                        // Job Code ID
                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Job_Code_ID"]);

                        // job Detail ID
                        txtJobcardDetID.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Jobcard_Det_ID"]);

                        //Vecv Percentage
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["VECV_Share"]);
                        if (dTmpValue != 999)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }
                        //Dealer Percentage
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Dealer_Share"]);
                        if (dTmpValue != 999)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Cust Percentage
                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Cust_Share"]);
                        if (dTmpValue != 999)
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }


                        TextBox txtAccQty = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccQty") as TextBox);
                        TextBox txtDeduction = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox);
                        TextBox txtDeductionAmt = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeductionAmt") as TextBox);
                        TextBox txtAccAmount = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox);

                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Accepted_Qty"]);
                        txtAccQty.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Deduction_Percentage"]);
                        txtDeduction.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Deducted_Amount"]);
                        txtDeductionAmt.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtPart.Rows[iRowCnt]["Accepted_Amount"]);
                        txtAccAmount.Text = dTmpValue.ToString("0.00");
                        dAccPartAmount = dAccPartAmount + dTmpValue;                        

                        idtRowCnt = idtRowCnt + 1;
                    }

                    PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "none");
                    PartDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "none");
                    PartDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");

                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");
                        if (OptShareType.SelectedValue == "1")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        }
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");
                        if (OptShareType.SelectedValue == "1")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        }
                    }

                    PartDetailsGrid.Rows[iRowCnt].Cells[20].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    PartDetailsGrid.Rows[iRowCnt].Cells[21].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

                    Label lnkCancel = (Label)PartDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //To Add New Line
                    LinkButton lnkNew = (LinkButton)PartDetailsGrid.Rows[iRowCnt].FindControl("lnkNew");

                    //Delete 
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    lnkNew.Style.Add("display", "none");
                    Chk.Style.Add("display", "");
                    lnkSelectPart.Style.Add("display", "none");
                    lblChngPart.Style.Add("display", "none");
                    
                    //If Part Id  is not allocated
                    if (sPartId == "0")
                    {
                        txtPartNo.Style.Add("display", "none");
                        if (bShowNewPart == true)
                        {
                            if (txtID.Text != "" && txtID.Text != "0" && DrpInvType.SelectedValue == "P") lnkSelectPart.Style.Add("display", "");
                            bShowNewPart = false;
                        }
                    }
                    else
                    {
                        lblChngPart.Style.Add("display", "");
                    }
                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        PartDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E")
                    {
                        lnkNew.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                    }
                    // Allow New To Last Row
                    if ((iRowCnt + 1) == PartDetailsGrid.Rows.Count)
                    {

                        lnkNew.Style.Add("display", "");
                    }
                    if (bRecordIsOpen == false)
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "");
                        //lblAddPart.Style.Add("display", "none");
                        lblChngPart.Style.Add("display", "none");
                    }
                    lnkNew.Style.Add("display", "none");
                    //Chk.Style.Add("display", "");
                    lnkCancel.Style.Add("display", "none");
                    lblChngPart.Style.Add("display", "none");
                    //drpJobCode.Enabled = false;
                    //drpPartMake.Enabled = false;
                    //txtQty.Attributes.Add("onkeydown", "return CheckForTextBoxValue(event,this,'5')");
                }
                //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                //{
                txtPartAmount.Text = dPartAmount.ToString("0.00");
                txtAccPartAmount.Text = dAccPartAmount.ToString("0.00");
                
                double dClaimAmount = 0;

                dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                      Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                txtClaimAmt.Text = dClaimAmount.ToString("0.00");

                double dAccClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                   Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                txtAccClaimAmt.Text = dAccClaimAmount.ToString("0.00");
                //}
                //lblPartRecCnt.Text = Func.Common.sRowCntOfTable(dtPart);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        //Fill Details From Part Grid
        private bool bFillDetailsFromPartGrid()
        {
            string sStatus = "";
            dtPart = (DataTable)Session["PartDetails"];
            int iCntForDelete = 0;
            int iPartID = 0;
            int iPartQty = 0;
            bool bValidate = false;
            double dRate = 0;

            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                //PartID                
                iPartID = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                if (iPartID != 0)
                {
                    //ID
                    if (txtRefClaimID.Text != "")
                        dtPart.Rows[iRowCnt]["ID"] = 0;

                    dtPart.Rows[iRowCnt]["Part_No_ID"] = iPartID;

                    //Jobcard_Det_ID
                    dtPart.Rows[iRowCnt]["Jobcard_Det_ID"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID") as TextBox).Text;

                    //PartNo Or NewPart
                    dtPart.Rows[iRowCnt]["part_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

                    //Part Name
                    dtPart.Rows[iRowCnt]["Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text;

                    // Failed Part Make
                    //dtPart.Rows[iRowCnt]["Failed_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartMake") as TextBox).Text;
                    dtPart.Rows[iRowCnt]["Failed_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("drpPartMake") as DropDownList).SelectedValue;                    

                    // Replaced PartID
                    dtPart.Rows[iRowCnt]["Replaced_Part_No_ID"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartID") as TextBox).Text;

                    //Replaced PartNo 
                    dtPart.Rows[iRowCnt]["Replaced_Part_No"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartNo") as TextBox).Text;

                    //Replaced Part Name
                    dtPart.Rows[iRowCnt]["Replaced_Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartName") as TextBox).Text;

                    // Replaced Part Make
                    //dtPart.Rows[iRowCnt]["Replaced_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRPartMake") as TextBox).Text;
                    //dtPart.Rows[iRowCnt]["Replaced_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("drpRPartMake") as DropDownList).SelectedValue;
                    dtPart.Rows[iRowCnt]["Replaced_Make"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("drpPartMake") as DropDownList).SelectedValue;

                    // Get Qty
                    iPartQty = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtQty") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Qty"] = iPartQty;

                    // Get Rate
                    dRate = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Rate"] = dRate;

                    // Get Total
                    dtPart.Rows[iRowCnt]["Total"] = iPartQty * dRate;// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);

                    dtPart.Rows[iRowCnt]["TaxID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax") as TextBox).Text);

                    dtPart.Rows[iRowCnt]["BFRGST"] = Func.Convert.sConvertToString((PartDetailsGrid.Rows[iRowCnt].FindControl("txtBFRGST") as TextBox).Text);                    

                    //Accepted Qty & Accepted Amount
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmTechnicalGoodwill && ValenmFormUsedFor != clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        dtPart.Rows[iRowCnt]["Accepted_Qty"] = iPartQty;
                        dtPart.Rows[iRowCnt]["Accepted_Amount"] = dtPart.Rows[iRowCnt]["Total"];
                    }

                    //JobCode  

                    dtPart.Rows[iRowCnt]["Job_Code_ID"] = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                    //Deducation amount
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmTechnicalGoodwill)
                    {
                        dtPart.Rows[iRowCnt]["Deduction_Percentage"] = 0;
                        dtPart.Rows[iRowCnt]["Deducted_Amount"] = 0;
                    }




                    // Record Status
                    sStatus = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Status"]);

                    dtPart.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                    dtPart.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                    if (iPartQty != 0)
                    {
                        bValidate = true;
                        dtPart.Rows[iRowCnt]["Status"] = "S";// for Save 
                    }
                    CheckBox Chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtPart.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }

                }
                //else
                //    if (Func.Convert.iConvertToInt(dtPart.Rows[iRowCnt]["ID"]) != 0)
                //    {
                //        if(iPartID==0)
                //            dtPart.Rows[iRowCnt]["Status"] = "D";
                //    }
            }

            if (iCntForDelete != 0)
            {
                if (iCntForDelete == PartDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                }
            }
            return bValidate;
        }

        //To Get Total Part Count
        private void CalculatePartGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Part Description            
                    TextBox txtPartID = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    sPartID = txtPartID.Text;
                    if (sPartID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblPartRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Labour Function

        // Create Row To Labour Grid
        private void CreateNewRowToLabourGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                int iMaxLabourGridRowCount = 0;
                DataTable dtDefaultLabour = new DataTable();
                int iRowCntStartFrom = 0;
                iMaxLabourGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxLabourGridRowCount"]);
                if (Session["LabourDetails"] != null)
                {
                    dtDefaultLabour = (DataTable)Session["LabourDetails"];
                }
                else
                {
                    dtDefaultLabour = dtLabour;
                }

                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultLabour.Rows.Count == 0)
                    {
                        dtDefaultLabour.Columns.Clear();
                        dtDefaultLabour.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("GCR_Det_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Labour_Desc", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultLabour.Columns.Add(new DataColumn("VECV_Share", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Dealer_Share", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Cust_Share", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Accepted_ManHrs", typeof(double)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Deduction_Percentage", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Deducted_Amount", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Accepted_Amount", typeof(float)));
                        dtDefaultLabour.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultLabour.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultLabour.Rows.Count >= iMaxLabourGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxLabourGridRowCount;
                }

                iMaxLabourGridRowCount = iMaxLabourGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLabourGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultLabour.NewRow();
                    dr["ID"] = 0;
                    dr["GCR_Det_ID"] = 0;
                    dr["Labour_ID"] = 0;
                    dr["Labour_Code"] = "";
                    dr["Labour_Desc"] = "";
                    dr["ManHrs"] = 0;
                    dr["Rate"] = 0;
                    dr["Total"] = 0;
                    dr["Job_Code_ID"] = 0;
                    dr["VECV_Share"] = 999;
                    dr["Dealer_Share"] = 999;
                    dr["Cust_Share"] = 999;
                    dr["Accepted_ManHrs"] = 0;
                    dr["Deduction_Percentage"] = 0;
                    dr["Deducted_Amount"] = 0;
                    dr["Accepted_Amount"] = 0;
                    dr["Status"] = "N";
                    dr["ChangeDetails_YN"] = "N";
                    dtDefaultLabour.Rows.Add(dr);
                    dtDefaultLabour.AcceptChanges();
                }

            Bind:
                Session["LabourDetails"] = dtDefaultLabour;
                LabourDetailsGrid.DataSource = dtDefaultLabour;
                LabourDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Bind Data to labour Grid
        private void BindDataToLaborGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToLabourGrid(iNoRowToAdd);
                    SetControlPropertyToLabourGrid(bRecordIsOpen);
                }
                else
                {
                    LabourDetailsGrid.DataSource = dtLabour;
                    LabourDetailsGrid.DataBind();
                    SetControlPropertyToLabourGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Labour Grid
        private void SetControlPropertyToLabourGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "N";
                // Hide Labour Id  Column
                if (LabourDetailsGrid.Rows.Count == 0) return;
                LabourDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none");
                string sLabourId = "0";
                int idtRowCnt = 0;
                double dLabourAmount = 0;
                double dAccLabourAmount = 0;
                bool bShowLabour = true;
                double dTmpValue = 0;
                string sLstTwoDigit = "";
                string sAddLbrID = "";
                string sFirstFiveDigit = "";
                if (LabourDetailsGrid.Rows.Count != 0)
                {
                    LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");
                    LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                    LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");
                }
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                {
                    if (LabourDetailsGrid.Rows.Count != 0)
                    {
                        LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");
                        LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");
                        LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "");

                    }
                }
                LabourDetailsGrid.HeaderRow.Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                LabourDetailsGrid.HeaderRow.Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                LabourDetailsGrid.HeaderRow.Cells[16].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

             

                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //LabourID            
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[1].Style.Add("display", "none");

                    //LabourNo Or NewLabour
                    TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");

                    // New  Labour Control
                    //Label lblNewLabour = (Label)LabourDetailsGrid.Rows[iRowCnt].FindControl("lblNewLabour");
                    //lblNewLabour.Attributes.Add("onclick", " return ShowMultiLabourMaster(this," + sDealerId + ")");
                    //lblNewLabour.Style.Add("display", "none");

                    LinkButton lnkSelectLabour = (LinkButton)LabourDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectLabour");
                    lnkSelectLabour.Attributes.Add("onclick", "return ShowMultiLabourMaster(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");
                    lnkSelectLabour.Style.Add("display", "none");

                    DropDownList drpLbrDescription = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription");
                    if (txtUserType.Text == "6")
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Location.iDealerId.ToString());
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpLbrDescription, clsCommon.ComboQueryType.AddLaborDesc, Location.iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString());
                    }
                    TextBox txtLbrDescription = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription");


                    //Labour Name
                    TextBox txtLabourDesc = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc");
                    //txtLabourDesc.Attributes.Add("disabled", "disabled");

                    // Man Hrs
                    TextBox txtManHrs = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs");

                    //Rate
                    TextBox txtRate = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate");


                    //Total
                    TextBox txtTotal = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");                  

                    // Old Amount
                    TextBox txtOldAmount = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtOldAmount");
                    txtOldAmount.Style.Add("display", "none");

                    //JobCode
                    DropDownList drpJobCode = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    //Jobcard Det ID
                    TextBox txtJobcardDetID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID");

                    TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox);
                    TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1") as TextBox);
                    TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2") as TextBox);
                   
                    sLabourId = "0";
                    sRecordStatus = "N";

                    if (idtRowCnt < dtLabour.Rows.Count)
                    {
                        // labour ID 
                        sLabourId = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Labour_ID"]);
                        txtLabourID.Text = sLabourId;

                        //Labour Code 
                        txtLabourCode.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Labour_Code"]);
                        txtLabourCode.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        if (sLabourId != "0")
                        {
                            sLstTwoDigit = txtLabourCode.Text.ToString().Substring(Func.Convert.iConvertToInt(txtLabourCode.Text.ToString().Length) - 2, 2);
                            sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                        }
                        
                        txtManHrs.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        drpLbrDescription.Style.Add("display", "none");
                        txtLbrDescription.Style.Add("display", "none");
                        txtLabourDesc.Style.Add("display", "none");
                        if (sFirstFiveDigit == "MTIMI" || sFirstFiveDigit == "MTICC")
                        {
                            txtTotal.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtTotal.Attributes.Add("onblur", "return calculateLabourTotal(this);");

                            //sAddLbrID = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"]);
                            drpLbrDescription.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"]);

                            //txtLbrDescription.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Labour_Desc"]);
                            //sAddLbrID = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"]);                

                            if (sFirstFiveDigit == "MTIMI")
                            {
                                ListItem lstitm = new ListItem("NEW", "9999");
                                drpLbrDescription.Items.Add(lstitm);
                                drpLbrDescription.Attributes.Add("onChange", "OnLbrDescValueChange(this,'" + txtLbrDescription.ID + "')");
                            }
                            if (drpLbrDescription.SelectedValue == "9999")
                            {
                                drpLbrDescription.Style.Add("display", "none");
                                txtLbrDescription.Style.Add("display", "");
                                txtLabourDesc.Style.Add("display", "none");
                            }
                            else if (sFirstFiveDigit == "MTICC")
                            {
                                drpLbrDescription.Style.Add("display", "none");
                                txtLbrDescription.Style.Add("display", "none");
                                txtLabourDesc.Style.Add("display", "");
                            }
                            else
                            {
                                drpLbrDescription.Style.Add("display", "");
                                txtLbrDescription.Style.Add("display", "none");
                                txtLabourDesc.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                            drpLbrDescription.Style.Add("display", "none");
                            txtLbrDescription.Style.Add("display", "none");
                            txtLabourDesc.Style.Add("display", "");
                        }
                        drpLbrDescription.Enabled = (sFirstFiveDigit == "MTIMI") ? true : false;
                        //Labour Description
                        txtLabourDesc.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Labour_Desc"]);

                        txtLabourDesc.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        //Man Hrs
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["ManHrs"]);
                        if (dTmpValue != 0)
                        {
                            txtManHrs.Text = dTmpValue.ToString("0.00");
                        }
                        //txtManHrs.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");


                        //Rate 
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Rate"]);
                        if (dTmpValue != 0)
                        {
                            txtRate.Text = dTmpValue.ToString("0.00");
                        }
                        //txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");



                        //Total
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Total"]);
                        dLabourAmount = dLabourAmount + dTmpValue;
                        if (dTmpValue != 0)
                        {
                            txtTotal.Text = dTmpValue.ToString("0.00");
                        }
                        //txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        //if (txtLabourCode.Text != "999999")
                        //if (txtLabourCode.Text.EndsWith("99") || txtLabourCode.Text == "999991")//999991 for Service Van Charges
                        //{
                        //    txtLabourDesc.Attributes.Add("readonly", "readonly");
                        //    txtManHrs.Attributes.Add("readonly", "readonly");
                        //}
                        //else
                        //{
                        //    txtLabourDesc.Attributes.Add("readonly", "readonly");
                        //    txtManHrs.Attributes.Add("readonly", "readonly");
                        //    txtRate.Attributes.Add("readonly", "readonly");
                        //    txtTotal.Attributes.Add("readonly", "readonly");
                        //}

                        txtOldAmount.Text = txtTotal.Text;

                        // job Code
                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Job_Code_ID"]);

                        // job Detail ID
                        txtJobcardDetID.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Jobcard_Det_ID"]);

                        // VECV Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["VECV_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Dealer Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Dealer_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Cust Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Cust_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        sRecordStatus = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Status"]);

                        txtLTax.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["TaxID"]).Trim();
                        txtLTax1.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax1"]).Trim();
                        txtLTax2.Text = Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Tax2"]).Trim();

                        TextBox txtDeduction = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox);
                        TextBox txtDeductionAmt = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeductionAmt") as TextBox);
                        TextBox txtAccAmount = (TextBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox);

                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Deduction_Percentage"]);
                        txtDeduction.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Deducted_Amount"]);
                        txtDeductionAmt.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtLabour.Rows[iRowCnt]["Accepted_Amount"]);
                        txtAccAmount.Text = dTmpValue.ToString("0.00");
                        dAccLabourAmount = dAccLabourAmount + dTmpValue;

                        idtRowCnt = idtRowCnt + 1;

                        if (Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["ChangeDetails_YN"]) == "Y")
                        {
                            LabourDetailsGrid.Rows[iRowCnt].Enabled = true;
                        }                  
                    }


                    LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");

                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                        LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");
                        LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");
                        if (OptShareType.SelectedValue == "1")
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        if (OptShareType.SelectedValue == "1")
                        {
                            LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                            LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");
                            LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                    }

                    LabourDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LabourDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

                    //New 
                    LinkButton lnkNew = (LinkButton)LabourDetailsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    lnkNew.Style.Add("display", "none");

                    Label lnkCancel = (Label)LabourDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //Delete 
                    CheckBox Chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "");

                    //If Labour Id  is not allocated
                    if (sLabourId == "0")
                    {
                        if (bShowLabour == true)
                        {
                            bShowLabour = false;
                            if (txtID.Text != "" && txtID.Text != "0" && DrpInvType.SelectedValue == "L") lnkSelectLabour.Style.Add("display", "");
                        }
                        txtLabourCode.Style.Add("display", "none");
                    }
                    else
                    {
                        //lblNewLabour.Style.Add("display", "none");
                    }
                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        LabourDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                    }
                    // Allow New To Last Row
                    if ((iRowCnt + 1) == LabourDetailsGrid.Rows.Count)
                    {
                        lnkNew.Style.Add("display", "");

                    }
                    if (bRecordIsOpen == false)
                    {
                        lnkNew.Style.Add("display", "none");
                        //Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        lnkSelectLabour.Style.Add("display", "none");
                    }
                    //drpJobCode.Enabled = false;
                    lnkNew.Style.Add("display", "none");
                    //Chk.Style.Add("display", "none");
                    lnkCancel.Style.Add("display", "none");                    
                }
                txtLabourAmount.Text = dLabourAmount.ToString("0.00");
                txtAccLabourAmount.Text = dAccLabourAmount.ToString("0.00");
                //lblLabourRecCnt.Text = Func.Common.sRowCntOfTable(dtLabour);
                double dClaimAmount = 0;

                dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                     Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                txtClaimAmt.Text = dClaimAmount.ToString("0.00");

                double dAccClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                     Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                txtAccClaimAmt.Text = dAccClaimAmount.ToString("0.00");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Labour  Grid
        private bool bFillDetailsFromLabourGrid()
        {
            string sStatus = "";
            dtLabour = (DataTable)Session["LabourDetails"];
            int iCntForDelete = 0;
            int iLabourID = 0;
            bool bValidate = true;
            double dManHrs = 0;
            double dRate = 0;
            string sLabCodeNo = "";
            string sLstTwoDigit = "";
            string sFirstFiveDigit = "";
            int iLbrDescriptionID = 0;
            string sLbrDescription = "";

            for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
            {
                //LabourID                
                iLabourID = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID") as TextBox).Text);

                if (iLabourID != 0)
                {
                    bValidate = true;

                    //ID
                    if (txtRefClaimID.Text != "")
                        dtLabour.Rows[iRowCnt]["ID"] = 0;

                    sLstTwoDigit = "";
                    sFirstFiveDigit = "";
                    iLbrDescriptionID = 0;
                    sLbrDescription = "";

                    // Labour ID
                    dtLabour.Rows[iRowCnt]["Labour_ID"] = iLabourID;

                    //Jobcard_Det_ID
                    dtLabour.Rows[iRowCnt]["Jobcard_Det_ID"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID") as TextBox).Text;

                    //Labour Code 
                    dtLabour.Rows[iRowCnt]["Labour_Code"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;

                    //Labour Description
                    dtLabour.Rows[iRowCnt]["Labour_Desc"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourDesc") as TextBox).Text;

                    TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");
                    DropDownList drpLbrDescription = (DropDownList)LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription");

                    if (iLabourID != 0)
                    {
                        sLstTwoDigit = txtLabourCode.Text.ToString().Substring(Func.Convert.iConvertToInt(txtLabourCode.Text.ToString().Length) - 2, 2);
                        sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                    }
                    if (sFirstFiveDigit == "MTIMI")
                    {
                        iLbrDescriptionID = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpLbrDescription") as DropDownList).SelectedValue);
                        sLbrDescription = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLbrDescription") as TextBox).Text.ToString();
                        //dtLabour.Rows[iRowCnt]["AddLbrDescriptionID"] = iLbrDescriptionID;

                        //Labor Additional Description
                        if (iLbrDescriptionID == Func.Convert.iConvertToInt("9999") && sLbrDescription != "")
                            dtLabour.Rows[iRowCnt]["Labour_Desc"] = sLbrDescription; 
                        else
                            dtLabour.Rows[iRowCnt]["Labour_Desc"] = drpLbrDescription.SelectedItem.ToString(); 
                    }

                    // Get ManHrs
                    dManHrs = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["ManHrs"] = dManHrs;

                    // Get Rate
                    dRate = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Rate"] = dRate;

                    dtLabour.Rows[iRowCnt]["TaxID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax") as TextBox).Text);

                    sLabCodeNo = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;
                    sLstTwoDigit = sLabCodeNo.ToString().Substring(Func.Convert.iConvertToInt(sLabCodeNo.ToString().Length) - 2, 2);
                    sFirstFiveDigit = sLabCodeNo.ToString().Substring(0, 5);

                    // Get Total                    
                    //dtLabour.Rows[iRowCnt]["Total"] = dManHrs * dRate;// Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Total"] = (sFirstFiveDigit == "MTIMI" || sFirstFiveDigit == "MTIOU" || sFirstFiveDigit == "MTICC") ? Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text) : dManHrs * dRate;
                    //dtLabour.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);


                    //Accepted ManHrs & Accepted Amount
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmTechnicalGoodwill && ValenmFormUsedFor != clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        dtLabour.Rows[iRowCnt]["Accepted_ManHrs"] = dManHrs;
                        dtLabour.Rows[iRowCnt]["Accepted_Amount"] = dtLabour.Rows[iRowCnt]["Total"];
                    }

                    //JobCode                
                    dtLabour.Rows[iRowCnt]["Job_Code_ID"] = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                    dtLabour.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                    dtLabour.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtLabour.Rows[iRowCnt]["Status"]);


                    dtLabour.Rows[iRowCnt]["Status"] = "S";// for Save      

                    CheckBox Chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtLabour.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete != 0)
            {
                if (iCntForDelete == LabourDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                }
            }
            return bValidate;
        }

        //To Get Total Labour Count
        private void CalculateLabourGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sLabourID = "";
                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Labour Description            
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                    sLabourID = txtLabourID.Text;
                    if (sLabourID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblLabourRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Click on Labour Select
        protected void lnkSelectLabour_Click(object sender, EventArgs e)
        {
            try
            {
                bFillDetailsFromLabourGrid();
                BindDataToLaborGrid(true, 0);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Lubricant Function

        // Create Row To Lubricant  Grid
        private void CreateNewRowToLubricantGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                int iMaxLubricantGridRowCount = 0;
                DataTable dtDefaultLubricant = new DataTable();
                int iRowCntStartFrom = 0;
                iMaxLubricantGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxLubricantGridRowCount"]);
                if (Session["LubricantDetails"] != null)
                {
                    dtDefaultLubricant = (DataTable)Session["LubricantDetails"];
                }
                else
                {
                    dtDefaultLubricant = dtLubricant;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultLubricant.Rows.Count == 0)
                    {
                        dtDefaultLubricant.Columns.Clear();
                        dtDefaultLubricant.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("GCR_Det_ID", typeof(int)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Lubricant_ID", typeof(int)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Lubricant_Description", typeof(string)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Qty", typeof(double)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("UOM", typeof(string)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Rate", typeof(string)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("VECV_Share", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Dealer_Share", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Cust_Share", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Deduction_Percentage", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Deducted_Amount", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Accepted_Amount", typeof(float)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultLubricant.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultLubricant.Rows.Count >= iMaxLubricantGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxLubricantGridRowCount;
                }

                iMaxLubricantGridRowCount = iMaxLubricantGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxLubricantGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultLubricant.NewRow();
                    dr["ID"] = 0;
                    dr["GCR_Det_ID"] = 0;
                    dr["Lubricant_ID"] = 0;
                    dr["Lubricant_Description"] = "";
                    dr["Qty"] = 0;
                    dr["UOM"] = "";
                    dr["Total"] = 0;
                    dr["Job_Code_ID"] = 0;
                    dr["VECV_Share"] = 999;
                    dr["Dealer_Share"] = 999;
                    dr["Cust_Share"] = 999;
                    dr["Accepted_Qty"] = 0;
                    dr["Deduction_Percentage"] = 0;
                    dr["Deducted_Amount"] = 0;
                    dr["Accepted_Amount"] = 0;
                    dr["Status"] = "N";
                    dr["ChangeDetails_YN"] = "N";
                    dtDefaultLubricant.Rows.Add(dr);
                    dtDefaultLubricant.AcceptChanges();
                }


            Bind: ;
                Session["LubricantDetails"] = dtDefaultLubricant;
                LubricantDetailsGrid.DataSource = dtDefaultLubricant;
                LubricantDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Bind Data to Lubricant  Grid
        private void BindDataToLubricantGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToLubricantGrid(iNoRowToAdd);
                    SetControlPropertyToLubricantGrid(bRecordIsOpen);
                }
                else
                {
                    LubricantDetailsGrid.DataSource = dtLubricant;
                    LubricantDetailsGrid.DataBind();
                    SetControlPropertyToLubricantGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Lubricant  Grid
        private void SetControlPropertyToLubricantGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                double dLubricantaAmount = 0;
                double dAccLubricantaAmount = 0;
                double dTmpValue = 0;
                int iModelGroupId = 0;
                bool bShowNewLubricant = true;
                iModelGroupId = Func.Convert.iConvertToInt(txtModelGroupID.Text);
                string sLubricantId = "";

                if (LubricantDetailsGrid.Rows.Count != 0)
                {
                    LubricantDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");// VEVC %
                    LubricantDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");// Dealer %
                    LubricantDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");// Cust %

                    LubricantDetailsGrid.HeaderRow.Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.HeaderRow.Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.HeaderRow.Cells[16].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.HeaderRow.Cells[17].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                }
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                {
                    if (LubricantDetailsGrid.Rows.Count != 0)
                    {
                        LubricantDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");// VEVC %
                        LubricantDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");// Dealer %
                        LubricantDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");// Cust %
                    }
                }
                for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Lubricant Type
                    DropDownList drpLubricantType = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubricantType");                   
                    
                    Func.Common.BindDataToCombo(drpLubricantType, clsCommon.ComboQueryType.LubricantType, 0, "");
                    
                    //Lubricant Description            
                    TextBox txtNewLubricantDesc = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtNewLubricantDesc");
                    txtNewLubricantDesc.Style.Add("display", "none");

                    // Add New Part 
                    LinkButton lnkSelectLubricant = (LinkButton)LubricantDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectLubricant");
                    lnkSelectLubricant.Attributes.Add("onclick", "return ShowSpWPFLubricant(this,'" + Func.Convert.sConvertToString(Location.iDealerId) + "');");

                    DropDownList drpLubLoc = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc");

                    DropDownList drpLubCap = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubCap");
                    
                    //Quantity
                    TextBox txtQty = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtQty");

                    //Rate
                    TextBox txtRate = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtRate");

                    //Total
                    TextBox txtTotal = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");

                    TextBox txtOTax = (TextBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax") as TextBox);
                    TextBox txtOTax1 = (TextBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax1") as TextBox);
                    TextBox txtOTax2 = (TextBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax2") as TextBox);

                    TextBox txtOBFRGST = (TextBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOBFRGST") as TextBox);

                    // Old Amount
                    TextBox txtOldAmount = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOldAmount");
                    txtOldAmount.Style.Add("display", "none");

                    //JobCode
                    DropDownList drpJobCode = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    //Jobcard Det ID
                    TextBox txtJobcardDetID = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID");
                    Label lblNo = (Label)LubricantDetailsGrid.Rows[iRowCnt].FindControl("lblNo");

                    sRecordStatus = "N";
                    if (idtRowCnt < dtLubricant.Rows.Count)
                    {

                        //Description 
                        txtNewLubricantDesc.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Lubricant_Description"].ToString().Trim());
                        
                        txtNewLubricantDesc.Style.Add("display", "none");
                        drpLubricantType.Style.Add("display", "");
                        drpLubricantType.Enabled = false;
                        //Lubricant Type
                        sLubricantId = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Lubricant_ID"]);
                        drpLubricantType.SelectedValue = sLubricantId;
                        if (idtRowCnt > 0) lblNo.Text = Func.Convert.sConvertToString(idtRowCnt);
                        //drpLubData.SelectedValue = sLubricantId;

                        //if (drpLubricantType.SelectedItem.Text == "NEW")// If Others Lubricant Type
                        //{
                        //    txtNewLubricantDesc.Style.Add("display", "");
                        //}

                        Func.Common.BindDataToCombo(drpLubLoc, clsCommon.ComboQueryType.LubricantTypeJobcard, Func.Convert.iConvertToInt(txtModelID.Text));
                        drpLubLoc.SelectedValue = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Jobcard_Det_ID"]);

                        Func.Common.BindDataToCombo(drpLubCap, clsCommon.ComboQueryType.LubricantCapacity, Func.Convert.iConvertToInt(txtModelID.Text));
                        drpLubCap.SelectedValue = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Jobcard_Det_ID"]);

                        drpLubLoc.Attributes.Add("onBlur", "SetLubCapasityOnLubLocationChange(this,'" + drpLubCap.ID + "')");
                        drpLubCap.Attributes.Add("disabled", "disabled");

                        //Set Qty
                        dTmpValue = Func.Convert.dConvertToDouble((dtLubricant.Rows[iRowCnt]["Qty"]));
                        if (dTmpValue != 0)
                        {
                            txtQty.Text = dTmpValue.ToString("0.00");
                        }


                        //Set UOM
                        (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtUOM") as TextBox).Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["UOM"]);

                        //Set Rate 
                        dTmpValue = Func.Convert.dConvertToDouble((dtLubricant.Rows[iRowCnt]["Rate"]));
                        if (dTmpValue != 0)
                        {
                            txtRate.Text = dTmpValue.ToString("0.00");
                        }

                        //Set Total 
                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Total"]);
                        if (dTmpValue != 0)
                        {
                            txtTotal.Text = dTmpValue.ToString("0.00");
                        }
                        txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        
                        dLubricantaAmount = dLubricantaAmount + Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Total"]);
                        txtOldAmount.Text = txtTotal.Text;

                        txtOTax.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["TaxID"]).Trim();
                        txtOTax1.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Tax1"]).Trim();
                        txtOTax2.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Tax2"]).Trim();
                        txtOBFRGST.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["BFRGST"]).Trim();

                        //Job Code 
                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Job_Code_ID"]);

                        // job Detail ID
                        txtJobcardDetID.Text = "0";

                        // VECV Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["VECV_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Dealer Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Dealer_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Cust Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Cust_Share"]);
                        if (dTmpValue != 999)
                        {
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        TextBox txtAccQty = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtAccQty") as TextBox);
                        TextBox txtDeduction = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox);
                        TextBox txtDeductionAmt = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeductionAmt") as TextBox);
                        TextBox txtAccAmount = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox);

                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Accepted_Qty"]);
                        txtAccQty.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Deduction_Percentage"]);
                        txtDeduction.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Deducted_Amount"]);
                        txtDeductionAmt.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Accepted_Amount"]);
                        txtAccAmount.Text = dTmpValue.ToString("0.00");
                        dAccLubricantaAmount = dAccLubricantaAmount + Func.Convert.dConvertToDouble(dtLubricant.Rows[iRowCnt]["Accepted_Amount"]);

                        sRecordStatus = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Status"]);
                        idtRowCnt = idtRowCnt + 1;
                    }
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");

                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");
                        if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                        {
                        }
                        else
                        {   
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                    }

                    LubricantDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

                    //New 
                    LinkButton lnkNew = (LinkButton)LubricantDetailsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    Label lnkCancel = (Label)LubricantDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //Delete 
                    CheckBox Chk = (CheckBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "");

                    lnkNew.Style.Add("display", "none");                    

                    lnkSelectLubricant.Style.Add("display", "none");

                    if (sLubricantId== "0")
                    {
                        drpLubricantType.Style.Add("display", "none");
                        if (bShowNewLubricant == true)
                        {
                            if (txtID.Text != "" && txtID.Text != "0" && DrpInvType.SelectedValue == "P") lnkSelectLubricant.Style.Add("display", "");
                            bShowNewLubricant = false;
                        }
                    }                    
                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        LubricantDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                    }

                    // Allow New To Last Row
                    if ((iRowCnt + 1) == LubricantDetailsGrid.Rows.Count)
                    {

                        lnkNew.Style.Add("display", "");
                    }
                    if (bRecordIsOpen == false)
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                        lnkSelectLubricant.Style.Add("display", "none");
                    }
                    
                    lnkNew.Style.Add("display", "none");
                    //Chk.Style.Add("display", "none");
                    lnkCancel.Style.Add("display", "none");                    
                    //drpJobCode.Enabled = false;                    
                    (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtUOM") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                }
                
                txtLubricantAmount.Text = dLubricantaAmount.ToString("0.00");
                txtAccLubricantAmount.Text = dAccLubricantaAmount.ToString("0.00");
                //lblLubricantRecCnt.Text = Func.Common.sRowCntOfTable(dtLubricant);
                double dClaimAmount = 0;

                dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                      Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                txtClaimAmt.Text = dClaimAmount.ToString("0.00");

                double dAccClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                   Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                txtAccClaimAmt.Text = dAccClaimAmount.ToString("0.00");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //Fill Details From Lubricant Grid
        private bool bFillDetailsFromLubricantGrid()
        {
            string sStatus = "";
            dtLubricant = (DataTable)Session["LubricantDetails"];
            int iCntForDelete = 0;
            int iLubricantID = 0;
            bool bValidate = true;
            string sLubDescription = "";

            for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
            {
                DropDownList drpLubricantType = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubricantType");
                //LubricantID                
                iLubricantID = Func.Convert.iConvertToInt(drpLubricantType.SelectedValue);

                if (iLubricantID != 0)
                {
                    bValidate = true;

                    //ID
                    if (txtRefClaimID.Text != "")
                        dtLubricant.Rows[iRowCnt]["ID"] = 0;

                    // Lubricant ID
                    dtLubricant.Rows[iRowCnt]["Lubricant_ID"] = iLubricantID;

                    //Lub Location ID
                    dtLubricant.Rows[iRowCnt]["Jobcard_Det_ID"] = Func.Convert.iConvertToInt((LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubLoc") as DropDownList).SelectedValue);                    

                    //Lubricant Description
                    sLubDescription = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtNewLubricantDesc") as TextBox).Text;
                    if (sLubDescription != "")
                    {
                        dtLubricant.Rows[iRowCnt]["Lubricant_Description"] = sLubDescription;
                    }
                    else
                    {
                        dtLubricant.Rows[iRowCnt]["Lubricant_Description"] = drpLubricantType.SelectedItem.Text;
                    }

                    // Get Unit
                    dtLubricant.Rows[iRowCnt]["UOM"] = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtUOM") as TextBox).Text;

                    // Get Qty
                    dtLubricant.Rows[iRowCnt]["Qty"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtQty") as TextBox).Text);

                    //Get Rate
                    dtLubricant.Rows[iRowCnt]["Rate"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text);

                    // Get Total
                    dtLubricant.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);

                    dtLubricant.Rows[iRowCnt]["TaxID"] = Func.Convert.iConvertToInt((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax") as TextBox).Text);

                    dtLubricant.Rows[iRowCnt]["BFRGST"] = Func.Convert.sConvertToString((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOBFRGST") as TextBox).Text);

                    //Get Accepted_Qty & Accepted Amount
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmTechnicalGoodwill && ValenmFormUsedFor != clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        dtLubricant.Rows[iRowCnt]["Accepted_Qty"] = dtLubricant.Rows[iRowCnt]["Qty"];
                        dtLubricant.Rows[iRowCnt]["Accepted_Amount"] = dtLubricant.Rows[iRowCnt]["Total"];
                    }
                    //Get JobCode
                    //JobCode               
                    dtLubricant.Rows[iRowCnt]["Job_Code_ID"] = Func.Convert.iConvertToInt((LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                    dtLubricant.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                    dtLubricant.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                    dtLubricant.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["Status"]);

                    dtLubricant.Rows[iRowCnt]["Status"] = "S";// for Save      

                    CheckBox Chk = (CheckBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtLubricant.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }
            if (iCntForDelete != 0)
            {
                if (iCntForDelete == LubricantDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                }
            }
            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the lubricant details.');</script>");
            }
            return bValidate;
        }

        //To Get Total Lubricant Count
        private void CalculateLubricantGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sLubricantID = "";
                for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Lubricant Description            
                    DropDownList drpLubricantType = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubricantType");

                    sLubricantID = drpLubricantType.SelectedValue;
                    if (sLubricantID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblLubricantRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void lnkSelectLubricant_Click(object sender, EventArgs e)
        {
            try
            {
                bFillDetailsFromLubricantGrid();
                BindDataToLubricantGrid(true, 0);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Sublet Function
        // Create Row To Sublet  Grid
        private void CreateNewRowToSubletGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultSublet = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxSubletGridRowCount = 0;
                iMaxSubletGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxSubletGridRowCount"]);
                if (Session["SubletDetails"] != null)
                {
                    dtDefaultSublet = (DataTable)Session["SubletDetails"];
                }
                else
                {
                    dtDefaultSublet = dtSublet;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultSublet.Rows.Count == 0)
                    {
                        dtDefaultSublet.Columns.Clear();
                        dtDefaultSublet.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultSublet.Columns.Add(new DataColumn("GCR_Det_ID", typeof(int)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Labour_ID", typeof(int)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
                        dtDefaultSublet.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Rate", typeof(double)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Total", typeof(double)));
                        //dtDefaultSublet.Columns.Add(new DataColumn("Sublet_ID", typeof(int)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Sublet_Desc", typeof(string)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultSublet.Columns.Add(new DataColumn("VECV_Share", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Dealer_Share", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Cust_Share", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Deduction_Percentage", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Deducted_Amount", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Accepted_Amount", typeof(float)));
                        dtDefaultSublet.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultSublet.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultSublet.Rows.Count >= iMaxSubletGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxSubletGridRowCount;
                }

                iMaxSubletGridRowCount = iMaxSubletGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxSubletGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultSublet.NewRow();
                    dr["ID"] = 0;
                    dr["GCR_Det_ID"] = 0;
                    dr["Labour_ID"] = Func.Convert.iConvertToInt(HdnSubletLabID.Value);
                    dr["Labour_Code"] = Func.Convert.sConvertToString(HdnSubletLabCode.Value) ;
                    dr["ManHrs"] = 0;
                    dr["Rate"] = Func.Convert.dConvertToDouble(HdnSubletLabRate.Value);
                    //dr["Sublet_ID"] = 0;
                    dr["Sublet_Desc"] = "";
                    dr["Total"] = 0;
                    dr["Job_Code_ID"] = 0;
                    dr["VECV_Share"] = 999;
                    dr["Dealer_Share"] = 999;
                    dr["Cust_Share"] = 999;
                    dr["Deduction_Percentage"] = 0;
                    dr["Deducted_Amount"] = 0;
                    dr["Accepted_Amount"] = 0;
                    dr["Status"] = "N";
                    dr["ChangeDetails_YN"] = "N";
                    dtDefaultSublet.Rows.Add(dr);
                    dtDefaultSublet.AcceptChanges();
                }
            Bind: ;
                Session["SubletDetails"] = dtDefaultSublet;
                SubletDetailsGrid.DataSource = dtDefaultSublet;
                SubletDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Bind Data to Sublet Grid
        private void BindDataToSubletGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToSubletGrid(iNoRowToAdd);
                    SetControlPropertyToSubletGrid(bRecordIsOpen);
                }
                else
                {
                    SubletDetailsGrid.DataSource = dtSublet;
                    SubletDetailsGrid.DataBind();
                    SetControlPropertyToSubletGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // Set Control property To Sublet Grid
        private void SetControlPropertyToSubletGrid(bool bRecordIsOpen)
        {
            try
            {
                string sRecordStatus = "";
                int idtRowCnt = 0;
                double dSubletAmount = 0;
                double dAccSubletAmount = 0;
                double dTmpValue = 0;

                if (SubletDetailsGrid.Rows.Count != 0)
                {
                    SubletDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");
                    SubletDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");
                    SubletDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                }
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest || ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                {
                    if (SubletDetailsGrid.Rows.Count != 0)
                    {
                        SubletDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");
                        SubletDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");
                        SubletDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");
                    }
                }

                SubletDetailsGrid.HeaderRow.Cells[13].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                SubletDetailsGrid.HeaderRow.Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                SubletDetailsGrid.HeaderRow.Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                                
                for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //SubletNo Or NewSublet
                    //TextBox txtLabourCode = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");
                    TextBox txtLabourCode = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode");
                    

                    //Sublet Code
                    //DropDownList drpSubletCode = (DropDownList)SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSubletCode");
                    ////Func.Common.BindDataToCombo(drpSubletCode, clsCommon.ComboQueryType.DealerSubletCode, 0, " And ( labour_code like '44%' OR labour_code like '33%')");

                    //Func.Common.BindDataToCombo(drpSubletCode, clsCommon.ComboQueryType.DealerSubletCode, 0, " And ( labour_code like 'MTIOU%') and M_LabourCode.LabGroupID in(select Id from M_LabourGroupMaster "
                    //   + " where M_LabourGroupMaster.Basic_Mod_Cat_ID in (select Mod_Cat_ID_Basic from M_ModelMaster where ID="+ txtModelID.Text + "))");
                    
                    ////Sublet 
                    ////DropDownList drpSublet = (DropDownList)SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSublet");
                    ////Func.Common.BindDataToCombo(drpSublet, clsCommon.ComboQueryType.DealerSublet, 0);

                    ////Sublet Description            
                    TextBox txtNewSubletDesc = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtNewSubletDesc");
                    //txtNewSubletDesc.Style.Add("display", "none");

                    //// Add New Sublet  
                    //ListItem lstitm = new ListItem("NEW", "9999");
                    //drpSublet.Items.Add(lstitm);
                    //drpSublet.Attributes.Add("onChange", "OnComboValueChange(this,'" + txtNewSubletDesc.ID + "')");


                    //JobCode
                    DropDownList drpJobCode = (DropDownList)SubletDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode");
                    Func.Common.BindDataToCombo(drpJobCode, clsCommon.ComboQueryType.JobCode, 0);

                    //Jobcard Det ID
                    TextBox txtJobcardDetID = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID");

                    TextBox txtTotal = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");

                    TextBox txtSTax = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax") as TextBox);
                    TextBox txtSTax1 = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax1") as TextBox);
                    TextBox txtSTax2 = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax2") as TextBox); 

                    sRecordStatus = "N";

                    if (idtRowCnt < dtSublet.Rows.Count)
                    {

                        //Labour ID                            
                        //(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID") as TextBox).Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Labour_ID"]);

                        //Sublet /Labour Code
                        txtLabourCode.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Labour_Code"]);
                        //drpSubletCode.SelectedValue = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Labour_ID"]);

                        //Sublet /Labour Description

                        txtNewSubletDesc.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Sublet_Desc"].ToString().Trim());

                        //SubletID                            
                        //drpSublet.SelectedValue = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Sublet_ID"]);

                        //drpSublet.Style.Add("display", "");
                        txtNewSubletDesc.Style.Add("display", "");

                        //if (bRecordIsOpen == true && drpSublet.SelectedValue == "9999")
                        //    drpSublet.Style.Add("display", "none");
                        //else //if (bRecordIsOpen == true && drpSublet.SelectedValue != "9999" )
                        //    txtNewSubletDesc.Style.Add("display", "none");
                        ////else if (bRecordIsOpen == false && txtNewSubletDesc.Text.ToString().Length > 0)
                        ////    drpSublet.Style.Add("display", "none");
                        ////else if (bRecordIsOpen == false && txtNewSubletDesc.Text.ToString().Length == 0)
                        ////    txtNewSubletDesc.Style.Add("display", "none");


                        ////if (drpSublet.SelectedValue == "9999" && txtNewSubletDesc.Text.ToString().Length > 0)
                        ////    drpSublet.Style.Add("display", "none");




                        // Man Hrs
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Text = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["ManHrs"]).ToString("0.00");
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");


                        //Rate
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Text = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Rate"]).ToString("0.00");
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtRate") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        //Total
                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Total"]);
                        dSubletAmount = dSubletAmount + dTmpValue;
                        if (dTmpValue != 0)
                        {
                            txtTotal.Text = dTmpValue.ToString("0.00");
                        }
                        //(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

                        // Old Amount                
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtOldAmount") as TextBox).Text = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Total"]).ToString("0.00");


                        drpJobCode.SelectedValue = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Job_Code_ID"]);

                        // job Detail ID
                        txtJobcardDetID.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Jobcard_Det_ID"]);

                        sRecordStatus = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Status"]);

                        txtSTax.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["TaxID"]).Trim();
                        txtSTax1.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Tax1"]).Trim();
                        txtSTax2.Text = Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Tax2"]).Trim();

                        // VECV Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["VECV_Share"]);
                        if (dTmpValue != 999)
                        {
                            (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Dealer Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Dealer_Share"]);
                        if (dTmpValue != 999)
                        {
                            (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        //Cust Share
                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Cust_Share"]);
                        if (dTmpValue != 999)
                        {
                            (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text = dTmpValue.ToString("0.00");
                        }

                        TextBox txtDeduction = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox);
                        TextBox txtDeductionAmt = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeductionAmt") as TextBox);
                        TextBox txtAccAmount = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox);

                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Deduction_Percentage"]);
                        txtDeduction.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Deducted_Amount"]);
                        txtDeductionAmt.Text = dTmpValue.ToString("0.00");

                        dTmpValue = Func.Convert.dConvertToDouble(dtSublet.Rows[iRowCnt]["Accepted_Amount"]);
                        txtAccAmount.Text = dTmpValue.ToString("0.00");
                        dAccSubletAmount = dAccSubletAmount + dTmpValue;

                        idtRowCnt = idtRowCnt + 1;
                    }

                    SubletDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");

                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        SubletDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmTechnicalGoodwill || ValenmFormUsedFor == clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        SubletDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    }

                    SubletDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

                    //New 
                    LinkButton lnkNew = (LinkButton)SubletDetailsGrid.Rows[iRowCnt].FindControl("lnkNew");
                    lnkNew.Style.Add("display", "none");

                    Label lnkCancel = (Label)SubletDetailsGrid.Rows[iRowCnt].FindControl("lblCancel");

                    //Delete 
                    CheckBox Chk = (CheckBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "");

                    if (sRecordStatus == "D")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus == "E")
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "");
                        lnkCancel.Style.Add("display", "none");
                    }
                    // Allow New To Last Row
                    if ((iRowCnt + 1) == SubletDetailsGrid.Rows.Count)
                    {
                        lnkNew.Style.Add("display", "");
                    }
                    if (bRecordIsOpen == false)
                    {
                        lnkNew.Style.Add("display", "none");
                        Chk.Style.Add("display", "none");
                        lnkCancel.Style.Add("display", "none");
                    }

                    //lnkNew.Style.Add("display", "none");
                    //Chk.Style.Add("display", "none");
                    lnkCancel.Style.Add("display", "none");                    
                    //drpSubletCode.Enabled = false;
                    //drpJobCode.Enabled = false;
                    txtTotal.Enabled = false;
                    txtNewSubletDesc.Enabled = false;

                    if (txtID.Text != "" && txtID.Text != "0" && DrpInvType.SelectedValue == "L") txtTotal.Enabled = true;
                    if (txtID.Text != "" && txtID.Text != "0" && DrpInvType.SelectedValue == "L") txtNewSubletDesc.Enabled = true;
                }

                txtSubletAmount.Text = dSubletAmount.ToString("0.00");
                txtAccSubletAmount.Text = dAccSubletAmount.ToString("0.00");
                double dClaimAmount = 0;                

                dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                     Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                txtClaimAmt.Text = dClaimAmount.ToString("0.00");

                double dAccClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                   Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                txtAccClaimAmt.Text = dAccClaimAmount.ToString("0.00");
                //lblSubletRecCnt.Text = Func.Common.sRowCntOfTable(dtSublet);        
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Fill Details From Sublet Grid
        private bool bFillDetailsFromSubletGrid()
        {
            string sStatus = "";
            dtSublet = (DataTable)Session["SubletDetails"];
            int iCntForDelete = 0;
            int iLaborID = 0;
            bool bValidate = true;
            double dManHrs = 0;
            double dRate = 0;
            double dTotal = 0;
            string sSubletDescription = "";

            for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
            {
                ////DropDownList drpSublet = (DropDownList)(SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSublet"));
                //DropDownList drpSubletCode = (DropDownList)(SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSubletCode"));

                //// Sublet ID
                ////iSubletCatgID = Func.Convert.iConvertToInt(drpSublet.SelectedValue);
                //iLaborID = Func.Convert.iConvertToInt(drpSubletCode.SelectedValue);

                TextBox txtLabourID = (TextBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID"));
                iLaborID = Func.Convert.iConvertToInt(txtLabourID.Text);
                sSubletDescription = (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtNewSubletDesc") as TextBox).Text;
                dTotal = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);
                 //if (iLaborID != 0 )
                if ( sSubletDescription != "" && dTotal > 0)
                {
                    bValidate = true;

                    //ID
                    if (txtRefClaimID.Text != "")
                        dtSublet.Rows[iRowCnt]["ID"] = 0;

                    //Set Sublet Catg ID
                    //dtSublet.Rows[iRowCnt]["Sublet_ID"] = Func.Convert.iConvertToInt(drpSublet.SelectedValue);//iSubletCatgID;
                    //dtSublet.Rows[iRowCnt]["Sublet_ID"] = Func.Convert.iConvertToInt(drpSubletCode.SelectedValue);//iSubletCatgID;                    
                    // Labour ID
                    dtSublet.Rows[iRowCnt]["Labour_ID"] =Func.Convert.iConvertToInt(HdnSubletLabID.Value);

                    //Jobcard_Det_ID
                    dtSublet.Rows[iRowCnt]["Jobcard_Det_ID"] = Func.Convert.iConvertToInt((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtJobcardDetID") as TextBox).Text);

                    //Labour Code 
                    //dtSublet.Rows[iRowCnt]["Labour_Code"] = drpSubletCode.SelectedItem.Text;//(SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;
                    //dtSublet.Rows[iRowCnt]["Labour_Code"] = (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourCode") as TextBox).Text;
                    dtSublet.Rows[iRowCnt]["Labour_Code"] = Func.Convert.sConvertToString(HdnSubletLabCode.Value);

                    // Get ManHrs
                    //dManHrs = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtManHrs") as TextBox).Text);
                    //dtSublet.Rows[iRowCnt]["ManHrs"] = dManHrs;

                    dtSublet.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text);
                    dtSublet.Rows[iRowCnt]["Rate"] = Func.Convert.dConvertToDouble(HdnSubletLabRate.Value); ;

                    dtSublet.Rows[iRowCnt]["ManHrs"] = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal") as TextBox).Text)/Func.Convert.dConvertToDouble(HdnSubletLabRate.Value));

                    dtSublet.Rows[iRowCnt]["TaxID"] = Func.Convert.iConvertToInt((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax") as TextBox).Text);                  

                    //Sublet Description                    
                    dtSublet.Rows[iRowCnt]["Sublet_Desc"] = sSubletDescription;
                    
                    //Accepted ManHrs & Accepted Amount
                    if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmTechnicalGoodwill && ValenmFormUsedFor != clsWarranty.enmClaimType.enmCommercialGoodwill)
                    {
                        dtSublet.Rows[iRowCnt]["Accepted_Amount"] = dtSublet.Rows[iRowCnt]["Total"];
                    }

                    //JobCode


                    dtSublet.Rows[iRowCnt]["Job_Code_ID"] = Func.Convert.iConvertToInt((SubletDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);

                    dtSublet.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                    dtSublet.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                    dtSublet.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                    // Record Status
                    //sStatus =Func.Convert.sConvertToString(dtSublet.Rows[iRowCnt]["Status"]);

                    dtSublet.Rows[iRowCnt]["Status"] = "S";// for Save      

                    CheckBox Chk = (CheckBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    if (Chk.Checked == true)
                    {
                        dtSublet.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                }
            }

            if (iCntForDelete != 0)
            {
                if (iCntForDelete == SubletDetailsGrid.Rows.Count)
                {
                    bValidate = false;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter atleast one sublet details.');</script>");
                }
            }
            return bValidate;
        }

        //To Get Total Sublet Count
        private void CalculateSubletGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sSubletID = "";
                for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Sublet Description  
                    //Sujata 20012011
                    //TextBox txtSubletID = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
                    TextBox txtSubletID = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtTotal");
                    //Sujata 20012011

                    sSubletID = txtSubletID.Text;
                    //Sujata 20012011
                    //if (sSubletID != "0")
                    if (sSubletID != "")
                    //Sujata 20012011
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblSubletRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region Job Function
        // Create Row To Part Grid
        private void CreateNewRowToJobGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultJob = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxJobGridRowCount = 0;
                iMaxJobGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxJobGridRowCount"]);
                if (Session["JobDetails"] != null)
                {
                    dtDefaultJob = (DataTable)Session["JobDetails"];
                }
                else
                {
                    dtDefaultJob = dtJob;
                }

                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultJob.Rows.Count == 0)
                    {
                        dtDefaultJob.Columns.Clear();
                        dtDefaultJob.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_No", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Part_Name", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("job_code", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Culprit_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Defect_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Technical_ID", typeof(int)));
                        dtDefaultJob.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDefaultJob.Columns.Add(new DataColumn("Warratable_YN", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultJob.Rows.Count >= iMaxJobGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxJobGridRowCount;
                }

                iMaxJobGridRowCount = iMaxJobGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxJobGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultJob.NewRow();
                    dr["ID"] = 0;
                    dr["Part_No_ID"] = 0;
                    dr["Part_No"] = "";
                    dr["Part_Name"] = "";
                    dr["Job_Code_ID"] = 0;
                    dr["job_code"] = "";
                    dr["Culprit_ID"] = 0;
                    dr["Defect_ID"] = 0;
                    dr["Technical_ID"] = 0;
                    dr["Status"] = "N";
                    dr["Warratable_YN"] = "N";
                    dtDefaultJob.Rows.Add(dr);
                    dtDefaultJob.AcceptChanges();
                }

            Bind: ;
                Session["JobDetails"] = dtDefaultJob;
                JobDetailsGrid.DataSource = dtDefaultJob;
                JobDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Job Grid
        private bool bFillDetailsFromJobGrid(string sJbCdConfirm)
        {
            string sStatus = "";
            dtJob = (DataTable)Session["JobDetails"];
            int iCntForDelete = 0;
            //int iJobCodeID = 0;
            int iPartID = 0;
            bool bValidate = false;
            string sWarrantablePart = "";
            for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
            {
                //iJobCodeID = Func.Convert.iConvertToInt((JobDetailsGrid.Rows[iRowCnt].FindControl("drpJobCode") as DropDownList).SelectedValue);
                iPartID = Func.Convert.iConvertToInt((JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                ////ID
                //if (txtRefClaimID.Text != "")
                //dtJob.Rows[iRowCnt]["ID"] = 0;

                //JobCode                
                dtJob.Rows[iRowCnt]["Job_Code_ID"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID") as TextBox).Text;

                //JobCode                
                dtJob.Rows[iRowCnt]["job_code"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode") as Label).Text;

                //PartID                               
                bValidate = true;
                dtJob.Rows[iRowCnt]["Part_No_ID"] = iPartID;

                //Part No
                dtJob.Rows[iRowCnt]["Part_No"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo") as TextBox).Text;

                //Part Name
                dtJob.Rows[iRowCnt]["Part_Name"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text;

                //culprit Code  

                DropDownList drpCulpritCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpCulpritCode");
                dtJob.Rows[iRowCnt]["Culprit_ID"] = Func.Convert.iConvertToInt(drpCulpritCode.SelectedValue);

                //Defect Code
                DropDownList drpDefectCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpDefectCode");
                dtJob.Rows[iRowCnt]["Defect_ID"] = Func.Convert.iConvertToInt(drpDefectCode.SelectedValue);

                //Technical Code
                DropDownList drpTechnicalCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpTechnicalCode");
                dtJob.Rows[iRowCnt]["Technical_ID"] = Func.Convert.iConvertToInt(drpTechnicalCode.SelectedValue);
                sWarrantablePart = Func.Convert.sConvertToString((JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart") as TextBox).Text);

                //Warrantable Part 
                dtJob.Rows[iRowCnt]["Warratable_YN"] = (JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart") as TextBox).Text;

                TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");
                TextBox txtWarrJobCode = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrJobCode");

                // Record Status
                dtJob.Rows[iRowCnt]["Status"] = "S";

                if (txtWarrJobCode.Text == "Y" && sJbCdConfirm == "Y" && (iPartID == 0 || Func.Convert.iConvertToInt(drpCulpritCode.SelectedValue) == 0 || Func.Convert.iConvertToInt(drpDefectCode.SelectedValue) == 0))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
                    return false;
                }
                TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");
                if (txtWarrJobCode.Text == "Y" && sJbCdConfirm == "Y" && (txtPCRHDRID.Text.Trim() == "" || txtPCRHDRID.Text.Trim() == "0"))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Create PCR For All JobCode.');</script>");
                    return false;
                }

                //if (sWarrantablePart.ToUpper() == "N")
                //{
                //    if (dtJob.Rows[iRowCnt]["Technical_ID"].ToString() == "0")
                //    {
                //        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Select The Technical Code.');</script>");
                //        return false;
                //    }
                //}

            }

            if (iCntForDelete == JobDetailsGrid.Rows.Count)
            {
                bValidate = false;
            }

            Session["JobDetails"] = dtJob;

            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
            }
            return bValidate;
        }

        protected void lnkJbSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
                //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + hdnSelectedPartID.Value + ".');</script>");
                //bFillDetailsFromPartGrid();
                //BindDataToPartGrid(true, 0);
                bFillDetailsFromJobGrid("N");
                BindDataToJobGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Set Control property To Job Grid
        private void SetControlPropertyToJobGrid()
        {
            try
            {
                string sRecordStatus = "N";
                int idtRowCnt = 0;
                string sPartName = "";
                string sPartId = "";
                //JobDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Job ID
                    TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID");

                    //txtJobCodeDtlID
                    TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");

                    //txtWarrJobCode
                    TextBox txtWarrJobCode = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrJobCode");

                    //Job Code 
                    Label lblJobCode = (Label)JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode");

                    // Culprit  Part Control                
                    LinkButton lnkJbSelectPart = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkJbSelectPart");
                    lnkJbSelectPart.Attributes.Add("onclick", "return ShowPartMaster(this,'" + Func.Convert.sConvertToString(Location.iDealerId.ToString()) + "','" + lblJobCode.Text + "','N');");

                    //Part No
                    TextBox txtPartNo = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    //culprit Code
                    DropDownList drpCulpritCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpCulpritCode");
                    //Func.Common.BindDataToCombo(drpCulpritCode, clsCommon.ComboQueryType.CulpritCode, 0);
                    Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0);

                    //Defect Code
                    DropDownList drpDefectCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpDefectCode");
                    //Func.Common.BindDataToCombo(drpDefectCode, clsCommon.ComboQueryType.DefectCode, 0);                    
                    Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0);

                    //Technical Code
                    DropDownList drpTechnicalCode = (DropDownList)JobDetailsGrid.Rows[iRowCnt].FindControl("drpTechnicalCode");
                    Func.Common.BindDataToCombo(drpTechnicalCode, clsCommon.ComboQueryType.TechnicalCode, 0);
                    drpTechnicalCode.Attributes.Add("disabled", "disabled");

                    //txt warrantable Part 
                    TextBox txtWarrantablePart = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtWarrantablePart");

                    //link To PCR
                    LinkButton lnkSelectJobDtl = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectJobDtl");

                    //PCR HDR ID
                    TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");

                    sRecordStatus = "N";
                    sPartId = "0";

                    // Get Value from Culprit details table
                    if (idtRowCnt < dtJob.Rows.Count)
                    {
                        //Jobcode defined for Warranty or not
                        txtWarrJobCode.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["WarrJobcode"]);

                        //JobCode Dtls saved or not
                        txtJobCodeDtlSaved.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["JobCodeDtlSaved"]);

                        //Job Code ID
                        txtJobID.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Job_Code_ID"]);

                        //Jobcode
                        lblJobCode.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["job_code"]);

                        //Part ID       
                        sPartId = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_No_ID"]);
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text = sPartId;

                        txtPCRHDRID.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["PCRHDRID"]);

                        lnkSelectJobDtl.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + Location.iDealerId.ToString() + "','" + txtJobID.Text + "','" + txtPCRHDRID.Text.ToString() + "','" + txtJobCodeDtlSaved.Text.Trim() + "')");

                        lnkSelectJobDtl.Style.Add("display", (txtJobCodeDtlSaved.Text.Trim() == "N" || txtJobCodeDtlSaved.Text.Trim() == "") ? "none" : "");

                        //Part No
                        txtPartNo.Text = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_No"]);

                        //Part Name
                        sPartName = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Part_Name"]);
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).Text = sPartName;
                        (JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartName") as TextBox).ToolTip = sPartName;

                        sRecordStatus = Func.Convert.sConvertToString(dtJob.Rows[iRowCnt]["Status"]);
                        
                        Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Culprit_ID"]) + ")");
                        Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Defect_ID"]) + ")");

                        drpCulpritCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Culprit_ID"]);
                        drpDefectCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Defect_ID"]);
                        drpTechnicalCode.SelectedValue = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Technical_ID"]);
                        txtWarrantablePart.Text = Func.Convert.sConvertToString(dtJob.Rows[idtRowCnt]["Warratable_YN"]);

                        if (txtWarrantablePart.Text == "N")
                        {
                            drpTechnicalCode.Attributes.Remove("disabled");
                        }                        

                        idtRowCnt = idtRowCnt + 1;
                    }

                    //lnkCancel.Style.Add("display", "none");
                    if (sRecordStatus == "U")
                    {
                        txtPartNo.ReadOnly = true;

                    }

                    //If Part Id  is not allocated
                    if (sPartId == "0")
                    {
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                        //lblNewPart.Style.Add("display", "");
                        lnkJbSelectPart.Style.Add("display", "");

                    if (Session["DepartmentID"].ToString() == "6")
                    {
                        lnkJbSelectPart.Style.Add("display", "none");
                        drpDefectCode.Enabled = false;
                        drpCulpritCode.Enabled = false;
                        drpTechnicalCode.Enabled = false;
                    }
                    drpDefectCode.Enabled = (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") ? false : true;
                    drpCulpritCode.Enabled = (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") ? false : true;
                    lnkJbSelectPart.Style.Add("display", (hdnClaimConfirm.Value == "N" || hdnCSMSubmit.Value == "Y") ? "none" : "");
                }
                //lblJobRecCnt.Text = Func.Common.sRowCntOfTable(dtJob);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
               
        //To Get Total Job Count
        private void CalculateJobGridCnt()
        {
            try
            {
                int iRecCnt = 0;// To calculate actual selectes record count.
                string sJobID = "";
                for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Job Description            
                    TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartID");

                    sJobID = txtJobID.Text;
                    if (sJobID != "0")
                    {
                        iRecCnt = iRecCnt + 1;

                    }
                }
                lblJobRecCnt.Text = iRecCnt.ToString();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void btnJobSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                string sJbCdConfirm = "N";
                if (bFillDetailsFromJobGrid(sJbCdConfirm) == true)
                {
                    if (ObjWarranty.bSaveJobcodeDetails(Func.Convert.iConvertToInt(txtID.Text), dtJob, sJbCdConfirm) == true)
                    {
                        for (int iRowCnt = 0; iRowCnt < JobDetailsGrid.Rows.Count; iRowCnt++)
                        {
                            TextBox txtJobID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobID");

                            TextBox txtJobCodeDtlSaved = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtJobCodeDtlSaved");

                            TextBox txtPartNo = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPartNo");

                            Label lblJobCode = (Label)JobDetailsGrid.Rows[iRowCnt].FindControl("lblJobCode");

                            TextBox txtPCRHDRID = (TextBox)JobDetailsGrid.Rows[iRowCnt].FindControl("txtPCRHDRID");

                            LinkButton lnkSelectJobDtl = (LinkButton)JobDetailsGrid.Rows[iRowCnt].FindControl("lnkSelectJobDtl");

                            if (txtPartNo.Text.Trim() != "" && txtPartNo.Text.Trim() != "0") txtJobCodeDtlSaved.Text = "Y";

                            lnkSelectJobDtl.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + Location.iDealerId.ToString() + "','" + txtJobID.Text + "','" + txtPCRHDRID.Text.ToString() + "','" + txtJobCodeDtlSaved.Text.Trim() + "')");

                            lnkSelectJobDtl.Style.Add("display", (txtJobCodeDtlSaved.Text.Trim() == "N" || txtJobCodeDtlSaved.Text.Trim() == "") ? "none" : "");
                        }
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Saved JobCode Details.');</script>");
                    }
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnJobConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                string sJbCdConfirm = "Y";
                if (bFillDetailsFromJobGrid(sJbCdConfirm) == true)
                {
                    if (ObjWarranty.bSaveJobcodeDetails(Func.Convert.iConvertToInt(txtID.Text), dtJob, sJbCdConfirm) == true)                    
                    {
                        btnJobSave.Visible = false;
                        btnJobConfirm.Visible = false;                        
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Warranty Claim Submitted.');</script>");
                    }
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        //Bind Data to Job Grid
        private void BindDataToJobGrid()
        {
            try
            {
                JobDetailsGrid.DataSource = dtJob;
                JobDetailsGrid.DataBind();
                SetControlPropertyToJobGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void lnkSelectJobDtl_Click(object sender, EventArgs e)
        {
            try
            {
                //GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region File Attachment Functions
        // To Save the attach Document 
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            DataRow dr;
            int iRecorFound = 0;
            for (int i = 0; i < uploads.Count; i++)
            {
                try
                {
                    //Retrieving the fullpath of the File.
                    sSourceFileName = Path.GetFileName(uploads[i].FileName);

                    if (sSourceFileName.Trim() != "")
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

                        string[] splClaimNo = txtClaimNo.Text.Split('/');
                        if (splClaimNo.Length > 1)
                        {
                            txtClaimNo.Text = "";
                            for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                                txtClaimNo.Text = txtClaimNo.Text + splClaimNo[iCnt];
                        }


                        //dr["File_Names"] = sSourceFileName;
                        //if (ValenmFormUsedFor != clsWarranty.enmClaimType.enmNormal)
                        if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                            dr["File_Names"] = Func.Convert.sConvertToString(sDealerId) + "_R_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName;
                        else
                            dr["File_Names"] = Func.Convert.sConvertToString(sDealerId) + "_C_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName;

                        dr["UserId"] = Func.Convert.sConvertToString(sDealerId);
                        dr["Status"] = "S";
                        dr["CreatedUserRole"] = 6;
                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        //Saving it in temperory Directory.               

                        if (!System.IO.Directory.Exists(sPath + "Claim Request"))
                            System.IO.Directory.CreateDirectory(sPath + "Claim Request");
                        if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                        {
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Claim Request");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }
                            uploads[i].SaveAs((sPath + "Claim Request" + "\\" + Func.Convert.sConvertToString(sDealerId) + "_R_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName + ""));
                        }
                        else
                        {
                            DirectoryInfo destination = new DirectoryInfo(sPath + "Warranty Claim");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }
                            uploads[i].SaveAs((sPath + "Warranty Claim" + "\\" + Func.Convert.sConvertToString(sDealerId) + "_C_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName + ""));
                        }
                    }

                }

                catch (Exception ex)
                {
                    return false;
                    Func.Common.ProcessUnhandledException(ex);
                }

            }
            return true;
        }

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
            dtFileAttach.Columns.Add(new DataColumn("CreatedUserRole", typeof(int)));
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
                    if (txtRefClaimID.Text != "")
                        dr["ID"] = 0;
                    else
                        dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                    dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                    dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                    dr["UserId"] = Func.Convert.iConvertToInt(sDealerId);

                    ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                    if (ChkForDelete.Checked == true)
                    {
                        dr["Status"] = "D";
                    }
                    else
                    {
                        dr["Status"] = "S";
                    }
                    dr["CreatedUserRole"] = 6;
                    dtFileAttach.Rows.Add(dr);
                    dtFileAttach.AcceptChanges();
                }
            }
        }

        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }
        #endregion

        private void ClearAllSession()
        {
            Session["ComplaintsDetails"] = null;
            Session["InvestigationDetails"] = null;
            Session["PartDetails"] = null;
            Session["LabourDetails"] = null;
            Session["LubricantDetails"] = null;
            Session["SubletDetails"] = null;
            Session["JobDetails"] = null;
        }

        //To Get data From Request
        protected void lblSelectRequest_Click(object sender, EventArgs e)
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                DataSet ds = new DataSet();
                int iRequestD = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;
                SetFormControlAsPerClaimType();
                if (iRequestD != 0)
                {
                    //drpClaimType.Style.Add("display", "none");                                    
                    GetDataAndDisplay(iRequestD);
                    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "EWC", Location.iDealerId);
                    txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                    txtClaimRevNo.Text = "";
                    OptShareType.Enabled = false;
                    txtVECVShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtCustomerShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtDealerShare.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    //Temporary Commented by Shyamal as on 12112013
                    RequestDetails.Style.Add("display", "none");
                }
                ds = null;
                ObjWarranty = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void lblSelectModel_Click(object sender, EventArgs e)
        {
            try
            {
                txtOdometer.Enabled = true;
                txtHrsReading.Enabled = true;
                //sujata 22012011
                Label14.Visible = false;
                Label15.Visible = false;
                txtLastMeterReading.Text = "0.00";
                //sujata 22012011
                GetVehicleDetails(hdnChassis.Value);

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private void GetVehicleDetails(string sChassisNo)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsVehicle = new DataSet();
                //dsVehicle = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetVehicleDetailsOfChassis", hdnJobcardID.Value, DropClaimTypes.SelectedValue, ((iMenuId == 415) ? "Request" : "Claim"));
                dsVehicle = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetVehicleDetailsOfExportChassis", txtchassisID.Text, DropClaimTypes.SelectedValue, ((iMenuId == 415) ? "Request" : "Claim" + DrpInvType.SelectedValue.ToString()), Location.iDealerId,txtRequestID.Text);                

                string sTmpValue = "";
                DisplayData(dsVehicle);
                DrpInvType.Enabled = false;
                
                sClaimTypeIdChassis = DropClaimTypes.SelectedValue;
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", ((sClaimTypeIdChassis == "14") ? "WP" : (sClaimTypeIdChassis == "2") ? "GT" : (sClaimTypeIdChassis == "8" || sClaimTypeIdChassis == "16") ? "GC" : (sClaimTypeIdChassis == "6") ? "CP" : (sClaimTypeIdChassis == "10") ? "RM" : "WC") + DrpInvType.SelectedValue.ToString(), Location.iDealerId);                
                hdnTrNo.Value = Location.sDealerCode + "/C/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
                
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);

                dsVehicle = null;
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

        protected void btnCreateWarrantyClaim_Click(object sender, EventArgs e)
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                int iRequestId = 0;
                string strClaimType = "";
                iRequestId = Func.Convert.iConvertToInt(txtID.Text);
                //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                //    strClaimType = "GW";
                //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
                //    strClaimType = "HR";

                if (ObjWarranty.bCreateWarrantyClaimFromRequest(Location.sDealerCode, Location.iDealerId, iRequestId) == true)
                {
                    iClaimID = Func.Convert.iConvertToInt(txtID.Text);
                    FillSelectionGrid();
                    GetDataAndDisplay(iClaimID);
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Claim created successfully.');</script>");
                }
                else
                {

                }
                ObjWarranty = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //Sujata 19012011
        //Check wheather warranty Claim have one part or labour or sublet
        private bool bChkDetailsFromPartLabourSubletGrid()
        {
            bool bValidate = false;
            int iPartID;

            // check for Part 
            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                iPartID = Func.Convert.iConvertToInt((PartDetailsGrid.Rows[iRowCnt].FindControl("txtPartID") as TextBox).Text);
                if (iPartID != 0)
                {
                    bValidate = true;
                }
            }
            if (bValidate == false)
            {
                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //LabourID                
                    iPartID = Func.Convert.iConvertToInt((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID") as TextBox).Text);

                    if (iPartID != 0)
                    {
                        bValidate = true;
                    }
                }
            }
            if (bValidate == false)
            {
                for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
                {
                    DropDownList drpLubricantType = (DropDownList)LubricantDetailsGrid.Rows[iRowCnt].FindControl("drpLubricantType");
                    //LubricantID                
                    iPartID = Func.Convert.iConvertToInt(drpLubricantType.SelectedValue);

                    if (iPartID != 0)
                    {
                        bValidate = true;
                    }
                }
            }
            if (bValidate == false)
            {
                for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //DropDownList drpSubletCode = (DropDownList)(SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSubletCode"));
                    TextBox txtLabourID = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
                    // Sublet ID
                    //iPartID = Func.Convert.iConvertToInt(drpSubletCode.SelectedValue);
                    iPartID = Func.Convert.iConvertToInt(txtLabourID.Text);
                    if (iPartID != 0)
                    {
                        bValidate = true;
                    }
                }
            }
            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Part / Lubricant / Labour /Sublet details.');</script>");
            }

            return bValidate;
        }
        //Sujata 19012011
        //Sujata 20012011
        private bool bChkDetailsFromServiceHistory()
        {
            bool bValidate = false;
            string sClaimType = "";
            DataTable dt = new DataTable();
            int iID = Func.Convert.iConvertToInt(txtID.Text);

            clsWarranty objWarranty = new clsWarranty();

            if (iMenuId == 796)
            {
                dt = objWarranty.GetWarrantyServiceHistory(iID, "ServiceGrid", "WC");
                sClaimType = "WC";
            }
            else if (iMenuId == 242)
            {
                dt = objWarranty.GetWarrantyServiceHistory(iID, "ServiceGrid", "GR");
                sClaimType = "GR";
            }
            else if (iMenuId == 415)
            {
                dt = objWarranty.GetWarrantyServiceHistory(iID, "ServiceGrid", "HR");
                sClaimType = "HR";
            }
            if (dt.Rows.Count > 0)
            {
                DataView dvServiceDetails = dt.DefaultView;
                dvServiceDetails.RowFilter = "Claim_Hdr_ID=" + iID + " and Service_Type='Warranty' and ClaimOrRequest ='" + sClaimType + "' and Service_No <> '0' and Service_No <> '' ";
                if (dvServiceDetails.ToTable().Rows.Count > 0)
                    bValidate = true;
                else
                    bValidate = false;
            }
            if (hdnReturnedCnt.Value == "0")
            {
                if (bValidate == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Service History details.');</script>");
                }
            }
            else
                bValidate = true;
            return bValidate;
        }
        //Sujata 20012011    
        protected void DropClaimTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sClaimTypeIdChassis = DropClaimTypes.SelectedValue;                
                if (txtUserType.Text != "6")
                {   
                    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", ((sClaimTypeIdChassis == "14") ? "WP" : (sClaimTypeIdChassis == "2") ? "GT" : (sClaimTypeIdChassis == "8" || sClaimTypeIdChassis == "16") ? "GC" : (sClaimTypeIdChassis == "6") ? "CP" : (sClaimTypeIdChassis == "10") ? "RM" : "WC") + DrpInvType.SelectedValue.ToString(), Location.iDealerId);
                }
                //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this," + Location.iDealerId + ",'" + Func.Convert.sConvertToString(Session["HOBR_ID"]) + "','" + sClaimTypeIdChassis + "','" + ((iMenuId == 415) ? "Request" : "Claim") + "')");
                ClearFormControl();
                lnkSrvVAN.Visible = false;
                lblSelectModel.Visible = (Func.Convert.iConvertToInt(sClaimTypeIdChassis) > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnReSubmitRequest_Click(object sender, EventArgs e)
        {
            hdnReSubmitRequest.Value = "P";
            PSelectionGrid.Style.Add("display", "none");
            lblServiceHistroy.Style.Add("display", "none");
            txtRefClaimID.Text = txtID.Text;
            iClaimID = (txtID.Text == "" || txtID.Text == "0") ? Func.Convert.iConvertToInt(txtRefClaimID.Text) : Func.Convert.iConvertToInt(txtID.Text);
            FillSelectionGrid();
            GetDataAndDisplay(iClaimID);
            txtRefClaimNo.Text = txtID.Text;
            txtID.Text = "";
            btnReSubmitRequest.Visible = false;
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            {
                //VHP Waaranty Start
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GR", Location.iDealerId);
                //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GCR", Location.iDealerId);
                //VHP Waaranty Start
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim)
            {
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "HR", Location.iDealerId);
                txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
            }
        }
        protected void btnReSubmitClaim_Click(object sender, EventArgs e)
        {
            hdnReSubmitClaim.Value = "P";
            PSelectionGrid.Style.Add("display", "none");
            lblServiceHistroy.Style.Add("display", "none");
            txtRefClaimID.Text = txtID.Text;
            iClaimID = (txtID.Text == "" || txtID.Text == "0") ? Func.Convert.iConvertToInt(txtRefClaimID.Text) : Func.Convert.iConvertToInt(txtID.Text);
            FillSelectionGrid();
            GetDataAndDisplay(iClaimID);
            hdnTrNo.Value = Location.sDealerCode + "/CS/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());
            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();

            //DataTable dtFileAttachResubmit = new DataTable();

            //DataRow dr;
            //dtFileAttachResubmit = new DataTable();
            ////Get Header InFormation        
            //dtFileAttachResubmit.Columns.Add(new DataColumn("ID", typeof(int)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("Description", typeof(string)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("File_Names", typeof(string)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("UserId", typeof(int)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("Status", typeof(string)));            
         
            //dr = dtFileAttachResubmit.NewRow();                   
            //dr["ID"] = 0;                   
            //dr["Description"] = "";
            //dr["File_Names"] = "";
            //dr["UserId"] = Func.Convert.iConvertToInt(sDealerId);
            //dr["Status"] = "S";
                    
            //dtFileAttachResubmit.Rows.Add(dr);
            //dtFileAttachResubmit.AcceptChanges();           
            
            //FileAttchGrid.DataSource = dtFileAttachResubmit;
            //FileAttchGrid.DataBind();

            txtRefClaimNo.Text = txtID.Text;
            txtID.Text = "";
            txtDealerRemark.Text = "";
            txtDealerRemark.Enabled= true;
            btnReSubmitClaim.Visible = false;            
            //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal)
            //    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "EC", Location.iDealerId);
            //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            //    txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "GR", Location.iDealerId);
            txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);


        }
        protected void btnReturnedClaim_Click(object sender, EventArgs e)
        {
            hdnReturnedClaim.Value = "P";
            PSelectionGrid.Style.Add("display", "none");
            lblServiceHistroy.Style.Add("display", "none");
            txtRefClaimID.Text = txtID.Text;
            iClaimID = (txtID.Text == "" || txtID.Text == "0") ? Func.Convert.iConvertToInt(txtRefClaimID.Text) : Func.Convert.iConvertToInt(txtID.Text);
            FillSelectionGrid();
            GetDataAndDisplay(iClaimID);
            hdnTrNo.Value = Location.sDealerCode + "/CR/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());

            //DataTable dtFileAttachResubmit = new DataTable();

            //DataRow dr;
            //dtFileAttachResubmit = new DataTable();
            ////Get Header InFormation        
            //dtFileAttachResubmit.Columns.Add(new DataColumn("ID", typeof(int)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("Description", typeof(string)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("File_Names", typeof(string)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("UserId", typeof(int)));
            //dtFileAttachResubmit.Columns.Add(new DataColumn("Status", typeof(string)));

            //dr = dtFileAttachResubmit.NewRow();
            //dr["ID"] = 0;
            //dr["Description"] = "";
            //dr["File_Names"] = "";
            //dr["UserId"] = Func.Convert.iConvertToInt(sDealerId);
            //dr["Status"] = "S";

            //dtFileAttachResubmit.Rows.Add(dr);
            //dtFileAttachResubmit.AcceptChanges();

            //FileAttchGrid.DataSource = dtFileAttachResubmit;
            //FileAttchGrid.DataBind();

            FileAttchGrid.DataSource = null;
            FileAttchGrid.DataBind();

            txtRefClaimNo.Text = txtID.Text;
            txtID.Text = "";
            txtDealerRemark.Text = "";
            txtDealerRemark.Enabled = true;
            btnReturnedClaim.Visible = false;
            txtClaimDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {

            Location.SetControlValue();
            sDealerId = Location.iDealerId.ToString();
            SetDocumentDetails();
            FillSelectionGrid();
            if (txtUserType.Text == "6")
            {
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);

                btnReturnedClaim.Visible = false;
                btnReSubmitRequest.Visible = false;
                btnCreateWarrantyClaim.Visible = false;
                lblSelectModel.Visible = false;
            }
            trNewAttachment.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
            trNewAttachment1.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
        }
        private void BindDataToGrid()
        {
            try
            {
                GrdPartGroup.DataSource = dtJbGrpTaxDetails;
                GrdPartGroup.DataBind();

                Acc_GrdPartGroup.DataSource = Acc_dtGrpTaxDetails;
                Acc_GrdPartGroup.DataBind();

                int iRow = dtWarrJobDescDet.Rows.Count;
                for (int iRowCnt = iRow + 1; iRowCnt <= 5; iRowCnt++)
                {
                    DataRow dr;

                    dr = dtWarrJobDescDet.NewRow();
                    dr["ID"] = 0;
                    dr["InvDescID"] = 0;
                    dtWarrJobDescDet.Rows.Add(dr);
                    dtWarrJobDescDet.AcceptChanges();
                }                

                InvJobDescGrid.DataSource = dtWarrJobDescDet;
                InvJobDescGrid.DataBind();
                SetControlPropertyToInvDescGrid();

                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();

                Acc_SetGridControlPropertyTax();
                Acc_SetGridControlPropertyTaxCalculation();

                PInvDesc.Style.Add("display", "none");
                //PInvDesc.Style.Add("display", (hdnISDocGST.Value.ToString() == "Y") ? "" : "none");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private bool bFillDetailsFromTaxGrid()
        {

            dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];
            int iCouponID = 0;
            int iSrvjobID = 0;
            bool bValidate = true;

            //For Tax Details
            dtJbGrpTaxDetails = (DataTable)(Session["JbGrpTaxDetails"]);           

            //if (bSaveTmTxDtls == true)
            //{                
            for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
            {
                //Group Code
                TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                dtJbGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                //Group Name
                TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                //Get Net Amount
                TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                //Get Discount Perc
                TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                //Get Discount Amount
                TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                // Get Tax
                DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                //Get Tax Percentage                
                DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                dtJbGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                //Get Tax Amount
                TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                // Get Tax1
                DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                //Get Tax1 Percentage                
                DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                //Get Tax1 Amount
                TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                // Get Tax2
                DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                //Get Tax2 Percentage                
                DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                //Get Tax2 Amount
                TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                // Get Total
                TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                dtJbGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
            }
            return bValidate;
        }
        
        private void SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    //string srowGRPID = Func.Convert.sConvertToString("L");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        // Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                         //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                   if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                       Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                       Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))
                        //Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));


                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        //Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else
                        //Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
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
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                for (int i = 0; i < PartDetailsGrid.Rows.Count; i++)
                {
                    TextBox TxtExclTotal = (TextBox)PartDetailsGrid.Rows[i].FindControl("txtTotal");

                    //TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax2") as TextBox);


                    //if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                    TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if ("01" == txtGRPID.Text.Trim() && "01" != "" && drpTax.SelectedValue == txtPTax.Text)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                        }
                    }
                }

                for (int i = 0; i < LubricantDetailsGrid.Rows.Count; i++)
                {
                    TextBox TxtExclTotal = (TextBox)LubricantDetailsGrid.Rows[i].FindControl("txtTotal");

                    //TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtOTax = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax") as TextBox);
                    TextBox txtOTax1 = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax1") as TextBox);
                    TextBox txtOTax2 = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax2") as TextBox);


                    //if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                    TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if ("02" == txtGRPID.Text.Trim() && "02" != "" && drpTax.SelectedValue == txtOTax.Text)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                        }
                    }
                }
                
                for (int i = 0; i < LabourDetailsGrid.Rows.Count; i++)
                {
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourID");

                    if (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != "")
                    {
                        string sFirstFiveDigit = "";
                        TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourCode");
                        sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);                       
                       
                        TextBox TxtExclTotal = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtTotal");

                        //TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax") as TextBox);
                        TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax1") as TextBox);
                        TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax2") as TextBox);

                        double LTotal = 0;
                        
                        LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());
                        

                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                            DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                            TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            //if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)
                            if ("L" == txtGRPID.Text.Trim() && "L" != "" && drpTax.SelectedValue == txtLTax.Text)                                
                            {
                                txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal)).ToString("0.00"));
                            }
                        }
                    }
                }


                for (int i = 0; i < SubletDetailsGrid.Rows.Count; i++)
                {
                    TextBox txtLabourID = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtLabourID");
                    //DropDownList drpSubletCode = (DropDownList)SubletDetailsGrid.Rows[i].FindControl("drpSubletCode");

                    if (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != "")
                    //if (drpSubletCode.SelectedValue.Trim() != "0" && drpSubletCode.SelectedValue.Trim() != "")                        
                    {
                        //string sFirstFiveDigit = "";
                        //TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourCode");
                        //sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                        //TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox TxtExclTotal = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtTotal");
                        TextBox txtSTax = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax");      
                        TextBox txtSTax1 = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax1");
                        TextBox txtSTax2 = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax2");

                        double LTotal = 0;

                        LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());


                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                            DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                            TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            //if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)                            
                            //if (drpTax.SelectedValue == txtSTax.Text)
                            if ("L" == txtGRPID.Text.Trim() && "L" != "" && drpTax.SelectedValue == txtSTax.Text)                                
                            {
                                txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal)).ToString("0.00"));
                            }
                        }
                    }
                }

                //GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", "none");
                //GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", "none");
                txtDTaxAmt.Text = "0.00";
                txtDTax1Amt.Text = "0.00";
                txtDTax2Amt.Text = "0.00";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", "none");
                    GrdPartGroup.Rows[iRowCnt].Cells[4].Style.Add("display", "none");

                    GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", "none");
                    GrdPartGroup.Rows[iRowCnt].Cells[5].Style.Add("display", "none");

                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    //group Percentage
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    //group Discount Amount
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    //Doc Discount Amount
                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

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
                    if (sTax1ApplOn == "1")
                    {
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
                    //Sujata 23092014 End

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

                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 2);
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));

                    double dDocTotTaxAmt = Func.Convert.dConvertToDouble(txtDTaxAmt.Text);
                    double dDocTotTax1Amt = Func.Convert.dConvertToDouble(txtDTax1Amt.Text);
                    double dDocTotTax2Amt = Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                    dGrpMTaxAmt = dGrpMTaxAmt + dDocTotTaxAmt;
                    dGrpTax1Amt = dGrpTax1Amt + dDocTotTax1Amt;
                    dGrpTax2Amt = dGrpTax2Amt + dDocTotTax2Amt;

                    txtDTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));
                    txtDTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));
                    txtDTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));  
                }

                double dClaimAmount = Func.Convert.dConvertToDouble(txtPartAmount.Text) + Func.Convert.dConvertToDouble(txtLabourAmount.Text) +
                     Func.Convert.dConvertToDouble(txtLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtSubletAmount.Text) + Func.Convert.dConvertToDouble(txtDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtDTax2Amt.Text);

                txtClaimAmt.Text = dClaimAmount.ToString("0.00");

                trTax2.Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");
                trTax1.Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");
                lblDTaxlbl.Text = (hdnISDocGST.Value == "Y") ? (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Part + Lubricant + Labour + Sublet IGST Amt: " : "Part + Lubricant + Labour + Sublet SGST Amt: " : "Labour + Sublet Tax Amt: "; // Hide Header    SGST
                lblDTax1lbl.Text = (hdnISDocGST.Value == "Y") ? "Part + Lubricant + Labour + Sublet Additional CGST Amt: " : "Part + Lubricant + Labour + Sublet Additional Tax1 Amt: "; // Hide Header  
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

                int iLTaxID = 0;
                int iLTaxID1 = 0;
                int iLTaxID2 = 0;

                dtJbGrpTaxDetails = (DataTable)Session["JbGrpTaxDetails"];

                Boolean bDtSelPartRow = false;
                dtJbGrpTaxDetails.Clear();

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "01";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    if (sGrCode.Length > 0)
                    {
                        TextBox txtPTax = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax");
                        iPartTaxID = Func.Convert.iConvertToInt(txtPTax.Text);

                        TextBox txtPTax1 = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(txtPTax1.Text);

                        TextBox txtPTax2 = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtPTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(txtPTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = "01";
                        dr["Gr_Name"] = "Spares";

                        dr["net_inv_amt"] = 0;

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


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }

                for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "02";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;                    

                    if (sGrCode.Length > 0)
                    {
                        TextBox txtOTax = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax");
                        iPartTaxID = Func.Convert.iConvertToInt(txtOTax.Text);

                        TextBox txtOTax1 = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(txtOTax1.Text);

                        TextBox txtOTax2 = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtOTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(txtOTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = "02";
                        dr["Gr_Name"] = "Lubricant";

                        dr["net_inv_amt"] = 0;

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


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }


                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iLTaxID = 0;
                    iLTaxID1 = 0;
                    iLTaxID2 = 0;
                    bDtSelPartRow = false;

                    //TextBox txtLGroupCode = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");
                    //sGrCode = txtLGroupCode.Text.Trim();                    
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");

                    sGrCode = "L";

                    CheckBox ChkForDelete = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0 && (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != ""))
                    {
                        TextBox txtLTax = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax");
                        iLTaxID = Func.Convert.iConvertToInt(txtLTax.Text);

                        TextBox txtLTax1 = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax1");
                        iLTaxID1 = Func.Convert.iConvertToInt(txtLTax1.Text);

                        TextBox txtLTax2 = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtLTax2");
                        iLTaxID2 = Func.Convert.iConvertToInt(txtLTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iLTaxID) &&
                            iLTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iLTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Spares" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iLTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iLTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iLTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }

                for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iLTaxID = 0;
                    iLTaxID1 = 0;
                    iLTaxID2 = 0;
                    bDtSelPartRow = false;

                    //TextBox txtLGroupCode = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtLGroupCode");
                    //sGrCode = txtLGroupCode.Text.Trim();                    
                    TextBox txtLabourID = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtLabourID");
                    //DropDownList drpSubletCode = (DropDownList)SubletDetailsGrid.Rows[iRowCnt].FindControl("drpSubletCode");

                    sGrCode = "L";

                    CheckBox ChkForDelete = (CheckBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                    if (sGrCode.Length > 0 && (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != ""))
                    //if (sGrCode.Length > 0 && (drpSubletCode.SelectedValue.Trim() != "0" && drpSubletCode.SelectedValue.Trim() != ""))
                    {
                        TextBox txtSTax = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax");
                        iLTaxID = Func.Convert.iConvertToInt(txtSTax.Text);

                        TextBox txtSTax1 = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax1");
                        iLTaxID1 = Func.Convert.iConvertToInt(txtSTax1.Text);

                        TextBox txtSTax2 = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtSTax2");
                        iLTaxID2 = Func.Convert.iConvertToInt(txtSTax2.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtJbGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtJbGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtJbGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iLTaxID) &&
                            iLTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;

                    if (bDtSelPartRow == false && sGrCode != "" && iLTaxID > 0 && ChkForDelete.Checked != true)
                    {
                        dr = dtJbGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Spares" : sGrCode.Trim() == "02" ? "Lubricant" : sGrCode.Trim() == "L" ? "Labour" : "Local Part";

                        dr["net_inv_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iLTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iLTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iLTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtJbGrpTaxDetails.Rows.Add(dr);
                        dtJbGrpTaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
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

                    if (idtRowCnt < dtWarrJobDescDet.Rows.Count)
                    {

                        //DrpInvJobDesc.Attributes.Add("onChange", "return CheckInvDescSelected(event,this);");

                        DrpInvJobDesc.SelectedValue = Func.Convert.sConvertToString(dtWarrJobDescDet.Rows[iRowCnt]["InvDescID"]);

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
            dtWarrJobDescDet = (DataTable)Session["InvJobDescDet"];

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

                    dtWarrJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                    iCntForDelete++;
                }
                else
                {
                    dtWarrJobDescDet.Rows[iRowCnt]["InvDescID"] = iInvDescID;
                }
            }
            bValidate = true;
            //if (iCntForDelete == 0)
            //{
            //    bValidate = false;
            //}

            //if (bValidate == false)
            //{
            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please select At least one Warranty Job Description.');</script>");
            //}
        Last:
            return bValidate;
        }


        #endregion

        protected void DrpInvType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "EC" + DrpInvType.SelectedValue.ToString(), Location.iDealerId);
            //txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", DrpInvType.SelectedValue.ToString() + "C", Location.iDealerId);
            if (txtUserType.Text != "6")
            {
                sClaimTypeIdChassis = DropClaimTypes.SelectedValue;
                txtClaimNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", ((sClaimTypeIdChassis == "14") ? "WP" : (sClaimTypeIdChassis == "2") ? "GT" : (sClaimTypeIdChassis == "8" || sClaimTypeIdChassis == "16") ? "GC" : (sClaimTypeIdChassis == "6") ? "CP" : (sClaimTypeIdChassis == "10") ? "RM" : "WC") + DrpInvType.SelectedValue.ToString(), Location.iDealerId);                
            }
            DrpInvType.Enabled = false;
            lnkSrvVAN.Visible = false;
            lblSelectModel.Visible = (Func.Convert.iConvertToInt(sClaimTypeIdChassis) > 0) ? true : false;
        }

        private void Acc_SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");                    
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Tax
                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");                    
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))                        
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else                        
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))                    
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else                        
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));                    

                    DropDownList drpTaxTag = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");

                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))                        
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + "  and isnull(Active,'N')='Y'"));
                    else                        
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnISDocGST.Value == "N"))                        
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else                        
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));


                    DropDownList DrpTax1ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")                        
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));
                    else                        
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : " " + ((hdnISDocGST.Value == "Y") ? "" : " and isnull(Is_Service_Tax,'N') ='N' ") + " and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnISDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
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
        private void Acc_SetGridControlPropertyTaxCalculation()
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
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                for (int i = 0; i < PartDetailsGrid.Rows.Count; i++)
                {
                    TextBox TxtExclTotal = (TextBox)PartDetailsGrid.Rows[i].FindControl("txtAccAmount");

                    //TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtPTax = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax") as TextBox);
                    TextBox txtPTax1 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax1") as TextBox);
                    TextBox txtPTax2 = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPTax2") as TextBox);

                    //if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                    TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                        if ("01" == txtGRPID.Text.Trim() && "01" != "" && drpTax.SelectedValue == txtPTax.Text)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                        }
                    }
                }

                for (int i = 0; i < LubricantDetailsGrid.Rows.Count; i++)
                {
                    TextBox TxtExclTotal = (TextBox)LubricantDetailsGrid.Rows[i].FindControl("txtAccAmount");

                    //TextBox txtGrNo = (TextBox)(PartDetailsGrid.Rows[i].FindControl("txtPartGroupCode") as TextBox);

                    TextBox txtOTax = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax") as TextBox);
                    TextBox txtOTax1 = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax1") as TextBox);
                    TextBox txtOTax2 = (TextBox)(LubricantDetailsGrid.Rows[i].FindControl("txtOTax2") as TextBox);


                    //if (txtGrNo.Text.Trim() != "") TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);
                    TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text), 2);

                    for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if ("02" == txtGRPID.Text.Trim() && "02" != "" && drpTax.SelectedValue == txtOTax.Text)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                        }
                    }
                }

                for (int i = 0; i < LabourDetailsGrid.Rows.Count; i++)
                {
                    TextBox txtLabourID = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourID");

                    if (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != "")
                    {
                        string sFirstFiveDigit = "";
                        TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourCode");
                        sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);

                        TextBox TxtExclTotal = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtAccAmount");

                        //TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox txtLTax = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax") as TextBox);
                        TextBox txtLTax1 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax1") as TextBox);
                        TextBox txtLTax2 = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLTax2") as TextBox);

                        double LTotal = 0;

                        LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());


                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                            DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                            TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            //if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)
                            if ("L" == txtGRPID.Text.Trim() && "L" != "" && drpTax.SelectedValue == txtLTax.Text)
                            {
                                txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal)).ToString("0.00"));
                            }
                        }
                    }
                }


                for (int i = 0; i < SubletDetailsGrid.Rows.Count; i++)
                {
                    TextBox txtLabourID = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtLabourID");
                    //DropDownList drpSubletCode = (DropDownList)SubletDetailsGrid.Rows[i].FindControl("drpSubletCode");

                    if (txtLabourID.Text.Trim() != "0" && txtLabourID.Text.Trim() != "")
                    //if (drpSubletCode.SelectedValue.Trim() != "0" && drpSubletCode.SelectedValue.Trim() != "")
                    {
                        //string sFirstFiveDigit = "";
                        //TextBox txtLabourCode = (TextBox)LabourDetailsGrid.Rows[i].FindControl("txtLabourCode");
                        //sFirstFiveDigit = txtLabourCode.Text.ToString().Substring(0, 5);
                        //TextBox txtGrNo = (TextBox)(LabourDetailsGrid.Rows[i].FindControl("txtLGroupCode") as TextBox);

                        TextBox TxtExclTotal = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtAccAmount");
                        TextBox txtSTax = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax");
                        TextBox txtSTax1 = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax1");
                        TextBox txtSTax2 = (TextBox)SubletDetailsGrid.Rows[i].FindControl("txtSTax2");

                        double LTotal = 0;

                        LTotal = Func.Convert.dConvertToDouble(TxtExclTotal.Text.Trim());


                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(LTotal), 2);

                        for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                        {
                            TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                            DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                            TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                            //if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedValue == txtLTax.Text)                            
                            //if (drpTax.SelectedValue == txtSTax.Text)
                            if ("L" == txtGRPID.Text.Trim() && "L" != "" && drpTax.SelectedValue == txtSTax.Text)
                            {
                                txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(LTotal)).ToString("0.00"));
                            }
                        }
                    }
                }
                txtAccDTaxAmt.Text = "0.00";
                txtAccDTax1Amt.Text = "0.00";
                txtAccDTax2Amt.Text = "0.00";

                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    Acc_GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", "none");
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[4].Style.Add("display", "none");

                    Acc_GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", "none");
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[5].Style.Add("display", "none");

                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    //group Percentage
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    //group Discount Amount
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    //Doc Discount Amount
                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    TextBox txtTaxTag = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100));
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
                    if (sTax1ApplOn == "1")
                    {
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
                    //Sujata 23092014 End

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

                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    dGrpTotal = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt)), 2);
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));

                    double dDocTotTaxAmt = Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text);
                    double dDocTotTax1Amt = Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text);
                    double dDocTotTax2Amt = Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                    dGrpMTaxAmt = dGrpMTaxAmt + dDocTotTaxAmt;
                    dGrpTax1Amt = dGrpTax1Amt + dDocTotTax1Amt;
                    dGrpTax2Amt = dGrpTax2Amt + dDocTotTax2Amt;

                    txtAccDTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));
                    txtAccDTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));
                    txtAccDTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));
                }
                double dClaimAmount = Func.Convert.dConvertToDouble(txtAccPartAmount.Text) + Func.Convert.dConvertToDouble(txtAccLabourAmount.Text) +
                     Func.Convert.dConvertToDouble(txtAccLubricantAmount.Text) + Func.Convert.dConvertToDouble(txtAccSubletAmount.Text) + Func.Convert.dConvertToDouble(txtAccDTaxAmt.Text) + Func.Convert.dConvertToDouble(txtAccDTax1Amt.Text) + Func.Convert.dConvertToDouble(txtAccDTax2Amt.Text);

                txtAccClaimAmt.Text = dClaimAmount.ToString("0.00");

                trTax2.Style.Add("display", (hdnISDocGST.Value == "Y") ? "none" : "");
                trTax1.Style.Add("display", (hdnISDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");
                lblAccDTaxlbl.Text = (hdnISDocGST.Value == "Y") ? (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Accepted Part + Lubricant + Labour + Sublet IGST Amt: " : "Accepted Part + Lubricant + Labour + Sublet SGST Amt: " : "Accepted Labour + Sublet Tax Amt: "; // Hide Header    SGST
                lblAccDTax1lbl.Text = (hdnISDocGST.Value == "Y") ? "Accepted Part + Lubricant + Labour + Sublet Additional CGST Amt: " : "Accepted Part + Lubricant + Labour + Sublet Additional Tax1 Amt: "; // Hide Header  
                                
                lblAccClaimAmt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccDTax1lbl.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccDTax2Amt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccDTaxlbl.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccLabourAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccLubricantAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccPartAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                lblAccSubletAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");

                txtAccClaimAmt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccDTax1Amt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccDTax2Amt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccDTaxAmt.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccLabourAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccLubricantAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccPartAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
                txtAccSubletAmount.Style.Add("display", (hdnIsSHQResource.Value.Trim() == "Y") ? "" : "none");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private bool Acc_bFillDetailsFromTaxGrid()
        {
            Acc_dtGrpTaxDetails = (DataTable)Session["JbAccGrpTaxDetails"];
            int iCouponID = 0;
            int iSrvjobID = 0;
            bool bValidate = true;

            //For Tax Details
            Acc_dtGrpTaxDetails = (DataTable)(Session["JbAccGrpTaxDetails"]);

            //if (bSaveTmTxDtls == true)
            //{                
            for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
            {
                //Group Code
                TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                //Group Name
                TextBox txtMGrName = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                //Get Net Amount
                TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                //Get Discount Perc
                TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                //Get Discount Amount
                TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                // Get Tax
                DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                //Get Tax Percentage                
                DropDownList drpTaxPer = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                //Get Tax Amount
                TextBox txtGrTaxAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                // Get Tax1
                DropDownList drpTax1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                //Get Tax1 Percentage                
                DropDownList drpTaxPer1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                //Get Tax1 Amount
                TextBox txtGrTax1Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                // Get Tax2
                DropDownList drpTax2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                //Get Tax2 Percentage                
                DropDownList drpTaxPer2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                //Get Tax2 Amount
                TextBox txtGrTax2Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                // Get Total
                TextBox txtTaxTot = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
            }
            return bValidate;
        }

        protected void BtnSaveAccGrpDtl_Click(object sender, EventArgs e)
        {
            bool bSaveRecord = false;
            
            iClaimID = Func.Convert.iConvertToInt(txtID.Text);

            clsWarranty ObjWarranty = new clsWarranty();
            
            Acc_bFillDetailsFromTaxGrid();

            bSaveRecord = ObjWarranty.bSaveWarrantyClaimAccTaxRecord(Acc_dtGrpTaxDetails, iClaimID);
           
            if (bSaveRecord == true)
            {   
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Accepted Amount Tax Details For Warranty Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
                hdnAccDetUpdate.Value = "Y";
                BtnSaveAccGrpDtl.Enabled = false;
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Accepted Amount Tax Details For Warranty Claim") + "','" + Server.HtmlEncode(txtClaimNo.Text) + "');</script>");
            }
        }
        private int i = 1;
        protected void PartDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtPartID != "0")
                {
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }
        private int j = 1;
        protected void LabourDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {   
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string txtLabourID = (e.Row.FindControl("txtLabourID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);
                if (txtLabourID != "0")
                {
                    lblNo.Text = j.ToString();
                    j++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    j = 1;
                }
            }
        }       

        protected void lnkSrvVAN_Click(object sender, EventArgs e)
        {
            try
            {
                dtLabour = (DataTable)Session["LabourDetails"];
                lblLabourRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) == 0) ? Func.Common.sRowCntOfTable(dtLabour) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtLabour)) - 1);
                BindDataToLaborGrid(true, 0);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        
    }
}