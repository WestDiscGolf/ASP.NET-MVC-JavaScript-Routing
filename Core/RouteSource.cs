using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace JsRouting.Core
{
    /// <summary>
    /// Route source
    /// </summary>
    public abstract class RouteSource : IRouteSource
    {
        /// <summary>
        /// Gets the routes used in the generation of JavaScript routing definitions
        /// </summary>
        public IList<Tuple<string, Route>> Routes
        {
            get 
            {
                var list = new List<Tuple<string, Route>>();
                this.Map(list);
                return list;
            }
        }

        /// <summary>
        /// Maps the routes
        /// </summary>
        /// <param name="routes">Route list</param>
        protected abstract void Map(IList<Tuple<string, Route>> routes);
    }
}