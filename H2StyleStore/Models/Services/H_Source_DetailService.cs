using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace H2StyleStore.Models.Services
{
	public class H_Source_DetailService
	{
		private readonly IH_Source_DetailRepository _repository;
		public H_Source_DetailService(IH_Source_DetailRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<H_Source_DetailDto> GetSource()
			=> _repository.GetSource();

		public IEnumerable<H_CheckInDto> GetCheckIn()
			=> _repository.GetCheckIn();

		public string CreateHDetail(H_Source_DetailDto dto)
		{
			try
			{
				//TotalHcoin(dto.Member_Id);
				_repository.CreateHDetail(dto);
				return "新增成功";
			}
			catch (Exception ex) { return ex.Message; }
		}

		public string CreateNewDetail(CreateH_Source_DetailDto dto)
		{
			try
			{
				_repository.CreatNewDetail(dto);

				return "新增成功";
			}
			catch(Exception ex)
			{
				return ex.Message;
			}
		}
		public void UpdateDetail(CreateH_Source_DetailDto dto)
		{
			CreateH_Source_DetailDto entity = _repository.GetSourceById(dto.Source_H_Id);
			if (entity == null) throw new Exception("找不到要修改的紀錄");

			entity.Member_Id = dto.Member_Id;
			entity.Activity_Id = dto.Activity_Id;
			entity.Difference_H = dto.Difference_H;
			entity.Remark = dto.Remark;
			entity.Employee_Name = dto.Employee_Name;
			_repository.UpdateHDetail(entity);
		}

		/// <summary>
		/// 計算所有 H coin
		/// </summary>
		/// <param name="id"></param>
		public void TotalHcoin(int id)
		{
			// 找出會員所有活動的紀錄
			var detail = _repository.GetTotalDetail(id);

			// 計算H幣總額
			int total = 0;
			foreach (var item in detail)
			{
				total += item.Difference_H;
			}

			// 修改Member的Total_H
			_repository.AddH_valueInMember(id, total);
		}


	}
}