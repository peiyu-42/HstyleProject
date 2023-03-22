using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class HCoinController : ControllerBase
	{
		private readonly HCoinService _service;
		private readonly int _member_id;
		public HCoinController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_service = new HCoinService(db);
			var claims = httpContextAccessor.HttpContext.User.Claims;
			if (claims.Any())
			{
				var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
				_member_id = memberid;
			}
		}

		// GET api/<HCoinController>/5
		// 得到打卡資料
		[Authorize]
		[HttpGet("CheckIn")]
		public async Task<HCheckInDTO> GetHCheckIn()
		{
			int memberId = _member_id;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				// 活動Id和項目
				int activityId_checkIn = 3;
				return await _service.GetHCheckIn(memberId, activityId_checkIn);
			}
		}

		// PUT api/<HCoinController>/5
		// 將打卡紀錄傳回資料庫
		[Authorize]
		[HttpPut("CheckIn")]
		public async Task<ActionResult> PutCheckIn()
		{
			int memberId = _member_id;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				string response = await _service.PutCheckInById(memberId);
				return Ok(response);
			}
		}

		// 取得花費活動的規則
		[HttpGet("CostHCoin")]
		public async Task<IEnumerable<HActivityDTO>> GetCostRule()
		{
			// 滿{多少}金額；可花費的上限，滿額的{多少}%
			return await _service.GetCostHCoinRule();
		}

		// 取得會員的總HCoin金額
		[Authorize]
		[HttpGet("TotalHCoin")]
		public async Task<int> GetTotalHCoin()
		{
			int memberId = _member_id;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				return await _service.GetTotalHCoinByMemberId(memberId);
			}
		}

		// 將會員花費的HCoin記錄到
		// POST api/<HCoinController>
		[Authorize]
		[HttpPost("CostHCoin")]
		public void PostCostHCoin([FromBody] int value)
		{
			int memberId = _member_id;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				// 將會員花費的HCoin記錄到會員的活動中
				_service.PostCostHCoinByMemberId(memberId, value);
			}
		}

		//// DELETE api/<HCoinController>/5
		//[HttpDelete("{id}")]
		//public void Delete(int id)
		//{
		//}
	}
}
