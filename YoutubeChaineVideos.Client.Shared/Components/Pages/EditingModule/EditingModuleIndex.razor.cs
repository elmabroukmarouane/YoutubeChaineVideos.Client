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
using YoutubeChaineVideos.Client.Domain.Models.LambdaManagement.Models;
using System.Net;
using YoutubeChaineVideos.Client.Busines.Extensions.Logging;


namespace YoutubeChaineVideos.Client.Shared.Components.Pages.EditingModule
{
    //[Authorize]
    public partial class EditingModuleIndex : IComponent, IDisposable
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private string? TitleContent { get; set; }
        [Inject]
        ILocalStorageService? LocalStorageService { get; set; }
        [Inject]
        protected IGenericService<VideoViewModel>? GenericService { get; set; }
        [Inject]
        TooltipService? TooltipService { get; set; }
        [Inject]
        private SweetAlertService? Swal { get; set; }
        private string TitleSwalTitle { get; set; } = string.Empty;
        private string MessageSwalTitle { get; set; } = string.Empty;
        public static IList<VideoViewModel>? Items { get; set; }
        public bool IsLoading { get; set; } = false;
        public int Count = 0;
        private RadzenFieldset? RadzenFieldsetUpload { get; set; }
        private RadzenFieldset? RadzenFieldsetDataGrid { get; set; }
        private VideoViewModel SelectedItem { get; set; } = new();
        private HashSet<VideoViewModel>? SelectedItems { get; set; }
        public string? TableEditingString { get; set; } = string.Empty;
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
            TitleContent = BaseSettingsApp?.BaseTitleApp + " - Editing Youtube Videos";
            Uri = $"{BaseSettingsApp?.BaseUrlApiWebHttp}EditingVideo";
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
                Items = null;
                var filterDataModel = new FilterDataModel()
                {
                    LambdaExpressionModel = new LambdaExpressionModel()
                    {
                        RootGroup = new ConditionGroupModel()
                        {
                            Conditions =
                            [
                                new ConditionModel()
                                {
                                    PropertyName = "IsDownloaded",
                                    ComparisonValue = null,
                                    ComparisonType = "IsTrue"
                                },
                                new ConditionModel()
                                {
                                    PropertyName = "IsEdited",
                                    ComparisonValue = null,
                                    ComparisonType = "IsTrue"
                                },
                                new ConditionModel()
                                {
                                    PropertyName = "IsUploaded",
                                    ComparisonValue = null,
                                    ComparisonType = "IsFalse"
                                }
                            ]
                        }
                    }
                };
                Items = await GenericService!.GetEntitiesAsync($"{Uri!}/filter", Token, filterDataModel);
                Items = Items?.OrderByDescending(x => x.Id).ToList();
                Count = Items?.Count ?? 0;
            }
            catch (Exception ex)
            {
                var log = LoggingMessaging.LoggingMessageError(
                    nameSpaceName: "YoutubeChaineVideos.Client.Shared",
                    statusCodeInt: (int)HttpStatusCode.InternalServerError,
                    statusCode: HttpStatusCode.InternalServerError.ToString(),
                    actionName: "Components.Pages.EditingModule - LoadData()",
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
            finally 
            { 
                IsLoading = false;
            }
        }

        private bool FilterFunc1(VideoViewModel item) => GenericService!.FilterFunc(item, TableEditingString);

        private async Task ShowDialogAsync(
            string TitileDialog = "Editing",
            string titleOkButton = "Editing")
        {
            try
            {
                var parameters = new DialogParameters<EditingModulePage>()
                {
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.Uri, $"{BaseSettingsApp?.BaseUrlApiWebHttp}EditingVideo/EditYouTubeVideos"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                var options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService!.ShowAsync<EditingModulePage>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (EntityDbResponse<VideoViewModel>)result.Data!;
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
                    actionName: "Components.Pages.EditingModule - ShowDialogAsync()",
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

        private async Task ShowClearFoldersDialogAsync(
            string TitileDialog = "Editing",
            string titleOkButton = "Editing")
        {
            try
            {
                var parameters = new DialogParameters<ClearEditingFoldersModulePage>()
                {
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.Uri, $"{BaseSettingsApp?.BaseUrlApiWebHttp}EditingVideo/ClearAndEmptyEditedVideosOutputFolders"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                var options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService!.ShowAsync<ClearEditingFoldersModulePage>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (MessageStatus)result.Data!;
                    if (data != null)
                    {
                        await LoadData();
                        await Swal!.FireAsync(new SweetAlertOptions()
                        {
                            Title = titleOkButton,
                            Text = data.Message,
                            Icon = data.StatusCode == System.Net.HttpStatusCode.OK ? SweetAlertIcon.Success : SweetAlertIcon.Error
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
                    actionName: "Components.Pages.EditingModule - ShowClearFoldersDialogAsync()",
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

        private async Task ShowTruncateTableDialogAsync(
            string TitileDialog = "Editing",
            string titleOkButton = "Editing")
        {
            try
            {
                var parameters = new DialogParameters<ClearTableEditingMobulePage>()
                {
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.Uri, $"{Uri!}/Truncate"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                var options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService!.ShowAsync<ClearTableEditingMobulePage>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (MessageStatus)result.Data!;
                    if (data != null)
                    {
                        await LoadData();
                        await Swal!.FireAsync(new SweetAlertOptions()
                        {
                            Title = titleOkButton,
                            Text = data.Message,
                            Icon = data.StatusCode == System.Net.HttpStatusCode.OK ? SweetAlertIcon.Success : SweetAlertIcon.Error
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
                    actionName: "Components.Pages.EditingModule - ShowTruncateTableDialogAsync()",
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
