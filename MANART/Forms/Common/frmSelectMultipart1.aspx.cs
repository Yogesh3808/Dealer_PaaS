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
    public partial class frmSelectMultipart1 : System.Web.UI.Page
    {
        DataSet dsSrchgrid = new DataSet();
        int iTotalCnt = 0;
        string sSelectedPartID = "";
        string sDocType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();


                if (!IsPostBack)
                {
                    lblTitle.Text = "Part Master";
                    DrpSelFrom.Visible = (Func.Convert.iConvertToInt(Request.QueryString["EstID"]) == 0) ? false : true;
                }

                bindGrid("A", "x");
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
                DataTable dtDetails = new DataTable();
                DataTable dtTemp = new DataTable();
                if (sDocType == "EXO")
                {
                    dtDetails = (DataTable)Session["LubricantDetails"];
                }
                else
                {
                    dtDetails = (DataTable)Session["PartDetails"];                    
                }
                idataCount = dtDetails.Rows.Count;
                dtTemp = dtDetails.Clone();

                // Get first row from table dtDetails,Previously it was last row,Changed by Shyamal on 02062012
                //dtTemp.ImportRow(dtDetails.Rows[0]);
                //dtDetails.Rows.RemoveAt(0);


                //DataTable dt = new DataTable();
                //dt = dsSrchgrid.Tables[0].Clone();

                DataRow dr;
                if (dtDetails.Columns.Count == 0)
                {
                    
                    if (sDocType =="EXO")
                    {                        
                        dtDetails.Columns.Add(new DataColumn("Lubricant_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Lubricant_Description", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("UOM", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("VECV_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Dealer_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Cust_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Deduction_Percentage", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Deducted_Amount", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Amount", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("TaxID", typeof(int)));  
                        dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Jobcard_Det_ID", typeof(int)));         
                    }
                    else if (sDocType =="EXP")
                    {
                        dtDetails.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("part_no", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Failed_Make", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Replaced_Part_No", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Replaced_Part_Name", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Replaced_Make", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Jobcard_Det_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("VECV_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Dealer_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Cust_Share", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Qty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Deduction_Percentage", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Deducted_Amount", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Accepted_Amount", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("TaxID", typeof(int)));  
                    }
                    else
                    {
                        dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));                        
                        dtDetails.Columns.Add(new DataColumn("part_type_tag", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("parts_no", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("ReqQty", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Stock", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("EstDtlID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("Tax", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("WarrRate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("AMCRate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("AccdFlag", typeof(string)));
                    }

                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax1", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Tax2", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));         
                    dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));                             
                }


                //Sujata 08012011
                string sPartID = sSelectedPartID;
                if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
                {
                    string myString = Func.Convert.sConvertToString(txtPartIds.Text);
                    string[] asd = myString.ToString().Split('#');
                    int iRowCnt = 0;

                    for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                    {
                        //if (dtDetails.Rows.Count == 301) break;
                        myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                        string[] delim = { "<--" };
                        string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                        dr = dtDetails.NewRow();                        
                        dr["ID"] = 0;                        
                        sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();
                        if (sDocType == "EXO")
                        {
                            dr["Lubricant_ID"] = Func.Convert.sConvertToString(myArray[0]);                            
                            dr["Lubricant_Description"] = Func.Convert.sConvertToString(myArray[2]);
                            dr["Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Total"] = Func.Convert.dConvertToDouble(0);
                            dr["BFRGST"] = "N";
                            dr["UOM"]="L";
                            dr["Job_Code_ID"] = Func.Convert.iConvertToInt(0);
                            dr["Jobcard_Det_ID"] = Func.Convert.iConvertToInt(0);
                            dr["VECV_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Dealer_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Cust_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Deduction_Percentage"] = Func.Convert.dConvertToDouble(0);
                            dr["Deducted_Amount"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Amount"] = Func.Convert.dConvertToDouble(0);
                            dr["TaxID"] = Func.Convert.iConvertToInt(myArray[9]);
                        }
                        else if (sDocType == "EXP")
                        {
                            dr["Part_No_ID"] = Func.Convert.sConvertToString(myArray[0]);
                            dr["part_no"] = Func.Convert.sConvertToString(myArray[1]);
                            dr["Part_Name"] = Func.Convert.sConvertToString(myArray[2]);
                            dr["Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Total"] = Func.Convert.dConvertToDouble(0);
                            dr["BFRGST"] = "N";

                            dr["Failed_Make"] = Func.Convert.iConvertToInt(0);
                            dr["Replaced_Part_No_ID"] = Func.Convert.sConvertToString(myArray[0]);
                            dr["Replaced_Part_No"] = Func.Convert.sConvertToString(myArray[1]);
                            dr["Replaced_Part_Name"] = Func.Convert.sConvertToString(myArray[2]);
                            dr["Replaced_Make"] = Func.Convert.iConvertToInt(0);
                            dr["Job_Code_ID"] = Func.Convert.iConvertToInt(0);
                            dr["Jobcard_Det_ID"] = Func.Convert.iConvertToInt(0);

                            dr["VECV_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Dealer_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Cust_Share"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Qty"] = Func.Convert.dConvertToDouble(0);
                            dr["Deduction_Percentage"] = Func.Convert.dConvertToDouble(0);
                            dr["Deducted_Amount"] = Func.Convert.dConvertToDouble(0);
                            dr["Accepted_Amount"] = Func.Convert.dConvertToDouble(0);
                            dr["TaxID"] = Func.Convert.iConvertToInt(myArray[9]);
                        }
                        else
                        {
                            dr["SRNo"] = iRowCnt;
                            dr["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                            dr["parts_no"] = Func.Convert.sConvertToString(myArray[1]);
                            dr["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                            dr["part_type_tag"] = Func.Convert.sConvertToString(myArray[5]);
                            dr["ReqQty"] = Func.Convert.dConvertToDouble(0);
                            dr["WarrRate"] = Func.Convert.dConvertToDouble(myArray[4]);
                            dr["AMCRate"] = Func.Convert.dConvertToDouble(myArray[12]);
                            dr["Stock"] = Func.Convert.dConvertToDouble(myArray[6]);
                            dr["EstDtlID"] = Func.Convert.iConvertToInt(myArray[7]);
                            dr["group_code"] = Func.Convert.sConvertToString(myArray[8]);
                            dr["Tax"] = Func.Convert.iConvertToInt(myArray[9]);
                            dr["AccdFlag"] = "N";
                        }                      
                        
                        dr["Tax1"] = Func.Convert.iConvertToInt(myArray[10]);
                        dr["Tax2"] = Func.Convert.iConvertToInt(myArray[11]);
                        dr["Status"] = "S";
                        dr["Rate"] = Func.Convert.dConvertToDouble(myArray[3]);

                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                        iRowCnt = iRowCnt + 1;
                    }
                }
                //Sujata 08012011

                //for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                //{
                //    if ((PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart") as CheckBox).Checked == true)
                //    {
                //        dr = dtDetails.NewRow();
                //        dr["SRNo"] = iRowCnt;
                //        dr["ID"] = 0;
                //        dr["Part_No_ID"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                //        dr["part_no"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblPartNo") as Label).Text; ;
                //        dr["Part_Name"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblPartName") as Label).Text; ;

                //        //Sujata 25122010
                //        //dr["Qty"] = 1;//(PartDetailsGrid.Rows[iRowCnt].FindControl("lblQty") as TextBox).Text; ;
                //        dr["Qty"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text; ;
                //        //Sujata 25122010
                //        dr["MOQ"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text; ;
                //        dr["FOBRate"] = Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);

                //        //dr["Total"] = 1 * Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);
                //        dr["Total"] = Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblMOQ") as Label).Text) * Convert.ToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblFOBRate") as Label).Text);
                //        dr["Status"] = "U";
                //        dr["Process_Accept"] = 0;
                //        dr["Process_Confirm"] = 0;
                //        dtDetails.Rows.Add(dr);
                //        dtDetails.AcceptChanges();
                //    }
                //}

                //No need to Add row again,Modified by Shyamal 02062012
                //dtDetails.ImportRow(dtTemp.Rows[0]);

                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");            
                //DLL.clsDB objDB = new DLL.clsDB();
                //int iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartSuperceeded", sPartID, iDealerId);
                //dtDetails = dsSrchgrid.Tables[0]; 

                if (sDocType == "EXO")
                {
                    Session["LubricantDetails"] = dtDetails;
                }
                else
                {
                    Session["PartDetails"] = dtDetails;
                }
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
            DataSet dsSrchgrid;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int iDealerId;
                int EstID;
                string RepairOrderDate = "";
                string sCustTaxTag = "";
                string sDocGST = "";
               
                //Sujata 24022011        
                if (Func.Convert.sConvertToString(txtSearch.Text) != "")
                {
                    sSearchPart = Func.Convert.sConvertToString(txtSearch.Text);
                    sSelect = Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue);
                }
                sDocType = Func.Convert.sConvertToString(Request.QueryString["sDocType"]);

                if (DrpSelFrom.SelectedValue == "E") 
                    sSelect = "E" + sSelect;
                else if (sDocType == "EXP" || sDocType == "EXO") 
                    sSelect = sDocType + sSelect;
                
                //Sujata 24022011
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                sSelectedPartID = Request.QueryString["SelectedPartID"].ToString();                
                RepairOrderDate = Func.Convert.sConvertToString(Request.QueryString["RepairOrderDate"]);
                EstID = Func.Convert.iConvertToInt(Request.QueryString["EstID"]);
                sCustTaxTag = Func.Convert.sConvertToString(Request.QueryString["CustTaxTag"]);
                sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"]);

                if (PagerV2_1.CurrentIndex == 0)
                {
                    PagerV2_1.CurrentIndex = 1;
                }

                //dsSrchgrid = BLL.Func.DB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDPRate", iDealerId, sSelectedPartID);
                //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDPRate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, sSelectedPartID);
                dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PartDetails_With_NDP_FOB_Rate_Paging", sSelect, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, iDealerId, RepairOrderDate, sSelectedPartID, EstID, sCustTaxTag, sDocGST);

                if (dsSrchgrid == null)
                {
                    lblNMsg.Visible = true;
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    //PartDetailsGrid.DataSource = dsSrchgrid;
                    //PartDetailsGrid.DataBind();

                    //iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][6]);
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
                    ChkSelectedParts();
                    return;
                }
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Req Qty dtPart.Rows[iRowCnt]["PartLabourID"]
                    if (sDocType == "EXP" || sDocType == "EXO")
                    {
                        PartDetailsGrid.HeaderRow.Cells[4].Style.Add("display", "none"); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[4].Style.Add("display", "none");//Hide Cell

                        PartDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none"); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//Hide Cell

                        PartDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none"); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "none");//Hide Cell
                    }
                    else
                    {
                        PartDetailsGrid.HeaderRow.Cells[4].Style.Add("display", ""); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[4].Style.Add("display", "");//Hide Cell

                        PartDetailsGrid.HeaderRow.Cells[7].Style.Add("display", ""); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");//Hide Cell

                        PartDetailsGrid.HeaderRow.Cells[13].Style.Add("display", ""); // Hide Header        
                        PartDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "");//Hide Cell
                    }
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
            string strAllLabor = null;
            string[] strAllLaborArr = null;

            //str = txtPartIds.Text;
            //if (str == "") return;
            //strArr = str.Split('#');
            strAllLabor = txtPartIds.Text;
            if (strAllLabor == "") return;
            strAllLaborArr = strAllLabor.Split('#');

            for (int i = 0; i < strAllLaborArr.Length; i++)
            {
                str = Func.Convert.sConvertToString(strAllLaborArr[i]);
                if (str == "") return;

                string[] stringSeparators = new string[] { "<--" };

                strArr = str.Split(stringSeparators, StringSplitOptions.None);

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    sPartID = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;

                    if (sPartID == Func.Convert.sConvertToString(strArr[0]))
                    {
                        chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                        chk.Checked = true;
                        break;
                    }

                    //for (int j = 0; j < strArr.Length; j++)
                    //{
                    //    if (sPartID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                    //    {
                    //        chk = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkPart");
                    //        chk.Checked = true;
                    //        break;
                    //    }
                    //}
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