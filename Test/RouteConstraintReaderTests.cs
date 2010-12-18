using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using JsRouting.Core;
using System.Web.Routing;
using Test.Helpers;
using System.Web;

namespace Test
{
    public class RouteConstraintReaderTests
    {
        private const string JavaScriptConstraintName = "jsConstraint";

        [Fact]
        public void Intercept_ReturnsTrue_WhenRouteDoesNotContainConstraints()
        {
            var def = new RouteDefinition(new Route("{controller}", new BlankRouteHandler()));
            Assert.True(new RouteConstraintReader().Intercept(def));
        }

        [Fact]
        public void Intercept_ReturnsTrue_WhenRouteContainsConstraints()
        {
            var def = new RouteDefinition(new Route("{controller}", new BlankRouteHandler()) { Constraints = new RouteValueDictionary(new { action = new EmptyConstraint() }) });
            Assert.True(new RouteConstraintReader().Intercept(def));
        }

        [Fact]
        public void Intercept_DoesNotAddConstraint_WhenConstraintDoesntHaveJavaScriptAttribute()
        {
            var def = new RouteDefinition(new Route("{controller}", new BlankRouteHandler()) { Constraints = new RouteValueDictionary(new { action = new EmptyConstraint() }) });
            new RouteConstraintReader().Intercept(def);
            Assert.Empty(def.Constraints);
        }

        [Fact]
        public void Intercept_AddsConstraint_WithCorrectName_WhenConstraintHasJavaScriptAttribute()
        {
            var def = new RouteDefinition(new Route("{controller}", new BlankRouteHandler()) { Constraints = new RouteValueDictionary(new { action = new JsConstraint() }) });
            new RouteConstraintReader().Intercept(def);
            Assert.Equal(JavaScriptConstraintName, def.Constraints.First().ConstraintName);
        }

        [Fact]
        public void Intercept_AddsConstraint_WithSerializedDataFromObject_WhenConstraintHasJavaScriptAttribute()
        {
            const string myPropVal = "foo";
            const int propertyVal = 5;
            const string expected = "{\"MyProperty\":\"foo\",\"PropertyValue\":5}";

            var def = new RouteDefinition(new Route("{controller}", new BlankRouteHandler())
            {
                Constraints = new RouteValueDictionary(new
                {
                    action = new JsConstraint
                        {
                            MyProperty = myPropVal,
                            PropertyValue = propertyVal
                        }
                })
            });

            new RouteConstraintReader().Intercept(def);
            Assert.Equal(expected, def.Constraints.First().Data);
        }

        public class EmptyConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                throw new NotImplementedException();
            }
        }

        [JsConstraint(JavaScriptConstraintName)]
        public class JsConstraint : IRouteConstraint
        {
            public string MyProperty { get; set; }

            public int PropertyValue { get; set; }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                throw new NotImplementedException();
            }
        }
    }
}