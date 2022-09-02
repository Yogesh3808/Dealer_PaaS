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
using System.IO;
using System.Data.OleDb;

namespace MANART.Forms.Spares
{
    public partial class frmPartClaimProcessing : System.Web.UI.Page
    {
        int iId = 0;
        string sConfirm = "";
        string sPClmType = "";
        string sDealerCode = "";
        string sDealerID = "";
        string sFin_No = "";
        DataTable dtHeader = new DataTable();
        DataTable dtDetails = new DataTable();
        DataTable dtPartClaimErrorCode = new DataTable();
        private DataTable dtTaxDetails = new DataTable();
        private DataTable dtGrpTaxDetails = new DataTable();
        private DataTable Acc_dtTaxDetails = new DataTable();
        private DataTable Acc_dtGrpTaxDetails = new DataTable();
        private bool bDetailsRecordExist = false;
        int iUserRoleId = 0;
        string AppAndRecjStatus = "N";
        //string sReqStatus = "";
        private DataTable dtFileAttach = new DataTable();
        private DataTable dtDetForReason = new DataTable();
        protected string baseUrl;
        string[] ALPart = new string[4];
        clsPartClaim objPartClaim = new clsPartClaim();
        public enum PartClaim
        {
            EnvNo,
            EnvDate,
            GrrNo,
            GrrDate
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iId = Func.Convert.iConvertToInt(Request.QueryString["ID"]);
                sConfirm = Func.Convert.sConvertToString(Request.QueryString["Confirm"]);
                sPClmType = Func.Convert.sConvertToString(Request.QueryString["PClmType"]);
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                hdnUserRoleId.Value = iUserRoleId.ToString();
                sDealerCode = Func.Convert.sConvertToString(Request.QueryString["sDealerCode"]);
                txtDealerCode.Text = sDealerCode;
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                sDealerID = Func.Convert.sConvertToString(Request.QueryString["sDealerID"]);
                sFin_No = Func.Convert.sConvertToString(Request.QueryString["sFinNo"]);

