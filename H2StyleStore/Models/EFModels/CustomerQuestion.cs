namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerQuestion
    {
        [Key]
        public int CustomerQuestion_Id { get; set; }

        public int? Member_Id { get; set; }

        public int QCategory_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Problem_Description { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        public DateTime AskTime { get; set; }

        public string Solution_Description { get; set; }

        public DateTime? SolveTime { get; set; }

        public int? Employee_Id { get; set; }

        public virtual Member Member { get; set; }

        public virtual Question_Categories Question_Categories { get; set; }
    }
}
