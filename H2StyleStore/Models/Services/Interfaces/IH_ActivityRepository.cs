using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IH_ActivityRepository
	{
		IEnumerable<H_ActivityDto> GetHActivity();

		void Create(H_ActivityDto dto);

		H_ActivityDto GetHActivityById(int id);

		void Update(H_ActivityDto dto);

		H_ActivityDto FindActivity(int? id);

		void DeleteActivity(int id);

		/// <summary>
		/// 找出會員的打卡紀錄
		/// </summary>
		/// <param name="id"></param>
		/// <param name="activityId_checkIn"></param>
		/// <returns></returns>
		H_CheckInDto GetCheckInById(int id, int activityId_checkIn);

		/// <summary>
		/// 修改打卡紀錄
		/// </summary>
		/// <param name="model_CheckIn"></param>
		void EditCheckIn(int id, int checkInTimes);

		/// <summary>
		/// 新增單筆活動紀錄，自動新增紀錄
		/// </summary>
		/// <param name="dto"></param>
		void CreateHDetail(H_Source_DetailDto dto);
	}
}
