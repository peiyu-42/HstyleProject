using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace H2StyleStore.Models.ViewModels
{
	public class CreateVideoVM
	{
		public int Id { get; set; }

		[Display(Name = "影片名稱")]
		[Required(ErrorMessage = "請填寫影片名稱")]
		public string Title { get; set; }
		
		//public int CategoryId{ get; set; }

		[Display(Name = "影片說明")]
		public string Description { get; set; }

		[Display(Name = "檔案上傳")]
		//[Required(ErrorMessage = "請上傳影片檔案")]
		public string FilePath { get; set; }

		[Display(Name = "上架時間")]
		public DateTime? OnShelffTime { get; set; }

		[Display(Name = "下架時間")]
		public DateTime? OffShelffTime { get; set; }

		[Display(Name = "影片縮圖")]
		//[Required(ErrorMessage = "請上傳影片縮圖")]
		public string Image { get; set; }

		[Display(Name = "類別")]
		[Required]
		public int CategoryId{ get; set; }

		[Display(Name = "標籤")]
		[Required(ErrorMessage = "請輸入標籤")]
		public string Tags { get; set; }
	}

	public static class CreateVideoDtoExts
	{
		public static CreateVideoDto ToCreateDto(this CreateVideoVM source)
		{
			 return new CreateVideoDto
			{
				Id = source.Id,
				Title = source.Title,
				Description = source.Description,
				FilePath = source.FilePath,
				OnShelffTime = source.OnShelffTime,
				OffShelffTime = source.OffShelffTime,
				Image= source.Image,
				Tags=source.Tags.Split(',').Select(t=>t.Trim()).ToList(),
				CategoryId=source.CategoryId,			
			};
		}
		public static UpdateVideoDto ToEditDto(this EditVideoVM source)
		{
			return new UpdateVideoDto
			{
				Id = source.Id,
				Title = source.Title,
				Description = source.Description,
				FilePath = source.FilePath,
				OnShelffTime = source.OnShelffTime,
				OffShelffTime = source.OffShelffTime,
				Image = source.Image,
				CategoryId = source.CategoryId,
				Tags = source.Tags.Split(',').ToList(),
			};
		}
	}
}