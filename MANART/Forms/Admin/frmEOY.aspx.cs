using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Globalization;
using MANART_BAL;
using MANART_DAL;
using System.Web.Services;

namespace MANART.Forms.Admin
{
    public partial class frmEOY : System.Web.UI.Page
    {
        private DataTable dtEOYPendingDoc = new DataTable();

        int iDealerId;
        string sDealerCode;
        int iHOBranchDealerId;
        bool bEOYDone = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["EOYPendingDoc"] = null;

                    DisplayPreviousRecord();

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void DisplayPreviousRecord()
        {
            try
            {
                clsEOY ObjEOY = new clsEOY();
                DataSet ds = new DataSet();
                //Please remove this Temporary comment

                iDealerId = Location.iDealerId;
                sDealerCode = Location.sDealerCode;
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);

                btnDownload.Style.Add("display", "none");
                btnEOY.Style.Add("display", "");
                btnEOY.Enabled = true;

                ds = ObjEOY.GetEOYPendingDoc(iDealerId, iHOBranchDealerId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DisplayData(ds);
                            btnDownload.Style.Add("display", "");
                            btnEOY.Enabled = false;
                        }
                        else
                        {
                            DisplayData(ds);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    return;
                //}

                //Display Part Details  
                Session["EOYPendingDoc"] = null;
                dtEOYPendingDoc = ds.Tables[0];
                Session["EOYPendingDoc"] = dtEOYPendingDoc;
                //lblPartRecCnt.Text = (Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtPart)) == 0) ? Func.Common.sRowCntOfTable(dtPart) : Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Func.Common.sRowCntOfTable(dtPart)) - 1);
                BindDataToPartGrid(true, 0);


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        // Create Row To Part Grid
        private void CreateNewRowToPartGrid(int iNoRowToAdd)
        {
            try
            {
                DataRow dr;
                DataTable dtDefaultPart = new DataTable();
                int iRowCntStartFrom = 0;
                int iMaxPartGridRowCount = 0;
                iMaxPartGridRowCount = Func.Convert.iConvertToInt(ConfigurationManager.AppSettings["MaxPartGridRowCount"]);

                if (Session["EOYPendingDoc"] != null)
                {
                    dtDefaultPart = (DataTable)Session["EOYPendingDoc"];
                }
                else
                {
                    dtDefaultPart = dtEOYPendingDoc;
                }
                if (iNoRowToAdd == 0)
                {
                    if (dtDefaultPart.Rows.Count == 0)
                    {
                        ////SRNo, Dealer_Spares_Code , ID ,DocNo , DocDate , Status        
                        dtDefaultPart.Columns.Clear();
                        dtDefaultPart.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDefaultPart.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDefaultPart.Columns.Add(new DataColumn("Dealer_Spares_Code", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("DocNo", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("DocDate", typeof(string)));
                        dtDefaultPart.Columns.Add(new DataColumn("Status", typeof(string)));
                    }
                    else
                    {
                        if (dtDefaultPart.Rows.Count >= iMaxPartGridRowCount) goto Bind;
                    }
                }
                else
                {
                    iRowCntStartFrom = iMaxPartGridRowCount;
                }

                iMaxPartGridRowCount = iMaxPartGridRowCount + iNoRowToAdd;

                for (int iRowCnt = iRowCntStartFrom; iRowCnt < iMaxPartGridRowCount; iRowCnt++)
                {
                    dr = dtDefaultPart.NewRow();
                    //SRNo, Dealer_Spares_Code , ID ,DocNo , DocDate , Status
                    dr["SRNo"] = "1";
                    dr["ID"] = 0;
                    dr["Dealer_Spares_Code"] = "";
                    dr["DocNo"] = "";
                    dr["DocDate"] = "";
                    dr["Status"] = "";
                    dtDefaultPart.Rows.Add(dr);
                    dtDefaultPart.AcceptChanges();
                }
            Bind:
                Session["EOYPendingDoc"] = dtDefaultPart;
                PartDetailsGrid.DataSource = dtDefaultPart;
                PartDetailsGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //Bind Data to Part Grid
        private void BindDataToPartGrid(bool bRecordIsOpen, int iNoRowToAdd)
        {
            try
            {
                if (bRecordIsOpen == true)
                {
                    CreateNewRowToPartGrid(iNoRowToAdd);
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
                else
                {
                    PartDetailsGrid.DataSource = dtEOYPendingDoc;
                    PartDetailsGrid.DataBind();
                    SetControlPropertyToPartGrid(bRecordIsOpen);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Part Grid
        private void SetControlPropertyToPartGrid(bool bRecordIsOpen)
        {
            try
            {
                int idtRowCnt = 0;

                if (PartDetailsGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {

                    //SRNo, Dealer_Spares_Code , ID ,DocNo , DocDate , Status                    
                    Label txtDcID = (Label)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtDcID") as Label);
                    Label txtDCDate = (Label)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtDCDate") as Label);
                    Label txtDCNo = (Label)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtDCNo") as Label);
                    //Label txtStatus = (Label)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtStatus") as Label);

                    int iDtlID = 0;
                    if (idtRowCnt < dtEOYPendingDoc.Rows.Count)
                    {
                        iDtlID = Func.Convert.iConvertToInt(dtEOYPendingDoc.Rows[iRowCnt]["ID"]);

                        //PartID                        
                        txtDcID.Text = Func.Convert.sConvertToString(dtEOYPendingDoc.Rows[iRowCnt]["ID"]); ;

                        //PartNo 
                        txtDCNo.Text = Func.Convert.sConvertToString(dtEOYPendingDoc.Rows[iRowCnt]["DocNo"]);

                        txtDCDate.Text = Func.Convert.sConvertToString(dtEOYPendingDoc.Rows[iRowCnt]["DocDate"]);

                        //Status
                        //txtStatus.Text = Func.Convert.sConvertToString(dtEOYPendingDoc.Rows[iRowCnt]["Status"]);

                        idtRowCnt = idtRowCnt + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            clsExcel objExcel = null;

            System.Data.DataTable dtDownload = null;
            try
            {
                objExcel = new clsExcel();
                //dtDownload = (DataTable)Session["PartDetails"];
                DataSet dts = new DataSet();
                clsEOY ObjEOY = new clsEOY();
                dts = ObjEOY.GetEOYPendingDoc(Location.iDealerId, iHOBranchDealerId);
                dtDownload = dts.Tables[0];
                //dtDownload.Rows.RemoveAt(0);
                if (dtDownload != null)
                    if (dtDownload.Rows.Count > 0)
                    {
                        dtDownload.Columns.Remove("SRNo");
                        dtDownload.Columns.Remove("ID");

                        dtDownload.Columns["DocNo"].ColumnName = "Document";
                        dtDownload.Columns["DocDate"].ColumnName = "Date";
                        dtDownload.Columns["Status"].ColumnName = "Status";
                    }
                objExcel.DownloadInExcelFile(dtDownload, Location.sDealerCode + "_EOYPendingDocDetails.xls", "");

            }
            catch (Exception ex)
            {
                //Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void btnEOY_Click(object sender, EventArgs e)
        {
            bool bEOYDone = false;
            clsEOY ObjEOY = new clsEOY();
            try
            {
                if (ObjEOY.bEOYAction(Location.iDealerId, Func.Convert.iConvertToInt(Session["HOBR_ID"]), Func.Convert.iConvertToInt(Session["UserID"].ToString())) == true)
                {
                    bEOYDone = true;
                    ModalPopUpExtender.Show();
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('EOY Done Successfully.');</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('EOY not Done contact to administrator');</script>");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            if (bEOYDone == true)
            {
                //ForcefullyLogoutUser();
               // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('EOY Done Successfully.');</script>");

            }
               
        }
        
        private void ForcefullyLogoutUser()
        {
            clsUser objUser = new clsUser();
            int iID = Func.Convert.iConvertToInt(Session["UserLoginTrackID"]);
            objUser.SaveUserLoginHistory(ref iID, 0, "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "");
            DataTable dt = new DataTable();
            if (Application["UserTrack"] != null)
                dt = (DataTable)Application["UserTrack"];
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Func.Convert.sConvertToString(dt.Rows[i]["LoginName"]) == Func.Convert.sConvertToString(Session["LoginName"]) && Func.Convert.sConvertToString(dt.Rows[i]["SessionID"]) == Session.SessionID.ToString())
                        {
                            dt.Rows.RemoveAt(i);
                            dt.AcceptChanges();
                            Application.Lock();
                            Application["Active"] = Convert.ToInt32(Application["Active"]) - 1;
                            //this adds 1 to the number of active users when a new user hits 
                            Application.UnLock();
                        }
                    }
                    Application["UserTrack"] = dt;
                }

            ClearSessionVlaue();
            Response.Redirect("~/frmLogin.aspx",true);
        }
        // To Clear all User Session 
        private void ClearSessionVlaue()
        {
            //Page.Response.Cache.SetCacheability(HttpCacheability.NoCache); 
            Session["sMenuValue"] = null;
            Session["sMenuText"] = null;
            Session["UserType"] = null;
            Session["UserID"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();

        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            ForcefullyLogoutUser();
        }

        protected void PartDetailsGrid_DataBound(object sender, EventArgs e)
        {
            
        }

        protected void PartDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int RowSpan = 2;
            for (int i = PartDetailsGrid.Rows.Count - 2; i >= 0; i--)
            {
                GridViewRow currRow = PartDetailsGrid.Rows[i];
                GridViewRow prevRow = PartDetailsGrid.Rows[i + 1];
                if (currRow.Cells[0].Text == prevRow.Cells[0].Text)
                {
                    currRow.Cells[0].RowSpan = RowSpan;
                    prevRow.Cells[0].Visible = false;
                    RowSpan += 1;
                }
                else
                {
                    RowSpan = 2;
                }
            }
        }
    }
}