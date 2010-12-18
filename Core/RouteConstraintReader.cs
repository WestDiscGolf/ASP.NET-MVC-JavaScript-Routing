using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace JsRouting.Core
{
    /// <summary>
    /// Route constraint that adds a constraint to the definition if present
    /// </summary>
    public class RouteConstraintReader : IRouteInterceptor
    {
        /// <summary>
        /// Route constraint type
        /// </summary>
        private static readonly Type constraintType = typeof(IRouteConstraint);

        /// <summary>
        /// JavaScript attribute type
        /// </summary>
        private static readonly Type attributeType = typeof(JsConstraintAttribute);

        /// <summary>
        /// JavaScript serializer
        /// </summary>
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// Intercepts the route definition before the route is output to JavaScript
        /// </summary>
        /// <param name="definition">Route definition to manipulate</param>
        /// <returns>
        /// Value indicating whether to add the route after the interception
        /// </returns>
        public bool Intercept(RouteDefinition definition)
        {
            var constraints = from constraint in (definition.Route.Constraints ?? new RouteValueDictionary())
                              where constraint.Value != null
                              where constraint.Value.GetType().GetInterfaces().Contains(constraintType)
                              let attr = constraint.Value.GetType().GetCustomAttributes(attributeType, false).OfType<JsConstraintAttribute>().FirstOrDefault()
                              where attr != null
                              select new RouteConstraintDefinition(constraint.Key, attr.ConstraintName, serializer.Serialize(constraint.Value));

            foreach (var constraintDefinition in constraints)
                definition.Constraints.Add(constraintDefinition);

            return true;
        }
    }
}