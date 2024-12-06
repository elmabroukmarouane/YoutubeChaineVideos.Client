using System.Net;

namespace YoutubeChaineVideos.Client.Domain.Models.Responses
{
    public class MessageStatus
    {
        public HttpStatusCode? StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
