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

namespace MANART.Forms.Admin
{
    public partial class frmSignup : System.Web.UI.Page
    {
        clsDB objDB = new clsDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                btnSignup.Enabled = false;
            }
            if (!IsPostBack)
            {
                txtLoginName.Attributes.Add("readonly", "readonly");
                txtAvailCheck.Text = "";
                //lblID.Style["display"] = "none";
                //OptionUserType.Items[0].Selected = true;
                FillCombo();
                //PanelLoginDetails.Style["visibility"] = "hidden";  
                if (TextBox_number.Text != "")
                {
                    string number_server_side = (string)Session[clsCaptcha.SESSION_CAPTCHA];
                    if (number_server_side != TextBox_number.Text)
                    {
                        lblCaptcha.Text = "The characters you entered didn't match the word verification. Please try again.<br/>";
                        TextBox_number.Text = "";
                        TextBox_number.Focus();
                    }
                    else
                    {

                    }

                }
            }
            //if (OptionUserType.Items[0].Selected == true)
            //    tdEmpCode.Style.Add("display", "none");
            //else
            //    tdEmpCode.Style.Add("display", "");
        }

        protected void btnCaptchaChange_Click(object sender, ImageClickEventArgs e)
        {

        }

        private void ClearForm()
        {
            txtLoginName.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtAvailCheck.Text = "";
            txtID.Text = "";
            lblUserRegitrationFor.Text = "";
            TextBox_number.Text = "";
            txtEmpCode.Text = "";
            drpDept.Items.Clear();
            drpDept.SelectedValue = "0";

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ClearForm();
            Response.Redirect("~/Default.aspx");
        }
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            if (bIsUserAvail() == true)
            {
                if (TextBox_number.Text != "")
                {
                    string number_server_side = (string)Session[clsCaptcha.SESSION_CAPTCHA];
                    if (number_server_side != TextBox_number.Text)
                    {
                        lblCaptcha.Text = "The characters you entered didn't match the word verification. Please try again.<br/>";
                        TextBox_number.Text = "";
                        TextBox_number.Focus();
                    }
                    else
                    {
                        if (SaveUser())
                        {
                            Server.Transfer("~/Forms/Admin/frmConfirm.aspx?UserName=" + txtFirstName.Text + " " + txtLastName.Text);
                        }
                    }
                }
            }
            else
                return;

        }

        private void UpdateValueFromControl(DataTable dtHdr)
        {
            try
            {
                string cntPartID = "";
                string sEmpCode = "";
                 int iDeptID = 0;
                DataRow dr;
                //Get Header InFormation        
                dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("LoginName", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Password", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("FirstName", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("LastName", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("Email", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("EmpCode", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("UserType", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserRole", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserDepartment", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("UserBasicModelCatIDs", typeof(string)));
                dtHdr.Columns.Add(new DataColumn("HOBR_HdrID", typeof(int)));
                dtHdr.Columns.Add(new DataColumn("DocNo", typeof(string)));                

                if (drpUserType.SelectedValue.ToString() == "3" || drpUserType.SelectedValue.ToString() == "4")
                {
                    for (int iMainCnt = 0; iMainCnt < chkDept.Items.Count; iMainCnt++)
                    {
                        for (int iSubCnt = 1; iSubCnt < 4; iSubCnt++)
                        {
                            if (chkDept.Items[iMainCnt].Selected == true && chkDept.Items[iMainCnt].Enabled == true)
                            {
                                if (chkDept.Items[iMainCnt].Text.ToUpper().Contains("SALES") == true)
                                {
                                    txtLoginName.Text = hdnSalesCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                                    sEmpCode = hdnSalesCode.Value.ToUpper();
                                    iDeptID = Func.Convert.iConvertToInt(chkDept.Items[iMainCnt].Value);
                                }
                                else
                                {
                                    txtLoginName.Text = hdnSpareCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                                    sEmpCode = hdnSpareCode.Value.ToUpper();
                                    iDeptID = Func.Convert.iConvertToInt(chkDept.Items[iMainCnt].Value);
                                }
                                dr = dtHdr.NewRow();
                                dr["ID"] = 0;
                                dr["UserID"] = (Session["UserID"] != null) ? (Func.Convert.iConvertToInt(Session["UserID"])) : 0;
                                dr["LoginName"] = txtLoginName.Text;
                                dr["Password"] = "Secure123*";
                                dr["FirstName"] = txtFirstName.Text;
                                dr["LastName"] = txtLastName.Text;
                                dr["Email"] = txtEmail.Text;
                                dr["EmpCode"] = sEmpCode;
                                dr["UserType"] = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
                                dr["UserRole"] = Func.Convert.iConvertToInt(drpLevels.SelectedValue);
                                // dr["UserDepartment"] = Func.Convert.iConvertToInt(drpDept.SelectedValue);
                                dr["UserDepartment"] = iDeptID;
                                dr["UserBasicModelCatIDs"] = "";
                                dr["HOBR_HdrID"] = Func.Convert.iConvertToInt(hdnHOBranchID.Value);
                                //dr["DocNo"] = drpDlrLocation.SelectedItem.Text;
                                dr["DocNo"] = Func.Convert.sConvertToString(hdnHOBranch.Value); 
                                dtHdr.Rows.Add(dr);
                                dtHdr.AcceptChanges();
                            }
                        }
                    }
                }
                else
                {
                    string UserBasicModelCatIDs = "";
                    foreach (ListItem LID in chkModelCategory.Items)
                    {
                        LID.Selected = true; 
                        if (LID.Selected)
                            if (UserBasicModelCatIDs == "")
                                UserBasicModelCatIDs = LID.Value.ToString();
                            else
                                UserBasicModelCatIDs = UserBasicModelCatIDs + "," + LID.Value.ToString();
                    }
                    dr = dtHdr.NewRow();
                    dr["ID"] = 0;
                    dr["UserID"] = (Session["UserID"] != null) ? (Func.Convert.iConvertToInt(Session["UserID"])) : 0;
                    dr["LoginName"] = txtLoginName.Text;
                    dr["Password"] = "Secure123*";
                    dr["FirstName"] = txtFirstName.Text;
                    dr["LastName"] = txtLastName.Text;
                    dr["Email"] = txtEmail.Text;
                    dr["EmpCode"] = txtEmpCode.Text.ToUpper();
                    dr["UserType"] = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
                    dr["UserRole"] = Func.Convert.iConvertToInt(drpLevels.SelectedValue);
                    dr["UserDepartment"] = Func.Convert.iConvertToInt(drpDept.SelectedValue);
                    dr["UserBasicModelCatIDs"] = Func.Convert.sConvertToString(UserBasicModelCatIDs);
                    dr["HOBR_HdrID"] = 0;
                    dr["DocNo"] = "";
                    dtHdr.Rows.Add(dr);
                    dtHdr.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private bool SaveUser()
        {
            clsUser objUser = new clsUser();
            DataTable dtHdr = null;
            try
            {
                dtHdr = new DataTable();
                UpdateValueFromControl(dtHdr);
                return objUser.AddUser(dtHdr);
            }
            catch (Exception ex)
            {
                return false;
            }

            //int iID = 0;
            //int iUsrID = 0;
            //string sLoginName = "";
            //string sFirstName = "";
            //string sLastName = "";
            //string sPassword = "";
            //string sEmail = "";
            //string sEmpCode = "";
            //int iUserType = 0;
            //int iUserRole = 0;
            //int iUserDepartment = 0;

            //iUsrID = Func.Convert.iConvertToInt(txtID.Text);
            //sLoginName = txtLoginName.Text;
            //sFirstName = txtFirstName.Text;
            //sLastName = txtLastName.Text;
            ////sPassword = "Secure123*";
            //sEmail = txtEmail.Text;
            //sEmpCode = txtEmpCode.Text;
            //iUserType = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
            //iUserRole = Func.Convert.iConvertToInt(drpLevels.SelectedValue);
            //iUserDepartment = Func.Convert.iConvertToInt(drpDept.SelectedValue);
            //if (drpUserType.SelectedValue.ToString() == "3" || drpUserType.SelectedValue.ToString() == "4")
            //{
            //    for (int iMainCnt = 0; iMainCnt < chkDept.Items.Count; iMainCnt++)
            //    {
            //        if (chkDept.Items[iMainCnt].Selected == true)
            //        {
            //            if (chkDept.Items[iMainCnt].Text.ToUpper().Contains("VEHICLE") == true)
            //            {                       
            //                    txtLoginName.Text = hdnSalesCode.Value + chkDept.Items[iMainCnt].Text.ToUpper();
            //                    sEmpCode = hdnSalesCode.Value;
            //            }
            //            else
            //            {                      
            //                    txtLoginName.Text = hdnSpareCode.Value + chkDept.Items[iMainCnt].Text.ToUpper();
            //                    sEmpCode = hdnSpareCode.Value;
            //            }
            //        }
            //        return objUser.AddUser(iID, iUsrID, sLoginName, sPassword, sFirstName, sLastName, sEmail, sEmpCode, iUserType, iUserRole, iUserDepartment);
            //    }

            //}
            //else
            //{
            //    return objUser.AddUser(iID, iUsrID, sLoginName, sPassword, sFirstName, sLastName, sEmail, sEmpCode, iUserType, iUserRole, iUserDepartment);
            //}



        }

        protected void btnAvailability_Click(object sender, ImageClickEventArgs e)
        {
            clsUser objectUser = new clsUser();
            int iUsrID = Func.Convert.iConvertToInt(txtID.Text);
            int flage = objectUser.UserValiadation(iUsrID, txtLoginName.Text, "avialable");
            if (flage == 0)
            {
                lblavail.Text = "User Not available!";
                lblavail.ForeColor = System.Drawing.Color.Red;

                txtLoginName.Text = "";
                txtLoginName.Focus();
                txtAvailCheck.Text = "0";
            }
            else if (flage == 1)
            {
                lblavail.Text = "User Avialable";
                lblavail.ForeColor = System.Drawing.Color.Green;
                txtAvailCheck.Text = "1";
                //txtPassword.Focus();
            }
            else if (flage == 2)
            {
                lblavail.Text = "Maximum 3 Users Avialable";
                lblavail.ForeColor = System.Drawing.Color.Red;
                txtAvailCheck.Text = "2";
                //txtPassword.Focus();
            }
        }

        private bool bIsUserAvail()
        {
            bool bIsAvail = true;
            clsUser objectUser = new clsUser();
            int iUsrID = Func.Convert.iConvertToInt(txtID.Text);
            int flage = objectUser.UserValiadation(iUsrID, txtLoginName.Text, "avialable");
            if (flage == 0)
            {
                lblavail.Text = "User Not avialable!";
                lblavail.ForeColor = System.Drawing.Color.Red;

                txtLoginName.Text = "";
                txtLoginName.Focus();
                txtAvailCheck.Text = "0";
                bIsAvail = false;
            }
            else if (flage == 1)
            {
                lblavail.Text = "User Avialable";
                lblavail.ForeColor = System.Drawing.Color.Green;
                txtAvailCheck.Text = "1";
                //txtPassword.Focus();
                bIsAvail = true;
            }
            else if (flage == 2)
            {
                lblavail.Text = "Maximum 3 Users Avialable";
                lblavail.ForeColor = System.Drawing.Color.Red;
                txtAvailCheck.Text = "2";
                //txtPassword.Focus();
                bIsAvail = false;
            }
            return bIsAvail;
        }

        //protected void OptionUserType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (OptionUserType.SelectedValue == "E")
        //        lblSearch.Visible = false;
        //    else
        //        lblSearch.Visible = true;
        //    ClearForm();
        //    FillCombo(Func.Convert.sConvertToString(OptionUserType.SelectedValue)); 
        //}
        private void FillCombo()
        {
            Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0);
            if (drpUserType.Items.Count > 1 && (Func.Convert.iConvertToInt(Session["UserID"])) != 8)
            {
                if (drpUserType.Items.FindByText("ADMIN") != null)
                    drpUserType.Items.Remove(new ListItem("ADMIN", "5"));
            }

            drpDealer.Items.Insert(0, new ListItem("--Select--", "0"));
            //drpDept.Items.Insert(0, new ListItem("--Select--", "0"));        
            drpLevels.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void drpUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

            clsUser objectUser = null;
            DataTable dtUserInfo = null;
            try
            {

                hdnSalesCode.Value = "";
                hdnSpareCode.Value = "";
                hdnHOBranchCode.Value = "";
                txtEmail.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtLoginName.Text = "";
                txtEmpCode.Text = "";

                txtEmpCode.Style.Add("display", "");
                lblEmpCode1.Style.Add("display", "");
                //BranchReportchnages Added by megha
                //if (drpUserType.SelectedValue == "3" || drpUserType.SelectedValue == "4")
                //02042018
                //if (drpUserType.SelectedValue == "3" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "6")
                if (drpUserType.SelectedValue == "3" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "6" || drpUserType.SelectedValue == "9")
                //02042018
                //BranchReportchnages Added by megha
                {
                    if (drpUserType.SelectedValue != "3") dvHOBranch.Style.Add("display", "none");

                    if (drpUserType.SelectedValue != "3") trDealer.Style.Add("display", "");                    

                    lblDealer.Style.Add("display", "");
                    drpDealer.Style.Add("display", "");
                    lblSelDlr.Style.Add("display", "");
                    //chkDept.Style.Add("display", "");
                    chkDept.Style.Add("display", "none");
                    chkDept.Items.Clear();
                    trDept.Style.Add("display", "");
                    //drpDept.Style.Add("display", "none");
                    drpDept.Style.Add("display", "");
                    btnSignup.Enabled = false;
                    txtEmpCode.Attributes.Add("readonly", "readonly");

                    txtEmpCode.Style.Add("display", "none");
                    lblEmpCode1.Style.Add("display", "none");

                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='D' AND WarrantyApplicable='N'");
                    Func.Common.BindDataToCombo(drpDlrLocation, clsCommon.ComboQueryType.DealerLocation, 0);
                    
                    if (drpUserType.SelectedValue == "3" )                    
                    {
                        Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.DealerUser, 0, " And Dealer_Origin='D' and Id not in(select distinct DealerID from TD_UserDealer) and Id in(select ID from M_Dealer where Dealer_Live='Y')");
                        //Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.DealerUser, 0, " And Dealer_Origin='D'");
                        dvHOBranch.Style.Add("display", "none");
                    }
                    else if (drpUserType.SelectedValue == "9") //02042018
                    {
                        Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.DealerUser, 0, " And Dealer_Origin='D' and Id not in(select distinct DealerID from M_MobUser) and Id in(select ID from M_Dealer where Dealer_Live='Y')");
                        dvHOBranch.Style.Add("display", "none");
                    }
                    //BranchReportchnages Added by megha
                    else if (drpUserType.SelectedValue == "6")
                    {
                        Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.MDDealerUser, 0, " And Dealer_Origin='D'");
                    }

                    //BranchReportchnages Added by megha
                    else
                    {
                        Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.DealerUser, 0, " And Dealer_Origin='E' and Id not in(select distinct DealerID from TD_UserDealer) and Id in(select ID from M_Dealer where Dealer_Live='Y')");

                    }

                    if (drpUserType.SelectedValue == "6")
                    {
                        trDept.Style.Add("display", "none");
                        drpLevels.SelectedValue = drpLevels.Items[2].Value;
                        drpLevels.Enabled = false;
                        drpDealer.Focus();
                    }
                    else
                    {
                        trDept.Style.Add("display", "");
                        drpLevels.SelectedValue = drpLevels.Items[1].Value;
                        drpLevels.Enabled = false;
                        drpDealer.Focus();

                    }
                }
                else if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "2" || drpUserType.SelectedValue == "8")
                {
                    dvHOBranch.Style.Add("display", "none");
                    trDealer.Style.Add("display", "none");
                    drpLoginName.Items.Clear();
                    drpEmpCode.Items.Clear();
                    btnSignup.Enabled = true;
                    txtFirstName.Enabled = true;
                    txtLastName.Enabled = true;
                    txtEmail.Enabled = true;
                    txtEmpCode.Enabled = true;

                    objectUser = new clsUser();
                    dtUserInfo = objectUser.GetETVUserLoginAndEmpCode(Func.Convert.iConvertToInt(drpUserType.SelectedValue));
                    drpLoginName.DataSource = dtUserInfo;
                    drpLoginName.DataTextField = "Name";
                    drpLoginName.DataValueField = "ID";
                    drpLoginName.DataBind();

                    drpEmpCode.DataSource = dtUserInfo;
                    drpEmpCode.DataTextField = "EmpCode";
                    drpEmpCode.DataValueField = "ID";
                    drpEmpCode.DataBind();


                    lblDealer.Style.Add("display", "none");
                    drpDealer.Style.Add("display", "none");
                    lblSelDlr.Style.Add("display", "none");
                    chkDept.Style.Add("display", "none");

                    drpDept.Style.Add("display", "");
                    trDept.Style.Add("display", "");

                    txtEmpCode.Attributes.Remove("readonly");

                    drpDealer.Items.Clear();
                    drpDealer.Items.Insert(0, new ListItem("--Select--", "0"));
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='E' and ID not in(4)");
                    if (drpUserType.SelectedValue == "1")
                        if (drpLevels.Items.FindByValue("4") != null)
                            drpLevels.Items.Remove(new ListItem(drpLevels.Items.FindByValue("4").Text, "4"));
                    drpLevels.Enabled = true;
                    drpLevels.Focus();
                    drpDept.Items.Add(new ListItem("--Select--", "0"));
                }
                else
                {
                    dvHOBranch.Style.Add("display", "none");
                    lblDealer.Style.Add("display", "none");
                    drpDealer.Style.Add("display", "none");
                    lblSelDlr.Style.Add("display", "none");
                    objectUser = new clsUser();
                    dtUserInfo = objectUser.GetETVUserLoginAndEmpCode(Func.Convert.iConvertToInt(drpUserType.SelectedValue));
                    drpLoginName.DataSource = dtUserInfo;
                    drpLoginName.DataTextField = "Name";
                    drpLoginName.DataValueField = "ID";
                    drpLoginName.DataBind();
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='A'");
                    drpLevels.SelectedValue = drpLevels.Items[1].Value;
                    drpLevels.Enabled = false;
                    trDept.Style.Add("display", "none");
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }

        protected void drpLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpUserType.SelectedValue == "2" && (drpLevels.SelectedValue == "4" || drpLevels.SelectedValue == "21"))
            {
                Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                if (drpDept.Items.Count > 1)
                {
                    if (drpDept.Items.FindByValue("6") != null)
                        drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("6").Text, "6"));
                    if (drpLevels.SelectedValue == "4")
                    {
                        if (drpDept.Items.FindByValue("7") != null)
                            drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("7").Text, "7"));
                        if (drpDept.Items.FindByValue("5") != null)
                            drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("5").Text, "5"));
                    }
                }
                Func.Common.FillCheckBoxList(chkModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
                if (chkModelCategory.Items.Count > 0)
                    chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));


            }
            else if (drpUserType.SelectedValue == "8")
            {
                Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                if (drpDept.Items.Count > 1)
                {
                    if (drpDept.Items.FindByValue("5") != null)
                        drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("5").Text, "5"));
                }
                Func.Common.FillCheckBoxList(chkModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
                if (chkModelCategory.Items.Count > 0)
                    chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));


            }
            else
            {
                if (drpUserType.SelectedValue == "2")
                {
                    Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");

                    if (drpDept.Items.FindByValue("8") != null)
                        drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("8").Text, "8"));

                    Func.Common.FillCheckBoxList(chkModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
                    if (chkModelCategory.Items.Count > 0)
                        chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));

                }
                else
                {
                    Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='E'");

                    if (drpDept.Items.FindByValue("4") != null)
                        drpDept.Items.Remove(new ListItem(drpDept.Items.FindByValue("4").Text, "4"));
                    Func.Common.FillCheckBoxList(chkModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);
                    if (chkModelCategory.Items.Count > 0)
                        chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));
                }
            }
        }
        protected void drpDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ClearForm();
                //Func.Common.BindDataToCombo(drpDlrLocation, clsCommon.ComboQueryType.DealerLocation, 0);
                //FillHOBRList(drpDealer, drpDlrLocation);
                //trDealer.Style.Add("display", "");
                ClearForm();
                DataSet dsMDuser;
                Func.Common.BindDataToCombo(drpDlrLocation, clsCommon.ComboQueryType.DealerLocation, 0);
                if (drpUserType.SelectedValue == "6")
                {
                    int MDDealerID = Convert.ToInt32(drpDealer.SelectedValue);
                    dsMDuser = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetMDUser", MDDealerID);
                    if (dsMDuser.Tables[0].Rows.Count > 0)
                    {
                        //BranchReportchnages Added by megha

                        hdnSpareCode.Value = Func.Convert.sConvertToString(dsMDuser.Tables[0].Rows[0]["Dealer_Spares_Code"]);
                        if (txtLoginName.Text == "")
                            txtLoginName.Text = hdnSpareCode.Value + "ADMIN";
                        txtEmail.Text = Func.Convert.sConvertToString(dsMDuser.Tables[0].Rows[0]["Dealer_Email"]);
                        txtFirstName.Text = Func.Convert.sConvertToString(dsMDuser.Tables[0].Rows[0]["Dealer_Name"]);
                        btnSignup.Enabled = true;
                    }
                }
                //
                if (drpUserType.SelectedValue != "3" && drpUserType.SelectedValue !="4" && drpUserType.SelectedValue != "9")
                {
                    FillHOBRList(drpDealer, drpDlrLocation);
                    trDealer.Style.Add("display", "");
                }
                if (drpUserType.SelectedValue == "3" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "9")
                {
                    clsUser objectUser = null;
                    DataSet dsDealerUserInfo = null;
                    trDealer.Style.Add("display", "none");                    
                    objectUser = new clsUser();
                    dsDealerUserInfo = new DataSet();
                    chkDept.Items.Clear();
                    dsDealerUserInfo = objectUser.GetDealerUserInfo(Func.Convert.iConvertToInt(drpDealer.SelectedValue));

                    if (drpDealer.SelectedValue != "0")
                    {
                        chkDept.Style.Add("display", "");
                        Func.Common.FillCheckBoxList(chkDept, clsCommon.ComboQueryType.Department, 0, ((drpUserType.SelectedValue == "3") ? " and ExportDomestic='D'" : " and ExportDomestic='E'"));
                        drpDept.Style.Add("display", "none");
                        chkDept.Enabled = false;
                    }
                    if (dsDealerUserInfo != null && dsDealerUserInfo.Tables[1].Rows.Count > 0 && drpDealer.SelectedValue != "0")
                        for (int iCnt = 0; iCnt < dsDealerUserInfo.Tables[1].Rows.Count; iCnt++)
                        {
                            if (drpDept.Items.Count > 0)
                            {
                                drpDept.Items.Remove(new ListItem(Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptName"]), Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptID"])));

                            }
                        }
                    if (dsDealerUserInfo != null && dsDealerUserInfo.Tables[2].Rows.Count > 0 && drpDealer.SelectedValue != "0")
                    {
                        hdnSalesCode.Value = "";
                        hdnSpareCode.Value = "";
                        hdnHOBranchCode.Value = "";
                        txtEmail.Text = "";
                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtLoginName.Text = "";
                        txtEmpCode.Text = "";
                        txtFirstName.Enabled = false;
                        txtLastName.Enabled = false;
                        txtEmail.Enabled = false;
                        txtEmpCode.Enabled = false;

                        hdnSalesCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["VCode"]);
                        hdnSpareCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["SCode"]);
                        hdnHOBranchCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["HOBranchCode"]);
                        hdnHOBranch.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["HOBranchCode"]) == "00" ? "HO" : "Branch";
                        hdnHOBranchID.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["HOBrID"]);

                        txtEmail.Text = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["Email"]);
                        if (txtEmail.Text == "")
                            txtEmail.Enabled = true;
                        if (txtEmpCode.Text == "")
                            txtEmpCode.Enabled = true;
                        string[] splDealerName = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["DealerName"]).Split(' ');
                        int iFirstCnt = ((splDealerName.Length) % 2 == 0) ? Func.Convert.iConvertToInt(splDealerName.Length / 2) : Func.Convert.iConvertToInt(splDealerName.Length / 2) + 1;
                        for (int iCnt = 0; iCnt < splDealerName.Length; iCnt++)
                        {
                            if (iCnt < iFirstCnt)
                                txtFirstName.Text = txtFirstName.Text + " " + Func.Convert.sConvertToString(splDealerName[iCnt]);
                            else
                                txtLastName.Text = txtLastName.Text + " " + Func.Convert.sConvertToString(splDealerName[iCnt]);

                        }
                    }
                    else
                    {
                        hdnSalesCode.Value = "";
                        hdnSpareCode.Value = "";
                        hdnHOBranchCode.Value = "";
                        txtEmail.Text = "";
                        txtEmail.Text = "";
                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtLoginName.Text = "";
                        txtFirstName.Enabled = true;
                        txtLastName.Enabled = true;
                        txtEmail.Enabled = true;
                        txtEmpCode.Enabled = true;
                    }
                    if (drpUserType.SelectedValue == "9")
                    {
                        txtLoginName.Text = hdnSalesCode.Value;
                        btnSignup.Enabled = true; 
                    }
                    else
                    {
                        setDepartment(dsDealerUserInfo);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }
        protected void drpDlrLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillHOBRList(drpDealer, drpDlrLocation);
        }
        private void FillHOBRList(DropDownList drpDealer, DropDownList drpDlrLocation)
        {
            try
            {
                if (drpDlrLocation.SelectedItem.Text == "HO") // for HO       
                    Func.Common.BindDataToCombo(drpHOBRList, clsCommon.ComboQueryType.DealerHOBRList, 0, " And HODlr_Id='" + drpDealer.SelectedValue + "' AND HOBranchCode = '" + drpDlrLocation.SelectedValue + "'");
                else if (drpDlrLocation.SelectedValue == "0")// for Select
                    Func.Common.BindDataToCombo(drpHOBRList, clsCommon.ComboQueryType.DealerHOBRList, 0, " And HODlr_Id='" + drpDealer.SelectedValue + "' AND HOBranchCode = '-1'");
                else// for Branch
                    Func.Common.BindDataToCombo(drpHOBRList, clsCommon.ComboQueryType.DealerHOBRList, 0, " And HODlr_Id='" + drpDealer.SelectedValue + "' AND HOBranchCode <>'00'");
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        protected void drpHOBRList_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsUser objectUser = null;
            DataSet dsDealerUserInfo = null;
            try
            {
                objectUser = new clsUser();
                dsDealerUserInfo = new DataSet();
                chkDept.Items.Clear();
                dsDealerUserInfo = objectUser.GetDealerUserInfo(Func.Convert.iConvertToInt(drpHOBRList.SelectedValue));

                if (drpUserType.SelectedValue == "3" && drpDealer.SelectedValue != "0")
                {
                    chkDept.Style.Add("display", "");  
                    Func.Common.FillCheckBoxList(chkDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                   // Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                    drpDept.Style.Add("display", "none");
                    chkDept.Enabled = false; 
                }
                //BranchReportchnages Added by megha
                else if (drpUserType.SelectedValue == "6" && drpDealer.SelectedValue != "0")
                {
                    //Func.Common.FillCheckBoxList(chkDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                    Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                }
                //BranchReportchnages Added by megha
                else if (drpUserType.SelectedValue == "4" && drpDealer.SelectedValue != "0")
                {
                    //Func.Common.FillCheckBoxList(chkDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='E'");
                    Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='E'");

                }




                //if (dsDealerUserInfo != null && dsDealerUserInfo.Tables[1].Rows.Count > 0 && drpDealer.SelectedValue != "0")
                //    for (int iCnt = 0; iCnt < dsDealerUserInfo.Tables[1].Rows.Count; iCnt++)
                //    {
                //        if (chkDept.Items.Count > 0)
                //        {
                //            chkDept.Items.Remove(new ListItem(Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptName"]), Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptID"])));

                //        }
                //    }

                if (dsDealerUserInfo != null && dsDealerUserInfo.Tables[1].Rows.Count > 0 && drpDealer.SelectedValue != "0")
                    for (int iCnt = 0; iCnt < dsDealerUserInfo.Tables[1].Rows.Count; iCnt++)
                    {
                        if (drpDept.Items.Count > 0)
                        {
                            drpDept.Items.Remove(new ListItem(Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptName"]), Func.Convert.sConvertToString(dsDealerUserInfo.Tables[1].Rows[iCnt]["DeptID"])));

                        }
                    }

                if (dsDealerUserInfo != null && dsDealerUserInfo.Tables[2].Rows.Count > 0 && drpDealer.SelectedValue != "0")
                {
                    hdnSalesCode.Value = "";
                    hdnSpareCode.Value = "";
                    hdnHOBranchCode.Value = "";
                    txtEmail.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtLoginName.Text = "";
                    txtEmpCode.Text = "";
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    txtEmail.Enabled = false;
                    txtEmpCode.Enabled = false;

                    hdnSalesCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["VCode"]);
                    hdnSpareCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["SCode"]);
                    hdnHOBranchCode.Value = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["HOBranchCode"]);
                    txtEmail.Text = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["Email"]);
                    if (txtEmail.Text == "")
                        txtEmail.Enabled = true;
                    if (txtEmpCode.Text == "")
                        txtEmpCode.Enabled = true;
                    string[] splDealerName = Func.Convert.sConvertToString(dsDealerUserInfo.Tables[2].Rows[0]["DealerName"]).Split(' ');
                    int iFirstCnt = ((splDealerName.Length) % 2 == 0) ? Func.Convert.iConvertToInt(splDealerName.Length / 2) : Func.Convert.iConvertToInt(splDealerName.Length / 2) + 1;
                    for (int iCnt = 0; iCnt < splDealerName.Length; iCnt++)
                    {
                        if (iCnt < iFirstCnt)
                            txtFirstName.Text = txtFirstName.Text + " " + Func.Convert.sConvertToString(splDealerName[iCnt]);
                        else
                            txtLastName.Text = txtLastName.Text + " " + Func.Convert.sConvertToString(splDealerName[iCnt]);

                    }
                }
                else
                {
                    hdnSalesCode.Value = "";
                    hdnSpareCode.Value = "";
                    hdnHOBranchCode.Value = "";
                    txtEmail.Text = "";
                    txtEmail.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtLoginName.Text = "";
                    txtFirstName.Enabled = true;
                    txtLastName.Enabled = true;
                    txtEmail.Enabled = true;
                    txtEmpCode.Enabled = true;
                }
                //BranchReportchnages Added by megha
                if (drpUserType.SelectedValue == "6")
                {
                    if (txtLoginName.Text == "")
                        txtLoginName.Text = hdnSpareCode.Value + "MD";
                    btnSignup.Enabled = true;

                }
                else
                {
                    setDepartment(dsDealerUserInfo);
                }
                //BranchReportchnages Added by megha
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (objectUser != null) objectUser = null;
                if (dsDealerUserInfo != null) dsDealerUserInfo = null;
            }
        }
        private void setDepartment(DataSet dsDealerUserInfo)
        {
            int iChkCnt = 0;
            for (int iMainCnt = 0; iMainCnt < chkDept.Items.Count; iMainCnt++)
            {
                int iSelect = 0;
                for (int iSubCnt = 0; iSubCnt < dsDealerUserInfo.Tables[0].Rows.Count; iSubCnt++)
                {
                    if (Func.Convert.sConvertToString(chkDept.Items[iMainCnt].Value) == Func.Convert.sConvertToString(dsDealerUserInfo.Tables[0].Rows[iSubCnt]["DeptID"]))
                        iSelect = iSelect + 1;

                }
                if (iSelect > 0)
                {
                    chkDept.Items[iMainCnt].Enabled = false;
                    chkDept.Items[iMainCnt].Selected = true;
                }

                else
                {
                    chkDept.Items[iMainCnt].Enabled = true;
                    iChkCnt = iChkCnt + 1;
                    chkDept.Items[iMainCnt].Selected = true;
                    if (chkDept.Items[iMainCnt].Text.ToUpper().Contains("SALES") == true)
                    {
                        if (hdnSalesCode.Value.Trim() != "")
                        {
                            for (int iSubCnt = 1; iSubCnt < 4; iSubCnt++)
                            {
                                if (txtLoginName.Text == "")
                                    txtLoginName.Text = hdnSalesCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                                else
                                    txtLoginName.Text = txtLoginName.Text + "/" + hdnSalesCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "<script>alert('Dealer user for Sale cannot create.Sales code not found!')</script>");

                        }
                    }
                    else if (chkDept.Items[iMainCnt].Text.ToUpper().Contains("SALES") == false)
                    {
                        if (hdnSpareCode.Value.Trim() != "")
                        {
                            for (int iSubCnt = 1; iSubCnt < 4; iSubCnt++)
                            {
                                if (txtLoginName.Text == "")
                                    txtLoginName.Text = hdnSpareCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                                else
                                    txtLoginName.Text = txtLoginName.Text + "/" + hdnSpareCode.Value + chkDept.Items[iMainCnt].Text.ToUpper() + "0" + Func.Convert.sConvertToString(iSubCnt);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "<script>alert('Dealer user for Spare/Service cannot create.Spare/Service code not found')</script>");

                        }
                    }
                }
            }
            if (iChkCnt == 0)
                btnSignup.Enabled = false;
            else
                btnSignup.Enabled = true;
        }

        protected void drpDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsUser objectUser = null;
            try
            {
                int iUserTypeID = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
                string DeptText = drpDept.SelectedItem.Text.ToUpper();
                if (iUserTypeID == 3 || iUserTypeID == 4 || iUserTypeID == 6)
                {
                    objectUser = new  clsUser();
                    txtLoginName.Text = "";
                    int iMaxDocNo =  objectUser.iGetMaxDocNo(drpDlrLocation.SelectedItem.Text, drpHOBRList.SelectedValue + "/" + drpDept.SelectedValue);
                    if (drpDept.SelectedItem.Text.ToUpper() == "SALES")
                        txtLoginName.Text = hdnSalesCode.Value + hdnHOBranchCode.Value + drpDept.SelectedItem.Text.ToUpper() + (iMaxDocNo + 1).ToString().PadLeft(2, '0');
                    else if (drpDept.SelectedValue != "0")
                        txtLoginName.Text = hdnSpareCode.Value + hdnHOBranchCode.Value + drpDept.SelectedItem.Text.ToUpper() + (iMaxDocNo + 1).ToString().PadLeft(2, '0');
                    txtEmpCode.Style.Add("display", "none");
                    lblEmpCode1.Style.Add("display", "none");
                    if (txtLoginName.Text != "")
                        btnSignup.Enabled = true;
                    else
                        btnSignup.Enabled = false;
                }
                else
                {
                    txtEmpCode.Style.Add("display", "");
                    lblEmpCode1.Style.Add("display", "");
                    //if (DeptText == "Service")
                    //    txtEmpCode.Style.Add("display", "");
                    //else
                    //{
                    //    txtEmpCode.Style.Add("display", "none");
                    //}
                    if ((DeptText == "SALES" || DeptText == "SERVICE") && (iUserTypeID == 2 || iUserTypeID == 1 || iUserTypeID == 8))
                    {

                        lblModelCategoryMD.Style.Add("display", "");
                        chkModelCategory.Style.Add("display", "");
                        lblModelCategory.Style.Add("display", "");
                        foreach (ListItem LID in chkModelCategory.Items)
                        {
                            LID.Selected = true;                           
                        }
                        lblModelCategoryMD.Style.Add("display", "none");
                        chkModelCategory.Style.Add("display", "none");
                        lblModelCategory.Style.Add("display", "none");
                    }
                    else
                    {
                        lblModelCategoryMD.Style.Add("display", "none");
                        if (chkModelCategory != null)
                            chkModelCategory.Style.Add("display", "none");
                        else
                            chkModelCategory = null;
                        lblModelCategory.Style.Add("display", "none");
                    }
                }
            }
            catch (Exception Ex)
            {
            }
            finally
            {
            }
        }
    }
}