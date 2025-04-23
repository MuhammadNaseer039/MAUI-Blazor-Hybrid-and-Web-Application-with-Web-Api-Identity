using LayoutPracticeMAUIWEB.Services;
using LayoutPracticeMAUIWEB.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Components.Authorization;

namespace LayoutPracticeMAUIWEB
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

            // Add device-specific services used by the LayoutPracticeMAUIWEB.Shared project
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddSingleton<IBlogService,BlogService>();
            builder.Services.AddScoped<ITokenStorageService, HybridTokenStorageService>();
            builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddHttpClient<IAuthService,AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7202/api/");
            });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
