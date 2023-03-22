using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace HStyleApi.Models.InfraStructures.Repositories
{
    public class CartRepo
    {
        private readonly AppDbContext _db;
        private string _basePath;
        public CartRepo(AppDbContext db)
        {
            _db= db;
		    _basePath = "https://localhost:44313/Images";
		}
        
        public void AddItem(int memberId, int specId)
		{
            var newItem = new Cart()
            {
                MemberId = memberId,
                SpecId = specId,
                Quantity = 1
            };
            _db.Add(newItem);
            _db.SaveChanges();
        }

        public CartListDTO GetCart(int memberId)
        {
            var cartItems =_db.Carts
                .Where(c => c.MemberId == memberId)
                .Include(c => c.Spec)
                .ThenInclude(s => s.Product)
                .ThenInclude(p => p.Imgs)
                .Select(x => new CartDTO()
            {
                MemberId = x.MemberId,
                ProductId=x.Spec.ProductId,
                ProductName = x.Spec.Product.ProductName,
                Image =$"{_basePath}/ProductImages/{x.Spec.Product.Imgs.FirstOrDefault().Path}",
                SpecId= x.SpecId,
                Size= x.Spec.Size,
                Color= x.Spec.Color,
                Discontinued = x.Spec.Discontinued,
                Quantity= x.Quantity,
				UnitPrice = x.Spec.Product.UnitPrice,

            });

            var cartList = new CartListDTO()
            {
                MemberId= memberId,
                CartItems = cartItems,

            };
            return cartList; 
        }

        public int CheckStock(int specId)
        {
            var stock = (from product in _db.Products
                       join spec in _db.Specs
                       on product.ProductId equals spec.ProductId
                       where spec.Id == specId
                       select spec.Stock).FirstOrDefault();
            return stock;
        }
		public bool CheckStocks(int memberId)
		{
            var shortItems = _db.Carts.Include(x => x.Spec).Where(x => x.MemberId == memberId && x.Quantity > x.Spec.Stock);
            bool isShort = shortItems.Count() > 0;
            _db.Carts.RemoveRange(shortItems);
            _db.SaveChanges();
            
			return !isShort;
		}
		public bool IsExit(int memberId, int specId)
        {
			var cartItems = _db.Carts.Where(x => x.MemberId == memberId && x.SpecId == specId);
            return cartItems.Any();
		}
		public bool IsOne(int memberId, int specId)
		{
			var cartItems = _db.Carts.Where(x => x.MemberId == memberId && x.SpecId == specId).SingleOrDefault();

            if (cartItems == null) throw new Exception("購物車中沒有此商品");
			return cartItems.Quantity==1;
		}
		public void UpdateItem(int memberId, int specId, int qty)
        {
            var cart = _db.Carts.Where(x => x.MemberId == memberId && x.SpecId == specId).SingleOrDefault();
            cart.Quantity = qty; 

            _db.SaveChanges();
        }
		public void DeleteItem(int memberId, int specId)
		{
			var cart = _db.Carts.Where(x => x.MemberId == memberId && x.SpecId == specId).SingleOrDefault();
			_db.Carts.Remove(cart);

			_db.SaveChanges();
		}

		public int CreateOrder(OrderDTO value)
		{
            if (value.Total <= 0)
            {
                throw new Exception("購物車內沒有商品");
            }
			var order = new Order()
            {
				MemberId = value.MemberId,
				OrderDetails = value.OrderDetails
				.Select(x => new OrderDetail
				{
					ProductId = x.ProductId,
					ProductName = x.ProductName,
					Quantity = x.Quantity,
					UnitPrice = x.UnitPrice,
					SubTotal = x.Quantity * x.UnitPrice,
					Color = x.Color,
					Size = x.Size,
				}).ToList(),
				Total = value.Total,
                Discount = value.Discount,
				Payment = value.Payment,
				ShipVia = value.ShipVia,
				ShipName = value.ShipName,
				ShipPhone = value.ShipPhone,
				ShipAddress = value.ShipAddress,
				CreatedTime = DateTime.Now,
				StatusId = value.StatusId,
				StatusDescriptionId = value.StatusDescriptionId,
			};
            _db.Orders.Add(order);
			_db.SaveChanges();
			var orderId = order.OrderId;
            var dataLog = new OrderLog
            {
                OrderId = orderId,
                StatusChangedTime = DateTime.Now,
                Status = "待付款",
            };
            _db.OrderLogs.Add(dataLog);
            var cartItems = _db.Carts.Where(x => x.MemberId == order.MemberId);
            var dbStock = _db.Specs.Where(x => cartItems.Select(c =>c.SpecId).Contains(x.Id)).ToList();
            foreach (var cartItem in cartItems)
            {
                dbStock.Where(x => x.Id == cartItem.SpecId).FirstOrDefault().Stock -= cartItem.Quantity;
            }
            _db.Carts.RemoveRange(cartItems);

            _db.SaveChanges();
            return order.OrderId;
		}
	}
}
