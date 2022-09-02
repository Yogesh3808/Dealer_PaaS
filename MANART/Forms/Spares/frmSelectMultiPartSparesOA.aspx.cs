using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Data;
using AjaxControlToolkit;
using System.Drawing;
using System.IO;

namespace MANART.Forms.Spares
{
    public partial class frmSelectMultiPartSparesOA : System.Web.UI.Page
    {
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;

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
                dtDetails = (DataTable)Session["PartDetails"];
                idataCount = dtDetails.Rows.Count;


                dtTemp = dtDetails.Clone();
                dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                // dtDetails.Rows.RemoveAt(idataCount - 1);



                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    ////SELECT 1 as SRNo,0 as ID,0 as Part_ID, '' as  part_no,'' as group_code ,
                    ////'' as Part_Name,1 as Qty, 1.00 as MRPRate,0 as bal_qty,0 as discount_per,0 as discount_amt,
                    ////0 as disc_rate,1.00 as Total, '00' as fin_no ,'N' as Status    

                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("bal_qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));
                }

                //Sujata 08012011
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
                        //SRNo,ID,Part_ID,part_no,Part_Name,Qty,group_code,bal_qty,MRPRate,discount_per,disc_rate,discount_amt,Total,Status
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[4]);

                        dr["Qty"] = 0.00;
                        dr["Unit"] = Func.Convert.sConvertToString(myArray[12]).Trim();
                        dr["Price"] = Func.Convert.sConvertToString(myArray[13]);

                        dr["group_code"] = Func.Convert.sConvertToString(myArray[5]);
                        dr["bal_qty"] = Func.Convert.dConvertToDouble(myArray[7]);

                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["discount_per"] = Func.Convert.dConvertToDouble(myArray[10]);
                        dr["disc_rate"] = 0.00;
                        dr["discount_amt"] = 0.00;
                        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[8]);
                        dr["Total"] = Convert.ToDouble(0);
                        dr["Status"] = "U";
                        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[9].Trim());
                        dr["BFRGST"] = Func.Convert.sConvertToString(myArray[14].Trim());
                        dr["BFRGST_Stock"] = Func.Convert.dConvertToDouble(myArray[15]);


                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                //dtDetails.ImportRow(dtTemp.Rows[0]);

                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");
                Session["PartDetails"] = dtDetails;

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
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                int iDealerId;
                string sSelectedPartID = "";
                int iHOBrID = 0, iCustID = 0;
                string sDocGST = "", sCustTaxTag = "", sGSTOAType="";
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                iCustID = Func.Convert.iConvertToInt(Request.QueryString["sCustID"]);
                sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"]);
                sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);
                sGSTOAType = Func.Convert.sConvertToString(Request.QueryString["GSTOAType"]);
                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }

                object[] ParaValues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustID, iHOBrID, sDocGST, sCustTaxTag, sGSTOAType };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_OAPartDetailsWithMRPRate_GetPaging", ParaValues);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_OAPartDetailsWithMRPRate_GetPaging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustID, iHOBrID, sDocGST, sCustTaxTag);


                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][5]);
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
    }
}