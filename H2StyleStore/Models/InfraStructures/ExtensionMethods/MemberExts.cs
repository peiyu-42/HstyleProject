using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;

namespace H2StyleStore.Models.Infrastructures.ExtensionMethods
{
    public static class MemberExts
    {
        public static MemberDto ToDto(this Member entity)
        {
            return entity == null
                ? null
                : new MemberDto
                {
                    Id = entity.Id,
                    Account = entity.Account,
                    EncryptedPassword = entity.EncryptedPassword, //老樣子驗證碼
                    Email = entity.Email,
                    Name = entity.Name,
                    Phone_Number = entity.Phone_Number,
                    Mail_verify = entity.Mail_verify,
                    Mail_code = entity.Mail_code
                };
        }
    }
}