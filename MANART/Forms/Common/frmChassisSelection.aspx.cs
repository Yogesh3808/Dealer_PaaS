using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Common
{
    public partial class frmChassisSelection : System.Web.UI.Page
    {
        private string sSelText, sSelType,sClmInvType,sScreen ;
        private int iJobtype, iDealerID, iHOBrID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ExpirePageCache();
            DisplayData();            
        }
        private void DisplayData()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;

                sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                sSelText = txtSearch.Text.ToString();
                iJobtype = Func.Convert.iConvertToInt(Request.QueryString["JobTypeID"].ToString());
                iDealerID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                iHOBrID = Func.Convert.iConvertToInt(Request.QueryString["HOBR_ID"].ToString());                
                
                sScreen = Func.Convert.sConvertToString(Request.QueryString["Screen"].ToString());

                if (sScreen == "C")
                {
                    sClmInvType = Func.Convert.sConvertToString(Request.QueryString["ClmInvType"].ToString());                
                    if (sClmInvType == "P" || sClmInvType == "L")
                    {
                        DdlSelctionCriteria.Items.Add(new ListItem("RequestNo", "R", true));
                        sSelType = "Claim" + sClmInvType + sSelType;
                    }
                }

                if (!IsPostBack)
                {
                    lblTitle.Text = "Chassis Selection";
                }

                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                ViewState["Chassis"] = dtSrchgrid;
                Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                //// if (DdlSelctionCriteria.SelectedValue == "Chassis_no" && txtSearch.Text != "")
                //dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                //ChassisGrid.DataSource = dvDetails;
                ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

                if (ChassisGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < ChassisGrid.Rows.Count; iRowCnt++)
                {                   
                    //Show Vehicle In No 
                        ChassisGrid.HeaderRow.Cells[5].Style.Add("display", (iJobtype == 0 || sScreen != "C") ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[5].Style.Add("display", (iJobtype == 0 || sScreen != "C")  ? "none" : "");//show Cell

                    //Show Vehicle In Date
                    ChassisGrid.HeaderRow.Cells[6].Style.Add("display", (iJobtype == 0 || sScreen != "C") ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[6].Style.Add("display", (iJobtype == 0 || sScreen != "C") ? "none" : "");//show Cell

                    //Show Last Kms
                    ChassisGrid.HeaderRow.Cells[18].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[18].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Hrs
                    ChassisGrid.HeaderRow.Cells[19].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[19].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Hrs
                    if (sScreen == "C" || sScreen == "R")
                    {
                        if (sScreen == "C") ChassisGrid.HeaderRow.Cells[5].Text = "Request No";
                        if (sScreen == "C") ChassisGrid.HeaderRow.Cells[6].Text = "Request Date";

                        ChassisGrid.HeaderRow.Cells[36].Style.Add("display", "none"); // show Header        
                        ChassisGrid.Rows[iRowCnt].Cells[36].Style.Add("display", "none");//show Cell

                        //Show Last Hrs
                        ChassisGrid.HeaderRow.Cells[37].Style.Add("display", "none"); // show Header        
                        ChassisGrid.Rows[iRowCnt].Cells[37].Style.Add("display", "none");//show Cell
                    }
                     
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" && btnSearch.Text == "Search")
                btnSearch.Text = "ClearSearch";
            else if (txtSearch.Text != "" && btnSearch.Text == "ClearSearch")
            {
                txtSearch.Text = "";
                btnSearch.Text = "Search";
            }
            DisplayData();
        }

        protected void ChassisGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ChassisGrid.PageIndex = e.NewPageIndex;
            DisplayData();
        }    
    }
}