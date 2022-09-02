using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Configuration;

 

namespace MANART_BAL
{
    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {


        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        #region IReportServerCredentials Members
        public CustomReportCredentials()
        {
            _UserName = "";
            _PassWord = "";
            _DomainName = "";
        }
        public CustomReportCredentials(string userName, string password,string DomainName)
        {
            
            _UserName = userName;
            _PassWord = password;
            _DomainName = DomainName;
        }


        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            return false;
        }

        //public WindowsIdentity ImpersonationUser
        //{
        //    get
        //    {

        //        System.Security.Principal.WindowsImpersonationContext impersonationContext = null;
        //        System.Security.Principal.WindowsIdentity currentWindowsIdentity = null;
        //        currentWindowsIdentity = (System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity;
        //        impersonationContext = currentWindowsIdentity.Impersonate();

        //        //Insert your code that runs under the security context of the authenticating user here.

        //        impersonationContext.Undo();

        //        return currentWindowsIdentity; // not use ImpersonationUser
        //    }
        //}
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                //_UserName = "vikram.k";
                //_PassWord = "Admin@123";
                //_DomainName = "";
                
                string userName = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["UserName"]);
                string password = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["UserPassword"]);
                string domainName = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["UserDomain"]);

                return new System.Net.NetworkCredential(userName, password, domainName);
            }
        }

        #endregion

    }

}


