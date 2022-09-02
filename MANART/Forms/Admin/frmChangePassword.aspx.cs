using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Admin
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        int iUserID = 0;
        string sNewPassword = "";
        static string prevPage = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FirstName"] != null && Session["LastName"] != null)
                lblUserName.Text = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
            if (Session["UserID"] != null)
                txtUserID.Text = Session["UserID"].ToString();
            if (Session["Password"] != null)
                hdnPassword.Value = Session["Password"].ToString();

            if (Page.IsPostBack)
            {
                txtOldPassword.Attributes.Add("value", this.txtOldPassword.Text);
                txtPassword.Attributes.Add("value", this.txtPassword.Text);
                txtRPassword.Attributes.Add("value", this.txtRPassword.Text);


            }
            else
                prevPage = Request.UrlReferrer.ToString();

        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                sNewPassword = txtPassword.Text;
                iUserID = Func.Convert.iConvertToInt(txtUserID.Text);
                int ret = 0;
                clsUser objUser = new clsUser();
                ret = objUser.PasswordValidation(sNewPassword, iUserID, 0);
                if (ret == 1)
                {

                    if (objUser.ChangePassword(iUserID, sNewPassword) == true)
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Password Changed Successfully');</script>");
                        Session["Password"] = sNewPassword;
                        btnChangePassword.Enabled = false;
                        LblConfirmMessage.Text = "";
                        Response.Redirect("~/frmLogin.aspx?Msg=Success", false);
                    }
                    else
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Error in Change Password!');</script>");
                    }
                }
                else
                {
                    txtPassword.Focus();
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('The new Password should be different from last 5 Password!');</script>");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}