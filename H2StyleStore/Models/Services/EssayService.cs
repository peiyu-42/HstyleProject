using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{
	public class EssayService
	{
		private readonly IEssayRepository _repository;

		public EssayService(IEssayRepository repository)
		{
			_repository = repository;
		}


		public IEnumerable<EssayDTO> GetEssays()
		{
			return _repository.GetEssays();
		}

		public CreateEssayDTO GetEssay(int essayId)
		{
			return _repository.GetEssay(essayId);
		}

		public (bool, string) Create(EssayDTO dto)
		{
			if (_repository.IsExist(dto.ETitle))
			{
				return (false, "標題名稱已使用，請更改名稱");
			}

			_repository.Create(dto);

			return (true, null);

		}

		public (bool, string) Create(CreateEssayDTO dto)
		{
			if (_repository.IsExist(dto.ETitle))
			{
				return (false, "標題名稱已使用，請更改名稱");
			}



			_repository.Create(dto);

			return (true, null);

		}

		public (bool, string) Edit(CreateEssayDTO dTO)
		{
			if (_repository.EditIsExist(dTO.ETitle, dTO.Essay_Id))
			{
				return (false, "標題名稱已使用，請更改名稱");
			}

			_repository.Edit(dTO);
			return (true, null);
		}
		public void Delete(int id)
		{
			if (_repository.GetEssay(id).Essay_Id != id)
			{
				return;
			}

			_repository.Delete(id);
		}
	}
}