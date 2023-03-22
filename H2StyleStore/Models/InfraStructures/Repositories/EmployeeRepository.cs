using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.ExtensionMethods;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class EmployeeRepository: IEmployeeRepository
	{
		private AppDbContext db = new AppDbContext();
		public void Create(Register_EmployeeDto dto)
		{
			Employee employees = new Employee  
			{
				Account = dto.Account,
				Title = null, 
				Permission_id = 4,
				EncryptedPassword = dto.EncryptedPassword, //加密密碼 
			};

			db.Employees.Add(employees);
			db.SaveChanges();
		}

		public EmployeeDto Load(int EmployeeId)
		{
			Employee entity = db.Employees.SingleOrDefault(x => x.Employee_id == EmployeeId);
			if (entity == null) return null;

			EmployeeDto result = new EmployeeDto
			{
				Employee_id = entity.Employee_id,
				Account = entity.Account,
				EncryptedPassword = entity.EncryptedPassword, //加密密碼  
				Title = entity.Title,
			};

			return result;
		}

		public EmployeeDto GetByAccount(string account)
		{
			return db.Employees
				.SingleOrDefault(x => x.Account == account)
				.ToDto1();  //可能有using 的問題
		}

		public bool IsExist(string account)
		{
			var entity = db.Employees.SingleOrDefault(x => x.Account == account);

			return (entity != null);

		}
	}
}