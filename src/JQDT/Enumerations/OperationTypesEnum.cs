namespace JQDT.Enumerations
{
    using System;

    /// <summary>
    /// Operation types
    /// </summary>
    [Flags]
    internal enum OperationTypesEnum
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,

        /// <summary>
        /// A search by string
        /// </summary>
        Search = 1,

        /// <summary>
        /// Range operations including: greater than, greater than or equal, less than and less than or equal
        /// </summary>
        Range = 2,

        /// <summary>
        /// Equals operation
        /// </summary>
        Equals = 4,
    }
}