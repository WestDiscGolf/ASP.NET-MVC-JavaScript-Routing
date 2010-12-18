namespace JsRouting.Core
{
    /// <summary>
    /// Controller action interceptor
    /// </summary>
    public interface IControllerActionInterceptor
    {
        /// <summary>
        /// Controller action interceptor
        /// </summary>
        /// <param name="definition">Controller action definition</param>
        /// <returns>Value indicating whether to accept the controller action</returns>
        bool Intercept(ControllerActionDefinition definition);
    }
}