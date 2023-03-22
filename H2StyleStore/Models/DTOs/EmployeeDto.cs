using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class EmployeeDto
	{
		public int Employee_id { get; set; }


		public string Account { get; set; }

		public string Title { get; set; }

		public int? Permission_id { get; set; }

		public string EncryptedPassword { get; set; }

		public virtual PermissionsE PermissionsE { get; set; }

	}
}