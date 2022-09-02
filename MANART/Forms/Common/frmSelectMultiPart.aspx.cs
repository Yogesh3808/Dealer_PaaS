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
    public partial class frmSelectMultiPart : System.Web.UI.Page
    {
        int iTotalCnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();


                if (!IsPostBack)
                {

                    lblTitle.Text = "Part Master";
                    bindGrid("A", "x");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            PagerV2_1.CurrentIndex = currnetPageIndx;
            bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
        }

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        private void bindGrid(string sSelect, string sSearchPart)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                int iDealerId;
                string sSelectedPartID;
                string RepairOrderDate = "";
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                RepairOrderDate = Func.Convert.sConvertToString(Request.QueryString["RepairOrderDate"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();

                //Sujata 24022011        
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                //Sujata 24022011

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }

                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("xyzTmpTable", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID);
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDP_FOB_Rate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, RepairOrderDate, sSelectedPartID);
                if (dsSrchgrid == null)
                {
                    //Sujata 24022011
                    lblNMsg.Visible = true;
                    //Sujata 24022011
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][6]);
                    PagerV2_1.ItemCount = iTotalCnt;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts();
                    //Sujata 24022011
                    lblNMsg.Visible = false;
                    //Sujata 24022011
                }
                else
                {
                    //Sujata 24022011
                    lblNMsg.Visible = true;
                    PagerV2_1.ItemCount = 0;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts();
                    //Sujata 24022011
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

        private void ChkSelectedParts()
        {
            CheckBox chk = new CheckBox();
            string sPartID = "";
            string str = null;
            string[] strArr = null;
            str = txtPartIds.Text;
            if (str == "") return;
            strArr = str.Split('#');
            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;

                for (int j = 0; j < strArr.Length; j++)
                {
                    if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                    {
                        chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                        chk.Checked = true;
                        break;
                    }
                }
            }
        }

        private string GetIdFromString(string strArr)
        {
            string sID = "";
            char[] strToParse = strArr.ToCharArray();
            // convert string to array of chars    
            char ch; int charpos = 0;
            ch = strToParse[charpos];
            while (ch != ' ')
            {
                sID = sID + ch;
                charpos++;
                ch = strToParse[charpos];

            }
            return sID;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PagerV2_1.ItemCount = 10;
                PagerV2_1.CurrentIndex = 0;
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
                PartDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }    
    }
}