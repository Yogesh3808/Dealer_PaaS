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
using System.Drawing;
using System.Net;
using MANART_BAL;
using MANART_DAL;
using System.IO;

namespace MANART.Forms.Spares
{
    public partial class frmOpenFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenDocument();
        }
        private void OpenDocument()
        {
            try
            {
                string sBasePath = "";
                string sOldBasePath = "";
                string sFullPath = "";
                string sFileName = Uri.UnescapeDataString(Request.QueryString["FileName"].ToString()).Replace("&nbsp;", " ");

                sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
                sOldBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadOldDomesticFiles"]);
                string str = sBasePath + @"Parts\Model Catelogue\" + sFileName;
                if (System.IO.File.Exists(sBasePath + @"Parts\Model Catelogue\" + sFileName))
                    sFullPath = sBasePath + @"Parts\Model Catelogue\" + sFileName;
                if (System.IO.File.Exists(sOldBasePath + @"Parts\Model Catelogue\" + sFileName))
                    sFullPath = sOldBasePath + @"Parts\Model Catelogue\" + sFileName;

                //sFullPath = Path.Combine(Server.MapPath("~/DownLoadFiles/ModelCatelogue/"), sFileName);

                //Check File Exist In dealer Folder
                if (!System.IO.File.Exists(@sFullPath))
                {
                    Response.Write(@sFullPath);
                    Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in perticular folder path!');</script>");
                    return;
                }
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(@sFullPath);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", buffer.Length.ToString());
                    Response.BinaryWrite(buffer);
                    return;
                }
               
            }
            catch (Exception ex)
            // file IO errors
            {
                //SupportClass.WriteStackTrace(e, Console.Error);
                Func.Common.ProcessUnhandledException(ex);
            }
        }

    }
}