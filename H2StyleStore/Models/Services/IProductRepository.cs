using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2StyleStore.Models.Services
{
	public interface IProductRepository
	{
		IEnumerable<ProductDto> GetProducts();
		IEnumerable<ProductDto> GetProducts(string term);
		CreateProductDto GetProduct(int id);
		int GetNoOfProducts();
		bool IsExist(string productName);
		bool EditIsExist(string productName, int id);

		//void Create(ProductDto product);

		void Create(CreateProductDto product);

		void Edit(CreateProductDto dto);

		void EditDiscontinued(List<EditAllDto> newItems);
		List<ProductDto> GetFiltedProducts(string filterStr);
		int NewCategory(string newCategory);
	}
}
