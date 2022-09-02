using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Common
{
    public partial class frmDump : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
            int iUserRollId = Func.Convert.iConvertToInt(Session["UserRole"]);
            frame1.Attributes["src"] = "http://192.168.1.67/Dump/?&MenuID= " + iMenuId + "&UserRole= " + iUserRollId;
        }
    }
}