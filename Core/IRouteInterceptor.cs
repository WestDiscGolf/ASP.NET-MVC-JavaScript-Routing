namespace JsRouting.Core
{
    /// <summary>
    /// Route interceptor before the route is output to JavaScript
    /// </summary>
    public interface IRouteInterceptor
    {
        /// <summary>
        /// Intercepts the route definition before the route is output to JavaScript
        /// </summary>
        /// <param name="definition">Route definition to manipulate</param>
        /// <returns>Value indicating whether to add the route after the interception</returns>
        bool Intercept(RouteDefinition definition);
    }
}