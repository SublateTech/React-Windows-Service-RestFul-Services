using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShiolWinSvc
{
    //    [XmlRoot("Processed")]
    //[XmlType("Processed")]
    [Serializable]
    public class UniversalFrameProvider
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }
        public string DialedNumber { get; set; }
        public int Duration { get; set; }
        public string Anexo { get; set; }
        public string Data { get; set; }
        public int Type { get; set; } //Type 0=Saliente, 1=Entrante
        
        public string  Shiol { get; set; }
        public string ToString()
        {
            return $"<Processed><Date>{Date.ToShortDateString()}</Date><Time>{Time.ToShortTimeString()}</Time><Anexo>{Anexo}</Anexo><DialedNumber>{DialedNumber}</DialedNumber><Duration>{Duration}</Duration><UserID>{User}</UserID></Processed>";
        }

        public bool Process()
        {
            try
            {
                
            } catch (Exception ex)
            { 
                LogFile.saveRegistro(ex.Message, levels.error);
            }
            return true;
        }

        public static string SecondsToTimeFormat(int secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)t.TotalHours,
                t.Minutes,
                t.Seconds);
        }

        public static string SecondsToDurationFormat(int secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            return string.Format("{0:D2}:{1:D2}'{2:D2}",
                (int)t.TotalHours,
                t.Minutes,
                t.Seconds);
        }
        
    }

    public static class Extension
    {
        public static int WordCount(this string str)
        {
            string[] userString = str.Split(new char[] { ' ', '.', '?' },
                                        StringSplitOptions.RemoveEmptyEntries);
            int wordCount = userString.Length;
            return wordCount;
        }
        public static int TotalCharWithoutSpace(this string str)
        {
            int totalCharWithoutSpace = 0;
            string[] userString = str.Split(' ');
            foreach (string stringValue in userString)
            {
                totalCharWithoutSpace += stringValue.Length;
            }
            return totalCharWithoutSpace;
        }

        /// <summary>
        ///     Extracts the middle part of the input string limited with the length parameter
        /// </summary>
        /// <param name="val">The input string to take the middle part from</param>
        /// <param name="start">The input string to take the middle part from</param>
        /// <param name="length">The total number characters to take from the input string</param>
        /// <returns>The substring starting at startIndex 0 until length</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Mid(this string val, int start, int length)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }
            return val.Substring(start, length);
        }

        /// <summary>
        ///     Extracts the left part of the input string limited with the length parameter
        /// </summary>
        /// <param name="val">The input string to take the left part from</param>
        /// <param name="length">The total number characters to take from the input string</param>
        /// <returns>The substring starting at startIndex 0 until length</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Left(this string val, int length)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }
            return val.Substring(0, length);
        }

        /// <summary>
        ///     Extracts the right part of the input string limited with the length parameter
        /// </summary>
        /// <param name="val">The input string to take the right part from</param>
        /// <param name="length">The total number characters to take from the input string</param>
        /// <returns>The substring taken from the input string</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Right(this string val, int length)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }
            return val.Substring(val.Length - length);
        }

        /// <summary>
        ///     Checks if date with dateFormat is parse-able to System.DateTime format returns boolean value if true else false
        /// </summary>
        /// <param name="data">String date</param>
        /// <param name="dateFormat">date format example dd/MM/yyyy HH:mm:ss</param>
        /// <returns>boolean True False if is valid System.DateTime</returns>
        public static bool IsDateTime(this string data, string dateFormat)
        {
            // ReSharper disable once RedundantAssignment
            DateTime dateVal = default(DateTime);
            return DateTime.TryParseExact(data, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dateVal);
        }

        /// <summary>
        ///     Converts the string representation of a number to its 32-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int32</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int32.MinValue or greater than System.Int32.MaxValue
        /// </remarks>
        public static int ToInt32(this string value)
        {
            int number;
            Int32.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts the string representation of a number to its 64-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int64</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int64.MinValue or greater than System.Int64.MaxValue
        /// </remarks>
        public static long ToInt64(this string value)
        {
            long number;
            Int64.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts the string representation of a number to its 16-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int16</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int16.MinValue or greater than System.Int16.MaxValue
        /// </remarks>
        public static short ToInt16(this string value)
        {
            short number;
            Int16.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts the string representation of a number to its System.Decimal equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Decimal</returns>
        /// <remarks>
        ///     The conversion fails if the s parameter is null, is not a number in a valid format, or represents a number
        ///     less than System.Decimal.MinValue or greater than System.Decimal.MaxValue
        /// </remarks>
        public static Decimal ToDecimal(this string value)
        {
            Decimal number;
            Decimal.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts string to its boolean equivalent
        /// </summary>
        /// <param name="value">string to convert</param>
        /// <returns>boolean equivalent</returns>
        /// <remarks>
        ///     <exception cref="ArgumentException">
        ///         thrown in the event no boolean equivalent found or an empty or whitespace
        ///         string is passed
        ///     </exception>
        /// </remarks>
        public static bool ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value");
            }
            string val = value.ToLower().Trim();
            switch (val)
            {
                case "false":
                    return false;
                case "f":
                    return false;
                case "true":
                    return true;
                case "t":
                    return true;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    throw new ArgumentException("Invalid boolean");
            }
        }
    }

    public static class ShiolExtension
    {
        /// <summary>
        ///     Convert input to Shiol Tarificador Date
        /// </summary>
        /// <param name="val">The input string in the form start, length, format </param>
        /// <returns>The converted date</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static DateTime ToDate(this string val, string param)
        {
            String[] str = param.Split(',');

            if (str.Length < 3)
            {
                throw new ArgumentNullException("number of arguments:" + val);
            }

            int start = str[0].ToString().ToInt32();
            int length = str[1].ToString().ToInt32();
            string format = str[2].ToString();



            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }

            string strDate = val.Substring(start, length);

            //IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            DateTime date = DateTime.Now;

            //format = "ddMMyy";
            //strDate = strDate.Replace("/", "").Replace("-", "");

            strDate = strDate.Replace("/", "").Replace("-", "");
            format = format.Replace("Y", "y").Replace("D", "d").Replace("m", "M").Replace("/", "").Replace("-", "");

            try
            {
                // date = DateTime.Parse(strDate, culture);
                //  DateTime.TryParseExact(strDate, format, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);
                //date = DateTime.ParseExact(strDate, format, CultureInfo.InvariantCulture);
                //    DateTime temp = DateTime.ParseExact(strDate, format, culture);
                date = DateTime.ParseExact(strDate, format, CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                //throw new ArgumentOutOfRangeException("date","date cannot be parsed");
                //LogFile.saveRegistro("date cannot be parsed." + ex.Message);
            }

            return date;
        }
        /// <summary>
        ///     Convert input to Shiol Tarificador Time
        /// </summary>
        /// <param name="val">The input string in the form start, length, format </param>
        /// <returns>The converted date</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static DateTime ToTime(this string val, string param)
        {
            String[] str = param.Split(',');

            if (str.Length < 3)
            {
                throw new ArgumentNullException("number of arguments:" + val);
            }

            int start = str[0].ToString().ToInt32();
            int length = str[1].ToString().ToInt32();
            string format = str[2].ToString();


            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }
            
            string strTime = val.Substring(start, length);
            format = format.Replace("HH", "hh").Replace("MM", "mm").Replace("TT", "tt");

            DateTime time = DateTime.MinValue;
            try
            {
                time = DateTime.ParseExact(strTime, format, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("time",
                    "time cannot be parsed" + ex.Message);
                //LogFile.saveRegistro("Date Extension Function error." + ex.Message);
            }

            return time;
        }
        /// <summary>
        ///     Convert input to Shiol Tarificador Dialed Number
        /// </summary>
        /// <param name="val">The input string in the form start, length, format </param>
        /// <returns>The converted date</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string ToDialedNumber(this string val, string param, out int type)
        {
            String[] str = param.Split(',');

            if (str.Length < 2)
            {
                throw new ArgumentNullException("number of arguments:" + val);
            }

            int start = str[0].ToString().ToInt32();
            int length = str[1].ToString().ToInt32();


            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }

            type = 0;
            string strDialed = val.Substring(start, length);
            try
            {
                //Modificación para INKARY
                if (strDialed.IndexOf("<I>") > -1)
                {
                    type = 1;
                    strDialed = strDialed.Replace("<I>", "");
                }
                strDialed = strDialed.Trim();
                //Grabar("Es una llamada entrante.");
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("time",
                    "time cannot be parsed" + ex.Message);
                //LogFile.saveRegistro("Date Extension Function error." + ex.Message);
            }

            return strDialed;
        }
        /// <summary>
        ///     Convert input to Shiol Tarificador Anexo Number
        /// </summary>
        /// <param name="val">The input string in the form start, length, format </param>
        /// <returns>The converted date</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Mid(this string val, string param)
        {
            String[] str = param.Split(',');

            if (str.Length < 2)
            {
                throw new ArgumentNullException("number of arguments:" + val);
            }

            int start = str[0].ToString().ToInt32();
            int length = str[1].ToString().ToInt32();


            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }


            string strResult = "";
            try
            {
                strResult = val.Substring(start, length);
                strResult = strResult.Trim();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("Anexo",
                    "time cannot be parsed" + ex.Message);
                //LogFile.saveRegistro("Date Extension Function error." + ex.Message);
            }

            return strResult;
        }
        public static string Purify(this string val)
        {
            if (val == null)
                return null;

            val = val.Replace('\n', ' ').TrimStart();
            
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }

            if (val.Length < ShiolExtension.getMinimumLength())  //ShiolConfiguration.Instance.Config.MinLengthFrame)
            {
                LogFile.saveRegistro($"Trama ({val.Length})debe ser más grande que el tamaño mínimo = {ShiolExtension.getMinimumLength()}", levels.error);
                return null;
            }

            

            
            return val;

        }
        /// <summary>
        ///     Convert input to Shiol Tarificador Duration
        /// </summary>
        /// <param name="val">The input string in the form start, length, format </param>
        /// <returns>Total Duration in Seconds</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static int ToDuration(this string val, string param)
        {
            String[] str = param.Split(',');

            if (str.Length < 3)
            {
                throw new ArgumentNullException("number of arguments:" + val);
            }

            int start = str[0].ToString().ToInt32();
            int length = str[1].ToString().ToInt32();
            string format = str[2].ToString();


            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }

            string strTime = val.Substring(start, length);

            DateTime date = DateTime.MinValue;
            format = format.Replace("'", ":");
            strTime = strTime.Replace("H", "h").Replace("M", "m").Replace("S", "s").Replace("'", ":");
            try
            {
                date = DateTime.ParseExact(strTime, format, CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                LogFile.saveRegistro("Duration Extension Function error." + ex.Message,levels.error);
            }


            int totalSegundos = 0;

            totalSegundos = date.Second + (date.Minute * 60) + (date.Hour * 3600);
            
            return totalSegundos;
        }
        public static int getOffset(string param)
        {
            return param.Split(',')[0].ToInt32() + param.Split(',')[1].ToInt32();
        }
        public static int getMinimumLength()
        {
            int minimum = 0;
            
            
            return minimum;
        }
        public static int SecondsToMinutes(int secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            if (t.Seconds > 0)
                t = TimeSpan.FromSeconds(secs + 60);
            return t.Hours * 60 + t.Minutes;
        }
    }

}
