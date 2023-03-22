using HStyleApi.Models.EFModels;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace HStyleApi.Models.DTOs
{
	public class PCommentDTO
	{
		public int CommentId { get; set; }
		public string Color { get; set; }
		public string Size { get; set; }
		public string CommentContent { get; set; }
		public int Score { get; set; }
		public string CreatedTime { get; set; }

		public string Account { get; set; }

		public IEnumerable<string> PcommentImgs { get; set; }
	}

	public static class ProductCommentExts
	{
		private static readonly object _basePath = "https://localhost:44313/Images";
		public static PCommentDTO ToDto(this ProductComment source)
		{
			return new PCommentDTO
			{
				CommentId = source.CommentId,
				CommentContent = source.CommentContent,
				PcommentImgs = source.PcommentImgs.Select(x => $"{_basePath}/PCommentImages/{x.Path}"),
				Score = source.Score,
				Account = source.Order.Member.Account,
				Color = source.Order.OrderDetails.Where(x => x.OrderId == source.OrderId).FirstOrDefault(x => x.ProductId == source.ProductId).Color,
				Size = source.Order.OrderDetails.Where(x => x.OrderId == source.OrderId).FirstOrDefault(x => x.ProductId == source.ProductId).Size,
				CreatedTime = source.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
			};

		}
	}
}
