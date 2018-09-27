using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleHttp.WEB;
using System.Web;
using System.Threading;

namespace SimpleHttp
{
    class Listener
    {
        private HttpListener httpListener;
        public HttpListenerContext context;

        public Listener()
        {
            this.httpListener = new HttpListener();

         //   System.Web.HttpContext.Current.Server.MapPath("/build");
                    
            
        }

        
        public async Task StartAsync(string port, RestRequestTree requestTree)
        {
            m_requestTree = requestTree;
            
            this.httpListener.Prefixes.Add(string.Format("http://localhost:{0}/", port));
            this.httpListener.Start();

            Console.WriteLine("Listening for requests on port {0}.", port);

            Request request = await GetNextRequestAsync();

            while (request != null)
            {                
                if (!await TryRespondAsync(request))
                    Console.WriteLine("HTTP 404 for {0}.", request.Endpoint);
                
                request = await GetNextRequestAsync();
            }
        }

        private RestMethod HttpMethodToRestMethod(string method)
        {
            switch (method)
            {
                case "GET":
                    return RestMethod.GET;
                case "POST":
                    return RestMethod.POST;
                case "PUT":
                    return RestMethod.PUT;
                case "DELETE":
                    return RestMethod.DELETE;
                case "OPTIONS":
                    return RestMethod.OPTIONS;

                default:
                    throw new Exception("Bad http method");
            }
        }

        private async Task<bool> TryRespondAsync(Request httpRequest)
        {
            Response httpResponse = new Response(context.Response);

            var url = httpRequest.httpRequest.RawUrl;

            if (url == FAVICON_URL) // && IgnoreFaviconRequests)
                return true;

            RestRequestParameters parameters;
            
            
            httpResponse.httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");

            var method = HttpMethodToRestMethod(httpRequest.httpRequest.HttpMethod);
            if (method == RestMethod.OPTIONS)
            {
                
                httpResponse.httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                httpResponse.httpListenerResponse.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                httpResponse.httpListenerResponse.StatusCode = 200;
                //httpResponse.AddHeader("Access-Control-Allow‌​-Credentials", "true");
                //httpResponse.AddHeader("Access-Control-Max-Age", "1728000");

                //await httpResponse.SendAsync();
                httpResponse.httpListenerResponse.Close();

                return true;
            }
            


            var handler = m_requestTree.GetRequestHandler(url, method, out parameters);
            httpRequest.Parameters = parameters;
            httpRequest.PathVariables = GetVarParameters(httpRequest);

            if (handler == null)
            {
                Console.WriteLine("Error:404 {0}: {1} {2}", DateTime.Now, httpRequest.Method, httpRequest.Endpoint);

                //resource not found
                httpResponse.httpListenerResponse.StatusCode = 404;
                return false;
            }
            else
                Console.WriteLine("{0}: {1} {2}", DateTime.Now, httpRequest.Method, httpRequest.Endpoint);

            // TODO: Thread pooling!
            await Task.Run(() =>
            {
                handler(httpRequest, httpResponse);
            });
            return true;
        }

        internal WEB.RestRequestTree m_requestTree;

        private async Task<Request> GetNextRequestAsync()
        {
            try
            {
                this.context = await this.httpListener.GetContextAsync();
                HttpListenerRequest httpRequest = this.context.Request;
                return new Request(httpRequest);
            }
            catch (Exception)
            {
                //TODO: output/log exception
                return null;
            }
        }

        private const string FAVICON_URL = "/favicon.ico";

        private WEB.RestRequestParameters GetVarParameters(Request httpListenerRequest)
        {
            WEB.RestRequestParameters pathVariables = new WEB.RestRequestParameters();
            //Parameters = parameters;

            foreach (var key in httpListenerRequest.Query.AllKeys)
            {
                if (pathVariables[key] != null)
                {
                    throw new Exception("Parameters of same name provided in request");
                }
                pathVariables[key] = httpListenerRequest.Query[key];
            }

            return pathVariables;
        }
    }
}
