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
using AjaxControlToolkit;
using MANART_BAL;
using MANART_DAL;
using System.IO;
using System.Drawing;
using System.Data.OleDb;

namespace MANART.Forms.Spares
{
    public partial class frmPartsTarget : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private int iDealerID = 0;
        private string sDealerID = "";
        private int iYearID = 0;
        int iMenuId = 0;
        string cType;
        int iUserId = 0;
        int iUserType = 0;
        string sDealerIds = "";
        string DealerOrigin = "";
        private string sUserRole = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                sUserRole = Func.Convert.sConvertToString(Session["UserRole"]);
                //  Location.b.bHideDealerDetails = false;
                Location.bUseSpareDealerCode = true;
                //if (iMenuId == 235 || iMenuId == 272)
                //{
                //    cType = "V";
                //    lblVehSpr.Text = "V";
                if (iMenuId == 174)
                {
                    GrdAnnualTarget.Columns[2].HeaderText = "Distributor Code";
                    GrdAnnualTarget.Columns[3].HeaderText = "Distributor Name";
                    GrdAnnualTarget.Columns[5].Visible = false;
                    DealerOrigin = "E";

                }
                else
                {
                    DealerOrigin = "D";
                }
                if (iMenuId == 697)// Dealer Display
                {
                    Location.Visible = false;
                    Location1.Visible = true;
                    tdSelectFile.Style.Add("display", "none");
                    tdUploadFile.Style.Add("display", "none");
                }
                else if (sUserRole == "3")
                {
                    Location.Visible = true;
                    Location1.Visible = false;
                    tdSelectFile.Style.Add("display", "none");
                    tdUploadFile.Style.Add("display", "none");
                }
                else
                {
                    Location.Visible = true;
                    Location1.Visible = false;
                }
                ToolbarC.Visible = false;
                //}
                //else
                //{
                //    cType = "S";
                //    lblVehSpr.Text = "S";
                //}
                //if (Location.sDealerId == "")
                //    // sDealerIds = Func.Common.GetDealersByUserID(iUserId);
                //    sDealerIds = "0";
                //else
                sDealerIds = Location.sDealerId;
                //GetCurrentYear();
                iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);


                if (!IsPostBack)
                {
                    //sDealerIds = Location.sDealerId;
                    //iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);


                    //vrushali14032011_Begin
                    //iDealerID = Location.iDealerId;
                    //sDealerID = Location.iDealerId;
                    //vrushali14032011_End
                }

                //sDealerID = Func.Convert.sConvertToString(Location.iDealerId);
                //iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);
                //Location.DDLSelectedIndexChanged += new EventHandler(Location_DDLSelectedIndexChanged);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.bUseImgOrButton = true;

                if (!IsPostBack)
                {
                    Session["ModelDetails"] = null;
                }
                //vrushali14032011_Begin
                //iDealerID = Location.iDealerId;
                //sDealerID = Location.sDealerId;
                //vrushali14032011_End
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                // DisplayCurrentRecord();       
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
                iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            FillCombo();
        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            FillCombo();
        }
        //ToValidate Record
        private bool bValidateRecord()
        {
            string sMessage = "";

            bool bValidateRecord = true;


            if (drpYear.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Select the Year.";
                bValidateRecord = false;
            }

            if (DateTime.Today.Year != Func.Convert.iConvertToInt(drpYear.SelectedItem.Text))
            {
                sMessage = sMessage + "\\n You Can not Edit Other Than Current Year Data.";
                bValidateRecord = false;
            }

            if (GrdAnnualTarget.Rows.Count == 0)
            {
                sMessage = sMessage + "\\n Please Link at least one spare budget To Dealer.";
                bValidateRecord = false;

            }
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                return false;
            }


            bool bCheckDetailsExist = false;
            bool bCheckDetailsRetailExist = true;
            for (int iRowCnt = 0; iRowCnt < GrdAnnualTarget.Rows.Count; iRowCnt++)
            {
                Double iYearValue, iQ1L1Value, iQ2L1Value, iQ3L1Value, iQ4L1Value;
                Double iRetailValue;
                iYearValue = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblAnnualBugPlan") as TextBox).Text);
                iQ1L1Value = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L1BugPlan") as TextBox).Text);
                iQ2L1Value = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ2L1BugPlan") as TextBox).Text);
                iQ3L1Value = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ3L1BugPlan") as TextBox).Text);
                iQ4L1Value = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ4L1BugPlan") as TextBox).Text);
                if (Func.Convert.iConvertToInt(Session["UserType"]) == 2)
                {
                    iRetailValue = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q1BugPlan") as TextBox).Text);

                    if (iRetailValue > 0.00)
                    {
                        bCheckDetailsRetailExist = true;

                    }
                }


                if (iYearValue > 0.00)
                {
                    bCheckDetailsExist = true;

                    if (iYearValue < iQ1L1Value + iQ2L1Value + iQ3L1Value + iQ4L1Value)
                    {
                        Page.RegisterStartupScript("Close1", "<script language='javascript'>alert('Sum of All Quater L1 value must be less than or equal to Annual L1 value.');</script>");
                        bValidateRecord = false;
                        break;
                    }
                }
            }

            if (Func.Convert.iConvertToInt(Session["UserType"]) == 2)
            {
                if (bCheckDetailsRetailExist == false)
                {
                    Page.RegisterStartupScript("Close1", "<script language='javascript'>alert('Enter Retail Target For Atleast One Dealer');</script>");
                    bValidateRecord = false;
                }


            }

            if (bCheckDetailsExist == false)
            {
                if (Func.Convert.iConvertToInt(Session["UserType"]) == 1)
                {
                    Page.RegisterStartupScript("Close1", "<script language='javascript'>alert('Enter Annual Target For Atleast One Distributor');</script>");
                    bValidateRecord = false;

                }
                else
                {
                    Page.RegisterStartupScript("Close1", "<script language='javascript'>alert('Enter Annual Target For Atleast One Dealer');</script>");
                    bValidateRecord = false;


                }
            }

            return bValidateRecord;

        }
        //ToSave Record
        private bool bSaveRecord(bool bSaveWithConfirm)
        {
            try
            {
                if (bValidateRecord() == false)
                {
                    return false;
                }
                clsSparesTarget objSpaTargt = new clsSparesTarget();

                string sTargetFlg = "";


                FillDetailsFromGrid(ref sTargetFlg);

                //vrushali14032011_Begin
                //if (objVehTargt.bSaveRecordWithModel(cType, iDealerID, iYearID, dtDetails) == true)
                if (objSpaTargt.bAnnualTarget(dtDetails) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    return false;
                }
                objSpaTargt = null;
                return true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }
        }
        //Fill Details From Grid
        private void FillDetailsFromGrid(ref string sTargetFlg)
        {
            DataRow dr;
            dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("Dealer_Code", typeof(string)));
            dtDetails.Columns.Add(new DataColumn("Year_Id", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("YTD_Target", typeof(decimal)));

            dtDetails.Columns.Add(new DataColumn("T1_Q1", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q1L1", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q1L2", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q1L3", typeof(decimal)));

            dtDetails.Columns.Add(new DataColumn("T1_Q2", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q2L1", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q2L2", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q2L3", typeof(decimal)));

            dtDetails.Columns.Add(new DataColumn("T1_Q3", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q3L1", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q3L2", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q3L3", typeof(decimal)));

            dtDetails.Columns.Add(new DataColumn("T1_Q4", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q4L1", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q4L2", typeof(decimal)));
            dtDetails.Columns.Add(new DataColumn("Q4L3", typeof(decimal)));

            for (int iRowCnt = 0; iRowCnt < GrdAnnualTarget.Rows.Count; iRowCnt++)
            {

                dr = dtDetails.NewRow();
                //dr["SRNo"] = "1";            
                dr["Dealer_Code"] = Func.Convert.sConvertToString((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblDealerCode") as Label).Text);
                dr["Year_Id"] = Func.Convert.iConvertToInt(drpYear.SelectedItem.Value);
                dr["YTD_Target"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblAnnualBugPlan") as TextBox).Text);

                dr["T1_Q1"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q1BugPlan") as TextBox).Text);
                dr["Q1L1"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L1BugPlan") as TextBox).Text);
                dr["Q1L2"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L2BugPlan") as TextBox).Text);
                dr["Q1L3"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L3BugPlan") as TextBox).Text);

                dr["T1_Q2"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q2BugPlan") as TextBox).Text);
                dr["Q2L1"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ2L1BugPlan") as TextBox).Text);
                dr["Q2L2"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ2L2BugPlan") as TextBox).Text);
                dr["Q2L3"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ2L3BugPlan") as TextBox).Text);

                dr["T1_Q3"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q3BugPlan") as TextBox).Text);
                dr["Q3L1"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ3L1BugPlan") as TextBox).Text);
                dr["Q3L2"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ3L2BugPlan") as TextBox).Text);
                dr["Q3L3"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ3L3BugPlan") as TextBox).Text);

                dr["T1_Q4"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q4BugPlan") as TextBox).Text);
                dr["Q4L1"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ4L1BugPlan") as TextBox).Text);
                dr["Q4L2"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ4L2BugPlan") as TextBox).Text);
                dr["Q4L3"] = Func.Convert.dConvertToDouble((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ4L3BugPlan") as TextBox).Text);

                dtDetails.Rows.Add(dr);
                dtDetails.AcceptChanges();

            }
        }
        private void GetCurrentYear()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsCurYear = new DataSet();
                dsCurYear = objDB.ExecuteQueryAndGetDataset("select Id from M_Year where Year=" + DateTime.Now.Year.ToString());

                if (dsCurYear.Tables[0].Rows.Count > 0)
                {
                    drpYear.SelectedValue = dsCurYear.Tables[0].Rows[0]["Id"].ToString();
                }
            }
            catch
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                string strDisAbleBackButton;
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                if (!IsPostBack)
                {
                    FillCombo();
                    GetCurrentYear();
                    iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);
                    DisplayCurrentRecord();
                }
                btnReadonly();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

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
        protected void DetailsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpYear, clsCommon.ComboQueryType.YearCode, 0);
            clsUser objUser = new clsUser();
            DataSet dsState = new DataSet();
            dsState = null;
            chkDealer.Items.Clear();
            if (Func.Convert.iConvertToInt(Session["UserType"]) == 2)
            {
                //dsState = objUser.FillDealerForState(Func.Convert.iConvertToInt(Session["UserID"]), Location.iRegionId, Location.iCountryId, "N");
                dsState = objUser.FillDealerForState(Func.Convert.iConvertToInt(Session["UserID"]), Location.iRegionId, Location.iCountryId);
            }
            else if (Func.Convert.iConvertToInt(Session["UserType"]) == 1)
                dsState = objUser.FillDealerForCountry(Func.Convert.iConvertToInt(Session["UserID"]), Location.iRegionId, Location.iCountryId);
            if (dsState != null)
                if (dsState.Tables[dsState.Tables.Count - 1].Rows.Count > 0)
                {
                    chkDealer.DataSource = dsState.Tables[dsState.Tables.Count - 1];
                    chkDealer.DataTextField = "SName";
                    chkDealer.DataValueField = "ID";
                    chkDealer.DataBind();
                }
        }
        private void DisplayCurrentRecord()
        {
            try
            {
                DataTable dtDetails = null;
                DataSet ds = new DataSet();
                if (iMenuId == 697)
                    sDealerIds = Session["iDealerID"].ToString();
                else
                    sDealerIds = Location.sDealerId;

                if (sDealerIds == "")
                    sDealerIds = "0";

                iYearID = Func.Convert.iConvertToInt(drpYear.SelectedValue);
                if (iYearID != 0)
                {
                    clsSparesTarget objSparesTarget = new clsSparesTarget();

                    ds = objSparesTarget.GetSparesTargetAnnualMaster(sDealerIds, iYearID);
                    if (ds != null)
                    {
                        if (Func.Common.iTableCntOfDatatSet(ds) > 0)
                        {
                            dtDetails = ds.Tables[0];
                            Session["ModelDetails"] = dtDetails;
                            BindDataToGrid();
                            objSparesTarget = null;
                            dvbudplan.Style.Add("display", "");
                        }
                        else
                        {
                            dtDetails = ds.Tables[0];
                            Session["ModelDetails"] = dtDetails;
                            BindDataToGrid();
                            dvbudplan.Style.Add("display", "");
                        }
                    }
                    else
                    {
                        ds = null;
                        dtDetails = null;
                        Session["ModelDetails"] = dtDetails;
                        BindDataToGrid();
                        dvbudplan.Style.Add("display", "none");
                    }

                }
                else
                {
                    ds = null;
                    dtDetails = null;
                    Session["ModelDetails"] = dtDetails;
                    BindDataToGrid();
                    dvbudplan.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGrid()
        {
            try
            {
                if (Session["ModelDetails"] == null)
                    Session["ModelDetails"] = dtDetails;
                else
                    dtDetails = (DataTable)Session["ModelDetails"];

                GrdAnnualTarget.DataSource = dtDetails;
                GrdAnnualTarget.DataBind();
                // GridControlEnableDisable();
                //SetGridControlProperty();
                //GridColShowHide();
                GrdAnnualTarget.Enabled = false;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void GridControlEnableDisable()
        {
            try
            {
                string AllowEdit;
                //CheckBox chk = new CheckBox();
                TextBox txt = new TextBox();

                for (int iRowCnt = 0; iRowCnt < GrdAnnualTarget.Rows.Count; iRowCnt++)
                {
                    AllowEdit = Func.Convert.sConvertToString((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblStatus") as Label).Text);

                    if (AllowEdit == "N")
                    {
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtRetail") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtOrdrIntNo") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtBlPsNo") as TextBox).Enabled = false;
                    }
                    else
                        if (AllowEdit == "Y")
                        {
                            if (DateTime.Today.Year != Func.Convert.iConvertToInt(drpYear.SelectedItem.Text))
                            {
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty") as TextBox).Enabled = false;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty") as TextBox).Enabled = false;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtRetail") as TextBox).Enabled = false;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtOrdrIntNo") as TextBox).Enabled = false;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtBlPsNo") as TextBox).Enabled = false;
                            }
                            else
                            {
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty") as TextBox).Enabled = true;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty") as TextBox).Enabled = true;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtRetail") as TextBox).Enabled = true;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtOrdrIntNo") as TextBox).Enabled = true;
                                (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtBlPsNo") as TextBox).Enabled = true;

                                //(GrdAnnualTarget.Rows[iRowCnt].FindControl("txtOrdrIntNo") as TextBox).MaxLength = 6;
                                //(GrdAnnualTarget.Rows[iRowCnt].FindControl("txtBlPsNo") as TextBox).MaxLength = 6;


                                //Set the textbox length.
                                if (cType == "V")
                                {
                                    //  (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty") as TextBox).MaxLength = 6;
                                    //(GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty") as TextBox).MaxLength = 6;
                                    (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtRetail") as TextBox).MaxLength = 0;
                                }
                                else
                                {
                                    //(GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty") as TextBox).MaxLength = 11;
                                    //(GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty") as TextBox).MaxLength = 11;
                                    (GrdAnnualTarget.Rows[iRowCnt].FindControl("txtRetail") as TextBox).MaxLength = 11;
                                }

                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void GridColShowHide()
        {
            try
            {
                if (DateTime.Today.Year != Func.Convert.iConvertToInt(drpYear.SelectedItem.Text))
                {
                    btnSave.Visible = false;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                else
                {
                    btnSave.Visible = false;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                }


                if (cType == "V")
                {
                    lblTitle.Text = "Vehicle Budget Plan";
                    GrdAnnualTarget.Columns[3].Visible = true;
                    GrdAnnualTarget.Columns[4].Visible = false;
                    GrdAnnualTarget.Columns[7].Visible = false;
                    GrdAnnualTarget.Columns[5].HeaderText = "Annual Target T1(Qty)";
                    GrdAnnualTarget.Columns[6].HeaderText = "Annual Target T2(Qty)";

                }

                else
                    if (cType == "S")
                    {
                        lblTitle.Text = "Annual Spares Target";
                        GrdAnnualTarget.Columns[3].Visible = false;
                        GrdAnnualTarget.Columns[4].Visible = false;
                        GrdAnnualTarget.Columns[7].Visible = true;
                        GrdAnnualTarget.Columns[5].HeaderText = "Annual Target OffTakeL1(Amt)";
                        GrdAnnualTarget.Columns[6].HeaderText = "Annual Target OffTakeL2(Amt)";
                    }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void SetGridControlProperty()
        {
            try
            {
                int YearCd = 0;
                string sDealerID = "";
                for (int iRowCnt = 0; iRowCnt < GrdAnnualTarget.Rows.Count; iRowCnt++)
                {
                    sDealerID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["DealerID"]);

                    YearCd = Func.Convert.iConvertToInt(drpYear.SelectedItem.Text);
                    if (YearCd == Func.Convert.iConvertToInt(DateTime.Now.Year.ToString()))
                    {
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("lblAnnualBugPlan") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("lblT1_Q1BugPlan") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L1BugPlan") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L2BugPlan") as TextBox).Enabled = false;
                        (GrdAnnualTarget.Rows[iRowCnt].FindControl("lblQ1L3BugPlan") as TextBox).Enabled = false;
                    }


                    ////ImageButton tmpImgBtn = (ImageButton)GrdAnnualTarget.Rows[iRowCnt].FindControl("ImgSelect");
                    ////int iModelId = 0;
                    ////iModelId = Func.Convert.iConvertToInt((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblModId") as Label).Text);

                    ////string sModelName;
                    ////sModelName = Func.Convert.sConvertToString((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblModelName") as Label).Text);

                    ////int YearCd;
                    ////YearCd = Func.Convert.iConvertToInt(drpYear.SelectedItem.Text);

                    ////string sDealerCd;

                    //////vrushali15032011_Begin
                    //////sDealerCd = Func.Convert.sConvertToString(Location.sDealerId.ToString());

                    ////sDealer_ID = Func.Convert.sConvertToString((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblDealerID") as Label).Text);
                    ////sDealerCd = Func.Convert.sConvertToString((GrdAnnualTarget.Rows[iRowCnt].FindControl("lblDealerName") as Label).Text);
                    //////vrushali15032011_End

                    //////vrushali14032011_Begin
                    //////tmpImgBtn.Attributes.Add("onClick", "ShowMonthTarget(this," + iDealerID + "," + iModelId + "," + iYearID + ",'" + sDealerCd + "','" + sModelName + "','" + YearCd + "'," + iMenuId + ");");
                    ////tmpImgBtn.Attributes.Add("onClick", "ShowMonthTarget(this," + sDealer_ID + "," + iModelId + "," + iYearID + ",'" + sDealerCd + "','" + sModelName + "','" + YearCd + "'," + iMenuId + ");");
                    //////vrushali14032011_End
                    ////TextBox txtT1Qty = (TextBox)GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT1Qty");
                    ////TextBox txtT2Qty = (TextBox)GrdAnnualTarget.Rows[iRowCnt].FindControl("txtT2Qty");

                    //----------------- Commented by shyamal it is using for vehicle =-----------------
                    ////TextBox txtYearlyValue = (TextBox)GrdAnnualTarget.Rows[iRowCnt].FindControl("lblAnnualBugPlan");
                    ////if (lblVehSpr.Text == "V")//Vehicle
                    ////{
                    ////    txtYearlyValue.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'5')");

                    ////}
                    ////else
                    ////{
                    ////  //  txtT1Qty.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6')");
                    ////   // txtT2Qty.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6')");
                    ////}
                    //------------------ Shyamal -------------------------
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            // if (bSaveRecord(false) == false) return;         
            // DisplayCurrentRecord();
            //}
            //catch
            //{

            //}
        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false) == false) return;
                    DisplayCurrentRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Comment by Vikram
        //protected void GrdAnnualTarget_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    LinkButton lnkSelectPart = (LinkButton)row.FindControl("lnkSelectPart");
        //    Label lblDealerID = (Label)row.FindControl("lblDealerID");
        //    Label lblDealerName = (Label)row.FindControl("lblDealerName");
        //    Label lblDealerCode = (Label)row.FindControl("lblDealerCode");
        //    TextBox lblAnnualBugPlan = (TextBox)row.FindControl("lblAnnualBugPlan");
        //    // Label lblModel_cat_Id = (Label)row.FindControl("lblModel_cat_Id");
        //    // Label lblModel_cat_Name = (Label)row.FindControl("lblModel_cat_Name");
        //    //  Label lblTonnage_ID = (Label)row.FindControl("lblTonnage_ID");
        //    //Label lblTonnage_Name = (Label)row.FindControl("lblTonnage_Name");
        //    //take  value In Session
        //    //Session["dlrCode"] = Location.sDealerCode;

        //    ////Session["dlrCode"] = lblDealerCode.Text; 
        //    ////Session["DealerName"] = lblDealerName.Text;

        //    // Session["Mod_Cat_Code"] = lblModel_cat_Name.Text;
        //    // Session["Tonnage"] = lblTonnage_Name.Text;

        //    ////Session["Year"] = drpYear.SelectedItem.Text;

        //    //lnkSelectPart.Attributes.Add("onClick", "ShowMonthTarget1(this,'" + lblDealerID.Text + "','" + lblModel_cat_Id.Text + "','" + drpYear.SelectedValue + "','" + lblTonnage_ID.Text + "');");
        //    // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMonthTarget1(this,'" + lblDealerID.Text + "','" + lblModel_cat_Id.Text + "','" + drpYear.SelectedValue + "','" + lblTonnage_ID.Text +"','"+ cType  + "');</script>");
        //    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMonthTarget1(this,'" + lblDealerID.Text + "','" + drpYear.SelectedValue + "','" + lblAnnualBugPlan.Text + "','" + lblDealerName.Text + "','" + lblDealerCode.Text + "','" + drpYear.SelectedItem.Text + "');</script>");


        //}
        protected void BtnShow_Click(object sender, EventArgs e)
        {
            DisplayCurrentRecord();

            if (drpYear.SelectedItem.Text == DateTime.Now.Year.ToString())
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
            else
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DataTable dtHdr = null;
            try
            {
                if ((txtFilePath.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    DataSet dsNotUploadchassisDetails = new DataSet();
                    DataTable dtLayout = new DataTable();
                    string query = null;
                    string connString = "";
                    string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    string strFileExtType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
                    string strFileType = ".xlsx";

                    //Check file type
                    if (strFileExtType == ".xls" || strFileExtType == ".xlsx")
                        txtFilePath.SaveAs(Server.MapPath("~/DownLoadFiles/PartsTarget/" + strFileName + strFileType));
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return;
                    }
                    string strNewPath = Server.MapPath("~/DownLoadFiles/PartsTarget/" + strFileName + strFileType);

                    //Connection String to Excel Workbook
                    if (strFileType.Trim() == ".xls")
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    else if (strFileType.Trim() == ".xlsx")
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    query = "SELECT * FROM [Sheet1$]";

                    //Create the connection object
                    conn = new OleDbConnection(connString);
                    //Open connection
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //Create the command object
                    cmd = new OleDbCommand(query, conn);
                    da = new OleDbDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dtHdr = new DataTable();

                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                            dtDetails = ds.Tables[0];
                        else
                        {
                            string sMessage = "Empty File Selected .";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            goto Last;
                        }

                        clsSparesTarget objSpaTargt = new clsSparesTarget();
                        int iCountNotUpload = 0;
                        // For checking all uploaded or not
                        DataSet dsUploadPartsTarget;
                        dsUploadPartsTarget = objSpaTargt.UploadPartsTargetDetails(txtFilePath.FileName, dtDetails, iUserId);
                        if (dsUploadPartsTarget == null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ErrorInUpload();", true);
                            return;
                        }
                        else
                        {
                            SetListOfNotUploadedDealerCode(dsUploadPartsTarget.Tables[0]);
                            dtDetails = dsUploadPartsTarget.Tables[1];
                        }
                        if (iCountNotUpload != 0)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + " Dealer Target not uploaded.');</script>");
                        }

                        //clsSparesTarget objSpaTargt = new clsSparesTarget();
                        if (objSpaTargt.bAnnualTarget(dtDetails) == true)
                        {

                            int TotalRecords = 0;
                            int TotalSuccess = 0;
                            int TotalError = 0;
                            ViewState["AnnualTarget"] = null;
                            ViewState["AnnualTarget"] = ds.Tables[0];

                            if (dtDetails != null && dtDetails.Rows.Count > 0)
                                TotalSuccess = dtDetails.Rows.Count;

                            //TotalRecords = TotalSuccess + TotalError;

                            string sMessage = "File Uploaded successfully! Total Records:" + TotalSuccess;
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            //lblMessage.Text = "Data retrieved successfully! Total Records:" + ds.Tables[0].Rows.Count;
                            //lblMessage.ForeColor = System.Drawing.Color.Green;
                            //lblMessage.Visible = true;

                            da.Dispose();
                            conn.Close();
                            conn.Dispose();

                        }
                        else
                        {
                            string sMessage = "Error : Data Uploading Failed!";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                        }

                    }
                    catch (Exception ex)
                    {
                        string sMessage = "Error : Data Uploading Failed!";
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    }
                }
                else
                {
                    string sMessage = "Please select an excel file first";
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                }
            Last: ;
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void SetListOfNotUploadedDealerCode(DataTable dtDealerCodeList)
        {
            try
            {
                string sDealerCodeList = "";
                if (Func.Common.iRowCntOfTable(dtDealerCodeList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtDealerCodeList.Rows.Count; iRowCnt++)
                    {
                        sDealerCodeList = sDealerCodeList + Func.Convert.sConvertToString(dtDealerCodeList.Rows[iRowCnt]["Dealer_Code"]) + ",";
                    }
                    lblListPartNo.Text = "Dealer Code which are Parts Annual Target not uploaded";
                    txtListPartNo.Text = sDealerCodeList;
                    txtListPartNo.Visible = true;
                    lblListPartNo.Visible = true;
                }
                else
                {
                    txtListPartNo.Visible = false;
                    lblListPartNo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpYear.SelectedItem.Text == DateTime.Now.Year.ToString() && iMenuId != 697)
            {
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                tdSelectFile.Style.Add("display", "");
                tdUploadFile.Style.Add("display", "");
                GrdAnnualTarget.Enabled = false;
            }
            else
            {
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                tdSelectFile.Style.Add("display", "none");
                tdUploadFile.Style.Add("display", "none");
                GrdAnnualTarget.Enabled = false;
            }
            DisplayCurrentRecord();

            if (drpYear.SelectedItem.Text == DateTime.Now.Year.ToString())
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
            else
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);

        }
    }
}