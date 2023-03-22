using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace H2StyleStore.Models.DTOs
{
	public class CustomerQuestionDTO
	{
		public int CustomerQuestion_Id { get; set; }

		public int? Member_Id { get; set; }

		public string Member_Name { get; set; }

		public int QCategory_Id { get; set; }

		public string QCategory_Name { get; set; }

		public string Title { get; set; }

		public string Problem_Description { get; set; }

		public string FilePath { get; set; }

		public DateTime AskTime { get; set; }

		public string Solution_Description { get; set; }

		public DateTime? SolveTime { get; set; }

		public int? Employee_Id { get; set; }

		public string Employee_Name { get; set; }

	}

	public static class CustomerQuestionDTOExts
	{
		public static CustomerQuestionDTO ToDTO(this CustomerQuestion source)
		{
			string member_Name;
			if (source.Member_Id == null)
			{
				member_Name = string.Empty;
			}
			else
			{
				member_Name = source.Member.Name;
			}
			return new CustomerQuestionDTO
			{
				CustomerQuestion_Id = source.CustomerQuestion_Id,
				Member_Id = source.Member_Id,
				Member_Name = member_Name,
				QCategory_Id = source.QCategory_Id,
				QCategory_Name =source.Question_Categories.Category_Name,
				Title = source.Title,
				Problem_Description = source.Problem_Description,
				FilePath = source.FilePath,
				AskTime = source.AskTime,
				Solution_Description = source.Solution_Description,
				SolveTime = source.SolveTime,
				Employee_Id = source.Employee_Id,
			};
		}
	}
}