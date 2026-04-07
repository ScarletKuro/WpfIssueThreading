using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Wpf;

namespace WpfIssue;

public class Program
{
    public static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args)
            .UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
                options.ValidateOnBuild = true;
            }).Build();
        host.UseWpfViewModelLocator<App, IServiceProvider>(host.Services);
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddWpf<App>();
        services.AddBlazorWebViewDeveloperTools();
        services.AddScoped(sp =>
        {
            var uriHelper = sp.GetRequiredService<NavigationManager>();
            var client = new HttpClient { BaseAddress = new Uri(uriHelper.BaseUri) };
            return client;
        });
        services.AddWpfBlazorWebView();
    }
}