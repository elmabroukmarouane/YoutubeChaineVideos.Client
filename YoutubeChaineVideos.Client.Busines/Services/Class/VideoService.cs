using YoutubeChaineVideos.Client.Busines.Services.Interface;
using System.Net.Http.Json;
using YoutubeChaineVideos.Client.Domain.Models.Requests;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Responses;

namespace YoutubeChaineVideos.Client.Busines.Services.Class
{
    public class VideoService(HttpClient httpClient) : IVideoService
    {
        #region ATTRIBUTES
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentException(null, nameof(httpClient));
        #endregion

        #region SEARCH AND ADD YOUTUBE VIDEOS IN DB
        public async Task<VideoViewModel?> SearchAddVideos(string uri, string? token, SearchAddVideosRequest searchAddVideosRequest)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.PostAsJsonAsync(uri, searchAddVideosRequest);
                var entity = new VideoViewModel
                {
                    StatusCode = response.StatusCode
                };
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region DOWNLOAD AND UPDATE VIDEOS IN DB
        public async Task<VideoViewModel?> DownloadUpdateVideos(string uri, string? token)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.GetAsync(uri);
                var entity = new VideoViewModel
                {
                    StatusCode = response.StatusCode
                };
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region EDITING VIDEOS IN DB
        public async Task<VideoViewModel?> EditingVideos(string uri, string? token, EditingVideosRequest editingVideosRequest)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.PostAsJsonAsync(uri, editingVideosRequest);
                var entity = new VideoViewModel
                {
                    StatusCode = response.StatusCode
                };
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region CLEAR AND EMPTY TEMP VIDEOS, AUDIOS AND OUTPUT FOLDERS
        public async Task<MessageStatus> ClearAndEmptyVideosAudiosOutputFolders(string uri, string? token)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.GetAsync(uri);
                return new MessageStatus()
                {
                    StatusCode = response.StatusCode,
                    Message = response.Content.ReadAsStringAsync().Result
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPLOAD VIDEOS TO YOUTUBE
        public async Task<MessageStatus> UploadVideosToYouTube(string uri, string? token)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.GetAsync(uri);
                return new MessageStatus()
                {
                    StatusCode = response.StatusCode,
                    Message = response.Content.ReadAsStringAsync().Result
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region TRUNCATE VIDEOS TABLE
        public async Task<MessageStatus> Truncate(string uri, string? token)
        {
            try
            {
                // SetTokenToHeader(token);
                var response = await _httpClient.GetAsync(uri);
                return new MessageStatus()
                {
                    StatusCode = response.StatusCode,
                    Message = response.Content.ReadAsStringAsync().Result
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region ADDTOKEN
        private void SetTokenToHeader(string? token)
        {
            if (token != null)
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
            }
        }
        #endregion
    }
}
