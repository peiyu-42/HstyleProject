using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class CommonQuestionDTO
	{
		public int CommonQuestion_Id { get; set; }

		public int QCategory_Id { get; set; }

		public string QCategory_Name { get; set; }

		public string Question { get; set; }

		public string Answer { get; set; }

		public int Satisfaction_No { get; set; }

		public int Satiafaction_Yes { get; set; }
	}

	public static class CommonQuestionDTOExts
	{
		public static CommonQuestionDTO ToDTO(this CommonQuestion source)
		{
			return new CommonQuestionDTO
			{
				CommonQuestion_Id = source.CommonQuestion_Id,
				QCategory_Id = source.QCategory_Id,
				QCategory_Name=source.Question_Categories.Category_Name,
				Question = source.Question,
				Answer = source.Answer,
				Satisfaction_No = source.Satisfaction_No,
				Satiafaction_Yes = source.Satisfaction_Yes,
			};
		}
	}
}