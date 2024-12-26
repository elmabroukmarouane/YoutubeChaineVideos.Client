using YoutubeChaineVideos.Client.Busines.Services.Interface;

namespace YoutubeChaineVideos.Client.Shared.Services.Classes
{
    public class YouTubeSourceAppProvider : IYouTubeSourceAppProvider
    {
        private readonly string _sourceApp;

        public YouTubeSourceAppProvider(string sourceApp)
        {
            _sourceApp = sourceApp;
        }

        public string GetSourceApp() => _sourceApp;
    }
}
