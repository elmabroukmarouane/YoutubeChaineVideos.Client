using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeUploadVideoCredential
{
    public partial class YouTubeUploadVideoCredentialDelete : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeUploadVideoCredentialViewModel? YouTubeUploadVideoCredentialViewModel { get; set; }
        [Parameter]
        public string? ContentMessageDelete { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeUploadVideoCredentialViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeUploadVideoCredentialViewModelResponse = new YouTubeUploadVideoCredentialViewModel();
            YouTubeUploadVideoCredentialViewModelResponse = await GenericService!.DeleteAsync(Uri!, Token, YouTubeUploadVideoCredentialViewModel!);
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeUploadVideoCredentialViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ? 
                new MessageStatus()
                {
                    StatusCode = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeUploadVideoCredentialViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity deleted successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode,
                    Message = "Delete Failed !"
                },
                Entity = YouTubeUploadVideoCredentialViewModelResponse
            }));
        }

    }
}
