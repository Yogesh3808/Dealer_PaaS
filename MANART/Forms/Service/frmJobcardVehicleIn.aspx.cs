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
using System.Globalization;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Service
{
    public partial class frmJobcardVehicleIn : System.Web.UI.Page
    {
        private DataTable dtDetails = new DataTable();
        private DataTable dtIncoDetails = new DataTable();
        private int iDocID = 0;
        int iDealerId;
        int iUserID = 0;
        string sDealerCode;
        int iHOBranchDealerId;
        private bool bDetailsRecordExist = false;
        string sNew = "N";
        protected void Page_Init(object sender, EventArgs e)
        {
            ToolbarC.bUseImgOrButton = true;
            //ToolbarC.iFormIdToOpenForm = 1;
            //ToolbarC.iValidationIdForConfirm = 3;
            //ToolbarC.iValidationIdForSave = 3;      

            Location.bUseSpareDealerCode = true;
            Location.SetControlValue();
            //MDUser Change
            txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
            txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
            
            if (txtUserType.Text == "6")
            {
                Location.iDealerId = Func.Convert.iConvertToInt(Session["DealerID"]);
                Session["HOBR_ID"] = Func.Convert.sConvertToString(Session["DealerID"]);
            }
            //MDUser Change
            if (!IsPostBack)
            {
                Session["AccDetails"] = null;
                DisplayPreviousRecord();
            }
            SearchGrid.sGridPanelTitle = "Vehicle Reporting Time List";
            FillSelectionGrid();

            if (iDocID != 0)
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
            SetDocumentDetails();

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillCombo();
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
                if (!IsPostBack)
                {
                    Session["AccDetails"] = null;

                    DisplayPreviousRecord();

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void Location_DealerSelectedIndexChanged(object sender, EventArgs e)
        {

            FillCombo();
            FillSelectionGrid();
            PSelectionGrid.Style.Add("display", "");
            ClearFormControl();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //MDUser Change
            if (txtUserType.Text == "6")
            {
                FillSelectionGrid();
            }//MDUser Change
            string strDisAbleBackButton;
            strDisAbleBackButton = "<SCRIPT language=javascript>\n";
            strDisAbleBackButton += "window.history.forward(1);\n";
            strDisAbleBackButton += "\n</SCRIPT>";
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
            btnReadonly();
        }
        // Function Use for Readonly User
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15" || objCommon.sUserRole == "19")
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
        // set Document text 
        private void SetDocumentDetails()
        {
            lblTitle.Text = "Vehicles Reporting Time";
            lblDocNo.Text = "Tr No.:";
            lblDocDate.Text = "Time In:";
            //lblSelectModel.Attributes.Add("onclick", " return ShowChassisMaster(this,'" + Location.iDealerId.ToString() + "','" + Session["HOBR_ID"].ToString() + "')");
        }

        // FillCombo
        private void FillCombo()
        {
            //if (Location.iDealerId != 0)
            //{            
            //    Func.Common.BindDataToCombo(drpPaymentTerms, clsCommon.ComboQueryType.PaymentTerms, 0, " And Dealer_Id='" + Location.iDealerId + "' or PaymentTerm_des = 'Others'");//" And Dealer_Id='" + Location.iDealerId + "' And PaymentTerm_des <> 'Others'"         
            //    Func.Common.BindDataToCombo(drpModeofShipment, clsCommon.ComboQueryType.ModeofDispatch, Location.iDealerId);
            //    Func.Common.BindDataToCombo(drpIncoTerms, clsCommon.ComboQueryType.INCOTerms, Location.iDealerId);
            //}
            //Func.Common.BindDataToCombo(drpValidityDays, clsCommon.ComboQueryType.ValidityDays, 0);
            //Func.Common.BindDataToCombo(drpMultiModalDestination, clsCommon.ComboQueryType.ModalDestinationForCountry, 0);
            //Func.Common.BindDataToCombo(drpNominatedAgency, clsCommon.ComboQueryType.NominatedAgency, 0);
            //Func.Common.BindDataToCombo(drpThirdPartyInspectionAgency, clsCommon.ComboQueryType.ThirdPartyInspectionAgency, 0);
            //Func.Common.BindDataToCombo(drpHdrShipmentDays, clsCommon.ComboQueryType.UsanceDays, 0);
            //Func.Common.BindDataToCombo(drpVECVBank, clsCommon.ComboQueryType.BankStateMent, 0);        
            //Func.Common.FillLCClause(LCClauseList);
        }

        //BindData to Grid
        private void BindDataToGrid()
        {
            //If No Data in Grid
            if (Session["AccDetails"] == null)
            {
                Session["AccDetails"] = dtDetails;
            }
            else
            {
                dtDetails = (DataTable)Session["AccDetails"];
            }
            Session["AccDetails"] = dtDetails;
            DetailsGrid.DataSource = dtDetails;
            DetailsGrid.DataBind();
            SetGridControlProperty(false);
        }

        // Set Control property of the Grid    
        private void SetGridControlProperty(bool bClearIncoAmount)
        {
            //int iIncoTermId = 0;
            //DataTable dtIncoColumns;
            //int iCurentColumnId = 8;
            //int iIncoTermColumnId = 0;
            //int iIncoRowCnt = 0;
            //double dIncoAmount = 0;
            //double dTotalAmount = 0;
            //double dLineTotal = 0;
            //int iIncoDataCnt = 0;
            //double dQty = 0, dFobRate = 0;
            //bool bHideColumn = false;

            //if (DetailsGrid.Rows.Count == 0) return;

            //iIncoTermId = Func.Convert.iConvertToInt(drpIncoTerms.SelectedValue);

            //clsVehicleReportingTime ObjVehReportingTm = new clsVehicleReportingTime();
            //dtIncoColumns = ObjVehReportingTm.GetIncoTermsDetails(iIncoTermId);
            //ObjVehReportingTm = null;

            //if (dtIncoColumns == null)
            //{
            //    return;
            //}
            ////set hide  all Columns             



            //DetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none"); // Hide Freight //Now its a Original Rate (as per incoterms it is a Max)
            //if (drpShowBreakup.SelectedValue == "Y")
            //{
            //    DetailsGrid.HeaderRow.Cells[11].Style.Add("display", "");// Hide Insurance //Now its a FOB Max
            //    DetailsGrid.HeaderRow.Cells[12].Style.Add("display", "");// Hide Carriage //Now its a Freight 
            //    DetailsGrid.HeaderRow.Cells[13].Style.Add("display", "");// Hide Commission //Now its a Insurance
            //}
            //else
            //{
            //    DetailsGrid.HeaderRow.Cells[11].Style.Add("display", "none");// Hide Insurance //Now its a FOB Max
            //    DetailsGrid.HeaderRow.Cells[12].Style.Add("display", "none");// Hide Carriage //Now its a Freight 
            //    DetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none");// Hide Commission //Now its a Insurance
            //}
            //DetailsGrid.HeaderRow.Cells[14].Style.Add("display", "none");// Hide OtherCharges
            //DetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");// Hide Taxes
            ////if(drpShowDiscount.SelectedValue=="N")
            ////DetailsGrid.HeaderRow.Cells[16].Style.Add("display", "none");// Hide Discount
            ////else
            //DetailsGrid.HeaderRow.Cells[16].Style.Add("display", "");//Show Discount  
            ////Display Columns as Per Link to Inco Terms   
            //txtMarineIns.Text = "";
            //for (int iGridRowCnt = 0; iGridRowCnt < DetailsGrid.Rows.Count; iGridRowCnt++) // Read Rows
            //{
            //    TextBox txtUnitAmount = (TextBox)DetailsGrid.Rows[iGridRowCnt].FindControl("txtUnitAmount");
            //    //txtUnitAmount.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");           

            //    DropDownList drpShipmentDays = (DropDownList)DetailsGrid.Rows[iGridRowCnt].FindControl("drpShipmentDays");
            //    Func.Common.BindDataToCombo(drpShipmentDays, clsCommon.ComboQueryType.ShipmentDays, 0);
            //    drpShipmentDays.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iGridRowCnt]["ShipmentDays_Id"]);
            //    TextBox txtHeadUserIDDisc = (TextBox)DetailsGrid.Rows[iGridRowCnt].FindControl("txtHeadUserIDDisc");
            //    txtUnitAmount.Enabled = (txtHeadUserIDDisc.Text == "1" && txtUserRoleID.Text != "1") ? false : true;

            //    iIncoRowCnt = 0;
            //    TextBox txtDiscount = (TextBox)DetailsGrid.Rows[iGridRowCnt].FindControl("txtDiscount");

            //    //if (drpShowDiscount.SelectedValue == "N")
            //    //DetailsGrid.Rows[iGridRowCnt].Cells[16].Style.Add("display", "none");//Hide Cell
            //    //else
            //    //DetailsGrid.Rows[iGridRowCnt].Cells[16].Style.Add("display", "");//Show Cell
            //    DetailsGrid.Rows[iGridRowCnt].Cells[17].Style.Add("display", "");//Show Cell

            //    //iTabIndex = txtDiscount.TabIndex;
            //    //for (int iGridColCnt = 10; iGridColCnt <= 15; iGridColCnt++)// Read Column from Freight to Taxes
            //    for (int iGridColCnt = 10; iGridColCnt <= 15; iGridColCnt++)// Read Column from Freight to Taxes
            //    {
            //        bHideColumn = true;

            //        if (bHideColumn == true && (iGridColCnt != 11 && iGridColCnt != 12 && iGridColCnt != 13))
            //        {
            //            if (iGridColCnt != 10) (DetailsGrid.Rows[iGridRowCnt].Cells[iGridColCnt].Controls[1] as TextBox).Text = "";
            //            DetailsGrid.HeaderRow.Cells[iGridColCnt].Style.Add("display", "none"); // Hide Header        
            //            DetailsGrid.Rows[iGridRowCnt].Cells[iGridColCnt].Style.Add("display", "none");//Hide Cell
            //        }
            //        else if (iGridColCnt == 11 || iGridColCnt == 12 || iGridColCnt == 13)
            //        {
            //            bHideColumn = (drpShowBreakup.SelectedValue == "Y") ? false : true;
            //            if (bHideColumn == true)
            //            {
            //                DetailsGrid.HeaderRow.Cells[iGridColCnt].Style.Add("display", "none"); // Hide Header        
            //                DetailsGrid.Rows[iGridRowCnt].Cells[iGridColCnt].Style.Add("display", "none");//Hide Cell
            //            }
            //            else
            //            {
            //                DetailsGrid.HeaderRow.Cells[iGridColCnt].Style.Add("display", ""); // Hide Header        
            //                DetailsGrid.Rows[iGridRowCnt].Cells[iGridColCnt].Style.Add("display", "");//Hide Cell
            //            }
            //        }
            //    }
            //    TextBox txtTotal = (TextBox)DetailsGrid.Rows[iGridRowCnt].FindControl("txtTotal");
            //    txtTotal.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
            //    if (bClearIncoAmount == true)
            //    {
            //        dQty = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iGridRowCnt].FindControl("txtQty") as TextBox).Text);
            //        dFobRate = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iGridRowCnt].FindControl("txtFOBRate") as TextBox).Text);
            //        dLineTotal = (dQty * dFobRate);
            //        txtTotal.Text = dLineTotal.ToString("0.00");
            //    }
            //    dTotalAmount = dTotalAmount + Func.Convert.dConvertToDouble(txtTotal.Text);

            //}
            //txtTotalAmt.Text = dTotalAmount.ToString("0.00");
            //txtGrandTotalAmt.Text = dTotalAmount.ToString("0.00");

        }

        //Fill Details From Grid
        private bool bFillDetailsFromGrid()
        {
            dtDetails = (DataTable)Session["AccDetails"];

            double dQty = 0;

            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                Label lblAccessoryNo = DetailsGrid.Rows[iRowCnt].FindControl("lblAccessoryNo") as Label;

                int iDtSelPartRow = 0;

                for (int iDtRowCnt = 0; iDtRowCnt < dtDetails.Rows.Count; iDtRowCnt++)
                {
                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iDtRowCnt]["AccID"]) == Func.Convert.iConvertToInt(lblAccessoryNo.Text))
                    {
                        iDtSelPartRow = iDtRowCnt;
                        break;
                    }
                }

                // Get Qty

                dQty = Func.Convert.dConvertToDouble((DetailsGrid.Rows[iRowCnt].FindControl("txtAccessoryQty") as TextBox).Text);
                dtDetails.Rows[iDtSelPartRow]["Qty"] = dQty;
            }
            bDetailsRecordExist = true;
            return true;
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
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    return;
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false, false) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
                {
                    if (bSaveRecord(true, false) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                else if (ObjImageButton.ID == "ToolbarButton4")//for Cancel
                {
                    if (bSaveRecord(false, true) == false) return;
                    PSelectionGrid.Style.Add("display", "");
                }
                FillSelectionGrid();
                GetDataAndDisplay();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // To Display Previous Record
        private void DisplayPreviousRecord()
        {
            try
            {
                clsVehicleReportingTime ObjVehReportingTm = new clsVehicleReportingTime();
                DataSet ds = new DataSet();
                //Please remove this Temporary comment

                iDealerId = Location.iDealerId;
                sDealerCode = Location.sDealerCode;
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                //iDealerId = 158;// Entry taken from TM_DealerHOBRData for 158
                //sDealerCode = "1S3045";

                ds = ObjVehReportingTm.GetVehicleReportingTime(iDocID, "New", iDealerId, iHOBranchDealerId);
                if (ds != null) // if no Data Exist
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ds.Tables[0].Rows[0]["veh_in_cancel"] = "N";
                            ds.Tables[0].Rows[0]["veh_in_confirm"] = "N";
                            //ds.Tables[1].Rows[0]["Status"] = "N";                        
                            sNew = "Y";
                            DisplayData(ds);
                        }
                    }
                }
                txtID.Text = "";
                txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);
                //txtDocDate.Text = Func.Common.sGetCurrentDate(Location.iCountryId, false);
                txtDocDate.Text = Func.Common.sGetCurrentDateTime(Location.iCountryId, true);

                lblSelectModel.Visible = true;
                if (txtUserType.Text == "6") lblSelectModel.Visible = false;

                ObjVehReportingTm = null;
                ds = null;
                ObjVehReportingTm = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //To Cancel The Record
        private void CancelRecord()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                if (txtID.Text != "")
                {
                    string ssql = "Update TM_Proforma Set Proforma_Cancel ='Y' where ID=" + txtID.Text;
                    objDB.BeginTranasaction();
                    objDB.ExecuteQuery(ssql);
                    objDB.CommitTransaction();
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8);</script>");
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                }
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }

        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select records.";
            bool bValidateRecord = true;
            //if (txtDocDate.Text == "")
            if (txtDocDate.Text == "")
            {
                sMessage = sMessage + "\\n Enter the document date.";
                bValidateRecord = false;
            }
            if (txtCRMTicketNo.Text == "" || Func.Convert.iConvertToInt(txtPreviousDocId.Text.ToString())== 0)
            {
                sMessage = sMessage + "\\n Please Select Call Ticket No.";
                bValidateRecord = false;
            }
            ////if (txtLastDateNegotiation.Text == "")
            ////{
            ////    sMessage = sMessage + "\\n Enter the Last Negotiation date.";
            ////    bValidateRecord = false;
            ////}
            //if (txtValidityDate.Text == "")
            //{
            //    sMessage = sMessage + "\\n Enter the Validity date.";
            //    bValidateRecord = false;
            //}
            //if (drpIncoTerms.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Shipment Terms.";
            //    bValidateRecord = false;
            //}
            //else
            //{
            //    if (drpIncoTerms.SelectedItem.Text == "FOB")
            //    {
            //        if (drpNominatedAgency.SelectedValue == "0")
            //        {
            //            sMessage = sMessage + "\\n Select the Nominated Agency.";
            //            bValidateRecord = false;
            //        }
            //    }
            //}
            //if (drpPaymentTerms.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Payment Terms.";
            //    bValidateRecord = false;
            //}
            //if (drpPaymentTerms.SelectedItem.Text == "Others")
            //{
            //    sMessage = sMessage + "\\n Change payment term From others.";
            //    bValidateRecord = false;
            //}

            //if (drpPortofDischarge.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Port of discharge.";
            //    bValidateRecord = false;
            //}
            //if (drpModeofShipment.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the Mode of shipment";
            //    bValidateRecord = false;
            //}
            //if (drpValidityDays.SelectedValue == "0")
            //{
            //    sMessage = sMessage + "\\n Select the validity days.";
            //    bValidateRecord = false;
            //}
            ////if (txtLastDateNegotiation.Text == "")
            ////{
            ////    sMessage = sMessage + "\\n Enter the Last Date Of Negotiation.";
            ////    bValidateRecord = false;
            ////}
            //if (drpMultiModalShipment.SelectedValue == "Y")
            //{
            //    if (drpMultiModalDestination.SelectedValue == "0")
            //    {
            //        sMessage = sMessage + "\\n Select the muli modal shipment destination.";
            //        bValidateRecord = false;
            //    }
            //    drpMultiModalDestination.Enabled = true;
            //}
            ////sujata 15012011
            ////if (drpVECVBank.SelectedValue == "0")
            ////{
            ////    sMessage = sMessage + "\\n Select the VECV Bank.";
            ////    bValidateRecord = false;
            ////}
            ////sujata 15012011
            //if (DetailsGrid.Rows.Count == 0)
            //{
            //    sMessage = sMessage + "\\n Please select at least one model.";
            //    bValidateRecord = false;
            //}
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            //Validation For Model Ramarks
            return bValidateRecord;
        }
        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;
                //Get Header InFormation   
                //ID,Inward_no,Inward_tr_no,time_in ,Dealer_ID,DlrBranchID,cr_date,veh_in_confirm ,veh_in_cancel ,Job_HDR_ID  ,WthApp, App_no, Chassis_ID

                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Inward_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Inward_tr_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("time_in", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DlrBranchID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("veh_in_confirm", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("veh_in_cancel", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Chassis_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Customer_name", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Job_HDR_ID", typeof(int)));
                //dtHdr.Columns.Add(new DataColumn("WthApp", typeof(bool)));
                dtHdr.Columns.Add(new DataColumn("App_no", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("CRM_HDR_ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("TrNo", typeof(string)));

                dr = dtHdr.NewRow();

                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Inward_tr_no"] = txtDocNo.Text;
                dr["time_in"] = txtDocDate.Text;
                dr["Inward_no"] = Func.Convert.sConvertToString(txtManDocNo.Text);
                dr["Dealer_ID"] = Location.iDealerId;
                dr["DlrBranchID"] = (iHOBranchDealerId == 0) ? Func.Convert.iConvertToInt(Session["HOBR_ID"]) : iHOBranchDealerId;
                dr["veh_in_confirm"] = "N";
                dr["veh_in_cancel"] = "N";
                dr["Chassis_ID"] = Func.Convert.iConvertToInt(txtChassisID.Text.ToString());
                dr["Customer_name"] = txtCustName.Text;
                dr["Job_HDR_ID"] = 0;
                dr["App_no"] = "";
                dr["CRM_HDR_ID"] = Func.Convert.iConvertToInt(txtPreviousDocId.Text.ToString());
                dr["TrNo"] = Func.Convert.sConvertToString(hdnTrNo.Value);

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        //ToSave Record
        private bool bSaveRecord(bool bSaveWithConfirm, bool bSaveWithCancel)
        {
            try
            {
                if (bValidateRecord() == false)
                {
                    return false;
                }

                DataTable dtHdr = new DataTable();
                clsVehicleReportingTime ObjVehReportingTm = new clsVehicleReportingTime();
                UpdateHdrValueFromControl(dtHdr);
                //Get Model Details     
                bDetailsRecordExist = false;
                bFillDetailsFromGrid();

                if (bSaveWithConfirm == true)
                {
                    dtHdr.Rows[0]["veh_in_confirm"] = "Y";
                }
                if (bSaveWithCancel == true)
                {
                    dtHdr.Rows[0]["veh_in_cancel"] = "Y";
                }

                if (ObjVehReportingTm.bSaveVehicleReporting(ref iDocID, Location.sDealerCode, dtHdr, dtDetails) == true)
                {
                    if (bSaveWithConfirm == true)
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(7,'" + Server.HtmlEncode("Vehicles Reporting Time") + "','" + Server.HtmlEncode(txtDocNo.Text) + "')", true);
                        return true;
                    }
                    else if (bSaveWithCancel == true)
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(8);</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(8,'" + Server.HtmlEncode("Vehicles Reporting Time") + "','" + Server.HtmlEncode(txtDocNo.Text) + "')", true);
                        return true;
                    }
                    else
                    {

                        txtID.Text = Func.Convert.sConvertToString(iDocID);
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4,'" + Server.HtmlEncode("Vehicles Reporting Time") + "','" + Server.HtmlEncode(txtDocNo.Text) + "')", true);
                        return true;
                    }
                }
                else
                {
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5,'" + Server.HtmlEncode("Vehicles Reporting Time") + "','" + Server.HtmlEncode(txtDocNo.Text) + "')", true);
                    return false;
                }

                ObjVehReportingTm = null;
                return true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.sGridPanelTitle = "Vehicle Reporting Time List";
                SearchGrid.AddToSearchCombo("Manual Inward No");
                SearchGrid.AddToSearchCombo("Tr No");
                SearchGrid.AddToSearchCombo("Time In");
                SearchGrid.AddToSearchCombo("Chassis No");
                SearchGrid.AddToSearchCombo("Ticket No");
                SearchGrid.AddToSearchCombo("Ticket Date");
                SearchGrid.AddToSearchCombo("Tr Status");
                //MDUser Change
                //SearchGrid.iDealerID = Location.iDealerId;
                //SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                if (txtUserType.Text == "6")
                {
                    SearchGrid.iDealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["DealerID"]);
                }
                else
                {
                    SearchGrid.iDealerID = Location.iDealerId;
                    SearchGrid.iBrHODealerID = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                }
                //MDUser Change
                SearchGrid.sSqlFor = "VehRepTimeIn";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            GetDataAndDisplay();
        }

        private void GetDataAndDisplay()
        {
            try
            {
                clsVehicleReportingTime ObjVehReportingTm = new clsVehicleReportingTime();
                DataSet ds = new DataSet();
                int iDocID = Func.Convert.iConvertToInt(txtID.Text);
                iHOBranchDealerId = Func.Convert.iConvertToInt(Session["HOBR_ID"]);//set it to actual ho branch id
                //iProformaID = 1;
                if (iDocID != 0)
                {
                    ds = ObjVehReportingTm.GetVehicleReportingTime(iDocID, "All", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);

                }
                else
                {
                    ds = ObjVehReportingTm.GetVehicleReportingTime(iDocID, "Max", Location.iDealerId, iHOBranchDealerId);
                    if (ds == null) // if no Data Exist
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);

                    DisplayData(ds);
                }
                ds = null;
                ObjVehReportingTm = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void ClearFormControl()
        {
            //Display Header                                
            txtDocNo.Text = "";
            txtDocDate.Text = "";
            txtChassisNo.Text = "";
            txtVehicleNo.Text = "";
            txtCustName.Text = "";
            txtManDocNo.Text = "";


            DetailsGrid.DataSource = null;
            DetailsGrid.DataBind();
        }

        // Display Data 
        private void DisplayData(DataSet ds)
        {
            try
            {
                bool bEnableControls = true;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inward_tr_no"]);
                txtDocDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["time_in"]);

                hdnTrNo.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["TrNo"]);

                txtPreviousDocId.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CRM_HDR_ID"]);
                txtCRMTicketNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_No"]);
                txtCRMTicketDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ticket_Date"]);

                txtManDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Inward_no"]);
                txtChassisID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_ID"]);
                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vehicle_No"]);
                txtCustName.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);

                txtCustEdit.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CustEdit"]);
                if (txtCustEdit.Text == "Y")
                {
                    //txtCustName.Attributes.Add("readOnly", "false");// = (txtCustEdit.Text == "Y") ? false : true; 
                    //txtCustName.Attributes.Add("readOnly", "");
                    txtCustName.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                }
                else
                {
                    txtCustName.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                }


                // Commented By Shyamal as on 16/02/2011
                //Display Details
                hdnConfirm.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_confirm"]);
                hdnCancle.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["veh_in_cancel"]);

                Session["AccDetails"] = null;
                dtDetails = ds.Tables[1];
                Session["AccDetails"] = dtDetails;
                BindDataToGrid();

                ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, true); //print option enable for save & confirm
                ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, true);

                lblSelectModel.Visible = (txtID.Text == "" || txtID.Text == "0") ? true : false;
                
                if (txtUserType.Text == "6") lblSelectModel.Visible = false;

                // If Record is Confirm or cancel then it is not editable            
                if (hdnConfirm.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);                    
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                }
                if (hdnCancle.Value == "Y")
                {
                    bEnableControls = false;
                    ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
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

        // To enable or disable fields
        private void MakeEnableDisableControls(bool bEnable)
        {

        }
        #region Select Chassis Link
        protected void lblSelectModel_Click(object sender, EventArgs e)
        {
            BindData();
            mpeSelectChassis.Show();
        }
        private string sSelText, sSelType;
        private int iJobtype, iDealerID, iHOBrID;

        private void BindData()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSrchgrid;
                sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                sSelText = txtSearch.Text.ToString();
                iJobtype = 0;
                iDealerID = Func.Convert.iConvertToInt(Location.iDealerId);
                iHOBrID = Func.Convert.iConvertToInt(Session["HOBR_ID"].ToString());

                dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_ChassisSelection", sSelType, sSelText, iJobtype, iDealerID, iHOBrID);
                ViewState["Chassis"] = dtSrchgrid;
                Session["ChassisDetails"] = dtSrchgrid;

                if (dtSrchgrid == null)
                {
                    return;
                }

                DataView dvDetails = new DataView();
                dvDetails = dtSrchgrid.DefaultView;
                ChassisGrid.DataSource = dtSrchgrid;
                ChassisGrid.DataBind();

                if (ChassisGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < ChassisGrid.Rows.Count; iRowCnt++)
                {

                    //Show Vehicle In No 
                    ChassisGrid.HeaderRow.Cells[5].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[5].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Vehicle In Date
                    ChassisGrid.HeaderRow.Cells[6].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[6].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Kms
                    ChassisGrid.HeaderRow.Cells[18].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[18].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell

                    //Show Last Hrs
                    ChassisGrid.HeaderRow.Cells[19].Style.Add("display", iJobtype == 0 ? "none" : ""); // show Header        
                    ChassisGrid.Rows[iRowCnt].Cells[19].Style.Add("display", iJobtype == 0 ? "none" : "");//show Cell
                }
                hdnTrNo.Value = Location.sDealerCode + "/I/" + Func.Convert.sConvertToString(Session["UserID"].ToString()) + "/" + DateTime.Today.Date.ToString("dd/MM/yyyy") + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss\\:ffffff").Trim());

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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" && btnSearch.Text == "Search")
                btnSearch.Text = "ClearSearch";
            else if (txtSearch.Text != "" && btnSearch.Text == "ClearSearch")
            {
                txtSearch.Text = "";
                btnSearch.Text = "Search";
            }
            BindData();
            mpeSelectChassis.Show();
        }

        protected void ChassisGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ChassisGrid.PageIndex = e.NewPageIndex;
            BindData();
            mpeSelectChassis.Show();
        }

        // This Code Not Required, Userfull code write Using Javascript in SetChassisDetails() function
        protected void btnOkChassis_Click(object sender, EventArgs e)
        {
            DataRow[] dtSrchgrid;
            DataTable dtSearch;
            dtSearch = (DataTable)Session["ChassisDetails"];
            //dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString() + " and VehInID=" + txtPreviousDocId.Text.ToString());
            if (dtSearch != null)
            {
                dtSrchgrid = dtSearch.Select("Chassis_ID = " + txtChassisID.Text.ToString(), "", DataViewRowState.CurrentRows);

                if (dtSrchgrid.Length != 0)
                {
                    for (int i = 0; i < dtSrchgrid.Length; i++)
                    {
                        txtChassisID.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_ID"]);
                        txtChassisNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Chassis_no"]);
                        txtVehicleNo.Text = Func.Convert.sConvertToString(dtSrchgrid[i]["Vehicle_No"]);
                    }
                }
            }
        }
        #endregion
    }
}