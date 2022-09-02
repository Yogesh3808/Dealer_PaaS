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
using AjaxControlToolkit;
using Microsoft.Reporting.WebForms;


namespace MANART.Forms.Common
{
    public partial class frmReportView : System.Web.UI.Page
    {
        int iMenuId = 0;
        int iMonthID = 0;
        int iUserRollId = 0;
        int iParaCount = 0;
        int iModelCategory = 0;



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                if (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                {
                    lblDelOrg.Text = "E";
                }
                else
                {
                    lblDelOrg.Text = "D";
                }

                if (iMenuId == 405 || iMenuId == 213)
                {
                    lblVehSpr.Text = "V";
                }
                else
                {
                    lblVehSpr.Text = "S";
                }

                iUserRollId = Func.Convert.iConvertToInt(Session["UserRole"]);
                lblRegionID.Text = Func.Convert.sConvertToString(drpRegionSelection.SelectedValue);
                lblCountryID.Text = Func.Convert.sConvertToString(drpCountrySelection0.SelectedValue);
                lblDealerID.Text = Func.Convert.sConvertToString(drpDealer.SelectedValue);
                lblCSMID.Text = Func.Convert.sConvertToString(0);
                lblcusttype.Text = Func.Convert.sConvertToString(drpcusttype.SelectedValue);

                if (lblRegionID.Text == "")
                {
                    lblRegionID.Text = "0";
                }
                //sujata 18032011
                //Sujata 19042011
                //UserSetting();
                //Sujata 19042011
                //Sujata 18032011
                //Sujata 02042011
                //if (Func.Convert.iConvertToInt(Session["UserRole"]) == 6 || Func.Convert.iConvertToInt(Session["UserRole"]) == 5)
                //{
                //    DataTable dtDealer = null;
                //    DataSet dsDealer = new DataSet();

                //    clsReport objReport = new clsReport();

                //    dsDealer = objReport.SetDealerID(Func.Convert.iConvertToInt(Session["UserID"]));
                //    dtDealer = dsDealer.Tables[0];
                //    if (dtDealer.Rows.Count == 0)
                //    {

                //    }
                //    else
                //    {
                //        lblDealerID.Text = Func.Convert.sConvertToString(dtDealer.Rows[0]["DealerID"]);
                //    }
                //    //lblDealerID.Text  = Func.Convert.sConvertToString(Session["UserID"]);
                //}

                //if (Func.Convert.iConvertToInt(Session["UserRole"]) == 4)
                //{
                //    lblCSMID.Text = Func.Convert.sConvertToString(Session["UserID"]);
                //}
                //else
                //{
                //    lblCSMID.Text = Func.Convert.sConvertToString(drpCSMSelection.SelectedValue);
                //}

                //if (lblCSMID.Text == "")
                //{
                //    lblCSMID.Text = Func.Convert.sConvertToString(0);
                //}

                //if (lblCountryID.Text == "")
                //{
                //    lblCountryID.Text = Func.Convert.sConvertToString(0);
                //}
                //if (lblcusttype.Text == "")
                //{
                //    lblcusttype.Text = Func.Convert.sConvertToString(drpcusttype.SelectedValue);
                //}


                //iMonthID = Func.Convert.iConvertToInt(drpMonthSelection.SelectedValue);
                //iModelCategory = Func.Convert.iConvertToInt(drpCategorySelection.SelectedValue);
                //lblStatusID.Text = Func.Convert.sConvertToString(DrpStatus.SelectedValue);
                //Sujata 02042011

                ControlDisplay(iUserRollId);

                //vhpj
                //ControlDisable();
                if (!IsPostBack)
                {
                    iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                    //Sujata 15032011 Add Code for tree display
                    PopulateReportGroupTreeMenu(iMenuId, TreeReportGroup);

                    string node = "";
                    node = TreeReportGroup.Nodes[0].Value.ToString();
                    lblTitle.Text = TreeReportGroup.Nodes[0].Text.ToString() + " Report List";
                    //node="1";
                    TreeBasedReportOption(node);
                    //Func.Common.FillRadioButtonList(RadioButtonList1, clsCommon.ComboQueryType.ReportList, 0, " and Report_Menu_id=" + Func.Convert.sConvertToString(iMenuId));
                    //Sujata 15032011

                    //To fill Drowp down list 
                    Func.Common.BindDataToCombo(drpMonthSelection, clsCommon.ComboQueryType.MonthCode, 0);
                    Func.Common.BindDataToCombo(drpDealer, clsCommon.ComboQueryType.DealerAll, 0, " and Dealer_Origin='" + lblDelOrg.Text + "'");
                    Func.Common.BindDataToCombo(drpcusttype, clsCommon.ComboQueryType.CustomerType, 0);
                    //Sujata 20042011
                    //Sujata 23052014
                    if (iMenuId == 432 || iMenuId == 499 || iMenuId == 431 || iMenuId == 32 || iMenuId == 213 || iMenuId == 405 || iMenuId == 422 || iMenuId == 26 || iMenuId == 513 || iMenuId == 514)
                    {
                        Func.Common.BindDataToCombo(DrpBasModCatID, clsCommon.ComboQueryType.ReportModelCat, 0, " and ((ID <>0) and (ID <> 4))");
                        DrpBasModCatID.SelectedIndex = 1;
                        DrpBasModCatID.Visible = (iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16 || iUserRollId == 8) ? true : false;
                        Label3.Visible = (iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16 || iUserRollId == 8) ? true : false;
                        DrpBasModCatID.Visible = false;
                        Label3.Visible = false;
                    }
                    else
                    {
                        Func.Common.BindDataToCombo(DrpBasModCatID, clsCommon.ComboQueryType.ReportModelCat, 0, " and ID =0");
                        DrpBasModCatID.SelectedIndex = 1;
                        DrpBasModCatID.Visible = false;
                        Label3.Visible = false;
                    }
                    //Sujata 23052014
                    Func.Common.BindDataToCombo(DrpReportOption, clsCommon.ComboQueryType.ReportOption, 0, " and ReportOrigin='" + lblDelOrg.Text + "'");
                    DrpReportOption.SelectedValue = "1";
                    //Sujata 27082011
                    //if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 )
                    //if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16 )
                    //Megha 19092011
                    //vrushali 28102013
                    if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16 || iUserRollId == 8 || iUserRollId == 19)
                    //Megha 19092011
                    //Sujata 27082011
                    {
                        DrpReportOption.Visible = false;
                        lblRptOptionLbl.Visible = false;
                    }
                    else
                    {
                        DrpReportOption.Visible = true;
                        lblRptOptionLbl.Visible = true;
                    }
                    //Sujata 20042011
                    if (iUserRollId == 1)
                    {
                        Func.Common.BindDataToCombo(drpRegionSelection, clsCommon.ComboQueryType.RegionUserWise, 0, " and Domestic_Export='" + lblDelOrg.Text + "'");
                    }
                    else if (iUserRollId == 2)
                    {
                        Func.Common.BindDataToCombo(drpRegionSelection, clsCommon.ComboQueryType.RegionUserWise, 0, " and ID in (select distinct RegionID from M_Sys_UserPermissions where Userid=" + Func.Convert.iConvertToInt(Session["UserID"]) + ")");
                    }
                    else if (iUserRollId == 3)
                    {
                        Func.Common.BindDataToCombo(drpCountrySelection0, clsCommon.ComboQueryType.CountryUserWise, 0, " and ID in (select distinct CountryId from M_Sys_UserPermissions where Userid=" + Func.Convert.iConvertToInt(Session["UserID"]) + ")");
                    }
                    //To fill Drowp down list 
                    Session["ParamterList"] = null;
                    //Sujata 20022011
                    ReportwiseFillCombo();
                    //Sujata 20022011
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        public void ControlDisable()
        {
            //drpRegionSelection.Enabled = false;
            //drpCSMSelection.Enabled = false;
            //drpRegionSelection.Enabled = false;
            //drpCountrySelection0.Enabled = false;
            //drpMonthSelection.Enabled = false;
            //drpCategorySelection.Enabled = false;
            //txtFromDate.Enabled = false;
            //drpCategorySelection.Enabled = false;
            //txtToDate.Enabled = false;

        }

        public void ControlDisplay(int iUsrRlid)
        {
            try
            {
                if (iUsrRlid == 1 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 31)
                {
                    drpRegionSelection.Enabled = true;
                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 31)
                    {
                        drpCountrySelection0.Enabled = true;
                        drpCSMSelection.Enabled = true;
                    }
                }
                else if (iUsrRlid == 2)
                {
                    drpRegionSelection.Enabled = true;
                    drpCountrySelection0.Enabled = true;
                    drpCSMSelection.Enabled = true;
                }
                else if (iUsrRlid == 3)
                {
                    drpRegionSelection.Enabled = false;
                    drpCountrySelection0.Enabled = true;
                    drpCSMSelection.Enabled = true;
                }
                else if (iUsrRlid == 4)
                {
                    drpRegionSelection.Enabled = false;
                    drpCountrySelection0.Enabled = false;
                    drpCSMSelection.Enabled = false;
                }
                //Megha 19092011
                else if (iUsrRlid == 8)
                {
                    DrpReportOption.Visible = false;
                    lblRptOptionLbl.Visible = false;
                }
                //Megha 19092011

                //drpMonthSelection.Enabled = Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 5 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 9 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 6 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 7 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 3 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 4 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 10 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 12 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 26 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 30 ? true : false;
                //drpMonthSelection.Enabled = (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) >= 13 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) <= 25) ? false : true;
                //txtFromDate.Enabled = (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) >= 13 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) <= 22) || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 24 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 25 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 10 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 12 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 30 ? true : false;
                //txtToDate.Enabled = (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) >= 13 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) <= 22) || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 24 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 25 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 10 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 12 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 30 ? true : false;
                //drpCategorySelection.Enabled = Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) >= 26 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) <= 29 ? true : false;

                //Sujata 19042011
                //if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 0)
                //{
                //    GetReportOptionDtl();
                //}
                //Sujata 19042011
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        public Microsoft.Reporting.WebForms.ReportParameter[] lstParameters(int ReportID, int iParaCount)
        {

            Microsoft.Reporting.WebForms.ReportParameter[] parm;
            DataTable DtRptControl = null;
            DataSet dsRptDtl = new DataSet();
            clsReport objReport = new clsReport();
            string sParaName;
            string sParaValCntrl;
            string sParaValCntrlCubeExpr;
            string sParameterValue;

            parm = new Microsoft.Reporting.WebForms.ReportParameter[iParaCount];
            //Sujata 19042011
            //dsRptDtl = objReport.SetReportParameters(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue));
            dsRptDtl = objReport.SetReportParameters(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue), Func.Convert.iConvertToInt(lblRportHG.Text));
            //Sujata 19042011
            DtRptControl = dsRptDtl.Tables[0];
            if (DtRptControl.Rows.Count == 0)
            {

            }
            else
            {
                //This is code for set all parameter setting values.
                for (int i = 0; i <= iParaCount - 1; i++)
                {
                    sParaName = Func.Convert.sConvertToString(DtRptControl.Rows[i]["ParaName"]);
                    if (Func.Convert.bConvertToBoolean(DtRptControl.Rows[i]["ParaValFlg"]) == false)
                    {
                        sParaValCntrl = Func.Convert.sConvertToString(DtRptControl.Rows[i]["ParaValCntrl"]);
                        sParaValCntrlCubeExpr = Func.Convert.sConvertToString(DtRptControl.Rows[i]["RptCubeExpr"]);
                        Label lblCntrl = new Label();
                        lblCntrl = (WebControl)PlaceHolder1.FindControl(sParaValCntrl) as Label;
                        sParameterValue = Func.Convert.sConvertToString(lblCntrl.Text);
                        if (sParaValCntrlCubeExpr != "")
                        {
                            if (sParaValCntrl != "lblLoginUserName")
                            {
                                if (lblCntrl.Text == "All")
                                {
                                    sParameterValue = sParaValCntrlCubeExpr + ".[" + sParameterValue + "]";
                                }
                                else
                                {
                                    sParameterValue = sParaValCntrlCubeExpr + ".&[" + sParameterValue + "]";
                                }
                            }
                            else if (sParaValCntrl == "lblLoginUserName" && (iUserRollId == 6 || iUserRollId == 5))
                            {
                                //Sujata 02082011
                                //sParameterValue = sParaValCntrlCubeExpr + ".&[" + lblDealerID.Text + "]";
                                if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 50 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 54 && (sParaValCntrlCubeExpr != "Dealer"))
                                {
                                    sParameterValue = sParaValCntrlCubeExpr + ".&[" + lblDealerID.Text + "]";
                                }
                                else
                                {
                                    sParameterValue = lblDealerID.Text;
                                }
                                //Sujata 02082011
                            }
                        }

                        //parm[i] = new Microsoft.Reporting.WebForms.ReportParameter(sParaName, Func.Convert.sConvertToString(lblCntrl.Text), false);
                        parm[i] = new Microsoft.Reporting.WebForms.ReportParameter(sParaName, Func.Convert.sConvertToString(sParameterValue), false);
                    }
                    else
                    {
                        parm[i] = new Microsoft.Reporting.WebForms.ReportParameter(sParaName, Func.Convert.sConvertToString(DtRptControl.Rows[i]["ParaValue"]), false);
                    }
                }
                //This is code for set all parameter setting values.
            }
            return parm;
        }

