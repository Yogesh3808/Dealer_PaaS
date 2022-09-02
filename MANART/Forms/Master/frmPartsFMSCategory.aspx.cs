using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Data;
using MANART.WebParts;
using System.Data.OleDb;

namespace MANART.Forms.Master
{
    public partial class frmPartsFMSCategory : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        int iMenuId = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        int iUserId = 0;
        private int iID = 0;
        string sDealerIds = "";
        DataSet ds = new DataSet();
        private string sUserRole = "";
        string sLoginName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.bUseImgOrButton = true;
                Location.bUseSpareDealerCode = true;

                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                sUserRole = Func.Convert.sConvertToString(Session["UserRole"]);
                sLoginName = Func.Convert.sConvertToString(Session["LoginName"]);

                if (iMenuId == 716 && sUserRole == "3")// ASM User
                {
                    Location1.Visible = false;
                    Location.Visible = true;
                    Table2.Visible = false;
                    divBtnShow.Style.Add("display", "");
                    tblPartsFMSCatUpload.Visible = false;
                }
                //else if (iMenuId == 716 && sUserRole == "1")// Head User
                else if (iMenuId == 716 &&  (sLoginName == "DCAN703161" || sLoginName == "DCAN703114"))//Deepak and Alok
                {
                    Location1.Visible = false;
                    Table2.Visible = false;
                    divBtnShow.Style.Add("display", "");
                    divBtnShow.Visible = true;
                    Location.Visible = true;// Multiselect Location
                }
                else
                {// Dealer User
                    Location.Visible = false;// Multiselect Location
                    Location1.Visible = true;//Dealer Details
                    divBtnShow.Style.Add("display", "none");
                    Table2.Visible = false;
                    tblPartsFMSCatUpload.Visible = false;
                }
                ToolbarC.Visible = false;

                if (iID != 0)
                {
                    DisplayCurrentRecord();
                }

                sDealerIds = Location.sDealerId;

