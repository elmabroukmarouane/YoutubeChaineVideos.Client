namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class YouTubeApiSearchQueryViewModel : Entity
    {
        public string? SearchQuery { get; set; }
        public string? NextPageToken { get; set; }
        public bool? IsActive { get; set; }
    }
}
