using Microsoft.AspNetCore.Components;

namespace YoutubeChaineVideos.Client.Shared.Components.Common
{
    public partial class DescriptionCardContent
    {
        [Parameter] 
        public RenderFragment? ChildContent { get; set; }
    }
}
