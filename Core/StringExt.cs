using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsRouting.Core
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// Concats all strings into a single string
        /// </summary>
        /// <param name="strings">Strings to concat together</param>
        /// <param name="deliminator">Deliminator between the string values</param>
        /// <param name="delimEnd">Flag to indicate if the deliminator should be placed at the end</param>
        /// <returns>Composed string</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="strings"/> is null</exception>
        public static string ConcatAll(this IEnumerable<string> strings, string deliminator = "", bool delimEnd = false)
        {
            if (strings == null) throw new ArgumentNullException("strings");

            deliminator = deliminator ?? string.Empty;
            strings = strings.Where(str => !string.IsNullOrEmpty(str));

            // check for empty strings
            if (!strings.Any())
                return string.Empty;

            if (delimEnd) // force the deliminator to be at the end
                return strings.Aggregate(new StringBuilder(), (builder, value) => builder.Append(value).Append(deliminator)).ToString();

            return strings.Skip(1)
                          .Aggregate(new StringBuilder(strings.First()), (builder, value) => builder.Append(deliminator).Append(value))
                          .ToString();
        }

        /// <summary>
        /// Appends the string to the StringBuilder
        /// </summary>
        /// <param name="str">String to append</param>
        /// <param name="builder">Builder to append to</param>
        /// <returns>Original string builder with string appended</returns>
        public static StringBuilder AppendTo(this string str, StringBuilder builder)
        {
            return builder.Append(str);
        }
    }
}