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
using Microsoft.ReportingServices.ReportRendering;
using Microsoft.Reporting.WebForms;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Net;
using Microsoft.Reporting;

namespace MANART.Forms.Common
{
    public partial class frmDocumentView : System.Web.UI.Page
    {
        int iMenuId = 0;
        string strReportName = "";
        string ExportYesNo = "";
        string RptTitle = "";
        string FPDAYesNo = "";
        string DepoID = "";
        string DealerID = "";
        string RegionID = "";
        string CurrDate = "";
        string ModelId = "";
        string ProdCat = "";
        int iID = 0;
        int UserRoleID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-IN");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-IN");
                if (!IsPostBack)
                {
                    iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                    if (Request.QueryString["RptName"] != null)
                    {
                        strReportName = Request.QueryString["RptName"];
                        ExportYesNo = Request.QueryString["ExportYesNo"];
                        FPDAYesNo = Request.QueryString["FPDAYesNo"];
                        UserRoleID = Func.Convert.iConvertToInt(Request.QueryString["UserRoleId"]);
                        DepoID = Request.QueryString["DepoId"];
                        DealerID = Request.QueryString["DealerID"];
                        RegionID = Request.QueryString["RegionId"];
                        CurrDate = Request.QueryString["CurrDate"];
                        ModelId = Request.QueryString["ModelId"];
                        ProdCat = Request.QueryString["ProdCat"];

                    }
                    //Megha 24012011 FPDA Report
                    if (Request.QueryString["ID"] != null && ExportYesNo == "F")
                    {
                        iID = Func.Convert.iConvertToInt(Request.QueryString["ID"]);
                        //ReportView(ReportViewer1, strReportName, iID.ToString() + FPDAYesNo + UserRoleID);
                        ReportViewFPDA(ReportViewer1, strReportName, iID.ToString(), FPDAYesNo, UserRoleID.ToString());
                    }
                    //Megha 14022012 Indent Cosolidation
                    else if (Request.QueryString["ID"] != null && ExportYesNo == "I")
                    {
                        iID = Func.Convert.iConvertToInt(Request.QueryString["ID"]);
                        //ReportView(ReportViewer1, strReportName, iID.ToString() + FPDAYesNo + UserRoleID);
                        // ReportViewIndentConsolidation(ReportViewer1, strReportName, DepoID, DealerID, CurrDate, ModelId, ProdCat);
                        ReportViewIndentConsolidation(ReportViewer1, strReportName, iID.ToString(), RegionID, DepoID, CurrDate, ProdCat, iMenuId.ToString());
                    }
                    //else if (Request.QueryString["ID"] == null && ExportYesNo == "D")
                    //{
                    else if (Request.QueryString["ID"] != null && ExportYesNo == "D")
                    {
                        iID = Func.Convert.iConvertToInt(Request.QueryString["ID"]);
                        //ReportView(ReportViewer1, strReportName, iID.ToString() + FPDAYesNo + UserRoleID);
                        //  ReportViewDepotIndent(ReportViewer1, strReportName, CurrDate,RegionID);
                        ReportViewDepotIndent(ReportViewer1, strReportName, iID.ToString(), ProdCat);
                    }

                   //Megha 14022012 Indent Cosolidation
                    else if (Request.QueryString["ID"] != null && (FPDAYesNo == "" || FPDAYesNo == null))
                    {
                        iID = Func.Convert.iConvertToInt(Request.QueryString["ID"]);
                        ReportView(ReportViewer1, strReportName, iID.ToString());
                    }

                    else if (Session["ParamterList"] != null && strReportName != null)
                    {
                        ReportViewFromReportWindow(strReportName);
                    }

                }
                // ExpirePageCache();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }



