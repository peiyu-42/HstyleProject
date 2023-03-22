using H2StyleStore.filter;
using H2StyleStore.Models;
using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using PagedList;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("1", "3")]
	public class EssaysController : Controller
	{

		private EssayService essayService;
		private readonly IEssayRepository _essayRepository;
		private AppDbContext _db = new AppDbContext();

		public EssaysController()
		{
			var db = new AppDbContext();
			_essayRepository = new EssayRepository(db);
			IEssayRepository repo = new EssayRepository(db);
			this.essayService = new EssayService(repo);
		}
		// GET: Products
		public ActionResult Index(int? categoryId, string eTitle, string tagName,
			string sortOrder, string currentFilter, string searchString, int? page)
		{
			ViewBag.Categories = _essayRepository.GetCategoriesSelect(categoryId);
			ViewBag.EssayTitle = eTitle;
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TagName = tagName;
			//ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			//ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var data = essayService.GetEssays().Select(x => x.ToVM());

			//switch (sortOrder)
			//{
			//	case "name_desc":
			//		data = data.OrderByDescending(e => e.ETitle);
			//		break;
			//	case "Date_On":
			//		data = data.OrderBy(e => e.UpLoad);
			//		break;
			//	case "Date_Off":
			//		data = data.OrderBy(e => e.Removed);
			//		break;
			//	case "date_desc_On":
			//		data = data.OrderByDescending(e => e.UpLoad);
			//		break;
			//	case "date_desc_Off":
			//		data = data.OrderByDescending(e => e.Removed);
			//		break;
			//	default:  // Name ascending 
			//		data = data.OrderBy(e => e.Essay_Id);
			//		break;
			//}



			//var data = essayService.GetEssays().Select(x => x.ToVM());
			// 若有篩選categoryid
			if (categoryId.HasValue) data = data.Where(p => p.CategoryId == categoryId.Value);

			// 若有篩選 productName
			if (string.IsNullOrEmpty(eTitle) == false) data = data.Where(p => p.ETitle.Contains(eTitle));
			if (string.IsNullOrEmpty(tagName) == false) data = data.Where(p => p.Tags.Contains(tagName));

			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(data.OrderByDescending(x=>x.UpLoad).ToPagedList(pageNumber, pageSize));
		}

		//public ActionResult UploadEssay()
		//{
		//	ViewBag.PCategoryItems = new EssayRepository(new AppDbContext()).GetCategories();
		//	return View();
		//}

		/// <summary>
		/// create essay test
		/// </summary>
		/// <returns></returns>
		public ActionResult NewEssay()
		{
			ViewBag.VideoCategoriesItems = new EssayRepository(new AppDbContext()).GetCategories(null);
			return View();
		}

		[HttpPost]
		public ActionResult NewEssay(CreateEssayVM model, HttpPostedFileBase[] files)
		{

			ViewBag.VideoCategoriesItems = new EssayRepository(new AppDbContext()).GetCategories(null);
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			model.Influencer_Name = this.User.Identity.Name;
			//fill Remove, Upload properties value
			bool isDateTime = DateTime.TryParse(Request.Form["Upload"], out DateTime dt1);
			model.UpLoad = dt1;

			bool itDateTime = DateTime.TryParse(Request.Form["Removed"], out DateTime dt2);
			model.Removed = dt2;


			try
			{
				if (model.Removed <= model.UpLoad)
				{

					throw new Exception("下架時間不能小於上架時間");

				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("Removed", ex.Message);
				//return View(model);
			}

			try
			{
				if (model.UpLoad <= DateTime.Today)
				{

					throw new Exception("上架時間不能小於今天");

				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("UpLoad", ex.Message);

			}
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (files[1] != null)
			{

				string path = Server.MapPath("/Images/Essaysimage");
				var helper = new UploadFileHelper();

				if (model.Images == null) { model.Images = new List<string>(); }
				foreach (var file in files)
				{
					if (file == null)
					{
						continue;
					}
					try
					{
						string result = helper.SaveAs(path, file);
						//string OriginalFileName = System.IO.Path.GetFileName(file.FileName);
						string FileName = result;

						model.Images.Add($"{FileName}");
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(string.Empty, "上傳檔案失敗: " + ex.Message);

					}
				}
			}


			try
			{
				CreateEssayDTO essayDTO = model.ToCreateDTO();
				(bool IsSuccess, string ErrorMessage) result = essayService.Create(essayDTO);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}


			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}


			ViewBag.message = "Blog Datails Are Successfully..!";

			return View(model);
		}
		public ActionResult EditEssays(int id)
		{
			var data = essayService.GetEssay(id).ToCreateVM();
			var categories = new EssayRepository(new AppDbContext()).GetCategories(data.CategoryId).ToList();

			ViewBag.VideoCategories = categories;
			return View(data);
		}
		[HttpPost]
		public ActionResult EditEssays(CreateEssayVM model, HttpPostedFileBase[] files)
		{
			ViewBag.VideoCategories = new EssayRepository(new AppDbContext()).GetCategories(null);
			model.Influencer_Name = this.User.Identity.Name;

			if (files[1] != null)
			{

				string path = Server.MapPath("/Images/Essaysimage");
				var helper = new UploadFileHelper();

				if (model.Images == null) { model.Images = new List<string>(); }
				foreach (var file in files)
				{
					if (file == null)
					{
						continue;
					}
					try
					{
						string result = helper.SaveAs(path, file);
						//string OriginalFileName = System.IO.Path.GetFileName(file.FileName);
						string FileName = result;

						model.Images.Add($"{FileName}");
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(string.Empty, "上傳檔案失敗: " + ex.Message);

					}
				}
			}

			try
			{
				CreateEssayDTO essayDTO = model.ToCreateDTO();
				(bool IsSuccess, string ErrorMessage) result = essayService.Edit(essayDTO);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}


			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}


			return View(model);
		}

		public ActionResult Delete(int id)
		{

			essayService.Delete(id);

			return RedirectToAction("Index");
		}


	}
}