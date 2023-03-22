using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace H2StyleStore.Models.ViewModels
{
	public class EditVideoVM
	{
		public int Id { get; set; }

		[Display(Name = "影片名稱")]
	
		public string Title { get; set; }

		[Display(Name = "影片說明")]
		public string Description { get; set; }

		[Display(Name = "影片路徑")]
		public string FilePath { get; set; }

		[Display(Name = "上架時間")]
		public DateTime? OnShelffTime { get; set; }

		[Display(Name = "下架時間")]
		public DateTime? OffShelffTime { get; set; }

		[Display(Name = "影片縮圖")]
		public string Image { get; set; }

		[Display(Name = "類別")]
		public int CategoryId { get; set; }

		[Display(Name = "標籤")]
		public string Tags { get; set; }
	}
}