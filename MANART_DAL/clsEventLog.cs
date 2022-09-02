using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;

namespace MANART_DAL
{
    public class clsEventLog
    {
        // Constructors
        public clsEventLog(string logName) :
            this(logName, Process.GetCurrentProcess().ProcessName, ".") { }

        public clsEventLog(string logName, string source) : this(logName, source, ".") { }

        public clsEventLog(string logName, string source, string machineName)
        {
            this.logName = logName;
            this.source = source;
            this.machineName = machineName;

            if (!EventLog.SourceExists(source, machineName))
            {
                EventSourceCreationData sourceData = new EventSourceCreationData(source, logName);
                sourceData.MachineName = machineName;

                EventLog.CreateEventSource(sourceData);
            }

            log = new EventLog(logName, machineName, source);
            log.EnableRaisingEvents = true;
        }

        // Fields
        private EventLog log = null;
        private string source = "";
        private string logName = "";
        private string machineName = ".";

        // Properties
        public string Name
        {
            get { return (logName); }
        }

        public string SourceName
        {
            get { return (source); }
        }

        public string Machine
        {
            get { return (machineName); }
        }

        // Methods

        public void DoBackup(string sLogName)
        {
            string sBackup = sLogName;  // could be for example "Application" 
            //   EventLog log = new EventLog();
            log.Source = sBackup;

            var query = from EventLogEntry entry in log.Entries orderby entry.TimeGenerated descending select entry;

            string sBackupName = sBackup + "Log";
            var xml = new XDocument(
                new XElement(sBackupName,
                    from EventLogEntry entry in log.Entries
                    orderby entry.TimeGenerated descending
                    select new XElement("Log",
                      new XElement("Message", entry.Message),
                      new XElement("TimeGenerated", entry.TimeGenerated),
                      new XElement("Source", entry.Source),
                      new XElement("EntryType", entry.EntryType.ToString())
                    )
                  )
                );

            DateTime oggi = DateTime.Now;
            string sToday = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            string path = String.Format("{0}_{1}.xml", sBackupName, sToday);
            xml.Save(Path.Combine(Environment.CurrentDirectory, path));
        }



        public void WriteToLog(string message, EventLogEntryType type,
        CategoryType category, EventIDType eventID)
        {
            if (log == null)
            {
                throw (new ArgumentNullException("log", "This Event Log has not been opened or has been closed."));
            }

            log.WriteEntry(message, type, (int)eventID, (short)category);
        }

        public void WriteToLog(string message, EventLogEntryType type, CategoryType category, EventIDType eventID, byte[] rawData)
        {
            if (log == null)
            {
                throw (new ArgumentNullException("log", "This Event Log has not been opened or has been closed."));
            }

            log.WriteEntry(message, type, (int)eventID, (short)category, rawData);
        }

        public EventLogEntryCollection GetEntries()
        {
            if (log == null)
            {
                throw (new ArgumentNullException("log", "This Event Log has not been opened or has been closed."));
            }

            return (log.Entries);
        }

        public void ClearLog()
        {
            if (log == null)
            {
                throw (new ArgumentNullException("log", "This Event Log has not been opened or has been closed."));
            }

            log.Clear();
        }

        public void CloseLog()
        {
            if (log == null)
            {
                throw (new ArgumentNullException("log", "This Event Log has not been opened or has been closed."));
            }
            log.Close();
            log = null;
        }

        public void DeleteLog()
        {
            if (EventLog.SourceExists(source, machineName))
            {
                EventLog.DeleteEventSource(source, machineName);
            }

            if (logName != "Application" &&
            logName != "Security" &&
            logName != "System")
            {
                if (EventLog.Exists(logName, machineName))
                {
                    EventLog.Delete(logName, machineName);
                }
            }

            if (log != null)
            {
                log.Close();
                log = null;
            }
        }
        public enum EventIDType
        {
            NA = 0,
            Read = 1,
            Write = 2,
            ExceptionThrown = 3,
            BufferOverflowCondition = 4,
            SecurityFailure = 5,
            SecurityPotentiallyCompromised = 6
        }

        public enum CategoryType : short
        {
            None = 0,
            WriteToDB = 1,
            ReadFromDB = 2,
            WriteToFile = 3,
            ReadFromFile = 4,
            AppStartUp = 5,
            AppShutDown = 6,
            UserInput = 7
        }




    }
}
