using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttp.WEB
{
    internal class WildCardUriRequestHandlerNode : UriRequestHandlerNode
    {
        public WildCardUriRequestHandlerNode(RestDigestibleUri uri, RestMethod method, Action<Request, Response> handler)
        {
            AddRestRequestHandler(uri, method, handler);
        }

        public override bool MatchesUriPattern(RestDigestibleUri uri)
        {
            return true;

        }

        protected override int GetSearchPriority()
        {
            return 2;
        }

        protected override void HandleParameters(RestDigestibleUri uri, RestRequestParameters parameters)
        { }

        private const string ASTRISK = "*";
    }
}
