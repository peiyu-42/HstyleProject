using HStyleApi.Controllers;
using HStyleApi.Models.DTOs;
using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System.ComponentModel.Design;

namespace HStyleApi.Models.Services
{
	public class VideoService
	{
		private VideoRepository _videoRepository;

		public VideoService(AppDbContext db)
		{
			_videoRepository = new VideoRepository(db);
		}

		public async Task<IEnumerable<VideoDTO>> GetVideos(string? keyword)
		{
			var data = await _videoRepository.GetVideos(keyword);
			return data;
		}

		public async Task<VideoDTO> GetVideo(int videoId)
		{
			var data = _videoRepository.GetVideo(videoId);
			return await data;
		}

		public async Task<IEnumerable<ProductDto>> GetRecommendationProduct(int videoId)
		{
			var data = _videoRepository.GetRecommendationProduct(videoId);
			return await data;
		}

		public async Task<IEnumerable<VideoDTO>> GetNews()
		{
			var data = _videoRepository.GetNews();
			return await data;
		}

		public async Task<IEnumerable<VideoLikeDTO>> GetLikeVideos(int memberId)
		{
			var data = await _videoRepository.GetLikeVideos(memberId);
			return data;
		}

		public void PostLike(int memberId, int videoId)
		{
			if (memberId == 0 || videoId == 0) throw new Exception();
			else _videoRepository.PostLike(memberId, videoId);
		}

		public void PostView(int videoId)
		{
			_videoRepository.PostView(videoId);
		}

		public async Task<IEnumerable<VideoCommentDTO>> GetComments(int videoId)
		{
			return await _videoRepository.GetComments(videoId);
		}

		public void CreateComment(string comment, int memberId, int videoId)
		{
			_videoRepository.CreateComment(comment, memberId, videoId);
		}

		public void PostCommentLike(int memberId, int commentId)
		{
			_videoRepository.PostCommentLike(memberId, commentId);
		}

		public async Task<IEnumerable<VCommentLikeDTO>> GetCommentLikes(int memberId)
		{
			var data = await _videoRepository.GetCommentLikes(memberId);
			return data;
		}
	}
}
