using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MANART.Forms.Common
{
    public class MyUtils
    {
        public static string GetWebsiteRoot()
        {
            HttpRequest req = HttpContext.Current.Request;

            string port = (req.Url.Port == 80 || req.Url.Port == 443 ? "" : ":" + req.Url.Port.ToString());

            string wsroot = req.Url.Scheme + "://" + req.Url.Host + port + req.ApplicationPath + "Forms/Common/"; // 

            if (wsroot.EndsWith("/"))
                return wsroot;
            else
                return wsroot + "/";
        }
    }
}