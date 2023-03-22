using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace HStyleApi.Models.InfraStructures.Repositories
{
	public class OrderRepo
	{
		private readonly AppDbContext _db;
		public OrderRepo(AppDbContext db )
		{
			_db = db;
		}

		public IEnumerable<OrderDTO> GetOrders(int memberId)
		{
			var data = _db.Orders.Include(o => o.OrderDetails)
								.Include(o => o.Status)
								.Where(o => o.MemberId== memberId).OrderByDescending(x => x.OrderId).Select(x => x.ToDTO());

			return data;
			
		}
		public OrderDTO GetOrder(int orderId)
		{
			var data = _db.Orders.Include(o => o.OrderDetails).Include(o => o.Status).Where(o => o.OrderId == orderId).Select(x => x.ToDTO()).FirstOrDefault();


			return data;

		}

		public void SavePayInfo(int orderId,string paypalInfo)
		{
			var order = _db.Orders.Where(x =>x.OrderId == orderId).SingleOrDefault();
            order.PayInfo = paypalInfo;

			_db.SaveChanges();

		}

        public void UpdateOrder(string palpaltoken)
        {
            var order = _db.Orders.Where(o => o.PayInfo== palpaltoken).SingleOrDefault();
			order.StatusId = 2;
			order.StatusDescriptionId = 10;
			var dataLog = new OrderLog
			{
				OrderId = order.OrderId,
				StatusChangedTime = DateTime.Now,
				Status = "待處理",
			};
			_db.OrderLogs.Add(dataLog);
			_db.SaveChanges();
        }
		public void UpdateOrder(int orderId)
		{
			var order = _db.Orders.Where(o => o.OrderId == orderId).SingleOrDefault();
			order.StatusId = 7;
			order.StatusDescriptionId = 6;
			var dataLog = new OrderLog
			{
				OrderId = order.OrderId,
				StatusChangedTime = DateTime.Now,
				Status = "待處理",
			};
			_db.OrderLogs.Add(dataLog);
			_db.SaveChanges();
		}

		public void returnGoods(int orderId)
        {
            var order = _db.Orders.Where(x => x.OrderId == orderId).SingleOrDefault();
			order.RequestRefundTime = DateTime.Now;
			order.RequestRefund = true;
			order.StatusId = 7;
			order.StatusDescriptionId = 6;
			var dataLog = new OrderLog
			{
				OrderId = order.OrderId,
				StatusChangedTime = DateTime.Now,
				Status = "退貨處理中",
			};
			_db.OrderLogs.Add(dataLog);
			_db.SaveChanges();
        }

        public bool CheckStatus(int orderId)
        {
			var order = _db.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();

            if (order.CreatedTime.AddDays(14) < DateTime.Now)
			{
				throw new Exception("您的訂單已超過允許退貨時間");
			}
			if(order.StatusId == 7)
			{
                throw new Exception("您的訂單退貨申請已在處理中");
            }
			return true;
        }
    }
}
