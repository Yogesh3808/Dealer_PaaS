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
    public partial class frmPartMaster : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iPartID = 0;
        private int ID = 0;

        private string newactive;
        private string RateEditing_InDMS;
        int iMenuId = 0;
        int totalcount = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                clsPart objPart = new clsPart();
                DataSet ds = new DataSet();
                // ds = objPart.GetMaxPart();
                if (!IsPostBack)
                {
                   // Func.Common.BindDataToCombo(drpUnit, clsCommon.ComboQueryType.Unit, 0, "");
                    clsDB objDB = new clsDB();
                    if (iMenuId == 606 || iMenuId == 301)
                    {
                        lblTitle.Text = "Part";
                        ds = objPart.GetMaxPart("01");
                    }
                    else if (iMenuId == 608)
                    {
                        lblTitle.Text = "Local Part";
                        ds = objPart.GetMaxPart("99");
                    }
                    iPartID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iPartID == 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            iPartID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                            txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        }
                    }
                    if (iPartID != 0)
                    {
                        GetDataAndDisplay();
                    }
                }

                FillSelectionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

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
                clsPart objPart = new clsPart();
                DataSet ds = new DataSet();
                //int iPartID = Func.Convert.iConvertToInt(txtID.Text);
                //iPartID = 1;
                if (iPartID != 0)
                {
                    if (iMenuId == 606 )
                    {
                       // ToolbarC.Visible = false;
                        ds = objPart.GetPart(iPartID, "01",iDealerID );
                    }
                    else if (iMenuId == 301)
                    {
                         ToolbarC.Visible = false;
                         lblLocation.Visible = false;
                         txtLocation.Visible = false; 
                        ds = objPart.GetPart(iPartID, "01",0);
                    }
                    else if (iMenuId == 608)
                    {
                       // ToolbarC.Visible = true;
                        ds = objPart.GetPart(iPartID, "99",iDealerID);
                    }
                    DisplayData(ds);
                    objPart = null;
                }
                else
                {
                    ds = null;
                    objPart = null;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }


        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                iPartID = Func.Convert.iConvertToInt(txtID.Text);
                // ViewState["PartID"] = iPartID;
                GetDataAndDisplay();
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
                string groupCode = "";

                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {

                     txtPartNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["parts_no"]);
                    txtMTIPartName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["part_name"]);
                    txtBaseUnit.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Base Unit"]);
                    //drpUnit.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UnitId"]);
                    txtGroup.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gr_Name"]);
                    txtMinOrderQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MOQ"]);
                    txtPartCategory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartCategory"]);
                    txtBlockForPurchase.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Blk_fr_purch"]);
                    txtSupersededPart.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Superseded Part"]);
                    txtLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Location"]);
                    newactive = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (newactive == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else if (newactive == "N")
                    {
                        drpActive.SelectedValue = "2";
                    }
                    groupCode = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gr_No"]);

                    //if (groupCode == "99")//saving and editing allowed only local part 
                    //{
                    //    txtPartNo.Enabled = true;
                    //    txtMTIPartName.Enabled = true;
                    //    txtBaseUnit.Text = "Ltrs.";
                    //   // drpUnit.Enabled = true;
                    //    txtGroup.Enabled = true;
                    //    txtMinOrderQty.Enabled = true;
                    //    drpActive.Enabled = true;
                    //    txtPartCategory.Enabled = true;

                    //}
                    //else
                    //{
                    //    txtPartNo.Enabled = false;
                    //    txtMTIPartName.Enabled = false;
                    //    //txtBaseUnit.Text = "Ltrs.";
                    //   // drpUnit.Enabled = false;
                    //    txtGroup.Enabled = false;
                    //    txtMinOrderQty.Enabled = false;
                    //    drpActive.Enabled = false;
                    //    txtPartCategory.Enabled = false;

                    //}



                }
                else
                {
                    ClearDealerHeader();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void ClearDealerHeader()
        {
            txtPartNo.Text = "";
            txtMTIPartName.Text = "";
            txtBaseUnit.Text = "Ltrs.";
           // drpUnit.SelectedValue = "0";
            txtGroup.Text = "Local Part";
            txtMinOrderQty.Text = "";
            drpActive.SelectedValue = "1";
            txtPartCategory.Text = "";
            txtPartNo.Enabled = true;
            txtMTIPartName.Enabled = true;
            txtBaseUnit.Text = "Ltrs.";
            //drpUnit.Enabled = true;
            txtGroup.Enabled = true;
            txtMinOrderQty.Enabled = true;
            drpActive.Enabled = true;
            txtPartCategory.Enabled = true;


        }

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = false;
                SearchGrid.AddToSearchCombo("Part No");
                SearchGrid.AddToSearchCombo("Part Name");
                SearchGrid.AddToSearchCombo("Group");
                SearchGrid.AddToSearchCombo("Part Category");
                SearchGrid.iDealerID = iDealerID;
               
                if (iMenuId == 606)
                {
                    SearchGrid.sModelPart = "01";
                    SearchGrid.sGridPanelTitle = "Part List";
                }
                else if (iMenuId == 301)
                {
                    SearchGrid.iDealerID = 1;
                    SearchGrid.sModelPart = "01";
                    SearchGrid.sGridPanelTitle = "Part List";
                }
                else if (iMenuId == 608)
                {
                    SearchGrid.sModelPart = "99";
                    SearchGrid.sGridPanelTitle = "Local Part List";
                }
                SearchGrid.sSqlFor = "PartMaster";
                SearchGrid.iBrHODealerID = iHOBr_id;




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
        private bool bValidateRecord()
        {
            string sMessage = " ";
            bool bValidateRecord = true;
             if (txtLocation.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Location.";
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
                clsPart objPart = new clsPart();
                ImageButton ObjImageButton = (ImageButton)sender;

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                    txtID.Text = "0";
                    ClearDealerHeader();
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                     if (bValidateRecord() == false) return;

                    ID = Func.Convert.iConvertToInt(txtID.Text);

                    //ID = objPart.bSaveLocalPartMaster(ID, txtPartNo.Text.Trim(), txtMTIPartName.Text.Trim(),
                    //    drpUnit.SelectedItem.Text, drpActive.SelectedItem.Text, txtMinOrderQty.Text, txtPartCategory.Text.Trim());
                    ID = objPart.bSaveLocalPartMaster(ID, txtLocation.Text.Trim(), iDealerID,"01","");
                    if (ID > 0)
                    {
                        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);

                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
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
                string strFileName = "PartMaster";
                dtUpload = objExcel.UploadExcelFile(txtFilePath, HttpContext.Current.Server.MapPath("~/DownLoadFiles/PartLocation/"), strFileName, ref sMessage);
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
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage1 + + totalcount + "');</script>");


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

        private void SetListOfNotUploadedPartNo(DataTable dtPartList)
        {
            try
            {
                clsDB objDB =new clsDB();
                string  sPartList="";
                totalcount = dtPartList.Rows.Count;
                if (Func.Common.iRowCntOfTable(dtPartList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                    {
                       string partname=  dtPartList.Rows[iRowCnt]["PartNo"].ToString();
                       if (partname != "")
                       {
                           DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_CheckBinlocation", iDealerID, dtPartList.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtPartList.Rows[iRowCnt]["BinLoation"]);
                           if (ds != null)
                           {
                               if (ds.Tables[0].Rows.Count != 0)
                               {
                                   totalcount = totalcount - 1;
                                   sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["PartNo"]) + ",";
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
                if ((txtFilePath.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    string query = null;
                    string connString = "";
                    string strFileName = "SparesLocation" + Session["iDealerID"].ToString() + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        txtFilePath.SaveAs(Server.MapPath("~/DownloadFiles/PartLocation/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/PartLocation/" + strFileName + ".xlsx");

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

        public bool bSaveRecord(DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Details
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
            //saveDetails
            int Chassis_ID = 0;
            int iRowCnt = 0;
            bool bSaveRecord = false;
            try
            {
                if (dtDet.Rows.Count > 0)
                {
                    for (iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        if (dtDet.Rows[iRowCnt]["PartNo"].ToString() != "")
                        {
                            //objDB.ExecuteStoredProcedure("SP_EGP_DCSUploadMasterLocalPartsPriceMaster", dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtDet.Rows[iRowCnt]["Eff From Date"], dtDet.Rows[iRowCnt]["Eff To Date"], dtDet.Rows[iRowCnt]["MRP"], iDealerID);
                           // objDB.ExecuteStoredProcedure("SP_DCSUploadMasterLocalPartsPriceMaster", dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtDet.Rows[iRowCnt]["EffFromDate"], dtDet.Rows[iRowCnt]["EffToDate"], dtDet.Rows[iRowCnt]["MRP"], dtDet.Rows[iRowCnt]["LIST"], dtDet.Rows[iRowCnt]["NDP"], iDealerID);
                            objDB.ExecuteStoredProcedure("sp_SaveBinlocation",iDealerID, dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtDet.Rows[iRowCnt]["BinLoation"]);

                        }
                    }
                    if (dtDet.Rows.Count == iRowCnt)
                    {
                       
                    }


                }

                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }
    }
}