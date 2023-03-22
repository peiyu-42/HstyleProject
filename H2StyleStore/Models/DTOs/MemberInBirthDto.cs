using H2StyleStore.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.DTOs
{
	public class MemberInBirthDto
	{
		public int Id { get; set; }
	}

	public static class MemberInBirthExts
	{
		public static MemberInBirthDto ToBirthDto(this Member source)
		{
			return new MemberInBirthDto
			{
				Id = source.Id
			};
		}
	}
}