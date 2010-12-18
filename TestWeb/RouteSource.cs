using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using JsRouting.Core;

namespace TestWeb
{
    public class WebRoutes : RouteSource
    {
        protected override void Map(IList<Tuple<string, Route>> routes)
        {
            routes.MapRoute("Default", 
                            "{controller}/{action}/{id}", 
                            new { controller = "Home", action = "Index", id = "" });
        }
    }

    [JsConstraint("notEmpty")]
    public class NotEmpty : IRouteConstraint, IJavaScriptAddition
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values[parameterName] != null && !string.IsNullOrEmpty(values[parameterName].ToString());
        }

        public string ToJavaScript()
        {
            return @"$.routeManager.constraintTypeDefs.notEmpty = function(param){
                return function(data){
                    return data[param] && data[param].length > 0;
                };
            };";
        }
    }
}