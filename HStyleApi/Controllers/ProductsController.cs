using H2StyleStore.Models.Infrastructures;
using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.Xml;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ProductServices _Service;
		private readonly int _member_id;
		private readonly AppDbContext _db;

		public ProductsController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_Service = new ProductServices(db);
			_db = db;
            var claims = httpContextAccessor.HttpContext.User.Claims;
            if (claims.Any())
            {
                var data = int.TryParse(claims.Where(x => x.Type == "MemberId").FirstOrDefault().Value, out int memberid);
                _member_id = memberid;
            }
        }

		[HttpGet("test")]
		public dynamic GetTags(int product_id)
		{
			var orders = _db.Orders.Where(x => x.MemberId == _member_id).OrderByDescending(x => x.OrderId).ToList().Take(5);

			var productsId = orders.Select(x => x.OrderDetails.Select(x => x.ProductId)).ToArray();
			List<int> Ordersproducts = new List<int>();
			foreach (var order in productsId)
			{
				foreach (var pId in order)
				{
					Ordersproducts.Add(pId);
				}
			}
			var dbPro = _db.Products.Include(x => x.Tags);


			var tags = new List<int>();
			foreach (var product in Ordersproducts)
			{
				var ts = dbPro.Where(x => x.ProductId == product).SingleOrDefault().Tags;
				foreach (var t in ts)
				{
					tags.Add(t.Id);
				}
			}

			Dictionary<int, int> tagsCount = new Dictionary<int, int>();

			foreach (var id in tags)
			{
				if (tagsCount.ContainsKey(id))
				{
					tagsCount[id]++;
				}
				else
				{
					tagsCount.Add(id, 1);
				}
			}

			var maxValueKey = tagsCount.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

			var products = dbPro.Where(x => !Ordersproducts.Contains(x.ProductId));

			


			return products;

		}

		// 商品總覽(全部商品)
		[HttpGet("products")]
		public ActionResult<ProductDto> LoadProducts([FromQuery] string? keyword)
		{
			IEnumerable<ProductDto> data;
			try
			{
				data = _Service.LoadProducts(keyword);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}

		//單一商品頁
		[HttpGet("{product_id}")]
		public ActionResult GetProduct(int product_id)
		{
			ProductDto data;
			try
			{
				 data = _Service.GetProduct(product_id);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(data);

		}

		//新品頁
		[HttpGet("newProducts")]
		public ActionResult<ProductDto> LoadNewProducts(string tag)
		{
			IEnumerable<ProductDto> data;
			List<string> tags = new List<string>();
			try
			{
				tags.Add(tag);
				data = _Service.GetNewProducts(tags);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(data);

		}


		//依類別分類
		[HttpGet("PCategory/{PCategoryName}")]
		public ActionResult<ProductDto> LoadPCategory(string PCategoryName)
		{
			IEnumerable<ProductDto> data;
			try
			{
				data = _Service.LoadProductsByCategory(PCategoryName);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}


		//商品收藏
		[Authorize]
		[HttpPost("product/like")]
		public ActionResult LikesProduct (int product_id)
		{
			_Service.LikesProduct(product_id, _member_id);
			return Ok();
		}

		//商品收藏瀏覽
		[Authorize]
		[HttpGet("products/likes")]
		public ActionResult LoadLikeProducts()
		{
			var data = _Service.LoadLikeProducts(_member_id);

			return Ok(data);
		}

		//商品推薦(根據此商品特徵篩選) //用在你可能會喜歡(商品頁)
		[HttpGet("ProdRec/{product_id}")]
		public ActionResult<ProductDto> ProductRecommend(int product_id)
		{
			IEnumerable<ProductDto> data;
			try
			{
				data = _Service.GetRecommendByProducts(product_id);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}		

			return Ok(data);
		}

		//會員專屬推薦
		[Authorize]
		[HttpGet("MemberRec")]
		public ActionResult<ProductDto> OrderRecommend()
		{
			IEnumerable<ProductDto> data;
			try
			{
				data = _Service.GetRecommendByOrder(_member_id);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}

		[HttpGet("NewRec")]
		public ActionResult<ProductDto> newProductsRecommend()
		{
			IEnumerable<ProductDto> data;
			try
			{
				data = _Service.GetNewProductsRecommend();
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}


		//評論頁
		[HttpGet("comment")]
		public ActionResult GetComment(int orderId,  int productId)
		{
			PCommentGetDTO data;
			try
			{
				data = _Service.GetComment(productId, orderId);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
			return Ok(data);
		}

		//新增評論++
		[Authorize]
		[HttpPost("comment")]
		public IActionResult CreateComment([FromForm] PCommentPostDTO comment, int orderId, int productId)
		{
			string path = "../H2StyleStore/Images/PCommentImages/";

			comment.PcommentImgs = new List<string>();

			if (comment.files != null)
			{
				foreach (var file in comment.files)
				{
					try
					{
						if (file.Length > 0)
						{
							var helper = new UploadFileHelper();
							string result = helper.SaveAs(path, file);
							string FileName = result;
							comment.PcommentImgs.Add($"{FileName}");
						}
					}
					catch (Exception ex)
					{

						return BadRequest(ex.Message);
					}

				}
			}
			
			var data = _Service.CreateComment(comment, orderId, productId);

			return Ok("新增成功");
		}

		//評論是否有幫助
		[Authorize]
		[HttpPost("helpfulComment")]
		public ActionResult HelpfulComment(int comment_id)
		{
			_Service.HelpfulComment(comment_id, _member_id);

			return Ok();
		}

		//這個會員有按過哪些評論有幫助
		[Authorize]
		[HttpGet("helpfulComment/member")]
		public ActionResult<HelpfulDTO> LoadHelpfulComment()
		{
			 IEnumerable<HelpfulDTO> data;

			try
			{
				data = _Service.LoadHelpfulComment(_member_id);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}

		[HttpGet("AllhelpfulComments")]
		public ActionResult<HelpfulDTO> AllhelpfulComments(int commentId)
		{
			IEnumerable<HelpfulDTO> data;

			try
			{
				data = _Service.AllhelpfulComments(commentId);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}


		//所有評論瀏覽
		[HttpGet("comments/{product_id}")]
		public ActionResult<PCommentDTO> LoadComments(int product_id)
		{
			IEnumerable<PCommentDTO> data;
			try
			{
				 data = _Service.LoadComments(product_id);	
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return Ok(data);
		}

	}
}
