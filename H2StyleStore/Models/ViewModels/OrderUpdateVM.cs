using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class OrderUpdateVM
	{
		public int Order_id { get; set; }

		public DateTime? ShippedDate { get; set; }

		public string Status { get; set; }

		public string EmployeeAccount { get; set; }

		public int Status_id { get; set; }
	}
	public static class OrderUpdateVMExts
	{
		public static OrderDTO ToDTO(this OrderUpdateVM source)
		{
			return new OrderDTO
			{
				Order_id = source.Order_id,
				ShippedDate = source.ShippedDate,
				EmployeeAccount= source.EmployeeAccount,
				Status_id = source.Status_id,
				Status= source.Status,
			};
		}
	}
}