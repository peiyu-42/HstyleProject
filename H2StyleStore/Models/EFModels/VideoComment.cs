namespace H2StyleStore.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VideoComment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VideoComment()
        {
            VCommentLikes = new HashSet<VCommentLike>();
        }

        public int Id { get; set; }

        public int VideoId { get; set; }

        public int MemberId { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedTime { get; set; }

        public int Like { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VCommentLike> VCommentLikes { get; set; }

        public virtual Video Video { get; set; }
    }
}
