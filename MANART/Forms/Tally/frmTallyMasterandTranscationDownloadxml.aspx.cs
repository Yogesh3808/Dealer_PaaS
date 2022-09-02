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
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ionic.Zip;
//using System.IO.Compression;



namespace MANART.Forms.Tally
{

    public partial class frmTallyMasterandTranscationDownloadxml : System.Web.UI.Page
    {
        private string iDealerID = "";
         string strpath="";
         string strpathMove = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            iDealerID = Func.Convert.sConvertToString(Session["iDealerID"]);
          //  string userDownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
            BindGridview();
        
        }

        protected void btnGetFiles_Click(object sender, EventArgs e)
        {
            BindGridview();
           
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
           // lblMessage.Text = "Files are downloaded successfully ";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                //string drive = txtdrive.Text;
                //string  strFilePath =  drive + "TallyXml\\";
                //zip.(DateTime.Now.ToString("dd-MMM-yyyy"), strFilePath);
                zip.AddDirectoryByName("Files");
               
           
                foreach (GridViewRow row in gvDetails.Rows)
                {
                   
                        string filePath = (row.FindControl("lblFilePath") as Label).Text;
                        zip.AddFile(filePath, "Files");

                        //string strfldDateTimeName = "";
                        //strfldDateTimeName = DateTime.Now.ToString("dd-MMM-yyyy");

                        //string fileName = (row.FindControl("lblFileName") as Label).Text;
                       

                        //strpathMove = @"E:\DataExchange\TallyInArchive\" + strfldDateTimeName + "\\" + fileName;

                        //string directory = Path.GetDirectoryName(strpathMove);
                        //CreateDirectory(new DirectoryInfo(directory));


                        //System.IO.File.Move(filePath, strpathMove);
                        //File.Delete(filePath);



                    
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", "TallyXml_" + DateTime.Now.ToString("dd-MMM-yyyy"));
                
               
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
              
                zip.Save(Response.OutputStream);

                for (int j = 0; j < gvDetails.Rows.Count; j++)
                {
                   

                    string strfldDateTimeName = "";
                    strfldDateTimeName = DateTime.Now.ToString("dd-MMM-yyyy");
                    System.Web.UI.WebControls.Label filePath1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[j].FindControl("lblFilePath");
                    string filepath = Func.Convert.sConvertToString(filePath1.Text);
                    System.Web.UI.WebControls.Label fileName1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[j].FindControl("lblFileName");
                    string fileName = Func.Convert.sConvertToString(fileName1.Text);

                    strpathMove = @"E:\DataExchange\TallyInArchive\" + strfldDateTimeName + "\\" + fileName;

                    string directory = Path.GetDirectoryName(strpathMove);
                    CreateDirectory(new DirectoryInfo(directory));

                    if (!File.Exists(strpathMove))
                    {
                        System.IO.File.Move(filepath, strpathMove);
                        File.Delete(filepath);
                    }

                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;

                }

                Response.End();
                //Response.Flush();
                // Response.Close();  
            }
            //for (int j = 0; j < gvDetails.Rows.Count; j++)
            //{
            //    string strfldDateTimeName = "";
            //    strfldDateTimeName = DateTime.Now.ToString("dd-MMM-yyyy");
            //    System.Web.UI.WebControls.Label filePath1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[j].FindControl("lblFilePath");
            //    string filepath = Func.Convert.sConvertToString(filePath1.Text);
            //    System.Web.UI.WebControls.Label fileName1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[j].FindControl("lblFileName");
            //    string fileName = Func.Convert.sConvertToString(fileName1.Text);

            //    strpathMove = @"E:\DataExchange\TallyInArchive\" + strfldDateTimeName + "\\" + fileName;

            //    string directory = Path.GetDirectoryName(strpathMove);
            //    CreateDirectory(new DirectoryInfo(directory));

                
            //        System.IO.File.Move(filepath, strpathMove);
            //        File.Delete(filepath);

                
            //}

           
          

        }

      

