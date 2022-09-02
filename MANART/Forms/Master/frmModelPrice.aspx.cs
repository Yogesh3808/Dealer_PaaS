using System;
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
    public partial class frmModelPrice : System.Web.UI.Page
    {
        private int iPartPriceID = 0;
        private int Dealer_ED = 0;
        private int DealerId = 0;
        private string DealerOrigin;
        private int _iBrHODealerID = 0;
        private string _sModelPart = "";
        private string _sSqlFor = "";
        private DataSet dsSrchgrid;
    
        DataSet DSPrice = new DataSet();
        clsDB objDB = new clsDB();
        DataTable dtDetails = null;
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                string[] url1 = HttpContext.Current.Request.Url.AbsoluteUri.Split('=');
                if (url1.Length > 1)
                    if (Convert.ToInt32(url1[1]) == 468)//For Export Model Price Add new Menu and check with OR Condition like Domestic
                    {
                        DealerOrigin = "E";
                    }
                    else if (Convert.ToInt32(url1[1]) == 467 || Convert.ToInt32(url1[1]) == 287 || Convert.ToInt32(url1[1]) == 615)
                    {
                        DealerOrigin = "D";
                    }

                DealerId = Location.iDealerId;
                FillSelectionGrid();
                //ExpirePageCache();
                ExpirePageCache();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected override void OnPreRender(EventArgs e)
        {

            try
            {
                base.OnPreRender(e);

                DealerId = Location.iDealerId;
                FillDetails();
                FillSelectionGrid();
                string strDisAbleBackButton;
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                btnReadonly();
                if (txtSearch.Text != "" || hdnSort.Value != "N")
                {
                    GetDataAndBindToGrid();
                    return;
                }
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
            Location.bUseSpareDealerCode = false;          
            txtUseDate.Style.Add("display", "none");
            string _sGridPanelTitle = "Model Price Details";
            lblTtlSelection.Text = _sGridPanelTitle;          
            CPESelection.CollapsedText = "Show " + _sGridPanelTitle;
            CPESelection.ExpandedText = "Hide " + _sGridPanelTitle;
            //Megha 12082011
        }

        private void FillDetails()
        {
            DealerId = Location.iDealerId;

            clsDB objDB = new clsDB();
            try
            {
                DSPrice = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxModelPrice", DealerOrigin, DealerId);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

            // FillSelectionGrid();
           // iPartPriceID = Func.Convert.iConvertToInt(txtID.Text);
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
              //  SearchGrid1.FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid("", "", DealerOrigin, DealerId, "PartPrice"));
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
                    _sSqlFor = "ModelPrice";
                     clsCommon objclsComman = new clsCommon();
                    //Sujata 03032015_Begin
                    //FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
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
                
                //SearchGrid.bGridFillUsingSql = true;
              //  SearchGrid1.sGridPanelTitle = "Model Price Details";
              //  SearchGrid1.AddToSearchCombo("Model Code");
              //  //SearchGrid.AddToSearchCombo("Model Name");
              //  //SearchGrid.AddToSearchCombo("Dealer Code");
              //  SearchGrid1.AddToSearchCombo("Dealer");
              //  //SearchGrid.AddToSearchCombo("From Date");
              //  //SearchGrid.AddToSearchCombo("To Date");           
              //  SearchGrid1.AddToSearchCombo("MRP");
              //  SearchGrid1.AddToSearchCombo("NDP");
              //  SearchGrid1.AddToSearchCombo("NDP");
              ////  SearchGrid.AddToSearchCombo("Depot Code");
              //  SearchGrid1.sModelPart = DealerOrigin;
              //  SearchGrid1.iDealerID = Location.iDealerId;
              //  SearchGrid1.sSqlFor = "ModelPrice";

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
                DealerId = Location.iDealerId;
                FillSelectionGrid();
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

                    txtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model Code"]);
                    txtModelName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model Name"]);
                    txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer Code"]);
                    txtDealer.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer"]);
                    txtEffectiveFrom.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["From Date"]);
                    txtEffectiveTo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["To Date"]);
                    txtDepoCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Depo Code"]);
                    txtMRP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MRP"]);
                    txtNDP.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NDP"]);
                    txtActive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    //Megha 11/06/2013
                    txtHsncode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HSN_Code"]);
                    txtDCSUpdateDate.Text = (ds.Tables[0].Rows[0]["Cr_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Cr_Date"]).ToString("dd/MM/yyyy HH:mm") : "";
                    txtXMLCreationDate.Text = (ds.Tables[0].Rows[0]["XML_Cr_Date"].ToString() != "") ? Convert.ToDateTime(ds.Tables[0].Rows[0]["XML_Cr_Date"]).ToString("dd/MM/yyyy HH:mm") : "";
                    //Megha 11/06/2013

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
            txtModelCode.Text = "";
            txtModelName.Text = "";
            txtDealerCode.Text = "";
            txtDealer.Text = "";
            txtEffectiveFrom.Text = "";
            txtEffectiveTo.Text = "";
            txtDepoCode.Text = "";
            txtMRP.Text = "";
            txtNDP.Text = "";
            txtActive.Text = "";
            //Megha 11/06/2013
            txtDCSUpdateDate.Text = "";
            txtXMLCreationDate.Text = "";
            //Megha 11/06/2013
        }
        private void GetDataAndDisplay()
        {
            clsDB objDB = new clsDB();
            try
            {

                DataSet ds = new DataSet();
                //int iPartID = Func.Convert.iConvertToInt(txtID.Text);
                //iPartID = 1;
                DealerId = Location.iDealerId;
                if (iPartPriceID != 0)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetModelPrice", DealerOrigin, DealerId, iPartPriceID, "Max");
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
                DealerId = Location.iDealerId;
                FillDetails();
                FillSelectionGrid();

                //// SearchGrid.bIsCollapsable = false; 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            DealerId = Location.iDealerId;
            FillDetails();
            FillSelectionGrid();
        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            DealerId = 0;
            DealerId = Location.iDealerId;
            FillDetails();
            FillSelectionGrid();
        }

        // NEW MODEL POPUP COAD

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
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetModelPrice", DealerOrigin, DealerId, iPartPriceID, "Max");
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
                
                throw;
            }
        }
        private void Fillmodelpopupdetails(int iPartID)
        {
            try
            {
                       ViewState["partPrice"] = null;
                       DataSet dsDetails = objDB.ExecuteQueryAndGetDataset("Select Model_ID,Dealer_ID from M_ModelRate where ID=" + iPartID);
                        int Model_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Model_ID"].ToString());
                        int Dealer_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Dealer_ID"].ToString());
                        dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetModelPrice", "E", Dealer_ID, Model_ID, "All");
                        if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                        {

                            ModelPriceGrid.DataSource = null;
                            ModelPriceGrid.DataBind();
                        }
                        else
                        {

                            ModelPriceGrid.DataSource = dtDetails;
                            ViewState["partPrice"] = dtDetails;
                            ModelPriceGrid.DataBind();
                        }
                        ModalPopupExtender2.Show();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        protected void ModelPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ModelPriceGrid.DataSource = (DataTable)ViewState["partPrice"];
                ModelPriceGrid.PageIndex = e.NewPageIndex;
                ModelPriceGrid.DataBind();
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

                throw;
            }
        }

    }
}