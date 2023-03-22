using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using H2StyleStore.Models.ViewModels;
using System.Web.Razor.Generator;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _db;
		public ProductRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<ProductDto> GetProducts()
		{
			IEnumerable<Product> query = _db.Products;
				//.Include("PCategory");
			
			query = query.OrderBy(x => x.DisplayOrder);

			return query.Select(x => x.ToDto());
		}

		public IEnumerable<ProductDto> GetProducts(string term)
		{
			IEnumerable<Product> query = _db.Products;
			//.Include("PCategory");
			query = query.Where(x => x.Product_Name.StartsWith(term));
			query = query.OrderBy(x => x.DisplayOrder);

			return query.Select(x => x.ToDto());
		}

		public CreateProductDto GetProduct(int id)
		{
			IEnumerable<Product> query = _db.Products;
			//.Include("PCategory");
			var item = query.FirstOrDefault(x => x.Product_Id == id).ToCreateDto();


			return item;
		}
		public int GetNoOfProducts()
		{
			return _db.Products.Count();
		}

		public IEnumerable<SelectListItem> GetCategories(int? categoryId)
		{
			var data = _db.PCategories;

			
			foreach (var item in data)
			{
				yield return new SelectListItem { 
					Value = item.PCategory_Id.ToString(),
					Text = item.PCategoryName,
					Selected= (categoryId.HasValue && item.PCategory_Id == categoryId)
				};
				
			}
			
		}

		public bool IsExist(string productName)
		{
			var product = _db.Products.SingleOrDefault(x => x.Product_Name == productName);
			return (product != null);
		}

		public bool EditIsExist(string productName, int id)
		{
			var product = _db.Products.Where(x => x.Product_Id != id).SingleOrDefault(x => x.Product_Name == productName);
			return (product != null);
		}

		//public void Create(ProductDto dto)
		//{
		//	int.TryParse(dto.PCategoryName, out int catgoryId);
		//	Product product = new Product
		//	{
		//		Product_Name = dto.Product_Name,
		//		UnitPrice = dto.UnitPrice,
		//		Description = dto.Description,
		//		Create_at = DateTime.Now,
		//		Discontinued = dto.Discontinued,
		//		DisplayOrder = dto.DisplayOrder,
		//		Category_Id = catgoryId,
		//	};
		//	_db.Products.Add(product);
			

			
		//	//b.Images.Add();
		//	//_db.Tags.Add()
		//	_db.SaveChanges();


		//}
		public void Create(CreateProductDto dto)
		{
			Product product = new Product
			{
				Product_Name = dto.Product_Name,
				UnitPrice = dto.UnitPrice,
				Description = dto.Description,
				Create_at = DateTime.Now,
				Discontinued = dto.Discontinued,
				DisplayOrder = dto.DisplayOrder,
				Category_Id = dto.Category_Id,
			};
			if (_db.Products.Count() != 0)
			{
				bool isBigest = dto.DisplayOrder > _db.Products.Max(x => x.DisplayOrder);
			
				if(isBigest == false)
				{
				var refreshProduct = _db.Products.Where(x => x.DisplayOrder >= dto.DisplayOrder).OrderBy(x => x.DisplayOrder).ToArray();
				for (int i = 0; i < refreshProduct.Count(); i++)
				{
					refreshProduct[i].DisplayOrder = dto.DisplayOrder + 1 + i;
				}
			}
			}

			_db.Products.Add(product);

			foreach(string tag in dto.tags)
			{
				var tags = _db.Tags.Select(x => x.TagName).ToList();
				if(tags.Contains(tag) == false) 
				{	
					Tag newTag = new Tag { TagName= tag };
					product.Tags.Add(newTag);
				}
				else
				{
					Tag oldTag =_db.Tags.Where(x => x.TagName == tag).FirstOrDefault();
					product.Tags.Add(oldTag);
				}
			}

			foreach (string path in dto.images)
			{
				Image image = new Image	{Path = path,};
				product.Images.Add(image);
			}


			List<Spec> specs= dto.specs.Select(x => new Spec
			{
				Color= x.Color,
				Size= x.Size,
				Stock= x.Stock,
			}).ToList();

			foreach (var spec in specs)
			{
				product.Specs.Add(spec);
			}
			
			
			_db.SaveChanges();

			





			//b.Images.Add();
			//_db.Tags.Add()
			


		}

		public void Edit(CreateProductDto dto)
		{
			var product = _db.Products.Find(dto.ProductID);
			product.Category_Id = dto.Category_Id;
			product.Product_Name = dto.Product_Name;
			product.Product_Id = dto.ProductID;
			product.UnitPrice = dto.UnitPrice;
			product.Description = dto.Description;
			product.Discontinued = dto.Discontinued;

			bool isBigger =dto.DisplayOrder  > product.DisplayOrder;
			int refreshPoint = isBigger ? product.DisplayOrder : dto.DisplayOrder;
			if (isBigger)
			{
				var refreshProduct = _db.Products.Where(x => x.DisplayOrder > refreshPoint && x.DisplayOrder<=dto.DisplayOrder).OrderBy(x =>x.DisplayOrder).ToArray();
				for (int i = 0; i < refreshProduct.Count(); i++)
				{
					refreshProduct[i].DisplayOrder = refreshPoint + i;
				}
			}
			else
			{
				var refreshProduct = _db.Products.Where(x => x.DisplayOrder >= refreshPoint && x.Product_Id != dto.ProductID).OrderBy(x => x.DisplayOrder).ToArray();
				for (int i = 0; i < refreshProduct.Count(); i++)
				{
					refreshProduct[i].DisplayOrder = refreshPoint + 1+ i;
				}
			}			
			product.DisplayOrder = dto.DisplayOrder;

			foreach (var dbTag in product.Tags.ToArray())
			{
				product.Tags.Remove(dbTag);
			}

			foreach (string tag in dto.tags)
			{
				var tags = _db.Tags.Select(x => x.TagName).ToList();
				if (tags.Contains(tag) == false)
				{
					Tag newTag = new Tag { TagName = tag };
					product.Tags.Add(newTag);
				}
				else
				{
					Tag oldTag = _db.Tags.Where(x => x.TagName == tag).FirstOrDefault();
					product.Tags.Add(oldTag);
				}
			}

			List<Spec> specs = dto.specs.Select(x => new Spec
			{
				Id = x.Id,
				Color = x.Color,
				Size = x.Size,
				Stock = x.Stock
			}).ToList();
			
			var dbSpecs = _db.Specs.Where(x => x.Product_Id == dto.ProductID).ToArray();

			//foreach (var dbSpec in dbSpecs)
			//{
			//	_db.Specs.Remove(dbSpec);
			//}

			var dbSpecId = dbSpecs.Select(x => x.Id);
			var dtoSpIds = new List<int>();
			foreach (var spec in specs)
			{
				if (dbSpecId.Contains(spec.Id) == false)
				{
					product.Specs.Add(spec);
					dtoSpIds.Add(spec.Id);
				}
				else if (dbSpecId.Contains(spec.Id))
				{
					var dbspec = _db.Specs.Where(x => x.Id == spec.Id).FirstOrDefault();
					dbspec.Size = spec.Size;
					dbspec.Stock = spec.Stock;
					dbspec.Color = spec.Color;
					dtoSpIds.Add(spec.Id);
				}
			}
			foreach (var spec in dbSpecs)
			{
				if (dtoSpIds.Contains(spec.Id) == false)
				{
					spec.Discontinued = true;
				}
			}



			foreach (var dbimg in _db.Images.ToArray())
			{
				product.Images.Remove(dbimg);
			}



			foreach (string img in dto.images)
			{
				var imgs = _db.Images.Select(x => x.Path).ToList();
				if (imgs.Contains(img) == false)
				{
					Image newImg = new Image { Path = img };
					product.Images.Add(newImg);
				}
				else
				{
					Image oldImg = _db.Images.Where(x => x.Path == img).FirstOrDefault();
					product.Images.Add(oldImg);
				}
			}
			_db.SaveChanges();
		}

		public void EditDiscontinued(List<EditAllDto> newItems)
		{
			var product = _db.Products;

			foreach (var item in newItems) {
				product.Find(item.id).Discontinued = item.discontinued;
			}


			_db.SaveChanges();
		}

		public List<ProductDto> GetFiltedProducts(string filterStr)
		{
			IEnumerable<Product> products = _db.Products;
			products = products.Where(
				x =>
				x.UnitPrice.ToString().Contains(filterStr)
				|| x.Product_Name.Contains(filterStr) 			
				|| string.Join("", x.Tags.Select(y => y.TagName)).Contains(filterStr)
				|| x.Description.Contains(filterStr)
				|| x.PCategory.PCategoryName.Contains(filterStr)
				);

			products = products.OrderBy(x => x.DisplayOrder);
			var data = products.Select(x => x.ToDto()).ToList();

			return data;
		}

		public int NewCategory(string categoryName)
		{
			var categories = _db.PCategories;
			var newCategory = new PCategory { PCategoryName= categoryName };
			categories.Add(newCategory);

			_db.SaveChanges();
			
			return _db.PCategories.Max(x => x.PCategory_Id);
		}
	}
}