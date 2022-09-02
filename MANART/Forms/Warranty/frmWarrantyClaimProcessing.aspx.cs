using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Script;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using MANART_BAL;
using MANART_DAL;
//Sujata  02062011
using System.IO;
//Sujata  02062011

namespace MANART.Forms.Warranty
{
    public partial class frmWarrantyClaimProcessing : System.Web.UI.Page
    {
        private int iClaimID = 0;
        private clsWarranty.enmClaimType ValenmFormUsedFor;
        string sDealerId = "";
        int iUserId = 0;
        int iUserClaimSlabId = 0;
        string sRequestOrClaim = "";
        int iUserRoleId = 0;
        string sDomestic_Export = "";

        //Sujata 12012011
        private DataTable dtFileAttach = new DataTable();
        //Sujata 12012011
        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    string viewState = Request.Form["__VSTATE"];
        //    byte[] bytes = Convert.FromBase64String(viewState);
        //    bytes = clsCompress.Decompress(bytes);
        //    LosFormatter formatter = new LosFormatter();
        //    return formatter.Deserialize(Convert.ToBase64String(bytes));
        //}

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    LosFormatter formatter = new LosFormatter();
        //    StringWriter writer = new StringWriter();
        //    formatter.Serialize(writer, viewState);
        //    string viewStateString = writer.ToString();
        //    byte[] bytes = Convert.FromBase64String(viewStateString);
        //    bytes = clsCompress.Compress(bytes);
        //    ClientScript.RegisterHiddenField("__VSTATE", Convert.ToBase64String(bytes));
        //}
        protected void Page_Init(object sender, EventArgs e)
        {
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
            iClaimID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
            sRequestOrClaim = Func.Convert.sConvertToString(Request.QueryString["RequestOrClaim"]);

            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            iUserClaimSlabId = Func.Convert.iConvertToInt(Session["UserClaimSlabID"]);
            iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);

            txtRequestOrClaim.Text = sRequestOrClaim;
            txtRequestOrClaim.Style.Add("display", "none");



