namespace ScheduleWork.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Essay
    {
        [Key]
        public int Essay_Id { get; set; }

        public int Influencer_Id { get; set; }

        public DateTime UplodTime { get; set; }

        [Required]
        [StringLength(500)]
        public string ETitle { get; set; }

        [Required]
        public string EContent { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpLoad { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Removed { get; set; }

        public int CategoryId { get; set; }

        public bool? PON { get; set; }
    }
}
