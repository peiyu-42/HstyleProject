using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class H_ActivityDto
	{
		public int H_Activity_Id { get; set; }

		public string Activity_Name { get; set; }

		public string Activity_Describe { get; set; }

		public int H_Value { get; set; }
	}

	public static class H_ActivityExts
	{
		public static H_ActivityDto ToDto(this H_Activities source)
		{
			return new H_ActivityDto
			{
				H_Activity_Id = source.H_Activity_Id,
				Activity_Name = source.Activity_Name,
				Activity_Describe = source.Activity_Describe,
				H_Value = source.H_Value
			};
		}
	}
}