                if (!IsPostBack)
                {
                    FillCombo();
                    FillHdrAndDtls(iId, "All");//, sReqStatus
                    EnableDisable(sConfirm, "");
                    lblMessage(sConfirm);
                }
                ExpirePageCache();
                SetDocument();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void FillCombo()
        {
            // Func.Common.BindDataToCheckBoxList(chkError, clsCommon.ComboQueryType.PartClaimErrorCode, 0, "");
        }
        private void SetDocument()
        {
            iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
            if (iUserRoleId == 2 )// if user is RSM or PartClaim Manager
            {
                txtRSMRemark.ReadOnly = false;
                txtCSMRemark.ReadOnly = false;
                //txtASMRemark.ReadOnly = true;
                //trShow3PLRemark.Style.Add("display", "");
                //txtHeadRemark.ReadOnly = true;
            }
            else if (iUserRoleId == 3 || iUserRoleId == 12)// if user is Asm or 3PL
            {
                txtCSMRemark.ReadOnly = true;
                txtASMRemark.ReadOnly = true;
                //txtRSMRemark.ReadOnly = true;
                //txtHeadRemark.ReadOnly = true;
                //trShow3PLRemark.Style.Add("display", "none");
            }
            //else if (iUserRoleId == 2)// if user is Rsm
            //{
            //    txtCSMRemark.ReadOnly = true;
            //    txtASMRemark.ReadOnly = true;
            //    //txtRSMRemark.ReadOnly = false;
            //    //txtHeadRemark.ReadOnly = true;
            //}
            //else if (iUserRoleId == 1)// if user is Head
            //{
            //    txtCSMRemark.ReadOnly = true;
            //    txtASMRemark.ReadOnly = true;
            //    //txtRSMRemark.ReadOnly = true;
            //    //txtHeadRemark.ReadOnly = false;
            //}
            //else
            //{
            //    txtCSMRemark.ReadOnly = true;
            //    txtASMRemark.ReadOnly = true;
            //    //txtRSMRemark.ReadOnly = true;
            //    //txtHeadRemark.ReadOnly = true;
            //}        
        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        private void lblMessage(string sStatus)
        {
            try
            {
                if (sStatus == "N")
                {
                    lblTitle.Text = "Part Claim Processing";
                }
                else
                    if (sStatus == "Y")
                    {
                        lblTitle.Text = "Part Claim Approved";
                    }
                    else
                        if (sStatus == "R")
                        {
                            lblTitle.Text = "Part Claim Rejected";
                        }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clsPartClaim objPartClaim = new clsPartClaim();

                string ApproveStatus = "N";
                if (hdnUserRoleId.Value == "2")// RSM or Parts Support Desk
                {
                    hdnPartClaimMgr.Value = "Y";
                    ApproveStatus = "Y";
                    AppAndRecjStatus = "Y";
                }
                //if (hdnUserRoleId.Value == "11" || hdnUserRoleId.Value == "1") // CSM Or HEAD
                //{
                //    hdnPartClaimMgr.Value = "Y";
                //    ApproveStatus = 'Y';
                //    AppAndRecjStatus = "Y";
                //}
                //else if (hdnUserRoleId.Value == "12")
                //{
                //    ApproveStatus = 'N';
                //}

                //if (hdnUserRoleId.Value == "12" && hdnPartClaimMgr.Value == "Y")
                //{
                //    hdnPartClaimMgr.Value = "P";
                //    hdn3PL.Value = "Y";
                //    hdnProcessContinue.Value = "N";
                //}
                bDetailsRecordExist = false;
                PartClaimSave(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                }
                else
                {
                    //if (CheckForApprove(true) == false)
                    //{
                    //    string vMsg = "Please Approve at least one Part..";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);
                    //    objPartClaim = null;
                    //    return;
                    //}
                    //Sujata 27012011 
                    if (objPartClaim.bSavePartClaim(dtHeader, dtDetails, dtPartClaimErrorCode, ApproveStatus, "Y", iUserRoleId, Acc_dtGrpTaxDetails, Acc_dtTaxDetails) == true)
                    //Sujata 27012011
                    {
                        DivBtn.Visible = false;
                        txtRejectionReason.Text = "";
                        //Sujata 27012011
                        //txtRejectionReason.Enabled = false;    
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(17,'" + Server.HtmlEncode("Claim") + "','" + Server.HtmlEncode(txtPartClaimNo.Text) + "');</script>");
                        //string vMsg = "Claim Processing Details Approved..";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);
                        btnApprove.Enabled = false;
                        btnSave.Enabled = false;
                        btnReject.Enabled = false;
                        objPartClaim = null;
                        FillHdrAndDtls(Func.Convert.iConvertToInt(txtID.Text), "All");//, sReqStatus
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage(2,'" + txtPartClaimNo.Text + "');", true);
                        EnableDisable(sConfirm, "Y");
                        lblMessage(sConfirm);
                        return;
                    }
                }
                //}
                //else
                //{
                //    if (hdnUserRoleId.Value == "3")
                //    {
                //        hdnPartClaimMgr.Value = "P";
                //        hdn3PL.Value = "Y";
                //    }
                //    else
                //    {
                //        hdnPartClaimMgr.Value = "Y";
                //        hdn3PL.Value = "P";
                //    }

                //    //Sujata 27012011
                //    PartClaimSave();
                //    //Sujata 27012011 
                //    if (objPartClaim.bSavePartClaim(dtHeader, dtDetails, Convert.ToChar(sConfirm), 'N', iUserRoleId) == true)
                //    //Sujata 27012011
                //    {
                //        if (hdnUserRoleId.Value != "3")
                //        {
                //            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Part Claim Details Processed..');</script>");
                //            string vMsg = "Part Claim Details Processed..";
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);
                //        }
                //        else
                //        {
                //            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Part Claim Details Back to Part Claim Manager..');</script>");
                //            string vMsg = "Part Claim Details Back to Part Claim Manager..";
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);
                //        }

                //        objPartClaim = null;
                //        //Sujata 28012011
                //        FillHdrAndDtls(Func.Convert.iConvertToInt(txtID.Text));//, sReqStatus
                //        //EnableDisable(sConfirm);
                //        lblMessage(sConfirm);
                //        //Sujata 28012011
                //        return;
                //    }
                //}

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                clsPartClaim objPartClaim = new clsPartClaim();

                string ApproveStatus = "N";
                if (hdnUserRoleId.Value == "2")//RSM or Parts Support Desk
                {
                    hdnPartClaimMgr.Value = "Y";
                    ApproveStatus = "R";
                    AppAndRecjStatus = "R";
                }
                //if (hdnUserRoleId.Value == "11" || hdnUserRoleId.Value == "1")
                //    if (hdnUserRoleId.Value == "11" )
                //{
                //    hdnPartClaimMgr.Value = "Y";
                //    ApproveStatus = 'R';
                //    AppAndRecjStatus = "R";
                //}
                //else if (hdnUserRoleId.Value == "12")
                //{
                //    ApproveStatus = 'N';
                //}

                //if (hdnUserRoleId.Value == "12" && hdnPartClaimMgr.Value == "Y")
                //{
                //    hdnPartClaimMgr.Value = "P";
                //    hdn3PL.Value = "Y";
                //    hdnProcessContinue.Value = "N";
                //}

                if (CheckForCancle(false) == false)
                {
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('All Part Should be Rejected.');</script>");
                    string vMsg = "Claim Processing Can not Rejected..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);
                    objPartClaim = null;
                    return;
                }
                bDetailsRecordExist = false;
                PartClaimSave(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                }
                else
                {
                    if (objPartClaim.bSavePartClaim(dtHeader, dtDetails, dtPartClaimErrorCode, ApproveStatus, "R", iUserRoleId, Acc_dtGrpTaxDetails, Acc_dtTaxDetails) == true)
                    {
                        btnApprove.Enabled = false;
                        btnSave.Enabled = false;
                        btnReject.Enabled = false;
                        objPartClaim = null;
                        FillHdrAndDtls(Func.Convert.iConvertToInt(txtID.Text), "All");//, sReqStatus
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage(3,'" + txtPartClaimNo.Text + "');", true);
                        EnableDisable(sConfirm, "");
                        lblMessage(sConfirm);
                        
                        return;
                    }
                }

                //if (objPartClaim.bSavePartClaim(PartClaimSave(),"") == true)
                //{
                //    DivBtn.Visible = false;
                //    txtRejectionReason.Enabled = false;
                //    objPartClaim = null;
                //    return;
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void PartClaimSave(bool bDisplayMsg)
        {
            try
            {
                DataRow dr;
                DataRow dtr;
                CheckBox chkMRN = new CheckBox();

                string sStatus = "";
                string DtlId = "";
                dtHeader.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHeader.Columns.Add(new DataColumn("Claim_Type", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Dealer_Code", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Approved_Date", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Approved_By", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Head_Remark", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("RSM_Remark", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("CSM_Remark", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("ASM_Remark", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Approval_No", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Is_PartClaimMgr", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Is_3PL", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Is_ProcessContinue", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Is_Show3PLRemark", typeof(string)));
                dtHeader.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dtHeader.Columns.Add(new DataColumn("Fin_no", typeof(string)));
                

                dr = dtHeader.NewRow();
                dr["ID"] = Func.Convert.iConvertToInt(txtID.Text);
                dr["Claim_Type"] = txtClaimType.Text;
                dr["Dealer_Code"] = sDealerCode;
                dr["Approved_Date"] = txtApprovedDate.Text;
                dr["Approved_By"] = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                dr["Remarks"] = txtRemarks.Text;
                dr["Head_Remark"] = "";
                dr["RSM_Remark"] = txtRSMRemark.Text.Trim();
                dr["CSM_Remark"] = txtCSMRemark.Text;
                dr["ASM_Remark"] = txtASMRemark.Text;
                dr["Approval_No"] = txtApprovedNo.Text;

                dr["Is_PartClaimMgr"] = hdnPartClaimMgr.Value;
                dr["Is_3PL"] = hdn3PL.Value;
                dr["Is_ProcessContinue"] = hdnProcessContinue.Value;
                dr["Is_Show3PLRemark"] = (ChkShow3PLRemark.Checked == true) ? "Y" : "N";
                dr["Dealer_ID"] = Func.Convert.iConvertToInt(sDealerID);
                dr["Fin_no"] = sFin_No.Trim();

                dtHeader.Rows.Add(dr);
                dtHeader.AcceptChanges();


                dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("Approved_qty", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("AccTotal", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Claim Qty", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Rejection_Res", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Approved_Status", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Shortage_Claim_Type_Id", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("ShortageClaimTypeCredit_Id", typeof(int)));


                for (int iRowCnt = 0; iRowCnt < GridPartClaimDetail.Rows.Count; iRowCnt++)
                {
                    sStatus = "";
                    dtr = dtDetails.NewRow();
                    dtr["ID"] = 0;
                    DtlId = (GridPartClaimDetail.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                    string[] ID = DtlId.Split(' ');
                    dtr["ID"] = Func.Convert.iConvertToInt(ID[0]);
                    dtr["Claim Qty"] = Func.Convert.dConvertToDouble((GridPartClaimDetail.Rows[iRowCnt].FindControl("lblClaimQty") as Label).Text);
                    dtr["Approved_qty"] = Func.Convert.dConvertToDouble((GridPartClaimDetail.Rows[iRowCnt].FindControl("txtApprovedQty") as TextBox).Text);
                    dtr["AccTotal"] = Func.Convert.dConvertToDouble((GridPartClaimDetail.Rows[iRowCnt].FindControl("txtAccTotal") as TextBox).Text);
                    dtr["Rejection_Res"] = (GridPartClaimDetail.Rows[iRowCnt].FindControl("txtDescription") as TextBox).Text;

                    if (Func.Convert.dConvertToDouble(dtr["Approved_qty"]) <= Func.Convert.dConvertToDouble(dtr["Claim Qty"]) && Func.Convert.dConvertToDouble(dtr["Approved_qty"]) > 0)
                        dtr["Approved_Status"] = "Y";
                    else
                        dtr["Approved_Status"] = "N";

                    if (sPClmType == "Manufacturing Defect" || sPClmType == "Shortage")
                    {
                        dtr["Shortage_Claim_Type_Id"] = 1;
                        dtr["ShortageClaimTypeCredit_Id"] = 0;
                    }
                    else
                    {
                        dtr["Shortage_Claim_Type_Id"] = 1;
                        dtr["ShortageClaimTypeCredit_Id"] = 0;

                    }
                    dtDetails.Rows.Add(dtr);
                    dtDetails.AcceptChanges();

                    //Vikram Kite on 08/09/2016
                    //GetPartClaimErrorCode();
                }

                int iCntError = 0;
                string sQtyMsg = "";
                int iPartID = 0, iClaimQty = 0, iApprQty = 0;
                for (int iCnt = 0; iCnt < dtDetails.Rows.Count; iCnt++)
                {
                    iPartID = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["ID"]);
                    iClaimQty = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Claim Qty"]);
                    iApprQty = Func.Convert.iConvertToInt(dtDetails.Rows[iCnt]["Approved_qty"]);

                    if (iPartID != 0 && iClaimQty > iApprQty && Func.Convert.sConvertToString(dtDetails.Rows[iCnt]["Rejection_Res"]) == "")
                    {
                        iCntError = iCntError + 1;
                        sQtyMsg = "Please Enter Reason at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('" + sQtyMsg + "')", true);
                        break;
                    }
                    if (iPartID != 0 && iClaimQty < iApprQty)
                    {
                        iCntError = iCntError + 1;
                        sQtyMsg = "Please Enter Less Quantity from Claim Quantity at Row No " + (iCnt + 1) + " in Page No " + (Func.Convert.iConvertToInt(iCnt / 10) + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('" + sQtyMsg + "')", true);
                        break;
                    }
                }
                if (hdnPartClaimMgr.Value == "Y" && txtRSMRemark.Text == "" && hdn3PL.Value == "N" && AppAndRecjStatus != "N")
                {
                    iCntError = iCntError + 1;
                    sQtyMsg = "Please Enter Parts Support Desk Remark.....";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('" + sQtyMsg + "')", true);
                }
                // GroupTax Details
                //For Tax Details
                Acc_dtGrpTaxDetails = (DataTable)(ViewState["AccGrpTaxDetails"]);
                Acc_dtTaxDetails = (DataTable)(ViewState["AccTaxDetails"]);
                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    //Group Code
                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["group_code"] = txtGRPID.Text.Trim();

                    //Group Name
                    TextBox txtMGrName = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtMGrName");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Gr_Name"] = txtMGrName.Text;

                    //Get Net Amount
                    TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_net_inv_amt"] = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    //Get Discount Perc
                    TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["discount_per"] = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);

                    //Get Discount Amount
                    TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtGrDiscountAmt.Text);

                    // Get Tax
                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax_Code"] = Func.Convert.iConvertToInt(drpTax.SelectedValue);

                    //Get Tax Percentage                
                    DropDownList drpTaxPer = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["TAX_Percentage"] = Func.Convert.dConvertToDouble(drpTaxPer.SelectedItem);

                    //Get Tax Amount
                    TextBox txtGrTaxAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax_amt"] = Func.Convert.dConvertToDouble(txtGrTaxAmt.Text);

                    // Get Tax1
                    DropDownList drpTax1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"] = Func.Convert.iConvertToInt(drpTax1.SelectedValue);

                    //Get Tax1 Percentage                
                    DropDownList drpTaxPer1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax1_Per"] = Func.Convert.dConvertToDouble(drpTaxPer1.SelectedItem);

                    //Get Tax1 Amount
                    TextBox txtGrTax1Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax1_amt"] = Func.Convert.dConvertToDouble(txtGrTax1Amt.Text);

                    // Get Tax2
                    DropDownList drpTax2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"] = Func.Convert.iConvertToInt(drpTax2.SelectedValue);

                    //Get Tax2 Percentage                
                    DropDownList drpTaxPer2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Tax2_Per"] = Func.Convert.dConvertToDouble(drpTaxPer2.SelectedItem);

                    //Get Tax2 Amount
                    TextBox txtGrTax2Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_tax2_amt"] = Func.Convert.dConvertToDouble(txtGrTax2Amt.Text);

                    // Get Total
                    TextBox txtTaxTot = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");
                    Acc_dtGrpTaxDetails.Rows[iRowCnt]["Acc_Total"] = Func.Convert.dConvertToDouble(txtTaxTot.Text);
                }

                for (int iRowCnt = 0; iRowCnt < Acc_GrdDocTaxDet.Rows.Count; iRowCnt++)
                {
                    //Doc ID
                    TextBox txtDocID = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocID");
                    Acc_dtTaxDetails.Rows[iRowCnt]["ID"] = txtDocID.Text;

                    //Get Net Amount
                    TextBox txtDocTotal = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocTotal");
                    Acc_dtTaxDetails.Rows[iRowCnt]["Acc_net_tr_amt"] = Func.Convert.dConvertToDouble(txtDocTotal.Text);
                    //Get Net Amount
                    //TextBox txtDocRevTotal = (TextBox)GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocRevTotal");
                    //dtInvTaxDetails.Rows[iRowCnt]["net_rev_amt"] = Func.Convert.dConvertToDouble(txtDocRevTotal.Text);

                    //Get Discount amt
                    TextBox txtDocDisc = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtDocDisc");
                    Acc_dtTaxDetails.Rows[iRowCnt]["discount_amt"] = Func.Convert.dConvertToDouble(txtDocDisc.Text);

                    //Get Amt Before Tax (with Discount)
                    TextBox txtBeforeTax = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtBeforeTax");
                    Acc_dtTaxDetails.Rows[iRowCnt]["before_tax_amt"] = Func.Convert.dConvertToDouble(txtBeforeTax.Text);

                    // Get Tax 
                    TextBox txtMSTAmt = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtMSTAmt");
                    Acc_dtTaxDetails.Rows[iRowCnt]["Acc_mst_amt"] = Func.Convert.dConvertToDouble(txtMSTAmt.Text);

                    //Get Tax         
                    TextBox txtCSTAmt = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtCSTAmt");
                    Acc_dtTaxDetails.Rows[iRowCnt]["Acc_cst_amt"] = Func.Convert.dConvertToDouble(txtCSTAmt.Text);

                    //Get Tax1 Amount
                    TextBox txtTax1 = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax1");
                    Acc_dtTaxDetails.Rows[iRowCnt]["Acc_surcharge_amt"] = Func.Convert.dConvertToDouble(txtTax1.Text);

                    // Get Tax2 Amount
                    TextBox txtTax2 = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtTax2");
                    Acc_dtTaxDetails.Rows[iRowCnt]["tot_amt"] = Func.Convert.dConvertToDouble(txtTax2.Text);

                    //Get PF Per                 
                    TextBox txtPFPer = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFPer");
                    Acc_dtTaxDetails.Rows[iRowCnt]["pf_per"] = Func.Convert.dConvertToDouble(txtPFPer.Text);

                    //Get PF Amount
                    TextBox txtPFAmt = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtPFAmt");
                    Acc_dtTaxDetails.Rows[iRowCnt]["pf_amt"] = Func.Convert.dConvertToDouble(txtPFAmt.Text);

                    // Get Other Per
                    TextBox txtOtherPer = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherPer");
                    Acc_dtTaxDetails.Rows[iRowCnt]["other_per"] = Func.Convert.dConvertToDouble(txtOtherPer.Text);

                    //Get Other Amount
                    TextBox txtOtherAmt = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtOtherAmt");
                    Acc_dtTaxDetails.Rows[iRowCnt]["other_money"] = Func.Convert.dConvertToDouble(txtOtherAmt.Text);

                    //Get grand Total Amount
                    TextBox txtGrandTot = (TextBox)Acc_GrdDocTaxDet.Rows[iRowCnt].FindControl("txtGrandTot");
                    Acc_dtTaxDetails.Rows[iRowCnt]["Acc_Claim_Amt"] = Func.Convert.dConvertToDouble(txtGrandTot.Text);
                }
                //END

                if (iCntError > 0)
                    bDetailsRecordExist = false;
                else
                    bDetailsRecordExist = true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void GetPartClaimErrorCode()
        {
            int iRecord = 0;
            try
            {
                if (dtPartClaimErrorCode.Columns.Count == 0)
                {
                    dtPartClaimErrorCode.Columns.Add(new DataColumn("PartClaimErrHdr_ID", typeof(int)));
                }

                for (int iRowCnt = 0; iRowCnt < chkError.Items.Count; iRowCnt++)
                {
                    if (chkError.Items[iRowCnt].Selected == true)
                    {

                        DataRow drPartClaimErrorCode = dtPartClaimErrorCode.NewRow();
                        drPartClaimErrorCode["PartClaimErrHdr_ID"] = Func.Convert.iConvertToInt(chkError.Items[iRowCnt].Value);
                        dtPartClaimErrorCode.Rows.Add(drPartClaimErrorCode);

                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }


        private void FillHdrAndDtls(int ID, string DocStatus) //,string sStatus
        {
            try
            {
                sConfirm = Func.Convert.sConvertToString(Request.QueryString["Confirm"]);

                DataSet ds = new DataSet();
                DataTable dtDetails = null;
                clsPartClaim objPartClaim = new clsPartClaim();
                
                //if (iUserRoleId == 3 && sConfirm == "N")
                //    ds = null;
                //else
                ds = objPartClaim.GetPartClaim(ID.ToString(), DocStatus, "N", 0, iUserRoleId, "", "");

                if (ds != null)
                {
                    FillPartClaimHeader(ds.Tables[0]);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dtDetForReason = ds.Tables[1];
                        dtDetails = ds.Tables[1];
                        ViewState["dtDetails"] = dtDetails;
                        //dtDetails = (DataTable)(ViewState["dtDetails"]);
                      
                        //SetControlPropertyToPartClaimGridForReason(true);
                        
                        //objPartClaim = null;
                    }
                    else
                    {
                        GridPartClaimDetail.DataSource = null;
                        GridPartClaimDetail.DataBind();
                        ViewState["dtDetails"] = null;

                    }

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        ViewState["GrpTaxDetails"] = null;
                        ViewState["TaxDetails"] = null;
                        ViewState["AccGrpTaxDetails"] = null;
                        ViewState["AccTaxDetails"] = null;

                        dtGrpTaxDetails = ds.Tables[3];
                        ViewState["GrpTaxDetails"] = dtGrpTaxDetails;
                       
                        dtTaxDetails = ds.Tables[4];
                        ViewState["TaxDetails"] = dtTaxDetails;

                        Acc_dtGrpTaxDetails = ds.Tables[5];
                        ViewState["AccGrpTaxDetails"] = Acc_dtGrpTaxDetails;

                        Acc_dtTaxDetails = ds.Tables[6];
                        ViewState["AccTaxDetails"] = Acc_dtTaxDetails;
                    }
                    BindDataToGrid();

                    // Display Attach file  Details  
                    if (PFileAttchDetails.Visible == true)
                    {
                        dtFileAttach = ds.Tables[2];
                        // lblFileAttachRecCnt.Text = Func.Common.sRowCntOfTable(dtFileAttach);
                        ShowAttachedFiles();
                    }
                    //dtPartClaimErrorCode = ds.Tables[3];
                }
                else
                {
                    GridPartClaimDetail.DataSource = null;
                    GridPartClaimDetail.DataBind();

                    GrdPartGroup.DataSource = null;
                    GrdPartGroup.DataBind();

                    GrdDocTaxDet.DataSource = null;
                    GrdDocTaxDet.DataBind();

                    Acc_GrdPartGroup.DataSource = null;
                    Acc_GrdPartGroup.DataBind();

                    Acc_GrdDocTaxDet.DataSource = null;
                    Acc_GrdDocTaxDet.DataBind();

                    // lblMessage.Text = "MRN Not Exist!";
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void FillPartClaimHeader(DataTable dtHeader)
        {
            try
            {
                string sInClaim = "";
                if (dtHeader.Rows.Count == 0)
                {
                    return;
                }
                txtID.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ID"]);
                txtUserType.Text = Func.Convert.sConvertToString(Session["UserType"]);
                txtPartClaimNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["part_claim_no"]);
                txtClaimDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["claim_date"]);
                txtClaimAmount.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtHeader.Rows[0]["claim_amt"]));
                txtLrNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Lr_No"]);
                txtLRDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Lr_date"]);
                txtApprovedNo.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_No"]);
                txtApprovedDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_Date"]);
                txtApprovedBy.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approved_By"]);
                txtRejectionReason.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Rejection_Reason"]);
                sInClaim = Func.Convert.sConvertToString(dtHeader.Rows[0]["surveyor_rpt_no"]);
                txtRemarks.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Remarks"]);
                txtBoxCondition.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Box_Description"]);
                //txtHeadRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Head_Remark"]);
                txtRSMRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["RSM_Remark"]);
                txtCSMRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["CSM_Remark"]);
                txtASMRemark.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["ASM_Remark"]);
                txtClaimType.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Claim Type"]);

                hdnPartClaimMgr.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["Is_PartClaimMgr"]);
                hdn3PL.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["Is_3PL"]);
                hdnProcessContinue.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["Is_ProcessContinue"]);
                ChkShow3PLRemark.Checked = (Func.Convert.sConvertToString(dtHeader.Rows[0]["Is_Show3PLRemark"]) == "Y") ? true : false;
                hdnIsDocGST.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["DocGST"]);
                hdnCustTaxTag.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["CustTaxTag"]);
                hdnDealerID.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["DealerID"]);
                txtAccClaimAmt.Text = Func.Convert.sConvertToString(Func.Convert.dConvertToDouble(dtHeader.Rows[0]["Acc_Claim_Amt"]));
                hdnIsRoundOFF.Value = Func.Convert.sConvertToString(dtHeader.Rows[0]["IsRoundOFF"]);

