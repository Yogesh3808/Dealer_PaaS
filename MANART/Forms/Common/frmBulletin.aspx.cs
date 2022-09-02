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
using AjaxControlToolkit;
using MANART_BAL;
using MANART_DAL;
using System.IO;
using System.Drawing;
using System.Data.OleDb;

namespace MANART.Forms.Common
{
    public partial class frmBulletin : System.Web.UI.Page
    {
        int iUserId = 0;
        int iUserType = 0;
        int iMenuId = 0;
        int iDocTypeID = 0;
        int iDealerID = 0;
        string strNewPath = "";
        clsContent objContent = null;
        private DataTable dtFileAttach = new DataTable();
        private string sUserRole = "";
        string sLoginName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
            iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            sUserRole = Func.Convert.sConvertToString(Session["UserRole"]);
            sLoginName = Func.Convert.sConvertToString(Session["LoginName"].ToString().Trim());

            if (!IsPostBack)
            {
                fillGrid();
                FillCombo();
            }
            if (sLoginName == "DCAN703311" && iMenuId == 492 && iUserType == 2) // || sLoginName == "DCANMIC070"
            {
                UploafFile.Style.Add("display", "");
                VisibleTrueFalse(true);
            }
            else if (iMenuId == 492 && sUserRole == "1")// MTI
            {
                UploafFile.Style.Add("display", "");
                VisibleTrueFalse(true);
            }
            else
            {
                VisibleTrueFalse(false);
            }

        }
        private void VisibleTrueFalse(bool bVisible)
        {
            lblDocName.Visible = bVisible;
            txtDocName.Visible = bVisible;
            lblMDocName.Visible = bVisible;
            lblDocHeading.Visible = bVisible;
            txtDocHeading.Visible = bVisible;
            lblMDocHeading.Visible = bVisible;
        }

        private void fillGrid()
        {
            DataSet ds = new DataSet();
            clsContent objContent = new clsContent();
            int UserTypeID = Func.Convert.iConvertToInt(Session["UserType"]);
            iDocTypeID = Func.Convert.iConvertToInt(drpDocType.SelectedValue);
            if (UserTypeID == 1)
            {
                //ds = objContent.GetDocumentBulletin(0, UserTypeID, "E", "All");
            }
            else if (UserTypeID == 2)
            {
                // ds = objContent.GetDocumentBulletin(0, UserTypeID, "D", "All");
                ds = objContent.GetDocumentBulletin_New(0, UserTypeID, iDocTypeID, "All");
            }
            else if (UserTypeID == 3)
            {
                //ds = objContent.GetDocumentBulletin(Func.Convert.iConvertToInt(Session["iDealerID"]), UserTypeID, "D", "All");
                ds = objContent.GetDocumentBulletin_New(iDealerID, UserTypeID, iDocTypeID, "All");
            }
            else if (UserTypeID == 4)
            {
                // ds = objContent.GetDocumentBulletin(Func.Convert.iConvertToInt(Session["iDealerID"]), UserTypeID, "E", "All");
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBulletin.DataSource = ds;
                gvBulletin.DataBind();
            }
        }

