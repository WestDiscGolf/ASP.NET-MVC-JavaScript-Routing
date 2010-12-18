using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace JsRouting.Core
{
    /// <summary>
    /// Documentation reader
    /// </summary>
    public class DocumentationReader
    {
        /// <summary>
        /// All the members declared in the documentation file
        /// </summary>
        private readonly IEnumerable<XElement> members;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationReader"/> class.
        /// </summary>
        /// <param name="assembly">The assembly from which to read documentation</param>
        public DocumentationReader(Assembly assembly)
        {
            var docFile = assembly.Location.Replace(".dll", ".xml");

            if (File.Exists(docFile))
                this.members = XDocument.Load(docFile).Descendants("doc").Descendants("members").FirstOrDefault().Descendants();
            else
                this.members = Enumerable.Empty<XElement>();
        }

        /// <summary>
        /// Gets the parameter description of a type
        /// </summary>
        /// <param name="method">Method value</param>
        /// <param name="parameter">Parameter name</param>
        /// <returns>Parameter description</returns>
        public string ParameterDescription(MethodInfo method, string parameter)
        {
            var elem = FindElement(method);
            if (elem != null)
                return elem.Descendants("param")
                           .Where(e => e.Attribute("name") != null && e.Attribute("name").Value.Equals(parameter))
                           .Select(e => e.Value)
                           .FirstOrDefault() ?? string.Empty;
            return string.Empty;
        }

        /// <summary>
        /// Gets the parameter description of a type
        /// </summary>
        /// <param name="method">Method value</param>
        /// <returns>Method documentation description</returns>
        public string MethodDescription(MethodInfo method)
        {
            var elem = FindElement(method);
            if (elem != null)
                return elem.Descendants("summary")
                           .Select(e => e.Value)
                           .FirstOrDefault() ?? string.Empty;
            return string.Empty;
        }

        /// <summary>
        /// Finds the comment element
        /// </summary>
        /// <param name="method">Method to find</param>
        /// <returns>Method comment XML</returns>
        private XElement FindElement(MethodInfo method)
        { 
            var stringBuilder = new StringBuilder("M:").Append(method.DeclaringType.FullName).Append(".").Append(method.Name).Append("(");
            var parameters = AddTo(stringBuilder, method.GetParameters().Select(p => p.ParameterType)).Append(")");
            return members.FirstOrDefault(elem => elem.Attribute("name") != null && elem.Attribute("name").Value.Equals(stringBuilder.ToString().Replace("()", "")));
        }

        /// <summary>
        /// Adds parameter info to the string builder
        /// </summary>
        /// <param name="builder">String builder to add to</param>
        /// <param name="paramTypes">Parameter types</param>
        /// <returns>String builder with additional items</returns>
        private static StringBuilder AddTo(StringBuilder builder, IEnumerable<Type> paramTypes)
        {
            if (!paramTypes.Any())
                return builder;
            return paramTypes.Skip(1).Aggregate(builder.Append(paramTypes.First().FullName), (b, type) => b.Append(",").Append(type.FullName));
        }
    }
}