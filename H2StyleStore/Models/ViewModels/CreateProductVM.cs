using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CreateProductVM
	{
		public int ProductId { get; set; }

		[Display(Name = "商品名稱")]
		[Required]
		public string Product_Name { get; set; }

		[Display(Name = "單價")]
		[Required]
		public int UnitPrice { get; set; }

		[Display(Name = "商品描述")]
		[Required]
		public string Description { get; set; }


		[Display(Name = "創建時間")]
		public DateTime Create_at { get; set; }

		[Display(Name = "下架")]
		public bool Discontinued { get; set; }

		[Display(Name = "類別")]
		public int Category_Id { get; set; }
		[Display(Name = "排序")]
		public int DisplayOrder { get; set; }

		[Display(Name = "照片")]
		public List<string> images { get; set; }

		[Display(Name = "規格")]
		[Required(ErrorMessage ="規格至少一項")]
		public List<SpecVm> specs { get; set; }

		[Display(Name = "標籤")]
		public string tags { get; set; }
	}


	public static class CreateProductVMExts
	{

		public static CreateProductDto ToCreateDto(this CreateProductVM source)
		=> new CreateProductDto
		{
			ProductID = source.ProductId,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			DisplayOrder = source.DisplayOrder,
			Category_Id = source.Category_Id,
			images = source.images,
			specs = source.specs.Where(s => string.IsNullOrEmpty(s.Color) == false).Select(x => x.ToDto()).ToList(),
			tags = source.tags.Split(',').Select(x => x.Trim()).ToList(),

		};

		public static CreateProductDto ToCreateDto(this Product source)
		=> new CreateProductDto
		{
			ProductID = source.Product_Id,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			DisplayOrder = source.DisplayOrder,
			Category_Id = source.Category_Id,
			images = source.Images.Select(x => x.Path).ToList(),
			specs = source.Specs.Where(x => x.Discontinued == false).Select(x => x.ToDto()).ToList(),
			tags = source.Tags.Select(x => x.TagName).ToList(),

		};
		public static CreateProductVM ToCreateVM(this CreateProductDto source)
		=> new CreateProductVM
		{
			ProductId = source.ProductID,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			DisplayOrder = source.DisplayOrder,
			Category_Id = source.Category_Id,
			images = source.images,
			specs = source.specs.Select(x => x.ToVM()).ToList(),
			tags = string.Join(", ", source.tags),

		};


	}
}