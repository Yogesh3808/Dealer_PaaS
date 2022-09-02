using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;

namespace MANART
{
    public partial class frmException : System.Web.UI.Page
    {
        private string sType = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            sType = Func.Convert.sConvertToString(Request.QueryString["sType"]);
            if (sType == "Session")
            {
                lblTitlenew.Text = "Your session has expired. Please log in again........!";
            }
            else if (sType == "Error")
            {
                if (Page.IsPostBack == false)
                {
                    if (Session["UserID"] != null)
                    {
                        DataTable dt = new DataTable();
                        if (Application["UserTrack"] != null)
                            dt = (DataTable)Application["UserTrack"];
                        if (dt != null)
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Func.Convert.sConvertToString(dt.Rows[i]["LoginName"]) == Func.Convert.sConvertToString(Session["LoginName"]) && Func.Convert.sConvertToString(dt.Rows[i]["SessionID"]) == Session.SessionID.ToString())
                                    {
                                        dt.Rows.RemoveAt(i);
                                        dt.AcceptChanges();
                                        ClearSessionVlaue();
                                        Application.Lock();
                                        Application["Active"] = Convert.ToInt32(Application["Active"]) - 1;
                                        //this adds 1 to the number of active users when a new user hits 
                                        Application.UnLock();
                                    }
                                }
                                Application["UserTrack"] = dt;
                            }
                    }
                }
                Session.Clear();
                Session.Abandon();
            }
        }
        protected void lnkLogOut_Click(object sender, ImageClickEventArgs e)
        {
            ClearSessionVlaue();
            Response.Redirect("~/frmLogin.aspx", false);

        }
        private void ClearSessionVlaue()
        {
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session["sMenuValue"] = null;
            Session["sMenuText"] = null;
            Session["UserType"] = null;
            Session["UserID"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();

        }
    }
}