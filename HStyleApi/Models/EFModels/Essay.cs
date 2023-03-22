﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HStyleApi.Models.EFModels
{
    public partial class Essay
    {
        public Essay()
        {
            EassyFollows = new HashSet<EassyFollow>();
            Elikes = new HashSet<Elike>();
            EssaysComments = new HashSet<EssaysComment>();
            Imgs = new HashSet<Image>();
            Tags = new HashSet<Tag>();
        }

        public int EssayId { get; set; }
        public int InfluencerId { get; set; }
        public DateTime UplodTime { get; set; }
        public string Etitle { get; set; }
        public string Econtent { get; set; }
        public DateTime? UpLoad { get; set; }
        public DateTime? Removed { get; set; }
        public int CategoryId { get; set; }
        public bool? Pon { get; set; }

        public virtual VideoCategory Category { get; set; }
        public virtual Employee Influencer { get; set; }
        public virtual ICollection<EassyFollow> EassyFollows { get; set; }
        public virtual ICollection<Elike> Elikes { get; set; }
        public virtual ICollection<EssaysComment> EssaysComments { get; set; }

        public virtual ICollection<Image> Imgs { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}