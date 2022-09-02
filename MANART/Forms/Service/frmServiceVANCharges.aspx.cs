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
    public partial class frmServiceVANCharges : System.Web.UI.Page
    {
        private DataTable dtJob = new DataTable();
        private int iJobHDRID = 0;
        private int iClaimHDRID = 0;
        private int iGCRHDRID = 0;
        private int iDealerId = 0;        
        private int iSrvVANHDRID = 0;
        private DataTable dtLabour = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();

                txtUserType.Text = Session["UserType"].ToString();

                if (!IsPostBack)
                {
                    lblTitle.Text = "Service VAN Charges";
                    DisplayRecord();
                }
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
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iClaimHDRID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iGCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["GCRID"]);

                bool bSave = true;
                string sMessage = " Please enter/select records.";
                if(txtFrmLocation.Text.Trim()=="")
                {
                    sMessage = sMessage + "\\n Enter the from Location.";
                    bSave = false;
                }
                if (DrpLabType.SelectedValue.Trim() == "N")
                {
                    sMessage = sMessage + "\\n Enter the from Labour Type.";
                    bSave = false;
                }
                if ((DrpSrvVAN.SelectedValue.Trim() == "V" || DrpSrvVAN.SelectedValue.Trim() == "M") && Func.Convert.dConvertToDouble(txtStKms.Text.Trim())==0 )
                {
                    sMessage = sMessage + "\\n Enter the Start Kms.";
                    bSave = false;
                }
                if ((DrpSrvVAN.SelectedValue.Trim() == "V" || DrpSrvVAN.SelectedValue.Trim() == "M") && Func.Convert.dConvertToDouble(txtEndKms.Text.Trim()) ==0)
                {
                    sMessage = sMessage + "\\n Enter the End Kms.";
                    bSave = false;
                }
                if ((DrpSrvVAN.SelectedValue.Trim() =="V" || DrpSrvVAN.SelectedValue.Trim()  =="M") && Func.Convert.dConvertToDouble(txtStKms.Text.Trim()) > Func.Convert.dConvertToDouble(txtEndKms.Text.Trim()))
                {
                    sMessage = sMessage + "\\n End Kms should be greater than Start Kms.";
                    bSave = false;
                }
                if (txtDistKm.Text.Trim() == "" || Func.Convert.dConvertToDouble(txtDistKm.Text.Trim()) == 0)
                {
                    sMessage = sMessage + "\\n Enter the Distance Travelled One Way(In Km).";
                    bSave = false;
                }
                if (txtNoTrip.Text.Trim() == "" || Func.Convert.iConvertToInt(txtNoTrip.Text.Trim()) == 0)
                {
                    sMessage = sMessage + "\\n Enter the No Of trip.";
                    bSave = false;
                }
                if (txtFailDate.Text.Trim() == "")
                {
                    sMessage = sMessage + "\\n Enter the complaint date.";
                    bSave = false;
                }
                if (txtTripRate.Text.Trim() == "" || Func.Convert.dConvertToDouble(txtTripRate.Text.Trim()) == 0)
                {
                    sMessage = sMessage + "\\n Enter the Trip rate.";
                    bSave = false;
                }
                if (txtToLocation.Text.Trim() == "" )
                {
                    sMessage = sMessage + "\\n Enter the To Location.";
                    bSave = false;
                }
                if (DrpSrvVAN.SelectedValue.Trim() == "N")
                {
                    sMessage = sMessage + "\\n select the Service VAN Type.";
                    bSave = false;
                }
                if (txtVANOutDate.Text.Trim() == "")
                {
                    sMessage = sMessage + "\\n select the VAN Out time.";
                    bSave = false;
                }
                if (drpPMechanic.SelectedValue.Trim() == "0" && iJobHDRID > 0)
                {
                    sMessage = sMessage + "\\n select the Mechanic.";
                    bSave = false;
                }
                DateTime dtSuppliedDate = DateTime.Parse(txtVANOutDate.Text);
                DateTime dtSuppliedDate1 = DateTime.Parse(txtFailDate.Text);
                if (dtSuppliedDate < dtSuppliedDate1)
                {
                    sMessage = sMessage + "\\n VAN Out Time should be greater than Complaint date.";
                    bSave = false;
                }

                if (bSave == false) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                if (bSave == true)
                {
                    DataTable dtHdr = new DataTable();
                    clsJobcard ObjJobcard = new clsJobcard();
                    clsWarranty ObjWarranty = new clsWarranty();

                    UpdateHdrValueFromControl(dtHdr);
                    iSrvVANHDRID = Func.Convert.iConvertToInt(txtID.Text);
                    //Get File Attach
                    if(iClaimHDRID>0)
                    {
                        if (ObjWarranty.bSaveWarrantyServiceVAN(ref iSrvVANHDRID, dtHdr, txtDlrCode.Text.Trim().ToString(), 0) == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Service VAN Details saved successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iSrvVANHDRID);
                            GetDataAndDisplay();
                        }
                    }
                    else if (iGCRHDRID > 0)
                    {
                        if (ObjWarranty.bSaveGCRServiceVAN(ref iSrvVANHDRID, dtHdr, txtDlrCode.Text.Trim().ToString(), 0) == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Service VAN Details saved successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iSrvVANHDRID);
                            GetDataAndDisplay();
                        }
                    }
                    else
                    {
                        if (ObjJobcard.bSaveJobcardServiceVAN(ref iSrvVANHDRID, dtHdr, txtDlrCode.Text.Trim().ToString(), 0) == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Service VAN Details saved successfully.')</script>");
                            txtID.Text = Func.Convert.sConvertToString(iSrvVANHDRID);
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

        public void DisplayRecord()
        {
            try
            {
                iClaimHDRID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iGCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["GCRID"]);
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iSrvVANHDRID = Func.Convert.iConvertToInt(Request.QueryString["SrvVANHDRID"]);
                txtWarrantyTag.Text= Func.Convert.sConvertToString(Request.QueryString["sWarrTag"]);
                txtAggregate.Text = Func.Convert.sConvertToString(Request.QueryString["sAggregate"]); 
                    

                Func.Common.BindDataToCombo(drpPMechanic, clsCommon.ComboQueryType.Employee, iDealerId, " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type = 2");                
                DrpSrvVAN.Items.Add(new ListItem("Select", "N", true));
                DrpSrvVAN.Items.Add(new ListItem("Motorcycle", "M", true));
                DrpSrvVAN.Items.Add(new ListItem("Public Transport", "P", true));                
                DrpSrvVAN.Items.Add(new ListItem("VAN", "V", true));

                if (iSrvVANHDRID != 0 && iSrvVANHDRID > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(iSrvVANHDRID);
                    GetDataAndDisplay();
                    DrpSrvVAN.Enabled = false;
                }
                else
                {
                    DrpSrvVAN.SelectedValue = "N";
                    DisplayPreviousRecord();
                    DrpSrvVAN.Enabled = true;
                }
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
                iClaimHDRID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iGCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["GCRID"]);
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iSrvVANHDRID = Func.Convert.iConvertToInt(Request.QueryString["SrvVANHDRID"]);
                txtWarrantyTag.Text = Func.Convert.sConvertToString(Request.QueryString["sWarrTag"]);
                txtAggregate.Text = Func.Convert.sConvertToString(Request.QueryString["sAggregate"]);
                if (iClaimHDRID > 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_ClaimServiceVAN", iSrvVANHDRID, "NEW", iClaimHDRID, DrpSrvVAN.SelectedValue);
                }
                else if (iGCRHDRID > 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_GCRServiceVAN", iSrvVANHDRID, "NEW", iGCRHDRID, DrpSrvVAN.SelectedValue);
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_JobcardServiceVAN", iSrvVANHDRID, "NEW", iJobHDRID, DrpSrvVAN.SelectedValue);
                }

                if (ds != null) // if no Data Exist
                {
                    DisplayData(ds);
                }
                txtID.Text = "";                
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);                 
                lblTitle.Text = "Service VAN Chartges :" + txtJobNo.Text;
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
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iClaimHDRID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iGCRHDRID = Func.Convert.iConvertToInt(Request.QueryString["GCRID"]);

                iSrvVANHDRID = Func.Convert.iConvertToInt(txtID.Text);
                if (iClaimHDRID > 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_ClaimServiceVAN", iSrvVANHDRID, "All", 0, "N");
                }
                else if (iGCRHDRID > 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_GCRServiceVAN", iSrvVANHDRID, "All", 0, "N");
                }
                else
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_JobcardServiceVAN", iSrvVANHDRID, "All", 0, "N");
                }

                if (ds != null) // if no Data Exist
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SrvVANHDRID"]);
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

                txtPreviousDocId.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Jobcard_HDR_ID"]);
                txtJobNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCardNo"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Complaint_Tm"]);
                txtJobTypeID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Job_Ty_ID"]);
                                
                if (iClaimHDRID >0)
                {
                    if (Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 16)
                    {
                        DrpLabType.Items.Add(new ListItem("Goodwill", "W", true));
                    }
                    else
                    {
                        DrpLabType.Items.Add(new ListItem("Warranty", "W", true));
                    }
                }
                else if (iGCRHDRID > 0)
                {
                    if (Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 16)
                    {
                        DrpLabType.Items.Add(new ListItem("Goodwill", "W", true));
                    }
                    else
                    {
                        DrpLabType.Items.Add(new ListItem("Warranty", "W", true));
                    }

                }
                else
                {
                    DrpLabType.Items.Add(new ListItem("Select", "N", true));
                    DrpLabType.Items.Add(new ListItem("Paid", "P", true));
                    DrpLabType.Items.Add(new ListItem("FOC", "F", true));
                    if (Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 5)
                    {
                        //DrpLabType.Items.Add(new ListItem("SA", "A", true));
                        DrpLabType.Items.Add(new ListItem("RMC", "A", true));
                        if (txtWarrantyTag.Text.Trim() != "N" || txtAggregate.Text.Trim() == "G") DrpLabType.Items.Add(new ListItem("Warranty", "W", true));
                    }
                    else if (Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 10 || Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 15)
                    {
                        if (txtWarrantyTag.Text.Trim() != "N" || txtAggregate.Text.Trim() == "G") DrpLabType.Items.Add(new ListItem("Warranty", "W", true));
                    }
                    else if (Func.Convert.iConvertToInt(txtJobTypeID.Text.ToString()) == 12)
                    {
                        DrpLabType.Items.Add(new ListItem("Enroute Technical", "E", true));
                        DrpLabType.Items.Add(new ListItem("Enroute non-Technical", "R", true));
                        DrpLabType.Items.Add(new ListItem("Transit", "T", true));
                    }
                }

                txtFrmLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["From_Location"]);
                txtToLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["To_Location"]);

                txtDistKm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["One_Way_Dist"]);
                txtNoTrip.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NO_Trips"]);

                DrpSrvVAN.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TravelMode"]);
                txtFailDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Complaint_Tm"]);
                txtVANOutDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Mech_VAN_Out_Tm"]);

                txtStKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["StKms"]);
                txtEndKms.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["EndKms"]);

                txtTripRate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Serv_Rate"]);
                txtTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Tot_Amt"]);
                DrpLabType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabType"]);

                Func.Common.BindDataToCombo(drpPMechanic, clsCommon.ComboQueryType.Employee, Func.Convert.iConvertToInt(Request.QueryString["DealerID"]), " and HOBrID=" + Session["HOBR_ID"].ToString() + " and Empl_Type = 2"
                + ((Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["SrvMechID"]) > 0) ? " or ID = " + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SrvMechID"]) : ""));                
                
                drpPMechanic.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SrvMechID"]);

                txtJobConfirm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["job_confirm"]).Trim();
                lblTitle.Text = "Service VAN Charges:" + txtJobNo.Text;

                btnSave.Visible = (txtJobConfirm.Text.Trim() == "N") ? true : false;

                Session["LabourDetails"] = null;
                dtLabour = ds.Tables[1];
                Session["LabourDetails"] = dtLabour;

                if (DrpSrvVAN.SelectedValue == "P")
                {
                    txtTripRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this);");
                    txtTripRate.Attributes.Add("onkeypress", " return CheckForTextBoxValue(event,this,'6');");
                    txtTripRate.Attributes.Add("onBlur", "return SetSrvVANTotal();");
                }
                else
                {
                    txtTripRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                }

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

            bEnable = true;
            if (txtUserType.Text == "6")
            {
                btnSave.Enabled = false;
            }
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

                dtHdr.Columns.Add(new DataColumn("SrvVANHDRID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Jobcard_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("From_Location", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("To_Location", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("One_Way_Dist", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("NO_Trips", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TravelMode", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Complaint_Tm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Mech_VAN_Out_Tm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Serv_Rate", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Tot_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("LabType", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("SrvMechID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("StKms", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("EndKms", typeof(double)));

                dtHdr.Columns.Add(new DataColumn("Claim_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("GCR_HDR_ID", typeof(int)));
                dr = dtHdr.NewRow();

                dr["SrvVANHDRID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Jobcard_HDR_ID"] =Func.Convert.iConvertToInt(txtPreviousDocId.Text);                
                dr["From_Location"] = Func.Convert.sConvertToString(txtFrmLocation.Text.Trim());
                dr["To_Location"] = Func.Convert.sConvertToString(txtToLocation.Text.Trim());
                dr["One_Way_Dist"] = Func.Convert.dConvertToDouble(txtDistKm.Text);
                dr["NO_Trips"] = Func.Convert.iConvertToInt(txtNoTrip.Text);
                dr["TravelMode"] = Func.Convert.sConvertToString(DrpSrvVAN.SelectedValue);
                dr["Complaint_Tm"] = Func.Convert.sConvertToString(txtFailDate.Text);
                dr["Mech_VAN_Out_Tm"] = Func.Convert.sConvertToString(txtVANOutDate.Text);
                dr["Serv_Rate"] =Func.Convert.dConvertToDouble(txtTripRate.Text);
                dr["Tot_Amt"] = Func.Convert.dConvertToDouble(txtTotal.Text);
                dr["LabType"] = Func.Convert.sConvertToString(DrpLabType.SelectedValue);
                dr["SrvMechID"] =Func.Convert.iConvertToInt(drpPMechanic.SelectedValue);
                dr["StKms"] = Func.Convert.dConvertToDouble(txtStKms.Text);
                dr["EndKms"] = Func.Convert.dConvertToDouble(txtEndKms.Text);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        protected void DrpSrvVAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sFrom_Location, sTo_Location, sComplaint_Tm, sMech_VAN_Out_Tm, sLabType;
            double dOne_Way_Dist,dStKms,dEndKms;
            int iNO_Trips, iSrvMechID, iPrvSrvVANHDRID;

            sFrom_Location = Func.Convert.sConvertToString(txtFrmLocation.Text.Trim());
            sTo_Location= Func.Convert.sConvertToString(txtToLocation.Text.Trim());
            dOne_Way_Dist= Func.Convert.dConvertToDouble(txtDistKm.Text);
            iNO_Trips = Func.Convert.iConvertToInt(txtNoTrip.Text);            
            sComplaint_Tm= Func.Convert.sConvertToString(txtFailDate.Text);
            sMech_VAN_Out_Tm = Func.Convert.sConvertToString(txtVANOutDate.Text);
            dStKms = Func.Convert.dConvertToDouble(txtStKms.Text);
            dEndKms = Func.Convert.dConvertToDouble(txtEndKms.Text);
            
            sLabType= Func.Convert.sConvertToString(DrpLabType.SelectedValue);
            iSrvMechID = Func.Convert.iConvertToInt(drpPMechanic.SelectedValue);
            iPrvSrvVANHDRID = Func.Convert.iConvertToInt(txtID.Text);
             if (DrpSrvVAN.SelectedValue=="P")
             {
                 txtTripRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this);");
                txtTripRate.Attributes.Add("onkeypress", " return CheckForTextBoxValue(event,this,'6');");
                txtTripRate.Attributes.Add("onBlur", "return SetSrvVANTotal();");
             }
             else
             {
                 txtTripRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
             }

            DisplayPreviousRecord();

            txtFrmLocation.Text = sFrom_Location;
            txtToLocation.Text = sTo_Location;
            txtDistKm.Text= Func.Convert.sConvertToString(dOne_Way_Dist);
            txtNoTrip.Text = Func.Convert.sConvertToString(iNO_Trips);
            txtFailDate.Text = Func.Convert.sConvertToString(sComplaint_Tm);
            txtVANOutDate.Text = Func.Convert.sConvertToString(sMech_VAN_Out_Tm);
            txtStKms.Text = Func.Convert.sConvertToString(dStKms);
            txtEndKms.Text = Func.Convert.sConvertToString(dEndKms);

            txtID.Text = Func.Convert.sConvertToString(iPrvSrvVANHDRID);
            iSrvVANHDRID = iPrvSrvVANHDRID;
            DrpLabType.SelectedValue = Func.Convert.sConvertToString(sLabType);
            drpPMechanic.SelectedValue = Func.Convert.sConvertToString(iSrvMechID);
            
            txtTotal.Text  = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(dOne_Way_Dist) * 2) * iNO_Trips * Func.Convert.dConvertToDouble(txtTripRate.Text));            
        }         
    }
}