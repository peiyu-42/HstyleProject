﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HStyleApi.Models.EFModels
{
    public partial class OrderLog
    {
        public int LogId { get; set; }
        public int OrderId { get; set; }
        public DateTime StatusChangedTime { get; set; }
        public string Status { get; set; }
        public int? EmployeeId { get; set; }
    }
}