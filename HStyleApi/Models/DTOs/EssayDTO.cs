using HStyleApi.Models.EFModels;
using System.Drawing;

namespace HStyleApi.Models.DTOs
{
	public class EssayDTO
	{
		public int EssayId { get; set; }
		public int InfluencerId { get; set; }
		public string InfluencerName { get; set; }
		public DateTime UplodTime { get; set; }
		public string Etitle { get; set; }
		public string Econtent { get; set; }
		public DateTime? UpLoad { get; set; }
		public DateTime? Removed { get; set; }
		public int CategoryId { get; set; }
		
		public DateTime? Pon { get; set; }

		public string CategoryName { get; set; }
		public IEnumerable<string> Imgs { get; set; }
		public IEnumerable<string> Tags { get; set; }
		public int Like { get; set; }

		public IEnumerable<int> MemberId { get; set; }

		//public virtual VideoCategory Category { get; set; }
		//public virtual Employee Influencer { get; set; }
		//public virtual ICollection<EassyFollow> EassyFollows { get; set; }
		//public virtual ICollection<EssaysComment> EssaysComments { get; set; }

		//public virtual ICollection<Image> Imgs { get; set; }
		//public virtual ICollection<Member> Members { get; set; }
		//public virtual ICollection<Tag> Tags { get; set; }


	}
	public static class EssayExts
	{
		private static readonly object _basePath = "https://localhost:44313/Images";
		public static EssayDTO ToEssayDTO(this Essay source)
		{
			return new EssayDTO()
			{
				EssayId = source.EssayId,
				InfluencerId = source.InfluencerId,
				InfluencerName = source.Influencer.Account,
				UpLoad = source.UpLoad,
				Etitle = source.Etitle,
				Econtent = source.Econtent,
				UplodTime = source.UplodTime,
				Removed = source.Removed,
				CategoryId = source.CategoryId,
				//Imgs = source.Imgs.Select(x => x.Path),

				Imgs = source.Imgs.Select(x => $"{_basePath}/Essaysimage/{x.Path}"),
				Tags = source.Tags.Select(x => x.TagName),
				Like = source.Elikes.GroupBy(x => x.EssayId).Count(),
				CategoryName = source.Category.CategoryName,
				MemberId = source.Elikes.Select(x => x.MemberId),
			
			};
		}
	}
}
