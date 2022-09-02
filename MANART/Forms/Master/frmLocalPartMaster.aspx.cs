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

namespace MANART.Forms.Master
{
    public partial class frmLocalPartMaster : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iPartID = 0;
        private int ID = 0;

        private string newactive;
        private string RateEditing_InDMS;
        int iMenuId = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                clsPart objPart = new clsPart();
                DataSet ds = new DataSet();
                // ds = objPart.GetMaxPart();
                if (!IsPostBack)
                {
                   // Func.Common.BindDataToCombo(drpUnit, clsCommon.ComboQueryType.Unit, 0, "");
                    clsDB objDB = new clsDB();
                    if (iMenuId == 698) // MTI User 
                    {
                        lblTitle.Text = "Local Part";
                        ds = objPart.GetMaxPart("99");
                    }
                    else if (iMenuId == 608)  //Dealer User
                    {
                        lblTitle.Text = "Local Part";
                        ds = objPart.GetMaxPart("99");
                    }
                    iPartID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iPartID == 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            iPartID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                            txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                        }
                    }
                    if (iPartID != 0)
                    {
                        GetDataAndDisplay();
                    }
                }

                FillSelectionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

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
        private void SetDocumentDetails()
        {
            //lblTitle.Text = " Chassis Header Details ";
            //lblDocNo.Text = "RFP No.:";
            //lblDocDate.Text = "RFP Date:";              
        }
        private void GetDataAndDisplay()
        {
            try
            {
                clsPart objPart = new clsPart();
                DataSet ds = new DataSet();
                //int iPartID = Func.Convert.iConvertToInt(txtID.Text);
                //iPartID = 1;
                if (iPartID != 0)
                {
                    if (iMenuId == 698) //MTI User 
                    {
                        ToolbarC.Visible = true;
                        ds = objPart.GetPart(iPartID, "99", iDealerID);

                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                        lblLocation.Visible = false;
                        txtLocation.Visible = false;
                        lblLocalPartName.Visible = false;
                        txtLocalPartName.Visible = false;
                    }
                    else if (iMenuId == 608) //Dealer User
                    {
                        ToolbarC.Visible = true;
                        ds = objPart.GetPart(iPartID, "99",iDealerID );
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                        lblLocation.Visible = true;
                        txtLocation.Visible = true;
                        lblLocalPartName.Visible = true;
                        txtLocalPartName.Visible = true;
                          txtPartNo.Enabled = false;
                          txtMTIPartName.Enabled = false;
                          txtBaseUnit.Enabled = false;
                        //    drpUnit.Enabled = false;
                          txtGroup.Enabled = false;
                          txtMinOrderQty.Enabled = false;
                          drpActive.Enabled = false;
                          txtPartCategory.Enabled = false;

                    }
                    DisplayData(ds);
                    objPart = null;
                }
                else
                {
                    ds = null;
                    objPart = null;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }


        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                iPartID = Func.Convert.iConvertToInt(txtID.Text);
                // ViewState["PartID"] = iPartID;
                GetDataAndDisplay();
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
                string groupCode = "";

                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //Display Header    
                if (ds.Tables[0].Rows.Count > 0)
                {

                    txtPartNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["parts_no"]);
                    txtMTIPartName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["part_name"]);
                    txtLocalPartName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LocalPartName"]);
                    txtBaseUnit.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Base Unit"]);
                   // drpUnit.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["UnitId"]);
                    txtGroup.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gr_Name"]);
                    txtMinOrderQty.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["MOQ"]);
                    txtPartCategory.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartCategory"]);
                    txtLocation.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Location"]);
                    newactive = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (newactive == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else if (newactive == "N")
                    {
                        drpActive.SelectedValue = "2";
                    }
                    groupCode = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Gr_No"]);

                    //if (groupCode == "99")//saving and editing allowed only local part 
                    //{
                    //    txtPartNo.Enabled = true;
                    //    txtMTIPartName.Enabled = true;
                    //    //txtBaseUnit.Text = "Ltrs.";
                    //    drpUnit.Enabled = true;
                    //    txtGroup.Enabled = true;
                    //    txtMinOrderQty.Enabled = true;
                    //    drpActive.Enabled = true;
                    //    txtPartCategory.Enabled = true;

                    //}
                    //else
                    //{
                    //    txtPartNo.Enabled = false;
                    //    txtMTIPartName.Enabled = false;
                    //    //txtBaseUnit.Text = "Ltrs.";
                    //    drpUnit.Enabled = false;
                    //    txtGroup.Enabled = false;
                    //    txtMinOrderQty.Enabled = false;
                    //    drpActive.Enabled = false;
                    //    txtPartCategory.Enabled = false;

                    //}



                }
                else
                {
                    ClearDealerHeader();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void ClearDealerHeader()
        {
            txtPartNo.Text = "";
            txtMTIPartName.Text = "";
           // txtBaseUnit.Text = "Ltrs.";
            txtBaseUnit.Text = "";
          //  drpUnit.SelectedValue = "0";
            txtGroup.Text = "Local Part";
            txtMinOrderQty.Text = "";
            drpActive.SelectedValue = "1";
            txtPartCategory.Text = "";
            

        }

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.bGridFillUsingSql = false;
                SearchGrid.AddToSearchCombo("Part No");
                SearchGrid.AddToSearchCombo("Part Name");
                SearchGrid.AddToSearchCombo("Group");
                SearchGrid.AddToSearchCombo("Part Category");
                SearchGrid.iDealerID = iDealerID;

                if (iMenuId == 698)
                {
                    SearchGrid.sModelPart = "99";
                    SearchGrid.sGridPanelTitle = "Local Part List";
                    SearchGrid.iDealerID = 1;
                }
                else if (iMenuId == 608)
                {
                    SearchGrid.sModelPart = "99";
                    SearchGrid.sGridPanelTitle = "Local Part List";
                }
                SearchGrid.sSqlFor = "PartMaster";
                SearchGrid.iBrHODealerID = iHOBr_id;




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
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                clsPart objPart = new clsPart();
                ImageButton ObjImageButton = (ImageButton)sender;

                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {

                    txtID.Text = "0";
                    ClearDealerHeader();

                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {


                    ID = Func.Convert.iConvertToInt(txtID.Text);
                    if (iMenuId == 698)
                    {
                        ID = objPart.bSaveLocalPartMaster_ForMTI(ID, txtPartNo.Text.Trim(), txtMTIPartName.Text.Trim(),
                            txtBaseUnit.Text  , drpActive.SelectedItem.Text, txtMinOrderQty.Text, txtPartCategory.Text.Trim());
                    }
                    else if (iMenuId == 608)
                    {
                        ID = objPart.bSaveLocalPartMaster(ID, txtLocation.Text.Trim(), iDealerID, "99", txtLocalPartName.Text.Trim());
                    }

                    
                   
                    if (ID > 0)
                    {
                        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);

                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }

                    txtID.Text = Func.Convert.sConvertToString(ID);
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