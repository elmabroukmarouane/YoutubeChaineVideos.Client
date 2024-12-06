namespace YoutubeChaineVideos.Client.Domain.Models.LambdaManagement.Models
{
    public class ConditionGroupModel
    {
        public string LogicalOperator { get; set; } = "AND";
        public List<ConditionModel> Conditions { get; set; } = [];
        public List<ConditionGroupModel> Groups { get; set; } = [];
    }
}
