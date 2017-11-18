namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.ProductModelIllustration")]
    public partial class ProductModelIllustration
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductModelID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IllustrationID { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Illustration Illustration { get; set; }

        public virtual ProductModel ProductModel { get; set; }
    }
}
