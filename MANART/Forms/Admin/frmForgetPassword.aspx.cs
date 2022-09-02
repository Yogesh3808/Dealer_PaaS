using System;
using System.Web;
using System.Web.UI;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Admin
{
    public partial class frmForgetPassword : System.Web.UI.Page
    {
        string sUserName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sUserName = Request.QueryString["LoginName"];
            ValidateUserAndSendPassword();
            ExpirePageCache();
        }

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        private bool ValidateUserAndSendPassword()
        {
            clsUser objclsUser = new clsUser();
            string sEmailID = "";
            bool flage = objclsUser.CheckUserExist(sUserName, ref sEmailID);
            if (flage == false)
            {
                lblForgotPasswordMessage.Text = "Sorry User Does Not Exist!!!";
                btnCancel.Visible = false;
                return false;
            }
            else
            {
                lblForgotPasswordMessage.Text = "Password will be mailed to you on following Mail id : \n" + sEmailID;
                btnCancel.Visible = true;
                return true;
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateUserAndSendPassword() == true)
                {
                    clsUser objclsUser = new clsUser();
                    try
                    {
                        objclsUser.EmailForgotPasswordSend(sUserName);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ClosePopupWindow();", true);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ClosePopupWindow();", true);
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }   
    }
}