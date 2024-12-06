using YoutubeChaineVideos.Client.Domain.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages
{
    //[Authorize]
    public partial class Home
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        public string? TitleContent { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingApp { get; set; }
        protected override void OnInitialized() => TitleContent = BaseSettingApp!.BaseTitleApp + " - Home";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime!.InvokeVoidAsync("initAdminTemplate");
            }
        }
    }
}
