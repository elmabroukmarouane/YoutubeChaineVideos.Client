using Blazored.LocalStorage;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using Radzen;
using Radzen.Blazor;
using MudBlazor;
using CurrieTechnologies.Razor.SweetAlert2;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Responses;
using System.Net;
using YoutubeChaineVideos.Client.Busines.Extensions.Logging;


namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeApiChannel
{
    //[Authorize]
    public partial class YouTubeApiChannelIndex : IComponent, IDisposable
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private string? TitleContent { get; set; }
        [Inject]
        ILocalStorageService? LocalStorageService { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiChannelViewModel>? GenericService { get; set; }
        [Inject]
        TooltipService? TooltipService { get; set; }
        [Inject]
        private SweetAlertService? Swal { get; set; }
        private string TitleSwalTitle { get; set; } = string.Empty;
        private string MessageSwalTitle { get; set; } = string.Empty;
        public static IList<YouTubeApiChannelViewModel>? Items { get; set; }
        public bool IsLoading { get; set; } = false;
        public int Count = 0;
        private RadzenFieldset? RadzenFieldsetUpload { get; set; }
        private RadzenFieldset? RadzenFieldsetDataGrid { get; set; }
        private YouTubeApiChannelViewModel SelectedItem { get; set; } = new();
        private HashSet<YouTubeApiChannelViewModel>? SelectedItems { get; set; }
        private bool IsUpdate { get; set; } = false;
        public string? TableSearchString { get; set; } = string.Empty;
        public string? Token { get; set; }
        [Inject]
        IDialogService? DialogService { get; set; }
        [Inject]
        IYouTubeSourceAppProvider? YouTubeSourceAppProvider { get; set; }
        [Inject]
        protected IGenericService<YouTubeApiAppLogViewModel>? GenericLogService { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        protected override async Task OnInitializedAsync()
        {
            TitleContent = BaseSettingsApp?.BaseTitleApp + " - YouTubeApiChannels";
            Uri = BaseSettingsApp?.BaseUrlApiWebHttp + "Channel";
            //Token = await LocalStorageService!.GetItemAsStringAsync("token");
            await LoadData();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime!.InvokeVoidAsync("initAdminTemplate");
            }
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;
                Items = await GenericService!.GetEntitiesAsync(Uri!, Token);
                Items = Items?.OrderByDescending(x => x.Id).ToList();
                Count = Items != null ? Items.Count : 0;
            }
            catch (Exception ex)
            {
                var log = LoggingMessaging.LoggingMessageError(
                    nameSpaceName: "YoutubeChaineVideos.Client.Shared",
                    statusCodeInt: (int)HttpStatusCode.InternalServerError,
                    statusCode: HttpStatusCode.InternalServerError.ToString(),
                    actionName: "Components.Pages.YouTubeApiChannel - LoadData()",
                    exception: ex
                );
                await GenericLogService!.CreateAsync(BaseSettingsApp?.BaseUrlApiWebHttp + "Log", Token, new YouTubeApiAppLogViewModel()
                {
                    Level = "Error",
                    Message = log,
                    Source = YouTubeSourceAppProvider?.GetSourceApp(),
                });
                throw new Exception(ex.Message, ex);
            }
            finally { IsLoading = false; }
        }

        private bool FilterFunc1(YouTubeApiChannelViewModel item) => GenericService!.FilterFunc(item, TableSearchString);

        private async Task ShowDialogAsync(
            string TitileDialog = "Add",
            YouTubeApiChannelViewModel? YouTubeApiChannelViewModel = null,
            bool isUpdate = false,
            string titleOkButton = "Add")
        {
            try
            {
                var parameters = new DialogParameters<YouTubeApiChannelAddUpdate>()
                {
                    {
                        x => x.YouTubeApiChannelViewModel, YouTubeApiChannelViewModel ?? new YouTubeApiChannelViewModel()
                    },
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.IsUpdate, isUpdate
                    },
                    {
                        x => x.Uri, BaseSettingsApp?.BaseUrlApiWebHttp + "Channel"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                IsUpdate = isUpdate;
                var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService!.ShowAsync<YouTubeApiChannelAddUpdate>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (EntityDbResponse<YouTubeApiChannelViewModel>)result.Data!;
                    if (data != null)
                    {
                        await LoadData();
                        await Swal!.FireAsync(new SweetAlertOptions()
                        {
                            Title = titleOkButton,
                            Text = data.MessageStatus?.Message,
                            Icon = data.MessageStatus?.StatusCode == System.Net.HttpStatusCode.OK ? SweetAlertIcon.Success : SweetAlertIcon.Error
                        });
                    }
                }
                else
                {
                    await Swal!.FireAsync(new SweetAlertOptions()
                    {
                        Title = titleOkButton,
                        Text = "Operation Canceled !",
                        Icon = SweetAlertIcon.Warning
                    });
                }
                if (isUpdate) IsUpdate = false;
            }
            catch (Exception ex)
            {
                var log = LoggingMessaging.LoggingMessageError(
                    nameSpaceName: "YoutubeChaineVideos.Client.Shared",
                    statusCodeInt: (int)HttpStatusCode.InternalServerError,
                    statusCode: HttpStatusCode.InternalServerError.ToString(),
                    actionName: "Components.Pages.YouTubeApiChannel - ShowDialogAsync()",
                    exception: ex
                );
                await GenericLogService!.CreateAsync(BaseSettingsApp?.BaseUrlApiWebHttp + "Log", Token, new YouTubeApiAppLogViewModel()
                {
                    Level = "Error",
                    Message = log,
                    Source = YouTubeSourceAppProvider?.GetSourceApp(),
                });
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task ShowDeleteDialogAsync(
            YouTubeApiChannelViewModel? YouTubeApiChannelViewModel = null,
            string TitileDialog = "Delete",
            string titleOkButton = "Delete")
        {
            try
            {
                var parameters = new DialogParameters<YouTubeApiChannelDelete>()
                {
                    {
                        x => x.ContentMessageDelete, "Are you sure to delete this row ?"
                    },
                    {
                        x => x.YouTubeApiChannelViewModel, YouTubeApiChannelViewModel ?? new YouTubeApiChannelViewModel()
                    },
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.Uri, BaseSettingsApp?.BaseUrlApiWebHttp + "Channel"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                var options = new MudBlazor.DialogOptions() { CloseButton = true, BackdropClick = false };
                var dialog = await DialogService!.ShowAsync<YouTubeApiChannelDelete>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (EntityDbResponse<YouTubeApiChannelViewModel>)result.Data!;
                    if (data != null)
                    {
                        await LoadData();
                        await Swal!.FireAsync(new SweetAlertOptions()
                        {
                            Title = titleOkButton,
                            Text = data.MessageStatus?.Message,
                            Icon = data.MessageStatus?.StatusCode == System.Net.HttpStatusCode.OK ? SweetAlertIcon.Success : SweetAlertIcon.Error
                        });
                    }
                }
                else
                {
                    await Swal!.FireAsync(new SweetAlertOptions()
                    {
                        Title = titleOkButton,
                        Text = "Operation Canceled !",
                        Icon = SweetAlertIcon.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                var log = LoggingMessaging.LoggingMessageError(
                    nameSpaceName: "YoutubeChaineVideos.Client.Shared",
                    statusCodeInt: (int)HttpStatusCode.InternalServerError,
                    statusCode: HttpStatusCode.InternalServerError.ToString(),
                    actionName: "Components.Pages.YouTubeApiChannel - ShowDeleteDialogAsync()",
                    exception: ex
                );
                await GenericLogService!.CreateAsync(BaseSettingsApp?.BaseUrlApiWebHttp + "Log", Token, new YouTubeApiAppLogViewModel()
                {
                    Level = "Error",
                    Message = log,
                    Source = YouTubeSourceAppProvider?.GetSourceApp(),
                });
                throw new Exception(ex.Message, ex);
            }
        }

        void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null, string? message = default) => TooltipService?.Open(elementReference, message ?? string.Empty, options);

        private TooltipOptions CreateTooltipOptions() => new() { Position = TooltipPosition.Top, Style = "background-color: var(--rz-secondary); color: var(--rz-text-contrast-color)" };

        public void Dispose() => GC.SuppressFinalize(TooltipService!);
    }
}
