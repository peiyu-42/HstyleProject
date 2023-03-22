using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace H2StyleStore.Models.ViewModels
{
	public class CreateEssayVM
	{
		[Key]
		public int Essay_Id { get; set; }

		// todo 登入自動輸入
		[Display(Name = "員工賬號")]
		public string Influencer_Name { get; set; }

		[Required]
		[StringLength(500)]
		[Display(Name = "標題名稱")]
		public string ETitle { get; set; }

		[Required]
		[Display(Name = "摘要")] // 修改Html加入...
		public string EContent { get; set; }

		// 下拉
		[Required]
		[Display(Name = "類別名稱")]
		public int CategoryId { get; set; }

		[Required]
		[Display(Name = "發佈時間")]
		[Column(TypeName = "datetime2")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime UpLoad { get; set; }

		[Required]
		[Display(Name = "下架時間")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime Removed { get; set; }

		[Display(Name = "圖片")]
		public List<string> Images { get; set; }

		[Required]
		[Display(Name = "標籤")]
		public string Tags { get; set; }
	}

	public static class CreateEssayDTOExts
	{
		public static CreateEssayDTO ToCreateDTO(this CreateEssayVM source)
		{
			return new CreateEssayDTO
			{
				Essay_Id = source.Essay_Id,
				Influencer_Name = source.Influencer_Name,
				ETitle = source.ETitle,
				EContent = source.EContent,
				UpLoad = source.UpLoad,
				Removed = source.Removed,
				CategoryId = source.CategoryId,
				Images = source.Images,
				Tags = source.Tags.Split(',').Select(x => x.Trim()).ToList(),
			};
		}
		public static CreateEssayDTO ToCreateDTO(this Essay source)
		=> new CreateEssayDTO
		{
			Essay_Id = source.Essay_Id,
			Influencer_Id = source.Influencer_Id,
			ETitle = source.ETitle,
			EContent = source.EContent,
			UpLoad = source.UpLoad.Value,
			Removed = source.Removed.Value,
			CategoryId = source.CategoryId,
			Images = source.Images.Select(x => x.Path).ToList(),
			Tags = source.Tags.Select(x => x.TagName).ToList(),

		};
		public static CreateEssayVM ToCreateVM(this CreateEssayDTO source)
			=> new CreateEssayVM
			{
				Essay_Id = source.Essay_Id,
				Influencer_Name = source.Influencer_Name,
				ETitle = source.ETitle,
				EContent = source.EContent,
				UpLoad = source.UpLoad,
				Removed = source.Removed,
				CategoryId = source.CategoryId,
				Images = source.Images,
				Tags = string.Join(",", source.Tags),
			};
	}
}