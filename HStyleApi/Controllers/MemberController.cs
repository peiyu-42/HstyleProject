using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HStyleApi.Models.InfraStructures;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using static System.Net.WebRequestMethods;





// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        //private readonly MemberServices _Service;

        //public MemberController(AppDbContext db)
        //{
        //    _Service = new MemberServices(db);
        //}
        private readonly int _memberId;

        private readonly AppDbContext _context;
        public MemberController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var claims = httpContextAccessor.HttpContext.User.Claims;
            if (claims.Any())
            {
                var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
                _memberId = memberid;
            }
            //TODO從COOKIE取

        }
        //      public MemberController(AppDbContext context) 整合上面
        //      {
        //          _context = context;

        //}
        // GET: api/<MemberController>
        [Authorize]
        [HttpGet]
        public ActionResult<MemberDTO> GetMemberInfo()
        {
            //var memberId = int.Parse(HttpContext.User.FindFirst("memberId")!.Value);
            MemberDTO member = _context.Members                
            .Include(x => x.Permission)
            .FirstOrDefault(x => x.Id == _memberId).ToDto();


            return Ok(member);

        }

        [Authorize]
        [HttpGet]
        [Route("GetAddressInfo")]
        public IActionResult GetAddressInfo()
        {
            //var memberId = int.Parse(HttpContext.User.FindFirst("memberId")!.Value);
            //AddAddressDTO AddAddress = _context.Addresses
            // .Include(X => X.Member)
            //.FirstOrDefault(x => x.MemberId == _memberId).ToDTO();

            IEnumerable<AddAddressDTO> address = _context.Addresses.Include(x => x.Member).Where(x => x.MemberId == _memberId).Select(x => x.ToDTO());
            

            return Ok(address);

        }


        [HttpPost("LogIn")]
        [AllowAnonymous]
        public IActionResult LogIn(LogInDTO value)
        {
            var member = _context.Members.FirstOrDefault(x => x.Account == value.Account);

            if (member == null)
            {
                return BadRequest("帳號或密碼錯誤");
            }

            string encryptedPwd = HashUtility.ToSHA256(value.Password, RegisterDTO.SALT);

            if (String.CompareOrdinal(member.EncryptedPassword, encryptedPwd) != 0)
            {
                return BadRequest("帳號或密碼錯誤");
            }

            if (member.MailVerify == false)
            {
                return BadRequest("會員資格尚未確認");
            }
            else
            {
                var claims = new List<Claim>
                 {
                   new Claim("MemberId", member.Id.ToString()), //測試用
                   new Claim(ClaimTypes.Name, member.Account),
                   new Claim("FullName", member.Name),
                  };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }

            // 建立一個包含用戶相關聲明(Claim)的JavaScript對象
            var userData = new
            {
                id = member.Id,
                account = member.Account,
                name = member.Name,
                email = member.Email,
            };

            // 將JavaScript對象傳遞回前端
            return Ok(userData);
        }
  
        [HttpGet("id")] //測試用
        public int GetMemberId()
        {
            var claims = User.FindFirst("MemberId");
            if (claims != null)
            {
                int memberId = int.Parse(claims.Value);
                // Do something with the member ID
                return memberId;
            }
            else
            {
                // 無法獲取 MemberId，返回未授權狀態碼
                return (-1);
            }
        }
		[HttpGet("NoLogin")]
		public IActionResult NoLogin()
        {
            return Unauthorized();
        }


		[Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {

            var claims = User.FindFirst("MemberId");
            if (claims != null)
            {
                int memberId = int.Parse(claims.Value);
                // Do something with the member ID
            }
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok("登出成功");
            }
            
            return BadRequest("您尚未登入");
            

        }


        [HttpPost("EditMember")]
        public async Task<string> EditMember(EditMemberDTO dto)
        {
       
            var member = _context.Members.FirstOrDefault(m => m.Id == _memberId);
            if (member == null)
            {
                return ("會員不存在");
            }

            member.Name = dto.Name;                   //試抓出   MemberRepository
            //member.Email = dto.Email;  //Email 唯一值必須驗證
            //member.Account = dto.Account;
            member.PhoneNumber = dto.PhoneNumber;//手機號碼
            member.Address = dto.Address;
            //member.Gender = dto.Gender;
            //member.Birthday = dto.Birthday;  //不要讓他改     會有優惠問題

            _context.SaveChanges();
            return "更新成功";
        }

        [HttpPost("AddAddress")]
        public async Task<string> AddAddress(AddAddressDTO dto)
        {
            MemberDTO member = _context.Members.FirstOrDefault(m => m.Id == _memberId).ToDto();

            Address address = new Address
            {
                DestinationName = dto.DestinationName,                       
                Destination = dto.Destination,
                DestinationThe = dto.DestinationThe,
                DestinationCategory = "7-11",  //改了這個 保護資料庫不會錯
                MemberId = member.Id,
                Preset=false, //改了這個 保護資料庫不會錯
                Gender =true,  //改了這個 保護資料庫不會錯


            };            
            _context.Addresses.Add(address);
            _context.SaveChanges();
            return "新增地址成功";
        }

        //[Authorize]
        [HttpPost("Register")]
        public async Task<string> Register(RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return "輸入錯誤";
            }
            if (_context.Members.Any(x => x.Account == register.Account))
            {
                return "帳號已存在";
            }

            Member member = new Member
            {
                Name = register.Name,                         //試抓出   MemberRepository
                Email = register.Email,   //Email 必須輸入正確的 要改驗證
                Account = register.Account,
                PhoneNumber = register.Phone_Number, //手機號碼
                Address = register.Address,
                Gender = register.Gender,
                Birthday = register.Birthday,
                PermissionId = 1,
                Jointime = DateTime.Now,
                MailVerify = false, //預設是未確認的會員  IsConfirmed=我資料庫的 Mail_verify
                MailCode = Guid.NewGuid().ToString("N"),//mail的確認確認碼  ConfirmCode=我資料庫的 Mail_code
                EncryptedPassword = register.EncryptedPassword, //加密密碼 
                TotalH = 0,

            };
            _context.Members.Add(member);
            _context.SaveChanges();
            SendEmail(member);  //確認寄信用  
            return "註冊成功請去信箱驗證";
        }

        [HttpPost("SendEmail")]
        public string SendEmail(Member member)
        {

            var message = new MimeMessage();

            // 添加寄件者
            message.From.Add(new MailboxAddress("H2StyleStore", "pawpawpetSite@gmail.com"));

            // 添加收件者
            message.To.Add(new MailboxAddress("New Member", $"{member.Email}"));

            // 設定郵件標題
            message.Subject = "Welcome";

            // 使用 BodyBuilder 建立郵件內容
            var bodyBuilder = new BodyBuilder();

            string result = $"http://localhost:5173/MemberForgetPasswordEmail/{member.Account}/{member.MailCode}";

            //string result = Request.Scheme + "://" + Request.Host + $"/api/Member/EmailConfirm/?account={member.Account}&confirmCode={member.MailCode}";

            //string result = "https://localhost:44313/Images/MemberImage/456.jpg";

            // 設定 HTML 內容
            bodyBuilder.HtmlBody = $"<a href=\"{result}\">點此連結驗證</a>";

            // 設定郵件內容
            message.Body = bodyBuilder.ToMessageBody();
            
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("vunvun0213@gmail.com", "yvupanwipdifkupw");
                client.Send(message);
                client.Disconnect(true);
            }
            return "成功";
        }

        [HttpGet("EmailConfirm")]
        public string EmailConfirm(string account, string confirmCode)
        {

            var emailmember = _context.Members.FirstOrDefault(x => x.Account == account);
            if (emailmember == null)
            {
                return "認證失敗";
            }
            else if (string.Compare(confirmCode, emailmember.MailCode) != 0) { return "認證失敗!"; }
            else
            {
                emailmember.MailVerify = true;
                emailmember.MailCode = null;
                _context.SaveChanges();
                return "驗證成功";
            }
        }

        [HttpPost("ForgetPassword")]
        public string ForgetPassword(ForgetPasswordDTO forgetPasseordMember)
        {
            if (ModelState.IsValid == false)
            {
                return "帳號或信箱輸入錯誤";
            }
            var member = _context.Members.SingleOrDefault(x => x.Account == forgetPasseordMember.Account);
            if (member == null) { return "帳號或信箱輸入錯誤"; }
            if (string.Compare(forgetPasseordMember.Email, member.Email) == 0)
            {
                string newpassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                SendForgetPasswordEmail(member, newpassword);  //改這行
                member.EncryptedPassword = HashUtility.ToSHA256(newpassword, RegisterDTO.SALT);
                _context.SaveChanges();
                return "電子郵件已寄出";
            }
            else
            {
                return "帳號或信箱輸入錯誤";
            }
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        //public string ResetPassword([FromBody] string account, [FromBody] string oldPassword, [FromBody] string newPassword, [FromBody] string newPassword2)
        public string ResetPassword(string oldPassword,  string newPassword,  string newPassword2)
        {
       
            var member = _context.Members.SingleOrDefault(x => x.Id == _memberId);

            var encryptedPassword = HashUtility.ToSHA256(oldPassword, RegisterDTO.SALT);
            if (member == null)
            {
                return "帳號或密碼輸入錯誤";
            }
            else if (newPassword != newPassword2)
            {
                return "密碼確認有誤";
            }
            else if (newPassword == null|| newPassword==null|| oldPassword==null)
            {
                return "數值不能為空";
            }
            else if (string.Compare(member.EncryptedPassword, encryptedPassword) == 0)
            {
                member.EncryptedPassword = HashUtility.ToSHA256(newPassword, RegisterDTO.SALT);
                _context.SaveChanges();
                 
                return "修改成功";
            }
            else
            {
                return "帳號或密碼輸入錯誤";
            }
        }
        [HttpPost("SendForgetPasswordEmail")]
        public string SendForgetPasswordEmail(Member member, string newPassword)
        {

            var message = new MimeMessage();

            // 添加寄件者
            message.From.Add(new MailboxAddress("H2StyleStore", "pawpawpetSite@gmail.com"));

            // 添加收件者
            message.To.Add(new MailboxAddress("New Member", $"{member.Email}"));

            // 設定郵件標題
            message.Subject = "會員新密碼";

            // 使用 BodyBuilder 建立郵件內容
            var bodyBuilder = new BodyBuilder();

            //string result = Request.Scheme + "://" + Request.Host + $"/api/Members/LogIn";  //前端驗證完成頁面  給他個驗證完成  改這邊

            string result = "http://localhost:5173/ForgetPasswordEmail";  //前端驗證完成頁面  給他個驗證完成  改這邊

            // 設定 HTML 內容
            bodyBuilder.HtmlBody = $"<p>新密碼:{newPassword}</p>" +
                                   $"<a href=\"{result}\">點此連結登入</a>";

            // 設定郵件內容
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("pawpawpetSite@gmail.com", "fbqolajjiyshkxyg");
                client.Send(message);
                client.Disconnect(true);
            }
            return "成功";
        }
        // GET api/<MemberController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MemberController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MemberController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MemberController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 驗證 Google 登入授權
        /// </summary>
        /// <returns></returns>
        //public IActionResult ValidGoogleLogin()
        //{
        //    string? formCredential = Request.Form["credential"]; //回傳憑證
        //    string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
        //    string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

        //    // 驗證 Google Token
        //    GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
        //    if (payload == null)
        //    {
        //        // 驗證失敗
        //        ViewData["Msg"] = "驗證 Google 授權失敗";
        //    }
        //    else
        //    {
        //        //驗證成功，取使用者資訊內容
        //        ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
        //        ViewData["Msg"] += "Email:" + payload.Email + "<br>";
        //        ViewData["Msg"] += "Name:" + payload.Name + "<br>";
        //        ViewData["Msg"] += "Picture:" + payload.Picture;
        //    }

        //    return View();
        //}

        //[HttpPost("VerifyGoogleLogin")]  //測試用
        //public async Task<IActionResult> VerifyGoogleLogin([FromBody] Dictionary<string, string> formData)
        //{
        //    string formCredential, formToken, cookiesToken;
        //    formData.TryGetValue("credential", out formCredential);
        //    formData.TryGetValue("g_csrf_token", out formToken);
        //    cookiesToken = Request.Cookies["g_csrf_token"];

        //    // 驗證 Google Token
        //    GoogleJsonWebSignature.Payload? payload = await VerifyGoogleToken(formCredential, formToken, cookiesToken);
        //    if (payload == null)
        //    {
        //        // 驗證失敗
        //        return BadRequest("驗證 Google 授權失敗");
        //    }
        //    else
        //    {
        //        // 驗證成功，返回使用者資訊內容
        //        return Ok(new
        //        {
        //            email = payload.Email,
        //            name = payload.Name,
        //            picture = payload.Picture
        //        });
        //    }
        //}

        ///// <summary>
        ///// 驗證 Google Token
        ///// </summary>
        ///// <param name="formCredential"></param>
        ///// <param name="formToken"></param>
        ///// <param name="cookiesToken"></param>
        ///// <returns></returns>
        //public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        //{
        //    // 檢查空值
        //    if (formCredential == null || formToken == null && cookiesToken == null)
        //    {
        //        return null;
        //    }

        //    GoogleJsonWebSignature.Payload? payload;
        //    try
        //    {
        //        // 驗證 token
        //        if (formToken != cookiesToken)
        //        {
        //            return null;
        //        }

        //        // 驗證憑證
        //        IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
        //        string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
        //        var settings = new GoogleJsonWebSignature.ValidationSettings()
        //        {
        //            Audience = new List<string>() { GoogleApiClientId }
        //        };
        //        payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
        //        if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
        //        {
        //            return null;
        //        }
        //        if (payload.ExpirationTimeSeconds == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            DateTime now = DateTime.Now.ToUniversalTime();
        //            DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
        //            if (now > expiration)
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return payload;
        //}
    }
   
}

