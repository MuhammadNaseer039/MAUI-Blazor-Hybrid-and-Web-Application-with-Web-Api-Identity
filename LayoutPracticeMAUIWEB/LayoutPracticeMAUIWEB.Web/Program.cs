using LayoutPracticeMAUIWEB.Shared.Services;
using LayoutPracticeMAUIWEB.Web.Components;
using LayoutPracticeMAUIWEB.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace LayoutPracticeMAUIWEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Add device-specific services used by the LayoutPracticeMAUIWEB.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddSingleton<IBlogService,BlogService>();
            builder.Services.AddScoped<ITokenStorageService , TokenStorageService>();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
            
                sp.GetRequiredService<CustomAuthStateProvider>()
            );
            builder.Services.AddAuthorizationCore();
            builder.Services.AddHttpClient<IAuthService,AuthService>(options =>
            {
                options.BaseAddress = new Uri("https://localhost:7202/api/");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(typeof(LayoutPracticeMAUIWEB.Shared._Imports).Assembly);

            app.Run();
        }
    }
}
