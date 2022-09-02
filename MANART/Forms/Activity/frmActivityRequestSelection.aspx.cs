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

namespace MANART.Forms.Activity
{
    public partial class frmActivityRequestSelection : System.Web.UI.Page
    {
        string sUserType = "";
        int iMenuId = 0;
        int iDeptId = 0;
        string sDealerIds = "";
        int iUserId = 0;
        int DeptId = 0;
        //Megha 28072015
        int iUserRoleId = 0;
        int UserDeptId = 0;

        //Megha 28072015
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ClientScript.RegisterClientScriptInclude(this.GetType(), "myScript", "../../CSS/jquery.datepick.css");
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                if (iMenuId == 74)
                {
                   // DeptId = 1;
                    txtApprProcc.Text = "Approval";
                }
                else if (iMenuId == 75)
                {
                   // DeptId = 1;
                    txtApprProcc.Text = "Processing";
                }
               
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                //Megha 28072015
                iUserRoleId = Func.Convert.iConvertToInt(Session["UserRole"]);

                UserDeptId=Func.Convert.iConvertToInt(Session["DepartmentID"]);

                txtUserDeptId.Text = UserDeptId.ToString(); 


                txtUserRoleID.Text = iUserRoleId.ToString();
               
                //Megha 28072015
                sUserType = Session["UserType"].ToString();
                // Megha 08072011
              
                //if (DeptId == 1 )
                //    Location.bUseSpareDealerCode = true;
                //else
                //    Location.bUseSpareDealerCode = false;
                Location.bUseSpareDealerCode = true;
           
                if (Location.sDealerId == "")
                    sDealerIds = Func.Common.GetDealersByUserID(iUserId);
                else
                    sDealerIds = Location.sDealerId;

                if ((iUserRoleId == 2 || iUserRoleId == 3) && UserDeptId==7)
                {
                    DeptId = 2;
                }
                else if((iUserRoleId == 2) && UserDeptId==6)
                {
                    DeptId = 1;
                }

               

                //lblTitle.Text = Activity.GetActivitylblName(iMenuId, "Title");
                //lblActStatus.Text = Activity.GetActivitylblName(iMenuId, "Status");
                //txtApprProcc.Text = Activity.GetActivitylblName(iMenuId, "ApprProcc");


                if (txtSearchText.Text != "")
                    btnClearSearch.Style.Add("display", "");
                else
                    btnClearSearch.Style.Add("display", "none");

                if (drpActivityCalimStatus.SelectedValue == "P")
                    trDate.Style.Add("display", "none");
                else
                {
                    trDate.Style.Add("display", "");
                    //txtFromDate.sOnBlurScript = " return CheckDateLess(ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate,ctl00_ContentPlaceHolder1_txtToDate_txtDocDate,'From Date should be Less than To Date');";
                    //txtToDate.sOnBlurScript = " return CheckDateGreater(ctl00_ContentPlaceHolder1_txtToDate_txtDocDate,ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate,'To Date should be Greater than From Date');";
                }
                // if (!IsPostBack)
                // {

                FillActivityRequest();
                //  }        
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtDetails = null;
                dtDetails = (ViewState["dtDetails"] != null) ? (DataTable)ViewState["dtDetails"] : null;
                ActivityClaim.DataSource = dtDetails;
                ActivityClaim.DataBind();
                //drpActivityCalimStatus.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            FillActivityRequest();
        }
        private void FillActivityRequest()
        {
            try
            {
                string sSelect = "";
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                if (txtSearchText.Text == "")
                {
                    sSelect = drpActivityCalimStatus.SelectedValue.ToString();
                }
                else
                {
                    sSelect = "0";
                }
                DataSet ds = new DataSet();
                DataTable dtDetails = null;
                clsActivityHeads objActivity = new clsActivityHeads();

                string sCurrDate = Func.Common.sGetCurrentDate();
                if (drpActivityCalimStatus.SelectedValue != "P")
                {
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = "01/01/" + DateTime.Now.Year;//Func.Common.sGetMonthStartDate(sCurrDate);
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = "31/12/9999";//Func.Common.sGetMonthEndDate(sCurrDate);
                    }
                }
                else
                {
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = "01/01/2017"; //Func.Common.sGetMonthStartDate(sCurrDate);
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = "31/12/9999";//Func.Common.sGetMonthEndDate(sCurrDate);
                    }
                }

                if (iMenuId == 74)
                {
                    ds = objActivity.GetActivityForProcessing(iUserId, sDealerIds, "ApprovalActivity", sSelect, txtFromDate.Text, txtToDate.Text, DeptId);
                }
                else if (iMenuId == 75)
                {
                    ds = objActivity.GetActivityForProcessing(iUserId, sDealerIds, "ProcessingActivity", sSelect, txtFromDate.Text, txtToDate.Text, DeptId);
                    

                }
                
               
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dtDetails = ds.Tables[0];

                          

                            DataView dvDetails = new DataView();
                            dvDetails = dtDetails.DefaultView;
                            if (drpActivity.SelectedValue == "1" && txtSearchText.Text != "")
                                dvDetails.RowFilter = "Activity_Req_No Like '" + txtSearchText.Text + "'";// AND Activity_Req_Date>'" + dtStart.ToString("dd/MM/yyyy") + "'" + "AND Activity_Req_Date<'" + dtEnd.ToString("dd/MM/yyyy") + "'";
                            else if (drpActivity.SelectedValue == "2" && txtSearchText.Text != "")
                                dvDetails.RowFilter = "Activity_Name Like '" + txtSearchText.Text + "'";//AND Activity_Req_Date>'" + dtStart.ToString("dd/MM/yyyy") + "'" + "AND Activity_Req_Date<'" + dtEnd.ToString("dd/MM/yyyy") + "'";

