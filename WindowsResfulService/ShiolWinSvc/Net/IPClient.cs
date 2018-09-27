using System;

namespace SimpleSocketClient
{
    using SimpleSocketClient;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    public class IPClient
    {
        
        public delegate void OnConnectedEventHandler();
        public delegate void OnDisconnectedEventHandler(string data);
        public delegate void OnDataReceivedEventHandler(string data);

        /// <summary>
        /// Called when there is an exception thrown while trying to connect to the remote location.
        /// </summary>
        
        public event OnConnectedEventHandler OnConnected;
        public event OnDisconnectedEventHandler OnDisconnected;
        public event OnDataReceivedEventHandler OnDataReceived;

        private AsyncSocket asyncSocket;
        

        public TaskScheduler taskUI { set; get; }

        /// <summary>
        /// Gets the hostname set in the constructor
        /// </summary>
        public string Hostname { get; private set; }
        /// <summary>
        /// Gets the port set in the constructor
        /// </summary>
        public int Port { get; private set; }
        
        public bool IsConnected()
        {
            return asyncSocket.IsConnected;
        }
        /// <summary>
        /// Creates an asynchronous client that deals with the xmpp protocol
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        public IPClient(string hostname, int port)
        {
            this.Hostname = hostname;
            this.Port = port;
           

            // create socket
            this.asyncSocket = new AsyncSocket(this.Hostname, this.Port);
            this.asyncSocket.OnConnected += onConnected;
            this.asyncSocket.OnDisconnected += AsyncSocket_OnDisconnected;
            this.asyncSocket.OnConnectException += onConnectionException;
            this.asyncSocket.OnDataReceived += onDataReceived;
            this.asyncSocket.OnSocketUnexpectedClosed += onSocketUnexpectedlyClosed;
            
        }

        private void AsyncSocket_OnDisconnected()
        {
            if (this.OnDisconnected != null)
                OnDisconnected("Disconnected.");
        }

        public void BeginConnect()
        {
            this.asyncSocket.BeginConnect();
        }

        public void Disconnect()
        {
            this.asyncSocket.Disconnect();
        }

        public void Send(String  str)
        {
            // convert document into bytes
            var data = Encoding.UTF8.GetBytes(str);

            // send through our socket
            this.asyncSocket.Send(data);
        }

        private void onConnected()
        {
            // bubble event upwards
            if (this.OnConnected != null)
            {   
               this.OnConnected();
            }
        }

        private void onConnectionException(Exception ex)
        {
            // to do: log something
            // otherwise..nothing to do, really.
            
            // bubble exception upwards
            if (this.OnDisconnected != null)
            {
                this.OnDisconnected("Exception! :" + ex.Message);
            }
        }

        private void onDataReceived(byte[] buffer, int length)
        {
            // create stream from buffer
            var stream = new MemoryStream(buffer, 0, length);
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            if (this.OnDataReceived != null)
            {
                
                if (taskUI != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        this.OnDataReceived(text);
                    }, System.Threading.CancellationToken.None, TaskCreationOptions.None, taskUI);
                }
                else
                {
                    this.OnDataReceived(text);
                }
            }
        }

        private void onSocketUnexpectedlyClosed(IOException ex)
        {
            // to do: log something

            // bubble exception upwards
            if (this.OnDisconnected != null)
            {
                this.OnDisconnected("OnSocketUnexpectedlyClosed:" + ex.Message);
            }
        }
        
    }
}
