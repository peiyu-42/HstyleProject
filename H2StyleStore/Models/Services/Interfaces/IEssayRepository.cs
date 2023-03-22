using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IEssayRepository
	{
		IEnumerable<EssayDTO> GetEssays();

		CreateEssayDTO GetEssay(int id);
		bool IsExist(string ETiltle);

		bool EditIsExist(string ETiltle, int id);
		void Create(EssayDTO essay);
		void Create(CreateEssayDTO essay);
		void Delete(int id);
		IEnumerable<SelectListItem> GetCategories(int? categoryId);

		void Edit(CreateEssayDTO dTO);

		IEnumerable<SelectListItem> GetCategoriesSelect(int? categoryId);

		//EssayDTO Load(int memberId);
		//EssayDTO GetByAccount(string account);
		//void ActiveRegister(int memberId);

		//void Update(EssayDTO entity);

		//void UpdatePassword(int memberId, string newEncryptedPassword);
	}
}
