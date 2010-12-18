using System;
using System.IO;
using System.Linq;
using JsRouting.Core;

namespace Output
{
    public static class Program
    {
        private static readonly string[] DoNotScan = { "JsRouting.", "StructureMap.dll" };

        public static void Main(string[] args)
        {
            if (args.Length < 2)
                throw new Exception("Must specify JavaScript file as first argument and additional directories containing assemblies after for the routes to be output correctly.");

            var outputLocation = args.First().Without("\"").Trim();

            // get a stream of assemblies that could contain routes, constraints, etc
            var assemblies = from assemblyDirectory in args.Skip(1)
                             from fileLocation in Directory.EnumerateFiles(assemblyDirectory.Without("\"").Trim())
                             where fileLocation.EndsWith(".dll")
                             let fileName = Path.GetFileName(fileLocation)
                             where !DoNotScan.Any(fileName.Contains) // HACK to not scan StrucuteMap and the JsRouting DLLs
                             select fileLocation;

            var outputJs = JsRouting.Core.Output.ManagerJs + 
                           AssemblyLoader.Load(assemblies)
                                         .GetInstance<JsRouting.Core.Output>()
                                         .ToJavaScript();

            // delete file if it already exists
            if (File.Exists(outputLocation))
                File.Delete(outputLocation);

            // write to path
            File.WriteAllText(outputLocation, outputJs);
        }

        private static string Without(this string original, string substring)
        {
            return (original ?? string.Empty).Replace(substring, string.Empty);
        }
    }
}