                // Disable Confirm,Cancel,Print Button
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
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
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
            if (!IsPostBack)
            {
                DisplayCurrentRecord();
            }
        }


        private void DisplayCurrentRecord()
        {
            try
            {
                DataSet ds = new DataSet();
                if (iMenuId == 717)//Dealer Menu
                    sDealerIds = Func.Convert.sConvertToString(Session["iDealerID"]);
                else
                    sDealerIds = Location.sDealerId;

                if (sDealerIds == "")
                    sDealerIds = "0";

                clsFMSCategory objFMSCat = new clsFMSCategory();

                ds = objFMSCat.GetDealerFMSCatWise(iID, sDealerIds, "All");
                if (ds != null)
                {
                    GrdPartsFMSCat.DataSource = ds.Tables[0];
                    GrdPartsFMSCat.DataBind();
                    dvbudplan.Style.Add("display", "");
                    //DisplayData(ds);
                    objFMSCat = null;
                    txtID.Text = "";
                }
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
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtCat_AF.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_AF"]);
                    txtCat_BF.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_BF"]);
                    txtCat_CF.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_CF"]);

                    txtCat_AM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_AM"]);
                    txtCat_BM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_BM"]);
                    txtCat_CM.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_CM"]);

                    txtCat_AS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_AS"]);
                    txtCat_BS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_BS"]);
                    txtCat_CS.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cat_CS"]);
                    if (iMenuId == 706)
                        bEnableControls = false;
                    else
                        bEnableControls = true;

                    if (bEnableControls == true)
                        MakeEnableDisableControls(true);
                    else
                        MakeEnableDisableControls(false);
                }
                else
                {
                    ClearStockingNorms();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            txtCat_AF.Enabled = bEnable;
            txtCat_BF.Enabled = bEnable;
            txtCat_CF.Enabled = bEnable;
            txtCat_AM.Enabled = bEnable;
            txtCat_BM.Enabled = bEnable;
            txtCat_CM.Enabled = bEnable;
            txtCat_AS.Enabled = bEnable;
            txtCat_BS.Enabled = bEnable;
            txtCat_CS.Enabled = bEnable;
        }
        private void ClearStockingNorms()
        {
            txtCat_AF.Text = "";
            txtCat_BF.Text = "";
            txtCat_CF.Text = "";

            txtCat_AM.Text = "";
            txtCat_BM.Text = "";
            txtCat_CM.Text = "";

            txtCat_AS.Text = "";
            txtCat_BS.Text = "";
            txtCat_CS.Text = "";

        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
        private bool bValidateRecord()
        {
            string sMessage = " ";
            bool bValidateRecord = true;
            if (txtCat_AF.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory AF.";
                bValidateRecord = false;
            }
            if (txtCat_BF.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory BF.";
                bValidateRecord = false;
            }
            if (txtCat_CF.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory CF.";
                bValidateRecord = false;
            }
            if (txtCat_AM.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory AM.";
                bValidateRecord = false;
            }
            if (txtCat_BM.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory BM.";
                bValidateRecord = false;
            }
            if (txtCat_CM.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory CM.";
                bValidateRecord = false;
            }
            if (txtCat_AS.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory AS.";
                bValidateRecord = false;
            }
            if (txtCat_BS.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory BS.";
                bValidateRecord = false;
            }
            if (txtCat_CS.Text.Trim() == "")
            {
                sMessage = sMessage + "\\n Please Enter Catagory CS.";
                bValidateRecord = false;
            }

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                clsStockingNorms objStkNorms = new clsStockingNorms();
                ImageButton ObjImageButton = (ImageButton)sender;

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    txtID.Text = "0";
                    ClearStockingNorms();
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord() == false) return;
                    UpdateHdrValueFromControl(dtDetails);

                    if (objStkNorms.bSaveStockNormsCatWise(ref iID, dtDetails) == true)
                    {
                        txtID.Text = Func.Convert.sConvertToString(iID);
                        if (iID > 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Stocking Norms For Dealer Code ") + "','" + Server.HtmlEncode(Location1.sDealerCode) + "');</script>");
                        }
                        else
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Stocking Norms For Dealer Code ") + "','" + Server.HtmlEncode(Location1.sDealerCode) + "');</script>");
                        }
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Stocking Norms For Dealer Code  ") + "','" + Server.HtmlEncode(Location1.sDealerCode) + "');</script>");
                        return;
                    }

                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    //if (txtID.Text == "")
                    //{
                    DisplayCurrentRecord();
                    //}

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void UpdateHdrValueFromControl(DataTable dtDetails)
        {
            DataRow dr;
            //Get Header InFormation        
            dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Dealer_Id", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("User_ID", typeof(int)));

            dtDetails.Columns.Add(new DataColumn("Cat_AF", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_BF", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_CF", typeof(int)));

            dtDetails.Columns.Add(new DataColumn("Cat_AM", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_BM", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_CM", typeof(int)));

            dtDetails.Columns.Add(new DataColumn("Cat_AS", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_BS", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("Cat_CS", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("HoBrID", typeof(int)));

            dr = dtDetails.NewRow();
            dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
            dr["Dealer_Id"] = iDealerID;
            dr["User_ID"] = iUserId;

            dr["Cat_AF"] = Func.Convert.iConvertToInt(txtCat_AF.Text.Trim());
            dr["Cat_BF"] = Func.Convert.iConvertToInt(txtCat_BF.Text.Trim());
            dr["Cat_CF"] = Func.Convert.iConvertToInt(txtCat_CF.Text.Trim());

            dr["Cat_AM"] = Func.Convert.iConvertToInt(txtCat_AM.Text.Trim());
            dr["Cat_BM"] = Func.Convert.iConvertToInt(txtCat_BM.Text.Trim());
            dr["Cat_CM"] = Func.Convert.iConvertToInt(txtCat_CM.Text.Trim());

            dr["Cat_AS"] = Func.Convert.iConvertToInt(txtCat_AS.Text.Trim());
            dr["Cat_BS"] = Func.Convert.iConvertToInt(txtCat_BS.Text.Trim());
            dr["Cat_CS"] = Func.Convert.iConvertToInt(txtCat_CS.Text.Trim());
            dr["HoBrID"] = iHOBr_id;

            dtDetails.Rows.Add(dr);
            dtDetails.AcceptChanges();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            DisplayCurrentRecord();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DataTable dtHdr = null;
            try
            {
                if ((txtFilePath.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    DataSet dsNotUploadchassisDetails = new DataSet();
                    DataTable dtLayout = new DataTable();
                    string query = null;
                    string connString = "";
                    string strFileName = System.IO.Path.GetFileNameWithoutExtension(txtFilePath.FileName);
                    //+'_'+ DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    string strFileExtType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
                    string strFileType = ".xlsx";

                    //Check file type
                    if (strFileExtType == ".xls" || strFileExtType == ".xlsx")
                        txtFilePath.SaveAs(Server.MapPath("~/DownLoadFiles/FMSCategory/" + strFileName + strFileType));
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return;
                    }
                    string strNewPath = Server.MapPath("~/DownLoadFiles/FMSCategory/" + strFileName + strFileType);

                    //Connection String to Excel Workbook
                    if (strFileType.Trim() == ".xls")
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    else if (strFileType.Trim() == ".xlsx")
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    query = "SELECT * FROM [Sheet1$]";

                    //Create the connection object
                    conn = new OleDbConnection(connString);
                    //Open connection
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //Create the command object
                    cmd = new OleDbCommand(query, conn);
                    da = new OleDbDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dtHdr = new DataTable();

                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                            dtHdr = ds.Tables[0];
                        else
                        {
                            string sMessage = "Empty File Selected .";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            goto Last;
                        }

                        clsFMSCategory objFMSCat = new clsFMSCategory();
                        int iCountNotUpload = 0;
                        // For checking all uploaded or not
                        DataSet dsUploadPartsTarget;
                        dsUploadPartsTarget = objFMSCat.UploadPartsFMSCategoryDetails(txtFilePath.FileName, dtHdr, iUserId);
                        if (dsUploadPartsTarget == null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);
                            return;
                        }
                        else
                        {
                            SetListOfNotUploadedDealerCode(dsUploadPartsTarget.Tables[0]);
                            dtHdr = dsUploadPartsTarget.Tables[1];
                        }
                        if (iCountNotUpload != 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + "Stocking Norms not uploaded.');</script>");
                        }

                        if (objFMSCat.bPartsFMSCategory(dtHdr, iUserId) == true)
                        {

                            int TotalRecords = 0;
                            int TotalSuccess = 0;
                            int TotalError = 0;
                            ViewState["FMSCat"] = null;
                            ViewState["FMSCat"] = ds.Tables[0];

                            if (dtHdr != null && dtHdr.Rows.Count > 0)
                                TotalSuccess = dtHdr.Rows.Count;

                            //TotalRecords = TotalSuccess + TotalError;

                            string sMessage = "File Uploaded successfully! Total Records:" + TotalSuccess;
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            //lblMessage.Text = "Data retrieved successfully! Total Records:" + ds.Tables[0].Rows.Count;
                            //lblMessage.ForeColor = System.Drawing.Color.Green;
                            //lblMessage.Visible = true;

                            da.Dispose();
                            conn.Close();
                            conn.Dispose();

                        }
                        else
                        {
                            string sMessage = "Error : Data Uploading Failed!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                        }

                    }
                    catch (Exception ex)
                    {
                        string sMessage = "Error : Data Uploading Failed!";
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    }
                }
                else
                {
                    string sMessage = "Please select an excel file first";
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                }
            Last: ;
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void SetListOfNotUploadedDealerCode(DataTable dtDealerCodeList)
        {
            try
            {
                string sDealerCodeList = "";
                if (Func.Common.iRowCntOfTable(dtDealerCodeList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtDealerCodeList.Rows.Count; iRowCnt++)
                    {
                        sDealerCodeList = sDealerCodeList + Func.Convert.sConvertToString(dtDealerCodeList.Rows[iRowCnt]["Dealer_Code"]) + "_" + Func.Convert.sConvertToString(dtDealerCodeList.Rows[iRowCnt]["Part_No"]) + "_" + Func.Convert.sConvertToString(dtDealerCodeList.Rows[iRowCnt]["Part_Cat"]) + ",";
                    }
                    lblListPartNo.Text = "Dealer Code which are Parts Stocking Category not uploaded";
                    txtListPartNo.Text = sDealerCodeList;
                    txtListPartNo.Visible = true;
                    lblListPartNo.Visible = true;
                }
                else
                {
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

    }
}