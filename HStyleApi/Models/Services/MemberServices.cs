using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures;
using HStyleApi.Models.InfraStructures.Repositories;

namespace HStyleApi.Models.Services
{
    public class MemberServices
    {
        private  MemberRepositories _MemberRepositories;
        public MemberServices(AppDbContext db)
        {
            _MemberRepositories = new MemberRepositories(db);
        }

        //public (bool Success, string Message) MemberRegister(RegisterDTO dto, string urlTemplate)
        //{
        //    if (_MemberRepositories(dto.Account!, dto.Name!))
        //    {
        //        return (false, "帳號已存在");
        //    }
        //    // 驗證暱稱是否重複


        //    dto.Mail_code = Guid.NewGuid().ToString("N");

        //    _MemberRepositories(dto);

        //    MemberDTO entity = _MemberRepositories.GetByAccount(dto.Account!);
        //    // 發email
        //    string url = string.Format(urlTemplate, entity.Id, dto.Mail_code);

        //    new EmailHelper().SendConfirmRegisterEmail(url, dto.Name!, dto.Email!);

        //    return (true, "註冊成功，已發送驗證信");
        //}


    }
}
