using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HStyleApi.Models.DTOs.ECommentLikesDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class EssayController : ControllerBase
	{
		private EssayService _service;
		private readonly int _memberId;
		public EssayController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_service = new EssayService(db);
			var claims = httpContextAccessor.HttpContext.User.Claims;
			if (claims.Any())
			{
				var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
				_memberId = memberid;
			}
		}
		// GET: api/<EssayController>
		[HttpGet]
		//FromQuery  =傳value篩選 代 service rpst
		public async Task<ActionResult<IEnumerable<EssayDTO>>> GetEssays([FromQuery] string? keyword)
		{
			try
			{
				IEnumerable<EssayDTO> data = await _service.GetEssays(keyword);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			//return data;
		}

		// GET api/<EssayController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<EssayDTO>> GetEssay(int id)
		{

			try
			{
				EssayDTO data = await _service.GetEssays(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET api/<VideoController>/5  
		// 推薦商品
		[HttpGet("Recommenations/{id}")]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetRecommendationProduct(int id)
		{
			IEnumerable<ProductDto> products = await _service.GetRecommendationProduct(id);
			return Ok(products);
		}


		//[HttpGet("News")] 
		[HttpGet("News")]
		public async Task<ActionResult<IEnumerable<EssayDTO>>> GetNews()
		{
			try
			{
				IEnumerable<EssayDTO> data = await _service.GetNews();
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		// GET api/<EssayController>/EssayLike/5
		[Authorize]
		[HttpGet("Elike")]
		public async Task<IEnumerable<EssayDTO>> GetlikeEssays()
		{
			var memberId = _memberId;
			if (memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				return await _service.GetlikeEssays(memberId);
			}
		}
		// POST api/<EssayController>
		[Authorize]
		[HttpPost("Elike /{essayId}")]
		public void PostELike( int essayId)
		{
			var memberId = _memberId;
			if (memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				_service.PostELike(memberId, essayId);
			}
		}

		//GET api/<EssayController>/5 
		//GET 所有評論
		[HttpGet("Comments/{essayId}")]
		public async Task<IEnumerable<EssayCommentDTO>> GetComments(int essayId)
		{
			return await _service.GetComments(essayId);
		}

		//POST api/<VideoController>/Comment/5
		//POST 評論
		[Authorize]
		[HttpPost("Comment/{essayId}")]
		public void CreateComment([FromBody] CommentDTO comment, int essayId)
		{
			var memberId = _memberId;
			if(memberId <= null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
             _service.CreateComment(comment.comment, memberId,essayId);
			}
			
		}

		//POST api/<VideoController>/CommentLike
		[Authorize]
		[HttpPost("CommentLike/{ecommentId}")]
		public void PostCommentLike(int ecommentId)
		{
			var memberId = _memberId;
			if(memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				_service.PostCommentLike(memberId, ecommentId);
			}
		}

		//TODO 抓到使用者按讚的留言
		//GET api/<VideoController>/Comment/Likes
		[Authorize]
		[HttpGet("comment/Likes")]
        public async Task<IEnumerable<ECommentLikesDTO>> GetCommentLikes()
		{
			var memberId = _memberId;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				return await _service.GetECommentLikes(memberId);

			}
		}
	}
}
