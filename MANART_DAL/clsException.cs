using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Diagnostics;

namespace MANART_DAL
{
    /// <summary>
    /// Summary description for clsException
    /// </summary>
    public class clsException
    {
        public clsException()
        {

        }

        public void SendException(Exception objectException)
        {
            clsEventLog objectEventLog = null;
            string ErrorMessage = null;
            string sExceptionType = objectException.GetType().ToString().Substring(7);
            switch (sExceptionType)
            {
                case "SystemException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "AccessException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "ArgumentException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "ArgumentNullException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "ArgumentOutOfRangeException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "ArithmeticException ":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "ArrayTypeMismatchException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "BadImageFormatException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "CoreException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "DivideByZeroException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;

                    break;
                case "FormatException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "IndexOutOfRangeException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "InvalidCastExpression":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;

                    break;
                case "InvalidOperationException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "MissingMemberException ":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;

                    break;
                case "NotFiniteNumberException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "NullReferenceException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "OutOfMemoryException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "StackOverflowException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "PathTooLongException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "DirectoryNotFoundException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "UnauthorizedAccessException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "FileNotFoundException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "SecurityException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;
                case "OverFlowException":
                    objectEventLog = new clsEventLog("App");
                    ErrorMessage = objectException.Message + " " + objectException.Source + " " + objectException.TargetSite + " " + objectException.StackTrace;
                    objectEventLog.WriteToLog(ErrorMessage, EventLogEntryType.Error, clsEventLog.CategoryType.None, clsEventLog.EventIDType.NA);
                    ErrorMessage = null;
                    objectEventLog = null;
                    break;

            }
        }
    }
}
