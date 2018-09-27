using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHttp.WEB
{
    internal class RestRequestTree
    {
        public RestRequestTree()
        {
            m_handlerNodes = new List<IUriRequestHandlerNode>();
        }

        public Action<Request, Response> GetRequestHandler(string url, RestMethod method, out RestRequestParameters parameters)
        {
            var digestibleUrl = new RestDigestibleUri(url);

            parameters = new RestRequestParameters();

            foreach (var node in m_handlerNodes)
            {
                if (node.MatchesUriPattern(digestibleUrl))
                {
                    return node.GetRestRequestHandler(digestibleUrl, method, parameters);
                }
            }

            return null;
        }

        public void AddRequestHandler(string url, RestMethod method, Action<Request, Response> handler)
        {
            var digestibleUrl = new RestDigestibleUri(url);

            foreach (var node in m_handlerNodes)
            {
                if (node.MatchesUriPattern(digestibleUrl))
                {
                    node.AddRestRequestHandler(digestibleUrl, method, handler);
                    return;
                }
            }

            var newNode = new NamedUriRequestHandlerNode(digestibleUrl, method, handler);

            m_handlerNodes.Add(newNode);

        }


        private ICollection<IUriRequestHandlerNode> m_handlerNodes;
    }
}