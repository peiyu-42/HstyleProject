using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class H_Source_DetailRepository : IH_Source_DetailRepository
	{
		private AppDbContext _db;
		//private H_ActivityService activityService;

		public H_Source_DetailRepository(AppDbContext db)
		{
			_db = db;
			//IH_ActivityRepository ac_repo = new H_ActivityRepository(_db);
			//activityService = new H_ActivityService(ac_repo);
		}

		public IEnumerable<H_Source_DetailDto> GetSource()
		{
			IEnumerable<H_Source_Details> detail = _db.H_Source_Details
				.OrderBy(d => d.Source_H_Id)
				.ToList();
			var data = detail.Select(d => d.ToDto());

			return data;
		}

		public IEnumerable<H_CheckInDto> GetCheckIn()
		{
			IEnumerable<H_CheckIns> checkIns = _db.H_CheckIns
				.OrderBy(c => c.Last_Time)
				.ToList();
			var data = checkIns.Select(d => d.ToDto());

			return data;
		}

		public H_Source_DetailDto FindDetail(int? id)
		{
			if (id == null) return null;

			H_Source_DetailDto activity = _db.H_Source_Details.Find(id).ToDto();
			return activity;
		}

		public void DeleteDetail(int id)
		{
			H_Source_Details detail = _db.H_Source_Details.SingleOrDefault(a => a.Source_H_Id == id);

			_db.H_Source_Details.Remove(detail);
			_db.SaveChanges();
		}

		public void CreateHDetail(H_Source_DetailDto dto)
		{
			Member member = _db.Members.SingleOrDefault(m => m.Id == dto.Member_Id);

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

			member.Total_H = detail.Total_H_SoFar;
			_db.SaveChanges();
		}

		/// <summary>
		/// 新增單筆活動紀錄，手動新增
		/// </summary>
		/// <param name="dto"></param>
		public void CreatNewDetail(CreateH_Source_DetailDto dto)
		{
			// 計算Hcoin總和，再記錄在Total_H_SorFar中
			//activityService.TotalHcoin(dto.Member_Id);
			H_Source_Details data = new H_Source_Details()
			{
				Member_Id = dto.Member_Id,
				Activity_Id = dto.Activity_Id,
				Difference_H = dto.Difference_H,
				Event_Time = DateTime.Now,
				Total_H_SoFar = _db.Members.Find(dto.Member_Id).Total_H + dto.Total_H_SoFar,
				Remark = dto.Remark,
				Employee_Id = _db.Employees.Where(x => x.Account == dto.Employee_Name).FirstOrDefault().Employee_id
			};

			_db.H_Source_Details.Add(data);
			_db.SaveChanges();
		}

		public IEnumerable<MemberInBirthDto> MemberInBirth(DateTime birth)
		{
			var member = _db.Members
				.Where(b => b.Birthday.Month == birth.Month).ToList();

			return member.Select(m => m.ToBirthDto());
		}

		public IEnumerable<H_Source_DetailDto> GetTotalDetail(int id)
		{
			var detail = _db.H_Source_Details
				.Where(d => d.Member_Id == id).ToList();

			return detail.Select(d => d.ToDto());
		}

		public IEnumerable<SelectListItem> GetActivities(int? activityId)
		{
			var items = _db.H_Activities
				.Select(a => new SelectListItem
				{ Value = a.H_Activity_Id.ToString(), Text = a.Activity_Name, Selected = (activityId.HasValue && a.H_Activity_Id == activityId.Value) })
				.ToList()
				.Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇" });

			return items;
		}

		public CreateH_Source_DetailDto GetSourceById(int? id)
		{
			H_Source_Details detail = _db.H_Source_Details
				.SingleOrDefault(d => d.Source_H_Id == id);

			var data = detail.ToEditDto();

			return data;
		}

		public void UpdateHDetail(CreateH_Source_DetailDto dto)
		{
			H_Source_Details detail = _db.H_Source_Details.SingleOrDefault(d => d.Source_H_Id == dto.Source_H_Id);
			int employeeId = _db.Employees.SingleOrDefault(e => e.Account == dto.Employee_Name).Employee_id;

			detail = new H_Source_Details
			{
				Member_Id = dto.Member_Id,
				Activity_Id = dto.Activity_Id,
				Difference_H = dto.Difference_H,
				Event_Time = dto.Event_Time,
				Remark = dto.Remark,
				Employee_Id = employeeId,
			};
			_db.SaveChanges();
		}

		/// <summary>
		/// 將H_Value傳入Member資料表
		/// </summary>
		/// <param name="id"></param>
		/// <param name="total"></param>
		public void AddH_valueInMember(int id, int total)
		{
			var item = _db.Members
				.FirstOrDefault(m => m.Id == id);

			item.Total_H = total;

			_db.SaveChanges();
		}

	}
}