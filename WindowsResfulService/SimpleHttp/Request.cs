using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttp
{
    public class Request
    {
        public HttpListenerRequest httpRequest;
        private string body;
        private string method;

        internal Request(HttpListenerRequest httpRequest)
        {
            this.httpRequest = httpRequest;
            method = this.httpRequest.HttpMethod;
            Query = System.Web.HttpUtility.ParseQueryString(httpRequest.Url.Query);
        }

        public WEB.RestRequestParameters Parameters {get;set;}

        // public string[] Parameters { get; private set; }

        public string Endpoint
        {
            get { return this.httpRequest.RawUrl; }
        }

        public NameValueCollection Query { get; internal set; }
        public WEB.RestRequestParameters PathVariables { get; internal set; }
        public string Method
        {
            get { return method; }
            set {  method = value; }
        }

        public async Task<string> GetBodyAsync()
        {
            //TODO: handle exceptions
            if (Method == RestMethod.GET.ToString() || !this.httpRequest.HasEntityBody)
                return null;

            if (this.body == null)
            {
                byte[] buffer = new byte[this.httpRequest.ContentLength64];
                using (Stream inputStream = this.httpRequest.InputStream)
                {
                    await inputStream.ReadAsync(buffer, 0, buffer.Length);
                }

                this.body = Encoding.UTF8.GetString(buffer);
            }

            return this.body;
        }
    }
}
