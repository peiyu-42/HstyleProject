using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class MemberResponseDTO
	{
		public int CustomerQuestionId { get; set; }
		public int? MemberId { get; set; }
		public int QcategoryId { get; set; }
		public string? Title { get; set; }
		public string? ProblemDescription { get; set; }
		public string? FilePath { get; set; }
		public DateTime AskTime { get; set; }
		public string SolutionDescription { get; set; }
		public DateTime? SolveTime { get; set; }
		//public int? EmployeeId { get; set; }
	}

	public static class MemberResponseDTOExts
	{
		public static MemberResponseDTO ToMemberResponseDTO(this CustomerQuestion source)
		{
			return new MemberResponseDTO
			{
				//CustomerQuestionId = dto.CustomerQuestionId,
				MemberId = source.MemberId,
				QcategoryId = source.QcategoryId,
				Title = source.Title,
				ProblemDescription = source.ProblemDescription,
				FilePath = source.FilePath,
				AskTime = source.AskTime,
				SolutionDescription = source.SolutionDescription,
				//SolveTime = source.SolveTime,
			};
		}
	}
}
