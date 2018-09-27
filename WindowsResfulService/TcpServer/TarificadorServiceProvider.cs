using System;
using System.Text;
using System.Windows.Forms;
using TcpLib;

namespace TcpServerDemo
{
	/// <SUMMARY>
	/// EchoServiceProvider. Just replies messages received from the clients.
	/// </SUMMARY>
	public class TarificadorServiceProvider: TcpServiceProvider
	{
		private string _receivedStr;
        private ConnectionState state;


        public override object Clone()
		{
			return new TarificadorServiceProvider();
		}

		public override void OnAcceptConnection(ConnectionState state)
		{
			_receivedStr = "";
			if(!state.Write(Encoding.UTF8.GetBytes("-"), 0, 1))
				state.EndConnection(); //if write fails... then close connection
		}

        public void Send(String _receivedStr)
        {   
            state.Write(Encoding.UTF8.GetBytes(_receivedStr), 0,
                _receivedStr.Length);
            _receivedStr = "";
        }

		public override void OnReceiveData(ConnectionState state)
		{
            this.state = state;
			byte[] buffer = new byte[1024];
			while(state.AvailableData > 0)
			{
				int readBytes = state.Read(buffer, 0, 1024);
				if(readBytes > 0)
				{
					_receivedStr += Encoding.UTF8.GetString(buffer, 0, readBytes);
					if(_receivedStr =="SMDR\r\n")
					{
                        _receivedStr = "Password:"; 

                        state.Write(Encoding.UTF8.GetBytes(_receivedStr), 0,
							_receivedStr.Length);
						_receivedStr = "";
					}

                    if (_receivedStr == "PCCSMDR\r\n")
                    {
                        _receivedStr = "******";
                        state.Write(Encoding.UTF8.GetBytes(_receivedStr), 0,
                            _receivedStr.Length);
                        _receivedStr = "";
                    }
                }
				else state.EndConnection(); //If read fails then close connection
			}
		}


		public override void OnDropConnection(ConnectionState state)
		{
			//Nothing to clean here
		}
	}
}
