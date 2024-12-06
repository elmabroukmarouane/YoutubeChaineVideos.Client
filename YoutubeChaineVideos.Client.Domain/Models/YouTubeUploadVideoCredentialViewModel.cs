namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class YouTubeUploadVideoCredentialViewModel : Entity
    {
        public string? ApplicationName { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? AuthUri { get; set; }
        public string? TokenUri { get; set; }
        public string? RedirectUris { get; set; }
    }
}
