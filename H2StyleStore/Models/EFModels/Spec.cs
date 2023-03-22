namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Spec")]
    public partial class Spec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Spec()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }

        public int Product_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Color { get; set; }

        [Required]
        [StringLength(10)]
        public string Size { get; set; }

        public int Stock { get; set; }

        public bool Discontinued { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual Product Product { get; set; }
    }
}
