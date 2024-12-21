using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using YoutubeChaineVideos.Client.Domain.Models.Requests;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using CurrieTechnologies.Razor.SweetAlert2;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Radzen;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.SearchAndDownloadModule.SearchModule
{
    public partial class SearchModulePage : ComponentBase
    {
        private bool isLoading {  get; set; }
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
        protected SearchAddVideosRequest? SearchAddVideosRequest { get; set; }
        private string? QuerySearch { get; set; } = string.Empty;
        private int MaxResults { get; set; } = 0;
        private string? VideoType { get; set; } = "video";
        private bool GetRecentVideos { get; set; } = false;
        protected IList<YouTubeApiChannelViewModel>? YouTubeApiChannelViewModels { get; set; }
        protected YouTubeApiChannelViewModel? SelectedYouTubeApiChannelViewModel { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiChannelViewModel>? GenericServiceChannel { get; set; }
        [Inject]
        protected IVideoService? VideoService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private string GetDisplayName(YouTubeApiChannelViewModel? youTubeApiChannelViewModel) => $"{youTubeApiChannelViewModel?.ChannelName}";
        protected override async Task OnInitializedAsync()
        {
            YouTubeApiChannelViewModels = await GenericServiceChannel!.GetEntitiesAsync(BaseSettingsApp?.BaseUrlApiWebHttp + "Channel", null);
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
            SearchAddVideosRequest = new SearchAddVideosRequest()
            {
                ChannelId = SelectedYouTubeApiChannelViewModel!.Id,
                MaxResults = MaxResults,
                QuerySearch = QuerySearch!,
                VideoType = VideoType!,
                GetRecentVideos = GetRecentVideos
            };

            if (SearchAddVideosRequest == null)
            {
                // Log or handle the null scenario
                return;
            }

            isLoading = true;

            var entity = await VideoService!.SearchAddVideos(Uri!, Token, SearchAddVideosRequest!);
            if (Success || Errors.Length <= 0)
            {
                isLoading = false;
                if (entity is not null && entity.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<VideoViewModel>()
                    {
                        IsSuccess = true,
                        MessageStatus = entity.StatusCode == System.Net.HttpStatusCode.OK ?
                        new MessageStatus()
                        {
                            StatusCode = entity.StatusCode,
                            Message = "Search done successfully !"
                        } :
                        new MessageStatus()
                        {
                            StatusCode = entity.StatusCode,
                            Message = "Search Failed !"
                        },
                        Entity = entity
                    }));
                }
            }
        }

    }
}
