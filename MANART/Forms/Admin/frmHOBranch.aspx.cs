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

namespace MANART.Forms.Admin
{
    public partial class frmHOBranch : System.Web.UI.Page
    {
        string DealerHOBranchName = "";
        string DealerHOName = "";
        string DealerHOBranchCode = "";
        string DealerHOCode = "";
        int DealerHOBranchID = 0;
        int DealerHOID = 0;
        private int iID;
        protected void Page_Load(object sender, EventArgs e)
        {
            clsHOBranch objHOBranch = new clsHOBranch();
            DataSet ds = new DataSet();
            iID = Func.Convert.iConvertToInt(txtID.Text);

            ds = objHOBranch.GetMaxHOBranch_Record(0);
            if (!IsPostBack)
            {
                // FillCombo("All");

                iID = Func.Convert.iConvertToInt(txtID.Text);
                if (iID == 0)
                    iID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                if (iID != 0)
                {
                    GetDataAndDisplay();

                }
            }

            FillSelectionGrid();
        }
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("HO Dealer Name");
                SearchGrid.AddToSearchCombo("Branch Dealer Name");
                SearchGrid.AddToSearchCombo("HOBranchCode");
                SearchGrid.AddToSearchCombo("DealerLocationType");
                SearchGrid.iDealerID = 1;
                SearchGrid.sSqlFor = "HOBranchList";
                SearchGrid.iBrHODealerID = 1;
                SearchGrid.sGridPanelTitle = "HOBranch List";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillCombo(string Type)
        {

            clsHOBranch objHOBranch = new clsHOBranch();
            DataTable dtHOBranch = new DataTable();
            DataTable dtHO = new DataTable();
            DataTable dtLocationType = new DataTable();
            if (drpHOBranch.SelectedValue == "1")
            {
                txtHOBranchCode.Text = "00";
                lblHODealerID.Visible = false;
                drpHODealerName.Visible = false;

            }
            else
            {
                txtHOBranchCode.Text = "01";
                lblHOBranchDealerID.Text = "Select Branch Dealer";
                lblHODealerID.Visible = true;
                drpHODealerName.Visible = true;


                dtHO = objHOBranch.GetHODealer(Func.Convert.iConvertToInt(drpHOBranch.SelectedValue));
                if (dtHO != null)
                {
                    drpHODealerName.DataSource = dtHO;
                    drpHODealerName.DataTextField = "Name";
                    drpHODealerName.DataValueField = "ID";
                    drpHODealerName.DataBind();
                    drpHODealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                }

                else
                {
                    //drpHODealerName.DataSource = null;
                    //drpHODealerName.DataBind();
                    drpHODealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            // iHOBranchID = Func.Convert.iConvertToInt(drpHOBranch.SelectedValue);
            dtHOBranch = objHOBranch.GetHOBranchDealer(Func.Convert.iConvertToInt(drpHOBranch.SelectedValue), Type);
            if (dtHOBranch != null)
            {
                drpHOBranchDealerName.DataSource = dtHOBranch;
                drpHOBranchDealerName.DataTextField = "Name";
                drpHOBranchDealerName.DataValueField = "ID";
                drpHOBranchDealerName.DataBind();
                drpHOBranchDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
            }

            else
            {
                //drpHODealerName.DataSource = null;
                //drpHODealerName.DataBind();
                drpHOBranchDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
            }


            dtLocationType = objHOBranch.GetDealerLocationType(0);
            if (dtLocationType != null)
            {
                drpDealerLocationType.DataSource = dtLocationType;
                drpDealerLocationType.DataTextField = "Name";
                drpDealerLocationType.DataValueField = "ID";
                drpDealerLocationType.DataBind();
                drpDealerLocationType.Items.Insert(0, new ListItem("--Select--", "0"));
            }

            else
            {
                //drpHODealerName.DataSource = null;
                //drpHODealerName.DataBind();
                drpDealerLocationType.Items.Insert(0, new ListItem("--Select--", "0"));
            }



        }
        protected void drpHOBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCombo("New");

        }
        protected void btnHOBranchUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string sMessage = "";
                clsHOBranch objHOBranch = new clsHOBranch();
                DealerHOBranchCode = (string)ViewState["DlrHOBranchCode"];
                DealerHOCode = (string)ViewState["DlrHOCode"];

                DealerHOBranchID = Convert.ToInt32(ViewState["DlrHOBranchID"]);
                DealerHOID = Convert.ToInt32(ViewState["DlrHOID"]);

                if (drpHOBranch.SelectedValue == "1")
                {
                    if (objHOBranch.bSaveDealerHOBranchEntry(DealerHOBranchID, DealerHOBranchCode, DealerHOBranchID, DealerHOBranchCode, txtHOBranchCode.Text.Trim(), Func.Convert.iConvertToInt(drpDealerLocationType.SelectedValue)) == true)
                    {
                        //sMessage = "Dealer HO Entry updated successfully....!";
                        // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO Entry updated successfully....!');</script>");
                        // ScriptManager.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                        //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script language='javascript'>  alert('" + sMessage + ".');</script>", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ErrorMessage();", true);
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "<script language='javascript'>  alert('" + sMessage + ".');</script>", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO Entry updated successfully....!');</script>");
                    }
                }
                else
                {
                    if (objHOBranch.bSaveDealerHOBranchEntry(DealerHOID, DealerHOCode, DealerHOBranchID, DealerHOBranchCode, txtHOBranchCode.Text.Trim(), Func.Convert.iConvertToInt(drpDealerLocationType.SelectedValue)) == true)
                    {

                        // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO-Branch Entry updated successfully....!');</script>");
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO-Branch Entry updated successfully....!');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void drpHOBranchDealerName_SelectedIndexChanged(object sender, EventArgs e)
        {

            // DealerHOBranchName = drpHOBranchDealerName.SelectedItem.Text;
            HOBranchDealerName();


        }
        private void HOBranchDealerName()
        {
            if (drpHOBranchDealerName.SelectedIndex > 0)
            {
                string[] strarr = null;
                string str = drpHOBranchDealerName.SelectedValue;
                strarr = str.Split('#');
                //int dealercode_Final_HOBranch = DealerHOBranchName.IndexOf("1V");
                //if (dealercode_Final_HOBranch == -1)
                //    dealercode_Final_HOBranch = DealerHOBranchName.IndexOf("1S");
                //DealerHOBranchCode = DealerHOBranchName.Substring(dealercode_Final_HOBranch, 6);
                ViewState["DlrHOBranchCode"] = strarr[1];
                ViewState["DlrHOBranchID"] = strarr[0];

            }
        }
        protected void drpHODealerName_SelectedIndexChanged(object sender, EventArgs e)
        {

            HODealerName();

        }
        private void HODealerName()
        {
            if (drpHODealerName.SelectedIndex > 0)
            {
                string[] strarr_HO = null;
                string str_HO = "";
                str_HO = drpHODealerName.SelectedValue;
                strarr_HO = str_HO.Split('#');
                //DealerHOName = drpHODealerName.SelectedItem.Text;
                //int dealercode_Final_HO = DealerHOName.IndexOf("1V");
                //if (dealercode_Final_HO == -1)
                //    dealercode_Final_HO = DealerHOName.IndexOf("1S");
                //DealerHOCode = DealerHOName.Substring(dealercode_Final_HO, 6);
                ViewState["DlrHOCode"] = strarr_HO[1];
                ViewState["DlrHOID"] = strarr_HO[0];
                DealerHOID = Convert.ToInt32(ViewState["DlrHOID"]);


                clsHOBranch objHOBranch = new clsHOBranch();
                DataSet ds = new DataSet();
                ds = objHOBranch.GetBranchCode(DealerHOID);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtHOBranchCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Code"]);
                    }
                }

            }
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
            iID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();


        }
        private void GetDataAndDisplay()
        {
            try
            {
                clsHOBranch objHOBranch = new clsHOBranch();
                DataSet ds = new DataSet();
                if (iID != 0)
                {


                    ds = objHOBranch.GetHOBranchData(iID);
                    DisplayData(ds);
                    objHOBranch = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    objHOBranch = null;

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
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    // ClearDealerHeader();
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    drpDealerLocationType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerLocationType"]);
                    string HOBranchCode = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HOBranchCode"]);
                    if (HOBranchCode == "00")
                    {
                        drpHOBranch.SelectedValue = "1";
                    }
                    else
                    {
                        drpHOBranch.SelectedValue = "2";
                    }
                    FillCombo("All");


                    //151#1V1079
                    // drpHOBranchDealerName.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HODlr_Id"]) + "#" + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HODlr_Code"]);
                    if (HOBranchCode == "00")
                    {
                        drpHOBranchDealerName.SelectedValue = ds.Tables[0].Rows[0]["HODlr_Id"] + "#" + ds.Tables[0].Rows[0]["HODlr_Code"];
                    }
                    else
                    {
                        drpHOBranchDealerName.SelectedValue = ds.Tables[0].Rows[0]["Dlr_Id"] + "#" + ds.Tables[0].Rows[0]["Dlr_Code"];
                        drpHODealerName.SelectedValue = ds.Tables[0].Rows[0]["HODlr_Id"] + "#" + ds.Tables[0].Rows[0]["HODlr_Code"];
                    }
                    // drpHODealerName.SelectedValue = ds.Tables[0].Rows[0]["Dlr_Id"]) + "#" + Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dlr_Code"];



                }

                else
                {
                    //ClearDealerHeader();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;

                clsSupplier objSupplier = new clsSupplier();

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                    txtID.Text = "0";
                    iID = 0;
                    FillCombo("New");



                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {


                    string sMessage = "";
                    clsHOBranch objHOBranch = new clsHOBranch();

                    HOBranchDealerName();
                    HODealerName();

                    DealerHOBranchCode = (string)ViewState["DlrHOBranchCode"];
                    DealerHOCode = (string)ViewState["DlrHOCode"];

                    DealerHOBranchID = Convert.ToInt32(ViewState["DlrHOBranchID"]);
                    DealerHOID = Convert.ToInt32(ViewState["DlrHOID"]);

                    if (drpHOBranch.SelectedValue == "1")
                    {
                        if (objHOBranch.bSaveDealerHOBranchEntry(DealerHOBranchID, DealerHOBranchCode, DealerHOBranchID, DealerHOBranchCode, txtHOBranchCode.Text.Trim(), Func.Convert.iConvertToInt(drpDealerLocationType.SelectedValue)) == true)
                        {
                            //sMessage = "Dealer HO Entry updated successfully....!";
                            // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO Entry updated successfully....!');</script>");
                            // ScriptManager.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script language='javascript'>  alert('" + sMessage + ".');</script>", true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ErrorMessage();", true);
                            //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "<script language='javascript'>  alert('" + sMessage + ".');</script>", true);
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO Entry updated successfully....!');</script>");
                        }
                    }
                    else
                    {
                        if (objHOBranch.bSaveDealerHOBranchEntry(DealerHOID, DealerHOCode, DealerHOBranchID, DealerHOBranchCode, txtHOBranchCode.Text.Trim(), Func.Convert.iConvertToInt(drpDealerLocationType.SelectedValue)) == true)
                        {

                            // Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO-Branch Entry updated successfully....!');</script>");
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Dealer HO-Branch Entry updated successfully....!');</script>");
                        }
                    }




                    txtID.Text = Func.Convert.sConvertToString(iID);
                    FillSelectionGrid();

                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
    }
}