                if (sInClaim != "")
                {
                    tblIns.Visible = true;
                    txtSurveyNo.Text = sInClaim;
                    txtSurveyDate.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Survey_date"]);
                    //txtSurveyBy.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Surv_name"]);
                    txtIncComp.Text = Func.Convert.sConvertToString(dtHeader.Rows[0]["Insurance_comp"]);

                }
                else
                    tblIns.Visible = false;
                if (sPClmType == "Damage" && sInClaim != "")
                {
                    trDamageType.Style.Add("display", "");
                    drpDamageType.SelectedValue = Func.Convert.sConvertToString(dtHeader.Rows[0]["DamageType"]);
                }
                else
                    trDamageType.Style.Add("display", "none");

                sConfirm = Func.Convert.sConvertToString(dtHeader.Rows[0]["Approval_Status"]);
                EnableDisable(sConfirm, hdn3PL.Value);
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
                dtDetails = (DataTable)(ViewState["dtDetails"]);
                GridPartClaimDetail.DataSource = dtDetails;
                GridPartClaimDetail.DataBind();

                GrdPartGroup.DataSource = dtGrpTaxDetails;
                GrdPartGroup.DataBind();

                GrdDocTaxDet.DataSource = dtTaxDetails;
                GrdDocTaxDet.DataBind();

