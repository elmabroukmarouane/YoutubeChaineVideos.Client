using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiRemainingQuota
{
    public partial class YouTubeApiRemainingQuotaAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeApiRemainingQuotaViewModel? YouTubeApiRemainingQuotaViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiRemainingQuotaViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        protected override void OnInitialized() => Uri = BaseSettingsApp?.BaseUrlApiWebHttp + "RemainingQuota";

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeApiRemainingQuotaViewModelResponse = new YouTubeApiRemainingQuotaViewModel();
            YouTubeApiRemainingQuotaViewModel!.RemainingQuotaDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (IsUpdate)
            {
                YouTubeApiRemainingQuotaViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeApiRemainingQuotaViewModel!);
            }
            else
            {
                YouTubeApiRemainingQuotaViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeApiRemainingQuotaViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeApiRemainingQuotaViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeApiRemainingQuotaViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ?
                new MessageStatus()
                {
                    StatusCode = YouTubeApiRemainingQuotaViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeApiRemainingQuotaViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeApiRemainingQuotaViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeApiRemainingQuotaViewModelResponse
            }));
        }

    }
}
