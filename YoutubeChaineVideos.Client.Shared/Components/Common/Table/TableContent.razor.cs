using Microsoft.AspNetCore.Components;

namespace YoutubeChaineVideos.Client.Shared.Components.Common.Table
{
    public partial class TableContent
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
