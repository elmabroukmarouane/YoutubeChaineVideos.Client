using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.SearchAndDownloadModule.ClearTableMobule
{
    public partial class ClearTableMobulePage : ComponentBase
    {
        private bool IsLoading {  get; set; }
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        private bool IsSqlite { get; set; } = true;
        [Inject]
        protected IVideoService? VideoService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            IsLoading = true;

            var messageStatus = await VideoService!.Truncate($"{Uri}?isSqlite={IsSqlite}", Token);
            if (Success || Errors.Length <= 0)
            {
                IsLoading = true;
                if (messageStatus is not null)
                {
                    IsLoading = false;
                    MudDialog?.Close(DialogResult.Ok(messageStatus));
                }
            }
        }

    }
}
