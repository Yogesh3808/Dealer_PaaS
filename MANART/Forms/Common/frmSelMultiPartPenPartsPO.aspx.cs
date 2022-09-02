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
    public partial class frmSelMultiPartPenPartsPO : System.Web.UI.Page
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
                dtDetails = (DataTable)Session["MRPartDetails"];
                idataCount = dtDetails.Rows.Count;


                dtTemp = dtDetails.Clone();
                dtTemp.ImportRow(dtDetails.Rows[idataCount - 1]);
                //dtDetails.Rows.RemoveAt(idataCount - 1);



                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    //SRNo,ID,Part_ID,PO_Det_ID,part_no,group_code,Part_Name,Bill_Qty,MRPRate,Recv_Qty,Bal_PO_Qty,
                    //Accept_Rate,Disc_Per,Total,Status,Tax_Per,PO_No,Qty,PartTaxID,MOQ,
                    //Descripancy_YN,Shortage_Qty,Excess_Qty,Damage_Qty,Man_Defect_Qty,Wrong_Supply_Qty,Retain_YN,Wrg_Part_ID,Wrg_Part_No,Wrg_Part_Name

                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));

                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("PO_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Bill_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Recv_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Bal_PO_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Accept_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Disc_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRP_Rate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Ass_Value", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Tax_Per", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("PO_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(int)));

                    dtDetails.Columns.Add(new DataColumn("Descripancy_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Shortage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Excess_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Damage_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Man_Defect_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Wrong_Supply_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Retain_YN", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Wrg_Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(String)));
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
                        dr["PO_Det_ID"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["PO_No"] = Func.Convert.sConvertToString(myArray[3]);
                        dr["Part_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["part_no"] = Func.Convert.sConvertToString(myArray[5]);
                        dr["Part_Name"] = Func.Convert.sConvertToString(myArray[6]);
                        dr["group_code"] = Func.Convert.sConvertToString(myArray[7]);

                        dr["Bill_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                        dr["Recv_Qty"] = Func.Convert.dConvertToDouble(myArray[11]);
                        dr["Bal_PO_Qty"] = 0;

                        dr["MRPRate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["Accept_Rate"] = Func.Convert.dConvertToDouble(myArray[1]);
                        dr["Descripancy_YN"] = "N";
                        dr["Shortage_Qty"] = 0;
                        dr["Excess_Qty"] = 0;
                        dr["Damage_Qty"] = 0;
                        dr["Man_Defect_Qty"] = 0;
                        dr["Wrong_Supply_Qty"] = 0;
                        dr["Retain_YN"] = "N";
                        dr["Wrg_Part_ID"] = 0;
                        dr["Wrg_Part_No"] = "";
                        dr["Wrg_Part_Name"] = "";
                        dr["disc_per"] = Func.Convert.dConvertToDouble(myArray[12]);
                        //dr["discount_amt"] = Convert.ToDouble(myArray[13]);
                        dr["Total"] = Convert.ToDouble(myArray[15]);
                        dr["PartTaxID"] = Func.Convert.dConvertToDouble(myArray[10]);
                        dr["Status"] = "U";
                        dr["TaxTag"] = Func.Convert.sConvertToString(myArray[24]);
                        dr["MRP_Rate"] = 0.00;
                        dr["Ass_Value"] = 0.00;

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }

                //dtDetails.ImportRow(dtTemp.Rows[0]);

                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");
                Session["MRPartDetails"] = dtDetails;

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
                int iSupplierID;
                string sSelectedPartID = "";
                string sIs_AutoReceipt = "";
                int iHOBrID = 0;
                //Sujata 24022011        
                if (Func.Convert.sConvertToString(txtSearch.Text.Trim()) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                //Sujata 24022011
                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();
                iSupplierID = Func.Convert.iConvertToInt(Request.QueryString["SupplierID"].ToString());
                sIs_AutoReceipt = Request.QueryString["Is_AutoReceipt"].ToString();
                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }

                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPOPartDetails_With_MRPRate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID, iSupplierID, sIs_AutoReceipt,iHOBrID);


                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    //PartDetailsGrid.DataSource = dsSrchgrid;
                    //PartDetailsGrid.DataBind();

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