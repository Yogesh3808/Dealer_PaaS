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
using System.IO;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Admin
{
    public partial class frmHdrContentMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (fupFileName.HasFile)
            //    fupFileName.Attributes.Add("onblur", "return CheckContentMgmt('" + fupFileName.PostedFile.ContentLength + "')");

            if (!IsPostBack)
            {

                FillCombo();
                fillGrid();
                DisplayCurrentRecord();

            }
        }
        private void fillGrid()
        {
            DataSet ds = new DataSet();
            clsContent objContent = new clsContent();
            ds = objContent.GetContent(0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFSDetails.DataSource = ds;
                gvFSDetails.DataBind();
            }
        }
        private void FillCombo()
        {

            Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");

        }
        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select the records.";
            bool bValidateRecord = true;

            //if ((txtID.Text == "0" || txtID.Text == "") && txtFileName.Text == "")
            //if (fupFileName.FileName == "")
            //{
            //    sMessage = sMessage + "\\n Please Select File.";
            //    bValidateRecord = false;
            //}

            ////if (fupFileName.HasFile)
            ////{
            ////    if (Path.GetExtension(fupFileName.PostedFile.FileName).ToLower() != ".pdf")
            ////    {
            ////        sMessage = sMessage + "\\n Please select pdf file.";
            ////        bValidateRecord = false;
            ////    }
            ////}

            if (fupFileName.HasFile)
            {
                if ((fupFileName.FileName).Length > 100)
                {
                    sMessage = sMessage + "\\n File name size limit upto 100 chracter.";
                    bValidateRecord = false;
                }
            }

            if (txtDocumentName.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Document Name.";
                bValidateRecord = false;
            }

            if (txtDocumentHeading.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Document Header.";
                bValidateRecord = false;
            }
            if (drpDept.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select Department.";
                bValidateRecord = false;
            }
            if (txtEffFromDate.Text == "")
            {
                sMessage = sMessage + "\\n Please Select Effective From Date.";
                bValidateRecord = false;
            }
            if (txtEffToFromDate.Text == "")
            {
                sMessage = sMessage + "\\n Please Select Effective To Date.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            return bValidateRecord;
        }
        private Boolean SaveFile()
        {
            string CurrentPath = Server.MapPath("~/PDF/");
            if (fupFileName.PostedFile.FileName != "")
            {
                //CurrentPath = CurrentPath + fupFileName.FileName;
                if (File.Exists(CurrentPath + fupFileName.FileName))
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('File already exist,please choose different file name.');</script>");
                    return false;
                }
                else
                {
                    if ((txtID.Text != "0" || txtID.Text != "") && txtFileName.Text != "")
                    {
                        File.Delete(CurrentPath + txtFileName.Text);
                    }
                    fupFileName.SaveAs(CurrentPath + fupFileName.FileName);
                }
                return true;
            }
            return true;
        }
        //protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton ObjImageButton = (ImageButton)sender;

        //        if (ObjImageButton.ID == "ToolbarButton1")//for New
        //        {               
        //            NewRecord();
        //            ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNew);
        //            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
        //            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
        //            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
        //            return;
        //        }
        //        else if (ObjImageButton.ID == "ToolbarButton2")//for Save
        //        {
        //            //clsContent objContent = new clsContent();
        //            clsContent objContent = new clsContent();
        //            if (bValidateRecord() == false)
        //            {
        //                goto Last;
        //            }

        //            if (objContent.bSaveContentMaster(ContentSave("N")) == true)
        //            {
        //                //SaveFile();//fupFileName
        //                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");                    
        //            }
        //            else
        //            {
        //                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
        //            }                
        //            objContent = null;
        //        }
        //        else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
        //        {
        //            clsContent objContent = new clsContent();
        //            if (bValidateRecord() == false)
        //            {
        //                goto Last;
        //            }
        //            if (objContent.bSaveContentMaster(ContentSave("Y")) == true)
        //            {
        //                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
        //            }
        //            else
        //            {
        //                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
        //            }
        //            objContent = null;
        //        }

        //        //iContentID = Func.Convert.iConvertToInt(txtID.Text);
        //        GetDataAndDisplay();
        //        fillGrid();
        //    Last: ;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        private DataTable ContentSave(string ConfirmStatus)
        {
            DataTable dtHdr = new DataTable();
            DataRow dr;
            //Get Header InFormation        
            dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("File_Name", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("Document_Name", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Document_Heading", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("Effective_Date_From", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Effective_Date_To", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Dept_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Active_Status", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));


            dr = dtHdr.NewRow();
            dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
            dr["File_Name"] = (fupFileName.HasFile) ? fupFileName.FileName : txtFileName.Text;//txtFileName.Text;
            dr["Document_Name"] = txtDocumentName.Text;
            dr["Document_Heading"] = txtDocumentHeading.Text;
            dr["Effective_Date_From"] = txtEffFromDate.Text;
            dr["Effective_Date_To"] = txtEffToFromDate.Text;
            dr["Dept_ID"] = Func.Convert.iConvertToInt(drpDept.SelectedValue);
            dr["Active_Status"] = "Y"; //(ConfirmStatus == "Y") ? "Y" : txtActiveStatus.Text;
            dr["Is_Confirm"] = ConfirmStatus;
            dtHdr.Rows.Add(dr);
            dtHdr.AcceptChanges();
            return dtHdr;

        }
        private void DisplayCurrentRecord()
        {
            DataTable dtDetails = null;
            DataSet ds = new DataSet();
            clsContent objContent = new clsContent();
            ds = objContent.GetContent(Func.Convert.iConvertToInt(txtID.Text));
            if (ds != null)
            {
                dtDetails = ds.Tables[0];
                if (dtDetails.Rows.Count > 0)
                {
                    DisplayData(dtDetails);
                }
            }

        }
        private void NewRecord()
        {
            txtID.Text = "0";
            txtFileName.Text = "";
            txtEffFromDate.Text = "";
            txtEffToFromDate.Text = "";
            txtDocumentName.Text = "";
            txtDocumentHeading.Text = "";
            // txtObjective.Text = "";
            //drpDealerType.SelectedValue = "D";
            drpDept.SelectedValue = "0";
            EnableDisable("N");
            txtFileName.Focus();

        }
        private void EnableDisable(string sConfirmStatus)
        {

            if (sConfirmStatus == "Y")
            {
                txtFileName.Enabled = false;
                txtEffFromDate.Enabled = false;
                txtEffToFromDate.Enabled = false;
                txtDocumentHeading.Enabled = false;
                txtDocumentName.Enabled = false;
                txtActiveStatus.Enabled = false;
                drpDept.Enabled = false;
                bSave.Enabled = false;
                btnConfirm.Enabled = false;

                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);

            }
            else if (sConfirmStatus == "N")
            {

                txtFileName.Enabled = true;
                txtEffFromDate.Enabled = true;
                txtEffToFromDate.Enabled = true;
                txtDocumentHeading.Enabled = true;
                txtDocumentName.Enabled = true;
                txtActiveStatus.Enabled = true;
                drpDept.Enabled = true;
                bSave.Enabled = true;
                btnConfirm.Enabled = false;

            }
            else if (sConfirmStatus == "P")
            {

                txtFileName.Enabled = true;
                txtEffFromDate.Enabled = true;
                txtEffToFromDate.Enabled = true;
                txtDocumentHeading.Enabled = true;
                txtDocumentName.Enabled = true;
                txtActiveStatus.Enabled = true;
                drpDept.Enabled = true;
                bSave.Enabled = true;
                btnConfirm.Enabled = true;
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmSave);

            }

        }
        private void GetDataAndDisplay()
        {
            DataTable dtDetails = null;


            DataSet ds = new DataSet();
            clsContent objContent = new clsContent();
            ds = objContent.GetContent(Func.Convert.iConvertToInt(txtID.Text));//Func.Convert.iConvertToInt(txtID.Text)
            dtDetails = ds.Tables[0];
            DisplayData(dtDetails);
            objContent = null;

            dtDetails = null;

        }
        private void DisplayData(DataTable dtContent)
        {
            string sConfirmStatus = "";
            if (dtContent.Rows.Count == 0)
            {
                return;
            }

            //Display Header   
            txtID.Text = Func.Convert.sConvertToString(dtContent.Rows[0]["ID"]);
            txtFileName.Text = Func.Convert.sConvertToString(dtContent.Rows[0]["File_Name"]);
            //fupFileName.PostedFile.FileName  = "~PDF\"" + Func.Convert.sConvertToString(dtContent.Rows[0]["File_Name"]);
            txtDocumentName.Text = Func.Convert.sConvertToString(dtContent.Rows[0]["Document_Name"]);
            txtDocumentHeading.Text = Func.Convert.sConvertToString(dtContent.Rows[0]["Document_Heading"]);

            //txtActiveStatus.Text = Func.Convert.sConvertToString(dtContent.Rows[0]["Active_Status"]);

            drpDept.SelectedValue = Func.Convert.sConvertToString(dtContent.Rows[0]["Dept_ID"]);

            txtEffFromDate.Text = Func.Convert.tConvertToDate(dtContent.Rows[0]["Effective_Date_From"], false);
            txtEffToFromDate.Text = Func.Convert.tConvertToDate(dtContent.Rows[0]["Effective_Date_To"], false);
            sConfirmStatus = Func.Convert.sConvertToString(dtContent.Rows[0]["Is_Confirm"]);


            EnableDisable(sConfirmStatus);

        }
        protected void gvFSDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).ID == "ImgSelect")
            if (e.CommandName == "ImgSelect")
            {

                DataTable dtDetails = null;
                DataSet ds = new DataSet();
                clsContent objContent = new clsContent();
                txtID.Text = e.CommandArgument.ToString();
                ds = objContent.GetContent(Func.Convert.iConvertToInt(txtID.Text));
                if (ds != null)
                {
                    dtDetails = ds.Tables[0];
                    if (dtDetails.Rows.Count > 0)
                    {
                        DisplayData(dtDetails);
                    }
                }

            }
            if (e.CommandName == "Delete")
            {

                string[] splstring = e.CommandArgument.ToString().Split('/');
                string CurrentPath = Server.MapPath("~/PDF/");
                txtID.Text = splstring[0];
                clsContent objContent = new clsContent();
                if (objContent.bDeleteContentMaster(Func.Convert.iConvertToInt(txtID.Text)) == true)
                {
                    if (splstring.Length > 1)
                        if (File.Exists(CurrentPath + splstring[1]))
                            File.Delete(CurrentPath + splstring[1]);
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Document Deleted Successfully.');</script>");
                }
                GetDataAndDisplay();
                fillGrid();
            }
        }

        protected void gvFSDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                //clsContent objContent = new clsContent();
                clsContent objContent = new clsContent();
                if (bValidateRecord() == false)
                {
                    goto Last;
                }
                if (SaveFile() == false)
                {
                    goto Last;
                }
                if (objContent.bSaveContentMaster(ContentSave("P")) == true)
                {

                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }
                GetDataAndDisplay();
                fillGrid();
            Last: ;
                objContent = null;
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //clsContent objContent = new clsContent();
                clsContent objContent = new clsContent();
                if (bValidateRecord() == false)
                {
                    goto Last;
                }
                if (SaveFile() == false)
                {
                    goto Last;
                }
                if (objContent.bSaveContentMaster(ContentSave("Y")) == true)
                {

                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }
                GetDataAndDisplay();
                fillGrid();
            Last: ;
                objContent = null;
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            NewRecord();
        }

        protected void gvFSDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFSDetails.PageIndex = e.NewPageIndex;
            fillGrid();
        }
    }
}