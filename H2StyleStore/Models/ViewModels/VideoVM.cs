using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
    public class VideoVM
    {
        public int Id { get; set; }

        //public virtual Image Image { get; set; }

        [Display(Name ="影片名稱")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "影片說明")]
        public string Description { get; set; }

        //public int ImageId { get; set; }

		[Display(Name = "上架時間")]
		//public DateTime? OnShelffTime { get; set; }
		public string OnShelffTime { get; set; }

		[Display(Name = "下架時間")]
		//public DateTime? OffShelffTime { get; set; }
		public string OffShelffTime { get; set; }

        [Display(Name = "狀態")]
		public string IsOnShelff { get; set; }

		[Display(Name = "標籤")]
		public IEnumerable<string> Tags { get; set; }

		[Display(Name = "類別")]
		public string CategoryName { get; set; }

		public string Image { get; set; }

        public int CategoryId{ get; set; }
    }

    public static class VideoDtoExts
    {
        public static VideoVM ToVM(this VideoDto source)
        {
            string onShelffTimeValue = "";
            string offShelffTimeValue = "";
            string isOnShelff = "";


			if (source.OnShelffTime == null)onShelffTimeValue = "無";            
            else onShelffTimeValue = source.OnShelffTime.ToString();

            if (source.OffShelffTime == null) offShelffTimeValue = "無";
            else offShelffTimeValue = source.OffShelffTime.ToString();

			if (source.IsOnShelff==true) isOnShelff = "已上架";
			else isOnShelff ="未上架" ;


			return new VideoVM
            {
                Id = source.Id,
                CategoryId= source.CategoryId,
                Title = source.Title,
                Description = source.Description,
                //ImageId = source.ImageId,
                OnShelffTime = onShelffTimeValue,
                OffShelffTime = offShelffTimeValue,
                Image = source.Image.Path,
                Tags = source.Tags.Select(x=>x.TagName), 
                CategoryName=source.CategoryName,
                IsOnShelff= isOnShelff,
            };
        }
    }
} 