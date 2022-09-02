using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using Microsoft.ReportingServices.ReportRendering
using Microsoft.Reporting.WebForms;
using MANART_DAL;

namespace MANART_BAL
{
    public class clsReport
    {
        public clsReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public void SetReportValue(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1)
        {

            Microsoft.Reporting.WebForms.ReportParameter[] parm = new Microsoft.Reporting.WebForms.ReportParameter[1];

            parm[0] = new Microsoft.Reporting.WebForms.ReportParameter("Preshipment_ID", sParam1);
            //parm[0].= "Preshipment_ID";
            //parm[0].Value = BLL.Func.Convert.iConvertToInt(sParam1);
            ReportViewer1.ShowCredentialPrompts = false;

           // ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("deployment", "Secure123*", "DCSServer");

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;

            // Sujata 
            //ReportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://192.168.1.252/ReportServer");
            string strReportServer;
            strReportServer = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportServer"]);
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(strReportServer);
            // Sujata 

            //ReportViewer1.ServerReport.ReportPath = @"\DCSReports\Report1.rdl";
            sReportName = "/DCSReports/BillOfExchangeFirst";
            ReportViewer1.ServerReport.ReportPath = sReportName;

            Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
            ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
            ObjCredential[0].Name = "DataSource1";
            ObjCredential[0].Password = "sa";
            ObjCredential[0].UserId = "sa";
            ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);
            ReportViewer1.ServerReport.SetParameters(parm);
            ReportViewer1.ServerReport.Refresh();

        }
        public void TestReport(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1)
        {

            try
            {
                //Microsoft.Reporting.WebForms.ReportParameter[] parm = new Microsoft.Reporting.WebForms.ReportParameter[1];

                //parm[0] = new Microsoft.Reporting.WebForms.ReportParameter("Preshipment_ID", sParam1);
                //parm[0].= "Preshipment_ID";
                //parm[0].Value = BLL.Func.Convert.iConvertToInt(sParam1);
                ReportViewer1.ShowCredentialPrompts = false;

               // ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("deployment", "Secure123*", "DCSServer");

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://192.168.1.253/ReportServer");
                //ReportViewer1.ServerReport.ReportPath = @"\DCSReports\Report1.rdl";
                sReportName = "/DCSReports/Report1";
                ReportViewer1.ServerReport.ReportPath = sReportName;


                Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                ObjCredential[0].Name = "DataSource1";
                ObjCredential[0].Password = "sa";
                ObjCredential[0].UserId = "sa";
                ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);
                //ReportViewer1.ServerReport.SetParameters(parm);
                ReportViewer1.ServerReport.Refresh();
            }
            catch
            {
            }

        }
        public void TestReport2(Microsoft.Reporting.WebForms.ReportViewer ReportViewer1, string sReportName, string sParam1)
        {

            try
            {
                Microsoft.Reporting.WebForms.ReportParameter[] parm = new Microsoft.Reporting.WebForms.ReportParameter[1];

                parm[0] = new Microsoft.Reporting.WebForms.ReportParameter("CountryId", sParam1);
                //parm[0].= "Preshipment_ID";
                //parm[0].Value = BLL.Func.Convert.iConvertToInt(sParam1);
                ReportViewer1.ShowCredentialPrompts = false;

               // ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("deployment", "Secure123*", "DCSServer");

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                ReportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://192.168.1.253/ReportServer");
                //ReportViewer1.ServerReport.ReportPath = @"\DCSReports\Report1.rdl";
                //sReportName = "/DCSReports/Report1";
                sReportName = "/DCSReports/PreShipment";
                ReportViewer1.ServerReport.ReportPath = sReportName;



                Microsoft.Reporting.WebForms.DataSourceCredentials[] ObjCredential = new Microsoft.Reporting.WebForms.DataSourceCredentials[1];
                ObjCredential[0] = new Microsoft.Reporting.WebForms.DataSourceCredentials();
                ObjCredential[0].Name = "DataSource1";
                ObjCredential[0].Password = "sa";
                ObjCredential[0].UserId = "sa";
                ReportViewer1.ServerReport.SetDataSourceCredentials(ObjCredential);
                ReportViewer1.ServerReport.SetParameters(parm);
                ReportViewer1.ServerReport.Refresh();
            }
            catch
            {
            }

        }

        public DataSet SetDealerID(int UserId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_UserDealer", UserId);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet SetReportURL(int Report_Option_no, int Report_GEO_POS)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetReportURL", Report_Option_no, Report_GEO_POS);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet SetReportControls(int Report_Option_no, int Menu_ID, int Report_GEO_POS)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetReportOptionDtls", Report_Option_no, Menu_ID, Report_GEO_POS);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet SetReportParameters(int Report_Option_no, int Report_GEO_POS)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetReportParameterDtls", Report_Option_no, Report_GEO_POS);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Sujata 18032011
        public DataSet SetUserRegionSetting(int iUserId, string sUserOrgn, int iHeirarchy, int iDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETRegion", iUserId, sUserOrgn, iHeirarchy, iDeptID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet SetUserStateSetting(int iUserId, int iRegionID, string sUserOrgn, int iHeirarchy, int iDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETState", iUserId, iRegionID, sUserOrgn, iHeirarchy, iDeptID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet SetUserCountrySetting(int iUserId, int iRegionID, string sUserOrgn, int iHeirarchy, int iDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETCountry", iUserId, iRegionID, sUserOrgn, iHeirarchy, iDeptID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet SetCSMSetting(int iUserId, int iRegionID, int iStateId, int iCountryId, string sUserOrgn, int iHeirarchy, int iDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETCSM", iUserId, iRegionID, iStateId, iCountryId, sUserOrgn, iHeirarchy, iDeptID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        public DataSet SetDealerSetting(int iUserId, string sUserOrgn, int iRegionID, int iStateId, int iCountryId, int iCSMID, int iHeirarchy, int iDeptID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETDealerID", iUserId, sUserOrgn, iRegionID, iStateId, iCountryId, iCSMID, iHeirarchy, iDeptID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }



        //Sujata 18032011

        //vrushali02042011_Begin

        public DataSet SetYearSetting(string sYearcd)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetYearID", sYearcd);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //vrushali02042011_End

        //Sujata 27042011_Begin
        public DataSet SetMonthSetting(int MonthId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMonthName", MonthId);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        //Sujata 27042011_End

        //Sujata 23052012_Begin
        public DataSet SetUserLoginNameSetting(int iUserId, int iMenuDeptId)
       // public DataSet SetUserLoginNameSetting(int iUserId, int iMenuDeptId)
        //Sujata 23052012_End
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                //Sujata 23052012_Begin
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetLoginName", iUserId );
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetLoginName", iUserId, iMenuDeptId);
                //Sujata 23052012_End
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        //Sujata 25052011
        public DataSet SetHeadLoginNameSetting(int iUserId, int iDeptId, int iBasicModCatID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_SETHeadLogin", iUserId, iDeptId, iBasicModCatID);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        //Sujata 25052011

        public DataSet SetEGPLoginNameSetting(int iUserId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetEGPLoginName", iUserId);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
