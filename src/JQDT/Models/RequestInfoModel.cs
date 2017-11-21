namespace JQDT.Models
{
    /// <summary>
    /// Request Info Model
    /// </summary>
    internal class RequestInfoModel
    {
        /// <summary>
        /// Gets or sets the table parameters.
        /// </summary>
        /// <value>
        /// The table parameters.
        /// </value>
        internal DataTableAjaxPostModel TableParameters { get; set; }

        /// <summary>
        /// Gets or sets the helpers.
        /// </summary>
        /// <value>
        /// The helpers.
        /// </value>
        internal RequestHelpers Helpers { get; set; }
    }
}