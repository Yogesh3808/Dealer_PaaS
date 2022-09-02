using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MANART.Forms.Spares
{
    public partial class frmStockAdjustment : System.Web.UI.Page
    {
        private int iStockAdjID = 0;
        string sFileName = "";
        int iDistributorID = 0;
        private DataTable dtDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        clsSpareStockAdj objStockAdj = null;
        string sNew = "Y";
        int TotCntQty = 0;
        private int dealerID = 0;
        int iUserId = 0;
        int iHOBr_id = 0;
        private int iDealerID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                ToolbarC.bUseImgOrButton = true;

                Location.bUseSpareDealerCode = true;
                Location.SetControlValue();
                clsSupplier ObjSup = new clsSupplier();
                DataSet dsDealer = new DataSet();
                DataSet ds = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);

                //if (iUserId != 0)
                //{
                //    dsDealer = ObjSup.GetDealerID(iUserId);

                //    if (dsDealer.Tables[0].Rows.Count > 0)
                //    {
                //        dealerID = Func.Convert.iConvertToInt(dsDealer.Tables[0].Rows[0]["DealerID"]);

                //        //  ds = ObjSup.GetMaxSupplier(dealerID);
                //    }

                //}
                //For MD User
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);

                if (txtUserType.Text == "6")
                {
                    iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }

                if (!IsPostBack)
                {
                    Session["StkAdjPart"] = null;
                    FillCombo();
                    DisplayPreviousRecord();
                }

                SearchGrid.sGridPanelTitle = " Stock Adjustment List";

                if (iStockAdjID != 0)
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
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(ddlGST_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (3,4) and isnull(Active,'N')='Y' ");
            ddlGST_Type.SelectedValue = "0";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Location.DealerSelectedIndexChanged += new EventHandler(Location_DealerSelectedIndexChanged);
            iDistributorID = Func.Convert.iConvertToInt(Session["iDealerID"]);
            iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
            // For MD User
            if (txtUserType.Text == "6")
            {
                iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);

            }


            FillSelectionGrid();
            if (!IsPostBack)
            {
                Session["StkAdjPart"] = null;
                DisplayPreviousRecord();
                GenerateStockAdjNo();
            }

        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iDistributorID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                FillSelectionGrid();
                Session["StkAdjPart"] = null;
                DisplayPreviousRecord();
                GenerateStockAdjNo();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            iStockAdjID = Func.Convert.iConvertToInt(txtID.Text);
            GetDataAndDisplay();
        }
        private void GetDataAndDisplay()
        {
            try
            {

                DataSet ds = new DataSet();
                objStockAdj = new clsSpareStockAdj();
                iDistributorID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                // For MD User
                if (txtUserType.Text == "6")
                {
                    iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                if (iStockAdjID != 0)
                {
                    // ds = objStockAdj.GetStockAdj(iStockAdjID, "All", Func.Convert.iConvertToInt(Session["iDealerID"].ToString()), 0);
                    ds = objStockAdj.GetStockAdj(iStockAdjID, "All", iDistributorID, 0);
                    sNew = "N";
                    DisplayData(ds);
                    objStockAdj = null;
                }
                else
                {
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                }
                ds = null;
                objStockAdj = null;
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
                iDistributorID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                if (txtUserType.Text == "6")
                {
                    iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    //dealerID = iDistributorID;
                }

                objStockAdj = new clsSpareStockAdj();
                ds = objStockAdj.GetStockAdj(iStockAdjID, "New", iDistributorID, 0);

                if (ds != null) // if no Data Exist
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            ds.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            ds.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";
                            if (Func.Convert.sConvertToString(hdnTrNo.Value) == "" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]) == "" && Func.Convert.sConvertToString(txtUserType.Text) != "6")
                            {
                                ds.Tables[0].Rows[0]["TrNo"] = iDistributorID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "STK";
                                hdnTrNo.Value = iDistributorID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "STK";
                            }
                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                //txtStockAdjNo.Text = Func.Common.sGetMaxDocNo(Location1.sDealerCode, "", "StockAdj", Func.Convert.iConvertToInt(Location1.iDealerId));
                txtStockAdjDate.Text = Func.Common.sGetCurrentDate(1, false);
                UploafFile.Style.Add("display", txtUserType.Text.Trim() == "6" ? "none" : "");
                objStockAdj = null;
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


                txtStockAdjNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["StockAdj_No"]);
                txtStockAdjDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["StockAdj_Date"]);
                txtStockAdjDate.Enabled = false;
                txtStockAdjDate.Style.Add("class", "NonEditableFields");
                txtReference.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Reference"]);
                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);
                hdnGSTType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTType"]);
                if (sNew == "N")
                    Func.Common.BindDataToCombo(ddlGST_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (3,4) ");
                else
                    Func.Common.BindDataToCombo(ddlGST_Type, clsCommon.ComboQueryType.GSTOAType, 0, "And ID in (3,4) and isnull(Active,'N')='Y' ");

                ddlGST_Type.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GSTType"]);
                if (Func.Convert.sConvertToString(ddlGST_Type.SelectedValue) == "3")
                    hdnGSTType.Value = "O";
                else
                    hdnGSTType.Value = "N";
                ddlGST_Type.Enabled = (sNew == "N") ? false : true;

                if (txtUserType.Text == "6")
                {
                    FillCombo();
                }

                txtStockAdjDate.Enabled = false;
                Session["StkAdjPart"] = null;

                dtDetails = ds.Tables[1];
                Session["StkAdjPart"] = dtDetails;
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    hdnConfirm.Value = "Y";
                }
                else
                {
                    hdnConfirm.Value = "N";
                }
                BindDataToGrid();
                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);
                // If Record is Confirm or cancel then it is not editable            
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Confirm"]) == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);

                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true);
                }
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Cancel"]) == "Y")
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
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void MakeEnableDisableControls(bool bEnable)
        {
            //Enable header Controls of Form        
            //txtStockAdjDate.Enabled = bEnable;
            //drpStockAdjType.Enabled = bEnable;

            PartGrid.Enabled = bEnable;
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, bEnable);
            ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, bEnable);

        }
        private void DisplayCurrentRecord()
        {
            try
            {

                DataSet ds = new DataSet();
                //int iDealerId = 3;
                iDistributorID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                //For MD User
                if (txtUserType.Text == "6")
                {
                    iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);

                }
                objStockAdj = new clsSpareStockAdj();
                ds = objStockAdj.GetStockAdj(iStockAdjID, "Max", iDistributorID, 0);
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
                    Session["StkAdjPart"] = null;
                    BindDataToGrid();

                }
                ds = null;
                objStockAdj = null;
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

                    if (Func.Convert.iConvertToInt(Session["iDealerID"]) != 0)
                    {
                        GenerateStockAdjNo();
                    }

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
                iStockAdjID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected override void OnPreRender(EventArgs e)
        {
            if (txtUserType.Text == "6")
            {
                iDistributorID = Func.Convert.iConvertToInt(Session["DealerID"]);
                FillSelectionGrid();
            }
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
                SearchGrid.AddToSearchCombo("StockAdj No");
                SearchGrid.AddToSearchCombo("StockAdj Date");
                // SearchGrid.iDealerID = iDistributorID;
                SearchGrid.iDealerID = iDistributorID;
                SearchGrid.sModelPart = Func.Convert.sConvertToString(iUserId);
                SearchGrid.sSqlFor = "SparesStockAdj";
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
                dtHdr.Columns.Add(new DataColumn("StockAdj_No", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("StockAdj_Date", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Reference", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Is_Cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserId", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("GSTType", typeof(int)));

                dr = dtHdr.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["StockAdj_No"] = txtStockAdjNo.Text;
                dr["StockAdj_Date"] = txtStockAdjDate.Text;
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(Session["iDealerID"]);
                dr["Reference"] = txtReference.Text.Trim();
                dr["UserId"] = iUserId;
                dr["Is_Confirm"] = "N";
                dr["Is_Cancel"] = "N";
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);
                dr["GSTType"] = Func.Convert.iConvertToInt(ddlGST_Type.SelectedValue);
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
            if (txtStockAdjDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
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
            clsSpareStockAdj objStockAdj = new clsSpareStockAdj();
            try
            {
                if (bValidateRecord() == false)
                {
                    return false;
                }
                if (PartGrid.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }
                UpdateHdrValueFromControl(dtHdr);
                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["Is_Confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["Is_Cancel"] = "Y";
                }
                //Get Part Details     
                bDetailsRecordExist = false;
                FillDetailsFromGrid(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                    return false;
                }

                if (objStockAdj.bSaveRecord(Location.sDealerCode, Func.Convert.iConvertToInt(Session["iDealerID"]), dtHdr, dtDetails, ref iStockAdjID) == true)
                {
                    txtID.Text = Func.Convert.sConvertToString(iStockAdjID);
                    if (bSaveWithConfirm == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7,'" + Server.HtmlEncode("Stock Adjustment ") + "','" + Server.HtmlEncode(txtStockAdjNo.Text) + "');</script>");
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8,'" + Server.HtmlEncode("Stock Adjustment  ") + "','" + Server.HtmlEncode(txtStockAdjNo.Text) + "');</script>");
                        return true;
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4,'" + Server.HtmlEncode("Stock Adjustment  ") + "','" + Server.HtmlEncode(txtStockAdjNo.Text) + "');</script>");
                        return true;
                    }
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5,'" + Server.HtmlEncode("Stock Adjustment  ") + "','" + Server.HtmlEncode(txtStockAdjNo.Text) + "');</script>");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }
        private void GenerateStockAdjNo()
        {
            objStockAdj = new clsSpareStockAdj();
            txtStockAdjNo.Text = Func.Convert.sConvertToString(objStockAdj.GenerateStockAdj(Func.Convert.sConvertToString(Session["sDealerCode"]), iDealerID));
        }
        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            //DataTable dtExistRecord = null;
            //DataTable dtSysStock = null;
            //StringBuilder sPartIDs = null;
            try
            {
                //objStockAdj = new clsSpareStockAdj();
                //dtSysStock = new DataTable();
                //dtExistRecord = new DataTable();
                //dtExistRecord = (DataTable)Session["StkAdjPart"];
                //if (dtExistRecord != null)
                //{
                //    sPartIDs = new StringBuilder();
                //    for (int icnt = 0; icnt < dtExistRecord.Rows.Count; icnt++)
                //    {
                //        if (sPartIDs.Length == 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //            sPartIDs.Append(Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //        else if (sPartIDs.Length > 0 && Func.Convert.iConvertToInt(dtExistRecord.Rows[icnt]["Part_ID"]) != 0)
                //            sPartIDs.Append("," + Func.Convert.sConvertToString(dtExistRecord.Rows[icnt]["Part_ID"]));
                //    }
                //dtSysStock = objStockAdj.GetSysStock(Func.Convert.sConvertToString(sPartIDs), iUserId, "StockAdjustment", Func.Convert.iConvertToInt(Session["iDealerID"]), Func.Convert.iConvertToInt(txtID.Text));
                //dtSysStock = objStockAdj.GetSysStock(Func.Convert.sConvertToString(sPartIDs), 3, "StockAdjustment", Func.Convert.iConvertToInt(Session["iDealerID"]), Func.Convert.iConvertToInt(txtID.Text));
                //if (dtSysStock != null && dtSysStock.Rows.Count > 0)
                //    Session["StkAdjPart"] = dtSysStock;
                //}
                FillDetailsFromGrid(false);
                BindDataToGrid();
                FillDetailsFromGrid(false);
                Session["StkAdjPart"] = dtDetails;
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objStockAdj != null) objStockAdj = null;
                //if (dtSysStock != null) dtSysStock = null;
                //if (dtExistRecord != null) dtExistRecord = null;
                //if (sPartIDs != null) sPartIDs = null;
            }

        }
        private void FillDetailsFromGrid(bool bDisplayMsg)
        {
            try
            {
                string sStatus = "";
                string strStockAdjsNo = "";
                dtDetails = (DataTable)Session["StkAdjPart"];

                int iCntForDelete = 0;
                int iCntForSelect = 0;
                int iCntError = 0;
                int iCntError1 = 0;
                string sQtyMsg = "";
                if (dtDetails.Rows.Count > 0)
                {
                    for (int iMRowCnt = 0; iMRowCnt < dtDetails.Rows.Count; iMRowCnt++)
                    {
                        for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                        {
                            //PartID                
                            TextBox txtPartID = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartID");
                            TextBox txtStatus = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtStatus");
                            CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");

                            if (Func.Convert.iConvertToInt(dtDetails.Rows[iMRowCnt]["Part_ID"]) == Func.Convert.iConvertToInt(txtPartID.Text))
                            {
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "U") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "U")
                                    {
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
                                    }
                                    iCntForSelect = iCntForSelect + 1;
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "O") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "O")
                                    {
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                if ((Func.Convert.iConvertToInt(txtPartID.Text) == 0 && txtStatus.Text == "M") || txtStatus.Text == "N")
                                {
                                    if (txtStatus.Text == "M")
                                    {
                                        if (strStockAdjsNo == "")
                                            strStockAdjsNo = iRowCnt.ToString();
                                        else
                                            strStockAdjsNo = strStockAdjsNo + "," + iRowCnt.ToString();
                                    }
                                    continue;
                                }
                                dtDetails.Rows[iMRowCnt]["Part_ID"] = Func.Convert.iConvertToInt(txtPartID.Text);

                                if (txtPartID.Text != "" && txtPartID.Text != "0" && Chk.Checked == false)
                                {
                                    iCntForSelect = iCntForSelect + 1;
                                }

                                //PartNo Or NewPart
                                TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");
                                dtDetails.Rows[iMRowCnt]["Part_No"] = txtPartNo.Text;

                                //Part Name
                                TextBox txtPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                                dtDetails.Rows[iMRowCnt]["Part_Name"] = txtPartName.Text;

                                //Group code
                                TextBox txtGrNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtGrNo");
                                dtDetails.Rows[iRowCnt]["group_code"] = txtGrNo.Text;

                                // Get Sys Qty
                                TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                                dtDetails.Rows[iMRowCnt]["Qty"] = Func.Convert.dConvertToDouble(txtQuantity.Text);

                                // Get Physical Qty
                                TextBox txtPhysicalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPhysicalQty");
                                dtDetails.Rows[iMRowCnt]["Physical_Qty"] = Func.Convert.dConvertToDouble(txtPhysicalQty.Text);

                                // Get Inward Qty
                                TextBox txtInwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInwQty");
                                dtDetails.Rows[iMRowCnt]["Inward_Qty"] = Func.Convert.dConvertToDouble(txtInwQty.Text);

                                // Get Outward Qty
                                TextBox txtOutwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOutwQty");
                                dtDetails.Rows[iMRowCnt]["Outward_Qty"] = Func.Convert.dConvertToDouble(txtOutwQty.Text);

                                // Get Reason
                                TextBox txtReason = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtReason");
                                dtDetails.Rows[iMRowCnt]["Reason"] = txtReason.Text.Trim();

                                // Get Status                           
                                dtDetails.Rows[iMRowCnt]["Status"] = txtStatus.Text;

                                // Get Outward Qty
                                TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                                dtDetails.Rows[iMRowCnt]["BFRGST"] = Func.Convert.sConvertToString(txtBFRGST.Text);

                                // Get Reason
                                TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");
                                dtDetails.Rows[iMRowCnt]["BFRGST_Stock"] = Func.Convert.dConvertToDouble(txtBFRGST_Stock.Text);

                                sStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);


                                if (Chk.Checked == true)
                                {
                                    dtDetails.Rows[iRowCnt]["Status"] = "D";
                                    iCntForDelete++;
                                }
                                else
                                {
                                    if (txtPartID.Text != "")
                                    {
                                        if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "M")
                                        {
                                            dtDetails.Rows[iMRowCnt]["Status"] = "M";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() == "U")
                                        {
                                            dtDetails.Rows[iRowCnt]["Status"] = "U";
                                            bDetailsRecordExist = true;
                                        }
                                        else if (dtDetails.Rows[iMRowCnt]["Status"].ToString() != "U" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "N" && dtDetails.Rows[iMRowCnt]["Status"].ToString() != "C")
                                        {
                                            dtDetails.Rows[iRowCnt]["Status"] = "E";
                                            bDetailsRecordExist = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    int iPartID = 0;
                    int iGSTTypeID = 0;
                    double dCurrStock = 0.00;
                    double dPhyQty = 0.00;
                    double dBFRGstStock = 0.00;
                    string sReason = "", sRecoredStatus = "", sBFRGST = "", sPartNo = "";
                    double dInwQty = 0.00;
                    double dOutQty = 0.00;
                    for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                    {
                        iPartID = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Part_ID"]);
                        dCurrStock = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Qty"]);
                        dPhyQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Physical_Qty"]);
                        sReason = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Reason"]);
                        sRecoredStatus = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Status"]);
                        dBFRGstStock = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["BFRGST_Stock"]);
                        sBFRGST = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["BFRGST"]);
                        iGSTTypeID = Func.Convert.iConvertToInt(ddlGST_Type.SelectedValue);
                        dInwQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Inward_Qty"]);
                        dOutQty = Func.Convert.dConvertToDouble(dtDetails.Rows[iCnt]["Outward_Qty"]);
                        sPartNo = Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Part_No"]);


                        if (iPartID != 0 && sRecoredStatus != "C" && sRecoredStatus != "D" && sQtyMsg == "")
                        {
                            if (dPhyQty == 0.00 && dInwQty == 0.00 && dOutQty == 0.00)
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please enter Physical Quantity at Row No " + (iCnt) + " / Part No is " + sPartNo;
                            }
                            if (iPartID != 0 && dCurrStock != dPhyQty && sReason == "" && iGSTTypeID == 4)
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please enter Reason at Row No " + (iCnt) + " / Part No is " + sPartNo;
                            }
                            if (dBFRGstStock != dPhyQty && sReason == "" && iGSTTypeID == 3)
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Please enter Reason at Row No " + (iCnt) + " / Part No is " + sPartNo;
                            }
                            if (sBFRGST == "Y" && (Func.Convert.iConvertToInt(ddlGST_Type.SelectedValue) == 4) && dPhyQty < dBFRGstStock)
                            {
                                iCntError = iCntError + 1;
                                sQtyMsg = "Pre GST Stock is " + dBFRGstStock + ". " + " Physical Qty should be >= " + dBFRGstStock + " at Row No " + (iCnt) + " / Part No is " + sPartNo;
                            }
                            if ((Func.Convert.iConvertToInt(ddlGST_Type.SelectedValue) == 3) && sBFRGST == "Y")
                            {
                                if (dPhyQty > dBFRGstStock)
                                {
                                    iCntError = iCntError + 1;
                                    sQtyMsg = "Pre GST Stock is " + dBFRGstStock + ". " + " Physical Qty should be <= " + dBFRGstStock + " at Row No " + (iCnt) + " / Part No is " + sPartNo;
                                }
                                if (dPhyQty > dCurrStock && sQtyMsg == "")
                                {
                                    iCntError = iCntError + 1;
                                    sQtyMsg = "Post GST Stock is  " + dCurrStock + ". " + " Physical Qty should be <= " + dCurrStock + " at Row No " + (iCnt) + " / Part No is " + sPartNo;
                                }
                            }
                        }

                        //if (sQtyMsg != "")
                        //{
                        //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
                        //    bDetailsRecordExist = false;
                        //    break;
                        //}
                    }


                    if (iCntForDelete == iCntForSelect)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter atleast One Record !');</script>");
                        bDetailsRecordExist = false;
                    }
                    else if (iCntError > 0)
                    {
                        if (bDisplayMsg == true) Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sQtyMsg + "');</script>");
                        bDetailsRecordExist = false;
                    }
                }
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
                    dtDetails.Columns.Add(new DataColumn("SRNo", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Part_No", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Group_code", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MOQ", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("MRPRate", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Physical_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Inward_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Outward_Qty", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Status", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("Reason", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST", typeof(String)));
                    dtDetails.Columns.Add(new DataColumn("BFRGST_Stock", typeof(double)));


                }
                else if (dtDetails.Rows.Count == 1)
                {
                    if (dtDetails.Rows[0]["ID"].ToString() == "0" && dtDetails.Rows[0]["Part_ID"].ToString() == "0")
                    {
                        goto Exit;
                    }
                }
                dr = dtDetails.NewRow();
                dr["SRNo"] = "1";
                dr["ID"] = 0;
                dr["Part_ID"] = 0;
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Group_code"] = "";
                dr["Qty"] = 0.00;
                dr["MOQ"] = 1;
                dr["MRPRate"] = 0.00;
                dr["Total"] = 0.00;
                dr["Physical_Qty"] = 0.00;
                dr["Inward_Qty"] = 0.00;
                dr["Outward_Qty"] = 0.00;
                dr["Status"] = "N";
                dr["Reason"] = "";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;
                //dtDetails.Rows.Add(dr);
                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();

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
                if (Session["StkAdjPart"] == null)
                {
                    CreateNewRowToDetailsTable();
                    Session["StkAdjPart"] = dtDetails;
                }
                else
                {
                    dtDetails = (DataTable)Session["StkAdjPart"];
                    if (Func.Convert.sConvertToString(dtDetails.Rows[0]["Part_ID"]) != "0" && hdnConfirm.Value == "N")
                        CreateNewRowToDetailsTable();
                    dtDetails = (DataTable)Session["StkAdjPart"];
                }
                PartGrid.DataSource = dtDetails;
                PartGrid.DataBind();
                SetGridControlProperty();
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



                if (PartGrid.Rows.Count == 0) return;
                PartGrid.HeaderRow.Cells[1].Style.Add("display", "none");

                string sDealerId = Func.Convert.sConvertToString(Session["iDealerID"]);
                string sPartID = "", sPartName = "", cPartID = "";
                for (int iRowCnt = 0; iRowCnt < PartGrid.Rows.Count; iRowCnt++)
                {

                    PartGrid.HeaderRow.Cells[5].Text = (hdnGSTType.Value == "O") ? "PreGST Phy Qty" : "Physical Qty"; // Hide Header    
                    PartGrid.HeaderRow.Cells[6].Text = (hdnGSTType.Value == "O") ? "PreGST Inward Qty" : "Inward Qty"; // Hide Header    
                    PartGrid.HeaderRow.Cells[7].Text = (hdnGSTType.Value == "O") ? "PreGST Outward Qty" : "Outward Qty"; //

                    LinkButton lnkSelectPart = (LinkButton)PartGrid.Rows[iRowCnt].FindControl("lnkSelectPart");

                    lnkSelectPart.Attributes.Add("onclick", "return ShowMultiPartSearchfromStockAdj(this,'" + sDealerId + "','" + ddlGST_Type.ID + "');");
                    sPartID = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Part_ID"]);
                    TextBox txtGPartName = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartName");
                    TextBox txtPartNo = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPartNo");

                    TextBox txtQuantity = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtQuantity");
                    TextBox txtPhysicalQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtPhysicalQty");
                    TextBox txtInwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInwQty");
                    TextBox txtOutwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOutwQty");
                    TextBox txtreason = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtreason");
                    TextBox txtBFRGST = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST");
                    TextBox txtBFRGST_Stock = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtBFRGST_Stock");

                    if (sPartID != "" && sPartID != "0")
                    {
                        double dSysQty = 0.00;
                        double dPhysicalQty = 0.00;
                        double dBFRGST_Stock = 0.00;
                        double dInwQty = 0.00;
                        double dOutQty = 0.00;
                        dSysQty = Func.Convert.dConvertToDouble(txtQuantity.Text);
                        dPhysicalQty = Func.Convert.dConvertToDouble(txtPhysicalQty.Text);
                        dBFRGST_Stock = Func.Convert.dConvertToDouble(txtBFRGST_Stock.Text);
                        if (hdnConfirm.Value == "N")
                        {
                            if (Func.Convert.sConvertToString(ddlGST_Type.SelectedValue) == "3")
                            {
                                txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                if (dSysQty > dBFRGST_Stock)
                                {
                                    dOutQty = dBFRGST_Stock - dPhysicalQty;
                                    txtOutwQty.Text = dOutQty < 0 ? Func.Convert.sConvertToString("0.00") : Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                }
                                if (dBFRGST_Stock > dSysQty)
                                {
                                    dOutQty = dSysQty - dPhysicalQty;
                                    txtOutwQty.Text = dOutQty < 0 ? Func.Convert.sConvertToString("0.00") : Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                }
                                if (dBFRGST_Stock == dSysQty)
                                {
                                    dOutQty = dSysQty - dPhysicalQty;
                                    txtOutwQty.Text = dOutQty < 0 ? Func.Convert.sConvertToString("0.00") : Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                }
                            }
                            else
                            {
                                if (dSysQty > dBFRGST_Stock)
                                {//A
                                    //txtInwQty.Text = (dPhysicalQty >= dBFRGST_Stock && dSysQty > dPhysicalQty) ? Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                    if (dPhysicalQty >= dBFRGST_Stock && dSysQty > dPhysicalQty)
                                    {//1
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                        dOutQty = dSysQty - dPhysicalQty;
                                        txtOutwQty.Text = Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                    }//1
                                    else if (dPhysicalQty >= dBFRGST_Stock && dPhysicalQty > dSysQty)
                                    {//2
                                        dInwQty = dPhysicalQty - dSysQty;
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                        dOutQty = 0.00;
                                        txtOutwQty.Text = Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                    }//2
                                }//A
                                else if (dBFRGST_Stock > dSysQty)
                                {//B
                                    if (dPhysicalQty >= dBFRGST_Stock && dSysQty > dPhysicalQty)
                                    {//1
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                    }//1
                                    else if (dPhysicalQty >= dBFRGST_Stock && dPhysicalQty > dSysQty)
                                    {//2
                                        dInwQty = dPhysicalQty - dSysQty;
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                    }//2
                                    txtOutwQty.Text = Func.Convert.sConvertToString(dOutQty.ToString("0.00"));

                                }//B
                                else if (dBFRGST_Stock == dSysQty)
                                {
                                    if (dPhysicalQty >= dBFRGST_Stock && dSysQty > dPhysicalQty)
                                    {//1
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                    }//1
                                    else if (dPhysicalQty >= dBFRGST_Stock && dPhysicalQty > dSysQty)
                                    {//2
                                        dInwQty = dPhysicalQty - dSysQty;
                                        txtInwQty.Text = Func.Convert.sConvertToString(dInwQty.ToString("0.00"));
                                    }//2
                                    txtOutwQty.Text = Func.Convert.sConvertToString(dOutQty.ToString("0.00"));
                                }
                            }
                        }
                    }

                    // if (txtPhysicalQty.Text == "")
                    //     txtPhysicalQty.Text = txtQuantity.Text;
                    // dSysQty = Func.Convert.dConvertToDouble(txtQuantity.Text);
                    // dPhysicalQty = Func.Convert.dConvertToDouble(txtPhysicalQty.Text);
                    // //TextBox txtInwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtInwQty");
                    // txtInwQty.Text = (dPhysicalQty > dSysQty) ? Func.Convert.sConvertToString((dPhysicalQty - dSysQty)) : "0";
                    //// TextBox txtOutwQty = (TextBox)PartGrid.Rows[iRowCnt].FindControl("txtOutwQty");
                    // txtOutwQty.Text = (dSysQty > dPhysicalQty) ? Func.Convert.sConvertToString((dSysQty - dPhysicalQty)) : "0";

                    //Status
                    sRecordStatus = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Status"]);

                    if (sPartID == "0")
                    {
                        lnkSelectPart.Style.Add("display", "");
                        txtGPartName.Style.Add("display", "none");
                        txtQuantity.Style.Add("display", "none");
                        txtPhysicalQty.Style.Add("display", "none");
                        txtInwQty.Style.Add("display", "none");
                        txtOutwQty.Style.Add("display", "none");
                        txtreason.Style.Add("display", "none");
                        txtBFRGST_Stock.Style.Add("display", "none");
                    }
                    else
                    {
                        lnkSelectPart.Style.Add("display", "none");
                    }

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


                    //Delete 
                    CheckBox Chk = (CheckBox)PartGrid.Rows[iRowCnt].FindControl("ChkForDelete");
                    Chk.Style.Add("display", "none");
                    Label lblDelete = (Label)PartGrid.Rows[iRowCnt].FindControl("lblDelete");
                    lblDelete.Style.Add("display", "none");


                    //If Record is uploaded then it is not allowed to change
                    if (sRecordStatus.Trim() == "U")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                    }
                    if (sRecordStatus.Trim() == "M")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                        if (sPartID == "0" && sRecordStatus == "M")
                        {
                            // lnkSelectPart.Style.Add("display", "none");
                            PartGrid.Rows[iRowCnt].Visible = false;
                        }

                    }


                    //If Part Id  is not allocated           
                    if (sRecordStatus.Trim() == "D")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = true;
                        lblDelete.Style.Add("display", "");
                        //PartGrid.Rows[iRowCnt].BackColor = Color.Orange;
                    }
                    else if (sRecordStatus.Trim() == "E")
                    {
                        Chk.Style.Add("display", "");
                        lblDelete.Style.Add("display", "");
                        Chk.Checked = false;
                    }
                    else if (sRecordStatus.Trim() == "P")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");
                    }
                    else if (sRecordStatus.Trim() == "O")
                    {
                        Chk.Style.Add("display", "");
                        Chk.Checked = false;
                        lblDelete.Style.Add("display", "");

                        if ((iRowCnt + 1) == PartGrid.Rows.Count)
                        {
                            //lnkNew.Style.Add("display", "");
                        }

                    }
                    else if (sRecordStatus.Trim() == "C")
                    {
                        lblDelete.Style.Add("display", "");
                        Chk.Checked = true;
                    }
                    if (txtUserType.Text == "6")
                    {
                        lnkSelectPart.Style.Add("display", "none");

                    }

                }


                //txtTotal.Text = dTotal.ToString("0.00");
                //txtTotalQty.Text = dTotalQty.ToString("0");

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void PartGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Label RFPtatus = (Label)PartGrid.Rows[0].FindControl("lblRFPStatus");
                if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "New")
                {
                    FillDetailsFromGrid(false);
                    CreateNewRowToDetailsTable();
                    Session["StkAdjPart"] = dtDetails;
                    BindDataToGrid();
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

                dtDetails = (DataTable)Session["StkAdjPart"];
                PartGrid.DataSource = dtDetails;
                PartGrid.PageIndex = e.NewPageIndex;
                PartGrid.DataBind();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        #region Xlsx Upload For Stock Adjustment
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                objStockAdj = new clsSpareStockAdj();
                DataTable dt = GetListOfPartNo();

                int iCountNotUpload = 0;

                string sFileNameForSave = "";
                DataSet dsUploadPartDetails;
                System.Threading.Thread.Sleep(2000);
                sFileName = FileUpload1.FileName;
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));

                dsUploadPartDetails = objStockAdj.UploadPartDetailsAndGetPartDetails(FileUpload1.FileName, Func.Convert.iConvertToInt(Session["iDealerID"]), 0, dt, hdnGSTType.Value, iUserId);
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
                dr["Part_No"] = "";
                dr["Part_Name"] = "";
                dr["Qty"] = 0.00;
                dr["Physical_Qty"] = 0.00;
                dr["Inward_Qty"] = 0.00;
                dr["Outward_Qty"] = 0.00;
                dr["MRPRate"] = 0.00;
                dr["Total"] = 0.00;
                dr["Reason"] = "";
                dr["Status"] = "N";
                dr["Reason"] = "";
                dr["BFRGST"] = "N";
                dr["BFRGST_Stock"] = 0.00;

                dtDetails.Rows.InsertAt(dr, 0);
                dtDetails.AcceptChanges();
                Session["StkAdjPart"] = dtDetails;
                BindDataToGrid();
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
                    string strFileName = "StockAdj" + Func.Convert.sConvertToString(Session["iDealerID"]) + DateTime.Now.ToString("ddMMyyyy_HHmmss");//ExportLocation.iDealerId
                    string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();

                    //Check file type
                    if (strFileType == ".xls" || strFileType == ".xlsx")
                    {
                        FileUpload1.SaveAs(Server.MapPath("~/DownloadFiles/StockAdj/" + strFileName + ".xlsx"));
                    }
                    else
                    {
                        lblMessage.Text = "Only excel files allowed";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        return null;
                    }

                    string strNewPath = Server.MapPath("~/DownloadFiles/StockAdj/" + strFileName + ".xlsx");

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
        #endregion
        int i = 1;
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
        protected void ddlGST_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Func.Convert.iConvertToInt(ddlGST_Type.SelectedValue) == 3)
                    hdnGSTType.Value = "O";
                else
                    hdnGSTType.Value = "N";
                UploafFile.Style.Add("display", hdnGSTType.Value.Trim() == "O" ? "none" : "");

                DataSet dschg = new DataSet();
                objStockAdj = new clsSpareStockAdj();
                dschg = objStockAdj.GetStockAdj(iStockAdjID, "New", iDealerID, 0);


                if (dschg != null) // if no Data Exist
                {
                    if (dschg.Tables.Count > 0)
                    {
                        if (dschg.Tables[0].Rows.Count == 1)
                        {
                            dschg.Tables[0].Rows[0]["Is_Cancel"] = "N";
                            dschg.Tables[0].Rows[0]["Is_Confirm"] = "N";
                            dschg.Tables[1].Rows[0]["Status"] = "N";
                            sNew = "Y";

                            if (Func.Convert.sConvertToString(hdnTrNo.Value) == "" && Func.Convert.sConvertToString(dschg.Tables[0].Rows[0]["TrNo"]) == "" && Func.Convert.sConvertToString(txtUserType.Text) != "6")
                            {
                                dschg.Tables[0].Rows[0]["TrNo"] = iDistributorID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "STK";
                                hdnTrNo.Value = iDistributorID + "/" + iUserId + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss").Trim()) + "/" + "STK";
                            }
                        }

                        Session["StkAdjPart"] = null;
                        dtDetails = dschg.Tables[1];
                        Session["StkAdjPart"] = dtDetails;

                        BindDataToGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

    }
}