                            if (txtSearchText.Text == "")
                            {
                                //Pending 
                                if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 1 && UserDeptId == 7)
                                {
                                    dvDetails.RowFilter = "Status Like 'Pending At Aftersales Head'";

                                }
                                if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 1 && UserDeptId==6)
                                {
                                    dvDetails.RowFilter = "Status Like 'Pending At Parts Head'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 2 && DeptId==1)
                                {
                                    dvDetails.RowFilter = "Status Like 'Pending At RPM'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 2 && DeptId == 2)
                                {
                                    dvDetails.RowFilter = "Status Like 'Pending At RSM'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 3 && DeptId == 2)
                                {
                                    dvDetails.RowFilter = "Status Like 'Pending at ASM'";

                                }
                                //else if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 4)
                                //{
                                //    dvDetails.RowFilter = "Status Like 'Pending at CSM'";
                                //}
                                //else if (drpActivityCalimStatus.SelectedValue == "P" && iUserRoleId == 9)
                                //{
                                //    dvDetails.RowFilter = "Status Like 'Pending at Finance' OR Status Like 'Documents Awaited'";
                                //}
                                //Approved
                                else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 1 && UserDeptId == 7)
                                {
                                    dvDetails.RowFilter = "Status Like 'Approved by Aftersales Head'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 1 && UserDeptId == 6)
                                {
                                    dvDetails.RowFilter = "Status Like 'Approved by Parts Head'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 2 && DeptId == 1)
                                {
                                    dvDetails.RowFilter = "Status Like 'Approved by RPM";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 2 &&  DeptId== 2)
                                {
                                    dvDetails.RowFilter = "Status Like 'Approved by RSM";
                                }
                               //Return
                                else if (drpActivityCalimStatus.SelectedValue == "R" && iUserRoleId == 1 && UserDeptId == 7) 
                                {
                                    dvDetails.RowFilter = "Status Like 'Aftersales Head Return Claim for Modification' OR Status Like 'Aftersales Head Return Request for Modification'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "R" && iUserRoleId == 1 && UserDeptId == 6)
                                {
                                  dvDetails.RowFilter = "Status Like 'Parts Head Return Claim for Modification' OR Status Like 'Parts Head Return Request for Modification'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "R" && iUserRoleId == 2 && DeptId == 1)
                                {
                                    dvDetails.RowFilter = "Status Like 'RPM Return Claim for Modification' OR Status Like 'RPM Return Request for Modification'";
                                   
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "R" && iUserRoleId == 2 && DeptId == 2)
                                {
                                    dvDetails.RowFilter = "Status Like 'RSM Return Claim for Modification' OR Status Like 'RSM Return Request for Modification'";

                                }
                                else if (drpActivityCalimStatus.SelectedValue == "R" && iUserRoleId == 3)
                                {
                                    dvDetails.RowFilter = "Status Like 'ASM Return Claim for Modification' OR Status Like 'ASM Return Request for Modification'";

                                }

                                //else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 1)
                                //{
                                //    dvDetails.RowFilter = "Status Like 'Approved by HEAD'";
                                //}
                               
                                //else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 3)
                                //{
                                //    dvDetails.RowFilter = "Status Like 'Approved by ASM'";
                                //}
                                //else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 4)
                                //{
                                //    dvDetails.RowFilter = "Status Like 'Approved by CSM'";
                                //}
                                else if (drpActivityCalimStatus.SelectedValue == "Y" && iUserRoleId == 9)
                                {
                                    // dvDetails.RowFilter = "Status Like 'Approved by Finance'";
                                    dvDetails.RowFilter = "Status Like 'Approved by Finance' OR Status Like 'Approved by Finance with Exception'";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "D" && iUserRoleId == 9)
                                {
                                    // dvDetails.RowFilter = "Status Like 'Approved by Finance'";
                                    dvDetails.RowFilter = "Status Like 'GST Invoice Awaited'";
                                }
                                //rejected
                                else if (drpActivityCalimStatus.SelectedValue == "J" && iUserRoleId == 1)
                                {
                                    dvDetails.RowFilter = "Status Like 'Rejected by HEAD'";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "J" && iUserRoleId == 2)
                                {
                                    // dvDetails.RowFilter = "Status Like 'Pending at Finance' OR Status Like 'Documents Awaited'";
                                    dvDetails.RowFilter = "Status Like 'Rejected by RSM' OR Status Like 'Return by RSM to Dealer for Modification'";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "J" && iUserRoleId == 3)
                                {
                                    dvDetails.RowFilter = "Status Like 'Rejected by ASM' OR Status Like 'Return by ASM to Dealer for Modification'";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "J" && iUserRoleId == 4)
                                {
                                    dvDetails.RowFilter = "Status Like 'Rejected by CSM' OR Status Like 'Return by CSM to Dealer for Modification'";
                                }
                                else if (drpActivityCalimStatus.SelectedValue == "J" && iUserRoleId == 9)
                                {
                                    dvDetails.RowFilter = "Status Like 'Rejected by Finance' OR Status Like 'Return by Finance to Dealer for Modification'";
                                }
                            }

                            ActivityClaim.DataSource = dvDetails.ToTable();
                            ActivityClaim.DataBind();
                            ViewState["dtDetails"] = dvDetails.ToTable();
                            objActivity = null;
                            txtFlage.Text = sSelect;
                            lblMessage.Text = "";
                        }
                        else
                        {
                            ActivityClaim.DataSource = null;
                            ActivityClaim.DataBind();
                            lblMessage.Text = "Activity Does Not Exist!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillActivityRequest();
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            FillActivityRequest();
        }

        protected void ActivityClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ActivityClaim.PageIndex = e.NewPageIndex;
            FillActivityRequest();

        }

    }
}