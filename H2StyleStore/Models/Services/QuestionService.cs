using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{
	public class QuestionService
	{
		private IQuestionRepository _questionRepository;
		public QuestionService(IQuestionRepository questionRepository)
		{
			_questionRepository = questionRepository;
		}

		public IEnumerable<CommonQuestionDTO> GetAllCommonQuestion()
		{
			return _questionRepository.GetAllCommonQuestion();
		}

		public (bool IsSuccess, string ErrorMessage) CreateCommonQ(CommonQuestionDTO dto)
		{
			_questionRepository.CreateCommonQ(dto);
			return (true, null);
		}

		public void UpdateCommonQ(CommonQuestionDTO dto)
		{
			_questionRepository.UpdateCommonQ(dto);
		}

		public void DeleteCommonQ(int commonQId)
		{
			_questionRepository.DeleteCommonQ(commonQId);
		}

		public IEnumerable<CustomerQuestionDTO> GetAllCustomerQuestion()
		{
			return _questionRepository.GetAllCustomerQuestion();
		}

		public CustomerQuestionDTO GetCustomerQById(int id)
		{
			return _questionRepository.GetCustomerQById(id);
		}

		public void UpdatetCustomerQ(CustomerQuestionDTO dto)
		{
			_questionRepository.UpdatetCustomerQ(dto);
		}
	}
}