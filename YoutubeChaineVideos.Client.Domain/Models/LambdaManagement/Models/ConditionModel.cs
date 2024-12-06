namespace YoutubeChaineVideos.Client.Domain.Models.LambdaManagement.Models
{
    public class ConditionModel
    {
        public string PropertyName { get; set; } = string.Empty;
        public string? ComparisonValue { get; set; } = string.Empty;
        public string ComparisonType { get; set; } = string.Empty;
    }
}
