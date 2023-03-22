using H2StyleStore.Models.DTOs;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace H2StyleStore.Models.Services
{
    public class VideoService
    {
        private IVideoRepository _repository;
        public VideoService(IVideoRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<VideoDto> GetVideos()
        {
            return _repository.GetVideos();
        }

        public (bool IsSuccess,string ErrorMessage) CreateVideo(CreateVideoDto dto)
        {

			if (_repository.IsExist(dto.Image,dto.FilePath))
			{
				return (false, "這部影片已經上傳過了");
			}

            if (dto.FilePath == null) return (false, "請上傳影片檔案");

			//if (dto.OnShelffTime == null && dto.OffShelffTime == null) dto.IsOnShelff = true;
			//else if (dto.OnShelffTime.HasValue && dto.OffShelffTime == null)
			//{
			//	if (DateTime.Now >= dto.OnShelffTime) dto.IsOnShelff = true;
			//	else dto.IsOnShelff = false;
			//}
			//else if (dto.OnShelffTime == null && dto.OffShelffTime.HasValue)
			//{
			//	if (DateTime.Now < dto.OffShelffTime) dto.IsOnShelff = true;
			//	else dto.IsOnShelff = false;
			//}
			//else
			//{
			//	if (DateTime.Now >= dto.OnShelffTime && DateTime.Now < dto.OffShelffTime) dto.IsOnShelff = true;
			//	else dto.IsOnShelff = false;
			//}

			_repository.CreateVideo(dto);
            return (true, null);
        }

		public void UpdateVideo(UpdateVideoDto request)
		{
            UpdateVideoDto entity = _repository.GetVideoById(request.Id);
			if (entity == null)
			{
				throw new Exception("找不到此影片");
			}

			//if (request.OnShelffTime == null && request.OffShelffTime == null) request.IsOnShelff = true;
			//else if (request.OnShelffTime.HasValue && request.OffShelffTime == null)
			//{
			//	if (DateTime.Now >= request.OnShelffTime) request.IsOnShelff = true;
			//	else request.IsOnShelff = false;
			//}
			//else if (request.OnShelffTime == null && request.OffShelffTime.HasValue)
			//{
			//	if (DateTime.Now < request.OffShelffTime) request.IsOnShelff = true;
			//	else request.IsOnShelff = false;
			//}
			//else
			//{
			//	if (DateTime.Now >= request.OnShelffTime && DateTime.Now < request.OffShelffTime) request.IsOnShelff = true;
			//	else request.IsOnShelff = false;
			//}
			entity.Title = request.Title;
			entity.Description = request.Description;
			entity.CategoryId = request.CategoryId;
			entity.FilePath = request.FilePath;
			entity.OffShelffTime = request.OffShelffTime;
			entity.OnShelffTime = request.OnShelffTime;
			entity.CreatedTime = request.CreatedTime;
			entity.Tags = request.Tags;
			entity.Image = request.Image;
			//entity.IsOnShelff = request.IsOnShelff;

			_repository.Update(entity);
		}
	}
}