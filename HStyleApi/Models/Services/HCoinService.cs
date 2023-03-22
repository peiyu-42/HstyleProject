using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Core.Types;

namespace HStyleApi.Models.Services
{
	public class HCoinService
	{
		private HCoinRepository _hCoinRepo;

		public HCoinService(AppDbContext db)
		{
			_hCoinRepo = new HCoinRepository(db);
		}

		// 取得打卡紀錄
		public async Task<HCheckInDTO> GetHCheckIn(int id, int activityId_checkIn)
		{
			return await _hCoinRepo.GetCheckInByMemberId(id, activityId_checkIn);
		}

		// 修改打卡紀錄
		public async Task<string> PutCheckInById(int memberId)
		{
			// 活動Id和項目
			int activityId_checkIn = 3;
			var activity = _hCoinRepo.FindActivityById(activityId_checkIn);

			// 找出member打卡紀錄
			var memberCheckIn = await _hCoinRepo.GetCheckInByMemberId(memberId, activityId_checkIn);

			// 打卡次數加一
			var checkInTimes = memberCheckIn.CheckInTimes + 1;

			// 如果今天以打卡過就不可再打卡
			if (memberCheckIn.LastTime.Date == DateTime.Today)
			{
				return "今天已經打過卡囉!";
			}

			// 若打卡次數為七，則增加活動紀錄(Detail)，並將次數改為0
			if (checkInTimes < 0 || checkInTimes > 7)
			{
				return "打卡紀錄有錯誤";
			}
			else if (checkInTimes == 7)
			{
				HSourceDetailDTO model_Detail = new HSourceDetailDTO
				{
					MemberId = memberId,
					ActivityId = activityId_checkIn,
					DifferenceH = activity.HValue,
					EventTime = DateTime.Now,
				};
				_hCoinRepo.CreateHDetail(model_Detail);
				checkInTimes = 0;
			}

			// 修改打卡紀錄
			_hCoinRepo.EditCheckInById(memberId, checkInTimes);

			return "打卡成功";
		}

		// 取得花費活動的規則
		public async Task<IEnumerable<HActivityDTO>> GetCostHCoinRule()
		{
			// 滿{多少}金額；可花費的上限，滿額的{多少}%
			return await _hCoinRepo.GetCostHCoinRule();
		}

		// 取得會員的總HCoin金額
		public async Task<int> GetTotalHCoinByMemberId(int memberId)
		{
			return await _hCoinRepo.GetTotalHCoinByMemberId(memberId);
		}

		// 將會員花費的HCoin記錄到會員的活動中		
		public void PostCostHCoinByMemberId(int memberId, int value)
		{
			if (value < 0) { throw new Exception("請傳回正數的值"); }
			if (value == 0) { return; }

			// 取得會員的總HCoin金額
			int memberTotalHCoin = _hCoinRepo.GetTotalHByMemberId(memberId);
			if (memberTotalHCoin < value) { throw new Exception("沒有足夠的H幣可以扣除"); }

			_hCoinRepo.PostCostHCoinByMemberId(memberId, value);
		}
	}
}
