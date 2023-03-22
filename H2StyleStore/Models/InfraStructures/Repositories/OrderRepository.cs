using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using PagedList;
using H2StyleStore.Models.Infrastructures.ExtensionMethods;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AppDbContext _db;

		public OrderRepository(AppDbContext db)
		{
			_db = db;
		}

		public void Update(OrderDTO entity)
		{
			Order order = _db.Orders.Find(entity.Order_id);
            Employee employee = _db.Employees.Where(x => x.Account == entity.EmployeeAccount).FirstOrDefault();

            var log = new Order_Log
			{
				Order_id = entity.Order_id,
				Status = entity.Status,
				Employee_id= employee.Employee_id,
				Status_ChangedTime = DateTime.Now,
			};

			_db.Order_Log.Add(log);
			Order_Status newStatus = _db.Order_Status.Where(x => x.Status == entity.Status).FirstOrDefault();

			

			order.Status_id = newStatus.Status_id;
			order.ShippedDate = entity.ShippedDate;
			order.Employee_id = employee.Employee_id;

			Order_StatusDescription newDescription = _db.Order_StatusDescription.Where(x => x.Status_id == newStatus.Status_id).FirstOrDefault();
			order.Status_Description_id = newDescription.Description_id;

			try
			{
				_db.SaveChanges();
			}
			catch (Exception)
			{

				throw;
			}

		}

		public IEnumerable<Order_DetailDTO> FindById(int? id)
		{
			IEnumerable<Order_Details> order_detail = _db.Order_Details;
			var data = order_detail.Select(od => od.ToDTO());

			return data;
		}

		public IEnumerable<SelectListItem> GetStatus(int? status)
		{
			var items = _db.Order_Status.Select(x => new SelectListItem
			{
				Value = x.Status_id.ToString(),
				Text = x.Status,
				Selected = (status.HasValue && x.Status_id == status.Value)
			})
			.ToList()
			.Prepend(new SelectListItem { Value = string.Empty, Text = "所有" });

			return items;
		}

		public IEnumerable<OrderDTO> Load()
		{
			IEnumerable<Order> orders = _db.Orders;
			var data = orders.Select(o => o.ToDTO());

			return data;
		}

		public OrderDTO GetOrderbyId(int id)
		{
			OrderDTO order = _db.Orders.Find(id).ToDTO();
			if (order.Employee_id == null)
			{
				order.EmployeeAccount = string.Empty;
			}
			else
			{
				Employee employee = _db.Employees.Where(x => x.Employee_id == order.Employee_id).FirstOrDefault();
				order.EmployeeAccount = employee.Account;
			}

			return order;
		}

		public IEnumerable<SelectListItem> GetStatus()
		{
			var items = _db.Order_Status.Where(x => x.Status_id != 6).Select(x => new SelectListItem
			{
				Value = x.Status_id.ToString(),
				Text = x.Status,
			})
			.ToList();

			return items;
		}

		public int StatusTransfer(string status)
		{
			var data = _db.Order_Status.FirstOrDefault(x => x.Status == status);
			int result = data.Status_id;

			return result;
		}

		public IEnumerable<SelectListItem> GetStatusDescription()
		{
			var items = _db.Order_StatusDescription.Where(x => x.Status_id == 6).Select(x => new SelectListItem
			{
				Value = x.Description_id.ToString(),
				Text = x.Description,
			})
			.ToList()
			.Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇" });

			return items;
		}

		public void Cancel(int status_Description, int order_id)
		{
			Order order = _db.Orders.Find(order_id);

			var log = new Order_Log
			{
				Order_id = order_id,
				Status = "取消",
				Status_ChangedTime = DateTime.Now,
			};

			_db.Order_Log.Add(log);

			order.Status_Description_id = status_Description;
			if (status_Description < 6)
			{
				order.Status_id = 6;
			}

			_db.SaveChanges();

		}

        public MemberDto GetMember(int order_id)
        {
            OrderDTO order = _db.Orders.Find(order_id).ToDTO();
            MemberDto member = _db.Members.FirstOrDefault(x => x.Id == order.Member_id).ToDto();

			return member;
        }

        public string GetCancelDescription(int status_Description)
        {
            Order_StatusDescription description = _db.Order_StatusDescription.Find(status_Description);
			string content = description.Description;
			return content;
        }
    }
}