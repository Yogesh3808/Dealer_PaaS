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
using System.Net;

namespace MANART.Forms.Spares
{
    public partial class frmPartsCatalogue : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private string sDealerID = "";
        private int iYearID = 0;
        int iMenuId = 0;
        string cType;
        int iUserId = 0;
        int iUserType = 0;
        string sDealerIds = "";
        string DealerOrigin = "";
        string sLoginName = "";
        private string sDealerCode = "";
        private string sDcodeOrRcode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string url = Func.Convert.sConvertToString(Request.QueryString["FileName"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                sLoginName = Func.Convert.sConvertToString(Session["LoginName"].ToString().Trim());
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);

                // Get Cleint IP Address
                //lblCleintIPAddress.Text = GetIPAddress();
                //if (sLoginName == "DCAN702994" && ( lblCleintIPAddress.Text == "192.168.1.80" || Label1.Text == "123.201.34.111"))//Sam Varughese
                if (sLoginName == "DCAN702688" && iMenuId == 138 && iUserType == 2) // || sLoginName == "DCANMIC070"
                {
                    tdSelectFile.Style.Add("display", "");
                    tdUploadFile.Style.Add("display", "");
                    TblControl.Visible = true;
                }
                else
                {
                    tdSelectFile.Style.Add("display", "none");
                    tdUploadFile.Style.Add("display", "none");
                    TblControl.Visible = false;
                }

                Location.Visible = false;
                Location1.Visible = false;
                ToolbarC.Visible = false;

                sDealerIds = Location.sDealerId;

                // IP Related
                //    string myExternalIP;
                //    string strHostName = System.Net.Dns.GetHostName();
                //    string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                //    string clientip = clientIPAddress.ToString();
                //    System.Net.HttpWebRequest request =

                //(System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.whatismyip.org");
                //    request.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE" +
                //        "6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //    System.Net.HttpWebResponse response =
                //    (System.Net.HttpWebResponse)request.GetResponse();
                //    using (System.IO.StreamReader reader = new
                //    StreamReader(response.GetResponseStream()))
                //    {
                //        myExternalIP = reader.ReadToEnd();
                //        reader.Close();
                //    }
                //    Label1.Text = myExternalIP.ToString();
                // END
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.bUseImgOrButton = true;

                if (!IsPostBack)
                {
                    Session["ModelDetails"] = null;

                }

                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                // DisplayCurrentRecord();       
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                string strDisAbleBackButton;
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                if (!IsPostBack)
                {
                    DisplayCurrentRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayCurrentRecord()
        {
            try
            {
                DataTable dtDetails = null;
                DataSet ds = new DataSet();
                if (iMenuId == 697)
                    sDealerIds = Session["iDealerID"].ToString();
                else
                    sDealerIds = Location.sDealerId;

                if (sDealerIds == "")
                    sDealerIds = "0";
                if (sDealerCode.Trim().StartsWith("R"))
                    sDcodeOrRcode = "R";
                else
                    sDcodeOrRcode = "D";


                clsSparesTarget objSparesTarget = new clsSparesTarget();
                ds = objSparesTarget.GetModelCatelogue(sDcodeOrRcode);

                if (ds != null)
                {
                    if (Func.Common.iTableCntOfDatatSet(ds) > 0)
                    {
                        dtDetails = ds.Tables[0];
                        Session["ModelDetails"] = dtDetails;
                        BindDataToGrid();
                        objSparesTarget = null;
                        dvbudplan.Style.Add("display", "");
                    }
                    else
                    {
                        dtDetails = ds.Tables[0];
                        Session["ModelDetails"] = dtDetails;
                        BindDataToGrid();
                        dvbudplan.Style.Add("display", "");
                    }
                }
                else
                {
                    ds = null;
                    dtDetails = null;
                    Session["ModelDetails"] = dtDetails;
                    BindDataToGrid();
                    dvbudplan.Style.Add("display", "none");
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGrid()
        {
            try
            {
                if (Session["ModelDetails"] == null)
                    Session["ModelDetails"] = dtDetails;
                else
                    dtDetails = (DataTable)Session["ModelDetails"];

                GrdAnnualTarget.DataSource = dtDetails;
                GrdAnnualTarget.DataBind();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    //if (bSaveRecord(false) == false) return;
                    DisplayCurrentRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            int Model_Cat_ID = 0;
            string Model_Name = "";
            var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow row1 = GrdAnnualTarget.Rows[rowIndex];

            Label lblModel_Cat_ID = (Label)row1.FindControl("lblModel_Cat_ID");
            Model_Cat_ID = Func.Convert.iConvertToInt(lblModel_Cat_ID.Text);
            Label lblModel_Name = (Label)row1.FindControl("lblModel_Name");
            Model_Name = lblModel_Name.Text;

            bindGrid(Model_Cat_ID, Model_Name);
            mpeSelectModel.Show();
        }
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        public void bindGrid(int iModel_Cat_ID, string sModelName)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                if (sModelName != "")
                {
                    lblModelName.Text = sModelName;
                }

                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_ModelAggregate", iModel_Cat_ID);

                if (dsSrchgrid == null)
                {
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                }
                else
                {
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    return;
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

        #region Upload PDF Files
        private bool IsPdf(HttpPostedFile file)
        {
            return ((file != null) && (System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "application/pdf")) && (file.ContentLength > 0));
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                HttpPostedFile file = txtFilePath.PostedFile;
                if ((file != null) && (file.ContentLength > 0))
                {
                    if (IsPdf(file) == false)
                    {
                        // Invalid file type 
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please make sure your file is in pdf format')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select File')", true);
                    return;
                }

                if (txtFilePath.HasFile)
                {
                    clsContent objContent = new clsContent();
                    bool bNewFile = false;
                    string sBasePath = "";
                    string sFullPath = "";
                    sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);

                    string strFileName = Path.GetFileNameWithoutExtension(txtFilePath.PostedFile.FileName);
                    string strFileExtType = Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
                    string strFileType = strFileExtType;
                    //sFullPath = sBasePath + @"Parts\Model Catelogue\" + strFileName + strFileExtType;
                    string strFileNamePath = @"Parts\Model Catelogue\" + strFileName + strFileExtType;
                    sFullPath = Path.Combine(sBasePath, strFileNamePath);
                    
                    if (System.IO.File.Exists(@sFullPath))
                    {
                        System.IO.File.Delete(@sFullPath);
                        bNewFile = true;
                    }
                    else
                    {
                        lblMessage.Text = "The file you are trying to upload new File!";
                        bNewFile = false;
                    }


                    string[] acceptedFileTypes = new string[1];
                    acceptedFileTypes[0] = ".pdf";

                    bool acceptFile = false;
                    //should we accept the file?
                    for (int i = 0; i <= 0; i++)
                    {
                        if (strFileExtType == acceptedFileTypes[i])
                        {
                            //accept the file, yay!
                            acceptFile = true;
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
                    {
                        if (bNewFile == true)
                        {
                            //Saving it in temperory Directory.                                       
                            DirectoryInfo destination = new DirectoryInfo(sBasePath + "Parts\\Model Catelogue");
                            if (!destination.Exists)
                            {
                                destination.Create();
                            }
                            txtFilePath.SaveAs((sBasePath + "Parts\\Model Catelogue" + "\\" + strFileName + strFileExtType));

                            //txtFilePath.SaveAs(@sFullPath);
                            //txtFilePath.SaveAs(@"\\10.173.2.22\Upload Documents\Transaction\Parts\Model Catelogue\" + strFileName + strFileExtType);
                            //txtFilePath.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["DownloadDomesticFiles"]) + "\\Parts\\Model Catelogue\\" + strFileName + strFileExtType);
                   
                            //txtFilePath.SaveAs(ServerPath + strFileName + strFileExtType);
                            string sMessage = "File Uploaded successfully!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        }
                        else
                        {
                            string sMessage = "This File does not exist in Parts Catalogue List!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        }

                    }//END ELSE
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #endregion

        #region GetCleintIPAddress
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        #endregion
    }
}