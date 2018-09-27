using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleSocketClient;


namespace ShiolWinSvc
{
    public  class IPDeviceProvider:UniversalDeviceProvider
    {
        static private IPClient client;
        public override event OnDataReceivedEventHandler OnDataReceived;

        private CancellationToken token;
        private int connectionAttemps = 0;
        

        public override void Connect()
        {
            string hostname = ""; // ShiolConfiguration.Instance.Config.Communication.IP;
            int port = 8080; // ShiolConfiguration.Instance.Config.Communication.IPPort;
            
            client = new IPClient(hostname, port);
           // client.taskUI = TaskScheduler.FromCurrentSynchronizationContext();
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnDataReceived += Client_OnDataReceived;
            //Console.WriteLine("Connecting to " + ShiolConfiguration.Instance.Config.Communication.IP + ":" + ShiolConfiguration.Instance.Config.Communication.IPPort);
            client.BeginConnect();

        }

        async private void Client_OnDisconnected(string message)
        {
            client = null;
            Console.WriteLine("Disconnected!! " + message);
            LogFile.saveRegistro("Not Connected - " + message, levels.error);
            if (connectionAttemps == 0)
            {
                token = new CancellationToken(false);
                await TryReconnect();
            }
        }

        private void Client_OnConnected()
        {
            Console.WriteLine("Connected!!");
            LogFile.saveRegistro("Connected", levels.info);
            token = new CancellationToken(true);
            connectionAttemps = 0;
        }

        private  void Client_OnDataReceived(string data)
        {
 
            if (data.Trim() == "-")
            {
                LogFile.saveRegistro("Sending User...", levels.debug);
                //client.Send(ShiolConfiguration.Instance.Config.Communication.User +"\r\n");
                return;
            }

            if (data.ToLower().IndexOf("password") > -1)
            {
                LogFile.saveRegistro("Sending Password...", levels.debug);
                //client.Send(ShiolConfiguration.Instance.Config.Communication.Password + "\r\n");
                return;
            }

            if (data.IndexOf("*") > -1 || data.IndexOf("-----") > -1) // || (ShiolConfiguration.Instance.Config.Communication.User + "\r\n").IndexOf(data) > -1)
            {
                return;
            }

            if (data.IndexOf("Date") > -1)
            {
                LogFile.saveRegistro("Getting Pending Frames...", levels.debug);
                return;
            }

            Console.WriteLine(data);
            LogFile.saveRegistro(data, levels.debug);

            if (OnDataReceived != null && data.Length > 1)
                OnDataReceived(data);
 
        }
        
        public override void Disconnect()
        {
            client.Disconnect();
            client = null;
        }
        
        static private void startThreadClient()
        {
            while (!client.IsConnected())
            {

            }
        }
        //public async Task TryReconnect(CancellationToken token = default(CancellationToken))
        public async Task TryReconnect()
        {
            int tryIntervals = 5; // ShiolConfiguration.Instance.Config.TryReconnectEvery;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(tryIntervals), token);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task canceled...");
                    break;
                }
                if (client == null)
                {
                    LogFile.saveRegistro("Trying to connect... (every " + tryIntervals + " minutes)", levels.warning);
                    Console.WriteLine("Trying to connect...");
                    connectionAttemps++;
                    this.Connect();
                }
            }
        }

    }
}



