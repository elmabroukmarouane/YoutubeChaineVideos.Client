using Microsoft.AspNetCore.Components;

namespace YoutubeChaineVideos.Client.Shared.Components.Common.Table.Parts
{
    public partial class TableHeader
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
