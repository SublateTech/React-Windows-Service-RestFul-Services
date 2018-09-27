using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ShiolWinSvc
{
    public class XmlLogger
    {
        private readonly string _directorypath;
        private readonly string _filepath;
        private readonly string _filename;

        protected XmlTextWriter xw;

        /// <summary>
        /// Creates OwnLog.Logger at current location with the default name Log-yyyy-MM-dd-HH-mm.txt.
        /// </summary>
        public XmlLogger() : this("Log")
        {   
        }

        /// <summary>
        /// Creates OwnLog.Logger at current location.
        /// </summary>
        /// <param name="name">Specifies the name before the date used in the filename.</param>
        public XmlLogger(string name) : this(name, "")
        {
        }

        /// <summary>
        /// Creates OwnLog.Logger.
        /// </summary>
        /// <param name="name">Specifies the name before the date used in the filename.</param>
        /// <param name="folder">Specifies the folder. Can be relative or absolute.</param>
        public XmlLogger(string name, string folder)
        {
            _filename = name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; //-HH-mm
            if (string.IsNullOrEmpty(folder))
            {
                _directorypath = folder;
                _filename = "";
                _filepath = "";
            }
            else
            {
                _directorypath = folder;
                _filepath = Path.Combine(folder,_filename);
            }
        }
                
        public string getFileName()
        {
            return _filename;
        }

        public void saveEvent(UniversalFrameProvider processed)
        {

            try
            {
                
                if (!File.Exists(_filepath))
                {
                    Directory.CreateDirectory(_directorypath);

                }

                StreamWriter sw = File.AppendText(_filepath);

                using (xw = new XmlTextWriter(sw))
                {
                    //XmlTextWriter xtw = new XmlTextWriter(sw);
                    xw.WriteStartElement("Event");
                    xw.WriteStartElement("Received");
                    xw.WriteElementString("Data", processed.Data.Replace("<", "&lt;").Replace(">", "&gt;"));
                    xw.WriteElementString("Date", $"{DateTime.Now}");
                    xw.WriteEndElement();

                    xw.WriteStartElement("Processed");
                    xw.WriteElementString("Date", processed.Date.ToString("dd-MM-yyyy"));
                    xw.WriteElementString("Time", processed.Time.ToString("hh:mm tt"));
                    xw.WriteElementString("Duration", UniversalFrameProvider.SecondsToDurationFormat(processed.Duration));
                    xw.WriteElementString("Anexo", processed.Anexo.ToString());
                    xw.WriteElementString("DialedNumber", processed.DialedNumber);
                    xw.WriteElementString("UserID", processed.User);
                    xw.WriteElementString("Shiol", processed.Shiol);
                    
                    xw.WriteEndElement();
                    xw.WriteRaw("\n");

                    xw.Close();
                }
            }
            catch (Exception ex)
            {
                EventLog ev = new EventLog();
                ev.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

        }
    }
}
