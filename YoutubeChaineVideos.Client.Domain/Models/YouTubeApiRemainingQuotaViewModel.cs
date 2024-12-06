namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class YouTubeApiRemainingQuotaViewModel : Entity
    {
        public int? RemainingQuota { get; set; }
        public string? RemainingQuotaDate { get; set; }
    }
}
