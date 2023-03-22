using H2StyleStore.filter;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("2", "3")]
	public class ProductsController : Controller
	{
		private ProductService productService;

		public ProductsController()
		{
			var db = new AppDbContext();
			IProductRepository repo = new ProductRepository(db);
			this.productService = new ProductService(repo);
		}

		public static string select_user_name;
		// GET: Products
		
		public ActionResult Index()
        {
            
            return View();
        }

		public ActionResult PagedPartial(string searchStr, int? page, int? pageSize)
		{
			//找出符合篩選條件的資料

			List<ProductVM> filtedProducts = productService.productsFilter(searchStr).Select(x => x.ToVM()).ToList();

			//將資料轉為 PagedList 的資料

			var pageNumber = (page == null)? 1: (int)page;// 若無傳入 Page，預設查詢第1頁

			pageSize = pageSize ?? 10;
			var onePageOfProducts = filtedProducts.ToPagedList(pageNumber, (int)pageSize); // 參數說明: ToPagedList( 第幾頁 , 一頁要顯示多少資料 )

			
	

			return PartialView("PagedPartial", onePageOfProducts);
		}

		[HttpPost]
		public string EditAll(List<EditAllVM> editAlls)
		{
			try
			{
				productService.EditDiscontinued(editAlls.Select(x => x.ToDto()).ToList());
			}
			catch(Exception ex)
			{
				return "儲存失敗，原因: "+ ex.Message;
			}

			return "儲存成功";
		}

		[HttpPost]
		public JsonResult NewCategory(string newCategory)
		{
			int newId = -1;
			try
			{
				newId = productService.NewCategory(newCategory);
			}
			catch (Exception ex)
			{
				return Json( "儲存失敗，原因: " + ex.Message);
			}
			List<string> result = new List<string>();
			result.Add(newId.ToString());
			result.Add(newCategory);
			return Json(result);
		}

		//取得商品名稱自動跳出選項
		public JsonResult QueryProducts(string term)
		{
			var items = productService.GetProducts(term).ToList();
			
			return Json(items.DefaultIfEmpty(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult NewProduct()
		{
			ViewBag.PCategoryItems = new ProductRepository(new AppDbContext()).GetCategories(null);
			ViewBag.NoOfProducts = productService.GetNoOfProducts()+1;
			return View();
		}
		[HttpPost]
		public ActionResult NewProduct(CreateProductVM model, HttpPostedFileBase[] files)
		{

			ViewBag.PCategoryItems = new ProductRepository(new AppDbContext()).GetCategories(null);
			ViewBag.NoOfProducts = productService.GetNoOfProducts()+1;

			if (files[0] != null)
			{
				string path = Server.MapPath("/Images/ProductImages");
				var helper = new UploadFileHelper();


				model.images = new List<string>();

				foreach (var file in files)
				{
					try
					{
						string result = helper.SaveAs(path, file);
						//string OriginalFileName = System.IO.Path.GetFileName(file.FileName);
						string FileName = result;

						model.images.Add($"{FileName}");
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("images", "上傳檔案失敗: " + ex.Message);

					}
				}
			}
			else
			{
				ModelState.AddModelError("images", "必須上傳商品照片");
				return View(model);
			}

			try
			{
				var productDto = model.ToCreateDto();
				(bool IsSuccess, string ErrorMessage) result = productService.Create(productDto);

				if(result.IsSuccess == false)
				{
					throw new Exception(result.ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				
			}

			if(ModelState.IsValid) return RedirectToAction("Index");

			return View(model);
		}

		public ActionResult EditProduct(int id)
		{
			var data = productService.GetProduct(id).ToCreateVM();
			var categories = new ProductRepository(new AppDbContext()).GetCategories(data.Category_Id).ToList();
			
			ViewBag.PCategoryItems = categories;
			ViewBag.NoOfProducts = productService.GetNoOfProducts();

			return View(data);
		}
		[HttpPost]
		public ActionResult EditProduct(CreateProductVM model, HttpPostedFileBase[] files)
		{

			ViewBag.PCategoryItems = new ProductRepository(new AppDbContext()).GetCategories(null);
			ViewBag.NoOfProducts = productService.GetNoOfProducts();



			if (files[0] != null)
			{
				string path = Server.MapPath("/Images/ProductImages");
				var helper = new UploadFileHelper();
				if(model.images == null) { model.images = new List<string>(); }
				foreach (var file in files)
				{
					try
					{
						string result = helper.SaveAs(path, file);
						//string OriginalFileName = System.IO.Path.GetFileName(file.FileName);
						string FileName = result;

						model.images.Add($"{FileName}");
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(string.Empty, "上傳檔案失敗: " + ex.Message);

					}
				}
			}

			try
			{
				var productDto = model.ToCreateDto();
				(bool IsSuccess, string ErrorMessage) result = productService.Edit(productDto);
				if (result.IsSuccess == false) throw new Exception(result.ErrorMessage);
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

	}

}