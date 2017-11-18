namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.ShoppingCartItem")]
    public partial class ShoppingCartItem
    {
        public int ShoppingCartItemID { get; set; }

        [Required]
        [StringLength(50)]
        public string ShoppingCartID { get; set; }

        public int Quantity { get; set; }

        public int ProductID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
