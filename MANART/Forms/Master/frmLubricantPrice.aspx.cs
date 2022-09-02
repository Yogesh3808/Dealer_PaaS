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

namespace MANART.Forms.Master
{

    public partial class frmLubricantPrice : System.Web.UI.Page
    {
        private string _sModelPart = "";
        private int iLubPriceID = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        private int iID;
        int iUserId = 0;
        private int StateId = 0;
        private int CountryId = 0;
        private string DealerOrigin;
        private string LubType;
        int iMenuId = 0;
        string[] url1;
        DataSet DSPrice = new DataSet();
        clsDB objDB = new clsDB();
        private DataSet dsDetails = new DataSet();
        DataTable dtDetails = null;
        private string sDealerCode = "";
   
       protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                    iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                    iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                    iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                    sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);

                    if (iDealerID == 0)
                    {
                        iDealerID = 201;
                     }
                    iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                    if (iMenuId == 723 && iUserId==28)  // Upload option to Deepak Panwar only
                    {
                        PDealerHeaderDetails.Visible = true;
                       // PSelectionGrid.Visible = true;
                    }
                    else
                    {
                        PDealerHeaderDetails.Visible = false;
                    }


                if (!IsPostBack)
                {
                    
                    DisplayData(iDealerID);
                    GetMaxLubPartRate();
                }
                //ModalPopupExtender1.Show();
               
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

       private void GetMaxLubPartRate()
       {
           try
           {
               clsDB objDB = new clsDB();
               DataSet ds1 = new DataSet();
               ds1 = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxLubpartRate");
               DisplayData(ds1);
           }
           catch (Exception)
           {
               
             
           }
       }
       private void DisplayData(int iDealerID)
        {
            try
            {

                dsDetails = GetLubricantPartRate(iDealerID);

                if (dsDetails.Tables[0].Rows.Count > 0)
                {

                    DataView dvSearchDetail = new DataView();
                    dvSearchDetail = dsDetails.Tables[0].DefaultView;
                    //if (drpSearch.SelectedValue == "1" && txtSearch.Text != "")
                    //{
                    //    dvSearchDetail.RowFilter = "State LIKE '" + txtSearch.Text + "*'";
                    //}
                    if (drpSearch.SelectedValue == "1" && txtSearch.Text != "")
                    {
                        dvSearchDetail.RowFilter = "LubricantCode LIKE '" + txtSearch.Text + "*'";
                    }
                    dtDetails = dvSearchDetail.ToTable();
                    if (dtDetails.Rows.Count > 0)
                    {
                        gvLubPartRateDetails.DataSource = dtDetails;
                        gvLubPartRateDetails.DataBind();
                        GridColumnHideShow();
                        btnClearSearch.Visible = true;
                        lblMessage1.Visible = false;
                    }
                    else
                    {
                        gvLubPartRateDetails.DataSource = null;
                        gvLubPartRateDetails.DataBind();
                        lblMessage1.Text = "Record does not Exist ";
                        lblMessage1.ForeColor = System.Drawing.Color.Red;
                        lblMessage1.Visible = true;
                        btnClearSearch.Visible = true;
                    }
                }
                else
                {
                    //lblMessage.Text = "Data not Found";
                    //lblMessage.ForeColor = System.Drawing.Color.Green;
                    //lblMessage.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
       private void GridColumnHideShow()
       {
           for (int i = 0; i < gvLubPartRateDetails.Rows.Count; i++)
           {
               gvLubPartRateDetails.HeaderRow.Cells[9].Style.Add("display", (sDealerCode.Trim().StartsWith("R"))? "none":""); // Hide Header        
               gvLubPartRateDetails.Rows[i].Cells[9].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : "");//Hide Cell

               gvLubPartRateDetails.HeaderRow.Cells[10].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : ""); // Hide Header        
               gvLubPartRateDetails.Rows[i].Cells[10].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : "");//Hide Cell
           }

       }
       private void GridColumnHideShowForPriceHistory()
       {
           for (int i = 0; i < PartPriceGrid.Rows.Count; i++)
           {
               PartPriceGrid.HeaderRow.Cells[6].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : ""); // Hide Header        
               PartPriceGrid.Rows[i].Cells[6].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : "");//Hide Cell

