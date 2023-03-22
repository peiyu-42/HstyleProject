using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace H2StyleStore.Models.ViewModels
{
	public class Register_EmployeesVM
	{
		public int Employee_id { get; set; }

		[Display(Name = "帳號")]
		[Required]
		[StringLength(50)]
		public string Account { get; set; }

		/// <summary>
		/// 明碼
		/// </summary>
		[Display(Name = "密碼")]
		[Required]
		[StringLength(50)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "密碼確認")]
		[Required]
		[StringLength(50)]
		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		
		public string ConfirmPassword { get; set; }

		//[StringLength(50)]
	 //   public string Title { get; set; }   應該不需要給人輸入吧~?


	}

	public static class Register_EmployeesVMVMExts
	{
		public static Register_EmployeeDto ToRequest_EmployeesVMDto(this Register_EmployeesVM source)
		{
			return new Register_EmployeeDto
			{
				Account = source.Account,
				Password = source.Password,

			};
		}
	}
}
