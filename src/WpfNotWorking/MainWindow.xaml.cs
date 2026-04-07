using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using WpfIssue.Services;

namespace WpfIssue;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IBackButtonHandler
{
    private bool _startupOverlayHidden;
    private WebView2CompositionControl? _initializedWebView;

    public MainWindow(IServiceProvider serviceProvider, SomeScope someScope)
    {
        InitializeComponent();
        Closed += MainWindowClosed;
        BlazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;
        BlazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
        RootComponent.Parameters = new Dictionary<string, object?>
        {
            { nameof(AppBlazor.SomeScope), someScope },
            { nameof(AppBlazor.BackButtonHandler), this }
        };
        BlazorWebView.Services = serviceProvider;
    }

    private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
    {
        e.EnvironmentOptions = new CoreWebView2EnvironmentOptions("--no-proxy-server")
        {
            AreBrowserExtensionsEnabled = false,
            EnableTrackingPrevention = false,
        };
    }

    private void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
        _initializedWebView = e.WebView;

        if (_initializedWebView.CoreWebView2 is { } coreWebView)
        {
            coreWebView.NavigationCompleted -= HandleInitialNavigationCompleted;
            coreWebView.NavigationCompleted += HandleInitialNavigationCompleted;
            return;
        }

        HideStartupOverlay();
    }

    private void HandleInitialNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (sender is CoreWebView2 coreWebView)
        {
            coreWebView.NavigationCompleted -= HandleInitialNavigationCompleted;
        }

        Dispatcher.Invoke(HideStartupOverlay);
    }

    private void HideStartupOverlay()
    {
        if (_startupOverlayHidden)
        {
            return;
        }

        _startupOverlayHidden = true;

        var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(180))
        {
            FillBehavior = FillBehavior.Stop
        };

        fadeOut.Completed += (_, _) =>
        {
            StartupOverlay.Visibility = Visibility.Collapsed;
            StartupOverlay.Opacity = 1;
        };

        StartupOverlay.BeginAnimation(OpacityProperty, fadeOut);
    }

    private void MainWindowClosed(object? sender, EventArgs e)
    {
        if (_initializedWebView?.CoreWebView2 is { } coreWebView)
        {
            coreWebView.NavigationCompleted -= HandleInitialNavigationCompleted;
        }

        if (BlazorWebView is { } blazorWebView)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            blazorWebView.BlazorWebViewInitializing -= BlazorWebViewInitializing;
            blazorWebView.BlazorWebViewInitialized -= BlazorWebViewInitialized;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        Closed -= MainWindowClosed;
    }

    public Task HandleBackAsync()
    {
        Close();
        return Task.CompletedTask;
    }
}