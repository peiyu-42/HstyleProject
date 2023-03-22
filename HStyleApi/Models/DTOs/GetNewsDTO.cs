using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class GetNewsDTO
	{
		public string Etitle { get; set; }
		public IEnumerable<string> Imgs { get; set; }
		public string Title { get; set; }
		public string? Image { get; set; }
	}
}
