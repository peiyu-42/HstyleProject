using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HStyleApi.Models.InfraStructures.Repositories
{
	public class HCoinRepository
	{
		private AppDbContext _db;
		public HCoinRepository(AppDbContext db) { _db = db; }

		public async Task<IEnumerable<HCheckInDTO>> GetHCheckIn(int id)
		{
			var data = await _db.HCheckIns.Where(c => c.MemberId == id)
				.Select(c => c.ToHCheckInDTO()).ToListAsync();

			if (data == null)
			{
				throw new Exception();
			}

			return data;
		}

		public HActivityDTO FindActivityById(int? activityId)
		{
			if (activityId == null) throw new Exception("找不到此活動紀錄");

			var activity = _db.HActivities.SingleOrDefault(a => a.HActivityId == activityId);
			if (activity == null) { throw new Exception("找不到此活動紀錄"); }

			return activity.ToDTO();
		}

		public async Task<HCheckInDTO> GetCheckInByMemberId(int memberId, int activityId_checkIn)
		{
			HCheckIn? checkIn = _db.HCheckIns.SingleOrDefault(c => c.MemberId == memberId);

			// 若沒有此會員的打卡紀錄，就新增一筆次數為0的打卡紀錄
			if (checkIn == null)
			{
				checkIn = new HCheckIn
				{
					MemberId = memberId,
					ActivityId = activityId_checkIn,
					CheckInTimes = 0,
					LastTime = DateTime.Now.AddDays(-1),
				};
				_db.HCheckIns.Add(checkIn);
				await _db.SaveChangesAsync();
			}

			return checkIn.ToHCheckInDTO();
		}

		public void CreateHDetail(HSourceDetailDTO dto)
		{
			// 找出會員總Hcoin
			Member? member = _db.Members.SingleOrDefault(m => m.Id == dto.MemberId);
			if (member == null) throw new Exception("找不到此會員的紀錄");
			// 將活動的Coin加入會員的Total_H裡
			member.TotalH += dto.DifferenceH;
			_db.SaveChanges();

			HSourceDetail detail = new HSourceDetail
			{
				MemberId = dto.MemberId,
				ActivityId = dto.ActivityId,
				DifferenceH = dto.DifferenceH,
				EventTime = dto.EventTime,
				TotalHSoFar = member.TotalH + dto.DifferenceH,
			};
			_db.HSourceDetails.Add(detail);
			_db.SaveChanges();
		}

		public void EditCheckInById(int memberId, int checkInTimes)
		{
			// 找出原先紀錄
			HCheckIn? oldData = _db.HCheckIns.SingleOrDefault(c => c.MemberId == memberId);
			if (oldData == null) throw new Exception("找不到此會員的紀錄");

			// 修改此會員的打卡次數
			oldData.CheckInTimes = checkInTimes;
			oldData.LastTime = DateTime.Now;
			_db.SaveChanges();
		}

		// 取得花費活動的規則
		public async Task<IEnumerable<HActivityDTO>> GetCostHCoinRule()
		{
			// 滿{多少}金額
			int hActivityFullId = 6;
			// 可花費的上限，滿額的{多少}%
			int hActivityCostId = 7;
			IEnumerable<HActivity> hActivity = _db.HActivities.Where(a => a.HActivityId == hActivityFullId || a.HActivityId == hActivityCostId);

			return hActivity.Select(a => a.ToDTO());
		}

		// 取得會員的總HCoin金額
		public async Task<int> GetTotalHCoinByMemberId(int memberId)
		{
			Member member = _db.Members.SingleOrDefault(m => m.Id == memberId);
			if (member == null) throw new Exception("找不到此會員");
			int totalH = member.TotalH == null ? 0 : (int)member.TotalH;
			return  totalH;
		}

		// 取得會員的總HCoin金額，同步
		public int GetTotalHByMemberId(int memberId)
		{
			Member member =  _db.Members.SingleOrDefault(m => m.Id == memberId);
			if (member == null) throw new Exception("找不到此會員");
			return member.TotalH == null ? 0 : (int)member.TotalH;
		}

		/// <summary>
		/// 將會員花費的HCoin記錄到會員的HCoin活動中
		/// </summary>
		/// <param name="memberId">會員Id</param>
		/// <param name="value">花費的HCoin</param>
		public void PostCostHCoinByMemberId(int memberId, int value)
		{
			int costValue = value * -1;
			// 找出會員目前的總金額，並更改花費過後的總金額
			Member member = _db.Members.SingleOrDefault(m => m.Id == memberId);
			if (member == null) throw new Exception("找不到此會員");
			member.TotalH += costValue;
			_db.SaveChanges();

			// 可花費活動的Id
			int hActivityCostId = 7;
			// 新增一筆活動紀錄
			HSourceDetail detail = new HSourceDetail
			{
				MemberId = memberId,
				ActivityId = hActivityCostId,
				DifferenceH = costValue,
				EventTime = DateTime.Now,
				TotalHSoFar = member.TotalH,
			};
			_db.HSourceDetails.Add(detail);
			_db.SaveChanges();
		}
	}
}
