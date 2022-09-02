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

namespace MANART.Forms.Master
{
    public partial class frmSiteMaintainance : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            txtSelDate.sOnBlurScript = " return SetCurrentFutureDate(ContentPlaceHolder1_txtSelDate_txtDocDate,'Select Date Should be Greater or equal to Current Date');";
            if (!IsPostBack)
            {
                //fillGrid();
                FillCombo();
                DisplayCurrentRecord();
                fillGrid();
            }

        }
        private void fillGrid()
        {
            DataSet ds = new DataSet();
            clsPageUnderMaintainance objPageUnderMaintainance = new clsPageUnderMaintainance();
            ds = objPageUnderMaintainance.GetMaintainance(0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFSDetails.DataSource = ds;
                gvFSDetails.DataBind();
            }
        }
        private void FillCombo()
        {
            for (int i = 1; i <= 24; i++)
            {
                drpStartHours.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
            for (int i = 1; i <= 60; i++)
            {
                drpStartMinutes.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
            for (int i = 1; i <= 24; i++)
            {
                drpEndHours.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
            for (int i = 1; i <= 60; i++)
            {
                drpEndMinutes.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
            for (int i = 1; i <= 24; i++)
            {
                drpDMsgHours.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
            for (int i = 1; i <= 60; i++)
            {
                drpDMsgMinutes.Items.Add(new ListItem(((i - 1).ToString().Length == 1) ? "0" + (i - 1).ToString() : (i - 1).ToString(), (i - 1).ToString()));
            }
        }
        //private void fillGrid()
        //{
        //    DataSet ds = new DataSet();
        //    clsContent clsPageUnderMaintainance = new clsContent();
        //    ds = clsPageUnderMaintainance.GetContent(Func.Convert.iConvertToInt(txtID.Text));
        //    if (ds!=null)
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        gvFSDetails.DataSource = ds;
        //        gvFSDetails.DataBind();
        //    }
        //}    

        private DataTable ContentSave()
        {
            DataTable dtHdr = new DataTable();
            DataRow dr;
            string[] sSelDate = txtSelDate.Text.Split('/');
            string[] sDmsgDate = txtDMsgTime.Text.Split('/');
            DateTime dtStart = new DateTime(Func.Convert.iConvertToInt(sSelDate[2]), Func.Convert.iConvertToInt(sSelDate[1]), Func.Convert.iConvertToInt(sSelDate[0]));
            dtStart = dtStart.Add(new TimeSpan(Convert.ToInt32(drpStartHours.SelectedValue), Convert.ToInt32(drpStartMinutes.SelectedValue), 0));
            DateTime dtEnd = new DateTime(Func.Convert.iConvertToInt(sSelDate[2]), Func.Convert.iConvertToInt(sSelDate[1]), Func.Convert.iConvertToInt(sSelDate[0]));
            dtEnd = dtEnd.Add(new TimeSpan(Convert.ToInt32(drpEndHours.SelectedValue), Convert.ToInt32(drpEndMinutes.SelectedValue), 0));

            DateTime dtDmsg = new DateTime(Func.Convert.iConvertToInt(sDmsgDate[2]), Func.Convert.iConvertToInt(sDmsgDate[1]), Func.Convert.iConvertToInt(sDmsgDate[0]));
            dtDmsg = dtDmsg.Add(new TimeSpan(Convert.ToInt32(drpDMsgHours.SelectedValue), Convert.ToInt32(drpDMsgMinutes.SelectedValue), 0));
            //DateTime dtDmsg = new DateTime(Func.Convert.iConvertToInt(sSelDate[2]), Func.Convert.iConvertToInt(sSelDate[1]), Func.Convert.iConvertToInt(sSelDate[0]));
            //dtDmsg = dtDmsg.Add(new TimeSpan(Convert.ToInt32(drpDMsgHours.SelectedValue), Convert.ToInt32(drpDMsgMinutes.SelectedValue), 0));

            //string[] sStartTime = txtStartTime.Text.Split(':');
            //string[] sEndTime = txtEndTime.Text.Split(':');

            //dtStart = dtStart.AddHours(Func.Convert.iConvertToInt(sStartTime[0]));
            //dtStart = dtStart.AddMinutes(Func.Convert.iConvertToInt(sStartTime[0]));
            //dtEnd = dtStart.AddHours(Func.Convert.iConvertToInt(sEndTime[0]));
            //dtEnd = dtStart.AddMinutes(Func.Convert.iConvertToInt(sEndTime[0]));

            //Get Header InFormation        
            dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("StartTime", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("EndTime", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("DMsgTime", typeof(String)));
            dtHdr.Columns.Add(new DataColumn("Region", typeof(string)));

            dr = dtHdr.NewRow();
            dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
            dr["StartTime"] = dtStart.ToString("dd/MM/yyy HH:mm");
            dr["EndTime"] = dtEnd.ToString("dd/MM/yyy HH:mm");
            dr["DMsgTime"] = dtDmsg.ToString("dd/MM/yyy HH:mm");
            dr["Region"] = txtRegion.Text.Trim();
            dtHdr.Rows.Add(dr);
            dtHdr.AcceptChanges();
            return dtHdr;

        }
        private void DisplayCurrentRecord()
        {
            DataTable dtDetails = null;
            DataSet ds = new DataSet();
            txtID.Text = "0";
            clsPageUnderMaintainance objPageUnderMaintainance = new clsPageUnderMaintainance();
            ds = objPageUnderMaintainance.GetMaintainance(Func.Convert.iConvertToInt(txtID.Text));
            if (ds != null)
            {
                dtDetails = ds.Tables[0];
                if (dtDetails.Rows.Count > 0)
                {
                    DisplayData(dtDetails);
                }
            }

        }
        private void NewRecord()
        {
            txtID.Text = "0";
            txtSelDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDMsgTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
            drpStartHours.SelectedValue = "-1";
            drpStartMinutes.SelectedValue = "-1";
            drpEndHours.SelectedValue = "-1";
            drpEndMinutes.SelectedValue = "-1";
            drpDMsgHours.SelectedValue = "-1";
            drpDMsgMinutes.SelectedValue = "-1";
            txtRegion.Text = "";
          
            //txtStartTime.Text = "";
            //txtEndTime.Text = "";
            //txtDMsgTime.Text = "";
            //txtRegion.Text = "";
            //txtStartTime.Focus();

        }
        private void EnableDisable(string sConfirmStatus)
        {

            if (sConfirmStatus == "Y")
            {
                //txtStartTime.Enabled = false;
                //txtEndTime.Enabled = false;
                //txtDMsgTime.Enabled = false;
                txtRegion.Enabled = false;

                // ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmSave, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmConfirm, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmCancel, false);
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmPrint, false);
                //ToolbarC.sSetMessage(WebParts_Toolbar.enmToolbarType.enmConfirm);

            }
            else
                if (sConfirmStatus == "N")
                {

                    //txtStartTime.Enabled = true;
                    //txtEndTime.Enabled = true;
                    //txtDMsgTime.Enabled = true;
                    txtRegion.Enabled = true;


                    //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmSave, true);
                    //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmConfirm, true);
                    //ToolbarC.sSetMessage(WebParts_Toolbar.enmToolbarType.enmSave);

                }

        }
        private void GetDataAndDisplay()
        {
            DataTable dtDetails = null;


            DataSet ds = new DataSet();
            clsPageUnderMaintainance clsPageUnderMaintainance = new clsPageUnderMaintainance();
            ds = clsPageUnderMaintainance.GetMaintainance(Func.Convert.iConvertToInt(txtID.Text));
            dtDetails = ds.Tables[0];
            DisplayData(dtDetails);
            clsPageUnderMaintainance = null;

            dtDetails = null;

        }
        private void DisplayData(DataTable dtActivityDtls)
        {
            string sConfirmStatus = "";
            if (dtActivityDtls.Rows.Count == 0)
            {
                return;
            }

            //Display Header   
            string[] sStart, sEndTime, sDMsgTime, sTestTime;

            string sGetDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


            sStart = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["StartTime"]).Split(':');
            sEndTime = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["EndTime"]).Split(':');
            sDMsgTime = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["DisplayMsgTime"]).Split(':');

            sTestTime = (sDMsgTime[0]).Split('/');

            DateTime testDate = new DateTime(Func.Convert.iConvertToInt(Func.Convert.sConvertToString(sTestTime[2]).Substring(0, 4)), Func.Convert.iConvertToInt(sTestTime[1]), Func.Convert.iConvertToInt(sTestTime[0]));
            testDate = testDate.AddHours(Func.Convert.iConvertToInt(sDMsgTime[0].Substring(sStart[0].Length - 2, 2)));
            testDate = testDate.AddMinutes(Func.Convert.iConvertToInt(sDMsgTime[1]));

            DateTime testDate1 = new DateTime(Func.Convert.iConvertToInt(Func.Convert.sConvertToString(sTestTime[2]).Substring(0, 4)), Func.Convert.iConvertToInt(sTestTime[1]), Func.Convert.iConvertToInt(sTestTime[0]));
            testDate1 = testDate1.AddHours(Func.Convert.iConvertToInt(sStart[0].Substring(sStart[0].Length - 2, 2)));
            testDate1 = testDate1.AddMinutes(Func.Convert.iConvertToInt(sStart[1]));

            if (DateTime.Compare(DateTime.Now, testDate) >= 0 && DateTime.Compare(DateTime.Now, testDate1) <= 0)
            {

            }
            else
            {
            }


            //DateTime .Compare (
            //if ("#" + sGetDate + "#" >= "#" + Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["DisplayMsgTime"]) + "#" && sGetDate <= Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["StartTime"]))

            txtSelDate.Text = Func.Convert.sConvertToString(sStart[0].Substring(0, sStart[0].Length - 2));
            txtID.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["ID"]);
            drpStartHours.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sStart[0].Substring(sStart[0].Length - 2, 2)));
            drpStartMinutes.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sStart[1]));
            drpEndHours.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sEndTime[0].Substring(sStart[0].Length - 2, 2)));
            drpEndMinutes.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sEndTime[1]));
            drpDMsgHours.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sDMsgTime[0].Substring(sStart[0].Length - 2, 2)));
            drpDMsgMinutes.SelectedValue = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(sDMsgTime[1]));
            txtDMsgTime.Text = Func.Convert.sConvertToString(sDMsgTime[0].Substring(0, sDMsgTime[0].Length - 2));

            //txtStartTime.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["StartTime"]);
            //txtEndTime.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["EndTime"]);
            txtRegion.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["Region"]);
            //txtDMsgTime.Text = Func.Convert.sConvertToString(dtActivityDtls.Rows[0]["DMsgTime"]);       

            // EnableDisable(sConfirmStatus);

            //gvFSDetails.DataSource = dtActivityDtls;
            //gvFSDetails.DataBind();

        }

        protected void gvFSDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).ID == "ImgSelect")
            if (e.CommandName == "ImgSelect")
            {
                txtID.Text = e.CommandArgument.ToString();
                GetDataAndDisplay();
            }
        }
        protected void bSave_Click(object sender, EventArgs e)
        {
            //clsContent objContent = new clsContent();
            clsPageUnderMaintainance objPageUnderMaintainance = new clsPageUnderMaintainance();
            if (objPageUnderMaintainance.bSaveMaintainance(ContentSave()) == true)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                GetDataAndDisplay();
                fillGrid();
            }
            else
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
            }
            objPageUnderMaintainance = null;

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
        protected void btnNEW_Click(object sender, EventArgs e)
        {
            NewRecord();
        }
    }
}