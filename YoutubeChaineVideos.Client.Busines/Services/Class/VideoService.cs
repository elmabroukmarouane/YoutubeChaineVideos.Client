using YoutubeChaineVideos.Client.Busines.Services.Interface;
using System.Net.Http.Json;
using YoutubeChaineVideos.Client.Domain.Models.Requests;
using YoutubeChaineVideos.Client.Domain.Models;

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
