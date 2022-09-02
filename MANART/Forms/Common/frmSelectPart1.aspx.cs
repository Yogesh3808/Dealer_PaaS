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

namespace MANART.Forms.Common
{
    public partial class frmSelectPart : System.Web.UI.Page
    {
        DataSet dsSrchgrid;
        private string sDealerId;
        private string sSelectedPartID;
        int iDealerId;
        private string sSuperceded;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                DataSet dsSrchgrid;

                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                sSuperceded = Request.QueryString["Superceded"].ToString();

                if (!IsPostBack)
                {
                    lblTitle.Text = "Part Master";
                    DisplayData();
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {

            }
        }
        private void DisplayData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            DataView dvSrchgrid = null;
            try
            {
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails", iDealerId, sSelectedPartID, sSuperceded);
                if (dsSrchgrid == null)
                {
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    dvSrchgrid = dsSrchgrid.Tables[0].DefaultView;
                    string sSearchValue = "";
                    if (txtSearch.Text != "")
                    {
                        sSearchValue = DdlSelctionCriteria.SelectedValue + "='" + txtSearch.Text + "'";
                        dvSrchgrid.RowFilter = sSearchValue;
                    }
                    if (Request.QueryString["Superceded"].ToString() == "Y")
                    {
                        PartDetailsGrid.Columns[4].Visible = false;
                        PartDetailsGrid.Columns[5].Visible = false;
                    }
                    
                    PartDetailsGrid.DataSource = dvSrchgrid.ToTable();
                    PartDetailsGrid.DataBind();
                }
                else
                {
                    return;
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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }


        protected void PartDetailsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PartDetailsGrid.PageIndex = e.NewPageIndex;
            DisplayData();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
    }
}