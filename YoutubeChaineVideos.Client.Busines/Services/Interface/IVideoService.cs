using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Requests;

namespace YoutubeChaineVideos.Client.Busines.Services.Interface
{
    public interface IVideoService
    {
        #region SEARCH AND ADD YOUTUBE VIDEOS IN DB
        Task<VideoViewModel?> SearchAddVideos(string uri, string? token, SearchAddVideosRequest searchAddVideosRequest);
        #endregion
    }
}