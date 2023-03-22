using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class PCommentGetDTO
	{
		public int CommentId { get; set; }
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		public string ProductName { get; set; }
		public string ProductPhoto { get; set; }
		public string Color { get; set; }
		public string Size { get; set; }

	}
}
