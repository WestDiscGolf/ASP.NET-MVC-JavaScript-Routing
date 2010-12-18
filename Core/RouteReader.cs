using System;
using System.Collections.Generic;
using System.Linq;

namespace JsRouting.Core
{
    /// <summary>
    /// Route source that outputs the acceptable route definitions
    /// </summary>
    public class RouteReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteReader"/> class.
        /// </summary>
        /// <param name="routeSources">The route sources.</param>
        /// <param name="interceptors">Route interceptors</param>
        public RouteReader(IEnumerable<IRouteSource> routeSources, IEnumerable<IRouteInterceptor> interceptors)
        {
            if (routeSources == null) throw new ArgumentNullException("routeSources");
            if (interceptors == null) throw new ArgumentNullException("interceptors");

            this.Definitions = from source in routeSources
                               from route in source.Routes
                               let def = new RouteDefinition(route.Item2) { Name = route.Item1 }
                               where interceptors.AsParallel().All(interceptor => interceptor.Intercept(def))
                               select def;
        }

        /// <summary>
        /// Gets the route definitions from the route sources
        /// </summary>
        public IEnumerable<RouteDefinition> Definitions { get; private set; }
    }
}