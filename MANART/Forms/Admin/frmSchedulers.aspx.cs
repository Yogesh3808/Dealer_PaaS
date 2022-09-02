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
using MANART_BAL;
using MANART_DAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Threading;
using System.Globalization;
using System.IO;

namespace MANART.Forms.Admin
{
    public partial class frmSchedulers : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iDealerLocationTypeID = 0;
        string sDealerId = "";
        int iUserId = 0;
        int iMenuId = 0;
        string sDealerCode = "";
        clsDealerUtility objdealUti;
        string strPatchName = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            Fillcombo();
          
            FillSelectionGrid();
            Session["IsDealerView"] = "Yes";

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {

             

                FillJobGrid();
                int UsreType = (Session["UserType"] != null) ? Func.Convert.iConvertToInt(Session["UserType"]) : 0;
                int UserRole = (Session["UserRole"] != null) ? Func.Convert.iConvertToInt(Session["UserRole"]) : 0;
                int UserID = (Session["UserID"] != null) ? Func.Convert.iConvertToInt(Session["UserID"]) : 0;
                // Condition Added for Eicher Admin Login
                if (UsreType == 5 && UserRole == 8 && UserID != 8)
                {
                    Menu1.Items[0].Selected = true;
                    //Menu1.Items.Remove(new MenuItem("Patch Management", "1"));
                    //Menu1.Items.Remove(new MenuItem("Dealer Live Entry", "2"));
                    //Menu1.Items.RemoveAt(0);
                    //Menu1.Items.RemoveAt(2);
                    //Menu1.Items.RemoveAt(0);
                    //Menu1.Items.RemoveAt(2);
                
                    //Panel3.Style.Add("display", "none");
                    //GridView2.Style.Add("display", "none");
                    MultiView1.ActiveViewIndex = 0;

                }
            }


        }
        private void Fillcombo()
        {
            //clsDB objDB = new clsDB();
            //DataSet DsProcessName = new DataSet();
            //try
            //{

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            //finally
            //{
            //    if (objDB != null) objDB = null;
            //}


        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
        }
        // set Document text 
      
        // FillCombo
        private void FillCombo()
        {
            // Func.Common.BindDataToCombo(drpPaymentTerms, clsCommon.ComboQueryType.PaymentTerms, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpIncoTerms, clsCommon.ComboQueryType.INCOTerms, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpPortofDischarge, clsCommon.ComboQueryType.PortofDischargeForCountry, Location.iCountryId);
            //Func.Common.BindDataToCombo(drpModeofShipment, clsCommon.ComboQueryType.ModeofDispatch, Location.iDealerId);
            //Func.Common.BindDataToCombo(drpNominatedAgency, clsCommon.ComboQueryType.NominatedAgency, 0);
            //Func.Common.BindDataToCombo(drpThirdPartyInspectionAgency, clsCommon.ComboQueryType.ThirdPartyInspectionAgency, 0);
        }

        // to create Emty Row To Grid
        

        //BindData to Grid
     
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        private bool bValidateRecord()
        {
            
            return false;
        }
        //ToSave Record
   
        // To Clear The form data
       
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {

        
         
            GetDataAndDisplay();
        }
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
       
        

      
       

     
       
  
   
        private void FillSelectionGrid()
        {
           

        }
        private void DisplayControl(bool bDisplay)
        {

        }
      
        protected void Menu1_MenuItemClick1(object sender, MenuEventArgs e)
        {
            MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
            //Int16 i;
            //for (i = 0; i < Menu1.Items.Count - 1; i++)
            //{ 
            //    if
            //}

            //Make the selected menu item reflect the correct imageurl
            //For i = 0 To Menu1.Items.Count - 1
            //    If i = e.Item.Value Then
            //        Menu1.Items(i).ImageUrl = "selectedtab.gif"
            //    Else
            //        Menu1.Items(i).ImageUrl = "unselectedtab.gif"
            //    End If
            //Next
        }

     
        private void GetDataAndDisplay()
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            DataSet ds = new DataSet();
            if (iDealerID != 0)
            {
             
                ds = ObjDealerutilitty.GetDeatlerDeatilsforLive(iDealerID, iDealerLocationTypeID);
                if (ds != null)
                {
                    ViewState["ISlive"] = null;
                    ViewState["ISPartialylive"] = null;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                 
                        ViewState["ISPartialylive"] = ds.Tables[1].Rows[0]["partialy_live"].ToString();
                 
                        ViewState["ISlive"] = ds.Tables[1].Rows[0]["Dealer_Live"].ToString();
                 
                    }
                }
                ObjDealerutilitty = null;
            }

            ds = null;
            ObjDealerutilitty = null;
          
        }

      
        private void FillJobGrid()
        {
            DataSet ds = new DataSet();
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            ds = ObjDealerutilitty.FillSQLJobDeatils();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView2.DataSource = ds.Tables[0];
                GridView2.DataBind();
            }

        }
        protected void btnbtnSQLJOBDisable(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {

                Button lblJoStatus = sender as Button;
                GridViewRow row = lblJoStatus.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblEnabled");
                Label lblJobName = (Label)row.FindControl("lblname");
                if (lblpatchstatus.Text.Trim() == "Disabled")
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Can not update disable status. This JOb Already Disable Mode.!');</script>");

                }
                else
                {
                    if (ObjDealerutilitty.bUpdateSqlJobStatus(lblJobName.Text.Trim(), 0) == true)
                    {
                        Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                        FillJobGrid();
                    }
                }

            }
            ObjDealerutilitty = null;
        }
        protected void btnbtnSQLJOBEnable(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {

                Button lblJoStatus = sender as Button;
                GridViewRow row = lblJoStatus.NamingContainer as GridViewRow;


                Label lblpatchstatus = (Label)row.FindControl("lblEnabled");
                Label lblJobName = (Label)row.FindControl("lblname");
                if (lblpatchstatus.Text.Trim() == "Enabled")
                {

                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Can not update Enable status. This Job Already Enable Mode.!');</script>");

                }
                else
                {
                    if (ObjDealerutilitty.bUpdateSqlJobStatus(lblJobName.Text.Trim(), 1) == true)
                    {
                        Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                        FillJobGrid();
                    }
                }

            }
            ObjDealerutilitty = null;
        }
   
        private int MagicFindFileCount(string strDirectory, string strFilter)
        {
            int nFiles = 0;
            if (Directory.Exists(strDirectory))
            {
                System.IO.DirectoryInfo dire;
                nFiles = Directory.GetFiles(strDirectory, strFilter).Length;
                dire = new System.IO.DirectoryInfo(strDirectory);
                nFiles = dire.GetFiles(strFilter).Length;
                //foreach (String dir in Directory.GetDirectories(strDirectory))
                //{
                //    //nFiles += Directory.GetFiles(dir, strFilter).Length;
                //    dire = new System.IO.DirectoryInfo(dir + "\\");
                //    nFiles += dire.GetFiles(strFilter).Length;   
                //}
            }
            return nFiles;
        }
        private int MagicFindFileCountForSubDirectory(string strDirectory, string strFilter)
        {
            int nFiles = 0;
            if (Directory.Exists(strDirectory))
            {
                string[] files = System.IO.Directory.GetFiles(strDirectory, "*.xml", System.IO.SearchOption.AllDirectories);


                ////nFiles = Directory.GetFiles(strDirectory, strFilter).Length;
                ////dire = new System.IO.DirectoryInfo(strDirectory);
                ////nFiles = dire.GetFiles(strFilter).Length;
                //foreach (String dir in Directory.GetDirectories(strDirectory))
                //{
                //    System.IO.DirectoryInfo dire;
                //    dire = new System.IO.DirectoryInfo(@"C:\DataExchange\DCS\FileIn\48");
                //    nFiles =nFiles+ dire.GetFiles(strFilter).Length;
                //    dire = null;
                //}
                nFiles = files.Length;
            }
            return nFiles;
        }

        protected void btnEnableAll_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {

                if (ObjDealerutilitty.bUpdateSqlJobStatus("All1", 1) == true)
                {
                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                    FillJobGrid();
                }
            }
            ObjDealerutilitty = null;
        }

        protected void btnDisableAll_Click(object sender, EventArgs e)
        {
            clsDealerUtility ObjDealerutilitty = new clsDealerUtility();
            if (sender != null)
            {
              
                if (ObjDealerutilitty.bUpdateSqlJobStatus("All1", 0) == true)
                {
                    Page.RegisterStartupScript("CloseDownloadStatusSingleOne1", "<script language='javascript'>alert('Job Status Updated successfully...!');</script>");
                    FillJobGrid();
                }
            }
            ObjDealerutilitty = null;
        }
      
        
    }
}