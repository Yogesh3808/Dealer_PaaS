using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using System.IO;
using System.Data.OleDb;

namespace MANART.Forms.Spares
{
    public partial class FrmSparesOA : System.Web.UI.Page
    {
        private int iDealerID = 0;
        string sFileName = "";
        string OANo = "";
        private int iOAID = 0;
        int DealerID = 0;
        //string DealerCode = "";
        private DataTable dtDetails = new DataTable();
        private DataTable dtOATaxDetails = new DataTable();
        private DataTable dtOAGrpTaxDetails = new DataTable();
        private DataSet dsCreatedBy = new DataSet();
        private bool bDetailsRecordExist = false;
        clsSparesOA objOA = null;
        string sNew = "Y";
        int TotCntQty = 0;
        int iUserId = 0;
        private int UsreType;
        int iHOBr_id = 0;
        int Cust_State = 0;
        string TaxTag = "";
        private bool sDiscChange = false;
        string sDealerCode = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iValidationIdForSave = 63;
                ToolbarC.iValidationIdForConfirm = 63;
                ToolbarC.bUseImgOrButton = true;
                ToolbarC.iFormIdToOpenForm = 103; //print option
                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();

                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);
                //For MD User
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                //For MD User
                if (!IsPostBack)
                {
                    Session["PartDetails"] = null;
                    Session["OATaxDetails"] = null;
                    Session["OAGrpTaxDetails"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = "OA List";

                FillSelectionGrid();
                if (iOAID != 0)
                {
                    GetDataAndDisplay();
                }
                if (txtID.Text == "")
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                //SetDocumentDetails();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PartDetails"] = null;
                Session["OATaxDetails"] = null;
                Session["OAGrpTaxDetails"] = null;
                GenerateOANo();
                //GetCreatedBy();
                DisplayPreviousRecord();
            }
            Location.drpCountryIndexChanged += new EventHandler(Location_drpCountryIndexChanged);
            Location.drpRegionIndexChanged += new EventHandler(Location_drpRegionChanged);
            //if (Location.iDealerId != 0)
            //{
            //    FillSelectionGrid();
            //    DisplayCurrentRecord();
            //    PSelectionGrid.Style.Add("display", "");
            //}
        }
        protected void Location_drpCountryIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GenerateOANo();
                //GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Location_drpRegionChanged(object sender, EventArgs e)
        {
            try
            {
                GenerateOANo();
                //GetCreatedBy();
                FillSelectionGrid();
                DisplayCurrentRecord();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and ISNULL(CM.Active,'N')='Y'");
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And CM.Cust_Type != 6 CM.Active='Y'");
            //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DealerID='" + Location.iDealerId + "') And Cust_Type != 6");
            //FIlter for Dealer ID and BranchCOde
            //" And ( DealerID='" + Location.iDealerId + "') And ( DealerHOBrID='"+ iHOBr_id+"') ");
            DrpCustomer.SelectedValue = "0";

            Func.Common.BindDataToCombo(ddlGST_OA_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (1,2) and isnull(Active,'N')='Y' ");
            ddlGST_OA_Type.SelectedValue = "0";
        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iOAID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objOA = new clsSparesOA();
                if (iOAID != 0)
                {
                    ds = objOA.GetOA(iOAID, "All", Location.iDealerId, 0);
                    sNew = "N";
                    DisplayData(ds);
                    objOA = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objOA = null;
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

                DataSet ds = new DataSet();
                // int iDealerId = 3;
                DealerID = Location.iDealerId;

                objOA = new clsSparesOA();
                ds = objOA.GetOA(iOAID, "New", DealerID, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["oa_canc"] = "N";
                            ds.Tables[0].Rows[0]["oa_lock"] = "N";
                            ds.Tables[0].Rows[0]["oa_com"] = "N";
                            sNew = "Y";

                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtOADate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                UploafFile.Style.Add("display", txtUserType.Text.Trim() == "6" ? "none" : "");
                objOA = null;
                ds = null;

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
                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                UploafFile.Style.Add("display", "none");
                if (sNew == "N")
                {
                    txtOANo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_no"]);
                    txtOADate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_Date"]);
                    DrpCustomer.Enabled = false;
                    DrpCustomer.CssClass = "NonEditableFields";
                }
                if (sNew == "N")
                {
                    Func.Common.BindDataToCombo(ddlGST_OA_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (1,2) ");
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 ");
                    //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And CM.Cust_Type != 6");
                }
                else
                {
                    Func.Common.BindDataToCombo(ddlGST_OA_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (1,2) and isnull(Active,'N')='Y' ");
                    Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And CM.Cust_Type <> 18 and DealerID=" + Location.iDealerId + " and Cust_Type != 6 and ISNULL(CM.Active,'N')='Y'");
                    //Func.Common.BindDataToCombo(DrpCustomer, clsCommon.ComboQueryType.Customer, 0, " And ( DCL.DealerID='" + Location.iDealerId + "') And CM.Cust_Type != 6 CM.Active='Y'");
                }
                DrpCustomer.SelectedValue = "0";
                //if (txtUserType.Text == "6")
                //{
                //    FillCombo();
                //}
                DrpCustomer.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustID"]);
                txtTotal.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["net_tr_amt"]);
                txtRefernce.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["reference"]);
                txtNarration.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["narration"]);
                txtValidDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_validity_date"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustTaxTag"]);
                hdnIsDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                hdnGSTOAType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTOAType"]);
                ddlGST_OA_Type.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTOAType"]);
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["IsRoundOFF"]);
                if (Func.Convert.sConvertToString(ddlGST_OA_Type.SelectedValue) == "1")
                    hdnGSTOAType.Value = "O";
                else
                    hdnGSTOAType.Value = "N";
                ddlGST_OA_Type.Enabled = (sNew == "N") ? false : true;
                DrpCustomer.Enabled = (sNew == "N") ? false : true;

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_PerAmt"]) == "Per")
                    rbtLstDiscount.SelectedValue = "Per";
                else
                    rbtLstDiscount.SelectedValue = "Amt";
                //rbtLstDiscount.Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");

                txtOADate.Enabled = false;

                Session["PartDetails"] = null;
                Session["OATaxDetails"] = null;
                Session["OAGrpTaxDetails"] = null;

                dtDetails = ds.Tables[1];
                Session["PartDetails"] = dtDetails;

                dtOAGrpTaxDetails = ds.Tables[2];
                Session["OAGrpTaxDetails"] = dtOAGrpTaxDetails;

                dtOATaxDetails = ds.Tables[3];
                Session["OATaxDetails"] = dtOATaxDetails;

                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_lock"]);

                BindDataToGrid();
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, sNew == "N" ? true : false);
                // If Record is Confirm or cancel then it is not editable      

                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_lock"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["oa_canc"]) == "Y")
                {
                    bEnableControls = false;
                }
                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);
                }
                if (txtUserType.Text.Trim() == "6")
                {
                    DrpCustomer.Enabled = false;
                    UploafFile.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form        
            //txtOADate.Enabled = bEnable;
            //drpPoType.Enabled = bEnable;
            //DrpCustomer.Enabled = bEnable;

            PartGrid.Enabled = bEnable;
            GrdPartGroup.Enabled = bEnable;
            GrdDocTaxDet.Enabled = bEnable;

            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }
        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                GenerateOANo();
                //GetCreatedBy();
                FillSelectionGrid();
                if (txtUserType.Text != "6")
                {
                    DisplayCurrentRecord();
                }
                if (txtUserType.Text == "6")
                {
                    DisplayPreviousRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerId = 3;
                DealerID = Location.iDealerId;
                objOA = new clsSparesOA();
                ds = objOA.GetOA(iOAID, "Max", DealerID, 0);
                if (ds == null) // if no Data Exist
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    sNew = "N";
                    DisplayData(ds);
                }
                else
                {
                    DisplayData(ds);
                    Session["PartDetails"] = null;
                    Session["OATaxDetails"] = null;
                    Session["OAGrpTaxDetails"] = null;

                    BindDataToGrid();

                }
                ds = null;
                objOA = null;
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
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    PSelectionGrid.Style.Add("display", "none");
                    DisplayPreviousRecord();
                    if (Location.iDealerId != 0)
                    {
                        GenerateOANo();
                        //GetCreatedBy();
                    }
                    DrpCustomer.Enabled = true;
                    DrpCustomer.CssClass = "ComboBoxFixedSize";

                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false, false) == false) return;
                    if (txtID.Text == "")
                    {
                        FillSelectionGrid();
                        DisplayCurrentRecord();
                        PSelectionGrid.Style.Add("display", "");
                        return;
                    }
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bSaveRecord(true, false) == false) return;
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (bSaveRecord(false, true) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                iOAID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
            CPEPartDetails.Collapsed = false;
            btnReadonly();
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
                else if (objCommon.sUserRole == "19")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
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
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("OA No");
                SearchGrid.AddToSearchCombo("OA Date");
                SearchGrid.AddToSearchCombo("OA Status");
                SearchGrid.AddToSearchCombo("Customer Name");
                SearchGrid.iDealerID = Location.iDealerId;
                SearchGrid.sSqlFor = "SparesOA";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                string cntPartID = "";
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("oa_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("oa_Date", typeof(string)));

                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("CustID", typeof(int)));

                dtHdr.Columns.Add(new DataColumn("oa_close", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("oa_canc", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("oa_com", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("oa_lock", typeof(string)));

                ////net_tr_amt,reference,narration,oa_validity_date
                dtHdr.Columns.Add(new DataColumn("net_tr_amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("reference", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("narration", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("oa_validity_date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("IS_PerAmt", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("DocGST", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CustTaxTag", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GSTOAType", typeof(int)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["oa_no"] = txtOANo.Text;
                dr["oa_Date"] = txtOADate.Text;
                dr["CustID"] = Func.Convert.iConvertToInt(DrpCustomer.SelectedValue);
                dr["Dealer_ID"] = Location.iDealerId;
                dr["oa_close"] = "N";
                dr["oa_canc"] = "N";
                dr["oa_com"] = "N";
                dr["oa_lock"] = "N";

                dr["net_tr_amt"] = 0;
                dr["reference"] = txtRefernce.Text;
                dr["narration"] = txtNarration.Text;
                dr["oa_validity_date"] = txtValidDate.Text;
                dr["IS_PerAmt"] = Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue);
                dr["DocGST"] = Func.Convert.sConvertToString(hdnIsDocGST.Value);
                dr["CustTaxTag"] = Func.Convert.sConvertToString(hdnCustTaxTag.Value);
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);
                dr["GSTOAType"] = Func.Convert.iConvertToInt(ddlGST_OA_Type.SelectedValue);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bValidateRecord()
        {

            string sMessage = " Please enter the select records.";
            bool bValidateRecord = true;
            if (txtOADate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (DrpCustomer.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please Select Customer!";
                bValidateRecord = false;
            }
            if (ddlGST_OA_Type.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please Select Rate Type";
                bValidateRecord = false;
            }

            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
        }
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            DataTable dtHdr = new DataTable();
            clsSparesOA objOA = new clsSparesOA();
            try
            {
                bool bEnblSave = false;
                if (bSaveWithConfirm == false && bSaveWithCancel == false)
                {
                    bEnblSave = true;
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                }
                if (bValidateRecord() == false)
                {
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
                UpdateHdrValueFromControl(dtHdr);
                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["oa_lock"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["oa_canc"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true, true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }

                if (objOA.bSaveRecordWithPart(Location.sDealerCode, dtHdr, dtDetails, dtOAGrpTaxDetails, dtOATaxDetails) == true)
                {
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Parts OA") + "','" + Server.HtmlEncode(txtOANo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Parts OA") + "','" + Server.HtmlEncode(txtOANo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Parts OA") + "','" + Server.HtmlEncode(txtOANo.Text) + "');</script>");
                        if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Parts OA") + "','" + Server.HtmlEncode(txtOANo.Text) + "');</script>");
                    if (bEnblSave == true) ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        private void GenerateOANo()
        {
            //clsDB objDB = new clsDB();
            //try
            //{
            //    DataSet dsDCode = new DataSet();
            //    DealerID = Location.iDealerId;
            //    dsDCode = objDB.ExecuteQueryAndGetDataset("Select Dealer_Spares_Code from M_Dealer where Id=" + DealerID);

            //    if (dsDCode.Tables[0].Rows.Count > 0)
            //    {
            //        DealerCode = dsDCode.Tables[0].Rows[0]["Dealer_Spares_Code"].ToString();
            //    }
            //}
            //catch
            //{

            //}
            //finally
            //{
            //    if (objDB != null) objDB = null;
            //}
            objOA = new clsSparesOA();
            txtOANo.Text = Func.Convert.sConvertToString(objOA.GenerateOANo(sDealerCode, DealerID));
            //txtOANo.Text = Func.Convert.sConvertToString(objOA.GenerateOANo(DealerCode, DealerID));
            //txtOANo.Text = Func.Common.sGetMaxDocNo(Convert.ToString(Location.iDealerId), "", "OA", Location.iDealerId);
            //txtOANo.Text = Func.Common.sGetMaxDocNo(DealerCode, "", "OA", Location.iDealerId);

        }
        private void GetCreatedBy()
        {
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            UsreType = Func.Convert.iConvertToInt(Session["UserType"]);

            clsDB objDB = new clsDB();
            try
            {
                dsCreatedBy = objDB.ExecuteStoredProcedureAndGetDataset("SP_POCreatedByName", iUserId, UsreType);
                //txtCreatedBy.Text = dsCreatedBy.Tables[0].Rows[0]["PO_CreatedBy"].ToString();
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

        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {

                FillDetailsFromGrid(false, false);
                BindDataToGrid();
                FillDetailsFromGrid(false, false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["PartDetails"] = dtDetails;
                Session["OATaxDetails"] = dtOATaxDetails;
                Session["OAGrpTaxDetails"] = dtOAGrpTaxDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg, bool bSaveTmTxDtls)
        {
            try
            {
                string sStatus = "";
                string strPosNo = "";
                dtDetails = (DataTable)Session["PartDetails"];
                bDetailsRecordExist = true;
                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iDtSelPartRow = 0;
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                    TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");

                    if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U"))
                    {
                        if (txtStatus.Text == "U")
                        {
                            if (strPosNo == "")
                                strPosNo = iRowCnt.ToString();
                            else
                                strPosNo = strPosNo + "," + iRowCnt.ToString();
                        }
                        iCntForSelect = iCntForSelect + 1;
                        continue;
                    }
                    iDtSelPartRow = 0;
                    for (int iDtRowCnt = 0; iDtRowCnt < dtDetails.Rows.Count; iDtRowCnt++)
                    {
                        if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                        {
                            iDtSelPartRow = iDtRowCnt;
                            break;
                        }
                    }
                    hdnSelectedPartID.Value = hdnSelectedPartID.Value + ((hdnSelectedPartID.Value.Length > 0) ? "," : "") + Func.Convert.iConvertToInt(txtPartID.Text);

                    dtDetails.Rows[iRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);


                    //PartNo Or NewPart
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                    dtDetails.Rows[iRowCnt]["part_no"] = txtPartNo.Text;

                    //Part Name
                    TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    dtDetails.Rows[iRowCnt]["Part_Name"] = txtPartName.Text;

                    // Get Qty
                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    dtDetails.Rows[iRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                    //Group code
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                    dtDetails.Rows[iRowCnt]["group_code"] = txtGrNo.Text;

                    // Get Bal Qty
                    TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                    dtDetails.Rows[iRowCnt]["bal_qty"] = Func.Convert.dConvertToDouble(txtBalQty.Text);

                    // Get Price
                    TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                    dtDetails.Rows[iRowCnt]["Price"] = Func.Convert.dConvertToDouble(txtPrice.Text);

                    // Get MRPRate
                    TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    dtDetails.Rows[iRowCnt]["MRPRate"] = Func.Convert.dConvertToDouble(txtMRPRate.Text);

                    //Discount Per
                    TextBox txtDiscountPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountPer");
                    dtDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtDiscountPer.Text);

                    //Discount rate
                    TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                    dtDetails.Rows[iRowCnt]["disc_rate"] = Func.Convert.dConvertToDouble(txtDiscountRate.Text);

                    //Discount Amt
                    TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                    dtDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDiscountAmt.Text);

                    // Get Total
                    TextBox txtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                    dtDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTotal.Text);

                    //Get BFR GST Rate Flag
                    TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                    dtDetails.Rows[iRowCnt]["BFRGST"] = txtBFRGST.Text.Trim();

                    // Get BFR GST Stock
                    TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");
                    dtDetails.Rows[iRowCnt]["BFRGST_Stock"] = Func.Convert.dConvertToDouble(txtBFRGST_Stock.Text);

                    //Sujata 26052014_Begin
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                    dtDetails.Rows[iRowCnt]["PartTaxID"] = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                    DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                    Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                    if (DrpPartTax1.Items.Count == 2)
                    {
                        DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                    }

                    DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                    Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                    if (DrpPartTax2.Items.Count == 2)
                    {
                        DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                    }
                    //Sujata 26052014_End

                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                    //dtDetails.Rows[iRowCnt]["Status"] = "";            
                    if (txtPartID.Text != "" && txtPartID.Text != "0" && Chk.Checked == false)
                    {
                        iCntForSelect = iCntForSelect + 1;
                    }
                    if (Chk.Checked == true)
                    {
                        dtDetails.Rows[iRowCnt]["Status"] = "D";
                        iCntForDelete++;
                    }
                    else
                    {
                        dtDetails.Rows[iRowCnt]["Status"] = "N";
                    }
                }
                if (iCntForDelete == iCntForSelect)
                {
                    if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
                    bDetailsRecordExist = false;
                }

                //For Tax Details
                dtOAGrpTaxDetails = (DataTable)(Session["OAGrpTaxDetails"]);
                dtOATaxDetails = (DataTable)(Session["OATaxDetails"]);

                //if (bSaveTmTxDtls == true)
                //{                
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Net Reverse Amount
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //dtOAGrpTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtGrnetrevamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(txtTaxPer.Text);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(txtTax2Per.Text);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    dtOAGrpTaxDetails.Rows[iRowCnt]["Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }

                for (int iRowCnt = 0; iRowCnt < GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    dtOATaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    dtOATaxDetails.Rows[iRowCnt]["net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);

                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtOATaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    dtOATaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    dtOATaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    dtOATaxDetails.Rows[iRowCnt]["mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    dtOATaxDetails.Rows[iRowCnt]["cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    dtOATaxDetails.Rows[iRowCnt]["surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    dtOATaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    dtOATaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    dtOATaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    //Get PF IGST or SGST Per  
                    TextBox txtPFTaxPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFTaxPer");
                    dtOATaxDetails.Rows[iRowCnt]["PF_Tax_Per"] = Func.Convert.dConvertToDouble(txtPFTaxPer.Text);

                    //Get PF IGST or SGST Amount  
                    TextBox txtPFIGSTorSGSTAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFIGSTorSGSTAmt");
                    dtOATaxDetails.Rows[iRowCnt]["PF_IGSTorSGST_Amt"] = Func.Convert.dConvertToDouble(txtPFIGSTorSGSTAmt.Text);

                    //Get PF CGST Per
                    TextBox txtPFTaxPer1 = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFTaxPer1");
                    dtOATaxDetails.Rows[iRowCnt]["PF_Tax_Per1"] = Func.Convert.dConvertToDouble(txtPFTaxPer1.Text);

                    //Get PF CGST Amount
                    TextBox txtPfCGSTrAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPfCGSTrAmt");
                    dtOATaxDetails.Rows[iRowCnt]["PF_CGST_Amt"] = Func.Convert.dConvertToDouble(txtPfCGSTrAmt.Text);


                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    dtOATaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    dtOATaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    dtOATaxDetails.Rows[iRowCnt]["oa_tot"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToDetailsTable()
        {
            try
            {
                DataRow dr;
                if (dtDetails.Rows.Count == 0)
                {
                    if (dtDetails.Columns.Count == 0)
                    {
                        dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("part_no", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                        dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("bal_qty", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("disc_rate", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                        dtDetails.Columns.Add(new DataColumn("PartTaxID", typeof(int)));
                        dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("TaxTag", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(string)));
                        dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));
                    }
                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0")
                    {
                        goto Exit;
                    }
                }
                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["part_no"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 0.00;
                dr["group_code"] = "";
                dr["bal_qty"] = 0.00;
                dr["MRPRate"] = 0.00;
                dr["discount_per"] = 0.00;
                dr["disc_rate"] = 0.00;
                dr["discount_amt"] = 0.00;
                dr["Total"] = 0.00;
                dr["PartTaxID"] = 0;
                dr["Status"] = "U";
                if (Func.Convert.sConvertToString(dtDetails.Rows[0]["TaxTag"]) == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();

            Exit: ;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void CreateNewRowToTaxGroupDetailsTable()
        {
            try
            {
                string sGrCode = "";
                int iPartTaxID = 0;
                int iPartTaxID1 = 0;
                int iPartTaxID2 = 0;
                Boolean bDtSelPartRow = false;
                dtOAGrpTaxDetails.Clear();
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {
                    sGrCode = "";
                    iPartTaxID = 0;
                    iPartTaxID1 = 0;
                    iPartTaxID2 = 0;
                    bDtSelPartRow = false;

                    TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                    sGrCode = txtGrNo.Text.Trim();
                    if (sGrCode.Length > 0)
                    {
                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        iPartTaxID = Func.Convert.iConvertToInt(drpPartTax.SelectedValue);

                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        iPartTaxID1 = Func.Convert.iConvertToInt(DrpPartTax1.SelectedItem.Text);

                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        iPartTaxID2 = Func.Convert.iConvertToInt(DrpPartTax2.SelectedItem.Text);
                    }
                    for (int iRCnt = 0; iRCnt < dtOAGrpTaxDetails.Rows.Count; iRCnt++)
                    {
                        if (Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRCnt]["group_code"]) == Func.Convert.sConvertToString(sGrCode) &&
                            Func.Convert.iConvertToInt(dtOAGrpTaxDetails.Rows[iRCnt]["Tax_Code"]) == Func.Convert.iConvertToInt(iPartTaxID) &&
                            iPartTaxID != 0)
                        {
                            bDtSelPartRow = true;
                            break;
                        }
                    }

                    DataRow dr;
                    //if (dtOAGrpTaxDetails.Rows.Count == 0)
                    //{
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("group_code", typeof(String)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Gr_Name", typeof(String)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("net_inv_amt", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("discount_per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("discount_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax_Code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("TAX_Percentage", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax_Tag", typeof(string)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax1_code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax1_Per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax1_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax2_code", typeof(int)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Tax2_Per", typeof(double)));
                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("tax2_amt", typeof(double)));

                    //    dtOAGrpTaxDetails.Columns.Add(new DataColumn("Total", typeof(double)));

                    //}
                    //else if (dtOAGrpTaxDetails.Rows.Count == 1)
                    //{
                    //    if (dtOAGrpTaxDetails.Rows[0]["ID"].ToString() == "0")
                    //    {
                    //        goto Exit;
                    //    }
                    //}
                    if (bDtSelPartRow == false && sGrCode != "" && iPartTaxID > 0)
                    {
                        dr = dtOAGrpTaxDetails.NewRow();

                        dr["SRNo"] = "1";
                        dr["ID"] = 0;
                        dr["group_code"] = sGrCode;
                        dr["Gr_Name"] = sGrCode.Trim() == "01" ? "Parts" : sGrCode.Trim() == "02" ? "Lubricant" : "Local Part";

                        dr["net_inv_amt"] = 0;
                        //dr["net_rev_amt"] = 0;

                        dr["discount_per"] = 0;
                        dr["discount_amt"] = 0;

                        dr["Tax_Code"] = iPartTaxID;
                        dr["TAX_Percentage"] = 0;
                        dr["Tax_Tag"] = "";
                        dr["tax_amt"] = 0;

                        dr["tax1_code"] = iPartTaxID1;
                        dr["Tax1_Per"] = 0;
                        dr["tax1_amt"] = 0;

                        dr["tax2_code"] = iPartTaxID2;
                        dr["Tax2_Per"] = 0;
                        dr["tax2_amt"] = 0;

                        dr["Total"] = 0;


                        dtOAGrpTaxDetails.Rows.Add(dr);
                        dtOATaxDetails.AcceptChanges();
                    }
                }

            Exit: ;
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
                //If No Data in Grid
                if (Session["PartDetails"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["PartDetails"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["PartDetails"];
                    if (dtDetails.Rows.Count == 0)
                        CreateNewRowToDetailsTable();
                    else
                    {
                        if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                            CreateNewRowToDetailsTable();
                    }
                    dtDetails = (DataTable)Session["PartDetails"];
                }

                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();

                GrdPartGroup.DataSource = dtOAGrpTaxDetails;
                GrdPartGroup.DataBind();

                GrdDocTaxDet.DataSource = dtOATaxDetails;
                GrdDocTaxDet.DataBind();

                SetGridControlProperty();
                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();
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
                string sRecordStatus = "";

                double dTotalQty = 0;
                double dTotal = 0;
                double dPartTotal = 0;
                double dPartQty = 0;
                double dPartRate = 0;
                double dDiscPer = 0;
                double dDiscRate = 0;
                double dPartMrpPrice = 0;

                double dExclPartDiscRate = 0;
                double dExclPartTotal = 0;
                double dExclTotal = 0;

                double dPartTax = 0;
                double dPartTax1 = 0;
                double dPartTax2 = 0;
                string sGroupCode = "";
                double dRevMainTaxRate = 0;
                double dDiscRevRate = 0;

                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Location.iDealerId.ToString();
                string sPartID = "", sPartName = "", cPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    if (iRowCnt != Func.Convert.iConvertToInt(PartGrid.Rows.Count))
                    {
                        TextBox txtPartNo1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                        cPartID = txtPartNo1.Text;
                        sGroupCode = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["group_code"]).Trim();

                        TextBox txtMRPRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");

                        //GST Relates Work Part Tax
                        PartGrid.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                        PartGrid.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                        DropDownList drpPartTax = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");
                        drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);


                        DropDownList drpPartTaxPer = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTaxPer");
                        //Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");
                        if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        else
                            Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                        TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTaxPer");
                        txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                        TextBox txtDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                        TextBox txtDiscountAmt = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                        TextBox txtPrtTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");

                        TextBox txtExclDiscountRate = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtExclDiscountRate");
                        TextBox TxtExclTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("TxtExclTotal");


                        DropDownList DrpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1");
                        Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                        //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPPartTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type = 2" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //else
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPPartTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type = 2 ");
                        DropDownList DrpPartTax1Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax1Per");
                        TextBox txtPartTax1Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax1Per");

                        if (DrpPartTax1.Items.Count == 2)
                        {
                            DrpPartTax1.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax1Per, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " and ID=" + DrpPartTax1.SelectedItem);
                            if (DrpPartTax1Per.Items.Count == 2)
                            {
                                DrpPartTax1Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax1.SelectedItem);
                            }
                            txtPartTax1Per.Text = Func.Convert.sConvertToString(DrpPartTax1Per.SelectedItem);
                        }

                        DropDownList DrpPartTax2 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2");
                        Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                        //if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPPartTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type = 3" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //else
                        //    Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.EGPPartTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type = 3");
                        DropDownList DrpPartTax2Per = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("DrpPartTax2Per");
                        TextBox txtPartTax2Per = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartTax2Per");

                        if (DrpPartTax2.Items.Count == 2)
                        {
                            DrpPartTax2.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);
                            Func.Common.BindDataToCombo(DrpPartTax2Per, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " and ID=" + DrpPartTax2.SelectedItem);
                            if (DrpPartTax2Per.Items.Count == 2)
                            {
                                DrpPartTax2Per.SelectedValue = Func.Convert.sConvertToString(DrpPartTax2.SelectedItem);
                            }
                            txtPartTax2Per.Text = Func.Convert.sConvertToString(DrpPartTax2Per.SelectedItem);
                        }
                        if (sGroupCode == "02")
                            drpPartTax.Enabled = false;
                        else
                            drpPartTax.Enabled = true;
                        //Vikram 31012017  Start                   
                        if (sGroupCode == "99")
                        {
                            txtMRPRate.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtMRPRate.Attributes.Add("onblur", "return CalculateInvPartTotal(event,this);");
                        }
                        else
                            txtMRPRate.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        // VIkram 31012017 END

                        if (cPartID != "")
                        {
                            dPartQty = 0;
                            dPartRate = 0;
                            dDiscRate = 0;
                            dDiscPer = 0;
                            dPartMrpPrice = 0;
                            dDiscRevRate = 0;

                            dPartQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Qty"]);
                            //Reverse Rate Calulation for System Part
                            dPartMrpPrice = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["Price"]);
                            //Commit on 18.03.2017
                            //dPartTax = Math.Round(dPartMrpPrice * Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / (100 + Func.Convert.dConvertToDouble(txtPartTaxPer.Text)),2);
                            //if (sGroupCode == "01")
                            //    dPartRate = dPartMrpPrice - dPartTax;
                            clsDB objDB = new clsDB();
                            if (sGroupCode == "01" || sGroupCode == "02")
                                dPartRate = objDB.ExecuteStoredProcedure_double("Sp_GetReverseRate", dPartMrpPrice, Func.Convert.iConvertToInt(drpPartTax.SelectedValue));
                            else
                                dPartRate = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["MRPRate"]);
                            // Set Reverse Calculated Rate to Rate Column
                            txtMRPRate.Text = Func.Convert.sConvertToString(dPartRate);

                            dDiscPer = Func.Convert.dConvertToDouble(dtDetails.Rows[iRowCnt]["discount_per"]);
                            dDiscRate = Func.Convert.dConvertToDouble(dPartRate * (dDiscPer / 100));
                            dPartRate = Func.Convert.dConvertToDouble(dPartRate - dDiscRate);
                            //dPartRate = dPartQty * Func.Convert.dConvertToDouble(dPartRate - dDiscRate);

                            txtDiscountAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dDiscRate));
                            txtDiscountRate.Text = Func.Convert.sConvertToString(dPartRate);
                            dPartTotal = Func.Convert.dConvertToDouble(dPartQty * dPartRate);
                            txtPrtTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dPartTotal, 2)));
                            dTotal = dTotal + Func.Convert.dConvertToDouble(dPartTotal);

                            //New Code for Reverse rate Calculation on 02052017_Vikram Begin
                            //if (sGroupCode == "01" || sGroupCode == "02")
                            //{
                            //    dRevMainTaxRate = dPartMrpPrice / (1 + (Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtPartTaxPer.Text) / 100)));
                            //    dDiscRevRate = Func.Convert.dConvertToDouble(dRevMainTaxRate * dDiscPer / 100);
                            //    txtExclDiscountRate.Text = Func.Convert.sConvertToString(Math.Round(dDiscRevRate, 2));
                            //    dRevMainTaxRate = dRevMainTaxRate - dDiscRevRate;
                            //    dExclPartTotal = Func.Convert.dConvertToDouble(dPartQty * dRevMainTaxRate);
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}
                            //else
                            //{
                            //    dExclPartTotal = dPartTotal;
                            //    TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            //    dExclTotal = dExclTotal + dExclPartTotal;
                            //}
                            //End

                            dExclPartTotal = dPartTotal;
                            TxtExclTotal.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(Math.Round(dExclPartTotal, 2)));
                            dExclTotal = dExclTotal + dExclPartTotal;

                        }
                    }

                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");
                    lnkSelectPart.Attributes.Add("onclick", "return ShowSpWPFPart_New(this,'" + sDealerId + "','" + DrpCustomer.ID + "'," + iHOBr_id + ",'" + ddlGST_OA_Type.ID + "');");
                    //lblSelectPart.Style.Add("display", "none");
                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    TextBox txtBalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBalQty");
                    TextBox txtUnit = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtUnit");
                    TextBox txtPrice = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPrice");
                    TextBox txtMRPRate1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtMRPRate");
                    TextBox txtDiscountPer1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountPer");
                    TextBox txtDiscountAmt1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountAmt");
                    TextBox txtDiscountRate1 = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtDiscountRate");
                    TextBox txtGTotal = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtTotal");
                    DropDownList drpPartTax1 = (DropDownList)PartGrid.Rows[iRowCnt].FindControl("drpPartTax");
                    TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    Chk.Style.Add("display", "");
                    lblDelete.Style.Add("display", "");

                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);
                    // Vikram Hide Disc Per for GST
                    //PartGrid.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    //PartGrid.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    if (sPartID == "0")
                    {
                        Chk.Style.Add("display", "none");
                        lblDelete.Style.Add("display", "none");
                        lnkSelectPart.Style.Add("display", "");
                        txtPartNo.Style.Add("display", "none");
                        txtGPartName.Style.Add("display", "none");
                        txtQuantity.Style.Add("display", "none");
                        txtBalQty.Style.Add("display", "none");
                        txtUnit.Style.Add("display", "none");
                        txtPrice.Style.Add("display", "none");
                        txtMRPRate1.Style.Add("display", "none");
                        txtDiscountPer1.Style.Add("display", "none");
                        txtDiscountAmt1.Style.Add("display", "none");
                        txtDiscountRate1.Style.Add("display", "none");
                        txtGTotal.Style.Add("display", "none");
                        drpPartTax1.Style.Add("display", "none");
                        txtBFRGST_Stock.Style.Add("display", "none");
                    }
                    else
                        lnkSelectPart.Style.Add("display", "none");
                    if (txtGPartName.Text == "")
                    {
                        lnkSelectPart.Style.Add("display", "");
                        txtPartNo.Style.Add("display", "none");
                    }
                    else
                    {
                        lnkSelectPart.Style.Add("display", "none");
                        txtPartNo.Style.Add("display", "");

                    }


                    //If Part Id  is not allocated           
                    if (sRecordStatus == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        //PartGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    //else
                    //{
                    //    Chk.Style.Add("display", "");
                    //    Chk.Checked = false;
                    //}
                    if (txtUserType.Text == "6")
                    {

                        lnkSelectPart.Style.Add("display", "none");
                    }
                }

                txtTotal.Text = dTotal.ToString("0.00");
                txtTotalQty.Text = dTotalQty.ToString("0");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void SetGridControlPropertyTax()
        {
            try
            {

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);

                    //Vikram Begin 05062017_BEgin Dislay hide and label change code related to GST Appl and non appl.
                    GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    //Hide Group Disc per
                    GrdPartGroup.HeaderRow.Cells[4].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[4].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //hide Group Disc Amt
                    GrdPartGroup.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdPartGroup.Rows[iRowCnt].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //END

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    if (sDiscChange == true)
                    {
                        txtGrDiscountPer.Text = Func.Convert.sConvertToString(0.00);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString(0.00);
                    }
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtGrDiscountPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountPer.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");
                            txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        }
                        else
                        {
                            txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtGrDiscountAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtGrDiscountAmt.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");
                        }
                    }

                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    {
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");
                        //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, " And ( DealerID='" + Location.iDealerId + "') and Tax_Type= 1 ");
                    }
                    //Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.EGPMainTax, 0, " And ( DealerID='" + Location.iDealerId + "')");



                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                    {
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                        //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    }
                    //Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.EGPMainTaxPer, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");
                    //Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, " And ( DealerID='" + Location.iDealerId + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);

                    drpTax.Attributes.Add("onBlur", "SetMainTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");

                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    //Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    //Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    //Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));


                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();
                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    //Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    //Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    //Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, " And ( DealerID='" + Location.iDealerId + "')");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + Location.iDealerId + "')");


                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtOAGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
                    txtTax2Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer2.SelectedValue) == 0 ? "0" : drpTaxPer2.SelectedItem.Text);

                    drpTax.Enabled = false;
                    drpTax1.Enabled = false;
                    drpTax2.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void SetGridControlPropertyTaxCalculation()
        {
            try
            {
                double dGrpTotal = 0.00;
                double dGrpDiscPer = 0.00;
                double dGrpDiscAmt = 0.00;
                double dGrpTaxAppAmt = 0.00;

                double dGrpMTaxPer = 0.00;
                double dGrpMTaxAmt = 0.00;

                double dGrpTax1Per = 0.00;
                double dGrpTax1Amt = 0.00;

                double dGrpTax2Per = 0.00;
                double dGrpTax2Amt = 0.00;

                double dDocTotalAmtFrPFOther = 0.00;
                double dDocDiscAmt = 0.00;
                double dDocLSTAmt = 0.00;
                double dDocCSTAmt = 0.00;
                double dDocTax1Amt = 0.00;
                double dDocTax2Amt = 0.00;
                string sGrpMTaxTag = "";

                double TotalOA = 0.00;
                double TotalRev = 0.00;

                string sTax1ApplOn = "";
                string sTax2ApplOn = "";
                double dDocPFTaxPer = 0;
                double dDocPFTaxPer1 = 0;

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                    //TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                    //txtGrnetrevamt.Text = "0";
                }

                for (int i = 0; i < PartGrid.Rows.Count; i++)
                {
                    //TextBox txtTotal = (TextBox)PartGrid.Rows[i].FindControl("txtTotal");
                    TextBox TxtExclTotal = (TextBox)PartGrid.Rows[i].FindControl("TxtExclTotal");
                    TextBox txtGrNo = (TextBox)PartGrid.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)PartGrid.Rows[i].FindControl("drpPartTax");

                    TextBox txtPartTaxPer = (TextBox)PartGrid.Rows[i].FindControl("txtPartTaxPer");
                    TextBox txtPartTax1Per = (TextBox)PartGrid.Rows[i].FindControl("txtPartTax1Per");
                    CheckBox Chk = (CheckBox)PartGrid.Rows[i].FindControl("ChkForDelete");

                    if (txtGrNo.Text.Trim() != "" && drpPartTax.SelectedIndex != 0 && Chk.Checked == false)
                    {
                        TotalOA = TotalOA + Func.Convert.dConvertToDouble(TxtExclTotal.Text);
                        //TotalRev = Math.Round(TotalRev + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                        if (Func.Convert.dConvertToDouble(txtPartTaxPer.Text) > dDocPFTaxPer)
                            dDocPFTaxPer = Func.Convert.dConvertToDouble(txtPartTaxPer.Text);
                        if (Func.Convert.dConvertToDouble(txtPartTax1Per.Text) > dDocPFTaxPer1)
                            dDocPFTaxPer1 = Func.Convert.dConvertToDouble(txtPartTax1Per.Text);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0 && Chk.Checked == false)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(TxtExclTotal.Text)).ToString("0.00"));
                            //txtGrnetrevamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetrevamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
                        }
                    }
                }

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    TextBox txtGrnetrevamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetrevamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                    {
                        dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                        dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                        txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));
                    }
                    else
                    {
                        dGrpDiscPer = 0.00;
                        dGrpDiscAmt = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);
                    }

                    //Doc Discount Amount
                    dDocDiscAmt = Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt);

                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100));
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    //Vikram 02052017_BEGIN
                    //double sum = 0;
                    if (sTax1ApplOn == "1")
                    {
                        //Vikram Begin_24052017
                        //double DiscNetAmt = 0;
                        //DiscNetAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt)), 2);
                        //DiscNetAmt = DiscNetAmt + dGrpMTaxAmt;
                        //sum = DiscNetAmt / (1 + (Func.Convert.dConvertToDouble(dGrpTax1Per / 100)));
                        //dGrpTax1Amt = Math.Round(sum * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                        //END
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else
                    {
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    //Sujata 23092014 End

                    dDocTax1Amt = Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Func.Convert.dConvertToDouble(txtTax2Per.Text);
                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    else
                    {
                        dGrpTax2Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100));
                    }
                    dDocTax2Amt = Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));

                    //Vikram BEGIN _02052017
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    //END
                    //dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));

                    dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    //Hide Group Disc Amt
                    GrdDocTaxDet.HeaderRow.Cells[3].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[3].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //PF Per and PF Amount Display dated 28092017 Vikram
                    GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", "");//Hide Cell
                    GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", "");//Hide Cell
                    //PF Tax per 
                    GrdDocTaxDet.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Freight IGST Tax% " : "Freight SGST Tax%"; //IGST or SGST per
                    GrdDocTaxDet.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "N") ? "none" : "");//Hide Cell
                    //New PF IGST or SGST Amount
                    GrdDocTaxDet.HeaderRow.Cells[14].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "Freight IGST Amt " : "Freight SGST Amt ";
                    // PF Tax per 1
                    GrdDocTaxDet.HeaderRow.Cells[15].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[15].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : "");//Hide Cell
                    //New PF CGST Amount
                    GrdDocTaxDet.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : ""); // Hide Header  
                    GrdDocTaxDet.Rows[i].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "N" || (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O")) ? "none" : "");//Hide Cell

                    ////Hide Group Disc Amt
                    //GrdDocTaxDet.HeaderRow.Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    //GrdDocTaxDet.Rows[i].Cells[9].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    ////Hide Group Disc Amt
                    //GrdDocTaxDet.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    //GrdDocTaxDet.Rows[i].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //Hide Group Disc Amt
                    GrdDocTaxDet.HeaderRow.Cells[11].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[11].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    //Hide Group Disc Amt
                    GrdDocTaxDet.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END

                    TextBox txtDocTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocRevTotal");
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtPFTaxPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFTaxPer");
                    TextBox txtPFIGSTorSGSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFIGSTorSGSTAmt");
                    TextBox txtPFTaxPer1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFTaxPer1");
                    TextBox txtPfCGSTrAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPfCGSTrAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    //txtDocRevTotal.Text = Func.Convert.sConvertToString(TotalRev.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    txtPFTaxPer.Text = Func.Convert.sConvertToString(dDocPFTaxPer.ToString("0.00"));
                    txtPFTaxPer1.Text = Func.Convert.sConvertToString(dDocPFTaxPer1.ToString("0.00"));

                    double dDocPFPer = 0;
                    double dDocPFAmt = 0;
                    double dDocOtherPer = 0;
                    double dDocOtherAmt = 0;
                    double dDocGrandAmt = 0;
                    double dDocPFTaxAmt1 = 0;
                    double dDocPFTaxAmt2 = 0;

                    if (sDiscChange == true)
                    {
                        txtPFPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtPFAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                        txtOtherPer.Text = Func.Convert.sConvertToString(0.00.ToString("0.00"));
                        txtOtherAmt.Text = Func.Convert.sConvertToString(0.ToString("0.00"));
                    }
                    if (hdnIsDocGST.Value == "N")// if (hdnIsDocGST.Value == "Y")
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFPer.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");

                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                        }
                        else
                        {
                            txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtPFAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtPFAmt.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");

                            //dDocPFPer = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_per"]), 2);
                            //dDocPFAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["pf_amt"]), 2);
                            dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                            dDocPFAmt = Func.Convert.dConvertToDouble(txtPFAmt.Text);
                        }
                    }
                    txtPFPer.Text = Func.Convert.sConvertToString(dDocPFPer.ToString("0.00"));
                    txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));
                    if (hdnCustTaxTag.Value == "I" && hdnIsDocGST.Value == "Y")
                    {
                        dDocPFTaxAmt1 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                        dDocPFTaxAmt2 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                    }
                    else
                    {
                        dDocPFTaxAmt1 = Func.Convert.dConvertToDouble(dDocPFAmt) * Func.Convert.dConvertToDouble(dDocPFTaxPer / 100);
                    }
                    txtPFIGSTorSGSTAmt.Text = Func.Convert.sConvertToString(dDocPFTaxAmt1.ToString("0.00"));//Set IGST or SGST Amount
                    txtPfCGSTrAmt.Text = Func.Convert.sConvertToString(dDocPFTaxAmt2.ToString("0.00"));//Set CGST Amount

                    //dDocPFPer = Func.Convert.dConvertToDouble(txtPFPer.Text);
                    //dDocPFAmt = Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocPFPer / 100);
                    //txtPFAmt.Text = Func.Convert.sConvertToString(dDocPFAmt.ToString("0.00"));

                    //dDocTotalAmtFrPFOther = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt));
                    if (hdnIsDocGST.Value == "Y")
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt) + Func.Convert.dConvertToDouble(dDocPFTaxAmt1) + Func.Convert.dConvertToDouble(dDocPFTaxAmt2)), 2);
                    }
                    else
                    {
                        dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    }
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }
                    else
                    {
                        if (Func.Convert.sConvertToString(rbtLstDiscount.SelectedValue) == "Per")
                        {
                            txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherPer.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherPer.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100)), 2);
                        }
                        else
                        {
                            txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                            txtOtherAmt.Attributes.Add("onkeypress", "return CheckForTextBoxValue(event,this,'6');");
                            txtOtherAmt.Attributes.Add("onblur", "return CalulateOAPartGranTotal();");

                            dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                            //dDocOtherAmt = Math.Round(Func.Convert.dConvertToDouble(dtInvTaxDetails.Rows[0]["other_money"]), 2);
                            dDocOtherAmt = Func.Convert.dConvertToDouble(txtOtherAmt.Text);
                        }
                    }
                    //dDocOtherPer = Func.Convert.dConvertToDouble(txtOtherPer.Text);
                    //dDocOtherAmt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) * Func.Convert.dConvertToDouble(dDocOtherPer / 100));
                    //txtOtherAmt.Text = Func.Convert.sConvertToString(dDocOtherAmt.ToString("0.00"));

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void PartGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                dtDetails = (DataTable)Session["PartDetails"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void drpPoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateOANo();
        }
        protected void GrdPartGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void drpPartTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDetailsFromGrid(false, false);
            CreateNewRowToTaxGroupDetailsTable();
            Session["PartDetails"] = dtDetails;
            Session["OATaxDetails"] = dtOATaxDetails;
            Session["OAGrpTaxDetails"] = dtOAGrpTaxDetails;
            BindDataToGrid();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                objOA = new clsSparesOA();
                DataTable dt = GetListOfPartNo();

                int iCountNotUpload = 0;

                string sFileNameForSave = "";
                DataSet dsUploadPartDetails;
                System.Threading.Thread.Sleep(2000);
                sFileName = FileUpload1.FileName;
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));

                dsUploadPartDetails = objOA.UploadPartDetailsAndGetPartDetails(FileUpload1.FileName, Func.Convert.iConvertToInt(Location.iDealerId), dt, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue), iHOBr_id, hdnCustTaxTag.Value, hdnIsDocGST.Value, hdnGSTOAType.Value);
                if (dsUploadPartDetails == null)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in upload the the file contact admin.');</script>");
                    return;
                }
                else
                {
                    SetListOfNotUploadedPartNo(dsUploadPartDetails.Tables[0]);
                    dtDetails = dsUploadPartDetails.Tables[1];
                }
                if (iCountNotUpload != 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + iCountNotUpload + " parts not uploaded.');</script>");
                }
                DataRow dr;
                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["part_no"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 1;
                dr["group_code"] = "";
                dr["bal_qty"] = 1;
                dr["MRPRate"] = 1;
                dr["discount_per"] = 0.00;
                dr["disc_rate"] = 0.00;
                dr["discount_amt"] = 0.00;
                dr["Total"] = 1;
                dr["PartTaxID"] = 0;
                dr["Status"] = "U";
                if (Func.Convert.sConvertToString(dtDetails.Rows[1]["TaxTag"]) == "I" || hdnCustTaxTag.Value == "I")
                    dr["TaxTag"] = "I";
                else
                    dr["TaxTag"] = "O";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;
               

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();
                Session["PartDetails"] = dtDetails;
                BindDataToGrid();
                //Sujata 23062014 To set a tax grid we need to do following code
                FillDetailsFromGrid(false, false);
                CreateNewRowToTaxGroupDetailsTable();
                Session["PartDetails"] = dtDetails;
                Session["OATaxDetails"] = dtOATaxDetails;
                Session["OAGrpTaxDetails"] = dtOAGrpTaxDetails;
                BindDataToGrid();
                //Sujata 23062014 To set a tax grid we need to do following code
                CPEPartDetails.Collapsed = false;


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private DataTable GetListOfPartNo()
        {
            DataTable dtLayout = new DataTable();
            try
            {
                //DataTable dtLayout = new DataTable();
                if ((FileUpload1.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    DataSet ds = new DataSet();
                    string query = null;
                    string connString = "";
                    string strFileName = "SparesOA" + Location.iDealerId + DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        FileUpload1.SaveAs(Server.MapPath("~/DownloadFiles/SparesOA/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/SparesOA/" + strFileName + ".xlsx");

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

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
                    dtLayout = ds.Tables[0];

                }
                //return dtLayout;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtLayout;

        }
        private void SetListOfNotUploadedPartNo(DataTable dtPartList)
        {
            try
            {
                string sPartList = "";
                if (Func.Common.iRowCntOfTable(dtPartList) != 0)
                {
                    for (int iRowCnt = 0; iRowCnt < dtPartList.Rows.Count; iRowCnt++)
                    {
                        sPartList = sPartList + Func.Convert.sConvertToString(dtPartList.Rows[iRowCnt]["PartNo"]) + ",";
                    }
                    lblListPartNo.Text = "Parts which are not uploaded";
                    txtListPartNo.Text = sPartList;
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

        protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dschg = new DataSet();
            DealerID = Location.iDealerId;

            objOA = new clsSparesOA();
            dschg = objOA.GetOA(iOAID, "New", DealerID, Func.Convert.iConvertToInt(DrpCustomer.SelectedValue));

            if (dschg != null) // if no Data Exist
            {
                if (dschg.Tables.Count > 0)
                {
                    if (dschg.Tables[0].Rows.Count == 1)
                    {
                        hdnCustTaxTag.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["CustTaxTag"]);
                        hdnIsDocGST.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["DocGST"]);
                    }

                    Session["PartDetails"] = null;
                    Session["InvTaxDetails"] = null;
                    Session["InvGrpTaxDetails"] = null;

                    dtDetails = dschg.Tables[1];
                    Session["PartDetails"] = dtDetails;

                    dtOAGrpTaxDetails = dschg.Tables[2];
                    Session["InvGrpTaxDetails"] = dtOAGrpTaxDetails;

                    dtOATaxDetails = dschg.Tables[3];
                    Session["InvTaxDetails"] = dtOATaxDetails;

                    BindDataToGrid();
                }
            }
            hdnTrNo.Value = iDealerID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "OA" + "/" + Func.Convert.iConvertToInt(DrpCustomer.SelectedValue);
        }

        private int i = 1;
        protected void PartGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string txtPartID = (e.Row.FindControl("txtPartID") as TextBox).Text;
                Label lblNo = (e.Row.FindControl("lblNo") as Label);

                int no = e.Row.RowIndex;
                if (txtPartID != "0")
                {
                    lblNo.Text = i.ToString();
                    i++;
                }
                else
                {
                    (e.Row.FindControl("lblNo") as Label).Visible = false;
                    i = 1;
                }
            }
        }

        protected void rbtLstDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnConfirm.Value == "N")
                {
                    sDiscChange = true;
                    FillDetailsFromGrid(false, false);
                    BindDataToGrid();
                    sDiscChange = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void ddlGST_OA_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(ddlGST_OA_Type.SelectedValue) == 1)
                hdnGSTOAType.Value = "O";
            else
                hdnGSTOAType.Value = "N";
            int iCustID = Func.Convert.iConvertToInt(DrpCustomer.SelectedValue);

            DataSet dschg = new DataSet();
            DealerID = Location.iDealerId;

            objOA = new clsSparesOA();
            dschg = objOA.GetOA(iOAID, "New", DealerID, iCustID);

            if (dschg != null) // if no Data Exist
            {
                if (dschg.Tables.Count > 0)
                {
                    if (dschg.Tables[0].Rows.Count == 1)
                    {
                        hdnCustTaxTag.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["CustTaxTag"]);
                        hdnIsDocGST.Value = Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["DocGST"]);
                    }

                    Session["PartDetails"] = null;
                    Session["InvTaxDetails"] = null;
                    Session["InvGrpTaxDetails"] = null;

                    dtDetails = dschg.Tables[1];
                    Session["PartDetails"] = dtDetails;

                    dtOAGrpTaxDetails = dschg.Tables[2];
                    Session["InvGrpTaxDetails"] = dtOAGrpTaxDetails;

                    dtOATaxDetails = dschg.Tables[3];
                    Session["InvTaxDetails"] = dtOATaxDetails;

                    BindDataToGrid();
                }
            }
        }

    }
}