using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class EssayRepository : IEssayRepository
	{
		private readonly AppDbContext _db;
		public EssayRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<EssayDTO> GetEssays()
		{
			IEnumerable<Essay> query = _db.Essays;
			return query.Select(x => x.ToDto());
		}

		public CreateEssayDTO GetEssay(int id)
		{
			IEnumerable<Essay> query = _db.Essays;
			var item = query.FirstOrDefault(x => x.Essay_Id == id).ToCreateDTO();
			return item;
		}

		public IEnumerable<SelectListItem> GetCategoriesSelect(int? categoryId)
		{
			var data = _db.VideoCategories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.CategoryName
				,
					Selected = (categoryId.HasValue && c.Id == categoryId.Value)
				})
				.ToList()
				.Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇" });

			return data;
		}



		public IEnumerable<SelectListItem> GetCategories(int? categoryId)
		{
			var data = _db.VideoCategories;


			foreach (var item in data)
			{
				yield return new SelectListItem
				{
					Value = item.Id.ToString(),
					Text = item.CategoryName,

					Selected = (categoryId.HasValue && item.Id == categoryId)
				};

			}
		}
		public bool IsExist(string etitle)
		{
			var essays = _db.Essays.SingleOrDefault(x => x.ETitle == etitle);
			return (essays != null);
		}

		public bool EditIsExist(string etitle, int id)
		{
			var product = _db.Essays.Where(x => x.Essay_Id != id).SingleOrDefault(x => x.ETitle == etitle);
			return (product != null);
		}

		//private AppDbContext db = new AppDbContext();
		public void Create(EssayDTO dto)
		{
			int.TryParse(dto.CategoryName, out int catgoryId);
			Employee employee = _db.Employees.Where(x => x.Account == dto.Influencer_Name).FirstOrDefault();
			Essay essays = new Essay
			{
				Essay_Id = dto.Essay_Id,
				Influencer_Id = employee.Employee_id,
				UplodTime = dto.UplodTime,
				ETitle = dto.ETitle,
				EContent = dto.EContent,
				UpLoad = dto.UpLoad,
				Removed = dto.Removed,
				CategoryId = catgoryId,

			};

			_db.Essays.Add(essays);
			_db.SaveChanges();
		}

		public void Create(CreateEssayDTO dto)
		{
			Employee employee = _db.Employees.Where(x => x.Account == dto.Influencer_Name).FirstOrDefault();
			//int.TryParse(dto.CategoryName, out int catgoryId);
			Essay essays = new Essay
			{

				Influencer_Id = employee.Employee_id,
				//Influencer_Id = dto.Influencer_Id,
				UplodTime = DateTime.Now,
				ETitle = dto.ETitle,
				EContent = dto.EContent,
				UpLoad = dto.UpLoad,
				Removed = dto.Removed,
				CategoryId = dto.CategoryId,

			};
			_db.Essays.Add(essays);

			foreach (string tag in dto.Tags)
			{
				var tags = _db.Tags.Select(x => x.TagName).ToList();
				if (tags.Contains(tag) == false)
				{
					Tag newTag = new Tag { TagName = tag };
					essays.Tags.Add(newTag);
				}
				else
				{
					Tag oldTag = _db.Tags.Where(x => x.TagName == tag).FirstOrDefault();
					essays.Tags.Add(oldTag);
				}
			}

			foreach (string path in dto.Images)
			{
				Image image = new Image { Path = path, };
				essays.Images.Add(image);
			}

			_db.SaveChanges();
		}
		public void Edit(CreateEssayDTO dTO)
		{
			Employee employee = _db.Employees.Where(x => x.Account == dTO.Influencer_Name).FirstOrDefault();
			var essay = _db.Essays.Find(dTO.Essay_Id);
			essay.Influencer_Id = employee.Employee_id;
			essay.ETitle = dTO.ETitle;
			essay.EContent = dTO.EContent;
			essay.UpLoad = dTO.UpLoad;
			essay.Removed = dTO.Removed;
			essay.CategoryId = dTO.CategoryId;
			foreach (var dbTag in essay.Tags.ToArray())
			{
				essay.Tags.Remove(dbTag);
			}
			foreach (string tag in dTO.Tags)
			{
				var tags = _db.Tags.Select(x => x.TagName).ToList();
				if (tags.Contains(tag) == false)
				{
					Tag newTag = new Tag { TagName = tag };
					essay.Tags.Add(newTag);
				}
				else
				{
					Tag oldTag = _db.Tags.Where(x => x.TagName == tag).FirstOrDefault();
					essay.Tags.Add(oldTag);
				}
			}

			foreach (var dbing in _db.Images.ToArray())
			{
				essay.Images.Remove(dbing);
			}
			foreach (string img in dTO.Images)
			{
				var imgs = _db.Images.Select(x => x.Path).ToList();
				if (imgs.Contains(img) == false)
				{
					Image newImg = new Image { Path = img };
					essay.Images.Add(newImg);
				}
				else
				{
					Image oldImg = _db.Images.Where(x => x.Path == img).FirstOrDefault();
					essay.Images.Add(oldImg);
				}
			}



			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			var essay = _db.Essays.FirstOrDefault(x => x.Essay_Id == id);
			_db.Essays.Remove(essay);
			_db.SaveChanges();
		}


	}
}