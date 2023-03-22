using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class GoogleLoginController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public GoogleLoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }



    [HttpPost("/login")]
    public IActionResult Login([FromBody] GoogleLoginRequest request)
    {
        try
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                //Audience = new[] { Configuration["Google:ClientId"] } 會有BUG 
            }).Result;

            // 驗證成功，建立登入使用者的身分識別資訊
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, payload.Subject),
            new Claim(ClaimTypes.Name, payload.Email),
            // 可以新增其他您需要的使用者屬性
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 將使用者登入狀態保存至 Cookie 中
            HttpContext.SignInAsync(principal).Wait();

            return Ok();
        }
        catch (InvalidJwtException)
        {
            // 驗證失敗，拒絕登入
            return Unauthorized();
        }
    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; }
    }

}