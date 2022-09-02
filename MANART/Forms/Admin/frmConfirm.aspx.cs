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

namespace MANART.Forms.Admin
{
    public partial class frmConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sUserName = Request.QueryString["UserName"].ToString(); ;
            lblConfirmation.Text = sUserName + "  Register Successfully! ";
            lblEmailconfirmation.Text = sUserName + " will receive a confirmation e-mail as soon as his account is activated.";
        }
    }
}