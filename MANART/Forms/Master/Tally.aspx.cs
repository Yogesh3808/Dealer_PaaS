using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
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
using System.Data.OleDb;

namespace MANART.Forms.Master
{
    public partial class Tally : System.Web.UI.Page
    {
        int iMenuId = 0;
        int totalcount = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            clsPart objPart = new clsPart();
            DataSet ds = new DataSet();
            iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
            iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
            FillSelectionGrid();
            if (!IsPostBack)
            {
                //ds = GetdealerWisePart(iDealerID);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                //    GetDataAndDisplay();
                //} 
            }
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                clsPart objPart = new clsPart();
                ImageButton ObjImageButton = (ImageButton)sender;

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                    txtID.Text = "0";
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    int ID = Func.Convert.iConvertToInt(txtID.Text);
                    ID = SaveDealerwisepart(ID, txtSalesLedger.Text, txtPurchaseLedger.Text, iDealerID,ddluploadfile.SelectedValue);
                    if (ID > 0)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                       Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                    txtID.Text = Func.Convert.sConvertToString(ID);
                    FillSelectionGrid();

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            GetDataAndDisplay();
        }

        private int SaveDealerwisepart(int ID, string salesLedger, string purchaseLedger, int iDealerID,string uploadfilenm)
        {
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();

                ID = objDB.ExecuteStoredProcedure("SP_Save_Dealerwisepart", ID, salesLedger, purchaseLedger, iDealerID, uploadfilenm);


                objDB.CommitTransaction();
                return ID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return ID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        private DataSet GetdealerWisePart(int iDealerID,string filename)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxPartDealerwise", iDealerID, filename);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        private void FillSelectionGrid()
        {
            try
            {
                if (ddluploadfile.SelectedValue == "1")
                {
                    
                    SearchGrid.bGridFillUsingSql = false;
                    SearchGrid.AddToSearchCombo1("Part No");
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sSqlFor = "DealerwisePart";
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sGridPanelTitle = "Part Ledger Details";
                }
                else if (ddluploadfile.SelectedValue == "2")
                {
                    SearchGrid.bGridFillUsingSql = false;
                    SearchGrid.AddToSearchCombo1("Model Code");
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sSqlFor = "ModelLedger";
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sGridPanelTitle = "Model Ledger Details";
                }

                else if (ddluploadfile.SelectedValue == "3")
                {

                    SearchGrid.bGridFillUsingSql = false;
                    SearchGrid.AddToSearchCombo1("labour Code");
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sSqlFor = "LabourLedger";
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sGridPanelTitle = "Labour Ledger Details";
   
                }

                else if (ddluploadfile.SelectedValue == "4")
                {
                    SearchGrid.bGridFillUsingSql = false;
                    SearchGrid.AddToSearchCombo1("Tax Name");
                    SearchGrid.iDealerID = iDealerID;
                    SearchGrid.sSqlFor = "TaxLedger";
                    SearchGrid.iBrHODealerID = iHOBr_id;
                    SearchGrid.sGridPanelTitle = "Tax Ledger Details";
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DataTable dtUpload = null;
            clsExcel objExcel = null;
            try
            {
                txtListPartNo.Visible = false;
                lblListPartNo.Visible = false;
                objExcel = new clsExcel();
                DataTable dt = GetListOfPartNo();
                dtUpload = new DataTable();
                string sMessage = "";
                string strFileName = "Ledger";
                dtUpload = objExcel.UploadExcelFile(txtFilePath, HttpContext.Current.Server.MapPath("~/DownLoadFiles/Ledger/"), strFileName, ref sMessage);
                if (dtUpload == null && sMessage != "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    return;
                }
                if (bSaveRecord(dtUpload) == true)
                {
                    if (dtUpload == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in upload the the file contact admin.');</script>");
                        return;
                    }
                    else
                    {
                        SetListOfNotUploadedPartNo(dtUpload);
                        //dtDetails = dsUploadPartDetails.Tables[1];
                    }
                    //if (iCountNotUpload != 0)
                    //{
                    //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + " parts not uploaded.');</script>");
                    //}
                    string sMessage1 = "File Uploaded successfully! Total Records: ";
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage1 + +totalcount + "');</script>");

                    FillSelectionGrid();
                }

            }

            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (dtUpload != null) dtUpload = null;
                if (objExcel != null) objExcel = null;
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
        }

        public bool bSaveRecord(DataTable dtDet)
        {
               bool bSaveRecord = false;
               clsDB objDB = new clsDB();
            try
            {
           
                objDB.BeginTranasaction();               
                if (bSaveDetails(objDB, dtDet) == false) goto ExitFunction;
                bSaveRecord = true;

            ExitFunction:
                if (bSaveRecord == false)
                {
                    objDB.RollbackTransaction();
                    bSaveRecord = false;
                }
                else
                {
                    objDB.CommitTransaction();
                }
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

        private bool bSaveDetails(clsDB objDB, DataTable dtDet)
        {


            int iRowCnt = 0;
            bool bSaveRecord = false;
            try
            {
                if (dtDet.Rows.Count > 0)
                {
                    if (ddluploadfile.SelectedValue == "1")
                    {
                        for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                        {
                            if (dtDet.Rows[iRowCnt]["PART CODE"].ToString() != "")
                            {
                                objDB.ExecuteStoredProcedure("sp_SaveSalesLedger", iDealerID, dtDet.Rows[iRowCnt]["PART CODE"].ToString().Trim(), dtDet.Rows[iRowCnt]["Sales Ledger"], dtDet.Rows[iRowCnt]["Purchase Ledger"]);

                            }
                        }
                        if (dtDet.Rows.Count == iRowCnt)
                        {

                        }
                    }
                    else if (ddluploadfile.SelectedValue == "2")
                    {
                        for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                        {
                            if (dtDet.Rows[iRowCnt]["Model_code"].ToString() != "")
                            {
                                 objDB.ExecuteStoredProcedure("sp_SaveSalesModelLedger", iDealerID, dtDet.Rows[iRowCnt]["Model_code"].ToString().Trim(), dtDet.Rows[iRowCnt]["Sales Ledger"], dtDet.Rows[iRowCnt]["Purchase Ledger"]);

                            }
                        }
                        if (dtDet.Rows.Count == iRowCnt)
                        {

                        }
                    }
                    else if (ddluploadfile.SelectedValue == "3")
                    {
                        for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                        {
                            if (dtDet.Rows[iRowCnt]["Labour_code"].ToString() != "")
                            {
                               objDB.ExecuteStoredProcedure("sp_SaveSalesLabourLedger", iDealerID, dtDet.Rows[iRowCnt]["Labour_code"].ToString().Trim(), dtDet.Rows[iRowCnt]["Sales Ledger"], dtDet.Rows[iRowCnt]["Purchase Ledger"]);

                            }
                        }
                        if (dtDet.Rows.Count == iRowCnt)
                        {

                        }
                    }
                   
                }
              
                bSaveRecord = true;
             
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }

        private DataTable GetListOfPartNo()
        {
            DataTable dtLayout = new DataTable();
            try
            {
               
                if ((txtFilePath.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    string query = null;
                    string connString = "";
                    string strFileName = "Ledger" + Session["iDealerID"].ToString() + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();

                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        txtFilePath.SaveAs(Server.MapPath("~/DownloadFiles/Ledger/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/Ledger/" + strFileName + ".xlsx");

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    query = "SELECT * FROM [Sheet1$]";

                    conn = new OleDbConnection(connString);
         
                    if (conn.State == ConnectionState.Closed) conn.Open();
                 
                    cmd = new OleDbCommand(query, conn);
                    da = new OleDbDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dtLayout = ds.Tables[0];

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtLayout;

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
              GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void GetDataAndDisplay()
        {
            try
            {
                clsPart objPart = new clsPart();
                DataSet ds = new DataSet();
                int ID = Func.Convert.iConvertToInt(txtID.Text);
                ds = GetPartDetails(ID,ddluploadfile.SelectedValue);
                DisplayData(ds);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void DisplayData(DataSet ds)
        {
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPartNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["parts_no"]);
                    txtSalesLedger.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Sales_Ledger"]);
                    txtPurchaseLedger.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Purchase_Ledger"]);
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private DataSet GetPartDetails(int ID,string FileName)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartDealerwiseDetails", ID, FileName);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        private void SetListOfNotUploadedPartNo(DataTable dtPartList)
        {
            try
            {
                clsDB objDB = new clsDB();
                string sPartList = "";
                totalcount = dtPartList.Rows.Count;
                if (Func.Common.iRowCntOfTable(dtPartList) != 0)
                {
                    if(ddluploadfile.SelectedValue=="1")
                    {
                        for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                        {
                            string partname = dtPartList.Rows[iRowCnt]["PART CODE"].ToString();
                            if (partname != "")
                            {
                                DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_CheckPartSalesLedger", iDealerID, dtPartList.Rows[iRowCnt]["PART CODE"].ToString().Trim(), dtPartList.Rows[iRowCnt]["Sales Ledger"], ddluploadfile.SelectedValue);
                                if (ds != null)
                                {
                                    if (ds.Tables[0].Rows.Count != 0)
                                    {
                                        totalcount = totalcount - 1;
                                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["PART CODE"]) + ",";
                                        lblListPartNo.Text = "Parts which are not uploaded";
                                        txtListPartNo.Text = sPartList.ToString();
                                        txtListPartNo.Visible = true;
                                        lblListPartNo.Visible = true;
                                    }
                                }

                            }
                            else
                            {
                                totalcount = totalcount - 1;
                            }
                        }
                    }
                    if (ddluploadfile.SelectedValue == "2")
                    {
                        for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                        {
                            string partname = dtPartList.Rows[iRowCnt]["Model_code"].ToString();
                            if (partname != "")
                            {
                                DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_CheckPartSalesLedger", iDealerID, dtPartList.Rows[iRowCnt]["Model_code"].ToString().Trim(), dtPartList.Rows[iRowCnt]["Sales Ledger"], ddluploadfile.SelectedValue);
                                if (ds != null)
                                {
                                    if (ds.Tables[0].Rows.Count != 0)
                                    {
                                        totalcount = totalcount - 1;
                                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["Model_code"]) + ",";
                                        lblListPartNo.Text = "Model which are not uploaded";
                                        txtListPartNo.Text = sPartList.ToString();
                                        txtListPartNo.Visible = true;
                                        lblListPartNo.Visible = true;
                                    }
                                }

                            }
                            else
                            {
                                totalcount = totalcount - 1;
                            }
                        }
                    }
                    if (ddluploadfile.SelectedValue == "3")
                    {
                        for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                        {
                            string partname = dtPartList.Rows[iRowCnt]["Labour_code"].ToString();
                            if (partname != "")
                            {
                                DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_CheckPartSalesLedger", iDealerID, dtPartList.Rows[iRowCnt]["Labour_code"].ToString().Trim(), dtPartList.Rows[iRowCnt]["Sales Ledger"], ddluploadfile.SelectedValue);
                                if (ds != null)
                                {
                                    if (ds.Tables[0].Rows.Count != 0)
                                    {
                                        totalcount = totalcount - 1;
                                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["Labour_code"]) + ",";
                                        lblListPartNo.Text = "Labour which are not uploaded";
                                        txtListPartNo.Text = sPartList.ToString();
                                        txtListPartNo.Visible = true;
                                        lblListPartNo.Visible = true;
                                    }
                                }

                            }
                            else
                            {
                                totalcount = totalcount - 1;
                            }
                        }
                    }
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

        protected void ddluploadfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                txtListPartNo.Visible = false;
                lblListPartNo.Visible = false;
                if (ddluploadfile.SelectedValue =="1")
                {
                    TblControl.Visible = true;
                    UploafFile.Visible = true;
                    clearheader();
                    ds = GetdealerWisePart(iDealerID, ddluploadfile.SelectedValue);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        GetDataAndDisplay();
                    }
                 
                    lblfilename.Text = "Part Name";
                    lblFileuploadname.Text = "DealerCode_PartSaleLedger_Datestamp";
                    FillSelectionGrid();
               
                }
                else if (ddluploadfile.SelectedValue == "2")
                {
                    TblControl.Visible = true;
                    UploafFile.Visible = true;
                    clearheader();
                    ds = GetdealerWisePart(iDealerID, ddluploadfile.SelectedValue);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        GetDataAndDisplay();
                    }                    
                    lblfilename.Text = "Model Name";
                    lblFileuploadname.Text = "DealerCode_ModelLedger_Datestamp";
                    FillSelectionGrid();
     
                }
                else if (ddluploadfile.SelectedValue == "3")
                {
                    clearheader();
                    TblControl.Visible = true;
                    UploafFile.Visible = true;
                    ds = GetdealerWisePart(iDealerID, ddluploadfile.SelectedValue);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        GetDataAndDisplay();
                    }
                    lblfilename.Text = "Labour Code";
                    lblFileuploadname.Text = "DealerCode_Labourledger_Datestamp";
                    FillSelectionGrid();
                }
                else if (ddluploadfile.SelectedValue == "4")
                {
                    clearheader();
                    TblControl.Visible = true;
                    UploafFile.Visible = false;
                    ds = GetdealerWisePart(iDealerID, ddluploadfile.SelectedValue);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        GetDataAndDisplay();
                    }
                    lblfilename.Text = "Tax Name";
                    lblFileuploadname.Text = "DealerCode_Taxledger_Datestamp";
                    FillSelectionGrid();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void clearheader()
        {
            try
            {
                txtPartNo.Text = "";
                txtSalesLedger.Text = "";
                txtPurchaseLedger.Text = "";
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
