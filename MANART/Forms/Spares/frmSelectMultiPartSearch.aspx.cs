using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Spares
{
    public partial class frmSelectMultiPartSearch : System.Web.UI.Page
    {
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        string sSourchFrom = "";
        int iUserID;
        string sTransFrom = String.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                btnBack.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnBack, null) + ";");

                if (!IsPostBack)
                {
                    lblTitle.Text = "Part Master";
                }

                iUserID = Func.Convert.iConvertToInt(Session["UserID"]);
                sTransFrom = Func.Convert.sConvertToString(Request.QueryString["TransFrom"]);
                sSourchFrom = Func.Convert.sConvertToString(Request.QueryString["SourchFrom"]);
                bindGrid("S", "x");
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                int idataCount = 0;
                string[] asd;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                dtDetails = (DataTable)Session[sSourchFrom];
                idataCount = dtDetails.Rows.Count;


                dtTemp = dtDetails.Clone();
                //dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                //dtDetails.Rows.RemoveAt(idataCount - 1);

                // Get first row from table dtDetails,Previously it was last row which is above commented by Shyamal,Changed by Shyamal on 11062012
                if (dtDetails.Rows.Count != 0)
                {
                    dtTemp.ImportRow(dtDetails.Rows[0]);
                    dtDetails.Rows.RemoveAt(0);
                }

                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Physical_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Inward_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Outward_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Reason", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));
                }

                if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds.Text);

                    // string[] asd = myString.ToString().Split('#');
                    asd = myString.ToString().Split('#');
                    int iRowCnt = 0;
                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        dr = dtDetails.NewRow();
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["Part_ID"] = Func.Convert.iConvertToInt(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["Group_code"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Qty"] = Func.Convert.dConvertToDouble(myArray[4]);
                        dr["MOQ"] = Func.Convert.dConvertToDouble(myArray[5]);
                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[6]);
                        dr["Total"] = 0.00;
                        dr["Status"] = "U";
                        dr["Physical_Qty"] = 0.00;
                        dr["Inward_Qty"] = 0.00;
                        dr["Outward_Qty"] = 0.00;
                        dr["Reason"] = "";
                        dr["BFRGST"] = Func.Convert.sConvertToString(myArray[8].Trim());
                        dr["BFRGST_Stock"] = Func.Convert.dConvertToDouble(myArray[9]);
                        
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                //No need to Add row again,Modified by Shyamal 11062012
                //dtDetails.ImportRow(dtTemp.Rows[0]);
                if (dtDetails.Rows.Count == 0 && dtTemp.Rows.Count != 0)
                    dtDetails.ImportRow(dtTemp.Rows[0]);
                Session[sSourchFrom] = dtDetails;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PagerV2_1.ItemCount = 10;
                PagerV2_1.CurrentIndex = 0;
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        public void bindGrid(string sSelect, string sSearchPart)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                int iDealerId;
                int iSupplierID;
                string sSelectedPartID = "";
                string sGSTType = "";
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                //Sujata 24022011
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iSupplierID = Func.Convert.iConvertToInt(Request.QueryString["SupplierID"]);
                sSelectedPartID = Func.Convert.sConvertToString(Request.QueryString["SelectedPartID"]);
                sGSTType = Func.Convert.sConvertToString(Request.QueryString["GSTType"]);

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_PartDetailsForStockAdjustment_GetPaging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, sGSTType);

                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_PartDetailsWithMRPRate_GETPaging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, iSupplierID, iUserID, sTransFrom, sSelectedPartID);

                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][16]);
                    PagerV2_1.ItemCount = iTotalCnt;
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts();
                    lblNMsg.Visible = false;
                }
                else
                {
                    lblNMsg.Visible = true;
                    PagerV2_1.ItemCount = 0;
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
                    string[] srtArrPartID = strArr[j].Split(new string[] { "<--" }, StringSplitOptions.None);
                    for (int K = 0; K < srtArrPartID.Length; K++)
                        if (sPartID == Func.Convert.sConvertToString(srtArrPartID[K]))
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
    }
}