using System;
using System.Collections.Generic;
using System.Web.Routing;
using JsRouting.Core;
using Test.Helpers;
using Xunit;

namespace Test
{
    public class RouteSourceExtTests
    {
        [Fact]
        public void ToRouteCollection_ThrowsException_GivenNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => RouteSourceExt.ToRouteCollection(null));
        }

        [Fact]
        public void ToRouteCollection_ReturnsCollection_WithNamedRouteFromDictionary()
        {
            const string key = "route";
            var route = new Route("url", new BlankRouteHandler());

            var source = new RouteSource();
            source.Routes.Add(Tuple.Create(key, route));

            var collection = source.ToRouteCollection();
            Assert.Same(route, collection[key]);
        }

        private class RouteSource : IRouteSource
        {
            private readonly IList<Tuple<string, Route>> routes = new List<Tuple<string, Route>>();
            public IList<Tuple<string, Route>> Routes
            {
                get { return routes; }
            }
        }
   } 
}
