namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.TransactionHistory")]
    public partial class TransactionHistory
    {
        [Key]
        public int TransactionID { get; set; }

        public int ProductID { get; set; }

        public int ReferenceOrderID { get; set; }

        public int ReferenceOrderLineID { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(1)]
        public string TransactionType { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal ActualCost { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
