namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.ProductReview")]
    public partial class ProductReview
    {
        public int ProductReviewID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string ReviewerName { get; set; }

        public DateTime ReviewDate { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        public int Rating { get; set; }

        [StringLength(3850)]
        public string Comments { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
