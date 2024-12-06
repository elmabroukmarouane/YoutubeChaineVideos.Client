namespace YoutubeChaineVideos.Client.Domain.Models.Settings
{
    public class BaseSettingsApp
    {
        public required string ChosenEnviroment { get; set; }
        public required string BaseUrlApiWebHttp { get; set; }
        public required string BaseUrlApiAndroidHttp { get; set; }
        public required string BaseTitleApp { get; set; }
    }
}
