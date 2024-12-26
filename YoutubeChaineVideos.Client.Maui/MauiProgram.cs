using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Radzen;
using System.Reflection;
using YoutubeChaineVideos.Client.Busines.Services.Class;
using YoutubeChaineVideos.Client.Busines.Services.Interface;
using YoutubeChaineVideos.Client.Domain.Models;
using YoutubeChaineVideos.Client.Domain.Models.Settings;
using YoutubeChaineVideos.Client.Shared.Services.Classes;


namespace YoutubeChaineVideos.Client.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddRadzenComponents();
            builder.Services.AddMudServices();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSweetAlert2(options =>
            {
                options.Theme = SweetAlertTheme.Dark;
                options.SetThemeForColorSchemePreference(ColorScheme.Light, SweetAlertTheme.Default);
                options.SetThemeForColorSchemePreference(ColorScheme.Dark, SweetAlertTheme.Dark);
            });
            builder.Services.AddScoped(sp => new HttpClient { });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            //Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#endif
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                var resourceName = environment == "Development" ? "YoutubeChaineVideos.Client.Maui.wwwroot.appsettings.development.json" : "YoutubeChaineVideos.Client.Maui.wwwroot.appsettings.json";
                using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"The configuration file '{resourceName}' was not found as an embedded resource.");
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonStream(stream);
                var configuration = configurationBuilder.Build();
                var appSettings = configuration.GetSection("BaseSettingsApp").Get<BaseSettingsApp>() ?? new BaseSettingsApp
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
            builder.Services.AddTransient<IGenericService<VideoViewModel>, GenericService<VideoViewModel>>();
            builder.Services.AddTransient<IGenericService<YouTubeApiAppLogViewModel>, GenericService<YouTubeApiAppLogViewModel>>();
            builder.Services.AddTransient<IVideoService, VideoService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddTransient<ITitleService, TitleService>();

            // Register a base path or URL for your application
            builder.Services.AddSingleton<IYouTubeSourceAppProvider>(new YouTubeSourceAppProvider("Android"));

            return builder.Build();
        }
    }
}
