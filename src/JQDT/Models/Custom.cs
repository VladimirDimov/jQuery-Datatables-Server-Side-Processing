namespace JQDT.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for custom data
    /// </summary>
    internal class Custom
    {
        /// <summary>
        /// Gets or sets the filters.
        /// Use data model property name for the key.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        internal Dictionary<string, IEnumerable<FilterModel>> Filters { get; set; }
    }
}