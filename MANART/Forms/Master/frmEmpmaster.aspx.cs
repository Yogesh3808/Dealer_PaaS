using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Master
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private int iID;
        private DataSet dsState;
        int iUserId = 0;
        int iHOBr_id = 0;
        DataSet DSPrice = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                clsSupplier ObjSup = new clsSupplier();
                DataSet ds = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iID = Func.Convert.iConvertToInt(txtID.Text);
                ds = ObjSup.GetMaxEmp(iHOBr_id, iDealerID);
                if (!IsPostBack)
                {
                    FillEmpdelarcoad();
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
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
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void FillEmpdelarcoad()
        {
            try
            {
                clsDB objDB = new clsDB();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "EmpType");
                if (dsState != null)
                {
                    ddlemptype.DataSource = dsState.Tables[0];
                    ddlemptype.DataTextField = "Etype";
                    ddlemptype.DataValueField = "ID";
                    ddlemptype.DataBind();
                    ddlemptype.Items.Insert(0, new ListItem("--Select--", "0"));
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
                clsSupplier ObjDealer = new clsSupplier();
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = ObjDealer.GetEmployee(iID);
                    DisplayData(ds);
                    ObjDealer = null;
                }
                else   
                {
                    ds = null;
                    DisplayData(ds);
                    ObjDealer = null;
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
                    ddlemptype.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Empl_Type"]);
                    txtEmpnm.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Name"]);
                    txtMobile.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Contact_No"]);
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
           
                string sMessage = " ";
                bool bValidateRecord = true;
                if (txtEmpnm.Text == "")
                {
                    sMessage = sMessage + "\\n Please Enter Employee Name.";
                    bValidateRecord = false;
                }
                else if (ddlemptype.SelectedValue == "0")
                {
                    sMessage = sMessage + "\\n Please select the Employee Type.";
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
                clsSupplier objSupplier = new clsSupplier();
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
                    iID = objSupplier.bSaveEmplMaster(iID, iDealerID, iHOBr_id, txtEmpnm.Text, ddlemptype.SelectedValue, drpActive.SelectedItem.Text,txtMobile.Text);
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
                objSupplier = null;
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
                SearchGrid.AddToSearchCombo("Employee Name");
                SearchGrid.AddToSearchCombo("Contact No.");
                SearchGrid.AddToSearchCombo("Employee Type");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "EmplMaster";
                SearchGrid.iBrHODealerID = iHOBr_id;
                SearchGrid.sGridPanelTitle = "Employee Master List";
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
                ddlemptype.SelectedValue = "0";
                txtEmpnm.Text = "";
                txtMobile.Text = "";
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
    
       
