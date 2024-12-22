using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.EditingModule
{
    public partial class ClearEditingFoldersModulePage : ComponentBase
    {
        private bool IsLoading {  get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IVideoService? VideoService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            IsLoading = true;

            var messageStatus = await VideoService!.ClearAndEmptyVideosAudiosOutputFolders(Uri!, Token);
            IsLoading = true;
            if (messageStatus is not null)
            {
                IsLoading = false;
                MudDialog?.Close(DialogResult.Ok(messageStatus));
            }
        }

    }
}
