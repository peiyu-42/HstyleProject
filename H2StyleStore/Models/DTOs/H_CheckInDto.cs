using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class H_CheckInDto
	{
		public int CheckIn_H_Id { get; set; }
		public string Member_Name { get; set; }

		public int Member_Id { get; set; }

		public int Activity_Id { get; set; }

		public string Activity { get; set; }

		public int CheckIn_Times { get; set; }

		public DateTime Last_Time { get; set; }
	}

	public static class HCheckInExts
	{
		public static H_CheckInDto ToDto(this H_CheckIns source)
		{
			return new H_CheckInDto
			{
				CheckIn_H_Id = source.CheckIn_H_Id,
				Member_Name = source.Member.Name,
				Member_Id = source.Member_Id,
				Activity_Id = source.Activity_Id,
				Activity = source.H_Activities.Activity_Name,
				CheckIn_Times = source.CheckIn_Times,
				Last_Time = source.Last_Time
			};
		}
	}
		
}