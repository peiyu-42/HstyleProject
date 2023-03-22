using H2StyleStore.Models.DTOs;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class H_ActivityVM
	{
		[Key]
		public int H_Activity_Id { get; set; }

		[Required]
		[StringLength(50)]
		[DisplayName("活動名稱")]
		public string Activity_Name { get; set; }

		[Required]
		[DisplayName("活動描述")]
		public string Activity_Describe { get; set; }

		[DisplayName("H幣值")]
		public int H_Value { get; set; }
	}

	public static class H_ActiviyDtoExts
	{
		public static H_ActivityVM ToVM(this H_ActivityDto dto)
		{
			return new H_ActivityVM
			{
				H_Activity_Id = dto.H_Activity_Id,
				Activity_Name = dto.Activity_Name,
				Activity_Describe = dto.Activity_Describe,
				H_Value = dto.H_Value
			};
		}

		public static H_ActivityDto ToDto(this H_ActivityVM vm)
		{
			return new H_ActivityDto
			{
				H_Activity_Id = vm.H_Activity_Id,
				Activity_Name = vm.Activity_Name,
				Activity_Describe = vm.Activity_Describe,
				H_Value = vm.H_Value
			};
		}
	}
}