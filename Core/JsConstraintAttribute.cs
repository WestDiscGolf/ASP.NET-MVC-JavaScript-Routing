using System;

namespace JsRouting.Core
{
    /// <summary>
    /// Constraint persisted through JavaScript
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class JsConstraintAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsConstraintAttribute"/> class.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        public JsConstraintAttribute(string constraintName)
        {
            this.ConstraintName = constraintName;
        }

        /// <summary>
        /// Gets the constraint JavaScript name
        /// </summary>
        public string ConstraintName { get; private set; }
    }
}