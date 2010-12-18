using System;
using System.Web.Routing;

namespace Test.Helpers
{
    public class BlankRouteHandler : IRouteHandler
    {
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
}