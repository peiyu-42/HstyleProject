using H2StyleStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.ViewModels
{
	public class EditAllVM
	{
		public int id { get; set; }
		public bool discontinued { get; set; }
	}

	public static class EditAllExts
	{
		public static EditAllDto ToDto(this EditAllVM source)
		{
			return new EditAllDto
			{
				id = source.id,
				discontinued = source.discontinued,
			};
		}
	}
}