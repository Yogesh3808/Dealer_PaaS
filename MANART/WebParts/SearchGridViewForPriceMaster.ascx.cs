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

namespace MANART.WebParts
{
    public partial class SearchGridViewForPriceMaster : System.Web.UI.UserControl
    {
        private DataSet dsSrchgrid;
        private bool _bIsCallForServer = false;
        private int _iDealerID = 0;
        //Sujata 03032015
        private int _iBrHODealerID = 0;
        //Sujata 03032015
        private int _iID = 0;
        private string _sModelPart = "";
        private string _sSqlFor = "";
        private string _sGridPanelTitle = "";
        private bool bCallFromSearch = false;
        private bool bIsSetAtPageLoad = false;
        private bool _bIsCollapsable = true;
        public bool bGridFillUsingSql = false;
        protected string _SGridName = "SearchGrid";

        DataTable dsPriceDetails = new DataTable();
        //public bool bSetGridToCenter
        public bool bIsCollapsable
        {
            get { return _bIsCollapsable; }
            set { _bIsCollapsable = value; }
        }
        public int iDealerID
        {
            get { return _iDealerID; }
            set { _iDealerID = value; }
        }
        //Sujata 03032015_Begin
        public int iBrHODealerID
        {
            get { return _iBrHODealerID; }
            set { _iBrHODealerID = value; }
        }
        //Sujata 03032015_End
        public int iID
        {
            get { return _iID; }
            set { _iID = value; }
        }
        public string sGridPanelTitle
        {
            get { return _sGridPanelTitle; }
            set
            {
                _sGridPanelTitle = value;
            }
        }
        public string sModelPart
        {
            get { return _sModelPart; }
            set { _sModelPart = value; }
        }
        public string sSqlFor
        {
            get { return _sSqlFor; }
            set { _sSqlFor = value; }
        }

        public bool bIsCallForServer
        {
            get { return _bIsCallForServer; }
            set { _bIsCallForServer = value; }
        }
        public string sGridName
        {
            get { return _SGridName; }
            set
            {
                _SGridName = value;
            }
        }
        public delegate void ImageClick(object sender, ImageClickEventArgs e);

        public event ImageClick Image_Click;


