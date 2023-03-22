using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class ProductDto
	{
		public int Product_Id { get; set; }

		public string Product_Name { get; set; }

		public int UnitPrice { get; set; }

		public string Description { get; set; }

		public DateTime Create_at { get; set; }

		public bool Discontinued { get; set; }

		public int DisplayOrder { get; set; }

		public string PCategoryName { get; set; }

		public IEnumerable<string> imgs { get; set; }

		public IEnumerable<SpecDto> specs { get; set; }
		public IEnumerable<string> tags { get; set; }
	}

	public static class ProductExts
	{
		public static ProductDto ToDto(this Product source)
		=> new ProductDto
		{
			Product_Id = source.Product_Id,
			Product_Name = source.Product_Name,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.Create_at,
			Discontinued = source.Discontinued,
			DisplayOrder = source.DisplayOrder,
			PCategoryName = source.PCategory.ToDto(),
			imgs = source.Images.Select(x => x.ToPDto()),
			specs = source.Specs.Where(x => x.Discontinued == false).Select(x => x.ToDto()),
			tags = source.Tags.Select(x => x.ToPDto()),
			
		};
	}
}