using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class PCommentPostDTO
	{
		public int CommentId { get; set; }
		public string? CommentContent { get; set; }
		public int Score { get; set; }
		public DateTime CreatedTime { get; set; }
		public List<string>? PcommentImgs { get; set; }

		public List<IFormFile>? files { get; set; }
	}
}
