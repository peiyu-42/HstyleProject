using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IEmployeeRepository
	{
		bool IsExist(string account);
		void Create(Register_EmployeeDto dto);

		//EmployeeDto Load(int memberId);
		EmployeeDto GetByAccount(string account);
		//void ActiveRegister(int memberId);

		//void Update(EmployeeDto entity);

		//void UpdatePassword(int memberId, string newEncryptedPassword);
	}
}
