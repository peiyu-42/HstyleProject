using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class H_Source_DetailDto
	{
		public int Source_H_Id { get; set; }

		// from Member
		public string Member_Name{ get; set; }

		public int Member_Id { get; set; }

		public int Activity_Id { get; set; }

		// from Activity
		public string H_Activity_Name { get; set; }

		public int Difference_H { get; set; }

		public DateTime Event_Time { get; set; }

		public int? Total_H_SoFar { get; set; }

		[StringLength(500)]
		public string Remark { get; set; }

		public int? Employee_Id { get; set; }
	}

	public static class HSourceDetailExts
	{
		public static H_Source_DetailDto ToDto(this H_Source_Details source)
		{
			return new H_Source_DetailDto
			{
				Source_H_Id = source.Source_H_Id,
				Member_Id = source.Member_Id,
				Member_Name= source.Member.Name,
				Activity_Id = source.Activity_Id,
				H_Activity_Name = source.H_Activities.Activity_Name,
				Difference_H = source.Difference_H,
				Event_Time = source.Event_Time,
				Total_H_SoFar = source.Member.Total_H,
				Remark = source.Remark,
				Employee_Id = source.Employee_Id,
			};
		}
	}
}