        public Microsoft.Reporting.WebForms.ReportParameter[] parm = new Microsoft.Reporting.WebForms.ReportParameter[1];
        public void ReportView(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1)
        {

            try
            {
                string strReportServer;

                ReportViewer1.ShowCredentialPrompts = false;
                parm[0] = new Microsoft.Reporting.WebForms.ReportParameter("ID", sParam1);


                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment",  "Secure123*",  "DCSSERVER");
                //ReportViewer1.ServerReport.ReportServerCredentials = new  ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //VItthal Report Start
                strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
                //VItthal Report End

                ReportViewer1.ServerReport.ReportPath = sReportName;
                Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                ObjCredential[0].Name = "DataSource1";
                ObjCredential[0].Password = "Secure123*";
                ObjCredential[0].UserId = "sa";
                ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);            

                ReportViewer1.ServerReport.SetParameters(parm);
                ReportViewer1.ServerReport.Refresh();
                //*** Comment Start By Vitthal on 16/03/2010 
                //*** User Want Report View Format After View Report user will select  Export Option for Printing 

                //if (ExportYesNo == "Y")
                //{
                //    string mimeType, encoding, extension, deviceInfo;
                //    string[] streamids;
                //    Microsoft.Reporting.WebForms.Warning[] warnings;
                //    string format = "PDF"; //Desired format goes here (PDF, Excel, or Image)

                //    deviceInfo = "<DeviceInfo> <SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //    byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                //    Response.Clear();

                //    if (format == "PDF")
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("Content-disposition", "filename=output.pdf");
                //    }
                //    else if (format == "Excel")
                //    {
                //        Response.ContentType = "application/excel";
                //        Response.AddHeader("Content-disposition", "filename=output.xls");
                //    }

                //    Response.OutputStream.Write(bytes, 0, bytes.Length);
                //    Response.OutputStream.Flush();
                //    Response.OutputStream.Close();
                //    Response.Flush();
                //    Response.Close();
                //}

                //*** Comment End By Vitthal on 16/03/2010 


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }
        //Megha 24012011 FPDA Report
        public Microsoft.Reporting.WebForms.ReportParameter[] parmFPDA = new Microsoft.Reporting.WebForms.ReportParameter[3];
        public void ReportViewFPDA(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1, string sParam2, string sParam3)
        {

            try
            {
                string strReportServer;




                ReportViewer1.ShowCredentialPrompts = false;
                parmFPDA[0] = new Microsoft.Reporting.WebForms.ReportParameter("ID", sParam1);
                parmFPDA[1] = new Microsoft.Reporting.WebForms.ReportParameter("FPDAYesNo", sParam2);
                parmFPDA[2] = new Microsoft.Reporting.WebForms.ReportParameter("UserRoleId", sParam3);

                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment",  "Secure123*",  "DCSSERVER");
                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //VItthal Report Start
                strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
                //VItthal Report End

                ReportViewer1.ServerReport.ReportPath = sReportName;
                //Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                //ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                //ObjCredential[0].Name = "DataSource1";
                //ObjCredential[0].Password = "Secure123*";
                //ObjCredential[0].UserId = "sa";
                // ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential); 


                ReportViewer1.ServerReport.SetParameters(parmFPDA);
                ReportViewer1.ServerReport.Refresh();
                //*** Comment Start By Vitthal on 16/03/2010 
                //*** User Want Report View Format After View Report user will select  Export Option for Printing 

                //if (ExportYesNo == "Y")
                //{
                //    string mimeType, encoding, extension, deviceInfo;
                //    string[] streamids;
                //    Microsoft.Reporting.WebForms.Warning[] warnings;
                //    string format = "PDF"; //Desired format goes here (PDF, Excel, or Image)

                //    deviceInfo = "<DeviceInfo> <SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //    byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                //    Response.Clear();

                //    if (format == "PDF")
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("Content-disposition", "filename=output.pdf");
                //    }
                //    else if (format == "Excel")
                //    {
                //        Response.ContentType = "application/excel";
                //        Response.AddHeader("Content-disposition", "filename=output.xls");
                //    }

                //    Response.OutputStream.Write(bytes, 0, bytes.Length);
                //    Response.OutputStream.Flush();
                //    Response.OutputStream.Close();
                //    Response.Flush();
                //    Response.Close();
                //}

                //*** Comment End By Vitthal on 16/03/2010 


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }
        //Megha 24012011 FPDA Report

        //Megha14022012 Indent consolidation
        //public Microsoft.Reporting.WebForms.ReportParameter[] parmIndentcon = new Microsoft.Reporting.WebForms.ReportParameter[5];

        //public void ReportViewIndentConsolidation(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1, string sParam2, string sParam3, string sParam4, string sParam5)
        //{

        //    try
        //    {
        //        string strReportServer;

        //        ReportViewer1.ShowCredentialPrompts = false;

        //        parmIndentcon[0] = new Microsoft.Reporting.WebForms.ReportParameter("Depot_ID", sParam1);
        //        parmIndentcon[1] = new Microsoft.Reporting.WebForms.ReportParameter("DealerID", sParam2);
        //        parmIndentcon[2] = new Microsoft.Reporting.WebForms.ReportParameter("MonthDate", sParam3);
        //        parmIndentcon[3] = new Microsoft.Reporting.WebForms.ReportParameter("Model_ID", sParam4);
        //        parmIndentcon[4] = new Microsoft.Reporting.WebForms.ReportParameter("CatID", sParam5);

        //        //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment",  "Secure123*",  "DCSSERVER");
        //        ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
        //        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        //        //VItthal Report Start
        //        strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
        //        ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
        //        //VItthal Report End

        //        ReportViewer1.ServerReport.ReportPath = sReportName;
        //        Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
        //        ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
        //        ObjCredential[0].Name = "DataSource1";
        //        ObjCredential[0].Password = "Secure123*";
        //        ObjCredential[0].UserId = "sa";
        //        ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential); 


        //        ReportViewer1.ServerReport.SetParameters(parmIndentcon);
        //        ReportViewer1.ServerReport.Refresh();
        //        //*** Comment Start By Vitthal on 16/03/2010 
        //        //*** User Want Report View Format After View Report user will select  Export Option for Printing 

        //        //if (ExportYesNo == "Y")
        //        //{
        //        //    string mimeType, encoding, extension, deviceInfo;
        //        //    string[] streamids;
        //        //    Microsoft.Reporting.WebForms.Warning[] warnings;
        //        //    string format = "PDF"; //Desired format goes here (PDF, Excel, or Image)

        //        //    deviceInfo = "<DeviceInfo> <SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

        //        //    byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

        //        //    Response.Clear();

        //        //    if (format == "PDF")
        //        //    {
        //        //        Response.ContentType = "application/pdf";
        //        //        Response.AddHeader("Content-disposition", "filename=output.pdf");
        //        //    }
        //        //    else if (format == "Excel")
        //        //    {
        //        //        Response.ContentType = "application/excel";
        //        //        Response.AddHeader("Content-disposition", "filename=output.xls");
        //        //    }

        //        //    Response.OutputStream.Write(bytes, 0, bytes.Length);
        //        //    Response.OutputStream.Flush();
        //        //    Response.OutputStream.Close();
        //        //    Response.Flush();
        //        //    Response.Close();
        //        //}

        //        //*** Comment End By Vitthal on 16/03/2010 


        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);

