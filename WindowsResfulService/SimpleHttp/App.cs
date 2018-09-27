using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SimpleHttp
{
    public class App
    {
        private Server server;
        

        public App()
        {
            this.server = new Server(new Listener(), new WEB.RestRequestTree());
        }

        public async void Start (string port = "8080")
        {
            AutoResetEvent keepAlive = new AutoResetEvent(false);
            await this.server.StartAsync(port);
            keepAlive.WaitOne();
        }

        
        public void Get(string endpoint, Action<Request, Response> handler)
        {
            
            Register(RestMethod.GET, endpoint, handler);
            
        }

        public void Post(string endpoint, Action<Request, Response> handler)
        {
            Register(RestMethod.POST, endpoint, handler);
        }

        public void Put(string endpoint, Action<Request, Response> handler)
        {
            Register(RestMethod.PUT, endpoint, handler);
        }

        public void Delete(string endpoint, Action<Request, Response> handler)
        {
            Register(RestMethod.DELETE, endpoint, handler);
        }

        public void Register(RestMethod method, string uri, Action<Request, Response> handler)
        {
            //check if running
         //   if (IsListening)
         //       throw new ApplicationException("RestService is listening, cannot register new methods");

            this.server.m_requestTree.AddRequestHandler(uri, method, handler);
            
        }


    }
}
