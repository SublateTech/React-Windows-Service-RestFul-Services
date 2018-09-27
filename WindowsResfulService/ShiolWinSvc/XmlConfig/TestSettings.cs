using System;
using XmlConfig;
using System.Collections.Generic;

namespace XmlConfig
{
    [Serializable]
    public class ServiceShiolConfiguration : IEquatable<ServiceShiolConfiguration>
    {
        public string CentralType;
        public string LogDirectory;
        public ConectionConfiguration Communication;
        public List<DataFrameConfiguration> DataFrames { get; set; }
        public ServiceShiolConfiguration()
        {

            CentralType = "TYPE1";
            LogDirectory = "C:\\SHIOL";
            DataFrames = new List < DataFrameConfiguration > ();

            DataFrameConfiguration dataFrame = new DataFrameConfiguration();
            dataFrame.Prefix = "@";
            dataFrame.Name = "Trama Unica";
            dataFrame.ID = "1";
            dataFrame.Length = 65;

            //DataFrame
            dataFrame.Date = "0,5";
            dataFrame.DialedNumber = "26,13";
            dataFrame.Anexo = "9,3";
            dataFrame.UserID = dataFrame.Anexo;
            dataFrame.Time = "9,7";
            dataFrame.Duration = "57,8";
            dataFrame.DateFormat = "MM/DD/YYYY";
            DataFrames.Add(dataFrame);

            dataFrame = new DataFrameConfiguration();
            dataFrame.Prefix = "N";
            dataFrame.Name = "LLamada Saliente";
            dataFrame.ID = "3";
            dataFrame.Length = 65;

            //DataFrame
            dataFrame.Date = "0,5";
            dataFrame.DialedNumber = "26,13";
            dataFrame.Anexo = "9,3";
            dataFrame.UserID = dataFrame.Anexo;
            dataFrame.Time = "9,7";
            dataFrame.Duration = "57,8";
            dataFrame.DateFormat = "MM/DD/YYYY";
            dataFrame.Duration = "";
            
            DataFrames.Add(dataFrame);

            Communication = new ConectionConfiguration();
            Communication.CentralName = "SUPERXLS";
            Communication.Conexion = "IP";
            Communication.IP = "127.0.0.1";
            Communication.SerialSettings = "COM4,1200,8,None,None";
            Communication.User = "SMDR";
            Communication.Password = "PCCSMDR";
        }
        public void ClearRegistration()
        {   
        }

        #region Implementation of IEquatable<ServerRegistrationInfo>

        public bool Equals(ServiceShiolConfiguration other)
        {
            if (other == null)
                return false;

            return true;

        }

        #endregion
        
    }  
    public class DataFrameConfiguration
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Prefix { get; set; }
        public int Length { get; set; }
        public int MaxLength { get; set; }

        public string Date { get; set; }
        public string DateFormat { get; set; }
        public string Time { get; set; }
        public string UserID { get; set; }
        public string DialedNumber { get; set; }
        public string Duration { get; set; }
        public string Anexo { get; set; }

        /*
        public List<string> test { get; set; }
        public List<string> test2 { get; set; }
        */
    }
    public class ConectionConfiguration
    {
        public string CentralName { get; set; }
        public string Conexion { get; set; }
        public string IP { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string SerialPort { get; set; }
        public string SerialSettings { get; set; }
    }

    public class TestSettings: Settings
    {
        [SettingItem("ShiolConfigurationInfo")]
        public ServiceShiolConfiguration ShiolConfigurationInfo
        {
            get
            {
                var info = (ServiceShiolConfiguration) this["ShiolConfigurationInfo"];
                if (info == null)
                {
                    info = new ServiceShiolConfiguration();
                    this["ShiolConfigurationInfo"] = info;
                }
                return info;
            }
            set
            {
                this["ShiolConfigurationInfo"] = value;
            }
        }
    }
}