        protected void BindGridview()
        {
             strpath = @"E:\DataExchange\Tally\" + iDealerID + "\\";
            
           // strpath = @"E:\DataExchange\Tally\";
            string[] folders = Directory.GetFiles(strpath, "*", SearchOption.AllDirectories);
           
            DataTable dt = new  DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            dt.Columns.Add(new DataColumn("FilePath", typeof(string)));
            foreach (string path in folders)
            {
                dr = dt.NewRow();
                dr["FileName"] = Path.GetFileName(path);
                dr["FilePath"] = Path.GetFullPath(path);
                //files.Add(new ListItem(Path.GetFileName(path)), new ListItem(Path.GetFullPath(path)));
                //files.Add(new ListItem(Path.GetFullPath(path)));
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            if (dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
            else
            {
                lblNoMsg.Text = "Files are not Exists";
                lblNoMsg.ForeColor = System.Drawing.Color.Red;
                lblNoMsg.Visible = true;
            }

        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                System.Web.UI.WebControls.Label filePath = (System.Web.UI.WebControls.Label)gvDetails.Rows[0].FindControl("lblFilePath");
                string sFileName = ((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text;

                FileInfo fileToDownload = null;
                fileToDownload = new System.IO.FileInfo(Func.Convert.sConvertToString(filePath));
                // Response.ContentType = ReturnExtension(fileToDownload.Extension.ToLower());
                Response.ContentType = "application/xml";
                System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(sFileName) + "\"";
                Response.AppendHeader("Content-Disposition", disHeader);
                Response.Flush();
                Response.WriteFile(fileToDownload.FullName);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void btnDownload1111_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strFileDestPath = "";
            string strFileDestPath_Print = "";
            string drive = "";
            string strfldDateTimeName = "";
            byte[] responseBytes = null;
            for (i = 0; i < gvDetails.Rows.Count; i++)
            {

                System.Web.UI.WebControls.Label filePath1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[i].FindControl("lblFilePath");
                string filepath = Func.Convert.sConvertToString(filePath1.Text);
                //  LinkButton  fileName1 = (LinkButton)gvDetails.Rows[i].FindControl("lnkbtn");
                System.Web.UI.WebControls.Label fileName1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[i].FindControl("lblFileName");
                string fileName = Func.Convert.sConvertToString(fileName1.Text);

                strfldDateTimeName = DateTime.Now.ToString("dd-MMM-yyyy");
                drive = txtdrive.Text;
                            

                strFileDestPath = drive + "TallyXml\\" + strfldDateTimeName + "\\" + fileName;
                strFileDestPath_Print = drive + "TallyXml\\" + strfldDateTimeName;
                string directory = Path.GetDirectoryName(strFileDestPath);
                CreateDirectory(new DirectoryInfo(directory));

                WebClient wc = new WebClient();
                 wc.DownloadFile(filepath,  strFileDestPath);
              

                strpathMove = @"E:\DataExchange\TallyInArchive\" + strfldDateTimeName + "\\" + fileName;
                directory = Path.GetDirectoryName(strpathMove);
                CreateDirectory(new DirectoryInfo(directory));

                if (File.Exists(strFileDestPath))
                {
                    System.IO.File.Move(filepath, strpathMove);
                    File.Delete(filepath);

                }

                responseBytes = File.ReadAllBytes(filepath);
                File.WriteAllBytes(fileName, responseBytes);

            }
            if (i == gvDetails.Rows.Count)
            {
                lblMessage.Text = "Files are downloaded successfully to " + strFileDestPath_Print;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
           

        }
        protected void btnDownload1_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strFileDestPath = "";
            string strFileDestPath_Print = "";
            string drive ="";
            string strfldDateTimeName = "";
            for (i = 0; i < gvDetails.Rows.Count; i++)
            {

                System.Web.UI.WebControls.Label filePath1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[i].FindControl("lblFilePath");
                string filepath = Func.Convert.sConvertToString(filePath1.Text);
                //  LinkButton  fileName1 = (LinkButton)gvDetails.Rows[i].FindControl("lnkbtn");
                System.Web.UI.WebControls.Label fileName1 = (System.Web.UI.WebControls.Label)gvDetails.Rows[i].FindControl("lblFileName");
                string fileName = Func.Convert.sConvertToString(fileName1.Text);
                
                strfldDateTimeName = DateTime.Now.ToString("dd-MMM-yyyy");
                drive = txtdrive.Text;
                string[] url = filepath.Split('\\');


                string filepathURI = "http://192.168.1.24/DataExchange/Tally/" + url[3] + "/" + url[4] + "/" +  url[5];

                strFileDestPath =  drive + "TallyXml\\" + strfldDateTimeName + "\\" + fileName;
                strFileDestPath_Print = drive + "TallyXml\\" + strfldDateTimeName;
                string directory = Path.GetDirectoryName( strFileDestPath);
                CreateDirectory(new DirectoryInfo(directory));

                WebClient wc = new WebClient();
               // wc.DownloadFile(filepath,  strFileDestPath);
                wc.DownloadFile(filepathURI, strFileDestPath);
              
                strpathMove = @"E:\DataExchange\TallyInArchive\" + strfldDateTimeName + "\\" + fileName;
                 directory = Path.GetDirectoryName(strpathMove);
                CreateDirectory(new DirectoryInfo(directory));

                if (File.Exists(strFileDestPath))
                {
                    System.IO.File.Move(filepath, strpathMove);
                    File.Delete(filepath);
                   
                }

            }
            if (i == gvDetails.Rows.Count)
            {
                lblMessage.Text = "Files are downloaded successfully to " + strFileDestPath_Print;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
           // LinkButton lnkbtn = sender as LinkButton;
           // GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
           
           // string filePath = gvDetails.DataKeys[gvrow.RowIndex].Value.ToString();
           // string sFileName = gvDetails.DataKeys[gvrow.RowIndex].Value.ToString();
           // //string filePath = (gvDetails.Rows[gvrow.RowIndex].FindControl("FilePath") as TextBox).Text;
           // //string sFileName = (gvDetails.Rows[gvrow.RowIndex].FindControl("FileName") as LinkButton).Text;
           // //Response.ContentType = "application/xml";
           // //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
           // //Response.TransmitFile(Server.MapPath(filePath));
           // //Response.End();

           // FileInfo fileToDownload = null;
           // fileToDownload = new System.IO.FileInfo(@filePath);
           //// Response.ContentType = ReturnExtension(fileToDownload.Extension.ToLower());
           // Response.ContentType = "application/xml";
           // System.String disHeader = "Attachment; Filename=\"" + Server.UrlPathEncode(sFileName) + "\"";
           // Response.AppendHeader("Content-Disposition", disHeader);
           // Response.Flush();
           // Response.WriteFile(fileToDownload.FullName);
           // HttpContext.Current.ApplicationInstance.CompleteRequest();

           // //for (int i = 0; i < gvDetails.Rows.Count ; i++)
           // //{
           // //    downloadfiles((gvDetails.Rows[i].FindControl("FilePath") as TextBox).Text);
           // //}
           // //for (int iRowCnt = 0; iRowCnt < gvDetails.Rows.Count; iRowCnt++)
           // //{
           // //}

        }
        public static void CreateDirectory(DirectoryInfo directory)
        {
            if (!directory.Parent.Exists)
                CreateDirectory(directory.Parent);
            directory.Create();
        }
        private void downloadfiles(string strFilePath)
        {

            //System.IO.File.Move(strFilePath, strFileDestPath);


            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ClearContent();
            //Response.Clear();
            //Response.ContentType = "application/xml";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //Response.TransmitFile(Server.MapPath("~/jad/") + filename);
            //Response.End();

        }

        protected void btnpath_Click(object sender, EventArgs e)
        {
           
               

            //FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            ////FolderBrowserDialog folderDlg = new FolderBrowserDialog();

            //folderDlg.ShowNewFolderButton = true;
      
            //// Show the FolderBrowserDialog.
            //DialogResult result = folderDlg.ShowDialog();
            ////if (result == DialogResult.OK)
            ////{
            ////    txtdrive.Text = folderDlg.SelectedPath;
            ////    Environment.SpecialFolder root = folderDlg.RootFolder;
            ////}
            

            ////using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            ////            {
            ////    if (dialog.ShowDialog() == DialogResult.OK)
            ////    {
            ////        txtdrive.Text = dialog.SelectedPath;
            ////    }
            ////}
            ////string path = "~/";
            ////GetFilesFromDirectory(path);
        }
        private static void GetFilesFromDirectory(string DirPath)
        {
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(DirPath);
                //FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                //foreach (FileInfo FI in FileList)
                //{
                //    Console.WriteLine(FI.FullName);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}