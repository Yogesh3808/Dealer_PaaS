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
    public partial class frmSelMultiPartWithInv : System.Web.UI.Page
    {
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        int iUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                btnBack.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnBack, null) + ";");
                if (!IsPostBack)
                {
                    lblTitle.Text = "Invoice Details And Part Master";

                }
                iUserID = Func.Convert.iConvertToInt(Session["UserID"]);
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
                dtDetails = (DataTable)Session["SalesRetPart"];
                idataCount = dtDetails.Rows.Count;


                dtTemp = dtDetails.Clone();
                //dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                //dtDetails.Rows.RemoveAt(idataCount - 1);

                // Get first row from table dtDetails,Previously it was last row which is above commented by Shyamal,Changed by Shyamal on 11062012
                dtTemp.ImportRow(dtDetails.Rows[0]);
                dtDetails.Rows.RemoveAt(0);



                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Invoice_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Invoice_Qty", typeof(double)));

                    dtDetails.Columns.Add(new DataColumn("Ret_Qty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Stock_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_amt", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("lab_tag", typeof(String)));
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
                        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["Invoice_No"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Rate"] = Func.Convert.dConvertToDouble(myArray[4]);
                        dr["Stock_Qty"] = Func.Convert.dConvertToDouble(myArray[8]);
                        dr["group_code"] = Func.Convert.sConvertToString(myArray[6]);
                        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[9]);
                        dr["Invoice_Qty"] = Func.Convert.dConvertToDouble(myArray[5]);
                        dr["Ret_Qty"] = Func.Convert.dConvertToDouble(myArray[5]);
                        dr["disc_per"] = Func.Convert.dConvertToDouble(myArray[10]);
                        dr["disc_rate"] = Func.Convert.dConvertToDouble(myArray[12]);
                        dr["disc_amt"] = Func.Convert.dConvertToDouble(myArray[11]);
                        // dr["Total"] = 0.00;//Convert.ToDouble(Func.Convert.sConvertToString(myArray[1])) * Convert.ToDouble(Func.Convert.sConvertToString(myArray[2]));
                        dr["Total"] = Func.Convert.dConvertToDouble(myArray[13]);
                        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[14]).Trim();
                        dr["Unit"] = Func.Convert.sConvertToString(myArray[15].Trim());
                        dr["Price"] = Func.Convert.dConvertToDouble(myArray[16]);

                        dr["lab_tag"] = "N";
                        dr["Status"] = "U";
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                //dtDetails.ImportRow(dtTemp.Rows[0]);
                if (dtDetails.Rows.Count == 0)
                    dtDetails.ImportRow(dtTemp.Rows[0]);
                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");
                Session["SalesRetPart"] = dtDetails;

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
                string sSelectedPartID = "";
                string sSelPartIDInvNoPONo = "";
                int iHOBrID = 0;
                int iCustId = 0;
                string sDocGST = "", sCustTaxTag = "";
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                iCustId = Func.Convert.iConvertToInt(Request.QueryString["CustID"].ToString());
                sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"]);
                sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);
               
                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }

                object[] ParaValues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustId, iHOBrID, sDocGST, sCustTaxTag };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartsAndInvFor_SRN_Paging", ParaValues);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartsAndInvFor_SRN_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustId, iHOBrID);
                
                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    //PartDetailsGrid.DataSource = dsSrchgrid;
                    //PartDetailsGrid.DataBind();

                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][6]);
                    PagerV2_1.ItemCount = iTotalCnt;
                    //if (sAuto == "Y")
                    //{
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    ChkSelectedParts();
                    //}
                    //else
                    //{
                    //    PartDetailsGrid1.DataSource = dsSrchgrid;
                    //    PartDetailsGrid1.DataBind();
                    //    ChkSelectedParts1();
                    //}

                    lblNMsg.Visible = false;
                }
                else
                {
                    lblNMsg.Visible = true;
                    PagerV2_1.ItemCount = 0;
                    //if (sAuto == "Y")
                    //{
                    PartDetailsGrid.DataSource = dsSrchgrid;
                    PartDetailsGrid.DataBind();
                    //}
                    //else
                    //{
                    //    PartDetailsGrid1.DataSource = dsSrchgrid;
                    //    PartDetailsGrid1.DataBind();
                    //}
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
            string sInvNo = "";
            string str = null;
            string[] strArr = null;
            str = txtPartIds.Text;
            if (str == "") return;
            strArr = str.Split('#');
            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                sInvNo = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblInvoiceNo") as Label).Text;

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
        private void ChkSelectedParts1()
        {
            //CheckBox chk = new CheckBox();
            //string sPartID = "";
            //string str = null;
            //string[] strArr = null;
            //str = txtPartIds.Text;
            //if (str == "") return;
            //strArr = str.Split('#');
            //for (int iRowCnt = 0; iRowCnt < PartDetailsGrid1.Rows.Count; iRowCnt++)
            //{
            //    sPartID = (PartDetailsGrid1.Rows[iRowCnt].FindControl("lblID") as Label).Text;

            //    for (int j = 0; j < strArr.Length; j++)
            //    {
            //        if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
            //        {
            //            chk = (CheckBox)PartDetailsGrid1.Rows[iRowCnt].FindControl("ChkPart");
            //            chk.Checked = true;
            //            break;
            //        }
            //    }
            //}
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