namespace JQDT.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Default data table ajax model containing all default parameters.
    /// </summary>
    internal class DataTableAjaxPostModel
    {
        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        internal int Draw { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        internal int Start { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        internal int Length { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        internal List<Column> Columns { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        internal Search Search { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        internal List<Order> Order { get; set; }

        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        /// <value>
        /// The custom.
        /// </value>
        internal Custom Custom { get; set; }
    }
}