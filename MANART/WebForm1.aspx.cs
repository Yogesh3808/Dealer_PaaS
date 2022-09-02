using MANART_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MANART
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OnlyHeaderSaveDocList();
            }
        }

        private void OnlyHeaderSaveDocList()
        {
            DataSet ds = new DataSet();
            clsDB objDB = new clsDB();
            try
            {
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_OnlyHeaderSaveDocList");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvOnlyHeaderSave.DataSource = ds;
                    gvOnlyHeaderSave.DataBind();
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

        protected void OpenDocument(object sender, EventArgs e)
        {
            string status = "", sMessage="";
            if (txtDocNo.Text.Trim() == "")
            {
                sMessage = "Please Enter Document No";
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                clsDB objDB = new clsDB();
                try
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_OpenDocument",txtDocNo.Text.Trim(),Func.Convert.sConvertToString(ddlDocName.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        status = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["DocStatus"]);
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + status + ".');</script>");
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (objDB != null) objDB = null;
                    if (ds != null) ds = null;
                }
            }
        }

    }
}