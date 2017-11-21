namespace JQDT.Exceptions
{
    using System;

    [Serializable]
    internal class JQDataTablesException : Exception
    {
        public JQDataTablesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}