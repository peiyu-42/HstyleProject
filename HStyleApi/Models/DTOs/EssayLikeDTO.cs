using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class EssayLikeDTO
	{
		public int MemberId { get; set; }
		public int EssayId { get; set; }

		public virtual Essay Essay { get; set; }

	}

	public static class EssaylikeExts
	{
		public static Elike ToEssaylike(this EssayLikeDTO source)
		{
			return new Elike()
			{
				MemberId = source.MemberId,
				EssayId = source.EssayId
			};
		}
		public static EssayLikeDTO ToElikeDTO(this Elike source) //ToElikeDTO 重新命名 ToElikeDTO 從資料庫轉型到DTO 
		{
			return new EssayLikeDTO()
			{

				MemberId = source.MemberId,
				EssayId = source.EssayId,
			};
		}
	}
}
