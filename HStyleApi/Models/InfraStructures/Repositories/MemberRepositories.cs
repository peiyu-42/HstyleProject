using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace HStyleApi.Models.InfraStructures.Repositories
{
    public class MemberRepositories
    {
        private readonly AppDbContext _db;
        public MemberRepositories(AppDbContext db)
        {
            _db = db;
        }

        //        public MemberDTO GetByAccount(string Account)
        //{
        //    var data = _db.Members
        //        .SingleOrDefault(x => x.MemberAccount == Account)!;

        //    return data.ToDTO();
        //}



    }
}