        public void ReportView(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, Microsoft.Reporting.WebForms.ReportParameter[] parm)
        {
            try
            {
                string sRptName = "";

                string strreportURL = "";
                string[] sRptNamePath = sReportName.Split('/');
                sRptName = sRptNamePath[sRptNamePath.Length - 1];
                //strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                //string strreportURL = "'" + strReportServer + "/Pages/ReportViewer.aspx?%2fDCSReports%2f";
                //strreportURL=strreportURL +sRptName+"&rs%3aCommand=Render";

                Hashtable hashTblParalist = new Hashtable();
                Session["ParamterList"] = null;
                foreach (Microsoft.Reporting.WebForms.ReportParameter item in parm)
                {
                    hashTblParalist.Add(item.Name, item.Values[0]);
                    //strreportURL = strreportURL + "&" + item.Name + "=" + item.Values[0];        
                }
                Session["ParamterList"] = hashTblParalist;

                //sRptName = "/DCSReports/" + sRptName;
                // strreportURL = "'/DCS/Forms/Common/frmDocumentView.aspx" + "?RptName=" + sRptName + "'";
                strreportURL = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]) + "/" + sRptName;
                strreportURL = "'" + strreportURL + "'";
                Page.RegisterStartupScript("Close", "<script language='javascript'>ShowReports(" + strreportURL + ");</script>");
                //Page.RegisterStartupScript("Close", "<script language='javascript'>ShowReports("+strreportURL+");</script>");
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15)
            // if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16)
            //Megha 19092011
            if (iUserRollId == 5 || iUserRollId == 6 || iUserRollId == 9 || iUserRollId == 10 || iUserRollId == 11 || iUserRollId == 12 || iUserRollId == 13 || iUserRollId == 14 || iUserRollId == 15 || iUserRollId == 16 || iUserRollId == 8)
            {
                DrpReportOption.SelectedValue = "1";
                DrpReportOption.Visible = false;
                lblRptOptionLbl.Visible = false;
            }
            //Megha 19092011
            //else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 41 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 50 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 54 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 57 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 58 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 59 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 80 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 8 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 17 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 74 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 75 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 22 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 23 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 7 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 82 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 83)
            else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 48 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 103 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 62 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 63
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 64 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 84 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 85 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 82
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 86 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 87 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 83 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 88
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 22 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 23 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 83 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 102
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 66 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 67 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 68 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 69
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 73 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 46 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 47 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 45
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 50 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 61 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 27 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 2
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 3 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 7 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 8 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 9
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 10 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 11 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 17 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 74
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 75 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 78 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 79 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 81
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 26 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 40 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 43 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 19
                    || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 80)
            {
                DrpReportOption.SelectedValue = "1";
                DrpReportOption.Visible = false;
                lblRptOptionLbl.Visible = false;
            }
            else
            {
                DrpReportOption.Visible = true;
                lblRptOptionLbl.Visible = true;
            }
            ReportwiseFillCombo();
        }

        private void GetReportOptionDtl()
        {
            DataTable DtRptControl = null;
            DataSet dsRptDtl = new DataSet();

            MANART_BAL.clsReport objReport = new MANART_BAL.clsReport();
            //Sujata 19042011
            //dsRptDtl = objReport.SetReportControls(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue),iMenuId);
            dsRptDtl = objReport.SetReportControls(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue), iMenuId, Func.Convert.iConvertToInt(lblRportHG.Text));
            //Sujata 19042011

            DtRptControl = dsRptDtl.Tables[0];
            if (DtRptControl.Rows.Count == 0)
            {

            }
            //This is code for set all controls setting values.
            iParaCount = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["NoOfPara"]);

            lblMonth.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["Month"]);
            drpMonthSelection.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["Month"]);

            //lblFrmDt.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);
            //txtFromDate.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);

            //lblToDt.Visible= Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);
            //txtToDate.Visible= Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);

            LblRegion.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);
            drpRegionSelection.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);

            LblCountry.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);
            drpCountrySelection0.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);

            lblcusttypeid.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["CustType"]);
            drpcusttype.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["CustType"]);

            LblCSM.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);
            drpCSMSelection.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsLvl"]);

            lblModCat.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["ModCat"]);
            drpCategorySelection.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["ModCat"]);

            lblDealer.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["Dealer"]);
            drpDealer.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["Dealer"]);


            lblUsrEnter.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["UserEnter"]);
            txtUserVal.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["UserEnter"]);
            //vrushali20012011_Begin
            lblUsrEnter.Text = Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 51 ? "Chassis No" : "Please Enter Value For N";
            //vrushali20012011_End
            //lblFrmDt.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["ASOn"]) || Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);
            //txtFromDate.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["ASOn"]) || Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["FromToDt"]);
            //vrushali20012011_Begin
            lblFrmDt.Text = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["ASOn"]) == true ? "As On" : "From Date";
            //vrushali20012011_End

            lblStatus.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsStatus"]);
            DrpStatus.Visible = Func.Convert.bConvertToBoolean(DtRptControl.Rows[0]["IsStatus"]);

            //This is code for set all controls setting values.      
        }

        private string DisplayCurrentRecord()
        {
            DataTable dtDetails = null;
            DataSet ds = new DataSet();

         //  MANART_BAL.clsReport objReport = new MANART_BAL.clsReport();

            //Sujata 19042011
            //ds = objReport.SetReportURL(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue));
          //  ds = objReport.SetReportURL(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue), Func.Convert.iConvertToInt(lblRportHG.Text));
            //Sujata 19042011

            dtDetails = ds.Tables[0];
            if (dtDetails.Rows.Count == 0)
            {
                return "";
            }
            return Func.Convert.sConvertToString(dtDetails.Rows[0]["ReportUrl"]);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                string sRptName = "";

                lblReportOptNo.Text = Func.Convert.sConvertToString(RadioButtonList1.SelectedValue);

                txtFromDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + "01"); //Final Correct Statement.
                txtToDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + (DateTime.Now.Day));
                lblFromDate.Text = Func.Convert.sConvertToString(txtFromDate.Text);
                lblToDate.Text = Func.Convert.sConvertToString(txtToDate.Text);
                lblAreaID.Text = Func.Convert.iConvertToInt(lblCSMID.Text) > 0 ? lblCSMID.Text : (Func.Convert.iConvertToInt(lblCountryID.Text) > 0) ? lblCountryID.Text : lblRegionID.Text;
                lblUserEnter.Text = Func.Convert.sConvertToString(txtUserVal.Text);

                //lblDealerID.Text = Func.Convert.sConvertToString(drpDealer.SelectedValue);

                if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 16 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 17 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 19 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 20)
                {
                    lblRptOptn.Text = Func.Convert.iConvertToInt(lblCSMID.Text) > 0 ? "C" : (Func.Convert.iConvertToInt(lblCountryID.Text) > 0) ? "S" : Func.Convert.iConvertToInt(lblRegionID.Text) > 0 ? "H" : (iUserRollId == 1) ? "H" : iUserRollId == 2 ? "S" : iUserRollId == 3 ? "S" : iUserRollId == 4 ? "C" : "D";
                }
                else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 70)
                {
                    lblRptOptn.Text = Func.Convert.iConvertToInt(lblCSMID.Text) > 0 ? "D" : (Func.Convert.iConvertToInt(lblCountryID.Text) > 0) ? "C" : Func.Convert.iConvertToInt(lblRegionID.Text) > 0 ? "S" : (iUserRollId == 1) ? "H" : iUserRollId == 2 ? "S" : iUserRollId == 3 ? "S" : iUserRollId == 4 ? "C" : "D";
                }
                else
                {
                    //Vrushali18032011_Begin
                    //lblRptOptn.Text = Func.Convert.iConvertToInt(lblCSMID.Text) > 0 ? "D" : (Func.Convert.iConvertToInt(lblCountryID.Text) > 0) ? "C" : Func.Convert.iConvertToInt(lblRegionID.Text) > 0 ? "A" : (iUserRollId == 1) ? "H" : iUserRollId == 2 ? "A" : iUserRollId == 3 ? "A" : iUserRollId == 4 ? "C" : "D";
                    lblRptOptn.Text = (iUserRollId == 1) ? "H" : iUserRollId == 2 ? "A" : iUserRollId == 3 ? "C" : iUserRollId == 4 ? "D" : "E";
                    //Vrushali18032011_end
                }


                if (bValidateRecord() == true)
                {
                    //GetReportOptionDtl();

                    sRptName = DisplayCurrentRecord();

                    if (sRptName == "")
                    {
                        Page.RegisterStartupScript("Close", "<script language='javascript'>alert('No Report Linked.');</script>");
                    }
                    else
                    {
                        ReportView(ReportViewer1, Func.Convert.sConvertToString(sRptName), lstParameters(Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue), iParaCount));
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        public void ReportwiseFillCombo()
        {
            try
            {
                //Sujata 20022011
                if (iUserRollId == 5)
                {
                    LblRegion.Visible = false;
                    LblCountry.Visible = false;
                    LblCSM.Visible = false;
                    lblDealer.Visible = false;

                    drpRegionSelection.Visible = false;
                    drpCSMSelection.Visible = false;
                    drpRegionSelection.Visible = false;
                    drpCountrySelection0.Visible = false;
                    drpDealer.Visible = false;
                }
                //Sujata 20022011
                if (iMenuId == 422) //For Vehicle Reports  Menu
                {
                    if (iUserRollId == 1 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 31)
                    {
                        Func.Common.BindDataToCombo(drpRegionSelection, clsCommon.ComboQueryType.RegionUserWise, 0, " and Domestic_Export='" + lblDelOrg.Text + "'");
                    }
                    else if (iUserRollId == 2)
                    {
                        Func.Common.BindDataToCombo(drpRegionSelection, clsCommon.ComboQueryType.RegionUserWise, 0, " and ID in (select distinct RegionID from M_Sys_UserPermissions where Userid=" + Func.Convert.iConvertToInt(Session["UserID"]) + ")");
                    }
                    else if (iUserRollId == 3)
                    {
                        Func.Common.BindDataToCombo(drpCountrySelection0, clsCommon.ComboQueryType.CountryUserWise, 0, " and ID in (select distinct CountryId from M_Sys_UserPermissions where Userid=" + Func.Convert.iConvertToInt(Session["UserID"]) + ")");
                    }

                    Func.Common.BindDataToCombo(drpMonthSelection, clsCommon.ComboQueryType.MonthCode, 0);

                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 28 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 29)
                    {
                        Func.Common.BindDataToCombo(drpCategorySelection, clsCommon.ComboQueryType.ModelCategory, 0);
                    }
                }
                else if (iMenuId == 426) //For Spare Reports  Menu
                {
                    //Control Disable
                    ControlDisable();
                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 32)
                    {

                    }
                    else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 33 || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 38
                            || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 40
                            || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 45)
                    {
                        // txtFromDate.Text=
                        txtFromDate.Enabled = true;
                        txtToDate.Enabled = true;

                        //Sujata 24032011
                        //txtFromDate.Text = Func.Common.sGetCurrentDate(0, false);
                        //txtToDate.Text = Func.Common.sGetCurrentDate(0, false);
                    }
                    else if (iMenuId == 431) //For Service Reports  Menu
                    {
                        //Control Disable
                        ControlDisable();
                        if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 32)
                        {

                        }
                        else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 55 ||
                                   Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 56 ||
                                   Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 61 ||
                                   Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 62 ||
                                     Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 81

                                 )
                        {
                            // txtFromDate.Text=
                            txtFromDate.Enabled = true;
                            txtToDate.Enabled = true;

                            //Sujata 24032011
                            //txtFromDate.Text = Func.Common.sGetCurrentDate(0, false);
                            //txtToDate.Text = Func.Common.sGetCurrentDate(0, false);
                        }

                        else if (iMenuId == 432) //For Warranty Reports  Menu
                        {
                            //Control Disable
                            ControlDisable();
                            if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 32)
                            {

                            }
                            else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 51 ||
                                      Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 57 ||
                                     Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 82)
                            {
                                // txtFromDate.Text=
                                txtFromDate.Enabled = true;
                                txtToDate.Enabled = true;
                                //Sujata 24032011
                                //txtFromDate.Text = Func.Common.sGetCurrentDate(0, false);
                                //txtToDate.Text = Func.Common.sGetCurrentDate(0, false);
                            }
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        protected void drpRegionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblRegionID.Text = Func.Convert.sConvertToString(drpRegionSelection.SelectedValue);
            lblcusttype.Text = Func.Convert.sConvertToString(drpcusttype.SelectedValue);

            if (lblDelOrg.Text == "E")
            {
                Func.Common.BindDataToCombo(drpCountrySelection0, clsCommon.ComboQueryType.CountryForRegion, Func.Convert.iConvertToInt(lblRegionID.Text));
            }
            else
            {
                Func.Common.BindDataToCombo(drpCountrySelection0, clsCommon.ComboQueryType.StateForRegion, Func.Convert.iConvertToInt(lblRegionID.Text));
            }

        }
        protected void drpcusttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblcusttype.Text = Func.Convert.sConvertToString(drpcusttype.SelectedValue);
            if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 70)
            {
                Func.Common.BindDataToCombo(drpcusttype, clsCommon.ComboQueryType.CustomerType, 0);
            }

        }

        protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatusID.Text = Func.Convert.sConvertToString(DrpStatus.SelectedValue);
        }

        protected void drpDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDealerID.Text = Func.Convert.sConvertToString(drpDealer.SelectedValue);
        }

        protected void drpMonthSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMonthID.Text = Func.Convert.sConvertToString(drpMonthSelection.SelectedValue);
        }

        protected void drpCSMSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void drpCountrySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCountryID.Text = Func.Convert.sConvertToString(drpCountrySelection0.SelectedValue);
            if (lblDelOrg.Text == "E")
            {
                Func.Common.BindDataToCombo(drpCSMSelection, clsCommon.ComboQueryType.CSMList, 0, " and CountryId=" + Func.Convert.sConvertToString(lblCountryID.Text) + " and UserRole=4");
            }
            else
            {
                Func.Common.BindDataToCombo(drpCSMSelection, clsCommon.ComboQueryType.CSMList, 0, " and StateID=" + Func.Convert.sConvertToString(lblCountryID.Text) + " and UserRole=4");
            }
        }

        private bool bValidateRecord()
        {

            string sMessage = "";
            bool bValidateRecord = true;

            txtFromDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + "01"); //Final Correct Statement.
            txtToDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + (DateTime.Now.Day));

            if (lblRportHG.Text == "")
            {
                lblRportHG.Text = "1";
            }

            //Sujata 19042011
            if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 0)
            {
                GetReportOptionDtl();
            }
            UserSetting();
            //Sujata 19042011

            if (iUserRollId == 2)
            {
                if (drpRegionSelection.SelectedValue == "0")
                {
                    //sMessage = sMessage + "\\n Please Select Region";
                    //bValidateRecord = false;                
                }
            }
            if (iUserRollId == 3)
            {
                if (drpCountrySelection0.SelectedValue == "0")
                {
                    //sMessage = sMessage + "\\n Please Select Country";
                    //bValidateRecord = false;
                }
            }
            if (iUserRollId == 4)
            {
                if (drpCSMSelection.SelectedValue == "0")
                {
                    //sMessage = sMessage + "\\n Please Select Region";
                    //bValidateRecord = false;
                }
            }
            if (lblMonth.Visible == true && drpMonthSelection.SelectedValue == "0")
            {
                //sMessage = sMessage + "\\n Please Select Month";
                // bValidateRecord = false;
            }


            //if ((lblFrmDt.Visible == true || txtFromDate.Text == "") && (lblToDt.Visible == true && txtToDate.Text ==""  ))
            //{
            //    //sMessage = sMessage + "\\n Please Select Date";
            //    //bValidateRecord = false;
            //    //txtFromDate.Text = Func.Convert.sConvertToString(DateTime.Today);
            //    //txtFromDate.Text =   Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + "01"); //Final Correct Statement.
            //    //txtToDate.Text = Func.Convert.sConvertToString(DateTime.Today);

            //}        

            if (lblModCat.Visible == true && Func.Convert.iConvertToInt(drpCategorySelection.SelectedValue) == 0)
            {
                //sMessage = sMessage + "\\n Please Select To Model Catergory";
                //bValidateRecord = false;
            }

            if (lblUsrEnter.Visible == true && txtUserVal.Text == "")
            {
                //sMessage = sMessage + "\\n Please Select To Value For N";
                //bValidateRecord = false;
            }

            if (lblStatus.Visible == true && Func.Convert.sConvertToString(DrpStatus.SelectedValue) == "")
            {
                //sMessage = sMessage + "\\n Please Select Status";
                //bValidateRecord = false;
            }

            if (bValidateRecord == false)
            {
                //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }

            return bValidateRecord;
        }

        //Sujata 15032011
        public void PopulateReportGroupTreeMenu(int iMenuID, TreeView Tree)
        {

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            MANART_DAL.clsDB objDB = new MANART_DAL.clsDB();
            try
            {
                DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ReportTree", iMenuID);
                string sMenuName = "";
                string sMenuValue = "";
                string sMenuURl = "";
                string sMenuToolTip = "";
                int iParentID = 0;
                TreeNode Currrmenu;
                TreeNode Childmenu;
                string sParentID = "0";
                string sMenuId = "0";
                //Create Master Menu
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iParentID = Func.Convert.iConvertToInt(dtRow["ParentNode"]);
                    sMenuName = Func.Convert.sConvertToString(dtRow["NodeName"]);
                    sMenuId = Func.Convert.sConvertToString(dtRow["ID"]);
                    sMenuValue = Func.Convert.sConvertToString(dtRow["ID"]);
                    sMenuURl = Func.Convert.sConvertToString(dtRow["NodeName"]);
                    sMenuToolTip = Func.Convert.sConvertToString(dtRow["NodeName"]);
                    Currrmenu = new TreeNode(sMenuName, sMenuValue);
                    Currrmenu.ToolTip = sMenuToolTip;
                    Currrmenu.Target = sMenuId;
                    Tree.Nodes.Add(Currrmenu);
                }
                //Create Child Menu

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    sParentID = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["ParentNode"]);
                    sMenuName = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["NodeName"]);
                    sMenuValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["ID"]);
                    sMenuId = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["ID"]);
                    sMenuURl = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["NodeName"]);
                    sMenuToolTip = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["NodeName"]);
                    Childmenu = new TreeNode(sMenuName, sMenuValue);
                    Childmenu.Target = sMenuId;
                    Childmenu.ToolTip = sMenuToolTip;
                    if (Tree.FindNode(sParentID) != null)
                    {
                        Tree.FindNode(sParentID).ChildNodes.Add(Childmenu);
                    }
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
        //Sujata 15032011
        //Sujata 16032011
        protected void TreeReportGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            string node = "";
           // node = TreeReportGroup.SelectedNode.Value.ToString();
            //lblTitle.Text = TreeReportGroup.SelectedNode.Text.ToString() + " Report List";
            TreeBasedReportOption(node);
        }
        //Sujata 16032011
        public void TreeBasedReportOption(string ParentNode)
        {
            //Func.Common.FillRadioButtonList(RadioButtonList1, clsCommon.ComboQueryType.ReportList, 0, " and Report_Menu_id=" + Func.Convert.sConvertToString(iMenuId) + " and ParentNode='"+ ParentNode +"'");
            Func.Common.FillRadioButtonList(RadioButtonList1, clsCommon.ComboQueryType.ReportList, 0, " and Report_Menu_id=" + Func.Convert.sConvertToString(iMenuId) + " and ParentNode='" + ParentNode + "' and ReportHierachy=1");
        }

        public void UserSetting()
        {
            DataTable DtRptControl = null;
            DataSet dsRptDtl = new DataSet();
            MANART_BAL.clsReport objReport = new MANART_BAL.clsReport();
            try
            {
                //int iHeirarchy=0;
                //Sujata 19042011
                //lblHierarchy.Text =(iUserRollId == 4) ? "2":"1";
                lblHierarchy.Text = lblRportHG.Text;
                int iUsrRegionRSMid = 0;
                int iUsrStateASMid = 0;
                int iUsrCountryASMid = 0;
                int iUsrCSMid = 0;
                int iUsrDealerid = 0;
                //Sujata 19042011


                //Sujata 30042011
                lblUserRole.Text = Func.Convert.sConvertToString(Session["UserRole"]);
                //Sujata 30042011
                int iDeptID = 0;
                lblLogUserID.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(Session["UserID"]));


                //if (iMenuId == 432 || iMenuId == 499 || iMenuId == 431 || iMenuId == 32) iDeptID = 8;
                if (iMenuId == 432 || iMenuId == 499 || iMenuId == 431 || iMenuId == 32) iDeptID = 7;
                if (iMenuId == 213 || iMenuId == 405) iDeptID = 1;
                if (iMenuId == 219 || iMenuId == 419) iDeptID = 2;
                if (iMenuId == 437 || iMenuId == 420) iDeptID = 3;
                //Sujata 28112012_Begin
                //if (iMenuId == 426 ) iDeptID = 6;
                if (iMenuId == 426 || iMenuId == 28) iDeptID = 6;
                //Sujata 28112012_End
                if (iMenuId == 422 || iMenuId == 26) iDeptID = 5;
                //Branch Report Menu 
                if (iMenuId == 511 || iMenuId == 555) iDeptID = 6; //spare
                if (iMenuId == 513) iDeptID = 5;//vehicle
                if (iMenuId == 514) iDeptID = 7;//Warranty
                                                //Branch Report Menu 
                lblDeptID.Text = Func.Convert.sConvertToString(iDeptID);

                //Sujata 25052011
                //Sujata 27082011
                //if (Func.Convert.iConvertToInt(lblUserRole.Text) == 9 || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || Func.Convert.iConvertToInt(lblUserRole.Text) == 15)
                //Sujata 29102012_Begin add for 18
                if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 61)
                    lblWarrClaimType.Text = Func.Convert.iConvertToInt(lblUserRole.Text) == 18 ? "10" : "0";
                else lblWarrClaimType.Text = Func.Convert.iConvertToInt(lblUserRole.Text) == 18 ? "SA" : "All";

                //if (Func.Convert.iConvertToInt(lblUserRole.Text) == 9 || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || Func.Convert.iConvertToInt(lblUserRole.Text) == 15 || Func.Convert.iConvertToInt(lblUserRole.Text) == 16)
                //if (Func.Convert.iConvertToInt(lblUserRole.Text) == 9 || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || Func.Convert.iConvertToInt(lblUserRole.Text) == 15 || Func.Convert.iConvertToInt(lblUserRole.Text) == 16 || Func.Convert.iConvertToInt(lblUserRole.Text) == 18)
                //if ((Func.Convert.iConvertToInt(lblUserRole.Text) == 9 && iDeptID != 5) || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || Func.Convert.iConvertToInt(lblUserRole.Text) == 15  || Func.Convert.iConvertToInt(lblUserRole.Text) == 16 || Func.Convert.iConvertToInt(lblUserRole.Text) == 18 || Func.Convert.iConvertToInt(lblUserRole.Text) == 19)
                //  if ((Func.Convert.iConvertToInt(lblUserRole.Text) == 9 && iDeptID != 5) || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || (Func.Convert.iConvertToInt(lblUserRole.Text) == 15 && (iMenuId != 508 || (iMenuId == 508 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 105))) || Func.Convert.iConvertToInt(lblUserRole.Text) == 16 || Func.Convert.iConvertToInt(lblUserRole.Text) == 18 || Func.Convert.iConvertToInt(lblUserRole.Text) == 19)
                if ((Func.Convert.iConvertToInt(lblUserRole.Text) == 9 && iDeptID != 5) || Func.Convert.iConvertToInt(lblUserRole.Text) == 10 || Func.Convert.iConvertToInt(lblUserRole.Text) == 11 || Func.Convert.iConvertToInt(lblUserRole.Text) == 12 || Func.Convert.iConvertToInt(lblUserRole.Text) == 13 || Func.Convert.iConvertToInt(lblUserRole.Text) == 14 || (Func.Convert.iConvertToInt(lblUserRole.Text) == 15 && iMenuId != 508) || Func.Convert.iConvertToInt(lblUserRole.Text) == 16 || Func.Convert.iConvertToInt(lblUserRole.Text) == 18 || Func.Convert.iConvertToInt(lblUserRole.Text) == 19)
                //Sujata 29102012_End add for 18
                //Sujata 27082011
                {
                    //dsRptDtl = objReport.SetHeadLoginNameSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), (iDeptID == 8 ? 7 : iDeptID));
                    int iBasModCatID = 0;
                    iBasModCatID = Func.Convert.iConvertToInt(DrpBasModCatID.SelectedValue);
                   // dsRptDtl = objReport.SetHeadLoginNameSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), (iDeptID == 8 || iDeptID == 0 ? 7 : (iDeptID == 2 || iDeptID == 3 ? 3 : iDeptID)), iBasModCatID);
                    DtRptControl = dsRptDtl.Tables[0];
                    lblLogUserID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    lblUserRole.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["UserRole"]);
                }
                if (lblUserRole.Text == "19") // Branch Report User role
                {
                    lblDrlFrLocalData.Text = lblLogUserID.Text;
                }
                else
                {
                    //Sujata 25052011
                    //Sujata 23052012
                    //dsRptDtl = objReport.SetUserLoginNameSetting(Func.Convert.iConvertToInt(lblLogUserID.Text));
                    // dsRptDtl = objReport.SetUserLoginNameSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.iConvertToInt(iDeptID), Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue));
                    dsRptDtl = objReport.SetUserLoginNameSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.iConvertToInt(iDeptID));
                    //Sujata 23052012
                    DtRptControl = dsRptDtl.Tables[0];
                    if ((Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 2
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 9
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 10
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 11
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 12
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 78
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 79
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 106
                          || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 3
                          || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 122
                        || Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 2) &&
                        (iDeptID == 6) && (Func.Convert.iConvertToInt(lblUserRole.Text) != 5 && Func.Convert.iConvertToInt(lblUserRole.Text) != 6))
                    {
                        //lblLoginUserName.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["LoginName"]);
                        lblLoginUserName.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ServiceLoginName"]);
                        lblUserRole.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["UserRole"]);
                    }
                    else
                    {
                        lblLoginUserName.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ServiceLoginName"]);
                    }
                    if (Func.Convert.iConvertToInt(lblUserRole.Text) == 9 && iDeptID == 5) lblUserRole.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["UserRole"]);

                    lblLoginServiceUserName.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ServiceLoginName"]);

                    //Sujata 23052012
                    DtRptControl = dsRptDtl.Tables[1];
                    lblUserDeptID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["DepartmentID"]);
                    //Sujata 23052012

                    //Sujata 25052012 iDeptID
                    //dsRptDtl = objReport.SetUserRegionSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    dsRptDtl = objReport.SetUserRegionSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(lblUserDeptID.Text));
                    //Sujata 25052012
                    DtRptControl = dsRptDtl.Tables[0];
                    //Sujata 19042011
                    //lblRegionID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    lblRegionID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    //Sujata 02082011
                    //iUsrRegionRSMid = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["ID"]);
                    //Sujata 26122011
                    //if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 50 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 54)
                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 50 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 54 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 59 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 61 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 62 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 63 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 64 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 66 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 67 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 68 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 69 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 71 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 72 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 80 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 82)
                    //Sujata 26122011
                    {
                        lblRegionID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    }
                    else
                    {
                        lblRegionID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    }
                    //Sujata 02082011
                    //Sujata 19042011

                    //Sujata 19042011
                    //dsRptDtl = objReport.SetUserStateSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.iConvertToInt(lblRegionID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));            
                    //Sujata 25052012 
                    //dsRptDtl = objReport.SetUserStateSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    dsRptDtl = objReport.SetUserStateSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(lblUserDeptID.Text));
                    //Sujata 25052012 iDeptID
                    DtRptControl = dsRptDtl.Tables[0];
                    //Sujata 26122011
                    //lblStateID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 59 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 66 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 67 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 68 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 69 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 71 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 72 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 80 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 82)
                    {
                        lblStateID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    }
                    else
                    {
                        lblStateID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    }
                    //Sujata 26122011
                    iUsrStateASMid = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["ID"]);
                    //Sujata 19042011            

                    //Sujata 19042011
                    //Sujata 25052012 
                    //dsRptDtl = objReport.SetUserCountrySetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.iConvertToInt(lblRegionID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    //dsRptDtl = objReport.SetUserCountrySetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    dsRptDtl = objReport.SetUserCountrySetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(lblUserDeptID.Text));
                    //Sujata 25052012 
                    //DtRptControl = dsRptDtl.Tables[0];
                    //lblCountryID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);            
                    DtRptControl = dsRptDtl.Tables[0];
                    //Sujata 27042011
                    //lblCountryID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);            
                    if (Func.Convert.sConvertToString(lblDelOrg.Text) == "E" && Func.Convert.iConvertToInt(lblHierarchy.Text) == 2)
                    {
                        lblStateID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    }
                    else
                    {
                        lblCountryID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    }
                    iUsrCountryASMid = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["ID"]);
                    //Sujata 27042011            
                    //Sujata 19042011

                    //Sujata 19042011
                    //dsRptDtl = objReport.SetCSMSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.iConvertToInt(lblRegionID.Text), Func.Convert.iConvertToInt(lblStateID.Text), Func.Convert.iConvertToInt(lblCountryID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    //DtRptControl = dsRptDtl.Tables[0];
                    //lblCSMID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    //Sujata 25052012 
                    //dsRptDtl = objReport.SetCSMSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, iUsrStateASMid, iUsrCountryASMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    dsRptDtl = objReport.SetCSMSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), iUsrRegionRSMid, iUsrStateASMid, iUsrCountryASMid, Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(lblUserDeptID.Text));
                    //Sujata 25052012 
                    DtRptControl = dsRptDtl.Tables[0];
                    lblCSMID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    iUsrCSMid = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["ID"]);
                    //Sujata 19042011

                    //lblCSMID.Text = "0";

                    //Sujata 19042011
                    //dsRptDtl = objReport.SetDealerSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), Func.Convert.iConvertToInt(lblRegionID.Text), Func.Convert.iConvertToInt(lblStateID.Text), Func.Convert.iConvertToInt(lblCountryID.Text), Func.Convert.iConvertToInt(lblCSMID.Text), Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    //DtRptControl = dsRptDtl.Tables[0];
                    //lblDealerID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    //Sujata 25052012 
                    //dsRptDtl = objReport.SetDealerSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), iUsrRegionRSMid, iUsrStateASMid, iUsrCountryASMid, iUsrCSMid, Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(iDeptID));
                    dsRptDtl = objReport.SetDealerSetting(Func.Convert.iConvertToInt(lblLogUserID.Text), Func.Convert.sConvertToString(lblDelOrg.Text), iUsrRegionRSMid, iUsrStateASMid, iUsrCountryASMid, iUsrCSMid, Func.Convert.iConvertToInt(lblHierarchy.Text), Func.Convert.iConvertToInt(lblUserDeptID.Text));
                    //Sujata 25052012 
                    DtRptControl = dsRptDtl.Tables[0];

                    lblDealerID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Name"]);
                    iUsrDealerid = Func.Convert.iConvertToInt(DtRptControl.Rows[0]["ID"]);
                    //Sujata 02082011
                    lblDrlFrLocalData.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    //Sujata 02082011
                    if (lblUserRole.Text == "6" || lblUserRole.Text == "5")
                    {
                        lblDealerType.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Dealer_Type_Des"]);
                        lblDlrDealerID.Text = Func.Convert.sConvertToString(iUsrDealerid);
                    }
                    //Sujata 19042011


                    //SP_GETDealerID 

                    //vrushali26032011_Begin
                    //sujata 17052011
                    //lblWarrClaimType.Text = "0";
                    //Sujata 29102012_Begin
                    //lblWarrClaimType.Text = "All";
                    //Sujata 29102012_End
                    //sujata 17052011
                    //sujata 21052011
                    lblPartClaimType.Text = "All";

                    if (lblUserRole.Text != "6" && lblUserRole.Text != "5")
                    {
                        lblDealerType.Text = "All";
                    }
                    //sujata 21052011
                    //vrushali26032011_End

                    //SetYearSetting
                    //lblYearid.Text = "4";
                    //vrushali02042011_Begin
                    //Sujata 27042011
                    //dsRptDtl = objReport.SetYearSetting(Func.Convert.sConvertToString(DateTime.Now.Year));
                    //DtRptControl = dsRptDtl.Tables[0];
                    //lblYearid.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);

                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 59) // For Last year 
                    {

                        //dsRptDtl = objReport.SetYearSetting(Func.Convert.sConvertToString(DateTime.Now.Year));
                        dsRptDtl = objReport.SetYearSetting(Func.Convert.sConvertToString("2010"));
                        DtRptControl = dsRptDtl.Tables[0];
                        lblYearid.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    }
                    else if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 5) // For Last year 
                    {
                        lblYearid.Text = Func.Convert.sConvertToString(Func.Convert.iConvertToInt(DateTime.Now.Year) - 1);
                    }
                    else
                    {
                        lblYearid.Text = Func.Convert.sConvertToString(DateTime.Now.Year);
                    }
                    //Sujata 27042011
                    //vrushali02042011_End 

                    //vrushali02052011
                    lblCustomerType.Text = "All";
                    lblPOType.Text = "All";
                    lblModCat.Text = "All";
                    lblJobType.Text = "All";

                    //if ((Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 7) || (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 8)
                    if ((Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 8)
                        || (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 26)) // For Last year 
                    {
                        lblFromDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + "01" + "-" + "01"); //Final Correct Statement.
                        lblToDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + (DateTime.Now.Day));

                    }
                    else
                    {
                        lblFromDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + "01"); //Final Correct Statement.
                        lblToDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (DateTime.Now.Month) + "-" + (DateTime.Now.Day));

                    }

                    //lblFrmDate.Text = Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (Func.Convert.sConvertToString(DateTime.Now.Month).Length == 1 ? "0" : "") + (DateTime.Now.Month) + "-" + "01"); //Final Correct Statement.
                    //lblTDate.Text =   Func.Convert.sConvertToString(DateTime.Now.Year + "-" + (Func.Convert.sConvertToString(DateTime.Now.Month).Length == 1 ? "0" : "") + (DateTime.Now.Month) + "-" + (DateTime.Now.Day));

                    //vrushali03042011_Begin
                    // Indent Report display next month filter 

                    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 62 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 63 && Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) != 64)
                    {
                        dsRptDtl = objReport.SetMonthSetting(Func.Convert.iConvertToInt(DateTime.Now.Month));
                    }
                    else
                    {
                        dsRptDtl = objReport.SetMonthSetting(Func.Convert.iConvertToInt(DateTime.Now.Month + 1));
                    }

                    DtRptControl = dsRptDtl.Tables[0];

                    if ((Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 41) || (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 49))
                    {
                        lblMonthID.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["Month_Name"]);
                    }
                    else
                    {
                        lblMonthID.Text = DtRptControl.Rows[0]["Month_Name"] + " - " + Func.Convert.sConvertToString(DateTime.Now.Year);
                    }
                    lblMonthNo.Text = Func.Convert.sConvertToString(DtRptControl.Rows[0]["ID"]);
                    lblYearNo.Text = Func.Convert.sConvertToString(DateTime.Now.Year);
                    //vrushali03042011_End
                }
                DtRptControl = null;
                dsRptDtl = null;
                objReport = null;
            }
            catch (Exception ex)
            {

                //throw;
                Func.Common.ProcessUnhandledException(ex);
                DtRptControl = null;
                dsRptDtl = null;
                objReport = null;
            }

        }
        protected void DrpReportOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHierarchy.Text = DrpReportOption.SelectedValue;
            lblRportHG.Text = (Func.Convert.iConvertToInt(DrpReportOption.SelectedValue) != 2) ? "1" : "2";
        }
    }
}