                Acc_GrdPartGroup.DataSource = Acc_dtGrpTaxDetails;
                Acc_GrdPartGroup.DataBind();

                Acc_GrdDocTaxDet.DataSource = Acc_dtTaxDetails;
                Acc_GrdDocTaxDet.DataBind();

                SetGridControlProperty();
                SetControlPropertyToPartClaimGridForReason(true);
                SetGridControlPropertyTax();
                SetGridControlPropertyTaxCalculation();
                Acc_SetGridControlPropertyTax();
                Acc_SetGridControlPropertyTaxCalculation();
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
                string sGroupCode = "";
                for (int iRowCnt = 0; iRowCnt < GridPartClaimDetail.Rows.Count; iRowCnt++)
                {
                    // HO Apro Qty
                    GridPartClaimDetail.HeaderRow.Cells[21].Style.Add("display", "none"); // Hide Header        
                    GridPartClaimDetail.Rows[iRowCnt].Cells[21].Style.Add("display", "none");//Hide Cell 
                    // HO Rej Reason
                    GridPartClaimDetail.HeaderRow.Cells[22].Style.Add("display", "none"); // Hide Header        
                    GridPartClaimDetail.Rows[iRowCnt].Cells[22].Style.Add("display", "none");//Hide Cell 
                    if (sPClmType != "Wrong Supply")
                    {
                        if (sPClmType == "Shortage")
                        {
                            // Retain
                            GridPartClaimDetail.HeaderRow.Cells[5].Style.Add("display", "none"); // Hide Header        
                            GridPartClaimDetail.Rows[iRowCnt].Cells[5].Style.Add("display", "none");//Hide Cell 
                        }
                        // Wrg Part No
                        GridPartClaimDetail.HeaderRow.Cells[3].Style.Add("display", "none"); // Hide Header        
                        GridPartClaimDetail.Rows[iRowCnt].Cells[3].Style.Add("display", "none");//Hide Cell 
                        // Wrg Part Name
                        GridPartClaimDetail.HeaderRow.Cells[4].Style.Add("display", "none"); // Hide Header        
                        GridPartClaimDetail.Rows[iRowCnt].Cells[4].Style.Add("display", "none");//Hide Cell 
                    }
                    // Retain Dated 13122017
                    GridPartClaimDetail.HeaderRow.Cells[5].Style.Add("display", "none"); // Hide Header        
                    GridPartClaimDetail.Rows[iRowCnt].Cells[5].Style.Add("display", "none");//Hide Cell 


                    //Main Tax
                    sGroupCode = "01";
                    DropDownList drpPartTax = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("drpPartTax");
                    if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                        Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpPartTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    drpPartTax.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);


