using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiChannel
{
    public partial class YouTubeApiChannelAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiChannelViewModel? YouTubeApiChannelViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiChannelViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeApiChannelViewModelResponse = new YouTubeApiChannelViewModel();
            if (IsUpdate)
            {
                YouTubeApiChannelViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeApiChannelViewModel!);
            }
            else
            {
                YouTubeApiChannelViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeApiChannelViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeApiChannelViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeApiChannelViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ?
                new MessageStatus()
                {
                    StatusCode = YouTubeApiChannelViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeApiChannelViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeApiChannelViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeApiChannelViewModelResponse
            }));
        }

    }
}
