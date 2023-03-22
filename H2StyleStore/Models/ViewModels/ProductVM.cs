using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class ProductVM
	{
		public int Product_Id { get; set; }

		[Display(Name = "商品名稱")]
		public string Product_Name { get; set; }

		[Display(Name = "單價")]
		public int UnitPrice { get; set; }

		[Display(Name = "商品描述")]
		public string Description { get; set; }


		[Display(Name = "創建時間")]
		public DateTime Create_at { get; set; }

		[Display(Name = "下架")]
		public bool Discontinued { get; set; }

		[Display(Name = "類別名稱")]
		public string PCategoryName { get; set; }

		[Display(Name = "照片")]
		public IEnumerable<string> images { get; set; }

		[Display(Name = "規格")]
		public List<SpecVm> specs { get; set; }

		[Display(Name = "標籤")]
		public List<string> tags { get; set; }
	}


	public static class ProductDtoExts
	{
		public static ProductVM ToVM(this ProductDto source)
		=> new ProductVM
		{
			Product_Id = source.Product_Id,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			PCategoryName = source.PCategoryName,
			images = source.imgs,
			specs = source.specs.Select(x => x.ToVM()).ToList(),
			tags = source.tags.ToList(),
		};

		public static ProductDto ToDto(this ProductVM source)
		=> new ProductDto
		{
			Product_Id = source.Product_Id,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			//DisplayOrder = source.DisplayOrder,
			PCategoryName = source.PCategoryName,
			//imgs = source.Images.Select(x => x.ToDto()),
			specs = source.specs.Select(x => x.ToDto()),
			tags = source.tags,

		};
	}
}