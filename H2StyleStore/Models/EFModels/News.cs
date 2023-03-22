namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        [Key]
        public int News_Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string NTitle { get; set; }

        public DateTime NTime { get; set; }

        [Required]
        [StringLength(1000)]
        public string NContent { get; set; }

        public int NProduct_Id { get; set; }

        public int Photo_Id { get; set; }

        public int Tag_Id { get; set; }
    }
}
