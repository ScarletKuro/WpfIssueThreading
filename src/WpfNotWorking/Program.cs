using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.Hosting.Wpf.Bootstrap;
using SimpleInjector;
using WpfIssue.Bootstrap;

namespace WpfIssue;

public class Program
{
    public static void Main(string[] args)
    {
        using var container = RootBoot.CreateContainer();
        using IHost host = CreateHostBuilder(container, args)
            .UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
                options.ValidateOnBuild = true;
            }).Build();
        host.UseSimpleInjector(container)
            .UseWpfContainerBootstrap(container)
            .UseWpfViewModelLocator<App, Container>(container);
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(Container container, string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, collection) => ConfigureServices(context, collection, container));
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services, Container container)
    {
        services.AddSimpleInjector(container, options =>
        {
            options.AddLogging();
        });
        services.AddBootstrap<Container, RootBoot>();
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