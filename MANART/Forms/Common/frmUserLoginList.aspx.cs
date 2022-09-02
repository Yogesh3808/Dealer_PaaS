using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MANART.Forms.Common
{
    public partial class frmUserLoginList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                lblNote.Text = "Total No of Active User " + Application["Active"].ToString();
                DataTable dtActiveUser = new DataTable();
                dtActiveUser = (DataTable)Application["UserTrack"];
                DataView dvActiveUser = dtActiveUser.DefaultView;
                dvActiveUser.RowFilter = "LoginDateTime > '" + System.DateTime.Today.AddDays(-1) + "'";
                if (dtActiveUser.Rows.Count > 0)
                {
                    DocumentGrid.DataSource = dvActiveUser;
                    DocumentGrid.DataBind();
                }
            }
            //this.Response.AppendHeader("Refresh", Convert.ToString(5));

        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            lblNote.Text = "Total No of Active User " + Application["Active"].ToString();
            DataTable dtActiveUser = new DataTable();
            dtActiveUser = (DataTable)Application["UserTrack"];
            if (dtActiveUser.Rows.Count > 0)
            {
                DocumentGrid.DataSource = dtActiveUser;
                DocumentGrid.DataBind();
            }
        }
    }
}