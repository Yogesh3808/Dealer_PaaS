using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Diagnostics;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.Win32;
using System.IO;
using System.Data.OleDb;
using System.Data;
using MANART_DAL;
using System.Security.AccessControl;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsExcel
    /// </summary>
    public class clsExcel
    {
        public clsExcel()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public enum OfficeComponent
        {
            Word,
            Excel,
            PowerPoint,
            Outlook
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// gets the component's path from the registry. if it can't find it - retuns 
        ///an empty string
        /// <span class="code-SummaryComment"></summary></span>
        private string GetComponentPath(OfficeComponent _component)
        {
            const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";
            string toReturn = string.Empty;
            string _key = string.Empty;

            switch (_component)
            {
                case OfficeComponent.Word:
                    _key = "winword.exe";
                    break;
                case OfficeComponent.Excel:
                    _key = "excel.exe";
                    break;
                case OfficeComponent.PowerPoint:
                    _key = "powerpnt.exe";
                    break;
                case OfficeComponent.Outlook:
                    _key = "outlook.exe";
                    break;
            }

            //looks inside CURRENT_USER:
            RegistryKey _mainKey = Registry.CurrentUser;
            try
            {
                _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                if (_mainKey != null)
                {
                    toReturn = _mainKey.GetValue(string.Empty).ToString();
                }
            }
            catch
            { }

            //if not found, looks inside LOCAL_MACHINE:
            _mainKey = Registry.LocalMachine;
            if (string.IsNullOrEmpty(toReturn))
            {
                try
                {
                    _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                    if (_mainKey != null)
                    {
                        toReturn = _mainKey.GetValue(string.Empty).ToString();
                    }
                }
                catch
                { }
            }

            //closing the handle:
            if (_mainKey != null)
                _mainKey.Close();

            return toReturn;
        }
        /// <span class="code-SummaryComment"><summary></span>
        /// Gets the major version of the path. if file not found (or any other        
        /// exception occures - returns 0
        /// <span class="code-SummaryComment"></summary></span>
        private int GetMajorVersion(string _path)
        {
            int toReturn = 0;
            if (File.Exists(_path))
            {
                try
                {
                    FileVersionInfo _fileVersion = FileVersionInfo.GetVersionInfo(_path);
                    toReturn = _fileVersion.FileMajorPart;
                }
                catch
                { }
            }
            return toReturn;
        }
        public string Getversion()
        {
            int _ExcelVersion = GetMajorVersion(GetComponentPath(OfficeComponent.Excel));
            string retVal = "application/vnd.ms-excel";
            if (_ExcelVersion == 11) //"11 – Office 2003"
                return "application/vnd.ms-excel";
            else if (_ExcelVersion == 12) //"12 – Office 2007"
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return retVal;
        }
        public string FileExtension()
        {
            int _ExcelVersion = GetMajorVersion(GetComponentPath(OfficeComponent.Excel));

            string retVal = ".xls";
            if (_ExcelVersion == 11) //"11 – Office 2003"
                retVal = ".xls";
            else if (_ExcelVersion == 12) //"12 – Office 2007"
                retVal = ".xlsx";
            return retVal;
        }
        public void DownloadInExcelFile(System.Data.DataTable dataTable, string FileName, string SheetName)
        {
            string strFileExtType = System.IO.Path.GetExtension(FileName).ToString().ToLower();
            FileName = FileName.Replace(strFileExtType, "");
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add((SheetName != "") ? SheetName : "Sheet1");

                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {
                    worksheet.Column(i).AutoFit();

                    if (dataTable.Columns[i - 1].DataType == System.Type.GetType("System.DateTime"))
                    {
                        worksheet.Column(i).Style.Numberformat.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    }
                }

                HttpContext.Current.Response.ContentType = Getversion();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + FileExtension());
                HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }

        public DataTable UploadExcelFile(FileUpload objUpload, string FilePath, string SheetName, ref string ErrorMsg)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();
                if ((objUpload.HasFile))
                {

                    OleDbConnection conn = new OleDbConnection();
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataAdapter da = new OleDbDataAdapter();

                    DataSet dsNotUploadchassisDetails = new DataSet();
                    System.Data.DataTable dtLayout = new System.Data.DataTable();
                    string query = null;
                    string connString = "";

                    //HttpContext.Current.Server.MapPath("~/DownloadFiles/DownloadsBTST/" + Location.sDealerCode + "_BTST_WarrantyClaims.xlsx")
                    string strFileExtType = System.IO.Path.GetExtension(objUpload.FileName).ToString().ToLower();
                    string strFileName = objUpload.FileName.Replace(strFileExtType, "");
                    string strFileType = strFileExtType;


                    //Check file type
                    if (strFileExtType == ".xls" || strFileExtType == ".xlsx")
                    {
                        if (!Directory.Exists(FilePath))
                        {
                            Directory.CreateDirectory(FilePath);
                            DirectoryInfo dInfo = new DirectoryInfo(FilePath);
                            DirectorySecurity dSecurity = dInfo.GetAccessControl();
                            dSecurity.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                            dInfo.SetAccessControl(dSecurity);
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (File.Exists(FilePath + strFileName + strFileType))
                        {
                            File.Delete(FilePath + strFileName + strFileType);
                        }
                        objUpload.SaveAs(FilePath + strFileName + strFileType);
                    }
                    else
                    {

                        ErrorMsg = "Only excel files allowed";
                        return null;
                    }

                    string strNewPath = (FilePath + strFileName + strFileType);

                    //Connection String to Excel Workbook
                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    //this.dataGridView1.DataSource = sheet.ExportDataTable();

                    //Create the connection object
                    conn = new OleDbConnection(connString);
                    //Open connection
                    DataTable dtSheet = null;
                    if (conn.State == ConnectionState.Closed)
                        try
                        {
                            conn.Open();
                            var adc = conn;
                            dtSheet = new DataTable();
                            dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            query = "SELECT * FROM [" + dtSheet.Rows[0]["TABLE_NAME"].ToString() + "]";
                            //Create the command object
                            cmd = new OleDbCommand(query, conn);
                            da = new OleDbDataAdapter(cmd);
                            ds = new DataSet();
                            da.Fill(ds);

                            //}
                            //else
                            //{
                            //    string sMessage = "Error : Data Uploading Failed!";
                            //    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");

                            //}


                        }
                        catch (Exception ex)
                        {
                            ErrorMsg = "Error : Data Uploading Failed!";
                            return null;
                        }
                        finally
                        {
                            if (dtSheet != null) dtSheet = null;
                        }
                    return ds.Tables[0];
                }
                else
                {
                    ErrorMsg = "Please select an excel file first";
                    return null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return null;
            }


        }
    }
}
