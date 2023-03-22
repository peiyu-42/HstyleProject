using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class VideoLikeDTO
	{
		public string Title { get; set; }
		public bool IsOnShelff  { get; set; }

		public int VideoId{ get; set; }

		public int MemberId{ get; set; }

		public string CategoryName { get; set; }

		public int Likes { get; set; }

		public int? Views { get; set; }

		public IEnumerable<string> Tags { get; set; }

		public string? Image { get; set; }
	}
	public static class VideoLikeExts
	{
		private static readonly object _basePath = "https://localhost:44313";
		public static VideoLike ToVideoLike(this VideoLikeDTO source)
		{
			return new VideoLike()
			{
				VideoId = source.VideoId,
				MemberId = source.MemberId,
				
			};
		}
		public static VideoLikeDTO ToLikeDTO(this VideoLike source)
		{
			return new VideoLikeDTO()
			{
				VideoId = source.VideoId,
				MemberId = source.MemberId,
				Image = $"{_basePath}/Images/VideoImages/{source.Video.Image.Path}",
				//Image =source.Video.Image,
				Views=source.Video.VideoView.Views,
				Likes=source.Video.VideoLikes.GroupBy(v=>v.VideoId).Count(),
				CategoryName = source.Video.Category.CategoryName,
				Title=source.Video.Title,
				IsOnShelff=source.Video.IsOnShelff ?? true,
			};
		}
	}
}
