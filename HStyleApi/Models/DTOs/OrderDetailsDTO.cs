using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class OrderDetailsDTO
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int UnitPrice { get; set; }
		public int Quantity { get; set; }
		public string Color { get; set; }
		public string Size { get; set; }

		public int SubTotal { get; set; }
	}

	public static class OrderDetailsExt
	{
		public static OrderDetailsDTO ToDto(this OrderDetail source)
		{
			return new OrderDetailsDTO
			{
				OrderId = source.OrderId,
				ProductId = source.ProductId,	
				ProductName = source.ProductName,
				UnitPrice = source.UnitPrice,
				Quantity = source.Quantity,
				Color = source.Color,
				Size = source.Size,
				SubTotal = source.SubTotal,
			};
		}
	}
}
