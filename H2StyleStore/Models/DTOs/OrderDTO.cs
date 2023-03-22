using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class OrderDTO
	{
		public int Order_id { get; set; }

		public int Member_id { get; set; }

		public string MemberName { get; set; }

		public int? Employee_id { get; set; }

		public string EmployeeAccount { get; set; }

		public int Total { get; set; }

		public string Payment { get; set; }

		public DateTime? ShippedDate { get; set; }

		public string ShipVia { get; set; }

		public int Freight { get; set; }

		public string ShipName { get; set; }

		public string ShipAddress { get; set; }

		public string ShipPhone { get; set; }

		public DateTime? RequestRefundTime { get; set; }

		public bool RequestRefund { get; set; }

		public DateTime CreatedTime { get; set; }

		public int Status_id { get; set; }

		public int? Status_Description_id { get; set; }
		public string Status { get; set; }

		public string Status_Description{ get; set; }

		public IEnumerable<Order_DetailDTO> Order_Details { get; set; }
	}

	public static class OrderExts
	{
		public static OrderDTO ToDTO(this Order entity)
		{
			return entity == null
				? null
				: new OrderDTO
				{
					Order_id = entity.Order_id,
					Member_id = entity.Member_id,
					MemberName = entity.Member.Name,
					Employee_id = entity.Employee_id,
					Total = entity.Total,
					Payment = entity.Payment,
					ShippedDate = entity.ShippedDate,
					ShipVia = entity.ShipVia,
					Freight = entity.Freight,
					ShipName = entity.ShipName,
					ShipAddress = entity.ShipAddress,
					ShipPhone = entity.ShipPhone,
					RequestRefund = entity.RequestRefund,
					RequestRefundTime = entity.RequestRefundTime,
					CreatedTime = entity.CreatedTime,
					Status_id = entity.Status_id,
					Status = entity.Order_Status.Status,
					Status_Description_id = entity.Status_Description_id,
					Status_Description = entity.Order_StatusDescription.Description,
					Order_Details = entity.Order_Details.Select(x => x.ToDTO()),

				};
		}
	}
}