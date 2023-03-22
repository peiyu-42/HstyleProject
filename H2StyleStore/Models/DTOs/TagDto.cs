using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class TagDTO
	{
		public int Id { get; set; }

		public string TagName { get; set; }
	}
	public static class TagExts
	{
		public static string ToPDto(this Tag source)
		{
			return source.TagName;
		}

		public static TagDTO ToDto(this TagVM source)
		{
			return new TagDTO()
			{
				Id = source.Id,
				TagName = source.TagName
			};
		}
		public static TagDTO ToDto(this Tag source)
		{
			return new TagDTO()
			{
				Id = source.Id,
				TagName = source.TagName
			};
		}
	}
}