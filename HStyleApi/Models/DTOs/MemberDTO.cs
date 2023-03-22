using HStyleApi.Models.EFModels;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;

namespace HStyleApi.Models.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Gender { get; set; }

        public string? Birthday { get; set; }
        public int? PermissionId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Jointime { get; set; }
        public bool? MailVerify { get; set; }
        public string MailCode { get; set; }
        public int? TotalH { get; set; }
        public string EncryptedPassword { get; set; }
    }
    public static class MemberExts
    {
        public static MemberDTO ToDto(this Member source)
        {
            string birthday = string.Empty;
            if (source.Birthday.HasValue == true)
            {
                birthday= source.Birthday.Value.ToString("yyyy/MM/dd");
            }
            string gender = string.Empty;
            if (source.Gender.HasValue == true)
            {
                gender = "男";
            }
            else if (source.Gender == null)
            {
                gender = string.Empty; 
            }
            else
            {
                gender = "女";
            }
            return new MemberDTO
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                Account = source.Account,
                PhoneNumber = source.PhoneNumber,
                Address = source.Address,
                Gender = gender,
                Birthday = birthday,
                PermissionId = source.PermissionId,
                Jointime = source.Jointime,
                MailVerify = source.MailVerify,
                MailCode = source.MailCode,
                TotalH = source.TotalH,
                EncryptedPassword = source.EncryptedPassword,

            };
        }
    }
}
