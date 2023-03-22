using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class CustomerQuestionDTO
	{
		public int CustomerQuestionId { get; set; }
		public int? MemberId { get; set; }
		public int QcategoryId { get; set; }
		public string? Title { get; set; }
		public string? ProblemDescription { get; set; }
		public string? FilePath { get; set; }
		public IFormFile? file { get; set; }
		public DateTime AskTime { get; set; }

		//public string SolutionDescription { get; set; }
		//public DateTime? SolveTime { get; set; }
		//public int? EmployeeId { get; set; }
	}

	public static class CustomerQuestionDTOExts
	{
		public static CustomerQuestion ToCustomerQ(this CustomerQuestionDTO dto)
		{
			return new CustomerQuestion
			{
				//CustomerQuestionId = dto.CustomerQuestionId,
				MemberId = dto.MemberId,
				QcategoryId = dto.QcategoryId,
				Title = dto.Title ?? "",
				ProblemDescription = dto.ProblemDescription ?? "",
				FilePath = dto.FilePath ?? "",
				AskTime = dto.AskTime,
			};
		}
	}
}