               PartPriceGrid.HeaderRow.Cells[8].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : ""); // Hide Header        
               PartPriceGrid.Rows[i].Cells[8].Style.Add("display", (sDealerCode.Trim().StartsWith("R")) ? "none" : "");//Hide Cell
           }

       }

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if ((txtFilePath.HasFile))
        //        {

        //            OleDbConnection conn = new OleDbConnection();
        //            OleDbCommand cmd = new OleDbCommand();
        //            OleDbDataAdapter da = new OleDbDataAdapter();
        //            DataSet ds = new DataSet();
        //            DataSet dsNotUploadchassisDetails = new DataSet();
        //            DataTable dtLayout = new DataTable();
        //            string query = null;
        //            string connString = "";
        //            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //            string strFileExtType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
        //            string strFileType = ".xlsx";


        //            //Check file type
        //            if (strFileExtType == ".xls" || strFileExtType == ".xlsx")
        //            {
        //                txtFilePath.SaveAs(Server.MapPath("~/DownLoadFiles/LubricantPartRate/" + strFileName + strFileType));
        //            }
        //            else
        //            {
        //                lblMessage.Text = "Only excel files allowed";
        //                lblMessage.ForeColor = System.Drawing.Color.Red;
        //                lblMessage.Visible = true;
        //                return;
        //            }

        //            string strNewPath = Server.MapPath("~/DownLoadFiles/LubricantPartRate/" + strFileName + strFileType);

        //            //Connection String to Excel Workbook
        //            if (strFileType.Trim() == ".xls")
        //            {
        //                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        //            }
        //            else if (strFileType.Trim() == ".xlsx")
        //            {
        //                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        //            }

        //            query = "SELECT * FROM [Sheet1$]";
        //            //query = "SELECT [Country],[Capital] FROM [Sheet1$] WHERE [Currency]=’Rupee’"
        //            //query = "SELECT [Country],[Capital] FROM [Sheet1$]"

        //            //Create the connection object
        //            conn = new OleDbConnection(connString);
        //            //Open connection
        //            if (conn.State == ConnectionState.Closed) conn.Open();
        //            //Create the command object
        //            cmd = new OleDbCommand(query, conn);
        //            da = new OleDbDataAdapter(cmd);
        //            ds = new DataSet();
        //            da.Fill(ds);

        //            try
        //            {
        //                if (bSaveRecord(ds.Tables[0]) == true)
        //                {
        //                   dsDetails = GetLubricantPartRate();
        //                   DataView dvCurrentCountUploadDetail = new DataView();
        //                   if (dsDetails.Tables[0].Rows.Count > 0)
        //                   {
        //                       gvLubPartRateDetails.DataSource = dsDetails.Tables[0];
        //                       gvLubPartRateDetails.DataBind();
        //                       dvCurrentCountUploadDetail = dsDetails.Tables[0].DefaultView;
        //                       string Currdate = null;
        //                       Currdate = Func.Convert.tConvertToDate(DateTime.Now, false);
        //                       dvCurrentCountUploadDetail.RowFilter = "DCS_Update_Date LIKE '" + Currdate + "*'";

        //                       string sMessage = "File Uploaded successfully! Total Records: " + dvCurrentCountUploadDetail.Count;
        //                       Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");


        //                       da.Dispose();
        //                       conn.Close();
        //                       conn.Dispose();
        //                   }

        //                }
        //                else
        //                {
        //                    string sMessage = "Error : Data Uploading Failed!";
        //                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
        //               }


        //            }
        //            catch (Exception ex)
        //            {
        //                string sMessage = "Error : Data Uploading Failed!";
        //                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
        //           }



        //        }
        //        else
        //        {
        //            string sMessage = "Please select an excel file first";
        //            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
        //            //lblMessage.Text = "Please select an excel file first";
        //            //lblMessage.ForeColor = System.Drawing.Color.Red;
        //            //lblMessage.Visible = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }


        //}
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DataTable dtUpload = null;
            clsExcel objExcel = null;
            try
            {
                objExcel = new clsExcel();
                dtUpload = new DataTable();
                string sMessage = "";
                string strFileName = "LubricantPartRate";
                dtUpload = objExcel.UploadExcelFile(txtFilePath, HttpContext.Current.Server.MapPath("~/DownLoadFiles/LubricantPartRate/"), strFileName, ref sMessage);
                if (dtUpload == null && sMessage != "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    return;
                }
                if (bSaveRecord(dtUpload) == true)
                {
                    dsDetails = GetLubricantPartRate(iDealerID);
                    ////DataView dvCurrentCountUploadDetail = new DataView();
                    if (dsDetails.Tables[0].Rows.Count > 0)
                    {
                        gvLubPartRateDetails.DataSource = dsDetails.Tables[0];
                        gvLubPartRateDetails.DataBind();
                    //    dvCurrentCountUploadDetail = dsDetails.Tables[0].DefaultView;
                    //    string Currdate = null;
                    //    Currdate = Func.Convert.tConvertToDate(DateTime.Now, false);
                    //    dvCurrentCountUploadDetail.RowFilter = "DCS_Update_Date LIKE '" + Currdate + "*'";

                      //  string sMessage1 = "File Uploaded successfully! Total Records: " + dvCurrentCountUploadDetail.Count;
                        string sMessage1 = "File Uploaded successfully!";
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage1 + ".');</script>");


                    }

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

        public DataSet GetLubricantPartRate(int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetLubricantPartRate", iDealerID);
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
                        if (dtDet.Rows[iRowCnt]["LubricantCode"].ToString() != "")
                        {
                            objDB.ExecuteStoredProcedure("SP_MANUploadMasterLubricantPartsPriceMaster", dtDet.Rows[iRowCnt]["LubricantCode"].ToString().Trim(), dtDet.Rows[iRowCnt]["LubricantDescription"].ToString(), dtDet.Rows[iRowCnt]["PackSize"], dtDet.Rows[iRowCnt]["HSNCode"], dtDet.Rows[iRowCnt]["GST%"], dtDet.Rows[iRowCnt]["DLP"], dtDet.Rows[iRowCnt]["ListPrice"], dtDet.Rows[iRowCnt]["MRP"], dtDet.Rows[iRowCnt]["EffFromDate"], dtDet.Rows[iRowCnt]["EffToDate"], dtDet.Rows[iRowCnt]["Unit"]);

                        }
                    }
                    if (dtDet.Rows.Count == iRowCnt)
                    {
                        objDB.ExecuteStoredProcedure("Lubricantpriceconsumtion_DealerWise", 0);

                    }
                    //change for statewise consumption 
                    //if (dtDet.Rows.Count == iRowCnt)
                    //{

                    //    objDB.ExecuteStoredProcedure("SP_DCSUploadMasterLubricantPartsPriceMaster_Main_statenew", 0);
                    //}    

                }

                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DisplayData(iDealerID);
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            btnClearSearch.Visible = false;
            txtSearch.Text = "";
            lblMessage1.Visible = false;
            DisplayData(iDealerID);
        }
        protected void gvLubPartRateDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
            gvLubPartRateDetails.PageIndex = e.NewPageIndex;
            DisplayData(iDealerID);
        }

        protected void btnedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                clsDB objDB = new clsDB();
                DataSet ds = new DataSet();
                 GridViewRow clickedrow = ((ImageButton)sender).NamingContainer as GridViewRow;
                 int LubpartID = Convert.ToInt32(gvLubPartRateDetails.DataKeys[clickedrow.RowIndex].Value);
                 if (LubpartID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetLubpartRateDetails", LubpartID );
                    DisplayData(ds);

                }
            }
            catch (Exception)
            {
            }
        }

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

                    txtLubricantCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LubricantCode"]);
                    txtLubName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Lubricant Name"]);
                    txtUnit.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Unit"]);
                    //txtDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer"]);
                    txtEffectiveFrom.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Eff_From_Date"]);
                    txtEffectiveTo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Eff_To_Date"]);
                ///    txtDepoCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Depo Code"]);
                    txtMRP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MRP"]);
                    txtLISTPRICE.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LIST_Price"]);
               //     txtActive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    //Megha 11/06/2013
                    txtNDP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NDP"]);
                    txtPackSize.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PackSize"]);
                    txtDCSUpdateDate.Text = (ds.Tables[0].Rows[0]["DCS_Update_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["DCS_Update_Date"]).ToString("dd/MM/yyyy HH:mm") : "";
                   // txtXMLCreationDate.Text = (ds.Tables[0].Rows[0]["XML_Cr_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["XML_Cr_Date"]).ToString("dd/MM/yyyy HH:mm") : "";
                    //Megha 11/06/2013
                    txtHsncode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HSN_Code"]);
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTTaxper"]) != "")
                    {
                        txtPer.Text = String.Format("{0:0.00}", Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["GSTTaxper"]));
                    }
                    else
                    {
                        txtPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTTaxper"]);
                    }
                    if (sDealerCode.Trim().StartsWith("R"))
                    {// Hide DLP and List Price in case of Arunachala
                        lblDLP.Style.Add("display", "none");
                        txtNDP.Style.Add("display", "none");
                        lblListPrice.Style.Add("display", "none");
                        txtLISTPRICE.Style.Add("display", "none");
                    }
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
           // txtModelCode.Text = "";
            //txtModelName.Text = "";
            //txtDealerCode.Text = "";
            //txtDealer.Text = "";
            txtEffectiveFrom.Text = "";
            txtEffectiveTo.Text = "";
           // txtDepoCode.Text = "";
            txtMRP.Text = "";
            txtLISTPRICE.Text = "";
            //txtActive.Text = "";
            //Megha 11/06/2013
            txtDCSUpdateDate.Text = "";
          //  txtXMLCreationDate.Text = "";
            //Megha 11/06/2013
        }


        protected void Btnshowhistory_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                GridViewRow clickedrow = ((ImageButton)sender).NamingContainer as GridViewRow;
                int iPartID = Convert.ToInt32(gvLubPartRateDetails.DataKeys[clickedrow.RowIndex].Value);
                Fillmodelpopupdetails(iPartID);
                //ModalPopupExtender1.Show();
                //mp1.Show();
              //  ModalPopupExtender4.Show();

          
            }
            catch (Exception)
            {

            }
        }
        private void Fillmodelpopupdetails(int iPartID)
        {
            try
            {

                DataSet dsDetails = new DataSet(); ;

                //if (_sModelPart == "01" || _sModelPart == "02")
                //{
                //    //if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                //    //{

                //        dsDetails = objDB.ExecuteQueryAndGetDataset("Select Distinct Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.Part_ID=" + iPartID + " And M_PartRateMaster.Dealer_ID=" + iDealerID);
                //    //}
                //    //else
                //    //    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.ID=" + iPartID);
                //}
                //if (_sModelPart == "99")
                //{
                //    //if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                //    //{

                //    //    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.Part_ID=" + iPartID + " And M_LocalPartRate.Dealer_ID=" + DealerId);
                //    //}
                //    //else
                //    //    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.ID=" + iPartID);

                //}
               // dsDetails = objDB.ExecuteQueryAndGetDataset("Select Distinct Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.Part_ID=" + iPartID + " And M_PartRateMaster.Dealer_ID=" + iDealerID);

                dsDetails = objDB.ExecuteQueryAndGetDataset("Select Distinct Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.ID=" + iPartID);

                int Part_Id = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Part_ID"].ToString());
                int Dealer_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Dealer_ID"].ToString());
                string sGroup_code = Func.Convert.sConvertToString(dsDetails.Tables[0].Rows[0]["group_code"].ToString());

                dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetPartPrice", sGroup_code, Dealer_ID, Part_Id, "All");
                if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                {

                    PartPriceGrid.DataSource = null;
                    PartPriceGrid.DataBind();
                }
                else
                {
                    //if (Func.Convert.sConvertToString(Request.QueryString["Distributor"]) == "Y")
                    //{
                    //    PartPriceGrid.Columns[0].Visible = false;
                    //}
                    //else
                    //{
                    //    PartPriceGrid.Columns[0].Visible = true;
                    //}
                    PartPriceGrid.DataSource = dtDetails;
                    ViewState["LubricantPrice"] = dtDetails;
                    PartPriceGrid.DataBind();
                    GridColumnHideShowForPriceHistory();
                }
                ModalPopupExtender4.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //protected void PartPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        PartPriceGrid.DataSource = (DataTable)ViewState["LubricantPrice"];
        //        PartPriceGrid.PageIndex = e.NewPageIndex;
        //        PartPriceGrid.DataBind();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

     


        protected void btnShow_Click(object sender, EventArgs e)
        {
            //mp1.Show();
        }

    }
}