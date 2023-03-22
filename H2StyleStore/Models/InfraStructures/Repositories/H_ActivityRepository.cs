using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class H_ActivityRepository : IH_ActivityRepository
	{
		private AppDbContext _db;
		public H_ActivityRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<H_ActivityDto> GetHActivity()
		{
			IEnumerable<H_Activities> activity = _db.H_Activities.OrderBy(x => x.H_Activity_Id).ToList();
			var data = activity.Select(a => a.ToDto());

			return data;
		}

		public H_ActivityDto GetHActivityById(int id)
		{
			var data = _db.H_Activities.Find(id).ToDto();
			return data;
		}
		public void Update(H_ActivityDto dto)
		{
			H_Activities activity = _db.H_Activities.Find(dto.H_Activity_Id);

			activity.Activity_Name = dto.Activity_Name;
			activity.Activity_Describe = dto.Activity_Describe;
			activity.H_Activity_Id = dto.H_Activity_Id;
			activity.H_Value = dto.H_Value;

			_db.SaveChanges();
		}

		public void Create(H_ActivityDto dto)
		{
			H_Activities activity = new H_Activities
			{
				Activity_Name = dto.Activity_Name,
				Activity_Describe = dto.Activity_Describe,
				H_Value = dto.H_Value,
			};
			_db.H_Activities.Add(activity);
			_db.SaveChanges();
		}

		public H_ActivityDto FindActivity(int? id)
		{
			if (id == null) return null;

			H_ActivityDto activity = _db.H_Activities.Find(id).ToDto();
			return activity;
		}

		public void DeleteActivity(int id)
		{
			H_Activities activity = _db.H_Activities.SingleOrDefault(a => a.H_Activity_Id == id);

			_db.H_Activities.Remove(activity);
			_db.SaveChanges();
		}

		public H_CheckInDto GetCheckInById(int id, int activityId_checkIn)
		{
			H_CheckInDto checkIn = _db.H_CheckIns.SingleOrDefault(c => c.Member_Id == id).ToDto();

			// 若沒有此會員的打卡紀錄，就新增一筆次數為0的打卡紀錄
			if (checkIn == null)
			{
				H_CheckIns data = new H_CheckIns
				{
					Member_Id = id,
					Activity_Id = activityId_checkIn,
					CheckIn_Times = 0,
					Last_Time = DateTime.Now,
				};
				_db.H_CheckIns.Add(data);
				_db.SaveChanges();

				return data.ToDto();
			}

			return checkIn;
		}

		public void EditCheckIn(int id, int checkInTimes)
		{
			// 找出原先紀錄
			H_CheckIns oldData = _db.H_CheckIns.SingleOrDefault(c => c.Member_Id == id);

			// 新增最新一筆紀錄
			oldData = new H_CheckIns
			{
				CheckIn_Times = checkInTimes,
				Last_Time = DateTime.Now,
			};
			_db.SaveChanges();
		}

		public void CreateHDetail(H_Source_DetailDto dto)
		{
			// 找出會員總Hcoin
			Member member = _db.Members.SingleOrDefault(m => m.Id == dto.Member_Id);
			member.Total_H += dto.Difference_H;
			_db.SaveChanges();

			H_Source_Details detail = new H_Source_Details
			{
				Member_Id = dto.Member_Id,
				Activity_Id = dto.Activity_Id,
				Difference_H = dto.Difference_H,
				Event_Time = dto.Event_Time,
				Total_H_SoFar = member.Total_H + dto.Difference_H,
			};
			_db.H_Source_Details.Add(detail);
			_db.SaveChanges();
		}
	}
}