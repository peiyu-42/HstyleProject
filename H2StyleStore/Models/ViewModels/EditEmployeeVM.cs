using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class EditEmployeeVM
	{
		public int Employee_id { get; set; }

		[Required]
		[StringLength(50)]
		public string Account { get; set; }

		[StringLength(50)]
		public string Title { get; set; }

		public int? Permission_id { get; set; }


	}
}