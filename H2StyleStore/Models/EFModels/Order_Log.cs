namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_Log
    {
        [Key]
        public int Log_id { get; set; }

        public int Order_id { get; set; }

        public DateTime Status_ChangedTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public int? Employee_id { get; set; }
    }
}
