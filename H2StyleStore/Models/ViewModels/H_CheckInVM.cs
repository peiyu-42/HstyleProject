using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class H_CheckInVM
	{
		[Key]
		public int CheckIn_H_Id { get; set; }

		[DisplayName("會員")]
		public string Member_Name { get; set; }

		[DisplayName("活動")]
		public string Activity { get; set; }

		[DisplayName("打卡次數")]
		public int CheckIn_Times { get; set; }

		[DisplayName("最後打卡時間")]
		public DateTime Last_Time { get; set; }
	}
	public static class HCheckInExts
	{
		public static H_CheckInVM ToVM(this H_CheckInDto dto)
		{
			return new H_CheckInVM
			{
				CheckIn_H_Id = dto.CheckIn_H_Id,
				Member_Name = dto.Member_Name,
				Activity = dto.Activity,
				CheckIn_Times = dto.CheckIn_Times,
				Last_Time = dto.Last_Time
			};
		}
	}
}