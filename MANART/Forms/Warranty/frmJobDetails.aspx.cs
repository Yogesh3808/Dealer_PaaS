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
using System.IO;


namespace MANART.Forms.Warranty
{
    public partial class frmJobDetails : System.Web.UI.Page
    {
        private DataTable dtPart = new DataTable();
        private DataTable dtLabour = new DataTable();
        private DataTable dtLubricant = new DataTable();
        private DataTable dtSublet = new DataTable();
        private DataTable dtJob = new DataTable();
        private int iClaimID = 0;
        private int iJobID = 0;
        private clsWarranty.enmClaimType ValenmFormUsedFor;
        string sDealerId = "";
        int iUserId = 0;
        string sRequestOrClaim = "";
        int iUserClaimSlabId = 0;
        int iUserRoleId = 0;
        int iUserType = 0;
        int iUserChange = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                sRequestOrClaim = Func.Convert.sConvertToString(Request.QueryString["RequestOrClaim"]);
                iClaimID = Func.Convert.iConvertToInt(Request.QueryString["ClaimID"]);
                iJobID = Func.Convert.iConvertToInt(Request.QueryString["JobID"]);
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iUserClaimSlabId = Func.Convert.iConvertToInt(Session["UserClaimSlabID"]);
                iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
                if (!IsPostBack)
                {

                    Session["ComplaintsDetails"] = null;
                    Session["InvestigationDetails"] = null;
                    Session["PartDetails"] = null;
                    Session["LabourDetails"] = null;
                    Session["LubricantDetails"] = null;
                    Session["SubletDetails"] = null;
                    SetDocumentDetails();
                    FillCombo();
                    GetDataAndDisplay();
                }
                if (iClaimID != 0)
                {

                    //GetDataAndDisplay();
                    //if (ViewState["Claim_Status"] != null)
                    //    btnBack.Attributes.Add("onClick", "return GetJobDetails('" + Convert.ToString(ViewState["Claim_Status"]) + "');");
                }
                ExpirePageCache();
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
                //if ((iUserRoleId == 4  && iUserType == 2) || (iUserRoleId == 3 && iUserType == 1) || sRequestOrClaim == "R")
                //if (((iUserRoleId == 4 || iUserRoleId == 3 || iUserRoleId == 2 || iUserRoleId == 1 || iUserRoleId == 9 || iUserRoleId == 10) && iUserType == 2) || (iUserRoleId == 3 && iUserType == 1) || sRequestOrClaim == "R")
                PVehicleDetails.Style.Add("display", (sRequestOrClaim == "R") ? "none" : "");

                if (sRequestOrClaim != "R")
                {
                    if (rdoDeductionType.SelectedValue == "L")
                    {
                        txtPartHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtLabourHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtLubricantHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        txtSubletHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    }
                    else
                    {
                        txtPartHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        txtLabourHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        txtLubricantHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        txtSubletHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");

                        //txtPartHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                        //txtLabourHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                        //txtLubricantHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                        //txtSubletHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");

                        txtPartHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                        txtLabourHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                        txtLubricantHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                        txtSubletHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");

                        txtPartHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToParts(event,this)");
                        txtLabourHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToLabour(event,this)");
                        txtLubricantHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToLubricant(event,this)");
                        txtSubletHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToSublet(event,this)");

                    }
                }
                else
                {
                    rdoDeductionType.Style.Add("display", "none");
                }

                if (!IsPostBack)
                {
                    lblAddPart.Attributes.Add("onclick", " return ShowCausalPartMaster(this," + sDealerId + ")");
                }
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
        // set Document text 
        private void SetDocumentDetails()
        {
            try
            {
                //System.Threading.Thread.Sleep(2000);
                //Common

                lblRefClaimNo.Style.Add("display", "none");
                lblRefClaimDate.Style.Add("display", "none");

                txtRefClaimNo.Style.Add("display", "none");
                txtRefClaimDate.Style.Add("display", "none");


                txtRequestNo.Style.Add("display", "none");
                lblRequestNo.Style.Add("display", "none");
                lblRequestDate.Style.Add("display", "none");
                txtRequestDate.Style.Add("display", "none");

                lblTitle.Text = " Job Details";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // FillCombo
        private void FillCombo()
        {
            //Func.Common.BindDataToCombo(drpCulpritCode, clsCommon.ComboQueryType.CulpritCode, 0);
            Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0);
            
            //Func.Common.BindDataToCombo(drpCulpritCodeTemp, clsCommon.ComboQueryType.CulpritCode, 0);
            FillCulpritCodeTemp(clsWarranty.GetCulpritDefect("C", "N",0));

            //Func.Common.BindDataToCombo(drpDefectCode, clsCommon.ComboQueryType.DefectCode, 0);            
            Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0);

            //Func.Common.BindDataToCombo(drpDefectCodeTemp, clsCommon.ComboQueryType.DefectCode, 0);            
            FillDefectCodeTemp(clsWarranty.GetCulpritDefect("D", "N", 0));

            Func.Common.BindDataToCombo(drpTechnicalCode, clsCommon.ComboQueryType.TechnicalCode, 0);
        }

        private void FillCulpritCodeTemp(DataTable dtCulpritCode)
        {

            drpCulpritCodeTemp.DataSource = dtCulpritCode;
            drpCulpritCodeTemp.DataTextField = "Name";
            drpCulpritCodeTemp.DataValueField = "ID";
            drpCulpritCodeTemp.DataBind();
            //drpCulpritCodeTemp.Items.RemoveAt(0);
            drpCulpritCodeTemp.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void FillDefectCodeTemp(DataTable dtDefectCode)
        {
            drpDefectCodeTemp.DataSource = dtDefectCode;
            drpDefectCodeTemp.DataTextField = "Name";
            drpDefectCodeTemp.DataValueField = "ID";
            drpDefectCodeTemp.DataBind();
            //drpDefectCodeTemp.Items.RemoveAt(0);
            drpDefectCodeTemp.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        // To Add Part No and Name into Causel Part Combo
        //private void AddPartToCausalPart(string  sPartId,string sPartNo)
        //{
        //    ListItem lstPart = null;
        //    if (drpParts.Items.Count == 0)
        //    {
        //        lstPart = new ListItem("--Select--", "0");
        //        drpParts.Items.Add(lstPart);
        //    }
        //    lstPart = new ListItem(sPartNo, sPartId);
        //    drpParts.Items.Add(lstPart);
        //}
        // Set Control property To Part Grid
        private void SetControlPropertyToPartGrid()
        {
            try
            {
                int iActQtyFor = 0;
                if (PartDetailsGrid.Rows.Count == 0) return;
                PartDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");//Agrregate
                PartDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//Inv No
                PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");//Inv Date
                PartDetailsGrid.HeaderRow.Cells[11].Style.Add("display", "none");//dealerCode
                PartDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "none");//VECV Percentage            
                PartDetailsGrid.HeaderRow.Cells[17].Style.Add("display", "none");//Dealer Percentage   
                PartDetailsGrid.HeaderRow.Cells[18].Style.Add("display", "none");//Cust Percentage  
                PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", "none");//Agrregate
                PartDetailsGrid.HeaderRow.Cells[25].Style.Add("display", "none");//VAT(%) for SA
                PartDetailsGrid.HeaderRow.Cells[26].Style.Add("display", "none");//VAT(Amount) for SA
                //If Domestic Then Allowed details Changed Check box column not display
                //if (txtDomestic_Export.Text == "D")
                //{
                PartDetailsGrid.HeaderRow.Cells[22].Style.Add("display", "none");//Check box To Change Details            
                //}
                if ((ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal && iUserType == 2) || (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal && (iUserRoleId != 9 && iUserRoleId != 10) && iUserType == 1))
                {
                    PartDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");    //Agregate
                    PartDetailsGrid.HeaderRow.Cells[23].Style.Add("display", "");
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    PartDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[17].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[18].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", "none");//deduction
                    iActQtyFor = iActQtyFor + 1;
                }
                else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmSparesPartsWarranty)
                {
                    PartDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "");
                    PartDetailsGrid.HeaderRow.Cells[11].Style.Add("display", "");
                    //PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", "none");//deduction Changed by shyamal on 16/02/2011
                    PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", "");//deduction
                    iActQtyFor = iActQtyFor + 1;
                }
                //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                //{
                //    PartDetailsGrid.HeaderRow.Cells[25].Style.Add("display", "");//VAT(%) for SA
                //    PartDetailsGrid.HeaderRow.Cells[26].Style.Add("display", "");//VAT(Amount) for SA
                //    PartDetailsGrid.HeaderRow.Cells[19].Style.Add("display", "none");//deduction Percentage
                //    PartDetailsGrid.HeaderRow.Cells[21].Style.Add("display", "none");//Deduction Remark
                //    iActQtyFor = iActQtyFor + 1;
                //}
                string sPartId = "0";
                double dPartAmount = 0;
                double dSAPartTaxAmount = 0;
                string sPartNo = "";

                double dOrgPartTotal = 0;
                double dOrgPartTaxAmount = 0;
                double dPartTaxAmount = 0;
                for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Show dealer Flag Column only arunachala Case
                    PartDetailsGrid.HeaderRow.Cells[28].Style.Add("display", (txtDealerCode.Text.Trim().StartsWith("R") && (sRequestOrClaim != "R" || sRequestOrClaim != "HR")) ? "" : "none"); // Hide Header        
                    PartDetailsGrid.Rows[iRowCnt].Cells[28].Style.Add("display", (txtDealerCode.Text.Trim().StartsWith("R") && (sRequestOrClaim != "R" || sRequestOrClaim != "HR")) ? "" : "none");//Hide Cell

                    sPartId = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                    sPartNo = (PartDetailsGrid.Rows[iRowCnt].FindControl("lblPartNo") as Label).Text;


                    //AddPartToCausalPart(sPartId, sPartNo);

                    TextBox txtAccQty = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccQty");
                    
                    //txtAccQty.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    //if (iActQtyFor == 0)
                    //{
                        txtAccQty.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 6 + ")");
                        txtAccQty.Attributes.Add("onblur", "return CalculatePartAcceptedAmtQtyChangedNM(event,this)");
                    //}
                    //else
                    //{
                    //    txtAccQty.Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + 6 + ")");
                    //    txtAccQty.Attributes.Add("onblur", "return CalculatePartAcceptedAmtQtyChanged(event,this)");
                    //}