        //    }
        //}

        public Microsoft.Reporting.WebForms.ReportParameter[] parmIndentcon = new Microsoft.Reporting.WebForms.ReportParameter[6];

        public void ReportViewIndentConsolidation(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1, string sParam2, string sParam3, string sParam4, string sParam5, string sParam6)
        {

            try
            {
                string strReportServer;

                ReportViewer1.ShowCredentialPrompts = false;


                parmIndentcon[0] = new Microsoft.Reporting.WebForms.ReportParameter("ConIndent_ID", sParam1);
                parmIndentcon[1] = new Microsoft.Reporting.WebForms.ReportParameter("Region_ID", sParam2);
                parmIndentcon[2] = new Microsoft.Reporting.WebForms.ReportParameter("Depo_ID", sParam3);
                parmIndentcon[3] = new Microsoft.Reporting.WebForms.ReportParameter("MonthDate", sParam4);
                parmIndentcon[4] = new Microsoft.Reporting.WebForms.ReportParameter("CatID", sParam5);
                parmIndentcon[5] = new Microsoft.Reporting.WebForms.ReportParameter("MenuID", sParam6);

                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment",  "Secure123*",  "DCSSERVER");               
                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //VItthal Report Start
                strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
                //VItthal Report End

                ReportViewer1.ServerReport.ReportPath = sReportName;
                //Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                //ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                //ObjCredential[0].Name = "DataSource1";
                //ObjCredential[0].Password = "Secure123*";
                //ObjCredential[0].UserId = "sa";
                //ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);


                ReportViewer1.ServerReport.SetParameters(parmIndentcon);
                ReportViewer1.ServerReport.Refresh();
                //*** Comment Start By Vitthal on 16/03/2010 
                //*** User Want Report View Format After View Report user will select  Export Option for Printing 

                //if (ExportYesNo == "Y")
                //{
                //    string mimeType, encoding, extension, deviceInfo;
                //    string[] streamids;
                //    Microsoft.Reporting.WebForms.Warning[] warnings;
                //    string format = "PDF"; //Desired format goes here (PDF, Excel, or Image)

                //    deviceInfo = "<DeviceInfo> <SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //    byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                //    Response.Clear();

                //    if (format == "PDF")
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("Content-disposition", "filename=output.pdf");
                //    }
                //    else if (format == "Excel")
                //    {
                //        Response.ContentType = "application/excel";
                //        Response.AddHeader("Content-disposition", "filename=output.xls");
                //    }

                //    Response.OutputStream.Write(bytes, 0, bytes.Length);
                //    Response.OutputStream.Flush();
                //    Response.OutputStream.Close();
                //    Response.Flush();
                //    Response.Close();
                //}

                //*** Comment End By Vitthal on 16/03/2010 


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }

        ////Megha 12032012 Depot Indent
        public Microsoft.Reporting.WebForms.ReportParameter[] parmDepotIndent = new Microsoft.Reporting.WebForms.ReportParameter[2];

        public void ReportViewDepotIndent(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1, string sParam2)
        {

            try
            {
                string strReportServer;

                ReportViewer1.ShowCredentialPrompts = false;

                //parmDepotIndent[0] = new Microsoft.Reporting.WebForms.ReportParameter("pDate", sParam1);
                //parmDepotIndent[1] = new Microsoft.Reporting.WebForms.ReportParameter("pRegionID", sParam2);
                parmDepotIndent[0] = new Microsoft.Reporting.WebForms.ReportParameter("ID", sParam1);
                parmDepotIndent[1] = new Microsoft.Reporting.WebForms.ReportParameter("CatID", sParam2);



                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment",  "Secure123*",  "DCSSERVER");
                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //VItthal Report Start
                strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
                //VItthal Report End

                ReportViewer1.ServerReport.ReportPath = sReportName;
                //Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                //ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                //ObjCredential[0].Name = "DataSource1";
                //ObjCredential[0].Password = "Secure123*";
                //ObjCredential[0].UserId = "sa";
                // ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential); 


                ReportViewer1.ServerReport.SetParameters(parmDepotIndent);
                ReportViewer1.ServerReport.Refresh();
                //*** Comment Start By Vitthal on 16/03/2010 
                //*** User Want Report View Format After View Report user will select  Export Option for Printing 

                //if (ExportYesNo == "Y")
                //{
                //    string mimeType, encoding, extension, deviceInfo;
                //    string[] streamids;
                //    Microsoft.Reporting.WebForms.Warning[] warnings;
                //    string format = "PDF"; //Desired format goes here (PDF, Excel, or Image)

                //    deviceInfo = "<DeviceInfo> <SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //    byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                //    Response.Clear();

                //    if (format == "PDF")
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("Content-disposition", "filename=output.pdf");
                //    }
                //    else if (format == "Excel")
                //    {
                //        Response.ContentType = "application/excel";
                //        Response.AddHeader("Content-disposition", "filename=output.xls");
                //    }

                //    Response.OutputStream.Write(bytes, 0, bytes.Length);
                //    Response.OutputStream.Flush();
                //    Response.OutputStream.Close();
                //    Response.Flush();
                //    Response.Close();
                //}

                //*** Comment End By Vitthal on 16/03/2010 


            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }
        //Megha 12032012 Depot Indent
        //protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Func.Convert.iConvertToInt(RadioButtonList1.SelectedValue) == 1 )
        //    {
        //        ReportView(ReportViewer1, "/ReportServer/rptTargetVsSales", lstParameters(iMenuId));
        //    }
        //}
        public void ReportViewFromReportWindow(string sReportName)
        {

            try
            {
                string strReportServer;

                string strReportpath;

                strReportpath = sReportName;
                ReportViewer1.ShowCredentialPrompts = false;
                Hashtable hashTblParalist = new Hashtable();
                hashTblParalist = (Hashtable)Session["ParamterList"];
                int iCnt = 0;
                parm = new Microsoft.Reporting.WebForms.ReportParameter[hashTblParalist.Count];
                foreach (DictionaryEntry parameter in hashTblParalist)
                {

                    parm[iCnt] = new Microsoft.Reporting.WebForms.ReportParameter(Convert.ToString(parameter.Key), Convert.ToString(parameter.Value));
                    iCnt = iCnt + 1;
                }
                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Deployment", "Secure123*", "DCSSERVER");
                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(Convert.ToString(ConfigurationManager.AppSettings["UserName"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]), Convert.ToString(ConfigurationManager.AppSettings["UserDomain"]));
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //VItthal Report Start
                //strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);           
                if (sReportName.Trim() == "/DCSReports/RptUserHeirarchy" || sReportName.Trim() == "/DCSReports/RptDMSXMLGENLOG" || sReportName.Trim() == "/DCSReports/RptDCSFileExceptionDetails"
                    || sReportName.Trim() == "/DCSReports/RptDCSFileProcessResult" || sReportName.Trim() == "/DCSReports/RptDMSFileExchange")
                {
                    strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
                }
                else
                {
                    strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["SSRSReportServer"]);
                }

                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
                //VItthal Report End

                ReportViewer1.ServerReport.ReportPath = sReportName;

                //Sujata 15092012_Begin This coding is imporatant on 111 not necessory on 252.
                Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                ObjCredential[0].Name = "DataSource1";
                ObjCredential[0].Password = "Secure123*";
                ObjCredential[0].UserId = "sa";
                ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);            
                //Sujata 15092012_End

                ReportViewer1.ServerReport.SetParameters(parm);
                ReportViewer1.ServerReport.Refresh();



            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

    }
}