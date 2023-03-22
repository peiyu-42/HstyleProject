namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Comments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product_Comments()
        {
            Images = new HashSet<Image>();
        }

        [Key]
        public int Comment_id { get; set; }

        public int Product_id { get; set; }

        public int Order_id { get; set; }

        [StringLength(1000)]
        public string Comment_content { get; set; }

        public int Score { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }
    }
}
