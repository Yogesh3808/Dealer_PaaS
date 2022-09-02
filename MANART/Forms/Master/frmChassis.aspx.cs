using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_BAL;
using MANART_DAL;
using AjaxControlToolkit;
using System.IO;

namespace MANART.Forms.Master
{
    public partial class frmChassis : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        int iChassisID = 0;
        int iMenuId = 0;
        int iUserId = 0;
        string DealerId;
        string DestinationFilePath;
        private string sDealerCode = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                // this is defined because of printing option
                ToolbarC.iFormIdToOpenForm = 99;
                //Megha 20012012
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                if (iMenuId == 482)
                {

                    lblSAPInvoiceNo.Style.Add("display", "none");
                    txtSAPInvoiceNo.Style.Add("display", "none");
                    lblSAPInvoiceDate.Style.Add("display", "none");
                    txtSAPInvoiceDate.Style.Add("display", "none");
                    lblSAPInvoiceAmt.Style.Add("display", "none");
                    txtSAPInvoiceAmt.Style.Add("display", "none");
                    lblSellingDealerCode.Style.Add("display", "none");
                    txtSellDealCode.Style.Add("display", "none");
                    lblSellingDealerName.Style.Add("display", "none");
                    txtSellDealName.Style.Add("display", "none");
                    lblTheftFlag.Style.Add("display", "none");
                    txtTheftFlag.Style.Add("display", "none");
                    lblDealerSpareCode.Style.Add("display", "none");
                    txtSpareCode.Style.Add("display", "none");
                    lblDealerHDCode.Style.Add("display", "none");
                    txtHDCode.Style.Add("display", "none");
                    lblSAPSTNNo.Style.Add("display", "none");
                    txtSAPSTNNo.Style.Add("display", "none");
                    lblSAPSTNDate.Style.Add("display", "none");
                    txtSAPSTNDate.Style.Add("display", "none");
                    lblPDIDealerCode.Style.Add("display", "none");
                    txtPDIDealerCode.Style.Add("display", "none");
                    //lblSAPSTNReceivedDate.Style.Add("display", "none");
                    //txtSAPSTNReceivedDate.Style.Add("display", "none");
                    //lblSAPVEHPOSTReceivedDate.Style.Add("display", "none");
                    //txtSAPVEHPOSTReceivedDate.Style.Add("display", "none");
                    //lblSAPINSReceivedDate.Style.Add("display", "none");
                    //txtSAPINSReceivedDate.Style.Add("display", "none");
                    //lblDMSINSReceivedDate.Style.Add("display", "none");
                    //txtDMSINSReceivedDate.Style.Add("display", "none");
                    //lblLastXMLCreateDate.Style.Add("display", "none");
                    //txtLastXMLCreateDate.Style.Add("display", "none");
                    //lblSAPINSPostedDate.Style.Add("display", "none");
                    //txtSAPINSPostedDate.Style.Add("display", "none"); 
                    gvOwnerDetails.Columns[2].Visible = false;
                    gvOwnerDetails.Columns[3].Visible = false;
                    gvOwnerDetails.Columns[4].Visible = false;
                    gvOwnerDetails.Columns[5].Visible = false;
                    // PPortalDateDetails.Style.Add("display", "none");
                }

                //Megha 20012012




                if (!IsPostBack)
                {
                    //clsChassis ObjChassis = new clsChassis();
                    //DataSet ds = new DataSet();
                    //ds = ObjChassis.GetMaxChassis(0);
                    //if (iChassisID == 0)
                    //    iChassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);

                    //if (iChassisID != 0)
                    //{
                    //    GetDataAndDisplay();
                    //}

                    //if (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 2 || Func.Convert.iConvertToInt(Session["UserType"]) == 3) //service history option add domestiv dealer also 
                    //    lblServiceHistroy.Style.Add("display", "");
                    //else
                    //    lblServiceHistroy.Style.Add("display", "none");


                    Session["ModelDetails"] = null;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>disableBackButton();</script>");

                }

                SearchGrid.sGridPanelTitle = "Chassis Master";
                FillSelectionGrid();

                //iChassisID = Func.Convert.iConvertToInt(txtID.Text);
                //iChassisID = Func.Convert.iConvertToInt(SearchGrid.iID);   

                SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            clsCommon objCommon = null;
            DataSet dsChassis = new DataSet();
            try
            {
                objCommon = new clsCommon();
                if (sDealerCode.Trim().StartsWith("R")) { }
                else
                {
                    if (Cache["Chassis-1"] == null)
                    {
                        dsChassis = objCommon.GetSqlToFillSelectionGrid("", "", "", -1, "Chassis");
                        Cache["Chassis-1"] = dsChassis;
                        Cache.Insert("Chassis-1", dsChassis, null, DateTime.Now.AddSeconds(900), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, new System.Web.Caching.CacheItemRemovedCallback(CachedItemRemoveCallBack));

                    }
                }
                

                lblTitle.Text = "Chassis";
                SearchGrid.bIsCollapsable = false;
                EnableDisable();


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objCommon != null) objCommon = null;
                if (dsChassis != null) dsChassis = null;
            }
        }
        private void CachedItemRemoveCallBack(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            if (key.ToUpper() == "Chassis-1")
            {
                Cache.Remove("Chassis-1");
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            iChassisID = Func.Convert.iConvertToInt(SearchGrid.iID);
            txtID.Text = Func.Convert.sConvertToString(iChassisID);
            GetDataAndDisplay();
            string strReportpath;
            strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
            lblServiceHistroy.Attributes.Add("onClick", "return ShowClaimHistoryDtls('" + txtID.Text + "','" + strReportpath + "');");

            //}

        }
        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    iChassisID = Func.Convert.iConvertToInt(SearchGrid.iID);
        //    GetDataAndDisplay();
        //}
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
        private void SetDocumentDetails()
        {
            //lblTitle.Text = " Chassis Header Details ";
            //lblDocNo.Text = "RFP No.:";
            //lblDocDate.Text = "RFP Date:";              
        }
        private void GetDataAndDisplay()
        {
            try
            {
                clsChassis ObjChassis = new clsChassis();
                DataSet ds = new DataSet();
                if (iChassisID == 0)
                {
                    iChassisID = Func.Convert.iConvertToInt(txtID.Text);
                }
                //iChassisID = 1;
                if (iChassisID != 0)
                {

                    //ds = ObjChassis.GetChassis("", "", "", "","","");
                    ds = ObjChassis.GetChassis(iChassisID);
                    DisplayData(ds);
                    ObjChassis = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    ObjChassis = null;

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
            iChassisID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearChassisHeader();
                    ClearChassisService();
                    ClearWarranty_AMC();
                    ClearAggregate();
                    ClearChassisTyre();
                    gvFSDetails.Visible = false;
                    gvFloatDetails.Visible = false;
                    gvOwnerDetails.Visible = false;
                    gvinstall.Visible = false;

                    return;
                }
                gvFSDetails.Visible = true;
                gvFloatDetails.Visible = true;
                gvOwnerDetails.Visible = true;


                gvinstall.Visible = true;

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {

                    txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
                    txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
                    txtRegNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Reg_No"]);
                    txtFertCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_code"]);
                    txtModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_name"]);
                   // txtModelGroup.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelGroup"]);
                    txtModelCategory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ModelCategory"]);
                    //txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustName"]);
                    txtInWarranty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_Warranty"]);
                    txtInAggreWarranty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_Aggregate_Warranty"]);
                    txtInExtendedWarranty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_Extended_Warranty"]);
                    txtInAdditionalWarranty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_Additional_Warranty"]);
                    txtInAMC.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_AMC"]);
                    txtCoupanNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CouponNo"]);
                    txtInKAM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["In_KAM"]);
                    txtFloatFlag.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Float_flag"]);
                    txtTheftFlag.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Theft_flag"]);
                    txtVUO.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VOU"]);
                    txtCouponsBlocked.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Coupons_block"]);
                    txtSAPInvoiceNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SAP_Invoice_no"]);
                    txtSAPInvoiceDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_Invoice_Date"].ToString(), false);
                    txtSAPInvoiceAmt.Text = Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["Total"]).ToString();
                    txtDANo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DA_no"]);
                    txtSellDealCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Code"]);
                    txtSellDealName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Name"]);
                    //txtAddress.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Address"]);
                    //txtInvoiceNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sale_Invoice_no"]);
                    //txtInvoiceDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Sale_Invoice_date"].ToString(), false);
                    // txtInstallDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["Installation_date"].ToString(), false);
                    txtSpareCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Spare_Code"]); ;
                    txtHDCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HD_Code"]);
                    txtDirectCustomer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Direct_Customer"]);
                    txtSAPSTNNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SAP_STN_No"]);
                    txtSAPSTNDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_STN_date"].ToString(), false);
                    txtPDIDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PDIDealerCode"]);
                    txtDANo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SAPDANo"]);
                    txtChangeFertCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChangeModelCode"]);
                    txtChangeModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChangeModelName"]);
                    txtSAPSTNReceivedDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_STN_Received_Date"].ToString(), false);
                    txtSAPVEHPOSTReceivedDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_VEHPOST_Received_Date"].ToString(), false);
                    txtSAPINSReceivedDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_INS_Received_Date"].ToString(), false);
                    txtDMSINSReceivedDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["DCS_Cr_Date"].ToString(), false);
                    txtLastXMLCreateDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["XML_CR_Date"].ToString(), false);
                    txtSAPINSPostedDate.Text = Func.Convert.tConvertToDate(ds.Tables[0].Rows[0]["SAP_Posted_Date"].ToString(), false);
                    txtDCSINSStatus.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["INS_status"]);
                }
                else
                {
                    ClearChassisHeader();
                }
                //Display Warranty/AMC Details
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtWarEndDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Warranty_End_Date"].ToString(), false);
                    txtExtWarStarDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Extended_Warranty_start_Date"].ToString(), false);
                    txtAggreWarEndDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Aggregate_Warranty_End_Date"].ToString(), false);
                    txtExtWarEndDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Extended_Waranty_End_Date"].ToString(), false);

                    txtAddWarStartDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Additional_Warranty_Start_date"].ToString(), false);
                    txtAddWarEndDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["Additional_Warranty_End_Date"].ToString(), false);

                    txtAMCAggreNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_aggrement_No"]);
                    txtAMCAggreType.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_type"]);
                    txtAMCStartDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["AMC_Start_Date"].ToString(), false);
                    txtAMCEndDate.Text = Func.Convert.tConvertToDate(ds.Tables[1].Rows[0]["AMC_End_Date"].ToString(), false);
                    //Megha 07102013 SA start and end KM
                    txtSAStartKM.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_Start_KM"]);
                    txtSAEndKM.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_End_KM"]);
                    txtStartHrs.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_start_Hrs"]);
                    txtEndHrs.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["AMC_END_hrs1"]);
                    //Megha 07102013

                }
                else
                {
                    ClearWarranty_AMC();
                }

                //Display Coupon Details
                //  if (ds.Tables[2].Rows.Count > 0)
                // {
                gvCouponDetails.DataSource = ds.Tables[2];
                gvCouponDetails.DataBind();
                //   }

                //Display Aggregate Details 
                if (ds.Tables[3].Rows.Count > 0)
                {
                    txtBatteryNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Battery_no"]);
                    txtBatteryMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Battery_Make"]);
                    txtFIPNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["FIP_No"]);
                    txtFIPMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["FIP_Make"]);
                    txtFrontAxleNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Axle_No"]);
                    txtFrontAxleMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Axle_Make"]);
                    txtRearAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_1_No"]);
                    txtRearAxle1Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_1_Make"]);
                    txtRearAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_2_No"]);
                    txtRearAxle2Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_2_Make"]);
                    txtRearAxle3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_3_No"]);
                    txtRearAxle3Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Rear_Axle_3_Make"]);
                    txtTransNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Transmission_No"]);
                    txtTransMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Transmission_Make"]);
                    txtGearBoxNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Gear_Box_no"]);
                    txtGearBoxMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Gear_Box_Make"]);

                }
                else
                {
                    ClearAggregate();
                }

                //Display Chassis Tyre Details 
                if (ds.Tables[4].Rows.Count > 0)
                {
                    txtFrontLeftTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["TYRE_SIZE_FRONT1"]);
                    txtFrontRightTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["TYRE_SIZE_REAR"]);
                    txtRearLeftOuterTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["TYRE_SIZE_SPARE"]);
                    txtFRONT_TYRE_NO_LHS.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["FRONT_TYRE_NO_LHS"]);
                    txtFRONT_TYRE_NO_RHS.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["FRONT_TYRE_NO_RHS"]);
                    //// txtFrontLeftTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Left_Tyre_No"]);
                    // txtFrontLeftTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["FRONT_TYRE_NO_LHS"]);
                    //// txtFrontRightTyreMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Left_Tyre_Make"]);
                    // //txtFrontRightTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Right_Tyre_No"]);
                    // txtFrontRightTyreNo.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["FRONT_TYRE_NO_RHS"]);

                    // //txtFrontRightTyreMake.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["Front_Right_Tyre_Make"]);

                    //// txtRearLeftOuterTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_1_no"]);
                    // txtRearLeftOuterTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_OUTER_LHS_1"]);

                    //// txtRearLeftOuterTyreAxle1Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_1_Make"]);
                    // //txtRearLeftInnerTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_1_no"]);
                    // txtRearLeftInnerTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_INNER_LHS_1"]);
                    //// txtRearLeftInnerTyreAxle1Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_1_Make"]);
                    // //txtRearRightOuterTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_1_no"]);
                    // txtRearRightOuterTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_OUTER_RHS_1"]);
                    //// txtRearRightOuterTyreAxle1Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_1_Make"]);
                    //// txtRearRightInnerTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_1_no"]);
                    // txtRearRightInnerTyreAxle1No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_INNER_RHS_1"]);
                    //// txtRearRightInnerTyreAxle1Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_1_Make"]);

                    // //txtRearLeftOuterTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_2_no"]);
                    // txtRearLeftOuterTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_OUTER_LHS_2"]);
                    //// txtRearLeftOuterTyreAxle2Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_2_Make"]);
                    // //txtRearLeftInnerTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_2_no"]);
                    // txtRearLeftInnerTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_INNER_LHS_2"]);
                    //// txtRearLeftInnerTyreAxle2Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_2_Make"]);
                    //// txtRearRightOuterTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_2_no"]);
                    // txtRearRightOuterTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_OUTER_RHS_2"]);
                    //// txtRearRightOuterTyreAxle2Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_2_Make"]);
                    //// txtRearRightInnerTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_2_no"]);
                    // txtRearRightInnerTyreAxle2No.Text = Func.Convert.sConvertToString(ds.Tables[4].Rows[0]["REAR_TYRE_NO_INNER_RHS_2"]);
                    //// txtRearRightInnerTyreAxle2Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_2_Make"]);

                    // //txtRearLeftOuterTyreAxle3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_3_no"]);
                    //// txtRearLeftOuterTyreAxle3Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_Outer_Tyre_Axle_3_Make"]);
                    //// txtRearLeftInnerTyreAxle3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_3_no"]);
                    // //txtRearLeftInnerTyreAxle3Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearLeft_inner_Tyre_Axle_3_Make"]);
                    // //txtRearRightOuterTyreAxle3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_3_no"]);
                    //// txtRearRightOuterTyreAxle3Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_Outer_Tyre_Axle_3_Make"]);
                    // //txtRearRightInnerTyreAxle3No.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_3_no"]);
                    //// txtRearRightInnerTyreAxle3Make.Text = Func.Convert.sConvertToString(ds.Tables[3].Rows[0]["RearRight_inner_Tyre_Axle_3_Make"]);
                }
                else
                {
                    ClearChassisTyre();

                }

                //Free Service Details

                if (ds.Tables[5].Rows.Count > 0)
                {
                    txtTotPart.Text = Func.Convert.dConvertToDouble(ds.Tables[5].Rows[0]["PartTotal"]).ToString("#0.00");
                    txtTotLabour.Text = Func.Convert.dConvertToDouble(ds.Tables[5].Rows[0]["LabourTotal"]).ToString("#0.00");
                    txtTotLubricant.Text = Func.Convert.dConvertToDouble(ds.Tables[5].Rows[0]["LubricantTotal"]).ToString("#0.00");
                    txtTotSublet.Text = Func.Convert.dConvertToDouble(ds.Tables[5].Rows[0]["SubletTotal"]).ToString("#0.00");
                }
                else
                {
                    ClearChassisService();
                }
                //Sujata 16022011
                //if (ds.Tables[4].Rows.Count > 0)
                //{
                gvFSDetails.DataSource = ds.Tables[5];
                gvFSDetails.DataBind();
                //}
                //Sujata 16022011

                //Free Float Details
                //Sujata 16022011
                //if (ds.Tables[5].Rows.Count > 0)
                //{
                gvFloatDetails.DataSource = ds.Tables[6];
                gvFloatDetails.DataBind();

                //}
                //Sujata 16022011

                //Owner Details
                //Sujata 16022011
                //if (ds.Tables[6].Rows.Count > 0)
                //{
                gvOwnerDetails.DataSource = ds.Tables[7];
                gvOwnerDetails.DataBind();
                if (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                {
                    gvOwnerDetails.Columns[6].Visible = true;
                    gvOwnerDetails.Columns[7].Visible = true;
                }
                else
                {
                    gvOwnerDetails.Columns[6].Visible = false;
                    gvOwnerDetails.Columns[7].Visible = false;
                }
                //}
                //Sujata 16022011

                //Installation Details
                //Sujata 16022011
                //if (ds.Tables[7].Rows.Count > 0)
                //{
                gvinstall.DataSource = ds.Tables[8];
                gvinstall.DataBind();
                //}
                //Sujata 16022011

                //Megha13062012 
                if (iUserId == 321 || iUserId == 228 || iUserId == 22)
                {
                    txtFertCode.ReadOnly = false;
                    txtSAPSTNNo.ReadOnly = false;
                    txtSAPSTNDate.ReadOnly = false;
                    //if (txtRegNo.Text == "")
                    //{
                    txtRegNo.ReadOnly = false;
                    //}
                    //else
                    //{
                    //    txtRegNo.ReadOnly = true;  
                    //}

                    // // if (txtSAPSTNNo.Text == "" && txtSAPSTNDate.Text == "")
                    ////  {
                    //      txtSAPSTNNo.ReadOnly = false;
                    //      txtSAPSTNDate.ReadOnly = false;
                    ////  }
                    // // else
                    ////  {
                    //     // txtSAPSTNNo.ReadOnly = true;
                    //     // txtSAPSTNDate.ReadOnly = true;
                    ////  }

                    btnSave.Visible = true;
                    btnGenerate.Visible = true;
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        btnUpdate.Visible = true;
                    }
                    else
                    {
                        btnUpdate.Visible = false;
                    }
                }
                else
                {
                    txtFertCode.ReadOnly = true;
                    btnSave.Visible = false;
                    txtSAPSTNNo.ReadOnly = true;
                    txtSAPSTNDate.ReadOnly = true;
                    btnGenerate.Visible = false;
                    txtRegNo.ReadOnly = true;
                    btnUpdate.Visible = false;
                }
                if (iUserId == 226)
                {
                    btnGenerate.Visible = true;
                    btnSave.Visible = true;
                    txtRegNo.ReadOnly = false;

                }
                // iChassisID = Func.Convert.iConvertToInt(txtID.Text);
                //Megha13062012 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ClearChassisHeader()
        {
            txtChassisNo.Text = "";
            txtEngineNo.Text = "";
            txtRegNo.Text = "";
            txtFertCode.Text = "";
            txtModelName.Text = "";
           // txtModelGroup.Text = "";
            txtModelCategory.Text = "";
            txtInWarranty.Text = "";
            txtInAggreWarranty.Text = "";
            txtInExtendedWarranty.Text = "";
            txtInAdditionalWarranty.Text = "";
            txtInAMC.Text = "";
            txtCoupanNo.Text = "";
            txtInKAM.Text = "";
            txtFloatFlag.Text = "";
            txtTheftFlag.Text = "";
            txtVUO.Text = "";
            txtCouponsBlocked.Text = "";
            txtSAPInvoiceNo.Text = "";
            txtSAPInvoiceDate.Text = "";
            txtSAPInvoiceAmt.Text = "";
            txtDANo.Text = "";
            txtSellDealCode.Text = "";
            txtSellDealName.Text = "";
            txtSpareCode.Text = "";
            txtHDCode.Text = "";
            txtDirectCustomer.Text = "";
            txtSAPSTNNo.Text = "";
            txtSAPSTNDate.Text = "";
            txtPDIDealerCode.Text = "";
            txtChangeFertCode.Text = "";
            txtChangeModelName.Text = "";


        }
        private void ClearChassisService()
        {
            txtTotPart.Text = "0.00";
            txtTotLabour.Text = "0.00";
            txtTotLubricant.Text = "0.00";
            txtTotSublet.Text = "0.00";
        }
        private void ClearWarranty_AMC()
        {
            txtWarEndDate.Text = "";
            txtExtWarStarDate.Text = "";
            txtAggreWarEndDate.Text = "";
            txtExtWarEndDate.Text = "";

            txtAddWarStartDate.Text = "";
            txtAddWarEndDate.Text = "";

            txtAMCAggreNo.Text = "";
            txtAMCAggreType.Text = "";
            txtAMCStartDate.Text = "";
            txtAMCEndDate.Text = "";
            //Megha 07102013
            txtSAStartKM.Text = "";
            txtSAEndKM.Text = "";
            //Megha 07102013
        }
        private void ClearAggregate()
        {
            txtBatteryNo.Text = "";
            txtBatteryMake.Text = "";
            txtFIPNo.Text = "";
            txtFIPMake.Text = "";
            txtFrontAxleNo.Text = "";
            txtFrontAxleMake.Text = "";
            txtRearAxle1No.Text = "";
            txtRearAxle1Make.Text = "";
            txtRearAxle2No.Text = "";
            txtRearAxle2Make.Text = "";
            txtRearAxle3No.Text = "";
            txtRearAxle3Make.Text = "";
            txtTransNo.Text = "";
            txtTransMake.Text = "";
            txtGearBoxNo.Text = "";
            txtGearBoxMake.Text = "";
        }
        private void ClearChassisTyre()
        {
            txtFrontLeftTyreNo.Text = "";
            txtFrontRightTyreMake.Text = "";
            txtFrontRightTyreNo.Text = "";
            txtFrontRightTyreMake.Text = "";

            txtRearLeftOuterTyreAxle1No.Text = "";
            txtRearLeftOuterTyreAxle1Make.Text = "";
            //txtRearLeftInnerTyreAxle1No.Text = "";
            txtRearLeftInnerTyreAxle1Make.Text = "";
            txtRearRightOuterTyreAxle1Make.Text = "";
            txtRearRightInnerTyreAxle1No.Text = "";
            txtRearRightInnerTyreAxle1Make.Text = "";

            txtRearLeftOuterTyreAxle2No.Text = "";
            txtRearLeftOuterTyreAxle2Make.Text = "";
            txtRearLeftInnerTyreAxle2No.Text = "";
            txtRearLeftInnerTyreAxle2Make.Text = "";
            txtRearRightOuterTyreAxle2No.Text = "";
            txtRearRightOuterTyreAxle2Make.Text = "";
            txtRearRightInnerTyreAxle2No.Text = "";
            txtRearRightInnerTyreAxle2Make.Text = "";

            txtRearLeftOuterTyreAxle3No.Text = "";
            txtRearLeftOuterTyreAxle3Make.Text = "";
            txtRearLeftInnerTyreAxle3No.Text = "";
            txtRearLeftInnerTyreAxle3Make.Text = "";
            txtRearRightOuterTyreAxle3No.Text = "";
            txtRearRightOuterTyreAxle3Make.Text = "";
            txtRearRightInnerTyreAxle3No.Text = "";
            txtRearRightInnerTyreAxle3Make.Text = "";
        }
        private void FillSelectionGrid()
        {
            try
            {

                SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("Chassis No");
                SearchGrid.AddToSearchCombo("Engine No");
                //SearchGrid.AddToSearchCombo("Reg No");
                SearchGrid.AddToSearchCombo("Model Code");
                SearchGrid.AddToSearchCombo("Model Name");
                //SearchGrid.AddToSearchCombo("Model Group");
                SearchGrid.AddToSearchCombo("Model Category");
                SearchGrid.AddToSearchCombo("Customer Name");
                // SearchGrid.AddToSearchCombo("Direct Customer");
                //  SearchGrid.AddToSearchCombo("Ins Date");
                SearchGrid.sModelPart = (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 4) ? "E" : (sDealerCode.Trim().StartsWith("R")) ? "R": "";
                SearchGrid.iDealerID = (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 4) ? 99 : -1;
                SearchGrid.sSqlFor = "Chassis";
                SearchGrid.sGridPanelTitle = "Chassis List";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        private void EnableDisable()
        {
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
        }
        protected void gvFSDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvFSDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            LinkButton lnkSelectPart = (LinkButton)e.Row.FindControl("lnkServName");
            Label lblJobcardNo = (Label)e.Row.FindControl("lblJobcardNo");
            Label lblDealerID = (Label)e.Row.FindControl("lblDealerID");
            if (lnkSelectPart != null && lblJobcardNo != null && lblDealerID != null)
                lnkSelectPart.Attributes.Add("onclick", "return ShowServiceHistory1(this,'" + lblDealerID.Text + "','" + lblJobcardNo.Text + "');");

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            clsChassis objchassis = new clsChassis();
            DataSet ds = new DataSet();

            //DataSet dschassisId = new DataSet();
            //dschassisId = objchassis.GetchassisID(txtChassisNo.Text);

            //if (dschassisId.Tables[0].Rows.Count > 0)
            //{
            //    iChassisID =Convert.ToInt32(dschassisId.Tables[0].Rows[0]["ID"]);
            //}


            iChassisID = Func.Convert.iConvertToInt(txtID.Text);
            SearchGrid.iID = iChassisID;
            ds = objchassis.GetFertCodeAndServicePolicy(txtFertCode.Text);

            if (iUserId == 321 || iUserId == 228 || iUserId == 22)
            {

                if (ds.Tables[0].Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Entered Fert Not Present in DCS Master.');</script>");
                    goto Last;
                }


                if (ds.Tables[1].Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Service Policy Not Present for Entered Fert.');</script>");
                    goto Last;
                }
            }


            if (objchassis.bSaveFertCodeDetails(iChassisID, Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]), txtSAPSTNNo.Text, txtSAPSTNDate.Text, txtRegNo.Text) == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
            }

        Last: ;

        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DataSet dsDealerBefore = new DataSet();

            clsChassis objchassis = new clsChassis();

            dsDealerBefore = objchassis.GetDealerforChassisMaster();
            string FilePathBefore = Func.Convert.sConvertToString(dsDealerBefore.Tables[1].Rows[0]["FilePath"]);
            string SourceFilePathBefore = FilePathBefore + "\\1\\Masters\\SingleChassis";
            DirectoryInfo sourceBefore = new DirectoryInfo(SourceFilePathBefore);

            // source.Delete(true);
            //FileInfo[] filesDeletesBefore = sourceBefore.GetFiles();
            //foreach (FileInfo fileDeleteBefore in filesDeletesBefore)
            //{
            //    fileDeleteBefore.Delete();
            //}

            if (sourceBefore.Exists)
            {
                sourceBefore.Delete(true);
            }


            // clsChassis objchassis = new clsChassis();
            DataSet ds = new DataSet();

            //DataSet dschassisId = new DataSet();
            //dschassisId = objchassis.GetchassisID(txtChassisNo.Text);

            //if (dschassisId.Tables[0].Rows.Count > 0)
            //{
            //    iChassisID =Convert.ToInt32(dschassisId.Tables[0].Rows[0]["ID"]);
            //}


            iChassisID = Func.Convert.iConvertToInt(txtID.Text);
            SearchGrid.iID = iChassisID;

            if (objchassis.bGenearteChassisXML(iChassisID) == true)
            {
                XMLMoveToALLDealer();
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Chassis and Coupon XML Generated');</script>");

            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Chassis and Coupon XML Not Generated');</script>");
            }

        }

        private void XMLMoveToALLDealer()
        {
            DataSet dsDealer = new DataSet();
            clsChassis objchassis = new clsChassis();
            dsDealer = objchassis.GetDealerforChassisMaster();
            string FilePath = Func.Convert.sConvertToString(dsDealer.Tables[1].Rows[0]["FilePath"]);
            string SourceFilePath = FilePath + "\\1\\Masters\\SingleChassis";
            DirectoryInfo source = new DirectoryInfo(SourceFilePath);
            for (int cnt = 0; cnt < dsDealer.Tables[0].Rows.Count; cnt++)
            {

                if (Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["DealerType"]) == "S")
                {
                    DestinationFilePath = FilePath + "\\" + Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["Dealer_ID"]) + "\\Masters\\Spares";
                }
                else if (Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["DealerType"]) == "V")
                {
                    DestinationFilePath = FilePath + "\\" + Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["Dealer_ID"]) + "\\Masters\\Vehicles";
                }
                else if (Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["DealerType"]) == "B")
                {
                    DestinationFilePath = FilePath + "\\" + Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["Dealer_ID"]) + "\\Masters\\Spares";
                }
                DealerId = Func.Convert.sConvertToString(dsDealer.Tables[0].Rows[cnt]["Dealer_ID"]);
                DirectoryInfo destination = new DirectoryInfo(DestinationFilePath);

                if (!destination.Exists)
                {
                    destination.Create();
                }

                // Copy all files.         
                FileInfo[] files = source.GetFiles();
                foreach (FileInfo file in files)
                {
                    File.Copy(SourceFilePath + "\\" + file, DestinationFilePath + "\\" + file);//.CopyTo(Path.Combine(destination.FullName, file.Name));
                    FileInfo fleInfo = new FileInfo(DestinationFilePath + "\\" + file);
                    var newFileName = fleInfo.Name.Replace("1", DealerId);
                    File.Move(DestinationFilePath + "\\" + file, DestinationFilePath + "\\" + newFileName);

                }


            }
            //  source.Delete(true);
            //FileInfo[] filesDeletes = source.GetFiles();
            //foreach (FileInfo fileDelete in filesDeletes)
            //{
            //    fileDelete.Delete(); 
            //}

            if (source.Exists)
            {
                source.Delete(true);
            }

        }
        //private static string GetNewFileName(FileInfo fleInfo)
        //{
        //    var shortDate = fleInfo.Replace("1", DealerId);
        //    var timeInMilliSec = DateTime.Now.Millisecond.ToString();
        //    var format = string.Format("{0}_{1}", shortDate, timeInMilliSec);
        //    var extension = ".txt";
        //    return Path.Combine(fleInfo.DirectoryName,
        //string.Concat(fleInfo.Name.Split('.')[0], "_", format, extension));
        //}


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            clsChassis objchassis = new clsChassis();
            FillDetailsFromGrid();
            for (int cnt = 0; cnt < dtDetails.Rows.Count; cnt++)
            {
                if (objchassis.bUpdateCouponDetails(Func.Convert.iConvertToInt(dtDetails.Rows[cnt]["CouponID"]), Func.Convert.sConvertToString(dtDetails.Rows[cnt]["CouponNo"]), Func.Convert.sConvertToString(dtDetails.Rows[cnt]["LSPSD"]), Func.Convert.sConvertToString(dtDetails.Rows[cnt]["MSPSD"])) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");

                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }

            }
            SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iChassisID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();


        }
        private void FillDetailsFromGrid()
        {
            DataRow dr;
            dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("CouponID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("CouponNo", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("LSPSD", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("MSPSD", typeof(string)));

            for (int iRowCnt = 0; iRowCnt < gvCouponDetails.Rows.Count; iRowCnt++)
            {

                dr = dtDetails.NewRow();
                //dr["SRNo"] = "1";            
                dr["CouponID"] = Func.Convert.iConvertToInt((gvCouponDetails.Rows[iRowCnt].FindControl("txtCouponID") as TextBox).Text);
                dr["CouponNo"] = Func.Convert.sConvertToString((gvCouponDetails.Rows[iRowCnt].FindControl("txtCouponNo") as TextBox).Text);
                dr["LSPSD"] = Func.Convert.sConvertToString((gvCouponDetails.Rows[iRowCnt].FindControl("txtLSPSD") as TextBox).Text);
                dr["MSPSD"] = Func.Convert.sConvertToString((gvCouponDetails.Rows[iRowCnt].FindControl("txtMSPSD") as TextBox).Text);

                dtDetails.Rows.Add(dr);
                dtDetails.AcceptChanges();

            }
        }
    }
}