            if (!IsPostBack)
            {
                Session["ComplaintsDetails"] = null;
                Session["InvestigationDetails"] = null;
                Session["PartDetails"] = null;
                Session["LabourDetails"] = null;
                Session["LubricantDetails"] = null;
                Session["SubletDetails"] = null;

                //Location.SetControlValue();
                //sDealerId = Location.iDealerId.ToString();
                FillCombo();
                SetDocumentDetails();
                if (iClaimID != 0)
                {
                    GetDataAndDisplay();
                }

                string strReportpath;
                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                lblWarrantyHistroy.Attributes.Add("onClick", "return ShowChassisWDtls('" + txtchassisID.Text + "','" + strReportpath + "');");
                //Megha 27082013 Warranty claim Report 
                btnPrint.Attributes.Add("onClick", "return ShowReport(40,'" + strReportpath + "');");
                //Megha 27082013
                btnBack.Attributes.Add("onClick", "return CloseWarrantyClaimProsseingWindow();");
                if (txtDomestic_Export.Text == "D")
                {
                    btnPrint.Style.Add("display", "none");
                    if (txtRequestID.Text != "" && sRequestOrClaim == "C")
                    {
                        lnkSelectRequest.Attributes.Add("onClick", "return ShowClaimRequestDtls('" + iClaimID + "');");
                        //lnkSelectRequest.Style.Add("display", ""); //Temporary Commented as per discuss with Vikram Sir 09022017
                        lnkSelectRequest.Style.Add("display", "none");
                    }
                    else
                        lnkSelectRequest.Style.Add("display", "none");

                    lblServiceHistroy.Attributes.Add("onClick", "return ShowClaimHistoryDtls('" + txtchassisID.Text + "','" + strReportpath + "');");
                    if (iUserRoleId != 18)
                    {
                        trCSMRemark.Style.Add("display", "");
                        trRSMRemark.Style.Add("display", "");
                    }
                    else
                    {
                        trCSMRemark.Style.Add("display", "none");
                        trRSMRemark.Style.Add("display", "none");
                    }
                }
                else
                {
                    //btnPrint.Style.Add("display", "");
                    btnPrint.Style.Add("display", "none");
                    if (txtRequestID.Text != "" && txtRequestID.Text != "0" && sRequestOrClaim == "C")
                    {
                        lnkSelectRequest.Attributes.Add("onClick", "return ShowClaimRequestDtls('" + iClaimID + "');");
                        //lnkSelectRequest.Style.Add("display", "");
                        lnkSelectRequest.Style.Add("display", "none");                        
                    }
                    else
                        lnkSelectRequest.Style.Add("display", "none");

                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "");
                    if (sRequestOrClaim == "C")
                    {
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','P','W');");
                    }
                    else
                    {
                        lblServiceHistroy.Attributes.Add("onClick", "return ShowWServiceHistory('" + txtID.Text + "','P','R');");

                    }
                }
                ExpirePageCache();
            }
            //ToolbarC.iIdForConfirm = 1;
            //sDealerId = Location.iDealerId.ToString();                
            //VHP_Warranty start Display Warranty Claim
            //if (iClaimID != 0)
            //{
            //    GetDataAndDisplay();
            //}
            //VHP_Warranty END Display Warranty Claim

        }   
        protected void Page_Load(object sender, EventArgs e)
        {
            //sDealerId = Location.iDealerId.ToString();  
        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
        }
        // set Document text 
        private void SetDocumentDetails()
        {
            //System.Threading.Thread.Sleep(2000);
            //Common

            txtClaimRevNo.Style.Add("display", "none");
            txtClaimRevNo.Text = "0";

            lblRefClaimNo.Style.Add("display", "none");
            lblRefClaimDate.Style.Add("display", "none");

            txtRefClaimNo.Style.Add("display", "none");
            txtRefClaimDate.Style.Add("display", "none");

            txtRefClaimID.Style.Add("display", "none");

            // Fields for Request        
            txtRequestNo.Style.Add("display", "none");
            lblRequestNo.Style.Add("display", "none");
            lblRequestDate.Style.Add("display", "none");
            txtRequestDate.Style.Add("display", "none");
            txtRequestID.Style.Add("display", "none");

            // To Hide the buttons
            btnApprove.Style.Add("display", "none");
            btnReject.Style.Add("display", "none");
            btnReturn.Style.Add("display", "none");
            

            if (iUserRoleId == 4)// if user is Csm
            {
                txtCSMRemark.ReadOnly = false;
                txtRSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 3)// if user is Asm
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = false;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 2)// if user is Rsm
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = false;
                txtHeadRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 1)// if user is Head
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = false;
                txtSHQRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                trHeadRetailRemark.Style.Add("display", "none");
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 13)// if user is Head Retail
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = false;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                trHeadRemark.Style.Add("display", "none");
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 14)// if user is Head Sale Marketing After Marketing Remark
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = false;
                txtSAResourceRemark.ReadOnly = true;
                trHeadRemark.Style.Add("display", "none");
                if (sRequestOrClaim != "C")
                {
                    trSHQRemark.Style.Add("display", "none");
                    trSHQRRemark.Style.Add("display", "none");
                    trCSMRemark.Style.Add("display", "none");
                    trRSMRemark.Style.Add("display", "none");
                }
            }
            else if (iUserRoleId == 9)
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = false;
                txtSHQRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                //trSHQRemark.Style.Add("display", "none");
            }
            else if (iUserRoleId == 10)
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = false;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                txtSAResourceRemark.ReadOnly = true;
                //trSHQRRemark.Style.Add("display", "none");
            }
            else if (iUserRoleId == 18)
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                txtRSMRemark.ReadOnly = true;
                txtHeadRemark.ReadOnly = true;
                txtSHQRemark.ReadOnly = true;
                txtSHQRRemark.ReadOnly = true;
                txtHeadRetailRemark.ReadOnly = true;
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
                //txtSAResourceRemark.ReadOnly = false;
                txtSAResourceRemark.ReadOnly = true;

                trCSMRemark.Style.Add("display", "none");
                trASMRemark.Style.Add("display", "none");
                trRSMRemark.Style.Add("display", "none");
                trHeadRemark.Style.Add("display", "none");
                trSHQRemark.Style.Add("display", "none");
                trSHQRRemark.Style.Add("display", "none");
                trHeadRetailRemark.Style.Add("display", "none");
                trHeadSaleMkgAfterMkgRemark.Style.Add("display", "none");
                trSHQRRemark.Style.Add("display", "none");
                //trSAResourceRemark.Style.Add("display", "");
                trSAResourceRemark.Style.Add("display", "none");
            }
            trHeadRetailRemark.Style.Add("display", "none");
            trHeadSaleMkgAfterMkgRemark.Style.Add("display", "none");
        }
        // FillCombo
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpRouteType, clsCommon.ComboQueryType.RouteType, 0);
            Func.Common.BindDataToCombo(drpReason, clsCommon.ComboQueryType.ClaimReason, 0);
        }





        private bool bValidateRecord()
        {
            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            //if (txtDocDate.Text == "")
            //{
            //    sMessage = sMessage + "\\n Enter the document date.";
            //    bValidateRecord = false;
            //}

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }


        //ToSave Record
        private void SaveRecord()
        {

        }

        private void GetDataAndDisplay()
        {
            clsWarranty ObjWarranty = new clsWarranty();
            DataSet ds = new DataSet();
            if (iClaimID != 0)
            {
                //VHP_Warraty Claim Start
                //if (sRequestOrClaim == "R")
                //{
                //    ds = ObjWarranty.GetWarrantyClaim(iClaimID, "GoodwillConfirm", 0);
                //    lblTitle.Text = " Goodwill Request";
                //    lblClaimNo.Text = "Request No.";
                //    lblClaimDate.Text = "Request Date";
                //}
                //else if (sRequestOrClaim == "C")
                //{
                //    ds = ObjWarranty.GetWarrantyClaim(iClaimID, "RegularConfirm", 0);
                //    lblTitle.Text = " Warranty Claim(PRWC) ";
                //}
                if (sRequestOrClaim == "R")
                {
                    ds = ObjWarranty.GetWarrantyClaim(iClaimID, "GoodwillConfirm", 0, iUserRoleId);
                    lblTitle.Text = " Goodwill Request";
                    lblClaimNo.Text = "Request No.";
                    lblClaimDate.Text = "Request Date";
                    trWarranty.Style.Add("display", "none");
                    //btnReturn.Style.Add("display", "none");  
                }
                else if (sRequestOrClaim == "C")
                {

                    ds = ObjWarranty.GetWarrantyClaim(iClaimID, "RegularConfirm", 0, iUserRoleId);
                    if (iUserRoleId != 18)
                        lblTitle.Text = " Warranty Claim ";
                    else
                    {
                        lblTitle.Text = " Warranty Claim ";
                        lblClaimNo.Text = "Claim No.:";
                    }
                    trWarranty.Style.Add("display", "");
                }
                else if (sRequestOrClaim == "HR")
                {
                    ds = ObjWarranty.GetWarrantyClaim(iClaimID, "GoodwillConfirm", 0, iUserRoleId);
                    lblTitle.Text = " High Value Request";
                    trWarranty.Style.Add("display", "none");
                    //btnReturn.Style.Add("display", "none");  
                }
                //VHP_Warraty Claim Start
                DisplayData(ds);
            }
            ds = null;
            ObjWarranty = null;
        }

        // Display Data 
        private void DisplayData(DataSet ds)
        {
            bool bEnableControls = true;
            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }

            //Display Header        
            lblVehicleHistory.Style.Add("display", "none");
            txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);

            iClaimID = Func.Convert.iConvertToInt(txtID.Text);
            sDealerId = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);
            hdnDealerID.Value = sDealerId.Trim();
            //sujata 12012011
            txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Spares_Code"]);
            //sujata 12012011
            //Sujata 02062011
            txtDealerAID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);
            //Sujata 02062011
            txtClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_no"]);
            txtClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]);
            txtClaimTypeID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
            sDomestic_Export = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Domestic_Export"]);
            hdnIsAggregate.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Aggregate"]);
            txtDomestic_Export.Text = sDomestic_Export;
            hdnISDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
            hdnInvType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvType"]);

            hdnServVanID.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ServVanID"]);
            txtFrmLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["From_Location"]);
            txtToLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["To_Location"]);
            txtStKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["StKms"]);
            txtEndKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EndKms"]);

            ServVan1.Style.Add("display", (Func.Convert.iConvertToInt(hdnServVanID.Value) > 0) ? "" : "none");
            ServVan2.Style.Add("display", (Func.Convert.iConvertToInt(hdnServVanID.Value) > 0) ? "" : "none");
                        
            hdnOtherClaimId.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["OtherClaimID"]);

            lnkOtherClaim.Style.Add("display", (Func.Convert.iConvertToInt(hdnOtherClaimId.Value) > 0 && sRequestOrClaim == "C" && ((hdnInvType.Value == "P") || (hdnInvType.Value == "L")) && sDomestic_Export =="D") ? "" : "none");
            lnkOtherClaim.Text = (hdnInvType.Value == "P") ? "Labour Warranty Claim Details" : ((hdnInvType.Value == "L") ? "Part Warranty Claim Details" : "" );

            string strReportpath;
            strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
            lnkOtherClaim.Attributes.Add("onclick", "return ShowReport_OtherClaim(this,'" + strReportpath + "','" + Func.Convert.sConvertToString(Session["UserRole"]) + "');");
            
            if (Session["ClaimTypes"] != null)
            {
                ValenmFormUsedFor = (clsWarranty.enmClaimType)Session["ClaimTypes"];
            }
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest && sDomestic_Export == "D")
            {
                if (iUserRoleId != 2 && iUserRoleId != 9)
                    PRequestDetails.Enabled = false;
                //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                //VHP_Warraty  End Code Change         
                if (txtClaimTypeID.Text == "2" || txtClaimTypeID.Text == "22")//Technical Goodwill
                {
                    OptGoodwillType.Items[0].Selected = true;
                }
                else if (txtClaimTypeID.Text == "8" || txtClaimTypeID.Text == "16")//Commercial Goodwill
                {
                    OptGoodwillType.Items[1].Selected = true;
                }

                lblClaimNo.Text = "Request No.";
                lblClaimDate.Text = "Request Date";
            }
            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal && sDomestic_Export == "D" && (txtClaimTypeID.Text == "19" || txtClaimTypeID.Text == "20"))
            {
                if (iUserRoleId != 9)
                    PRequestDetails.Enabled = false;
                //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                //VHP_Warraty  End Code Change         
                if (txtClaimTypeID.Text == "19")//HD Technical Goodwill
                {
                    OptGoodwillType.Items[0].Selected = true;
                }
                else if (txtClaimTypeID.Text == "20")//HD Commercial Goodwill
                {
                    OptGoodwillType.Items[1].Selected = true;
                }

                OptGoodwillType.Items[0].Text = "HD Technical Goodwill";
                OptGoodwillType.Items[1].Text = "HD Commercial Goodwill";
                lblApprovalNo.Text = "Warranty Invoice No :";
                lblApprovalDt.Text = "Warranty Invoice Date :";
            }
            else
            {
                PRequestDetails.Style.Add("display", "none");
                lblApprovalNo.Text = "Warranty Invoice No :";
                lblApprovalDt.Text = "Warranty Invoice Date :";
            }

            lblApprovalNo.Text = (sRequestOrClaim == "C") ? "Warranty Invoice No :" : "Approval No.:";
            lblApprovalDt.Text = (sRequestOrClaim == "C") ? "Warranty Invoice Date :" : "Approval Date:";            

            txtClaimName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Name"]);
            txtClaimRevNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Rev_No"]);
            txtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_code"]);
            txtModelCode.Attributes.Add("readonly", "readonly");
            txtModelDescription.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Name"]);
            txtModelDescription.Attributes.Add("readonly", "readonly");

            txtChangeFertCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Change_Model_code"]);
            txtChangeFertCode.Attributes.Add("readonly", "readonly");
            txtChangeFertName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Change_Model_Name"]);
            txtChangeFertName.Attributes.Add("readonly", "readonly");

            txtModelDescription.Style.Add("display", "");
            lblModelCode.Style.Add("display", "");
            lblModelName.Style.Add("display", "");
            txtchassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ChassisHdrID"]);
            txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
            txtChassisNo.Attributes.Add("readonly", "readonly");
            txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
            txtEngineNo.Attributes.Add("readonly", "readonly");
            txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vehicle_No"]);
            txtVehicleNo.Attributes.Add("readonly", "readonly");
            txtCustomerName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);
            txtCustomerName.Attributes.Add("readonly", "readonly");
            txtCustomerAddress.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Address"]);
            txtCustomerAddress.Attributes.Add("readonly", "readonly");

            txtAggreagateNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AggregateNo"]);
            lblChassisNo.Text = (txtAggreagateNo.Text == "") ? "Chassis No.:" : "Aggregate No.:";

            lblEngineNo.Visible = (txtAggreagateNo.Text == "") ? true : false;
            txtEngineNo.Visible = (txtAggreagateNo.Text == "") ? true : false;            
            txtChassisNo.Visible = (txtAggreagateNo.Text == "") ? true : false;
            txtAggreagateNo.Visible = (txtAggreagateNo.Text == "") ? false : true;

            //=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_no"];
            //=Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_date"];
            txtInstallationDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["INS_date"]);
            txtInstallationDate.Attributes.Add("readonly", "readonly");
            txtOdometer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Odometer"]);
            txtOdometer.Attributes.Add("readonly", "readonly");
            txtHrsReading.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Hrs_reading"]);
            txtHrsReading.Attributes.Add("readonly", "readonly");
            txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VECV_Inv_no"]);
            txtInvoiceNo.Attributes.Add("readonly", "readonly");
            txtInvoiceDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VECV_Inv_date"]);
            txtInvoiceDate.Attributes.Add("readonly", "readonly");
            //Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_No"];
            //Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Date"];
            txtRepairOrderNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Order_No"]);
            txtRepairOrderNo.Attributes.Add("readonly", "readonly");
            txtRepairOrderDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Order_Date"]);
            txtRepairOrderDate.Attributes.Add("readonly", "readonly");
            txtRepairCompleteDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Repair_Complete_Date"]);
            txtRepairCompleteDate.Attributes.Add("readonly", "readonly");
            txtFailureDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Failure_Date"]);
            txtFailureDate.Attributes.Add("readonly", "readonly");
            drpRouteType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Root_Type_ID"]);
            txtTransporterName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Transporter_Name"]);


            txtRequestID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_ID"]);
            txtRequestNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]);// goodwill Request No 
            txtRequestDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_Date"]);// goodwill Request Date       

            txtRefClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_no"]);
            txtRefClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_date"]);

            txtApprovalNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_No"]);
            txtApprovalDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approval_Date"]);

            txtCSMRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_Remark"]);
            txtASMRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_Remark"]);
            txtRSMRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_Remark"]);
            txtHeadRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_Remark"]);
            txtHeadRetailRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadRetail_Remark"]);
            txtHeadSaleMkgAfterMkgRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadSaleMkgAfterMkg_Remark"]);
            txtSHQRRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SHQResource_Remark"]);
            txtSHQRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SHQ_Remark"]);
            hdnIsSHQResource.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]);
            hdnIsSHQ.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]);

            hdnRejectCount.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RejectCount"]);
            hdnReturnCount.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReturnCount"]);

            txtJCDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Job_ConfirmDate"]);
            txtWCCDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Warr_Confirmdate"]);
            txtDCSPostedDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Confirm_Date"]);

            hdnCurrency.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Currency"]);

            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]) == "Y")//csm
            {
                if (iUserRoleId == 4)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtCSMRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y")//Asm
            {
                if (iUserRoleId == 3)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtASMRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y")//Rsm
            {
                if (iUserRoleId == 2)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtRSMRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_SubmitYN"]) == "Y")//Head
            {
                if (iUserRoleId == 1)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtHeadRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadRetail_SubmitYN"]) == "Y")//Head Retail 
            {
                if (iUserRoleId == 13)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtHeadRetailRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadSaleMkgAfterMkg_SubmitYN"]) == "Y")//Head Sale Marketing  After Marketing
            {
                if (iUserRoleId == 14)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtHeadSaleMkgAfterMkgRemark.ReadOnly = true;
            }

            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SHQResource_SubmitYN"]) == "Y")//SHQResource 
            {
                if (iUserRoleId == 9)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtSHQRRemark.ReadOnly = true;
            }
            if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SHQ_SubmitYN"]) == "Y")//SHQ
            {
                if (iUserRoleId == 10)
                {
                    btnSave.Style.Add("display", "none");
                    btnSubmit.Style.Add("display", "none");
                }
                txtSHQRemark.ReadOnly = true;
            }

            //Sujata 02062011
            //GetFinalApprovalUserRoleId(Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Slab_ID"]));
            //Sujata 02062011

            txtPartAmount.Style.Add("display", "none");
            txtLabourAmount.Style.Add("display", "none");
            txtLubricantAmount.Style.Add("display", "none");
            txtSubletAmount.Style.Add("display", "none");
            txtClaimAmt.Style.Add("display", "none");
            //VHP_Warranty Start
            if ((sRequestOrClaim == "R" || sRequestOrClaim == "HR") && (sDomestic_Export == "D"))
            {
                bEnableControls = true;
                //if (iUserClaimSlabId == Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Slab_ID"]))// approve            
                btnApprove.Style.Add("display", "");
                btnReject.Style.Add("display", "");
                btnReturn.Style.Add("display", "");
                btnSubmit.Style.Add("display", "none");
                lblClaimTy.Text = "Request Type.:";
                lblClaimNo.Text = "Request No.:";
                lblClaimDate.Text = "Request Date:";              
            }

            //VHP_Warranty End

            if (iUserClaimSlabId == Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Slab_ID"]))
            {
                bEnableControls = true;
                //if (iUserClaimSlabId == Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Slab_ID"]))// approve            
                btnApprove.Style.Add("display", "");
                btnReject.Style.Add("display", "");
                btnReturn.Style.Add("display", "");
                btnSubmit.Style.Add("display", "none");
                //drpReason.Visible = true;
                btnSave.Style.Add("display", "none");
            }
            else
            {
                bEnableControls = false;
            }

            if (Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Status"]) == 1 || (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P" && (iUserRoleId == 9 || iUserRoleId == 18)) || (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "P" && iUserRoleId == 10))
            {
                btnApprove.Style.Add("display", "");
                btnReject.Style.Add("display", "");
                btnReturn.Style.Add("display", "");
                btnSave.Style.Add("display", "");
                btnSave.Style.Add("display", "none");
                btnSubmit.Style.Add("display", "none");
            }
            else
            {
                btnApprove.Style.Add("display", "none");
                btnReject.Style.Add("display", "none");
                btnReturn.Style.Add("display", "none");
                btnCancel.Style.Add("display", "none");
                btnOK.Style.Add("display", "none");
                btnSubmit.Style.Add("display", "none");
                drpReason.Style.Add("display", "none");
                btnSave.Style.Add("display", "none");

            }



            // Display Complaints Details     
            ComplaintsGrid.DataSource = ds.Tables[1];
            lblComplaintsRecCnt.Text = Func.Common.sRowCntOfTable(ds.Tables[1]);
            ComplaintsGrid.DataBind();


            // Display Investigations Details
            InvestigationsGrid.DataSource = ds.Tables[2];
            lblInvestigationsRecCnt.Text = Func.Common.sRowCntOfTable(ds.Tables[2]);
            InvestigationsGrid.DataBind();

            // Display Parts/Parameter 
            if (Func.Common.iRowCntOfTable(ds.Tables[3]) == 0)
            {
                PParameter.Style.Add("display", "none");
            }
            else
            {
                ParameterGrid.DataSource = ds.Tables[3];
                lblParametrRecCnt.Text = Func.Common.sRowCntOfTable(ds.Tables[3]);
                ParameterGrid.DataBind();
            }

            // Action Taken
            if (Func.Common.iRowCntOfTable(ds.Tables[4]) == 0)
            {
                PAction.Style.Add("display", "none");
            }
            else
            {
                ActionGrid.DataSource = ds.Tables[4];
                lblActionRecCnt.Text = Func.Common.sRowCntOfTable(ds.Tables[4]);
                ActionGrid.DataBind();
            }
            if (sDomestic_Export == "E")
            {
                trWarranty.Style.Add("display", "none");
                BindDataToExportSummary(ds.Tables[5]);
            }
            else if (sDomestic_Export == "D")
            {
                BindDataToDomesticSummary(ds.Tables[5]);
                drpRouteType.Enabled = false;
                txtTransporterName.Enabled = false;
                lblVehicleHistory.Style.Add("display", "none");
            }
            //Sujata 12012011
            // Display Attach file  Details  

            if (PFileAttchDetails.Visible == true)
            {
                dtFileAttach = ds.Tables[6];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();
            }


            // Commented by Shyamal as on 01082012 as per disscussion with Deepti Mam and Vikram Sir

            ////Sujata 12012011
            ////Sujata 02062011
            //if ((Func.Convert.iConvertToInt(txtfinalAppUserRole.Text) == iUserRoleId && txtClaimTypeID.Text == "8") || (iUserRoleId == 4 && (sRequestOrClaim == "C" || txtClaimTypeID.Text == "2")) || ((iUserRoleId == 9 || iUserRoleId == 10) && sRequestOrClaim == "C"))
            //{
            //    trNewAttachment.Style.Add("display", "");
            //    trNewAttachment1.Style.Add("display", "");
            //}
            //else
            //{
            //    trNewAttachment.Style.Add("display", "none");
            //    trNewAttachment1.Style.Add("display", "none");
            //}
            ////Sujata 02062011



            DataTable dtJobcode = new DataTable();
            dtJobcode = ds.Tables[7];
            hdnJobCode.Value = "";
            if (dtJobcode != null)
                if (dtJobcode.Rows.Count > 0)
                    for (int iCount = 0; iCount < dtJobcode.Rows.Count; iCount++)
                    {
                        if (iCount == dtJobcode.Rows.Count - 1)
                            hdnJobCode.Value = hdnJobCode.Value + Func.Convert.sConvertToString(dtJobcode.Rows[iCount]["Job_Code"]);
                        else
                            hdnJobCode.Value = hdnJobCode.Value + Func.Convert.sConvertToString(dtJobcode.Rows[iCount]["Job_Code"]) + ",";
                    }

            if (sDomestic_Export == "D")
            {
                if (iUserRoleId == 4)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "") //&& (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]).Trim() == "" || sRequestOrClaim == "R" || sRequestOrClaim == "HR")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 3)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 2)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 1)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 13)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadRetail_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 14)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadSaleMkgAfterMkg_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 9)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                    //btnReturn.Style.Add("display", "none");
                }
                else if (iUserRoleId == 10)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "P" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                    //btnReturn.Style.Add("display", "none");
                }
                else if (iUserRoleId == 15)
                {
                    bEnableControls = false;
                }
                else if (iUserRoleId == 18)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                    //btnReturn.Style.Add("display", "none");
                }
            }
            else
            {

                if (iUserRoleId == 3)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "N" && (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]).Trim() == "" || sRequestOrClaim == "HR") && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 2)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 1)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_SubmitYN"]) == "N" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                }
                else if (iUserRoleId == 9)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P" && txtTotalAccepted_Amount.Text != "")
                        bEnableControls = true;
                    else
                        bEnableControls = false;
                    //btnReturn.Style.Add("display", "none");
                }

            }

            if (bEnableControls == true)
            {
                MakeEnableDisableControls(true);
            }
            else
            {
                MakeEnableDisableControls(false);
            }

            //DetailsGrid.DataSource = ds.Tables[3];
            //DetailsGrid.DataBind();
            //SetGridControlProperty();
        }

        //Bind Data To Export Summary
        private void BindDataToExportSummary1111(DataTable dt)
        {
            int iLastSrNo = 1;
            int iCurrJobCode = 0;
            int iMainJobCode = 0;
            int iCurrentSrNo = 0;
            string sSrNo = "";
            string[] splSrNo = new string[7];
            DataTable dtCopy = new DataTable();
            dtCopy = dt.Copy();
            int iCurrentColumnIndex = 0;
            int iInsertRowIndex = 0;
            bool bInsertRecordForLastJobCode = false;
            for (iCurrentColumnIndex = 0; iCurrentColumnIndex < dt.Rows.Count; iCurrentColumnIndex++)
            {
                bInsertRecordForLastJobCode = false;
                iCurrJobCode = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["job_code_ID"]);
                iCurrentSrNo = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["SrNo"]);

                if (iCurrentColumnIndex == 0)
                    iMainJobCode = iCurrJobCode;

                if (iMainJobCode == iCurrJobCode)
                {
                    if (sSrNo == "")
                        sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    else
                        sSrNo = sSrNo + "," + Func.Convert.sConvertToString(iCurrentSrNo);

                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        string tmpSrNo = "";
                        splSrNo = sSrNo.Split(',');
                        for (int iCnt = 0; iCnt < 7; iCnt++)
                        {

                            int iRecordCnt = 0;
                            if (iCnt < splSrNo.Length)
                            {
                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }
                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }
                            else if (iCnt >= splSrNo.Length)
                            {

                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }

                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }

                        }
                    }
                }
                else
                {
                StepPrev:
                    ;
                    splSrNo = sSrNo.Split(',');
                    string tmpSrNo = "";
                    for (int iCnt = 0; iCnt < 7; iCnt++)
                    {
                        //if (iCnt == splSrNo.Length)
                        //    tmpSrNo = splSrNo[iCnt - 1];
                        if (iCnt < splSrNo.Length)
                        {
                            if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]))
                            {
                                InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                iInsertRowIndex = iInsertRowIndex + 1;
                                iLastSrNo = iLastSrNo + 1;
                            }
                            else
                            {
                                iInsertRowIndex = iInsertRowIndex + 1;
                                iLastSrNo = iLastSrNo + 1;
                            }
                        }
                        else if (iCnt >= splSrNo.Length)
                        {
                            if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]))
                            {
                                InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                iInsertRowIndex = iInsertRowIndex + 1;
                                iLastSrNo = iLastSrNo + 1;
                            }
                            else
                            {
                                iInsertRowIndex = iInsertRowIndex + 1;
                                iLastSrNo = iLastSrNo + 1;
                            }
                        }
                    }
                    iMainJobCode = iCurrJobCode;
                    sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    iLastSrNo = 1;
                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        iCurrentColumnIndex = iCurrentColumnIndex + 1;
                        goto StepPrev;
                    }
                }
            }
            // Code from Sandeep

            //    if (iMainJobCode == 0)
            //    {
            //        iMainJobCode = iCurrJobCode;
            //        if (iCurrentSrNo != 1)// If Part record not exists for the first job code
            //        {
            //            if (iCurrentSrNo == 5)//add dummy entry for Lubricant, Labor,Part
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);            
            //                iInsertRowIndex = iInsertRowIndex + 3;                        
            //            }
            //            else if (iCurrentSrNo == 4)//add dummy entry for  Labor,Part
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);            
            //                iInsertRowIndex = iInsertRowIndex + 2;
            //            }
            //            else if (iCurrentSrNo == 3)//add dummy entry for Part
            //            {                        
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);
            //                iInsertRowIndex = iInsertRowIndex + 1;
            //            }

            //        }
            //        iLastSrNo++;                
            //    }
            //    if (iMainJobCode == iCurrJobCode)// if job codes are same
            //    {
            //        if (iLastSrNo != iCurrentSrNo)
            //        {
            //            if (iCurrentSrNo == 4)//add dummy entry for Labor
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 1; 
            //            }
            //            else if (iCurrentSrNo == 5)//add dummy entry for Lubricant
            //            {
            //                if (iLastSrNo == 1)//add dummy entry for Labor
            //                {
            //                    InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //                    iInsertRowIndex = iInsertRowIndex + 2;
            //                }
            //                else if (iLastSrNo == 3)//add dummy entry for Lubricants
            //                {
            //                    InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
            //                    iInsertRowIndex = iInsertRowIndex + 1;
            //                }                        
            //            }
            //        }
            //        iLastSrNo =  iCurrentSrNo;
            //    }
            //    else // if job codes are different
            //    {
            //        if (iCurrentSrNo == 1 && (iLastSrNo != 5))
            //        {
            //            bInsertRecordForLastJobCode = true;
            //        }
            //        else if (iCurrentSrNo != 1 )
            //        { 
            //            bInsertRecordForLastJobCode = true;
            //        }
            //        // insert dummy entry for for last job code                
            //        if(bInsertRecordForLastJobCode==true)
            //        {
            //            if (iLastSrNo == 1)// add Record for Sublet,lubricant,labour
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);                        
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 3;  
            //            }
            //            else if (iLastSrNo == 3)// add Record for Sublet,Lubricant
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 2;  
            //            }
            //            else if (iLastSrNo == 4)// add Record for Sublet
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 1;  
            //            }
            //        }
            //        if (iCurrentSrNo == 3)// if Current Srno is for Labor
            //        {
            //            // add Record for Parts for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
            //            iInsertRowIndex = iInsertRowIndex + 1;  
            //        }
            //        else if (iCurrentSrNo == 4)// if Current Srno is for  Lubricant
            //        {                    
            //            // add Record for Parts,Labor for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);                    
            //            iInsertRowIndex = iInsertRowIndex + 2;  
            //        }
            //        else if (iCurrentSrNo == 5)// if Current Srno is for Sublet
            //        {                   
            //            // add Record for Parts,Labor for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricant", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
            //            iInsertRowIndex = iInsertRowIndex + 3;  
            //        }
            //        iMainJobCode = iCurrJobCode;
            //        iLastSrNo = iCurrentSrNo;
            //    }
            //    iInsertRowIndex++;
            //}
            //if (iLastSrNo != 5)
            //{
            //    if (iLastSrNo == 1)//Parts
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo ==3)//Labor
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo == 4)//Lubricant
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);                
            //    }            
            //}

            DetailsGrid.DataSource = dtCopy;
            DetailsGrid.DataBind();
            if (dt.Rows.Count > 0)
                SetGridControlProperty();
        }

        //static string JobType(string value)
        //{
        //    switch (value)
        //    {
        //        case "1":
        //            return "Parts";
        //        case "2":
        //            return "Tax";
        //        case "3":
        //            return "Labor";           
        //        case "4":
        //            return "Lubricant";
        //        case "5":            
        //            return "Sublet";
        //        default:
        //            return null;
        //    }
        //}

        static string JobType(string value)
        {
            switch (value)
            {
                case "1":
                    return "Parts";
                case "2":
                    return "Tax";
                case "3":
                    return "Labor Taxable";
                case "4":
                    return "Sublet";
                case "5":
                    return "Tax(Labor + Sublet)";
                case "6":
                    return "Labor NonTaxable";
                case "7":
                    return "Lubricants";


                default:
                    return null;
            }
        }

        static string JobTypeDom(string value)
        {
            switch (value)
            {
                case "1":
                    return "Parts";
                case "2":
                    return "Tax";
                case "3":
                    return "Labor Taxable";
                case "4":
                    return "Sublet";
                case "5":
                    return "Tax(Labor + Sublet)";
                case "6":
                    return "Labor NonTaxable";
                case "7":
                    return "Lubricants";

                default:
                    return null;
            }
        }

        //Bind Data To Export Summary
        private void BindDataToExportSummary(DataTable dt)
        {
            int iLastSrNo = 1;
            int iCurrJobCode = 0;
            int iMainJobCode = 0;
            int iCurrentSrNo = 0;
            string sSrNo = "";
            string[] splSrNo = new string[7];
            DataTable dtCopy = new DataTable();
            dtCopy = dt.Copy();
            int iCurrentColumnIndex = 0;
            int iInsertRowIndex = 0;
            bool bInsertRecordForLastJobCode = false;
            for (iCurrentColumnIndex = 0; iCurrentColumnIndex < dt.Rows.Count; iCurrentColumnIndex++)
            {
                bInsertRecordForLastJobCode = false;
                iCurrJobCode = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["job_code_ID"]);
                iCurrentSrNo = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["SrNo"]);

                if (iCurrentColumnIndex == 0)
                    iMainJobCode = iCurrJobCode;

                if (iMainJobCode == iCurrJobCode)
                {
                    if (sSrNo == "")
                        sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    else
                        sSrNo = sSrNo + "," + Func.Convert.sConvertToString(iCurrentSrNo);

                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        string tmpSrNo = "";
                        splSrNo = sSrNo.Split(',');
                        for (int iCnt = 0; iCnt < 7; iCnt++)
                        {

                            int iRecordCnt = 0;
                            if (iCnt < splSrNo.Length)
                            {
                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }
                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }
                            else if (iCnt >= splSrNo.Length)
                            {

                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }

                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }

                        }
                    }
                }
                else
                {
                StepPrev:
                    ;
                    splSrNo = sSrNo.Split(',');
                    int iCnt = 0;
                    for (iCnt = 1; iCnt < 8; iCnt++)
                    {
                        if (splSrNo.Contains(Func.Convert.sConvertToString(iCnt)) == false)
                        {
                            InsertEmtryRowToDataTable(dtCopy, iCnt, JobType(Func.Convert.sConvertToString(iCnt)), iMainJobCode, iInsertRowIndex);
                        }
                        iInsertRowIndex = iInsertRowIndex + 1;
                    }
                    //StepPrev:
                    //    ;
                    //    splSrNo = sSrNo.Split(',');
                    //    string tmpSrNo = "";           
                    //    for (iCnt = 1; iCnt < 8; iCnt++)
                    //    {
                    //        if (splSrNo.Contains(Func.Convert.sConvertToString(iCnt)) == false)
                    //        {
                    //            InsertEmtryRowToDataTable(dtCopy, iCnt, JobTypeDom(Func.Convert.sConvertToString(iCnt)), iMainJobCode, iInsertRowIndex);
                    //        }
                    //        iInsertRowIndex = iInsertRowIndex + 1;
                    //    }

                    //    for (int iCnt = 0; iCnt < 7; iCnt++)
                    //    {
                    //        //if (iCnt == splSrNo.Length)
                    //        //    tmpSrNo = splSrNo[iCnt - 1];
                    //        if (iCnt < splSrNo.Length)
                    //        {
                    //            if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]))
                    //            {
                    //                InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                    //                iInsertRowIndex = iInsertRowIndex + 1;
                    //                iLastSrNo = iLastSrNo + 1;
                    //            }
                    //            else
                    //            {
                    //                iInsertRowIndex = iInsertRowIndex + 1;
                    //                iLastSrNo = iLastSrNo + 1;
                    //            }
                    //        }
                    //        else if (iCnt >= splSrNo.Length)
                    //        {
                    //            if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]))
                    //            {
                    //                InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobType(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                    //                iInsertRowIndex = iInsertRowIndex + 1;
                    //                iLastSrNo = iLastSrNo + 1;
                    //            }
                    //            else
                    //            {
                    //                iInsertRowIndex = iInsertRowIndex + 1;
                    //                iLastSrNo = iLastSrNo + 1;
                    //            }
                    //        }
                    //    }


                    iMainJobCode = iCurrJobCode;
                    sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    iLastSrNo = 1;
                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        iCurrentColumnIndex = iCurrentColumnIndex + 1;
                        goto StepPrev;
                    }
                }
            }
            // Code from Sandeep

            //    if (iMainJobCode == 0)
            //    {
            //        iMainJobCode = iCurrJobCode;
            //        if (iCurrentSrNo != 1)// If Part record not exists for the first job code
            //        {
            //            if (iCurrentSrNo == 5)//add dummy entry for Lubricant, Labor,Part
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);            
            //                iInsertRowIndex = iInsertRowIndex + 3;                        
            //            }
            //            else if (iCurrentSrNo == 4)//add dummy entry for  Labor,Part
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);            
            //                iInsertRowIndex = iInsertRowIndex + 2;
            //            }
            //            else if (iCurrentSrNo == 3)//add dummy entry for Part
            //            {                        
            //                InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);
            //                iInsertRowIndex = iInsertRowIndex + 1;
            //            }

            //        }
            //        iLastSrNo++;                
            //    }
            //    if (iMainJobCode == iCurrJobCode)// if job codes are same
            //    {
            //        if (iLastSrNo != iCurrentSrNo)
            //        {
            //            if (iCurrentSrNo == 4)//add dummy entry for Labor
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 1; 
            //            }
            //            else if (iCurrentSrNo == 5)//add dummy entry for Lubricant
            //            {
            //                if (iLastSrNo == 1)//add dummy entry for Labor
            //                {
            //                    InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //                    iInsertRowIndex = iInsertRowIndex + 2;
            //                }
            //                else if (iLastSrNo == 3)//add dummy entry for Lubricants
            //                {
            //                    InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                    InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
            //                    iInsertRowIndex = iInsertRowIndex + 1;
            //                }                        
            //            }
            //        }
            //        iLastSrNo =  iCurrentSrNo;
            //    }
            //    else // if job codes are different
            //    {
            //        if (iCurrentSrNo == 1 && (iLastSrNo != 5))
            //        {
            //            bInsertRecordForLastJobCode = true;
            //        }
            //        else if (iCurrentSrNo != 1 )
            //        { 
            //            bInsertRecordForLastJobCode = true;
            //        }
            //        // insert dummy entry for for last job code                
            //        if(bInsertRecordForLastJobCode==true)
            //        {
            //            if (iLastSrNo == 1)// add Record for Sublet,lubricant,labour
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);                        
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
            //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 3;  
            //            }
            //            else if (iLastSrNo == 3)// add Record for Sublet,Lubricant
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 2;  
            //            }
            //            else if (iLastSrNo == 4)// add Record for Sublet
            //            {
            //                InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
            //                iInsertRowIndex = iInsertRowIndex + 1;  
            //            }
            //        }
            //        if (iCurrentSrNo == 3)// if Current Srno is for Labor
            //        {
            //            // add Record for Parts for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
            //            iInsertRowIndex = iInsertRowIndex + 1;  
            //        }
            //        else if (iCurrentSrNo == 4)// if Current Srno is for  Lubricant
            //        {                    
            //            // add Record for Parts,Labor for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);                    
            //            iInsertRowIndex = iInsertRowIndex + 2;  
            //        }
            //        else if (iCurrentSrNo == 5)// if Current Srno is for Sublet
            //        {                   
            //            // add Record for Parts,Labor for Current Job Code 
            //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricant", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
            //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
            //            iInsertRowIndex = iInsertRowIndex + 3;  
            //        }
            //        iMainJobCode = iCurrJobCode;
            //        iLastSrNo = iCurrentSrNo;
            //    }
            //    iInsertRowIndex++;
            //}
            //if (iLastSrNo != 5)
            //{
            //    if (iLastSrNo == 1)//Parts
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo ==3)//Labor
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo == 4)//Lubricant
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);                
            //    }            
            //}

            DetailsGrid.DataSource = dtCopy;
            DetailsGrid.DataBind();
            if (dt.Rows.Count > 0)
                SetGridControlProperty();
        }

        //Bind Data To Export Summary
        private void BindDataToDomesticSummary(DataTable dt)
        {
            int iLastSrNo = 1;
            int iCurrJobCode = 0;
            int iMainJobCode = 0;
            int iCurrentSrNo = 0;
            string sSrNo = "";
            string[] splSrNo = new string[7];
            DataTable dtCopy = new DataTable();
            dtCopy = dt.Copy();
            int iCurrentColumnIndex = 0;
            int iInsertRowIndex = 0;
            bool bInsertRecordForLastJobCode = false;
            for (iCurrentColumnIndex = 0; iCurrentColumnIndex < dt.Rows.Count; iCurrentColumnIndex++)
            {
                bInsertRecordForLastJobCode = false;
                iCurrJobCode = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["job_code_ID"]);
                iCurrentSrNo = Func.Convert.iConvertToInt(dt.Rows[iCurrentColumnIndex]["SrNo"]);

                if (iCurrentColumnIndex == 0)
                    iMainJobCode = iCurrJobCode;

                if (iMainJobCode == iCurrJobCode)
                {
                    if (sSrNo == "")
                        sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    else
                        sSrNo = sSrNo + "," + Func.Convert.sConvertToString(iCurrentSrNo);

                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        string tmpSrNo = "";
                        splSrNo = sSrNo.Split(',');
                        for (int iCnt = 0; iCnt < 7; iCnt++)
                        {

                            int iRecordCnt = 0;
                            if (iCnt < splSrNo.Length)
                            {
                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }
                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobTypeDom(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }
                            else if (iCnt >= splSrNo.Length)
                            {

                                for (int JCnt = 0; JCnt < splSrNo.Length; JCnt++)
                                {
                                    if (iLastSrNo == Func.Convert.iConvertToInt(splSrNo[JCnt]))
                                    {
                                        iRecordCnt = iRecordCnt + 1;
                                        break;
                                    }
                                }

                                if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]) && iRecordCnt == 0)
                                {
                                    InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobTypeDom(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                                else
                                {
                                    iInsertRowIndex = iInsertRowIndex + 1;
                                    iLastSrNo = iLastSrNo + 1;
                                }
                            }

                        }
                    }
                }
                else
                {
                StepPrev:
                    ;
                    splSrNo = sSrNo.Split(',');
                    int iCnt = 0;
                    for (iCnt = 1; iCnt < 8; iCnt++)
                    {
                        if (splSrNo.Contains(Func.Convert.sConvertToString(iCnt)) == false)
                        {
                            InsertEmtryRowToDataTable(dtCopy, iCnt, JobTypeDom(Func.Convert.sConvertToString(iCnt)), iMainJobCode, iInsertRowIndex);
                        }
                        iInsertRowIndex = iInsertRowIndex + 1;
                    }

                    //string tmpSrNo = "";
                    //for (int iCnt = 0; iCnt < 7; iCnt++)
                    //{                   
                    //    //if (iCnt == splSrNo.Length)
                    //    //    tmpSrNo = splSrNo[iCnt - 1];
                    //    if (iCnt < splSrNo.Length)
                    //    {
                    //        if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[iCnt]))
                    //        {
                    //            InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobTypeDom(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                    //            iInsertRowIndex = iInsertRowIndex + 1;
                    //            iLastSrNo = iLastSrNo + 1;
                    //        }
                    //        else
                    //        {
                    //            iInsertRowIndex = iInsertRowIndex + 1;
                    //            iLastSrNo = iLastSrNo + 1;
                    //        }
                    //    }
                    //    else if (iCnt >= splSrNo.Length)
                    //    {
                    //        if (iLastSrNo != Func.Convert.iConvertToInt(splSrNo[splSrNo.Length - 1]))
                    //        {
                    //            InsertEmtryRowToDataTable(dtCopy, iCnt + 1, JobTypeDom(Func.Convert.sConvertToString(iLastSrNo)), iMainJobCode, iInsertRowIndex);
                    //            iInsertRowIndex = iInsertRowIndex + 1;
                    //            iLastSrNo = iLastSrNo + 1;
                    //        }
                    //        else
                    //        {
                    //            iInsertRowIndex = iInsertRowIndex + 1;
                    //            iLastSrNo = iLastSrNo + 1;
                    //        }
                    //    }
                    //}
                    iMainJobCode = iCurrJobCode;
                    sSrNo = Func.Convert.sConvertToString(iCurrentSrNo);
                    iLastSrNo = 1;
                    if (iCurrentColumnIndex == dt.Rows.Count - 1)
                    {
                        iCurrentColumnIndex = iCurrentColumnIndex + 1;
                        goto StepPrev;
                    }
                }



                //if (iMainJobCode == 0)
                //{
                //    iMainJobCode = iCurrJobCode;
                //    if (iCurrentSrNo != 1)// If Part record not exists for the first job code
                //    {
                //        if (iCurrentSrNo == 3)//add dummy entry for Parts
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);                        
                //            iInsertRowIndex = iInsertRowIndex + 2;
                //            iCurrentSrNo++;
                //        }
                //        else if (iCurrentSrNo == 4)//add dummy entry for Labor
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);
                //            iInsertRowIndex = iInsertRowIndex + 3;
                //        }
                //        else if (iCurrentSrNo == 5)//add dummy entry for Lubricants ,Labor,Tax,Parts
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, 0);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, 0);
                //            iInsertRowIndex = iInsertRowIndex + 4;
                //        }
                //    }
                //    iLastSrNo = iCurrentSrNo;
                //}
                //if (iMainJobCode == iCurrJobCode)// if job codes are same
                //{

                //    if (iLastSrNo == iCurrentSrNo)
                //    {                    
                //        if (iCurrentSrNo == 4)//add dummy entry for Labor
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 5, "Sublet", iCurrJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 1;
                //        }
                //        else if (iCurrentSrNo == 5)//add dummy entry for Lubricant
                //        {
                //            if (iLastSrNo == 2)//add dummy entry for Labor
                //            {
                //                InsertEmtryRowToDataTable(dtCopy, 5, "Sublet", iCurrJobCode, iInsertRowIndex);
                //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
                //                InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
                //                iInsertRowIndex = iInsertRowIndex + 3;
                //            }
                //            else if (iLastSrNo == 3)
                //            {
                //                InsertEmtryRowToDataTable(dtCopy, 5, "Sublet", iCurrJobCode, iInsertRowIndex);
                //                InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, iInsertRowIndex);
                //                iInsertRowIndex = iInsertRowIndex + 2;
                //            }
                //        }                    
                //    }
                //    iCurrentSrNo++;
                //    if (iCurrentSrNo == 1)
                //    {
                //        iCurrentSrNo++;
                //    }
                //    iLastSrNo = iCurrentSrNo;
                //}
                //else // if job codes are different
                //{
                //    if (iCurrentSrNo == 1 && (iLastSrNo != 5))
                //    {
                //        bInsertRecordForLastJobCode = true;
                //    }
                //    else if (iCurrentSrNo != 1)
                //    {
                //        bInsertRecordForLastJobCode = true;
                //    }
                //    // insert dummy entry for for last job code                
                //    if (bInsertRecordForLastJobCode == true)
                //    {
                //        if (iLastSrNo == 2)// add Record for Sublet,lubricant,labour
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iMainJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 3;
                //        }
                //        else if (iLastSrNo == 3)// add Record for Sublet,Lubricant
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iMainJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iMainJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 2;
                //        }
                //        else if (iLastSrNo == 4)// add Record for Sublet
                //        {
                //            InsertEmtryRowToDataTable(dtCopy, 4, "SubLet", iMainJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 1;
                //        }
                //        if (iCurrentSrNo == 3)// if Current Srno is for Labor
                //        {
                //            // add Record for Parts for Current Job Code 
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 2;
                //        }
                //        else if (iCurrentSrNo == 4)// if Current Srno is for  Lubricant
                //        {
                //            // add Record for Parts,Labor for Current Job Code 
                //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 3;
                //        }
                //        else if (iCurrentSrNo == 4)// if Current Srno is for Sublet
                //        {
                //            // add Record for Parts,Labor for Current Job Code 
                //            InsertEmtryRowToDataTable(dtCopy, 4, "Lubricant", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 2, "Tax", iCurrJobCode, iInsertRowIndex);
                //            InsertEmtryRowToDataTable(dtCopy, 1, "Parts", iCurrJobCode, iInsertRowIndex);
                //            iInsertRowIndex = iInsertRowIndex + 4;
                //        }
                //    }

                //    iMainJobCode = iCurrJobCode;
                //    iLastSrNo = iCurrentSrNo;
                //}
                //iInsertRowIndex++;
            }
            //if (iLastSrNo != 4)
            //{
            //    if (iLastSrNo == 2)//Parts Tax
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 3, "Labor", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo == 3)//Labor
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 4, "Lubricants", iCurrJobCode, -1);
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //    if (iLastSrNo == 4)//Lubricant
            //    {
            //        InsertEmtryRowToDataTable(dtCopy, 5, "SubLet", iCurrJobCode, -1);
            //    }
            //}

            DetailsGrid.DataSource = dtCopy;
            DetailsGrid.DataBind();
            if (dt.Rows.Count > 0)
                SetGridControlProperty();
        }
        //Insert Empty record for which data is not enter
        private void InsertEmtryRowToDataTable(DataTable dt, int iSrNoCnt, string sItem, int iJobCode, int iPosition)
        {
            DataRow dr;
            dr = dt.NewRow();
            dr["job_code_ID"] = iJobCode;
            dr["Job_Code"] = "J" + iJobCode;
            dr["SrNo"] = iSrNoCnt;
            dr["Item"] = sItem;
            dr["ClaimAmount"] = 0.00;
            dr["Deduction_Percentage"] = 0.00;
            dr["Deducted_Amount"] = 0.00;
            dr["Accepted_Amount"] = 0.00;
            dr["AutoDeduct_Amount"] = 0.00;
            dr["FinalAccepted_Amount"] = 0.00;
            if (iPosition == -1)
            {
                dt.Rows.Add(dr);
            }
            else
            {
                dt.Rows.InsertAt(dr, iPosition);
            }
            dt.AcceptChanges();
        }

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {

            txtClaimDate.ReadOnly = true;
            //DetailsGrid.Enabled = bEnable;
            txtModelCode.ReadOnly = !bEnable;
            txtModelDescription.ReadOnly = !bEnable;
            txtChangeFertCode.ReadOnly = !bEnable;
            txtChangeFertName.ReadOnly = !bEnable;
            txtChassisNo.ReadOnly = !bEnable;
            txtEngineNo.ReadOnly = !bEnable;
            txtVehicleNo.ReadOnly = !bEnable;
            txtCustomerName.ReadOnly = !bEnable;
            txtCustomerAddress.ReadOnly = !bEnable;
            txtInstallationDate.ReadOnly = !bEnable;
            txtOdometer.ReadOnly = !bEnable;
            txtHrsReading.ReadOnly = !bEnable;
            txtRepairOrderNo.ReadOnly = !bEnable;

            txtFailureDate.ReadOnly = !bEnable;

            txtInvoiceNo.ReadOnly = !bEnable;
            txtInvoiceDate.ReadOnly = !bEnable;

            //lblServiceHistroy.Enabled = bEnable;
            //lblWarrantyHistroy.Enabled = bEnable;
            lnkSelectRequest.Enabled = true;

            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

            if (bEnable == true)
            {
                btnApprove.Style.Add("display", "");
                btnReject.Style.Add("display", "");
                btnReturn.Style.Add("display", "");
                if ((iUserRoleId == 10 && sDomestic_Export == "D" && sRequestOrClaim == "C") || (iUserRoleId == 9 && txtRequestNo.Text.Trim() != "" && sRequestOrClaim == "C") || (iUserRoleId == 18 && sDomestic_Export == "D" && sRequestOrClaim == "C"))
                {
                    btnReject.Style.Add("display", "none");
                    //btnReturn.Style.Add("display", "none");
                }
                if (sRequestOrClaim != "C")
                    btnReturn.Style.Add("display", "none");
                btnSave.Style.Add("display", "");
                btnSave.Style.Add("display", "none");
                btnSubmit.Style.Add("display", "none");
            }
            else
            {
                btnApprove.Style.Add("display", "none");
                btnReject.Style.Add("display", "none");
                btnReturn.Style.Add("display", "none");
                btnCancel.Style.Add("display", "none");
                btnOK.Style.Add("display", "none");
                if ((iUserRoleId == 4 || iUserRoleId == 3) && txtRequestNo.Text.Trim() != "" && sRequestOrClaim == "C" && hdnIsSHQ.Value == "N" && hdnIsSHQResource.Value == "N" && txtTotalAccepted_Amount.Text != "")
                    btnSubmit.Style.Add("display", "");
                else
                    btnSubmit.Style.Add("display", "none");
                drpReason.Style.Add("display", "none");
                btnSave.Style.Add("display", "none");
                if (iUserRoleId == 9 && sDomestic_Export == "E" && sRequestOrClaim == "C" && (hdnIsSHQ.Value == "N" || hdnIsSHQ.Value == "P") && (hdnIsSHQResource.Value == "N" || hdnIsSHQResource.Value == "P") && txtTotalAccepted_Amount.Text != "")
                {
                    btnSubmit.Style.Add("display", "");
                }
            }



        }


        private void SetGridControlProperty()
        {
            int iCurrentJobId = 0;
            int iMainJobId = 0;
            double dTotalClaimAmount = 0;
            //double dTotalDeductionPercentage = 0;
            double dTotalDeductedAmount = 0;
            double dTotalAcceptedAmount = 0;
            double dTotalAutoDeductedAmount = 0;
            double dTotalFinalAcceptedAmount = 0;

            double dCurrLineAccClaimAmount = 0;

            double dAccPartAmount = 0;
            double dAccLabourAmount = 0;
            double dAccLubricantAmount = 0;
            double dAccSubletAmount = 0;

            DetailsGrid.HeaderRow.Cells[0].Style.Add("display", "none"); // Hide Job Code ID 
            DetailsGrid.FooterRow.Cells[0].Style.Add("display", "none"); // Hide Job Code ID 

            if (txtDomestic_Export.Text == "E")//Export User Hide AutoDeduction Column .As Per discussion with deepti madam 
            {
                DetailsGrid.HeaderRow.Cells[6].Style.Add("display", "none");
                DetailsGrid.FooterRow.Cells[6].Style.Add("display", "none");
                txtTotalAutoDeduction.Style.Add("display", "none");
            }
            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                iCurrentJobId = Func.Convert.iConvertToInt((DetailsGrid.Rows[iRowCnt].FindControl("lblJobCodeID") as Label).Text);
                DetailsGrid.Rows[iRowCnt].Cells[0].Style.Add("display", "none"); // Hide Job Code ID                 
                

                if (txtDomestic_Export.Text == "E")
                {
                    DetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "none"); //Hide Auto deduction column               

                }
                TextBox txtClaimAmount = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtClaimAmount");
                TextBox txtDeduction_Percentage = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDeduction_Percentage");
                TextBox txtDeducted_Amount = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtDeducted_Amount");
                TextBox txtAccepted_Amount = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtAccepted_Amount");

                TextBox txtAutoDeductAmt = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtAutoDeductAmt");
                TextBox txtFinalAcceptedAmt = (TextBox)DetailsGrid.Rows[iRowCnt].FindControl("txtFinalAcceptedAmt");

                txtClaimAmount.Style.Add("readOnly", "true");
                //txtDeduction_Percentage.Style.Add("readOnly", "true");
                txtDeducted_Amount.Style.Add("readOnly", "true");
                txtAccepted_Amount.Style.Add("readOnly", "true");

                dTotalClaimAmount = dTotalClaimAmount + Func.Convert.dConvertToDouble(txtClaimAmount.Text);
                //dTotalDeductionPercentage = dTotalDeductionPercentage + Func.Convert.dConvertToDouble(txtDeduction_Percentage.Text);
                dTotalDeductedAmount = dTotalDeductedAmount + Func.Convert.dConvertToDouble(txtDeducted_Amount.Text);
                dCurrLineAccClaimAmount = Func.Convert.dConvertToDouble(txtAccepted_Amount.Text);
                dTotalAcceptedAmount = dTotalAcceptedAmount + dCurrLineAccClaimAmount;

                dTotalAutoDeductedAmount = dTotalAutoDeductedAmount + Func.Convert.dConvertToDouble(txtAutoDeductAmt.Text);
                dTotalFinalAcceptedAmount = dTotalFinalAcceptedAmount + Func.Convert.dConvertToDouble(txtFinalAcceptedAmt.Text);

                LinkButton lnkJobCode = (LinkButton)DetailsGrid.Rows[iRowCnt].FindControl("lnkJobCode");
                //lnkJobCode.Attributes.Add("onClick", "return ShowJobDetails(this,'" + iCurrentJobId + "','" + iClaimID + "')");

                if (iMainJobId == 0)
                {
                    iMainJobId = iCurrentJobId;
                }
                else
                {
                    if (iCurrentJobId == iMainJobId)
                    {
                        lnkJobCode.Text = "";
                    }
                    else if (iCurrentJobId != iMainJobId)
                    {
                        iMainJobId = iCurrentJobId;
                    }
                }
                Label lblItem = (Label)(DetailsGrid.Rows[iRowCnt].FindControl("lblItem"));

                if (txtDomestic_Export.Text == "E")
                {
                    if (lblItem.Text == "Tax" || lblItem.Text == "Labor NonTaxable" || lblItem.Text == "Tax(Labor + Sublet)")
                    {
                        DetailsGrid.Rows[iRowCnt].Style.Add("display", "none");
                    }

                    if (lblItem.Text == "Labor Taxable")
                    {
                        lblItem.Text = "Labor";
                    }

                }
                else
                {
                    if (lblItem.Text == "Tax")
                    {                        
                        DetailsGrid.Rows[iRowCnt].Style.Add("display", "none");   
                        //DetailsGrid.Rows[iRowCnt].Style.Add("display", (hdnISDocGST.Value.Trim() == "N") ? "none" : "");                          
                    }
                    if (lblItem.Text == "Tax(Part + Lubricant + Labor + Sublet)")
                    {
                        lblItem.Text = (hdnInvType.Value == "N") ? "Tax(Part + Lubricant + Labor + Sublet)" : (hdnInvType.Value == "P") ? "Tax(Part + Lubricant)" : "Tax(Labor + Sublet)";                         
                    }
                }
                if (lblItem.Text == "Parts")
                {
                    dAccPartAmount = dAccPartAmount + dCurrLineAccClaimAmount;
                }
                else if (lblItem.Text == "Labor")
                {
                    dAccLabourAmount = dAccLabourAmount + dCurrLineAccClaimAmount;
                }
                else if (lblItem.Text == "Lubricants")
                {
                    dAccLubricantAmount = dAccLubricantAmount + dCurrLineAccClaimAmount;
                }
                else if (lblItem.Text == "Sublet")
                {
                    dAccSubletAmount = dAccSubletAmount + dCurrLineAccClaimAmount;
                }
            }
            (DetailsGrid.FooterRow.FindControl("lblTotalClaimAmount") as Label).Text = Func.Convert.dConvertToDouble(dTotalClaimAmount).ToString("0.00");
            //(DetailsGrid.FooterRow.FindControl("lblTotalDeduction_Percentage") as Label).Text = Func.Convert.dConvertToDouble(dTotalDeductionPercentage).ToString("0.00");
            (DetailsGrid.FooterRow.FindControl("lblTotalDeducted_Amount") as Label).Text = Func.Convert.dConvertToDouble(dTotalDeductedAmount).ToString("0.00");
            (DetailsGrid.FooterRow.FindControl("lblTotalAccepted_Amount") as Label).Text = Func.Convert.dConvertToDouble(dTotalAcceptedAmount).ToString("0.00");

            (DetailsGrid.FooterRow.FindControl("lblTotalAutoDeductAmt") as Label).Text = Func.Convert.dConvertToDouble(dTotalAutoDeductedAmount).ToString("0.00");
            (DetailsGrid.FooterRow.FindControl("lblFinalAcceptedAmt") as Label).Text = Func.Convert.dConvertToDouble(dTotalFinalAcceptedAmount).ToString("0.00");
            lblTotal.Text = "Total:";
            lblTotalExp.Text = "Total:";
            lblTotal.Text = lblTotal.Text + "(Currency in " + hdnCurrency.Value + ")";
            lblTotalExp.Text = lblTotalExp.Text + "(Currency in " + hdnCurrency.Value + ")";
            if (txtDomestic_Export.Text == "E")
            {
                tblTotalMainExport.Style.Add("display", "");
                tblTotalMain.Style.Add("display", "none");
            }
            else
            {
                tblTotalMainExport.Style.Add("display", "none");
                tblTotalMain.Style.Add("display", "");
            }
            txtTotalClaimAmount.Text = Func.Convert.dConvertToDouble(dTotalClaimAmount).ToString("0.00");
            //txtTotalDeduction_Percentage.Text = Func.Convert.dConvertToDouble(dTotalDeductionPercentage).ToString("0.00");
            txtTotalDeducted_Amount.Text = Func.Convert.dConvertToDouble(dTotalDeductedAmount).ToString("0.00");
            txtTotalAccepted_Amount.Text = Func.Convert.dConvertToDouble(dTotalAcceptedAmount).ToString("0.00");

            txtTotalAutoDeduction.Text = Func.Convert.dConvertToDouble(dTotalAutoDeductedAmount).ToString("0.00");
            txtTotalFinalAcceptedAmt.Text = Func.Convert.dConvertToDouble(dTotalFinalAcceptedAmount).ToString("0.00");

            txtTotalClaimAmountExp.Text = Func.Convert.dConvertToDouble(dTotalClaimAmount).ToString("0.00");
            //txtTotalDeduction_Percentage.Text = Func.Convert.dConvertToDouble(dTotalDeductionPercentage).ToString("0.00");
            txtTotalDeducted_AmountExp.Text = Func.Convert.dConvertToDouble(dTotalDeductedAmount).ToString("0.00");
            txtTotalAccepted_AmountExp.Text = Func.Convert.dConvertToDouble(dTotalAcceptedAmount).ToString("0.00");
            txtTotalFinalAcceptedAmtExp.Text = Func.Convert.dConvertToDouble(dTotalFinalAcceptedAmount).ToString("0.00");


            // Set Total of Accepted amount Part/Labor/Lubricant/Sublet Wise
            txtPartAmount.Text = Func.Convert.dConvertToDouble(dAccPartAmount).ToString("0.00");
            txtLabourAmount.Text = Func.Convert.dConvertToDouble(dAccLabourAmount).ToString("0.00");
            txtLubricantAmount.Text = Func.Convert.dConvertToDouble(dAccLubricantAmount).ToString("0.00");
            txtSubletAmount.Text = Func.Convert.dConvertToDouble(dAccSubletAmount).ToString("0.00");
            txtClaimAmt.Text = Func.Convert.dConvertToDouble(dTotalAcceptedAmount).ToString("0.00");


        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;
            bSubmit = bSaveRecord(2, "M");
            if (bSubmit == true)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowMessage(9)", true);            
                lblMessage.Text = " Record is submitted successfully !";
            }
            else
            {
                lblMessage.Text = " Record is not submitted.";
            }
            GetDataAndDisplay();
            lblMessage.Visible = true;
            btnSubmit.Style.Add("display", "none");
        }
        //To Save the record
        private bool bSaveRecord(int iClaimStatus, string sSaveOrSubmit)
        {
            clsWarranty ObjWarranty = new clsWarranty();
            string sRemark = "";
            bool bSubmit = false;
            int iReasonID = 0;
            int iRequestTypeID = 0;
            if (iUserRoleId == 4)// if user is Csm
            {
                sRemark = txtCSMRemark.Text;
            }
            else if (iUserRoleId == 3)// if user is Asm
            {
                sRemark = txtASMRemark.Text;
            }
            else if (iUserRoleId == 2)// if user is Rsm
            {
                sRemark = txtRSMRemark.Text;
            }
            else if (iUserRoleId == 1)// if user is Head
            {
                sRemark = txtHeadRemark.Text;
            }
            else if (iUserRoleId == 13)// if user is Head Retail
            {
                sRemark = txtHeadRetailRemark.Text;
            }
            else if (iUserRoleId == 14)// if user is Head Sale Marketing After Marketing
            {
                sRemark = txtHeadSaleMkgAfterMkgRemark.Text;
            }
            else if (iUserRoleId == 9)// if user is SHQ Resource
            {
                sRemark = txtSHQRRemark.Text;
            }
            else if (iUserRoleId == 10)// if user is SHQ
            {
                sRemark = txtSHQRemark.Text;
            }
            else if (iUserRoleId == 18)// if user is SA Resource
            {
                sRemark = txtSAResourceRemark.Text;
            }
            if (iClaimStatus == 2)
            {
                drpReason.SelectedValue = "0";
                iReasonID = 0;
            }
            else
            {
                iReasonID = Func.Convert.iConvertToInt(drpReason.SelectedValue);
            }

            //VHP_Warranty Start
            if (Session["ClaimTypes"] != null)
            {
                ValenmFormUsedFor = (clsWarranty.enmClaimType)Session["ClaimTypes"];
            }
            if ((ValenmFormUsedFor == clsWarranty.enmClaimType.enmHighValueClaim) && (txtDomestic_Export.Text == "D"))
            {

                iRequestTypeID = 1;
            }
            // VHP_Warranty End
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
            {

                if (txtClaimTypeID.Text == "2" || txtClaimTypeID.Text == "8")//Technical/Commercial Goodwill ofDomestic
                {
                    if (OptGoodwillType.Items[0].Selected == true)
                    {
                        iRequestTypeID = 2;
                    }
                    else if (OptGoodwillType.Items[1].Selected == true)
                    {
                        iRequestTypeID = 8;
                    }

                }
                else if (txtClaimTypeID.Text == "22" || txtClaimTypeID.Text == "16")//Technical/Commercial Goodwill of Export
                {
                    if (OptGoodwillType.Items[0].Selected == true)
                    {
                        iRequestTypeID = 22;
                    }
                    else if (OptGoodwillType.Items[1].Selected == true)
                    {
                        iRequestTypeID = 16;
                    }
                }
            }
            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal && txtDomestic_Export.Text == "D")
            {

                if (txtClaimTypeID.Text == "19" || txtClaimTypeID.Text == "20")//Technical/Commercial Goodwill ofDomestic
                {
                    if (OptGoodwillType.Items[0].Selected == true)
                    {
                        iRequestTypeID = 19;
                    }
                    else if (OptGoodwillType.Items[1].Selected == true)
                    {
                        iRequestTypeID = 20;
                    }

                }

            }

            //Sujata 02062011
            //Get File Attach

            // Commented by Shyamal as on 28092012,Now file upload can be possible at any stage.
            //if ((Func.Convert.iConvertToInt(txtfinalAppUserRole.Text) == iUserRoleId && txtClaimTypeID.Text == "8") || (iUserRoleId == 4 && (sRequestOrClaim == "C" || txtClaimTypeID.Text == "2")))
            //{
            bSubmit = bSaveAttachedDocuments();
            if (bSubmit == false)
            {
                return bSubmit;
            }
            if (sRequestOrClaim != "C")
                bSaveGCRFileAttachDetails(dtFileAttach, Func.Convert.iConvertToInt(txtID.Text));
            else
                bSaveClaimFileAttachDetails(dtFileAttach, Func.Convert.iConvertToInt(txtID.Text));
            //}
            //Sujata 02062011

            bSubmit = ObjWarranty.bSubmitClaimForSave(sRequestOrClaim, iClaimID, iUserRoleId, sRemark, sSaveOrSubmit, iClaimStatus, iUserId, iReasonID, iRequestTypeID, txtDomestic_Export.Text, Convert.ToInt32(hdnDealerID.Value));
            ObjWarranty = null;
            if (bSubmit == true)
            {
                drpReason.Style.Add("display", "none");
            }
            return bSubmit;
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;
            bSubmit = bSaveRecord(2, "M");
            if (bSubmit == true)
            {
                lblMessage.Text = " Record is Approved successfully.!";
            }
            else
            {
                lblMessage.Text = " Record is not Approved.";
            }
            GetDataAndDisplay();
            lblMessage.Visible = true;

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            btnApprove.Style.Add("display", "none");
            btnReject.Style.Add("display", "none");
            btnReturn.Style.Add("display", "none");
            panReason.Style.Add("display", "");
            lblRejectOrConfirm.Text = "J";// For Reject
            btnOK.Text = "Reject";
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            btnApprove.Style.Add("display", "none");
            btnReject.Style.Add("display", "none");
            btnReturn.Style.Add("display", "none");
            panReason.Style.Add("display", "");
            lblRejectOrConfirm.Text = "R";// For Return 
            btnOK.Text = "Return";

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (lblRejectOrConfirm.Text == "J")
            {
                bool bSubmit = false;
                bSubmit = bSaveRecord(3, "J");
                if (bSubmit == true)
                {
                    lblMessage.Text = " Record is Rejected.!";
                }
                else
                {
                    lblMessage.Text = " Record is not Rejected.";
                }
                GetDataAndDisplay();
                lblMessage.Visible = true;
            }
            else
                if (lblRejectOrConfirm.Text == "R")
                {
                    bool bSubmit = false;
                    bSubmit = bSaveRecord(4, "R");
                    if (bSubmit == true)
                    {
                        lblMessage.Text = " Record is Returned.!";
                    }
                    else
                    {
                        lblMessage.Text = " Record is not Returned.";
                    }
                    GetDataAndDisplay();
                    lblMessage.Visible = true;
                }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            sDomestic_Export = txtDomestic_Export.Text;
            btnApprove.Style.Add("display", "");
            btnReject.Style.Add("display", "");
            btnReturn.Style.Add("display", "");
            if ((iUserRoleId == 10 && sDomestic_Export == "D" && sRequestOrClaim == "C") || (iUserRoleId == 9 && txtRequestNo.Text.Trim() != "" && sRequestOrClaim == "C") || (iUserRoleId == 18 && sDomestic_Export == "D" && sRequestOrClaim == "C"))
            {
                btnReject.Style.Add("display", "none");
                //btnReturn.Style.Add("display", "none");
            }
            if (sRequestOrClaim != "C")
                btnReturn.Style.Add("display", "none");

            panReason.Style.Add("display", "none");
            lblRejectOrConfirm.Text = "N";// For Return 
            btnOK.Text = "Return";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;

            bSubmit = bSaveRecord(1, "S");
            if (bSubmit == true)
            {
                lblMessage.Text = " Record is saved.!";
            }
            else
            {
                lblMessage.Text = " Record is not saved.";
            }
            GetDataAndDisplay();
            lblMessage.Visible = true;
        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GetDataAndDisplay();
        }
        protected void lblWarrantyHistroy_Click(object sender, EventArgs e)
        {

            ////This function is used for display chassis warranty details report.
            //int iDocId;
            //string sReportName="/reportserver_MSSqlr2/RptWarrantyClaimHistory";
            //string  Url = "/AUTODMS/Forms/Common/frmDocumentView.aspx?";
            //Url = Url + sReportName + "ID=" + iDocId;

        }
        protected void lblWarrantyHistroy_Click1(object sender, EventArgs e)
        {
            //string strReportpath;
            //strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
            //lblWarrantyHistroy.Attributes.Add("onClick", "return ShowChassisWDtls('" + txtchassisID.Text + "','" + strReportpath + "');");

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Page.RegisterStartupScript("Close", "<script language='javascript'> window.close(); </script>");
            //btnBack.Attributes.Add("onClick", "return CloseWarrantyClaimProsseingWindow();");
        }
        //Sujata 12012011
        // To Show Attach Documents.
        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();

                for (int iRowCnt = 0; iRowCnt < FileAttchGrid.Rows.Count; iRowCnt++)
                {
                    TextBox txtCrUserRole = (TextBox)FileAttchGrid.Rows[iRowCnt].FindControl("txtCrUserRole");
                    TextBox txtDescription = (TextBox)FileAttchGrid.Rows[iRowCnt].FindControl("txtDescription");
                    txtDescription.ReadOnly = (Func.Convert.iConvertToInt(txtCrUserRole.Text.Trim()) != Func.Convert.iConvertToInt(Session["UserRole"])) ? true : false;                                        
                }
            }
        }
        //Sujata 12012011

        //Sujata 02062011
        public void GetFinalApprovalUserRoleId(int iClaimSlabId)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetFinalApprovalUserRoleId", iClaimSlabId);
                if (ds != null)
                {
                    txtfinalAppUserRole.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Role_ID"]);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

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


                        //dr["File_Names"] = sSourceFileName;
                        if (sRequestOrClaim != "C")
                            dr["File_Names"] = Func.Convert.sConvertToString(txtDealerAID.Text) + "_R_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName;
                        else
                            dr["File_Names"] = Func.Convert.sConvertToString(txtDealerAID.Text) + "_C_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName;

                        dr["UserId"] = Func.Convert.sConvertToString(txtDealerAID.Text);
                        dr["Status"] = "S";
                        dr["CreatedUserRole"] = Func.Convert.iConvertToInt(Session["UserRole"]); 

                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        string[] splClaimNo = txtClaimNo.Text.Split('/');
                        if (splClaimNo.Length > 1)
                        {
                            txtClaimNo.Text = "";
                            for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                                txtClaimNo.Text = txtClaimNo.Text + splClaimNo[iCnt];
                        }
                        //Saving it in temperory Directory.
                        if (!System.IO.Directory.Exists(sPath + "Claim Request"))
                            System.IO.Directory.CreateDirectory(sPath + "Claim Request");
                        if (sRequestOrClaim != "C"){
                            uploads[i].SaveAs((sPath + "Claim Request" + "\\" + Func.Convert.sConvertToString(txtDealerAID.Text) + "_R_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName));
                            //uploads[i].SaveAs((sPath + "Claim Request" + "\\" + dr["File_Names"].ToString()));
                        }
                        else
                        {
                            uploads[i].SaveAs((sPath + "Warranty Claim" + "\\" + Func.Convert.sConvertToString(txtDealerAID.Text) + "_C_" + Func.Convert.sConvertToString(txtClaimNo.Text) + "_" + sSourceFileName));
                            //uploads[i].SaveAs((sPath + "Claim Request" + "\\" + dr["File_Names"].ToString()));
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
                dr = dtFileAttach.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtFileAttchID") as TextBox).Text);
                dr["Description"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("txtDescription") as TextBox).Text;
                dr["File_Names"] = (FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text;
                dr["UserId"] = Func.Convert.iConvertToInt(sDealerId);

                //ChkForDelete = (CheckBox)(FileAttchGrid.Rows[iGridRowCnt].FindControl("ChkForDelete"));

                //if (ChkForDelete.Checked == true)
                //{
                //    dr["Status"] = "D";
                //}
                //else
                //{
                dr["Status"] = "S";
                //}
                dr["CreatedUserRole"] = (Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtCrUserRole") as TextBox).Text) != 0) ? Func.Convert.iConvertToInt((FileAttchGrid.Rows[iGridRowCnt].FindControl("txtCrUserRole") as TextBox).Text) : Func.Convert.iConvertToInt(Session["UserRole"]);
                dtFileAttach.Rows.Add(dr);
                dtFileAttach.AcceptChanges();
            }

        }
        // Save Attached File Attached Details
        // Save Attached File Attached Details
        private bool bSaveGCRFileAttachDetails(DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GCR_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Sujata 02062011


        private bool bSaveClaimFileAttachDetails(DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"], dtFileAttach.Rows[iRowCnt]["CreatedUserRole"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_Warranty_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }
        protected void FileAttchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblFile = (Label)e.Row.FindControl("lblFile");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (lblFile != null)
                lblFile.Text = DataBinder.Eval(e.Row.DataItem, "File_Names").ToString().Replace(" ", "&nbsp;");

        }
    }
}