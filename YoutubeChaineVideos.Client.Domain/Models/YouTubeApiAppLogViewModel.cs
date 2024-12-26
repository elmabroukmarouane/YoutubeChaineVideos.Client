namespace YoutubeChaineVideos.Client.Domain.Models
{
    /// <summary>
    /// Level: Info, Warning, Error
    /// Message: Log message
    /// Source: "Web", "Android", "Windows Desktop", "Linux Desktop", "iOS"
    /// </summary>
    public class YouTubeApiAppLogViewModel : Entity
    {
        public string? Level { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
    }
}
