using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using StructureMap;

namespace JsRouting.Core
{
    /// <summary>
    /// Assembly loader
    /// </summary>
    public static class AssemblyLoader
    {
        /// <summary>
        /// Gets a container with the loaded dependencies
        /// </summary>
        /// <param name="assemblies">Assemblies to scan</param>
        /// <returns>Container containing various dependencies to form the JavaScript routing definitions</returns>
        public static IContainer Load(IEnumerable<string> assemblies)
        {
            return new Container(config =>
            {
                config.Scan(a =>
                {
                    // add the main JsRouting assembly to the scanner
                    a.Assembly(typeof(Output).Assembly);

                    // add the assemblies to scan
                    foreach (string assembly in assemblies)
                        a.Assembly(Assembly.LoadFrom(assembly));

                    // types to scan..
                    a.AddAllTypesOf<IRouteSource>();
                    a.AddAllTypesOf<IRouteInterceptor>();
                    a.AddAllTypesOf<ControllerBase>();
                    a.AddAllTypesOf<IControllerActionInterceptor>();
                    a.AddAllTypesOf<IJavaScriptAddition>();
                });
            });
        }
    }
}