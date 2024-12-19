namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class YouTubeApiChannelViewModel : Entity
    {
        public string? ChannelName { get; set; }
        public ICollection<YouTubeApiConfigViewModel>? YouTubeApiConfigViewModels { get; set; }
    }
}
