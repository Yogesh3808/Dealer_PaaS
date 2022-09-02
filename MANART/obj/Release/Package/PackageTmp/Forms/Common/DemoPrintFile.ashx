<%@ WebHandler Language="C#" class="Handler" %> 
using System;
using System.Web;

using Neodynamic.SDK.Web;

public class Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState, System.Web.SessionState.IReadOnlySessionState 
{
    
    public void ProcessRequest (HttpContext context) {
        if (WebClientPrint.ProcessPrintJob(context.Request))
        {

            bool useDefaultPrinter = (context.Request["useDefaultPrinter"] == "checked");
            string printerName = context.Server.UrlDecode(context.Request["printerName"]);

            string fileName = Guid.NewGuid().ToString("N") + "." + context.Request["filetype"];
            string filePath = null;
            //filePath = @"~/DownloadFiles/PartsCatalogue/100040715041.pdf";
            //switch (context.Request["filetype"])
            //{
            //    case "PDF":
            //        filePath = "~/files/LoremIpsum.pdf";
            //        break;
            //    case "TXT":
            //        filePath = "~/files/LoremIpsum.txt";
            //        break;
            //    case "DOC":
            //        filePath = "~/files/LoremIpsum.doc";
            //        break;
            //    case "XLS":
            //        filePath = "~/files/SampleSheet.xls";
            //        break;
            //    case "JPG":
            //        filePath = "~/files/penguins300dpi.jpg";
            //        break;
            //    case "PNG":
            //        filePath = "~/files/SamplePngImage.png";
            //        break;
            //    case "TIF":
            //        filePath = "~/files/patent2pages.tif";
            //        break;
            //}
            //string tmpPdf = context.Session["fileName"].ToString();
            string tmpPdf = context.Request["PDFName"].ToString();
            filePath = tmpPdf;
            
            if (filePath != null)
            {                
                //string tmpPdf = context.Request["PDFFile"];
                PrintFile file = new PrintFile(context.Server.MapPath("~/Forms/Common/PDFFiles/" + filePath), fileName);

                //Set license info...
                //WebClientPrint.LicenseOwner = "Shrikrishna Panvalkar";
                //WebClientPrint.LicenseOwner = "SHRIKRISHNA PANVALKAR";
                //WebClientPrint.LicenseKey = "D1563667CF420D633B3D95A58096DF9407676345";

                ClientPrintJob cpj = new ClientPrintJob();
                cpj.PrintFile = file;
                if (useDefaultPrinter || printerName == "null")
                    cpj.ClientPrinter = new DefaultPrinter();
                else
                    cpj.ClientPrinter = new InstalledPrinter(printerName);
                cpj.SendToClient(context.Response);
            }

        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


}