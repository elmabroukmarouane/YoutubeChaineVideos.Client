using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.LogModule
{
    public partial class YouTubeApiAppLogDelete : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public IList<YouTubeApiAppLogViewModel>? YouTubeApiAppLogViewModels { get; set; }
        [Parameter]
        public string? ContentMessageDelete { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiAppLogViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private DateTime? DateToday { get; set; } = DateTime.Today;
        public IList<YouTubeApiAppLogViewModel>? ItemsToDelete { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            if (DateToday != null)
            {
                var dateToday = DateToday.Value.ToString("dd/MM/yyyy");
                ItemsToDelete = YouTubeApiAppLogViewModels?.Where(x => x.CreateDate.GetValueOrDefault().ToString("dd/MM/yyyy").Contains(dateToday)).ToList();
            }
            else
            {
                return;
            }
            var YouTubeApiAppLogViewModelsResponse = (List<YouTubeApiAppLogViewModel>?)await GenericService!.DeleteAsync(Uri!, Token, ItemsToDelete!);
            if (Success || Errors.Length <= 0)
            {
                if (YouTubeApiAppLogViewModelsResponse is null || !YouTubeApiAppLogViewModelsResponse.Any())
                {
                    MudDialog?.Close(DialogResult.Ok(new MessageStatus()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Message = "There is nothing to delete !"
                    }));
                }
                else
                {
                    MudDialog?.Close(DialogResult.Ok(
                        YouTubeApiAppLogViewModelsResponse?[0].StatusCode == System.Net.HttpStatusCode.OK ?
                        new MessageStatus()
                        {
                            StatusCode = YouTubeApiAppLogViewModelsResponse?[0].StatusCode,
                            Message = typeof(YouTubeApiAppLogViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entities deleted successfully"
                        } :
                        new MessageStatus()
                        {
                            StatusCode = YouTubeApiAppLogViewModelsResponse?[0].StatusCode,
                            Message = "Delete Failed !"
                        }
                    ));
                }
            }
        }

    }
}
