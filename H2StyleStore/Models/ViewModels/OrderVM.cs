using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class OrderVM
	{
		[DisplayName("訂單編號")]
		public int Order_id { get; set; }

		[DisplayName("會員名稱")]
		public string MemberName { get; set; }

		[DisplayName("員工帳號")]
		public string EmployeeAccount { get; set; }

		[DisplayName("總額")]
		public int Total { get; set; }

		[DisplayName("付款方式")]
		public string Payment { get; set; }

		[DisplayName("出貨時間")]
		public DateTime? ShippedDate { get; set; }

		[DisplayName("配送方式")]
		public string ShipVia { get; set; }

		[DisplayName("運費")]
		public int Freight { get; set; }

		[Required]
		[StringLength(10)]
		[DisplayName("收件人")]
		public string ShipName { get; set; }

		[Required]
		[StringLength(60)]
		[DisplayName("寄送地址")]
		public string ShipAddress { get; set; }

		[Required]
		[StringLength(10)]
		[DisplayName("收件人電話")]
		public string ShipPhone { get; set; }

		[DisplayName("退貨時間")]
		public DateTime? RequestRefundTime { get; set; }

		[DisplayName("退貨")]
		public bool RequestRefund { get; set; }

		[DisplayName("訂單成立日期")]
		public DateTime CreatedTime { get; set; }


		[DisplayName("訂單狀態")]
		public string Status { get; set; }
		public int Status_id { get; set; }

		[DisplayName("訂單描述")]
		public string Status_Description{ get; set; }
		public string Status_Description_id { get; set; }
		public IEnumerable<Order_DetailsVM> Order_Details { get; set; }
		public IEnumerable<Order_Log>Order_Logs { get; set; }	
	}

	public static class OrderDTOExts
	{
		public static OrderVM ToVM(this OrderDTO source)
		{
			return new OrderVM
			{
				Order_id = source.Order_id,
				MemberName = source.MemberName,
				EmployeeAccount = source.EmployeeAccount,
				Total = source.Total,
				Payment = source.Payment,
				ShippedDate = source.ShippedDate,
				ShipVia = source.ShipVia,
				Freight = source.Freight,
				ShipName = source.ShipName,
				ShipAddress = source.ShipAddress,
				ShipPhone = source.ShipPhone,
				RequestRefundTime = source.RequestRefundTime,
				RequestRefund = source.RequestRefund,
				CreatedTime = source.CreatedTime,
				Status_id = source.Status_id,
				Status = source.Status,
				Status_Description = source.Status_Description,
				Order_Details= source.Order_Details.Select(x => x.ToVM()),
			};
		}
	}

	public static class OrderVMExts
	{
		public static OrderDTO ToDTO(this OrderVM source)
		{
			return new OrderDTO
			{
				Order_id = source.Order_id,
				EmployeeAccount = source.EmployeeAccount,
				Total = source.Total,
				Payment = source.Payment,
				ShippedDate = source.ShippedDate,
				ShipVia = source.ShipVia,
				Freight = source.Freight,
				ShipName = source.ShipName,
				ShipAddress = source.ShipAddress,
				ShipPhone = source.ShipPhone,
				RequestRefundTime = source.RequestRefundTime,
				RequestRefund = source.RequestRefund,
				CreatedTime = source.CreatedTime,
				Status_id = source.Status_id,
				Status = source.Status,
				Status_Description = source.Status_Description,				
			};
		}
	}


}