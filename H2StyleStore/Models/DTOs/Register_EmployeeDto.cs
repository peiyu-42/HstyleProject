using H2StyleStore.Models.Infrastructures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class Register_EmployeeDto
	{
		public const string SALT = "!@#$$DGTEGYT";
		public string Account { get; set; }

		/// <summary>
		/// 密碼,明碼
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 加密之後的密碼
		/// </summary>
		public string EncryptedPassword
		{
			get
			{
				string salt = SALT;
				string result = HashUtility.ToSHA256(this.Password, salt);
				return result;
			}
		}

		[StringLength(50)]
		public string Title { get; set; }
		public int Permission_id { get; set; }

	}
}