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

namespace MANART.Forms.Activity
{
    public partial class frmActivityMaster : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        private DataSet dsActType;
        int iUserId = 0;
        int iHOBr_id = 0;
        
        clsActivityHeads ObjActivity = new clsActivityHeads();
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                DataSet ds = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                
                iID = Func.Convert.iConvertToInt(txtID.Text);
                ds = ObjActivity.GetMaxActivity(0);
                SearchGrid.sGridPanelTitle = "List of Activity Master";
                if (!IsPostBack)
                {
                    FillActivity();
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                    //if (iID == 0)
                    //    iID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                    if (iID != 0)
                    {
                        GetDataAndDisplay();
                    }
                }
                FillSelectionGrid();
                if (txtUserType.Text == "3")
                {
                   // ToolbarC.Attributes.Add("display", "none");
                    ToolbarC.Visible = false;
                }
                else
                {
                   // ToolbarC.Attributes.Add("display", "");
                    ToolbarC.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillActivity()
        {
            try
            {
                clsDB objDB = new clsDB();
                dsActType = objDB.ExecuteStoredProcedureAndGetDataset("SP_Activity_FillDropdown", 0, "ActType");
                if (dsActType != null)
                {
                    ddlTypeoftheActivity.DataSource = dsActType.Tables[0];
                    ddlTypeoftheActivity.DataTextField = "Name";
                    ddlTypeoftheActivity.DataValueField = "ID";
                    ddlTypeoftheActivity.DataBind();
                    ddlTypeoftheActivity.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex) { }
        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
                iID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex) { }
        }

        private void GetDataAndDisplay()
        {
            try
            {
                clsActivityHeads ObjActivity = new clsActivityHeads();
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = ObjActivity.GetActivityMaster(iID);
                    DisplayData(ds);
                    ObjActivity = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    ObjActivity = null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayData(DataSet ds)
        {
            try
            {

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ClearDealerHeader();
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    ddlTypeoftheActivity.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Type_ID"]);
                    txtNameoftheActivity.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Activity_Name"]);
                    txtFromDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["From_Date"]);
                    txtToDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["To_Date"]);
                    txtGLCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GL_Code"]);
                    txtCostCenter.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Cost_Center"]);
                    string Active = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                    if (Active == "Y")
                    {
                        drpActive.SelectedValue = "1";
                    }
                    else
                    {
                        drpActive.SelectedValue = "2";
                    }

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

        private bool bValidateRecord()
        {
            clsDB objDB = new clsDB();
            int IsExist;
            iID = Func.Convert.iConvertToInt(txtID.Text);

            string sMessage = " ";
            bool bValidateRecord = true;
            if (txtNameoftheActivity.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Name of the Activity.";
                bValidateRecord = false;
            }
            else if (ddlTypeoftheActivity.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Type of Activity.";
                bValidateRecord = false;
            }
            else if (txtFromDate.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter From Date.";
                bValidateRecord = false;
            }
            else if (txtToDate.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter To Date.";
                bValidateRecord = false;
            }
            else if (txtGLCode.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter GL Code.";
                bValidateRecord = false;
            }
            else if (txtCostCenter.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Cost Center.";
                bValidateRecord = false;
            }
            IsExist = objDB.ExecuteStoredProcedure("SP_GetActivityMaster_Active", ddlTypeoftheActivity.SelectedValue, txtNameoftheActivity.Text);
            if (IsExist > 0 && iID == 0)
            {
                sMessage = sMessage + "\\n Name of the Activity Already Exist.";
                bValidateRecord = false;
            }



            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;

        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                clsActivityHeads ObjActivity = new clsActivityHeads();
                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    ClearDealerHeader();
                    txtID.Text = "0";
                    iID = 0;
                    //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord() == false) return;
                    iID = Func.Convert.iConvertToInt(txtID.Text);

                    iID = ObjActivity.bSaveActivityMaster(iID, Func.Convert.iConvertToInt(ddlTypeoftheActivity.SelectedValue), txtNameoftheActivity.Text, txtFromDate.Text, txtToDate.Text,
                       txtGLCode.Text, txtCostCenter.Text, drpActive.SelectedItem.Text);
                    if (iID > 0)
                    {
                        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);

                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    }
                    txtID.Text = Func.Convert.sConvertToString(iID);
                    FillSelectionGrid();

                }
                ObjActivity = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.AddToSearchCombo("Name of the Activity");
                SearchGrid.iDealerID = 1;
                SearchGrid.sSqlFor = "ListOfActivity";
                SearchGrid.iBrHODealerID = 1;
                
            }
            catch (Exception ex) { }
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                string strDisAbleBackButton;
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                btnReadonly();
            }
            catch (Exception ex) { }
        }
        // Function Use for Readonly User
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objCommon != null) objCommon = null;
            }
        }

        private void ClearDealerHeader()
        {
            try
            {
                ddlTypeoftheActivity.SelectedValue = "0";
                txtNameoftheActivity.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtGLCode.Text = "";
                txtCostCenter.Text = "";
                drpActive.SelectedValue = "1";
            }
            catch (Exception ex) { }
        }

        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iDealerID = Location.iDealerId;
                FillSelectionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }       
    }
}