using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace JsRouting.Core
{
    /// <summary>
    /// Extension methods for route dictionary
    /// </summary>
    public static class DictionaryExt
    {
        /// <summary>
        /// Maps a route to the dictionary
        /// </summary>
        /// <param name="routeDictionary">Route dictionary to map route</param>
        /// <param name="name">Route name</param>
        /// <param name="pattern">URL pattern</param>
        /// <param name="defaults">Default values</param>
        /// <param name="constraints">Constraints on the route</param>
        /// <returns>Route formed by the call to MapRoute</returns>
        public static Route MapRoute(this IList<Tuple<string, Route>> routeDictionary, string name, string pattern, object defaults = null, object constraints = null)
        {
            var route = new Route(pattern, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler());
            routeDictionary.Add(Tuple.Create(name, route));
            return route;
        }

        /// <summary>
        /// Adds the routes from a route dictionary source to the route collection
        /// </summary>
        /// <param name="routeDictionary">Route dictionary source</param>
        /// <param name="collection">Route collection for routes</param>
        /// <returns>Route collection with the routes added</returns>
        public static RouteCollection AddTo(this IList<Tuple<string, Route>> routeDictionary, RouteCollection collection)
        {
            foreach (var pair in routeDictionary)
                collection.Add(pair.Item1, pair.Item2);
            return collection;
        }
    }
}