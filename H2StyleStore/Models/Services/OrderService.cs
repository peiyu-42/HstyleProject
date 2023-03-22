using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace H2StyleStore.Models.Services
{
	public class OrderService
	{
		private readonly IOrderRepository _repository;

		public OrderService(IOrderRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<OrderDTO> Load()
		{
			return _repository.Load();
		}

		public IEnumerable<SelectListItem> GetStatus(int? status)
		{
			return _repository.GetStatus(status);
		}


		public IEnumerable<Order_DetailDTO> FindById(int? id)
		{
			return _repository.FindById(id);
		}

		public void Update(OrderDTO entity)
		{
			if (entity == null) throw new Exception("找不到要修改的訂單");

			_repository.Update(entity);
		}

		public OrderDTO GetOrderbyId(int id)
		{
			return _repository.GetOrderbyId(id);
		}

		public IEnumerable<SelectListItem> GetStatus()
		{
			return _repository.GetStatus();
		}

		public int StatusTransfer(string status)
		{
			return _repository.StatusTransfer(status);
		}

		public IEnumerable<SelectListItem> GetStatusDescription()
		{
			return _repository.GetStatusDescription();
		}

		public void Cancel(int status_Description, int order_id)
		{
		    _repository.Cancel(status_Description, order_id);
		}

        public MemberDto GetMember(int order_id)
        {
            return _repository.GetMember(order_id);
        }

        public string GetCancelDescription(int status_Description)
        {
            return _repository.GetCancelDescription(status_Description);
        }
    }
}