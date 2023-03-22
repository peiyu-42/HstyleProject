using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IH_Source_DetailRepository
	{
		/// <summary>
		/// 取得所有H幣活動資料
		/// </summary>
		/// <returns></returns>
		IEnumerable<H_Source_DetailDto> GetSource();

		/// <summary>
		/// 取得所有會員打卡資料
		/// </summary>
		/// <returns></returns>
		IEnumerable<H_CheckInDto> GetCheckIn();

		/// <summary>
		/// 取得單筆資料
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		H_Source_DetailDto FindDetail(int? id);

		/// <summary>
		/// 刪除單筆資料
		/// </summary>
		/// <param name="id"></param>
		void DeleteDetail(int id);

		/// <summary>
		/// 新增單筆活動紀錄，自動新增紀錄
		/// </summary>
		/// <param name="dto"></param>
		void CreateHDetail(H_Source_DetailDto dto);

		/// <summary>
		/// 新增單筆活動紀錄，自動新增紀錄
		/// </summary>
		/// <param name="dto"></param>
		void CreatNewDetail(CreateH_Source_DetailDto dto);

		/// <summary>
		/// 找出當月生日的會員
		/// </summary>
		/// <param name="birth">現在的日期</param>
		/// <returns></returns>
		IEnumerable<MemberInBirthDto> MemberInBirth(DateTime birth);

		/// <summary>
		/// 找出某個會員的活動紀錄
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IEnumerable<H_Source_DetailDto> GetTotalDetail(int id);

		/// <summary>
		/// 傳回活動選單
		/// </summary>
		/// <param name="activityId"></param>
		/// <returns></returns>
		IEnumerable<SelectListItem> GetActivities(int? activityId);

		/// <summary>
		/// 傳入所有 H_Value
		/// </summary>
		/// <param name="id">會員Id</param>
		/// <param name="total">總H_Value</param>
		void AddH_valueInMember(int id, int total);

		/// <summary>
		/// 取得單筆活動資料
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		CreateH_Source_DetailDto GetSourceById(int? id);

		/// <summary>
		/// 修改單筆活動紀錄
		/// </summary>
		/// <param name="dto"></param>
		void UpdateHDetail(CreateH_Source_DetailDto dto);

	}
}
