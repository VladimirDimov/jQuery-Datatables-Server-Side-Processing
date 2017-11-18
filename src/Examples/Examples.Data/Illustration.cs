namespace Examples.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.Illustration")]
    public partial class Illustration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Illustration()
        {
            ProductModelIllustrations = new HashSet<ProductModelIllustration>();
        }

        public int IllustrationID { get; set; }

        [Column(TypeName = "xml")]
        public string Diagram { get; set; }

        public DateTime ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductModelIllustration> ProductModelIllustrations { get; set; }
    }
}
