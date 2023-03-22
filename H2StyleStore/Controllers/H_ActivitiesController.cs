using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using PagedList;
using H2StyleStore.filter;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("2", "3")]

	public class H_ActivitiesController : Controller
	{
		private H_ActivityService _hActivityService;
		private IH_ActivityRepository _repository;

		public H_ActivitiesController()
		{
			var _db = new AppDbContext();
			IH_ActivityRepository repo = new H_ActivityRepository(_db);
			_hActivityService = new H_ActivityService(repo);
			_repository = new H_ActivityRepository(_db);
		}

		// GET: H_Activities
		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string activityName)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.ActivityName = activityName;
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			IEnumerable<H_ActivityVM> data = _hActivityService.GetHActivity().Select(a => a.ToVM());
			// Search
			if (string.IsNullOrEmpty(activityName) == false)
			{
				data = data
					.Where(a => a.Activity_Name.Contains(activityName) || a.Activity_Describe.Contains(activityName));
			}
			
			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(data.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult CreateActivity()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateActivity(H_ActivityVM model)
		{

			if (ModelState.IsValid)
			{
				_hActivityService.CreateNewActivity(model.ToDto());
				return RedirectToAction("Index");
			}

			return View(model);
		}

		[HttpGet]
		public ActionResult EditActivity(int id)
		{

			H_ActivityVM activity = _repository.GetHActivityById(id).ToVM();

			if (activity == null) return HttpNotFound();

			return View(activity);
		}

		[HttpPost]
		public ActionResult EditActivity(H_ActivityVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			H_ActivityDto request = model.ToDto();

			try
			{
				_hActivityService.UpdateActivity(request);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			else { return View(model); }
		}

		//GET: H_Activities1/Delete/5
		public ActionResult DeleteActivity(int? id)
		{
			if (id == null) return View("Index");

			H_ActivityVM hActivity = _repository.FindActivity(id).ToVM();

			if (hActivity == null) return HttpNotFound();

			return View(hActivity);
		}

		// POST: H_Activities1/Delete/5
		[HttpPost, ActionName("DeleteActivity")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			_repository.DeleteActivity(id);
			return RedirectToAction("Index");
		}
	}
}