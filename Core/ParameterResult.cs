using System.Reflection;

namespace JsRouting.Core
{
    /// <summary>
    /// Representation of the parameter
    /// </summary>
    public class ParameterResult
    {
        /// <summary>
        /// Gets or sets the parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is optional
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Gets or sets the default value JavaScript
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the parameter description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameter metadata
        /// </summary>
        public ParameterInfo ParameterInfo { get; set; }
    }
}