using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Infrastructures.ExtensionMethods
{
	public static class EmployeesExts
	{
		public static EmployeeDto ToDto1(this Employee entity)
		{
			return entity == null
				? null
				: new EmployeeDto
				{
					Permission_id= entity.Permission_id,
					Employee_id = entity.Employee_id,
					Account = entity.Account,
					EncryptedPassword = entity.EncryptedPassword, //老樣子驗證碼
					Title = entity.Title,
				
				};
		}
	}
}