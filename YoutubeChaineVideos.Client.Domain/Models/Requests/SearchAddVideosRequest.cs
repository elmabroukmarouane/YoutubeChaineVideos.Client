namespace YoutubeChaineVideos.Client.Domain.Models.Requests
{
    public class SearchAddVideosRequest
    {
        public required int ChannelId { get; set; }
        public required string QuerySearch { get; set; }
        public required int MaxResults { get; set; }
        public bool GetRecentVideos { get; set; } = false;
        public string VideoType { get; set; } = "video";
    }
}
