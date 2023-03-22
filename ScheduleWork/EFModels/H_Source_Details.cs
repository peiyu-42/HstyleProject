namespace ScheduleWork.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class H_Source_Details
    {
        [Key]
        public int Source_H_Id { get; set; }

        public int Member_Id { get; set; }

        public int Activity_Id { get; set; }

        public int Difference_H { get; set; }

        public DateTime Event_Time { get; set; }

        public int? Total_H_SoFar { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Employee_Id { get; set; }

        public virtual H_Activities H_Activities { get; set; }

        public virtual Member Member { get; set; }
    }
}
