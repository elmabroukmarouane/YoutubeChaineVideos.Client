using System.Net;

namespace YoutubeChaineVideos.Client.Domain.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }
}
