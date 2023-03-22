using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HStyleApi.Models.InfraStructures.Repositories
{
	public class ProductRepo
	{
		private readonly AppDbContext _db;
		public ProductRepo(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<ProductDto> LoadProducts(string? keyword)
		{
			IEnumerable<Product> data = _db.Products
										.Include(product => product.Category)
										.Include(product => product.Imgs)
										.Include(product => product.Specs)
										.Include(product => product.Tags).Where(x => x.Discontinued == false);

			IEnumerable<ProductDto> products;
			if (keyword == null)
			{

				products = data.OrderBy(x => x.DisplayOrder).Select(x => x.ToDto());
			}
			else
			{
				products = data.Where(x => x.ProductName.Contains(keyword))
								   .OrderBy(x => x.DisplayOrder).Select(x => x.ToDto());
			}

			return products;
		}

		public ProductDto GetProduct(int product_id)
		{
			if (_db.Products.Find(product_id) == null)
			{
				throw new Exception("查無此商品");
			}

			IEnumerable<Product> data = _db.Products.Include(product => product.Category)
										.Include(product => product.Imgs)
										.Include(product => product.Specs)
										.Include(product => product.Tags);

			var product = data.FirstOrDefault(x => x.ProductId == product_id).ToDto();


			return product;

		}

		public void CreateComment(PCommentPostDTO dto, int orderId, int productId)
		{

			ProductComment pcomment = new ProductComment
			{
				OrderId = orderId,
				ProductId = productId,
				CommentContent = dto.CommentContent,
				Score = dto.Score,
				CreatedTime = DateTime.Now,
			};


			_db.ProductComments.Add(pcomment);

			if (dto.files != null)
			{
				foreach (string path in dto.PcommentImgs)
				{
					Image image = new Image { Path = path, };
					pcomment.PcommentImgs.Add(image);
				}
			}

			_db.SaveChanges();
		}

		public PCommentGetDTO GetComment(int productId, int orderId)
		{
			var data = _db.Products.Include(product => product.Category)
										.Include(product => product.Imgs)
										.Include(product => product.Specs)
										.Include(product => product.Tags).FirstOrDefault(x => x.ProductId == productId);

			var product = data.ToDto();

			var orderDetail = _db.OrderDetails.Where(x => x.OrderId == orderId).FirstOrDefault(x => x.ProductId == productId);

			PCommentGetDTO comment = new PCommentGetDTO
			{
				ProductId = productId,
				OrderId = orderId,
				ProductName = product.Product_Name,
				ProductPhoto = product.Imgs.ToList()[0],
				Color = orderDetail.Color,
				Size = orderDetail.Size,
			};

			return comment;

		}

		public void HelpfulComment(int comment_id, int member_id)
		{
			var data = _db.PcommentsHelpfuls.Where(x => x.CommentId == comment_id).FirstOrDefault(x => x.MemberId == member_id);

			if (data == null)
			{
				PcommentsHelpful source = new PcommentsHelpful
				{
					CommentId = comment_id,
					MemberId = member_id
				};
				_db.PcommentsHelpfuls.Add(source);
			}
			else
			{
				_db.PcommentsHelpfuls.Remove(data);
			}

			_db.SaveChanges();
		}

		public IEnumerable<PCommentDTO> LoadComments(int product_id)
		{
			IEnumerable<ProductComment> data = _db.ProductComments.Include(x => x.PcommentImgs)
																  .Include(x => x.Product).Where(x => x.ProductId == product_id)
																  .Include(x => x.Order).ThenInclude(x => x.Member)
																  .Include(x => x.Order).ThenInclude(x => x.OrderDetails);

			var comments = data.Select(x => x.ToDto());


			return comments;
		}

		public void LikesProduct(int product_id, int member_id)
		{
			var data = _db.ProductLikes.Where(x => x.ProductId == product_id).FirstOrDefault(x => x.MemberId == member_id);
			if (data == null)
			{
				ProductLike source = new ProductLike
				{
					ProductId = product_id,
					MemberId = member_id
				};
				_db.ProductLikes.Add(source);
			}
			else
			{
				_db.ProductLikes.Remove(data);
			}

			_db.SaveChanges();
		}

		public IEnumerable<Product_LikeDTO> LoadLikeProducts(int member_id)
		{
			IEnumerable<ProductLike> data = _db.ProductLikes.Include(x => x.Product).ThenInclude(x => x.Imgs)
															.Where(x => x.MemberId == member_id && x.Product.Discontinued == false);

			var productsLike = data.Select(x => x.ToDto());

			return productsLike;
		}

		public int GetOrderMaxTag(int member_id)
		{
			//此會員的所有訂單的商品Id
			var orders = _db.Orders.Where(x => x.MemberId == member_id);
			var productsId = orders.Select(x => x.OrderDetails.Select(x => x.ProductId)).ToArray();

			List<int> ordersproducts = new List<int>();
			foreach (var order in productsId)
			{
				foreach (var pId in order)
				{
					if (pId != 37) //新品標籤
					{
						ordersproducts.Add(pId);
					}
					
				}
			}
			var dbPro = _db.Products.Include(x => x.Tags);

			//取得次數最高的tag
			var tags = new List<int>();
			foreach (var product in ordersproducts)
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

			var maxValueTag = tagsCount.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;


			return maxValueTag;

		}

		public Product GetProductInfo(int product_id)
		{
			var data = _db.Products.Include(x => x.Tags).Include(x => x.Specs)
								   .FirstOrDefault(x => x.ProductId == product_id);

			return data;

		}

		public List<int> GetProductByTags(IEnumerable<int> data, int product_id)
		{
			var products = _db.Products.Include(x => x.Tags).Where(x => x.ProductId != product_id);


			var products_id = products
	                         .Where(item => item.Tags.Any(x => data.Contains(x.Id)))
	                         .Select(item => item.ProductId)
	                         .Distinct()
	                         .ToList();


			return products_id;
		}

		public IEnumerable<ProductDto> GetProducts(List<int> recommendlist)
		{
			IEnumerable<Product> data = _db.Products
										.Include(product => product.Category)
										.Include(product => product.Imgs)
										.Include(product => product.Specs)
										.Include(product => product.Tags);

			List<ProductDto> source = new List<ProductDto>();

			foreach (var id in recommendlist)
			{
				var product = data.FirstOrDefault(x => x.ProductId == id).ToDto();

				source.Add(product);
			}

			return source;
		}

		public List<int> GetProductByColor(IEnumerable<string> colors, List<int> list_id)
		{
			var products = _db.Products.Include(x => x.Specs).Where(x => !list_id.Contains(x.ProductId));


			if (products == null)
			{
				return null;
			}

			List<int> products_id = new List<int>();

			foreach (var item in products)
			{
				foreach (var color in colors)
				{
					if (item.Specs.Any(x => x.Color.Contains(color.Replace("色", string.Empty)) == true))
					{
						products_id.Add(item.ProductId);
					}
				}
			}
			return products_id;
		}

		public List<int> GetProductsByOrder(int maxvaluetag, int member_id)
		{
			//最新一筆order
			var orders = _db.Orders.Where(x => x.MemberId == member_id).ToList().TakeLast(1);

			//沒買過東西推新品給他
			List<int> newproducts = new List<int>();
			if (orders == null)
			{
				newproducts = _db.Products.Where(x => x.Tags.Any(t => t.TagName.Contains("新品"))).Select(x => x.ProductId).ToList();
				return newproducts;
			};


			//最多購買的商品
			var productsId = orders.SelectMany(x => x.OrderDetails.Select(d => d.ProductId));

			List<int> ordersproducts = productsId.Distinct().ToList();

			//推給顧客有最多購買商品的tag項目
			var dbPro = _db.Products.Where(x => !ordersproducts.Contains(x.ProductId)).Where(x => x.Tags.Select(x => x.Id).Contains(maxvaluetag));

			var products = new List<int>();

			foreach (var item in dbPro)
			{
				products.Add(item.ProductId);
			}

			return products;
		}

		public List<int> GetProductsByTagsName(List<string> tagsName)
		{
			
			var tags = _db.Tags.Include(x => x.Products).Where(x => tagsName.Contains(x.TagName)).Select(x => x.Id).ToList();

			var data = _db.Products.Include(x => x.Tags).Select(x => new
	                    {
		                  productId = x.ProductId,
		                  isRecomended = x.Tags.Select(x => x.Id).ToList()
	                     }).ToList().Where(x => x.isRecomended.Intersect(tags).Any()).Select(x => x.productId).ToList();

			return data;
		}

		public IEnumerable<ProductDto> LoadProductsByCategory(string pCategoryName)
		{
			IEnumerable<Product> data = _db.Products
										.Include(product => product.Category)
										.Include(product => product.Imgs)
										.Include(product => product.Specs)
										.Include(product => product.Tags).Where(x => x.Discontinued == false)
										.Where(x => x.Category.PcategoryName == pCategoryName);

			var products = data.OrderBy(x => x.DisplayOrder).Select(x => x.ToDto());


			return products;

		}

		public IEnumerable<HelpfulDTO> LoadHelpfulComment(int member_id)
		{
			IEnumerable<PcommentsHelpful> data = _db.PcommentsHelpfuls.Where(x => x.MemberId == member_id);

			var result = data.Select(x => x.ToDto());


			return result;
		}

		public IEnumerable<HelpfulDTO> AllhelpfulComments(int commentId)
		{
			IEnumerable<PcommentsHelpful> data = _db.PcommentsHelpfuls.Where(x => x.CommentId == commentId);

			var result = data.Select(x => x.ToDto());


			return result;
		}
	}
}
