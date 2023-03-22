using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace H2StyleStore.Models.ViewModels
{
	public class EditCustomerQVM
	{
		[Key]
		public int CustomerQuestion_Id { get; set; }

		[Display(Name = "顧客名稱")]
		public string Member_Name { get; set; }

		[Display(Name = "類別名稱")]
		[Required]
		public int QCategory_Id { get; set; }

		[Required]
		[StringLength(100)]
		[Display(Name = "問題名稱")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "問題描述")]
		public string Problem_Description { get; set; }

		[StringLength(200)]
		[Display(Name = "檔案名稱")]
		public string FilePath { get; set; }

		//[Display(Name = "詢問時間")]
		//public DateTime AskTime { get; set; }

		[Display(Name = "回答描述")]
		public string Solution_Description { get; set; }

		//[Display(Name = "回答時間")]
		//public DateTime? SolveTime { get; set; }

		[Display(Name = "回答員工")]
		public string Employee_Name { get; set; }
	}

	public static class EditCustomerQVMExts
	{
		public static EditCustomerQVM ToEditVM(this CustomerQuestionDTO model)
		{
			return new EditCustomerQVM
			{
				CustomerQuestion_Id = model.CustomerQuestion_Id,
				Member_Name = model.Member_Name,
				QCategory_Id = model.QCategory_Id,
				Title = model.Title,
				Problem_Description = model.Problem_Description,
				FilePath = model.FilePath,
				//AskTime = model.AskTime,
				Solution_Description = model.Solution_Description,
				//SolveTime = model.SolveTime,
				//Employee_Name = model.Employee_Name,
			};
		}

		public static CustomerQuestionDTO ToDTO(this EditCustomerQVM model)
		{
			return new CustomerQuestionDTO
			{
				CustomerQuestion_Id = model.CustomerQuestion_Id,
				Member_Name = model.Member_Name,
				QCategory_Id = model.QCategory_Id,
				Title = model.Title,
				Problem_Description = model.Problem_Description,
				FilePath = model.FilePath,
				//AskTime = model.AskTime,
				Solution_Description = model.Solution_Description,
				//SolveTime = model.SolveTime,
				Employee_Name = model.Employee_Name,
			};
		}
	}
}