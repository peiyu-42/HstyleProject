using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using H2StyleStore.filter;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("2", "3")]
	public class H_Source_DetailsController : Controller
	{
		private H_Source_DetailService _detailService;
		private H_ActivityService _activityService;
		private IH_Source_DetailRepository _repository;
		private IH_ActivityRepository _activityRepository;

		public H_Source_DetailsController()
		{
			var db = new AppDbContext();
			var db2 = new AppDbContext();

			_repository = new H_Source_DetailRepository(db);
			_activityRepository = new H_ActivityRepository(db2);
			this._detailService = new H_Source_DetailService(_repository);
			_activityService = new H_ActivityService(_activityRepository);
		}

		// GET: H_Source_Details
		public ActionResult HDetail(string sortOrder, string currentFilter, string searchString, int? page, int? activityId, string memberName)
		{
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
			ViewBag.ActivitySortParm = String.IsNullOrEmpty(sortOrder) ? "Activity" : "";
			ViewBag.CurrentSort = sortOrder;
			// 將篩選條件放在ViewBag,稍後在 view page取回
			ViewBag.Activities = _repository.GetActivities(activityId);
			ViewBag.MemberName = memberName;

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var data = _detailService.GetSource().Select(x => x.ToVM());
			//foreach (var item in data)
			//{
			//	_detailService.TotalHcoin(item.Member_Id);
			//}

			// 若有篩選 activityId
			if (activityId.HasValue) data = data.Where(m => m.Activity_Id == activityId.Value);
			// 若有篩選 memberName
			if (string.IsNullOrEmpty(memberName) == false)
			{
				data = data
					.Where(a => a.Member_Name.Contains(memberName));
			}

			switch (sortOrder)
			{
				case "name_desc":
					data = data.OrderByDescending(s => s.Member_Name);
					break;
				case "Date":
					data = data.OrderBy(s => s.Event_Time);
					break;
				case "Activity":
					data = data.OrderByDescending(s => s.H_Activity_Name);
					break;
				default:  // Name ascending 
					data = data.OrderBy(s => s.Source_H_Id);
					break;
			}

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			return View(data.ToPagedList(pageNumber, pageSize));

		}


		public ActionResult CreateNewDetail()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateNewDetail(CreateH_Source_DetailVM model)
		{
			string user = User.Identity.Name;
			model.Employee_Name = user;
			
			if (ModelState.IsValid)
			{				
				_detailService.CreateNewDetail(model.ToDto());
				return RedirectToAction("HDetail");
			}

			return View(model);
		}

		public ActionResult EditDetail(int id)
		{
			CreateH_Source_DetailVM detail = _repository.GetSourceById(id).ToVM();
			if (detail == null) return HttpNotFound();

			return View(detail);
		}

		[HttpPost]
		public ActionResult EditDetail(CreateH_Source_DetailVM model)
		{
			string user = User.Identity.Name;
			model.Employee_Name = user;

			if(!ModelState.IsValid)
			{
				return View(model);
			}
			CreateH_Source_DetailDto request = model.ToDto();

			try
			{
				_detailService.UpdateDetail(request);
			}
			catch(Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			if(ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}
		}


		public ActionResult CheckIn()
		{
			var data = _detailService.GetCheckIn().Select(x => x.ToVM());
			return View(data);
		}

		public ActionResult DeleteDetail(int? id)
		{
			if (id == null) return View("Index");

			H_Source_DetailVM hDetail = _repository.FindDetail(id).ToVM();

			if (hDetail == null) return HttpNotFound();

			return View(hDetail);
		}

		// POST: H_Activities1/Delete/5
		[HttpPost, ActionName("DeleteDetail")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			_repository.DeleteDetail(id);
			return RedirectToAction("HDetail");
		}
	}
}