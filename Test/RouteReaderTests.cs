using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using JsRouting.Core;
using Xunit;

namespace Test
{
    public class RouteReaderTests
    {
        [Fact]
        public void Ctr_ThrowsException_GivenNullSources()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteReader(null, Enumerable.Empty<IRouteInterceptor>()));
        }

        [Fact]
        public void Ctr_ThrowsException_GivenNullInterceptors()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteReader(Enumerable.Empty<IRouteSource>(), null));
        }

        [Fact]
        public void Ctr_AddsRoute_WhenInterceptorReturnsTrue()
        {
            const string pattern = "{controller}/{action}";
            var source = new RouteSource();
            source.Routes.Add(Tuple.Create("foo", new Route(pattern, new BlankRouteHandler())));

            var reader = new RouteReader(new[] { source }, new[] { new BlankInterceptor { ReturnValue = true } });
            Assert.Equal(pattern, reader.Definitions.First().UrlPattern);
        }

        [Fact]
        public void Ctr_DoesNotAddRoute_WhenInterceptorReturnsFalse()
        { 
            var source = new RouteSource();
            source.Routes.Add(Tuple.Create("foo", new Route("", new BlankRouteHandler())));

            var reader = new RouteReader(new[] { source }, new[] { new BlankInterceptor { ReturnValue = false } });
            Assert.Empty(reader.Definitions);
        }

        [Fact]
        public void Ctr_CallsAllInterceptorsToDetermineAcceptance()
        {
            var source = new RouteSource();
            source.Routes.Add(Tuple.Create("foo", new Route("", new BlankRouteHandler())));

            var reader = new RouteReader(new[] { source }, new[] { new BlankInterceptor { ReturnValue = true }, new BlankInterceptor { ReturnValue = false }, new BlankInterceptor { ReturnValue = true } });
            Assert.Empty(reader.Definitions);
        }

        [Fact]
        public void Ctr_AddsRouteNameToRoute()
        {
            const string name = "name";
            var source = new RouteSource();
            source.Routes.Add(Tuple.Create(name, new Route("", new BlankRouteHandler())));

            var reader = new RouteReader(new[] { source }, new IRouteInterceptor[0]);
            Assert.Same(name, reader.Definitions.First().Name);
        }

        [Fact]
        public void Ctr_AddsRoutes_FromAllSources()
        {
            var source1 = new RouteSource();
            source1.Routes.Add(Tuple.Create("foo", new Route("", new BlankRouteHandler())));

            var source2 = new RouteSource();
            source1.Routes.Add(Tuple.Create("bar", new Route("{controller}", new BlankRouteHandler())));

            var source3 = new RouteSource();
            source1.Routes.Add(Tuple.Create("baz", new Route("url", new BlankRouteHandler())));
            source1.Routes.Add(Tuple.Create("baz2", new Route("url2", new BlankRouteHandler())));

            var reader = new RouteReader(new[] { source1, source2, source3 }, new IRouteInterceptor[0] );
            Assert.Equal(4, reader.Definitions.Count());
        }

        public class RouteSource : IRouteSource
        {
            private IList<Tuple<string, Route>> routes = new List<Tuple<string, Route>>();
            public IList<Tuple<string, Route>> Routes
            {
                get { return routes; }
            }
        }
        
        private class BlankInterceptor : IRouteInterceptor
        {
            public bool ReturnValue { get; set; }

            public bool Intercept(RouteDefinition definition)
            {
                return ReturnValue;
            }
        }

        private class BlankRouteHandler : IRouteHandler
        {
            public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
