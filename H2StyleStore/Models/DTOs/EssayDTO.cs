using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class EssayDTO
	{
		[Key]
		public int Essay_Id { get; set; }

		public int Influencer_Id { get; set; }

		public string Influencer_Name { get; set; }

		public DateTime UplodTime { get; set; }

		[Required]
		[StringLength(1000)]
		public string ETitle { get; set; }

		[Required]
		[StringLength(1000)]
		public string EContent { get; set; }

		public int CategoryId { get; set; }

		public string CategoryName { get; set; }

		public DateTime UpLoad { get; set; }

		public DateTime Removed { get; set; }

		public IEnumerable<string> images { get; set; }

		public IEnumerable<string> Tags { get; set; }

		public bool? PON { get; set; }
	}


	public static class EssayDtoExts
	{
		public static EssayDTO ToDto(this Essay source)
		=> new EssayDTO
		{
			Essay_Id = source.Essay_Id,
			Influencer_Id = source.Influencer_Id,
			Influencer_Name = source.Employee.Account,
			UplodTime = source.UplodTime,
			ETitle = source.ETitle,
			EContent = source.EContent,
			CategoryId = source.CategoryId,
			CategoryName = source.VideoCategory.CategoryName,
			UpLoad = source.UpLoad.Value,
			Removed = source.Removed.Value,
			images = source.Images.Select(i => i.ToPDto()),
			Tags = source.Tags.Select(i => i.ToPDto()),
			PON = source.PON
		};

		public static Essay ToDb(this EssayDTO source)
		=> new Essay
		{
			Essay_Id = source.Essay_Id,
			Influencer_Id = source.Influencer_Id,
			UplodTime = source.UplodTime,
			ETitle = source.ETitle,
			EContent = source.EContent,
			UpLoad = source.UpLoad,
			Removed = source.Removed,
		};


	}
}