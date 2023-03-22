namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class H_CheckIns
    {
        [Key]
        public int CheckIn_H_Id { get; set; }

        public int Member_Id { get; set; }

        public int Activity_Id { get; set; }

        public int CheckIn_Times { get; set; }

        public DateTime Last_Time { get; set; }

        public virtual H_Activities H_Activities { get; set; }

        public virtual Member Member { get; set; }
    }
}
