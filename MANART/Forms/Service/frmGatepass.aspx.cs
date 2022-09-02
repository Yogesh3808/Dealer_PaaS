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
    public partial class frmGatepass : System.Web.UI.Page
    {
        private DataTable dtJob = new DataTable();
        private int iJobHDRID = 0;
        private int iDealerId = 0;
        private int iJobCodeID = 0;
        private int iGPHDRID = 0;
        private DataTable dtComplaint = new DataTable();
        private DataTable dtActionTaken = new DataTable();
        private DataTable dtService = new DataTable();
        private DataTable dtFileAttach = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                ExpirePageCache();


                if (!IsPostBack)
                {
                    lblTitle.Text = "Gatepass Details";
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
                DataTable dtHdr = new DataTable();
                clsJobcard ObjJobcard = new clsJobcard();
                UpdateHdrValueFromControl(dtHdr);
                //Get File Attach
                if (ObjJobcard.bSaveGatepass(ref iGPHDRID, dtHdr, txtDlrCode.Text.Trim().ToString(), 0) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('GatePass " + txtDocNo.Text + " get saved successfully.')</script>");
                    txtID.Text = Func.Convert.sConvertToString(iGPHDRID);
                    GetDataAndDisplay();
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
                iJobHDRID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iGPHDRID = Func.Convert.iConvertToInt(Request.QueryString["GPID"]);

                if (iGPHDRID != 0 && iGPHDRID > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(iGPHDRID);
                    GetDataAndDisplay();
                }
                else
                    DisplayPreviousRecord();
                //if (iGPHDRID != 0 && iGPHDRID > 0 && iJobHDRID == 0)
                //{
                //    trJobcardDetails.visible = false;
                //    trchassisDetails.visible = false;
                //    trVahicleDetails.visible = false;
                //}

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
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iGPHDRID = Func.Convert.iConvertToInt(Request.QueryString["GPID"]);

                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_GatePass", iGPHDRID, "NEW", iJobHDRID);
                if (ds != null) // if no Data Exist
                {
                    DisplayData(ds);
                }
                txtID.Text = "";
                txtDocNo.Text = Func.Common.sGetMaxDocNo(txtDlrCode.Text.Trim(), "", "GPJ", iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false); 
                txtDocDate.Text = Func.Common.sGetCurrentDateTime(1, true);
                lblTitle.Text = "GatePass No:" + txtDocNo.Text;
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
                iGPHDRID = Func.Convert.iConvertToInt(txtID.Text);

                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_GatePass", iGPHDRID, "All", 0);
                if (ds != null) // if no Data Exist
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GPHDRID"]);
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

                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gp_No"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gp_date"]);

                txtPreviousDocId.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RefJobcardID"]);

                txtJobNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobCardNo"]);
                txtJbInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JbINVNo"]);
                txtSlInvNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SalesInvNo"]);

                txtVIN.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
                txtEngNr.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);

                txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_Name"]);
                txtVehRegNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Veh_Reg_No"]);
                txtDlrCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerCode"]);

                txtFailDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobDate"]);
                txtDealerBrID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DlrBranchID"]);
                txtDealerID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);
                txtNarr.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Narr"]);
                txtGPType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GPtype"]);

                lblTitle.Text = "GatePass No:" + txtDocNo.Text;

                if (iGPHDRID != 0 && iGPHDRID > 0 && txtGPType.Text.Trim() == "C")
                {
                    trJobcardDetails.Visible = false;
                    trchassisDetails.Visible = false;
                    trVahicleDetails.Visible = false;
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


                dtHdr.Columns.Add(new DataColumn("GPHDRID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Gp_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Gp_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("RefJobcardID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("RefJbInvID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("RefSlInvID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("GPtype", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Narr", typeof(string)));


                dr = dtHdr.NewRow();
                dr["GPHDRID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Gp_No"] = txtDocNo.Text;
                dr["Gp_date"] = txtDocDate.Text;
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(txtDealerID.Text.Trim());
                dr["DlrBranchID"] = Func.Convert.iConvertToInt(txtDealerBrID.Text.Trim());
                dr["RefJobcardID"] = Func.Convert.iConvertToInt(txtPreviousDocId.Text);
                dr["RefJbInvID"] = 0;
                dr["RefSlInvID"] = 0;
                //dr["GPtype"] = "J";
                dr["GPtype"] = Func.Convert.sConvertToString(txtGPType.Text);
                dr["Narr"] = txtNarr.Text;

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

    }
}