namespace JQDT.Models
{
    /// <summary>
    /// Request Info Model
    /// </summary>
    public class RequestInfoModel
    {
        /// <summary>
        /// Gets or sets the table parameters.
        /// </summary>
        /// <value>
        /// The table parameters.
        /// </value>
        public DataTableAjaxPostModel TableParameters { get; set; }

        /// <summary>
        /// Gets or sets the helpers.
        /// </summary>
        /// <value>
        /// The helpers.
        /// </value>
        public RequestHelpers Helpers { get; set; }
    }
}