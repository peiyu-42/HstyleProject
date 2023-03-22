using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class VideoCategoryDto
	{
		public int Id { get; set; }
		
		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }
	}

	public static class VideoCategoryExts
	{
		public static VideoCategoryDto ToDto(this VideoCategory source)
		{
			return new VideoCategoryDto()
			{
				Id = source.Id, 
				CategoryName = source.CategoryName,
				CategoryDescription = source.CategoryDescription
			};
		}

		public static VideoCategoryDto ToDto(this VideoCategoryVM source)
		{
			return new VideoCategoryDto()
			{
				Id = source.Id,
				CategoryName = source.CategoryName,
				CategoryDescription = source.CategoryDescription
			};
		}
	}
}