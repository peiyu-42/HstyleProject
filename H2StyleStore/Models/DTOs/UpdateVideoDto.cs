using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace H2StyleStore.Models.DTOs
{
	public class UpdateVideoDto
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string FilePath { get; set; }

		public int CategoryId { get; set; }

		public int ImageId { get; set; }

		public DateTime? OnShelffTime { get; set; }

		public DateTime? OffShelffTime { get; set; }

		public DateTime CreatedTime { get; set; }

		public bool IsOnShelff { get; set; }

		public string Image { get; set; }

		public string VideoCategory { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public List<string> Tags { get; set; }
	}
	public static class UpdateVideoExts
	{
		public static UpdateVideoDto ToEditDto(this Video source)
		{
			return new UpdateVideoDto()
			{
				Id = source.Id,
				Title = source.Title,
				Description = source.Description,
				FilePath = source.FilePath,
				CategoryId = source.CategoryId,
				Image = source.Image.Path,
				OnShelffTime = source.OnShelffTime,
				OffShelffTime = source.OffShelffTime,
				IsOnShelff=source.IsOnShelff,
				//CreatedTime = source.CreatedTime,
				Tags = source.Tags.Select(t => t.TagName).ToList()
			};
		}
		public static EditVideoVM ToEditVM(this UpdateVideoDto source)
		{
			return new EditVideoVM()
			{
				Id = source.Id,
				Title = source.Title,
				Description = source.Description,
				FilePath = source.FilePath,
				CategoryId = source.CategoryId,
				Image = source.Image,
				OnShelffTime = source.OnShelffTime,
				OffShelffTime = source.OffShelffTime,
				//CreatedTime = source.CreatedTime,
				Tags = string.Join(",",source.Tags.ToArray()),
			};
		}
	}
}