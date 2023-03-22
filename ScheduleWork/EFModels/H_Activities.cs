namespace ScheduleWork.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class H_Activities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public H_Activities()
        {
            H_CheckIns = new HashSet<H_CheckIns>();
            H_Source_Details = new HashSet<H_Source_Details>();
        }

        [Key]
        public int H_Activity_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Activity_Name { get; set; }

        [Required]
        public string Activity_Describe { get; set; }

        public int H_Value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<H_CheckIns> H_CheckIns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<H_Source_Details> H_Source_Details { get; set; }
    }
}
