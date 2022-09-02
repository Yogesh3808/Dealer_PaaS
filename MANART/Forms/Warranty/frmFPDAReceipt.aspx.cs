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
using System.Drawing;
using System.IO;

namespace MANART.Forms.Warranty
{
    public partial class frmFPDAReceipt : System.Web.UI.Page  
    {
        private DataTable dtDetails = new DataTable();    
        private int iDealerId = 0;        
        private int iFPDAHDRID = 0;        
        private string Display = "";
        private int ichassisID = 0;
        private DataTable dtComplaint = new DataTable();               

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                //txtDealerCode.Text = Func.Convert.sConvertToString(Session["sDealerCode"]);
                txtUserType.Text = Session["UserType"].ToString();                
                if (!IsPostBack)
                {
                    lblTitle.Text = "FPDA Receipt Details";
                    DisplayRecord();
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDtl = new DataTable();
                ClsFPDA ObjFPDA = new ClsFPDA();

                if (bValidateRecord() == false)
                {
                    
                }
                else
                {
                    dtDtl = UpdateHdrValueFromControl();

                    if (ObjFPDA.bSaveFPDAReceipt(txtID.Text, dtDtl) == true)
                    {
                        if (ObjFPDA.bSaveFPDAReceiptConfirm(txtID.Text, "P", txtReceiptRemark.Text, txtRecptNo.Text, txtRecpDate.Text, txtDealerCode.Text, Func.Convert.iConvertToInt(txtDealerID.Text), hdnReceiptStatus.Value.ToString()) == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('FPDA " + txtDocNo.Text + " Get Received successfully.')</script>");
                            GetDataAndDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private bool bValidateRecord()
        {
            string sMessage = " Please enter/select records.";
            bool bValidateRecord = true;


            for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
            {
                string sReceiptDate = "";
                string sClaimNo = "";
                string sPartNo = "";

                sClaimNo = (DetailsGrid.Rows[iRowCnt].FindControl("lblCliam_No") as Label).Text;
                sPartNo = (DetailsGrid.Rows[iRowCnt].FindControl("lblPart_Name") as Label).Text;    

                object objShipmentDate = DetailsGrid.Rows[iRowCnt].FindControl("txtReceiptDate");
                sReceiptDate = (objShipmentDate as MANART.WebParts.CurrentDate).Text;
                if((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true && sReceiptDate == "")
                {
                    sMessage = "Please enter the Received Date For Part " + sPartNo + " In Claim No " + sClaimNo;
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                    (objShipmentDate as MANART.WebParts.CurrentDate).Enabled = true;
                    bValidateRecord = false;
                    break;
                }
            }
            
            if (bValidateRecord == false)
            {
                //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            //Validation For Model Ramarks
            return bValidateRecord;
        }

        public void DisplayRecord()
        {
            try
            {
                
                
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iFPDAHDRID = Func.Convert.iConvertToInt(Request.QueryString["FPDAHDRID"]);
                txtID.Text = Func.Convert.sConvertToString(iFPDAHDRID);
                GetDataAndDisplay();
               
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
                
        private void GetDataAndDisplay()
        {
            
            DataSet ds = new DataSet();
            // 'Replace Func.DB to objDB by Shyamal on 05042012          
            try
            {   
                iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"]);
                iFPDAHDRID = Func.Convert.iConvertToInt(txtID.Text);
                ClsFPDA ObjFPDA = new ClsFPDA();               
                if (iFPDAHDRID != 0)
                {
                    ds = ObjFPDA.GetFPDA("All", Func.Convert.sConvertToString(iFPDAHDRID), iDealerId);
                    DisplayData(ds);
                    ObjFPDA = null;
                }               
                ds = null;
                ObjFPDA = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
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
                Display = Func.Convert.sConvertToString(Request.QueryString["Display"]);

                txtDocNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Advice_no"]);
                txtDocDate.Text = Func.Convert.tConvertToDate(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Advice_Date"]), false); 
                txtTransporter.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Transporter"]);
                txtNoOfCases.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["NoOfCases"]);
                txtLRNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR"]);
                txtLRDate.Text = Func.Convert.tConvertToDate(Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["LR_Date"]), false);
                txtRemarks.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Remarks"]);
                txtReceiptRemark.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReceiptRemark"]);
                
                txtRecpDate.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReceiptDate"]);
                hdnReceiptStatus.Value = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReceiptConfirm"]);

                txtRecptNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ReceiptNo"]);
                
                txtDealerID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_ID"]);
                txtDealerCode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Dealer_Spares_Code"]);                
                btnApprove.Visible = (hdnReceiptStatus.Value != "P") ? false : true;

                //if (hdnReceiptStatus.Value.ToString() == "N") txtRecptNo.Text = Func.Common.sGetMaxDocNo(txtDealerCode.Text, "", "FR", Func.Convert.iConvertToInt(txtDealerID.Text));
                if (hdnReceiptStatus.Value.ToString() == "N") txtRecptNo.Text = Func.Common.sGetMaxDocNo("D009999", "", "FR", 9999); 

                lblTitle.Text = "FPDA No: " + txtDocNo.Text;
                                
                btnSave.Visible = (hdnReceiptStatus.Value == "Y") ? false : true;                

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DetailsGrid.DataSource = ds.Tables[1];
                    DetailsGrid.DataBind();
                    Session["FPDAClaimsParts"] = ds.Tables[1];
                }
                else
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
                SetControlPropertyToDetailsGrid();
                MakeEnableDisableControls();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void SetControlPropertyToDetailsGrid()
        {
            try
            {
                if (DetailsGrid.Rows.Count == 0) return;

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {
                    //object objShipmentDate = DetailsGrid.Rows[iRowCnt].FindControl("txtReceiptDate");
                    //(objShipmentDate as MANART.WebParts.CurrentDate).Enabled = ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true) ? true : false;                                        
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        // To enable or disable fields
        private void MakeEnableDisableControls()
        {
           bool bEnable = false; 
                    

               Display = Func.Convert.sConvertToString(Request.QueryString["Display"]);
               if (Display == "Y") bEnable = false;

               txtTransporter.Enabled = bEnable;
               txtNoOfCases.Enabled = bEnable;
               txtLRNo.Enabled = bEnable;
               txtLRDate.Enabled = bEnable;
               txtRemarks.Enabled = bEnable;              
                
        }


        protected void lnkSelectPart_Click(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private DataTable UpdateHdrValueFromControl()
        {
            try
            {
                DataRow dr;
                dtDetails = new DataTable();
                dtDetails.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("Cliam_No", typeof(string)));                
                dtDetails.Columns.Add(new DataColumn("Part_Name", typeof(string)));                
                dtDetails.Columns.Add(new DataColumn("RecvdYN", typeof(bool)));                
                dtDetails.Columns.Add(new DataColumn("RecvdRemark", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("RecvdDate", typeof(string)));                

                for (int iRowCnt = 0; iRowCnt < DetailsGrid.Rows.Count; iRowCnt++)
                {
                    dr = dtDetails.NewRow();
                   
                    dr["Customer_Name"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblCustomer") as Label).Text;
                    dr["Cliam_No"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblCliam_No") as Label).Text;                   
                    dr["Part_Name"] = (DetailsGrid.Rows[iRowCnt].FindControl("lblPart_Name") as Label).Text;                    
                    if ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true)
                    {
                        dr["RecvdYN"] = 1;
                    }
                    else
                    {
                        dr["RecvdYN"] = 0;
                    }
                    dr["RecvdRemark"] = Func.Convert.sConvertToString((DetailsGrid.Rows[iRowCnt].FindControl("txtRecvdRemark") as TextBox).Text);
                    string sReceiptDate = "";
                    object objShipmentDate = DetailsGrid.Rows[iRowCnt].FindControl("txtReceiptDate");
                    sReceiptDate = (objShipmentDate as MANART.WebParts.CurrentDate).Text;
                    dr["RecvdDate"] = ((DetailsGrid.Rows[iRowCnt].FindControl("ChkForAccept") as CheckBox).Checked == true) ? sReceiptDate : "";
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                   
                }
                return dtDetails;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                throw;
            }
        }
        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.CheckBox)(e.CommandSource)).ID == "ChkForAccept")
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please enter the Job details.');</script>");
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDtl = new DataTable();
                ClsFPDA ObjFPDA = new ClsFPDA();
                if (bValidateRecord() == false)
                {

                }
                else
                {
                    dtDtl = UpdateHdrValueFromControl();

                    if (ObjFPDA.bSaveFPDAReceipt(txtID.Text, dtDtl) == true)
                    {
                        if (ObjFPDA.bSaveFPDAReceiptConfirm(txtID.Text, "Y", txtReceiptRemark.Text, txtRecptNo.Text, txtRecpDate.Text, txtDealerCode.Text, Func.Convert.iConvertToInt(txtDealerID.Text), hdnReceiptStatus.Value.ToString()) == true)
                        {
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Receipt confirmed successfully For FPDA " + txtDocNo.Text + ".')</script>");
                            GetDataAndDisplay();
                        }
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