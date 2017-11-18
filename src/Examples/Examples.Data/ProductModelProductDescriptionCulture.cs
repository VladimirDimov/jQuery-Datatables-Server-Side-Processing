namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.ProductModelProductDescriptionCulture")]
    public partial class ProductModelProductDescriptionCulture
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductModelID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductDescriptionID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(6)]
        public string CultureID { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Culture Culture { get; set; }

        public virtual ProductDescription ProductDescription { get; set; }

        public virtual ProductModel ProductModel { get; set; }
    }
}
