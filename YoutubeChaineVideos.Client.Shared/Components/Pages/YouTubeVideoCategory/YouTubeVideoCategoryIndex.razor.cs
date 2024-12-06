using Blazored.LocalStorage;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YoutubeChaineVideos.Client.Shared.Components.Extensions.LocalStorage;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using Radzen;
using Radzen.Blazor;
using MudBlazor;
using static MudBlazor.CategoryTypes;
using CurrieTechnologies.Razor.SweetAlert2;
using static System.Runtime.InteropServices.JavaScript.JSType;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Responses;

namespace YoutubeChaineVideos.Client.Shared.Components.Pages.YouTubeVideoCategory
{
    //[Authorize]
    public partial class YouTubeVideoCategoryIndex : IComponent, IDisposable
    {
        //[Inject]
        //ILocalStorageService? LocalStorageService { get; set; }
        //[Inject]
        //protected IGenericService<YouTubeVideoCategoryViewModel>? GenericService { get; set; }
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        //public static IList<YouTubeVideoCategoryViewModel>? InitialYouTubeVideoCategorys { get; set; }
        //public static IList<YouTubeVideoCategoryViewModel>? YouTubeVideoCategorys { get; set; }
        [Inject]
        private BaseSettingsApp? BaseSettingsApp { get; set; }
        private string? TitleContent { get; set; }
        [Inject]
        ILocalStorageService? LocalStorageService { get; set; }
        [Inject]
        protected IGenericService<YouTubeVideoCategoryViewModel>? GenericService { get; set; }
        [Inject]
        TooltipService? TooltipService { get; set; }
        [Inject]
        private SweetAlertService? Swal { get; set; }
        private string TitleSwalTitle { get; set; } = string.Empty;
        private string MessageSwalTitle { get; set; } = string.Empty;
        //private SweetAlertIcon? SweetAlertIcon { get; set; }
        //public static IList<YouTubeVideoCategoryViewModel>? InitialItems { get; set; }
        public static IList<YouTubeVideoCategoryViewModel>? Items { get; set; }
        //public RadzenDataGrid<YouTubeVideoCategoryViewModel>? YouTubeVideoCategoryGrid { get; set; }
        public bool IsLoading { get; set; } = false;
        public int Count = 0;
        //private IEnumerable<int> PageSizeOptions = new int[] { 10, 25, 50, 100 };
        //private string PagingSummaryFormat = "Displaying page {0} of {1} (total {2} records)";
        //private Modal? ModalAddUpdate { get; set; }
        private RadzenFieldset? RadzenFieldsetUpload { get; set; }
        private RadzenFieldset? RadzenFieldsetDataGrid { get; set; }
        private YouTubeVideoCategoryViewModel SelectedItem { get; set; } = new();
        private HashSet<YouTubeVideoCategoryViewModel>? SelectedItems { get; set; }
        //private bool IsBusy { get; set; } = false;
        //private string TitleModal { get; set; } = string.Empty;
        //private string SubmitButtonModal { get; set; } = string.Empty;
        private bool IsUpdate { get; set; } = false;
        //private YouTubeVideoCategoryViewModel? YouTubeVideoCategoryToInsert { get; set; } = new();
        //private YouTubeVideoCategoryViewModel? YouTubeVideoCategoryToUpdate { get; set; } = new();
        public string? TableSearchString { get; set; } = string.Empty;
        public string? Token { get; set; }
        [Inject]
        IDialogService? DialogService { get; set; }
        [Parameter]
        public string? Uri { get; set; }
        protected override async Task OnInitializedAsync()
        {
            TitleContent = BaseSettingsApp?.BaseTitleApp + " - Videos Categories";
            Uri = BaseSettingsApp?.BaseUrlApiWebHttp + "VideoCategory";
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
                //InitialItems = Items;
                Count = Items != null ? Items.Count : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally { IsLoading = false; }
        }

        private bool FilterFunc1(YouTubeVideoCategoryViewModel item) => FilterFunc(item, TableSearchString);

        private bool FilterFunc(YouTubeVideoCategoryViewModel item, string? tableSearchString)
        {
            if (string.IsNullOrWhiteSpace(tableSearchString))
                return true;
            if (item!.CategoryName!.Contains(tableSearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (item!.CreatedBy!.Contains(tableSearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (item!.CreateDate!.ToString()!.Contains(tableSearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (item!.UpdatedBy!.Contains(tableSearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (item!.UpdateDate!.ToString()!.Contains(tableSearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private async Task ShowDialogAsync(
            string TitileDialog = "Add",
            YouTubeVideoCategoryViewModel? YouTubeVideoCategoryViewModel = null,
            bool isUpdate = false,
            string titleOkButton = "Add")
        {
            try
            {
                var parameters = new DialogParameters<YouTubeVideoCategoryAddUpdate>()
                {
                    {
                        x => x.YouTubeVideoCategoryViewModel, YouTubeVideoCategoryViewModel ?? new YouTubeVideoCategoryViewModel()
                    },
                    {
                        x => x.TitleOkButton, titleOkButton
                    },
                    {
                        x => x.IsUpdate, isUpdate
                    },
                    {
                        x => x.Uri, BaseSettingsApp?.BaseUrlApiWebHttp + "VideoCategory"
                    },
                    {
                        x => x.Token, Token
                    }
                };
                IsUpdate = isUpdate;
                var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
                var dialog = await DialogService!.ShowAsync<YouTubeVideoCategoryAddUpdate>(TitileDialog, parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var data = (EntityDbResponse<YouTubeVideoCategoryViewModel>)result.Data!;
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
                throw new Exception(ex.Message, ex);
            }
        }

        void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null, string? message = default)
        {
            TooltipService?.Open(elementReference, message ?? string.Empty, options);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(TooltipService!);
        }
    }
}
