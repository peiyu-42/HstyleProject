﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HStyleApi.Models.EFModels
{
    public partial class Spec
    {
        public Spec()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }
        public bool Discontinued { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}