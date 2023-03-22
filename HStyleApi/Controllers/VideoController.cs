using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class VideoController : ControllerBase
	{
		private VideoService _service;
		private readonly int _memberId;

		public VideoController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_service = new VideoService(db);
			var claims = httpContextAccessor.HttpContext.User.Claims;
			if (claims.Any())
			{
				var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
				_memberId = memberid;
			}
		}
		// GET: api/<VideoController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideos([FromQuery] string? keyword)
		{

			try
			{
				IEnumerable<VideoDTO> data = await _service.GetVideos(keyword);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<VideoController>/5  
		[HttpGet("{videoId}")]
		public async Task<ActionResult<VideoDTO>> GetVideo(int videoId)
		{
			try
			{
				VideoDTO data = await _service.GetVideo(videoId);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<VideoController>/5  
		[HttpGet("Recommenations/{videoId}")]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetRecommendationProduct(int videoId)
		{
			IEnumerable<ProductDto> products = await _service.GetRecommendationProduct(videoId);
			return Ok(products);
		}

		// GET api/<VideoController>/5 
		[HttpGet("News")]
		public async Task<ActionResult<IEnumerable<VideoDTO>>> GetNews()
		{
			try
			{
				IEnumerable<VideoDTO> data = await _service.GetNews();
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		//GET api/<VideoController>/MyLike/5  
		[Authorize]
		[HttpGet("MyLike")]
		public async Task<IEnumerable<VideoLikeDTO>> GetLikeVideos()
		{
			var memberId = _memberId;
			if (memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				return await _service.GetLikeVideos(memberId);
			}
		}

		// POST api/<VideoController>/Like
		[Authorize]
		[HttpPost("Like/{videoId}")]
		public void PostLike(int videoId)
		{
			var memberId = _memberId;
			if (memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				_service.PostLike(memberId, videoId);
			}
		}

		// POST api/<VideoController>/View/5
		[HttpPost("View/{videoId}")]
		public void PostView(int videoId)
		{
			_service.PostView(videoId);
		}

		//GET api/<VideoController>/5 
		//GET 所有評論
		[HttpGet("Comments/{videoId}")]
		public async Task<IEnumerable<VideoCommentDTO>> GetComments(int videoId)
		{
			return await _service.GetComments(videoId);
		}

		//POST api/<VideoController>/Comment/5
		//POST 評論
		[Authorize]
		[HttpPost("Comment/{videoId}")]
		public void CreateComment([FromBody] CommentDTO comment, int videoId)
		{
			var memberId = _memberId;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				_service.CreateComment(comment.comment, memberId, videoId);
			}
		}

		//TODO 抓到使用者按讚的留言
		//GET api/<VideoController>/Comment/Likes
		[Authorize]
		[HttpGet("comment/Likes")]
		public async Task<IEnumerable<VCommentLikeDTO>> GetCommentLikes()
		{
			var memberId = _memberId;
			if (memberId <= 0)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				return await _service.GetCommentLikes(memberId);
				
			}
		}

		//POST api/<VideoController>/CommentLike
		[Authorize]
		[HttpPost("CommentLike/{commentId}")]
		public void PostCommentLike(int commentId)
		{
			var memberId = _memberId;
			if (memberId == null)
			{
				throw new Exception("請先登入會員");
			}
			else
			{
				_service.PostCommentLike(memberId, commentId);
			}
		}
	}
	public class CommentDTO
	{
		public string comment { get; set; }
	}
}
