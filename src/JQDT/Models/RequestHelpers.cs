namespace JQDT.Models
{
    using System;

    /// <summary>
    /// Request helper information model
    /// </summary>
    internal class RequestHelpers
    {
        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        internal Type ModelType { get; set; }

        /// <summary>
        /// Gets or sets the type of the data collection.
        /// </summary>
        /// <value>
        /// The type of the data collection.
        /// </value>
        internal Type DataCollectionType { get; set; }
    }
}