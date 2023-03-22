using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class VCommentLikeDTO
	{
		public int MemberId { get; set; }
		public int CommentId { get; set; }

		public virtual VideoComment Comment { get; set; }
	}
	public static class VCommentLikeExts
	{
		public static VCommentLikeDTO ToVCommentLikeDTO(this VcommentLike source)
		{
			return new VCommentLikeDTO()
			{
				MemberId = source.MemberId,
				CommentId=source.CommentId
			};
		}
	}
}
