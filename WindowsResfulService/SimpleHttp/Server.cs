using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttp
{
    class Server
    {
        private Listener listener;
        
        public WEB.RestRequestTree m_requestTree { get; private set; }


        public Server(Listener listener, WEB.RestRequestTree requestTree)
        {
            this.listener = listener;
            m_requestTree = requestTree;
            
        }

        public async Task StartAsync(string port)
        {
            Console.Write("SimpleHttp server 0.2\n\n");
            Console.WriteLine("Initialising server on port {0}...", port);
            await this.listener.StartAsync(port, m_requestTree);
        }
    }
}
