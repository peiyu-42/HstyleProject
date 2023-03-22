using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
    [Authorize]
    [EnableCors("AllowAny")]
	[Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _CartService;
        private readonly int _memberId;
        public CartController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _CartService = new CartService(db);
			var claims = httpContextAccessor.HttpContext.User.Claims;
			if (claims.Any())
			{
				var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
				_memberId = memberid;
			}
			//TODO從COOKIE取
			
		}
		

		
		[HttpPost("Checkout")]
        public IActionResult Checkout(CheckoutDTO value)
        {
            int orderId = 0;
			try
            {
				OrderDTO checkoutList = _CartService.GetCheckout(_memberId, value);
				orderId = _CartService.CreateOrder(checkoutList);
			}
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
			//Todo:新增信用卡支付，直接導向付款api           
			return Ok(orderId);
        }

		// GET: api/<CartController>
		
		[HttpGet]
        public CartListDTO Get()
        {
	
			return _CartService.GetCart(_memberId);
        }

		// POST api/<CartController>
		
		[HttpPost("{specId}")]
        public ActionResult Add(int specId)
        {
            
            if (_memberId <= 0)
            {
                return Unauthorized("請先登入帳戶");
            }
            try
            {
                _CartService.AddItem(_memberId, specId);

            }catch(Exception ex)
            {
                return BadRequest("商品缺貨中");
            }
            
            return Ok("新增成功");
        }
		[HttpDelete("{specId}")]
		public ActionResult Minus(int specId)
		{

			if (_memberId <= 0)
			{
				return Unauthorized("請先登入帳戶");
			}
			try
			{
				_CartService.MinusItem(_memberId, specId);

			}
			catch (Exception ex)
			{
				return BadRequest("商品已不存在");
			}

			return Ok("刪除成功");
		}

		




	}
}
