using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using System;
using YoutubeChaineVideos.Client.Shared.Components.Common;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiConfig
{
    public partial class YouTubeApiConfigAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiConfigViewModel? YouTubeApiConfigViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        protected IList<YouTubeApiChannelViewModel>? YouTubeApiChannelViewModels { get; set; }
        protected YouTubeApiChannelViewModel? SelectedYouTubeApiChannelViewModel { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiChannelViewModel>? GenericServiceChannel { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiConfigViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private string GetDisplayName(YouTubeApiChannelViewModel? youTubeApiChannelViewModel) => $"{youTubeApiChannelViewModel?.ChannelName}";
        protected override async Task OnInitializedAsync()
        {
            YouTubeApiChannelViewModels = await GenericServiceChannel!.GetEntitiesAsync(BaseSettingsApp?.BaseUrlApiWebHttp + "Channel", null);
            SelectedYouTubeApiChannelViewModel = YouTubeApiConfigViewModel?.YouTubeApiChannelViewModel;
        }

        private Task<IEnumerable<YouTubeApiChannelViewModel>> SearchYouTubeApiChannelViewModelsAsync(string searchText, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                // Check if the operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    return [];

                // Perform the search logic.
                var results = string.IsNullOrWhiteSpace(searchText)
                    ? YouTubeApiChannelViewModels
                    : YouTubeApiChannelViewModels?.Where(x => x.ChannelName!.Contains(searchText, StringComparison.OrdinalIgnoreCase));

                return results!.AsEnumerable();
            });
        }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeApiConfigViewModelResponse = new YouTubeApiConfigViewModel();
            YouTubeApiConfigViewModel!.ChannelId = SelectedYouTubeApiChannelViewModel!.Id;
            if (IsUpdate)
            {
                YouTubeApiConfigViewModel!.YouTubeApiChannelViewModel = SelectedYouTubeApiChannelViewModel;
                YouTubeApiConfigViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeApiConfigViewModel!);
            }
            else
            {
                YouTubeApiConfigViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeApiConfigViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeApiConfigViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeApiConfigViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ?
                new MessageStatus()
                {
                    StatusCode = YouTubeApiConfigViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeApiConfigViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeApiConfigViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeApiConfigViewModelResponse
            }));
        }

    }
}
