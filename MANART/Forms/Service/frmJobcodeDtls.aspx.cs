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
using System.Drawing;
using System.IO;


namespace MANART.Forms.Service
{
    public partial class frmJobcodeDtls : System.Web.UI.Page
    {
        private DataTable dtJob = new DataTable();
        private int iJobHDRID = 0;
        private int iDealerId = 0;
        private int iJobCodeID = 0;
        private int iPCRHDRID = 0;
        int iUserPCRHeadApprID = 0;
        private string Display = "";
        private int ichassisID = 0;
        private DataTable dtComplaint = new DataTable();
        private DataTable dtActionTaken = new DataTable();
        private DataTable dtService = new DataTable();
        private DataTable dtFileAttach = new DataTable();        
        private string sDislayFOR="";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Session["UserType"].ToString();
                iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetUserPCRHeadApprv, 0, "")));
                if (!IsPostBack)
                {
                    lblTitle.Text = "PCR Details";
                    DisplayRecord();
                }

                string strReportpath;
                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                // var Url = "/../Common/frmDocumentView1.aspx?RptName=/MANARTREPORT";
                //btnPrint.Attributes.Add("onclick", "return ShowReport_PCR(this,'" + strReportpath + "');");
                btnPrint.Attributes.Add("onclick", "return ShowReport_PCR(this,'" + strReportpath + "','" + Func.Convert.sConvertToString(Session["UserRole"]) + "');");
                lblServiceHistroy.Attributes.Add("onClick", "return ShowClaimHistoryDtls('" + txtReportChassisID.Text + "','" + strReportpath + "');");
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();

                if (bValidateRecord() == false)
                {
                    
                }
                else
                {
                    UpdateHdrValueFromControl(dtHdr);

                    //Get File Attach
                    if (bSaveAttachedDocuments() == true)
                    {
                        if (ObjJobcard.bSaveJobcodePCRDetails(ref iPCRHDRID, dtHdr, "N", txtDlrCode.Text.Trim().ToString(), 0, dtFileAttach, "N", txtReject.Text.Trim(), "N", "N", txtRejectASM.Text.Trim(), "N", "N", txtRejectRSM.Text.Trim(), "N", "N", txtRejectHead.Text.Trim(),"N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " get saved successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select records.";
            bool bValidateRecord = true;
            if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" && txtIsSubmit.Text.Trim() == "N" && txtReject.Text.Trim() == "N" && txtSubmitASM.Text.Trim() == "N" && txtSubmitRSM.Text.Trim() == "N" && txtSubmitHead.Text.Trim() == "N")
            {
                if (txtJobCodeID.Text.Trim() == "0" || txtJobCodeID.Text.Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Check Jobcode not valid.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if(txtVehAppl.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Vehicle Application.');</script>");
                bValidateRecord = false;
                return bValidateRecord;
                }
                if (txtNatLoadCar.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Nature of load carried.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtPayLoad.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Payload (in tonnes).');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtAvgPerDay.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Avg run per day.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtRoadCond.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Road condition.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtFuelCons.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Fuel consumption.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtEngOilCons.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Eng. oil consumption.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }
                if (txtDtlDesc.Text.ToString().Trim() == "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Detailed Description.');</script>");
                    bValidateRecord = false;
                    return bValidateRecord;
                }

            }
            
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            //Validation For Model Ramarks
            return bValidateRecord;
        }

        public void DisplayRecord()
        {
            try
            {
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iJobCodeID = Func.Convert.iConvertToInt(Request.QueryString["JobCodeID"]);
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iPCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["PCRID"]);
                sDislayFOR = Func.Convert.sConvertToString(Request.QueryString["sFOR"]);
                if (sDislayFOR == "CLM")
                {
                    Func.Common.BindDataToCombo(drpSalesRegID, clsCommon.ComboQueryType.Region, 0, " and Domestic_Export='E'");
                }
                else
                {
                    Func.Common.BindDataToCombo(drpSalesRegID, clsCommon.ComboQueryType.Region, 0, " and Domestic_Export='D'");
                }

                if (iPCRHDRID != 0 && iPCRHDRID > 0)
                {                   
                    txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                    GetDataAndDisplay();
                }
                else
                    DisplayPreviousRecord();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public void DisplayPreviousRecord()
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iJobCodeID = Func.Convert.iConvertToInt(Request.QueryString["JobCodeID"]);
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iPCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["PCRID"]);
                sDislayFOR = Func.Convert.sConvertToString(Request.QueryString["sFOR"]);

                if (sDislayFOR == "CLM")
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PCR", iJobHDRID, "NEWCLM", iJobCodeID);
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PCR", iJobHDRID, "NEW", iJobCodeID);
                }
                ichassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_ID"]);
                txtReportChassisID.Text = Func.Convert.sConvertToString(ichassisID);
                if (ds != null) // if no Data Exist
                {
                    DisplayData(ds);
                }
                txtID.Text = "";
                txtDocNo.Text = Func.Common.sGetMaxDocNo(txtDlrCode.Text.Trim(), "", "PCR", iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false); 
                txtDocDate.Text = Func.Common.sGetCurrentDateTime(1, true);
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
        
        private void GetDataAndDisplay()
        {
            clsDB objDB = new clsDB();
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {
                iPCRHDRID = Func.Convert.iConvertToInt(txtID.Text);
                sDislayFOR = Func.Convert.sConvertToString(Request.QueryString["sFOR"]);
                if (sDislayFOR == "CLM")
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PCR", iPCRHDRID, "AllCLM", 0);
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PCR", iPCRHDRID, "All", 0);
                }
                if (ds != null) // if no Data Exist
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PCRHDRID"]);
                    ichassisID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Chassis_ID"]);
                    txtReportChassisID.Text = Func.Convert.sConvertToString(ichassisID);
                    DisplayData(ds);
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
                Display = Func.Convert.sConvertToString(Request.QueryString["Display"]);

                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PCRNo"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PCRDate"]);
                txtCRMTicketNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_Ticket_No"]);
                txtPreviousDocId.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_Hdr_ID"]);

                txtJobCodeID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCodeID"]);
                txtJobNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCardNo"]);
                txtWarrClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_No"]);
                
                txtProduct.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Productline"]);
                txtVIN.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VIN"]);
                txtAggreagateNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AggregateNo"]);

                lblChassisNo.Text = (txtAggreagateNo.Text.Trim() == "") ? "Chassis No" : "Aggregate No";
                
                txtAggreagateNo.Style.Add("display", (txtAggreagateNo.Text.Trim() != "") ? "" : "none");
                txtVIN.Style.Add("display", (txtAggreagateNo.Text.Trim() != "") ? "none" : "");

                txtModel.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vehiclemodel"]);
                txtEngNr.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngineNo"]);
                txtEndType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Enginetype"]);

                txtPartID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartID"]).Trim();
                txtPrmPart.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartNo"]);
                //txtPrmPartSupplier.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartSuppID"]);
                txtPrmPartSupplier.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartSuppPart"]);                
                hdnPrmPartSupplier.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartSuppID"]);
                txtPrmPartBatchID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PrimFailPartBatch"]);
                
                txtDefectID.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DefectCodeID"]).Trim();
                txtDefectCat.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DefectCategory"]);
                //txtDefectCode.Text=  Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MANDefectCode"]);
                txtDefectCode.Text=  (HttpContext.Current.Session["UserType"].ToString() == "3") ? Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DMS_Defect_Code"]) : Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MANDefectCode"]);
                txtDefectDesc.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MANDefectDesc"]);

                txtVehAppl.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["VehicleAppl"]);
                txtNatLoadCar.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NatureLoadCar"]);
                txtAvgPerDay.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["AvgRunPerDay"]);
                txtRoadCond.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RoadCondition"]);
                txtFuelCons.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FuelConsump"]);
                txtEngOilCons.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EngOilConsump"]);
                txtPayLoad.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Payload"]);

                txtCustName.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Name"]);
                txtCustLoc.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Loc"]);
                txtVehRegNo.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Veh_Reg_No"]);
                txtDlrCode.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerCode"]);
                txtDlrName.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerName"]);
                txtDlrLoc.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerLoc"]);

                drpSalesRegID.SelectedValue= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalesRegionID"]);                

                txtFailDate.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FailureDate"]);
                txtFailKm.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FailureKm"]);
                txtFailHr.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["FailureHr"]);

                txtDtlDesc.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DtlDescription"]);
                txtObservMSE.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ObservByMSECSM"]);
                txtObservASE.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ObservByASM"]);
                txtObservRSM.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ObservByRSM"]);
                txtObservHead.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ObservByHead"]);
                
                txtDealerBrID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DlrBranchID"]);
                txtDealerID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);

                lblChngPart.Attributes.Add("onclick", " return ShowPartMaster(this," + txtDealerID.Text + ")");

                //txtInspBy.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InspectedBy"]);                
                Func.Common.BindDataToCombo(DrpSupervisorName, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + txtDealerBrID.Text + " and Empl_Type=1" + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["InspectedBy"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InspectedBy"]) : ""));                
                DrpSupervisorName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InspectedBy"]);

                txtIsSubmit.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsSubmit"]);
                txtApprove.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Approve"]);
                txtReject.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Reject"]);
 
                txtSubmitASM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsSubmitToASM"]);
                txtApproveASM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsASMApprove"]);
                txtRejectASM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsASMReject"]);

                txtSubmitRSM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsSubmitToRSM"]);
                txtApproveRSM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRSMApprove"]);
                txtRejectRSM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRSMReject"]);

                txtSubmitHead.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsSubmitToHead"]);
                txtApproveHead.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsHeadApprove"]);
                txtRejectHead.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsHeadReject"]);
                
                hdnMSERetCnt.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RejectCnt"]);
                hdnHeadRetCnt.Value= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadRejectCnt"]);
                txtPCRReject.Text= Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ISPCRReject"]);

                txtChassisSaleDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sale_Date"]);

                btnApprove.Visible = false;
                btnReject.Visible = false;
                btnSubmit.Visible = false;
                btnSave.Visible = false;
                btnRReject.Visible = false;
                btnReject.Text = "Return PCR";

                lblTitle.Text = "PCR " + txtDocNo.Text + " (J" + txtJobCodeID.Text + ")" + ((txtPCRReject.Text == "Y") ? " Rejected " : "");                              

                if (txtIsSubmit.Text.Trim() == "Y" || txtSubmitASM.Text.Trim() == "Y" || txtSubmitRSM.Text.Trim() == "Y")
                {
                    //if (txtPCRReject.Text =="N" && txtIsSubmit.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtApprove.Text.Trim() == "N" && txtReject.Text.Trim() == "N") btnApprove.Visible = true;
                    //if (txtPCRReject.Text == "N" && txtIsSubmit.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtApprove.Text.Trim() == "N" && txtReject.Text.Trim() == "N") btnReject.Visible = true;
                    //if (txtPCRReject.Text == "N" && hdnMSERetCnt.Value == "3" && txtIsSubmit.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtApprove.Text.Trim() == "N" && txtReject.Text.Trim() == "N") btnReject.Text = "Reject PCR";

                    //commented for PCR rejection at any stage of approval instead of after 3 time return Begin
                    //if (txtPCRReject.Text == "N" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnApprove.Visible = true;
                    //if (txtPCRReject.Text == "N" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnReject.Visible = true;
                    //if (txtPCRReject.Text == "N" && hdnMSERetCnt.Value == "3" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnReject.Text = "Reject PCR";

                    if (txtPCRReject.Text == "N" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnApprove.Visible = true;
                    if (txtPCRReject.Text == "N" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N")
                    {
                        btnReject.Visible = true;
                        btnRReject.Visible = true;
                    }
                    if (txtPCRReject.Text == "N" && hdnMSERetCnt.Value == "3" && txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N")
                    {
                        btnReject.Visible = false;                        
                    }
                    //commented for PCR rejection at any stage of approval instead of after 3 time return End

                    //if (txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnApprove.Visible = true;
                    //if (txtSubmitASM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N") btnReject.Visible = true;

                    //if (txtSubmitRSM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "2" && txtApproveRSM.Text.Trim() == "N" && txtRejectRSM.Text.Trim() == "N") btnApprove.Visible = true;
                    //if (txtSubmitRSM.Text.Trim() == "Y" && Func.Convert.sConvertToString(Session["UserRole"]) == "2" && txtApproveRSM.Text.Trim() == "N" && txtRejectRSM.Text.Trim() == "N") btnReject.Visible = true;

                    //commented for PCR rejection at any stage of approval instead of after 3 time return Begin
                    //if (txtPCRReject.Text == "N" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N") btnApprove.Visible = true;
                    //if (txtPCRReject.Text == "N" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N") btnReject.Visible = true;
                    //if (txtPCRReject.Text == "N" && hdnHeadRetCnt.Value == "3" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N") btnReject.Text = "Reject PCR";

                    if (txtPCRReject.Text == "N" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N") btnApprove.Visible = true;
                    if (txtPCRReject.Text == "N" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N")
                    {
                        btnReject.Visible = true;
                        btnRReject.Visible = true;
                    }
                    if (txtPCRReject.Text == "N" && hdnHeadRetCnt.Value == "3" && txtSubmitHead.Text.Trim() == "Y" && (Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtApproveHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "N")
                    {
                        btnReject.Visible = false;                        
                    }
                    //commented for PCR rejection at any stage of approval instead of after 3 time return End

                }
               
                
                    //lblTitle.Text = (txtID.Text.Trim() == "0" && txtID.Text.Trim() != "") ? "New PCR" :  
                    //                (txtApproveRSM.Text.Trim() == "Y") ? "Approved PCR By RSM" : 
                    //                (txtApproveASM.Text.Trim() == "Y") ? "Approved PCR By ASM" : 
                    //                (txtApprove.Text.Trim() == "Y") ? "Approved PCR By CSM" :                                     
                    //                (txtReject.Text.Trim() == "Y") ? "Rejected PCR By CSM" :
                    //                (txtRejectASM.Text.Trim() == "Y") ? "Rejected PCR By ASM" : 
                    //                (txtRejectRSM.Text.Trim() == "Y") ? "Rejected PCR By RSM" :                                    
                    //                (txtIsSubmit.Text.Trim() == "Y") ? "Submitted PCR By RSM" :"Saved PCR";


                //if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" && txtIsSubmit.Text.Trim() == "N" && txtReject.Text.Trim() == "N" && txtSubmitASM.Text.Trim() == "N" && txtSubmitRSM.Text.Trim() == "N" && txtSubmitHead.Text.Trim() == "N") btnSave.Visible = true;
                if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" && txtSubmitASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N" && txtSubmitRSM.Text.Trim() == "N" && txtSubmitHead.Text.Trim() == "N") btnSave.Visible = true;

                //btnSubmit.Visible = ((txtIsSubmit.Text.Trim() == "N" || (txtSubmitASM.Text.Trim() == "N" && txtApprove.Text.Trim() == "Y") || (txtSubmitRSM.Text.Trim() == "N" && txtApproveASM.Text.Trim() == "Y") || (txtSubmitHead.Text.Trim() == "N" && txtApproveRSM.Text.Trim() == "Y")) && (txtID.Text.Trim() != "0" && txtID.Text.Trim() != "") && Func.Convert.sConvertToString(Session["UserRole"]) == "6") ? true : false;
                //btnSubmit.Visible = ((txtIsSubmit.Text.Trim() == "N" || (txtSubmitHead.Text.Trim() == "N" && txtApprove.Text.Trim() == "Y")) && (txtID.Text.Trim() != "0" && txtID.Text.Trim() != "") && Func.Convert.sConvertToString(Session["UserRole"]) == "6") ? true : false;
                //btnSubmit.Visible = (txtPCRReject.Text == "N" && (txtIsSubmit.Text.Trim() == "N" || (txtSubmitHead.Text.Trim() == "N" && txtApprove.Text.Trim() == "Y")) && (txtID.Text.Trim() != "0" && txtID.Text.Trim() != "") && Func.Convert.sConvertToString(Session["UserRole"]) == "6") ? true : false;
                btnSubmit.Visible = (txtPCRReject.Text == "N" && (txtSubmitASM.Text.Trim() == "N" || (txtSubmitHead.Text.Trim() == "N" && txtApproveASM.Text.Trim() == "Y")) && (txtID.Text.Trim() != "0" && txtID.Text.Trim() != "") && Func.Convert.sConvertToString(Session["UserRole"]) == "6") ? true : false;

                if (Display =="Y")
                {
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    btnSubmit.Visible = false;
                    btnSave.Visible = false;
                    btnRReject.Visible = false;
                }

                //Display Complaints Details     
                dtComplaint = ds.Tables[1];
                Session["ComplaintsDetails"] = dtComplaint;
                lblComplaintsRecCnt.Text = Func.Common.sRowCntOfTable(dtComplaint);
                ComplaintsGrid.DataSource = dtComplaint;
                ComplaintsGrid.DataBind();
                
                // Display Action Details     
                dtActionTaken = ds.Tables[2];
                Session["ActionDetails"] = dtActionTaken;
                lblActionsRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtActionTaken)) == 0) ? Func.Common.sRowCntOfTable(dtActionTaken) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtActionTaken)));
                ActionsGrid.DataSource = dtActionTaken;
                ActionsGrid.DataBind();                

                //Display Free Service Details        
                Session["ServiceDtls"] = null;
                dtService = ds.Tables[3];
                Session["ServiceDtls"] = dtService;
                lblServicesRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtService)) == 0) ? Func.Common.sRowCntOfTable(dtService) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtService)) - 1);
                ServicesGrid.DataSource = dtService;
                ServicesGrid.DataBind();

                //Display Attachment Details
                dtFileAttach = ds.Tables[4];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();

                MakeEnableDisableControls();
                
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // To enable or disable fields
        private void MakeEnableDisableControls()
        {
           bool bEnable = false;                      

           //if (txtIsSubmit.Text.Trim() == "N" && txtApprove.Text.Trim() == "N" && txtReject.Text.Trim() == "N")
           if (txtSubmitASM.Text.Trim() == "N" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N")
               bEnable = true;
           else
               bEnable = false;

               Display = Func.Convert.sConvertToString(Request.QueryString["Display"]);
               if (Display == "Y") bEnable = false;

               drpSalesRegID.Enabled = bEnable;
               DrpSupervisorName.Enabled = bEnable;
               txtPrmPartBatchID.Enabled = bEnable;
               txtVehAppl.Enabled = bEnable;
               txtNatLoadCar.Enabled = bEnable;
               txtPayLoad.Enabled = bEnable;
               txtAvgPerDay.Enabled = bEnable;
               txtRoadCond.Enabled = bEnable;
               txtFuelCons.Enabled = bEnable;
               txtEngOilCons.Enabled = bEnable;

               txtDtlDesc.Enabled = (txtIsSubmit.Text.Trim() == "N" || txtReject.Text.Trim() == "Y") ? true :
                                    (txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" && (txtSubmitASM.Text.Trim() == "N" || txtRejectASM.Text.Trim() == "Y")) ? true :
                                    (txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" && txtSubmitASM.Text.Trim() == "Y" && txtApproveASM.Text.Trim() == "Y" && (txtSubmitRSM.Text.Trim() == "N" || txtRejectRSM.Text.Trim() == "Y")) ? true :
                                    (txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" && txtSubmitASM.Text.Trim() == "Y" && txtApproveASM.Text.Trim() == "Y"
                                    && txtSubmitRSM.Text.Trim() == "Y" && txtApproveRSM.Text.Trim() == "Y" && (txtSubmitHead.Text.Trim() == "N" || txtRejectHead.Text.Trim() == "Y")) ? true : bEnable;

               txtObservMSE.Enabled = false; // (txtIsSubmit.Text.Trim() == "N" || Func.Convert.sConvertToString(Session["UserRole"]) != "4") ? false : true;
               txtObservASE.Enabled = (txtSubmitASM.Text.Trim() == "N" || Func.Convert.sConvertToString(Session["UserRole"]) != "3") ? false : true;
               txtObservRSM.Enabled = false; // (txtSubmitRSM.Text.Trim() == "N" || Func.Convert.sConvertToString(Session["UserRole"]) != "2") ? false : true;
               txtObservHead.Enabled = (txtSubmitHead.Text.Trim() == "N" || (Func.Convert.sConvertToString(Session["UserRole"]) != "1" && Func.Convert.iConvertToInt(Session["UserID"]) != iUserPCRHeadApprID)) ? false : true;

               if (Display == "Y")
               {
                   txtDtlDesc.Enabled = bEnable;
                   txtObservMSE.Enabled = false;// bEnable;
                   txtObservASE.Enabled = bEnable;
                   txtObservRSM.Enabled = false;// bEnable;
                   txtObservHead.Enabled = bEnable;
                   txtDtlDesc.Enabled = bEnable;                   
               }
               trNewAttachment.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");
               trNewAttachment1.Style.Add("display", (txtUserType.Text == "6") ? "none" : "");                
        }
        
        
        private bool bFillDetailsFromPCRDetails()
        {
            string sStatus = "";            
            int iCntForDelete = 0;
            //int iJobCodeID = 0;
            int iPartID = 0;
            bool bValidate = false;
            string sWarrantablePart = "";
            

            //if (iCntForDelete == JobDetailsGrid.Rows.Count)
            //{
            //    bValidate = false;
            //}
            
           
            
            if (bValidate == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
            }
            return bValidate;
        }

        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;


                dtHdr.Columns.Add(new DataColumn("PCRHDRID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PCRNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PCRDate", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Jobcard_Hdr_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("JobCodeID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PrimFailPartSuppID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("PrimFailPartBatch", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("VehicleAppl", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Payload", typeof(string)));                
                dtHdr.Columns.Add(new DataColumn("NatureLoadCar", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("AvgRunPerDay", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("RoadCondition", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("FuelConsump", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("EngOilConsump", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("InspectedBy", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("SalesRegionID", typeof(int)));                
                dtHdr.Columns.Add(new DataColumn("DtlDescription", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ObservByMSECSM", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ObservByASM", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ObservByRSM", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("ObservByHead", typeof(string)));
                                              
                dr = dtHdr.NewRow();
                dr["PCRHDRID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["PCRNo"] = txtDocNo.Text;
                dr["PCRDate"] = txtDocDate.Text;
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(txtDealerID.Text.Trim());
                dr["DlrBranchID"] = Func.Convert.iConvertToInt(txtDealerBrID.Text.Trim());
                dr["Jobcard_Hdr_ID"] = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                dr["JobCodeID"] = Func.Convert.iConvertToInt(txtJobCodeID.Text);
                
                dr["PrimFailPartSuppID"] = Func.Convert.iConvertToInt(hdnPrmPartSupplier.Value);
                dr["PrimFailPartBatch"] = txtPrmPartBatchID.Text;
                dr["VehicleAppl"] = txtVehAppl.Text;
                dr["NatureLoadCar"] = txtNatLoadCar.Text;
                dr["Payload"] = Func.Convert.sConvertToString(txtPayLoad.Text);
                dr["AvgRunPerDay"] = txtAvgPerDay.Text;
                dr["RoadCondition"] = txtRoadCond.Text;
                dr["FuelConsump"] = txtFuelCons.Text;
                dr["EngOilConsump"] = txtEngOilCons.Text;
                dr["SalesRegionID"] = Func.Convert.iConvertToInt(drpSalesRegID.SelectedValue.ToString());
                dr["InspectedBy"] = Func.Convert.iConvertToInt(DrpSupervisorName.SelectedValue.ToString());

                dr["DtlDescription"] = txtDtlDesc.Text;
                dr["ObservByMSECSM"] = txtObservMSE.Text;
                dr["ObservByASM"] = txtObservASE.Text;
                dr["ObservByRSM"] = txtObservRSM.Text;
                dr["ObservByHead"] = txtObservHead.Text;

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        #region File Attachment Functions
        // To Save the attach Document 
        private bool bSaveAttachedDocuments()
        {
            // Get Details Of The Existing file attach
            bFillDetailsFromFileAttachGrid();

            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            string sPath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
            string sSourceFileName = "";
            string sSourceFileName1 = "";
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

                        string[] splClaimNo = txtDocNo.Text.Split('/');
                        if (splClaimNo.Length > 1)
                        {
                            lblFileName.Text = "";
                            for (int iCnt = 0; iCnt < splClaimNo.Length; iCnt++)
                                lblFileName.Text = lblFileName.Text + splClaimNo[iCnt];
                        }                        
                        sSourceFileName1 = Func.Convert.sConvertToString(Request.QueryString["DealerID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                        sSourceFileName1 = sSourceFileName1.Replace("/", "");
                        dr["File_Names"] = sSourceFileName1;

                       // dr["File_Names"] = Func.Convert.sConvertToString(iDealerId) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName;
                        dr["UserId"] = Func.Convert.sConvertToString(iDealerId);
                        dr["Status"] = "S";

                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();

                        //Saving it in temperory Directory.                                       
                        DirectoryInfo destination = new DirectoryInfo(sPath + "JobPCR");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }
                            uploads[i].SaveAs((sPath + "JobPCR" + "\\" + Func.Convert.sConvertToString(Request.QueryString["DealerID"]) + "_" + Func.Convert.sConvertToString(lblFileName.Text) + "_" + sSourceFileName + ""));                        
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
            CheckBox ChkForDelete;
            for (int iGridRowCnt = 0; iGridRowCnt < FileAttchGrid.Rows.Count; iGridRowCnt++)
            {
                if ((FileAttchGrid.Rows[iGridRowCnt].FindControl("lblFile") as Label).Text.Trim() != "")
                {
                    dr = dtFileAttach.NewRow();
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
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();
                if (bValidateRecord() == false)
                {

                }
                else
                {
                    UpdateHdrValueFromControl(dtHdr);

                    if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" &&
                       (txtSubmitASM.Text.Trim() == "N" && txtApproveASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "N")
                       || (txtRejectASM.Text.Trim() == "Y"))
                    {
                        txtSubmitASM.Text = "Y";
                        txtRejectASM.Text = "N";
                    }
                    if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" &&
                           txtSubmitASM.Text.Trim() == "Y" && txtApproveASM.Text.Trim() == "Y" &&
                           txtSubmitHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "Y")
                    {
                        txtSubmitHead.Text = "Y";
                        txtRejectHead.Text = "N";
                    }

                    //if(Func.Convert.sConvertToString(Session["UserRole"]) == "6" &&
                    //       txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" &&
                    //       txtSubmitASM.Text.Trim() == "N" && txtRejectASM.Text.Trim() == "Y") 
                    //{
                    //    txtSubmitASM.Text = "Y" ;
                    //    txtRejectASM.Text = "N";
                    //}

                    //if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" &&
                    //        txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" && txtSubmitASM.Text.Trim() == "Y" && txtApproveASM.Text.Trim() == "Y"
                    //        && txtSubmitRSM.Text.Trim() == "N" && txtRejectRSM.Text.Trim() == "Y")
                    //{
                    //    txtSubmitRSM.Text = "Y";
                    //    txtRejectRSM.Text = "N";
                    //}
                    //if (Func.Convert.sConvertToString(Session["UserRole"]) == "6" &&
                    //        txtIsSubmit.Text.Trim() == "Y" && txtApprove.Text.Trim() == "Y" && txtSubmitASM.Text.Trim() == "Y" && txtApproveASM.Text.Trim() == "Y"
                    //        && txtSubmitRSM.Text.Trim() == "Y" && txtApproveRSM.Text.Trim() == "Y" && txtSubmitHead.Text.Trim() == "N" && txtRejectHead.Text.Trim() == "Y")
                    //{
                    //    txtSubmitHead.Text = "Y";
                    //    txtRejectHead.Text = "N";
                    //}


                    //Get File Attach
                    if (bSaveAttachedDocuments() == true)
                    {

                        if (ObjJobcard.bSaveJobcodePCRDetails(ref iPCRHDRID, dtHdr, txtIsSubmit.Text.Trim(),
                        txtDlrCode.Text.Trim().ToString(), 0, dtFileAttach, txtApprove.Text.Trim(), txtReject.Text.Trim(),
                        txtSubmitASM.Text.Trim(), txtApproveASM.Text.Trim(), txtRejectASM.Text.Trim(),
                        txtSubmitRSM.Text.Trim(), txtApproveRSM.Text.Trim(), txtRejectRSM.Text.Trim(), txtSubmitHead.Text.Trim(), txtApproveHead.Text.Trim(), txtRejectHead.Text.Trim(),"N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " Get Submit successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();
                if (//(Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtObservMSE.Text.Trim()=="") ||
                    (Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtObservASE.Text.Trim() == "")||
                    //(Func.Convert.sConvertToString(Session["UserRole"]) == "2" && txtObservRSM.Text.Trim() == "")||
                    ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtObservHead.Text.Trim() == ""))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Observations.')</script>");
                }
                else
                {
                    UpdateHdrValueFromControl(dtHdr);

                    //Get File Attach
                    if (bSaveAttachedDocuments() == true)
                    {
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "4")
                        //{
                        //    txtApprove.Text = "Y";
                        //    txtSubmitHead.Text = "Y";
                        //}
                        if (Func.Convert.sConvertToString(Session["UserRole"]) == "3")
                        {
                            txtApproveASM.Text = "Y";
                            txtSubmitHead.Text = "Y";
                        }
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "2")
                        //{
                        //    txtApproveRSM.Text = "Y";
                        //    txtSubmitHead.Text = "Y";
                        //}
                        if ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID))
                        {
                            txtApproveHead.Text = "Y";
                        }

                        if (ObjJobcard.bSaveJobcodePCRDetails(ref iPCRHDRID, dtHdr,
                            txtIsSubmit.Text.Trim(), txtDlrCode.Text.Trim().ToString(), 0, dtFileAttach,
                            txtApprove.Text.Trim(), txtReject.Text.Trim(), txtSubmitASM.Text.Trim(),
                            txtApproveASM.Text.Trim(), txtRejectASM.Text.Trim(), txtSubmitRSM.Text.Trim(),
                            txtApproveRSM.Text.Trim(), txtRejectRSM.Text.Trim(), txtSubmitHead.Text.Trim(), txtApproveHead.Text.Trim(), txtRejectHead.Text.Trim(),"N") == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " Get Approved successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();
                if (//(Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtObservMSE.Text.Trim() == "") ||
                    (Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtObservASE.Text.Trim() == "") ||
                    //(Func.Convert.sConvertToString(Session["UserRole"]) == "2" && txtObservRSM.Text.Trim() == "") ||
                    ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtObservHead.Text.Trim() == ""))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Observations.')</script>");
                }
                else
                {
                    UpdateHdrValueFromControl(dtHdr);

                    //Get File Attach
                    if (bSaveAttachedDocuments() == true)
                    {
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "4") txtReject.Text = "Y";
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "4") txtIsSubmit.Text = "N";
                        
                        if (Func.Convert.sConvertToString(Session["UserRole"]) == "3") txtRejectASM.Text = "Y";
                        if (Func.Convert.sConvertToString(Session["UserRole"]) == "3") txtSubmitASM.Text = "N";

                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "2") txtRejectRSM.Text = "Y";
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "2") txtSubmitRSM.Text = "N";

                        if ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID)) txtRejectHead.Text = "Y";
                        if ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID)) txtSubmitHead.Text = "N";

                        if (ObjJobcard.bSaveJobcodePCRDetails(ref iPCRHDRID, dtHdr, txtIsSubmit.Text.Trim(), txtDlrCode.Text.Trim().ToString(), 0, dtFileAttach, txtApprove.Text.Trim(),
                            txtReject.Text.Trim(), txtSubmitASM.Text.Trim(), txtApproveASM.Text.Trim(), txtRejectASM.Text.Trim(), txtSubmitRSM.Text.Trim(),
                            txtApproveRSM.Text.Trim(), txtRejectRSM.Text.Trim(), txtSubmitHead.Text.Trim(), txtApproveHead.Text.Trim(), txtRejectHead.Text.Trim(), "N") == true)
                        {
                            if (btnReject.Text == "Return PCR")
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " Get Returned.')</script>");
                            }
                            else
                            {
                                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " Get Rejected.')</script>");
                            }
                            txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void btnRReject_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();
                if (//(Func.Convert.sConvertToString(Session["UserRole"]) == "4" && txtObservMSE.Text.Trim() == "") ||
                    (Func.Convert.sConvertToString(Session["UserRole"]) == "3" && txtObservASE.Text.Trim() == "") ||
                    //(Func.Convert.sConvertToString(Session["UserRole"]) == "2" && txtObservRSM.Text.Trim() == "") ||
                    ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) && txtObservHead.Text.Trim() == ""))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Observations.')</script>");
                }
                else
                {
                    UpdateHdrValueFromControl(dtHdr);

                    //Get File Attach
                    if (bSaveAttachedDocuments() == true)
                    {
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "4") txtReject.Text = "Y";
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "4") txtIsSubmit.Text = "N";

                        if (Func.Convert.sConvertToString(Session["UserRole"]) == "3") txtRejectASM.Text = "Y";
                        if (Func.Convert.sConvertToString(Session["UserRole"]) == "3") txtSubmitASM.Text = "N";

                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "2") txtRejectRSM.Text = "Y";
                        //if (Func.Convert.sConvertToString(Session["UserRole"]) == "2") txtSubmitRSM.Text = "N";

                        if ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID)) txtRejectHead.Text = "Y";
                        if ((Func.Convert.sConvertToString(Session["UserRole"]) == "1" || Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID)) txtSubmitHead.Text = "N";

                        if (ObjJobcard.bSaveJobcodePCRDetails(ref iPCRHDRID, dtHdr, txtIsSubmit.Text.Trim(), txtDlrCode.Text.Trim().ToString(), 0, dtFileAttach, txtApprove.Text.Trim(),
                            txtReject.Text.Trim(), txtSubmitASM.Text.Trim(), txtApproveASM.Text.Trim(), txtRejectASM.Text.Trim(), txtSubmitRSM.Text.Trim(),
                            txtApproveRSM.Text.Trim(), txtRejectRSM.Text.Trim(), txtSubmitHead.Text.Trim(), txtApproveHead.Text.Trim(), txtRejectHead.Text.Trim(), "Y") == true)
                        {

                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('PCR " + txtDocNo.Text + " Get Rejected.')</script>");

                            txtID.Text = Func.Convert.sConvertToString(iPCRHDRID);
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        
                        
    }
}