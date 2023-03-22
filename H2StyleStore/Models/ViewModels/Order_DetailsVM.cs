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
	public class Order_DetailsVM
	{
		[DisplayName("訂單編號")]
		public int Order_id { get; set; }

		[DisplayName("商品編號")]
		public int Product_id { get; set; }

		[Required]
		[StringLength(50)]
		[DisplayName("商品名稱")]
		public string ProductName { get; set; }

		[DisplayName("單價")]
		public int UnitPrice { get; set; }

		[DisplayName("小計")]
		public int SubTotal { get; set; }

		[DisplayName("數量")]
		public int Quantity { get; set; }

		[DisplayName("折扣")]
		public int? Discount { get; set; }
	}
	public static class Order_DetailDTOExts
	{
		public static Order_DetailsVM ToVM(this Order_DetailDTO source)
		{
			return new Order_DetailsVM
			{
				Order_id = source.Order_id,
				Product_id = source.Product_id,
				ProductName = source.ProductName,	
				UnitPrice = source.UnitPrice,
				SubTotal = source.SubTotal,
				Quantity = source.Quantity, 
				Discount = source.Discount,
			};
		}
	}
}