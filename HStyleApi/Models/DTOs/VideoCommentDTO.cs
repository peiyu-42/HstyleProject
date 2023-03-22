using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class VideoCommentDTO
	{
		public int Id { get; set; }
		public int VideoId { get; set; }
		public int MemberId { get; set; }
		public string MemberName{ get; set; }
		public string Comment { get; set; }
		public DateTime CreatedTime { get; set; }
		public int Like { get; set; }

		public virtual Member Member { get; set; }
		public virtual Video Video { get; set; }
		public virtual ICollection<VcommentLike> VcommentLikes { get; set; }
	}
	public static class VideoCommentExts
	{
		public static VideoCommentDTO ToCommentDTO(this VideoComment source)
		{
			return new VideoCommentDTO()
			{
				Id= source.Id,
				VideoId= source.VideoId,
				MemberId= source.MemberId,
				Comment= source.Comment,
				CreatedTime= source.CreatedTime,
				Like= source.Like,
				MemberName=source.Member.Name
			};
		}

		public static VideoComment ToComment(this VideoCommentDTO source)
		{
			return new VideoComment()
			{
				Id= source.Id,
				VideoId= source.VideoId,
				MemberId= source.MemberId,
				Comment= source.Comment,
				CreatedTime= source.CreatedTime,
				Like= source.Like,
			};
		}
	}
}
