using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class QuestionCategoryDTO
	{
		public int QcategoryId { get; set; }
		public string? CategoryName { get; set; }
	}

	public static class QuestionCategoryDTOExts
	{
		public static QuestionCategoryDTO ToQCDTO(this QuestionCategory source)
		{
			return new QuestionCategoryDTO
			{
				QcategoryId = source.QcategoryId,
				CategoryName = source.CategoryName,
			};
		}
	}
}
