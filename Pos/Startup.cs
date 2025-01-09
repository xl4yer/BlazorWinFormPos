using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using MudBlazor;
using Neodynamic.Blazor;

namespace Pos;

public static class Startup
{
    public static IServiceProvider? Services { get; private set; }

    public static void Init()
    {
        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices(WireupServices)
                       .Build();
        Services = host.Services;
    }

    private static void WireupServices(IServiceCollection services)
    {
        services.AddWindowsFormsBlazorWebView();
        services.AddMudServices();
        services.AddBlazoredLocalStorage();
        services.AddScoped<MudThemeProvider>();
        services.AddJSPrintManager();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
    }
}
