namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class VideoViewModel : Entity
    {
        public string? VideoId { get; set; }
        public string? CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        public string? Tags { get; set; }
        public string? ChannelId { get; set; }
        public string? ChannelTitle { get; set; }
        public string? DownloadedName { get; set; }
        public string? DownloadedPath { get; set; }
        public string? EditedName { get; set; }
        public string? EditedPath { get; set; }
        public double? Duration { get; set; }
        public bool? IsDownloaded { get; set; }
        public bool? IsEdited { get; set; }
        public bool? IsUploaded { get; set; }
    }
}
