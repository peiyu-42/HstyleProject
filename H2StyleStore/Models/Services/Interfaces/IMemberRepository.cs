using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H2StyleStore.Models.DTOs;

namespace H2StyleStore.Models.Services.Interfaces
{
    public interface IMemberRepository
    {
        bool IsExist(string account);
        void Create(RegisterDto dto);

        MemberDto Load(int memberId);
        MemberDto GetByAccount(string account);
        void ActiveRegister(int memberId);

        void Update(MemberDto entity);

        void UpdatePassword(int memberId, string newEncryptedPassword);
    }
}
