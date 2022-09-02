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
using AjaxControlToolkit;
using System.Drawing;
using MANART_BAL;
using MANART_DAL;

namespace MANART.Forms.Master
{
    public partial class frmSystemParameterValue : System.Web.UI.Page
    {
        private DataTable dtDetails_syspara = new DataTable();
        private DataTable dtDetails_para = new DataTable();



        private int iDealerID = 0;
        private string sDealerID = "";
        private string sDealer_ID = "";
        private int iYearID = 0;
        int iMenuId = 0;
        string cType;
        int iUserId = 0;
        int iUserType = 0;
        string sDealerIds = "";

        int iHOBr_id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                iUserType = Func.Convert.iConvertToInt(Session["UserType"]);
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                if (!IsPostBack)
                {
                    DisplayCurrentRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.bUseImgOrButton = true;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        private bool bSaveRecord(bool bSaveWithConfirm)
        {
            try
            {

                clsParameter ObjParameter = new clsParameter();



                FillDetailsFromGrid_syspara();
                FillDetailsFromGrid_para();

                if (ObjParameter.bSaveSystemParametersValue(dtDetails_syspara, dtDetails_para, iDealerID, iHOBr_id) == true)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                    return false;
                }
                ObjParameter = null;
                return true;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }

        }

        //Fill Details From Grid
        private void FillDetailsFromGrid_syspara()
        {
            DataRow dr;
            dtDetails_syspara = new DataTable();
            dtDetails_syspara.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails_syspara.Columns.Add(new DataColumn("ParameterName", typeof(string)));
            dtDetails_syspara.Columns.Add(new DataColumn("ParaValue", typeof(string)));
            dtDetails_syspara.Columns.Add(new DataColumn("Active", typeof(string)));
            dtDetails_syspara.Columns.Add(new DataColumn("Editable", typeof(string)));

            for (int iRowCnt = 0; iRowCnt < GrdSystemParameterValue.Rows.Count; iRowCnt++)
            {

                dr = dtDetails_syspara.NewRow();

                dr["ID"] = Func.Convert.iConvertToInt((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblID") as Label).Text);
                // if (Func.Convert.iConvertToInt((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblID") as Label).Text) == 0)
                // {
                dr["ParameterName"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblParameterName") as Label).Text);
                dr["ParaValue"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("txtPara_Value") as TextBox).Text);
                dr["Active"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("drpActive") as DropDownList).SelectedItem.Text);
                dr["Editable"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("drpEditable") as DropDownList).SelectedItem.Text);
                //  }
                // else
                // {
                //     dr["ParameterName"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblParameterName") as Label).Text);
                //     dr["ParaValue"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("txtPara_Value") as TextBox).Text);
                //     dr["Active"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblActive") as Label).Text);
                //     dr["Editable"] = Func.Convert.sConvertToString((GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblEditable") as Label).Text);
                //// }
                dtDetails_syspara.Rows.Add(dr);
                dtDetails_syspara.AcceptChanges();
            }
        }

