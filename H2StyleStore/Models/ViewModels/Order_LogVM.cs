using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class Order_LogVM
	{
		public int Log_id { get; set; }

		public int Order_id { get; set; }

		public DateTime Status_ChangedTime { get; set; }

		public string Status { get; set; }
	}
}