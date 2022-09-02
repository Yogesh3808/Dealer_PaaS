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

namespace MANART.Forms.Admin
{
    public partial class frmPasswodRetrieve : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGetPwd_Click(object sender, EventArgs e)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtUser = new DataTable();
                dtUser = objDB.ExecuteStoredProcedureAndGetDataTable("SP_RetrivePassword", txtLoginName.Text.Trim(), Func.Convert.iConvertToInt(Session["UserID"]));
                if (dtUser != null)
                    if (dtUser.Rows.Count > 0 && Func.Convert.iConvertToInt(dtUser.Rows[0]["userID"]) != 0)
                    {
                        txtPassword.Text = clsCrypto.Decrypt(Func.Convert.sConvertToString(dtUser.Rows[0]["Password"]));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ErrorMessage();", true);
                        txtPassword.Text = "";
                    }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
            txtLoginName.Text = "";
            txtLoginName.Focus();
        }
    }
}