using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class TagVM
	{
		public int Id { get; set; }

		public string TagName { get; set; }
	}

	public static class TagVMExts
	{
        public static TagVM ToVM(this TagDTO source)
		{
			return new TagVM
			{
				TagName = source.TagName,
            };
        }
		
	}
	public static class TagExts
	{
		public static TagVM ToVM(this TagDTO source)
		{
			return new TagVM()
			{
				Id = source.Id,
				TagName = source.TagName
			};
		}
		
	}
}