namespace JQDT.Exceptions
{
    using System;
    using JQDT.Enumerations;

    /// <summary>
    /// Thrown when try to perform operation on a type that is not valid for it.
    /// </summary>
    /// <seealso cref="JQDT.Exceptions.JQDataTablesException" />
    [Serializable]
    internal class InvalidTypeForOperationException : JQDataTablesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTypeForOperationException"/> class.
        /// </summary>
        public InvalidTypeForOperationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTypeForOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidTypeForOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTypeForOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidTypeForOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTypeForOperationException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="operation">The operation.</param>
        public InvalidTypeForOperationException(Type type, OperationTypesEnum operation)
            : this($"Invalid type {type.FullName} for {operation.ToString()} operation.")
        {
        }
    }
}