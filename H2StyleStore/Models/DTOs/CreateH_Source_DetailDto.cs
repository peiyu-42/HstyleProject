using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class CreateH_Source_DetailDto
	{
		public int Source_H_Id { get; set; }

		public int Member_Id { get; set; }

		public int Activity_Id { get; set; }

		public int Difference_H { get; set; }

		public DateTime Event_Time { get; set; }

		public int? Total_H_SoFar { get; set; }

		public string Remark { get; set; }

		public string Employee_Name { get; set; }
	}
	
	public static class CreateH_Source_DetailDtoExts
	{
		public static CreateH_Source_DetailVM ToVM(this CreateH_Source_DetailDto source)
		{
			return new CreateH_Source_DetailVM
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
		public static CreateH_Source_DetailDto ToEditDto(this H_Source_Details source)
		{
			return new CreateH_Source_DetailDto
			{
				Source_H_Id = source.Source_H_Id,
				Member_Id = source.Member_Id,
				Activity_Id = source.Activity_Id,
				Difference_H = source.Difference_H,
				//Event_Time= DateTime.Now,
				//Total_H_SoFar= source.Total_H_SoFar,
				Remark = source.Remark,
				//Employee_Name = source.Employee_Name,				
			};
		}
	}
}