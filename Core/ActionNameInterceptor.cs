using System;
using System.Linq;
using System.Web.Mvc;

namespace JsRouting.Core
{
    /// <summary>
    /// Interceptor that replaces the name of the controller action when a ActionName attribute is applied
    /// </summary>
    public class ActionNameInterceptor : IControllerActionInterceptor
    {
        /// <summary>
        /// Action name attribute type
        /// </summary>
        private static readonly Type actionNameAttribute = typeof(ActionNameAttribute);

        /// <summary>
        /// Controller action interceptor that changes the action name when specified by attribute
        /// </summary>
        /// <param name="definition">Controller action definition</param>
        /// <returns>
        /// Value indicating whether to accept the controller action
        /// </returns>
        public bool Intercept(ControllerActionDefinition definition)
        {
            var attribute = definition.Method
                                      .GetCustomAttributes(actionNameAttribute, true)
                                      .OfType<ActionNameAttribute>()
                                      .FirstOrDefault();

            if (attribute != null)
                definition.ActionName = attribute.Name;

            return true;
        }
    }
}