using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using Newtonsoft.Json.Linq;

namespace HStyleApi.Models.Services
{
    public class CartService
    {
		private HCoinRepository _hCoinRepo;
		private readonly CartRepo _repo;
        public CartService(AppDbContext db)
        {
            _repo = new CartRepo(db);
			_hCoinRepo = new HCoinRepository(db);
		}
		public CartListDTO GetCart(int memberId)
        {
            return _repo.GetCart(memberId);
        }

        public OrderDTO GetCheckout(int memberId, CheckoutDTO value)
        {
			var cartlist = GetCart(memberId);
			bool isInStock = _repo.CheckStocks(memberId);
			double discountPercentage = 0.2;
			
			if(isInStock == false)
			{
				throw new Exception("您有商品庫存不足，已刪除不足品項，請重新選購");
			}
			if (value.Discount < 0) { throw new Exception("優惠幣欄位不可為負值"); }
			
			int freight = 100;
			int freeShippingStd = 10000;
			OrderDTO newOrder = new OrderDTO
			{
				MemberId = memberId,
				OrderDetails = cartlist
				.CartItems
				.Select(x => new OrderDetailsDTO
				{
					ProductId = x.ProductId,
					ProductName = x.ProductName,
					Quantity = x.Quantity,
					UnitPrice = x.UnitPrice,
					SubTotal = x.Quantity * x.UnitPrice,
					Color = x.Color,
					Size = x.Size,
				}),
				Total = cartlist.Total,
				Payment = value.Payment,
				ShipVia = value.ShipVia,
				ShipName = value.ShipName,
				ShipPhone = value.ShipPhone,
				ShipAddress = value.ShipAddress,
				CreatedTime = DateTime.Now,
				Discount = value.Discount ?? 0,
				StatusId = 1,//待付款
				StatusDescriptionId = 9,

			};
			newOrder.Freight = newOrder.Total > freeShippingStd ? 0 : freight;
			int memberTotalHCoin = _hCoinRepo.GetTotalHByMemberId(memberId);
			if (memberTotalHCoin < newOrder.Discount || newOrder.Discount > newOrder.Total * discountPercentage)
			{ throw new Exception("H幣不足或不符使用規範"); }
			newOrder.Total -= (int)newOrder.Discount;

			return newOrder;
		}

		public void AddItem(int memberId, int specId)
        {
			var cartItem = _repo.GetCart(memberId).CartItems.Where(x => x.SpecId == specId).FirstOrDefault();
			int qty = cartItem == null ? 1 : cartItem.Quantity+1;

			if (_repo.CheckStock(specId) < qty) throw new Exception("庫存量不足，已刪除品項，請重新選購");    

            if (cartItem == null)
            {
                _repo.AddItem(memberId,specId);
            }
            else
            {
				_repo.UpdateItem(memberId, specId, qty);
			}
            
            //cart.AddItem(cartProd, qty);

            
        }
		public void MinusItem(int memberId, int specId)
		{
			var cartItem = _repo.GetCart(memberId).CartItems.Where(x => x.SpecId == specId).FirstOrDefault();

			if (cartItem == null) throw new Exception("商品已不存在");

			int qty = cartItem.Quantity - 1;
			if (qty == 0)
			{
				_repo.DeleteItem(memberId, specId);
			}
			else
			{
				_repo.UpdateItem(memberId, specId, qty);
			}

			//cart.AddItem(cartProd, qty);


		}

		public int CreateOrder( OrderDTO checkoutList)
		{

			int orderId = _repo.CreateOrder(checkoutList);
			if (checkoutList.Discount > 0) _hCoinRepo.PostCostHCoinByMemberId(checkoutList.MemberId, (int)checkoutList.Discount);

			return orderId;
		}
	}
    
}
