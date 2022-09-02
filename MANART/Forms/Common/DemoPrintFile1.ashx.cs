using System;
using System.Web;

using Neodynamic.SDK.Web;

public class Handler : IHttpHandler {
    
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

            if (filePath != null)
            {
                string tmpPdf = context.Session["fileName"].ToString();
                PrintFile file = new PrintFile(context.Server.MapPath(@"D:\MANART\MANART\DownloadFiles\PartsCatalogue\" + tmpPdf), fileName);
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