        private void FillDetailsFromGrid_para()
        {
            DataRow dr;
            dtDetails_para = new DataTable();
            dtDetails_para.Columns.Add(new DataColumn("ID", typeof(int)));
            dtDetails_para.Columns.Add(new DataColumn("ParameterName", typeof(string)));
            dtDetails_para.Columns.Add(new DataColumn("FlagValue", typeof(string)));
            dtDetails_para.Columns.Add(new DataColumn("Active", typeof(string)));
            dtDetails_para.Columns.Add(new DataColumn("Editable", typeof(string)));

            for (int iRowCnt = 0; iRowCnt < GrdParameterValue.Rows.Count; iRowCnt++)
            {

                dr = dtDetails_para.NewRow();

                dr["ID"] = Func.Convert.iConvertToInt((GrdParameterValue.Rows[iRowCnt].FindControl("lblID") as Label).Text);
                // if (Func.Convert.iConvertToInt((GrdParameterValue.Rows[iRowCnt].FindControl("lblID") as Label).Text) == 0)
                //{
                dr["ParameterName"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("lblParameterName") as Label).Text);
                dr["FlagValue"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("drpFlagValue") as DropDownList).SelectedItem.Text);
                dr["Active"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("drpActive") as DropDownList).SelectedItem.Text);
                dr["Editable"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("drpEditable") as DropDownList).SelectedItem.Text);
                // // }
                // // else
                ////  {
                //      dr["ParameterName"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("lblParameterName") as Label).Text);
                //      dr["FlagValue"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("lblFlagValue") as Label).Text);
                //      dr["Active"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("lblActive") as Label).Text);
                //      dr["Editable"] = Func.Convert.sConvertToString((GrdParameterValue.Rows[iRowCnt].FindControl("lblEditable") as Label).Text);
                // // }

                dtDetails_para.Rows.Add(dr);
                dtDetails_para.AcceptChanges();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                //string strDisAbleBackButton;
                //ToolbarC.EnableDisableImage(WebParts_Toolbar.enmToolbarType.enmNew, false);
                //strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                //strDisAbleBackButton += "window.history.forward(1);\n";
                //strDisAbleBackButton += "\n</SCRIPT>";
                //ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                if (!IsPostBack)
                {

                    DisplayCurrentRecord();
                }
                //btnReadonly();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }



        private void DisplayCurrentRecord()
        {
            try
            {
                DataTable dtDetails_syspara = null;
                DataTable dtDetails_para = null;
                DataSet ds = new DataSet();

                clsParameter ObjParameter = new clsParameter();

                ds = ObjParameter.GetSystemParameters(iDealerID, iHOBr_id, 0);


                if (Func.Common.iRowCntOfTable(ds.Tables[0]) > 0)
                {
                    dtDetails_syspara = ds.Tables[0];
                    Session["SysParaDetails"] = dtDetails_syspara;
                    BindDataToGrid();
                    ObjParameter = null;
                }
                else
                {
                    dtDetails_syspara = ds.Tables[0];
                    Session["SysParaDetails"] = dtDetails_syspara;
                    BindDataToGrid();
                }
                if (Func.Common.iRowCntOfTable(ds.Tables[1]) > 0)
                {
                    dtDetails_para = ds.Tables[1];
                    Session["ParaDetails"] = dtDetails_para;
                    BindDataToGrid();
                    ObjParameter = null;
                }
                else
                {
                    dtDetails_para = ds.Tables[1];
                    Session["ParaDetails"] = dtDetails_para;
                    BindDataToGrid();
                }


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void BindDataToGrid()
        {
            try
            {
                //If No Data in Grid
                if (Session["SysParaDetails"] == null)
                {
                    Session["SysParaDetails"] = dtDetails_syspara;
                }
                else
                {
                    dtDetails_syspara = (DataTable)Session["SysParaDetails"];
                }

                GrdSystemParameterValue.DataSource = dtDetails_syspara;
                GrdSystemParameterValue.DataBind();
                SetGridControlProperty_syspara();

                if (Session["ParaDetails"] == null)
                {
                    Session["ParaDetails"] = dtDetails_para;
                }
                else
                {
                    dtDetails_para = (DataTable)Session["ParaDetails"];
                }

                GrdParameterValue.DataSource = dtDetails_para;
                GrdParameterValue.DataBind();
                SetGridControlProperty_para();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }



        private void SetGridControlProperty_syspara()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdSystemParameterValue.Rows.Count; iRowCnt++)
                {


                    Label lblID = (Label)GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblID");
                    lblID.Text = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["ID"]);

                    Label lblParameterName = (Label)GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblParameterName");
                    lblParameterName.Text = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["sys_para_desc"]);

                    TextBox txtPara_Value = (TextBox)GrdSystemParameterValue.Rows[iRowCnt].FindControl("txtPara_Value");
                    txtPara_Value.Text = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["Para_Value"]);

                    DropDownList drpActive = (DropDownList)GrdSystemParameterValue.Rows[iRowCnt].FindControl("drpActive");
                    drpActive.SelectedValue = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["Active"]);
                    drpActive.CssClass = "GridComboBoxFixedSize";
                    // drpActive.Width = Unit.Pixel(70);
                    // drpActive.Visible = false;

                    //Label lblActive = (Label)GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblActive");
                    //lblActive.Text = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["Active"]);
                    //lblActive.Visible = true;

                    DropDownList drpEditable = (DropDownList)GrdSystemParameterValue.Rows[iRowCnt].FindControl("drpEditable");
                    drpEditable.SelectedValue = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["Editable"]);
                    drpEditable.CssClass = "GridComboBoxFixedSize";
                    //  drpEditable.Width = Unit.Pixel(70);
                    // drpEditable.Visible = false;

                    //Label lblEditable = (Label)GrdSystemParameterValue.Rows[iRowCnt].FindControl("lblEditable");
                    //lblEditable.Text = Func.Convert.sConvertToString(dtDetails_syspara.Rows[iRowCnt]["Editable"]);
                    //lblEditable.Visible = true;


                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void SetGridControlProperty_para()
        {
            try
            {
                for (int iRowCnt = 0; iRowCnt < GrdParameterValue.Rows.Count; iRowCnt++)
                {


                    Label lblID = (Label)GrdParameterValue.Rows[iRowCnt].FindControl("lblID");
                    lblID.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["ID"]);

                    Label lblParameterName = (Label)GrdParameterValue.Rows[iRowCnt].FindControl("lblParameterName");
                    lblParameterName.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["sys_para_desc"]);

                    //TextBox txtPara_Value = (TextBox)GrdParameterValue.Rows[iRowCnt].FindControl("txtPara_Value");
                    //txtPara_Value.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["Para_Value"]);

                    DropDownList drpFlagValue = (DropDownList)GrdParameterValue.Rows[iRowCnt].FindControl("drpFlagValue");
                    drpFlagValue.SelectedValue = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["flag_Value"]);
                    drpFlagValue.CssClass = "GridComboBoxFixedSize";
                    //drpFlagValue.Width = Unit.Pixel(70);
                    //  drpFlagValue.Visible = false;

                    //Label lblFlagValue = (Label)GrdParameterValue.Rows[iRowCnt].FindControl("lblFlagValue");
                    //lblFlagValue.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["flag_Value"]);
                    //lblFlagValue.Visible = true;

                    DropDownList drpActive = (DropDownList)GrdParameterValue.Rows[iRowCnt].FindControl("drpActive");
                    drpActive.SelectedValue = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["Active"]);
                    drpActive.CssClass = "GridComboBoxFixedSize";
                    // drpActive.Width = Unit.Pixel(70);
                    // drpActive.Visible = false;

                    //Label lblActive = (Label)GrdParameterValue.Rows[iRowCnt].FindControl("lblActive");
                    //lblActive.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["Active"]);
                    //lblActive.Visible = true;
                    DropDownList drpEditable = (DropDownList)GrdParameterValue.Rows[iRowCnt].FindControl("drpEditable");
                    drpEditable.SelectedValue = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["Editable"]);
                    drpEditable.CssClass = "GridComboBoxFixedSize";
                    //drpEditable.Width = Unit.Pixel(70);



                    //Label lblEditable = (Label)GrdParameterValue.Rows[iRowCnt].FindControl("lblEditable");
                    //lblEditable.Text = Func.Convert.sConvertToString(dtDetails_para.Rows[iRowCnt]["Editable"]);
                    //lblEditable.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bSaveRecord(false) == false) return;
                    DisplayCurrentRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void GrdSystemParameterValue_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Session["SysParaDetails"] = dtDetails_syspara;
                GrdSystemParameterValue.DataSource = dtDetails_syspara;
                GrdSystemParameterValue.DataBind();
                SetGridControlProperty_syspara();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void GrdParameterValue_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Session["ParaDetails"] = dtDetails_para;
                GrdParameterValue.DataSource = dtDetails_para;
                GrdParameterValue.DataBind();
                SetGridControlProperty_para();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void BtnShow_Click(object sender, EventArgs e)
        {
            DisplayCurrentRecord();

        }
        protected void GrdSystemParameterValue_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }
    }
}