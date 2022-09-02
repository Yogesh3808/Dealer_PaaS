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
using System.Net;
using System.ComponentModel;

namespace MANART.Forms.Spares
{
    public partial class frmOpenAttachDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DownloadDocument();
            OpenDocument();
        }
        // New Code Created by Vikram Dated 12/01/2018 Not Used 
        private void DownloadDocument()
        {
            FileInfo fileToDownload = null;
            string sBasePath = "";
            string sOldBasePath = "";
            string sFullPath = "";
            string strDocType = "";
            string sFileName = Uri.UnescapeDataString(Request.QueryString["FileName"].ToString()).Replace("&nbsp;", " "); ;
            string sDealerCode = Request.QueryString["DealerCode"].ToString();
            string sUserType = Request.QueryString["UserType"].ToString();
            if (sUserType == "2" || sUserType == "3" || sUserType == "8")
            {
                sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
                sOldBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadOldDomesticFiles"]);

                if (!Directory.Exists(@sBasePath))
                {// Check Directory present or Not
                    Directory.CreateDirectory(@sBasePath);
                }
                else
                {// list all sub Directory in Directory (sub Folder)
                    string[] dirs = Directory.GetDirectories(@sBasePath, "*", SearchOption.AllDirectories);
                    foreach (string dir in dirs)
                    {
                        if (System.IO.File.Exists(dir + @"\" + sFileName))// check file exists 
                        {
                            sFullPath = dir + @"\" + sFileName;
                            break;
                        }
                    }// END Foreach
                } //END Else

                if (sFullPath == "")
                {// if C Drive File Doesnot Exists then check on E Drive
                    if (!Directory.Exists(@sOldBasePath)) // Check Directoty Present or Not
                    {
                        Directory.CreateDirectory(@sOldBasePath);
                    }
                    bool isEmpty = !Directory.EnumerateFiles(@sOldBasePath).Any();// check Directory contains any file or subFolder
                    if (isEmpty == true)
                    {
                        Response.Write(@sFullPath);
                        Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in particular Dealer folder path!');</script>");
                        return;
                    }
                    else
                    {
                        string[] dirsOld = Directory.GetDirectories(@sOldBasePath, "*", SearchOption.AllDirectories);//List of all Sub Folder in E Drive
                        foreach (string dirOld in dirsOld)
                        {
                            if (System.IO.File.Exists(dirOld + @"\" + sFileName))
                            {
                                sFullPath = dirOld + @"\" + sFileName;
                                break;
                            }
                            else
                            {
                                Response.Write(@sFullPath);
                                Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in particular Dealer folder path!');</script>");
                                return;
                            }
                        }// END Foreach
                    }// END Else
                }// END IF


                fileToDownload = new System.IO.FileInfo(@sFullPath);
                Response.ContentType = ReturnExtension(fileToDownload.Extension.ToLower());
                System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(sFileName) + "\"";
                Response.AppendHeader("Content-Disposition", disHeader);
                Response.Flush();
                Response.WriteFile(fileToDownload.FullName);
                HttpContext.Current.ApplicationInstance.CompleteRequest();


            }
        }
        // to open The Document
        private void OpenDocument()
        {
            try
            {
                FileInfo fileToDownload = null;
                string sBasePath = "";
                string sOldBasePath = "";
                string sFullPath = "";
                string strDocType = "";
                string sFileName = Uri.UnescapeDataString(Request.QueryString["FileName"].ToString()).Replace("&nbsp;", " "); ;
                string sDealerCode = Request.QueryString["DealerCode"].ToString();
                string sUserType = Request.QueryString["UserType"].ToString();

                //char[] delimiterChars = { '_'};
                //string[] words = sFileName.Split(delimiterChars);
                //string OpenFileName = words[2].ToString();

                if (sUserType == "1" || sUserType == "4")
                {
                    sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["WarrantyClaimAttachFilePath"]);
                    sOldBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadOldDomesticFiles"]);
                    if (System.IO.File.Exists(sBasePath + @"Warranty Claim\" + sFileName))
                        sFullPath = sBasePath + @"Warranty Claim\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Warranty Claim\" + sFileName))
                        sFullPath = sOldBasePath + @"Warranty Claim\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Claim Request\" + sFileName))
                        sFullPath = sBasePath + @"Claim Request\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Claim Request\" + sFileName))
                        sFullPath = sOldBasePath + @"Claim Request\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Sale\M3\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Sale\M3\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Sale\M6\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Sale\M6\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Sale\M7\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Sale\M7\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Sale\M8\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Sale\M8\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"JobPCR\" + sFileName))
                        sFullPath = sOldBasePath + @"JobPCR\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Parts\Part Claim\" + sFileName))
                        sFullPath = sBasePath + @"Parts\Part Claim\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Parts\Part Claim\" + sFileName))
                        sFullPath = sOldBasePath + @"Parts\Part Claim\" + sFileName;

                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Purchase\Direct Billing Receipt\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Purchase\Direct Billing Receipt\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Purchase\VehOrderFormDealer\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Purchase\VehOrderFormDealer\" + sFileName;

                    if (System.IO.File.Exists(sOldBasePath + @"Vehicle Purchase\VehOrderFormMTI\" + sFileName))
                        sFullPath = sOldBasePath + @"Vehicle Purchase\VehOrderFormMTI\" + sFileName;


                }

                if (sUserType == "2" || sUserType == "3" || sUserType == "8")
                {
                    sBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadDomesticFiles"]);
                    sOldBasePath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["DownloadOldDomesticFiles"]);

                    if (System.IO.File.Exists(sBasePath + @"JobPCR\" + sFileName))
                        sFullPath = sBasePath + @"JobPCR\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Warranty Claim\" + sFileName))
                        sFullPath = sBasePath + @"Warranty Claim\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Warranty Claim\" + sFileName))
                        sFullPath = sOldBasePath + @"Warranty Claim\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Parts\Part Claim\" + sFileName))
                        sFullPath = sBasePath + @"Parts\Part Claim\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Parts\Part Claim\" + sFileName))
                        sFullPath = sBasePath + @"Parts\Part Claim\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Claim Request\" + sFileName))
                        sFullPath = sBasePath + @"Claim Request\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Claim Request\" + sFileName))
                        sFullPath = sOldBasePath + @"Claim Request\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Jobcard\" + sFileName))
                        sFullPath = sBasePath + @"Jobcard\" + sFileName;
                    if (System.IO.File.Exists(sOldBasePath + @"Jobcard\" + sFileName))
                        sFullPath = sOldBasePath + @"Jobcard\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"MRNFiles\" + sFileName))
                        sFullPath = sBasePath + @"MRNFiles\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Vehicle Sale\M3\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Sale\M3\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Vehicle Sale\M6\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Sale\M6\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Vehicle Sale\M7\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Sale\M7\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Vehicle Sale\M8\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Sale\M8\" + sFileName;
                    if (System.IO.File.Exists(sBasePath + @"Vehicle Purchase\Direct Billing Receipt\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Purchase\Direct Billing Receipt\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Vehicle Purchase\VehOrderFormDealer\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Purchase\VehOrderFormDealer\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Vehicle Purchase\VehOrderFormMTI\" + sFileName))
                        sFullPath = sBasePath + @"Vehicle Purchase\VehOrderFormMTI\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Coupon Claim\" + sFileName))
                        sFullPath = sBasePath + @"Coupon Claim\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Coupon Claim\" + sFileName))
                        sFullPath = sBasePath + @"Coupon Claim\" + sFileName;

                    if (System.IO.File.Exists(sBasePath + @"Activity\" + sFileName))
                        sFullPath = sBasePath + @"Activity\" + sFileName;
                }


                //Check File Exist In dealer Folder
                if (!System.IO.File.Exists(@sFullPath))
                {
                    Response.Write(@sFullPath);
                    Page.RegisterStartupScript("Close", "<script language='javascript'> alert('This File does not exist in particular Dealer folder path!');</script>");
                    return;
                }
                fileToDownload = new System.IO.FileInfo(@sFullPath);


                // Set the ContentType
                Response.ContentType = ReturnExtension(fileToDownload.Extension.ToLower());

                System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(sFileName) + "\"";
                //System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(OpenFileName) + "\"";

                Response.AppendHeader("Content-Disposition", disHeader);

                Response.Flush();
                Response.WriteFile(fileToDownload.FullName);
                // Response.WriteFile(Context.Server.MapPath(fileToDownload.FullName)); 
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.End();

                //else if (sFileType == "Image")//Image
                //{
                //    Bitmap bmp = new Bitmap(20, 50);
                //    Response.ContentType = "image/jpeg";
                //    Response.TransmitFile(Server.MapPath("~/images/Part0001.png"));
                //    Response.AppendHeader("Content-Disposition", "attachment; sFileName=123.jpg");
                //    bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //    Response.End();
                //}
            }
            catch (Exception ex)
            // file IO errors
            {
                //SupportClass.WriteStackTrace(e, Console.Error);
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        // To Get File Extension
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl": return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }
    }
}