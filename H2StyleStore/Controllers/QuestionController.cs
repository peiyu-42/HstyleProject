using H2StyleStore.filter;
using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("2", "3")]
	public class QuestionController : Controller
	{
		private QuestionService _questionService;
		private IQuestionRepository _questionRepo;
		public QuestionController()
		{
			var db = new AppDbContext();
			IQuestionRepository repo = new QuestionRepository(db);
			_questionService = new QuestionService(repo);
			_questionRepo = new QuestionRepository(db);

		}

		// GET: Question
		public ActionResult Index(string currentFilter, string searchString, int? page
			, int? qcategoryId, string keyWord)
		{

			// 將篩選條件放在ViewBag,稍後在 view page取回
			ViewBag.QCategory = _questionRepo.GetQCategories(qcategoryId);
			ViewBag.KeyWord = keyWord;

			if (searchString != null) { page = 1; }
			else { searchString = currentFilter; }
			ViewBag.CurrentFilter = searchString;

			IEnumerable<CommonQuestionDTO> data = _questionService.GetAllCommonQuestion();

			// 若有篩選 qcategoryId
			if (qcategoryId.HasValue) data = data.Where(q => q.QCategory_Id == qcategoryId.Value);
			// 若有篩選 keyWord
			if (string.IsNullOrEmpty(keyWord) == false)
			{
				data = data.Where(q => q.Question.Contains(keyWord)
				|| q.Answer.Contains(keyWord));
			}

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			return View(data.Select(q => q.ToVM()).ToPagedList(pageNumber, pageSize));
		}

		public ActionResult CreateCommonQ()
		{
			ViewBag.QCategoryItems = _questionRepo.GetQCategories(null);
			return View();
		}

		[HttpPost]
		public ActionResult CreateCommonQ(CreateCommonQVM model)
		{
			ViewBag.QCategoryItems = _questionRepo.GetQCategories();
			try
			{
				(bool IsSuccess, string ErrorMessage) response = _questionService.CreateCommonQ(model.ToDto());
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid) return RedirectToAction("Index");

			return View(model);
		}

		public ActionResult EditCommonQ(int id)
		{
			CreateCommonQVM model = _questionRepo.GetCommonQById(id).ToEditVM();
			var qCategory = _questionRepo.GetQCategories(id).ToList();
			ViewBag.QCategoryItems = qCategory;

			if (model == null) return HttpNotFound();
			return View(model);
		}

		[HttpPost]
		public ActionResult EditCommonQ(CreateCommonQVM model)
		{
			var qCategory = _questionRepo.GetQCategories(model.QCategory_Id);
			ViewBag.QCategoryItems = qCategory;

			if (!ModelState.IsValid) return View(model);

			try
			{
				_questionService.UpdateCommonQ(model.ToDto());
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid) return RedirectToAction("Index");
			return View(model);
		}

		public ActionResult DeleteCommonQ(int? id)
		{
			if (id == null) return View("Index");

			CommonQuestionVM question = _questionRepo.GetCommonQById(id).ToVM();

			if (question == null) return HttpNotFound();

			return View(question);
		}

		[HttpPost, ActionName("DeleteCommonQ")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			_questionService.DeleteCommonQ(id);
			return RedirectToAction("Index");
		}

		public ActionResult GetCustomerQ(string currentFilter, string searchString, int? page
			, int? qcategoryId, string keyword)
		{

			// 將篩選條件放在ViewBag,稍後在 view page取回
			ViewBag.QCategory = _questionRepo.GetQCategories(qcategoryId);
			ViewBag.Keyword = keyword;

			if (searchString != null) { page = 1; }
			else { searchString = currentFilter; }
			ViewBag.CurrentFilter = searchString;

			IEnumerable<CustomerQuestionDTO> data = _questionService.GetAllCustomerQuestion();

			// 若有篩選 qcategoryId
			if (qcategoryId.HasValue) data = data.Where(q => q.QCategory_Id == qcategoryId.Value);
			// 若有篩選 keyword
			if (string.IsNullOrEmpty(keyword) == false)
			{
				data = data.Where(q => q.Title.Contains(keyword)
				|| q.Problem_Description.Contains(keyword));
			}

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			return View(data.Select(q => q.ToVM()).ToPagedList(pageNumber, pageSize));
		}

		public ActionResult EditCustomerQ(int id)
		{
			EditCustomerQVM model = _questionRepo.GetCustomerQById(id).ToEditVM();
			var qCategory = _questionRepo.GetQCategories(id).ToList();
			ViewBag.QCategoryItems = qCategory;

			if (model == null) return HttpNotFound();
			return View(model);
		}

		[HttpPost]
		public ActionResult EditCustomerQ(EditCustomerQVM model)
		{
			if (!ModelState.IsValid) return View(model);

			string user = User.Identity.Name;
			model.Employee_Name = user;

			var qCategory = _questionRepo.GetQCategories(model.QCategory_Id);
			ViewBag.QCategoryItems = qCategory;

			if (!ModelState.IsValid) return View(model);

			try
			{
				_questionService.UpdatetCustomerQ(model.ToDTO());
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid) return RedirectToAction("GetCustomerQ");
			return View(model);
		}
	}
}