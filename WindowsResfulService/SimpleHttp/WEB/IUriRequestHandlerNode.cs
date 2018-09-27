using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttp.WEB
{
    public delegate void RestRequestHandler(Request request, Response response);

    internal interface IUriRequestHandlerNode
    {

        Action<Request, Response> HttpGetRequestHandler
        {
            get;
            set;
        }

        Action<Request, Response> HttpPostRequestHandler
        {
            get;
            set;
        }



        bool MatchesUriPattern(RestDigestibleUri uri);

        Action<Request, Response> GetRestRequestHandler(RestDigestibleUri uri, RestMethod method, RestRequestParameters parameters);

        void AddRestRequestHandler(RestDigestibleUri uri, RestMethod method, Action<Request, Response> handler);
    }
}
