using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class CategoryDTO
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }

		public string CategoryDescription { get; set; }

	}

	public static class CategoryExts
	{
		public static string ToDto(this VideoCategory source)
		//=> new PCategoryDto
		{
			return source.CategoryName;
		
		}//;
	}
}