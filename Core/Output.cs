using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace JsRouting.Core
{
    /// <summary>
    /// Object representing the output formatter
    /// </summary>
    public class Output : IJavaScriptSource
    {
        /// <summary>
        /// Beginning line for output
        /// </summary>
        private static readonly string beginLine = Environment.NewLine + "(function($, undefined){";

        /// <summary>
        /// Ending line for output
        /// </summary>
        private static readonly string endLine = "})(jQuery);";

        /// <summary>
        /// Route reader
        /// </summary>
        private readonly RouteReader routeReader;

        /// <summary>
        /// Controller action reader
        /// </summary>
        private readonly ControllerReader controllerReader;

        /// <summary>
        /// Additional JavaScript values
        /// </summary>
        private readonly IEnumerable<IJavaScriptAddition> additions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Output"/> class.
        /// </summary>
        /// <param name="routeReader">The route reader.</param>
        /// <param name="controllerReader">Controller action reader.</param>
        public Output(RouteReader routeReader, ControllerReader controllerReader, IEnumerable<IJavaScriptAddition> additions)
        {
            if (routeReader == null) throw new ArgumentNullException("reader");
            this.routeReader = routeReader;
            this.controllerReader = controllerReader;
            this.additions = additions ?? Enumerable.Empty<IJavaScriptAddition>();
        }

        /// <summary>
        /// Gets the output for the manager JS embedded resource
        /// </summary>
        public static string ManagerJs
        {
            get
            {
                var stream = typeof(Output).Assembly.GetManifestResourceStream("JsRouting.Core.JavaScript.RouteManager.js");
                var streamReader = new StreamReader(stream);
                var value = streamReader.ReadToEnd();
                streamReader.Dispose();
                return value;
            }
        }

        /// <summary>
        /// Gets the JavaScript output 
        /// </summary>
        /// <returns>JavaScript output for route generation</returns>
        public string ToJavaScript()
        {
            var stringBuilder = new StringBuilder().AppendLine(beginLine);
            
            // add additional values
            this.additions.Aggregate(stringBuilder, (builder, value) => builder.Append("\r\n").Append(value.ToJavaScript())).AppendLine();

            // add values from reader
            this.routeReader
                .Definitions
                .OfType<IJavaScriptSource>()
                .Concat(this.controllerReader.Definitions)
                .Aggregate(stringBuilder, (builder, value) => builder.AppendLine(value.ToJavaScript()));

            // add ending line
            stringBuilder.AppendLine(endLine);

            return stringBuilder.ToString();
        }
    }
}