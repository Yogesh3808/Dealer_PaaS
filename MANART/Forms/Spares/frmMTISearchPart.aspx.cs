using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Data;
using MANART_DAL;

namespace MANART.Forms.Spares
{
    public partial class frmMTISearchPart : System.Web.UI.Page
    {
        int iUserId = 0;
        int iDealerID = 0;
        int iMenuId = 0;
        string sDealerCode = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                //  iDealerID = Location.iDealerId; 
                if (!IsPostBack)
                {
                    FillRegion();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        public void FillRegion()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtRegion = new DataTable();
                dtRegion = objDB.ExecuteQueryAndGetDataTable("select  M_Region.ID,M_Region.Region_Name as Name from M_Region  where Domestic_Export ='D'");
                drpRegion.DataSource = dtRegion;
                drpRegion.DataTextField = "Name";
                drpRegion.DataValueField = "ID";
                drpRegion.DataBind();
                drpRegion.Items.Insert(0, new ListItem("All", "0"));
                //if (iMenuId == 573)
                //{
                //    dtRegion = objDB.ExecuteQueryAndGetDataTable("select  M_Region.ID,M_Region.Region_Name as Name from M_Dealer inner join M_Region on M_Region.ID=M_Dealer.Dealer_Region_ID where M_Dealer.ID=" + iDealerID + "");
                //    drpRegion.DataSource = dtRegion;
                //    drpRegion.DataTextField = "Name";
                //    drpRegion.DataValueField = "ID";
                //    drpRegion.DataBind();

                //}
                //else
                //{
                //    dtRegion = objDB.ExecuteQueryAndGetDataTable("select M_Region.ID,M_Region.Region_Name as Name from  M_Region where M_Region.ID  in(select distinct(RegionId) from M_Sys_UserPermissions where userid =" + iUserId + " And IsActive=1) order by M_Region.Region_Name");
                //    drpRegion.DataSource = dtRegion;
                //    drpRegion.DataTextField = "Name";
                //    drpRegion.DataValueField = "ID";
                //    drpRegion.DataBind();
                //    drpRegion.Items.Insert(0, new ListItem("All", "0"));
                //}

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
        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            btnPrint.Attributes.Add("onclick", "return ShowReportPartSearch('" + iMenuId + "');");
        }

        protected void btnSearchPart_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayPreviousRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
            }
        }

        private void DisplayPreviousRecord()
        {
            try
            {
                DataTable dtPart = new DataTable();

                dtPart = GetStockByPartNo(Func.Convert.iConvertToInt(drpRegion.SelectedItem.Value),iUserId, txtPartNo.Text.Trim());
                if (dtPart != null && dtPart.Rows.Count > 0)
                {
                    PartGrid.DataSource = dtPart;
                    PartGrid.DataBind();
                    lblConfirm.Visible = false;
                    btnPrint.Visible = true;
                }
                else
                {
                    PartGrid.DataSource = dtPart;
                    PartGrid.DataBind();
                    lblConfirm.Visible = true;
                    btnPrint.Visible = false;
                    lblConfirm.Text = "Record Does Not Exist !";
                }

                // Modify By Vikram on 24.09.2016 For Show Stock to Dealer
                if (iMenuId == 573)// Dealer Menu
                {
                    //for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    //{
                    //    PartGrid.HeaderRow.Cells[8].Visible = false;
                    //    PartGrid.Rows[iRowCnt].FindControl("txtQuantity").Visible = false;
                    //}
                    for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["Dealer_Spares_Code"]) == sDealerCode)
                        {
                            PartGrid.HeaderRow.Cells[8].Visible = true;
                            PartGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");//Hide Cell 
                        }
                        else
                        {
                            PartGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Hide Cell
                        }

                    }
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
        public DataTable GetStockByPartNo(int RegionId,int UserID, string PartNo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtPart;
                dtPart = objDB.ExecuteStoredProcedureAndGetDataTable("SP_DCS_GetStockByPartNo", RegionId, UserID, PartNo);
                return dtPart;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}