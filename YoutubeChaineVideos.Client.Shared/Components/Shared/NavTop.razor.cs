using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace YoutubeChaineVideos.Client.Shared.Components.Shared
{
    //[Authorize]
    public partial class NavTop : IComponent
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }

        private async Task ToogleSideBar()
        {
            if (JSRuntime != null)
            {
                await JSRuntime!.InvokeVoidAsync("toogleSideBar");
            }
        }
    }
}
