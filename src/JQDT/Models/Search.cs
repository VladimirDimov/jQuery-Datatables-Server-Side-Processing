namespace JQDT.Models
{
    /// <summary>
    /// Search default model
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Search"/> is regex.
        /// </summary>
        /// <value>
        ///   <c>true</c> if regex; otherwise, <c>false</c>.
        /// </value>
        public bool Regex { get; set; }
    }
}