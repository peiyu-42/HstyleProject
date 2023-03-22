using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.Infrastructures;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{
	public class EmployeeService
	{
		private readonly IEmployeeRepository repository;
		public EmployeeService(IEmployeeRepository repo)
		{
			this.repository = repo;
		}

		public (bool IsSuccess, string ErrorMessage) CreateNewMember(Register_EmployeeDto dto)  //創造新員工
		{
			// todo 判斷各欄位是否正確

			// 判斷帳號是否已存在
			if (repository.IsExist(dto.Account))
			{
				return (false, "帳號已存在");
			}

			#region 建立一個會員記錄

			//	 建立 ConfirmCode
			//dto.Mail_code = Guid.NewGuid().ToString("N");   //確認碼

			repository.Create(dto);

			#endregion

			return (true, null);
		}
		　
		//public void ActiveRegister(int memberId, string confirmCode)　　　email 驗證用
		//{
		//	EmployeeDto dto = repository.Load(memberId);
		//	if (dto == null) return;

		//	if (string.Compare(dto.Mail_code, confirmCode) != 0) return;

		//	repository.ActiveRegister(memberId);
		//}

		public (bool IsSuccess, string ErrorMessage) Login(string account, string password)//12-26 02:02:02
		{
			EmployeeDto member = repository.GetByAccount(account);

			if (member == null)
			{
				return (false, "帳密有誤(帳號)");
			}

			//if (member.Mail_verify.HasValue == false || member.Mail_verify.HasValue && member.Mail_verify.Value == false)
			//{
			//	return (false, "會員資格尚未確認");
			//}

			string encryptedPwd = HashUtility.ToSHA256(password, Register_EmployeeDto.SALT);   //12-26 AM 02:14:00

			return (String.CompareOrdinal(member.EncryptedPassword, encryptedPwd) == 0)
				? (true, null)
				: (false, "帳密有誤");
		}

		//public void UpdateProfile(UpdateProfileDto request)
		//{
		//	// todo 驗證傳入的屬性值是否正確

		//	// 取得在db裡的原始記錄
		//	EmployeeDto entity = repository.GetByAccount(request.Account);
		//	if (entity == null) throw new Exception("找不到要修改的會員記錄");

		//	// 更新記錄
		//	entity.Name = request.Name;
		//	entity.Email = request.Email;
		//	entity.Phone_Number = request.Mobile;
		//	entity.Account = request.Account;

		//	repository.Update(entity);

		//}

		//public void ChangePassword(ChangePasswordRequest request)   //前端用來變更密碼用的 前端在抓出來用  目前資料庫沒有
		//{
		//	// todo 驗證傳入的屬性值是否正確

		//	// 取得在db裡的原始記錄
		//	MemberDto entity = repository.GetByAccount(request.CurrentUserAccount);
		//	if (entity == null) throw new Exception("找不到要修改的會員記錄");

		//	// 判斷原始密碼是否相同
		//	string encryptedPassword = HashUtility.ToSHA256(request.OriginalPassword, RegisterDto.SALT);

		//	bool isSamePassword = string.Compare(encryptedPassword, entity.EncryptedPassword) == 0;

		//	if (!isSamePassword) throw new Exception("原始密碼不符,無法變更");

		//	// 更新記錄
		//	var newEncryptedPassword = HashUtility.ToSHA256(request.NewPassword, RegisterDto.SALT);

		//	repository.UpdatePassword(entity.Id, newEncryptedPassword);
		//}

		/// <summary>
		/// 請求重設密碼
		/// </summary>
		/// <param name="account"></param>
		/// <param name="email"></param>
		/// <param name="urlTemplate"></param>
		/// <exception cref="Exception"></exception>
		//public void RequestResetPassword(string account, string email, string urlTemplate)
		//{
		//	// todo 檢查傳入參數的合理性

		//	// 檢查account,email正確性
		//	MemberDto entity = repository.GetByAccount(account);

		//	if (entity == null)
		//	{
		//		throw new Exception("帳號或 Email 錯誤"); // 故意不告知確切錯誤原因
		//	}

		//	if (string.Compare(email, entity.Email) != 0)
		//	{
		//		throw new Exception("帳號或 Email 錯誤");
		//	}

		//	// 檢查 IsConfirmed必需是true
		//	if (entity.Mail_verify == false)
		//	{
		//		throw new Exception("您還沒有啟用本帳號, 請先完成才能重設密碼");
		//	}

		//	// 更新記錄, 填入 confirmCode
		//	string confirmCode = Guid.NewGuid().ToString("N");
		//	entity.Mail_code = confirmCode;
		//	repository.Update(entity);

		//	// 發email
		//	string url = string.Format(urlTemplate, entity.Id, confirmCode);

		//	new EmailHelper().SendForgetPasswordEmail(url, entity.Name, email);
		//}

		/// <summary>
		/// 重設密碼
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="confirmCode"></param>
		/// <param name="newPassword"></param>
		/// <exception cref="Exception"></exception>
		//public void ResetPassword(int memberId, string confirmCode, string plainTextPassword)   //重設密碼
		//{
		//	// todo 檢查傳入參數值是否合理


		//	string encryptedPassword = HashUtility.ToSHA256(plainTextPassword, RegisterDto.SALT);

		//	MemberDto entity = repository.Load(memberId);
		//	// 檢查有沒有記錄
		//	if (entity == null) throw new Exception("找不到對應的會員記錄");

		//	// 檢查confirmcode 
		//	if (string.Compare(confirmCode, entity.Mail_code) != 0)
		//	{
		//		throw new Exception("找不到對應的會員記錄");
		//	}

		//	// 更新密碼
		//	repository.UpdatePassword(memberId, encryptedPassword);

		//	// 將confirmCode清空
		//	entity.Mail_code = null;
		//	repository.Update(entity);
		//}
	}
}
