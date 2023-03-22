namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Essay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Essay()
        {
            Eassy_Follows = new HashSet<Eassy_Follows>();
            Elikes = new HashSet<Elike>();
            Essays_Comments = new HashSet<Essays_Comments>();
            Images = new HashSet<Image>();
            Tags = new HashSet<Tag>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Eassy_Follows> Eassy_Follows { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Elike> Elikes { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Essays_Comments> Essays_Comments { get; set; }

        public virtual VideoCategory VideoCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
