using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using AKSoftware.Localization.MultiLanguages;
using SMSGateway.Client.Authentication;
using MudBlazor;

namespace SMSGateway.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            builder.Services.AddTransient<AuthorizationMessageHandler>();
            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
            builder.Services.AddMudServices();
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

            
            await builder.Build().RunAsync();
        }
    }
}
