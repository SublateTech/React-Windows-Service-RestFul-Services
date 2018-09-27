using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Configuration;
using System.Globalization;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;


namespace ShiolWinSvc
{
    class LogFile
    {
        
        static XmlLogger LogTramas = null;
        //static string temp_dir = System.AppDomain.CurrentDomain.BaseDirectory;

        static readonly LoggingProvider.ILogger logger = LoggingProvider.GetLogger(typeof(LogFile));
        
        static public void saveRegistro(String text, levels logLevel)
        {
            try
            {
                
                switch (logLevel)
                {
                    case levels.debug:
                        logger.Debug(text);
                        break;
                    case levels.error:
                        logger.Error(text);
                        break;
                    case levels.info:
                        logger.Info(text);
                        break;
                    case levels.warning:
                        logger.Warn(text);
                        break;
                    default:
                        logger.Info(text);
                        break;
                }

            }
            catch (Exception ex)
            {
                EventLog ev = new EventLog();
                ev.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        static public void saveEvent(UniversalFrameProvider processed )
        {
            try
            {
                if (LogTramas != null)
                {
                    string strDate = LogTramas.getFileName().Mid("Tramas".Length + 1, 10).Replace("-", "");
                    DateTime date = DateTime.ParseExact(strDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                    if (date != DateTime.Today)
                    {
                        LogTramas = null;
                    }
                }

                if (LogTramas == null)
                    LogTramas = new XmlLogger("Tramas", ShiolConfiguration.Instance.Config.LogDirectory);
            
                LogTramas.saveEvent(processed);
            }
            catch (Exception ex)
            {
                EventLog ev = new EventLog();
                ev.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

            
        }

        public static void Main(string[] args)
        {

            UniversalFrameProvider uFrame = new UniversalFrameProvider();
            uFrame.Data = "20/01/18 08:29PM   106 28 <I>12793301               0'03 00:00'54     ";
            uFrame.Process();
            LogFile.saveEvent(uFrame);

            /*
            
            XmlValidatingReader vr =  new XmlValidatingReader(new XmlTextReader("logfile.xml"));
            vr.ValidationType = ValidationType.None;
            vr.EntityHandling = EntityHandling.ExpandEntities;

            XmlDocument doc = new XmlDocument();
            doc.Load(vr);
            
            foreach (XmlElement element in doc.SelectNodes("//Event"))
            {
                string file = element.ChildNodes[0].InnerText;
               // string date = element.ChildNodes[1].InnerText;

                
                Console.WriteLine("{0}", file);

                XmlNode element1 = element["Processed"];
                
                Console.WriteLine("{0}-{1} ", element1["Date"].Name, element1["Date"].InnerText);
                Console.WriteLine("{0}-{1} ", element1["Time"].Name, element1["Time"].InnerText);
                Console.WriteLine("{0}-{1} ", element1["Anexo"].Name, element1["Anexo"].InnerText);
                Console.WriteLine("{0}-{1} ", element1["DialedNumber"].Name, element1["DialedNumber"].InnerText);
                Console.WriteLine("{0}-{1} ", element1["UserID"].Name, element1["UserID"].InnerText);
                Console.WriteLine("{0}-{1} ", element1["Duration"].Name, element1["Duration"].InnerText);
                
                
                foreach (XmlNode element2 in element["Processed"])
                {
                    Console.WriteLine("{0}-{1} ", element2.Name, element2.InnerText);
                    
                }
                
                
            }
            
            */
            Console.ReadKey();
        }
    }
}