        protected void Image1_Click(object sender, ImageClickEventArgs e)
        {
            Image_Click(sender, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (dsSrchgrid != null)
                {
                    BindDataToGrid(dsSrchgrid.Tables[0]);
                }


                //  DdlSelctionCriteria.Attributes.Add("onchange", "return SetDateFields();");
                DdlSelctionCriteria.Attributes.Add("onchange", "return SetDateFields('" + _SGridName + "');");

                //txtDocDateTo.bCheckforCurrentDate = false;
                //txtDocDateFrom.bCheckforCurrentDate = false;
                txtUseDate.Style.Add("display", "none");
                divDateSearch.Style.Add("display", "none");

                lblTtlSelection.Text = _sGridPanelTitle;
                CPESelection.CollapsedText = "Show " + _sGridPanelTitle;
                CPESelection.ExpandedText = "Hide " + _sGridPanelTitle;
                if (txtDocDateFrom.Text.Trim() != "")
                {
                    divDateSearch.Style.Add("display", "none");
                }
                if (_iDealerID != 0 && bCallFromSearch == false)
                {
                    bIsSetAtPageLoad = true;
                    clsCommon objclsComman = new clsCommon();
                    //Sujata 03032015_Begin
                    //FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                    if (_iBrHODealerID == 0)
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                    }
                    else
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID.ToString(), _sSqlFor, _iBrHODealerID));
                    }
                    //Sujata 03032015_End
                    objclsComman = null;
                }

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {

            if (txtSearch.Text != "" || hdnSort.Value != "N")
            {
                GetDataAndBindToGrid();
                return;
            }
            if (_bIsCollapsable == false) CPESelection.Collapsed = false;
            else CPESelection.Collapsed = true;
            //if (bIsSetAtPageLoad == true) return;
            if (bGridFillUsingSql == true) return;
            base.OnPreRender(e);
            if (bCallFromSearch == false)
            {
                clsCommon objclsComman = new clsCommon();
                //Sujata 04032015_Begin
                //FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                if (_iBrHODealerID == 0)
                {
                    FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                }
                else
                {
                    FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID.ToString(), _sSqlFor, _iBrHODealerID));
                }
                //Sujata 04032015_End
                objclsComman = null;
            }
        }
        public void FillSelectionGrid(DataSet ds)
        {
            if (Func.Common.iTableCntOfDatatSet(ds) == 0)
            {
                dsSrchgrid = null;
                BindDataToGrid(null);
            }
            else
            {
                dsSrchgrid = ds.Copy();
                BindDataToGrid(dsSrchgrid.Tables[0]);
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
                    SearchExpression = string.Format("{0} '{1}*'", SelectionGrid.SortExpression, strSearchText);

                }
                strSearchOnField = "[" + strSearchOnField + "]";
                dv.RowFilter = strSearchOnField + " like" + SearchExpression;
                dt = dv.ToTable();
                System.Threading.Thread.Sleep(2000);
                if (dv.Count > 0)
                {
                    iID = Func.Convert.iConvertToInt(dt.Rows[0]["ID"]);
                    SelectionGrid.DataSource = dv;
                    SelectionGrid.DataBind();
                    CreateImageToEachRow();
                    HideColumn();
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
                HideColumn();
            }
            catch (Exception ex)
            {
                lblConfirm.Text = "Record Does Not Exist !";
                //BindDataToGrid(dsSrch.Tables[0]);
                iID = 0;
                BindDataToGrid(null);
                btnClearSearch.Visible = true;
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
                    bCallFromSearch = true;
                }
                if (txtSearch.Text != "" && (txtUseDate.Text == "No" || txtUseDate.Text.Trim() == ""))
                {
                    txtDocDateFrom.Text = "";
                    txtDocDateTo.Text = "";
                    divDateSearch.Style.Add("display", "none");
                    txtSearch.Style.Add("display", "");
                    bCallFromSearch = true;
                    SearchText(txtSearch.Text, DdlSelctionCriteria.SelectedValue.ToString(), dsSrchgrid);
                }
                else if (txtSearch.Text != "" && txtUseDate.Text == "Yes")
                {
                    clsCommon objclsComman = new clsCommon();
                    //Sujata 04032015_Begin
                    //FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                    if (_iBrHODealerID == 0)
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID, _sSqlFor));
                    }
                    else
                    {
                        FillSelectionGrid(objclsComman.GetSqlToFillSelectionGrid(txtDocDateFrom.Text, txtDocDateTo.Text, _sModelPart, _iDealerID.ToString(), _sSqlFor, _iBrHODealerID));
                    }
                    //Sujata 04032015_End
                    btnClearSearch.Visible = true;
                    divDateSearch.Style.Add("display", "");
                    txtSearch.Style.Add("display", "none");
                    objclsComman = null;
                    bCallFromSearch = true;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        /// <summary>
        /// Method Bind Grid Column to Combo Box For Search 
        /// </summary>
        public void AddToSearchCombo(string sName)
        {
            if (sName != "")
            {
                if (DdlSelctionCriteria.Items.FindByText(sName) == null)
                {
                    DdlSelctionCriteria.Items.Add(sName);
                }
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

        protected void SelectionGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //SelectionGrid.DataSource = dsSrchgrid.Tables[0];
            //Session["PriceRateDetails"] = dsSrchgrid.Tables[0];
            SelectionGrid.PageIndex = e.NewPageIndex;
            SelectionGrid.DataBind();
            CreateImageToEachRow();
            HideColumn();
        }
        public void BindDataToGrid(DataTable dt)
        {
            try
            {
                lblSort.Visible = false;
                drpSort.Visible = false;
                lblSorted.Visible = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    iID = Func.Convert.iConvertToInt(dt.Rows[0]["ID"]);
                    lblSort.Visible = true;
                    lblSorted.Visible = true;
                    drpSort.Visible = true;
                }
                Session["PriceRateDetails"] = dt;
                SelectionGrid.DataSource = dt;
                SelectionGrid.DataBind();
                CreateImageToEachRow();
                HideColumn();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void CreateImageToEachRow()
        {
            ImageButton img1 = null;

            if (SelectionGrid.Rows.Count != 0)
            {
                if (_bIsCallForServer == false)
                {
                    HtmlImage img = null;
                    for (int i = 0; i < SelectionGrid.Rows.Count; i++)
                    {
                        img = new HtmlImage();
                        img.Src = "~/Images/arrowRight.png";
                        img.Attributes["runat"] = "server";
                        img.Attributes["onclick"] = "return onGridViewRowSelected(this);";
                        img.Attributes.Add("onmouseover", "return SetCancelStyleonMouseOver(this);");
                        img.Attributes.Add("onmouseout", "return SetCancelStyleOnMouseOut(this);");
                        SelectionGrid.Rows[i].Cells[0].Controls.Add(img);

                        img1 = new ImageButton();
                        img1.ImageUrl = "~/Images/History.png";
                        img1.Attributes["runat"] = "server";

                        img1.Attributes["onclick"] = "return ShowPriceWiseDealarHistory(this)";
                        SelectionGrid.Rows[i].Cells[13].Controls.Add(img1);
                    }
                }
                else
                {
                    ImageButton img = null;

                    for (int i = 0; i < SelectionGrid.Rows.Count; i++)
                    {
                        img = new ImageButton();
                        img.ID = i.ToString();
                        img.ImageUrl = "~/Images/arrowRight.png";
                        img.Attributes["runat"] = "server";
                        img.Attributes["AutoPostBack"] = "true";
                        img.Attributes.Add("onClick", "return onGridViewRowSelected(this);");
                        img.Attributes.Add("onmouseover", "return SetCancelStyleonMouseOver(this);");
                        img.Attributes.Add("onmouseout", "return SetCancelStyleOnMouseOut(this);");

                        //img.Attributes["onClientclick"] = "return onGridViewRowSelected(this);";                    
                        img.Click += new ImageClickEventHandler(Image1_Click);
                        SelectionGrid.Rows[i].Cells[0].Controls.Add(img);

                        img1 = new ImageButton();
                        img1.ImageUrl = "~/Images/History.png";
                        img1.Attributes["runat"] = "server";

                        img1.Attributes["onclick"] = "return ShowPriceWiseDealarHistory(this)";
                        SelectionGrid.Rows[i].Cells[13].Controls.Add(img1);
                    }

                }
            }
            CPESelection.Collapsed = false;
            //SelectionGrid.PageIndex = 0;
        }
        private void HideColumn()
        {
            //  To Hide the ID and RecordUsed Colummn
            if (SelectionGrid.Rows.Count != 0)
            {
                SelectionGrid.HeaderRow.Cells[1].Style.Add("display", "none");
                SelectionGrid.HeaderRow.Cells[2].Style.Add("display", "none");
                if (SelectionGrid.HeaderRow.Cells[3].Text == "Model Code")
                {
                    //SelectionGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                    SelectionGrid.HeaderRow.Cells[12].Style.Add("display", "none");
                }
                for (int i = 0; i < SelectionGrid.Rows.Count; i++)
                {
                    SelectionGrid.Rows[i].Cells[1].Style.Add("display", "none");
                    SelectionGrid.Rows[i].Cells[2].Style.Add("display", "none");
                    if (SelectionGrid.HeaderRow.Cells[3].Text == "Model Code")
                    {
                        //Megha 30/11/2011 --Hide Eff.To date column
                        SelectionGrid.HeaderRow.Cells[8].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[8].Style.Add("display", "none");
                        //Megha 30/11/2011
                        //Megha 30/11/2011 --Hide Eff.To date column
                        SelectionGrid.HeaderRow.Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[11].Style.Add("display", "none");
                        //Megha 30/11/2011

                        SelectionGrid.Rows[i].Cells[12].Style.Add("display", "none");
                    }
                    if (SelectionGrid.HeaderRow.Cells[3].Text == "Lubricant Type")
                    {
                        //SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[8].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[10].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[8].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[10].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[13].Width = 150;
                    }
                    if (SelectionGrid.HeaderRow.Cells[3].Text == "Service Name")
                    {
                        //SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[4].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[10].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[11].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[4].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[10].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[13].Width = 150;
                    }
                    if (SelectionGrid.HeaderRow.Cells[13].Text == "Labor Rate History")
                    {
                        //SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[10].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[11].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[10].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[13].Width = 150;
                    }
                    if (SelectionGrid.HeaderRow.Cells[13].Text == "Labor Rate History")
                    {
                        //SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[9].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[10].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[11].Style.Add("display", "none");
                        SelectionGrid.HeaderRow.Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[9].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[10].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[11].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[12].Style.Add("display", "none");
                        SelectionGrid.Rows[i].Cells[13].Width = 150;
                    }


                }
            }

        }

        protected void DdlSelctionCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void drpSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dvSearch = new DataView(dsSrchgrid.Tables[0]);
                dvSearch.Sort = dsSrchgrid.Tables[0].Columns[2].ColumnName + " " + drpSort.SelectedValue;
                BindDataToGrid(dvSearch.ToTable());
                bCallFromSearch = true;
                hdnSort.Value = "Sort";

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

    }
}