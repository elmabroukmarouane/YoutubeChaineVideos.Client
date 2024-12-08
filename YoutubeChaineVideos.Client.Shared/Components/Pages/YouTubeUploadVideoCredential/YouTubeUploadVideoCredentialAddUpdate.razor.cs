using YoutubeChaineVideos.Client.Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using System.Reflection;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeUploadVideoCredential
{
    public partial class YouTubeUploadVideoCredentialAddUpdate : ComponentBase
    {
        private bool Success { get; set; } = true;
        private string[] Errors { get; set; } = [];
        private MudForm? Form { get; set; }
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public YouTubeUploadVideoCredentialViewModel? YouTubeUploadVideoCredentialViewModel { get; set; }
        [Parameter]
        public string? TitleOkButton { get; set; }
        [Parameter]
        public bool IsUpdate { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        [Parameter]
        public string? Token { get; set; }
        [Inject]
        protected IGenericService<YouTubeUploadVideoCredentialViewModel>? GenericService { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task Ok()
        {
            var YouTubeUploadVideoCredentialViewModelResponse = new YouTubeUploadVideoCredentialViewModel();
            if (IsUpdate)
            {
                YouTubeUploadVideoCredentialViewModelResponse = await GenericService!.UpdateAsync(Uri!, Token, YouTubeUploadVideoCredentialViewModel!);
            }
            else
            {
                YouTubeUploadVideoCredentialViewModelResponse = await GenericService!.CreateAsync(Uri!, Token, YouTubeUploadVideoCredentialViewModel!);
            }
            if (Success || Errors.Length <= 0) MudDialog?.Close(DialogResult.Ok(new EntityDbResponse<YouTubeUploadVideoCredentialViewModel>()
            {
                IsSuccess = true,
                MessageStatus = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode == System.Net.HttpStatusCode.OK ?
                new MessageStatus()
                {
                    StatusCode = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode,
                    Message = typeof(YouTubeUploadVideoCredentialViewModel).ToString().Replace("YoutubeChaineVideos.Client.Domain.Models.", string.Empty).Replace("ViewModel", string.Empty) + " entity" + (IsUpdate ? " updated" : " added") + " successfully"
                } :
                new MessageStatus()
                {
                    StatusCode = YouTubeUploadVideoCredentialViewModelResponse?.StatusCode,
                    Message = (IsUpdate ? "Update" : "Add") + " Failed !"
                },
                Entity = YouTubeUploadVideoCredentialViewModelResponse
            }));
        }

        private string GetPropertyValue(PropertyInfo property) =>
        property.GetValue(YouTubeUploadVideoCredentialViewModel)?.ToString() ?? string.Empty;

        private void OnValueChanged(PropertyInfo property, string newValue)
        {
            try
            {
                var convertedValue = Convert.ChangeType(newValue, property.PropertyType);
                property.SetValue(YouTubeUploadVideoCredentialViewModel, convertedValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error setting value for {property.Name}: {ex.Message}", ex);
            }
        }
    }
}
