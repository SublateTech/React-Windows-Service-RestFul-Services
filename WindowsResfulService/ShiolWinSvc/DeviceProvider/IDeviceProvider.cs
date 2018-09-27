using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiolWinSvc
{
    public interface IDeviceProvider
    {
        /// <SUMMARY>
        /// Process Connection
        /// </SUMMARY>
        void Connect();

        /// <SUMMARY>
        /// Process Disconnection
        /// </SUMMARY>
        void Disconnect();



    }
}
