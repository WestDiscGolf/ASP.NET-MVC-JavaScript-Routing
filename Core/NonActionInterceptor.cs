using System;
using System.Linq;
using System.Web.Mvc;

namespace JsRouting.Core
{
    /// <summary>
    /// Interceptor that rejects methods with non-action attribute
    /// </summary>
    public class NonActionInterceptor : IControllerActionInterceptor
    {
        /// <summary>
        /// Non-action attribute type
        /// </summary>
        private static readonly Type nonActionAttribute = typeof(NonActionAttribute);

        /// <summary>
        /// Controller action interceptor that removes method with non-action attribute
        /// </summary>
        /// <param name="definition">Controller action definition</param>
        /// <returns>
        /// Value indicating whether to accept the controller action
        /// </returns>
        public bool Intercept(ControllerActionDefinition definition)
        {
            return !definition.Method.GetCustomAttributes(nonActionAttribute, true).Any();
        }
    }
}