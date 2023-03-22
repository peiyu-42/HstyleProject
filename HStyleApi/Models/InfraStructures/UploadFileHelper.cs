using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Infrastructures
{
	public class UploadFileHelper
	{
		///<summary>
		///將檔案上傳存檔
		///</summary>
		///<param name="path">要存放的實體路徑</param>
		///<param name="file"></param>>
		///<exception cref="UploadFileNullException">沒有上傳檔案,或檔案大小為 0 byte 時, 丟出本例外</exception>
		///<returns></returns>>
		public string SaveAs(string path, IFormFile file)
		 {

			if (file == null || string.IsNullOrEmpty(file.FileName) || file.Length == 0)
			{
				throw new UploadFileNullException();
			}

			string ext= System.IO.Path.GetExtension(file.FileName);
			string newFileName= Guid.NewGuid().ToString("N") + ext;
			string fullPath= path + newFileName;
			using (var stream = System.IO.File.Create(fullPath))
			{
				file.CopyToAsync(stream);
			}
			return newFileName;
		}
	}

	public class UploadFileNullException : ApplicationException
	{
	}
}