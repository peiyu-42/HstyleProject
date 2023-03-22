using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class HelpfulDTO
	{
		public int MemberId { get; set; }
		public int CommentId { get; set; }
	}

	public static class HelpfulExt
	{
		public static HelpfulDTO ToDto(this PcommentsHelpful source)
		=> new HelpfulDTO
		{
			MemberId = source.MemberId,
			CommentId = source.CommentId,
		};
	}
}
