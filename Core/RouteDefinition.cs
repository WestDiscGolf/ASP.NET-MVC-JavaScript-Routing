using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Text;
using System.Linq;

namespace JsRouting.Core
{
    /// <summary>
    /// JavaScript representation of the route
    /// </summary>
    public class RouteDefinition : IJavaScriptSource
    {
        /// <summary>
        /// Route registration pattern
        /// </summary>
        private static readonly string mapRoute = "$.routeManager.mapRoute('{0}', '{1}', {2})";

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteDefinition"/> class.
        /// </summary>
        /// <param name="route">The route.</param>
        public RouteDefinition(Route route)
        {
            if (route == null)
                throw new ArgumentNullException("route");

            this.Route = route;
            this.UrlPattern = route.Url.Replace("{*", "{");
            this.Constraints = new List<RouteConstraintDefinition>();
            this.DefaultValues = route.Defaults == null ? new Dictionary<string, object>() : new Dictionary<string,object>(route.Defaults);
        }

        /// <summary>
        /// Gets or sets the route used to define the route definition
        /// </summary>
        public Route Route { get; private set; }

        /// <summary>
        /// Gets or sets the route name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pattern
        /// </summary>
        public string UrlPattern { get; set; }

        /// <summary>
        /// Gets a list of the constraints for the route
        /// </summary>
        public IList<RouteConstraintDefinition> Constraints { get; private set; }

        /// <summary>
        /// Gets a dictionary of default route values
        /// </summary>
        public IDictionary<string, object> DefaultValues { get; private set; }

        /// <summary>
        /// Gets the JavaScript utilized to define the route
        /// </summary>
        /// <returns>JavaScript used to to define the route</returns>
        public override string ToString()
        {
            var defaltValues = new JavaScriptSerializer().Serialize(this.DefaultValues);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(mapRoute, this.UrlPattern, this.Name, defaltValues);

            this.Constraints
                .Select(constraint => constraint.ToJavaScript())
                .Aggregate(stringBuilder, (builder, constraintJs) => builder.Append(".addConstraint(").Append(constraintJs).Append(")"));

            // add final semicolon
            stringBuilder.Append(";");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the JavaScript utilized to define the route
        /// </summary>
        /// <returns>JavaScript used to to define the route</returns>
        public string ToJavaScript()
        {
            return this.ToString();
        }
    }
}