namespace ScheduleWork.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Video
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string FilePath { get; set; }

        public int CategoryId { get; set; }

        public int ImageId { get; set; }

        public DateTime? OnShelffTime { get; set; }

        public DateTime? OffShelffTime { get; set; }

        public bool? IsOnShelff { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
