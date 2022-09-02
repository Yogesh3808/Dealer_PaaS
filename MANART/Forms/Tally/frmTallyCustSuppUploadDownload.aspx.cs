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
using MANART.WebParts;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MANART.Forms.Tally
{
    public partial class frmTallyCustSuppUploadDownload : System.Web.UI.Page        
    {
        private DataTable dtDetails_syspara = new DataTable();
        private DataTable dtDetails_para = new DataTable();

        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        string sMessage = "";
        string DealerOrigin = "";
        string DealerCode = "";
        string DOrigin = "";
        int iUserId = 0;
        int iHOBr_id = 0;
        string sFileName = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                DealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                if (!IsPostBack)
                {
                    DrpMasterType.SelectedValue = "Customer";
                    txtType.Text = "Customer";
                    btnDownload.Text = "Download " + txtType.Text + " List";
                    BtnRefresh.Text = "Refresh " + txtType.Text + " List";
                    trSupp.Visible = (txtType.Text == "Supplier") ? true : false;
                    trCust.Visible = (txtType.Text == "Supplier") ? false : true;
                    CPEModelDetails.CollapsedText = txtType.Text + " List Details";
                    CPEModelDetails.ExpandedText = txtType.Text + " List Details";
                    btnUpload.Text = "Upload " + txtType.Text;
                }
            
                if (iDealerID != 0)
                {
                    GetDataAndDisplay();
                }
                SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                
                if (!IsPostBack)
                {
                    DrpMasterType.SelectedValue = "Customer";
                    txtType.Text = "Customer";
                    btnDownload.Text = "Download " + txtType.Text + " List";
                    BtnRefresh.Text = "Refresh " + txtType.Text + " List";
                    trSupp.Visible = (txtType.Text == "Supplier") ? true : false;
                    trCust.Visible = (txtType.Text == "Supplier") ? false : true;
                    CPEModelDetails.CollapsedText = txtType.Text + " List Details";
                    CPEModelDetails.ExpandedText = txtType.Text + " List Details";
                    btnUpload.Text = "Upload " + txtType.Text;
                }
               
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {              
                GetDataAndDisplay();
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
                ClsTally ObjTally = new ClsTally();
                DataSet ds = new DataSet();
               
                if (iDealerID != 0)
                {
                    ds = ObjTally.GetCustSuppList(iDealerID,txtType.Text.ToString().Trim());
                    DisplayData(ds);
                    ObjTally = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    ObjTally = null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
       
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
          //  SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iDealerID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
            ReadOnlyProperty(true);

        }
        private void ReadOnlyProperty(bool Flag)
        {
            if (Flag == true)
            {
                           

            }
            else
            {
               
            }
        }
        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                txtDealerCode.Text = DealerCode;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DetailsGrid.DataSource = ds.Tables[0];
                    DetailsGrid.DataBind();                    
                }
                else
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void ClearDealerHeader()
        {
           
        }
      

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
       
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            clsExcel objExcel = null;

            System.Data.DataTable dtDownload = null;
            try
            {
                objExcel = new clsExcel();                
                DataSet dts = new DataSet();
                ClsTally ObjTally = new ClsTally();

                dts = ObjTally.GetCustSuppList(iDealerID, txtType.Text.Trim());
                dtDownload = dts.Tables[0];
               
                if (dtDownload != null)
                    if (dtDownload.Rows.Count > 0)
                    {
                        dtDownload.Columns.Remove("DealerID");
                        dtDownload.Columns.Remove("ID");
                        dtDownload.Columns["Name"].ColumnName = "Name";
                        dtDownload.Columns["DCANCode"].ColumnName = "DCANCode";
                        dtDownload.Columns["LedgerName"].ColumnName = "LedgerName";        
                    }
                objExcel.DownloadInExcelFile(dtDownload, txtDealerCode.Text + "_" + txtType.Text.Trim() + ".xls", "");
            }
            catch (Exception ex)
            {
                //Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                ClsTally ObjTally = new ClsTally();
                DataTable dt = GetListOfPartNo();

                int iCountNotUpload = 0;

                string sFileNameForSave = "";
                DataSet dsUploadPartDetails;
                System.Threading.Thread.Sleep(2000);
                sFileName = FileUpload1.FileName;
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));

                //Changed by Vikram on Date 20.06.2016                
                dsUploadPartDetails = ObjTally.UploadCustomerDetailsAndGetCustomerDetails(FileUpload1.FileName, Convert.ToInt32(Session["iDealerID"].ToString()), dt, txtType.Text.Trim());
                if (dsUploadPartDetails == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);                    
                    return;
                }
                else
                {                    
                    dtDetails = dsUploadPartDetails.Tables[0];
                    SetListOfNotUploadedPartNo(dsUploadPartDetails.Tables[1]);
                }
                if (iCountNotUpload != 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + " "+ txtType.Text.Trim()+ "  not uploaded.');</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + txtType.Text.Trim() + " uploaded and Ledger names are updated into master updated .');</script>");
                }
             
                DetailsGrid.DataSource = dtDetails;
                DetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void SetListOfNotUploadedPartNo(DataTable dtPartList)
        {
            try
            {
                string sPartList = "";
                if (Func.Common.iRowCntOfTable(dtPartList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                    {
                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["Name"]) +" ("+ Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["DCANCode"]) + "),";
                    }
                    lblListPartNo.Text = ((txtType.Text.Trim() == "Customer") ? "Customer" : "Supplier") + " which are not uploaded";
                    txtListPartNo.Text = sPartList;
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
        private DataTable GetListOfPartNo()
        {
            DataTable dtLayout = new DataTable();
            try
            {
                //DataTable dtLayout = new DataTable();
                if ((FileUpload1.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    string query = null;
                    string connString = "";
                    string strFileName = ((txtType.Text.Trim() == "Customer") ? "Customer" : "Supplier") + Session["iDealerID"].ToString() + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        FileUpload1.SaveAs(Server.MapPath("~/DownloadFiles/"+ txtType.Text.Trim() +"/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/" + txtType.Text.Trim() + "/" + strFileName + ".xlsx");

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

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
                    dtLayout = ds.Tables[0];

                }
                //return dtLayout;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtLayout;

        }

        protected void DrpMasterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtType.Text = DrpMasterType.Text.Trim();
            btnDownload.Text = "Download " + txtType.Text + " List";
            BtnRefresh.Text = "Refresh " + txtType.Text + " List";
            trSupp.Visible = (txtType.Text == "Supplier") ? true : false;
            trCust.Visible = (txtType.Text == "Supplier") ? false : true;
            CPEModelDetails.CollapsedText = txtType.Text + " List Details";
            CPEModelDetails.ExpandedText = txtType.Text + " List Details";
            btnUpload.Text = "Upload " + txtType.Text;
            txtListPartNo.Text = "";
            txtListPartNo.Visible = false;
            lblListPartNo.Visible = false;
            GetDataAndDisplay();
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
            iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            DealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
            if (iDealerID != 0)
            {
                GetDataAndDisplay();
            }
            SetDocumentDetails();

        }        
    }
}