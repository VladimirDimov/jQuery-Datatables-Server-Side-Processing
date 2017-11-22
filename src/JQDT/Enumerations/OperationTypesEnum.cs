namespace JQDT.Enumerations
{
    using System;

    [Flags]
    internal enum OperationTypesEnum
    {
        Default = 0,

        /// <summary>
        /// A search by string
        /// </summary>
        Search = 1,

        /// <summary>
        /// Range operations including: =, <, >, <=, >=
        /// </summary>
        Range = 2,
    }
}