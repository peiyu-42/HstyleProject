using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.Infrastructures;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{
	public class ProductService
	{
		private readonly IProductRepository _repository;

		public ProductService(IProductRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<ProductDto> GetProducts()
		{
			return _repository.GetProducts();
		}

		public IEnumerable<string> GetProducts(string term)
		{
			var itemNames = _repository.GetProducts(term).Select(x => x.Product_Name);
			return itemNames;
		}

		public CreateProductDto GetProduct(int productId)
		{
			return _repository.GetProduct(productId);
		}
		public int GetNoOfProducts()
		{
			return _repository.GetNoOfProducts();
		}

		//public (bool, string ) Create(ProductDto dto)
		//{
		//	if (_repository.IsExist(dto.Product_Name))
		//	{
		//		return (false, "商品名稱已使用，請更改名稱");
		//	}

		//	_repository.Create(dto);

		//	return (true, null);

		//}

		public (bool, string) Create(CreateProductDto dto)
		{
			if (_repository.IsExist(dto.Product_Name))
			{
				return (false, "商品名稱已使用，請更改名稱");
			}

			if (dto.UnitPrice <= 0) return (false, "商品價格須為正整數");

			foreach (var spec in dto.specs)
			{
				if (spec.Stock <= 0) return (false, "商品數量須為正整數");
			}


			_repository.Create(dto);

			return (true, null);

		}

		public (bool, string) Edit(CreateProductDto dto)
		{
			if (_repository.EditIsExist(dto.Product_Name, dto.ProductID))
			{
				return (false, "商品名稱已使用，請更改名稱");
			}

			if (dto.UnitPrice <= 0) return (false, "商品價格須為正整數");

			foreach (var spec in dto.specs)
			{
				if (spec.Stock <= 0) return (false, "商品數量須為正整數");
			}

			_repository.Edit(dto);

			return (true, null);
		}

		public void EditDiscontinued(List<EditAllDto> newItems)
		{
			_repository.EditDiscontinued(newItems);
		}

		internal List<ProductDto> productsFilter(string searchFilter)
		{
			bool isEmpty = string.IsNullOrEmpty(searchFilter);
			if (isEmpty)
			{
				var data = _repository.GetProducts().ToList();
				return (data);
			}
			else
			{
				var data = _repository.GetFiltedProducts(searchFilter);
				return (data);
			}
		}

		public int NewCategory(string newCategory)
		{
			return _repository.NewCategory(newCategory);
		}
	}
}