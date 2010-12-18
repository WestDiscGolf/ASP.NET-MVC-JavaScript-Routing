using System;
using System.Web.Routing;
using JsRouting.Core;
using Test.Helpers;
using Xunit;

namespace Test
{
    public class RouteDefinitionTests
    {
        private const string pattern = "{controller}/{action}";
        private const string defaultController = "Foo";
        private const string routeName = "myroute";
        const string expectedPattern = @"$.routeManager.mapRoute('{controller}/{action}', 'myroute', {""controller"":""Foo""});";

        [Fact]
        public void Ctr_SetsRouteProperty()
        {
            var route = TestRoute;
            var definition = new RouteDefinition(route);
            Assert.Same(route, definition.Route);
        }

        [Fact]
        public void Ctr_ThrowsException_GivenNullRoute()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteDefinition(null));
        }

        [Fact]
        public void Ctr_SetsUrlPatternProperty()
        {
            var route = TestRoute;
            var definition = new RouteDefinition(route);
            Assert.Same(route.Url, definition.UrlPattern);
        }

        [Fact]
        public void Ctr_SetsConstraintsPropertyToEmptyList()
        {
            Assert.Empty(new RouteDefinition(TestRoute).Constraints);
        }

        [Fact]
        public void Ctr_SetsDefaultValues_ToTheDefaultValuesFromRoute()
        {
            var route = TestRoute;
            Assert.Equal(route.Defaults["controller"], new RouteDefinition(route).DefaultValues["controller"]);
        }

        [Fact]
        public void Ctr_SetsDefaultValues_ToACopyOfRouteDefaults()
        {
            var route = TestRoute;
            Assert.NotSame(route.Defaults, new RouteDefinition(route).DefaultValues);
        }

        [Fact]
        public void ToJavaScript_ReturnsRouteManagerRouteDefinitionPattern()
        {
            var definition = new RouteDefinition(TestRoute) { Name = routeName };
            Assert.Equal(expectedPattern, definition.ToJavaScript());   
        }

        [Fact]
        public void Ctr_SetsRouteName_WithoutWildcard()
        {
            var route = new Route("{controller}/{*rest}", new BlankRouteHandler());
            Assert.Equal("{controller}/{rest}", new RouteDefinition(route).UrlPattern);
        }

        private static Route TestRoute
        {
            get
            {
                return new Route(pattern, new BlankRouteHandler())
                {
                    Defaults = new RouteValueDictionary
                    {
                        { "controller", defaultController }
                    }
                };
            }
        }
    }
}