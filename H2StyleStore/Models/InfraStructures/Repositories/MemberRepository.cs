using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.ExtensionMethods;
using H2StyleStore.Models.Services.Interfaces;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private AppDbContext db = new AppDbContext();  
        public void Create(RegisterDto dto)
        {
            Member member = new Member  //存取資料庫 這還沒搞定
            { 
                Name = dto.Name,
                Email = dto.Email,
                Account = dto.Account,
                Phone_Number = dto.Phone_Number, //手機號碼
				Address =dto.Address,
				Gender = dto.Gender,
				Birthday =dto.Birthday,
				Permission_Id = 1,
				Jointime =DateTime.Now,
				Mail_verify = false, //預設是未確認的會員  IsConfirmed=我資料庫的 Mail_verify
				Mail_code = dto.Mail_code,//mail的確認確認碼  ConfirmCode=我資料庫的 Mail_code
				EncryptedPassword = dto.EncryptedPassword, //加密密碼 
                Total_H = 0,
                  
               
            };

            db.Members.Add(member);
            db.SaveChanges();


			// H幣活動，註冊成功就送H幣
			int activityId_register = 1; // Hcoin的活動ID
            int difference_H = db.H_Activities.SingleOrDefault(a => a.H_Activity_Id == activityId_register).H_Value; // Hcoin的值
            var memberHcoin = db.Members.SingleOrDefault(m => m.Account == dto.Account); // 新註冊會員的ID
            H_Source_Details detail = new H_Source_Details
            {
                Member_Id = memberHcoin.Id,
                Activity_Id = activityId_register,
                Difference_H = difference_H,
                Event_Time = DateTime.Now,
                Total_H_SoFar = difference_H,
            }; // 新增此會員的註冊Hcoin
			db.H_Source_Details.Add(detail);
			db.SaveChanges();
            member.Total_H += difference_H;
			db.SaveChanges();
		}

		public MemberDto Load(int memberId)
        {
            Member entity = db.Members.SingleOrDefault(x => x.Id == memberId);
            if (entity == null) return null;

            MemberDto result = new MemberDto
            {
                Id = entity.Id,
                Account = entity.Account,
                EncryptedPassword = entity.EncryptedPassword, //加密密碼  
                Email = entity.Email,
                Name = entity.Name,
                Phone_Number = entity.Phone_Number, //手機號碼
                Mail_verify = entity.Mail_verify,   //預設是未確認的會員  IsConfirmed=我資料庫的 Mail_verify
                Mail_code = entity.Mail_code   //mail的確認確認碼  ConfirmCode=我資料庫的 Mail_code
            };

            return result;
        }

        public MemberDto GetByAccount(string account)
        {
            return db.Members
                .SingleOrDefault(x => x.Account == account)
                .ToDto();
        }

        public bool IsExist(string account)
        {
            var entity = db.Members.SingleOrDefault(x => x.Account == account);

            return (entity != null);

        }

        public void ActiveRegister(int memberId)
        {
            var member = db.Members.Find(memberId);
            member.Mail_verify = true;
            member.Mail_code = null;
            db.SaveChanges();
        }

        /// <summary>
        /// 更新記錄,本method不會更新密碼
        /// </summary>
        /// <param name="entity"></param>
        public void Update(MemberDto entity)
        {
            Member member = db.Members.Find(entity.Id);

            member.Email = entity.Email;
            member.Name = entity.Name;
            member.Phone_Number = entity.Phone_Number;
            // 在忘記密碼時, 使用者請求重設密碼, 會叫用本method,所以以下二個屬性也要更新
            member.Mail_verify = entity.Mail_verify;
            member.Mail_code = entity.Mail_code;

            db.SaveChanges();
        }

        public void UpdatePassword(int memberId, string newEncryptedPassword)
        {
            var member = db.Members.Find(memberId);

            member.EncryptedPassword = newEncryptedPassword;

            db.SaveChanges();
        }
    }
}