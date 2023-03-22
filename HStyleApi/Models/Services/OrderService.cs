using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;

namespace HStyleApi.Models.Services
{
	public class OrderService
	{
		private readonly OrderRepo _repo;
		public OrderService(OrderRepo repo)
		{
			_repo = repo;
		}

		public IEnumerable<OrderDTO> GetOrders(int memberId)
		{
			var orders = _repo.GetOrders(memberId);
			return orders;
		}

		public OrderDTO GetOrder(int orderId)
		{
			var order = _repo.GetOrder(orderId);
			return order;
		}

		public void SavePayInfo(int orderId, string payInfo)
		{
			_repo.SavePayInfo( orderId, payInfo);
		}

        public void UpdateOrder(string paypaltoken)
        {
            _repo.UpdateOrder(paypaltoken);
        }

        public void returnGoods(int orderId)
        {
			//檢查時間、狀態
			bool canReturn = _repo.CheckStatus(orderId);
			if(canReturn)
			{
                _repo.returnGoods(orderId);

            }

            
        }
    }
}
