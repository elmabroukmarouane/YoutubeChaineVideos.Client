using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace YoutubeChaineVideos.Client.Shared.Components.Common
{
    public partial class CodeSnippet
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
