using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.SearchAndDownloadModule.DownloadModule
{
    public partial class DownloadModulePage : ComponentBase
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
        private bool ClearOutputFolder { get; set; } = false;
        [Inject]
        protected IVideoService? VideoService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            IsLoading = true;

            var entity = await VideoService!.DownloadUpdateVideos(Uri!, Token!);
            if (entity is not null)
            {
                IsLoading = false;
                MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<VideoViewModel>()
                {
                    IsSuccess = true,
                    MessageStatus = entity.StatusCode == System.Net.HttpStatusCode.OK ?
                    new MessageStatus()
                    {
                        StatusCode = entity.StatusCode,
                        Message = "Videos downloaded successfully !"
                    } :
                    new MessageStatus()
                    {
                        StatusCode = entity.StatusCode,
                        Message = "Download Failed !"
                    },
                    Entity = entity
                }));
            }
        }

    }
}
