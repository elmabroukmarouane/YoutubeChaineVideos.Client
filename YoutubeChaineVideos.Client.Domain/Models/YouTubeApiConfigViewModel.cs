namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class YouTubeApiConfigViewModel : Entity
    {
        public int ChannelId { get; set; }
        public string? ApiKey { get; set; }
        public int? MaxResults { get; set; }
        public int? DailyQuota { get; set; }
        public double? DurationQuota { get; set; }
        public int? SearchCost { get; set; }
        public int? SearchTagsCost { get; set; }
        public YouTubeApiChannelViewModel? YouTubeApiChannelViewModel { get; set; }
    }
}
