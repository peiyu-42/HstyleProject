using H2StyleStore.filter;
using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using Microsoft.Win32;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("2", "3")]
	public class OrderController : Controller
	{
		private OrderService orderService;

		public OrderController()
		{
			var db = new AppDbContext();
			IOrderRepository repo = new OrderRepository(db);
			this.orderService = new OrderService(repo);
		}

		// GET: Order
		public ActionResult Index(int? status_id, int? pageSize)
		{
			ViewBag.Status = orderService.GetStatus(status_id);
			List<SelectListItem> pageSizeList = new List<SelectListItem>
			{
			new SelectListItem{ Text = "5", Value = "5", Selected = pageSize == 5},
			new SelectListItem{ Text = "10", Value = "10", Selected = pageSize == 10},
			new SelectListItem{ Text = "15", Value = "15", Selected = pageSize == 15},
			};
			ViewBag.pageSizeList = pageSizeList;
			var data = orderService.Load()
				.Select(x => x.ToVM());

			return View(data);
		}


		public ActionResult PartialPage(int? status_id, string searchString, string sortOrder, int? page, int? pageSize)
		{

			ViewBag.Status_order = orderService.GetStatus();
			//ViewBag.CreatetimeSortParm = sortOrder == "date" ? "date_desc" : "date";
			//ViewBag.TotalSortParm = sortOrder == "total" ? "total_desc" : "";

			var data = orderService.Load();

			//可排序
			switch (sortOrder)
			{

				//	case "date":
				//		data = data.OrderBy(o => o.CreatedTime);
				//		break;
				//	case "date_desc":
				//		data = data.OrderByDescending(o => o.CreatedTime);
				//		break;
				//	case "total":
				//		data = data.OrderBy(o => o.Total);
				//		break;
				//	case "total_desc":
				//		data = data.OrderByDescending(o => o.Total);
				//		break;
				default:
					data = data.OrderByDescending(o => o.Order_id);
					break;
			}


			//可篩選
			if (status_id.HasValue)
			{
				data = data.Where(s => s.Status_id == status_id.Value);
			}
			//可搜尋
			if (string.IsNullOrEmpty(searchString) == false)
			{
				data = data.Where(n => n.MemberName.Contains(searchString) || n.Order_id.ToString().Contains(searchString));
			}
			var list = data.Select(x => x.ToVM());

			//檢視頁數
			pageSize = pageSize ?? 10;
			int pageNumber = (page ?? 1);

			return PartialView(list.ToPagedList(pageNumber, (int)pageSize));
		}
		[HttpGet]
		public ActionResult Search(string key)
		{
			//參數key為使用者輸入在input的資訊
			var dataIds = orderService.Load()
				.Select(x => x.Order_id.ToString())
				.Where(x=> x.StartsWith(key))
				.Take(5).ToList(); //拿取前五筆資料配對的資料

			var dataNames = orderService.Load()
				.Select(x => x.MemberName)
				.Where(x => x.StartsWith(key))
				.Take(5).ToList(); //拿取前五筆資料配對的資料

			var data = dataIds.Union(dataNames);

			return Json(data.DefaultIfEmpty(), JsonRequestBehavior.AllowGet);
		}
		public ActionResult Details(int id)
		{
			var data = orderService.FindById(id).Select(x => x.ToVM());
			if (id != null)
			{
				data = data.Where(od => od.Order_id == id);
			}

			return View(data);
		}

		[HttpPost]
		public ActionResult Update(OrderUpdateVM[] orders)
		{
			try
			{
				for (int i = 0; i < orders.Length; i++)
				{
					orders[i].EmployeeAccount = this.User.Identity.Name;
					var status_id = orderService.StatusTransfer(orders[i].Status);
					orders[i].Status_id = status_id;
					orderService.Update(orders[i].ToDTO());
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "修改失敗: " + ex.Message);
			}
			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}

			return RedirectToAction("Index");


		}
		public ActionResult Edit(int id)
		{
			ViewBag.Status_order = orderService.GetStatus();
			ViewBag.StatusDescription = orderService.GetStatusDescription();
			var data = orderService.GetOrderbyId(id).ToVM();

			return View(data);
		}

		[HttpPost]
		public ActionResult Edit(OrderVM order)
		{
			try
			{
				order.EmployeeAccount = this.User.Identity.Name;
				orderService.Update(order.ToDTO());
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, "修改失敗: " + ex.Message);
			}
			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}

			return View(order);

		}

		[HttpPost]
		public string Cancel(int status_Description, int order_id)
		{

			try
			{
				orderService.Cancel(status_Description, order_id);
			}
			catch (Exception ex)
			{

				return "取消訂單失敗，原因:" + ex.Message;
			}
			return "取消訂單成功";


		}
   	
		[HttpPost]
		public void SendMail(int status_Description, int order_id)
		{
			var member = orderService.GetMember(order_id);
			var content = orderService.GetCancelDescription(status_Description);
			//設定smtp主機
			string smtpAddress = "smtp.gmail.com";
			//設定Port
			int portNumber = 587;
			bool enableSSL = true;
			//填入寄送方email和密碼
			string emailFrom = "h11830123@gmail.com";
			string password = "yetzwlvxoejigxns";
			//收信方email 可以用逗號區分多個收件人
			string emailTo = member.Email;
			//主旨
			string subject = $"您於H'style購買的訂單編號#{order_id}訂單狀態改為:已取消";
			//內容
			string body = $"親愛的{member.Name}您好,\r\n 訂單編號#{order_id}訂單狀態改為:已取消。\r\n取消原因:{content}\r\n如有疑慮請來電客服，若造成您的不便敬請見諒！";

			using (MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(emailFrom, "H'style");
				mail.To.Add(emailTo);
				mail.Subject = subject;
				mail.Body = body;
				// 若你的內容是HTML格式，則為True
				mail.IsBodyHtml = false;

				using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
				{
					smtp.Credentials = new NetworkCredential(emailFrom, password);
					smtp.EnableSsl = enableSSL;
					smtp.Send(mail);
				}
			}
		}

	}
}