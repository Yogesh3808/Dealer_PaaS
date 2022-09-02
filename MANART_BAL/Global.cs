using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO.Compression;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsGlobal
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.PostReleaseRequestState +=
                new EventHandler(Global_PostReleaseRequestState);
        }

        private void Global_PostReleaseRequestState(
            object sender, EventArgs e)
        {
            string contentType = Response.ContentType;

            if (contentType == "text/javascript" ||
                contentType == "text/css")
            {
                Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

                string acceptEncoding =
                    Request.Headers["Accept-Encoding"];

                if (acceptEncoding != null)
                {
                    if (acceptEncoding.Contains("gzip"))
                    {
                        Response.Filter = new GZipStream(
                            Response.Filter, CompressionMode.Compress);
                        Response.AppendHeader(
                            "Content-Encoding", "gzip");
                    }
                    else if (acceptEncoding.Contains("deflate"))
                    {
                        Response.Filter = new DeflateStream(
                            Response.Filter, CompressionMode.Compress);
                        Response.AppendHeader(
                            "Content-Encoding", "deflate");
                    }
                }
            }
        }

    }
}
