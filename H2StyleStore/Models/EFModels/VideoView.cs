namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VideoView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VideoId { get; set; }

        public int Views { get; set; }

        public virtual Video Video { get; set; }
    }
}
