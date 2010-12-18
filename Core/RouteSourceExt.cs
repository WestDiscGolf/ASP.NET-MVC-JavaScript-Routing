using System;
using System.Web.Routing;

namespace JsRouting.Core
{
    /// <summary>
    /// Extension methods for <see cref="T:IRouteSource"/>
    /// </summary>
    public static class RouteSourceExt
    {
        /// <summary>
        /// Generates a route collection from a route source
        /// </summary>
        /// <param name="source">Route source from which to generate a route collection</param>
        /// <returns>Route collection</returns>
        public static RouteCollection ToRouteCollection(this IRouteSource source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var collection = new RouteCollection();

            foreach (var pair in source.Routes)
                collection.Add(pair.Item1, pair.Item2);

            return collection;
        }
    }
}