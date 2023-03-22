using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CreateProductDto
	{
		public int ProductID { get; set; }
		public string Product_Name { get; set; }

		public int UnitPrice { get; set; }

		public string Description { get; set; }


		public DateTime Create_at { get; set; }

		public bool Discontinued { get; set; }
		public int DisplayOrder { get; set; }

		public int Category_Id { get; set; }

		public List<string> images { get; set; }

		public List<SpecDto> specs { get; set; }

		public List<string> tags { get; set; }
	}


	public static class CreateProductDtoExts
	{
		public static ProductVM ToCreateVM(this ProductDto source)
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

		public static ProductDto ToCreateDto(this ProductVM source)
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