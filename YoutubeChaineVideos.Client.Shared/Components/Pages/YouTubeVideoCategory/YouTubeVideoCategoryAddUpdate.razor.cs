using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeVideoCategory
{
    public partial class YouTubeVideoCategoryAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeVideoCategoryViewModel? YouTubeVideoCategoryViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeVideoCategoryViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeVideoCategoryViewModelResponse = new YouTubeVideoCategoryViewModel();
            if (IsUpdate)
            {
                YouTubeVideoCategoryViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeVideoCategoryViewModel!);
            }
            else
            {
                YouTubeVideoCategoryViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeVideoCategoryViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeVideoCategoryViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeVideoCategoryViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ? 
                new MessageStatus()
                {
                    StatusCode = YouTubeVideoCategoryViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeVideoCategoryViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeVideoCategoryViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeVideoCategoryViewModelResponse
            }));
        }

    }
}
