using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using X.PagedList;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IOrderRepository
	{
		IEnumerable<OrderDTO> Load();

		IEnumerable<SelectListItem> GetStatus(int? status);

		IEnumerable<Order_DetailDTO> FindById(int? id);

		void Update(OrderDTO entity);
		OrderDTO GetOrderbyId(int id);
		IEnumerable<SelectListItem> GetStatus();
		int StatusTransfer(string status);
		IEnumerable<SelectListItem> GetStatusDescription();
		void Cancel(int status_Description, int order_id);
        string GetCancelDescription(int status_Description);
		MemberDto GetMember(int order_id);
    }
}
