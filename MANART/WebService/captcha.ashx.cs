using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.SessionState;
using MANART_DAL;
using MANART_BAL;


namespace MANART.WebService
{
    /// <summary>
    /// Summary description for captcha
    /// </summary>
    public class captcha : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "Image/jpeg";

            clsCaptcha captcha = new clsCaptcha();
            string str = captcha.DrawNumbers(5);
            if (context.Session[clsCaptcha.SESSION_CAPTCHA] == null) context.Session.Add(clsCaptcha.SESSION_CAPTCHA, str);
            else
            {
                context.Session[clsCaptcha.SESSION_CAPTCHA] = str;
            }
            Bitmap bmp = captcha.Result;
            bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}