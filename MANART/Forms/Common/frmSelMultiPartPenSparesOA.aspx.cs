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
using AjaxControlToolkit;
using System.Drawing;
using System.IO;

namespace MANART.Forms.Common
{
    public partial class frmSelMultiPartPenSparesOA : System.Web.UI.Page
    {
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        string sSelCaseP = "";
        clsEGPSparesInvoice objInv = null;
        int iDealerId;
        int iCustId;
        string sSelectedPartID = "";
        int iHOBrID = 0;
        string sDocGST = "", sCustTaxTag = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                btnBack.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnBack, null) + ";");
                sSelCaseP = Request.QueryString["SelCase"].ToString();
                if (!IsPostBack)
                {
                    lblTitle.Text = sSelCaseP == "D" ? "Pending OA and Part Master" : "Part Master";
                }

                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                iCustId = Func.Convert.iConvertToInt(Request.QueryString["CustID"].ToString());
                sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"]);
                sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);

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
                string strPart = "", PartIds = "";
                int idataCount = 0;
                string[] asd;
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                dtDetails = (DataTable)Session["PartDetails"];
                idataCount = dtDetails.Rows.Count;

                dtTemp = dtDetails.Clone();
                dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                //dtDetails.Rows.RemoveAt(idataCount - 1);

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
                    dtDetails.Columns.Add(new DataColumn("OA_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("OANo", typeof(String)));

                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    // OA Balance Qty
                    dtDetails.Columns.Add(new DataColumn("OABal_Qty", typeof(int)));

                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Unit", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Price", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("bal_qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));

                    dtDetails.Columns.Add(new DataColumn("lab_tag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("foc_tag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("war_tag", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("part_type_tag", typeof(String)));
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
                        //SRNo,ID,Part_ID,part_no,Part_Name,Qty,group_code,bal_qty,MRPRate,discount_per,disc_rate,discount_amt,Total,Status
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["OA_Det_ID"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["OANo"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[5]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[6]);

                        strPart = Func.Convert.sConvertToString(myArray[2]) + "-" + Func.Convert.sConvertToString(myArray[0]);
                        PartIds = PartIds + strPart + ",";


                        dr["group_code"] = Func.Convert.sConvertToString(myArray[7]);
                        dr["bal_qty"] = Func.Convert.dConvertToDouble(myArray[9]);
                        dr["Unit"] = Func.Convert.sConvertToString(myArray[18].Trim());
                        dr["Price"] = Func.Convert.sConvertToString(myArray[19]);
                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);

                        ////Vikram Kite
                        dr["OABal_Qty"] = 0;
                        //dr["OABal_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                        //dr["Qty"] = 0;
                        dr["Qty"] = Func.Convert.dConvertToDouble(myArray[11]);

                        dr["discount_per"] = Func.Convert.dConvertToDouble(myArray[12]);
                        dr["discount_amt"] = Func.Convert.dConvertToDouble(myArray[13]);
                        dr["disc_rate"] = Func.Convert.dConvertToDouble(myArray[14]);
                        dr["Total"] = Func.Convert.dConvertToDouble(myArray[15]);
                        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[10]);
                        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[16]).Trim();
                        dr["Status"] = "U";

                        dr["lab_tag"] = "N";
                        dr["foc_tag"] = "N";
                        dr["war_tag"] = "N";
                        dr["part_type_tag"] = (Func.Convert.sConvertToString(myArray[7]).Trim() == "02") ? "O" : "P";
                        dr["BFRGST"] = Func.Convert.sConvertToString(myArray[20].Trim());
                        dr["BFRGST_Stock"] = Func.Convert.dConvertToDouble(myArray[21]);

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                    sSelectedPartID = sSelectedPartID + ',' + PartIds;

                }
                if (txtOANO.Text.Trim() != "" && ddlPenOAList.SelectedIndex != -1)
                {

                    DataSet dsSelectAll;
                    DataTable dtSelAllOA = new DataTable();
                    clsDB objDB = new clsDB();
                    object[] ParaValues = { "OA", Func.Convert.sConvertToString(ddlPenOAList.SelectedItem.Text.Trim()), 15, 1, iDealerId, sSelectedPartID,
                                              iCustId, "A", iHOBrID, sDocGST, sCustTaxTag };
                    dsSelectAll = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetAllOAPartDetails_With_MRPRate_NoPaging", ParaValues);
                    dtSelAllOA = dsSelectAll.Tables[0];
                    int iRowCnt = 0;
                    for (int iRow = 0; iRow < dtSelAllOA.Rows.Count; iRow++)
                    {
                        dr = dtDetails.NewRow();
                        dr["SRNo"] = iRowCnt;
                        dr["ID"] = 0;
                        dr["OA_Det_ID"] = Func.Convert.iConvertToInt(dtSelAllOA.Rows[iRow]["OA_Det_ID"]);
                        dr["OANo"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["OANo"].ToString().Trim());
                        dr["Part_ID"] = Func.Convert.iConvertToInt(dtSelAllOA.Rows[iRow]["Id"]);
                        dr["part_no"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["Part_No"].ToString().Trim());
                        dr["Part_Name"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["Part_Name"].ToString().Trim());

                        dr["group_code"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["group_code"].ToString().Trim());
                        dr["bal_qty"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["Cl_Bal"]);
                        dr["Unit"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["Unit"].ToString().Trim());
                        dr["Price"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["Price"]);
                        dr["MRPRate"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["Rate"]);
                        dr["OABal_Qty"] = 0.00;
                        dr["Qty"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["Qty"]);
                        dr["discount_per"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["discount_per"]);
                        dr["discount_amt"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["discount_amt"]);
                        dr["disc_rate"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["disc_rate"]);
                        dr["Total"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["Total"]);
                        dr["PartTaxID"] = Func.Convert.iConvertToInt(dtSelAllOA.Rows[iRow]["PartTaxID"]);
                        dr["TaxTag"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["TaxTag"].ToString().Trim());
                        dr["Status"] = "U";
                        dr["lab_tag"] = "N";
                        dr["foc_tag"] = "N";
                        dr["war_tag"] = "N";
                        dr["part_type_tag"] = (Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["group_code"].ToString().Trim()) == "02") ? "O" : "P";
                        dr["BFRGST"] = Func.Convert.sConvertToString(dtSelAllOA.Rows[iRow]["BFRGST"].ToString().Trim());
                        dr["BFRGST_Stock"] = Func.Convert.dConvertToDouble(dtSelAllOA.Rows[iRow]["BFRGST_Stock"]);

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
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid;
                //int iDealerId;
                //int iCustId;
                //string sSelectedPartID = "";
                //int iHOBrID = 0;
                //string sDocGST = "", sCustTaxTag = "";
                if (Func.Convert.sConvertToString(txtSearch.Text.Trim()) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                if (Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue) == "OA")
                {
                    checkAll_OARef.Style.Add("display", "");
                    lblSelectAll.Style.Add("display", "");
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                    if (Func.Convert.sConvertToString(ddlPenOAList.SelectedIndex) != "-1" && Func.Convert.sConvertToString(ddlPenOAList.SelectedIndex) != "0")
                        sSearchPart = Func.Convert.sConvertToString(ddlPenOAList.SelectedItem.Text.Trim());
                    else
                        sSearchPart = "";
                }
                else
                {
                    checkAll_OARef.Style.Add("display", "none");
                    lblSelectAll.Style.Add("display", "none");
                }
                //iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                //iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                //sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                //iCustId = Func.Convert.iConvertToInt(Request.QueryString["CustID"].ToString());
                //sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"]);
                //sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }
                object[] ParaValues = { sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustId, sSelCaseP, iHOBrID, sDocGST, sCustTaxTag };
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetOAPartDetails_With_MRPRate_Paging", ParaValues);

                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetOAPartDetails_With_MRPRate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iCustId, sSelCaseP,iHOBrID);
                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][7]);
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
            string sOADetID = "";
            string str = null;
            string[] strArr = null;
            string strAllParts = null;
            string[] strAllPartsArr = null;

            strAllParts = txtPartIds.Text;
            if (strAllParts == "") return;
            strAllPartsArr = strAllParts.Split('#');

            for (int i = 0; i < strAllPartsArr.Length; i++)
            {
                str = Func.Convert.sConvertToString(strAllPartsArr[i]);
                if (str == "") return;

                string[] stringSeparators = new string[] { "<--" };

                strArr = str.Split(stringSeparators, StringSplitOptions.None);

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                    sOADetID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblOADetID") as Label).Text;

                    if (sPartID == Func.Convert.sConvertToString(strArr[0]) && sOADetID == Func.Convert.sConvertToString(strArr[2]))
                    {
                        chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                        chk.Checked = true;
                        break;
                    }


                }
            }
        }
        //private void ChkSelectedParts()
        //{
        //    CheckBox chk = new CheckBox();
        //    string sPartID = "";
        //    string str = null;
        //    string[] strArr = null;
        //    str = txtPartIds.Text;
        //    if (str == "") return;
        //    strArr = str.Split('#');
        //    for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
        //    {
        //        sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;

        //        for (int j = 0; j < strArr.Length; j++)
        //        {
        //            if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
        //            {
        //                chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
        //                chk.Checked = true;
        //                break;
        //            }
        //        }
        //    }
        //}

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


        protected void DdlSelctionCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue) == "OA")
            {
                ddlPenOAList.Style.Add("display", "");
                checkAll_OARef.Style.Add("display", "");
                lblSelectAll.Style.Add("display", "");
                txtSearch.Style.Add("display", "none");
                FillCombo();
            }
            else
            {
                ddlPenOAList.Style.Add("display", "none");
                checkAll_OARef.Style.Add("display", "none");
                lblSelectAll.Style.Add("display", "none");
                txtSearch.Style.Add("display", "");
            }
            if (Func.Convert.sConvertToString(ddlPenOAList.SelectedValue) != "0")
                bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
        }
        private void FillCombo()
        {
            DataTable dtOANoList = null;
            objInv = new clsEGPSparesInvoice();
            try
            {
                dtOANoList = new DataTable();
                dtOANoList = objInv.GetAllPendingOAList(iDealerId, iCustId, sSelectedPartID);
                ddlPenOAList.DataValueField = "ID";
                ddlPenOAList.DataTextField = "OA_NO";
                ddlPenOAList.DataSource = dtOANoList;
                ddlPenOAList.DataBind();
                ddlPenOAList.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (dtOANoList != null) dtOANoList = null;
                if (objInv != null) objInv = null;
            }
        }
    }
}