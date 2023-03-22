using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using H2StyleStore.Models.Infrastructures;

namespace H2StyleStore.Models.DTOs
{
    public class RegisterDto
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
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Jointime { get; set; }
        public bool Gender { get; set; }


        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone_Number { get; set; } //手機號碼  Mobile

        public string Mail_code { get; set; } //確認碼   ConfirmCode
    }
}