namespace HStyleApi.Models.DTOs
{
	public class CheckoutDTO
	{
		public string Payment { get; set; }
		public string ShipVia { get; set; }
		public int Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipPhone { get; set; }
		public int? Discount { get; set; }

	}
}
