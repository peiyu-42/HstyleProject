using HStyleApi.Models.EFModels;
using System.Xml.Linq;

namespace HStyleApi.Models.DTOs
{
	public class EssayCommentDTO
	{
		public int CommentId { get; set; }
		public int MemberId { get; set; }
		public string MemberName { get; set; }
		public int EssayId { get; set; }
		public string Ecomment { get; set; }
		public DateTime Etime { get; set; }
		public int Elike { get; set; }

		public virtual Essay Essay { get; set; }
		public virtual Member Member { get; set; }
		//public virtual ICollection<Ecommentlike> Ecommentlikes { get; set; }

		public virtual ICollection<Member> Members { get; set; }
	}
	public static class EssayCmmentExts
	{
		public static EssayCommentDTO ToCommentDTO(this EssaysComment source)
		{
			return new EssayCommentDTO
			{
				CommentId = source.CommentId,
				MemberId = source.MemberId,
				EssayId = source.EssayId,
				Ecomment = source.Ecomment,
				Etime = source.Etime,
				Elike = source.Elike,
				MemberName = source.Member.Name,

			};
		}
		public static EssaysComment ToComment(this EssayCommentDTO source)
		{
			return new EssaysComment()
			{
				CommentId = source.CommentId,
				MemberId = source.MemberId,
				EssayId = source.EssayId,
				Ecomment = source.Ecomment,
				Etime = source.Etime,
				Elike = source.Elike,
			};
		}
	}
}
