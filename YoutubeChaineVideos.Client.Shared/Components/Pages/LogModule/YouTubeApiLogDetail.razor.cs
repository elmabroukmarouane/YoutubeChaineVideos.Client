using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.LogModule
{
    public partial class YouTubeApiLogDetail : ComponentBase
    {
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiAppLogViewModel? YouTubeApiAppLogViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        public string? PrettyJsonLog { get; set; }

        private void Cancel() => MudDialog?.Cancel();
    }
}
