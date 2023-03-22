using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class OrderDTO
	{
		public int OrderId { get; set; }  
		public int MemberId { get; set; }
		public IEnumerable<OrderDetailsDTO> OrderDetails { get; set; }
		public int Total { get; set; }
		public string Payment { get; set; }
		public string ShipVia { get; set; }
		public int Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipPhone { get; set; }
		public DateTime CreatedTime { get; set; }
		public int StatusId { get; set; }
		public string Status { get; set; }
		public int? StatusDescriptionId { get; set; }
		public int Discount { get; set; }
		public string PayInfo { get; set; }


    }

	public static class OrderExtension
	{
		public static OrderDTO ToDTO(this Order value)
		{
			OrderDTO data = new OrderDTO
			{
				OrderId = value.OrderId,
				MemberId = value.MemberId,
				OrderDetails = value.OrderDetails.Select(x => x.ToDTO()),
				Total = value.Total,
				Payment = value.Payment,
				ShipVia = value.ShipVia,
				ShipName = value.ShipName,
				ShipPhone = value.ShipPhone,
				ShipAddress = value.ShipAddress,
				CreatedTime = value.CreatedTime,
				StatusId = value.StatusId,//有待付款狀態
				Status = value.Status.Status,
				StatusDescriptionId = value.StatusDescriptionId,
				Freight = value.Freight,
				Discount = value.Discount ?? 0,
				PayInfo = value.PayInfo,

			};
			return data;
		}

		public static OrderDetailsDTO ToDTO(this OrderDetail value)
		{
			var data = new OrderDetailsDTO
			{
				OrderId = value.OrderId,
				ProductId = value.ProductId,
				ProductName = value.ProductName,
				Quantity = value.Quantity,
				UnitPrice = value.UnitPrice,
				SubTotal = value.SubTotal,
				Color = value.Color,
				Size = value.Size,
			};
			return data;
				 
			
		}
	}
}
