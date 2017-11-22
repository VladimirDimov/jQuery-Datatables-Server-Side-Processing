namespace JQDT.Exceptions
{
    using System;
    using JQDT.Enumerations;

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

        public InvalidTypeForOperationException(Type type, OperationTypesEnum operation)
            : this($"Invalid type {type.FullName} for {operation.ToString()} operation.")
        {
        }
    }
}