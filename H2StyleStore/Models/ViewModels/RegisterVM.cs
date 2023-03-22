using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using H2StyleStore.Models.DTOs;
using System.Xml.Linq;
using H2StyleStore.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace H2StyleStore.Models.ViewModels
{
    public class RegisterVM
    {
        public int Id { get; set; }

        [Display(Name = "帳號")]
        [Required]
        [StringLength(30)]
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

        [Display(Name = "性別")]
		[Required]
		public bool Gender { get; set; }

		[Required]
		[Display(Name = "地址")]
        public string Address { get; set; }
       
        [Display(Name = "生日")]
        [Column(TypeName = "date")]
		[Required]
		public DateTime Birthday { get; set; }
       
        [Display(Name = "加入日期")]
        [Column(TypeName = "datetime")]
		[Required]
		public DateTime Jointime { get; set; }

        [Display(Name = "信箱")]
        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; }
    
        [Display(Name = "姓名")]
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        
        [Display(Name = "手機")]
        [StringLength(10)]
		[Required]
		public string Phone_Number { get; set; }
    }

    public static class RegisterVMExts
    {
        public static RegisterDto ToRequestDto(this RegisterVM source)
        {
            return new RegisterDto
            {
                Account = source.Account,
                Password = source.Password,
				Gender= source.Gender,
				Address = source.Address,
                Birthday = source.Birthday,
                Jointime = source.Jointime,
                Email = source.Email,
                Name = source.Name,
                Phone_Number = source.Phone_Number
            };
        }
    }
}
