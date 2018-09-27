using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace ShiolWinSvc
{
    public enum levels { all, debug, info, error, warning };
    /// <summary>
    /// class to get settings from config.xml
    /// config.xml must have ellements path,format,level with artibutes
    /// </summary>
    public class ConfigFromFile
    {

        /// <summary>
        /// Get format log file from config.xml
        /// </summary>
        /// <returns></returns>
        public static string GetFormatLogger()
        {

            if (!File.Exists("config.xml"))
            {
                Console.WriteLine("config.xml does not exist");
                return null;
            }
            XmlTextReader textReader = new XmlTextReader("config.xml");
            textReader.ReadToFollowing("format");
            textReader.MoveToFirstAttribute();
            return textReader.Value;
        }
        /// <summary>
        /// Get level from config.xml
        /// </summary>
        /// <returns></returns>
        public static levels GetLevelLogger()
        {
            try
            {

                if (!File.Exists("config.xml"))
                {
                    Console.WriteLine("config.xml does not exist");
                    return levels.all;
                }
                XmlTextReader textReader = new XmlTextReader("config.xml");
                textReader.ReadToFollowing("level");
                textReader.MoveToFirstAttribute();
                levels level;
                Enum.TryParse<levels>(textReader.Value, out level);
                return level;
            }
            catch (XmlException e)
            {
                Console.WriteLine("GetLevelLogger" + e.Message);
                return levels.all;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetLevelLogger " + e.Message);
                return levels.all;
            }
        }
        /// <summary>
        /// Get path for log file from config.xml
        /// </summary>
        /// <returns></returns>
        public static string GetPathLogger()
        {
            try
            {
                if (!File.Exists("config.xml"))
                {
                    Console.WriteLine("config.xml does not exist");
                    return null;
                }
                XmlTextReader textReader = new XmlTextReader("config.xml");
                textReader.ReadToFollowing("path");
                textReader.MoveToFirstAttribute();
                return textReader.Value;
            }
            catch (XmlException e)
            {
                Console.WriteLine("GetPathLogger" + e.Message);
                return null;
            }
        }
    }

}
