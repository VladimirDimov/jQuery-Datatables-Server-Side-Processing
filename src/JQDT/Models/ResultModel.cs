namespace JQDT.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Result model that is passed to the client.
    /// </summary>
    internal class ResultModel
    {
        /// <summary>
        /// Gets or sets the Draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        internal int Draw { get; set; }

        /// <summary>
        /// Gets or sets the records total.
        /// </summary>
        /// <value>
        /// The records total.
        /// </value>
        internal int RecordsTotal { get; set; }

        /// <summary>
        /// Gets or sets the RecordsFiltered.
        /// </summary>
        /// <value>
        /// The records filtered.
        /// </value>
        internal object RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        internal List<object> Data { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        internal string Error { get; set; }
    }
}