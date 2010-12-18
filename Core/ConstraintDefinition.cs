using System;
using System.Collections.Generic;

namespace JsRouting.Core
{
    /// <summary>
    /// JavaScript constraint definition
    /// </summary>
    public class RouteConstraintDefinition : IJavaScriptSource
    {
        /// <summary>
        /// Constraint JavaScript representation format
        /// </summary>
        private static readonly string ConstraintJs = "$.routeManager.constraintTypeDefs.{0}('{1}', {2})";

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteConstraintDefinition"/> class.
        /// </summary>
        /// <param name="parameterName">Constraining Parameter</param>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="data">JavaScript representation of the data passed to the constraint</param>
        public RouteConstraintDefinition(string parameterName, string constraintName, string data)
        {
            if (data == null) throw new ArgumentNullException("data");

            this.Data = data;
            this.ConstraintName = constraintName;
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets or sets the data passed to the constraint
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Gets or sets the name of the constraint
        /// </summary>
        public string ConstraintName { get; private set; }

        /// <summary>
        /// Gets or sets the parameter name for the constraint
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Creates the initialization JavaScript for the constraint
        /// </summary>
        /// <returns>Constraint JavaScript</returns>
        public string ToJavaScript()
        {
            return string.Format(ConstraintJs, this.ConstraintName, this.ParameterName, this.Data);
        }
    }
}