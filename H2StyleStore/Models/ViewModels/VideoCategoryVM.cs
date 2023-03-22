using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class VideoCategoryVM
	{
		public int Id { get; set; }

		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }
	}

	public static class VideoCategoryExts
	{
		public static VideoCategoryVM ToVM(this VideoCategoryDto source)
		{
			return new VideoCategoryVM()
			{
				Id=source.Id,
				CategoryName=source.CategoryName,
				CategoryDescription=source.CategoryDescription
			};
		}
	}
}