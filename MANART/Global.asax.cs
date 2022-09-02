using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Globalization;

namespace MANART
{
    public class Global : System.Web.HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;


        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            Application["Active"] = 0;
            Application["UserTrack"] = null;
        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            // Session["UserID"] = null;
            //HttpContext.Current.Session.Clear();    
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised. 
            //HttpContext.Current.Session.Clear(); 
            //HttpContext.Current.Response.Cookies.Clear();
            //int cookiesCnt = Request.Cookies.Count - 1;

            //for (int i = 0; i < cookiesCnt; i++)
            //{

            //    Request.Cookies[i].Expires = DateTime.Now.AddDays(-1);

            //    Response.Cookies.Add(Request.Cookies[i]);

            //}          
        }
    }
}