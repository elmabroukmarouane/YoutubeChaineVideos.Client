using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using YoutubeChaineVideos.Client.Domain.Models.Requests;
using YoutubeChaineVideos.Client.Domain.Models.Responses;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.EditingModule
{
    public partial class EditingModulePage : ComponentBase
    {
        private bool IsLoading {  get; set; }
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
        private bool IsRotated { get; set; } = true;
        private bool IsEffected { get; set; } = false;
        private bool IsOverlaid { get; set; } = true;
        [Inject]
        protected IVideoService? VideoService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        protected EditingVideosRequest? EditingVideosRequest { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            EditingVideosRequest = new EditingVideosRequest()
            {
                IsRotated = IsRotated,
                IsEffected = IsEffected,
                IsOverlaid = IsOverlaid
            };

            if (EditingVideosRequest == null)
            {
                MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<VideoViewModel>()
                {
                    IsSuccess = false,
                    MessageStatus = new MessageStatus()
                    {
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Message = "Editing request is required !"
                    },
                    Entity = null
                }));
            }

            IsLoading = true;

            var entity = await VideoService!.EditingVideos(Uri!, Token, EditingVideosRequest!);
            if (Success || Errors.Length <= 0)
            {
                IsLoading = false;
                if (entity is not null)
                {
                    MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<VideoViewModel>()
                    {
                        IsSuccess = true,
                        MessageStatus = entity.StatusCode == System.Net.HttpStatusCode.OK ?
                        new MessageStatus()
                        {
                            StatusCode = entity.StatusCode,
                            Message = "Editing done successfully !"
                        } :
                        new MessageStatus()
                        {
                            StatusCode = entity.StatusCode,
                            Message = "Editing Failed !"
                        },
                        Entity = entity
                    }));
                }
            }
        }

    }
}
