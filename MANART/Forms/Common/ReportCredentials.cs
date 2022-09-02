using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace MANART.Forms.Common
{
    public class ReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {

        string _userName, _password, _domain;

        public ReportCredentials(string userName, string password, string domain)
        {

            _userName = userName;

            _password = password;

            _domain = domain;

        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
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

                return new System.Net.NetworkCredential(_userName, _password, _domain);

            }

        }



        public bool GetFormsCredentials(out System.Net.Cookie authCoki, out string userName, out string password, out string authority)
        {

            userName = _userName;

            password = _password;

            authority = _domain;

            //authCoki = new System.Net.Cookie(".ASPXAUTH", ".ASPXAUTH", "/", "Domain");
            authCoki = new System.Net.Cookie("deployment", "Secure123*", "/", "DCSServer");
            //authCoki = new System.Net.Cookie("deployment", "Admin@123", "/", "");
            

            return true;

        }



    }

    //class ReportCredentials
    //{
    //    private string p1;
    //    private string p2;
    //    private string p3;

    //    public ReportCredentials(string p1, string p2, string p3)
    //    {
    //        // TODO: Complete member initialization
    //        this.p1 = p1;
    //        this.p2 = p2;
    //        this.p3 = p3;
    //    }
    //}
}