        protected void gvBulletin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBulletin.PageIndex = e.NewPageIndex;
            fillGrid();
        }
        protected void gvBulletin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HtmlAnchor achFileName = (HtmlAnchor)e.Row.Cells[0].FindControl("achFileName");
            Label lblDoc_Type = (Label)e.Row.Cells[0].FindControl("lblDoc_Type");
            if (achFileName != null)
            {
                if (Func.Convert.iConvertToInt(lblDoc_Type.Text) == 1)

                    achFileName.HRef = "~/DownLoadFiles/Bulletin/" + achFileName.InnerHtml;
                else
                    achFileName.HRef = "~/DownLoadFiles/PartsCatalogue/" + achFileName.InnerHtml;
                achFileName.Target = "_blank";
            }

        }
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpDocType, clsCommon.ComboQueryType.DocType, 0, "");
            drpDocType.SelectedValue = "0";
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            fillGrid();
        }
        private bool IsPdforZip(HttpPostedFile file)
        {
            return ((file != null) && (System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "application/pdf") || System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "application/zip") || System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "application/x-zip-compressed")) && (file.ContentLength > 0));
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                HttpPostedFile file = txtFilePath.PostedFile;
                if ((file != null) && (file.ContentLength > 0))
                {
                    //if (IsPdforZip(file) == false)
                    //{
                    //    // Invalid file type 
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please make sure your file is in pdf or zip format')", true);
                    //    return;
                    //}
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select File')", true);
                    return;
                }

                int iFileSize = file.ContentLength;
                double filelimit = Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetFileSizeLimit, 0, ""));
                if (iFileSize > Func.Convert.iConvertToInt(filelimit))
                {
                    // File exceeds the file maximum size
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please make sure your file size is less than 3 MB')", true);
                    return;
                }

                // Validation
                string sMessages = "";
                bool bValidateRecord = true;
                if (drpDocType.SelectedValue == "0")
                {
                    sMessages = sMessages + "\\n *Please select the Document Type.";
                    bValidateRecord = false;
                }
                if (txtDocName.Text == "")
                {
                    sMessages = sMessages + "\\n *Enter the document Name.";
                    bValidateRecord = false;
                }
                if (txtDocHeading.Text == "")
                {
                    sMessages = sMessages + "\\n *Enter the document date.";
                    bValidateRecord = false;
                }
                if (bValidateRecord == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessages + ".');</script>");
                    return;
                }

                //
                if ((txtFilePath.HasFile) && bValidateRecord == true)
                {
                    clsContent objContent = new clsContent();

                    string strFileName = Path.GetFileNameWithoutExtension(txtFilePath.PostedFile.FileName);
                    string strFileExtType = Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
                    string strFileType = strFileExtType;
                    //string sMsg = "";
                    string sFullPath = "";

                    // Check File Type
                    string[] acceptedFileTypes = new string[2];
                    acceptedFileTypes[0] = ".pdf";
                    acceptedFileTypes[1] = ".zip";

                    bool acceptFile = false;
                    //should we accept the file?
                    for (int i = 0; i <= 1; i++)
                    {
                        if (strFileExtType == acceptedFileTypes[i])
                        {  //accept the file, yay!
                            acceptFile = true;
                        }
                    }


                    if (iDocTypeID == 1 || Func.Convert.iConvertToInt(drpDocType.SelectedValue) == 1 && acceptFile == true)
                    {
                        //Does the file already exist?
                        if (File.Exists(Server.MapPath("~/DownLoadFiles/Bulletin/" + strFileName + strFileType)))
                        {
                            //sMsg = "A file with the name <b>" + txtFilePath.FileName + "</b> already exists on the server.";
                            //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMsg + ".');</script>");
                            //return;
                            sFullPath = Server.MapPath("~/DownLoadFiles/Bulletin/" + strFileName + strFileType);
                            System.IO.File.Delete(@sFullPath);
                        }
                    }
                    else
                    {
                        if (File.Exists(Server.MapPath("~/DownLoadFiles/PartsCatalogue/" + strFileName + strFileType)) && acceptFile == true)
                        {
                            //sMsg = "A file with the name <b>" + txtFilePath.FileName + "</b> already exists on the server.";
                            //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMsg + ".');</script>");
                            //return;
                            sFullPath = Server.MapPath("~/DownLoadFiles/PartsCatalogue/" + strFileName + strFileType);
                            System.IO.File.Delete(@sFullPath);
                        }
                    }


                    if (!acceptFile)
                    {
                        lblMessage.Text = "The file you are trying to upload is not a permitted file type!";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return;
                    }
                    else
                    {//
                        if (iDocTypeID == 1 || Func.Convert.iConvertToInt(drpDocType.SelectedValue) == 1)
                            strNewPath = ("~/DownLoadFiles/Bulletin/" + strFileName + strFileType);
                        else
                            strNewPath = ("~/DownLoadFiles/PartsCatalogue/" + strFileName + strFileType);

                        txtFilePath.SaveAs(Server.MapPath(strNewPath));

                        UpdateHdrValueFromControl(dtFileAttach);
                        if (objContent.bSaveRecordWithFile(dtFileAttach) == true)
                        {
                            string sMessage = "File Uploaded successfully!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        }
                        else
                        {
                            string sMessage = "Error : Data Uploading Failed!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            return;
                        }
                        fillGrid();
                        ClearControl();
                    }//END ELSE
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ClearControl()
        {
            txtDocName.Text = "";
            txtDocHeading.Text = "";
        }
        #region UpdateHeader
        private void UpdateHdrValueFromControl(DataTable dtFileAttach)
        {
            DataRow dr;
            //Get Header InFormation        
            dtFileAttach.Columns.Add(new DataColumn("ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("File_Name", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("Doc_Name", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("Doc_Heading", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("Doc_Type", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Region_ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("State_ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Country_ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("User_ID", typeof(int)));
            dtFileAttach.Columns.Add(new DataColumn("Active_Status", typeof(string)));
            dtFileAttach.Columns.Add(new DataColumn("Path", typeof(string)));

            dr = dtFileAttach.NewRow();
            dr["ID"] = 0;
            dr["File_Name"] = Path.GetFileName(txtFilePath.PostedFile.FileName);
            dr["Doc_Name"] = txtDocName.Text.Trim();
            dr["Doc_Heading"] = txtDocHeading.Text.Trim();
            dr["Doc_Type"] = Func.Convert.iConvertToInt(drpDocType.SelectedValue);
            dr["Region_ID"] = 0;
            dr["State_ID"] = 0;
            dr["Country_ID"] = 0;
            dr["User_ID"] = iUserId;
            dr["Active_Status"] = "Y";
            dr["Path"] = strNewPath;

            dtFileAttach.Rows.Add(dr);
            dtFileAttach.AcceptChanges();

        }
        #endregion

        protected void gvBulletin_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "EditButton")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvBulletin.Rows[index];
                Label lblDocument_Path = (Label)row.FindControl("lblDocument_Path");
                string str = Path.GetExtension(lblDocument_Path.Text);
                if (str == ".pdf")
                    Response.Redirect("~/Forms/Spares/frmPartsCatalogue.aspx?FileName=" + lblDocument_Path.Text);
            }
        }
    }
}