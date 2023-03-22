namespace HStyleApi.Models.DTOs
{
	public class CartListDTO
	{
		public int MemberId { get; set; }
		public IEnumerable<CartDTO> CartItems { get;set; }
		private int _total = 0;
		
		public int Total
		{
			get
			{
				int total = 0;
				foreach (var cart in this.CartItems)
				{
					total += cart.Quantity * cart.UnitPrice;
				}
				return total;
			}
			
		}
		


	}
}