                    //txtAccQty.Attributes.Add("onblur", "CalculatePartAcceptedAmtQtyChanged(event,this)");

                    TextBox txtDeduction = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction");
                    //txtDeduction.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                    //txtDeduction.Attributes.Add("onblur", "CalculatePartAcceptedAmtDeductionChanged(event,this)");
                    txtDeduction.Attributes.Add("onblur", "CalculatePartAcceptedAmtDeductionChanged(event,this)");

                    TextBox txtDeductionRemarks = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtRemarks");
                    //txtDeductionRemarks.Attributes.Add("onblur", "DeductionChangedRemark(event,this)");


                    TextBox txtAccAmount = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount");
                    //txtAccAmount.Attributes.Add("readOnly", "true");
                    TextBox txtDealerOrMTI_Flag = (TextBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerOrMTI_Flag") as TextBox);
                    txtDealerOrMTI_Flag.Text = Func.Convert.sConvertToString(dtPart.Rows[iRowCnt]["DealerOrMTI_Flag"]).Trim();

                    PartDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");// Agrregate
                    PartDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//Inv No
                    PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//Inv Date
                    PartDetailsGrid.Rows[iRowCnt].Cells[11].Style.Add("display", "none");//dealerCode
                    PartDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "none");//VECV Percentage                
                    PartDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", "none");//Dealer Percentage                
                    PartDetailsGrid.Rows[iRowCnt].Cells[18].Style.Add("display", "none");//Cust Percentage 
                    PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", "none");//Agrregate Name 
                    PartDetailsGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "none");//VAT(%) for SA 
                    PartDetailsGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "none");//VAT(Amount) for SA 
                    if (((ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal || ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC) && iUserType == 2) || (ValenmFormUsedFor == clsWarranty.enmClaimType.enmNormal && (iUserRoleId != 9 && iUserRoleId != 10) && iUserType == 1))
                    {
                        PartDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");// Agrregate
                        PartDetailsGrid.Rows[iRowCnt].Cells[23].Style.Add("display", "");// Agrregate
                        if (rdoDeductionType.SelectedValue == "G")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                        else
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        }
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        PartDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "");// VECV Percentage                
                        PartDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", "");// Dealer Percentage             
                        PartDetailsGrid.Rows[iRowCnt].Cells[18].Style.Add("display", "");// Cust Percentage  
                        PartDetailsGrid.Rows[iRowCnt].Cells[19].Style.Add("display", "none");//deduction

                        TextBox txtVECVShare = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare");
                        //txtVECVShare.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");

                        txtVECVShare.Attributes.Add("onblur", "CalculatePartAcceptedAmtVecvPercentChanged(event,this)");
                    }
                    else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmSparesPartsWarranty)
                    {
                        PartDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");//Inv No
                        PartDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "");//Inv Date
                        PartDetailsGrid.Rows[iRowCnt].Cells[11].Style.Add("display", "");//dealerCode
                        if (rdoDeductionType.SelectedValue == "G")
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                        else
                        {
                            (PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        }
                    }
                    //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                    //{
                    //    PartDetailsGrid.Rows[iRowCnt].Cells[25].Style.Add("display", "");//VAT(%) for SA
                    //    PartDetailsGrid.Rows[iRowCnt].Cells[26].Style.Add("display", "");//VAT(Amount) for SA
                    //    PartDetailsGrid.Rows[iRowCnt].Cells[15].Enabled = false;//Actual Qty
                    //    PartDetailsGrid.Rows[iRowCnt].Cells[19].Style.Add("display", "none");//deduction
                    //    PartDetailsGrid.Rows[iRowCnt].Cells[21].Style.Add("display", "none");//Deduction Remark

                    //    dSAPartTaxAmount = dSAPartTaxAmount + Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtVATAmt") as TextBox).Text);
                    //}
                    dOrgPartTotal = dOrgPartTotal + Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                    dPartAmount = dPartAmount + Func.Convert.dConvertToDouble(txtAccAmount.Text);

                    //if (txtDomestic_Export.Text == "D")
                    //{
                    PartDetailsGrid.Rows[iRowCnt].Cells[22].Style.Add("display", "none");//Check Change Details
                    //}
                    //else
                    //{
                    //    CheckBox ChkForAccept = (CheckBox)PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept");
                    //    ChkForAccept.Checked = false;
                    //    if (dtPart.Rows.Count>0)
                    //    if (dtPart.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    //    {
                    //        ChkForAccept.Checked = true;
                    //    }
                    //}
                    TextBox txtVATPercentage = (TextBox)PartDetailsGrid.Rows[iRowCnt].FindControl("txtVATPercentage");

                    dOrgPartTaxAmount = dOrgPartTaxAmount + ((Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) * Func.Convert.dConvertToDouble(txtVATPercentage.Text.ToString())) / 100);
                    dPartTaxAmount = dPartTaxAmount + ((Func.Convert.dConvertToDouble(txtAccAmount.Text) * Func.Convert.dConvertToDouble(txtVATPercentage.Text.ToString())) / 100);

                }
                txtOrgPartAmount.Text = dOrgPartTotal.ToString("0.00");
                txtPartAmount.Text = dPartAmount.ToString("0.00");
                //if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                //{
                //    txtOrgPartTaxAmount.Text = dSAPartTaxAmount.ToString("0.00");
                //    txtPartTaxAmount.Text = dSAPartTaxAmount.ToString("0.00");
                //}
                //else
                //{
                    txtOrgPartTaxAmount.Text = ((dOrgPartTotal * Func.Convert.dConvertToDouble(hdnPartTax.Value)) / 100).ToString("0.00");
                    txtPartTaxAmount.Text = ((dPartAmount * Func.Convert.dConvertToDouble(hdnPartTax.Value)) / 100).ToString("0.00");
                //}

                if (iUserChange == 0)
                {
                    if (hdnISDocGST.Value.Trim() == "Y")
                    {

                        txtOrgLabourST.Text = dOrgPartTaxAmount.ToString("0.00");
                        txtAccLabourST.Text = dPartTaxAmount.ToString("0.00");

                        txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgPartTotal + dOrgPartTaxAmount).ToString("0.00");
                        txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dPartAmount + dPartTaxAmount).ToString("0.00");
                    }
                    else
                    {
                        txtOrgLabourST.Text = "0.00";
                        txtAccLabourST.Text = "0.00";

                        txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgPartTotal + Func.Convert.dConvertToDouble((txtOrgPartTaxAmount.Text == "") ? "0" : txtOrgPartTaxAmount.Text)).ToString("0.00");
                        txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dPartAmount + Func.Convert.dConvertToDouble((txtPartTaxAmount.Text == "") ? "0" : txtPartTaxAmount.Text)).ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Labour Grid
        //private void SetControlPropertyToLabourGrid()
        //{
        //    try
        //    {
        //        double dLabourAmount = 0;
        //        double dOrgLabourTotal = 0;
        //        double dLaborNonTaxAmtTotal = 0;
        //        double dOrgLaborAccidentalAmtTotal = 0;
        //        double dAccLaborAccidentalAmtTotal = 0;
        //        if (LabourDetailsGrid.Rows.Count == 0) return;
        //        if (LabourDetailsGrid.Rows.Count != 0)
        //        {
        //            LabourDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");//VECV Percentage            
        //            LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");//Dealer Percentage   
        //            LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//Cust Percentage  
        //            LabourDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");//VAT(%) for SA         
        //            LabourDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "none");//VAT(Amount) for SA    
        //        }
        //        if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
        //        {
        //            if (LabourDetailsGrid.Rows.Count != 0)
        //            {
        //                LabourDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");//VECV Percentage            
        //                LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");//Dealer Percentage   
        //                LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");//Cust Percentage   
        //                LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");//deduction
        //            }
        //        }
        //        else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
        //        {
        //            if (LabourDetailsGrid.Rows.Count != 0)
        //            {
        //                LabourDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "");//VAT(%) for SA         
        //                LabourDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "");//VAT(Amount) for SA                       
        //                LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");//deduction
        //                LabourDetailsGrid.HeaderRow.Cells[12].Style.Add("display", "none");//deduction Remark 
        //            }
        //        }
        //        //If Domestic Then Allowed details Changed Check box column not display
        //        //if (txtDomestic_Export.Text == "D")
        //        //{
        //            LabourDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none");//Check box To Change Details            
        //        //}
        //        for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
        //        {


        //            LabourDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//VECV Percentage 
        //            LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Dealer Percentage
        //            LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//Cust Percentage
        //            LabourDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");//VAT(%) for SA         
        //            LabourDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "none");//VAT(Amount) for SA 
        //            if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
        //            {
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");//VECV Percentage 
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");//Dealer Percentage
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");//Cust Percentage
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//deduction
        //                TextBox txtVECVShare = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare");
        //                txtVECVShare.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
        //                txtVECVShare.Attributes.Add("onblur", "CalculateLabourAcceptedAmtVecvPercentChanged(event,this)");
        //            }
        //            else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
        //            {
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");//VAT(%) for SA         
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "");//VAT(Amount) for SA                       
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//deduction
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//deduction Remark 

        //            }
        //            else
        //                if (rdoDeductionType.SelectedValue == "G")
        //                {
        //                    (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
        //                }
        //                else
        //                {
        //                    (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
        //                }
        //            if ((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblLabourCode") as Label).Text.Contains("55555"))
        //            {
        //                dOrgLaborAccidentalAmtTotal = dOrgLaborAccidentalAmtTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
        //                dAccLaborAccidentalAmtTotal = dAccLaborAccidentalAmtTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
        //            }
        //            if ((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblLabourCode") as Label).Text.Contains("33333"))
        //            {
        //                dLaborNonTaxAmtTotal = dLaborNonTaxAmtTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);

        //            }
        //            else
        //            {
        //                dOrgLabourTotal = dOrgLabourTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
        //                dLabourAmount = dLabourAmount + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
        //            }

        //            //if (txtDomestic_Export.Text == "D")
        //            //{
        //                LabourDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "none");//Check box
        //            //}
        //            //else
        //            //{
        //            //    CheckBox ChkForAccept = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept");
        //            //    ChkForAccept.Checked = false;
        //            //    if (dtLabour.Rows.Count > 0)
        //            //    if (dtLabour.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
        //            //    {
        //            //        ChkForAccept.Checked = true;
        //            //    }
        //            //}
        //        }
        //        double dNetOrgLaborAccidentalAmtTotal = 0;
        //        double dNetAccLaborAccidentalAmtTotal = 0;
        //        dNetOrgLaborAccidentalAmtTotal = (dOrgLaborAccidentalAmtTotal * 30) / 100;
        //        dNetAccLaborAccidentalAmtTotal = (dAccLaborAccidentalAmtTotal * 30) / 100;

        //        hdnOrgAccdentalAmtTotal.Value = dOrgLaborAccidentalAmtTotal.ToString("0.00");
        //        txtOrgLabourAmount.Text = (dOrgLabourTotal - dNetOrgLaborAccidentalAmtTotal).ToString("0.00");
        //        txtOrgLabourST.Text = (((dOrgLabourTotal - dNetOrgLaborAccidentalAmtTotal) * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100).ToString("0.00");
        //        txtOrgNonTaxLabourAmt.Text = (dLaborNonTaxAmtTotal + dNetOrgLaborAccidentalAmtTotal).ToString("0.00");

        //        hdnAccAccdentalAmtTotal.Value =dAccLaborAccidentalAmtTotal.ToString("0.00");
        //        txtLabourAmount.Text = (dLabourAmount - dNetAccLaborAccidentalAmtTotal).ToString("0.00");
        //        txtAccLabourST.Text = (((dLabourAmount - dNetAccLaborAccidentalAmtTotal) * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100).ToString("0.00");
        //        txtAccNonTaxLabourAmt.Text = (dLaborNonTaxAmtTotal + dNetAccLaborAccidentalAmtTotal).ToString("0.00");

        //        if (iUserChange == 0)
        //        {
        //            txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgLabourTotal  + (((dOrgLabourTotal - dNetOrgLaborAccidentalAmtTotal) * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100) + dLaborNonTaxAmtTotal).ToString("0.00");
        //            txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dLabourAmount + (((dLabourAmount-dNetAccLaborAccidentalAmtTotal) * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100) + dLaborNonTaxAmtTotal).ToString("0.00");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}
        private void SetControlPropertyToLabourGrid()
        {
            try
            {
                double dLabourAmount = 0;
                double dOrgLabourTotal = 0;
                double dLaborNonTaxAmtTotal = 0;
                double dAccLaborNonTaxAmount = 0;

                if (LabourDetailsGrid.Rows.Count == 0) return;
                if (LabourDetailsGrid.Rows.Count != 0)
                {
                    LabourDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");//VECV Percentage            
                    LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");//Dealer Percentage   
                    LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//Cust Percentage  
                    LabourDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");//VAT(%) for SA         
                    LabourDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "none");//VAT(Amount) for SA    
                }
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    if (LabourDetailsGrid.Rows.Count != 0)
                    {
                        LabourDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");//VECV Percentage            
                        LabourDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");//Dealer Percentage   
                        LabourDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "");//Cust Percentage   
                        LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");//deduction
                    }
                }
                //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                //{
                //    if (LabourDetailsGrid.Rows.Count != 0)
                //    {
                //        LabourDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "");//VAT(%) for SA         
                //        LabourDetailsGrid.HeaderRow.Cells[16].Style.Add("display", "");//VAT(Amount) for SA                       
                //        LabourDetailsGrid.HeaderRow.Cells[10].Style.Add("display", "none");//deduction
                //        LabourDetailsGrid.HeaderRow.Cells[12].Style.Add("display", "none");//deduction Remark 
                //    }
                //}
                //If Domestic Then Allowed details Changed Check box column not display
                //if (txtDomestic_Export.Text == "D")
                //{
                LabourDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none");//Check box To Change Details            
                //}
                for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
                {


                    LabourDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");//VECV Percentage 
                    LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");//Dealer Percentage
                    LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//Cust Percentage
                    LabourDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");//VAT(%) for SA         
                    LabourDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "none");//VAT(Amount) for SA 
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        LabourDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");//VECV Percentage 
                        LabourDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");//Dealer Percentage
                        LabourDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "");//Cust Percentage
                        LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//deduction
                        TextBox txtVECVShare = (TextBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare");
                        //txtVECVShare.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                        txtVECVShare.Attributes.Add("onblur", "CalculateLabourAcceptedAmtVecvPercentChanged(event,this)");
                    }
                    //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                    //{
                    //    LabourDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");//VAT(%) for SA         
                    //    LabourDetailsGrid.Rows[iRowCnt].Cells[16].Style.Add("display", "");//VAT(Amount) for SA                       
                    //    LabourDetailsGrid.Rows[iRowCnt].Cells[10].Style.Add("display", "none");//deduction
                    //    LabourDetailsGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//deduction Remark 

                    //}
                    else
                        if (rdoDeductionType.SelectedValue == "G")
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                        else
                        {
                            (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        }
                    //if ((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblLabourCode") as Label).Text == "333333")
                    //          dLaborNonTaxAmtTotal = dLaborNonTaxAmtTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);

                    //dOrgLabourTotal = dOrgLabourTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                    //dLabourAmount = dLabourAmount + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);

                    if ((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblLabourCode") as Label).Text.Contains("33333"))
                    {
                        dLaborNonTaxAmtTotal = dLaborNonTaxAmtTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                        dAccLaborNonTaxAmount = dAccLaborNonTaxAmount + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                    }
                    else if ((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblLabourCode") as Label).Text.Contains("55555"))
                    {
                        dLaborNonTaxAmtTotal = dLaborNonTaxAmtTotal + (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) - (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) * 70 / 100));
                        dOrgLabourTotal = dOrgLabourTotal + (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) - (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) * 30 / 100));
                        dLabourAmount = dLabourAmount + (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text) - (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text) * 30 / 100));
                        dAccLaborNonTaxAmount = dAccLaborNonTaxAmount + (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text) - (Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text) * 70 / 100));
                    }
                    else
                    {
                        dOrgLabourTotal = dOrgLabourTotal + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                        dLabourAmount = dLabourAmount + Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                    }

                    //if (txtDomestic_Export.Text == "D")
                    //{
                    LabourDetailsGrid.Rows[iRowCnt].Cells[13].Style.Add("display", "none");//Check box
                    //}
                    //else
                    //{
                    //    CheckBox ChkForAccept = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept");
                    //    ChkForAccept.Checked = false;
                    //    if (dtLabour.Rows.Count > 0)
                    //    if (dtLabour.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    //    {
                    //        ChkForAccept.Checked = true;
                    //    }
                    //}
                }

                txtOrgLabourAmount.Text = dOrgLabourTotal.ToString("0.00");
                txtOrgLabourST.Text = (Func.Convert.dConvertToDouble((txtOrgLabourST.Text == "") ? "0.00" : txtOrgLabourST.Text) + ((dOrgLabourTotal * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100)).ToString("0.00");
                txtOrgNonTaxLabourAmt.Text = dLaborNonTaxAmtTotal.ToString("0.00");

                txtLabourAmount.Text = dLabourAmount.ToString("0.00");
                txtAccLabourST.Text = (Func.Convert.dConvertToDouble((txtAccLabourST.Text == "") ? "0.00" : txtAccLabourST.Text) + ((dLabourAmount * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100)).ToString("0.00");

                //txtAccNonTaxLabourAmt.Text = dLaborNonTaxAmtTotal.ToString("0.00");
                txtAccNonTaxLabourAmt.Text = dAccLaborNonTaxAmount.ToString("0.00");


                if (iUserChange == 0)
                {
                    txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgLabourTotal + ((dOrgLabourTotal * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100) + dLaborNonTaxAmtTotal).ToString("0.00");
                    //txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dLabourAmount + ((dLabourAmount * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100) + dLaborNonTaxAmtTotal).ToString("0.00");
                    txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dLabourAmount + ((dLabourAmount * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100) + dAccLaborNonTaxAmount).ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Set Control property To Lubricant  Grid
        private void SetControlPropertyToLubricantGrid()
        {
            try
            {
                double dLubricantAmount = 0;
                double dOrgLubricantAmount = 0;
                double dOrgLubricantTaxAmount = 0;
                double dLubricantTaxAmount = 0;
                if (LubricantDetailsGrid.Rows.Count == 0) return;
                if (LubricantDetailsGrid.Rows.Count != 0)
                {
                    LubricantDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "none");//VECV Percentage
                    LubricantDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");//Deler Percentage
                    LubricantDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");//Cust Percentage
                    LubricantDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "none");//VAT(%) for SA         
                    LubricantDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");//VAT(Amount) for SA 
                }

                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    if (LubricantDetailsGrid.Rows.Count != 0)
                    {
                        LubricantDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "");//VECV Percentage
                        LubricantDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");//Deler Percentage
                        LubricantDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");//Cust Percentage
                        LubricantDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//deduction
                    }
                }
                //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                //{
                //    if (LubricantDetailsGrid.Rows.Count != 0)
                //    {
                //        LubricantDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "");//VAT(%) for SA         
                //        LubricantDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "");//VAT(Amount) for SA                       
                //        LubricantDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//deduction
                //        LubricantDetailsGrid.HeaderRow.Cells[11].Style.Add("display", "none");//deduction Remark 
                //    }
                //}


                ////If Domestic Then Allowed details Changed Check box column not display
                //if (txtDomestic_Export.Text == "D")
                //{
                LubricantDetailsGrid.HeaderRow.Cells[12].Style.Add("display", "none");//Check box To Change Details            
                //}
                for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
                {
                    //Show dealer Flag Column only arunachala Case
                    LubricantDetailsGrid.HeaderRow.Cells[17].Style.Add("display", (txtDealerCode.Text.Trim().StartsWith("R") && (sRequestOrClaim != "R" || sRequestOrClaim != "HR")) ? "" : "none"); // Hide Header        
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[17].Style.Add("display", (txtDealerCode.Text.Trim().StartsWith("R") && (sRequestOrClaim != "R" || sRequestOrClaim != "HR")) ? "" : "none");//Hide Cell

                    TextBox txtLubDealerOrMTI_Flag = (TextBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtLubDealerOrMTI_Flag") as TextBox);
                    txtLubDealerOrMTI_Flag.Text = Func.Convert.sConvertToString(dtLubricant.Rows[iRowCnt]["DealerOrMTI_Flag"]).Trim();
                    TextBox txtDeduction = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction");
                    //txtDeduction.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                    txtDeduction.Attributes.Add("onblur", "CalculateLubricantAcceptedAmtDeductionChanged(event,this)");


                    TextBox txtAccAmount = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount");
                    TextBox txtVATPercentage = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVATPercentage");
                    
                    //txtAccAmount.Attributes.Add("readOnly", "true");

                    LubricantDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "none");//VAT(%) for SA         
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");//VAT(Amount) for SA   

                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "");//VECV Percentage
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");//Deler Percentage
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");//Cust Percentage                
                        LubricantDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//deduction
                        TextBox txtVECVShare = (TextBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare");
                        //txtVECVShare.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                        txtVECVShare.Attributes.Add("onblur", "CalculateLubricantAcceptedAmtVecvPercentChanged(event,this)");
                    }
                    //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                    //{
                    //    LubricantDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");//VAT(%) for SA         
                    //    LubricantDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");//VAT(Amount) for SA                       
                    //    LubricantDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//deduction
                    //    LubricantDetailsGrid.Rows[iRowCnt].Cells[11].Style.Add("display", "none");//deduction Remark 
                    //}
                    else
                        if (rdoDeductionType.SelectedValue == "G")
                        {
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                        else
                        {
                            (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        }
                    dOrgLubricantAmount = dOrgLubricantAmount + Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                    dLubricantAmount = dLubricantAmount + Func.Convert.dConvertToDouble(txtAccAmount.Text);

                    dOrgLubricantTaxAmount = dOrgLubricantTaxAmount + ((Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text) * Func.Convert.dConvertToDouble(txtVATPercentage.Text.ToString())) / 100);
                    dLubricantTaxAmount = dLubricantTaxAmount + ((Func.Convert.dConvertToDouble(txtAccAmount.Text) * Func.Convert.dConvertToDouble(txtVATPercentage.Text.ToString())) / 100);
                    

                    //if (txtDomestic_Export.Text == "D" || txtDomestic_Export.Text == "E")
                    //{
                    LubricantDetailsGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//Check box
                    //}
                    //else
                    //{
                    //    CheckBox ChkForAccept = (CheckBox)LubricantDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept");
                    //    ChkForAccept.Checked = false;
                    //    if (dtLubricant.Rows.Count > 0)
                    //    if (dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    //    {
                    //        ChkForAccept.Checked = true;
                    //    }
                    //}
                }
                txtOrgLubricantAmount.Text = dOrgLubricantAmount.ToString("0.00");
                txtLubricantAmount.Text = dLubricantAmount.ToString("0.00");
                if (iUserChange == 0)
                {
                    if (hdnISDocGST.Value.Trim() == "Y")
                    {
                        txtOrgLabourST.Text = (Func.Convert.dConvertToDouble((txtOrgLabourST.Text == "") ? "0.00" : txtOrgLabourST.Text) + dOrgLubricantTaxAmount).ToString("0.00");
                        txtAccLabourST.Text = (Func.Convert.dConvertToDouble((txtAccLabourST.Text == "") ? "0.00" : txtAccLabourST.Text) + dLubricantTaxAmount).ToString("0.00");


                        txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgLubricantAmount + dOrgLubricantTaxAmount).ToString("0.00");
                        txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dLubricantAmount + dLubricantTaxAmount).ToString("0.00");
                    }
                    else
                    {
                        txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgLubricantAmount).ToString("0.00");
                       txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dLubricantAmount).ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // Set Control property To Sublet Grid
        private void SetControlPropertyToSubletGrid()
        {
            try
            {
                double dSubletAmount = 0;
                double dOrgSubletAmount = 0;
                double dOrgLaborTaxAmtTotal = 0;
                double dAccLaborTaxAmtTotal = 0;

                if (SubletDetailsGrid.Rows.Count == 0) return;

                if (SubletDetailsGrid.Rows.Count != 0)
                {
                    SubletDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "none");//VECV Percentage
                    SubletDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "none");//Dealer Percentage
                    SubletDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "none");//Cust Percentage
                    SubletDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "none");//VAT(%) for SA         
                    SubletDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "none");//VAT(Amount) for SA 
                }
                if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                {
                    if (SubletDetailsGrid.Rows.Count != 0)
                    {
                        SubletDetailsGrid.HeaderRow.Cells[6].Style.Add("display", "");//VECV Percentage
                        SubletDetailsGrid.HeaderRow.Cells[7].Style.Add("display", "");//Dealer Percentage
                        SubletDetailsGrid.HeaderRow.Cells[8].Style.Add("display", "");//Cust Percentage
                        SubletDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//deduction
                    }
                }
                //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                //{
                //    if (LubricantDetailsGrid.Rows.Count != 0)
                //    {
                //        SubletDetailsGrid.HeaderRow.Cells[14].Style.Add("display", "");//VAT(%) for SA         
                //        SubletDetailsGrid.HeaderRow.Cells[15].Style.Add("display", "");//VAT(Amount) for SA                       
                //        SubletDetailsGrid.HeaderRow.Cells[9].Style.Add("display", "none");//deduction
                //        SubletDetailsGrid.HeaderRow.Cells[11].Style.Add("display", "none");//deduction Remark 
                //    }
                //}
                //If Domestic Then Allowed details Changed Check box column not display
                if (txtDomestic_Export.Text == "D")
                {
                    //VHP_Warranty  start
                    //SubletDetailsGrid.HeaderRow.Cells[13].Style.Add("display", "none");//Check box To Change Details            
                    //VHP_Warranty  End
                }
                else
                {
                    SubletDetailsGrid.HeaderRow.Cells[3].Style.Add("display", "none");//Hide ManHrs
                    SubletDetailsGrid.HeaderRow.Cells[4].Style.Add("display", "none");//Hide Rate
                }
                SubletDetailsGrid.HeaderRow.Cells[12].Style.Add("display", "none");//Check box To Change Details 
                for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
                {

                    TextBox txtDeduction = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction");
                    //txtDeduction.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                    txtDeduction.Attributes.Add("onblur", "CalculateSubLetAcceptedAmtDeductionChanged(event,this)");



                    SubletDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "none");
                    SubletDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "none");//VAT(%) for SA         
                    SubletDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "none");//VAT(Amount) for SA   
                    if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmGoodwillRequest)
                    {
                        SubletDetailsGrid.Rows[iRowCnt].Cells[6].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[7].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[8].Style.Add("display", "");
                        SubletDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");
                        TextBox txtVECVShare = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare");
                        //txtVECVShare.Attributes.Add("onkeypress", "CheckPercentageAmount(event,this)");
                        txtVECVShare.Attributes.Add("onblur", "CalculateSubletAcceptedAmtVecvPercentChanged(event,this)");
                    }
                    //else if (ValenmFormUsedFor == clsWarranty.enmClaimType.enmAMC)
                    //{
                    //    SubletDetailsGrid.Rows[iRowCnt].Cells[14].Style.Add("display", "");//VAT(%) for SA         
                    //    SubletDetailsGrid.Rows[iRowCnt].Cells[15].Style.Add("display", "");//VAT(Amount) for SA                       
                    //    SubletDetailsGrid.Rows[iRowCnt].Cells[9].Style.Add("display", "none");//deduction
                    //    SubletDetailsGrid.Rows[iRowCnt].Cells[11].Style.Add("display", "none");//deduction Remark 
                    //}
                    else
                        if (rdoDeductionType.SelectedValue == "G")
                        {
                            (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                        }
                        else
                        {
                            (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                        }

                    TextBox txtAccAmount = (TextBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount");
                    //txtAccAmount.Attributes.Add("readOnly", "true");

                    dOrgSubletAmount = dOrgSubletAmount + Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("lblClaimedAmount") as Label).Text);
                    dSubletAmount = dSubletAmount + Func.Convert.dConvertToDouble(txtAccAmount.Text);

                    if (txtDomestic_Export.Text == "D")
                    {
                        //VHP_Warranty  start
                        SubletDetailsGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//Check box
                        //VHP_Warranty  End
                    }
                    else
                    {
                        SubletDetailsGrid.Rows[iRowCnt].Cells[3].Style.Add("display", "none");//Hide ManHRS
                        SubletDetailsGrid.Rows[iRowCnt].Cells[4].Style.Add("display", "none");//Hide Rate
                        SubletDetailsGrid.Rows[iRowCnt].Cells[12].Style.Add("display", "none");//Check box
                        //CheckBox ChkForAccept = (CheckBox)SubletDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept");
                        //ChkForAccept.Checked = false;
                        //if (dtSublet.Rows.Count > 0)
                        //if (dtSublet.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                        //{
                        //    ChkForAccept.Checked = true;
                        //}
                    }
                }

                txtOrgSubletAmount.Text = dOrgSubletAmount.ToString("0.00");
                dOrgLaborTaxAmtTotal = Func.Convert.dConvertToDouble((txtOrgLabourST.Text == "") ? "0.00" : txtOrgLabourST.Text); ;
                dAccLaborTaxAmtTotal = Func.Convert.dConvertToDouble((txtAccLabourST.Text == "") ? "0.00" : txtAccLabourST.Text);
                txtOrgLabourST.Text = (Func.Convert.dConvertToDouble((txtOrgLabourST.Text == "") ? "0.00" : txtOrgLabourST.Text) + (dOrgSubletAmount * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100).ToString("0.00");
                txtSubletAmount.Text = dSubletAmount.ToString("0.00");
                txtAccLabourST.Text = (Func.Convert.dConvertToDouble((txtAccLabourST.Text == "") ? "0.00" : txtAccLabourST.Text) + (dSubletAmount * Func.Convert.dConvertToDouble(hdnLabourST.Value)) / 100).ToString("0.00");
                if (iUserChange == 0)
                {
                    txtOrgAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtOrgAccClaimAmt.Text) + dOrgSubletAmount + Func.Convert.dConvertToDouble(txtOrgLabourST.Text) - dOrgLaborTaxAmtTotal).ToString("0.00");
                    txtAccClaimAmt.Text = (Func.Convert.dConvertToDouble(txtAccClaimAmt.Text) + dSubletAmount + Func.Convert.dConvertToDouble(txtAccLabourST.Text) - dAccLaborTaxAmtTotal).ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }



        //Fill Details From Part Grid
        private bool bFillDetailsFromPartGrid()
        {

            dtPart = (DataTable)Session["PartDetails"];
            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < PartDetailsGrid.Rows.Count; iRowCnt++)
            {
                dtPart.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                dtPart.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                dtPart.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                dtPart.Rows[iRowCnt]["Accepted_Qty"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccQty") as TextBox).Text);
                dtPart.Rows[iRowCnt]["Deduction_Percentage"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Text);
                dtPart.Rows[iRowCnt]["Accepted_Amount"] = Func.Convert.dConvertToDouble((PartDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                dtPart.Rows[iRowCnt]["Deduction_Remark"] = (PartDetailsGrid.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text;

                CheckBox ChkAccept = (CheckBox)(PartDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept"));
                if (ChkAccept.Checked == true)
                {
                    if (dtPart.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "N")
                    {
                        dtPart.Rows[iRowCnt]["ChangeDetails_YN"] = "Y";
                    }
                    else
                    {
                        dtPart.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }

                }
                else
                {
                    if (dtPart.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    {
                        dtPart.Rows[iRowCnt]["ChangeDetails_YN"] = "N";
                    }
                    else
                    {
                        dtPart.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
            }

            return bValidate;

        }


        //Fill Details From Labour  Grid
        private bool bFillDetailsFromLabourGrid()
        {
            dtLabour = (DataTable)Session["LabourDetails"];
            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
            {
                dtLabour.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                //VHP_Waaraty start
                dtLabour.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                dtLabour.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);
                //VHP_Waaraty END
                //dtLabour.Rows[iRowCnt]["Accepted_HRS"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccManHrs") as TextBox).Text);
                dtLabour.Rows[iRowCnt]["Deduction_Percentage"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Text);
                dtLabour.Rows[iRowCnt]["Accepted_Amount"] = Func.Convert.dConvertToDouble((LabourDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                dtLabour.Rows[iRowCnt]["Deduction_Remark"] = (LabourDetailsGrid.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text;

                CheckBox ChkAccept = (CheckBox)(LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept"));
                if (ChkAccept.Checked == true)
                {
                    if (dtLabour.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "N")
                    {
                        dtLabour.Rows[iRowCnt]["ChangeDetails_YN"] = "Y";
                    }
                    else
                    {
                        dtLabour.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                else
                {
                    if (dtLabour.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    {
                        dtLabour.Rows[iRowCnt]["ChangeDetails_YN"] = "N";
                    }
                    else
                    {
                        dtLabour.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                bValidate = true;
            }

            return bValidate;

        }

        //Fill Details From Lubricant Grid
        private bool bFillDetailsFromLubricantGrid()
        {


            dtLubricant = (DataTable)Session["LubricantDetails"];
            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < LubricantDetailsGrid.Rows.Count; iRowCnt++)
            {
                dtLubricant.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                dtLubricant.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                dtLubricant.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                //dtLubricant.Rows[iRowCnt]["Accepted_Qty"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtAccQty") as TextBox).Text);
                dtLubricant.Rows[iRowCnt]["Deduction_Percentage"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Text);
                dtLubricant.Rows[iRowCnt]["Accepted_Amount"] = Func.Convert.dConvertToDouble((LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                dtLubricant.Rows[iRowCnt]["Deduction_Remark"] = (LubricantDetailsGrid.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text;

                CheckBox ChkAccept = (CheckBox)(LubricantDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept"));
                if (ChkAccept.Checked == true)
                {
                    if (dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "N")
                    {
                        dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"] = "Y";
                    }
                    else
                    {
                        dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                else
                {
                    if (dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    {
                        dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"] = "N";
                    }
                    else
                    {
                        dtLubricant.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                bValidate = true;
            }
            return bValidate;
        }

        //Fill Details From Sublet Grid
        private bool bFillDetailsFromSubletGrid()
        {

            dtSublet = (DataTable)Session["SubletDetails"];
            bool bValidate = true;
            for (int iRowCnt = 0; iRowCnt < SubletDetailsGrid.Rows.Count; iRowCnt++)
            {
                dtSublet.Rows[iRowCnt]["VECV_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtVECVShare") as TextBox).Text);
                dtSublet.Rows[iRowCnt]["Dealer_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDealerShare") as TextBox).Text);
                dtSublet.Rows[iRowCnt]["Cust_Share"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtCustShare") as TextBox).Text);

                dtSublet.Rows[iRowCnt]["Deduction_Percentage"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtDeduction") as TextBox).Text);
                dtSublet.Rows[iRowCnt]["Accepted_Amount"] = Func.Convert.dConvertToDouble((SubletDetailsGrid.Rows[iRowCnt].FindControl("txtAccAmount") as TextBox).Text);
                dtSublet.Rows[iRowCnt]["Deduction_Remark"] = (SubletDetailsGrid.Rows[iRowCnt].FindControl("txtRemarks") as TextBox).Text;

                CheckBox ChkAccept = (CheckBox)(SubletDetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept"));
                if (ChkAccept.Checked == true)
                {
                    if (dtSublet.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "N")
                    {
                        dtSublet.Rows[iRowCnt]["ChangeDetails_YN"] = "Y";
                    }
                    else
                    {
                        dtSublet.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                else
                {
                    if (dtSublet.Rows[iRowCnt]["ChangeDetails_YN"].ToString() == "Y")
                    {
                        dtSublet.Rows[iRowCnt]["ChangeDetails_YN"] = "N";
                    }
                    else
                    {
                        dtSublet.Rows[iRowCnt]["ChangeDetails_YN"] = "";
                    }
                }
                bValidate = true;
            }
            return bValidate;
        }

        private void UpdateHdrValueFromControl(DataTable dtHdr)
        {
            try
            {
                DataRow dr;
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Part_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Labor_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Lubricant_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Sublet_Amt", typeof(double)));
                dtHdr.Columns.Add(new DataColumn("Domestic_Export", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("PartDeduction", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("LabourDeduction", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("LubricantDeduction", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("SubletDeduction", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("Is_Global", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));                
                

                dr = dtHdr.NewRow();
                dr["ID"] = iClaimID;
                
                //dr["Part_Amt"] = Func.Convert.dConvertToDouble(txtTotalPartAmount.Text);
                //dr["Labor_Amt"] = Func.Convert.dConvertToDouble(txtTotalLabourAmount.Text);
                //dr["Lubricant_Amt"] = Func.Convert.dConvertToDouble(txtTotalLubricantAmount.Text);
                //dr["Sublet_Amt"] = Func.Convert.dConvertToDouble(txtTotalSubletAmount.Text);

                dr["Part_Amt"] = Func.Convert.dConvertToDouble(txtPartAmount.Text);
                dr["Labor_Amt"] = Func.Convert.dConvertToDouble(txtLabourAmount.Text);
                dr["Lubricant_Amt"] = Func.Convert.dConvertToDouble(txtLubricantAmount.Text);
                dr["Sublet_Amt"] = Func.Convert.dConvertToDouble(txtSubletAmount.Text);
                dr["Domestic_Export"] = txtDomestic_Export.Text;
                dr["PartDeduction"] = Func.Convert.iConvertToInt(txtPartHeaderDeduction.Text);
                dr["LabourDeduction"] = Func.Convert.iConvertToInt(txtLabourHeaderDeduction.Text);
                dr["LubricantDeduction"] = Func.Convert.iConvertToInt(txtLubricantHeaderDeduction.Text);
                dr["SubletDeduction"] = Func.Convert.iConvertToInt(txtSubletHeaderDeduction.Text);
                dr["Is_Global"] = rdoDeductionType.SelectedValue;
                dr["Job_Code_ID"] = iJobID;

                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private bool bFillCulpritDetails(DataTable dtJob)
        {
            bool bValidate = true;
            DataRow dr;
            dtJob.Columns.Add(new DataColumn("ID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Claim_Hdr_ID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Part_No_ID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Culprit_ID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Defect_ID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Technical_ID", typeof(int)));

            dr = dtJob.NewRow();

            dr["ID"] = Func.Convert.iConvertToInt(lblDefectCulprit.Text);
            dr["Claim_Hdr_ID"] = iClaimID;
            dr["Part_No_ID"] = Func.Convert.iConvertToInt(lblCausalPartNo.Text);
            dr["Culprit_ID"] = Func.Convert.iConvertToInt(drpCulpritCode.SelectedValue);
            dr["Defect_ID"] = Func.Convert.iConvertToInt(drpDefectCode.SelectedValue);
            dr["Technical_ID"] = Func.Convert.iConvertToInt(drpTechnicalCode.SelectedValue);

            dtJob.Rows.Add(dr);
            dtJob.AcceptChanges();
            return bValidate;
        }
        //ToSave Record
        private void SaveRecord()
        {
            try
            {
                bool bSaveRecord = false;
                DataTable dtJob = new DataTable();
                //DataTable dtHdr = new DataTable();
                clsWarranty ObjWarranty = new clsWarranty();
                DataTable dtHdr = new DataTable();
                UpdateHdrValueFromControl(dtHdr);

                // Get Culprit Details
                if (bFillCulpritDetails(dtJob) == false) return;
                //Get Part Details     
                if (bFillDetailsFromPartGrid() == false) return;

                //Get Labour Details     
                if (bFillDetailsFromLabourGrid() == false) return;

                //Get Lubricant Details     
                if (bFillDetailsFromLubricantGrid() == false) return;

                //Get Sublet Details     
                if (bFillDetailsFromSubletGrid() == false) return;

                bSaveRecord = ObjWarranty.bProcessJobDetails(dtHdr, dtJob, dtPart, dtLabour, dtLubricant, dtSublet, iUserId, sRequestOrClaim, iUserRoleId);

                if (bSaveRecord == true)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowMessage(4)", true);
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Job Deduction Details get Saved.');</script>");
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowMessage(5)", true);
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in Job Deduction Saving.');</script>");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private void GetDataAndDisplay()
        {
            try
            {
                clsWarranty ObjWarranty = new clsWarranty();
                DataSet ds = new DataSet();
                if (iClaimID != 0)
                {
                    ds = ObjWarranty.GetJobDetails(iJobID, iClaimID, sRequestOrClaim, "", iUserRoleId);
                    DisplayData(ds);

                    if (Func.Common.iTableCntOfDatatSet(ds) == 0)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(6);</script>");
                        return;
                    }
                }
                ds = null;
                ObjWarranty = null;
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
                bool bEnableControls = true;

                string sClaim_TypeID = "0";


                //Display Header        
                txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                iClaimID = Func.Convert.iConvertToInt(txtID.Text);
                sDealerId = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);
                txtClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_no"]);
                txtClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_date"]);


                txtModelCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_code"]);
                txtModelDescription.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_Name"]);
                txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]); ;
                txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
                txtVehicleNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Vehicle_No"]);
                txtClaimType.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Name"]);
                txtDomestic_Export.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Domestic_Export"]);

                txtPCRHDRID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PCRHDRID"]);
                txtJobHDRID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["JobHDRID"]);
                hdnISDocGST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocGST"]);
                hdnInvType.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["InvType"]);

                lnkSelectJobDtl.Attributes.Add("onclick", " return GetJobcodeDtls(this,'" + sDealerId + "','" + Func.Convert.sConvertToString(iJobID) + "','" + txtPCRHDRID.Text.ToString() + "','Y')");

                lnkSelectJobDtl.Style.Add("display", "");
                if (sRequestOrClaim == "R" || sRequestOrClaim == "HR") lnkSelectJobDtl.Style.Add("display", "none"); 

                txtPartHeaderDeduction.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartDeduction"]);
                txtLabourHeaderDeduction.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabourDeduction"]);
                txtLubricantHeaderDeduction.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LubricantDeduction"]);
                txtSubletHeaderDeduction.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletDeduction"]);
                rdoDeductionType.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_Global"]);
                hdnHeaderPartDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartDeduction"]);
                hdnHeaderLabourDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabourDeduction"]);
                hdnHeaderLubricantDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LubricantDeduction"]);
                hdnHeaderSubletDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletDeduction"]);
                hdnHeaderPrePartDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartDeduction"]);
                hdnHeaderPreLabourDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabourDeduction"]);
                hdnHeaderPreLubricantDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["SubletDeduction"]);
                hdnHeaderPreSubletDeduction.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabourDeduction"]);
                sClaim_TypeID = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_type_Id"]);
                hdnPartTax.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["PartTax_Percentage"]);
                hdnLabourST.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LabourST_Percentage"]);
                hdnCurrency.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Currency"]);
                txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DealerCode"]);

                lblClaimAmt.Text = "Claimed Amount";
                lblAcceptedAmt.Text = "Accepted Amount";
                lblClaimAmt.Text = lblClaimAmt.Text + "(in " + hdnCurrency.Value + ")";
                lblAcceptedAmt.Text = lblAcceptedAmt.Text + "(in " + hdnCurrency.Value + ")";

                if (sRequestOrClaim == "HR" && sClaim_TypeID == "17")
                    PJobDetails.Style.Add("display", "none");
                else
                    PJobDetails.Style.Add("display", "");

                //VHP Start WC
                //ViewState["Claim_Status"] = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Status"]);

                //if (ViewState["Claim_Status"] != null)
                btnBack.Attributes.Add("onClick", "return GetJobDetails('" + Convert.ToString(ViewState["Claim_Status"]) + "');");


                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Status"]) != "1" && (iUserRoleId != 9 && iUserRoleId != 10 && iUserRoleId != 4 && iUserRoleId != 3 && iUserRoleId != 2 && iUserRoleId != 1))
                {

                    btnSave.Visible = false;
                    PartDetailsGrid.Enabled = false;
                    LabourDetailsGrid.Enabled = false;
                    LubricantDetailsGrid.Enabled = false;
                    SubletDetailsGrid.Enabled = false;
                    //lblRequestNo.Style.Add("display", "none");
                }
                else
                {
                    //btnSave.Enabled = fasle;
                }

                PVehicleDetails.Style.Add("display", (sRequestOrClaim == "R") ? "none" : "");

                clsWarranty ObjWarranty = new clsWarranty();
                ValenmFormUsedFor = ObjWarranty.GetEnmClaimType(sClaim_TypeID);
                HdnClaimType.Value = sClaim_TypeID;
                ObjWarranty = null;
                if ((sClaim_TypeID == "2" || sClaim_TypeID == "8" || sClaim_TypeID == "16") && Func.Convert.sConvertToString(Request.QueryString["RequestOrClaim"]) != "C")
                {
                    ValenmFormUsedFor = clsWarranty.enmClaimType.enmGoodwillRequest;
                    lblClaimNo.Text = "Request No.";
                    lblClaimDate.Text = "Request Date";
                }


                txtPartAmount.Text = "0.00";
                txtLabourAmount.Text = "0.00";
                txtLubricantAmount.Text = "0.00";
                txtSubletAmount.Text = "0.00";
                txtAccClaimAmt.Text = "0.00";
               


                txtRequestNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]);// goodwill Request No 
                txtRequestDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_Date"]);// goodwill Request Date

                if (txtRequestNo.Text != "")
                {

                    lblRequestNo.Style.Add("display", "none");
                    lblRequestDate.Style.Add("display", "none");
                    txtRequestNo.Style.Add("display", "none");
                    txtRequestDate.Style.Add("display", "none");
                }
                txtRefClaimNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_no"]);
                txtRefClaimDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Ref_Claim_date"]);

                if (txtRefClaimNo.Text != "")
                {
                    lblRefClaimNo.Style.Add("display", "");
                    lblRefClaimDate.Style.Add("display", "");

                    txtRefClaimNo.Style.Add("display", "");
                    txtRefClaimDate.Style.Add("display", "");
                }

                // Display Culprit/ Technical/ Defect Details       
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    Func.Common.BindDataToCombo(drpCulpritCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.CulpritCode : clsCommon.ComboQueryType.CulpritCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Culprit_ID"]) + ")");
                    Func.Common.BindDataToCombo(drpDefectCode, (HttpContext.Current.Session["UserType"].ToString() == "3") ? clsCommon.ComboQueryType.DefectCode : clsCommon.ComboQueryType.DefectCodeMTI, 0, " or (Id =" + Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Defect_ID"]) + ")");

                    FillCulpritCodeTemp(clsWarranty.GetCulpritDefect("C", "N", Func.Convert.iConvertToInt(ds.Tables[1].Rows[0]["Culprit_ID"])));
                    FillDefectCodeTemp(clsWarranty.GetCulpritDefect("D", "N", Func.Convert.iConvertToInt(ds.Tables[1].Rows[0]["Defect_ID"])));

                    drpCulpritCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Culprit_ID"]);
                    drpCulpritCodeTemp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Culprit_ID"]);
                    drpDefectCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Defect_ID"]);
                    drpDefectCodeTemp.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Defect_ID"]);
                    drpTechnicalCode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Technical_ID"]);
                    lblJobCode.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Job_Code"]);
                    if (Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Warratable"]) == "N")
                        drpTechnicalCode.Enabled = true;
                    else
                        drpTechnicalCode.Enabled = false;

                    txtPartNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Part_No"]);
                    lblCausalPartNo.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Part_ID"]);
                    lblDefectCulprit.Text = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                    // AddPartToCausalPart(Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Part_ID"]), Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Part_No"]));
                }
                //drpParts.SelectedIndex = 1;
                //Display Part Details   
                dtPart = ds.Tables[2];

                Session["PartDetails"] = dtPart;
                PartDetailsGrid.DataSource = dtPart;
                PartDetailsGrid.DataBind();
                lblPartRecCnt.Text = Func.Common.sRowCntOfTable(dtPart);
                SetControlPropertyToPartGrid();
                if (dtPart.Rows.Count > 0)
                    txtPartHeaderDeduction.Enabled = true;
                else
                    txtPartHeaderDeduction.Enabled = false;
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    //  drpParts.SelectedValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["ID"]);
                }
                // Display Labour Details     
                dtLabour = ds.Tables[3];
                Session["LabourDetails"] = dtLabour;
                LabourDetailsGrid.DataSource = dtLabour;
                LabourDetailsGrid.DataBind();
                lblLabourRecCnt.Text = Func.Common.sRowCntOfTable(dtLabour);
                SetControlPropertyToLabourGrid();
                if (dtLabour.Rows.Count > 0)
                    txtLabourHeaderDeduction.Enabled = true;
                else
                    txtLabourHeaderDeduction.Enabled = false;

                // Display Lubricant Details    
                dtLubricant = ds.Tables[4];
                Session["LubricantDetails"] = dtLubricant;
                LubricantDetailsGrid.DataSource = dtLubricant;
                LubricantDetailsGrid.DataBind();
                lblLubricantRecCnt.Text = Func.Common.sRowCntOfTable(dtLubricant);
                SetControlPropertyToLubricantGrid();
                if (dtLubricant.Rows.Count > 0)
                    txtLubricantHeaderDeduction.Enabled = true;
                else
                    txtLubricantHeaderDeduction.Enabled = false;

                // Display Sublet Details
                dtSublet = ds.Tables[5];
                Session["SubletDetails"] = dtSublet;
                SubletDetailsGrid.DataSource = dtSublet;
                SubletDetailsGrid.DataBind();
                lblSubletRecCnt.Text = Func.Common.sRowCntOfTable(dtSublet);
                SetControlPropertyToSubletGrid();
                if (dtSublet.Rows.Count > 0)
                    txtSubletHeaderDeduction.Enabled = true;
                else
                    txtSubletHeaderDeduction.Enabled = false;

                bEnableControls = true;
                //if (iUserClaimSlabId == Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["Claim_Slab_ID"]))
                //{
                //    bEnableControls = true;
                //}
                //else
                //{
                //    bEnableControls = false;
                //}

                if (txtDomestic_Export.Text == "D")
                {
                    if (iUserRoleId == 4)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N") //Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["GCR_No"]).Trim() == "" &&
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 3)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["CSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 2)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 1)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 13)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadRetail_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 14)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["HeadSaleMkgAfterMkg_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 9)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P")// && sRequestOrClaim=="C" && txtRequestNo.Text =="")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 10)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "P")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 15)
                    {
                        bEnableControls = false;
                    }
                    else if (iUserRoleId == 18)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P")// && sRequestOrClaim=="C" && txtRequestNo.Text =="")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                }
                else
                {
                    if (iUserRoleId == 3)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 2)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 1)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQ"]) == "N" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ASM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["RSM_SubmitYN"]) == "Y" && Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Head_SubmitYN"]) == "N")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                    else if (iUserRoleId == 9)
                    {
                        if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Is_SHQResource"]) == "P")// && sRequestOrClaim=="C" && txtRequestNo.Text =="")
                            bEnableControls = true;
                        else
                            bEnableControls = false;
                    }
                }

                if (bEnableControls == true)
                {
                    MakeEnableDisableControls(true);
                }
                else
                {
                    MakeEnableDisableControls(false);
                }



                // To Update Accepted Part Amount To Header 
                txtTotalPartAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[6].Rows[0]["TotalPartAmount"]).ToString("0.00");
                txtTotalLabourAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[6].Rows[0]["TotalLabourAmount"]).ToString("0.00");
                txtTotalLubricantAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[6].Rows[0]["TotalLubricantAmount"]).ToString("0.00");
                txtTotalSubletAmount.Text = Func.Convert.dConvertToDouble(ds.Tables[6].Rows[0]["TotalSubletAmount"]).ToString("0.00");

                txtTotalPartAmount.Style.Add("display", "none");
                txtTotalLabourAmount.Style.Add("display", "none");
                txtTotalLubricantAmount.Style.Add("display", "none");
                txtTotalSubletAmount.Style.Add("display", "none");

                //VHP WC Start
                //if (txtDomestic_Export.Text == "D")
                //{
                //drpDefectCode.Enabled = false;
                //drpCulpritCode.Enabled = false;
                //drpTechnicalCode.Enabled = false;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Warratable"]) == "N" && ds.Tables[1].Rows.Count > 0)
                        drpTechnicalCode.Enabled = true;
                    else
                        drpTechnicalCode.Enabled = false;
                }
                txtPartNo.Enabled = false;
                //}
                if (Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Claim_Status"]) != "1" && (iUserRoleId != 9 && iUserRoleId != 10 && iUserRoleId != 1 && iUserRoleId != 2 && iUserRoleId != 3 && iUserRoleId != 4))
                {

                    btnSave.Enabled = false;
                    PartDetailsGrid.Enabled = false;
                    LabourDetailsGrid.Enabled = false;
                    LubricantDetailsGrid.Enabled = false;
                    SubletDetailsGrid.Enabled = false;
                    PJobDetails.Enabled = false;
                    txtPartNo.Enabled = false;
                    //lblRequestNo.Style.Add("display", "none");
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
            txtClaimDate.ReadOnly = true;
            txtModelCode.ReadOnly = true;
            txtModelDescription.ReadOnly = true;


            txtChassisNo.ReadOnly = true;
            txtEngineNo.ReadOnly = true;
            txtVehicleNo.ReadOnly = true;



            txtPartAmount.Attributes.Add("readOnly", "true");
            txtLabourAmount.Attributes.Add("readOnly", "true");
            txtLubricantAmount.Attributes.Add("readOnly", "true");
            txtSubletAmount.Attributes.Add("readOnly", "true");
            txtAccClaimAmt.Attributes.Add("readOnly", "true");

            txtOrgPartAmount.Attributes.Add("readOnly", "true");
            txtOrgLabourAmount.Attributes.Add("readOnly", "true");
            txtOrgLubricantAmount.Attributes.Add("readOnly", "true");
            txtOrgSubletAmount.Attributes.Add("readOnly", "true");
            txtOrgAccClaimAmt.Attributes.Add("readOnly", "true");

            chkCulpritInfo.Checked = false;
            chkDefectInfo.Checked = false;

            if (chkCulpritInfo.Checked == false)
            {
                drpCulpritCode.Style.Add("display", "");
                drpCulpritCodeTemp.Style.Add("display", "none");
            }
            else if (chkCulpritInfo.Checked == true)
            {
                drpCulpritCode.Style.Add("display", "none");
                drpCulpritCodeTemp.Style.Add("display", "");
            }

            if (chkDefectInfo.Checked == false)
            {
                drpDefectCode.Style.Add("display", "");
                drpDefectCodeTemp.Style.Add("display", "none");
            }
            else if (chkDefectInfo.Checked == true)
            {
                drpDefectCode.Style.Add("display", "none");
                drpDefectCodeTemp.Style.Add("display", "");
            }

            chkCulpritInfo.Visible = false;
            chkDefectInfo.Visible = false;

            if (iUserRoleId == 18)
                PVehicleDetails.Enabled = false;
            else
                PVehicleDetails.Enabled = bEnable;

            lblAddPart.Enabled = bEnable;
            PartDetailsGrid.Enabled = bEnable;
            LabourDetailsGrid.Enabled = bEnable;
            LubricantDetailsGrid.Enabled = bEnable;
            SubletDetailsGrid.Enabled = bEnable;
            //if (sRequestOrClaim == "C" && txtRequestNo.Text != "" && iUserRoleId == 4)
            //{
            //    btnSave.Enabled = true;
            //    PJobDetails.Enabled = true;
            //}
            //else
            //{
            //    btnSave.Enabled = bEnable;
            //    PJobDetails.Enabled = bEnable;
            //}
            btnSave.Enabled = bEnable;
            PJobDetails.Enabled = bEnable;
            //Megha   Export jobdetails. As per discussion with DeeptiMadam 
            if (txtDomestic_Export.Text == "E")
            {
                PTax1.Style.Add("display", "none");
                LTax1.Style.Add("display", "none");
                LTTax1.Style.Add("display", "none");
                lblLaborTaxable.Text = "<b>Labor:</b>";
                lblLaborTaxable1.Text = "<b>Labor:</b>";

            }
            else
            {
                PTax1.Style.Add("display", "none");
                lblOrgLabourST.Text = (hdnInvType.Value == "N") ? "<b>Tax(Labor + Sublet): </b>" : (hdnInvType.Value == "P") ? "<b>Tax(Part + Lubricant): </b>" : "<b>Tax(Labor + Sublet): </b>";
                lblAccLabourST.Text = (hdnInvType.Value == "N") ? "<b>Tax(Labor + Sublet): </b>" : (hdnInvType.Value == "N") ? "<b>Tax(Part + Lubricant): </b>" : "<b>Tax(Labor + Sublet): </b>";
            }
            //Megha Export jobdetails
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
            txtOrgAccClaimAmt.Text = "";
            txtOrgLabourST.Text = "";
            txtAccLabourST.Text = "";
            GetDataAndDisplay();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //if (ViewState["Claim_Status"] != null)
            //    btnBack.Attributes.Add("onClick", "return GetJobDetails('" + Convert.ToString(ViewState["Claim_Status"]) + "');");

        }
        protected void rdoDeductionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sRequestOrClaim != "R")
            {
                if (rdoDeductionType.SelectedValue == "L")
                {
                    txtPartHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtLabourHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtLubricantHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    txtSubletHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                }
                else
                {
                    txtPartHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                    txtLabourHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                    txtLubricantHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
                    txtSubletHeaderDeduction.Attributes.Add("onkeydown", "return ToSetKeyPressValueTrue(event,this)");

                    //txtPartHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    //txtLabourHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    //txtLubricantHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");
                    //txtSubletHeaderDeduction.Attributes.Add("onkeypress", "return CheckPercentageAmount(event,this)");

                    txtPartHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                    txtLabourHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                    txtLubricantHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");
                    txtSubletHeaderDeduction.Attributes.Add("onblur", "return CheckPercentageValue(event,this)");

                    txtPartHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToParts(event,this)");
                    txtLabourHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToLabour(event,this)");
                    txtLubricantHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToLubricant(event,this)");
                    txtSubletHeaderDeduction.Attributes.Add("onblur", "return AddDeductionPercentageToSublet(event,this)");

                }
            }

            if (rdoDeductionType.SelectedValue == "L")
            {
                iUserChange = 1;
                clsWarranty ObjWarranty = new clsWarranty();
                if (HdnClaimType.Value != "")
                    ValenmFormUsedFor = ObjWarranty.GetEnmClaimType(HdnClaimType.Value);

                SetControlPropertyToPartGrid();
                SetControlPropertyToLabourGrid();
                SetControlPropertyToLubricantGrid();
                SetControlPropertyToSubletGrid();
            }
            else
            {
                iUserChange = 1;
                clsWarranty ObjWarranty = new clsWarranty();
                if (HdnClaimType.Value != "")
                    ValenmFormUsedFor = ObjWarranty.GetEnmClaimType(HdnClaimType.Value);

                SetControlPropertyToPartGrid();
                SetControlPropertyToLabourGrid();
                SetControlPropertyToLubricantGrid();
                SetControlPropertyToSubletGrid();
            }
        }
    }
}