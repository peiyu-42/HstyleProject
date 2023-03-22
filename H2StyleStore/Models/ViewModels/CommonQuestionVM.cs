using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CommonQuestionVM
	{
		//[Display(Name = "問題Id")]
		[Key]
		public int CommonQuestion_Id { get; set; }

		[Display(Name = "問題分賴")]
		public string QCategory_Name{ get; set; }

		[Display(Name = "問題")]
		[Required]
		public string Question { get; set; }

		[Display(Name = "回答")]
		[Required]
		public string Answer { get; set; }

		//public int Satiafaction_Click { get; set; }

		//public int Satiafaction_Yes { get; set; }
	}

	public static class CommonQuestionVMExts
	{
		public static CommonQuestionVM ToVM(this CommonQuestionDTO dto)
		{
			return new CommonQuestionVM
			{
				CommonQuestion_Id = dto.CommonQuestion_Id,
				QCategory_Name = dto.QCategory_Name,
				Question = dto.Question,
				Answer = dto.Answer,
				//Satiafaction_Click = dto.Satiafaction_Click,
				//Satiafaction_Yes = dto.Satiafaction_Yes
			};
		}

		public static CommonQuestionDTO ToDto(this CommonQuestionVM model)
		{
			return new CommonQuestionDTO
			{
				CommonQuestion_Id = model.CommonQuestion_Id,
				QCategory_Name = model.QCategory_Name,
				Question = model.Question,
				Answer = model.Answer,
			};
		}
	}

}