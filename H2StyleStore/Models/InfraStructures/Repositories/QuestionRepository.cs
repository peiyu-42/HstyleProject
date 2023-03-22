using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class QuestionRepository : IQuestionRepository
	{
		private AppDbContext _db;
		public QuestionRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<CommonQuestionDTO> GetAllCommonQuestion()
		{
			IEnumerable<CommonQuestionDTO> question = _db.CommonQuestions.OrderBy(q => q.QCategory_Id).ToList().Select(q => q.ToDTO());
			return question;
		}

		public void CreateCommonQ(CommonQuestionDTO dto)
		{
			CommonQuestion question = new CommonQuestion
			{
				QCategory_Id = dto.QCategory_Id,
				Question = dto.Question,
				Answer = dto.Answer,
			};
			_db.CommonQuestions.Add(question);
			_db.SaveChanges();
		}

		public IEnumerable<SelectListItem> GetQCategories(int? categoryId)
		{
			var data = _db.Question_Categories.Select(q => new SelectListItem
			{ Value = q.QCategory_Id.ToString(), Text = q.Category_Name, Selected = (categoryId.HasValue && q.QCategory_Id == categoryId) })
				.ToList().Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇" });
			return data;
		}

		public IEnumerable<SelectListItem> GetQCategories()
		{
			var data = _db.Question_Categories;
			foreach (var item in data)
			{
				yield return new SelectListItem
				{ Value = item.QCategory_Id.ToString(), Text = item.Category_Name };
			}
		}

		public CommonQuestionDTO GetCommonQById(int? commonQId)
		{
			if (!commonQId.HasValue) { throw new Exception("找不到這一筆紀錄"); }

			CommonQuestionDTO data = _db.CommonQuestions
				.FirstOrDefault(q => q.CommonQuestion_Id == commonQId).ToDTO();
			return data;
		}

		public void UpdateCommonQ(CommonQuestionDTO dto)
		{
			CommonQuestion question = _db.CommonQuestions.Find(dto.CommonQuestion_Id);

			question.QCategory_Id = dto.QCategory_Id;
			question.Question = dto.Question;
			question.Answer = dto.Answer;

			_db.SaveChanges();
		}

		public void DeleteCommonQ(int commonQId)
		{
			CommonQuestion question = _db.CommonQuestions.Find(commonQId);

			_db.CommonQuestions.Remove(question);
			_db.SaveChanges();
		}

		public IEnumerable<CustomerQuestionDTO> GetAllCustomerQuestion()
		{
			IEnumerable<CustomerQuestionDTO> question = _db.CustomerQuestions
				.OrderBy(q => q.Solution_Description)
				.ThenBy(q => q.AskTime).ThenBy(q => q.QCategory_Id)
				.ToList().Select(q => q.ToDTO());
			return question;
		}

		public CustomerQuestionDTO GetCustomerQById(int? customerQId)
		{
			if (!customerQId.HasValue) { throw new Exception("找不到這一筆紀錄"); }

			CustomerQuestionDTO data = _db.CustomerQuestions
				.FirstOrDefault(q => q.CustomerQuestion_Id == customerQId).ToDTO();
			return data;
		}

		public void UpdatetCustomerQ(CustomerQuestionDTO dto)
		{
			CustomerQuestion question = _db.CustomerQuestions.Find(dto.CustomerQuestion_Id);

			question.Solution_Description = dto.Solution_Description;
			question.SolveTime = DateTime.Now;
			question.Employee_Id = _db.Employees.Where(x => x.Account == dto.Employee_Name).FirstOrDefault().Employee_id;
			_db.SaveChanges();
		}
	}
}
