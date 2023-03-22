using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class SpecVm
	{
		public int Id { get; set; }
		[Required]
		public string Color { get; set; }

		public string Size { get; set; }

		public int Stock { get; set; }

	}

	public static class SpecVMExts
	{
		public static SpecVm ToVM(this SpecDto source)
			=> new SpecVm
			{
				Id= source.Id,
				Color = source.Color,
				Size = source.Size,
				Stock = source.Stock,
			};
	}
}