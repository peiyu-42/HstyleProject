using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class SpecDto
	{
		public int SpecId { get; set; }
		public string Color { get; set; }

		public string Size { get; set; }

		public int Stock { get; set; }

	}

	public static class SpecExts
	{
		public static SpecDto ToDto(this Spec source)
		=> new SpecDto
		{
			SpecId = source.Id,
			Color = source.Color,
			Size = source.Size,
			Stock = source.Stock,
		};
	}
}
