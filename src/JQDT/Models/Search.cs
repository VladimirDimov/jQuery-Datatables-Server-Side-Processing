namespace JQDT.Models
{
    /// <summary>
    /// Search default model
    /// </summary>
    internal class Search
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        internal string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Search"/> is regex.
        /// </summary>
        /// <value>
        ///   <c>true</c> if regex; otherwise, <c>false</c>.
        /// </value>
        internal bool Regex { get; set; }
    }
}