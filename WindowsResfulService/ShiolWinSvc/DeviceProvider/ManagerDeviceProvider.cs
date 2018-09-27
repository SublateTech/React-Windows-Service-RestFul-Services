using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiolWinSvc
{
    public class ManagerDeviceProvider : AbstractServiceProvider
    {
        private UniversalDeviceProvider uDeviceProvider = null;
        private UniversalFrameProvider uFrameProvider = new UniversalFrameProvider();
        
        /// <SUMMARY>
        /// Start Service
        /// </SUMMARY>
        public override void StartService()
        {
            /*
            Console.WriteLine("Device: " + ShiolConfiguration.Instance.Config.Communication.Conexion);
            if (ShiolConfiguration.Instance.Config.Communication.Conexion == "IP")
                uDeviceProvider = new IPDeviceProvider();
            else if (ShiolConfiguration.Instance.Config.Communication.Conexion == "Serial")
                uDeviceProvider = new SerialDeviceProvider();
            else
            {
                LogFile.saveRegistro("Conexion Indefinida:" + ShiolConfiguration.Instance.Config.Communication.Conexion, levels.error);
                return;
            }
            */

            uDeviceProvider.OnDataReceived += UDeviceProvider_OnDataReceived;
            uDeviceProvider.Connect();
        }

        public void UDeviceProvider_OnDataReceived(string data)
        {
            uFrameProvider.Data = data.Purify();
            if (uFrameProvider.Data != null )
            {
                uFrameProvider.Process();

               // ShiolSqlServerProvider SqlProvider = new ShiolSqlServerProvider();
               // SqlProvider.Save(ref uFrameProvider);

                LogFile.saveEvent(uFrameProvider);
                
            }
        }

        /// <SUMMARY>
        /// Stop Service
        /// </SUMMARY>
        public override void StopService()
        {
            if (uDeviceProvider!=null)
                uDeviceProvider.Disconnect();
        }
        
    }
}
