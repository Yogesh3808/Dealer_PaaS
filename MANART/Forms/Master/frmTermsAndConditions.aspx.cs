using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Data.SqlClient;
using System.Configuration;

namespace MANART.Forms.Master
{
    public partial class frmTermsAndConditions : System.Web.UI.Page
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
                ds = ObjSup.GetMaxTandC(iDealerID);
                if (!IsPostBack)
                {
                    FillTAndCDetails();
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
                   iID = objSupplier.bSaveTermsAndConditions(iID, iDealerID, txttermsandcondition.Text, ddltransction.SelectedValue, drpActive.SelectedItem.Text);
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

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.AddToSearchCombo("Transaction Name");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "TransAndCds";
                SearchGrid.sGridPanelTitle = "Terms And Condition List";
            }
            catch (Exception ex) { }
        }

        private bool bValidateRecord()
        {
            string sMessage = " ";
            bool bValidateRecord = true;
            if (txttermsandcondition.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Terms And Conditions.";
                bValidateRecord = false;
            }
            else if (ddltransction.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the Transaction Type.";
                bValidateRecord = false;
            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }

        private void ClearDealerHeader()
        {
            try
            {
                ddltransction.SelectedValue = "0";
                txttermsandcondition.Text = "";
                drpActive.SelectedValue = "1";
            }
            catch (Exception ex) { }
        }

        private void FillTAndCDetails()
        {
            try
            {
                clsDB objDB = new clsDB();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_TermsandCondition", "TransName");
                if (dsState != null)
                {
                    ddltransction.DataSource = dsState.Tables[0];
                    ddltransction.DataTextField = "TransactionName";
                    ddltransction.DataValueField = "ID";
                    ddltransction.DataBind();
                    ddltransction.Items.Insert(0, new ListItem("--Select--", "0"));
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
                    ds = ObjDealer.GetTAndCDetails(iID);
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
                    ddltransction.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Trans_ID"]);
                    txttermsandcondition.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TAndC"]);
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

        protected void ddltransction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                };
                txttermsandcondition.Text = "";
                string query = "select ID,TAndC from M_TandCdealerWise where Trans_ID='"+ ddltransction.SelectedValue +"' and Dealer_ID='" + Func.Convert.iConvertToInt(Session["iDealerID"]) +"'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                if (dt.Rows.Count != 0)
                {
                    txtID.Text = dt.Rows[0]["ID"].ToString();
                    txttermsandcondition.Text = dt.Rows[0]["TAndC"].ToString();
                }               
               
            }
            catch (Exception ex) { }
        }
    }
}