                    DropDownList drpPartTaxPer = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("drpPartTaxPer");
                    if (Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["TaxTag"]) == "I")
                        Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1" + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpPartTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((sGroupCode.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    drpPartTaxPer.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartTaxID"]);

                    TextBox txtPartTaxPer = (TextBox)GridPartClaimDetail.Rows[iRowCnt].FindControl("txtPartTaxPer");
                    txtPartTaxPer.Text = Func.Convert.sConvertToString(drpPartTaxPer.SelectedItem);

                    //Additional Tax 1
                    DropDownList DrpPartTax1 = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("DrpPartTax1");
                    Func.Common.BindDataToCombo(DrpPartTax1, clsCommon.ComboQueryType.EGPPartTax1, 0, " and ID=" + drpPartTax.SelectedValue);
                    DropDownList DrpPartTax1Per = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("DrpPartTax1Per");
                    TextBox txtPartTax1Per = (TextBox)GridPartClaimDetail.Rows[iRowCnt].FindControl("txtPartTax1Per");
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

                    //Additional Tax 2
                    DropDownList DrpPartTax2 = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("DrpPartTax2");
                    Func.Common.BindDataToCombo(DrpPartTax2, clsCommon.ComboQueryType.EGPPartTax2, 0, " and ID=" + drpPartTax.SelectedValue);
                    DropDownList DrpPartTax2Per = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("DrpPartTax2Per");
                    TextBox txtPartTax2Per = (TextBox)GridPartClaimDetail.Rows[iRowCnt].FindControl("txtPartTax2Per");
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
                    ////
                    Label lblClaimQty = (GridPartClaimDetail.Rows[iRowCnt].FindControl("lblClaimQty") as Label);
                    TextBox txtHOApprovedQty = (GridPartClaimDetail.Rows[iRowCnt].FindControl("txtHOApprovedQty") as TextBox);
                    TextBox txtApprovedQty = (GridPartClaimDetail.Rows[iRowCnt].FindControl("txtApprovedQty") as TextBox);
                    RadioButton rdoApproved = (GridPartClaimDetail.Rows[iRowCnt].FindControl("rdoApproved") as RadioButton);
                    RadioButton rdoRejected = (GridPartClaimDetail.Rows[iRowCnt].FindControl("rdoRejected") as RadioButton);
                    if (Func.Convert.iConvertToInt(txtApprovedQty.Text) > 0)
                        rdoApproved.Checked = true;
                    else
                        rdoRejected.Checked = true;

                    if (txtClaimType.Text == "Wrong Supply")
                    {
                        txtApprovedQty.Attributes.Add("onblur", "return CalculateAccPartClaimTotal(event,this);");
                        //txtApprovedQty.Attributes.Add("onblur", "return ValidateQuantity(event,this,'');");
                        //txtApprovedQty.Attributes.Add("onblur", "return ValidateQuantityForWrongSupply(event,this,'');");
                        rdoApproved.Attributes.Add("onclick", "return ValidateQuantityForWrongSupply(event,this,'Approved');");
                        rdoRejected.Attributes.Add("onclick", "return ValidateQuantityForWrongSupply(event,this,'Rejected');");
                    }
                    else if (txtClaimType.Text == "Shortage")
                    {
                        txtApprovedQty.Attributes.Add("onblur", "return CalculateAccPartClaimTotal(event,this);");
                        //txtApprovedQty.Attributes.Add("onblur", "return ValidateQuantity(event,this,'');");
                        //txtApprovedQty.Attributes.Add("onblur", "return ValidateQuantityForShortage(event,this,'');");
                        rdoApproved.Attributes.Add("onclick", "return ValidateQuantityForShortage(event,this,'Approved');");
                        rdoRejected.Attributes.Add("onclick", "return ValidateQuantityForShortage(event,this,'Rejected');");
                    }
                    else
                    {
                        txtApprovedQty.Attributes.Add("onblur", "return CalculateAccPartClaimTotal(event,this);");
                        //txtApprovedQty.Attributes.Add("onblur", "return ValidateQuantity(event,this,'');");
                        rdoApproved.Attributes.Add("onclick", "return ValidateQuantity(event,this,'Approved');");
                        rdoRejected.Attributes.Add("onclick", "return ValidateQuantity(event,this,'Rejected');");
                    }

                    TextBox txtDescription = (GridPartClaimDetail.Rows[iRowCnt].FindControl("txtDescription") as TextBox);
                    TextBox txtHODescription = (GridPartClaimDetail.Rows[iRowCnt].FindControl("txtHODescription") as TextBox);
                    CheckBox chkMRN = (GridPartClaimDetail.Rows[iRowCnt].FindControl("ChkStatusApproved") as CheckBox);
                    DropDownList drpClaimType = (GridPartClaimDetail.Rows[iRowCnt].FindControl("drpClaimType") as DropDownList);
                    Func.Common.BindDataToCombo(drpClaimType, clsCommon.ComboQueryType.PartClaimType, 0, "");

                    DropDownList drpShortageClaimTypeCredit = (GridPartClaimDetail.Rows[iRowCnt].FindControl("drpShortageClaimTypeCredit") as DropDownList);
                    Func.Common.BindDataToCombo(drpShortageClaimTypeCredit, clsCommon.ComboQueryType.PartClaimType, 0, "");



                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Shortage_Claim_Type_Id"]) != 0)
                        drpClaimType.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["Shortage_Claim_Type_Id"]);
                    if (Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["ShortageClaimTypeCredit_Id"]) != 0)
                        drpShortageClaimTypeCredit.SelectedValue = Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["ShortageClaimTypeCredit_Id"]);

                    //DropDownList DrpDescription = (GridPartClaimDetail.Rows[iRowCnt].FindControl("drpDescription") as DropDownList);
                    ////sujata 28012011
                    ////chkMRN.Checked = Func.Convert.sConvertToString(ds.Tables[1].Rows[iRowCnt]["Approved_Status"].ToString());
                    //if (txtApprovedQty.Text == "")
                    //    txtApprovedQty.Text = lblClaimQty.Text;

                    //txtDescription.Style.Add("display", "none");
                    ////chkMRN.Enabled = false;

                    //if(txtApprovedQty.Text == lblClaimQty.Text)                        
                    //    DrpDescription.Style.Add("display", "none");
                    //else
                    //     DrpDescription.Style.Add("display", "");


                    //if (chkMRN.Checked)
                    //{
                    //    DrpDescription.Style.Add("display", "none");
                    //    if (Func.Convert.iConvertToInt(lblClaimQty.Text) > Func.Convert.iConvertToInt(txtApprovedQty.Text))
                    //    {
                    //        DrpDescription.Style.Add("display", "");
                    //    }
                    //    else
                    //    {
                    //        DrpDescription.Style.Add("display", "none");
                    //    }
                    if ((iUserRoleId == 1) && sConfirm == "Y" && (Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Role_ID"]) == 4 || Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartClaimRequest_Status"]) == "N"))
                    {
                        GridPartClaimDetail.Rows[iRowCnt].Enabled = true;
                        //sReqStatus = "Y";
                    }
                    //}
                    //else
                    //{
                    //    DrpDescription.Style.Add("display", "");
                    //    //if (iUserRoleId == 3 && sConfirm == "Y" && (Func.Convert.iConvertToInt(dtDetails.Rows[iRowCnt]["Role_ID"]) == 4 || Func.Convert.sConvertToString(dtDetails.Rows[iRowCnt]["PartClaimRequest_Status"]) == "N"))
                    //    //    GridPartClaimDetail.Rows[iRowCnt].Enabled = false;
                    //}

                }

                //for (int iColCnt = 0; iColCnt < GridPartClaimDetail.Columns.Count; iColCnt++)
                //{
                //    if (sConfirm == "R")
                //    {
                //        GridPartClaimDetail.Columns[iColCnt].Visible = (GridPartClaimDetail.Columns[iColCnt].HeaderText == "HO Appr Qty" || GridPartClaimDetail.Columns[iColCnt].HeaderText == "HO Rej Reason") ? false : true;
                //    }
                //    if (sPClmType != "Wrong Supply")
                //    {
                //        GridPartClaimDetail.Columns[iColCnt].Visible = (GridPartClaimDetail.Columns[iColCnt].HeaderText == "Recv Part No" || GridPartClaimDetail.Columns[iColCnt].HeaderText == "Recv Part Name") ? false : true;
                //    }
                //    if (sPClmType == "Shortage")
                //        GridPartClaimDetail.Columns[iColCnt].Visible = (GridPartClaimDetail.Columns[iColCnt].HeaderText == "Retain" || GridPartClaimDetail.Columns[iColCnt].HeaderText == "Recv Part No" || GridPartClaimDetail.Columns[iColCnt].HeaderText == "Recv Part Name") ? false : true;

                //    GridPartClaimDetail.Columns[GridPartClaimDetail.Columns.Count - 1].Visible = txtClaimType.Text != "Shortage" ? false : true;
                //    GridPartClaimDetail.Columns[GridPartClaimDetail.Columns.Count - 2].Visible = txtClaimType.Text != "Shortage" ? false : true;
                //}
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
                    // END
                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }


                    //Tax
                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxTag = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList DrpTax1ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList drpTaxPer2 = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
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
                double dGrpTotal = 0;
                double dGrpDiscPer = 0;
                double dGrpDiscAmt = 0;
                double dGrpTaxAppAmt = 0;

                double dGrpMTaxPer = 0;
                double dGrpMTaxAmt = 0;

                double dGrpTax1Per = 0;
                double dGrpTax1Amt = 0;

                double dGrpTax2Per = 0;
                double dGrpTax2Amt = 0;

                double dDocTotalAmtFrPFOther = 0;
                double dDocDiscAmt = 0;
                double dDocLSTAmt = 0;
                double dDocCSTAmt = 0;
                double dDocTax1Amt = 0;
                double dDocTax2Amt = 0;
                string sGrpMTaxTag = "";

                double TotalOA = 0;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                for (int i = 0; i < GridPartClaimDetail.Rows.Count; i++)
                {
                    TextBox txtTotal = (TextBox)GridPartClaimDetail.Rows[i].FindControl("txtTotal");
                    TextBox txtGrNo = (TextBox)GridPartClaimDetail.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)GridPartClaimDetail.Rows[i].FindControl("drpPartTax");

                    if (txtGrNo.Text.Trim() != "")
                    {
                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(txtTotal.Text), 2);
                    }

                    for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(txtTotal.Text)).ToString("0.00"));
                        }
                    }
                }
                for (int iRowCnt = 0; iRowCnt < GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

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
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    if (sTax1ApplOn == "1")
                    {
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }


                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }


                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
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
                    TextBox txtDocDisc = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtOtherPer = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                   
                    double dDocPFAmt = 0;
                    double dDocOtherAmt = 0;

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void Acc_SetGridControlPropertyTax()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");
                    string srowGRPID = Func.Convert.sConvertToString(txtGRPID.Text);
                    //Vikram Begin 05062017_BEgin Dislay hide and label change code related to GST Appl and non appl.
                    Acc_GrdPartGroup.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST" : "IGST") : "Tax"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[8].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST %" : "IGST %") : "Tax %"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[9].Text = (hdnIsDocGST.Value == "Y") ? ((Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "SGST Amt" : "IGST Amt") : "Tax Amt"; // Hide Header   

                    Acc_GrdPartGroup.HeaderRow.Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[10].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[12].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[13].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[10].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST" : "Tax1"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[12].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST %" : "Tax1 %"; // Hide Header   
                    Acc_GrdPartGroup.HeaderRow.Cells[13].Text = (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "CGST Amt" : "Tax1 Amt"; // Hide Header                       

                    Acc_GrdPartGroup.HeaderRow.Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[14].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[16].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdPartGroup.HeaderRow.Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdPartGroup.Rows[iRowCnt].Cells[17].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell
                    // END
                    TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");

                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtGrDiscountPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtGrDiscountAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }


                    //Tax
                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax, clsCommon.ComboQueryType.TaxTag_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Type= 1 " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_I, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer, clsCommon.ComboQueryType.TaxTag_Per_O, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')" + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxTag = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxTag");
                    Func.Common.BindDataToCombo(drpTaxTag, clsCommon.ComboQueryType.EGPMainTaxLSTCST, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    drpTax.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxPer.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);
                    drpTaxTag.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax_code"]);

                    drpTax.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTaxPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtTaxTag = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");

                    txtTaxPer.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer.SelectedValue) == 0 ? "0" : drpTaxPer.SelectedItem.Text);
                    txtTaxTag.Text = Func.Convert.sConvertToString(drpTaxTag.SelectedItem);


                    drpTax.Attributes.Add("onBlur", "SetMainInvTax(this,'" + drpTaxPer.ID + "','" + txtTaxPer.ID + "','" + drpTaxTag.ID + "','" + txtTaxTag.ID + "')");


                    //Additional Tax 1
                    DropDownList drpTax1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTax1, clsCommon.ComboQueryType.EGPAdditionalTax1, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')  and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList drpTaxPer1 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer1");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || (srowGRPID.Trim() == "L" && hdnIsDocGST.Value == "N"))
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(drpTaxPer1, clsCommon.ComboQueryType.EGPAdditionalTax1Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    DropDownList DrpTax1ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    if (Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I" || srowGRPID.Trim() == "L")
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='I' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));
                    else
                        Func.Common.BindDataToCombo(DrpTax1ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "') and Tax_Tag='O' " + ((srowGRPID.Trim() == "L") ? " and isnull(Is_Service_Tax,'N') ='Y' and isnull(Active,'N')='Y'" : "  and isnull(Is_Service_Tax,'N') ='N' and isnull(Active,'N')='Y'"));

                    drpTax1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    drpTaxPer1.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);

                    DrpTax1ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax1_code"]);
                    TextBox txtTax1ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1ApplOn");
                    txtTax1ApplOn.Text = DrpTax1ApplOn.SelectedItem.ToString();

                    drpTax1.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax1Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    drpTax1.Attributes.Add("onBlur", "SetAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");
                    txtTax1Per.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(drpTaxPer1.SelectedValue) == 0 ? "0" : drpTaxPer1.SelectedItem.Text);

                    drpTax1.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer1.ID + "','" + txtTax1Per.ID + "')");

                    //Additional Tax 2
                    DropDownList drpTax2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax2");
                    Func.Common.BindDataToCombo(drpTax2, clsCommon.ComboQueryType.EGPAdditionalTax2, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList drpTaxPer2 = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTaxPer2");
                    Func.Common.BindDataToCombo(drpTaxPer2, clsCommon.ComboQueryType.EGPAdditionalTax2Per, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    DropDownList DrpTax2ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    Func.Common.BindDataToCombo(DrpTax2ApplOn, clsCommon.ComboQueryType.EGPPartAddTaxAppl, 0, ((hdnIsDocGST.Value == "Y") ? " And isnull(IS_GST,'N')='Y' " : " And isnull(IS_GST,'N')='N' ") + " And ( DealerID='" + hdnDealerID.Value + "')");

                    drpTax2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    drpTaxPer2.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    DrpTax2ApplOn.SelectedValue = Func.Convert.sConvertToString(Acc_dtGrpTaxDetails.Rows[iRowCnt]["tax2_code"]);
                    TextBox txtTax2ApplOn = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2ApplOn");
                    txtTax2ApplOn.Text = DrpTax2ApplOn.SelectedItem.ToString();

                    drpTax2.Attributes.Add("onblur", "CheckForComboValue(event,this)");

                    TextBox txtTax2Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    drpTax2.Attributes.Add("onBlur", "SetInvAdditionalTax(this,'" + drpTaxPer2.ID + "','" + txtTax2Per.ID + "')");
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
        private void Acc_SetGridControlPropertyTaxCalculation()
        {
            try
            {
                double dGrpTotal = 0;
                double dGrpDiscPer = 0;
                double dGrpDiscAmt = 0;
                double dGrpTaxAppAmt = 0;

                double dGrpMTaxPer = 0;
                double dGrpMTaxAmt = 0;

                double dGrpTax1Per = 0;
                double dGrpTax1Amt = 0;

                double dGrpTax2Per = 0;
                double dGrpTax2Amt = 0;

                double dDocTotalAmtFrPFOther = 0;
                double dDocDiscAmt = 0;
                double dDocLSTAmt = 0;
                double dDocCSTAmt = 0;
                double dDocTax1Amt = 0;
                double dDocTax2Amt = 0;
                string sGrpMTaxTag = "";

                double TotalOA = 0;
                string sTax1ApplOn = "";
                string sTax2ApplOn = "";

                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                    txtGrnetinvamt.Text = "0";
                }

                for (int i = 0; i < GridPartClaimDetail.Rows.Count; i++)
                {
                    TextBox txtAccTotal = (TextBox)GridPartClaimDetail.Rows[i].FindControl("txtAccTotal");
                    TextBox txtGrNo = (TextBox)GridPartClaimDetail.Rows[i].FindControl("txtGrNo");
                    DropDownList drpPartTax = (DropDownList)GridPartClaimDetail.Rows[i].FindControl("drpPartTax");

                    if (txtGrNo.Text.Trim() != "")
                    {
                        TotalOA = Math.Round(TotalOA + Func.Convert.dConvertToDouble(txtAccTotal.Text), 2);
                    }

                    for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                    {
                        TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                        DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                        TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");
                        if (txtGrNo.Text.Trim() == txtGRPID.Text.Trim() && txtGrNo.Text.Trim() != "" && drpTax.SelectedIndex == drpPartTax.SelectedIndex && drpTax.SelectedIndex != 0)
                        {
                            txtGrnetinvamt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) + Func.Convert.dConvertToDouble(txtAccTotal.Text)).ToString("0.00"));
                        }
                    }
                }
                for (int iRowCnt = 0; iRowCnt < Acc_GrdPartGroup.Rows.Count; iRowCnt++)
                {
                    TextBox txtGRPID = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGRPID");

                    DropDownList drpTax = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("drpTax");

                    TextBox txtGrnetinvamt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrnetinvamt");

                    dGrpTotal = Func.Convert.dConvertToDouble(txtGrnetinvamt.Text);

                    TextBox txtGrDiscountPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountPer");
                    TextBox txtGrDiscountAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrDiscountAmt");
                    dGrpDiscPer = Func.Convert.dConvertToDouble(txtGrDiscountPer.Text);
                    dGrpDiscAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal) * Func.Convert.dConvertToDouble(dGrpDiscPer / 100)), 2);
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString((Func.Convert.dConvertToDouble(txtGrnetinvamt.Text) * Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(txtGrDiscountPer.Text) / 100)).ToString("0.00"));

                    dDocDiscAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpDiscAmt) + Func.Convert.dConvertToDouble(dDocDiscAmt), 2);

                    TextBox txtTaxTag = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTag");
                    TextBox txtTaxPer = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxPer");
                    TextBox txtGrTaxAmt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTaxAmt");

                    TextBox txtTax1Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax1Per");
                    TextBox txtGrTax1Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax1Amt");
                    DropDownList DrpTax1ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax1ApplOn");
                    sTax1ApplOn = DrpTax1ApplOn.SelectedItem.ToString();

                    TextBox txtTax2Per = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTax2Per");
                    TextBox txtGrTax2Amt = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtGrTax2Amt");
                    DropDownList DrpTax2ApplOn = (DropDownList)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("DrpTax2ApplOn");
                    sTax2ApplOn = DrpTax2ApplOn.SelectedItem.ToString();

                    TextBox txtTaxTot = (TextBox)Acc_GrdPartGroup.Rows[iRowCnt].FindControl("txtTaxTot");

                    //group Discount Amount display                                   
                    txtGrDiscountAmt.Text = Func.Convert.sConvertToString(dGrpDiscAmt.ToString("0.00"));
                    //Amount whiich is applicable for tax
                    dGrpTaxAppAmt = Math.Round(Func.Convert.dConvertToDouble(dGrpTotal) - Func.Convert.dConvertToDouble(dGrpDiscAmt), 2);

                    //Main tax calculation
                    dGrpMTaxPer = Func.Convert.dConvertToDouble(txtTaxPer.Text.Trim());
                    dGrpMTaxAmt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) * Func.Convert.dConvertToDouble(dGrpMTaxPer / 100)), 2);
                    sGrpMTaxTag = txtTaxTag.Text.Trim();
                    //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
                    if (sGrpMTaxTag == "I")
                    {
                        dDocLSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocLSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    else if (sGrpMTaxTag == "O")
                    {
                        dDocCSTAmt = Math.Round(Func.Convert.dConvertToDouble(dDocCSTAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt), 2);
                    }
                    txtGrTaxAmt.Text = Func.Convert.sConvertToString(dGrpMTaxAmt.ToString("0.00"));

                    dGrpTax1Per = Func.Convert.dConvertToDouble(txtTax1Per.Text);

                    if (sTax1ApplOn == "1")
                    {
                        dGrpTax1Amt = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100));
                    }
                    else if (sTax1ApplOn == "3")
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax1Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax1Per / 100)), 2);
                    }


                    dDocTax1Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax1Amt), 2);
                    txtGrTax1Amt.Text = Func.Convert.sConvertToString(dGrpTax1Amt.ToString("0.00"));

                    dGrpTax2Per = Math.Round(Func.Convert.dConvertToDouble(txtTax2Per.Text), 2);

                    if (sTax2ApplOn == "1")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else if (sTax2ApplOn == "3")
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTotal - dGrpDiscAmt + dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }
                    else
                    {
                        dGrpTax2Amt = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpMTaxAmt) * Func.Convert.dConvertToDouble(dGrpTax2Per / 100)), 2);
                    }


                    dDocTax2Amt = Math.Round(Func.Convert.dConvertToDouble(dDocTax2Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt), 2);
                    txtGrTax2Amt.Text = Func.Convert.sConvertToString(dGrpTax2Amt.ToString("0.00"));
                    dGrpTotal = Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dGrpTaxAppAmt) + Func.Convert.dConvertToDouble(dGrpMTaxAmt) + Func.Convert.dConvertToDouble(dGrpTax1Amt) + Func.Convert.dConvertToDouble(dGrpTax2Amt));
                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dGrpTotal), 2); //This takes for apply PF and Other tax
                    txtTaxTot.Text = Func.Convert.sConvertToString(dGrpTotal.ToString("0.00"));
                }

                for (int i = 0; i < Acc_GrdDocTaxDet.Rows.Count; i++)
                {
                    //Vikram GST Work Begin_08062017
                    Acc_GrdDocTaxDet.HeaderRow.Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[5].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[6].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "I") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[7].Style.Add("display", (hdnIsDocGST.Value == "Y" && Func.Convert.sConvertToString(hdnCustTaxTag.Value) == "O") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : ""); // Hide Header        
                    Acc_GrdDocTaxDet.Rows[i].Cells[8].Style.Add("display", (hdnIsDocGST.Value == "Y") ? "none" : "");//Hide Cell

                    Acc_GrdDocTaxDet.HeaderRow.Cells[5].Text = (hdnIsDocGST.Value == "Y") ? "SGST" : "LST Amt"; // Hide Header    SGST
                    Acc_GrdDocTaxDet.HeaderRow.Cells[6].Text = (hdnIsDocGST.Value == "Y") ? "IGST" : "CST Amt"; // Hide Header    IGST
                    Acc_GrdDocTaxDet.HeaderRow.Cells[7].Text = (hdnIsDocGST.Value == "Y") ? "CGST" : "Tax 1"; // Hide Header   
                    //END
                    TextBox txtDocTotal = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtDocTotal");
                    TextBox txtDocDisc = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtDocDisc");
                    TextBox txtMSTAmt = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtMSTAmt");
                    TextBox txtCSTAmt = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtCSTAmt");

                    TextBox txtTax1 = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtTax1");
                    TextBox txtTax2 = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtTax2");
                    TextBox txtPFPer = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtPFPer");
                    TextBox txtPFAmt = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtPFAmt");
                    TextBox txtOtherPer = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtOtherPer");
                    TextBox txtOtherAmt = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtOtherAmt");
                    TextBox txtGrandTot = (TextBox)Acc_GrdDocTaxDet.Rows[i].FindControl("txtGrandTot");

                    txtDocTotal.Text = Func.Convert.sConvertToString(TotalOA.ToString("0.00"));
                    txtDocDisc.Text = Func.Convert.sConvertToString(dDocDiscAmt.ToString("0.00"));

                    txtMSTAmt.Text = Func.Convert.sConvertToString(dDocLSTAmt.ToString("0.00"));
                    txtCSTAmt.Text = Func.Convert.sConvertToString(dDocCSTAmt.ToString("0.00"));

                    txtTax1.Text = Func.Convert.sConvertToString(dDocTax1Amt.ToString("0.00"));
                    txtTax2.Text = Func.Convert.sConvertToString(dDocTax2Amt.ToString("0.00"));

                    double dDocPFAmt = 0;
                    double dDocOtherAmt = 0;
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtPFAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtPFPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther = Math.Round(Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocPFAmt)), 2);
                    if (hdnIsDocGST.Value == "Y")
                    {
                        txtOtherAmt.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                        txtOtherPer.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this);");
                    }

                    dDocTotalAmtFrPFOther =Math.Round( Func.Convert.dConvertToDouble(Func.Convert.dConvertToDouble(dDocTotalAmtFrPFOther) + Func.Convert.dConvertToDouble(dDocOtherAmt)), hdnIsRoundOFF.Value == "Y" ? 0 : 2);
                    txtGrandTot.Text = Func.Convert.sConvertToString(dDocTotalAmtFrPFOther.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void EnableDisable(string sConfirmStatus, string Is3PL)
        {
            Boolean bEnable = sConfirmStatus == "N" ? true : false;

            //if (sReqStatus != "Y")
            //{
            GridPartClaimDetail.Enabled = bEnable;
            GrdPartGroup.Enabled = false;
            GrdDocTaxDet.Enabled = false;
            Acc_GrdPartGroup.Enabled = bEnable;
            Acc_GrdDocTaxDet.Enabled = bEnable;
            FileAttchGrid.Enabled = false;
            txtCSMRemark.Enabled = bEnable;
            txtRSMRemark.Enabled = bEnable;
            txtRemarks.Enabled = bEnable;
            //if (Is3PL == "P")
            //{
            //    btnReject.Visible = false;
            //    btnSave.Visible = false;            
            //    GridPartClaimDetail.Enabled = false;
            //    btnApprove.Visible = false;
            //}
            //if (Is3PL == "P" && hdnUserRoleId.Value == "12")
            //{
            //    btnReject.Visible = false;
            //    btnApprove.Visible = false;
            //    GridPartClaimDetail.Enabled = false;           
            //    btnSave.Visible = true;
            //}
            //else if (Is3PL != "P" && hdnUserRoleId.Value == "12")
            //{
            //    btnReject.Visible = false;
            //    btnSave.Visible = false;
            //    GridPartClaimDetail.Enabled = false;           
            //    btnApprove.Visible = false;
            //}

            // if (hdnPartClaimMgr.Value == "Y" && hdnProcessContinue.Value == "N" && (hdnUserRoleId.Value == "11" || hdnUserRoleId.Value == "4" || hdnUserRoleId.Value == "1") && (Is3PL == "N" || Is3PL == "Y"))
            if (hdnPartClaimMgr.Value == "Y" && hdnProcessContinue.Value == "N" && (hdnUserRoleId.Value == "2") && (Is3PL == "N" || Is3PL == "Y"))
            {
                // Approved Claim
                btnReject.Visible = bEnable;
                btnApprove.Visible = bEnable;
                btnSave.Visible = false;
            }
            //else if (hdnPartClaimMgr.Value == "P" && hdnProcessContinue.Value == "N" && (hdnUserRoleId.Value == "11" || hdnUserRoleId.Value == "4" || hdnUserRoleId.Value == "1") && (Is3PL == "N" || Is3PL == "Y"))
            else if (hdnPartClaimMgr.Value == "P" && hdnProcessContinue.Value == "N" && (hdnUserRoleId.Value == "2") && (Is3PL == "N" || Is3PL == "Y"))
            {
                // Saving Claim
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnSave.Visible = true;
            }
            else if (hdnPartClaimMgr.Value == "Y" && hdnProcessContinue.Value == "Y" && hdnUserRoleId.Value == "2" && Is3PL == "P")
            {
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnSave.Visible = false;
            }
            else if (hdnPartClaimMgr.Value == "Y" && hdnProcessContinue.Value == "Y" && hdnUserRoleId.Value == "12" && Is3PL == "P")
            {
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnSave.Visible = true;
            }
            else if (hdnPartClaimMgr.Value == "Y" && hdnProcessContinue.Value == "N" && hdnUserRoleId.Value == "12" && Is3PL == "P")
            {
                btnReject.Visible = true;
                btnApprove.Visible = true;
                btnSave.Visible = false;
            }
            else if ((hdnPartClaimMgr.Value == "P" || hdnPartClaimMgr.Value == "Y") && hdnProcessContinue.Value == "N" && hdnUserRoleId.Value == "12" && (Is3PL == "Y" || Is3PL == "N"))
            {
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnSave.Visible = false;
            }
            //else if (hdnUserRoleId.Value == "3")
            else if (hdnUserRoleId.Value == "3" || hdnUserRoleId.Value == "1" || hdnUserRoleId.Value == "4")
            {
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnSave.Visible = false;
                GridPartClaimDetail.Enabled = false;
                Acc_GrdPartGroup.Enabled = false;
                Acc_GrdDocTaxDet.Enabled = false;
            }

            //btnApprove.Enabled = bEnable;
            //btnReject.Enabled = bEnable;
            //btnSave.Enabled = bEnable;
            //}
            //else
            //{
            //    GridPartClaimDetail.Enabled = true;
            //    btnApprove.Enabled = true;
            //    btnReject.Enabled = true;
            //    btnSave.Enabled = true;
            //}

            //btnApprove.Enabled = bEnable;
            //btnReject.Enabled = bEnable;
            //btnSave.Enabled = bEnable;  

            //if (sConfirmStatus == "N")
            //if (txtID.Text != "")
            if (txtID.Text != "" && txtApprovedNo.Text == "")
            {
                //
                //txtApprovedNo.Text = objPartClaim.GenerateApprovalNo(sDealerCode, Func.Convert.iConvertToInt(hdnDealerID.Value), txtClaimType.Text, "N");
                //txtApprovedNo.Text = clsPartClaim.sGetMaxDocNo("PC");
                //txtApprovedDate.Text = Func.Common.sGetCurrentDate(1, false);
                //txtRejectionReason.Enabled = false;            
                //DivBtn.Visible = true;
            }
            else
            {
                //txtRejectionReason.Enabled = false;
                DivBtn.Visible = false;
            }
        }

        //private void HideGridCol()
        //{
        //    if (GridPartClaimDetail.Columns.Count > 0)
        //    {
        //        GridPartClaimDetail.Columns[4].Visible = false;
        //        GridPartClaimDetail.Columns[5].Visible = false;
        //    }
        //    else
        //    {

        //        GridPartClaimDetail.HeaderRow.Cells[4].Visible = false;
        //        GridPartClaimDetail.HeaderRow.Cells[5].Visible = false;

        //        foreach (GridViewRow gvr in GridPartClaimDetail.Rows)
        //        {

        //            gvr.Cells[4].Visible = false;
        //            gvr.Cells[5].Visible = false;

        //        }

        //    }

        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsPartClaim objPartClaim = new clsPartClaim();
                if (hdnUserRoleId.Value == "2")
                {
                    hdnPartClaimMgr.Value = "Y";
                    hdnProcessContinue.Value = "N";
                    AppAndRecjStatus = "N";
                }

                //if (hdn3PLStatus.Value == "3PL" && hdnUserRoleId.Value == "11")
                //{
                //    hdnPartClaimMgr.Value = "Y";
                //    hdn3PL.Value = "P";
                //    hdnProcessContinue.Value = "Y";
                //}
                ////else if (hdn3PLStatus.Value != "3PL" && (hdnUserRoleId.Value == "11" || hdnUserRoleId.Value == "1"))
                //else if (hdnUserRoleId.Value == "11")
                //{
                //    hdnPartClaimMgr.Value = "Y";
                //    hdnProcessContinue.Value = "N";
                //    AppAndRecjStatus = "N";
                //}
                //else if (hdnUserRoleId.Value == "12" && hdnPartClaimMgr.Value == "Y" && hdn3PL.Value == "P")
                //{
                //    hdnPartClaimMgr.Value = "Y";
                //    hdn3PL.Value = "P";
                //    hdnProcessContinue.Value = "N";
                //}
                bDetailsRecordExist = false;
                PartClaimSave(true);
                if (bDetailsRecordExist == false)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter details record');</script>");
                }
                else
                {
                    sConfirm = Func.Convert.sConvertToString(Request.QueryString["Confirm"]);
                    //if (objPartClaim.bSavePartClaim(dtHeader, dtDetails) == true)
                    //if (iUserRoleId != 3)
                    //    sConfirm = "N";
                    if (objPartClaim.bSavePartClaim(dtHeader, dtDetails, dtPartClaimErrorCode, Func.Convert.sConvertToString(sConfirm), "N", iUserRoleId, Acc_dtGrpTaxDetails, Acc_dtTaxDetails) == true)
                    {
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(17,'" + Server.HtmlEncode("Claim") + "','" + Server.HtmlEncode(txtPartClaimNo.Text) + "');</script>");
                        //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Claim Processing Details Saved.');</script>");
                        //string vMsg = "Claim Processing Details Saved..";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage(1,'" + txtPartClaimNo.Text + "');", true);
                        objPartClaim = null;
                        FillHdrAndDtls(Func.Convert.iConvertToInt(txtID.Text), "All");//, sReqStatus
                        EnableDisable(sConfirm, "");
                        lblMessage(sConfirm);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void GridPartClaimDetail_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GridPartClaimDetail.PageIndex = e.NewSelectedIndex;
            FillHdrAndDtls(Func.Convert.iConvertToInt(Request.QueryString["ID"]), "All");//, sReqStatus
        }
        // To Show Attach Documents.
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // GetDataAndDisplay();
        }
        private void ShowAttachedFiles()
        {
            if (dtFileAttach != null || dtFileAttach.Rows.Count != 0)
            {
                FileAttchGrid.DataSource = dtFileAttach;
                FileAttchGrid.DataBind();
            }
        }
        private Boolean CheckForApprove(Boolean chckStatus)
        {
            //int ichecked=0;
            //for (int iRowCnt = 0; iRowCnt < GridPartClaimDetail.Rows.Count; iRowCnt++)
            //{
            //    int iApprQty = 0;
            //    iApprQty = Func.Convert.iConvertToInt((GridPartClaimDetail.Rows[iRowCnt].FindControl("txtApprovedQty") as TextBox).Text);
            //    CheckBox chkPartClaim = new CheckBox();
            //    chkPartClaim = (GridPartClaimDetail.Rows[iRowCnt].FindControl("ChkStatusApproved") as CheckBox);
            //    if (chkPartClaim.Checked == chckStatus)
            //    {
            //        ichecked = ichecked + 1;
            //    }
            //}
            //if (ichecked ==0)
            //{
            //    return false ;
            //}
            return true;
        }
        private Boolean CheckForCancle(Boolean chckStatus)
        {
            //int ichecked = 0;
            //for (int iRowCnt = 0; iRowCnt < GridPartClaimDetail.Rows.Count; iRowCnt++)
            //{
            //    int iApprQty = 0;
            //    iApprQty = Func.Convert.iConvertToInt((GridPartClaimDetail.Rows[iRowCnt].FindControl("txtApprovedQty") as TextBox).Text);
            //    CheckBox chkPartClaim = new CheckBox();
            //    chkPartClaim = (GridPartClaimDetail.Rows[iRowCnt].FindControl("ChkStatusApproved") as CheckBox);
            //    if (chkPartClaim.Checked == chckStatus)
            //    {
            //        ichecked = ichecked + 1;
            //    }
            //}
            //if (ichecked != GridPartClaimDetail.Rows.Count)
            //{
            //    return false;
            //}
            return true;
        }
        
        private void SetControlPropertyToPartClaimGridForReason(bool bRecordIsOpen)
        {
            try
            {
                int idtRowCnt = 0;

                for (int iRowCnt = 0; iRowCnt < GridPartClaimDetail.Rows.Count; iRowCnt++)
                {

                    //Reason
                    //DropDownList drpDescription = (DropDownList)GridPartClaimDetail.Rows[iRowCnt].FindControl("drpDescription");
                    //Func.Common.BindDataToCombo(drpDescription, clsCommon.ComboQueryType.PartClaimRejectionReason, 0);

                    //Reason 
                    TextBox txtDescription = (TextBox)GridPartClaimDetail.Rows[iRowCnt].FindControl("txtDescription");

                    // Add New Description
                    ListItem lstitm = new ListItem("NEW", "9999");
                    //drpDescription.Items.Add(lstitm);
                    //drpDescription.Attributes.Add("onChange", "OnComboValueChange(this,'" + txtDescription.ID + "')");

                    if (idtRowCnt < dtDetForReason.Rows.Count)
                    {
                        txtDescription.Text = Func.Convert.sConvertToString(dtDetForReason.Rows[iRowCnt]["Rejection_Res"].ToString().Trim());
                        //drpDescription.SelectedValue = Func.Convert.sConvertToString(dtDetForReason.Rows[iRowCnt]["Rejection_Res"]);

                        idtRowCnt = idtRowCnt + 1;
                    }

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        public void ExportToSpreadsheet(DataTable table, string name)
        {
            GridView gvPartDetails = new GridView();
            gvPartDetails.DataSource = table;
            gvPartDetails.DataBind();
            HtmlForm form = new HtmlForm();
            string attachment = "attachment; filename=Employee.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            form.Controls.Add(gvPartDetails);
            this.Controls.Add(form);
            form.RenderControl(htextw);
            Response.Write(stw.ToString());
            Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtExport = new DataTable();
            if (ViewState["dtDetails"] != null)
            {
                dtExport = (DataTable)ViewState["dtDetails"];
                dtExport.Columns.Remove("ID");
                dtExport.Columns.Remove("PartClaim_Hdr_ID");
                dtExport.Columns.Remove("part_ID");
                dtExport.Columns.Remove("Role_ID");
                dtExport.Columns.Remove("RecvPartID");
                dtExport.Columns.Remove("Box Condition");
                dtExport.Columns.Remove("grr_tr_no");
                dtExport.Columns.Remove("PartClaimRequest_Status");
                dtExport.Columns.Remove("retain");
                dtExport.Columns.Remove("Approved_Status");
                dtExport.Columns.Remove("Shortage_Claim_Type_Id");
                dtExport.Columns.Remove("ShortageClaimTypeCredit_Id");
                dtExport.Columns.Remove("ShortageClaimType");
                dtExport.Columns.Remove("Box_Description");
                dtExport.Columns.Remove("Inv_No1");
                dtExport.Columns.Remove("GRR_tr_No1");
                ExportToSpreadsheet(dtExport, "PartClaim");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            clsCommon objCommon = new clsCommon();
            if (objCommon.sUserRole == "15")
            {
                btnSave.Style.Add("display", "none");
                btnApprove.Style.Add("display", "none");
                btnReject.Style.Add("display", "none");
            }
        }
        protected void GridPartClaimDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if ((Label)e.Row.FindControl("lblInvoiceNo") != null)
            //{

            //    if (ALPart[(int)PartClaim.EnvNo] == ((Label)e.Row.FindControl("lblInvoiceNo")).Text)
            //        e.Row.FindControl("lblInvoiceNo").Visible = false;
            //    else
            //        ALPart[(int)PartClaim.EnvNo] = ((Label)e.Row.FindControl("lblInvoiceNo")).Text;
            //}
            //if ((Label)e.Row.FindControl("lblInvoiceDate") != null)
            //{

            //    if (ALPart[(int)PartClaim.EnvDate] == ((Label)e.Row.FindControl("lblInvoiceDate")).Text)
            //        e.Row.FindControl("lblInvoiceDate").Visible = false;
            //    else
            //        ALPart[(int)PartClaim.EnvDate] = ((Label)e.Row.FindControl("lblInvoiceDate")).Text;
            //}
            //if ((Label)e.Row.FindControl("lblGrrNo") != null)
            //{

            //    if (ALPart[(int)PartClaim.GrrNo] == ((Label)e.Row.FindControl("lblGrrNo")).Text)
            //        e.Row.FindControl("lblGrrNo").Visible = false;
            //    else
            //        ALPart[(int)PartClaim.GrrNo] = ((Label)e.Row.FindControl("lblGrrNo")).Text;
            //}
            //if ((Label)e.Row.FindControl("lblGrrDate") != null)
            //{

            //    if (ALPart[(int)PartClaim.GrrDate] == ((Label)e.Row.FindControl("lblGrrDate")).Text)
            //        e.Row.FindControl("lblGrrDate").Visible = false;
            //    else
            //        ALPart[(int)PartClaim.GrrDate] = ((Label)e.Row.FindControl("lblGrrDate")).Text;
            //}
        }

    }
}