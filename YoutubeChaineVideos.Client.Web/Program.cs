using Blazored.LocalStorage;
using YoutubeChaineVideos.Client.Busines.Services.Class;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Radzen;
using CurrieTechnologies.Razor.SweetAlert2;
using YoutubeChaineVideos.Client.Domain.Models.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddRadzenComponents();
builder.Services.AddMudServices();
builder.Services.AddSweetAlert2(options =>
{
    options.Theme = SweetAlertTheme.Dark;
    options.SetThemeForColorSchemePreference(ColorScheme.Light, SweetAlertTheme.Default);
    options.SetThemeForColorSchemePreference(ColorScheme.Dark, SweetAlertTheme.Dark);
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

try
{
	var appSettings = builder.Configuration.GetSection("BaseSettingsApp").Get<BaseSettingsApp>() ?? new()
	{
		BaseTitleApp = string.Empty,
		BaseUrlApiAndroidHttp = string.Empty,
		BaseUrlApiWebHttp = string.Empty,
		ChosenEnviroment = string.Empty
	};
	builder.Services.AddSingleton(appSettings);
}
catch (Exception ex)
{
	throw new Exception(ex.Message, ex);
}

builder.Services.AddTransient<IGenericService<YouTubeVideoCategoryViewModel>, GenericService<YouTubeVideoCategoryViewModel>>();
builder.Services.AddTransient<IGenericService<YouTubeApiRemainingQuotaViewModel>, GenericService<YouTubeApiRemainingQuotaViewModel>>();
builder.Services.AddTransient<IGenericService<YouTubeApiSearchQueryViewModel>, GenericService<YouTubeApiSearchQueryViewModel>>();
builder.Services.AddTransient<IGenericService<YouTubeUploadVideoCredentialViewModel>, GenericService<YouTubeUploadVideoCredentialViewModel>>();
builder.Services.AddTransient<IGenericService<YouTubeApiChannelViewModel>, GenericService<YouTubeApiChannelViewModel>>();
builder.Services.AddTransient<IGenericService<YouTubeApiConfigViewModel>, GenericService<YouTubeApiConfigViewModel>>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddTransient<ITitleService, TitleService>();

await builder.Build().RunAsync();
