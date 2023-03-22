using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.ComponentModel.Design;
using System.Xml.Linq;
using static HStyleApi.Models.DTOs.ECommentLikesDTO;

namespace HStyleApi.Models.InfraStructures.Repositories
{

	public class EssayReposity
	{
		private readonly ProductRepo _PRepo;
		public AppDbContext _db;

		public EssayReposity(AppDbContext db)
		{
			_db = db;
			_PRepo = new ProductRepo(db);
		}

		//文章列表
		public async Task<IEnumerable<EssayDTO>> GetEssays(string keyword)
		{
			IEnumerable<Essay> data = await _db.Essays.Include(e => e.Imgs)
				.Include(e => e.Tags)
				.Include(e => e.Elikes)
				.Include(e => e.Category)
				.Where(e => e.Pon == true)
				.ToListAsync();
			if (data == null)
			{
				throw new Exception("目前沒有文章");
			}
			if (keyword == null)
			{
				IEnumerable<EssayDTO> essays = data.Select(e => e.ToEssayDTO());
				return essays;
			}
			else
			{
				IEnumerable<EssayDTO> selectEssays = data.Where(x => x.Etitle.Contains(keyword) || x.Tags.Select(t => t.TagName).Contains(keyword)).Select(x => x.ToEssayDTO());
				return selectEssays;
			}
		}
		//單一文章
		public async Task<EssayDTO> GetEssay(int id)
		{
			IEnumerable<EssayDTO> data = await _db.Essays
				.Include(e => e.Imgs)
				.Include(e => e.Category)
				.Include(e => e.Tags)
				.Include(e => e.Elikes)
				.Include(e => e.Influencer)
				.Where(e => e.Pon == true).Select(e => e.ToEssayDTO())
				.ToListAsync();
			var essay = data.SingleOrDefault(e => e.EssayId == id);
			if (data == null)
			{
				throw new Exception("找不到任何内容！");
			}
		return essay;
		}

	//根據影片推薦相關商品
		public async Task<IEnumerable<ProductDto>> GetRecommendationProduct(int id)
		{
			var essayTags = _db.Essays
				.SingleOrDefault(v => v.EssayId == id).ToEssayDTO();
			//.Select(v => v.tags).ToList();
			var product = await _db.Products.Include(v => v.ProductLikes).ToListAsync();

			List<string> tags = new List<string>();
			foreach (var tag in essayTags.Tags)
			{
				tags.Add(tag);
			}
			var productId = _PRepo.GetProductsByTagsName(tags);
			List<Product> recoProduct = new List<Product>();
			foreach (int Id in productId)
			{
				var recoProductItem = product
					.FirstOrDefault(p => p.ProductId == Id);
				recoProduct.Add(recoProductItem);
			}

			return recoProduct.OrderByDescending(p => p.ProductLikes.Count())
				.Take(3).Select(p => p.ToDto()).ToList();
		}

        public async Task<IEnumerable<EssayDTO>> GetNews()
        {


            IEnumerable<Essay> dataE = await _db.Essays
                                                        .Include(e => e.Imgs)
														.Include(e => e.Category)
                                                        .Include(e => e.Tags)
														.ThenInclude(e => e.Videos)
                                                        .ToListAsync();

            IEnumerable<EssayDTO> newsE = dataE.Where(e => e.Pon == true).OrderByDescending(e => e.Tags.GroupBy(e => e.TagName).Count())
                                                        .OrderByDescending(e => e.UpLoad)
                                                        .Take(3).Select(e => e.ToEssayDTO()).ToList();
            return newsE;
        }

	

		//使用者收藏文章
		public async Task<IEnumerable<EssayDTO>> GetlikeEssays(int memberId)
		{
			IEnumerable<int> data = await _db.Elikes
				.Include(e => e.Essay)
				.ThenInclude(e => e.Imgs)//根據essay裡面撈資料 ThenInclude
				.Include(e => e.Essay)//關聯essay ==> tags
				.ThenInclude(e => e.Tags)
				.Include(e => e.Essay)
				.ThenInclude(e => e.Category)
				.Where(e => e.MemberId == memberId)
				.Select(x => x.EssayId)
				.ToListAsync();

			IEnumerable<EssayDTO> likeEssays = await _db.Essays
				.Include(e => e.Imgs)
				.Include(e => e.Category)
				.Include(e => e.Tags)
				.Include(e => e.Elikes)
				.Include(e => e.Influencer)
				.Where(e => data.Contains(e.EssayId)).Select(e => e.ToEssayDTO())
				.ToListAsync();

			
			return likeEssays;
		}
		//收藏文章
		public void PostELike(int memberId, int essayId)
		{
			var data = _db.Elikes.Where(e => e.MemberId == memberId).FirstOrDefault(e => e.EssayId == essayId);
			if (data == null)
			{
				Elike like = new Elike //like 新的容器，存入到資料表Elike，在一個新的Elike
				{
					MemberId = memberId,
					EssayId = essayId,
				};
				_db.Elikes.Add(like);
			}
			else _db.Elikes.Remove(data);
			_db.SaveChanges();
		}
		//Get 評論
		public async Task<IEnumerable<EssayCommentDTO>> GetComments(int essayId)
		{
			IEnumerable<EssayCommentDTO> data = await _db.EssaysComments.Include(e => e.Member)
										   .Where(e => e.EssayId==essayId)
										   .Select(e => e.ToCommentDTO()).ToListAsync();
			return data;
		}
		//Post評論
		public void CreateComment(string comment, int memberId, int essayId)
		{
			if (comment == null || memberId == 0 || essayId == 0) throw new Exception();

			EssaysComment essaysComment = new EssaysComment()
			{
				EssayId = essayId,
                MemberId = memberId,
                Ecomment = comment.ToString(),
                Etime = DateTime.Now,
                Elike = 0

			};
			_db.Add(essaysComment);
			_db.SaveChanges();
		}
		public void PostCommentLike(int memberId, int commentId)
		{
			var Ecommentdata = _db.EssaysComments.SingleOrDefault(e => e.CommentId == commentId);
			var data = _db.EcommentsLikes.SingleOrDefault(e => e.CommentId==commentId && e.MemberId == memberId);
			if (data== null)
			{
				EcommentsLike ecommentlike = new EcommentsLike()
				{
					MemberId = memberId,
					CommentId = commentId
				};
				_db.Add(ecommentlike);
				Ecommentdata.Elike ++;
			}
			else
			{
				_db.Remove(data);
				Ecommentdata.Elike--;
			}
			_db.SaveChanges();
		}

		//Get會員喜歡的評論
		public async Task<IEnumerable<ECommentLikesDTO>> GetECommentLikes(int memberId)
		{
			IEnumerable<ECommentLikesDTO> data = await _db.EcommentsLikes
													.Where(e => e.MemberId == memberId)
													.Select(e => e.ToECommentLikesDTO()).ToListAsync();

			return data;
		}
	}
}
