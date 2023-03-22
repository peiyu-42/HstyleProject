using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.UI.WebControls;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
	public class VideoRepository : IVideoRepository
	{
		private readonly AppDbContext _db;

		public VideoRepository(AppDbContext db)
		{
			_db = db;
		}
		public IEnumerable<VideoDto> GetVideos()
		{
			IEnumerable<Video> query = _db.Videos;
			return query.Select(v => v.ToDto());
		}

		public void CreateVideo(CreateVideoDto dto)
		{
			string path = dto.Image;
			Image image = new Image { Path = path };
			_db.Images.Add(image);
			_db.SaveChanges();

			var imageid = _db.Images.Where(i => i.Path == image.Path).FirstOrDefault();
			Video video = new Video()
			{
				Title = dto.Title,
				Description = dto.Description,
				FilePath = dto.FilePath,
				CategoryId = dto.CategoryId,
				ImageId = imageid.Image_Id,
				OnShelffTime = dto.OnShelffTime,
				OffShelffTime = dto.OffShelffTime,
				//IsOnShelff = dto.IsOnShelff,
				CreatedTime = DateTime.Now
			};
			_db.Videos.Add(video);

			foreach (string tag in dto.Tags)
			{
				var tags = _db.Tags.Select(t => t.TagName).ToList();
				if (tags.Contains(tag) == true)
				{
					Tag oldTag = _db.Tags.Where(t => t.TagName == tag).FirstOrDefault();
					video.Tags.Add(oldTag);
				}
				else
				{
					Tag newTag = new Tag { TagName = tag };
					video.Tags.Add(newTag);
				}
			}
			_db.SaveChanges();

			var videoOther = _db.Videos.SingleOrDefault(v => v.Title == dto.Title);
			VideoView videoView = new VideoView()
			{
				VideoId = videoOther.Id,
				Views = 0,
			};

			_db.VideoViews.Add(videoView);
			_db.SaveChanges();
		}

		public IEnumerable<SelectListItem> GetVideoCategories()
		{
			var data = _db.VideoCategories;
			foreach (var item in data)
			{
				yield return new SelectListItem { Value = item.Id.ToString(), Text = item.CategoryName };
			}
		}

		public IEnumerable<SelectListItem> GetVideoCategories(int? categoryId)
		{
			var data = _db.VideoCategories.Select(x => new SelectListItem
			{ Value = x.Id.ToString(), Text = x.CategoryName, Selected = (categoryId.HasValue && x.Id == categoryId) })
			.ToList().Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇" });
			return data;
		}

		public UpdateVideoDto GetVideoById(int videoId)
		{
			UpdateVideoDto query = _db.Videos.FirstOrDefault(v => v.Id == videoId).ToEditDto();
			return query;
		}

		public bool IsExist(string image, string filePath)
		{
			var video = _db.Videos.SingleOrDefault(x => x.Image.Path == image);
			video = _db.Videos.SingleOrDefault(x => x.FilePath == filePath);
			return (video != null);
		}
		public void Update(UpdateVideoDto dto)
		{
			string path = dto.Image;
			Image image = new Image { Path = path };
			_db.Images.Add(image);
			_db.SaveChanges();

			var imageid = _db.Images.Where(i => i.Path == image.Path).FirstOrDefault();

			Video video = _db.Videos.Find(dto.Id);
			video.Title = dto.Title;
			video.Description = dto.Description;
			video.CreatedTime = DateTime.Now;
			video.CategoryId = dto.CategoryId;
			video.FilePath = dto.FilePath;
			video.ImageId = imageid.Image_Id;
			video.OnShelffTime = dto.OnShelffTime;
			video.OffShelffTime = dto.OffShelffTime;
			video.IsOnShelff = dto.IsOnShelff;

			foreach (var dbTag in video.Tags.ToArray())
			{
				video.Tags.Remove(dbTag);
			}

			foreach (var tag in dto.Tags)
			{
				var tags = _db.Tags.Select(t => t.TagName).ToList();
				if (tags.Contains(tag) == true)
				{
					Tag oldTag = _db.Tags.Where(t => t.TagName == tag).FirstOrDefault();
					video.Tags.Add(oldTag);
				}
				else
				{
					Tag newTag = new Tag { TagName = tag };
					video.Tags.Add(newTag);
				}
			}
			_db.SaveChanges();
		}

		public void Update(CreateVideoDto entity)
		{
			throw new NotImplementedException();
		}
	}
}