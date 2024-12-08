using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiSearchQuery
{
    public partial class YouTubeApiSearchQueryDelete : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiSearchQueryViewModel? YouTubeApiSearchQueryViewModel { get; set; }
        [Parameter]
        public string? ContentMessageDelete { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiSearchQueryViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeApiSearchQueryViewModelResponse = new YouTubeApiSearchQueryViewModel();
            YouTubeApiSearchQueryViewModelResponse = await GenericService!.DeleteAsync(Uri!, Token, YouTubeApiSearchQueryViewModel!);
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeApiSearchQueryViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeApiSearchQueryViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ? 
                new MessageStatus()
                {
                    StatusCode = YouTubeApiSearchQueryViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeApiSearchQueryViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity deleted successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeApiSearchQueryViewModelResponse?.StatusCode,
                    Message = "Delete Failed !"
                },
                Entity = YouTubeApiSearchQueryViewModelResponse
            }));
        }

    }
}
