namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Details = new HashSet<Order_Details>();
            Product_Comments = new HashSet<Product_Comments>();
        }

        [Key]
        public int Order_id { get; set; }

        public int Member_id { get; set; }

        public int? Employee_id { get; set; }

        public int Total { get; set; }

        [Required]
        [StringLength(20)]
        public string Payment { get; set; }

        public DateTime? ShippedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipVia { get; set; }

        public int Freight { get; set; }

        [Required]
        [StringLength(10)]
        public string ShipName { get; set; }

        [Required]
        [StringLength(60)]
        public string ShipAddress { get; set; }

        [Required]
        [StringLength(10)]
        public string ShipPhone { get; set; }

        public DateTime? RequestRefundTime { get; set; }

        public bool RequestRefund { get; set; }

        public DateTime CreatedTime { get; set; }

        public int Status_id { get; set; }

        public int? Status_Description_id { get; set; }

        public int? Discount { get; set; }

        [StringLength(50)]
        public string PayInfo { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        public virtual Order_Status Order_Status { get; set; }

        public virtual Order_StatusDescription Order_StatusDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Comments> Product_Comments { get; set; }
    }
}
