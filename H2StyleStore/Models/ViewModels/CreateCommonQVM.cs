using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CreateCommonQVM
	{
		[Key]
		public int CommonQuestion_Id { get; set; }

		[Display(Name = "問題類別")]
		[Required]
		public int QCategory_Id { get; set; }

		[Display(Name = "問題")]
		[Required(ErrorMessage = "請填寫問題")]
		public string Question { get; set; }

		[Display(Name = "回答")]
		[Required(ErrorMessage = "請回答問題")]
		public string Answer { get; set; }
	}

	public static class CreateCommonQExts
	{
		public static CommonQuestionDTO ToDto(this CreateCommonQVM model)
		{
			return new CommonQuestionDTO
			{
				CommonQuestion_Id = model.CommonQuestion_Id,
				QCategory_Id = model.QCategory_Id,
				Question = model.Question,
				Answer = model.Answer,
			};
		}

		public static CreateCommonQVM ToEditVM(this CommonQuestionDTO dto)
		{
			return new CreateCommonQVM
			{
				CommonQuestion_Id = dto.CommonQuestion_Id,
				QCategory_Id = dto.QCategory_Id,
				Question = dto.Question,
				Answer = dto.Answer,
			};
		}
	}
}