using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml.Linq;
using PagedList;
using H2StyleStore.filter;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("1", "3")]
	public class VideoController : Controller
	{
		private VideoService _videoService;
		private readonly IVideoRepository _videoRepository;
		private AppDbContext _db = new AppDbContext();
		public VideoController()
		{
			var db = new AppDbContext();
			_videoRepository = new VideoRepository(db);
			IVideoRepository repo = new VideoRepository(db);
			this._videoService = new VideoService(repo);
		}

		public ActionResult Index(int? categoryId, string videoTitle, string tagName, 
			string sortOrder, string currentFilter, string searchString, int? page)
		{
			// 將篩選條件放在ViewBag,稍後在 view page取回
			ViewBag.Categories = _videoRepository.GetVideoCategories(categoryId);
			ViewBag.VideoTitle = videoTitle;
			ViewBag.TagName = tagName;
			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "category" : "category_desc";
			ViewBag.DateSortParm = sortOrder == "Date_On" ? "date_desc_On" : "Date_On";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var data = _videoService.GetVideos().Select(x => x.ToVM());

			switch (sortOrder)
			{
				case "name_desc":
					data = data.OrderByDescending(s => s.Title);
					break;
				case "Date_On":
					data = data.OrderBy(s => s.OnShelffTime);
					break;
				case "Date_Off":
					data = data.OrderBy(s => s.OffShelffTime);
					break;
				case "date_desc_On":
					data = data.OrderByDescending(s => s.OnShelffTime);
					break;
				case "date_desc_Off":
					data = data.OrderByDescending(s => s.OffShelffTime);
					break;
				case "category_desc":
					data = data.OrderByDescending(s => s.CategoryId);
					break;
				case "category":
					data = data.OrderByDescending(s => s.CategoryId);
					break;
				default:  // Name ascending 
					data = data.OrderBy(s => s.Id);
					break;
			}


			// 若有篩選categoryid
			if (categoryId.HasValue) data = data.Where(p => p.CategoryId == categoryId.Value);

			// 若有篩選 productName
			if (string.IsNullOrEmpty(videoTitle) == false) data = data.Where(p => p.Title.Contains(videoTitle));
			// data = data.Where(p => Left(p.Name)=="AB");

			//Tag篩選
			if (string.IsNullOrEmpty(tagName) == false) data = data.Where(p => p.Tags.Contains(tagName));

			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(data.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult CreateVideo()          
		{
			ViewBag.VideoCategoryItems = _videoRepository.GetVideoCategories(null);
			return View();
		}

		[HttpPost]
		public ActionResult CreateVideo(CreateVideoVM model, HttpPostedFileBase videoFile, HttpPostedFileBase imageFile)
		{
			ViewBag.VideoCategoryItems = _videoRepository.GetVideoCategories();
			if (!ModelState.IsValid) { return View(model); }
			try
			{
				if (model.OffShelffTime<=DateTime.Now)
				{
					throw new Exception("下架時間不能小於今天");
				}else if (model.OffShelffTime <= model.OnShelffTime)
				{
					throw new Exception("下架時間不能小於上架時間");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("OffShelffTime", ex.Message);
				return View(model);
			}

			try
			{
				if (model.OnShelffTime <= DateTime.Today)
				{
					throw new Exception("上架時間不能小於今天");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("OffShelffTime", ex.Message);
				return View(model);
			}

			var imagePath = Server.MapPath("/Images/VideoImages");
			var videoPath = Server.MapPath("/Videos");
			var helper = new UploadFileHelper();

			try
			{
				string videoSave = helper.SaveAs(videoPath, videoFile);
				string videoFileName = videoSave;
				model.FilePath=videoFileName;

				string imageSave = helper.SaveAs(imagePath, imageFile);
				string imageFileName = imageSave;
				model.Image = imageFileName;

				try
				{
					if (model.Image == null)
					{
						throw new Exception("請上傳影片縮圖檔");
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Image", ex.Message);
					return View(model);
				}

				try
				{
					if (model.Image == null)
					{
						throw new Exception("請上傳影片縮圖檔");
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Image", ex.Message);
					return View(model);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "影片上傳失敗: " + ex.Message);
			}

			try
			{
				(bool IsSuccess, string ErrorMessage) response =_videoService.CreateVideo(model.ToCreateDto());
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid) return RedirectToAction("Index");

			return View(model);
		}

		public ActionResult EditVideo(int id)
		{  
			EditVideoVM model = _videoRepository.GetVideoById(id).ToEditVM();
			var videoCategory = _videoRepository.GetVideoCategories(id).ToList();
			ViewBag.VideoCategoryItems = videoCategory;

			if (model == null) return HttpNotFound();
			return View(model);
		}

		[HttpPost]
		public ActionResult EditVideo(EditVideoVM model, HttpPostedFileBase videoFile, HttpPostedFileBase imageFile)
		{
			var videoCategory = _videoRepository.GetVideoCategories(model.Id);
			ViewBag.VideoCategoryItems = videoCategory;

			var imagePath = Server.MapPath("/Images/VideoImages");
			var videoPath = Server.MapPath("/videos");
			var helper = new UploadFileHelper();
			if (videoFile == null || string.IsNullOrEmpty(videoFile.FileName) || videoFile.ContentLength == 0)
			{
				model.FilePath = model.FilePath;
			}
			else
			{
				try
				{
					string videoSave = helper.SaveAs(videoPath, videoFile);
					string videoFileName = videoSave;
					model.FilePath = videoFileName;
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, "影片上傳失敗: " + ex.Message);
				}
			}

			if (imageFile == null || string.IsNullOrEmpty(imageFile.FileName) || imageFile.ContentLength == 0)
			{
				model.Image = model.Image;
			}
			else
			{
				try
				{
					string imageSave = helper.SaveAs(imagePath, imageFile);
					string imageFileName = imageSave;
					model.Image = imageFileName;
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, "圖片上傳失敗: " + ex.Message);
				}

			}

			if (ModelState.IsValid == false) return View(model);

			UpdateVideoDto request = model.ToEditDto();

			try
			{
				_videoService.UpdateVideo(request);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid == true)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}
		}
	}
}