using H2StyleStore.Models.DTOs;
using System.Collections.Generic;
using System.Web.Mvc;

namespace H2StyleStore.Models.Infrastructures.Repositories
{
    public interface IVideoRepository
    {
        IEnumerable<VideoDto> GetVideos();
        void CreateVideo(CreateVideoDto dto);
        bool IsExist(string image, string filePath);
		UpdateVideoDto GetVideoById(int videoId);
		void Update(UpdateVideoDto entity);
        IEnumerable<SelectListItem> GetVideoCategories();
        IEnumerable<SelectListItem> GetVideoCategories(int? categoryId);
	}
}