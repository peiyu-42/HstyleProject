using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class CreateVideoDto
	{
		public int Id{ get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string FilePath { get; set; }

		public int CategoryId { get; set; }

		public int ImageId { get; set; }

		public DateTime? OnShelffTime { get; set; }

		public DateTime? OffShelffTime { get; set; }

		public DateTime CreatedTime { get; set; }

		public bool IsOnShelff { get; set; }

		public string Image { get; set; }

		public string VideoCategory { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public List<string> Tags { get; set; }
	}
}