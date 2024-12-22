using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Requests;
using YoutubeChaineVideos.Client.Domain.Models.Responses;

namespace YoutubeChaineVideos.Client.Busines.Services.Interface
{
    public interface IVideoService
    {
        #region SEARCH AND ADD YOUTUBE VIDEOS IN DB
        Task<VideoViewModel?> SearchAddVideos(string uri, string? token, SearchAddVideosRequest searchAddVideosRequest);
        #endregion

        #region DOWNLOAD AND UPDATE VIDEOS IN DB
        Task<VideoViewModel?> DownloadUpdateVideos(string uri, string? token);
        #endregion

        #region CLEAR AND EMPTY TEMP VIDEOS, AUDIOS AND OUTPUT FOLDERS
        Task<MessageStatus> ClearAndEmptyVideosAudiosOutputFolders(string uri, string? token);
        #endregion

        #region EDITING VIDEOS IN DB
        Task<VideoViewModel?> EditingVideos(string uri, string? token, EditingVideosRequest editingVideosRequest);
        #endregion

        #region UPLOAD VIDEOS TO YOUTUBE
        Task<MessageStatus> UploadVideosToYouTube(string uri, string? token);
        #endregion

        #region TRUNCATE VIDEOS TABLE
        Task<MessageStatus> Truncate(string uri, string? token);
        #endregion
    }
}