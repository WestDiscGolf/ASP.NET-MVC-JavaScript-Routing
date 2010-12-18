using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace JsRouting.Core
{
    /// <summary>
    /// Route source
    /// </summary>
    public interface IRouteSource
    {
        /// <summary>
        /// Gets the routes used in the generation of JavaScript routing definitions
        /// </summary>
        IList<Tuple<string, Route>> Routes { get; }
    }
}