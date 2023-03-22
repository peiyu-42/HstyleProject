using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
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

		public IEnumerable<string> Imgs { get; set; }

		public IEnumerable<SpecDto> Specs { get; set; }
		public IEnumerable<string> Tags { get; set; }
	}

	public static class ProductExts
	{
		private static readonly object _basePath = "https://localhost:44313/Images";

		public static ProductDto ToDto(this Product source)
		=> new ProductDto
		{
			Product_Id = source.ProductId,
			Product_Name = source.ProductName,
			UnitPrice = source.UnitPrice,
			Description = source.Description,
			Create_at = source.CreateAt,
			Discontinued = source.Discontinued,
			DisplayOrder = source.DisplayOrder,
			PCategoryName = source.Category.PcategoryName,
			Imgs = source.Imgs.Select(x => $"{_basePath}/ProductImages/{x.Path}"),
			Specs = source.Specs.Where(x => x.Discontinued == false).Select(x => x.ToDto()),
			Tags = source.Tags.Select(x => x.TagName),

		};
	}
}
