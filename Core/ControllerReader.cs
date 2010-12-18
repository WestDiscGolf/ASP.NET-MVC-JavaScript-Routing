using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using System.Reflection;
using System.Web.Script.Serialization;

namespace JsRouting.Core
{
    /// <summary>
    /// Controller action reader
    /// </summary>
    public class ControllerReader 
    {
        /// <summary>
        /// Action result type
        /// </summary>
        private static readonly Type actionResult = typeof(ActionResult);

        /// <summary>
        /// Nullable generic type
        /// </summary>
        private static readonly Type nullableType = typeof(Nullable<>);

        /// <summary>
        /// Documentation readers
        /// </summary>
        private readonly IDictionary<Assembly, DocumentationReader> docReaders = new Dictionary<Assembly, DocumentationReader>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerReader"/> class.
        /// </summary>
        /// <param name="container">The dependency container.</param>
        /// <param name="interceptors">Controller action interceptors</param>
        public ControllerReader(IContainer container, IEnumerable<IControllerActionInterceptor> interceptors)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (interceptors == null) throw new ArgumentNullException("interceptors");

            var paramInterceptor = new ParameterInterceptor(this.FindReader);
            var descritionInterceptor = new SummaryInterceptor(this.FindReader);

            this.Definitions = from inst in container.Model.InstancesOf<ControllerBase>()
                               from action in inst.ConcreteType.GetMethods()
                               where action.IsPublic
                               where actionResult.IsAssignableFrom(action.ReturnType)
                               let def = new ControllerActionDefinition(action)
                               where descritionInterceptor.Intercept(def) && paramInterceptor.Intercept(def) && 
                                     interceptors.AsParallel().All(interceptor => interceptor.Intercept(def))
                               select def;
        }

        /// <summary>
        /// Gets the controller actions
        /// </summary>
        public IEnumerable<ControllerActionDefinition> Definitions { get; private set; }

        /// <summary>
        /// Finds a documentation reader for the assembly
        /// </summary>
        /// <param name="assembly">Assembly to find documentation</param>
        /// <returns>Documentation reader</returns>
        private DocumentationReader FindReader(Assembly assembly)
        {
            if (!docReaders.ContainsKey(assembly))
                docReaders[assembly] = new DocumentationReader(assembly);

            return docReaders[assembly];
        }

        /// <summary>
        /// Interceptor that adds a description
        /// </summary>
        private class SummaryInterceptor : IControllerActionInterceptor
        {
            /// <summary>
            /// Function that finds a reader
            /// </summary>
            private readonly Func<Assembly, DocumentationReader> findReader;

            /// <summary>
            /// Initializes a new instance of the <see cref="SummaryInterceptor"/> class.
            /// </summary>
            /// <param name="findReader">The find reader function</param>
            public SummaryInterceptor(Func<Assembly, DocumentationReader> findReader)
            {
                this.findReader = findReader;
            }

            /// <summary>
            /// Controller action interceptor
            /// </summary>
            /// <param name="definition">Controller action definition</param>
            /// <returns>
            /// Value indicating whether to accept the controller action
            /// </returns>
            public bool Intercept(ControllerActionDefinition definition)
            {
                definition.Description = this.findReader(definition.Method.DeclaringType.Assembly).MethodDescription(definition.Method);
                return true;
            }
        }

        /// <summary>
        /// Interceptor that adds parameter info
        /// </summary>
        private class ParameterInterceptor : IControllerActionInterceptor
        {
            /// <summary>
            /// Function to find documentation reader
            /// </summary>
            private readonly Func<Assembly, DocumentationReader> findReader;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterInterceptor"/> class.
            /// </summary>
            /// <param name="docReaders">The doc readers.</param>
            public ParameterInterceptor(Func<Assembly, DocumentationReader> findReader)
            {
                this.findReader = findReader;
            }

            /// <summary>
            /// JavaScript serializer
            /// </summary>
            private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

            /// <summary>
            /// Controller action interceptor
            /// </summary>
            /// <param name="definition">Controller action definition</param>
            /// <returns>
            /// Value indicating whether to accept the controller action
            /// </returns>
            public bool Intercept(ControllerActionDefinition definition)
            {

                var parameters = definition.Method
                                           .GetParameters()
                                           .Select(p => new ParameterResult
                                           {
                                               Name = p.Name,
                                               Optional = p.IsOptional || (p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition().Equals(nullableType)),
                                               DefaultValue = serializer.Serialize(p.DefaultValue ?? string.Empty),
                                               Description = this.findReader(definition.Method.DeclaringType.Assembly).ParameterDescription(definition.Method, p.Name),
                                               ParameterInfo = p
                                           });

                foreach (var param in parameters)
                    definition.Parameters.AddLast(param);

                return true;
            }
        }
    }
}