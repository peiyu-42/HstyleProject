namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonQuestion
    {
        [Key]
        public int CommonQuestion_Id { get; set; }

        public int QCategory_Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        public int Satisfaction_Yes { get; set; }

        public int Satisfaction_No { get; set; }

        public virtual Question_Categories Question_Categories { get; set; }
    }
}
