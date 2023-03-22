namespace HStyleApi.Models.DTOs
{
    public class CartDTO
    {
		public int MemberId { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string Image { get; set; }
		public int SpecId { get; set; }
		public string Size { get; set; }
		public string Color { get; set; }
		public bool Discontinued { get; set; }

		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
		private int _total = 0;
		
	}
}
