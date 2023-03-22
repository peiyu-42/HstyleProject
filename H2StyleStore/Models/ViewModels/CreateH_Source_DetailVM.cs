using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CreateH_Source_DetailVM
	{
		[Key]
		public int Source_H_Id { get; set; }

		[DisplayName("會員Id")]
		public int Member_Id { get; set; }

		[DisplayName("活動Id")]
		public int Activity_Id { get; set; }

		[DisplayName("H幣的值")]
		public int Difference_H { get; set; }		

		[StringLength(500)]
		[DisplayName("活動描述")]
		public string Remark { get; set; }

		[DisplayName("員工Id")]
		public string Employee_Name { get; set; }		
	}

	public static class CreateH_Source_DetailVMExts
	{
		public static CreateH_Source_DetailDto ToDto(this CreateH_Source_DetailVM source)
		{
			return new CreateH_Source_DetailDto
			{
				Source_H_Id = source.Source_H_Id,
				Member_Id = source.Member_Id,
				Activity_Id = source.Activity_Id,
				Difference_H = source.Difference_H,
				//Event_Time= source.Event_Time,
				//Total_H_SoFar= source.Total_H_SoFar,
				Remark = source.Remark,
				Employee_Name = source.Employee_Name
			};
		}
	}
}