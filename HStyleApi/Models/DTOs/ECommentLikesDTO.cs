using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class ECommentLikesDTO
	{
			public int MemberId { get; set; }
			public int CommentId { get; set; }
	}
	public static class ECommentLikeExts
	{
		public static ECommentLikesDTO ToECommentLikesDTO(this EcommentsLike source)
		{
			return new ECommentLikesDTO()
			{
				MemberId = source.MemberId,
				CommentId = source.CommentId
			};
		}
	}
}
