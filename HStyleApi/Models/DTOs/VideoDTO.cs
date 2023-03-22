using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class VideoDTO
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string? Description { get; set; }

		public string FilePath { get; set; }

		public int CategoryId { get; set; }

		public int ImageId { get; set; }

		public DateTime? OnShelffTime { get; set; }

		public DateTime? OffShelffTime { get; set; }

		public bool? IsOnShelff { get; set; }

		public DateTime CreatedTime { get; set; }

		public string Image { get; set; }

		public IEnumerable<string> Tags { get; set; }

		public string TagName { get; set; }

		//public virtual ICollection<VideoLike> VideoLikes { get; set; }

		public virtual VideoView VideoView { get; set; }

		public string CategoryName { get; set; }

		public int Likes { get; set; }

		public int Views{ get; set; }

		public IEnumerable<int> MemberId { get; set; }
	}

	public static class VideoExts
	{
		private static readonly object _basePath = "https://localhost:44313";
		public static VideoDTO ToVideoDTO(this Video source)
		{
			
			return new VideoDTO()
			{
				Id = source.Id,
				Title = source.Title,
				Description = source.Description,
				FilePath =$"{_basePath}/Videos/{source.FilePath}",
				CategoryId = source.CategoryId,
				ImageId = source.ImageId,
				Image=$"{_basePath}/Images/VideoImages/{source.Image.Path}",
				OnShelffTime = source.OnShelffTime,
				OffShelffTime = source.OffShelffTime,
				CreatedTime = source.CreatedTime,
				Tags = source.Tags.Select(x => 
				x.TagName),
				//VideoLikes = source.VideoLikes,
				CategoryName = source.Category.CategoryName,
				Likes = source.VideoLikes.GroupBy(x => x.VideoId).Count(),
				Views = source.VideoView.Views,
				MemberId = source.VideoLikes.Select(x => x.MemberId),
				IsOnShelff= source.IsOnShelff
			};
		}
	}
}
