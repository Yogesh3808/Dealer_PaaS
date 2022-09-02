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

namespace MANART.Forms.Warranty
{
    public partial class frmFPDASelection : System.Web.UI.Page
    {
        DataSet dsSrchgrid;
        private int iDealerId;
        private string sSelectedPartID;
        string sPageName = "";
        string sClaimType = "";
        string FromDate = "";
        string ToDate = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                ExpirePageCache();
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                if (Request.QueryString["PageName"] != null)
                {
                    sPageName = Request.QueryString["PageName"].ToString();
                    hdnFor.Value = sPageName;
                }
                if (Request.QueryString["FromDate"] != null)
                    FromDate = Request.QueryString["FromDate"].ToString();

                if (Request.QueryString["ToDate"] != null)
                    ToDate = Request.QueryString["ToDate"].ToString();

                if (Request.QueryString["ClaimType"] != null)
                    sClaimType = Request.QueryString["ClaimType"].ToString();                

                if (!IsPostBack)
                {
                    Session["FPDAClaims"] = null;
                    DisplayData();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                ExpirePageCache();

                if (sPageName == "BankSTMT")
                    dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetWarrantyForBankStatement", iDealerId);
                else if (sPageName == "CouponClaim")
                    dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetJobcardForCouponClaim", iDealerId, sClaimType);
                else
                {
                    dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_FPDAWarrantyClaimNos", iDealerId, FromDate, ToDate);
                    //lblNote.Text = " Note : 1. List of claim credited between credit period from " + FromDate + " to " + ToDate + ".";//"</br> 2. Claim no will not be appeared for next FPDA creation which are not selected in the list.";
                    lblNote.Text = " Note : 1. List of claims between period from " + FromDate + " to " + ToDate + ".";//"</br> 2. Claim no will not be appeared for next FPDA creation which are not selected in the list.";
                }

                if (Func.Common.iRowCntOfTable(dsSrchgrid.Tables[0]) == 0)
                {
                    lblNote.Text = " Records Does Not Exist For Selection Criteria!";
                    return;
                }

                if (dsSrchgrid == null)
                {
                    return;
                }
                if (dsSrchgrid.Tables[0].Rows.Count != 0)
                {
                    ClaimsDetailsGrid.DataSource = dsSrchgrid;
                    ClaimsDetailsGrid.DataBind();
                    if (sPageName == "BankSTMT")
                    {
                        foreach (DataControlField col in ClaimsDetailsGrid.Columns)
                        {
                            if (col.HeaderText == "Claim Credit Date")
                            {
                                int colPos = ClaimsDetailsGrid.Columns.IndexOf(col);
                                ClaimsDetailsGrid.Columns[colPos].Visible = false;
                            }
                        }
                    }
                    else if (sPageName == "CouponClaim")
                    {
                        //foreach (DataControlField col in ClaimsDetailsGrid.Columns)
                        //{
                        //    int colPos = ClaimsDetailsGrid.Columns.IndexOf(col);
                        //    //if (colPos !=1 && colPos !=4 && colPos !=5)
                        //    if (colPos != 2 && colPos != 5 && colPos != 6)
                        //    {                                
                        //        //ClaimsDetailsGrid.Columns[colPos].Visible = false;
                        //    }                            
                        //}
                        for (int iRowCnt = 0; iRowCnt <= ClaimsDetailsGrid.Rows.Count; iRowCnt++)
                        {  
                            ClaimsDetailsGrid.HeaderRow.Cells[1].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[1].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[3].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[3].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[4].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[4].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//Hide Cell

                            ClaimsDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none"); // Hide Header        
                            ClaimsDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//Hide Cell                         

                        }
                        Session["CouponClaims"] = dsSrchgrid;
                    }
                    else
                    {
                        foreach (DataControlField col in ClaimsDetailsGrid.Columns)
                        {
                            int colPos = ClaimsDetailsGrid.Columns.IndexOf(col);
                            if (colPos == 2)
                            {
                                ClaimsDetailsGrid.Columns[colPos].Visible = false;
                            }
                        }
                    }
                }
                else
                {
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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        //protected void btnBack_Click(object sender, EventArgs e)
        //{
        //            DataTable dtDetails = new DataTable();
        //    //dt = dsSrchgrid.Tables[0].Clone();
        //            string swarratyClaimIDs = "";
        //    DataRow dr;
        //    if (dtDetails.Rows.Count == 0)
        //    {

        //        dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
        //        dtDetails.Columns.Add(new DataColumn("WarrantyClaim", typeof(string)));
        //    }

        //    for (int iRowCnt = 0; iRowCnt < ClaimsDetailsGrid.Rows.Count; iRowCnt++)
        //    {
        //        if ((ClaimsDetailsGrid.Rows[iRowCnt].FindControl("ChkPart") as CheckBox).Checked == true)
        //        {
        //            swarratyClaimIDs = swarratyClaimIDs + (ClaimsDetailsGrid.Rows[iRowCnt].FindControl("lblWarrantyClaim_No") as Label).Text + ",";
        //            //dr = dtDetails.NewRow();

        //            //dr["ID"] = (ClaimsDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
        //            //dr["WarrantyClaim"] = (ClaimsDetailsGrid.Rows[iRowCnt].FindControl("lblWarrantyClaim_No") as Label).Text;
        //            //dtDetails.Rows.Add(dr);
        //            //dtDetails.AcceptChanges();
        //        }
        //    }   


        //    //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");
        //   // Session["FPDAClaims"] = swarratyClaimIDs;
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);  
        //}


        protected void btnBackToParaent_Click(object sender, EventArgs e)
        {
            try
            {

                string swarratyClaimIDs = "";
                Hashtable categoryIDList = new Hashtable();

                RememberOldValues();
                RePopulateValues();

                categoryIDList = (Hashtable)ViewState["WarCHECKED_ITEMS"];

                // Loop through all items of a Hashtable
                if (categoryIDList != null) //null if condition Added By Sujata_03122012_Begin 
                {//Added By Sujata_03122012_Begin 
                    IDictionaryEnumerator en = categoryIDList.GetEnumerator();
                    while (en.MoveNext())
                    {
                        swarratyClaimIDs = swarratyClaimIDs + en.Value.ToString() + ((en.Value.ToString() != "") ? "," : "");
                    }
                }//Added By Sujata_03122012_End

                //Page.RegisterStartupScript("Close", "<script language='javascript'>CloseMe();</script>");

                Session["FPDAClaims"] = swarratyClaimIDs;
                if (sPageName == "BankSTMT")
                    Session["BankStmtWClaims"] = swarratyClaimIDs;
                if (sPageName == "CouponClaim")
                {
                    if ((String)Session["CouponClaims"] != null)
                    {
                        swarratyClaimIDs = (String)Session["CouponClaims"] + "," + swarratyClaimIDs;
                    }
                    Session["CouponClaims"] = swarratyClaimIDs;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void RememberOldValues()
        {
            Hashtable categoryIDList = new Hashtable();
            int index = -1;
            foreach (GridViewRow row in ClaimsDetailsGrid.Rows)
            {
                if (((Label)row.FindControl("lblNo")) != null)
                {
                    index = int.Parse(((Label)row.FindControl("lblNo")).Text);
                    string sWarClaimID = Func.Convert.sConvertToString(((Label)row.FindControl("lblClaimID")).Text);
                    bool result = ((CheckBox)row.FindControl("ChkPart")).Checked;
                    // Check in the Session
                    if (ViewState["WarCHECKED_ITEMS"] != null)
                        categoryIDList = (Hashtable)ViewState["WarCHECKED_ITEMS"];
                    if (result)
                    {
                        if (!categoryIDList.Contains(index))
                            categoryIDList.Add(index, sWarClaimID);
                    }
                    else
                        categoryIDList.Remove(index);
                }
            }
            if (categoryIDList != null && categoryIDList.Count > 0)
                ViewState["WarCHECKED_ITEMS"] = categoryIDList;

        }

        private void RePopulateValues()
        {
            Hashtable categoryIDList = (Hashtable)ViewState["WarCHECKED_ITEMS"];
            if (categoryIDList != null && categoryIDList.Count > 0)
            {
                foreach (GridViewRow row in ClaimsDetailsGrid.Rows)
                {
                    int index = int.Parse(((Label)row.FindControl("lblNo")).Text);
                    CheckBox myCheckBox = (CheckBox)row.FindControl("ChkPart");

                    if (categoryIDList.Contains(index))
                    {
                        myCheckBox.Checked = true;
                    }
                    else
                        myCheckBox.Checked = false;
                }
            }
        }
        protected void ClaimsDetailsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RememberOldValues();
            ClaimsDetailsGrid.PageIndex = e.NewPageIndex;
            DisplayData();
            RePopulateValues();
        }   

    }
}