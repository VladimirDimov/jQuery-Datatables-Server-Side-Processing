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
        public int Draw { get; internal set; }

        /// <summary>
        /// Gets or sets the records total.
        /// </summary>
        /// <value>
        /// The records total.
        /// </value>
        public int RecordsTotal { get; internal set; }

        /// <summary>
        /// Gets or sets the RecordsFiltered.
        /// </summary>
        /// <value>
        /// The records filtered.
        /// </value>
        public object RecordsFiltered { get; internal set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public List<object> Data { get; internal set; }
    }
}