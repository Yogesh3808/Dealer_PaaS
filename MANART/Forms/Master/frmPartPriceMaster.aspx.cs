using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using AjaxControlToolkit;
using System.Drawing;

namespace MANART.Forms.Master
{
    public partial class frmPartPriceMaster : System.Web.UI.Page
    {
        private int iPartPriceID = 0;
        private int Dealer_ED = 0;

        int iUserId = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        private string GroupCode;
        int iMenuId = 0;
        private string _sModelPart = "";
        private string _sSqlFor = "";
        private int DealerId = 0;
        private string DealerOrigin;
        private DataSet dsSrchgrid;
        private int _iBrHODealerID = 0;
        DataSet DSPrice = new DataSet();
        clsDB objDB = new clsDB();
        DataTable dtDetails = null;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                if (iMenuId == 607 || iMenuId == 467)
                {

                    btnUpload.Visible = false;
                    GroupCode = "01";
                }
                else if (iMenuId == 609)
                {
                    CntDealerHeaderDetails.Visible = true;
                    btnUpload.Visible = true;
                    GroupCode = "99";
                }
                if (iMenuId == 607)
                {
                    CntDealerHeaderDetails.Visible = false;
                }

                Location.bUseSpareDealerCode = true;
                iDealerID = Location.iDealerId;
                FillSelectionGrid();
                if (!IsPostBack)
                {
                    FillDetails();
                }
                //ExpirePageCache();
                ExpirePageCache();

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
                objExcel = new clsExcel();
                dtUpload = new DataTable();
                string sMessage = "";
                string strFileName = "LocalPartRate";
                dtUpload = objExcel.UploadExcelFile(txtFilePath, HttpContext.Current.Server.MapPath("~/DownLoadFiles/LocalPartRate/"), strFileName, ref sMessage);
                if (dtUpload == null && sMessage != "")
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    return;
                }
                if (bSaveRecord(dtUpload) == true)
                {
                    //dsDetails = GetLubricantPartRate();
                    //DataView dvCurrentCountUploadDetail = new DataView();
                    //if (dsDetails.Tables[0].Rows.Count > 0)
                    //{
                    //    gvLubPartRateDetails.DataSource = dsDetails.Tables[0];
                    //    gvLubPartRateDetails.DataBind();
                    //    //dvCurrentCountUploadDetail = dsDetails.Tables[0].DefaultView;
                    //    //string Currdate = null;
                    //    //Currdate = Func.Convert.tConvertToDate(DateTime.Now, false);
                    //    //dvCurrentCountUploadDetail.RowFilter = "DCS_Update_Date LIKE '" + Currdate + "*'";

                    //    //string sMessage1 = "File Uploaded successfully! Total Records: " + dvCurrentCountUploadDetail.Count;
                    string sMessage1 = "File Uploaded successfully! Total Records: ";
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage1 + ".');</script>");


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
                            objDB.ExecuteStoredProcedure("SP_DCSUploadMasterLocalPartsPriceMaster", dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtDet.Rows[iRowCnt]["EffFromDate"], dtDet.Rows[iRowCnt]["EffToDate"], dtDet.Rows[iRowCnt]["MRP"], dtDet.Rows[iRowCnt]["LIST"], dtDet.Rows[iRowCnt]["NDP"], iDealerID);

                        }
                    }
                    if (dtDet.Rows.Count == iRowCnt)
                    {
                        objDB.ExecuteStoredProcedure("SP_DCSUploadMasterLocalPartsPriceMaster_Main", 0);
                    }


                }

                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }
        protected override void OnPreRender(EventArgs e)
        {

            try
            {
                base.OnPreRender(e);

                iDealerID = Location.iDealerId;
                if (!IsPostBack)
                {
                    FillDetails();
                }
                FillSelectionGrid();
                string strDisAbleBackButton;
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                if (txtSearch.Text != "" || hdnSort.Value != "N")
                {
                    GetDataAndBindToGrid();
                    return;
                }
                btnReadonly();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //Megha 12082011
            //Change for display dealer Code in vehicle/spares 
            Location.bUseSpareDealerCode = true;
            txtUseDate.Style.Add("display", "none");
            string _sGridPanelTitle = "Part Price Details";
            lblTtlSelection.Text = _sGridPanelTitle;
            CPESelection.CollapsedText = "Show " + _sGridPanelTitle;
            CPESelection.ExpandedText = "Hide " + _sGridPanelTitle;
            //Megha 12082011
        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            iDealerID = Location.iDealerId;
            FillDetails();
            FillSelectionGrid();

        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            iDealerID = Location.iDealerId;
            FillDetails();
            FillSelectionGrid();
        }
        private void FillDetails()
        {
            iDealerID = Location.iDealerId;
            clsDB objDB = new clsDB();
            try
            {
                DSPrice = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxPartPrice", GroupCode, iDealerID);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            // FillSelectionGrid();
            //iPartPriceID = Func.Convert.iConvertToInt(txtID.Text);
            if (DSPrice.Tables[0].Rows.Count > 0)
            {
                if (iPartPriceID == 0)
                    iPartPriceID = Func.Convert.iConvertToInt(DSPrice.Tables[0].Rows[0]["ID"]);
                if (iPartPriceID != 0)
                {
                    GetDataAndDisplay();
                }


                // FillGrid();
            }
            else
            {
                ClearDealerHeader();
            }


        }

        private void FillGrid()
        {
            try
            {
                clsCommon objclsComman = new clsCommon();
           //     SearchGrid.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", GroupCode, iDealerID, "PartPrice"));
                objclsComman = null;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        private void FillSelectionGrid()
        {
            try
            {
                _sModelPart = DealerOrigin;
                DealerId = Location.iDealerId;
                _sSqlFor = "PartPrice";
                clsCommon objclsComman = new clsCommon();
                //SearchGrid.bGridFillUsingSql = true;
                //SearchGrid.sGridPanelTitle = "Part Price Details";
                //SearchGrid.AddToSearchCombo("Part No");
                //SearchGrid.AddToSearchCombo("Part Name");
                //SearchGrid.AddToSearchCombo("Dealer Code");
                //SearchGrid.AddToSearchCombo("Dealer");
                //SearchGrid.iDealerID = iDealerID;
                //SearchGrid.sSqlFor = "PartPrice";
                if (iMenuId == 607 || iMenuId == 467)
                {
                   
                   //SearchGrid.sModelPart = "01";
                    _sModelPart = "01";
                    GroupCode = "01";
                }
                else if (iMenuId == 609)
                {
                    CntDealerHeaderDetails.Visible = true;
                   // SearchGrid.sModelPart = "99";
                    _sModelPart = "99";
                    GroupCode = "99";
                }
              
                //SearchGrid.iBrHODealerID = iHOBr_id;
                if (DealerId == 0)
                {
                    FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, DealerId, _sSqlFor));
                }
                else
                {
                    FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, DealerId.ToString(), _sSqlFor, _iBrHODealerID));
                }
                //Sujata 03032015_End
                objclsComman = null;

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

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                iPartPriceID = Func.Convert.iConvertToInt(txtID.Text);
                ViewState["PartID"] = iPartPriceID;
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
                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {

                    txtPartNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Part No"]);
                    txtPartName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Part Name"]);
                    txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer Code"]);
                    txtDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer"]);
                    txtEffectiveFrom.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["From Date"]);
                    txtEffectiveTo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["To Date"]);
                    txtLISTPRICE.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LIST"]);
                    txtMRP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MRP"]);
                    txtNDP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NDP"]);
                    txtActive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
               
                    //Megha 15/05/2013
                    txtCategory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Part_cat"]);
                    txtHSNCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HSN_Code"]);
                    txtDCSUpdateDate.Text = (ds.Tables[0].Rows[0]["Cr_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Cr_Date"]).ToString("dd/MM/yyyy HH:mm") : "";
                    txtXMLCreationDate.Text = (ds.Tables[0].Rows[0]["XML_Cr_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["XML_Cr_Date"]).ToString("dd/MM/yyyy HH:mm") : "";

                    
                    if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTTaxper"]) != "")
                    {
                        txtPer.Text = String.Format("{0:0.00}", Func.Convert.dConvertToDouble(ds.Tables[0].Rows[0]["GSTTaxper"]));
                    }
                    else
                    {
                        txtPer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTTaxper"]);
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
            txtPartNo.Text = "";
            txtPartName.Text = "";
            txtDealerCode.Text = "";
            txtDealer.Text = "";
            txtEffectiveFrom.Text = "";
            txtEffectiveTo.Text = "";
            txtLISTPRICE.Text = "";
            txtMRP.Text = "";
            txtNDP.Text = "";
            txtActive.Text = "";
            //Megha 15/05/2013
            txtDCSUpdateDate.Text = "";
            txtXMLCreationDate.Text = "";
            //Megha 15/05/2013
        }
        private void GetDataAndDisplay()
        {

            clsDB objDB = new clsDB();
            try
            {

                DataSet ds = new DataSet();
                //int iPartID = Func.Convert.iConvertToInt(txtID.Text);
                //iPartID = 1;
                iDealerID = Location.iDealerId;
                if (iPartPriceID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartPrice", GroupCode, iDealerID, iPartPriceID, "Max");
                    DisplayData(ds);

                }
                else
                {
                    ClearDealerHeader();
                    ds = null;
                    // lblConfirm.Text = "Record Does Not Exist !";
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            FillDetails();
        }
        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iDealerID = Location.iDealerId;
                FillDetails();
                FillSelectionGrid();

                //// SearchGrid.bIsCollapsable = false; 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //NEW MODEL POPUP CODE
        public void FillSelectionGrid(DataSet ds)
        {
            if (Func.Common.iTableCntOfDatatSet(ds) == 0)
            {
                // dsSrchgrid = null;
                BindDataToGrid(null);
            }
            else
            {
                dsSrchgrid = ds.Copy();
                BindDataToGrid(dsSrchgrid.Tables[0]);
            }
        }
        public void BindDataToGrid(DataTable dt)
        {
            try
            {
                lblSort.Visible = false;
                // drpSort.Visible = false;
                lblSorted.Visible = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    // iID = Func.Convert.iConvertToInt(dt.Rows[0]["ID"]);
                    lblSort.Visible = true;
                    lblSorted.Visible = true;
                    //drpSort.Visible = true;
                }
                SearchGrid1.DataSource = dt;
                SearchGrid1.DataBind();


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetDataAndBindToGrid();
        }
        private void GetDataAndBindToGrid()
        {
            try
            {
                lblConfirm.Text = "";
                if (hdnSort.Value == "Sort")
                {
                    DataView dvSearch = new DataView(dsSrchgrid.Tables[0]);
                    dvSearch.Sort = dsSrchgrid.Tables[0].Columns[2].ColumnName + " " + drpSort.SelectedValue;
                    BindDataToGrid(dvSearch.ToTable());
                    // bCallFromSearch = true;
                }
                if (txtSearch.Text != "" && (txtUseDate.Text == "No" || txtUseDate.Text.Trim() == ""))
                {
                    txtDocDateFrom.Text = "";
                    txtDocDateTo.Text = "";
                    divDateSearch.Style.Add("display", "none");
                    txtSearch.Style.Add("display", "");
                    //  bCallFromSearch = true;
                    SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString(), dsSrchgrid);
                }
                else if (txtSearch.Text != "" && txtUseDate.Text == "Yes")
                {
                    clsCommon objclsComman = new clsCommon();
                    //Sujata 04032015_Begin
                    //FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                    if (_iBrHODealerID == 0)
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, DealerId, _sSqlFor));
                    }
                    else
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, DealerId.ToString(), _sSqlFor, _iBrHODealerID));
                    }
                    btnClearSearch.Visible = true;
                    divDateSearch.Style.Add("display", "");
                    txtSearch.Style.Add("display", "none");
                    objclsComman = null;
                    //bCallFromSearch = true;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void drpSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dvSearch = new DataView(dsSrchgrid.Tables[0]);
                dvSearch.Sort = dsSrchgrid.Tables[0].Columns[2].ColumnName + " " + drpSort.SelectedValue;
                BindDataToGrid(dvSearch.ToTable());
                //   bCallFromSearch = true;
                hdnSort.Value = "Sort";

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SearchText(string strSearchText, string strSearchOnField, DataSet dsSrch)
        {
            try
            {
                lblConfirm.Text = "";
                DataView dv = new DataView(dsSrch.Tables[0]);
                DataTable dt = new DataTable();
                string SearchExpression = null;

                if (!String.IsNullOrEmpty(strSearchText))
                {
                    SearchExpression = string.Format("{0} '%{1}%'", SearchGrid1.SortExpression, strSearchText);

                }
                strSearchOnField = "[" + strSearchOnField + "]";
                dv.RowFilter = strSearchOnField + " like" + SearchExpression;
                dt = dv.ToTable();
                System.Threading.Thread.Sleep(2000);
                if (dv.Count > 0)
                {
                    // iID = Func.Convert.iConvertToInt(dt.Rows[0]["ID"]);
                    SearchGrid1.DataSource = dv;
                    SearchGrid1.DataBind();
                    //CreateImageToEachRow();
                    //HideColumn();
                    btnClearSearch.Visible = true;
                }
                else
                {
                    lblConfirm.Text = "Record Does Not Exist !";
                    //iID = 0;
                    //BindDataToGrid(dsSrch.Tables[0]);
                    //BindDataToGrid(null);
                    btnClearSearch.Visible = true;
                }
                //   HideColumn();
            }
            catch (Exception ex)
            {
                lblConfirm.Text = "Record Does Not Exist !";
                //BindDataToGrid(dsSrch.Tables[0]);
                //   iID = 0;
                BindDataToGrid(null);
                btnClearSearch.Visible = true;
            }
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {

            txtDocDateFrom.Text = "";
            txtDocDateTo.Text = "";
            lblConfirm.Text = "";
            divDateSearch.Style.Add("display", "none");
            txtSearch.Style.Add("display", "");
            txtSearch.Text = "";
            if (dsSrchgrid != null)                              //*Megha
            {
                BindDataToGrid(dsSrchgrid.Tables[0]);
            }                                                  //* 
            btnClearSearch.Visible = false;
        }
        protected void btnedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                clsDB objDB = new clsDB();
                DataSet ds = new DataSet();
                GridViewRow clickedrow = ((ImageButton)sender).NamingContainer as GridViewRow;
                iPartPriceID = Convert.ToInt32(SearchGrid1.DataKeys[clickedrow.RowIndex].Value);
                if (iPartPriceID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartPrice", GroupCode, iDealerID, iPartPriceID, "Max");
                    DisplayData(ds);

                }
            }
            catch (Exception)
            {
            }
        }
        protected void Btnshowhistory_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                GridViewRow clickedrow = ((ImageButton)sender).NamingContainer as GridViewRow;
                int iPartID = Convert.ToInt32(SearchGrid1.DataKeys[clickedrow.RowIndex].Value);
                Fillmodelpopupdetails(iPartID);
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

                    if (_sModelPart == "01" || _sModelPart == "02")
                    {
                        if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                        {

                            dsDetails = objDB.ExecuteQueryAndGetDataset("Select Distinct Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.Part_ID=" + iPartID + " And M_PartRateMaster.Dealer_ID=" + DealerId);
                        }
                        else
                            dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.ID=" + iPartID);
                    }
                    if (_sModelPart == "99")
                    {
                        if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                        {
                            
                            dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.Part_ID=" + iPartID + " And M_LocalPartRate.Dealer_ID=" + DealerId);
                        }
                        else
                            dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.ID=" + iPartID);

                    }

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
                        if (Func.Convert.sConvertToString(Request.QueryString["Distributor"]) == "Y")
                        {
                            PartPriceGrid.Columns[0].Visible = false;
                        }
                        else
                        {
                            PartPriceGrid.Columns[0].Visible = true;
                        }
                        PartPriceGrid.DataSource = dtDetails;
                        ViewState["partPrice"] = dtDetails;
                        PartPriceGrid.DataBind();
                    }
                ModalPopupExtender2.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void PartPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PartPriceGrid.DataSource = (DataTable)ViewState["partPrice"];
                PartPriceGrid.PageIndex = e.NewPageIndex;
                PartPriceGrid.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void SearchGrid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SearchGrid1.PageIndex = e.NewPageIndex;
                SearchGrid1.DataBind();
                FillSelectionGrid();
            }
            catch (Exception)
            {
                             
            }
        }
    }
}