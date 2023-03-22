namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        [Key]
        public int address_id { get; set; }

        public int Member_id { get; set; }

        public bool? Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string destination_name { get; set; }

        [Required]
        [StringLength(100)]
        public string destination { get; set; }

        [Required]
        [StringLength(10)]
        public string destination_THE { get; set; }

        public bool? preset { get; set; }

        [StringLength(50)]
        public string destination_category { get; set; }

        public virtual Member Member { get; set; }
    }
}
