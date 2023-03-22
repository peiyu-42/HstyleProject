using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
    public class UpdateProfileDto
    {
        public string Account { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
    }
}