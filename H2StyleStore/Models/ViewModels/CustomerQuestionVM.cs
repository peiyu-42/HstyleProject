using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class CustomerQuestionVM
	{

		[Key]
		public int CustomerQuestion_Id { get; set; }

		[Display(Name ="顧客名稱")]
		public string Member_Name{ get; set; }

		[Display(Name ="分類名稱")]
		public string QCategory_Name { get; set; }

		[Required]
		[StringLength(100)]
		[Display(Name ="問題名稱")]
		public string Title { get; set; }

		[Required]
		[Display(Name ="問題描述")]
		public string Problem_Description { get; set; }

		[StringLength(200)]
		[Display(Name ="檔案名稱")]
		public string FilePath { get; set; }

		[Display(Name ="詢問時間")]
		public DateTime AskTime { get; set; }

		[Display(Name ="回答描述")]
		public string Solution_Description { get; set; }

		[Display(Name ="回答時間")]
		public DateTime? SolveTime { get; set; }

		[Display(Name ="回答員工")]
		public int? Employee_Id { get; set; }
	}

	public static class CustomerQuestionVMExts
	{
		public static CustomerQuestionVM ToVM(this CustomerQuestionDTO model)
		{
			return new CustomerQuestionVM
			{
				CustomerQuestion_Id = model.CustomerQuestion_Id,
				Member_Name = model.Member_Name,
				QCategory_Name = model.QCategory_Name,
				Title = model.Title,
				Problem_Description = model.Problem_Description,
				FilePath = model.FilePath,
				AskTime = model.AskTime,
				Solution_Description = model.Solution_Description,
				SolveTime = model.SolveTime,
				Employee_Id = model.Employee_Id,
			};
		}
	}
}