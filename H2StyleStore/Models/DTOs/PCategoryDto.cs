using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class PCategoryDto
	{
		public int PCategory_Id { get; set; }

		public string PCategoryName { get; set; }

		public string PCategory_Description { get; set; }
	}

	public static class PCategoryExts
	{
		public static string ToDto(this PCategory source)
		//=> new PCategoryDto
		{
			return source.PCategoryName;
			//PCategory_Id = source.PCategory_Id,
			//PCategoryName = source.PCategoryName,
			//PCategory_Description = source.PCategory_Description,
		}//;
	}
}