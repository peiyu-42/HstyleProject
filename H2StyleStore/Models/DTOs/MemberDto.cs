using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string EncryptedPassword { get; set; }  // 改成員工的 試試看 (加密密碼  目前資料庫沒有 前台在抓出來用)

        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone_Number { get; set; } //Mobile=

        public bool? Mail_verify { get; set; } //Mail_verify = IsConfirmed

        public string Mail_code { get; set; }  //Mail_code  =  ConfirmCode
    }
}