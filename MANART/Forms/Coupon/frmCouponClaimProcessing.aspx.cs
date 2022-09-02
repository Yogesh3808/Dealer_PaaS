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
using System.IO;

namespace MANART.Forms.Coupon
{
    public partial class frmCouponClaimProcessing : System.Web.UI.Page
    {
        private int iClaimID = 0;
        private clsWarranty.enmClaimType ValenmFormUsedFor;
        string sDealerId = "";
        int iUserId = 0;
        int iUserClaimSlabId = 0;
        string sRequestOrClaim = "";
        int iUserRoleId = 0;
        string sDomestic_Export = "";

        private DataTable dtHeaderdata = new DataTable();
        private DataTable dtFileAttach = new DataTable();
        private DataTable dtDetails = new DataTable();

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
            iClaimID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
            //iClaimID = Func.Convert.iConvertToInt(AESEncrytDecry.DecryptStringAES(Request.QueryString["ClaimID"]));

            sRequestOrClaim = Func.Convert.sConvertToString(Request.QueryString["RequestOrClaim"]);

            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            iUserClaimSlabId = Func.Convert.iConvertToInt(Session["UserClaimSlabID"]);
            iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);


            if (!IsPostBack)
            {
                Session["ComplaintsDetails"] = null;
                Session["InvestigationDetails"] = null;
                Session["PartDetails"] = null;
                Session["LabourDetails"] = null;
                Session["LubricantDetails"] = null;
                Session["SubletDetails"] = null;


                if (iClaimID != 0)
                {
                    GetDataAndDisplay();
                }

                string strReportpath;
                strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);


                btnBack.Attributes.Add("onClick", "return CloseCouponClaimProsseingWindow();");
                btnSave.Attributes.Add("onClick", "return checkCouponSelectStatus();");
                btnConfirm.Attributes.Add("onClick", "return CheckBeforeConfirmRecord();");

