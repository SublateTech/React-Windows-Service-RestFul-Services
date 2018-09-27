using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiolWinSvc
{
    public class UniversalDeviceProvider : IDeviceProvider
    {


        public delegate void OnConnectExceptionHandler(Exception ex);
        public delegate void OnConnectedEventHandler();
        public delegate void OnSocketUnexpectedlyClosedHandler(IOException ex);
        public delegate void OnDataReceivedEventHandler(string data);

        /// <summary>
        /// Called when there is an exception thrown while trying to connect to the remote location.
        /// </summary>
        public virtual event OnConnectExceptionHandler OnConnectException;
        public virtual event OnConnectedEventHandler OnConnected;
        public virtual event OnSocketUnexpectedlyClosedHandler OnSocketUnexpectedlyClosed;
        public virtual event OnDataReceivedEventHandler OnDataReceived;

        public UniversalDeviceProvider()
        {   
        }
        public virtual void Connect()
        {
            throw new NotImplementedException();
        }
        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
