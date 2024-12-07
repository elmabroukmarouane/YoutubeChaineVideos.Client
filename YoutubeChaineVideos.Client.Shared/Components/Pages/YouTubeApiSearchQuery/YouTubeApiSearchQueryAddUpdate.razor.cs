using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiSearchQuery
{
    public partial class YouTubeApiSearchQueryAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiSearchQueryViewModel? YouTubeApiSearchQueryViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiSearchQueryViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        protected override void OnInitialized() => Uri = BaseSettingsApp?.BaseUrlApiWebHttp + "SearchQuery";

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeApiSearchQueryViewModelResponse = new YouTubeApiSearchQueryViewModel();
            if (IsUpdate)
            {
                YouTubeApiSearchQueryViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeApiSearchQueryViewModel!);
            }
            else
            {
                YouTubeApiSearchQueryViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeApiSearchQueryViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeApiSearchQueryViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeApiSearchQueryViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ?
                new MessageStatus()
                {
                    StatusCode = YouTubeApiSearchQueryViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeApiSearchQueryViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeApiSearchQueryViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeApiSearchQueryViewModelResponse
            }));
        }

    }
}
