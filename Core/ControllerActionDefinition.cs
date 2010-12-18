using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsRouting.Core
{
    /// <summary>
    /// Controller action definition
    /// </summary>
    public class ControllerActionDefinition : IJavaScriptSource
    {
        /// <summary>
        /// JavaScript registration
        /// </summary>
        private static readonly string jsPattern = @"$.routeManager.mapControllerAction('{0}', '{1}', {2});";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionDefinition"/> class.
        /// </summary>
        /// <param name="method">The method</param>
        public ControllerActionDefinition(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");
            this.Method = method;
            this.ActionName = method.Name;
            this.ControllerName = method.DeclaringType.Name.Replace("Controller", string.Empty);
            this.Parameters = new LinkedList<ParameterResult>();
        }

        /// <summary>
        /// Gets the name of the controller action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets the name of the controller for the action.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets the method for the action.
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Gets a list of parameters
        /// </summary>
        public LinkedList<ParameterResult> Parameters { get; private set; }

        /// <summary>
        /// Gets or sets the method description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the JavaScritpt registration for the controller action
        /// </summary>
        /// <returns>Controller action JavaScript</returns>
        public string ToJavaScript()
        {
            var builder = new StringBuilder().AppendFormat("function({0}additionalData)", this.Parameters.Select(p => p.Name).ConcatAll(", ", true))
                                             .Append("{\r\n")
                                             .AppendFormat("/// <summary>{0}</summary>", this.Description.Trim().Replace("\n", "\n/// "));

            var defaults = this.Parameters.Where(p => !string.IsNullOrEmpty(p.DefaultValue) && !p.DefaultValue.Equals("null", StringComparison.OrdinalIgnoreCase))
                                          .ToDictionary(p => p.Name, p => p.DefaultValue);

            defaults["controller"] = "\"" + this.ControllerName + "\"";
            defaults["action"] = "\"" + this.ActionName + "\"";

            // add the parameter comments for IntelliSense
            this.Parameters.Aggregate(builder, AppendComment);

            // create the call to the action method
            builder.Append("\r\n return $.routeManager.action($.extend({");
            defaults.Select(pair => string.Format("{0}:{1}", pair.Key, pair.Value))
                    .ConcatAll(", ")
                    .AppendTo(builder)
                    .Append("}, additionalData, {");
            
            this.Parameters
                .Select(param => string.Format("{0}:{0}", param.Name))
                .ConcatAll(", ")
                .AppendTo(builder)
                .Append("})); }");

            return string.Format(jsPattern, this.ControllerName, this.ActionName, builder.ToString());
        }

        /// <summary>
        /// Appends a parameter comment to the string builder
        /// </summary>
        /// <param name="sb">String Builder</param>
        /// <param name="param">Parameter to append</param>
        /// <returns>String builder with appended comment</returns>
        private static StringBuilder AppendComment(StringBuilder sb, ParameterResult param)
        {
            return sb.AppendFormat("\r\n/// <param name=\"{0}\" mayBeNull=\"{1}\" optional=\"{1}\" type=\"{2}\">{3}</param>",
                                   param.Name,
                                   param.Optional.ToString().ToLowerInvariant(),
                                   param.ParameterInfo.ParameterType.Name,
                                   param.Description.Trim().Replace("\n", "\n/// "));
        }
    }
}