                ExpirePageCache();
            }


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

        private bool bValidateRecord(string sSaveOrConfirm)
        {
            string sMessage = "";
            //string sMessage = " Please select Coupon.";
            bool bValidateRecord = true;
            
            if (bCheckHeaderFromCouponClaim() == false)
            {                
                bValidateRecord = false;
            }            
            //   if (sSaveOrConfirm=="N")
            //   {
            //for (int iRowCnt = 0; iRowCnt < CouponClaimGrid.Rows.Count; iRowCnt++)
            //  {

            //  System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)CouponClaimGrid.Rows[iRowCnt].FindControl("ChkPart");
            //  if (Chk.Checked == true)
            //  {
            //      bValidateRecord = true;
            //      break;
            //  }
            //  else
            //  {
            //      bValidateRecord = false;
            //  }
            //}
            // }
            //if (bValidateRecord == false)
            //{
            //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            //}
            return bValidateRecord;
        }




        private void GetDataAndDisplay()
        {
            clsCouponClaim ObjCouponClaim = new clsCouponClaim();
            DataSet ds = new DataSet();
            if (iClaimID != 0)
            {

                ds = ObjCouponClaim.GetCouponClaim("All", iClaimID, 0, 0, "");
                lblTitle.Text = "Coupon Selection";


                DisplayData(ds);
            }
            ds = null;
            ObjCouponClaim = null;
        }
        private void BindDataToGrid()
        {

            dtDetails = (System.Data.DataTable)Session["CouponData"];
            lblCouponClaimRecCnt.Text = Func.Common.sRowCntOfTable(dtDetails);
            CouponClaimGrid.DataSource = dtDetails;
            CouponClaimGrid.DataBind();
            SetGridControlProperty();
        }
        // Display Data 
        private void DisplayData(DataSet ds)
        {
            bool bEnableControls = true;
            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }


            txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
            iClaimID = Func.Convert.iConvertToInt(txtID.Text);
            txtDealerAID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerID"]);
            txtDlrcode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dlr_Code"]);
            txtDlrName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Name"]);
            txtCouponClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Coupon_Claim_No"]);
            txtCouponClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]);
            hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Confirm"]);

            txtApprNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CouponInvoiceNo"]);
            txtApprDt.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CouponInvoiceDate"]);

            if (ds.Tables[1].Rows.Count > 0)
            {
                Session["CouponData"] = ds.Tables[1];
                BindDataToGrid();
            }


            if (PFileAttchDetails.Visible == true)
            {
                dtFileAttach = ds.Tables[3];
                lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                ShowAttachedFiles();
            }
            if (hdnConfirm.Value == "Y")
            {
                MakeEnableDisableControls(false);
            }
            else
            {
                MakeEnableDisableControls(true);
            }



        }
        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form        
            txtDlrcode.Enabled = bEnable;
            txtDlrName.Enabled = bEnable;
            txtCouponClaimNo.Enabled = bEnable;
            txtCouponClaimDate.Enabled = bEnable;
            CouponClaimGrid.Enabled = bEnable;
            trNewAttachment.Visible = bEnable;
            trNewAttachment1.Visible = bEnable;
            btnSave.Enabled = bEnable;
            btnConfirm.Enabled = bEnable;

        }
        private void SetGridControlProperty()
        {
            try
            {
                string sDeleteStatus = "";
                int idtRowCnt = 0;
                int iCntOfDelete = 0;
                string sModelID;
                double iClaimAmount = 0;
                dtDetails = (System.Data.DataTable)Session["CouponData"];
                for (int iRowCnt = 0; iRowCnt < CouponClaimGrid.Rows.Count; iRowCnt++)
                {
                    string strReportpath;
                    strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
                    int sJobcard_HDR_ID = Func.Convert.iConvertToInt((CouponClaimGrid.Rows[iRowCnt].FindControl("lblJobID") as Label).Text.Trim());
                    LinkButton lnkSelectPart = (LinkButton)CouponClaimGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowReport_Proforma(this,'" + strReportpath + "'," + sJobcard_HDR_ID + ");");
                    //lnkSelectPart.Attributes.Add("onclick", "return ShowReport_Proforma(this,'" + strReportpath + "');");

                    System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)CouponClaimGrid.Rows[iRowCnt].FindControl("ChkPart");
                    System.Web.UI.WebControls.TextBox txtRejRemark = (System.Web.UI.WebControls.TextBox)CouponClaimGrid.Rows[iRowCnt].FindControl("txtRejRemark");
                    Chk.Attributes.Add("onclick", "return SelectDeleteCheckboxForLabour(this);");
                    if (Func.Convert.sConvertToString(dtDetails.Rows[(CouponClaimGrid.PageIndex * CouponClaimGrid.PageSize) + iRowCnt]["Accept"]) == "Y")
                    {
                        Chk.Checked = true;
                        txtRejRemark.Enabled = false;
                        txtRejRemark.Text = "";
                    }
                    else
                    {
                        Chk.Checked = false;
                        txtRejRemark.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }




        //To Save the record
        private bool bSaveRecord(int iClaimStatus, string sSaveOrConfirm)
        {
            clsCouponClaim ObjCouponClaim = new clsCouponClaim();

            bool bSubmit = false;

            if (bValidateRecord(sSaveOrConfirm) == false)
            {
                return false;
            }
            else
            {


                bSubmit = bSaveAttachedDocuments();
                if (bSubmit == false)
                {
                    return bSubmit;
                }
                bSaveClaimFileAttachDetails(dtFileAttach, Func.Convert.iConvertToInt(txtID.Text));
                bSubmit = bSaveCouponClaimDetails();
                if (bSubmit == false)
                {                  
                    return bSubmit;
                }                
                bSubmit = ObjCouponClaim.bSubmitCouponClaimForSaveConfirm(dtHeaderdata, Func.Convert.iConvertToInt(txtID.Text), sSaveOrConfirm);

                // bSubmit = ObjWarranty.bSubmitClaimForSave(sRequestOrClaim, iClaimID, iUserRoleId, sRemark, sSaveOrSubmit, iClaimStatus, iUserId, iReasonID, iRequestTypeID, txtDomestic_Export.Text, Convert.ToInt32 (hdnDealerID.Value));        
                ObjCouponClaim = null;

                return bSubmit;
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;

            bSubmit = bSaveRecord(1, "N");
            // if (bSaveRecord(1, "N") == false) return;
            // bSubmit = true;
            if (bSubmit == true)
            {
                lblMessage.Text = " Record is saved.!";
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Text = " Record is not saved.";
                lblMessage.Visible = true;
                for (int iRowCnt = 0; iRowCnt < CouponClaimGrid.Rows.Count; iRowCnt++)
                {
                    System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)CouponClaimGrid.Rows[iRowCnt].FindControl("ChkPart");
                    System.Web.UI.WebControls.TextBox txtRejRemark = (System.Web.UI.WebControls.TextBox)CouponClaimGrid.Rows[iRowCnt].FindControl("txtRejRemark");
                    txtRejRemark.Enabled = (Chk.Checked == true) ? false : true;                                         
                }
                return;
            }
            GetDataAndDisplay();
            //lblMessage.Visible = true;
        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GetDataAndDisplay();
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
            }
        }
        private bool bCheckHeaderFromCouponClaim()
        {
            try
            {
                bool bSaveRecord = false;
                CheckBox ChkForCouponStatus;
                TextBox txtRejRemark;
                Label lblcouponno;
                //DataRow dr;
                //dtHeaderdata = new DataTable();
                ////Get Header InFormation        
                //dtHeaderdata.Columns.Add(new DataColumn("ID", typeof(int)));
                //dtHeaderdata.Columns.Add(new DataColumn("Coupon_Status", typeof(string)));
                //dtHeaderdata.Columns.Add(new DataColumn("Reason", typeof(string)));
                
                //for (int iGridRowCnt = 0; iGridRowCnt < CouponClaimGrid.Rows.Count; iGridRowCnt++)
                //{
                //    dr = dtHeaderdata.NewRow();
                //    dr["ID"] = Func.Convert.iConvertToInt((CouponClaimGrid.Rows[iGridRowCnt].FindControl("lblID") as Label).Text);
                //    ChkForCouponStatus = (CheckBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("ChkPart"));
                //    if (ChkForCouponStatus.Checked == true)
                //    {
                //        dr["Coupon_Status"] = "Y";
                //    }
                //    else
                //    {
                //        dr["Coupon_Status"] = "N";
                //    }                    
                //    txtRejRemark = (TextBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("txtRejRemark"));
                //    dr["Reason"] = txtRejRemark.Text.ToString();

                //    dtHeaderdata.Rows.Add(dr);
                //    dtHeaderdata.AcceptChanges();
                //}                
                
                for (int iGridRowCnt = 0; iGridRowCnt < CouponClaimGrid.Rows.Count; iGridRowCnt++)
                {
                    lblcouponno = (Label)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("lblcouponno"));
                    ChkForCouponStatus = (CheckBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("ChkPart"));
                    txtRejRemark = (TextBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("txtRejRemark"));
                    if (ChkForCouponStatus.Checked == false && txtRejRemark.Text.Trim() == "")
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Rejection Remark.');</script>");
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter Rejection Remark for Coupon no : " + lblcouponno.Text.ToString() + "');</script>");
                        txtRejRemark.Focus();
                        return false;
                    }                    
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private bool bSaveCouponClaimDetails()
        {
            try
            {                
                bFillHeaderFromCouponClaim();
                return true;
            }

            catch (Exception ex)
            {
                return false;
                Func.Common.ProcessUnhandledException(ex);
            }


            return true;
        }


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


                        //dr["File_Names"] = sSourceFileName;
                        //dr["File_Names"] = Func.Convert.sConvertToString(txtDealerAID.Text) + "_" + Func.Convert.sConvertToString(txtCouponClaimNo.Text) + "_" + sSourceFileName;
                        sSourceFileName1 = Func.Convert.sConvertToString(txtDealerAID.Text) + "_" + Func.Convert.sConvertToString(txtCouponClaimNo.Text) + "_" + sSourceFileName;
                        sSourceFileName1 = sSourceFileName1.Replace("/", "");

                        dr["File_Names"] = sSourceFileName1;
                        

                        dr["UserId"] = Func.Convert.sConvertToString(txtDealerAID.Text);
                        dr["Status"] = "S";

                        dtFileAttach.Rows.Add(dr);
                        dtFileAttach.AcceptChanges();


                        //Saving it in temperory Directory.
                        if (!System.IO.Directory.Exists(sPath + "Coupon Claim"))
                            System.IO.Directory.CreateDirectory(sPath + "Coupon Claim");

                        //uploads[i].SaveAs((sPath + "Coupon Claim" + "\\" + Func.Convert.sConvertToString(txtDealerAID.Text) + "_" + Func.Convert.sConvertToString(txtCouponClaimNo.Text) + "_" + sSourceFileName));
                        uploads[i].SaveAs(sPath + "Coupon Claim" + "\\" + sSourceFileName1);

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
                dtFileAttach.Rows.Add(dr);
                dtFileAttach.AcceptChanges();
            }
        }
        private void bFillHeaderFromCouponClaim()
        {
            bool bSaveRecord = false;
            DataRow dr;
            dtHeaderdata = new DataTable();
            //Get Header InFormation        
            dtHeaderdata.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHeaderdata.Columns.Add(new DataColumn("Coupon_Status", typeof(string)));
            dtHeaderdata.Columns.Add(new DataColumn("Reason", typeof(string)));
            CheckBox ChkForCouponStatus;
            for (int iGridRowCnt = 0; iGridRowCnt < CouponClaimGrid.Rows.Count; iGridRowCnt++)
            {
                dr = dtHeaderdata.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt((CouponClaimGrid.Rows[iGridRowCnt].FindControl("lblID") as Label).Text);
                ChkForCouponStatus = (CheckBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("ChkPart"));
                if (ChkForCouponStatus.Checked == true)
                {
                    dr["Coupon_Status"] = "Y";
                }
                else
                {
                    dr["Coupon_Status"] = "N";
                }
                TextBox txtRejRemark;
                txtRejRemark = (TextBox)(CouponClaimGrid.Rows[iGridRowCnt].FindControl("txtRejRemark"));
                dr["Reason"] = txtRejRemark.Text.ToString();

                dtHeaderdata.Rows.Add(dr);
                dtHeaderdata.AcceptChanges();
            }
        }

        private bool bSaveClaimFileAttachDetails(DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iCouponClaimDetID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iCouponClaimDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iCouponClaimDetID == 0)
                        {
                            iCouponClaimDetID = objDB.ExecuteStoredProcedure("SP_CouponClaim_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_CouponClaim_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_CouponClaim_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
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
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            bool bSubmit = false;

            bSubmit = bSaveRecord(1, "Y");
            // if (bSaveRecord(1, "Y") == false) return;
            // bSubmit = true;
            if (bSubmit == true)
            {
                lblMessage.Text = " Record is Confirmed.!";
            }
            else
            {
                lblMessage.Text = " Record is not Confirmed.";
            }
            GetDataAndDisplay();
            lblMessage.Visible = true;
        }
    }
}