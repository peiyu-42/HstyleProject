﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HStyleApi.Models.EFModels
{
    public partial class EssaysComment
    {
        public EssaysComment()
        {
            EcommentsLikes = new HashSet<EcommentsLike>();
        }

        public int CommentId { get; set; }
        public int MemberId { get; set; }
        public int EssayId { get; set; }
        public string Ecomment { get; set; }
        public DateTime Etime { get; set; }
        public int Elike { get; set; }

        public virtual Essay Essay { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<EcommentsLike> EcommentsLikes { get; set; }
    }
}