namespace JQDT.Models
{
    using System;

    /// <summary>
    /// Request helper information model
    /// </summary>
    public class RequestHelpers
    {
        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; set; }
        public Type DataCollectionType { get; internal set; }
    }
}