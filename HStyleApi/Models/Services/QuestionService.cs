using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HStyleApi.Models.Services
{
	public class QuestionService
	{
		private QuestionRepository _questionRepository;

		public QuestionService(AppDbContext db)
		{
			_questionRepository = new QuestionRepository(db);
		}

		public async Task<IEnumerable<CommonQuestionDTO>> GetCommonQuestion(string? keyword)
		{
			var data = _questionRepository.GetCommonQuestion(keyword);
			return await data;
		}

		public async Task<IEnumerable<QuestionCategoryDTO>> GetQuestionCategory()
		{
			var data = _questionRepository.GetQuestionCategory();
			return await data;
		}

		public async Task<IEnumerable<MemberResponseDTO>> GetMemberQResponse(int memberId)
		{
			var data = _questionRepository.GetMemberQResponse(memberId);
			return await data;
		}

		public void PostCustomerQuestion(CustomerQuestionDTO dto)
		{
			if (dto == null)
			{
				throw new Exception();
			}
			else
			{
				_questionRepository.PostCustomerQuestion(dto);
			}
		}

		public void PutSatisfactionYes(int CommonQId)
		{
			_questionRepository.PutSatisfactionYes(CommonQId);
		}

		public void PutSatisfactionNo(int CommonQId)
		{
			_questionRepository.PutSatisfactionNo(CommonQId);
		}


	}
}
