using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using YoutubeChaineVideos.Client.Shared.Components.Common;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeVideoCategory
{
    public partial class YouTubeVideoCategoryDelete : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeVideoCategoryViewModel? YouTubeVideoCategoryViewModel { get; set; }
        [Parameter]
        public string? ContentMessageDelete { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeVideoCategoryViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        protected override void OnInitialized() => Uri = BaseSettingsApp?.BaseUrlApiWebHttp + "VideoCategory";

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeVideoCategoryViewModelResponse = new YouTubeVideoCategoryViewModel();
            YouTubeVideoCategoryViewModelResponse = await GenericService!.DeleteAsync(Uri!, Token, YouTubeVideoCategoryViewModel!);
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeVideoCategoryViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeVideoCategoryViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ? 
                new MessageStatus()
                {
                    StatusCode = YouTubeVideoCategoryViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeVideoCategoryViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity deleted successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeVideoCategoryViewModelResponse?.StatusCode,
                    Message = "Delete Failed !"
                },
                Entity = YouTubeVideoCategoryViewModelResponse
            }));
        }

    }
}
