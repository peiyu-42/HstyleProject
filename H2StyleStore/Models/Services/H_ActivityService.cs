using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services.Interfaces;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{

	public class H_ActivityService
	{

		private readonly IH_ActivityRepository _repository;
		public H_ActivityService(IH_ActivityRepository repo)
		{
			//AppDbContext db = new AppDbContext();
			_repository = repo;
		}


		public IEnumerable<H_ActivityDto> GetHActivity()
		{
			return _repository.GetHActivity();
		}

		public string CreateNewActivity(H_ActivityDto dto)
		{
			try
			{
				_repository.Create(dto);

				return "新增成功";
			}
			catch (Exception ex) { return ex.Message; }
		}

		public void UpdateActivity(H_ActivityDto dto)
		{
			H_ActivityDto entity = _repository.GetHActivityById(dto.H_Activity_Id);
			if (entity == null) throw new Exception("找不到要修改的紀錄");

			entity.Activity_Name = dto.Activity_Name;
			entity.Activity_Describe = dto.Activity_Describe;
			entity.H_Value = dto.H_Value;

			_repository.Update(entity);
		}

		/// <summary>
		/// 註冊送H幣
		/// </summary>
		/// <param name="id">會員Id</param>
		public void HcoinRegister(int id)
		{
			int activityId_register = 1;

			int difference_H = _repository.FindActivity(activityId_register).H_Value;

			H_Source_DetailDto model = new H_Source_DetailDto
			{
				Member_Id = id,
				Activity_Id = activityId_register,
				Difference_H = difference_H,
				Event_Time = DateTime.Now,
			};
			_repository.CreateHDetail(model);

			// 修改H幣總額
			//TotalHcoin(id);
		}

		/// <summary>
		/// 購物fullPrice(滿額)送H幣
		/// 花費滿多少，就送H幣，訂單結案且已送貨後30天發放，若有人退訂就手動刪除紀錄
		/// </summary>
		/// <param name="id">會員Id</param>
		/// <param name="price">訂單總金額</param>
		public string HcoinOrderPrice(int id, int price)
		{
			// todo 需要修改規則，改為排程==>已改
			// 花費滿額
			int activityId_fullPrice = 4;
			// 得到的H幣
			int activityId_Hcoin = 5;
			int fullPrice = _repository.FindActivity(activityId_fullPrice).H_Value;
			int hCoin = _repository.FindActivity(activityId_Hcoin).H_Value;

			if (price < fullPrice) return null;

			H_Source_DetailDto model = new H_Source_DetailDto
			{
				Member_Id = id,
				Activity_Id = activityId_Hcoin,
				Difference_H = hCoin,
				Event_Time = DateTime.Now,
			};
			_repository.CreateHDetail(model);

			// 修改H幣總額
			//TotalHcoin(id);

			return "購物滿額送H幣: " + hCoin.ToString();
		}

		/// <summary>
		/// 打卡滿七天就送H幣，不必連續
		/// </summary>
		/// <param name="id">會員Id</param>
		/// <returns></returns>
		public string HcoinCheckIn(int id)
		{
			// 活動Id和項目
			int activityId_checkIn = 3;
			var activity = _repository.FindActivity(activityId_checkIn);

			// 找出member打卡紀錄
			var memberCheckIn = _repository.GetCheckInById(id, activityId_checkIn);

			// 打卡次數加一
			int checkInTimes = memberCheckIn.CheckIn_Times + 1;

			// 若打卡次數為七，則增加活動紀錄(Detail)，並將次數改為0
			if (checkInTimes < 0 || checkInTimes > 7)
			{
				return "打卡紀錄有錯誤";
			}
			else if (checkInTimes == 7)
			{
				H_Source_DetailDto model_Detail = new H_Source_DetailDto
				{
					Member_Id = id,
					Activity_Id = activityId_checkIn,
					Difference_H = activity.H_Value,
					Event_Time = DateTime.Now,
				};
				_repository.CreateHDetail(model_Detail);

				checkInTimes = 0;
			}

			// 修改打卡紀錄
			_repository.EditCheckIn(id, checkInTimes);

			return "打卡成功";
		}

		///// <summary>
		///// 計算所有 H coin
		///// </summary>
		///// <param name="id"></param>
		//public void TotalHcoin(int id)
		//{
		//	// 找出會員所有活動的紀錄
		//	var detail = _sourceDetailRepository.GetTotalDetail(id);

		//	// 計算H幣總額
		//	int total = 0;
		//	foreach (var item in detail)
		//	{
		//		total += item.Difference_H;
		//	}

		//	// 修改Member的Total_H
		//	_sourceDetailRepository.AddH_valueInMember(id, total);
		//}

	}
}