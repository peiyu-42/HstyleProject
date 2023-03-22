using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class VideoViewDTO
	{
		public int VideoId { get; set; }
		public int? Views { get; set; }
		public virtual Video Video { get; set; }
	}
}
