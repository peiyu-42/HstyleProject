using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace H2StyleStore.Models.Services.Interfaces
{
	public interface IQuestionRepository
	{
		/// <summary>
		/// 取得所有常見問題的資料
		/// </summary>
		/// <returns></returns>
		IEnumerable<CommonQuestionDTO> GetAllCommonQuestion();
		/// <summary>
		/// 新增常見問題
		/// </summary>
		/// <param name="dto"></param>
		void CreateCommonQ(CommonQuestionDTO dto);
		/// <summary>
		/// 取得問題分類的選項
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		IEnumerable<SelectListItem> GetQCategories(int? categoryId);
		IEnumerable<SelectListItem> GetQCategories();
		/// <summary>
		/// 經由Id取得一筆常見問題的資料
		/// </summary>
		/// <param name="commonQId"></param>
		/// <returns></returns>
		CommonQuestionDTO GetCommonQById(int? commonQId);
		/// <summary>
		/// 修改常見問題
		/// </summary>
		/// <param name="dto"></param>
		void UpdateCommonQ(CommonQuestionDTO dto);
		/// <summary>
		/// 刪除一筆常見問題
		/// </summary>
		/// <param name="id">問題Id</param>
		void DeleteCommonQ(int commonQId);
		/// <summary>
		/// 取得顧客所有問題
		/// </summary>
		/// <returns></returns>
		IEnumerable<CustomerQuestionDTO> GetAllCustomerQuestion();
		/// <summary>
		/// 經由Id取得一筆顧客提問的紀錄
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		CustomerQuestionDTO GetCustomerQById(int? id);
		/// <summary>
		/// 回覆顧客提問
		/// </summary>
		/// <param name="dto"></param>
		void UpdatetCustomerQ(CustomerQuestionDTO dto);
	}
}
