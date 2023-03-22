using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace HStyleApi.Models.InfraStructures.Repositories
{
	public class QuestionRepository
	{
		private AppDbContext _db;

		public QuestionRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<CommonQuestionDTO>> GetCommonQuestion(string? keyword)
		{
			if (keyword==null)
			{
				IEnumerable<CommonQuestionDTO> question = await _db.CommonQuestions
					.Select(q => q.ToCommonQuestionDTO()).ToListAsync();
				return question;
			}
			else
			{
				IEnumerable<CommonQuestionDTO> question = await _db.CommonQuestions
				.Where(q => q.Question.Contains(keyword) || q.Answer.Contains(keyword))
				.Select(q => q.ToCommonQuestionDTO()).ToListAsync();
				return question;
			}
		}

		public async Task<IEnumerable<QuestionCategoryDTO>> GetQuestionCategory()
		{
			var data = await _db.QuestionCategories.Select(q => q.ToQCDTO()).ToListAsync();

			return data;
		}

		public async Task<IEnumerable<MemberResponseDTO>> GetMemberQResponse(int memberId)
		{
			var data = await _db.CustomerQuestions.Where(q=>q.MemberId== memberId)
				.Select(q => q.ToMemberResponseDTO()).ToListAsync();
			return data;
		}

		public void PostCustomerQuestion(CustomerQuestionDTO dto)
		{
			CustomerQuestion question = dto.ToCustomerQ();
			_db.CustomerQuestions.Add(question);
			_db.SaveChanges();
		}

		public void PutSatisfactionYes(int CommonQId)
		{
			CommonQuestion question = _db.CommonQuestions.SingleOrDefault(q => q.CommonQuestionId == CommonQId);
			if (question == null) throw new Exception("找不到紀錄");
				
			question.SatisfactionYes += 1;
			_db.SaveChanges();
		}

		public void PutSatisfactionNo(int CommonQId)
		{
			CommonQuestion question = _db.CommonQuestions.SingleOrDefault(q => q.CommonQuestionId == CommonQId);
			if (question == null) throw new Exception("找不到紀錄");

			question.SatisfactionNo += 1;
			_db.SaveChanges();
		}
	}
}
